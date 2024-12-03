using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace TcPCM_Connect_Global
{
    class CoreExport
    {
        public static void Additional_Material(int row, Excel.Worksheet worksheet, Part part, Part.Material material)
        {
            if (material.materialId == 0)
            {
                if (material.name.ToLower().Contains("em"))
                {
                    worksheet.Range["AS27"].Formula = $"=O{row}";
                }
                else if(material.name.ToLower().Contains("방청유"))
                {
                    worksheet.Range["AS28"].Formula = $"=O{row}";
                }
                return;
            }

            if (material.netWeight == 0)
            {
                return;
            }
            string query = $@"
            select
                SubstanceUniqueKey,
                ManualPlateLength * 1000 as ManualPlateLength,ManualWidth, ManualThickness * 1000 as ManualThickness, ManualDensity / 1000 as ManualDensity, ManualPlateMass, 
                SprueLossesRate, ManualNetVolume, ShotMassLossesRate,
                ManualDeployedMassBeforeLosses,	ManualLossesRate,
                ManualScrapRate
            from[dbo].[MaterialMasses]
            where Id = (select[MaterialMassId] from BomEntries where PartID = {material.materialId}  and MaterialMassId is not null)";

            DataTable materialProperty = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

            //double drossloss = global.ConvertDouble(materialProperty.Rows[0]["ShotMassLossesRate"]);
            worksheet.Range["AS4"].Value = materialProperty.Rows[0]["SubstanceUniqueKey"]?.ToString().Replace("[LGMagna]", "").Split('_')[0].Replace("Ear-31", "").Replace("원형2", "").Replace("Ear-41", "");
            double method = global.ConvertDouble( materialProperty.Rows[0]["ShotMassLossesRate"]);
            string methodStr = "용접";
            if (method == 0) methodStr = "용접";
            else if (method == 1) methodStr = "Laser 용접";
            else if (method == 2) methodStr = "SB재";
            else if (method == 3) methodStr = "Dot Bonding";
            else if (method == 4) methodStr = "Interlock only";
            worksheet.Range["AS5"].Value = methodStr;

            worksheet.Range["AS6"].Value = materialProperty.Rows[0]["ManualWidth"];
            worksheet.Range["AS7"].Value = materialProperty.Rows[0]["ManualPlateLength"];
            worksheet.Range["AS8"].Value = materialProperty.Rows[0]["ManualThickness"];

            worksheet.Range["AS9"].Value = materialProperty.Rows[0]["ManualDeployedMassBeforeLosses"];
            worksheet.Range["AS10"].Value = materialProperty.Rows[0]["ManualLossesRate"];
            worksheet.Range["AS11"].Formula = $"=AS9+AS10";
            worksheet.Range["AS12"].Formula = $"=AS9/AS11";
            worksheet.Range["AS13"].Formula = $"=AS10/AS11";

            worksheet.Range["AS14"].Value = material.unitCost;
            worksheet.Range["AS15"].Value = material.unitCost;
            worksheet.Range["AS16"].Value = material.quantity;

            query = $@"
                      select a.SubstanceUniqueKey, COALESCE(CAST(b.DecimalValue AS NVARCHAR(MAX)), b.ListItemValues) AS Value, c.UniqueKey
                      from (select * FROM [dbo].[PartRevisions] Where PartID = {material.materialId}) as a
                      left join PartRevisionPropertyValues as b
                      on a.Id = b.PartRevisionID
                      left join ClassificationProperties as c
                      on b.ClassificationPropertyId = c.Id ";

            DataTable materialMaster = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

            worksheet.Range["AS17"].Value = materialMaster.Select($"UniqueKey = 'Stator 외경형상1'").First()["Value"]?.ToString().Replace("1", "");
            worksheet.Range["AS18"].Value = materialProperty.Rows[0]["ManualScrapRate"];
            worksheet.Range["AS22"].Value = materialProperty.Rows[0]["SprueLossesRate"];
            worksheet.Range["AS24"].Value = materialProperty.Rows[0]["ManualPlateMass"];
            worksheet.Range["AS25"].Value = materialProperty.Rows[0]["ManualNetVolume"];

        }
        public static void Additional_Manufacturing(int row, Excel.Worksheet worksheet, Part.Manufacturing manufacturing)
        {
            if (!manufacturing.manufacturingName.Contains("타발")) return;

            string query = $@"
            select ManualMaxAcceleration
            from [LaserCycleTimeCalculators]
            where [ManufacturingStepId] = {manufacturing.id}";

            DataTable property = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

            worksheet.Range["AS23"].Value = property.Rows[0]["ManualMaxAcceleration"];

            worksheet.Range["AS32"].Formula = $"=X{row}";
            worksheet.Range["AS33"].Formula = $"=AC{row}";
            worksheet.Range["AS34"].Formula = $"=AD{row}";
            worksheet.Range["AS35"].Formula = $"=AA{row}";
            worksheet.Range["AS36"].Formula = $"=Z{row}";
            worksheet.Range["AS37"].Formula = $"=AG{row}";
            worksheet.Range["AS38"].Formula = $"=AH{row}";
            worksheet.Range["AS39"].Formula = $"=AI{row}";

            worksheet.Range["AT32"].Formula = $"=AB{row}";
            worksheet.Range["AT33"].Formula = $"=AC{row}";
            worksheet.Range["AT34"].Formula = $"=AD{row}";
            worksheet.Range["AT35"].Formula = $"=AA{row}";
            worksheet.Range["AT36"].Formula = $"=Z{row}";
            worksheet.Range["AT37"].Formula = $"=AG{row}";
            worksheet.Range["AT38"].Formula = $"=AH{row}";
            worksheet.Range["AT39"].Formula = $"=AI{row}";
        }    
    }
}
