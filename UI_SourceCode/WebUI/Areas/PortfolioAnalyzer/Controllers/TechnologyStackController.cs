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
    public class TechnologyStackController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/TechnologyStack/

        public ActionResult Index()
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Countries.Get().Where(x => x.RegionId == 0).OrderBy(m => m.CountryName).ToList();
            var applicationList = unitOfWork.APRHostList.Get().Where(x => x.ClientId == client.ClientId).ToList();
            ViewData["host"] = applicationList.Select(p => new APRHostList { Id = p.Id, HostName = p.HostName.ToString() });
            ViewData["vendor"] = PortfolioAnalyzerHelper.Vendors.Select(p => new Vendor { Id = p.Id, VendorName = p.VendorName.ToString() });

            //var vendorlist = unitOfWork.Vendor.Get().ToList();
            //VendorModel vendors = vendorlist.Select(p => new Vendor { Id = p.Id, VendorName = p.VendorName.ToString() });

            ApplicationInformationModel values = new ApplicationInformationModel()
            {
                //Customers = CustomerRepository.Repository.Customers,
                Vendor = PortfolioAnalyzerHelper.Vendors,
                VendorProduct = PortfolioAnalyzerHelper.VendorProducts,
                VendorProductVersion = PortfolioAnalyzerHelper.VendorProductVersions
                //Products = ProductRepository.Repository.Products
            };
            return View(values);
        }

        public IEnumerable<SelectListItem> AppHostlist()
        {

            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;
           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Countries.Get().Where(x => x.RegionId == 0).OrderBy(m => m.CountryName).ToList();
            var applicationList = unitOfWork.APRHostList.Get().Where(x => x.ClientId == client.ClientId).ToList();
            var item = applicationList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.HostName.ToString() });
            return item;

        }


        [HttpPost]
        public ActionResult GetAppHardwarelist(int ApplicationID, [DataSourceRequest]DataSourceRequest request)
        {
            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<APRAppHardwareModel, APRAppHardware>();
                IList<APRAppHardwareModel> aprAppHardwareModel = new List<APRAppHardwareModel>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var APRAppHardwareDetail = unitOfWork.APRAppHardware.Get().Where(w => w.ApplicationId == ApplicationID);
                if (APRAppHardwareDetail != null)
                {
                    foreach (var h in APRAppHardwareDetail)
                    {
                        APRAppHardwareModel aPRAppHardwareModel = new APRAppHardwareModel();
                        entityMapper.MapToEntity(h, aPRAppHardwareModel);
                        aprAppHardwareModel.Add(aPRAppHardwareModel);
                    }
                }

                return Json(aprAppHardwareModel.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAppHardware([DataSourceRequest] DataSourceRequest request, int ApplicationID, APRAppHardwareModel apphardware)
        {
            if (apphardware != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.InsertAppHardware(apphardware);
                apphardware.Id = res;
            }

            return Json(new[] { apphardware }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult UpdateAppHardware([DataSourceRequest] DataSourceRequest request, APRAppHardwareModel appHardwareData)
        {

            if (appHardwareData != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdateAppHardware(appHardwareData);
            }

            return Json(new[] { appHardwareData }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyAppHardware([DataSourceRequest] DataSourceRequest request, APRAppHardwareModel appHardwareData)
        {
            if (appHardwareData != null)
            {
                var v = PortfolioAnalyzerHelper.DeleteAppHardware(appHardwareData.Id);
            }

            return Json(new[] { appHardwareData }.ToDataSourceResult(request, ModelState));
        }


        public JsonResult GetVendors()
        {
            return Json(PortfolioAnalyzerHelper.Vendors, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<SelectListItem> GetVendorlist()
        {

            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Countries.Get().Where(x => x.RegionId == 0).OrderBy(m => m.CountryName).ToList();
            var applicationList = unitOfWork.Vendor.Get().ToList();
            var item = applicationList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.VendorName.ToString() });
            return item;

        }

        public JsonResult GetVendorProduct(int VendorId)
        {
            return Json(PortfolioAnalyzerHelper.GetVendorProducts(VendorId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVendorProductVersion(int ProductId)
        {
            return Json(PortfolioAnalyzerHelper.GetVendorProductVersions(ProductId), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetAppSoftwarelist(int ApplicationID, [DataSourceRequest]DataSourceRequest request)
        {
            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<APRAppSoftwareModel, APRAppSoftware>();
                IList<APRAppSoftwareModel> aprAppSoftwareModel = new List<APRAppSoftwareModel>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var APRAppSoftwareDetail = unitOfWork.APRAppSoftware.Get().Where(w => w.ApplicationId == ApplicationID);
                if (APRAppSoftwareDetail != null)
                {
                    foreach (var h in APRAppSoftwareDetail)
                    {
                        APRAppSoftwareModel aPRAppSoftwareModel = new APRAppSoftwareModel();
                        entityMapper.MapToEntity(h, aPRAppSoftwareModel);
                        aprAppSoftwareModel.Add(aPRAppSoftwareModel);
                    }
                }

                return Json(aprAppSoftwareModel.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAppSoftware([DataSourceRequest] DataSourceRequest request, int ApplicationID, APRAppSoftwareModel appsoftware)
        {
            if (appsoftware != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.InsertAppSoftware(appsoftware);
                appsoftware.Id = res;
            }

            return Json(new[] { appsoftware }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult UpdateAppSoftware([DataSourceRequest] DataSourceRequest request, APRAppSoftwareModel appSoftwareData)
        {

            if (appSoftwareData != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdateAppSoftware(appSoftwareData);
            }

            return Json(new[] { appSoftwareData }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyAppSoftware([DataSourceRequest] DataSourceRequest request, APRAppSoftwareModel appSoftwareData)
        {
            if (appSoftwareData != null)
            {
                var v = PortfolioAnalyzerHelper.DeleteAppSoftware(appSoftwareData.Id);
            }

            return Json(new[] { appSoftwareData }.ToDataSourceResult(request, ModelState));
        }
    }
}
