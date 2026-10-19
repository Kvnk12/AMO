using AutoMapper;
using Domain.Entities.ClientProfileManager;
using DotNetOpenAuth.AspNet;
using Elmah;
using Microsoft.Web.WebPages.OAuth;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.Service;
using NTTData.AMOPortal.WebUI.Dto;
//using NTTData.AMOPortal.WebUI.Infrastructure;
using NTTData.AMOPortal.WebUI.Mapper;
using RouteDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;
using WebUI.Areas.ClientProfileManager.ViewModel;
using log4net;
using System.Web.Helpers;
using System.Security.Claims;
using NTTData.AMOPortal.WebUI.Infrastructure;

namespace WebUI
{
    
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
           WebSecurity.InitializeDatabaseConnection("Db", "UserProfile", "Id", "UserName", autoCreateTables: false);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            log4net.Config.XmlConfigurator.Configure();


            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);


            BundleConfig.RegisterBundles(BundleTable.Bundles);

          ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            Mapper.CreateMap<PostIdeaInput, IMPostIdea>();
          //  Mapper.CreateMap<PostChallengeInput, IMPostChallenge>();
        }
        // ELMAH Filtering
        protected void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            FilterError404(e);
        }

        protected void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            FilterError404(e);
        }

        // Dismiss 404 errors for ELMAH
        private void FilterError404(ExceptionFilterEventArgs e)
        {
            if (e.Exception.GetBaseException() is HttpException)
            {
                HttpException ex = (HttpException)e.Exception.GetBaseException();
                if (ex.GetHttpCode() == 404)
                    e.Dismiss();
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");
        }
    }
}