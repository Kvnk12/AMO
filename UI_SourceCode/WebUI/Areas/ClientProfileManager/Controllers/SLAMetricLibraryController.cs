using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Dto;
using WebUI.Areas.IncidentNavigator.Models;
using WebUI.Areas.ClientProfileManager.ViewModel;
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
using Domain.Entities.IncidentNavigator;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class SLAMetricLibraryController : Controller
    {
        // GET: ClientProfileManager/SLAMetricLibrary
        public ActionResult Index()
        {
            Db db = new Db();
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            MetricModel c = prepareCommonParameters();
            List<MetricSpecification> MetricSpecificationList = (from a in db.MetricSpecifications
                                                                 where a.ClientId == client.ClientId
                                                                 select a).ToList();

            List<ServiceCategory> ServiceCategoryList = (from a in db.ServiceCategories
                                                         select a).ToList();

            List<NTTData.AMOPortal.Domain.Entities.Service> ServiceList = (from a in db.Services
                                                                           select a).ToList();

            List<Metric> MetricList = (from a in db.Metrics
                                       select a).ToList();

            ViewBag.MetricSpecificationList = MetricSpecificationList;
            ViewBag.ServiceCategoryList = ServiceCategoryList;
            ViewBag.ServiceList = ServiceList;
            ViewBag.MetricList = MetricList;
            return View(c);
        }

        [HttpPost]
        public ActionResult GetMetricLibraryList([DataSourceRequest] DataSourceRequest request,string serviceName, string serviceCatName)
        {
           ClientDetail client = (ClientDetail)Session["SessionClient"];
           var rec = ClientProfileManagerHelper.GetListOfMetrics(serviceName, serviceCatName);
            foreach(var met in rec)
            {
                met.Flag = 1;
            }

          
           return Json(rec.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteMetric([DataSourceRequest] DataSourceRequest request, MetricModel metModel)
        {
            Db db = new Db();
            ClientDetail Client = (ClientDetail)Session["SessionClient"];

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.Metric .Delete(metModel.Id);
                unitOfWork.Save();

            }
            return Json(new[] { metModel }.ToDataSourceResult(request, ModelState));
        }

        private MetricModel prepareCommonParameters()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            MetricModel mod = new MetricModel();
            GetFilterInfo(mod, client.ClientId);
            return mod;
        }
        public static void GetFilterInfo(MetricModel mod, int ClientId)
        {
            try
            {
                var paramName = new string[] { "ClientId" };
                var paramValue = new object[] { ClientId };
                DataTable dt = new DataTable();

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DataSet ds = new DataSet();


                    ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllFilterforSLAMetric", paramName, paramValue);

                    List<SelectListItem> ls;

                    for (int t = 0; t < ds.Tables.Count; t++)
                    {
                        int i = 0;
                        ls = new List<SelectListItem>();
                        foreach (DataRow row in ds.Tables[t].Rows)
                        {
                            ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = row.ItemArray[1].ToString() });
                            i++;
                        }
                        switch (t)
                        {
                            case 0:
                                mod.ServiceCategoryList = ls;
                                break;
                            case 1:
                                mod.ServiceList = ls;
                                break;
                            case 2:
                                mod.ServiceComponentList = ls;
                                break;
                            case 3:
                                mod.MetricPerspectiveList = ls;
                                break;
                            case 4:
                                mod.MetricTypeList = ls;
                                break;
                        }

                    }

                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveSlaMetricDetails([DataSourceRequest] DataSourceRequest request,MetricModel dt)
        {
            Db db = new Db();
            var Cl = (ClientDetail)Session["SessionClient"];
            var UserId = (int)Session["UserId"];
            var userName = (from a in db.UserProfiles
                            where a.Id == UserId
                            select a.UserName).FirstOrDefault().ToString();
            var temp = ClientProfileManagerHelper.SaveSLAMetric(dt, userName);
            return Json(new[] { temp }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult getSlaMetricDetails_View(int Id = 0)
        {
            var contextFactory = new DbContextFactory();
            Db db = new Db();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Metric km = unitOfWork.Metric.GetByID(Id);
            MetricModel metval = new MetricModel();

            //var Accountname = (from a in db.ClientAccountSetup
            //                   where a.Id == e.AccountId
            //                   select a.AccountName).FirstOrDefault().ToString();

            var serviceCategory = (from a in db.ServiceCategories
                                   where a.Id == km.ServiceCategoryId
                                   select a.Name).FirstOrDefault();

            var service = (from a in db.Services
                           where a.Id == km.ServiceId
                           select a.Name).FirstOrDefault();


            var metricType = (from a in db.MetricTypes
                              where a.Id == km.MetricTypeId
                              select a.TypeName).FirstOrDefault();

            var metricPerspective = (from a in db.MetricPerspectives
                                     where a.Id == km.MetricPerspectiveId
                                     select a.PerspectiveName).FirstOrDefault();

            var serviceComponent = (from a in db.ServiceComponents
                                     where a.Id == km.ServiceComponentId
                                     select a.Name).FirstOrDefault();

            metval.Id = km.Id;
            metval.MetricName = km.MetricName;
            metval.Objective = km.Objective;
            metval.ObjectiveType = km.ObjectiveType;
            //slaval.ContractId = km.ContractId;
            metval.Calculation = km.Calculation;
            metval.CalculationDataElement = km.CalculationDataElement;
            metval.ContextualDataElement = km.ContextualDataElement;
            metval.inUse = km.inUse;
            metval.ServiceCategoryId = km.ServiceCategoryId;
            metval.ServiceId = km.ServiceId;
            metval.ServiceComponentId = km.ServiceComponentId;
            metval.MetricPerspectiveId = km.MetricPerspectiveId;
            metval.MetricTypeId = km.MetricTypeId;
            metval.ChartIndicator = km.ChartIndicator;
            metval.MetricUnit = km.MetricUnit;
            metval.Rules = km.Rules;
            metval.MetricUnit = km.MetricUnit;
            metval.PrimaryAudience = km.PrimaryAudience;
            metval.ServiceCategory = serviceCategory;
            metval.Service = service;
            metval.MetricType = metricType;
            metval.MetricPerspective = metricPerspective;
            metval.ChartIndicator = km.ChartIndicator;
            metval.MetricUnit = km.MetricUnit;
            metval.Rules = km.Rules;
            metval.ServiceComponent = serviceComponent;
            metval.PrimaryAudience = km.PrimaryAudience;
            return Json(metval, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult getSlaMetricDetailsForEdit(int Ids)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //int Ids = Convert.ToInt32(Id);
            Metric km = unitOfWork.Metric.GetByID(Ids);
            Db db = new Db();

            var serviceCategory = (from a in db.ServiceCategories
                                  where a.Id == km.ServiceCategoryId
                                  select a.Name).FirstOrDefault();

            var service =  (from a in db.Services
                            where a.Id == km.ServiceId
                            select a.Name).FirstOrDefault();

            var metricPerspective = (from a in db.MetricPerspectives
                                     where a.Id == km.MetricPerspectiveId
                                     select a.PerspectiveName).FirstOrDefault();

            var metricType = (from a in db.MetricTypes
                                     where a.Id == km.MetricTypeId
                                     select a.TypeName).FirstOrDefault();

            MetricModel metval = new MetricModel();      


            metval.Id = km.Id;
            metval.MetricName = km.MetricName;
            metval.Objective = km.Objective;
            metval.ObjectiveType = km.ObjectiveType;
            //slaval.ContractId = km.ContractId;
            metval.Calculation = km.Calculation;
            metval.CalculationDataElement = km.CalculationDataElement;
            metval.ContextualDataElement = km.ContextualDataElement;
            metval.inUse = km.inUse;
            metval.ServiceCategoryId = km.ServiceCategoryId;
            metval.ServiceId = km.ServiceId;
            metval.ServiceComponentId = km.ServiceComponentId;
            metval.MetricPerspectiveId = km.MetricPerspectiveId;
            metval.MetricTypeId = km.MetricTypeId;
            metval.ChartIndicator = km.ChartIndicator;
            metval.MetricUnit = km.MetricUnit;
            metval.Rules = km.Rules;
            metval.MetricUnit = km.MetricUnit;
            metval.PrimaryAudience = km.PrimaryAudience;
            metval.ServiceCategory = serviceCategory;
            metval.Service = service;
            metval.MetricType = metricType;
            metval.MetricPerspective = metricPerspective;
            metval.ChartIndicator = km.ChartIndicator;
            metval.MetricUnit = km.MetricUnit;
            metval.Rules = km.Rules;
            metval.PrimaryAudience = km.PrimaryAudience;

            return Json(metval, JsonRequestBehavior.AllowGet);

         

        }

    }
}