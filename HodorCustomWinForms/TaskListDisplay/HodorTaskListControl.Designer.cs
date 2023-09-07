namespace HodorCustomWinForms.TaskListDisplay
{
    partial class HodorTaskListControl
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
            DisposeStuff();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.currentTaskContainer = new HodorTaskListItemControl();
            this.childTaskFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.viewParentTaskButton = new System.Windows.Forms.Button();
            this.createTaskControl = new HodorCreateTaskControl();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 900);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 900);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.currentTaskContainer);
            this.panel2.Controls.Add(this.childTaskFlowPanel);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(465, 770);
            this.panel2.TabIndex = 0;
            // 
            // currentTaskContainer
            // 
            this.currentTaskContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentTaskContainer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.currentTaskContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentTaskContainer.Location = new System.Drawing.Point(16, 45);
            this.currentTaskContainer.Name = "currentTaskContainer";
            this.currentTaskContainer.Size = new System.Drawing.Size(410, 53);
            this.currentTaskContainer.TabIndex = 2;
            // 
            // childTaskFlowPanel
            // 
            this.childTaskFlowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.childTaskFlowPanel.AutoScroll = true;
            this.childTaskFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.childTaskFlowPanel.Location = new System.Drawing.Point(16, 141);
            this.childTaskFlowPanel.Name = "childTaskFlowPanel";
            this.childTaskFlowPanel.Size = new System.Drawing.Size(413, 619);
            this.childTaskFlowPanel.TabIndex = 0;
            this.childTaskFlowPanel.WrapContents = false;
            this.childTaskFlowPanel.SizeChanged += new System.EventHandler(this.childTaskFlowPanel_SizeChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Child tasks";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(108, 22);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current task";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.viewParentTaskButton);
            this.panel3.Controls.Add(this.createTaskControl);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 779);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(465, 118);
            this.panel3.TabIndex = 1;
            // 
            // viewParentTaskButton
            // 
            this.viewParentTaskButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewParentTaskButton.Location = new System.Drawing.Point(16, 59);
            this.viewParentTaskButton.Name = "viewParentTaskButton";
            this.viewParentTaskButton.Size = new System.Drawing.Size(410, 53);
            this.viewParentTaskButton.TabIndex = 5;
            this.viewParentTaskButton.Text = "View parent task";
            this.viewParentTaskButton.UseVisualStyleBackColor = true;
            this.viewParentTaskButton.Click += new System.EventHandler(this.viewParentTaskButton_Click);
            // 
            // createTaskControl
            // 
            this.createTaskControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.createTaskControl.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.createTaskControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.createTaskControl.Location = new System.Drawing.Point(16, 0);
            this.createTaskControl.Name = "createTaskControl";
            this.createTaskControl.Size = new System.Drawing.Size(410, 53);
            this.createTaskControl.TabIndex = 1;
            // 
            // HodorTaskListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "HodorTaskListControl";
            this.Size = new System.Drawing.Size(471, 900);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel childTaskFlowPanel;
        private HodorCreateTaskControl createTaskControl;
        private System.Windows.Forms.Label label1;
        private HodorTaskListItemControl currentTaskContainer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button viewParentTaskButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}