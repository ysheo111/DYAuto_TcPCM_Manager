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

            foreach (DataGridViewRow row in dgv.Rows)
            {
                JObject detailInfo = new JObject();
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    string cellValue = row.Cells[col.Name].Value?.ToString();
                    if(name == "재료관리비" || name == "판매관리비율")
                    {
                        if (col.Name.Contains("율") && row.Cells[col.Name].Value != null)
                        {
                            cellValue = cellValue.Replace("%","");
                            detailInfo.Add("관리비율", cellValue);
                            detailInfo.Add("Overhead rate unique identifier (Overhead rate detail)", $"{col.Tag}||Siemens.TCPCM.Classification.Overhead.Rate");
                            category.Add(detailInfo);
                        }
                        else if (col.Name.Contains("Plant"))
                        {
                             if(cellValue != null)
                                detailInfo.Add(col.Name, cellValue);
                             else
                                detailInfo.Add(col.Name, row.Cells["지역"].Value?.ToString());
                        }
                        else if (col.Name.Contains("업종"))
                        {
                            if(name == "판매관리비율")
                            {
                                detailInfo.Add(col.Name, cellValue);
                            }
                            else
                            {
                                if (cellValue != null)
                                    detailInfo.Add(col.Name, $"{row.Cells["Plant"].Value?.ToString()}||{cellValue}");
                                else
                                    detailInfo.Add(col.Name, "||");
                            }
                        }
                        else
                            detailInfo.Add(col.Name, cellValue);
                    }
                    else
                    {
                        if (col.Name.Contains("율") && row.Cells[col.Name].Value != null) 
                        {
                            //JObject item = new JObject(detailInfo);
                            //item.Add("Designation", $"{col.Tag}||Siemens.TCPCM.Classification.Overhead.Rate");
                            //item.Add("Value", cellValue);
                            //category.Add(item);
                        }
                        else detailInfo.Add(col.Name, cellValue);
                    }
                }
            }
            string err = null;
            JObject postData = new JObject
            {
                { "Data", category },
                { "ConfigurationGuid", global_iniLoad.GetConfig(className.Replace("4",""), name.Replace(" ","")) }
            };
            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);           

            return err;
        }
    }
}
