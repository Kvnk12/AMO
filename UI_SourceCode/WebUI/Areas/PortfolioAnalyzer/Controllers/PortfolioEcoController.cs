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
    public class PortfolioEcoController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/Portfolio/
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult Index()
        {

            return View();
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetClientEcos()
        {
            if (ViewData["ClientEcoList"] == null)
            {
                //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                ViewData["ClientEcoList"] = EcoSystemList(client.ClientId);
                int ClientId = client.ClientId;
            }
            return Json(ViewData["ClientEcoList"], JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetClientApps()
        {
            if (ViewData["ClientApplicationList"] == null)
            {
              //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                ViewData["ClientApplicationList"] = ApplicationList(client.ClientId);
                int ClientId = client.ClientId;
            }
            return Json(ViewData["ClientApplicationList"], JsonRequestBehavior.AllowGet);
        }


        public IEnumerable<SelectListItem> ApplicationList(int ClientID)
        {

        
            Db db = new Db();
            var item = (from apps in db.Applications
                        where apps.ClientId == ClientID
                        select new
                        {
                            Value = apps.Id,
                            Text = apps.ApplicationName
                        }
                ).AsEnumerable().Select(s => new SelectListItem
                {
                    Value = s.Value.ToString(),
                    Text = s.Text
                }).ToList();
            return item;

        }

        public IEnumerable<SelectListItem> EcoSystemList(int ClientID)
        {


            Db db = new Db();
            var item = (from apps in db.APREcoSystem
                        where apps.ClientId == ClientID
                        select new
                        {
                            Value = apps.Id,
                            Text = apps.EcosystemName
                        }
                ).AsEnumerable().Select(s => new SelectListItem
                {
                    Value = s.Value.ToString(),
                    Text = s.Text
                }).ToList();
            return item;

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
        public ActionResult DestroyPortfolio([DataSourceRequest] DataSourceRequest request, PortfolioModel Portfolio)
        {
            if (Portfolio != null)
            {
                var v = PortfolioAnalyzerHelper.DeletePortfolio(Portfolio.LOBId);
            }

            return Json(new[] { Portfolio }.ToDataSourceResult(request, ModelState));
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetEcosForLOB([DataSourceRequest]DataSourceRequest request, int Id)
        {
          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(PortfolioAnalyzerHelper.GetPortfolioEcoDetails(Id).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateEcoApp(PortfolioEcoModel portfolioEco, int Id)
        {
            if (portfolioEco != null && ModelState.IsValid)
            {
               // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                //portfolio.ClientId = client.Id;
                portfolioEco.PortFolioId = Id;
                var res = PortfolioAnalyzerHelper.CreatePortfolioEco(portfolioEco);
                portfolioEco.LobEcoId = res;
            }
            return Json(new[] { portfolioEco }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingLOBEco([DataSourceRequest] DataSourceRequest request, PortfolioEcoModel portfolioEco)
        {
            if (portfolioEco != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdatePortfolioEco(portfolioEco);
            }

            return Json(new[] { portfolioEco }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteLOBEco([DataSourceRequest] DataSourceRequest request, PortfolioEcoModel portfolioEco)
        {
            if (portfolioEco != null)
            {
                var v = PortfolioAnalyzerHelper.DeletePortfolioEco(portfolioEco.LobEcoId);
            }
            return Json(new[] { portfolioEco }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetAppsbyLOBECOId([DataSourceRequest]DataSourceRequest request, int LobEcoId)
        {
            return Json(PortfolioAnalyzerHelper.GetPortfolioEcoApps(LobEcoId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateAppsByLOBEcoId([DataSourceRequest] DataSourceRequest request, PortfolioEcoAppsModel PEA)
        {
            if (PEA != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdatePortfolioEcoApps(PEA);
            }

            return Json(new[] { PEA }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAppsByLOBEcoId(PortfolioEcoAppsModel PEA, int Id)
        {
            if (PEA != null && ModelState.IsValid)
            {
                //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                //portfolio.ClientId = client.Id;
                PEA.LobEcoId = Id;
                var res = PortfolioAnalyzerHelper.CreatePortfolioEcoApps(PEA);
                PEA.PEAId = res;
            }
            return Json(new[] { PEA }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAppsByLOBEcoId([DataSourceRequest] DataSourceRequest request, PortfolioEcoAppsModel PEA)
        {
            if (PEA != null)
            {
                var v = PortfolioAnalyzerHelper.DeletePortfolioEcoApps(PEA.PEAId);
            }
            return Json(new[] { PEA }.ToDataSourceResult(request, ModelState));
        }
    }
}
