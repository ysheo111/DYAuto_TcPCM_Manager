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
            if (className == "표준 공정 라이브러리")
            {
                if (!string.IsNullOrEmpty(txtInfo.Text))
                {
                    if (!string.IsNullOrEmpty(query)) query += " AND ";
                    query += $" Info = '{txtInfo.Text}' ";
                }
                if (!string.IsNullOrEmpty(txtPartName.Text))
                {
                    if (!string.IsNullOrEmpty(query)) query += " AND ";
                    query += $" PartName = '{txtPartName.Text}' ";
                }
                if (!string.IsNullOrEmpty(txtName.Text))
                {
                    if (!string.IsNullOrEmpty(query)) query += " AND ";
                    query += $" Name = N'{txtName.Text}' ";
                }
                if (!string.IsNullOrEmpty(txtCategory.Text))
                {
                    if (!string.IsNullOrEmpty(query)) query += " AND ";
                    query += $" Category = N'{txtCategory.Text}' ";
                }
                if (!string.IsNullOrEmpty(txtMachine.Text))
                {
                    if (!string.IsNullOrEmpty(query)) query += " AND ";
                    query += $" Machine = N'{txtMachine.Text}' ";
                }
            }
            else if(className == "전력단가" || className == "임률")
                query += $" COALESCE(A.DateValidFrom, B.DateValidFrom) BETWEEN '{rjDatePicker1.Value.ToString("yyyy-MM-dd")}' AND '{rjDatePicker2.Value.ToString("yyyy-MM-dd")}'";
            else //if (className == "material" || className == "공간 생산 비용")
                query += $" DateValidFrom BETWEEN '{rjDatePicker1.Value.ToString("yyyy-MM-dd")}' AND '{rjDatePicker2.Value.ToString("yyyy-MM-dd")}'";

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //-> Events Methods
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadConfiguration()
        {
            if(className == "표준 공정 라이브러리")
            {
                rjDatePicker1.Visible = false;
                rjDatePicker2.Visible = false;
                combo_date.Visible = false;

                label1.Text = "품번";
                txtInfo.Visible = true;
                label2.Text = "대표품명";
                label2.Visible = true;
                txtPartName.Visible = true;
                label3.Text = "세부 공정명";
                label3.Visible = true;
                txtName.Visible = true;
                label4.Text = "업종";
                label4.Visible = true;
                txtCategory.Visible = true;
                label5.Text = "장비명";
                label5.Visible = true;
                txtMachine.Visible = true;
            }
            else
            {
                rjDatePicker1.Visible = true;
                rjDatePicker2.Visible = true;
                combo_date.Visible = true;
            }

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
