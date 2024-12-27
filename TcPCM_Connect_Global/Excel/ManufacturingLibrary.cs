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
        List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> cosumRateList = new List<Dictionary<string, string>>();
        string sheetName = null;

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
                    sheetName = sheetSelect.ReturnValue1;

                    worksheet = workBook.Worksheets.Item[sheetName];

                    LoadData(worksheet);
                }
                else
                {
                    foreach (string returnValue in sheetSelect.ReturnValues)
                    {
                        sheetName = returnValue;

                        worksheet = workBook.Worksheets.Item[sheetName];

                        LoadData(worksheet);
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

        public string LoadData(Excel.Worksheet worksheet)//, string sheetname)//string className)
        {
            string err = null;
            try
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

                //List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();
                //List<Dictionary<string, string>> cosumRateList = new List<Dictionary<string, string>>();
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

                    Dictionary<string, string> minMaxAvg = new Dictionary<string, string>();
                    for (int i = cosumRateIndex; i < cosumRateIndex + 3; i++)
                    {
                        minMaxAvg.Add(xlRng[firstRow+1, i].ToString(), xlRng[row, i].ToString());
                    }
                    cosumRateList.Add(minMaxAvg);
                }

                //err = MakePostData(values, cosumRateList, className);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return err;
        }
        public string MakePostData(string className)//, string sheetName)//List<Dictionary<string, string>> values, List<Dictionary<string, string>> cosumRateList,
        {
            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/MasterData/Import";
            string err = null;

            JArray category = new JArray();
            JObject item = new JObject();

            string[] dArray = { "설비명", "공정 형태", "Cycle time", "Cavity", "Set-up time", "Utilization ratio" };
            string[] rArray = { "설비명", "Number of workers" };
            string[] aArray = { "설비명", "기계명", "Number of Machine", @"설비가 (k\)", "설치면적 (㎡)", "전력량 (KWh)", "전력소비율 (%)", "설비내용년수", "수선/개조비", "설비구분" };
            string[] allArray = dArray.Concat(rArray).Concat(aArray).ToArray();

            bool partPlag = false;
            try
            {
                for (int i = 0; i < values.Count; i++)
                {
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
                    foreach(var cosumRate in cosumRateList[i])
                    {
                        values[i]["기계명"] = values[i]["기계명"] + cosumRate.Key;
                        values[i]["전력소비율 (%)"] = cosumRate.Value;
                        category.Add(MakeJArray(values[i], item, allArray, aArray, "A"));
                        values[i]["기계명"] = values[i]["기계명"].Replace(cosumRate.Key,"");
                    }
                    category.Add(MakeJArray(values[i], item, allArray, rArray, "R"));
                }
                JObject postData = new JObject
                    {
                        {  "Data", category },
                        {  "ConfigurationGuid", global_iniLoad.GetConfig(className, "공정Import")},//"6abf068a-2bc8-49fa-90b4-4e794c686b6e"},
                        {  "TargetType", "Folder"},
                        {  "TargetId", 203}
                    };
                err = WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return err;
        }

        public JObject MakeJArray(Dictionary<string, string> keyValues, JObject item, string[] All, string[] array, string LineType)
        {
            JObject item2 = (JObject)item.DeepClone();

            item2.Add("Line Type", LineType);
            foreach (var data in keyValues)
            {
                if (All.Contains(data.Key))
                {
                    if (array != null && array.Contains(data.Key))
                        item2.Add(data.Key, data.Value);
                    else
                        item2.Add(data.Key, null);
                }
            }
            return item2;
        }
    }
}
