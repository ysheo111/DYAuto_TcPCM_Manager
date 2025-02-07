using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace TcPCM_Connect_Global
{
    /// <summary>
    /// 
    /// </summary>
    public class CostFactor
    {
        string USDesignName = null;
        public string Import(string className, string name, DataGridView dgv)
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/MasterData/Import";

            JArray category = new JArray();
            JArray categoryLabor = new JArray();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                JObject item = new JObject();

                bool nullCheck = false;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (row.Cells[col.Name].Value != null && !col.Name.ToLower().Contains("valid")) nullCheck = true;

                    if (name == "Plant")
                    {
                        item.Add("지역", row.Cells["지역"].Value?.ToString());
                        item.Add("Designation", row.Cells["Designation"].Value?.ToString());

                        if(!string.IsNullOrEmpty(row.Cells["Designation-US"].Value?.ToString()))
                            item.Add("Designation-US", "[DYA]"+row.Cells["Designation-US"].Value.ToString());
                        else if(!string.IsNullOrEmpty(USDesignName) )
                            item.Add("Designation-US", USDesignName);
                        else
                            item.Add("Designation-US", null);

                        item.Add("Number", row.Cells["지역"].Value?.ToString());
                        break;
                    }

                    //if (name == "직접임률" && (col.Name.Contains("간접") || col.Name.Contains("경비")))
                    //    continue;
                    //else if (name == "간접임률" && (col.Name.Contains("직접") || col.Name.Contains("경비")))
                    //    continue;

                    if (col.Name.Contains("연간 작업 일수"))
                        item.Add(col.Name, global.ConvertDouble(row.Cells[col.Name].Value) * global.ConvertDouble(row.Cells["Shift 당 작업 시간"].Value) * global.ConvertDouble(row.Cells["Shift"].Value));
                    else if (col.Name.Contains("Labor"))
                    {
                        JObject addictionalItem = new JObject(item);
                        string categoryName = $"Siemens.TCPCM.MasterData.CostFactor.Common.LaborBurdenShift{System.Text.RegularExpressions.Regex.Match(col.Name, @"\d+").Value}";
                        addictionalItem.Add("Labor burden", row.Cells[col.Name].Value?.ToString());
                        addictionalItem.Add("구분", categoryName);
                        if (nullCheck) categoryLabor.Add(addictionalItem);
                    }
                    else if (col.Name.Contains("Designation-US") && !string.IsNullOrEmpty(item["지역"]?.ToString()) )
                    {
                        if(row.Cells[col.Name].Value == null)
                        {
                            string searchQuery = $"select Name_LOC from BDRegions" +
                                $" where CAST(UniqueKey AS NVARCHAR(MAX)) like N'%{item["지역"].ToString()}%'" +
                                $" And CAST(Name_LOC AS NVARCHAR(MAX)) like '%en-US%'";
                            string result = global_DB.ScalarExecute(searchQuery, (int)global_DB.connDB.PCMDB);
                            USDesignName = NameSplit(result);
                            item.Add(col.Name, USDesignName);
                        }
                        else
                            item.Add(col.Name, "[DYA]"+row.Cells[col.Name].Value?.ToString());
                    }
                    else
                        item.Add(col.Name, row.Cells[col.Name].Value?.ToString());
                }
                if (name == "공간 생산 비용")
                {
                    item.Add("결과"
                        , global.ConvertDouble(item["건축비"]) * global.ConvertDouble(item["건물점유비율"]) / 100 / global.ConvertDouble(item["건물상각년수"]) / 12 );
                    item.Add("건물상각년수(년)"
                        , $@"건축비 : {global.ConvertDouble(item["건축비"])} 건물점유비율 : {global.ConvertDouble(item["건물점유비율"])} 건물상각년수 : {global.ConvertDouble(item["건물상각년수"])} 계산수식 : 건축비 * 건물점유비율 / 건물상각년수 / 100 / 12");
                }

                if (nullCheck) category.Add(item);
            }
            string err = null;


            if (name == "작업일수")
            {
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", global_iniLoad.GetConfig(className, "작업일수") }
                }), err);

                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", global_iniLoad.GetConfig(className, "작업시간") }
                }), err);

                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", global_iniLoad.GetConfig(className, "Shift") }
                }), err);

            }
            if(className == "ExchangeRate")
            {
                JObject postData = new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", global_iniLoad.GetConfig(className, name+"_Header") }
                };
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);

                if (err == null)
                {
                    postData = new JObject
                    {
                        { "Data", category },
                        { "ConfigurationGuid", global_iniLoad.GetConfig(className, name+"_Detail") }
                    };
                    err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                }
            }
            else
            {
                JObject postData = new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", global_iniLoad.GetConfig(className, name.Replace(" ","")) }
                };
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            }

            if (name == "임률")
            {
                JObject postData = new JObject
                    {
                        { "Data", categoryLabor },
                        { "ConfigurationGuid", global_iniLoad.GetConfig(className, "LaborBurden") }
                    };
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            }

            return err;
        }

        public string SegmantImport(string className, string name, DataGridView dgv, List<string> segmantList)
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/MasterData/Import";

            JArray category = new JArray();
            JArray categoryLabor = new JArray();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach(string segmant in segmantList)
                {
                    JObject item = new JObject();
                    bool nullCheck = false;

                    if (row.Cells["지역"].Value != null) nullCheck = true;

                    item.Add(name, segmant);
                    item.Add("Designation", "[DYA]" + segmant);
                    item.Add("Plant", row.Cells["지역"].Value?.ToString());
                    if (nullCheck) category.Add(item);
                    else break;
                }
            }
            string err = null;

            JObject postData = new JObject
            {
                { "Data", category },
                { "ConfigurationGuid", global_iniLoad.GetConfig(className, name.Replace(" ","")) }
            };
            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);

            return err;
        }

        public string NameSplit(string input)
        {
            //List<string> desiredLanguages = new List<string>() { "en-US", "ko-KR", "ru-RU", "ja-JP", "pt-BR", "de-DE" };
            string delimiter = "</split>";
            string[] xmlFiles = input.Split(new string[] { delimiter }, StringSplitOptions.None);

            for (int i = 0; i < xmlFiles.Length; i++)
            {
                string name = "";
                string xmlString = xmlFiles[i];
                try
                {   
                    XDocument doc = XDocument.Parse(xmlString);

                    var enValue = doc.Descendants("value")
                             .FirstOrDefault(v => (string)v.Attribute("lang") == "en-US");

                    if (enValue != null)
                    {
                        name = enValue.Value;
                    }
                }
                catch
                {
                    if (i == xmlFiles.Length - 1)
                        name = $"{xmlString}";
                }
                if (!string.IsNullOrEmpty(name))
                    input = name;
            }
            return input;
        }
    }
}
