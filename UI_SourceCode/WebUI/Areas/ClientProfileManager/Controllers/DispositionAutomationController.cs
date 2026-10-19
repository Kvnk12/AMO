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
    public class DispositionAutomationController : Controller
    {
        //
        // GET: /ClientProfileManager/DispositionAutomation/

        public ActionResult Index()
        {

            return View();
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetDispositionDetails(DispositionAutomationModel DispAutomation)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            int uid = (int)Session["UserId"];
            var res = ClientProfileManagerHelper.UpdateDispositionDetail(DispAutomation, client.ClientId);


            return Json(DispAutomation);
        }
        public ActionResult GetDispositionDetail()
        {
            // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];

            var model = ClientProfileManagerHelper.getClientDetail(client.ClientId);
            return Json(model);


        }

    }
}
