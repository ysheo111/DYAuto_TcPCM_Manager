using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using TcPCM_Connect.xaml;
using TcPCM_Connect_Global;
using System.Runtime.InteropServices;

namespace TcPCM_Connect
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();

            this.TransparencyKey = Color.Turquoise;
            this.BackColor = Color.Turquoise;

        }

        #region Form 움직일 수 있게 하는 함수
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        public static string auth;

        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void frmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void frmLogin_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void uc_MouseDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void uc_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void uc_MouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            dragging = false;
        }
        #endregion

        UserControl2 uc = new UserControl2();

        private void frmLogin_Load(object sender, EventArgs e)
        {

#if DEBUG
            global_iniLoad.loadDBInfo(false);
            uc.txtUser.Text = "admin";
            uc.txtPass.Password = "admin";
#else
             global_iniLoad.loadDBInfo(true);
#endif

            elementHost1.Child = uc;

            //마우스로 폼 이동 가능하게 하는 이벤트
            uc.MouseDown += uc_MouseDown;
            uc.MouseMove += uc_MouseMove;            
            uc.MouseUp += uc_MouseUp;

            uc.signUp.PreviewMouseDown += SignUp;
            uc.btnClose.Click += Form_Close;
            uc.btnMinimize.Click += Form_Minimized;
            uc.btnLogin.Click += Form_Login;
            uc.txtUser.KeyDown += new System.Windows.Input.KeyEventHandler(Login_TextChange);
            uc.txtPass.KeyDown += new System.Windows.Input.KeyEventHandler(Login_TextChange);

        }

        public void Form_Minimized(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void Form_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SignUp(object sender, EventArgs e)
        {
            subfrmSignUp subfrmsignUp = new subfrmSignUp();
            subfrmsignUp.ShowDialog();
        }

        public void Login_TextChange(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) LoginCheck();
        }

        public void Form_Login(object sender, EventArgs e)
        {
            LoginCheck();
        }

        public void LoginCheck()
        {
            //labelDialogResult.Text = "Dialog Box Result";
            string info = UserDB.LoginCheck(uc.txtUser.Text, uc.txtPass.Password);

            if ((bool)(info?.Contains("관리자")))
            {
                CustomMessageBox.RJMessageBox.Show(info, "로그인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                uc.txtPass.Password = "";
            }
            else if(info?.Length > 0)
            {
                frmMain frmmain = new frmMain();
                frmmain.lastLogin = info.Split('\\')[0];
                frmmain.connectName = info.Split('\\')[1];
                global.loginID = uc.txtUser.Text;
                auth = info.Split('\\')[2];

                frmmain.Show(this);
                this.Hide();

                //메인폼에서 다시 로그인창으로 돌아올 때 초기 창과 동일하게 뜨도록 처리.
                uc.txtUser.Text = uc.txtPass.Password = "";
                uc.txtUser.Focus();
            }
            else
            {
                CustomMessageBox.RJMessageBox.Show("로그인에 실패하였습니다. 아이디와 비밀번호를 확인해주세요.", "로그인"
                    , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                uc.txtPass.Password = "";
            }
        }
    }
}
