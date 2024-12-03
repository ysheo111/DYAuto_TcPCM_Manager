using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using TcPCM_Connect_Global;
using Application = System.Windows.Forms.Application;
using Excel = Microsoft.Office.Interop.Excel;

namespace TcPCM_Connect
{
    public partial class frmCoreCalculator : Form
    {
       
        public frmCoreCalculator()
        {
            InitializeComponent();
        }        

        private void frmDashboard_Load(object sender, EventArgs e)
        {
         
        }

        private void ConfigureDataGridView(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //dgv.Columns[dgv.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.BorderStyle = BorderStyle.Fixed3D;
            for(int i=0; i<dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells[dgv.Columns.Count - 1].ReadOnly = true;
            }

    

            //dgv.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 10);
        }

        private void DefaultCellStyle(DataGridView dgv)
        {
            // 첫 번째 컬럼(예: 텍스트 컬럼)에 대해서는 포맷 설정을 하지 않습니다.
            dgv.Columns[0].DefaultCellStyle.Format = ""; // 빈 문자열로 설정하거나 생략할 수 있습니다.

            // 나머지 숫자 컬럼에 대해서만 포맷 설정을 합니다.
            for (int i = 1; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].DefaultCellStyle.Format = "#,###,##0.##";
            }
        }


        private void btn_Date_Click(object sender, EventArgs e)
        {
            double start = global.ConvertDouble(TextBox_StartYear.Texts);
            double end = global.ConvertDouble(TextBox_EndYear.Texts);

            dgv_Line.Rows.Clear();
            dgv_Overheads.Rows.Clear();
            dgv_Production.Rows.Clear();

            dgv_Line.Columns.Clear();
            dgv_Overheads.Columns.Clear();
            dgv_Production.Columns.Clear();

            dgv_Line.Columns.Add("분류", "분류");
            dgv_Overheads.Columns.Add("분류", "분류");
            dgv_Production.Columns.Add("분류", "분류");

            dgv_Production.Rows.Add();
            dgv_Production.Rows[dgv_Production.Rows.Count - 1].Cells[0].Value = "LCR";
            dgv_Production.Rows.Add();
            dgv_Production.Rows[dgv_Production.Rows.Count - 1].Cells[0].Value = "MCR";

            dgv_Line.Rows.Add();
            dgv_Line.Rows[dgv_Line.Rows.Count - 1].Cells[0].Value = "LCR";
            dgv_Line.Rows.Add();
            dgv_Line.Rows[dgv_Line.Rows.Count - 1].Cells[0].Value = "MCR";

            dgv_Overheads.Rows.Add();
            dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 1].Cells[0].Value = "LCR Max. Capa.";
            dgv_Overheads.Rows.Add();
            dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 1].Cells[0].Value = "LCR 물량반영";
            dgv_Overheads.Rows.Add();
            dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 1].Cells[0].Value = "LCR 최대 Capa 대비 비율";


            dgv_Overheads.Rows.Add();
            dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 1].Cells[0].Value = "MCR Max. Capa.";
            dgv_Overheads.Rows.Add();
            dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 1].Cells[0].Value = "MCR 물량반영";
            dgv_Overheads.Rows.Add();
            dgv_Overheads.Rows[dgv_Overheads.Rows.Count - 1].Cells[0].Value = "MCR 최대 Capa 대비 비율";

            for (int i=0; i<=(end-start); i++)
            {
                dgv_Line.Columns.Add((start+i).ToString(), (start + i).ToString());
                dgv_Overheads.Columns.Add((start+i).ToString(), (start + i).ToString());
                dgv_Production.Columns.Add((start+i).ToString(), (start + i).ToString());

            }

            dgv_Overheads.Columns.Add("Total", "Total");
            dgv_Line.Columns.Add("Max", "Max");
            dgv_Production.Columns.Add("Total", "Total");

            DefaultCellStyle(dgv_Line);
            DefaultCellStyle(dgv_Production);
            DefaultCellStyle(dgv_Overheads);

            ConfigureDataGridView(dgv_Line);
            ConfigureDataGridView(dgv_Production);
            ConfigureDataGridView(dgv_Overheads);
        }

        private void dgv_Production_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_Production.Columns.Count == 0) return;
            double sum=0;
            for(int i=1; i< dgv_Production.Columns.Count-1; i++)
            {
                sum += global.ConvertDouble(dgv_Production.Rows[e.RowIndex].Cells[i].Value);
            }
            dgv_Production.Rows[e.RowIndex].Cells[dgv_Production.Columns.Count - 1].Value = sum;

            ConfigureDataGridView(dgv_Line);
            ConfigureDataGridView(dgv_Production);
            ConfigureDataGridView(dgv_Overheads);

            dgv_Line_CellEndEdit(dgv_Line, new DataGridViewCellEventArgs(1, e.RowIndex));
        }

        private void dgv_Line_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_Line.Columns.Count == 0) return;
            double max = 0;
            for (int i = 1; i < dgv_Line.Columns.Count - 1; i++)
            {
                if (global.ConvertDouble(dgv_Production.Rows[e.RowIndex].Cells[i].Value) == 0)
                {
                    dgv_Line.Rows[e.RowIndex].Cells[i].Value = 0;
                }
                else
                {
                    dgv_Line.Rows[e.RowIndex].Cells[i].Value =
                                         Math.Truncate( global.ConvertDouble(dgv_Production.Rows[e.RowIndex].Cells[i].Value)
                                          / global.ConvertDouble(e.RowIndex == 0 ? TextBox_LCR.Texts : TextBox_MCR.Texts)) + 1;
                }
                max = Math.Max(global.ConvertDouble(dgv_Line.Rows[e.RowIndex].Cells[i].Value), max);
            }
            dgv_Line.Rows[e.RowIndex].Cells[dgv_Line.Columns.Count - 1].Value = max;

            ConfigureDataGridView(dgv_Line);
            ConfigureDataGridView(dgv_Production);
            ConfigureDataGridView(dgv_Overheads);

            dgv_Overheads_CellEndEdit(dgv_Overheads, new DataGridViewCellEventArgs(1, e.RowIndex));
        }

        private void dgv_Overheads_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_Overheads.Columns.Count == 0) return;
            bool lcr = (e.RowIndex + 1) / 3 < 1;
            double sum = 0, sum2 = 0;
            for (int i = 1; i < dgv_Overheads.Columns.Count - 1; i++)
            {
                dgv_Overheads.Rows[e.RowIndex * 3].Cells[i].Value = 
                     global.ConvertDouble(dgv_Line.Rows[e.RowIndex].Cells[i].Value)
                    * global.ConvertDouble(lcr ? TextBox_LCR.Texts : TextBox_MCR.Texts);

                dgv_Overheads.Rows[e.RowIndex*3+1].Cells[i].Value =  global.ConvertDouble(dgv_Production.Rows[e.RowIndex].Cells[i].Value);
                dgv_Overheads.Rows[e.RowIndex * 3 + 2].Cells[i].Value = global.ConvertDouble(dgv_Overheads.Rows[e.RowIndex * 3+1].Cells[i].Value) / global.ConvertDouble(dgv_Overheads.Rows[e.RowIndex * 3].Cells[i].Value) * 100;
                sum += global.ConvertDouble(dgv_Overheads.Rows[e.RowIndex * 3].Cells[i].Value);
                sum2 += global.ConvertDouble(dgv_Overheads.Rows[e.RowIndex * 3+1].Cells[i].Value);
            }
            
            dgv_Overheads.Rows[e.RowIndex * 3].Cells[dgv_Overheads.Columns.Count - 1].Value = sum;
            dgv_Overheads.Rows[e.RowIndex * 3 + 1].Cells[dgv_Overheads.Columns.Count - 1].Value = sum2;

            dgv_Overheads.Rows[e.RowIndex * 3 + 2].Cells[dgv_Overheads.Columns.Count - 1].Value  = sum- sum2;

            TextBox_LCRCost.Texts = 
                Math.Round(( global.ConvertDouble( dgv_Overheads.Rows[2].Cells[dgv_Overheads.Columns.Count - 1].Value)/
                global.ConvertDouble(dgv_Production.Rows[0].Cells[dgv_Overheads.Columns.Count - 1].Value)*100),2).ToString();

            TextBox_MCRCost.Texts =
                Math.Round((global.ConvertDouble(dgv_Overheads.Rows[5].Cells[dgv_Overheads.Columns.Count - 1].Value) /
                global.ConvertDouble(dgv_Production.Rows[1].Cells[dgv_Overheads.Columns.Count - 1].Value)*100),2).ToString();



            ConfigureDataGridView(dgv_Line);
            ConfigureDataGridView(dgv_Production);
            ConfigureDataGridView(dgv_Overheads);
        }


        private void TextBox_CycleTime__TextChanged(object sender, EventArgs e)
        {
            TextBox_LCR.Texts
              = (global.ConvertDouble(TextBox_Day.Texts) *
              (global.ConvertDouble(TextBox_Hour.Texts) * 60 * 60 / global.ConvertDouble(TextBox_CycleTime.Texts))).ToString();

            TextBox_MCR.Texts = (global.ConvertDouble(TextBox_LCR.Texts) / 270 * 6 * 48).ToString();

            dgv_Line_CellEndEdit(dgv_Line, new DataGridViewCellEventArgs(1, 0));
            dgv_Line_CellEndEdit(dgv_Line, new DataGridViewCellEventArgs(1, 1));
        }

        private void TextBox_StartYear_Validated(object sender, EventArgs e)
        {
            CustomControls.RJControls.RJTextBox textBox = (CustomControls.RJControls.RJTextBox)sender;
            if (!double.TryParse(textBox.Text, out double num)) return;
            textBox.Text = string.Format("{0:#,##0.##}", double.Parse(textBox.Text));
        }

        private void TextBox_LCR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) ||
                     e.KeyChar == Convert.ToChar(Keys.Back) ||
                     e.KeyChar == '.' ||
                     (e.KeyChar == 3 && ModifierKeys == Keys.Control) ||  // Ctrl+C
                     (e.KeyChar == 1 && ModifierKeys == Keys.Control)))   // Ctrl+A
            {
                e.Handled = true;
            }
        }

        private void TextBox_LCRCost_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void TextBox_StartYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자와 백스페이스만 입력
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void dgv_Production_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0) return;
            DataGridView dgv = (DataGridView)sender;                
            if (double.TryParse(e.Value?.ToString(), out double value))
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    e.Value = 0.0;
                }
                e.CellStyle.Format = "#,###,##0.##";
            }
            else if (e.Value is string)
            {
                e.Value = 0.0;
            }
            else if (e.Value is null)
            {
                e.Value = 0.0;
            }
            //e.FormattingApplied = true;           
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

        }

        private void btn_Load_Click(object sender, EventArgs e)
        {

        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;

            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = ".xlsx|";
                dlg.FileName = $"코어물량반영.xlsx";

                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return;
                else if (dialog != DialogResult.OK)
                {
                    CustomMessageBox.RJMessageBox.Show($"Error : 저장위치가 올바르게 선택되지 않았습니다.", "포장비&물류비", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (File.Exists(dlg.FileName))
                {
                    try
                    {
                        File.Delete(dlg.FileName);
                    }
                    catch
                    {
                        CustomMessageBox.RJMessageBox.Show($"Error : 기존 파일을 삭제하는데 실패했습니다.프로그램이 사용중인 경우 다른 파일명을 해 주시기 바립니다.", "포장비&물류비", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                //Excel 프로그램 실행
                application = new Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일만들기                
                workBook = application.Workbooks.Add();

                // Add a new worksheet
                Excel.Worksheet worksheet = workBook.Worksheets[1];

                ExcelExport excel = new ExcelExport();
                double maxCol = 0;
                int fraction = 2, col = 2;
                worksheet.Cells[fraction++, col].Value = label1.Text;
                excel.SaveDataGridViewToWorksheet(dgv_Production, worksheet, fraction++, col);
                fraction += dgv_Production.RowCount+1;
                maxCol = Math.Max(maxCol, dgv_Production.ColumnCount+ col);

                worksheet.Cells[fraction++, col].Value = label4.Text;
                worksheet.Cells[fraction, col++].Value = label11.Text;
                worksheet.Cells[fraction, col++].Value = TextBox_Day.Texts;
                col++;
                worksheet.Cells[fraction, col++].Value = label12.Text;
                worksheet.Cells[fraction, col++].Value = TextBox_Hour.Texts;
                col++;
                worksheet.Cells[fraction, col++].Value = label55.Text;
                worksheet.Cells[fraction++, col++].Value = TextBox_CycleTime.Texts;
                maxCol = Math.Max(maxCol, col);

                col = 2;
                worksheet.Cells[fraction, col++].Value = label6.Text;
                worksheet.Cells[fraction, col++].Value = TextBox_LCR.Texts;
                worksheet.Cells[fraction, col++].Value = label5.Text;
                col++;
                worksheet.Cells[fraction, col++].Value = label8.Text;
                worksheet.Cells[fraction, col++].Value = TextBox_MCR.Texts;
                worksheet.Cells[fraction++, col++].Value = label7.Text;
                maxCol = Math.Max(maxCol, col);

                col = 2;                
                worksheet.Cells[fraction++, col].Value = label2.Text;
                excel.SaveDataGridViewToWorksheet(dgv_Line, worksheet, fraction++, col);
                fraction += dgv_Line.RowCount +1;

                col = 2;               
                worksheet.Cells[fraction++, col].Value = label3.Text + " ";
                excel.SaveDataGridViewToWorksheet(dgv_Overheads, worksheet, fraction++, col);
                fraction += dgv_Overheads.RowCount + 1;

                col = 2;
                worksheet.Cells[fraction++, col].Value = label10.Text;
                worksheet.Cells[fraction, col++].Value = label17.Text;
                worksheet.Cells[fraction, col++].Value = TextBox_LCRCost.Texts;
                worksheet.Cells[fraction, col++].Value = label9.Text;
                col++;
                worksheet.Cells[fraction, col++].Value = label18.Text;
                worksheet.Cells[fraction, col++].Value = TextBox_MCRCost.Texts;
                worksheet.Cells[fraction, col++].Value = label19.Text;
                maxCol = Math.Max(maxCol, col);

                Excel.Range usedRange = worksheet.Range[
                    worksheet.Cells[1, 1],
                    worksheet.Cells[fraction + 1, maxCol + 1]
                ];

                usedRange.Columns.AutoFit();
                // Save the workbook as a new file
                workBook.SaveAs(dlg.FileName);

            }
            catch (Exception ex)
            {
                CustomMessageBox.RJMessageBox.Show($"Error : {ex.Message}", "포장비&물류비", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

    }
}
