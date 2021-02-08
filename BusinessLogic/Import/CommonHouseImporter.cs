using ReestrBKS.DataAccess;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ReestrBKS.BusinessLogic.Import
{
    public class CommonHouseImporter : AbstractImporter<CommonHouseLine>
    {
        public CommonHouseImporter(string fileName, Stream fileStream, ReestrContext context) :base(fileName, fileStream, context)
        {
        }

        protected override int startRowIndex => 5;
        protected override int cellsCount => 21;

        protected override CommonHouseLine GetAbstractLine(int year, int month, List<string> cellsText)
        {
            string accountNumber = cellsText[2];
            string address = cellsText[3].Replace("ком", ".ком");

            string incCharge = cellsText[5] ?? "0";
            string incBalance = cellsText[6] ?? "0";
            string incPenalty = cellsText[7] ?? "0";
            string chargeCorrectBalance = cellsText[8] ?? "0";
            string chargeCorrectPenalty = cellsText[9] ?? "0";
            string chargeMonth = cellsText[10] ?? "0";
            string chargeBalance = cellsText[11] ?? "0";
            string chargePenalty = cellsText[12] ?? "0";
            string paymentTotal = cellsText[13] ?? "0";
            string paymentBalance = cellsText[14] ?? "0";
            string paymentPenalty = cellsText[15] ?? "0";
            string outBalance = cellsText[16] ?? "0";
            string outPenalty = cellsText[17] ?? "0";

            Subject subject = GetSubject(address, null, 0.0f, 0.0f);
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                CommonHouseLine abstractLine = new CommonHouseLine()
                {
                    Year = year,
                    Month = month,
                    Subject = subject,
                    AccountNumber = accountNumber,
                    IncCharge = Convert.ToDouble(incCharge, provider),
                    IncBalance = Convert.ToDouble(incBalance, provider),
                    IncPenalty = Convert.ToDouble(incPenalty, provider),
                    ChargeCorrectBalance = Convert.ToDouble(chargeCorrectBalance, provider),
                    ChargeCorrectPenalty = Convert.ToDouble(chargeCorrectPenalty, provider),
                    ChargeMonth = Convert.ToDouble(chargeMonth, provider),
                    ChargeBalance = Convert.ToDouble(chargeBalance, provider),
                    ChargePenalty = Convert.ToDouble(chargePenalty, provider),
                    PaymentTotal = Convert.ToDouble(paymentTotal, provider),
                    PaymentBalance = Convert.ToDouble(paymentBalance, provider),
                    PaymentPenalty = Convert.ToDouble(paymentPenalty, provider),
                    OutBalance = Convert.ToDouble(outBalance, provider),
                    OutPenalty = Convert.ToDouble(outPenalty, provider)
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
