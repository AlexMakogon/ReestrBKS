using DocumentFormat.OpenXml.Packaging;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using OpenXmlUtils;
using System.Text;
using System.Linq;

namespace ReestrBKS.BusinessLogic.Report
{
    public class CommonHouseReporter : AbstractReporter<CommonHouseLine>
    {
        public CommonHouseReporter(string userName) : base(userName)
        {

        }

        public override string GetDetailsReport(List<CommonHouseLine> lines, Subject subject, Person person, DateTime startDate, DateTime finishDate)
        {
            throw new NotImplementedException();
        }

        public override string GetGeneralReport(List<CommonHouseLine> lines, Subject subject, Person person, DateTime startDate, DateTime finishDate)
        {
            string fileName = string.Format(@"{0}\{1}.xlsx", tempDirectory, Guid.NewGuid().ToString());
            string templateFile = string.Format(@"{0}\templates\CommonHouseGeneralReport.xlsx", Directory.GetCurrentDirectory());
            File.Copy(templateFile, fileName);
            SpreadsheetDocument xcell = SpreadsheetDocument.Open(fileName, true);
            ExcellUtil ExcellUtil = new ExcellUtil(xcell);

            ExcellUtil.InsertText(subject.GetAddress(), "B3");

            var groups = lines
                .GroupBy(p => new { p.Year, p.Month, p.AccountNumber })
                .OrderBy(p => p.Key.Year)
                .ThenBy(p => p.Key.Month)
                .ThenBy(p => p.Key)
                .Select(p => new { p.Key, Lines = p });

            double sumF = 0;
            double sumG = 0;
            double sumH = 0;
            double sumI = 0;
            double sumJ = 0;
            double sumK = 0;
            double sumL = 0;
            double sumM = 0;

            double lastC = 0;
            double lastD = 0;
            double lastE = 0;
            double lastN = 0;
            double lastO = 0;

            int rowIndex = 11;
            foreach (CommonHouseLine line in lines)
            {
                lastC = line.IncCharge;
                lastD = line.IncBalance;
                lastE = line.IncPenalty;
                sumF += line.ChargeCorrectBalance;
                sumG += line.ChargeCorrectPenalty;
                sumH += line.ChargeMonth;
                sumI += line.ChargeBalance;
                sumJ += line.ChargePenalty;
                sumK += line.PaymentTotal;
                sumL += line.PaymentBalance;
                sumM += line.PaymentPenalty;
                lastN = line.OutBalance;
                lastO = line.OutPenalty;

                ExcellUtil.InsertText(line.AccountNumber, "A" + rowIndex);
                ExcellUtil.InsertText(line.Year + " / " + line.Month, "B" + rowIndex);
                ExcellUtil.InsertNumber(line.IncCharge.ToString("F"), "C" + rowIndex);
                ExcellUtil.InsertNumber(line.IncBalance.ToString("F"), "D" + rowIndex);
                ExcellUtil.InsertNumber(line.IncPenalty.ToString("F"), "E" + rowIndex);
                ExcellUtil.InsertNumber(line.ChargeCorrectBalance.ToString("F"), "F" + rowIndex);
                ExcellUtil.InsertNumber(line.ChargeCorrectPenalty.ToString("F"), "G" + rowIndex);
                ExcellUtil.InsertNumber(line.ChargeMonth.ToString("F"), "H" + rowIndex);
                ExcellUtil.InsertNumber(line.ChargeBalance.ToString("F"), "I" + rowIndex);
                ExcellUtil.InsertNumber(line.ChargePenalty.ToString("F"), "J" + rowIndex);
                ExcellUtil.InsertNumber(line.PaymentTotal.ToString("F"), "K" + rowIndex);
                ExcellUtil.InsertNumber(line.PaymentBalance.ToString("F"), "L" + rowIndex);
                ExcellUtil.InsertNumber(line.PaymentPenalty.ToString("F"), "M" + rowIndex);
                ExcellUtil.InsertNumber(line.OutBalance.ToString("F"), "N" + rowIndex);
                ExcellUtil.InsertNumber(line.OutPenalty.ToString("F"), "O" + rowIndex);

                foreach (char colName in "ABCDEFGHIJKLMNO" )
                    ExcellUtil.SetBorder(colName.ToString() + rowIndex);

                rowIndex += 1;
            }

            ExcellUtil.InsertText("Итого", "B" + rowIndex);
            ExcellUtil.InsertNumber(lastC.ToString("F"), "C" + rowIndex);
            ExcellUtil.InsertNumber(lastD.ToString("F"), "D" + rowIndex);
            ExcellUtil.InsertNumber(lastE.ToString("F"), "E" + rowIndex);
            ExcellUtil.InsertNumber(sumF.ToString("F"), "F" + rowIndex);
            ExcellUtil.InsertNumber(sumG.ToString("F"), "G" + rowIndex);
            ExcellUtil.InsertNumber(sumH.ToString("F"), "H" + rowIndex);
            ExcellUtil.InsertNumber(sumI.ToString("F"), "I" + rowIndex);
            ExcellUtil.InsertNumber(sumJ.ToString("F"), "J" + rowIndex);
            ExcellUtil.InsertNumber(sumK.ToString("F"), "K" + rowIndex);
            ExcellUtil.InsertNumber(sumL.ToString("F"), "L" + rowIndex);
            ExcellUtil.InsertNumber(sumM.ToString("F"), "M" + rowIndex);
            ExcellUtil.InsertNumber(lastN.ToString("F"), "N" + rowIndex);
            ExcellUtil.InsertNumber(lastO.ToString("F"), "O" + rowIndex);

            foreach (char colName in "ABCDEFGHIJKLMNO")
                ExcellUtil.SetBorder(colName.ToString() + rowIndex);

            ExcellUtil.InsertText("Оператор", "A" + (rowIndex + 2));
            ExcellUtil.InsertText(UserName, "F" + (rowIndex + 2));

            xcell.Save();
            xcell.Close();
            return fileName;
        }
    }
}
