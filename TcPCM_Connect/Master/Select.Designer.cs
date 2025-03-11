namespace TcPCM_Connect
{
    partial class Select
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
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.labelCaption = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelBody = new System.Windows.Forms.Panel();
            this.panel_Select = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.txtPartName = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.txtMachine = new System.Windows.Forms.TextBox();
            this.rjDatePicker2 = new CustomControls.RJControls.RJDatePicker();
            this.combo_date = new CustomControls.RJControls.RJComboBox();
            this.rjDatePicker1 = new CustomControls.RJControls.RJDatePicker();
            this.btn_Cancel = new CustomControls.RJControls.RJButton();
            this.btn_Create = new CustomControls.RJControls.RJButton();
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
            this.panelTitleBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.Size = new System.Drawing.Size(789, 48);
            this.panelTitleBar.TabIndex = 3;
            this.panelTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseDown);
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelCaption.ForeColor = System.Drawing.Color.White;
            this.labelCaption.Location = new System.Drawing.Point(11, 9);
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
            this.btnClose.Location = new System.Drawing.Point(735, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(54, 48);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelBody.Controls.Add(this.panel_Select);
            this.panelBody.Controls.Add(this.btn_Cancel);
            this.panelBody.Controls.Add(this.btn_Create);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 0);
            this.panelBody.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(14, 11, 0, 0);
            this.panelBody.Size = new System.Drawing.Size(789, 388);
            this.panelBody.TabIndex = 5;
            // 
            // panel_Select
            // 
            this.panel_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Select.Controls.Add(this.rjDatePicker2);
            this.panel_Select.Controls.Add(this.combo_date);
            this.panel_Select.Controls.Add(this.rjDatePicker1);
            this.panel_Select.Controls.Add(this.label5);
            this.panel_Select.Controls.Add(this.label4);
            this.panel_Select.Controls.Add(this.label3);
            this.panel_Select.Controls.Add(this.label2);
            this.panel_Select.Controls.Add(this.label1);
            this.panel_Select.Controls.Add(this.txtMachine);
            this.panel_Select.Controls.Add(this.txtCategory);
            this.panel_Select.Controls.Add(this.txtName);
            this.panel_Select.Controls.Add(this.txtPartName);
            this.panel_Select.Controls.Add(this.txtInfo);
            this.panel_Select.Location = new System.Drawing.Point(11, 59);
            this.panel_Select.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_Select.Name = "panel_Select";
            this.panel_Select.Size = new System.Drawing.Size(763, 265);
            this.panel_Select.TabIndex = 65;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "검색기간";
            this.label5.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "검색기간";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "검색기간";
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "검색기간";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "검색기간";
            // 
            // txtInfo
            // 
            this.txtInfo.ForeColor = System.Drawing.Color.Black;
            this.txtInfo.Location = new System.Drawing.Point(112, 21);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(368, 25);
            this.txtInfo.TabIndex = 6;
            this.txtInfo.Visible = false;
            // 
            // txtPartName
            // 
            this.txtPartName.ForeColor = System.Drawing.Color.Black;
            this.txtPartName.Location = new System.Drawing.Point(112, 66);
            this.txtPartName.Name = "txtPartName";
            this.txtPartName.Size = new System.Drawing.Size(368, 25);
            this.txtPartName.TabIndex = 6;
            this.txtPartName.Visible = false;
            // 
            // txtName
            // 
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.Location = new System.Drawing.Point(112, 111);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(368, 25);
            this.txtName.TabIndex = 6;
            this.txtName.Visible = false;
            // 
            // txtCategory
            // 
            this.txtCategory.ForeColor = System.Drawing.Color.Black;
            this.txtCategory.Location = new System.Drawing.Point(112, 156);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(368, 25);
            this.txtCategory.TabIndex = 6;
            this.txtCategory.Visible = false;
            // 
            // txtMachine
            // 
            this.txtMachine.ForeColor = System.Drawing.Color.Black;
            this.txtMachine.Location = new System.Drawing.Point(112, 201);
            this.txtMachine.Name = "txtMachine";
            this.txtMachine.Size = new System.Drawing.Size(368, 25);
            this.txtMachine.TabIndex = 6;
            this.txtMachine.Visible = false;
            // 
            // rjDatePicker2
            // 
            this.rjDatePicker2.BorderColor = System.Drawing.Color.Black;
            this.rjDatePicker2.BorderSize = 1;
            this.rjDatePicker2.Font = new System.Drawing.Font("굴림", 9.5F);
            this.rjDatePicker2.Location = new System.Drawing.Point(486, 12);
            this.rjDatePicker2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rjDatePicker2.MinimumSize = new System.Drawing.Size(4, 35);
            this.rjDatePicker2.Name = "rjDatePicker2";
            this.rjDatePicker2.Size = new System.Drawing.Size(235, 35);
            this.rjDatePicker2.SkinColor = System.Drawing.Color.White;
            this.rjDatePicker2.TabIndex = 2;
            this.rjDatePicker2.TextColor = System.Drawing.Color.Black;
            // 
            // combo_date
            // 
            this.combo_date.BackColor = System.Drawing.Color.White;
            this.combo_date.BorderColor = System.Drawing.Color.Black;
            this.combo_date.BorderSize = 1;
            this.combo_date.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.combo_date.Font = new System.Drawing.Font("굴림", 10F);
            this.combo_date.ForeColor = System.Drawing.Color.DimGray;
            this.combo_date.IconColor = System.Drawing.Color.Black;
            this.combo_date.Items.AddRange(new object[] {
            "오늘",
            "당월",
            "전월",
            "3개월",
            "6개월",
            "1년"});
            this.combo_date.ListBackColor = System.Drawing.Color.White;
            this.combo_date.ListTextColor = System.Drawing.Color.DimGray;
            this.combo_date.Location = new System.Drawing.Point(96, 12);
            this.combo_date.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.combo_date.Name = "combo_date";
            this.combo_date.Padding = new System.Windows.Forms.Padding(1);
            this.combo_date.Size = new System.Drawing.Size(111, 35);
            this.combo_date.TabIndex = 0;
            this.combo_date.Texts = "오늘";
            this.combo_date.OnSelectedIndexChanged += new System.EventHandler(this.combo_date_OnSelectedIndexChanged);
            // 
            // rjDatePicker1
            // 
            this.rjDatePicker1.BorderColor = System.Drawing.Color.Black;
            this.rjDatePicker1.BorderSize = 1;
            this.rjDatePicker1.Font = new System.Drawing.Font("굴림", 9.5F);
            this.rjDatePicker1.Location = new System.Drawing.Point(245, 12);
            this.rjDatePicker1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rjDatePicker1.MinimumSize = new System.Drawing.Size(4, 35);
            this.rjDatePicker1.Name = "rjDatePicker1";
            this.rjDatePicker1.Size = new System.Drawing.Size(235, 35);
            this.rjDatePicker1.SkinColor = System.Drawing.Color.White;
            this.rjDatePicker1.TabIndex = 1;
            this.rjDatePicker1.TextColor = System.Drawing.Color.Black;
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
            this.btn_Cancel.Location = new System.Drawing.Point(697, 335);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(71, 39);
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
            this.btn_Create.Location = new System.Drawing.Point(619, 335);
            this.btn_Create.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(71, 39);
            this.btn_Create.TabIndex = 63;
            this.btn_Create.Text = "조회";
            this.btn_Create.TextColor = System.Drawing.Color.DodgerBlue;
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // Select
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 388);
            this.Controls.Add(this.panelTitleBar);
            this.Controls.Add(this.panelBody);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Select";
            this.Text = "ConfigSetting";
            this.Load += new System.EventHandler(this.ConfigSetting_Load);
            this.panelTitleBar.ResumeLayout(false);
            this.panelTitleBar.PerformLayout();
            this.panelBody.ResumeLayout(false);
            this.panel_Select.ResumeLayout(false);
            this.panel_Select.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private CustomControls.RJControls.RJDatePicker rjDatePicker1;
        private CustomControls.RJControls.RJComboBox combo_date;
        private CustomControls.RJControls.RJDatePicker rjDatePicker2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.TextBox txtMachine;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPartName;
    }
}