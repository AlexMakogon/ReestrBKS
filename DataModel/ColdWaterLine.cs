using System;
using System.Collections.Generic;
using System.Text;

namespace ReestrBKS.DataModel
{
    public class ColdWaterLine : IAbstractLine
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string AccountNumber { get; set; }
        public int? PersonId { get; set; }
        public Person Person { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int? AmountTypeId { get; set; }
        public AmountType AmountType { get; set; }

        public double IncBalance { get; set; } // Сальдо вх
        public double IncBalanceDebit { get; set; } // Сальдо вх дебет
        public double IncBalanceCredit { get; set; } // Сальдо вх кредит
        public double ColdWater { get; set; } // ХВС
        public double WaterDisposal { get; set; } // Водоотведение
        public double ColdWaterCommon { get; set; } // ХВС(ОДН)
        public double ColdWaterIncrease { get; set; } // Надбавка ХВС
        public double ColdWaterHotIncrease { get; set; } // Надбавка ХВС на ГВС
        public double ColdWaterHot { get; set; } // ХВС на ГВС
        public double ColdWaterHotCommon { get; set; } // ХВС на ГВС (ОДН)
        public double WaterDisposalCommon { get; set; } // Водоотведение (ОДН)
        public double ColdWaterHotIncCoeff { get; set; } // Пов-й к-т нв ХВСнаГВС 10
        public double ColdWaterIncCoeff { get; set; } // Пов-щий коэф нв ХВС 10
        public double HotWater { get; set; } // ГВС
        public double SummerWatering { get; set; } // Летний полив
        public double Heating { get; set; } // Отопление
        public double Total { get; set; } // Итого
        public double Penalty { get; set; } // Пени
        public double OutBalance { get; set; } // Сальдо исх
        public double OutBalanceDebit { get; set; } // Сальдо исх дебет
        public double OutBalanceCredit { get; set; } // Сальдо исх кредит
    }
}
