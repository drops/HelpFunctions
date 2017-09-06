using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpFunctions;
using System.Net.Mail;

namespace HelpFunctions
{
    public class Mailer
    {
        ErrorLog eLog;

        SmtpClient Client;
        MailMessage Message;

        public int Port { get; set; }
        public string Host { get; set; }
        public bool IsSSL = false;
        public int TimeOut = 0;
        public bool defaultCredential = false;
        public string mailLogin { get; set; }
        public string mailPassword { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        MailAddressCollection addressCollection;

        int ErrorLogMode = 0;  // 0 - zapis do pliku, 
                               // 1 - zapis do pliku i komunikat w konsoli,
                               // 2 - zapis do pliku i poinformowanie oknem dialogowym, że wystąpił błąd,
                               // 3 - zapis do pliku i wyświetlenie treści w oknie dialogowym

        public Mailer()
        {
            eLog = new ErrorLog();
        }

        public bool SendMail()
        {
            Client = CreateSmtpClient();
            Message = CreateMailMessage();
            try
            {
                Client.Send(Message);
                return true;
            }catch (Exception e)
            {
                SaveError("Mailer->SendMail: " + e.ToString());
                return false;
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            try
            {
                client.Port = this.Port;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = this.Host;
                client.EnableSsl = IsSSL;
                client.Timeout = TimeOut;
                client.UseDefaultCredentials = defaultCredential;
                client.Credentials = new System.Net.NetworkCredential(mailLogin, mailPassword);

            }catch (Exception e)
            {
                SaveError("Mailer->CreateSmtpClient: " + e.ToString());
            }

            return client;
        }

        private MailMessage CreateMailMessage()
        {
            MailMessage mail = new MailMessage();
            try
            {
                mail.From = new MailAddress(this.From);
                for(int i = 0; i < addressCollection.Count; i++)
                {
                    mail.To.Add(addressCollection[i]);
                }
                mail.Subject = this.Subject;
                mail.Body = Content;
                mail.IsBodyHtml = false;
            }catch(Exception e)
            {
                SaveError("Mailer->CreateMessage: " + e.ToString());
            }
            return mail;
        }

        public void AddAddresses(string address)
        {
            if(addressCollection == null) addressCollection = new MailAddressCollection();
            addressCollection.Add(address);
        }

        public void ClearToAddresses()
        {
            Message.To.Clear();
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
            if (ErrorLogMode == 0) eLog.WriteLog(ErrorMessage);
            else if (ErrorLogMode == 1) eLog.WriteAndShowLog(ErrorMessage);
            else if (ErrorLogMode == 2) eLog.WriteToLogAndTell(ErrorMessage);
            else if (ErrorLogMode == 3) eLog.WriteAndShowWindowLog(ErrorMessage);
            else eLog.WriteLog(ErrorMessage);
            return ErrorMessage;
        }

        public void ChangeErrorLogFileName(string filename)
        {
            eLog.ChangeFileName(filename);
        }

    }
}
