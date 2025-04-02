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
    public partial class frmExchange : Form
    {
        public string className = "";
        public frmExchange()
        {
            InitializeComponent();
        }

        private void frmExchange_Load(object sender, EventArgs e)
        {
            dgv_ExchangeRate.AllowUserToAddRows= true;
            ((DataGridViewComboBoxColumn)dgv_ExchangeRate.Columns["통화"]).DataSource = global_DB.ListSelect("Select IsoCode as name From Currencies", 0);
            dgv_ExchangeRate.Columns["통화"].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
            dgv_ExchangeRate.Columns["Valid_From"].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
            dgv_ExchangeRate.Columns["Valid_From"].Name = "Valid From";

            ((DataGridViewComboBoxColumn)dgv_ExchangeRate.Columns["구분자"]).DataSource = new List<string> { "계획환율", "실적환율" };
            dgv_ExchangeRate.Columns["통화"].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            ExcelImport excel = new ExcelImport();
            string err = excel.LoadMasterData("Exchange", dgv_ExchangeRate);

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Exchange", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_ExcelCreate_Click(object sender, EventArgs e)
        {
            ExcelExport excel = new ExcelExport();
            string err = excel.ExportLocationGrid(dgv_ExchangeRate, "환율");
            if (err != null) CustomMessageBox.RJMessageBox.Show($"Export 실패하였습니다\n{err}", "Exchange", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("Export 완료 되었습니다.", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            try
            {
                Exchange exchange = new Exchange();
                string err = exchange.Import(dgv_ExchangeRate);                

                if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "ExchangeRate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "ExchangeRate", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "ExchangeRate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Thread.Sleep(100);
            LoadingScreen.CloseSplashScreen();
        }

        private void btn_Configuration_Click(object sender, EventArgs e)
        {
            ConfigSetting config = new ConfigSetting();
            config.className = "ExchangeRate";
            config.Show();
        }

        private void dgv_ExchangeRate_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgv_ExchangeRate.Columns[e.ColumnIndex].Name == "환율" && dgv_ExchangeRate.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                if (double.TryParse(dgv_ExchangeRate.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out double number))
                {
                    dgv_ExchangeRate.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = number.ToString("#,##0.#####");
                }
            }

            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_ExchangeRate_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void searchButton1_DetailSearchButtonClick(object sender, EventArgs e)
        {
            Select select = new Select();
            select.className = "환율";
            if (select.ShowDialog() == DialogResult.OK)
            {
                SearchMethod(select.query);
            }
        }
        private void SearchMethod(string detailQuery)
        {
            dgv_ExchangeRate.Rows.Clear();
            string inputString = "", searchQuery = "";
            inputString = searchButton1.text;

            //전체 검색
            searchQuery = $@"SELECT MDExchangeRateDetails.DateValidFrom, BDState.UniqueKey, MDExchangeRateHeaders.Name_LOC, Currencies.IsoCode, MDExchangeRateDetails.ExchangeRate
                             FROM MDExchangeRateHeaders
                             JOIN MDExchangeRateDetails ON MDExchangeRateHeaders.Id = MDExchangeRateDetails.ExchangeRateHeaderId
                             JOIN Currencies ON MDExchangeRateHeaders.CurrencyId = Currencies.Id
                             JOIN BDState on BDState.Id = MDExchangeRateDetails.StateId
                            Where Currencies.IsoCode != 'KRW'";

            if (!string.IsNullOrEmpty(inputString) && !string.IsNullOrEmpty(detailQuery))//검색 값 있고 상세 검색O
            {
                searchQuery += $@" And {detailQuery}
                            And ( CAST(MDExchangeRateHeaders.Name_LOC AS NVARCHAR(MAX)) like N'%{inputString}%'
                            or Cast(Currencies.IsoCode AS NVARCHAR(MAX)) like N'%{inputString}%' )";
            }
            else if (!string.IsNullOrEmpty(inputString) && string.IsNullOrEmpty(detailQuery))//검색 값 있고 상세 검색X
            {
                searchQuery += $@" And ( CAST(MDExchangeRateHeaders.Name_LOC AS NVARCHAR(MAX)) like N'%{inputString}%'
                             or Cast(Currencies.IsoCode AS NVARCHAR(MAX)) like N'%{inputString}%' ) ";
            }
            else if (string.IsNullOrEmpty(inputString) && !string.IsNullOrEmpty(detailQuery))//검색 값 없고 상세 검색O
            {
                searchQuery += $" AND {detailQuery}";
            }

            DataTable dataTable = global_DB.MutiSelect(searchQuery, (int)global_DB.connDB.PCMDB);
            if (dataTable == null) return;

            foreach (DataRow row in dataTable.Rows)
            {
                dgv_ExchangeRate.Rows.Add();
                int i = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    string result = row[col].ToString();
                    result = NameSplit(result);
                    int count = dataTable.Columns.Count - (dataTable.Columns.Count - i++);
                    if (result == "M")
                        result = "실적환율";
                    else if (result == "T")
                        result = "계획환율";
                    dgv_ExchangeRate.Rows[dgv_ExchangeRate.Rows.Count - 2].Cells[count].Value = result;
                }
            }
            dgv_ExchangeRate.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void searchButton1_SearchButtonClick(object sender, EventArgs e)
        {
            SearchMethod(null);
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
