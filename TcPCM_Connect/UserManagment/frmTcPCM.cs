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
    public partial class frmTcPCM : Form
    {

        public frmTcPCM()
        {
            InitializeComponent();
        }
        private void ConfigSetting_Load(object sender, EventArgs e)
        {
          
        }

        //-> Events Methods
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btn_Change_Click(object sender, EventArgs e)
        {
            if (txt_ID.Texts == null) return;
            string query = $@"Update TcPCM 
                    Set UserID='{txt_ID.Texts}',Password='{txt_Password.Texts}'
                    Where ID=1; ";

            global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
            CustomMessageBox.RJMessageBox.Show("수정이 완료 되었습니다.", "TcPCM 계정"
                , MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
