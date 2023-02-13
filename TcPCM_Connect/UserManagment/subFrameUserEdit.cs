using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class subFrameUserEdit : Form
    {
        public int Idx;
        public string id, fristName, lastName, auth;

        public subFrameUserEdit()
        {
            InitializeComponent();
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void subFrameUserEdit_Load(object sender, EventArgs e)
        {
            //콤보박스에 권한 리스트 추가
            string comboQuery = $"Select [LogonName] as name from [Users] Where LogonName != 'System'";

            foreach(string list in global_DB.ListSelect(comboQuery, (int)global_DB.connDB.PCMDB))
            {
                cb_Auth.Items.Add(list);
            }

            txt_ID.Texts = id;
            txt_FirstName.Texts = fristName;
            txt_LastName.Texts = lastName;
            cb_Auth.Texts = auth;
        }

        private void btn_SignUp_Click(object sender, EventArgs e)
        {
            string comboQuery = $@"UPDATE UserInfo
                                SET FirstName = N'{txt_FirstName.Texts}'
                                    , LastName = N'{txt_LastName.Texts}'";

            if (txt_Password.Texts?.Length > 0) comboQuery += $", Password =  dbo.fn_EncryptString('{txt_Password.Texts}') ";
            
            comboQuery += $@", UserInfo.Authority = '{cb_Auth.Texts}'
                            FROM UserInfo as a WHERE a.Idx={Idx} ";

            global_DB.ScalarExecute(comboQuery, (int)global_DB.connDB.selfDB);

            CustomMessageBox.RJMessageBox.Show("수정이 완료 되었습니다.", "회원 정보 수정" , MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
