namespace TcPCM_Connect
{
    partial class frmMachine
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
            this.roundBorderPanel2 = new TcPCM_Connect.RoundBorderPanel();
            this.btn_Shift = new CustomControls.RJControls.RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_Classification = new CustomControls.RJControls.RJComboBox();
            this.btn_Configuration = new CustomControls.RJControls.RJButton();
            this.btn_Save = new CustomControls.RJControls.RJButton();
            this.dgv_Machine = new System.Windows.Forms.DataGridView();
            this.btn_Create = new CustomControls.RJControls.RJButton();
            this.roundBorderPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Machine)).BeginInit();
            this.SuspendLayout();
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
            this.roundBorderPanel2.Controls.Add(this.btn_Shift);
            this.roundBorderPanel2.Controls.Add(this.label2);
            this.roundBorderPanel2.Controls.Add(this.cb_Classification);
            this.roundBorderPanel2.Controls.Add(this.btn_Configuration);
            this.roundBorderPanel2.Controls.Add(this.btn_Save);
            this.roundBorderPanel2.Controls.Add(this.dgv_Machine);
            this.roundBorderPanel2.Controls.Add(this.btn_Create);
            this.roundBorderPanel2.IsFill = true;
            this.roundBorderPanel2.Location = new System.Drawing.Point(5, 4);
            this.roundBorderPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.roundBorderPanel2.Name = "roundBorderPanel2";
            this.roundBorderPanel2.Size = new System.Drawing.Size(1050, 615);
            this.roundBorderPanel2.TabIndex = 62;
            // 
            // btn_Shift
            // 
            this.btn_Shift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Shift.BackColor = System.Drawing.Color.White;
            this.btn_Shift.BackgroundColor = System.Drawing.Color.White;
            this.btn_Shift.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Shift.BorderRadius = 5;
            this.btn_Shift.BorderSize = 1;
            this.btn_Shift.FlatAppearance.BorderSize = 0;
            this.btn_Shift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Shift.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Shift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Shift.Location = new System.Drawing.Point(734, 20);
            this.btn_Shift.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Shift.Name = "btn_Shift";
            this.btn_Shift.Size = new System.Drawing.Size(105, 39);
            this.btn_Shift.TabIndex = 67;
            this.btn_Shift.Text = "Shift 설정 변경";
            this.btn_Shift.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Shift.UseVisualStyleBackColor = false;
            this.btn_Shift.Click += new System.EventHandler(this.btn_Shift_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(33, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 18);
            this.label2.TabIndex = 66;
            this.label2.Text = "설비 분류 : ";
            // 
            // cb_Classification
            // 
            this.cb_Classification.BackColor = System.Drawing.Color.White;
            this.cb_Classification.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.cb_Classification.BorderSize = 1;
            this.cb_Classification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cb_Classification.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_Classification.ForeColor = System.Drawing.Color.DimGray;
            this.cb_Classification.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.cb_Classification.Items.AddRange(new object[] {
            "설비"});
            this.cb_Classification.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.cb_Classification.ListTextColor = System.Drawing.Color.DimGray;
            this.cb_Classification.Location = new System.Drawing.Point(118, 22);
            this.cb_Classification.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cb_Classification.Name = "cb_Classification";
            this.cb_Classification.Padding = new System.Windows.Forms.Padding(1);
            this.cb_Classification.Size = new System.Drawing.Size(136, 38);
            this.cb_Classification.TabIndex = 65;
            this.cb_Classification.Texts = "설비";
            this.cb_Classification.OnSelectedIndexChanged += new System.EventHandler(this.cb_Classification_OnSelectedIndexChanged);
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
            this.btn_Configuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Configuration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Configuration.Location = new System.Drawing.Point(845, 20);
            this.btn_Configuration.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Configuration.Name = "btn_Configuration";
            this.btn_Configuration.Size = new System.Drawing.Size(168, 39);
            this.btn_Configuration.TabIndex = 62;
            this.btn_Configuration.Text = "Configuration 설정 변경";
            this.btn_Configuration.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Configuration.UseVisualStyleBackColor = false;
            this.btn_Configuration.Click += new System.EventHandler(this.btn_Configuration_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.AllowDrop = true;
            this.btn_Save.BackColor = System.Drawing.Color.White;
            this.btn_Save.BackgroundColor = System.Drawing.Color.White;
            this.btn_Save.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btn_Save.BorderRadius = 5;
            this.btn_Save.BorderSize = 1;
            this.btn_Save.FlatAppearance.BorderSize = 0;
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Save.ForeColor = System.Drawing.Color.DodgerBlue;
            this.btn_Save.Location = new System.Drawing.Point(553, 20);
            this.btn_Save.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(55, 39);
            this.btn_Save.TabIndex = 59;
            this.btn_Save.Text = "저장";
            this.btn_Save.TextColor = System.Drawing.Color.DodgerBlue;
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // dgv_Machine
            // 
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.DimGray;
            this.dgv_Machine.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_Machine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Machine.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_Machine.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Machine.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_Machine.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Machine.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_Machine.ColumnHeadersHeight = 40;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Machine.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_Machine.EnableHeadersVisualStyles = false;
            this.dgv_Machine.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.dgv_Machine.Location = new System.Drawing.Point(26, 78);
            this.dgv_Machine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgv_Machine.Name = "dgv_Machine";
            this.dgv_Machine.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.PaleVioletRed;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Machine.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_Machine.RowHeadersVisible = false;
            this.dgv_Machine.RowHeadersWidth = 32;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_Machine.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgv_Machine.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgv_Machine.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dgv_Machine.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgv_Machine.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv_Machine.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_Machine.RowTemplate.Height = 32;
            this.dgv_Machine.Size = new System.Drawing.Size(995, 500);
            this.dgv_Machine.TabIndex = 61;
            this.dgv_Machine.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_Machine_CellFormatting);
            this.dgv_Machine.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Machine_CellValueChanged);
            this.dgv_Machine.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_Machine_DataError);
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
            this.btn_Create.Location = new System.Drawing.Point(614, 20);
            this.btn_Create.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(114, 39);
            this.btn_Create.TabIndex = 58;
            this.btn_Create.Text = "엑셀 Import";
            this.btn_Create.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // frmMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 624);
            this.Controls.Add(this.roundBorderPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmMachine";
            this.Text = "frmExchange";
            this.Load += new System.EventHandler(this.frmExchange_Load);
            this.roundBorderPanel2.ResumeLayout(false);
            this.roundBorderPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Machine)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgv_Machine;
        private RoundBorderPanel roundBorderPanel2;
        private CustomControls.RJControls.RJButton btn_Create;
        private CustomControls.RJControls.RJButton btn_Save;
        private CustomControls.RJControls.RJButton btn_Configuration;
        private System.Windows.Forms.Label label2;
        private CustomControls.RJControls.RJComboBox cb_Classification;
        private CustomControls.RJControls.RJButton btn_Shift;
    }
}