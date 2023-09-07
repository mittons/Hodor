using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Contracts.Dto;
using Contracts.DtoContracts;
using Contracts.MediatorPattern;
using Contracts.ObserverPattern;
using HodorData;
using ViewNavigation.Dto;

namespace ViewNavigation
{
    public class ViewController : IViewController
    {

        private IHodorTaskService _taskService = TaskServiceFactory.GetTaskServiceInstance();

        public ViewController()
        {
            //todo dispose?
            _taskService.AddTaskServiceDataSetChangedObserver(this);

            _viewTaskFocus = new ViewTaskFocus()
            {
                FocusedTaskId = HodorTask.PROJECT_TREE_ROOT_PARENT_ID,
                FocusedForestId = 0
            };

            InitLocalDataCache();
        }


        #region ICurrentViewDataObservable
        private List<ICurrentViewDataObserver> _currentDisplayDataObservers = new List<ICurrentViewDataObserver>();
        private ViewTaskFocus _viewTaskFocus;
        private bool _inBatchEditMode = false;
        private bool _batchEditModeViewDataSetModified = false;
        private bool _batchEditModeViewTaskFocusModified = false;

        public IViewTaskFocus GetViewTaskFocus()
        {
            return _viewTaskFocus;
        }

        //make sure it is in focused forest, throw error otherwise
        public void SetFocusedTaskGivenFocusedForest(int focusedTaskId, object sender)
        {
            _viewTaskFocus.FocusedTaskId = focusedTaskId;
            NotifyViewTaskFocusChanged(sender);
        }

        public void SetFocusedTaskAndForest(int focusedTaskIdInForest, int focusedProjectForestId, object sender)
        {
            _viewTaskFocus.FocusedTaskId = focusedTaskIdInForest;
            _viewTaskFocus.FocusedForestId = focusedProjectForestId;
            NotifyViewTaskFocusChanged(sender);
        }

        //todo return cloned instances.
        public IEnumerable<IProjectForest> GetDisplayedprojectForests()
        {
            return _currentlyDisplayedProjectForests;
        }

        public void NotifyViewTaskFocusChanged(object sender)
        {
            if (_inBatchEditMode)
            {
                _batchEditModeViewTaskFocusModified = true;
                return;
            }

            foreach (var currentDisplayDataObserver in _currentDisplayDataObservers)
            {
                currentDisplayDataObserver.OnViewTaskFocusChanged(sender);
            }
        }

        public void NotifyViewDataSetChanged(object sender)
        {
            if (_inBatchEditMode)
            {
                _batchEditModeViewDataSetModified = true;
                return;
            }
            foreach (var currentDisplayDataObserver in _currentDisplayDataObservers)
            {
                currentDisplayDataObserver.OnViewDataSetChanged(sender);
            }
        }

        public void AddCurrentViewDataObserver(ICurrentViewDataObserver observer)
        {
            if (!_currentDisplayDataObservers.Contains(observer))
            {
                _currentDisplayDataObservers.Add(observer);
            }
        }

        public void RemoveViewDisplayDataObserver(ICurrentViewDataObserver observer)
        {
            if (_currentDisplayDataObservers.Contains(observer))
            {
                _currentDisplayDataObservers.Remove(observer);
            }
        }


        public void StartBatchDataSetUpdate()
        {
            _inBatchEditMode = true;
            _batchEditModeViewDataSetModified = false;
            _batchEditModeViewTaskFocusModified = false;

            _taskService.StartBatchDataSetUpdate();
        }

        public void FinishBatchDataSetUpdate(object sender)
        {
            bool shouldSendNotifyViewTaskFocusChanged = _batchEditModeViewTaskFocusModified;
            bool shouldSendNotifyViewDataSetChanged = _batchEditModeViewDataSetModified;
            _inBatchEditMode = false;

            if (shouldSendNotifyViewTaskFocusChanged)
            {
                NotifyViewTaskFocusChanged(sender);
            }

            if (shouldSendNotifyViewDataSetChanged)
            {
                NotifyViewDataSetChanged(sender);
            }

            _taskService.FinishBatchDataSetUpdate(sender);
        }

        #endregion

        #region ITextEditorDisplayMediator

        private List<ITextEditorDisplay> _textEditorDisplays = new List<ITextEditorDisplay>();

        public void AddTextEditorDisplay(ITextEditorDisplay display)
        {
            if (!_textEditorDisplays.Contains(display))
            {
                _textEditorDisplays.Add(display);
            }
        }

        public void RemoveTextEditorDisplay(ITextEditorDisplay display)
        {
            if (_textEditorDisplays.Contains(display))
            {
                _textEditorDisplays.Remove(display);
            }
        }

        public void DisplayTaskDetailsInTextEditor(int taskId)
        {
            foreach (var textEditorDisplay in _textEditorDisplays)
            {
                textEditorDisplay.DisplayTaskDetails(taskId);
            }
        }

        public void DisplayProjectForestInTextEditor(int projectForestId)
        {
            foreach (var textEditorDisplay in _textEditorDisplays)
            {
                textEditorDisplay.DisplayProjectForest(projectForestId);
            }
        }

        #endregion

        #region ITaskServiceDataSetObserver

        public void OnTaskServiceDataSetChanged(object sender)
        {
            //todo: look if we want to do something with this call back
            //NotifyViewDataSetChanged(sender);
        }

        #endregion

        #region DataOperations

        private List<HodorTask> _tasks = new List<HodorTask>();
        private List<IProjectForest> _currentlyDisplayedProjectForests = new List<IProjectForest>();
        private int _currentSessionProjectForestId;

        #region Init

        private void InitLocalDataCache()
        {
            _currentlyDisplayedProjectForests = _taskService.GetCurrentlyDisplayedProjectForests();
            foreach (var forest in _currentlyDisplayedProjectForests)
            {
                if (forest.Type == ProjectForestType.SessionProjectForest)
                {
                    _currentSessionProjectForestId = forest.Id;
                }
                foreach (var projectTreeRootId in forest.ProjectTreeRootIds)
                {
                    InitLocalDataCacheRecursive(projectTreeRootId);
                }
            }
        }

        private void InitLocalDataCacheRecursive(int taskId)
        {
            var task = _taskService.GetTask(taskId);
            if (_tasks.All(x => x.Id != taskId))
            {
                _tasks.Add(task);
                var children = _taskService.GetChildren(taskId);
                foreach (var child in children)
                {
                    InitLocalDataCacheRecursive(child.Id);
                }
            }
        }

        #endregion

        #region Create

        public HodorTask CreateTask(string title, int parentId, HodorTaskStatus status, object sender)
        {
            var task = _taskService.CreateTask(title, parentId, status, sender);
            _tasks.Add(task);
            AfterCreateTask(task, sender);
            NotifyViewDataSetChanged(sender);
            return task.Clone();
        }

        public HodorTask CreateTask(string title, int parentId, HodorTaskStatus status, int ordinal, object sender)
        {
            var task = _taskService.CreateTask(title, parentId, status, ordinal, sender);
            _tasks.Add(task);
            AfterCreateTask(task, sender);
            NotifyViewDataSetChanged(sender);
            return task.Clone();
        }

        //Returns the root task of the new project tree
        public HodorTask CreateProjectTree(string rootTaskTitle, HodorTaskStatus rootTaskStatus, object sender)
        {
            var task = _taskService.CreateProjectTree(rootTaskTitle, rootTaskStatus, sender);
            _tasks.Add(task);
            foreach (var fullProjectForest in _currentlyDisplayedProjectForests.Where(x => x.Type == ProjectForestType.FullProjectForest).Where(fullProjectForest => !fullProjectForest.ProjectTreeRootIds.Contains(task.Id)))
            {
                fullProjectForest.ProjectTreeRootIds.Add(task.Id);
            }
            //todo add call to AfterCreateTask? none of the current logic applies to this case (this task is the only one in its tree, the logic currently only deals with updating ancestors in the tree (and/or their decendants))
            NotifyViewDataSetChanged(sender);
            return task.Clone();
        }

        //Returns the root task of the new project tree
        public HodorTask CreateProjectTree(string rootTaskTitle, HodorTaskStatus rootTaskStatus, int projectTreeOrdinal, object sender)
        {
            var task = _taskService.CreateProjectTree(rootTaskTitle, rootTaskStatus, projectTreeOrdinal, sender);
            _tasks.Add(task);
            foreach (var fullProjectForest in _currentlyDisplayedProjectForests.Where(x => x.Type == ProjectForestType.FullProjectForest).Where(fullProjectForest => !fullProjectForest.ProjectTreeRootIds.Contains(task.Id)))
            {
                fullProjectForest.ProjectTreeRootIds.Add(task.Id);
            }
            //todo add call to AfterCreateTask? none of the current logic applies to this case (this task is the only one in its tree, the logic currently only deals with updating ancestors in the tree (and/or their decendants))
            NotifyViewDataSetChanged(sender);
            return task.Clone();
        }

        #endregion

        #region Read

        public HodorTask GetTask(int taskId)
        {
            var task = GetTask_Internal(taskId);
            return task != null ? task.Clone() : null;
        }

        private HodorTask GetTask_Internal(int taskId)
        {
            return _tasks.SingleOrDefault(x => x.Id == taskId);
        }

        public List<HodorTask> GetChildren(int taskId)
        {
            return _tasks.Where(x => x.ParentId == taskId).Select(x => x.Clone()).OrderBy(x => x.Ordinal).ToList();
        }

        private List<HodorTask> GetChildren_Internal(int taskId)
        {
            return _tasks.Where(x => x.ParentId == taskId).OrderBy(x => x.Ordinal).ToList();
        }

        //Todo return cloned instance
        //Todo: Doesn't check if project forest with given id exists in task service/or here
        public IProjectForest GetProjectForest(int projectForestId)
        {
            return _currentlyDisplayedProjectForests.SingleOrDefault(x => x.Id == projectForestId);
        }

        //todo return cloned object
        //todo doesnt make sure that there is a current session.
        public ISessionProjectForest GetCurrentSessionProjectForest()
        {
            return _currentlyDisplayedProjectForests.OfType<ISessionProjectForest>().SingleOrDefault(x => x.Id == _currentSessionProjectForestId);
        }

        #endregion

        #region Update

        public void UpdateTask(int taskId, object sender, string updatedTitle = null, int? updatedTaskOrdinal = null, HodorTaskStatus? updatedStatus = null)
        {
            UpdateTask_Internal(taskId, sender, true, updatedTitle, updatedTaskOrdinal, updatedStatus);
        }


        private void UpdateTask_Internal(int taskId, object sender, bool triggerNotify, string updatedTitle = null, int? updatedTaskOrdinal = null, HodorTaskStatus? updatedStatus = null)
        {
            var oldTask = GetTask_Internal(taskId);
            var oldTaskStatus = oldTask.CurrentTaskStatus;
            var isTaskStatusChanged = (updatedStatus != null && oldTaskStatus != updatedStatus);

            var updated = _taskService.UpdateTask(taskId, updatedTitle, updatedStatus, updatedTaskOrdinal, sender);
            if (updated)
            {
                _tasks.RemoveAll(x => x.Id == taskId);
                var updatedTask = _taskService.GetTask(taskId);
                _tasks.Add(updatedTask);

                if (isTaskStatusChanged)
                {
                    AfterTaskStatusChanged(updatedTask, oldTaskStatus, sender);
                }
                if (triggerNotify)
                {
                    NotifyViewDataSetChanged(sender);
                }
            }
        }



        //Todo: Doesn't check if project forest with given id exists in task service/or here
        public void AddProjectTreeToProjectForest(int projectTreeRootTaskId, int projectForestId, object sender)
        {
            var projTreeRootTask = GetTask_Internal(projectTreeRootTaskId);
            if (projTreeRootTask.ParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
            {
                throw new SystemException("VC: Trying to add a non-root task as a project tree root task to a project forest");
            }

            _taskService.AddProjectTreeToProjectForest(projectTreeRootTaskId, projectForestId, sender);

            var projectForest = _currentlyDisplayedProjectForests.SingleOrDefault(x => x.Id == projectForestId);
            if (projectForest != null)
            {
                if (!projectForest.ProjectTreeRootIds.Contains(projectTreeRootTaskId))
                {
                    projectForest.ProjectTreeRootIds.Add(projectTreeRootTaskId);
                }
                var forest = projectForest as ISessionProjectForest;
                if (forest != null)
                {
                    forest.ProjectTreeTaskCompletionGoals.Add(projectTreeRootTaskId, 0);
                }
            }
            NotifyViewDataSetChanged(sender);
        }


        //Todo: Doesn't check if project forest with given id exists in task service/or here
        public void UpdateTaskCompletionGoalForTreeInSession(int sessionProjectForestId, int projectTreeRootId, int updatedTaskCompletionGoal)
        {
            _taskService.UpdateTaskCompletionGoalForTreeInSession(sessionProjectForestId, projectTreeRootId, updatedTaskCompletionGoal);
            
            var session = _currentlyDisplayedProjectForests.OfType<ISessionProjectForest>().SingleOrDefault(x => x.Id == sessionProjectForestId);
            if (session != null)
            {
                if (!session.ProjectTreeTaskCompletionGoals.ContainsKey(projectTreeRootId))
                {
                    throw new SystemException("TS: Tried to set task completion goal for a project tree that isnt part of the session specified");
                }
                if (updatedTaskCompletionGoal < 0)
                {
                    throw new SystemException("TS: Task completion goal for a project tree in a session must be non-negative");
                }
                session.ProjectTreeTaskCompletionGoals[projectTreeRootId] = updatedTaskCompletionGoal;
            }
        }

        #endregion

        #region Delete

        //todo done2
        public void DeleteTaskAndSubTasks(int taskId, object sender)
        {
            if (_viewTaskFocus.FocusedTaskId != -1)
            {
                var fTaskId = _viewTaskFocus.FocusedTaskId;
                while (fTaskId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
                {
                    var fTask = GetTask_Internal(fTaskId);
                    if (taskId == fTaskId)
                    {
                        _viewTaskFocus.FocusedTaskId = fTask.ParentId;
                        NotifyViewTaskFocusChanged(sender);
                        break;
                    }
                    fTaskId = fTask.ParentId;
                }
            }
            var task = GetTask_Internal(taskId);
            
            _taskService.DeleteTaskAndSubTasks(taskId, sender);

            DeleteTaskAndSubTasks_RecursiveHelper(taskId);

            var siblingTasks = _tasks.Where(x => x.ParentId == task.ParentId).ToList();
            foreach (var siblingTask in siblingTasks)
            {
                siblingTask.Ordinal = _taskService.GetTask(siblingTask.Id).Ordinal;
            }

            if (task.ParentId == HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
            {
                foreach (var forest in _currentlyDisplayedProjectForests.Where(forest => forest.ProjectTreeRootIds.Contains(task.Id)))
                {
                    forest.ProjectTreeRootIds.Remove(task.Id);
                }
            }
            
            AfterDeleteTask(task, sender);

            NotifyViewDataSetChanged(sender);
        }

        private void DeleteTaskAndSubTasks_RecursiveHelper(int taskId)
        {
            var childTaskIds = _tasks.Where(x => x.ParentId == taskId).Select(y => y.Id).ToList();
            foreach (var childTaskId in childTaskIds)
            {
                DeleteTaskAndSubTasks_RecursiveHelper(childTaskId);
            }
            _tasks.RemoveAll(x => x.Id == taskId);
        }

        #endregion




        #endregion

        #region BuisnessLogic


        //todo remove this and circumvent the UpdateTask_Internal function when this is true
        private bool _applyingCompletedStatusToSubtreeInternally = false;

        private void AfterCreateTask(HodorTask createdTask, object sender)
        {
            PerformCompletedStatusLogicAfterCreateTask(createdTask, sender);
            PerformImpendingStatusLogicAfterCreateTask(createdTask, sender);
        }

        private void AfterDeleteTask(HodorTask deletedTask, object sender)
        {
            PerformImpendingLogicAfterDeleteTask(deletedTask, sender);
        }

        private void AfterTaskStatusChanged(HodorTask updatedTask, HodorTaskStatus oldTaskStatus, object sender)
        {
            if (!_applyingCompletedStatusToSubtreeInternally)
            {
                PerformCompletedStatusLogicAfterTaskStatusChanged(updatedTask, oldTaskStatus, sender);
                PerformImpendingStatusLogicAfterTaskStatusChanged(updatedTask, oldTaskStatus, sender);
            }
        }

        #region CompletedTask Logic

        /// <summary>
        /// If the following holds for every node before the TaskCreate then this function should ensure it also holds true after the TaskCreate:
        ///     If a node has CurrentTaskStatus == Completed then either of the following is true:
        ///         The node is a leaf node
        ///         Every decendant of the node has CurrentTaskStatus == Completed
        /// </summary>
        /// <param name="createdTask"></param>
        /// <param name="sender"></param>
        private void PerformCompletedStatusLogicAfterCreateTask(HodorTask createdTask, object sender)
        {
            if ((createdTask.CurrentTaskStatus != HodorTaskStatus.Completed) &&
                createdTask.ParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
            {
                var parent = GetTask(createdTask.ParentId);
                if (parent.CurrentTaskStatus == HodorTaskStatus.Completed)
                {
                    UpdateTask_Internal(parent.Id, sender, false, updatedStatus: HodorTaskStatus.Todo);
                }
            }
        }

        /// <summary>
        /// If the following holds for every node before the CurrentTaskStatus change then this function should ensure it also holds true after the CurrentTaskStatus change:
        ///     If a node has CurrentTaskStatus == Completed then either of the following is true:
        ///         The node is a leaf node
        ///         Every decendant of the node has CurrentTaskStatus == Completed 
        /// </summary>
        /// <param name="updatedTask"></param>
        /// <param name="oldTaskStatus"></param>
        /// <param name="sender"></param>
        private void PerformCompletedStatusLogicAfterTaskStatusChanged(HodorTask updatedTask, HodorTaskStatus oldTaskStatus, object sender)
        {
            if (oldTaskStatus == HodorTaskStatus.Completed &&
                updatedTask.ParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
            {
                var parent = GetTask(updatedTask.ParentId);
                if (parent.CurrentTaskStatus == HodorTaskStatus.Completed)
                {
                    UpdateTask_Internal(parent.Id, sender, false, updatedStatus: HodorTaskStatus.Todo);
                }
            }
            else if (updatedTask.CurrentTaskStatus == HodorTaskStatus.Completed)
            {
                var childTasks = GetChildren_Internal(updatedTask.Id).Where(x => x.CurrentTaskStatus != HodorTaskStatus.Completed).ToList();
                _applyingCompletedStatusToSubtreeInternally = true;
                ApplyCompletedStatusToAllDecentantTasks(childTasks, sender);
                _applyingCompletedStatusToSubtreeInternally = false;
            }
        }

        private void ApplyCompletedStatusToAllDecentantTasks(List<HodorTask> childTasks, object sender)
        {
            foreach (var childTask in childTasks)
            {
                var grandChildTasks = GetChildren_Internal(childTask.Id).Where(x => x.CurrentTaskStatus != HodorTaskStatus.Completed).ToList();
                ApplyCompletedStatusToAllDecentantTasks(grandChildTasks, sender);
                if (childTask.CurrentTaskStatus != HodorTaskStatus.Completed)
                {
                    UpdateTask_Internal(childTask.Id, sender, false, updatedStatus: HodorTaskStatus.Completed);
                }
            }

        }

        #endregion

        #region ImpendingTask Logic

        /// <summary>
        /// If the following holds for every node before the TaskCreate then this function should ensure it also holds true after the TaskCreate:
        ///     If a node has CurrentTaskStatus == Impending then one of the following is true: 
        ///         It is a leaf node
        ///         None of its decendants have CurrentTaskStatus != Todoo
        ///         One or more of its decendants have CurrentTaskStatus == Todoo and has atleast one decendant with CurrentTaskStatus == Impending (where one of the two previous arguments hold at that decendant).
        /// </summary>
        /// <param name="createdTask"></param>
        /// <param name="sender"></param>
        private void PerformImpendingStatusLogicAfterCreateTask(HodorTask createdTask, object sender)
        {
            if (createdTask.CurrentTaskStatus == HodorTaskStatus.Todo)
            {
                if (createdTask.ParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
                {
                    var parent = GetTask(createdTask.ParentId);
                    if (parent.CurrentTaskStatus == HodorTaskStatus.Impending)
                    {
                        var siblings = GetChildren_Internal(parent.Id);
                        if (siblings.TrueForAll(x => x.Id == createdTask.Id || x.CurrentTaskStatus == HodorTaskStatus.Completed))
                        {
                            UpdateTask_Internal(createdTask.Id, sender, false, updatedStatus: HodorTaskStatus.Impending);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If the following holds for every node before the TaskDelete then this function should ensure it also holds true after the TaskDelete:
        ///     If a node has CurrentTaskStatus == Impending then one of the following is true: 
        ///         It is a leaf node
        ///         None of its decendants have CurrentTaskStatus != Todoo
        ///         One or more of its decendants have CurrentTaskStatus == Todoo and has atleast one decendant with CurrentTaskStatus == Impending (where one of the two previous arguments hold at that decendant).
        /// </summary>
        /// <param name="deletedTask"></param>
        /// <param name="sender"></param>
        private void PerformImpendingLogicAfterDeleteTask(HodorTask deletedTask, object sender)
        {
            var nextParentId = deletedTask.ParentId;
            while (nextParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
            {
                var currTask = GetTask_Internal(nextParentId);
                if (currTask.CurrentTaskStatus == HodorTaskStatus.Impending)
                {
                    var children = GetChildren_Internal(currTask.Id);
                    if (children.TrueForAll(child => !SubTreeHasImpendingTaskNode(child)) &&
                        children.Exists(SubTreeHasTodoTaskNode))
                    {
                        ApplyImpendingToFirstTodoNodeHitInDfsThatHasNoTodoDecendants(currTask, sender);
                    }
                    break;
                }
                nextParentId = currTask.ParentId;
            }
        }

        /// <summary>
        /// If the following holds for every node before the CurrentTaskStatus change then this function should ensure it also holds true after the CurrentTaskStatus change:
        ///     If a node has CurrentTaskStatus == Impending then one of the following is true: 
        ///         It is a leaf node
        ///         None of its decendants have CurrentTaskStatus != Todoo
        ///         One or more of its decendants have CurrentTaskStatus == Todoo and has atleast one decendant with CurrentTaskStatus == Impending (where one of the two previous arguments hold at that decendant).
        /// If this
        /// </summary>
        /// <param name="updatedTask"></param>
        /// <param name="oldTaskStatus"></param>
        /// <param name="sender"></param>
        private void PerformImpendingStatusLogicAfterTaskStatusChanged(HodorTask updatedTask, HodorTaskStatus oldTaskStatus, object sender)
        {
            //if new value of CurrentTaskStatus is Impending do for updated task / or If old value of CurrentTaskStatus is Impending and the updated task has an ancestor with Impending do for the ancestor:
                //if subtree has no ImpendingStatus task nodes and if subtree has atleast one TodoStatus task node then
                    //find the first decendant in dfs order that has TodoStatus and has no TodoStatus task decendants (and by the above conditional check no ImpendingStatus either)
                        //set that decendant to ImpendingStatus
            var tryApplyImpendingToDecendantIfNeeded = false;
            HodorTask taskToUse = null;

            if (updatedTask.CurrentTaskStatus == HodorTaskStatus.Impending)
            {
                tryApplyImpendingToDecendantIfNeeded = true;
                taskToUse = updatedTask;
            }
            else if (oldTaskStatus == HodorTaskStatus.Impending)
            {
                var nextParentId = updatedTask.ParentId;
                while (nextParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
                {
                    var currTask = GetTask_Internal(nextParentId);
                    if (currTask.CurrentTaskStatus == HodorTaskStatus.Impending)
                    {
                        tryApplyImpendingToDecendantIfNeeded = true;
                        taskToUse = currTask;
                        break;
                    }
                    nextParentId = currTask.ParentId;
                }
            }
            //old status was not impending nor todoo, i.e. it was completed
            else if (updatedTask.CurrentTaskStatus == HodorTaskStatus.Todo)
            {
                if (updatedTask.ParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
                {
                    var parent = GetTask(updatedTask.ParentId);
                    if (parent.CurrentTaskStatus == HodorTaskStatus.Impending)
                    {
                        var siblings = GetChildren_Internal(parent.Id);
                        if (siblings.TrueForAll(x => x.Id == updatedTask.Id || x.CurrentTaskStatus == HodorTaskStatus.Completed))
                        {
                            ApplyImpendingToFirstTodoNodeHitInDfsThatHasNoTodoDecendants(parent, sender);
                        }
                    }
                }
            }

            if (tryApplyImpendingToDecendantIfNeeded)
            {
                var children = GetChildren_Internal(taskToUse.Id);
                if (children.TrueForAll(child => !SubTreeHasImpendingTaskNode(child)) &&
                    children.Exists(SubTreeHasTodoTaskNode))
                {
                    ApplyImpendingToFirstTodoNodeHitInDfsThatHasNoTodoDecendants(taskToUse, sender);
                }
            }
        }





        //FYI: Recusrive
        private bool SubTreeHasImpendingTaskNode(HodorTask subTreeRootTask)
        {
            return subTreeRootTask.CurrentTaskStatus == HodorTaskStatus.Impending ||
                   GetChildren_Internal(subTreeRootTask.Id)
                       .Where(x => x.CurrentTaskStatus != HodorTaskStatus.Completed)
                       .Any(SubTreeHasImpendingTaskNode);
        }


        //FYI: Recusrive
        private bool SubTreeHasTodoTaskNode(HodorTask subTreeRootTask)
        {
            return subTreeRootTask.CurrentTaskStatus == HodorTaskStatus.Todo || GetChildren_Internal(subTreeRootTask.Id)
                .Where(x => x.CurrentTaskStatus != HodorTaskStatus.Completed).Any(SubTreeHasTodoTaskNode);
        }


        private bool ApplyImpendingToFirstTodoNodeHitInDfsThatHasNoTodoDecendants(HodorTask task, object sender)
        {
            var children = GetChildren_Internal(task.Id).Where(x => x.CurrentTaskStatus != HodorTaskStatus.Completed);
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (var childTask in children)
// ReSharper restore LoopCanBeConvertedToQuery
            {
                if (ApplyImpendingToFirstTodoNodeHitInDfsThatHasNoTodoDecendants(childTask, sender)) return true;
            }
            if (task.CurrentTaskStatus != HodorTaskStatus.Todo) return false;

            UpdateTask_Internal(task.Id, sender, false, updatedStatus: HodorTaskStatus.Impending);
            return true;
        }

        #endregion

        #endregion

    }
}
