using System;
using System.Windows.Forms;

namespace HodorCustomWinForms.TaskListDisplay
{
    public partial class HodorCreateTaskControl : UserControl
    {
        public HodorCreateTaskControl()
        {
            InitializeComponent();
        }

        #region CreateTaskDelegation
        public delegate void OnCreateTaskDelegate(string title);

        public event OnCreateTaskDelegate OnCreateTask;
        #endregion


        private void createTaskButton_Click(object sender, EventArgs e)
        {
            if (OnCreateTask != null)
            {
                OnCreateTask(this.textBox1.Text);
                this.textBox1.Text = "";
            }
        }

    }
}
