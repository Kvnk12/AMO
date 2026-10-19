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
using Domain.Entities.PortfolioAnalyzer;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class MetaDataController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/MetaData/
        [CustomAuthorize(new string[] {"admin", "sysadmin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult Index()
        {
            metadatadto input = new metadatadto();

            input.AppInfo = new List<MetaDataModel>();
            return View(input);
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "admin", "sysadmin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [HttpPost]
        public PartialViewResult ViewMetaData(int MetadataTypeId)
        {
           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var input = PortfolioAnalyzerHelper.GetMetaDataModel(client.ClientId, MetadataTypeId);
            return PartialView("_MetadataList", input);
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "admin", "sysadmin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [HttpPost]
        public PartialViewResult EditMetaData(int MetadataTypeId, int metadataId)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var input = PortfolioAnalyzerHelper.GetMetaDataModel(client.ClientId, MetadataTypeId);
            return PartialView("_MetadataList", input);
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "admin", "sysadmin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [HttpPost]
        public PartialViewResult DeleteMetaData(int MetadataTypeId, int MetaDataItem)
        {
          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.DeleteMeta(MetadataTypeId, MetaDataItem);
                TempData["MetaData"] = "MetaData Deleted";
            }
            var input = PortfolioAnalyzerHelper.GetMetaDataModel(client.ClientId, MetadataTypeId);
            return PartialView("_MetadataList", input);
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "admin", "sysadmin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [HttpPost]
        public PartialViewResult SaveMetaData(int MetadataTypeId, string MetaDataItemText, int? metadataid)
        {
           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            MetaDataModel meta = new MetaDataModel();
            if (ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.SaveMeta(MetadataTypeId, MetaDataItemText, metadataid, client.ClientId);
                TempData["MetaData"] = "MetaData Saved";
            }
            return PartialView("_MetadataList", PortfolioAnalyzerHelper.GetMetaDataModel(client.ClientId, MetadataTypeId));

        }


        [CustomAuthorize(new string[] { "admin", "sysadmin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult TechnologyStackMetadata(APRHostlistModel h)
        {
            
            return View(h);
        }

        [CustomAuthorize(new string[] { "admin", "sysadmin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult VendorProductVersionMetadata()
        {

            return View();
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetVendorlist([DataSourceRequest]DataSourceRequest request)
        {

           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            //if (ViewData["ClientApplicationList"] == null)
            //    ViewData["ClientApplicationList"] = ApplicationList(client.Id);
            return Json(PortfolioAnalyzerHelper.GetVendorlist().AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            // return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateVendor([DataSourceRequest]DataSourceRequest request, VendorMetadataModel vendor)
        {
            if (vendor != null && ModelState.IsValid)
            {
               
                var res = PortfolioAnalyzerHelper.CreateVendorlist(vendor);
                vendor.VId = res;
            }
            return Json(new[] { vendor }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateVendor([DataSourceRequest] DataSourceRequest request, VendorMetadataModel Vendor)
        {
            if (Vendor != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdateVendor(Vendor);
            }

            return Json(new[] { Vendor }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyVendor([DataSourceRequest] DataSourceRequest request, VendorMetadataModel Vendor)
        {
            if (Vendor != null)
            {
                var v = PortfolioAnalyzerHelper.DeleteVendor(Vendor.VId);
            }

            return Json(new[] { Vendor }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetProductsByVendor([DataSourceRequest]DataSourceRequest request, int Id)
        {
            return Json(PortfolioAnalyzerHelper.GetVendorProducts(Id).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateProduct(VendorProductModel product, int id)
        {
            if (product != null && ModelState.IsValid)
            {
                //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                //portfolio.ClientId = client.Id;
                product.VendorId = id;
                var res = PortfolioAnalyzerHelper.CreateProduct(product);
                product.ProductId= res;
            }
            return Json(new[] { product }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateProduct([DataSourceRequest] DataSourceRequest request, VendorProductModel product)
        {
            if (product != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdateProduct(product);
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteProduct([DataSourceRequest] DataSourceRequest request, VendorProductModel product)
        {
            if (product != null)
            {
                var v = PortfolioAnalyzerHelper.DeleteProduct(product.ProductId);
            }
            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetVersionsByProduct([DataSourceRequest]DataSourceRequest request, int productid)
        {
            return Json(PortfolioAnalyzerHelper.GetVendorProductVersions(productid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateProductVersion(VendorProductVersionModel ver, int id)
        {
            if (ver != null && ModelState.IsValid)
            {
                //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                //portfolio.ClientId = client.Id;
                ver.ProductId =id;
                var res = PortfolioAnalyzerHelper.CreateProductVerion(ver);
                ver.VersionId = res;
            }
            return Json(new[] { ver }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateProductVersion([DataSourceRequest] DataSourceRequest request, VendorProductVersionModel ver)
        {
            if (ver != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdateProductVersion(ver);
            }

            return Json(new[] { ver }.ToDataSourceResult(request, ModelState));
        }

        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteProductVersion([DataSourceRequest] DataSourceRequest request, VendorProductVersionModel ver)
        {
            if (ver != null)
            {
                var v = PortfolioAnalyzerHelper.DeleteProductVersion(ver.VersionId);
            }
            return Json(new[] { ver }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult SolutionIndex()
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var slectList = PortfolioAnalyzerHelper.dispSolutionlist(client.ClientId);
            ViewData["DispSolutions"] = slectList;
            return View("DispositionSolution");
        }



        /// <summary>
        /// main(parent) Table - disposition
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDispositions([DataSourceRequest] DataSourceRequest request)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var model = PortfolioAnalyzerHelper.readDisposition(client.ClientId);
            return Json( model.ToDataSourceResult(request, ModelState));
        }

       
        [HttpPost]
        public ActionResult GetDispSolutionsForDisp([DataSourceRequest] DataSourceRequest request, int DispositionId = 0)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            return Json(PortfolioAnalyzerHelper.dispsolutionLinkRead(DispositionId, client.ClientId).ToDataSourceResult(request),JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult CreateDispSolutionSln([DataSourceRequest] DataSourceRequest request, DispNDispSlnModel model, int DispositionId=0)
        {
           //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (model!=null && DispositionId != 0)
            {
                model.ClientId = client.ClientId;
                model.DispId = DispositionId;
                
                var i = PortfolioAnalyzerHelper.dispsolutionLinkCreate(model);
                model.MapId = i;

            }
            return Json(new[] { model}.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult EditDispSolution([DataSourceRequest] DataSourceRequest request, DispNDispSlnModel model)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (model != null)
            {
                PortfolioAnalyzerHelper.dispsolutionLinkUpdate(model);
            }
            return Json(new[] {model}.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult DeleteDispSolution([DataSourceRequest] DataSourceRequest request, DispNDispSlnModel model)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            PortfolioAnalyzerHelper.dispsolutionLinkDelete(model.MapId);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }







    }
}

