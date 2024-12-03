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
            double meltingPower = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                object force = row.Cells[MasterData.Machine.maxClampingForce].Value;
                if (type == "프레스")
                {
                    force = null;
                }
                    //string designation = row.Cells[MasterData.Machine.designation2].Value?.ToString().Length > 0 ? "_": "";
                JObject item = new JObject
                {
                    { MasterData.Machine.designation1, $"[LGMagna]{row.Cells[MasterData.Machine.designation1].Value}{(force != null ? $" {force}" : "")}"  },
                    { MasterData.Machine.maxClampingForce, row.Cells[MasterData.Machine.maxClampingForce].Value?.ToString() },
                     { MasterData.Machine.category, row.Cells[MasterData.Machine.category].Value?.ToString() },
                    
                    { "Column1",  global.ConvertDouble( row.Cells[MasterData.Machine.maxClampingForce].Value)*10 },
                    { "구분 1", null},
                };
                if (type == "다이캐스팅")
                {
                    item.Add(MasterData.Machine.technology, "Siemens.TCPCM.Classification.Technology.DieCasting");
                    item.Add(MasterData.Machine.asset, "Siemens.TCPCM.Classification.CapitalAsset.Machine.Casting.DieCastingMachine");
                    item.Add(MasterData.Machine.resetEjector, row.Cells[MasterData.Machine.blowOutMold2].Value?.ToString());
                    item.Add(MasterData.Machine.closeSilder, row.Cells[MasterData.Machine.closeSilder2].Value?.ToString());
                    item.Add(MasterData.Machine.closeMold, row.Cells[MasterData.Machine.closeMold2].Value?.ToString()); 
                    item.Add(MasterData.Machine.fillMaterial, row.Cells[MasterData.Machine.fillMaterial2].Value?.ToString());
                    item.Add(MasterData.Machine.shotTime, row.Cells[MasterData.Machine.injectPartingAgent2].Value?.ToString());
                    item.Add(MasterData.Machine.openMold, row.Cells[MasterData.Machine.openSilder2].Value?.ToString());
                    double setup = global.ConvertDouble(row.Cells[MasterData.Machine.openMold2].Value)*60;
                    item.Add(MasterData.Machine.blowOutMold, setup);
                    item.Add(MasterData.Machine.injectPartingAgent, 0);
                    item.Add(MasterData.Machine.openSilder, 0);
                    item.Add(MasterData.Machine.removeCast, 0);
                    item.Add(MasterData.Machine.retractEjector, 0);
                    //item.Add(MasterData.Machine.openMold, row.Cells[MasterData.Machine.openMold2].Value?.ToString());
                    //item.Add(MasterData.Machine.removeCast, row.Cells[MasterData.Machine.removeCast2].Value?.ToString());
                    //item.Add(MasterData.Machine.retractEjector, row.Cells[MasterData.Machine.retractEjector2].Value?.ToString());
                    //item.Add(MasterData.Machine.resetEjector, row.Cells[MasterData.Machine.resetEjector2].Value?.ToString());
                    //item.Add(MasterData.Machine.shotTime, row.Cells[MasterData.Machine.shotTime2].Value?.ToString());
    }
                else if (type == "사출")
                {
                    item.Add(MasterData.Machine.asset, "Siemens.TCPCM.Classification.CapitalAsset.Machine.PlasticProcessing.InjectionMolding");
                    item.Add(MasterData.Machine.technology, "Siemens.TCPCM.Classification.Technology.InjectionMolding");
                    item.Add(MasterData.Machine.setupTime, row.Cells[MasterData.Machine.setupTime].Value?.ToString());
                    item.Add(MasterData.Machine.dryRunningTime, row.Cells[MasterData.Machine.dryRunningTime].Value?.ToString());
                    item.Add(MasterData.Machine.meltingPower, row.Cells[MasterData.Machine.meltingPower].Value?.ToString());
                    item.Add(MasterData.Machine.movePlasticizingUnit,row.Cells[ MasterData.Machine.movePlasticizingUnit].Value?.ToString());
                    meltingPower = 1 + (global.ConvertDouble(row.Cells[MasterData.Machine.maxClampingForce].Value) - 65) * 0.006;
                    item["구분 1"] = meltingPower;
                }
                else if (type == "프레스")
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, "Siemens.TCPCM.Classification.CapitalAsset.Machine.StampingDrawingBending.Press");
                    item.Add(MasterData.Machine.setupTime, row.Cells[MasterData.Machine.setupTime].Value?.ToString());
                    item.Add(MasterData.Machine.Foaming, row.Cells[MasterData.Machine.Foaming].Value?.ToString());
                    item.Add(MasterData.Machine.Drawing, row.Cells[MasterData.Machine.Drawing].Value?.ToString());
                    item.Add(MasterData.Machine.SPM, row.Cells[MasterData.Machine.SPM].Value?.ToString());
                }
                else if (row.Cells[MasterData.Machine.process].Value?.ToString().ToLower().Contains("press") == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, "Siemens.TCPCM.Classification.CapitalAsset.Machine.StampingDrawingBending.Press");
                }
                else if (row.Cells[MasterData.Machine.process].Value?.ToString().ToLower().Contains("inspection") == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, "Siemens.TCPCM.Classification.CapitalAsset.Machine.Testing");
                }
                else if (new List<string>() { "progressive" }.Contains(row.Cells[MasterData.Machine.process].Value?.ToString().ToLower()) == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, $"Siemens.TCPCM.Classification.CapitalAsset.Machine.StampingDrawingBending.Progressive");
                }
                else if (new List<string>() { "leak test" }.Contains(row.Cells[MasterData.Machine.process].Value?.ToString().ToLower()) == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, $"Siemens.TCPCM.Classification.CapitalAsset.Machine.Testing.{row.Cells[MasterData.Machine.process].Value}");
                }
                else if (row.Cells[MasterData.Machine.process].Value?.ToString().ToLower().Contains("shot") == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, "Siemens.TCPCM.Classification.CapitalAsset.Machine.Deburring.ShotBlasting");
                }
                else if (new List<string>() { "grinding", "mc", "polishing" }.Contains(row.Cells[MasterData.Machine.process].Value?.ToString().ToLower()) == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, $"Siemens.TCPCM.Classification.CapitalAsset.Machine.Machining.{row.Cells[MasterData.Machine.process].Value}");
                }
                else if (new List<string>() { "smelting furnace" }.Contains(row.Cells[MasterData.Machine.process].Value?.ToString().ToLower()) == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, $"Siemens.TCPCM.Classification.CapitalAsset.Machine.Casting.SmeltingFurnace");
                }
                else if (new List<string>() { "impregnation", "trimming" , "core compaction" }.Contains(row.Cells[MasterData.Machine.process].Value?.ToString().ToLower()) == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, $"Siemens.TCPCM.Classification.CapitalAsset.Machine.Casting.{row.Cells[MasterData.Machine.process].Value?.ToString().Replace(" ","")}");
                }
                else if (new List<string>() { "laser marking" }.Contains(row.Cells[MasterData.Machine.process].Value?.ToString().ToLower()) == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, $"Siemens.TCPCM.Classification.CapitalAsset.Machine.Label.{row.Cells[MasterData.Machine.process].Value?.ToString().Replace(" ", "")}");
                }
                else if (new List<string>() { "laser welding" }.Contains(row.Cells[MasterData.Machine.process].Value?.ToString().ToLower()) == true)
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, $"Siemens.TCPCM.Classification.CapitalAsset.Machine.Joining.LaserWelding");
                }
                else
                {
                    item.Add(MasterData.Machine.technology, "");
                    item.Add(MasterData.Machine.asset, $"Siemens.TCPCM.Classification.CapitalAsset.Machine.{row.Cells[MasterData.Machine.process].Value}");
                }
                
                if (item[MasterData.Machine.designation1].ToString() != "[LGMagna]")  header.Add(item);

                double poweUtiliation = global.ConvertDouble(global.ConvertDouble(row.Cells[MasterData.Machine.poweUtiliation].Value?.ToString().Replace("%", "")));
                poweUtiliation = poweUtiliation <= 1 ? poweUtiliation *100 : poweUtiliation;

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
                item = new JObject
                {
                    { MasterData.Machine.designation1, $"[LGMagna]{row.Cells[MasterData.Machine.designation1].Value}{(force != null ? $" {force}" : "")}" },
                    { MasterData.Machine.currency, row.Cells[MasterData.Machine.currency].Value?.ToString() },
                    { MasterData.Machine.acquisition, global.ConvertDouble( row.Cells[MasterData.Machine.acquisition].Value?.ToString() )},
                    { MasterData.Machine.imputed, global.ConvertDouble(row.Cells[MasterData.Machine.imputed].Value?.ToString()) },
                    { MasterData.Machine.validFrom, row.Cells[MasterData.Machine.validFrom].Value?.ToString() },
                    { MasterData.Machine.space, global.ConvertDouble(row.Cells[MasterData.Machine.space].Value?.ToString()) },
                    { MasterData.Machine.ratedPower, global.ConvertDouble(row.Cells[MasterData.Machine.ratedPower].Value?.ToString()) },
                    { MasterData.Machine.poweUtiliation, poweUtiliation},
                    { MasterData.Machine.region, row.Cells[MasterData.Machine.region].Value?.ToString()},
                    { MasterData.Machine.maintance, row.Cells[MasterData.Machine.maintance].Value?.ToString()},
                    {"Technical availability (Capital asset detail)", 100},
                };
                if (type == "사출") item["Technical availability (Capital asset detail)"] = row.Cells[MasterData.Machine.setupTime].Value?.ToString();
                    if (item[MasterData.Machine.designation1].ToString() != "[LGMagna]") detail.Add(item);
            }

            string headerConfig = "";
            if (type=="사출") headerConfig = global_iniLoad.GetConfig("Machine", "Import_Injection_Header");
            else if (type== "다이캐스팅") headerConfig = global_iniLoad.GetConfig("Machine", "Import_Diecast_Header");
            else if (type == "프레스") headerConfig = global_iniLoad.GetConfig("Machine", "Import_Press_Header");
            else headerConfig = global_iniLoad.GetConfig("Machine", "Import_ETC_Header");

            string err = null;
            JObject postData = new JObject
            {
                { "Data", header },
                { "ConfigurationGuid", headerConfig }
            };

            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);

            postData = new JObject
            {
                { "Data", detail },
                { "ConfigurationGuid", global_iniLoad.GetConfig("Machine", "Import_Detail") }
            };

            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);            

            return err; 
        }
    }
}
