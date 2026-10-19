
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using NTTData.AMOPortal.WebUI.Dto;
using WebMatrix.WebData;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using DotNetOpenAuth.AspNet;
using NTTData.AMOPortal.Service;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Mapper;
using System.Net.Mail;
using System.Web.Configuration;
using System.IO;
//using NTTData.AMOPortal.WebUI.Filters;
using System.Data;
using Data;
using NTTData.AMOPortal.WebUI.Filters;
using System.Net;
using WebUI.Controllers;
using WebUI.Dto;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using WebUI.App_Start;
using log4net;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Okta.AspNet;
using WebUI;
namespace NTTData.AMOPortal.WebUI.Controllers
{
    enum EmailType
    {
        PWDRESET,
        INITIALACCESS,
        ACCESSUPDATE,
        SUCCESSFULUSERCREATION
    }

    public class AccountController : Controller
    {
     //   IAuthProvider authProvider;
        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));
        //public AccountController(IAuthProvider auth)
        //{
        //    authProvider = auth;
        //}


        [Authorize]
        public ActionResult Roles()
        {
            return View();
        }


        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Log.Info("Login attempt starting:" + User.Identity.Name + User.Identity.IsAuthenticated.ToString());
            var IsIN = Session["UserId"] != null ? true : false;
            if (IsIN && User.Identity.IsAuthenticated)
            ///if (User.Identity.IsAuthenticated)
            {
                var userClaims = HttpContext.GetOwinContext().Authentication.User.Claims.ToList();
                string cls = "";
                foreach (var cl in userClaims)
                {
                    cls = cls + "||" + cl.Type + "::" + cl.Value;
                }
                Log.Info(cls);
                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Appstore", "Home");
                else
                    return RedirectToLocal(returnUrl);
            }
            else
            {
                ////WebSecurity.Logout();
                ////Session.Abandon();
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
           //// }
        }

        /// <summary>
        /// temp action just to verify some functionalities.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
       public ActionResult Callback()
        {

            var userClaims = HttpContext.GetOwinContext().Authentication.User.Claims; //.ToList();
            string cls = "";
            foreach (var cl in userClaims)
            {
                cls = cls + "|" + cl.Type + ":" + cl.Value;
            }
            Log.Info(cls);
            return RedirectToAction("Dashboard","Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ActionTrack]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try {

            if (string.IsNullOrEmpty(model.UserName)|| string.IsNullOrEmpty(model.Password))
            {
                Log.Info("Login attempt");
                ModelState.AddModelError("LoginError", "");
                return View(model);
            }
               // ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
                model.UserName = model.UserName.Trim();
                //  if (ModelState.IsValid && authProvider.Authenticate(model.UserName, model.Password, false))

                    //if (!HttpContext.User.Identity.IsAuthenticated && model.UserName.ToLower().Contains("@nttdata.com"))
                    //{
                    //    Log.Debug("before okta login change.");
                    //  //  ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
                    //    HttpContext.GetOwinContext().Authentication.Challenge(
                    //            OktaDefaults.MvcAuthenticationType);
                    //    return new HttpUnauthorizedResult();
                    //}

                    //else if (HttpContext.User.Identity.IsAuthenticated)
                 if (HttpContext.User.Identity.IsAuthenticated)
                {

                    ////var userClaims = HttpContext.GetOwinContext().Authentication.User.Claims.ToList();
                    ////string cls = "";
                    ////foreach (var cl in userClaims)
                    ////{
                    ////    cls = cls + "|"+ cl.Type+":" + cl.Value;
                    ////}
                    ////Log.Info(cls);
                    return SetAfterAuthentication(model,returnUrl);
                }
               
                else if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, false))
            {
                //after mfa
                int i = WebSecurity.GetUserId(model.UserName);
                Session["UserId"] = i;
                //Get Menu Items Access
                //Session["NewClient"] = null;
                Session["NewSessionClient"] = null;
                GetAccessToMenuItems();
                // SetDefaultClientId(i);
                //MFA---
                ///  var Client = (Client)Session["Client"];
                var Client = (ClientDetail)Session["SessionClient"];

              //  Session["NewClient"] = Client;
                Session["NewSessionClient"] = Client;
                 var mfa = GetMFADetails(Client);
                if (mfa.GoodToGo)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    //Save username and password in Session for later Login
                    MFAUserLogin UserLogin = new MFAUserLogin();
                    UserLogin.UserName = model.UserName;
                    UserLogin.Password = model.Password;
                    Session["MFAUserLogin"] = UserLogin;
                    //logout - login after OTP verification
                    WebSecurity.Logout();
                    return RedirectToAction("AuthVerify", new { returnUrl = returnUrl });
                }
            }
           
                ModelState.AddModelError("LoginError", "User Name or Password is incorrect.");
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error in AccountController: Login", ex);
                throw ex;
            }
        }


        private ActionResult  SetAfterAuthentication(LoginModel model, string strUrl)
        {
            
                //after mfa
                int i = WebSecurity.GetUserId(model.UserName);
                Session["UserId"] = i;
                //Get Menu Items Access
                //Session["NewClient"] = null;
                Session["NewSessionClient"] = null;
                GetAccessToMenuItems();
                // SetDefaultClientId(i);
                //MFA---
                ///  var Client = (Client)Session["Client"];
                var Client = (ClientDetail)Session["SessionClient"];

                //  Session["NewClient"] = Client;
                Session["NewSessionClient"] = Client;
                var mfa = GetMFADetails(Client);
                if (mfa.GoodToGo)
                {
                    return RedirectToLocal(strUrl);
                }
                else
                {
                    //Save username and password in Session for later Login
                    MFAUserLogin UserLogin = new MFAUserLogin();
                    UserLogin.UserName = model.UserName;
                    UserLogin.Password = model.Password;
                    Session["MFAUserLogin"] = UserLogin;
                    //logout - login after OTP verification
                    WebSecurity.Logout();
                    return RedirectToAction("AuthVerify", new { returnUrl = strUrl });
                }
 
        }

        [AllowAnonymous]
        public ActionResult AuthVerify(string returnUrl = "")
        {
            Log.Debug("It's in Auth verify url is"+ returnUrl);
            if (Session["MFAModel"] != null)
            {
                var model = (MFAModel)Session["MFAModel"];
                if (model.MFARequired)
                {
                    return View("Login_MFA", model);
                }
                else
                {
                    if (returnUrl.Trim() != "")
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Appstore", "Home");
                    }
                }
            }
            else
            {
                // var Client = (Client)Session["NewClient"];
                var Client = (ClientDetail)Session["NewSessionClient"];
                var mfa = GetMFADetails(Client);
                if (mfa.MFARequired)
                {
                    return View("Login_MFA", mfa);
                }
                else
                {
                    return RedirectToAction("Appstore", "Home");
                }
            }
        }
    
        private MFAModel GetMFADetails(ClientDetail Cl)
        {
            try { 
            MFAModel mode = new MFAModel();
            if (Cl.EnableMFA)
            {
                Db db = new Db();
                DbContextFactory dbcontext = new DbContextFactory();
                Repo<UserProfile> UserProfiles = new Repo<UserProfile>(dbcontext);
                int currentuser = Convert.ToInt32(Session["UserId"]);
                var user = UserProfiles.Get(currentuser);// && m.isDeleted == false); // checking active users with allowed roles.  
                var loginDevice = HttpContext.Request.UserHostName;
                var AnyDevices = db.MFAUserLoggedInDevice
                                        .Where(e => e.ClientId == Cl.ClientId && e.DeviceName == loginDevice
                                        && e.UserId == currentuser && e.IsDeleted == false).Any();
                if (user.Phone != null)
                {
                    var input = user.Phone;
                    mode.PhoneNumber = user.Phone;
                    string masked = user.CountryCode.ToString() + " XXXX XXX-" + input.Substring(input.Length - 3);
                    mode.MphoneNum = masked;
                    mode.CountryCode = user.CountryCode;
                }
                long temp;
                //Set deafult Values
                mode.IsEmailVerfied = true;
                mode.IsPhoneVerfied = true;

                //just check login user
                if (user == null)
                {
                    mode.IsUserNull = true;
                    mode.GoodToGo = false;
                }
                if (!user.IsEmailVerified)
                {
                    mode.IsEmailVerfied = false;
                    mode.GoodToGo = false;
                }
                if (user.Phone == null || !long.TryParse(user.Phone, out temp))
                {
                    mode.IsPhoneNull = true;
                    mode.GoodToGo = false;
                }
                if (!user.IsPhoneVerified)
                {
                    mode.IsPhoneVerfied = false;
                    mode.GoodToGo = false;
                }
                //if (user.Phone != null && user.IsPhoneVerified != false)//true
                //{
                //    mode.GoodToGo= true;
                //}
                if (AnyDevices)
                {
                    mode.NewDevice = false;
                    mode.GoodToGo = true;
                    Session["Authenticated"] = true;
                }
                mode.MFARequired = true;
                return mode;
            }
            else
            {
                mode.GoodToGo = true;
                mode.Authenticated = true;
                mode.MFARequired = false;
                //Session["Authenticated"] = true;
                return mode;
            }
            }
            catch (Exception ex)
            {
                Log.Error("Error in AccountController: Getting MFA Details ", ex);
                throw ex;
            }
        }

        private MFAModel OLD_GetMFADetails(Client Cl)
        {
            MFAModel mode = new MFAModel();
            if (Cl.EnableMFA)
            {
                Db db = new Db();
                DbContextFactory dbcontext = new DbContextFactory();
                Repo<UserProfile> UserProfiles = new Repo<UserProfile>(dbcontext);
                int currentuser = Convert.ToInt32(Session["UserId"]);
                var user = UserProfiles.Get(currentuser);// && m.isDeleted == false); // checking active users with allowed roles.  
                var loginDevice = HttpContext.Request.UserHostName;
                var AnyDevices = db.MFAUserLoggedInDevice
                                        .Where(e => e.ClientId == Cl.Id && e.DeviceName == loginDevice
                                        && e.UserId == currentuser && e.IsDeleted == false).Any();
                long temp;
                //just check login user
                if (user == null)
                {
                    mode.IsUserNull = true;
                    mode.GoodToGo = false;
                }
                //if (!user.IsEmailVerified)
                //{
                //    mode.IsEmailVerfied = false;
                //    mode.GoodToGo = false;
                //}
                if (user.Phone == null || !long.TryParse(user.Phone, out temp))
                {
                    mode.IsPhoneNull = true;
                    mode.GoodToGo = false;
                }
                if (!user.IsPhoneVerified)
                {
                    mode.IsPhoneVerfied = false;
                    mode.GoodToGo = false;
                }
                if (AnyDevices)
                {
                    mode.NewDevice = false;
                    mode.GoodToGo = true;
                }
                return mode;
            }
            else
            {
                mode.GoodToGo = true;
                mode.Authenticated = true;
                return mode;
            }

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult VerifyUserCode(string code = "", bool RememberThisDevice = false)
        {
            var UserId = (int)Session["UserId"];
            if (code.Trim() != "")
            {
                var sessObj = (OTPModel)Session["OTPModel"];
               // var NewClient = (Client)Session["NewClient"];
                var NewClient = (ClientDetail)Session["NewSessionClient"];

                var deviceName = HttpContext.Request.UserHostName;
                var deviceIp = HttpContext.Request.UserHostAddress;
                //Use the Service call and use its data
                var clientData = new ClientDetail()
                {
                    ClientDevice = deviceName,
                    ClientIP = deviceIp,
                    ClientId = NewClient.ClientId,
                    UserName = UserId.ToString()
                };
                MFAOTP otpModel = new MFAOTP()
                {
                    Client = clientData,
                    OTP = code,
                    otpID = sessObj.LogRequestId,
                    OTPType = (int)sessObj.MFAType,
                    RememberDevice = RememberThisDevice
                };
                //POST Request
                string requestUrl = WebConfigurationManager.AppSettings["APIVerifyOTP"].ToString();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MFAOTP));
                MemoryStream mem = new MemoryStream();
                ser.WriteObject(mem, otpModel);
                string dataString = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Encoding = Encoding.UTF8;
                string ronse = webClient.UploadString(requestUrl, "POST", dataString);
                //Parsing JSON string response into model
                var ResModel = JsonConvert.DeserializeObject<MFAOTPStatus>(ronse);
                if (ResModel.Status)
                {
                    Session["Authenticated"] = true;
                    SetUserWorkingClient(NewClient.ClientId);
                    Session["OTPModel"] = null;
                   // Session["NewClient"] = null;
                    Session["NewSessionClient"] = null;

                    var login = (MFAUserLogin)Session["MFAUserLogin"];
                    if (login != null)
                    {
                        WebSecurity.Login(login.UserName, login.Password, false);
                        Session["MFAUserLogin"] = null;
                    }
                    return Json(new { success = "Home/Appstore" });
                }
                //Service call's data
                else
                {
                    if (ResModel.OTPExpired)
                    {
                        return Json(new { expire = ResModel.Message });
                    }
                    else
                    {
                        return Json(new { failure = ResModel.Message });
                    }
                }
            }
            else
            {
                return Json(new { failure = "Wrong!, Password does't match!" });
            }

        }


        public ContentResult SetUserWorkingClient(int ClientId)
        {
            DbContextFactory db = new DbContextFactory();
            Repo<Client> client = new Data.Repo<Client>(db);
            ClientDetail sessionclient = new ClientDetail();

            var Client = client.Get(ClientId);
            sessionclient.ClientId = Client.Id;
            sessionclient.SupportEcoSystem = Client.SupportEcoSystem;
            sessionclient.SubVerticalId = Client.SubVerticalId;
            sessionclient.VerticalId = Client.VerticalId;
            sessionclient.MultipleApprovals = Client.MultipleApprovals;
            sessionclient.ClientName = Client.ClientName;
            //sessionclient.UserName=Client.UserId
            // sessionclient.PhoneNumber=Client.
            //sessionclient.ClientIP=Client
            // sessionclient.ClientDevice=Client

           // Session["NewClient"] = Client;
            Session["NewSessionClient"] = sessionclient;
            Session["Authenticated"] = (bool?)Session["Authenticated"] == null ? false : (bool)Session["Authenticated"];
            //  var mfa = GetMFADetails(Client);
            var mfa = GetMFADetails(sessionclient);
            if (mfa.GoodToGo || (bool)Session["Authenticated"] == true)
            {
               // Session["Client"] = Client;
                Session["SessionClient"] = sessionclient;
                GetAccessToMenuItems();
                var input = (HomeInput)Session["MenuItemsInput"];
                if (input.UserRole.ToLower() != "superuser")
                {
                    input.UserRole = Dto.GetDropDownListItems.UserRoleForClient(input.UserId, ClientId).Text;
                }
                Session["MenuItemsInput"] = input;
                return null;
            }
            else
            {
               // Session["NewClient"] = Client;
                Session["NewSessionClient"] = sessionclient;
                return Content("AuthVerify");
            }
        }


        private void GetAccessToMenuItems()
        {
            try { 
            // UserApplications ua = new UserApplications();
            HomeInput input = new HomeInput();
            DbContextFactory dbcontext = new DbContextFactory();
            Repo<UserProfile> UserProfiles = new Data.Repo<UserProfile>(dbcontext);
            Db db = new Db();
            int UserId = 0;
            string superUserRole = "";
            string userDisplayName = "";
            string userLoginId = "";
            if (Session["UserId"] != null)
            {
                UserId = Convert.ToInt32(Session["UserId"]);
                var userObject = UserProfiles.Get(UserId);
                superUserRole = userObject.RoleId.ToLower();
                userLoginId = userObject.UserName;
                userDisplayName = userObject.FirstName + " " + userObject.LastName;
            }
            else
            {
                Log.Warn("Please login again.");
                throw new Exception("Please login again.");
            }
            input.UserId = UserId;
            input.UserLoginId = userLoginId;
            input.UserDisplayName = userDisplayName;
            var Ulist = (from r in db.UserApplications
                         join sa in db.AppStore on r.ApplicationId equals sa.Id
                         join role in db.UserRoles on r.RoleId equals role.Id
                         where (r.UserId == UserId && r.IsDeleted == false )
                         select new HomeInput.UserApplication {
                             UserId = r.UserId,
                             ClientId = r.ClientId,
                             RoleId = r.RoleId,
                             ApplicationId = r.ApplicationId,
                             ApplicationName = sa.ApplicationName,
                             RoleName=role.RoleName//,
                           // isDeleted = r.isDeleted
                         }).ToList();

            ////var UserApplications = Ulist.Select(e => new HomeInput.UserApplication
            ////{
            ////    UserId = e.UserId,
            ////    ClientId = e.ClientId,
            ////    RoleId = e.RoleId,
            ////    ApplicationId = e.ApplicationId,
            ////    ApplicationName=e.a
            ////    isDeleted = e.isDeleted

            ////}).ToList();

            input.UserApplications = Ulist;
            int userClient = 0;
            if (superUserRole == "superuser")
            {
                input.UserRole = "superuser";
              //  if (Session["Client"] == null)
                    if (Session["SessionClient"] == null)
                    {
                    //Session["Client"] = (from r in db.Clients
                    //                     select r).FirstOrDefault();
                    var Client = (from r in db.Clients
                                  select r).FirstOrDefault();
                   // Session["Client"] = Client;
                  //  Session["SessionClient"] = Client;
                    ClientDetail sessionclient = new ClientDetail();
                    sessionclient.ClientId = Client.Id;
                    sessionclient.SupportEcoSystem = Client.SupportEcoSystem;
                    sessionclient.SubVerticalId = Client.SubVerticalId;
                    sessionclient.VerticalId = Client.VerticalId;
                    sessionclient.MultipleApprovals = Client.MultipleApprovals;
                    sessionclient.ClientName = Client.ClientName;
                    sessionclient.EnableMFA = Client.EnableMFA;
                    Session["SessionClient"] = sessionclient;
                }

                input.PortfolioAnalyzer = true;
                input.BusinessAlignment = true;
                input.SLAManager = true;
                input.ServiceCatalog = true;
                input.IdeaManager = true;
                input.MigrationPlanner = true;
                input.RetirementAccelerator = true;
                input.KnowledgeManager = true;
                input.StaffingModelOptimizer = true;
                input.CloudMigrator = true;
                input.CodeAnalyzer = true;
                input.PerformancePredictor = true;
                input.IncidentNavigator = true;
                input.SourcingeffectivenessEvaluator = true;
                input.ClientProfileManager = true;
				input.TransformationAccelerator = true;
                input.DemandManager = true;
                input.FeedbackManager = true;
                input.AssessmentManager = true;
            }
            else
            {
                if (Ulist.Count() <= 0)
                    throw new Exception("Login User is not authorised to access any of the AM-Optimization tools.");
               // if (Session["Client"] != null)
                    if (Session["SessionClient"] != null)
                    {
                    // var client = (Client)Session["Client"];
                    var client = (ClientDetail)Session["SessionClient"];
                    userClient = client.ClientId;
                    if (superUserRole != "superuser")
                    {
                        input.UserRole = Dto.GetDropDownListItems.UserRoleForClient(UserId, userClient).Text;
                    }
                    var temp = Ulist.Where(r => r.ClientId == userClient);
                    Ulist = temp.ToList();
                }
                else
                {
                    userClient = Ulist.Select(s => s.ClientId).FirstOrDefault();
                    //Session["Client"] = (from r in db.Clients
                    //                     where (r.Id == userClient)
                    //                     select r).FirstOrDefault();
                    var Client = (from r in db.Clients
                                  where (r.Id == userClient)
                                  select r).FirstOrDefault();
                    //Session["Client"] = Client;
                    ClientDetail sessionclient = new ClientDetail();
                    sessionclient.ClientId = Client.Id;
                    sessionclient.SupportEcoSystem = Client.SupportEcoSystem;
                    sessionclient.SubVerticalId = Client.SubVerticalId;
                    sessionclient.VerticalId = Client.VerticalId;
                    sessionclient.MultipleApprovals = Client.MultipleApprovals;
                    sessionclient.ClientName = Client.ClientName;
                    Session["SessionClient"] = sessionclient;

                    if (superUserRole != "superuser")
                    {
                        input.UserRole = Dto.GetDropDownListItems.UserRoleForClient(UserId, userClient).Text;
                    }
                    var temp = Ulist.Where(r => r.ClientId == userClient);
                    Ulist = temp.ToList();
                }

                foreach (var v in Ulist)
                {
                    int Appid = v.ApplicationId;
                    switch (Appid)
                    {
                        case 1:
                            input.PortfolioAnalyzer = true;
                            break;
                        case 2:
                            input.BusinessAlignment = true;
                            break;
                        case 3:
                            input.SLAManager = true;
                            break;
                        case 4:
                            input.ServiceCatalog = true;
                            break;
                        case 5:
                            input.IdeaManager = true;
                            break;
                        case 6:
                            input.MigrationPlanner = true;
                            break;
                        case 7:
                            input.RetirementAccelerator = true;
                            break;
                        case 8:
                            input.KnowledgeManager = true;
                            break;
                        case 9:
                            input.StaffingModelOptimizer = true;
                            break;
                        case 10:
                            input.CloudMigrator = true;
                            break;
                        case 11:
                            input.CodeAnalyzer = true;
                            break;
                        case 12:
                            input.PerformancePredictor = true;
                            break;
                        case 13:
                            input.IncidentNavigator = true;
                            break;
                        case 14:
                            input.SourcingeffectivenessEvaluator = true;
                            break;
                        case 15:
                            input.ClientProfileManager = true;
                            break;
                        case 17:
                            input.TransformationAccelerator = true;
                            break;
                        case 18:
                            input.DemandManager = true;
                            break;
                        case 19:
                            input.FeedbackManager = true;
                            break;
                        case 20:
                            input.AssessmentManager = true;
                            break;
                    }
                }
            }
            Session["MenuItemsInput"] = input;
            }
            catch(Exception ex)
            {
                Log.Error("Error in getting access to Menu Items ",ex);
            }
        }


        //
        // POST: /Account/LogOff

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionTrack]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            string sessionidcookie = System.Configuration.ConfigurationManager.AppSettings["SessionIdCookieName"].ToString();
            Response.Cookies[sessionidcookie].Expires = epoch;
            Response.Cookies.Add(new HttpCookie(sessionidcookie, ""));
            Session.Abandon();
            //if (HttpContext.User.Identity.IsAuthenticated )
            //{
            //    HttpContext.GetOwinContext().Authentication.SignOut(
            //        CookieAuthenticationDefaults.AuthenticationType,
            //        OktaDefaults.MvcAuthenticationType);
            //}


            return RedirectToAction("Login", "Account");
        }

        public ActionResult PostLogout()
        {
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/Register
        // [AllowAnonymous]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, null, null)]
        public ActionResult Register()
        {
            RegisterModel model = new RegisterModel();
            //To Check NTTDataResource [CheckBox] default
            model.NTTDataResource = true;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //IsAlreadySigned
        [HttpPost]
        public JsonResult IsUserAlreadySigned(string UserName)
        {
            return Json(IsUserAvailable(UserName.Trim()));

        }

        [HttpPost]
        public JsonResult IsAlreadySigned(string Email)
        {
            ModelState.Clear();
            return Json(IsUserAvailable(Email.Trim()));

        }

        public JsonResult GetUserByUserName(string ID)
        {
            //WebSecurity.get
            try
            {
                DbContextFactory dbcontext = new DbContextFactory();
                Repo<UserProfile> UserProfiles = new Data.Repo<UserProfile>(dbcontext);

                var user = (from u in UserProfiles
                            where (u.Email.ToUpper() == ID.Trim().ToUpper() || u.UserName.ToLower() == ID.Trim().ToLower())
                            select new RegisterModel
                            {
                                UserName = u.Email,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                CountryCode = u.CountryCode,
                                Phone = u.Phone,
                                JobTitle = u.JobTitle,
                                Id = u.Id,
                                ReportingManager = u.ReportingManager
                            }).FirstOrDefault();
                if (user == null)
                {
                    user = new RegisterModel();
                    user.UserName = "";
                }
                ModelState.Clear();
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Log.Error("Error in getting User Names ", ex);
                throw ex;
            }
        }

        public bool IsUserAvailable(string ID)
        {

            DbContextFactory dbcontext = new DbContextFactory();
            Repo<UserProfile> UserProfiles = new Data.Repo<UserProfile>(dbcontext);

            var RegEmailId = (from u in UserProfiles
                              where (u.Email.ToUpper() == ID.Trim().ToUpper() || u.UserName.ToLower() == ID.Trim().ToLower())
                              select new { ID }).FirstOrDefault();

            bool status;
            if (RegEmailId != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }
            return status;
        }



        [HttpPost]
        // [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, null, null)]
        [ActionTrack]
        public ActionResult RegisterUser(RegisterModel model)
        {
            // bool usrexist=false;
            ////if (ModelState.Select(s => s.Value.ToString().StartsWith("User name already")).Any())
            ////    usrexist = true;
            try
            {
                var uid = (int)Session["UserId"];
                if (ModelState.IsValid)
                {
                    try
                    {
                        // UserRole ur = new UserRole();
                        // Client client = (Client)Session["Client"];

                        ClientDetail client = (ClientDetail)Session["SessionClient"];

                        // string rolesIds = "User";//string.Join(",", model.selectedRoles);
                        //string tempPWD= WebSecurity.
                        //First char of user name with substring till @' and last two char of phone. 
                        string pwdSuffix = "";
                        if (!string.IsNullOrEmpty(model.Phone))
                            pwdSuffix = model.Phone.Substring(model.Phone.Length - 2, 2);
                        else
                            pwdSuffix = "01";
                        model.UserName = model.UserName.Trim();
                        string pwd = model.UserName.Substring(0, 1).ToUpper() + model.UserName.Substring(0, model.UserName.IndexOf("@") + 1) + pwdSuffix;

                        try
                        {
                            model.UserName = model.UserName.Trim();
                            int userid = WebSecurity.GetUserId(model.UserName);
                            if (userid <= 0)
                            {
                                WebSecurity.CreateUserAndAccount(model.UserName, pwd,
                                                    new { FirstName = model.FirstName, LastName = model.LastName, Email = model.UserName, Phone = model.Phone, Jobtitle = model.JobTitle, Active = model.Active, RoleId = "User", ReportingManager = model.ReportingManager, CountryCode = model.CountryCode, Createdby = uid, CreatedDt = DateTime.Now, IsDeleted = false, NTTDataResource = model.NTTDataResource, DefaultClientId = 0 });
                                userid = WebSecurity.GetUserId(model.UserName);
                            }
                            DbContextFactory db = new DbContextFactory();
                            UserApplications ua = new UserApplications();
                            repoUserApplications = new Repo<UserApplications>(db);
                            UserApplicationsservice = new CrudService<UserApplications>(repoUserApplications);
                            ua.ClientId = client.ClientId;

                            ua.IsDeleted = false;
                            ua.RoleId = 1;//user role
                            ua.UserId = userid; // WebSecurity.GetUserId(model.UserName);
                                                //ua.ApplicationId = 15; //client profile manager user
                            ua.ApplicationId = 16; // "Default" application for newly created User
                            if (!UserApplicationsservice.Where(w => w.ApplicationId == 16 && w.ClientId == client.ClientId && w.UserId == ua.UserId && w.RoleId == ua.RoleId).Any())
                            {
                                int ID = UserApplicationsservice.Create(ua);
                            }
                            UserApplicationsservice.Save();
                        }
                        catch (Exception)
                        {
                            model.UserName = model.UserName.Trim();
                            if (!WebSecurity.IsConfirmed(model.UserName) && WebSecurity.UserExists(model.UserName) && WebSecurity.GetCreateDate(model.UserName) <= DateTime.MinValue)
                            {
                                WebSecurity.CreateAccount(model.UserName, pwd);

                                //Adding "Default" applciation to created user
                                DbContextFactory db = new DbContextFactory();
                                UserApplications ua = new UserApplications();
                                //getting UserId
                                int userid = WebSecurity.GetUserId(model.UserName);
                                repoUserApplications = new Repo<UserApplications>(db);
                                UserApplicationsservice = new CrudService<UserApplications>(repoUserApplications);
                                ua.ClientId = client.ClientId;
                                ua.IsDeleted = false;
                                ua.RoleId = 1;//user role
                                ua.UserId = userid; // WebSecurity.GetUserId(model.UserName);
                                                    //ua.ApplicationId = 15; //client profile manager user
                                ua.ApplicationId = 16; // "Default" application for newly created User
                                if (!UserApplicationsservice.Where(w => w.ApplicationId == 16 && w.ClientId == client.ClientId && w.UserId == ua.UserId && w.RoleId == ua.RoleId).Any())
                                {
                                    int ID = UserApplicationsservice.Create(ua);
                                }
                                UserApplicationsservice.Save();

                                //Try method failed - so need to save other user's details entered in form

                                var contextFactory = new DbContextFactory();
                                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                                var up = unitOfWork.UserProfile.GetByID(userid);
                                if (up != null)
                                {
                                    up.FirstName = model.FirstName.Trim();
                                    up.LastName = model.LastName.Trim();
                                    up.Email = model.UserName.Trim();
                                    up.CountryCode = model.CountryCode;
                                    up.Phone = String.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim();
                                    up.JobTitle = String.IsNullOrWhiteSpace(model.JobTitle) ? null : model.JobTitle.Trim();
                                    up.Active = model.Active;
                                    up.RoleId = "User";
                                    up.ReportingManager = String.IsNullOrWhiteSpace(model.ReportingManager) ? null : model.ReportingManager.Trim();
                                    up.IsDeleted = false;
                                    up.NTTDataResource = model.NTTDataResource;
                                    up.DefaultClientId = 0;
                                    up.Createdby = uid;
                                    up.CreatedDt = DateTime.Now;
                                    unitOfWork.UserProfile.Update(up);
                                    unitOfWork.Save();
                                }
                            }
                        }
                        return Json(true, JsonRequestBehavior.AllowGet);
                        // return PartialView("Register", model);
                        //return RedirectToAction("Users", "Account");
                    }
                    catch (MembershipCreateUserException e)
                    {
                        Log.Warn("Error while creating user." + e.InnerException);
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                    return Json(false, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("Users", "Account");
                }
                else
                {
                    // return PartialView("Register", model);
                }

                // If we got this far, something failed, redisplay form
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Log.Error("Error in RegisterUser ", ex);
                throw ex;
            }
        }


        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        [HttpGet]
        [ActionTrack]
        public JsonResult AddDefaultRole(string userName)
        {
            userName = userName.Trim();
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            try
            {

                DbContextFactory db = new DbContextFactory();
                UserApplications ua = new UserApplications();
                repoUserApplications = new Repo<UserApplications>(db);
                UserApplicationsservice = new CrudService<UserApplications>(repoUserApplications);
                ua.ClientId = client.ClientId;
                ua.IsDeleted = false;
                ua.RoleId = 1;//user role
                ua.UserId = WebSecurity.GetUserId(userName);
                //ua.ApplicationId = 15; //client profile manager user
                ua.ApplicationId = 16; // "Default" application for newly created User
                if (!UserApplicationsservice.Where(w => w.ApplicationId == 16 && w.ClientId == client.ClientId && w.UserId == ua.UserId && w.RoleId == ua.RoleId).Any())
                {
                    int ID = UserApplicationsservice.Create(ua);
                }
            }
            catch (Exception ex)
            {
                Log.Error("ignored error 'AddDefaultRole' ", ex);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
            // return RedirectToAction("Users", "Account");
        }

        [Authorize]
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, null, null)]
        public ActionResult UserProfileEdit(int id)
        {
            DbContextFactory db = new DbContextFactory();
            Repo<UserProfile> UserProfiles = new Data.Repo<UserProfile>(db);
            var uid = (int)Session["UserId"];
            UserProfile selectedUser = UserProfiles.Get(id);

            UserProfileModel upm = new UserProfileModel();
            if (selectedUser.Id != 0)
            {
                upm.Id = selectedUser.Id;
                upm.FirstName = selectedUser.FirstName;
                upm.LastName = selectedUser.LastName;
                upm.UserName = selectedUser.UserName;
                //email is user name
                upm.Email = selectedUser.UserName;
                upm.JobTitle = selectedUser.JobTitle;
                upm.Phone = selectedUser.Phone;
                upm.CountryCode = selectedUser.CountryCode.GetValueOrDefault();
                upm.Active = selectedUser.Active;
                upm.ReportingManager = selectedUser.ReportingManager;
                upm.NTTDataResource = selectedUser.NTTDataResource;
                upm.IsPhoneVerified = selectedUser.IsPhoneVerified;
                upm.UpdatedBy = uid;
                upm.UpdatedDt = DateTime.Now;
                //  string[] separators = new string[1];
                // separators[0] = ",";
                // string[] roles = selectedUser.RoleId.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
                // upm.selectedRoles = roles;
                //upm.selectedRoles = Array.ConvertAll(roles, int.Parse);
            }
            return Json(upm, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, null, null)]
        public ActionResult UserProfileSave(UserProfileModel upm)
        {
            try
            {
                UserProfile up = new UserProfile();
                var uid = (int)Session["UserId"];
                DbContextFactory db = new DbContextFactory();
                Repo<UserProfile> UserProfiles = new Data.Repo<UserProfile>(db);
                UserProfile SelectedUser = UserProfiles.Get(upm.Id);

                SelectedUser.FirstName = upm.FirstName.Trim();
                SelectedUser.LastName = upm.LastName.Trim();
                SelectedUser.Phone = upm.Phone;
                SelectedUser.CountryCode = upm.CountryCode;
                //email is user name
                SelectedUser.Email = upm.UserName.Trim();
                SelectedUser.UserName = upm.UserName.Trim();
                SelectedUser.Active = upm.Active;
                SelectedUser.JobTitle = upm.JobTitle;
                // SelectedUser.RoleId = string.Join(",", upm.selectedRoles);
                SelectedUser.IsDeleted = false;
                SelectedUser.ReportingManager = upm.ReportingManager;
                SelectedUser.NTTDataResource = upm.NTTDataResource;
                SelectedUser.UpdatedBy = uid;
                SelectedUser.UpdatedDt = DateTime.Now;
                SelectedUser.IsPhoneVerified = upm.IsPhoneVerified;


                UserProfiles.Save();


                return RedirectToAction("Users");
            }
            catch(Exception ex)
            {
                Log.Error("Error in Saving User Details ", ex);
                throw ex;
            }
        }

        private List<UserProfileModel> FetchUsersForLoginUser(int userid)
        {
            List<UserProfileModel> userProfile = new List<UserProfileModel>();
           // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var paramNameset = new string[] { "userid" };
                var paramValueset = new object[] { userid };
                var res = unitOfWork.GetDataFromStoredProcedure("GETUserProfilesForUser", paramNameset, paramValueset);

                foreach (DataRow dr in res.Rows)
                {
                    var up = new UserProfileModel();
                    up.FirstName = dr["FirstName"].ToString();
                    up.LastName = dr["LastName"].ToString();
                    //email is user name
                    up.Email = dr["UserName"].ToString();
                    up.UserName = dr["UserName"].ToString();
                    up.JobTitle = dr["JobTitle"].ToString();
                    up.Phone = dr["Phone"].ToString();
                    up.Id = Convert.ToInt32(dr["id"]);
                    var IsNttDataResource = Convert.ToBoolean(dr["NTTDataResource"]);
                    if (IsNttDataResource)
                    {
                        up.NTTDataResourceString = "NTT Data";
                    }
                    else
                    {
                        up.NTTDataResourceString = client.ClientName;
                    }
                    userProfile.Add(up);
                }
            }
            return userProfile;
        }

        private List<UserProfileModel> FetchUsersForLoginUser(int userid, int clientid)
        {
            List<UserProfileModel> userProfile = new List<UserProfileModel>();
           // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var paramNameset = new string[] { "userid", "clientid" };
                var paramValueset = new object[] { userid, clientid };
                var res = unitOfWork.GetDataFromStoredProcedure("GETUserProfilesForUserClient", paramNameset, paramValueset);

                foreach (DataRow dr in res.Rows)
                {
                    var up = new UserProfileModel();                   
                        up.FirstName = dr["FirstName"].ToString();
                        up.LastName = dr["LastName"].ToString();
                        //email is user name
                        up.Email = dr["UserName"].ToString();
                        up.UserName = dr["UserName"].ToString();
                        up.JobTitle = dr["JobTitle"].ToString();
                        up.Phone = dr["Phone"].ToString();
                        up.Id = Convert.ToInt32(dr["id"]);
                        var IsNttDataResource = Convert.ToBoolean(dr["NTTDataResource"]);
                        if (IsNttDataResource)
                        {
                            up.NTTDataResourceString = "NTT Data";
                        }
                        else
                        {
                            up.NTTDataResourceString = client.ClientName;
                        }
                        userProfile.Add(up);
                    }
            }
            return userProfile;
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult User_UpdateProfile(UserProfileModel upm)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var usEntity = unitOfWork.UserProfile.GetByID(upm.Id);
            Helper.updateUserDetails(upm);
            //If changed - OTP Verification
            if (upm.Phone == usEntity.Phone || upm.CountryCode == usEntity.CountryCode)
            {

            }
            return View("EditProfile", upm);
        }

        [HttpPost]
        public ActionResult AjaxVerifyPhoneNumber(int Code = 0)
        {
            //var client = (Client)Session["NewClient"];
            var client = (ClientDetail)Session["NewSessionClient"];
            var sessObj = (OTPModel)Session["OTPModel"];
            var UserId = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var deviceName = HttpContext.Request.UserHostName;
            var deviceIp = HttpContext.Request.UserHostAddress;
            if (Code != 0)
            {
                //Use the Service call and use its data
                var clientData = new ClientDetail()
                {
                    ClientDevice = deviceName,
                    ClientIP = deviceIp,
                    ClientId = client.ClientId,
                    UserName = UserId.ToString()
                };
                MFAOTP otpModel = new MFAOTP()
                {
                    Client = clientData,
                    OTP = Code.ToString(),
                    otpID = sessObj.LogRequestId,
                    OTPType = (int)sessObj.MFAType,
                    RememberDevice = false
                };
                //POST Request
                string requestUrl = WebConfigurationManager.AppSettings["APIVerifyOTP"].ToString();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MFAOTP));
                MemoryStream mem = new MemoryStream();
                ser.WriteObject(mem, otpModel);
                string dataString = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Encoding = Encoding.UTF8;
                string ronse = webClient.UploadString(requestUrl, "POST", dataString);
                //Parsing JSON string response into model
                var ResModel = JsonConvert.DeserializeObject<MFAOTPStatus>(ronse);
                if (!ResModel.Status)
                {
                    return Json(new { failure = ResModel.Message });
                }
                else
                {
                    //Save the Number
                    //updateUserPhoneNumber(UserId,)
                    return Json(new { success = ResModel.Message });
                }
            }
            else
            {
                return Json(new { failure = "Invalid Entries" });
            }
        }
        [HttpPost]
        public ActionResult AjaxSendOTP(int CountryCode = 0, string Phone = "")
        {
           // var client = (Client)Session["NewClient"];
            var client = (ClientDetail)Session["NewSessionClient"];
            var UserId = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var deviceName = HttpContext.Request.UserHostName;
            var deviceIp = HttpContext.Request.UserHostAddress;
            if (CountryCode != 0 && Phone.Trim().Length == 10)
            {
                var TempPhone = "+" + CountryCode.ToString() + Phone;
                MFAOTPInput input = new MFAOTPInput()
                {
                    Message = "SMS",
                    Client = new ClientDetail { ClientDevice = deviceName, ClientId = client.ClientId, ClientIP = deviceIp, UserName = UserId.ToString(), PhoneNumber = TempPhone },
                    OTPType = (int)EnumMFAType.sms,
                    RememberDevice = false
                };

                //POST Request
                string requestUrl = WebConfigurationManager.AppSettings["APISendSMS"].ToString();
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
            else
            {
                return Json(new { failure = "Invalid Entries" });
            }
        }
        private void setOTPDetailsInSession(MFAOTPOutput model)
        {
            OTPModel mo = new OTPModel();
            mo.LogRequestId = model.otpID;
            mo.MFAType = (EnumMFAType)model.InputRequest.OTPType;

            Session["OTPModel"] = mo;
        }
        public ActionResult UserEditProfilePage()
        {
            var Uid = (int)Session["UserId"];
            var model = Helper.getUserDetails(Uid);
            return View("EditProfile", model);
        }



        [Authorize]
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, null, null)]
        public ActionResult DeleteUser(int Id)
        {
            try
            {
                DbContextFactory db = new DbContextFactory();
                UserApplications surApp = new UserApplications();
                DelRepo<UserApplications> UsrDet = new Data.DelRepo<UserApplications>(db);
                IEnumerable<UserApplications> users = UsrDet.GetAll().Where(m => m.UserId == Id);
                int icount = 0, jcount = 0;

                foreach (var user in users)
                {
                    if (user.ApplicationId != 16)
                    {
                        icount = user.Id;
                    }
                    else
                    {
                        jcount = user.Id;
                    }
                }
                if (jcount > 0 && icount == 0)
                {
                    Repo<UserApplications> UserApplication = new Data.Repo<UserApplications>(db);

                    UserApplications SelectedApp = UserApplication.Get(jcount);
                    UserApplication.Delete(SelectedApp);
                    UserApplication.Save();
                }
                if (icount > 0)
                {
                    TempData["UserExist"] = "0";
                }
                else
                {
                    TempData["UserExist"] = "1";
                    UserProfile up = new UserProfile();
                    Repo<UserProfile> UserProfiles = new Data.Repo<UserProfile>(db);

                    UserProfile SelectedUser = UserProfiles.Get(Id);

                    string[] userNames = new string[1];
                    string[] roleNames = new string[1];

                    userNames[0] = SelectedUser.UserName;
                    roleNames[0] = SelectedUser.RoleId;


                    if (System.Web.Security.Roles.IsUserInRole(SelectedUser.UserName, SelectedUser.RoleId))
                    {
                        System.Web.Security.Roles.RemoveUsersFromRoles(userNames,
                          roleNames);
                    }
                    UserProfiles.Delete(SelectedUser);
                    UserProfiles.Save();
                }

                return RedirectToAction("Users");
            }
            catch(Exception ex)
            {
                Log.Error("Error in Deleting User ", ex);
                throw ex;
            }
        }

        [Authorize]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, null, null)]
        public ActionResult Users()
        {
            int currentuser = Convert.ToInt32(Session["UserId"]);
            //Client client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            HomeInput input = (HomeInput)Session["MenuItemsInput"];
            // if (input.UserRole.ToLower() == "sysadmin" || input.UserRole.ToLower() == "superuser" || input.UserRole.ToLower() == "admin")
            // if (Helper.HasRole(new string[] { "sysadmin", "admin", "superuser" }))
            if (Helper.HasRole((int)Helper.AMOApplicationName.ClientProfileManager, (int) Helper.AMOUserRoles.Sysadmin))
         
            {
                // List<UserProfileModel> usersModel = FetchUsersForLoginUser(currentuser);
             //   List<UserProfileModel> usersModel = FetchUsersForLoginUser(currentuser, client.ClientId);

                return View();
            }
            else
            {
                return RedirectToAction("Appstore", "Home");
                //return dddRedirectToAction("Users");
            }

        }


        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionTrack]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.PasswordNotMatch ? "Entered password not matches with the confirmed Password"
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionTrack]
        public ActionResult Manage(LocalPasswordModel model)
        {
            try
            {
                bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                ViewBag.HasLocalPassword = hasLocalAccount;
                ViewBag.ReturnUrl = Url.Action("Manage");
                if (hasLocalAccount)
                {
                    if (ModelState.IsValid)
                    {
                        // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                        bool changePasswordSucceeded;
                        try
                        {
                            if (model.NewPassword == model.ConfirmPassword)
                            {
                                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                            }
                            else
                            {
                                return RedirectToAction("Manage", new { Message = ManageMessageId.PasswordNotMatch });
                            }

                        }
                        catch (Exception)
                        {
                            changePasswordSucceeded = false;
                        }

                        if (changePasswordSucceeded)
                        {
                            return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                        }
                        else
                        {
                            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                        }
                    }
                }
                else
                {
                    // User does not have a local password so remove any validation errors caused by a missing
                    // OldPassword field
                    ModelState state = ModelState["OldPassword"];
                    if (state != null)
                    {
                        state.Errors.Clear();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                            return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", e);
                        }
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch(Exception ex)
            {
                Log.Error("Error in changing password ", ex);
                throw ex;
            }
        }
        //Added by Raj 10/10/2017

        [AllowAnonymous]
        public ActionResult ForgotPassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.emailSent ? "Email has Sent to associated email address from your account."

                : "";
            ViewBag.ReturnUrl = Url.Action("ForgotPassword");
            return View();

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserName))
                {
                    ModelState.AddModelError("LoginError", "");
                    return View(model);
                }
                model.UserName = model.UserName.Trim();
                bool isExists = WebSecurity.UserExists(model.UserName);

                if (isExists)
                {
                    if (ModelState.IsValid)
                    {
                        // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                        // bool changePasswordSucceeded;

                        if (!WebSecurity.IsConfirmed(model.UserName) && WebSecurity.GetCreateDate(model.UserName) <= DateTime.MinValue)
                        {
                            WebSecurity.CreateAccount(model.UserName, "Defaultpwd@001");
                        }
                        string token = WebSecurity.GeneratePasswordResetToken(model.UserName, 1440);
                        var resetLink = Url.Action("ResetPassword", "Account", new { token = token }, "http");

                        int userid = WebSecurity.GetUserId(model.UserName);
                        DbContextFactory db = new DbContextFactory();
                        Repo<UserProfile> UserProfiles = new Data.Repo<UserProfile>(db);

                        UserProfile selectedUser = UserProfiles.Get(userid);
                        var emailaddress = selectedUser.Email;
                        //send mail
                        string subject = "Action Required: Reset your password";

                        try
                        {

                            string bodyEmail = PopulateBody(resetLink, EmailType.PWDRESET, selectedUser.FirstName, "", "");
                            Helper.SendEMail(emailaddress, subject, bodyEmail);
                            return RedirectToAction("PasswordSuccess", new { Message = ManageMessageId.emailSent });


                        }
                        catch (Exception ex)
                        {
                            ViewBag.StatusMessage = "Error occured while sending email." + ex.Message;
                        }

                    }
                }
                else
                {

                    ModelState.AddModelError("", "User does not exist");

                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch(Exception ex)
            {
                Log.Error("Error in changing password ", ex);
                throw ex;
            }
        }

        private string PopulateBody(string link, EmailType emailtype, string toUserName, string fromUser, string clientname)
        {

            string body = string.Empty;
            if (emailtype == EmailType.PWDRESET)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/ForgotPasswordEmailTemplate.html")))
                {
                    body = reader.ReadToEnd();
                }
            }
            else if (emailtype == EmailType.INITIALACCESS)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/UserAccessPasswordResetEmailTemplate.html")))
                {
                    body = reader.ReadToEnd();
                }
            }
            else if (emailtype == EmailType.ACCESSUPDATE)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/UserAccessUpdateEmailTemplate.html")))
                {
                    body = reader.ReadToEnd();
                }
            }
            else if (emailtype == EmailType.SUCCESSFULUSERCREATION)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/SuccessfulPwdUpdateEmailTemplate.html")))
                {
                    body = reader.ReadToEnd();
                }

            }
            body = body.Replace("{Link}", link);
            body = body.Replace("{UserName}", toUserName);
            body = body.Replace("{CreatedByUser}", fromUser);
            body = body.Replace("{clientname}", clientname);

            ////body = body.Replace("{Link}", link);
            //string absolutePath = HttpContext.Current.Request.Url.ToString()
            return body;
        }
        //
        // GET: /Account/Manage
        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            try
            {
                Db db = new Db();
                int? userid = WebSecurity.GetUserIdFromPasswordResetToken(token);

                bool any = (from j in db.webpages_Membership
                            where (j.PasswordVerificationToken == token
                            && j.PasswordVerificationTokenExpirationDate >= DateTime.Now)
                            select j).Any();
                if (any == true)
                {
                    ResetPasswordModel model = new ResetPasswordModel() { Token = token };
                }
                else
                {
                    return RedirectToAction("PasswordSuccess", new { Message = ManageMessageId.TokenExpired });
                }
                return View();
            }
            catch(Exception ex)
            {
                Log.Error("Error in Reseting Password ", ex);
                throw ex;
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                string token = model.Token;
                int userid = WebSecurity.GetUserIdFromPasswordResetToken(token);
                Db db = new Db();
                bool any = (from j in db.webpages_Membership
                            where (j.PasswordVerificationToken == token && j.PasswordVerificationTokenExpirationDate >= DateTime.Now)
                            select j).Any();

                if (any)
                {
                    if (ModelState.IsValid)
                    {
                        var userProfile = (from up in db.UserProfiles where up.Id == userid select up).FirstOrDefault();
                        bool changePasswordSucceeded;
                        try
                        {

                            changePasswordSucceeded = WebSecurity.ResetPassword(token, model.NewPassword);

                        }
                        catch (Exception)
                        {
                            changePasswordSucceeded = false;
                        }

                        if (changePasswordSucceeded)
                        {

                            UserPasswordLog u = new UserPasswordLog();

                            DbContextFactory db1 = new DbContextFactory();
                            Repo<UserPasswordLog> UserPasswordLogs = new Data.Repo<UserPasswordLog>(db1);
                            u.UserId = userid;
                            u.PasswordChangedDatetime = DateTime.Now;
                            string IPAddress = IPHelper.GetIPAddress(Request.ServerVariables["HTTP_VIA"],
                                                           Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                                                           Request.ServerVariables["REMOTE_ADDR"]);
                            //     string IP4Address = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                            //Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
                            u.IPAddress = IPAddress;
                            UserPasswordLogs.Insert(u);

                            UserPasswordLogs.Save();
                            var resetLink = Url.Action("Login", "Account", null, "http");
                            string subject = "AM Optimization tool(s) – Password created successfuly";
                            string bodyEmail = PopulateBody(resetLink, EmailType.SUCCESSFULUSERCREATION, userProfile.FirstName, User.Identity.Name, "");
                            Helper.SendEMail(userProfile.Email, subject, bodyEmail);

                            return RedirectToAction("PasswordSuccess", new { Message = ManageMessageId.SetPasswordSuccess });
                        }
                        else
                        {
                            return RedirectToAction("PasswordSuccess", new { Message = ManageMessageId.TokenExpired });
                        }
                    }
                }
                else
                {
                    // User does not have a local password so remove any validation errors caused by a missing
                    // OldPassword field
                    return RedirectToAction("PasswordSuccess", new { Message = ManageMessageId.TokenExpired });

                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch(Exception ex)
            {
                Log.Error("Error in Reseting Password ", ex);
                throw ex;
            }
        }

        //
        // GET: /Account/Manage
        [AllowAnonymous]
        public ActionResult PasswordSuccess(ManageMessageId? message)
        {
            ViewBag.StatusMessage =

            message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
               : message == ManageMessageId.SetPasswordSuccess ? "Your password has been reset."
               : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
               : message == ManageMessageId.emailSent ? "Email has been sent to your email address."
                  : message == ManageMessageId.TokenExpired ? "Token is expired.click on forgot password link from login page."
               : "";

            return View();
        }


        // Add By Ram


        [HttpGet]
        [Authorize]
        [ActionTrack]
        public ActionResult AssignApplication()
        {
            var input = new UserProfileModel();

            return View(input);
        }

        //private UserApplications entityUserApplications;
        private CrudService<UserApplications> UserApplicationsservice;
       // private Mapper<UserApplications, UserProfileModel> UserApplicationscreateMapper;
        private Repo<UserApplications> repoUserApplications;

        [HttpPost]
        [Authorize]
        [ActionTrack]
        public PartialViewResult RemoveUserApplication(int userApplicationId)
        {
            DbContextFactory db = new DbContextFactory();
            repoUserApplications = new Repo<UserApplications>(db);
            int userid = repoUserApplications.Get(userApplicationId).UserId;
            UserApplicationsservice = new CrudService<UserApplications>(repoUserApplications);
            UserApplicationsservice.Delete(userApplicationId);
            //re
            return PartialView("_UserClientsApplicationsRoles", GetAllApplicationForUser(userid));
        }

        public static string GetClientName(int cid)
        {

            string name = "";
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var c = unitOfWork.Clients.GetByID(cid);
                name = c.ClientName;
            }
            return name;
        }

        [HttpPost]
        [Authorize]
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, null, null)]
        public ActionResult AssignApplication(FormCollection frm)
        {
            try
            {
                //  var client = (Client)Session["Client"];
                int usid = (int)Session["UserId"];
                var client = (ClientDetail)Session["SessionClient"];
                // UserProfileModel input = new UserProfileModel();
                var val = frm.GetValue("chkselectList");
                var uid = frm.GetValue("UserID").AttemptedValue.ToString();
                //  var clientid = frm.GetValue("ClientId").AttemptedValue.ToString();
                var roleid = frm.GetValue("RoleId").AttemptedValue.ToString();

                // string ud = uid.AttemptedValue;   
                if (val.AttemptedValue.Length > 0)
                {
                    string[] arr = val.AttemptedValue.Split(",".ToCharArray());
                    DbContextFactory db = new DbContextFactory();
                    UserApplications ua = new UserApplications();
                    ua.UserId = Convert.ToInt32(uid);
                    // ua.ClientId = Convert.ToInt32(clientid);
                    ua.ClientId = client.ClientId;
                    ua.RoleId = Convert.ToInt32(roleid);
                    ua.CreatedBy = usid;
                    ua.CreatedDt = DateTime.Now;
                    repoUserApplications = new Repo<UserApplications>(db);
                    UserApplicationsservice = new CrudService<UserApplications>(repoUserApplications);
                    var uaKeys = UserApplicationsservice.GetAll().Where(w => w.UserId == ua.UserId && w.RoleId == ua.RoleId && w.ClientId == ua.ClientId).Select(s => s.Id).ToList();
                    var existinguserRoleAppCount = UserApplicationsservice.GetAll().Where(w => w.UserId == ua.UserId).Count();
                    //delete previous apps for user
                    foreach (int i in uaKeys)
                    {
                        UserApplicationsservice.Delete(i);
                    }

                    foreach (string i in arr)
                    {
                        ua.ApplicationId = Convert.ToInt32(i);
                        var checkkk = UserApplicationsservice.GetAll().Where(g => g.ApplicationId == ua.ApplicationId && g.UserId == ua.UserId && g.ClientId == ua.ClientId).Any();
                        if (!checkkk)
                        {
                            if (ua.ApplicationId != 20)
                            {
                                var ID = UserApplicationsservice.Create(ua);
                            }
                            else
                            {
                                var ID = UserApplicationsservice.Create(ua);
                                var userapplist = UserApplicationsservice.GetAll().Where(w => w.UserId == ua.UserId && w.ClientId == ua.ClientId && w.IsDeleted == false).ToList();
                                var isim = userapplist.FindAll(x => x.ApplicationId == 5&& x.IsDeleted==false).ToList();
                                if (isim.Count != 0)
                                {
                                    var isuser = isim.FindAll(x => x.RoleId == 1 || x.RoleId==6 &&x.IsDeleted==false).Select(x=>x.Id).ToList();
                                    foreach (var item in isuser)
                                    {
                                        UserApplicationsservice.Delete(item);
                                        ua.ApplicationId = 5;
                                        ua.RoleId = 2;
                                        UserApplicationsservice.Create(ua);
                                        ua.RoleId = Convert.ToInt32(roleid);
                                    }
                                   
                                }
                                else
                                {
                                    ua.ApplicationId = 5;
                                    ua.RoleId = 2;
                                    UserApplicationsservice.Create(ua);
                                }
                                        

                            }

                        }
                       
                    }
                    ////  A3MM - IM integration by DP

                    //var userapplist = UserApplicationsservice.GetAll().Where(w => w.UserId == ua.UserId && w.ClientId == ua.ClientId && w.IsDeleted == false).ToList();
                    //var isa3mm = userapplist.FindAll(x => x.ApplicationId == 20).Any();
                    //var isim = userapplist.FindAll(x => x.ApplicationId == 5).Any();

                    //if (isa3mm)
                    //{
                    //    if (!isim)
                    //    {
                    //        ua.ApplicationId = 5;
                    //        UserApplicationsservice.Create(ua);
                    //    }
                    //}

                    ////  end--by dp

                    Repo<UserProfile> UserProfiles = new Data.Repo<UserProfile>(db);
                    UserProfile selectedUser = UserProfiles.Get(ua.UserId);
                    if (existinguserRoleAppCount <= 0 && arr.Count() > 0)
                    {
                        string token = WebSecurity.GeneratePasswordResetToken(selectedUser.UserName, 1440);
                        var resetLink = Url.Action("ResetPassword", "Account", new { token = token }, "http");
                        string subject = "AM Optimization tool(s) access email verification ";
                        string bodyEmail = PopulateBody(resetLink, EmailType.INITIALACCESS, selectedUser.FirstName, User.Identity.Name, "");
                        Helper.SendEMail(selectedUser.Email, subject, bodyEmail);
                    }
                    else if (arr.Count() > 0)
                    {
                        UserApplicationsDto usrapp = new UserApplicationsDto();
                        //usrapp.ClientName = client.ClientName;
                        //var userApps = new UserApplicationsInput();
                        var clineName = "";
                        if (ua.ClientId != 0)
                        {
                            clineName = GetClientName(ua.ClientId);
                        }
                        var link = "";

                        {
                            //Base url of application.
                            link = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
                        }


                        string subject = "AM Optimization tool(s) access update confirmation";
                        string bodyEmail = PopulateBody(link, EmailType.ACCESSUPDATE, selectedUser.FirstName, User.Identity.Name, clineName);
                        Helper.SendEMail(selectedUser.Email, subject, bodyEmail);
                    }
                }
                return RedirectToAction("AssignUser", new { id = Convert.ToInt32(uid) });
            }
            catch(Exception ex)
            {
                Log.Error("Error in Assigning Application ", ex);
                throw ex;
            }
        }

        //[AllowHtml]
        //[HttpGet]

        public JsonResult GetUserRoles(int clientId)
        {
            var rolList = FetchUserRoles(clientId);
            return Json(rolList, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<SelectListItem> FetchUserRoles(int clientId)
        {
            int currentuser = Convert.ToInt32(Session["UserId"]);
            var input = (HomeInput)Session["MenuItemsInput"];
            int loginUserRoleWeightage = 0;
            if (input.UserRole.ToLower() == "superuser")
            {
                loginUserRoleWeightage = 1000; //higher weight for super user
            }
            else
            {
                Db db = new Db();
                // Queryable Ulist;
                loginUserRoleWeightage = (from uar in db.UserApplications
                                          join r in db.UserRoles on uar.RoleId equals r.Id
                                          join sa in db.AppStore on uar.ApplicationId equals sa.Id
                                          where (uar.UserId == currentuser && uar.ClientId == clientId && sa.IsDeleted == false)
                                          orderby r.Weightage descending
                                          select r.Weightage
                             ).FirstOrDefault();
            }
            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.UserRole> roles = new Data.Repo<Domain.Entities.UserRole>(Context);
            List<NTTData.AMOPortal.Domain.Entities.UserRole> roleList = roles.GetAll().Where(w => w.Weightage <= loginUserRoleWeightage).ToList();

            return roleList.Select(s => new SelectListItem { Text = s.RoleName, Value = s.Id.ToString() });


        }

        public PartialViewResult GetSelectedApps(int ClientId, int RoleId, int UserId)
        {
            Db db = new Db();
            //selected apps for role and client
            int currentuser = Convert.ToInt32(Session["UserId"]);
            var input = (HomeInput)Session["MenuItemsInput"];
            var userAppIds = (from uar in db.UserApplications
                              join sa in db.AppStore on uar.ApplicationId equals sa.Id
                              where (uar.UserId == UserId && uar.RoleId == RoleId && uar.ClientId == ClientId && sa.IsDeleted == false && sa.ApplicationName.ToLower() != "default")
                              select uar.ApplicationId).ToArray();//.ToList();

            //get applications for login users
            //if 
            IOrderedEnumerable<SelectListItem> loginAppList;
            if (input.UserRole.ToLower() == "superuser")
            {
                loginAppList = GetAllApplication().Select
                           (s => new SelectListItem { Text = s.AppName, Value = s.Appid.ToString() }).OrderBy(o => o.Value);
            }
            else
            {
                loginAppList = GetAllApplicationForUser(currentuser).Select
                           (s => new SelectListItem { Text = s.AppName, Value = s.Appid.ToString() }).OrderBy(o => o.Value);
            }
            var userApps = new UserApplicationsInput();
            userApps.ClientId = ClientId;
            userApps.RoleId = RoleId;
            userApps.selectedAppIds = userAppIds;
            userApps.Apps = loginAppList.GroupBy(x => x.Value, (key, group) => group.First()).ToList();
            //roles for lokgin users(weightage less than hieghst role)
            userApps.UserRoles = FetchUserRoles(ClientId).ToList();
            return PartialView("_AppStoreApplications", userApps);
            //eUserProfile.SelectedAppsList = Ulist.ToList();
        }

        private List<UserApplicationsDto> GetAllApplication()
        {
            Db db = new Db();

            return (from sa in db.AppStore
                    where (sa.IsDeleted == false && sa.ApplicationName.ToLower() != "default")
                    select new UserApplicationsDto
                    {
                        Appid = sa.Id,
                        AppName = sa.ApplicationName,
                    }).Distinct().ToList();

        }


        private List<UserApplicationsDto> GetAllApplicationForUser(int userid)
        {
            //applications for a user to display 
         //   Client client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int clientId = client.ClientId;
            Db db = new Db();

            return (from uar in db.UserApplications
                    join r in db.UserRoles on uar.RoleId equals r.Id
                    join sa in db.AppStore on uar.ApplicationId equals sa.Id
                    join cl in db.Clients on uar.ClientId equals cl.Id
                    where (uar.UserId == userid && sa.IsDeleted == false && uar.ClientId == clientId && sa.ApplicationName.ToLower() != "default")
                    select new UserApplicationsDto
                    {
                        Id = uar.Id,
                        UserId = uar.UserId,
                        ClientId = uar.ClientId,
                        ClientName = cl.ClientName,
                        Appid = sa.Id,
                        AppName = sa.ApplicationName,
                        RoleName = r.RoleName,
                        RoleId = r.Id
                    }).Distinct().ToList();

        }

        // [Authorize]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, null, null)]
        public ActionResult AssignUser(int id)
        {
            try
            {
                //  var Clint = (Client)Session["Client"];
                var Clint = (ClientDetail)Session["SessionClient"];
                DbContextFactory Context = new DbContextFactory();
                Mapper<UserProfile, UserProfileModel> mapUserProfile = new Mapper<UserProfile, UserProfileModel>();
                Repo<UserProfile> repoUser = new Repo<UserProfile>(Context);
                CrudService<UserProfile> serviceUser = new CrudService<UserProfile>(repoUser);
                UserProfileModel eUserProfile = mapUserProfile.MapToInput(serviceUser.Get(id));

                Db db = new Db();
                // Queryable Ulist;
                int currentuser = Convert.ToInt32(Session["UserId"]);
                var input = (HomeInput)Session["MenuItemsInput"];
                //  var client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                IOrderedEnumerable<SelectListItem> loginAppList;
                if (input.UserRole.ToLower() == "superuser")
                {
                    loginAppList = GetAllApplication().Select
                               (s => new SelectListItem { Text = s.AppName, Value = s.Appid.ToString() }).OrderBy(o => o.Value);
                }
                else
                {
                    loginAppList = GetAllApplicationForUser(currentuser).Select(
                               s => new SelectListItem
                               {
                                   Text = s.AppName,
                                   Value = s.Appid.ToString()
                               }).OrderBy(s => s.Value);

                }
                eUserProfile.SelectedAppsList = GetAllApplicationForUser(id);
                eUserProfile.SelectedUserApplicationInput = new UserApplicationsInput();
                eUserProfile.SelectedUserApplicationInput.Apps = loginAppList.GroupBy(x => x.Value, (key, group) => group.First()).ToList();

                //eUserProfile.SelectedUserApplicationInput.UserRoles = new List<SelectListItem>();
                eUserProfile.SelectedUserApplicationInput.UserRoles = FetchUserRoles(client.ClientId).ToList();// new List<SelectListItem>();
                eUserProfile.SelectedUserApplicationInput.ClientId = Clint.ClientId;

                return View("AssignUserApplication", eUserProfile);
            }
            catch(Exception ex)
            {
                Log.Error("Error in Assigning Application ", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult UMGetUserList([DataSourceRequest] DataSourceRequest request)
        {
            var client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            return Json(FetchUsersForLoginUser(usid, client.ClientId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        #region The following code commented as not going to be used anytime soon
        ////
        //// POST: /Account/ExternalLogin

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //}

        ////
        //// GET: /Account/ExternalLoginCallback

        //[AllowAnonymous]
        //public ActionResult ExternalLoginCallback(string returnUrl)
        //{
        //    AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //    if (!result.IsSuccessful)
        //    {
        //        return RedirectToAction("ExternalLoginFailure");
        //    }

        //    if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        // If the current user is logged in add the new account
        //        OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        // User is new, ask for their desired membership name
        //        string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
        //        ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
        //        ViewBag.ReturnUrl = returnUrl;
        //        return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        //    }
        //}

        ////
        //// POST: /Account/ExternalLoginConfirmation

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        //{
        //    string provider = null;
        //    string providerUserId = null;

        //    if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
        //    {
        //        return RedirectToAction("Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Insert a new user into the database
        //        using (Db db = new Db())
        //        {
        //            UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
        //            // Check if user already exists
        //            if (user == null)
        //            {
        //                // Insert name into the profile table
        //                db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
        //                db.SaveChanges();

        //                OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
        //                OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

        //                return RedirectToLocal(returnUrl);
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
        //            }
        //        }
        //    }

        //    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        ////
        //// GET: /Account/ExternalLoginFailure

        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        //[AllowAnonymous]
        //[ChildActionOnly]
        //public ActionResult ExternalLoginsList(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        //}

        //[ChildActionOnly]
        //public ActionResult RemoveExternalLogins()
        //{
        //    ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
        //    List<ExternalLogin> externalLogins = new List<ExternalLogin>();
        //    foreach (OAuthAccount account in accounts)
        //    {
        //        AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

        //        externalLogins.Add(new ExternalLogin
        //        {
        //            Provider = account.Provider,
        //            ProviderDisplayName = clientData.DisplayName,
        //            ProviderUserId = account.ProviderUserId,
        //        });
        //    }

        //    ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        //}

        #endregion

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Appstore", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            emailSent,
            TokenExpired,
            PasswordNotMatch
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        //added by Raj 10/11/2017

    }


}
