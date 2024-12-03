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
using System.Windows.Forms.DataVisualization.Charting;

namespace TcPCM_Connect
{
    public partial class frmChart : Form
    {
        Dictionary<string, Dictionary<string, DataGridView>> sortingData;

        //Constructor
        public frmChart()
        {
            InitializeComponent();
        }

        public frmChart(Dictionary<string, Dictionary<string, DataGridView>> partData)
        {
            InitializeComponent();
            sortingData = partData;
        }

        private void frmChart_Load(object sender, EventArgs e)
        {
            CustomColor.GetChartColor(chart1);
            CustomColor.GetChartColor(chart2);

            chart1.Series.Clear();

            foreach (var item in sortingData)
            {
                double prevValue = 0;
                string x = item.Value["종합"].Rows[0].Cells[Report.Designation.basic].Value?.ToString();

                DataGridView dgv_summary = item.Value["요약"];
                foreach (DataGridViewColumn col in dgv_summary.Columns)
                {
                    if (col.HeaderText.Contains("계") || col.HeaderText.Contains("구분")) continue;

                    if (chart1.Series.IsUniqueName(col.Name))
                    {
                        chart1.Series.Add(col.Name);
                        chart1.Series[col.Name].ChartType = SeriesChartType.RangeColumn;
                    }

                    if (col.HeaderText != "부품가") chart1.Series[col.Name].Points.AddXY(x, global.ConvertDouble(dgv_summary.Rows[0].Cells[col.Name].Value) + prevValue, prevValue);
                    else chart1.Series[col.Name].Points.AddXY(x, global.ConvertDouble(dgv_summary.Rows[0].Cells[col.Name].Value));
                    prevValue += global.ConvertDouble(dgv_summary.Rows[0].Cells[col.Name].Value);
                }
            }
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult result = chart1.HitTest(e.X, e.Y);            

            string properties = "";
            if (result.Series==null) return;

            if (result.Series.Name == "재료비" || result.Series.Name == "재료관리비")
            {
                properties = "재료";
            }
            else if (result.Series.Name == "하위파트")
            {
                properties = "하위파트";
            }
            else if (result.Series.Name == "Setup" || result.Series.Name == "경비" || result.Series.Name == "노무비")
            {
                properties = "공정";
            }
            else if (result.Series.Name == "관리비" || result.Series.Name == "이윤" || result.Series.Name == "기타")
            {
                properties = "기타";
            }
            else return;

            chart2.Series.Clear();

            foreach (var item in sortingData)
            {
                string x = item.Value["종합"].Rows[0].Cells[Report.Designation.basic].Value?.ToString();

                DataGridView dgv = item.Value[properties];

                if (properties == "기타")
                {
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Name == Report.Cost.materialOverheads) continue;

                        if (chart2.Series.IsUniqueName(col.Name))
                        {
                            chart2.Series.Add(col.Name);
                            chart2.Series[col.Name].IsValueShownAsLabel = true;
                        }
                        chart2.Series[col.Name].Points.AddXY(x, Math.Round(global.ConvertDouble(dgv.Rows[0].Cells[col.Name].Value), 2));
                    }
                }
                else
                {
                    c1FlexGrid1.Rows.Count = 1;
                    c1FlexGrid1.Cols.Count = 0;

                    c1FlexGrid1.Cols.Count = dgv.Columns.Count;
                    c1FlexGrid1.Rows.Fixed = 1;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        string seriesName = row.Cells[1].Value?.ToString();
                        if (seriesName == null) continue;
                        if (chart2.Series.IsUniqueName(seriesName))
                        {
                            chart2.Series.Add(seriesName);
                            chart2.Series[seriesName].IsValueShownAsLabel = true;
                        }

                        double y = global.ConvertDouble(row.Cells[dgv.Columns.Count - 1].Value);
                        c1FlexGrid1.Rows.Add();
                        foreach (DataGridViewColumn col in dgv.Columns)
                        {
                            if (!c1FlexGrid1.Cols.Contains(col.Name))
                            {
                                //c1FlexGrid1.SetData(0, col.Index, dgv.Columns[col.Index].HeaderText);
                                c1FlexGrid1.Cols[col.Index].Name = dgv.Columns[col.Index].Name;
                                c1FlexGrid1.Cols[col.Index].Caption = dgv.Columns[col.Index].HeaderText;
                            }
                            object ifDoubleRoundValue = row.Cells[col.Index].Value is double ? Math.Round(global.ConvertDouble(row.Cells[col.Index].Value), 2) : row.Cells[col.Index].Value;
                            c1FlexGrid1.SetData(c1FlexGrid1.Rows.Count - 1, col.Index, ifDoubleRoundValue);
                        }

                        chart2.Series[seriesName].Points.AddXY(x, Math.Round(y, 2));
                    }
                }
            }

            //foreach (var item in partData)
            //{
            //    string x = item["기본"][Report.Designation.basic]?.ToString();
            //    DataGridView dgv = sortingData[item["기본"]["PartNumber"]?.ToString()][properties];

            //    if (properties == "기타")
            //    {
            //        foreach (DataGridViewColumn col in dgv.Columns)
            //        {
            //            if (col.Name == Report.Cost.materialOverheads) continue;

            //            if (chart2.Series.IsUniqueName(col.Name))
            //            {
            //                chart2.Series.Add(col.Name);
            //                chart2.Series[col.Name].IsValueShownAsLabel = true;
            //            }
            //            chart2.Series[col.Name].Points.AddXY(x, Math.Round(global.ConvertDouble(dgv.Rows[0].Cells[col.Name].Value),2));
            //        }
            //    }
            //    else
            //    {
            //        c1FlexGrid1.Rows.Count = 1;
            //        c1FlexGrid1.Cols.Count = 0;

            //        c1FlexGrid1.Cols.Count = dgv.Columns.Count;
            //        c1FlexGrid1.Rows.Fixed = 1;
            //        foreach (DataGridViewRow row in dgv.Rows)
            //        {
            //            string seriesName = row.Cells[1].Value?.ToString();
            //            if (chart2.Series.IsUniqueName(seriesName))
            //            {
            //                chart2.Series.Add(seriesName);
            //                chart2.Series[seriesName].IsValueShownAsLabel = true;
            //            }

            //            double y = global.ConvertDouble(row.Cells[dgv.Columns.Count - 1].Value);
            //            if (properties == "재료")
            //            {
            //                double materialOverhead =
            //                  global.ConvertDouble(sortingData[item["기본"]["PartNumber"]?.ToString()]["기타"].Rows[0].Cells[Report.Cost.materialOverheads].Value);

            //                y = y * (1+materialOverhead / 100);

            //            }

            //            c1FlexGrid1.Rows.Add();
            //            foreach (DataGridViewColumn col in dgv.Columns)
            //            {                       
            //                if(!c1FlexGrid1.Cols.Contains(col.Name))
            //                {
            //                    //c1FlexGrid1.SetData(0, col.Index, dgv.Columns[col.Index].HeaderText);
            //                    c1FlexGrid1.Cols[col.Index].Name = dgv.Columns[col.Index].Name;
            //                    c1FlexGrid1.Cols[col.Index].Caption = dgv.Columns[col.Index].HeaderText;
            //                }
            //                object ifDoubleRoundValue = row.Cells[col.Index].Value is double ? Math.Round(global.ConvertDouble(row.Cells[col.Index].Value), 2) : row.Cells[col.Index].Value;
            //                c1FlexGrid1.SetData(c1FlexGrid1.Rows.Count - 1, col.Index, ifDoubleRoundValue);
            //            }

            //            chart2.Series[seriesName].Points.AddXY(x, Math.Round(y,2));
            //        }

            //    }

            //}
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
