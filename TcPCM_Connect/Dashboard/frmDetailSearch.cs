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
    public partial class frmDetailSearch : Form
    {
        public string ReturnValue1 { get; set; }
        public string ReturnValue2 { get; set; }
        public string ReturnValue3 { get; set; }
        public frmDetailSearch()
        {
            InitializeComponent();
        }
        private void ConfigSetting_Load(object sender, EventArgs e)
        {
            LoadConfiguration();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            string partCaseQuery="", folderCaseQuery = "", projectCaseQuery = "";

            if(cb_date.Checked)
            {
                folderCaseQuery += $"ModificationDate between '{dt_start.Value.ToString("YYYY-MM-DD hh:mm:ss")}' and '{dt_end.Value.ToString("YYYY-MM-DD hh:mm:ss")}'";
                projectCaseQuery = folderCaseQuery+ $" or CreationDate between '{dt_start.Value.ToString("YYYY-MM-DD hh:mm:ss")}' and '{dt_end.Value.ToString("YYYY-MM-DD hh:mm:ss")}'";
                partCaseQuery = projectCaseQuery+ $" or CalculationTime between '{dt_start.Value.ToString("YYYY-MM-DD hh:mm:ss")}' and '{dt_end.Value.ToString("YYYY-MM-DD hh:mm:ss")}'";
            }

            if(cb_manufacturing.Checked)
            {
                partCaseQuery += $" {(partCaseQuery.Length > 0 ? " and " : "")} id in (select calculationid from ManufacturingSteps cast(Name_LOC as nvarchar(MAX)) Like '%{tb_manufacturing.Texts}%')";
            }

            ReturnValue1 = folderCaseQuery;
            ReturnValue2 = projectCaseQuery;
            ReturnValue3 = partCaseQuery;
            //foreach (projectCaseQuery row in dgv_Config.Rows)
            //{
            //    if (row.Cells["Name"].Value == null || row.Cells["GUID"].Value==null) continue;
            //    query += $@"Update Configuration 
            //        Set Name=N'{row.Cells["Name"].Value.ToString().Replace(row.Cells["Name"].Value.ToString().Split('_')[0]+"_","")}',GUID='{row.Cells["GUID"].Value}'
            //        Where ID={row.Cells["Id"].Value};
            //        ";
            //}
            //global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
            //LoadConfiguration();
            //CustomMessageBox.RJMessageBox.Show("수정이 완료 되었습니다.", "Configuration"
            //    , MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //-> Events Methods
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadConfiguration()
        {
            //string query = $@"Select Id, Concat(Class, '_', Name) as Name, GUID From Configuration Where Class = '{className}'";

            //DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);

            //if (dataTable == null) return;
            //dgv_Config.Columns.Clear();
            //dgv_Config.DataSource = dataTable;
            //dgv_Config.Columns["Id"].Visible = false;
            //dgv_Config.Columns["Name"].ReadOnly = true;
            //dgv_Config.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private void combo_date_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            dt_end.Value = DateTime.Now;
            string DateType = combo_date.SelectedItem.ToString();
            switch (DateType)
            {
                case "오늘":
                    dt_start.Value = DateTime.Now;
                    break;
                case "당월":
                    dt_start.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                    dt_end.Value = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day);
                    break;
                case "전월":
                    dt_start.Value = DateTime.Now.AddMonths(-1);
                    break;
                case "3개월":
                    dt_start.Value = DateTime.Now.AddMonths(-3);
                    break;
                case "6개월":
                    dt_start.Value = DateTime.Now.AddMonths(-6);
                    break;
                case "1년":
                    dt_start.Value = DateTime.Now.AddYears(-1);
                    break;
            }
        }
    }
}
