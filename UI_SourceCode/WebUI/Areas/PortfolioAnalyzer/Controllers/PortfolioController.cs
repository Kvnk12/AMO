using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using WebUI.Areas.PortfolioAnalyzer.Models;
using WebUI.Areas.PortfolioAnalyzer.Controllers;
using NTTData.AMOPortal.Data;
using Data;
using WebUI.Areas.PortfolioAnalyzer.Dto;
using NTTData.AMOPortal.WebUI.Filters;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class PortfolioController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/Portfolio/
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult Index()
        {

            return View();
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetClientApps()
        {
            if (ViewData["ClientApplicationList"] == null)
            {
               // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                ViewData["ClientApplicationList"] = ApplicationList(client.ClientId);
                int ClientId = client.ClientId;
            }
            return Json(ViewData["ClientApplicationList"], JsonRequestBehavior.AllowGet);
        }

       
        public IEnumerable<SelectListItem> ApplicationList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            //var contextFactory = new DbContextFactory();
            //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Application.Get(x => x.ClientId == ClientID).ToList();
            //var item = applicationList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ApplicationName.ToString(), Selected = true });
            Db db = new Db();
            var item = (from apps in db.Applications
                        where apps.ClientId == ClientID
                        select new
                        {
                            Value = apps.Id,
                            Text = apps.ApplicationName
                        }
                ).AsEnumerable().Select( s=> new SelectListItem
                {
                    Value = s.Value.ToString(),
                    Text = s.Text
                }).ToList();
            return item;

        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult PortfolioInfo([DataSourceRequest]DataSourceRequest request)
        {

          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ViewData["ClientApplicationList"]==null)
                ViewData["ClientApplicationList"] = ApplicationList(client.ClientId);
            return Json(PortfolioAnalyzerHelper.GetPortfolioDetails(client.ClientId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

           // return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreatePortfolio([DataSourceRequest]DataSourceRequest request, PortfolioModel portfolio)
        {
            if (portfolio != null && ModelState.IsValid)
            {
                //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                portfolio.ClientId = client.ClientId;
                var res = PortfolioAnalyzerHelper.CreatePortfolioDetails(portfolio);
                portfolio.LOBId = res;
            }
            return Json(new[] { portfolio }.ToDataSourceResult(request, ModelState));
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateLOBApp( PortfolioAppModel portfolio, int Id)
        {
            if (portfolio != null && ModelState.IsValid)
            {
                //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                //portfolio.ClientId = client.Id;
                portfolio.PortFolioId = Id;
                var res = PortfolioAnalyzerHelper.CreatePortfolioApps(portfolio);
                portfolio.LobAppId = res;
            }
            return Json(new[] { portfolio }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePortfolio([DataSourceRequest] DataSourceRequest request, PortfolioModel Portfolio)
        {
            if (Portfolio != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdatePortfolioInfo(Portfolio);
            }

            return Json(new[] { Portfolio }.ToDataSourceResult(request, ModelState));
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingLOBApp([DataSourceRequest] DataSourceRequest request, PortfolioAppModel PortfolioApp)
        {
            if (PortfolioApp != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdatePortfolioApps(PortfolioApp);
            }

            return Json(new[] { PortfolioApp }.ToDataSourceResult(request, ModelState));
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyPortfolio([DataSourceRequest] DataSourceRequest request, PortfolioModel Portfolio)
        {
            if (Portfolio != null)
            {
                var v = PortfolioAnalyzerHelper.DeletePortfolio(Portfolio.LOBId);
            }

            return Json(new[] { Portfolio }.ToDataSourceResult(request, ModelState));
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteLOBApp([DataSourceRequest] DataSourceRequest request, PortfolioAppModel PortfolioApp)
        {
            if (PortfolioApp != null)
            {
                var v = PortfolioAnalyzerHelper.DeletePortfolioApp(PortfolioApp.LobAppId);
            }
            return Json(new[] { PortfolioApp }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetAppsForLOB([DataSourceRequest]DataSourceRequest request, int Id)
        {
          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(PortfolioAnalyzerHelper.GetPortfolioAppDetails(Id).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [HttpGet]
        public ActionResult applicationDetails([DataSourceRequest] DataSourceRequest request, int appid)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationDetails = unitOfWork.Application.GetByID(appid);
            var iten = new appinfo();
            iten._appaliasname = applicationDetails.AliasName;
            iten._clientappno = applicationDetails.ClientApplicationNumber;
            return Json(iten, JsonRequestBehavior.AllowGet);
        }


    }
}
