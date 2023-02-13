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
    public partial class ConfigSetting : Form
    {
        public string className = "";
        public ConfigSetting()
        {
            InitializeComponent();
        }
        private void ConfigSetting_Load(object sender, EventArgs e)
        {
            LoadConfiguration();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            string query = "";

            foreach (DataGridViewRow row in dgv_Config.Rows)
            {
                if (row.Cells["Name"].Value == null || row.Cells["GUID"].Value==null) continue;
                query += $@"Update Configuration 
                    Set Name=N'{row.Cells["Name"].Value.ToString().Replace(row.Cells["Name"].Value.ToString().Split('_')[0]+"_","")}',GUID='{row.Cells["GUID"].Value}'
                    Where ID={row.Cells["Id"].Value};
                    ";
            }
            global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
            LoadConfiguration();
            CustomMessageBox.RJMessageBox.Show("수정이 완료 되었습니다.", "Configuration"
                , MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //-> Events Methods
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadConfiguration()
        {
            string query = $@"Select Id, Concat(Class, '_', Name) as Name, GUID From Configuration Where Class = '{className}'";

            DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);

            if (dataTable == null) return;
            dgv_Config.Columns.Clear();
            dgv_Config.DataSource = dataTable;
            dgv_Config.Columns["Id"].Visible = false;
            dgv_Config.Columns["Name"].ReadOnly = true;
            dgv_Config.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

    }
}
