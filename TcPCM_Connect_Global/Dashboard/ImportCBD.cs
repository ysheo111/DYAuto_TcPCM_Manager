using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Microsoft.Office.Interop.Excel;

namespace TcPCM_Connect_Global
{
    /// <summary>
    /// 
    /// </summary>
    public class ImportCBD
    {
        public void Material(DataGridView dgv, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            DataGridViewRow row = dgv.Rows[e.RowIndex];

            if (new List<string>() { Report.Cost.quantity, Report.Cost.netWeight }.Contains(col.Name))
            {
                row.Cells[Report.Cost.scrapQuantity].Value
                    = global.ConvertDouble(row.Cells[Report.Cost.quantity].Value) - global.ConvertDouble(row.Cells[Report.Cost.netWeight].Value);

            }
            else if (new List<string>() { Report.Cost.materialCosts, Report.Cost.quantity,
                Report.Cost.scrapQuantity, Report.Cost.scrapPrice, Report.Cost.loss }.Contains(col.Name)
)
            {
                row.Cells["Value"].Value
                   = (global.ConvertDouble(row.Cells[Report.Cost.materialCosts].Value) * global.ConvertDouble(row.Cells[Report.Cost.quantity].Value) 
                   * (1 + global.ConvertDouble(row.Cells[Report.Cost.loss].Value) / 100))
                     - (global.ConvertDouble(row.Cells[Report.Cost.scrapQuantity].Value) * global.ConvertDouble(row.Cells[Report.Cost.scrapPrice].Value));

                if (!dgv.Columns["TotalValue"].Visible) row.Cells["TotalValue"].Value = row.Cells["Value"].Value;

            }
            else if (new List<string>() { "Value", "소요량" }.Contains(col.Name))
            {
                row.Cells["TotalValue"].Value = global.ConvertDouble(row.Cells["Value"].Value) * global.ConvertDouble(row.Cells["소요량"].Value);
            }
            else if (col.Name == "TotalValue")
            {
                //row.Cells["overheads"].Value = global.ConvertDouble(row.Cells["TotalValue"].Value) * reference[Report.Cost.materialOverheads];
            }
        }

        public void External(DataGridView dgv, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            DataGridViewRow row = dgv.Rows[e.RowIndex];

            if (new List<string>() { Report.Cost.materialCosts, Report.Cost.quantity }.Contains(col.Name))
            {
                row.Cells["TotalValue"].Value 
                    = global.ConvertDouble(row.Cells[Report.Cost.materialCosts].Value) * global.ConvertDouble(row.Cells[Report.Cost.quantity].Value);
            }
            else if (col.Name == "TotalValue")
            {
                //row.Cells["overheads"].Value = global.ConvertDouble(row.Cells["TotalValue"].Value) * reference[Report.Cost.externalmaterialOverheads];
            }
        }

        public void Manufacturing(DataGridView dgv, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            DataGridViewRow row = dgv.Rows[e.RowIndex];

            string shift = row.Cells[Report.Designation.shift].Value == null ? "2Shift" : row.Cells[Report.Designation.shift].Value?.ToString();
            string category = row.Cells[Report.Designation.machineCategory].Value == null ? "범용" : row.Cells[Report.Designation.machineCategory].Value?.ToString();

            //if (new List<string>() { Report.Cost.cycleTime, Report.Cost.maunCavity, Report.Cost.setupTime }.Contains(col.Name))
            //{
            //    //lot
            //    row.Cells[Report.Cost.lot].Value =
            //        Math.Truncate((global.ConvertDouble(reference[shift]) * 3600 - global.ConvertDouble(row.Cells[Report.Cost.setupTime].Value) * 60)
            //        / Math.Round(global.ConvertDouble(row.Cells[Report.Cost.cycleTime].Value), 2) * global.ConvertDouble(row.Cells[Report.Cost.maunCavity].Value));
            //}
            //else if (col.Name == Report.Designation.shift)
            //{
            //    //lot, 기계상각비, 건물상각비                
            //    //lot
            //    row.Cells[Report.Cost.lot].Value =
            //          Math.Truncate((global.ConvertDouble(reference[shift]) * 3600 - global.ConvertDouble(row.Cells[Report.Cost.setupTime].Value) * 60)
            //          / Math.Round( global.ConvertDouble(row.Cells[Report.Cost.cycleTime].Value),2)* global.ConvertDouble(row.Cells[Report.Cost.maunCavity].Value));

            //    //기계상각비
            //    row.Cells[Report.Cost.imputed].Value =
            //         global.ConvertDouble(row.Cells[Report.Cost.acquisition].Value) * 1000 / reference[category]
            //         / (reference[Report.Cost.workhour] * reference[shift]);

            //    //건물상각비
            //    row.Cells[Report.Cost.space].Value =
            //      global.ConvertDouble(row.Cells[Report.Cost.auxiliaryArea].Value) * reference[Report.Cost.spaceMachine]
            //      / (reference[Report.Cost.workhour] * reference[shift]);

            //}
            //else if (col.Name == Report.Cost.lot)
            //{
            //    //ct
            //    row.Cells["CycleTime"].Value =
            //        global.ConvertDouble(row.Cells[Report.Cost.cycleTime].Value) * (1 + reference[Report.Cost.marginRate])
            //        / global.ConvertDouble(row.Cells[Report.Cost.maunCavity].Value)
            //        + global.ConvertDouble(row.Cells[Report.Cost.setupTime].Value) * 60 / global.ConvertDouble(row.Cells[Report.Cost.lot].Value);
            //}
            //else if (new List<string>() { "CycleTime", "소요량" }.Contains(col.Name))
            //{
            //    //노무비계
            //    double cycleTimeSetup = global.ConvertDouble(row.Cells[Report.Cost.cycleTime].Value) * (1 + reference[Report.Cost.marginRate])
            //    / global.ConvertDouble(row.Cells[Report.Cost.maunCavity].Value)
            //    + global.ConvertDouble(row.Cells[Report.Cost.setupTime].Value) * (1 + reference[Report.Cost.manufacturingOverheads]) * 60
            //    / global.ConvertDouble(row.Cells[Report.Cost.lot].Value);

            //    if (double.IsNaN(cycleTimeSetup)) row.Cells["totalLabor"].Value = 0;
            //    else
            //    {
            //        row.Cells["totalLabor"].Value =
            //           global.ConvertDouble(row.Cells["소요량"].Value ?? 1) * cycleTimeSetup
            //           * global.ConvertDouble(row.Cells[Report.Cost.laborNum].Value) * global.ConvertDouble(row.Cells[Report.Cost.laborCosts].Value) / 3600;
            //    }

            //    //경비 계
            //    row.Cells["totalMachine"].Value =
            //        global.ConvertDouble(row.Cells[Report.Cost.machineCosts].Value) / 3600 * global.ConvertDouble(row.Cells["CycleTime"].Value)
            //        * global.ConvertDouble(row.Cells["소요량"].Value ?? 1);
            //}
            //else if (new List<string>() { Report.Cost.laborNum, Report.Cost.laborCosts }.Contains(col.Name))
            //{
            //    //노무비계
            //    double cycleTimeSetup = global.ConvertDouble(row.Cells[Report.Cost.cycleTime].Value) * (1 + reference[Report.Cost.marginRate])
            //    / global.ConvertDouble(row.Cells[Report.Cost.maunCavity].Value)
            //    + global.ConvertDouble(row.Cells[Report.Cost.setupTime].Value) * (1 + reference[Report.Cost.manufacturingOverheads]) * 60
            //    / global.ConvertDouble(row.Cells[Report.Cost.lot].Value);

            //    if (double.IsNaN(cycleTimeSetup)) row.Cells["totalLabor"].Value = 0;
            //    else
            //    {
            //        row.Cells["totalLabor"].Value =
            //           global.ConvertDouble(row.Cells["소요량"].Value ?? 1) * cycleTimeSetup
            //           * global.ConvertDouble(row.Cells[Report.Cost.laborNum].Value) * global.ConvertDouble(row.Cells[Report.Cost.laborCosts].Value) / 3600;
            //    }
            //}
            //else if (new List<string>() { Report.Cost.acquisition, Report.Designation.machineCategory}.Contains(col.Name))
            //{
            //    //기계상각비
            //    row.Cells[Report.Cost.imputed].Value =
            //         global.ConvertDouble(row.Cells[Report.Cost.acquisition].Value) * 1000 / reference[category] 
            //         / (reference[Report.Cost.workhour] * reference[shift]);
            //}
            //else if (col.Name == Report.Cost.auxiliaryArea)
            //{
            //    //건물상각비
            //    row.Cells[Report.Cost.space].Value =
            //      global.ConvertDouble(row.Cells[Report.Cost.auxiliaryArea].Value) * reference[Report.Cost.spaceMachine]
            //      / (reference[Report.Cost.workhour] * reference[shift]);
            //}
            //else if (new List<string>() { Report.Cost.ratePower, Report.Cost.utilizationPower }.Contains(col.Name))
            //{
            //    //전력비
            //    row.Cells[Report.Cost.energyCostRate].Value =
            //        global.ConvertDouble(row.Cells[Report.Cost.ratePower].Value) * global.ConvertDouble(row.Cells[Report.Cost.utilizationPower].Value)/100
            //        * reference[Report.Cost.energyMachine];
            //}
            //else if (new List<string>() { Report.Cost.imputed, Report.Cost.space }.Contains(col.Name))
            //{
            //    //수선비, 계
            //    //수선비
            //    row.Cells[Report.Cost.maintance].Value =
            //        (global.ConvertDouble(row.Cells[Report.Cost.imputed].Value) + global.ConvertDouble(row.Cells[Report.Cost.space].Value))
            //        * reference[Report.Cost.maintance + "_" + shift];

            //    row.Cells[Report.Cost.machineCosts].Value =
            //      (global.ConvertDouble(row.Cells[Report.Cost.imputed].Value) + global.ConvertDouble(row.Cells[Report.Cost.space].Value)
            //      + global.ConvertDouble(row.Cells[Report.Cost.energyCostRate].Value) + global.ConvertDouble(row.Cells[Report.Cost.maintance].Value))
            //       * (1 + reference[Report.Cost.manufacturingOverheads]);
            //}
            //else if (new List<string>() { Report.Cost.energyCostRate, Report.Cost.maintance }.Contains(col.Name))
            //{
            //    //계
            //    row.Cells[Report.Cost.machineCosts].Value =
            //      (global.ConvertDouble(row.Cells[Report.Cost.imputed].Value) + global.ConvertDouble(row.Cells[Report.Cost.space].Value)
            //      + global.ConvertDouble(row.Cells[Report.Cost.energyCostRate].Value) + global.ConvertDouble(row.Cells[Report.Cost.maintance].Value))
            //      * (1+reference[Report.Cost.manufacturingOverheads]);
            //}
            //else if (col.Name == Report.Cost.machineCosts)
            //{
            //    //경비 계
            //    row.Cells["totalMachine"].Value =
            //        global.ConvertDouble(row.Cells[Report.Cost.machineCosts].Value) / 3600 * global.ConvertDouble(row.Cells["CycleTime"].Value)
            //        * global.ConvertDouble(row.Cells["소요량"].Value ?? 1);
            //}
        }

        private void PCMImportMapping
            (int level, Dictionary<string, object> item, ref int sequenceNumber, ref JObject process, ref JArray data)
        {
            if (item["Category"].ToString().ToLower().Contains("material"))
            {
                if (process.Count != 0) data.Add(process);
                process = new JObject();
                JObject scrap = new JObject();

                process.Add(Report.LineType.level, level+1);
                process.Add(Report.LineType.lineType, "P");
                process.Add(Report.LineType.mode, "Siemens.TCPCM.CalculationQuality.Standardprice");
                process.Add(Report.Designation.procument, "Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");

                scrap.Add(Report.LineType.level, level + 1);
                scrap.Add(Report.LineType.lineType, "P");
                scrap.Add(Report.LineType.mode, "Siemens.TCPCM.CalculationQuality.Standardprice");
                scrap.Add(Report.Designation.procument, "Siemens.TCPCM.ProcurementType.Purchase_RawMaterial");

                int unitFactor = 1000 * 1000;
                string unitName = "mg";
                if (new List<string> { "EA", "pcs", "Pcs" }.Contains(item[Report.Cost.materialPriceUnit]?.ToString()))
                {
                    unitFactor = 10000;
                    unitName = $"{(item[Report.Cost.materialPriceUnit]?.ToString().ToLower()=="pcs"?"pcs":"EA")}/10000";
                }
                else if (new List<string> { "ml", "mL" }.Contains(item[Report.Cost.materialPriceUnit]?.ToString()))
                {
                    unitFactor = 1000;
                    unitName = $"ul";
                }

                foreach (var subItem in item)
                {
                    if (new List<string> { Report.Designation.substance, Report.Cost.density, Report.Cost.materialPriceUnit }.Contains(subItem.Key))
                    {
                        string unit = subItem.Value?.ToString();
                        if (new List<string> { "pcs", "Pcs" }.Contains(item[Report.Cost.materialPriceUnit]?.ToString())) unit = $"{item[Report.Cost.materialPriceUnit]?.ToString().ToLower()}";
                        process.Add(subItem.Key, unit);
                        scrap.Add(subItem.Key, unit);
                    }
                    else if (subItem.Key == Report.Designation.basic)
                    {
                        process.Add(subItem.Key, subItem.Value?.ToString());
                        scrap.Add(subItem.Key, subItem.Value?.ToString() + "_Scrap");
                    }
                    else if (subItem.Key == Report.Cost.quantity)
                    {
                        process.Add(subItem.Key, global.ConvertDouble(subItem.Value) * unitFactor);
                    }
                    else if (subItem.Key == Report.Cost.scrapPrice) scrap.Add(Report.Cost.materialCosts, subItem.Value?.ToString());
                    else if (subItem.Key == Report.Cost.scrapQuantity)
                    {
                        scrap.Add(Report.Cost.quantity, -global.ConvertDouble(subItem.Value) * unitFactor);
                    }
                    else if (subItem.Key.Contains("[mm]")) process.Add(subItem.Key, global.ConvertDouble(subItem.Value) / unitFactor);
                    else if (!process.ContainsKey(subItem.Key)) process.Add(subItem.Key, subItem.Value?.ToString());
                }

                process.Add(Report.Cost.materailType, item[Report.Cost.length] != null ? "Siemens.TCPCM.Classification.Material.SemiFinished.SheetMetal.Plate" : "");
                process.Add(Report.LineType.materialMode, item[Report.Cost.length] != null ? "Siemens.TCPCM.MaterialCalculatorTemplate.Plate" : "");
                process.Add(Report.Cost.materialQuantityUnit, unitName);
                scrap.Add(Report.Cost.materialQuantityUnit, unitName);

                data.Add(process);
                if(global.ConvertDouble(scrap[Report.Cost.materialCosts])!=0) data.Add(scrap);
                process = new JObject();

                //item.Add(CBD.Import.vaildFrom, DateTime.Today);
                //item.Add(CBD.Import.designation, process);   ,

            }
            else if (item["Category"].ToString().ToLower().Contains("external"))
            {
                if (process.Count != 0) data.Add(process);
                process = new JObject();
                process.Add(Report.LineType.level, level + 1);
                process.Add(Report.LineType.lineType, "P");
                process.Add(Report.LineType.mode, "Siemens.TCPCM.CalculationQuality.Standardprice");
                process.Add(Report.Designation.procument, "Siemens.TCPCM.ProcurementType.InternalPurchase");
                process.Add(Report.Designation.basic + " (Calculation)", "Outsourcing Material");

                foreach (var subItem in item)
                {

                    if (process.ContainsKey(subItem.Key)) continue;
                    else if (global.ConvertDouble(item[Report.Cost.quantity]) % 1 != 0 
                        && (subItem.Key== Report.Cost.quantity || subItem.Key == Report.Cost.materialQuantityUnit))
                    {
                        if (new List<string> { "EA", "pcs", "Pcs" }.Contains(item[Report.Cost.materialQuantityUnit]?.ToString()))
                        {
                            if (subItem.Key == Report.Cost.quantity ) process.Add(subItem.Key, global.ConvertDouble(subItem.Value) * 10000); 
                            else if (subItem.Key == Report.Cost.materialQuantityUnit) process.Add(subItem.Key, $"{ (item[Report.Cost.materialQuantityUnit]?.ToString().ToLower() == "pcs" ? "pcs" : "EA")}/10000");                            
                        }
                        else if (new List<string> { "KG", "kg","Kg" }.Contains(item[Report.Cost.materialQuantityUnit]?.ToString()))
                        {
                            if (subItem.Key == Report.Cost.quantity) process.Add(subItem.Key, global.ConvertDouble(subItem.Value) * 1000 * 1000);
                            else if (subItem.Key == Report.Cost.materialQuantityUnit) process.Add(subItem.Key, "mg");
                        }
                        else if (new List<string> { "ml", "mL" }.Contains(item[Report.Cost.materialQuantityUnit]?.ToString()))
                        {
                            if (subItem.Key == Report.Cost.quantity) process.Add(subItem.Key, global.ConvertDouble(subItem.Value) * 1000);
                            else if (subItem.Key == Report.Cost.materialQuantityUnit) process.Add(subItem.Key, "ul");
                        }
                    }
                    else process.Add(subItem.Key, subItem.Value?.ToString());
                }

                data.Add(process);
                process = new JObject();
            }
            else if (item["Category"].ToString().ToLower().Contains("subpart"))
            {
                if (process.Count != 0) data.Add(process);
                level++;

                List<Dictionary<string, object>> subPartInfo = ((Dictionary<string, List<Dictionary<string, object>>>)item["value"]).First().Value;
                JObject subPartProcess = new JObject();

                Dictionary<string, double> subPartReference = (Dictionary<string, double>)(subPartInfo.Find(x => x.ContainsValue("dgv_BaicInfo"))["MasterData"]);
                subPartProcess.Add(Report.LineType.level, level);
                subPartProcess.Add(Report.Cost.materialOverheads, subPartReference[Report.Cost.materialOverheads]*100);
                subPartProcess.Add(Report.Cost.externalmaterialOverheads, subPartReference[Report.Cost.externalmaterialOverheads] * 100);
                subPartProcess.Add(Report.LineType.lineType, "P");
                subPartProcess.Add(Report.Designation.basic + " (Calculation)", "Benchmark price");
                subPartProcess.Add(Report.Cost.profit, subPartReference[Report.Cost.profitPer] * 100);
                subPartProcess.Add(Report.Cost.overheads, subPartReference[Report.Cost.overheadsPer] * 100);
                subPartProcess.Add(Report.Cost.quantity, item[Report.Cost.quantity]?.ToString());

                subPartProcess.Add(Report.Cost.materialQuantityUnit, item[Report.Cost.materialQuantityUnit]?.ToString() == "Pcs" ? "pcs":item[Report.Cost.materialQuantityUnit]?.ToString());
                foreach (var info in subPartInfo)
                {
                    PCMImportMapping(level, info, ref sequenceNumber, ref subPartProcess, ref data);
                }

                process = new JObject();
            }
            else if (item["Category"].ToString().ToLower().Contains("manufacturing"))
            {
                if (process.Count != 0) data.Add(process);
                process = new JObject();
                process.Add(Report.LineType.level, level+1);
                process.Add(Report.LineType.lineType, "D");
                process.Add(Report.Designation.basic, item[Report.Designation.basic]?.ToString());
                //process.Add(Report.Designation.shift, item[Report.Designation.shift]?.ToString());
                process.Add(Report.Cost.manufacturingCategory, sequenceNumber * 10);
                process.Add(Report.Cost.cycleTime, item[Report.Cost.cycleTime]?.ToString());
                process.Add(Report.Cost.maunCavity, item[Report.Cost.maunCavity]?.ToString());

                //process.Add(Report.Cost.lot, item[Report.Cost.lot]?.ToString());
                //process.Add(Report.Cost.shifts, 
                //    reference.ContainsKey(item[Report.Designation.shift]?.ToString() ?? "") ? reference[item[Report.Designation.shift]?.ToString()] / 10 : 2);

                //process.Add(Report.Cost.workhour, global.ConvertDouble(process[Report.Cost.shifts]) * 10 * reference[Report.Cost.workhour]);
                //process.Add(Report.Cost.marginRate, reference[Report.Cost.marginRate] * 100);
                //process.Add(Report.Cost.manufacturingOverheads, reference[Report.Cost.manufacturingOverheads] * 100);
                //process.Add(Report.Cost.setupTime, item[Report.Cost.setupTime]?.ToString());

                //double maintance
                //   = global.ConvertDouble(item[Report.Cost.maintance]) * global.ConvertDouble(process[Report.Cost.shifts]) * 10 * reference[Report.Cost.workhour];

                //data.Add(process);

                //process = new JObject();
                //process.Add(Report.LineType.level, level+1);
                //process.Add(Report.LineType.lineType, "M");
                //process.Add(Report.Designation.basic, item[Report.Designation.machine]?.ToString());
                //process.Add(Report.Designation.machine, item[Report.Designation.machine]?.ToString());
                //process.Add(Report.Designation.machineCategory, item[Report.Designation.machineCategory]?.ToString());
                //process.Add(Report.Cost.manufacturingCategory, sequenceNumber * 10);
                //process.Add(Report.Cost.setupLaborNum, item[Report.Cost.laborNum]?.ToString());
                //process.Add(Report.Cost.setUpLabor, item[Report.Cost.laborCosts]?.ToString());
                //process.Add(Report.Cost.acquisition, global.ConvertDouble(item[Report.Cost.acquisition]) * 1000);

                //process.Add(Report.Cost.imputed,
                //    reference.ContainsKey(item[Report.Designation.machineCategory]?.ToString()??"") ? reference[item[Report.Designation.machineCategory]?.ToString()]  : 10);

                //process.Add(Report.Cost.auxiliaryArea, item[Report.Cost.auxiliaryArea]?.ToString());
                //process.Add(Report.Cost.space, reference[Report.Cost.spaceMachine] / 12);
                //process.Add(Report.Cost.ratePower, item[Report.Cost.ratePower]?.ToString());
                //process.Add(Report.Cost.utilizationPower, item[Report.Cost.utilizationPower]?.ToString());
                //process.Add(Report.Cost.energyCostRate, reference[Report.Cost.energyMachine]);
                //               process.Add(Report.Cost.maintance, maintance);
                //process.Add(Report.Cost.quantity, 1);
                //data.Add(process);

                process = new JObject();
                process.Add(Report.LineType.level, level+1);
                process.Add(Report.LineType.lineType, "L");
                process.Add(Report.Designation.basic, "노무비");

                string[] fraction = FractionConverter.Convert(Convert.ToDecimal(item[Report.Cost.laborNum])).Replace(" ", "").Split('/');
                process.Add(Report.Cost.quantity, fraction[0]);
                process.Add(Report.Cost.attendNum, fraction.Length > 1 ? fraction[1] : "1");
                process.Add(Report.Cost.laborCosts, item[Report.Cost.laborCosts]?.ToString());
                process.Add(Report.Cost.manufacturingCategory, sequenceNumber * 10);
                data.Add(process);
                process = new JObject();
                sequenceNumber++;
            }
            else
            {
                foreach (var subItem in item)
                {
                    if (process.ContainsKey(subItem.Key)) continue;
                    if (subItem.Key == Report.Designation.basic)
                    {
                        process.Add(Report.Designation.basic,
                            item[Report.Designation.basic] == null ? "new part" : item[Report.Designation.basic]?.ToString());
                    }
                    else if (subItem.Key == Report.Designation.dateOfCalc)
                    {
                        process.Add(subItem.Key, subItem.Value is DateTime ? ((DateTime)subItem.Value).ToString("yyyy-MM-dd") : "");
                    }
                    else process.Add(subItem.Key, subItem.Value?.ToString());
                }
            }
        }

        public string Import(Dictionary<string, List<Dictionary<string, object>>> exportData, string target)
        {
            JArray data = new JArray();
            int sequenceNumber = 0;

            foreach (var part in exportData)
            {
                int level = 1;
                JObject process = new JObject();
                Dictionary<string, double> reference = (Dictionary<string, double>)(part.Value.Find(x => x.ContainsValue("dgv_BaicInfo"))["MasterData"]);
                process.Add(Report.LineType.level, level);
                process.Add(Report.Cost.materialOverheads, reference[Report.Cost.materialOverheads] * 100);
                process.Add(Report.Cost.externalmaterialOverheads, reference[Report.Cost.externalmaterialOverheads] * 100);
                process.Add(Report.Cost.profit, reference[Report.Cost.profitPer] * 100);
                process.Add(Report.Cost.overheads, reference[Report.Cost.overheadsPer] * 100);
                process.Add(Report.LineType.lineType, "P");
                process.Add(Report.Cost.quantity,1);
                process.Add(Report.Designation.basic + " (Calculation)", "Benchmark price");
                foreach (var info in part.Value)
                {
                    PCMImportMapping(level, info,  ref sequenceNumber, ref process, ref data);
                }
            }

            JObject postData = new JObject()
            {
                {"Data",data },
                { "PlantUniqueKey","" },
                { "ConfigurationGuid",global_iniLoad.GetConfig("CBD", "Import") },
                { "TargetType", (target.Contains("f") ? "Folder" :"Project")},
                { "TargetID", target.Substring(1,target.Length-1) },
            };

            String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Import";
            string err = null;
            //return err;
            return WebAPI.ErrorCheck(WebAPI.POST(callUrl, postData), err);
        }

        public void DGVImport(DataGridView dgv, Dictionary<string, object> content)
        {
            dgv.Rows.Add();
            DataGridViewRow row = dgv.Rows[dgv.Rows.Count - 1];

            int idx = 0;
            foreach (var item in content)
            {
                idx = ColumnValidCheck(dgv, idx);
                if (dgv.Columns.Count <= idx) break;

                row.Cells[idx].Value = item.Value;
                idx++;
            }
        }

        public void SubPartDGVImport(DataGridView dgv, Dictionary<string, object> content)
        {
            dgv.Rows.Add();
            DataGridViewRow row = dgv.Rows[dgv.Rows.Count - 1];

            foreach(DataGridViewColumn col in dgv.Columns)
            {
                if(content.ContainsKey(col.Name)) row.Cells[col.Name].Value = content[col.Name];
            }
        }

        private int ColumnValidCheck(DataGridView dgv, int idx)
        {
            while(true)
            {
                if (dgv.Columns.Count <= idx) break;
                else if (!dgv.Columns[idx].Visible || dgv.Columns[idx].ReadOnly) idx++;
                else break;
            }

            return idx;
        }

        public Dictionary<string, DataGridView> TcPCMMatching
            (Workbook workBook, Worksheet worksheet, Bom.ExportLang mode, string quantity, string quantityUnit, string price, string substance)
        {
            //Excel의 Marker 읽어옴
            Microsoft.Office.Interop.Excel.Range range = worksheet.UsedRange;

            List<int> marker = new List<int>();
            for (int i = 1; i <= range.Rows.Count + 1; i++)
            {
                string CellColor = worksheet.Cells[i, 2].Interior.Color.ToString();
                if (worksheet.Cells[i, 2].Interior.Color.Equals(11052288)) marker.Add(i);
            }

            //사용중인 범위(한번도 사용하지 않은 범위는 포함되지 않음)
            Range usedRange = worksheet.UsedRange;

            //마지막 Cell
            Range lastCell = usedRange.SpecialCells(XlCellType.xlCellTypeLastCell);

            //전체 범위 (왼쪽 상단의 Cell부터 사용한 맨마지막 범위까지)
            Range totalRange = worksheet.get_Range(worksheet.get_Range("A1"), lastCell);

            object[,] xlRng = totalRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);
            
            int excelOrder = 0;

            ExcelImport import = new ExcelImport();

            Dictionary<string, DataGridView> assySorting = new Dictionary<string, DataGridView>();
            assySorting.Add("종합", import.GetExcelData(xlRng, marker, 1, excelOrder, range.Columns.Count, "dgv_BaicInfo"));
            excelOrder++;

            assySorting.Add("요약", import.GetExcelData(xlRng, marker, 3, excelOrder, range.Columns.Count, "dgv_Summary"));
            excelOrder++;

            assySorting.Add("하위파트", import.GetExcelData(xlRng, marker, 1, excelOrder, range.Columns.Count, "dgv_SubPart"));
            excelOrder++;

            assySorting.Add("재료", import.GetExcelData(xlRng, marker, 2, excelOrder, range.Columns.Count, "dgv_Material"));
            excelOrder++;

            assySorting.Add("공정", import.GetExcelData(xlRng, marker, 2, excelOrder, range.Columns.Count, "dgv_Manufacturing"));
            excelOrder++;

            assySorting.Add("기타", import.GetExcelData(xlRng, marker, 1, excelOrder, range.Columns.Count, "dgv_Etc"));
            excelOrder++;

            return assySorting;
            //Basic
            //List<int> basicCol = new List<int>();
            //for (int i = 1; i <= range.Columns.Count + 1; i++)
            //{
            //    if (worksheet.Cells[marker[excelOrder] + 1, i].Value != null) basicCol.Add(i);
            //}

            //int useColIdxBasic = 0;
            //Dictionary<int, string> basic = new Dictionary<int, string>();
            //DataGridView dgv_BaicInfo = new DataGridView(); dgv_BaicInfo.Name = "dgv_BaicInfo"; dgv_BaicInfo.AllowUserToAddRows = false;

            //for (int i = 1; i < range.Columns.Count; i++)
            //{
            //    if (xlRng[marker[excelOrder]+1, i] != null) dgv_BaicInfo.Columns.Add($"{xlRng[marker[excelOrder] + 1, i]}", i.ToString());
            //    //basic.Add(i, $"{xlRng[marker[excelOrder] +1, i]}");
            //}

            //for(int i= marker[excelOrder] + 2; i< marker[excelOrder+1];i++)
            //{
            //    dgv_BaicInfo.Rows.Add();
            //    for (int j=0; j< dgv_BaicInfo.Columns.Count; j++)
            //    {
            //        dgv_BaicInfo.Rows[dgv_BaicInfo.Rows.Count - 1].Cells[j].Value = xlRng[i, int.Parse(dgv_BaicInfo.Columns[j].HeaderText)];
            //    }                
            //}


            //bool rowNullCheck = false;
            ////원/부재료
            ////원/부재료의 통합 컬럼의 첫머리를 List에 추가
            //Dictionary<int, string> materialCol = new Dictionary<int, string>();
            //for (int i = 1; i < range.Columns.Count; i++)
            //{
            //    if (xlRng[marker[excelOrder] - 1, i] != null) materialCol.Add(i, $"{xlRng[marker[excelOrder] - 1, i]}");
            //    else if (xlRng[marker[excelOrder] - 2, i] != null) materialCol.Add(i, $"{xlRng[marker[excelOrder] - 2, i]}");

            //    //if (worksheet.Cells[marker[excelOrder] - 1, i].Value != null) materialCol.Add(i,$"{worksheet.Cells[marker[excelOrder] - 1, i].Value}");
            //    //else if (worksheet.Cells[marker[excelOrder] - 2, i].Value != null) materialCol.Add(i,$"{worksheet.Cells[marker[excelOrder] - 2, i].Value}");
            //}

            //for (int i = marker[excelOrder]; i < marker[excelOrder + 1] - 5; i++)
            //{
            //    Dictionary<string, object> materialInfo = new Dictionary<string, object>();
            //    rowNullCheck = false;
            //    foreach (var col in materialCol)
            //    {
            //        object value;

            //        if ((col.Value.ToLower().Contains("scrap") && !(col.Value.ToLower().Contains("단가") || col.Value.ToLower().Contains("cost")))
            //            || col.Value.Contains("관리비") || col.Value.Contains("Overhead") || col.Value.Contains("계") || col.Value.Contains("Total")) continue;

            //        else if ((col.Value.Contains("율") || col.Value.Contains("률") || col.Value.Contains("Rate")) && xlRng[i, col.Key]?.ToString().Length > 0)
            //        {
            //            value = global.ConvertDouble(xlRng[i, col.Key]) * 100;
            //            //value = global.ConvertDouble(worksheet.Cells[i, col.Key].Value) * 100;
            //        }
            //        else value = xlRng[i, col.Key];

            //        if (value?.ToString().Length > 0) rowNullCheck = true;
            //        materialInfo.Add(col.Value, value);
            //    }

            //    if(rowNullCheck) partInfo.Add($"소재_{i}", materialInfo);
            //    else break;
            //}
            //excelOrder++;

            ////서브 파트
            //int externalStart = 1, externalEnd = 3;

            //Dictionary<int, string> subPartCol = new Dictionary<int, string>();
            //bool subPartEnd = false;
            ////List<int> subPartCol = new List<int>();
            //for (int i = 1; i < range.Columns.Count; i++)
            //{
            //    if (worksheet.Cells[marker[excelOrder] - 3, i].Value != null && !subPartEnd) subPartEnd = true;
            //    else if (worksheet.Cells[marker[excelOrder] - 3, i].Value != null && subPartEnd)
            //    {
            //        externalStart = i;
            //        break;
            //    }

            //    if (xlRng[marker[excelOrder] - 1, i] != null) subPartCol.Add(i, $"{xlRng[marker[excelOrder] - 1, i]}");
            //    else if (xlRng[marker[excelOrder] - 2, i] != null) subPartCol.Add(i, $"{xlRng[marker[excelOrder] - 2, i]}");
            //}

            //for (int i = marker[excelOrder]; i < marker[excelOrder + 1] - 5; i++)
            //{
            //    string subPartName = null, subPartquantity = null, subPartUnit = null, subPartPrice = null, subPartSubstance = null;

            //    foreach (var col in subPartCol)
            //    {
            //        if (col.Value.Contains("명") || col.Value.Contains("Designation")) subPartName = $"{xlRng[i, col.Key]}";
            //        else if (col.Value.Contains("소요량") || col.Value.Contains("Usage")) subPartquantity = $"{xlRng[i, col.Key]}";
            //        else if (col.Value.Contains("단위") || col.Value.Contains("Unit")) subPartUnit = $"{xlRng[i, col.Key]}";
            //        else if (col.Value.Contains("단가") || col.Value.Contains("Cost Per Each")) subPartPrice = $"{xlRng[i, col.Key]}";
            //        else if (col.Value.Contains("재질") || col.Value.Contains("Materials")) subPartSubstance = $"{xlRng[i, col.Key]}";

            //        if(subPartName != null && subPartquantity != null &&  subPartUnit != null && subPartPrice != null && subPartSubstance != null)
            //        {
            //            if (workBook.Worksheets.OfType<Worksheet>().FirstOrDefault(ws => ws.Name == subPartName) == null) continue;

            //            Dictionary<string, Dictionary<string, object>> subPartInfo = TcPCMMatching(workBook, workBook.Sheets[subPartName], mode, subPartquantity, subPartUnit,subPartPrice, subPartSubstance);
            //            partInfo.Add($"하위파트_{col.Key}_{i}", subPartInfo.ToDictionary(pair => pair.Key, pair => (object)pair.Value));
            //            subPartName = subPartquantity = subPartUnit = subPartPrice = null;
            //        }
            //    }                
            //}

            ////외부 재료비
            //Dictionary<int, string> externalCol = new Dictionary<int, string>();
            //for (int i = externalStart; i < range.Columns.Count; i++)
            //{
            //    if (xlRng[marker[excelOrder] - 1, i] != null) externalCol.Add(i, $"{xlRng[marker[excelOrder] - 1, i]}");
            //    else if (xlRng[marker[excelOrder] - 2, i] != null) externalCol.Add(i, $"{xlRng[marker[excelOrder] - 2, i]}");
            //}

            //for (int i = marker[excelOrder]; i < marker[excelOrder + 1] - 5; i++)
            //{
            //    Dictionary<string, object> externalMaterialInfo = new Dictionary<string, object>();
            //    rowNullCheck = false;

            //    foreach (var col in externalCol)
            //    {
            //        object value;

            //        if (col.Value.Contains("관리비") || col.Value.Contains("Overhead") || col.Value.Contains("계") || col.Value.Contains("Total")) continue;
            //        else if ((col.Value.Contains("율") || col.Value.Contains("률") || col.Value.Contains("Rate")) && xlRng[i, col.Key]?.ToString().Length > 0)
            //        { 
            //            value = global.ConvertDouble(xlRng[i, col.Key]) * 100;
            //        }
            //        else value = xlRng[i, col.Key];

            //        if(externalMaterialInfo.ContainsKey(col.Value))
            //        {
            //            if (externalMaterialInfo.Count > 0) partInfo.Add($"외주_1_{i}", externalMaterialInfo);
            //            externalMaterialInfo = new Dictionary<string, object>();
            //        }

            //        if(value?.ToString().Length>0) rowNullCheck = true;
            //        externalMaterialInfo.Add(col.Value, value);
            //    }

            //    if (rowNullCheck) partInfo.Add($"외주_2_{i}", externalMaterialInfo);
            //    else break;
            //}

            //excelOrder++;

            ////가공비
            //Dictionary<int, string> manufacturingCol = new Dictionary<int, string>();
            //for (int i = 1; i < range.Columns.Count; i++)
            //{
            //    if (xlRng[marker[excelOrder] - 1, i] != null) manufacturingCol.Add(i, $"{xlRng[marker[excelOrder] - 1, i] }");
            //    else if (xlRng[marker[excelOrder] - 2, i] != null) manufacturingCol.Add(i, $"{xlRng[marker[excelOrder] - 2, i] }");
            //}
            //for (int i = marker[excelOrder]; i <= marker[excelOrder + 1] - 3; i++)
            //{
            //    Dictionary<string, object> manufacturingInfo = new Dictionary<string, object>();
            //    rowNullCheck = false;
            //    foreach (var col in manufacturingCol)
            //    {
            //        object value;

            //        if ((col.Value.Contains("율") || col.Value.Contains("률") || col.Value.Contains("Rate")) && !col.Value.Contains("임") && xlRng[i, col.Key]?.ToString().Length > 0)
            //        {
            //            value = global.ConvertDouble(xlRng[i, col.Key]) * 100;
            //        }
            //        else if (col.Value.ToLower().Contains("계") || col.Value.Contains("Total")
            //            || (col.Value.Contains("비") && !col.Value.Contains("설비") && !col.Value.Contains("준비"))
            //            || (col.Value.Contains("Depreciation") || col.Value.Contains("Electricity Costs") || col.Value.Contains("Repairing"))
            //            || col.Value.Contains("Lot") || (col.Value.Contains("C/T") && !col.Value.Contains("Net")))
            //        {
            //            continue;
            //        }
            //        else if (col.Value.Contains("Machine Div"))
            //        {
            //            value = xlRng[i, col.Key]?.ToString()?.ToString().Contains("Private") == true ? "전용" : "범용";
            //        }
            //        else value = xlRng[i, col.Key];

            //        if (value?.ToString().Length > 0) rowNullCheck = true;
            //        manufacturingInfo.Add(col.Value, value);
            //    }

            //    if (rowNullCheck) partInfo.Add($"공정_{i}", manufacturingInfo);
            //    else break;

            //}
            //excelOrder++;

            //return partInfo;
        }
    }
}
