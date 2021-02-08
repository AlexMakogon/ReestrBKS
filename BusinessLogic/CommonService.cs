using System;
using System.Collections.Generic;
using System.Text;

namespace ReestrBKS.BusinessLogic
{
    public class CommonService
    {
        private static string[] monthes = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

        public static string GetMonthName(int monthNumber)
        {
            return monthes[monthNumber - 1];
        }
    }
}
