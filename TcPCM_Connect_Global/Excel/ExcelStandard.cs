using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Application = System.Windows.Forms.Application;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using DataTable = System.Data.DataTable;

namespace TcPCM_Connect_Global
{
    public class ExcelStandard
    {
        public string Export(List<string> calcList, string fileLocation)
        {
            string err = "";

            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
            try
            {
                string query = $@"SELECT
                            distinct c.id AS CalculationId,
							ppe.ManualSalesPrice as SalesPrice,
	                        p2.Name_LOC_Extracted,
                            p2.PartNo,
                            p.StartOfProductionDate,
                            p.PeriodsAfterSOP,							
                            par.*
                        FROM
                            ProjectAnnualRequirements par
                        JOIN
                            Projects p ON par.ProjectId = p.Id
						left JOIN
                            Calculations c ON par.PartId = c.PartId
                        JOIN
                            ProjectPartEntries ppe ON par.ProjectId = ppe.ProjectId and c.PartId = ppe.PartId
                        left join
                            Parts p2 ON c.PartId = p2.Id
                        WHERE
                            ppe.PartId in (SELECT PartId FROM Calculations WHERE id in ({ string.Join(", ", calcList)})) 
	                        and c.Id in ({string.Join(", ", calcList)})";

                System.Data.DataTable requirements = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

                int rowIndex = 11, addRowIndex = 0;
                int colIndex = 3;

                File.Copy($@"{Application.StartupPath}\DY AUTO 사전원가 양식.xlsx", $@"{fileLocation}\견적 및 표준원가.xlsx", true);
                //Excel 프로그램 실행
                application = new Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일로부터 불러오기                
                workBook = application.Workbooks.Open($@"{fileLocation}\견적 및 표준원가.xlsx");
                Excel.Worksheet worksheet = workBook.Sheets["견적원가 레포트"];
                Excel.Worksheet worksheet2 = workBook.Sheets["년간손익"];
                Excel.Worksheet worksheet3 = workBook.Sheets["경제성 분석"];
                Excel.Worksheet worksheet4 = workBook.Sheets["가공비"];
                Excel.Worksheet worksheet5 = workBook.Sheets["투자 및 감상비"];
                worksheet.Select();

                query = $@"select AVG(Value*3600) as name from MDCostFactorDetails 
                          where id in (

                          select WageCostFactorDetailId from ManufacturingSteps as a
                          join[Labor] as b on a.Id = b.ManufacturingStepId
                          where CalculationId in ({ string.Join(", ", calcList)}))";

                int sop = ((DateTime)requirements.Rows[0]["StartOfProductionDate"]).Year;
                double after = global.ConvertDouble(requirements.Rows[0]["PeriodsAfterSOP"]);

                List<string> partName = new List<string>();
                List<string> partNo = new List<string>();
                worksheet3.Cells[6, 6].Value = sop - 1;
                for (int i = 0; i < after; i++)
                {
                    worksheet.Range[worksheet.Cells[rowIndex - 2, 5 + i], worksheet.Cells[rowIndex + 1, 5 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                    worksheet.Range[worksheet.Cells[50, 5 + i], worksheet.Cells[74, 5 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                    worksheet2.Range[worksheet2.Cells[3, 5 + i], worksheet2.Cells[60, 5 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                    worksheet3.Range[worksheet3.Cells[5, 7 + i], worksheet3.Cells[23, 7 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                    worksheet4.Range[worksheet4.Cells[4, 5 + i], worksheet4.Cells[10, 5 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                    worksheet5.Range[worksheet5.Cells[4, 6 + i], worksheet5.Cells[30, 6 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동

                    Range range = worksheet.Range[worksheet.Cells[rowIndex - 1, 5 + i], worksheet.Cells[rowIndex + 1, 5 + i]];
                    Range range2 = worksheet2.Range[worksheet2.Cells[3, 5 + i], worksheet2.Cells[60, 5 + i]];
                    Range range3 = worksheet3.Range[worksheet3.Cells[5, 7 + i], worksheet3.Cells[23, 7 + i]];
                    Range range4 = worksheet4.Range[worksheet4.Cells[4, 5 + i], worksheet4.Cells[10, 5 + i]];
                    Range range5 = worksheet5.Range[worksheet5.Cells[4, 6 + i], worksheet5.Cells[30, 6 + i]];

                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    worksheet2.Range[worksheet2.Cells[15, 5 + i], worksheet2.Cells[59, 5 + i]].Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.ColumnWidth = 12;
                    range2.ColumnWidth = 10;
                    range3.ColumnWidth = 10.38;
                    range4.ColumnWidth = 13.5;
                    range5.ColumnWidth = 11.13;
                    worksheet5.Cells[4, 6 + i].Value = worksheet.Cells[rowIndex - 1, 5 + i].Value = worksheet2.Cells[15, 5 + i].Value = worksheet4.Cells[4, 5 + i].Value = worksheet3.Cells[6, 7 + i].Value = sop + i;
                    worksheet5.Cells[15, 6 + i].Formula = $"=SUM({global.NumberToLetter(6+i)}5:{global.NumberToLetter(6 + i)}14)";
                    worksheet5.Cells[24, 6 + i].Formula = $"=SUM({global.NumberToLetter(6+i)}16:{global.NumberToLetter(6 + i)}23)";
                    worksheet5.Cells[26, 6 + i].Formula = $"={global.NumberToLetter(6 + i)}5";
                    worksheet5.Cells[27, 6 + i].Formula = $"={global.NumberToLetter(6 + i)}10+{global.NumberToLetter(6 + i)}11+{global.NumberToLetter(6 + i)}12+{global.NumberToLetter(6 + i)}13+{global.NumberToLetter(6 + i)}14";
                    worksheet5.Cells[28, 6 + i].Formula = $"={global.NumberToLetter(6 + i)}6+{global.NumberToLetter(6 + i)}7+{global.NumberToLetter(6 + i)}8+{global.NumberToLetter(6 + i)}9";
                    worksheet5.Cells[29, 6 + i].Formula = $"=SUM({global.NumberToLetter(6+i)}26:{global.NumberToLetter(6 + i)}28)";
                    worksheet3.Cells[7, 7 + i].Value = i+1;
                    worksheet.Cells[51, 5 + i].Value= $"{sop + i}년";
                    worksheet.Cells[60, 5 + i].Value = worksheet.Cells[68, 5 + i].Value = $"{sop + i-1}년";
                }

                DataTable invest = new DataTable();
                invest.Columns.Add(" ");
                for (int rowIdx = 5; rowIdx<=29; rowIdx++)
                {
                    DataRow row = invest.Rows.Add();
                    if (rowIdx == 25) row[" "] = "";
                    else row[" "]= $"=SUM(F{rowIdx}:{global.NumberToLetter(5 + (int)after)}{rowIdx})";
                    
                    //worksheet5.Cells[rowIdx, 6 + after + 1].Formula = $"=SUM(F{rowIdx}:={global.NumberToLetter(6 + (int)after)}{rowIdx})";
                }
                WriteDataToExcel(invest, worksheet5, 5, 6 + (int)after);

                string errRequirement = WriteOptimizedToExcel(workBook, requirements, after, ref partName, ref partNo, ref addRowIndex, ref rowIndex, ref colIndex);
                if (errRequirement != null) err += ("\n" + errRequirement);

                worksheet.Range[worksheet.Cells[28 + addRowIndex, 5], worksheet.Cells[28 + addRowIndex, 5 + (partName.Count - 1) * 2 + 1]].Merge();

                Interface export = new Interface();
                JObject apiResult = export.LoadCalc(calcList, "Estimate");
                if (apiResult == null) return "데이터 조회 시 오류가 발생하였습니다.";
                err += SetMaterial(apiResult, workBook, calcList.Count);

                string err2 = SetManufacturing(workBook, calcList, (int)after, addRowIndex, partName, partNo);
                if (err2 != null) err += ("\n" + err2);

                worksheet2.Select();

                SetAnnualProfitAndLoss(workBook, requirements, sop, (int)after, partName, addRowIndex);
                worksheet.Range[worksheet.Cells[54 + addRowIndex, 5], worksheet.Cells[54 + addRowIndex, 5 + after]].NumberFormat = "0.00%";
                worksheet.Range[worksheet.Cells[56 + addRowIndex, 5], worksheet.Cells[56 + addRowIndex, 5 + after]].NumberFormat = "0.00%";

            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (workBook != null)
                {
                    //변경점 저장하면서 닫기
                    workBook.Save();
                    //workBook.Close(true);
                    //Excel 프로그램 종료
                    //application.Quit();
                    ////오브젝트 해제1
                    //ExcelCommon.ReleaseExcelObject(workBook);
                    //ExcelCommon.ReleaseExcelObject(application);
                }
            }


            return err;
        }

        private string SetAnnualProfitAndLoss(Workbook workBook, System.Data.DataTable requirements, int sop, int after, List<string> partName, int addRowIndex)
        {
            try
            {
                Excel.Worksheet worksheet = workBook.Sheets["견적원가 레포트"];
                Excel.Worksheet worksheet2 = workBook.Sheets["년간손익"];
                Excel.Worksheet worksheet3 = workBook.Sheets["경제성 분석"];
                Excel.Worksheet worksheet4 = workBook.Sheets["가공비"];
                Excel.Worksheet worksheet5 = workBook.Sheets["투자 및 감상비"];

                var dataToWrite = new List<(Excel.Range, object)>();
                var dataToFormat = new List<(Excel.Range, object)>();
                Dictionary<int, List<double>> profitAndLoss = new Dictionary<int, List<double>>();

                // ** 1. EconomicIncreases 조회 **
                string query = $@"
            SELECT i.* FROM [TcPCM2312_Patch3].[dbo].[EconomicIncreases] AS i
            LEFT JOIN EconomicCalculations AS c ON i.EconomicCalculationId = c.Id
            LEFT JOIN Projects AS p ON c.ProjectId = p.Id
            LEFT JOIN CostElementDefinition AS e ON e.Id = I.CostElementDefinitionId
            WHERE p.Id = {requirements.Rows[0]["ProjectId"]} AND c.Deleted IS NULL";

                System.Data.DataTable increase = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

                // ** 2. EconomicOverheadRates 조회 **
                query = $@"
            SELECT * FROM [TcPCM2312_Patch3].[dbo].[EconomicOverheadRates] AS i
            LEFT JOIN EconomicCalculations AS c ON i.EconomicCalculationId = c.Id
            LEFT JOIN CostElementDefinition AS e ON e.Id = I.CostElementDefinitionId
            WHERE c.Id = {increase.Rows[0]["EconomicCalculationId"]} AND c.Deleted IS NULL 
            ORDER BY CostElementDefinitionId, DateValidFrom";

                System.Data.DataTable overheads = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

                System.Data.DataTable increaseDataTable = new System.Data.DataTable();
                System.Data.DataTable formulaDataTable = new System.Data.DataTable();

                for (int i = 0; i < after; i++)
                {
                    increaseDataTable.Columns.Add($"Col_{i}");
                    formulaDataTable.Columns.Add($"Col_{i}");
                }

                for(int i=0;i<=8;i++)
                {
                    increaseDataTable.Rows.Add();
                }
                // ** 3. Overhead 데이터 처리 **
                foreach (DataRow row in overheads.Rows)
                {
                    int rowIdx = 0;
                    int idx = int.Parse(row["CostElementDefinitionId"].ToString());

                    if (idx == 161) rowIdx = 9;
                    else if (idx == 154) rowIdx = 12;
                    else if (idx == 160) rowIdx = 10;
                    else if (idx == 156) rowIdx = 11;
                    else if (new List<int> { 159, 152, 339, 157 }.Contains(idx))
                    {
                        if (!profitAndLoss.ContainsKey(idx)) profitAndLoss.Add(idx, new List<double>());
                        if (global.ConvertDouble(row["ManualRate"]) != 0) profitAndLoss[idx].Add(global.ConvertDouble(row["ManualRate"]));
                        continue;
                    }
                    else continue;

                    int colIdx = ((DateTime)row["DateValidFrom"]).Year - sop;
                    if (colIdx < 0 || colIdx >= after) continue;

                    if (!profitAndLoss.ContainsKey(idx)) profitAndLoss.Add(idx, new List<double>());
                    if (global.ConvertDouble(row["ManualRate"]) != 0) profitAndLoss[idx].Add(global.ConvertDouble(row["ManualRate"]));

                    increaseDataTable.Rows[rowIdx - 4][$"Col_{colIdx}"] = row["ManualRate"];
                    //dataToWrite.Add((worksheet2.Cells[rowIdx, colIdx + 5], row["ManualRate"]));
                    //dataToFormat.Add((worksheet2.Cells[rowIdx, colIdx + 5], "0.00%"));
                }

                for(int i=0; i<22;i++)
                {
                    formulaDataTable.Rows.Add();
                }

                // ** 4. Increase 데이터 처리 **
                foreach (DataRow row in increase.Rows)
                {
                    int rowIdx = 0;
                    int idx = int.Parse(row["CostElementDefinitionId"].ToString());
                    List<double> list = new List<double>();

                    if (idx == 425) rowIdx = 4;
                    else if (idx == 447) rowIdx = 6;
                    else if (idx == 421) rowIdx = 5;
                    else if (idx == 443) rowIdx = 7;
                    else if (idx == 410) rowIdx = 8;
                    else continue;

                    for (int i = 0; i < after; i++)
                    {
                        string name = i == 0 ? "FractionValue0" : $"FractionValueAfter{i}";
                        increaseDataTable.Rows[rowIdx - 4][$"Col_{i}"] = global.ConvertDouble(row[name]);
                        if (global.ConvertDouble(row[name]) != 0) list.Add(global.ConvertDouble(row[name]));
                        //dataToWrite.Add((worksheet2.Cells[rowIdx, 5 + i], global.ConvertDouble(row[name])));
                        //if (global.ConvertDouble(row[name]) != 0) list.Add(global.ConvertDouble(row[name]));
                        //dataToFormat.Add((worksheet2.Cells[rowIdx, 5 + i], "0.00%"));

                        GenerateFormula(worksheet2,  i,  partName, ref formulaDataTable);
                        //// **최적화된 방식으로 엑셀에 수식 입력**
                        //ApplyDataToExcel(new List<(Excel.Range, object)>(), dataToFormula, new List<(Excel.Range, object)>());
                    }

                    if (!profitAndLoss.ContainsKey(idx)) profitAndLoss.Add(idx, list);
                    else profitAndLoss[idx].AddRange(list);
                }

                // ** 🚀 한 번에 Excel에 쓰기 (DataTable → Excel) **
                int startRow = 4;
                int startCol = 5; // **🔥 5번째 열부터 데이터 시작**
             
                WriteDataToExcel(increaseDataTable, worksheet2, startRow, startCol);

                // ** 🚀 한 번에 Excel에 쓰기 (DataTable → Excel) **
                startRow = 16 + 22 * partName.Count;
                 startCol = 5; // **🔥 5번째 열부터 데이터 시작**

                WriteDataToExcel(formulaDataTable, worksheet2, startRow, startCol);

                worksheet2.Range[worksheet2.Cells[16, 5 + after], worksheet2.Cells[37, 5 + after]].Copy();
                worksheet2.Range[worksheet2.Cells[38, 5 + after], worksheet2.Cells[37 + (partName.Count-1) * 22, 5 + after]].PasteSpecial(Excel.XlPasteType.xlPasteFormats);
                // 클립보드 해제 (엑셀 실행 속도 최적화)
                worksheet2.Application.CutCopyMode = 0;

                worksheet2.Range[worksheet2.Cells[4, 5 + after], worksheet2.Cells[37 + (partName.Count) * 22, 5 + after]].Copy();
                worksheet2.Range[worksheet2.Cells[4, 5], worksheet2.Cells[37 + (partName.Count) * 22, 5 + after - 1]].PasteSpecial(Excel.XlPasteType.xlPasteFormats);
                // 클립보드 해제 (엑셀 실행 속도 최적화)
                worksheet2.Application.CutCopyMode = 0;
                object[,] dataArray = new object[1, (partName.Count + 1) * 22];

                // DataTable의 데이터를 배열로 변환 (Excel에 한 번에 입력하기 위함)
                for (int r = 0; r < 1; r++)
                {
                    for (int c = 0; c < (partName.Count+1) * 22; c++)
                    {
                        dataArray[r, c] = $"=AVERAGE(E{c+16}:{global.NumberToLetter(4+after)}{c+16})";
                    }
                }

                worksheet2.Range[worksheet2.Cells[16, 5 + after], worksheet2.Cells[37 + (partName.Count) * 22, 5 + after]].Value2 = dataArray;

                //**6.Most Frequent 값 계산 후 입력**
                var dataToMostFreq = new List<(Excel.Range, object)>
             {
                    (worksheet.Cells[22 + addRowIndex, 3], FindMostFrequents(profitAndLoss, 425)),
                    (worksheet.Cells[22 + addRowIndex, 4], FindMostFrequents(profitAndLoss, 421)),
                    (worksheet.Cells[22 + addRowIndex, 5], FindMostFrequents(profitAndLoss, 339)),
                    (worksheet.Cells[22 + addRowIndex, 6], FindMostFrequents(profitAndLoss, 447)),
                    (worksheet.Cells[22 + addRowIndex, 7], FindMostFrequents(profitAndLoss, 443)),
                    (worksheet.Cells[22 + addRowIndex, 8], FindMostFrequents(profitAndLoss, 410)),
                    (worksheet.Cells[22 + addRowIndex, 11], FindMostFrequents(profitAndLoss, 160)),

                    (worksheet.Cells[24 + addRowIndex, 3], $"{sop}년"),
                    (worksheet.Cells[24 + addRowIndex, 7], FindMostFrequents(profitAndLoss, 161)),
                    (worksheet.Cells[24 + addRowIndex, 8], FindMostFrequents(profitAndLoss, 156)),
                    (worksheet.Cells[24 + addRowIndex, 9], FindMostFrequents(profitAndLoss, 157)),
                    (worksheet.Cells[24 + addRowIndex, 10], FindMostFrequents(profitAndLoss, 154)),
                    (worksheet.Cells[24 + addRowIndex, 11], FindMostFrequents(profitAndLoss, 152))
             };

                worksheet3.Cells[4, 6].Value= FindMostFrequents(profitAndLoss, 159);
                worksheet3.Cells[4, 4].Value = FindMostFrequents(profitAndLoss, 157);                
                worksheet3.Cells[4, 8].Value = FindMostFrequents(profitAndLoss, 152);


                ApplyDataToExcelGroup(dataToMostFreq, new List<(Excel.Range, object)>(), new List<(Excel.Range, object)>());

            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }
        public void GenerateFormula(Excel.Worksheet worksheet2, int i, List<string> partName, ref DataTable formulaDataTable)
        {
            int target = 0;
            int partCount = partName.Count;

            // 데이터 저장 리스트 (엑셀 API 호출 최소화)
            var dataToFormula = new List<(Excel.Range, object)>();

            List<int> baseRows = Enumerable.Range(0, partCount)
                                           .Select(j => 16 + j * 22)
                                           .ToList();

            List<int> multiplierRows = Enumerable.Range(17, 21).ToList();

            // 1. 첫 번째 수식 (SUM)
            string sumFormula = string.Join("+", baseRows.Select(row => $"E${row}"));
            //dataToFormula.Add((worksheet2.Cells[target++, i + 5], $"={sumFormula}"));
           
            formulaDataTable.Rows[target++][$"Col_{i}"] = $"={sumFormula}";
            //worksheet2.Cells[target, i + 5].Select();
            // 2. 나머지 수식 (곱셈 및 덧셈)
            foreach (int multiplierRow in multiplierRows)
            {
                string formulaParts = string.Join("+", baseRows.Select(row => $"E${row}*E${multiplierRow + ((row - 16) / 22) * 22}"));

                if (multiplierRow == 32)
                    formulaDataTable.Rows[target++][$"Col_{i}"] = $"=E{baseRows.Last() + 37}/E{baseRows.Last() + 23}"; 
                else if (multiplierRow == 37)
                    formulaDataTable.Rows[target++][$"Col_{i}"] = $"=E{baseRows.Last() + 42}/E{baseRows.Last() + 23}";
                else
                    formulaDataTable.Rows[target++][$"Col_{i}"] = $"={formulaParts}";
            }

        }

        private string SetManufacturing(Workbook workBook, List<string> calcList, int after, int addRowIndex, List<string> partName, List<string> PartNo)
        {
            string err = null;
            try
            {
                string query = $@"WITH LaborData AS (
                                        SELECT 
                                            a.id as Mid,
                                            e.UniqueKey,
                                            AVG(b.PersonCount) AS AvgPerson,        
                                            AVG((CASE 
                                                WHEN d.Id IS NULL THEN b.ManualWage 
                                                ELSE d.Value 
                                            END) * 3600) AS AvgCost
                                        FROM ManufacturingSteps AS a  
                                        JOIN RestManufacturingOverheadCosts AS c ON a.Id = c.ManufacturingStepId
                                        JOIN [Labor] AS b ON a.Id = b.ManufacturingStepId OR b.RestManufacturingOverheadCostsId = c.Id
                                        LEFT JOIN MDCostFactorDetails AS d ON b.WageCostFactorDetailId = d.Id
                                        LEFT JOIN MDCostFactorHeaders AS e ON b.WageCostFactorHeaderId = e.Id
                                        WHERE a.CalculationId IN ({string.Join(", ", calcList)})
                                        GROUP BY a.id, e.UniqueKey
                                    )
                                    SELECT					
 	                                    t.LifeTimeToolCount,
	                                    a.CalculationId,
	                                    a.ManualPartsPerCycle,
	                                    ManualCycleTime,
	                                    a.Id as ManufacturingId,
	                                    10 as toolYear,
	                                    t.MasterToolId,
	                                    t.ManualInvestPerTool,
	                                    t.CachedInvestPerTool,
	                                    t.ToolUsageFraction*100 as new,
	                                    t.id,
                                        p.PartNo,
                                        p.Name_LOC_Extracted,
	                                    case 
		                                    when dbo.GetSingleTranslation(a.Name_LOC,'ko-KR','') is not null then dbo.GetSingleTranslation(a.Name_LOC,'ko-KR','')
		                                    else dbo.GetSingleTranslation(a.Name_LOC,'en-US','') 
	                                    END as name,
	                                    L.*,	
	                                    ManualCycleTime,
	                                    ManualUtilizationRate,
                                        RequiredNumberOfMachinesInManufacturingStep,
	                                    CASE 
		                                    When  ManualCycleTime = 0 or m.RequiredNumberOfMachinesInManufacturingStep = 0 then null
		                                    WHEN UseManualMachineHourlyRate = 0 THEN MachineSystemCostsPerPart * 3600 / ManualCycleTime / m.RequiredNumberOfMachinesInManufacturingStep
		                                    WHEN UseManualMachineHourlyRate = 1 THEN MachineSystemCostsPerPart * 3600 / ManualCycleTime * ManualUtilizationRate / m.RequiredNumberOfMachinesInManufacturingStep
	                                    END  AS Machine,

	                                    case
		                                    When  ManualCycleTime = 0 or a.ShiftsPerDay = 0 then null
		                                    when a.ShiftModelMode=2 then a.ShiftModelProductionTimePerYearCostFactorValue/3600/a.ShiftsPerDay*(3600/ManualCycleTime*ManualUtilizationRate)
		                                    else c.WeeksPerYear*c.TimePerShift/3600*(3600/ManualCycleTime*ManualUtilizationRate)
	                                    END as '1Shift',
	                                    case
		                                    When  ManualCycleTime = 0 or a.ShiftsPerDay = 0 then null
		                                    when a.ShiftModelMode=2 then (a.ShiftModelProductionTimePerYearCostFactorValue/3600/a.ShiftsPerDay)*(3600/ManualCycleTime*ManualUtilizationRate)*2
		                                    else c.WeeksPerYear*c.TimePerShift/3600*(3600/ManualCycleTime*ManualUtilizationRate)*2
	                                    END as '2Shift'

                                    FROM  [ManufacturingSteps] as a  
                                    left Join (SELECT 
                                        ld.Mid,
                                        MAX(CASE WHEN ld.UniqueKey = N'직접노무비' THEN ld.AvgPerson END) AS DirectPerson,
                                        MAX(CASE WHEN ld.UniqueKey = N'직접노무비' THEN ld.AvgCost END) AS DirectCost,
                                        MAX(CASE WHEN ld.UniqueKey = N'간접노무비' THEN ld.AvgPerson END) AS IndirectPerson,
                                        MAX(CASE WHEN ld.UniqueKey = N'간접노무비' THEN ld.AvgCost END) AS IndirectCost
                                    FROM LaborData ld
                                    GROUP BY ld.Mid) as L on L.Mid = a.Id
                                    left JOIN Machines AS m ON a.Id = m.ManufacturingStepId
                                    left join Tools as t on t.ManufacturingStepId = a.Id
                                    Join Calculations as c on c.Id = a.CalculationId
                                    Join Parts as p on p.Id = c.PartId
	                                    WHERE a.CalculationId IN ({string.Join(", ", calcList)})
                                    order by CalculationId, Mid;";

                System.Data.DataTable table = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

                Excel.Worksheet worksheet4 = workBook.Sheets["가공비"];
                Excel.Worksheet worksheet = workBook.Sheets["견적원가 레포트"];
                Excel.Worksheet worksheet2 = workBook.Sheets["년간손익"];
                Excel.Worksheet worksheet6 = workBook.Sheets["금형비"];

                worksheet4.Rows[5].Cells[5].Value = worksheet.Cells[24 + addRowIndex, 4].Value = table.AsEnumerable().Where(row => global.ConvertDouble(row.Field<double?>("DirectPerson")) != 0).Average(row => global.ConvertDouble(row.Field<double?>("DirectCost")));
                worksheet4.Rows[6].Cells[5].Value = worksheet.Cells[24 + addRowIndex, 5].Value = table.AsEnumerable().Where(row => global.ConvertDouble(row.Field<double?>("IndirectPerson")) != 0).Average(row => global.ConvertDouble(row.Field<double?>("IndirectCost")));
                worksheet4.Rows[7].Cells[5].Value = worksheet.Cells[24 + addRowIndex, 6].Value = table.AsEnumerable().Where(row => global.ConvertDouble(row.Field<double?>("RequiredNumberOfMachinesInManufacturingStep")) != 0).Average(row => global.ConvertDouble(row.Field<double?>("Machine")));
                worksheet.Cells[22 + addRowIndex, 9].Value = table.AsEnumerable().Average(row => global.ConvertDouble(row.Field<double?>("ManualUtilizationRate")));

                for (int i = 1; i < after; i++)
                {
                    worksheet4.Rows[5].Cells[5 + i].Formula = $"={global.NumberToLetter(5 + (i - 1))}5*(1+{global.NumberToLetter(5 + (after))}5)";
                    worksheet4.Rows[6].Cells[5 + i].Formula = $"={global.NumberToLetter(5 + (i - 1))}6*(1+{global.NumberToLetter(5 + (after))}6)";
                    worksheet4.Rows[7].Cells[5 + i].Formula = $"={global.NumberToLetter(5 + (i - 1))}7*(1+{global.NumberToLetter(5 + (after))}7)";
                }

                int rowCount = 0;
                int part = 15, prevPart = 0;
                double calc = 0;
                DataTable dt = new DataTable();
                DataTable machine = new DataTable();
                DataTable tool = new DataTable();
                worksheet4.Select();
                for (int i = 0; i < 8; i++)
                {
                    dt.Columns.Add($"{i}");
                }

                for (int i = 0; i < 12; i++)
                {
                    machine.Columns.Add($"{i}");
                }

                for (int i = 0; i < 17; i++)
                {
                    tool.Columns.Add($"{i}");
                }

                for (int i = 0; i < 6; i++)
                {
                    dt.Rows.Add();
                }

                for (int i = 1; i < calcList.Count; i++)
                {
                    Excel.Range sourceRange = worksheet4.Range[worksheet4.Rows[15 + rowCount], worksheet4.Rows[21 + rowCount]];
                    Excel.Range targetRange = worksheet4.Range[worksheet4.Rows[22 + rowCount], worksheet4.Rows[28 + rowCount]];

                    // 원본 범위를 복사
                    sourceRange.Copy();

                    // 대상 범위에 삽입 (Insert)
                    targetRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    worksheet4.Application.CutCopyMode = 0;
                }

                int partCnt = 0;
                double manufacturingId = 0;
                List<int> machineNumber = new List<int>();
                List<int> toolNumber = new List<int>();
                Dictionary<string, int> toolDict = new Dictionary<string, int>();
                foreach (DataRow row in table.Rows)
                {
                    if (global.ConvertDouble(row["CalculationId"]) != calc)
                    {
                        worksheet4.Rows[part].Cells[3].Value = partName[partCnt];

                        //dt.Rows.InsertAt(dt.Rows[0], part + rowCount);
                        //dt.Rows.InsertAt(dt.Rows[0], part + rowCount + 1);
                        //dt.Rows.InsertAt(dt.Rows[0], part + 2 + rowCount * 3);

                        WriteDataToExcel(dt, worksheet4, part, 5);
                        dt.Rows.Clear();
                        for (int i = 0; i < 6; i++)
                        {
                            dt.Rows.Add();
                        }
                        if (prevPart != 0)
                        {
                            SetManufacturingSum(worksheet4, worksheet, part, addRowIndex, partCnt, rowCount);


                            part = prevPart + (rowCount + 1) * 3 + 1;
                        }
                        machineNumber.Add(machine.Rows.Count);
                        toolNumber.Add(tool.Rows.Count);
                        partCnt++;
                        prevPart = part;
                        rowCount = 0;
                    }

                    calc = global.ConvertDouble(row["CalculationId"]);

                    if (manufacturingId != global.ConvertDouble(row["ManufacturingId"]) && (row["DirectPerson"] != DBNull.Value || row["IndirectPerson"] != DBNull.Value || row["RequiredNumberOfMachinesInManufacturingStep"] != DBNull.Value))
                    {
                        if (global.ConvertDouble(row["CalculationId"]) == calc && rowCount != 0)
                        {
                            ((Excel.Range)worksheet4.Rows[part + rowCount]).Insert(Missing.Value, worksheet4.Rows[part + rowCount]);
                            ((Excel.Range)worksheet4.Rows[part + 2 + rowCount * 2]).Insert(Missing.Value, worksheet4.Rows[part + 2 + rowCount * 2]);
                            ((Excel.Range)worksheet4.Rows[part + 4 + rowCount * 3]).Insert(Missing.Value, worksheet4.Rows[part + 4 + rowCount * 3]);
                        }

                        DataRow row2 = dt.NewRow();
                        row2[-5 + 5] = row["name"];
                        row2[-5 + 6] = row["DirectPerson"];
                        row2[-5 + 7] = row["ManualCycleTime"];
                        row2[-5 + 8] = row["ManualUtilizationRate"];
                        row2[-5 + 9] = $"=IF(ISERROR(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())),\"\",(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())))";
                        row2[-5 + 10] = $"=IF(ISERROR(INDIRECT(\"F\"&ROW())/ INDIRECT(\"I\"&ROW())),\"\",(INDIRECT(\"F\"&ROW())/INDIRECT(\"I\"&ROW())))";
                        row2[-5 + 11] = row["DirectCost"];
                        row2[-5 + 12] = $"=IF(ISERROR(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())),\"\",(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())))";
                        dt.Rows.InsertAt(row2, rowCount);

                        DataRow row3 = dt.NewRow();
                        row3[-5 + 5] = row["name"];
                        row3[-5 + 6] = row["IndirectPerson"];
                        row3[-5 + 7] = row["ManualCycleTime"];
                        row3[-5 + 8] = row["ManualUtilizationRate"];
                        row3[-5 + 9] = $"=IF(ISERROR(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())),\"\",(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())))";
                        row3[-5 + 10] = $"=IF(ISERROR(INDIRECT(\"F\"&ROW())/ INDIRECT(\"I\"&ROW())),\"\",(INDIRECT(\"F\"&ROW())/INDIRECT(\"I\"&ROW())))";
                        row3[-5 + 11] = row["IndirectCost"];
                        row3[-5 + 12] = $"=IF(ISERROR(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())),\"\",(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())))";
                        dt.Rows.InsertAt(row3, 2 + rowCount * 2);

                        DataRow row4 = dt.NewRow();
                        row4[-5 + 5] = row["name"];
                        row4[-5 + 6] = row["RequiredNumberOfMachinesInManufacturingStep"];
                        row4[-5 + 7] = row["ManualCycleTime"];
                        row4[-5 + 8] = row["ManualUtilizationRate"];
                        row4[-5 + 9] = $"=IF(ISERROR(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())),\"\",(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())))";
                        row4[-5 + 10] = $"=IF(ISERROR(INDIRECT(\"F\"&ROW())/ INDIRECT(\"I\"&ROW())),\"\",(INDIRECT(\"F\"&ROW())/INDIRECT(\"I\"&ROW())))";
                        row4[-5 + 11] = row["Machine"];
                        row4[-5 + 12] = $"=IF(ISERROR(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())),\"\",(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())))";
                        dt.Rows.InsertAt(row4, 4 + rowCount * 3);

                        rowCount++;
                    }

                    if (global.ConvertDouble(row["MasterToolId"]) == 5)
                    {
                        int add = part + (rowCount - 1) * 3 + (calcList.Count - partCnt - 1) * 7 + machine.Rows.Count + 21;
                        ((Excel.Range)worksheet4.Rows[add]).Insert(Missing.Value, worksheet4.Rows[add]);
                        DataRow row5 = machine.Rows.Add();
                        int cnt = 0;
                        row5[cnt++] = $"{partName[partCnt - 1]}_{row["name"]}";
                        row5[cnt++] = row["ManualInvestPerTool"];
                        row5[cnt++] = row["new"];
                        row5[cnt++] = $"=SUM(INDIRECT(\"D\"&ROW()):INDIRECT(\"E\"&ROW()))";
                        row5[cnt++] = row["toolYear"];
                        row5[cnt++] = row["1Shift"];
                        row5[cnt++] = row["2Shift"];
                        row5[cnt++] = $"=INDIRECT(\"I\"&ROW())";
                        row5[cnt++] = $"='{worksheet.Name}'!{ worksheet.Cells[10 + partCnt, 5 + after].Address[false]}";
                        row5[cnt++] = $"=IFERROR(INDIRECT(\"F\"&ROW())/INDIRECT(\"G\"&ROW())/INDIRECT(\"H\"&ROW()), 0)";
                        row5[cnt++] = $"=IFERROR(INDIRECT(\"F\"&ROW())/INDIRECT(\"G\"&ROW())/INDIRECT(\"I\"&ROW()), 0)";
                        row5[cnt++] = $"=IFERROR(INDIRECT(\"D\"&ROW())/INDIRECT(\"G\"&ROW())/INDIRECT(\"J\"&ROW()) + INDIRECT(\"E\"&ROW())/INDIRECT(\"K\"&ROW()), 0)";
                    }
                    else if (global.ConvertDouble(row["MasterToolId"]) == 9 || global.ConvertDouble(row["MasterToolId"]) == 10)
                    {

                        string name = $"{row["Name_LOC_Extracted"]}_{row["name"]}";

                        if (toolDict.ContainsKey(name))
                        {
                            tool.Rows[toolDict[name]][8] = global.ConvertDouble(tool.Rows[toolDict[name]][8]) + 1;
                        }
                        else
                        {
                            ((Excel.Range)worksheet6.Rows[6 + tool.Rows.Count]).Insert(Missing.Value, worksheet6.Rows[6 + tool.Rows.Count]);
                            int cnt = 1;
                            DataRow row5 = tool.Rows.Add();
                            if (toolDict.ContainsKey(name)) tool.Rows[toolDict[name]][10] = global.ConvertDouble(tool.Rows[toolDict[name]][20]) + 1;
                            else
                            {
                                toolDict.Add(name, tool.Rows.Count - 1);
                                row5[cnt++] = $"{name}";
                                row5[cnt++] = global.ConvertDouble(row["MasterToolId"]) == 9 ? $"공용" : $"전용";
                                row5[cnt++] = $"{row["PartNo"]}";
                                row5[cnt++] = $"{row["ManualPartsPerCycle"]}";
                                row5[cnt++] = $"{row["LifeTimeToolCount"]}";
                                row5[cnt++] = $"=G{tool.Rows.Count + 5}*H{tool.Rows.Count + 5}";
                                row5[cnt++] = $"{row["CachedInvestPerTool"]}";
                                row5[cnt++] = $"{row["CachedInvestPerTool"]}";
                                row5[cnt++] = 1;
                                row5[cnt++] = $"='{worksheet.Name}'!{ worksheet.Cells[10 + partCnt, 5 + after].Address[false]}";
                                row5[cnt++] = $"=L{tool.Rows.Count + 5}*M{tool.Rows.Count + 5}";
                                row5[cnt++] = $"=IF(ISERROR(ROUNDUP(N{tool.Rows.Count + 5}/I{tool.Rows.Count + 5},0)),0,(ROUNDUP(N{tool.Rows.Count + 5}/I{tool.Rows.Count + 5},0)))";
                                row5[cnt++] = $"=J{tool.Rows.Count + 5}+K{tool.Rows.Count + 5}*(O{tool.Rows.Count + 5}-1)";
                                row5[cnt++] = global.ConvertDouble(row["MasterToolId"]) == 10 ? "" : $"=IF(ISERROR(J{tool.Rows.Count + 5}/H{tool.Rows.Count + 5}*L{tool.Rows.Count + 5}),0,(J{tool.Rows.Count + 5}/H{tool.Rows.Count + 5}*L{tool.Rows.Count + 5}))";
                                row5[cnt++] = global.ConvertDouble(row["MasterToolId"]) == 9 ? "" : $"=P{tool.Rows.Count + 5}/N{tool.Rows.Count + 5}";
                                row5[cnt++] = $"=Q{tool.Rows.Count + 5}+R{tool.Rows.Count + 5}";
                            }
                        }
                    }

                    //worksheet4.Rows[part + rowCount].Cells[5].Value = row["name"];
                    //worksheet4.Rows[part + rowCount].Cells[6].Value = row["DirectPerson"];
                    //worksheet4.Rows[part + rowCount].Cells[7].Value = row["ManualCycleTime"];
                    //worksheet4.Rows[part + rowCount].Cells[8].Value = row["ManualUtilizationRate"];
                    //worksheet4.Rows[part + rowCount].Cells[9].Formula = $"=IF(ISERROR(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())),\"\",(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())))";
                    //worksheet4.Rows[part + rowCount].Cells[10].Formula = $"=IF(ISERROR(INDIRECT(\"F\"&ROW())/ INDIRECT(\"I\"&ROW())),\"\",(INDIRECT(\"F\"&ROW())/INDIRECT(\"I\"&ROW())))";
                    //worksheet4.Rows[part + rowCount].Cells[11].Value = row["DirectCost"];
                    //worksheet4.Rows[part + rowCount].Cells[12].Formula = $"=IF(ISERROR(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())),\"\",(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())))";

                    //worksheet4.Rows[part + 2 + rowCount * 2].Cells[5].Value = row["name"];
                    //worksheet4.Rows[part + 2 + rowCount * 2].Cells[6].Value = row["IndirectPerson"];
                    //worksheet4.Rows[part + 2 + rowCount * 2].Cells[7].Value = row["ManualCycleTime"];
                    //worksheet4.Rows[part + 2 + rowCount * 2].Cells[8].Value = row["ManualUtilizationRate"];
                    //worksheet4.Rows[part + 2 + rowCount * 2].Cells[9].Formula = $"=IF(ISERROR(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())),\"\",(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())))";
                    //worksheet4.Rows[part + 2 + rowCount * 2].Cells[10].Formula = $"=IF(ISERROR(INDIRECT(\"F\"&ROW())/ INDIRECT(\"I\"&ROW())),\"\",(INDIRECT(\"F\"&ROW())/INDIRECT(\"I\"&ROW())))";
                    //worksheet4.Rows[part + 2 + rowCount * 2].Cells[11].Value = row["IndirectCost"];
                    //worksheet4.Rows[part + 2 + rowCount * 2].Cells[12].Formula = $"=IF(ISERROR(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())),\"\",(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())))";

                    //worksheet4.Rows[part + 4 + rowCount * 3].Cells[5].Value = row["name"];
                    //worksheet4.Rows[part + 4 + rowCount * 3].Cells[6].Value = row["RequiredNumberOfMachinesInManufacturingStep"];
                    //worksheet4.Rows[part + 4 + rowCount * 3].Cells[7].Value = row["ManualCycleTime"];
                    //worksheet4.Rows[part + 4 + rowCount * 3].Cells[8].Value = row["ManualUtilizationRate"];
                    //worksheet4.Rows[part + 4 + rowCount * 3].Cells[9].Formula = $"=IF(ISERROR(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())),\"\",(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())))";
                    //worksheet4.Rows[part + 4 + rowCount * 3].Cells[10].Formula = $"=IF(ISERROR(INDIRECT(\"F\"&ROW())/ INDIRECT(\"I\"&ROW())),\"\",(INDIRECT(\"F\"&ROW())/INDIRECT(\"I\"&ROW())))";
                    //worksheet4.Rows[part + 4 + rowCount * 3].Cells[11].Value = row["Machine"];
                    //worksheet4.Rows[part + 4 + rowCount * 3].Cells[12].Formula = $"=IF(ISERROR(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())),\"\",(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())))";
                }

                WriteDataToExcel(dt, worksheet4, part, 5);
                SetManufacturingSum(worksheet4, worksheet, part, addRowIndex, partCnt, rowCount);

                WriteDataToExcel(tool, worksheet6, 6, 3);
                machineNumber.Add(machine.Rows.Count);
                toolNumber.Add(tool.Rows.Count);

                for (int i =0;i< machineNumber.Count-1;i++)
                {
                    int targetRow = 37 + addRowIndex;
                    int targetCol = (i) * 2 + 5;

                    worksheet.Cells[targetRow, targetCol].Formula = $"=SUM('가공비'!N{ part + rowCount * 3 +10 + machineNumber[i]}:N{ part + rowCount * 3 + 9 + machineNumber[i+1]})";
                    worksheet.Cells[targetRow++, targetCol].NumberFormat = "#,##0";
                    worksheet.Cells[targetRow, targetCol].Formula = $"=SUM('금형비'!S{ 6+toolNumber[i]}:S{6+ toolNumber[i+1]})";    
                    worksheet.Cells[targetRow++, targetCol].NumberFormat = "#,##0";

                    targetCol++;
                    for(int j=0; j<15;j++)
                    {                        
                        worksheet.Cells[32 + addRowIndex+j, targetCol].Formula = $"={global.NumberToLetter(targetCol-1)}{32+addRowIndex+j}/{global.NumberToLetter(targetCol - 1)}{32 + addRowIndex}";
                    }
                }

                rowCount = tool.Rows.Count;
                int colCount = tool.Columns.Count;
                int startRow = 6;
                int startCol = 3;

                // Excel Range 지정하여 한 번에 데이터 입력
                Excel.Range startCell = (Excel.Range)worksheet6.Cells[startRow, startCol];
                startCell.Worksheet.Select();
                startCell.Select();
                Excel.Range endCell = (Excel.Range)worksheet6.Cells[startRow + rowCount - 1, startCol + colCount - 1];
                Excel.Range writeRange = worksheet6.Range[startCell, endCell];
                writeRange.Style = "표준"; // 한국어 엑셀

                // 모든 셀에 테두리 추가
                writeRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // 실선 테두리
                //writeRange.Borders.Weight = Excel.XlBorderWeight.xlThin;       // 기본 두께 설정

                // 숫자 포맷 적용
                worksheet6.Range[startCell, endCell].NumberFormat = "#,##0";

                WriteDataToExcel(machine, worksheet4, part+ dt.Rows.Count+4, 3);

                worksheet4.Range[worksheet4.Cells[15, 6], worksheet4.Cells[part + dt.Rows.Count, 7]].NumberFormat = "#,##0";
                worksheet4.Range[worksheet4.Cells[15, 8], worksheet4.Cells[part + dt.Rows.Count, 8]].NumberFormat = "0.00%";
                worksheet4.Range[worksheet4.Cells[15, 10], worksheet4.Cells[part + dt.Rows.Count, 10]].NumberFormat = "#,##0.0000";
                worksheet4.Range[worksheet4.Cells[15, 11], worksheet4.Cells[part + dt.Rows.Count, 12]].NumberFormat = "#,##0";

                worksheet6.Cells[startRow + rowCount, 10].Formula = $"=SUMIF($E$6:$E${5+tool.Rows.Count},$E{6 + tool.Rows.Count},J6:J{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount, 11].Formula = $"=SUMIF($E$6:$E${5+tool.Rows.Count},$E{6 + tool.Rows.Count},K6:K{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount, 16].Formula = $"=SUMIF($E$6:$E${5+tool.Rows.Count},$E{6 + tool.Rows.Count},P6:P{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount, 17].Formula = $"=SUMIF($E$6:$E${5+tool.Rows.Count},$E{6 + tool.Rows.Count},Q6:Q{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount, 18].Formula = $"=SUMIF($E$6:$E${5+tool.Rows.Count},$E{6 + tool.Rows.Count},R6:R{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount, 19].Formula = $"=SUMIF($E$6:$E${5+tool.Rows.Count},$E{6 + tool.Rows.Count},S6:S{5 + tool.Rows.Count})";

                worksheet6.Cells[startRow + rowCount+1, 10].Formula = $"=SUMIF($E$6:$E${5 + tool.Rows.Count},$E{7 + tool.Rows.Count},J6:J{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount+1, 11].Formula = $"=SUMIF($E$6:$E${5 + tool.Rows.Count},$E{7 + tool.Rows.Count},K6:K{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount+1, 16].Formula = $"=SUMIF($E$6:$E${5 + tool.Rows.Count},$E{7 + tool.Rows.Count},P6:P{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount+1, 17].Formula = $"=SUMIF($E$6:$E${5 + tool.Rows.Count},$E{7 + tool.Rows.Count},Q6:Q{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount+1, 18].Formula = $"=SUMIF($E$6:$E${5 + tool.Rows.Count},$E{7 + tool.Rows.Count},R6:R{5 + tool.Rows.Count})";
                worksheet6.Cells[startRow + rowCount+1, 19].Formula = $"=SUMIF($E$6:$E${5 + tool.Rows.Count},$E{7 + tool.Rows.Count},S6:S{5 + tool.Rows.Count})";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return err;
        }

        private void SetManufacturingSum(Worksheet worksheet4, Worksheet worksheet, int part, int addRowIndex, int partCnt, int rowCount)
        {
            // 대상 셀
            int targetRow = 34 + addRowIndex;
            int targetCol = (partCnt - 1) * 2 + 5;
            int sourceCol = 12;

            worksheet4.Rows[part + rowCount].Cells[12].Formula = $"=SUM(L{part}:L{part + rowCount - 1})";
            worksheet4.Range[worksheet4.Cells[part, 4], worksheet4.Cells[part + rowCount - 1, 4]].Merge();
            worksheet4.Rows[part + 1 + rowCount * 2].Cells[12].Formula = $"=SUM(L{part + rowCount + 1}:L{part + 2 + (rowCount - 1) * 2})";
            worksheet4.Range[worksheet4.Cells[part + rowCount + 1, 4], worksheet4.Cells[part + 2 + (rowCount - 1) * 2, 4]].Merge();
            worksheet4.Rows[part + 2 + rowCount * 3].Cells[12].Formula = $"=SUM(L{part + 1 + rowCount * 2 + 1}:L{part + 4 + (rowCount - 1) * 3})";
            worksheet4.Range[worksheet4.Cells[part + 1 + rowCount * 2 + 1, 4], worksheet4.Cells[part + 4 + (rowCount - 1) * 3, 4]].Merge();
            worksheet4.Rows[part + 3 + rowCount * 3].Cells[12].Formula = $"=SUM(L{part + rowCount},L{part + 1 + rowCount * 2},L{part + 2 + rowCount * 3})";

            worksheet.Cells[targetRow, targetCol].NumberFormat = "#,##0";
            worksheet.Cells[targetRow++, targetCol].Formula = $"='{worksheet4.Name}'!{worksheet4.Cells[part + rowCount, sourceCol].Address[false]}";
            worksheet.Cells[targetRow, targetCol].NumberFormat = "#,##0";
            worksheet.Cells[targetRow++, targetCol].Formula = $"='{worksheet4.Name}'!{worksheet4.Cells[part + 1 + rowCount * 2, sourceCol].Address[false]}";
            worksheet.Cells[targetRow, targetCol].NumberFormat = "#,##0";
            worksheet.Cells[targetRow++, targetCol].Formula = $"='{worksheet4.Name}'!{worksheet4.Cells[part + 2 + rowCount * 3, sourceCol].Address[false]}";

            //worksheet.Cells[targetRow, targetCol].NumberFormat = "#,##0";
            //worksheet.Cells[targetRow++, targetCol].Formula = $"='{worksheet4.Name}'!{worksheet4.Cells[part + 1 + rowCount * 2, sourceCol].Address[false]}";
            //worksheet.Cells[targetRow, targetCol].NumberFormat = "#,##0";
            //worksheet.Cells[targetRow++, targetCol].Formula = $"='금형비'!{workBook.Sheets["금형비"].Cells[part + 1 + rowCount * 2, sourceCol].Address[false]}";
        }

        private string SetMaterial(JObject data, Workbook workBook, int cnt)
        {
            try
            {
                Excel.Worksheet worksheet = workBook.Sheets["재료비 상세내역"];
                Excel.Worksheet worksheet3 = workBook.Sheets["견적원가 레포트"];
                Worksheet worksheet7 = workBook.Sheets["재료비"];
                worksheet.Select();

                int num = -1;
                int idx = 0;
                int addiction = 0;

                List<string> excelNameIdx = new List<string>();
                List<string> excelNumberIdx = new List<string>();
                Dictionary<int, List<string>> subExcelIdx = new Dictionary<int, List<string>>();

                // ** 📌 DataTable 생성 (엑셀 데이터 저장용) **
                DataTable dt = new DataTable();

                // ** 📌 고정 헤더 추가 **
                string[] baseHeaders = { "Level", "품번", "품명", "Q'TY", "단위", "소유량[kg]", "재료비갭", "탄소배출량갭", "Currency(Calculation)", "단위2", "Exchange rate (Blanks / assembly part)" };
                foreach (var header in baseHeaders)
                { 
                    if(header=="품번" || header == "품명") dt.Columns.Add(header, typeof(string));
                    else dt.Columns.Add(header, typeof(object));
                }                

                int dynamicStartCol = dt.Columns.Count; // 동적으로 추가될 컬럼 시작 위치
              
                // ** 📌 데이터 추가 (DataTable에 저장) **
                foreach (var element in data["data"])
                {
                    if (global.ConvertDouble(element["Level"]) == 0)
                    {
                        num++;
                        if (num != 0)
                        {
                            // ** Excel에 새 컬럼 추가 **
                            worksheet.Columns[7 + num].Insert(Excel.XlInsertShiftDirection.xlShiftToRight);
                            worksheet.Columns[10 + num].Insert(Excel.XlInsertShiftDirection.xlShiftToRight);
                            worksheet7.Columns[9 + num].Insert(Excel.XlInsertShiftDirection.xlShiftToRight);
                            worksheet7.Columns[13 + num].Insert(Excel.XlInsertShiftDirection.xlShiftToRight);

                            Excel.Range sourceRange = worksheet.Range[worksheet.Cells[1, 11 + 2 * num], worksheet.Cells[4, 23 + 2 * num]];
                            Excel.Range targetRange = worksheet.Range[worksheet.Cells[1, 11 + num * 13 + 2 * num], worksheet.Cells[4, 23 + num * 13 + 2 * num]];
                            sourceRange.Copy(targetRange);


                        }
                        foreach (var pair in subExcelIdx)
                        {
                            excelNameIdx.Insert(pair.Key - 5, pair.Value[0]);
                            excelNumberIdx.Insert(pair.Key - 5, pair.Value[1]);
                        }

                        // ** 🔥 새 컬럼이 생기면 DataTable에도 추가 **
                        string[] dynamicHeaders = { "설계중량[g]", "Material",  "Scrap", "Material costs","Material Management Cost",
                                                "Manufacturing costs", "Sales and admin costs", "R&D costs",
                                                "Transport costs", "Other costs", "Profit", "Total", "Carbon footprint" };

                        foreach (var header in dynamicHeaders)
                            dt.Columns.Add($"{header}_{num}", typeof(object));

                        dt.Columns.Add($"재료비_{num}", typeof(object));
                        dt.Columns[$"재료비_{num}"].SetOrdinal(6 + num);                        
                        dt.Columns.Add($"탄소배출량_{num}", typeof(object));
                        dt.Columns[$"탄소배출량_{num}"].SetOrdinal(8 + num * 2);
                        subExcelIdx.Clear();
                        addiction = 0;
                        continue;
                    }
                    else if (element["indicator"].ToString().Contains("Raw material"))
                    {
                        string col = $"Material_{num}";
                        if (element["품명"]?.ToString().ToUpper().Contains("SCRAP") == true) col = $"Scrap_{num}";
                        worksheet.Rows[idx - 2].Cells[col].Value = global.ConvertDouble(element["총액"]);
                    }
                    else
                    {
                        string name = element["품번"]?.ToString() ?? "";
                        string number = element["품명"]?.ToString() ?? "";

                        bool needInsert = false;
                      
                        if (num == 0)
                        {
                            excelNameIdx.Add(element["품번"]?.ToString() ?? "");
                            excelNumberIdx.Add(element["품명"]?.ToString() ?? "");
                            needInsert = true;
                        }
                        else if (name.Length > 0 && excelNameIdx.Contains(name)) idx = FindIndex(name, excelNameIdx, idx) + addiction;
                        else if (number.Length > 0 && excelNumberIdx.Contains(number)) idx = FindIndex(number, excelNumberIdx, idx) + addiction;
                        else
                        {
                            subExcelIdx.Add(idx, new List<string>() { name, number });
                            addiction++;
                            needInsert = true;
                        }

                        DataRow row = null;
                        if (needInsert)
                        {
                            // ** 📌 DataTable에 삽입할 경우 Insert 사용 **
                            row = dt.NewRow();
                            dt.Rows.InsertAt(row, idx);
                        }
                        else
                        {
                            row = dt.Rows[idx];
                        }

                        row["Level"] = global.ConvertDouble(element["Level"]);
                        row["품번"] = element["품번"]?.ToString() ?? "";
                        row["품명"] = element["품명"]?.ToString() ?? "";
                        row["단위"] = element["단위"]?.ToString() ?? "";
                        row["단위2"] = element["단위2"]?.ToString() ?? "";
                        row["Currency(Calculation)"] = element["Currency (Calculation)"]?.ToString() ?? "";
                        row["Q'TY"] = global.ConvertDouble(element["Q'TY"]);
                        row["소유량[kg]"] = global.ConvertDouble(element["소유량[kg]"]);
                        row["Exchange rate (Blanks / assembly part)"] = global.ConvertDouble(element["Exchange rate (Blanks / assembly part)"]);

                        int colOffset = dynamicStartCol + (num * 11); // 동적 컬럼 시작 위치
                        row[$"설계중량[g]_{num}"] = global.ConvertDouble(element["설계중량[g]"]);
                        row[$"Material costs_{num}"] = global.ConvertDouble(element["Material costs"]);
                        row[$"Material Management Cost_{num}"] = global.ConvertDouble(element["Material Management Cost, Total"]);
                        row[$"Manufacturing costs_{num}"] = global.ConvertDouble(element["Manufacturing costs III"]);
                        row[$"Sales and admin costs_{num}"] = global.ConvertDouble(element["Sales and general administration costs, Total"]);
                        row[$"R&D costs_{num}"] = global.ConvertDouble(element["Research and development costs, Total"]);
                        row[$"Transport costs_{num}"] = global.ConvertDouble(element["Transport costs, Total"]);
                        row[$"Other costs_{num}"] = global.ConvertDouble(element["Others, Total"]);
                        row[$"Profit_{num}"] = global.ConvertDouble(element["Profit, Total"]);
                        row[$"Total_{num}"] = global.ConvertDouble(element["총액"]);
                        row[$"Carbon footprint_{num}"] = global.ConvertDouble(element["Carbon footprint (Calculation)[kg CO2e]"]);

                        row[$"재료비_{num}"] = $"={global.NumberToLetter((cnt-1)*2+22+num*13 )}{idx+5}";
                        row[$"탄소배출량_{num}"] = $"={global.NumberToLetter((cnt - 1) * 2 + 23 + num * 13)}{idx+5}";

                        idx++;
                    }
                }               

                DataTable dt2 = new DataTable();
                
                List<object> list = new List<object>();
                for (int k = 0; k < 12+cnt*2; k++)
                {
                    dt2.Columns.Add($"{k}");
                }

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow row = dt.Rows[j];
                    if (global.ConvertDouble(row["Level"]) == 1)
                    {
                        if(list.Count>0)
                        {
                            DataRow d = dt2.Rows.Add();
                            worksheet7.Rows[dt2.Rows.Count+6].Insert(XlInsertShiftDirection.xlShiftDown);
                            for (int k= 0; k < list.Count; k++)
                            {
                                d[k] = list[k];
                            }

                            list.Clear();
                        }

                        list.Add(dt2.Rows.Count+1);
                        list.Add(row["품번"]);
                        list.Add(row["품명"]);
                        list.Add(row["Currency(Calculation)"]);
                        list.Add(row["Exchange rate (Blanks / assembly part)"]);

                        list.Add(row["단위2"]);
                        for(int cnt2=0; cnt2 <= cnt; cnt2++)
                        {
                            list.Add($"= '{worksheet.Name}'!{ worksheet.Cells[j+5, 7 + cnt2].Address[false]}");
                            //list.Add($"={global.NumberToLetter(7+cnt2)}{row}");
                        }                       

                        list.Add("kg Co2e");

                        for (int cnt2 = 0; cnt2 <= cnt; cnt2++)
                        {
                            list.Add($"= '{worksheet.Name}'!{ worksheet.Cells[j + 5, 7 + cnt2 + cnt + 2].Address[false]}");
                            //list.Add($"={global.NumberToLetter(7 + cnt2+cnt-1)}{row}");
                        }

                    }
                    else if (global.ConvertDouble(row["Level"]) == 2)
                    {
                        list.Clear();
                        DataRow d = dt2.Rows.Add ();
                        worksheet7.Rows[dt2.Rows.Count + 6].Insert(XlInsertShiftDirection.xlShiftDown);

                        list.Add(dt2.Rows.Count);
                        list.Add(row["품번"]);
                        list.Add(row["품명"]);
                        list.Add(row["Currency(Calculation)"]);
                        list.Add(row["Exchange rate (Blanks / assembly part)"]);

                        list.Add(row["단위2"]);
                        for (int cnt2 = 0; cnt2 <= cnt; cnt2++)
                        {                            
                            list.Add($"= '{worksheet.Name}'!{ worksheet.Cells[j+5, 7 + cnt2].Address[false]}");
                            //list.Add($"={global.NumberToLetter(7 + cnt2)}{row}");
                        }

                        list.Add("kg Co2e");

                        for (int cnt2 = 0; cnt2 <= cnt; cnt2++)
                        {
                            list.Add($"= '{worksheet.Name}'!{ worksheet.Cells[j+5, 7 + cnt2 + cnt +2].Address[false]}");
                            //list.Add($"={global.NumberToLetter(7 + cnt2 + cnt - 1)}{row}");
                        }

                        for (int k = 0; k < list.Count; k++)
                        {
                            d[k] = list[k];
                        }

                        list.Clear();
                    }
                }

                if (list.Count > 0)
                {
                    DataRow d = dt2.Rows.Add();
                    worksheet7.Rows[dt2.Rows.Count + 6].Insert(XlInsertShiftDirection.xlShiftDown);
                    for (int k = 0; k < list.Count; k++)
                    {
                        d[k] = list[k];
                    }

                    list.Clear();
                }
                DataRow d2 = dt2.Rows.Add();

                worksheet7.Rows[dt2.Rows.Count + 5].Delete(XlDeleteShiftDirection.xlShiftUp);

                dt.Columns.Remove("Currency(Calculation)");
                dt.Columns.Remove("단위2");
                dt.Columns.Remove("Exchange rate (Blanks / assembly part)");
                // ** 🚀 한 번에 Excel에 쓰기 (DataTable → Excel) **
                int startRow = 5;
                int startCol = 1; // **🔥 5번째 열부터 데이터 시작**

                Excel.Range writeRange = worksheet.Range[worksheet.Cells[startRow, startCol], worksheet.Cells[startRow + dt.Rows.Count - 1, startCol + dt.Columns.Count - 1]];
                Excel.Range sourceRow = worksheet.Range[worksheet.Cells[startRow, startCol], worksheet.Cells[startRow, startCol + dt.Columns.Count - 1]];

                WriteDataToExcel(dt, worksheet, startRow, startCol);
                for (int cnt2 = 0; cnt2 <= cnt; cnt2++)
                {
                    d2[cnt2 + 6] = $"=SUM({global.NumberToLetter(cnt2 + 9)}6:{global.NumberToLetter(cnt2 + 9)}{dt2.Rows.Count + 4})";
                    if (cnt2 == cnt) continue;
                    worksheet3.Rows[33 + cnt-1].Cells[5 + cnt2 * 2].Formula = $"= '{worksheet7.Name}'!{ worksheet7.Cells[dt2.Rows.Count + 5, cnt2 + 9].Address[false]}";
                    worksheet3.Rows[33 + cnt-1].Cells[5 + cnt2 * 2].NumberFormat = "#,##0";
                    worksheet3.Rows[46 + cnt - 1].Cells[5 + cnt2 * 2].NumberFormat = "0.00%";                    
                    //list.Add($"={global.NumberToLetter(7 + cnt2)}{row}");
                }

                for (int cnt2 = 0; cnt2 <= cnt; cnt2++)
                {
                    d2[cnt2 + cnt+8] = $"=SUM({global.NumberToLetter(cnt2+ cnt + 11)}6:{global.NumberToLetter(cnt2 + cnt + 11)}{dt2.Rows.Count +4})";
                    if (cnt2 == cnt) continue;
                    worksheet3.Rows[47 + cnt - 1].Cells[5 + cnt2 * 2].Formula = $"= '{worksheet7.Name}'!{ worksheet7.Cells[dt2.Rows.Count + 5, cnt2 + cnt + 11].Address[false]}";
                    worksheet3.Rows[47 + cnt - 1].Cells[5 + cnt2 * 2].NumberFormat = "#,##0";
                    worksheet3.Rows[46 + cnt - 1].Cells[5 + cnt2 * 2].NumberFormat = "0.00%";
                    //list.Add($"={global.NumberToLetter(7 + cnt2 + cnt - 1)}{row}");
                }               

                // ** 📌 테두리 적용 **
                writeRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                writeRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                // **🚀 서식만 복사하기**
                sourceRow.Copy();
                writeRange.PasteSpecial(Excel.XlPasteType.xlPasteFormats);
                sourceRow.Application.CutCopyMode = 0;
                // 클립보드 해제 (엑셀 실행 속도 최적화)
  
                WriteDataToExcel(dt2, worksheet7, 6, 3);

                worksheet.Range[worksheet.Cells[5, 7], worksheet.Cells[6 + dt.Rows.Count - 1, 7+dt.Columns.Count]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                worksheet.Range[worksheet.Cells[5, 1], worksheet.Cells[6 + dt.Rows.Count - 1, 6]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                worksheet7.Range[worksheet7.Cells[6, 4], worksheet7.Cells[6 + dt2.Rows.Count - 1, 4]].NumberFormat = "@";
                worksheet.Range[worksheet.Cells[5, 2], worksheet.Cells[6 + dt.Rows.Count - 1, 2]].NumberFormat = "@";
                worksheet7.Range[worksheet7.Cells[6, 3], worksheet7.Cells[6 + dt2.Rows.Count - 1,dt2.Columns.Count - 2]].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private int FindIndex(string name, List<string> list, int idx)
        {
            List<int> indices = list.Select((item, index) => new { item, index })
                             .Where(x => x.item == name)
                             .Select(x => x.index)
                             .ToList();

            if (indices == null || indices.Count == 0)
            {
                throw new ArgumentException("indices 리스트가 비어있거나 null입니다.");
            }

            int closestValue = indices[0];
            int minDifference = Math.Abs(indices[0] - idx);
            for (int i = 1; i < indices.Count; i++)
            {
                int difference = Math.Abs(indices[i] - idx);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestValue = indices[i];
                }
            }

            return closestValue;
        }

        private double? FindMostFrequents(Dictionary<int, List<double>> dict, int idx)
        {
            if (!dict.ContainsKey(idx)) return 0;

            return dict[idx].GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
        }
        private string WriteOptimizedToExcel(Excel.Workbook workBook, System.Data.DataTable requirements, double after, ref List<string> partName, ref List<string> partNo, ref int addRowIndex, ref int rowIndex, ref int colIndex)
        {
            try
            {
                Excel.Worksheet worksheet = workBook.Sheets["견적원가 레포트"];
                Excel.Worksheet worksheet2 = workBook.Sheets["년간손익"];
                Excel.Worksheet worksheet3 = workBook.Sheets["경제성 분석"];
                Excel.Worksheet worksheet5 = workBook.Sheets["투자 및 감상비"];

                var dataToWrite = new List<(Excel.Range, object)>();
                var dataToFormula = new List<(Excel.Range, object)>();
                var dataToFormat = new List<(Excel.Range, object)>();

                int totalRow = requirements.Rows.Count - 1;
                DataTable investment = new DataTable();
               
                foreach (DataRow row in requirements.Rows)
                {                    
                    if (rowIndex != 11)
                    {
                        worksheet.Rows[rowIndex].Insert(XlInsertShiftDirection.xlShiftDown);
                        addRowIndex++;
                    }
                    colIndex = 3;
                    if (partName.Count != 0)
                    {
                        worksheet.Range[worksheet.Cells[27 + addRowIndex, 7], worksheet.Cells[48 + addRowIndex, 8]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                        Excel.Range sourceRange = worksheet2.Range[worksheet2.Cells[16, 5], worksheet2.Cells[37, 7 + after]];
                        Excel.Range sourceRange2 = worksheet2.Range[worksheet2.Cells[16, 3], worksheet2.Cells[37, 4]];

                        Excel.Range targetRange = worksheet2.Range[worksheet2.Cells[16 + 22 * (partName.Count), 5], worksheet2.Cells[37 + 22 * (partName.Count), 7 + after]];
                        Excel.Range targetRange2 = worksheet2.Range[worksheet2.Cells[16 + 22 * (partName.Count), 3], worksheet2.Cells[37 + 22 * (partName.Count), 4]];

                        // 원본 범위를 복사
                        targetRange.Copy();
                        targetRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        targetRange.Application.CutCopyMode = 0;

                        sourceRange2.Copy();
                        targetRange2.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        sourceRange2.Application.CutCopyMode = 0;
                    }
                    //worksheet2.Cells[16 + partName.Count * 22, 3].Value = worksheet.Cells[rowIndex, colIndex].Value = worksheet.Cells[29 + addRowIndex, 5 + partName.Count * 2].Value = row["Name_LOC_Extracted"];
                    string partNameValue = row["Name_LOC_Extracted"].ToString();
                    dataToWrite.Add((worksheet2.Cells[16 + partName.Count * 22, 3], partNameValue));
                    dataToWrite.Add((worksheet.Cells[rowIndex, colIndex], partNameValue));
                    dataToWrite.Add((worksheet.Cells[29 + addRowIndex, 5 + partName.Count * 2], partNameValue));

                    worksheet.Range[worksheet.Cells[29 + addRowIndex, 5 + partName.Count * 2], worksheet.Cells[29 + addRowIndex, 5 + partName.Count * 2 + 1]].Merge();

                    dataToWrite.Add((worksheet.Cells[30 + addRowIndex, 5 + partName.Count * 2], "금액"));
                    dataToWrite.Add((worksheet.Cells[30 + addRowIndex, 5 + partName.Count * 2 + 1], "비중"));
                    dataToWrite.Add((worksheet.Cells[31 + addRowIndex, 5 + partName.Count * 2], row["SalesPrice"]));
                    dataToFormat.Add((worksheet.Cells[31 + addRowIndex, 5 + partName.Count * 2], "#,##0"));

                    string colName = global.NumberToLetter(5 + partName.Count * 2);
                    int totalRowIndex = requirements.Rows.Count - 1;
                    var formulas = new (int rowOffset, string formula)[]
                    {
                        (32, $"=SUM({colName}{40 + totalRowIndex}:{colName}{44 + totalRowIndex})"),
                        (40, $"=SUM({colName}{33 + totalRowIndex}:{colName}{39 + totalRowIndex})"),
                        (41, $"={colName}{31 + totalRowIndex}*G{24 + totalRowIndex}"),
                        (42, "='포장 및 물류비 '!N11"),
                        (43, "='포장 및 물류비 '!J23"),
                        (44, $"=({colName}{31 + totalRowIndex}-재료비!J17)*K{22 + totalRowIndex}"),
                        (45, $"={colName}{31 + totalRowIndex}-{colName}{32 + totalRowIndex}"),
                        (46, $"={colName}{45 + totalRowIndex}/{colName}{31 + totalRowIndex}")
                    };
                    foreach (var (rowOffset, formula) in formulas)
                    {
                        dataToFormula.Add((worksheet.Cells[rowOffset + addRowIndex, 5 + partName.Count * 2], formula));
                        dataToFormat.Add((worksheet.Cells[rowOffset + addRowIndex, 5 + partName.Count * 2], "#,##0"));
                    }

                    int sourceRow = 31 + totalRow;
                    int sourceCol = 5 + partName.Count * 2;
                    int targetRow = 17 + partName.Count * 22;

                    for (int i = 0; i < 14; i++)
                    {
                        dataToFormula.Add((worksheet2.Cells[targetRow + i, 5], $"='{worksheet.Name}'!{worksheet.Cells[sourceRow++, sourceCol].Address[false]}"));
                    }

                    partName.Add(row["Name_LOC_Extracted"].ToString());
                    partNo.Add(row["PartNo"].ToString());

                    worksheet.Range[worksheet.Cells[rowIndex, colIndex], worksheet.Cells[rowIndex, colIndex + 1]].Merge();
                    colIndex += 2;

                    dataToFormula.Add((worksheet3.Cells[11, 6], $"=SUM(F8:F10)"));
                    dataToFormula.Add((worksheet3.Cells[12, 6], $"='{worksheet5.Name}'!{worksheet5.Cells[15, 6].Address[false]}"));

                    dataToFormula.Add((worksheet3.Cells[15, 6], $"=SUM(F12:F14)"));
                    dataToFormula.Add((worksheet3.Cells[16, 6], $"=F11-F15"));
                    dataToFormula.Add((worksheet3.Cells[16, 6], $"=F16"));
                    dataToFormula.Add((worksheet3.Cells[16, 6], $"=F18"));

                    dataToFormula.Add((worksheet3.Cells[20, 6], $"=IRR({global.NumberToLetter(6)}16:{global.NumberToLetter(6 + (int)after)}16)"));
                    dataToFormula.Add((worksheet3.Cells[21, 6], $"=SUM({global.NumberToLetter(6)}16:{global.NumberToLetter(6 + (int)after)}16)"));
                    dataToFormula.Add((worksheet3.Cells[22, 6], $"=IF(ISERROR(SUM({global.NumberToLetter(7)}22:{global.NumberToLetter(6 + (int)after)}22))>0,(SUM({global.NumberToLetter(7)}22:{global.NumberToLetter(6 + (int)after)}22)),\"회수불가\")"));

                    for (int i = 0; i < after; i++)
                    {
                        dataToWrite.Add((worksheet.Cells[rowIndex, colIndex], row[$"ManualValue{i}"]));
                        dataToFormat.Add((worksheet.Cells[rowIndex, colIndex++], "#,##0"));
                        dataToFormula.Add((worksheet2.Cells[16 + (partName.Count - 1) * 22, 5 + i], $"='{worksheet.Name}'!{worksheet.Cells[11 + (partName.Count - 1), 5 + i].Address[false]}"));

                        int year = 52 + addRowIndex;
                        int sourceCol2 = 5 + i;

                        dataToFormula.Add((worksheet.Cells[year++, sourceCol2], $"='{worksheet2.Name}'!{worksheet2.Cells[39 + (partName.Count - 1) * 22, sourceCol2].Address[false]}"));
                        dataToFormula.Add((worksheet.Cells[year++, sourceCol2], $"='{worksheet2.Name}'!{worksheet2.Cells[53 + (partName.Count - 1) * 22, sourceCol2].Address[false]}"));
                        dataToFormula.Add((worksheet.Cells[year++, sourceCol2], $"=E{year - 2}/E{year - 3}"));
                        dataToFormula.Add((worksheet.Cells[year++, sourceCol2], $"='{worksheet2.Name}'!{worksheet2.Cells[57 + (partName.Count - 1) * 22, sourceCol2].Address[false]}"));
                        dataToFormula.Add((worksheet.Cells[year, sourceCol2], $"=E{year - 1}/E{year - 4}"));

                        int cnt = 8;
                        int colCnt = 7 + i;
                        //dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"='{worksheet5.Name}'!{worksheet5.Cells[53 + (partName.Count - 1) * 22, sourceCol2].Address[false]}"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"='{worksheet2.Name}'!{worksheet2.Cells[53 + (partName.Count - 1) * 22, sourceCol2].Address[false]}"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"='{worksheet5.Name}'!{worksheet5.Cells[24, sourceCol2+1].Address[false]}"));

                        cnt = 11;
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"=SUM({global.NumberToLetter(6 + i)}8:{global.NumberToLetter(6 + i)}10)"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"='{worksheet5.Name}'!{worksheet5.Cells[15, sourceCol2 + 1].Address[false]}"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"='{worksheet2.Name}'!{worksheet2.Cells[57 + (partName.Count - 1) * 22, sourceCol2].Address[false]}"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"='{worksheet2.Name}'!{worksheet2.Cells[39 + (partName.Count - 1) * 22, sourceCol2].Address[false]}*$H4"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"=SUM({global.NumberToLetter(colCnt)}12:{global.NumberToLetter(colCnt)}14)"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"={global.NumberToLetter(colCnt)}11-{global.NumberToLetter(colCnt)}15"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"={global.NumberToLetter(colCnt - 1)}17-{global.NumberToLetter(colCnt)}16"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"={global.NumberToLetter(colCnt)}16/(1+$D4)^{global.NumberToLetter(colCnt)}7"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, colCnt], $"={global.NumberToLetter(colCnt - 1)}19+{global.NumberToLetter(colCnt)}18"));

                        cnt = 69;
                        int cnt2 = 11;

                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2, 6 + i].Address[false]}"));
                        cnt2 = 15;
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false]}"));
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false]}"));
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false]}"));
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false]}"));
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false]}"));

                        int profitTargetRow = 17 + (partName.Count - 1) * 22;
                        int profitTargetCol = 5 + i;
                        int profitSourceRow = 4;

                        if (i != 0)
                        {
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}*(1-{global.NumberToLetter(profitTargetCol)}{profitSourceRow++})"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"=SUM({global.NumberToLetter(profitTargetCol)}{profitTargetRow }:{global.NumberToLetter(profitTargetCol)}{profitTargetRow + 4 })"));

                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}*(1-{global.NumberToLetter(profitTargetCol)}{profitSourceRow++})"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}*(1+{global.NumberToLetter(profitTargetCol)}{profitSourceRow++})"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}*(1+{global.NumberToLetter(profitTargetCol)}{profitSourceRow++})"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}*(1+{global.NumberToLetter(profitTargetCol)}{profitSourceRow++})"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"=SUM({global.NumberToLetter(profitTargetCol)}{profitTargetRow - 8}:{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2 })"));

                            dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow-2}*{global.NumberToLetter(profitTargetCol)}{profitSourceRow++}"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow - 2}"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow++}*{global.NumberToLetter(profitTargetCol)}{profitSourceRow++}"));

                            dataToFormula.Add((worksheet3.Cells[22, 6 + i], $"=IF(AND({global.NumberToLetter(6 + i)}17<0,{global.NumberToLetter(6 + i + 1)}17>=0),{global.NumberToLetter(6 + i)}7+ABS({global.NumberToLetter(6 + i)}17/{global.NumberToLetter(6 + i + 1)}16),\"\")"));

                        }
                        if(partName.Count==1)
                        {
                            investment.Columns.Add($"{i}");
                            for (int rowOffset = 0; rowOffset < 3; rowOffset++)
                            {
                                DataRow investmentRow = investment.NewRow();
                                if (investment.Rows.Count<3) investmentRow = investment.Rows.Add();
                                else  investmentRow = investment.Rows[rowOffset];
                                investmentRow[$"{i}"] = $"='{worksheet5.Name}'!{worksheet5.Cells[26 + rowOffset, 6 + i].Address[false]}";
                            }
                        }


                        profitTargetRow = 31 + (partName.Count - 1) * 22;
                        profitSourceRow = 11;
                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 15}-{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 14}"));
                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}/{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 16}"));

                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol],
                            $"='{worksheet5.Name}'!{worksheet5.Cells[15, profitTargetCol + 1].Address[false]} * {global.NumberToLetter(profitTargetCol)}{profitSourceRow++}/'{worksheet.Name}'!{worksheet.Cells[12 + requirements.Rows.Count-1, profitTargetCol ].Address[false]}"));

                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 4}-{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}"));
                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}*{global.NumberToLetter(profitTargetCol)}{profitSourceRow++}"));
                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 3}-{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}"));
                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}/{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 21}"));

                        if (totalRow == addRowIndex)
                        {
                            dataToFormula.Add((worksheet.Cells[12 + addRowIndex, 5 + i], $"=SUM({global.NumberToLetter(5 + i)}{11}:{global.NumberToLetter(5 + i)}{11 + addRowIndex})"));
                            dataToFormat.Add((worksheet.Cells[12 + addRowIndex, 5 + i], "#,##0"));
                        }
                    }

                    if (totalRow == addRowIndex)
                    {
                        dataToFormula.Add((worksheet.Cells[12 + addRowIndex, 5 + after], $"=SUM({global.NumberToLetter(5 + (int)after)}{11}:{global.NumberToLetter(5 + (int)after)}{11 + addRowIndex})"));
                        dataToFormat.Add((worksheet.Cells[12 + addRowIndex, 5 + after], "#,##0"));
                    }

                    dataToFormula.Add((worksheet.Cells[75+addRowIndex, 5], $"='{worksheet3.Name}'!{worksheet3.Cells[20, 6].Address[false]}"));
                    dataToFormula.Add((worksheet.Cells[76 + addRowIndex, 5], $"='{worksheet3.Name}'!{worksheet3.Cells[21, 6].Address[false]}"));
                    dataToFormula.Add((worksheet.Cells[77 + addRowIndex, 5], $"='{worksheet3.Name}'!{worksheet3.Cells[22, 6].Address[false]}"));

                    dataToFormula.Add((worksheet.Cells[rowIndex, colIndex], $"=SUM(E{rowIndex}:{global.NumberToLetter(colIndex - 1)}{rowIndex})"));

                    rowIndex++;
                }

                WriteDataToExcel(investment, worksheet,61+addRowIndex,5);
                worksheet.Range[worksheet.Cells[addRowIndex + 69, 5], worksheet.Cells[addRowIndex + 74, ((int)5 + after+1)]].NumberFormat = "#,##0,,;\"▲\"#,##0,,;\"-\"";
                worksheet.Range[worksheet.Cells[addRowIndex + 69, 5], worksheet.Cells[addRowIndex + 74, ((int)5 + after + 1)]].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                worksheet.Range[worksheet.Cells[addRowIndex + 69, 5], worksheet.Cells[addRowIndex + 74, ((int)5 + after + 1)]].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Range[worksheet.Cells[addRowIndex + 75, 3], worksheet.Cells[addRowIndex + 77, 3]].Copy();
                worksheet.Range[worksheet.Cells[addRowIndex + 75, 5], worksheet.Cells[addRowIndex + 77, 5]].PasteSpecial(Excel.XlPasteType.xlPasteFormats);

                // 클립보드 해제 (엑셀 실행 속도 최적화)
                worksheet.Application.CutCopyMode = 0;

                worksheet.Cells[addRowIndex + 75,5].NumberFormat = "#,##0.0%;\"▲\" #,##0.0%;\" - \"";
                worksheet.Cells[addRowIndex + 76,5].NumberFormat = "#,##0,,;\"▲\"#,##0,,;\" - \"";
                worksheet.Cells[addRowIndex + 77,5].NumberFormat = "##,##0.0\"년\"";
                worksheet.Range[worksheet.Cells[addRowIndex + 51, 5], worksheet.Cells[addRowIndex + 56, ((int)5 + after+1)]].NumberFormat = "#,##0,,";
                worksheet.Range[worksheet.Cells[addRowIndex + 54, 5], worksheet.Cells[addRowIndex + 54, ((int)5 + after+1)]].NumberFormat = "0.00%";
                worksheet.Range[worksheet.Cells[addRowIndex + 56, 5], worksheet.Cells[addRowIndex + 56, ((int)5 + after+1)]].NumberFormat = "0.00%";

                Excel.Application excelApp = workBook.Application;
                //excelApp.ScreenUpdating = false;  // 화면 업데이트 중지 (속도 향상)
                excelApp.Calculation = Excel.XlCalculation.xlCalculationManual; // 자동 계산 비활성화
                                                                                // *** 최적화 적용 ***
                ApplyDataToExcelGroup(dataToWrite, dataToFormula, dataToFormat);

                excelApp.Calculation = Excel.XlCalculation.xlCalculationAutomatic; // 자동 계산 다시 활성화
                //excelApp.ScreenUpdating = true; // 화면 업데이트 다시 활성화
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }

        private void ApplyDataToExcelGroup(List<(Excel.Range, object)> dataToWrite, List<(Excel.Range, object)> dataToFormula, List<(Excel.Range, object)> dataToFormat)
        {
            // ** 값을 입력할 데이터와 수식을 입력할 데이터를 합치기 **
            var combinedData = new List<(Excel.Range, object)>(dataToWrite);
            combinedData.AddRange(dataToFormula);

            // ** 비슷한 범위끼리 그룹화 **
            var groupedData = GroupRanges(combinedData);

            // ** 그룹별로 한 번에 값/수식 입력 **
            foreach (var group in groupedData)
            {
                var ranges = group.Select(d => d.Item1).ToArray();
                var values = group.Select(d => d.Item2).ToArray();
                ranges.First().Worksheet.Select();
                ranges.First().Select();
                SetValues(ranges, values);
            }

            // ** 포맷을 한 번에 입력 **
            if (dataToFormat.Count > 0)
            {
                foreach (var (range, format) in dataToFormat)
                {
                    range.NumberFormat = format;
                }
            }
        }
        private void ApplyDataToExcel(List<(Excel.Range, object)> dataToWrite, List<(Excel.Range, object)> dataToFormula, List<(Excel.Range, object)> dataToFormat)
        {
            var combinedData = new List<(Excel.Range, object)>(dataToWrite);
            combinedData.AddRange(dataToFormula);

            // ** Excel에 한 번에 쓰도록 처리 **
            if (combinedData.Count > 0)
            {
                foreach (var data in combinedData)
                {
                    data.Item1.Value2 = data.Item2;
                }                
            }

            // ** 포맷을 한 번에 입력 **
            if (dataToFormat.Count > 0)
            {
                foreach (var (range, format) in dataToFormat)
                {
                    range.NumberFormat = format;
                }
            }
        }

        private List<List<(Excel.Range, object)>> GroupRanges(List<(Excel.Range, object)> data)
        {
            // 워크시트별로 그룹화 (워크시트가 다르면 그룹화하면 안됨)
            var sheetGroups = data.GroupBy(d => d.Item1.Worksheet);

            var groupedData = new List<List<(Excel.Range, object)>>();

            foreach (var sheetGroup in sheetGroups)
            {
                // 정렬: 행(row) → 열(column) 순서로 정렬 (비슷한 범위끼리 인접하게 만들기)
                var sortedData = sheetGroup.OrderBy(d => d.Item1.Row).ThenBy(d => d.Item1.Column).ToList();

                List<(Excel.Range, object)> currentGroup = new List<(Excel.Range, object)>();
                Excel.Range lastRange = null;

                foreach (var item in sortedData)
                {
                    Excel.Range currentRange = item.Item1;

                    if (lastRange != null && AreRangesSimilar(lastRange, currentRange))
                    {
                        // 기존 그룹에 추가
                        currentGroup.Add(item);
                    }
                    else
                    {
                        // 새로운 그룹 생성
                        if (currentGroup.Count > 0) groupedData.Add(currentGroup);
                        currentGroup = new List<(Excel.Range, object)> { item };
                    }

                    lastRange = currentRange;
                }

                // 마지막 그룹 추가
                if (currentGroup.Count > 0) groupedData.Add(currentGroup);
            }

            return groupedData;
        }


        private bool AreRangesSimilar(Excel.Range range1, Excel.Range range2)
        {
            // 같은 워크시트에서만 그룹화
            if (range1.Worksheet != range2.Worksheet)
                return false;

            // 같은 행(Row) 그룹 (열은 다를 수 있음)
            if (range1.Row == range2.Row && Math.Abs(range1.Column - range2.Column) <= 1)
                return true;

            // 같은 열(Column) 그룹 (행은 다를 수 있음)
            if (range1.Column == range2.Column && Math.Abs(range1.Row - range2.Row) <= 1)
                return true;

            return false;
        }

        private void SetValues(Excel.Range[] ranges, object[] values)
        {
            try
            {
                if (ranges.Length == 1)
                {
                    ranges[0].Value2 = values[0]; // 단일 값이면 바로 입력
                    return;
                }

                // ** 그룹화된 범위를 하나의 연속된 범위로 변환 **
                Excel.Range firstCell = ranges.First();
                Excel.Range lastCell = ranges.Last();
                Excel.Worksheet ws = firstCell.Worksheet;
                Excel.Range fullRange = ws.Range[firstCell, lastCell];

                // ** 전체 행과 열 개수 계산 **
                int rowCount = fullRange.Rows.Count;
                int colCount = fullRange.Columns.Count;
                object[,] valueArray = new object[rowCount, colCount];

                // ** Dictionary를 사용하여 원래 위치 매핑 **
                Dictionary<(int, int), object> valueMap = new Dictionary<(int, int), object>();

                for (int i = 0; i < ranges.Length; i++)
                {
                    int row = ranges[i].Row - firstCell.Row; // 상대적인 위치
                    int col = ranges[i].Column - firstCell.Column;
                    valueMap[(row, col)] = values[i];
                }

                // ** valueArray에 값 채우기 (없는 값은 null로 채움) **
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        if (valueMap.TryGetValue((i, j), out object value))
                            valueArray[i, j] = value;
                        else
                            valueArray[i, j] = null; // 빈 값 처리
                    }
                }

                // ** 한 번에 입력 **
                fullRange.Value2 = valueArray;


            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
        }

        public void WriteDataToExcel(DataTable table, Excel.Worksheet worksheet4, int startRow, int startCol)
        {
            int rowCount = table.Rows.Count;
            int colCount = table.Columns.Count;

            object[,] dataArray = new object[rowCount, colCount];

            // DataTable의 데이터를 배열로 변환 (Excel에 한 번에 입력하기 위함)
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    dataArray[r, c] = table.Rows[r][c];
                }
            }

            // Excel Range 지정하여 한 번에 데이터 입력
            Excel.Range startCell = (Excel.Range)worksheet4.Cells[startRow, startCol];
            Excel.Range endCell = (Excel.Range)worksheet4.Cells[startRow + rowCount - 1, startCol+ colCount-1];
            Excel.Range writeRange = worksheet4.Range[startCell, endCell];

            startCell.Worksheet.Select();
            startCell.Select();
            writeRange.Value2 = dataArray;

            // 숫자 포맷 적용
            worksheet4.Range[startCell, endCell].NumberFormat = "#,##0";

            //// 자동 합계 적용
            //int totalRow = startRow + rowCount;
            //worksheet4.Cells[totalRow, colCount].Formula = $"=SUM({endCell.Address[false]})";
        }

    }
}
