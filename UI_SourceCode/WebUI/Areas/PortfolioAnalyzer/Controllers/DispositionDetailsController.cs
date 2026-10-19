using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using Data;
using System.Collections.Generic;
using System.Web;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Globalization;
using WebUI.Areas.PortfolioAnalyzer.Models;
using Domain.Entities.PortfolioAnalyzer;
using WebUI.Areas.PortfolioAnalyzer.Dto;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Linq;
using System.Web.Mvc;
using NTTData.AMOPortal.WebUI.Filters;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class DispositionDetailsController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/DispositionDetails/
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult Index()
        {
            DispositionDetailsModel Disposition = new DispositionDetailsModel();
            return View("Index", Disposition);
               
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDispositionDetails(int ApplicationID)
        {
            //if (ServiceModel != null && ModelState.IsValid) 
            //{
            Db db = new Db();
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<DispositionDetailsModel, APRAppDisposition>();
                DispositionDetailsModel DispViewModel = new DispositionDetailsModel();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var DispositionInfo = unitOfWork.APRAppDisposition.Get().Where(w=> w.AppId==ApplicationID).FirstOrDefault();
                if (DispositionInfo != null)
                {
                    entityMapper.MapToEntity(DispositionInfo, DispViewModel);
                }
                DispViewModel.AppId = ApplicationID;
                var AutomatedDisp=( from adid in db.APRAppDisposition
                                    join ad in db.APRDisposition on adid.AutomatedDispId equals ad.Id into tempDisposition
                                    join ads in db.APRDispositionSolution on adid.AutomatedDispSolutionId equals ads.Id into tempDisSolution
                                    from Disposition in tempDisposition.DefaultIfEmpty()
                                    from DisSolution in tempDisSolution.DefaultIfEmpty()
                                    where adid.AppId== ApplicationID
                                    select new
                                    {
                                       disposition1=Disposition.DispositionName,
                                       dispostion2= DisSolution.DispositionSolution,

                                    }
                                    ).ToList();
                foreach (var item in AutomatedDisp)
                {
                    DispViewModel.AutomatedDisp = item.disposition1;
                    DispViewModel.AutomatedDispSolution = item.dispostion2;
                }
                var cid = unitOfWork.APRRoadmapTargetApp.Get().Where(e => e.APRDisId == DispViewModel.Id).Select(e => e.APRRoadmapTargetAppId.ToString()).ToArray();
                DispViewModel.RoadmapTargetApp= cid;
                return Json(DispViewModel, JsonRequestBehavior.AllowGet);

            }
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DispositionUpdate(DispositionDetailsModel DispositionInformation)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            DispositionInformation.Analysis = Server.HtmlDecode(DispositionInformation.Analysis);
            DispositionInformation.Recommendation = Server.HtmlDecode(DispositionInformation.Recommendation);

            var res = PortfolioAnalyzerHelper.UpdateDisposition(DispositionInformation, ClientId);

           // return View("Index", DispositionInformation);
            return Json(res, JsonRequestBehavior.AllowGet);
        }



    }

}
