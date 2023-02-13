using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class frmLotCalc : Form
    {
        public frmLotCalc()
        {
            InitializeComponent();
        }

        private void btn_Calc_Click(object sender, EventArgs e)
        {
            txt_lot.Texts =
                    Math.Truncate((global.ConvertDoule(txt_workday.Texts) * global.ConvertDoule(txt_shift.Texts) * 3600 - global.ConvertDoule(txt_setup.Texts) * 60)
                    / global.ConvertDoule(txt_ct.Texts) * global.ConvertDoule(txt_cavity.Texts)).ToString();
        }

        private void frmLotCalc_Load(object sender, EventArgs e)
        {

        }

        private void txt_workday_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;

            if (((Control)sender).Name == "txt_setup") btn_Calc.PerformClick();            
            else if (((Control)sender).Name == "txt_MaintainRate") btn_CalcMaintance.PerformClick();            
            else this.SelectNextControl((Control)sender, true, true, true, true);
            
        }

        private void txt_lot__TextChanged(object sender, EventArgs e)
        {
            
        }


        private void txt_lot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((Control.ModifierKeys == Keys.Control && e.KeyChar.Equals('\u0003')) || (Control.ModifierKeys == Keys.Control && e.KeyChar.Equals('\u0001'))) e.Handled = false;
            else e.Handled = true;
        }

        private void btn_CalcMaintance_Click(object sender, EventArgs e)
        {
            txt_Maintance.Texts =
                   Math.Round((global.ConvertDoule(txt_Imputed.Texts) + global.ConvertDoule(txt_SpaceCost.Texts))
                   * global.ConvertDoule(txt_MaintainRate.Texts) / 100 ,2).ToString();

        }
    }
}
