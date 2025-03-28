﻿namespace TcPCM_Connect
{
    partial class frmDetailSearch
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
            this.dt_end = new CustomControls.RJControls.RJDatePicker();
            this.combo_date = new CustomControls.RJControls.RJComboBox();
            this.dt_start = new CustomControls.RJControls.RJDatePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Cancel = new CustomControls.RJControls.RJButton();
            this.btn_Create = new CustomControls.RJControls.RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_manufacturing = new CustomControls.RJControls.RJTextBox();
            this.cb_date = new System.Windows.Forms.CheckBox();
            this.cb_manufacturing = new System.Windows.Forms.CheckBox();
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
            this.panelTitleBar.Size = new System.Drawing.Size(690, 38);
            this.panelTitleBar.TabIndex = 3;
            this.panelTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseDown);
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelCaption.ForeColor = System.Drawing.Color.White;
            this.labelCaption.Location = new System.Drawing.Point(10, 7);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(85, 20);
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
            this.btnClose.Location = new System.Drawing.Point(643, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(47, 38);
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
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(12, 9, 0, 0);
            this.panelBody.Size = new System.Drawing.Size(690, 310);
            this.panelBody.TabIndex = 5;
            // 
            // panel_Select
            // 
            this.panel_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Select.Controls.Add(this.cb_manufacturing);
            this.panel_Select.Controls.Add(this.cb_date);
            this.panel_Select.Controls.Add(this.tb_manufacturing);
            this.panel_Select.Controls.Add(this.label2);
            this.panel_Select.Controls.Add(this.dt_end);
            this.panel_Select.Controls.Add(this.combo_date);
            this.panel_Select.Controls.Add(this.dt_start);
            this.panel_Select.Controls.Add(this.label1);
            this.panel_Select.Location = new System.Drawing.Point(10, 47);
            this.panel_Select.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_Select.Name = "panel_Select";
            this.panel_Select.Size = new System.Drawing.Size(668, 212);
            this.panel_Select.TabIndex = 65;
            // 
            // dt_end
            // 
            this.dt_end.BorderColor = System.Drawing.Color.Black;
            this.dt_end.BorderSize = 1;
            this.dt_end.Font = new System.Drawing.Font("굴림", 9.5F);
            this.dt_end.Location = new System.Drawing.Point(446, 14);
            this.dt_end.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dt_end.MinimumSize = new System.Drawing.Size(4, 35);
            this.dt_end.Name = "dt_end";
            this.dt_end.Size = new System.Drawing.Size(206, 35);
            this.dt_end.SkinColor = System.Drawing.Color.White;
            this.dt_end.TabIndex = 2;
            this.dt_end.TextColor = System.Drawing.Color.Black;
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
            this.combo_date.Location = new System.Drawing.Point(114, 14);
            this.combo_date.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.combo_date.Name = "combo_date";
            this.combo_date.Padding = new System.Windows.Forms.Padding(1);
            this.combo_date.Size = new System.Drawing.Size(97, 35);
            this.combo_date.TabIndex = 0;
            this.combo_date.Texts = "오늘";
            this.combo_date.OnSelectedIndexChanged += new System.EventHandler(this.combo_date_OnSelectedIndexChanged);
            // 
            // dt_start
            // 
            this.dt_start.BorderColor = System.Drawing.Color.Black;
            this.dt_start.BorderSize = 1;
            this.dt_start.Font = new System.Drawing.Font("굴림", 9.5F);
            this.dt_start.Location = new System.Drawing.Point(235, 14);
            this.dt_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dt_start.MinimumSize = new System.Drawing.Size(4, 35);
            this.dt_start.Name = "dt_start";
            this.dt_start.Size = new System.Drawing.Size(206, 35);
            this.dt_start.SkinColor = System.Drawing.Color.White;
            this.dt_start.TabIndex = 1;
            this.dt_start.TextColor = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "기간";
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
            this.btn_Cancel.Location = new System.Drawing.Point(610, 268);
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
            this.btn_Create.Location = new System.Drawing.Point(542, 268);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(62, 31);
            this.btn_Create.TabIndex = 63;
            this.btn_Create.Text = "조회";
            this.btn_Create.TextColor = System.Drawing.Color.DodgerBlue;
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "공정명";
            // 
            // tb_manufacturing
            // 
            this.tb_manufacturing.BackColor = System.Drawing.SystemColors.Window;
            this.tb_manufacturing.BorderColor = System.Drawing.Color.Black;
            this.tb_manufacturing.BorderFocusColor = System.Drawing.Color.HotPink;
            this.tb_manufacturing.BorderRadius = 0;
            this.tb_manufacturing.BorderSize = 1;
            this.tb_manufacturing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_manufacturing.ForeColor = System.Drawing.Color.DimGray;
            this.tb_manufacturing.Location = new System.Drawing.Point(114, 66);
            this.tb_manufacturing.Margin = new System.Windows.Forms.Padding(4);
            this.tb_manufacturing.Multiline = false;
            this.tb_manufacturing.Name = "tb_manufacturing";
            this.tb_manufacturing.Padding = new System.Windows.Forms.Padding(7);
            this.tb_manufacturing.PasswordChar = false;
            this.tb_manufacturing.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.tb_manufacturing.PlaceholderText = "";
            this.tb_manufacturing.ReadOnly = false;
            this.tb_manufacturing.Size = new System.Drawing.Size(538, 31);
            this.tb_manufacturing.TabIndex = 4;
            this.tb_manufacturing.Texts = "";
            this.tb_manufacturing.UnderlinedStyle = true;
            // 
            // cb_date
            // 
            this.cb_date.AutoSize = true;
            this.cb_date.Location = new System.Drawing.Point(14, 26);
            this.cb_date.Name = "cb_date";
            this.cb_date.Size = new System.Drawing.Size(15, 14);
            this.cb_date.TabIndex = 5;
            this.cb_date.UseVisualStyleBackColor = true;
            // 
            // cb_manufacturing
            // 
            this.cb_manufacturing.AutoSize = true;
            this.cb_manufacturing.Location = new System.Drawing.Point(14, 73);
            this.cb_manufacturing.Name = "cb_manufacturing";
            this.cb_manufacturing.Size = new System.Drawing.Size(15, 14);
            this.cb_manufacturing.TabIndex = 6;
            this.cb_manufacturing.UseVisualStyleBackColor = true;
            // 
            // frmDetailSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 310);
            this.Controls.Add(this.panelTitleBar);
            this.Controls.Add(this.panelBody);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmDetailSearch";
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
        private CustomControls.RJControls.RJDatePicker dt_start;
        private CustomControls.RJControls.RJComboBox combo_date;
        private CustomControls.RJControls.RJDatePicker dt_end;
        private System.Windows.Forms.Label label2;
        private CustomControls.RJControls.RJTextBox tb_manufacturing;
        private System.Windows.Forms.CheckBox cb_manufacturing;
        private System.Windows.Forms.CheckBox cb_date;
    }
}