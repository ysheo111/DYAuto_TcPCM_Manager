using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace TcPCM_Connect_Global
{
    class InjectionExport
    {
        public static void Additional_Material(int row, Excel.Worksheet worksheet, Part part, Part.Material material)
        {
            string query = $@"select a.SubstanceUniqueKey as '소재', b.UniqueKey as '재료분류', c.SprueRecyclingRate*100 as 'Loss율', d.UniqueKey as '재생여부'
                            from (select PartID, SubstanceClassificationId, SubstanceUniqueKey from PartRevisions where PartID = {material.materialId}) as a
                            left join Classifications as b
                            on a.SubstanceClassificationId = b.id
                            left join 
                            (
	                            select {material.materialId} as PartId, SprueRecyclingRate from [dbo].[MaterialMasses] 
	                            where Id = (select [MaterialMassId] from BomEntries where PartID = {material.materialId}  and MaterialMassId is not null )
                            ) as c
                            on a.PartId =  c.PartId
                            left join 
                            (
	                            select {material.materialId} as PartId, UniqueKey from BDCustomers
	                            where Id = (select BDCustomerId from Calculations where Id = {part.header.guid})
                            ) as d
                            on a.PartId =  d.PartId";

            DataTable properties = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);

            if(properties.Rows.Count <= 0 || properties.Rows[0]["Loss율"]==DBNull.Value)
            {
                return;
            }

            worksheet.Range["AR5"].Value = properties.Rows[0]["소재"]?.ToString().Replace("[LGMagna]","");
            worksheet.Range["AS5"].Value = properties.Rows[0]["재료분류"];
            worksheet.Range["AT5"].Value = properties.Rows[0]["재생여부"];

            worksheet.Range["AR8"].Value = material.netWeight;
            worksheet.Range["AS8"].Value = material.loss;
            worksheet.Range["AT8"].Value = material.quantity;
            worksheet.Range["AS12"].Value = properties.Rows[0]["Loss율"];

            worksheet.Range["AS11"].Value = material.etc / (material.total - material.etc + material.scrap) * 100;            
        }        
        public static void Additional_Manufacturing(int row, Excel.Worksheet worksheet, Part.Manufacturing manufacturing)
        {
            string query = $@"SELECT [Name]
	                        ,[CycleTimeRelevance]
	                        ,[ManualSecondaryTime] *[CycleTimeRelevance] as [ManualSecondaryTime]
                        FROM [dbo].[CycleTimeSteps]
                        where [CycleTimeInjectionId] = (Select Id From [InjectingCycleTimeCalculators] where ManufacturingStepId = {manufacturing.id})";

            DataTable properties = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
            if (properties.Rows.Count == 0) return;
                        
            foreach(DataRow property in properties.Rows)
            {
                string name = property["Name"]?.ToString();
                if (name?.ToLower().Contains("slide")==true)
                {
                    worksheet.Range["AS18"].Value = property["CycleTimeRelevance"];
                    worksheet.Range["AT18"].Value = property["ManualSecondaryTime"];
                }
                else if (name?.Contains("기본") == true)
                {
                    worksheet.Range["AS16"].Value = property["CycleTimeRelevance"];
                    worksheet.Range["AT16"].Value = property["ManualSecondaryTime"];
                }
                else if (name?.ToLower().Contains("insert") == true)
                {
                    worksheet.Range["AS17"].Value = property["CycleTimeRelevance"];
                    worksheet.Range["AT17"].Value = property["ManualSecondaryTime"];
                }
            }
            worksheet.Range["AT19"].Formula = $"=SUM(AT16:AT18)";
            //worksheet.Range["AS25"].Formula = $"{manufacturing.netCycleTime}";

            worksheet.Range["AR23"].Formula = $"=T{row}";
            worksheet.Range["AS23"].Formula = $"=R{row}";
            worksheet.Range["AT23"].Formula = $"=AR23*60/AS23";

            worksheet.Range["AS27"].Formula = $"=X{row}";
            worksheet.Range["AS28"].Formula = $"=AC{row}";
            worksheet.Range["AS29"].Formula = $"=AD{row}";
            worksheet.Range["AS30"].Formula = $"=AA{row}";
            worksheet.Range["AS31"].Formula = $"=Z{row}";
            worksheet.Range["AS32"].Formula = $"=AG{row}";
            worksheet.Range["AS33"].Formula = $"=AH{row}";
            worksheet.Range["AS34"].Formula = $"=AI{row}";

            worksheet.Range["AT27"].Formula = $"=AB{row}";
            worksheet.Range["AT28"].Formula = $"=AC{row}";
            worksheet.Range["AT29"].Formula = $"=AD{row}";
            worksheet.Range["AT30"].Formula = $"=AA{row}";
            worksheet.Range["AT31"].Formula = $"=Z{row}";
            worksheet.Range["AT32"].Formula = $"=AG{row}";
            worksheet.Range["AT33"].Formula = $"=AH{row}";
            worksheet.Range["AT34"].Formula = $"=AI{row}";
        }
    }
}
