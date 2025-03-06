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

namespace TcPCM_Connect
{
    public partial class frmVendorPart : Form
    {
        public string className = "";
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

        }

        private void ColumnAdd()
        {
            dgv_Vendor.Columns.Add("품번","품번");
            dgv_Vendor.Columns.Add("품명","품명");
            dgv_Vendor.Columns.Add("차종","차종");
            dgv_Vendor.Columns.Add("업로드", "업로드");
        }

        private void dgv_Category_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //else if(dgv_Category.Columns[e.ColumnIndex].Name.Contains("임률") || dgv_Category.Columns[e.ColumnIndex].Name.Contains("경비") || dgv_Category.Columns[e.ColumnIndex].Name.Contains("단가") || dgv_Category.Columns[e.ColumnIndex].Name.Contains("탄소배출량"))
            //{
            //    row.Cells[e.ColumnIndex].Value = !double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out double number) ?
            //        row.Cells[e.ColumnIndex].Value : number.ToString("N2");
            //}
            global.CommaAdd(e, 2);
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
            //var cell = dgv_Category.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //if (cell.Value != null && decimal.TryParse(cell.Value.ToString(), out decimal number))
            //{
            //    cell.Value = number.ToString("#,##0");
            //}

            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_Category_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void searchButton1_SearchButtonClick(object sender, EventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show("검색 버튼이 클릭되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void searchButton1_DetailSearchButtonClick_1(object sender, EventArgs e)
        {
            Select select = new Select();
            select.ShowDialog();
        }

        private void searchButton1_SearchButtonClick_1(object sender, EventArgs e)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            dgv_Vendor.Rows.Clear();

            LoadingScreen.CloseSplashScreen();
        }        
    }
}
