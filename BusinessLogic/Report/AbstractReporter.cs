using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReestrBKS.BusinessLogic.Report
{
    public abstract class AbstractReporter<T> where T : class, IAbstractLine
    {
        internal string UserName;
        internal string tempDirectory;

        public abstract string GetGeneralReport(List<T> lines, Subject subject, Person person, DateTime startDate, DateTime finishDate);
        public abstract string GetDetailsReport(List<T> lines, Subject subject, Person person, DateTime startDate, DateTime finishDate);

        public AbstractReporter(string userName)
        {
            UserName = userName;
            tempDirectory = Directory.GetCurrentDirectory() + "\\temp";
            if (!Directory.Exists(tempDirectory))
                Directory.CreateDirectory(tempDirectory);
        }
    }
}
