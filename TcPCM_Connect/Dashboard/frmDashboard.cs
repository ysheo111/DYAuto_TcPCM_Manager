﻿using System;
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

namespace TcPCM_Connect
{
    public partial class frmDashboard : Form
    {
        public string privateFolder;
        ExportCBD export = new ExportCBD();
        Dictionary<string, Dictionary<string, Part>> partData;
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

        private void pb_Refresh_Click(object sender, EventArgs e)
        {
            tv_Bom.Nodes.Clear();
            string query = $@"select CONCAT('f&',Id, '&',STRING_AGG(CONCAT(m.c.value('@lang', 'varchar(max)'), ':', m.c.value('.', 'nvarchar(max)')), '|'))  as name
            from Folders
            OUTER APPLY Folders.Name_LOC.nodes('/translations/value') as m(c)
            where m.c.value('.', 'nvarchar(max)')  like '%Private folder%' or m.c.value('.', 'nvarchar(max)')  like '%Public folder%'
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
            export.FindDashboard(searchButton1.text, tv_Bom.ImageList, dgv_BaicInfo);
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
                if (Control.ModifierKeys == Keys.Control && e.Button==MouseButtons.Left)
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
                    if(selectedNode.Count == 0)
                    {
                        selectedNode.Add(clickedNode);
                    }
                    selectedNode = selectedNode.Distinct().ToList();
                    foreach (TreeNode node in selectedNode) // 여기 수정
                    {
                        selectedNodes.Add(node.Text);
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


        private void 테스트ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void searchButton1_DetailSearchButtonClick(object sender, EventArgs e)
        {

        }
    }
}
