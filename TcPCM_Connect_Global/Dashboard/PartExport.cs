using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TcPCM_Connect_Global
{
    public class PartExport
    {
        public string ExportPartBom(List<string> nodes, string fileLocation, Bom.ExportLang lang)
        {
            Interface export = new Interface();
            List<string> calcList = export.AllRootCalcId(nodes);
            JObject apiResult = export.LoadCalc(calcList,"Export");
            if (apiResult == null) return "데이터 조회 시 오류가 발생하였습니다.";

            Dictionary<string, Part> partList = PartBomSorting(apiResult);
            if (partList.Count <= 0) return "부품이 존재하지 않습니다.";
            else if (partList.Count == 1 && partList.First().Value == new Part()) return $"{partList.First().Key }";

            PartExcel excel = new PartExcel();
            string err = excel.Export(lang, fileLocation, partList);

            return err;
        }


        // 타입 변환을 위한 헬퍼 메서드
        private object ConvertToType(object value, Type targetType)
        {
            if (value == null || targetType == null) return null;

            try
            {
                // 문자열에서 변환이 필요한 경우
                if (value is string stringValue)
                {
                    if (targetType.IsEnum)
                    {
                        return Enum.Parse(targetType, stringValue);
                    }
                    return Convert.ChangeType(stringValue, targetType);
                }

                // 이미 올바른 타입인 경우
                if (targetType.IsInstanceOfType(value))
                {
                    return value;
                }

                // 기타 경우
                return Convert.ChangeType(value, targetType);
            }
            catch
            {
                // 변환 실패 시 기본값 반환
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }
        }

        private void SetMembers(MemberInfo[] members, Dictionary<string, object> values, Type type, dynamic part)
        {
            foreach (MemberInfo property in members)
            {
                if (type.GetField(property.Name) == null) continue;
                string key = ((FieldInfo)property).GetValue(property.Name)?.ToString();
                string additional = "";
                if (!values.ContainsKey(key))
                {
                    if (values.ContainsKey(key + "[%]")) additional = "[%]";
                    else if (values.ContainsKey(key + "[1]")) additional = "[1]";
                    else if (values.ContainsKey(key + "[/h]")) additional = "[/h]";
                    else if (values.ContainsKey(key + "[m²]")) additional = "[m²]";
                    else if (values.ContainsKey(key + "[Year(s)]")) additional = "[Year(s)]";
                    else if (values.ContainsKey(key + "[mm]")) additional = "[mm]";
                    else if (values.ContainsKey(key + "[/Year(s)]")) additional = "[/Year(s)]";
                    else if (values.ContainsKey(key + "[/(m²*year)]")) additional = "[/(m²*year)]";
                    else if (values.ContainsKey(key + "[h]")) additional = "[h]";
                    else if (values.ContainsKey(key + "[kW]")) additional = "[kW]";
                    else if (values.ContainsKey(key + "[/kWh]")) additional = "[/kWh]";
                    else continue;
                }
                // 타입에 따라 변환
                if((part[property.Name] is double && part[property.Name] != 0) || (part[property.Name] is string && part[property.Name] != null)) continue;
                object convertedValueSummary = ConvertToType(values[key+ additional], type.GetField(property.Name).FieldType);
                part[property.Name] = convertedValueSummary;
            }

        }

        private Dictionary<string, Part> PartBomSorting(JObject apiResult)
        {
            Dictionary<string, Part> partList = new Dictionary<string, Part>();
            Part part = new Part();
            string prevRootPart = "";
            try
            {
                foreach (var element in apiResult["data"])
                {
                    Dictionary<string, object> values = element.ToObject<Dictionary<string, object>>();
                    if (values[Report.LineType.level] == null)
                    {
                        if (prevRootPart.Length > 0) partList.Add(prevRootPart, part);
                        part = new Part();
                        prevRootPart = values[Report.Header.partID]?.ToString();

                        MemberInfo[] headerMembers = typeof(Report.Header).GetMembers(BindingFlags.Static | BindingFlags.Public);
                        MemberInfo[] summaryMembers = typeof(Report.Summary).GetMembers(BindingFlags.Static | BindingFlags.Public);

                        SetMembers(headerMembers, values, typeof(Part.Header), part.header);
                        SetMembers(summaryMembers, values, typeof(Part.Summary), part.summary);
                    }
                    else if (values[Report.LineType.view]?.ToString().Contains("Buyer view") == true) continue;
                    else if (values[Report.LineType.lineType]?.ToString().Contains("Raw material") == true)
                    {
                        if (values[Report.Material.name]?.ToString().ToLower().Contains("scrap") == true)
                        {
                            part.material[part.material.Count - 1].scrapUnitPrice = global.ConvertDouble(values[Report.Material.rawMaterial]);
                            part.material[part.material.Count - 1].netWeight += global.ConvertDouble(values[Report.Material.quantity]);
                        }
                        else
                        {
                            part.material[part.material.Count - 1].unitCost = global.ConvertDouble(values[Report.Material.rawMaterial]);
                            part.material[part.material.Count - 1].netWeight += global.ConvertDouble(values[Report.Material.quantity]);
                            part.material[part.material.Count - 1].grossWeight = global.ConvertDouble(values[Report.Material.quantity]);
                            part.material[part.material.Count - 1].qunantityUnit = values[Report.Material.unit]?.ToString();
                        }
                    }
                    else if (values[Report.LineType.lineType]?.ToString().Contains("Part") == true)
                    {
                        Part.Material material = new Part.Material();
                        //material.name = values[Report.Material.name]?.ToString();
                        //material.itemNumber = values[Report.Material.itemNumber]?.ToString();
                        //material.substance = values[Report.Material.substance]?.ToString();
                        //material.quantity = global.ConvertDouble(values[Report.Material.quantity]);
                        //material.trash = global.ConvertDouble(values[Report.Material.trash]);
                        //material.transport = values[Report.Material.transport]?.ToString();
                        //material.width = global.ConvertDouble(values[Report.Material.width]);
                        //material.length = global.ConvertDouble(values[Report.Material.length]);
                        //material.thickness = global.ConvertDouble(values[Report.Material.thickness]);

                        MemberInfo[] memberInfos = typeof(Report.Material).GetMembers(BindingFlags.Static | BindingFlags.Public);
                        SetMembers(memberInfos, values, typeof(Part.Material), material);
                        if (values[Report.LineType.method]?.ToString().Contains("rough") == true)
                        {
                            material.unitCost = global.ConvertDouble(values[Report.Material.rawMaterial]);
                            material.grossWeight = null;
                            material.netWeight = null;
                        }
                        else material.netWeight = 0;
                        part.material.Add(material);
                    }
                    else if (values[Report.LineType.lineType]?.ToString().Contains("Process manufacturing step") == true)
                    {
                        Part.Manufacturing manufacturing = new Part.Manufacturing();
                        manufacturing.quantity = global.ConvertDouble( values["Quantity, direct input value (Manufacturing Step)"]);
                        manufacturing.price = global.ConvertDouble(values["Price (Manufacturing Step)"]);
                    }
                    else if (values[Report.LineType.lineType]?.ToString().Contains("Detailed manufacturing step") == true)
                    {
                        Part.Manufacturing manufacturing = new Part.Manufacturing();
                        MemberInfo[] manufacturingMember = typeof(Report.Manufacturing).GetMembers(BindingFlags.Static | BindingFlags.Public);
                        SetMembers(manufacturingMember, values, typeof(Part.Manufacturing), manufacturing);
                        manufacturing.quantity = global.ConvertDouble(values["Q'TY 공정[1]"]);
                        //manufacturing.manufacturingName = 
                        part.manufacturing.Add(manufacturing);
                    }
                    else if (values[Report.LineType.lineType]?.ToString().Contains("Machine") == true)
                    {
                        if (part.manufacturing[part.manufacturing.Count - 1].machineCost != 0)
                        {
                            part.manufacturing[part.manufacturing.Count - 1].otherMachineCost = global.ConvertDouble(values[Report.Manufacturing.machineCost]);
                            part.manufacturing[part.manufacturing.Count - 1].otherYearOfMachine = global.ConvertDouble(values[Report.Manufacturing.amotizingYearOfMachine + "[Year(s)]"]);
                        }
                        else
                        {
                            MemberInfo[] manufacturingMember = typeof(Report.Manufacturing).GetMembers(BindingFlags.Static | BindingFlags.Public);
                            SetMembers(manufacturingMember, values, typeof(Part.Manufacturing), part.manufacturing[part.manufacturing.Count - 1]);
                        }
                    }
                    else if (values[Report.LineType.lineType]?.ToString().Contains("Labor") == true)
                    {
                        MemberInfo[] manufacturingMember = typeof(Report.Manufacturing).GetMembers(BindingFlags.Static | BindingFlags.Public);
                        SetMembers(manufacturingMember, values, typeof(Part.Manufacturing), part.manufacturing[part.manufacturing.Count - 1]);
                    }

                }

                if (prevRootPart.Length > 0) partList.Add(prevRootPart, part);
            }
            catch(Exception e)
            {
                return new Dictionary<string, Part>() { { e.Message, new Part() } };
            }

            return partList;
        }


        //String callUrl = $"{global.serverURL}/{global.serverURLPath}/api/{global.version}/Calculations/Export";

        //JObject postData = new JObject();
        //postData.Add("CalculationIds", JArray.FromObject(itemList));
        //postData.Add("ConfigurationGuid", global_iniLoad.GetConfig("CBD", "Export"));
        //var apiResult = WebAPI.POST(callUrl, postData);
    }
}
