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

        public double IncCharge { get; set; } // На начало -> Начислено
        public double IncBalance { get; set; } // На начало -> Остаток долга
        public double IncPenalty { get; set; } // На начало -> Остаток процента
        public double ChargeCorrectBalance { get; set; } // Начисления -> Корректировка -> Основной долг
        public double ChargeCorrectPenalty { get; set; } // Начисления -> Корректировка -> Проценты
        public double ChargeMonth { get; set; } // Начисления -> Ежемесячный аннуитетный платеж
        public double ChargeBalance { get; set; } // Начисления -> Основной долг
        public double ChargePenalty { get; set; } // Начисления -> Процентов
        public double PaymentTotal { get; set; } // Оплата -> Итого оплачено
        public double PaymentBalance { get; set; } // Оплата -> Оплата долга
        public double PaymentPenalty { get; set; } // Оплата -> Оплата процентов
        public double OutBalance { get; set; } // На конец -> Остаток основного долга
        public double OutPenalty { get; set; } // На конец -> Остаток долга по процентам
    }
}
