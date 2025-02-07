using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace TcPCM_Connect_Global
{
    public class ManufacturingLibrary
    {
        JArray AllCategory = new JArray();

        string[] dArray = { "설비명", "공정 형태", "Cycle time", "Cavity", "Set-up time", "Utilization ratio" };
        string[] rArray = { "설비명", "Number of workers" };
        string[] aArray = { "설비명", "기계명", "Number of Machine", "설비가", "설치면적", "전력량", "전력소비율", "설비내용년수", "수선/개조비", "설비구분" };

        public string ExcelOpen()
        {
            Excel.Worksheet worksheet = null;
            Excel.Application application = null;
            Excel.Workbook workBook = null;
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();

                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel)
                    return null;
                else if (dialog != DialogResult.OK)
                    return "ERROR : 파일 오픈에 실패하였습니다.";

                //Excel 프로그램 실행
                application = new Excel.Application();
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

                if (sheetSelect.ShowDialog() == DialogResult.Cancel)
                    return null;
                if(sheetSelect.ReturnValues == null)
                {
                    worksheet = workBook.Worksheets.Item[sheetSelect.ReturnValue1];

                    LoadData(worksheet, sheetSelect.ReturnValue1);
                }
                else
                {
                    foreach (string returnValue in sheetSelect.ReturnValues)
                    {
                        worksheet = workBook.Worksheets.Item[returnValue];

                        LoadData(worksheet, returnValue);
                    }
                }
                return null;
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(false);

                    application.Quit();

                    ExcelCommon.ReleaseExcelObject(workBook);
                    ExcelCommon.ReleaseExcelObject(application);
                }
            }
        }
        public string LoadData(Excel.Worksheet worksheet, string sheetName)
        {
            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> cosumRateList = new List<Dictionary<string, string>>();
            string err = null;
            try
            {
                int firstCol = 0, firstRow = 0, cosumRateIndex = 0;

                Excel.Range usedRange = worksheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                int colCount = usedRange.Columns.Count;

                string[] keys = new string[colCount + 1];

                #region 헤더 만들기
                for (int row = 1; row <= rowCount; row++)
                {
                    for (int col = 1; col <= colCount; col++)
                    {
                        object headerValue = usedRange.Cells[row,col]?.Value2;
                        if (dArray.Contains(headerValue) || rArray.Contains(headerValue) || aArray.Contains(headerValue))
                        {
                            firstRow = row;
                            break;
                        }
                    }
                    if (firstRow != 0)
                        break;
                }
                for (int row = firstRow; row <= rowCount; row++)
                {
                    for (int col = 1; col <= colCount; col++)
                    {
                        object cellValue = usedRange.Cells[row, col]?.Value2;
                        if (cellValue == null)
                            continue;
                        if (firstCol == 0)
                            firstCol = col;

                        keys[col] = usedRange.Cells[row, col]?.Value2;
                    }
                    if (!keys.All(x => x == null)) break;
                }
                #endregion

                #region 값 정리하기(values, cosumRateList)
                for (int row = firstRow + 2; row <= rowCount; row++)
                {
                    bool flag = true;
                    var rowData = new Dictionary<string, string>();
                    string name = null;
                    for (int col = 1; col <= colCount; col++)
                    {
                        string key = keys[col];
                        object cellValue = usedRange.Cells[row, col]?.Value2;
                        if (key != null)
                        {
                            if (key.Contains("설비명"))
                            {
                                name = cellValue?.ToString();
                                if (name == null || name.Contains("예시"))
                                {
                                    flag = false;
                                    break;
                                }
                                rowData["기계명"] = name;
                            }
                            if (keys[col].Contains("전력소비율"))
                            {
                                cosumRateIndex = col;
                                if (Convert.ToDouble(cellValue) > 1)
                                {
                                    rowData[key] = cellValue == null ? null : (Convert.ToDouble(cellValue) / 100).ToString();
                                    continue;
                                }
                            }
                            rowData[key] = cellValue == null ? null : cellValue.ToString();
                        }
                    }
                    if (flag)
                    {
                        values.Add(rowData);

                        string abc = usedRange.Cells[firstRow + 1, cosumRateIndex]?.Value2.ToString();
                        if (abc != null && (abc.Contains("MIN") || abc.Contains("MAX") || abc.Contains("AVG")))
                        {
                            Dictionary<string, string> minMaxAvg = new Dictionary<string, string>();
                            for (int i = cosumRateIndex; i < cosumRateIndex + 3; i++)
                            {
                                if (Convert.ToDouble(usedRange.Cells[row, i]?.Value2) > 1)
                                    minMaxAvg.Add(usedRange.Cells[firstRow + 1, i]?.Value2.ToString(), (Convert.ToDouble(usedRange.Cells[row, i]?.Value2) / 100).ToString());
                                else
                                    minMaxAvg.Add(usedRange.Cells[firstRow + 1, i]?.Value2.ToString(), usedRange.Cells[row, i]?.Value2.ToString());
                            }
                            cosumRateList.Add(minMaxAvg);
                        }
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(name))
                            break;
                    }
                }
                #endregion

                AllCategory.Merge(MakePostData(sheetName, values, cosumRateList));
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return err;
        }

        public JArray MakePostData(string sheetName, List<Dictionary<string, string>> values, List<Dictionary<string, string>> cosumRateList)
        {
            string[] allArray = dArray.Concat(rArray).Concat(aArray).ToArray();

            JArray category = new JArray();
            JObject item = new JObject();

            bool partPlag = false;
            try
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i]["설비명"].Contains("예시"))
                        continue;
                    if (!partPlag)
                    {
                        item.Add("품번", "");
                        item.Add("품명", sheetName);
                        item.Add("Assembly level", 1);
                        category.Add(MakeJArray(values[i], item, allArray, null, "M"));
                        partPlag = true;
                    }
                    item = new JObject();
                    item.Add("품번", i+1);
                    item.Add("품명", sheetName);
                    item.Add("Assembly level", 2);
            
                    category.Add(MakeJArray(values[i], item, allArray, dArray, "D"));
                    if(cosumRateList.Count > 0)
                    {
                        foreach (var cosumRate in cosumRateList[i])
                        {
                            values[i]["기계명"] = values[i]["기계명"] + cosumRate.Key;
                            values[i]["전력소비율 (%)"] = cosumRate.Value;
                            category.Add(MakeJArray(values[i], item, allArray, aArray, "A"));
                            values[i]["기계명"] = values[i]["기계명"].Replace(cosumRate.Key, "");
                        }
                    }
                    else
                        category.Add(MakeJArray(values[i], item, allArray, aArray, "A"));

                    category.Add(MakeJArray(values[i], item, allArray, rArray, "R"));
                }
            }
            catch
            {
                return null;
            }
            return category;
        }

        public JObject MakeJArray(Dictionary<string, string> keyValues, JObject item, string[] All, string[] array, string LineType)
        {
            JObject item2 = (JObject)item.DeepClone();

            item2.Add("Line Type", LineType);
            foreach (var data in keyValues)
            {
                if (All.Any(serach => data.Key.Contains(serach)))
                {
                    if (array != null && array.Any(serach => data.Key.Contains(serach)))
                        item2.Add(data.Key, data.Value);
                    else
                        item2.Add(data.Key, null);
                }
            }
            return item2;
        }

        public string Import(string className, string targetType, string targetId)
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Import";
            string err = null;

            JObject postData = new JObject
                    {
                        {  "Data", AllCategory },
                        {  "ConfigurationGuid", global_iniLoad.GetConfig(className, "공정Import")},
                        {  "TargetType", targetType },
                        {  "TargetId", targetId }
                    };
            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);

            return err;
        }
    }
}
