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

namespace TcPCM_Connect.Component
{
    public partial class ShiftSetting : Form
    {
        public ShiftSetting()
        {
            InitializeComponent();
        }
        private void ShiftSetting_Load(object sender, EventArgs e)
        {
            LoadConfiguration();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            string query = "";

            foreach (DataGridViewRow row in dgv_Shift.Rows)
            {
                if (row.Cells["Name"].Value == null || row.Cells["ShiftRate"].Value == null) continue;
                query += $@"Update Shift 
                    Set Name=N'{row.Cells["Name"].Value}',ShiftRate='{row.Cells["ShiftRate"].Value}'
                    Where ID={row.Cells["Id"].Value};
                    ";
            }
            global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
            LoadConfiguration();
            CustomMessageBox.RJMessageBox.Show("수정이 완료 되었습니다.", "Shift"
                , MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void LoadConfiguration()
        {
            string query = $@"SELECT * FROM Shift";

            DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);

            if (dataTable == null) return;
            dgv_Shift.Columns.Clear();
            dgv_Shift.DataSource = dataTable;
            dgv_Shift.Columns["Id"].Visible = false;
            dgv_Shift.Columns["Name"].ReadOnly = true;
            dgv_Shift.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
