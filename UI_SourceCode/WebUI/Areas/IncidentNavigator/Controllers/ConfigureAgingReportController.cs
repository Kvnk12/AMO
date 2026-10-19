using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Dto;
using WebUI.Areas.IncidentNavigator.Models;
using Kendo.Mvc.UI;
using WebUI.Areas.IncidentNavigator.Dto;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.WebUI.Filters;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System.Collections.Specialized;
using Domain.Entities;
using System.Web.Configuration;
using Data;
using NTTData.AMOPortal.Data;
using System.Data;
using System.IO;
using log4net;
using Domain.Entities.IncidentNavigator;


namespace WebUI.Areas.IncidentNavigator.Controllers
{
   

    public class ConfigureAgingReportController : Controller
    {
        //
        // GET: /IncidentNavigator/ConfigureAgingReport/
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigureAgingReportController));
        [ActionTrack]
        public ActionResult Index()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View();
        }

        public ActionResult ConfigureAging()
        {
            try
            {
                ViewData["dateTypeList"] = GetListOfDateType();
                ViewData["Count"] = getCountConfig();
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                return View("ConfigureAging");
            }
            catch (Exception ex)
            {
                Log.Error("ConfigureAgingReport controller Index  Action error 'ConfigureAging()' ", ex);
                throw ex;
            }
        }

        public IEnumerable<SelectListItem> GetListOfDateType()
        {
            try
            {
                List<SelectListItem> dateTypeList = new List<SelectListItem>();
                var newItem = new SelectListItem { Text = "Assigneddatetime", Value = "Assigneddatetime" };
                var newItem1 = new SelectListItem { Text = "Createddatetime", Value = "Createddatetime" };
                dateTypeList.Add(newItem);
                dateTypeList.Add(newItem1);
                return dateTypeList;
            }
            catch (Exception ex)
            {
                Log.Error("ConfigureAgingReport controller Index  Action error 'GetListOfDateType()' ", ex);
                throw ex;
            }
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult GetConfigAging([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                //int usid = (int)Session["UserId"];
                var Cl = (ClientDetail)Session["SessionClient"];
                return Json(IncidentNavigatorHelper.GetConfigAgingQuery(Cl.ClientId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("ConfigureAgingReport controller Index  Action error 'GetConfigAging()' ", ex);
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditConfigAging([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<ConfigureAgingReportModel> ConfigAgingReportList)
        {
            try
            {
                int usid = (int)Session["UserId"];
                var Cl = (ClientDetail)Session["SessionClient"];
                foreach (var item in ConfigAgingReportList)
                {
                    item.ClientID = Cl.ClientId;
                    var id = IncidentNavigatorHelper.UpdateConfigReportQuery(item, usid);
                }
                return Json(new[] { ConfigAgingReportList }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("ConfigureAgingReport controller Index  Action error 'EditConfigAging()' ", ex);
                throw ex;
            }
        }

     

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddConfigAging([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<ConfigureAgingReportModel> ConfigAgingReportList)
        {
            try
            {
                int usid = (int)Session["UserId"];
                var Cl = (ClientDetail)Session["SessionClient"];
                if (ConfigAgingReportList != null)
                {
                    foreach (var item in ConfigAgingReportList)
                    {
                        item.ClientID = Cl.ClientId;
                        var id = IncidentNavigatorHelper.AddNewConfigReportQuery(item, usid);
                        if (id == 0)
                        {
                            ModelState.AddModelError("Id", "Service Improvement has plan and progress updated, can’t be deleted");
                            return Json(new[] { ConfigAgingReportList }.ToDataSourceResult(request, ModelState));
                        }
                    }
                }
                return Json(new[] { ConfigAgingReportList }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("ConfigureAgingReport controller Index  Action error 'AddConfigAging()' ", ex);
                throw ex;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteConfigAging([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<ConfigureAgingReportModel> ConfigAgingReportList)
        {
            try {
                Db db = new Db();
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                foreach (var item in ConfigAgingReportList)
                {
                    var v = IncidentNavigatorHelper.RemoveConfigReportQuery(item.ID);
                }

                return Json(new[] { ConfigAgingReportList }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("ConfigureAgingReport controller Index  Action error 'DeleteConfigAging()' ", ex);
                throw ex;
            }
        }

        public int getCountConfig()
        {
            try
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                Db db = new Db();
                int count = (from a in db.ConfigAgingQuery
                             where a.ClientID == Client.ClientId
                             select a).Count();

                return count;
            }
            catch (Exception ex)
            {
                Log.Error("ConfigureAgingReport controller Index  Action error 'getCountConfig()' ", ex);
                throw ex;
            }
        }

        
        [HttpPost]
        [ActionTrack]
        public ActionResult deleteConfig(int SeqNo, int ID)
        {
            try
            {
                var res = IncidentNavigatorHelper.deleteConfigAgingIN(SeqNo, ID);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("ConfigureAgingReport controller Index  Action error 'deleteConfig()' ", ex);
                throw ex;
            }
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
