using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace TcPCM_Connect_Global
{
    class PressExport
    {
        public static void Additional_Material(int row, Excel.Worksheet worksheet, Part part, Part.Material material)
        {
            string query = $@"
            select
                SubstanceUniqueKey,ManualDensity/1000 as ManualDensity
                ,BorderLengthOffset*1000 as BorderLengthOffset, BorderWidthOffset*1000 as BorderWidthOffset
                ,MinimalPlateLength*1000 as MinimalPlateLength, MinimalPlateWidth*1000 as MinimalPlateWidth
            from[dbo].[MaterialMasses]
            where Id = (select[MaterialMassId] from BomEntries where PartID = {material.materialId}  and MaterialMassId is not null)";

            DataTable materialProperty = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

            query = $@"
                    select a.SubstanceUniqueKey, COALESCE(CAST(b.DecimalValue AS NVARCHAR(MAX)), b.ListItemValues) AS Value, c.UniqueKey
                      from (select * FROM [dbo].[PartRevisions] Where PartID = {material.materialId}) as a
                      left join PartRevisionPropertyValues as b
                      on a.Id = b.PartRevisionID
                      left join ClassificationProperties as c
                      on b.ClassificationPropertyId = c.Id";

            DataTable materialProperty2 = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

            query = $@"
                    select a.SubstanceUniqueKey, COALESCE(CAST(b.DecimalValue AS NVARCHAR(MAX)), b.ListItemValues) AS Value, c.UniqueKey
                      from (select * FROM [dbo].[PartRevisions] Where PartID = {part.header.partID}) as a
                      left join PartRevisionPropertyValues as b
                      on a.Id = b.PartRevisionID
                      left join ClassificationProperties as c
                      on b.ClassificationPropertyId = c.Id";

            DataTable headerProperty = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

            if (materialProperty.Rows.Count==0|| materialProperty2.Rows.Count== 0) return;

            double netSize = global.ConvertDouble(headerProperty.Select("UniqueKey='Siemens.TCPCM.Property.Geometry.Forming.UnfoldingX'").First()["Value"]);
            worksheet.Range["AR6"].Value = $"{netSize*1000}";
            netSize = global.ConvertDouble(headerProperty.Select("UniqueKey='Siemens.TCPCM.Property.Geometry.Forming.UnfoldingX'").First()["Value"]);
            worksheet.Range["AS6"].Value = $"{netSize * 1000}";
            netSize = global.ConvertDouble(headerProperty.Select("UniqueKey='Siemens.TCPCM.Property.Geometry.Common.MinHeight'").First()["Value"]);
            worksheet.Range["AT6"].Value = $"{netSize * 1000}";

            worksheet.Range["AR12"].Value = $"{materialProperty.Rows[0]["MinimalPlateLength"]}";
            worksheet.Range["AS12"].Value = $"{materialProperty.Rows[0]["MinimalPlateWidth"]}";
            double thickness = global.ConvertDouble(materialProperty2.Select("UniqueKey='Siemens.TCPCM.Property.Geometry.FormingSeparatingJoining.SheetThickness'").First()["Value"]);
            worksheet.Range["AT12"].Value = $"{ thickness*1000}";

            worksheet.Range["AR14"].Value = $"{materialProperty.Rows[0]["ManualDensity"]}";

            worksheet.Range["AR17"].Value = $"{material.netWeight}";
            worksheet.Range["AT17"].Value = $"{material.grossWeight}";

            double sum = part.summary.labor + part.summary.machine + part.summary.material + part.summary.administrationCostsTotal + part.summary.profitTotal + part.summary.materialOverheadTotal;
            worksheet.Range["AS20"].Value = (part.summary.defect / (sum- part.summary.defect)) * 100;
            worksheet.Range["AS21"].Value = material.etc/(material.total- material.etc+material.scrap) * 100;

        }        
        public static void Additional_Manufacturing(int row, Excel.Worksheet worksheet, Part.Manufacturing manufacturing)
        {
           string query = $@"select * FROM [dbo].[StampingCycleTimeCalculators] Where [ManufacturingStepId] = {manufacturing.id}";

            DataTable property = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
            if (property.Rows.Count == 0) return;

            worksheet.Range["AS25"].Formula = $"{manufacturing.netCycleTime}";

            worksheet.Range["AR29"].Formula = $"=T{row}";
            worksheet.Range["AS29"].Formula = $"=R{row}";
            worksheet.Range["AT29"].Formula = $"=AR29*60/AS29";

            worksheet.Range["AS33"].Formula = $"=X{row}";
            worksheet.Range["AS34"].Formula = $"=AC{row}";
            worksheet.Range["AS35"].Formula = $"=AD{row}";
            worksheet.Range["AS36"].Formula = $"=AA{row}";
            worksheet.Range["AS37"].Formula = $"=Z{row}";
            worksheet.Range["AS38"].Formula = $"=AG{row}";
            worksheet.Range["AS39"].Formula = $"=AH{row}";
            worksheet.Range["AS40"].Formula = $"=AI{row}";

            worksheet.Range["AT33"].Formula = $"=AB{row}";
            worksheet.Range["AT34"].Formula = $"=AC{row}";
            worksheet.Range["AT35"].Formula = $"=AD{row}";
            worksheet.Range["AT36"].Formula = $"=AA{row}";
            worksheet.Range["AT37"].Formula = $"=Z{row}";
            worksheet.Range["AT38"].Formula = $"=AG{row}";
            worksheet.Range["AT39"].Formula = $"=AH{row}";
            worksheet.Range["AT40"].Formula = $"=AI{row}";
        }
    }
}
