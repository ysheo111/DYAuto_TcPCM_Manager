using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcPCM_Connect_Global
{
    public class Overheads
    {
        public string Import(string className, string name, DataGridView dgv)
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/MasterData/Import";

            JArray category = new JArray();
            JArray category2 = new JArray();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                JObject detailInfo = new JObject();
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    string cellValue = row.Cells[col.Name].Value?.ToString();
                    if(name.Contains("재료관리비율") || name == "판매관리비율" || name == "재료 Loss율" || name == "경제성 검토")
                    {
                        if (col.Name.Contains("율") && !string.IsNullOrEmpty(cellValue))
                        {
                            cellValue = cellValue.Replace("%","");
                            detailInfo.Add("관리비율", cellValue);
                            if(name == "판매관리비율")
                                detailInfo.Add("Overhead rate unique identifier (Overhead rate detail)", $"{col.Tag}");
                            else
                                detailInfo.Add("Overhead rate unique identifier (Overhead rate detail)", $"{col.Tag}||Siemens.TCPCM.Classification.Overhead.Rate");
                            category.Add(detailInfo);
                        }
                        else if (col.Name.Contains("Customer"))
                            detailInfo.Add("업종", cellValue);
                        else if (col.Name.Contains("업종"))
                        {
                            if(name == "사외 재료관리비율")
                                detailInfo.Add(col.Name, $"{row.Cells["지역"].Value}||{cellValue}");
                            else
                                detailInfo.Add(col.Name, $"{row.Cells["Plant"].Value}||{cellValue}");
                        }
                        else if((col.Name.Contains("WACC") || col.Name.Contains("법인세") || col.Name.Contains("운전 자금")) && !string.IsNullOrEmpty(cellValue))
                        {
                            cellValue = cellValue.Replace("%", "");

                            JObject Info = (JObject)detailInfo.DeepClone();
                            Info.Add("관리비율", cellValue);
                            Info.Add("Overhead rate unique identifier (Overhead rate detail)", $"{col.Tag}||Siemens.TCPCM.Classification.Overhead.Rate");
                            category.Add(Info);
                        }
                        else
                            detailInfo.Add(col.Name, cellValue);
                    }
                    else if(name == "년간손익분석")
                    {
                        if((col.Name.Contains("금융비율") || col.Name.Contains("법인세")) && !string.IsNullOrEmpty(cellValue))
                        {
                            cellValue = cellValue.Replace("%", "");
                            JObject Info = (JObject)detailInfo.DeepClone();
                            Info.Add("관리비율", cellValue);
                            Info.Add("Overhead rate unique identifier (Overhead rate detail)", $"{col.Tag}||Siemens.TCPCM.Classification.Overhead.Rate");
                            category.Add(Info);
                        }
                        else if ((col.Name.Contains("판가 A/CR율") || col.Name.Contains("구매 A/CR율") ||
                            col.Name.Contains("직접노무비율") || col.Name.Contains("간접노무비율") || col.Name.Contains("경비율")) && !string.IsNullOrEmpty(cellValue))
                        {
                            cellValue = cellValue.Replace("%", "");
                            JObject Info = (JObject)detailInfo.DeepClone();
                            Info.Add("관리비율", cellValue);
                            Info.Add("Overhead rate unique identifier (Overhead rate detail)", $"{col.Tag}");// Siemens.TCPCM.Classification.Increase.Rate");
                            category2.Add(Info);
                        }
                        else if (col.Name.Contains("Plant"))
                        {
                            if (cellValue != null)
                                detailInfo.Add(col.Name, cellValue);
                            else
                                detailInfo.Add(col.Name, row.Cells["지역"].Value?.ToString());
                        }
                        else
                            detailInfo.Add(col.Name, cellValue);
                    }
                    else
                    {
                        if (col.Name.Contains("율") && row.Cells[col.Name].Value != null)
                        {
                            JObject Info = (JObject)detailInfo.DeepClone();
                            Info.Add("관리비율", cellValue);
                            Info.Add("Overhead rate unique identifier (Overhead rate detail)", $"{col.Tag}||Siemens.TCPCM.Classification.Overhead.Rate");
                            category.Add(Info);
                        }
                        else if (col.Name.Contains("지역"))
                        {
                            detailInfo.Add(col.Name, cellValue);
                            detailInfo.Add("Plant", cellValue);
                        }
                        else if(col.Name.Contains("업종"))
                            detailInfo.Add(col.Name, $"{row.Cells["지역"].Value}||{cellValue}");
                        else detailInfo.Add(col.Name, cellValue);
                    }
                }
            }
            string err = null;
            //string configName = name.Replace(" ", "").Contains("재료관리비율") ? "재료관리비율" : name.Replace(" ", "");
            JObject postData = new JObject
            {
                { "Data", category },
                { "ConfigurationGuid", global_iniLoad.GetConfig(className.Replace("4", ""), name.Replace(" ", "")) }
            };
            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            if (name == "년간손익분석" && string.IsNullOrEmpty(err))
            {
                postData = new JObject
                {
                    { "Data", category2 },
                    { "ConfigurationGuid", global_iniLoad.GetConfig("IncreaseRate", name.Replace(" ","")) }
                };
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            }

            return err;
        }
    }
}
