using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using TcPCM_Connect_Global;
using Application = System.Windows.Forms.Application;
using Excel = Microsoft.Office.Interop.Excel;

namespace TcPCM_Connect
{
    public partial class frmCalculator : Form
    {
       
        public frmCalculator()
        {
            InitializeComponent();
        }        

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            // 첫 번째 표: 포장사양
            dgv_package1.Columns.Add("Column1", "종류");
            dgv_package1.Columns[0].ReadOnly = true;
            dgv_package1.Columns.Add("Column2", "수량");
            dgv_package1.Rows.Add("나라", "");
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();

            // 콤보박스 셀에 항목 추가
            string query = $@"SELECT Distinct [나라] as Name FROM [Transport]";
            List<string> comboBoxItems = global_DB.ListSelect(query, (int)global_DB.connDB.selfDB);
            comboBoxCell.Items.AddRange(comboBoxItems.ToArray());            
            dgv_package1.Rows[0].Cells[1] = comboBoxCell;
            dgv_package1.Rows[0].Cells[1].Value = comboBoxCell.Items[0];

            dgv_package1.Rows.Add("포장사양", "");
            DataGridViewComboBoxCell comboBoxCell2 = new DataGridViewComboBoxCell();
            List<string> comboBoxItems2 = new List<string>() { "Box", "Rack" };
            comboBoxCell2.Items.AddRange(comboBoxItems2.ToArray());
            dgv_package1.Rows[1].Cells[1] = comboBoxCell2;
            dgv_package1.Rows[1].Cells[1].Value = comboBoxCell2.Items[0];

            dgv_package1.Rows.Add("공법", "");
            DataGridViewComboBoxCell comboBoxCell3 = new DataGridViewComboBoxCell();
            List<string> comboBoxItems3 = new List<string>() { "Injection", "Press", "etc" };
            comboBoxCell3.Items.AddRange(comboBoxItems3.ToArray());
            dgv_package1.Rows[2].Cells[1] = comboBoxCell3;
            dgv_package1.Rows[2].Cells[1].Value = comboBoxCell3.Items[0];

            dgv_package1.Rows.Add("대당수량", "");
            dgv_package1.Rows.Add("Total Vol.", "");
            dgv_package1.Rows.Add("Model life", "");
            dgv_package1.Rows.Add("Poly box", "");
            dgv_package1.Rows.Add("Pallet", "");
            dgv_package1.Rows[dgv_package1.Rows.Count - 1].Cells[1].Value = 1;
            //dgv_package1.Rows.Add("제품수/용기", "");
            dgv_package1.Rows.Add("제품수/box", "");
            //dgv_package1.Rows.Add("용기/pallet", "");
            dgv_package1.Rows.Add("Steel rack", "");
            dgv_package1.Rows[dgv_package1.Rows.Count - 1].Cells[1].Value = 1;
            dgv_package1.Rows.Add("제품수/rack", "");

            dgv_master.Columns.Add("Column1", "종류");
            dgv_master.Columns[0].ReadOnly = true;
            dgv_master.Columns.Add("Column2", "수량");
            dgv_master.Rows.Add("box 가격", "");
            dgv_master.Rows.Add("pallet 가격", "");
            dgv_master.Rows.Add("rack 가격", "");
            dgv_master.Rows.Add("추가포장비 (비닐백) ", "");

            // 두 번째 표: 포장비
            dgv_package2.Columns.Add("Column1", "항목");
            dgv_package2.Columns[0].ReadOnly = true;
            dgv_package2.Columns.Add("Column2", "수량");
            dgv_package2.Rows.Add("협력사재고 (일)", "");
            dgv_package2.Rows[dgv_package2.Rows.Count-1].Cells[1].Value = 6;
            dgv_package2.Rows.Add("LG재고 (일)", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Value = 3;
            dgv_package2.Rows.Add("운송용 BOX 수 (일)", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Value = 4;
            dgv_package2.Rows.Add("일일 제품 사용량", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("일일 필요 박스 수량", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("총 필요 박스 수량", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("연간 박스 회전율", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("박스 사용 횟수", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Value = 20;
            dgv_package2.Rows.Add("박스 재 재작횟수", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("총 제작 박스수량", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("일일 필요 Pallet 수량", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("Pallet 사용 횟수", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Value = 10;
            dgv_package2.Rows.Add("총 필요 Pallet 수량", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("제품당 포장비", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("추가포장비(비닐,집게등)", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package2.Rows.Add("최종 포장비/ea", "");
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].ReadOnly = true;
            dgv_package2.Rows[dgv_package2.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;

            // 두 번째 표: 포장비
            dgv_package3.Columns.Add("Column1", "항목");
            dgv_package3.Columns[0].ReadOnly = true;
            dgv_package3.Columns.Add("Column2", "수량");
            dgv_package3.Rows.Add("협력사재고 (일)", "");
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].Cells[1].Value = 6;
            dgv_package3.Rows.Add("LG재고 (일)", "");
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].Cells[1].Value = 3;
            dgv_package3.Rows.Add("운송용 Rack 수 (일)", "");
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].Cells[1].Value = 4;
            dgv_package3.Rows.Add("일일 제품 사용량", "");
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].ReadOnly = true;
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package3.Rows.Add("일일 필요 Rack 수량", "");
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].ReadOnly = true;
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package3.Rows.Add("총 필요 Rack 수량", "");
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].ReadOnly = true;
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package3.Rows.Add("제품당 포장비", "");
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].ReadOnly = true;
            dgv_package3.Rows[dgv_package3.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;

            // 세 번째 표: 운송비
            dgv_transport.Columns.Add("Column1", "항목");
            dgv_transport.Columns[0].ReadOnly = true;
            dgv_transport.Columns.Add("Column2", "수량");
            dgv_transport.Rows.Add("차종 (ton)", "");
            DataGridViewComboBoxCell comboBoxCell4 = new DataGridViewComboBoxCell();
            dgv_transport.Rows[0].Cells[1] = comboBoxCell4;
            dgv_transport.Rows.Add("편도거리 (km)", "");
            dgv_transport.Rows[dgv_transport.Rows.Count - 1].Cells[1].Value = 250;
            dgv_transport.Rows.Add("운송비", "");
            dgv_transport.Rows[dgv_transport.Rows.Count - 1].ReadOnly = true;
            dgv_transport.Rows[dgv_transport.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_transport.Rows.Add("적재 제품수", "");
            dgv_transport.Rows[dgv_transport.Rows.Count - 1].ReadOnly = true;
            dgv_transport.Rows[dgv_transport.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_transport.Rows.Add("제품당 운송비", "");
            dgv_transport.Rows[dgv_transport.Rows.Count - 1].ReadOnly = true;
            dgv_transport.Rows[dgv_transport.Rows.Count - 1].Cells[1].Style.BackColor = Color.Wheat;
            dgv_package1_CellEndEdit(dgv_package1, new DataGridViewCellEventArgs(1, 0));
            // 각각의 DataGridView 설정
            ConfigureDataGridView(dgv_package1);
            ConfigureDataGridView(dgv_package2);
            ConfigureDataGridView(dgv_package3);
            ConfigureDataGridView(dgv_transport);
        }

        private void ConfigureDataGridView(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.Columns[dgv.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.BorderStyle = BorderStyle.Fixed3D;
            //dgv.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 10);
        }

        private void dgv_package1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgv_package2_CellEndEdit(dgv_package2, new DataGridViewCellEventArgs(1, 1));
            dgv_package3_CellEndEdit(dgv_package3, new DataGridViewCellEventArgs(1, 1));
            //dgv_package1.Rows[7].Cells[1].Value = global.ConvertDouble(dgv_package1.Rows[7].Cells[1].Value)* global.ConvertDouble(dgv_package1.Rows[8].Cells[1].Value);
            // 콤보박스 셀에 항목 추가
            if(e.RowIndex==0)
            {
                string query = $@"SELECT [차종] as Name FROM [Transport] Where [나라] = N'{dgv_package1.Rows[0].Cells[1].Value}'";
                List<string> comboBoxItems = global_DB.ListSelect(query, (int)global_DB.connDB.selfDB);
                dgv_transport.Rows[0].Cells[1].Value = null; 
                ((DataGridViewComboBoxCell)dgv_transport.Rows[0].Cells[1]).Items.Clear();
                ((DataGridViewComboBoxCell)dgv_transport.Rows[0].Cells[1]).Items.AddRange(comboBoxItems.ToArray());
                if (comboBoxItems.Count <= 0) return;
                dgv_transport.Rows[0].Cells[1].Value = comboBoxItems[0];
            }
            dgv_master_CellEndEdit(dgv_master, new DataGridViewCellEventArgs(1, 1));
            dgv_transport_CellEndEdit(dgv_transport, new DataGridViewCellEventArgs(1, 1));
        }

        private void dgv_package2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double box = global.ConvertDouble( dgv_master.Rows[0].Cells[1].Value);
            double pallet = global.ConvertDouble(dgv_master.Rows[1].Cells[1].Value);
            double etc = global.ConvertDouble(dgv_master.Rows[3].Cells[1].Value);

            double machine = global.ConvertDouble(dgv_package1.Rows[3].Cells[1].Value );
            double volumn = global.ConvertDouble(dgv_package1.Rows[4].Cells[1].Value );
            double lifetime = global.ConvertDouble(dgv_package1.Rows[5].Cells[1].Value );
            dgv_package2.Rows[3].Cells[1].Value = Math.Ceiling(machine * volumn/ lifetime/260);
            dgv_package2.Rows[4].Cells[1].Value = Math.Ceiling(global.ConvertDouble(dgv_package2.Rows[3].Cells[1].Value)/global.ConvertDouble(dgv_package1.Rows[8].Cells[1].Value));
            double sum = global.ConvertDouble(dgv_package2.Rows[0].Cells[1].Value) + global.ConvertDouble(dgv_package2.Rows[1].Cells[1].Value) + global.ConvertDouble(dgv_package2.Rows[2].Cells[1].Value);
            dgv_package2.Rows[5].Cells[1].Value = global.ConvertDouble(dgv_package2.Rows[4].Cells[1].Value)* sum;
            dgv_package2.Rows[6].Cells[1].Value = global.ConvertDouble(dgv_package2.Rows[4].Cells[1].Value)*260/ global.ConvertDouble(dgv_package2.Rows[5].Cells[1].Value);

            double AI17 = global.ConvertDouble(dgv_package2.Rows[6].Cells[1].Value);
            double AI18 = global.ConvertDouble(dgv_package2.Rows[7].Cells[1].Value);
            double T6 = lifetime;

            double result = (AI17 * T6 / AI18) < 1 ? 1 : Math.Ceiling(AI17 * T6 / AI18*10)/10;

            dgv_package2.Rows[8].Cells[1].Value = result;
            dgv_package2.Rows[9].Cells[1].Value = global.ConvertDouble(dgv_package2.Rows[5].Cells[1].Value)* global.ConvertDouble(dgv_package2.Rows[8].Cells[1].Value);
            dgv_package2.Rows[10].Cells[1].Value = Math.Ceiling(global.ConvertDouble(dgv_package2.Rows[4].Cells[1].Value) / global.ConvertDouble(dgv_package1.Rows[6].Cells[1].Value));
            //dgv_package2.Rows[10].Cells[1].Value = global.ConvertDouble(dgv_package2.Rows[4].Cells[1].Value) / global.ConvertDouble(dgv_package1.Rows[10].Cells[1].Value);
            dgv_package2.Rows[12].Cells[1].Value = 260* global.ConvertDouble(dgv_package2.Rows[10].Cells[1].Value)/ global.ConvertDouble(dgv_package2.Rows[11].Cells[1].Value)* lifetime;
            //dgv_package2.Rows[13].Cells[1].Value = global.ConvertDouble(dgv_package2.Rows[9].Cells[1].Value)*15000/(machine* volumn) + global.ConvertDouble(dgv_package2.Rows[12].Cells[1].Value)* 20000 / (machine * volumn);
            dgv_package2.Rows[13].Cells[1].Value = global.ConvertDouble(dgv_package2.Rows[9].Cells[1].Value)* box / (machine* volumn) + global.ConvertDouble(dgv_package2.Rows[12].Cells[1].Value)* pallet / (machine * volumn);
            dgv_package2.Rows[14].Cells[1].Value = etc / global.ConvertDouble(dgv_package1.Rows[8].Cells[1].Value);
            dgv_package2.Rows[15].Cells[1].Value = global.ConvertDouble(dgv_package2.Rows[13].Cells[1].Value) + global.ConvertDouble(dgv_package2.Rows[14].Cells[1].Value) ;
        }

        private void dgv_package3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double rack = global.ConvertDouble(dgv_master.Rows[2].Cells[1].Value);

            double machine = global.ConvertDouble(dgv_package1.Rows[3].Cells[1].Value);
            double volumn = global.ConvertDouble(dgv_package1.Rows[4].Cells[1].Value);
            double lifetime = global.ConvertDouble(dgv_package1.Rows[5].Cells[1].Value);

            dgv_package3.Rows[3].Cells[1].Value = Math.Ceiling(machine * volumn / lifetime / 260);
            dgv_package3.Rows[4].Cells[1].Value = Math.Ceiling(global.ConvertDouble(dgv_package3.Rows[3].Cells[1].Value) / global.ConvertDouble(dgv_package1.Rows[10].Cells[1].Value));
            double sum = global.ConvertDouble(dgv_package3.Rows[0].Cells[1].Value) + global.ConvertDouble(dgv_package3.Rows[1].Cells[1].Value) + global.ConvertDouble(dgv_package3.Rows[2].Cells[1].Value);
            dgv_package3.Rows[5].Cells[1].Value = global.ConvertDouble(dgv_package3.Rows[4].Cells[1].Value) * sum;
            dgv_package3.Rows[6].Cells[1].Value = global.ConvertDouble(dgv_package3.Rows[5].Cells[1].Value) * rack / (machine* volumn);
        }

        private void dgv_transport_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string query = $@"SELECT Value as Name
                            FROM
                            (
                                SELECT 
                                    CAST([고정비] AS nvarchar(50)) as [고정비],
                                    CAST([변동비] AS nvarchar(50)) as [변동비],
                                    CAST([Pallet 수량] AS nvarchar(50)) as [Pallet 수량],
                                    CAST([통화] AS nvarchar(50)) as [통화]
                                FROM 
                                    [Transport]
                                WHERE 
                                    [차종] = N'{dgv_transport.Rows[0].Cells[1].Value}' 
                                    AND [나라] =  N'{dgv_package1.Rows[0].Cells[1].Value}'
                            ) AS SourceTable
                            UNPIVOT
                            (
                                Value FOR FieldName IN ([고정비], [변동비], [Pallet 수량],[통화])
                            ) AS Unpivoted
                            ";
            List<string> item = global_DB.ListSelect(query, (int)global_DB.connDB.selfDB);
            if (item.Count == 0) return;

            label_currency.Text = label_currency2.Text = label_currency4.Text = label_currency3.Text = $"({item[3]})";
            dgv_transport.Rows[2].Cells[1].Value = global.ConvertDouble(item[1]) * 2 *global.ConvertDouble(dgv_transport.Rows[1].Cells[1].Value) + global.ConvertDouble(item[0]);
            dgv_transport.Rows[3].Cells[1].Value = (global.ConvertDouble(dgv_package1.Rows[6].Cells[1].Value)* global.ConvertDouble(dgv_package1.Rows[8].Cells[1].Value) + global.ConvertDouble(dgv_package1.Rows[10].Cells[1].Value))* global.ConvertDouble(item[2]);
            dgv_transport.Rows[4].Cells[1].Value = global.ConvertDouble(dgv_transport.Rows[2].Cells[1].Value) / global.ConvertDouble(dgv_transport.Rows[3].Cells[1].Value);
        }

        private void btn_Detail_Click(object sender, EventArgs e)
        {
            frmTransport transport = new frmTransport();
            transport.ShowDialog();
        }

        private void dgv_transport_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox의 선택된 값이 변경되었을 때 실행할 코드
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                dgv_transport_CellEndEdit(dgv_transport, new DataGridViewCellEventArgs(1, 0));
            }
        }

        private void dgv_transport_EditingControlShowing_1(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgv_transport.CurrentCell is DataGridViewComboBoxCell)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged; // 중복 방지를 위해 이벤트 제거
                    comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
                }
            }
        }

        private void dgv_transport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 || ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell) return;
            if (double.TryParse(e.Value?.ToString(), out double value))
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    e.Value = 0.0;
                }
                e.CellStyle.Format = "#,###,##0.##";
            }
            else if (e.Value is string)
            {
                e.Value = 0.0;
            }
            else if (e.Value is null)
            {
                e.Value = 0.0;
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;

            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = ".xlsx|";
                dlg.FileName = $"운송비_포장비.xlsx";

                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return;
                else if (dialog != DialogResult.OK)
                {
                    CustomMessageBox.RJMessageBox.Show($"Error : 저장위치가 올바르게 선택되지 않았습니다.", "포장비&물류비", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (File.Exists(dlg.FileName))
                {
                    try
                    {
                        File.Delete(dlg.FileName);
                    }
                    catch
                    {
                        CustomMessageBox.RJMessageBox.Show($"Error : 기존 파일을 삭제하는데 실패했습니다.프로그램이 사용중인 경우 다른 파일명을 해 주시기 바립니다.", "포장비&물류비", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                //Excel 프로그램 실행
                application = new Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일만들기                
                workBook = application.Workbooks.Add();

                // Add a new worksheet
                Excel.Worksheet worksheet = workBook.Worksheets[1];

                ExcelExport excel = new ExcelExport();
                double maxRow = 0;
                int fraction = 2, col = 2;
                worksheet.Cells[fraction++, col].Value = "1. "+label1.Text;
                excel.SaveDataGridViewToWorksheet(dgv_package1, worksheet, fraction++, col);
                maxRow = Math.Max(maxRow,dgv_package1.RowCount+fraction);

                fraction = 2;
                col += dgv_package1.ColumnCount + 1;
                worksheet.Cells[fraction++, col].Value = "2. " + label2.Text+" "+ label_currency.Text;
                excel.SaveDataGridViewToWorksheet(dgv_package2, worksheet, fraction++, col);
                maxRow = Math.Max(maxRow, dgv_package2.RowCount + fraction);

                fraction = 2;
                col += dgv_package2.ColumnCount + 1;
                worksheet.Cells[fraction++, col].Value = "2. " + label2.Text + " " + label_currency.Text;
                excel.SaveDataGridViewToWorksheet(dgv_package3, worksheet, fraction++, col);
                maxRow = Math.Max(maxRow, dgv_package3.RowCount + fraction);

                fraction = 2;
                col += dgv_package3.ColumnCount + 1;
                worksheet.Cells[fraction++, col].Value = "3. " + label3.Text+" "+ label_currency2.Text;
                excel.SaveDataGridViewToWorksheet(dgv_transport, worksheet, fraction++, col);
                maxRow = Math.Max(maxRow, dgv_transport.RowCount + fraction);

                Excel.Range usedRange = worksheet.Range[
                    worksheet.Cells[1, 1],
                    worksheet.Cells[maxRow+1, col+1]
                ];

                usedRange.Columns.AutoFit();
                // Save the workbook as a new file
                workBook.SaveAs(dlg.FileName);

            }
            catch (Exception ex)
            {
                CustomMessageBox.RJMessageBox.Show($"Error : {ex.Message}", "포장비&물류비", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }


        }

        private void dgv_master_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double box = 0, pallet = 0, rack = 0,etc=0;
            if (dgv_package1.Rows[0].Cells[1].Value?.ToString().Contains("한국") == true)
            {
                box=25000;
                pallet = 35000;
                rack = 1500000;
                etc = 150;
            }
            else if (dgv_package1.Rows[0].Cells[1].Value?.ToString().Contains("중국") == true)
            {
                box = 150;
                pallet = 210;
                rack = 8830;
                etc = 1;
            }
            else if (dgv_package1.Rows[0].Cells[1].Value?.ToString().Contains("멕시코") == true)
            {
                box = 20;
                pallet = 30;
                rack = 1000;
                etc = 0.100;
            }

            dgv_master.Rows[0].Cells[1].Value = box;
            dgv_master.Rows[1].Cells[1].Value = pallet;
            dgv_master.Rows[2].Cells[1].Value = rack;
            dgv_master.Rows[3].Cells[1].Value = etc;
        }
    }
}
