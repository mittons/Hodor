using HodorCustomWinForms.TaskListDisplay;
using HodorCustomWinForms.TextEditorDisplay;
using HodorCustomWinForms.TreeViewExplorerDisplay;

namespace HodorCustomWinForms.MainWindowDisplay
{
    partial class EndBossFinalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EndBossFinalForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.hodorTreeView1 = new HodorTreeView();
            this.taskListControl = new HodorTaskListControl();
            this.textEditorContainerControl1 = new HodorCustomWinForms.TextEditorDisplay.TextEditorContainerControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1595, 900);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.hodorTreeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1595, 900);
            this.splitContainer1.SplitterDistance = 426;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.taskListControl);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textEditorContainerControl1);
            this.splitContainer2.Size = new System.Drawing.Size(1165, 900);
            this.splitContainer2.SplitterDistance = 475;
            this.splitContainer2.TabIndex = 2;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "normal_folder.png");
            this.imageList1.Images.SetKeyName(1, "document_icon.jpg");
            // 
            // hodorTreeView1
            // 
            this.hodorTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hodorTreeView1.Location = new System.Drawing.Point(0, 0);
            this.hodorTreeView1.Name = "hodorTreeView1";
            this.hodorTreeView1.Size = new System.Drawing.Size(426, 900);
            this.hodorTreeView1.TabIndex = 0;
            // 
            // taskListControl
            // 
            this.taskListControl.Location = new System.Drawing.Point(0, 0);
            this.taskListControl.Name = "taskListControl";
            this.taskListControl.Size = new System.Drawing.Size(471, 900);
            this.taskListControl.TabIndex = 1;
            // 
            // textEditorContainerControl1
            // 
            this.textEditorContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditorContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.textEditorContainerControl1.Name = "textEditorContainerControl1";
            this.textEditorContainerControl1.Size = new System.Drawing.Size(686, 900);
            this.textEditorContainerControl1.TabIndex = 0;
            // 
            // EndBossFinalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1595, 900);
            this.Controls.Add(this.panel1);
            this.Name = "EndBossFinalForm";
            this.Text = "EndBossFinalForm";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private HodorTreeView hodorTreeView1;
        private System.Windows.Forms.ImageList imageList1;
        private HodorTaskListControl taskListControl;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private TextEditorContainerControl textEditorContainerControl1;
    }
}