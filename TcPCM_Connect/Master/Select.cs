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
    public partial class Select : Form
    {
        public string className = "";
        public string query = "";
        public Select()
        {
            InitializeComponent();
        }
        private void ConfigSetting_Load(object sender, EventArgs e)
        {
            LoadConfiguration();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            //if (className == "Material")
            //    query = " AND";
            //else
            //    query = "Where";
            query += $" DateValidFrom BETWEEN '{rjDatePicker1.Value.ToString("yyyy-MM-dd")}' AND '{rjDatePicker2.Value.ToString("yyyy-MM-dd")}'";
            this.DialogResult = DialogResult.OK;
            this.Close();

            //foreach (DataGridViewRow row in dgv_Config.Rows)
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
            rjDatePicker2.Value = DateTime.Now;
            string DateType = combo_date.SelectedItem.ToString();
            switch (DateType)
            {
                case "오늘":
                    rjDatePicker1.Value = DateTime.Now;
                    break;
                case "당월":
                    rjDatePicker1.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                    rjDatePicker2.Value = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day);
                    break;
                case "전월":
                    rjDatePicker1.Value = DateTime.Now.AddMonths(-1);
                    break;
                case "3개월":
                    rjDatePicker1.Value = DateTime.Now.AddMonths(-3);
                    break;
                case "6개월":
                    rjDatePicker1.Value = DateTime.Now.AddMonths(-6);
                    break;
                case "1년":
                    rjDatePicker1.Value = DateTime.Now.AddYears(-1);
                    break;
            }
        }
    }
}
