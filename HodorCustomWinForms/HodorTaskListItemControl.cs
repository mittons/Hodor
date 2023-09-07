using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Contracts;
using Contracts.Dto;
using HodorData;
using ViewNavigation;

namespace HodorCustomWinForms
{
    public partial class HodorTaskListItemControl : UserControl
    {
        private int? _displayedTaskId;
        private bool _isDisplayChildrenClickEnabled = false;

        public HodorTaskListItemControl()
        {
            InitializeComponent();
        }

        public HodorTaskListItemControl(HodorTask displayedTask, bool isDisplayChildrenClickEnabled)
        {
            InitializeComponent();
            this.SetData(displayedTask, isDisplayChildrenClickEnabled);
        }

        public void SetBackgroundColor(Color bgColor)
        {
            this.BackColor = bgColor;
        }

        public void SetData(HodorTask task, bool isDisplayChildrenClickEnabled)
        {
            this._displayedTaskId = task.Id;
            SetDisplayData(task.Title, task.CurrentTaskStatus == HodorTaskStatus.Completed, isDisplayChildrenClickEnabled, true, true);
        }

        public void DisplayProjectForestInfo(string projectForestTitle)
        {
            this._displayedTaskId = null;
            SetDisplayData(projectForestTitle, false, false, false, false);
        }

        private void SetDisplayData(string title, bool isCheckboxChecked, bool isDisplayChildrenClickEnabled, bool isViewDetailsButtonEnabled,
            bool isCheckboxEnabled)
        {
            titleLabel.Text = title;

            this.checkBox1.CheckedChanged -= checkBox1_CheckedChanged;
            this.checkBox1.Checked = isCheckboxChecked;
            this.checkBox1.CheckedChanged += checkBox1_CheckedChanged;

            this._isDisplayChildrenClickEnabled = isDisplayChildrenClickEnabled;
            
            this.SetBackgroundColor(isDisplayChildrenClickEnabled ? Color.White : Color.WhiteSmoke);

            this.viewDetailsButton.Enabled = isViewDetailsButtonEnabled;

            this.checkBox1.Enabled = isCheckboxEnabled;
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            if (_isDisplayChildrenClickEnabled)
            {
                Debug.Assert(_displayedTaskId != null, "_displayedTaskId != null");
                ViewControllerFactory.GetViewControllerInstance().SetFocusedTaskGivenFocusedForest((int)_displayedTaskId, this);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Debug.Assert(_displayedTaskId != null, "_displayedTaskId != null");
            ViewControllerFactory.GetViewControllerInstance()
                .UpdateTask((int)_displayedTaskId, this,
                    updatedStatus: checkBox1.Checked ? HodorTaskStatus.Completed : HodorTaskStatus.Todo);
            //_displayedTask.IsCompleted = checkBox1.Checked;
        }

        //Todo make sure this doesnt leak
        #region viewTaskDetailsDelegation

        private void viewDetailsButton_Click(object sender, EventArgs e)
        {
            if (_displayedTaskId != null)
            {
                ViewControllerFactory.GetViewControllerInstance().DisplayTaskDetailsInTextEditor((int)_displayedTaskId);
            }
        }
        #endregion
    }
}
