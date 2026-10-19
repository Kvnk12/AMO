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
using WebUI.Areas.IncidentNavigator.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class INIncidentServicehoursController : Controller
    {
        //
        // GET: /IncidentNavigator/INIncidentServicehours/

        [ActionTrack]
        public ActionResult Index(int? ClientId)
        {
            INIncidentServicehoursModel c = new INIncidentServicehoursModel();
            c.ClientId = ClientId;
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


    }
}
