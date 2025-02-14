﻿using System;
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
    public partial class workSheetSelect : Form
    {
        public List<string> workSheet = new List<string>();
        public string ReturnValue1 { get; set; }
        public List<string> ReturnValues { get; set; }

        public workSheetSelect()
        {
            InitializeComponent();
        }

        private void btn_Check_Click(object sender, EventArgs e)
        {
            if(dgv_WorkSheet.SelectedCells.Count == 1)
                this.ReturnValue1 = dgv_WorkSheet.Rows[dgv_WorkSheet.CurrentCell.RowIndex].Cells["WorkSheet"].Value?.ToString();
            else
            {
                this.ReturnValues = new List<string>();
                foreach (DataGridViewCell cell in dgv_WorkSheet.SelectedCells)
                {
                    if (cell.OwningColumn.Name == "WorkSheet" && cell.Value != null)
                    {
                        ReturnValues.Add(cell.Value.ToString());
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void workSheetSelect_Load(object sender, EventArgs e)
        {
            dgv_WorkSheet.Columns.Add("WorkSheet", "WorkSheet");

            foreach(string item in workSheet)
            {
                dgv_WorkSheet.Rows.Add();
                dgv_WorkSheet.Rows[dgv_WorkSheet.Rows.Count - 1].Cells["WorkSheet"].Value = item;
            }
        }

        private void dgv_WorkSheet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_WorkSheet.SelectedCells.Count == 1)
                this.ReturnValue1 = dgv_WorkSheet.Rows[dgv_WorkSheet.CurrentCell.RowIndex].Cells["WorkSheet"].Value?.ToString();
            else
            {
                this.ReturnValues = new List<string>();
                foreach (DataGridViewCell cell in dgv_WorkSheet.SelectedCells)
                {
                    if (cell.OwningColumn.Name == "WorkSheet" && cell.Value != null)
                    {
                        ReturnValues.Add(cell.Value.ToString());
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
