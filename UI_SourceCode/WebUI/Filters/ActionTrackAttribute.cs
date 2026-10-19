using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using System.Web.Routing;
using NTTData.AMOPortal.WebUI.Dto;
using Data;
using WebUI.Dto;
using log4net;
using NTTData.AMOPortal.Service;

namespace NTTData.AMOPortal.WebUI.Filters
{

    public class ActionTrackAttribute : ActionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger("UserActionTracking");
       
        [Serializable]
        private class ActionTrackingLog
        {
            public string area { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public int? ClientId { get; set; }
            public string sessionId { get; set; }
           
        }
        protected DateTime start_time;
        private static TimeZoneInfo Eastern_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //start_time = DateTime.Now;
            start_time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Eastern_Standard_Time); //EST time
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            RouteData route_data = filterContext.RouteData;
            //route_data.
           // TimeSpan duration = (DateTime.Now - start_time);
            string area = (string)route_data.DataTokens["area"];
            string controller = (string)route_data.Values["controller"];
            string action = (string)route_data.Values["action"];
            string struri = (string)route_data.Values["url"];
            string additionalDetail = "";
            var lstSessionActionTracking = (List<ActionTrackingLog>) HttpContext.Current.Session["SessionActionTracking"];

            if (lstSessionActionTracking==null)
            {
                lstSessionActionTracking = new List<ActionTrackingLog>();
            }
            
           
            if (controller== "PIDataMask" && action== "Download")
            {
                additionalDetail = "File data masking client utility got downloaded.";
            }
            try
            {
                string sessionid = HttpContext.Current.Session.SessionID;
                var userid = HttpContext.Current.Session["UserId"];
                HomeInput MenuItemsInput = (HomeInput)filterContext.HttpContext.Session["MenuItemsInput"];
               // Client client = (Client)HttpContext.Current.Session["Client"];
                ClientDetail client = (ClientDetail)HttpContext.Current.Session["SessionClient"];
                int currentuser = Convert.ToInt32(userid);
                string IPAddress = IPHelper.GetIPAddress(filterContext.HttpContext.Request.ServerVariables["HTTP_VIA"],
                                                           filterContext.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                                                           filterContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"]);
               // string xx=filterContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                if (!lstSessionActionTracking.Where(w => w.sessionId== sessionid && w.ClientId == client.ClientId && w.Controller == controller && w.Action == action).Any())
                  {
                    var actintrackingobj = new ActionTrackingLog();
                    actintrackingobj.sessionId = sessionid;
                    actintrackingobj.ClientId = client.ClientId;
                    actintrackingobj.Controller = controller;
                    actintrackingobj.Action = action;
                    lstSessionActionTracking.Add(actintrackingobj);
                    HttpContext.Current.Session["SessionActionTracking"] = lstSessionActionTracking;
                    using (var contextFactory = new DbContextFactory())
                    {
                        UserActionTracking useractionTracking = new UserActionTracking();
                        useractionTracking.Action = action;
                        useractionTracking.Controller = controller;
                        useractionTracking.ActionStartTime = start_time;
                        //useractionTracking.ActionEndTime = DateTime.Now;
                        useractionTracking.ActionEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Eastern_Standard_Time);
                        useractionTracking.Area = area;
                        useractionTracking.userid = currentuser;
                        if (MenuItemsInput != null)
                        {
                            useractionTracking.userrole = MenuItemsInput.UserRole;
                        }
                        useractionTracking.workingClient = client.ClientId;
                        useractionTracking.AdditionalDetail = additionalDetail;
                        useractionTracking.URLstring = struri;
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        unitOfWork.UserActionTracking.Insert(useractionTracking);
                        unitOfWork.Save();

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            
            
        }
    }
}