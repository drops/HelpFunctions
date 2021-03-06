﻿using System;
using System.IO;

namespace HelpFunctions
{
    public class Log
    {
        string FileName = "log.txt";
        string Path = AppDomain.CurrentDomain.BaseDirectory;
        ErrorLog errorLog = new ErrorLog();

        int ErrorLogMode = 0;  // 0 - zapis do pliku, 
                               // 1 - zapis do pliku i komunikat w konsoli,
                               // 2 - zapis do pliku i poinformowanie oknem dialogowym, że wystąpił błąd,
                               // 3 - zapis do pliku i wyświetlenie treści w oknie dialogowym

        

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
                File.AppendAllText(Path + FileName, AddDate(logMessage) + Environment.NewLine);
            }catch (Exception e)
            {
                SaveError(e.ToString());
            }
        }

        public void WriteAndShow(string logMessage)
        {
            try
            {
                File.AppendAllText(Path + FileName, AddDate(logMessage) + Environment.NewLine);
                Console.WriteLine(AddDate(logMessage));
            }catch (Exception e)
            {
                SaveError(e.ToString());
            }
        }

        private string AddDate(string message)
        {
            return DateTime.Now + ": " + message;
        }

        public void SetErrorLogFileName(string filename)
        {
            errorLog.ChangeFileName(filename);
        }

        /// <summary>
        // 0 - zapis do pliku, 
        // 1 - zapis do pliku i komunikat w konsoli,
        // 2 - zapis do pliku i poinformowanie oknem dialogowym, że wystąpił błąd,
        // 3 - zapis do pliku i wyświetlenie treści w oknie dialogowym
        /// </summary>
        /// <param name="mode"></param>
        public void SetErrorLogMode(int mode)
        {
            ErrorLogMode = mode;
        }
        private string SaveError(string ErrorMessage)
        {
            if (ErrorLogMode == 0) errorLog.WriteLog(ErrorMessage);
            else if (ErrorLogMode == 1) errorLog.WriteAndShowLog(ErrorMessage);
            else if (ErrorLogMode == 2) errorLog.WriteToLogAndTell(ErrorMessage);
            else if (ErrorLogMode == 3) errorLog.WriteAndShowWindowLog(ErrorMessage);
            else errorLog.WriteLog(ErrorMessage);
            return ErrorMessage;
        }

    }
}
