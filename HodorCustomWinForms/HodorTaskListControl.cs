using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Contracts;
using Contracts.Dto;
using Contracts.DtoContracts;
using Contracts.ObserverPattern;
using HodorData;
using ViewNavigation;
using ViewNavigation.Dto;

namespace HodorCustomWinForms
{
    public partial class HodorTaskListControl : UserControl, ICurrentViewDataObserver
    {
        //todo Does this have any purpose?
//        private List<HodorTask> _childTasks;

        private IViewController _viewController =
            ViewControllerFactory.GetViewControllerInstance();

        private IViewTaskFocus currentViewTaskFocus;

        private bool _vScrollBarModOnChildPanelActive = false;

        #region Ctor and dispose

        public HodorTaskListControl()
        {
            InitializeComponent();
        
            _viewController.AddCurrentViewDataObserver(this);
            this.createTaskControl.OnCreateTask += this.CreateTask;
            
            DisplayFocusedTask();
        }

        private void DisposeStuff()  
        {
            _viewController.RemoveViewDisplayDataObserver(this);
            this.createTaskControl.OnCreateTask -= this.CreateTask;
        }

        #endregion

        #region ICurrentDisplayDataObserver

        #region OnViewTaskFocusChanged

        public void OnViewTaskFocusChanged(object sender)
        {
            DisplayFocusedTask();
        }

        #endregion

        #region OnViewDataSetChanged

        public void OnViewDataSetChanged(object sender)
        {
            //if (sender == null || !(sender is Control && IsThisOrChildControl((Control)sender)))
            //{
                DisplayFocusedTask();
            //}
        }

        private bool IsThisOrChildControl(Control ctrl)
        {
            return this == ctrl || this.currentTaskContainer == ctrl || this.childTaskFlowPanel.Controls.Contains(ctrl);
        }

        #endregion

        #endregion


        private void PopulateChildTaskFlowPanel(IEnumerable<HodorTask> tasksToDisplay)
        {
            childTaskFlowPanel.Controls.Clear();
            foreach (var hodorBaseTask in tasksToDisplay)
            {
                AddNewChildTaskToFlowPanel(hodorBaseTask);
            }
        }

        private void AddNewChildTaskToFlowPanel(HodorTask taskToDisplay)
        {
            var vScrollVisibleBefore = this.childTaskFlowPanel.VerticalScroll.Visible;

            var taskControl = new HodorTaskListItemControl(taskToDisplay, true);
            taskControl.Margin = new Padding(0,3,3,0);
            //taskControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            SetTaskListItemControlWidth(taskControl);
            childTaskFlowPanel.Controls.Add(taskControl);
            taskControl.PerformLayout();

            var vScrollVisibleAfter = this.childTaskFlowPanel.VerticalScroll.Visible;
            if (!_vScrollBarModOnChildPanelActive && vScrollVisibleAfter)
            {
                _vScrollBarModOnChildPanelActive = true;
                this.childTaskFlowPanel.Width += System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            }
            if (vScrollVisibleAfter && !vScrollVisibleAfter)
            {
                _vScrollBarModOnChildPanelActive = false;
                this.childTaskFlowPanel.Width -= System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            }
        }

        public void DisplayFocusedTask()
        {
            List<HodorTask> childTasks;
            var vtf = _viewController.GetViewTaskFocus();

            var shouldPersistVerticalScrollPos = false;
            var persistedVerticalScrollPos = 0;
            var persistedLowestVisibleControlIdx = 0;


            if (currentViewTaskFocus != null && currentViewTaskFocus.FocusedTaskId == vtf.FocusedTaskId)
            {
                if (currentViewTaskFocus.FocusedTaskId == HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
                {
                    if (currentViewTaskFocus.FocusedForestId == vtf.FocusedForestId)
                    {   
                        shouldPersistVerticalScrollPos = this.childTaskFlowPanel.VerticalScroll.Visible;
                        persistedVerticalScrollPos = this.childTaskFlowPanel.VerticalScroll.Value;
                    }
                }
                else
                {
                    //Todo be aware that the forestIds might be different here ( but that shouldnt matter as we are displaying the same taskId as before and it should have the same list of children)
                    shouldPersistVerticalScrollPos = this.childTaskFlowPanel.VerticalScroll.Visible;
                    persistedVerticalScrollPos = this.childTaskFlowPanel.VerticalScroll.Value;

                }
            }
            if (shouldPersistVerticalScrollPos)
            {
                var lowestVisibleControl =
                    this.childTaskFlowPanel.GetChildAtPoint(new Point(10, this.childTaskFlowPanel.Height - 1)) as HodorTaskListItemControl;
                var offset = 2;
                while (lowestVisibleControl == null)
                {
                    lowestVisibleControl =
                        this.childTaskFlowPanel.GetChildAtPoint(new Point(10,
                            this.childTaskFlowPanel.Height - offset)) as HodorTaskListItemControl;
                    offset += 1;
                }
                persistedLowestVisibleControlIdx = this.childTaskFlowPanel.Controls.IndexOf(lowestVisibleControl);

            }

            currentViewTaskFocus = new ViewTaskFocus
            {
                FocusedTaskId = vtf.FocusedTaskId,
                FocusedForestId = vtf.FocusedForestId
            };
            
            if (vtf.FocusedTaskId == HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
            {   
                var projForest = _viewController.GetProjectForest(vtf.FocusedForestId);
                childTasks = projForest.ProjectTreeRootIds.Select(_viewController.GetTask).ToList();
                currentTaskContainer.DisplayProjectForestInfo(projForest.Title);
                viewParentTaskButton.Enabled = false;
            }
            else
            {
                var currentTask = _viewController.GetTask(vtf.FocusedTaskId);

                childTasks = _viewController.GetChildren(vtf.FocusedTaskId).ToList();
                currentTaskContainer.SetData(currentTask, false);
                viewParentTaskButton.Enabled = true;
            }
            PopulateChildTaskFlowPanel(childTasks);

            if (shouldPersistVerticalScrollPos && this.childTaskFlowPanel.VerticalScroll.Visible)
            {
                //this part is to get the scroll bar to move (otherwise it would just stay at the top after getting moved when ctrls 
                //were cleared in PopulateChildTaskFlowPanel)
                var ctrlToScrollToIdx = persistedLowestVisibleControlIdx < this.childTaskFlowPanel.Controls.Count
                    ? persistedLowestVisibleControlIdx
                    : this.childTaskFlowPanel.Controls.Count - 1;
                var ctrlToScrollTo = this.childTaskFlowPanel.Controls[ctrlToScrollToIdx];

                this.childTaskFlowPanel.ScrollControlIntoView(ctrlToScrollTo);


                //the following part is to get the illusion of a perfectly persisted scroll location
                //(and it will be pretty close, the view will be correct but the scroll bar will probably be a tiny bit off)
                var maxVerticalScrollPos = this.childTaskFlowPanel.DisplayRectangle.Height -
                                           this.childTaskFlowPanel.ClientRectangle.Height;

                this.childTaskFlowPanel.AutoScroll = true;
                this.childTaskFlowPanel.VerticalScroll.Value = (persistedVerticalScrollPos <= maxVerticalScrollPos) ? persistedVerticalScrollPos : maxVerticalScrollPos;
                
            }
        }

        public void CreateTask(string title)
        {
            if (title != "")
            {
                var vScrollVisibleBefore = this.childTaskFlowPanel.VerticalScroll.Visible;

                HodorTask createdTask;
                var vtf = _viewController.GetViewTaskFocus();
                if (vtf.FocusedTaskId == HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
                {
                    createdTask = _viewController.CreateProjectTree(title, HodorTaskStatus.Todo, this);
                    var dpf = _viewController.GetProjectForest(vtf.FocusedForestId);
                    if (dpf.Type != HodorProjectForestType.FullProjectForest)
                    {
                        _viewController.AddProjectTreeToProjectForest(createdTask.Id, dpf.Id, this);
                    }
                }
                else
                {
                    createdTask = _viewController.CreateTask(title, vtf.FocusedTaskId, HodorTaskStatus.Todo, this);
                }
//                AddNewChildTaskToFlowPanel(createdTask);

                var max = this.childTaskFlowPanel.HorizontalScroll.Maximum;
                var min = this.childTaskFlowPanel.HorizontalScroll.Minimum;
                var dr = this.childTaskFlowPanel.DisplayRectangle;
                var cr = this.childTaskFlowPanel.ClientRectangle;
            }
        }

        private void viewParentTaskButton_Click(object sender, EventArgs e)
        {
            var vtf = _viewController.GetViewTaskFocus();
            var ft = _viewController.GetTask(vtf.FocusedTaskId);
            _viewController.SetFocusedTaskGivenFocusedForest(ft.ParentId, this);
        }

        private void childTaskFlowPanel_SizeChanged(object sender, EventArgs e)
        {
            if (_vScrollBarModOnChildPanelActive != this.childTaskFlowPanel.VerticalScroll.Visible)
            {
                _vScrollBarModOnChildPanelActive = !_vScrollBarModOnChildPanelActive;
                if (_vScrollBarModOnChildPanelActive)
                {
                    this.childTaskFlowPanel.Width += System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
                }
                else
                {
                    this.childTaskFlowPanel.Width -= System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
                }
            }

            foreach (HodorTaskListItemControl control in childTaskFlowPanel.Controls)
            {
                SetTaskListItemControlWidth(control);
            }
        }

        private void SetTaskListItemControlWidth(HodorTaskListItemControl control)
        {
            var vScrollBarWidth = this.childTaskFlowPanel.VerticalScroll.Visible
                   ? System.Windows.Forms.SystemInformation.VerticalScrollBarWidth
                   : 0;
            control.Width = childTaskFlowPanel.Width - 3 - vScrollBarWidth;
        }
    }
}
