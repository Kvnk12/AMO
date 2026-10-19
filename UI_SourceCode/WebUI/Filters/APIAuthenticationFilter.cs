using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.webap;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Dto;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Security.Principal;
using System.Net.Http;
using System.Net;
using System.Threading;
using WebMatrix.WebData;
using log4net;
namespace NTTData.AMOPortal.WebUI.Filters
{
    public class APIAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger("UserActionTracking");
        public APIAuthenticationAttribute()
        {
            // int currentuser = Convert.ToInt32(httpContext.Session["UserId"]);
        }
        public APIAuthenticationAttribute(int role, int tool)
        {
            this.allowedrole = role;
            this.allowedtool = tool;

        }
        private  int allowedrole;
        private  int allowedtool;
        private  int clientid;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            IEnumerable<string> ApiKeyHeaderValues = null;

            if (actionContext.Request.Headers.TryGetValues("X-AMOClientKey", out ApiKeyHeaderValues))
            {
                string[] apiKeyHeaderValue = ApiKeyHeaderValues.First().Split(':');
                 int.TryParse(apiKeyHeaderValue[0],out clientid);
            }

                if (actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers
                    .Authorization.Parameter;

                // decoding authToken we get decode value in 'Username:Password' format  
                var decodeauthToken = System.Text.Encoding.UTF8.GetString(
                    Convert.FromBase64String(authToken));

                // spliting decodeauthToken using ':'   (UserName and Password)
                var arrCredential = decodeauthToken.Split(':');

                // at 0th postion of array we get username and at 1st we get password  

                if (IsAuthorizedUser(arrCredential[0], arrCredential[1], clientid, this.allowedtool))
                {
                    // setting current principle  
                    Thread.CurrentPrincipal = new GenericPrincipal(
                           new GenericIdentity(arrCredential[0]), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request
                        .CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
            base.OnAuthorization(actionContext);

        }
      

        private bool HasRole(int UserId, int AppId, int RoleId, int clientid)
        {     var db = new Db();
            int isSuperUser = (from u in db.UserProfiles where u.Id == UserId && u.RoleId=="superuser" select u).Count() ;

            if (isSuperUser >= 1) //  "superuser")
                {
                    return true;
                }
           
            var roleweight = (from rl in db.UserRoles where rl.Id == RoleId select rl.Weightage).FirstOrDefault();
            var Ulist = (from r in db.UserApplications
                         join rl in db.UserRoles on r.RoleId equals rl.Id
                         where ((r.ClientId == clientid) &&  (r.UserId == UserId) && (rl.Weightage>= roleweight) &&  (AppId==r.ApplicationId)) 
                         select r).ToList();
            if (Ulist.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public  bool IsAuthorizedUser(string Username, string Password, int ClientId, int AMOToolId)
        {
            try
            {
                // if (WebSecurity.Login("amo", "Ams@1234", false))
                if (WebSecurity.Login(Username, Password, false))
                {
                    //get get user roles for amotools
                    int UserId = WebSecurity.GetUserId(Username);
                    return (HasRole(UserId, AMOToolId, this.allowedrole, ClientId));
                }
                else
                {
                    Log.Info("Attempting unauthorised API access.");
                    return false;
                    // In this method we can handle our database logic here...  
                   // return Username == "nag" && Password == "nag" && AMOToolId == 1 && ClientId == 1;
                
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
    }
}