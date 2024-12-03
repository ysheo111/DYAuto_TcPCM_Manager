
namespace TcPCM_Connect
{
    partial class subfrmSignUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(subfrmSignUp));
            this.BarraTitulo = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnCerrar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_LastName = new CustomControls.RJControls.RJTextBox();
            this.txt_FirstName = new CustomControls.RJControls.RJTextBox();
            this.txt_Password = new CustomControls.RJControls.RJTextBox();
            this.txt_ID = new CustomControls.RJControls.RJTextBox();
            this.btn_SignUp = new System.Windows.Forms.Button();
            this.BarraTitulo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BarraTitulo
            // 
            this.BarraTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.BarraTitulo.Controls.Add(this.label6);
            this.BarraTitulo.Controls.Add(this.BtnCerrar);
            this.BarraTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.BarraTitulo.Location = new System.Drawing.Point(0, 0);
            this.BarraTitulo.Name = "BarraTitulo";
            this.BarraTitulo.Size = new System.Drawing.Size(314, 35);
            this.BarraTitulo.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 13.8F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(103, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 24);
            this.label6.TabIndex = 15;
            this.label6.Text = "회원가입";
            // 
            // BtnCerrar
            // 
            this.BtnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCerrar.FlatAppearance.BorderSize = 0;
            this.BtnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCerrar.Image = global::TcPCM_Connect.Properties.Resources._1564505_close_delete_exit_remove_icon;
            this.BtnCerrar.Location = new System.Drawing.Point(270, 0);
            this.BtnCerrar.Name = "BtnCerrar";
            this.BtnCerrar.Size = new System.Drawing.Size(44, 35);
            this.BtnCerrar.TabIndex = 4;
            this.BtnCerrar.UseVisualStyleBackColor = true;
            this.BtnCerrar.Click += new System.EventHandler(this.BtnCerrar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(21, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 15);
            this.label5.TabIndex = 28;
            this.label5.Text = "아이디";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(21, 331);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 15);
            this.label3.TabIndex = 23;
            this.label3.Text = "이름";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(21, 261);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 15);
            this.label1.TabIndex = 22;
            this.label1.Text = "성";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(21, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "비밀번호";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txt_LastName);
            this.panel1.Controls.Add(this.txt_FirstName);
            this.panel1.Controls.Add(this.txt_Password);
            this.panel1.Controls.Add(this.txt_ID);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btn_SignUp);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(314, 495);
            this.panel1.TabIndex = 29;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(18, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(274, 30);
            this.label7.TabIndex = 34;
            this.label7.Text = "아래 항목들을 모두 입력하시고\r\n회원가입 버튼을 누르시면 회원가입이 완료됩니다.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 20F, System.Drawing.FontStyle.Regular);
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(13, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 31);
            this.label4.TabIndex = 33;
            this.label4.Text = "환영합니다!";
            // 
            // txt_LastName
            // 
            this.txt_LastName.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_LastName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_LastName.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_LastName.BorderRadius = 0;
            this.txt_LastName.BorderSize = 1;
            this.txt_LastName.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_LastName.ForeColor = System.Drawing.Color.DimGray;
            this.txt_LastName.Location = new System.Drawing.Point(24, 352);
            this.txt_LastName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_LastName.Multiline = false;
            this.txt_LastName.Name = "txt_LastName";
            this.txt_LastName.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txt_LastName.PasswordChar = false;
            this.txt_LastName.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_LastName.PlaceholderText = "";
            this.txt_LastName.ReadOnly = false;
            this.txt_LastName.Size = new System.Drawing.Size(253, 30);
            this.txt_LastName.TabIndex = 3;
            this.txt_LastName.Texts = "";
            this.txt_LastName.UnderlinedStyle = true;
            // 
            // txt_FirstName
            // 
            this.txt_FirstName.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_FirstName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_FirstName.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_FirstName.BorderRadius = 0;
            this.txt_FirstName.BorderSize = 1;
            this.txt_FirstName.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_FirstName.ForeColor = System.Drawing.Color.DimGray;
            this.txt_FirstName.Location = new System.Drawing.Point(24, 282);
            this.txt_FirstName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_FirstName.Multiline = false;
            this.txt_FirstName.Name = "txt_FirstName";
            this.txt_FirstName.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txt_FirstName.PasswordChar = false;
            this.txt_FirstName.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_FirstName.PlaceholderText = "";
            this.txt_FirstName.ReadOnly = false;
            this.txt_FirstName.Size = new System.Drawing.Size(253, 30);
            this.txt_FirstName.TabIndex = 2;
            this.txt_FirstName.Texts = "";
            this.txt_FirstName.UnderlinedStyle = true;
            // 
            // txt_Password
            // 
            this.txt_Password.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_Password.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_Password.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_Password.BorderRadius = 0;
            this.txt_Password.BorderSize = 1;
            this.txt_Password.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_Password.ForeColor = System.Drawing.Color.DimGray;
            this.txt_Password.Location = new System.Drawing.Point(24, 212);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Password.Multiline = false;
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txt_Password.PasswordChar = true;
            this.txt_Password.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_Password.PlaceholderText = "";
            this.txt_Password.ReadOnly = false;
            this.txt_Password.Size = new System.Drawing.Size(253, 30);
            this.txt_Password.TabIndex = 1;
            this.txt_Password.Texts = "";
            this.txt_Password.UnderlinedStyle = true;
            // 
            // txt_ID
            // 
            this.txt_ID.BackColor = System.Drawing.Color.Gainsboro;
            this.txt_ID.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.txt_ID.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(59)))), ((int)(((byte)(173)))));
            this.txt_ID.BorderRadius = 0;
            this.txt_ID.BorderSize = 1;
            this.txt_ID.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_ID.ForeColor = System.Drawing.Color.DimGray;
            this.txt_ID.Location = new System.Drawing.Point(24, 142);
            this.txt_ID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_ID.Multiline = false;
            this.txt_ID.Name = "txt_ID";
            this.txt_ID.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txt_ID.PasswordChar = false;
            this.txt_ID.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_ID.PlaceholderText = "";
            this.txt_ID.ReadOnly = false;
            this.txt_ID.Size = new System.Drawing.Size(253, 30);
            this.txt_ID.TabIndex = 0;
            this.txt_ID.Texts = "";
            this.txt_ID.UnderlinedStyle = true;
            // 
            // btn_SignUp
            // 
            this.btn_SignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(48)))), ((int)(((byte)(213)))));
            this.btn_SignUp.FlatAppearance.BorderSize = 0;
            this.btn_SignUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SignUp.Font = new System.Drawing.Font("감탄로드돋움체 Regular", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_SignUp.ForeColor = System.Drawing.Color.White;
            this.btn_SignUp.Location = new System.Drawing.Point(24, 431);
            this.btn_SignUp.Name = "btn_SignUp";
            this.btn_SignUp.Size = new System.Drawing.Size(253, 32);
            this.btn_SignUp.TabIndex = 4;
            this.btn_SignUp.Text = "회원가입";
            this.btn_SignUp.UseVisualStyleBackColor = false;
            this.btn_SignUp.Click += new System.EventHandler(this.btn_SignUp_Click);
            // 
            // subfrmSignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(314, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BarraTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "subfrmSignUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "회원가입";
            this.BarraTitulo.ResumeLayout(false);
            this.BarraTitulo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BarraTitulo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnCerrar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private CustomControls.RJControls.RJTextBox txt_ID;
        private CustomControls.RJControls.RJTextBox txt_Password;
        private CustomControls.RJControls.RJTextBox txt_FirstName;
        private CustomControls.RJControls.RJTextBox txt_LastName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_SignUp;
    }
}