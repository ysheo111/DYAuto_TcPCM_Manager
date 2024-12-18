﻿using Newtonsoft.Json.Linq;
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
    public class CostFactor
    {
        public string Import(string className, string name, DataGridView dgv)
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/MasterData/Import";

            JArray category = new JArray();
            JArray categoryLabor = new JArray();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                JObject item = new JObject();

                bool nullCheck = false;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (row.Cells[col.Name].Value != null && !col.Name.ToLower().Contains("valid")) nullCheck = true;

                    if (col.Name.Contains("연간 작업 일수"))
                        item.Add(col.Name, global.ConvertDouble(row.Cells[col.Name].Value) * global.ConvertDouble(row.Cells["Shift 당 작업 시간"].Value) * global.ConvertDouble(row.Cells["Shift"].Value));
                    else if (col.Name.Contains("Labor"))
                    {
                        JObject addictionalItem = new JObject(item);
                        string categoryName = $"Siemens.TCPCM.MasterData.CostFactor.Common.LaborBurdenShift{System.Text.RegularExpressions.Regex.Match(col.Name, @"\d+").Value}";
                        addictionalItem.Add("Labor burden", row.Cells[col.Name].Value?.ToString());
                        addictionalItem.Add("구분", categoryName);
                        if (nullCheck) categoryLabor.Add(addictionalItem);
                    }
                    //else if (col.Name.Contains("지역") || col.Name.Contains("업종"))
                    //    item.Add("UniqueId", row.Cells[col.Name].Value?.ToString());
                    else
                        item.Add(col.Name, row.Cells[col.Name].Value?.ToString());
                }
                if (name == "공간 생산 비용")
                {
                    item.Add("결과"
                        , global.ConvertDouble(item["건축비"]) * global.ConvertDouble(item["건물점유비율"]) / 100 / global.ConvertDouble(item["건물상각년수"]) / 12 );
                    item.Add("건물상각년수(년)"
                        , $@"건축비 : {global.ConvertDouble(item["건축비"])} 건물점유비율 : {global.ConvertDouble(item["건물점유비율"])} 건물상각년수 : {global.ConvertDouble(item["건물상각년수"])} 계산수식 : 건축비 * 건물점유비율 / 건물상각년수 / 100 / 12");
                }

                if (nullCheck) category.Add(item);
            }
            string err = null;


            if (name == "작업일수")
            {
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", global_iniLoad.GetConfig(className, "작업일수") }
                }), err);

                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", global_iniLoad.GetConfig(className, "작업시간") }
                }), err);

                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", global_iniLoad.GetConfig(className, "Shift") }
                }), err);

            }
            else
            {
                JObject postData = new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", global_iniLoad.GetConfig(className, name.Replace(" ","")) }
                };
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            }


            if (name == "임률")
            {
                JObject postData = new JObject
                    {
                        { "Data", categoryLabor },
                        { "ConfigurationGuid", global_iniLoad.GetConfig(className, "LaborBurden") }
                    };
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            }


            return err;
        }
    }
}
