using ReestrBKS.DataAccess;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ReestrBKS.BusinessLogic.Import
{
    public class HotWaterImporter : AbstractImporter<HotWaterLine>
    {
        public HotWaterImporter(string fileName, Stream fileStream, ReestrContext context) :base(fileName, fileStream, context)
        {

        }

        protected override int startRowIndex => 2;
        protected override int cellsCount => 25;

        protected override HotWaterLine GetAbstractLine(int year, int month, List<string> cellsText)
        {
            string accountNumber = cellsText[2];
            string personName = cellsText[3];
            string address = cellsText[4].Replace("ком", ".ком");
            string subjectType = cellsText[5];
            string amountType = cellsText[6];
            string totalArea = cellsText[7] ?? "0";
            string livingArea = cellsText[8] ?? "0";

            string incBalance = cellsText[10] ?? "0";
            string incBalanceDebit = cellsText[11] ?? "0";
            string incBalanceCredit = cellsText[12] ?? "0";
            string heating = cellsText[13] ?? "0";
            string hotWater = cellsText[14] ?? "0";
            string hotWaterTE = cellsText[15] ?? "0";
            string hotWaterCommonTE = cellsText[16] ?? "0";
            string hotWaterCommonTN = cellsText[17] ?? "0";
            string hotWaterIncreaseTN = cellsText[18] ?? "0";
            string coldWater = cellsText[19] ?? "0";
            string total = cellsText[20] ?? "0";
            string penalty = cellsText[21] ?? "0";
            string outBalance = cellsText[22] ?? "0";
            string outBalanceDebit = cellsText[23] ?? "0";
            string outBalanceCredit = cellsText[24] ?? "0";

            if (string.IsNullOrEmpty(personName))
                throw new Exception("Не указан наниматель - " + address);

            DataModel.Person person = GetPerson(personName);
            AmountType aType = GetAmountType(amountType);
            Subject subject = GetSubject(address, subjectType, float.Parse(totalArea, CultureInfo.InvariantCulture)
                , float.Parse(livingArea, CultureInfo.InvariantCulture));
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                HotWaterLine abstractLine = new HotWaterLine()
                {
                    Year = year,
                    Month = month,
                    Person = person,
                    AmountType = aType,
                    Subject = subject,
                    AccountNumber = accountNumber,
                    IncBalance = Convert.ToDouble(incBalance, provider),
                    IncBalanceDebit = Convert.ToDouble(incBalanceDebit, provider),
                    IncBalanceCredit = Convert.ToDouble(incBalanceCredit, provider),
                    Heating = Convert.ToDouble(heating, provider),
                    HotWater = Convert.ToDouble(hotWater, provider),
                    HotWaterTE = Convert.ToDouble(hotWaterTE, provider),
                    HotWaterCommonTE = Convert.ToDouble(hotWaterCommonTE, provider),
                    HotWaterCommonTN = Convert.ToDouble(hotWaterCommonTN, provider),
                    HotWaterIncreaseTN = Convert.ToDouble(hotWaterIncreaseTN, provider),
                    ColdWater = Convert.ToDouble(coldWater, provider),
                    Total = Convert.ToDouble(total, provider),
                    Penalty = Convert.ToDouble(penalty, provider),
                    OutBalance = Convert.ToDouble(outBalance, provider),
                    OutBalanceDebit = Convert.ToDouble(outBalanceDebit, provider),
                    OutBalanceCredit = Convert.ToDouble(outBalanceCredit, provider)
                };

                return abstractLine;
            }
            catch(Exception exc)
            {
                string message = exc.Message;
                throw new Exception(message);
            }
        }
    }
}
