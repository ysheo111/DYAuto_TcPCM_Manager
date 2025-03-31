using CustomControls.RJControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using TcPCM_Connect_Global;
using System.Xml.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;

namespace TcPCM_Connect
{
    public partial class frmVendorPart : Form
    {
        public string className = "";
        List<int> rowIndex = new List<int>();
        public frmVendorPart()
        {
            InitializeComponent();
        }

        private void frmExchange_Load(object sender, EventArgs e)
        {
            dgv_Vendor.AllowUserToAddRows= true;
            ColumnAdd();
            dgv_Vendor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            PartExcel import = new PartExcel();
            string err = import.BulkImport("자동");
            if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "협력사 부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "협력사 부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_ExcelCreate_Click(object sender, EventArgs e)
        {
            DataGridView dgv_Info = new DataGridView();
            DataGridView dgv_Material = new DataGridView();
            DataGridView dgv_Manufacturing = new DataGridView();
            PartExport bom = new PartExport();
            string err = null;

            string query = $@"SELECT [차종],[품번],[품명],[업체명],[납품국가],[화폐],[물류조건],[제조국가],[업체적용환율]
                                ,[업체적용환율단위],[작성일],[작성자],[업종],[일반관리비],[이윤],[재료관리비]
                                ,[R&D비],[포장&운반비],[기타],[UserID],[SOPAfter1],[SOPAfter2],[SOPAfter3],[비고]
                            FROM VendorInfo";
            dgv_Info = DTtoDGV(query, "Id");

            query = $@"SELECT [품번],[품명],[공급기준],[재질],[두께],[가로],[세로],[단위],[원재료단가],[Q'TY]
                        ,[폐기물처리비],[NET중량],[투입중량],[SCRAP단가],[비고]
                    FROM VendorMaterial";
            dgv_Material = DTtoDGV(query, "VendorInfoId");

            query = $@"SELECT VendorInfo.품번,[외주가공비],[외주개수],[외주가공명],[공정명],a.[품번],a.[업종],[작업자수]
                        ,[표준작업],[CVT],[Q'TY],[효율],[임율],[건물내용년수],[기계명],[년간가동일],[일일가동시간]
                        ,[설비취득가],[내용년수],[기계투영면적],[부대설비비율],[건축비],[수선비율],[전력용량],[전력단가]
                        ,[전력소비율],[기타설비내용년수],[간접경비율],[건물상각비],[투입비용],a.[비고]
                    FROM VendorManufacturing a
                    join VendorInfo on a.VendorInfoId = VendorInfo.Id";
            dgv_Manufacturing = DTtoDGV(query, "VendorInfoId");

            ExcelExport excel = new ExcelExport();
            err = excel.ExportLocationGrid(dgv_Info, "Vendor_Info");
            excel.ExportWorkSheet(excel.WorkBook, dgv_Manufacturing, "Vendor_Manufacturing");
            excel.ExportWorkSheet(excel.WorkBook, dgv_Material, "Vendor_Material");

            if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public DataGridView DTtoDGV(string query, string searchString)
        {
            DataGridView dgv = new DataGridView();
            foreach (DataGridViewRow row in dgv_Vendor.Rows)
            {
                if (row.IsNewRow) continue;

                string searchQuery = null;
                searchQuery = query + $" where {searchString} = {row.Cells["Id"].Value}";

                DataTable dt = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.selfDB);
                if (dgv.ColumnCount == 0)
                {
                    foreach (DataColumn dataCol in dt.Columns)
                    {
                        dgv.Columns.Add(dataCol.ColumnName, dataCol.ColumnName);
                    }
                }
                foreach (DataRow dataRow in dt.Rows)
                {
                    int rowIndex = dgv.Rows.Add();
                    for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                    {
                        dgv.Rows[rowIndex].Cells[colIndex].Value = dataRow[colIndex];
                    }
                }
            }
            return dgv;
        }

        private void ColumnAdd()
        {
            dgv_Vendor.Columns.Add("Id", "Id");
            dgv_Vendor.Columns["Id"].Visible = false;
            dgv_Vendor.Columns.Add("품번","품번");
            dgv_Vendor.Columns.Add("품명","품명");
            dgv_Vendor.Columns.Add("차종","차종");
            //dgv_Vendor.Columns.Add("업로드", "업로드");
        }

        private void dgv_Category_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //global.CommaAdd(e, 2);
            if (!dgv_Vendor.Columns[e.ColumnIndex].Name.Contains("Valid")) return;

            DataGridViewRow row = dgv_Vendor.Rows[e.RowIndex];

            if (row.Cells[e.ColumnIndex].Value == null) return;

            row.Cells[e.ColumnIndex].Value = !DateTime.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out DateTime dt) ?
                row.Cells[e.ColumnIndex].Value : dt.ToString("yyyy-MM-dd");
        }

        private void dgv_Category_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            
            if(dgv_Vendor.Columns.Contains("Designation") && dgv_Vendor.Columns[e.ColumnIndex].Name != "Designation")
            {
                if(dgv_Vendor.Columns[e.ColumnIndex].Name != "Designation-US")
                    dgv_Vendor.Rows[e.RowIndex].Cells["Designation"].Value = $"[DYA]{dgv_Vendor.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}";
            }
            else if(dgv_Vendor.Columns[e.ColumnIndex].Name == "UOM Code")
            {
                dgv_Vendor.Rows[e.RowIndex].Cells["UniqueId"].Value = dgv_Vendor.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? null : dgv_Vendor.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToLower();
            }

            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_Category_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void searchButton1_DetailSearchButtonClick_1(object sender, EventArgs e)
        {
            Select select = new Select();
            select.className = "VendorPart";
            if (select.ShowDialog() == DialogResult.OK)
            {
                SearchMethod(select.query);
            }
        }

        private void searchButton1_SearchButtonClick_1(object sender, EventArgs e)
        {
            SearchMethod(null);
        }
        private void SearchMethod(string detailQuery)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            dgv_Vendor.Rows.Clear();

            string inputString = "", searchQuery = "";
            inputString = searchButton1.text;

            //전체 검색
            searchQuery = $"SELECT Id,품번,품명,차종 FROM VendorInfo";

            //입력값 검색
            if (!string.IsNullOrEmpty(inputString))
            {
                searchQuery += $@" WHERE 품번 like '%{inputString}%'
                                or 품명 like N'%{inputString}%'
                                or 차종 like N'%{inputString}%' ";
            }
            else if (!string.IsNullOrEmpty(detailQuery))
            {
                searchQuery = $@"SELECT  V.Id,V.품번,V.품명,V.차종 FROM VendorInfo V
                                    left JOIN VendorMaterial on V.Id = VendorMaterial.VendorInfoId
                                    left JOIN VendorManufacturing on V.Id = VendorManufacturing.VendorInfoId
                                WHERE {detailQuery}";
            }

            DataTable dataTable = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.selfDB);
            if (dataTable == null) return;

            foreach (DataRow row in dataTable.Rows)
            {
                dgv_Vendor.Rows.Add();
                int i = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    string result = row[col]?.ToString();
                    int count = dataTable.Columns.Count - (dataTable.Columns.Count - i++);
                    dgv_Vendor.Rows[dgv_Vendor.Rows.Count - 2].Cells[count].Value = result;
                }
            }
            LoadingScreen.CloseSplashScreen();
        }

        private void dgv_Vendor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu = contextMenuStrip1;
                menu.Show(MousePosition.X, MousePosition.Y);

                HashSet<int> selectedRowsIndex = new HashSet<int>();
                foreach (DataGridViewCell cell in dgv_Vendor.SelectedCells)
                {
                    selectedRowsIndex.Add(cell.RowIndex);
                }
                rowIndex = selectedRowsIndex.ToList();
            }
        }

        private void dgv_Vendor_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int selectedRowCount = dgv_Vendor.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow).Distinct().Count();
            if (selectedRowCount > 1) return;

            string infoId = dgv_Vendor.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            Component.frmVendorDetail vendorDetail = new Component.frmVendorDetail();
            vendorDetail.infoId = infoId;
            vendorDetail.ShowDialog();
        }

        private void 삭제하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string result = null;
            bool notErr = true;
            foreach (int row in rowIndex)
            {
                string query = $"delete from VendorMaterial where VendorInfoId = {dgv_Vendor.Rows[row].Cells["Id"].Value}";
                result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                if(string.IsNullOrEmpty(result))
                {
                    query = $"delete from VendorManufacturing where VendorInfoId = {dgv_Vendor.Rows[row].Cells["Id"].Value}";
                    result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                    if (string.IsNullOrEmpty(result))
                    { 
                        query = $"delete from VendorInfo where id = {dgv_Vendor.Rows[row].Cells["Id"].Value}";
                        result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                        if (!string.IsNullOrEmpty(result))
                        {
                            notErr = false;
                            CustomMessageBox.RJMessageBox.Show($"{dgv_Vendor.Rows[row].Cells["품번"]}의 VendorInfo 삭제를 실패하였습니다\n{result}", "VendorPart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        notErr = false;
                        CustomMessageBox.RJMessageBox.Show($"{dgv_Vendor.Rows[row].Cells["품번"]}의 VendorManufacturing 삭제를 실패하였습니다\n{result}", "VendorPart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    notErr = false;
                    CustomMessageBox.RJMessageBox.Show($"{dgv_Vendor.Rows[row].Cells["품번"]}의 VendorMaterial 삭제를 실패하였습니다\n{result}", "VendorPart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (notErr)
            {
                searchButton1.text = null;
                SearchMethod(null);
                CustomMessageBox.RJMessageBox.Show($"삭제를 완료하였습니다", "VendorPart", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
