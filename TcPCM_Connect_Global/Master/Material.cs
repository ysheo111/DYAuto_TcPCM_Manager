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
                materialType = MasterData.Material.price;
            else
                materialType = MasterData.Material.material;

            JArray substance = new JArray();
            JArray scrap = new JArray();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                JObject item = new JObject();
                JObject scrapItem = new JObject();
                string materialName = null;
                if (row.Cells["재질명"].Value == null || row.Cells["재질명"].Value?.ToString().Length <= 0) continue;
                foreach (string category in materialType) 
                {
                    if (category.Contains("소재명"))
                    {
                        materialName = row.Cells[category].Value?.ToString();
                        if(materialName == "알루미늄")
                            materials = "Siemens.TCPCM.Classification.Material.RawMaterial.CastingMaterial";
                        else if (materialName == "플라스틱")
                            materials = "Siemens.TCPCM.Classification.Material.RawMaterial.Plastic";
                        else if (materialName == "철판" || materialName == "철강" || materialName == "특수강")
                             materials = "Siemens.TCPCM.Classification.Material.SemiFinished.SheetMetal.Plate";
                        else
                            materials = "";
                        item.Add("Materials", $"{materials}");
                        scrapItem.Add("Materials", $"Siemens.TCPCM.Classification.Material.Scrap");
                        item.Add("Designation", $"[DYA]{row.Cells[category].Value}");
                    }
                    else if (category.Contains("재질명"))
                    {
                        if (type != "원소재 단가")
                        {
                            item.Add(category, $"{row.Cells[category].Value}");
                            item.Add("Designation", $"[DYA]{row.Cells[category].Value}");
                        }
                        else
                        {
                            item["Designation"] = $"{item["Designation"]}_{row.Cells[category].Value}";
                            item.Add("substance", $"{row.Cells[category].Value}");
                        }
                    }
                    else if (category.Contains("GRADE"))
                    {
                        item["Designation"] = $"{item["Designation"]}_{row.Cells[category].Value}";
                        
                        if (type != "원소재 단가")
                            item["재질명"] = $"{item["재질명"]}_{row.Cells[category].Value}";
                        else
                        {
                            item["substance"] = $"{item["substance"]}_{row.Cells[category].Value}";
                            scrapItem.Add("substance", item["substance"]);

                            item["Item number"] = $"{item["substance"]}";
                            scrapItem.Add("Item number", $"{item["substance"]}_SCRAP");

                            item["revision"] = $"{item["substance"]} ||";
                            scrapItem.Add("revision", $"{item["substance"]}_SCRAP ||");
                            scrapItem.Add("Designation", $"{item["Designation"]}_SCRAP");
                        }
                    }
                    else if (category.Contains("원재료 단위"))
                        item.Add("단위", row.Cells[category].Value?.ToString().ToLower());
                    else if (category.Contains("스크랩 단위"))
                        scrapItem.Add("단위", row.Cells[category].Value?.ToString().ToLower());
                    else if (category.Contains("원재료 단가"))
                        item.Add("단가", row.Cells[category].Value?.ToString().ToLower());
                    else if (category.Contains("스크랩 단가"))
                        scrapItem.Add("단가", row.Cells[category].Value?.ToString().ToLower());
                    else
                    {
                        item.Add(category, row.Cells[category].Value?.ToString());
                        scrapItem.Add(category, row.Cells[category].Value?.ToString());
                    } 
                }
                substance.Add(item);
                scrap.Add(scrapItem);
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
                        if (err == null)
                        {
                            postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Material", "Import_Carbon");
                            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                            if (scrap.Count != 0 && err == null)
                            {
                                postData["Data"] = scrap;
                                postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Material", "Import_Header");
                                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                                if (err == null)
                                {
                                    postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Material", "Import_Revision");
                                    err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                                    if (err == null)
                                    {
                                        postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Material", "Import_Detail");
                                        err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                                        if (err == null)
                                        {
                                            postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Material", "Import_Carbon");
                                            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);


            return err;
          
        }
    }
}
