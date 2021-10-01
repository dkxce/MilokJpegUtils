namespace CryptoJPEG
{
    partial class MainJPEGForm
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "DEFFILTER",
            "Default File Filter",
            "*.* or Regex `(.jpg$)|(.jpeg$)`",
            "*.*"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "DEFOP",
            "Default File Operation",
            "Select From List Of Operations",
            "Encrypt to ENX"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "JPEGFILE",
            "JPEG File Name",
            "Image to Small Crypt",
            "IMAGE.JPGX"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "KEYWORD",
            "Keyword (Password)",
            "Encrypt and Decrypt Keyword",
            "0n14NaNzGtJtr07yb14dkxce8"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "SUBFOLDERS",
            "Scan Subfolders",
            "Yes or No",
            "Yes"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x010F",
            "Make",
            "{Make}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x0110",
            "Model",
            "{Model}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x010E",
            "ImageDescription",
            "{ImageDescription}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x0131",
            "Software",
            "{Software}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x013B",
            "Artist",
            "{Artist}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x8298",
            "Copyright",
            "{Copyright}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x9286",
            "UserComment",
            "{UserComment}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x927C",
            "MakerNote",
            "{MakerNote}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] {
            "0xA420",
            "ImageUniqueID",
            "{ImageUniqueID}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x0132",
            "DateTime",
            "{DateTime}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x9003",
            "DateTimeOriginal",
            "{DateTimeOriginal}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x9004",
            "DateTimeDigitized",
            "{DateTimeDigitized}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] {
            "0xA430",
            "OwnerName",
            "{OwnerName}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x010D",
            "DocumentName",
            "{DocumentName}",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "START", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)))),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Start FileIndex"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "File Numeration"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "1", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))))}, -1);
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem(new string[] {
            "SKVOZ",
            "Unical ID per Dir",
            "Yes or No (True or False)",
            "Yes"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "RENAME", System.Drawing.SystemColors.WindowText, System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128))))), new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)))),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Rename Files"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Yes or No (True or False)"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "No", System.Drawing.SystemColors.WindowText, System.Drawing.Color.White, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))))}, -1);
            System.Windows.Forms.ListViewItem listViewItem23 = new System.Windows.Forms.ListViewItem(new string[] {
            "RNPATTERN",
            " .. File Name Pattern",
            "{ExifTag} or {ExifTag:Format}",
            "{FileIndex:0000} {Make} {FileModifiedDateTime:yyMMdd}"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem24 = new System.Windows.Forms.ListViewItem(new string[] {
            "RWEXIF",
            "Rewrite Exif",
            "Yes or No (True or False)",
            "No"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192))))), null);
            System.Windows.Forms.ListViewItem listViewItem25 = new System.Windows.Forms.ListViewItem(new string[] {
            "RESIZE",
            "Resize Images",
            "No/Resize/Reduce/Enlarge",
            "No"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))), null);
            System.Windows.Forms.ListViewItem listViewItem26 = new System.Windows.Forms.ListViewItem(new string[] {
            "WIDTH",
            " .. Max Image Width",
            "New Width in Pixels",
            "2000"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem27 = new System.Windows.Forms.ListViewItem(new string[] {
            "HEIGHT",
            " .. Max Image Height",
            "New Height in Pixels",
            "2000"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem28 = new System.Windows.Forms.ListViewItem(new string[] {
            "QUALITY",
            "Max Image Quality",
            "1..100 in %",
            "75"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem29 = new System.Windows.Forms.ListViewItem(new string[] {
            "SAVETOSUB",
            "Save To",
            "Source/Subfolder/Path",
            "Source"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192))))), null);
            System.Windows.Forms.ListViewItem listViewItem30 = new System.Windows.Forms.ListViewItem(new string[] {
            "SAVEPATH",
            "Save Path",
            "Path or Subfolder",
            "Processed"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192))))), null);
            System.Windows.Forms.ListViewItem listViewItem31 = new System.Windows.Forms.ListViewItem(new string[] {
            "CASE",
            "Change Case",
            "No...",
            "Lower Extention"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem32 = new System.Windows.Forms.ListViewItem(new string[] {
            "TXTWMADD",
            "Add Text Watermark",
            "Yes or No (True or False)",
            "No"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255))))), null);
            System.Windows.Forms.ListViewItem listViewItem33 = new System.Windows.Forms.ListViewItem(new string[] {
            "TXTWMTEXT",
            " .. Text",
            "Text",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem34 = new System.Windows.Forms.ListViewItem(new string[] {
            "TXTWMOP",
            " .. Opacity",
            "1..100",
            "75"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem35 = new System.Windows.Forms.ListViewItem(new string[] {
            "TXTWMPOS",
            " .. Position",
            "Range",
            "4 - Bottom Right"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem36 = new System.Windows.Forms.ListViewItem(new string[] {
            "TXTWMFONT",
            " .. Font",
            "Font Name",
            "Arial"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem37 = new System.Windows.Forms.ListViewItem(new string[] {
            "TXTWMSIZE",
            " .. Font Size",
            "in Pixels or % ",
            "10%"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem38 = new System.Windows.Forms.ListViewItem(new string[] {
            "TXTWMCOLOR",
            " .. Color",
            "Color",
            "#FF0000"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem39 = new System.Windows.Forms.ListViewItem(new string[] {
            "IMGWMADD",
            "Add Image Watermark",
            "Yes or No (True or False)",
            "No"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255))))), null);
            System.Windows.Forms.ListViewItem listViewItem40 = new System.Windows.Forms.ListViewItem(new string[] {
            "IMGWMFILE",
            " .. File",
            "Full Path to File",
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem41 = new System.Windows.Forms.ListViewItem(new string[] {
            "IMGWMOP",
            " .. Opacity",
            "1..100%",
            "75"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem42 = new System.Windows.Forms.ListViewItem(new string[] {
            "IMGWMPOS",
            " .. Position",
            "Range",
            "0 - Center"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainJPEGForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.listFile = new System.Windows.Forms.ListView();
            this.columnHeader24 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader27 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader28 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader37 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader34 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader38 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader39 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.applyWorkToAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearFileListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeNotExistingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.removeFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeNoOperationFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeCompletedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.processFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ST_TTF = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ST_PROCESS = new System.Windows.Forms.ToolStripStatusLabel();
            this.listPARAMS = new System.Windows.Forms.ListView();
            this.columnHeader30 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader31 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader32 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader35 = new System.Windows.Forms.ColumnHeader();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.exRW = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadExifDataFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveExifDataToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.todobatch = new System.Windows.Forms.ListView();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.ctct = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ctnt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.exif = new System.Windows.Forms.ListView();
            this.ID = new System.Windows.Forms.ColumnHeader();
            this.Parameter = new System.Windows.Forms.ColumnHeader();
            this.Value = new System.Windows.Forms.ColumnHeader();
            this.tabControl1.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Controls.Add(this.tabPage11);
            this.tabControl1.Controls.Add(this.tabPage10);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(936, 556);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.listFile);
            this.tabPage9.Controls.Add(this.statusStrip1);
            this.tabPage9.Controls.Add(this.listPARAMS);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(928, 530);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Text = "File List";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // listFile
            // 
            this.listFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader24,
            this.columnHeader27,
            this.columnHeader28,
            this.columnHeader37,
            this.columnHeader34,
            this.columnHeader38,
            this.columnHeader39});
            this.listFile.ContextMenuStrip = this.contextMenuStrip1;
            this.listFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listFile.FullRowSelect = true;
            this.listFile.GridLines = true;
            this.listFile.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listFile.Location = new System.Drawing.Point(3, 98);
            this.listFile.Name = "listFile";
            this.listFile.OwnerDraw = true;
            this.listFile.Size = new System.Drawing.Size(922, 407);
            this.listFile.TabIndex = 11;
            this.listFile.UseCompatibleStateImageBehavior = false;
            this.listFile.View = System.Windows.Forms.View.Details;
            this.listFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listFile_MouseDoubleClick);
            this.listFile.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listFile_DrawColumnHeader);
            this.listFile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listFile_KeyPress);
            this.listFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listFile_KeyDown);
            this.listFile.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listFile_DrawSubItem);
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "Number";
            this.columnHeader24.Width = 53;
            // 
            // columnHeader27
            // 
            this.columnHeader27.Text = "Directory";
            this.columnHeader27.Width = 88;
            // 
            // columnHeader28
            // 
            this.columnHeader28.Text = "File Name";
            this.columnHeader28.Width = 117;
            // 
            // columnHeader37
            // 
            this.columnHeader37.Text = "File Size";
            this.columnHeader37.Width = 57;
            // 
            // columnHeader34
            // 
            this.columnHeader34.Text = "Operation";
            this.columnHeader34.Width = 99;
            // 
            // columnHeader38
            // 
            this.columnHeader38.Text = "New File Name";
            this.columnHeader38.Width = 103;
            // 
            // columnHeader39
            // 
            this.columnHeader39.Text = "Status";
            this.columnHeader39.Width = 385;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDirectoryToolStripMenuItem,
            this.addFilesToolStripMenuItem,
            this.toolStripMenuItem5,
            this.applyWorkToAllToolStripMenuItem,
            this.toolStripMenuItem1,
            this.clearStatusToolStripMenuItem,
            this.toolStripMenuItem3,
            this.clearFileListToolStripMenuItem,
            this.removeNotExistingToolStripMenuItem,
            this.toolStripMenuItem4,
            this.removeFilesToolStripMenuItem,
            this.removeNoOperationFilesToolStripMenuItem,
            this.removeCompletedToolStripMenuItem,
            this.toolStripMenuItem2,
            this.processFilesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(265, 254);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // addDirectoryToolStripMenuItem
            // 
            this.addDirectoryToolStripMenuItem.Name = "addDirectoryToolStripMenuItem";
            this.addDirectoryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.addDirectoryToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.addDirectoryToolStripMenuItem.Text = "Add Directory...";
            this.addDirectoryToolStripMenuItem.Click += new System.EventHandler(this.addDirectoryToolStripMenuItem_Click);
            // 
            // addFilesToolStripMenuItem
            // 
            this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            this.addFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.addFilesToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.addFilesToolStripMenuItem.Text = "Add Files...";
            this.addFilesToolStripMenuItem.Click += new System.EventHandler(this.addFilesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(261, 6);
            // 
            // applyWorkToAllToolStripMenuItem
            // 
            this.applyWorkToAllToolStripMenuItem.Name = "applyWorkToAllToolStripMenuItem";
            this.applyWorkToAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.A)));
            this.applyWorkToAllToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.applyWorkToAllToolStripMenuItem.Text = "Apply Operation to All...";
            this.applyWorkToAllToolStripMenuItem.Click += new System.EventHandler(this.applyWorkToAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(261, 6);
            // 
            // clearStatusToolStripMenuItem
            // 
            this.clearStatusToolStripMenuItem.Enabled = false;
            this.clearStatusToolStripMenuItem.Name = "clearStatusToolStripMenuItem";
            this.clearStatusToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.clearStatusToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.clearStatusToolStripMenuItem.Text = "Clear Status";
            this.clearStatusToolStripMenuItem.Click += new System.EventHandler(this.clearStatusToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(261, 6);
            // 
            // clearFileListToolStripMenuItem
            // 
            this.clearFileListToolStripMenuItem.Enabled = false;
            this.clearFileListToolStripMenuItem.Name = "clearFileListToolStripMenuItem";
            this.clearFileListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.clearFileListToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.clearFileListToolStripMenuItem.Text = "Clear File List";
            this.clearFileListToolStripMenuItem.Click += new System.EventHandler(this.clearFileListToolStripMenuItem_Click);
            // 
            // removeNotExistingToolStripMenuItem
            // 
            this.removeNotExistingToolStripMenuItem.Enabled = false;
            this.removeNotExistingToolStripMenuItem.Name = "removeNotExistingToolStripMenuItem";
            this.removeNotExistingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.removeNotExistingToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.removeNotExistingToolStripMenuItem.Text = "Remove Not Existing";
            this.removeNotExistingToolStripMenuItem.Click += new System.EventHandler(this.removeNotExistingToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(261, 6);
            // 
            // removeFilesToolStripMenuItem
            // 
            this.removeFilesToolStripMenuItem.Enabled = false;
            this.removeFilesToolStripMenuItem.Name = "removeFilesToolStripMenuItem";
            this.removeFilesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.removeFilesToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.removeFilesToolStripMenuItem.Text = "Remove Files";
            this.removeFilesToolStripMenuItem.Click += new System.EventHandler(this.removeFilesToolStripMenuItem_Click);
            // 
            // removeNoOperationFilesToolStripMenuItem
            // 
            this.removeNoOperationFilesToolStripMenuItem.Enabled = false;
            this.removeNoOperationFilesToolStripMenuItem.Name = "removeNoOperationFilesToolStripMenuItem";
            this.removeNoOperationFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.removeNoOperationFilesToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.removeNoOperationFilesToolStripMenuItem.Text = "Remove NO Operation Files";
            this.removeNoOperationFilesToolStripMenuItem.Click += new System.EventHandler(this.removeNoOperationFilesToolStripMenuItem_Click);
            // 
            // removeCompletedToolStripMenuItem
            // 
            this.removeCompletedToolStripMenuItem.Enabled = false;
            this.removeCompletedToolStripMenuItem.Name = "removeCompletedToolStripMenuItem";
            this.removeCompletedToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Delete)));
            this.removeCompletedToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.removeCompletedToolStripMenuItem.Text = "Remove Completed";
            this.removeCompletedToolStripMenuItem.Click += new System.EventHandler(this.removeCompletedToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(261, 6);
            // 
            // processFilesToolStripMenuItem
            // 
            this.processFilesToolStripMenuItem.Enabled = false;
            this.processFilesToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.processFilesToolStripMenuItem.Name = "processFilesToolStripMenuItem";
            this.processFilesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.processFilesToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.processFilesToolStripMenuItem.Text = "Process Files";
            this.processFilesToolStripMenuItem.Click += new System.EventHandler(this.processFilesToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.ST_TTF,
            this.toolStripStatusLabel2,
            this.ST_PROCESS});
            this.statusStrip1.Location = new System.Drawing.Point(3, 505);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(922, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel1.Text = "Total Files:";
            // 
            // ST_TTF
            // 
            this.ST_TTF.Name = "ST_TTF";
            this.ST_TTF.Size = new System.Drawing.Size(13, 17);
            this.ST_TTF.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(48, 17);
            this.toolStripStatusLabel2.Text = "Process:";
            // 
            // ST_PROCESS
            // 
            this.ST_PROCESS.Name = "ST_PROCESS";
            this.ST_PROCESS.Size = new System.Drawing.Size(35, 17);
            this.ST_PROCESS.Text = "NONE";
            // 
            // listPARAMS
            // 
            this.listPARAMS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listPARAMS.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader30,
            this.columnHeader31,
            this.columnHeader32,
            this.columnHeader35});
            this.listPARAMS.Dock = System.Windows.Forms.DockStyle.Top;
            this.listPARAMS.FullRowSelect = true;
            this.listPARAMS.GridLines = true;
            this.listPARAMS.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listPARAMS.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.listPARAMS.Location = new System.Drawing.Point(3, 3);
            this.listPARAMS.MultiSelect = false;
            this.listPARAMS.Name = "listPARAMS";
            this.listPARAMS.OwnerDraw = true;
            this.listPARAMS.Scrollable = false;
            this.listPARAMS.Size = new System.Drawing.Size(922, 95);
            this.listPARAMS.TabIndex = 12;
            this.listPARAMS.UseCompatibleStateImageBehavior = false;
            this.listPARAMS.View = System.Windows.Forms.View.Details;
            this.listPARAMS.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listPARAMS_MouseDoubleClick);
            this.listPARAMS.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listPARAMS_DrawColumnHeader);
            this.listPARAMS.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listPARAMS_DrawItem);
            this.listPARAMS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listPARAMS_KeyPress);
            this.listPARAMS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listPARAMS_KeyDown);
            this.listPARAMS.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listPARAMS_DrawSubItem);
            // 
            // columnHeader30
            // 
            this.columnHeader30.Text = "ID";
            this.columnHeader30.Width = 0;
            // 
            // columnHeader31
            // 
            this.columnHeader31.Text = "Parameter";
            this.columnHeader31.Width = 141;
            // 
            // columnHeader32
            // 
            this.columnHeader32.Text = "Note";
            this.columnHeader32.Width = 174;
            // 
            // columnHeader35
            // 
            this.columnHeader35.Text = "Value";
            this.columnHeader35.Width = 607;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.exRW);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(928, 530);
            this.tabPage11.TabIndex = 2;
            this.tabPage11.Text = "Exif Rewrite";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // exRW
            // 
            this.exRW.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.exRW.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.exRW.ContextMenuStrip = this.contextMenuStrip2;
            this.exRW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exRW.FullRowSelect = true;
            this.exRW.GridLines = true;
            this.exRW.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.exRW.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18,
            listViewItem19});
            this.exRW.Location = new System.Drawing.Point(3, 3);
            this.exRW.MultiSelect = false;
            this.exRW.Name = "exRW";
            this.exRW.Scrollable = false;
            this.exRW.Size = new System.Drawing.Size(922, 524);
            this.exRW.TabIndex = 8;
            this.exRW.UseCompatibleStateImageBehavior = false;
            this.exRW.View = System.Windows.Forms.View.Details;
            this.exRW.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.exRW_MouseDoubleClick);
            this.exRW.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.exRW_KeyPress);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 45;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Parameter";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "From Exif or File";
            this.columnHeader3.Width = 112;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 668;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadExifDataFromFileToolStripMenuItem,
            this.saveExifDataToFileToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(212, 48);
            // 
            // loadExifDataFromFileToolStripMenuItem
            // 
            this.loadExifDataFromFileToolStripMenuItem.Name = "loadExifDataFromFileToolStripMenuItem";
            this.loadExifDataFromFileToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.loadExifDataFromFileToolStripMenuItem.Text = "Load Exif Data from File...";
            this.loadExifDataFromFileToolStripMenuItem.Click += new System.EventHandler(this.loadExifDataFromFileToolStripMenuItem_Click);
            // 
            // saveExifDataToFileToolStripMenuItem
            // 
            this.saveExifDataToFileToolStripMenuItem.Name = "saveExifDataToFileToolStripMenuItem";
            this.saveExifDataToFileToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.saveExifDataToFileToolStripMenuItem.Text = "Save Exif Data to File...";
            this.saveExifDataToFileToolStripMenuItem.Click += new System.EventHandler(this.saveExifDataToFileToolStripMenuItem_Click);
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.todobatch);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(928, 530);
            this.tabPage10.TabIndex = 3;
            this.tabPage10.Text = "Batch Work Parameters";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // todobatch
            // 
            this.todobatch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.todobatch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12});
            this.todobatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.todobatch.FullRowSelect = true;
            this.todobatch.GridLines = true;
            this.todobatch.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listViewItem37.Tag = "";
            this.todobatch.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem20,
            listViewItem21,
            listViewItem22,
            listViewItem23,
            listViewItem24,
            listViewItem25,
            listViewItem26,
            listViewItem27,
            listViewItem28,
            listViewItem29,
            listViewItem30,
            listViewItem31,
            listViewItem32,
            listViewItem33,
            listViewItem34,
            listViewItem35,
            listViewItem36,
            listViewItem37,
            listViewItem38,
            listViewItem39,
            listViewItem40,
            listViewItem41,
            listViewItem42});
            this.todobatch.Location = new System.Drawing.Point(3, 3);
            this.todobatch.MultiSelect = false;
            this.todobatch.Name = "todobatch";
            this.todobatch.OwnerDraw = true;
            this.todobatch.Scrollable = false;
            this.todobatch.Size = new System.Drawing.Size(922, 524);
            this.todobatch.TabIndex = 9;
            this.todobatch.UseCompatibleStateImageBehavior = false;
            this.todobatch.View = System.Windows.Forms.View.Details;
            this.todobatch.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.todobatch_MouseDoubleClick);
            this.todobatch.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.todobatch_DrawColumnHeader);
            this.todobatch.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.todobatch_DrawItem);
            this.todobatch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.todobatch_KeyPress);
            this.todobatch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.todobatch_KeyDown);
            this.todobatch.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.todobatch_DrawSubItem);
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "ID";
            this.columnHeader9.Width = 0;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Parameter";
            this.columnHeader10.Width = 121;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Note";
            this.columnHeader11.Width = 161;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Value";
            this.columnHeader12.Width = 640;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.ctct);
            this.tabPage7.Controls.Add(this.label9);
            this.tabPage7.Controls.Add(this.ctnt);
            this.tabPage7.Controls.Add(this.label7);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(928, 530);
            this.tabPage7.TabIndex = 7;
            this.tabPage7.Text = "CryptText";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // ctct
            // 
            this.ctct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctct.Location = new System.Drawing.Point(3, 270);
            this.ctct.Multiline = true;
            this.ctct.Name = "ctct";
            this.ctct.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ctct.Size = new System.Drawing.Size(922, 257);
            this.ctct.TabIndex = 8;
            this.ctct.TextChanged += new System.EventHandler(this.ctct_TextChanged);
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.Location = new System.Drawing.Point(3, 255);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(922, 15);
            this.label9.TabIndex = 7;
            this.label9.Text = "Crypted Text:";
            // 
            // ctnt
            // 
            this.ctnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctnt.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctnt.Location = new System.Drawing.Point(3, 18);
            this.ctnt.Multiline = true;
            this.ctnt.Name = "ctnt";
            this.ctnt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ctnt.Size = new System.Drawing.Size(922, 237);
            this.ctnt.TabIndex = 0;
            this.ctnt.TextChanged += new System.EventHandler(this.ctnt_TextChanged);
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(922, 15);
            this.label7.TabIndex = 6;
            this.label7.Text = "Normal Text:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.exif);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(928, 530);
            this.tabPage3.TabIndex = 8;
            this.tabPage3.Text = "Read JPEG Exif Tags";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // exif
            // 
            this.exif.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.exif.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.Parameter,
            this.Value});
            this.exif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exif.FullRowSelect = true;
            this.exif.GridLines = true;
            this.exif.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.exif.Location = new System.Drawing.Point(3, 3);
            this.exif.MultiSelect = false;
            this.exif.Name = "exif";
            this.exif.Scrollable = false;
            this.exif.Size = new System.Drawing.Size(922, 524);
            this.exif.TabIndex = 6;
            this.exif.UseCompatibleStateImageBehavior = false;
            this.exif.View = System.Windows.Forms.View.Details;
            this.exif.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.exif_MouseDoubleClick);
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 65;
            // 
            // Parameter
            // 
            this.Parameter.Text = "Parameter";
            this.Parameter.Width = 174;
            // 
            // Value
            // 
            this.Value.Text = "Value";
            this.Value.Width = 683;
            // 
            // MainJPEGForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 556);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainJPEGForm";
            this.Text = "Milok JPEG Utils (CryptoJPEG, ExifRewrite, ImageReady, ENX)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabPage11.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.ListView listFile;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader27;
        private System.Windows.Forms.ColumnHeader columnHeader28;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.ListView listPARAMS;
        private System.Windows.Forms.ColumnHeader columnHeader30;
        private System.Windows.Forms.ColumnHeader columnHeader31;
        private System.Windows.Forms.ColumnHeader columnHeader32;
        private System.Windows.Forms.ColumnHeader columnHeader35;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearFileListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ColumnHeader columnHeader37;
        private System.Windows.Forms.ColumnHeader columnHeader34;
        private System.Windows.Forms.ColumnHeader columnHeader38;
        private System.Windows.Forms.ColumnHeader columnHeader39;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel ST_TTF;
        private System.Windows.Forms.ToolStripMenuItem addFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem processFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeNoOperationFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel ST_PROCESS;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.ToolStripMenuItem removeCompletedToolStripMenuItem;
        private System.Windows.Forms.ListView exRW;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TextBox ctct;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ctnt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView exif;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader Parameter;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.ListView todobatch;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ToolStripMenuItem clearStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem applyWorkToAllToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem saveExifDataToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadExifDataFromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeNotExistingToolStripMenuItem;
    }
}

