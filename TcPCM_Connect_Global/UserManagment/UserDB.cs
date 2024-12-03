using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcPCM_Connect_Global
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDB
    {
        /// <summary>
        /// 로그인 정보 확인, 로그인 정보가 맞다면 마지막 로그인 정보를 업데이트
        /// </summary>
        /// <returns>마지막 로그인 정보, 이름, 권한을 반환</returns>
        public static string LoginCheck(string id, string pwd)
        {
            //만약 전달 받은 정보가 존재한다면 마지막 로그인 시간을 업데이트하고 회원정보를 반환
            string condition = $"ID = '{id}' And Password = dbo.fn_EncryptString('{pwd}')";
            string query = $@"If exists(Select * from UserInfo Where {condition})
                                begin
                                    UPDATE UserInfo SET LastHistory = GETDATE() Where {condition}
                                end
 
                                Select CONCAT(CONVERT(CHAR(19),LastHistory,20),'\', FirstName ,' ' ,LastName,'\',a.Authority)
                                from UserInfo as a
                                Where {condition}";

            string name = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);

            return name;
        }
        /// <summary>
        /// 중복 아이디를 확인하여 회원가입
        /// </summary>
        /// <returns>회원가입이 성공적이로 되었다면 true를 반환</returns>
        public static bool SignUp(string id, string pwd, string first, string last)
        {
            //만약 기존에 동일 아이디가 있다면 false를 반환 없다면 insert 후 true를 반환
            string query = $@"If exists(Select * from UserInfo Where ID = '{id}')
                                begin
                                    Select 'false'
                                end
                              else 
                                begin
                                    Insert Into UserInfo (Authority, ID, Password, FirstName, LastName) 
                                    Values(N'associate', N'{id}', dbo.fn_EncryptString(N'{pwd}'), N'{first}', N'{last}' )

                                    Select 'true'
                                end";

            string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.selfDB);

            return Convert.ToBoolean(result);
        }
    }
}
