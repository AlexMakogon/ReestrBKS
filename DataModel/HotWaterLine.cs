using System;
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

        public double IncBalance { get; set; } //AbstractValueType type1 = new AbstractValueType() { Id = 1, Name = "Сальдо вх" };
        public double IncBalanceDebit { get; set; }//AbstractValueType type2 = new AbstractValueType() { Id = 2, Name = "Сальдо вх дебет" };
        public double IncBalanceCredit { get; set; }//AbstractValueType type3 = new AbstractValueType() { Id = 3, Name = "Сальдо вх кредит" };
        public double Heating { get; set; }//AbstractValueType type4 = new AbstractValueType() { Id = 4, Name = "Отопление" };
        public double HotWater { get; set; }//AbstractValueType type5 = new AbstractValueType() { Id = 5, Name = "ГВС" };
        public double HotWaterTE { get; set; }//AbstractValueType type6 = new AbstractValueType() { Id = 6, Name = "ГВС т\\э" };
        public double HotWaterCommonTE { get; set; }//AbstractValueType type7 = new AbstractValueType() { Id = 7, Name = "ГВС (ОДН) т\\э" };
        public double HotWaterCommonTN { get; set; }//AbstractValueType type8 = new AbstractValueType() { Id = 8, Name = "ГВС (ОДН) т\\н" };
        public double HotWaterIncreaseTN { get; set; }//AbstractValueType type9 = new AbstractValueType() { Id = 9, Name = "Надбавка ГВС т/н" };
        public double ColdWater { get; set; }//AbstractValueType type10 = new AbstractValueType() { Id = 10, Name = "ХВС на ГВС т/э" };
        public double Total { get; set; }//AbstractValueType type11 = new AbstractValueType() { Id = 11, Name = "Итого" };
        public double Penalty { get; set; }//AbstractValueType type12 = new AbstractValueType() { Id = 12, Name = "Пени" };
        public double OutBalance { get; set; }//AbstractValueType type13 = new AbstractValueType() { Id = 13, Name = "Сальдо исх" };
        public double OutBalanceDebit { get; set; }//AbstractValueType type14 = new AbstractValueType() { Id = 14, Name = "Сальдо исх дебет" };
        public double OutBalanceCredit { get; set; }//AbstractValueType type15 = new AbstractValueType() { Id = 15, Name = "Сальдо исх кредит" };
    }
}
