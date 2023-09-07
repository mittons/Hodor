using System.Windows.Forms;
using ViewNavigation;

namespace HodorCustomWinForms.MainWindowDisplay
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
