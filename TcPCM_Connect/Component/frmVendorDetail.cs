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
        private DataTable MaterialDT;
        private DataTable ManufacturingDT;
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
            List<string> valueList = new List<string>();
            foreach (DataGridViewRow row in dgv_VendorMaterial.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["Id"].Value == null) continue;

                query += $@"";
            }
            if(valueList.Count > 0)
            {
                global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                LoadMaterial();
                CustomMessageBox.RJMessageBox.Show("수정이 완료 되었습니다.", "VendorPart" , MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Update()
        {
            DataTable modifyRows = MaterialDT.GetChanges(DataRowState.Modified);
            if (modifyRows == null || modifyRows.Rows.Count == 0) return;
            foreach(DataRow row in modifyRows.Rows)
            {

            }
        }
        private void LoadMaterial()
        {
            string query = $@"SELECT id,VendorInfoId,[품번],[품명],[공급기준],[재질],[두께],[가로],[세로],[단위],[원재료단가],[Q'TY]
                        ,[폐기물처리비],[NET중량],[투입중량],[SCRAP단가],[비고]
                        FROM VendorMaterial where VendorInfoId = {infoId}";
            //DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);
            MaterialDT = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);

            if (MaterialDT == null) return;
            dgv_VendorMaterial.Columns.Clear();
            dgv_VendorMaterial.DataSource = MaterialDT;
            dgv_VendorMaterial.Columns["Id"].Visible = false;
            dgv_VendorMaterial.Columns["VendorInfoId"].Visible = false;
            dgv_VendorMaterial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            query = $@"SELECT id,VendorInfoId,[공정명],[품번],[업종],[작업자수]
                        ,[표준작업],[CVT],[Q'TY],[효율],[임율],[건물내용년수],[기계명],[년간가동일],[일일가동시간]
                        ,[설비취득가],[내용년수],[기계투영면적],[부대설비비율],[건축비],[수선비율],[전력용량],[전력단가]
                        ,[전력소비율],[기타설비내용년수],[간접경비율],[건물상각비],[투입비용],[비고]
                    FROM VendorManufacturing where VendorInfoId = {infoId}";
            //,[외주가공비],[외주개수],[외주가공명]
            ManufacturingDT = global_DB.MutiSelect(query, (int)global_DB.connDB.selfDB);

            if (ManufacturingDT == null) return;
            dgv_VendorManufacturing.Columns.Clear();
            dgv_VendorManufacturing.DataSource = ManufacturingDT;
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
            foreach (var item in items)
            {
                if(item.Value == "Material")
                {
                    string query = $"delete from VendorMaterial where id = {item.Key}";
                    string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                    if (!string.IsNullOrEmpty(result))
                    {
                        CustomMessageBox.RJMessageBox.Show($"삭제를 실패하였습니다\n{result}", "VendorMaterial", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if(item.Value == "Manufacturing")
                {
                    string query = $"delete from VendorManufacturing where id = {item.Key}";
                    string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                    if (!string.IsNullOrEmpty(result))
                    {
                        CustomMessageBox.RJMessageBox.Show($"삭제를 실패하였습니다\n{result}", "VendorMaterial", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            LoadMaterial();
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

        private void dgv_VendorMaterial_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string rowNumber = (e.RowIndex + 1).ToString();

            Rectangle headerBounds = new Rectangle(
                e.RowBounds.Left,
                e.RowBounds.Top,
                dgv_VendorMaterial.RowHeadersWidth,
                e.RowBounds.Height);

            e.Graphics.DrawString(rowNumber,
                dgv_VendorMaterial.Font,
                SystemBrushes.ControlText,
                headerBounds,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }

        private void dgv_VendorManufacturing_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string rowNumber = (e.RowIndex + 1).ToString();

            Rectangle headerBounds = new Rectangle(
                e.RowBounds.Left,
                e.RowBounds.Top,
                dgv_VendorManufacturing.RowHeadersWidth,
                e.RowBounds.Height);

            e.Graphics.DrawString(rowNumber,
                dgv_VendorManufacturing.Font,
                SystemBrushes.ControlText,
                headerBounds,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
    }
}
