using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace TcPCM_Connect_Global
{
    public enum TagType
    {
        Folder,
        Project,
        Part,
        Tool
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExportCBD
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="publicFolder"></param>
        /// <returns></returns>
        public List<string> HierarchicalExplore(TagType type, string id)
        {
            string query = "";
            if (type== TagType.Folder)
            {
                query = $@"select CONCAT('f&',Id, '&',STRING_AGG(CONCAT(m.c.value('@lang', 'varchar(max)'), ':', m.c.value('.', 'nvarchar(max)')), '|'))  as name
                            from Folders
                            OUTER APPLY Folders.Name_LOC.nodes('/translations/value') as m(c)
                            where ParentId = {id}
                            GROUP BY Id
                            Union
                            select CONCAT('p&',a.Id, '&', a.projectName) as name from
                            (
	                            select
		                            s.Id,
                                    s.FolderID,
		                            STRING_AGG(CONCAT(m.c.value('@lang', 'varchar(max)'), ':', m.c.value('.', 'nvarchar(max)')), '|') as projectName
	                            from Projects as s
		                            outer apply s.Name_LOC.nodes('/translations/value') as m(c)
                                where s.FolderID in ({id})
	                            group by s.Id, s.FolderID
                            ) as a";

                query += $@"
                            UNION 
                            select CONCAT('&',a.Id, '&:',b.Name_LOC_Extracted) as name   
                            from Calculations as a
                            right join 
                            (
	                            select * from Parts where Id in (select PartID from FolderEntries where FolderId = {id})
                            )as b
                            on a.Partid = b.Id;
                            ";
            }

            else if (type == TagType.Project)
            {
                query = $@"select CONCAT('&',a.Id, '&:',b.Name_LOC_Extracted) as name   
                            from Calculations as a
                            right join 
                            (
	                            select * from Parts where Id in (select PartID from ProjectPartEntries where ProjectID = {id})
                            )as b
                            on a.Partid = b.Id;
                            ";
            }
            else
            {
                query = $@"select CONCAT('&',b.Id, '&',a.Name_LOC_Extracted) as name   
                            from Parts as a
                            right join 
                            (
	                            select * from Calculations where id in (select CurrentCalcId from CalcBomChildren({id}) where CurrentCalcId <> {id})
                            ) as b
                            on a.Id = b.PartId;
                            ";
            }

            return global_DB.ListSelect(query, (int)global_DB.connDB.PCMDB);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExploreNodeAdd(TreeNodeCollection parent, List<string> items)
        {
            foreach (string item in items)
            {
                string[] info = item.Split('&');
                string[] name = info[2].Split('|');

                TreeNode node = new TreeNode();
                node.Name = info[0] + info[1];

                if (info[0] == "f")
                {
                    node.ImageIndex = node.SelectedImageIndex = 0;
                    node.Tag = TagType.Folder;
                }
                else if (info[0] == "p")
                {
                    node.ImageIndex = node.SelectedImageIndex = 2;
                    node.Tag = TagType.Project;
                }
                else
                {
                    node.ImageIndex = node.SelectedImageIndex = 3;
                    node.Tag = TagType.Part;
                }

                node.Nodes.Add("expand", "");
                node.Text = name[0].Split(':')[0];

                int index = Array.FindIndex(name, m => m.Contains("en-US"));
                if (index > -1) node.Text = name[index].Split(':')[1];
                else
                {
                    index = Array.FindIndex(name, m => m.Contains("ko-KR"));
                    if (index > -1) node.Text = name[index].Split(':')[1];
                    else node.Text = name[0].Split(':')[1];
                }

                parent.Add(node);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="selectList"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, Part>> PartDataExport(List<string> partList)
        {
            var partInfoList = LoadTcPCM(partList);
            if (partInfoList == null) return null;

            return partInfoList;
        }

        private Dictionary<string, Dictionary<string, Part>> LoadTcPCM(List<string> itemList)
        {
            if (itemList?.Count <= 0) return null;

            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Export";

            JObject postData = new JObject();
            postData.Add("CalculationIds", JArray.FromObject(itemList));
            postData.Add("ConfigurationGuid", global_iniLoad.GetConfig("CBD", "Export"));
            var apiResult = WebAPI.POST(callUrl, postData);

            BomExport bomExport = new BomExport();
            string query = string.Join(") Union select CurrentCalcId, ParentCalcId from [dbo].[CalcBomChildren](", itemList);
            System.Data.DataTable calcBomChildren = global_DB.MutiSelect($"select CurrentCalcId, ParentCalcId from [dbo].[CalcBomChildren]({query})", (int)global_DB.connDB.PCMDB);
            foreach (DataRow row in calcBomChildren.Rows)
            {
                bomExport.bom.Add(row["CurrentCalcId"].ToString(), row["ParentCalcId"]?.ToString());
            }

            if (apiResult?.Length <= 0) return null;

            var chartData = bomExport.SimpleDataSort(apiResult);
            return chartData;
        } 
     
    }
}
