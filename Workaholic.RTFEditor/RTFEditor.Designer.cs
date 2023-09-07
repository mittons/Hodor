namespace Workaholic.RTFEditor
{
    partial class RTFEditor
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTFEditor));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.ms = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.italicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.underlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.strikeoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.selectfontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ts = new System.Windows.Forms.ToolStrip();
            this.tsNew = new System.Windows.Forms.ToolStripButton();
            this.tsOpen = new System.Windows.Forms.ToolStripButton();
            this.tsSave = new System.Windows.Forms.ToolStripButton();
            this.tsFileSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsCut = new System.Windows.Forms.ToolStripButton();
            this.tsCopy = new System.Windows.Forms.ToolStripButton();
            this.tsPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsUndo = new System.Windows.Forms.ToolStripButton();
            this.tsRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBold = new System.Windows.Forms.ToolStripButton();
            this.tsItalic = new System.Windows.Forms.ToolStripButton();
            this.tsUnderline = new System.Windows.Forms.ToolStripButton();
            this.tsStrikeout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsLeft = new System.Windows.Forms.ToolStripButton();
            this.tsCenter = new System.Windows.Forms.ToolStripButton();
            this.tsRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFontColor = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsSelectFontColor = new System.Windows.Forms.ToolStripMenuItem();
            this.tsBackgroundColor = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsSelectBackgroundColor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBullet = new System.Windows.Forms.ToolStripButton();
            this.tsIdentPlus = new System.Windows.Forms.ToolStripButton();
            this.tsIdentMinus = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.tsFontType = new System.Windows.Forms.ToolStripButton();
            this.tsZoom = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.ms.SuspendLayout();
            this.ts.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.rtb);
            resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.ms);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.ts);
            // 
            // rtb
            // 
            resources.ApplyResources(this.rtb, "rtb");
            this.rtb.HideSelection = false;
            this.rtb.Name = "rtb";
            this.rtb.TabStop = false;
            this.rtb.SelectionChanged += new System.EventHandler(this.rtb_SelectionChanged);
            this.rtb.TextChanged += new System.EventHandler(this.rtb_TextChanged);
            this.rtb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtb_KeyDown);
            // 
            // ms
            // 
            resources.ApplyResources(this.ms, "ms");
            this.ms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.formatToolStripMenuItem});
            this.ms.Name = "ms";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::Workaholic.RTFEditor.Properties.Resources.newdocument;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::Workaholic.RTFEditor.Properties.Resources.open;
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::Workaholic.RTFEditor.Properties.Resources.save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator7,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Image = global::Workaholic.RTFEditor.Properties.Resources.undo;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            resources.ApplyResources(this.undoToolStripMenuItem, "undoToolStripMenuItem");
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Image = global::Workaholic.RTFEditor.Properties.Resources.redo;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            resources.ApplyResources(this.redoToolStripMenuItem, "redoToolStripMenuItem");
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = global::Workaholic.RTFEditor.Properties.Resources.cut;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::Workaholic.RTFEditor.Properties.Resources.copy;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::Workaholic.RTFEditor.Properties.Resources.paste;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boldToolStripMenuItem,
            this.italicToolStripMenuItem,
            this.underlineToolStripMenuItem,
            this.strikeoutToolStripMenuItem,
            this.toolStripSeparator8,
            this.leftToolStripMenuItem,
            this.centerToolStripMenuItem,
            this.rightToolStripMenuItem,
            this.toolStripSeparator9,
            this.selectfontToolStripMenuItem});
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            resources.ApplyResources(this.formatToolStripMenuItem, "formatToolStripMenuItem");
            // 
            // boldToolStripMenuItem
            // 
            this.boldToolStripMenuItem.Name = "boldToolStripMenuItem";
            resources.ApplyResources(this.boldToolStripMenuItem, "boldToolStripMenuItem");
            this.boldToolStripMenuItem.Click += new System.EventHandler(this.boldToolStripMenuItem_Click);
            // 
            // italicToolStripMenuItem
            // 
            this.italicToolStripMenuItem.Name = "italicToolStripMenuItem";
            resources.ApplyResources(this.italicToolStripMenuItem, "italicToolStripMenuItem");
            this.italicToolStripMenuItem.Click += new System.EventHandler(this.italicToolStripMenuItem_Click);
            // 
            // underlineToolStripMenuItem
            // 
            this.underlineToolStripMenuItem.Name = "underlineToolStripMenuItem";
            resources.ApplyResources(this.underlineToolStripMenuItem, "underlineToolStripMenuItem");
            this.underlineToolStripMenuItem.Click += new System.EventHandler(this.underlineToolStripMenuItem_Click);
            // 
            // strikeoutToolStripMenuItem
            // 
            this.strikeoutToolStripMenuItem.Name = "strikeoutToolStripMenuItem";
            resources.ApplyResources(this.strikeoutToolStripMenuItem, "strikeoutToolStripMenuItem");
            this.strikeoutToolStripMenuItem.Click += new System.EventHandler(this.strikeoutToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // leftToolStripMenuItem
            // 
            this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
            resources.ApplyResources(this.leftToolStripMenuItem, "leftToolStripMenuItem");
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            // 
            // centerToolStripMenuItem
            // 
            this.centerToolStripMenuItem.Name = "centerToolStripMenuItem";
            resources.ApplyResources(this.centerToolStripMenuItem, "centerToolStripMenuItem");
            this.centerToolStripMenuItem.Click += new System.EventHandler(this.centerToolStripMenuItem_Click);
            // 
            // rightToolStripMenuItem
            // 
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            resources.ApplyResources(this.rightToolStripMenuItem, "rightToolStripMenuItem");
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // selectfontToolStripMenuItem
            // 
            this.selectfontToolStripMenuItem.Name = "selectfontToolStripMenuItem";
            resources.ApplyResources(this.selectfontToolStripMenuItem, "selectfontToolStripMenuItem");
            this.selectfontToolStripMenuItem.Click += new System.EventHandler(this.selectfontToolStripMenuItem_Click);
            // 
            // ts
            // 
            this.ts.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.ts, "ts");
            this.ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNew,
            this.tsOpen,
            this.tsSave,
            this.tsFileSeparator,
            this.tsCut,
            this.tsCopy,
            this.tsPaste,
            this.toolStripSeparator4,
            this.tsUndo,
            this.tsRedo,
            this.toolStripSeparator1,
            this.tsBold,
            this.tsItalic,
            this.tsUnderline,
            this.tsStrikeout,
            this.toolStripSeparator2,
            this.tsLeft,
            this.tsCenter,
            this.tsRight,
            this.toolStripSeparator3,
            this.tsFontColor,
            this.tsBackgroundColor,
            this.toolStripSeparator5,
            this.tsBullet,
            this.tsIdentPlus,
            this.tsIdentMinus,
            this.toolStripSeparator6,
            this.tsFontSize,
            this.tsFontType,
            this.tsZoom});
            this.ts.Name = "ts";
            // 
            // tsNew
            // 
            this.tsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsNew.Image = global::Workaholic.RTFEditor.Properties.Resources.newdocument;
            resources.ApplyResources(this.tsNew, "tsNew");
            this.tsNew.Name = "tsNew";
            this.tsNew.Click += new System.EventHandler(this.tsNew_Click);
            // 
            // tsOpen
            // 
            this.tsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsOpen.Image = global::Workaholic.RTFEditor.Properties.Resources.open;
            resources.ApplyResources(this.tsOpen, "tsOpen");
            this.tsOpen.Name = "tsOpen";
            this.tsOpen.Click += new System.EventHandler(this.tsRefresh_Click);
            // 
            // tsSave
            // 
            this.tsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsSave, "tsSave");
            this.tsSave.Name = "tsSave";
            this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // tsFileSeparator
            // 
            this.tsFileSeparator.Name = "tsFileSeparator";
            resources.ApplyResources(this.tsFileSeparator, "tsFileSeparator");
            // 
            // tsCut
            // 
            this.tsCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsCut, "tsCut");
            this.tsCut.Name = "tsCut";
            this.tsCut.Click += new System.EventHandler(this.tsCut_Click);
            // 
            // tsCopy
            // 
            this.tsCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsCopy, "tsCopy");
            this.tsCopy.Name = "tsCopy";
            this.tsCopy.Click += new System.EventHandler(this.tsCopy_Click);
            // 
            // tsPaste
            // 
            this.tsPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsPaste, "tsPaste");
            this.tsPaste.Name = "tsPaste";
            this.tsPaste.Click += new System.EventHandler(this.tsPaste_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // tsUndo
            // 
            this.tsUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsUndo.Image = global::Workaholic.RTFEditor.Properties.Resources.undo;
            resources.ApplyResources(this.tsUndo, "tsUndo");
            this.tsUndo.Name = "tsUndo";
            this.tsUndo.Click += new System.EventHandler(this.tsUndo_Click);
            // 
            // tsRedo
            // 
            this.tsRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRedo.Image = global::Workaholic.RTFEditor.Properties.Resources.redo;
            resources.ApplyResources(this.tsRedo, "tsRedo");
            this.tsRedo.Name = "tsRedo";
            this.tsRedo.Click += new System.EventHandler(this.tsRedo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsBold
            // 
            this.tsBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBold.Image = global::Workaholic.RTFEditor.Properties.Resources.bold;
            resources.ApplyResources(this.tsBold, "tsBold");
            this.tsBold.Name = "tsBold";
            this.tsBold.Click += new System.EventHandler(this.tsBold_Click);
            // 
            // tsItalic
            // 
            this.tsItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsItalic.Image = global::Workaholic.RTFEditor.Properties.Resources.italic;
            resources.ApplyResources(this.tsItalic, "tsItalic");
            this.tsItalic.Name = "tsItalic";
            this.tsItalic.Click += new System.EventHandler(this.tsItalic_Click);
            // 
            // tsUnderline
            // 
            this.tsUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsUnderline.Image = global::Workaholic.RTFEditor.Properties.Resources.underline;
            resources.ApplyResources(this.tsUnderline, "tsUnderline");
            this.tsUnderline.Name = "tsUnderline";
            this.tsUnderline.Click += new System.EventHandler(this.tsUnderline_Click);
            // 
            // tsStrikeout
            // 
            this.tsStrikeout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsStrikeout.Image = global::Workaholic.RTFEditor.Properties.Resources.strikethrough;
            resources.ApplyResources(this.tsStrikeout, "tsStrikeout");
            this.tsStrikeout.Name = "tsStrikeout";
            this.tsStrikeout.Click += new System.EventHandler(this.tsStrikeout_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tsLeft
            // 
            this.tsLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsLeft.Image = global::Workaholic.RTFEditor.Properties.Resources.justifyleft;
            resources.ApplyResources(this.tsLeft, "tsLeft");
            this.tsLeft.Name = "tsLeft";
            this.tsLeft.Click += new System.EventHandler(this.tsLeft_Click);
            // 
            // tsCenter
            // 
            this.tsCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsCenter.Image = global::Workaholic.RTFEditor.Properties.Resources.justifycenter;
            resources.ApplyResources(this.tsCenter, "tsCenter");
            this.tsCenter.Name = "tsCenter";
            this.tsCenter.Click += new System.EventHandler(this.tsCenter_Click);
            // 
            // tsRight
            // 
            this.tsRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRight.Image = global::Workaholic.RTFEditor.Properties.Resources.justifyright;
            resources.ApplyResources(this.tsRight, "tsRight");
            this.tsRight.Name = "tsRight";
            this.tsRight.Click += new System.EventHandler(this.tsRight_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tsFontColor
            // 
            this.tsFontColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFontColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSelectFontColor});
            this.tsFontColor.Image = global::Workaholic.RTFEditor.Properties.Resources.forecolor;
            resources.ApplyResources(this.tsFontColor, "tsFontColor");
            this.tsFontColor.Name = "tsFontColor";
            this.tsFontColor.Click += new System.EventHandler(this.tsFontColor_Click);
            // 
            // tsSelectFontColor
            // 
            this.tsSelectFontColor.Name = "tsSelectFontColor";
            resources.ApplyResources(this.tsSelectFontColor, "tsSelectFontColor");
            this.tsSelectFontColor.Click += new System.EventHandler(this.tsSelectFontColor_Click);
            // 
            // tsBackgroundColor
            // 
            this.tsBackgroundColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBackgroundColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSelectBackgroundColor});
            this.tsBackgroundColor.Image = global::Workaholic.RTFEditor.Properties.Resources.backcolor;
            resources.ApplyResources(this.tsBackgroundColor, "tsBackgroundColor");
            this.tsBackgroundColor.Name = "tsBackgroundColor";
            this.tsBackgroundColor.Click += new System.EventHandler(this.tsBackgroundColor_Click);
            // 
            // tsSelectBackgroundColor
            // 
            this.tsSelectBackgroundColor.Name = "tsSelectBackgroundColor";
            resources.ApplyResources(this.tsSelectBackgroundColor, "tsSelectBackgroundColor");
            this.tsSelectBackgroundColor.Click += new System.EventHandler(this.tsSelectBackgroundColor_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // tsBullet
            // 
            this.tsBullet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBullet.Image = global::Workaholic.RTFEditor.Properties.Resources.bullist;
            resources.ApplyResources(this.tsBullet, "tsBullet");
            this.tsBullet.Name = "tsBullet";
            this.tsBullet.Click += new System.EventHandler(this.tsBullet_Click);
            // 
            // tsIdentPlus
            // 
            this.tsIdentPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsIdentPlus.Image = global::Workaholic.RTFEditor.Properties.Resources.indent;
            resources.ApplyResources(this.tsIdentPlus, "tsIdentPlus");
            this.tsIdentPlus.Name = "tsIdentPlus";
            this.tsIdentPlus.Click += new System.EventHandler(this.tsIdentPlus_Click);
            // 
            // tsIdentMinus
            // 
            this.tsIdentMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsIdentMinus.Image = global::Workaholic.RTFEditor.Properties.Resources.outdent;
            resources.ApplyResources(this.tsIdentMinus, "tsIdentMinus");
            this.tsIdentMinus.Name = "tsIdentMinus";
            this.tsIdentMinus.Click += new System.EventHandler(this.tsIdentMinus_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // tsFontSize
            // 
            this.tsFontSize.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            resources.ApplyResources(this.tsFontSize, "tsFontSize");
            this.tsFontSize.Items.AddRange(new object[] {
            resources.GetString("tsFontSize.Items"),
            resources.GetString("tsFontSize.Items1"),
            resources.GetString("tsFontSize.Items2"),
            resources.GetString("tsFontSize.Items3"),
            resources.GetString("tsFontSize.Items4"),
            resources.GetString("tsFontSize.Items5"),
            resources.GetString("tsFontSize.Items6"),
            resources.GetString("tsFontSize.Items7"),
            resources.GetString("tsFontSize.Items8"),
            resources.GetString("tsFontSize.Items9"),
            resources.GetString("tsFontSize.Items10"),
            resources.GetString("tsFontSize.Items11"),
            resources.GetString("tsFontSize.Items12"),
            resources.GetString("tsFontSize.Items13"),
            resources.GetString("tsFontSize.Items14"),
            resources.GetString("tsFontSize.Items15")});
            this.tsFontSize.Name = "tsFontSize";
            this.tsFontSize.SelectedIndexChanged += new System.EventHandler(this.tsFontSize_SelectedIndexChanged);
            this.tsFontSize.Leave += new System.EventHandler(this.tsFontSize_Leave);
            this.tsFontSize.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tsFontSize_KeyUp);
            // 
            // tsFontType
            // 
            this.tsFontType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsFontType.Name = "tsFontType";
            resources.ApplyResources(this.tsFontType, "tsFontType");
            this.tsFontType.Click += new System.EventHandler(this.tsFontType_Click);
            // 
            // tsZoom
            // 
            resources.ApplyResources(this.tsZoom, "tsZoom");
            this.tsZoom.Items.AddRange(new object[] {
            resources.GetString("tsZoom.Items"),
            resources.GetString("tsZoom.Items1"),
            resources.GetString("tsZoom.Items2"),
            resources.GetString("tsZoom.Items3"),
            resources.GetString("tsZoom.Items4"),
            resources.GetString("tsZoom.Items5"),
            resources.GetString("tsZoom.Items6"),
            resources.GetString("tsZoom.Items7")});
            this.tsZoom.Name = "tsZoom";
            this.tsZoom.SelectedIndexChanged += new System.EventHandler(this.tsZoom_SelectedIndexChanged);
            this.tsZoom.Leave += new System.EventHandler(this.tsZoom_Leave);
            this.tsZoom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tsZoom_KeyUp);
            // 
            // RTFEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.DoubleBuffered = true;
            this.Name = "RTFEditor";
            this.Load += new System.EventHandler(this.RTFEditor_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ms.ResumeLayout(false);
            this.ms.PerformLayout();
            this.ts.ResumeLayout(false);
            this.ts.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.ToolStrip ts;
        private System.Windows.Forms.ToolStripButton tsNew;
        private System.Windows.Forms.ToolStripButton tsOpen;
        private System.Windows.Forms.ToolStripButton tsSave;
        private System.Windows.Forms.ToolStripSeparator tsFileSeparator;
        private System.Windows.Forms.ToolStripButton tsBold;
        private System.Windows.Forms.ToolStripButton tsItalic;
        private System.Windows.Forms.ToolStripButton tsUnderline;
        private System.Windows.Forms.ToolStripButton tsStrikeout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsLeft;
        private System.Windows.Forms.ToolStripButton tsCenter;
        private System.Windows.Forms.ToolStripButton tsRight;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsCut;
        private System.Windows.Forms.ToolStripButton tsCopy;
        private System.Windows.Forms.ToolStripButton tsPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox tsFontSize;
        private System.Windows.Forms.ToolStripButton tsFontType;
        private System.Windows.Forms.ToolStripDropDownButton tsFontColor;
        private System.Windows.Forms.ToolStripMenuItem tsSelectFontColor;
        private System.Windows.Forms.ToolStripDropDownButton tsBackgroundColor;
        private System.Windows.Forms.ToolStripMenuItem tsSelectBackgroundColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsBullet;
        private System.Windows.Forms.ToolStripButton tsIdentPlus;
        private System.Windows.Forms.ToolStripButton tsIdentMinus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsUndo;
        private System.Windows.Forms.ToolStripButton tsRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox tsZoom;
        private System.Windows.Forms.MenuStrip ms;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem italicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem underlineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem strikeoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem selectfontToolStripMenuItem;

    }
}
