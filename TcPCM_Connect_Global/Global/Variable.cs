using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcPCM_Connect_Global
{

    public class Bom
    {
        public enum SearchMode
        {
            Folder,
            Project,
            Part
        }

        public enum ExportMode
        {
            Simple,
            Detail
        }
        public enum ExportLang
        {
            Kor,
            Eng
        }
        public enum ManufacturingType
        {
            사출 = 0,
            주조 = 1,
            프레스 = 2,
            코어 = 3,
            일반,
            가공
        }
    }
    public class Variable
    {
        public enum Lang
        {
            eng = 0,
            kor = 1
        }


        public enum ChartMode
        {
            basic = 0,
            line = 1,
            waterfall = 2,            
        }
    }
    public class CBD
    {
        public Dictionary<(int, int), string> column = new Dictionary<(int, int), string>
        {
         {(2, 2), Report.Header.modelName},
        {(3, 2), Report.Header.partNumber},
        {(4, 2), Report.Header.partName},

        {(5, 2), Report.Header.company},
        {(6, 2), Report.Header.customer},
        {(7, 2), Report.Header.currency},
        {(8, 2), Report.Header.transport},

        {(5, 5), Report.Header.category},
        {(6, 5), Report.Header.suppier},
        {(7, 5), Report.Header.exchangeRate},

        {(2, 18), Report.Header.dateOfCalc},
        {(3, 18), Report.Header.author},

        {(11, 8), Report.Summary.administrationCosts},
        {(11, 9), Report.Summary.profit},
        {(11, 10), Report.Summary.materialOverhead},
        {(11, 11), Report.Summary.rnd},
        {(11, 12), Report.Summary.packageTransport},
        {(11, 13), Report.Summary.etc},

        {(22, 3), Report.Material.name},
        {(22, 5),  Report.Material.itemNumber },
        {(22, 6),  Report.Material.transport },
        {(22, 7),  Report.Material.substance },

        {(22, 8),  Report.Material.thinckness },
        {(22, 9),  Report.Material.length },
        {(22, 10), Report.Material.width },
        {(22, 11), Report.Material.netWeight },
        {(22, 12), Report.Material.grossWeight },
        {(22, 13), Report.Material.unit },

        {(22, 14), Report.Material.rawMaterial },
        {(22, 15), Report.Material.quantity },
        {(22, 17), Report.Material.scrap },
        {(22, 19), Report.Material.trash },


        {(29, 2), "구성부품명"},
        {(29, 5), "구성부품번호"},
        {(29, 6), "공정명"},
        {(29, 7), "설비명"},
        {(29, 8), "Cavity수"},
        {(29, 9), "작업자수"},
        {(29, 10), "작업시간"},
        {(29, 11), "수량"},
        {(29, 12), "임률"},
        {(29, 13), "노무비"},
        {(29, 14), "경비율"},
        {(29, 15), "제조경비"},
        {(29, 16), "비고"},
        {(28, 15), "Efficiency"},
        {(27, 18), "생산수량"},
        {(27, 19), "생산기간"},
        {(27, 20), "총생산수량"},
        {(29, 18), "Lot수량"},
        {(29, 19), "가동효율"},
        {(29, 20), "준비시간"},
        {(29, 21), "정미작업시간"},
        {(29, 22), "표준작업시간"},
        {(29, 23), "기계가액"},
        {(29, 24), "기계상각연수"},
        {(29, 25), "기계투영면적"},
        {(29, 26), "부대설비면적비"},
        {(29, 27), "건축단가"},
        {(29, 28), "건물상각연수"},
        {(29, 29), "연간작업일수"},
        {(29, 30), "일일작업시간"},
        {(29, 31), "기계전력용량"},
        {(29, 32), "전력소비율"},
        {(29, 33), "전력단가"},
        {(29, 34), "수선비율"},
        {(29, 35), "간접경비율"},
        {(29, 36), "기계상각비"},
        {(29, 37), "건물상각비"},
        {(29, 38), "전력비"},
        {(29, 39), "수선비"},
        {(29, 40), "직접경비율"},
        {(29, 41), "간접경비율"},
        {(29, 42), "경비율"},
        {(44, 2), "분류"},
        {(44, 4), "유형"},
        {(44, 5), "상세"},
        {(44, 6), "비용지급방식"},
        {(44, 7), "Cavity"},
        {(44, 8), "수명"},
        {(44, 9), "제작방식"},
        {(44, 10), "제조사/공급사"},
        {(44, 11), "LeadTime"},
        {(44, 12), "연간Capa."},
        {(44, 13), "단가"},
        {(44, 14), "수량"},
        {(44, 15), "총비용"},
        {(44, 16), "Remark"}
        };

        public string FindValue(int row, int col)
        {
            if (column.TryGetValue((row, col), out string value))
            {
                return value;
            }
            return null; // or throw an exception if not found
        }

        public string CleanString(string input)
        {
            return input?.Replace("\n", "").Replace("\r", "").Replace(" ", "");
        }
    }

    public class Report
    {
        public static class LineType
        {
            public static string lineType = "Line type (Additional information)";
            public static string source = "Cost source (Calculation)";
            public static string level = "Assembly hierarchy level";
            public static string material = "part";
            public static string dross = "Siemens.TCPCM.ProcurementType.InternalSale";
            public static string materialExternal = "outsourcing";
            public static string detailedManufacturingStep = "Manufacturing step (Detailed manufacturing step)";
            public static string controllingManufacturingStep = "Manufacturing step (Controlling manufacturing step)";
            public static string externalManufacturingStep = "Manufacturing step (External manufacturing step)";
            public static string machine = "Machine";
            public static string tool = "Tool or device";
            public static string labor = "Labor";
            public static string view = "Tier detailed view (Additional information BOM export)";
            public static string mode = "Mode";
            public static string materialMode = "Calculator template (Calculation)";
        }

        public static class Header
        {
            public static string modelName = "차종";
            public static string partNumber = "품번";
            public static string partName = "품명";

            public static string company = "업체명";
            public static string customer = "납품국가";
            public static string currency = "화폐";
            public static string transport = "물류조건";

            public static string category = "업종";           
            public static string suppier = "제조국가";
            public static string exchangeRate = "업체적용환율";

            public static string author = "작성자";          
            public static string dateOfCalc = "작성일";     
            
            public static string guid = "작성일";          
            public static string partID = "작성일";          
        }

        public static class Summary
        {
            public static string variant = "물량 Variant, Total";//기타비
            public static string material = "Direct material costs";
            public static string labor = "Direct labor";
            public static string setupTotal = "Set-up costs";
            public static string machine = "Machine costs";
            public static string setupMachine = "Set-up costs machine";
            public static string machineOverheads = "Residual manufacturing overhead costs";
            public static string administrationCostsTotal = "Sales and general administration costs, Total";
            public static string profitTotal = "Profit, Total";
            public static string materialOverheadTotal = "Material overhead costs, Total";
            public static string packageTotal = "포장비, Total";
            public static string transportTotal = "Transport costs, Total";
            public static string financialTotal = "금융비, Total";
            public static string moldTotal = "Tools / devices allocation depreciation and interest";
            public static string total = "Net sales price";

            public static string administrationCosts = "일반관리비";
            public static string profit = "이윤";
            public static string materialOverhead = "재료관리비";
            public static string rnd = "R&D비";
            public static string packageTransport = "포장&운반비";
            public static string etc = "기타";
        }

        public static class Material
        {
            public static string name = "구성부품명";
            public static string itemNumber = "DYA품번";
            public static string transport = "공급기준";
            public static string substance = "재질";

            public static string thinckness = "두께";
            public static string length = "가로";
            public static string width = "세로";
            public static string netWeight = "Net 중량";
            public static string grossWeight = "투입중량";
            public static string unit = "단위";

            public static string rawMaterial = "원재료단가";
            public static string quantity = "Q'TY";
            public static string scrap = "SCRAP단가";
            public static string trash = "폐기물처리비";

            public static string qunantityUnit = "Quantity unit (Blanks / assembly part)";//Material Price 
            public static string scrapQunantityUnit = "Quantity unit (Scrap)";//스크랩
            public static string unitCost = "Material costs (MtC)";
            public static string externalQuantity = "Quantity (Manufacturing step)";//external Manufacturing
            public static string externalQunantityUnit = "Quantity unit (Manufacturing step)";//external Manufacturing
            public static string etc = "etc, Current part";//기타비
            public static string etcCost = "Material scrap costs (Blanks / assembly part)";//기타비
            public static string total = "total";
            public static string totalQuantity = "totalQuantity";
            public static string externalTotal = "Net sales price external manufacturing (Manufacturing step)";
            public static string comment = "comment"; //Material Price = Quantity (Blanks / assembly part)*Material costs (MtC)
            //public static string modelName = "Material costs (MtC, Scrap)"; //스크랩 = Quantity (Blanks / assembly part)*Material costs (MtC)
            public static string priceUnit = "Price unit (Blanks / assembly part)";//Material Price 
            public static string exteranlPriceUnit = "Price unit (Blanks / assembly part)";//Material Price 
            public static string priceComment = "투입 사유 및 단가 산출 내역";
            public static string supplier = "제조사/공급사";
            public static string qunantityComment = "투입 소요량 및 수량 산출 내역";
            public static string returnRatio = "Recovery rate, Overhead rate[%]";            
            public static string scrapUnitCost = "Material costs (Scrap)";
            public static string loss = "Sprue weight5[%]";

            public static string returnCost = "Recovery rate, Current part";
            public static string pressLoss = "Other costs1, Total";
            public static string other = "Other costs2, Total";
            //public static string injectionRecovery = "회수율(%) (Bulk materials calculator)[%]";
            //public static string diecastingRecovery = "Sprue and overflows loss rate (Bulk materials calculator)[%]";
            public static string etcComment = "기타비 산출 내역";
            public static string guid = "Cross-database identifier (GUID) (Calculation)";

            public static string materialId = "Database internal identifier (Part)";
            public static string scrapId = "Database internal identifier (Part)";
            public static string drossId = "Database internal identifier (Part)";
        }

        public static class Manufacturing
        {
            public static string partName = "Designation";
            public static string itemNumber = "Item number (Part)";
            public static string id = "Database internal identifier (Manufacturing step)";
            public static string machineId = "Database internal identifier (Machine)";
            public static string laborId = "Database internal identifier (Labor)";
            public static string sequence = "Sequence Number (Manufacturing step)";
            public static string manufacturingName= "Designation (Manufacturing step)";
            public static string machineName= "Designation (Machine)";
            public static string cavity= "Number of parts per cycle (Manufacturing step)";
            public static string workingTime="working time";
            public static string workers= "Number of workers (Labor)";
            public static string quantity= "Quantity";
            public static string grossWage= "Gross wage (Labor)[/h]";
            public static string laborCosts= "Costs (Labor)";
            public static string laborSetupCosts= "Set-up costs labor (Manufacturing step)";
            public static string machineCostRate= "machineCostRate";
            public static string machineSetupCostRate= "Set-up costs machine (Manufacturing step)";
            public static string machinaryCost = "Manufacturing costs II (Manufacturing step)";
            public static string remark="remark";
            public static string total = "total";

            public static string lotQty= "Usable parts per lot (Manufacturing step)";
            public static string oee= "Time overhead (Manufacturing step)[%]";
            public static string et = "Utilization ratio (ET rate) (Manufacturing step)[%]";
            public static string prepare= "Set-up time (Manufacturing step)";
            public static string prepareUnit= "Set-up time unit (Manufacturing step)";
            public static string netCycleTime= "Cycle time (Manufacturing step)";
            public static string netCycleTimeUnit= "Cycle time unit (Manufacturing step)";
            public static string standardCycleTime= "standardCycleTime";
            public static string machineCost= "Acquisition value (Machine)";
            public static string amotizingYearOfMachine= "Imputed depreciation, direct input value (Machine)[Year(s)]";
            public static string machineArea= "Required gross area including auxiliary areas (Machine)[m²]";
            public static string rationForSupplementaryMachine= "Space cost factor (Machine)[/(m²*month)]";
            //public static string factoryBuildingCost;
            //public static string amotizingYearOfFactory;
            public static string workingDayPerYear= "Production days per year (Manufacturing step)";
            public static string workingTimePerShift= "Production hours per shift factor (Manufacturing step)[h]";
            public static string workingTimePerDay= "Shifts per production day factor (Manufacturing step)";
            public static string machinePower= "Rated power (Machine)[kW]";
            public static string machinePowerEfficiency= "Power utilization (Machine)[%]";
            public static string machinePowerCost = "Energy cost factor (Machine)[/kWh]";
            public static string ratioOfMachineRepair= "Residual manufacturing overhead rate (Manufacturing step)[%]";
            public static string ratioOfIndirectlyMachineryCost= "Other expenditures overheads rate(Manufacturing step)[%]";
            public static string amotizingCostOfMachine= "Imputed depreciation costs rate (Machine)[/h]";
            public static string amotizingCostOfFactory= "Space costs rate (Machine)[/h]";
            public static string amotizingCostOfPower = "Energy costs rate (Machine)[/h]";
            public static string machineRepairCost = "Ratio of Machine Repair (Manufacturing step)[/h]";
            public static string directExpenseRatio= "Machine costs rate (Machine)[/h]";
            public static string redirectExpenseRatio=  "Other expenditures cost (Manufacturing step)";
            public static string techology= "Techology";

            public static string usage = "내역[1]";
            public static string manufacturer = "Manufacturer (Machine)";
        }

        public static class Tooling
        {
            public static string partName = "Designation";
            public static string id = "Database internal identifier (Tool or device)";
            public static string manufacturingName = "Higher level manufacturing step (Tool or device)";
            public static string itemNumber = "Item number (Part)";
            public static string tooling = "Designation (Tool or device)";
            public static string currency = "Calculation currency (Tool or device)";
            public static string unitCost = "금형비 * (Tool or device)";
            public static string cavity = "금형개수 * (Tool or device)";
            public static string quantity = "Number of directly paid tools, direct input (Tool or device)";
            public static string lifetime = "Processed parts (lifetime) (Tool or device)[Pcs]";
            public static string type = "Tool allocation calculator (Tool or device)";
            public static string total = "Allocated costs (Tool or device)";

            public static string method = "Maintenance costs per tool or device, calculation mode (Tool or device)";
            public static string leadtime = "Labor burden (Tool or device)[%]";
            public static string annualCapa = "Gross wage (Tool or device)[/h]";
        }

        public static class Cost
        {
            public static string netSalesPrice = "Net sales price";
            public static string assyShift = "Shifts per production day factor (Calculation)";

            public static string materialCosts = "Material costs (MtC)";
            public static string quantity = "Quantity";
            public static string quantityUnit = "Quantity unit";
            public static string density = "Density (Revision)[g/cm³]";
            public static string length = "Length (Bulk materials calculator)[mm]";
            public static string width = "Width (Bulk materials calculator)[mm]";
            public static string thickness = "Thickness (Bulk materials calculator)[mm]";
            public static string lengthGeometry = "Plate length[mm]";
            public static string widthGeometry = "Width[mm]";
            public static string thicknessGeometry = "Sheet thickness[mm]";
            public static string netWeight = "Net weight (Bulk materials calculator)";
            public static string cavity = "Number of parts (Bulk materials calculator)";
            public static string materialQuantityUnit = "Quantity unit (Blanks / assembly part)";
            public static string materailType = "Materials (Revision)";
            public static string totalMaterial = "Material costs";
            public static string material = "Direct material costs";
            public static string materialPriceUnit = "Price unit (Blanks / assembly part)";
            public static string valid = "valid from";
            public static string materialOverheads = "재료 관리비, Overhead rate[%]";
            public static string externalmaterialOverheads = "외주 재료 관리비, 오버헤드 비율[%]";
            public static string scrapQuantity = "ScrapQuantity";
            public static string scrapQuantityUnit = "ScrapUnit";
            public static string scrapPriceUnit = "ScrapPriceUnit";
            public static string scrapPrice = "ScrapPrice";
            public static string loss = "loss[%]";
            public static string lossPrice = "Material scrap costs (Blanks / assembly part)";
            //public static string lossPlate = "Sprue and overflows loss rate (Bulk materials calculator)_Plate[%]";
            //public static string lossCasting = "Loss (Bulk materials calculator)";
            //public static string recycle = "recycle";
            //public static string recycleInjections = "Recycling rate for sprue (Bulk materials calculator)[%]";
            //public static string recycleCasting = "Sprue and overflows loss rate (Bulk materials calculator)[%]";
            //public static string shot = "Shot weight (Bulk materials calculator)";

            public static string cycleTime = "Cycle time (Manufacturing step)";
            public static string cycleTimeUnit = "Cycle time unit (Manufacturing step)";
            public static string maunCavity = "Number of parts per cycle (Manufacturing step)";
            public static string utilization = "Utilization ratio (Manufacturing step)[%]";
            public static string manufacturing1 = "Manufacturing costs I";
            public static string manufacturing2 = "Manufacturing costs II";
            public static string manufacturing3 = "Manufacturing costs III";
            public static string manufacturingSetUp = "Set-up costs";
            public static string manufacturingLabor = "Direct labor";
            public static string manufacturingCosts = "Manufacturing costs II";
            public static string machineCosts = "Machine costs rate (Machine)[/h]";
            public static string laborCosts = "Gross wage (Labor)[/h]";
            public static string laborNum = "Number of workers (Labor)";
            public static string attendNum = "Attended systems (Labor)";
            public static string totalLabor = "Costs (Labor)";
            public static string marginRate = "Manufacturing lot fraction (Manufacturing step)[%]";
            public static string acquisition = "Acquisition value (Machine)";
            public static string imputed = "Imputed depreciation costs rate (Machine)[/h]";
            public static string space = "Space cost factor (Machine)[/(m²*month)]";
            public static string ratePower = "Rated power (Machine)[kW]";
            public static string utilizationPower = "Power utilization (Machine)[%]";
            public static string energyCostRate = "Energy cost factor (Machine)[/kWh]";
            public static string maintance= "Maintenance costs rate (Machine)[/h]";
            public static string maintanceRate = "Maintenance costs (Machine)[%]";
            public static string setUpLabor = "Set-up costs labor (Machine)";
            public static string setUpMachine = "Set-up costs machine (Machine)";
            public static string setupTime = "Machine downtime production system (Machine)[min]";
            public static string setupTimeUnit = "준비시간단위";
            public static string setupLaborNum = "Number of persons for SetUp (Machine)";
            public static string lot = "Usable parts per lot (Manufacturing step)";
            public static string auxiliaryArea = "Required gross area including auxiliary areas (Machine)[m²]";
            public static string totalMachinePerCycleTime = "Machine costs (Machine)";
            public static string manufacturingCategory = "Sequence Number (Manufacturing step)";
            public static string manufacturingOverheads = "Residual manufacturing overhead rate (Manufacturing step)[%]";
            public static string imputedMachineYear = "Imputed depreciation, direct input value (Machine)[Year(s)]";
            public static string spaceMachine = "Space costs rate (Machine)[/h]";
            public static string energyMachine = "Energy costs rate (Machine)[/h]";
            public static string shifts = "Shifts per production day factor (Manufacturing step)";
            public static string workhour = "Production hours per year (Manufacturing step)[h]";
            public static string moldItemNumber = "구성부품번호";
            public static string moldManufacturingDesignation = "공정명";
            public static string moldCapitalAsset = "설비명";
            public static string moldType = "유형";
            public static string moldContent = "내역";
            public static string moldStatus = "자작/외주";
            public static string moldSupplier = "제조사/공급사";

            public static string mold = "Other overhead costs 02, Total";
            public static string jig  = "Other overhead costs 03, Total";
            public static string package = "포장비, Total";
            public static string transport = "Transport costs";
            public static string overheads = "Sales and general administration costs, Total";
            public static string profit = "Profit, Total";
            public static string overheadsPer = "Sales and general administration costs, Overhead rate[%]";
            public static string profitPer = "Profit, Overhead rate[%]";
        }

        public static class Designation
        {
            public static string car = "차종명";
            public static string productionName = "제품명";
            public static string company = "회사명";
            public static string team = "소속팀";
            public static string owner = "작성자";

            public static string procument = "In-house manufacturing";
            public static string basic = "Designation";
            public static string calculation = "Designation (Calculation)";
            public static string manufacturingStep = "Designation (Manufacturing step)";
            public static string methodology = "Calculation methodology (Calculation)";
            public static string machine = "Designation (Machine)";
            public static string partImg = "Part image (Calculation)";
            public static string substance = "Substance designation (Revision)";
            public static string dateOfCalc = "Date of calculation (Calculation)";
            public static string itemNumber = "Item number (Part)";
            public static string modified = "Modified by (Calculation)";
            public static string status = "Status (Calculation)";
            public static string forcast = "Average annual requirement usable parts (Calculation)";
            public static string productionDayPerYear = "Production days per year (Calculation)";
            public static string productionHourPerYear = "Production hours per year (Calculation)[h]";
            public static string supplier = "Supplier (Calculation)";

            public static string production = "Supplier (Calculation)";
            public static string modifiedDate = "Modified (Calculation)";
            public static string kindOfCar = "Kind of Car";
            public static string numberOfLot = "Number of manufacturing lots (Calculation)[1/year]";

            public static string customer = "Customer (Calculation)";
            public static string shift = "Plant (Manufacturing step)";
            public static string machineCategory = "Manufacturer (Machine)";
            public static string region = "Region (Calculation)";
            public static string productionStart = "Production start (Calculation)";
            public static string lifeTime = "Lifetime (Calculation)[Year(s)]";
            public static string materialGUID = "Cross-database identifier (GUID) (Calculation)";
            public static string calculationID = "Database internal identifier (ID) (Calculation)";
            public static string manufacturingGUID = "Database internal identifier (Manufacturing step)";
            public static string labor = "Database internal identifier (ID) (Calculation)";
            public static string parent = "Parent";
        }


        public static string material = "소재비 계";
        public static string process = "가공비 계";
        public static string etcProcess = "기타 가공비 계";
        public static string afterProcess = "후처리비 계";
        public static string etc = "기타 계";
        public static string managment = "관리비";
        public static string profit = "기업이윤";
        public static string estimatedPrice = "견적가";

        public static  Dictionary<string, double> quantityFactor 
            = new Dictionary<string, double>
                {
                    { "kg" , 1000*1000},
                    { "g" , 1000},
                    { "mg" , 1},
                };
    }

    /// <summary>
    /// report 사용에 필요 변수
    /// </summary>
    public class Summary
    {
        public static class LineType
        {
            public static string level = "Assembly hierarchy level (Assembly hierarchy)";
            public static string lineType = "Line type (Additional information)";
            public static string designation = "Designation (Part)";
        }

        public static class Cost
        {
            public static string rawMaterial = "Raw materials (price)";
            public static string purchasedPart = "Purchased parts";
            public static string purchasedPartInter = "Purchased parts (inter company)";
            public static string externalManufacturing = "External manufacturing steps";
            public static string directMaterial = "Direct material costs";

            public static string materialScrap = "Material scrap";
            public static string materialOverheads = "Material overhead costs";
            public static string interestMaterial = "Interest on material stock";
            public static string materialCosts = "Material costs (Calculation)";
            public static string totalMaterialCosts = "totalMaterialPrice";

            public static string machineCosts = "Machine costs";
            public static string setupCosts = "Set-up costs";
            public static string directLabor = "Direct labor";
            public static string tools = "Tools and devices, maintenance costs";
            public static string processManufacturing = "Process manufacturing step costs";
            public static string Manufacturing1 = "Manufacturing costs I (Calculation)";

            public static string residualManufacturing = "Residual manufacturing overhead costs";
            public static string manufacturingSetup = "Manufacturing and set-up scrap costs";
            public static string manufacturingTools = "Tools / devices allocation depreciation and interest";
            public static string directManufacturing = "Direct manufacturing costs";
            public static string Manufacturing2 = "Manufacturing costs II";

            public static string interestProgress = "Interest on work in progress";
            public static string Manufacturing3 = "Manufacturing costs III (Calculation)";

            public static string productionCost1 = "Production costs I (Calculation)";

            public static string specialDirectCosts = "Special direct costs";
            public static string productionCost2 = "Production costs II (Calculation)";

            public static string overheads = "Overheads after PC";
            public static string primeCosts = "Prime costs (Calculation)";

            public static string profit = "Profit";
            public static string cashSales = "Cash sales price (Calculation)";

            public static string termsOfPayment = "Terms of payment";
            public static string netSalesPriceEx = "Net sales price ex works (Calculation)";

            public static string incoterms = "Incoterms";
            public static string netSalesPrice = "Net sales price (Calculation)";
        }
    }
   
    public class MasterData
    {
        public class Material
        {
            public static List<string> casting = new List<string>() { "재료명", "구분", "비중", "주조 온도 최소(℃)", "주조 온도 최대(℃)", "T-factor" };
            public static List<string> injection = new List<string>() { "재료명", "계열", "구분", "비중", "탈형 온도(℃)", "사출 온도(℃)", "금형 온도(℃)", "내부 탈형 압력 계수", "열확산도(mm²/s)" };
            public static List<string> plate = new List<string>() { "재료명", "구분", "비중", "인장 강도(N/mm²)", "전단 강도(N/mm²)" ,"생산 계수"};
            public static List<string> process = new List<string>() { "재료명", "구분", "비중" };
            public static List<string> material = new List<string>() { "Valid From", "지역", "구분", "재료명",  "비중", "통화", "가격", "가격 단위", "스크랩 비용", "스크랩 비용 단위"};
        }

        public class Machine
        {
            public static string designation = "이름";
            public static string designation1= "설비명";
            public static string designation2 = "구분 2";
            public static string process = "공정";
            public static string technology = "Technology";
            public static string asset = "Asset";
            public static string maxClampingForce = "최대 톤수";
            public static string currency = "통화";
            public static string category = "구분";
            public static string acquisition = "설비가";
            public static string imputed = "기계상각년수";
            public static string space = "설치면적";
            public static string ratedPower = "전력용량";
            public static string poweUtiliation = "전력소비율";
            public static string maintance = "수선비율";
            public static string validFrom = "Valid From";
            public static string setup = "준비시간";
            public static string region = "지역";

            public static string setupTime = "준비시간";
            public static string dryRunningTime = "기본C/T 두께 2 이하";
            public static string meltingPower = "기본C/T 두께 3.5 이하";           
            public static string movePlasticizingUnit = "기본C/T 두께 3.5 초과";

            public static string Foaming = "Foaming";
            public static string Drawing = "Drawing";
            public static string SPM = "SPM";

            public static string retractEjector = "Retract ejector";
            public static string closeSilder = "Close slider";
            public static string closeMold = "Close mold";
            public static string fillMaterial = "Fill material";
            public static string shotTime = "Shot time";
            public static string openMold = "Open mold";
            public static string openSilder = "Open slider";
            public static string injectPartingAgent = "inject parting agent";
            public static string blowOutMold = "Blow out mold";

            public static string removeCast = "Remove cast";
            public static string resetEjector = "Reset ejector";


            public static string blowOutMold2 = "표준시간";
            public static string closeSilder2 = "Insert";
            public static string closeMold2 = "S/C 0EA";
            public static string fillMaterial2 = "S/C 1EA";
            public static string injectPartingAgent2 = "S/C 2~3EA";
            public static string openSilder2 = "S/C 4~10EA";
            public static string openMold2 = "준비시간";
            //public static string removeCast2 = "Remove cast";
            //public static string retractEjector2 = "Retract ejector";
            //public static string resetEjector2 = "Reset ejector";
            //public static string shotTime2 = "Shot time";

        }
    }
}
