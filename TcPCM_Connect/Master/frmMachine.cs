﻿using System;
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
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class frmMachine : Form
    {
        public string className = "";
        public frmMachine()
        {
            InitializeComponent();
        }

        private void frmExchange_Load(object sender, EventArgs e)
        {
            if (!frmLogin.auth.Contains("admin")) btn_Configuration.Visible = false;
            dgv_Machine.AllowUserToAddRows = true;
            ColumnAdd();
            Capitalcolumn();
            dgv_Machine.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            ExcelImport excel = new ExcelImport();
            string err = excel.LoadMasterData("Machine", dgv_Machine);

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Machine", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            try
            {
                Machine machine = new Machine();
                string err = machine.Import(dgv_Machine, cb_Classification.Texts);                

                if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "Machine", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    CellReadOnly();
                    CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "Machine", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } 
            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "Machine", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Thread.Sleep(100);
            LoadingScreen.CloseSplashScreen();
        }
        private void searchButton1_DetailSearchButtonClick(object sender, EventArgs e)
        {
            Select select = new Select();
            select.className = "설비";
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
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            dgv_Machine.Rows.Clear();

            string columnName = cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString();
            string inputString = "", searchQuery = "", havingQuery = ""; ;
            inputString = searchButton1.text;

            searchQuery = @"select DateValidFrom As 'Valid From', Currencies.IsoCode AS '통화', BDSegments.UniqueKey AS '업종',[MDAssetHeaderPropertyValues].[DecimalValue] as '최대 톤수',
	                            MDAssetHeaders.UniqueKey As '설비명', Manufacturers.Name As '설비구분', 
	                            MAX(CASE WHEN CostElementDefinition.UniqueKey like '%machine%value%' THEN MDAssetDetailInvests.Price END) AS '설비가',
	                            DepreciationTime AS '기계상각년수', RequiredSpaceGross AS '설치면적', ConnectedLoad /1000 As '전력용량', PowerOnTimeRate * 100 AS '전력소비율',
	                            MAX(CASE WHEN CostElementDefinition.UniqueKey like '%other%machine%' THEN MDAssetDetailInvests.Price END) AS '기타 비용', OtherFixCostsRate AS '내용년수'
                                
                            from MDAssetDetails
	                            join Currencies on CurrencyId = Currencies.Id
	                            join BDSegments on SegmentId = BDSegments.Id
	                            join MDAssetHeaders on AssetHeaderId = MDAssetHeaders.Id
                                left join [MDAssetHeaderPropertyValues] on MDAssetHeaders.Id = [MDAssetHeaderPropertyValues].[AssetHeaderId] and [MDAssetHeaderPropertyValues].[ClassificationPropertyId] = 703
	                            left Join Manufacturers on Manufacturers.id = MDAssetHeaders.ManufacturerId
	                            Join MDAssetDetailInvests on MDAssetDetailInvests.AssetDetailId = MDAssetDetails.Id
	                            join CostElementDefinition on CostElementDefinition.id = MDAssetDetailInvests.CostElementDefinitionId
                            where MDAssetDetails.AssetHeaderId in (select id from MDAssetHeaders where CAST(Name_LOC AS NVARCHAR(MAX)) like N'%[[DYA]]%')";
            havingQuery = @" Group BY Manufacturers.Name,MDAssetHeaders.UniqueKey,DateValidFrom, Currencies.IsoCode,
	                        PowerOnTimeRate,RequiredSpaceGross,ConnectedLoad,BDSegments.UniqueKey,OtherFixCostsRate,DepreciationTime,[MDAssetHeaderPropertyValues].[DecimalValue]";
            
            if (!string.IsNullOrEmpty(inputString))
            {
                searchQuery = searchQuery + $" And CAST(MDAssetHeaders.UniqueKey AS NVARCHAR(MAX)) like N'%{inputString}%'";
            }
            if (!string.IsNullOrEmpty(detailQuery))
                searchQuery += $" And {detailQuery}";

            searchQuery += havingQuery;

            DataTable dataTable = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.PCMDB);
            if (dataTable == null) return;
            double maxPress = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                dgv_Machine.Rows.Add();
                int i = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    string result = row[col].ToString();
                    if (col.ColumnName == "최대 톤수")
                    {
                        maxPress = global.ConvertDouble(result);
                        continue;
                    }
                    int count = dataTable.Columns.Count - (dataTable.Columns.Count - i++);
                    if (col.ColumnName == "설비명")
                    {
                        string[] aa = result.Split('_');
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["설비명"].Value = aa[0];
                        if (aa.Length == 2)
                        {
                            dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["설비명"].Value = aa[0];
                            dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["사양 정보"].Value = aa[1];
                        }
                        else if (aa.Length == 3)
                        {
                            dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["설비명"].Value = aa[0];
                            if (global.ConvertDouble(aa[1]) != 0)
                            {
                                dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["최대 톤수"].Value = aa[1];
                            }
                           else dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["사양 정보"].Value = aa[1] +"_"+ aa[2];
                        }
                        else if (aa.Length > 3)
                        {
                            if(maxPress>0)
                            {
                                string[] split = result.Split(maxPress.ToString().ToCharArray());
                                dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["설비명"].Value = split[0];
                                dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["최대 톤수"].Value = maxPress;
                                dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["사양 정보"].Value = split[1].Replace(maxPress.ToString()+"_","");
                            }
                            else if (int.TryParse(aa[1], out int num))
                            {
                                dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["최대 톤수"].Value = aa[1];
                                for (int a = 2; a < aa.Length; a++)
                                {
                                    dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["사양 정보"].Value = dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["사양 정보"].Value + aa[a];
                                }
                            }
                            else
                            {
                                for (int a = 1; a < aa.Length; a++)
                                {
                                    dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["사양 정보"].Value = dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["사양 정보"].Value + aa[a];
                                }
                            }
                        }
                        i += 2;
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["설비명"].ReadOnly = true;
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["최대 톤수"].ReadOnly = true;
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["사양 정보"].ReadOnly = true;
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["설비명"].Style.BackColor = Color.LightGray;
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["최대 톤수"].Style.BackColor = Color.LightGray;
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells["사양 정보"].Style.BackColor = Color.LightGray;
                    }
                    else
                    {
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells[count].Value = result;
                    }
                    if (col.ColumnName.Contains("Valid") || col.ColumnName.Contains("UniqueKey") || col.ColumnName.Contains("업종"))
                    {
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells[count].ReadOnly = true;
                        dgv_Machine.Rows[dgv_Machine.Rows.Count - 2].Cells[count].Style.BackColor = Color.LightGray;
                    }
                }
            }
            LoadingScreen.CloseSplashScreen();
        }
        private void CellReadOnly()
        {
            List<string> UniqueColumns = new List<string> { "Valid From", "업종", "설비명", "최대 톤수", "사양 정보" };
            foreach (DataGridViewRow row in dgv_Machine.Rows)
            {
                if (row.IsNewRow) continue;
                foreach (DataGridViewColumn col in dgv_Machine.Columns)
                {
                    if (UniqueColumns.Contains(col.Name))
                    {
                        dgv_Machine.Rows[row.Index].Cells[col.Name].ReadOnly = true;
                        dgv_Machine.Rows[row.Index].Cells[col.Name].Style.BackColor = Color.LightGray;
                    }
                }
            }
        }
        private void btn_Configuration_Click(object sender, EventArgs e)
        {
            ConfigSetting config = new ConfigSetting();
            config.className = "Machine";
            config.Show();
        }
        private void btn_Shift_Click(object sender, EventArgs e)
        {
            Component.ShiftSetting shift = new Component.ShiftSetting();
            shift.Show();
        }

        private void ColumnAdd()
        {
            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = calendar.HeaderText = MasterData.Machine.validFrom;
            dgv_Machine.Columns.Add(calendar);
            calendar.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Machine.Columns[MasterData.Machine.validFrom].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
            
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.Name = combo.HeaderText = MasterData.Machine.currency;
            combo.FlatStyle = FlatStyle.Flat;
            combo.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Machine.Columns.Add(combo);
            ((DataGridViewComboBoxColumn)dgv_Machine.Columns[MasterData.Machine.currency]).DataSource = global_DB.ListSelect("Select IsoCode as name From Currencies", 0);
            dgv_Machine.Columns[MasterData.Machine.currency].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);

            DataGridViewComboBoxColumn segCombo = new DataGridViewComboBoxColumn();
            segCombo.Name = segCombo.HeaderText = MasterData.Machine.segment;
            segCombo.FlatStyle = FlatStyle.Flat;
            segCombo.SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_Machine.Columns.Add(segCombo);
            ((DataGridViewComboBoxColumn)dgv_Machine.Columns[MasterData.Machine.segment]).DataSource = global_DB.ListSelect("SELECT DISTINCT UniqueKey as name FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'", 0);
            dgv_Machine.Columns[MasterData.Machine.segment].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);

            //dgv_Machine.Columns.Add(MasterData.Machine.segment, MasterData.Machine.segment);

            dgv_Machine.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgv_Machine_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e, 2);
            if (!dgv_Machine.Columns[e.ColumnIndex].Name.Contains("Valid")) return;

            DataGridViewRow row = dgv_Machine.Rows[e.RowIndex];

            if (row.Cells[e.ColumnIndex].Value == null) return;
            row.Cells[e.ColumnIndex].Value = !DateTime.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out DateTime dt) ?
                row.Cells[e.ColumnIndex].Value : dt.ToString("yyyy-MM-dd");
        }

        private void dgv_Machine_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgv_Machine.Columns[e.ColumnIndex].Name == "내용년수") return;

            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_Machine_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cb_Classification_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_Machine.Rows.Clear();
            dgv_Machine.Columns.Clear();
            System.Windows.Forms.ComboBox combo = (System.Windows.Forms.ComboBox)sender;
            if (combo.SelectedIndex < 0) return;

            ColumnAdd();
            if (combo.SelectedItem?.ToString() == "설비") Capitalcolumn();
            dgv_Machine.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Capitalcolumn()
        {
            //dgv_Machine.Columns.Add(MasterData.Machine.customer, "업체명");
            dgv_Machine.Columns.Add(MasterData.Machine.designation1, MasterData.Machine.designation1);
            dgv_Machine.Columns.Add(MasterData.Machine.maxClampingForce, MasterData.Machine.maxClampingForce);
            dgv_Machine.Columns.Add(MasterData.Machine.maker, MasterData.Machine.maker);

            DataGridViewComboBoxColumn com = new DataGridViewComboBoxColumn();
            com.Items.Add("");
            com.Items.Add("범용");
            com.Items.Add("전용");
            com.Items.Add("대여");
            com.Name = com.HeaderText = MasterData.Machine.manufacturer;

            dgv_Machine.Columns.Add(com);
            dgv_Machine.Columns.Add(MasterData.Machine.acquisition, MasterData.Machine.acquisition);
            dgv_Machine.Columns[MasterData.Machine.acquisition].DefaultCellStyle.Format = "N2";
            dgv_Machine.Columns.Add(MasterData.Machine.imputed, MasterData.Machine.imputed);
            dgv_Machine.Columns.Add(MasterData.Machine.space, MasterData.Machine.space);
            dgv_Machine.Columns.Add(MasterData.Machine.ratedPower, MasterData.Machine.ratedPower);
            dgv_Machine.Columns.Add(MasterData.Machine.poweUtiliation, MasterData.Machine.poweUtiliation);
            dgv_Machine.Columns.Add(MasterData.Machine.acquisitionEx, MasterData.Machine.acquisitionEx);
            dgv_Machine.Columns[MasterData.Machine.acquisitionEx].DefaultCellStyle.Format = "N2";
            dgv_Machine.Columns.Add("내용년수", "기타 내용년수");
        }

        private void dgv_Machine_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgv_Machine.IsCurrentCellInEditMode)
                dgv_Machine.EndEdit();

            string columnName = dgv_Machine.Columns[e.ColumnIndex].Name;
            bool ascending = true;

            if (dgv_Machine.Tag is Tuple<string, bool> prevSort && prevSort.Item1 == columnName)
                ascending = !prevSort.Item2;

            dgv_Machine.Sort(dgv_Machine.Columns[columnName],
                ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

            dgv_Machine.Tag = Tuple.Create(columnName, ascending);
        }

        private void dgv_Machine_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string rowNumber = (e.RowIndex + 1).ToString();

            Rectangle headerBounds = new Rectangle(
                e.RowBounds.Left,
                e.RowBounds.Top,
                dgv_Machine.RowHeadersWidth,
                e.RowBounds.Height);

            e.Graphics.DrawString(rowNumber,
                dgv_Machine.Font,
                SystemBrushes.ControlText,
                headerBounds,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
    }
}
