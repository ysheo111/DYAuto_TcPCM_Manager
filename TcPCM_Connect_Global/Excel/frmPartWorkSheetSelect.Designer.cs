
namespace TcPCM_Connect_Global
{
    partial class frmPartWorkSheetSelect
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.combo_Part = new System.Windows.Forms.ComboBox();
            this.combo_Manufacturing = new System.Windows.Forms.ComboBox();
            this.btn_Check = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "부품원가계산서";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "제조경비 산출근거";
            // 
            // combo_Part
            // 
            this.combo_Part.FormattingEnabled = true;
            this.combo_Part.Location = new System.Drawing.Point(141, 22);
            this.combo_Part.Name = "combo_Part";
            this.combo_Part.Size = new System.Drawing.Size(200, 20);
            this.combo_Part.TabIndex = 2;
            // 
            // combo_Manufacturing
            // 
            this.combo_Manufacturing.FormattingEnabled = true;
            this.combo_Manufacturing.Location = new System.Drawing.Point(141, 64);
            this.combo_Manufacturing.Name = "combo_Manufacturing";
            this.combo_Manufacturing.Size = new System.Drawing.Size(200, 20);
            this.combo_Manufacturing.TabIndex = 3;
            // 
            // btn_Check
            // 
            this.btn_Check.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Check.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_Check.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Check.Location = new System.Drawing.Point(262, 101);
            this.btn_Check.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Check.Name = "btn_Check";
            this.btn_Check.Size = new System.Drawing.Size(79, 24);
            this.btn_Check.TabIndex = 64;
            this.btn_Check.Text = "확인";
            this.btn_Check.UseVisualStyleBackColor = false;
            this.btn_Check.Click += new System.EventHandler(this.btn_Check_Click);
            // 
            // frmPartWorkSheetSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(361, 138);
            this.Controls.Add(this.btn_Check);
            this.Controls.Add(this.combo_Manufacturing);
            this.Controls.Add(this.combo_Part);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmPartWorkSheetSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "부품원가계산서";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPartWorkSheetSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox combo_Part;
        private System.Windows.Forms.ComboBox combo_Manufacturing;
        private System.Windows.Forms.Button btn_Check;
    }
}