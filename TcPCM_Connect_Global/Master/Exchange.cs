using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static TcPCM_Connect_Global.Report;

namespace TcPCM_Connect_Global
{
    /// <summary>
    /// 
    /// </summary>
    public class Exchange
    {
        /// <summary>
        /// 
        /// </summary>
        public string Import(DataGridView dgv)
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/MasterData/Import";

            JArray exchange = new JArray();
            JArray exchangeDetail = new JArray();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["통화"].Value == null || row.Cells["통화"].Value?.ToString().Length <= 0) continue;

                //if (row.Cells["ISO"].Value?.ToString() == "KRW") continue;

                JObject item = new JObject
                {
                    { "이름", row.Cells["이름"].Value?.ToString() },
                    { "통화", row.Cells["통화"].Value?.ToString() }
                };
                exchange.Add(item);

                string time = row.Cells["Valid From"].Value == null ? "" : !DateTime.TryParse(row.Cells["Valid From"].Value.ToString(), out DateTime dt) ? row.Cells["Valid From"].Value.ToString() : dt.ToString("yyyy-MM-dd");
                item = new JObject
                {
                    { "통화", row.Cells["통화"].Value?.ToString() },
                    { "환율", global.ConvertDouble(row.Cells["환율"].Value)},
                    { "구분자", row.Cells["구분자"].Value?.ToString() },
                    //global.ConvertDouble(eur[row.Cells["Valid From"].Value])/Math.Round( global.ConvertDouble(row.Cells["환율"].Value), 4) },
                    { "Valid From", time }
                };
                exchangeDetail.Add(item);
            }

            string err = null;
            JObject postData = new JObject
            {
                { "Data", exchange },
                { "ConfigurationGuid", global_iniLoad.GetConfig("ExchangeRate", "Import_Header") }
            };
            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            if(err == null)
            {
                postData = new JObject
                {
                    { "Data", exchangeDetail },
                    { "ConfigurationGuid", global_iniLoad.GetConfig("ExchangeRate", "Import_Detail") }
                };
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            }

            return err; 
        }
    }
}
