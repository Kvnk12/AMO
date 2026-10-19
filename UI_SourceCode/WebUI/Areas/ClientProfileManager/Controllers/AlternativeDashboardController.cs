using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.ViewModel;
using WebUI.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class AlternativeDashboardController : Controller
    {
        //
        // GET: /ClientProfileManager/AlternativeDashboard/

        public ActionResult Index()
        {
            return View();
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetAlternativeDetails(AlternativeDashboardModel AltDashboard)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var res = ClientProfileManagerHelper.UpdateAlternativeDetail(AltDashboard, client.ClientId);


            return Json(AltDashboard);
        }
        public ActionResult GetDispositionDetail()
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];

            var model = ClientProfileManagerHelper.getAlternativeClientDetail(client.ClientId);
            return Json(model);


        }

    }
}

