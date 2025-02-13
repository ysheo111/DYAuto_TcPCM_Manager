using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Data;

namespace TcPCM_Connect_Global
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcelImport
    {
        HashSet<string> incorrect = new HashSet<string>();
        public CBD cbd = new CBD();
        public ExcelImport()
        {
            incorrect = new HashSet<string>();
            cbd = new CBD();
        }

        private string CombineString(int nameRow, int rowCol, int colCol, Excel.Worksheet worksheet)
        {
            string name = "";
            for (int i = 0; i < nameRow; i++)
            {
                name += cbd.CleanString(worksheet.Cells[rowCol + i, colCol].Value?.ToString());
            }
            return name;
        }

        public void CellVaildation(string colName, int nameRow, int rowCol, int colCol, int rowValue, int colValue, Excel.Worksheet worksheet, ref JObject value, int cbd2)
        {
            string name = CombineString(nameRow, rowCol, colCol, worksheet);
            if (!name.Replace("개발비용",Report.Summary.rnd).Replace("전략","전력").Contains(cbd.FindValue(cbd2, colCol)))
            {
                incorrect.Add($"{cbd.FindValue(cbd2, colCol)}을 다시 확인해주세요. ({name} {rowCol} {colCol})");
                //(worksheet.Cells[row, excelCol]).Interior.Color = Excel.XlRgbColor.rgbYellow;
            }
            else value.Add(colName, $"{worksheet.Cells[rowValue, colValue].Value}");
        }

        public void CellVaildation(string colName, int nameRow, int rowCol, int colCol, Excel.Worksheet worksheet, object data, ref JObject value, int cbd2)
        {
            string name = CombineString(nameRow, rowCol, colCol, worksheet);
            if (!name.Contains(cbd.FindValue(cbd2, colCol)))
            {
                incorrect.Add($"{cbd.FindValue(cbd2, colCol)}을 다시 확인해주세요. ({rowCol} {colCol})");
                //(worksheet.Cells[row, excelCol]).Interior.Color = Excel.XlRgbColor.rgbYellow;
            }
            else value.Add(colName, JToken.FromObject(data));
        }
        public void CellVaildationDT(string colName, int nameRow, int rowCol, int colCol, int rowValue, int colValue, Excel.Worksheet worksheet, ref DataTable value, int cbd2)
        {
            string name = CombineString(nameRow, rowCol, colCol, worksheet);
            if (!name.Replace("개발비용", Report.Summary.rnd).Replace("전략", "전력").Contains(cbd.FindValue(cbd2, colCol)))
            {
                incorrect.Add($"{cbd.FindValue(cbd2, colCol)}을 다시 확인해주세요. ({name} {rowCol} {colCol})");
                //(worksheet.Cells[row, excelCol]).Interior.Color = Excel.XlRgbColor.rgbYellow;
            }
            else
            {
                if (!value.Columns.Contains(colName)) value.Columns.Add(colName);
                value.Rows[value.Rows.Count - 1][colName] = $"{worksheet.Cells[rowValue, colValue].Value}";
            }
        }

        public void CellVaildationDT(string colName, int nameRow, int rowCol, int colCol, Excel.Worksheet worksheet, object data, ref DataTable value, int cbd2)
        {
            string name = CombineString(nameRow, rowCol, colCol, worksheet);
            if (!name.Contains(cbd.FindValue(cbd2, colCol)))
            {
                incorrect.Add($"{cbd.FindValue(cbd2, colCol)}을 다시 확인해주세요. ({rowCol} {colCol})");
                //(worksheet.Cells[row, excelCol]).Interior.Color = Excel.XlRgbColor.rgbYellow;
            }
            else
            {
                if (!value.Columns.Contains(colName)) value.Columns.Add(colName);
                value.Rows[value.Rows.Count - 1][colName] = data;
            }
        }

        /// <summary>
        /// 
        /// </summary>


        public List<Dictionary<string, object>> AllImport(string worksheetName)
        {
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;

            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return null;
                else if (dlg.ShowDialog() != DialogResult.OK)
                {
                    items.Add(new Dictionary<string, object>() { { "error", "파일 오픈에 실패하였습니다." } });
                    return items;
                }

                //Excel 프로그램 실행
                application = new Microsoft.Office.Interop.Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일로부터 불러오기
                workBook = application.Workbooks.Open(dlg.FileName);

                List<string> workSheetList = new List<string>();
                foreach (Excel.Worksheet sheet in workBook.Worksheets)
                {
                    if (sheet.Visible != Excel.XlSheetVisibility.xlSheetVisible) continue;
                    workSheetList.Add(sheet.Name);
                }
                workSheetSelect sheetSelect = new workSheetSelect();
                sheetSelect.workSheet = workSheetList;

                if (sheetSelect.ShowDialog() == DialogResult.Cancel) return null;
                string val = sheetSelect.ReturnValue1;

                Excel.Worksheet worksheet = workBook.Worksheets.Item[val];
                //Excel.Range xlRng = worksheet.UsedRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

                object[,] xlRng = worksheet.UsedRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

                List<string> keys = new List<string>();
                int firstCol = 0, firstRow;
                for (firstRow = xlRng.GetLowerBound(0); firstRow <= xlRng.GetUpperBound(0); firstRow++)
                {
                    for (int col = xlRng.GetLowerBound(1); col <= xlRng.GetUpperBound(1); col++)
                    {
                        if (xlRng[firstRow, col]?.ToString().Length == 0) continue;
                        if (firstCol == 0) firstCol = col;

                        keys.Add(xlRng[firstRow, col]?.ToString());
                    }

                    if (keys.Count > 0) break;
                }

                for (int row = firstRow + 1; firstRow <= xlRng.GetUpperBound(0); row++)
                {
                    Dictionary<string, object> rowInfo = new Dictionary<string, object>();
                    bool emptyCheck = false;
                    for (int col = firstCol; col < keys.Count; col++)
                    {
                        if (xlRng[row, col] != null) emptyCheck = true;
                        rowInfo.Add(keys[col], xlRng[row, col]?.ToString());
                    }
                    if (emptyCheck) items.Add(rowInfo);
                    else break;
                }
            }
            catch (Exception exc)
            {
                items.Insert(0, new Dictionary<string, object>() { { "error", exc.Message } });
                return items;
            }
            finally
            {
                if (workBook != null)
                {
                    //변경점 저장안하고 닫기
                    workBook.Close(false);
                    //Excel 프로그램 종료
                    application.Quit();
                    //오브젝트 해제1
                    ExcelCommon.ReleaseExcelObject(workBook);
                    ExcelCommon.ReleaseExcelObject(application);
                }
            }
            return items;
        }

        public string LoadMasterData(string worksheetName, DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.Rows.Clear();
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;

            try
            {
                OpenFileDialog dlg = new OpenFileDialog();

                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return null;
                else if (dialog != DialogResult.OK) return "ERROR : 파일 오픈에 실패하였습니다.";

                //Excel 프로그램 실행
                application = new Microsoft.Office.Interop.Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = false;
                //파일로부터 불러오기
                workBook = application.Workbooks.Open(dlg.FileName);

                List<string> workSheetList = new List<string>();
                foreach (Excel.Worksheet sheet in workBook.Worksheets)
                {
                    if (sheet.Visible == Excel.XlSheetVisibility.xlSheetVisible) workSheetList.Add(sheet.Name);
                }
                workSheetSelect sheetSelect = new workSheetSelect();
                sheetSelect.workSheet = workSheetList;

                if (sheetSelect.ShowDialog() == DialogResult.Cancel) return null;
                //string val = sheetSelect.ReturnValue1;

                //Excel.Worksheet worksheet = workBook.Worksheets.Item[val];
                Excel.Worksheet worksheet = null;
                List<string> resultValueList = new List<string>();

                if (sheetSelect.ReturnValues == null)
                    resultValueList.Add(sheetSelect.ReturnValue1);
                else
                    resultValueList = sheetSelect.ReturnValues;

                foreach (string returnValue in resultValueList)
                {
                    worksheet = workBook.Worksheets.Item[returnValue];

                    object[,] xlRng = worksheet.UsedRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

                    string[] keys = new string[xlRng.GetUpperBound(1) + 1];
                    int firstCol = 0, firstRow;
                    for (firstRow = xlRng.GetLowerBound(0); firstRow <= xlRng.GetUpperBound(0); firstRow++)
                    {
                        for (int col = xlRng.GetLowerBound(1); col <= xlRng.GetUpperBound(1); col++)
                        {
                            if (xlRng[firstRow, col] == null) continue;
                            if (firstCol == 0) firstCol = col;
                        
                            string key = xlRng[firstRow, col]?.ToString();

                            if (key == "나라")
                                key = "지역";
                            else if (key == "재료명")
                                key = "재질명";
                            else if (key == "segmant")
                                key = "업종";

                            if (worksheetName == "Machine" && key == "OTHER UTILITY")
                            {
                                key = "기타비용";
                                keys[col + 1] = "내용년수";
                            }

                            if (worksheetName == "재료관리비")
                            {
                                if (key.Contains("재료") && key.Contains("Loss") && key.Contains("율"))
                                    key = "재료 관리비율";
                            }
                            if (worksheetName == "Machine" && (key.Contains("설비가") || key.Contains("사양") || key.Contains("기계상각년수") || key.Contains("설치면적") || key.Contains("전력")))
                            {
                                int index = key.IndexOf('(');
                                if(index != -1)
                                    key = key.Substring(0,index).Trim();
                            }

                            if (!keys.Any(item => item == key))// && keys[col] == null)
                                keys[col] = key;
                            //keys[col] = key;
                        }

                        if (!keys.All(x => x == null))
                        {
                            if (keys.Any(colName => colName != null && dgv.Columns.Contains(colName)))
                                break;
                        }
                    }

                    for (int row = firstRow + 1; row <= xlRng.GetUpperBound(0); row++)
                    {
                        dgv.Rows.Add();
                        bool flag = false;
                        foreach (DataGridViewColumn col in dgv.Columns)
                        {
                            for (int category = xlRng.GetLowerBound(0); category < keys.Length; category++)
                            {
                                if (keys[category] == null) continue;
                                if (keys[category].Contains(col.Name) && !keys.Where(key => key != keys[category]).Any(key => key == col.Name)) //!keys.Where(key => key != keys[category]).Contains(col.Name))
                                {
                                    if (xlRng[row, category] == null)
                                    {
                                        dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Style.BackColor = Color.Red;
                                        continue;
                                    }

                                    bool dateTypeCheck = global.TryFormat("{####-##-##}", out string resultValue, xlRng[row, category].ToString());
                                    if ((col.Name.ToLower().Contains("valid") && !dateTypeCheck) || (!col.Name.ToLower().Contains("valid") && dateTypeCheck))
                                    {
                                        dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Style.BackColor = Color.Yellow;
                                    }

                                    if(col.Name.ToLower().Contains("valid") && !dateTypeCheck)
                                    {
                                        flag = false;
                                        break;
                                    }
                                    
                                    flag = true;

                                    if (resultValue.Contains("CO2e/kg"))
                                        resultValue = resultValue.Replace("CO2e/kg","").Trim();
                                    if(worksheetName == "임률" && col.Name == "지역")
                                        dgv.Rows[dgv.Rows.Count - 1].Cells["Plant"].Value = resultValue;
                                    else if (worksheetName == "표준 공정")
                                    {
                                        if(col.Name == "기계명" && resultValue != null)
                                            dgv.Rows[dgv.Rows.Count - 1].Cells["기계명"].Value = resultValue;
                                        else if (col.Name == "톤수" && resultValue != null)
                                            dgv.Rows[dgv.Rows.Count - 1].Cells["톤수"].Value += resultValue;
                                        else if (col.Name == "메이커" && resultValue != null)
                                            dgv.Rows[dgv.Rows.Count - 1].Cells["메이커"].Value += resultValue;
                                        else
                                            dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Value = resultValue;
                                    }
                                    else if (worksheetName == "단가 관리 리스트" && col.Name == "통화")
                                    {
                                        dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Value = resultValue.Trim();
                                    }
                                    else
                                        dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Value = resultValue;

                                    if (new List<string>() { "지역", "공정", "업종", "설비명", "설비구분", "통화", "Valid From", "구분자", "재질명", "소재명", "메이커", "구분", "비중", "비중 단위", "가격 단위", "이름", "ISO", "UOM Code", "UOM 명", "Plant", "GRADE", "단위", "사양 정보", "업체명", "품번", "대표품명", "기계명", "톤수", "메이커", "세부 공정명", "comment", "원재료 단위", "스크랩 단위", "참고품명" }.Contains(col.Name)) continue;

                                    if ((((col.Name.Contains("율") || col.Name.Contains("률")) && !col.Name.Contains("임률") && !col.Name.Contains("환율"))) && !col.Name.Contains("수선비율"))
                                    {
                                        dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Value = global.ConvertDouble(resultValue) * 100;
                                    }
                                    if (!double.TryParse(resultValue, out double resultDouble)) dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Style.BackColor = Color.Yellow;
                                }
                                else if (keys[category] == "임률" && !keys.Where(key => key != keys[category]).Any(key => key == col.Name))
                                {
                                    if (xlRng[row, category] == null)
                                    {
                                        dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Style.BackColor = Color.Red;
                                        continue;
                                    }

                                    bool dateTypeCheck = global.TryFormat("{####-##-##}", out string resultValue, xlRng[row, category].ToString());
                                    if ((col.Name.ToLower().Contains("valid") && !dateTypeCheck) || (!col.Name.ToLower().Contains("valid") && dateTypeCheck))
                                    {
                                        dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Style.BackColor = Color.Yellow;
                                    }

                                    flag = true;
                                    dgv.Rows[dgv.Rows.Count - 1].Cells["직접임률"].Value = global.ConvertDouble(resultValue);
                                    if (!double.TryParse(resultValue, out double resultDouble)) dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Style.BackColor = Color.Yellow;
                                }
                            }
                        }
                        if (!flag) dgv.Rows.RemoveAt(dgv.Rows.Count - 1);
                        else if(dgv.Columns.Contains("Valid From"))
                            if(string.IsNullOrEmpty(dgv.Rows[dgv.Rows.Count - 1].Cells["Valid From"].Value?.ToString())) dgv.Rows.RemoveAt(dgv.Rows.Count - 1);
                    }
                }
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
            finally
            {
                if (workBook != null)
                {
                    //변경점 저장 안하면서 닫기
                    workBook.Close(false);
                    //Excel 프로그램 종료
                    application.Quit();
                    //오브젝트 해제1
                    ExcelCommon.ReleaseExcelObject(workBook);
                    ExcelCommon.ReleaseExcelObject(application);
                }
            }

            dgv.AllowUserToAddRows = true;
            return null;
        }
    }
}
