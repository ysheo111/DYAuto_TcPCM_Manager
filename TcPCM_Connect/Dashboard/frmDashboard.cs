using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using TcPCM_Connect_Global;

namespace TcPCM_Connect
{
    public partial class frmDashboard : Form
    {
        public string privateFolder;
        ExportCBD export = new ExportCBD();
        Dictionary<string, Dictionary<string, Dictionary<string, object>>> partData;
        Bom.ExportLang mode = Bom.ExportLang.Kor;

        public frmDashboard()
        {
            InitializeComponent();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            if (!frmLogin.auth.Contains("admin")) btn_Configuration.Visible = false;

            // 아이콘 리스트 만들기
            ImageList myimageList = new ImageList();
            myimageList.Images.Add(Properties.Resources.icon4); //닫힌 폴더 아이콘
            myimageList.Images.Add(Properties.Resources.icon3); //열린 폴더 아이콘
            myimageList.Images.Add(Properties.Resources.icon6); //닫힌 프로젝트 아이콘
            myimageList.Images.Add(Properties.Resources.icon5); //열린 프로젝트 아이콘
            tv_Bom.ImageList = myimageList;

            pb_Refresh_Click(this.pb_Refresh, null);
            BasicInfoColumn();
        }

        private void SelectNode()
        {
            if (tv_Bom.SelectedNode == null) return;

            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            try
            {
                dgv_BaicInfo.Rows.Clear();

                List<string> selectList = FindChild(tv_Bom.SelectedNode);
                partData = export.PartDataExport(dgv_BaicInfo, selectList);

                if (partData == null) CustomMessageBox.RJMessageBox.Show($"Error : 데이터가 존재하지 않습니다.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (partData.ContainsKey("Error")) CustomMessageBox.RJMessageBox.Show($"Error : {partData["Error"].Keys.ToList()[0]}", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            catch
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 작업중 오류가 발생하였습니다. 다시 시도해주세요.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            LoadingScreen.CloseSplashScreen();
        }

        private void tv_Bom_DoubleClick(object sender, EventArgs e)
        {
            SelectNode();
        }

        private List<string> FindChild(TreeNode node)
        {
            List<string> childList = new List<string> {node.Name};
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Nodes.Count != 0) childList = childList.Concat(FindChild(child)).ToList();
                else  childList.Add(child.Name);
            }
            return childList;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            frmCBD frm = new frmCBD();
            frm.ShowDialog();
        }

        private void pb_Refresh_Click(object sender, EventArgs e)
        {
            tv_Bom.Nodes.Clear();
            List<string> folders = export.FindFolder(privateFolder);
            export.FolderList(tv_Bom, folders);
        }

        private void dgv_UserInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e,2);
        }

        private void btn_Configuration_Click(object sender, EventArgs e)
        {
            ConfigSetting config = new ConfigSetting();
            config.className = "CBD";
            config.Show();
        }

        private void dgv_UserInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmCBD frm = new frmCBD();            
            frm.part = partData[dgv_BaicInfo.Rows[e.RowIndex].Cells["PartNumber"].Value?.ToString()];
            frm.mode = "Export";
            frm.exportMode = mode;
            frm.ShowDialog();
        }

        private void tv_Bom_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Display context menu for eg:
                tv_Bom.SelectedNode = tv_Bom.GetNodeAt(new Point(e.X, e.Y));
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void 내보내기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv_Bom.SelectedNode == null) return;
            if (tv_Bom.SelectedNode.FullPath.Contains("Public folder") && !frmLogin.auth.Contains("admin"))
            {
                CustomMessageBox.RJMessageBox.Show("작업자에게 권한이 없습니다. 다시 시도해주세요.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                frmCBD frm = new frmCBD();
                frm.mode = "Import";
                frm.exportMode = mode;
                frm.currentNode = tv_Bom.SelectedNode.Name;
                frm.ShowDialog();
            }
        }
        private void BasicInfoColumn()
        {
            DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            dataGridViewCheckBoxColumn.Name = dataGridViewCheckBoxColumn.HeaderText = "Check";
            dgv_BaicInfo.Columns.Add(dataGridViewCheckBoxColumn);
            dgv_BaicInfo.Columns.Add("PartNumber", "PartNumber");
            dgv_BaicInfo.Columns["PartNumber"].Visible=false;
            dgv_BaicInfo.Columns.Add(Report.Designation.status, "상태");
            dgv_BaicInfo.Columns.Add(Report.Designation.itemNumber, "도번");
            dgv_BaicInfo.Columns.Add(Report.Designation.basic, "부품명");
            //dgv_BaicInfo.Columns.Add(Report.Designation.customer, "고객사");
            dgv_BaicInfo.Columns.Add(Report.Designation.supplier, "협력사");
            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = Report.Designation.dateOfCalc;
            calendar.HeaderText = "작성일";
            calendar.DefaultCellStyle.Format = "yyyy-MM-dd";
            dgv_BaicInfo.Columns.Add(calendar);
            dgv_BaicInfo.Columns.Add(Report.Designation.modified, "작성자");
            //dgv_BaicInfo.Columns.Add(Report.Designation.forcast, "Forcast(year)");
            //dgv_BaicInfo.Columns.Add(Report.Designation.productionDayPerYear, "일생산량");
        }

        private void cb_Mode_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Mode.Checked)
            {
                cb_Mode.Image = Properties.Resources.간략1;
                mode = Bom.ExportLang.Eng;
            }
            else
            {
                cb_Mode.Image = Properties.Resources.상세1;
                mode = Bom.ExportLang.Kor;
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                CustomMessageBox.RJMessageBox.Show($"Error : 저장위치가 올바르게 선택되지 않았습니다.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in dgv_BaicInfo.Rows)
            {
                if (!(bool)(row.Cells["Check"].Value??false)) continue;

                frmCBD frm = new frmCBD();

                frm.part = partData[row.Cells["PartNumber"].Value?.ToString()];
                frm.mode = "ExcelExportAll";
                frm.fileLocation = dlg.SelectedPath;
                frm.exportMode = mode;
                frm.Hide();
                frm.ShowDialog();
            }

            CustomMessageBox.RJMessageBox.Show($"CBD 출력이 완료되었습니다.", "Cost Break Down", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgv_BaicInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgv_BaicInfo.Rows[e.RowIndex].Cells["Check"].Value = !(bool)(dgv_BaicInfo.Rows[e.RowIndex].Cells["Check"].Value??false);
        }
    }
}
