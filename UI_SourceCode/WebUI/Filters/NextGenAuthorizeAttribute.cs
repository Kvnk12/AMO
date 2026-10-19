using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Dto;

namespace NTTData.AMOPortal.WebUI.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute()
        {
           // int currentuser = Convert.ToInt32(httpContext.Session["UserId"]);
        }
        private readonly string[] allowedroles;
        private readonly string[] allowedtools;
        private readonly string[] allowedclients;

        private readonly int[] allowedroleids;
        private readonly int[] allowedtoolids;
        public CustomAuthorizeAttribute(string[] roles, string[] tools, string[] clients)
        {
            this.allowedroles = roles;
            this.allowedtools = tools;
            this.allowedclients = clients;
        }

        public CustomAuthorizeAttribute(int[] roleids, int[] toolids)
        {
            this.allowedroleids = roleids;
            this.allowedtoolids = toolids;

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            int currentuser =Convert.ToInt32(httpContext.Session["UserId"]);

            //just check login user
            if (currentuser == 0)
            {
                return false;
            }

            //just check login user
            if(allowedroles==null && allowedtools==null && allowedclients==null && allowedroleids==null && allowedtoolids==null)
            {
                return true;
            }

            if (allowedtools!= null || allowedroles!=null)
                return HasRole(currentuser, allowedtools, allowedroles);
            else
                return HasRole(currentuser, allowedtoolids,allowedroleids);

            ////if (allowedclients != null)
            ////{
            ////    var userAppList = Ulist.Where(w => allowedclients.Where(s => s.ToLower() == w.ClientName.ToLower()).Count() > 0).ToList();
            ////    if (userAppList == null || userAppList.Count() <= 0)
            ////    {
            ////        authorize = false;
            ////    }
            ////}



            ////return authorize;
        }

        private  bool HasRole(int UserId,  string[] AppNames, string[] RoleNames)
        {
            ClientDetail client =(ClientDetail) HttpContext.Current.Session["SessionClient"];
            var input = (WebUI.Dto.HomeInput)HttpContext.Current.Session["MenuItemsInput"];
            if (UserId != 0)
            {

                if (input.UserRole.ToLower() == "superuser")
                {
                    return true;
                }
            }
            List<Dto.HomeInput.UserApplication> Ulist;
           if (AppNames==null)
              Ulist = (from r in input.UserApplications
                         where (r.ClientId == client.ClientId && r.UserId == UserId && RoleNames.Contains(r.RoleName.ToLower()))
                         select r).ToList();
           else if (RoleNames==null)
                Ulist = (from r in input.UserApplications
                         where (r.ClientId == client.ClientId && AppNames.Contains(r.ApplicationName.Trim()) && r.UserId == UserId )
                         select r).ToList();
            else
                Ulist = (from r in input.UserApplications
                         where (r.ClientId == client.ClientId && AppNames.Contains(r.ApplicationName.Trim()) && r.UserId == UserId && RoleNames.Contains(r.RoleName.ToLower()))
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


        private bool HasRole(int UserId, int[] AppIds, int[] RoleIds)
        {
            ClientDetail client = (ClientDetail)HttpContext.Current.Session["SessionClient"];
            var input = (WebUI.Dto.HomeInput)HttpContext.Current.Session["MenuItemsInput"];
            if (UserId != 0)
            {

                if (input.UserRole.ToLower() == "superuser")
                {
                    return true;
                }
            }

            var Ulist = (from r in input.UserApplications
                         where ((r.ClientId == client.ClientId) &&  (r.UserId == UserId) && (RoleIds.Contains(r.RoleId)) &&  (AppIds.Contains(r.ApplicationId)) )
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

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}