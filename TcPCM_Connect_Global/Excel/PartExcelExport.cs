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
    class PartExcelExport
    {
        public string Export(Bom.ExportLang lang, string fileLocation, Dictionary<string, Part> parts)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;

            try
            {
                foreach (KeyValuePair<string, Part> part in parts)
                {
                    File.Copy($@"{Application.StartupPath}\부품원가계산서.xlsx", $@"{fileLocation}\부품원가계산서_{part.Value.header.partName}.xlsx", true);
                    //Excel 프로그램 실행
                    application = new Excel.Application();
                    //Excel 화면 띄우기 옵션
                    application.Visible = true;
                    //파일로부터 불러오기                
                    workBook = application.Workbooks.Open($@"{fileLocation}\부품원가계산서_{part.Value.header.partName}.xlsx");
                    CBDMatching(lang, workBook, part.Value);
                }
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
            finally
            {
                if (workBook != null)
                {
                    //변경점 저장하면서 닫기
                    workBook.Save();

                    ////Excel 프로그램 종료
                    //workBook.Close();
                    //application.Quit();

                    ////오브젝트 해제1
                    //ExcelCommon.ReleaseExcelObject(workBook);
                    //ExcelCommon.ReleaseExcelObject(application);
                }
            }

            return null;
        }

        public void CBDMatching(Bom.ExportLang lang, Excel.Workbook workbook, Part part)
        {
            try
            {
                string sheetName = (lang == Bom.ExportLang.Kor ? "부품원가계산서" : "Quotation");
                string sheetName2 = (lang == Bom.ExportLang.Kor ? "제조경비 산출근거(변경 기준)" : "Quotation");
                Excel.Worksheet worksheet = workbook.Sheets[sheetName];
                Excel.Worksheet worksheet2 = workbook.Sheets[sheetName2];
                worksheet.Select();
                //CBD의 기본정보
                worksheet.get_Range("B1", "B1").Select();

                //Excel의 사용범위를 읽어옴
                Excel.Range range = worksheet.UsedRange;

                int lastCol = 49;

                int excelOrder = 0;
                //Basic
                int row = 2, excelCol = 3;
                worksheet.Cells[row++, excelCol].Value = part.header.modelName?.Replace("[DYA]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.partNumber?.Replace("[DYA]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.partName?.Replace("[DYA]", "");

                worksheet.Cells[row++, excelCol].Value = part.header.company?.Replace("[DYA]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.customer?.Replace("[DYA]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.currency?.Replace("[DYA]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.transport?.Replace("[DYA]", "");

                row = 5; excelCol = 6;
                string category = part.header.category != null ? part.header.category?.Split('-')[0].Replace(" ", "").Replace("[DYA]", "") : "";
                worksheet.Cells[row++, excelCol].Value = category;
                worksheet.Cells[row++, excelCol].Value = part.header.suppier?.Replace("[DYA]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.exchangeRate;
                worksheet.Cells[row++, excelCol].Value = part.header.exchangeRateCurrency?.Replace("[DYA]", "");

                row = 2; excelCol = 19;
                worksheet.Cells[row++, excelCol].Value = part.header.dateOfCalc.ToString("yyyy-MM-dd");
                worksheet.Cells[row++, excelCol].Value = part.header.author?.Replace("[DYA]", "");                

                //summary
                row = 13; excelCol = 8;
                worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.summary.administrationCosts);
                worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.summary.profit);
                worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.summary.materialOverhead);
                                                         
                row = 14; excelCol = 11;                 
                worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.summary.rnd);
                worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.summary.packageTransport);
                worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.summary.etc);

                //원/부재료           
                for (int i = 0; i < part.material.Count; i++)
                {
                    row = 25 + i;
                    excelCol = 2;
                    Excel.Range cell = worksheet.Cells[row, excelCol] as Excel.Range;
                    cell.Select();                    
                    worksheet.Cells[row, excelCol++].Value = i + 1;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].name?.Replace("[DYA]", "");
                    excelCol++;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].itemNumber?.Replace("[DYA]", "");
                    worksheet.Cells[row, excelCol++].Value = "";//공급기준
                    worksheet.Cells[row, excelCol++].Value = part.material[i].substance?.Replace("[DYA]", "");
                    worksheet.Cells[row, excelCol++].Value = "";//두께
                    worksheet.Cells[row, excelCol++].Value = "";//가로
                    worksheet.Cells[row, excelCol++].Value = "";//세로
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].netWeight);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].grossWeight);
                    worksheet.Cells[row, excelCol++].Value = part.material[i].qunantityUnit;
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].unitCost);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].quantity);
                    worksheet.Cells[row, excelCol++].Formula = part.material[i].grossWeight != null ? $"=IFERROR(L{row}*N{row}*O{row}, \"\")" : $"=IFERROR(N{row}*O{row}, \"\")";
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].scrapUnitPrice);
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR((L{row}-K{row})*Q{row}*O{row}, \"\")";
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].trash);
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR(P{row}-R{row}+S{row}, \"\")";
                }

                //가공비        
                for (int i = 0; i < part.manufacturing.Count; i++)
                {
                    row = 57 + i;
                    int row2 = 8 + i;
                    excelCol = 2;
                    Excel.Range cell = worksheet.Cells[row, excelCol] as Excel.Range;
                    cell.Select();
                    worksheet.Cells[row, excelCol++].Value = i + 1;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].partName?.Replace("[DYA]", "");
                    excelCol++;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].itemNumber?.Replace("[DYA]", "");
                    category = part.manufacturing[i].category != null ? part.manufacturing[i].category?.Split('-')[0].Replace(" ", "").Replace("[DYA]", "") : "";
                    worksheet.Cells[row, excelCol++].Value = category;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineName?.Replace("[DYA]", "");
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].workers);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].cycletime);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].cavity);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].quantity);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].utillization);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].grossWage);
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR(H{row}*I{row}*K{row}*M{row}/(J{row}*60*60*L{row}), \"\")";
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR('{sheetName2}'!AB{row2}, \"\")";
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR(O{row}*K{row}*I{row}/(60*60*L{row}*J{row}), \"\")";

                    excelCol = 2;
                    worksheet2.Cells[row2, excelCol++].Value = i + 1;
                    worksheet2.Cells[row2, excelCol++].Value = part.manufacturing[i].partName?.Replace("[DYA]", "");
                    worksheet2.Cells[row2, excelCol++].Value = part.manufacturing[i].category?.Replace("[DYA]", "");
                    worksheet2.Cells[row2, excelCol++].Value = part.manufacturing[i].machineName?.Replace("[DYA]", "");
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].productionDay);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].productionTime / part.manufacturing[i].productionDay);

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machineCost);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].amotizingYearOfMachine);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(H{row2}/I{row2}/F{row2}/G{row2}, \"\")";

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machineArea);
                    if (part.manufacturing[i].spaceCost != 0)
                    {
                        worksheet2.Range[worksheet2.Cells[row2, excelCol], worksheet2.Cells[row2, excelCol + 2]].Merge();
                        worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].spaceCost);
                        excelCol += 2;
                        worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(K{row2}*L{row2}/F{row2}/G{row2}, \"\")";
                    }
                    else
                    {
                        excelCol += 3;
                        worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(K{row2}*L{row2}*M{row2}/N{row2}/F{row2}/G{row2}, \"\")";
                    }

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].ratioOfMachineRepair);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR((J{row2}+O{row2})*P{row2}, \"\")";

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machinePower);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machinePowerCost);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machinePowerEfficiency);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(R{row2}*S{row2}*T{row2}, \"\")";

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].otherMachineCost);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].otherYearOfMachine);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(V{row2}/(W{row2}*F{row2})/G{row2}, \"\")";

                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(J{row2}+O{row2}+Q{row2}+U{row2}+X{row2}, \"\")";

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].redirectExpenseRatio);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(Y{row2}*Z{row2}, \"\")";

                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(ROUND(Y{row2}+AA{row2},0), \"\")";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.StackTrace}");
            }
        }


        private void ExcelSet(int i, int headerSize, int excelOrder, int lastRow, int lastCol, List<int> marker, List<int> colList, DataGridView dgv, Excel.Worksheet worksheet)
        {
            int row = marker[excelOrder] + i + headerSize;
            ((Excel.Range)worksheet.Cells[row, 2]).Select();

            if (i >= lastRow)
            {
                ((Excel.Range)worksheet.Rows[row]).Insert(Missing.Value, worksheet.Rows[row - 1]);
                for (int j = excelOrder + 1; j < marker.Count; j++)
                {
                    marker[j]++;
                }
            }

            int useColIdx = 0;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (!col.Visible) continue;
                if (colList.Count - 1 < useColIdx) continue;

                worksheet.Cells[row, colList[useColIdx]].Value = dgv.Rows[i].Cells[col.Name].Value;

                int merge = colList.Count > useColIdx + 1 ? colList[useColIdx + 1] : lastCol;
                if (colList[useColIdx] != merge) worksheet.Range[worksheet.Cells[row, colList[useColIdx]], worksheet.Cells[row, merge - 1]].Merge();
                useColIdx++;
            }
        }

        public string SingleFileExport(Bom.ExportLang exportMode, Dictionary<string, Part> parts, Bom.ManufacturingType type)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = ".xlsx|";
            dlg.FileName = $"{ parts.First().Value.header.partName}_표준원가분석.xlsx";

            DialogResult dialog = dlg.ShowDialog();
            if (dialog == DialogResult.Cancel) return null;
            else if (dialog != DialogResult.OK) return $"Error : 저장위치가 올바르게 선택되지 않았습니다.";

            if (File.Exists(dlg.FileName))
            {
                try
                {
                    File.Delete(dlg.FileName);
                }
                catch
                {
                    return "기존 파일을 삭제하는데 실패했습니다.프로그램이 사용중인 경우 다른 파일명을 해 주시기 바립니다";
                }
            }

            string err = Export(exportMode, dlg.FileName, parts);
            if (err != null) return $"Error : {err}";
            else return null;
        }

        public double Average(List<double> values)
        {
            return values.Count() > 0 ? Math.Round(values.ToArray().Average()) : 0;
        }

        public void SaveDataGridViewToWorksheet(DataGridView dgv, Excel.Worksheet worksheet, int startRow, int startCol)
        {
            // Add the headers
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                worksheet.Cells[startRow, startCol + i].Value = dgv.Columns[i].HeaderText;
            }

            // Add the data rows
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    object value = dgv.Rows[i].Cells[j].Value;
                    if (double.TryParse(value?.ToString(), out double result))
                    {
                        if (double.IsNaN(result) || double.IsInfinity(result))
                        {
                            value = null;
                        }
                    }

                    worksheet.Cells[startRow + i + 1, startCol + j].Value = value;
                }
            }

            // Get the range of the filled cells
            Excel.Range usedRange = worksheet.Range[
                worksheet.Cells[startRow, startCol],
                worksheet.Cells[startRow + dgv.Rows.Count, startCol + dgv.Columns.Count - 1]
            ];

            // Add borders to all cells in the used range
            usedRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            usedRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
        }




    }
}
