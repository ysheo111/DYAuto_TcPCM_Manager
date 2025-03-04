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
using System.Threading;
using System.Text;

namespace TcPCM_Connect_Global
{
    public class PartExcel
    {
        public string Export(Bom.ExportLang lang, string fileLocation, Dictionary<string, Part> parts)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
            string err = "";
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

                    string e = CBDMatching(lang, workBook, part.Value);
                    if (e != null) err += e;
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

            return err.Length > 0 ? err : null;
        }

        public string CBDMatching(Bom.ExportLang lang, Excel.Workbook workbook, Part part)
        {
            try
            {
                string sheetName = (lang == Bom.ExportLang.Kor ? "부품원가계산서" : lang == Bom.ExportLang.Eng ? "Part Price cost(ENG)" : "Part Price cost(CHN)");
                string sheetName2 = (lang == Bom.ExportLang.Kor ? "제조경비 산출근거" : lang == Bom.ExportLang.Eng ? "Manufaturing cost(ENG)" : "Manufaturing cost(CHN)");

                Excel.Worksheet worksheet = workbook.Sheets[sheetName];
                Excel.Worksheet worksheet2 = workbook.Sheets[sheetName2];

                foreach (Excel.Worksheet sheet in workbook.Sheets)
                {
                    if (sheet != worksheet && sheet != worksheet2) sheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                }
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
                    worksheet.Cells[row, excelCol++].Value = part.material[i].transport;//공급기준
                    worksheet.Cells[row, excelCol++].Value = part.material[i].substance?.Replace("[DYA]", "");
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].thickness);//두께
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].width);//가로
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].length);//세로
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].netWeight);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].grossWeight);
                    worksheet.Cells[row, excelCol++].Value = part.material[i].qunantityUnit;
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].unitCost);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].quantity);
                    worksheet.Cells[row, excelCol++].Formula = part.material[i].grossWeight != null ? $"=IFERROR(L{row}*N{row}*O{row}, 0)" : $"=IFERROR(N{row}*O{row}, 0)";
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].scrapUnitPrice);
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR((L{row}-K{row})*Q{row}*O{row}, 0)";
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.material[i].trash);
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR(P{row}-R{row}+S{row}, 0)";
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

                    string[] machineName = part.manufacturing[i].machineName?.Replace("[DYA]", "").Split('_');
                    worksheet.Cells[row, excelCol++].Value = machineName[0];

                    string sec = (machineName.Length >= 2 ? machineName[1] : "");
                    int start = 2;
                    if (!double.TryParse(sec, out double num)) start--;
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(num);

                    string etc = "";
                    for (int cnt = start; cnt < machineName.Length; cnt++)
                    {
                        etc += machineName[cnt];
                    }
                    worksheet.Cells[row, excelCol++].Value = etc;

                    if (part.manufacturing[i].price != 0)
                    {
                        worksheet.Cells[row, 18].Value = global.ZeroToNull(part.manufacturing[i].price);
                        worksheet.Cells[row, 13].Value = global.ZeroToNull(part.manufacturing[i].quantity);
                        continue;
                    }

                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].workers);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].cycletime);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].cavity);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].quantity);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].utillization);
                    worksheet.Cells[row, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].grossWage);
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR(J{row}*K{row}*M{row}*O{row}/(L{row}*60*60*N{row}), 0)";
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR('{sheetName2}'!AD{row2}, 0)";
                    worksheet.Cells[row, excelCol++].Formula = $"=IFERROR(Q{row}*M{row}*K{row}/(60*60*N{row}*L{row}), 0)";

                    excelCol = 2;
                    worksheet2.Cells[row2, excelCol++].Value = i + 1;
                    worksheet2.Cells[row2, excelCol++].Value = part.manufacturing[i].partName?.Replace("[DYA]", "");
                    worksheet2.Cells[row2, excelCol++].Value = category;

                    worksheet2.Cells[row2, excelCol++].Value = machineName[0];
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(num);
                    worksheet2.Cells[row2, excelCol++].Value = etc;

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].productionDay);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].productionTime / part.manufacturing[i].productionDay);

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machineCost);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].amotizingYearOfMachine);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(J{row2}/K{row2}/H{row2}/I{row2}, 0)";

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machineArea);
                    if (part.manufacturing[i].spaceCost != 0)
                    {
                        worksheet2.Range[worksheet2.Cells[row2, excelCol], worksheet2.Cells[row2, excelCol + 2]].Merge();
                        worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].spaceCost);
                        excelCol += 2;
                        worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(M{row2}*N{row2}/H{row2}/I{row2}, 0)";
                    }
                    else
                    {
                        excelCol += 3;
                        worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(M{row2}*N{row2}*O{row2}/P{row2}/H{row2}/I{row2}, 0)";
                    }

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].ratioOfMachineRepair);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR((L{row2}+Q{row2})*R{row2}, 0)";

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machinePower);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machinePowerCost);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].machinePowerEfficiency);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(T{row2}*U{row2}*V{row2}, 0)";

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].otherMachineCost);
                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].otherYearOfMachine);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(X{row2}/(Y{row2}*H{row2})/I{row2}, 0)";

                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(L{row2}+Q{row2}+S{row2}+W{row2}+Z{row2}, 0)";

                    worksheet2.Cells[row2, excelCol++].Value = global.ZeroToNull(part.manufacturing[i].redirectExpenseRatio);
                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(AA{row2}*AB{row2}, 0)";

                    worksheet2.Cells[row2, excelCol++].Formula = $"=IFERROR(ROUND(AA{row2}+AC{row2},0), 0)";
                }
            }
            catch (Exception e)
            {
                return $"{e.Message}";
            }
            return null;
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


        public string Import(string tagetType, double tagetID, string mode)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
            ExcelImport excel = new ExcelImport();
            string err = null;
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                int cnt = 0;
                dlg.Multiselect = true;
                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return null;
                else if (dialog != DialogResult.OK) return $"Error : 파일 오픈에 실패하였습니다.";

                if (mode == "자동")
                {
                    Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
                    splashthread.IsBackground = true;
                    splashthread.Start();
                }
                foreach (string file in dlg.FileNames)
                {
                    cnt++;
                    Thread.Sleep(100);
                    LoadingScreen.UdpateStatusTextWithStatus($"{cnt}/{dlg.FileNames.Length}");
                    //Excel 프로그램 실행
                    application = new Microsoft.Office.Interop.Excel.Application();
                    //파일로부터 불러오기
                    workBook = application.Workbooks.Open(file);

                    List<string> workSheetList = new List<string>();
                    foreach (Excel.Worksheet sheet in workBook.Worksheets)
                    {
                        if (sheet.Visible != Excel.XlSheetVisibility.xlSheetVisible) continue;
                        workSheetList.Add(sheet.Name);
                    }
                    string val = "";
                    string val2 = "";
                    if (mode == "자동")
                    {
                        val = workSheetList[workSheetList.Count - 2];
                        val2 = workSheetList[workSheetList.Count - 1];
                        application.Visible = false;
                    }
                    else
                    {
                        //Excel 화면 띄우기 옵션
                        application.Visible = true;

                        frmPartWorkSheetSelect workSheetSelect = new frmPartWorkSheetSelect();
                        workSheetSelect.workSheet = workSheetList;
                        if (workSheetSelect.ShowDialog() == DialogResult.Cancel) return null;
                        val = workSheetSelect.ReturnValue1;
                        val2 = workSheetSelect.ReturnValue2;
                    }
                    Excel.Worksheet worksheet = workBook.Worksheets.Item[val];
                    Excel.Worksheet worksheet2 = workBook.Worksheets.Item[val2];

                    worksheet.Activate();

                    //CBD의 기본정보
                    worksheet.get_Range("G2", "G2").Select();

                    JArray data = new JArray();
                    JObject header = new JObject();
                    JArray manufacturings = new JArray();
                    //Basic
                    int row = 2, excelCol = 2;
                    header.Add(Report.LineType.lineType, "M");
                    header.Add(Report.LineType.level, 1);
                    header.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase");
                    header.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Benchmarkcalculation");
                    string segment = "";
                    List<string> colName = excel.cbd.column.Values.ToList();
                    for (int i = 0; i < 13; i++)
                    {
                        int nameRow = i < 9 ? 1 : 2;  // Conditional assignment for nameRow

                        if (i == 11)
                        {
                            string date;
                            try
                            {
                                date = worksheet.Cells[row, excelCol + 1].Value.ToString("yyyy-MM-dd");
                            }
                            catch (Exception ex)
                            {
                                date = DateTime.Now.ToString("yyyy-MM-dd");
                                Console.WriteLine($"Error retrieving date: {ex.Message}");  // Log error message
                            }
                            excel.CellVaildation(colName[i], nameRow, row++, excelCol, worksheet, date, ref header, row - 1);
                        }
                        else if (i == 10) header.Add(Report.Header.exchangeRateCurrency, worksheet.Cells[row++, excelCol + 1].Value);
                        else if (colName[i] == Report.Header.category) segment = $"{worksheet.Cells[row++, excelCol + 1].Value}";
                        else
                        {
                            excel.CellVaildation(colName[i], nameRow, row, excelCol, row++, excelCol + 1, worksheet, ref header, row - 1);
                        }

                        if (i == 6)  // Adjusted condition to avoid unnecessary checks
                        {
                            excelCol = 5;
                            row = 5;
                        }
                        if (i == 10)
                        {
                            excelCol = 18;
                            row = 2;
                        }

                    }
                    header.Add(Report.Header.category, $"{header[Report.Header.suppier]}||{segment}");
                    excelCol = 8; row = 11;
                    for (int i = 13; i < 16; i++)
                    {
                        excel.CellVaildation(colName[i], 2, row, excelCol, row + 2, excelCol++, worksheet, ref header, row);
                    }
                    for (int i = 16; i < 19; i++)
                    {
                        excel.CellVaildation(colName[i], 3, row, excelCol, row + 3, excelCol++, worksheet, ref header, row);
                    }
                    header.Add(Report.LineType.comment, $"{worksheet.Cells[14, 15].Value}");
                    string query = $@"select Value as name from DoubleDefaultValues as a 
                                    join Properties as b on a.PropertyIdentifier = b.Guid
                                    where b.Name = 'ManualUtilizationRate' and a.ConfigurationId = 2

                                    union all

                                    select[반영율] as name
                                    FROM[PCI].[dbo].[Sprue]
                                    where[업종] like N'%{segment}%'";

                    List<string> properies = global_DB.ListSelect(query, (int)global_DB.connDB.PCMDB);

                    //원/부재료
                    int materialDefault = 3;
                    JArray materials = new JArray();
                    int cntMaterial = 0;
                    try
                    {
                        while (true)
                        {
                            int j = 25 + cntMaterial;
                            cntMaterial++;
                            worksheet.get_Range($"C{j}", $"C{j}").Select();
                            JObject part = new JObject();
                            JObject material = new JObject();
                            JObject scrap = new JObject();
                            string sprue = (properies.Count < 2 ? null : (global.ConvertDouble(properies[1]) * 100).ToString());
                            part.Add("기타2", sprue);
                            int colIndex = 17, nameRow = 3;
                            int[] values = { 11, 12, 13, 14, 17 };

                            double unit = 1, net = global.ConvertDouble(worksheet.Cells[j, 11].Value);

                            if (((Excel.Range)worksheet.Cells[j, 2]).Value?.ToString().Contains("가  공  비") == true) break;
                            if (((Excel.Range)worksheet.Cells[j, 3]).Value == null) continue;

                            if (worksheet.Cells[j, 5].Value?.ToString().Contains("외주") == true)
                            {
                                JObject manufacturing = new JObject();

                                manufacturing.Add(Report.Manufacturing.sequence, j * 10);
                                manufacturing.Add(Report.LineType.lineType, "F");
                                manufacturing.Add(Report.LineType.level, "2");
                                manufacturing.Add(Report.Header.partName, header[Report.Header.partName]);
                                manufacturing.Add(Report.Header.suppier, header[Report.Header.suppier]);
                                manufacturing.Add(Report.Header.currency, header[Report.Header.currency]);
                                manufacturing.Add(Report.Manufacturing.externalPrice, worksheet.Cells[j, 20].Value);
                                manufacturing.Add(Report.Manufacturing.externalQuntity, 1);
                                manufacturing.Add(Report.Manufacturing.partName, worksheet.Cells[j, 3].Value);
                                manufacturing.Add(Report.LineType.comment, worksheet.Cells[j, 21].Value);

                                manufacturings.Add(manufacturing);
                            }
                            else
                            {
                                for (int i = 3; i < 22; i++)
                                {
                                    if (i == 4 || i == 16 || i == 18 || i == 20)
                                    {
                                        colIndex--;
                                        continue;
                                    }
                                    excelCol = i;


                                    if (!values.Contains(i)) excel.CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref part, 22);
                                    else if (net != 0)
                                    {
                                        if (i == 14) excel.CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material, 22);
                                        else if (i == 17) excel.CellVaildation(Report.Material.rawMaterial, nameRow, 22, excelCol, j, excelCol, worksheet, ref scrap, 22);
                                        else if (i == 13)
                                        {
                                            excel.CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material, 22);
                                            excel.CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref scrap, 22);

                                            if (worksheet.Cells[j, excelCol].Value.ToString().ToLower().Contains("kg")) unit /= Math.Pow(10, 3);
                                            else if (worksheet.Cells[j, excelCol].Value.ToString().ToLower().Contains("t")) unit /= Math.Pow(10, 6);
                                        }
                                    }

                                }

                                part.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase");
                                part.Add(Report.LineType.level, "2");
                                part.Add(Report.Header.currency, header[Report.Header.currency]);
                                part.Add(Report.LineType.lineType, "M");
                                part.Add(Report.Header.dateOfCalc, header[Report.Header.dateOfCalc]);
                                part.Add(Report.Header.suppier, header[Report.Header.suppier]);
                                part.Add(Report.Header.category, header[Report.Header.category]);

                                if (net > 0)
                                {
                                    part.Add(Report.Material.netWeight, net / unit);
                                    excel.CellVaildation(Report.Material.grossWeight, nameRow, 22, 12, j, 12, worksheet, ref part, 22);
                                    part[Report.Material.grossWeight] = (global.ConvertDouble(part[Report.Material.grossWeight])) / unit;

                                    material.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");
                                    scrap.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");

                                    part.Add(Report.LineType.materials, "Siemens.TCPCM.Classification.Material.InjectionMoldingPart");
                                    material.Add(Report.LineType.materials, "Siemens.TCPCM.Classification.Material.RawMaterial.Plastic");
                                    scrap.Add(Report.LineType.materials, "Siemens.TCPCM.Classification.Material.Scrap");
                                    material.Add(Report.Header.partName, "RawMaterial");
                                    scrap.Add(Report.Header.partName, "Scrap");

                                    material.Add("WeightType", "Deployed weight");
                                    scrap.Add("WeightType", "Scrap / Waste");

                                    material.Add(Report.Header.dateOfCalc, header[Report.Header.dateOfCalc]);
                                    scrap.Add(Report.Header.dateOfCalc, header[Report.Header.dateOfCalc]);

                                    material.Add(Report.Header.suppier, header[Report.Header.suppier]);
                                    scrap.Add(Report.Header.suppier, header[Report.Header.suppier]);

                                    material.Add(Report.Header.category, header[Report.Header.category]);
                                    scrap.Add(Report.Header.category, header[Report.Header.category]);

                                    part.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Benchmarkcalculation");
                                    material.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Estimation(rough)");
                                    scrap.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Estimation(rough)");

                                    material.Add(Report.LineType.lineType, "M");
                                    scrap.Add(Report.LineType.lineType, "M");

                                    material.Add(Report.LineType.level, "3");
                                    scrap.Add(Report.LineType.level, "3");

                                    material.Add(Report.Header.currency, header[Report.Header.currency]);
                                    scrap.Add(Report.Header.currency, header[Report.Header.currency]);

                                    material.Add(Report.Header.partNumber, part[Report.Material.substance]);
                                    scrap.Add(Report.Header.partNumber, part[Report.Material.substance] + "_Scrap");

                                    materials.Add(part);
                                    materials.Add(material);
                                    materials.Add(scrap);
                                }

                                else
                                {
                                    part.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Estimation(rough)");
                                    excel.CellVaildation(Report.Material.unit, nameRow, 22, 13, j, 13, worksheet, ref part, 22);
                                    part[Report.Material.quantity]
                                        = global.ConvertDouble($"{worksheet.Cells[j, 12].Value}") * global.ConvertDouble(part[Report.Material.quantity]);
                                    excel.CellVaildation(Report.Material.rawMaterial, nameRow, 22, 14, j, 14, worksheet, ref part, 22);
                                    materials.Add(part);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        err = e.Message + "\n";
                    }

                    MemberInfo[] manufacturingMembers = typeof(Report.Manufacturing).GetMembers(BindingFlags.Static | BindingFlags.Public);
                    MemberInfo[] headerMembers = typeof(Report.Header).GetMembers(BindingFlags.Static | BindingFlags.Public);
                    MemberInfo[] materialMembers = typeof(Report.Material).GetMembers(BindingFlags.Static | BindingFlags.Public);
                    MemberInfo[] summary = typeof(Report.Summary).GetMembers(BindingFlags.Static | BindingFlags.Public);

                    MemberInfo[] total = new MemberInfo[manufacturingMembers.Length + headerMembers.Length + materialMembers.Length + summary.Length];
                    manufacturingMembers.CopyTo(total, 0);
                    headerMembers.CopyTo(total, manufacturingMembers.Length);
                    materialMembers.CopyTo(total, manufacturingMembers.Length + headerMembers.Length);
                    summary.CopyTo(total, manufacturingMembers.Length + headerMembers.Length + materialMembers.Length);

                    foreach (MemberInfo member in total)
                    {
                        string key = ((FieldInfo)member).GetValue(member.Name)?.ToString();
                        if (!header.ContainsKey(key)) header.Add(key, null);
                    }
                    header.Add("기타2", "");
                    header.Add("WeightType", "");
                    data.Add(header);
                    data.Merge(materials);

                    int[] manufacturingList = { 12, 17, 19, 23, 26, 27, 29 };
                    int manufacturingDefault = 3;

                    int cntJ = 0;
                    cntMaterial += 25;
                    while (true)
                    {
                        int j = cntMaterial + 2 + cntJ;
                        int machinLine = 8 + cntJ;
                        cntJ++;

                        int colIndex = 32;
                        int nameRow = 2;

                        worksheet.get_Range($"C{j}", $"C{j}").Select();

                        if (((Excel.Range)worksheet.Cells[j, 3]).Value == null)
                        {
                            if (cntJ == 1) continue;
                            break;
                        }
                        JObject manufacturing = new JObject();
                        JObject machine = new JObject();
                        JObject otherMachine = new JObject();
                        JObject labor = new JObject();

                        manufacturing.Add(Report.Manufacturing.sequence, j * 10);
                        labor.Add(Report.Manufacturing.sequence, j * 10);
                        machine.Add(Report.Manufacturing.sequence, j * 10);

                        manufacturing.Add(Report.LineType.lineType, "D");
                        labor.Add(Report.LineType.lineType, "R");
                        machine.Add(Report.LineType.lineType, "A");

                        manufacturing.Add(Report.LineType.level, "2");
                        labor.Add(Report.LineType.level, "2");
                        machine.Add(Report.LineType.level, "2");

                        manufacturing.Add(Report.Header.partName, header[Report.Header.partName]);
                        labor.Add(Report.Header.partName, header[Report.Header.partName]);
                        machine.Add(Report.Header.partName, header[Report.Header.partName]);

                        manufacturing.Add(Report.Header.suppier, header[Report.Header.suppier]);

                        manufacturing.Add(Report.Header.currency, header[Report.Header.currency]);
                        labor.Add(Report.Header.currency, header[Report.Header.currency]);
                        machine.Add(Report.Header.currency, header[Report.Header.currency]);

                        if (worksheet.Cells[j, 5].Value?.ToString().Contains("외주") == true)
                        {
                            manufacturing[Report.LineType.lineType] = "F";
                            manufacturing.Add(Report.Manufacturing.partName, worksheet.Cells[j, 3].Value);
                            manufacturing.Add(Report.Manufacturing.externalPrice, worksheet.Cells[j, 18].Value);
                            manufacturing.Add(Report.Manufacturing.externalQuntity, 1);

                            manufacturings.Add(manufacturing);
                        }
                        else
                        {
                            for (int i = 3; i < 20; i++)
                            {
                                if (i == 4 || (16 <= i && i <= 18))
                                {
                                    colIndex--;
                                    continue;
                                }

                                else if (colName[colIndex + i] == Report.Manufacturing.category)
                                {
                                    excel.CellVaildation(colName[colIndex + i], nameRow, cntMaterial, i, worksheet, $"{header[Report.Header.suppier]}||{worksheet.Cells[j, i].Value}", ref manufacturing, 55);
                                    excel.CellVaildation(colName[colIndex + i], nameRow, cntMaterial, i, worksheet, $"{header[Report.Header.suppier]}||{worksheet.Cells[j, i].Value}", ref labor, 55);
                                }
                                else if (colName[colIndex + i] == Report.Manufacturing.machineName)
                                {
                                    continue;
                                }
                                else
                                {
                                    excel.CellVaildation(colName[colIndex + i], nameRow, cntMaterial, i, j, i, worksheet, ref manufacturing, 55);
                                    excel.CellVaildation(colName[colIndex + i], nameRow, cntMaterial, i, j, i, worksheet, ref labor, 55);
                                }

                                //excel.CellVaildation(colName[colIndex + i], nameRow, 55, i, j, i, worksheet, ref machine);
                            }

                            colIndex += 17;
                            nameRow = 4;

                            for (int i = 3; i < 30; i++)
                            {

                                try
                                {
                                    if (manufacturingList.Contains(i))
                                    {
                                        colIndex--;
                                        continue;
                                    }
                                    else if (colName[colIndex + i] == Report.Manufacturing.category)
                                    {
                                        excel.CellVaildation(colName[colIndex + i], nameRow, 4, i, worksheet2, $"{header[Report.Header.suppier]}||{worksheet2.Cells[machinLine, i].Value}", ref machine, 4);
                                    }
                                    else if (colName[colIndex + i] == Report.Manufacturing.machineName)
                                    {
                                        if (machine.ContainsKey(Report.Manufacturing.machineName))
                                        {
                                            machine[Report.Manufacturing.machineName] +=
                                                worksheet2.Cells[machinLine, i].Value == null ? "" : $"_{worksheet2.Cells[machinLine, i].Value.ToString()}";
                                        }
                                        else machine.Add(Report.Manufacturing.machineName, worksheet2.Cells[machinLine, i].Value?.ToString());
                                    }
                                    else if (machine.ContainsKey(Report.Manufacturing.rationForSupplementaryMachine3) && machine.ContainsKey(colName[colIndex + i])) continue;
                                    else if (colName[colIndex + i] == Report.Manufacturing.otherYearOfMachine)
                                    {
                                        machine.Add(Report.Manufacturing.otherYearOfMachine, $"{worksheet2.Cells[machinLine, i].Value}");
                                    }
                                    else if (machine.ContainsKey(colName[colIndex + i])) excel.CellVaildation(Report.Manufacturing.rationForSupplementaryMachine3, nameRow, 4, i, machinLine, i, worksheet2, ref machine, 4);
                                    else if (worksheet2.Cells[machinLine, i].Value == null) continue;
                                    else excel.CellVaildation(colName[colIndex + i], nameRow, 4, i, machinLine, i, worksheet2, ref machine, 4);
                                }
                                catch (Exception e)
                                {
                                    err += e.Message + "\n";
                                }

                            }

                            double rationForSupplementaryMachine = global.ConvertDouble(machine[Report.Manufacturing.rationForSupplementaryMachine3]) != 0 ?
                                   global.ConvertDouble(machine[Report.Manufacturing.rationForSupplementaryMachine1])
                               * global.ConvertDouble(machine[Report.Manufacturing.rationForSupplementaryMachine2])
                               / global.ConvertDouble(machine[Report.Manufacturing.rationForSupplementaryMachine3]) : 0;
                            machine.Add(Report.Manufacturing.spaceCost, rationForSupplementaryMachine);

                            //machine[Report.Manufacturing.otherMachineCost] 
                            //    = global.ConvertDouble(machine[Report.Manufacturing.otherMachineCost]) / global.ConvertDouble(machine[Report.Manufacturing.otherYearOfMachine])
                            //     / global.ConvertDouble(manufacturings[Report.Manufacturing.productionDay]) / global.ConvertDouble(manufacturings[Report.Manufacturing.productionTime]);
                            manufacturing.Add(Report.Manufacturing.redirectExpenseRatio, machine[Report.Manufacturing.redirectExpenseRatio]);
                            manufacturing.Add(Report.Manufacturing.productionDay, machine[Report.Manufacturing.productionDay]);
                            manufacturing.Add(Report.Manufacturing.productionTime,
                                global.ConvertDouble(machine[Report.Manufacturing.productionDay]) * global.ConvertDouble(machine[Report.Manufacturing.productionTime]));

                            manufacturings.Add(manufacturing);
                            manufacturings.Add(labor);
                            manufacturings.Add(machine);
                        }
                    }
                    data.Merge(manufacturings);
                    String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Import";
                    string response = WebAPI.POST(callUrl, new JObject
                    {
                        { "Data", data },
                        { "ConfigurationGuid", global_iniLoad.GetConfig("CBD", "Import") },
                        { "TargetType", tagetType},
                        { "TargetId",  tagetID.ToString()},
                    });

                    try
                    {
                        JObject postResult = JObject.Parse(response);
                        if ((bool)postResult["success"] == false)
                        {
                            err = postResult["message"].ToString();
                            MessageBox.Show(err);
                        }
                    }
                    catch
                    {
                        err += response;
                    }
                    data[0][Report.Header.partName] = $"표준_{data[0][Report.Header.partName]}";
                    string response2 = WebAPI.POST(callUrl, new JObject
                    {
                        { "Data", data },
                        { "ConfigurationGuid", global_iniLoad.GetConfig("CBD", "Import2") },
                        { "TargetType", tagetType},
                        { "TargetId",  tagetID.ToString()},
                    });

                    try
                    {
                        JObject postResult2 = JObject.Parse(response2);
                        if ((bool)postResult2["success"] == false)
                        {
                            err = postResult2["message"].ToString();
                            MessageBox.Show(err);
                        }
                    }
                    catch
                    {
                        err += response2;
                    }
                    finally
                    {
                        if (workBook != null)
                        {
                            //변경점 저장하면서 닫기
                            workBook.Close(true);
                            //Excel 프로그램 종료
                            application.Quit();
                            //오브젝트 해제1
                            ExcelCommon.ReleaseExcelObject(workBook);
                            ExcelCommon.ReleaseExcelObject(application);
                        }
                    }
                }

                LoadingScreen.CloseSplashScreen();
            }
            catch (Exception exc)
            {
                return exc.Message;
            }

            return err;
        }
        public string BulkImport(string mode)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
            ExcelImport excel = new ExcelImport();
            string err = null;
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                int cnt = 0;
                dlg.Multiselect = true;
                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return null;
                else if (dialog != DialogResult.OK) return $"Error : 파일 오픈에 실패하였습니다.";

                if (mode == "자동")
                {
                    Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
                    splashthread.IsBackground = true;
                    splashthread.Start();
                }
                foreach (string file in dlg.FileNames)
                {
                    cnt++;
                    Thread.Sleep(100);
                    LoadingScreen.UdpateStatusTextWithStatus($"{cnt}/{dlg.FileNames.Length}");
                    //Excel 프로그램 실행
                    application = new Microsoft.Office.Interop.Excel.Application();
                    //파일로부터 불러오기
                    workBook = application.Workbooks.Open(file);

                    List<string> workSheetList = new List<string>();
                    foreach (Excel.Worksheet sheet in workBook.Worksheets)
                    {
                        if (sheet.Visible != Excel.XlSheetVisibility.xlSheetVisible) continue;
                        workSheetList.Add(sheet.Name);
                    }
                    string val = "";
                    string val2 = "";
                    if (mode == "자동")
                    {
                        val = workSheetList[workSheetList.Count - 2];
                        val2 = workSheetList[workSheetList.Count - 1];
                        application.Visible = false;
                    }
                    else
                    {
                        //Excel 화면 띄우기 옵션
                        application.Visible = true;

                        frmPartWorkSheetSelect workSheetSelect = new frmPartWorkSheetSelect();
                        workSheetSelect.workSheet = workSheetList;
                        if (workSheetSelect.ShowDialog() == DialogResult.Cancel) return null;
                        val = workSheetSelect.ReturnValue1;
                        val2 = workSheetSelect.ReturnValue2;
                    }
                    Excel.Worksheet worksheet = workBook.Worksheets.Item[val];
                    Excel.Worksheet worksheet2 = workBook.Worksheets.Item[val2];

                    worksheet.Activate();

                    //CBD의 기본정보
                    worksheet.get_Range("G2", "G2").Select();

                    //Basic
                    int row = 2, excelCol = 2;
                    string segment = "";
                    List<string> colName = excel.cbd.column.Values.ToList();
                    DataTable header = new DataTable();
                    header.Rows.Add();

                    try
                    {
                        for (int i = 0; i < 13; i++)
                        {
                            int nameRow = i < 9 ? 1 : 2;  // Conditional assignment for nameRow

                            if (i == 11)
                            {
                                string date;
                                try
                                {
                                    date = worksheet.Cells[row, excelCol + 1].Value.ToString("yyyy-MM-dd");
                                }
                                catch (Exception ex)
                                {
                                    date = DateTime.Now.ToString("yyyy-MM-dd");
                                    Console.WriteLine($"Error retrieving date: {ex.Message}");  // Log error message
                                }
                                excel.CellVaildationDT(colName[i], nameRow, row++, excelCol, worksheet, date, ref header, row - 1);
                            }
                            else if (i == 10)
                            {
                                if (!header.Columns.Contains(Report.Header.exchangeRateCurrency)) header.Columns.Add(Report.Header.exchangeRateCurrency);
                                header.Rows[header.Rows.Count - 1][Report.Header.exchangeRateCurrency] = worksheet.Cells[row++, excelCol + 1].Value;
                            }
                            else if (colName[i] == Report.Header.category) segment = $"{worksheet.Cells[row++, excelCol + 1].Value}";
                            else
                            {
                                excel.CellVaildationDT(colName[i], nameRow, row, excelCol, row++, excelCol + 1, worksheet, ref header, row - 1);
                            }

                            if (i == 6)  // Adjusted condition to avoid unnecessary checks
                            {
                                excelCol = 5;
                                row = 5;
                            }
                            if (i == 10)
                            {
                                excelCol = 18;
                                row = 2;
                            }

                        }
                    }
                    catch(Exception ex22)
                    {
                        MessageBox.Show($"{file}\n{ex22.Message}");
                    }

                    if (!header.Columns.Contains(Report.Header.category)) header.Columns.Add(Report.Header.category);
                    header.Rows[header.Rows.Count - 1][Report.Header.category] = $"{header.Rows[header.Rows.Count - 1][Report.Header.suppier]}||{segment}";

                    excelCol = 8; row = 11;
                    for (int i = 13; i < 16; i++)
                    {
                        excel.CellVaildationDT(colName[i], 2, row, excelCol, row + 2, excelCol++, worksheet, ref header, row);
                    }
                    for (int i = 16; i < 19; i++)
                    {
                        excel.CellVaildationDT(colName[i], 3, row, excelCol, row + 3, excelCol++, worksheet, ref header, row);
                    }

                    if (!header.Columns.Contains(Report.LineType.comment)) header.Columns.Add(Report.LineType.comment);
                    header.Rows[header.Rows.Count - 1][Report.LineType.comment] = $"{worksheet.Cells[14, 15].Value}";

                    //원/부재료
                    int materialDefault = 3;
                    JArray materials = new JArray();
                    int cntMaterial = 0;

                    DataTable material = new DataTable();
                    DataTable manufacturing = new DataTable();
                    try
                    {
                        while (true)
                        {
                            int j = 25 + cntMaterial;
                            cntMaterial++;
                            worksheet.get_Range($"C{j}", $"C{j}").Select();

                            int colIndex = 17, nameRow = 3;
                            int[] values = { 11, 12, 13, 14, 17 };

                            double unit = 1, net = global.ConvertDouble(worksheet.Cells[j, 11].Value);

                            if (((Excel.Range)worksheet.Cells[j, 2]).Value?.ToString().Contains("가  공  비") == true) break;
                            if (((Excel.Range)worksheet.Cells[j, 3]).Value == null) continue;

                            if (worksheet.Cells[j, 5].Value?.ToString().Contains("외주") == true)
                            {
                                manufacturing.Rows.Add();

                                if (!manufacturing.Columns.Contains(Report.Manufacturing.externalPrice)) manufacturing.Columns.Add(Report.Manufacturing.externalPrice);
                                manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.externalPrice] = worksheet.Cells[j, 20].Value;

                                if (!manufacturing.Columns.Contains(Report.Manufacturing.externalQuntity)) manufacturing.Columns.Add(Report.Manufacturing.externalQuntity);
                                manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.externalQuntity] = 1;

                                if (!manufacturing.Columns.Contains(Report.Manufacturing.manufacturingName)) manufacturing.Columns.Add(Report.Manufacturing.manufacturingName);
                                manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.manufacturingName] = worksheet.Cells[j, 3].Value;

                                if (!manufacturing.Columns.Contains(Report.LineType.comment)) manufacturing.Columns.Add(Report.LineType.comment);
                                manufacturing.Rows[manufacturing.Rows.Count - 1][Report.LineType.comment] = worksheet.Cells[j, 21].Value;

                                //manufacturing.Add(Report.Manufacturing.sequence, j * 10);
                                //manufacturing.Add(Report.LineType.lineType, "F");
                                //manufacturing.Add(Report.LineType.level, "2");
                                //manufacturing.Add(Report.Header.partName, header[Report.Header.partName]);
                                //manufacturing.Add(Report.Header.suppier, header[Report.Header.suppier]);
                                //manufacturing.Add(Report.Header.currency, header[Report.Header.currency]);
                                //manufacturing.Add(Report.Manufacturing.externalPrice, worksheet.Cells[j, 20].Value);
                                //manufacturing.Add(Report.Manufacturing.externalQuntity, 1);
                                //manufacturing.Add(Report.Manufacturing.partName, worksheet.Cells[j, 3].Value);
                                //manufacturing.Add(Report.LineType.comment, worksheet.Cells[j, 21].Value);

                                //manufacturings.Add(manufacturing);
                            }
                            else
                            {
                                material.Rows.Add();

                                for (int i = 3; i < 22; i++)
                                {
                                    if (i == 4 || i == 16 || i == 18 || i == 20)
                                    {
                                        colIndex--;
                                        continue;
                                    }
                                    excelCol = i;


                                    if (!values.Contains(i)) excel.CellVaildationDT(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material, 22);
                                    else if (net != 0)
                                    {
                                        if (i == 14) excel.CellVaildationDT(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material, 22);
                                        else if (i == 17) excel.CellVaildationDT(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material, 22);
                                        else if (i == 13)
                                        {
                                            excel.CellVaildationDT(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material, 22);

                                            if (worksheet.Cells[j, excelCol].Value.ToString().ToLower().Contains("kg")) unit /= Math.Pow(10, 3);
                                            else if (worksheet.Cells[j, excelCol].Value.ToString().ToLower().Contains("t")) unit /= Math.Pow(10, 6);
                                        }
                                    }

                                }

                                if (net > 0)
                                {
                                    excel.CellVaildationDT(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material, 22);

                                    excel.CellVaildationDT(Report.Material.netWeight, nameRow, 22, 11, j, 11, worksheet, ref material, 22);
                                    excel.CellVaildationDT(Report.Material.grossWeight, nameRow, 22, 12, j, 12, worksheet, ref material, 22);
                                }

                                else
                                {
                                    excel.CellVaildationDT(Report.Material.unit, nameRow, 22, 13, j, 13, worksheet, ref material, 22);
                                    material.Rows[material.Rows.Count - 1][Report.Material.quantity]
                                        = global.ConvertDouble($"{worksheet.Cells[j, 12].Value}") * global.ConvertDouble(material.Rows[material.Rows.Count - 1][Report.Material.quantity]);
                                    excel.CellVaildationDT(Report.Material.rawMaterial, nameRow, 22, 14, j, 14, worksheet, ref material, 22);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        err = e.Message + "\n";
                    }

                    int[] manufacturingList = { 12, 17, 19, 23, 26, 27, 29 };
                    int manufacturingDefault = 3;

                    int cntJ = 0;
                    cntMaterial += 25;
                    while (true)
                    {
                        int j = cntMaterial + 2 + cntJ;
                        int machinLine = 8 + cntJ;
                        cntJ++;

                        int colIndex = 32;
                        int nameRow = 2;

                        worksheet.get_Range($"C{j}", $"C{j}").Select();

                        if (((Excel.Range)worksheet.Cells[j, 3]).Value == null)
                        {
                            if (cntJ == 1) continue;
                            break;
                        }
                        manufacturing.Rows.Add();
                        if (worksheet.Cells[j, 5].Value?.ToString().Contains("외주") == true)
                        {
                            //manufacturing[Report.LineType.lineType] = "F";
                            //manufacturing.Add(Report.Manufacturing.partName, worksheet.Cells[j, 3].Value);
                            //manufacturing.Add(Report.Manufacturing.externalPrice, worksheet.Cells[j, 18].Value);
                            //manufacturing.Add(Report.Manufacturing.externalQuntity, 1);

                            //manufacturings.Add(manufacturing);
                            
                            if (!manufacturing.Columns.Contains(Report.Manufacturing.externalPrice)) manufacturing.Columns.Add(Report.Manufacturing.externalPrice);
                            manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.externalPrice] = worksheet.Cells[j, 20].Value;

                            if (!manufacturing.Columns.Contains(Report.Manufacturing.externalQuntity)) manufacturing.Columns.Add(Report.Manufacturing.externalQuntity);
                            manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.externalQuntity] = 1;

                            if (!manufacturing.Columns.Contains(Report.Manufacturing.manufacturingName)) manufacturing.Columns.Add(Report.Manufacturing.manufacturingName);
                            manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.manufacturingName] = worksheet.Cells[j, 3].Value;

                            if (!manufacturing.Columns.Contains(Report.LineType.comment)) manufacturing.Columns.Add(Report.LineType.comment);
                            manufacturing.Rows[manufacturing.Rows.Count - 1][Report.LineType.comment] = worksheet.Cells[j, 21].Value;
                        }
                        else
                        {
                            for (int i = 3; i < 20; i++)
                            {
                                if (i == 4 || (16 <= i && i <= 18))
                                {
                                    colIndex--;
                                    continue;
                                }

                                else if (colName[colIndex + i] == Report.Manufacturing.category)
                                {
                                    excel.CellVaildationDT(colName[colIndex + i], nameRow, cntMaterial, i, worksheet, $"{header.Rows[header.Rows.Count - 1][Report.Header.suppier]}||{worksheet.Cells[j, i].Value}", ref manufacturing, 55);
                                }
                                else if (colName[colIndex + i] == Report.Manufacturing.machineName)
                                {
                                    continue;
                                }
                                else
                                {
                                    excel.CellVaildationDT(colName[colIndex + i], nameRow, cntMaterial, i, j, i, worksheet, ref manufacturing, 55);
                                }

                                //excel.CellVaildation(colName[colIndex + i], nameRow, 55, i, j, i, worksheet, ref machine);
                            }

                            colIndex += 17;
                            nameRow = 4;

                            for (int i = 3; i < 30; i++)
                            {

                                try
                                {
                                    if (manufacturingList.Contains(i))
                                    {
                                        colIndex--;
                                        continue;
                                    }
                                    else if (colName[colIndex + i] == Report.Manufacturing.category)
                                    {
                                        excel.CellVaildationDT(colName[colIndex + i], nameRow, 4, i, worksheet2, $"{header.Rows[header.Rows.Count - 1][Report.Header.suppier]}||{worksheet2.Cells[machinLine, i].Value}", ref manufacturing, 4);
                                    }
                                    else if (colName[colIndex + i] == Report.Manufacturing.machineName)
                                    {
                                        if (!manufacturing.Columns.Contains(Report.Manufacturing.machineName)) manufacturing.Columns.Add(Report.Manufacturing.machineName);
                                        if (manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.machineName] != DBNull.Value)
                                        {
                                            manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.machineName] +=
                                                worksheet2.Cells[machinLine, i].Value == null ? "" : $"_{worksheet2.Cells[machinLine, i].Value.ToString()}";
                                        }
                                        else
                                        {
                                            manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.machineName] = worksheet2.Cells[machinLine, i].Value;
                                        }
                                    }
                                    else if (manufacturing.Columns.Contains(Report.Manufacturing.rationForSupplementaryMachine3) &&  manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.rationForSupplementaryMachine3] != DBNull.Value
                                      && manufacturing.Columns.Contains(colName[colIndex + i])  && manufacturing.Rows[manufacturing.Rows.Count - 1][colName[colIndex + i]] != DBNull.Value) continue;
                                    else if (colName[colIndex + i] == Report.Manufacturing.otherYearOfMachine)
                                    {
                                        if (!manufacturing.Columns.Contains(Report.Manufacturing.otherYearOfMachine)) manufacturing.Columns.Add(Report.Manufacturing.otherYearOfMachine);
                                        manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.otherYearOfMachine] = $"{worksheet2.Cells[machinLine, i].Value}";
                                    }
                                    else if ( colName[colIndex + i] == Report.Manufacturing.amotizingYearOfMachine && manufacturing.Columns.Contains(Report.Manufacturing.amotizingYearOfMachine) && manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.amotizingYearOfMachine] != DBNull.Value)  
                                    {
                                        if (!manufacturing.Columns.Contains(Report.Manufacturing.rationForSupplementaryMachine3)) manufacturing.Columns.Add(Report.Manufacturing.rationForSupplementaryMachine3);
                                        excel.CellVaildationDT(Report.Manufacturing.rationForSupplementaryMachine3, nameRow, 4, i, machinLine, i, worksheet2, ref manufacturing, 4);
                                    }
                                    else if (worksheet2.Cells[machinLine, i].Value == null) continue;
                                    else excel.CellVaildationDT(colName[colIndex + i], nameRow, 4, i, machinLine, i, worksheet2, ref manufacturing, 4);
                                }
                                catch (Exception e)
                                {
                                    err += e.Message + "\n";
                                }

                            }

                            double rationForSupplementaryMachine = global.ConvertDouble(manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.rationForSupplementaryMachine3]) != 0 ?
                                   global.ConvertDouble(manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.rationForSupplementaryMachine1])
                               * global.ConvertDouble(manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.rationForSupplementaryMachine2])
                               / global.ConvertDouble(manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.rationForSupplementaryMachine3]) : 0;

                            if (!manufacturing.Columns.Contains(Report.Manufacturing.spaceCost)) manufacturing.Columns.Add(Report.Manufacturing.spaceCost);
                            manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.spaceCost] = rationForSupplementaryMachine;

                            if (!manufacturing.Columns.Contains(Report.Manufacturing.productionTime)) manufacturing.Columns.Add(Report.Manufacturing.productionTime);
                            manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.productionTime] =
                                global.ConvertDouble(manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.productionDay])
                                * global.ConvertDouble(manufacturing.Rows[manufacturing.Rows.Count - 1][Report.Manufacturing.productionTime]); 
                        }
                    }

                    String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Import";
                    string query = GenerateInsertQuery(header.Rows[0], manufacturing, material);
                    query = query.Replace("\r\n", " ");
                    string result = global_DB.ScalarExecute(query, ((int)global_DB.connDB.selfDB));
                    if (workBook != null)
                    {
                        //변경점 저장하면서 닫기
                        workBook.Close(true);
                        //Excel 프로그램 종료
                        application.Quit();
                        //오브젝트 해제1
                        ExcelCommon.ReleaseExcelObject(workBook);
                        ExcelCommon.ReleaseExcelObject(application);
                    }
                }

                LoadingScreen.CloseSplashScreen();
            }
            catch (Exception exc)
            {
                LoadingScreen.CloseSplashScreen();
                return exc.Message;
            }

            return err;
        }
        private string GenerateInsertQuery(DataRow vendorInfo, DataTable vendorManufacturing, DataTable vendorMaterial)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("BEGIN TRANSACTION;");
                query.AppendLine("DECLARE @InsertedVendorInfo TABLE (NewId INT);");

                // 1. VendorInfo 데이터 삽입 및 중복 검사
                string vendorInfoColumns = string.Join(", ", vendorInfo.Table.Columns.Cast<DataColumn>().Select(col => $"[{col.ColumnName}]"));
                string vendorInfoValues = string.Join(", ", vendorInfo.Table.Columns.Cast<DataColumn>().Select(col => FormatValue(vendorInfo[col])));
                string vendorInfoPkColumn = "Id";
                query.AppendLine($@"
    DECLARE @ExistingId INT;
    SELECT @ExistingId = {vendorInfoPkColumn} FROM VendorInfo WHERE Left([품번], 7) = '{vendorInfo["품번"].ToString().Substring(0,7)}';
    
    IF @ExistingId IS NULL
    BEGIN
        INSERT INTO VendorInfo ({vendorInfoColumns})
        OUTPUT INSERTED.Id INTO @InsertedVendorInfo (NewId)
        VALUES ({vendorInfoValues});
        SELECT @ExistingId = NewId FROM @InsertedVendorInfo;
END
    ELSE
BEGIN
        UPDATE VendorInfo
        SET {string.Join(", ", vendorInfo.Table.Columns.Cast<DataColumn>().Select(col => $"[{col.ColumnName}] = {FormatValue(vendorInfo[col])}"))}
        WHERE {vendorInfoPkColumn} = @ExistingId;
        
    END;");

                // 2. VendorManufacturing 데이터 삽입 및 중복 제거
                if (vendorManufacturing.Rows.Count > 0)
                {
                    string vendorManufacturingColumns = string.Join(", ", vendorManufacturing.Columns.Cast<DataColumn>().Select(col => $"[{col.ColumnName}]"));
                    query.AppendLine($"DELETE FROM VendorManufacturing WHERE VendorInfoId IN (@ExistingId);");
                    query.AppendLine($"INSERT INTO VendorManufacturing ({vendorManufacturingColumns}, VendorInfoId)");
                    query.AppendLine("SELECT * FROM (VALUES");

                    int count = 0;
                    foreach (DataRow row in vendorManufacturing.Rows)
                    {
                        if (count++ > 0) query.AppendLine(",");
                        query.Append($"({string.Join(", ", vendorManufacturing.Columns.Cast<DataColumn>().Select(col => FormatValue(row[col])))}, @ExistingId)");
                    }

                    query.AppendLine($") AS tmp ({vendorManufacturingColumns}, VendorInfoId);");
                }

                // 3. VendorMaterial 데이터 삽입 및 중복 제거
                if (vendorMaterial.Rows.Count > 0)
                {
                    string vendorMaterialColumns = string.Join(", ", vendorMaterial.Columns.Cast<DataColumn>().Select(col => $"[{col.ColumnName}]"));
                    query.AppendLine($"DELETE FROM VendorMaterial WHERE VendorInfoId IN (@ExistingId);");
                    query.AppendLine($"INSERT INTO VendorMaterial ({vendorMaterialColumns}, VendorInfoId)");
                    query.AppendLine("SELECT * FROM (VALUES");

                    int count = 0;
                    foreach (DataRow row in vendorMaterial.Rows)
                    {
                        if (count++ > 0) query.AppendLine(",");
                        query.Append($"({string.Join(", ", vendorMaterial.Columns.Cast<DataColumn>().Select(col => FormatValue(row[col])))}, @ExistingId)");
                    }

                    query.AppendLine($") AS tmp ({vendorMaterialColumns}, VendorInfoId);");
                }

                query.AppendLine("COMMIT TRANSACTION;");
                return query.ToString();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }            
        }

        // 데이터 값 형식 변환 함수 (문자열에는 '' 추가, 숫자는 그대로, NULL 처리)
        private string FormatValue(object value)
        {
            if (value == null || value == DBNull.Value)
                return "NULL";

            if ( value is DateTime) return $"'{value}'";
            else if(value is string) return $"'{((string)value).Replace("'","''")}'";

            return value.ToString();
        }
    }
}
