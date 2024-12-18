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
using TcPCM_Connect_Global;
using System.Xml.Linq;

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
            else
            {
                CostFactor costFactor = new CostFactor();
                string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
                string err2 = costFactor.Import("Category", columnName.Replace("4", ""), dgv_Category);

                if (err2 != null) CustomMessageBox.RJMessageBox.Show($"Import 실패하였습니다\n{err2}", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else CustomMessageBox.RJMessageBox.Show("Import 완료 되었습니다.", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();
            
            try
            {
                CostFactor costFactor = new CostFactor();
                string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
                string err = costFactor.Import("Category", columnName.Replace("4", ""), dgv_Category);

                if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                dgv_Category.Columns.Add("지역", "지역");
                ValidFromAdd("Valid From");
                dgv_Category.Columns.Add("업종", "업종");
                dgv_Category.Columns.Add("연간 작업 일수", "연간 작업 일수");
                dgv_Category.Columns.Add("Shift", "Shift");
                dgv_Category.Columns.Add("Shift 당 작업 시간", "Shift 당 작업 시간");
            }
            else if (columnName == "공간 생산 비용")
            {
                dgv_Category.Columns.Add("지역", "지역");
                CurrencyAdd("통화");
                ValidFromAdd("Valid From");
                dgv_Category.Columns.Add("업종", "업종");
                dgv_Category.Columns.Add("건물상각년수", "건물상각년수");
                dgv_Category.Columns.Add("건물점유비율", "건물점유비율");
                dgv_Category.Columns.Add("건축비", "건축비");
            }
            else if (columnName == "전력단가")
            {
                dgv_Category.Columns.Add("지역", "지역");
                dgv_Category.Columns.Add("업종", "업종");
                CurrencyAdd("통화");
                ValidFromAdd("Valid From");
                dgv_Category.Columns.Add("전력단가", "전력단가");
            }
            else if (columnName == "임률")
            {
                dgv_Category.Columns.Add("지역", "지역");
                //dgv_Category.Columns.Add("구분4", "구분4");
                dgv_Category.Columns.Add("업종", "업종");
                CurrencyAdd("통화");
                ValidFromAdd("Valid From");
                dgv_Category.Columns.Add("임률", "임률");
                dgv_Category.Columns.Add("Labor burden (1Shift)", "Labor burden (1Shift)");
                dgv_Category.Columns.Add("Labor burden (2Shift)", "Labor burden (2Shift)");
                dgv_Category.Columns.Add("Labor burden (3Shift)", "Labor burden (3Shift)");
            }
            else if(columnName == "단위")
            {
                dgv_Category.Columns.Add("UOM Code", "UOM Code");
                dgv_Category.Columns.Add("UOM명", "UOM명");
                dgv_Category.Columns.Add("UniqueId", "UniqueId");
                dgv_Category.Columns["UniqueId"].Visible = false;
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
            dgv_Category.Columns.Add(combo);
            ((DataGridViewComboBoxColumn)dgv_Category.Columns[MasterData.Machine.currency]).DataSource = global_DB.ListSelect("Select IsoCode as name From Currencies", 0);
            dgv_Category.Columns[MasterData.Machine.currency].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }

        private void ValidFromAdd(string columnName)
        {
            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = calendar.HeaderText = columnName;
            dgv_Category.Columns.Add(calendar);
            dgv_Category.Columns[columnName].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }

        private void dgv_Category_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!dgv_Category.Columns[e.ColumnIndex].Name.Contains("Valid")) return;

            DataGridViewRow row = dgv_Category.Rows[e.RowIndex];

            if (row.Cells[e.ColumnIndex].Value == null) return;
            row.Cells[e.ColumnIndex].Value = !DateTime.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out DateTime dt) ?
                row.Cells[e.ColumnIndex].Value : dt.ToString("yyyy-MM-dd");
        }

        private void dgv_Category_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            
            if(dgv_Category.Columns.Contains("Designation") && dgv_Category.Columns[e.ColumnIndex].Name != "Designation")
            {
                dgv_Category.Rows[e.RowIndex].Cells["Designation"].Value = $"[DYA]{dgv_Category.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}";
            }
            else if(dgv_Category.Columns[e.ColumnIndex].Name == "UOM Code")
            {
                dgv_Category.Rows[e.RowIndex].Cells["UniqueId"].Value = dgv_Category.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? null : dgv_Category.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToLower();
            }

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
            dgv_Category.Rows.Clear();

            string inputString = "", searchQeury = "", category = "", DBcolumnName = "";
            inputString = searchButton1.text;
            List<string> resultList = new List<string>();

            string columnName = cb_Classification.SelectedItem == null ? "지역" : cb_Classification.SelectedItem.ToString();
            if (columnName == "지역")
            {
                category = "BDRegions";
                DBcolumnName = "Name_LOC";
            }
            else if (columnName == "업종")
            {
                category = "BDCustomers";
                DBcolumnName = "Name_LOC";
            }
            else if (columnName == "단위")
            {
                category = "Units";
                DBcolumnName = "DisplayName_LOC, FullName_LOC";
            }

            //입력값 검색
            if (!string.IsNullOrEmpty(inputString))
            {
                if (columnName == "단위")
                    searchQeury = $"SELECT {DBcolumnName} as name FROM {category} where CAST({DBcolumnName} AS NVARCHAR(MAX)) LIKE N'%{inputString}%'";
                else
                    searchQeury = $"SELECT {DBcolumnName} as name FROM {category} where UniqueKey LIKE N'%{inputString}%' And CAST({DBcolumnName} AS NVARCHAR(MAX)) Like '%[[DYA]]%'";
            }
            //전체 검색
            else
            {
                if(columnName == "단위")
                    searchQeury = $"SELECT {DBcolumnName} as name FROM {category}";
                else
                    searchQeury = $"SELECT {DBcolumnName} as name FROM {category} where CAST({DBcolumnName} AS NVARCHAR(MAX)) Like '%[[DYA]]%'";
            }
            DataTable dataTable = global_DB.MutiSelect(searchQeury, (int)global_DB.connDB.PCMDB);

            foreach (DataRow row in dataTable.Rows)
            {
                dgv_Category.Rows.Add();
                int i = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    string result = row[col].ToString();
                    result = NameSplit(result);
                    int count = dataTable.Columns.Count - (dataTable.Columns.Count - i++);
                    dgv_Category.Rows[dgv_Category.Rows.Count-2].Cells[count].Value = result;
                }
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
    }
}
