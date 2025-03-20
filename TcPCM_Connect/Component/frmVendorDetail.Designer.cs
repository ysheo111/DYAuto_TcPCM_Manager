
namespace TcPCM_Connect.Component
{
    partial class frmVendorDetail
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.삭제하기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btn_Cancel = new CustomControls.RJControls.RJButton();
            this.btn_Create = new CustomControls.RJControls.RJButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_VendorManufacturing = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgv_VendorMaterial = new System.Windows.Forms.DataGridView();
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.labelCaption = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_VendorManufacturing)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_VendorMaterial)).BeginInit();
            this.panelTitleBar.SuspendLayout();
            this.panel3.SuspendLayout();
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelTitleBar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1149, 731);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btn_Create);
            this.panel4.Controls.Add(this.btn_Cancel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 685);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1143, 43);
            this.panel4.TabIndex = 0;
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
            this.btn_Cancel.Location = new System.Drawing.Point(1069, 1);
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
            this.btn_Create.Location = new System.Drawing.Point(992, 1);
            this.btn_Create.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(71, 39);
            this.btn_Create.TabIndex = 63;
            this.btn_Create.Text = "수정";
            this.btn_Create.TextColor = System.Drawing.Color.DodgerBlue;
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.dgv_VendorManufacturing);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 398);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1143, 281);
            this.panel2.TabIndex = 67;
            // 
            // dgv_VendorManufacturing
            // 
            this.dgv_VendorManufacturing.AllowUserToAddRows = false;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.DimGray;
            this.dgv_VendorManufacturing.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgv_VendorManufacturing.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgv_VendorManufacturing.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_VendorManufacturing.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_VendorManufacturing.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_VendorManufacturing.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_VendorManufacturing.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgv_VendorManufacturing.ColumnHeadersHeight = 40;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_VendorManufacturing.DefaultCellStyle = dataGridViewCellStyle13;
            this.dgv_VendorManufacturing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_VendorManufacturing.EnableHeadersVisualStyles = false;
            this.dgv_VendorManufacturing.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.dgv_VendorManufacturing.Location = new System.Drawing.Point(0, 0);
            this.dgv_VendorManufacturing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgv_VendorManufacturing.Name = "dgv_VendorManufacturing";
            this.dgv_VendorManufacturing.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.PaleVioletRed;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_VendorManufacturing.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgv_VendorManufacturing.RowHeadersVisible = false;
            this.dgv_VendorManufacturing.RowHeadersWidth = 32;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_VendorManufacturing.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dgv_VendorManufacturing.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_VendorManufacturing.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgv_VendorManufacturing.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dgv_VendorManufacturing.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgv_VendorManufacturing.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv_VendorManufacturing.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_VendorManufacturing.RowTemplate.Height = 32;
            this.dgv_VendorManufacturing.Size = new System.Drawing.Size(1141, 279);
            this.dgv_VendorManufacturing.TabIndex = 62;
            this.dgv_VendorManufacturing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgv_VendorManufacturing_MouseDown);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dgv_VendorMaterial);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1143, 281);
            this.panel1.TabIndex = 66;
            // 
            // dgv_VendorMaterial
            // 
            this.dgv_VendorMaterial.AllowUserToAddRows = false;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.DimGray;
            this.dgv_VendorMaterial.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dgv_VendorMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_VendorMaterial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgv_VendorMaterial.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_VendorMaterial.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_VendorMaterial.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_VendorMaterial.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_VendorMaterial.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgv_VendorMaterial.ColumnHeadersHeight = 40;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_VendorMaterial.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgv_VendorMaterial.EnableHeadersVisualStyles = false;
            this.dgv_VendorMaterial.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.dgv_VendorMaterial.Location = new System.Drawing.Point(-1, 4);
            this.dgv_VendorMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgv_VendorMaterial.Name = "dgv_VendorMaterial";
            this.dgv_VendorMaterial.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.PaleVioletRed;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_VendorMaterial.RowHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgv_VendorMaterial.RowHeadersVisible = false;
            this.dgv_VendorMaterial.RowHeadersWidth = 32;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_VendorMaterial.RowsDefaultCellStyle = dataGridViewCellStyle20;
            this.dgv_VendorMaterial.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_VendorMaterial.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgv_VendorMaterial.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dgv_VendorMaterial.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgv_VendorMaterial.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv_VendorMaterial.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_VendorMaterial.RowTemplate.Height = 32;
            this.dgv_VendorMaterial.Size = new System.Drawing.Size(1142, 275);
            this.dgv_VendorMaterial.TabIndex = 62;
            this.dgv_VendorMaterial.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgv_VendorDetail_MouseDown);
            // 
            // panelTitleBar
            // 
            this.panelTitleBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTitleBar.BackColor = System.Drawing.Color.DimGray;
            this.panelTitleBar.Controls.Add(this.labelCaption);
            this.panelTitleBar.Location = new System.Drawing.Point(3, 4);
            this.panelTitleBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.Size = new System.Drawing.Size(1143, 46);
            this.panelTitleBar.TabIndex = 6;
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCaption.ForeColor = System.Drawing.Color.White;
            this.labelCaption.Location = new System.Drawing.Point(11, 14);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(131, 20);
            this.labelCaption.TabIndex = 4;
            this.labelCaption.Text = "Vendor_Material";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.DimGray;
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(3, 345);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1143, 46);
            this.panel3.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Vendor_Manufacturing";
            // 
            // frmVendorDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 731);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimizeBox = false;
            this.Name = "frmVendorDetail";
            this.ShowIcon = false;
            this.Text = "VendorDetail";
            this.Load += new System.EventHandler(this.ShiftSetting_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_VendorManufacturing)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_VendorMaterial)).EndInit();
            this.panelTitleBar.ResumeLayout(false);
            this.panelTitleBar.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 삭제하기ToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelTitleBar;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgv_VendorMaterial;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgv_VendorManufacturing;
        private System.Windows.Forms.Panel panel4;
        private CustomControls.RJControls.RJButton btn_Create;
        private CustomControls.RJControls.RJButton btn_Cancel;
    }
}