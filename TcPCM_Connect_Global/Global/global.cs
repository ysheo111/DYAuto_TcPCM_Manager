using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace TcPCM_Connect_Global
{
    public class global
    {
        public static string authorisationToken = "";
        public static string refreshTokenId = "";
        public static string serverURL = "https://pcm/tcpcm";
        public static string serverURLPath = "tcpcm";
        public static string port = "";
        public static string userID = "Administrator";
        public static string password = "";
        public static string version = "v1";
        public static string dataSource = "";
        public static string dataSourceInterview = "";
        public static string loginID = "";
        public static string loginPWD = "";

        public static double ConvertDouble(object obj)
        {
            obj = obj ?? "";
            return double.TryParse(string.Concat(obj?.ToString().Where(c => !char.IsWhiteSpace(c))) ,out double result) ? ValidDoubleCheck(result) : 0;
        }

        public static string ZeroToNull(double? obj)
        {
            string test = obj?.ToString();
            if (ConvertDouble( obj) == 0) test = "";
            return test;
        }

        public static string dgv_Category_DataError(DataGridView dgv, DataGridViewDataErrorEventArgs e)
        {
            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "ZAR";
            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.Yellow;
            return $"Error : {e.RowIndex + 1}행의 {dgv.Columns[e.ColumnIndex].HeaderText}가 잘못되었습니다.";
        }

        private static double ValidDoubleCheck(double result)
        {
            if (Double.IsNaN(result) || Double.IsInfinity(result) || Double.IsNegativeInfinity(result) || Double.IsPositiveInfinity(result)) return 0;
            return result;
        }

        public static string nullCheck(object value)
        {
            return value?.ToString() ?? "";
        }

        public static Dictionary<string, TValue> ToDictionary<TValue>(object obj)
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, TValue>>(JsonConvert.SerializeObject(obj));
            }
            catch
            {
                return null;
            }
        }

        public static bool TryFormat(string format, out string result, string args)
        {
            string changeArgs = args;
            if (args.Length > 10) changeArgs = args.Substring(0, 10);
            result = args;
            try
            {
                bool formatCheck = System.Text.RegularExpressions.Regex.IsMatch(changeArgs, @"[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
                if (formatCheck)
                {
                    // Do something.
                    result = changeArgs;
                    return true;
                }
                else
                {
                    // Do something else.
                    return false;
                }                
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static void MasterDataValiding(DataGridView dgv, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString().Length <= 0)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.Red;
                return;
            }
            else dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.White;

            bool dateTypeCheck = TryFormat("{####-##-##}", out string result, dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString());
            if ((dgv.Columns[e.ColumnIndex].Name.ToLower().Contains("valid") && !dateTypeCheck)
                || (!dgv.Columns[e.ColumnIndex].Name.ToLower().Contains("valid") && dateTypeCheck))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.Yellow;
            }

            //if(global.ConvertDouble(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) < 0)
            //{
            //    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.Yellow;
            //}

            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value= result;
        }

        public static void CommaAdd(DataGridViewCellFormattingEventArgs e, int n)
        {
            if (e.Value == null) return;

            string num = e.Value.ToString();
            double doubleNum;
            string point = new string('#', n);
            if (double.TryParse(num, out doubleNum)) //데이터를 double로 변환시도
            {
                //string format = $"{point}";
                //int numberOfDecimalPlaces = 2;
                doubleNum = ValidDoubleCheck(doubleNum);
                string formatString = String.Concat("{0:#,###.", point, "}");
                if (0 < doubleNum && doubleNum < 1) formatString = String.Concat("{0:0.", point, "}");

                e.Value = string.Format(formatString, doubleNum);
            }
            else
            {
                e.Value = num;
            }
        }

        public void dgv_EditingControlShowing(string[] standard, object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            DataGridView dgv = ((DataGridView)sender);

            if (!standard.Any(data => dgv.Columns[dgv.CurrentCell.ColumnIndex].Name.Contains(data))) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public static DataGridView CloneDataGrid(DataGridView mainDataGridView)
        {
            DataGridView cloneDataGridView = new DataGridView();

            if (cloneDataGridView.Columns.Count == 0)
            {
                foreach (DataGridViewColumn datagrid in mainDataGridView.Columns)
                {
                    cloneDataGridView.Columns.Add(datagrid.Clone() as DataGridViewColumn);
                }
            }

            DataGridViewRow dataRow = new DataGridViewRow();

            for (int i = 0; i < mainDataGridView.Rows.Count; i++)
            {
                dataRow = (DataGridViewRow)mainDataGridView.Rows[i].Clone();
                int Index = 0;
                foreach (DataGridViewCell cell in mainDataGridView.Rows[i].Cells)
                {
                    dataRow.Cells[Index].Value = cell.Value;
                    Index++;
                }
                cloneDataGridView.Rows.Add(dataRow);
            }
            cloneDataGridView.AllowUserToAddRows = false;
            cloneDataGridView.Refresh();


            return cloneDataGridView;
        }
    }
}
