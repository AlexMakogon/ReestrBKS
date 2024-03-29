﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ReestrBKS.DataModel
{
    public class HotWaterLine : IAbstractLine
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
        public double Heating { get; set; } // Отопление
        public double HotWater { get; set; } // ГВС
        public double HotWaterTE { get; set; } // ГВС т\\э
        public double HotWaterCommonTE { get; set; } // ГВС (ОДН) т\\э
        public double HotWaterCommonTN { get; set; } // ГВС (ОДН) т\\н
        public double HotWaterIncreaseTN { get; set; } // Надбавка ГВС т/н
        public double ColdWater { get; set; } // ХВС на ГВС т/э
        public double Total { get; set; } // Итого
        public double Penalty { get; set; } // Пени
        public double OutBalance { get; set; } // Сальдо исх
        public double OutBalanceDebit { get; set; } // Сальдо исх дебет
        public double OutBalanceCredit { get; set; } // Сальдо исх кредит
    }
}
