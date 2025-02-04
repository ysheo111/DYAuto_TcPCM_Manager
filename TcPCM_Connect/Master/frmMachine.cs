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
        private void btn_Shift_Click(object sender, EventArgs e)
        {
            Component.ShiftSetting shift = new Component.ShiftSetting();
            shift.Show();
        }

        private void ColumnAdd()
        {
            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = calendar.HeaderText = MasterData.Machine.validFrom;
            dgv_Machine.Columns.Add(calendar);
            dgv_Machine.Columns[MasterData.Machine.validFrom].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
            
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.Name = combo.HeaderText = MasterData.Machine.currency;
            combo.FlatStyle = FlatStyle.Flat;
            dgv_Machine.Columns.Add(combo);
            ((DataGridViewComboBoxColumn)dgv_Machine.Columns[MasterData.Machine.currency]).DataSource = global_DB.ListSelect("Select IsoCode as name From Currencies", 0);
            dgv_Machine.Columns[MasterData.Machine.currency].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);

            DataGridViewComboBoxColumn segCombo = new DataGridViewComboBoxColumn();
            segCombo.Name = segCombo.HeaderText = MasterData.Machine.segment;
            segCombo.FlatStyle = FlatStyle.Flat;
            dgv_Machine.Columns.Add(segCombo);
            ((DataGridViewComboBoxColumn)dgv_Machine.Columns[MasterData.Machine.segment]).DataSource = global_DB.ListSelect("SELECT DISTINCT UniqueKey as name FROM BDSegments WHERE UniqueKey LIKE '%[^0-9]%'", 0);
            dgv_Machine.Columns[MasterData.Machine.segment].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);

            //dgv_Machine.Columns.Add(MasterData.Machine.segment, MasterData.Machine.segment);

            dgv_Machine.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgv_Machine_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e, 2);
            if (!dgv_Machine.Columns[e.ColumnIndex].Name.Contains("Valid")) return;

            DataGridViewRow row = dgv_Machine.Rows[e.RowIndex];

            if (row.Cells[e.ColumnIndex].Value == null) return;
            row.Cells[e.ColumnIndex].Value = !DateTime.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out DateTime dt) ?
                row.Cells[e.ColumnIndex].Value : dt.ToString("yyyy-MM-dd");
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
            if (combo.SelectedItem?.ToString() == "설비") Capitalcolumn();
            dgv_Machine.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void InjectionColumn()
        {
            dgv_Machine.Columns.Add(MasterData.Machine.setupTime, MasterData.Machine.setupTime);
            dgv_Machine.Columns.Add(MasterData.Machine.dryRunningTime, MasterData.Machine.dryRunningTime);
            dgv_Machine.Columns.Add(MasterData.Machine.meltingPower, MasterData.Machine.meltingPower);
            dgv_Machine.Columns.Add(MasterData.Machine.movePlasticizingUnit, MasterData.Machine.movePlasticizingUnit);
        }

        private void Capitalcolumn()
        {
            dgv_Machine.Columns.Add(MasterData.Machine.customer, "업체명");
            dgv_Machine.Columns.Add(MasterData.Machine.designation1, MasterData.Machine.designation1);
            dgv_Machine.Columns.Add(MasterData.Machine.maxClampingForce, MasterData.Machine.maxClampingForce);
            dgv_Machine.Columns.Add(MasterData.Machine.maker, MasterData.Machine.maker);
            dgv_Machine.Columns.Add(MasterData.Machine.manufacturer, MasterData.Machine.manufacturer);
            dgv_Machine.Columns.Add(MasterData.Machine.acquisition, MasterData.Machine.acquisition);
            dgv_Machine.Columns[MasterData.Machine.acquisition].DefaultCellStyle.Format = "N2";
            dgv_Machine.Columns.Add(MasterData.Machine.imputed, MasterData.Machine.imputed);
            dgv_Machine.Columns.Add(MasterData.Machine.space, MasterData.Machine.space);
            dgv_Machine.Columns.Add(MasterData.Machine.ratedPower, MasterData.Machine.ratedPower);
            dgv_Machine.Columns.Add(MasterData.Machine.poweUtiliation, MasterData.Machine.poweUtiliation);
            dgv_Machine.Columns.Add(MasterData.Machine.acquisitionEx, MasterData.Machine.acquisitionEx);
            dgv_Machine.Columns[MasterData.Machine.acquisitionEx].DefaultCellStyle.Format = "N2";
            dgv_Machine.Columns.Add("내용년수", "기타 내용년수");
        }
    }
}
