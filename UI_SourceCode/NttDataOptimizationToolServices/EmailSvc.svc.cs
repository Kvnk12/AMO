using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web.Configuration;

namespace NttDataOptimizationToolServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmailSvc" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmailSvc.svc or EmailSvc.svc.cs at the Solution Explorer and start debugging.
    public class EmailSvc : IEmailSvc
    {


        public bool SendMail(EmailDetail email)
        {

           // string SMTPServer = WebConfigurationManager.AppSettings["SMTPServer"];
            string HOST = WebConfigurationManager.AppSettings["smtpHost"];
            int PORT = Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);
            string EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];

            MailMessage mail = new MailMessage();
            
            mail.From = new MailAddress(EmailFrom);
            // mail.To
            mail.To.Add(email.To); // Replace with a "To" address. If your account is still in the

            if (!string.IsNullOrEmpty(email.BCc))
            {
                mail.Bcc.Add(email.BCc);
            }
            if (!string.IsNullOrEmpty(email.Cc))
            {
                mail.Bcc.Add(email.Cc);
            }

            mail.Subject = email.Subject + " #secure# ";
            mail.Body = email.Body;
            mail.IsBodyHtml = email.IsBodyHtml;
       


            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {

                // Send the email. 
                try
                {

                    String pwd = WebConfigurationManager.AppSettings["smtpUserPwd"];
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(EmailFrom, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;

        }
    }
}
