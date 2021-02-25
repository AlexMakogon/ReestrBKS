using ReestrBKS.DataAccess;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ReestrBKS.BusinessLogic.Import
{
    public class ColdWaterImporter : AbstractImporter<ColdWaterLine>
    {
        public ColdWaterImporter(string fileName, Stream fileStream, ReestrContext context) : base(fileName, fileStream, context)
        {
        }

        protected override int startRowIndex => 2;
        protected override int cellsCount => 32;

        protected override ColdWaterLine GetAbstractLine(int year, int month, List<string> cellsText)
        {
            string accountNumber = cellsText[2];
            string personName = cellsText[4];
            string address = cellsText[5].Replace("ком", ".ком");
            string subjectType = cellsText[6];
            string amountType = cellsText[7];
            string totalArea = cellsText[8] ?? "0";
            string livingArea = cellsText[9] ?? "0";

            string incBalance = cellsText[11] ?? "0";
            string incBalanceDebit = cellsText[12] ?? "0";
            string incBalanceCredit = cellsText[13] ?? "0";
            string coldWater = cellsText[14] ?? "0";
            string waterDisposal = cellsText[15] ?? "0";
            string coldWaterCommon = cellsText[16] ?? "0";
            string coldWaterIncrease = cellsText[17] ?? "0";
            string coldWaterHotIncrease = cellsText[18] ?? "0";
            string coldWaterHot = cellsText[19] ?? "0";
            string coldWaterHotCommon = cellsText[20] ?? "0";
            string waterDisposalCommon = cellsText[21] ?? "0";
            string coldWaterHotIncCoeff = cellsText[22] ?? "0";
            string coldWaterIncCoeff = cellsText[23] ?? "0";
            string hotWater = cellsText[24] ?? "0";
            string summerWatering = cellsText[25] ?? "0";
            string heating = cellsText[26] ?? "0";
            string total = cellsText[27] ?? "0";
            string penalty = cellsText[28] ?? "0";
            string outBalance = cellsText[29] ?? "0";
            string outBalanceDebit = cellsText[30] ?? "0";
            string outBalanceCredit = cellsText[31] ?? "0";

            if (string.IsNullOrEmpty(personName))
                throw new Exception("Не указан наниматель - " + address);

            DataModel.Person person = GetPerson(personName);
            AmountType aType = GetAmountType(amountType);
            Subject subject = GetSubject(address, subjectType, float.Parse(totalArea, CultureInfo.InvariantCulture)
                , float.Parse(livingArea, CultureInfo.InvariantCulture));
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                ColdWaterLine abstractLine = new ColdWaterLine()
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
                    ColdWater = Convert.ToDouble(coldWater, provider),
                    WaterDisposal = Convert.ToDouble(waterDisposal, provider),
                    ColdWaterCommon = Convert.ToDouble(coldWaterCommon, provider),
                    ColdWaterIncrease = Convert.ToDouble(coldWaterIncrease, provider),
                    ColdWaterHotIncrease = Convert.ToDouble(coldWaterHotIncrease, provider),
                    ColdWaterHot = Convert.ToDouble(coldWaterHot, provider),
                    ColdWaterHotCommon = Convert.ToDouble(coldWaterHotCommon, provider),
                    WaterDisposalCommon = Convert.ToDouble(waterDisposalCommon, provider),
                    ColdWaterHotIncCoeff = Convert.ToDouble(coldWaterHotIncCoeff, provider),
                    ColdWaterIncCoeff = Convert.ToDouble(coldWaterIncCoeff, provider),
                    HotWater = Convert.ToDouble(hotWater, provider),
                    SummerWatering = Convert.ToDouble(summerWatering, provider),
                    Heating = Convert.ToDouble(heating, provider),
                    Total = Convert.ToDouble(total, provider),
                    Penalty = Convert.ToDouble(penalty, provider),
                    OutBalance = Convert.ToDouble(outBalance, provider),
                    OutBalanceDebit = Convert.ToDouble(outBalanceDebit, provider),
                    OutBalanceCredit = Convert.ToDouble(outBalanceCredit, provider)
                };

                return abstractLine;
            }
            catch (Exception exc)
            {
                string message = exc.Message;
                throw new Exception(message);
            }
        }
    }
}
