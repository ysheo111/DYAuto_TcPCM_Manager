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
    public partial class frmMaterial : Form
    {
        public string className = "";
        private MonthCalendar calendar;
        private DateTime selectedDate;
        public frmMaterial()
        {
            InitializeComponent();
        }

        private void frmExchange_Load(object sender, EventArgs e)
        {
            searchButton1.detailSearchButton.Visible = false;
            if (!frmLogin.auth.Contains("admin")) btn_Configuration.Visible = false;
            dgv_Material.AllowUserToAddRows = true;
            Material(MasterData.Material.injection);
            //InjectionColumn();
            dgv_Material.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void btn_DBLoad_Click(object sender, EventArgs e)
        {
            calendar = new MonthCalendar
            {
                Location = btn_DBLoad.Location,
                MaxSelectionCount = 1,
                Name = "buttonCalendar"
            };
            calendar.DateSelected += Calendar_DateSelected;

            this.Controls.Add(calendar);

            calendar.BringToFront();
            calendar.Focus();

            calendar.Leave += (s, args) =>
            {
                this.Controls.Remove(calendar);
                calendar.Dispose();
            };
        }
        private void Calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            selectedDate = e.Start;
            this.Controls.Remove(calendar);
            calendar.Dispose();
            DB_Load();
        }
        private void DB_Load()
        {
            try
            {
                Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
                splashthread.IsBackground = true;
                splashthread.Start();

                dgv_Material.Rows.Clear();

                string searchQuery = @"With A as(
	                                            select distinct
		                                            BDRegions.UniqueKey as region,
		                                            Currencies.IsoCode As IsoCode,
		                                            MDMaterialHeaders.UniqueKey As cName,
		                                            Units.Name As '원재료 단위',
                                                    MDMaterialHeaders.Name_LOC_Extracted As '소재명'
	                                            from MDMaterialDetails
		                                            left join MDMaterialHeaders on MaterialHeaderId = MDMaterialHeaders.Id
		                                            left join Units on MDMaterialDetails.UnitId = Units.Id
		                                            LEFT join BDRegions on RegionId = BDRegions.Id
		                                            LEFT join Currencies on CurrencyId = Currencies.Id
	                                            where MaterialHeaderId in
		                                            (select id from MDMaterialHeaders where CAST(Name_LOC AS NVARCHAR(MAX))  like '%[[DYA]]%')
		                                            And MDMaterialHeaders.UniqueKey not like '%_scrap'
                                            ), B as(
	                                            select distinct
		                                            BDRegions.UniqueKey as region,
		                                            Currencies.IsoCode As IsoCode,
		                                            MDMaterialHeaders.UniqueKey As sName,
		                                            Units.Name As '스크랩 단위'
	                                            from MDMaterialDetails
		                                            left join MDMaterialHeaders on MaterialHeaderId = MDMaterialHeaders.Id
		                                            left join Units on MDMaterialDetails.UnitId = Units.Id
		                                            LEFT join BDRegions on RegionId = BDRegions.Id
		                                            LEFT join Currencies on CurrencyId = Currencies.Id
	                                            where MaterialHeaderId in
		                                            (select id from MDMaterialHeaders where CAST(Name_LOC AS NVARCHAR(MAX))  like '%[[DYA]]%')
		                                            And MDMaterialHeaders.UniqueKey like '%_scrap'
                                            ), C as(
	                                            select distinct
		                                            BDRegions.UniqueKey as region,
		                                            MDMaterialHeaders.UniqueKey As sName,
		                                            Units.Name As '탄소발생량 단위'
	                                            from MDMaterialCo2Details
		                                            left join MDMaterialHeaders on MaterialHeaderId = MDMaterialHeaders.Id
		                                            left join Units on MDMaterialCo2Details.UnitId = Units.Id
		                                            LEFT join BDRegions on RegionId = BDRegions.Id
	                                            where MaterialHeaderId in
		                                            (select id from MDMaterialHeaders where CAST(Name_LOC AS NVARCHAR(MAX))  like '%[[DYA]]%')
		                                            And MDMaterialHeaders.UniqueKey like '%_scrap'
                                            ) select
                                            COALESCE(A.region, B.region, C.region) as Region,
                                            COALESCE(A.IsoCode, B.IsoCode) as IsoCode,
                                            A.[원재료 단위],
                                            B.[스크랩 단위],
                                            C.[탄소발생량 단위],
                                            A.소재명,
                                            A.cName
                                            as name From A Full outer join B on
                                            A.region = B.region And A.IsoCode = B.IsoCode
                                            And B.sName = A.cName+'_scrap'
                                            Full outer join C on
                                            COALESCE(A.region, B.region) = C.region
                                            And C.sName = B.sName
                                            Where
	                                            COALESCE(A.region, B.region, C.region) IS NOT NULL
	                                            And A.[원재료 단위] is not null";
                DataTable dataTable = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.PCMDB);
                if (dataTable == null) return;

                foreach (DataRow row in dataTable.Rows)
                {
                    dgv_Material.Rows.Add();
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        string result = row[col].ToString();
                        result = NameSplit(result);

                        if (col.ColumnName == "Region")
                            dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["지역"].Value = result;
                        else if (col.ColumnName == "IsoCode")
                            dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["통화"].Value = result;
                        else if (col.ColumnName == "원재료 단위")
                            dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["원재료 단위"].Value = result;
                        else if (col.ColumnName == "스크랩 단위")
                            dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["스크랩 단위"].Value = result;
                        else if (col.ColumnName == "탄소발생량 단위")
                            dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["단위"].Value = result;
                        else if (col.ColumnName == "소재명")
                        {
                            result = result.Replace("[DYA]", "");
                            string[] resultArray = result.Split(' ');
                            dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["소재명"].Value = resultArray[0];
                        }
                        else
                        {
                            string[] resultArray = result.Split('_');
                            dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["재질명"].Value = resultArray[0];
                            dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["GRADE"].Value = resultArray[1];
                        }
                    }
                    dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["Valid From"].Value = selectedDate;
                }
                CustomMessageBox.RJMessageBox.Show($"불러오기에 성공하였습니다", "Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다", "Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadingScreen.CloseSplashScreen();
        }
        private void btn_ExcelCreate_Click(object sender, EventArgs e)
        {
            ExcelExport excel = new ExcelExport();
            string columnName = cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString();
            string err = excel.ExportLocationGrid(dgv_Material, columnName);

            if (err != null) CustomMessageBox.RJMessageBox.Show($"Export 실패하였습니다\n{err}", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("Export 완료 되었습니다.", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btn_Create_Click(object sender, EventArgs e)
        {
            ExcelImport excel = new ExcelImport();
            string err = excel.LoadMasterData(cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString(), dgv_Material);

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            string columnName = cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString();
            if(columnName == "마그넷 와이어")
            {
                ImportMagnetWire();
            }
            else if (columnName == "Sprue")
            {
                Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
                splashthread.IsBackground = true;
                splashthread.Start();
                try
                {
                    string query = "";
                    foreach (DataGridViewRow row in dgv_Material.Rows)
                    {
                        if (row.Cells["업종"].Value == null) continue;
                        double value = 0;
                        if ((double)row.Cells["반영율"].Value > 1)
                            value = (double)row.Cells["반영율"].Value * 0.01;
                        query += $@"Update Sprue Set 반영율 = '{value}' Where 업종 = N'{row.Cells["업종"].Value}' ";

                    }
                    global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);
                }
                catch
                {
                    CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "마그넷 와이어", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Thread.Sleep(100);
                LoadingScreen.CloseSplashScreen();
            }
            else
                ImportMethod();
        }
        private void ImportMagnetWire()
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();
            try
            {
                string err = null;
                //List<string> thickTypeList = new List<string>();
                for (int rowIndex = 0; rowIndex < dgv_Material.Rows.Count - 1; rowIndex++)
                {
                    DataGridViewRow row = dgv_Material.Rows[rowIndex];
                    if (string.IsNullOrEmpty(row.Cells["type"].Value?.ToString())) continue;

                    string thickType = $"{row.Cells["두께"].Value}_{row.Cells["type"].Value}";

                    string searchQuery = $@"SELECT id FROM MD_MagnetWire
                                    where ValidFrom = '{row.Cells["Valid From"].Value}'
                                    And 두께 = '{global.ConvertDouble(row.Cells["두께"].Value)}'
                                    And type = '{row.Cells["type"].Value}'";
                    string partId = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.selfDB);
                    
                    if (string.IsNullOrEmpty(partId))
                    {
                        searchQuery = $@"Insert into MD_MagnetWire (ValidFrom, 가공비, 두께, type)
                            Values('{row.Cells["Valid From"].Value}', {global.ConvertDouble(row.Cells["가공비"].Value)}, {global.ConvertDouble(row.Cells["두께"].Value)}, '{row.Cells["type"].Value}')";
                        err = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.selfDB);
                    }
                    else
                    {
                        searchQuery = $@"Update MD_MagnetWire
                            Set ValidFrom='{row.Cells["Valid From"].Value}', 가공비='{global.ConvertDouble(row.Cells["가공비"].Value)}',두께='{global.ConvertDouble(row.Cells["두께"].Value)}', type = '{row.Cells["type"].Value}'
                            where Id = {partId}";
                        err = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.selfDB);
                    }
                }
                if (!string.IsNullOrEmpty(err)) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "마그넷 와이어", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "마그넷 와이어", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "마그넷 와이어", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Thread.Sleep(100);
            LoadingScreen.CloseSplashScreen();
        }
        private void ImportMethod()
        {
            //bool duple = IsDuplicate();
            string type = cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString();
            if (type == "원소재 단가")
            {
                isSubstance();
                if (!IsDuplicate())
                    return;
            }

            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            try
            {
                dgv_Material.AllowUserToAddRows = false;
                Material material = new Material();
                string err = material.Import(type, dgv_Material);

                if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dgv_Material.AllowUserToAddRows = true;
            LoadingScreen.CloseSplashScreen();
        }
        private void isSubstance()
        {
            List<string> list = new List<string>();
            foreach (DataGridViewRow row in dgv_Material.Rows)
            {
                if (row.IsNewRow) continue;

                string nameValue = row.Cells["재질명"].Value?.ToString();
                string grandValue = row.Cells["GRADE"].Value?.ToString();

                string searchQuery = $@"select UniqueKey as name from MDSubstances
                                        where CAST(UniqueKey AS NVARCHAR(MAX)) = N'{nameValue}_{grandValue}'";

                string result = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);

                if (string.IsNullOrEmpty(result))
                {
                    list.Add($"{nameValue}_{grandValue}");
                }
            }
            if(list.Count != 0)
            {
                string message = "";
                foreach(string msg in list)
                {
                    message += $"{msg}\n";
                }
                CustomMessageBox.RJMessageBox.Show($"아래의 물성치 정보가 없습니다:", "Material", $"{message}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private bool IsDuplicate()
        {
            List<string> list = new List<string>();
            List<int> numList = new List<int>();
            string searchQuery = $@"With A as(
	                                    select Name_LOC,Id from MDMaterialHeaders
	                                    where CAST(MDMaterialHeaders.Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%'
                                    ), B as(
	                                    select MaterialHeaderId,DateValidFrom,BDRegions.UniqueKey as region from MDMaterialDetails
	                                    join BDRegions ON RegionId = BDRegions.Id
	                                    where MaterialHeaderId in (select Id from MDMaterialHeaders where CAST(MDMaterialHeaders.Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%')
                                    )
                                    select 
                                    DateValidFrom,
                                    region,
                                    Name_LOC
                                    as name from A
                                    Full Outer Join B ON A.Id = B.MaterialHeaderId";

            DataTable resultData = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.PCMDB);

            if (resultData.Rows.Count == 0) return false;

            foreach (DataGridViewRow row in dgv_Material.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Tag != null)
                    if ((bool)row.Tag == false)
                        row.Tag = null;

                string dateValue = row.Cells["Valid From"].Value?.ToString();
                string regionValue = row.Cells["지역"].Value?.ToString();
                string nameValue = row.Cells["재질명"].Value?.ToString();
                string grandValue = row.Cells["GRADE"].Value?.ToString();

                bool isMatch = resultData.AsEnumerable().Any(r =>
                {
                    DateTime DBValid = DateTime.Parse(r[0].ToString()).Date;
                    DateTime dgvValid = DateTime.Parse(dateValue);

                    string nameLoc = r[2]?.ToString();
                    if (nameLoc == null) return false;

                    nameLoc = NameSplit(nameLoc);
                    nameLoc = nameLoc.Replace("[DYA]", "");
                    string[] splitNames = nameLoc.Split('_');

                    return DBValid == dgvValid &&
                            r[1].ToString() == regionValue &&
                            splitNames[1] == nameValue &&
                            splitNames[2] == grandValue;
                });

                if (isMatch)
                {
                    list.Add($"{dateValue} {regionValue} {nameValue}_{grandValue}");
                    numList.Add(row.Index);
                }
            }
            if (list.Count != 0)
            {
                string array = "";
                foreach (string a in list)
                {
                    array += $"{a}\n";
                }

                if (string.IsNullOrEmpty(array)) return true;
                DialogResult result = CustomMessageBox.RJMessageBox.Show($"중복되는 값을 덮어씌우겠습니까? \n(Yes=덮어쓰기,No=중복 값 제외하고 Import,Cancel=Import 취소)", "Material", $"{array}", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    foreach (int index in numList)
                    {
                        dgv_Material.Rows[index].Tag = false;
                    }
                }
                else if (result == DialogResult.Cancel) return false;
            }

            return true;
        }

        private void btn_Configuration_Click(object sender, EventArgs e)
        {
            ConfigSetting config = new ConfigSetting();
            config.className = "Material' or Class = 'Substance";
            config.Show();
        }

        private void cb_Classification_OnSelectedIndexChanged(object sender, EventArgs e)
        {   
            System.Windows.Forms.ComboBox combo = (System.Windows.Forms.ComboBox)sender;
            if (combo.SelectedItem?.ToString().Contains("SAP") == true)
            {
                btn_Save.Visible = false;
                btn_Create.Visible = false;
            }
            else
            {
                btn_Save.Visible = true;
                btn_Create.Visible = true;
            }
            if (combo.SelectedIndex < 0) return;
            dgv_Material.Columns.Clear();
            dgv_Material.DataSource = null;
            dgv_Material.Rows.Clear();
            btn_DBLoad.Visible = false;
            if (combo.SelectedItem?.ToString() == "Sprue")
            {
                //string query = $@"SELECT DISTINCT UniqueKey as '업종' FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'";
                string query = $@"SELECT 업종,반영율*100 as 반영율 FROM [PCI].[dbo].[Sprue]";

                DataTable dataTable = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
                if (dataTable == null) return;

                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        row[col] = row[col]?.ToString().Trim();
                    }
                }
                dgv_Material.DataSource = dataTable;
                //dgv_Config.Columns.Add("업종", "업종");
                dgv_Material.Columns["업종"].ReadOnly = true;
                //dgv_Config.Columns.Add("반영율", "반영율");

                dgv_Material.AllowUserToAddRows = false;                
            }
            else if (combo.SelectedItem?.ToString()== "다이캐스팅")
                Material(MasterData.Material.casting); //DieCastingColumn();
            else if (combo.SelectedItem?.ToString() == "사출")
                Material(MasterData.Material.injection); //InjectionColumn();
            else if (combo.SelectedItem?.ToString() == "프레스")
                Material(MasterData.Material.plate); //PlateColumn();
            else if (combo.SelectedItem?.ToString() == "원소재 단가")
            {
                Material(MasterData.Material.price); //PriceColumn();
                btn_DBLoad.Visible = true;
                searchButton1.detailSearchButton.Visible = true;
            }
            else if (combo.SelectedItem?.ToString() == "단가 관리" || combo.SelectedItem?.ToString().Contains("SAP")==true)
            {
                Material(MasterData.Material.management);
                searchButton1.detailSearchButton.Visible = true;
            }
            else if (combo.SelectedItem?.ToString() == "마그넷 와이어")
            {
                Material(MasterData.Material.magnet);
                searchButton1.detailSearchButton.Visible = true;
            }
            else
                Material(MasterData.Material.material);

            dgv_Material.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if(dgv_Material.DataSource == null) dgv_Material.AllowUserToAddRows = true; //dgv_Material.Rows.Add();
        }
        private void Material(List<string> variable)
        {
            dgv_Material.Columns.Clear();

            foreach (string item in variable)
            {
                if (item.Contains("Valid"))
                {
                    CalendarColumn calendar = new CalendarColumn();
                    calendar.Name = calendar.HeaderText = item;
                    calendar.SortMode = DataGridViewColumnSortMode.Programmatic;
                    dgv_Material.Columns.Add(calendar);
                    dgv_Material.Columns[item].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
                }
                else if (item.Contains("통화"))
                {
                    DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
                    combo.Name = combo.HeaderText = item;
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.SortMode = DataGridViewColumnSortMode.Programmatic;
                    dgv_Material.Columns.Add(combo);
                    ((DataGridViewComboBoxColumn)dgv_Material.Columns[item]).DataSource = global_DB.ListSelect("Select IsoCode as name From Currencies", 0);
                    dgv_Material.Columns[item].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
                }
                else if (item.Contains("지역"))
                {
                    DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
                    combo.Name = combo.HeaderText = item;
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.SortMode = DataGridViewColumnSortMode.Programmatic;
                    dgv_Material.Columns.Add(combo);
                    ((DataGridViewComboBoxColumn)dgv_Material.Columns[MasterData.Machine.region]).DataSource = global_DB.ListSelect("Select UniqueKey as name From BDRegions where CAST(Name_LOC AS NVARCHAR(MAX)) like '%[[DYA]]%'", 0);
                    dgv_Material.Columns[MasterData.Machine.region].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
                }
                else if (item.Contains("업종"))
                {
                    DataGridViewComboBoxColumn segCombo = new DataGridViewComboBoxColumn();
                    segCombo.Name = segCombo.HeaderText = item;
                    segCombo.FlatStyle = FlatStyle.Flat;
                    segCombo.SortMode = DataGridViewColumnSortMode.Programmatic;
                    dgv_Material.Columns.Add(segCombo);
                    ((DataGridViewComboBoxColumn)dgv_Material.Columns[MasterData.Machine.segment]).DataSource = global_DB.ListSelect("SELECT DISTINCT UniqueKey as name FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'", 0);
                    dgv_Material.Columns[MasterData.Machine.segment].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
                }
                else if (item.Contains("탄소발생량 단위"))
                    dgv_Material.Columns.Add("단위", item);
                else dgv_Material.Columns.Add(item, item);
            }
        }

        private void dgv_Material_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString();
            if (columnName == "Sprue") return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_Material_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dgv_Material_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!dgv_Material.Columns[e.ColumnIndex].Name.Contains("Valid")) return;

            DataGridViewRow row = dgv_Material.Rows[e.RowIndex];

            if (row.Cells[e.ColumnIndex].Value == null) return;
            row.Cells[e.ColumnIndex].Value = !DateTime.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out DateTime dt) ?
                row.Cells[e.ColumnIndex].Value : dt.ToString("yyyy-MM-dd");
        }
        private void searchButton1_SearchButtonClick(object sender, EventArgs e)
        {
            SearchMethod(null);
        }
        public void SearchMethod(string dateQuery)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();
            dgv_Material.DataSource = null;
            dgv_Material.Rows.Clear();

            string columnName = cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString();

            if (columnName == "Sprue")
            {
                //string query = $@"SELECT DISTINCT UniqueKey as '업종' FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'";
                string query = $@"SELECT 업종,반영율*100 as 반영율 FROM [PCI].[dbo].[Sprue]";

                DataTable dataTable2 = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
                if (dataTable2 == null) return;

                foreach (DataRow row in dataTable2.Rows)
                {
                    foreach (DataColumn col in dataTable2.Columns)
                    {
                        row[col] = row[col]?.ToString().Trim();
                    }
                }
                dgv_Material.DataSource = dataTable2;
                //dgv_Config.Columns.Add("업종", "업종");
                dgv_Material.Columns["업종"].ReadOnly = true;
                //dgv_Config.Columns.Add("반영율", "반영율");

                dgv_Material.AllowUserToAddRows = false;

                LoadingScreen.CloseSplashScreen();
                return;
            }
            
            string inputString = "", searchQuery = "", colQuery = "", havingQuery = ""; ;
            inputString = searchButton1.text;
            if (columnName == "원소재 단가")
            {
                searchQuery = @"With A as( select distinct BDRegions.UniqueKey as region, DateValidFrom, Currencies.IsoCode As IsoCode,
		                                    MDMaterialHeaders.UniqueKey As UniqueKey, Price, Units.Name As '원재료 단위' ,MDMaterialHeaders.Name_LOC_Extracted As '소재명'
	                                    from MDMaterialDetails
		                                    left join MDMaterialHeaders on MaterialHeaderId = MDMaterialHeaders.Id
		                                    left join Units on MDMaterialDetails.UnitId = Units.Id
		                                    LEFT join BDRegions on RegionId = BDRegions.Id
		                                    LEFT join Currencies on CurrencyId = Currencies.Id
	                                    where MaterialHeaderId in
		                                    (select id from MDMaterialHeaders where CAST(Name_LOC AS NVARCHAR(MAX))  like '%[[DYA]]%')
		                                    And MDMaterialHeaders.UniqueKey not like '%_scrap' ),
                                    B as( select distinct BDRegions.UniqueKey as region, Currencies.IsoCode As IsoCode,
		                                    MDMaterialHeaders.UniqueKey As sName, Price, Units.Name As '스크랩 단위'
	                                    from MDMaterialDetails
		                                    left join MDMaterialHeaders on MaterialHeaderId = MDMaterialHeaders.Id
		                                    left join Units on MDMaterialDetails.UnitId = Units.Id
		                                    LEFT join BDRegions on RegionId = BDRegions.Id
		                                    LEFT join Currencies on CurrencyId = Currencies.Id
	                                    where MaterialHeaderId in
		                                    (select id from MDMaterialHeaders where CAST(Name_LOC AS NVARCHAR(MAX))  like '%[[DYA]]%')
		                                    And MDMaterialHeaders.UniqueKey like '%_scrap' ),
                                    C as( select distinct BDRegions.UniqueKey as region, MDMaterialHeaders.UniqueKey As sName,
		                                    MDMaterialCo2Details.value, (Units.Name+' CO2e/'+Units.Name) As '탄소발생량 단위'
	                                    from MDMaterialCo2Details
		                                    left join MDMaterialHeaders on MaterialHeaderId = MDMaterialHeaders.Id
		                                    left join Units on MDMaterialCo2Details.UnitId = Units.Id
		                                    LEFT join BDRegions on RegionId = BDRegions.Id
	                                    where MaterialHeaderId in
		                                    (select id from MDMaterialHeaders where CAST(Name_LOC AS NVARCHAR(MAX))  like '%[[DYA]]%')
		                                    And MDMaterialHeaders.UniqueKey like '%_scrap' )
                                select top 1000
                                    A.DateValidFrom as 'Valid From',
                                    COALESCE(A.region, B.region, C.region) as 'Region',
                                    COALESCE(A.IsoCode, B.IsoCode) as '통화',
                                    A.소재명,
                                    A.UniqueKey,
                                    A.Price as '원재료 단가',
                                    A.[원재료 단위],
                                    B.Price as '스크랩 단가',
                                    B.[스크랩 단위],
                                    C.Value as '탄소발생량',
                                    C.[탄소발생량 단위]
                                From A
                                    Full outer join B on A.region = B.region And A.IsoCode = B.IsoCode And B.sName = A.UniqueKey+'_scrap'
                                    Full outer join C on COALESCE(A.region, B.region) = C.region And C.sName = B.sName
                                Where
	                                COALESCE(A.region, B.region, C.region) IS NOT NULL
	                                And A.[원재료 단위] is not null";
            }
            else if (columnName == "마그넷 와이어")
            {
                searchQuery = "select ValidFrom,가공비,두께,type from [PCI].[dbo].[MD_MagnetWire]";
            }
            else if(columnName == "단가 관리")
            {
                searchQuery = @"select top 1000 DateValidFrom,BDRegions.UniqueKey as 지역,Currencies.IsoCode as 통화, h.UniqueKey + Number as 품번,h.Name_LOC as 참고품명,Price as 단가 from MDMaterialHeaders as h
                                left join MDMaterialDetails as d on h.id = d.MaterialheaderId
                                LEFT join MDMaterialHeaderRevisions as r on d.MaterialHeaderRevisionId = r.Id
                                join BDRegions on d.RegionId = BDRegions.Id
                                join Currencies on d.CurrencyId = Currencies.Id
                                where LEN(Number) IN (2,3) and h.UniqueKey not like '%SAP_%'";
                                //where h.UniqueKey Not Like '%SAP%'
            }
            else if (columnName == "SAP 구매단가")
            {
                searchQuery = @"select top 1000 DateValidFrom,BDRegions.UniqueKey as 지역,Currencies.IsoCode as 통화, REPLACE(h.UniqueKey + Number, 'SAP_', '') as 품번, h.Name_LOC as 참고품명,Price as 단가 from MDMaterialHeaders as h
                                left join MDMaterialDetails as d on h.id = d.MaterialheaderId
                                LEFT join MDMaterialHeaderRevisions as r on d.MaterialHeaderRevisionId = r.Id
                                join BDRegions on d.RegionId = BDRegions.Id
                                join Currencies on d.CurrencyId = Currencies.Id
                                where LEN(Number) IN (2,3) and h.UniqueKey like '%SAP_%' and d.TagOfMaterialDetailId = 2";
                //where h.UniqueKey Not Like '%SAP%'
            }
            else if (columnName == "SAP 표준단가")
            {
                searchQuery = @"select top 1000 DateValidFrom,BDRegions.UniqueKey as 지역,Currencies.IsoCode as 통화,REPLACE(h.UniqueKey + Number, 'SAP_', '') as 품번,h.Name_LOC as 참고품명,Price as 단가 from MDMaterialHeaders as h
                                left join MDMaterialDetails as d on h.id = d.MaterialheaderId
                                LEFT join MDMaterialHeaderRevisions as r on d.MaterialHeaderRevisionId = r.Id
                                join BDRegions on d.RegionId = BDRegions.Id
                                join Currencies on d.CurrencyId = Currencies.Id
                                where LEN(Number) IN (2,3) and h.UniqueKey like '%SAP_%' and d.TagOfMaterialDetailId = 1";
                //where h.UniqueKey Not Like '%SAP%'
            }
            else
            {
                if (columnName == "다이캐스팅")
                {
                    colQuery = @",MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 332 THEN MDSubstancePropertyValues.DecimalValue END) AS '주조 온도 최소',
	                            MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 331 THEN MDSubstancePropertyValues.DecimalValue END) AS '주조 온도 최대',
	                            MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 330 THEN MDSubstancePropertyValues.DecimalValue END) AS 'T-factor'";
                    havingQuery = @"Group by UniqueKey,Density HAVING 
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 332 THEN MDSubstancePropertyValues.DecimalValue END) IS NOT NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 331 THEN MDSubstancePropertyValues.DecimalValue END) IS NOT NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 330 THEN MDSubstancePropertyValues.DecimalValue END) IS NOT NULL";
                }
                else if (columnName == "사출")
                {
                    colQuery = @",MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 328 THEN MDSubstancePropertyValues.DecimalValue END) AS '탈형 온도',
	                            MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 329 THEN MDSubstancePropertyValues.DecimalValue END) AS '사출 온도',
	                            MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 327 THEN MDSubstancePropertyValues.DecimalValue END) AS '금형 온도',
	                            MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 326 THEN MDSubstancePropertyValues.DecimalValue * 1000000 END) AS '열확산도'";
                    havingQuery = @"Group by UniqueKey,Density HAVING 
                                    MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 328 THEN MDSubstancePropertyValues.DecimalValue END) IS NOT NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 329 THEN MDSubstancePropertyValues.DecimalValue END) IS NOT NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 327 THEN MDSubstancePropertyValues.DecimalValue END) IS NOT NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 326 THEN MDSubstancePropertyValues.DecimalValue END) IS NOT NULL";
                }
                else if (columnName == "프레스")
                {
                    colQuery = @",MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 324 THEN MDSubstancePropertyValues.DecimalValue / 1000000 END) AS '인장 강도',
	                            MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 296 THEN MDSubstancePropertyValues.DecimalValue / 1000000 END) AS '전단 강도'";
                    havingQuery = @"Group by UniqueKey,Density HAVING
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 324 THEN MDSubstancePropertyValues.DecimalValue END) IS NOT NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 296 THEN MDSubstancePropertyValues.DecimalValue END) IS NOT NULL";
                }
                else if (columnName == "기타")
                {
                    havingQuery = @"Group by UniqueKey,Density HAVING 
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 332 THEN MDSubstancePropertyValues.DecimalValue END) IS NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 331 THEN MDSubstancePropertyValues.DecimalValue END) IS NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 330 THEN MDSubstancePropertyValues.DecimalValue END) IS NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 328 THEN MDSubstancePropertyValues.DecimalValue END) IS NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 329 THEN MDSubstancePropertyValues.DecimalValue END) IS NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 327 THEN MDSubstancePropertyValues.DecimalValue END) IS NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 326 THEN MDSubstancePropertyValues.DecimalValue END) IS NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 324 THEN MDSubstancePropertyValues.DecimalValue END) IS NULL AND
	                                MAX(CASE WHEN MDSubstancePropertyValues.ClassificationPropertyId = 296 THEN MDSubstancePropertyValues.DecimalValue END) IS NULL";
                }
                searchQuery = $@"select DISTINCT UniqueKey,Density*0.001 as Density {colQuery}
                                from MDSubstances
                                LEFT join MDSubstancePropertyValues on MDSubstances.id = MDSubstancePropertyValues.SubstanceId
                                where MDSubstances.id in (select SubstanceId from MDSubstanceStandardNames where CAST(Name_LOC AS NVARCHAR(MAX))  like '%[[DYA]]%')
                                {havingQuery}";
            }

            if (!string.IsNullOrEmpty(inputString))
            {
                if (columnName == "원소재 단가")
                    searchQuery += $" And A.소재명 like N'%{inputString}%'";
                else if (columnName == "마그넷 와이어")
                    searchQuery += $" where 가공비 like '%{inputString}%' or 두께 like '%{inputString}%' or type like '%{inputString}%'";
                else if (columnName == "단가 관리" || columnName.Contains("SAP"))
                    searchQuery += $" And ( (h.UniqueKey + Number like '%{inputString}%') or (CAST(h.Name_LOC AS NVARCHAR(MAX)) like '%{inputString}%') )";
                else
                    searchQuery += $" And CAST(UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'";
            }
            if (!string.IsNullOrEmpty(dateQuery))
            {
                if (columnName == "원소재 단가")
                    searchQuery += $" And {dateQuery}";
                else if (columnName == "마그넷 와이어")
                    searchQuery += $" Where {dateQuery}";
                else if (columnName == "단가 관리" || columnName.Contains("SAP"))
                    searchQuery += $" And {dateQuery}";
            }
            if (columnName.Contains("단가 관리") || columnName.Contains("SAP"))
            {
                searchQuery += " order by d.DateValidFrom desc ";
            }
            DataTable dataTable = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.PCMDB);
            if (dataTable == null)
            {
                LoadingScreen.CloseSplashScreen();
                return;
            }
  
            foreach (DataRow row in dataTable.Rows)
            {
                dgv_Material.Rows.Add();
                int i = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    string result = row[col].ToString();
                    result = NameSplit(result);

                    int count = dataTable.Columns.Count - (dataTable.Columns.Count - i++);
                    if (col.ColumnName == "UniqueKey")
                    {
                        string[] aa = result.Split('_');
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["재질명"].Value = aa[0];
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["재질명"].ReadOnly = true;
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["재질명"].Style.BackColor = Color.LightGray;
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["GRADE"].Value = aa[1];
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["GRADE"].ReadOnly = true;
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells["GRADE"].Style.BackColor = Color.LightGray;
                        i++;
                    }
                    else if (col.ColumnName == "소재명")
                    {
                        result = result.Replace("[DYA]", "");
                        string[] resultArray = result.Split(' ');
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells[count].Value = resultArray[0];
                    }
                    else
                    {
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells[count].Value = result;
                    }
                    if (col.ColumnName.Contains("Valid") || col.ColumnName.Contains("UniqueKey") || col.ColumnName.Contains("Region") || col.ColumnName.Contains("소재명"))
                    {
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells[count].ReadOnly = true;
                        dgv_Material.Rows[dgv_Material.Rows.Count - 2].Cells[count].Style.BackColor = Color.LightGray;
                    }
                }
            }
            
            LoadingScreen.CloseSplashScreen();

            if (columnName.Contains("단가"))
            {
                CustomMessageBox.RJMessageBox.Show($"{columnName}는 최신 날짜를 기준으로 최대 1000개까지 조회가능합니다.", "Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public string NameSplit(string input)
        {
            List<string> desiredLanguages = new List<string>() { "en-US", "ko-KR", "ru-RU", "ja-JP", "pt-BR", "de-DE" };
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
                    if (columnName == "지역")
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
        
        private void searchButton1_DetailSearchButtonClick(object sender, EventArgs e)
        {
            string columnName = cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString();
            if (columnName == "사출" || columnName == "다이캐스팅" || columnName == "프레스" || columnName == "기타") return;
            Select select = new Select();
            select.className = "Material";
            if(select.ShowDialog() == DialogResult.OK)
            {
                SearchMethod(select.query);
            }
        }

        private void dgv_Material_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgv_Material.IsCurrentCellInEditMode)
                dgv_Material.EndEdit();

            string columnName = dgv_Material.Columns[e.ColumnIndex].Name;
            bool ascending = true;

            if (dgv_Material.Tag is Tuple<string, bool> prevSort && prevSort.Item1 == columnName)
                ascending = !prevSort.Item2;

            dgv_Material.Sort(dgv_Material.Columns[columnName],
                ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

            dgv_Material.Tag = Tuple.Create(columnName, ascending);
        }
    }
}