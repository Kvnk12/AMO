using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IncidentNavigator.Models;
using Kendo.Mvc.UI;
using WebUI.Areas.IncidentNavigator.Dto;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.WebUI.Filters;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.Data;
using Data;
using WebUI.Dto;
using System.Collections.Specialized;
using Domain.Entities;

namespace WebUI.Controllers
{
    public class ServicePlanController : Controller
    {
        //
        // GET: /ServicePlanning/

        public ActionResult Index()
        {
            Db db = new Db();
            var Cl = (ClientDetail)Session["SessionClient"];
            IList<ServicePlanningModel> ServicePlanList = getService(Cl.ClientId);

            //distinct year
            List<int?> distinctYearList = (from a in db.ServiceImprovementPlan_Q
                                           select a.Year).Distinct().ToList();

            List<string> distintYearString = new List<string>();

            foreach(var item in distinctYearList)
            {
                distintYearString.Add(item.ToString());
            }

            //distinct service improvement type
            List<string> distinctImpTypeList = (from a in db.ServiceImprovementType
                                                select a.ImprovementType).Distinct().ToList();

            ViewBag.Year = distinctYearList;
            ViewBag.ImpType = distinctImpTypeList;

            return View(ServicePlanList);
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
                                      select new ServicePlanningModel
                                      {
                                          Id = c.Id,
                                          ImprovementType = a.ImprovementType,
                                          Quarter = c.Quarter,
                                          Goal = b.Goal,
                                          Year = c.Year,
                                          GoalPlan = c.Goal,
                                          Comment = c.Comment
                                      }).OrderBy(e=>e.Year).ThenBy(e=>e.ImprovementType).ThenBy(e=>e.Quarter);

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
                        bb.Comment = _ServicePlan.Comment;
                        Serviceplans.Add(bb);
                    }
                }
            }

         return Serviceplans;
        }

        public ActionResult Update(int uid , decimal uval)
        {
            Db db = new Db();

            ServiceImprovementPlan_Q record = (from a in db.ServiceImprovementPlan_Q
                          where a.Id == uid
                          select a).FirstOrDefault();

            ServiceImprovementPlan_Q rec = db.ServiceImprovementPlan_Q.Find(uid);
            db.ServiceImprovementPlan_Q.Remove(rec);

            if (record != null)
            {
                ServiceImprovementPlan_Q bb = new ServiceImprovementPlan_Q();
                bb.Id = record.Id;
                bb.ClientId = record.ClientId;
                bb.ServiceImprovementGoalId = record.ServiceImprovementGoalId;
                bb.Year = record.Year;
                bb.Quarter = record.Quarter;
                bb.GoalAchievement = record.GoalAchievement;
                bb.Comment = record.Comment;
                bb.CreatedBy = record.CreatedBy;
                bb.PlanModifiedBy = record.PlanModifiedBy;
                bb.CreatedDate = record.CreatedDate;
                bb.PlanModifiedDate = record.PlanModifiedDate;
                bb.Goal = uval;
                bb.AchivementModifiedBy = record.AchivementModifiedBy;
                bb.AchievementModifiedDate = record.AchievementModifiedDate;

                db.ServiceImprovementPlan_Q.Add(bb);
                db.SaveChanges();
            }

            return null;
        }

    }
}
