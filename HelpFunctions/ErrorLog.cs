using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace HelpFunctions
{
    class ErrorLog
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
            catch (Exception e) { }
            return errorMessage;
        }

        public void WriteAndShowLog(string errorMessage)
        {
            try
            {
                File.AppendAllText(Path + Filename, AddDateAndHeader(errorMessage) + Environment.NewLine);
                Console.WriteLine(AddDateAndHeader(errorMessage) + Environment.NewLine);
            }
            catch (Exception e) { }
        }

        public void WriteAndShowWindowLog(string errorMessage)
        {
            try
            {
                File.AppendAllText(Path + Filename, AddDateAndHeader(errorMessage) + Environment.NewLine);
                MessageBox.Show(AddDateAndHeader(errorMessage) + Environment.NewLine, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e) { }
        }

        private string AddDateAndHeader(string msg)
        {
            return DateTime.Now + "Błąd: " + msg;
        }
    }
}
