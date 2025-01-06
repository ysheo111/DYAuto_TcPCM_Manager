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
                    worksheet.Cells[row, excelCol++].Value = part.material[i].transport;//공급기준
                    worksheet.Cells[row, excelCol++].Value = part.material[i].substance?.Replace("[DYA]", "");
                    worksheet.Cells[row, excelCol++].Value = part.material[i].thickness;//두께
                    worksheet.Cells[row, excelCol++].Value = part.material[i].width;//가로
                    worksheet.Cells[row, excelCol++].Value = part.material[i].length;//세로
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

                    if (part.manufacturing[i].price != 0)
                    {
                        worksheet.Cells[row, 16].Value = global.ZeroToNull(part.manufacturing[i].price);
                        worksheet.Cells[row, 11].Value = global.ZeroToNull(part.manufacturing[i].quantity);
                        continue;
                    }

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


        public string Import(string tagetType, double tagetID)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
            ExcelImport excel = new ExcelImport();
            string err = null;
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();

                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return null;
                else if (dialog != DialogResult.OK) return $"Error : 파일 오픈에 실패하였습니다.";

                //Excel 프로그램 실행
                application = new Microsoft.Office.Interop.Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일로부터 불러오기
                workBook = application.Workbooks.Open(dlg.FileName);

                List<string> workSheetList = new List<string>();
                foreach (Excel.Worksheet sheet in workBook.Worksheets)
                {
                    if (sheet.Visible != Excel.XlSheetVisibility.xlSheetVisible) continue;
                    workSheetList.Add(sheet.Name);
                }

                frmPartWorkSheetSelect workSheetSelect = new frmPartWorkSheetSelect();
                workSheetSelect.workSheet = workSheetList;
                if (workSheetSelect.ShowDialog() == DialogResult.Cancel) return null;
                string val = workSheetSelect.ReturnValue1;
                string val2 = workSheetSelect.ReturnValue2;

                Excel.Worksheet worksheet = workBook.Worksheets.Item[val];
                Excel.Worksheet worksheet2 = workBook.Worksheets.Item[val2];
                worksheet.Activate();

                //CBD의 기본정보
                worksheet.get_Range("G2", "G2").Select();

                JArray data = new JArray();
                JObject header = new JObject();

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
                        excel.CellVaildation(colName[i], nameRow, row++, excelCol, worksheet, date, ref header);
                    }
                    else if (i == 10) header.Add(Report.Header.exchangeRateCurrency, worksheet.Cells[row++, excelCol + 1].Value);
                    else if (colName[i] == Report.Header.category) segment = $"{worksheet.Cells[row++, excelCol + 1].Value}";
                    else
                    {
                        excel.CellVaildation(colName[i], nameRow, row, excelCol, row++, excelCol + 1, worksheet, ref header);
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
                    excel.CellVaildation(colName[i], 2, row, excelCol, row + 2, excelCol++, worksheet, ref header);
                }
                for (int i = 16; i < 19; i++)
                {
                    excel.CellVaildation(colName[i], 3, row, excelCol, row + 3, excelCol++, worksheet, ref header);
                }

                //원/부재료
                int materialDefault = 3;
                JArray materials = new JArray();
                try
                {
                    for (int j = 25; j < 52; j++)
                    {
                        worksheet.get_Range($"C{j}", $"C{j}").Select();
                        JObject part = new JObject();
                        JObject material = new JObject();
                        JObject scrap = new JObject();
                        int colIndex = 16, nameRow = 3;
                        int[] values = { 11, 12, 13, 14, 17 };

                        double unit = 1, net = global.ConvertDouble(worksheet.Cells[j, 11].Value);
                        if (((Excel.Range)worksheet.Cells[j, 3]).Value == null) break;


                        for (int i = 3; i < 20; i++)
                        {
                            if (i == 4 || i == 16 || i == 18)
                            {
                                colIndex--;
                                continue;
                            }
                            excelCol = i;

                            if (!values.Contains(i)) excel.CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref part);
                            else if (net != 0)
                            {
                                if (i == 14) excel.CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material);
                                else if (i == 17) excel.CellVaildation(Report.Material.rawMaterial, nameRow, 22, excelCol, j, excelCol, worksheet, ref scrap);
                                else if (i == 13)
                                {
                                    excel.CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material);
                                    excel.CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref scrap);

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
                            excel.CellVaildation(Report.Material.grossWeight, nameRow, 22, 12, j, 12, worksheet, ref part);
                            part[Report.Material.grossWeight] = (global.ConvertDouble(part[Report.Material.grossWeight]) - net) / unit;

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

                            materials.Add(part);
                            materials.Add(material);
                            materials.Add(scrap);
                        }

                        else
                        {
                            part.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Estimation(rough)");
                            excel.CellVaildation(Report.Material.unit, nameRow, 22, 13, j, 13, worksheet, ref part);
                            excel.CellVaildation(Report.Material.rawMaterial, nameRow, 22, 14, j, 14, worksheet, ref part);
                            materials.Add(part);
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

                MemberInfo[] total = new MemberInfo[manufacturingMembers.Length + headerMembers.Length + materialMembers.Length];
                manufacturingMembers.CopyTo(total, 0);
                headerMembers.CopyTo(total, manufacturingMembers.Length);
                materialMembers.CopyTo(total, manufacturingMembers.Length + headerMembers.Length);

                foreach (MemberInfo member in total)
                {
                    string key = ((FieldInfo)member).GetValue(member.Name)?.ToString();
                    if (!header.ContainsKey(key)) header.Add(key, null);
                }

                header.Add("WeightType", "");
                data.Add(header);
                data.Merge(materials);

                int[] manufacturingList = { 10, 15, 17, 21, 24, 25, 27 };
                int manufacturingDefault = 3;
                JArray manufacturings = new JArray();

                for (int j = 57; j < 89; j++)
                {
                    int machinLine = j - 49;
                    int colIndex = 30;
                    int nameRow = 2;

                    worksheet.get_Range($"C{j}", $"C{j}").Select();
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

                    for (int i = 3; i < 14; i++)
                    {
                        if (i == 4)
                        {
                            colIndex--;
                            continue;
                        }
                        else if (colName[colIndex + i] == Report.Manufacturing.category)
                        {
                            excel.CellVaildation(colName[colIndex + i], nameRow, 55, i, worksheet, $"{header[Report.Header.suppier]}||{worksheet.Cells[j, i].Value}", ref manufacturing);
                            excel.CellVaildation(colName[colIndex + i], nameRow, 55, i, worksheet, $"{header[Report.Header.suppier]}||{worksheet.Cells[j, i].Value}", ref labor);
                        }
                        else
                        {
                            excel.CellVaildation(colName[colIndex + i], nameRow, 55, i, j, i, worksheet, ref manufacturing);
                            excel.CellVaildation(colName[colIndex + i], nameRow, 55, i, j, i, worksheet, ref labor);
                        }

                        //excel.CellVaildation(colName[colIndex + i], nameRow, 55, i, j, i, worksheet, ref machine);
                    }

                    colIndex += 11;
                    nameRow = 4;

                    for (int i = 3; i < 28; i++)
                    {
                        try
                        {
                            if (manufacturingList.Contains(i))
                            {
                                colIndex--;
                                continue;
                            }
                            else if (i == 22 || i == 23) excel.CellVaildation(colName[colIndex + i], nameRow, 4, i, machinLine, i, worksheet2, ref otherMachine);
                            else if (machine.ContainsKey(colName[colIndex + i])) excel.CellVaildation(Report.Manufacturing.rationForSupplementaryMachine3, nameRow, 4, i, machinLine, i, worksheet2, ref machine);
                            else if (colName[colIndex + i] == Report.Manufacturing.category)
                            {
                                excel.CellVaildation(colName[colIndex + i], nameRow, 4, i, worksheet2, $"{header[Report.Header.suppier]}||{worksheet2.Cells[machinLine, i].Value}", ref machine);
                            }
                            else excel.CellVaildation(colName[colIndex + i], nameRow, 4, i, machinLine, i, worksheet2, ref machine);
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
                    manufacturing.Add(Report.Manufacturing.redirectExpenseRatio, machine[Report.Manufacturing.redirectExpenseRatio]);
                    manufacturing.Add(Report.Manufacturing.productionDay, machine[Report.Manufacturing.productionDay]);
                    manufacturing.Add(Report.Manufacturing.productionTime,
                        global.ConvertDouble(machine[Report.Manufacturing.productionDay]) * global.ConvertDouble(machine[Report.Manufacturing.productionTime]));

                    manufacturings.Add(manufacturing);
                    manufacturings.Add(labor);
                    manufacturings.Add(machine);

                    if (global.ConvertDouble(otherMachine[Report.Manufacturing.otherMachineCost]) != 0)
                    {
                        otherMachine.Add(Report.Manufacturing.sequence, j * 10);
                        otherMachine.Add(Report.Manufacturing.machineName, "Other Machine");
                        otherMachine.Add(Report.LineType.lineType, "A");
                        otherMachine.Add(Report.LineType.level, "2");
                        otherMachine.Add(Report.Manufacturing.machineCost, otherMachine[Report.Manufacturing.otherMachineCost]);
                        //otherMachine.Add(Report.Manufacturing.amotizingYearOfMachine, otherMachine[Report.Manufacturing.otherYearOfMachine]);
                        otherMachine.Add(Report.Header.currency, header[Report.Header.currency]);
                        otherMachine.Add(Report.Header.partName, header[Report.Header.partName]);
                        manufacturings.Add(otherMachine);
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
                    if ((bool)postResult["success"] == false) err = postResult["message"].ToString();
                }
                catch
                {
                    err += response;
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
                    workBook.Close(true);
                    //Excel 프로그램 종료
                    application.Quit();
                    //오브젝트 해제1
                    ExcelCommon.ReleaseExcelObject(workBook);
                    ExcelCommon.ReleaseExcelObject(application);
                }
            }
            return err;
        }

    }
}
