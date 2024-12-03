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
            else if (type == "코어")
            {
                materialType = MasterData.Material.plate;
                materials = "SemiFinished.SheetMetal.Plate";
                materialCategory = "Press";
            }
            else if (type == "가공")
            {
                materialType = MasterData.Material.process;
                materials = "RawMaterial";
                materialCategory = "Machining";
            }

            JArray substance = new JArray();
            JArray material = new JArray();
            JArray scrap = new JArray();
            JArray dross = new JArray();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                JObject item = new JObject();
                if (row.Cells["재료명"].Value == null || row.Cells["재료명"].Value?.ToString().Length <= 0) continue;
                foreach (string category in materialType) 
                {
                    if (category.Contains("재료명"))
                    {
                        item.Add(category, $"[LGMagna]{row.Cells[category].Value}");
                        item.Add("Column1", $"[LGMagna]{row.Cells[category].Value}");
                    }
                    else item.Add(category, row.Cells[category].Value?.ToString());
                }

                if (type == "코어")
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        item["구분"] = $"{(i == 1 ? "원형2" : i == 2 ? "Ear-31" : "Ear-41")}";
                        item["Column1"] =  $"{item["재료명"]}{item["구분"]}";
                        substance.Add(new JObject(item));
                    }

                }

                substance.Add(item);

                item = new JObject();
                JObject scrapitem = new JObject();
                JObject drossitem = new JObject();
                foreach (string category in MasterData.Material.material)
                {
                    if (category.Contains("가격"))
                    {
                        string unit = row.Cells[category].Value?.ToString();
                        if (category.Contains("단위"))
                        {
                            unit = row.Cells[category].Value == null ? "kg" : row.Cells[category].Value?.ToString().ToLower();
                            unit= unit.Replace("ea","pcs");
                            unit = unit.Replace("shot", "Shot");
                        }
                        item.Add(category, unit);

                        if (type == "다이캐스팅")
                        {
                            string unit2 = $"-{row.Cells[category.Replace("가격", "DROSS 비용")].Value?.ToString()}";
                            if (category.Contains("단위")) unit2 = row.Cells[category.Replace("가격", "DROSS 비용")].Value == null ? "kg" : row.Cells[category.Replace("가격", "DROSS 비용")].Value?.ToString().ToLower();
                            drossitem.Add(category, unit2);
                        }
                    }
                    else if (category.Contains("스크랩"))
                    {
                        string unit = row.Cells[category].Value?.ToString();
                        if (category.Contains("단위"))
                        {
                            unit = row.Cells[category].Value == null ? "kg" : row.Cells[category].Value?.ToString().ToLower();
                            unit = unit.Replace("ea", "pcs");
                            unit = unit.Replace("shot", "Shot");
                        }
                        scrapitem.Add(category.Replace("스크랩 비용", "가격"), unit);
                    }
                    
                    else if (category.Contains("Valid From"))
                    {
                        string time = row.Cells[category].Value == null ? "" : !DateTime.TryParse(row.Cells[category].Value.ToString(), out DateTime dt) ? row.Cells[category].Value.ToString() : dt.ToString("yyyy-MM-dd");
                        item.Add(category, time);
                        scrapitem.Add(category, time);
                        drossitem.Add(category, time);
                    }
                    else if (category.Contains("재료명"))
                    {
                        item.Add(category, $"[LGMagna]{row.Cells[category].Value}");
                        scrapitem.Add(category, $"[LGMagna]{row.Cells[category].Value}_Scrap");
                        drossitem.Add(category, $"[LGMagna]{row.Cells[category].Value}_Dross");
                    }
                    else
                    {
                        item.Add(category, $"{row.Cells[category].Value}");
                        scrapitem.Add(category, $"{row.Cells[category].Value}");
                        drossitem.Add(category, $"{row.Cells[category].Value}");
                    }
                }
                
                item.Add("재료명_리비전", $"[LGMagna]{row.Cells["재료명"].Value}||");
                item.Add("Materials", $"Siemens.TCPCM.Classification.Material.{materials}");
                item.Add("Column2", $"{materialCategory}");
                item.Add("Substance", $"[LGMagna]{row.Cells["재료명"].Value}");
                item.Add("Column1", $"Siemens.TCPCM.ProcurementType.Purchase");
                item.Add("산출기준", $"");
                item.Add("외경형상", $"");
                item.Add("외경형상2", $"");
                item.Add("Cavity", $"");

                scrapitem.Add("Materials", "Siemens.TCPCM.Classification.Material.Scrap");
                scrapitem.Add("재료명_리비전", $"[LGMagna]{row.Cells["재료명"].Value}_Scrap||");
                scrapitem.Add("Substance", $"[LGMagna]{row.Cells["재료명"].Value}");
                scrapitem.Add("Column2", $"{materialCategory}");
                scrapitem.Add("Column1", $"Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");
                scrapitem.Add("산출기준", $"");
                scrapitem.Add("외경형상", $"");
                scrapitem.Add("외경형상2", $"");
                scrapitem.Add("Cavity", $"");


                drossitem.Add("재료명_리비전", $"[LGMagna]{row.Cells["재료명"].Value}_Dross||");
                drossitem.Add("Materials", $"Siemens.TCPCM.Classification.Material.{materials}");
                drossitem.Add("Column2", $"{materialCategory}");
                drossitem.Add("Substance", $"[LGMagna]{row.Cells["재료명"].Value}");
                drossitem.Add("Column1", $"Siemens.TCPCM.ProcurementType.InternalSale");
                drossitem.Add("산출기준", $"");
                drossitem.Add("외경형상", $"");
                drossitem.Add("외경형상2", $"");
                drossitem.Add("Cavity", $"");

                if (type=="코어")
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        item["재료명"] = $"[LGMagna]{row.Cells["재료명"].Value}_{i}";
                        item["재료명_리비전"] = $"[LGMagna]{row.Cells["재료명"].Value}_{i}||";
                        item["Cavity"] = $"{(Math.Floor((double)i / 3) != 0 ? 1 : 2)}";
                        item["산출기준"] = $"{(i%3 != 2 ? 2 : i==2 ? 4.5: 3)}";
                        item["외경형상"] = $"{(i % 3 == 1 ? "원형2" : i%3 == 2 ? "Ear-31": "Ear-41")}";
                        item["외경형상2"] = $"{(i%3==0?3:i%3)}";
                        item["Substance"] = $"{item["재료명"]}{item["외경형상"]}"; 
                        material.Add(new JObject(item));
                    }
                    
                }
                else material.Add(item);
                if(scrapitem["가격"]?.ToString().Length > 0) scrap.Add(scrapitem);
                if (drossitem["가격"]?.ToString().Length > 0) dross.Add(drossitem);
            }

            JObject postData = new JObject
            {
                { "Data", substance },
                { "ConfigurationGuid", global_iniLoad.GetConfig("Substance",  "구분") }
            };

            string err = null;
            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Substance", type == "코어" ? "프레스" : type);
            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);

            List<string> import = new List<string>() {"Import_Header", "Import_Revision", "Import_Detail" };

            foreach (string item in import)
            {
                postData["Data"] = material;
                postData["ConfigurationGuid"] = global_iniLoad.GetConfig("Material", item);
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);

                if (dross.Count != 0)
                {
                    postData["Data"] = dross;
                    err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                }

                if (scrap.Count != 0)
                {
                    postData["Data"] = scrap;
                    err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
                }

              
            }

            return err;
          
        }
    }
}
