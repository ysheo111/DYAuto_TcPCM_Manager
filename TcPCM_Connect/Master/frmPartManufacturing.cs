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
        List<DataGridViewRow> selectItem = new List<DataGridViewRow>();
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

            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = calendar.HeaderText = "Valid From";
            dgv_PartManufacturing.Columns.Add(calendar);
            dgv_PartManufacturing.Columns["Valid From"].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
            //dgv_PartManufacturing.Columns.Add("Valid From", "Valid From");

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
                    string searchQuery = $@"SELECT id FROM [PCI].[dbo].[PartInfo]
                        where Info = '{row.Cells["품번"].Value}'
                        And PartName = N'{row.Cells["대표품명"].Value}'
                        And ValidFrom = N'{row.Cells["Valid From"].Value}'";
                    string partId = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                    //
                    if (string.IsNullOrEmpty(partId))
                    {
                        searchQuery = $"Insert into [PCI].[dbo].[PartInfo] (Info, PartName, ValidFrom) Values (N'{row.Cells["품번"].Value}', N'{row.Cells["대표품명"].Value}', N'{row.Cells["Valid From"].Value}' )";
                        err = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                        searchQuery = $"SELECT id FROM [PCI].[dbo].[PartInfo] where Info = '{row.Cells["품번"].Value}' And PartName = N'{row.Cells["대표품명"].Value}' And ValidFrom = N'{row.Cells["Valid From"].Value}'";
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
                    string name = row.Cells["세부 공정명"].Value?.ToString().Trim();
                    string itemName = string.Join("_",new[] {row.Cells["기계명"].Value, row.Cells["톤수"].Value, row.Cells["메이커"].Value }.Select(obj => obj?.ToString()).Where(n => !string.IsNullOrEmpty(n))).Trim();
                    string sagName = row.Cells["업종"].Value?.ToString().Trim();
                    string comment = row.Cells["comment"].Value?.ToString().Trim();

                    string insertQuery = $@"Insert into [PCI].[dbo].[Manufacturing]
                                            (PartId,Name,Machine,Category,Cycletime,Cavity,Quantity,Utilization,NumberOfWorkers,Comment)
                                            Values ({partId},N'{name}',N'{itemName}',N'{sagName}',{global.ConvertDouble(row.Cells["Cycle time"].Value)},{global.ConvertDouble(row.Cells["Cavity"].Value)},{global.ConvertDouble(row.Cells["Q'ty"].Value)},{global.ConvertDouble(ratio)},{global.ConvertDouble(row.Cells["Number of workers"].Value)},N'{comment}')";
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
        private void click()
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();
            try
            {
                string err = null;
                List<string> partIds = new List<string>();
                
                HashSet<(string Info, string Name, string ValidFrom)> partKeys = new HashSet<(string Info, string Name, string ValidFrom)>();
                for (int rowIndex = 0; rowIndex < dgv_PartManufacturing.Rows.Count - 1; rowIndex++)
                {
                    DataGridViewRow row = dgv_PartManufacturing.Rows[rowIndex];
                    partKeys.Add((
                            row.Cells["품번"].Value?.ToString().Trim(),
                            row.Cells["대표품명"].Value?.ToString().Trim(),
                            row.Cells["Valid From"].Value?.ToString()
                        ));
                }
                string partInfoSelect = string.Join(" or ", partKeys.Select(key =>
                $"(Info = '{key.Info}' AND PartName = N'{key.Name}' AND ValidFrom = '{key.ValidFrom}')"));
                string partInfoQuery = $"SELECT id FROM [PCI].[dbo].[PartInfo] where {partInfoSelect}";
                DataTable partInfoTable = global_DB.MutiSelect(partInfoQuery, (int)global_DB.connDB.PCMDB);

                for (int rowIndex = 0; rowIndex < dgv_PartManufacturing.Rows.Count - 1; rowIndex++)
                {
                    DataGridViewRow row = dgv_PartManufacturing.Rows[rowIndex];
                    if (string.IsNullOrEmpty(row.Cells["세부 공정명"].Value?.ToString()))
                    {
                        err = "세부 공정명에는 빈 값이 있으면 안됩니다.";
                        break;
                    }
                    string searchQuery = $@"SELECT id FROM [PCI].[dbo].[PartInfo]
                        where Info = '{row.Cells["품번"].Value}'
                        And PartName = N'{row.Cells["대표품명"].Value}'
                        And ValidFrom = N'{row.Cells["Valid From"].Value}'";
                    string partId = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                    //
                    if (string.IsNullOrEmpty(partId))
                    {
                        searchQuery = $"Insert into [PCI].[dbo].[PartInfo] (Info, PartName, ValidFrom) Values (N'{row.Cells["품번"].Value}', N'{row.Cells["대표품명"].Value}', N'{row.Cells["Valid From"].Value}' )";
                        err = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                        searchQuery = $"SELECT id FROM [PCI].[dbo].[PartInfo] where Info = '{row.Cells["품번"].Value}' And PartName = N'{row.Cells["대표품명"].Value}' And ValidFrom = N'{row.Cells["Valid From"].Value}'";
                        partId = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                        partIds.Add(partId);
                    }
                    else if (!partIds.Contains(partId))
                    {
                        partIds.Add(partId);
                        searchQuery = $"delete from [PCI].[dbo].[Manufacturing] where PartId = '{partId}'";
                        string result = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                    }

                    object ratioValue = row.Cells["Utilization ratio"].Value;
                    string ratio = null;
                    if (ratioValue != null)
                    {
                        double ratioFlot = double.Parse(ratioValue.ToString());
                        if (ratioFlot > 1)
                        {
                            ratioFlot = ratioFlot / 100;
                        }
                        ratio = ratioFlot.ToString();
                    }
                    string name = row.Cells["세부 공정명"].Value?.ToString().Trim();
                    string itemName = string.Join("_", new[] { row.Cells["기계명"].Value, row.Cells["톤수"].Value, row.Cells["메이커"].Value }.Select(obj => obj?.ToString()).Where(n => !string.IsNullOrEmpty(n))).Trim();
                    string sagName = row.Cells["업종"].Value?.ToString().Trim();
                    string comment = row.Cells["comment"].Value?.ToString().Trim();

                    string insertQuery = $@"Insert into [PCI].[dbo].[Manufacturing]
                                            (PartId,Name,Machine,Category,Cycletime,Cavity,Quantity,Utilization,NumberOfWorkers,Comment)
                                            Values ({partId},N'{name}',N'{itemName}',N'{sagName}',{global.ConvertDouble(row.Cells["Cycle time"].Value)},{global.ConvertDouble(row.Cells["Cavity"].Value)},{global.ConvertDouble(row.Cells["Q'ty"].Value)},{global.ConvertDouble(ratio)},{global.ConvertDouble(row.Cells["Number of workers"].Value)},N'{comment}')";
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
            select.className = "표준 공정 라이브러리";
            if (select.ShowDialog() == DialogResult.OK)
            {
                SearchMethod(select.query);
            }
        }

        private void searchButton1_SearchButtonClick(object sender, EventArgs e)
        {
            SearchMethod(null);
        }
        private void SearchMethod(string detailQuery)
        {
            dgv_PartManufacturing.Rows.Clear();

            string inputString = "", searchQuery = "";
            inputString = searchButton1.text;

            //전체 검색
            searchQuery = @"select ValidFrom,Info,PartName,Name,Category,Machine,Cycletime,Cavity,Quantity,Utilization,NumberOfWorkers,Comment from [PCI].[dbo].[Manufacturing]
                                join[PCI].[dbo].[PartInfo] on[PCI].[dbo].[PartInfo].Id = [PCI].[dbo].[Manufacturing].PartId";

            //입력값 검색
            if (!string.IsNullOrEmpty(inputString) && string.IsNullOrEmpty(detailQuery))
            {
                searchQuery = searchQuery + $@" where Info like N'%{inputString}%'
                                          OR PartName like N'%{inputString}%'
                                          OR Name like N'%{inputString}%'
                                          OR Category like N'%{inputString}%'
                                          OR Machine like N'%{inputString}%'";
            }
            else if(!string.IsNullOrEmpty(detailQuery))
            {
                searchQuery += $" where {detailQuery}";
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
                        if (splitResult.Length == 2)
                        {
                            dgv_PartManufacturing.Rows[dgv_PartManufacturing.Rows.Count - 2].Cells["기계명"].Value = splitResult[0].Trim();
                            dgv_PartManufacturing.Rows[dgv_PartManufacturing.Rows.Count - 2].Cells["메이커"].Value = splitResult[1].Trim();
                            i += 2;
                        }
                        else if (splitResult.Length == 3)
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

        private void dgv_PartManufacturing_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu = contextMenuStrip1;

                menu.Show(e.Location);
            }
            List<DataGridViewRow> item = new List<DataGridViewRow>();
            if (e.Button == MouseButtons.Right)
            {
                foreach (DataGridViewCell cell in dgv_PartManufacturing.SelectedCells)
                {
                    DataGridViewRow selectRow = dgv_PartManufacturing.Rows[cell.RowIndex];
                    item.Add(selectRow);
                }
                SelectItems(item);
            }
        }
        private void SelectItems(List<DataGridViewRow> item)
        {
            selectItem = item;
            contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
        }
        private void 전체품번삭제하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in selectItem)
            {
                string query = $@"delete from Manufacturing
                                where Info = '{item.Cells["품번"]}' and PartName = N'{item.Cells["대표품명"]}' ";
                string err = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                if(err == null)
                {
                    query = $@"delete from [PCI].[dbo].[PartInfo]
                            where Info = '{item.Cells["품번"]}' and PartName = N'{item.Cells["대표품명"]}' ";
                    err = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                    if (err == null)
                        CustomMessageBox.RJMessageBox.Show("삭제 완료 되었습니다.", "표준 공정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        CustomMessageBox.RJMessageBox.Show($"삭제 실패하였습니다\n{err}", "표준 공정", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    CustomMessageBox.RJMessageBox.Show($"삭제 실패하였습니다\n{err}", "표준 공정", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void 선택품목삭제하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in selectItem)
            {
                string itemName = string.Join("_", new[] { item.Cells["기계명"].Value, item.Cells["톤수"].Value, item.Cells["메이커"].Value }.Select(obj => obj?.ToString()).Where(n => !string.IsNullOrEmpty(n))).Trim();
                string query = $@"delete from Manufacturing
                            where Info = '{item.Cells["품번"]}' and PartName = N'{item.Cells["대표품명"]}'
                            and Name = N'{item.Cells["세부 공정명"]}' and Category = N'{item.Cells["업종"]}'
                            and Machine = N'{itemName}' 
                            and Cycletime = '{item.Cells["Cycle time"]}' and Cavity = '{item.Cells["Cavity"]}'
                            and Quantity = '{item.Cells["Q'ty"]}' and Utilization = '{item.Cells["Utilization ratio"]}'
                            and NumberOfWorkers = '{item.Cells["Number of workers"]} and comment = '{item.Cells["comment"]}";
                string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                //query = $@"delete from [PCI].[dbo].[PartInfo]
                //        where Info = '{item.Cells["품번"]}' and PartName = '{item.Cells["대표품명"]}' ";
                //result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
            }
        }
    }
}
