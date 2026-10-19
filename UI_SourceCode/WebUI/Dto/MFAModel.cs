using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebUI.App_Start;

namespace WebUI.Dto
{
    [Serializable]
    public class MFAModel
    {
        public int ClientId;
        public int UserId;

        [System.ComponentModel.DefaultValue(true)]
        public bool IsPhoneNull { get; set; }
        [System.ComponentModel.DefaultValue(true)]
        public bool IsPhoneVerfied { get; set; }
        public string PhoneNumber { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        public bool IsEmailVerfied { get; set; }
        [System.ComponentModel.DefaultValue(true)]
        public bool NewDevice { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        public bool GoodToGo { get; set; }

        public bool IsUserNull { get; set; }
        public int? CountryCode { get; set; }
        public IEnumerable<SelectListItem> CountryList
        {
            get
            {
                return Helper.GetCountriesList();
            }
        }
        public bool Authenticated { get; set; }
        /// <summary>
        /// Masked Phone Number
        /// </summary>
        public string MphoneNum { get; set; }
        /// <summary>
        /// Property holds "EnableMFA" from Client table
        /// </summary>
        public bool MFARequired { get; set; }

    }
    [Serializable]
    public class OTPModel
    {
        public string OTP { get; set; }
        public int UserId { get; set; }
        public int LogRequestId { get; set; }
        public EnumMFAType MFAType { get; set; }
        /// <summary>
        /// OTP valid only for #minutes 180 seconds
        /// </summary>
        public DateTime ValidTime { get; set; }
    }
    [Serializable]
    public enum EnumMFAType
    {
        sms = 1,
        email = 2

    }
    [Serializable]
    public class MFAOTP
    {
        public string OTP { get; set; }
        public int OTPType { get; set; }
        public ClientDetail Client { get; set; }
        public bool RememberDevice { get; set; }
        public int otpID { get; set; }
    }
    [Serializable]
    public class MFAOTPStatus
    {
        public string OTP { get; set; }
        public ClientDetail Client { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public bool OTPExpired { get; set; }
    }

    [Serializable]
    public class ClientDetail
    {
        public string UserName { get; set; }
        public int ClientId { get; set; }
        public string ClientIP { get; set; }
        public string ClientDevice { get; set; }
        public string ClientName { get; set; }
        public string PhoneNumber { get; set; }
        // public Guid UniqId { get; set; }
        public bool MultipleApprovals { get; set; }
        public bool? SupportEcoSystem { get; set; }
        public int VerticalId { get; set; }
        public int SubVerticalId { get; set; }
        public bool EnableMFA { get; set; }
    }

    [Serializable]
    public class MFAOTPInput
    {
        public int OTPType { get; set; }
        public string Message { get; set; } //optional
        public ClientDetail Client { get; set; }
        public bool RememberDevice { get; set; }
    }

    [Serializable]
    public class MFAOTPOutput
    {
        public MFAOTPInput InputRequest { set; get; }
        public bool Status { set; get; }
        public string Message { set; get; }
        public int otpID { get; set; }
    }

    [Serializable]
    public class MFAUserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
  [Serializable]
    public class LoggedDevices
    {
        public string DeviceName { get; set; }
        public int MFADeviceId { get; set; }
    }

}