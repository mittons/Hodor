using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Contracts;
using Contracts.Dto;
using Contracts.ObserverPattern;
using ViewNavigation;

namespace HodorCustomWinForms.SessionDisplay
{
    public partial class HodorSessionInfoControl : UserControl, ICurrentViewDataObserver
    {
        private IViewController _viewController =
            ViewControllerFactory.GetViewControllerInstance();

        public HodorSessionInfoControl()
        {
            InitializeComponent();
            InitializeControlWithData();

        }

        private void HodorSessionInfoControl_Load(object sender, EventArgs e)
        {

        }

        private void InitializeControlWithData()
        {
            var currentSessionProjectForest = _viewController.GetCurrentSessionProjectForest();

            sessionTitleLabel.Text = "Current session - " + currentSessionProjectForest.Title;
 
            foreach (
                var impendingTaskTree in
                    currentSessionProjectForest.ProjectTreeRootIds.Select(
                        x => ViewHelperFunctions.GetImpendingTaskTree(x, _viewController))
                        .OfType<ImpendingTaskTreeStruct>())
            {
                InitializeSessionInfoListItem(impendingTaskTree);
            }

        }

        private void InitializeSessionInfoListItem(ImpendingTaskTreeStruct impendingTaskTree)
        {
            var sessItem = new HodorSessionInfoListItem();

            sessItem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            sessItem.Tag = impendingTaskTree.ImpendingTaskTreeTask.Id;

            InitializeImpendingTreeView(impendingTaskTree, sessItem);

            this.flowLayoutPanel1.Controls.Add(sessItem);
        }

        private void InitializeImpendingTreeView(ImpendingTaskTreeStruct impendingTaskTree,
            HodorSessionInfoListItem sessItem)
        {
            sessItem.treeView.ShowPlusMinus = false;
            sessItem.treeView.ShowLines = false;
            sessItem.treeView.ImageList = null;
            sessItem.treeView.BeforeCollapse += sessionItemTreeView_BeforeCollapse;
            sessItem.treeView.BeforeSelect += sessionItemTreeView_BeforeSelect;
            sessItem.treeView.TreeViewNodeSorter = new SessionInfoTreeNodeComparer();


            PopulateImpendingTreeView_Recursive(impendingTaskTree, sessItem.treeView.Nodes, sessItem.treeView.Font);
            sessItem.treeView.ExpandAll();

            var add = sessItem.treeView.GetNodeCount(true) * sessItem.treeView.ItemHeight;
            sessItem.treeView.Height = add + 4;
        }

        private void PopulateImpendingTreeView_Recursive(ImpendingTaskTreeStruct impendingTaskTree, TreeNodeCollection toAddTo,
            Font treeViewFont)
        {
            var node = new TreeNode() {Tag = impendingTaskTree.ImpendingTaskTreeTask};
            node.Text = impendingTaskTree.ImpendingTaskTreeTask.Title;
            toAddTo.Add(node);

            ApplyImpendingTreeViewNodeStyles(impendingTaskTree, treeViewFont, node);            
    

            foreach (var taskChild in impendingTaskTree.ImpendingTaskTreeTaskChildren)
            {
                PopulateImpendingTreeView_Recursive(taskChild, node.Nodes, treeViewFont);
            }
        }

        private static void ApplyImpendingTreeViewNodeStyles(ImpendingTaskTreeStruct impendingTaskTree, Font treeViewFont,
            TreeNode node)
        {
            if (impendingTaskTree.ImpendingTaskTreeTask.CurrentTaskStatus == HodorTaskStatus.Impending)
            {
                node.NodeFont = new Font(treeViewFont, FontStyle.Underline | FontStyle.Bold);
            }
            else
            {
                node.NodeFont = new Font(treeViewFont, FontStyle.Bold);
                node.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            }
        }

        private void ApplyImpendingTreeViewNodeStyles(TreeNode node, ImpendingTaskTreeStruct impendingTaskTree, Font treeViewFont)
        {
            
        }

        private void sessionItemTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void sessionItemTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        #region Horizontal resize-ing of flowLayoutPanel children

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            foreach (UserControl control in flowLayoutPanel1.Controls)
            {
                SetTaskListItemControlWidth(control);
            }
        }

        private void SetTaskListItemControlWidth(UserControl childOfFlowLayoutPanel)
        {
//            var vScrollBarWidth = this.flowLayoutPanel1.VerticalScroll.Visible
//                   ? System.Windows.Forms.SystemInformation.VerticalScrollBarWidth
//                   : 0;
            childOfFlowLayoutPanel.Width = flowLayoutPanel1.Width - 3; // - vScrollBarWidth;
        }

        #endregion


        #region #ICurrentViewDataObserver

        public void OnViewTaskFocusChanged(object sender)
        {
            // whatever, should probably split these two notify functions up
        }

        public void OnViewDataSetChanged(object sender)
        {
            this.flowLayoutPanel1.Controls.Clear();
            InitializeControlWithData();

        }

        public void ClearFlowlayoutPanel()
        {
            foreach (var ctrl in this.flowLayoutPanel1.Controls.OfType<HodorSessionInfoListItem>())
            {
                ctrl.treeView.BeforeCollapse -= sessionItemTreeView_BeforeCollapse;
                ctrl.treeView.BeforeSelect -= sessionItemTreeView_BeforeSelect;
            }
            this.flowLayoutPanel1.Controls.Clear();
        }


        #endregion

    }

    public class SessionInfoTreeNodeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var tx = x as TreeNode;
            var ty = y as TreeNode;

            var nodeTaskX = tx.Tag as HodorTask;
            var nodeTaskY = ty.Tag as HodorTask;
            if (nodeTaskX == null || nodeTaskY == null)
            {
                throw new SystemException("Added node(s) to HodorSessionInfoControl that are without HodorTask tag");
            }
            return nodeTaskX.Ordinal - nodeTaskY.Ordinal;
        }
    }
}
