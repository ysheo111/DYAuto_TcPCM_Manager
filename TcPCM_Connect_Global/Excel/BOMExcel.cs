using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace TcPCM_Connect_Global
{
    class BOMExcel
    {
        public string Import(string tagetType, double tagetID)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
            ExcelImport excel = new ExcelImport();
            string err = null;
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();

                DialogResult dialog = dlg.ShowDialog();
                if (dialog == DialogResult.Cancel) return null;
                else if (dialog != DialogResult.OK) return $"Error : 파일 오픈에 실패하였습니다.";

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
                worksheet.Activate();
                //CBD의 기본정보
                worksheet.get_Range("G2", "G2").Select();

                JArray data = new JArray();

                //JObject header = new JObject();
                //header.Add(Report.LineType.lineType, "M");
                //header.Add(Report.LineType.level, 1);
                //header.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase");
                //header.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Benchmarkcalculation");
                int row = 3;
                while (true)
                {
                    double level = 0;
                    for(int col=0; col<10; col++)
                    {
                        if (global.ConvertDouble(worksheet.Cells[row, 1].Value) != 0) level = global.ConvertDouble(worksheet.Cells[row, 1].Value);
                    }
                    if (level==0) break;

                    JObject item = new JObject();

                    item.Add("Level", level);
                    item.Add("Line type", "M");
                    if(worksheet.Cells[row, 16].Value != null) item.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");
                    else item.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase");
                    item.Add("품번", worksheet.Cells[row, 11].Value);
                    item.Add("품명", worksheet.Cells[row, 12].Value);
                    item.Add("수량", worksheet.Cells[row, 13].Value);    
                    item.Add("단위", worksheet.Cells[row, 14].Value);                    
                    item.Add("재질", worksheet.Cells[row, 16].Value);
                    item.Add("설계중량", worksheet.Cells[row, 17].Value);
                    item.Add("유해물질", worksheet.Cells[row, 18].Value);
                    item.Add("소유량", worksheet.Cells[row, 20].Value);

                    data.Add(item);
                    row++;
                }


                String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Import";
                string response = WebAPI.POST(callUrl, new JObject
                {
                    { "Data", data },
                    { "ConfigurationGuid", global_iniLoad.GetConfig("CBD", "Import") },
                    { "TargetType", tagetType},
                    { "TargetId",  tagetID.ToString()},
                });

                try
                {
                    JObject postResult = JObject.Parse(response);
                    if ((bool)postResult["success"] == false) err = postResult["message"].ToString();
                }
                catch
                {
                    err += response;
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
                    //변경점 저장하면서 닫기
                    workBook.Close(true);
                    //Excel 프로그램 종료
                    application.Quit();
                    //오브젝트 해제1
                    ExcelCommon.ReleaseExcelObject(workBook);
                    ExcelCommon.ReleaseExcelObject(application);
                }
            }
            return err;
        }
    }
}
