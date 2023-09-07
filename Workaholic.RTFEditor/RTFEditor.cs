// .NET references
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

[assembly: CLSCompliant(true)]
namespace Workaholic.RTFEditor
{
    // biriwork
    // - find and replace muveletek
    // - insert image muvelet
    // - wordwrap ki/be kapcsolhatosag

    // Felhasznalo altal allithato vagy lekerdezheto ertekek, a control megjelenitese elott
    // - a felulet nyelve
    // - isDirty (Modified)

    /*
    
    http://www.codeproject.com/cs/miscctrl/CsExRichTextBox.asp
    public void InsertImage()  {
  ...
  string lstrFile = fileDialog.FileName;
  Bitmap myBitmap = new Bitmap(lstrFile);
  // Copy the bitmap to the clipboard.
  Clipboard.SetDataObject(myBitmap);
  // Get the format for the object type.
  DataFormats.Format myFormat = DataFormats.GetFormat (DataFormats.Bitmap);
  // After verifying that the data can be pasted, paste
  if(NoteBox.CanPaste(myFormat)) {
    NoteBox.Paste(myFormat);
  }
  else {
    MessageBox.Show("The data format that you attempted site" + 
      " is not supportedby this control.");
  }
  ...
}
*/

    /// <summary>
    /// Base class for RTFEditor control
    /// </summary>
    public partial class RTFEditor : UserControl
    {
        #region Event handlers

        [Description("Occurs when the Refresh command is issued.")]
        public event RTFEditorOnRefresh OnDocumentRefresh;

        [Description("Occurs when the Save command is issued.")]
        public event RTFEditorOnSave OnDocumentSave;

        #endregion Event handlers

        #region Input/output variables and properties

        /// <summary>
        /// Gets or sets the document text.
        /// </summary>
        /// <value>The document text.</value>
        [Description("Default document text in plain text format.")]
        public String DocumentText
        {
            get { return this.rtb.Text; }
            set { this.rtb.Text = value; }
        }


        public void SetDocumentText(string documentText)
        {
            this.DocumentText = documentText;
            this.rtb.SelectionStart = 0;
            this.rtb.SelectionLength = this.rtb.TextLength;
            this.TabSizeInPixels = _tabSizeInPixels;
            this.rtb.SelectionLength = 0;
        }

        /// <summary>
        /// Gets or sets the document RTF.
        /// </summary>
        /// <value>The document RTF.</value>
        [Description("Default document text in RTF format.")]
        public String DocumentRtf
        {
            get { return this.rtb.Rtf; }
            set { this.rtb.Rtf = value; }
        }

        /// <summary>
        /// Gets or sets the default font family.
        /// </summary>
        /// <value>The default font family.</value>
        private String defaultFontFamily = "Arial";
        [Description("Default font style.")]
        public String DefaultFontFamily
        {
            get { return this.defaultFontFamily; }
            set { this.defaultFontFamily = value; }
        }

        /// <summary>
        /// Gets or sets the size of the default font.
        /// </summary>
        /// <value>The size of the default font.</value>
        private int defaultFontSize = 8;
        [Description("Default font size.")]
        public int DefaultFontSize
        {
            get { return this.defaultFontSize; }
            set { this.defaultFontSize = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [file panel visible].
        /// </summary>
        /// <value><c>true</c> if [file panel visible]; otherwise, <c>false</c>.</value>
        [Description("The file operation (new, open, save) panel is visible (true) or hidden (false).")]
        public bool FilePanelVisible
        {
            get
            {
                if (this.tsNew.Visible || this.newToolStripMenuItem.Visible)
                { return true; }
                else { return false; }
            }
            set
            {
                this.tsNew.Visible = value;
                this.tsOpen.Visible = value;
                this.tsSave.Visible = value;
                this.tsFileSeparator.Visible = value;

                this.newToolStripMenuItem.Visible = value;
                this.openToolStripMenuItem.Visible = value;
                this.saveToolStripMenuItem.Visible = value;
                this.fileToolStripMenuItem.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable new document].
        /// </summary>
        /// <value><c>true</c> if [enable new document]; otherwise, <c>false</c>.</value>
        [Description("New document operation is enabled (true) or not (false).")]
        public bool EnableNewDocument
        {
            get
            {
                if (this.tsNew.Visible || this.newToolStripMenuItem.Visible)
                { return true; }
                else { return false; }
            }
            set
            {
                this.tsNew.Visible = value;
                this.newToolStripMenuItem.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable refresh document].
        /// </summary>
        /// <value><c>true</c> if [enable refresh document]; otherwise, <c>false</c>.</value>
        [Description("Refresh document operation is enabled (true) or not (false).")]
        public bool EnableRefreshDocument
        {
            get
            {
                if (this.tsOpen.Visible || this.openToolStripMenuItem.Visible)
                { return true; }
                else { return false; }
            }
            set
            {
                this.tsOpen.Visible = value;
                this.openToolStripMenuItem.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable save document].
        /// </summary>
        /// <value><c>true</c> if [enable save document]; otherwise, <c>false</c>.</value>
        [Description("Save document operation is enabled (true) or not (false).")]
        public bool EnableSaveDocument
        {
            get
            {
                if (this.tsSave.Visible || this.saveToolStripMenuItem.Visible)
                { return true; }
                else { return false; }
            }
            set
            {
                this.tsSave.Visible = value;
                this.saveToolStripMenuItem.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [default word wrap].
        /// </summary>
        /// <value><c>true</c> if [default word wrap]; otherwise, <c>false</c>.</value>
        private bool defaultWordWrap = false;
        [Description("Editor should use word wrap (true) or not (false).")]
        public bool DefaultWordWrap
        {
            get { return this.defaultWordWrap; }
            set { this.defaultWordWrap = value; }
        }

        /// <summary>
        /// Gets or sets the color of the default background.
        /// </summary>
        /// <value>The color of the default background.</value>
        private Color defaultBackgroundColor = Color.White;
        [Description("Default background color.")]
        public Color DefaultBackgroundColor
        {
            get { return this.defaultBackgroundColor; }
            set { this.defaultBackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the default font.
        /// </summary>
        /// <value>The color of the default font.</value>
        private Color defaultFontColor = Color.Black;
        [Description("Default foreground (font) color.")]
        public Color DefaultFontColor
        {
            get { return this.defaultFontColor; }
            set { this.defaultFontColor = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        [Description("The text in the editor is read only (true) or writeable (false).")]
        public Boolean ReadOnly
        {
            get { return this.rtb.ReadOnly; }
            set { this.rtb.ReadOnly = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [tool strip visible].
        /// </summary>
        /// <value><c>true</c> if [tool strip visible]; otherwise, <c>false</c>.</value>
        [Description("The toolstrip of the editor is visible (true) or not (false).")]
        public Boolean ToolStripVisible
        {
            get { return this.ts.Visible; }
            set { this.ts.Visible = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [menu visible].
        /// </summary>
        /// <value><c>true</c> if [menu visible]; otherwise, <c>false</c>.</value>
        [Description("The menu of the editor is visible (true) or not (false).")]
        public Boolean MenuVisible
        {
            get { return this.ms.Visible; }
            set { this.ms.Visible = value; }
        }


        private int _tabSizeInPixels = -1;
        /// <summary>
        /// Setter for tab size - measured in pixels.
        /// </summary>
        /// <value>New tab size in pixels.</value>
        [Description("Setter for tab size - measured in pixels")]
        public int TabSizeInPixels
        {
            set
            {
                _tabSizeInPixels = value; 
                this.rtb.SelectionTabs = new int[] { 1 * value, 2 * value, 3 * value, 4 * value, 5 * value, 6 * value, 7 * value, 8 * value, 9 * value }; 
            }
        }

        /// <summary>
        /// Enables or disables using the tab key as text input for the rich text box
        /// </summary>
        /// <value><c>true</c> if tab character can be entered into the rich text box; otherwise, <c>false</c>.</value>
        [Description("Setter for tab size - measured in pixels")]
        public bool RichTextBoxAcceptsTab
        {
            get { return this.rtb.AcceptsTab; }
            set { this.rtb.AcceptsTab = value; }
        }

        #endregion Input/output variables and properties

        #region Control constructor and load event

        /// <summary>
        /// Initializes a new instance of the <see cref="RTFEditor"/> class.
        /// </summary>
        public RTFEditor()
        {
            InitializeComponent();
        }

        private string _initialText = null;

        public string InitialText
        {
            get { return _initialText; }
            set { _initialText = value; } 
            
        }

        /// <summary>
        /// Handles the Load event of the RTFEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RTFEditor_Load(object sender, EventArgs e)
        {
            // Set up default values
            this.rtb.AllowDrop = true;

            // Set up default values defined as user-changable
            this.tsFontSize.SelectedText = this.defaultFontSize.ToString();
            this.tsFontType.Text = this.defaultFontFamily;
            this.rtb.WordWrap = this.defaultWordWrap;

            if (String.IsNullOrEmpty(this.rtb.Text))
            {
                this.rtb.Font = new Font(this.defaultFontFamily, this.defaultFontSize, FontStyle.Regular);
            }

            this.tsFontColor.BackColor = this.defaultBackgroundColor;
            this.tsFontColor.ForeColor = this.tsFontColor.BackColor;
            this.tsBackgroundColor.BackColor = this.defaultFontColor;
            this.tsBackgroundColor.ForeColor = this.tsBackgroundColor.BackColor;

            if (_initialText != null)
            {
                this.DocumentText = _initialText;
            }

            _currentText = this.rtb.Text;
            _undoStack = new List<string>();
            _redoStack = new List<string>();

            // Default language

            rtb.Focus();

            
        }

        #endregion Control constructor and load event

        #region Selection changed event

        /// <summary>
        /// Handles the SelectionChanged event of the rtb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rtb_SelectionChanged(object sender, EventArgs e)
        {
            // Font style, family and size
            if (this.rtb.SelectionFont != null)
            {
                this.tsBold.Checked = this.rtb.SelectionFont.Bold;
                this.boldToolStripMenuItem.Checked = this.rtb.SelectionFont.Bold;
                this.tsItalic.Checked = this.rtb.SelectionFont.Italic;
                this.italicToolStripMenuItem.Checked = this.rtb.SelectionFont.Italic;
                this.tsUnderline.Checked = this.rtb.SelectionFont.Underline;
                this.underlineToolStripMenuItem.Checked = this.rtb.SelectionFont.Underline;
                this.tsStrikeout.Checked = this.rtb.SelectionFont.Strikeout;
                this.strikeoutToolStripMenuItem.Checked = this.rtb.SelectionFont.Strikeout;

                this.tsFontType.Text = this.rtb.SelectionFont.Name;
                this.tsFontType.ToolTipText = this.rtb.SelectionFont.Name;
                double fontSize = Math.Round(this.rtb.SelectionFont.Size);
                this.tsFontSize.Text = fontSize.ToString();
            }

            // Alignment
            this.tsRight.Checked = (this.rtb.SelectionAlignment == HorizontalAlignment.Right);
            this.rightToolStripMenuItem.Checked = (this.rtb.SelectionAlignment == HorizontalAlignment.Right);
            this.tsLeft.Checked = (this.rtb.SelectionAlignment == HorizontalAlignment.Left);
            this.leftToolStripMenuItem.Checked = (this.rtb.SelectionAlignment == HorizontalAlignment.Left);
            this.tsCenter.Checked = (this.rtb.SelectionAlignment == HorizontalAlignment.Center);
            this.centerToolStripMenuItem.Checked = (this.rtb.SelectionAlignment == HorizontalAlignment.Center);

            // Font color and background color
            this.tsFontColor.BackColor = this.rtb.SelectionColor;
            this.tsFontColor.ForeColor = this.rtb.SelectionColor;
            this.tsBackgroundColor.BackColor = this.rtb.SelectionBackColor;
            this.tsBackgroundColor.ForeColor = this.rtb.SelectionBackColor;

            // Bulleting
            this.tsBullet.Checked = this.rtb.SelectionBullet;
        }

        #endregion Selection changed event

        #region File management

        /// <summary>
        /// Handles the Click event of the tsNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsNew_Click(object sender, EventArgs e)
        {
            if (!this.EnableNewDocument)
            {
                return;
            }

            // biriwork - isDirty vizsgalat
            this.rtb.Clear();
        }

        /// <summary>
        /// Handles the Click event of the tsRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsRefresh_Click(object sender, EventArgs e)
        {
            if (!this.EnableRefreshDocument)
            {
                return;
            }

            // biriwork - isDirty vizsgalat

            // Fire open event if we have a subscription
            RTFEditorEventArgs ea = new RTFEditorEventArgs();
            ea.Cancel = false;
            if (OnDocumentRefresh != null)
            {
                OnDocumentRefresh(this, ea);
            }

            if (!ea.Cancel)
            {
                // Open file and send it to the RTF editor
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(ofd.FileName))
                    {
                        try
                        {
                            this.rtb.LoadFile(ofd.FileName);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                this.rtb.LoadFile(ofd.FileName, RichTextBoxStreamType.PlainText);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the tsSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsSave_Click(object sender, EventArgs e)
        {
            if (!this.EnableSaveDocument)
            {
                return;
            }

            // biriwork - isdirty torlese

            // Fire save event if we have a subscription
            RTFEditorEventArgs ea = new RTFEditorEventArgs();
            ea.Cancel = false;
            if (OnDocumentSave != null)
            {
                OnDocumentSave(this, ea);
            }

            if (!ea.Cancel)
            {
                // Save file from RTF editor
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(sfd.FileName))
                    {
                        this.rtb.SaveFile(sfd.FileName);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the newToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsNew_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the refreshToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsRefresh_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the saveToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsSave_Click(sender, e);
        }

        #endregion File management

        #region Clipboard management

        /// <summary>
        /// Handles the Click event of the tsCut control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsCut_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.Cut();
        }

        /// <summary>
        /// Handles the Click event of the tsCopy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsCopy_Click(object sender, EventArgs e)
        {
            this.rtb.Copy();
        }

        /// <summary>
        /// Handles the Click event of the tsPaste control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsPaste_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.Paste();
        }

        /// <summary>
        /// Handles the Click event of the cutToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsCut_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the copyToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsCopy_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the pasteToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsPaste_Click(sender, e);
        }

        #endregion Clipboard management

        #region Font style management

        /// <summary>
        /// Handles the Click event of the tsBold control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsBold_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(!this.rtb.SelectionFont.Bold, this.rtb.SelectionFont.Italic, this.rtb.SelectionFont.Underline, this.rtb.SelectionFont.Strikeout);
        }

        /// <summary>
        /// Handles the Click event of the tsItalic control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsItalic_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(this.rtb.SelectionFont.Bold, !this.rtb.SelectionFont.Italic, this.rtb.SelectionFont.Underline, this.rtb.SelectionFont.Strikeout);
        }

        /// <summary>
        /// Handles the Click event of the tsUnderline control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsUnderline_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(this.rtb.SelectionFont.Bold, this.rtb.SelectionFont.Italic, !this.rtb.SelectionFont.Underline, this.rtb.SelectionFont.Strikeout);
        }

        /// <summary>
        /// Handles the Click event of the tsStrikeout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsStrikeout_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(this.rtb.SelectionFont.Bold, this.rtb.SelectionFont.Italic, this.rtb.SelectionFont.Underline, !this.rtb.SelectionFont.Strikeout);
        }

        /// <summary>
        /// Handles the Click event of the strikeoutToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void strikeoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsStrikeout_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the underlineToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void underlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsUnderline_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the italicToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsItalic_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the boldToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsBold_Click(sender, e);
        }

        /// <summary>
        /// Changes the font style.
        /// </summary>
        /// <param name="fontBold">if set to <c>true</c> [font bold].</param>
        /// <param name="fontItalic">if set to <c>true</c> [font italic].</param>
        /// <param name="fontUnderline">if set to <c>true</c> [font underline].</param>
        /// <param name="fontStrikeout">if set to <c>true</c> [font strikeout].</param>
        private void ChangeFontStyle(bool fontBold, bool fontItalic, bool fontUnderline, bool fontStrikeout)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            int fontStyle = 0;

            if (fontBold) { fontStyle += (int)FontStyle.Bold; }
            if (fontItalic) { fontStyle += (int)FontStyle.Italic; }
            if (fontUnderline) { fontStyle += (int)FontStyle.Underline; }
            if (fontStrikeout) { fontStyle += (int)FontStyle.Strikeout; }

            this.rtb.SelectionFont = new Font(this.rtb.SelectionFont, (FontStyle)fontStyle);
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        #endregion Font style management

        #region Alignment management

        /// <summary>
        /// Handles the Click event of the tsLeft control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsLeft_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionAlignment = HorizontalAlignment.Left;
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        /// <summary>
        /// Handles the Click event of the tsCenter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsCenter_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionAlignment = HorizontalAlignment.Center;
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        /// <summary>
        /// Handles the Click event of the tsRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsRight_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionAlignment = HorizontalAlignment.Right;
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        /// <summary>
        /// Handles the Click event of the leftToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsLeft_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the centerToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsCenter_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the rightToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsRight_Click(sender, e);
        }

        #endregion Alignment management

        #region Font type and size change

        /// <summary>
        /// Handles the SelectedIndexChanged event of the tsFontSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeFontTypeAndSize(this.rtb.SelectionFont.FontFamily, Int32.Parse(this.tsFontSize.SelectedItem.ToString()));
        }

        /// <summary>
        /// Handles the Leave event of the tsFontSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsFontSize_Leave(object sender, EventArgs e)
        {
            try
            {
                ChangeFontTypeAndSize(this.rtb.SelectionFont.FontFamily, Int32.Parse(this.tsFontSize.Text));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the tsFontSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void tsFontSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeFontTypeAndSize(this.rtb.SelectionFont.FontFamily, Int32.Parse(this.tsFontSize.Text));
            }
        }

        /// <summary>
        /// Handles the Click event of the tsFontType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsFontType_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            FontDialog fontDlg = new FontDialog();
            fontDlg.Font = this.rtb.SelectionFont;
            fontDlg.ShowColor = true;
            if (fontDlg.ShowDialog() == DialogResult.OK)
            {
                this.rtb.SelectionFont = fontDlg.Font;
                this.rtb.SelectionColor = fontDlg.Color;

                rtb_SelectionChanged(rtb, new EventArgs());
            }
        }

        /// <summary>
        /// Handles the Click event of the selectfontToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void selectfontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsFontType_Click(sender, e);
        }

        /// <summary>
        /// Changes the size of the font type and.
        /// </summary>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        private void ChangeFontTypeAndSize(FontFamily fontFamily, int fontSize)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            // New font
            try
            {
                Font myFont = new Font(fontFamily, fontSize, this.rtb.SelectionFont.Style);
                this.rtb.SelectionFont = myFont;
                rtb_SelectionChanged(rtb, new EventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Font type and size change

        #region Font and background color

        /// <summary>
        /// Handles the Click event of the tsFontColor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsFontColor_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionColor = this.tsFontColor.BackColor;
        }

        /// <summary>
        /// Handles the Click event of the tsBackgroundColor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsBackgroundColor_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionBackColor = this.tsBackgroundColor.BackColor;
        }

        /// <summary>
        /// Handles the Click event of the tsSelectFontColor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsSelectFontColor_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = this.rtb.SelectionColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                this.rtb.SelectionColor = colorDlg.Color;
                rtb_SelectionChanged(rtb, new EventArgs());
            }
        }

        /// <summary>
        /// Handles the Click event of the tsSelectBackgroundColor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsSelectBackgroundColor_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = this.rtb.SelectionBackColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                this.rtb.SelectionBackColor = colorDlg.Color;
                rtb_SelectionChanged(rtb, new EventArgs());
            }
        }

        #endregion Font and background color

        #region Bulleting

        /// <summary>
        /// Handles the Click event of the tsBullet control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsBullet_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionBullet = !this.rtb.SelectionBullet;
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        #endregion Bulleting

        #region Text identing

        /// <summary>
        /// Handles the Click event of the tsIdentPlus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsIdentPlus_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            if (this.rtb.SelectionAlignment == HorizontalAlignment.Left)
            {
                this.rtb.SelectionIndent += 36;
            }
        }

        /// <summary>
        /// Handles the Click event of the tsIdentMinus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsIdentMinus_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            if (this.rtb.SelectionAlignment == HorizontalAlignment.Left && this.rtb.SelectionIndent >= 36)
            {
                this.rtb.SelectionIndent -= 36;
            }
        }

        #endregion Text identing

        #region Undo/redo

        /// <summary>
        /// Handles the Click event of the tsUndo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsUndo_Click(object sender, EventArgs e)
        {
//            if (this.rtb.ReadOnly)
//            {
//                return;
//            }
//
//            this.rtb.Undo();
            this.Undo();
        }

        /// <summary>
        /// Handles the Click event of the tsRedo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsRedo_Click(object sender, EventArgs e)
        {
//            if (this.rtb.ReadOnly)
//            {
//                return;
//            }
//
//            this.rtb.Redo();
            this.Redo();
        }

        /// <summary>
        /// Handles the Click event of the undoToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsUndo_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the redoToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsRedo_Click(sender, e);
        }

        #endregion Undo/redo

        #region Undo/redo extended

        private List<string> _undoStack = null;
        private List<string> _redoStack = null;
        private bool _inUndo = false;
        private bool _inRedo = false;

        private string _currentText = null;

        private void rtb_TextChanged(object sender, EventArgs e)
        {
            if (_inUndo || _inRedo) return;

            if (_undoStack == null) return;

            UndoRedoHandleTextChanged(((RichTextBox)sender).Text);
        }

        private void UndoRedoHandleTextChanged(string text)
        {
            _undoStack.Add(_currentText);
            _currentText = text;
            _redoStack.Clear();
        }

        private void Redo()
        {
            if (CanRedo()) Redo_Core();
        }

        private void Redo_Core()
        {
            if (!CanRedo()) return;

            _inRedo = true;

            _undoStack.Add(this.rtb.Text);
            var idx = _redoStack.Count - 1;
            this.SetDocumentText(_redoStack[idx]);
            _currentText = _redoStack[idx];
            _redoStack.RemoveAt(idx);

            _inRedo = false;
        }

        private bool CanRedo()
        {
            return _redoStack.Count > 0;
        }

        private void Undo()
        {
            if (CanUndo()) Undo_Core();
        }

        private void Undo_Core()
        {
            if (!CanUndo()) return;

            _inUndo = true;

            _redoStack.Add(_currentText);
            var idx = _undoStack.Count - 1;
            this.SetDocumentText(_undoStack[idx]);
            _currentText = _undoStack[idx];
            _undoStack.RemoveAt(idx);

            _inUndo = false;
        }

        private bool CanUndo()
        {
            return (_undoStack.Count > 0);
        }

        #endregion

        #region Zoom in/out

        /// <summary>
        /// Handles the Leave event of the tsZoom control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsZoom_Leave(object sender, EventArgs e)
        {
            try
            {
                ChangeZoomValue(this.tsZoom.Text);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the tsZoom control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeZoomValue(this.tsZoom.SelectedItem.ToString());
        }

        /// <summary>
        /// Changes the zoom value.
        /// </summary>
        /// <param name="zoomValue">The zoom value.</param>
        private void ChangeZoomValue(String zoomValue)
        {
            // Write back to control
            if (!zoomValue.Contains("%"))
            {
                this.tsZoom.Text = zoomValue + "%";
            }

            // Strip percentage symbol
            zoomValue = zoomValue.Replace("%", "");

            try
            {
                this.rtb.ZoomFactor = float.Parse(zoomValue) / 100;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the tsZoom control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void tsZoom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeZoomValue(this.tsZoom.Text);
            }
        }

        #endregion Zoom in/out

        private void rtb_KeyDown(object sender, KeyEventArgs e)
        {

            var lines = this.rtb.Lines;
            
            var rtbSelStart = this.rtb.SelectionStart;
            var rtbSelLen = this.rtb.SelectionLength;
            var rtbSelEnd = rtbSelStart + rtbSelLen;

           var currCharIndex = 0;
            var selStartLineIdx = -1;
            var selEndLineIdx = -1;
            var startLineFound = false;
            var xtraEndLineWithNoCharSel = false;

            for (int i = 0; i < lines.Length; i++)
            {
                currCharIndex += lines[i].Length + 1;
                if (!startLineFound && rtbSelStart < currCharIndex)
                {
                    selStartLineIdx = i;
                    startLineFound = true;
                }
                if (startLineFound && rtbSelEnd <= currCharIndex)
                {
                    selEndLineIdx = i;
                    xtraEndLineWithNoCharSel = rtbSelEnd == currCharIndex;
                    break;
                }
            }
            //todo add controlchar to this
            //If we are dealing we a case that matches the default textbox behaviour, let the default code handle it.
            if (selStartLineIdx == selEndLineIdx && !xtraEndLineWithNoCharSel && (e.Modifiers != Keys.Shift)) return;


            if (e.KeyCode == Keys.Tab)
            {
                var multiLineSelect = true;
                //update tabbing
                if (e.Modifiers == Keys.Shift)
                {
                    if (selStartLineIdx == selEndLineIdx && !xtraEndLineWithNoCharSel)
                    {
                        multiLineSelect = false;
                        if (lines[selStartLineIdx].StartsWith("\t"))
                        {
                            lines[selStartLineIdx] = lines[selStartLineIdx].Substring(1);
                        }
                    }
                    else
                    {
                        for (int i = selStartLineIdx; i <= selEndLineIdx; i++)
                        {
                            if (lines[i].StartsWith("\t"))
                            {
                                lines[i] = lines[i].Substring(1);
                            }
                        } 
                    }
                }
                else
                {
                    for (int i = selStartLineIdx; i <= selEndLineIdx; i++)
                    {
                        lines[i] = '\t' + lines[i];
                    }
                }
                rtb.Lines = lines;
                

                var wordWrapFix = false;
                if (this.rtb.WordWrap)
                {
                    wordWrapFix = true;
                    this.rtb.WordWrap = false;
                }

                //Reselect Lines
                this.rtb.SelectionStart = this.rtb.GetFirstCharIndexFromLine(selStartLineIdx);

                if (multiLineSelect)
                {
                    var selectionEnd = (this.rtb.GetFirstCharIndexFromLine(selEndLineIdx) +
                                        this.rtb.Lines[selEndLineIdx].Length + (xtraEndLineWithNoCharSel ? 1 : 0));
                    this.rtb.SelectionLength = selectionEnd - this.rtb.SelectionStart;
                }

                if (wordWrapFix)
                {
                    this.rtb.WordWrap = true;
                }


                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }
    }

    #region EventArgs extension and event delegates

    /// <summary>
    /// Class to extend EventArgs with a cancel parameter.
    /// </summary>
    public class RTFEditorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RTFEditorEventArgs"/> is cancel.
        /// </summary>
        /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
        private Boolean cancel;
        public Boolean Cancel
        {
            get { return this.cancel; }
            set { this.cancel = value; }
        }
    }

    public delegate void RTFEditorOnRefresh(object sender, RTFEditorEventArgs e);
    public delegate void RTFEditorOnSave(object sender, RTFEditorEventArgs e);

    #endregion EventArgs extension and event delegates
}
