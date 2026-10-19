using Domain.Entities.PortfolioAnalyzer;
using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.WebUI.Filters;
using WebUI.Areas.PortfolioAnalyzer.Dto;
using WebUI.Areas.PortfolioAnalyzer.Models;
using Data;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Data;
using PagedList;
using WebUI.Dto;
namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class ApplicationInformationController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/ApplicationInformation/
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult Index(int? page)
        {
            ApplicationInfo input = new ApplicationInfo();
           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            input.AppInfo = PortfolioAnalyzerHelper.GetApplicationInfo(client.ClientId, page);
            return View(input);
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult AppEdit(int ApplicationID, int ClientId)
        {
            var input = PortfolioAnalyzerHelper.GetApplicationInfo1(ApplicationID);
           // ApplicationInfo appinfo = new ApplicationInfo();
           // appinfo.AppInfo = input;
            return View(input);
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult AddApplication()
        {
            return View("AppAdd");
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [HttpPost]
        public ActionResult UpdateAppInfo(ApplicationInformationModel AppInfoInput)
        {
           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            AppInfoInput.ClientId = client.ClientId;
            if (ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdateAppInfoBasic(AppInfoInput);
                TempData[""] = "Application Updated";
            }
            
            ApplicationInfo input = new ApplicationInfo();
            input.ClientId = client.ClientId;
            input.AppInfo = PortfolioAnalyzerHelper.GetApplicationInfo(client.ClientId,1);
            return View("index", input);
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [HttpPost]
        public ActionResult AddNewApp(ApplicationInformationModel AppInfoInput)
        {
           
           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            AppInfoInput.ClientId= client.ClientId;
            if (ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.AddAppInfo(AppInfoInput);
                TempData[""] = "New Application Added";
            }
            ApplicationInfo input = new ApplicationInfo();
            input.ClientId = client.ClientId;
            input.AppInfo = PortfolioAnalyzerHelper.GetApplicationInfo(client.ClientId,1);
            return View("index", input);
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult DeleteApplication(int ApplicationID)
        {

            var res = PortfolioAnalyzerHelper.DeleteApplicationInfo(ApplicationID);
            TempData[""]="Application Deleted";
            ApplicationInfo input = new ApplicationInfo();
           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            
            input.AppInfo = PortfolioAnalyzerHelper.GetApplicationInfo(client.ClientId,1);
            return View("index", input);
        }

    }
}
