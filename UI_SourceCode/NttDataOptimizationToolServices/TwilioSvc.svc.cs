using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NttDataOptimizationToolServices
{
    public class TwilioSvc : ITwilioSvc
    {
        public string VerifyLoginOTP(string username,string OTP)
        {
            return "OTP verified:" + username +OTP+ " was successful";
        }
    }
}
