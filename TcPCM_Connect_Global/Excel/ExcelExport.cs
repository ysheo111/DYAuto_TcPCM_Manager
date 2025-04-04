using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using Excel = Microsoft.Office.Interop.Excel;

namespace TcPCM_Connect_Global
{
    public class ExcelExport
    {
        public Excel.Workbook WorkBook = null;
        public Excel.Application Apply = null;
        public string ExportLocationGrid(DataGridView dgv, string columnName)
        {
            Excel.Application application = null;
            Excel.Workbook workBook = null;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = ".xlsx|";
            dlg.FileName = $"{columnName}.xlsx";
            try
            {
                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return null;
                else if (dialog != DialogResult.OK) return $"Error : 저장위치가 올바르게 선택되지 않았습니다.";

                try
                {
                    application = (Excel.Application)Marshal.GetActiveObject("Excel.Application");
                }
                catch
                {
                    application = new Excel.Application();
                }
                Apply = application;
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일로부터 불러오기
                workBook = application.Workbooks.Add();// application.Workbooks.Open(dlg.FileName);
                WorkBook = workBook;

                //ExportSingleGridToSheet(workBook, dgv, columnName);

                //if (workBook.Sheets.Count > 0)
                //{
                //    Excel.Worksheet firstSheet = workBook.Sheets[1];
                //    firstSheet.Delete();
                //}
                Excel.Worksheet worksheet = workBook.Sheets[1];
                worksheet.Name = $"{columnName}";
                worksheet.Visible = Excel.XlSheetVisibility.xlSheetVisible;

                int lastCount = dgv.ColumnCount;
                if (columnName == "지역" || columnName == "단위")
                    lastCount--;
                else if (columnName == "업종")
                    lastCount = lastCount - 2;

                for (int i = 0; i < lastCount; i++)
                {
                    worksheet.Cells[2, i + 2] = dgv.Columns[i].Name;
                    worksheet.Cells[2, i + 2].Interior.Color = Excel.XlRgbColor.rgbLightGray;

                    for (int row = 1; row < dgv.RowCount; row++)
                    {
                        string input = dgv.Rows[row - 1].Cells[i].Value?.ToString();
                        if (!string.IsNullOrEmpty(input))
                            input = input.Replace("[DYA]", "");
                        worksheet.Cells[row + 2, i + 2] = input;
                    }
                }
                string lastColumnAlpha = GetExcelColumnName(worksheet.UsedRange.Columns.Count + 1);

                Excel.Range range = worksheet.Range[$"B2:{lastColumnAlpha}{dgv.RowCount + 1}"];
                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                worksheet.Columns.AutoFit();

                application.DisplayAlerts = false;
                workBook.SaveAs(dlg.FileName);
                application.DisplayAlerts = true;
            }
            catch (Exception exc)
            {
                if (workBook != null)
                {
                    //Excel 프로그램 종료
                    workBook.Close();
                    application.Quit();
                }
                return exc.Message;
            }
            return null;
        }
        private static string GetExcelColumnName(int columnNumber)
        {
            string columnName = string.Empty;
            while(columnNumber > 0)
            {
                //columnNumber--;
                int modulo = (columnNumber - 1) % 26;
                columnName = (char)('A' + modulo) + columnName; //Convert.ToChar(65 + modulo).ToString();// + columnName;
                columnNumber /= 26;
            }

            return columnName;
        }

        public string ExportWorkSheet(Excel.Workbook workBook, DataGridView dgv, string sheetName)
        {
            try
            {
                Excel.Worksheet worksheet = workBook.Sheets.Add(After: workBook.Sheets[workBook.Sheets.Count]);
                worksheet.Name = sheetName;
                worksheet.Visible = Excel.XlSheetVisibility.xlSheetVisible;

                int lastCount = dgv.ColumnCount;
                if (sheetName == "지역" || sheetName == "단위")
                    lastCount--;
                else if (sheetName == "업종")
                    lastCount = lastCount - 2;

                for (int i = 0; i < lastCount; i++)
                {
                    worksheet.Cells[2, i + 2] = dgv.Columns[i].Name;
                    worksheet.Cells[2, i + 2].Interior.Color = Excel.XlRgbColor.rgbLightGray;
                    worksheet.Cells[2, i + 2].NumberFormat = "@";

                    for (int row = 1; row < dgv.RowCount; row++)
                    {
                        string input = dgv.Rows[row - 1].Cells[i].Value?.ToString();
                        if (!string.IsNullOrEmpty(input))
                            input = input.Replace("[DYA]", "");
                        worksheet.Cells[row + 2, i + 2] = input;
                    }
                }
                string lastColumnAlpha = GetExcelColumnName(worksheet.UsedRange.Columns.Count + 1);

                Excel.Range range = worksheet.Range[$"B2:{lastColumnAlpha}{dgv.RowCount + 1}"];
                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                worksheet.Columns.AutoFit();

                Apply.DisplayAlerts = false;
                workBook.Save();
                Apply.DisplayAlerts = true;
            }
            catch(Exception exc)
            {
                if (workBook != null)
                {
                    //Excel 프로그램 종료
                    workBook.Close();
                    Apply.Quit();
                }
                return exc.Message;
            }
            return null;
        }

    }
}
