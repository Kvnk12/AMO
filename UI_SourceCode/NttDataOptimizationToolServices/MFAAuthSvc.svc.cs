using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NttDataOptimizationToolServices
{
    public class MFAAuthSvc : IMFAAuthSvc
    {
       
        public bool RemoveDevice(LoggedDevices Device)
        {
            return ServiceHelper.deleteDevice(Device);
        }

        public List<LoggedDevices> SavedDevices(ClientDetail Client)
        {

            return ServiceHelper.getLoggedInDevices(Client);
        }

        public MFAOTPOutput SendEmailOTP(MFAOTPInput OTPDetail)
        {
            return MFAEmailAuthService.SendEmail(OTPDetail);
        }

        public MFAOTPOutput SendSMSToUser(MFAOTPInput OTPDetail)
        {
            return MFASMSAuthService.SendSMSToUser(OTPDetail);
        }

        public MFAOTPOutput SendSMSOTP(MFAOTPInput OTPDetail)
        {

           return MFASMSAuthService.SendSMS(OTPDetail);
          //  return new MFAOTPOutput { InputRequest = new MFAOTPInput(), Message = "success", Status = true };
        }

        public MFAOTPStatus VerifyOTP(MFAOTP OTP)
        {

            return ServiceHelper.VerifyOTP(OTP);
        }
    }
}
