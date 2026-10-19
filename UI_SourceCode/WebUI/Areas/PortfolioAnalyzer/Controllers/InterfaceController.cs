using Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.PortfolioAnalyzer.Dto;
using WebUI.Areas.PortfolioAnalyzer.Models;
using NTTData.AMOPortal.WebUI.Filters;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class InterfaceController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/Interface/
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult Index()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            AprInterfaceModel apr=InterfaceMethodmetadataList(ClientId);
            return View(apr);
        }
        private AprInterfaceModel InterfaceMethodmetadataList(int ClientID)
        {
            AprInterfaceModel mod=new AprInterfaceModel();
            var ls = new List<SelectListItem>();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var InterfaceMethdList = unitOfWork.APRInterfaceMethod.Get().Where(x => x.ClientId == ClientID).ToList();
            foreach (var item in InterfaceMethdList)
            {
                ls.Add(new SelectListItem() { Text = item.InterfaceMethod, Value = item.Id.ToString() });
            }
            mod.InterfaceMethodList = ls;
            return mod;
        }

        // public int appiddd=1 ;
        //        public IEnumerable<SelectListItem> FilterApplicationList(int ClientID)

        public IEnumerable<SelectListItem> FilterApplicationList1(int apId)
        {

            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.Application.Get(x => x.ClientId == ClientId && x.Id!=apId).ToList();

            var item = applicationList.OrderBy(x => x.Id).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ApplicationName.ToString(), Selected = true });
            return item;

        }


        
        public ActionResult FilterApplicationList(int apId)
        {
            var aplist = FilterApplicationList1(apId);
            return Json(aplist, JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [HttpPost]
        public ActionResult Read1([DataSourceRequest] DataSourceRequest request, int ApplicationID, string interfacetype)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];

           
            return Json(PortfolioAnalyzerHelper.getall(ApplicationID, interfacetype).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, AprInterfaceModel interfacemodel, int ApplicationID, string interfacetype)
        {
          //  var xcxc = PortfolioAnalyzerHelper.getall(ApplicationID, interfacetype);
            if (interfacemodel != null && ModelState.IsValid)
            {
                
                       // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                interfacemodel.AppId = ApplicationID;
                        interfacemodel.InterfaceType = interfacetype;

                        var res = PortfolioAnalyzerHelper.AddInterface(interfacemodel);
                        interfacemodel.Id = res;
                    
                
               
                
                //// interfacemodel.ApplicationInterfacedText=<GetAppNameFor(ApplicationID)> todo to avoid null display
            }
            return Json(new[] { interfacemodel }.ToDataSourceResult(request, ModelState));

        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, AprInterfaceModel interfacemodel)
        {
            if (interfacemodel != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdateInterface(interfacemodel);
            }

            return Json(new[] { interfacemodel }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, AprInterfaceModel interfacemodel)
        {
            if (interfacemodel != null)
            {
                var v = PortfolioAnalyzerHelper.DeleteInterface(interfacemodel.Id);
            }

            return Json(new[] { interfacemodel }.ToDataSourceResult(request, ModelState));
        }

    }
}
