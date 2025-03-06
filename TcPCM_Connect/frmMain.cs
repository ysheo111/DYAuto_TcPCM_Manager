using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcPCM_Connect_Global;
using System.Threading;
using System.IO;
using Point = System.Drawing.Point;

namespace TcPCM_Connect
{
    public partial class frmMain : Form
    {
        string privateFolder;
        public string connectName = "", lastLogin = "";
        //Constructor
        public frmMain()
        {
            InitializeComponent();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
        }
        private void FormMenuPrincipal_Load(object sender, EventArgs e)
        {
            lb_name.Text = connectName;

            if (!frmLogin.auth.ToLower().Contains("admin"))
            {
                btn_Master.Visible = false;
                p_Master.Visible = false;
                btn_UserManage.Visible = false;
            }
            global_iniLoad.loadDBInfo(true);
            //btn_Dashboard.PerformClick();   
            btn_Dashboard.PerformClick();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            //CustomMessageBox.RJMessageBox.Show("정말 종료 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (CustomMessageBox.RJMessageBox.Show("정말 로그아웃 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmLogin p = (frmLogin)this.Owner;
                p.Show();
                this.Close();
            }
        }

        private void btnSalir_MouseHover(object sender, EventArgs e)
        {
            this.tip_logout.SetToolTip(this.btnSalir, "로그아웃 버튼 입니다.");
        }

        private void btn_CBD_Click(object sender, EventArgs e )
        {
            ChangeButtonColor(((Button)sender).Name, p_Manu);

            frmDashboard frm = new frmDashboard();

            string query = $"Select PrivateFolderId From UserSettings as a Inner Join Users as b on a.UserId = b.Id Where LogonName = '{frmLogin.auth}'";
            privateFolder = global_DB.ScalarExecute(query, (int)global_DB.connDB.PCMDB);

            frm.privateFolder = privateFolder;
            frmApply(frm, ref panelContenedorForm);
        }

        private void btn_Category_Click(object sender, EventArgs e)
        {
            ChangeButtonColor("btn_Master", p_Manu);
            ChangeButtonColor(((Button)sender).Name, p_Master);

            frmCategory frm = new frmCategory();
            frmApply(frm, ref panelContenedorForm);
        }

        private void btn_Exchange_Click(object sender, EventArgs e)
        {
            ChangeButtonColor("btn_Master", p_Manu);
            ChangeButtonColor(((Button)sender).Name, p_Master);

            frmExchange frm = new frmExchange();
            frmApply(frm, ref panelContenedorForm);
        }

        private void btn_Material_Click(object sender, EventArgs e)
        {
            ChangeButtonColor("btn_Master",p_Manu);
            ChangeButtonColor(((Button)sender).Name, p_Master);

            frmMaterial frm = new frmMaterial();
            frmApply(frm, ref panelContenedorForm);
        }

        private void btn_Overheads_Click(object sender, EventArgs e)
        {
            ChangeButtonColor("btn_Master", p_Manu);
            ChangeButtonColor(((Button)sender).Name, p_Master);

            frmOverheads frm = new frmOverheads();
            frmApply(frm, ref panelContenedorForm);
        }

        private void sideButton1_Click(object sender, EventArgs e)
        {
            ChangeButtonColor("btn_Master", p_Manu);
            ChangeButtonColor(((Button)sender).Name, p_Master);

            frmMachine frm = new frmMachine();
            frmApply(frm, ref panelContenedorForm);
        }

        private void btn_UserManage_Click(object sender, EventArgs e)
        {
            ChangeButtonColor(((Button)sender).Name, p_Manu);

            frmUserManagment frmuserManagment = new frmUserManagment();
            frmApply(frmuserManagment, ref panelContenedorForm);
        }

        private void sideButton3_Click(object sender, EventArgs e)
        {
            ChangeButtonColor("btn_Master", p_Manu);
            ChangeButtonColor(((Button)sender).Name, p_Master);

            frmVendorPart frm = new frmVendorPart();
            frmApply(frm, ref panelContenedorForm);
        }
        private void btn_Master_Click(object sender, EventArgs e)
        {
            p_Master.Visible = !p_Master.Visible;
        }

        public void ChangeButtonColor(string name, Panel p)
        {
            foreach (Control ctrl in p.Controls)
            {
                if (!(ctrl is CustomControls.RJControls.SideButton))
                {
                    ChangeButtonColor("", (Panel)ctrl);
                }
                else
                {
                    if (ctrl.Name == name) ctrl.BackColor = Color.LightGray;
                    else ctrl.BackColor = Color.FromArgb(235, 235, 235);
                }
            }
        }

        public void frmApply(Form frm, ref Panel p_MainForm)
        {
            p_MainForm.Controls.Clear();
            frm.TopLevel = false;
            frm.TopMost = true;
            p_MainForm.Controls.Add(frm);
            frm.Dock = DockStyle.Fill;
            frm.Show();
            frm.Activate();
        }

        #region 화면 이동 가능
        //METODO PARA REDIMENCIONAR/CAMBIAR TAMAÑO A FORMULARIO  TIEMPO DE EJECUCION ----------------------------------------------------------
        private int tolerance = 15;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));

            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);

            region.Exclude(sizeGripRectangle);
            this.panelContenedorPrincipal.Region = region;
            this.Invalidate();
        }
        //----------------COLOR Y GRIP DE RECTANGULO INFERIOR
        protected override void OnPaint(PaintEventArgs e)
        {

            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(55, 61, 69));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);

            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }

        //METODO PARA ARRASTRAR EL FORMULARIO---------------------------------------------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //METODO PARA HORA Y FECHA ACTUAL ----------------------------------------------------------
        private void tmFechaHora_Tick(object sender, EventArgs e)
        {
            lbFecha.Text = DateTime.Now.ToLongDateString();
            lblHora.Text = DateTime.Now.ToString("HH:mm:ssss");
        }

        //METODOS PARA ANIMACION DE MENU SLIDING--
        private void btnMenu_Click(object sender, EventArgs e)
        {
            //-------CON EFECTO SLIDING
            if (panelMenu.Width > 60) this.tmContraerMenu.Start();
            else this.tmExpandirMenu.Start();
        }

        private void tmExpandirMenu_Tick(object sender, EventArgs e)
        {
            if (panelMenu.Width >= 235)
            {
                this.tmExpandirMenu.Stop();
                //p_Header.BackColor = Color.White;
            }
            else panelMenu.Width = panelMenu.Width + 5;

        }

        private void tmContraerMenu_Tick(object sender, EventArgs e)
        {
            if (panelMenu.Width <= 60)
            {
                this.tmContraerMenu.Stop();
                //p_Header.BackColor = Color.White;
            }
            else panelMenu.Width = panelMenu.Width - 5;
        }

        //METODO PARA ABRIR FORM DENTRO DE PANEL-----------------------------------------------------
        private void AbrirFormEnPanel(object formHijo)
        {
            if (this.panelContenedorForm.Controls.Count > 0)
                this.panelContenedorForm.Controls.RemoveAt(0);
            Form fh = formHijo as Form;
            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            this.panelContenedorForm.Controls.Add(fh);
            this.panelContenedorForm.Tag = fh;
            fh.Show();
        }

        //METODOS PARA CERRAR,MAXIMIZAR, MINIMIZAR FORMULARIO------------------------------------------------------
        int lx, ly;
        int sw, sh;
        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            btnMaximizar.Visible = false;
            btnNormal.Visible = true;

        }

        private void sideButton2_Click(object sender, EventArgs e)
        {
            ChangeButtonColor("btn_Master", p_Manu);
            ChangeButtonColor(((Button)sender).Name, p_Master);

            frmPartManufacturing frm = new frmPartManufacturing();
            frmApply(frm, ref panelContenedorForm);
        }


        private void btnNormal_Click(object sender, EventArgs e)
        {
            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
            btnNormal.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            //CustomMessageBox.RJMessageBox.Show("정말 종료 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (CustomMessageBox.RJMessageBox.Show("정말 종료 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        #endregion
    }
}
