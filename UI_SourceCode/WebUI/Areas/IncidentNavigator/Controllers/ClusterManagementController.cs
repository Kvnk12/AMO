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
using System.Data;
using Data;
using Domain.Entities.IncidentNavigator;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class ClusterManagementController : Controller
    {
        //
        // GET: /IncidentNavigator/ClusterManagement/

        [ActionTrack]
        public ActionResult Index(int? ClientId)
        {
            ClusterManagementInput c = new ClusterManagementInput();
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientID = client.ClientId;
            c.ClientId = ClientID;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View(c);
        }


        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult INIncidentClusteredUpdate(List<Thing> listObject)
        {
            if (listObject != null && ModelState.IsValid)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    string targetOldValue = "";
                    string IPAddress = IPHelper.GetIPAddress(Request.ServerVariables["HTTP_VIA"],
                                                      Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                                                      Request.ServerVariables["REMOTE_ADDR"]);

                    foreach (var v in listObject)
                    {

                        var target = unitOfWork.INIncidentClustered.Get(x => x.INCIDENT_ID == v.id).FirstOrDefault();
                        targetOldValue = target.CLUSTERID.ToString();
                        if (target != null)
                        {
                            target.CLUSTERID = v.value;

                            unitOfWork.INIncidentClustered.Update(target);

                        }

                        using (var db1 = new DbContextFactory())
                        {
                            var DB = new UnitOfWorks(db1.GetContext());
                            INAuditTable newitem = new INAuditTable();
                            newitem.UserId = (int)Session["UserId"];
                            newitem.ClientId = target.Clientid;
                            newitem.Timestamp = DateTime.Now;
                            newitem.IncidentId = target.INCIDENT_ID.ToString();
                            newitem.Action = "Update";
                            newitem.ColumnName = "Cluster";
                            newitem.OldValue = targetOldValue;
                            newitem.NewValue = v.value.ToString();
                            newitem.IPAddress = IPAddress;
                            DB.INAuditTable.Insert(newitem);
                            DB.Save();

                        }

                    }

                    unitOfWork.Save();

                }

            }

            return Json(ModelState.ToDataSourceResult());
        }
        public class Thing
        {
            public int id { get; set; }
            public int value { get; set; }
            //public string IncidentID { get; set; }
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
