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
        string sheetName;
        public string ExcelOpen()
        {
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

                if (sheetSelect.ShowDialog() == DialogResult.Cancel)
                    return null;
                string sheetName = sheetSelect.ReturnValue1;

                Excel.Worksheet worksheet = workBook.Worksheets.Item[sheetName];

                LoadData(worksheet, sheetName);

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
                    //변경점 저장 안하면서 닫기
                    workBook.Close(false);
                    //Excel 프로그램 종료
                    application.Quit();
                    //오브젝트 해제1
                    ExcelCommon.ReleaseExcelObject(workBook);
                    ExcelCommon.ReleaseExcelObject(application);
                }
            }
        }
        public void LoadData(Excel.Worksheet worksheet, string sheetName)
        {
            object[,] xlRng = worksheet.UsedRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);
            string[] keys = new string[xlRng.GetUpperBound(1) + 1];
            int firstCol = 0, firstRow = 0, cosumRateIndex = 0;

            for (firstRow = xlRng.GetLowerBound(0); firstRow <= xlRng.GetUpperBound(0); firstRow++)
            {
                for (int col = xlRng.GetLowerBound(1); col <= xlRng.GetUpperBound(1); col++)
                {
                    if (xlRng[firstRow, col] == null)
                        continue;
                    if (firstCol == 0)
                        firstCol = col;

                    keys[col] = xlRng[firstRow, col]?.ToString();
                    if (keys[col].Contains("전력소비율"))
                        cosumRateIndex = col;
                }
                if (!keys.All(x => x == null)) break;
            }

            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();
            List<List<string>> cosumRateList = new List<List<string>>();
            for (int row = firstRow + 2; row <= xlRng.GetUpperBound(0); row++)
            {
                var rowData = new Dictionary<string, string>();
                for (int col = xlRng.GetLowerBound(1); col <= xlRng.GetUpperBound(1); col++)
                {
                    string key = keys[col - xlRng.GetLowerBound(1)];
                    if (key != null)
                    {
                        if (key.Contains("설비명"))
                        {
                            rowData["기계명"] = xlRng[row, col - xlRng.GetLowerBound(1)] == null ? null : xlRng[row, col - xlRng.GetLowerBound(1)].ToString();
                        }
                        rowData[key] = xlRng[row, col - xlRng.GetLowerBound(1)] == null ? null : xlRng[row, col - xlRng.GetLowerBound(1)].ToString();
                    }
                }
                values.Add(rowData); // 행 데이터를 리스트에 추가

                List<string> minMaxAvg = new List<string>();
                for (int i = cosumRateIndex; i < cosumRateIndex + 3; i++)
                {
                    minMaxAvg.Add(xlRng[row, i].ToString());
                }
                cosumRateList.Add(minMaxAvg);
            }

            for (int i=0; i<values.Count; i++)
            {
                MakePostData(values[i], cosumRateList[i], sheetName);
            }
        }
        public void MakePostData(Dictionary<string, string> keyValues, List<string> cosumRateList, string sheetName)
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/MasterData/Import";
            string err = null;

            JArray category = new JArray();
            JObject item = new JObject();

            string[] dArray = { "Cycle time", "Cavity", "Set - up time", "Utilization ratio" };
            string[] rArray = { "Number of workers" };
            string[] aArray = { "기계명", "공정 형태", "Number of Machine", "Attended system", "설비가", "설치면적", "전력량", "전력소비율", "설비내용년수", "수선/개조비", "설비구분" };

            string[] allArray = dArray.Concat(rArray).Concat(aArray).ToArray();

            item.Add("품번", "");
            item.Add("품명", sheetName);
            item.Add("Assembly level", 1);
            //item.Add("Line Type", "M");
            //category.Add(item);
            category.Add(MakeJArray(keyValues, item, allArray, null, "M"));

            item = new JObject();
            item.Add("품번", "10");
            item.Add("품명", sheetName);
            item.Add("Assembly level", 2);

            JObject item2 = (JObject)item.DeepClone();
            item2.Add("기계명",keyValues["설비명"]);
            
            category.Add(MakeJArray(keyValues, item, allArray, dArray, "D"));
            category.Add(MakeJArray(keyValues, item, allArray, rArray, "R"));
            category.Add(MakeJArray(keyValues, item2, allArray, aArray, "A"));

            JObject postData = new JObject
                {
                    { "Data", category },
                    { "ConfigurationGuid", "6abf068a-2bc8-49fa-90b4-4e794c686b6e" }
                };
            err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
        }

        public JObject MakeJArray(Dictionary<string, string> keyValues, JObject item, string[] All, string[] array, string LineType)
        {
            JObject item2 = (JObject)item.DeepClone();

            item2.Add("Line Type", LineType);
            try
            {
                foreach (var a in keyValues)
                {
                    //if (All.Contains(a.Key))
                    //{

                    //}
                    if (array != null && array.Contains(a.Key))
                        item2.Add(a.Key, a.Value);
                    else
                        item2.Add(a.Key, null);
                }
            }
            catch
            {

            }
            return item2;
        }
    }
}
