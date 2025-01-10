using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace TcPCM_Connect_Global
{
    /// <summary>
    /// 
    /// </summary>
    public class Material
    { 
        public string Import(string type, DataGridView dgv)
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/MasterData/Import";
            List<string> materialType = new List<string>();
            string materials="", materialCategory = "";
            if (type == "다이캐스팅")
            {
                materialType = MasterData.Material.casting;
                materials = "RawMaterial.CastingMaterial";
                materialCategory = "Diecasting";
            }
            else if (type == "사출")
            {
                materialType = MasterData.Material.injection;
                materials = "RawMaterial.Plastic";
                materialCategory = "Injection";
            }
            else if (type == "프레스")
            {
                materialType = MasterData.Material.plate;
                materials = "SemiFinished.SheetMetal.Coil";
                materialCategory = "Press";
            }
            else if(type == "원소재 단가")
            {
                materialType = MasterData.Material.price;
            }
            else
            {
                materialType = MasterData.Material.material;
            }

            JArray substance = new JArray();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                JObject item = new JObject();
                if (row.Cells["재질명"].Value == null || row.Cells["재질명"].Value?.ToString().Length <= 0) continue;
                foreach (string category in materialType) 
                {
                    if (category.Contains("재질명"))
                    {
                        if (type != "원소재 단가")
                            item.Add("재질명", $"{row.Cells[category].Value}");
                        else
                            item.Add("substance", $"{row.Cells[category].Value}");
                        item.Add("Designation", $"[DYA]{row.Cells[category].Value}");
                    }
                    else if (category.Contains("GRADE"))
                    {
                        item["Designation"] = $"{item["Designation"]}_{row.Cells[category].Value}";
                        if (type != "원소재 단가")
                            item["재질명"] = $"{item["재질명"]}_{row.Cells[category].Value}";
                        else
                        {
                            item["substance"] = $"{item["substance"]}_{row.Cells[category].Value}";
                            item["Item number"] = $"{item["substance"]}_{row.Cells[category].Value}_SCRAP";
                            item["revision"] = $"{item["substance"]}_{row.Cells[category].Value}_SCRAP||";
                            item["Designation"] = $"{item["Designation"]}_SCRAP";
                        }
                    }
                    else item.Add(category, row.Cells[category].Value?.ToString());
                }
                if (type == "코어")
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        item["구분"] = $"{(i == 1 ? "원형2" : i == 2 ? "Ear-31" : "Ear-41")}";
                        item["Column1"] =  $"{item["재질명"]}{item["구분"]}";
                        substance.Add(new JObject(item));
                    }

                }

                substance.Add(item);
            }

            JObject postData = new JObject
            {
                { "Data", substance },
                { "ConfigurationGuid", global_iniLoad.GetConfig("Substance", type == "코어" ? "프레스" : type) }
            };
            string err = null;

            if(type == "원소재 단가")
            {
                postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Material", "Import_Header");
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                if(err == null)
                {
                    postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Material", "Import_Revision");
                    err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                    if (err == null)
                    {
                        postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Material", "Import_Detail");
                        err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                    }
                }
            }
            else
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);

            return err;
          
        }
    }
}
