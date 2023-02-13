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
    public partial class subfrmSignUp : Form
    {
        public subfrmSignUp()
        {
            InitializeComponent();
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_SignUp_Click(object sender, EventArgs e)
        {
            //필수정보 내용이 없다면, 오류 메세지창을 띄움.
            if(txt_ID.Texts?.Length <= 0 && txt_Password.Texts?.Length <= 0 && txt_FirstName.Texts?.Length <= 0 && txt_LastName.Texts?.Length <= 0)
            {
                CustomMessageBox.RJMessageBox.Show("필수정보 누락으로 회원가입에 실패하였습니다. 다시 확인해주세요.", "회원가입"
                    , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            //회원가입 성공 여부 반환 후 상황에 맞는 메세지 박스를 띄움.
            if (UserDB.SignUp(txt_ID.Texts, txt_Password.Texts, txt_FirstName.Texts, txt_LastName.Texts))
            {
                CustomMessageBox.RJMessageBox.Show("회원가입이 완료되었습니다.", "회원가입");
                this.Close();
            }
            else
            {
                CustomMessageBox.RJMessageBox.Show("동일한 아이디가 기존에 존재합니다. 다시 확인해주세요.", "회원가입"
                        , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_Password.Texts = "";
            }
        }
    }
}
