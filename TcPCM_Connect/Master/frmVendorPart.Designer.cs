﻿namespace TcPCM_Connect
{
    partial class frmVendorPart
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.삭제하기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundBorderPanel2 = new TcPCM_Connect.RoundBorderPanel();
            this.btn_ExcelCreate = new CustomControls.RJControls.RJButton();
            this.searchButton1 = new TcPCM_Connect.Controller.SearchButton();
            this.dgv_Vendor = new System.Windows.Forms.DataGridView();
            this.btn_Create = new CustomControls.RJControls.RJButton();
            this.contextMenuStrip1.SuspendLayout();
            this.roundBorderPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Vendor)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.삭제하기ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 28);
            // 
            // 삭제하기ToolStripMenuItem
            // 
            this.삭제하기ToolStripMenuItem.Name = "삭제하기ToolStripMenuItem";
            this.삭제하기ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.삭제하기ToolStripMenuItem.Text = "삭제하기";
            this.삭제하기ToolStripMenuItem.Click += new System.EventHandler(this.삭제하기ToolStripMenuItem_Click);
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
            this.roundBorderPanel2.Controls.Add(this.btn_ExcelCreate);
            this.roundBorderPanel2.Controls.Add(this.searchButton1);
            this.roundBorderPanel2.Controls.Add(this.dgv_Vendor);
            this.roundBorderPanel2.Controls.Add(this.btn_Create);
            this.roundBorderPanel2.IsFill = true;
            this.roundBorderPanel2.Location = new System.Drawing.Point(5, 4);
            this.roundBorderPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.roundBorderPanel2.Name = "roundBorderPanel2";
            this.roundBorderPanel2.Size = new System.Drawing.Size(1061, 615);
            this.roundBorderPanel2.TabIndex = 62;
            // 
            // btn_ExcelCreate
            // 
            this.btn_ExcelCreate.AllowDrop = true;
            this.btn_ExcelCreate.BackColor = System.Drawing.Color.White;
            this.btn_ExcelCreate.BackgroundColor = System.Drawing.Color.White;
            this.btn_ExcelCreate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_ExcelCreate.BorderRadius = 5;
            this.btn_ExcelCreate.BorderSize = 1;
            this.btn_ExcelCreate.FlatAppearance.BorderSize = 0;
            this.btn_ExcelCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ExcelCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ExcelCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_ExcelCreate.Location = new System.Drawing.Point(830, 21);
            this.btn_ExcelCreate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_ExcelCreate.Name = "btn_ExcelCreate";
            this.btn_ExcelCreate.Size = new System.Drawing.Size(202, 39);
            this.btn_ExcelCreate.TabIndex = 66;
            this.btn_ExcelCreate.Text = "부품원가계산서 List 다운로드";
            this.btn_ExcelCreate.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_ExcelCreate.UseVisualStyleBackColor = false;
            this.btn_ExcelCreate.Click += new System.EventHandler(this.btn_ExcelCreate_Click);
            // 
            // searchButton1
            // 
            this.searchButton1.BackColor = System.Drawing.Color.Transparent;
            this.searchButton1.DetailSearchButtonBackColor = System.Drawing.Color.White;
            this.searchButton1.Location = new System.Drawing.Point(26, 22);
            this.searchButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.searchButton1.Name = "searchButton1";
            this.searchButton1.PanelBackColor = System.Drawing.Color.Transparent;
            this.searchButton1.Size = new System.Drawing.Size(458, 38);
            this.searchButton1.TabIndex = 65;
            this.searchButton1.text = "";
            this.searchButton1.TextBoxBackColor = System.Drawing.Color.WhiteSmoke;
            this.searchButton1.SearchButtonClick += new System.EventHandler(this.searchButton1_SearchButtonClick_1);
            this.searchButton1.DetailSearchButtonClick += new System.EventHandler(this.searchButton1_DetailSearchButtonClick_1);
            // 
            // dgv_Vendor
            // 
            this.dgv_Vendor.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.DimGray;
            this.dgv_Vendor.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_Vendor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Vendor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Vendor.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_Vendor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Vendor.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_Vendor.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Vendor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_Vendor.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Vendor.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_Vendor.EnableHeadersVisualStyles = false;
            this.dgv_Vendor.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.dgv_Vendor.Location = new System.Drawing.Point(26, 78);
            this.dgv_Vendor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgv_Vendor.Name = "dgv_Vendor";
            this.dgv_Vendor.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.PaleVioletRed;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Vendor.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_Vendor.RowHeadersVisible = false;
            this.dgv_Vendor.RowHeadersWidth = 32;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_Vendor.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_Vendor.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_Vendor.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgv_Vendor.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv_Vendor.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgv_Vendor.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv_Vendor.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_Vendor.RowTemplate.Height = 32;
            this.dgv_Vendor.Size = new System.Drawing.Size(1006, 500);
            this.dgv_Vendor.TabIndex = 61;
            this.dgv_Vendor.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Vendor_CellDoubleClick);
            this.dgv_Vendor.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_Category_CellFormatting);
            this.dgv_Vendor.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Category_CellValueChanged);
            this.dgv_Vendor.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_Category_DataError);
            this.dgv_Vendor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgv_Vendor_MouseDown);
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
            this.btn_Create.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Create.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Create.Location = new System.Drawing.Point(654, 21);
            this.btn_Create.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(169, 39);
            this.btn_Create.TabIndex = 58;
            this.btn_Create.Text = "부품원가계산서 업로드";
            this.btn_Create.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // frmVendorPart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 622);
            this.Controls.Add(this.roundBorderPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmVendorPart";
            this.Text = "frmExchange";
            this.Load += new System.EventHandler(this.frmExchange_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.roundBorderPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Vendor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgv_Vendor;
        private RoundBorderPanel roundBorderPanel2;
        private CustomControls.RJControls.RJButton btn_Create;
        private Controller.SearchButton searchButton1;
        private CustomControls.RJControls.RJButton btn_ExcelCreate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 삭제하기ToolStripMenuItem;
    }
}