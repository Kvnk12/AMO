using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using NTTData.AMOPortal.Data;
using WebUI.Areas.TransformationAccelerator.Model;
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
using log4net;

namespace WebUI.Areas.TransformationAccelerator.Controllers
{
    public class ServicePlanController : Controller
    {
        //
        // GET: /TransformationAccelerator/ServicePlan/
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServicePlanController));

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult Index()
        {
            Db db = new Db();
            var Cl = (ClientDetail)Session["SessionClient"];
            IList<ServicePlanningModel> ServicePlanList = getService(Cl.ClientId);

            //distinct year
            List<int?> distinctYearList = (from a in db.ServiceImprovementPlan_Q
                                           where Cl.ClientId==a.ClientId
                                           select a.Year).Distinct().ToList();

            List<string> distintYearString = new List<string>();

            foreach (var item in distinctYearList)
            {
                distintYearString.Add(item.ToString());
            }

            //distinct service improvement type
            //List<string> distinctImpTypeList = (from a in db.ServiceImprovementType
            //                                    select a.ImprovementType).Distinct().ToList();
            List<string> distinctServImpType = (from a in db.ServiceImprovementGoal
                                                join b in db.ServiceImprovementType on a.ImprovementTypeId equals b.Id
                                                where Cl.ClientId == a.ClientId
                                                select b.ImprovementType).Distinct().OrderBy(e => e).ToList();

            var serviceStatusQry = (from a in db.ServiceImprovementPlanStatus
                                    where a.ClientId == Cl.ClientId
                                    select a).OrderByDescending(g => g.Id).FirstOrDefault();

            if (serviceStatusQry != null)
            {
                var serviceStatus = serviceStatusQry.Status;

                if (serviceStatus == "SAVED")
                {
                    serviceStatus = "NOT FINALIZED";
                }

                ViewBag.Status = serviceStatus;
            }
            else
            {
                ViewBag.Status = "NOT FINALIZED";
            }
            ViewBag.Year = distinctYearList;
            ViewBag.ImpType = distinctServImpType;

            return View(ServicePlanList);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult ImprovementProgressView()
        {
            try { 
            // TAReportObservationModel mod = prepareCommonParameters();
            //return View("", mod);
            Db db = new Db();
            var Cl = (ClientDetail)Session["SessionClient"];
            IList<ServicePlanningModel> ServicePlanList = getService(Cl.ClientId);

            //distinct year
            List<int?> distinctYearList = (from a in db.ServiceImprovementPlan_Q
                                           where Cl.ClientId == a.ClientId
                                           select a.Year).Distinct().ToList();

            //distinct service improvement type
            //List<string> distinctImpTypeList = (from a in db.ServiceImprovementType
            //select a.ImprovementType).Distinct().ToList();

            List<string> distinctServImpType = (from a in db.ServiceImprovementGoal 
                                                                join b in db.ServiceImprovementType on a.ImprovementTypeId equals b.Id
                                                                where Cl.ClientId == a.ClientId
                                                                select b.ImprovementType).Distinct().OrderBy(e => e).ToList();           

            var serviceStatusQry = (from a in db.ServiceImprovementPlanStatus
                                    where a.ClientId == Cl.ClientId
                                    select a).OrderByDescending(g => g.Id).FirstOrDefault();
            

            if (serviceStatusQry != null)
            {
                serviceStatusQry.Status.ToUpper();
                var serviceStatus = serviceStatusQry.Status;
                
                if (serviceStatus == "SAVED")
                {
                    serviceStatus = "NOT FINALIZED";
                }

                ViewBag.Status = serviceStatus;
            }
            else
            {
                ViewBag.Status = "NOT FINALIZED";
            }

            ViewBag.Year = distinctYearList;
            ViewBag.ImpType = distinctServImpType;


            return View(ServicePlanList);

            }
            catch (Exception ex)
            {
                Log.Error("Error in ServicePlanController: ImprovementProgressView ", ex);
                throw ex;
            }
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult SIAgreement()
        {
            Db db = new Db();
            var Cl = (ClientDetail)Session["SessionClient"];

            ViewData["ServiceImpList"] = GetListOfServiceAgreement(Cl.ClientId);

            return View();
        }

        public JsonResult Drop_Get_SA()
        {
            Db db = new Db();
            var res = (from a in db.ServiceImprovementType
                       select a).ToList();

            return Json(res.Select(p => new { ImprovementTypeId = p.Id, ImprovementType = p.ImprovementType }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public IList<ServicePlanningModel> getService(int clientid)
        {
            IList<ServicePlanningModel> Serviceplans = new List<ServicePlanningModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<ServicePlanningModel> ServicePlanDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();

                //Linq query to return required fields from ServiceImprovementType,ServiceImprovementGoal,ServiceImprovementPlan_Q tables 
                ServicePlanDetails = (from a in db.ServiceImprovementType
                                      join b in db.ServiceImprovementGoal on a.Id equals b.ImprovementTypeId
                                      join c in db.ServiceImprovementPlan_Q on b.Id equals c.ServiceImprovementGoalId
                                      where b.ClientId == clientid && c.ClientId == clientid
                                      select new ServicePlanningModel
                                      {
                                          Id = c.Id,
                                          ImprovementType = a.ImprovementType,
                                          Quarter = c.Quarter,
                                          Goal = b.Goal,
                                          Year = c.Year,
                                          GoalPlan = c.Goal,
                                          Comment = c.Comment,
                                          GoalAchievement = c.GoalAchievement,
                                          yearQtr = c.Year
                                      }).OrderBy(e => e.Year).ThenBy(e => e.ImprovementType).ThenBy(e => e.Quarter).ToList();


                foreach(var a in ServicePlanDetails)
                {
                    var b = (a.Year.ToString() + a.Quarter);
                    a.yearQtr = Convert.ToInt32(b);
                }

                if (ServicePlanDetails != null)
                {
                    foreach (var _ServicePlan in ServicePlanDetails)
                    {
                        ServicePlanningModel bb = new ServicePlanningModel();
                        bb.Id = _ServicePlan.Id;
                        bb.ImprovementType = _ServicePlan.ImprovementType;
                        bb.Year = _ServicePlan.Year;
                        bb.Quarter = _ServicePlan.Quarter;
                        bb.Goal = _ServicePlan.Goal;
                        bb.GoalPlan = _ServicePlan.GoalPlan;
                        bb.GoalAchievement = _ServicePlan.GoalAchievement;
                        bb.Comment = _ServicePlan.Comment;
                        bb.yearQtr = _ServicePlan.yearQtr;
                        Serviceplans.Add(bb);
                    }
                }
            }

            return Serviceplans;
        }

        //updating goal
        //public ActionResult Update(int uid, decimal uval)
        //{

        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

        //        var paramNameset1 = new string[] { "uid", "goal" };
        //        var paramValueset1 = new object[] { uid, uval };
        //        unitOfWork.ExecProcedure("updateGoal", paramNameset1, paramValueset1); 

        //    }
        //    return null;
        //}

        public ActionResult UpdateNew(string uid)
        {
            try { 
            var Cl = (ClientDetail)Session["SessionClient"];
            //var contextFactory = new DbContextFactory();
            
            var userid = (int)Session["UserId"];
            var allIdnValues = uid.Split('|');
            using (var contextFactory = new DbContextFactory())
            {
                foreach (string item in allIdnValues)
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    if (!string.IsNullOrEmpty(item))
                    {
                        var idvalues = item.Split(',');
                        var id = Convert.ToInt32(idvalues[0]);
                        var val = Convert.ToDecimal(idvalues[1]);

                        var servicePlanQ = unitOfWork.ServiceImprovementPlan_Q.Get(g => g.Id == id).FirstOrDefault();
                        servicePlanQ.Goal = val;
                        unitOfWork.ServiceImprovementPlan_Q.Update(servicePlanQ);
                        //unitOfWork.Save();

                        ServiceImprovementPlanStatus serviceStatusRec = new ServiceImprovementPlanStatus();
                        serviceStatusRec.ClientId = Cl.ClientId;
                        serviceStatusRec.Modified_By = userid;
                        serviceStatusRec.Modified_Date = DateTime.Now;
                        serviceStatusRec.Status = "SAVED";
                        unitOfWork.ServiceImprovementPlanStatus.Insert(serviceStatusRec);
                        unitOfWork.Save();
                    }

                }
            }
            
            return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ServicePlanController: UpdateNew ", ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UpdateValidation()
        {
            try { 
            var result = false;
            var Cl = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            int uid = (int)Session["UserId"];

            //IList<ServicePlanningModel> Serviceplans = new List<ServicePlanningModel>();          

            //var servicePlanQ = unitOfWork.ServiceImprovementPlan_Q.Get();

            //var serviceStatusRec = unitOfWork.ServiceImprovementPlanStatus.Get(g => g.ClientId == Cl.ClientId).FirstOrDefault();


            //adding audit -> Saved
            ServiceImprovementPlanStatus serviceStatusRec = new ServiceImprovementPlanStatus();
            serviceStatusRec.ClientId = Cl.ClientId;
            serviceStatusRec.Modified_By = uid;
            serviceStatusRec.Modified_Date = DateTime.Now;
            serviceStatusRec.Status = "Finalized";
            unitOfWork.ServiceImprovementPlanStatus.Insert(serviceStatusRec);
            unitOfWork.Save();



            List<string> impList = new List<string>();

            var impType = (from a in db.ServiceImprovementType
                           select a).ToList();

            //----------------------------------get sum of quarters(goals) for specific service improvement Type
            foreach (var item in impType)
            {
                var GoalAchievementPlanQ = (from a in db.ServiceImprovementPlan_Q
                                            join b in db.ServiceImprovementGoal on a.ServiceImprovementGoalId equals b.Id
                                            where b.ImprovementTypeId == item.Id && a.ClientId == Cl.ClientId
                                            select a.Goal).ToList().Sum();

                var overallGoal = (from a in db.ServiceImprovementGoal
                                   where a.ClientId == Cl.ClientId && a.ImprovementTypeId == item.Id
                                   select a.Goal).FirstOrDefault();

                if (overallGoal < GoalAchievementPlanQ)
                {

                    result = true;
                    impList.Add(item.ImprovementType);
                }


            }

            string[] impListArray = impList.ToArray();

            return Json(new { isValid = result , impArray = impListArray });
            }
            catch (Exception ex)
            {
                Log.Error("Error in ServicePlanController: UpdateValidation ", ex);
                throw ex;
            }
        }

        public ActionResult UpdateProgressGoal(string uid)
        {
            try { 
            var Cl = (ClientDetail)Session["SessionClient"];
            //var contextFactory = new DbContextFactory();

            var userid = (int)Session["UserId"];
            var allIdnValues = uid.Split('|');
            using (var contextFactory = new DbContextFactory())
            {
                foreach (string item in allIdnValues)
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    if (!string.IsNullOrEmpty(item))
                    {
                        var idvalues = item.Split(',');
                        var id = Convert.ToInt32(idvalues[0]);
                        var val = Convert.ToDecimal(idvalues[1]);
                        var servicePlanQ = unitOfWork.ServiceImprovementPlan_Q.Get(g => g.Id == id).FirstOrDefault();
                        servicePlanQ.GoalAchievement = val;
                        unitOfWork.ServiceImprovementPlan_Q.Update(servicePlanQ);
                        unitOfWork.Save();
                    }
                }
            }

            //var contextFactory = new DbContextFactory();
            //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var servicePlanQ = unitOfWork.ServiceImprovementPlan_Q.Get(g => g.Id == uid).FirstOrDefault();
            //servicePlanQ.GoalAchievement = uval;
            //unitOfWork.ServiceImprovementPlan_Q.Update(servicePlanQ);
            //unitOfWork.Save();

            return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ServicePlanController: UpdateProgressGoal ", ex);
                throw ex;
            }
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        //updating comments
        public ActionResult updateComments(int uid, string comments , decimal goalPlan )
        {
            try { 
            var Cl = (ClientDetail)Session["SessionClient"];

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var paramNameset1 = new string[] { "uid", "comments" };
                var paramValueset1 = new object[] { uid, comments };
                unitOfWork.ExecProcedure("updateComments", paramNameset1, paramValueset1);
                var servicePlanQ = unitOfWork.ServiceImprovementPlan_Q.Get(g => g.Id == uid).FirstOrDefault();
                servicePlanQ.Goal = goalPlan;
                unitOfWork.ServiceImprovementPlan_Q.Update(servicePlanQ);
                unitOfWork.Save();
            }


            return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ServicePlanController: updateComments ", ex);
                throw ex;
            }
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult updateCommentsProgress(int uid, string comments, decimal goalPlan)
        {
            try { 
            var Cl = (ClientDetail)Session["SessionClient"];

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var paramNameset1 = new string[] { "uid", "comments" };
                var paramValueset1 = new object[] { uid, comments };
                unitOfWork.ExecProcedure("updateComments", paramNameset1, paramValueset1);
                var servicePlanQ = unitOfWork.ServiceImprovementPlan_Q.Get(g => g.Id == uid).FirstOrDefault();
                servicePlanQ.GoalAchievement = goalPlan;
                unitOfWork.ServiceImprovementPlan_Q.Update(servicePlanQ);
                unitOfWork.Save();
            }


            return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ServicePlanController: updateCommentsProgress ", ex);
                throw ex;
            }
        }

        public IEnumerable<SelectListItem> GetListOfServiceAgreement(int clientid)
        {
            var ServTypenames = TAHelper.GetListOfServAgreement(clientid);
            return ServTypenames;
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        [HttpPost]
        public ActionResult GetServImp([DataSourceRequest] DataSourceRequest request)
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            return Json(TAHelper.GetServiceImprovementType(Cl.ClientId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AddServImp([DataSourceRequest] DataSourceRequest request, ServiceAgreementModel ServiceAgreement)
        {
            try { 
            Db db = new Db();
            var Cl = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var sid = Convert.ToInt32(ServiceAgreement.ImprovementTypeId);
            ServiceAgreement.ClientId = Cl.ClientId;

            //Improvement Type Name of ImpId
            var serviceImpType = (from a in db.ServiceImprovementType
                                  where a.Id == sid
                                  select a.ImprovementType).FirstOrDefault().ToString();

            var succList = (from a in db.ServiceImprovementGoal
                            join b in db.ServiceImprovementType on a.ImprovementTypeId equals b.Id
                            where a.ClientId == Cl.ClientId
                            select b.ImprovementType).ToList();

            //existing of steadt state engagement OR make check in index load -> make +Add enable/disable

            if (!succList.Contains(serviceImpType))
            {
                if (ServiceAgreement != null && ModelState.IsValid)
                {
                    var res = TAHelper.CreateServiceImprovementType(ServiceAgreement, Cl.ClientId);
                    if (res >0)
                    {
                        if (ServiceAgreement.Goal != 0M)
                        {
                            setService(Cl.ClientId, uid, Convert.ToInt32(ServiceAgreement.ImprovementTypeId));
                        }
                    }

                    //ServiceAgreement.Id = res;
                    //Identify the quarters between steady state start and end date or the renewal's end date
                    //Add records in PlanQ table for above quarters for the given res id
                }
            }
            else
            {
                return JavaScript("alert('Service Improvement  Already Exists'); location.reload(true);");
            }
            return Json(new[] { ServiceAgreement }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in ServicePlanController: AddServImp ", ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult EditServImp([DataSourceRequest] DataSourceRequest request, ServiceAgreementModel ServiceAgreement)
        {
            try {
            var Cl = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
            // if (imItemITBenefitsInput != null && imItemITBenefitsInput.Name.Trim() != "" && imItemITBenefitsInput.Detail.Trim() != "" && ModelState.IsValid)
            if (ServiceAgreement != null && ModelState.IsValid)
            {
                var res = TAHelper.UpdateServiceImprovementType(ServiceAgreement);
                if (res)
                {
                    List<ServiceImprovementPlan_Q> slist = (from a in db.ServiceImprovementPlan_Q
                                                         where ServiceAgreement.Id == a.ServiceImprovementGoalId
                                                         select a).ToList();
                    if (ServiceAgreement.Goal != 0M && slist.Count==0)
                    {
                        setService(Cl.ClientId, uid, Convert.ToInt32(ServiceAgreement.ImprovementTypeId));
                    }
                }
            }
            return Json(new[] { ServiceAgreement }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in IdeaApprovalController: EditServImp ", ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult DeleteServImp([DataSourceRequest] DataSourceRequest request, ServiceAgreementModel ServiceAgreement)
        {
            try { 
            Db db = new Db();
            var Cl = (ClientDetail)Session["SessionClient"];
            int? id = Convert.ToInt32(ServiceAgreement.ImprovementTypeId);

            var succList = (from a in db.ServiceImprovementPlan_Q
                            join b in db.ServiceImprovementGoal on a.ServiceImprovementGoalId equals b.Id
                            where b.ImprovementTypeId == id && Cl.ClientId == a.ClientId
                            select a).ToList();

            if (ServiceAgreement != null && succList.Count() == 0)
            {
                var v = TAHelper.RemoveServiceImprovementType(ServiceAgreement.Id);
                if (v)
                {
                    checkService(Cl.ClientId, ServiceAgreement.Id);
                }
            }
            else
            {
                //return Json(new[] { ServiceAgreement }.ToDataSourceResult(request, ModelState));
                //return JavaScript("alert('Service Improvement has plan and progress updated, can’t be deleted'); location.reload(true);");
                //return JavaScript("location.reload(true);");
                var succList1 = GetListOfServiceAgreement(Cl.ClientId);
               
                ModelState.AddModelError("Id", "Service Improvement has plan and progress updated, can’t be deleted");
                return  Json(new[] { succList1 }.ToDataSourceResult(request, ModelState)); 
            }                       

            return Json(new[] { ServiceAgreement }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in ServicePlanController: DeleteServImp ", ex);
                throw ex;
            }
        }

        public static void setService(int clientid, int uid, int? impId)
        {
            Db db = new Db();
            var SteadyDate = (from a in db.ClientEngagement
                              where a.EngagementPhaseId == 5 && a.ClientId == clientid
                              select new
                              {
                                  startDate = a.StartDate,
                                  endDate = a.EndDate
                              }).FirstOrDefault();
            if (SteadyDate != null)
            {
                DateTime sdate = SteadyDate.startDate.Value;
                DateTime edate = SteadyDate.endDate.Value;

                using (var contextFactory = new DbContextFactory())
                {
                    for (int yea = sdate.Year; yea <= edate.Year; yea++)
                    {
                        int monstart = 1;
                        int monend = 12;
                        if (yea == sdate.Year)
                            monstart = sdate.Month;
                        if (yea == edate.Year)
                            monend = edate.Month;
                        while (monstart <= monend)
                        {
                            if (monstart >= 1 && monstart <= 3)
                            {
                                createplan(clientid, uid, "1", yea, impId);
                                monstart = 4;
                            }
                            else if (monstart >= 4 && monstart <= 6)
                            {
                                createplan(clientid, uid, "2", yea, impId);
                                monstart = 7;
                            }
                            else if (monstart >= 7 && monstart <= 9)
                            {
                                createplan(clientid, uid, "3", yea, impId);
                                monstart = 10;
                            }
                            else if (monstart >= 10 && monstart <= 12)
                            {
                                createplan(clientid, uid, "4", yea, impId);
                                monstart = 13;
                            }
                        }
                    }                 
                }
            }

        }

        public static void createplan(int cid, int uid, string quater, int year, int? impid)
        {
            Db db = new Db();
            List<ServiceImprovementPlan_Q> spq = (from a in db.ServiceImprovementPlan_Q
                                                  join b in db.ServiceImprovementGoal on a.ServiceImprovementGoalId equals b.Id
                                                  where a.ClientId == cid && a.Quarter == quater && year == a.Year && b.ImprovementTypeId == impid
                                                  select a).ToList();
            if (spq.Count == 0)
            {
                List<ServicePlanningModel> SerPlan = new List<ServicePlanningModel>();
                var sTypeGoal = (from a in db.ServiceImprovementType
                                 join b in db.ServiceImprovementGoal on a.Id equals b.ImprovementTypeId
                                 where cid == b.ClientId && b.ImprovementTypeId==impid
                                 select b).FirstOrDefault();
                var _Details = new ServiceImprovementPlan_Q();

                    _Details.ClientId = cid;
                    _Details.ServiceImprovementGoalId = sTypeGoal.Id;
                    _Details.Year = year;
                    _Details.Quarter = quater;
                    _Details.GoalAchievement = 0M;
                    _Details.Comment = "";
                    _Details.CreatedBy = uid;
                    _Details.PlanModifiedBy = uid;
                    _Details.CreatedDate = DateTime.Now;
                    _Details.PlanModifiedDate = null;
                    _Details.Goal = 0M;
                    _Details.AchivementModifiedBy = uid;
                    _Details.AchievementModifiedDate = null;
                    db.ServiceImprovementPlan_Q.Add(_Details);
                    db.SaveChanges();
                
            }
        }

        public static void checkService(int clientid, int Id)
        {

            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
                var SteadyDate = (from a in db.ClientEngagement
                                  where a.EngagementPhaseId == 5 && a.ClientId == clientid
                                  select new
                                  {
                                      startDate = a.StartDate,
                                      endDate = a.EndDate
                                  }).FirstOrDefault();
                DateTime sdate = SteadyDate.startDate.Value;
                DateTime edate = SteadyDate.endDate.Value;

                for (int yea = sdate.Year; yea <= edate.Year; yea++)
                {
                    int monstart = 1;
                    int monend = 12;
                    if (sdate.Year == yea)
                        monstart = sdate.Month;
                    if (yea == edate.Year)
                        monend = edate.Month;
                    while (monstart <= monend)
                    {
                        string quarter = "";
                        if (monstart >= 1 && monstart <= 3)
                        {
                            quarter = "1";
                            monstart = 4;
                        }
                        else if (monstart >= 4 && monstart <= 6)
                        {
                            quarter = "2";
                            monstart = 7;
                        }
                        else if (monstart >= 7 && monstart <= 9)
                        {
                            quarter = "3";
                            monstart = 10;
                        }
                        else if (monstart >= 10 && monstart <= 12)
                        {
                            quarter = "4";
                            monstart = 13;
                        }
                        var spq = (from a in db.ServiceImprovementPlan_Q
                                   where a.ClientId == clientid && a.Quarter == quarter && yea == a.Year && a.Id == Id
                                   select a).FirstOrDefault();
                        deleteservice(spq);
                      
                    }
                }
            }
        }

        public static void deleteservice(ServiceImprovementPlan_Q list)
        {
            if(list!=null)
            {
                if (list.Goal == 0M)
                {
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        unitOfWork.ServiceImprovementPlan_Q.Delete(list.Id);
                        unitOfWork.Save();
                    }
                }
            }
            
        }
    }
}
