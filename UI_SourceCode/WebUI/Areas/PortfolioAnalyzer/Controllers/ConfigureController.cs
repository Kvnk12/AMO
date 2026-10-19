using Data;
using Domain.Entities.PortfolioAnalyzer;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Filters;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.PortfolioAnalyzer.Models;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class ConfigureController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/Configure/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult EcoSystem()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult GetEcosystems([DataSourceRequest]DataSourceRequest request)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<APREcoSystemModel, APREcoSystem>();
                IList<APREcoSystemModel> aprEcosystemModelList = new List<APREcoSystemModel>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var APREcosystemDetails = unitOfWork.APREcoSystem.Get().Where(w => w.ClientId == ClientId);
                if (APREcosystemDetails != null)
                {
                    foreach (var ecosystem in APREcosystemDetails)
                    {
                        APREcoSystemModel ecosystemModel = new APREcoSystemModel();
                        entityMapper.MapToEntity(ecosystem, ecosystemModel);
                        aprEcosystemModelList.Add(ecosystemModel);
                    }
                }



                return Json(aprEcosystemModelList.ToList().ToList().ToDataSourceResult(request));

            }
        }


        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateEcosystem([DataSourceRequest] DataSourceRequest request,  APREcoSystemModel ecosystem)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ecosystem.ClientId = client.ClientId;
            if (ecosystem != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.InsertEcosystem(ecosystem);
                ecosystem.Id = res;
            }

            return Json(new[] { ecosystem }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult UpdateEcosystem([DataSourceRequest] DataSourceRequest request, APREcoSystemModel ecosystem)
        {

           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ecosystem.ClientId = client.ClientId;
            if (ecosystem != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdateEcosystem(ecosystem);
              
            }

            return Json(new[] { ecosystem }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyEcosystem([DataSourceRequest] DataSourceRequest request, APREcoSystemModel ecosystem)
        {
            if (ecosystem != null)
            {
                var v = PortfolioAnalyzerHelper.DeleteEcosystem(ecosystem.Id);
            }

            return Json(new[] { ecosystem }.ToDataSourceResult(request, ModelState));
        }

    }
}