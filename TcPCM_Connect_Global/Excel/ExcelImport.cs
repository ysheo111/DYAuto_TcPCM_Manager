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
            for(int i=0;i<nameRow;i++)
            {
                name += cbd.CleanString(worksheet.Cells[rowCol, colCol].Value?.ToString());
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
            else value.Add(colName, worksheet.Cells[rowValue, colValue].Value);
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
                header.Add("Assembly hierarchy level","1");
                header.Add("Line type","P");
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
                    if (worksheet.Cells[row, 1].Value==null) break;
                    JObject item = new JObject();

                    item.Add("Designation", worksheet.Cells[6, 2].Value);
                    item.Add("내역", "");
                    item.Add("Assembly hierarchy level", "2");
                    item.Add("Line type", "D");
                    item.Add("Designation(Manufacturing)", worksheet.Cells[row, 1].Value);
                    item.Add("Cycle time (Manufacturing step)", worksheet.Cells[row, 4].Value);
                    item.Add("MFG", (row-11)*10);

                    ops.Add(item);
                    row++;
                }

                frmMachineSelect machine = new frmMachineSelect(ops.Count);
                DialogResult dialogResult = machine.ShowDialog();
                List<string> machineName= machine.ReturnValue1;
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
                        item.Add("MFG", (i-1)*10);
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
                int excelOrder = 0;
                //Basic
                int row = 2, excelCol = 2;
                header.Add(Report.LineType.lineType, "M");
                header.Add(Report.LineType.level, 0);

                //        { (2, 2), Report.Header.modelName},
                //{ (3, 2), Report.Header.partNumber},
                //{ (4, 2), Report.Header.partName},

                //{ (5, 2), Report.Header.company},
                //{ (6, 2), Report.Header.customer},
                //{ (7, 2), Report.Header.currency},
                //{ (8, 2), Report.Header.transport},

                //{ (5, 5), Report.Header.category},
                //{ (6, 5), Report.Header.suppier},
                //{ (7, 5), Report.Header.exchangeRate},

                //{ (2, 18), Report.Header.dateOfCalc},
                //{ (3, 18), Report.Header.author},

                int i = 0;
                foreach (KeyValuePair<(int, int), string> pair in cbd.column)
                {
                    int nameRow = i < 9 ? 1 : 2;  // Conditional assignment for nameRow

                    if (i == 10)
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
                        CellVaildation(pair.Value, nameRow, row++, excelCol, worksheet, date, ref header);
                    }
                    else
                    {
                        CellVaildation(pair.Value, nameRow, row, excelCol, row++, excelCol + 1, worksheet, ref header);
                    }

                    if (i == 6)  // Adjusted condition to avoid unnecessary checks
                    {
                        excelCol = 5;
                        row = 5;
                    }
                    if (i == 9)
                    {
                        excelCol = 18;
                        row = 2;
                    }
                    else if (i == 11)  // Adjusted condition to avoid unnecessary checks
                    {
                        break;  // Break after setting row for efficiency
                    }

                    i++;
                }

                
                foreach(KeyValuePair<(int,int),string> pair in cbd.column)
                {
                    int nameRow = 1;
                    if (i == 9) nameRow = 2;
                    if(i==10)
                    {
                        string date = "";
                        try
                        {
                            date = worksheet.Cells[row, excelCol+1].Value.ToString("yyyy-MM-dd");
                        }
                        catch
                        {
                            date = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        CellVaildation(pair.Value, nameRow, row++, excelCol, worksheet, date, ref header);
                    }
                    else
                    {
                        CellVaildation(pair.Value, nameRow, row, excelCol, row++, excelCol + 1, worksheet, ref header);
                    }
                    i++;

                    if (i == 7)
                    {
                        excelCol = 5;
                        row = 5;
                    }
                    else if (i == 10)
                    {
                        excelCol = 18;
                        row = 2;
                    }
                    else if (i == 12) break;
                }

                //CellVaildation(Report.Header.productName, row, excelCol-2, row++, excelCol, worksheet, ref header);
                //CellVaildation(Report.Header.partName, row, excelCol-2, row++, excelCol, worksheet, ref header);
                //CellVaildation(Report.Header.partNumber, row, excelCol-2, row++, excelCol, worksheet, ref header);
                //CellVaildation(Report.Header.region, row, excelCol-2, row++, excelCol, worksheet, ref header);
                //string date = "";
                //try
                //{
                //    date = worksheet.Cells[row, excelCol].Value.ToString("yyyy-MM-dd");
                //}
                //catch
                //{
                //    date = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                //CellVaildation(Report.Header.SOP, row++, excelCol-2, worksheet,date, ref header);

                //row = 2; excelCol = 15;
                //CellVaildation(Report.Header.company, row, excelCol - 1, row++, excelCol, worksheet, ref header);
                //CellVaildation(Report.Header.team, row, excelCol - 1, row++, excelCol, worksheet, ref header);
                //CellVaildation(Report.Header.author, row, excelCol - 1, row++, excelCol, worksheet, ref header);
                //date = "";
                //try
                //{
                //    date = worksheet.Cells[row++, excelCol].Value.ToString("yyyy-MM-dd");
                //}
                //catch
                //{
                //    date = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                //CellVaildation(Report.Header.dateOfCalculation, row, excelCol - 1, worksheet, date, ref header);
                //CellVaildation(Report.Header.incoterms, row, excelCol - 1, row++, excelCol, worksheet, ref header);
                //CellVaildation(Report.Header.currency, row, excelCol - 1, row++, excelCol, worksheet, ref header);

                //row = 11; excelCol = 11;
                //CellVaildation(Report.Summary.packageTotal, row-2, excelCol, row, excelCol++, worksheet, ref header);
                //CellVaildation(Report.Summary.transportTotal, row-2, excelCol, row, excelCol++, worksheet, ref header);
                //excelCol++;
                //CellVaildation(Report.Summary.etc, row - 2, excelCol, row, excelCol++, worksheet, ref header);

                //excelCol = 18;
                //CellVaildation(Report.Summary.administrationCosts, row-2, excelCol, row, excelCol++, worksheet, ref header);
                //CellVaildation(Report.Summary.profit, row-2, excelCol, row, excelCol++, worksheet, ref header);
                //CellVaildation(Report.Summary.materialOverhead, row-2, excelCol, row, excelCol++, worksheet, ref header);               

                //header.Add(Report.Summary.administrationCosts, worksheet.Cells[10, excelCol++].Value);
                //header.Add(Report.Summary.profit, worksheet.Cells[10, excelCol++].Value);
                //header.Add(Report.Summary.materialOverhead, worksheet.Cells[10, excelCol++].Value);

                ////원/부재료
                //row = 17;
                //int initRow = row-2;
                //double returnRatio = 0;
                //JArray materials = new JArray();
                //while (true)
                //{
                //    if (((Excel.Range)worksheet.Cells[row, 2]).MergeCells) break;

                //    JObject material = new JObject();
                //    JObject scrap = new JObject();

                //    //material.Add(Report.Material.priceUnit, worksheet.Cells[16, 9].Value);
                //    material.Add(Report.Header.currency, header[Report.Header.currency]);
                //    //scrap.Add(Report.Material.priceUnit, worksheet.Cells[16, 13].Value);
                //    scrap.Add(Report.Header.currency, header[Report.Header.currency]);

                //    excelCol = 3;
                //    if (worksheet.Cells[row, excelCol].Value == null)
                //    {
                //        row++;
                //        continue;
                //    }

                //    CellVaildation(Report.Material.name, initRow, excelCol - 1, row, excelCol, worksheet, ref material);
                //    //material.Add(Report.Material.name, worksheet.Cells[row, excelCol].Value);
                //    material.Add(Report.LineType.lineType, "P");
                //    material.Add(Report.LineType.level, 1);
                //    material.Add("Procurement", "Siemens.TCPCM.ProcurementType.Purchase");
                //    material.Add("Calculation quality", "Siemens.TCPCM.CalculationQuality.Estimation(rough)");

                //    scrap.Add(Report.Material.name, $"{worksheet.Cells[row, excelCol].Value}_Scrap");
                //    scrap.Add(Report.LineType.lineType, "P");
                //    scrap.Add(Report.LineType.level, 1);
                //    scrap.Add("Procurement", $"Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");
                //    scrap.Add("Calculation quality", $"Siemens.TCPCM.CalculationQuality.Estimation(rough)");

                //    excelCol = 5;
                //    CellVaildation(Report.Material.itemNumber, initRow, excelCol, row, excelCol++, worksheet, ref material);
                //    //material.Add(Report.Material.itemNumber, worksheet.Cells[row, excelCol++].Value);

                //    CellVaildation(Report.Material.substance, initRow + 1, excelCol, row, excelCol, worksheet, ref material);
                //    CellVaildation(Report.Material.substance, initRow + 1, excelCol, row, excelCol++, worksheet, ref scrap);

                //    //material.Add(Report.Material.substance, worksheet.Cells[row, excelCol].Value);
                //    //scrap.Add(Report.Material.substance, worksheet.Cells[row, excelCol++].Value);
                //    //material.Add(Report.Material.standard, worksheet.Cells[row, excelCol].Value);
                //    //scrap.Add(Report.Material.standard, worksheet.Cells[row, excelCol++].Value);
                //    excelCol++;
                //    CellVaildation(Report.Material.qunantityUnit, initRow, excelCol, row, excelCol, worksheet, ref material);
                //    string[] quantityArray = material[Report.Material.qunantityUnit]?.ToString().Replace("Pcs", "pcs").Split('/');
                //    material[Report.Material.qunantityUnit] = quantityArray[0];
                //    CellVaildation(Report.Material.qunantityUnit, initRow, excelCol, row, excelCol, worksheet, ref scrap);
                //    scrap[Report.Material.qunantityUnit] = quantityArray[0];
                //    CellVaildation(Report.Material.priceUnit, initRow, excelCol, row, excelCol, worksheet, ref material);
                //    material[Report.Material.priceUnit] = (quantityArray.Length == 1? quantityArray[0]: quantityArray[1]);
                //    CellVaildation(Report.Material.priceUnit, initRow, excelCol, row, excelCol++, worksheet, ref scrap);
                //    scrap[Report.Material.priceUnit] = (quantityArray.Length == 1 ? quantityArray[0] : quantityArray[1]);
                //    //material.Add(Report.Material.qunantityUnit, worksheet.Cells[row, excelCol].Value);
                //    //scrap.Add(Report.Material.qunantityUnit, worksheet.Cells[row, excelCol].Value);
                //    //material.Add(Report.Material.priceUnit, worksheet.Cells[row, excelCol].Value);
                //    //scrap.Add(Report.Material.priceUnit, worksheet.Cells[row, excelCol++].Value);

                //    CellVaildation(Report.Material.unitCost, initRow, excelCol, row, excelCol++, worksheet, ref material);
                //    CellVaildation(Report.Material.netWeight, initRow + 1, excelCol, row, excelCol++, worksheet, ref material);
                //    CellVaildation(Report.Material.quantity, initRow + 1, excelCol, row, excelCol++, worksheet, ref material);                    
                //    double quantity = global.ConvertDouble(worksheet.Cells[row, excelCol].Value);
                //    double gross = global.ConvertDouble(material[Report.Material.quantity]);
                //    material[Report.Material.quantity] = gross == 0 ? quantity : gross;
                //    //material.Add(Report.Material.unitCost, worksheet.Cells[row, excelCol++].Value);
                //    //material.Add(Report.Material.netWeight, worksheet.Cells[row, excelCol++].Value);                  
                //    //material.Add(Report.Material.quantity, worksheet.Cells[row, excelCol++].Value);

                //    double sprue =
                //        Math.Round((global.ConvertDouble(material[Report.Material.quantity]) - global.ConvertDouble(material[Report.Material.netWeight]))
                //    / global.ConvertDouble(material[Report.Material.netWeight]) * 100);
                   
                //    material.Add(Report.Material.grossWeight, (global.ConvertDouble(material[Report.Material.netWeight]) == 0 ? quantity : sprue));
                //    //header.Add(Report.Material.grossWeight, sprue);
                //    scrap.Add(Report.Material.grossWeight, (global.ConvertDouble(material[Report.Material.netWeight]) == 0 ? quantity : sprue));

                //    scrap.Add(Report.Material.quantity,
                //        -(global.ConvertDouble(material[Report.Material.quantity]) - global.ConvertDouble(material[Report.Material.netWeight])));

                //    if (row == 17)
                //    {
                //        //header.Add(Report.Material.standard, $"Siemens.TCPCM.Classification.Material.DieCastingPart");
                //        header.Add(Report.Material.netWeight, worksheet.Cells[row, 10].Value);
                //        //material.Add(Report.Material.standard, "Siemens.TCPCM.Classification.Material.RawMaterial.Standard");

                //        scrap[Report.Material.quantity] = global.ConvertDouble(material[Report.Material.netWeight]) - global.ConvertDouble(material[Report.Material.quantity]);

                //        //if (manufacturingType == Bom.ManufacturingType.사출.ToString())
                //        //{
                //        //    header.Add(Report.Material.netWeight, worksheet.Cells[row, 10].Value);
                //        //    header.Add(Report.Material.standard, $"Siemens.TCPCM.Classification.Material.InjectionMoldingPart");
                //        //    material.Add(Report.Material.standard, "Siemens.TCPCM.Classification.Material.RawMaterial.Plastic");                           
                //        //}
                //        //else if (manufacturingType == Bom.ManufacturingType.주조.ToString())
                //        //{
                //        //    header.Add(Report.Material.standard, $"Siemens.TCPCM.Classification.Material.DieCastingPart");
                //        //    header.Add(Report.Material.netWeight, worksheet.Cells[row, 10].Value);
                //        //    //header.Add(Report.Material.loss, 100);
                //        //    //header.Add(Report.Material.netWeight, worksheet.Cells[row, 11].Value);
                //        //    material.Add(Report.Material.standard, "Siemens.TCPCM.Classification.Material.RawMaterial.CastingMaterial");
                //        //}
                //        //else if (manufacturingType == Bom.ManufacturingType.프레스.ToString())
                //        //{
                //        //    header.Add(Report.Material.standard, $"Siemens.TCPCM.Classification.Material.StampingPart");
                //        //    header.Add(Report.Material.netWeight, worksheet.Cells[row, 10].Value);

                //        //    material.Add(Report.Material.standard, "Siemens.TCPCM.Classification.Material.SemiFinished.SheetMetal.Plate");
                //        //    header.Add(Report.Header.width, worksheet.Cells[5, 26].Value);
                //        //    header.Add(Report.Header.height, worksheet.Cells[5, 27].Value);
                //        //    header.Add(Report.Header.thinkness, worksheet.Cells[5, 29].Value);
                //        //}
                //        //else if (manufacturingType == Bom.ManufacturingType.가공.ToString() || manufacturingType == Bom.ManufacturingType.프레스.ToString())
                //        //{
                //        //    //header.Add(Report.Material.standard, $"Siemens.TCPCM.Classification.Material.DieCastingPart");
                //        //    header.Add(Report.Material.netWeight, worksheet.Cells[row, 10].Value);
                //        //    //material.Add(Report.Material.standard, "Siemens.TCPCM.Classification.Material.RawMaterial.Standard");

                //        //    scrap[Report.Material.quantity] = global.ConvertDouble(material[Report.Material.netWeight])-global.ConvertDouble(material[Report.Material.quantity]);
                //        //}

                //        material.Add("Weight  Type", "Deployed weight");
                //        //scrap.Add(Report.Material.standard, $"Siemens.TCPCM.Classification.Material.Scrap");
                //        scrap.Add("Weight  Type", $"Scrap / Waste");
                //    }

                //    excelCol = 14;
                //    double etc = global.ConvertDouble(worksheet.Cells[row, excelCol].Value) / (global.ConvertDouble(worksheet.Cells[row, excelCol+1].Value)- global.ConvertDouble(worksheet.Cells[row, excelCol].Value)) * 100;
                //    CellVaildation(Report.Material.etc, initRow, excelCol, worksheet, etc, ref material);
                //    //etc += worksheet.Cells[row, excelCol++].Value;

                //    excelCol = 23;
                //    JObject dross = new JObject();
                //    if (worksheet.Cells[row, excelCol-2].Value != null && worksheet.Cells[row, excelCol - 2].Value?.ToString().Contains("dross"))
                //    {
                //        string strDross = worksheet.Cells[row, excelCol - 2].Value?.ToString();
                //        string[] split =  strDross.Split(':');
                //        dross = new JObject( material);
                //        dross["Procurement"] = "Siemens.TCPCM.ProcurementType.InternalSale";
                //        dross[Report.Material.quantity] = split[1].Split(',')[0].Replace(" ","");
                //        dross[Report.Material.unitCost] = split[2].Split(',')[0].Replace(" ", "");
                        
                //    }
                //    excelCol++;
                //    if (returnRatio == 0 && worksheet.Cells[row, excelCol].Value != 0)
                //    {
                //        returnRatio = 100 - worksheet.Cells[row, excelCol].Value;
                //    }
                //    excelCol++;

                //    CellVaildation(Report.Material.unitCost, initRow+1, excelCol, row, excelCol++, worksheet, ref scrap);
                    
                //    //scrap.Add(Report.Material.unitCost, worksheet.Cells[row, excelCol++].Value);
                //    materials.Add(material);
                //    if (global.ConvertDouble(worksheet.Cells[row, excelCol].Value) != 0) materials.Add(scrap);
                //    if (dross.Count!=0)materials.Add(dross);
                //    row++;
                //}


                //CellVaildation(Report.Material.returnRatio, 16, 24, worksheet, returnRatio, ref header);

                //int increase = row - 26;
                //Dictionary<(int, int), string > temp = new Dictionary<(int, int), string>(cbd.column);
                //cbd.column.Clear();
                //foreach (KeyValuePair< (int, int), string> keyValue in (increase > 0 ? temp.Reverse() : temp))
                //{
                //    cbd.column.Add((keyValue.Key.Item1+ increase, keyValue.Key.Item2), keyValue.Value);                    
                //}
                ////header.Add(Report.Material.etc, etc);
                ////header.Add(Report.Material.returnRatio, returnRatio);
                ////CellVaildation(Report.Header.plcVolume, row+1, 18, row + 2, 18, worksheet, ref header);
                ////CellVaildation(Report.Header.plc, row + 1, 19, row + 2, 19, worksheet, ref header);
                ////CellVaildation(Report.Header.annualQty, row + 1, 20, row + 2, 20, worksheet, ref header);

                ////header.Add(Report.Header.plcVolume, worksheet.Cells[row + 1, 18].Value);
                ////header.Add(Report.Header.plc, worksheet.Cells[row + 1, 19].Value);
                ////header.Add(Report.Header.annualQty, worksheet.Cells[row + 1, 20].Value);

                //MemberInfo[] manufacturingMembers = typeof(Report.Manufacturing).GetMembers(BindingFlags.Static | BindingFlags.Public);
                //MemberInfo[] headerMembers = typeof(Report.Header).GetMembers(BindingFlags.Static | BindingFlags.Public);
                //MemberInfo[] materialMembers = typeof(Report.Material).GetMembers(BindingFlags.Static | BindingFlags.Public);
                //MemberInfo[] toolMembers = typeof(Report.Tooling).GetMembers(BindingFlags.Static | BindingFlags.Public);

                //MemberInfo[] total = new MemberInfo[manufacturingMembers.Length + headerMembers.Length + materialMembers.Length+ toolMembers.Length];
                //manufacturingMembers.CopyTo(total, 0);
                //headerMembers.CopyTo(total, manufacturingMembers.Length);
                //materialMembers.CopyTo(total, manufacturingMembers.Length + headerMembers.Length);
                //toolMembers.CopyTo(total, manufacturingMembers.Length + headerMembers.Length+materialMembers.Length);

                //foreach (MemberInfo member in total)
                //{
                //    string key = ((FieldInfo)member).GetValue(member.Name)?.ToString();
                //    if (!header.ContainsKey(key)) header.Add(key, null);
                //}

                //header.Add("Weight  Type","");
                //header.Add("Procurement", "Siemens.TCPCM.ProcurementType.Purchase");
                //header.Add("Calculation quality", "Siemens.TCPCM.CalculationQuality.Benchmarkcalculation");
                //data.Add(header);
                //data.Merge(materials);

                ////가공비
                //int currencyRow = row+5;
                //row += 5;
                //while (true)
                //{
                //    if (((Excel.Range)worksheet.Cells[row, 2]).MergeCells) break;

                //    JObject manufacturing = new JObject();
                //    JObject machine = new JObject();
                //    JObject labor = new JObject();

                //    manufacturing.Add(Report.LineType.lineType, "D");
                //    manufacturing.Add(Report.LineType.level, 1);
                //    manufacturing.Add(Report.Material.grossWeight, header[Report.Material.grossWeight]);
                //    manufacturing.Add(Report.Material.loss, header[Report.Material.loss]);
                //    manufacturing.Add(Report.Material.quantity, 1);
                //    //if (manufacturingType == Bom.ManufacturingType.사출.ToString())
                //    //{
                //    //    manufacturing.Add(Report.Manufacturing.techology, "Siemens.TCPCM.Classification.Technology.InjectionMolding");
                //    //}
                //    //else if (manufacturingType == Bom.ManufacturingType.주조.ToString() || manufacturingType == Bom.ManufacturingType.가공.ToString())
                //    //{
                //    //    manufacturing.Add(Report.Manufacturing.techology, "Siemens.TCPCM.Classification.Technology.DieCasting");
                //    //}
                //    machine.Add(Report.LineType.lineType, "M");
                //    machine.Add(Report.LineType.level, 1);
                //    labor.Add(Report.LineType.lineType, "L");
                //    labor.Add(Report.LineType.level, 1);

                //    excelCol = 2;
                //    if (worksheet.Cells[row, 6].Value == null)
                //    {
                //        row++;
                //        continue;
                //    }
                //    manufacturing.Add(Report.Manufacturing.sequence, row);
                //    machine.Add(Report.Manufacturing.sequence, row);
                //    labor.Add(Report.Manufacturing.sequence, row);

                //    excelCol = 3;
                //    CellVaildation(Report.Manufacturing.partName, currencyRow - 2, excelCol-1, row, excelCol, worksheet, ref manufacturing);
                //    CellVaildation(Report.Manufacturing.partName, currencyRow - 2, excelCol-1, row, excelCol, worksheet, ref machine);
                //    CellVaildation(Report.Manufacturing.partName, currencyRow - 2, excelCol-1, row, excelCol++, worksheet, ref labor);

                //    //manufacturing.Add(Report.Manufacturing.partName, worksheet.Cells[row, excelCol].Value);
                //    //machine.Add(Report.Manufacturing.partName, worksheet.Cells[row, excelCol].Value);
                //    //labor.Add(Report.Manufacturing.partName, worksheet.Cells[row, excelCol++].Value);

                //    excelCol = 5;
                //    CellVaildation(Report.Manufacturing.itemNumber, currencyRow - 2, excelCol, row, excelCol, worksheet, ref manufacturing);
                //    CellVaildation(Report.Manufacturing.itemNumber, currencyRow - 2, excelCol, row, excelCol, worksheet, ref machine);
                //    CellVaildation(Report.Manufacturing.itemNumber, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref labor);

                //    CellVaildation(Report.Manufacturing.manufacturingName, currencyRow - 2, excelCol, row, excelCol, worksheet, ref manufacturing);
                //    CellVaildation(Report.Manufacturing.manufacturingName, currencyRow - 2, excelCol, row, excelCol, worksheet, ref machine);
                //    CellVaildation(Report.Manufacturing.manufacturingName, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref labor);

                //    CellVaildation(Report.Manufacturing.machineName, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);
                //    CellVaildation(Report.Manufacturing.cavity, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref manufacturing);
                //    CellVaildation(Report.Manufacturing.workers, currencyRow - 2, excelCol, row, excelCol, worksheet, ref labor);
                //    CellVaildation(Report.Manufacturing.workers, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);

                //    excelCol++;
                //    CellVaildation(Report.Manufacturing.usage, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref manufacturing);                   
                //    CellVaildation(Report.Manufacturing.grossWage, currencyRow - 2, excelCol, row, excelCol, worksheet, ref labor);
                //    CellVaildation(Report.Manufacturing.grossWage, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);

                //    //labor.Add(Report.Manufacturing.grossWage, worksheet.Cells[row, excelCol].Value);
                //    //machine.Add(Report.Manufacturing.grossWage, worksheet.Cells[row, excelCol++].Value);
                //    //labor.Add(Report.Material.priceUnit, worksheet.Cells[currencyRow, excelCol++].Value);
                //    excelCol++;
                //    labor.Add(Report.Header.currency, header[Report.Header.currency]);

                //    excelCol = 16;
                //    CellVaildation(Report.Manufacturing.et, currencyRow - 3, excelCol-1, currencyRow - 3, excelCol++, worksheet, ref manufacturing);
                //    manufacturing[Report.Manufacturing.et] = global.ConvertDouble(manufacturing[Report.Manufacturing.et]) * 100;

                //    excelCol++;
                //    CellVaildation(Report.Manufacturing.lotQty, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref manufacturing);
                //    CellVaildation(Report.Manufacturing.oee, currencyRow - 2, excelCol, worksheet, (worksheet.Cells[row, excelCol++].Value), ref manufacturing);
                //    CellVaildation(Report.Manufacturing.prepare, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref manufacturing);
                //    CellVaildation(Report.Manufacturing.netCycleTime, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref manufacturing);

                //    //manufacturing.Add(Report.Manufacturing.lotQty, worksheet.Cells[row, excelCol++].Value);
                //    //manufacturing.Add(Report.Manufacturing.oee, 100 - worksheet.Cells[row, excelCol++].Value);
                //    //manufacturing.Add(Report.Manufacturing.prepare, worksheet.Cells[row, excelCol++].Value);
                //    //manufacturing.Add(Report.Manufacturing.netCycleTime, worksheet.Cells[row, excelCol++].Value);

                //    excelCol = 23;
                //    CellVaildation(Report.Manufacturing.machineCost, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);
                //    //machine.Add(Report.Manufacturing.machineCost, worksheet.Cells[row, excelCol].Value);

                //    machine.Add(Report.Header.currency, header[Report.Header.currency]);
                //    manufacturing.Add(Report.Header.currency, header[Report.Header.currency]);

                //    CellVaildation(Report.Manufacturing.amotizingYearOfMachine, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);
                //    CellVaildation(Report.Manufacturing.machineArea, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);

                //    //machine.Add(Report.Manufacturing.amotizingYearOfMachine, worksheet.Cells[row, excelCol++].Value);
                //    //machine.Add(Report.Manufacturing.machineArea, worksheet.Cells[row, excelCol++].Value);

                //    double rationForSupplementaryMachine = 0;
                //    if (worksheet.Cells[row, excelCol].Value != null && worksheet.Cells[row, excelCol + 1].Value == null && worksheet.Cells[row, excelCol + 2].Value == null)
                //    {
                //        rationForSupplementaryMachine = global.ConvertDouble(worksheet.Cells[row, excelCol].Value);
                //    }
                //    else
                //    {
                //        rationForSupplementaryMachine =
                //       global.ConvertDouble(worksheet.Cells[row, excelCol++].Value) / 100
                //       * global.ConvertDouble(worksheet.Cells[row, excelCol++].Value) /
                //       global.ConvertDouble(worksheet.Cells[row, excelCol++].Value);
                //    }

                //    CellVaildation(Report.Manufacturing.rationForSupplementaryMachine, currencyRow - 2, 26, worksheet, rationForSupplementaryMachine, ref machine);
                //    //machine.Add(Report.Manufacturing.rationForSupplementaryMachine, rationForSupplementaryMachine);

                //    excelCol = 29;
                //    double workingDayPerYear = worksheet.Cells[row, excelCol++].Value * worksheet.Cells[row, excelCol].Value;
                //    CellVaildation(Report.Manufacturing.workingDayPerYear, currencyRow - 2, excelCol, worksheet, workingDayPerYear, ref manufacturing);
                //    CellVaildation(Report.Manufacturing.workingTimePerDay, currencyRow - 2, excelCol, worksheet, worksheet.Cells[row, excelCol++].Value, ref manufacturing);
                //    CellVaildation(Report.Manufacturing.machinePower, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);
                //    CellVaildation(Report.Manufacturing.machinePowerEfficiency, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);
                //    CellVaildation(Report.Manufacturing.machinePowerCost, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);
                //    CellVaildation(Report.Manufacturing.ratioOfMachineRepair, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref machine);
                //    CellVaildation(Report.Manufacturing.ratioOfIndirectlyMachineryCost, currencyRow - 2, excelCol, row, excelCol++, worksheet, ref manufacturing);


                //    if(global.ConvertDouble( machine[Report.Manufacturing.amotizingYearOfMachine])==0)
                //    {
                //        machine[Report.Manufacturing.machineCost] = worksheet.Cells[row, 40].Value;
                //        machine[Report.Manufacturing.usage] = 1;
                //    }
                //    //excelCol = 45;
                //    //CellVaildation(Report.Manufacturing.machineRepairCost, currencyRow - 1, excelCol, worksheet,(worksheet.Cells[row, excelCol++].Value* workingDayPerYear), ref machine);
                //    //manufacturing.Add(Report.Manufacturing.workingDayPerYear, worksheet.Cells[row, excelCol++].Value * worksheet.Cells[row, excelCol].Value);
                //    //manufacturing.Add(Report.Manufacturing.workingTimePerDay, worksheet.Cells[row, excelCol++].Value / 10);
                //    //machine.Add(Report.Manufacturing.machinePower, worksheet.Cells[row, excelCol++].Value);
                //    //machine.Add(Report.Manufacturing.machinePowerEfficiency, worksheet.Cells[row, excelCol++].Value);
                //    //machine.Add(Report.Manufacturing.machinePowerCost, worksheet.Cells[row, excelCol++].Value);
                //    //manufacturing.Add(Report.Manufacturing.ratioOfMachineRepair, worksheet.Cells[row, excelCol++].Value);
                //    //manufacturing.Add(Report.Manufacturing.ratioOfIndirectlyMachineryCost, worksheet.Cells[row, excelCol++].Value);

                //    data.Add(manufacturing);
                //    data.Add(machine);
                //    data.Add(labor);

                //    row++;
                //}

                ////tool
                //increase = (row +3)- 44;
                //temp = new Dictionary<(int, int), string>(cbd.column);
                //cbd.column.Clear();
                //foreach (KeyValuePair<(int, int), string> keyValue in (increase > 0 ? temp.Reverse() : temp))
                //{
                //    cbd.column.Add((keyValue.Key.Item1 + increase, keyValue.Key.Item2), keyValue.Value);
                //}

                //currencyRow = 44 + increase;
                //row = (currencyRow + 2);

                //JObject manufacturingTool = new JObject();
                //manufacturingTool.Add(Report.Manufacturing.manufacturingName, "Proto 금형 & 투자비");
                //manufacturingTool.Add(Report.Manufacturing.sequence, currencyRow);
                //manufacturingTool.Add(Report.LineType.lineType, "C");
                //manufacturingTool.Add(Report.LineType.level, 1);
                //manufacturingTool.Add(Report.Manufacturing.netCycleTime, "1");
                //manufacturingTool.Add(Report.Header.currency, header[Report.Header.currency]);
                //manufacturingTool.Add(Report.Header.partName, header[Report.Header.partName]);

                //data.Add(manufacturingTool);
                //while (true)
                //{
                //    if (((Excel.Range)worksheet.Cells[row, 2]).MergeCells) break;

                //    if (worksheet.Cells[row, 15].Value == null)
                //    {
                //        row++;
                //        continue;
                //    }

                //    JObject tool = new JObject();                   
                //    //manufacturing.Add(Report.Manufacturing.techology, "양산금형비");
                //    tool.Add(Report.LineType.lineType, "T");
                //    tool.Add(Report.LineType.level, 1);
                //    tool.Add(Report.Manufacturing.sequence, currencyRow);
                //    tool.Add(Report.Header.partName, header[Report.Header.partName]);
                //    tool.Add(Report.Header.currency, header[Report.Header.currency]);
                //    manufacturingTool.Add(Report.Manufacturing.usage, 1);

                //    excelCol = 5;
                //    CellVaildation(Report.Tooling.tooling, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.type, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.cavity, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.lifetime, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.method, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);

                //    if (  tool[Report.Tooling.method].Contains("In-house") )
                //    {
                //        tool[Report.Tooling.method] = "Direct input";
                //    }
                //    else
                //    {
                //        tool[Report.Tooling.method] = "Input of percentage rate";
                //    }
                //    excelCol++;
                //    CellVaildation(Report.Tooling.leadtime, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.annualCapa, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.unitCost, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.quantity, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);

                //    data.Add(tool);
                //    row++;
                //}


                ////tool
                //increase = (row + 3) - 54;
                ////temp = new Dictionary<(int, int), string>(cbd.column);
                ////cbd.column.Clear();
                ////foreach (KeyValuePair<(int, int), string> keyValue in (increase > 0 ? temp.Reverse() : temp))
                ////{
                ////    cbd.column.Add((keyValue.Key.Item1 + increase, keyValue.Key.Item2), keyValue.Value);
                ////}

                //row = (54 + increase + 2);
                //manufacturingTool = new JObject();
                //manufacturingTool.Add(Report.Manufacturing.manufacturingName, "양산 금형 & 투자비");
                //manufacturingTool.Add(Report.LineType.lineType, "C");
                //manufacturingTool.Add(Report.Manufacturing.netCycleTime, "1");
                //manufacturingTool.Add(Report.LineType.level, 1);
                //manufacturingTool.Add(Report.Manufacturing.sequence, currencyRow+1);
                //manufacturingTool.Add(Report.Header.currency, header[Report.Header.currency]);
                //manufacturingTool.Add(Report.Header.partName, header[Report.Header.partName]);
                //manufacturingTool.Add(Report.Manufacturing.usage,1);

                //data.Add(manufacturingTool);
                //while (true)
                //{
                //    if (((Excel.Range)worksheet.Cells[row, 2]).MergeCells) break;

                //    if (worksheet.Cells[row, 15].Value == null)
                //    {
                //        row++;
                //        continue;
                //    }

                //    JObject tool = new JObject();
                //    tool.Add(Report.LineType.lineType, "T");
                //    tool.Add(Report.LineType.level, 1);                    
                //    tool.Add(Report.Manufacturing.sequence, currencyRow + 1);
                //    tool.Add(Report.Header.partName, header[Report.Header.partName]);
                //    tool.Add(Report.Header.currency, header[Report.Header.currency]);

                //    excelCol = 5;
                //    CellVaildation(Report.Tooling.tooling, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.type, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    tool[Report.Tooling.type] = tool[Report.Tooling.type]?.ToString().Replace("&","");
                //    CellVaildation(Report.Tooling.cavity, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.lifetime, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.method, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    if (tool[Report.Tooling.method]?.ToString().ToLower().Contains("house")==true)
                //    {
                //        tool[Report.Tooling.method] = "Direct input";
                //    }
                //    else
                //    {
                //        tool[Report.Tooling.method] = "Input of percentage rate";
                //    }
                //    excelCol++;
                //    CellVaildation(Report.Tooling.leadtime, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.annualCapa, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.unitCost, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);
                //    CellVaildation(Report.Tooling.quantity, currencyRow, excelCol, row, excelCol++, worksheet, ref tool);

                //    data.Add(tool);

                //    row++;
                //}

                //String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Import";
                //string response = WebAPI.POST(callUrl, new JObject
                //{
                //    { "Data", data },
                //    { "ConfigurationGuid", global_iniLoad.GetConfig("CBD", "Import") },
                //    { "TargetType", tagetType},
                //    { "TargetId",  tagetID.ToString()},
                //});

                //try
                //{
                //    JObject postResult = JObject.Parse(response);
                //    if ((bool)postResult["success"] == false) err = postResult["message"].ToString();
                //}
                //catch
                //{
                //    err = response;
                //}
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
                else if(dlg.ShowDialog() != DialogResult.OK)
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
                        if(xlRng[row, col] != null) emptyCheck =true;
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
                   if(sheet.Visible==Excel.XlSheetVisibility.xlSheetVisible) workSheetList.Add(sheet.Name);
                }
                workSheetSelect sheetSelect = new workSheetSelect();
                sheetSelect.workSheet = workSheetList;

                if (sheetSelect.ShowDialog() == DialogResult.Cancel) return null;
                string val = sheetSelect.ReturnValue1;

                Excel.Worksheet worksheet = workBook.Worksheets.Item[val];
                //Excel.Range xlRng = worksheet.UsedRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);
                //var lastCell = worksheet.ra;

                object[,] xlRng = worksheet.UsedRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

                string[] keys = new string[xlRng.GetUpperBound(1)+1];
                int firstCol = 0, firstRow;
                for (firstRow = xlRng.GetLowerBound(0); firstRow <= xlRng.GetUpperBound(0); firstRow++)
                {
                    for (int col = xlRng.GetLowerBound(1); col <= xlRng.GetUpperBound(1); col++)
                    {
                        if (xlRng[firstRow, col]==null) continue;
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
                        for (int category= xlRng.GetLowerBound(0); category < keys.Length; category++)
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

                                dgv.Rows[dgv.Rows.Count-1].Cells[col.Name].Value = resultValue;
                                if (new List<string>() { "지역", "공정", "업종", "설비명", "설비구분", "통화", "Valid From", "구분자", "재료명", "메이커", "구분", "비중", "비중 단위", "가격 단위", "스크랩 비용 단위", "이름", "ISO","UOM Code", "UOM 명" }.Contains(col.Name)) continue;

                                if ((((col.Name.Contains("율") || col.Name.Contains("률")) && !col.Name.Contains("임률") && !col.Name.Contains("환율")))&& !col.Name.Contains("수선비율"))
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
