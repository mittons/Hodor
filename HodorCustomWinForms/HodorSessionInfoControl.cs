using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contracts;
using ViewNavigation;

namespace HodorCustomWinForms
{
    public partial class HodorSessionInfoControl : UserControl
    {
        private IViewController _viewController =
            ViewControllerFactory.GetViewControllerInstance();

        public HodorSessionInfoControl()
        {
            InitializeComponent();
            var testForest = _viewController.GetProjectForest(0);
            foreach (var impendingTaskTree in testForest.ProjectTreeRootIds.Select(x => ViewHelperFunctions.GetImpendingTaskTree(x, _viewController)).OfType<ImpendingTaskTreeStruct>())
            {
                var sessItem = new HodorSessionInfoListItem();
                sessItem.Tag = impendingTaskTree.ImpendingTaskTreeTask.Id;
                //sessItem.treeView.
//                sessItem.treeView.Nodes;
            }
        }


        /*
        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void AddProjectForestToTreeView(IHodorProjectForest projectForest)
        {
            var projectForestRootNode = new TreeNode(projectForest.Title) { Tag = projectForest.Id };
            AddSubTaskNodes(projectForest.ProjectTreeRootIds.Select(_viewController.GetTask), projectForestRootNode);
            this.treeView1.Nodes.Add(projectForestRootNode);
            this.treeView1.ShowLines = false;
            this.treeView1.ImageList = null;
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ExpandAll();
            this.treeView1.BeforeCollapse += treeView1_BeforeCollapse;
        }
         */
    }
}
