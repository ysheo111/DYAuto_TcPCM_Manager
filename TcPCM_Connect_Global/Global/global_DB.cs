using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.IO;

namespace TcPCM_Connect_Global
{
    /// <summary>
    /// DB에 관련된 클래스
    /// </summary>
    public class global_DB
    {
        public enum connDB
        {
            PCMDB,
            selfDB
        }

        public enum exeDBMode
        {
            Scalar,
            ScalarDelete,
            Muti,
            List
        }
        /// <summary>
        /// 결과가 하나인 쿼리 실행 후 결과값을 반환
        /// </summary>
        /// <param name="query"></param>
        /// <param name="dbInfo"></param>
        /// <returns></returns>
        public static string ScalarExecute(string query, int dbInfo)
        {
            string callUrl = $"{global.serverURL}:{global.port}/cbd/Interface/Scalar";
            JObject post = new JObject
            {
                { "query", query },
                { "dbInfo", dbInfo }
            };
            try
            {
                JObject result = WebAPI.QueryPost(callUrl, post);
                if (result["error"]?.ToString().Length<=0) return result["data"]?.ToString();
                else return result["error"]?.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show($"조회를 실패하였습니다.${e.Message}");
            }

            return null;
        }

        /// <summary>
        /// 삭제를 실행 후 성공여부를 bool로 반환
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool ScalarDeleteBool(string query)
        {
            string totalQuery = $@"{query} 
                                IF @@ROWCOUNT <>1
                                 BEGIN
                                  RAISERROR ('An error occured',10,1)
                                  SELECT 'false'
                                 END
                                ELSE 
                                 BEGIN
                                  SELECT 'true'
                                 END";

            string callUrl = global.serverURL + ":8243/cbd/Interface/Delete";
            JObject post = new JObject
            {
                { "query", totalQuery },
                { "dbInfo", (int)connDB.selfDB }
            };

            try
            {
                JObject result = WebAPI.QueryPost(callUrl, post);
                if (result["error"]?.ToString().Length<=0) return Convert.ToBoolean(result["data"]?.ToString());
                else return false;
            }
            catch (Exception e)
            {
                MessageBox.Show($"조회를 실패하였습니다.${e.Message}");
            }

            return false;
        }
        /// <summary>
        ///  결과값이 테이블인 쿼리의 결과를 DataTable로 반환함.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="dbInfo"></param>
        /// <returns></returns>
        public static DataTable MutiSelect(string query, int dbInfo)
        {
            string callUrl = $"{global.serverURL}:{global.port}/cbd/Interface/Muti";
            JObject post = new JObject
            {
                { "query", query },
                { "dbInfo", dbInfo }
            };

            try
            {
                JObject result = WebAPI.QueryPost(callUrl, post);
                if (result["error"]?.ToString().Length <= 0 && result["data"]?.ToString().Length > 0)
                {
                    DataTable dt = new DataTable();
                    if (result["data"] is JArray)
                    {
                        JArray a = (JArray)result["data"];
                        foreach (var item in a)
                        {
                            Dictionary<object, object> pairs = item.ToObject<Dictionary<object, object>>();
                            DataRow dr = dt.NewRow();
                            foreach (var temp in pairs)
                            {
                                if (!dt.Columns.Contains(temp.Key?.ToString())) dt.Columns.Add(temp.Key?.ToString());
                                dr[temp.Key?.ToString()] = temp.Value;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataSet ds = new DataSet();
                        using (StringReader stringReader = new StringReader(result["data"]?.ToString()))
                        {
                            ds = new DataSet();
                            ds.ReadXml(stringReader);
                        }
                        dt = ds.Tables[0];
                    }                   

                    return dt;
                }
                else
                {
                    MessageBox.Show($"조회를 실패하였습니다.${result["error"]}");
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"조회를 실패하였습니다.${e.Message}");
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="dbInfo"></param>
        /// <returns></returns>
        public static List<string> ListSelect(string query, int dbInfo)
        {
            string callUrl = $"{global.serverURL}:{global.port}/cbd/Interface/List";
            JObject post = new JObject
            {
                { "query", query },
                { "dbInfo", dbInfo }
            };

            try
            {
                JObject result = WebAPI.QueryPost(callUrl, post);
                if (result["error"]?.ToString().Length <= 0 && result["data"]?.ToString().Length > 0)
                {
                    JArray a = (JArray)result["data"];
                    List<string> person = a.ToObject<List<string>>();
                    return person;
                }
                else return null;
            }
            catch (Exception e)
            {
                MessageBox.Show($"조회를 실패하였습니다.${e.Message}");
            }

            return null;
        }
    }
}
