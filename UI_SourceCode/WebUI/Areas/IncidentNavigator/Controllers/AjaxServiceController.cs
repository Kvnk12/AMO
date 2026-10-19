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
using WebUI.Areas.IncidentNavigator.Models;
using Data;
using Domain.Entities.IncidentNavigator;
using Kendo.Mvc.Export;
using Telerik.Documents.SpreadsheetStreaming;

using System.Collections;
using WebUI.Dto;
using System.IO;
using log4net;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class AjaxServiceController : Controller
    {

        //public  int ClientId
        //{
        //    get
        //    {


        //        return 1;
        //    }


        //}

        private static readonly ILog Log = LogManager.GetLogger(typeof(AjaxServiceController));

        //
        // GET: /IncidentNavigator/ClusterReport/
        [ActionTrack]
        [HttpPost]
        public ActionResult GetData([DataSourceRequest]DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            try
            {
                // var client = (Client)Session["Client"];
                List<ClusterReportInput> ClientWorkshops = IncidentNavigatorHelper.INGetClusterReportlist(ClientId).ToList();
                var model = new ClusterReportInput();

                return Json((ClientWorkshops.OrderByDescending(x => x.TotalIncidents).ToList().ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController   GetData , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetClustersList(string TicketType, string ConfigItemName)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            string ticketType = string.Join(",", v);
           // var client = (Client)Session["Client"];
            List<ClusterlistModel> ClientWorkshops = IncidentNavigatorHelper.INGetDistinctClusters(ClientId, ticketType, ConfigItemName).ToList();


            return Json(ClientWorkshops.ToList().Select(p => new { Value = p.ClusterIDfromCluster.ToString(), Text = p.ClusterLablefromCluster.ToString() }), JsonRequestBehavior.AllowGet);
        }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetClustersList , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClustersWordcloud([DataSourceRequest]DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            List<ClusterlistModel> ClientWorkshops = IncidentNavigatorHelper.INGetDistinctClustersForWordCloud(ClientId).ToList();

            return Json(ClientWorkshops.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetClustersWordcloud , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIncidentClustersList([DataSourceRequest]DataSourceRequest request, int ClusterId)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            List<IncidentsClusterlistModel> IncidentClusterlist = IncidentNavigatorHelper.INGetIncidentClusterlistforWordCloud(ClientId, ClusterId).ToList();
            var model = new IncidentsClusterlistModel();

            return Json((IncidentClusterlist.ToList().ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetIncidentClustersList , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCascadeConfigItem()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            Db db = new Db();
            var IncidentList = (from Inc in db.Incidents
                                where Inc.ClientId == ClientId
                                select new { Value = Inc.ConfigItemName, Text = Inc.ConfigItemName }).Distinct().ToList();
            IncidentList.Add(new { Value = "All", Text = "All" });
                return Json(IncidentList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetCascadeConfigItem , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<SelectListItem> ConfigItemlist(int ClientID)
        {
            try
            { 
            Db db = new Db();
            var item = (from Inc in db.Incidents
                                where Inc.ClientId == ClientID
                                select new SelectListItem { Value= Inc.ConfigItemName, Text= Inc.ConfigItemName, Selected=true }).OrderBy(x=>x.Text).Distinct().ToList();
            if(item.Count>0)
            {
                item.Add(new SelectListItem {  Text="All", Value="All"});
            }
            return item;


            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  ConfigItemlist , Client ID is " + ClientID, ex);
            }
            return null;
        }


        public IEnumerable<SelectListItem> TicketTypelist(int ClientID)
        {
            try { 
           var list = IncidentNavigatorHelper.getColumnValuesList(ClientID, "Type").ToList().Select(f => new SelectListItem { Value = f.FieldValue, Text = f.FieldValue });
            return list;
        }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  TicketTypelist , Client ID is " + ClientID, ex);
            }
            return null;
}

        public IEnumerable<SelectListItem> Tracklist(int ClientID)
        {
            try
            { 
            var list = IncidentNavigatorHelper.getColumnValuesList(ClientID, "Tower").ToList().Select(f => new SelectListItem { Value = f.FieldValue, Text = f.FieldValue });
            return list;
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  Tracklist , Client ID is " + ClientID, ex);
            }
            return null;
        }

        public IEnumerable<SelectListItem> Statuslist(int ClientID)
        {
            try
            {
            var list = IncidentNavigatorHelper.getColumnValuesList(ClientID, "Status").ToList().Select(f => new SelectListItem { Value = f.FieldValue, Text = f.FieldValue });
            return list;
        }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  Statuslist , Client ID is " + ClientID, ex);
            }
            return null;
}
        public IEnumerable<SelectListItem> Prioritylist(int ClientID)
        {
            try { 
            var list = IncidentNavigatorHelper.getColumnValuesList(ClientID, "Priority").ToList().Select(f => new SelectListItem { Value = f.FieldValue, Text = f.FieldValue });
            return list;
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  Prioritylist , Client ID is " + ClientID, ex);
            }
            return null;
        }
        public IEnumerable<SelectListItem> GetColumnList(int ClientID)
        {
            try
            { 
            var list = IncidentNavigatorHelper.getMappedColumnValuesList(ClientID).ToList().Select(f => new SelectListItem { Value = f.INFieldName, Text = f.INFieldName });
            return list;
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetColumnList , Client ID is " + ClientID, ex);
            }
            return null;
        }
        public ActionResult GetCascadeType()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            Db db = new Db();

            var Typelist = (from Inc in db.Incidents
                            where Inc.ClientId==ClientId
                            select new { Value=Inc.Type, Text=Inc.Type}).Distinct().ToList();
            Typelist.Add(new { Value = "All", Text = "All" });

            return Json(Typelist, JsonRequestBehavior.AllowGet);
                // return Json(Typelist.Select(p => new { Value = p.ToString(), Text = p.ToString() }), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetCascadeType , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult Cluster_Update([DataSourceRequest] DataSourceRequest request, ClusterlistModel Cluster)
        {
            try { 
            if (Cluster != null && ModelState.IsValid)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    if (Cluster.ClusterIDfromCluster > 0)
                    {
                        var target = unitOfWork.INClusters.GetByID(Cluster.ClusterIDfromCluster);


                        target.ClusterLabel = Cluster.ClusterLablefromCluster;

                        unitOfWork.INClusters.Update(target);
                    }

                    unitOfWork.Save();

                }

            }
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  Cluster_Update , Client ID is " , ex);
            }

            return Json(ModelState.ToDataSourceResult());
        }


        public class Thing
        {
            public int id { get; set; }
            public int value { get; set; }
            //public string IncidentID { get; set; }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult INIncidentClusteredUpdate([DataSourceRequest] DataSourceRequest request, List<Thing> listObject)
        {
            try { 
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
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  INIncidentClusteredUpdate , Client ID is " , ex);
            }
            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult GetHasSLAmanager()
        {
            try
            { 
            var items = new INIncidentServicehoursModel();

            return Json(items.HasSLAmanagerList.Select(g => new { Text = g.Text, HasSLAManager = g.Value }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetHasSLAmanager , Client ID is " , ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult INIncidentServicehoursInsertorUpdate(INIncidentServicehoursModel ServiceModel)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            if (ServiceModel != null && ModelState.IsValid)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    foreach (var v in ServiceModel.Priorities)
                    {
                       // var client = (Client)Session["Client"];
                        var target = unitOfWork.INIncidentServicehours.Get(x => x.ClientId == ClientId && x.Priority == v.Value).FirstOrDefault();
                        if (target != null)
                        {
                            if (ServiceModel.HasSLAManager != null && ServiceModel.HasSLAManager == 1)
                            {
                                target.HasSLAManager = true;
                            }
                            else
                            {
                                target.HasSLAManager = false;
                            }
                            switch (target.Priority)
                            {
                                case "1":

                                    target.PriorityServicehours = CheckNull(ServiceModel.P1ServiceHours);
                                    target.WeekendHours = CheckNull(ServiceModel.P1WeekendHours);
                                    target.BlendedRate = ServiceModel.P1Blendedrate.HasValue ? ServiceModel.P1Blendedrate.Value : 0;

                                    break;

                                case "2":

                                    target.PriorityServicehours = CheckNull(ServiceModel.P2ServiceHours);
                                    target.WeekendHours = CheckNull(ServiceModel.P2WeekendHours);
                                    target.BlendedRate = ServiceModel.P2Blendedrate.HasValue ? ServiceModel.P2Blendedrate.Value : 0;

                                    break;

                                case "3":

                                    target.PriorityServicehours = CheckNull(ServiceModel.P3ServiceHours);
                                    target.WeekendHours = CheckNull(ServiceModel.P3WeekendHours);
                                    target.BlendedRate = ServiceModel.P3Blendedrate.HasValue ? ServiceModel.P3Blendedrate.Value : 0;

                                    break;

                                case "4":

                                    target.PriorityServicehours = CheckNull(ServiceModel.P4ServiceHours);
                                    target.WeekendHours = CheckNull(ServiceModel.P4WeekendHours);
                                    target.BlendedRate = ServiceModel.P4Blendedrate.HasValue ? ServiceModel.P4Blendedrate.Value : 0;

                                    break;

                                case "5":

                                    target.PriorityServicehours = CheckNull(ServiceModel.P5ServiceHours);
                                    target.WeekendHours = CheckNull(ServiceModel.P5WeekendHours);
                                    target.BlendedRate = ServiceModel.P5Blendedrate.HasValue ? ServiceModel.P5Blendedrate.Value : 0;

                                    break;

                            }

                            unitOfWork.INIncidentServicehours.Update(target);

                        }
                        else
                        {
                            INIncidentServicehours newitem = new INIncidentServicehours();
                            newitem.Priority = v.Value;
                            newitem.ClientId = ClientId;
                            if (ServiceModel.HasSLAManager != null && ServiceModel.HasSLAManager == 1)
                            {
                                newitem.HasSLAManager = true;
                            }
                            else
                            {
                                newitem.HasSLAManager = false;
                            }

                            switch (v.Value)
                            {
                                case "1":
                                    newitem.Priority = "1";
                                    newitem.PriorityServicehours = CheckNull(ServiceModel.P1ServiceHours);
                                    newitem.WeekendHours = CheckNull(ServiceModel.P1WeekendHours);
                                    newitem.BlendedRate = ServiceModel.P1Blendedrate.HasValue ? ServiceModel.P1Blendedrate.Value : 0;

                                    break;

                                case "2":
                                    newitem.Priority = "2";
                                    newitem.PriorityServicehours = CheckNull(ServiceModel.P2ServiceHours);
                                    newitem.WeekendHours = CheckNull(ServiceModel.P2WeekendHours);
                                    newitem.BlendedRate = ServiceModel.P2Blendedrate.HasValue ? ServiceModel.P2Blendedrate.Value : 0;

                                    break;

                                case "3":
                                    newitem.Priority = "3";
                                    newitem.PriorityServicehours = CheckNull(ServiceModel.P3ServiceHours);
                                    newitem.WeekendHours = CheckNull(ServiceModel.P3WeekendHours);
                                    newitem.BlendedRate = ServiceModel.P3Blendedrate.HasValue ? ServiceModel.P3Blendedrate.Value : 0;

                                    break;

                                case "4":
                                    newitem.Priority = "4";
                                    newitem.PriorityServicehours = CheckNull(ServiceModel.P4ServiceHours);
                                    newitem.WeekendHours = CheckNull(ServiceModel.P4WeekendHours);
                                    newitem.BlendedRate = ServiceModel.P4Blendedrate.HasValue ? ServiceModel.P4Blendedrate.Value : 0;

                                    break;

                                case "5":
                                    newitem.Priority = "5";
                                    newitem.PriorityServicehours = CheckNull(ServiceModel.P5ServiceHours);
                                    newitem.WeekendHours = CheckNull(ServiceModel.P5WeekendHours);
                                    newitem.BlendedRate = ServiceModel.P5Blendedrate.HasValue ? ServiceModel.P5Blendedrate.Value : 0;

                                    break;

                            }
                            unitOfWork.INIncidentServicehours.Insert(newitem);

                        }




                        //}

                        unitOfWork.Save();
                        if (ServiceModel.HasSLAManager != 1)
                        {
                            IncidentNavigatorHelper.ExecuteINResolutionCalc(ClientId);
                            IncidentNavigatorHelper.ExecuteINBlendedRateCalc(ClientId);

                        }
                    }

                }


            }

            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  INIncidentServicehoursInsertorUpdate , Client ID is " + ClientId, ex);
            }
            return Json(ModelState.ToDataSourceResult());

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult INIncidentServicehoursGet(INIncidentServicehoursModel ServiceModel)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            {
            if (ServiceModel != null && ModelState.IsValid)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var target = unitOfWork.INIncidentServicehours.Get(x => x.ClientId == ClientId).ToList();
                    if (target.Count > 0)
                    {
                        ServiceModel.HasSLAManager = Convert.ToInt32(target.FirstOrDefault().HasSLAManager);

                        foreach (var v in target)
                        {
                            switch (v.Priority)
                            {
                                case "1":

                                    ServiceModel.P1Blendedrate = v.BlendedRate;
                                    ServiceModel.P1ServiceHours = v.PriorityServicehours;
                                    ServiceModel.P1WeekendHours = v.WeekendHours;
                                    break;

                                case "2":

                                    ServiceModel.P2Blendedrate = v.BlendedRate;
                                    ServiceModel.P2ServiceHours = v.PriorityServicehours;
                                    ServiceModel.P2WeekendHours = v.WeekendHours;
                                    break;

                                case "3":

                                    ServiceModel.P3Blendedrate = v.BlendedRate;
                                    ServiceModel.P3ServiceHours = v.PriorityServicehours;
                                    ServiceModel.P3WeekendHours = v.WeekendHours;
                                    break;

                                case "4":

                                    ServiceModel.P4Blendedrate = v.BlendedRate;
                                    ServiceModel.P4ServiceHours = v.PriorityServicehours;
                                    ServiceModel.P4WeekendHours = v.WeekendHours;
                                    break;

                                case "5":

                                    ServiceModel.P5Blendedrate = v.BlendedRate;
                                    ServiceModel.P5ServiceHours = v.PriorityServicehours;
                                    ServiceModel.P5WeekendHours = v.WeekendHours;
                                    break;

                                    //case "3":

                                    //    target.PriorityServicehours = CheckNull(ServiceModel.P3ServiceHours);
                                    //    target.WeekendHours = CheckNull(ServiceModel.P3WeekendHours);
                                    //    target.BlendedRate = ServiceModel.P3Blendedrate.HasValue ? ServiceModel.P3Blendedrate.Value : 0;

                                    //    break;

                                    //case "4":

                                    //    target.PriorityServicehours = CheckNull(ServiceModel.P4ServiceHours);
                                    //    target.WeekendHours = CheckNull(ServiceModel.P4WeekendHours);
                                    //    target.BlendedRate = ServiceModel.P4Blendedrate.HasValue ? ServiceModel.P4Blendedrate.Value : 0;

                                    //    break;

                                    //case "5":

                                    //    target.PriorityServicehours = CheckNull(ServiceModel.P5ServiceHours);
                                    //    target.WeekendHours = CheckNull(ServiceModel.P5WeekendHours);
                                    //    target.BlendedRate = ServiceModel.P5Blendedrate.HasValue ? ServiceModel.P5Blendedrate.Value : 0;

                                    //    break;

                            }
                        }
                    }

                }

            }

            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  INIncidentServicehoursGet , Client ID is " + ClientId, ex);
            }

            return Json(ServiceModel, JsonRequestBehavior.AllowGet);
        }


        public int? CheckNull(int? item)
        {
            if (item != null)
            {
                return item.Value;
            }
            else
            {
                return null;
            }

        }

        [ActionTrack]
        [HttpPost]
        public ActionResult GetDashboardTrend(string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            string ticketType = string.Join(",", v);
            // var client = (Client)Session["Client"];
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INDashboardTicketTrend(ClientId, ticketType, ConfigItemName, StartDate, EndDate).ToList();
                return Json(model);

            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboardTrend , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult GetDashboardTrendnew(string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INNewDashboardTicketTrend("TicketTrend", ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate).ToList();

            return Json(model);
        }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboardTrendnew , Client ID is " + ClientId, ex);
            }
            return null;
}

        [HttpPost]
        public ActionResult GetDashboardWorkgroup(string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            string ticketType = string.Join(",", v);
           // var client = (Client)Session["Client"];
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INDashboardTopWorkgroups(ClientId, ticketType, ConfigItemName, StartDate, EndDate).ToList();

            return Json(model);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboardWorkgroup , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetDashboardTopHoldtime(string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            string ticketType = string.Join(",", v);
           // var client = (Client)Session["Client"];
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INDashboardTopholdtimeUnresolved(ClientId, ticketType, ConfigItemName, StartDate, EndDate).ToList();

            return Json(model);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboardTopHoldtime , Client ID is " + ClientId, ex);
            }
            return null;
        }


        [HttpPost]
        public ActionResult GetDashboardTrendnewGrid(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, [DataSourceRequest]DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INNewDashboardTicketTrend("TicketTrend", ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate).ToList();
            return Json((model.ToList().ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboardTrendnewGrid , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetDashboardYearMonth(int ClientId)
        {
            try { 
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INYearmonthForDashboard(ClientId).ToList();

            DashboardTrendModel d = new DashboardTrendModel();
            d.DefaultYearMonth = model.Count - 1;
            return Json(model.Select(g => new { Text = g.ddlyearMonth.ToString(), Value = g.ddlyearMonth.ToString() }), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboardYearMonth , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetDashboarSeverity(string TicketType, string ConfigItemName)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            string ticketType = string.Join(",", v);
           // var client = (Client)Session["Client"];
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INDashboardSeverity(ClientId, ticketType, ConfigItemName).ToList();

            return Json(model);
        }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboarSeverity , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult GetDashboarClusters(string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            string ticketType = string.Join(",", v);
           // var client = (Client)Session["Client"];
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INDashboardCluster(ClientId, ticketType, ConfigItemName, StartDate, EndDate).ToList();

            return Json(model);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboarClusters , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult GetDashboarClustersDrilldown(string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate , String clusterlabel, [DataSourceRequest]DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            string ticketType = string.Join(",", v);
           // var client = (Client)Session["Client"];
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INDashboardClusterDrilldown(ClientId, ticketType, ConfigItemName, StartDate, EndDate , clusterlabel).ToList();

           // return Json(model);
            return Json((model.ToList().ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);
        }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboarClustersDrilldown , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);


        }


        [ActionTrack]
        [HttpPost]
        public ActionResult GetDashboardTicketTrendInflowDrilldown(string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate,string InflowOutflow,string YearMonth, [DataSourceRequest]DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            string ticketType = string.Join(",", v);
           // var client = (Client)Session["Client"];
            List<DashboardTrendModel> model = null;
            if (InflowOutflow == "Ticket Count")
            {
                model = IncidentNavigatorHelper.INDashboardTicketTrendInflowDrilldown(ClientId, ticketType, ConfigItemName, StartDate, EndDate, YearMonth).ToList();
            }

            else
            {
                model = IncidentNavigatorHelper.INDashboardTicketTrendOutflowDrilldown(ClientId, ticketType, ConfigItemName, StartDate, EndDate, YearMonth).ToList();

            }
          //  return Json(model);
            return Json((model.ToList().ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboardTicketTrendInflowDrilldown , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }


        [ActionTrack]
        [HttpPost]
        public ActionResult GetDashboardTicketHoldtimeDrilldown(string IncidentId, [DataSourceRequest]DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            List<DashboardTrendModel> model = IncidentNavigatorHelper.INDashboardTop5HoldtimeDrilldown(IncidentId).ToList();

            //return Json(model);
             return Json((model.ToList().ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboardTicketHoldtimeDrilldown , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult GetDashboardAvgResolutionCost(string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            string ticketType = string.Join(",", v);
            //var client = (Client)Session["Client"];
            IList<DashboardTrendModel> model = IncidentNavigatorHelper.INDashboardAvgResolutionCost(ClientId, ticketType, ConfigItemName, StartDate, EndDate);

            return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetDashboardAvgResolutionCost , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);


        }


        public ActionResult GetINClusterAudit([DataSourceRequest]DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            //if (ClientID.HasValue)
            //{
           // var client = (Client)Session["Client"];
            List<ClusterAuditModel> Auditlist = IncidentNavigatorHelper.INGetClusterAuditlist(ClientId).ToList();


            return Json((Auditlist.ToList().ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);
                //}
                //else
                //    return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  GetINClusterAudit , Client ID is " + ClientId, ex);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public FileStreamResult ClusterAuditExportServer(string model, string data, string clientid)
        {
            try { 
            var columnsData = JsonConvert.DeserializeObject<IList<ExportColumnSettings>>(HttpUtility.UrlDecode(model));
            dynamic options = JsonConvert.DeserializeObject(HttpUtility.UrlDecode(data));
            SpreadDocumentFormat exportFormat = options.format.ToString() == "csv" ? exportFormat = SpreadDocumentFormat.Csv : exportFormat = SpreadDocumentFormat.Xlsx;
            Action<ExportCellStyle> cellStyle = new Action<ExportCellStyle>(ChangeCellStyle);
            Action<ExportRowStyle> rowStyle = new Action<ExportRowStyle>(ChangeRowStyle);
            Action<ExportColumnStyle> columnStyle = new Action<ExportColumnStyle>(ChangeColumnStyle);

            string fileName = string.Format("{0}.{1}", options.title, options.format);
            string mimeType = Helpers.GetMimeType(exportFormat);

            int ClientID = Convert.ToInt32(JsonConvert.DeserializeObject(HttpUtility.UrlDecode(clientid)));
            List<ClusterAuditModel> Auditlist = IncidentNavigatorHelper.INGetClusterAuditlist(ClientID).ToList();
            Stream exportStream = exportFormat == SpreadDocumentFormat.Xlsx ?
                Auditlist.ToXlsxStream(columnsData, (string)options.title.ToString(), cellStyleAction: cellStyle, rowStyleAction: rowStyle, columnStyleAction: columnStyle) :
                Auditlist.ToCsvStream(columnsData);

            var fileStreamResult = new FileStreamResult(exportStream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);

            return fileStreamResult;
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  ClusterAuditExportServer , Client ID is " + clientid, ex);
            }
            return null;

        }
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult Export([DataSourceRequest]DataSourceRequest request)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<ClusterReportInput> ClientWorkshops = IncidentNavigatorHelper.INGetClusterReportlist(ClientId).ToList();
            //Get the data representing the current grid state - page, sort and filter
            var v = ClientWorkshops.ToDataSourceResult(request).Data;

            ////Create new Excel workbook
            //var workbook = new HSSFWorkbook();

            ////Create new Excel sheet
            //var sheet = workbook.CreateSheet();

            ////(Optional) set the width of the columns
            //sheet.SetColumnWidth(0, 10 * 256);
            //sheet.SetColumnWidth(1, 50 * 256);
            //sheet.SetColumnWidth(2, 50 * 256);
            //sheet.SetColumnWidth(3, 50 * 256);

            ////Create a header row
            //var headerRow = sheet.CreateRow(0);

            ////Set the column names in the header row
            //headerRow.CreateCell(0).SetCellValue("Product ID");
            //headerRow.CreateCell(1).SetCellValue("Product Name");
            //headerRow.CreateCell(2).SetCellValue("Unit Price");
            //headerRow.CreateCell(3).SetCellValue("Quantity Per Unit");

            ////(Optional) freeze the header row so it is not scrolled
            //sheet.CreateFreezePane(0, 1, 0, 1);

            //int rowNumber = 1;

            ////Populate the sheet with values from the grid data
            //foreach (Product product in products)
            //{
            //    //Create a new row
            //    var row = sheet.CreateRow(rowNumber++);

            //    //Set values for the cells
            //    row.CreateCell(0).SetCellValue(product.ProductID);
            //    row.CreateCell(1).SetCellValue(product.ProductName);
            //    row.CreateCell(2).SetCellValue(product.UnitPrice.ToString());
            //    row.CreateCell(3).SetCellValue(product.QuantityPerUnit.ToString());
            //}

            ////Write the workbook to a memory stream
            //MemoryStream output = new MemoryStream();
            //Workbook.Write(output);

            //////Return the result to the end user

            //return File(output.ToArray(),   //The binary data of the XLS file
            //    "application/vnd.ms-excel", //MIME type of Excel files
            //    "GridExcelExport.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
            return Json(v);
        }
        [HttpPost]
        public FileStreamResult ClusterExportServer(string model, string data)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            { 
            var columnsData = JsonConvert.DeserializeObject<IList<ExportColumnSettings>>(HttpUtility.UrlDecode(model));
            dynamic options = JsonConvert.DeserializeObject(HttpUtility.UrlDecode(data));
            SpreadDocumentFormat exportFormat = options.format.ToString() == "csv" ? exportFormat = SpreadDocumentFormat.Csv : exportFormat = SpreadDocumentFormat.Xlsx;
            Action<ExportCellStyle> cellStyle = new Action<ExportCellStyle>(ChangeCellStyle);
            Action<ExportRowStyle> rowStyle = new Action<ExportRowStyle>(ChangeRowStyle);
            Action<ExportColumnStyle> columnStyle = new Action<ExportColumnStyle>(ChangeColumnStyle);

            string fileName = string.Format("{0}.{1}", options.title, options.format);
            string mimeType = Helpers.GetMimeType(exportFormat);

           // var client = (Client)Session["Client"];
            List<ClusterReportInput> ClientWorkshops = IncidentNavigatorHelper.INGetClusterReportlist(ClientId).ToList();

            Stream exportStream = exportFormat == SpreadDocumentFormat.Xlsx ?
                ClientWorkshops.ToXlsxStream(columnsData, (string)options.title.ToString(), cellStyleAction: cellStyle, rowStyleAction: rowStyle, columnStyleAction: columnStyle) :
                ClientWorkshops.ToCsvStream(columnsData);

            var fileStreamResult = new FileStreamResult(exportStream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);

            return fileStreamResult;
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  ClusterExportServer , Client ID is " + ClientId, ex);
            }
            return null;
        }


        [HttpPost]
        public FileStreamResult ClusterWordcloudExportServer(string model, string data, string clientid, string configitem, string tickettype, string clusterid)
        {
            try { 
            var columnsData = JsonConvert.DeserializeObject<IList<ExportColumnSettings>>(HttpUtility.UrlDecode(model));
            dynamic options = JsonConvert.DeserializeObject(HttpUtility.UrlDecode(data));
            SpreadDocumentFormat exportFormat = options.format.ToString() == "csv" ? exportFormat = SpreadDocumentFormat.Csv : exportFormat = SpreadDocumentFormat.Xlsx;
            Action<ExportCellStyle> cellStyle = new Action<ExportCellStyle>(ChangeCellStyle);
            Action<ExportRowStyle> rowStyle = new Action<ExportRowStyle>(ChangeRowStyle);
            Action<ExportColumnStyle> columnStyle = new Action<ExportColumnStyle>(ChangeColumnStyle);

            string fileName = string.Format("{0}.{1}", options.title, options.format);
            string mimeType = Helpers.GetMimeType(exportFormat);

            int ClientId = Convert.ToInt32(JsonConvert.DeserializeObject(HttpUtility.UrlDecode(clientid)));
            string ConfigItemName = JsonConvert.DeserializeObject(HttpUtility.UrlDecode(configitem)).ToString();
            string TicketType = JsonConvert.DeserializeObject(HttpUtility.UrlDecode(tickettype)).ToString();
            int ClusterId = Convert.ToInt32(JsonConvert.DeserializeObject(HttpUtility.UrlDecode(clusterid)));
            List<IncidentsClusterlistModel> IncidentClusterlist = IncidentNavigatorHelper.INGetIncidentClusterlist(ClientId, TicketType, ConfigItemName, ClusterId).ToList();

            Stream exportStream = exportFormat == SpreadDocumentFormat.Xlsx ?
                IncidentClusterlist.ToXlsxStream(columnsData, (string)options.title.ToString(), cellStyleAction: cellStyle, rowStyleAction: rowStyle, columnStyleAction: columnStyle) :
                IncidentClusterlist.ToCsvStream(columnsData);

            var fileStreamResult = new FileStreamResult(exportStream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);

            return fileStreamResult;
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  ClusterWordcloudExportServer , Client ID is " + clientid, ex);
            }
            return null;
        }

        private void ChangeCellStyle(ExportCellStyle e)
        {

            try{
                bool isHeader = e.Row == 0;
            SpreadCellFormat format = new SpreadCellFormat
            {
                //ForeColor = isHeader ? SpreadThemableColor.FromRgb(50, 54, 58) : SpreadThemableColor.FromRgb(214, 214, 217),
                //IsItalic = true,
                //VerticalAlignment = SpreadVerticalAlignment.Center,
                //WrapText = true,
                //Fill = SpreadPatternFill.CreateSolidFill(isHeader ? new SpreadColor(93, 227, 0) : new SpreadColor(50, 54, 58))
            };
            e.Cell.SetFormat(format);
            }
            catch (Exception ex)
            {
                Log.Error("AjaxServiceController  ChangeCellStyle , Client ID is " , ex);
            }

        }

        private void ChangeRowStyle(ExportRowStyle e)
        {
            //e.Row.SetHeightInPixels(e.Index == 0 ? 80 : 30);
        }

        private void ChangeColumnStyle(ExportColumnStyle e)
        {
            //double width = e.Name == "Product name" || e.Name == "Category Name" ? 250 : 100;
            //e.Column.SetWidthInPixels(width);
           
        }
    }

}
