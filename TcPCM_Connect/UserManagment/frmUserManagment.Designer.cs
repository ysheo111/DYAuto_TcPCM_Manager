
namespace TcPCM_Connect
{
    partial class frmUserManagment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_UserInfo = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Edit = new CustomControls.RJControls.RJButton();
            this.btn_SignUp = new CustomControls.RJControls.RJButton();
            this.btn_Delete = new CustomControls.RJControls.RJButton();
            this.txt_ID = new CustomControls.RJControls.RJTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Password = new CustomControls.RJControls.RJTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Change = new CustomControls.RJControls.RJButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_UserInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_UserInfo
            // 
            this.dgv_UserInfo.AllowUserToAddRows = false;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.DimGray;
            this.dgv_UserInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_UserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_UserInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_UserInfo.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_UserInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_UserInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_UserInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_UserInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_UserInfo.ColumnHeadersHeight = 30;
            this.dgv_UserInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_UserInfo.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_UserInfo.EnableHeadersVisualStyles = false;
            this.dgv_UserInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.dgv_UserInfo.Location = new System.Drawing.Point(50, 104);
            this.dgv_UserInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgv_UserInfo.Name = "dgv_UserInfo";
            this.dgv_UserInfo.ReadOnly = true;
            this.dgv_UserInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.PaleVioletRed;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_UserInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_UserInfo.RowHeadersVisible = false;
            this.dgv_UserInfo.RowHeadersWidth = 51;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_UserInfo.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgv_UserInfo.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgv_UserInfo.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dgv_UserInfo.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgv_UserInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv_UserInfo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_UserInfo.RowTemplate.Height = 35;
            this.dgv_UserInfo.Size = new System.Drawing.Size(766, 409);
            this.dgv_UserInfo.TabIndex = 7;
            this.dgv_UserInfo.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_UserInfo_CellDoubleClick);
            this.dgv_UserInfo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_UserInfo_DataBindingComplete);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 49;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "NOMBRES";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 106;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "APELLIDOS";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 113;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "DIRECCION";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 113;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "TELEFONO";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 110;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 43;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "NOMBRES";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 93;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "APELLIDOS";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 97;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "DIRECCION";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 97;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "TELEFONO";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 95;
            // 
            // btn_Edit
            // 
            this.btn_Edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Edit.BackColor = System.Drawing.Color.White;
            this.btn_Edit.BackgroundColor = System.Drawing.Color.White;
            this.btn_Edit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Edit.BorderRadius = 5;
            this.btn_Edit.BorderSize = 1;
            this.btn_Edit.FlatAppearance.BorderSize = 0;
            this.btn_Edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Edit.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Edit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Edit.Location = new System.Drawing.Point(865, 150);
            this.btn_Edit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(105, 39);
            this.btn_Edit.TabIndex = 59;
            this.btn_Edit.Text = "정보 수정";
            this.btn_Edit.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Edit.UseVisualStyleBackColor = false;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_SignUp
            // 
            this.btn_SignUp.AllowDrop = true;
            this.btn_SignUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SignUp.BackColor = System.Drawing.Color.White;
            this.btn_SignUp.BackgroundColor = System.Drawing.Color.White;
            this.btn_SignUp.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btn_SignUp.BorderRadius = 5;
            this.btn_SignUp.BorderSize = 1;
            this.btn_SignUp.FlatAppearance.BorderSize = 0;
            this.btn_SignUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SignUp.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_SignUp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.btn_SignUp.Location = new System.Drawing.Point(865, 104);
            this.btn_SignUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_SignUp.Name = "btn_SignUp";
            this.btn_SignUp.Size = new System.Drawing.Size(105, 39);
            this.btn_SignUp.TabIndex = 60;
            this.btn_SignUp.Text = "신규 등록";
            this.btn_SignUp.TextColor = System.Drawing.Color.DodgerBlue;
            this.btn_SignUp.UseVisualStyleBackColor = false;
            this.btn_SignUp.Click += new System.EventHandler(this.btn_SignUp_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Delete.BackColor = System.Drawing.Color.White;
            this.btn_Delete.BackgroundColor = System.Drawing.Color.White;
            this.btn_Delete.BorderColor = System.Drawing.Color.Crimson;
            this.btn_Delete.BorderRadius = 5;
            this.btn_Delete.BorderSize = 1;
            this.btn_Delete.FlatAppearance.BorderSize = 0;
            this.btn_Delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Delete.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Delete.ForeColor = System.Drawing.Color.Crimson;
            this.btn_Delete.Location = new System.Drawing.Point(865, 196);
            this.btn_Delete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(105, 39);
            this.btn_Delete.TabIndex = 61;
            this.btn_Delete.Text = "삭제";
            this.btn_Delete.TextColor = System.Drawing.Color.Crimson;
            this.btn_Delete.UseVisualStyleBackColor = false;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // txt_ID
            // 
            this.txt_ID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_ID.BorderColor = System.Drawing.Color.Gray;
            this.txt_ID.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.txt_ID.BorderRadius = 0;
            this.txt_ID.BorderSize = 2;
            this.txt_ID.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_ID.ForeColor = System.Drawing.Color.DimGray;
            this.txt_ID.Location = new System.Drawing.Point(217, 31);
            this.txt_ID.Margin = new System.Windows.Forms.Padding(5);
            this.txt_ID.Multiline = false;
            this.txt_ID.Name = "txt_ID";
            this.txt_ID.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_ID.PasswordChar = false;
            this.txt_ID.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_ID.PlaceholderText = "";
            this.txt_ID.ReadOnly = false;
            this.txt_ID.Size = new System.Drawing.Size(166, 35);
            this.txt_ID.TabIndex = 62;
            this.txt_ID.Texts = "";
            this.txt_ID.UnderlinedStyle = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(184, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 16);
            this.label2.TabIndex = 63;
            this.label2.Text = "ID : ";
            // 
            // txt_Password
            // 
            this.txt_Password.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_Password.BorderColor = System.Drawing.Color.Gray;
            this.txt_Password.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.txt_Password.BorderRadius = 0;
            this.txt_Password.BorderSize = 2;
            this.txt_Password.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_Password.ForeColor = System.Drawing.Color.DimGray;
            this.txt_Password.Location = new System.Drawing.Point(499, 31);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(5);
            this.txt_Password.Multiline = false;
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_Password.PasswordChar = false;
            this.txt_Password.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_Password.PlaceholderText = "";
            this.txt_Password.ReadOnly = false;
            this.txt_Password.Size = new System.Drawing.Size(234, 35);
            this.txt_Password.TabIndex = 64;
            this.txt_Password.Texts = "";
            this.txt_Password.UnderlinedStyle = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(411, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 65;
            this.label1.Text = "Password :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(47, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 16);
            this.label3.TabIndex = 66;
            this.label3.Text = "TcPCM 계정 관리";
            // 
            // btn_Change
            // 
            this.btn_Change.AllowDrop = true;
            this.btn_Change.BackColor = System.Drawing.Color.White;
            this.btn_Change.BackgroundColor = System.Drawing.Color.White;
            this.btn_Change.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btn_Change.BorderRadius = 5;
            this.btn_Change.BorderSize = 1;
            this.btn_Change.FlatAppearance.BorderSize = 0;
            this.btn_Change.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Change.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Change.ForeColor = System.Drawing.Color.DodgerBlue;
            this.btn_Change.Location = new System.Drawing.Point(758, 27);
            this.btn_Change.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Change.Name = "btn_Change";
            this.btn_Change.Size = new System.Drawing.Size(58, 39);
            this.btn_Change.TabIndex = 67;
            this.btn_Change.Text = "변경";
            this.btn_Change.TextColor = System.Drawing.Color.DodgerBlue;
            this.btn_Change.UseVisualStyleBackColor = false;
            this.btn_Change.Click += new System.EventHandler(this.btn_Change_Click);
            // 
            // frmUserManagment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1008, 562);
            this.Controls.Add(this.btn_Change);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_ID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Edit);
            this.Controls.Add(this.btn_SignUp);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.dgv_UserInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmUserManagment";
            this.Text = "frmUserManagment";
            this.Load += new System.EventHandler(this.frmUserManagment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_UserInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgv_UserInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private CustomControls.RJControls.RJButton btn_Edit;
        private CustomControls.RJControls.RJButton btn_SignUp;
        private CustomControls.RJControls.RJButton btn_Delete;
        private CustomControls.RJControls.RJTextBox txt_ID;
        private System.Windows.Forms.Label label2;
        private CustomControls.RJControls.RJTextBox txt_Password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private CustomControls.RJControls.RJButton btn_Change;
    }
}