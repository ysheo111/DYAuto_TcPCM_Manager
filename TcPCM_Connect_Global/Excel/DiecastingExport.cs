using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace TcPCM_Connect_Global
{
    class DieCastingExport
    {

        public static DataTable Additional_ETC(Excel.Worksheet worksheet, Part part)
        {
            worksheet.Range["AS14"].Value = part.header.region;

            string query =
                $@"select YearlyRequirementOfAssemblies, PartsPerAssembly, ManualLotSizeOutput, CalculatedYearlyRequirementOutput, CallsPerYear,CalculatedLotSize
                    from [dbo].[Calculations] where [Id] = {part.header.guid}";

            DataTable calculation = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
            worksheet.Range["AW22"].Value = calculation.Rows[0]["YearlyRequirementOfAssemblies"];
            worksheet.Range["AW23"].Value = calculation.Rows[0]["PartsPerAssembly"];
            worksheet.Range["AW24"].Value = calculation.Rows[0]["ManualLotSizeOutput"];
            worksheet.Range["AW25"].Value = calculation.Rows[0]["CalculatedYearlyRequirementOutput"];
            worksheet.Range["AW26"].Value = calculation.Rows[0]["CalculatedYearlyRequirementOutput"];
            worksheet.Range["AW27"].Value = calculation.Rows[0]["CallsPerYear"];
            worksheet.Range["AW28"].Value = calculation.Rows[0]["CalculatedLotSize"];

            query = $@"  
                      select a.SubstanceUniqueKey, b.DecimalValue, c.UniqueKey
                      from (select * FROM [dbo].[PartRevisions] Where PartID = {part.header.partID}) as a
                      left join PartRevisionPropertyValues as b
                      on a.Id = b.PartRevisionID
                      left join ClassificationProperties as c
                      on b.ClassificationPropertyId = c.Id
            ";

            calculation = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
            worksheet.Range["AR5"].Value = calculation.Select($"UniqueKey = 'Siemens.TCPCM.Property.Geometry.Common.PartSurface'").First()["SubstanceUniqueKey"]?.ToString().Replace("[LGMagna]", "");
            worksheet.Range["AS5"].Value = global.ConvertDouble(calculation.Select($"UniqueKey = 'Siemens.TCPCM.Property.Geometry.Common.MinLength'").First()["DecimalValue"]) * 1000;
            worksheet.Range["AT5"].Value = global.ConvertDouble(calculation.Select($"UniqueKey = 'Siemens.TCPCM.Property.Geometry.Common.MinWidth'").First()["DecimalValue"]) * 1000;
            worksheet.Range["AU5"].Value = global.ConvertDouble(calculation.Select($"UniqueKey = 'Siemens.TCPCM.Property.Geometry.Common.MinHeight'").First()["DecimalValue"]) * 1000;

            return calculation;
        }
        public static void Additional_Material(int row, Excel.Worksheet worksheet, Part part, Part.Material material, DataTable calculation)
        {
            if (material.name.Contains("용해")) worksheet.Range["AS15"].Value = material.total;
            else if (material.name.Contains("주조")) worksheet.Range["AS16"].Value = material.total;
            else if (material.name.ToLower().Contains("blast") || material.name.ToLower().Contains("s/b")) worksheet.Range["AS17"].Value = material.total;
            else if (material.name.Contains("함침")) worksheet.Range["AS18"].Value = material.total;
            else if (material.dross != null)
            {
               double net= global.ConvertDouble(calculation.Select($"UniqueKey = 'Siemens.TCPCM.Property.Geometry.Common.NetWeight'").First()["DecimalValue"]);

                string query = $@"
                        select [ShotMassLossesRate], [SprueLossesRate],[ManualSprueRate] from [dbo].[MaterialMasses] 
                        where Id = (select [MaterialMassId] from BomEntries where PartID = {material.materialId}  and MaterialMassId is not null)
                    ";
                DataTable materialProperty = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
                double loss = global.ConvertDouble( materialProperty.Rows[0]["ShotMassLossesRate"]);
                double err = global.ConvertDouble( materialProperty.Rows[0]["SprueLossesRate"]);
                double sprue = global.ConvertDouble( materialProperty.Rows[0]["ManualSprueRate"]);

                query = $@"
                        select [ShotMassLossesRate], [SprueLossesRate],[ManualSprueRate] from [dbo].[MaterialMasses] 
                        where Id = (select [MaterialMassId] from BomEntries where PartID = {material.drossId} and MaterialMassId is not null)
                    ";
                materialProperty = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
                double drossloss = global.ConvertDouble(materialProperty.Rows[0]["ShotMassLossesRate"]);

                double materialCost = material.netWeight * 1.05;
                double injection = material.netWeight * 1.05/ sprue;
                double gross = materialCost+ injection* loss*(1+ err);
                double scrap = gross - material.netWeight;

                worksheet.Range["AR9"].Value = gross;
                worksheet.Range["AS9"].Value = scrap;
                worksheet.Range["AT9"].Value = materialCost;
                worksheet.Range["AU9"].Value = injection;

                worksheet.Range["AR11"].Value = material.unitCost;
                worksheet.Range["AS11"].Value = -material.scrapUnitPrice;
                worksheet.Range["AT11"].Value = material.drossUnitPrice;

                worksheet.Range["AS22"].Value = sprue*100;
                worksheet.Range["AS23"].Value = 99;
                worksheet.Range["AS24"].Value = loss*100;
                worksheet.Range["AS25"].Value = drossloss*100;
                worksheet.Range["AS26"].Value = err*100;

                double sum = part.summary.labor + part.summary.machine + part.summary.material + part.summary.administrationCostsTotal + part.summary.profitTotal + part.summary.materialOverheadTotal;
                worksheet.Range["AS28"].Value = (part.summary.defect / (sum - part.summary.defect)) * 100;
            }
        }        
        public static void Additional_Manufacturing(int row, Excel.Worksheet worksheet, Part.Manufacturing manufacturing, DataTable cycleTime)
        {
            DataRow matchingRow = cycleTime.Select($"ID = {manufacturing.id}").First();
            if (manufacturing.manufacturingName.Contains("용해"))
            {               
                worksheet.Range["AS34"].Formula = $"=U{row}";
                worksheet.Range["AT44"].Value = $"{matchingRow["용해"]}";
            }
            else if (manufacturing.manufacturingName.Contains("주조"))
            {
                DataRow matchingInsertRow = cycleTime.Select($"ID = {manufacturing.id} and Name = 'Insert물 수량 (ea)'").First();
                DataRow[] matchingScoreRow = cycleTime.Select($"ID = {manufacturing.id} and CycleTimeRelevance <> 0 and Name like '%S/Core%'");

                worksheet.Range["AU38"].Value = $"";
                worksheet.Range["AV38"].Formula = $"=IF({matchingRow["PartsPerCycleCache"]}=1,1.5,IF({matchingRow["PartsPerCycleCache"]}=2,1.35,1.3))";
                worksheet.Range["AW38"].Value = $"";
                worksheet.Range["AZ38"].Value = $"{manufacturing.machineName.Replace("[LGMagna]주조기 ", "")}";

                worksheet.Range["AS35"].Formula = $"=U{row}";
                worksheet.Range["AT45"].Value = $"{matchingRow["주조기본시간"]}";
                worksheet.Range["AT46"].Value = $"{matchingScoreRow.First()["Name"]}".Replace("S/Core 수량 (", "").Replace("ea)", "");
                double time = 0;
                foreach(DataRow score in matchingScoreRow)
                {
                    time += global.ConvertDouble(score["CycleTimeRelevance"]) * global.ConvertDouble(score[(score["Name"])?.ToString()]);
                }

                worksheet.Range["AT47"].Value = $"{time}";
                worksheet.Range["AT48"].Value = $"{matchingInsertRow["CycleTimeRelevance"]}";
                worksheet.Range["AT49"].Value = $"{global.ConvertDouble( matchingRow["Insert 시간"])* global.ConvertDouble(matchingInsertRow["CycleTimeRelevance"])}";
                worksheet.Range["AT50"].Value = $"{matchingRow["가동률 보정치"]}";

                worksheet.Range["AU34"].Value = $"{matchingRow["준비시간"]}";

                double mode = global.ConvertDouble( matchingRow["CastingPressureMode"]);
                double manual = global.ConvertDouble(matchingRow["ManualCastingPressure"]);
                double force = global.ConvertDouble(matchingRow["Force"]);
                double press = 0;
                if (mode == 1) press = manual / 100000;
                else if (mode == 2) press = 600;
                else if (mode == 3) press = 900;
                else if (mode == 4) press = 100;

                worksheet.Range["AU38"].Value = $"{press}";
                worksheet.Range["AW38"].Value = $"{press* force/100*global.ConvertDouble(matchingRow["PartsPerCycleCache"])}";

                worksheet.Range["AS61"].Formula = $"=X{row}";
                worksheet.Range["AS62"].Formula = $"=AC{row}";
                worksheet.Range["AS63"].Formula = $"=AD{row}";
                worksheet.Range["AS64"].Formula = $"=AA{row}";
                worksheet.Range["AS65"].Formula = $"=Z{row}";
                worksheet.Range["AS66"].Formula = $"=AG{row}";
                worksheet.Range["AS67"].Formula = $"=AH{row}";
                worksheet.Range["AS68"].Formula = $"=AI{row}";

                worksheet.Range["AT61"].Formula = $"=AB{row}";
                worksheet.Range["AT62"].Formula = $"=AC{row}";
                worksheet.Range["AT63"].Formula = $"=AD{row}";
                worksheet.Range["AT64"].Formula = $"=AA{row}";
                worksheet.Range["AT65"].Formula = $"=Z{row}";
                worksheet.Range["AT66"].Formula = $"=AG{row}";
                worksheet.Range["AT67"].Formula = $"=AH{row}";
                worksheet.Range["AT68"].Formula = $"=AI{row}";
            }
            else if (manufacturing.manufacturingName.Contains("사상"))
            {
                worksheet.Range["AS36"].Formula = $"=U{row}";
                worksheet.Range["AT51"].Value = $"{matchingRow["LoadLength"]}";
                worksheet.Range["AT52"].Value = $"{matchingRow["LoadHeight"]}";
            }
            else if (manufacturing.manufacturingName.ToLower().Contains("함침"))
            {               
                worksheet.Range["AY38"].Formula = $"=ROUNDUP({matchingRow["LoadMass"]}/(AS5*AT5*AU5/1000),0)";
            }
            else if (manufacturing.manufacturingName.ToLower().Contains("s/b"))
            {
                string type = "";
                string name = manufacturing.manufacturingName.ToLower();
                if (name.Contains("housing")|| name.Contains("하우징")) type = "Housing류";
                else if (name.Contains("bracket") || name.Contains("브라켓")) type = "소형bracket류";
                else if (name.Contains("cover") || name.Contains("커버")) type = "Cover류";
                worksheet.Range["AS37"].Formula = $"=U{row}";
                worksheet.Range["AS53"].Value = $"{type}";
                worksheet.Range["AT53"].Value = $"{matchingRow["LoadMass"]}";
                worksheet.Range["AT54"].Value = $"{matchingRow["LoadLength"]}";
                worksheet.Range["AT55"].Value = $"{matchingRow["PartsPerCycleCache"]}";
                worksheet.Range["AT56"].Formula = $"=MIN(AS5,AT5)/AT53";

            }
            else if (manufacturing.manufacturingName.Contains("세척"))
            {
                worksheet.Range["AS38"].Formula = $"=U{row}";
            }
            else if (manufacturing.manufacturingName.Contains("검사"))
            {
                worksheet.Range["AS39"].Formula = $"=U{row}";
            }
            else if (manufacturing.manufacturingName.Contains("포장"))
            {
                worksheet.Range["AS40"].Formula = $"=U{row}";
            }
        }
        public static DataTable Additional_CycleTime(Excel.Worksheet worksheet, Part part)
        {
            worksheet.Range["AV34"].Formula = $"=AW28";
            worksheet.Range["AW34"].Formula = $"=AU34/AW28";

            string query =
                $@"
                    SELECT * 
                    FROM 
                    (
                        SELECT 
                            ID,
                            PartsPerCycleCache,
                            COALESCE(
                                t.c.value('(value[@lang=""ko - KR""])[1]', 'nvarchar(max)'),  -- ko-KR 값을 먼저 시도
                                t.c.value('(value[@lang=""en-US""])[1]', 'nvarchar(max)')-- ko - KR 값이 없으면 en - US 값 시도
                            ) AS TranslatedValue,
                            ManualCycleTime
                        FROM
                            [ManufacturingSteps] AS s
                            CROSS APPLY s.Name_LOC.nodes('/translations') AS t(c)
                        WHERE
                            [CalculationId] = {part.header.guid}
                    ) AS c
                    LEFT JOIN
                    (
                        SELECT
                            a.Id,
                            a.ManufacturingStepId,
		                    b.CycleTimeRelevance,
		                    b.Name,
                            CastingPressureMode,
                            ProjectedAreaPerPartCache*(1+AreaSurchargeFraction)*100 as 'Force', 
                            ManualCastingPressure,
                            a.[PoorCoolingSurcharge] AS '가동률 보정치',
                            MAX(a.BlowOutMouldTime) AS '준비시간',
                            MAX(a.EjectorResetTime) AS '주조기본시간',
                            MAX(a.CloseGateValveTime) AS 'Insert 시간',	
                            MAX(a.CloseMouldTime)AS 'S/Core 수량 (0ea)',
		                    MAX(a.FillMaterialTime)AS 'S/Core 수량 (1ea)',
		                    MAX( a.ShootTime)AS 'S/Core 수량 ( 2~3ea)',
		                    MAX(a.OpenMouldTime) AS 'S/Core 수량 (~4ea)',
                            MAX(b.ManualSecondaryTime) AS '용해'
                        FROM 
                            [dbo].[CastingCycleTimeCalculators] AS a
                        LEFT JOIN 
                            [CycleTimeSteps] AS b
                        ON 
                            a.Id = b.CycleTimeCastingId
                        GROUP BY 
                            a.Id, 
                            a.[PoorCoolingSurcharge], 
                            a.ManufacturingStepId,
		                    b.CycleTimeRelevance,
                            CastingPressureMode,
                            ManualCastingPressure,
                            b.Name,
                            ProjectedAreaPerPartCache*(1+AreaSurchargeFraction)*100
                    ) AS d

                    ON c.Id = d.ManufacturingStepId
                    left join
                    (
                        SELECT ManufacturingStepId, [ManualMachine1Capacity]*3600 as 'a',[Machine1UsageFraction]*100 as 'b'
                      FROM[TcPCM2023P1].[dbo].[LineBalancingCycleTimeCalculators]
                    ) as e
                    ON c.Id = e.ManufacturingStepId
                    left join
                    (
                        SELECT ManufacturingStepId
                          ,[LoadLength]*1000 as [LoadLength]
		                  ,[LoadWidth]*1000 as [LoadWidth]
		                  ,[LoadHeight]*1000 as [LoadHeight] 
                          ,[LoadMass]
                          ,[ManualCycleTime]
                          FROM [TcPCM2023P1].[dbo].[ChamberLoadingCycleTimeCalculator]
                    ) as f
                    ON c.Id = f.ManufacturingStepId";

            return global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

        }
    }
}
