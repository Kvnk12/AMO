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
    
    public class DataCollectionController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/DataCollection/
        [HttpGet]
        public ActionResult Index()
        {
            GetPersonnelList();
            return View();
        }
        [ValidateAntiForgeryToken]
        public ActionResult IndexAction()
        {
            return null;
        }
        public ActionResult Financial()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(ApplicationInformationModel model)
        //{
        //    return View(model);
        //}
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetApplicationinfoItem(int ApplicationID)
        {
            //if (ServiceModel != null && ModelState.IsValid) 
            //{
            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<ApplicationInformationModel, Application>();
                ApplicationInformationModel AppViewModel = new ApplicationInformationModel();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var ApplicationInfo = unitOfWork.Application.GetByID(Convert.ToInt32(ApplicationID));
                if (ApplicationInfo != null)
                {

                    entityMapper.MapToEntity(ApplicationInfo, AppViewModel);
                }
                var cid = unitOfWork.APRApplicationCountry.Get().Where(e => e.AppId == AppViewModel.Id).Select(e => e.CountryId.ToString()).ToArray();
                AppViewModel.CountriesId = cid;

                //    return Json(ServiceModel, JsonRequestBehavior.AllowGet);

                // ApplicationInfo appinfo = new ApplicationInfo();
                // appinfo.AppInfo = input;_DemographicTabs
                // return PartialView("_DemographicTabs", AppViewModel);
                return Json(AppViewModel, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult GetApplicationinFinanceDetail(int ApplicationID, [DataSourceRequest]DataSourceRequest request)
        {
            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<APRFinancialModel, APRAppFinancial>();
                IList<APRFinancialModel> aprFinDetailListViewModel = new List<APRFinancialModel>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var APRFinancialInfoDetail = unitOfWork.APRAppFinancial.Get().Where(w => w.AppId == ApplicationID);
                if (APRFinancialInfoDetail != null)
                {
                    foreach (var financialentity in APRFinancialInfoDetail)
                    {
                        APRFinancialModel aprFinViewModel = new APRFinancialModel();
                        entityMapper.MapToEntity(financialentity, aprFinViewModel);
                        aprFinDetailListViewModel.Add(aprFinViewModel);
                    }
                }

                return Json(aprFinDetailListViewModel.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateFinancial([DataSourceRequest] DataSourceRequest request, int AppId, APRFinancialModel product)
        {
            if (product != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.InsertAppFinanceInfo(product);
                product.Id = res;
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult UpdateApplicationinFinanceDetail(APRFinancialModel appFinancialData)
        {

            // PortfolioAnalyzerHelper.ClientApplicationNumber();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyFinancial([DataSourceRequest] DataSourceRequest request, APRFinancialModel product)
        {
            if (product != null)
            {
                var v = PortfolioAnalyzerHelper.DeleteAppFinancial(product.Id);
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }


        public void GetPersonnelList()
        {


            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var PersonnelList = unitOfWork.Personnel.Get().ToList();
            var list = PersonnelList.ToList().Select(A => new { PersonnelId = A.Id.ToString(), EmailAddress = A.EmailAddress });

            ViewData["Personnel"] = list;
        }

        //dropdown values for perspective  type
        public IEnumerable<SelectListItem> PerspectiveCategory(int ClientID)
        {
           

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
           
            var perspectCat = unitOfWork.PerspectiveCategory.Get().Where(x => x.ClientId == ClientID &&x.ToolId==1).ToList();
            var item = perspectCat.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.PerspectiveCategoryName.ToString() });
      
            return ALLItem.Concat(item);
        }
        public IEnumerable<SelectListItem> ALLItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "BOTH",
                    Text = "All"
                }, count: 1);
            }
        }


        public ActionResult GetPerspectiveCategory()
        {
            if (ViewData["PerspectiveCategory"] == null)
            {
               // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                ViewData["PerspectiveCategory"] = PerspectiveCategory(client.ClientId);
                int ClientId = client.ClientId;
            }
            return Json(ViewData["PerspectiveCategory"], JsonRequestBehavior.AllowGet);
        }
    }


}
