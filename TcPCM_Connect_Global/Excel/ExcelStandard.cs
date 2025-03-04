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
                worksheet3.Cells[6, 6].Value = sop - 1;
                for (int i = 0; i < after; i++)
                {
                    worksheet.Range[worksheet.Cells[rowIndex - 2, 5 + i], worksheet.Cells[rowIndex + 1, 5 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                    worksheet.Range[worksheet.Cells[50, 5 + i], worksheet.Cells[74, 5 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                    worksheet2.Range[worksheet2.Cells[3, 5 + i], worksheet2.Cells[60, 5 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                    worksheet3.Range[worksheet3.Cells[5, 7 + i], worksheet3.Cells[23, 7 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동
                    worksheet4.Range[worksheet4.Cells[4, 5 + i], worksheet4.Cells[10, 5 + i]].Insert(XlInsertShiftDirection.xlShiftToRight); // 오른쪽으로 셀 이동

                    Range range = worksheet.Range[worksheet.Cells[rowIndex - 1, 5 + i], worksheet.Cells[rowIndex + 1, 5 + i]];
                    Range range2 = worksheet2.Range[worksheet2.Cells[3, 5 + i], worksheet2.Cells[60, 5 + i]];
                    Range range3 = worksheet3.Range[worksheet3.Cells[5, 7 + i], worksheet3.Cells[23, 7 + i]];
                    Range range4 = worksheet4.Range[worksheet4.Cells[4, 5 + i], worksheet4.Cells[10, 5 + i]];

                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    worksheet2.Range[worksheet2.Cells[15, 5 + i], worksheet2.Cells[59, 5 + i]].Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.ColumnWidth = 12;
                    range2.ColumnWidth = 10;
                    range3.ColumnWidth = 10.38;
                    range4.ColumnWidth = 13.5;
                    worksheet.Cells[rowIndex - 1, 5 + i].Value = worksheet2.Cells[15, 5 + i].Value = worksheet4.Cells[4, 5 + i].Value = worksheet3.Cells[6, 7 + i].Value = sop + i;
                    worksheet3.Cells[7, 7 + i].Value = i;
                    worksheet.Cells[51, 5 + i].Value = worksheet.Cells[60, 5 + i].Value = worksheet.Cells[68, 5 + i].Value = $"{sop + i}년";
                }

                string errRequirement = WriteOptimizedToExcel(workBook, requirements, after, ref partName, ref addRowIndex, ref rowIndex, ref colIndex);
                if (errRequirement != null) err += ("\n" + errRequirement);

                worksheet.Range[worksheet.Cells[28 + addRowIndex, 5], worksheet.Cells[28 + addRowIndex, 5 + (partName.Count - 1) * 2 + 1]].Merge();

                Interface export = new Interface();
                JObject apiResult = export.LoadCalc(calcList, "Estimate");
                if (apiResult == null) return "데이터 조회 시 오류가 발생하였습니다.";
                err += SetMaterial(apiResult, workBook, calcList.Count);

                string err2 = SetManufacturing(workBook, calcList, (int)after, addRowIndex, partName);
                if (err2 != null) err += ("\n" + err2);

                worksheet2.Select();

                SetAnnualProfitAndLoss(workBook, requirements, sop, (int)after, partName, addRowIndex);


            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                //if (workBook != null)
                //{
                //    //변경점 저장하면서 닫기
                //    workBook.Close(true);
                //    //Excel 프로그램 종료
                //    application.Quit();
                //    //오브젝트 해제1
                //    ExcelCommon.ReleaseExcelObject(workBook);
                //    ExcelCommon.ReleaseExcelObject(application);
                //}
            }


            return err;
        }

        //public void GenerateFormula(Excel.Worksheet worksheet2, int i, List<string> partName)
        //{
        //    int target = 16 + 22 * partName.Count;
        //    List<int> baseRows = new List<int>();
        //    for(int j=0;j<partName.Count;j++)
        //    {
        //        baseRows.Add(16+j*22);
        //    }

        //    List<int> multiplierRows = new List<int>();
        //    for (int j = 17; j <= 37; j++)
        //    {
        //        multiplierRows.Add(j);
        //    }

        //    int partCount = partName.Count; // part 개수

        //    if (partCount < 1)
        //    {
        //        throw new ArgumentException("partName의 개수는 최소 1개 이상이어야 합니다.");
        //    }

        //    // 1. 첫 번째 수식 (더하기)
        //    string sumFormula = string.Join("+", baseRows.Take(partCount).Select(row => $"E${row}"));
        //    worksheet2.Cells[target++, i + 5].Formula = $"={sumFormula}";

        //    // 2. 나머지 수식 (곱셈 및 덧셈)
        //    foreach (int multiplierRow in multiplierRows)
        //    {
        //        string formulaParts = string.Join("+", Enumerable.Range(0, partCount)
        //            .Select(j => $"E${baseRows[j]}*E${multiplierRow + (j * 22)}"));

        //        if(multiplierRow==32) worksheet2.Cells[target++, i + 5].Formula = $"=E{baseRows.Last() + 37}/E{baseRows.Last() + 23}";
        //        else if(multiplierRow == 37) worksheet2.Cells[target++, i + 5].Formula = $"=E{baseRows.Last()+42}/E{baseRows.Last() + 23}";
        //        else worksheet2.Cells[target++, i + 5].Formula = $"={formulaParts}";
        //    }
        //}
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
                int startCol = 4; // **🔥 5번째 열부터 데이터 시작**

                int rowCount = increaseDataTable.Rows.Count;
                int colCount = increaseDataTable.Columns.Count;

                Excel.Range startCell = worksheet2.Cells[startRow, startCol];
                Excel.Range endCell = worksheet2.Cells[startRow + rowCount - 1, startCol + colCount - 1];
                Excel.Range writeRange = worksheet2.Range[startCell, endCell];

                // ** 🔥 Excel 범위에 DataTable 데이터 한 번에 입력 **
                object[,] dataArray = new object[rowCount, colCount];
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        dataArray[i, j] = increaseDataTable.Rows[i][j];
                    }
                }

                writeRange.Value2 = dataArray;

                Excel.Range sourceRow = worksheet2.Range[worksheet2.Cells[startRow, startCol + colCount], worksheet2.Cells[startRow + rowCount - 1, startCol + colCount]];

                // ** 📌 테두리 적용 **
                writeRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                // **🚀 서식만 복사하기**
                sourceRow.Copy();
                writeRange.PasteSpecial(Excel.XlPasteType.xlPasteFormats);
                // 클립보드 해제 (엑셀 실행 속도 최적화)
                worksheet.Application.CutCopyMode = 0;

                // ** 🚀 한 번에 Excel에 쓰기 (DataTable → Excel) **
                startRow = 16 + 22 * partName.Count;
                 startCol = 5; // **🔥 5번째 열부터 데이터 시작**

                 rowCount = formulaDataTable.Rows.Count;
                 colCount = formulaDataTable.Columns.Count;

                startCell = worksheet2.Cells[startRow, startCol];
                endCell = worksheet2.Cells[startRow + rowCount - 1, startCol + colCount - 1];
                writeRange = worksheet2.Range[startCell, endCell];

                // ** 🔥 Excel 범위에 DataTable 데이터 한 번에 입력 **
                object[,] dataArray2 = new object[rowCount, colCount];
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        dataArray2[i, j] = formulaDataTable.Rows[i][j];
                    }
                }

                writeRange.Value2 = dataArray2;

                sourceRow = worksheet2.Range[worksheet2.Cells[16, startCol], worksheet2.Cells[37, startCol + colCount - 1]];

                // **🚀 서식만 복사하기**
                sourceRow.Copy();
                writeRange.PasteSpecial(Excel.XlPasteType.xlPasteFormats);

                // 클립보드 해제 (엑셀 실행 속도 최적화)
                worksheet.Application.CutCopyMode = 0;

                // ** 5. 결과값 엑셀에 입력 (한 번에) **
                //ApplyDataToExcel(dataToWrite, new List<(Excel.Range, object)>(), dataToFormat);

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

        private string SetManufacturing(Workbook workBook, List<string> calcList, int after, int addRowIndex, List<string> partName)
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
	                                a.CalculationId,
	                                case 
		                                when dbo.GetSingleTranslation(a.Name_LOC,'ko-KR','') is not null then dbo.GetSingleTranslation(a.Name_LOC,'ko-KR','')
		                                else dbo.GetSingleTranslation(a.Name_LOC,'en-US','') 
	                                END as name,
	                                L.*,	
	                                ManualCycleTime,
	                                ManualUtilizationRate,
                                    RequiredNumberOfMachinesInManufacturingStep,
	                                CASE 
		                                WHEN UseManualMachineHourlyRate = 0 THEN MachineSystemCostsPerPart * 3600 / ManualCycleTime / m.RequiredNumberOfMachinesInManufacturingStep
		                                WHEN UseManualMachineHourlyRate = 1 THEN MachineSystemCostsPerPart * 3600 / ManualCycleTime * ManualUtilizationRate / m.RequiredNumberOfMachinesInManufacturingStep
	                                END  AS Machine
                                FROM 
                                (SELECT 
                                    ld.Mid,
                                    MAX(CASE WHEN ld.UniqueKey = N'직접노무비' THEN ld.AvgPerson END) AS DirectPerson,
                                    MAX(CASE WHEN ld.UniqueKey = N'직접노무비' THEN ld.AvgCost END) AS DirectCost,
                                    MAX(CASE WHEN ld.UniqueKey = N'간접노무비' THEN ld.AvgPerson END) AS IndirectPerson,
                                    MAX(CASE WHEN ld.UniqueKey = N'간접노무비' THEN ld.AvgCost END) AS IndirectCost
                                FROM LaborData ld
                                GROUP BY ld.Mid) as L
                                JOIN [ManufacturingSteps] as a  on L.Mid = a.Id
                                JOIN Machines AS m ON a.Id = m.ManufacturingStepId
                                order by CalculationId, Mid;";

                System.Data.DataTable table = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

                Excel.Worksheet worksheet4 = workBook.Sheets["가공비"];
                Excel.Worksheet worksheet = workBook.Sheets["견적원가 레포트"];
                Excel.Worksheet worksheet2 = workBook.Sheets["년간손익"];

                worksheet4.Rows[5].Cells[5].Value = worksheet.Cells[24 + addRowIndex, 4].Value = table.AsEnumerable().Average(row => global.ConvertDouble(row.Field<double?>("DirectCost")));
                worksheet4.Rows[6].Cells[5].Value = worksheet.Cells[24 + addRowIndex, 5].Value = table.AsEnumerable().Average(row => global.ConvertDouble(row.Field<double?>("IndirectCost")));
                worksheet4.Rows[7].Cells[5].Value = worksheet.Cells[24 + addRowIndex, 6].Value = table.AsEnumerable().Average(row => global.ConvertDouble(row.Field<double?>("Machine")));
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

                worksheet4.Select();
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
                foreach (DataRow row in table.Rows)
                {
                    if (global.ConvertDouble(row["CalculationId"]) == calc)
                    {
                        ((Excel.Range)worksheet4.Rows[part + rowCount]).Insert(Missing.Value, worksheet4.Rows[part + rowCount]);
                        ((Excel.Range)worksheet4.Rows[part + 2 + rowCount * 2]).Insert(Missing.Value, worksheet4.Rows[part + 2 + rowCount * 2]);
                        ((Excel.Range)worksheet4.Rows[part + 4 + rowCount * 3]).Insert(Missing.Value, worksheet4.Rows[part + 4 + rowCount * 3]);
                    }
                    else
                    {
                        worksheet4.Rows[part].Cells[3].Value = partName[partCnt];

                        if (prevPart != 0)
                        {
                            SetManufacturingSum(worksheet4, worksheet, part, addRowIndex, partCnt, rowCount);

                            part = prevPart + (rowCount + 1) * 3 + 1;
                        }
                        partCnt++;
                        prevPart = part;
                        rowCount = 0;
                    }

                    calc = global.ConvertDouble(row["CalculationId"]);
                    worksheet4.Rows[part + rowCount].Cells[3].Select();
                    worksheet4.Rows[part + rowCount].Cells[5].Value = row["name"];
                    worksheet4.Rows[part + rowCount].Cells[6].Value = row["DirectPerson"];
                    worksheet4.Rows[part + rowCount].Cells[7].Value = row["ManualCycleTime"];
                    worksheet4.Rows[part + rowCount].Cells[8].Value = row["ManualUtilizationRate"];
                    worksheet4.Rows[part + rowCount].Cells[9].Formula = $"=IF(ISERROR(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())),\"\",(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())))";
                    worksheet4.Rows[part + rowCount].Cells[10].Formula = $"=IF(ISERROR(INDIRECT(\"F\"&ROW())/ INDIRECT(\"I\"&ROW())),\"\",(INDIRECT(\"F\"&ROW())/INDIRECT(\"I\"&ROW())))";
                    worksheet4.Rows[part + rowCount].Cells[11].Value = row["DirectCost"];
                    worksheet4.Rows[part + rowCount].Cells[12].Formula = $"=IF(ISERROR(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())),\"\",(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())))";

                    worksheet4.Rows[part + 2 + rowCount * 2].Cells[5].Value = row["name"];
                    worksheet4.Rows[part + 2 + rowCount * 2].Cells[6].Value = row["IndirectPerson"];
                    worksheet4.Rows[part + 2 + rowCount * 2].Cells[7].Value = row["ManualCycleTime"];
                    worksheet4.Rows[part + 2 + rowCount * 2].Cells[8].Value = row["ManualUtilizationRate"];
                    worksheet4.Rows[part + 2 + rowCount * 2].Cells[9].Formula = $"=IF(ISERROR(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())),\"\",(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())))";
                    worksheet4.Rows[part + 2 + rowCount * 2].Cells[10].Formula = $"=IF(ISERROR(INDIRECT(\"F\"&ROW())/ INDIRECT(\"I\"&ROW())),\"\",(INDIRECT(\"F\"&ROW())/INDIRECT(\"I\"&ROW())))";
                    worksheet4.Rows[part + 2 + rowCount * 2].Cells[11].Value = row["IndirectCost"];
                    worksheet4.Rows[part + 2 + rowCount * 2].Cells[12].Formula = $"=IF(ISERROR(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())),\"\",(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())))";

                    worksheet4.Rows[part + 4 + rowCount * 3].Cells[5].Value = row["name"];
                    worksheet4.Rows[part + 4 + rowCount * 3].Cells[6].Value = row["RequiredNumberOfMachinesInManufacturingStep"];
                    worksheet4.Rows[part + 4 + rowCount * 3].Cells[7].Value = row["ManualCycleTime"];
                    worksheet4.Rows[part + 4 + rowCount * 3].Cells[8].Value = row["ManualUtilizationRate"];
                    worksheet4.Rows[part + 4 + rowCount * 3].Cells[9].Formula = $"=IF(ISERROR(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())),\"\",(3600/INDIRECT(\"G\"&ROW())*INDIRECT(\"H\"&ROW())))";
                    worksheet4.Rows[part + 4 + rowCount * 3].Cells[10].Formula = $"=IF(ISERROR(INDIRECT(\"F\"&ROW())/ INDIRECT(\"I\"&ROW())),\"\",(INDIRECT(\"F\"&ROW())/INDIRECT(\"I\"&ROW())))";
                    worksheet4.Rows[part + 4 + rowCount * 3].Cells[11].Value = row["Machine"];
                    worksheet4.Rows[part + 4 + rowCount * 3].Cells[12].Formula = $"=IF(ISERROR(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())),\"\",(INDIRECT(\"J\"&ROW())*INDIRECT(\"K\"&ROW())))";

                    rowCount++;
                }

                SetManufacturingSum(worksheet4, worksheet, part, addRowIndex, partCnt, rowCount);

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
            worksheet.Cells[targetRow++, targetCol].Formula = $"='{worksheet4.Name}'!{worksheet4.Cells[part + rowCount, sourceCol].Address[false, false]}";
            worksheet.Cells[targetRow, targetCol].NumberFormat = "#,##0";
            worksheet.Cells[targetRow++, targetCol].Formula = $"='{worksheet4.Name}'!{worksheet4.Cells[part + 1 + rowCount * 2, sourceCol].Address[false, false]}";
            worksheet.Cells[targetRow, targetCol].NumberFormat = "#,##0";
            worksheet.Cells[targetRow++, targetCol].Formula = $"='{worksheet4.Name}'!{worksheet4.Cells[part + 2 + rowCount * 3, sourceCol].Address[false, false]}";
        }

        private string SetMaterial(JObject data, Workbook workBook, int cnt)
        {
            try
            {
                Excel.Worksheet worksheet = workBook.Sheets["재료비 상세내역"];
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
                string[] baseHeaders = { "Level", "품번", "품명", "Q'TY", "단위", "소유량[kg]", "재료비갭", "탄소배출량갭" };
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
                        bool needInsert = false;
                        string name = element["품번"]?.ToString() ?? "";
                        string number = element["품명"]?.ToString() ?? "";
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
                        row["Q'TY"] = global.ConvertDouble(element["Q'TY"]);
                        row["소유량[kg]"] = global.ConvertDouble(element["소유량[kg]"]);

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

                // ** 🚀 한 번에 Excel에 쓰기 (DataTable → Excel) **
                int startRow = 5;
                int startCol = 1; // **🔥 5번째 열부터 데이터 시작**

                int rowCount = dt.Rows.Count;
                int colCount = dt.Columns.Count;

                Excel.Range startCell = worksheet.Cells[startRow, startCol];
                Excel.Range endCell = worksheet.Cells[startRow + rowCount - 1, startCol + colCount - 1];
                Excel.Range writeRange = worksheet.Range[startCell, endCell];

                // ** 🔥 Excel 범위에 DataTable 데이터 한 번에 입력 **
                object[,] dataArray = new object[rowCount, colCount];
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        dataArray[i, j] = dt.Rows[i][j];
                    }
                }

                writeRange.Value2 = dataArray;

                Excel.Range sourceRow = worksheet.Range[worksheet.Cells[startRow, startCol], worksheet.Cells[startRow, startCol + colCount - 1]];

                // ** 📌 테두리 적용 **
                writeRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                // **🚀 서식만 복사하기**
                sourceRow.Copy();
                writeRange.PasteSpecial(Excel.XlPasteType.xlPasteFormats);

                // 클립보드 해제 (엑셀 실행 속도 최적화)
                worksheet.Application.CutCopyMode = 0;

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
        private string WriteOptimizedToExcel(Excel.Workbook workBook, System.Data.DataTable requirements, double after, ref List<string> partName, ref int addRowIndex, ref int rowIndex, ref int colIndex)
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

                        sourceRange2.Copy();
                        targetRange2.Insert(Excel.XlInsertShiftDirection.xlShiftDown);

                        worksheet2.Application.CutCopyMode = 0;
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
                        dataToFormula.Add((worksheet2.Cells[targetRow + i, 5], $"='{worksheet.Name}'!{worksheet.Cells[sourceRow++, sourceCol].Address[false, false]}"));
                    }

                    partName.Add(row["Name_LOC_Extracted"].ToString());

                    worksheet.Range[worksheet.Cells[rowIndex, colIndex], worksheet.Cells[rowIndex, colIndex + 1]].Merge();
                    colIndex += 2;

                    for (int i = 0; i < after; i++)
                    {
                        dataToWrite.Add((worksheet.Cells[rowIndex, colIndex], row[$"ManualValue{i}"]));
                        dataToFormat.Add((worksheet.Cells[rowIndex, colIndex++], "#,##0"));
                        dataToFormula.Add((worksheet2.Cells[16 + (partName.Count - 1) * 22, 5 + i], $"='{worksheet.Name}'!{worksheet.Cells[11 + (partName.Count - 1), 5 + i].Address[false, false]}"));

                        int year = 52 + addRowIndex;
                        int sourceCol2 = 5 + i;

                        dataToFormula.Add((worksheet.Cells[year++, sourceCol2], $"='{worksheet2.Name}'!{worksheet2.Cells[39+ (partName.Count - 1)*22, sourceCol2].Address[false, false]}"));
                        dataToFormula.Add((worksheet.Cells[year++, sourceCol2], $"='{worksheet2.Name}'!{worksheet2.Cells[53 + (partName.Count - 1) * 22, sourceCol2].Address[false, false]}"));
                        dataToFormula.Add((worksheet.Cells[year++, sourceCol2], $"=E{year - 2}/E{year - 3}"));
                        dataToFormula.Add((worksheet.Cells[year++, sourceCol2], $"='{worksheet2.Name}'!{worksheet2.Cells[57 + (partName.Count - 1) * 22, sourceCol2].Address[false, false]}"));
                        dataToFormula.Add((worksheet.Cells[year, sourceCol2], $"=E{year - 1}/E{year - 4}"));

                        int cnt = 8;
                        if(i!=0) dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"='{worksheet2.Name}'!{worksheet2.Cells[53 + (partName.Count - 1) * 22, sourceCol2-1].Address[false, false]}"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"='{worksheet5.Name}'!{worksheet5.Cells[53 + (partName.Count - 1) * 22, sourceCol2].Address[false, false]}"));

                        cnt = 11;
                        dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"=SUM({global.NumberToLetter(6 + i)}8:{global.NumberToLetter(6 + i)}10)"));
                        cnt++;
                        if (i != 0) dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"='{worksheet2.Name}'!{worksheet2.Cells[57 + (partName.Count - 1) * 22, sourceCol2-1].Address[false, false]}"));
                        if (i != 0) dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"='{worksheet2.Name}'!{worksheet2.Cells[39 + (partName.Count - 1) * 22, sourceCol2-1].Address[false, false]}*$H4"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"=SUM({global.NumberToLetter(6 + i)}12:{global.NumberToLetter(6 + i)}14)"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"={global.NumberToLetter(6 + i)}11-{global.NumberToLetter(6 + i)}15"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"={global.NumberToLetter(6 + i - 1)}17-{global.NumberToLetter(6 + i)}16"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"={global.NumberToLetter(6 + i)}16/(1+$D4)^{global.NumberToLetter(6 + i)}7"));
                        dataToFormula.Add((worksheet3.Cells[cnt++, 6 + i], $"={global.NumberToLetter(6 + i - 1)}19+{global.NumberToLetter(6 + i)}18"));

                        cnt = 69;
                        int cnt2 = 11;
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2, 6 + i].Address[false, false]}"));
                        cnt2 = 15;
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false, false]}"));
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false, false]}"));
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false, false]}"));
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false, false]}"));
                        dataToFormula.Add((worksheet.Cells[addRowIndex + cnt++, 5 + i], $"='{worksheet3.Name}'!{worksheet3.Cells[cnt2++, 6 + i].Address[false, false]}"));

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

                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow++}*{global.NumberToLetter(profitTargetCol)}{profitSourceRow++}"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol - 1)}{profitTargetRow++}"));
                            dataToFormula.Add((worksheet2.Cells[profitTargetRow, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow++}*{global.NumberToLetter(profitTargetCol)}{profitSourceRow++}"));

                            dataToFormula.Add((worksheet3.Cells[19, 6 + i], $"=IF(AND({global.NumberToLetter(6 + i)}17<0,{global.NumberToLetter(6 + i + 1)}17>=0),{global.NumberToLetter(6 + i)}7+ABS({global.NumberToLetter(6 + i)}17/{global.NumberToLetter(6 + i + 1)}16),\"\")"));

                        }


                        profitTargetRow = 31 + (partName.Count - 1) * 22;

                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 15}-{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 14}"));
                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 16}/{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}"));

                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol],
                            $"='{worksheet5.Name}'!{worksheet5.Cells[15, profitTargetCol + 1].Address[false, false]} * {global.NumberToLetter(profitTargetCol)}{profitSourceRow++}/'{worksheet.Name}'!{worksheet.Cells[12 + addRowIndex, profitTargetCol + 1].Address[false, false]}"));

                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 4}-{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}"));
                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}*{global.NumberToLetter(profitTargetCol)}{profitSourceRow++}"));
                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 3}-{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}"));
                        dataToFormula.Add((worksheet2.Cells[profitTargetRow++, profitTargetCol], $"={global.NumberToLetter(profitTargetCol)}{profitTargetRow - 2}/{global.NumberToLetter(profitTargetCol)}{profitTargetRow - 21}"));

                        dataToFormat.Add((worksheet2.Range[worksheet2.Cells[17 + (partName.Count - 1) * 22, profitTargetCol], worksheet2.Cells[profitTargetRow, profitTargetCol]], "#,##0"));

                        dataToFormat.Add((worksheet2.Cells[32 + (partName.Count - 1) * 22, profitTargetCol], "0.00%"));
                        dataToFormat.Add((worksheet2.Cells[37 + (partName.Count - 1) * 22, profitTargetCol], "0.00%"));

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

                    dataToFormula.Add((worksheet3.Cells[20, 6], $"=IRR({global.NumberToLetter(6)}16:{global.NumberToLetter(6 + (int)after)}16)"));
                    dataToFormula.Add((worksheet3.Cells[21, 6], $"=SUM({global.NumberToLetter(6)}16:{global.NumberToLetter(6 + (int)after)}16)"));
                    dataToFormula.Add((worksheet3.Cells[22, 6], $"=IF(ISERROR(SUM({global.NumberToLetter(6)}22:{global.NumberToLetter(6 + (int)after)}22))>0,(SUM({global.NumberToLetter(6)}22:{global.NumberToLetter(6 + (int)after)}22)),\"회수불가\")"));

                    dataToFormula.Add((worksheet.Cells[74, 5], $"='{worksheet3.Name}'!{worksheet3.Cells[20, 6].Address[false, false]}"));
                    dataToFormula.Add((worksheet.Cells[75, 5], $"='{worksheet3.Name}'!{worksheet3.Cells[21, 6].Address[false, false]}"));
                    dataToFormula.Add((worksheet.Cells[76, 5], $"='{worksheet3.Name}'!{worksheet3.Cells[22, 6].Address[false, false]}"));

                    dataToFormula.Add((worksheet.Cells[rowIndex, colIndex], $"=SUM(E{rowIndex}:{global.NumberToLetter(colIndex - 1)}{rowIndex})"));

                    rowIndex++;
                }

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
    }
}
