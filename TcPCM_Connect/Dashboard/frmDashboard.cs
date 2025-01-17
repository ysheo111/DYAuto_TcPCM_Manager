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
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

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
            List<string> query = new List<string>() { searchButton1.text, folderQuery, projectQuery, partQuery };
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
            string err = bom.ExportPartBom(selectItem, dialog.FileName, mode);
            if (err != null) CustomMessageBox.RJMessageBox.Show($"저장을 실패하였습니다\n{err}", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else CustomMessageBox.RJMessageBox.Show("저장이 완료 되었습니다.", "부품원가계산서", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //dialog.FileName

        }

        private void eXCEL올리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PartExcel import = new PartExcel();
            Dictionary<string, string> id = GetTargetTypeID();
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

        private Dictionary<string, string> GetTargetTypeID()
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

        private void rjButton1_Click(object sender, EventArgs e)
        {
            string folderPath = @"\\172.31.15.23\DYA PCM Project\0. 공통\2.기준정보\4. 연구소\대표 차종 EPL"; // 엑셀 파일이 있는 폴더 경로
            string[] filePaths = Directory.GetFiles(folderPath, "*.xls"); // .xlsx 파일만 가져오기

            string targetColumnName = "재질"; // 찾고자 하는 이름
            List<string> materialData = new List<string>();
            List<string> file = new List<string>();
            List<string> part = new List<string>();
            foreach (string filePath in filePaths)
            {
                Application excelApp = new Application();
                Workbook workbook = excelApp.Workbooks.Open(filePath);
                Worksheet worksheet = workbook.Worksheets[1]; // 첫 번째 시트

                Range usedRange = worksheet.UsedRange;
                int lastRow = usedRange.Rows.Count;
                int targetColumnIndex = 0; // 찾고자 하는 열의 인덱스
                int targetColumnIndex2 = 0; // 찾고자 하는 열의 인덱스

                // "재질"이라는 이름이 있는 열 찾기
                for (int col = 1; col <= usedRange.Columns.Count; col++)
                {
                    string header = Convert.ToString(usedRange.Cells[2, col].Value);
                    if (header == targetColumnName)
                    {
                        targetColumnIndex = col;
                    }
                    else if (header == "품번")
                    {
                        targetColumnIndex2 = col;
                    }
                }

                // 찾은 열의 모든 행의 내용 추출
                if (targetColumnIndex > 0)
                {
                    
                    for (int row = 3; row <= lastRow; row++)
                    {
                        string material = Convert.ToString(usedRange.Cells[row, targetColumnIndex].Value);
                        string material2 = Convert.ToString(usedRange.Cells[row, targetColumnIndex2].Value);
                        if (material.Length <= 0) continue;
                        file.Add(filePath.Split('\\')[filePath.Split('\\').Length-1]);
                        materialData.Add(material);
                        part.Add(material2);
                    }
                    // CSV 파일로 저장 예시
                    // using (StreamWriter sw = new StreamWriter("output.csv"))
                    // {
                    //     foreach (string material in materialData)
                    //     {
                    //         sw.WriteLine(material);
                    //     }
                    // }
                }
                else
                {
                    Console.WriteLine($"파일 {filePath}에서 '{targetColumnName}' 열을 찾을 수 없습니다.");
                }

                workbook.Close(false);
                excelApp.Quit();

                //MessageBox.Show(filePath.Split('\\')[filePath.Split('\\').Length - 1]);
            }

            // 새로운 엑셀 파일 생성
            string newFilePath = $@"{folderPath}\extracted_materials2.xlsx";
            Application newExcelApp = new Application();
            Workbook newWorkbook = newExcelApp.Workbooks.Add();
            Worksheet newWorksheet = newWorkbook.Worksheets.Add();
            Worksheet newWorksheet2 = newWorkbook.Worksheets.Add();

            // 추출된 데이터를 새로운 엑셀 파일에 기입
            
            for(int i=0;i< materialData.Count;i++)
            {
                newWorksheet.Cells[i+1, 1] = file[i];
                newWorksheet.Cells[i+1, 2] = part[i];
                newWorksheet.Cells[i + 1, 3] = materialData[i];
            }

            int rowIndex = 1;
            foreach (string material in materialData.Distinct())
            {
                newWorksheet2.Cells[rowIndex, 1] = material;
                rowIndex++;
            }

            // 새로운 엑셀 파일 저장
            newWorkbook.SaveAs(newFilePath);
            newWorkbook.Close();
            newExcelApp.Quit();

            Console.WriteLine($"재질 데이터가 {newFilePath} 파일에 저장되었습니다.");
        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            string folderPath = @"D:\309. DY\2. 진행자료\3. 기준정보\원소재 검증3.xlsx"; // 엑셀 파일이 있는 폴더 경로
          
            string targetColumnName = "재질"; // 찾고자 하는 이름
            List<string> materialData = new List<string>();
            List<string> materialData2 = new List<string>();
            List<string> file = new List<string>();
            List<string> part = new List<string>();

            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(folderPath);
            List<string> worksheets = new List<string>() { "WIPER", "ELTT", "CFM", "PWM" };
            foreach (var sheetName in worksheets)
            {
                Worksheet worksheet = workbook.Worksheets.get_Item(sheetName); // 첫 번째 시트
                Range usedRange = worksheet.UsedRange;
                int lastRow = usedRange.Rows.Count;
                int targetColumnIndex = 0; // 찾고자 하는 열의 인덱스
                int targetColumnIndex2 = 0; // 찾고자 하는 열의 인덱스
                int targetColumnIndex3 = 0; // 찾고자 하는 열의 인덱스

                // "재질"이라는 이름이 있는 열 찾기
                for (int col = 1; col <= usedRange.Columns.Count; col++)
                {
                    string header = Convert.ToString(usedRange.Cells[2, col].Value);
                    string header2 = Convert.ToString(usedRange.Cells[1, col].Value);
                    string header3 = Convert.ToString(usedRange.Cells[3, col].Value);
                    if (header == targetColumnName|| header2 == targetColumnName|| header3 == targetColumnName)
                    {
                        targetColumnIndex = col;
                    }
                    else if (header == "품번"|| header2 == "품번"|| header3 == "품번")
                    {
                        targetColumnIndex2 = col;
                    }
                    else if (header == "품명" || header2 == "품명" || header3 == "품명")
                    {
                        targetColumnIndex3 = col;
                    }
                }

                // 찾은 열의 모든 행의 내용 추출
                if (targetColumnIndex3 > 0)
                {

                    for (int row = 3; row <= lastRow; row++)
                    {
                        string material = Convert.ToString(usedRange.Cells[row, targetColumnIndex].Value);
                        string material2 = Convert.ToString(usedRange.Cells[row, targetColumnIndex2].Value);
                        string material3 = Convert.ToString(usedRange.Cells[row, targetColumnIndex3].Value);
                        if (material3 == null|| material3.Length <= 0) continue;
                        file.Add(worksheet.Name);
                        if (material != null)  materialData.Add(material);
                        else materialData.Add("");
                        string[] strings = material3.Split(',');

                        var filteredStrings = strings.Where(s => s.Trim().ToLower().StartsWith("x"));
                        string test = "x";
                        if (filteredStrings.Any())
                        {
                            test = filteredStrings.FirstOrDefault();
                        }
                        else
                        {
                            Console.WriteLine("x로 시작하는 요소가 없습니다.");
                        }

                        materialData2.Add(test.Substring(1));
                        part.Add(material2);
                    }
                    // CSV 파일로 저장 예시
                    // using (StreamWriter sw = new StreamWriter("output.csv"))
                    // {
                    //     foreach (string material in materialData)
                    //     {
                    //         sw.WriteLine(material);
                    //     }
                    // }
                }
                else
                {
                    //Console.WriteLine($"파일 {filePath}에서 '{targetColumnName}' 열을 찾을 수 없습니다.");
                }



                //MessageBox.Show(filePath.Split('\\')[filePath.Split('\\').Length - 1]);
            }
            workbook.Close(false);
            excelApp.Quit();
            // 새로운 엑셀 파일 생성
            string newFilePath = $@"D:\309. DY\2. 진행자료\3. 기준정보\원소재 검증5.xlsx";
            Application newExcelApp = new Application();
            Workbook newWorkbook = newExcelApp.Workbooks.Add();
            Worksheet newWorksheet = newWorkbook.Worksheets.Add();
            Worksheet newWorksheet2 = newWorkbook.Worksheets.Add();
            Worksheet newWorksheet3 = newWorkbook.Worksheets.Add();

            // 추출된 데이터를 새로운 엑셀 파일에 기입

            for (int i = 0; i < materialData.Count; i++)
            {
                newWorksheet.Cells[i + 1, 1] = file[i];
                newWorksheet.Cells[i + 1, 2] = part[i];
                newWorksheet.Cells[i + 1, 3] = materialData[i];
                newWorksheet.Cells[i + 1, 4] = materialData2[i];
            }

            int rowIndex = 1;
            foreach (string material in materialData2.Distinct())
            {
                newWorksheet2.Cells[rowIndex, 1] = material;
                rowIndex++;
            }

            rowIndex = 1;
            foreach (string material in materialData.Distinct())
            {
                newWorksheet3.Cells[rowIndex, 1] = material;
                rowIndex++;
            }

            // 새로운 엑셀 파일 저장
            newWorkbook.SaveAs(newFilePath);
            newWorkbook.Close();
            newExcelApp.Quit();

            Console.WriteLine($"재질 데이터가 {newFilePath} 파일에 저장되었습니다.");
        }
        private void rjButton3_Click(object sender, EventArgs e)
        {
            string folderPath = @"D:\102. TcPCM\209. DB연결서버";
            Application excelApp = new Application();
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;

            Workbook workbook = excelApp.Workbooks.Open(folderPath + @"\부품원가계산서_아도_241231.xlsx", ReadOnly: false);

            foreach (Worksheet sheet in workbook.Worksheets)
            {
                // 첫 번째 시트와 마지막 시트 건너뛰기
                if (sheet.Index == 1 || sheet.Index == workbook.Worksheets.Count)
                    continue;

                string newFilePath = Path.Combine(folderPath, $"아도_{sheet.Name}.xlsx");

                // 새 워크북 생성
                Application newExcelApp = new Application();
                Workbook newWorkbook = newExcelApp.Workbooks.Add();

                try
                {
                    // 첫 번째 시트 복사
                    Worksheet firstSheet = (Worksheet)workbook.Worksheets[1];
                    firstSheet.Copy(Before: newWorkbook.Worksheets[1]);

                    // 현재 시트 복사
                    sheet.Copy(Before: newWorkbook.Worksheets[newWorkbook.Worksheets.Count]);

                    // 마지막 시트 복사
                    Worksheet lastSheet = (Worksheet)workbook.Worksheets[workbook.Worksheets.Count];
                    lastSheet.Copy(Before: newWorkbook.Worksheets[newWorkbook.Worksheets.Count]);

                    // 새 워크북 저장 및 닫기
                    newWorkbook.SaveAs(newFilePath);
                    newWorkbook.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing sheet {sheet.Name}: {ex.Message}");
                }
                finally
                {
                    newExcelApp.Quit();
                    ReleaseExcelObject(newWorkbook);
                    ReleaseExcelObject(newExcelApp);
                }
            }

            // 기존 워크북 닫기
            workbook.Close(false);
            excelApp.Quit();
            ReleaseExcelObject(workbook);
            ReleaseExcelObject(excelApp);
        }
        private void CopySheetContent(Worksheet sourceSheet, Workbook targetWorkbook)
        {
                try
                {
                    // 시트를 새 워크북에 복사
                    sourceSheet.Copy(Before: targetWorkbook.Worksheets[1]);
                Worksheet copiedSheet = (Worksheet)targetWorkbook.Worksheets[1];
                copiedSheet.Name = $"{sourceSheet.Name}_Copy";
                // 복사된 워크시트 가져오copiedSheet.Name = $"{sourceSheet.Name}_Copy";기
                // Worksheet copiedSheet = (Worksheet)targetWorkbook.Worksheets[1];
                //Console.WriteLine($"Sheet '{sourceSheet.Name}' copied successfully as '{copiedSheet.Name}'");
            }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying worksheet: {ex.Message}");
                }
            //try
            //{
            //    // 원본 시트의 사용 범위 가져오기
            //    Range sourceRange = sourceSheet.UsedRange;
            //    int rows = sourceRange.Rows.Count;
            //    int cols = sourceRange.Columns.Count;

            //    // 값 복사
            //    object[,] data = sourceRange.Value2; // 값 배열로 가져오기
            //    Range targetRange = targetSheet.Range["A1"].Resize[rows, cols];
            //    targetRange.Value2 = data; // 값 복사

            //    // 수식 복사
            //    object[,] formulas = sourceRange.Formula; // 수식 배열로 가져오기
            //    targetRange.Formula = formulas; // 수식 복사

            //    // 열 너비와 행 높이 복사
            //    for (int col = 1; col <= cols; col++)
            //    {
            //        targetSheet.Columns[col].ColumnWidth = sourceSheet.Columns[col].ColumnWidth;
            //    }
            //    for (int row = 1; row <= rows; row++)
            //    {
            //        targetSheet.Rows[row].RowHeight = sourceSheet.Rows[row].RowHeight;
            //    }
            //    Console.WriteLine("Sheet content with formulas copied successfully.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error copying sheet content: {ex.Message}");
            //}
            //    try
            //    {
            //        Range sourceRange = sourceSheet.UsedRange;
            //        for (int row = 1; row <= sourceRange.Rows.Count; row++)
            //        {
            //            for (int col = 1; col <= sourceRange.Columns.Count; col++)
            //            {
            //                Range sourceCell = sourceRange.Cells[row, col] as Range;
            //                Range targetCell = targetSheet.Cells[row, col] as Range;

            //                if (sourceCell != null && targetCell != null)
            //                {
            //                    // 복사: 값
            //                    targetCell.Value2 = sourceCell.Value2;

            //                    // 복사: 수식
            //                    targetCell.Formula = sourceCell.Formula;

            //                    // 복사: 서식
            //                    targetCell.Font.Name = sourceCell.Font.Name;
            //                    targetCell.Font.Size = sourceCell.Font.Size;
            //                    targetCell.Font.Bold = sourceCell.Font.Bold;
            //                    targetCell.Font.Italic = sourceCell.Font.Italic;
            //                    targetCell.Font.Underline = sourceCell.Font.Underline;

            //                    targetCell.Interior.Color = sourceCell.Interior.Color;
            //                    targetCell.HorizontalAlignment = sourceCell.HorizontalAlignment;
            //                    targetCell.VerticalAlignment = sourceCell.VerticalAlignment;
            //                    targetCell.NumberFormat = sourceCell.NumberFormat;

            //                    targetCell.Borders.LineStyle = sourceCell.Borders.LineStyle;
            //                    targetCell.Borders.Weight = sourceCell.Borders.Weight;
            //                }
            //            }
            //        }

            //        // 열 너비와 행 높이 복사
            //        for (int col = 1; col <= sourceRange.Columns.Count; col++)
            //        {
            //            targetSheet.Columns[col].ColumnWidth = sourceSheet.Columns[col].ColumnWidth;
            //        }
            //        for (int row = 1; row <= sourceRange.Rows.Count; row++)
            //        {
            //            targetSheet.Rows[row].RowHeight = sourceSheet.Rows[row].RowHeight;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error copying sheet content manually: {ex.Message}");
            //    }
        }
        private void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error releasing object: {ex.Message}");
            }
            finally
            {
                GC.Collect();
            }
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
