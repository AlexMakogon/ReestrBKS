using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReestrBKS.BusinessLogic
{
    public static class Logger
    {
        private static string logFile = "log.txt";

        static Logger()
        {
            if (!File.Exists(logFile))
                File.CreateText(logFile).Close();
        }

        public static void WriteStr(string str)
        {
            using (StreamWriter file = new StreamWriter(logFile, true))
            {
                file.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("dd.MM.yyyy HH:mm"), str));
                file.Close();
            }  
        }
    }
}
