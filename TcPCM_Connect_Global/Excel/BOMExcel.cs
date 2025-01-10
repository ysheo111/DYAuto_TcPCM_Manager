using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace TcPCM_Connect_Global
{
    public class BOMExcel
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
                JObject header = new JObject();
                header.Add("Level", 1);
                header.Add("Line type", "M");
                header.Add("품명", "Ass'y");
                data.Add(header);

                while (true)
                {
                    double level = 0;
                    for (int col = 0; col < 10; col++)
                    {
                        if (global.ConvertDouble(worksheet.Cells[row, 1 + col].Value) != 0)
                        {
                            level = global.ConvertDouble(worksheet.Cells[row, 1 + col].Value);
                            break;
                        }
                    }
                    if (level == 0) break;

                    JObject item = new JObject();
                    level += 1;
                    item.Add("Level", level);
                    item.Add("Line type", "M");

                    item.Add("품번", worksheet.Cells[row, 11].Value);
                    item.Add("수량", worksheet.Cells[row, 13].Value);
                    string substance = worksheet.Cells[row, 16].Value;
                    string result = global_DB.ScalarExecute($"Select id from [MDSubstances] where UniqueKey = '{substance}'", (int)global_DB.connDB.PCMDB);
                    if (result.Length > 0) item.Add("재질", worksheet.Cells[row, 16].Value);
                    else item.Add("재질", "");

                    item.Add("단위", worksheet.Cells[row, 14].Value);
                    item.Add("설계중량", worksheet.Cells[row, 17].Value);
                    item.Add("유해물질", worksheet.Cells[row, 18].Value);
                    item.Add("소유량", worksheet.Cells[row, 20].Value);
                    item.Add("품명", worksheet.Cells[row, 12].Value);
                    item.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase");

                    item.Add(Report.LineType.materials, "Siemens.TCPCM.Classification.Material.InjectionMoldingPart");
                    data.Add(item);

                    if (worksheet.Cells[row, 16].Value?.ToString().Length > 0)
                    {
                        JObject subitem1 = new JObject(item);

                        subitem1[Report.LineType.procument] = "Siemens.TCPCM.ProcurementType.Purchase_RawMaterial";
                        subitem1["Level"] = level + 1;
                        subitem1.Add("계산방법", "Siemens.TCPCM.CalculationQuality.Estimation(rough)");

                        subitem1["품명"] = "Raw Material";
                        subitem1.Add("WeightType", "Deployed weight");
                        subitem1[Report.LineType.materials]= "Siemens.TCPCM.Classification.Material.RawMaterial.Plastic";
                        data.Add(subitem1);

                        JObject subitem2 = new JObject(subitem1);
                        subitem2["품명"] = "Scrap";
                        subitem2["WeightType"] = "Scrap / Waste";
                        subitem2[Report.LineType.materials] = "Siemens.TCPCM.Classification.Material.Scrap";

                        data.Add(subitem2);

                    }

                    row++;
                }


                String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Import";
                string response = WebAPI.POST(callUrl, new JObject
                {
                    { "Data", data },
                    { "ConfigurationGuid", global_iniLoad.GetConfig("CBD", "BOM Import") },
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
        public string ExportPartBom(List<string> nodes, string fileLocation, Bom.ExportLang lang)
        {
            Interface export = new Interface();
            List<string> calcList = export.AllRootCalcId(nodes);
            JObject apiResult = export.LoadCalc(calcList,"BOM Export");
            if (apiResult == null) return "데이터 조회 시 오류가 발생하였습니다.";

            //Dictionary<string, Part> partList = BomSorting(apiResult);
            //if (partList.Count <= 0) return "부품이 존재하지 않습니다.";
            //else if (partList.Count == 1 && partList.First().Value == new Part()) return $"{partList.First().Key }";

            string err = BomSorting(apiResult,lang, fileLocation);

            return err;
        }

        private string BomSorting(JObject apiResult, Bom.ExportLang lang, string fileLocation)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
            try
            {
                string name = $"{apiResult["data"][0][Report.Header.partName]}({apiResult["data"][0][Report.Header.partNumber]})";
                File.Copy($@"{Application.StartupPath}\BOM Export.xlsx", $@"{fileLocation}\{name}.xlsx", true);
                //Excel 프로그램 실행
                application = new Excel.Application();
                //Excel 화면 띄우기 옵션
                application.Visible = true;
                //파일로부터 불러오기                
                workBook = application.Workbooks.Open($@"{fileLocation}\{name}.xlsx");
                Excel.Worksheet worksheet = workBook.Sheets["ExportTable"];
                worksheet.Select();

                int row = 2;
                foreach (var element in apiResult["data"])
                {                    
                    Dictionary<string, object> values = element.ToObject<Dictionary<string, object>>();
                    if (values[Report.LineType.lineType]?.ToString().Contains("Raw material") == true) continue;
                    else if (values[Report.LineType.lineType]?.ToString().Contains("Part") != true) continue;
                    else if (global.ConvertDouble(values[Report.LineType.level]) <=0) continue;

                    int level = (int)global.ConvertDouble(values[Report.LineType.level]);
                    row++;
                    worksheet.get_Range($"K{row}", $"K{row}").Select();
                              
                    worksheet.Cells[row, level].Value = level;

                    // Get the range of the filled cells
                    Excel.Range usedRange = worksheet.Range[
                        worksheet.Cells[row, 11],
                        worksheet.Cells[row,24]
                    ];

                    // Add borders to all cells in the used range
                    worksheet.Cells[row, level].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    worksheet.Cells[row, level].Borders.Weight = Excel.XlBorderWeight.xlThin;

                    usedRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    usedRange.Borders.Weight = Excel.XlBorderWeight.xlThin;

                    worksheet.Cells[row, 11].NumberFormat = "@";
                    worksheet.Cells[row, 11].Value = values[Report.Header.partNumber?.ToString().Replace("[DYA]", "")].ToString();                    
                    worksheet.Cells[row, 12].Value = values[Report.Header.partName]?.ToString().Replace("[DYA]", "").ToString();
                    worksheet.Cells[row, 13].Value = values[Report.Material.quantity];
                    worksheet.Cells[row, 14].Value = values[Report.Material.unit];
                    worksheet.Cells[row, 16].Value = values[Report.Material.substance]?.ToString().Replace("[DYA]", "");
                    worksheet.Cells[row, 17].Value = values[Report.Material.netWeight+ "[g]"];
                    worksheet.Cells[row, 18].Value = values[Report.Material.designWeight + "[g]"];
                    worksheet.Cells[row, 19].Formula = global.ConvertDouble(values[Report.Material.netWeight + "[g]"])>0? $"= (R{row} - Q{row}) / R{row}" : "";
                    worksheet.Cells[row, 19].NumberFormat = "###,##%";
                    worksheet.Cells[row, 20].Formula = global.ConvertDouble(values[Report.Material.netWeight + "[g]"])>0? $"= (Q{row} - R{row}) / Q{row}" : "";
                    worksheet.Cells[row, 20].NumberFormat = "###,##%";
                    worksheet.Cells[row, 21].Value = values[Report.Material.harmful];
                    worksheet.Cells[row, 24].Value = values["총액"];
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                workBook.Save();
            }
            return null;
        }
    }
}
