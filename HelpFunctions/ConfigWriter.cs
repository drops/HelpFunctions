using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace HelpFunctions
{
    public class ConfigWriter
    {
        string FileName = "set.xml";
        string Path = AppDomain.CurrentDomain.BaseDirectory;

        XElement rootElement;

        public XmlDocument document;
        ErrorLog errorLog = new ErrorLog();
        int ErrorLogMode = 0;  // 0 - zapis do pliku, 
                               // 1 - zapis do pliku i komunikat w konsoli,
                               // 2 - zapis do pliku i poinformowanie oknem dialogowym, że wystąpił błąd,
                               // 3 - zapis do pliku i wyświetlenie treści w oknie dialogowym


        public ConfigWriter()
        {
            try
            {
                if (File.Exists(Path + FileName) == true)
                {
                    document = new XmlDocument();
                    document.Load(Path + FileName);
                }
                else
                {
                    document = new XmlDocument();
                }
            }catch (Exception e)
            {
                SaveError("ConfigWriter->Konstruktor: " + e.ToString());
            }
            
        }

        public ConfigWriter(string path, string filename)
        {
            Path = path;
            FileName = filename;
            try
            {
                if (File.Exists(Path + FileName) == true)
                {
                    document = new XmlDocument();
                    document.Load(Path + FileName);
                }
                else
                {
                    document = new XmlDocument();
                }
            }
            catch (Exception e)
            {
                SaveError("ConfigWriter->Konstruktor: " + e.ToString());
            }
        }

        public void SetErrorLogFileName(string filename)
        {
            errorLog.ChangeFileName(filename);
        }

        public void CreateNewNode(string newNode, string content)
        {
            try
            {
                XmlNode node = document.CreateElement(newNode);
                node.InnerText = content;
                document.AppendChild(node);
            }catch (Exception e)
            {
                SaveError("ConfigWriter->CreateNewNode: " + e.ToString());
            }
            
        }
        public void CreateNewNode(string newNode, string content, XmlNode parentNode)
        {
            try
            {
                XmlNode node = document.CreateElement(newNode);
                node.InnerText = content;
                parentNode.AppendChild(node);
            }catch (Exception e)
            {
                SaveError("ConfigWriter->CreateNewNode: " + e.ToString());
            }
            
        }

        public void CreateNewNodeIfNotExists(string newNode, string content, XmlNode parentNode)
        {
            try
            {
                if (parentNode.SelectNodes(newNode).Count == 0)
                {
                    XmlNode node = document.CreateElement(newNode);
                    node.InnerText = content;
                    parentNode.AppendChild(node);
                }
            }catch(Exception e)
            {
                SaveError("ConfigWriter->CreateNewIfNotExists: " + e.ToString());
            }
            
        }

        public void CreateNewNodeIfNotExists(string newNode, string content)
        {
            try
            {
                if (document.SelectNodes(newNode).Count == 0)
                {
                    XmlNode node = document.CreateElement(newNode);
                    node.InnerText = content;
                    document.AppendChild(node);
                }
            }catch (Exception e)
            {
                SaveError("ConfigWriter->CreateNewNodeIfNotExists: " + e.ToString());
            }
            
        }

        public XmlNode GetNode(string xpath)
        {
            return document.SelectSingleNode(xpath);
        }

        public void FillNode(XmlNode node, string content)
        {
            try
            {
                node.InnerText = content;
            }catch (Exception e)
            {
                SaveError("ConfigWriter->FillNode: " + e.ToString());
            }
            
        }

        public void SaveXmlDocument()
        {
            try
            {
                document.Save(Path + FileName);
            }catch(Exception e)
            {
                SaveError("ConfigWriter->SaveXmlDocument: " + e.ToString());
            }
            
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
