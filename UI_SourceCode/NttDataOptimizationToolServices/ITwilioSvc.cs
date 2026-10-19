using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NttDataOptimizationToolServices
{
 
    [ServiceContract]
    public interface ITwilioSvc
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/VerifyLoginOTP/{username}/{OTP}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //[WebInvoke(Method = "GET", UriTemplate = "/VerifyLoginOTP/{username}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string VerifyLoginOTP(string username);
        string VerifyLoginOTP(string username,string OTP);

    }

}
