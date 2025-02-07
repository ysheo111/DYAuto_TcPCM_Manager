﻿using CustomControls.RJControls;
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
using System.Xml.Linq;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class frmOverheads : Form
    {
        public string className = "";
        public frmOverheads()
        {
            InitializeComponent();
        }

        private void frmExchange_Load(object sender, EventArgs e)
        {
            dgv_Overheads.AllowUserToAddRows= true;
            ColumnAdd();
            dgv_Overheads.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            ExcelImport excel = new ExcelImport();            
            string err = excel.LoadMasterData(cb_Classification.SelectedItem == null ? "재료관리비" : cb_Classification.SelectedItem.ToString(),dgv_Overheads);

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void btn_ExcelCreate_Click(object sender, EventArgs e)
        {
            ExcelExport excel = new ExcelExport();
            string columnName = cb_Classification.SelectedItem == null ? "재료관리비" : cb_Classification.SelectedItem.ToString();
            string err = excel.ExportLocationGrid(dgv_Overheads, columnName);

            if (err != null) CustomMessageBox.RJMessageBox.Show($"Export 실패하였습니다\n{err}", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("Export 완료 되었습니다.", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            ImportMethod();
        }

        private void ImportMethod()
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            try
            {
                Overheads overheads = new Overheads();
                string columnName = cb_Classification.SelectedItem == null ? "재료관리비" : cb_Classification.SelectedItem.ToString();
                string err = overheads.Import("Overheads", columnName, dgv_Overheads);

                if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            LoadingScreen.CloseSplashScreen();
        }

        private void btn_Configuration_Click(object sender, EventArgs e)
        {
            ConfigSetting config = new ConfigSetting();
            config.className = "Overheads";
            config.Show();
        }

        private void cb_Classification_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox combo = (System.Windows.Forms.ComboBox)sender;
            if (combo.SelectedIndex < 0) return;

            ColumnAdd();
            dgv_Overheads.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;            
        }
        private void ColumnAdd()
        {
            dgv_Overheads.Columns.Clear();
            string columnName = cb_Classification.SelectedItem == null ? "재료관리비" : cb_Classification.SelectedItem.ToString();
            if (columnName == "재료관리비")
            {
                ValidFromAdd("Valid From");
                dgv_Overheads.Columns.Add("지역", "지역");
                dgv_Overheads.Columns.Add("Plant", "Plant");
                dgv_Overheads.Columns.Add("업종", "업종");
                dgv_Overheads.Columns.Add("재료 관리비율", "재료 관리비율");
                dgv_Overheads.Columns["재료 관리비율"].Tag = "Siemens.TCPCM.CostType.OthermaterialcostsafterMOC";
                //"Siemens.TCPCM.CostType.Materialoverheadcosts";
                //dgv_Overheads.Columns.Add("외주 재료 관리비율", "외주 재료 관리비율");
                //dgv_Overheads.Columns["외주 재료 관리비율"].Tag = "Siemens.TCPCM.CostType.OthermaterialcostsafterMOC";
            }
            else
            {
                ValidFromAdd("Valid From");
                dgv_Overheads.Columns.Add("지역", "지역");
                //dgv_Overheads.Columns.Add("구분4", "구분4");
                dgv_Overheads.Columns.Add("업종", "업종");
                dgv_Overheads.Columns.Add("수량", "수량");
                dgv_Overheads.Columns.Add("간접 경비율", "간접 경비율");
                dgv_Overheads.Columns["간접 경비율"].Tag = "Siemens.TCPCM.CostType.Residualmanufacturingoverheadcosts";
                dgv_Overheads.Columns.Add("회수율", "회수율");
                dgv_Overheads.Columns["회수율"].Tag = "Siemens.TCPCM.CostType.OthermaterialcostsbeforeMOC";
                //dgv_Overheads.Columns.Add("수선비율", "수선비율");
                //dgv_Overheads.Columns["수선비율"].Tag = "Siemens.TCPCM.CostType.Residualmanufacturingoverheadcosts";
                dgv_Overheads.Columns.Add("일반 관리비율", "일반 관리비율");
                dgv_Overheads.Columns["일반 관리비율"].Tag = "Siemens.TCPCM.CostType.Salesandgeneraladministrationcosts";
                dgv_Overheads.Columns.Add("이윤율", "이윤율");
                dgv_Overheads.Columns["이윤율"].Tag = "Siemens.TCPCM.CostType.Profit";
            }
        }

        private void ValidFromAdd(string columnName)
        {
            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = calendar.HeaderText = columnName;
            dgv_Overheads.Columns.Add(calendar);
            dgv_Overheads.Columns[columnName].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }

        private void dgv_Overheads_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow row = dgv_Overheads.Rows[e.RowIndex];

            if(row.Cells[e.ColumnIndex].Value==null) return;

            //if (dgv_Overheads.Columns[e.ColumnIndex].Name.Contains("율") || dgv_Overheads.Columns[e.ColumnIndex].Name.Contains("scrap"))
            //{
            //    row.Cells[e.ColumnIndex].Value = Double.TryParse(row.Cells[e.ColumnIndex].Value?.ToString(), out double result) ? row.Cells[e.ColumnIndex].Value : result;
            //    if (0 < result && result < 1) row.Cells[e.ColumnIndex].Value = result * 100;
            //}
            if (dgv_Overheads.Columns[e.ColumnIndex].Name.Contains("Valid"))
            {
                row.Cells[e.ColumnIndex].Value = !DateTime.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out DateTime dt) ?
                    row.Cells[e.ColumnIndex].Value : dt.ToString("yyyy-MM-dd");
            }
        }

        private void dgv_Overheads_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgv_Overheads.Columns[e.ColumnIndex].Name == "재료 관리비율" && !dgv_Overheads.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Contains("%"))
            {
                string percent = $"{dgv_Overheads.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}%";
                dgv_Overheads.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = percent;
            }

            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_Overheads_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void searchButton1_SearchButtonClick(object sender, EventArgs e)
        {
            dgv_Overheads.Rows.Clear();

            string columnName = cb_Classification.SelectedItem == null ? "재료관리비" : cb_Classification.SelectedItem.ToString();
            string inputString = "", searchQuery = "";
            inputString = searchButton1.text;

            //전체 검색
            searchQuery = $@"SELECT DateValidFrom, BDRegions.UniqueKey, BDPlants.UniqueKey, BDSegments.UniqueKey, Value
                            as name FROM MDOverheadDetails
                            LEFT join BDRegions ON RegionId = BDRegions.Id
                            LEFT join BDSegments ON SegmentId = BDSegments.Id
                            LEFT join BDPlants ON MDOverheadDetails.PlantId = BDPlants.Id
                            where OverheadHeaderID
                            IN(select id from MDOverheadHeaders where CAST(Name_LOC AS NVARCHAR(MAX)) like N'%재료 관리비%')
                            And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%[[DYA]]%'";

            //입력값 검색
            if (!string.IsNullOrEmpty(inputString))
            {
                searchQuery = searchQuery + $@" AND( CAST(BDRegions.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'
                                                OR CAST(BDPlants.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'
                                                OR CAST(BDSegments.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%')";
            }

            DataTable dataTable = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.PCMDB);
            if (dataTable == null) return;

            foreach (DataRow row in dataTable.Rows)
            {
                dgv_Overheads.Rows.Add();
                int i = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    string result = row[col].ToString();
                    if(columnName == "재료관리비" && double.TryParse(result, out double parseResult) && parseResult < 1)
                    {
                        result = $"{(parseResult * 100).ToString()}%";
                    }
                    int count = dataTable.Columns.Count - (dataTable.Columns.Count - i++);
                    dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 2].Cells[count].Value = result;
                }
            }
        }

        private void searchButton1_DetailSearchButtonClick(object sender, EventArgs e)
        {
            Select select = new Select();
            select.ShowDialog();
        }
    }
}
