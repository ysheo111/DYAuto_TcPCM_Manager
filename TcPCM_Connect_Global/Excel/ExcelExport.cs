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

        public string Export(Bom.ExportLang lang, string fileLocation,  Dictionary< string, Part> parts, Bom.ManufacturingType type)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;

            try
            {
                File.Copy(Application.StartupPath + $@"\CBD.xlsx", fileLocation, true);

                //Excel 프로그램 실행
                application = new Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일로부터 불러오기                
                workBook = application.Workbooks.Open(fileLocation);
                //frmInit = frm;
                foreach(KeyValuePair <string, Part> part in parts) 
                {
                    //foreach (Excel.Worksheet sheet in workBook.Worksheets)
                    //{
                    //    if (sheet.Name == part.Value.header.partName) break;
                    //}
                    string sheetName = (lang == Bom.ExportLang.Kor ? "Export" : "Quotation");
                    sheetName = $"{sheetName}_{type.ToString()}";
                    workBook.Worksheets[sheetName].Copy(Type.Missing, workBook.Sheets[workBook.Sheets.Count]); // copy
                    workBook.Worksheets[$"Export_일반"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    workBook.Worksheets[$"Export_주조"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    workBook.Worksheets[$"Export_사출"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    workBook.Worksheets[$"Export_프레스"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    workBook.Worksheets[$"Export_코어"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    workBook.Worksheets[$"Quotation_일반"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    workBook.Worksheets[$"Quotation_주조"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    workBook.Worksheets[$"Quotation_사출"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    workBook.Worksheets[$"Quotation_프레스"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    workBook.Worksheets[$"Quotation_코어"].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    Excel.Worksheet worksheet = workBook.Sheets[workBook.Sheets.Count];

                    //같은 이름 있는지 체크(코드 추가하기)
                    worksheet.Name = part.Value.header.partName;        // rename
                    worksheet.Visible = Excel.XlSheetVisibility.xlSheetVisible;
                    worksheet.Select();

                    CBDMatching(lang, worksheet, part.Value, type);
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

        public void CBDMatching(Bom.ExportLang lang, Excel.Worksheet worksheet, Part part, Bom.ManufacturingType type)
        {
            try
            {
                //foreach (Excel.Worksheet sheet in workBook.Worksheets)
                //{
                //    if (sheet.Name == part.header.partName) return;
                //}

                //workBook.Worksheets[$"Export"].Copy(Type.Missing, workBook.Sheets[workBook.Sheets.Count]); // copy
                //Excel.Worksheet worksheet = workBook.Sheets[workBook.Sheets.Count];

                //worksheet.Name = part.header.partName;        // rename
                //worksheet.Visible = Excel.XlSheetVisibility.xlSheetVisible;
                //worksheet.Select();

                //CBD의 기본정보
                worksheet.get_Range("B1", "B1").Select();

                //Excel의 사용범위를 읽어옴
                Excel.Range range = worksheet.UsedRange;
                int lastCol = 49;

                //각 단계마다 헤더를 두어서 찾는 코드
                //int[] marker = new int[6];
                List<int> marker = new List<int>();
                marker.Add(13);
                marker.Add(27);
                marker.Add(52);

                //for (int i = 1; i <= range.Rows.Count+1; i++)
                //{
                //    string CellColor = worksheet.Cells[i, 2].Interior.Color.ToString();
                //    if (worksheet.Cells[i, 2].Interior.Color.Equals("#D6DCE4")) marker.Add(i);
                //}
                int excelOrder = 0;
                //Basic
                int row = 2, excelCol =4;
                worksheet.Cells[row++, excelCol].Value = part.header.modelName?.Replace("[LGMagna]","");
                worksheet.Cells[row++, excelCol].Value = part.header.productName?.Replace("[LGMagna]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.partName?.Replace("[LGMagna]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.partNumber?.Replace("[LGMagna]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.region?.Replace("[LGMagna]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.SOP;

                row = 2; excelCol = 15;
                worksheet.Cells[row++, excelCol].Value = part.header.company?.Replace("[LGMagna]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.team?.Replace("[LGMagna]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.author?.Replace("[LGMagna]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.dateOfCreation;
                worksheet.Cells[row++, excelCol].Value = part.header.incoterms?.Replace("[LGMagna]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.currency?.Replace("[LGMagna]", "");

                //row = 5; excelCol = 26;
                //worksheet.Cells[row, excelCol++].Value = part.header.width;
                //worksheet.Cells[row, excelCol++].Value = part.header.height;
                //worksheet.Cells[row, ++excelCol].Value = part.header.thinkness;

                //summary
                row = 11; excelCol = 4;
                worksheet.Cells[row, excelCol++].Value = part.summary.transportTotal;
                worksheet.Cells[row, excelCol++].Value = part.summary.moldTotal;
                worksheet.Cells[row, excelCol++].Value = part.summary.etc+part.summary.defect+part.summary.variant;

                excelCol = 18;
                worksheet.Cells[row, excelCol++].Value = part.summary.administrationCosts;
                worksheet.Cells[row, excelCol++].Value = part.summary.profit;
                worksheet.Cells[row, excelCol++].Value = part.summary.materialOverhead;

                //worksheet.Cells[row, excelCol++].Value = part.summary.package;
                //excelCol += 3;
                //worksheet.Cells[row, excelCol++].Value = part.summary.transport;
                //excelCol += 3;
                //worksheet.Cells[row, excelCol++].Value = part.summary.development;
                //excelCol += 3;
                //worksheet.Cells[row, excelCol++].Value = part.summary.etc;
                //excelCol += 3;
                //excelOrder++;
                DataTable material = new DataTable();
  
                //원/부재료
                MemberInfo[] materialMembers = typeof(Part.Material).GetMembers(BindingFlags.Public);
                row = marker[excelOrder] + 5;
                for (int i = 0; i < part.material.Count - 9; i++)
                {
                    Excel.Range rangeToInsert = worksheet.Range["A" + row, "AQ" + row];

                    // 지정된 범위에 행을 삽입합니다.
                    rangeToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);
                    for (int j = excelOrder + 1; j < marker.Count; j++)
                    {
                        marker[j]++;
                    }
                }
                //worksheet.Cells[marker[excelOrder] + 3, 9].Value = part.header.currency;
                //worksheet.Cells[marker[excelOrder] + 3, 13].Value = part.header.currency;
                //worksheet.Cells[marker[excelOrder] + 3, 14].Value = part.header.currency;
                //worksheet.Cells[marker[excelOrder] + 3, 15].Value = part.header.currency;
               
                for (int i = 0; i < part.material.Count; i++)
                {
                    row = marker[excelOrder] + 4 + i;
                    Excel.Range cell = worksheet.Cells[row, excelCol] as Excel.Range;
                    cell.Select();
                    excelCol = 2;
                    worksheet.Cells[row, excelCol++].Value = i + 1;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].name?.Replace("[LGMagna]", "");
                    excelCol++;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].itemNumber?.Replace("[LGMagna]", "");
                    worksheet.Cells[row, excelCol++].Value = part.material[i].substance?.Replace("[LGMagna]", "");
                    worksheet.Cells[row, excelCol++].Value = part.material[i].standard?.Replace("[LGMagna]", "");
                    worksheet.Cells[row, excelCol++].Value = $"{part.material[i].qunantityUnit}/{part.material[i].priceUnit}";
                    worksheet.Cells[row, excelCol++].Value = part.material[i].unitCost;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].netWeight;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].quantity;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].totalQuantity;
                    worksheet.Cells[row, excelCol++].Formula = $"=Z{row}";
                    worksheet.Cells[row, excelCol++].Value = part.material[i].etc;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].total;
                    //worksheet.Cells[row, excelCol++].Value = part.material[i].comment;
                    worksheet.Cells[row, excelCol + 5].Value = part.material[i].dross;

                    excelCol = 24;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].returnRatio;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].scrapUnitPrice;
                    worksheet.Cells[row, excelCol++].Value = part.material[i].scrap;

                }

                excelOrder++;
                //가공비
                MemberInfo[] manufacturingMembers = typeof(Part.Manufacturing).GetMembers(BindingFlags.Public);
                row = marker[excelOrder] + 5;
                for (int i = 0; i < part.manufacturing.Count - 10; i++)
                {
                    Excel.Range rangeToInsert = worksheet.Range["A" + row, "AQ" + row];

                    // 지정된 범위에 행을 삽입합니다.
                    rangeToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);
                    for (int j = excelOrder + 1; j < marker.Count; j++)
                    {
                        marker[j]++;
                    }
                }

                DataTable cycleTime = new DataTable();
                double etRow = marker[excelOrder] + 1;
                worksheet.Cells[marker[excelOrder] + 1, 16].Value = part.manufacturing[0].et/100;                
                worksheet.Cells[marker[excelOrder] + 1, 18].Value = part.header.plc==0 ? 0 : part.header.plcVolume / part.header.plc;
                worksheet.Cells[marker[excelOrder] + 1, 19].Value = part.header.plc;
                worksheet.Cells[marker[excelOrder] + 1, 20].Value = part.header.plcVolume;
                worksheet.Range["AD4"].Value = part.header.plcVolume;

                for (int i = 0; i < part.manufacturing.Count; i++)
                {
                    row = marker[excelOrder] + 4 + i;

                    string workingTime = "";
                    if (type == Bom.ManufacturingType.주조)
                    {
                        part.manufacturing[i].workingTime = global.ConvertDouble(part.manufacturing[i].sequence);
                        workingTime = $"={part.manufacturing[i].workingTime}";
                        if (part.manufacturing[i].workingTime == 0)
                        {
                            part.manufacturing[i].workingTime = part.manufacturing[i].netCycleTime * (2 - part.manufacturing[i].oee/100) + part.manufacturing[i].prepare * 60 / part.manufacturing[i].lotQty;
                            workingTime = $"=U{row}*(2-S{row}/100)+T{row}*60/R{row}";
                        }

                        part.manufacturing[i].laborCosts = part.manufacturing[i].workingTime * part.manufacturing[i].grossWage / 3600;

                        part.manufacturing[i].machinaryCost = part.manufacturing[i].laborCosts + part.manufacturing[i].workingTime * part.manufacturing[i].machineCostRate / 3600;

                    }
                    else
                    {
                        part.manufacturing[i].workingTime = part.manufacturing[i].netCycleTime * (2 - part.manufacturing[i].oee / 100) + part.manufacturing[i].prepare * 60 / part.manufacturing[i].lotQty;
                        workingTime = $"=U{row}*(2-S{row}/100)+T{row}*60/R{row}";
                        part.manufacturing[i].laborCosts = part.manufacturing[i].workingTime * part.manufacturing[i].grossWage / 3600;

                        part.manufacturing[i].machinaryCost = part.manufacturing[i].laborCosts + part.manufacturing[i].workingTime * part.manufacturing[i].machineCostRate / 3600;
                    }

                  
                    excelCol = 2;
                    Excel.Range cell = worksheet.Cells[row, excelCol] as Excel.Range;
                    cell.Select();
                    worksheet.Cells[row, excelCol++].Value = i + 1;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].partName?.Replace("[LGMagna]", "");
                    excelCol++;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].itemNumber?.Replace("[LGMagna]", "");
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].manufacturingName?.Replace("[LGMagna]", "");
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineName?.Replace("[LGMagna]", "");
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].cavity;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].workers;                    
                   
                    worksheet.Cells[row, excelCol++].Formula = workingTime;                    
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].usage;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].grossWage;
                    worksheet.Cells[row, excelCol++].Formula =$"=I{row}*J{row}*K{row}*L{row}/3600/P{etRow}/H{row}";
                    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].laborCosts;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineCostRate;
                    worksheet.Cells[row, excelCol++].Formula = $"=(J{row}*K{row}*N{row}/3600)/P{etRow}/H{row}";
                    
                    excelCol = 18;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].lotQty;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].oee;
                   
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].prepare;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].netCycleTime;
                    worksheet.Cells[row, excelCol++].Formula = workingTime;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineCost;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].amotizingYearOfMachine;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineArea;

                    string query = $@"SELECT 
                        COALESCE(
                            t.c.value('(value[@lang=""ko - KR""])[1]', 'nvarchar(max)'),  -- ko-KR 값을 먼저 시도
                            t.c.value('(value[@lang=""en-US""])[1]', 'nvarchar(max)')-- ko - KR 값이 없으면 en - US 값 시도
                        ) AS TranslatedValue
                    FROM
                        MDCostFactorDetailComments AS s
                        CROSS APPLY s.[Text_LOC].nodes('/translations') AS t(c)
                    WHERE
                        ParentId = (
                            SELECT ID
                            FROM dbo.MDCostFactorDetails
                            WHERE Id = (
                                SELECT SpaceCostFactorDetailId
                                FROM Machines
                                WHERE Id = 9797
                            )
                        ); ";

                    string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.PCMDB);
                    //worksheet.Range[worksheet.Cells[row, excelCol], worksheet.Cells[row, excelCol+2]].Merge();
                    if (result.Length == 0)
                    {
                        worksheet.Range[worksheet.Cells[row, excelCol], worksheet.Cells[row, excelCol + 2]].Merge();
                        worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].rationForSupplementaryMachine;
                    }
                    else
                    {
                        string[] split = result.Split(':');
                        worksheet.Cells[row, excelCol++].Value = split[2].Split(' ')[1];
                        worksheet.Cells[row, excelCol++].Value = split[1].Split(' ')[1];
                        worksheet.Cells[row, excelCol++].Value = split[3].Split(' ')[1];
                    }
                   

                    excelCol = 29;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].workingDayPerYear;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].workingTimePerDay;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machinePower;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machinePowerEfficiency;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machinePowerCost;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].ratioOfMachineRepair;
                    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].ratioOfIndirectlyMachineryCost;

                    worksheet.Cells[row, excelCol++].Formula = $"=IF(X{row}=0,0,IF(AC{row}=0,0,IF(AD{row}=0,0,(W{row}/X{row}/AC{row}/AD{row}))))";
                    worksheet.Cells[row, excelCol++].Formula = $"=IF(AB{row}=0,0,IF(AC{row}=0,0,IF(AD{row}=0,0,(Y{row}*(Z{row}/100)*AA{row}/AB{row}/AC{row}/AD{row}))))";
                    worksheet.Cells[row, excelCol++].Formula = $"=AE{row}*(AF{row}/100)*AG{row}";
                    worksheet.Cells[row, excelCol++].Formula = $"=(AP{row}+AQ{row})*(AN{row}/100)";
                    worksheet.Cells[row, excelCol++].Formula = $"=SUM(AJ{row}:AM{row})";
                    worksheet.Cells[row, excelCol++].Formula = $"=AN{row}*(AI{row}/100)";
                    worksheet.Cells[row, excelCol++].Formula = $"=SUM(AN{row}:AO{row})";
                    //worksheet.Cells[row, excelCol++].Formula = "=";

                    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].amotizingCostOfMachine;
                    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].amotizingCostOfFactory;
                    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].amotizingCostOfPower;
                    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineRepairCost;
                    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].directExpenseRatio;
                    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].redirectExpenseRatio;
                    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineCostRate;
                }

                // 양산 금형비
                MemberInfo[] toolMembers = typeof(Part.Tooling).GetMembers(BindingFlags.Public);
                excelOrder++;
                row = marker[excelOrder] + 4 + 2;

                for (int i = 0; i < part.tooling.Count - 5; i++)
                {
                    // 행을 삽입할 범위를 지정합니다. 예를 들어, 1행부터 row-1 행까지의 범위
                    Excel.Range rangeToInsert = worksheet.Range["A" + row, "AQ" + row];

                    // 지정된 범위에 행을 삽입합니다.
                    rangeToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);
                }
               
                //worksheet.Cells[marker[excelOrder] + 2, 13].Value = part.header.currency;
                //worksheet.Cells[marker[excelOrder] + 2, 15].Value = part.header.currency;
                for (int i = 0; i < part.tooling.Count; i++)
                {
                    row = marker[excelOrder] + 4 + i;

                    excelCol = 2;
                    worksheet.Cells[row, excelCol++].Value = i + 1;
                    //worksheet.Cells[row, excelCol++].Value = part.tooling[i].partName;

                    excelCol = 5;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].tooling;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].type;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].cavity;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].lifetime;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].method;

                    excelCol = 11;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].leadtime;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].annualCapa;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].unitCost;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].quantity;
                    worksheet.Cells[row, excelCol++].Value = part.tooling[i].total;

                }

            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.StackTrace}");
            }
        }

        private void ExcelSet(int i, int headerSize, int excelOrder, int lastRow, int lastCol, List<int> marker, List<int> colList, DataGridView dgv, Excel.Worksheet worksheet)
        {
            int row = marker[excelOrder] + i+ headerSize;
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
                if (colList.Count-1< useColIdx) continue;

                worksheet.Cells[row, colList[useColIdx]].Value = dgv.Rows[i].Cells[col.Name].Value;

                int merge = colList.Count > useColIdx + 1 ? colList[useColIdx + 1] : lastCol;
                if(colList[useColIdx] != merge) worksheet.Range[worksheet.Cells[row, colList[useColIdx]], worksheet.Cells[row, merge - 1]].Merge();
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

            string err = Export(exportMode, dlg.FileName, parts, type);
            if (err != null) return  $"Error : {err}";
            else return null;
        }

        public double Average(List<double> values)
        {
            return values.Count() > 0 ? Math.Round(values.ToArray().Average()):0;
        }

        public string ExportLocationGrid(DataGridView dgv)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = ".xlsx|";

                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return null;
                else if (dialog != DialogResult.OK) return $"Error : 저장위치가 올바르게 선택되지 않았습니다.";

                //Excel 프로그램 실행
                application = new Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일로부터 불러오기
                workBook = application.Workbooks.Open(dlg.FileName);

                Excel.Worksheet worksheet = workBook.Sheets[1];
                worksheet.Name = "지역";
                worksheet.Visible = Excel.XlSheetVisibility.xlSheetVisible;

                worksheet.Cells[1, 1] = dgv.Columns[0].Name;
                for (int row = 1; row < dgv.RowCount; row++)
                {
                    worksheet.Cells[row + 1, 1] = dgv.Rows[row - 1].Cells[0].Value.ToString();
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
                    workBook.Close();
                    application.Quit();
                    ////오브젝트 해제1
                    //ExcelCommon.ReleaseExcelObject(workBook);
                    //ExcelCommon.ReleaseExcelObject(application);
                }
            }

            return null;
        }
    }
}
