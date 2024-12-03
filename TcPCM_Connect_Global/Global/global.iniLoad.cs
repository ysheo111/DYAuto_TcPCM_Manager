using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections;
using Microsoft.Office.Interop.Excel;
using Application = System.Windows.Forms.Application;

namespace TcPCM_Connect_Global
{
    public class global_iniLoad
    {
        #region ini 입력 메소드
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion

        /// <summary>
        /// DB와 Web Server 에 관련된 ini 값을 전역변수에 할당
        /// </summary>
        public static void loadDBInfo(bool check)
        {
            string filePath = Application.StartupPath + @"\dbinfo.ini";

            global.serverURL = GetIniValue("Server Info", "serverURL", filePath);
            global.port = GetIniValue("Server Info", "port", filePath);
            string admin =global_DB.ScalarExecute($"Select Concat(UserId,'|', Password) From TcPCM", (int)global_DB.connDB.selfDB);
            if (!admin.Contains('|'))
            {
                System.Windows.Forms.MessageBox.Show($"Error : {admin}");
                return;
            }
            global.userID = admin.Split('|')[0];
            global.password = admin.Split('|')[1];
        }

        /// <summary>
        /// ini에서 Guid를 불러오는 함수
        /// </summary>
        /// <param name="classification">ini 파일 내의 대제목</param>
        /// <param name="name">ini 파일 내의 소제목</param>
        /// <returns> 조건에 맞는 Guid </returns>
        public static string GetConfig(string classification, string name)
        {           
            return global_DB.ScalarExecute($"Select GUID From Configuration Where Class=N'{classification}' and Name=N'{name}'"
                , (int)global_DB.connDB.selfDB); ;
        }

        public static string GetIniValue(string Section, string Key, string iniPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, null, temp, 255, iniPath);
            return temp.ToString();
        }


    }
}
