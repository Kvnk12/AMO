using Data;
using Domain.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.TransformationAccelerator.Model;
using WebUI.Dto;
using log4net;

namespace WebUI.Areas.TransformationAccelerator.Controllers
{
    public class EngagementTimeLineController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EngagementTimeLineController));

        // GET: /TransformationAccelerator/EngagementTimeLine/
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult Index()
        {            
            Db db = new Db();
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            
            var engCPhases = (from a in db.ClientEngagement
                              where a.ClientId == client.ClientId
                              select a.EngagementPhaseId).Distinct().ToList();

            var engPhases = (from a in db.EngagementPhases
                             select a).ToList();

            IEnumerable<EngagementType> EngLine = new List<EngagementType>();
            List<EngagementType> elist = new List<EngagementType>();
            foreach (var type in engPhases)
            {
                if (!engCPhases.Contains(type.Id) && type.Id != (int)EngType.Renewal)
                {
                    var _Details = new ClientEngagement();
                    _Details.EngagementPhaseId = type.Id;
                    _Details.ClientId = client.ClientId;
                    _Details.StartDate = null;
                    _Details.EndDate = null;
                    _Details.CreatedDate = DateTime.Now;
                    _Details.CreatedBy = uid;
                    _Details.ModifiedBy = uid;
                    _Details.ModifiedDate = null;
                    db.ClientEngagement.Add(_Details);
                    db.SaveChanges();
                }
            }

            //BaseLine Data
            BaseLineDataModel bData = new BaseLineDataModel();          
            bData = getBaseLineData(client.ClientId);

            return View("Index", bData);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        [HttpPost]
        public JsonResult getTimeLine([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];   
            var result = getEngTimeLine(client.ClientId);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<EngagementType> getEngTimeLine(int clientId)
        {
            IEnumerable<EngagementType> EngagementTimeLine = null;

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();

                EngagementTimeLine = (from a in db.ClientEngagement
                                      join b in db.UserProfiles on a.ModifiedBy equals b.Id
                                      join c in db.EngagementPhases on a.EngagementPhaseId equals c.Id
                                      where a.ClientId == clientId
                                      select new EngagementType
                                      {
                                          Id = a.Id,
                                          EngagementTypes = c.EngagementPhase,
                                          StartDate = a.StartDate,
                                          EndDate = a.EndDate,
                                          ModifiedBy = b.FirstName + " " + b.LastName,
                                          ModifiedDate = a.ModifiedDate,
                                          EngagementTypeId=a.EngagementPhaseId.Value,
                                      }).OrderBy(e => e.EngagementTypeId).ToList();             
            }
            return EngagementTimeLine;
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        [HttpPost]
        public ActionResult ClientEngagementCreate([DataSourceRequest] DataSourceRequest request, EngagementType model)
        {
          try { 
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            var uid = (int)Session["UserId"];
            Db db = new Db();

            if (model != null)
            {
                if (model.EndDate >= model.StartDate)
                {
                    var resDate = (from a in db.ClientEngagement
                                   where client.ClientId == a.ClientId && a.EngagementPhaseId == 6
                                   select new
                                   {
                                       resId = a.Id,
                                       resStDate = a.StartDate,
                                       resEnDate = a.EndDate
                                   }).OrderByDescending(e => e.resId).FirstOrDefault();
                    var stdyDate = (from a in db.ClientEngagement
                                    where a.ClientId == client.ClientId && a.EngagementPhaseId == 5
                                    select new
                                    {
                                        StDate = a.StartDate,
                                        EnDate = a.EndDate
                                    }).FirstOrDefault();
                    if (stdyDate.StDate != null && stdyDate.EnDate != null)
                    {
                        if (model.StartDate > stdyDate.EnDate)
                        {
                            if (resDate != null)
                            {
                                if (model.StartDate >= resDate.resEnDate)
                                {
                                    int EngagementId = createClientEngag(model, client.ClientId, uid);
                                    var uname = (from a in db.UserProfiles where a.Id == uid select a.FirstName + a.LastName).FirstOrDefault();
                                    model.Id = EngagementId;
                                    model.EngagementTypes = "Renewal";
                                    model.ModifiedBy = uname;
                                    model.ModifiedDate = DateTime.Now;
                                }
                                else
                                {
                                    ModelState.AddModelError("EndDate", "StartDate of Renewal Phase should be Greater than EndDate of Previous Renewal Phase");
                                }
                            }
                            else
                            {
                                int EngagementId = createClientEngag(model, client.ClientId, uid);
                                var uname = (from a in db.UserProfiles where a.Id == uid select a.FirstName + a.LastName).FirstOrDefault();
                                model.Id = EngagementId;
                                model.EngagementTypes = "Renewal";
                                model.ModifiedBy = uname;
                                model.ModifiedDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("EndDate", "StartDate of Renewal Phase should be Greater than EndDate of Steady State Phase");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("EndDate", "Select StartDate and EndDate of Steady State Phase");
                    }

                }
                else
                {
                    // return JavaScript("alert('EndDate should be Greater than StartDate'); location.reload(true);");
                    ModelState.AddModelError("EndDate", "EndDate should be Greater than StartDate");
                }

            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
          }
          catch (Exception ex)
          {
                Log.Error("Error in EngagementTimeLineController: ClientEngagementCreate ", ex);
                throw ex;
          }
        }

        public static int createClientEngag(EngagementType eng, int clientId, int userId)
        {
            Db db = new Db();
            var _Details = new ClientEngagement();

            using (var contextFactory = new DbContextFactory())
            {
                _Details.EngagementPhaseId =(int) EngType.Renewal;
                _Details.ClientId = clientId;
                _Details.StartDate = eng.StartDate;
                _Details.EndDate = eng.EndDate;
                _Details.CreatedDate = DateTime.Now;
                _Details.CreatedBy = userId;
                _Details.ModifiedBy = userId;
                _Details.ModifiedDate = DateTime.Now;
                db.ClientEngagement.Add(_Details);
                db.SaveChanges();
            }
            return _Details.Id;
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        [HttpPost]
        public ActionResult ClientEngagementUpdate([DataSourceRequest] DataSourceRequest request, EngagementType model)
        {
          try { 
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
            int steadystateId = (int)EngType.SteadyState;
            int baselineId = (int)EngType.Baseline;
            int renewalId = (int)EngType.Renewal;
            if (model != null)
            {


                var stdyDate = (from a in db.ClientEngagement
                                where a.ClientId == client.ClientId && a.EngagementPhaseId == steadystateId
                                select new
                                {
                                    StDate = a.StartDate,
                                    EnDate = a.EndDate
                                }).FirstOrDefault();
                var baseDate = (from a in db.ClientEngagement
                                where a.ClientId == client.ClientId && a.EngagementPhaseId == baselineId
                                select new
                                {
                                    StDate = a.StartDate,
                                    EnDate = a.EndDate
                                }).FirstOrDefault();


                if (model.EndDate >= model.StartDate)
                {
                    ClientEngagement engItem = (from a in db.ClientEngagement where a.ClientId == client.ClientId select a).FirstOrDefault(x => x.Id == model.Id);
                    if (engItem.EngagementPhaseId == renewalId)
                    {
                        if (stdyDate.StDate != null && stdyDate.EnDate != null)
                        {
                            if (model.StartDate > stdyDate.EnDate)
                            {
                                var resDate = (from a in db.ClientEngagement
                                               where client.ClientId == a.ClientId && a.EngagementPhaseId == renewalId && a.Id < model.Id
                                               select new
                                               {
                                                   resId = a.Id,
                                                   resStDate = a.StartDate,
                                                   resEnDate = a.EndDate
                                               }).OrderByDescending(e => e.resId).FirstOrDefault();
                                if (resDate != null)
                                {
                                    if (model.StartDate > resDate.resEnDate)
                                    {
                                        var uname = (from a in db.UserProfiles where a.Id == uid select a.FirstName + a.LastName).FirstOrDefault();
                                        int EngagementId = updateClientEngag(model, client.ClientId, uid);
                                        model.Id = EngagementId;
                                        model.ModifiedBy = uname;
                                        model.ModifiedDate = DateTime.Now;
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("EndDate", "StartDate of Renewal Phase should be Greater than EndDate of Previous Renewal Phase");
                                    }
                                }
                                else
                                {
                                    var uname = (from a in db.UserProfiles where a.Id == uid select a.FirstName + a.LastName).FirstOrDefault();
                                    int EngagementId = updateClientEngag(model, client.ClientId, uid);
                                    model.Id = EngagementId;
                                    model.ModifiedBy = uname;
                                    model.ModifiedDate = DateTime.Now;
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("EndDate", "StartDate of Renewal Phase should be Greater than EndDate of Steady State Phase");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("EndDate", "Select StartDate and EndDate of Steady State Phase");
                        }
                    }
                    else if (engItem.EngagementPhaseId == steadystateId)
                    {
                        if (baseDate.StDate != null && baseDate.EnDate != null)
                        {
                            if (model.StartDate > baseDate.StDate)
                            {
                                var uname = (from a in db.UserProfiles where a.Id == uid select a.FirstName + a.LastName).FirstOrDefault();
                                int EngagementId = updateClientEngag(model, client.ClientId, uid);
                                model.Id = EngagementId;
                                model.ModifiedBy = uname;
                                model.ModifiedDate = DateTime.Now;
                            }
                            else
                            {
                                ModelState.AddModelError("EndDate", "StartDate of Steady State Phase should be Greater than StartDate of BaseLine Phase");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("EndDate", "Select StartDate and EndDate of BaseLine Phase");
                        }

                    }
                    else
                    {
                        var uname = (from a in db.UserProfiles where a.Id == uid select a.FirstName + a.LastName).FirstOrDefault();
                        int EngagementId = updateClientEngag(model, client.ClientId, uid);
                        model.Id = EngagementId;
                        model.ModifiedBy = uname;
                        model.ModifiedDate = DateTime.Now;
                    }
                }
                else
                {
                    ModelState.AddModelError("EndDate", "EndDate should be Greater than StartDate");
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
          }
          catch (Exception ex)
          {
                Log.Error("Error in EngagementTimeLineController: ClientEngagementCreate ", ex);
                throw ex;
          }
        }

        public static int updateClientEngag(EngagementType eng, int clientId, int userId)
        {
            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                ClientEngagement updateitem = (from a in db.ClientEngagement where a.ClientId == clientId select a).FirstOrDefault(x => x.Id == eng.Id);
                DateTime? inSdate = updateitem.StartDate;
                DateTime? inEdate = updateitem.EndDate;
                updateitem.Id = eng.Id;
                updateitem.EngagementPhaseId = updateitem.EngagementPhaseId;
                updateitem.CreatedBy = updateitem.CreatedBy;
                updateitem.CreatedDate = updateitem.CreatedDate;
                updateitem.ClientId = clientId;
                updateitem.StartDate = eng.StartDate;
                updateitem.EndDate = eng.EndDate;
                updateitem.ModifiedBy = userId;
                updateitem.ModifiedDate = DateTime.Now;
                db.SaveChanges();

                if (updateitem.EngagementPhaseId == 5)
                {

                    DateTime? pSdate = eng.StartDate;
                    DateTime? pEdate = eng.EndDate;

                    if (pSdate < inSdate)
                    {
                        setService(clientId, userId, pSdate.Value, inSdate.Value);
                    }
                    else if (pSdate > inSdate)
                    {
                        checkService(clientId, inSdate.Value, pSdate.Value);
                    }
                    if (inEdate < pEdate)
                    {
                        setService(clientId, userId, inEdate.Value, pEdate.Value);
                    }
                    else if (inEdate > pEdate)
                    {
                        checkService(clientId, pEdate.Value, inEdate.Value);
                    }
                    setService(clientId, userId, eng.StartDate.Value, eng.EndDate.Value);
                }
                if (updateitem.EngagementPhaseId ==(int) EngType.ClientContract)
                {
                    DateTime? pSdate = eng.StartDate;
                    DateTime? pEdate = eng.EndDate;
                    if (pSdate < inSdate)
                    {
                        setResource(clientId, userId, pSdate.Value, inSdate.Value);
                    }
                    else if (pSdate > inSdate)
                    {
                        checkResource(clientId, inSdate.Value, pSdate.Value);
                    }
                    if (inEdate < pEdate)
                    {
                        setResource(clientId, userId, inEdate.Value, pEdate.Value);
                    }
                    else if (inEdate > pEdate)
                    {
                        checkResource(clientId, pEdate.Value, inEdate.Value);
                    }
                    setResource(clientId, userId, eng.StartDate.Value, eng.EndDate.Value);
                }
            }
            return eng.Id;
        }

        [HttpPost]
        public JsonResult ClientEngagementDelete([DataSourceRequest] DataSourceRequest request, EngagementType model)
        {
            Db db = new Db();
            int Eid = deleteClientEngagement(model.Id);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public static int deleteClientEngagement(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.ClientEngagement.Delete(Id);
                unitOfWork.Save();
            }
            return Id;
        }

        /*BaseLine Data View*/     
        public BaseLineDataModel getBaseLineData(int cid)        
        {
            Db db = new Db();
            var uid = (int)Session["UserId"];
            var bldata = new BaseLineDataModel();
            var bsData = new BaseLineData();
            bsData = (from a in db.BaseLineData
                     where a.ClientId == cid
                     select a).FirstOrDefault();
            if (bsData == null)
            {
                var bData = new BaseLineData();
                var BaseData = (from a in db.ClientEngagement
                                where a.ClientId == cid && a.EngagementPhaseId == 4
                                select new
                                {
                                    StDate = a.StartDate,
                                    EnDate = a.EndDate
                                }).FirstOrDefault();

                if (BaseData == null || (BaseData.StDate == null && BaseData.EnDate == null))
                {
                    bData.EngBaseLinePeriod = "-";                  
                }
                else
                {
                    var sdate = BaseData.StDate.Value.ToString("MMM dd yyyy");
                    var edate = BaseData.EnDate.Value.ToString("MMM dd yyyy");
                    bData.EngBaseLinePeriod = sdate + " to " + edate;
                }
                bData.MonthlyINCount = 0;
                bData.MonthlyEventCount = 0;
                bData.MonthlySRCount = 0;
                bData.ClientId = cid;
                bData.CreatedBy = uid;
                bData.CreatedDate = DateTime.Now;
                db.BaseLineData.Add(bData);
                db.SaveChanges();
                bsData = bData;
            }
            bldata.BaseLinePeriod = bsData.EngBaseLinePeriod;
            bldata.IncMonth = bsData.MonthlyINCount;
            bldata.EveMonth = bsData.MonthlyEventCount;
            bldata.SerReqMonth = bsData.MonthlySRCount;
            return bldata;
        }

        
        public ActionResult getBaseLineINData()
        {
          try { 
            Db db = new Db();
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;
            BaseLineDataModel bData = new BaseLineDataModel();
            var BaseData = (from a in db.ClientEngagement
                            where a.ClientId == cid && a.EngagementPhaseId == 4
                            select new
                            {
                                StDate = a.StartDate,
                                EnDate = a.EndDate
                            }).FirstOrDefault();

            if (BaseData == null || (BaseData.StDate == null && BaseData.EnDate == null))
            {
                bData.BaseLinePeriod = "-";
                bData.IncMonth = 0;
                bData.EveMonth = 0;
                bData.SerReqMonth = 0;
                //   bData.resDetails = ResourceDetails();
                return Json(bData);
            }
            else
            {
                var InPerMon = (from a in db.Incidents
                                where a.ClientId == cid && (a.CreatedDateTime >= BaseData.StDate && a.CreatedDateTime <= BaseData.EnDate) && (a.Type.StartsWith("in") || a.Type.StartsWith("IN"))
                                select a).Count();
                var EvPerMon = (from a in db.Incidents
                                where a.ClientId == cid && (a.CreatedDateTime >= BaseData.StDate && a.CreatedDateTime <= BaseData.EnDate) && (a.Type == "event" || a.Type == "Event")
                                select a).Count();
                var SRPerMon = (from a in db.Incidents
                                where a.ClientId == cid && (a.CreatedDateTime >= BaseData.StDate && a.CreatedDateTime <= BaseData.EnDate) && (a.Type.StartsWith("sr") || a.Type.StartsWith("SR") || (a.Type == "service request" || a.Type == "Service Request"))
                                select a).Count();
                var sdate = BaseData.StDate.Value.ToString("MMM dd yyyy");
                var edate = BaseData.EnDate.Value.ToString("MMM dd yyyy");
                bData.BaseLinePeriod = sdate + " to " + edate;
                bData.IncMonth = InPerMon;
                bData.EveMonth = EvPerMon;
                bData.SerReqMonth = SRPerMon;
                //   bData.resDetails = ResourceDetails();
                return Json(bData);
            }
          }
          catch (Exception ex)
          {
                Log.Error("Error in EngagementTimeLineController: getBaseLineINData ", ex);
                throw ex;
          }
        }

        [HttpPost]
        public BaseLineDataModel UpdateBaseLineData(string uid)
        {
          try { 
            var client = (ClientDetail)Session["SessionClient"];
            var userid = (int)Session["UserId"];
            var allBaseValues = uid.Split('|');
            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                int incmonth=0;
                int evemonth=0;
                int srmonth=0;
                var baseperiod="";
                foreach (string item in allBaseValues)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var idvalues = item.Split(',');
                        var id = idvalues[0];
                        var val = idvalues[1];
                       switch(id)
                        {
                            case "BaseLinePeriod":
                                baseperiod = val;
                                break;
                            case "IncMonth":
                                incmonth = Convert.ToInt32(val);
                                break;
                            case "EveMonth":
                                evemonth = Convert.ToInt32(val);
                                break;
                            case "SRMonth":
                                srmonth = Convert.ToInt32(val);
                                break;                                                                    
                        }
                    }
                }
                BaseLineData updateitem = (from a in db.BaseLineData where a.ClientId == client.ClientId select a).FirstOrDefault();               
                updateitem.ClientId = client.ClientId;
                updateitem.EngBaseLinePeriod = baseperiod;
                updateitem.MonthlyINCount = incmonth;
                updateitem.MonthlyEventCount = evemonth;
                updateitem.MonthlySRCount = srmonth;
                updateitem.ModifiedBy = userid;
                updateitem.ModifiedDate = DateTime.Now;  
                db.SaveChanges();
            }

            return null;
          }
          catch (Exception ex)
          {
                Log.Error("Error in EngagementTimeLineController: UpdateBaseLineData ", ex);
                throw ex;
          }
        }
   
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult ResourceDetails()
        {
          try { 
            Db db = new Db();
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];

            //Resource Details Data

            var contDate = (from a in db.ClientEngagement
                            where client.ClientId == a.ClientId && a.EngagementPhaseId == (int)EngType.ClientContract
                            select a).FirstOrDefault();
            var stdyDate = (from a in db.ClientEngagement
                            where client.ClientId == a.ClientId && a.EngagementPhaseId == (int)EngType.SteadyState
                            select a).FirstOrDefault();
            List<int> YearList = new List<int>();


            if (contDate.StartDate != null && stdyDate.EndDate != null)
            {
                DateTime sDate = contDate.StartDate.Value;
                DateTime ssDate = contDate.StartDate.Value;
                DateTime eDate = stdyDate.EndDate.Value;

                while (sDate <= eDate)
                {
                    int year = sDate.Year;
                    YearList.Add(year);
                    sDate = sDate.AddMonths(1);
                }
                setResource(client.ClientId, uid, ssDate, eDate);
            }

            List<int> distinctYearList = new List<int>();

            distinctYearList = YearList.Select(x => x).Distinct().ToList();

            IList<ResourceDetailsModel> ResDetails = getResource(client.ClientId);

            List<string> distinctLocation = (from a in db.EngagementLocation
                                             select a.Location).Distinct().OrderBy(e => e).ToList();
            if (distinctLocation.Count == 0)
            {
                foreach (Location item in Enum.GetValues(typeof(Location)))
                {
                    EngagementLocation engLoc = new EngagementLocation();
                    engLoc.Id = (int)item;
                    engLoc.Location = item.ToString();
                    db.EngagementLocation.Add(engLoc);
                    db.SaveChanges();
                }
            }
            ViewBag.Year = distinctYearList;
            ViewBag.location = distinctLocation;
            return PartialView("_ResourceDetails", ResDetails);
          }
          catch (Exception ex)
          {
                Log.Error("Error in EngagementTimeLineController: ResourceDetails ", ex);
                throw ex;
          }
        }

        [HttpPost]
        public JsonResult UpdateClientFTE(string uid)
        {
          try { 
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var userid = (int)Session["UserId"];
            Db db = new Db();
            var allIdnValues = uid.Split('|');
            using (var contextFactory = new DbContextFactory())
            {
                foreach (string item in allIdnValues)
                {
                    if (!string.IsNullOrEmpty(item))
                    {                       
                        var idvalues = item.Split(',');
                        if (!(idvalues[0]== "IncMonth" || idvalues[0] == "EveMonth" || idvalues[0] == "SRMonth"))
                        {
                            var id = Convert.ToInt32(idvalues[0]);
                            var val = Convert.ToDecimal(idvalues[1]);
                            ClientEngagementFTE_Q updateitem = (from a in db.ClientEngagementFTE_Q where a.ClientId == client.ClientId select a).FirstOrDefault(x => x.Id == id);
                            updateitem.Id = id;
                            updateitem.ClientId = client.ClientId;
                            updateitem.FTE = val;
                            updateitem.ModifiedBy = userid;
                            updateitem.ModifiedDate = DateTime.Now;
                            db.SaveChanges();
                        }
                        
                    }

                }
            }

            return Json("Resource Details updated successfully");
          }
          catch (Exception ex)
          {
                Log.Error("Error in EngagementTimeLineController: UpdateClientFTE ", ex);
                throw ex;
          }
        }

        private IList<ResourceDetailsModel> GetResourceDetails()
        {
            Db db = new Db();
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];

            //Resource Details Data

            var contDate = (from a in db.ClientEngagement
                            where client.ClientId == a.ClientId && a.EngagementPhaseId == 1
                            select a).FirstOrDefault();
            List<int> YearList = new List<int>();
            if (contDate.StartDate!=null&&contDate.EndDate!=null)
            {
                DateTime sDate;
                DateTime ssDate = contDate.StartDate.Value;
                DateTime eDate;
                var resDate = (from a in db.ClientEngagement
                               where client.ClientId == a.ClientId && a.EngagementPhaseId == 6
                               select new
                               {
                                   resId = a.Id,
                                   resStDate = a.StartDate,
                                   resEnDate = a.EndDate
                               }).OrderByDescending(e => e.resId).FirstOrDefault();
                if (resDate != null)
                {
                    sDate = contDate.StartDate.Value;
                    eDate = resDate.resEnDate.Value;
                    while (sDate <= eDate)
                    {
                        int year = sDate.Year;
                        YearList.Add(year);
                        sDate = sDate.AddMonths(1);
                    }
                }
                else
                {
                    sDate = contDate.StartDate.Value;
                    eDate = contDate.EndDate.Value;
                    while (sDate <= eDate)
                    {
                        int year = sDate.Year;
                        YearList.Add(year);
                        sDate = sDate.AddYears(1);
                    }
                }
                setResource(client.ClientId, uid, ssDate, eDate);
            }

            List<int> distinctYearList = new List<int>();

            distinctYearList = YearList.Select(x => x).Distinct().ToList();

            IList<ResourceDetailsModel> ResDetails = getResource(client.ClientId);

            List<string> distinctLocation = (from a in db.EngagementLocation
                                             select a.Location).Distinct().OrderBy(e => e).ToList();
            if (distinctLocation.Count == 0)
            {
                foreach (Location item in Enum.GetValues(typeof(Location)))
                {
                    EngagementLocation engLoc = new EngagementLocation();
                    engLoc.Id = (int)item;
                    engLoc.Location = item.ToString();
                    db.EngagementLocation.Add(engLoc);
                    db.SaveChanges();
                }
            }
            ViewBag.Year = distinctYearList;
            ViewBag.location = distinctLocation;
            return (ResDetails);
        }

        /*Resource Details View*/
        public static IList<ResourceDetailsModel> getResource(int clientid)
        {
            IList<ResourceDetailsModel> Engagementplans = new List<ResourceDetailsModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<ResourceDetailsModel> ResourceDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();

                ResourceDetails = (from a in db.ClientEngagementFTE_Q
                                   join b in db.EngagementLocation on a.LocationId equals b.Id
                                   where a.ClientId == clientid
                                   select new ResourceDetailsModel
                                   {
                                       Id = a.Id,
                                       Location = b.Location,
                                       Quarter = a.Quarter,
                                       Year = a.Year,
                                       FTE = a.FTE,
                                   }).OrderBy(e => e.Year).ThenBy(e => e.Location).ThenBy(e => e.Quarter);

                if (ResourceDetails != null)
                {
                    foreach (var item in ResourceDetails)
                    {
                        ResourceDetailsModel rd = new ResourceDetailsModel();
                        rd.Id = item.Id;
                        rd.Location = item.Location;
                        rd.Year = item.Year;
                        rd.Quarter = item.Quarter;
                        rd.FTE = item.FTE;
                        Engagementplans.Add(rd);
                    }
                }
            }
            return Engagementplans;
        }

        /*Seting ServicePlan for Steady State period */
        public static void setService(int clientid, int uid, DateTime sdate, DateTime edate)
        {
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
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
                            createplan(clientid, uid, "1", yea);
                            monstart = 4;
                        }
                        else if (monstart >= 4 && monstart <= 6)
                        {
                            createplan(clientid, uid, "2", yea);
                            monstart = 7;
                        }
                        else if (monstart >= 7 && monstart <= 9)
                        {
                            createplan(clientid, uid, "3", yea);
                            monstart = 10;
                        }
                        else if (monstart >= 10 && monstart <= 12)
                        {
                            createplan(clientid, uid, "4", yea);
                            monstart = 13;
                        }
                    }                     
                 }                   
            }
        }
        
        public static void createplan(int cid, int uid, string quater, int year)
        {
            Db db = new Db();

            var sTypeGoal = (from a in db.ServiceImprovementType
                             join b in db.ServiceImprovementGoal on a.Id equals b.ImprovementTypeId
                             where cid == b.ClientId
                             select b);
            foreach (var item in sTypeGoal)
            {
                List<ServiceImprovementPlan_Q> spq = (from a in db.ServiceImprovementPlan_Q
                                                      where a.ClientId == cid && a.Quarter == quater && year == a.Year && a.ServiceImprovementGoalId == item.Id
                                                      select a).ToList();
                if (spq.Count == 0)
                {
                    var _Details = new ServiceImprovementPlan_Q();

                    _Details.ClientId = cid;
                    _Details.ServiceImprovementGoalId = item.Id;
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
        }

        public static void checkService(int clientid, DateTime sdate, DateTime edate)
        {
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
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
                        List<ServiceImprovementPlan_Q> spq = (from a in db.ServiceImprovementPlan_Q
                                                              where a.ClientId == clientid && a.Quarter == quarter && yea == a.Year
                                                              select a).ToList();
                        deleteservice(spq);                       
                    }
                }
            }
        }

        public static void deleteservice(List<ServiceImprovementPlan_Q> list)
        {
            foreach (var item in list)
            {
                if (item.Goal == 0M)
                {
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        unitOfWork.ServiceImprovementPlan_Q.Delete(item.Id);
                        unitOfWork.Save();
                    }
                }
            }
        }

        /*Seting ResourceDetails for Client Contract period */
        public static void setResource(int clientid, int uid, DateTime sdate, DateTime edate)
        {
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();

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
                            createResource(clientid, uid, "1", yea);
                            monstart = 4;
                        }
                        else if (monstart >= 4 && monstart <= 6)
                        {
                            createResource(clientid, uid, "2", yea);
                            monstart = 7;
                        }
                        else if (monstart >= 7 && monstart <= 9)
                        {
                            createResource(clientid, uid, "3", yea);
                            monstart = 10;
                        }
                        else if (monstart >= 10 && monstart <= 12)
                        {
                            createResource(clientid, uid, "4", yea);
                            monstart = 13;
                        }
                    }
                }
                int dYear = edate.Year;              
                List<ClientEngagementFTE_Q> deleteRes = new List<ClientEngagementFTE_Q>();
                if (edate.Month >= 1 && edate.Month <= 3)
                {               
                    deleteRes = (from a in db.ClientEngagementFTE_Q
                                 where a.ClientId == clientid && a.Year >= dYear && (a.Quarter == "2" || a.Quarter == "3" || a.Quarter == "4")
                                 select a).ToList();
                }
                else if (edate.Month >= 4 && edate.Month <= 6)
                {              
                    deleteRes = (from a in db.ClientEngagementFTE_Q
                                 where a.ClientId == clientid && a.Year >= dYear && (a.Quarter == "3" || a.Quarter == "4")
                                 select a).ToList();
                }
                else if (edate.Month >= 7 && edate.Month <= 10)
                {                
                    deleteRes = (from a in db.ClientEngagementFTE_Q
                                 where a.ClientId == clientid && a.Year >= dYear && (a.Quarter == "4")
                                 select a).ToList();
                }
                else if (edate.Month >= 11 && edate.Month <= 12)
                {                 
                    deleteRes = (from a in db.ClientEngagementFTE_Q
                                 where a.ClientId == clientid && a.Year > dYear && (a.Quarter == "1" || a.Quarter == "2" || a.Quarter == "3" || a.Quarter == "4")
                                 select a).ToList();
                }
                deleteResource(deleteRes);
            }
        }

        public static void createResource(int cid, int uid, string quater, int year)
        {
            Db db = new Db();
            var engLoc = (from a in db.EngagementLocation
                          select a.Id);

           var rsd = (from a in db.ClientEngagementFTE_Q
                                               where a.ClientId == cid && a.Quarter == quater && year == a.Year 
                                               select a.LocationId).ToList();
            foreach (var item in engLoc)
            {
                //.ToList();
                if (rsd.Select(s => s == item).ToList().Count == 0)
                {
                    var _Details = new ClientEngagementFTE_Q();
                    _Details.ClientId = cid;
                    _Details.LocationId = item;
                    _Details.Year = year;
                    _Details.Quarter = quater;
                    _Details.FTE = 0M;
                    _Details.CreatedBy = uid;
                    _Details.ModifiedBy = uid;
                    _Details.CreatedDate = DateTime.Now;
                    _Details.ModifiedDate = null;
                    db.ClientEngagementFTE_Q.Add(_Details);
                    db.SaveChanges();
                }
            }
        }

        public static void checkResource(int clientid, DateTime sdate, DateTime edate)
        {
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
               for(int yea=sdate.Year; yea<=edate.Year;yea++)
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
                        List<ClientEngagementFTE_Q> rds = (from a in db.ClientEngagementFTE_Q
                                                           where a.ClientId == clientid && a.Quarter == quarter && yea == a.Year
                                                           select a).ToList();
                        deleteResource(rds);
                    }                   
                }
            }
        }

        public static void deleteResource(List<ClientEngagementFTE_Q> list)
        {
            foreach (var item in list)
            {              
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        unitOfWork.ClientEngagementFTE_Q.Delete(item.Id);
                        unitOfWork.Save();
                    }
            }
        }
    }
}
