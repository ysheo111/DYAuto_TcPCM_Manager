using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomMessageBox.Private
{
public partial class FormMessageBox : Form
{
    //Fields
    private Color primaryColor = Color.DimGray;
    private int borderSize = 2;

        public string ReturnValue1 { get; set; }

        //Properties
        public Color PrimaryColor
    {
        get { return primaryColor; }
        set
        {
            primaryColor = value;
            this.BackColor = primaryColor;//Form Border Color
            this.panelTitleBar.BackColor = PrimaryColor;//Title Bar Back Color
        }
    }

    //Constructors
    public FormMessageBox(string text)
    {
        InitializeComponent();
        InitializeItems();
        this.PrimaryColor = primaryColor;
        this.labelMessage.Text = text;
        this.labelCaption.Text = "";
        SetFormSize();
        SetButtons(MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);//Set Default Buttons
    }
    public FormMessageBox(string text, string caption)
    {
        InitializeComponent();
        InitializeItems();
        this.PrimaryColor = System.Drawing.ColorTranslator.FromHtml("#462AD8") ;
        this.labelMessage.Text = text;
        this.labelCaption.Text = caption;
        SetFormSize();
        SetButtons(MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);//Set Default Buttons
            
    }
    public FormMessageBox(string text, string caption, MessageBoxButtons buttons)
    {
        InitializeComponent();
        InitializeItems();
        this.PrimaryColor = primaryColor;
        this.labelMessage.Text = text;
        this.labelCaption.Text = caption;
        SetFormSize();
        SetButtons(buttons, MessageBoxDefaultButton.Button1);//Set [Default Button 1]
    }

    public FormMessageBox(string text, string caption, MessageBoxButtons buttons, string[] combo)
    {
            InitializeComponent();
            InitializeItems();
            this.PrimaryColor = primaryColor;
            this.labelMessage.Text = text;
            this.labelCaption.Text = caption;

            SetFormSize();
            this.Size = new Size(this.Width, this.Height+cb_Select.Height+10);
            this.panelBody.Size = new Size(this.panelBody.Width, this.panelBody.Height+cb_Select.Height + 10);
            this.panelInput.Size = new Size(this.cb_Select.Width,cb_Select.Height + 10);

            panelInput.Visible = true;
            cb_Select.Visible = true;
            panelInput.Location = new Point(this.labelMessage.Location.X+10, this.labelMessage.Location.Y + this.labelMessage.Height);
            cb_Select.Location = new Point(0, 0);
            cb_Select.Items.AddRange(combo);
            cb_Select.SelectedIndex = 0;

            SetButtons(buttons, MessageBoxDefaultButton.Button1);//Set [Default Button 1]
            
            //this.panelBody.Height += cb_Select.Height;
            //this.Height += cb_Select.Height;

            //this.panelButtons.Location = new Point(this.panelButtons.Location.X, this.panelButtons.Location.Y+cb_Select.Height);
        }

        public FormMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
    {
        InitializeComponent();
        InitializeItems();
        this.PrimaryColor = primaryColor;
        this.labelMessage.Text = text;
        this.labelCaption.Text = caption;
        SetFormSize();
        SetButtons(buttons, defaultButton);
    }

    //-> Private Methods
    private void InitializeItems()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.Padding = new Padding(borderSize);//Set border size
        this.labelMessage.MaximumSize = new Size(550, 0);
        this.btnClose.DialogResult = DialogResult.Cancel;
        this.button1.DialogResult = DialogResult.OK;
        this.button1.Click += new System.EventHandler(this.button1_Click);

        this.button1.Visible = false;
        this.button2.Visible = false;
        this.button3.Visible = false;
        this.cb_Select.Visible = false;
        this.panelInput.Visible = false;
    }
    private void SetFormSize()
    {
        int widht = this.labelMessage.Width +  this.panelBody.Padding.Left;
        int height = this.panelTitleBar.Height + this.labelMessage.Height + this.panelButtons.Height + this.panelBody.Padding.Top;

        this.Size = new Size(widht, height);
    }
    private void SetButtons(MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
    {
        int xCenter = (this.panelButtons.Width - button1.Width) / 2;
        int yCenter = (this.panelButtons.Height - button1.Height) / 2;

        switch (buttons)
        {
            case MessageBoxButtons.OK:
                //OK Button
                button1.Visible = true;
                button1.Location = new Point(xCenter, yCenter);
                button1.Text = "OK";
                button1.DialogResult = DialogResult.OK;//Set DialogResult
                    button1.BackColor = PrimaryColor;
                //button1.BackColor = System.Drawing.ColorTranslator.FromHtml("#462AD8");

                    //Set Default Button
                    SetDefaultButton(defaultButton);
                break;
            case MessageBoxButtons.OKCancel:
                //OK Button
                button1.Visible = true;
                button1.Location = new Point(xCenter - (button1.Width / 2) - 5, yCenter);
                button1.Text = "OK";
                button1.DialogResult = DialogResult.OK;//Set DialogResult

                //Cancel Button
                button2.Visible = true;
                button2.Location = new Point(xCenter + (button2.Width / 2) + 5, yCenter);
                button2.Text = "Cancel";
                button2.DialogResult = DialogResult.Cancel;//Set DialogResult
                button2.BackColor = Color.DimGray;

                //Set Default Button
                if (defaultButton != MessageBoxDefaultButton.Button3)//There are only 2 buttons, so the Default Button cannot be Button3
                    SetDefaultButton(defaultButton);
                else SetDefaultButton(MessageBoxDefaultButton.Button1);

                break;

            case MessageBoxButtons.RetryCancel:
                //Retry Button
                button1.Visible = true;
                button1.Location = new Point(xCenter - (button1.Width / 2) - 5, yCenter);
                button1.Text = "Retry";
                button1.DialogResult = DialogResult.Retry;//Set DialogResult

                //Cancel Button
                button2.Visible = true;
                button2.Location = new Point(xCenter + (button2.Width / 2) + 5, yCenter);
                button2.Text = "Cancel";
                button2.DialogResult = DialogResult.Cancel;//Set DialogResult
                button2.BackColor = Color.DimGray;

                //Set Default Button
                if (defaultButton != MessageBoxDefaultButton.Button3)//There are only 2 buttons, so the Default Button cannot be Button3
                    SetDefaultButton(defaultButton);
                else SetDefaultButton(MessageBoxDefaultButton.Button1);
                break;

            case MessageBoxButtons.YesNo:
                //Yes Button
                button1.Visible = true;
                button1.Location = new Point(xCenter - (button1.Width / 2) - 5, yCenter);
                button1.Text = "Yes";
                button1.DialogResult = DialogResult.Yes;//Set DialogResult

                //No Button
                button2.Visible = true;
                button2.Location = new Point(xCenter + (button2.Width / 2) + 5, yCenter);
                button2.Text = "No";
                button2.DialogResult = DialogResult.No;//Set DialogResult
                button2.BackColor = Color.IndianRed;

                //Set Default Button
                if (defaultButton != MessageBoxDefaultButton.Button3)//There are only 2 buttons, so the Default Button cannot be Button3
                    SetDefaultButton(defaultButton);
                else SetDefaultButton(MessageBoxDefaultButton.Button1);
                break;
            case MessageBoxButtons.YesNoCancel:
                //Yes Button
                button1.Visible = true;
                button1.Location = new Point(xCenter - button1.Width - 5, yCenter);
                button1.Text = "Yes";
                button1.DialogResult = DialogResult.Yes;//Set DialogResult

                //No Button
                button2.Visible = true;
                button2.Location = new Point(xCenter, yCenter);
                button2.Text = "No";
                button2.DialogResult = DialogResult.No;//Set DialogResult
                button2.BackColor = Color.IndianRed;

                //Cancel Button
                button3.Visible = true;
                button3.Location = new Point(xCenter + button2.Width + 5, yCenter);
                button3.Text = "Cancel";
                button3.DialogResult = DialogResult.Cancel;//Set DialogResult
                button3.BackColor = Color.DimGray;

                //Set Default Button
                SetDefaultButton(defaultButton);
                break;

            case MessageBoxButtons.AbortRetryIgnore:
                //Abort Button
                button1.Visible = true;
                button1.Location = new Point(xCenter - button1.Width - 5, yCenter);
                button1.Text = "Abort";
                button1.DialogResult = DialogResult.Abort;//Set DialogResult
                button1.BackColor = Color.Goldenrod;

                //Retry Button
                button2.Visible = true;
                button2.Location = new Point(xCenter, yCenter);
                button2.Text = "Retry";
                button2.DialogResult = DialogResult.Retry;//Set DialogResult                    

                //Ignore Button
                button3.Visible = true;
                button3.Location = new Point(xCenter + button2.Width + 5, yCenter);
                button3.Text = "Ignore";
                button3.DialogResult = DialogResult.Ignore;//Set DialogResult
                button3.BackColor = Color.IndianRed;

                //Set Default Button
                SetDefaultButton(defaultButton);
                break;
        }
    }
    private void SetDefaultButton(MessageBoxDefaultButton defaultButton)
    {
        switch (defaultButton)
        {
            case MessageBoxDefaultButton.Button1://Focus button 1
                button1.Select();
                button1.ForeColor = Color.White;
                button1.Font = new Font(button1.Font, FontStyle.Underline);
                break;
            case MessageBoxDefaultButton.Button2://Focus button 2
                button2.Select();
                button2.ForeColor = Color.White;
                button2.Font = new Font(button2.Font, FontStyle.Underline);
                break;
            case MessageBoxDefaultButton.Button3://Focus button 3
                button3.Select();
                button3.ForeColor = Color.White;
                button3.Font = new Font(button3.Font, FontStyle.Underline);
                break;
        }
    }   

    //-> Events Methods
    private void btnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ReturnValue1 = cb_Select.Text;
        }

    #region -> Drag Form
    [DllImport("user32.DLL", EntryPoint = "SendMessage")]
    private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
    [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
    private extern static void ReleaseCapture();
    private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
    {
        ReleaseCapture();
        SendMessage(this.Handle, 0x112, 0xf012, 0);
    }
        #endregion

        private void cb_Select_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Select.Items[cb_Select.SelectedIndex].ToString() != TcPCM_Connect_Global.Bom.ManufacturingType.가공.ToString()) return;

            

        }
    }
}
