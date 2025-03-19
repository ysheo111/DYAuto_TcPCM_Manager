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
    public partial class frmVendorDetail : Form
    {
        public string infoId = "";
        public frmVendorDetail()
        {
            InitializeComponent();
        }
        private void ShiftSetting_Load(object sender, EventArgs e)
        {
            LoadMaterial();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            string query = "";

            foreach (DataGridViewRow row in dgv_VendorMaterial.Rows)
            {
                if (row.Cells[""].Value == null) continue;
                query += $@"";
            }
            global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
            LoadMaterial();
            CustomMessageBox.RJMessageBox.Show("수정이 완료 되었습니다.", "VendorPart"
                , MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void LoadMaterial()
        {
            string query = $@"SELECT * FROM VendorMaterial where VendorInfoId = {infoId}";
            DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);

            if (dataTable == null) return;
            dgv_VendorMaterial.Columns.Clear();
            dgv_VendorMaterial.DataSource = dataTable;
            dgv_VendorMaterial.Columns["Id"].Visible = false;
            dgv_VendorMaterial.Columns["VendorInfoId"].Visible = false;
            dgv_VendorMaterial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            query = $@"SELECT * FROM VendorManufacturing where VendorInfoId = {infoId}";
            dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);

            if (dataTable == null) return;
            dgv_VendorManufacturing.Columns.Clear();
            dgv_VendorManufacturing.DataSource = dataTable;
            dgv_VendorManufacturing.Columns["Id"].Visible = false;
            dgv_VendorManufacturing.Columns["VendorInfoId"].Visible = false;
            dgv_VendorManufacturing.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private void 삭제하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (DataRow row in dgv_VendorMaterial.SelectedRows)
            //{
            //    string query = $"delete from VendorMaterial VendorInfo where id = {row["Id"]}";
            //    string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
            //}
            foreach (var item in items)
            {
                if(item.Value == "Material")
                {
                    string query = $"delete from VendorMaterial where id = {item.Key}";
                    string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                }
                else if(item.Value == "Manufacturing")
                {
                    string query = $"delete from VendorManufacturing where id = {item.Key}";
                    string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                }
            }
        }
        Dictionary<string, string> items = new Dictionary<string, string>();
        private void dgv_VendorDetail_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //List<string> item = new List<string>();
                foreach (DataGridViewCell cell in dgv_VendorMaterial.SelectedCells)
                {
                    //item.Add(dgv_VendorMaterial.Rows[cell.RowIndex].Cells["Id"].Value.ToString());
                    if (items.ContainsKey(dgv_VendorMaterial.Rows[cell.RowIndex].Cells["Id"].Value.ToString())) continue;
                    items.Add(dgv_VendorMaterial.Rows[cell.RowIndex].Cells["Id"].Value.ToString(), "Material");
                }
                foreach (DataGridViewCell cell in dgv_VendorManufacturing.SelectedCells)
                {
                    //item.Add(dgv_VendorManufacturing.Rows[cell.RowIndex].Cells["Id"].Value.ToString());
                    if (items.ContainsKey(dgv_VendorManufacturing.Rows[cell.RowIndex].Cells["Id"].Value.ToString())) continue;
                    items.Add(dgv_VendorManufacturing.Rows[cell.RowIndex].Cells["Id"].Value.ToString(), "Manufacturing");
                }

                ContextMenuStrip menu = contextMenuStrip1;
                menu.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void dgv_VendorManufacturing_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                foreach (DataGridViewCell cell in dgv_VendorMaterial.SelectedCells)
                {
                    if (items.ContainsKey(dgv_VendorMaterial.Rows[cell.RowIndex].Cells["Id"].Value.ToString())) continue;
                    items.Add(dgv_VendorMaterial.Rows[cell.RowIndex].Cells["Id"].Value.ToString(), "Material");
                }
                foreach (DataGridViewCell cell in dgv_VendorManufacturing.SelectedCells)
                {
                    if (items.ContainsKey(dgv_VendorManufacturing.Rows[cell.RowIndex].Cells["Id"].Value.ToString())) continue;
                    items.Add(dgv_VendorManufacturing.Rows[cell.RowIndex].Cells["Id"].Value.ToString(), "Manufacturing");
                }

                ContextMenuStrip menu = contextMenuStrip1;
                menu.Show(MousePosition.X, MousePosition.Y);
            }
        }
    }
}
