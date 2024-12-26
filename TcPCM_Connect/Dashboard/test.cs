using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcPCM_Connect.Dashboard
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TcPCM_Connect_Global.ManufacturingLibrary library = new TcPCM_Connect_Global.ManufacturingLibrary();
            string err = library.ExcelOpen();

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "Cost factor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {

            }
        }
    }
}
