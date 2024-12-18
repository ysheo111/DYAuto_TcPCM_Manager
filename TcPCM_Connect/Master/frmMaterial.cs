﻿using CustomControls.RJControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class frmMaterial : Form
    {
        public string className = "";
        public frmMaterial()
        {
            InitializeComponent();
        }

        private void frmExchange_Load(object sender, EventArgs e)
        {
            dgv_Material.AllowUserToAddRows= true;
            Material();
            InjectionColumn();
            dgv_Material.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            ExcelImport excel = new ExcelImport();            
            string err = excel.LoadMasterData(cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString(),dgv_Material);
           
            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Material",MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            try
            {
                dgv_Material.AllowUserToAddRows = false;
                Material material = new Material();
                string err = material.Import(cb_Classification.SelectedItem == null ? "사출" : cb_Classification.SelectedItem.ToString(), dgv_Material);

                if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dgv_Material.AllowUserToAddRows = true;
            LoadingScreen.CloseSplashScreen();
        }

        private void btn_Configuration_Click(object sender, EventArgs e)
        {
            ConfigSetting config = new ConfigSetting();
            config.className = "Material' or Class = 'Substance";
            config.Show();
        }

        private void cb_Classification_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox combo = (System.Windows.Forms.ComboBox)sender;
            if (combo.SelectedIndex < 0) return;

            Material();
            if (combo.SelectedItem?.ToString()== "다이캐스팅") DieCastingColumn();
            else if(combo.SelectedItem?.ToString()== "사출") InjectionColumn();
            else if(combo.SelectedItem?.ToString()== "프레스" || combo.SelectedItem?.ToString() == "코어") PlateColumn();

            dgv_Material.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;            
        }
        private void Material()
        {
            dgv_Material.Columns.Clear();

            foreach(string item in MasterData.Material.material)
            {
                if (item.Contains("Valid"))
                {
                    CalendarColumn calendar = new CalendarColumn();
                    calendar.Name = calendar.HeaderText = item;
                    dgv_Material.Columns.Add(calendar);
                    dgv_Material.Columns[item].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
                }
                else if (item.Contains("통화"))
                {
                    DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
                    combo.Name = combo.HeaderText = item;
                    combo.FlatStyle = FlatStyle.Flat;
                    dgv_Material.Columns.Add(combo);
                    ((DataGridViewComboBoxColumn)dgv_Material.Columns[item]).DataSource = global_DB.ListSelect("Select IsoCode as name From Currencies", 0);
                    dgv_Material.Columns[item].DefaultCellStyle.Padding = new Padding(0, 4, 0, 0);
                }
                else  dgv_Material.Columns.Add(item, item);
            }
        }

        private void DieCastingColumn()
        {
            dgv_Material.Columns.Add("DROSS 비용", "DROSS 비용");
            dgv_Material.Columns.Add("DROSS 비용 단위", "DROSS 비용 단위");
            dgv_Material.Columns.Add("주조 온도 최소(℃)", "주조 온도 최소(℃)");
            dgv_Material.Columns.Add("주조 온도 최대(℃)", "주조 온도 최대(℃)");
            dgv_Material.Columns.Add("T-factor", "T-factor");
        }

        private void InjectionColumn()
        {
            dgv_Material.Columns.Add("계열", "계열");
            dgv_Material.Columns.Add("탈형 온도(℃)", "탈형 온도(℃)");
            dgv_Material.Columns.Add("사출 온도(℃)", "사출 온도(℃)");
            dgv_Material.Columns.Add("금형 온도(℃)", "금형 온도(℃)");
            dgv_Material.Columns.Add("내부 탈형 압력 계수", "내부 탈형 압력 계수");
            dgv_Material.Columns.Add("열확산도(mm²/s)", "열확산도(mm²/s)");
        }

        private void PlateColumn()
        {
            dgv_Material.Columns.Add("인장 강도(N/mm²)", "인장 강도(N/mm²)");
            dgv_Material.Columns.Add("전단 강도(N/mm²)", "전단 강도(N/mm²)");
            dgv_Material.Columns.Add("생산 계수", "생산 계수");
        }

        private void dgv_Material_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            global.MasterDataValiding((DataGridView)sender, e);
        }

        private void dgv_Material_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CustomMessageBox.RJMessageBox.Show(global.dgv_Category_DataError((DataGridView)sender, e), "DataError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
