using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Contracts.Dto;
using Contracts.DtoContracts;
using Contracts.ObserverPattern;
using HodorData.Dto;

namespace HodorData
{
    public class HodorTaskServiceStub : IHodorTaskService
    {
        private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private List<HodorTask> _tasks = new List<HodorTask>(); 
        private Dictionary<int, List<int>> _childTaskLists = new Dictionary<int, List<int>>();
        private List<IProjectForest> _projectForests = new List<IProjectForest>(); 
        private int _nextId = 0;
        private int _fullProjectForestId;


        private List<ITaskServiceDataSetObserver> _taskServiceDataSetObservers = new List<ITaskServiceDataSetObserver>();
        private bool _inBatchEditMode = false;
        private bool _batchEditModeDataSetModified = false;


        public HodorTaskServiceStub()
        {
            CreateMockTasks();
            CreateMockProjectForests();
        }

        #region init
        private void CreateMockTasks()
        {
            CreateTask_Core("Hodor", HodorTask.PROJECT_TREE_ROOT_PARENT_ID, HodorTaskStatus.Todo); //0
            CreateTask_Core("Think on what I need from hodor currently", 0, HodorTaskStatus.Todo);
            CreateTask_Core("Look at moving the usage of the ApplicationControl class into a custom Control class", 0, HodorTaskStatus.Todo);
            CreateTask_Core("Decide how to propogate events in the system (propogating them throughout the system VS each object subscribes to a global CallBack service)", 0, HodorTaskStatus.Todo);
            CreateTask_Core("Make a clean working version of current code", 0, HodorTaskStatus.Todo);
            CreateTask_Core("See how the old note file \"HodorCurr2\" compares to the object/layer list.", 0, HodorTaskStatus.Todo);
            CreateTask_Core("Decide what I want to implement next", 0, HodorTaskStatus.Todo);
            CreateTask_Core("Look at old note files for ideas.", 6, HodorTaskStatus.Impending);
            CreateTask_Core("Implement text view for editing task trees", 0, HodorTaskStatus.Todo);
            CreateTask_Core("Search for winforms wysiwyg editors", 8, HodorTaskStatus.Impending);
            CreateTask_Core("Go over functionality in RTFEditor", 8, HodorTaskStatus.Todo);
            CreateTask_Core("Save file", 10, HodorTaskStatus.Todo);
            CreateTask_Core("Open file", 10, HodorTaskStatus.Todo);
            CreateTask_Core("Events", 8, HodorTaskStatus.Todo);
            CreateTask_Core("Figure out which files from the RTFEditor code I need to copy into my project for my needs", 8, HodorTaskStatus.Todo);
            CreateTask_Core("We need the RTFEditor.cs files", 14, HodorTaskStatus.Todo);
            CreateTask_Core("Copy files", 8, HodorTaskStatus.Todo);
            CreateTask_Core("Modify TextEditorContainerControl for management of RTFEditor", 8, HodorTaskStatus.Todo);
            CreateTask_Core("Create IOnOpenTaskTreesTextViewObserver", 17, HodorTaskStatus.Todo);
            CreateTask_Core("Create IOnOpenTaskTreesTextViewObservable", 17, HodorTaskStatus.Todo);
            CreateTask_Core("Implement IOnOpenTaskTreesTextViewObservable in IViewController", 17, HodorTaskStatus.Todo);
            CreateTask_Core("Implement AddOnOpenTaskTreesTextViewObserver", 20, HodorTaskStatus.Todo);
            CreateTask_Core("Implement RemoveOnOpenTaskTreesTextViewObserver", 20, HodorTaskStatus.Todo);
            CreateTask_Core("Create NotifyOpenTaskTreesTextView", 17, HodorTaskStatus.Todo);
            CreateTask_Core("Use NotifyOpenTaskTreesTextView", 17, HodorTaskStatus.Todo);
            CreateTask_Core("Implement OnOpenTaskTreesTextViewObservers", 17, HodorTaskStatus.Todo);
            CreateTask_Core("Write code to open up RTFEditor", 17, HodorTaskStatus.Todo);
            CreateTask_Core("first", HodorTask.PROJECT_TREE_ROOT_PARENT_ID, HodorTaskStatus.Todo);//27
            CreateTask_Core("second", HodorTask.PROJECT_TREE_ROOT_PARENT_ID, HodorTaskStatus.Todo);//28
            CreateTask_Core("Create NotifyOpenTaskTreesTextView", 27, HodorTaskStatus.Todo);
            CreateTask_Core("Use NotifyOpenTaskTreesTextView", 28, HodorTaskStatus.Todo);
            CreateTask_Core("Implement OnOpenTaskTreesTextViewObservers", 29, HodorTaskStatus.Todo);
            CreateTask_Core("Write code to open up RTFEditor", 29, HodorTaskStatus.Impending);
            CreateTask_Core("Go over functionality in RTFEditor", 30, HodorTaskStatus.Impending);
            CreateTask_Core("Save file", 30, HodorTaskStatus.Impending);
            CreateTask_Core("Open file", 34, HodorTaskStatus.Todo);
            CreateTask_Core("Events", 34, HodorTaskStatus.Todo);
            
        }


        private void CreateMockProjectForests()
        {
            _fullProjectForestId = 0;
            var fullProjectForest = new FullProjectForest
            {
                Id = _fullProjectForestId,
                Title = "All Project Trees",
                ProjectTreeRootIds = GetChildren(HodorTask.PROJECT_TREE_ROOT_PARENT_ID).Select(x => x.Id).ToList()
            };
            _projectForests.Add(fullProjectForest);
            var currentSession = new SessionProjectForest()
            {
                Id = 1,
                Title = "07.02.16 Session",
                ProjectTreeRootIds = GetChildren(HodorTask.PROJECT_TREE_ROOT_PARENT_ID).Select(x => x.Id).ToList(),
                ProjectTreeTaskCompletionGoals = new Dictionary<int, int>()
            };
            currentSession.ProjectTreeTaskCompletionGoals.Add(0, 12);
            currentSession.ProjectTreeTaskCompletionGoals.Add(27, 2);
            currentSession.ProjectTreeTaskCompletionGoals.Add(28, 0);
            _projectForests.Add(currentSession);
        }

        #endregion

        #region Create

        public HodorTask CreateTask(string title, int parentId, HodorTaskStatus status, object sender)
        {
            var newTask = CreateTask_Core(title, parentId, status);
            NotifyTaskServiceDataSetChanged(sender);
            return newTask;
        }

        public HodorTask CreateTask(string title, int parentId, HodorTaskStatus status, int ordinal, object sender)
        {
            _rwLock.EnterWriteLock();

            var task = CreateTask_Core(title, parentId, status);

            UpdateOrdinal_Core(task, parentId, ordinal);

            _rwLock.ExitWriteLock();

            NotifyTaskServiceDataSetChanged(sender);

            return task;
        }

        //Returns the root task of the new project tree
        public HodorTask CreateProjectTree(string rootTaskTitle, HodorTaskStatus rootTaskStatus, object sender)
        {
            _rwLock.EnterWriteLock();
            var task = CreateTask_Core(rootTaskTitle, HodorTask.PROJECT_TREE_ROOT_PARENT_ID, rootTaskStatus);
            foreach (var fullProjectForest in _projectForests.Where(x => x.Type == ProjectForestType.FullProjectForest).Where(fullProjectForest => !fullProjectForest.ProjectTreeRootIds.Contains(task.Id)))
            {
                fullProjectForest.ProjectTreeRootIds.Add(task.Id);
            }
            _rwLock.ExitWriteLock();

            NotifyTaskServiceDataSetChanged(sender);
            return task;
        }

        //Returns the root task of the new project tree
        public HodorTask CreateProjectTree(string rootTaskTitle, HodorTaskStatus rootTaskStatus, int projectTreeOrdinal, object sender)
        {
            _rwLock.EnterWriteLock();
            var task = CreateTask_Core(rootTaskTitle, HodorTask.PROJECT_TREE_ROOT_PARENT_ID, rootTaskStatus);
            foreach (var fullProjectForest in _projectForests.Where(x => x.Type == ProjectForestType.FullProjectForest).Where(fullProjectForest => !fullProjectForest.ProjectTreeRootIds.Contains(task.Id)))
            {
                fullProjectForest.ProjectTreeRootIds.Add(task.Id);
            }
            UpdateOrdinal_Core(task, HodorTask.PROJECT_TREE_ROOT_PARENT_ID, projectTreeOrdinal);
            _rwLock.ExitWriteLock();

            NotifyTaskServiceDataSetChanged(sender);
            return task;
        }

        private HodorTask CreateTask_Core(string title, int parentId, HodorTaskStatus status)
        {
            _rwLock.EnterWriteLock();

            int taskId = _nextId;
            _nextId++;
            var newTask = new HodorTask
            {
                Id = taskId,
                Title = title,
                ParentId = parentId,
                CurrentTaskStatus = status,
                Ordinal = GetChildren(parentId).Count()
            };

            _tasks.Add(newTask);
            AddChildToTask(parentId, taskId);

            _rwLock.ExitWriteLock();

            return newTask;
        }



        private void AddChildToTask(int taskId, int childId)
        {
            List<int> childTasks;
            _childTaskLists.TryGetValue(taskId, out childTasks);

            if (childTasks == null)
            {
                childTasks = new List<int>();
                _childTaskLists.Add(taskId, childTasks);
            }

            childTasks.Add(childId);
        }


        #endregion

        #region Read

        public HodorTask GetTask(int taskId)
        {
            _rwLock.EnterReadLock();
            var task = _tasks.SingleOrDefault(x => x.Id == taskId);
            _rwLock.ExitReadLock();
            return task;
        }

        public List<HodorTask> GetChildren(int taskId)
        {
            _rwLock.EnterReadLock();
            var children = !_childTaskLists.ContainsKey(taskId)
                ? new List<HodorTask>()
                : _childTaskLists[taskId].Select(GetTask).OrderBy(x => x.Ordinal).ToList();
            _rwLock.ExitReadLock();
            return children;
        }

        public List<int> GetAllProjectTreeRootIds()
        {
            return GetChildren(HodorTask.PROJECT_TREE_ROOT_PARENT_ID).Select(x => x.Id).ToList();
        }

        public IProjectForest GetProjectForest(int projectForestId)
        {
            return _projectForests.SingleOrDefault(x => x.Id == projectForestId);
        }

        public List<IProjectForest> GetCurrentlyDisplayedProjectForests()
        {
            return _projectForests;
        }

        #endregion

        #region Update

        public bool UpdateTask(int taskId, string updatedTitle, HodorTaskStatus? updatedStatus, int? updatedTaskOrdinal, object sender)
        {
            _rwLock.EnterWriteLock();

            var taskToUpdate = _tasks.SingleOrDefault(x => x.Id == taskId);

            if (taskToUpdate == null) throw new SystemException("Task to update doesnt exist in tasks list in TaskService");

            var taskUpdated = false;

            if (updatedTitle != null && !string.Equals(taskToUpdate.Title, updatedTitle, StringComparison.Ordinal))
            {
                taskToUpdate.Title = updatedTitle;
                taskUpdated = true;
            }



            if (updatedTaskOrdinal != null && taskToUpdate.Ordinal != updatedTaskOrdinal)
            {
                UpdateOrdinal_Core(taskToUpdate, taskToUpdate.ParentId, (int)updatedTaskOrdinal);
                taskUpdated = true;
            }
            if (updatedStatus != null && taskToUpdate.CurrentTaskStatus != updatedStatus)
            {
                taskToUpdate.CurrentTaskStatus = (HodorTaskStatus)updatedStatus;
                taskUpdated = true;
            }

            _rwLock.ExitWriteLock();

            if (taskUpdated)
            {
                NotifyTaskServiceDataSetChanged(sender);
            }

            return taskUpdated;
        }

        private void UpdateOrdinal_Core(HodorTask task, int parentId, int newOrdinal)
        {
            _rwLock.EnterWriteLock();
            
            var siblings = GetChildren(parentId).ToList();

            if (task.Ordinal >= siblings.Count())
                throw new SystemException("Ordinal is wrong in HodorTaskServiceStub CreateTask");

            if (task.Ordinal == newOrdinal)
                return;

            if (task.Ordinal < newOrdinal)
            {
                for (int i = task.Ordinal + 1; i <= newOrdinal; i++)
                {
                    siblings[i].Ordinal = i - 1;
                }
            }
            else
            {
                for (int i = task.Ordinal - 1; i >= newOrdinal; i--)
                {
                    siblings[i].Ordinal = i + 1;
                }
            }
            task.Ordinal = newOrdinal;

            _rwLock.ExitWriteLock();
        }

        public void AddProjectTreeToProjectForest(int projectTreeRootTaskId, int projectForestId, object sender)
        {
            _rwLock.EnterWriteLock();

            var projTreeRootTask = GetTask(projectTreeRootTaskId);
            if (projTreeRootTask.ParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
            {
                throw new SystemException("VC: Trying to add a non-root task as a project tree root task to a project forest");
            }

            var projectForest = _projectForests.SingleOrDefault(x => x.Id == projectForestId);
            if (projectForest != null)
            {
                if (!projectForest.ProjectTreeRootIds.Contains(projectTreeRootTaskId))
                {
                    projectForest.ProjectTreeRootIds.Add(projectTreeRootTaskId);
                    var forest = projectForest as ISessionProjectForest;
                    if (forest != null)
                    {
                        forest.ProjectTreeTaskCompletionGoals.Add(projectTreeRootTaskId, 0);
                    }
                }
            }

            NotifyTaskServiceDataSetChanged(sender);

            _rwLock.ExitWriteLock();
        }

        public void UpdateTaskCompletionGoalForTreeInSession(int sessionProjectForestId, int projectTreeRootId, int updatedTaskCompletionGoal)
        {
            _rwLock.EnterWriteLock();

            var session = _projectForests.OfType<ISessionProjectForest>().SingleOrDefault(x => x.Id == sessionProjectForestId);
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

            _rwLock.ExitWriteLock();
            //todo just does null;
            NotifyTaskServiceDataSetChanged(null);
        }

        #endregion

        #region Delete

        //Deletes the given task and all its decendants
        public void DeleteTaskAndSubTasks(int taskId, object sender)
        {
            //don't want to delete the root
            if (taskId == HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
            {
                throw new SystemException("TS: Trying to delete task with id: PROJECT_TREE_ROOT_PARENT_ID");
            }
            
            _rwLock.EnterWriteLock();

            if (_childTaskLists.ContainsKey(taskId))
            {
                var childTaskList = _childTaskLists[taskId];
                foreach (var childTaskId in childTaskList)
                {
                    DeleteTaskAndSubTasks_RecursiveHelper(childTaskId);
                }
                _childTaskLists.Remove(taskId);
            }
            if (_tasks.Exists(x => x.Id == taskId))
            {
                var task = _tasks.Single(x => x.Id == taskId);
                var parentId = task.ParentId;

                //if project tree then remove from project forests 
                if (parentId == HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
                {
                    foreach (var forest in _projectForests.Where(forest => forest.ProjectTreeRootIds.Contains(task.Id)))
                    {
                        forest.ProjectTreeRootIds.Remove(task.Id);
                    }
                }

                //fix ordinals
                var siblings = GetChildren(parentId);
                for (int i = task.Ordinal + 1; i < siblings.Count; i++)
                {
                    siblings[i].Ordinal = i - 1;
                }

                //remove from sibling lists
                _childTaskLists[parentId].RemoveAll(x => x == taskId);
                
                //remove task
                _tasks.RemoveAll(x => x.Id == taskId);
            }

            _rwLock.ExitWriteLock();

            NotifyTaskServiceDataSetChanged(sender);
        }

        private void DeleteTaskAndSubTasks_RecursiveHelper(int subTaskId)
        {
            if (_childTaskLists.ContainsKey(subTaskId))
            {
                var childTaskList = _childTaskLists[subTaskId];
                foreach (var childTaskId in childTaskList)
                {
                    DeleteTaskAndSubTasks_RecursiveHelper(childTaskId);
                }
                _childTaskLists.Remove(subTaskId);
            }
            _tasks.RemoveAll(x => x.Id == subTaskId);
        }

        #endregion

        #region ITaskServiceDataSetObservable

        public void AddTaskServiceDataSetChangedObserver(ITaskServiceDataSetObserver observer)
        {
            if (!_taskServiceDataSetObservers.Contains(observer))
            {
                _taskServiceDataSetObservers.Add(observer);
            }
        }

        public void RemoveTaskServiceDataSetChangedObserver(ITaskServiceDataSetObserver observer)
        {
            if (_taskServiceDataSetObservers.Contains(observer))
            {
                _taskServiceDataSetObservers.Remove(observer);
            }
        }

        public void NotifyTaskServiceDataSetChanged(object sender)
        {
            if (_inBatchEditMode)
            {
                _batchEditModeDataSetModified = true;
            }
            else
            {
                foreach (var taskServiceDataSetObserver in _taskServiceDataSetObservers)
                {
                    taskServiceDataSetObserver.OnTaskServiceDataSetChanged(sender);
                }
            }
        }

        //MAKE SURE WE ALWAYS CALL FinishBatchDataSetUpdate AFTER THIS 
        public void StartBatchDataSetUpdate()
        {
            _rwLock.EnterWriteLock();

            _inBatchEditMode = true;
            _batchEditModeDataSetModified = false;
        }

        public void FinishBatchDataSetUpdate(object sender)
        {
            bool shouldSendNotifyDataSetChanged = _batchEditModeDataSetModified;
            _inBatchEditMode = false;
            
            _rwLock.ExitWriteLock();

            if (shouldSendNotifyDataSetChanged)
            {
                NotifyTaskServiceDataSetChanged(sender);
            }
        }

        #endregion
    }
}

