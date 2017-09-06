using System;
using System.Xml;

namespace HelpFunctions
{
    public class ConfigReader
    {
        string FileName = "set.xml";
        string Path = AppDomain.CurrentDomain.BaseDirectory;


        XmlDocument document;
        ErrorLog errorLog = new ErrorLog();
        int ErrorLogMode = 0;  // 0 - zapis do pliku, 
                               // 1 - zapis do pliku i komunikat w konsoli,
                               // 2 - zapis do pliku i poinformowanie oknem dialogowym, że wystąpił błąd,
                               // 3 - zapis do pliku i wyświetlenie treści w oknie dialogowym





        public ConfigReader()
        {
            try
            {
                document = new XmlDocument();
                document.Load(Path + FileName);
            }catch (Exception e)
            {
                SaveError("ConfigReader->Konstruktor: " + e.ToString());
            }
        }

        public ConfigReader(string path, string filename)
        {
            Path = path;
            FileName = filename;
            try
            {
                document = new XmlDocument();
                document.Load(Path + FileName);
            }catch (Exception e)
            {
                SaveError("ConfigReader->Konstruktor: " + e.ToString());
            }
            
        }

        public string ReadDataFromConfig(string xmlPath)
        {
            if(document.DocumentElement.SelectSingleNode(xmlPath) != null)
            {
                return document.DocumentElement.SelectSingleNode(xmlPath).InnerText;
            }else
            {
                SaveError("ConfigReader->ReadDataFromConfig: Albo węzeł " + xmlPath + " nie istnieje, albo jest pusty. Mogą być problemy.");
                return "";
            }
        }

        public XmlNodeList ReadNodesFromConfig(string NodesName)
        {
            XmlNodeList NodeList = document.SelectNodes(NodesName);
            if(NodeList == null || NodeList.Count == 0)
            {
                SaveError("ConfigReader->ReadNodesFromConfig: Brak listy szukanych węzłów. Mogą być problemy.");
            }
            return NodeList;
        }

        public int ReturnNodesCount(string XPath)
        {
            return document.SelectNodes(XPath).Count;
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
