using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contracts;
using Contracts.Dto;
using Contracts.DtoContracts;
using Contracts.ObserverPattern;
using HodorData;
using ViewNavigation;

namespace HodorCustomWinForms
{
    public partial class HodorTreeView : UserControl, ICurrentViewDataObserver
    {
        private IViewController _viewController =
            ViewControllerFactory.GetViewControllerInstance();

        private bool _handlingTaskFocusEvent = false;

        #region Ctor and dispose
        
        public HodorTreeView()
        {
            InitializeComponent();
            this.treeView1.HideSelection = false;
            PopulateTreeView();
            this.treeView1.AfterSelect += AfterNodeSelect;
            _viewController.AddCurrentViewDataObserver(this);
        }

        protected void DisposeStuff(bool disposing)
        {
            this.treeView1.AfterSelect -= AfterNodeSelect;
            _viewController.RemoveViewDisplayDataObserver(this);
        }

        #endregion

        #region Event Handlers

        private void AfterNodeSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selctdNode = e.Node;
            setImpendingToolStripMenuItem.Enabled = selctdNode.Parent != null || !(selctdNode.Tag is HodorTask) ||
                                                    ((HodorTask) selctdNode.Tag).CurrentTaskStatus != HodorTaskStatus.Completed;
            
            if (!_handlingTaskFocusEvent)
            {
                TreeNode selectedNode = e.Node;

                //ProjectForest node selected
                if (selectedNode.Parent == null)
                {
                    var selectedProjectForestId = (int) selectedNode.Tag;
                    _viewController.SetFocusedTaskAndForest(HodorTask.PROJECT_TREE_ROOT_PARENT_ID, selectedProjectForestId, this);
                }
                //Task node selected
                else
                {
                    var selectedTask = (HodorTask) selectedNode.Tag;

                    var ancestorNode = selectedNode;
                    while (ancestorNode.Parent != null) ancestorNode = ancestorNode.Parent;
                    var selectedProjectForestId = (int)ancestorNode.Tag;

                    _viewController.SetFocusedTaskAndForest(selectedTask.Id, selectedProjectForestId, this);
                }
            }
            else
            {
                _handlingTaskFocusEvent = false;
            }
        }

        private void ToggleSelectedTaskImpendingStatus(object sender, EventArgs e)
        {
            var node = this.treeView1.SelectedNode as TreeNode;
            var nodeTask = (HodorTask)node.Tag;
            nodeTask.CurrentTaskStatus = nodeTask.CurrentTaskStatus != HodorTaskStatus.Impending
                ? HodorTaskStatus.Impending
                : HodorTaskStatus.Todo;
            _viewController.UpdateTask(nodeTask.Id, this, updatedStatus: nodeTask.CurrentTaskStatus);
        }

        #endregion

        private void PopulateTreeView()
        {
            this.treeView1.Nodes.Clear();

            var displayedProjectForests = _viewController.GetDisplayedprojectForests();

            foreach (var dpf in displayedProjectForests)
            {
                AddProjectForestToTreeView(dpf);
            }

            SelectCurrentViewFocusTask();
        }

        private void AddProjectForestToTreeView(IHodorProjectForest projectForest)
        {
            var projectForestRootNode = new TreeNode(projectForest.Title) { Tag = projectForest.Id };
            AddSubTaskNodes(projectForest.ProjectTreeRootIds.Select(_viewController.GetTask), projectForestRootNode, null);
            this.treeView1.Nodes.Add(projectForestRootNode);
        }

        private void AddSubTaskNodes(IEnumerable<HodorTask> childTasks, TreeNode nodeToAddTo, List<HodorTask> impendingTaskTreeTaskList)
        {
            foreach (var childTask in childTasks)
            {
                var newNode = new TreeNode(childTask.Title) {Tag = childTask.Clone()};
                ApplyImpendingStyle(newNode, childTask, impendingTaskTreeTaskList);
                ApplyTreeNodeImage(newNode, childTask);
                List<HodorTask> grandChildTasks = _viewController.GetChildren(childTask.Id).ToList();
                if (grandChildTasks.Count() != 0)
                {
                    AddSubTaskNodes(grandChildTasks, newNode, impendingTaskTreeTaskList ?? ViewHelperFunctions.GetImpendingTaskTreeTaskList(childTask.Id, _viewController));
                }
                nodeToAddTo.Nodes.Add(newNode);
            }
        }



        #region ICurrentViewDataObserver

        #region OnViewTaskFocusChanged

        public void OnViewTaskFocusChanged(object sender)
        {
            if (sender == this)
                return;
            SelectCurrentViewFocusTask();       
        }

        private void SelectCurrentViewFocusTask()
        {
            var viewTaskFocus = _viewController.GetViewTaskFocus();
            var focusedForestRootNode = this.treeView1.Nodes.Cast<TreeNode>().Single(tNode => (int)tNode.Tag == viewTaskFocus.FocusedForestId);
            if (viewTaskFocus.FocusedTaskId == -1)
            {
                if (this.treeView1.SelectedNode == focusedForestRootNode) return;
                _handlingTaskFocusEvent = true;
                this.treeView1.SelectedNode = focusedForestRootNode;
                return;
            }

            var focusedTask = _viewController.GetTask(viewTaskFocus.FocusedTaskId);
            var focusedTasksAncestors = new List<HodorTask>();
            var lastTask = focusedTask;
            while (lastTask.ParentId != HodorTask.PROJECT_TREE_ROOT_PARENT_ID)
            {
                lastTask = _viewController.GetTask(lastTask.ParentId);
                focusedTasksAncestors.Add(lastTask);
            }
            focusedTasksAncestors.Reverse();

            TreeNode currNode = focusedForestRootNode;
            if (!focusedForestRootNode.IsExpanded) focusedForestRootNode.Expand();

            foreach (var focusedTasksAncestor in focusedTasksAncestors)
            {
                var ancestor = focusedTasksAncestor;
                foreach (var node in currNode.Nodes.Cast<TreeNode>().Where(node => ((HodorTask) node.Tag).Id == ancestor.Id))
                {
                    if (!node.IsExpanded) node.Expand();
                    currNode = node;
                    break;
                }
            }
            foreach (var node in currNode.Nodes.Cast<TreeNode>().Where(node => ((HodorTask) node.Tag).Id == focusedTask.Id))
            {
                if (this.treeView1.SelectedNode == node) return;
                _handlingTaskFocusEvent = true;
                this.treeView1.SelectedNode = node;
                return;
            }
        }

         

        #endregion

        #region OnViewDataSetChanged

        public void OnViewDataSetChanged(object sender)
        {
            UpdateProjectForestRootNodes();
        }

        public void UpdateProjectForestRootNodes()
        {
            var displayedProjForests = _viewController.GetDisplayedprojectForests().ToList();
            
            //delete nodes of removed project forests from tree view
            //todo test
            var rootNodeProjectForestIds = this.treeView1.Nodes.Cast<TreeNode>().Select(treeNode => (int)treeNode.Tag);
            //todo test
            var removedProjectForestIds = rootNodeProjectForestIds.Except(displayedProjForests.Select(x => x.Id));
            foreach (var removedProjectForestId in removedProjectForestIds)
            {
                this.treeView1.Nodes.Remove(this.treeView1.Nodes.Cast<TreeNode>().Single(treeNode => removedProjectForestId == (int)treeNode.Tag));
            }

            //add displayed project forests missing from treeview
            foreach (var displayedProjForest in displayedProjForests)
            {
                var existsInTreeView = this.treeView1.Nodes.Cast<TreeNode>().Any(tNode => (int)tNode.Tag == displayedProjForest.Id);
                if (!existsInTreeView)
                {
                    AddProjectForestToTreeView(displayedProjForest);
                }
            }

            //fix order of rootnodes if it needs fixing
            for (var i = 0; i < displayedProjForests.Count; i++)
            {
                var displayedProjForest = displayedProjForests[i];
                for (var j = 0; j < this.treeView1.Nodes.Count; j++)
                {
                    if ((int)((TreeNode)this.treeView1.Nodes[j]).Tag != displayedProjForest.Id) continue;
                    var node = this.treeView1.Nodes[j];
                    if (i != j)
                    {
                        this.treeView1.Nodes.RemoveAt(j);
                        this.treeView1.Nodes.Insert(i, node);
                    }
                    if (!node.Text.Equals(displayedProjForest.Title))
                    {
                        node.Text = displayedProjForest.Title;
                    }
                    UpdateSubNodes(displayedProjForest.ProjectTreeRootIds.Select(_viewController.GetTask).ToList(), node, null);
                    break;
                }
            }
            
        }

        public void UpdateSubNodes(List<HodorTask> childTasks, TreeNode nodeToAddTo, List<HodorTask> impendingTaskTreeTaskList)
        {
            var toRemove = new List<TreeNode>();
            foreach (TreeNode node in nodeToAddTo.Nodes)
            {
                var exists = (from childTask in childTasks
                              let treeNodeTask = (HodorTask)node.Tag
                              where treeNodeTask.Id == childTask.Id
                              select childTask).Any();
                if (!exists)
                {
                    toRemove.Add(node);
                }
            }

            foreach (var treeNode in toRemove)
            {
                nodeToAddTo.Nodes.Remove(treeNode);
            }


            foreach (var childTask in childTasks)
            {
                TreeNode childTaskNode = null;
                foreach (TreeNode node in nodeToAddTo.Nodes)
                {
                    var treeNodeTask = (HodorTask)node.Tag;
                    if (treeNodeTask.Id == childTask.Id)
                    {
                        var setTask = false;
                        if (!node.Text.Equals(childTask.Title))
                        {
                            node.Text = childTask.Title;
                            setTask = true;
                        }
                        if (treeNodeTask.CurrentTaskStatus != childTask.CurrentTaskStatus)
                        {
                            ApplyTreeNodeImage(node, childTask);
                            setTask = true;
                        }
                        if (setTask)
                        {
                            node.Tag = childTask.Clone();
                        }
                        if (!ApplyImpendingStyle(node, childTask, impendingTaskTreeTaskList))
                        {
                            node.NodeFont = this.treeView1.Font;
                        }
                        childTaskNode = node;
                        break;
                    }
                }
                if (childTaskNode == null)
                {
                    childTaskNode = new TreeNode(childTask.Title) {Tag = childTask.Clone()};

                    ApplyImpendingStyle(childTaskNode, childTask, impendingTaskTreeTaskList);
                    ApplyTreeNodeImage(childTaskNode, childTask);

                    nodeToAddTo.Nodes.Insert(childTask.Ordinal, childTaskNode);

                }
                else
                {
                    if (nodeToAddTo.Nodes.IndexOf(childTaskNode) != childTask.Ordinal)
                    {
                        nodeToAddTo.Nodes.Remove(childTaskNode);
                        nodeToAddTo.Nodes.Insert(childTask.Ordinal, childTaskNode);
                    }

                }

                var grandChildTasks = _viewController.GetChildren(childTask.Id).ToList();
                if (grandChildTasks.Count() != 0)
                {
                    UpdateSubNodes(grandChildTasks, childTaskNode, impendingTaskTreeTaskList ?? ViewHelperFunctions.GetImpendingTaskTreeTaskList(childTask.Id, _viewController));
                }
            }
        }

        

        #endregion

        #endregion

        private bool ApplyImpendingStyle(TreeNode node, HodorTask task, List<HodorTask> impendingTaskTreeTaskList)
        {
            if (task.CurrentTaskStatus == HodorTaskStatus.Impending)
            {
                node.NodeFont = new Font(this.treeView1.Font, FontStyle.Underline | FontStyle.Italic | FontStyle.Bold);
                return true;
            }
            else if (impendingTaskTreeTaskList != null)
            {
                if (impendingTaskTreeTaskList.Exists(x => x.Id == task.Id))
                {
                    node.NodeFont = new Font(this.treeView1.Font, FontStyle.Underline);
                    return true;
                }
                return false;
            }
            return false;
        }

        private void ApplyTreeNodeImage(TreeNode node, HodorTask nodeTask)
        {
            node.ImageIndex = nodeTask.CurrentTaskStatus == HodorTaskStatus.Completed ? 0 : 1;
            node.SelectedImageIndex = nodeTask.CurrentTaskStatus == HodorTaskStatus.Completed ? 0 : 1;
        }
    }
}
