
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
            this.내보내기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundBorderPanel2 = new TcPCM_Connect.RoundBorderPanel();
            this.btn_Export = new CustomControls.RJControls.RJButton();
            this.pb_Refresh = new System.Windows.Forms.PictureBox();
            this.cb_Mode = new System.Windows.Forms.CheckBox();
            this.btn_Configuration = new CustomControls.RJControls.RJButton();
            this.dgv_BaicInfo = new System.Windows.Forms.DataGridView();
            this.tv_Bom = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1.SuspendLayout();
            this.roundBorderPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Refresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BaicInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.내보내기ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 28);
            // 
            // 내보내기ToolStripMenuItem
            // 
            this.내보내기ToolStripMenuItem.Name = "내보내기ToolStripMenuItem";
            this.내보내기ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.내보내기ToolStripMenuItem.Text = "Import";
            this.내보내기ToolStripMenuItem.Click += new System.EventHandler(this.내보내기ToolStripMenuItem_Click);
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
            this.roundBorderPanel2.Controls.Add(this.btn_Export);
            this.roundBorderPanel2.Controls.Add(this.pb_Refresh);
            this.roundBorderPanel2.Controls.Add(this.cb_Mode);
            this.roundBorderPanel2.Controls.Add(this.btn_Configuration);
            this.roundBorderPanel2.Controls.Add(this.dgv_BaicInfo);
            this.roundBorderPanel2.Controls.Add(this.tv_Bom);
            this.roundBorderPanel2.IsFill = true;
            this.roundBorderPanel2.Location = new System.Drawing.Point(14, 13);
            this.roundBorderPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.roundBorderPanel2.Name = "roundBorderPanel2";
            this.roundBorderPanel2.Size = new System.Drawing.Size(1032, 596);
            this.roundBorderPanel2.TabIndex = 60;
            // 
            // btn_Export
            // 
            this.btn_Export.AllowDrop = true;
            this.btn_Export.BackColor = System.Drawing.Color.White;
            this.btn_Export.BackgroundColor = System.Drawing.Color.White;
            this.btn_Export.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Export.BorderRadius = 5;
            this.btn_Export.BorderSize = 1;
            this.btn_Export.FlatAppearance.BorderSize = 0;
            this.btn_Export.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Export.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Export.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Export.Location = new System.Drawing.Point(268, 42);
            this.btn_Export.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(114, 25);
            this.btn_Export.TabIndex = 65;
            this.btn_Export.Text = "엑셀 Export";
            this.btn_Export.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Export.UseVisualStyleBackColor = false;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // pb_Refresh
            // 
            this.pb_Refresh.BackColor = System.Drawing.Color.Transparent;
            this.pb_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("pb_Refresh.Image")));
            this.pb_Refresh.Location = new System.Drawing.Point(39, 41);
            this.pb_Refresh.Name = "pb_Refresh";
            this.pb_Refresh.Size = new System.Drawing.Size(28, 24);
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
            this.cb_Mode.Location = new System.Drawing.Point(62, 38);
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
            this.btn_Configuration.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Configuration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Configuration.Location = new System.Drawing.Point(825, 19);
            this.btn_Configuration.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Configuration.Name = "btn_Configuration";
            this.btn_Configuration.Size = new System.Drawing.Size(177, 39);
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
            this.dgv_BaicInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_BaicInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_BaicInfo.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_BaicInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_BaicInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_BaicInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_BaicInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_BaicInfo.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_BaicInfo.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_BaicInfo.EnableHeadersVisualStyles = false;
            this.dgv_BaicInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.dgv_BaicInfo.Location = new System.Drawing.Point(268, 75);
            this.dgv_BaicInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgv_BaicInfo.Name = "dgv_BaicInfo";
            this.dgv_BaicInfo.ReadOnly = true;
            this.dgv_BaicInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.PaleVioletRed;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_BaicInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_BaicInfo.RowHeadersVisible = false;
            this.dgv_BaicInfo.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_BaicInfo.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv_BaicInfo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_BaicInfo.RowTemplate.Height = 35;
            this.dgv_BaicInfo.Size = new System.Drawing.Size(734, 506);
            this.dgv_BaicInfo.TabIndex = 61;
            this.dgv_BaicInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_BaicInfo_CellClick);
            this.dgv_BaicInfo.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_UserInfo_CellDoubleClick);
            this.dgv_BaicInfo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_UserInfo_CellFormatting);
            // 
            // tv_Bom
            // 
            this.tv_Bom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tv_Bom.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tv_Bom.Location = new System.Drawing.Point(39, 75);
            this.tv_Bom.Name = "tv_Bom";
            this.tv_Bom.Size = new System.Drawing.Size(223, 506);
            this.tv_Bom.TabIndex = 60;
            this.tv_Bom.DoubleClick += new System.EventHandler(this.tv_Bom_DoubleClick);
            this.tv_Bom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tv_Bom_MouseDown);
            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1059, 624);
            this.Controls.Add(this.roundBorderPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
        private CustomControls.RJControls.RJButton btn_Export;
        private System.Windows.Forms.PictureBox pb_Refresh;
    }
}