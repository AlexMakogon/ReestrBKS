using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlUtils;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReestrBKS.BusinessLogic.Report
{
    public class HotWaterReporter : AbstractReporter<HotWaterLine>
    {
        public HotWaterReporter(string userName) :base(userName)
        {
            
        }

        public override string GetGeneralReport(List<HotWaterLine> lines, Subject subject, Person person, DateTime startDate, DateTime finishDate)
        {
            string fileName = string.Format(@"{0}\{1}.xlsx", tempDirectory, Guid.NewGuid().ToString());
            string templateFile = string.Format(@"{0}\templates\HotWaterGeneralReport.xlsx", Directory.GetCurrentDirectory());
            File.Copy(templateFile, fileName);
            SpreadsheetDocument xcell = SpreadsheetDocument.Open(fileName, true);
            ExcellUtil ExcellUtil = new ExcellUtil(xcell);

            ExcellUtil.InsertText(subject.GetAddress(), "C5");
            ExcellUtil.InsertText(person.Name, "C7");
            ExcellUtil.InsertText(lines.First().AccountNumber, "C9");

            var groups = lines
                .GroupBy(p => new { p.Year, p.Month })
                .OrderBy(p => p.Key.Year)
                .ThenBy(p => p.Key.Month)
                .Select(p => new { p.Key, Lines = p });

            double sumE = 0;
            double sumF = 0;
            double sumG = 0;
            double sumH = 0;
            double totalBalance = 0;
            double totalPenalty = 0;

            int rowIndex = 14;
            foreach (var group in groups)
            {
                ExcellUtil.InsertText(group.Key.Year + " / " + group.Key.Month, "A" + rowIndex);

                HotWaterLine incBalanceLine = group.Lines.FirstOrDefault(p => p.AmountType.Id == 2);
                if (incBalanceLine != null)
                {
                    ExcellUtil.InsertNumber(incBalanceLine.IncBalance.ToString("F"), "B" + rowIndex);
                    ExcellUtil.InsertNumber(incBalanceLine.Total.ToString("F"), "C" + rowIndex);
                    ExcellUtil.InsertNumber(incBalanceLine.Penalty.ToString("F"), "D" + rowIndex);
                }
                
                HotWaterLine chargeTotalLine = group.Lines.FirstOrDefault(p => p.AmountType.Id == 4);
                if (chargeTotalLine != null)
                {
                    ExcellUtil.InsertNumber(chargeTotalLine.Total.ToString("F"), "E" + rowIndex);
                    ExcellUtil.InsertNumber(chargeTotalLine.Penalty.ToString("F"), "F" + rowIndex);
                    sumE += chargeTotalLine.Total;
                    sumF += chargeTotalLine.Penalty;
                }

                HotWaterLine paidLine = group.Lines.FirstOrDefault(p => p.AmountType.Id == 6);
                if (paidLine != null)
                {
                    ExcellUtil.InsertNumber(paidLine.Total.ToString("F"), "G" + rowIndex);
                    ExcellUtil.InsertNumber(paidLine.Penalty.ToString("F"), "H" + rowIndex);
                    sumG += paidLine.Total;
                    sumH += paidLine.Penalty;
                }

                HotWaterLine outBalanceLine = group.Lines.FirstOrDefault(p => p.AmountType.Id == 7);
                if (outBalanceLine != null)
                {
                    ExcellUtil.InsertNumber(outBalanceLine.Total.ToString("F"), "I" + rowIndex);
                    ExcellUtil.InsertNumber(outBalanceLine.Penalty.ToString("F"), "J" + rowIndex);
                    totalBalance = outBalanceLine.Total;
                    totalPenalty = outBalanceLine.Penalty;
                }

                foreach (char colName in  "ABCDEFGHIJ" )
                {
                    ExcellUtil.SetBorder(colName.ToString() + rowIndex);
                }

                rowIndex += 1;
            }

            ExcellUtil.InsertText("Итого", "A" + rowIndex);
            ExcellUtil.InsertNumber(sumE.ToString("F"), "E" + rowIndex);
            ExcellUtil.InsertNumber(sumF.ToString("F"), "F" + rowIndex);
            ExcellUtil.InsertNumber(sumG.ToString("F"), "G" + rowIndex);
            ExcellUtil.InsertNumber(sumH.ToString("F"), "H" + rowIndex);

            foreach (char colName in "ABCDEFGHIJ")
                ExcellUtil.SetBorder(colName.ToString() + rowIndex);

            ExcellUtil.InsertText(string.Format("Задолженность за период с {0} г. по {1} г.",
                startDate.ToString("dd.MM.yyyy"), finishDate.ToString("dd.MM.yyyy")), "A" + (rowIndex + 2));
            ExcellUtil.InsertNumber(totalBalance.ToString("F"), "F" + (rowIndex + 2));

            ExcellUtil.InsertText("Начислено пени", "A" + (rowIndex + 4));
            ExcellUtil.InsertNumber(totalPenalty.ToString("F"), "F" + (rowIndex + 4));

            ExcellUtil.InsertText("Итого задолженность", "A" + (rowIndex + 6));
            ExcellUtil.InsertNumber((totalBalance + totalPenalty).ToString("F"), "F" + (rowIndex + 6));

            ExcellUtil.InsertText("Оператор", "A" + (rowIndex + 9));
            ExcellUtil.InsertText(UserName, "F" + (rowIndex + 9));

            xcell.Save();
            xcell.Close();
            return fileName;
        }

        public override string GetDetailsReport(List<HotWaterLine> lines, Subject subject, Person person, DateTime startDate, DateTime finishDate)
        {
            string fileName = string.Format(@"{0}\{1}.xlsx", tempDirectory, Guid.NewGuid().ToString());
            string templateFile = string.Format(@"{0}\templates\HotWaterDetailsReport.xlsx", Directory.GetCurrentDirectory());
            File.Copy(templateFile, fileName);
            SpreadsheetDocument xcell = SpreadsheetDocument.Open(fileName, true);
            ExcellUtil ExcellUtil = new ExcellUtil(xcell);

            ExcellUtil.InsertText(subject.GetAddress(), "C5");
            ExcellUtil.InsertText(person.Name, "C7");
            ExcellUtil.InsertText(lines.First().AccountNumber, "C9");

            var groups = lines
                .GroupBy(p => new { p.Year, p.Month })
                .OrderBy(p => p.Key.Year)
                .ThenBy(p => p.Key.Month)
                .Select(p => new { p.Key, Lines = p });

            double sumE = 0;
            double sumF = 0;
            double sumG = 0;
            double sumH = 0;
            double sumI = 0;
            double sumJ = 0;
            double sumK = 0;
            double sumL = 0;
            double sumM = 0;
            double sumN = 0;
            double totalBalance = 0;
            double totalPenalty = 0;

            int rowIndex = 14;
            foreach (var group in groups)
            {
                ExcellUtil.InsertText(group.Key.Year + " / " + group.Key.Month, "A" + rowIndex);

                HotWaterLine incBalanceLine = group.Lines.FirstOrDefault(p => p.AmountType.Id == 2);
                if (incBalanceLine != null)
                {
                    ExcellUtil.InsertNumber(incBalanceLine.IncBalance.ToString("F"), "B" + rowIndex);
                    ExcellUtil.InsertNumber(incBalanceLine.Total.ToString("F"), "C" + rowIndex);
                    ExcellUtil.InsertNumber(incBalanceLine.Penalty.ToString("F"), "D" + rowIndex);
                }

                HotWaterLine chargeLine = group.Lines.FirstOrDefault(p => p.AmountType.Id == 3);
                if (chargeLine != null)
                {
                    ExcellUtil.InsertNumber(chargeLine.Heating.ToString("F"), "E" + rowIndex);
                    ExcellUtil.InsertNumber((chargeLine.HotWater + chargeLine.HotWaterCommonTE).ToString("F"), "F" + rowIndex);
                    ExcellUtil.InsertNumber((chargeLine.HotWaterCommonTE + chargeLine.HotWaterCommonTN).ToString("F"), "G" + rowIndex);
                    ExcellUtil.InsertNumber(chargeLine.ColdWater.ToString("F"), "H" + rowIndex);
                    ExcellUtil.InsertNumber(chargeLine.HotWaterIncreaseTN.ToString("F"), "I" + rowIndex);
                    sumE += chargeLine.Heating;
                    sumF += chargeLine.HotWater + chargeLine.HotWaterCommonTE;
                    sumG += chargeLine.HotWaterCommonTE + chargeLine.HotWaterCommonTN;
                    sumH += chargeLine.ColdWater;
                    sumI += chargeLine.HotWaterIncreaseTN;
                }

                HotWaterLine oneTimeRecalc = group.Lines.FirstOrDefault(p => p.AmountType.Id == 5);
                if (oneTimeRecalc != null)
                {
                    ExcellUtil.InsertNumber(oneTimeRecalc.Total.ToString("F"), "J" + rowIndex);
                    sumJ += oneTimeRecalc.Total;
                }

                HotWaterLine chargeTotalLine = group.Lines.FirstOrDefault(p => p.AmountType.Id == 4);
                if (chargeTotalLine != null)
                {
                    ExcellUtil.InsertNumber(chargeTotalLine.Total.ToString("F"), "K" + rowIndex);
                    ExcellUtil.InsertNumber(chargeTotalLine.Penalty.ToString("F"), "L" + rowIndex);
                    sumK += chargeTotalLine.Total;
                    sumL += chargeTotalLine.Penalty;
                }

                HotWaterLine paidLine = group.Lines.FirstOrDefault(p => p.AmountType.Id == 6);
                if (paidLine != null)
                {
                    ExcellUtil.InsertNumber(paidLine.Total.ToString("F"), "M" + rowIndex);
                    ExcellUtil.InsertNumber(paidLine.Penalty.ToString("F"), "N" + rowIndex);
                    sumM += paidLine.Total;
                    sumN += paidLine.Penalty;
                }

                HotWaterLine outBalanceLine = group.Lines.FirstOrDefault(p => p.AmountType.Id == 7);
                if (outBalanceLine != null)
                {
                    ExcellUtil.InsertNumber(outBalanceLine.Total.ToString("F"), "O" + rowIndex);
                    ExcellUtil.InsertNumber(outBalanceLine.Penalty.ToString("F"), "P" + rowIndex);
                    totalBalance = outBalanceLine.Total;
                    totalPenalty = outBalanceLine.Penalty;
                }

                foreach (char colName in "ABCDEFGHIJKLMNOP" )
                    ExcellUtil.SetBorder(colName.ToString() + rowIndex);

                rowIndex += 1;
            }

            ExcellUtil.InsertText("Итого", "A" + rowIndex);
            ExcellUtil.InsertNumber(sumE.ToString("F"), "E" + rowIndex);
            ExcellUtil.InsertNumber(sumF.ToString("F"), "F" + rowIndex);
            ExcellUtil.InsertNumber(sumG.ToString("F"), "G" + rowIndex);
            ExcellUtil.InsertNumber(sumH.ToString("F"), "H" + rowIndex);
            ExcellUtil.InsertNumber(sumI.ToString("F"), "I" + rowIndex);
            ExcellUtil.InsertNumber(sumJ.ToString("F"), "J" + rowIndex);
            ExcellUtil.InsertNumber(sumK.ToString("F"), "K" + rowIndex);
            ExcellUtil.InsertNumber(sumL.ToString("F"), "L" + rowIndex);
            ExcellUtil.InsertNumber(sumM.ToString("F"), "M" + rowIndex);
            ExcellUtil.InsertNumber(sumN.ToString("F"), "N" + rowIndex);

            foreach (char colName in "ABCDEFGHIJKLMNOP")
                ExcellUtil.SetBorder(colName.ToString() + rowIndex);

            ExcellUtil.InsertText(string.Format("Задолженность за период с {0} по {1} г.",
                startDate.ToString("dd.MM.yyyy"), finishDate.ToString("dd.MM.yyyy")), "A" + (rowIndex + 2));
            ExcellUtil.InsertNumber(totalBalance.ToString("F"), "F" + (rowIndex + 2));

            ExcellUtil.InsertText("Начислено пени", "A" + (rowIndex + 4));
            ExcellUtil.InsertNumber(totalPenalty.ToString("F"), "F" + (rowIndex + 4));

            ExcellUtil.InsertText("Итого задолженность", "A" + (rowIndex + 6));
            ExcellUtil.InsertNumber((totalBalance + totalPenalty).ToString("F"), "F" + (rowIndex + 6));

            ExcellUtil.InsertText("Оператор", "A" + (rowIndex + 9));
            ExcellUtil.InsertText(UserName, "F" + (rowIndex + 9));

            xcell.Save();
            xcell.Close();
            return fileName;
        }
    }
}
