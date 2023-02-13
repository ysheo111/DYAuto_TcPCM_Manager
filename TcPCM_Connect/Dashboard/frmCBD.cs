using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcPCM_Connect_Global;
using System.Threading;

namespace TcPCM_Connect
{
    public partial class frmCBD : Form
    {
        //Constructor
        public frmCBD()
        {
            InitializeComponent();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
        }
        public frmCBD Clone() { return (frmCBD)this.MemberwiseClone(); }

        public Dictionary<string, Dictionary<string, object>> part;
        public Dictionary<string, string> validFrom;
        
        public string mode, currentNode, fileLocation;
        public Bom.ExportLang exportMode=Bom.ExportLang.Kor;

        ExportCBD export = new ExportCBD();
        ImportCBD import = new ImportCBD();

        Dictionary<string, double> reference = null;
        Dictionary<string, List<double>> masterData;

        Dictionary<string, DataGridView> assySorting = new Dictionary<string, DataGridView>();

        public Dictionary<string, DataGridView> ReturnValue1 { get; private set; }
        public Dictionary<string, List<Dictionary<string, object>>> ReturnValue2 { get; private set; }
        private void frmCBD_Load(object sender, EventArgs e)
        {
            //double overheadsRate = global.ConvertDoule(part["종합"][Report.Cost.materialOverheads]) / 100;
            assySorting.Clear();
            BasicInfoColumn();
            SubPartColumn();
            MaterialColumn();
            ExternalMaterialColumn();
            ManufacturingColumn();
            EtcColumn();

            assySorting.Add("소재", dgv_Material);
            assySorting.Add("외주", dgv_External);
            assySorting.Add("하위파트", dgv_SubPart);
            assySorting.Add("종합", dgv_BaicInfo);
            assySorting.Add("기타", dgv_Etc);
            assySorting.Add("요약", dgv_Summary);
            assySorting.Add("공정", dgv_Manufacturing);

            if (exportMode == Bom.ExportLang.Eng)
            {
                cb_Mode.Image = Properties.Resources.간략1;
                cb_Mode.Checked = true;

            }
            else
            {
                cb_Mode.Image = Properties.Resources.상세1;
                cb_Mode.Checked = false;
            }

            dgv_Summary.Rows.Clear();

            dgv_Summary.Rows.Add();
            dgv_Summary.Rows[0].Cells["구분"].Value = "금액";
            dgv_Summary.Rows.Add();
            dgv_Summary.Rows[1].Cells["구분"].Value = "비율";

            if (mode.Contains("Export"))
            {
                btn_Import.Visible = false;
                btn_ExportPCM.Visible = false;
                Export();
            }
            else if (mode.Contains("Import"))
            {
                if (part != null)
                {
                    if(part.ContainsKey("종합")) reference = part["종합"].ToDictionary(pair => pair.Key, pair => global.ConvertDoule(pair.Value));
                    Import(part);
                }

                btn_Export.Visible = false;
                this.ReturnValue2 = TcPCMExportFormatMatching();
            }

            this.ReturnValue1 = assySorting;

            if (mode == "ExcelExportAll") btn_Export.PerformClick();
            if (mode.Contains("Excel")) this.Close();
        }
        private void Import(Dictionary<string, Dictionary<string, object>> importPart)
        {
            dgv_BaicInfo.Rows.Clear();
            dgv_Etc.Rows.Clear();
            dgv_SubPart.Rows.Clear();
            dgv_Manufacturing.Rows.Clear();
            dgv_Material.Rows.Clear();
            dgv_External.Rows.Clear();

            dgv_BaicInfo.Rows.Add();
            export.SummaryExport(dgv_BaicInfo, part["종합"]);

            dgv_Etc.Rows.Add();
            export.SummaryExport(dgv_Etc, part["종합"]);

            foreach (var item in importPart)
            {
                if (item.Key.Contains("종합") || item.Value == null) continue;
                else if (item.Key.Contains("하위"))
                {
                    dgv_SubPart.Rows.Add();
                    export.SubPart(dgv_SubPart, item);
                    dgv_SubPart.Rows[dgv_SubPart.Rows.Count-1].Cells["PartNumber"].Value = item.Key;
                }
                else if (item.Key.Contains("공정")) import.DGVImport(dgv_Manufacturing, item.Value);
                else if (item.Key.Contains("소재")) import.DGVImport(dgv_Material, item.Value);
                else if (item.Key.Contains("외주")) import.DGVImport(dgv_External, item.Value);
            }

            //MaterialSummary();
            //ManufacturingSummary();
        }
        private void Export()
        {
            dgv_BaicInfo.Rows.Add();
            export.SummaryExport(dgv_BaicInfo, part["종합"]);

            dgv_Etc.Rows.Add();
            export.SummaryExport(dgv_Etc, part["종합"]);            

            masterData = new Dictionary<string, List<double>>()
            {
                { Report.Cost.manufacturingOverheads, new List<double>() },
                { Report.Cost.materialOverheads, new List<double>() { global.ConvertDoule(part["종합"][Report.Cost.materialOverheads])}},
                { Report.Cost.externalmaterialOverheads, new List<double>() { global.ConvertDoule(part["종합"][Report.Cost.externalmaterialOverheads])}},
                { Report.Cost.overheadsPer, new List<double>() { global.ConvertDoule(part["종합"][Report.Cost.overheadsPer])}},
                { Report.Cost.profitPer, new List<double>() { global.ConvertDoule(part["종합"][Report.Cost.profitPer])}},
                { Report.Cost.workhour, new List<double>() },
                { "1Shift", new List<double>() },
                { "2Shift", new List<double>() },
                { Report.Cost.marginRate, new List<double>() },
                { Report.Cost.spaceMachine, new List<double>() },
                { "범용", new List<double>() },
                { "전용", new List<double>() },
                { "", new List<double>() },
                { Report.Cost.energyMachine, new List<double>() },
                { Report.Cost.maintance+"_1Shift", new List<double>() },
                { Report.Cost.maintance+"_2Shift", new List<double>() },
                { Report.Cost.utilizationPower, new List<double>() },
            };

            foreach (var item in part)
            {
                if (item.Key.Contains("종합") || item.Value == null) continue;
                else if (item.Key.Contains("하위"))
                {
                    dgv_SubPart.Rows.Add();
                    export.SubPart(dgv_SubPart, item);
                }
                else if (item.Key.Contains("공정"))
                {
                    dgv_Manufacturing.Rows.Add();
                    export.Manufacturing(dgv_Manufacturing, item, ref masterData);
                }
                else if (!item.Value.ContainsKey(Report.Designation.basic)) continue;
                else if (item.Value[Report.Designation.basic]?.ToString().Contains("소재") == true)
                {
                    dgv_Material.Rows.Add();
                    masterData[Report.Cost.materialOverheads].Add(export.Material(dgv_Material, item));
                }
                else if (item.Value[Report.Designation.basic]?.ToString().Contains("외주") == true)
                {
                    dgv_External.Rows.Add();
                    masterData[Report.Cost.externalmaterialOverheads].Add(export.External(dgv_External, item));
                }
            }

            MaterialSummary();
            ManufacturingSummary();

            assySorting["소재"] = dgv_Material;
            assySorting["공정"] = dgv_Manufacturing;
            assySorting["외주"] = dgv_External;
            assySorting["하위파트"] = dgv_SubPart;
            assySorting["종합"] = dgv_BaicInfo;
            assySorting["기타"] = dgv_Etc;
            assySorting["요약"] = dgv_Summary;
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            ExcelExport export = new ExcelExport();

            string fileName =$"{fileLocation}\\표준원가분석.xlsx";

            if (mode != "ExcelExportAll")
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = ".xlsx|";
                dlg.FileName = $"{ assySorting["종합"].Rows[0].Cells[Report.Designation.basic].Value}_표준원가분석.xlsx";

                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    CustomMessageBox.RJMessageBox.Show($"Error : 저장위치가 올바르게 선택되지 않았습니다.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                fileName = dlg.FileName;
            }
            else fileName = fileName.Replace("표준원가분석", $"{assySorting["종합"].Rows[0].Cells[Report.Designation.basic].Value}_표준원가분석");

            string err = export.Export($"{dgv_BaicInfo.Rows[0].Cells[Report.Designation.basic].Value}", exportMode, new frmCBD(), fileName, part, assySorting, masterData);
            if (err != null) CustomMessageBox.RJMessageBox.Show($"Error : {err}", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if(mode != "ExcelExportAll") CustomMessageBox.RJMessageBox.Show($"CBD 출력이 완료되었습니다.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            ExcelImport import = new ExcelImport();
            var result = import.ImportCBD(exportMode);

            if (result.Item2 != null)
            {
                part = result.Item2;
                if (part.ContainsKey("종합")) reference = part["종합"].ToDictionary(pair => pair.Key, pair => global.ConvertDoule(pair.Value));
                Import(result.Item2);
            }

            if (result.Item1 != null) CustomMessageBox.RJMessageBox.Show($"Error : {result}", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show($"엑셀 Import가 완료되었습니다.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            Thread.Sleep(100);
            LoadingScreen.CloseSplashScreen();

        }
        private void AllDataFormatChange(ref Dictionary<string, List<Dictionary<string, object>>> exportData)
        {
            foreach (DataGridViewRow row in dgv_BaicInfo.Rows)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                item.Add("Category", dgv_BaicInfo.Name);
                foreach (DataGridViewColumn col in dgv_BaicInfo.Columns)
                {
                    item.Add(col.Name, row.Cells[col.Name].Value);
                }

                item.Add("MasterData", reference);

                string colName = row.Cells[Report.Designation.basic].Value?.ToString() ?? "new Parts";
                if (!exportData.ContainsKey(colName))
                {
                    exportData.Add(colName, new List<Dictionary<string, object>>());
                }
                exportData[colName].Add(item);

                DataFormatChange(dgv_Etc, ref exportData);

                //PCM Import 시 Config에 있는 모든 컬럼이 API에 포함되지 않으면 에러가 나는 버그가 있어서 임의로 모든 컬럼을 추가함.
                item.Add(Report.Cost.materailType,"");
                foreach (DataGridViewColumn col in dgv_Manufacturing.Columns)
                {
                    if (!item.ContainsKey(col.Name)) item.Add(col.Name, "");
                }
                item.Add(Report.Cost.shifts,"");
                item.Add(Report.Cost.workhour,"");
                item.Add(Report.Cost.marginRate, "");
                item.Add(Report.Cost.manufacturingOverheads, "");
                item.Add(Report.Cost.attendNum, "");
                item.Add(Report.Cost.setupLaborNum, "");
                item.Add(Report.Cost.setUpLabor, "");

                foreach (DataGridViewColumn col in dgv_Material.Columns)
                {
                    if (!item.ContainsKey(col.Name)) item.Add(col.Name, "");
                }

                foreach (DataGridViewColumn col in dgv_External.Columns)
                {
                    if (!item.ContainsKey(col.Name)) item.Add(col.Name, "");
                }

                foreach (DataGridViewColumn col in dgv_Etc.Columns)
                {
                    if(dgv_Etc.Rows.Count <= 0) item.Add(col.Name, "");
                    else item.Add(col.Name, dgv_Etc.Rows[0].Cells[col.Name].Value);
                }
            }
        }

        private void DataFormatChange(DataGridView dgv, ref Dictionary<string, List<Dictionary<string, object>>> exportData)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                item.Add("Category", dgv.Name);
                bool nullRowCheck = false;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.HeaderText.Contains("명") && row.Cells[col.Name].Value != null) nullRowCheck = true;
                    item.Add(col.Name, row.Cells[col.Name].Value);
                }

                string colName = dgv_BaicInfo.Rows[0].Cells[Report.Designation.basic].Value?.ToString() ?? "new Parts";
                if (!exportData.ContainsKey(colName)) exportData.Add(colName, new List<Dictionary<string, object>>());
                if(nullRowCheck) exportData[colName].Add(item);
            }
        }
        private void SubPartDataFormatChange(DataGridView dgv, ref Dictionary<string, List<Dictionary<string, object>>> exportData)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                frmCBD frm = new frmCBD();

                var delivery =
                    part[row.Cells["PartNumber"].Value?.ToString()].ToDictionary(pair => pair.Key, pair => pair.Value as Dictionary<string, object>);

                frm.part = delivery;
                frm.mode = "ExcelImport";
                frm.exportMode = exportMode;
                frm.ShowDialog();

                var subPart = frm.ReturnValue2;
                string colName = dgv_BaicInfo.Rows[0].Cells[Report.Designation.basic].Value?.ToString() ?? "new Parts";

                Dictionary<string, object> item = new Dictionary<string, object>();
                item.Add("Category", dgv.Name);
                bool nullRowCheck = false;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (row.Cells[col.Name].Value != null) nullRowCheck = true;
                    item.Add(col.Name, row.Cells[col.Name].Value);
                }
                item.Add("value", subPart);

                if (!exportData.ContainsKey(colName)) exportData.Add(colName, new List<Dictionary<string, object>>());               
                if (nullRowCheck) exportData[colName].Add(item);
            }            
        }

        private Dictionary<string, List<Dictionary<string, object>>> TcPCMExportFormatMatching()
        {
            Dictionary<string, List<Dictionary<string, object>>> exportData = new Dictionary<string, List<Dictionary<string, object>>>();
            AllDataFormatChange(ref exportData);
            DataFormatChange(dgv_Manufacturing, ref exportData);
            DataFormatChange(dgv_Material, ref exportData);
            SubPartDataFormatChange(dgv_SubPart, ref exportData);
            DataFormatChange(dgv_External, ref exportData);

            return exportData;
        }

        private void btn_ExportPCM_Click(object sender, EventArgs e)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();
            
            Dictionary<string, List<Dictionary<string, object>>> exportData = TcPCMExportFormatMatching();
            string result = import.Import(exportData, currentNode);
            if (result != null) CustomMessageBox.RJMessageBox.Show($"Error : {result}", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show($"CBD 내보내기가 완료되었습니다.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadingScreen.CloseSplashScreen();
        }

        private void dgv_SubPart_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmCBD frm = new frmCBD();

            var delivery =
                part[dgv_SubPart.Rows[e.RowIndex].Cells["PartNumber"].Value?.ToString()].ToDictionary(pair => pair.Key,
                pair => pair.Value is JObject ? ((JObject)pair.Value).ToObject<Dictionary<string, object>>() : pair.Value as Dictionary<string, object>);

            frm.part = delivery;
            frm.mode = mode;
            frm.exportMode = exportMode;
            frm.ShowDialog();
        }

        private void cb_Mode_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Mode.Checked)
            {
                cb_Mode.Image = Properties.Resources.간략1;
                exportMode = Bom.ExportLang.Eng;
            }
            else
            {
                cb_Mode.Image = Properties.Resources.상세1;
                exportMode = Bom.ExportLang.Kor;
            }
        }

        private void dgv_Manufacturing_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ((DataGridView)sender).Rows[e.RowIndex].Cells["No"].Value = e.RowIndex + 1;
        }

        private void dgv_BaicInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e, 0);
        }

        private void dgv_Manufacturing_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e, 1);
        }

        private void dgv_Summary_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e, 1);
        }
        private void dgv_SubPart_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e, 2);
        }

        private void dgv_ETC_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex].Name == "Value" || dgv.Columns[e.ColumnIndex].Name == "TotalValue"
                || dgv.Columns[e.ColumnIndex].Name == "overheads")
            {
                global.CommaAdd(e, 1);
            }
            else global.CommaAdd(e, 4);
        }

        private void dgv_BaicInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!btn_Import.Visible && dgv_BaicInfo.Columns[e.ColumnIndex].Name != Report.Designation.dateOfCalc) return;
            dgv_BaicInfo.Rows[0].Cells[Report.Designation.dateOfCalc].Value
                = DateTime.TryParse(dgv_BaicInfo.Rows[0].Cells[Report.Designation.dateOfCalc].Value?.ToString(), out DateTime date) 
                    ? date: DateTime.Now;

            assySorting["종합"] = dgv_BaicInfo;
        }

        private void dgv_Summary_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_Summary.Columns[e.ColumnIndex].Name != "계" && dgv_Summary.Columns[e.ColumnIndex].Name != "기타") return;

            dgv_Summary.Rows[0].Cells["부품가"].Value
                 = global.ConvertDoule(dgv_Summary.Rows[0].Cells["계"].Value) + global.ConvertDoule(dgv_Summary.Rows[0].Cells["재료관리비"].Value)
                  + global.ConvertDoule(dgv_Summary.Rows[0].Cells["관리비"].Value) + global.ConvertDoule(dgv_Summary.Rows[0].Cells["이윤"].Value)
                  + global.ConvertDoule(dgv_Summary.Rows[0].Cells["기타"].Value);

            foreach (DataGridViewColumn col in dgv_Summary.Columns)
            {
                if (col.Index == 0) continue;
                dgv_Summary.Rows[1].Cells[col.Index].Value 
                    = global.ConvertDoule(dgv_Summary.Rows[0].Cells[col.Index].Value) / global.ConvertDoule(dgv_Summary.Rows[0].Cells["부품가"].Value)*100;
            }

            assySorting["요약"] = dgv_Summary;
        }

        private void dgv_Material_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!btn_Import.Visible || reference==null) return;
            import.Material((DataGridView)sender, e, reference);
            MaterialSummary();

            assySorting["소재"] = dgv_Material;
        }

        private void dgv_External_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!btn_Import.Visible || reference == null) return;
            import.External((DataGridView)sender, e, reference);
            MaterialSummary();

            assySorting["외주"] = dgv_External;
        }

        private void dgv_Manufacturing_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!btn_Import.Visible || reference == null) return;
            import.Manufacturing((DataGridView)sender, e, reference);

            ManufacturingSummary();

            assySorting["공정"] = dgv_Manufacturing;
        }

        private void dgv_Etc_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dgv_Summary.Rows[0].Cells["기타"].Value
               = dgv_Etc.Columns.Cast<DataGridViewColumn>().Sum(t => Convert.ToDouble(dgv_Etc.Rows[0].Cells[t.Index].Value));

            assySorting["기타"] = dgv_Etc;
        }

        private void ManufacturingSummary()
        {
            dgv_Summary.Rows[0].Cells["노무비"].Value
                = dgv_Manufacturing.Rows.Cast<DataGridViewRow>().Sum(t => Convert.ToDouble(t.Cells["totalLabor"].Value));

            dgv_Summary.Rows[0].Cells["경비"].Value
                = dgv_Manufacturing.Rows.Cast<DataGridViewRow>().Sum(t => Convert.ToDouble(t.Cells["totalMachine"].Value));

            dgv_Summary.Rows[0].Cells["공정"].Value
                = global.ConvertDoule(dgv_Summary.Rows[0].Cells["노무비"].Value) + global.ConvertDoule(dgv_Summary.Rows[0].Cells["경비"].Value);

            ExcelExport excelExport = new ExcelExport();
            double overheads = reference != null ? reference[Report.Cost.overheadsPer] : excelExport.Average(masterData[Report.Cost.overheadsPer]) / 100; ;
            double profit = reference != null ? reference[Report.Cost.profitPer] : excelExport.Average(masterData[Report.Cost.profitPer]) / 100; ;

            dgv_Summary.Rows[0].Cells["관리비"].Value
                = global.ConvertDoule(dgv_Summary.Rows[0].Cells["공정"].Value) * overheads;

            dgv_Summary.Rows[0].Cells["이윤"].Value
                = (global.ConvertDoule(dgv_Summary.Rows[0].Cells["공정"].Value) + global.ConvertDoule(dgv_Summary.Rows[0].Cells["관리비"].Value))
                * profit;

            dgv_Summary.Rows[0].Cells["계"].Value
               = global.ConvertDoule(dgv_Summary.Rows[0].Cells["공정"].Value) + global.ConvertDoule(dgv_Summary.Rows[0].Cells["재료비"].Value);
        }

        private void MaterialSummary()
        {
            dgv_Summary.Rows[0].Cells["재료비"].Value 
                = dgv_SubPart.Rows.Cast<DataGridViewRow>().Sum(t => Convert.ToDouble(t.Cells["TotalValue"].Value))
                + dgv_Material.Rows.Cast<DataGridViewRow>().Sum(t => Convert.ToDouble(t.Cells["TotalValue"].Value))
                + dgv_External.Rows.Cast<DataGridViewRow>().Sum(t => Convert.ToDouble(t.Cells["TotalValue"].Value));

            dgv_Summary.Rows[0].Cells["재료관리비"].Value 
                = dgv_Material.Rows.Cast<DataGridViewRow>().Sum(t => Convert.ToDouble(t.Cells["overheads"].Value))
                + dgv_External.Rows.Cast<DataGridViewRow>().Sum(t => Convert.ToDouble(t.Cells["overheads"].Value));

            dgv_Summary.Rows[0].Cells["계"].Value
                = global.ConvertDoule(dgv_Summary.Rows[0].Cells["공정"].Value) + global.ConvertDoule(dgv_Summary.Rows[0].Cells["재료비"].Value);
        }

        private void dgv_Manufacturing_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dgv_Manufacturing.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
        }

        private void BasicInfoColumn()
        {
            dgv_BaicInfo.Columns.Clear();
            dgv_BaicInfo.Rows.Clear();

            if (btn_Import.Visible) dgv_BaicInfo.ReadOnly = false;

            dgv_BaicInfo.Columns.Add(Report.Designation.kindOfCar, "차종/제품");
            dgv_BaicInfo.Columns.Add(Report.Designation.status, "상태");
            dgv_BaicInfo.Columns.Add(Report.Designation.customer, "고객사");
            dgv_BaicInfo.Columns.Add(Report.Designation.basic, "부품명");
            dgv_BaicInfo.Columns.Add(Report.Designation.itemNumber, "도번");
            dgv_BaicInfo.Columns.Add(Report.Designation.supplier, "협력사");

            dgv_BaicInfo.Columns.Add(Report.Designation.modified, "작성자");

            CalendarColumn calendarDate = new CalendarColumn();
            calendarDate.Name = Report.Designation.modifiedDate;
            calendarDate.HeaderText = "작성일";
            calendarDate.DefaultCellStyle.Format = "yyyy-MM-dd";
            dgv_BaicInfo.Columns.Add(calendarDate);

            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = Report.Designation.dateOfCalc;
            calendar.HeaderText = "기준정보적용일";
            calendar.DefaultCellStyle.Format = "yyyy-MM-dd";
            dgv_BaicInfo.Columns.Add(calendar);

            dgv_BaicInfo.Columns.Add(Report.Cost.assyShift, "Shift Model");
            dgv_BaicInfo.Columns.Add(Report.Designation.productionHourPerYear, "Shift Model (h/year)");

            dgv_BaicInfo.Columns.Add(Report.Designation.region, "생산지");
            dgv_BaicInfo.Columns.Add(Report.Designation.productionStart, "생산 시작일");
            dgv_BaicInfo.Columns.Add(Report.Designation.lifeTime, "Life Time");
            dgv_BaicInfo.Columns.Add(Report.Designation.forcast, "Forecast(년평균)");
            dgv_BaicInfo.Columns.Add(Report.Designation.numberOfLot, "제조 Lot 수");
        }
        private void SubPartColumn()
        {
            dgv_SubPart.Columns.Clear();
            dgv_SubPart.Rows.Clear();

            if (btn_Import.Visible) dgv_SubPart.ReadOnly = false;

            dgv_SubPart.Columns.Add("No", "No");
            dgv_SubPart.Columns.Add(Report.Designation.basic, "구성품명");
            dgv_SubPart.Columns.Add(Report.Designation.substance, "재질/규격");
            dgv_SubPart.Columns.Add(Report.Cost.materialQuantityUnit, "단위");
            dgv_SubPart.Columns.Add(Report.Cost.materialCosts, "단가");
            dgv_SubPart.Columns.Add(Report.Cost.quantity, "소요량");
            dgv_SubPart.Columns.Add("PartNumber", "PartNumber");
            dgv_SubPart.Columns["PartNumber"].Visible = false;
            dgv_SubPart.Columns.Add("TotalValue", "계");
            dgv_SubPart.Columns["TotalValue"].ReadOnly = true;
        }
        private void MaterialColumn()
        {
            dgv_Material.Columns.Clear();
            dgv_Material.Rows.Clear();

            if (btn_Import.Visible) dgv_Material.ReadOnly = false;

            dgv_Material.Columns.Add("No", "No");
            dgv_Material.Columns.Add(Report.Designation.basic, "구성품명");
            dgv_Material.Columns.Add(Report.Designation.substance, "재질/규격");
            dgv_Material.Columns.Add(Report.Cost.density, "비중");
            dgv_Material.Columns.Add(Report.Cost.materialPriceUnit, "단위");
            dgv_Material.Columns.Add(Report.Cost.materialCosts, "재료단가");
            dgv_Material.Columns.Add(Report.Cost.valid, "재료단가 적용일");
            dgv_Material.Columns.Add(Report.Cost.length, "가로");
            dgv_Material.Columns.Add(Report.Cost.width, "세로");
            dgv_Material.Columns.Add(Report.Cost.thickness, "높이");
            dgv_Material.Columns.Add(Report.Cost.cavity, "Cavity");
            dgv_Material.Columns.Add(Report.Cost.quantity, "투입량");
            dgv_Material.Columns.Add(Report.Cost.netWeight, "Net");
            dgv_Material.Columns.Add(Report.Cost.loss, "Loss율");
            dgv_Material.Columns.Add(Report.Cost.scrapQuantity, "Scrap");
            dgv_Material.Columns[Report.Cost.scrapQuantity].ReadOnly = true;
            dgv_Material.Columns.Add(Report.Cost.scrapPrice, "Scrap단가");
            dgv_Material.Columns.Add(Report.Cost.lossPrice, Report.Cost.lossPrice);
            dgv_Material.Columns[Report.Cost.lossPrice].Visible = false;
            //dgv_Material.Columns.Add(Report.Cost.recycle, "회수율");
            dgv_Material.Columns.Add("Value", "소계");
            dgv_Material.Columns["Value"].ReadOnly = true;
            dgv_Material.Columns.Add("소요량", "소요량");
             dgv_Material.Columns["소요량"].Visible = false;
            dgv_Material.Columns.Add("TotalValue", "Total");
            dgv_Material.Columns["TotalValue"].ReadOnly = true;
            dgv_Material.Columns["TotalValue"].Visible = false;
            dgv_Material.Columns.Add("overheads", "재료 관리비");
            dgv_Material.Columns["overheads"].ReadOnly = true;
        }

        private void ExternalMaterialColumn()
        {
            dgv_External.Columns.Clear();
            dgv_External.Rows.Clear();

            if (btn_Import.Visible) dgv_External.ReadOnly = false;

            dgv_External.Columns.Add("No", "No");
            dgv_External.Columns.Add(Report.Designation.basic, "구성품명");
            dgv_External.Columns.Add(Report.Designation.substance, "재질/규격");
            dgv_External.Columns.Add(Report.Cost.materialQuantityUnit, "단위");
            dgv_External.Columns.Add(Report.Cost.materialCosts, "단가");
            dgv_External.Columns.Add(Report.Cost.quantity, "소요량");
            dgv_External.Columns.Add("TotalValue", "계");
            dgv_External.Columns["TotalValue"].ReadOnly = true;
            dgv_External.Columns.Add("overheads", "외주 재료 관리비");
            dgv_External.Columns["overheads"].ReadOnly = true;
        }

        private void ManufacturingColumn()
        {
            dgv_Manufacturing.Columns.Clear();
            dgv_Manufacturing.Rows.Clear();

            if (btn_Import.Visible) dgv_Manufacturing.ReadOnly = false;

            dgv_Manufacturing.Columns.Add(Report.Cost.manufacturingCategory, "구분");
            dgv_Manufacturing.Columns.Add(Report.Designation.basic, "공정명");
            dgv_Manufacturing.Columns.Add(Report.Designation.machine, "설비명");
            dgv_Manufacturing.Columns.Add("소요량", "소요량");
            dgv_Manufacturing.Columns["소요량"].Visible = false;
            dgv_Manufacturing.Columns.Add(Report.Cost.cycleTime, "Net C/T");
            dgv_Manufacturing.Columns.Add(Report.Cost.maunCavity, "Cavity");
            dgv_Manufacturing.Columns.Add(Report.Cost.setupTime, "준비시간");
            dgv_Manufacturing.Columns.Add(Report.Cost.lot, "Lot수량");
            dgv_Manufacturing.Columns[Report.Cost.lot].ReadOnly = true;
            dgv_Manufacturing.Columns.Add("CycleTime", "C/T");
            dgv_Manufacturing.Columns["CycleTime"].ReadOnly = true;
            dgv_Manufacturing.Columns.Add(Report.Cost.laborNum, "투입인원");
            dgv_Manufacturing.Columns.Add(Report.Cost.laborCosts, "임율");
            dgv_Manufacturing.Columns.Add("totalLabor", "노무비 계");
            dgv_Manufacturing.Columns["totalLabor"].ReadOnly = true;
            dgv_Manufacturing.Columns.Add(Report.Cost.acquisition, "설비가");
            dgv_Manufacturing.Columns.Add(Report.Cost.auxiliaryArea, "설치면적");
            dgv_Manufacturing.Columns.Add(Report.Cost.ratePower, "전력량");
            dgv_Manufacturing.Columns.Add(Report.Cost.utilizationPower, "전력소비율");
            dgv_Manufacturing.Columns.Add(Report.Cost.imputed, "기계상각비");
            dgv_Manufacturing.Columns[Report.Cost.imputed].ReadOnly = true;
            dgv_Manufacturing.Columns.Add(Report.Cost.space, "건물상각비");
            dgv_Manufacturing.Columns[Report.Cost.space].ReadOnly = true;
            dgv_Manufacturing.Columns.Add(Report.Cost.energyCostRate, "전력비");
            dgv_Manufacturing.Columns[Report.Cost.energyCostRate].ReadOnly = true;
            dgv_Manufacturing.Columns.Add(Report.Cost.maintance, "수선비");
            dgv_Manufacturing.Columns[Report.Cost.maintance].ReadOnly = true;

            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
            comboBoxColumn.FlatStyle = FlatStyle.Flat;
            comboBoxColumn.Name = Report.Designation.machineCategory;
            comboBoxColumn.HeaderText = "설비구분";
            comboBoxColumn.Items.Add("범용");
            comboBoxColumn.Items.Add("전용");
            comboBoxColumn.Items.Add("");
            dgv_Manufacturing.Columns.Add(comboBoxColumn);

            comboBoxColumn = new DataGridViewComboBoxColumn();
            comboBoxColumn.FlatStyle = FlatStyle.Flat;
            comboBoxColumn.Name = Report.Designation.shift;
            comboBoxColumn.HeaderText = "작업시간";
            comboBoxColumn.Items.Add("1Shift");
            comboBoxColumn.Items.Add("2Shift");
            dgv_Manufacturing.Columns.Add(comboBoxColumn);

            dgv_Manufacturing.Columns.Add(Report.Cost.machineCosts, "시간당경비 계");
            dgv_Manufacturing.Columns[Report.Cost.machineCosts].ReadOnly = true;
            dgv_Manufacturing.Columns.Add("totalMachine", "경비 계");
            dgv_Manufacturing.Columns["totalMachine"].ReadOnly = true;
        }

        private void EtcColumn()
        {
            dgv_Etc.Columns.Clear();
            dgv_Etc.Rows.Clear();

            if (btn_Import.Visible) dgv_Etc.ReadOnly = false;

            dgv_Etc.Columns.Add(Report.Cost.mold, "금형비");
            dgv_Etc.Columns.Add(Report.Cost.jig, "치공구비");
            dgv_Etc.Columns.Add(Report.Cost.transport, "물류비");
            dgv_Etc.Columns.Add(Report.Cost.package, "포장비");
        }

        #region 화면 이동 가능
        //METODO PARA REDIMENCIONAR/CAMBIAR TAMAÑO A FORMULARIO  TIEMPO DE EJECUCION ----------------------------------------------------------
        private int tolerance = 15;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));

            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);

            region.Exclude(sizeGripRectangle);
            this.panelContenedorPrincipal.Region = region;
            this.Invalidate();
        }
        //----------------COLOR Y GRIP DE RECTANGULO INFERIOR
        protected override void OnPaint(PaintEventArgs e)
        {

            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(55, 61, 69));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);

            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }
       
        //METODO PARA ARRASTRAR EL FORMULARIO---------------------------------------------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        //METODOS PARA CERRAR,MAXIMIZAR, MINIMIZAR FORMULARIO------------------------------------------------------
        int lx, ly;
        int sw, sh;

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            btnMaximizar.Visible = false;
            btnNormal.Visible = true;

        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
            btnNormal.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panelContenedorForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
