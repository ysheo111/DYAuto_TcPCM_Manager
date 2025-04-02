using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TcPCM_Connect_Global
{
    /// <summary>
    /// 
    /// </summary>
    public class Machine
    {
        /// <summary>
        /// 
        /// </summary>
        public string Import(DataGridView dgv, string type)
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/MasterData/Import";
            JArray header = new JArray();
            JArray detail = new JArray();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                bool flag = true;

                JObject item = new JObject();
                JObject detailitem = new JObject();

                if (type == "설비")
                {
                    foreach(string config in MasterData.Machine.machineDetailList)
                    {
                        if (row.Cells["설비명"].Value == null || string.IsNullOrEmpty(row.Cells["설비명"].Value?.ToString())) continue;

                        if (config.Contains("설비명"))
                        {
                            item.Add("Unique identifier", $"{row.Cells[MasterData.Machine.designation1].Value}");
                            detailitem.Add("Unique identifier", $"{row.Cells[MasterData.Machine.designation1].Value}");
                        }
                        else if (config == "업체명") continue;
                        else if (config.Contains("최대 톤수"))
                        {
                            item.Add("최대톤수", $"{global.ConvertDouble( row.Cells[MasterData.Machine.maxClampingForce].Value)}");
                            if (!string.IsNullOrEmpty(row.Cells[MasterData.Machine.maxClampingForce].Value?.ToString()))
                            {
                                item["Unique identifier"] += $"_{row.Cells[MasterData.Machine.maxClampingForce].Value}";
                                detailitem["Unique identifier"] += $"_{row.Cells[MasterData.Machine.maxClampingForce].Value}";
                            }
                        }
                        else if (config.Contains("사양 정보"))
                        {
                            if (!string.IsNullOrEmpty(row.Cells[MasterData.Machine.maker].Value?.ToString()))
                            {
                                item["Unique identifier"] += $"_{row.Cells[MasterData.Machine.maker].Value}";
                                detailitem["Unique identifier"] += $"_{row.Cells[MasterData.Machine.maker].Value}";
                            }
                        }
                        else if (config.Contains("설비구분"))
                        {
                            item.Add("설비구분", row.Cells[MasterData.Machine.manufacturer].Value?.ToString());
                        }
                        else if (config.Contains("전력소비율"))
                        {
                            double poweUtiliation = global.ConvertDouble(global.ConvertDouble(row.Cells[MasterData.Machine.poweUtiliation].Value?.ToString().Replace("%", "")));
                            poweUtiliation = poweUtiliation <= 1 ? poweUtiliation * 100 : poweUtiliation;
                            detailitem.Add(config, poweUtiliation);
                        }
                        else if (config.Contains("업종"))
                        {
                            detailitem.Add("업종", $"한국||{row.Cells["업종"].Value?.ToString()}");
                        }
                        else
                        {
                            detailitem.Add(config, row.Cells[config].Value?.ToString());
                        }
                    }
                    item.Add("Designation", $"[DYA]{item["Unique identifier"]}");
                    detailitem.Add("Designation", $"[DYA]{detailitem["Unique identifier"]}");
                    if (string.IsNullOrEmpty(item["Unique identifier"]?.ToString()) || string.IsNullOrEmpty(detailitem["Unique identifier"]?.ToString())) flag = false;
                }
                if (flag)
                {
                    header.Add(item);
                    detail.Add(detailitem);
                }

                //double poweUtiliation = global.ConvertDouble(global.ConvertDouble(row.Cells[MasterData.Machine.poweUtiliation].Value?.ToString().Replace("%", "")));
                //poweUtiliation = poweUtiliation <= 1 ? poweUtiliation *100 : poweUtiliation;

                //string query = @"select top 1 value as name from [MDCostFactorDetails] as a
                //                join
                //                (
                //                    SELECT *
                //                    FROM[tcpcm2].[dbo].[MDCostFactorHeaders] as a

                //                    where[UniqueKey] = 'Siemens.TCPCM.MasterData.CostFactor.Common.ProductionSpaceCosts'
                //                ) as b
                //                on a.CostFactorHeaderId = b.Id
                //                order by[DateValidFrom] desc";

                //string spaceCosts = global_DB.ScalarExecute(query, (int)global_DB.connDB.PCMDB);
                //item = new JObject
                //{
                //    { MasterData.Machine.designation1, $"[DYA]{row.Cells[MasterData.Machine.designation1].Value}{(force != null ? $" {force}" : "")}" },
                //    { MasterData.Machine.currency, row.Cells[MasterData.Machine.currency].Value?.ToString() },
                //    { MasterData.Machine.acquisition, global.ConvertDouble( row.Cells[MasterData.Machine.acquisition].Value?.ToString() )},
                //    { MasterData.Machine.imputed, global.ConvertDouble(row.Cells[MasterData.Machine.imputed].Value?.ToString()) },
                //    { MasterData.Machine.validFrom, row.Cells[MasterData.Machine.validFrom].Value?.ToString() },
                //    { MasterData.Machine.space, global.ConvertDouble(row.Cells[MasterData.Machine.space].Value?.ToString()) },
                //    { MasterData.Machine.ratedPower, global.ConvertDouble(row.Cells[MasterData.Machine.ratedPower].Value?.ToString()) },
                //    { MasterData.Machine.poweUtiliation, poweUtiliation},
                //    { MasterData.Machine.region, row.Cells[MasterData.Machine.region].Value?.ToString()},
                //    { MasterData.Machine.maintance, row.Cells[MasterData.Machine.maintance].Value?.ToString()},
                //    {"Technical availability (Capital asset detail)", 100},
                //};
                //if (type == "사출") item["Technical availability (Capital asset detail)"] = row.Cells[MasterData.Machine.setupTime].Value?.ToString();
                //    if (item[MasterData.Machine.designation1].ToString() != "[DYA]") detail.Add(item);
            }

            string headerConfig = "";
            if (type=="사출") headerConfig = global_iniLoad.GetConfig("Machine", "Import_Injection_Header");
            else if (type== "다이캐스팅") headerConfig = global_iniLoad.GetConfig("Machine", "Import_Diecast_Header");
            else if (type == "프레스") headerConfig = global_iniLoad.GetConfig("Machine", "Import_Press_Header");
            else if (type == "설비") headerConfig = global_iniLoad.GetConfig("Machine", "Import_Header");
            else headerConfig = global_iniLoad.GetConfig("Machine", "Import_ETC_Header");

            string err = null;
            JObject postData = new JObject
            {
                { "Data", header },
                { "ConfigurationGuid", headerConfig }
            };
            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);

            if(err == null)
            {
                postData = new JObject
                {
                    { "Data", detail },
                    { "ConfigurationGuid", global_iniLoad.GetConfig("Machine", "Import_Detail") }
                };
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);            
            }

            return err; 
        }
    }
}
