using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace HelpFunctions
{
    public class SoapSender
    {
        string textForSend { get; set; }
        public string targetAddress { get; set; }

        bool isAuthorization = false;
        string Login;
        string Password;

        string ReceivedText = "";
        XmlDocument responsedDocument;
        ErrorLog errorLog = new ErrorLog();

        XmlNamespaceManager manager;
        int ErrorLogMode = 0;  // 0 - zapis do pliku, 
                               // 1 - zapis do pliku i komunikat w konsoli,
                               // 2 - zapis do pliku i poinformowanie oknem dialogowym, że wystąpił błąd,
                               // 3 - zapis do pliku i wyświetlenie treści w oknie dialogowym


        public SoapSender()
        {

        }

        public SoapSender(string textforSend)
        {
            textForSend = textforSend;
        }

        public SoapSender(string textforSend, string login, string password, string targetaddress)
        {
            isAuthorization = true;
            textForSend = textforSend;
            Login = login;
            Password = password;
            targetAddress = targetaddress;
        }


        private HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"" + targetAddress);
            if (isAuthorization)
            {
                webRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Login + ":" + Password)));
                webRequest.PreAuthenticate = true;
            }

            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public void SendAndGetResponse()
        {
            try
            {
                if (textForSend == "") return;
                HttpWebRequest request = CreateWebRequest();
                XmlDocument document = new XmlDocument();
                document.LoadXml(textForSend);

                using (Stream stream = request.GetRequestStream())
                {
                    document.Save(stream);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        ReceivedText = rd.ReadToEnd();
                        
                    }
                }
                if(ReceivedText != "")
                {
                    responsedDocument = new XmlDocument();
                    responsedDocument.LoadXml(ReceivedText);
                }
            }catch (Exception ex)
            {
                SaveError(ex.ToString());
            }
        }

        public void AddNamespaces(string prefix, string uri)
        {
            if (manager == null) manager = new XmlNamespaceManager(responsedDocument.NameTable);
            manager.AddNamespace(prefix, uri);
        }

        public string ReadNodeInResponse(string xpath)
        {
            try
            {
                if (responsedDocument.DocumentElement.SelectSingleNode(xpath) == null) return "";
                else return responsedDocument.DocumentElement.SelectSingleNode(xpath).InnerText;
            }catch (Exception ex)
            {
                SaveError(ex.ToString());
                return "";
            }
            
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
