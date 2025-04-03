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
            if (!frmLogin.auth.Contains("admin")) btn_Configuration.Visible = false;
            dgv_Overheads.AllowUserToAddRows= true;
            ColumnAdd();
            dgv_Overheads.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            ExcelImport excel = new ExcelImport();            
            string err = excel.LoadMasterData(cb_Classification.SelectedItem == null ? "사내 재료관리비율" : cb_Classification.SelectedItem.ToString(),dgv_Overheads);

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void btn_ExcelCreate_Click(object sender, EventArgs e)
        {
            ExcelExport excel = new ExcelExport();
            string columnName = cb_Classification.SelectedItem == null ? "사내 재료관리비율" : cb_Classification.SelectedItem.ToString();
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
                string columnName = cb_Classification.SelectedItem == null ? "사내 재료관리비율" : cb_Classification.SelectedItem.ToString();
                string err = overheads.Import("Overheads", columnName, dgv_Overheads);

                if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    CellReadOnly();
                    CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } 
            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "Overheads", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            LoadingScreen.CloseSplashScreen();
        }
        private void CellReadOnly()
        {
            List<string> UniqueColumns = new List<string> { "Valid From", "지역", "Plant", "업종", "Incoterms" };
            foreach (DataGridViewRow row in dgv_Overheads.Rows)
            {
                if (row.IsNewRow) continue;
                foreach (DataGridViewColumn col in dgv_Overheads.Columns)
                {
                    if (UniqueColumns.Contains(col.Name))
                    {
                        dgv_Overheads.Rows[row.Index].Cells[col.Name].ReadOnly = true;
                        dgv_Overheads.Rows[row.Index].Cells[col.Name].Style.BackColor = Color.LightGray;
                    }
                }
            }
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
            string columnName = cb_Classification.SelectedItem == null ? "사내 재료관리비율" : cb_Classification.SelectedItem.ToString();
            if (columnName == "사내 재료관리비율")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                PlantAdd("Plant");
                dgv_Overheads.Columns.Add("재료 관리비율", "재료 관리비율");
                dgv_Overheads.Columns["재료 관리비율"].Tag = "Siemens.TCPCM.CostType.OthermaterialcostsafterMOC";
            }
            else if (columnName == "사외 재료관리비율")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                SegmentAdd("업종");
                dgv_Overheads.Columns.Add("재료 관리비율", "재료 관리비율");
                dgv_Overheads.Columns["재료 관리비율"].Tag = "Siemens.TCPCM.CostType.OthermaterialcostsafterMOC";
            }
            else if (columnName == "판매관리비율")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                PlantAdd("Plant");
                SegmentAdd("Incoterms");
                dgv_Overheads.Columns.Add("판매관리비율", "판매관리비율");
                dgv_Overheads.Columns["판매관리비율"].Tag = "Siemens.TCPCM.CostType.OtherOverheadCosts03";
                //dgv_Overheads.Columns["판매관리비율"].DefaultCellStyle.Format = "N2";
            }
            else if (columnName == "재료 Loss율")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                PlantAdd("Plant");
                dgv_Overheads.Columns.Add("재료 Loss율", "재료 Loss율");
                dgv_Overheads.Columns["재료 Loss율"].Tag = "Siemens.TCPCM.CostType.OthermaterialcostsafterMOC2";
            }
            else if (columnName == "경제성 검토")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                PlantAdd("Plant");
                dgv_Overheads.Columns.Add("WACC", "WACC");
                dgv_Overheads.Columns["WACC"].Tag = "Siemens.TCPCM.CostType.OtherOverheadCosts09";
                dgv_Overheads.Columns.Add("법인세", "법인세");
                dgv_Overheads.Columns["법인세"].Tag = "Siemens.TCPCM.CostType.OtherOverheadCosts05";
                dgv_Overheads.Columns.Add("운전 자금", "운전 자금");
                dgv_Overheads.Columns["운전 자금"].Tag = "Siemens.TCPCM.CostType.OtherOverheadCosts12";
            }
            else if(columnName == "년간손익분석")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                PlantAdd("Plant");
                dgv_Overheads.Columns.Add("판가 A/CR율", "판가 A/CR율");
                dgv_Overheads.Columns["판가 A/CR율"].Tag = "BE05B382-8303-4827-A6ED-460454B3AF2D";//Siemens.TCPCM.CostType.Netsalesprice";
                dgv_Overheads.Columns.Add("구매 A/CR율", "구매 A/CR율");
                dgv_Overheads.Columns["구매 A/CR율"].Tag = "56154235-1F26-4450-9039-3C54E1FAD816";// Siemens.TCPCM.CostType.Materialcosts";
                dgv_Overheads.Columns.Add("직접노무비율", "직접노무비율");
                dgv_Overheads.Columns["직접노무비율"].Tag = "EACC76D4-FD68-4AF9-8E2A-6E55AC60DBC0";// Siemens.TCPCM.CostType.Directlabor";
                dgv_Overheads.Columns.Add("간접노무비율", "간접노무비율");
                dgv_Overheads.Columns["간접노무비율"].Tag = "1CE907A6-3C78-40A0-AA93-B183F4E0BA35";// Siemens.TCPCM.CostType.Residualmanufacturingoverheadcosts";
                dgv_Overheads.Columns.Add("경비율", "경비율");
                dgv_Overheads.Columns["경비율"].Tag = "A3AB6096-159C-419A-89A3-C821818D5226";//Siemens.TCPCM.CostType.Machinecosts";
                dgv_Overheads.Columns.Add("금융비율", "금융비율");
                dgv_Overheads.Columns["금융비율"].Tag = "Siemens.TCPCM.CostType.OtherOverheadCosts02";
                dgv_Overheads.Columns.Add("법인세", "법인세");
                dgv_Overheads.Columns["법인세"].Tag = "Siemens.TCPCM.CostType.OtherOverheadCosts10";
            }
            else
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                SegmentAdd("업종");
                //dgv_Overheads.Columns.Add("수량", "수량");
                dgv_Overheads.Columns.Add("간접 경비율", "간접 경비율");
                dgv_Overheads.Columns["간접 경비율"].Tag = "Siemens.TCPCM.CostType.Residualmanufacturingoverheadcosts";
                //dgv_Overheads.Columns.Add("회수율", "회수율");
                //dgv_Overheads.Columns["회수율"].Tag = "Siemens.TCPCM.CostType.OthermaterialcostsbeforeMOC";
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
            calendar.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Overheads.Columns.Add(calendar);
            dgv_Overheads.Columns[columnName].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }
        private void RegionAdd(string columnName)
        {
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.Name = combo.HeaderText = columnName;
            combo.FlatStyle = FlatStyle.Flat;
            combo.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Overheads.Columns.Add(combo);
            ((DataGridViewComboBoxColumn)dgv_Overheads.Columns[MasterData.Machine.region]).DataSource = global_DB.ListSelect("Select UniqueKey as name From BDRegions where CAST(Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%'", 0);
            dgv_Overheads.Columns[MasterData.Machine.region].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }
        private void PlantAdd(string columnName)
        {
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.Name = combo.HeaderText = columnName;
            combo.FlatStyle = FlatStyle.Flat;
            combo.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Overheads.Columns.Add(combo);
            ((DataGridViewComboBoxColumn)dgv_Overheads.Columns["Plant"]).DataSource = global_DB.ListSelect("Select UniqueKey as name From BDPlants where CAST(Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%'", 0);
            dgv_Overheads.Columns["Plant"].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }
        private void SegmentAdd(string columnName)
        {
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.Name = combo.HeaderText = columnName;
            combo.FlatStyle = FlatStyle.Flat;
            combo.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Overheads.Columns.Add(combo);
            if ((cb_Classification.SelectedItem == null ? "사내 재료관리비율" : cb_Classification.SelectedItem.ToString()) == "판매관리비율")
            {
                ((DataGridViewComboBoxColumn)dgv_Overheads.Columns["Incoterms"]).DataSource = global_DB.ListSelect("Select UniqueKey as name From BDCustomers where CAST(Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%'", 0);
                dgv_Overheads.Columns["Incoterms"].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
            }
            else
            {
                ((DataGridViewComboBoxColumn)dgv_Overheads.Columns[MasterData.Machine.segment]).DataSource = global_DB.ListSelect("SELECT DISTINCT UniqueKey as name FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'", 0);
                dgv_Overheads.Columns[MasterData.Machine.segment].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
            }
        }
        private void dgv_Overheads_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow row = dgv_Overheads.Rows[e.RowIndex];

            if(row.Cells[e.ColumnIndex].Value==null) return;

            if (dgv_Overheads.Columns[e.ColumnIndex].Name.Contains("Valid"))
            {
                row.Cells[e.ColumnIndex].Value = !DateTime.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out DateTime dt) ?
                    row.Cells[e.ColumnIndex].Value : dt.ToString("yyyy-MM-dd");
            }
            //if (new List<string>() { "율", "WACC", "법인세", "운전 자금" }.Any(key => dgv_Overheads.Columns[e.ColumnIndex].Name.Contains(key)))
            //{
            //    if (decimal.TryParse(e.Value.ToString(), out decimal value))
            //    {
            //        e.Value = value.ToString("N2");
            //        e.FormattingApplied = true;
            //    }
            //}
        }

        private void dgv_Overheads_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (new List<string>() { "율", "WACC", "법인세", "운전 자금" }.Any(key => dgv_Overheads.Columns[e.ColumnIndex].Name.Contains(key))
                && !dgv_Overheads.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Contains("%"))
            {
                string percentValue = $"{dgv_Overheads.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}";
                if (decimal.TryParse(dgv_Overheads.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString(), out decimal value))
                {
                    if (value >= 0 && value < 1)
                        percentValue = (value * 100).ToString();
                }
                if(!string.IsNullOrEmpty(percentValue))
                    dgv_Overheads.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = $"{percentValue}%";
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

            string columnName = cb_Classification.SelectedItem == null ? "사내 재료관리비율" : cb_Classification.SelectedItem.ToString();
            string inputString = "", searchQuery = "", groupQuery = "", aQuery = "", bQuery = "";
            inputString = searchButton1.text;

            if(columnName == "사내 재료관리비율")
                searchQuery = @"SELECT DateValidFrom, BDRegions.UniqueKey, BDPlants.UniqueKey, Value
                                FROM MDOverheadDetails
                                    LEFT join BDRegions ON RegionId = BDRegions.Id
                                    LEFT join BDPlants ON PlantId = BDPlants.Id
                                where OverheadHeaderID
                                IN(select id from MDOverheadHeaders where CAST(Name_LOC AS NVARCHAR(MAX)) like N'%재료 관리비%')
                                And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%[[DYA]]%'
                                And SegmentId is null";
            else if (columnName == "사외 재료관리비율")
                searchQuery = @"SELECT DateValidFrom, BDRegions.UniqueKey, BDSegments.UniqueKey, Value
                                FROM MDOverheadDetails
                                    LEFT join BDRegions ON RegionId = BDRegions.Id
                                    LEFT join BDSegments ON SegmentId = BDSegments.Id
                                where OverheadHeaderID
                                    IN(select id from MDOverheadHeaders where CAST(Name_LOC AS NVARCHAR(MAX)) like N'%재료 관리비%')
                                And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%[[DYA]]%'
                                And SegmentId is not null";
            else if(columnName == "판매관리비율")
                searchQuery = @"SELECT DateValidFrom, BDRegions.UniqueKey, BDPlants.UniqueKey, BDCustomers.UniqueKey, Value
                                FROM MDOverheadDetails
                                    LEFT join BDRegions ON RegionId = BDRegions.Id
                                    LEFT join BDPlants ON MDOverheadDetails.PlantId = BDPlants.Id
                                    LEFT join BDCustomers ON CustomerId = BDCustomers.Id
                                where OverheadHeaderID
                                    IN(select id from MDOverheadHeaders where CAST(Name_LOC AS NVARCHAR(MAX)) like N'%판관비율%')
                                And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%[[DYA]]%'";
            else if (columnName == "재료 Loss율")
                searchQuery = @"SELECT DateValidFrom, BDRegions.UniqueKey, BDPlants.UniqueKey, Value
                                FROM MDOverheadDetails
                                    LEFT join BDRegions ON RegionId = BDRegions.Id
                                    LEFT join BDPlants ON MDOverheadDetails.PlantId = BDPlants.Id
                                where OverheadHeaderID
                                    IN(select id from MDOverheadHeaders where CAST(Name_LOC AS NVARCHAR(MAX)) like N'%Loss율%')
                                And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%[[DYA]]%'";
            else if (columnName == "경제성 검토")
            {
                searchQuery = @"SELECT DateValidFrom, BDRegions.UniqueKey, BDPlants.UniqueKey,
                                    MAX(CASE WHEN OverheadHeaderID = 13 THEN Value END) AS WACC,
                                    MAX(CASE WHEN OverheadHeaderID = 14 THEN Value END) AS 법인세,
                                    MAX(CASE WHEN OverheadHeaderID = 17 THEN Value END) AS 운전자금
                                FROM MDOverheadDetails D
                                    LEFT join BDRegions ON RegionId = BDRegions.Id
                                    LEFT join BDPlants ON D.PlantId = BDPlants.Id
                                where OverheadHeaderID IN(13,14,17)";
                groupQuery = " GROUP BY DateValidFrom, BDRegions.UniqueKey, BDPlants.UniqueKey";
            }
            else if (columnName == "년간손익분석")
            {
                aQuery = @"With A AS(
	                                select DateValidFrom, BDRegions.UniqueKey As Region, BDPlants.UniqueKey As Plant,
	                                MAX(CASE WHEN IncreaseRateHeaderId = 3 THEN Value END) AS '판가 A/CR율',
	                                MAX(CASE WHEN IncreaseRateHeaderId = 5 THEN Value END) AS '구매 A/CR율',
	                                MAX(CASE WHEN IncreaseRateHeaderId = 8 THEN Value END) AS 직접노무비율,
	                                MAX(CASE WHEN IncreaseRateHeaderId = 9 THEN Value END) AS 간접노무비율,
	                                MAX(CASE WHEN IncreaseRateHeaderId = 11 THEN Value END) AS 경비율
	                                from MDIncreaseRateDetails D
		                                LEFT join BDRegions ON RegionId = BDRegions.Id
		                                LEFT join BDPlants ON D.PlantId = BDPlants.Id
	                                where IncreaseRateHeaderId IN(3,5,8,9,11)";
                bQuery = @" GROUP BY DateValidFrom, BDRegions.UniqueKey, BDPlants.UniqueKey
                                ), B AS(
	                                SELECT DateValidFrom, BDRegions.UniqueKey As Region, BDPlants.UniqueKey As Plant,
	                                MAX(CASE WHEN OverheadHeaderID = 18 THEN Value END) AS 금융비율,
	                                MAX(CASE WHEN OverheadHeaderID = 15 THEN Value END) AS 법인세
	                                FROM MDOverheadDetails D
	                                LEFT join BDRegions ON RegionId = BDRegions.Id
	                                LEFT join BDPlants ON D.PlantId = BDPlants.Id
	                                where OverheadHeaderID IN(15,18)
	                                And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%[[DYA]]%'";
                groupQuery = @" GROUP BY DateValidFrom, BDRegions.UniqueKey, BDPlants.UniqueKey
                                ) Select
                                COALESCE(A.DateValidFrom,B.DateValidFrom) AS DateValidFrom,
                                COALESCE(A.Region,B.Region) AS UniqueKey,
                                COALESCE(A.Plant,B.Plant) AS UniqueKey,
                                A.[판가 A/CR율],
                                A.[구매 A/CR율],
                                A.직접노무비율,
                                A.간접노무비율,
                                A.경비율,
                                B.금융비율,
                                B.법인세
                                From A Full Outer Join B on A.DateValidFrom = B.DateValidFrom
                                And A.Region = B.Region And A.Plant = B.Plant";
            }
            else if (columnName == "Overheads")
            {
                searchQuery = @"SELECT DateValidFrom, BDRegions.UniqueKey, BDSegments.UniqueKey,
                                    MAX(CASE WHEN OverheadHeaderID = 3 THEN Value END) AS '간접 경비율',
                                    MAX(CASE WHEN OverheadHeaderID = 4 THEN Value END) AS '일반 관리비율',
                                    MAX(CASE WHEN OverheadHeaderID = 8 THEN Value END) AS 이윤율
                                FROM MDOverheadDetails D
                                    LEFT join BDRegions ON RegionId = BDRegions.Id
                                    LEFT join BDSegments ON D.SegmentId = BDSegments.Id
                                where OverheadHeaderID IN(3,4,8)
                                And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%[[DYA]]%'";
                groupQuery = " GROUP BY DateValidFrom, BDRegions.UniqueKey, BDSegments.UniqueKey";
            }

            //입력값 검색
            if (!string.IsNullOrEmpty(inputString))
            {
                if (columnName == "사내 재료관리비율" || columnName == "재료 Loss율")
                    searchQuery += $@" AND( CAST(BDRegions.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'
                                    OR CAST(BDPlants.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%')";
                else if (columnName == "사외 재료관리비율")
                    searchQuery += $@" AND( CAST(BDRegions.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'
                                    OR CAST(BDSegments.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%')";
                else if (columnName == "판매관리비율")
                    searchQuery += $@" AND( CAST(BDRegions.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'
                                    OR CAST(BDPlants.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'
                                    OR CAST(BDCustomers.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%')";
                else if(columnName == "경제성 검토")
                {
                    searchQuery += $@" AND( CAST(BDRegions.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'
                                    OR CAST(BDPlants.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%')
                                    {groupQuery}";
                }
                else if (columnName == "년간손익분석")
                {
                    string searchText = $@"AND( BDRegions.UniqueKey like N'%{inputString}%'
                                    OR BDPlants.UniqueKey like N'%{inputString}%')";
                    searchQuery = $@"{aQuery} {searchText} {bQuery} {searchText}  {groupQuery}";
                }
                else if (columnName == "Overheads")
                {
                    searchQuery += $@" AND( CAST(BDRegions.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'
                                    OR CAST(BDSegments.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%')
                                    {groupQuery}";
                }
            }
            else
            {
                if (columnName == "경제성 검토" || columnName == "Overheads")
                    searchQuery += groupQuery;
                else if (columnName == "년간손익분석")
                    searchQuery = aQuery + bQuery + groupQuery;
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                DataTable dataTable = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.PCMDB);
                if (dataTable != null)
                    foreach (DataRow row in dataTable.Rows)
                    {
                        dgv_Overheads.Rows.Add();
                        int i = 0;
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            string result = row[col].ToString();

                            int count = dataTable.Columns.Count - (dataTable.Columns.Count - i++);
                            dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 2].Cells[count].Value = result;

                            if (col.ColumnName.Contains("Valid") || col.ColumnName.Contains("UniqueKey"))
                            {
                                dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 2].Cells[count].ReadOnly = true;
                                dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 2].Cells[count].Style.BackColor = Color.LightGray;
                            }
                        }
                    }
            }

        }

        private void searchButton1_DetailSearchButtonClick(object sender, EventArgs e)
        {
            Select select = new Select();
            select.ShowDialog();
        }

        private void dgv_Overheads_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgv_Overheads.IsCurrentCellInEditMode)
                dgv_Overheads.EndEdit();

            string columnName = dgv_Overheads.Columns[e.ColumnIndex].Name;
            bool ascending = true;

            if (dgv_Overheads.Tag is Tuple<string, bool> prevSort && prevSort.Item1 == columnName)
                ascending = !prevSort.Item2;

            dgv_Overheads.Sort(dgv_Overheads.Columns[columnName],
                ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

            dgv_Overheads.Tag = Tuple.Create(columnName, ascending);
        }
    }
}
