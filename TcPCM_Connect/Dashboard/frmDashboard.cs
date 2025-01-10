using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using TcPCM_Connect_Global;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace TcPCM_Connect
{
    public partial class frmDashboard : Form
    {
        public string privateFolder;
        private string partQuery, projectQuery, folderQuery; 
        Interface export = new Interface();
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
            myimageList.Images.Add(Properties.Resources.tool); //툴아이콘
            tv_Bom.ImageList = myimageList;

            pb_Refresh_Click(this.pb_Refresh, null);
            BasicInfoColumn();
        }

        private void pb_Refresh_Click(object sender, EventArgs e)
        {
            tv_Bom.Nodes.Clear();
            string query = $@"select CONCAT('f&',Id, '&',STRING_AGG(CONCAT(m.c.value('@lang', 'varchar(max)'), ':', m.c.value('.', 'nvarchar(max)')), '|'))  as name
            from Folders
            OUTER APPLY Folders.Name_LOC.nodes('/translations/value') as m(c)
            where m.c.value('.', 'nvarchar(max)')  like '%Private folder%' or m.c.value('.', 'nvarchar(max)')  like '%Public folder%' or m.c.value('.', 'nvarchar(max)')  like '%Knowledge domain%'
            GROUP BY Id";
            List<string> init = global_DB.ListSelect(query, (int)global_DB.connDB.PCMDB);
            export.ExploreNodeAdd(tv_Bom.Nodes, init);
            //tv_Bom.TreeViewNodeSorter = new NodeSorter();
            //tv_Bom.Sort();
        }

        private void btn_Configuration_Click(object sender, EventArgs e)
        {
            ConfigSetting config = new ConfigSetting();
            config.className = "CBD";
            config.Show();
        }
        private void btn_Export_Click(object sender, EventArgs e)
        {
        }

        private void tv_Bom_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.Nodes.RemoveByKey("expand");
            List<string> add = export.HierarchicalExplore((TagType)e.Node.Tag, e.Node.Name.Remove(0, 1));
            export.ExploreNodeAdd(e.Node.Nodes, add);
        }


        private void searchButton1_SearchButtonClick(object sender, EventArgs e)
        {
            dgv_BaicInfo.Rows.Clear();
            // 'a'를 포함한 모든 노드 찾기
            List<string> query = new List<string>() { searchButton1.text, folderQuery, projectQuery, partQuery};
            export.FindDashboard(query, tv_Bom.ImageList, dgv_BaicInfo);
        }

        private void dgv_UserInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e, 2);
        }

        private void BasicInfoColumn()
        {
            dgv_BaicInfo.Columns.Add("PartNumber", "PartNumber");
            dgv_BaicInfo.Columns["PartNumber"].Visible = false;

            DataGridViewImageColumn dataGridViewCheckBoxColumn = new DataGridViewImageColumn();
            dataGridViewCheckBoxColumn.HeaderText = "";
            dataGridViewCheckBoxColumn.Name = "img";
            dgv_BaicInfo.Columns.Add(dataGridViewCheckBoxColumn);


            dgv_BaicInfo.Columns.Add(Report.Header.partName, "구성품명");
            dgv_BaicInfo.Columns[Report.Header.partName].ReadOnly = true;
            dgv_BaicInfo.Columns.Add("위치", "위치");
            dgv_BaicInfo.Columns["위치"].ReadOnly = true;

            dgv_BaicInfo.Columns["위치"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCheckBoxColumn.Width = 50;
            dgv_BaicInfo.Columns[Report.Header.partName].Width = (int)(dgv_BaicInfo.Width * 0.30);

        }
        List<string> selectItem = new List<string>();
        List<TreeNode> selectedNode = new List<TreeNode>();
        private void tv_Bom_MouseDown(object sender, MouseEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            TreeNode clickedNode = treeView.GetNodeAt(e.X, e.Y);

            if (clickedNode != null)
            {
                if (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Left)
                {
                    // Ctrl 키를 누르고 클릭한 경우: 선택 토글
                    clickedNode.BackColor = clickedNode.BackColor == Color.LightSkyBlue ? Color.White : Color.LightSkyBlue;
                    if (clickedNode.BackColor == Color.LightSkyBlue) selectedNode.Add(clickedNode);
                    else selectedNode.Remove(clickedNode);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    // 마우스 오른쪽 버튼 클릭한 경우: 선택된 노드 정보 표시
                    List<string> selectedNodes = new List<string>();
                    if (selectedNode.Count == 0)
                    {
                        selectedNode.Add(clickedNode);
                    }
                    selectedNode = selectedNode.Distinct().ToList();
                    foreach (TreeNode node in selectedNode) // 여기 수정
                    {
                        selectedNodes.Add(node.Name);
                    }
                    SelectItems(selectedNodes);
                }
                else
                {
                    // 모든 노드 선택 해제
                    foreach (TreeNode node in selectedNode)
                    {
                        node.BackColor = Color.White;
                    }
                    selectedNode.Clear();
                }
            }
        }

        private void dgv_BaicInfo_MouseDown(object sender, MouseEventArgs e)
        {
            List<string> item = new List<string>();
            if (e.Button == MouseButtons.Right)
            {
                foreach (DataGridViewCell cell in dgv_BaicInfo.SelectedCells)
                {
                    item.Add(dgv_BaicInfo.Rows[cell.RowIndex].Cells["PartNumber"].Value.ToString());
                }

                SelectItems(item);
            }
        }

        private void SelectItems(List<string> item)
        {
            selectItem = item;
            contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
        }


        private void searchButton1_DetailSearchButtonClick(object sender, EventArgs e)
        {
            frmDetailSearch search = new frmDetailSearch();
            if (search.ShowDialog() == DialogResult.Cancel) return;

            folderQuery = search.ReturnValue1;
            projectQuery = search.ReturnValue2;
            partQuery = search.ReturnValue3;
        }

        private void eXCEL내려받기ToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (selectItem.Count == 0)
            {
                CustomMessageBox.RJMessageBox.Show($"내보낼 부품이 선택되지 않았습니다.", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                CustomMessageBox.RJMessageBox.Show($"폴더 오픈을 실패하였습니다", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PartExport bom = new PartExport();
            string err= bom.ExportPartBom(selectItem, dialog.FileName, mode);
            if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //dialog.FileName

        }

        private void eXCEL올리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PartExcel import = new PartExcel();
            Dictionary<string,string> id= GetTargetTypeID();
            string err = import.Import(id["TargetType"], global.ConvertDouble(id["ID"]));

            if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 공정라이브러리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread splashthread = new Thread(new ThreadStart(LoadingScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();

            Dictionary<string, string> id = GetTargetTypeID();
            TcPCM_Connect_Global.ManufacturingLibrary library = new TcPCM_Connect_Global.ManufacturingLibrary();
            string err = library.ExcelOpen();

            if (err != null)
                CustomMessageBox.RJMessageBox.Show($"불러오기에 실패하였습니다\nError : {err}", "공정 라이브러리", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                err = library.Import("CBD", id["TargetType"], id["ID"]);
                if (err != null)
                    CustomMessageBox.RJMessageBox.Show($"저장에 실패하였습니다\nError : {err}", "공정 라이브러리", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "공정 라이브러리", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            LoadingScreen.CloseSplashScreen();
        }

        private Dictionary<string,string> GetTargetTypeID()
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            string Id = selectItem.Last();
            string targetType = Id.StartsWith("f") ? "Folder" :
                                Id.StartsWith("p") ? "Project" : "Calculation";
            
            if (Id.StartsWith("f") || Id.StartsWith("p"))
            {
                Id = Id.Substring(1); // 첫 글자 제거
            }
            pairs.Add("ID", Id);
            pairs.Add("TargetType", targetType);

            return pairs;
        }

        private void 업로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BOMExcel import = new BOMExcel();
            Dictionary<string, string> id = GetTargetTypeID();
            string err = import.Import(id["TargetType"], global.ConvertDouble(id["ID"]));

            if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "BOM Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "BOM Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void eXCEL다운로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectItem.Count == 0)
            {
                CustomMessageBox.RJMessageBox.Show($"내보낼 부품이 선택되지 않았습니다.", "BOM Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                CustomMessageBox.RJMessageBox.Show($"폴더 오픈을 실패하였습니다", "BOM Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BOMExcel bom = new BOMExcel();
            string err = bom.ExportPartBom(selectItem, dialog.FileName, mode);
            if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "BOM Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "BOM Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void rb_lang_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            if (radio.Checked)
            {
                mode = (Bom.ExportLang)radio.Tag;
            }
        }
    }
}
