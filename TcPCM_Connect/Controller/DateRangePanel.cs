using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcPCM_Connect.Controller
{
    public partial class DateRangePanel : UserControl
    {    // Public properties for control
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string[] DateOptions { get; set; }
        public string SelectedDateOption { get; set; }
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public const uint WM_SYSKEYDOWN = 0x104;

        public DateRangePanel()
        {
            InitializeComponent();

            DateOptions = new string[] { "오늘", "당월", "전월", "3개월", "6개월", "1년" };
            SelectedDateOption = "오늘";
        }

        public DateRangePanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        private void combo_date_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDateOption = combo_date.SelectedItem.ToString();
            UpdateDateRange();
        }

        private void dp_periods_CloseUp(object sender, EventArgs e)
        {
            dp_end.MinDate = dp_start.Value; // 최소 날짜를 시작 날짜 다음 날로 설정
            dp_end.Value = dp_start.Value; // 최소 날짜를 시작 날짜 다음 날로 설정           

            SendMessage(dp_end.Handle, WM_SYSKEYDOWN, (int)Keys.Down, 0);
        }

        private void UpdateDateRange()
        {
            if (combo_date.SelectedItem.ToString() == "오늘")
            {
                dp_start.Text = dp_end.Text = DateTime.Today.ToString(); // Set both to today's date
            }
            else if (combo_date.SelectedItem.ToString() == "당월")
            {
                // Set dp_start to the first day of the current month
                dp_start.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString();

                // Set dp_end to the last day of the current month
                DateTime lastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                dp_end.Text = lastDay.ToString();
            }
            else if (combo_date.SelectedItem.ToString() == "전월")
            {
                // Set dp_start to the first day of the previous month
                DateTime previousMonth = DateTime.Today.AddMonths(-1);
                dp_start.Text = new DateTime(previousMonth.Year, previousMonth.Month, 1).ToString();

                // Set dp_end to the last day of the previous month
                DateTime lastDayPreviousMonth = new DateTime(previousMonth.Year, previousMonth.Month, DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month));
                dp_end.Text = lastDayPreviousMonth.ToString();
            }
            else if (combo_date.SelectedItem.ToString() == "3개월")
            {
                // Set dp_start to the first day of the month 3 months ago
                DateTime threeMonthsAgo = DateTime.Today.AddMonths(-3);
                dp_start.Text = new DateTime(threeMonthsAgo.Year, threeMonthsAgo.Month, 1).ToString();

                // Set dp_end to the last day of the current month
                DateTime lastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                dp_end.Text = lastDay.ToString();
            }
            else if (combo_date.SelectedItem.ToString() == "6개월")
            {
                // Set dp_start to the first day of the month 6 months ago
                DateTime sixMonthsAgo = DateTime.Today.AddMonths(-6);
                dp_start.Text = new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1).ToString();

                // Set dp_end to the last day of the current month
                DateTime lastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                dp_end.Text = lastDay.ToString();
            }
            else if (combo_date.SelectedItem.ToString() == "1년")
            {
                // Set dp_start to the first day of the year
                DateTime beginningOfYear = new DateTime(DateTime.Today.Year, 1, 1);
                dp_start.Text = beginningOfYear.ToString();

                // Set dp_end to the last day of the year
                DateTime endOfYear = new DateTime(DateTime.Today.Year, 12, 31);
                dp_end.Text = endOfYear.ToString();
            }
            // Add an 'else' block to handle unexpected values in combo_date.Text (optional)
            else
            {
                //  Handle invalid selection (optional)
                //  e.g., display an error message or set default values
            }
        }

        [Category("Appearance")]
        [Description("라벨의 내용을 설정합니다.")]
        public string LabelText
        {
            get => label1.Text;
            set
            {
                if (label1 != null)
                    label1.Text = value;
            }
        }

    }
}
