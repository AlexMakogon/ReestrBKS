using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ReestrBKS.DataModel
{
    public class CommonHouseLine : IAbstractLine
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

        public double IncCharge { get; set; } //AbstractValueType type16 = new AbstractValueType() { Id = 16, Name = "На начало -> Начислено" };
        public double IncBalance { get; set; } //AbstractValueType type17 = new AbstractValueType() { Id = 17, Name = "На начало -> Остаток долга" };
        public double IncPenalty { get; set; } //AbstractValueType type18 = new AbstractValueType() { Id = 18, Name = "На начало -> Остаток процента" };
        public double ChargeCorrectBalance { get; set; } //AbstractValueType type19 = new AbstractValueType() { Id = 19, Name = "Начисления -> Корректировка -> Основной долг" };
        public double ChargeCorrectPenalty { get; set; }//AbstractValueType type20 = new AbstractValueType() { Id = 20, Name = "Начисления -> Корректировка -> Проценты" };
        public double ChargeMonth { get; set; } //AbstractValueType type21 = new AbstractValueType() { Id = 21, Name = "Начисления -> Ежемесячный аннуитетный платеж" };
        public double ChargeBalance { get; set; } //AbstractValueType type22 = new AbstractValueType() { Id = 22, Name = "Начисления -> Основной долг" };
        public double ChargePenalty { get; set; } //AbstractValueType type23 = new AbstractValueType() { Id = 23, Name = "Начисления -> Процентов" };
        public double PaymentTotal { get; set; } //AbstractValueType type24 = new AbstractValueType() { Id = 24, Name = "Оплата -> Итого оплачено" };
        public double PaymentBalance { get; set; } //AbstractValueType type25 = new AbstractValueType() { Id = 25, Name = "Оплата -> Оплата долга" };
        public double PaymentPenalty { get; set; } //AbstractValueType type26 = new AbstractValueType() { Id = 26, Name = "Оплата -> Оплата процентов" };
        public double OutBalance { get; set; } //AbstractValueType type27 = new AbstractValueType() { Id = 27, Name = "На конец -> Остаток основного долга" };
        public double OutPenalty { get; set; } //AbstractValueType type28 = new AbstractValueType() { Id = 28, Name = "На конец -> Остаток долга по процентам" };
    }
}
