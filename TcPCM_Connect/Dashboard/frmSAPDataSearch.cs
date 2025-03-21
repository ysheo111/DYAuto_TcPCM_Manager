using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class frmSAPDataSearch : Form, ISAPDataSearch
    {
        public List<string> ReturnValue1 { get; set; }
        public string partNo { get; set; }
        public string revision { get; set; }

        Dictionary<string, string> partName = new Dictionary<string, string>();
        public frmSAPDataSearch()
        {
            InitializeComponent();
        }
        private void ConfigSetting_Load(object sender, EventArgs e)
        {

            string quertSAP = $@"select distinct MATNR,
                                MAKTX
                                from MD_Profit 
                                where  MATNR like '{partNo + revision}%'";
           
            DataTable sap = global_DB.MutiSelect(quertSAP, (int)global_DB.connDB.selfDB);
            if(sap.Rows.Count <= 0)
            {
                ReturnValue1 = new List<string>();
                this.Close();
            }
            foreach(DataRow row in sap.Rows)
            {
                string result = System.Text.RegularExpressions.Regex.Replace($"{row["MATNR"]}", @"\b0+(\d+)", "$1");
                partName.Add(result, $"{row["MAKTX"]}");
                cb_name.Items.Add(result);
            }
            cb_name.Texts = cb_name.Items[0].ToString();
            txt_name.Texts = partName[cb_name.Texts];
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            List<string> date = new List<string>();

            for (DateTime current = dt_start.Value; current <= dt_end.Value; current = current.AddMonths(1))
            {
                string dateString = "'"+ current.ToString("yyyy") + "0" + current.ToString("MM")+ "'";
                date.Add(dateString);
            }            

            string quertSAP = $@"select                               
                                MATNR,
                                AVG(ABSMG ) as  qty, 
                                AVG(VV010 ) as  sales,
                                AVG(VV201+VV202+VV902+VV217+VV213+VV214+VV210+VV209 ) as  Material,
                                AVG(VV204+VV203 ) as  Direct,
                                AVG(VV206+VV205 ) as  InDirect,
                                AVG(VV219+VV220+VV218 ) as  Machine,
                                AVG(VV208+VV207 ) as  tool,
                                AVG(VV408 ) as  ETC,
                                AVG(VV215+VV216 ) as  Transport,
                                AVG(VV401+VV402 ) as  Package,
                                AVG(VV407+VV408+VV601+VV602+VV603+VV801+VV802 ) as  Overheads,
                                AVG(VV403+VV906 ) as  Royalty
                                from MD_Profit 
                                where  MATNR ='{cb_name.Texts}' AND PERIO IN ({string.Join(", ",date)})
                                Group by MATNR";

            DataTable sap = global_DB.MutiSelect(quertSAP, (int)global_DB.connDB.selfDB);
            List<string> item = new List<string>();
            if (sap.Rows.Count != 0)
            {
                DataRow row = sap.Rows[0];
                item.Add($"={row["sales"]}/$K$5");
                item.Add($"=SUM(G14:G18)");
                item.Add($"={row["Material"]}/$K$5");
                item.Add($"={row["Direct"]}/$K$5");
                item.Add($"={row["InDirect"]}/$K$5");
                item.Add($"={row["Machine"]}/$K$5");
                item.Add($"={row["tool"]}/$K$5");
                item.Add($"");
                item.Add($"={row["ETC"]}/$K$5");
                item.Add($"=SUM(G7:G13)");
                item.Add($"={row["Overheads"]}/$K$5");
                item.Add($"={row["Transport"]}/$K$5");
                item.Add($"={row["Package"]}/$K$5");
                item.Add($"={row["Royalty"]}/$K$5");
                item.Add($"=G5-G6");
                item.Add($"=G19/G5");
                item.Add($"{row["qty"]}");
            }
            ReturnValue1 = item;
            this.Close();
        }

        //-> Events Methods
        private void btnClose_Click(object sender, EventArgs e)
        {
            ReturnValue1 = new List<string>();
            this.Close();
        }

        #region -> Drag Form
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        private void combo_date_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            dt_end.Value = DateTime.Now;
            string DateType = combo_date.SelectedItem.ToString();
            switch (DateType)
            {
                case "오늘":
                    dt_start.Value = DateTime.Now;
                    break;
                case "당월":
                    dt_start.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                    dt_end.Value = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day);
                    break;
                case "전월":
                    dt_start.Value = DateTime.Now.AddMonths(-1);
                    break;
                case "3개월":
                    dt_start.Value = DateTime.Now.AddMonths(-3);
                    break;
                case "6개월":
                    dt_start.Value = DateTime.Now.AddMonths(-6);
                    break;
                case "1년":
                    dt_start.Value = DateTime.Now.AddYears(-1);
                    break;
            }
        }

        private void cb_name_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txt_name.Texts = partName[cb_name.Texts];
        }
    }
}
