using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;

namespace NttDataOptimizationToolServices
{
    public class MFASMSAuthService
    {
        public static MFAOTPOutput SendSMSToUser(MFAOTPInput OTPDetail)
        {
            
            string otplength = WebConfigurationManager.AppSettings["OtpLength"];
            string OTP = ServiceHelper.GenerateNumericOTP(Convert.ToInt32(otplength));

            string userphone = ServiceHelper.GetUserPhone(OTPDetail.Client.UserName);

            string MessageBody = "Your One Time Password(OTP) for accessing NTTdata Optimization tool is " + OTP + " . Please do not share it with anyone.";

            MFAOTPOutput outputResponse = new MFAOTPOutput();
            outputResponse.InputRequest = OTPDetail;
            string outmessage = "";
            if (SendTwilioSMS(userphone, MessageBody,out outmessage))
            {
                outputResponse.Status = true;
                outputResponse.Message = "Successfully sent OTP.";
            }
            else
            {
                outputResponse.Status = false;
                outputResponse.Message = "Error sending SMS.";
                EventLog log = new EventLog()
                {

                };
            }

            int UniqOTPRowId = ServiceHelper.logOTPRequest(OTP, Convert.ToInt32(OTPDetail.Client.UserName),
                    OTPDetail.Client.ClientId, (int)EnumMFAType.sms, OTPDetail.Client.ClientDevice, OTPDetail.Client.ClientIP);
            outputResponse.otpID = UniqOTPRowId;


            return outputResponse;
        }


        public static MFAOTPOutput SendSMS(MFAOTPInput OTPDetail)
        {

            string otplength = WebConfigurationManager.AppSettings["OtpLength"];
            string OTP = ServiceHelper.GenerateNumericOTP(Convert.ToInt32(otplength));
            string userphone = OTPDetail.Client.PhoneNumber;
            string MessageBody = "Your One Time Password(OTP) for accessing NTTdata Optimization tool is " + OTP + " . Please do not share it with anyone.";
            MFAOTPOutput outputResponse = new MFAOTPOutput();
            outputResponse.InputRequest = OTPDetail;
            string returnmsg = "";
            if (SendTwilioSMS(OTPDetail.Client.PhoneNumber, MessageBody, out returnmsg))
            {
                outputResponse.Status = true;
                outputResponse.Message = "One Time Password(OTP) has been sent to your registered phone number.";
            }
            else
            {
                outputResponse.Status = false;
                outputResponse.Message = "Error sending One Time Password(OTP) please try later OR choose an alternative option.";
            }
            int UniqOTPRowId = ServiceHelper.logOTPRequest(OTP, Convert.ToInt32(OTPDetail.Client.UserName),
                OTPDetail.Client.ClientId, (int)EnumMFAType.sms, OTPDetail.Client.ClientDevice, OTPDetail.Client.ClientIP);
            outputResponse.otpID = UniqOTPRowId;

            return outputResponse;
        }


        private static bool SendTwilioSMS(string phoneNumber, string MessageBody, out string outmessage)
        {
            string accountSid = WebConfigurationManager.AppSettings["TwilioAccountSID"];
            string authToken = WebConfigurationManager.AppSettings["TwilioAuthToken"];
            string FromPhoneNumber = WebConfigurationManager.AppSettings["TwilioFromNumber"];
            outmessage = "";

            try
            {
                //var client = new TwilioRestClient(accountSid, authToken);
                TwilioClient.Init(accountSid, authToken);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                | SecurityProtocolType.Tls11
                                                | SecurityProtocolType.Tls12
                                                | SecurityProtocolType.Ssl3;
                MessageResource message = MessageResource.Create(
                   from: new Twilio.Types.PhoneNumber(FromPhoneNumber),
                   body: MessageBody,
                   to: new Twilio.Types.PhoneNumber(phoneNumber)
               );
                // MessageResource message = MessageResource.Create(
                //    from: new Twilio.Types.PhoneNumber(FromPhoneNumber),
                //    body: MessageBody,
                //    to: new Twilio.Types.PhoneNumber(phoneNumber),
                //    client: client
                //);
                if (message.Status == MessageResource.StatusEnum.Undelivered || message.Status == MessageResource.StatusEnum.Failed)
                {
                   // outmessage=
                    return false;
                    //outputResponse.Message = "Failed to send OTP.";
                }
                else
                {
                    return true; ;
                }
            }
            catch(Exception ex)
            {
                //retry
                try
                {
                    var client = new TwilioRestClient(accountSid, authToken);
                    // TwilioClient.Init(accountSid, authToken);
                    // message = MessageResource.Create(
                    //    from: new Twilio.Types.PhoneNumber(FromPhoneNumber),
                    //    body: MessageBody,
                    //    to: new Twilio.Types.PhoneNumber(phoneNumber)
                    //);
                    MessageResource message = MessageResource.Create(
                       from: new Twilio.Types.PhoneNumber(FromPhoneNumber),
                       body: MessageBody,
                       to: new Twilio.Types.PhoneNumber(phoneNumber),
                       client: client
                   );
                    if (message.Status == MessageResource.StatusEnum.Undelivered || message.Status == MessageResource.StatusEnum.Failed)
                    {
                        return false;
                        //outputResponse.Message = "Failed to send OTP.";
                    }
                    else
                    {
                        return true; ;
                    }
                }
                catch(Exception ex1)
                {
                    outmessage = ex1.Message;
                    return false;
                    //outputResponse.Message = "Error sending SMS";
                }
            }
           
        }

    }
}
