using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using Data;
using WebUI.Dto;

namespace NTTData.AMOPortal.WebUI.Filters
{
    public class IdeaManagerAuthorizeAttribute : AuthorizeAttribute
    {
        public IdeaManagerAuthorizeAttribute()
        {

        }

        //private readonly string[] allowedroles;
        //private readonly string[] allowedtools;
        //private readonly string[] allowedclients;
        private readonly int IdeaId;

        public IdeaManagerAuthorizeAttribute(int IdeaId)
        {
            this.IdeaId = IdeaId;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
          //  var Client = (Client)httpContext.Session["Client"];
            ClientDetail Client = (ClientDetail)HttpContext.Current.Session["SessionClient"];
            DbContextFactory dbcontext = new DbContextFactory();
            Repo<UserProfile> UserProfiles = new Data.Repo<UserProfile>(dbcontext);
            int currentuser = Convert.ToInt32(httpContext.Session["UserId"]);
            bool authorize = false;

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                authorize = unitOfWork.IMPostIdea.Get().Where(f => f.ClientId == Client.ClientId && f.Id == this.IdeaId).Any();
            }
            
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}