namespace TcPCM_Connect
{
    partial class frmExchange
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_ExchangeRate = new System.Windows.Forms.DataGridView();
            this.roundBorderPanel2 = new TcPCM_Connect.RoundBorderPanel();
            this.btn_ExcelCreate = new CustomControls.RJControls.RJButton();
            this.searchButton1 = new TcPCM_Connect.Controller.SearchButton();
            this.btn_Configuration = new CustomControls.RJControls.RJButton();
            this.btn_Save = new CustomControls.RJControls.RJButton();
            this.btn_Create = new CustomControls.RJControls.RJButton();
            this.Valid_From = new TcPCM_Connect.CalendarColumn();
            this.구분자 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.이름 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.통화 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.환율 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ExchangeRate)).BeginInit();
            this.roundBorderPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_ExchangeRate
            // 
            this.dgv_ExchangeRate.AllowUserToAddRows = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.DimGray;
            this.dgv_ExchangeRate.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_ExchangeRate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_ExchangeRate.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_ExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_ExchangeRate.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_ExchangeRate.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_ExchangeRate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_ExchangeRate.ColumnHeadersHeight = 40;
            this.dgv_ExchangeRate.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Valid_From,
            this.구분자,
            this.이름,
            this.통화,
            this.환율});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_ExchangeRate.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_ExchangeRate.EnableHeadersVisualStyles = false;
            this.dgv_ExchangeRate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.dgv_ExchangeRate.Location = new System.Drawing.Point(26, 78);
            this.dgv_ExchangeRate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgv_ExchangeRate.Name = "dgv_ExchangeRate";
            this.dgv_ExchangeRate.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.PaleVioletRed;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_ExchangeRate.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_ExchangeRate.RowHeadersVisible = false;
            this.dgv_ExchangeRate.RowHeadersWidth = 32;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_ExchangeRate.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_ExchangeRate.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgv_ExchangeRate.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dgv_ExchangeRate.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgv_ExchangeRate.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgv_ExchangeRate.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_ExchangeRate.RowTemplate.Height = 32;
            this.dgv_ExchangeRate.Size = new System.Drawing.Size(995, 500);
            this.dgv_ExchangeRate.TabIndex = 61;
            this.dgv_ExchangeRate.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_ExchangeRate_CellValueChanged);
            this.dgv_ExchangeRate.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_ExchangeRate_ColumnHeaderMouseClick);
            this.dgv_ExchangeRate.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_ExchangeRate_DataError);
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
            this.roundBorderPanel2.Controls.Add(this.btn_Configuration);
            this.roundBorderPanel2.Controls.Add(this.btn_Save);
            this.roundBorderPanel2.Controls.Add(this.dgv_ExchangeRate);
            this.roundBorderPanel2.Controls.Add(this.btn_Create);
            this.roundBorderPanel2.IsFill = true;
            this.roundBorderPanel2.Location = new System.Drawing.Point(5, 4);
            this.roundBorderPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.roundBorderPanel2.Name = "roundBorderPanel2";
            this.roundBorderPanel2.Size = new System.Drawing.Size(1050, 615);
            this.roundBorderPanel2.TabIndex = 62;
            // 
            // btn_ExcelCreate
            // 
            this.btn_ExcelCreate.AllowDrop = true;
            this.btn_ExcelCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ExcelCreate.BackColor = System.Drawing.Color.White;
            this.btn_ExcelCreate.BackgroundColor = System.Drawing.Color.White;
            this.btn_ExcelCreate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_ExcelCreate.BorderRadius = 5;
            this.btn_ExcelCreate.BorderSize = 1;
            this.btn_ExcelCreate.FlatAppearance.BorderSize = 0;
            this.btn_ExcelCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ExcelCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ExcelCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_ExcelCreate.Location = new System.Drawing.Point(713, 20);
            this.btn_ExcelCreate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_ExcelCreate.Name = "btn_ExcelCreate";
            this.btn_ExcelCreate.Size = new System.Drawing.Size(114, 39);
            this.btn_ExcelCreate.TabIndex = 68;
            this.btn_ExcelCreate.Text = "엑셀 Export";
            this.btn_ExcelCreate.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_ExcelCreate.UseVisualStyleBackColor = false;
            this.btn_ExcelCreate.Click += new System.EventHandler(this.btn_ExcelCreate_Click);
            // 
            // searchButton1
            // 
            this.searchButton1.BackColor = System.Drawing.Color.Transparent;
            this.searchButton1.DetailSearchButtonBackColor = System.Drawing.Color.White;
            this.searchButton1.Location = new System.Drawing.Point(26, 20);
            this.searchButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.searchButton1.Name = "searchButton1";
            this.searchButton1.PanelBackColor = System.Drawing.Color.Transparent;
            this.searchButton1.Size = new System.Drawing.Size(420, 38);
            this.searchButton1.TabIndex = 67;
            this.searchButton1.text = "";
            this.searchButton1.TextBoxBackColor = System.Drawing.Color.WhiteSmoke;
            this.searchButton1.SearchButtonClick += new System.EventHandler(this.searchButton1_SearchButtonClick);
            this.searchButton1.DetailSearchButtonClick += new System.EventHandler(this.searchButton1_DetailSearchButtonClick);
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
            this.btn_Configuration.Location = new System.Drawing.Point(833, 20);
            this.btn_Configuration.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Configuration.Name = "btn_Configuration";
            this.btn_Configuration.Size = new System.Drawing.Size(180, 39);
            this.btn_Configuration.TabIndex = 62;
            this.btn_Configuration.Text = "Configuration 설정 변경";
            this.btn_Configuration.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Configuration.UseVisualStyleBackColor = false;
            this.btn_Configuration.Click += new System.EventHandler(this.btn_Configuration_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.AllowDrop = true;
            this.btn_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Save.BackColor = System.Drawing.Color.White;
            this.btn_Save.BackgroundColor = System.Drawing.Color.White;
            this.btn_Save.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btn_Save.BorderRadius = 5;
            this.btn_Save.BorderSize = 1;
            this.btn_Save.FlatAppearance.BorderSize = 0;
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Save.ForeColor = System.Drawing.Color.DodgerBlue;
            this.btn_Save.Location = new System.Drawing.Point(532, 19);
            this.btn_Save.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(55, 39);
            this.btn_Save.TabIndex = 59;
            this.btn_Save.Text = "저장";
            this.btn_Save.TextColor = System.Drawing.Color.DodgerBlue;
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Create
            // 
            this.btn_Create.AllowDrop = true;
            this.btn_Create.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Create.BackColor = System.Drawing.Color.White;
            this.btn_Create.BackgroundColor = System.Drawing.Color.White;
            this.btn_Create.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Create.BorderRadius = 5;
            this.btn_Create.BorderSize = 1;
            this.btn_Create.FlatAppearance.BorderSize = 0;
            this.btn_Create.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Create.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Create.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Create.Location = new System.Drawing.Point(593, 20);
            this.btn_Create.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(114, 39);
            this.btn_Create.TabIndex = 58;
            this.btn_Create.Text = "엑셀 Import";
            this.btn_Create.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(116)))), ((int)(((byte)(71)))));
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // Valid_From
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Valid_From.DefaultCellStyle = dataGridViewCellStyle3;
            this.Valid_From.HeaderText = "Valid From";
            this.Valid_From.MinimumWidth = 6;
            this.Valid_From.Name = "Valid_From";
            this.Valid_From.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Valid_From.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Valid_From.ToolTipText = "유효 날짜에 입력한 다음날 부터 해당 데이터가 적용됩니다.";
            this.Valid_From.Width = 200;
            // 
            // 구분자
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.구분자.DefaultCellStyle = dataGridViewCellStyle4;
            this.구분자.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.구분자.HeaderText = "구분자";
            this.구분자.MinimumWidth = 6;
            this.구분자.Name = "구분자";
            this.구분자.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.구분자.Width = 120;
            // 
            // 이름
            // 
            this.이름.HeaderText = "이름";
            this.이름.MinimumWidth = 6;
            this.이름.Name = "이름";
            this.이름.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.이름.Width = 260;
            // 
            // 통화
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.통화.DefaultCellStyle = dataGridViewCellStyle5;
            this.통화.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.통화.HeaderText = "통화";
            this.통화.MinimumWidth = 6;
            this.통화.Name = "통화";
            this.통화.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.통화.Width = 200;
            // 
            // 환율
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.환율.DefaultCellStyle = dataGridViewCellStyle6;
            this.환율.HeaderText = "환율";
            this.환율.MinimumWidth = 6;
            this.환율.Name = "환율";
            this.환율.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.환율.Width = 200;
            // 
            // frmExchange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 624);
            this.Controls.Add(this.roundBorderPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmExchange";
            this.Text = "frmExchange";
            this.Load += new System.EventHandler(this.frmExchange_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ExchangeRate)).EndInit();
            this.roundBorderPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgv_ExchangeRate;
        private RoundBorderPanel roundBorderPanel2;
        private CustomControls.RJControls.RJButton btn_Create;
        private CustomControls.RJControls.RJButton btn_Save;
        private CustomControls.RJControls.RJButton btn_Configuration;
        private CustomControls.RJControls.RJButton btn_ExcelCreate;
        private Controller.SearchButton searchButton1;
        private CalendarColumn Valid_From;
        private System.Windows.Forms.DataGridViewComboBoxColumn 구분자;
        private System.Windows.Forms.DataGridViewTextBoxColumn 이름;
        private System.Windows.Forms.DataGridViewComboBoxColumn 통화;
        private System.Windows.Forms.DataGridViewTextBoxColumn 환율;
    }
}