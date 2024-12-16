namespace TcPCM_Connect
{
    partial class DetailSearch
    {

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
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.labelCaption = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelBody = new System.Windows.Forms.Panel();
            this.panel_Select = new System.Windows.Forms.Panel();
            this.date_modified = new TcPCM_Connect.Controller.DateRangePanel(this.components);
            this.btn_Cancel = new CustomControls.RJControls.RJButton();
            this.btn_Create = new CustomControls.RJControls.RJButton();
            this.dateRangePanel1 = new TcPCM_Connect.Controller.DateRangePanel(this.components);
            this.dateRangePanel2 = new TcPCM_Connect.Controller.DateRangePanel(this.components);
            this.panelTitleBar.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panel_Select.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitleBar
            // 
            this.panelTitleBar.BackColor = System.Drawing.Color.DimGray;
            this.panelTitleBar.Controls.Add(this.labelCaption);
            this.panelTitleBar.Controls.Add(this.btnClose);
            this.panelTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitleBar.Location = new System.Drawing.Point(0, 0);
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.Size = new System.Drawing.Size(696, 38);
            this.panelTitleBar.TabIndex = 3;
            this.panelTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseDown);
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelCaption.ForeColor = System.Drawing.Color.White;
            this.labelCaption.Location = new System.Drawing.Point(10, 7);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(107, 25);
            this.labelCaption.TabIndex = 4;
            this.labelCaption.Text = "조회조건 설정";
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(79)))), ((int)(((byte)(95)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(649, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(47, 38);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.White;
            this.panelBody.Controls.Add(this.panel_Select);
            this.panelBody.Controls.Add(this.btn_Cancel);
            this.panelBody.Controls.Add(this.btn_Create);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(12, 9, 0, 0);
            this.panelBody.Size = new System.Drawing.Size(696, 418);
            this.panelBody.TabIndex = 5;
            // 
            // panel_Select
            // 
            this.panel_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Select.BackColor = System.Drawing.Color.White;
            this.panel_Select.Controls.Add(this.dateRangePanel2);
            this.panel_Select.Controls.Add(this.dateRangePanel1);
            this.panel_Select.Controls.Add(this.date_modified);
            this.panel_Select.Location = new System.Drawing.Point(10, 47);
            this.panel_Select.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_Select.Name = "panel_Select";
            this.panel_Select.Size = new System.Drawing.Size(675, 320);
            this.panel_Select.TabIndex = 65;
            // 
            // date_modified
            // 
            this.date_modified.DateOptions = new string[] {
        "오늘",
        "당월",
        "전월",
        "3개월",
        "6개월",
        "1년"};
            this.date_modified.EndDate = new System.DateTime(((long)(0)));
            this.date_modified.LabelText = "수정일자";
            this.date_modified.Location = new System.Drawing.Point(14, 65);
            this.date_modified.Name = "date_modified";
            this.date_modified.SelectedDateOption = "오늘";
            this.date_modified.Size = new System.Drawing.Size(647, 44);
            this.date_modified.StartDate = new System.DateTime(((long)(0)));
            this.date_modified.TabIndex = 4;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.AllowDrop = true;
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel.BackColor = System.Drawing.Color.White;
            this.btn_Cancel.BackgroundColor = System.Drawing.Color.White;
            this.btn_Cancel.BorderColor = System.Drawing.Color.Crimson;
            this.btn_Cancel.BorderRadius = 5;
            this.btn_Cancel.BorderSize = 1;
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Cancel.ForeColor = System.Drawing.Color.Crimson;
            this.btn_Cancel.Location = new System.Drawing.Point(616, 376);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(62, 31);
            this.btn_Cancel.TabIndex = 64;
            this.btn_Cancel.Text = "취소";
            this.btn_Cancel.TextColor = System.Drawing.Color.Crimson;
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btn_Create
            // 
            this.btn_Create.AllowDrop = true;
            this.btn_Create.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Create.BackColor = System.Drawing.Color.White;
            this.btn_Create.BackgroundColor = System.Drawing.Color.White;
            this.btn_Create.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btn_Create.BorderRadius = 5;
            this.btn_Create.BorderSize = 1;
            this.btn_Create.FlatAppearance.BorderSize = 0;
            this.btn_Create.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Create.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Create.ForeColor = System.Drawing.Color.DodgerBlue;
            this.btn_Create.Location = new System.Drawing.Point(549, 376);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(62, 31);
            this.btn_Create.TabIndex = 63;
            this.btn_Create.Text = "조회";
            this.btn_Create.TextColor = System.Drawing.Color.DodgerBlue;
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // dateRangePanel1
            // 
            this.dateRangePanel1.DateOptions = new string[] {
        "오늘",
        "당월",
        "전월",
        "3개월",
        "6개월",
        "1년"};
            this.dateRangePanel1.EndDate = new System.DateTime(((long)(0)));
            this.dateRangePanel1.LabelText = "유효일자";
            this.dateRangePanel1.Location = new System.Drawing.Point(14, 115);
            this.dateRangePanel1.Name = "dateRangePanel1";
            this.dateRangePanel1.SelectedDateOption = "오늘";
            this.dateRangePanel1.Size = new System.Drawing.Size(647, 44);
            this.dateRangePanel1.StartDate = new System.DateTime(((long)(0)));
            this.dateRangePanel1.TabIndex = 5;
            // 
            // dateRangePanel2
            // 
            this.dateRangePanel2.DateOptions = new string[] {
        "오늘",
        "당월",
        "전월",
        "3개월",
        "6개월",
        "1년"};
            this.dateRangePanel2.EndDate = new System.DateTime(((long)(0)));
            this.dateRangePanel2.LabelText = "생성일자";
            this.dateRangePanel2.Location = new System.Drawing.Point(14, 13);
            this.dateRangePanel2.Name = "dateRangePanel2";
            this.dateRangePanel2.SelectedDateOption = "오늘";
            this.dateRangePanel2.Size = new System.Drawing.Size(647, 44);
            this.dateRangePanel2.StartDate = new System.DateTime(((long)(0)));
            this.dateRangePanel2.TabIndex = 6;
            // 
            // DetailSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 418);
            this.Controls.Add(this.panelTitleBar);
            this.Controls.Add(this.panelBody);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DetailSearch";
            this.Text = "ConfigSetting";
            this.Load += new System.EventHandler(this.ConfigSetting_Load);
            this.panelTitleBar.ResumeLayout(false);
            this.panelTitleBar.PerformLayout();
            this.panelBody.ResumeLayout(false);
            this.panel_Select.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTitleBar;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelBody;
        private CustomControls.RJControls.RJButton btn_Create;
        private CustomControls.RJControls.RJButton btn_Cancel;
        private System.Windows.Forms.Panel panel_Select;
        private System.ComponentModel.IContainer components;
        private Controller.DateRangePanel date_modified;
        private Controller.DateRangePanel dateRangePanel1;
        private Controller.DateRangePanel dateRangePanel2;
    }
}