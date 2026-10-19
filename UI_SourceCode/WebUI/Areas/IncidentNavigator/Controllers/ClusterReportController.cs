using Domain.Entities.ClientProfileManager;
using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Globalization;
using WebUI.Areas.IncidentNavigator.Dto;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WebUI.Dto;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class ClusterReportController : Controller
    {
        //
        // GET: /IncidentNavigator/ClusterReport/

        [ActionTrack]
        public ActionResult Index( int? ClientId)
        {
            ClusterReportInput c = new ClusterReportInput();
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientID = client.ClientId;
            c.ClientId = ClientID;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            if (ClientId.HasValue)
            {
             
            //c.ClusterReportlist = IncidentNavigatorHelper.INGetClusterReportlist(ClientId.Value).ToList();
            //    var model = new ClusterReportInput();
            //    if (ClientWorkshops.Count > 0)
            //        model.ClusterReportlist = (ClientWorkshops.ToList().ToDataSourceResult(request));
            //   // model.ClientId = ClientId;
            //    return Json(model.ClusterReportlist, JsonRequestBehavior.AllowGet);
           }
            //else
            //    return Json(false, JsonRequestBehavior.AllowGet);
            return View(c);
            }
        public string getFullName(int userId)
        {
            Db db = new Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }

        //public ActionResult GetData([DataSourceRequest]DataSourceRequest request, int? ClientId)
        //{
        //    if (ClientId.HasValue)
        //    {
        //        List<ClusterReportInput> ClientWorkshops = IncidentNavigatorHelper.INGetClusterReportlist(ClientId.Value).ToList();
        //        var model = new ClusterReportInput();
        //        if (ClientWorkshops.Count > 0)
        //            model.ClusterReportlist = (ClientWorkshops.ToList().ToDataSourceResult(request));
        //        model.ClientId = ClientId;
        //        return Json(model, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //}

    }
   
    public class ExtendedController : Controller
    {
        //protected new JsonResult Json(object data)
        //{
        //    if (!Request.AcceptTypes.Contains("application/json"))
        //        return base.Json(data, "text/plain");
        //    else
        //        return base.Json(data);
        //}
    }
}
