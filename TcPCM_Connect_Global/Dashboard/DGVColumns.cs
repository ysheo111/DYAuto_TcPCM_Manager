using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcPCM_Connect_Global
{
   public class DGVColumns
    {
        public Dictionary<string, string> Manufacturing()
        {
            Dictionary<string, string> manufacturing = new Dictionary<string, string>();

            manufacturing.Add("No", "No");
            manufacturing.Add(Report.Designation.manufacturingStep, "공정명");
            manufacturing.Add(Report.Cost.workhour, "연간생산시간");
            manufacturing.Add(Report.Cost.cycleTime, "CylceTime");
            manufacturing.Add(Report.Cost.cycleTimeUnit, "시간단위");
            manufacturing.Add(Report.Cost.maunCavity, "Number of parts");
            manufacturing.Add(Report.Cost.utilization, "이용률");
            manufacturing.Add(Report.Cost.lot, "Lot수량");

            manufacturing.Add(Report.Designation.machine, "설비명");
            manufacturing.Add(Report.Cost.acquisition, "설비가");
            manufacturing.Add(Report.Cost.imputedMachineYear, "설비내용연수");
            manufacturing.Add(Report.Cost.auxiliaryArea, "설치면적");
            manufacturing.Add(Report.Cost.imputed, "기계상각비");
            manufacturing.Add(Report.Cost.spaceMachine, "공간비용");
            manufacturing.Add(Report.Cost.space, "건물상각비");
            manufacturing.Add(Report.Cost.energyMachine, "전력단가");
            manufacturing.Add(Report.Cost.ratePower, "전력량");
            manufacturing.Add(Report.Cost.utilizationPower, "전력소비율");
            manufacturing.Add(Report.Cost.maintanceRate, "수선비율");
            manufacturing.Add(Report.Cost.energyCostRate, "전력비");
            manufacturing.Add(Report.Cost.maintance, "수선비");
            manufacturing.Add(Report.Cost.machineCosts, "경비 계");
            manufacturing.Add(Report.Cost.totalMachinePerCycleTime, "시간 당 경비 계");

            manufacturing.Add(Report.Cost.laborNum, "투입인원");
            manufacturing.Add(Report.Cost.attendNum, "attended systems");
            manufacturing.Add(Report.Cost.laborCosts, "임율");
            manufacturing.Add(Report.Cost.totalLabor, "시간 당 임률");

            manufacturing.Add(Report.Cost.setupTime, "준비시간");
            manufacturing.Add(Report.Cost.setupTimeUnit, "준비시간단위");
            manufacturing.Add(Report.Cost.setUpMachine, "준비경비");
            manufacturing.Add(Report.Cost.setUpLabor, "준비노무비");

            manufacturing.Add(Report.Cost.manufacturingOverheads, "경비율");
            manufacturing.Add(Report.Cost.manufacturingCosts, "계");

            return manufacturing;
        }

    }
}
