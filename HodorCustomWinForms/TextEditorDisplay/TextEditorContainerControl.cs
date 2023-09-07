using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contracts;
using Contracts.Dto;
using Contracts.DtoContracts;
using Contracts.MediatorPattern;
using Contracts.ObserverPattern;
using ViewNavigation;
using Workaholic.RTFEditor;

namespace HodorCustomWinForms.TextEditorDisplay
{
    public partial class TextEditorContainerControl : UserControl, ITextEditorDisplay
    {
        private IViewController _viewController = ViewControllerFactory.GetViewControllerInstance();
        private RTFEditor _rtfEditor;

        private enum DisplayControl
        {
            None = 0,
            Application = 1,
            RTFEditor = 2
        }

        private DisplayControl _currentlyDisplayedControl = DisplayControl.None;

        public TextEditorContainerControl()
        {
            InitializeComponent();

            _viewController.AddTextEditorDisplay(this);
        }


        public void DisposeStuff()
        {
            _viewController.RemoveTextEditorDisplay(this);
        }

        public void ClearCurrentControls()
        {
            foreach (var control in this.Controls)
            {
                if (control.GetType() == typeof(ApplicationControl))
                {
                    ((ApplicationControl)control).KillMe();
                }
                if (control.GetType() == typeof (RTFEditor))
                {
                    ((RTFEditor)control).OnDocumentSave -= OnRTFEditorSave;
                    ((RTFEditor)control).OnDocumentRefresh -= OnRTFEditorRefresh;
                }
            }

            this.Controls.Clear();
        }

        #region ITextEditorDisplay

        public void DisplayTaskDetails(int taskId)
        {
            DisplayTaskDetailsInNotepad(taskId);
        }

        public void DisplayProjectForest(int projectForestId)
        {
            DisplayProjectTreesTextView(projectForestId);
        }

        #endregion

        #region Display task details

        private void DisplayTaskDetailsInNotepad(int taskId)
        {
            var taskToDisplayDetails = _viewController.GetTask(taskId);
            if (taskToDisplayDetails != null)
            {
                _currentlyDisplayedControl = DisplayControl.Application;
                string path = string.Format("{0}.txt", taskToDisplayDetails.Id);
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, GetDefaultFileContents(taskToDisplayDetails.Title));
                }

                ClearCurrentControls();


                var appContlr = new ApplicationControl(path, "notepad.exe");//"C:\\Users\\Notandi\\AppData\\Local\\Google\\Chrome\\Application\\chrome.exe");

                this.SuspendLayout();

                appContlr.Anchor = Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));

                appContlr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

                appContlr.Size = this.Size;
                appContlr.Location = this.Location;

                appContlr.Dock = DockStyle.Fill;

                appContlr.TabIndex = 0;

                //            appContlr.BringToFront();

                this.Controls.Add(appContlr);

                this.ResumeLayout();

                appContlr.Visible = true;
                appContlr.Start();
            }
        }

        private string GetDefaultFileContents(string goalString)
        {
            return string.Format("Goal:{0}\t{1}{0}Req:{0}\t{0}Resources:{0}\t", Environment.NewLine, goalString);
        }
        #endregion

        #region Display TreeForm ProjectForest in RTFEditor  

        private int _displayedTextFormProjectForestId;

        private void DisplayProjectTreesTextView(int projectForestId)
        {
            ClearCurrentControls();

            _displayedTextFormProjectForestId = projectForestId;

            _currentlyDisplayedControl = DisplayControl.RTFEditor;

            _rtfEditor = new RTFEditor();

            _rtfEditor.DefaultFontSize = 11;
            _rtfEditor.TabSizeInPixels = 28;

            _rtfEditor.OnDocumentSave += OnRTFEditorSave;
            _rtfEditor.OnDocumentRefresh += OnRTFEditorRefresh;

            _rtfEditor.EnableNewDocument = false;
            _rtfEditor.DefaultWordWrap = true;

            this.SuspendLayout();

            _rtfEditor.Anchor = Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            //rftEdtior.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            _rtfEditor.Size = this.Size;
            _rtfEditor.Location = this.Location;

            _rtfEditor.Dock = DockStyle.Fill;

            //_rtfEditor.TabIndex = 0;

            _rtfEditor.RichTextBoxAcceptsTab = true;

            var textFormTaskTrees = CreateTextFormTaskTrees(_viewController.GetProjectForest(projectForestId).ProjectTreeRootIds);
            _rtfEditor.InitialText = textFormTaskTrees;

            this.Controls.Add(_rtfEditor);

            this.ResumeLayout();
            _rtfEditor.Visible = true;
        }

        #region RTFEditor delegates

        private void OnRTFEditorSave(object sender, RTFEditorEventArgs e)
        {
            e.Cancel = true;

            ParseEditorLines();
        }

        private void OnRTFEditorRefresh(object sender, RTFEditorEventArgs e)
        {
            e.Cancel = true;

//            var textFormTaskTrees = CreateTextFormTaskTrees(GetRootTaskTreeIds());
//
//            _rtfEditor.DocumentText = textFormTaskTrees;
            DisplayProjectTreesTextView(_displayedTextFormProjectForestId);
        }

        #endregion

        private struct HodorTaskWithChildren
        {
            public HodorTask Task;
            public List<HodorTaskWithChildren> Children;
        }

        //Assumes proper formatting
        private void ParseEditorLines()
        {
            var currentTaskAtEachDepth = new List<HodorTaskWithChildren>();
            var rtfLines = _rtfEditor.DocumentText.Split('\n');

            var depthOfLastTask = -1;

            var parsingRootTask = false;
            var currentRootTaskId = -1;

            _viewController.StartBatchDataSetUpdate();

            var rootTasks = new List<HodorTaskWithChildren>();

            foreach (var rtfLine in rtfLines)
            {
                if (string.IsNullOrWhiteSpace(rtfLine) || rtfLine.StartsWith("Req"))
                {
                    continue;
                }
                else if (rtfLine.StartsWith("Task:"))
                {
                    parsingRootTask = true;
                    currentRootTaskId = -1;
                }
                else if (rtfLine.StartsWith("Task"))
                {
                    var colonIdx = rtfLine.IndexOf(':');
                    int.TryParse(rtfLine.Substring(4, colonIdx - 4), out currentRootTaskId);
                    parsingRootTask = true;
                }
                else
                {
                    var isCompleted = rtfLine.StartsWith("D");
                    var depth = 0;
                    var taskText = "";
                    var taskId = -1;
                    var ordinalNumber = -1;

                    if (parsingRootTask)
                    {
                        taskText = rtfLine.Split('\t').ElementAt(1);
                        taskId = currentRootTaskId;
                        ordinalNumber = rootTasks.Count;
                    }
                    else
                    {
                        var tabSplitLine = rtfLine.TrimEnd('\t').Split('\t');

                        depth = tabSplitLine.Count() - 1;
                        Debug.Assert(depth <= depthOfLastTask + 1);

                        var idSplit = tabSplitLine[depth].Split(new string[] {" -|", "|-"},
                            StringSplitOptions.RemoveEmptyEntries);

                        taskText = idSplit[0];
                        
                        if (idSplit.Count() == 2)
                        {
                            Debug.Assert(int.TryParse(idSplit[1].Substring(4), out taskId));
                        }

                        ordinalNumber = currentTaskAtEachDepth.ElementAt(depth - 1).Children.Count;
                    }

                    var currentTask = new HodorTask()
                    {
                        Id = taskId,
                        //todo use NX
                        CurrentTaskStatus = isCompleted ? HodorTaskStatus.Completed : HodorTaskStatus.Todo,
                        Title = taskText,
                        Ordinal = ordinalNumber
                    };

                    var currentTaskWithChildren = new HodorTaskWithChildren()
                    {
                        Task = currentTask,
                        Children = new List<HodorTaskWithChildren>()
                    };

                    for (var i = depthOfLastTask; i >= depth; i--)
                    {
                        currentTaskAtEachDepth.RemoveAt(i);
                    }
                    depthOfLastTask = depth;
                    currentTaskAtEachDepth.Add(currentTaskWithChildren);

                    if (parsingRootTask)
                    {
                        rootTasks.Add(currentTaskWithChildren);                        
                        parsingRootTask = false;
                    }
                    else
                    {
                        currentTaskAtEachDepth.ElementAt(depth - 1).Children.Add(currentTaskWithChildren);
                    }
                }

            }

            UpdateDataLayerTaskTreeRecursiveTopLevel(rootTasks);

            _viewController.FinishBatchDataSetUpdate(this);

            //todo reset view
        }

        private void UpdateDataLayerTaskTreeRecursiveTopLevel(List<HodorTaskWithChildren> taskSiblings)
        {
            //check if the datalayer has any tasks with this parentId that are not in the sibling list and delete them if so
            var viewContollerTaskSiblingIds = _viewController.GetProjectForest(_displayedTextFormProjectForestId).ProjectTreeRootIds;
            
            var deletedViewContollerTaskSiblingIds =
                viewContollerTaskSiblingIds.Where(tssId => !taskSiblings.Exists(y => y.Task.Id == tssId)).ToArray();


            for (var i = deletedViewContollerTaskSiblingIds.Count() - 1; i >= 0; i--)
            {
                _viewController.DeleteTaskAndSubTasks(deletedViewContollerTaskSiblingIds[i], this);
            }


            //set ordinals of tasksSiblings to the values they should have
            SetTrueOrdinalsForTasksGivenTaskServiceData(taskSiblings);

            
            IProjectForest dpf =
                _viewController.GetProjectForest(_displayedTextFormProjectForestId);

            for (var i = 0; i < taskSiblings.Count; i++)
            {
                taskSiblings[i].Task.ParentId = HodorTask.PROJECT_TREE_ROOT_PARENT_ID;
                var task = taskSiblings[i].Task;

                //if task was created in editor
                if (task.Id == -1)
                {
                    //create project tree 
                    var newTaskId = 
                        _viewController.CreateProjectTree(task.Title, task.CurrentTaskStatus, task.Ordinal, this).Id;
                    taskSiblings[i].Task.Id = newTaskId;

                    _viewController.AddProjectTreeToProjectForest(newTaskId, dpf.Id, this);
                }
                else if (!IsProjectTreeRootInProjectForest(task.Id, dpf))
                {
                    var taskInViewContoller = _viewController.GetTask(task.Id);
                    if (taskInViewContoller.ParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
                    {
                        //create project tree 
                        var newTaskId = 
                            _viewController.CreateProjectTree(task.Title, task.CurrentTaskStatus, task.Ordinal, this).Id;
                        taskSiblings[i].Task.Id = newTaskId;

                        _viewController.AddProjectTreeToProjectForest(newTaskId, dpf.Id, this);
                        
                    }
                    else if (dpf.Type != ProjectForestType.FullProjectForest)
                    {
                        //add to current forest
                        _viewController.AddProjectTreeToProjectForest(task.Id, dpf.Id, this);
                        //and update if needed
                        UpdateTaskIfNeeded(task);
                    }
                }
                else
                {
                    //update task in data layer
                    UpdateTaskIfNeeded(task);
                }

                UpdateDataLayerTaskTreeRecursive(taskSiblings[i].Children, taskSiblings[i].Task.Id);
            }
        }


        private void UpdateDataLayerTaskTreeRecursive(List<HodorTaskWithChildren> taskSiblings, int parentId)
        {
            //check if the datalayer has any tasks with this parentId that are not in the sibling list and delete them if so
            IEnumerable<int> viewContollerTaskSiblingIds = _viewController.GetChildren(parentId).Select(x => x.Id);
            var deletedViewContollerTaskSiblingIds =
                viewContollerTaskSiblingIds.Where(tssId => !taskSiblings.Exists(y => y.Task.Id == tssId)).ToArray();
            

            for (var i = deletedViewContollerTaskSiblingIds.Count() - 1; i >= 0; i--)
            {
                _viewController.DeleteTaskAndSubTasks(deletedViewContollerTaskSiblingIds[i], this);
            }

            //set ordinals of tasksSiblings to the values they should have
            SetTrueOrdinalsForTasksGivenTaskServiceData(taskSiblings);

            //check if there are any tasks that we need to create or update in the datalayer
            for (int i = 0; i < taskSiblings.Count; i++)
            {
                taskSiblings[i].Task.ParentId = parentId;
                var task = taskSiblings[i].Task;

                //if task was created in editor
                if (task.Id == -1 || !ChildIdExistsForParentInDataLayer(task.Id, parentId))
                {
                    var taskStatus = 
                    //create task in data layer
                    taskSiblings[i].Task.Id =
                        _viewController.CreateTask(task.Title, parentId, task.CurrentTaskStatus, task.Ordinal, this).Id;
                }
                else
                {
                    //update task in data layer
                    UpdateTaskIfNeeded(task);
                }

                UpdateDataLayerTaskTreeRecursive(taskSiblings[i].Children, taskSiblings[i].Task.Id);
            }

        }

        private void SetTrueOrdinalsForTasksGivenTaskServiceData(List<HodorTaskWithChildren> taskSiblings)
        {
            if (taskSiblings.Count == 0) return;
            if (taskSiblings.Count == 1)
            {
                var taskId = taskSiblings[0].Task.Id;
                var taskServiceTask = _viewController.GetTask(taskId);
                taskSiblings[0].Task.Ordinal = taskServiceTask != null ? taskServiceTask.Ordinal : 0;
                return;
            }

            var trueOrdinalValues = taskSiblings.Select(x => _viewController.GetTask(x.Task.Id)).Select(task => task != null ? (int?)task.Ordinal : null).ToList();

            //all tasks in the layer/list were created now, so they alrearoody have correct ordinal numbers
            if (trueOrdinalValues.TrueForAll(x => x == null))
                return;

            var listCount = taskSiblings.Count;

            //if the list starts with one or more null values then initialize them first
            if (trueOrdinalValues[0] == null)
            {
                var firstNumberIdx = trueOrdinalValues.FindIndex(x => x != null);
                var firstNumber = trueOrdinalValues[firstNumberIdx];
                for (var i = 0; i < firstNumberIdx; i++)
                {
                    trueOrdinalValues[i] = firstNumber + i;
                }
                for (var i = firstNumberIdx; i < listCount; i++)
                {
                    if (trueOrdinalValues[i] >= firstNumber) trueOrdinalValues[i] += firstNumberIdx;
                }
            }

            //initialize remaining null values
            var lastNumber = 0;
            for (var i = 0; i < listCount; i++)
            {
                if (trueOrdinalValues[i] == null)
                {
                    trueOrdinalValues[i] = lastNumber + 1;
                    for (var j = 0; j < listCount; j++)
                    {
                        if (j == i) continue;
                        if (trueOrdinalValues[j] >= lastNumber + 1) trueOrdinalValues[j] += 1;
                    }
                }
                lastNumber = (int)trueOrdinalValues[i];
            }

            //fix the first value if it needs fixing
            if (trueOrdinalValues[0] > trueOrdinalValues[1])
            {
                var firstNum = trueOrdinalValues[0];
                var secondNum = trueOrdinalValues[1];
                trueOrdinalValues[0] = secondNum;
                for (var i = 1; i < listCount; i++)
                {
                    if (trueOrdinalValues[i] >= secondNum && trueOrdinalValues[i] < firstNum) trueOrdinalValues[i] += 1;
                }
            }

            //fix the rest of the values except the last one
            for (var i = 1; i < listCount; i++)
            {
                //if we dont have the last guy in the list AND
                //if the dude below is smaller than our current guy and larger than the guy above our current guy
                //THEN set our current guy one below the guy above us
                if (i != listCount - 1 &&
                    trueOrdinalValues[i + 1] < trueOrdinalValues[i] &&
                    trueOrdinalValues[i + 1] > trueOrdinalValues[i - 1])
                {
                    var oldNumI = trueOrdinalValues[i];
                    var newNumI = trueOrdinalValues[i - 1] + 1;
                    trueOrdinalValues[i] = newNumI;
                    for (var j = 0; j < listCount; j++)
                    {
                        if (j == i) continue;
                        if (trueOrdinalValues[j] >= newNumI && trueOrdinalValues[j] < oldNumI) trueOrdinalValues[j] += 1;
                    }
                }
                //else if the guy above our current guy is larger than the current guy
                //THEN set the current guy as the guy above us
                else if (trueOrdinalValues[i - 1] > trueOrdinalValues[i])
                {
                    var oldNumI = trueOrdinalValues[i];
                    var newNumI = trueOrdinalValues[i - 1];
                    trueOrdinalValues[i] = newNumI;
                    for (var j = 0; j < listCount; j++)
                    {
                        if (j == i) continue;
                        if (trueOrdinalValues[j] <= newNumI && trueOrdinalValues[j] > oldNumI) trueOrdinalValues[j] -= 1;
                    }
                }
            }

            for (var i = 0; i < listCount; i++)
            {
                taskSiblings[i].Task.Ordinal = (int)trueOrdinalValues[i];
            }
        }

        private bool IsProjectTreeRootInProjectForest(int projectTreeRootId, IProjectForest projectForest)
        {
            return projectForest.ProjectTreeRootIds.Contains(projectTreeRootId);
        }

        private bool ChildIdExistsForParentInDataLayer(int childId, int parentId)
        {
            var children = _viewController.GetChildren(parentId).ToList();
            return children.Exists(x => x.Id == childId);
        }

        private void UpdateTaskIfNeeded(HodorTask editorTask)
        {
            var taskServiceVersion = _viewController.GetTask(editorTask.Id);
            
            //todo use NX
            var isTaskStatusChanged = editorTask.CurrentTaskStatus != taskServiceVersion.CurrentTaskStatus && (editorTask.CurrentTaskStatus == HodorTaskStatus.Completed || taskServiceVersion.CurrentTaskStatus == HodorTaskStatus.Completed);
            var isTitleChanged = !string.Equals(editorTask.Title, taskServiceVersion.Title, StringComparison.Ordinal);
            var isOrdinalChanged = editorTask.Ordinal != taskServiceVersion.Ordinal;
            

            //todo use NX in textEditor to set/display impending status
            if (isTaskStatusChanged || isTitleChanged || isOrdinalChanged)
            {
                _viewController.UpdateTask(
                    editorTask.Id, 
                    this, 
                    updatedTitle: isTitleChanged ? editorTask.Title : null, 
                    updatedTaskOrdinal: isOrdinalChanged ? (int?)editorTask.Ordinal : null, 
                    updatedStatus: isTaskStatusChanged ? (HodorTaskStatus?)editorTask.CurrentTaskStatus : null);
            }
        }

        private string CreateTextFormTaskTrees(List<int> taskTreeRootIds)
        {
            string textFormTaskTreesString = "";
            foreach (var taskTreeRootId in taskTreeRootIds)
            {
                var taskTreeRootTask = _viewController.GetTask(taskTreeRootId);
                textFormTaskTreesString += string.Format("Task{0}:\n", taskTreeRootTask.Id);
                if (taskTreeRootTask.CurrentTaskStatus == HodorTaskStatus.Completed)
                {
                    textFormTaskTreesString += "D";
                }
                textFormTaskTreesString += "\t" + taskTreeRootTask.Title + "\n";
                textFormTaskTreesString += "Req:\n";

                var childTasks = _viewController.GetChildren(taskTreeRootId);
                foreach (var childTask in childTasks)
                {
                    textFormTaskTreesString += CreateTaskTreeSubTaskString(childTask, 0);
                }
                textFormTaskTreesString += "\n";
            }
            return textFormTaskTreesString;
        }

        private string CreateTaskTreeSubTaskString(HodorTask currentTask, int level)
        {
            string textFormTaskTreesSubString = "";
            if (currentTask.CurrentTaskStatus == HodorTaskStatus.Completed)
            {
                textFormTaskTreesSubString += "D";
            }

            textFormTaskTreesSubString += String.Concat(Enumerable.Repeat("\t", level+1)) + 
                currentTask.Title +
                string.Format(" -|Task{0}|-", currentTask.Id) +
                "\n";
            var childTasks = _viewController.GetChildren(currentTask.Id);
            foreach (var childTask in childTasks)
            {
                textFormTaskTreesSubString += CreateTaskTreeSubTaskString(childTask, level+1);
            }
            return textFormTaskTreesSubString;
        }

        #endregion


    }
}
