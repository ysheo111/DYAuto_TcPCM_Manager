using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcPCM_Connect_Global
{
    public partial class frmPartWorkSheetSelect : Form
    {
        public string ReturnValue1 { get; set; }
        public string ReturnValue2 { get; set; }
        public frmPartWorkSheetSelect()
        {
            InitializeComponent();
        }

        public List<string> workSheet = new List<string>();
        private void frmPartWorkSheetSelect_Load(object sender, EventArgs e)
        {
            if (workSheet.Count>0) combo_Part.Text = workSheet[1];
            if (workSheet.Count>1) combo_Manufacturing.Text = workSheet[2];

            foreach(string sheet in workSheet)
            {
                combo_Part.Items.Add(sheet);
                combo_Manufacturing.Items.Add(sheet);
            }           
        }

        private void btn_Check_Click(object sender, EventArgs e)
        {
            ReturnValue1 = combo_Part.Text;
            ReturnValue2 = combo_Manufacturing.Text;

            this.Close();
        }
    }
}
