using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;

namespace TcPCM_Connect_Global
{
    class WebAPI
    {
        public static void GetAauthorisationToken()
        {///api/v1/Account/Logout
            try
            {
                String callUrl = $"{global.serverURL}/{global.serverURLPath}/token";

                //callUrl = "https://pcm/tcpcm/token";

                JObject getData = new JObject();
                getData.Add("grant_type", "password");
                getData.Add("authenticatorId", "Siemens.TCPCM.AuthenticationMethod.TcPCM");
                getData.Add("client_id", "TcPCM");
                getData.Add("client_secret", "A980B6F5-A7F1-4F0B-A3EE-BAAF575CA912");
                getData.Add("username", global.userID);
                getData.Add("password", global.password);

                List<string> postData = new List<string>();
                foreach (var test in getData)
                {
                    postData.Add(string.Format("{0}={1}", test.Key, test.Value));
                }

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl);
                // 인코딩 UTF-8
                byte[] sendData = UTF8Encoding.UTF8.GetBytes(string.Join("&", postData));
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "Post";
                httpWebRequest.ContentLength = sendData.Length;
                httpWebRequest.Accept = "*/*";

                //Console.WriteLine(getData.ToString());
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(sendData, 0, sendData.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                JObject data = new JObject();
                using (var reader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    data = JObject.Parse(reader.ReadToEnd());
                }

                global.authorisationToken = "Bearer " + data["access_token"];
                global.refreshTokenId = data["refresh_token"].ToString();
                httpWebResponse.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("error : " + e.Response);
            }


        }

        public static void LogOut()
        {
            try
            {
                String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Account/Logout?refreshTokenId={global.refreshTokenId}";

                //callUrl = "https://pcm/tcpcm/token";


                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl);
                // 인코딩 UTF-8
                //byte[] sendData = UTF8Encoding.UTF8.GetBytes(getData.ToString());
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "Get";
                httpWebRequest.Accept = "*/*";
                httpWebRequest.Headers["Authorization"] = global.authorisationToken;

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                JObject data = new JObject();

                httpWebResponse.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("error : " + e.Response);
            }
        }

        public static string POST(string callUrl, JObject postData)
        {
            GetAauthorisationToken();
            string result = "";
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl);
                // 인코딩 UTF-8
                byte[] sendData = UTF8Encoding.UTF8.GetBytes(postData.ToString());
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = sendData.Length;
                httpWebRequest.Accept = "*/*";
                httpWebRequest.Headers["Authorization"] = global.authorisationToken;
                httpWebRequest.ContentType = "application/json";

                try
                {
                    Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(sendData, 0, sendData.Length);
                    requestStream.Close();
                    using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (var reader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException e)
                {
                    // This is the line that gets you the response object
                    using (HttpWebResponse httpWebResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }            
            }
            catch (WebException e)
            {
                result = "다음과 같은 에러가 발생하였습니다. 관리자에게 문의 하십시오. \n" + e;
                //Console.WriteLine(postData.ToString());
            }
            LogOut();
            return result;
        }

        public static JObject QueryPost(string callUrl, JObject postData)
        {
            JObject result = new JObject();

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl);
                // 인코딩 UTF-8
                byte[] sendData = UTF8Encoding.UTF8.GetBytes(postData.ToString());
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = sendData.Length;
                httpWebRequest.ContentType= "application/json";
                httpWebRequest.Accept = "*/*";

                //Console.WriteLine("Data List");

                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(sendData, 0, sendData.Length);
                    requestStream.Close();
                    using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (var reader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                        {
                            string s = reader.ReadToEnd();
                            result = JObject.Parse(s);
                        }
                    }
                }
                catch(WebException e)
                {
                    try
                    {
                        // This is the line that gets you the response object
                        using (HttpWebResponse httpWebResponse = (HttpWebResponse)e.Response)
                        {
                            using (var reader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                            {
                                string s = reader.ReadToEnd();
                                result = JObject.Parse(s);
                            }
                        }
                    }
                    catch(Exception ed)
                    {
                        result.Add("error", $"DB연결 오류. 관리자에게 문의 하십시오. URL : {callUrl}");
                    }

                }
            }
            catch (WebException e)
            {
                result.Add("error", "다음과 같은 에러가 발생하였습니다. 관리자에게 문의 하십시오. \n" + e);
                //Console.WriteLine(postData.ToString());
            }

            return result;
        }

        public static string ErrorCheck(string apiResult, string prevErr)
        {
            string err = null;
            if (apiResult?.Length <= 0) err = "데이터가 없습니다.";
            else if (apiResult.Contains("다음과 같은 에러가 발생하였습니다.")) err = apiResult;
            else
            {
                JObject r = JObject.Parse(apiResult);
                if (r["success"]?.ToString().ToLower().Contains("true")!=true) err = r["message"]?.ToString();                
            }
            if (err != null) err = prevErr == null ? err : $"{prevErr}\n{err}";

            return err;
        }
    }
}
