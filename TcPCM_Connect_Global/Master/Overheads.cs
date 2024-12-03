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
                    if (col.Name.Contains("율") && row.Cells[col.Name].Value != null) 
                    {
                        JObject item = new JObject(detailInfo);
                        item.Add("Designation", $"{col.Tag}||Siemens.TCPCM.Classification.Overhead.Rate");
                        item.Add("Value", row.Cells[col.Name].Value?.ToString());
                        category.Add(item);
                    }
                    else detailInfo.Add(col.Name, row.Cells[col.Name].Value?.ToString());
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
