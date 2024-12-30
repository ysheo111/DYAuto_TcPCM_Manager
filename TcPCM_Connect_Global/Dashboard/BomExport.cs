using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Data;
using Newtonsoft.Json;
using System.Reflection;

namespace TcPCM_Connect_Global
{
    public class BomExport
    {
        //public Dictionary<string, string> bom = new Dictionary<string, string>();

        //Dictionary<string, Dictionary<string, Part>> parts = new Dictionary<string, Dictionary<string, Part>>();
        //Dictionary<string, Part> part = new Dictionary<string, Part>();
        //string  errorMsg = "", id;

        //enum partClas {
        //    root,
        //    part,
        //    material
        //}

        //public JArray SearchBomID(int flag, string searchName)
        //{
        //    string query = "";
        //    if (flag == (int)Bom.SearchMode.Part)
        //    {
        //        query = $@"select *
        //                from (select * from parts where Name_LOC_Extracted like '%{searchName}%') as a
        //                left join calculations as b
        //                on a.Id = b.PartId
        //                where (select count(CurrentCalcId) from CalcBomParents(b.Id) where CurrentCalcId = b.Id and ParentCalcId is null) != 0;";
        //    }
        //    else if (flag == (int)Bom.SearchMode.Project)
        //    {
        //        query = $@"select STRING_AGG(Id, ', ') from Projects where Deleted is null and Name_LOC_Extracted like '%{searchName}%'";
        //    }
        //    else if (flag == (int)Bom.SearchMode.Folder)
        //    {
        //        query = $@"Select STRING_AGG(b.Id, ', ') From  Folders as a 
        //                  join projects as b
        //                  on a.Id = b.FolderId 
        //                  where a.Deleted is null and CAST(a.Name_LOC as nvarchar(max)) LIKE '%{searchName}%'";
        //    }

        //    string result = global_DB.ScalarExecute(query, (int)global_DB.connDB.PCMDB);

        //    return JArray.Parse("[" + result + "]");
        //}

        //public Dictionary<string, Dictionary<string, Part>> SimpleDataSort(string apiResult)
        //{
        //    JObject r = JObject.Parse(apiResult);
        //    if (!r.ContainsKey("data"))
        //    {
        //        errorMsg = "데이터가 조회되지 않았습니다. 다시 조회해주세요. 같은 문제 반복 발생 시 관리자에게 문의하세요";
        //        parts.Add("Error", new Dictionary<string, Part> { { errorMsg, new Part() } });
        //        return parts;
        //    }

        //    try
        //    {
        //        foreach (var partItem in r["data"])
        //        {
        //            Dictionary<string, object> values = partItem.ToObject<Dictionary<string, object>>();
        //            string lineType = values[Report.LineType.lineType]?.ToString();
        //            string procument = values[Report.Designation.procument] == null ? "" : values[Report.Designation.procument]?.ToString();
        //            //string procument = values[Report.Designation.procument]?.ToString();
        //            //string partName = values[Report.Designation.basic]?.ToString();

        //            //Part의 기본정도에 관한 내용
        //            if (values[Report.LineType.level].ToString() == "0") SimpleBasicInfo(values, (int)partClas.root);
        //            else if (values[Report.LineType.source]?.ToString().Contains("Calculation") == true) SimpleBasicInfo(values, (int)partClas.part);
        //            //소재비일 경우
        //            else if (procument.Contains(Report.LineType.dross)) SimpleDross(values);
        //            else if (lineType.ToLower().Contains(Report.LineType.material)) SimpleMaterial(values);
        //            else if (lineType == Report.LineType.externalManufacturingStep) ExternalManufacturingStep(values);
        //            //Part의 기본정도에 관한 내용
        //            //else if (lineType.ToLower().Contains("part")) SimpleBasicInfo(values, (int)partClas.part);
        //            //else if (lineType == Report.LineType.controllingManufacturingStep) ControllingManufacturingStep(values);
        //            else if (lineType == Report.LineType.tool) Tooling(values);
        //            //Detailed ManufacturingStep
        //            else if (lineType == Report.LineType.detailedManufacturingStep) DetailedManufacturingStep(values);                    
        //            ////임율에 관한 내용
        //            else if (lineType == Report.LineType.machine)
        //            {
        //                string calcId = global_DB.ScalarExecute($"select CalculationId from ManufacturingSteps where id = (select ManufacturingStepId from Machines where id = {values[Report.Manufacturing.machineId]})", (int)global_DB.connDB.PCMDB);
        //                ManufacturingComponent(values, calcId);

        //                 Part.Manufacturing manufacturing = part[calcId].manufacturing[part[calcId].manufacturing.Count - 1];

        //                part[calcId].manufacturing[part[calcId].manufacturing.Count - 1].redirectExpenseRatio = (part[calcId].manufacturing[part[calcId].manufacturing.Count - 1].directExpenseRatio) * manufacturing.ratioOfIndirectlyMachineryCost/100;
        //                part[calcId].manufacturing[part[calcId].manufacturing.Count - 1].machineCostRate = part[calcId].manufacturing[part[calcId].manufacturing.Count - 1].directExpenseRatio + part[calcId].manufacturing[part[calcId].manufacturing.Count - 1].redirectExpenseRatio;

        //                if (manufacturing.manufacturer?.Contains("전용") == true) part[calcId].manufacturing[part[calcId].manufacturing.Count - 1].machineCostRate = manufacturing.machineCostRate - (manufacturing.amotizingCostOfMachine);
        //            }
        //            else if (lineType == Report.LineType.labor)
        //            {
        //                string calcId = global_DB.ScalarExecute($"select CalculationId from ManufacturingSteps where id = (select ManufacturingStepId from Labor where id = {values[Report.Manufacturing.laborId]})", (int)global_DB.connDB.PCMDB);

        //                ManufacturingComponent(values, calcId);

        //                Part.Manufacturing manufacturing = part[calcId].manufacturing[part[calcId].manufacturing.Count - 1];
        //                double div = 3600 * manufacturing.cavity;
        //                double laborCost = manufacturing.workingTime * manufacturing.grossWage/ div;
        //                part[calcId].manufacturing[part[calcId].manufacturing.Count - 1].laborCosts = laborCost;
        //                part[calcId].manufacturing[part[calcId].manufacturing.Count - 1].machinaryCost = (manufacturing.machineCostRate* manufacturing.workingTime) / div + laborCost;
        //            }
        //        }

        //        PartAdd();
        //        if (errorMsg.Length > 0) parts.Add("Error", new Dictionary<string, Part>{ { errorMsg, new Part() } }  );
        //    }
        //    catch (Exception e)
        //    {
        //        parts.Add("Error", new Dictionary<string, Part> { { e.Message, new Part() } });
        //    }

        //     return parts;
        //    //if (errorMsg.Length>0) MessageBox.Show($"데이터 정리중 오류가 발생한 개수는 {errorMsg.Count(f => f == '\n')} 입니다.\n {errorMsg}");
        //}

        //private void PartAdd()
        //{
        //    //이전 Part 정보를 Dictionary에 추가
        //    if (part.Count == 0) return;
        //    parts.Add(id, new Dictionary<string, Part>(part) );
        //}

        //private void SimpleBasicInfo(Dictionary<string, object> values, int flag)
        //{
        //    if (flag == (int)partClas.root)
        //    {
        //        PartAdd();

        //        id = values[Report.Designation.calculationID]?.ToString();
        //        this.part.Clear();
        //        PartHeaderAdd(values);
        //    }
        //    else if (flag == (int)partClas.part && values[Report.LineType.view]?.ToString().Contains("Manufacturer view") == true)
        //    {
        //        Part.Material material = new Part.Material();
        //        material.name = values[Report.Header.partName]?.ToString();
        //        material.itemNumber = values[Report.Header.partNumber]?.ToString();
        //        material.qunantityUnit = values[Report.Material.qunantityUnit]?.ToString();
        //        material.priceUnit = values[Report.Material.priceUnit]?.ToString();
        //        material.totalQuantity = global.ConvertDouble(values[Report.Material.quantity]);
        //        material.unitCost = global.ConvertDouble(values[Report.Summary.total]);
        //        material.total = material.totalQuantity * material.unitCost;
        //        string calcId = bom[values[Report.Header.guid].ToString()];
        //        part[calcId].material.Add(material);
        //        PartHeaderAdd(values);

        //    }       
        //}
        //private void SimpleDross(Dictionary<string, object> values)
        //{
        //    if (values[Report.Material.guid] == null || values[Report.LineType.view]?.ToString().Contains("Buyer view") != true) return;

        //    string perent = bom[values[Report.Header.guid].ToString()];

        //    //기존에 같은 파트나 재료가 있었다면 함수 종료            
        //    if (part[perent].material.FindAll(x => x.guid == values[Report.Material.guid].ToString()).Count > 0) return;

        //    try
        //    {
        //        Part.Material prevMaterial = part[perent].material.Last();
        //        double dross = global.ConvertDouble(values[Report.Material.quantity]);
        //        double dross2 = global.ConvertDouble(values[Report.Material.unitCost])+ global.ConvertDouble(values[Report.Material.etcCost]) / global.ConvertDouble(values[Report.Material.quantity]);
        //        double dross3 = dross * dross2;

        //        prevMaterial.dross = $"dross 양 : {dross}, dross 단가 : {dross2}, dross 총액 : {dross3}";
        //        prevMaterial.drossId = global.ConvertDouble(values[Report.Material.drossId]);
        //        prevMaterial.total += dross3;
        //        prevMaterial.drossUnitPrice = dross2;
        //        part[perent].material[part[perent].material.Count - 1] = prevMaterial;

        //    }
        //    catch (Exception e)
        //    {
        //        errorMsg += $"\n{perent} 소재비 추가 중에 오류가 발생했습니다. 해당 재료를 제외하고 조회합니다.";
        //    }
        //}

        //private void SimpleMaterial(Dictionary<string, object> values)
        //{
        //    if (values[Report.Material.guid] == null || values[Report.LineType.view]?.ToString().Contains("Buyer view") != true) return;

        //    string perent = bom[values[Report.Header.guid].ToString()];

        //    //기존에 같은 파트나 재료가 있었다면 함수 종료            
        //    if (part[perent].material.FindAll(x => x.guid == values[Report.Material.guid].ToString()).Count > 0) return;

        //    try
        //    {
        //        //재료명에 scrap이 들어간다면
        //        if (values[Report.Material.name]?.ToString().Contains("Scrap") == true)
        //        {
        //            Part.Material prevMaterial = part[perent].material.Last();
        //            prevMaterial.scrapUnitPrice = global.ConvertDouble(values[Report.Material.unitCost]) + global.ConvertDouble(values[Report.Material.etcCost]) / global.ConvertDouble(values[Report.Material.quantity]);

        //            prevMaterial.scrapId = global.ConvertDouble(values[Report.Material.scrapId]);
        //            prevMaterial.scrap = (-global.ConvertDouble(values[Report.Material.quantity]) * prevMaterial.scrapUnitPrice * (prevMaterial.returnRatio / 100));
        //            prevMaterial.total -= prevMaterial.scrap;

        //            part[perent].material[part[perent].material.Count - 1] = prevMaterial;
        //        }
        //        else
        //        {
        //            Part.Material material = new Part.Material();
        //            MemberInfo[] members = typeof(Report.Material).GetMembers(BindingFlags.Static | BindingFlags.Public);

        //            try
        //            {
        //                foreach (MemberInfo property in members)
        //                {
        //                    string key = ((FieldInfo)property).GetValue(property.Name)?.ToString();
        //                    if (!values.ContainsKey(key)) continue;

        //                    material[property.Name] = values[key];
        //                }
        //                if (material.qunantityUnit.ToLower().Contains("t") || material.qunantityUnit.ToLower().Contains("g"))
        //                {
        //                    if (part[perent].material.Count == 0)
        //                    {
        //                        material.returnRatio = 100 - part[perent].header.recovery;
        //                        material.etc = part[perent].header.etc + material.etcCost;
        //                    }
        //                    else
        //                    {
        //                        material.etc = material.etcCost;
        //                    }

        //                    material.total = material.quantity * material.unitCost + material.etc;

        //                    string query = $@"select comment
        //                             from MaterialMasses
        //                             where Id = (select MaterialMassId from [dbo].[BomEntries] and MaterialMassId is not null
        //                             where PartID = {values[Report.Header.partID]});";

        //                    string comment = global_DB.ScalarExecute(query, (int)global_DB.connDB.PCMDB);
        //                    material.comment = comment;
        //                    material.totalQuantity = 1;
        //                }
        //                else
        //                {                           
        //                    material.total = material.quantity * material.unitCost + material.etc;
        //                    material.totalQuantity = material.quantity;
        //                    material.quantity = 0;
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                errorMsg += $"\n{material.name} 추가 중 오류가 발생했습니다. 해당 프로세스를 제외하고 출력합니다.";
        //            }

                    
        //            part[perent].material.Add(material);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        errorMsg += $"\n{perent} 소재비 추가 중에 오류가 발생했습니다. 해당 재료를 제외하고 조회합니다.";
        //    }
        //}

        //private void ExternalManufacturingStep(Dictionary<string, object> values)
        //{
        //    Part.Material material = new Part.Material();
        //    MemberInfo[] members = typeof(Report.Material).GetMembers(BindingFlags.Static | BindingFlags.Public);
        //    string calcId = part.Values.Last().header["guid"]?.ToString();

        //    try
        //    {
        //        foreach (MemberInfo property in members)
        //        {
        //            string key = ((FieldInfo)property).GetValue(property.Name)?.ToString();
        //            if (!values.ContainsKey(key)) continue;

        //            material[property.Name] = values[key];
        //        }

        //        material.name = values[Report.Manufacturing.manufacturingName]?.ToString();
        //        material.quantity = 0;
        //        material.totalQuantity = global.ConvertDouble( values[Report.Material.externalQuantity]);
        //        material.priceUnit = values[Report.Material.externalQunantityUnit]?.ToString();
        //        material.qunantityUnit = values[Report.Material.externalQunantityUnit]?.ToString();
        //        material.total = global.ConvertDouble(values[Report.Material.externalTotal]);
        //    }
        //    catch (Exception e)
        //    {
        //        errorMsg += $"\n{material.name} 추가 중 오류가 발생했습니다. 해당 프로세스를 제외하고 출력합니다.";
        //    }

        //    part[calcId].material.Add(material);
        //}

        //private void Tooling(Dictionary<string, object> values)
        //{
        //    string calcId = global_DB.ScalarExecute($"select CalculationId from ManufacturingSteps where id = (select ManufacturingStepId from Tools where id = {values[Report.Tooling.id]})", (int)global_DB.connDB.PCMDB);

        //    //string manufacturingID = part.Values.Last().header.guid.ToString();
        //    //Part.Manufacturing manufacturing = part[manufacturingID].manufacturing[part[manufacturingID].manufacturing.Count - 1];

        //    Part.Tooling tool = new Part.Tooling();
        //    MemberInfo[] members = typeof(Report.Tooling).GetMembers(BindingFlags.Static | BindingFlags.Public);
        //    MemberInfo[] membersManufacturing = typeof(Report.Manufacturing).GetMembers(BindingFlags.Static | BindingFlags.Public);

        //    try
        //    {
        //        foreach (MemberInfo property in members)
        //        {
        //            string key = ((FieldInfo)property).GetValue(property.Name)?.ToString();
        //            if (!values.ContainsKey(key)) continue;

        //            tool[property.Name] = values[key];
        //        }
        //        tool.leadtime /= 100;
        //        tool.annualCapa /= 3600;
        //        if( tool.method.Contains("Direct") )
        //        {
        //            tool.method = "In-house";
        //        }
        //        else
        //        {
        //            tool.method = "Outsorcing";
        //        }

        //        //if(tool.type.Contains("ED&D상각비") || tool.type.Contains("Tooling costing"))
        //        //{
        //        //    tool.total = tool.unitCost / tool.annualCapa * tool.quantity;
        //        //}
        //    }
        //    catch (Exception e)
        //    {
        //        errorMsg += $"\n{tool.tooling} 추가 중 오류가 발생했습니다. 해당 프로세스를 제외하고 출력합니다.";
        //    }

        //    part[calcId].tooling.Add(tool);
        //}
        
        //private void DetailedManufacturingStep(Dictionary<string, object> values)
        //{
        //    Part.Manufacturing manufacturing = new Part.Manufacturing();
        //    MemberInfo[] members = typeof(Report.Manufacturing).GetMembers(BindingFlags.Static | BindingFlags.Public);
        //    string calcId = global_DB.ScalarExecute($"select CalculationId from ManufacturingSteps where id = {values[Report.Manufacturing.id]}", (int)global_DB.connDB.PCMDB);

        //    try
        //    {
        //        foreach (MemberInfo property in members)
        //        {
        //            string key = ((FieldInfo)property).GetValue(property.Name)?.ToString();
        //            if (!values.ContainsKey(key)) continue;

        //            manufacturing[property.Name] = values[key];
        //        }
        //        manufacturing.workingTimePerDay = manufacturing.workingTimePerShift / (manufacturing.workingDayPerYear);
        //        //double ratioOfIndirectlyMachineryCost = 0;
        //        //if (double.TryParse(global_DB.ScalarExecute($"SELECT ManualMiscOverheadRate FROM ManufacturingSteps Where id={manufacturing.id}", (int)global_DB.connDB.PCMDB), out ratioOfIndirectlyMachineryCost))
        //        //{
        //        //    manufacturing.ratioOfIndirectlyMachineryCost = ratioOfIndirectlyMachineryCost;
        //        //}
        //        //else manufacturing.ratioOfIndirectlyMachineryCost = 0;

        //            //manufacturing.ratioOfIndirectlyMachineryCost *= 100 ;     

        //        manufacturing.quantity = 1;
        //    }
        //    catch (Exception e)
        //    {
        //        errorMsg += $"\n{manufacturing.manufacturingName} 추가 중 오류가 발생했습니다. 해당 프로세스를 제외하고 출력합니다.";
        //    }

           
        //    part[calcId].manufacturing.Add(manufacturing);
        //    //sequence = true;
        //}

        //private void ManufacturingComponent(Dictionary<string, object> values, string calcId)
        //{
        //    MemberInfo[] members = typeof(Report.Manufacturing).GetMembers(BindingFlags.Static | BindingFlags.Public);

        //    try
        //    {
        //        foreach (MemberInfo property in members)
        //        {
        //            string key = ((FieldInfo)property).GetValue(property.Name)?.ToString();
        //            if (!values.ContainsKey(key)) continue;
        //            string value = part[calcId].manufacturing[part[calcId].manufacturing.Count - 1][property.Name]?.ToString();
        //            bool result = double.TryParse(value, out double dobuleParse) ? (dobuleParse == 0 ? false : true) : (value == null ? false : true);
        //            if (result) continue;

        //            part[calcId].manufacturing[part[calcId].manufacturing.Count - 1][property.Name] = values[key];
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        errorMsg += $"\n{part[calcId].manufacturing[part[calcId].manufacturing.Count - 1].manufacturingName} 추가 중 오류가 발생했습니다. 해당 프로세스를 제외하고 출력합니다.";
        //    }
        //}

        //private void PartHeaderAdd(Dictionary<string, object> values)
        //{
        //    Part.Header header = new Part.Header();
        //    MemberInfo[] members = typeof(Report.Header).GetMembers(BindingFlags.Static | BindingFlags.Public);

        //    try
        //    {
        //        foreach (MemberInfo property in members)
        //        {
        //            string key = ((FieldInfo)property).GetValue(property.Name)?.ToString();
        //            if (!values.ContainsKey(key)) continue;

        //            header[property.Name] = values[key];
        //        }
        //        header.etc = global.ConvertDouble(values[Report.Material.pressLoss]) +
        //            + global.ConvertDouble(values[Report.Material.other]) + global.ConvertDouble(values[Report.Material.trash]);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    Part.Summary summary = new Part.Summary();
        //    members = typeof(Report.Summary).GetMembers(BindingFlags.Static | BindingFlags.Public);

        //    try
        //    {
        //        foreach (MemberInfo property in members)
        //        {
        //            string key = ((FieldInfo)property).GetValue(property.Name)?.ToString();
        //            if (!values.ContainsKey(key)) continue;

        //            if (key == Report.Summary.labor)
        //            {
        //                double labor = global.ConvertDouble(values[key]) + global.ConvertDouble(values[Report.Summary.setupTotal]) - global.ConvertDouble(values[Report.Summary.setupMachine]);
        //                summary[property.Name] = labor;
        //            }
        //            else if (key == Report.Summary.machine)
        //            {
        //                double machine = global.ConvertDouble(values[key]) + global.ConvertDouble(values[Report.Summary.setupMachine]) + global.ConvertDouble(values[Report.Summary.machineOverheads]);
        //                summary[property.Name] = machine;
        //            }
        //            else if (key == Report.Summary.material)
        //            {
        //                double materialExceptionOverheads = global.ConvertDouble(values[key]) + global.ConvertDouble(values[Report.Material.etc]) + global.ConvertDouble(values[Report.Material.returnCost]);
        //                summary[property.Name] = materialExceptionOverheads;
        //            }
        //            else if (key == Report.Summary.etc) continue;
        //            else summary[property.Name] = values[key];
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }


        //    Part part = new Part();
        //    part.header = header;
        //    part.summary = summary;

        //    this.part.Add(values[Report.Header.guid]?.ToString(), part);
        //}
    }
}
