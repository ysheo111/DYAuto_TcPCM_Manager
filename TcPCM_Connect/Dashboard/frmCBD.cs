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
using System.IO;

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

        //public Dictionary<string, Dictionary<string, object>> part;
        public Dictionary<string, string> validFrom;
        
        public string mode, currentNode;
        public Bom.ExportLang exportMode=Bom.ExportLang.Kor;

        ExportCBD export = new ExportCBD();
        ImportCBD import = new ImportCBD();

        public Dictionary<string, DataGridView> assySorting = new Dictionary<string, DataGridView>();

        public Dictionary<string, DataGridView> ReturnValue1 { get; private set; }
        public Dictionary<string, List<Dictionary<string, object>>> ReturnValue2 { get; private set; }
        private void frmCBD_Load(object sender, EventArgs e)
        {
            BasicInfoColumn();
            SubPartColumn();
            MaterialColumn();
            ManufacturingColumn();
            EtcColumn();

            cb_Mode.Image = exportMode == Bom.ExportLang.Eng ? Properties.Resources.간략1 : Properties.Resources.상세1;
            cb_Mode.Checked = exportMode == Bom.ExportLang.Eng;

            dgv_Summary.Rows.Clear();

            if (mode.Contains("Export"))
            {
                dgv_Summary.Rows.Add();
                dgv_Summary.Rows[0].Cells["구분"].Value = "금액";
                dgv_Summary.Rows.Add();
                dgv_Summary.Rows[1].Cells["구분"].Value = "비율";

                if (this.Parent is frmCBD) this.Parent.Visible = false;

                btn_Import.Visible = false;
                btn_ExportPCM.Visible = false;

                //var test = assySorting["요약"].Rows[0];
                foreach (DataGridViewColumn col in dgv_Summary.Columns)
                {
                    if (assySorting["요약"].Columns.Contains(col.Name)) dgv_Summary.Rows[0].Cells[col.Name].Value = assySorting["요약"].Rows[0].Cells[col.Name].Value;
                }

                MappingDataGridView(dgv_Material, assySorting["재료"]);
                MappingDataGridView(dgv_Manufacturing , assySorting["공정"]);
                MappingDataGridView(dgv_SubPart , assySorting["하위파트"]);
                MappingDataGridView(dgv_BaicInfo , assySorting["종합"]);
                MappingDataGridView(dgv_Etc , assySorting["기타"]);
            }
            else
            {

            }
            //else
            //{
            //    assySorting.Add("재료", dgv_Material);
            //    assySorting.Add("하위파트", dgv_SubPart);
            //    assySorting.Add("종합", dgv_BaicInfo);
            //    assySorting.Add("기타", dgv_Etc);
            //    assySorting.Add("요약", dgv_Summary);
            //    assySorting.Add("공정", dgv_Manufacturing);
            //}
            //if (mode.Contains("Export"))
            //{
            //    btn_Import.Visible = false;
            //    btn_ExportPCM.Visible = false;
            //    port();
            //}
            //else if (mode.Contains("Import"))
            //{
            //    if (part != null)Import(part);
            //    btn_Export.Visible = false;
            //    this.ReturnValue2 = TcPCMExportFormatMatching();
            //}

            //this.ReturnValue1 = assySorting;

            //if (mode == "ExcelExportAll") btn_Export.PerformClick();
            //if (mode.Contains("Excel")) this.Close();
        }
        private void MappingImportDataGridView(DataGridView dgv, DataGridView sortedData)
        {
            dgv.Rows.Clear();
            foreach (DataGridViewRow row in sortedData.Rows)
            {
                int index = 0;
                dgv.Rows.Add();
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (!col.Visible) continue;
                    if (sortedData.Columns.Count < col.Index) break;
                    dgv.Rows[dgv.Rows.Count - 1].Cells[col.Index].Value = row.Cells[index].Value;
                    index++;
                }
            }
        }

        private void MappingDataGridView(DataGridView dgv, DataGridView sortedData)
        {
            foreach (DataGridViewRow row in sortedData.Rows)
            {
                dgv.Rows.Add();
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Name == "No") continue;
                    else if (sortedData.Columns.Contains(col.Name)) dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Value = row.Cells[col.Name].Value;
                }
            }
        }

        //private void MappingDataGridView(DataGridView dgv, DataGridView sortedData)
        //{
        //    foreach (DataGridViewRow row in sortedData.Rows)
        //    {
        //        dgv.Rows.Add();
        //        foreach (DataGridViewColumn col in dgv.Columns)
        //        {
        //            if (col.Name == "No") continue;
        //            else if (sortedData.Columns.Contains(col.Name)) dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Value = row.Cells[col.Name].Value;
        //        }
        //    }
        //}

        private void btn_Export_Click(object sender, EventArgs e)
        {
            ExcelExport export = new ExcelExport();
            string err = export.SingleFileExport(exportMode, $"{dgv_BaicInfo.Rows[0].Cells[Report.Designation.basic].Value}", assySorting);
            if (err != null) CustomMessageBox.RJMessageBox.Show($"Error : {err}", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show($"CBD 출력이 완료되었습니다.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void Import(Dictionary<string, Dictionary<string, object>> importPart)
        {
            //dgv_BaicInfo.Rows.Clear();
            //dgv_Etc.Rows.Clear();
            //dgv_SubPart.Rows.Clear();
            //dgv_Manufacturing.Rows.Clear();
            //dgv_Material.Rows.Clear();

            //dgv_BaicInfo.Rows.Add();
            //export.DataGridViewAdd(dgv_BaicInfo, part["기본"]);

            //dgv_Etc.Rows.Add();
            //export.DataGridViewAdd(dgv_Etc, part["기타"]);

            //foreach (var item in importPart)
            //{
            //    if (item.Key.Contains("종합") || item.Value == null) continue;
            //    else if (item.Key.Contains("하위"))
            //    {
            //        dgv_SubPart.Rows.Add();
            //        //export.SubPart(dgv_SubPart, item);
            //        export.DataGridViewAdd(dgv_SubPart, item.Value);
            //        dgv_SubPart.Rows[dgv_SubPart.Rows.Count - 1].Cells["PartNumber"].Value = item.Key;
            //    }
            //    else if (item.Key.Contains("공정")) import.DGVImport(dgv_Manufacturing, item.Value);
            //    else if (item.Key.Contains("소재")) import.DGVImport(dgv_Material, item.Value);
            //}

            //MaterialSummary();
            //ManufacturingSummary();
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
                 assySorting = result.Item2;

                MappingImportDataGridView(dgv_Material, assySorting["재료"]);
                MappingImportDataGridView(dgv_Manufacturing, assySorting["공정"]);
                MappingImportDataGridView(dgv_SubPart, assySorting["하위파트"]);
                MappingImportDataGridView(dgv_BaicInfo, assySorting["종합"]);
                MappingImportDataGridView(dgv_Summary, assySorting["요약"]);
                MappingImportDataGridView(dgv_Etc, assySorting["기타"]);
                //Import(result.Item2);
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
            //foreach (DataGridViewRow row in dgv.Rows)
            //{
            //    frmCBD frm = new frmCBD();

            //    var delivery =
            //        part[row.Cells["PartNumber"].Value?.ToString()].ToDictionary(pair => pair.Key, pair => pair.Value as Dictionary<string, object>);

            //    frm.part = delivery;
            //    frm.mode = "ExcelImport";
            //    frm.exportMode = exportMode;
            //    frm.ShowDialog();

            //    var subPart = frm.ReturnValue2;
            //    string colName = dgv_BaicInfo.Rows[0].Cells[Report.Designation.basic].Value?.ToString() ?? "new Parts";

            //    Dictionary<string, object> item = new Dictionary<string, object>();
            //    item.Add("Category", dgv.Name);
            //    bool nullRowCheck = false;
            //    foreach (DataGridViewColumn col in dgv.Columns)
            //    {
            //        if (row.Cells[col.Name].Value != null) nullRowCheck = true;
            //        item.Add(col.Name, row.Cells[col.Name].Value);
            //    }
            //    item.Add("value", subPart);

            //    if (!exportData.ContainsKey(colName)) exportData.Add(colName, new List<Dictionary<string, object>>());    Manufacturing           
            //    if (nullRowCheck) exportData[colName].Add(item);
            //}            
        }

        private Dictionary<string, List<Dictionary<string, object>>> TcPCMExportFormatMatching()
        {
            Dictionary<string, List<Dictionary<string, object>>> exportData = new Dictionary<string, List<Dictionary<string, object>>>();
            AllDataFormatChange(ref exportData);
            DataFormatChange(dgv_Manufacturing, ref exportData);
            DataFormatChange(dgv_Material, ref exportData);
            SubPartDataFormatChange(dgv_SubPart, ref exportData);
 
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
            frmDashboard frm = new frmDashboard();
            frm.LoadCBDForm(((Dictionary<string, object>)dgv_SubPart.Rows[e.RowIndex].Cells["PartNumber"].Value).ToDictionary(pair => pair.Key, pair => pair.Value as Dictionary<string, object>)); 
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

        private void dgv_BaicInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (!btn_Import.Visible && dgv_BaicInfo.Columns[e.ColumnIndex].Name != Report.Designation.dateOfCalc) return;
            //dgv_BaicInfo.Rows[0].Cells[Report.Designation.dateOfCalc].Value
            //    = DateTime.TryParse(dgv_BaicInfo.Rows[0].Cells[Report.Designation.dateOfCalc].Value?.ToString(), out DateTime date) 
            //        ? date: DateTime.Now;

            //assySorting["종합"] = dgv_BaicInfo;
        }

        private void dgv_Summary_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != 0 || e.ColumnIndex == 0 || dgv_Summary.Rows.Count < 2) return;

            //dgv_Summary.Rows[0].Cells["부품가"].Value
            //     = global.ConvertDouble(dgv_Summary.Rows[0].Cells["계"].Value)
            //      + global.ConvertDouble(dgv_Summary.Rows[0].Cells["관리비"].Value)
            //      + global.ConvertDouble(dgv_Summary.Rows[0].Cells["이윤"].Value)
            //      + global.ConvertDouble(dgv_Summary.Rows[0].Cells["기타"].Value);

            //foreach (DataGridViewColumn col in dgv_Summary.Columns)
            //{
            //    if (col.Index == 0) continue;
            //    dgv_Summary.Rows[1].Cells[col.Index].Value
            //        = global.ConvertDouble(dgv_Summary.Rows[0].Cells[col.Index].Value) / global.ConvertDouble(dgv_Summary.Rows[0].Cells["부품가"].Value) * 100;
            //}

            //assySorting["요약"] = dgv_Summary;
        }

        private void dgv_Material_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (!btn_Import.Visible) return;
            //import.Material((DataGridView)sender, e);
            //export.MaterialSummary();

            //assySorting["재료"] = dgv_Material;
        }

        private void dgv_Manufacturing_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (!btn_Import.Visible) return;
            //import.Manufacturing((DataGridView)sender, e);

            //export.ManufacturingSummary();

            //assySorting["공정"] = dgv_Manufacturing;
        }

        private void dgv_Etc_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //dgv_Summary.Rows[0].Cells["기타"].Value
            //   = dgv_Etc.Rows[0].Cells[Report.Cost.transport].Value;

            //assySorting["기타"] = dgv_Etc;
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

        private void dgv_Manufacturing_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dgv_Manufacturing.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
        }

        #region column 생성
        private void BasicInfoColumn()
        {
            dgv_BaicInfo.Columns.Clear();
            dgv_BaicInfo.Rows.Clear();

            if (btn_Import.Visible) dgv_BaicInfo.ReadOnly = false;

            DGVColumns columns = new DGVColumns();
            foreach (KeyValuePair<string, string> pair in columns.Basic())
            {
                if (pair.Key== Report.Designation.modifiedDate || pair.Key == Report.Designation.dateOfCalc)
                {
                    CalendarColumn calendarDate = new CalendarColumn();
                    calendarDate.Name = pair.Key;
                    calendarDate.HeaderText = pair.Value;
                    calendarDate.DefaultCellStyle.Format = "yyyy-MM-dd";
                    dgv_BaicInfo.Columns.Add(calendarDate);
                }
                else dgv_BaicInfo.Columns.Add(pair.Key, pair.Value);
            }
            dgv_BaicInfo.Columns["PartNumber"].Visible = false;
        }

        private void SubPartColumn()
        {
            dgv_SubPart.Columns.Clear();
            dgv_SubPart.Rows.Clear();

            if (btn_Import.Visible) dgv_SubPart.ReadOnly = false;

            DGVColumns columns = new DGVColumns();
            foreach (KeyValuePair<string, string> pair in columns.SubPart())
            {
                dgv_SubPart.Columns.Add(pair.Key, pair.Value);
            }
            dgv_SubPart.Columns["PartNumber"].Visible = false;
            dgv_SubPart.Columns["TotalValue"].ReadOnly = true;
        }

        private void MaterialColumn()
        {
            dgv_Material.Columns.Clear();
            dgv_Material.Rows.Clear();

            if (btn_Import.Visible) dgv_Material.ReadOnly = false;

            DGVColumns columns = new DGVColumns();
            foreach (KeyValuePair<string, string> pair in columns.Material())
            {
                dgv_Material.Columns.Add(pair.Key, pair.Value);
            }
            dgv_Material.Columns[Report.Cost.scrapQuantity].ReadOnly = true;
            dgv_Material.Columns[Report.Cost.lossPrice].Visible = false;
            dgv_Material.Columns[Report.Cost.totalMaterial].ReadOnly = true;
        }

        private void ManufacturingColumn()
        {
            dgv_Manufacturing.Columns.Clear();
            dgv_Manufacturing.Rows.Clear();

            if (btn_Import.Visible) dgv_Manufacturing.ReadOnly = false;

            DGVColumns columns = new DGVColumns();
            foreach (KeyValuePair<string, string> pair in columns.Manufacturing())
            {
                dgv_Manufacturing.Columns.Add(pair.Key, pair.Value);
            }

            dgv_Manufacturing.Columns[Report.Cost.imputed].ReadOnly = true;
            dgv_Manufacturing.Columns[Report.Cost.space].ReadOnly = true;
            dgv_Manufacturing.Columns[Report.Cost.energyCostRate].ReadOnly = true;
            dgv_Manufacturing.Columns[Report.Cost.maintance].ReadOnly = true;
            //dgv_Manufacturing.Columns[Report.Cost.totalMachinePerCycleTime].Visible = false;
            dgv_Manufacturing.Columns[Report.Cost.totalLabor].Visible = false;
            dgv_Manufacturing.Columns[Report.Cost.manufacturingCosts].ReadOnly = true;
        }

        public void EtcColumn()
        {
            dgv_Etc.Columns.Clear();
            dgv_Etc.Rows.Clear();

            if (btn_Import.Visible) dgv_Etc.ReadOnly = false;

            DGVColumns columns = new DGVColumns();
            foreach (KeyValuePair<string, string> pair in columns.Etc())
            {
                dgv_Etc.Columns.Add(pair.Key, pair.Value);
            }
        }
        #endregion

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
