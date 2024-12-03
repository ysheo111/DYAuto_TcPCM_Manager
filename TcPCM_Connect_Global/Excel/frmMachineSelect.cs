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
    public partial class frmMachineSelect : Form
    {
        public List<string> ReturnValue1 { get; set; }
        public frmMachineSelect()
        {
            InitializeComponent();
        }

        public frmMachineSelect(int count)
        {
            InitializeComponent();

            Panel panel = new Panel();
            panel.Size = new Size(374, 35);
            panel.Location = new Point(0, 10);

            Label label = new Label();
            label.Text = $"나라";
            label.Location = new Point(26, 14);
            label.Size = new Size(58, 15);

            ComboBox combo = new ComboBox();
            combo.Items.Add("한국");
            combo.Items.Add("중국");
            combo.Text = "한국";
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            combo.Size = new Size(252, 23);
            combo.Location = new Point(106, 11);

            panel.Controls.Add(label);
            panel.Controls.Add(combo);

            this.Controls.Add(panel);

            for (int i =0; i<count; i++)
            {
                PanelCreate(i + 1);
            }
        }

        private void PanelCreate(int i)
        {
            Panel panel = new Panel();
            panel.Size= new Size(374, 35);
            panel.Location = new Point(0,10+(40*i));

            Label label = new Label();
            label.Text = $"OP{i*10}";
            label.Location = new Point(26, 14);
            label.Size = new Size(58, 15);

            ComboBox combo = new ComboBox();
            combo.Items.Add("CNC (MCT)_vertical");
            combo.Items.Add("CNC (MCT)_horizontal");
            combo.Items.Add("CNC (MCT)_general");
            combo.Text = "CNC (MCT)_vertical";
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            combo.Size = new Size(252, 23);
            combo.Location = new Point(106, 11);

            panel.Controls.Add(label);
            panel.Controls.Add(combo);

            this.Controls.Add(panel);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            ReturnValue1 = new List<string>();
            foreach (var control in this.Controls)
            {
                if (!(control is Panel)) continue;
                
                foreach (var subControl in ((Panel)control).Controls)
                {
                    if (subControl is ComboBox) this.ReturnValue1.Add(((ComboBox)subControl).Text);
                }
            }
           this.DialogResult =  DialogResult.OK;
        }
    }
}
