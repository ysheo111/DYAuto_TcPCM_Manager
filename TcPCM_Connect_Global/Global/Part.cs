using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TcPCM_Connect_Global
{
    public class Part
    {
        public Header header { get; set; }
        public Summary summary { get; set; }
        public List<Material> material { get; set; }
        public List<Manufacturing> manufacturing { get; set; }
        public List<Tooling> tooling { get; set; }

        public Part()
        {
            header = new Header();
            summary = new Summary();
            material = new List<Material>() ;
            manufacturing = new List<Manufacturing>() ;
            tooling = new List<Tooling>() ;
        }

        public class Header
        {
            public object this[string propertyName]
            {
                get
                {
                    // probably faster without reflection:
                    // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                    // instead of the following
                    Type myType = typeof(Header);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    return myPropInfo.GetValue(this);
                }
                set
                {
                    Type myType = typeof(Header);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    myPropInfo.SetValue(this, value);
                }
            }

            public string modelName;
            public string partName;
            public string partNumber;

            public string company;
            public string customer;
            public string currency;           
            public string iso;
            public string transport;

            public string category;
            public string suppier;
            public double exchangeRate;
            public string exchangeRateCurrency;

            public string author;
            public DateTime dateOfCalculation;
        }

        public class Summary
        {
            public object this[string propertyName]
            {
                get
                {
                    // probably faster without reflection:
                    // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                    // instead of the following
                    Type myType = typeof(Summary);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    return myPropInfo.GetValue(this);
                }
                set
                {
                    Type myType = typeof(Summary);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    myPropInfo.SetValue(this, value);
                }
            }

            public double administrationCosts;
            public double profit;
            public double materialOverhead;
            public double rnd;
            public double packageTransport;
            public double etc;
        }

        public class Material
        {
            public object this[string propertyName]
            {
                get
                {
                    // probably faster without reflection:
                    // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                    // instead of the following
                    Type myType = typeof(Material);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    return myPropInfo.GetValue(this);
                }
                set
                {
                    Type myType = typeof(Material);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    if (myPropInfo == null) return;
                    myPropInfo.SetValue(this, value);
                }
            }

            public string name;
            public string itemNumber;
            public string transport;
            public string substance;
            public string qunantityUnit;//Material Price 
            public string scrapQunantityUnit;//스크랩
            public string dross;//스크랩
            public double unitCost;
            public double netWeight;
            public double grossWeight;//Material Price 
            public double quantity;//전체 수량
            public double scrap;//스크랩
            public double etc;//기타비
            public double etcCost;//기타비
            public double total;
            public double totalQuantity;
            public string comment; //Material Price = Quantity (Blanks / assembly part)*Material costs (MtC)
                                   //public string modelName = "Material costs (MtC, Scrap)"; //스크랩 = Quantity (Blanks / assembly part)*Material costs (MtC)
            public string priceUnit;//Material Price 
            public string priceComment;
            public string supplier;
            public string qunantityComment;
            public double returnRatio;
            public double returnCost;
            public double scrapUnitPrice;
            public double drossUnitPrice;
            public double loss;
            public double materialId;
            public double scrapId;
            public double drossId;
            public string etcComment;
            public string guid;

        }

        public class Manufacturing
        {
            public object this[string propertyName]
            {
                get
                {
                    // probably faster without reflection:
                    // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                    // instead of the following
                    Type myType = typeof(Manufacturing);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    return myPropInfo.GetValue(this);
                }
                set
                {
                    Type myType = typeof(Manufacturing);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    myPropInfo.SetValue(this, value);
                }
            }

            public string partName;
            public double id;
            public string itemNumber;
            public string sequence;
            public string manufacturingName;
            public string machineName;
            public double cavity;
            public double workingTime;
            public double workers;
            public double quantity;
            public double grossWage;
            public double laborCosts;
            public double laborSetupCosts;
            public double machineCostRate;
            public double machineSetupCostRate;
            public double machinaryCost;
            public string remark;
            public double machineId;
            public double laborId;
            public double total;

            public double lotQty;
            public double oee;
            public double et;
            public double prepare;
            public string prepareUnit;
            public double netCycleTime;
            public string netCycleTimeUnit;
            public double standardCycleTime;
            public double machineCost;
            public double amotizingYearOfMachine;
            public double machineArea;
            public double rationForSupplementaryMachine;
            public double factoryBuildingCost;
            public double amotizingYearOfFactory;
            public double workingDayPerYear;
            public double workingTimePerShift;
            public double workingTimePerDay;
            public double machinePower;
            public double machinePowerEfficiency;
            public double ratioOfMachineRepair;
            public double ratioOfIndirectlyMachineryCost;
            public double amotizingCostOfMachine;
            public double amotizingCostOfFactory;
            public double amotizingCostOfPower;
            public double machinePowerCost;
            public double machineRepairCost;
            public double directExpenseRatio;
            public double redirectExpenseRatio;
            public string techology;
            public double usage;
            public string manufacturer;
        }

        public class Tooling
        {
            public object this[string propertyName]
            {
                get
                {
                    // probably faster without reflection:
                    // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                    // instead of the following
                    Type myType = typeof(Tooling);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    return myPropInfo.GetValue(this);
                }
                set
                {
                    Type myType = typeof(Tooling);
                    FieldInfo myPropInfo = myType.GetField(propertyName);
                    myPropInfo.SetValue(this, value);
                }
            }

            public string partName;
            public double id;
            public string manufacturingName;
            public string itemNumber;
            public string tooling;
            public string currency;
            public double unitCost;
            public double cavity;
            public double lifetime;
            public string type;
            public double total;
            public double quantity;

            public string method;
            public double leadtime;
            public double annualCapa;

        }
    }
}
