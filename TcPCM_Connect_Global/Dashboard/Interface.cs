using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Xml.Linq;

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
    public class Interface
    {
        public List<string> AllRootCalcId(List<string> nodes)
        {
            var projects = nodes.Where(n => n.StartsWith("p")).Select(n => n.Substring(1)).ToList();
            var folders = nodes.Where(n => n.StartsWith("f")).Select(n => n.Substring(1)).ToList();
            var calculations = nodes.Where(n => !n.StartsWith("p") && !n.StartsWith("f")).ToList();

            var query = $@"Select Id as name 
                        from 
                            (select PartId 
                             from FolderEntries 
                             where FolderId in ({(folders.Count == 0 ? "''" : string.Join(",", folders))})
                             Union All
                             select PartId 
                             from ProjectPartEntries 
                             where ProjectId in ({(projects.Count == 0 ? "''" : string.Join(",", projects))})) as a
                        Left Join Calculations as b 
                        on a.PartID = b.PartID 
                        where Master = 1";

            return global_DB.ListSelect(query, (int)global_DB.connDB.PCMDB).Concat(calculations).ToList();
        }

        public JObject LoadCalc(List<string> calcList, string config)
        {
            if (calcList?.Count <= 0) return null;

            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Export";

            JObject postData = new JObject();
            postData.Add("CalculationIds", JArray.FromObject(calcList));
            postData.Add("ConfigurationGuid", global_iniLoad.GetConfig("CBD", config));
            var apiResult = WebAPI.POST(callUrl, postData);

            if (apiResult?.Length <= 0) return null;
            JObject r = JObject.Parse(apiResult);
            if (!r.ContainsKey("data"))
            {
                return null;
            }

            //var chartData = bomExport.SimpleDataSort(apiResult);
            return r;
        }
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

                if (parent.ContainsKey(info[0] + info[1])) return;

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
                    else if(name[0].Split(':').Length > 2) node.Text = name[0].Split(':')[1];
                }
                parent.Add(node);
            }
        }

        public void FindDashboard(List<string> search, ImageList image, DataGridView dgv)
        {
            //searchString  partQuery, projectQuery, folderQuery
            string query = $@"WITH RecursiveParents AS (
                SELECT 
                    cs.CurrentCalcId,
		            cs.ParentCalcId,  
                    p.Name_LOC_Extracted AS Path,
                    cs.CurrentCalcId AS Init,  -- Init 컬럼에 해당 레코드의 id를 초기값으로 설정
		            p.Name_LOC_Extracted  as name,
		            p.id as partID
		            FROM CalculationStructureView cs
		            INNER JOIN Parts p ON p.Id = (select PartId from Calculations as c where cs.CurrentCalcId=c.id)
		            WHERE p.Name_LOC_Extracted LIKE '%{search[0]}%' {(string.IsNullOrEmpty( search[1]) == false ? " and "+search[1]:"") } 

                UNION ALL

                SELECT 
                     cs.CurrentCalcId,
                     cs.ParentCalcId,  
                     p.Name_LOC_Extracted + '</split>' + fh.Path,
                    fh.Init,  -- 상위 레코드의 Init 값을 그대로 사용
		            fh.name,  -- 상위 레코드의 Init 값을 그대로 사용
		            p.id as partID
	            FROM CalculationStructureView cs
                INNER JOIN RecursiveParents fh ON cs.CurrentCalcId = fh.ParentCalcId
	            INNER JOIN Parts p ON p.Id = (select PartId from Calculations as c where cs.CurrentCalcId=c.id)
	            where p.PartType = 1 and p.Deleted is null
            ),
            RankedResults AS (
              SELECT *,
                     ROW_NUMBER() OVER (PARTITION BY Init ORDER BY LEN(Path) DESC) AS rn
              FROM RecursiveParents
            ),
            PartResult as (
	            SELECT
	              rr.Path, rr.init, rr.name, rr.partID,
	              pe.ProjectId as projectID,
	              fe.FolderId as folderID
	            FROM RankedResults rr
	            LEFT JOIN ProjectPartEntries pe ON rr.rn = 1 AND rr.partID = pe.partID
	            LEFT JOIN FolderEntries fe ON rr.rn = 1 AND rr.partID = fe.partID
	            WHERE rr.rn = 1 and (projectid is not null or folderid is not null)
            ),
            ProjectHierarchy AS (
                SELECT 
                    f.id, 
                    f.parentId, 
                    CAST(ISNULL(f.Name_LOC, '') AS NVARCHAR(MAX)) AS Path,
                    p.id AS ProjectId,
                    p.Name_LOC_Extracted AS ProjectName,
                    CAST(CONCAT(p.Name_LOC_Extracted, '</split>', CAST(f.Name_LOC AS NVARCHAR(MAX))) AS NVARCHAR(MAX)) AS FullPath,
		            p.id as init,
		            partPath, partInit, partName, datarow.partID
                FROM 
                (
		            select id as projectID, null as partPath, null as partInit, null as partName, null as partID from Projects 
                    where Name_LOC_Extracted LIKE '%{search[0]}%' and Deleted is null {(string.IsNullOrEmpty(search[2]) == false ? " and " + search[2] : "") }
		            union all select projectID as projectID, Path as partPath, init as partInit, name as partName, partID as partID from PartResult
	            ) as dataRow
                left JOIN Projects p ON dataRow.projectID = p.Id  -- folderID와 ProjectId를 연결
	            left join Folders f on p.FolderId = f.id
	            where p.id is not null
            ),
            ProjectInterface as 
            (
	            SELECT 
                init,
                Max(len(FullPath)) as ProjectPath
                FROM ProjectHierarchy fh
                GROUP BY init
            ),
             ProjectFinalResult AS (
                SELECT 
	            fh.id as FolderID,
                ProjectId, ProjectName,ProjectPath,
	            partInit, partName, partPath, partID
    
                FROM ProjectHierarchy fh
	            right join ProjectInterface as p on fh.init = p.init
            ),
            FolderHierarchy AS (
	            SELECT 
		            id, 
		            parentId, 
		            CAST(Name_LOC AS NVARCHAR(MAX)) AS Path,
		            id AS Init,  -- Init 컬럼에 해당 레코드의 id를 초기값으로 설정
		            CAST(Name_LOC AS NVARCHAR(MAX))  as name,

		            dataRow.ProjectId,
		            dataRow.ProjectName,
		            dataRow.ProjectPath,
		            dataRow.Path as partPath, dataRow.init as partInit, dataRow.name as partName, dataRow.partID as partID
		
	            FROM 
	                (
		            select id as folderid, null as ProjectId,null as ProjectName,null as ProjectPath, null as init, null as name, null as Path,null as  partID  from Folders 
                    where CAST(Name_LOC AS NVARCHAR(MAX)) like '%{search[0]}%' and Deleted is null {(string.IsNullOrEmpty(search[3]) == false ? " and " + search[3] : "") }
		            union all select folderID, null as ProjectId,null as ProjectName,null as ProjectPath, init, name, Path, partID from PartResult where projectID is null
		            union all select * from ProjectFinalResult
	            ) as dataRow
	
	            left join Folders on Folders.id = dataRow.folderid
	            UNION ALL

	            SELECT 
		            f.id, 
		            f.parentId, 
		             CAST(Name_LOC AS NVARCHAR(MAX))+'</split>' + fh.Path ,
		            fh.Init,  -- 상위 레코드의 Init 값을 그대로 사용
		            fh.name,  -- 상위 레코드의 Init 값을 그대로 사용

		            fh.ProjectId,
		            fh.ProjectName,
		            fh.ProjectPath,
		            fh.partPath,
		            fh.partInit,
		            fh.partName,
		            fh.partID		
	            FROM 
		            Folders f
	            INNER JOIN FolderHierarchy fh ON f.id = fh.parentId
	            where f.ParentId is not null
	            ),
            FolderInterface AS (
                SELECT 
                    *,
                    ROW_NUMBER() OVER (PARTITION BY partInit, ProjectId, init ORDER BY LEN(path) DESC) AS rn
                FROM FolderHierarchy
            )
            SELECT 
                partID,
                partName,
                partInit,
                partPath,
                ProjectId,
                ProjectName,
                ProjectPath,
                Init as folderid,
                name as folderName,
                p.Path as folderPath
            FROM 
                FolderInterface as p
            where rn = 1";

            System.Data.DataTable dt = global_DB.MutiSelect(query, (int)global_DB.connDB.PCMDB);
            //List<string> list = new List<string>();
            var imageMap = new Dictionary<string, int>
                {
                    {"part", 3},
                    {"project", 2},
                    {"folder", 0}
                };
            // 원하는 언어 순서
            List<string> desiredLanguages = new List<string>() { "en-US", "ko-KR", "ru-RU", "ja-JP", "pt-BR", "de-DE" };

            foreach (DataRow row in dt.Rows)
            {
                string id = "", name = "", path = "";
                int imageIndex = 0;

                if (row["partInit"] != DBNull.Value)
                {
                    id = $"&{row["partInit"]}";
                    path = $@"{row["folderPath"]}</split>{(row["ProjectId"] != DBNull.Value ? $" {row["ProjectName"]}</split>" : "")} {row["partPath"]}";
                    imageIndex = imageMap["part"];
                }
                else if (row["ProjectId"] != DBNull.Value)
                {
                    id = $@"p&{row["ProjectId"]}";
                    path = $@"{row["folderPath"]}</split> {row["ProjectName"]}";
                    imageIndex = imageMap["project"];
                }
                else
                {
                    id = $@"f&{row["folderid"]}";
                    path = $@"{row["folderPath"]}";
                    imageIndex = imageMap["folder"];
                }

                string delimiter = "</split>";
                string[] xmlFiles = path.Split(new string[] { delimiter }, StringSplitOptions.None); ;
                string newPath = "";

                for (int i = 0; i < xmlFiles.Length; i++)
                {
                    string xmlString = xmlFiles[i];
                    try
                    {
                        XDocument doc = XDocument.Parse(xmlString);

                        var translations = doc.Descendants("value")
                                           .OrderBy(v =>
                                           {
                                               string lang = (string)v.Attribute("lang");
                                               return lang == null ? int.MaxValue : desiredLanguages.IndexOf(lang);
                                           })
                                           .ToDictionary(v => (string)v.Attribute("lang") ?? string.Empty, v => (string)v);

                        foreach (var lang in desiredLanguages)
                        {
                            if (translations.ContainsKey(lang))
                            {
                                newPath += $"{translations[lang]}\\";
                                if (i == xmlFiles.Length - 1) name = $"{translations[lang]}";
                                break;
                            }
                        }
                    }
                    catch
                    {
                        newPath += $"{xmlString}\\";
                        if (i == xmlFiles.Length - 1) name = $"{xmlString}";
                    }
                }

                Image icon = image.Images[imageIndex];

                dgv.Rows.Add(id, icon, name, newPath.Remove(newPath.Length - 1));
            }
        }
    }
}
