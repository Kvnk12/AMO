using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NttDataOptimizationToolServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmailSvc" in both code and config file together.
    [ServiceContract]
    public interface IEmailSvc
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/VerifyOTP", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool SendMail(EmailDetail email);
       

    }
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

}
