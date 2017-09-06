using System;
using System.Net;

// TODO: Dodać sciaganie plikow i sprawdzanie czy juz takie istnieje na serwerze

namespace HelpFunctions
{
    public class FtpCommunicator
    {
        WebClient client;

        string Filename;

        string ftpAddress;
        string Login;
        string Password;
        string path = AppDomain.CurrentDomain.BaseDirectory;
        ErrorLog errorLog = new ErrorLog();
        int ErrorLogMode = 0;  // 0 - zapis do pliku, 
                               // 1 - zapis do pliku i komunikat w konsoli,
                               // 2 - zapis do pliku i poinformowanie oknem dialogowym, że wystąpił błąd,
                               // 3 - zapis do pliku i wyświetlenie treści w oknie dialogowym



        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
        
        

        public FtpCommunicator(string ftpaddress, string login, string password, string filename)
        {
            ftpAddress = ftpaddress;
            Login = login;
            Password = password;
            Filename = filename;

            client = new WebClient();
            client.Credentials = new NetworkCredential(Login, Password);
        }

        public bool UploadFileToFtp()
        {
            bool result = false;
            try
            {
                client.UploadFile(ftpAddress + @"/" + Filename, Path + Filename);

                result = true;
            }catch (Exception e)
            {
                SaveError(e.ToString());
                result = false;
            }

            return result;
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
