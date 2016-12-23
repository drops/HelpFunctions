using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpFunctions
{
    class Log
    {
        string FileName = "log.txt";
        string Path = AppDomain.CurrentDomain.BaseDirectory;

        public Log()
        {
            if(!File.Exists(Path + FileName))
            {
                File.Create(Path + FileName);
            }
        }

        public Log(string path, string filename)
        {
            FileName = filename;
            Path = path;

            if(!File.Exists(Path + FileName))
            {
                File.Create(Path + FileName);
            }
        }

        public void WriteLog(string logMessage)
        {
            try
            {
                File.AppendAllText(Path + FileName, AddDate(logMessage));
            }catch (Exception e)
            {

            }
        }

        public void WriteAndShow(string logMessage)
        {
            try
            {
                File.AppendAllText(Path + FileName, AddDate(logMessage));
                Console.WriteLine(AddDate(logMessage));
            }catch (Exception e)
            {

            }
        }

        private string AddDate(string message)
        {
            return DateTime.Now + ": " + message;
        }

    }
}
