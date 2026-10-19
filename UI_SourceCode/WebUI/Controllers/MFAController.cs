using Data;
using Domain.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using WebUI.App_Start;
using WebUI.Dto;

namespace WebUI.Controllers
{
    [AllowAnonymous]
    public class MFAController : Controller
    {
        // GET: /MFA/

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SendOTPSMS(string phone = "", string CountryCode = "")
            {
           // var client = (Client)Session["NewClient"];
            ClientDetail client = (ClientDetail)Session["NewSessionClient"];
            var UserId = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var deviceName = HttpContext.Request.UserHostName;
            var deviceIp = HttpContext.Request.UserHostAddress;
            //If phone Number already exists
            if (phone.Trim() == "" && CountryCode.Trim()=="")
            {
                MFAOTPInput input = new MFAOTPInput()
                {
                    Message = "SMS",
                    Client = new ClientDetail { ClientDevice = deviceName, ClientId = client.ClientId, ClientIP = deviceIp, UserName = UserId.ToString(), PhoneNumber="" },
                    OTPType = (int)EnumMFAType.sms,
                    RememberDevice = false
                };

                //POST Request
                string requestUrl = WebConfigurationManager.AppSettings["APISendSMSOTP"].ToString();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MFAOTPInput));
                MemoryStream mem = new MemoryStream();
                ser.WriteObject(mem, input);
                string dataString = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Encoding = Encoding.UTF8;
                string ronse=webClient.UploadString(requestUrl, "POST", dataString);
                
                var ResModel = JsonConvert.DeserializeObject<MFAOTPOutput>(ronse);
                //if false
                if (!ResModel.Status)
                {
                    return Json(new { failure = ResModel.Message });
                }
                else
                {
                    setOTPDetailsInSession(ResModel);
                    return Json(new { success = ResModel.Message });
                }

            }
            //If number doesnot exists- SAVE IN USERPROFILES AND CONTNUE
            else
            {
                //if phone number is Invalid
                if (phone.Trim().Length != 10)
                {
                    return Json(new { failure = "Invalid Phone Number!" });
                }
                //Only number is valid
                else
                {
                    MFAOTPInput input = new MFAOTPInput()
                    {
                        Message = "SMS",
                        Client = new ClientDetail { ClientDevice = deviceName, ClientId = client.ClientId, ClientIP = deviceIp, UserName = UserId.ToString(), PhoneNumber="" },
                        OTPType = (int)EnumMFAType.sms,
                        RememberDevice = false
                    };
                    var UPentity = unitOfWork.UserProfile.GetByID(UserId);
                    UPentity.Phone = phone.Trim();
                    if (CountryCode.Trim() != "")
                    {
                        UPentity.CountryCode = Convert.ToInt32(CountryCode.Trim());
                    }
                    unitOfWork.UserProfile.Update(UPentity);
                    unitOfWork.Save();

                    //POST Request
                    string requestUrl = WebConfigurationManager.AppSettings["APISendSMSOTP"].ToString();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MFAOTPInput));
                    MemoryStream mem = new MemoryStream();
                    ser.WriteObject(mem, input);
                    string dataString = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                    WebClient webClient = new WebClient();
                    webClient.Headers["Content-type"] = "application/json";
                    webClient.Encoding = Encoding.UTF8;
                    string ronse = webClient.UploadString(requestUrl, "POST", dataString);
                    var ResModel = JsonConvert.DeserializeObject<MFAOTPOutput>(ronse);
                    //if false
                    if (!ResModel.Status)
                    {
                        return Json(new { failure = ResModel.Message });
                    }
                    else
                    {
                        setOTPDetailsInSession(ResModel);
                        return Json(new { success = ResModel.Message });
                    }
                }
            }
        }

        public ActionResult ReSendOTPSMS()
        {
          //  var client = (Client)Session["NewClient"];
            ClientDetail client = (ClientDetail)Session["NewSessionClient"];
            var UserId = (int)Session["UserId"];
            var deviceName = HttpContext.Request.UserHostName;
            var deviceIp = HttpContext.Request.UserHostAddress;
            
            //Post Data
            MFAOTPInput input = new MFAOTPInput()
            {
                Message ="",
                Client = new ClientDetail { ClientDevice = deviceName, ClientId = client.ClientId, ClientIP = deviceIp, UserName = UserId.ToString(), PhoneNumber="" },
                OTPType = (int)EnumMFAType.sms,
                RememberDevice = false
            };
            //POST Request
            HttpClient api = new HttpClient();
            string requestUrl = WebConfigurationManager.AppSettings["APISendSMSOTP"].ToString();
            string jsonString = JsonConvert.SerializeObject(input);
            var webapi = new WebClient();
            var dataString = JsonConvert.SerializeObject(input);
            webapi.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            var ronse = webapi.UploadString(requestUrl, "POST", dataString);
            //Parsing JSON string response into model
            var ResModel = JsonConvert.DeserializeObject<MFAOTPOutput>(ronse);

            //if false
            if (!ResModel.Status)
            {
                return Json(new { failure = ResModel.Message });
            }
            else
            {
                return Json(new { success = ResModel.Message });
            }

        }

        private void setOTPDetailsInSession(MFAOTPOutput model)
        {
            OTPModel mo = new OTPModel();
            mo.LogRequestId = model.otpID;
            mo.MFAType = (EnumMFAType)model.InputRequest.OTPType;
            
            Session["OTPModel"] = mo;
        }
        [HttpPost]
        public JsonResult SendOTPEmail()
        {
            // var client = (Client)Session["NewClient"];
            ClientDetail client = (ClientDetail)Session["NewSessionClient"];
            var UserId = (int)Session["UserId"];
            var deviceName = HttpContext.Request.UserHostName;
            var deviceIp = HttpContext.Request.UserHostAddress;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            try
            {

                //Post Data
                MFAOTPInput input = new MFAOTPInput()
                {
                    Client = new ClientDetail { ClientDevice = deviceName, ClientId = client.ClientId, ClientIP = deviceIp, UserName = UserId.ToString(), PhoneNumber="" },
                    OTPType = (int)EnumMFAType.email,
                    RememberDevice = false
                };
                //POST Request
                HttpClient api = new HttpClient();
                string requestUrl = WebConfigurationManager.AppSettings["APISendEmailOTP"].ToString();
                string jsonString = JsonConvert.SerializeObject(input);
                var webapi = new WebClient();
                var dataString = JsonConvert.SerializeObject(input);
                webapi.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                var ronse = webapi.UploadString(requestUrl, "POST", dataString);
                //Parsing JSON string response into model
                var ResModel = JsonConvert.DeserializeObject<MFAOTPOutput>(ronse);

                if (ResModel.Status)
                {
                    setOTPDetailsInSession(ResModel);
                    return Json(new { success = ResModel.Message});
                }
                else
                {
                    return Json(new { failure= ResModel.Message });
                }

            }
            catch (Exception)
            {
                return Json(new { failure = "Server error! Please try again." });
            }

        }

        private static int logOTPRequest(string OTP, int UserId, int ClientId, int MFAType, string DeviceName, string DeviceIP)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var entity = new MFAUserAuthLogging()
            {
                ClientId = ClientId,
                UserId = UserId,
                OTP = OTP,
                MFATypeId = MFAType,
                DeviceIP = DeviceIP,
                DeviceName = DeviceName,
                RequestedTime = DateTime.Now,
            };
            unitOfWork.MFAUserAuthLogging.Insert(entity);
            unitOfWork.Save();
            return entity.Id;
        }
        public static void logOTPVerify(int LogId, bool RememberThisDevice)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var entity = unitOfWork.MFAUserAuthLogging.GetByID(LogId);
            entity.VerifiedTime = DateTime.Now;
            unitOfWork.MFAUserAuthLogging.Update(entity);
            unitOfWork.Save();
            if (RememberThisDevice)
            {
                SaveThisDevice(entity);
            }
        }

        private static void SaveThisDevice(MFAUserAuthLogging mod)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var entity = new MFAUserLoggedInDevice()
            {
                ClientId = mod.ClientId,
                UserId = mod.UserId,
                DeviceIP = mod.DeviceIP,
                DeviceName = mod.DeviceName,
                MFATypeId = mod.MFATypeId,
                CreatedDt = DateTime.Now,
                IsDeleted = false
            };
            unitOfWork.MFAUserLoggedInDevice.Insert(entity);
            unitOfWork.Save();
        }

        public static void Phone_Email_Verified(int UserId, EnumMFAType mfa)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var UP = unitOfWork.UserProfile.GetByID(UserId);
            switch (mfa)
            {
                case EnumMFAType.sms:
                    UP.IsPhoneVerified = true;
                    unitOfWork.UserProfile.Update(UP);
                    break;

                case EnumMFAType.email:
                    UP.IsEmailVerified = true;
                    unitOfWork.UserProfile.Update(UP);
                    break;
            }
            unitOfWork.Save();
        }

        [HttpPost]
        public ActionResult DeleteSavedDevice([DataSourceRequest] DataSourceRequest request, LoggedDevices model)
        {
            if (model!= null)
            {
                var v = Helper.deleteSavedDevice(model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult GetDeviceList([DataSourceRequest] DataSourceRequest request)
        {
            var UserId = (int)Session["UserId"];
            
            var SendData = new ClientDetail()
            {
                ClientDevice = "",
                ClientId = 0,
                ClientIP = "",
                PhoneNumber = "",
                UserName = UserId.ToString()
            };

            var model = Helper.getLoggedDevicesList(SendData);
            return Json( model.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }
    }
}
