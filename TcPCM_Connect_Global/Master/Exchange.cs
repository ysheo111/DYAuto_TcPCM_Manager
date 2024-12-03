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
            Dictionary<object, object> eur = new Dictionary<object, object>();
            try
            {
                eur = dgv.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells["ISO"].Value?.ToString().Equals("EUR")==true)
                    .GroupBy(r => r.Cells["Valid from"].Value, r => r.Cells["환율"].Value)
                    .ToDictionary(r => r.Key, r => r.Last());

                    //.ToDictionary(pair => );
                    //.First().Cells["환율"].Value); ; ; ;
            }
            catch(Exception e)
            {
                return "기준이 되는 유로 값을 찾을 수 없습니다. 유로를 입력해주세요.";
            }
            

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["ISO"].Value == null || row.Cells["ISO"].Value?.ToString().Length <= 0) continue;

                if (row.Cells["ISO"].Value?.ToString() == "KRW") continue;

                JObject item = new JObject
                {
                    { "Designation", row.Cells["이름"].Value?.ToString() },
                    { "IsoCode", row.Cells["ISO"].Value?.ToString() }
                };
                exchange.Add(item);

                string time = row.Cells["Valid From"].Value == null ? "" : !DateTime.TryParse(row.Cells["Valid From"].Value.ToString(), out DateTime dt) ? row.Cells["Valid From"].Value.ToString() : dt.ToString("yyyy-MM-dd");
                item = new JObject
                {
                    { "IsoCode", row.Cells["ISO"].Value?.ToString() },
                    { "Exchange Rate", global.ConvertDouble(eur[row.Cells["Valid From"].Value])/Math.Round( global.ConvertDouble(row.Cells["환율"].Value), 4) },
                    { "Vaild from", time }
                };
                exchangeDetail.Add(item);

                if(row.Cells["ISO"].Value?.ToString()=="EUR")
                {
                    item = new JObject
                    {
                        { "Designation", "Korean (South) Won" },
                        { "IsoCode", "KRW" }
                    };
                    exchange.Add(item);

                    item = new JObject
                    {
                        { "IsoCode", "KRW" },
                        { "Exchange Rate",eur[row.Cells["Valid From"].Value]?.ToString() },
                        { "Vaild from", time}
                    };
                    exchangeDetail.Add(item);
                }
            }
            string err = null;
            JObject postData = new JObject
            {
                { "Data", exchange },
                { "ConfigurationGuid", global_iniLoad.GetConfig("ExchangeRate", "Import_Header") }
            };

            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);

            postData = new JObject
            {
                { "Data", exchangeDetail },
                { "ConfigurationGuid", global_iniLoad.GetConfig("ExchangeRate", "Import_Detail") }
            };

            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);            

            return err; 
        }
    }
}
