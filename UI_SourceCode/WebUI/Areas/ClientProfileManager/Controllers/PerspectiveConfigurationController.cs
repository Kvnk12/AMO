using Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.Dto;
using WebUI.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class PerspectiveConfigurationController : Controller
    {
        //
        // GET: /ClientProfileManager/PerspectiveConfiguration/

        public ActionResult Index()
        {
            return View("Index");
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult ReadPerspectiveCategory([DataSourceRequest]DataSourceRequest request,int ToolNo=0)
        {

            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(ClientProfileManagerHelper.GetPrespectiveCategory(client.ClientId, ToolNo).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            // return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreatePerspectiveCategory([DataSourceRequest]DataSourceRequest request, PerspectiveCategoryModel perspectivecategorymodel,int ToolNo = 0)
        {
            if (perspectivecategorymodel != null && perspectivecategorymodel.PerspectiveCategoryName.Trim() != "" && ModelState.IsValid)
            {
                // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                perspectivecategorymodel.ClientId = client.ClientId;
                perspectivecategorymodel.ToolId = ToolNo;
                var res = ClientProfileManagerHelper.AddPerspectiveCategory(perspectivecategorymodel);
                perspectivecategorymodel.Id = res;
            }
            return Json(new[] { perspectivecategorymodel }.ToDataSourceResult(request, ModelState));
        }

        
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePerspectiveCategory([DataSourceRequest] DataSourceRequest request, PerspectiveCategoryModel perspectivecategorymodel)
        {
            if (perspectivecategorymodel != null && perspectivecategorymodel.PerspectiveCategoryName.Trim() != "" && ModelState.IsValid)
            {
                var res = ClientProfileManagerHelper.EditPerspectiveCategory(perspectivecategorymodel);
            }

            return Json(new[] { perspectivecategorymodel }.ToDataSourceResult(request, ModelState));
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletePerspectiveCategory([DataSourceRequest] DataSourceRequest request, PerspectiveCategoryModel perspectivecategorymodel)
        {
            if (perspectivecategorymodel != null)
            {
                ClientProfileManagerHelper.deleteAllPerspectives(perspectivecategorymodel.Id);
                var v = ClientProfileManagerHelper.DestroyPerspectiveCategory(perspectivecategorymodel.Id);
            }

            return Json(new[] { perspectivecategorymodel }.ToDataSourceResult(request, ModelState));
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult ReadSurveyPerspective([DataSourceRequest]DataSourceRequest request, int PercatID, int ToolNo = 0)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(ClientProfileManagerHelper.GetSurveyPerspective(PercatID, ToolNo).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSurveyPerspective(SurveyPerspectiveModel surveyperspectivemodel, int Id, int ToolNo = 0)
        {
            if (surveyperspectivemodel != null && surveyperspectivemodel.PerspectiveName.Trim() != "" && surveyperspectivemodel.PerspectiveType.Trim() != "" && ModelState.IsValid)
            {
               // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                surveyperspectivemodel.CreatedBy = usid;
                surveyperspectivemodel.Createddt = currentTime;
                surveyperspectivemodel.ClientId = client.ClientId;
                surveyperspectivemodel.ToolId = ToolNo;
                surveyperspectivemodel.PerspectiveCategoryId = Id;
                var res = ClientProfileManagerHelper.AddSurveyPerspective(surveyperspectivemodel);
                surveyperspectivemodel.SId = res;
            }
            return Json(new[] { surveyperspectivemodel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateSurveyPerspective([DataSourceRequest] DataSourceRequest request, SurveyPerspectiveModel surveyperspectivemodel)
        {
            if (surveyperspectivemodel != null && surveyperspectivemodel.PerspectiveName.Trim() != "" && surveyperspectivemodel.PerspectiveType.Trim() != "" && ModelState.IsValid)
            {

                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                surveyperspectivemodel.UpdatedBy = usid;
                surveyperspectivemodel.updateddt = currentTime;
                var res = ClientProfileManagerHelper.EditSurveyPerspective(surveyperspectivemodel);
            }

            return Json(new[] { surveyperspectivemodel }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSurveyPerspective([DataSourceRequest] DataSourceRequest request, SurveyPerspectiveModel surveyperspectivemodel)
        {
            if (surveyperspectivemodel != null)
            {
                var v = ClientProfileManagerHelper.DestorySurveyPerspective(surveyperspectivemodel.SId);
            }
            return Json(new[] { surveyperspectivemodel }.ToDataSourceResult(request, ModelState));
        }
        public IEnumerable<SelectListItem> ApplicationList(int ClientID,int usid)
        {
          //  int usid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var db = new Db();
            var toolList = (from tm in db.AppStore
                            join tr in db.UserApplications on tm.Id equals tr.ApplicationId
                            where tr.ClientId == ClientID && tr.UserId== usid
                            select new { Id=tm.Id,App=tm.ApplicationName }).ToList();
            var item = toolList.OrderBy(x => x.App).Distinct().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.App.ToString(), Selected = true });
            return item;

        }


    }
}
