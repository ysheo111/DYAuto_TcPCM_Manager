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
        public string buttonName = "";

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
                        Where ID={row.Cells["Id"].Value}; ";
                
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
            dgv_Config.Columns.Clear();
           
                string query = $@"Select Id, Concat(Class, '_', Name) as Name, GUID From Configuration Where Class = '{className}'";
                DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);
                if (dataTable == null) return;

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

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            dgv_Config.DataSource = null;
            dgv_Config.Columns.Clear();

            dgv_Config.Columns.Add("업종", "업종");
            dgv_Config.Columns["업종"].ReadOnly = true;
            dgv_Config.Columns.Add("반영율", "반영율");

            ExcelImport excel = new ExcelImport();
            string err = excel.LoadMasterData("Sprue", dgv_Config);
            dgv_Config.AllowUserToAddRows = false;

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Sprue", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            dgv_Config.Columns.Clear();
            string query = $@"SELECT 업종,반영율*100 as 반영율 FROM [PCI].[dbo].[Sprue]";
            //string query = "SELECT DISTINCT UniqueKey as name FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'";

            DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);
            if (dataTable == null) return;

            foreach(DataRow row in dataTable.Rows)
            {
                foreach(DataColumn col in dataTable.Columns)
                {
                    row[col] = row[col]?.ToString().Trim();
                }
            }
            dgv_Config.DataSource = dataTable;
            //dgv_Config.Columns["Id"].Visible = false;
            dgv_Config.Columns["업종"].ReadOnly = true;
        }
    }
}
