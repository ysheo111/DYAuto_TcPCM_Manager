
namespace TcPCM_Connect
{
    partial class frmLotCalc
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
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Calc = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_cavity = new CustomControls.RJControls.RJTextBox();
            this.txt_lot = new CustomControls.RJControls.RJTextBox();
            this.txt_setup = new CustomControls.RJControls.RJTextBox();
            this.txt_ct = new CustomControls.RJControls.RJTextBox();
            this.txt_shift = new CustomControls.RJControls.RJTextBox();
            this.txt_workday = new CustomControls.RJControls.RJTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_SpaceCost = new CustomControls.RJControls.RJTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_Imputed = new CustomControls.RJControls.RJTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_CalcMaintance = new System.Windows.Forms.Button();
            this.txt_Maintance = new CustomControls.RJControls.RJTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_MaintainRate = new CustomControls.RJControls.RJTextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(33, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(320, 16);
            this.label5.TabIndex = 37;
            this.label5.Text = "Production hour per shift (1Shift 당 작업 시간) ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(33, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 34;
            this.label2.Text = "Cycle Time";
            // 
            // btn_Calc
            // 
            this.btn_Calc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.btn_Calc.FlatAppearance.BorderSize = 0;
            this.btn_Calc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Calc.Font = new System.Drawing.Font("Microsoft PhagsPa", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Calc.ForeColor = System.Drawing.Color.White;
            this.btn_Calc.Location = new System.Drawing.Point(36, 454);
            this.btn_Calc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Calc.Name = "btn_Calc";
            this.btn_Calc.Size = new System.Drawing.Size(295, 40);
            this.btn_Calc.TabIndex = 33;
            this.btn_Calc.Text = "계산";
            this.btn_Calc.UseVisualStyleBackColor = false;
            this.btn_Calc.Click += new System.EventHandler(this.btn_Calc_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(33, 363);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 35;
            this.label1.Text = "Set-up time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(33, 515);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 16);
            this.label3.TabIndex = 36;
            this.label3.Text = "Usable parts per lot";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(33, 284);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 39;
            this.label4.Text = "Cavity";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(33, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(219, 16);
            this.label7.TabIndex = 37;
            this.label7.Text = "Shifts per production day (교대)";
            // 
            // txt_cavity
            // 
            this.txt_cavity.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_cavity.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_cavity.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_cavity.BorderRadius = 0;
            this.txt_cavity.BorderSize = 1;
            this.txt_cavity.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_cavity.ForeColor = System.Drawing.Color.DimGray;
            this.txt_cavity.Location = new System.Drawing.Point(36, 310);
            this.txt_cavity.Margin = new System.Windows.Forms.Padding(5);
            this.txt_cavity.Multiline = false;
            this.txt_cavity.Name = "txt_cavity";
            this.txt_cavity.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_cavity.PasswordChar = false;
            this.txt_cavity.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_cavity.PlaceholderText = "";
            this.txt_cavity.ReadOnly = false;
            this.txt_cavity.Size = new System.Drawing.Size(295, 35);
            this.txt_cavity.TabIndex = 3;
            this.txt_cavity.Texts = "";
            this.txt_cavity.UnderlinedStyle = true;
            this.txt_cavity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_workday_KeyPress);
            // 
            // txt_lot
            // 
            this.txt_lot.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_lot.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_lot.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_lot.BorderRadius = 0;
            this.txt_lot.BorderSize = 1;
            this.txt_lot.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_lot.ForeColor = System.Drawing.Color.DimGray;
            this.txt_lot.Location = new System.Drawing.Point(36, 541);
            this.txt_lot.Margin = new System.Windows.Forms.Padding(5);
            this.txt_lot.Multiline = false;
            this.txt_lot.Name = "txt_lot";
            this.txt_lot.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_lot.PasswordChar = false;
            this.txt_lot.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_lot.PlaceholderText = "";
            this.txt_lot.ReadOnly = false;
            this.txt_lot.Size = new System.Drawing.Size(295, 35);
            this.txt_lot.TabIndex = 5;
            this.txt_lot.Texts = "";
            this.txt_lot.UnderlinedStyle = true;
            this.txt_lot._TextChanged += new System.EventHandler(this.txt_lot__TextChanged);
            this.txt_lot.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_lot_KeyPress);
            // 
            // txt_setup
            // 
            this.txt_setup.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_setup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_setup.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_setup.BorderRadius = 0;
            this.txt_setup.BorderSize = 1;
            this.txt_setup.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_setup.ForeColor = System.Drawing.Color.DimGray;
            this.txt_setup.Location = new System.Drawing.Point(36, 390);
            this.txt_setup.Margin = new System.Windows.Forms.Padding(5);
            this.txt_setup.Multiline = false;
            this.txt_setup.Name = "txt_setup";
            this.txt_setup.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_setup.PasswordChar = false;
            this.txt_setup.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_setup.PlaceholderText = "";
            this.txt_setup.ReadOnly = false;
            this.txt_setup.Size = new System.Drawing.Size(295, 35);
            this.txt_setup.TabIndex = 4;
            this.txt_setup.Texts = "";
            this.txt_setup.UnderlinedStyle = true;
            this.txt_setup.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_workday_KeyPress);
            // 
            // txt_ct
            // 
            this.txt_ct.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_ct.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_ct.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_ct.BorderRadius = 0;
            this.txt_ct.BorderSize = 1;
            this.txt_ct.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_ct.ForeColor = System.Drawing.Color.DimGray;
            this.txt_ct.Location = new System.Drawing.Point(36, 232);
            this.txt_ct.Margin = new System.Windows.Forms.Padding(5);
            this.txt_ct.Multiline = false;
            this.txt_ct.Name = "txt_ct";
            this.txt_ct.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_ct.PasswordChar = false;
            this.txt_ct.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_ct.PlaceholderText = "";
            this.txt_ct.ReadOnly = false;
            this.txt_ct.Size = new System.Drawing.Size(295, 35);
            this.txt_ct.TabIndex = 2;
            this.txt_ct.Texts = "";
            this.txt_ct.UnderlinedStyle = true;
            this.txt_ct.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_workday_KeyPress);
            // 
            // txt_shift
            // 
            this.txt_shift.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_shift.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_shift.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_shift.BorderRadius = 0;
            this.txt_shift.BorderSize = 1;
            this.txt_shift.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_shift.ForeColor = System.Drawing.Color.DimGray;
            this.txt_shift.Location = new System.Drawing.Point(36, 159);
            this.txt_shift.Margin = new System.Windows.Forms.Padding(5);
            this.txt_shift.Multiline = false;
            this.txt_shift.Name = "txt_shift";
            this.txt_shift.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_shift.PasswordChar = false;
            this.txt_shift.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_shift.PlaceholderText = "";
            this.txt_shift.ReadOnly = false;
            this.txt_shift.Size = new System.Drawing.Size(295, 35);
            this.txt_shift.TabIndex = 1;
            this.txt_shift.Texts = "";
            this.txt_shift.UnderlinedStyle = true;
            this.txt_shift.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_workday_KeyPress);
            // 
            // txt_workday
            // 
            this.txt_workday.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_workday.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_workday.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_workday.BorderRadius = 0;
            this.txt_workday.BorderSize = 1;
            this.txt_workday.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_workday.ForeColor = System.Drawing.Color.DimGray;
            this.txt_workday.Location = new System.Drawing.Point(36, 88);
            this.txt_workday.Margin = new System.Windows.Forms.Padding(5);
            this.txt_workday.Multiline = false;
            this.txt_workday.Name = "txt_workday";
            this.txt_workday.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_workday.PasswordChar = false;
            this.txt_workday.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_workday.PlaceholderText = "";
            this.txt_workday.ReadOnly = false;
            this.txt_workday.Size = new System.Drawing.Size(295, 35);
            this.txt_workday.TabIndex = 0;
            this.txt_workday.Texts = "";
            this.txt_workday.UnderlinedStyle = true;
            this.txt_workday.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_workday_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(339, 241);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 16);
            this.label6.TabIndex = 40;
            this.label6.Text = "s";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(339, 399);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 16);
            this.label8.TabIndex = 41;
            this.label8.Text = "min";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(339, 97);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 16);
            this.label9.TabIndex = 42;
            this.label9.Text = "hour";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.DimGray;
            this.label10.Location = new System.Drawing.Point(339, 168);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 16);
            this.label10.TabIndex = 43;
            this.label10.Text = "shift";
            // 
            // txt_SpaceCost
            // 
            this.txt_SpaceCost.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_SpaceCost.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_SpaceCost.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_SpaceCost.BorderRadius = 0;
            this.txt_SpaceCost.BorderSize = 1;
            this.txt_SpaceCost.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_SpaceCost.ForeColor = System.Drawing.Color.DimGray;
            this.txt_SpaceCost.Location = new System.Drawing.Point(471, 159);
            this.txt_SpaceCost.Margin = new System.Windows.Forms.Padding(5);
            this.txt_SpaceCost.Multiline = false;
            this.txt_SpaceCost.Name = "txt_SpaceCost";
            this.txt_SpaceCost.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_SpaceCost.PasswordChar = false;
            this.txt_SpaceCost.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_SpaceCost.PlaceholderText = "";
            this.txt_SpaceCost.ReadOnly = false;
            this.txt_SpaceCost.Size = new System.Drawing.Size(295, 35);
            this.txt_SpaceCost.TabIndex = 7;
            this.txt_SpaceCost.Texts = "";
            this.txt_SpaceCost.UnderlinedStyle = true;
            this.txt_SpaceCost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_workday_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(468, 133);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(169, 16);
            this.label11.TabIndex = 46;
            this.label11.Text = "Space Cost (건물상각비)";
            // 
            // txt_Imputed
            // 
            this.txt_Imputed.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_Imputed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_Imputed.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_Imputed.BorderRadius = 0;
            this.txt_Imputed.BorderSize = 1;
            this.txt_Imputed.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_Imputed.ForeColor = System.Drawing.Color.DimGray;
            this.txt_Imputed.Location = new System.Drawing.Point(471, 88);
            this.txt_Imputed.Margin = new System.Windows.Forms.Padding(5);
            this.txt_Imputed.Multiline = false;
            this.txt_Imputed.Name = "txt_Imputed";
            this.txt_Imputed.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_Imputed.PasswordChar = false;
            this.txt_Imputed.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_Imputed.PlaceholderText = "";
            this.txt_Imputed.ReadOnly = false;
            this.txt_Imputed.Size = new System.Drawing.Size(295, 35);
            this.txt_Imputed.TabIndex = 6;
            this.txt_Imputed.Texts = "";
            this.txt_Imputed.UnderlinedStyle = true;
            this.txt_Imputed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_workday_KeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.Color.DimGray;
            this.label12.Location = new System.Drawing.Point(468, 62);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(236, 16);
            this.label12.TabIndex = 47;
            this.label12.Text = "Imputed Depreciation (기계상각비)";
            // 
            // btn_CalcMaintance
            // 
            this.btn_CalcMaintance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.btn_CalcMaintance.FlatAppearance.BorderSize = 0;
            this.btn_CalcMaintance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CalcMaintance.Font = new System.Drawing.Font("Microsoft PhagsPa", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CalcMaintance.ForeColor = System.Drawing.Color.White;
            this.btn_CalcMaintance.Location = new System.Drawing.Point(471, 305);
            this.btn_CalcMaintance.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_CalcMaintance.Name = "btn_CalcMaintance";
            this.btn_CalcMaintance.Size = new System.Drawing.Size(295, 40);
            this.btn_CalcMaintance.TabIndex = 48;
            this.btn_CalcMaintance.Text = "계산";
            this.btn_CalcMaintance.UseVisualStyleBackColor = false;
            this.btn_CalcMaintance.Click += new System.EventHandler(this.btn_CalcMaintance_Click);
            // 
            // txt_Maintance
            // 
            this.txt_Maintance.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_Maintance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_Maintance.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_Maintance.BorderRadius = 0;
            this.txt_Maintance.BorderSize = 1;
            this.txt_Maintance.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_Maintance.ForeColor = System.Drawing.Color.DimGray;
            this.txt_Maintance.Location = new System.Drawing.Point(471, 383);
            this.txt_Maintance.Margin = new System.Windows.Forms.Padding(5);
            this.txt_Maintance.Multiline = false;
            this.txt_Maintance.Name = "txt_Maintance";
            this.txt_Maintance.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_Maintance.PasswordChar = false;
            this.txt_Maintance.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_Maintance.PlaceholderText = "";
            this.txt_Maintance.ReadOnly = false;
            this.txt_Maintance.Size = new System.Drawing.Size(295, 35);
            this.txt_Maintance.TabIndex = 9;
            this.txt_Maintance.Texts = "";
            this.txt_Maintance.UnderlinedStyle = true;
            this.txt_Maintance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_lot_KeyPress);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.ForeColor = System.Drawing.Color.DimGray;
            this.label13.Location = new System.Drawing.Point(468, 357);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 16);
            this.label13.TabIndex = 50;
            this.label13.Text = "수선비";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.ForeColor = System.Drawing.Color.DimGray;
            this.label14.Location = new System.Drawing.Point(32, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(102, 22);
            this.label14.TabIndex = 51;
            this.label14.Text = "Lot 계산기";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.DimGray;
            this.label15.Location = new System.Drawing.Point(467, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(130, 22);
            this.label15.TabIndex = 52;
            this.label15.Text = "수선비 계산기";
            // 
            // txt_MaintainRate
            // 
            this.txt_MaintainRate.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_MaintainRate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_MaintainRate.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_MaintainRate.BorderRadius = 0;
            this.txt_MaintainRate.BorderSize = 1;
            this.txt_MaintainRate.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_MaintainRate.ForeColor = System.Drawing.Color.DimGray;
            this.txt_MaintainRate.Location = new System.Drawing.Point(471, 232);
            this.txt_MaintainRate.Margin = new System.Windows.Forms.Padding(5);
            this.txt_MaintainRate.Multiline = false;
            this.txt_MaintainRate.Name = "txt_MaintainRate";
            this.txt_MaintainRate.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.txt_MaintainRate.PasswordChar = false;
            this.txt_MaintainRate.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_MaintainRate.PlaceholderText = "";
            this.txt_MaintainRate.ReadOnly = false;
            this.txt_MaintainRate.Size = new System.Drawing.Size(295, 35);
            this.txt_MaintainRate.TabIndex = 8;
            this.txt_MaintainRate.Texts = "";
            this.txt_MaintainRate.UnderlinedStyle = true;
            this.txt_MaintainRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_workday_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.ForeColor = System.Drawing.Color.DimGray;
            this.label16.Location = new System.Drawing.Point(468, 206);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 16);
            this.label16.TabIndex = 54;
            this.label16.Text = "수선비율";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.ForeColor = System.Drawing.Color.DimGray;
            this.label17.Location = new System.Drawing.Point(774, 241);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(20, 16);
            this.label17.TabIndex = 55;
            this.label17.Text = "%";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.ForeColor = System.Drawing.Color.DimGray;
            this.label18.Location = new System.Drawing.Point(774, 97);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(79, 16);
            this.label18.TabIndex = 56;
            this.label18.Text = "currency/h";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.ForeColor = System.Drawing.Color.DimGray;
            this.label19.Location = new System.Drawing.Point(774, 168);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(79, 16);
            this.label19.TabIndex = 57;
            this.label19.Text = "currency/h";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("LG스마트체2.0 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.ForeColor = System.Drawing.Color.DimGray;
            this.label20.Location = new System.Drawing.Point(538, 206);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(190, 16);
            this.label20.TabIndex = 58;
            this.label20.Text = "(1Shift = 6%, 2Shift =12%)";
            // 
            // frmLotCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(870, 598);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txt_MaintainRate);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txt_Maintance);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btn_CalcMaintance);
            this.Controls.Add(this.txt_SpaceCost);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txt_Imputed);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_cavity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_lot);
            this.Controls.Add(this.txt_setup);
            this.Controls.Add(this.txt_ct);
            this.Controls.Add(this.txt_shift);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_workday);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Calc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLotCalc";
            this.Text = "frmLotCalc";
            this.Load += new System.EventHandler(this.frmLotCalc_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomControls.RJControls.RJTextBox txt_lot;
        private CustomControls.RJControls.RJTextBox txt_setup;
        private CustomControls.RJControls.RJTextBox txt_ct;
        private CustomControls.RJControls.RJTextBox txt_workday;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Calc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private CustomControls.RJControls.RJTextBox txt_cavity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private CustomControls.RJControls.RJTextBox txt_shift;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private CustomControls.RJControls.RJTextBox txt_SpaceCost;
        private System.Windows.Forms.Label label11;
        private CustomControls.RJControls.RJTextBox txt_Imputed;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btn_CalcMaintance;
        private CustomControls.RJControls.RJTextBox txt_Maintance;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private CustomControls.RJControls.RJTextBox txt_MaintainRate;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
    }
}