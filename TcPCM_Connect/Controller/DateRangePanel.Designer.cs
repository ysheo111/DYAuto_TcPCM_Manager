
using System.Windows.Forms;

namespace TcPCM_Connect.Controller
{
    partial class DateRangePanel
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.label1 = new System.Windows.Forms.Label();
            this.combo_date = new CustomControls.RJControls.RJComboBox();
            this.dp_start = new CustomControls.RJControls.RJDatePicker();
            this.dp_end = new CustomControls.RJControls.RJDatePicker();

            // 
            // dp_end
            // 
            this.dp_end.BorderColor = System.Drawing.Color.DimGray;
            this.dp_end.BorderSize = 0;
            this.dp_end.CalendarTitleBackColor = System.Drawing.SystemColors.ControlText;
            this.dp_end.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dp_end.Font = new System.Drawing.Font("굴림", 9.5F);
            this.dp_end.Location = new System.Drawing.Point(470, 4);
            this.dp_end.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dp_end.MinimumSize = new System.Drawing.Size(4, 35);
            this.dp_end.Name = "dp_end";
            this.dp_end.Size = new System.Drawing.Size(226, 35);
            this.dp_end.SkinColor = System.Drawing.Color.White;
            this.dp_end.TabIndex = 2;
            this.dp_end.TextColor = System.Drawing.Color.DimGray;
            // 
            // combo_date
            // 
            this.combo_date.BackColor = System.Drawing.Color.White;
            this.combo_date.BorderColor = System.Drawing.Color.DimGray;
            this.combo_date.BorderSize = 1;
            this.combo_date.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.combo_date.Font = new System.Drawing.Font("굴림", 9.5F);
            this.combo_date.ForeColor = System.Drawing.Color.DimGray;
            this.combo_date.IconColor = System.Drawing.Color.DimGray;
            this.combo_date.Items.AddRange(new object[] {
            "오늘",
            "당월",
            "전월",
            "3개월",
            "6개월",
            "1년"});
            this.combo_date.ListBackColor = System.Drawing.Color.White;
            this.combo_date.ListTextColor = System.Drawing.Color.DimGray;
            this.combo_date.Location = new System.Drawing.Point(93, 4);
            this.combo_date.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.combo_date.Name = "combo_date";
            this.combo_date.Padding = new System.Windows.Forms.Padding(1);
            this.combo_date.Size = new System.Drawing.Size(97, 35);
            this.combo_date.TabIndex = 0;
            this.combo_date.Texts = "오늘";
            this.combo_date.OnSelectedIndexChanged += new System.EventHandler(this.combo_date_OnSelectedIndexChanged);
            // 
            // dp_start
            // 
            this.dp_start.BorderColor = System.Drawing.Color.DimGray;
            this.dp_start.BorderSize = 0;
            this.dp_start.CalendarTitleBackColor = System.Drawing.SystemColors.ControlText;
            this.dp_start.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dp_start.Font = new System.Drawing.Font("굴림", 9.5F);
            this.dp_start.Location = new System.Drawing.Point(219, 4);
            this.dp_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dp_start.MinimumSize = new System.Drawing.Size(4, 35);
            this.dp_start.Name = "dp_start";
            this.dp_start.Size = new System.Drawing.Size(226, 35);
            this.dp_start.SkinColor = System.Drawing.Color.White;
            this.dp_start.TabIndex = 1;
            this.dp_start.TextColor = System.Drawing.Color.DimGray;
            this.dp_start.Value = new System.DateTime(2024, 11, 6, 0, 0, 0, 0);
            this.dp_start.CloseUp += new System.EventHandler(this.dp_periods_CloseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "검색기간";

            this.Controls.Add(label1);
            this.Controls.Add(dp_start);
            this.Controls.Add(combo_date);
            this.Controls.Add(dp_end);

            this.Size = new System.Drawing.Size(647, 44);
        }
        // UI elements
        private Label label1;
        private CustomControls.RJControls.RJComboBox combo_date;
        private CustomControls.RJControls.RJDatePicker dp_start;
        private CustomControls.RJControls.RJDatePicker dp_end;
        #endregion
    }
}
