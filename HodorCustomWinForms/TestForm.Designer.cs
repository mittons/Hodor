using HodorCustomWinForms.SessionDisplay;

namespace HodorCustomWinForms
{
    partial class TestForm
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
            this.hodorSessionInfoControl1 = new HodorSessionInfoControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // hodorSessionInfoControl1
            // 
            this.hodorSessionInfoControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hodorSessionInfoControl1.AutoSize = true;
            this.hodorSessionInfoControl1.BackColor = System.Drawing.SystemColors.Window;
            this.hodorSessionInfoControl1.Location = new System.Drawing.Point(0, 0);
            this.hodorSessionInfoControl1.Name = "hodorSessionInfoControl1";
            this.hodorSessionInfoControl1.Size = new System.Drawing.Size(584, 621);
            this.hodorSessionInfoControl1.TabIndex = 0;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 611);
            this.Controls.Add(this.hodorSessionInfoControl1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HodorSessionInfoControl hodorSessionInfoControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}