using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TcPCM_Connect_Global
{
    public partial class Loading : Form
    {
        delegate void StringParameterDelegate(string Text);
        delegate void StringParameterWithStatusDelegate(string Text);
        delegate void SplashShowCloseDelegate();

        bool CloseSplashScreenFlag = false;
        bool TextPanelVisibleFlag = false;
        bool ProgressbarVisibleFlag = false;

        public Loading()
        {
            InitializeComponent();
            txt_msg.Visible = TextPanelVisibleFlag;
            progressBar1.Visible = ProgressbarVisibleFlag;
            txt_msg.TabStop = false;
            txt_msg.Cursor = Cursors.Arrow;
            txt_msg.HideSelection = false;
            txt_msg.AutoSize = true;
            // 라벨의 AutoSize 속성을 true로 설정하여 내용에 맞게 자동으로 크기 조정
            //lb_msg.AutoSize = true;

            // 폼의 AutoSize 속성을 true로 설정하여 라벨 크기에 맞게 자동으로 크기 조정
            //this.AutoSize = true;

            // 폼의 AutoSizeMode을 GrowAndShrink로 설정하여 적절하게 크기 조정
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        public void ShowSplashScreen()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SplashShowCloseDelegate(ShowSplashScreen));
                return;
            }
            this.Show();
            Application.Run(this);
        }

        public void CloseSplashScreen()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SplashShowCloseDelegate(CloseSplashScreen));
                return;
            }
            CloseSplashScreenFlag = true;
            this.Close();
        }

        public void UdpateStatusTextWithStatus(string Text)
        {
            if (InvokeRequired)
            {
                Invoke(new StringParameterWithStatusDelegate(UdpateStatusTextWithStatusInternal), new object[] { Text });
                return;
            }

            UdpateStatusTextWithStatusInternal(Text);
        }

        private void SplashForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseSplashScreenFlag == false)
                e.Cancel = true;
        }

        private void UdpateStatusTextWithStatusInternal(string Text)
        {
            txt_msg.Text = Text;
            if (txt_msg.Text.Length > 0) TextPanelVisibleFlag = true;
            txt_msg.Visible = TextPanelVisibleFlag;
            if(txt_msg.Text.Contains("/"))
            {
                ProgressbarVisibleFlag = true;
                progressBar1.Visible = TextPanelVisibleFlag;
                progressBar1.Value = (int)((global.ConvertDouble(Text.Split('/')[0]) / global.ConvertDouble(Text.Split('/')[1]))*100);

                pictureBox1.Size = new Size(50,50);
                pictureBox1.Location
                    = new Point((int)((progressBar1.Size.Width - pictureBox1.Size.Width) /2), (int)((this.Size.Height - pictureBox1.Size.Height - txt_msg.Size.Height) / 2));
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            txt_msg.SelectionStart = txt_msg.TextLength;
            this.TransparencyKey = txt_msg.BackColor;
            txt_msg.ScrollToCaret();
        }
    }
}
