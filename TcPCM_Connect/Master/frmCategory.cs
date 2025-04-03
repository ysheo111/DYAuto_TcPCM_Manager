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
using System.Text.RegularExpressions;

namespace TcPCM_Connect
{
    public partial class frmCategory : Form
    {
        public string className = "";
        public frmCategory()
        {
            InitializeComponent();
        }

        private void frmExchange_Load(object sender, EventArgs e)
        {
            searchButton1.detailSearchButton.Visible = false;
            if (!frmLogin.auth.Contains("admin")) btn_Configuration.Visible = false;

            dgv_Category.AllowUserToAddRows= true;
            ColumnAdd();
            dgv_Category.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            ExcelImport excel = new ExcelImport();
            string err = excel.LoadMasterData(cb_Classification.SelectedItem == null ? "Cost factor" : cb_Classification.SelectedItem.ToString(),dgv_Category);

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_ExcelCreate_Click(object sender, EventArgs e)
        {
            ExcelExport excel = new ExcelExport();
            string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
            string err = excel.ExportLocationGrid(dgv_Category, columnName);
            
            if (err != null) CustomMessageBox.RJMessageBox.Show($"Export 실패하였습니다\n{err}", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("Export 완료 되었습니다.", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                CostFactor costFactor = new CostFactor();
                string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
                string err = null;
                if (columnName == "지역")
                {
                    err = costFactor.Import("Category", columnName.Replace("4", ""), dgv_Category);
                    if (err == null)
                    {
                        err = costFactor.Import("Category", "Plant2", dgv_Category);
                        if (err == null)
                        {
                            string searchQuery = "SELECT DISTINCT UniqueKey as name FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'";
                            List<string> segmantList = global_DB.ListSelect(searchQuery, (int)global_DB.connDB.PCMDB);
                            err = costFactor.SegmantImport("Category", "업종", dgv_Category, segmantList);
                        }
                    }
                }
                if(columnName == "Plant")
                {
                    err = costFactor.Import("Category", "Plant", dgv_Category);
                    if (err == null)
                    {
                        string searchQuery = "SELECT DISTINCT UniqueKey as name FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'";
                        List<string> segmantList = global_DB.ListSelect(searchQuery, (int)global_DB.connDB.PCMDB);
                        err = costFactor.SegmantImport("Category", "업종", dgv_Category, segmantList);
                    }
                }
                else if (columnName == "업종")
                {
                    string searchQuery = $"SELECT UniqueKey as name FROM BDPlants where CAST(Name_LOC AS NVARCHAR(MAX)) Like '%[[DYA]]%'";
                    List<string> regionList = global_DB.ListSelect(searchQuery, (int)global_DB.connDB.PCMDB);
                    foreach (string region in regionList)
                    {
                        foreach (DataGridViewRow row in dgv_Category.Rows)
                        {
                            if (string.IsNullOrEmpty(row.Cells["업종"].Value?.ToString()))
                                break;
                            row.Cells["Plant"].Value = region;
                        }
                        err = costFactor.Import("Category", columnName.Replace("4", ""), dgv_Category);
                        if (err != null)
                        {
                            CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            LoadingScreen.CloseSplashScreen();
                            return;
                        }
                    }
                }
                else if(columnName == "전력단가")
                {
                    err = costFactor.Import("Category", columnName.Replace("4", ""), dgv_Category);
                    //if (err == null)
                    //    err = costFactor.Import("Category", "탄소배출량", dgv_Category);
                }
                else if (columnName == "사외 임률")
                {
                    err = costFactor.Import("Category", $"직접임률2", dgv_Category);
                   
                }
                else if (columnName == "사내 임률")
                {
                    err = costFactor.Import("Category", $"직접임률", dgv_Category);
                    if (err == null)
                    {
                        err = costFactor.Import("Category", $"간접임률", dgv_Category);
                    }
                }
                else
                    err = costFactor.Import("Category", columnName.Replace("4", ""), dgv_Category);

                if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CellReadOnly();
                } 
            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            LoadingScreen.CloseSplashScreen();
        }

        private void btn_Configuration_Click(object sender, EventArgs e)
        {
            ConfigSetting config = new ConfigSetting();
            config.className = "Category";
            config.Show();
        }

        private void cb_Classification_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox combo = (System.Windows.Forms.ComboBox)sender;
            if (combo.SelectedIndex < 0) return;

            ColumnAdd();
            dgv_Category.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;            
        }
        private void ColumnAdd()
        {
            dgv_Category.Columns.Clear();
            string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();

            if (columnName == "작업일수")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                SegmentAdd("업종");
                dgv_Category.Columns.Add("연간 작업 일수", "연간 작업 일수");
                dgv_Category.Columns["연간 작업 일수"].Tag = "Siemens.TCPCM.MasterData.CostFactor.Common.ProductionWeeksPerYear";
                dgv_Category.Columns.Add("Shift 당 작업 시간", "Shift 당 작업 시간");
                dgv_Category.Columns["Shift 당 작업 시간"].Tag = "Siemens.TCPCM.MasterData.CostFactor.Common.ProductionHoursPerShift";
                dgv_Category.Columns.Add("Shift", "Shift");
                dgv_Category.Columns["Shift"].Tag = "Siemens.TCPCM.MasterData.CostFactor.Common.ShiftsPerProductionDay";
            }
            else if (columnName == "공간 생산 비용")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                SegmentAdd("업종"); 
                CurrencyAdd("통화");
                dgv_Category.Columns.Add("부대설비비율", "부대설비비율");
                dgv_Category.Columns.Add("건축비", "건축비");
                dgv_Category.Columns.Add("내용년수", "내용년수");
            }
            else if (columnName == "전력단가")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                CurrencyAdd("통화");
                //PlantAdd("Plant");
                dgv_Category.Columns.Add("전력단가", "전력단가");
                dgv_Category.Columns["전력단가"].DefaultCellStyle.Format = "N2";
                dgv_Category.Columns.Add("탄소배출량", "탄소배출량");
                dgv_Category.Columns["탄소배출량"].DefaultCellStyle.Format = "N2";
                dgv_Category.Columns["탄소배출량"].ReadOnly = true;
                carbon(0);
            }
            else if (columnName == "사내 임률")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                PlantAdd("Plant");
                CurrencyAdd("통화");
                dgv_Category.Columns.Add("간접임률", "간접임률");
                dgv_Category.Columns["간접임률"].DefaultCellStyle.Format = "N2";
                dgv_Category.Columns.Add("직접임률", "직접임률");
                dgv_Category.Columns["직접임률"].DefaultCellStyle.Format = "N2";
            }
            else if (columnName == "사외 임률")
            {
                ValidFromAdd("Valid From");
                RegionAdd("지역");
                //PlantAdd("Plant");
                SegmentAdd("업종");
                CurrencyAdd("통화");
                //dgv_Category.Columns["Plant"].Visible = false;
                dgv_Category.Columns.Add("직접임률", "직접임률");
                dgv_Category.Columns["직접임률"].DefaultCellStyle.Format = "N2";
            }
            else if (columnName == "업종")
            {
                dgv_Category.Columns.Add("업종", "업종");
                dgv_Category.Columns.Add("Designation", "Designation");
                dgv_Category.Columns["Designation"].Visible = false;
                dgv_Category.Columns.Add("Plant", "Plant");
                dgv_Category.Columns["Plant"].Visible = false;
            }
            //else if (columnName == "단위")
            //{
            //    dgv_Category.Columns.Add("UOM Code", "UOM Code");
            //    dgv_Category.Columns.Add("UOM 명", "UOM 명");
            //    dgv_Category.Columns.Add("UniqueId", "UniqueId");
            //    dgv_Category.Columns["UniqueId"].Visible = false;
            //}
            else if (columnName == "지역")
            {
                dgv_Category.Columns.Add("지역", "지역");
                //dgv_Category.Columns.Add("Designation-US", "지역 영문명");
                dgv_Category.Columns.Add("지역 영문명", "지역 영문명");
                dgv_Category.Columns.Add("Designation", "Designation");
                dgv_Category.Columns["Designation"].Visible = false;
            }
            else if(columnName == "Plant")
            {
                RegionAdd("지역");
                dgv_Category.Columns.Add("Plant", "Plant");
            }
            else
                dgv_Category.Columns.Add(columnName, columnName);

            if (dgv_Category.Columns.Count == 1)
            {
                dgv_Category.Columns.Add("Designation", "Designation");
                dgv_Category.Columns["Designation"].Visible = false;
            }
        }
        private void CurrencyAdd(string columnName)
        {
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.Name = combo.HeaderText = columnName;
            combo.FlatStyle = FlatStyle.Flat;
            combo.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Category.Columns.Add(combo);
            ((DataGridViewComboBoxColumn)dgv_Category.Columns[MasterData.Machine.currency]).DataSource = global_DB.ListSelect("Select IsoCode as name From Currencies", 0);
            dgv_Category.Columns[MasterData.Machine.currency].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }
        private void RegionAdd(string columnName)
        {
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.Name = combo.HeaderText = columnName;
            combo.FlatStyle = FlatStyle.Flat;
            combo.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Category.Columns.Add(combo);
            ((DataGridViewComboBoxColumn)dgv_Category.Columns[MasterData.Machine.region]).DataSource = global_DB.ListSelect("Select UniqueKey as name From BDRegions where CAST(Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%'", 0);
            dgv_Category.Columns[MasterData.Machine.region].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }
        private void PlantAdd(string columnName)
        {
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.Name = combo.HeaderText = columnName;
            combo.FlatStyle = FlatStyle.Flat;
            combo.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Category.Columns.Add(combo);
            ((DataGridViewComboBoxColumn)dgv_Category.Columns["Plant"]).DataSource = global_DB.ListSelect("Select UniqueKey as name From BDPlants where CAST(Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%'", 0);
            dgv_Category.Columns["Plant"].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }
        private void SegmentAdd(string columnName)
        {
            DataGridViewComboBoxColumn segCombo = new DataGridViewComboBoxColumn();
            segCombo.Name = segCombo.HeaderText = columnName;
            segCombo.FlatStyle = FlatStyle.Flat;
            segCombo.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Category.Columns.Add(segCombo);
            ((DataGridViewComboBoxColumn)dgv_Category.Columns[MasterData.Machine.segment]).DataSource = global_DB.ListSelect("SELECT DISTINCT UniqueKey as name FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'", 0);
            dgv_Category.Columns[MasterData.Machine.segment].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }
        private void ValidFromAdd(string columnName)
        {
            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = calendar.HeaderText = columnName;
            calendar.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Category.Columns.Add(calendar);
            dgv_Category.Columns[columnName].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }

        private void dgv_Category_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //else if(dgv_Category.Columns[e.ColumnIndex].Name.Contains("임률") || dgv_Category.Columns[e.ColumnIndex].Name.Contains("경비") || dgv_Category.Columns[e.ColumnIndex].Name.Contains("단가") || dgv_Category.Columns[e.ColumnIndex].Name.Contains("탄소배출량"))
            //{
            //    row.Cells[e.ColumnIndex].Value = !double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out double number) ?
            //        row.Cells[e.ColumnIndex].Value : number.ToString("N2");
            //}
            global.CommaAdd(e, 2);
            if (!dgv_Category.Columns[e.ColumnIndex].Name.Contains("Valid")) return;

            DataGridViewRow row = dgv_Category.Rows[e.RowIndex];

            if (row.Cells[e.ColumnIndex].Value == null) return;

            row.Cells[e.ColumnIndex].Value = !DateTime.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out DateTime dt) ?
                row.Cells[e.ColumnIndex].Value : dt.ToString("yyyy-MM-dd");
        }

        private void dgv_Category_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {            
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
            if (dgv_Category.Columns.Contains("Designation") && dgv_Category.Columns[e.ColumnIndex].Name != "Designation")
            {
                //if(dgv_Category.Columns[e.ColumnIndex].Name != "Designation-US")               
                if (columnName == "업종")
                {
                    dgv_Category.Rows[e.RowIndex].Cells["Designation"].Value = $"[DYA]{dgv_Category.Rows[e.RowIndex].Cells["업종"].Value}";
                }
                else
                    dgv_Category.Rows[e.RowIndex].Cells["Designation"].Value = $"[DYA]{dgv_Category.Rows[e.RowIndex].Cells["지역"].Value}";
            }
            else if(dgv_Category.Columns[e.ColumnIndex].Name == "UOM Code")
            {
                dgv_Category.Rows[e.RowIndex].Cells["UniqueId"].Value = dgv_Category.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? null : dgv_Category.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToLower();
            }
            else if(dgv_Category.Columns[e.ColumnIndex].Name == "Valid From" && columnName == "전력단가")
            {
                DateTime cell;
                if (dgv_Category.Rows[e.RowIndex].Cells[e.ColumnIndex].Value is DateTime)
                {
                    cell = (DateTime)dgv_Category.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                }
                else return;
                string query = $@"SELECT top 1 d.Value*3600*1000 as name, d.*
                          FROM [TcPCM2312_Patch3].[dbo].[MDCostFactorHeaders] as h
                          join [TcPCM2312_Patch3].[dbo].[MDCostFactorDetails] as d on h.ID = d.CostFactorHeaderId
                          where h.UniqueKey = 'Siemens.TCPCM.MasterData.CostFactor.Carbon.RateForEnergy.Electricity' 
                                and RegionId = (select Id from BDRegions where UniqueKey = 'Siemens.TCPCM.Region.Common.SouthKorea')
		                        and DateValidFrom <= '{cell.ToString("yyyy-MM-dd")}'
                        union all

		                  SELECT top 1 d.Value*3600*1000 as name, d.*
                          FROM [TcPCM2312_Patch3].[dbo].[MDCostFactorHeaders] as h
                          join [TcPCM2312_Patch3].[dbo].[MDCostFactorDetails] as d on h.ID = d.CostFactorHeaderId
                          where h.UniqueKey = 'Siemens.TCPCM.MasterData.CostFactor.Carbon.RateForEnergy.Electricity' 
                                and RegionId = (select Id from BDRegions where UniqueKey = 'Siemens.TCPCM.Region.Common.SouthKorea')
		                        order by DateValidFrom desc";

                List<string> value = global_DB.ListSelect(query, (int)global_DB.connDB.PCMDB);

                if(value.Count != 0) dgv_Category.Rows[e.RowIndex].Cells["탄소배출량"].Value = value[0];
            }
            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_Category_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            dgv_Category.Rows.Clear();

            string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
            string inputString = "", searchQuery = "", Aselect = "", Bselect = "", FullJoin = "";
            inputString = searchButton1.text;

            //전체 검색
            if (columnName == "지역")
                searchQuery = $"SELECT UniqueKey,Name_LOC FROM BDRegions where CAST(Name_LOC AS NVARCHAR(MAX)) Like '%[[DYA]]%'";
            else if (columnName == "Plant")
                searchQuery = @"select BDRegions.UniqueKey,BDPlants.UniqueKey from BDPlants
                                    join BDRegions on BDRegions.id = RegionId
                                where CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%'";
            else if (columnName == "업종")
                searchQuery = "SELECT DISTINCT UniqueKey FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'";
            else if (columnName == "단위")
                searchQuery = $"SELECT DisplayName_LOC,FullName_LOC FROM Units";
            else if (columnName == "전력단가")
            {
                Aselect = $@"With A AS(
	                        SELECT DateValidFrom,BDRegions.UniqueKey As region,Currencies.IsoCode,BDPlants.UniqueKey As plant,Value * 3600 * 1000 as value
                            from MDCostFactorDetails
	                        JOIN BDRegions ON RegionId = BDRegions.Id
	                        LEFT JOIN Currencies ON CurrencyId = Currencies.Id
	                        LEFT JOIN BDPlants ON PlantId = BDPlants.Id
	                        where CostFactorHeaderId in (
		                        select Id from MDCostFactorHeaders where UniqueKey = 'Siemens.TCPCM.MasterData.CostFactor.Common.ElectricityPrice' )
	                        And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%'";
                Bselect = $@"), B AS( SELECT top 1 d.Value * 3600 * 1000 as name, d.*
                              FROM[TcPCM2312_Patch3].[dbo].[MDCostFactorHeaders] as h
                          join[TcPCM2312_Patch3].[dbo].[MDCostFactorDetails] as d on h.ID = d.CostFactorHeaderId
                          JOIN BDRegions ON RegionId = BDRegions.Id
                          where h.UniqueKey = 'Siemens.TCPCM.MasterData.CostFactor.Carbon.RateForEnergy.Electricity'
                                and RegionId = (select Id from BDRegions where UniqueKey = 'Siemens.TCPCM.Region.Common.SouthKorea')";

                FullJoin = $@") SELECT A.DateValidFrom As 'Valid From',
	                                    A.region As 'Region',
                                        IsoCode As '통화',
                                        A.Value As '전력단가',
                                        b1.name As '탄소배출량'
                                        From A
                                        OUTER APPLY (
                                            SELECT TOP 1 name, DateValidFrom
                                            FROM B
                                            WHERE B.DateValidFrom < A.DateValidFrom
                                            ORDER BY B.DateValidFrom DESC
                                        ) AS B1";

                searchQuery = Aselect + Bselect +  FullJoin;
            }
            else if (columnName == "사외 임률")
            {
                searchQuery = $@"With A as(
	                        select DateValidFrom,RegionId,CurrencyId,Value*3600 as value,PlantId,SegmentId
		                        from MDCostFactorDetails where CostFactorHeaderId in
		                        (select Id from MDCostFactorHeaders where UniqueKey = N'직접노무비') 
                            ),B as(
	                        select DateValidFrom,RegionId,CurrencyId,Value*3600 as value,PlantId,SegmentId
		                        from MDCostFactorDetails where CostFactorHeaderId in
		                        ( select Id from MDCostFactorHeaders where UniqueKey = N'간접노무비' )
                            ),C as(
	                        select DateValidFrom,RegionId,PlantId,CurrencyId,MachineHourlyRate from MDAssetDetails
		                        where AssetHeaderId in (select Id from MDAssetHeaders where CAST(Name_LOC AS NVARCHAR(MAX)) like N'%경비%') )
                            select
                            COALESCE(A.DateValidFrom, B.DateValidFrom) As DateValidFrom,
                            BDRegions.UniqueKey As Region,
                            BDSegments.UniqueKey As Segment,
                            Currencies.IsoCode,
                            A.value As N'직접임률'
                            From A
                            Full outer join B on A.DateValidFrom = B.DateValidFrom And A.RegionId = B.RegionId And A.CurrencyId = B.CurrencyId And A.PlantId = B.PlantId
                            Full outer join C on COALESCE(A.DateValidFrom, B.DateValidFrom) = C.DateValidFrom And COALESCE(A.CurrencyId, B.CurrencyId) = C.CurrencyId And COALESCE(A.RegionId, B.RegionId) = C.RegionId And COALESCE(A.PlantId, B.PlantId) = C.PlantId
                            join BDRegions ON COALESCE(A.RegionId, B.RegionId) = BDRegions.Id
                            join BDPlants ON COALESCE(A.PlantId, B.PlantId) = BDPlants.Id
                            join Currencies ON COALESCE(A.CurrencyId, B.CurrencyId) = Currencies.Id
                            join BDSegments ON COALESCE(A.SegmentId, B.SegmentId) = BDSegments.Id
                            where BDPlants.UniqueKey = BDRegions.UniqueKey  and (BDRegions.UniqueKey like N'%{inputString}%' or BDSegments.UniqueKey like N'%{inputString}%')";
            }
            else if (columnName == "사내 임률")
            {
                searchQuery = $@"With A as(
	                        select DateValidFrom,RegionId,CurrencyId,Value*3600 as value,PlantId,SegmentId
		                        from MDCostFactorDetails where CostFactorHeaderId in
		                        (select Id from MDCostFactorHeaders where UniqueKey = N'직접노무비') 
                            ),B as(
	                        select DateValidFrom,RegionId,CurrencyId,Value*3600 as value,PlantId,SegmentId
		                        from MDCostFactorDetails where CostFactorHeaderId in
		                        ( select Id from MDCostFactorHeaders where UniqueKey = N'간접노무비' )
                            ),C as(
	                        select DateValidFrom,RegionId,PlantId,CurrencyId,MachineHourlyRate from MDAssetDetails
		                        where AssetHeaderId in (select Id from MDAssetHeaders where CAST(Name_LOC AS NVARCHAR(MAX)) like N'%경비%') )
                            select
                            COALESCE(A.DateValidFrom, B.DateValidFrom) As DateValidFrom,
                            BDRegions.UniqueKey As Region,
                            BDPlants.UniqueKey As Plant,
                            Currencies.IsoCode,
                            B.value As N'간접임률',
                            A.value As N'직접임률'
                            From A
                            Full outer join B on A.DateValidFrom = B.DateValidFrom And A.RegionId = B.RegionId And A.CurrencyId = B.CurrencyId And A.PlantId = B.PlantId
                            Full outer join C on COALESCE(A.DateValidFrom, B.DateValidFrom) = C.DateValidFrom And COALESCE(A.CurrencyId, B.CurrencyId) = C.CurrencyId And COALESCE(A.RegionId, B.RegionId) = C.RegionId And COALESCE(A.PlantId, B.PlantId) = C.PlantId
                            join BDRegions ON COALESCE(A.RegionId, B.RegionId) = BDRegions.Id
                            join BDPlants ON COALESCE(A.PlantId, B.PlantId) = BDPlants.Id
                            join Currencies ON COALESCE(A.CurrencyId, B.CurrencyId) = Currencies.Id
                            where BDPlants.UniqueKey <> BDRegions.UniqueKey and (BDRegions.UniqueKey like N'%{inputString}%' or BDPlants.UniqueKey like N'%{inputString}%')";
            }
            else if (columnName == "공간 생산 비용")
            {
                searchQuery = $@"select DateValidFrom,BDRegions.UniqueKey As Region,BDSegments.UniqueKey,Currencies.IsoCode,Text_LOC AS comment from MDCostFactorDetails
                                JOIN BDRegions ON RegionId = BDRegions.Id
                                JOIN Currencies ON CurrencyId = Currencies.Id
                                JOIN BDPlants ON PlantId = BDPlants.Id
                                JOIN BDSegments on SegmentId = BDSegments.Id
                                join MDCostFactorDetailComments on MDCostFactorDetails.Id = MDCostFactorDetailComments.ParentId
                                where CostFactorHeaderId in
                                (
	                                select Id from MDCostFactorHeaders where UniqueKey = 'Siemens.TCPCM.MasterData.CostFactor.Common.ProductionSpaceCosts'
                                )";
            }
            else if (columnName == "작업일수")
            {
                Aselect = @"SELECT DateValidFrom, BDRegions.UniqueKey, BDSegments.UniqueKey,
                                    MAX(CASE WHEN CostFactorHeaderId = 16 THEN NonPriceValue END) AS '연간 작업 일수',
                                    MAX(CASE WHEN CostFactorHeaderId = 12 THEN NonPriceValue/3600 END) AS 'Shift 당 작업 시간',
                                    MAX(CASE WHEN CostFactorHeaderId = 13 THEN NonPriceValue END) AS 'Shift'
                                FROM MDCostFactorDetails
                                    LEFT join BDRegions ON RegionId = BDRegions.Id
                                    LEFT join BDSegments ON SegmentId = BDSegments.Id
                                where CostFactorHeaderId IN(16,12,13)";
                Bselect = " GROUP BY DateValidFrom, BDRegions.UniqueKey, BDSegments.UniqueKey";
                searchQuery = Aselect + Bselect;
            }
                //입력값 검색
            if (!string.IsNullOrEmpty(inputString))
            {
                if (columnName == "지역")
                    searchQuery = searchQuery + $" And(Cast(Name_LOC AS NVARCHAR(MAX)) like N'%{inputString}%' or Cast(UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%')";
                else if(columnName == "Plant")
                    searchQuery = searchQuery + $" And(Cast(BDRegions.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%' or Cast(BDPlants.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%')";
                else if (columnName == "업종")
                    searchQuery = searchQuery + $" And UniqueKey LIKE N'%{inputString}%'";
                else if (columnName == "단위")
                {
                    searchQuery = searchQuery + $@" where CAST(DisplayName_LOC AS NVARCHAR(MAX)) like N'%{inputString}%'
                                                 or Cast(FullName_LOC AS NVARCHAR(MAX)) like N'%{inputString}%'";
                }
                else if (columnName == "전력단가")
                {
                    Aselect += $" And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%{inputString}%'";
                    Bselect += $" And CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%{inputString}%'";
                    searchQuery = $"{Aselect} {Bselect} {FullJoin}";
                    if (!string.IsNullOrEmpty(detailQuery))
                        searchQuery += $" Where {detailQuery}";
                }
                else if (columnName == "공간 생산 비용")
                {
                    searchQuery = searchQuery + $@"And (CAST(BDRegions.Name_LOC AS NVARCHAR(MAX)) like N'%{inputString}%'
                                                or CAST(Currencies.IsoCode AS NVARCHAR(MAX)) like N'%{inputString}%'
                                                or CAST(BDPlants.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'
                                                or CAST(BDSegments.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%')";
                    if (!string.IsNullOrEmpty(detailQuery))
                        searchQuery += $" AND {detailQuery}";
                }
                else if (columnName == "작업일수")
                {
                    searchQuery = $@"{Aselect} And (BDRegions.UniqueKey like N'%{inputString}%' or BDSegments.UniqueKey like N'%{inputString}%')";
                    if(!string.IsNullOrEmpty(detailQuery))
                        searchQuery += $" And {detailQuery}";
                    searchQuery += Bselect;
                }
            }
            else if (!string.IsNullOrEmpty(detailQuery))
            {
                if(columnName == "공간 생산 비용")
                    searchQuery += $" AND {detailQuery}";
                else
                    searchQuery += $" WHERE {detailQuery}";
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                DataTable dataTable = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.PCMDB);
                if (dataTable != null)
                    foreach (DataRow row in dataTable.Rows)
                    {
                        dgv_Category.Rows.Add();
                        int i = 0;
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            string result = row[col].ToString();
                            result = NameSplit(result);
                            int count = dataTable.Columns.Count - (dataTable.Columns.Count - i++);
                            if (columnName == "공간 생산 비용" && col.ColumnName == "comment")
                            {
                                string[] splitResult = result.Split('_');
                                dgv_Category.Rows[dgv_Category.Rows.Count - 2].Cells["부대설비비율"].Value = splitResult[0];
                                dgv_Category.Rows[dgv_Category.Rows.Count - 2].Cells["건축비"].Value = splitResult[1];
                                dgv_Category.Rows[dgv_Category.Rows.Count - 2].Cells["내용년수"].Value = splitResult[2];
                            }
                            else if(columnName == "지역" && col.ColumnName == "Name_LOC")
                            {
                                if(result != null)
                                {
                                    result = result.Replace("[DYA]", "");
                                    if (!Regex.IsMatch(result, "[A-Za-z]"))
                                        result = null;
                                }
                                dgv_Category.Rows[dgv_Category.Rows.Count - 2].Cells[count].Value = result;
                            }
                            else
                                dgv_Category.Rows[dgv_Category.Rows.Count - 2].Cells[count].Value = result;

                            if (col.ColumnName.Contains("Valid") || col.ColumnName.Contains("UniqueKey")
                                || col.ColumnName.Contains("Region") || col.ColumnName.Contains("Plant") || col.ColumnName.Contains("Segment"))
                            {
                                dgv_Category.Rows[dgv_Category.Rows.Count - 2].Cells[count].ReadOnly = true;
                                dgv_Category.Rows[dgv_Category.Rows.Count - 2].Cells[count].Style.BackColor = Color.LightGray;
                            }
                        }
                    }
            }

            LoadingScreen.CloseSplashScreen();
        }
        private void searchButton1_DetailSearchButtonClick_1(object sender, EventArgs e)
        {
            string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
            if (columnName == "전력단가" || columnName == "임률" || columnName == "공간 생산 비용" | columnName == "작업 일수")
            {
                Select select = new Select();
                select.className = columnName;
                if (select.ShowDialog() == DialogResult.OK)
                {
                    SearchMethod(select.query);
                }
            }
        }

        public string NameSplit(string input)
        {
            List<string> desiredLanguages = new List<string>() { "en-US", "ko-KR" , "ru-RU", "ja-JP", "pt-BR", "de-DE" };
            string delimiter = "</split>";
            string[] xmlFiles = input.Split(new string[] { delimiter }, StringSplitOptions.None);

            for (int i = 0; i < xmlFiles.Length; i++)
            {
                string name = "";
                string xmlString = xmlFiles[i];
                try
                {
                    XDocument doc = XDocument.Parse(xmlString);

                    var translations = doc.Descendants("value")
                                       .OrderBy(v =>
                                       {
                                           string lang = (string)v.Attribute("lang");
                                           return lang == null ? int.MaxValue : desiredLanguages.IndexOf(lang);
                                       })
                                       .ToDictionary(v => (string)v.Attribute("lang") ?? string.Empty, v => (string)v);
                    string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
                    if(columnName == "지역")
                    {
                        if (translations.ContainsKey("en-US"))
                            input = $"{translations["en-US"]}";
                        else
                            input = null;

                        break;
                    }
                    else
                    {
                        foreach (var lang in desiredLanguages)
                        {
                            if (translations.ContainsKey(lang))
                            {
                                if (i == xmlFiles.Length - 1)
                                    name = $"{translations[lang]}";
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    if (i == xmlFiles.Length - 1)
                        name = $"{xmlString}";
                }
                if (!string.IsNullOrEmpty(name))
                    input = name;
            }
            return input;
        }
        private void carbon(int row)
        {
            string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
            if (columnName == "전력단가")
            {
                DateTime cell;
                if (dgv_Category.Rows[row].Cells["Valid From"].Value is DateTime)
                {
                    cell = (DateTime)dgv_Category.Rows[row].Cells["Valid From"].Value;
                }
                else return;
                string query = $@"SELECT top 1 d.Value*3600*1000 as name, d.*
                          FROM [TcPCM2312_Patch3].[dbo].[MDCostFactorHeaders] as h
                          join [TcPCM2312_Patch3].[dbo].[MDCostFactorDetails] as d on h.ID = d.CostFactorHeaderId
                          where h.UniqueKey = 'Siemens.TCPCM.MasterData.CostFactor.Carbon.RateForEnergy.Electricity' 
                                and RegionId = (select Id from BDRegions where UniqueKey = 'Siemens.TCPCM.Region.Common.SouthKorea')
		                        and DateValidFrom <= '{cell.ToString("yyyy-MM-dd")}'
                        union all

		                  SELECT top 1 d.Value*3600*1000 as name, d.*
                          FROM [TcPCM2312_Patch3].[dbo].[MDCostFactorHeaders] as h
                          join [TcPCM2312_Patch3].[dbo].[MDCostFactorDetails] as d on h.ID = d.CostFactorHeaderId
                          where h.UniqueKey = 'Siemens.TCPCM.MasterData.CostFactor.Carbon.RateForEnergy.Electricity' 
                                and RegionId = (select Id from BDRegions where UniqueKey = 'Siemens.TCPCM.Region.Common.SouthKorea')
		                        order by DateValidFrom desc";

                List<string> value = global_DB.ListSelect(query, (int)global_DB.connDB.PCMDB);

                if (value.Count != 0) dgv_Category.Rows[row].Cells["탄소배출량"].Value = value[0];
            }
        }
        private void dgv_Category_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dgv_Category.Columns.Contains("탄소배출량")) carbon(e.RowIndex);
        }

        private void CellReadOnly()
        {
            //string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
            List<string> UniqueColumns = new List<string> { "Valid From", "지역", "Plant", "업종" };
            foreach(DataGridViewRow row in dgv_Category.Rows)
            {
                if (row.IsNewRow) continue;
                foreach(DataGridViewColumn col in dgv_Category.Columns)
                {
                    if (UniqueColumns.Contains(col.Name))
                    {
                        dgv_Category.Rows[row.Index].Cells[col.Name].ReadOnly = true;
                        dgv_Category.Rows[row.Index].Cells[col.Name].Style.BackColor = Color.LightGray;
                    }
                }
            }
        }

        private void dgv_Category_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgv_Category.IsCurrentCellInEditMode)
                dgv_Category.EndEdit();

            string columnName = dgv_Category.Columns[e.ColumnIndex].Name;
            bool ascending = true;

            if (dgv_Category.Tag is Tuple<string, bool> prevSort && prevSort.Item1 == columnName)
                ascending = !prevSort.Item2;

            dgv_Category.Sort(dgv_Category.Columns[columnName],
                ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

            dgv_Category.Tag = Tuple.Create(columnName, ascending);
        }
    }
}