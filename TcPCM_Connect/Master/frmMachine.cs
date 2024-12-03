using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class frmMachine : Form
    {
        public string className = "";
        public frmMachine()
        {
            InitializeComponent();
        }

        private void frmExchange_Load(object sender, EventArgs e)
        {
            dgv_Machine.AllowUserToAddRows = true;
            ColumnAdd();
            InjectionColumn();
            dgv_Machine.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            ExcelImport excel = new ExcelImport();
            string err = excel.LoadMasterData("Machine", dgv_Machine);

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Machine", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            try
            {
                Machine machine = new Machine();
                string err = machine.Import(dgv_Machine, cb_Classification.Texts);                

                if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "Machine", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "Machine", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "Machine", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Thread.Sleep(100);
            LoadingScreen.CloseSplashScreen();
        }

        private void btn_Configuration_Click(object sender, EventArgs e)
        {
            ConfigSetting config = new ConfigSetting();
            config.className = "Machine";
            config.Show();
        }

        private void ColumnAdd()
        {
            dgv_Machine.Columns.Add(MasterData.Machine.designation1, MasterData.Machine.designation1);
            //dgv_Machine.Columns.Add(MasterData.Machine.designation2, MasterData.Machine.designation2);
            if(cb_Classification.SelectedItem?.ToString() == "기타") dgv_Machine.Columns.Add(MasterData.Machine.process, MasterData.Machine.process);
            dgv_Machine.Columns.Add(MasterData.Machine.maxClampingForce, MasterData.Machine.maxClampingForce);
            //dgv_Machine.Columns.Add(MasterData.Machine.setup, MasterData.Machine.setup);

            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.Name = combo.HeaderText = MasterData.Machine.currency;
            combo.FlatStyle = FlatStyle.Flat;
            dgv_Machine.Columns.Add(combo);
            ((DataGridViewComboBoxColumn)dgv_Machine.Columns[MasterData.Machine.currency]).DataSource = global_DB.ListSelect("Select IsoCode as name From Currencies", 0);
            dgv_Machine.Columns[MasterData.Machine.currency].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);

            //if (cb_Classification.SelectedItem?.ToString() == "기타")  dgv_Machine.Columns.Add(MasterData.Machine.category, MasterData.Machine.category);

            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = calendar.HeaderText = MasterData.Machine.validFrom;
            dgv_Machine.Columns.Add(calendar);
            dgv_Machine.Columns[MasterData.Machine.validFrom].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);

            dgv_Machine.Columns.Add(MasterData.Machine.acquisition, MasterData.Machine.acquisition);
            dgv_Machine.Columns.Add(MasterData.Machine.region, MasterData.Machine.region);
            dgv_Machine.Columns.Add(MasterData.Machine.category, MasterData.Machine.category);
            dgv_Machine.Columns.Add(MasterData.Machine.maintance, MasterData.Machine.maintance);
            dgv_Machine.Columns.Add(MasterData.Machine.imputed, MasterData.Machine.imputed);
            dgv_Machine.Columns.Add(MasterData.Machine.space, MasterData.Machine.space);
            dgv_Machine.Columns.Add(MasterData.Machine.ratedPower, MasterData.Machine.ratedPower);
            dgv_Machine.Columns.Add(MasterData.Machine.poweUtiliation, MasterData.Machine.poweUtiliation);           
            dgv_Machine.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgv_Machine_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e, 2);
        }

        private void dgv_Machine_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_Machine_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cb_Classification_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_Machine.Rows.Clear();
            dgv_Machine.Columns.Clear();
            System.Windows.Forms.ComboBox combo = (System.Windows.Forms.ComboBox)sender;
            if (combo.SelectedIndex < 0) return;

            ColumnAdd();
            if (combo.SelectedItem?.ToString() == "다이캐스팅") DieCastingColumn();
            else if (combo.SelectedItem?.ToString() == "사출") InjectionColumn();
            else if (combo.SelectedItem?.ToString() == "프레스") PressColumn();
            dgv_Machine.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void DieCastingColumn()
        {
            dgv_Machine.Columns.Add(MasterData.Machine.openMold2, MasterData.Machine.openMold2);
            dgv_Machine.Columns.Add(MasterData.Machine.blowOutMold2, MasterData.Machine.blowOutMold2);
            dgv_Machine.Columns.Add(MasterData.Machine.closeSilder2, MasterData.Machine.closeSilder2);
            dgv_Machine.Columns.Add(MasterData.Machine.closeMold2, MasterData.Machine.closeMold2);
            dgv_Machine.Columns.Add(MasterData.Machine.fillMaterial2, MasterData.Machine.fillMaterial2);
            dgv_Machine.Columns.Add(MasterData.Machine.injectPartingAgent2, MasterData.Machine.injectPartingAgent2);
            dgv_Machine.Columns.Add(MasterData.Machine.openSilder2, MasterData.Machine.openSilder2);
            //dgv_Machine.Columns.Add(MasterData.Machine.openMold2, MasterData.Machine.openMold2);
            //dgv_Machine.Columns.Add(MasterData.Machine.removeCast2, MasterData.Machine.removeCast2);
            //dgv_Machine.Columns.Add(MasterData.Machine.retractEjector2, MasterData.Machine.retractEjector2);
            //dgv_Machine.Columns.Add(MasterData.Machine.resetEjector2, MasterData.Machine.resetEjector2);
            //dgv_Machine.Columns.Add(MasterData.Machine.shotTime2, MasterData.Machine.shotTime2);
        }

        private void InjectionColumn()
        {
            dgv_Machine.Columns.Add(MasterData.Machine.setupTime, MasterData.Machine.setupTime);
            dgv_Machine.Columns.Add(MasterData.Machine.dryRunningTime, MasterData.Machine.dryRunningTime);
            dgv_Machine.Columns.Add(MasterData.Machine.meltingPower, MasterData.Machine.meltingPower);
            dgv_Machine.Columns.Add(MasterData.Machine.movePlasticizingUnit, MasterData.Machine.movePlasticizingUnit);
        }

        private void PressColumn()
        {
            dgv_Machine.Columns.Add(MasterData.Machine.setupTime, MasterData.Machine.setupTime);
            dgv_Machine.Columns.Add(MasterData.Machine.Foaming, MasterData.Machine.Foaming);
            dgv_Machine.Columns.Add(MasterData.Machine.Drawing, MasterData.Machine.Drawing);
            dgv_Machine.Columns.Add(MasterData.Machine.SPM, MasterData.Machine.SPM);
        }
    }
}
