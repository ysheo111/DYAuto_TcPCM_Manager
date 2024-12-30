using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcPCM_Connect_Global
{
    public class PartBomExport
    {
        public string ExportPartBom(List<string> nodes)
        {
            List<string> calcList = AllRootCalcId(nodes);
            JObject apiResult = LoadCalc(calcList);
            if (apiResult == null) return "데이터 조회 시 오류가 발생하였습니다.";
            PartBomSorting(apiResult);
            return null;
        }

        private List<string> AllRootCalcId(List<string> nodes)
        {

            var projects = nodes.Where(n => n.StartsWith("p")).Select(n => n.Substring(1)).ToList();
            var folders = nodes.Where(n => n.StartsWith("f")).Select(n => n.Substring(1)).ToList();
            var calculations = nodes.Where(n => !n.StartsWith("p") && !n.StartsWith("f")).ToList();

            var query = $@"Select Id as name 
                        from 
                            (select PartId 
                             from FolderEntries 
                             where FolderId in ({string.Join(",", folders)})
                             Union All
                             select PartId 
                             from ProjectPartEntries 
                             where ProjectId in ({string.Join(",", projects)})) as a
                        Left Join Calculations as b 
                        on a.PartID = b.PartID 
                        where Master = 1";

            return global_DB.ListSelect(query, (int)global_DB.connDB.PCMDB).Concat(calculations).ToList();
        }

        private JObject LoadCalc(List<string> calcList)
        {
            if (calcList?.Count <= 0) return null;

            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Export";

            JObject postData = new JObject();
            postData.Add("CalculationIds", JArray.FromObject(calcList));
            postData.Add("ConfigurationGuid", global_iniLoad.GetConfig("CBD", "Export"));
            var apiResult = WebAPI.POST(callUrl, postData);

            if (apiResult?.Length <= 0) return null;
            JObject r = JObject.Parse(apiResult);
            if (!r.ContainsKey("data"))
            {
                return null;
            }

            //var chartData = bomExport.SimpleDataSort(apiResult);
            return r;
        }

        private void PartBomSorting(JObject apiResult)
        {
            Dictionary<string, Part> partList = new Dictionary<string, Part>();
            Part part = new Part();
            string prevRootPart = "";
            try
            {
                foreach (var element in apiResult["data"])
                {
                    Dictionary<string, object> values = element.ToObject<Dictionary<string, object>>();
                    if (values[Report.LineType.level] == null)
                    {                        
                        if(prevRootPart.Length>0) partList.Add(prevRootPart, part);
                        part = new Part();
                        prevRootPart = values[Report.Header.partID]?.ToString();

                        part.header.modelName = values[Report.Header.modelName]?.ToString();
                        part.header.partNumber = values[Report.Header.partNumber]?.ToString();
                        part.header.partName = values[Report.Header.partName]?.ToString();

                        part.header.company = values[Report.Header.company]?.ToString();
                        part.header.customer = values[Report.Header.customer]?.ToString();
                        part.header.currency = values[Report.Header.currency]?.ToString();
                        part.header.transport = values[Report.Header.transport]?.ToString();

                        part.header.category = values[Report.Header.category]?.ToString();
                        part.header.suppier = values[Report.Header.suppier]?.ToString();
                        part.header.exchangeRate = global.ConvertDouble(values[Report.Header.exchangeRate]);
                        part.header.exchangeRateCurrency = values[Report.Header.exchangeRateCurrency]?.ToString();

                        part.header.author = values[Report.Header.author]?.ToString();
                        part.header.dateOfCalculation = DateTime.Parse(values[Report.Header.dateOfCalc]?.ToString());

                        part.summary.administrationCosts = global.ConvertDouble(values[Report.Summary.administrationCosts+ "[%]"]);
                        part.summary.profit = global.ConvertDouble(values[Report.Summary.profit + "[%]"]);
                        part.summary.materialOverhead = global.ConvertDouble(values[Report.Summary.materialOverhead + "[%]"]);
                        part.summary.rnd = global.ConvertDouble(values[Report.Summary.rnd]);
                        part.summary.packageTransport = global.ConvertDouble(values[Report.Summary.packageTransport]);
                        part.summary.etc = global.ConvertDouble(values[Report.Summary.etc]);
                    }
                    else if (values[Report.LineType.view]?.ToString().Contains("Buyer view") == true) continue;
                    else if(values[Report.LineType.lineType]?.ToString().Contains("Raw material") ==true)
                    {
                        if(values[Report.Material.name]?.ToString().ToLower().Contains("scrap")==true)
                        {
                            part.material[part.material.Count - 1].scrapUnitPrice = global.ConvertDouble(values[Report.Material.rawMaterial]);
                            part.material[part.material.Count - 1].netWeight += global.ConvertDouble(values[Report.Material.quantity]);
                        }
                        else
                        {
                            part.material[part.material.Count - 1].unitCost = global.ConvertDouble(values[Report.Material.rawMaterial]);
                            part.material[part.material.Count - 1].netWeight += global.ConvertDouble(values[Report.Material.quantity]);
                            part.material[part.material.Count - 1].grossWeight = global.ConvertDouble(values[Report.Material.quantity]);
                            part.material[part.material.Count - 1].qunantityUnit = values[Report.Material.unit]?.ToString();
                        }
                    }
                    else if (values[Report.LineType.lineType]?.ToString().Contains("Part") == true)
                    {
                        Part.Material material = new Part.Material();
                        material.name = values[Report.Material.name]?.ToString();
                        material.itemNumber = values[Report.Material.itemNumber]?.ToString();
                        material.substance = values[Report.Material.substance]?.ToString();
                        material.quantity = global.ConvertDouble(values[Report.Material.quantity]);

                        part.material.Add(material);
                    }
                    else if (values[Report.LineType.lineType]?.ToString().Contains("Detailed manufacturing step") == true)
                    {
                        Part.Manufacturing manufacturing = new Part.Manufacturing();
                        //manufacturing.manufacturingName = 
                        part.manufacturing.Add(manufacturing);
                    }
                    //else if (values[Report.LineType.lineType]?.ToString().Contains("Machine") == true)
                    //{
                    //    part.manufacturing[part.manufacturing.Count - 1]
                    //}
                    //else if (values[Report.LineType.lineType]?.ToString().Contains("Labor") == true)
                    //{
                    //    part.manufacturing[part.manufacturing.Count - 1]
                    //}
                }

                if (prevRootPart.Length > 0) partList.Add(prevRootPart, part);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(partList);
        }


        //String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Export";

        //JObject postData = new JObject();
        //postData.Add("CalculationIds", JArray.FromObject(itemList));
        //postData.Add("ConfigurationGuid", global_iniLoad.GetConfig("CBD", "Export"));
        //var apiResult = WebAPI.POST(callUrl, postData);
    }
}
