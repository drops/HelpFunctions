using System;
using System.IO;
using System.Windows.Forms;

namespace HelpFunctions
{
    public class ErrorLog
    {
        string Filename = "errors.log";
        string Path = AppDomain.CurrentDomain.BaseDirectory;

        public ErrorLog()
        {
            if(!File.Exists(Path + Filename))
            {
                File.Create(Path + Filename);
            }
        }

        public ErrorLog(string path, string filename)
        {
            Path = path;
            Filename = filename;
            if (!File.Exists(Path + Filename))
            {
                File.Create(Path + Filename);
            }
        }

        public string WriteLog(string errorMessage)
        {
            try
            {
                File.AppendAllText(Path + Filename, AddDateAndHeader(errorMessage) + Environment.NewLine);
                return errorMessage;
            }
            catch (Exception e) { return "WriteLog: Nie można zapisać błędu: " + e.ToString(); }
        }

        public void ChangeFileName(string filename)
        {
            Filename = filename;
        }

        public string WriteAndShowLog(string errorMessage)
        {
            try
            {
                File.AppendAllText(Path + Filename, AddDateAndHeader(errorMessage) + Environment.NewLine);
                Console.WriteLine(AddDateAndHeader(errorMessage) + Environment.NewLine);
                return errorMessage;
            }
            catch (Exception e) { return "WriteAndShowLog: Nie można zapisać błędu: " + e.ToString(); }
        }

        public string WriteAndShowWindowLog(string errorMessage)
        {
            try
            {
                File.AppendAllText(Path + Filename, AddDateAndHeader(errorMessage) + Environment.NewLine);
                MessageBox.Show(AddDateAndHeader(errorMessage) + Environment.NewLine, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return errorMessage;
            }
            catch (Exception e) { return "WriteAndShowWindowLog: Nie można zapisać błędu: " + e.ToString(); }
        }

        public string WriteToLogAndTell(string errorMessage)
        {
            try
            {
                File.AppendAllText(Path + Filename, AddDateAndHeader(errorMessage) + Environment.NewLine);
                MessageBox.Show("Wystąpił błąd w działaniu programu. Treść błędu została zapisana w dzienniku błędów.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return errorMessage;
            }
            catch (Exception e) { return "WriteToLogAndTell: Nie można zapisać błędu: " + e.ToString(); }
        }

        private string AddDateAndHeader(string msg)
        {
            return DateTime.Now + "Błąd: " + msg;
        }
    }
}
