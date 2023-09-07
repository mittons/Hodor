using System.Windows.Forms;
using HodorData;
using ViewNavigation;

namespace HodorCustomWinForms.MainWindowForms
{
    public partial class MainHodorWindow : Form
    {
        public MainHodorWindow()
        {
            InitializeComponent();
        }

        private void openTaskTreesTextViewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var vc = ViewControllerFactory.GetViewControllerInstance();
            vc.DisplayProjectForestInTextEditor(vc.GetViewTaskFocus().FocusedForestId);
        }
    }
}
