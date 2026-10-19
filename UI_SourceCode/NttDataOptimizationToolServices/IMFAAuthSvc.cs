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
    public interface IMFAAuthSvc
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/VerifyOTP", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        MFAOTPStatus VerifyOTP(MFAOTP OTP);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SendSMSOTP", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        MFAOTPOutput SendSMSOTP(MFAOTPInput OTPDetail);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SendSMSToUser", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        MFAOTPOutput SendSMSToUser(MFAOTPInput OTPDetail);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SendEmailOTP", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        MFAOTPOutput SendEmailOTP(MFAOTPInput OTPDetail);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/RemoveDevice", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool RemoveDevice(LoggedDevices Device );

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SavedDevices", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<LoggedDevices> SavedDevices(ClientDetail Client);


    }

    [DataContract]
    public class MFAOTP
    {
        [DataMember]
        public string OTP { get; set; }
        [DataMember]
        public int OTPType { get; set; }
        [DataMember]
        public ClientDetail Client { get; set; }
        [DataMember]
        public bool RememberDevice { get; set; }
        [DataMember]
        public int otpID { get; set; }
    }

    [DataContract]
    public class MFAOTPStatus
    {
        [DataMember]
        public string OTP { get; set; }
        [DataMember]
        public ClientDetail Client { get; set; }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public bool OTPExpired{ get; set; }
    }

    [DataContract]
    public class LoggedDevices
    {
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public int MFADeviceId { get; set; }
    }
    
    [DataContract]
    public class ClientDetail
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public string ClientIP { get; set; }
        [DataMember]
        public string ClientDevice { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
    }


    [DataContract]
    public class MFAOTPInput
    {
        [DataMember]
        public int OTPType { get; set; }
        [DataMember]
        public string Message { get; set; } //optional
        [DataMember]
        public ClientDetail Client { get; set; }
        [DataMember]
        public bool RememberDevice { get; set; }
    }

    [DataContract]
    public enum EnumMFAType
    {
        sms = 1,
        email = 2,
        app= 3
    }

    [DataContract]
    public class MFAOTPOutput
    {
        [DataMember]
        public MFAOTPInput InputRequest { set; get; }
        [DataMember]
        public bool Status { set; get; }
        [DataMember]
        public string Message { set; get; }
        [DataMember]
        public int otpID { get; set; }
    }


}
