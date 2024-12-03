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

        private void tv_Bom_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Display context menu for eg:
                tv_Bom.SelectedNode = tv_Bom.GetNodeAt(new Point(e.X, e.Y));
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }       

        private void searchButton1_SearchButtonClick(object sender, EventArgs e)
        {
            dgv_BaicInfo.Rows.Clear();
            // 'a'를 포함한 모든 노드 찾기
            //List<object> nodesWithA = FindNodesByName(tv_Bom, searchButton1.text);
            FindNodesByName(tv_Bom, searchButton1.text);
        }

        public List<object> FindNodesByName(TreeView treeView, string keyword)
        {
            List<object> result = new List<object>();

            // 트리뷰의 모든 노드에 대해 재귀적으로 검색
            foreach (TreeNode node in treeView.Nodes)
            {
                SearchNode(node, keyword, result, "","");
            }

            return result;
        }

        private void SearchNode(TreeNode node, string keyword, List<object> result, string path,string icon)
        {
            // 현재 노드의 이름에 'a'가 포함되어 있으면 결과에 추가
            if (node.Text.Contains(keyword))
            {
                dgv_BaicInfo.Rows.Add("", tv_Bom.ImageList.Images[node.ImageIndex],$"{node.Text}", $"{ path.Remove(path.Length - 1)}");
            }

            // 자식 노드가 있으면 재귀적으로 검색
            foreach (TreeNode childNode in node.Nodes)
            {
                SearchNode(childNode, keyword, result, path + node.Text + "\\", icon);
            }
        }


        private void dgv_UserInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            global.CommaAdd(e, 2);
        }

        private void BasicInfoColumn()
        {
            //DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            //dataGridViewCheckBoxColumn.Name = dataGridViewCheckBoxColumn.HeaderText = "Check";
            //dgv_BaicInfo.Columns.Add(dataGridViewCheckBoxColumn);            

            dgv_BaicInfo.Columns.Add("PartNumber", "PartNumber");
            dgv_BaicInfo.Columns["PartNumber"].Visible = false;

            DataGridViewImageColumn dataGridViewCheckBoxColumn = new DataGridViewImageColumn();
            dataGridViewCheckBoxColumn.HeaderText = "아이콘";
            dataGridViewCheckBoxColumn.Name = "img";
            dgv_BaicInfo.Columns.Add(dataGridViewCheckBoxColumn);

            dgv_BaicInfo.Columns.Add(Report.Header.partName, "구성품명");
            dgv_BaicInfo.Columns[Report.Header.partName].ReadOnly = true;
            dgv_BaicInfo.Columns.Add("위치", "위치");
            dgv_BaicInfo.Columns["위치"].ReadOnly = true;
            dgv_BaicInfo.Columns.Add(Report.Header.author, "작성자");
            dgv_BaicInfo.Columns[Report.Header.author].ReadOnly = true;
            CalendarColumn calendar = new CalendarColumn();
            calendar.Name = Report.Header.dateOfCreation;
            calendar.HeaderText = "작성일";
            calendar.DefaultCellStyle.Format = "yyyy-MM-dd";
            dgv_BaicInfo.Columns.Add(calendar);
            dgv_BaicInfo.Columns.Add(Report.Header.partName, "차종명");
            dgv_BaicInfo.Columns[Report.Header.partName].ReadOnly = true;
            dgv_BaicInfo.Columns[Report.Header.dateOfCreation].ReadOnly = true;
        }
    }
}
