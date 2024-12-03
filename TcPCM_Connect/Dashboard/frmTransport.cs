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
    public partial class frmTransport : Form
    {
        public string className = "";
        public frmTransport()
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
                if (row.Cells["나라"].Value == null || row.Cells["차종"].Value == null) continue;
                query += $@"
                    IF EXISTS (SELECT * FROM Transport WHERE ID ='{row.Cells["ID"].Value}') 
                    BEGIN
                        Update Transport 
                        Set [차종]=N'{row.Cells["차종"].Value}',[통화]=N'{row.Cells["통화"].Value}',[나라]=N'{row.Cells["나라"].Value}',[변동비]='{row.Cells["변동비"].Value}',[고정비]='{row.Cells["고정비"].Value}',[Pallet 수량]='{row.Cells["Pallet 수량"].Value}'
                        Where ID='{row.Cells["ID"].Value}';
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Transport([나라], [통화], [차종], [변동비] ,[고정비] ,[Pallet 수량])
                        Values (N'{row.Cells["나라"].Value}',N'{row.Cells["통화"].Value}',N'{row.Cells["차종"].Value}','{row.Cells["변동비"].Value}','{row.Cells["고정비"].Value}','{row.Cells["Pallet 수량"].Value}')
                    END
                    ";
            }
            string msg = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
            if (msg.Length <= 0)
            {
                CustomMessageBox.RJMessageBox.Show("수정이 완료 되었습니다.", "Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadConfiguration();
            }
            else
            {
                CustomMessageBox.RJMessageBox.Show("수정이 실패 되었습니다.", "Configuration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //-> Events Methods
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadConfiguration()
        {
            string query = $@"SELECT [ID], [나라],[통화], [차종], [변동비] ,[고정비] ,[Pallet 수량]  FROM [Transport]";

            DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);

            if (dataTable == null) return;
            dgv_Config.Columns.Clear();
            dgv_Config.DataSource = dataTable;
            dgv_Config.Columns["ID"].Visible = false;
            //dgv_Config.Columns["Name"].ReadOnly = true;
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
