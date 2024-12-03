using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TcPCM_Connect
{
    public partial class Loading : Form
    {
        delegate void StringParameterDelegate(string Text);
        delegate void StringParameterWithStatusDelegate(string Text, string Detail);
        delegate void SplashShowCloseDelegate();

        bool CloseSplashScreenFlag = false;
        bool TextPanelVisibleFlag = false;

        public Loading()
        {
            InitializeComponent();
            txt_msg.Visible = TextPanelVisibleFlag;
            txt_msg.TabStop = false;
            txt_msg.Cursor = Cursors.Arrow;
            txt_msg.HideSelection = false;
            //txt_msg.AutoSize = true;
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

        public void UdpateStatusTextWithStatus(string Text, string Detail)
        {
            if (InvokeRequired)
            {
                Invoke(new StringParameterWithStatusDelegate(UdpateStatusTextWithStatusInternal), new object[] { Text, Detail });
                return;
            }

            UdpateStatusTextWithStatusInternal(Text, Detail);
        }

        private void SplashForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseSplashScreenFlag == false)
                e.Cancel = true;
        }

        private void UdpateStatusTextWithStatusInternal(string Text, string Detail)
        {
            txt_msg.Text = Detail;
            lb_msg.Text = Text;

            if (txt_msg.Text.Length > 0) TextPanelVisibleFlag = true;
            txt_msg.Visible = TextPanelVisibleFlag;

            txt_msg.SelectionStart = txt_msg.TextLength;
            txt_msg.ScrollToCaret();
        }
    }
}
