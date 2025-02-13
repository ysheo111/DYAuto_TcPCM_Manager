
namespace TcPCM_Connect
{
    partial class frmDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDashboard));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.테스트ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXCEL올리기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXCEL내려받기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXCELBulk업로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.공정라이브러리ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bOMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.업로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXCEL다운로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.작업지지서ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.내보내기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.basicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.injectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diecastingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXCEL업로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundBorderPanel2 = new TcPCM_Connect.RoundBorderPanel();
            this.p_lang = new System.Windows.Forms.Panel();
            this.rb_CHN = new System.Windows.Forms.RadioButton();
            this.rb_ENG = new System.Windows.Forms.RadioButton();
            this.rb_KOR = new System.Windows.Forms.RadioButton();
            this.searchButton1 = new TcPCM_Connect.Controller.SearchButton();
            this.pb_Refresh = new System.Windows.Forms.PictureBox();
            this.btn_Configuration = new CustomControls.RJControls.RJButton();
            this.dgv_BaicInfo = new System.Windows.Forms.DataGridView();
            this.tv_Bom = new System.Windows.Forms.TreeView();
            this.btn_Create = new CustomControls.RJControls.RJButton();
            this.contextMenuStrip1.SuspendLayout();
            this.roundBorderPanel2.SuspendLayout();
            this.p_lang.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Refresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BaicInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.테스트ToolStripMenuItem,
            this.공정라이브러리ToolStripMenuItem,
            this.bOMToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(199, 70);
            // 
            // 테스트ToolStripMenuItem
            // 
            this.테스트ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eXCEL업로드ToolStripMenuItem,
            this.eXCEL내려받기ToolStripMenuItem,
            this.eXCELBulk업로드ToolStripMenuItem,
            this.eXCEL올리기ToolStripMenuItem});
            this.테스트ToolStripMenuItem.Name = "테스트ToolStripMenuItem";
            this.테스트ToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.테스트ToolStripMenuItem.Text = "부품원가계산서";
            this.테스트ToolStripMenuItem.Click += new System.EventHandler(this.테스트ToolStripMenuItem_Click);
            // 
            // eXCEL올리기ToolStripMenuItem
            // 
            this.eXCEL올리기ToolStripMenuItem.Name = "eXCEL올리기ToolStripMenuItem";
            this.eXCEL올리기ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.eXCEL올리기ToolStripMenuItem.Text = "EXCEL 단일 업로드";
            this.eXCEL올리기ToolStripMenuItem.Click += new System.EventHandler(this.eXCEL올리기ToolStripMenuItem_Click);
            // 
            // eXCEL내려받기ToolStripMenuItem
            // 
            this.eXCEL내려받기ToolStripMenuItem.Name = "eXCEL내려받기ToolStripMenuItem";
            this.eXCEL내려받기ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.eXCEL내려받기ToolStripMenuItem.Text = "EXCEL 다운로드";
            this.eXCEL내려받기ToolStripMenuItem.Click += new System.EventHandler(this.eXCEL내려받기ToolStripMenuItem_Click);
            // 
            // eXCELBulk업로드ToolStripMenuItem
            // 
            this.eXCELBulk업로드ToolStripMenuItem.Name = "eXCELBulk업로드ToolStripMenuItem";
            this.eXCELBulk업로드ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.eXCELBulk업로드ToolStripMenuItem.Text = "EXCEL Bulk 업로드";
            this.eXCELBulk업로드ToolStripMenuItem.Click += new System.EventHandler(this.eXCELBulk업로드ToolStripMenuItem_Click);
            // 
            // 공정라이브러리ToolStripMenuItem
            // 
            this.공정라이브러리ToolStripMenuItem.Name = "공정라이브러리ToolStripMenuItem";
            this.공정라이브러리ToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.공정라이브러리ToolStripMenuItem.Text = "공정라이브러리 업로드";
            this.공정라이브러리ToolStripMenuItem.Click += new System.EventHandler(this.공정라이브러리ToolStripMenuItem_Click);
            // 
            // bOMToolStripMenuItem
            // 
            this.bOMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.업로드ToolStripMenuItem,
            this.eXCEL다운로드ToolStripMenuItem});
            this.bOMToolStripMenuItem.Name = "bOMToolStripMenuItem";
            this.bOMToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.bOMToolStripMenuItem.Text = "BOM";
            // 
            // 업로드ToolStripMenuItem
            // 
            this.업로드ToolStripMenuItem.Name = "업로드ToolStripMenuItem";
            this.업로드ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.업로드ToolStripMenuItem.Text = "EXCEL 업로드";
            this.업로드ToolStripMenuItem.Click += new System.EventHandler(this.업로드ToolStripMenuItem_Click);
            // 
            // eXCEL다운로드ToolStripMenuItem
            // 
            this.eXCEL다운로드ToolStripMenuItem.Name = "eXCEL다운로드ToolStripMenuItem";
            this.eXCEL다운로드ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.eXCEL다운로드ToolStripMenuItem.Text = "EXCEL 다운로드";
            this.eXCEL다운로드ToolStripMenuItem.Click += new System.EventHandler(this.eXCEL다운로드ToolStripMenuItem_Click);
            // 
            // 작업지지서ImportToolStripMenuItem
            // 
            this.작업지지서ImportToolStripMenuItem.Name = "작업지지서ImportToolStripMenuItem";
            this.작업지지서ImportToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // 내보내기ToolStripMenuItem
            // 
            this.내보내기ToolStripMenuItem.Name = "내보내기ToolStripMenuItem";
            this.내보내기ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // eToolStripMenuItem
            // 
            this.eToolStripMenuItem.Name = "eToolStripMenuItem";
            this.eToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // basicToolStripMenuItem
            // 
            this.basicToolStripMenuItem.Name = "basicToolStripMenuItem";
            this.basicToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // injectionToolStripMenuItem
            // 
            this.injectionToolStripMenuItem.Name = "injectionToolStripMenuItem";
            this.injectionToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // pressToolStripMenuItem
            // 
            this.pressToolStripMenuItem.Name = "pressToolStripMenuItem";
            this.pressToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // coreToolStripMenuItem
            // 
            this.coreToolStripMenuItem.Name = "coreToolStripMenuItem";
            this.coreToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // diecastingToolStripMenuItem
            // 
            this.diecastingToolStripMenuItem.Name = "diecastingToolStripMenuItem";
            this.diecastingToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // eXCEL업로드ToolStripMenuItem
            // 
            this.eXCEL업로드ToolStripMenuItem.Name = "eXCEL업로드ToolStripMenuItem";
            this.eXCEL업로드ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.eXCEL업로드ToolStripMenuItem.Text = "EXCEL 업로드";
            this.eXCEL업로드ToolStripMenuItem.Click += new System.EventHandler(this.eXCEL업로드ToolStripMenuItem_Click);
            // 
            // roundBorderPanel2
            // 
            this.roundBorderPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundBorderPanel2.BackgroundColor = System.Drawing.Color.White;
            this.roundBorderPanel2.BorderColor = System.Drawing.Color.Gainsboro;
            this.roundBorderPanel2.BorderRadius = 10;
            this.roundBorderPanel2.BorderSize = 1;
            this.roundBorderPanel2.Controls.Add(this.btn_Create);
            this.roundBorderPanel2.Controls.Add(this.p_lang);
            this.roundBorderPanel2.Controls.Add(this.searchButton1);
            this.roundBorderPanel2.Controls.Add(this.pb_Refresh);
            this.roundBorderPanel2.Controls.Add(this.btn_Configuration);
            this.roundBorderPanel2.Controls.Add(this.dgv_BaicInfo);
            this.roundBorderPanel2.Controls.Add(this.tv_Bom);
            this.roundBorderPanel2.IsFill = true;
            this.roundBorderPanel2.Location = new System.Drawing.Point(12, 10);
            this.roundBorderPanel2.Name = "roundBorderPanel2";
            this.roundBorderPanel2.Size = new System.Drawing.Size(903, 477);
            this.roundBorderPanel2.TabIndex = 60;
            // 
            // p_lang
            // 
            this.p_lang.BackColor = System.Drawing.Color.Transparent;
            this.p_lang.Controls.Add(this.rb_CHN);
            this.p_lang.Controls.Add(this.rb_ENG);
            this.p_lang.Controls.Add(this.rb_KOR);
            this.p_lang.Location = new System.Drawing.Point(64, 29);
            this.p_lang.Name = "p_lang";
            this.p_lang.Size = new System.Drawing.Size(166, 25);
            this.p_lang.TabIndex = 67;
            // 
            // rb_CHN
            // 
            this.rb_CHN.AutoSize = true;
            this.rb_CHN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rb_CHN.Location = new System.Drawing.Point(114, 3);
            this.rb_CHN.Name = "rb_CHN";
            this.rb_CHN.Size = new System.Drawing.Size(51, 19);
            this.rb_CHN.TabIndex = 2;
            this.rb_CHN.Tag = TcPCM_Connect_Global.Bom.ExportLang.CHN;
            this.rb_CHN.Text = "CHN";
            this.rb_CHN.UseVisualStyleBackColor = true;
            this.rb_CHN.CheckedChanged += new System.EventHandler(this.rb_lang_CheckedChanged);
            // 
            // rb_ENG
            // 
            this.rb_ENG.AutoSize = true;
            this.rb_ENG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rb_ENG.Location = new System.Drawing.Point(59, 3);
            this.rb_ENG.Name = "rb_ENG";
            this.rb_ENG.Size = new System.Drawing.Size(51, 19);
            this.rb_ENG.TabIndex = 1;
            this.rb_ENG.Tag = TcPCM_Connect_Global.Bom.ExportLang.Eng;
            this.rb_ENG.Text = "ENG";
            this.rb_ENG.UseVisualStyleBackColor = true;
            this.rb_ENG.CheckedChanged += new System.EventHandler(this.rb_lang_CheckedChanged);
            // 
            // rb_KOR
            // 
            this.rb_KOR.AutoSize = true;
            this.rb_KOR.Checked = true;
            this.rb_KOR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rb_KOR.Location = new System.Drawing.Point(5, 3);
            this.rb_KOR.Name = "rb_KOR";
            this.rb_KOR.Size = new System.Drawing.Size(51, 19);
            this.rb_KOR.TabIndex = 0;
            this.rb_KOR.TabStop = true;
            this.rb_KOR.Tag = TcPCM_Connect_Global.Bom.ExportLang.Kor;
            this.rb_KOR.Text = "KOR";
            this.rb_KOR.UseVisualStyleBackColor = true;
            this.rb_KOR.CheckedChanged += new System.EventHandler(this.rb_lang_CheckedChanged);
            // 
            // searchButton1
            // 
            this.searchButton1.BackColor = System.Drawing.Color.Transparent;
            this.searchButton1.DetailSearchButtonBackColor = System.Drawing.Color.White;
            this.searchButton1.Font = new System.Drawing.Font("Pretendard", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.searchButton1.Location = new System.Drawing.Point(245, 19);
            this.searchButton1.Name = "searchButton1";
            this.searchButton1.PanelBackColor = System.Drawing.Color.Transparent;
            this.searchButton1.Size = new System.Drawing.Size(373, 30);
            this.searchButton1.TabIndex = 66;
            this.searchButton1.text = "";
            this.searchButton1.TextBoxBackColor = System.Drawing.Color.WhiteSmoke;
            this.searchButton1.SearchButtonClick += new System.EventHandler(this.searchButton1_SearchButtonClick);
            this.searchButton1.DetailSearchButtonClick += new System.EventHandler(this.searchButton1_DetailSearchButtonClick);
            // 
            // pb_Refresh
            // 
            this.pb_Refresh.BackColor = System.Drawing.Color.Transparent;
            this.pb_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("pb_Refresh.Image")));
            this.pb_Refresh.Location = new System.Drawing.Point(34, 29);
            this.pb_Refresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pb_Refresh.Name = "pb_Refresh";
            this.pb_Refresh.Size = new System.Drawing.Size(24, 24);
            this.pb_Refresh.TabIndex = 62;
            this.pb_Refresh.TabStop = false;
            this.pb_Refresh.Click += new System.EventHandler(this.pb_Refresh_Click);
            // 
            // btn_Configuration
            // 
            this.btn_Configuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Configuration.BackColor = System.Drawing.Color.White;
            this.btn_Configuration.BackgroundColor = System.Drawing.Color.White;
            this.btn_Configuration.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Configuration.BorderRadius = 5;
            this.btn_Configuration.BorderSize = 1;
            this.btn_Configuration.FlatAppearance.BorderSize = 0;
            this.btn_Configuration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Configuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Configuration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Configuration.Location = new System.Drawing.Point(725, 18);
            this.btn_Configuration.Name = "btn_Configuration";
            this.btn_Configuration.Size = new System.Drawing.Size(157, 31);
            this.btn_Configuration.TabIndex = 63;
            this.btn_Configuration.Text = "Configuration 설정 변경";
            this.btn_Configuration.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Configuration.UseVisualStyleBackColor = false;
            this.btn_Configuration.Click += new System.EventHandler(this.btn_Configuration_Click);
            // 
            // dgv_BaicInfo
            // 
            this.dgv_BaicInfo.AllowUserToAddRows = false;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.DimGray;
            this.dgv_BaicInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dgv_BaicInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_BaicInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_BaicInfo.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_BaicInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_BaicInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_BaicInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_BaicInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgv_BaicInfo.ColumnHeadersHeight = 40;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_BaicInfo.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgv_BaicInfo.EnableHeadersVisualStyles = false;
            this.dgv_BaicInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.dgv_BaicInfo.Location = new System.Drawing.Point(250, 60);
            this.dgv_BaicInfo.Name = "dgv_BaicInfo";
            this.dgv_BaicInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.PaleVioletRed;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_BaicInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgv_BaicInfo.RowHeadersVisible = false;
            this.dgv_BaicInfo.RowHeadersWidth = 51;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_BaicInfo.RowsDefaultCellStyle = dataGridViewCellStyle20;
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_BaicInfo.RowTemplate.Height = 35;
            this.dgv_BaicInfo.Size = new System.Drawing.Size(626, 405);
            this.dgv_BaicInfo.TabIndex = 61;
            this.dgv_BaicInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgv_BaicInfo_MouseDown);
            // 
            // tv_Bom
            // 
            this.tv_Bom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tv_Bom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tv_Bom.Location = new System.Drawing.Point(34, 60);
            this.tv_Bom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tv_Bom.Name = "tv_Bom";
            this.tv_Bom.Size = new System.Drawing.Size(196, 406);
            this.tv_Bom.TabIndex = 60;
            this.tv_Bom.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tv_Bom_AfterExpand);
            this.tv_Bom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tv_Bom_MouseDown);
            // 
            // btn_Create
            // 
            this.btn_Create.AllowDrop = true;
            this.btn_Create.BackColor = System.Drawing.Color.White;
            this.btn_Create.BackgroundColor = System.Drawing.Color.White;
            this.btn_Create.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Create.BorderRadius = 5;
            this.btn_Create.BorderSize = 1;
            this.btn_Create.FlatAppearance.BorderSize = 0;
            this.btn_Create.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Create.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Create.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Create.Location = new System.Drawing.Point(632, 18);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(87, 31);
            this.btn_Create.TabIndex = 68;
            this.btn_Create.Text = "엑셀 Import";
            this.btn_Create.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(927, 499);
            this.Controls.Add(this.roundBorderPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmDashboard";
            this.Text = "frmDashboard";
            this.Load += new System.EventHandler(this.frmDashboard_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.roundBorderPanel2.ResumeLayout(false);
            this.p_lang.ResumeLayout(false);
            this.p_lang.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Refresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BaicInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private RoundBorderPanel roundBorderPanel2;
        private System.Windows.Forms.TreeView tv_Bom;
        private System.Windows.Forms.DataGridView dgv_BaicInfo;
        private CustomControls.RJControls.RJButton btn_Configuration;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 내보내기ToolStripMenuItem;
        private System.Windows.Forms.PictureBox pb_Refresh;
        private System.Windows.Forms.ToolStripMenuItem eToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 작업지지서ImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem basicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem injectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diecastingToolStripMenuItem;
        private Controller.SearchButton searchButton1;
        private System.Windows.Forms.ToolStripMenuItem 테스트ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eXCEL내려받기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eXCEL올리기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 공정라이브러리ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bOMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 업로드ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eXCEL다운로드ToolStripMenuItem;
        private System.Windows.Forms.Panel p_lang;
        private System.Windows.Forms.RadioButton rb_CHN;
        private System.Windows.Forms.RadioButton rb_ENG;
        private System.Windows.Forms.RadioButton rb_KOR;
        private System.Windows.Forms.ToolStripMenuItem eXCELBulk업로드ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eXCEL업로드ToolStripMenuItem;
        private CustomControls.RJControls.RJButton btn_Create;
    }
}