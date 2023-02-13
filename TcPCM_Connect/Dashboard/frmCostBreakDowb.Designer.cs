
namespace TcPCM_Connect.Dashboard
{
    partial class frmCostBreakDowb
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
            this.btn_Select = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_SearchName = new CustomControls.RJControls.RJTextBox();
            this.SuspendLayout();
            // 
            // btn_Select
            // 
            this.btn_Select.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.btn_Select.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Select.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btn_Select.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.btn_Select.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.btn_Select.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Select.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Select.ForeColor = System.Drawing.Color.Silver;
            this.btn_Select.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Select.Location = new System.Drawing.Point(372, 22);
            this.btn_Select.Name = "btn_Select";
            this.btn_Select.Size = new System.Drawing.Size(87, 31);
            this.btn_Select.TabIndex = 55;
            this.btn_Select.Text = "조회";
            this.btn_Select.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Select.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(32, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 54;
            this.label2.Text = "조회 품목 : ";
            // 
            // txt_SearchName
            // 
            this.txt_SearchName.BackColor = System.Drawing.SystemColors.Window;
            this.txt_SearchName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(34)))), ((int)(((byte)(39)))));
            this.txt_SearchName.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.txt_SearchName.BorderRadius = 0;
            this.txt_SearchName.BorderSize = 2;
            this.txt_SearchName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_SearchName.ForeColor = System.Drawing.Color.DimGray;
            this.txt_SearchName.Location = new System.Drawing.Point(113, 22);
            this.txt_SearchName.Margin = new System.Windows.Forms.Padding(4);
            this.txt_SearchName.Multiline = false;
            this.txt_SearchName.Name = "txt_SearchName";
            this.txt_SearchName.Padding = new System.Windows.Forms.Padding(7);
            this.txt_SearchName.PasswordChar = false;
            this.txt_SearchName.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txt_SearchName.PlaceholderText = "";
            this.txt_SearchName.ReadOnly = false;
            this.txt_SearchName.Size = new System.Drawing.Size(237, 31);
            this.txt_SearchName.TabIndex = 53;
            this.txt_SearchName.Texts = "";
            this.txt_SearchName.UnderlinedStyle = true;
            // 
            // frmCostBreakDowb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_Select);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_SearchName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCostBreakDowb";
            this.Text = "frmCostBreakDowb";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Select;
        private System.Windows.Forms.Label label2;
        private CustomControls.RJControls.RJTextBox txt_SearchName;
    }
}