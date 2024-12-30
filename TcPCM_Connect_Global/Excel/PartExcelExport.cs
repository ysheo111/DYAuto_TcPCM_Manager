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
                File.Copy(Application.StartupPath + $@"\부품원가계산서.xlsx", fileLocation, true);

                //Excel 프로그램 실행
                application = new Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일로부터 불러오기                
                workBook = application.Workbooks.Open(fileLocation);
                //frmInit = frm;
                foreach (KeyValuePair<string, Part> part in parts)
                {
                    string sheetName = (lang == Bom.ExportLang.Kor ? "Export" : "Quotation");
                    workBook.Worksheets[sheetName].Copy(Type.Missing, workBook.Sheets[workBook.Sheets.Count]); // copy                    
                    Excel.Worksheet worksheet = workBook.Sheets[workBook.Sheets.Count];

                    //같은 이름 있는지 체크(코드 추가하기)
                    worksheet.Name = part.Value.header.partName;        // rename
                    worksheet.Visible = Excel.XlSheetVisibility.xlSheetVisible;
                    workBook.Worksheets[sheetName].Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    worksheet.Select();

                    CBDMatching(lang, worksheet, part.Value);
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

        public void CBDMatching(Bom.ExportLang lang, Excel.Worksheet worksheet, Part part)
        {
            try
            {
                //CBD의 기본정보
                worksheet.get_Range("B1", "B1").Select();

                //Excel의 사용범위를 읽어옴
                Excel.Range range = worksheet.UsedRange;

                //         public static string modelName = "차종";
                //public static string partNumber = "품번";
                //public static string partName = "품명";

                //public static string company = "업체명";
                //public static string customer = "납품국가";
                //public static string currency = "화폐";
                //public static string transport = "물류조건";

                //public static string category = "업종";
                //public static string suppier = "제조국가";
                //public static string exchangeRate = "업체적용환율";
                //public static string exchangeRateCurrency = "업체적용환율단위";

                //public static string author = "작성자";
                //public static string dateOfCalc = "작성일";

                //public static string guid = "작성일";
                //public static string partID = "작성일";

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
                worksheet.Cells[row++, excelCol].Value = part.header.company?.Replace("[DYA]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.suppier?.Replace("[DYA]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.exchangeRate;
                worksheet.Cells[row++, excelCol].Value = part.header.exchangeRateCurrency?.Replace("[DYA]", "");

                row = 2; excelCol = 19;
                worksheet.Cells[row++, excelCol].Value = part.header.author?.Replace("[DYA]", "");
                worksheet.Cells[row++, excelCol].Value = part.header.dateOfCalculation;

                //summary
                row = 13; excelCol = 8;
                worksheet.Cells[row, excelCol++].Value = part.summary.administrationCosts;
                worksheet.Cells[row, excelCol++].Value = part.summary.profit;
                worksheet.Cells[row, excelCol++].Value = part.summary.materialOverhead;

                row = 14; excelCol = 11;
                worksheet.Cells[row, excelCol++].Value = part.summary.rnd;
                worksheet.Cells[row, excelCol++].Value = part.summary.packageTransport;
                worksheet.Cells[row, excelCol++].Value = part.summary.etc;

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


                //for (int i = 0; i < part.material.Count; i++)
                //{
                //    row = marker[excelOrder] + 4 + i;
                //    Excel.Range cell = worksheet.Cells[row, excelCol] as Excel.Range;
                //    cell.Select();
                //    excelCol = 2;
                //    worksheet.Cells[row, excelCol++].Value = i + 1;
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].name?.Replace("[LGMagna]", "");
                //    excelCol++;
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].itemNumber?.Replace("[LGMagna]", "");
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].substance?.Replace("[LGMagna]", "");
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].standard?.Replace("[LGMagna]", "");
                //    worksheet.Cells[row, excelCol++].Value = $"{part.material[i].qunantityUnit}/{part.material[i].priceUnit}";
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].unitCost;
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].netWeight;
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].quantity;
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].totalQuantity;
                //    worksheet.Cells[row, excelCol++].Formula = $"=Z{row}";
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].etc;
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].total;
                //    //worksheet.Cells[row, excelCol++].Value = part.material[i].comment;
                //    worksheet.Cells[row, excelCol + 5].Value = part.material[i].dross;

                //    excelCol = 24;
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].returnRatio;
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].scrapUnitPrice;
                //    worksheet.Cells[row, excelCol++].Value = part.material[i].scrap;

                //}

                //excelOrder++;
                ////가공비
                //MemberInfo[] manufacturingMembers = typeof(Part.Manufacturing).GetMembers(BindingFlags.Public);
                //row = marker[excelOrder] + 5;
                //for (int i = 0; i < part.manufacturing.Count - 10; i++)
                //{
                //    Excel.Range rangeToInsert = worksheet.Range["A" + row, "AQ" + row];

                //    // 지정된 범위에 행을 삽입합니다.
                //    rangeToInsert.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);
                //    for (int j = excelOrder + 1; j < marker.Count; j++)
                //    {
                //        marker[j]++;
                //    }
                //}

                //DataTable cycleTime = new DataTable();
                //double etRow = marker[excelOrder] + 1;
                //worksheet.Cells[marker[excelOrder] + 1, 16].Value = part.manufacturing[0].et / 100;
                //worksheet.Cells[marker[excelOrder] + 1, 18].Value = part.header.plc == 0 ? 0 : part.header.plcVolume / part.header.plc;
                //worksheet.Cells[marker[excelOrder] + 1, 19].Value = part.header.plc;
                //worksheet.Cells[marker[excelOrder] + 1, 20].Value = part.header.plcVolume;
                //worksheet.Range["AD4"].Value = part.header.plcVolume;

                //for (int i = 0; i < part.manufacturing.Count; i++)
                //{
                //    row = marker[excelOrder] + 4 + i;

                //    string workingTime = "";
                //    if (type == Bom.ManufacturingType.주조)
                //    {
                //        part.manufacturing[i].workingTime = global.ConvertDouble(part.manufacturing[i].sequence);
                //        workingTime = $"={part.manufacturing[i].workingTime}";
                //        if (part.manufacturing[i].workingTime == 0)
                //        {
                //            part.manufacturing[i].workingTime = part.manufacturing[i].netCycleTime * (2 - part.manufacturing[i].oee / 100) + part.manufacturing[i].prepare * 60 / part.manufacturing[i].lotQty;
                //            workingTime = $"=U{row}*(2-S{row}/100)+T{row}*60/R{row}";
                //        }

                //        part.manufacturing[i].laborCosts = part.manufacturing[i].workingTime * part.manufacturing[i].grossWage / 3600;

                //        part.manufacturing[i].machinaryCost = part.manufacturing[i].laborCosts + part.manufacturing[i].workingTime * part.manufacturing[i].machineCostRate / 3600;

                //    }
                //    else
                //    {
                //        part.manufacturing[i].workingTime = part.manufacturing[i].netCycleTime * (2 - part.manufacturing[i].oee / 100) + part.manufacturing[i].prepare * 60 / part.manufacturing[i].lotQty;
                //        workingTime = $"=U{row}*(2-S{row}/100)+T{row}*60/R{row}";
                //        part.manufacturing[i].laborCosts = part.manufacturing[i].workingTime * part.manufacturing[i].grossWage / 3600;

                //        part.manufacturing[i].machinaryCost = part.manufacturing[i].laborCosts + part.manufacturing[i].workingTime * part.manufacturing[i].machineCostRate / 3600;
                //    }


                //    excelCol = 2;
                //    Excel.Range cell = worksheet.Cells[row, excelCol] as Excel.Range;
                //    cell.Select();
                //    worksheet.Cells[row, excelCol++].Value = i + 1;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].partName?.Replace("[LGMagna]", "");
                //    excelCol++;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].itemNumber?.Replace("[LGMagna]", "");
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].manufacturingName?.Replace("[LGMagna]", "");
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineName?.Replace("[LGMagna]", "");
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].cavity;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].workers;

                //    worksheet.Cells[row, excelCol++].Formula = workingTime;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].usage;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].grossWage;
                //    worksheet.Cells[row, excelCol++].Formula = $"=I{row}*J{row}*K{row}*L{row}/3600/P{etRow}/H{row}";
                //    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].laborCosts;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineCostRate;
                //    worksheet.Cells[row, excelCol++].Formula = $"=(J{row}*K{row}*N{row}/3600)/P{etRow}/H{row}";

                //    excelCol = 18;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].lotQty;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].oee;

                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].prepare;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].netCycleTime;
                //    worksheet.Cells[row, excelCol++].Formula = workingTime;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineCost;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].amotizingYearOfMachine;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineArea;

                //    string query = $@"SELECT 
                //        COALESCE(
                //            t.c.value('(value[@lang=""ko - KR""])[1]', 'nvarchar(max)'),  -- ko-KR 값을 먼저 시도
                //            t.c.value('(value[@lang=""en-US""])[1]', 'nvarchar(max)')-- ko - KR 값이 없으면 en - US 값 시도
                //        ) AS TranslatedValue
                //    FROM
                //        MDCostFactorDetailComments AS s
                //        CROSS APPLY s.[Text_LOC].nodes('/translations') AS t(c)
                //    WHERE
                //        ParentId = (
                //            SELECT ID
                //            FROM dbo.MDCostFactorDetails
                //            WHERE Id = (
                //                SELECT SpaceCostFactorDetailId
                //                FROM Machines
                //                WHERE Id = 9797
                //            )
                //        ); ";

                //    string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.PCMDB);
                //    //worksheet.Range[worksheet.Cells[row, excelCol], worksheet.Cells[row, excelCol+2]].Merge();
                //    if (result.Length == 0)
                //    {
                //        worksheet.Range[worksheet.Cells[row, excelCol], worksheet.Cells[row, excelCol + 2]].Merge();
                //        worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].rationForSupplementaryMachine;
                //    }
                //    else
                //    {
                //        string[] split = result.Split(':');
                //        worksheet.Cells[row, excelCol++].Value = split[2].Split(' ')[1];
                //        worksheet.Cells[row, excelCol++].Value = split[1].Split(' ')[1];
                //        worksheet.Cells[row, excelCol++].Value = split[3].Split(' ')[1];
                //    }


                //    excelCol = 29;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].workingDayPerYear;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].workingTimePerDay;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machinePower;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machinePowerEfficiency;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machinePowerCost;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].ratioOfMachineRepair;
                //    worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].ratioOfIndirectlyMachineryCost;

                //    worksheet.Cells[row, excelCol++].Formula = $"=IF(X{row}=0,0,IF(AC{row}=0,0,IF(AD{row}=0,0,(W{row}/X{row}/AC{row}/AD{row}))))";
                //    worksheet.Cells[row, excelCol++].Formula = $"=IF(AB{row}=0,0,IF(AC{row}=0,0,IF(AD{row}=0,0,(Y{row}*(Z{row}/100)*AA{row}/AB{row}/AC{row}/AD{row}))))";
                //    worksheet.Cells[row, excelCol++].Formula = $"=AE{row}*(AF{row}/100)*AG{row}";
                //    worksheet.Cells[row, excelCol++].Formula = $"=(AP{row}+AQ{row})*(AN{row}/100)";
                //    worksheet.Cells[row, excelCol++].Formula = $"=SUM(AJ{row}:AM{row})";
                //    worksheet.Cells[row, excelCol++].Formula = $"=AN{row}*(AI{row}/100)";
                //    worksheet.Cells[row, excelCol++].Formula = $"=SUM(AN{row}:AO{row})";
                //    //worksheet.Cells[row, excelCol++].Formula = "=";

                //    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].amotizingCostOfMachine;
                //    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].amotizingCostOfFactory;
                //    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].amotizingCostOfPower;
                //    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineRepairCost;
                //    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].directExpenseRatio;
                //    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].redirectExpenseRatio;
                //    //worksheet.Cells[row, excelCol++].Value = part.manufacturing[i].machineCostRate;
                //}
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
