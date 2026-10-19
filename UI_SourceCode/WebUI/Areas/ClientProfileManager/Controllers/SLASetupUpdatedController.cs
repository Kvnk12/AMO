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
using WebUI.Areas.IncidentNavigator;
using WebUI.Areas.ClientProfileManager.ViewModel;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class SLASetupUpdatedController : Controller
    {
        // GET: ClientProfileManager/SLASetupUpdated
        public ActionResult Index()
        {
            MetricSpecificationsModel c = prepareCommonParameters();
            Db db = new Db();
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            List<string> SLAMenuList = new List<string>();
            List<ServiceCategory> ServiceCategoryList = (from a in db.ServiceCategories
                                                         select a).ToList();

            List<NTTData.AMOPortal.Domain.Entities.Service> ServiceList = (from a in db.Services
                                                                           select a).ToList();

            List<Metric> MetricList = (from a in db.Metrics
                                       select a).ToList();

            List<MetricSpecification> MetricSpecificationList = (from a in db.MetricSpecifications
                                                                 where a.ClientId == client.ClientId
                                                                 select a).ToList();



            ViewBag.ServiceCategoryList = ServiceCategoryList;
            ViewBag.ServiceList = ServiceList;
            ViewBag.MetricList = MetricList;
            ViewBag.MetricSpecificationList = MetricSpecificationList;


            for (int i = 0; i < ServiceCategoryList.Count; i++)
            {
                SLAMenuList.Add(ServiceCategoryList.ElementAt(i).Name);
                for (int j = 0; j < ServiceList.Count; j++)
                {
                    if (ServiceCategoryList.ElementAt(i).Id == ServiceList.ElementAt(j).ServiceCategoryId)
                    {
                        SLAMenuList.Add(ServiceList.ElementAt(j).Name);
                    }
                    for (int k = 0; k < MetricList.Count; k++)
                    {
                        if (MetricList.ElementAt(k).ServiceId == ServiceList.ElementAt(j).Id)
                        {
                            SLAMenuList.Add(MetricList.ElementAt(k).MetricName);
                        }
                    }
                }
            }

            return View(c);
        }

        private MetricSpecificationsModel prepareCommonParameters()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            MetricSpecificationsModel mod = new MetricSpecificationsModel();
            GetFilterInfo(mod, client.ClientId);
            return mod;
        }
        public static void GetFilterInfo(MetricSpecificationsModel mod, int ClientId)
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


                    ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllFilterforSLASetup", paramName, paramValue);

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
                                mod.AccountNamesList = ls;
                                break;
                            case 1:
                                mod.AccContractList = ls;
                                break;
                            case 2:
                                mod.MetricStatusList = ls;
                                break;
                            case 3:
                                mod.MetricUnitList = ls;
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


        [HttpPost]
        public ActionResult GetSlaList([DataSourceRequest] DataSourceRequest request, int Id, string MetricName)
        {
            //int usid = (int)Session["UserId"];
            try
            {
                var Cl = (ClientDetail)Session["SessionClient"];
                Db db = new Db();



                IEnumerable<MetricSpecificationsModel> klist = IncidentNavigatorHelper.getSlaList(Cl.ClientId, Id, MetricName);



                return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Log.Error("Error in GetKEDB", ex);
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSlaSetup([DataSourceRequest] DataSourceRequest request, MetricSpecificationsModel metSpecModel)
        {
            Db db = new Db();
            ClientDetail Client = (ClientDetail)Session["SessionClient"];

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.MetricSpecification.Delete(metSpecModel.Id);
                unitOfWork.Save();

            }
            return Json(new[] { metSpecModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SaveSlaSetupDetails(MetricSpecificationsModel dt)
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            dt.ClientId = Cl.ClientId;
            var temp = IncidentNavigatorHelper.SaveSLASetup(dt);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult getSlaSetupDetailsForEdit(int Ids,int Ids1)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //int Ids = Convert.ToInt32(Id);
            MetricSpecification km = unitOfWork.MetricSpecification.GetByID(Ids);
            Db db = new Db();

            //int Id1s = Convert.ToInt32(Id1);

            var MetricUnitId = (from a in db.Metrics
                                where a.Id == Ids
                                select a.MetricUnitId).FirstOrDefault();

            var MetricUnit = (from a in db.MetricUnits
                              where a.Id == MetricUnitId
                              select a.UnitName).FirstOrDefault();

            MetricSpecificationsModel slaval = new MetricSpecificationsModel();

            var Accountname = (from a in db.ClientAccountSetup
                               where a.Id == km.AccountId
                               select a.AccountName).FirstOrDefault().ToString();

            var Contractname = (from a in db.ClientAccountContract
                                where a.Id == km.ContractId
                                select a.ContractName).FirstOrDefault().ToString();

            var MetricStatus = (from a in db.MetricStatuss
                                where a.Id == km.MetricStatusId
                                select a.StatusValue).FirstOrDefault().ToString();


            var MetricName = (from a in db.Metrics
                              where a.Id == km.MetricId
                              select a.MetricName).FirstOrDefault();

            slaval.Id = km.Id;
            slaval.AccountName = Accountname;
            slaval.AccountId = km.AccountId;
            slaval.ContractName = Contractname;
            slaval.ContractId = km.ContractId;
            slaval.MetricStatusId = km.MetricStatusId;
            slaval.Priority = km.Priority;
            slaval.MetricName = MetricName;
            slaval.MetricId = km.MetricId;
            slaval.Weightage = km.Weight;
            slaval.MetricStatus = MetricStatus;
            slaval.HasSLA = km.HasSLA;
            slaval.StartDate = km.StartDate;
            slaval.EndDate = km.EndDate;
            slaval.MetricBaseline = km.MetricBaseline;
            slaval.MetricTarget = km.MetricTarget;
            slaval.MetricUnit = MetricUnit;
            slaval.MetricOwner = km.MetricOwner;
            slaval.Clocksetup = km.Clocksetup;
            slaval.ComplianceUnits = km.ComplianceUnits;
            slaval.ComplianceBaseline = km.ComplianceBaseline;
            slaval.ComplianceTarget = km.ComplianceTarget;
            slaval.ExceedsExpectation = km.ExceedsExpectation;
            //slaval.MetSLAExpectationsBTW = e.MetSLAExpectationsBTW;
            slaval.BelowExpectation = km.BelowExpectation;
            slaval.ClientPriority = km.ClientPriority;

            return Json(slaval, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult getSlaSetupDetails_View(int Id = 0)
        {
            var contextFactory = new DbContextFactory();
            Db db = new Db();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            MetricSpecification e = unitOfWork.MetricSpecification.GetByID(Id);
            MetricSpecificationsModel slaval = new MetricSpecificationsModel();

            var Accountname = (from a in db.ClientAccountSetup
                               where a.Id == e.AccountId
                               select a.AccountName).FirstOrDefault().ToString();


            var Contractname = (from a in db.ClientAccountContract
                                where a.Id == e.ContractId
                                select a.ContractName).FirstOrDefault().ToString();


            var MetricStatus = (from a in db.MetricStatuss
                                where a.Id == e.MetricStatusId
                                select a.StatusValue).FirstOrDefault().ToString();

            var MetricUnitId = (from a in db.Metrics
                                where a.Id == e.MetricId
                                select a.MetricUnitId).FirstOrDefault();

            var MetricUnit = (from a in db.MetricUnits
                              where a.Id == MetricUnitId
                              select a.UnitName).FirstOrDefault();

            slaval.Id = e.Id;
            slaval.AccountName = Accountname;
            slaval.ContractName = Contractname;
            slaval.Priority = e.Priority;
            slaval.ClientPriority = e.ClientPriority;
            slaval.MetricName = e.MetricName;
            slaval.MetricId = e.MetricId;
            slaval.Weightage = e.Weight;
            slaval.MetricStatus = MetricStatus;
            slaval.HasSLA = e.HasSLA;
            slaval.StartDate = e.StartDate;
            slaval.EndDate = e.EndDate;
            slaval.MetricBaseline = e.MetricBaseline;
            slaval.MetricTarget = e.MetricTarget;
            slaval.MetricUnit = MetricUnit;
            slaval.MetricOwner = e.MetricOwner;
            slaval.Clocksetup = e.Clocksetup;
            slaval.ComplianceUnits = e.ComplianceUnits;
            slaval.ComplianceBaseline = e.ComplianceBaseline;
            slaval.ComplianceTarget = e.ComplianceTarget;
            slaval.ExceedsExpectation = e.ExceedsExpectation;
            //slaval.MetSLAExpectationsBTW = e.MetSLAExpectationsBTW;
            slaval.BelowExpectation = e.BelowExpectation;
            return Json(slaval, JsonRequestBehavior.AllowGet);

        }



        public ActionResult getServiceandCategory(int Id)
        {
            var contextFactory = new DbContextFactory();
            Db db = new Db();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //MetricSpecification e = unitOfWork.MetricSpecification .GetByID(Id);
            MetricSpecificationsModel slaval = new MetricSpecificationsModel();
            var serviceID = (from a in db.Metrics
                             where a.Id == Id
                             select a.ServiceId).FirstOrDefault();

            var serviceCatId = (from a in db.Metrics
                                where a.Id == Id
                                select a.ServiceCategoryId).FirstOrDefault();

            slaval.Service = (from a in db.Services
                              where serviceID == a.Id
                              select a.Name).FirstOrDefault().ToString();

            slaval.ServiceCategory = (from a in db.ServiceCategories
                                      where serviceCatId == a.Id
                                      select a.Name).FirstOrDefault().ToString();

            return Json(slaval, JsonRequestBehavior.AllowGet);

        }




        [HttpPost]
        public ActionResult getMetricUnitName(string Id1)
        {
            var contextFactory = new DbContextFactory();
            Db db = new Db();
            int Ids = Convert.ToInt32(Id1);
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var MetricUnitId = (from a in db.Metrics
                                where a.Id == Ids
                                select a.MetricUnitId).FirstOrDefault();

            var MetricUnit = (from a in db.MetricUnits
                              where a.Id == MetricUnitId
                              select a.UnitName).FirstOrDefault();

            return Json(MetricUnit, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getContractId(int Accid = 0)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            Db db = new Db();
            //var dispSln = (from acc in db.Incidents
            //               join con in db.ClientAccountContract on acc.ContractId equals con.Id
            //               where con.AccountId == Accid && acc.ClientId == ClientId
            //               select new
            //               {
            //                   Id = con.Id,
            //                   ContractName = con.ContractName
            //               }).ToList().Distinct();

            var dispSln = (from acc in db.ClientAccountContract
                           where acc.AccountId == Accid && acc.ClientId == ClientId
                           select new
                           {
                               Id = acc.Id,
                               ContractName = acc.ContractName
                           }).ToList().Distinct();

            return Json(dispSln.ToList().Select(A => new { Value = A.Id.ToString(), Text = A.ContractName, Selected = true }), JsonRequestBehavior.AllowGet);

        }

    }

}