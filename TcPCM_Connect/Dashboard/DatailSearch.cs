using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class DetailSearch : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public const uint WM_SYSKEYDOWN = 0x104;


        public string className = "";
        public DetailSearch()
        {
            InitializeComponent();
        }
        public void ConfigSetting_Load(object sender, EventArgs e)
        {
            
        }

        public void btn_Create_Click(object sender, EventArgs e)
        {

        }

        //-> Events Methods
        public void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #region -> Drag Form
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        public extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        public extern static void ReleaseCapture();
        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        #endregion


    }
}
