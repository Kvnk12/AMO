using System;
using System.Web.Configuration;
using System.Runtime.Serialization;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
namespace NttDataOptimizationToolServices
{

    [DataContract]
    public class EmailDetail
    {
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public string Body { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Cc { get; set; }
        [DataMember]
        public string BCc { get; set; }
        [DataMember]
        public bool IsBodyHtml { get; set; }
    }

    public class maillgunclass
    {
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string text { get; set; }
    }

    public class MFAEmailAuthService
    {
        public static MFAOTPOutput SendEmail(MFAOTPInput OTPDetail)
        //public static MessageResource SendSMS(string Phone, string OTP)
        {
            string otplength = WebConfigurationManager.AppSettings["OtpLength"];
            MFAOTPOutput outputResponse = new MFAOTPOutput();
            string OTP =ServiceHelper.GenerateNumericOTP(Convert.ToInt32(otplength));
            string emailid = "";
            try
            {
            emailid = ServiceHelper.GetUserEmail(OTPDetail.Client.UserName);
           
            string MessageBody = "Your One Time Password(OTP) for accessing Optimization Tool is " + OTP + " . Please do not share it with anyone.";

            ////Email client
           // IEmailSvc emailSvc = new EmailSvc();
            EmailDetail email = new EmailDetail();
            //email.BCc = "";
            //email.Cc;
            email.Body = MessageBody;
            email.To = emailid;
            email.Subject = "NTTData Optimization Tool: OTP for user authentication";
            email.IsBodyHtml = false;

            outputResponse.InputRequest = OTPDetail;
            outputResponse.Message = "One Time Password(OTP) has been sent to your registered email id.";
            //check here for all sent status
                outputResponse.Status = SendMail(email);
                outputResponse.Status = true;
                
            }
            catch
            {

                outputResponse.InputRequest = OTPDetail;
                outputResponse.Status = false;
                outputResponse.Message = "Error sending One Time Password(OTP) please try later or contact support team.";
            }
            int UniqOTPRowId = ServiceHelper.logOTPRequest(OTP, Convert.ToInt32(OTPDetail.Client.UserName),
               OTPDetail.Client.ClientId, (int)EnumMFAType.sms, OTPDetail.Client.ClientDevice, OTPDetail.Client.ClientIP);
            outputResponse.otpID = UniqOTPRowId;
            return outputResponse;
        }



        private  static async Task SendEailBySendGrid(EmailDetail email)
        {
            //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
            var apiKey = "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";
            //var apiKey = "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";
            // var client = new SendGridClient("SG.oUWpqSqURLWuVyqGiSrVLQ.ThPmbloDgHm4ahu0kqomfhooguDrn7q429GUQK2g4mk");
            //  var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            string EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(EmailFrom, "Nextgentool Support");
            var subject = email.Subject;
            var to = new EmailAddress(email.To, email.To);
            var plainTextContent = email.Body;
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            string s="";
        }
        
        private static bool SendThroughMailJet(EmailDetail email)
        {
            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.To.Add(email.To);
            mailMessage.Subject = email.Subject;
            mailMessage.Body = email.Body;
            var smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
            return true;
        }

        public static bool SendMail(EmailDetail email)
        {

            // string SMTPServer = WebConfigurationManager.AppSettings["SMTPServer"];
            string HOST = WebConfigurationManager.AppSettings["smtpHost"];
            int PORT = Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);
            string EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
            String userid = WebConfigurationManager.AppSettings["smtpUserId"];
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
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
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
