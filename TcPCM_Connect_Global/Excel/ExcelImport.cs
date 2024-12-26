using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace TcPCM_Connect_Global
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcelImport
    {
        HashSet<string> incorrect = new HashSet<string>();
        CBD cbd = new CBD();
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

        private void CellVaildation(string colName, int nameRow, int rowCol, int colCol, int rowValue, int colValue, Excel.Worksheet worksheet, ref JObject value)
        {
            string name = CombineString(nameRow, rowCol, colCol, worksheet);
            if (!name.Contains(cbd.FindValue(rowCol, colCol)))
            {
                incorrect.Add($"{cbd.FindValue(rowCol, colCol)}을 다시 확인해주세요. ({name} {rowCol} {colCol})");
                //(worksheet.Cells[row, excelCol]).Interior.Color = Excel.XlRgbColor.rgbYellow;
            }
            else value.Add(colName, $"{worksheet.Cells[rowValue, colValue].Value}");
        }

        private void CellVaildation(string colName, int nameRow, int rowCol, int colCol, Excel.Worksheet worksheet, object data, ref JObject value)
        {
            string name = CombineString(nameRow, rowCol, colCol, worksheet);
            if (!name.Contains(cbd.FindValue(rowCol, colCol)))
            {
                incorrect.Add($"{cbd.FindValue(rowCol, colCol)}을 다시 확인해주세요. ({rowCol} {colCol})");
                //(worksheet.Cells[row, excelCol]).Interior.Color = Excel.XlRgbColor.rgbYellow;
            }
            else value.Add(colName, JToken.FromObject(data));
        }

        public string Manufacturing(string tagetType, double tagetID)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
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

                JObject header = new JObject();
                header.Add("Designation", worksheet.Cells[6, 2].Value);
                header.Add("Designation (Calculation)", "Benchmark price");
                header.Add("Assembly hierarchy level", "1");
                header.Add("Line type", "P");
                header.Add("Designation(Manufacturing)", "");
                header.Add("Cycle time (Manufacturing step)", "");
                header.Add("Cycle time unit (Manufacturing step)", "");
                header.Add("Quantity", "");
                header.Add("Tool", "");
                header.Add("MFG", "");
                header.Add("JT Number", "");
                header.Add("JT", "");
                header.Add("내역", "");

                data.Add(header);
                int row = 11;
                JArray ops = new JArray();
                while (true)
                {
                    if (worksheet.Cells[row, 1].Value == null) break;
                    JObject item = new JObject();

                    item.Add("Designation", worksheet.Cells[6, 2].Value);
                    item.Add("내역", "");
                    item.Add("Assembly hierarchy level", "2");
                    item.Add("Line type", "D");
                    item.Add("Designation(Manufacturing)", worksheet.Cells[row, 1].Value);
                    item.Add("Cycle time (Manufacturing step)", worksheet.Cells[row, 4].Value);
                    item.Add("MFG", (row - 11) * 10);

                    ops.Add(item);
                    row++;
                }

                frmMachineSelect machine = new frmMachineSelect(ops.Count);
                DialogResult dialogResult = machine.ShowDialog();
                List<string> machineName = machine.ReturnValue1;
                if (dialogResult == DialogResult.Cancel)
                {
                    MessageBox.Show("설비를 제외하고 Import 합니다.");
                    data.Merge(ops, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Concat
                    });
                }
                else
                {
                    int i = 1;
                    header["내역"] = machineName[0];
                    foreach (JObject op in ops)
                    {
                        op["내역"] = machineName[0];
                        data.Add(op);

                        JObject item = new JObject();

                        item.Add("Designation", worksheet.Cells[6, 2].Value);
                        item.Add("Tool", $"[DYA]{machineName[i]}");
                        item.Add("Assembly hierarchy level", "2");
                        item.Add("Line type", "M");
                        item.Add("MFG", (i - 1) * 10);
                        item.Add("내역", machineName[0]);
                        item.Add("Quantity", 1);

                        i++;
                        data.Add(item);
                    }
                }


                String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Import";
                string response = WebAPI.POST(callUrl, new JObject
                {
                    { "Data", data },
                    { "ConfigurationGuid", global_iniLoad.GetConfig("CBD", "NX") },
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
                    err = response;
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

            if (incorrect.Count > 0) err += ("\n" + string.Join("", incorrect));
            return err;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Import(string tagetType, double tagetID)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Excel.Workbook workBook = null;
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

                frmPartWorkSheetSelect workSheetSelect = new frmPartWorkSheetSelect();
                workSheetSelect.workSheet = workSheetList;
                if (workSheetSelect.ShowDialog() == DialogResult.Cancel) return null;
                string val = workSheetSelect.ReturnValue1;
                string val2 = workSheetSelect.ReturnValue2;

                Excel.Worksheet worksheet = workBook.Worksheets.Item[val];
                Excel.Worksheet worksheet2 = workBook.Worksheets.Item[val2];
                worksheet.Activate();

                //CBD의 기본정보
                worksheet.get_Range("G2", "G2").Select();

                JArray data = new JArray();
                JObject header = new JObject();

                //Basic
                int row = 2, excelCol = 2;
                header.Add(Report.LineType.lineType, "M");
                header.Add(Report.LineType.level, 1);
                header.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");
                header.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Benchmarkcalculation");
                string segment = "";
                List<string> colName = cbd.column.Values.ToList();
                for (int i = 0; i < 13; i++)
                {
                    int nameRow = i < 9 ? 1 : 2;  // Conditional assignment for nameRow

                    if (i == 11)
                    {
                        string date;
                        try
                        {
                            date = worksheet.Cells[row, excelCol + 1].Value.ToString("yyyy-MM-dd");
                        }
                        catch (Exception ex)
                        {
                            date = DateTime.Now.ToString("yyyy-MM-dd");
                            Console.WriteLine($"Error retrieving date: {ex.Message}");  // Log error message
                        }
                        CellVaildation(colName[i], nameRow, row++, excelCol, worksheet, date, ref header);
                    }
                    else if (i == 10)  header.Add(Report.Header.exchangeRateCurrency, worksheet.Cells[row++, excelCol + 1].Value);
                    else if (colName[i] == Report.Header.category) segment = $"{worksheet.Cells[row++, excelCol + 1].Value}";
                    else
                    {
                        CellVaildation(colName[i], nameRow, row, excelCol, row++, excelCol + 1, worksheet, ref header);
                    }

                    if (i == 6)  // Adjusted condition to avoid unnecessary checks
                    {
                        excelCol = 5;
                        row = 5;
                    }
                    if (i == 10)
                    {
                        excelCol = 18;
                        row = 2;
                    }

                }
                header.Add(Report.Header.category, $"{header[Report.Header.suppier]}||{segment}");
                
                excelCol = 8; row = 11;
                for (int i = 13; i < 16; i++)
                {
                    CellVaildation(colName[i], 2, row, excelCol, row + 2, excelCol++, worksheet, ref header);
                }
                for (int i = 16; i < 19; i++)
                {
                    CellVaildation(colName[i], 3, row, excelCol, row + 3, excelCol++, worksheet, ref header);
                }

                //원/부재료
                int materialDefault = 3;
                JArray materials = new JArray();
                try
                {
                    for (int j = 25; j < 52; j++)
                    {
                        worksheet.get_Range($"C{j}", $"C{j}").Select();
                        JObject part = new JObject();
                        JObject material = new JObject();
                        JObject scrap = new JObject();
                        int colIndex = 16, nameRow = 3;
                        int[] values = { 11, 12, 13, 14, 17 };

                        double unit = 1, net = global.ConvertDouble(worksheet.Cells[j, 11].Value);
                        if (((Excel.Range)worksheet.Cells[j, 3]).Value == null) break;


                        for (int i = 3; i < 20; i++)
                        {
                            if (i == 4 || i == 16 || i == 18)
                            {
                                colIndex--;
                                continue;
                            }
                            excelCol = i;

                            if (!values.Contains(i)) CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref part);
                            else if (net != 0)
                            {
                                if (i == 14) CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material);
                                else if (i == 17) CellVaildation(Report.Material.rawMaterial, nameRow, 22, excelCol, j, excelCol, worksheet, ref scrap);
                                else if (i == 13)
                                {
                                    CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref material);
                                    CellVaildation(colName[colIndex + excelCol], nameRow, 22, excelCol, j, excelCol, worksheet, ref scrap);

                                    if (worksheet.Cells[j, excelCol].Value.ToString().ToLower().Contains("kg")) unit /= Math.Pow(10, 3);
                                    else if (worksheet.Cells[j, excelCol].Value.ToString().ToLower().Contains("t")) unit /= Math.Pow(10, 6);
                                }
                            }

                        }

                        part.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase");
                        part.Add(Report.LineType.level, "2");
                        part.Add(Report.Header.currency, header[Report.Header.currency]);
                        part.Add(Report.LineType.lineType, "M");
                        part.Add(Report.Header.dateOfCalc, header[Report.Header.dateOfCalc]);
                        part.Add(Report.Header.suppier, header[Report.Header.suppier]);
                        part.Add(Report.Header.category, header[Report.Header.category]);

                        if (net > 0)
                        {
                            part.Add(Report.Material.netWeight, net / unit);
                            CellVaildation(Report.Material.grossWeight, nameRow, 22, 12, j, 12, worksheet, ref part);
                            part[Report.Material.grossWeight] = (global.ConvertDouble(part[Report.Material.grossWeight]) - net) / unit;

                            material.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");
                            scrap.Add(Report.LineType.procument, "Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");

                            part.Add(Report.LineType.materials, "Siemens.TCPCM.Classification.Material.InjectionMoldingPart");
                            material.Add(Report.LineType.materials, "Siemens.TCPCM.Classification.Material.RawMaterial.Plastic");
                            scrap.Add(Report.LineType.materials, "Siemens.TCPCM.Classification.Material.Scrap");
                            material.Add(Report.Header.partName, "RawMaterial");
                            scrap.Add(Report.Header.partName, "Scrap");

                            material.Add("WeightType", "Deployed weight");
                            scrap.Add("WeightType", "Scrap / Waste");

                            material.Add(Report.Header.dateOfCalc, header[Report.Header.dateOfCalc]);
                            scrap.Add(Report.Header.dateOfCalc, header[Report.Header.dateOfCalc]);

                            material.Add(Report.Header.suppier, header[Report.Header.suppier]);
                            scrap.Add(Report.Header.suppier, header[Report.Header.suppier]);

                            material.Add(Report.Header.category, header[Report.Header.category]);
                            scrap.Add(Report.Header.category, header[Report.Header.category]);

                            part.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Benchmarkcalculation");
                            material.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Estimation(rough)");
                            scrap.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Estimation(rough)");

                            material.Add(Report.LineType.lineType, "M");
                            scrap.Add(Report.LineType.lineType, "M");

                            material.Add(Report.LineType.level, "3");
                            scrap.Add(Report.LineType.level, "3");

                            material.Add(Report.Header.currency, header[Report.Header.currency]);
                            scrap.Add(Report.Header.currency, header[Report.Header.currency]);

                            materials.Add(part);
                            materials.Add(material);
                            materials.Add(scrap);
                        }

                        else
                        {
                            part.Add(Report.LineType.method, "Siemens.TCPCM.CalculationQuality.Estimation(rough)");
                            CellVaildation(Report.Material.unit, nameRow, 22, 13, j, 13, worksheet, ref part);
                            CellVaildation(Report.Material.rawMaterial, nameRow, 22, 14, j, 14, worksheet, ref part);
                            materials.Add(part);
                        }
                    }
                }
                catch (Exception e)
                {
                    err = e.Message + "\n";
                }

                MemberInfo[] manufacturingMembers = typeof(Report.Manufacturing).GetMembers(BindingFlags.Static | BindingFlags.Public);
                MemberInfo[] headerMembers = typeof(Report.Header).GetMembers(BindingFlags.Static | BindingFlags.Public);
                MemberInfo[] materialMembers = typeof(Report.Material).GetMembers(BindingFlags.Static | BindingFlags.Public);

                MemberInfo[] total = new MemberInfo[manufacturingMembers.Length + headerMembers.Length + materialMembers.Length];
                manufacturingMembers.CopyTo(total, 0);
                headerMembers.CopyTo(total, manufacturingMembers.Length);
                materialMembers.CopyTo(total, manufacturingMembers.Length + headerMembers.Length);

                foreach (MemberInfo member in total)
                {
                    string key = ((FieldInfo)member).GetValue(member.Name)?.ToString();
                    if (!header.ContainsKey(key)) header.Add(key, null);
                }

                header.Add("WeightType", "");
                data.Add(header);
                data.Merge(materials);

                int[] manufacturingList = { 10, 15, 17, 21, 24, 25, 27 };
                int manufacturingDefault = 3;
                JArray manufacturings = new JArray();

                for (int j = 57; j < 89; j++)
                {
                    int machinLine = j - 49;
                    int colIndex = 30;
                    int nameRow = 2;

                    worksheet.get_Range($"C{j}", $"C{j}").Select();
                    JObject manufacturing = new JObject();
                    JObject machine = new JObject();
                    JObject otherMachine = new JObject();
                    JObject labor = new JObject();

                    manufacturing.Add(Report.Manufacturing.sequence, j * 10);
                    labor.Add(Report.Manufacturing.sequence, j * 10);
                    machine.Add(Report.Manufacturing.sequence, j * 10);

                    manufacturing.Add(Report.LineType.lineType, "D");
                    labor.Add(Report.LineType.lineType, "R");
                    machine.Add(Report.LineType.lineType, "A");

                    manufacturing.Add(Report.LineType.level, "2");
                    labor.Add(Report.LineType.level, "2");
                    machine.Add(Report.LineType.level, "2");

                    manufacturing.Add(Report.Header.partName, header[Report.Header.partName]);
                    labor.Add(Report.Header.partName, header[Report.Header.partName]);
                    machine.Add(Report.Header.partName, header[Report.Header.partName]);

                    manufacturing.Add(Report.Header.suppier, header[Report.Header.suppier]);

                    manufacturing.Add(Report.Header.currency, header[Report.Header.currency]);                    
                    labor.Add(Report.Header.currency, header[Report.Header.currency]);
                    machine.Add(Report.Header.currency, header[Report.Header.currency]);

                    for (int i = 3; i < 14; i++)
                    {
                        if (i == 4)
                        {
                            colIndex--;
                            continue;
                        }
                        else if (colName[colIndex + i] == Report.Manufacturing.category)
                        {
                            CellVaildation(colName[colIndex + i], nameRow, 55, i, worksheet, $"{header[Report.Header.suppier]}||{worksheet.Cells[j, i].Value}", ref manufacturing);
                            CellVaildation(colName[colIndex + i], nameRow, 55, i, worksheet, $"{header[Report.Header.suppier]}||{worksheet.Cells[j, i].Value}", ref labor);
                        }
                        else
                        {
                            CellVaildation(colName[colIndex + i], nameRow, 55, i, j, i, worksheet, ref manufacturing);
                            CellVaildation(colName[colIndex + i], nameRow, 55, i, j, i, worksheet, ref labor);
                        }

                        //CellVaildation(colName[colIndex + i], nameRow, 55, i, j, i, worksheet, ref machine);
                    }

                    colIndex += 11;
                    nameRow = 4;

                    for (int i = 3; i < 28; i++)
                    {
                        try
                        {
                            if (manufacturingList.Contains(i))
                            {
                                colIndex--;
                                continue;
                            }
                            else if (i == 22 || i == 23) CellVaildation(colName[colIndex + i], nameRow, 4, i, machinLine, i, worksheet2, ref otherMachine);
                            else if (machine.ContainsKey(colName[colIndex + i])) CellVaildation(Report.Manufacturing.rationForSupplementaryMachine3, nameRow, 4, i, machinLine, i, worksheet2, ref machine);
                            else if (colName[colIndex + i] == Report.Manufacturing.category)
                            { 
                                CellVaildation(colName[colIndex + i], nameRow, 4, i, worksheet2, $"{header[Report.Header.suppier]}||{worksheet2.Cells[machinLine, i].Value}", ref machine);
                            }
                            else CellVaildation(colName[colIndex + i], nameRow, 4, i, machinLine, i, worksheet2, ref machine);
                        }
                        catch (Exception e)
                        {
                            err += e.Message+"\n";
                        }

                    }
                    double rationForSupplementaryMachine = global.ConvertDouble(machine[Report.Manufacturing.rationForSupplementaryMachine3]) != 0 ?
                        global.ConvertDouble(machine[Report.Manufacturing.rationForSupplementaryMachine1])
                    * global.ConvertDouble(machine[Report.Manufacturing.rationForSupplementaryMachine2])
                    / global.ConvertDouble(machine[Report.Manufacturing.rationForSupplementaryMachine3]) : 0;
                    machine.Add(Report.Manufacturing.spaceCost, rationForSupplementaryMachine);
                    manufacturing.Add(Report.Manufacturing.redirectExpenseRatio, machine[Report.Manufacturing.redirectExpenseRatio]);
                    manufacturing.Add(Report.Manufacturing.productionDay, machine[Report.Manufacturing.productionDay]);
                    manufacturing.Add(Report.Manufacturing.productionTime,
                        global.ConvertDouble(machine[Report.Manufacturing.productionDay])*global.ConvertDouble(machine[Report.Manufacturing.productionTime]));

                    manufacturings.Add(manufacturing);
                    manufacturings.Add(labor);
                    manufacturings.Add(machine);
                    if (global.ConvertDouble(otherMachine[Report.Manufacturing.otherMachineCost]) != 0)
                    {
                        otherMachine.Add(Report.Manufacturing.sequence, j * 10);
                        otherMachine.Add(Report.Manufacturing.machineName, "Other Machine");
                        otherMachine.Add(Report.LineType.lineType, "A");
                        otherMachine.Add(Report.LineType.level, "2");
                        otherMachine.Add(Report.Manufacturing.machineCost, otherMachine[Report.Manufacturing.otherMachineCost]);
                        //otherMachine.Add(Report.Manufacturing.amotizingYearOfMachine, otherMachine[Report.Manufacturing.otherYearOfMachine]);
                        otherMachine.Add(Report.Header.currency, header[Report.Header.currency]);
                        otherMachine.Add(Report.Header.partName, header[Report.Header.partName]);
                        manufacturings.Add(otherMachine);
                    }
                }

                data.Merge(manufacturings);

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
                string val = sheetSelect.ReturnValue1;

                Excel.Worksheet worksheet = workBook.Worksheets.Item[val];
                //Excel.Range xlRng = worksheet.UsedRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);
                //var lastCell = worksheet.ra;

                object[,] xlRng = worksheet.UsedRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

                string[] keys = new string[xlRng.GetUpperBound(1) + 1];
                int firstCol = 0, firstRow;
                for (firstRow = xlRng.GetLowerBound(0); firstRow <= xlRng.GetUpperBound(0); firstRow++)
                {
                    for (int col = xlRng.GetLowerBound(1); col <= xlRng.GetUpperBound(1); col++)
                    {
                        if (xlRng[firstRow, col] == null) continue;
                        if (firstCol == 0) firstCol = col;

                        keys[col] = xlRng[firstRow, col]?.ToString();
                    }

                    if (!keys.All(x => x == null)) break;
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
                            if (keys[category].Contains(col.Name) && !keys.Where(key => key != keys[category]).Contains(col.Name))
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

                                dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Value = resultValue;
                                if (new List<string>() { "지역", "공정", "업종", "설비명", "설비구분", "통화", "Valid From", "구분자", "재료명", "메이커", "구분", "비중", "비중 단위", "가격 단위", "스크랩 비용 단위", "이름", "ISO", "UOM Code", "UOM 명" }.Contains(col.Name)) continue;

                                if ((((col.Name.Contains("율") || col.Name.Contains("률")) && !col.Name.Contains("임률") && !col.Name.Contains("환율"))) && !col.Name.Contains("수선비율"))
                                {
                                    dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Value = global.ConvertDouble(resultValue) * 100;
                                }
                                if (!double.TryParse(resultValue, out double resultDouble)) dgv.Rows[dgv.Rows.Count - 1].Cells[col.Name].Style.BackColor = Color.Yellow;
                            }
                        }
                    }

                    if (!flag) dgv.Rows.RemoveAt(dgv.Rows.Count - 1);
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
