using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Linq;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class frmPartManufacturing : Form
    {
        public string className = "";
        public frmPartManufacturing()
        {
            InitializeComponent();
        }

        private void frmExchange_Load(object sender, EventArgs e)
        {
            dgv_PartManufacturing.AllowUserToAddRows = true;
            ColumnAdd();
            dgv_PartManufacturing.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void ColumnAdd()
        {
            dgv_PartManufacturing.Columns.Clear();

            dgv_PartManufacturing.Columns.Add("품번", "품번");
            dgv_PartManufacturing.Columns.Add("대표품명", "대표품명");
            dgv_PartManufacturing.Columns.Add("세부 공정명", "세부 공정명");
            dgv_PartManufacturing.Columns.Add("업종", "업종");
            dgv_PartManufacturing.Columns.Add("기계명", "기계명");
            dgv_PartManufacturing.Columns.Add("톤수", "톤수");
            dgv_PartManufacturing.Columns.Add("메이커", "메이커");
            dgv_PartManufacturing.Columns.Add("Number of workers", "Number of workers");
            dgv_PartManufacturing.Columns.Add("Cycle time", "Cycle time");
            dgv_PartManufacturing.Columns.Add("Cavity", "Cavity");
            dgv_PartManufacturing.Columns.Add("Q'ty", "Q'ty");
            dgv_PartManufacturing.Columns.Add("Utilization ratio", "Utilization ratio");
            dgv_PartManufacturing.Columns.Add("comment", "comment");
        }
        
        private void btn_Create_Click(object sender, EventArgs e)
        {
            ExcelImport excel = new ExcelImport();
            string err = excel.LoadMasterData("표준 공정", dgv_PartManufacturing);

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Exchange", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_ExcelCreate_Click(object sender, EventArgs e)
        {
            ExcelExport excel = new ExcelExport();
            string err = excel.ExportLocationGrid(dgv_PartManufacturing, "표준 공정");
            if (err != null) CustomMessageBox.RJMessageBox.Show($"Export 실패하였습니다\n{err}", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("Export 완료 되었습니다.", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();
            try
            {
                string err = null;
                List<string> partIds = new List<string>();
                for(int rowIndex = 0; rowIndex < dgv_PartManufacturing.Rows.Count-1; rowIndex++)
                {
                    DataGridViewRow row = dgv_PartManufacturing.Rows[rowIndex];
                    if (string.IsNullOrEmpty(row.Cells["세부 공정명"].Value?.ToString()))
                    {
                        err = "세부 공정명에는 빈 값이 있으면 안됩니다.";
                        break;
                    }
                    string searchQuery = $"SELECT id FROM [PCI].[dbo].[PartInfo] where Info = '{row.Cells["품번"].Value}' And PartName = N'{row.Cells["대표품명"].Value}'";
                    string partId = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                    //
                    if (string.IsNullOrEmpty(partId))
                    {
                        searchQuery = $"Insert into [PCI].[dbo].[PartInfo] (Info, PartName) Values (N'{row.Cells["품번"].Value}', N'{row.Cells["대표품명"].Value}' )";
                        err = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                        searchQuery = $"SELECT id FROM [PCI].[dbo].[PartInfo] where Info = '{row.Cells["품번"].Value}' And PartName = N'{row.Cells["대표품명"].Value}'";
                        partId = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                        partIds.Add(partId);
                    }
                    else if(!partIds.Contains(partId))
                    {
                        partIds.Add(partId);
                        searchQuery = $"delete from [PCI].[dbo].[Manufacturing] where PartId = '{partId}'";
                        string result = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                    }

                    object ratioValue = row.Cells["Utilization ratio"].Value;
                    string ratio = null;
                    if(ratioValue != null)
                    {
                        double ratioFlot = double.Parse(ratioValue.ToString());
                        if (ratioFlot > 1)
                        {
                            ratioFlot = ratioFlot / 100;
                        }
                        ratio = ratioFlot.ToString();
                    }
                    string itemName = string.Join("_",new[] {row.Cells["기계명"].Value, row.Cells["톤수"].Value, row.Cells["메이커"].Value }
                    .Select(obj => obj?.ToString())
                    .Where(n => !string.IsNullOrEmpty(n)));

                    string insertQuery = $@"Insert into [PCI].[dbo].[Manufacturing]
                                            (PartId,Name,Machine,Category,Cycletime,Cavity,Quantity,Utilization,NumberOfWorkers,Comment)
                                            Values ({partId},N'{row.Cells["세부 공정명"].Value}',N'{itemName}',N'{row.Cells["업종"].Value}',{global.ConvertDouble(row.Cells["Cycle time"].Value)},{global.ConvertDouble(row.Cells["Cavity"].Value)},{global.ConvertDouble(row.Cells["Q'ty"].Value)},{global.ConvertDouble(ratio)},{global.ConvertDouble(row.Cells["Number of workers"].Value)},N'{row.Cells["comment"].Value}')";
                    err = global_DB.ScalarExecute(insertQuery, (int)global_DB.connDB.PCMDB);
                    if (!string.IsNullOrEmpty(err)) break;
                }
                if (!string.IsNullOrEmpty(err)) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "표준 공정", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "표준 공정", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "표준 공정", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Thread.Sleep(100);
            LoadingScreen.CloseSplashScreen();
        }

        private void dgv_PartManufacturing_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_PartManufacturing_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void searchButton1_DetailSearchButtonClick(object sender, EventArgs e)
        {
            Select select = new Select();
            select.ShowDialog();
        }

        private void searchButton1_SearchButtonClick(object sender, EventArgs e)
        {
            dgv_PartManufacturing.Rows.Clear();

            string inputString = "", searchQuery = "";
            inputString = searchButton1.text;

            //전체 검색
            searchQuery = @"select Info,PartName,Name,Category,Machine,Cycletime,Cavity,Quantity,Utilization,NumberOfWorkers,Comment from [PCI].[dbo].[Manufacturing]
                                join[PCI].[dbo].[PartInfo] on[PCI].[dbo].[PartInfo].Id = [PCI].[dbo].[Manufacturing].PartId";

            //입력값 검색
            if (!string.IsNullOrEmpty(inputString))
            {
                searchQuery = searchQuery + $@" where Machine like N'%{inputString}%'
                                          OR Name like N'%{inputString}%'
                                          OR Info like N'%{inputString}%'";
            }

            DataTable dataTable = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.PCMDB);
            if (dataTable == null) return;

            foreach (DataRow row in dataTable.Rows)
            {
                dgv_PartManufacturing.Rows.Add();
                int i = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    string result = row[col].ToString();
                    int count = dataTable.Columns.Count - (dataTable.Columns.Count - i++);
                    if (col.ColumnName == "Machine")
                    {
                        string[] splitResult = result.Split('_');
                        if(splitResult.Length == 2)
                        {
                            dgv_PartManufacturing.Rows[dgv_PartManufacturing.Rows.Count - 2].Cells["기계명"].Value = splitResult[0].Trim();
                            dgv_PartManufacturing.Rows[dgv_PartManufacturing.Rows.Count - 2].Cells["메이커"].Value = splitResult[1].Trim();
                            i += 2;
                        }
                        else if(splitResult.Length == 3)
                        {
                            dgv_PartManufacturing.Rows[dgv_PartManufacturing.Rows.Count - 2].Cells["기계명"].Value = splitResult[0].Trim();
                            dgv_PartManufacturing.Rows[dgv_PartManufacturing.Rows.Count - 2].Cells["톤수"].Value = splitResult[1].Trim();
                            dgv_PartManufacturing.Rows[dgv_PartManufacturing.Rows.Count - 2].Cells["메이커"].Value = splitResult[2].Trim();
                            i += 2;
                        }
                    }
                    else
                        dgv_PartManufacturing.Rows[dgv_PartManufacturing.Rows.Count - 2].Cells[count].Value = result.Trim();
                }
            }
            dgv_PartManufacturing.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
