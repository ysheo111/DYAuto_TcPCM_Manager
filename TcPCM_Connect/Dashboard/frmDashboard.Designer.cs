
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.테스트ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXCEL올리기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXCEL내려받기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.작업지지서ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.내보내기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.basicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.injectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diecastingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundBorderPanel2 = new TcPCM_Connect.RoundBorderPanel();
            this.searchButton1 = new TcPCM_Connect.Controller.SearchButton();
            this.pb_Refresh = new System.Windows.Forms.PictureBox();
            this.cb_Mode = new System.Windows.Forms.CheckBox();
            this.btn_Configuration = new CustomControls.RJControls.RJButton();
            this.dgv_BaicInfo = new System.Windows.Forms.DataGridView();
            this.tv_Bom = new System.Windows.Forms.TreeView();
            this.공정라이브러리ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.roundBorderPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Refresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BaicInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.테스트ToolStripMenuItem,
            this.공정라이브러리ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 70);
            // 
            // 테스트ToolStripMenuItem
            // 
            this.테스트ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eXCEL올리기ToolStripMenuItem,
            this.eXCEL내려받기ToolStripMenuItem});
            this.테스트ToolStripMenuItem.Name = "테스트ToolStripMenuItem";
            this.테스트ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.테스트ToolStripMenuItem.Text = "부품원가계산서";
            // 
            // eXCEL올리기ToolStripMenuItem
            // 
            this.eXCEL올리기ToolStripMenuItem.Name = "eXCEL올리기ToolStripMenuItem";
            this.eXCEL올리기ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.eXCEL올리기ToolStripMenuItem.Text = "EXCEL 올리기";
            this.eXCEL올리기ToolStripMenuItem.Click += new System.EventHandler(this.eXCEL올리기ToolStripMenuItem_Click);
            // 
            // eXCEL내려받기ToolStripMenuItem
            // 
            this.eXCEL내려받기ToolStripMenuItem.Name = "eXCEL내려받기ToolStripMenuItem";
            this.eXCEL내려받기ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.eXCEL내려받기ToolStripMenuItem.Text = "EXCEL 내려받기";
            this.eXCEL내려받기ToolStripMenuItem.Click += new System.EventHandler(this.eXCEL내려받기ToolStripMenuItem_Click);
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
            // roundBorderPanel2
            // 
            this.roundBorderPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundBorderPanel2.BackgroundColor = System.Drawing.Color.White;
            this.roundBorderPanel2.BorderColor = System.Drawing.Color.Gainsboro;
            this.roundBorderPanel2.BorderRadius = 10;
            this.roundBorderPanel2.BorderSize = 1;
            this.roundBorderPanel2.Controls.Add(this.searchButton1);
            this.roundBorderPanel2.Controls.Add(this.pb_Refresh);
            this.roundBorderPanel2.Controls.Add(this.cb_Mode);
            this.roundBorderPanel2.Controls.Add(this.btn_Configuration);
            this.roundBorderPanel2.Controls.Add(this.dgv_BaicInfo);
            this.roundBorderPanel2.Controls.Add(this.tv_Bom);
            this.roundBorderPanel2.IsFill = true;
            this.roundBorderPanel2.Location = new System.Drawing.Point(12, 10);
            this.roundBorderPanel2.Name = "roundBorderPanel2";
            this.roundBorderPanel2.Size = new System.Drawing.Size(903, 477);
            this.roundBorderPanel2.TabIndex = 60;
            // 
            // searchButton1
            // 
            this.searchButton1.BackColor = System.Drawing.Color.Transparent;
            this.searchButton1.DetailSearchButtonBackColor = System.Drawing.Color.White;
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
            // cb_Mode
            // 
            this.cb_Mode.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_Mode.AutoSize = true;
            this.cb_Mode.BackColor = System.Drawing.Color.Transparent;
            this.cb_Mode.FlatAppearance.BorderSize = 0;
            this.cb_Mode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.cb_Mode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.cb_Mode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_Mode.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cb_Mode.Image = global::TcPCM_Connect.Properties.Resources.상세1;
            this.cb_Mode.Location = new System.Drawing.Point(59, 24);
            this.cb_Mode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_Mode.Name = "cb_Mode";
            this.cb_Mode.Size = new System.Drawing.Size(56, 31);
            this.cb_Mode.TabIndex = 64;
            this.cb_Mode.UseVisualStyleBackColor = false;
            this.cb_Mode.CheckedChanged += new System.EventHandler(this.cb_Mode_CheckedChanged);
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
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.DimGray;
            this.dgv_BaicInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_BaicInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_BaicInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_BaicInfo.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_BaicInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_BaicInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_BaicInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_BaicInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_BaicInfo.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_BaicInfo.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_BaicInfo.EnableHeadersVisualStyles = false;
            this.dgv_BaicInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.dgv_BaicInfo.Location = new System.Drawing.Point(250, 60);
            this.dgv_BaicInfo.Name = "dgv_BaicInfo";
            this.dgv_BaicInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.PaleVioletRed;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_BaicInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_BaicInfo.RowHeadersVisible = false;
            this.dgv_BaicInfo.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_BaicInfo.RowsDefaultCellStyle = dataGridViewCellStyle5;
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
            // 공정라이브러리ToolStripMenuItem
            // 
            this.공정라이브러리ToolStripMenuItem.Name = "공정라이브러리ToolStripMenuItem";
            this.공정라이브러리ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.공정라이브러리ToolStripMenuItem.Text = "공정라이브러리";
            this.공정라이브러리ToolStripMenuItem.Click += new System.EventHandler(this.공정라이브러리ToolStripMenuItem_Click);
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
            this.roundBorderPanel2.PerformLayout();
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
        private System.Windows.Forms.CheckBox cb_Mode;
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
    }
}