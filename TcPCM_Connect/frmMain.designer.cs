namespace TcPCM_Connect
{
    partial class frmMain
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.panelContenedorPrincipal = new System.Windows.Forms.Panel();
            this.panelContenedorForm = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.p_Header = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.p_Profile = new System.Windows.Forms.Panel();
            this.lb_name = new System.Windows.Forms.Label();
            this.p_Footer = new System.Windows.Forms.Panel();
            this.btnSalir = new System.Windows.Forms.PictureBox();
            this.p_Date = new System.Windows.Forms.Panel();
            this.lblHora = new System.Windows.Forms.Label();
            this.lbFecha = new System.Windows.Forms.Label();
            this.p_Manu = new System.Windows.Forms.Panel();
            this.btn_UserManage = new CustomControls.RJControls.SideButton();
            this.p_Master = new System.Windows.Forms.Panel();
            this.sideButton1 = new CustomControls.RJControls.SideButton();
            this.btn_Overheads = new CustomControls.RJControls.SideButton();
            this.btn_Category = new CustomControls.RJControls.SideButton();
            this.btn_Material = new CustomControls.RJControls.SideButton();
            this.btn_Exchange = new CustomControls.RJControls.SideButton();
            this.btn_Master = new CustomControls.RJControls.SideButton();
            this.btn_Dashboard = new CustomControls.RJControls.SideButton();
            this.rjTextBox1 = new CustomControls.RJControls.RJTextBox();
            this.PanelBarraTitulo = new System.Windows.Forms.Panel();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.btnNormal = new System.Windows.Forms.Button();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.lb_Title = new System.Windows.Forms.Label();
            this.btnMaximizar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.tmExpandirMenu = new System.Windows.Forms.Timer(this.components);
            this.tmContraerMenu = new System.Windows.Forms.Timer(this.components);
            this.tmFechaHora = new System.Windows.Forms.Timer(this.components);
            this.tip_logout = new System.Windows.Forms.ToolTip(this.components);
            this.panelContenedorPrincipal.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.p_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.p_Profile.SuspendLayout();
            this.p_Footer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).BeginInit();
            this.p_Date.SuspendLayout();
            this.p_Manu.SuspendLayout();
            this.p_Master.SuspendLayout();
            this.PanelBarraTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.SuspendLayout();
            // 
            // panelContenedorPrincipal
            // 
            this.panelContenedorPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.panelContenedorPrincipal.Controls.Add(this.panelContenedorForm);
            this.panelContenedorPrincipal.Controls.Add(this.panel1);
            this.panelContenedorPrincipal.Controls.Add(this.panelMenu);
            this.panelContenedorPrincipal.Controls.Add(this.PanelBarraTitulo);
            this.panelContenedorPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenedorPrincipal.Location = new System.Drawing.Point(0, 0);
            this.panelContenedorPrincipal.Name = "panelContenedorPrincipal";
            this.panelContenedorPrincipal.Size = new System.Drawing.Size(1137, 631);
            this.panelContenedorPrincipal.TabIndex = 0;
            // 
            // panelContenedorForm
            // 
            this.panelContenedorForm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelContenedorForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenedorForm.Location = new System.Drawing.Point(206, 46);
            this.panelContenedorForm.Name = "panelContenedorForm";
            this.panelContenedorForm.Size = new System.Drawing.Size(931, 556);
            this.panelContenedorForm.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(206, 602);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(931, 29);
            this.panel1.TabIndex = 5;
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.panelMenu.Controls.Add(this.p_Header);
            this.panelMenu.Controls.Add(this.p_Footer);
            this.panelMenu.Controls.Add(this.p_Manu);
            this.panelMenu.Controls.Add(this.rjTextBox1);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 46);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(206, 585);
            this.panelMenu.TabIndex = 2;
            // 
            // p_Header
            // 
            this.p_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.p_Header.Controls.Add(this.pictureBox1);
            this.p_Header.Controls.Add(this.p_Profile);
            this.p_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.p_Header.Location = new System.Drawing.Point(0, 0);
            this.p_Header.Name = "p_Header";
            this.p_Header.Size = new System.Drawing.Size(206, 102);
            this.p_Header.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TcPCM_Connect.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(4, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(195, 43);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // p_Profile
            // 
            this.p_Profile.Controls.Add(this.lb_name);
            this.p_Profile.ForeColor = System.Drawing.Color.DimGray;
            this.p_Profile.Location = new System.Drawing.Point(6, 56);
            this.p_Profile.Name = "p_Profile";
            this.p_Profile.Size = new System.Drawing.Size(195, 32);
            this.p_Profile.TabIndex = 14;
            // 
            // lb_name
            // 
            this.lb_name.AutoSize = true;
            this.lb_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_name.ForeColor = System.Drawing.Color.DimGray;
            this.lb_name.Location = new System.Drawing.Point(8, 8);
            this.lb_name.Name = "lb_name";
            this.lb_name.Size = new System.Drawing.Size(69, 17);
            this.lb_name.TabIndex = 5;
            this.lb_name.Text = "Nombres ";
            // 
            // p_Footer
            // 
            this.p_Footer.Controls.Add(this.btnSalir);
            this.p_Footer.Controls.Add(this.p_Date);
            this.p_Footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.p_Footer.Location = new System.Drawing.Point(0, 492);
            this.p_Footer.Name = "p_Footer";
            this.p_Footer.Size = new System.Drawing.Size(206, 93);
            this.p_Footer.TabIndex = 15;
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalir.Image = global::TcPCM_Connect.Properties.Resources.logout;
            this.btnSalir.Location = new System.Drawing.Point(1, 26);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(50, 51);
            this.btnSalir.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnSalir.TabIndex = 13;
            this.btnSalir.TabStop = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            this.btnSalir.MouseHover += new System.EventHandler(this.btnSalir_MouseHover);
            // 
            // p_Date
            // 
            this.p_Date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.p_Date.Controls.Add(this.lblHora);
            this.p_Date.Controls.Add(this.lbFecha);
            this.p_Date.Location = new System.Drawing.Point(44, 12);
            this.p_Date.Name = "p_Date";
            this.p_Date.Size = new System.Drawing.Size(152, 74);
            this.p_Date.TabIndex = 15;
            // 
            // lblHora
            // 
            this.lblHora.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHora.AutoSize = true;
            this.lblHora.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblHora.ForeColor = System.Drawing.Color.DimGray;
            this.lblHora.Location = new System.Drawing.Point(14, 14);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(103, 29);
            this.lblHora.TabIndex = 1;
            this.lblHora.Text = "21:49:45";
            // 
            // lbFecha
            // 
            this.lbFecha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbFecha.AutoSize = true;
            this.lbFecha.BackColor = System.Drawing.Color.Transparent;
            this.lbFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbFecha.ForeColor = System.Drawing.Color.DimGray;
            this.lbFecha.Location = new System.Drawing.Point(15, 48);
            this.lbFecha.Name = "lbFecha";
            this.lbFecha.Size = new System.Drawing.Size(124, 13);
            this.lbFecha.TabIndex = 4;
            this.lbFecha.Text = "2022년 8월 25일 목요일";
            // 
            // p_Manu
            // 
            this.p_Manu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.p_Manu.Controls.Add(this.btn_UserManage);
            this.p_Manu.Controls.Add(this.p_Master);
            this.p_Manu.Controls.Add(this.btn_Master);
            this.p_Manu.Controls.Add(this.btn_Dashboard);
            this.p_Manu.Location = new System.Drawing.Point(4, 107);
            this.p_Manu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.p_Manu.Name = "p_Manu";
            this.p_Manu.Size = new System.Drawing.Size(201, 380);
            this.p_Manu.TabIndex = 35;
            // 
            // btn_UserManage
            // 
            this.btn_UserManage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_UserManage.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(165)))), ((int)(((byte)(168)))));
            this.btn_UserManage.BorderRadius = 0;
            this.btn_UserManage.BorderSize = 3;
            this.btn_UserManage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_UserManage.FlatAppearance.BorderSize = 0;
            this.btn_UserManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.btn_UserManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_UserManage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_UserManage.ForeColor = System.Drawing.Color.DimGray;
            this.btn_UserManage.Image = global::TcPCM_Connect.Properties.Resources.master_user;
            this.btn_UserManage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_UserManage.Location = new System.Drawing.Point(0, 240);
            this.btn_UserManage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_UserManage.MinimumSize = new System.Drawing.Size(201, 37);
            this.btn_UserManage.Name = "btn_UserManage";
            this.btn_UserManage.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btn_UserManage.Size = new System.Drawing.Size(201, 37);
            this.btn_UserManage.TabIndex = 0;
            this.btn_UserManage.Text = "  사용자 관리";
            this.btn_UserManage.TextColor = System.Drawing.Color.DimGray;
            this.btn_UserManage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_UserManage.UseVisualStyleBackColor = false;
            this.btn_UserManage.Click += new System.EventHandler(this.btn_UserManage_Click);
            // 
            // p_Master
            // 
            this.p_Master.Controls.Add(this.sideButton1);
            this.p_Master.Controls.Add(this.btn_Overheads);
            this.p_Master.Controls.Add(this.btn_Category);
            this.p_Master.Controls.Add(this.btn_Material);
            this.p_Master.Controls.Add(this.btn_Exchange);
            this.p_Master.Dock = System.Windows.Forms.DockStyle.Top;
            this.p_Master.Location = new System.Drawing.Point(0, 74);
            this.p_Master.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.p_Master.Name = "p_Master";
            this.p_Master.Size = new System.Drawing.Size(201, 166);
            this.p_Master.TabIndex = 3;
            // 
            // sideButton1
            // 
            this.sideButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sideButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.sideButton1.BorderRadius = 0;
            this.sideButton1.BorderSize = 0;
            this.sideButton1.FlatAppearance.BorderSize = 0;
            this.sideButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.sideButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sideButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sideButton1.ForeColor = System.Drawing.Color.DimGray;
            this.sideButton1.Image = global::TcPCM_Connect.Properties.Resources.master_plant;
            this.sideButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sideButton1.Location = new System.Drawing.Point(3, 133);
            this.sideButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sideButton1.Name = "sideButton1";
            this.sideButton1.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.sideButton1.Size = new System.Drawing.Size(196, 27);
            this.sideButton1.TabIndex = 0;
            this.sideButton1.Text = "   설비";
            this.sideButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sideButton1.TextColor = System.Drawing.Color.DimGray;
            this.sideButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.sideButton1.UseVisualStyleBackColor = false;
            this.sideButton1.Click += new System.EventHandler(this.sideButton1_Click);
            // 
            // btn_Overheads
            // 
            this.btn_Overheads.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Overheads.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.btn_Overheads.BorderRadius = 0;
            this.btn_Overheads.BorderSize = 0;
            this.btn_Overheads.FlatAppearance.BorderSize = 0;
            this.btn_Overheads.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.btn_Overheads.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Overheads.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Overheads.ForeColor = System.Drawing.Color.DimGray;
            this.btn_Overheads.Image = global::TcPCM_Connect.Properties.Resources.master_overheads;
            this.btn_Overheads.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Overheads.Location = new System.Drawing.Point(3, 101);
            this.btn_Overheads.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Overheads.Name = "btn_Overheads";
            this.btn_Overheads.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btn_Overheads.Size = new System.Drawing.Size(196, 27);
            this.btn_Overheads.TabIndex = 4;
            this.btn_Overheads.Text = "   Overheads";
            this.btn_Overheads.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Overheads.TextColor = System.Drawing.Color.DimGray;
            this.btn_Overheads.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Overheads.UseVisualStyleBackColor = false;
            this.btn_Overheads.Click += new System.EventHandler(this.btn_Overheads_Click);
            // 
            // btn_Category
            // 
            this.btn_Category.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Category.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.btn_Category.BorderRadius = 0;
            this.btn_Category.BorderSize = 0;
            this.btn_Category.FlatAppearance.BorderSize = 0;
            this.btn_Category.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.btn_Category.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Category.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Category.ForeColor = System.Drawing.Color.DimGray;
            this.btn_Category.Image = global::TcPCM_Connect.Properties.Resources.master_factor;
            this.btn_Category.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Category.Location = new System.Drawing.Point(2, 5);
            this.btn_Category.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Category.Name = "btn_Category";
            this.btn_Category.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btn_Category.Size = new System.Drawing.Size(196, 27);
            this.btn_Category.TabIndex = 1;
            this.btn_Category.Text = "   Cost Factor";
            this.btn_Category.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Category.TextColor = System.Drawing.Color.DimGray;
            this.btn_Category.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Category.UseVisualStyleBackColor = false;
            this.btn_Category.Click += new System.EventHandler(this.btn_Category_Click);
            // 
            // btn_Material
            // 
            this.btn_Material.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Material.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.btn_Material.BorderRadius = 0;
            this.btn_Material.BorderSize = 0;
            this.btn_Material.FlatAppearance.BorderSize = 0;
            this.btn_Material.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.btn_Material.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Material.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Material.ForeColor = System.Drawing.Color.DimGray;
            this.btn_Material.Image = global::TcPCM_Connect.Properties.Resources.master_material;
            this.btn_Material.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Material.Location = new System.Drawing.Point(2, 69);
            this.btn_Material.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Material.Name = "btn_Material";
            this.btn_Material.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btn_Material.Size = new System.Drawing.Size(196, 27);
            this.btn_Material.TabIndex = 3;
            this.btn_Material.Text = "   재료";
            this.btn_Material.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Material.TextColor = System.Drawing.Color.DimGray;
            this.btn_Material.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Material.UseVisualStyleBackColor = false;
            this.btn_Material.Click += new System.EventHandler(this.btn_Material_Click);
            // 
            // btn_Exchange
            // 
            this.btn_Exchange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Exchange.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(61)))), ((int)(((byte)(92)))));
            this.btn_Exchange.BorderRadius = 0;
            this.btn_Exchange.BorderSize = 0;
            this.btn_Exchange.FlatAppearance.BorderSize = 0;
            this.btn_Exchange.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.btn_Exchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Exchange.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Exchange.ForeColor = System.Drawing.Color.DimGray;
            this.btn_Exchange.Image = global::TcPCM_Connect.Properties.Resources.master_exchange;
            this.btn_Exchange.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Exchange.Location = new System.Drawing.Point(3, 37);
            this.btn_Exchange.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Exchange.Name = "btn_Exchange";
            this.btn_Exchange.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btn_Exchange.Size = new System.Drawing.Size(196, 27);
            this.btn_Exchange.TabIndex = 2;
            this.btn_Exchange.Text = "   환율";
            this.btn_Exchange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Exchange.TextColor = System.Drawing.Color.DimGray;
            this.btn_Exchange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Exchange.UseVisualStyleBackColor = false;
            this.btn_Exchange.Click += new System.EventHandler(this.btn_Exchange_Click);
            // 
            // btn_Master
            // 
            this.btn_Master.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Master.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(165)))), ((int)(((byte)(168)))));
            this.btn_Master.BorderRadius = 0;
            this.btn_Master.BorderSize = 3;
            this.btn_Master.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_Master.FlatAppearance.BorderSize = 0;
            this.btn_Master.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.btn_Master.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Master.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Master.ForeColor = System.Drawing.Color.DimGray;
            this.btn_Master.Image = global::TcPCM_Connect.Properties.Resources.master;
            this.btn_Master.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Master.Location = new System.Drawing.Point(0, 37);
            this.btn_Master.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Master.MinimumSize = new System.Drawing.Size(201, 37);
            this.btn_Master.Name = "btn_Master";
            this.btn_Master.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btn_Master.Size = new System.Drawing.Size(201, 37);
            this.btn_Master.TabIndex = 2;
            this.btn_Master.Text = "  Master Data";
            this.btn_Master.TextColor = System.Drawing.Color.DimGray;
            this.btn_Master.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Master.UseVisualStyleBackColor = false;
            this.btn_Master.Click += new System.EventHandler(this.btn_Master_Click);
            // 
            // btn_Dashboard
            // 
            this.btn_Dashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Dashboard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(165)))), ((int)(((byte)(168)))));
            this.btn_Dashboard.BorderRadius = 0;
            this.btn_Dashboard.BorderSize = 3;
            this.btn_Dashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_Dashboard.FlatAppearance.BorderSize = 0;
            this.btn_Dashboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.btn_Dashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Dashboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Dashboard.ForeColor = System.Drawing.Color.DimGray;
            this.btn_Dashboard.Image = global::TcPCM_Connect.Properties.Resources.Dashboard;
            this.btn_Dashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Dashboard.Location = new System.Drawing.Point(0, 0);
            this.btn_Dashboard.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Dashboard.MinimumSize = new System.Drawing.Size(201, 37);
            this.btn_Dashboard.Name = "btn_Dashboard";
            this.btn_Dashboard.Size = new System.Drawing.Size(201, 37);
            this.btn_Dashboard.TabIndex = 1;
            this.btn_Dashboard.Text = "Cost Break Down";
            this.btn_Dashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Dashboard.TextColor = System.Drawing.Color.DimGray;
            this.btn_Dashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Dashboard.UseVisualStyleBackColor = false;
            this.btn_Dashboard.Click += new System.EventHandler(this.btn_CBD_Click);
            // 
            // rjTextBox1
            // 
            this.rjTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.rjTextBox1.BorderColor = System.Drawing.Color.DimGray;
            this.rjTextBox1.BorderFocusColor = System.Drawing.Color.HotPink;
            this.rjTextBox1.BorderRadius = 0;
            this.rjTextBox1.BorderSize = 1;
            this.rjTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjTextBox1.ForeColor = System.Drawing.Color.DimGray;
            this.rjTextBox1.Location = new System.Drawing.Point(6, 62);
            this.rjTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rjTextBox1.Multiline = false;
            this.rjTextBox1.Name = "rjTextBox1";
            this.rjTextBox1.Padding = new System.Windows.Forms.Padding(6);
            this.rjTextBox1.PasswordChar = false;
            this.rjTextBox1.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.rjTextBox1.PlaceholderText = "";
            this.rjTextBox1.ReadOnly = false;
            this.rjTextBox1.Size = new System.Drawing.Size(192, 29);
            this.rjTextBox1.TabIndex = 15;
            this.rjTextBox1.Texts = "";
            this.rjTextBox1.UnderlinedStyle = true;
            // 
            // PanelBarraTitulo
            // 
            this.PanelBarraTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(165)))), ((int)(((byte)(168)))));
            this.PanelBarraTitulo.Controls.Add(this.pictureBox8);
            this.PanelBarraTitulo.Controls.Add(this.btnNormal);
            this.PanelBarraTitulo.Controls.Add(this.btnMinimizar);
            this.PanelBarraTitulo.Controls.Add(this.lb_Title);
            this.PanelBarraTitulo.Controls.Add(this.btnMaximizar);
            this.PanelBarraTitulo.Controls.Add(this.btnCerrar);
            this.PanelBarraTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelBarraTitulo.Location = new System.Drawing.Point(0, 0);
            this.PanelBarraTitulo.Name = "PanelBarraTitulo";
            this.PanelBarraTitulo.Size = new System.Drawing.Size(1137, 46);
            this.PanelBarraTitulo.TabIndex = 1;
            this.PanelBarraTitulo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelBarraTitulo_MouseDown);
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = global::TcPCM_Connect.Properties.Resources.PerfectProCalc_1;
            this.pictureBox8.Location = new System.Drawing.Point(11, 11);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(33, 26);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox8.TabIndex = 5;
            this.pictureBox8.TabStop = false;
            // 
            // btnNormal
            // 
            this.btnNormal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNormal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNormal.FlatAppearance.BorderSize = 0;
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNormal.Image = global::TcPCM_Connect.Properties.Resources._8530587_window_restore_icon__3_;
            this.btnNormal.Location = new System.Drawing.Point(1056, 7);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(36, 36);
            this.btnNormal.TabIndex = 3;
            this.btnNormal.UseVisualStyleBackColor = true;
            this.btnNormal.Visible = false;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Image = global::TcPCM_Connect.Properties.Resources._8664917_window_minimize_icon__1_;
            this.btnMinimizar.Location = new System.Drawing.Point(1020, 7);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(36, 36);
            this.btnMinimizar.TabIndex = 2;
            this.btnMinimizar.UseVisualStyleBackColor = true;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMinimizar_Click);
            // 
            // lb_Title
            // 
            this.lb_Title.AutoSize = true;
            this.lb_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_Title.ForeColor = System.Drawing.Color.White;
            this.lb_Title.Location = new System.Drawing.Point(54, 14);
            this.lb_Title.Name = "lb_Title";
            this.lb_Title.Size = new System.Drawing.Size(153, 24);
            this.lb_Title.TabIndex = 4;
            this.lb_Title.Text = "TcPCM Manager";
            // 
            // btnMaximizar
            // 
            this.btnMaximizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaximizar.FlatAppearance.BorderSize = 0;
            this.btnMaximizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximizar.Image = global::TcPCM_Connect.Properties.Resources._8530693_window_maximize_icon;
            this.btnMaximizar.Location = new System.Drawing.Point(1056, 7);
            this.btnMaximizar.Name = "btnMaximizar";
            this.btnMaximizar.Size = new System.Drawing.Size(36, 36);
            this.btnMaximizar.TabIndex = 1;
            this.btnMaximizar.UseVisualStyleBackColor = true;
            this.btnMaximizar.Click += new System.EventHandler(this.btnMaximizar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Image = global::TcPCM_Connect.Properties.Resources._1564505_close_delete_exit_remove_icon;
            this.btnCerrar.Location = new System.Drawing.Point(1092, 7);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(36, 36);
            this.btnCerrar.TabIndex = 0;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // tmExpandirMenu
            // 
            this.tmExpandirMenu.Interval = 1;
            this.tmExpandirMenu.Tick += new System.EventHandler(this.tmExpandirMenu_Tick);
            // 
            // tmContraerMenu
            // 
            this.tmContraerMenu.Interval = 1;
            this.tmContraerMenu.Tick += new System.EventHandler(this.tmContraerMenu_Tick);
            // 
            // tmFechaHora
            // 
            this.tmFechaHora.Enabled = true;
            this.tmFechaHora.Tick += new System.EventHandler(this.tmFechaHora_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 631);
            this.Controls.Add(this.panelContenedorPrincipal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(793, 530);
            this.Name = "frmMain";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "\"\"\"";
            this.Load += new System.EventHandler(this.FormMenuPrincipal_Load);
            this.panelContenedorPrincipal.ResumeLayout(false);
            this.panelMenu.ResumeLayout(false);
            this.p_Header.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.p_Profile.ResumeLayout(false);
            this.p_Profile.PerformLayout();
            this.p_Footer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).EndInit();
            this.p_Date.ResumeLayout(false);
            this.p_Date.PerformLayout();
            this.p_Manu.ResumeLayout(false);
            this.p_Master.ResumeLayout(false);
            this.PanelBarraTitulo.ResumeLayout(false);
            this.PanelBarraTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelContenedorPrincipal;
        private System.Windows.Forms.Panel PanelBarraTitulo;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.Label lb_Title;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnMaximizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Panel panelContenedorForm;
        private System.Windows.Forms.Label lbFecha;
        private System.Windows.Forms.Label lblHora;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.PictureBox btnSalir;
        private System.Windows.Forms.Timer tmExpandirMenu;
        private System.Windows.Forms.Timer tmContraerMenu;
        private System.Windows.Forms.Timer tmFechaHora;
        private System.Windows.Forms.Label lb_name;
        private System.Windows.Forms.Panel p_Profile;
        private System.Windows.Forms.Panel p_Date;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel p_Header;
        private System.Windows.Forms.Panel p_Footer;
        private System.Windows.Forms.ToolTip tip_logout;
        private CustomControls.RJControls.RJTextBox rjTextBox1;
        private System.Windows.Forms.Panel p_Manu;
        private CustomControls.RJControls.SideButton btn_Dashboard;
        private CustomControls.RJControls.SideButton btn_Master;
        private CustomControls.RJControls.SideButton btn_UserManage;
        private System.Windows.Forms.Panel p_Master;
        private CustomControls.RJControls.SideButton btn_Exchange;
        private CustomControls.RJControls.SideButton btn_Material;
        private CustomControls.RJControls.SideButton btn_Category;
        private CustomControls.RJControls.SideButton btn_Overheads;
        private CustomControls.RJControls.SideButton sideButton1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

