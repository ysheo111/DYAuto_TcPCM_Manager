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
    public class ExcelSAP
    {
        public string Export(string calc, string fileLocation, System.Windows.Forms.Form form)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;

            try
            {
                File.Copy($@"{Application.StartupPath}\DY AUTO SAP 비교 원가 양식.xlsx", $@"{fileLocation}", true);
                //Excel 프로그램 실행
                application = new Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일로부터 불러오기                
                workBook = application.Workbooks.Open($@"{fileLocation}");
                Excel.Worksheet worksheet = workBook.Sheets["Export"];

                string query2 = $@"WITH PivotData AS(
                                --EconomicOverheadRates 데이터를 DateValidFrom 별로 개별 행 유지하면서 추출
                                SELECT
                                    i.DateValidFrom, --개별 행 유지
                                    i.EconomicCalculationId AS CalculationId,
                                    e.InternalName,
                                    i.ManualRate
                                FROM[TcPCM2312_Patch3].[dbo].[EconomicOverheadRates] AS i
                                LEFT JOIN CostElementDefinition AS e ON e.Id = i.CostElementDefinitionId
                                WHERE e.InternalName IN('OtherOverheadCosts03', 'OtherOverheadCosts04')
                            )

                            , PivotResult AS(
                                --PIVOT 실행(DateValidFrom 유지)
                                SELECT
                                    CalculationId,
                                    DateValidFrom,  --DateValidFrom을 포함하여 개별 행 유지
                                    ISNULL([OtherOverheadCosts03], 0) AS 판관비율,
                                    ISNULL([OtherOverheadCosts04], 0) AS 로열티
                                FROM PivotData
                                PIVOT(
                                    MAX(ManualRate) FOR InternalName IN([OtherOverheadCosts03], [OtherOverheadCosts04])
                                ) AS PivotTable
                            )

                            SELECT DISTINCT
                                c.id AS CalculationId,
                                ppe.ManualSalesPrice AS SalesPrice,
                                pd.DateValidFrom,  --DateValidFrom 개별 행 유지
                                ISNULL(pd.판관비율, 0) AS 판관비율,
                                ISNULL(pd.로열티, 0) AS 로열티,
                                p.StartOfProductionDate,
                                p.PeriodsAfterSOP,		
                                c3.ProjectAssumptionPeriodsCount* c3.ProjectAssumptionYearlyAverageRequirement AS requirement,
	                            pp.PartNo,
	                            pr.Number,
                                tc.*
                            FROM EconomicCalculations c
                            JOIN Projects p ON c.ProjectId = p.Id
                            JOIN ProjectPartEntries ppe ON p.Id = ppe.ProjectId
                            join Parts pp on pp.Id = ppe.PartId
                            JOIN Calculations c3 ON ppe.PartId = c3.PartId
                            JOIN PivotResult pd ON c.Id = pd.CalculationId
                            Join PartRevisions pr on pr.PartId = pp.Id
                            join [PCI].[dbo].[Transport] as tc on tc.CalculationId = c3.id
                            AND YEAR(pd.DateValidFrom) = YEAR(c3.CalculationTime)-- 연도 매칭
                            WHERE c3.Id = {calc}
                            AND c.Deleted IS NULL;
                ";
                DataTable basic = global_DB.MutiSelect(query2, (int)global_DB.connDB.PCMDB);
                double salesPrice = 0;
                string partNo ="",revision="",requirement="", saleoverhead="",roytal="",package="",transport="";
                if (basic.Rows.Count > 0)
                { 
                    salesPrice = global.ConvertDouble(basic.Rows[0]["SalesPrice"]);
                    partNo = basic.Rows[0]["PartNo"].ToString();
                    revision = basic.Rows[0]["Number"].ToString();
                    requirement = basic.Rows[0]["requirement"].ToString();
                    saleoverhead = basic.Rows[0]["판관비율"].ToString();
                    roytal = basic.Rows[0]["로열티"].ToString();
                    package = basic.Rows[0]["Package"].ToString();
                    transport = basic.Rows[0]["Transport"].ToString();
                }

                Interface export = new Interface();
                JObject apiResult = export.LoadCalc(new List<string>() { calc }, "SAP");
                if (apiResult == null) return "데이터 조회 시 오류가 발생하였습니다.";
                object material = apiResult["data"][0]["Direct material costs"];

                    string query = $@"
                    select  case 
		                        when dbo.GetSingleTranslation(m.Name_LOC,'ko-KR','') is not null then dbo.GetSingleTranslation(m.Name_LOC,'ko-KR','')
		                        else dbo.GetSingleTranslation(m.Name_LOC,'en-US','') 
	                        END as name,
		                    m.LaborCostsPerPart, m.MachineSystemCostsPerPart, m.ManufacturerToolCostsPerPart, m.RMOCCostsPerPart, t.MasterToolId
                    from Calculations as c
                    join ManufacturingSteps as m on c.Id = m.CalculationId
                    left join Tools as t on m.Id = t.ManufacturingStepId
                    where c.id  = {calc}";

                DataTable manufacturing = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
                double labor = 0, machine = 0, tool = 0,mold=0, inmanufacturing=0;
                foreach(DataRow row in manufacturing.Rows)
                {
                    labor += global.ConvertDouble(row["LaborCostsPerPart"]);
                    machine += global.ConvertDouble(row["MachineSystemCostsPerPart"]);
                    if (global.ConvertDouble(row["MasterToolId"]) == 5) tool += global.ConvertDouble(row["ManufacturerToolCostsPerPart"]);
                    else mold += global.ConvertDouble(row["ManufacturerToolCostsPerPart"]);
                    inmanufacturing += global.ConvertDouble(row["RMOCCostsPerPart"]); 
                }

                List<string> pcmItem = new List<string>();
                pcmItem.Add($"={salesPrice}");
                pcmItem.Add($"=SUM(E14:E18)");
                pcmItem.Add($"={material}");
                pcmItem.Add($"={labor}");
                pcmItem.Add($"={inmanufacturing}");
                pcmItem.Add($"={machine}");
                pcmItem.Add($"={tool}");
                pcmItem.Add($"={tool}");
                pcmItem.Add($"={mold}");
                pcmItem.Add($"=SUM(E7:E13)");
                pcmItem.Add($"={global.ConvertDouble(saleoverhead) * global.ConvertDouble( salesPrice)}");
                pcmItem.Add($"={transport}");
                pcmItem.Add($"={package}");
                pcmItem.Add($"={global.ConvertDouble( roytal)*global.ConvertDouble(material)}");
                pcmItem.Add($"=E5-E6");
                pcmItem.Add($"=E19/E5");

                Excel.Range startCell = (Excel.Range)worksheet.Cells[5, 5];
                Excel.Range endCell = (Excel.Range)worksheet.Cells[5 + pcmItem.Count - 1, 5];
                Excel.Range writeRange = worksheet.Range[startCell, endCell];

                startCell.Worksheet.Select();
                startCell.Select();
                object[,] dataArray2 = new object[pcmItem.Count, 1];
                for (int c = 0; c < pcmItem.Count; c++)
                {
                    dataArray2[c, 0] = pcmItem[c];
                }
                writeRange.Value2 = dataArray2;

                List<string> sap = new List<string>();
                if (form is ISAPDataSearch sapForm)
                {
                    sapForm.partNo = partNo;
                    sapForm.revision = (revision.Length > 2 ? revision.Substring(0, 2) : revision);
                    sapForm.ShowDialog();

                    sap= sapForm.ReturnValue1;
                }
                if (sap.Count <= 0) return "SAP 해당하는 데이터가 없습니다.";
                startCell = (Excel.Range)worksheet.Cells[5, 7];
                endCell = (Excel.Range)worksheet.Cells[5+ sap.Count-2, 7];
                writeRange = worksheet.Range[startCell, endCell];

                startCell.Worksheet.Select();
                startCell.Select();
                object[,] dataArray = new object[sap.Count, 1];

                for (int c = 0; c < sap.Count; c++)
                {
                    dataArray[c, 0] = sap[c];
                }
                writeRange.Value2 = dataArray;               

                worksheet.Cells[5, 11].Value = sap.Last();
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return null;
        }
    }
}
