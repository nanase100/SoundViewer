namespace SEViewer
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.seFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.seGenre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.seCaption = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.listView1 = new System.Windows.Forms.ListView();
			this.seGenre2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.button1 = new System.Windows.Forms.Button();
			this.comboBox5 = new System.Windows.Forms.ComboBox();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.comboBox4 = new System.Windows.Forms.ComboBox();
			this.textCopyStr = new System.Windows.Forms.TextBox();
			this.copyStrSelect = new System.Windows.Forms.ComboBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.checkBox9 = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.tabCategorySE = new System.Windows.Forms.CheckBox();
			this.tabCategoryBGM = new System.Windows.Forms.CheckBox();
			this.tabCategoryBGV = new System.Windows.Forms.CheckBox();
			this.tabCategoryUSEFULL = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.tabCategoryFAV = new System.Windows.Forms.CheckBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.オプション設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemCheck1 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemCheck2 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemCheck3 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemCheck4 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemCombo1 = new System.Windows.Forms.ToolStripComboBox();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.groupBox5.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 50;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 5000;
			this.toolTip1.InitialDelay = 1000;
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ReshowDelay = 100;
			this.toolTip1.ToolTipTitle = "便利機能";
			this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
			// 
			// seFileName
			// 
			this.seFileName.Text = "音ファイル名";
			this.seFileName.Width = 100;
			// 
			// seGenre
			// 
			this.seGenre.Text = "ジャンル";
			this.seGenre.Width = 68;
			// 
			// seCaption
			// 
			this.seCaption.Tag = "";
			this.seCaption.Text = "音の説明文";
			this.seCaption.Width = 360;
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.seFileName,
            this.seGenre,
            this.seGenre2,
            this.seCaption});
			this.listView1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(3, 29);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(607, 603);
			this.listView1.TabIndex = 9;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
			this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
			this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
			// 
			// seGenre2
			// 
			this.seGenre2.Text = "ジャンル2";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(10, 18);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(84, 43);
			this.button1.TabIndex = 8;
			this.button1.Text = "再生▷";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// comboBox5
			// 
			this.comboBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox5.DropDownHeight = 300;
			this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox5.FormattingEnabled = true;
			this.comboBox5.IntegralHeight = false;
			this.comboBox5.Location = new System.Drawing.Point(285, 3);
			this.comboBox5.Name = "comboBox5";
			this.comboBox5.Size = new System.Drawing.Size(325, 20);
			this.comboBox5.TabIndex = 4;
			this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
			// 
			// comboBox3
			// 
			this.comboBox3.DropDownHeight = 300;
			this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.IntegralHeight = false;
			this.comboBox3.Location = new System.Drawing.Point(3, 3);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(276, 20);
			this.comboBox3.TabIndex = 0;
			this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
			// 
			// comboBox4
			// 
			this.comboBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox4.FormattingEnabled = true;
			this.comboBox4.Items.AddRange(new object[] {
            "説明文から",
            "SE名から"});
			this.comboBox4.Location = new System.Drawing.Point(650, 5);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new System.Drawing.Size(79, 20);
			this.comboBox4.TabIndex = 1;
			this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
			// 
			// textCopyStr
			// 
			this.textCopyStr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textCopyStr.Location = new System.Drawing.Point(6, 50);
			this.textCopyStr.Name = "textCopyStr";
			this.textCopyStr.Size = new System.Drawing.Size(274, 19);
			this.textCopyStr.TabIndex = 0;
			this.textCopyStr.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// copyStrSelect
			// 
			this.copyStrSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.copyStrSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.copyStrSelect.FormattingEnabled = true;
			this.copyStrSelect.Items.AddRange(new object[] {
            "ファイル名のみコピーする",
            "1番目の形式でコピーする",
            "2番目の形式でコピーする",
            "3番目の形式でコピーする",
            "4番目の形式でコピーする",
            "5番目の形式でコピーする",
            "6番目の形式でコピーする",
            "7番目の形式でコピーする",
            "8番目の形式でコピーする",
            "9番目の形式でコピーする"});
			this.copyStrSelect.Location = new System.Drawing.Point(6, 24);
			this.copyStrSelect.Name = "copyStrSelect";
			this.copyStrSelect.Size = new System.Drawing.Size(274, 20);
			this.copyStrSelect.TabIndex = 7;
			this.copyStrSelect.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.copyStrSelect);
			this.groupBox5.Controls.Add(this.textCopyStr);
			this.groupBox5.Location = new System.Drawing.Point(3, 392);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(296, 84);
			this.groupBox5.TabIndex = 13;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "コピー文の選択・編集";
			// 
			// checkBox9
			// 
			this.checkBox9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox9.AutoSize = true;
			this.checkBox9.Location = new System.Drawing.Point(194, 75);
			this.checkBox9.Name = "checkBox9";
			this.checkBox9.Size = new System.Drawing.Size(96, 16);
			this.checkBox9.TabIndex = 23;
			this.checkBox9.Text = "ループ再生する";
			this.checkBox9.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(100, 18);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(190, 43);
			this.button2.TabIndex = 10;
			this.button2.Text = "停止■";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// tabCategorySE
			// 
			this.tabCategorySE.Appearance = System.Windows.Forms.Appearance.Button;
			this.tabCategorySE.Checked = true;
			this.tabCategorySE.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tabCategorySE.Location = new System.Drawing.Point(329, 2);
			this.tabCategorySE.Name = "tabCategorySE";
			this.tabCategorySE.Size = new System.Drawing.Size(59, 28);
			this.tabCategorySE.TabIndex = 18;
			this.tabCategorySE.Text = "SE";
			this.tabCategorySE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.tabCategorySE.UseVisualStyleBackColor = true;
			this.tabCategorySE.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			this.tabCategorySE.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBox2_MouseDown);
			// 
			// tabCategoryBGM
			// 
			this.tabCategoryBGM.Appearance = System.Windows.Forms.Appearance.Button;
			this.tabCategoryBGM.Location = new System.Drawing.Point(390, 2);
			this.tabCategoryBGM.Name = "tabCategoryBGM";
			this.tabCategoryBGM.Size = new System.Drawing.Size(59, 28);
			this.tabCategoryBGM.TabIndex = 19;
			this.tabCategoryBGM.Text = "BGM";
			this.tabCategoryBGM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.tabCategoryBGM.UseVisualStyleBackColor = true;
			this.tabCategoryBGM.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
			this.tabCategoryBGM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBox3_MouseDown);
			// 
			// tabCategoryBGV
			// 
			this.tabCategoryBGV.Appearance = System.Windows.Forms.Appearance.Button;
			this.tabCategoryBGV.Location = new System.Drawing.Point(451, 2);
			this.tabCategoryBGV.Name = "tabCategoryBGV";
			this.tabCategoryBGV.Size = new System.Drawing.Size(59, 28);
			this.tabCategoryBGV.TabIndex = 20;
			this.tabCategoryBGV.Text = "BGV";
			this.tabCategoryBGV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.tabCategoryBGV.UseVisualStyleBackColor = true;
			this.tabCategoryBGV.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			this.tabCategoryBGV.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBox4_MouseDown);
			// 
			// tabCategoryUSEFULL
			// 
			this.tabCategoryUSEFULL.Appearance = System.Windows.Forms.Appearance.Button;
			this.tabCategoryUSEFULL.Location = new System.Drawing.Point(512, 2);
			this.tabCategoryUSEFULL.Name = "tabCategoryUSEFULL";
			this.tabCategoryUSEFULL.Size = new System.Drawing.Size(59, 28);
			this.tabCategoryUSEFULL.TabIndex = 22;
			this.tabCategoryUSEFULL.Text = "BGV";
			this.tabCategoryUSEFULL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.tabCategoryUSEFULL.UseVisualStyleBackColor = true;
			this.tabCategoryUSEFULL.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
			this.tabCategoryUSEFULL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBox6_MouseDown);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.trackBar1);
			this.groupBox1.Controls.Add(this.checkBox9);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Location = new System.Drawing.Point(3, 482);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(296, 150);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "音の再生・停止・音量";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 109);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(197, 12);
			this.label1.TabIndex = 27;
			this.label1.Text = "音量ボリューム  (wavには適用されません";
			// 
			// trackBar1
			// 
			this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackBar1.Location = new System.Drawing.Point(7, 121);
			this.trackBar1.Maximum = 255;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(270, 45);
			this.trackBar1.TabIndex = 26;
			this.trackBar1.TickFrequency = 10;
			this.trackBar1.Value = 255;
			this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
			// 
			// tabCategoryFAV
			// 
			this.tabCategoryFAV.Appearance = System.Windows.Forms.Appearance.Button;
			this.tabCategoryFAV.Location = new System.Drawing.Point(573, 2);
			this.tabCategoryFAV.Name = "tabCategoryFAV";
			this.tabCategoryFAV.Size = new System.Drawing.Size(59, 28);
			this.tabCategoryFAV.TabIndex = 25;
			this.tabCategoryFAV.Text = "BGV";
			this.tabCategoryFAV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.tabCategoryFAV.UseVisualStyleBackColor = true;
			this.tabCategoryFAV.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.オプション設定ToolStripMenuItem,
            this.menuItemCombo1});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(949, 30);
			this.menuStrip1.TabIndex = 27;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// オプション設定ToolStripMenuItem
			// 
			this.オプション設定ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCheck1,
            this.menuItemCheck2,
            this.menuItemCheck3,
            this.menuItemCheck4});
			this.オプション設定ToolStripMenuItem.Name = "オプション設定ToolStripMenuItem";
			this.オプション設定ToolStripMenuItem.Size = new System.Drawing.Size(104, 26);
			this.オプション設定ToolStripMenuItem.Text = "オプション設定";
			// 
			// menuItemCheck1
			// 
			this.menuItemCheck1.Name = "menuItemCheck1";
			this.menuItemCheck1.Size = new System.Drawing.Size(316, 22);
			this.menuItemCheck1.Text = "コピー時に自動的に秀丸に貼付けを行う";
			this.menuItemCheck1.Click += new System.EventHandler(this.menuItemCheck1_Click);
			// 
			// menuItemCheck2
			// 
			this.menuItemCheck2.Name = "menuItemCheck2";
			this.menuItemCheck2.Size = new System.Drawing.Size(316, 22);
			this.menuItemCheck2.Text = "ウインドウを常に一番上に表示する";
			this.menuItemCheck2.Click += new System.EventHandler(this.menuItemCheck2_Click);
			// 
			// menuItemCheck3
			// 
			this.menuItemCheck3.Name = "menuItemCheck3";
			this.menuItemCheck3.Size = new System.Drawing.Size(316, 22);
			this.menuItemCheck3.Text = "音の種類切替時にコピー分も切り替える";
			this.menuItemCheck3.Click += new System.EventHandler(this.menuItemCheck3_Click);
			// 
			// menuItemCheck4
			// 
			this.menuItemCheck4.Checked = true;
			this.menuItemCheck4.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuItemCheck4.Name = "menuItemCheck4";
			this.menuItemCheck4.Size = new System.Drawing.Size(316, 22);
			this.menuItemCheck4.Text = "音をリストで選択した時に自動的に再生する";
			this.menuItemCheck4.Click += new System.EventHandler(this.menuItemCheck4_Click);
			// 
			// menuItemCombo1
			// 
			this.menuItemCombo1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.menuItemCombo1.Name = "menuItemCombo1";
			this.menuItemCombo1.Size = new System.Drawing.Size(210, 26);
			this.menuItemCombo1.Text = "検索はここ (正規表現つかえる！";
			this.menuItemCombo1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.menuItemCombo1_KeyDown);
			this.menuItemCombo1.TextChanged += new System.EventHandler(this.menuItemCombo1_TextChanged);
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.Location = new System.Drawing.Point(3, 3);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(296, 386);
			this.treeView1.TabIndex = 28;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 35);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			this.splitContainer1.Panel1.Controls.Add(this.groupBox5);
			this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel1.SizeChanged += new System.EventHandler(this.splitContainer1_Panel1_SizeChanged);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Panel2.Controls.Add(this.comboBox5);
			this.splitContainer1.Panel2.Controls.Add(this.comboBox3);
			this.splitContainer1.Panel2.SizeChanged += new System.EventHandler(this.splitContainer1_Panel2_SizeChanged);
			this.splitContainer1.Size = new System.Drawing.Size(925, 635);
			this.splitContainer1.SplitterDistance = 308;
			this.splitContainer1.TabIndex = 29;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(949, 682);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.tabCategoryFAV);
			this.Controls.Add(this.tabCategoryUSEFULL);
			this.Controls.Add(this.tabCategoryBGV);
			this.Controls.Add(this.tabCategoryBGM);
			this.Controls.Add(this.comboBox4);
			this.Controls.Add(this.tabCategorySE);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "SEViewer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ColumnHeader seFileName;
		private System.Windows.Forms.ColumnHeader seGenre;
		private System.Windows.Forms.ColumnHeader seCaption;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textCopyStr;
		private System.Windows.Forms.ComboBox copyStrSelect;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.CheckBox tabCategorySE;
		private System.Windows.Forms.CheckBox tabCategoryBGM;
		private System.Windows.Forms.CheckBox tabCategoryBGV;
		private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.CheckBox tabCategoryUSEFULL;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader seGenre2;
        private System.Windows.Forms.ComboBox comboBox5;
		private System.Windows.Forms.CheckBox tabCategoryFAV;
		private System.Windows.Forms.CheckBox checkBox9;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem オプション設定ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem menuItemCheck1;
		private System.Windows.Forms.ToolStripMenuItem menuItemCheck2;
		private System.Windows.Forms.ToolStripMenuItem menuItemCheck3;
		private System.Windows.Forms.ToolStripMenuItem menuItemCheck4;
		private System.Windows.Forms.ToolStripComboBox menuItemCombo1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
	}
}

