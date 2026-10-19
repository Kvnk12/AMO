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
    public class KEDBController : Controller
    {
        //
        // GET: /IncidentNavigator/KEDB/
        private static readonly ILog Log = LogManager.GetLogger(typeof(KEDBController));

        public static int[] TowerList;
        public static int[] IssueList;
        public static int[] SolutionCategoryList;
        public static int[] SolutionStatusList;

        [ActionTrack]
        public ActionResult Index(string Clause)
        {
            
            KEDBInstanceModel c = prepareCommonParameters();
            c.Clause = Clause;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            var Cl = (ClientDetail)Session["SessionClient"];
            Log.Debug("KEDB Index page , Client ID is " +Cl.ClientId);
           
            KEDBInstanceModel kedbView = new KEDBInstanceModel();
            Db db = new Db();

            //kedbView = IncidentNavigatorHelper.viewKedb(Cl.ClientId);
            kedbView = (from a in db.KEDBInstance
                        join b in db.KEDBIncident on a.Id equals b.KEDBID
                        join d in db.Incidents on b.IncidentID equals d.Id
                        select
                        new KEDBInstanceModel
                        {
                            TicketId = d.IncidentId,
                            Description = a.Description,
                            KEDBNumber = a.KEDBNumber,
                            IssueTypeId = a.IssueTypeId,
                            BusinessFunctionId = a.BusinessFunctionId,
                            SolutionCategoryId = a.SolutionCategoryId,
                            KnowledgeArticle = a.KnowledgeArticle,
                            SolutionStatusId = a.SolutionStatusId,
                            PointOfContact = a.PointOfContact,

                        }).FirstOrDefault();

            return View(c);
        }

        public ActionResult KedbSolStatusReport()
        {
            var Cl = (ClientDetail)Session["SessionClient"];

            Log.Debug("KEDB Index page , Client ID is " + Cl.ClientId);
            Log.Info("KEDB Index page , Client ID is " + Cl.ClientId);
            Log.Warn("KEDB Index page , Client ID is " + Cl.ClientId);
            KEDBInstanceModel mod = prepareReportParameters();

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

            ViewBag.userEmailList = userEmailList;
            return View("KedbSolStatusReport", mod);

        }

        public ActionResult getContractId(int Accid = 0)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            Db db = new Db();
            var dispSln = (from acc in db.Incidents
                           join con in db.ClientAccountContract on acc.ContractId equals con.Id
                           where con.AccountId == Accid && acc.ClientId == ClientId
                           select new
                           {
                               Id = con.Id,
                               ContractName = con.ContractName
                           }).ToList().Distinct();
            return Json(dispSln.ToList().Select(A => new { Value = A.Id.ToString(), Text = A.ContractName, Selected = true }), JsonRequestBehavior.AllowGet);

        }
        public static void GetFilterInfoKEDB(KEDBInstanceModel mod, int ClientId)
        {
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                DataSet ds = new DataSet();


                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllFilterInfo", paramName, paramValue);

                List<SelectListItem> ls;
                int i = 0;
                for (int t = 0; t < ds.Tables.Count; t++)
                {
                    i = 0;
                    ls = new List<SelectListItem>();
                    if (t < 10)
                    {
                        foreach (DataRow row in ds.Tables[t].Rows)
                        {
                            ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = i.ToString() });
                            i++;
                        }
                    }
                    else
                    {
                        foreach (DataRow row in ds.Tables[t].Rows)
                        {
                            ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = row.ItemArray[1].ToString() });
                            i++;
                        }
                    }
                    switch (t)
                    {

                        case 4:
                            mod.TowerList = ls;
                            break;
                       
                        case 10:
                            mod.AccountNamesList = ls;
                            break;
                        case 11:
                            mod.ContractList = ls;
                            break;
}

                }

                //Issue type
                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchIssueType", paramName, paramValue);
                i = 0;
                ls = new List<SelectListItem>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = i.ToString() });
                    i++;
                }
                mod.IssueTypeList = ls;

                // Category
                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchSolutionCategory", paramName, paramValue);

                i = 0;
                ls = new List<SelectListItem>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = i.ToString() });
                    i++;
                }
                mod.SolutionCategoryList = ls;

                // Status

                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchSolutionStatus", paramName, paramValue);

                i = 0;

                ls = new List<SelectListItem>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = i.ToString() });
                    i++;
                }
                mod.SolutionStatusList = ls;


                return;
            }

        }



        private KEDBInstanceModel prepareCommonParameters()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            KEDBInstanceModel mod = new KEDBInstanceModel();
            HomeInput input = (HomeInput)Session["MenuItemsInput"];
            var uid = (int)Session["UserId"];
            GetDrpdwnInfo(mod, client.ClientId, input, uid);

            return mod;
        }

        KEDBInstanceModel mod = new KEDBInstanceModel();
        private KEDBInstanceModel prepareReportParameters()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
           
            mod.SelectedTowerList = TowerList;
            mod.SelectedIssueTypeList = IssueList;
            mod.SelectedCategoryList = SolutionCategoryList;
            mod.SelectedStatusList = SolutionStatusList;
            mod.ClientId = client.ClientId;
            GetFilterInfoKEDB(mod, client.ClientId);
            

            mod.ToYear = DateTime.Today.Date;
            mod.FromYear = DateTime.Today.Date.AddMonths(-3);
            int uid = (int)Session["UserId"];
            mod.ClientName = client.ClientName;
            mod.UserName = IdeaManager.IMHelper.GetUserName(uid);
            mod.CreatedDt = DateTime.Now;
            mod.DateRange = mod.FromYear.Value.ToString("MM/dd/yyyy") + " To " + mod.ToYear.Value.ToString("MM/dd/yyyy");


            return mod;
        }

      

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveReportDetails( int[] TowerDropDown, int[] IssueDropDown, int[] CategoryDropDown, int[] StatusDropDown)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
     
          /*  TowerList = TowerDropDown;
            IssueList = IssueDropDown;
            if(CategoryDropDown != null)
                SolutionCategoryList = CategoryDropDown;
            if (StatusDropDown != null)
                SolutionStatusList = StatusDropDown;
          */
            return Json(0);
        }

        public static void GetDrpdwnInfo(KEDBInstanceModel mod, int ClientId, HomeInput input, int uid)
        {
            //IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                DataSet ds = new DataSet();
               // var cont = new KEDBController();

                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllDrpdwnforKEDB", paramName, paramValue);

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
                            mod.IssueTypeList = ls;
                            break;
                        case 1:
                            mod.BusinessFunctionList = ls;
                            break;
                        case 2:
                            mod.SolutionCategoryList = ls;
                            break;
                        case 3:

                            ////if (cont.IsKedbManagerOrSup(input.UserRole.ToLower(), ClientId, uid) == "true")
                            ////{
                            ////    mod.SolutionStatusList = ls;
                            ////}
                            ///
                            if (App_Start.Helper.HasRole( (int) App_Start.Helper.AMOApplicationName.IncidentNavigator, (int)App_Start.Helper.AMOUserRoles.KEDBManager ))
                            {
                                mod.SolutionStatusList = ls;
                            }
                            else
                            {
                                mod.SolutionStatusList = getuserSolStatus();
                            }
                            break;
                    }
                }
                return;
            }
        }
        

        ////public string IsKedbManagerOrSup(string userRole, int clientid, int uid)
        ////{
        ////    if (userRole == "superuser")
        ////    {
        ////        return "true";
        ////    }
        ////    else
        ////    {
        ////        Db db = new Db();
        ////        var user = (from a in db.UserApplications
        ////                    where a.UserId == uid & a.ClientId == clientid & a.RoleId == 8 & a.IsDeleted == false
        ////                    select a).FirstOrDefault();
        ////        if (user != null)
        ////        {
        ////            return "true";
        ////        }
        ////        else
        ////            return "false";
        ////    }
        ////}
        public static List<SelectListItem> getuserSolStatus()
        {
            Db db = new Db();
            var listOfAdmins = (from a in db.KEDBSolutionStatus
                                where a.Status.ToLower() != "approved" && a.Status.ToLower() != "retired"
                                select a).Distinct().ToList();
            var solstatus = listOfAdmins.Select(g =>
                new SelectListItem
                {
                    Text = g.Status,
                    Value = g.Id.ToString()
                }
                ).ToList();
            //return Json(new { Result = "OK", Records = AdminsWithName });
            return solstatus;
        }
        [ActionTrack]
        [HttpPost]
        public ActionResult GetKEDB([DataSourceRequest] DataSourceRequest request, string clause)
        {
            //int usid = (int)Session["UserId"];
            try
            {
                var Cl = (ClientDetail)Session["SessionClient"];
                Db db = new Db();

                //klist = (from a in db.KEDB
                //                select new KEDBModel
                //                {
                //                   Cause = a.Cause,
                //                   ResolutionSteps=a.ResolutionSteps
                //                }).ToList();

                IEnumerable<KEDBInstanceModel> klist = IncidentNavigatorHelper.getKEDB(Cl.ClientId, clause);

                Log.Debug("Fetch KEDB's in Grid  , Client ID is " + Cl.ClientId);

                return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetKEDB", ex);
                throw ex;
            }
        }


        public ActionResult KEDBDataSource([DataSourceRequest] DataSourceRequest request)
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
                      

            IEnumerable<KEDBInstanceModel> klist = IncidentNavigatorHelper.getKEDB(Cl.ClientId);

            return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ActionTrack]
        public ActionResult GenerateKedbId(string Ids)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            KEDBInstanceModel km = new KEDBInstanceModel();
            var res = IncidentNavigatorHelper.GenerateKEDB(client.ClientId);
            km.Id = res;
            km.KEDBNumber = unitOfWork.KEDBInstance.GetByID(res).KEDBNumber;
            km.klist = IncidentNavigatorHelper.getKEDBTicketById(Ids);
            km.Incid = Ids;
           // km.klist = IncidentNavigatorHelper.InsertKEDBIncidents(Ids, res, uid);
            return Json(km, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ActionTrack]
        public ActionResult SaveKEDBNumber(KEDBInstanceModel km)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            km.UpdatedBy = usid;
            km.ClientId = client.ClientId;
            var res = IncidentNavigatorHelper.UpdateKEDBNumber(km);
            return Json(km, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        [ActionTrack]
        public ActionResult DeleteKedbNumber(int delId)
        {
            var res = IncidentNavigatorHelper.DeleteKEDBNumber(delId);
            return Json(res, JsonRequestBehavior.AllowGet);

        }

       
        [HttpGet]
        public JsonResult viewKEDBDetails(int KEDBID)
        {
            //This method is used to fetch kedb details on click of view icon in kedb search grid

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();           
            //Below Function call returns KEDB Detail from Stored Procedure 
            IEnumerable<KEDBInstanceModel> kedbDetail = IncidentNavigatorHelper.ViewKEDB(client.ClientId, KEDBID);
            var e = kedbDetail.FirstOrDefault();
            var updatedBy =(e.UpdatedBy!=0)?e.UpdatedBy:0;      
            DateTime updatedDtTime = e.UpdatedDate;

            var Email = (from b in db.UserProfiles
                          where b.Id == updatedBy
                         select b.Email).FirstOrDefault();

           var UpdatedDateTime = updatedDtTime.ToString("MM/dd/yyyy");

            KEDBInstanceModel kedbval = new KEDBInstanceModel();
            kedbval.Id = e.Id ;
            kedbval.ClientId = e.ClientId; 
            kedbval.IncidentId = e.IncidentId; 
            kedbval.PointOfContact = e.PointOfContact;
            kedbval.KnowledgeArticle = e.KnowledgeArticle;
            kedbval.Description = e.Description;
            kedbval.KEDBNumber = e.KEDBNumber;
            kedbval.SolutionCategory = e.SolutionCategory;
            kedbval.SolStatus = e.SolStatus;
            kedbval.BusinessFunction = e.BusinessFunction;
            kedbval.KedbIssueType = e.KedbIssueType;
            kedbval.UpdatedUser = Email;
            kedbval.UpdatedDateTime= UpdatedDateTime;
            kedbval.ResolutionSteps = e.ResolutionSteps;
            kedbval.FileName = e.FileName;

            return Json(kedbval, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Assign()
        {
            KEDBInstanceModel km = new KEDBInstanceModel();
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View("AssignKEDB", km);
        }
        [ActionTrack]
        [HttpPost]
        public ActionResult GetKEDBIncidentDetail([DataSourceRequest] DataSourceRequest request, int kedbid,string Selected_TicketID)
        {
            Log.Debug("Selected Incident IDs " + Selected_TicketID);
            var Cl = (ClientDetail)Session["SessionClient"];
            IEnumerable<KEDBInstanceModel> klist = null;
            if (Selected_TicketID=="")
                 klist = IncidentNavigatorHelper.getKEDBTicket(Cl.ClientId, kedbid);
            else
                 klist = IncidentNavigatorHelper.getKEDBSelTicket(Cl.ClientId, kedbid, Selected_TicketID);

            return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        //public ActionResult GetKEDBIncidentDetailView([DataSourceRequest] DataSourceRequest request, int kedbid)
        //{
       //    var Cl = (ClientDetail)Session["SessionClient"];
         //    IEnumerable<KEDBInstanceModel> klist = IncidentNavigatorHelper.getKEDBTicket(Cl.ClientId, kedbid);

         //    return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

       //}



        [HttpGet]
        [ActionTrack]
        public ActionResult AddTicketsToKEDB(string Ids, int kedbID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            KEDBIncidentModel initialKEDBDetails = new KEDBIncidentModel();
            initialKEDBDetails = (from a in db.KEDBIncident
                                  where a.KEDBID == kedbID
                                  select new KEDBIncidentModel
                                  {
                                      Id = a.Id,
                                      KEDBID = a.KEDBID,
                                      IncidentID = a.IncidentID,
                                      CreatedDate = a.CreatedDate,
                                      UpdatedDate = a.UpdatedDate,
                                      CreatedBy = a.CreatedBy.Value,
                                      UpdatedBy = a.UpdatedBy.Value
                                  }).FirstOrDefault();

            Ids = Ids.Substring(1, Ids.Length - 2);
            string[] strINs = Ids.Split(new char[] { ',' });
            List<int> nums = new List<int>();
            foreach (var inID in strINs)
            {
                int id = Convert.ToInt32(inID);
                int res = IncidentNavigatorHelper.checkKEDBIncident(id);
                if (res == 0)
                {
                    nums.Add(id);
                }
            }

            var ret = IncidentNavigatorHelper.ADDKEDBIncidents(initialKEDBDetails, nums);

            return Json(ret, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionTrack]
        public ActionResult EditKedbId(int KEDBID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
            KEDBInstanceModel kd = IncidentNavigatorHelper.EditKEDBInsDetails(KEDBID, client.ClientId);
           
            return Json(kd, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ActionTrack]
        public ActionResult SaveEditedKEDBDetails(KEDBInstanceModel kd)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            kd.UpdatedBy = usid;
            kd.ClientId = client.ClientId;
            var res = IncidentNavigatorHelper.UpdateKEDBDetails(kd, usid, client.ClientId);
            return Json(kd, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ActionTrack]
        public ActionResult DelinkKEDBDetails(int kedbId, int ticketid)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
           
            var res = IncidentNavigatorHelper.DelinkKEDBDetails(kedbId, ticketid, usid, client.ClientId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult viewDelinkKEDBDetails(int Id, int KEDBID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var uid = (int)Session["UserId"];

            KEDBInstanceModel kedbval = (from a in db.KEDBInstance
                                         join b in db.KEDBIncident on a.Id equals b.KEDBID
                                         join c in db.Incidents.DefaultIfEmpty() on b.IncidentID equals c.Id
                                         join d in db.KEDBSolutionCategory.DefaultIfEmpty() on a.SolutionCategoryId equals d.Id into ps
                                         from d in ps.DefaultIfEmpty()
                                         join e in db.KEDBIssueType.DefaultIfEmpty() on a.IssueTypeId equals e.Id into ps1
                                         from e in ps1.DefaultIfEmpty()
                                         join f in db.KEDBSolutionStatus.DefaultIfEmpty() on a.SolutionStatusId equals f.Id into ps2
                                         from f in ps2.DefaultIfEmpty()
                                         join g in db.KEDBBusinessFunction.DefaultIfEmpty() on a.BusinessFunctionId equals g.Id into ps3
                                         from g in ps3.DefaultIfEmpty()
                                         where c.Id == Id && c.ClientId == client.ClientId && b.KEDBID == KEDBID
                                         select new KEDBInstanceModel
                                         {
                                             Id = a.Id,
                                             TicketId = c.IncidentId,
                                             ClientId = c.ClientId,
                                             IncidentId = c.Id,
                                             PointOfContact = a.PointOfContact,
                                             KnowledgeArticle = a.KnowledgeArticle,
                                             Description = a.Description,
                                             TicketDescription = c.Description,
                                             KEDBNumber = a.KEDBNumber,
                                             SolutionCategory = d.SolutionCategory,
                                             SolStatus = f.Status,
                                             BusinessFunction = g.BusinessFunction,
                                             KedbIssueType = e.KEDBIssueType,
                                             UpdatedBy = a.UpdatedBy,
                                             UpdatedDate = a.UpdatedDate,
                                             FileName = a.FileName,
                                             ResolutionSteps = a.ResolutionSteps
                                         }
                                             ).FirstOrDefault();
            kedbval.UpdatedUser = (from b in db.UserProfiles
                                   where b.Id == kedbval.UpdatedBy
                                   select b.Email).FirstOrDefault();
            kedbval.UpdatedDateTime = kedbval.UpdatedDate.ToString("MM/dd/yyyy");
            return Json(kedbval, JsonRequestBehavior.AllowGet);
        }
        public ActionResult KedbMetaData()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View();
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult GetIssueType([DataSourceRequest] DataSourceRequest request)
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IncidentNavigatorHelper.GetIssueTypeDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddIssueType(KEDBIssueTypeModel issueTypeModel, int Id)
        {

            if (issueTypeModel != null && issueTypeModel.IssueType.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                issueTypeModel.ClientId = Client.ClientId;
                var res = IncidentNavigatorHelper.CreateIssueType(issueTypeModel, Id,usid);
                issueTypeModel.Id = res;
            }       
            return Json(new[] { issueTypeModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditIssueType([DataSourceRequest] DataSourceRequest request, KEDBIssueTypeModel issueTypeModel)
        {
            if (issueTypeModel != null && issueTypeModel.IssueType.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                issueTypeModel.ClientId = Client.ClientId;
                var res = IncidentNavigatorHelper.UpdateIssueType(issueTypeModel,usid);
            }
            return Json(new[] { issueTypeModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteIssueType([DataSourceRequest] DataSourceRequest request, KEDBIssueTypeModel issueTypeModel)
        {
            Db db = new Db();
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            var kedbInstanceIDList = (from a in db.KEDBInstance
                                     where a.ClientId == Client.ClientId
                                     select a.IssueTypeId).ToList();
            var kedbIssueTypeList = (from a in db.KEDBIssueType
                                     where a.ClientId == Client.ClientId
                                     select a).ToList();
            if (kedbInstanceIDList.Contains(issueTypeModel.Id))
            {
                ModelState.AddModelError("IssueType", "Selected Issue Type is used by a KEDB, can’t be deleted");
                return Json(new[] { kedbIssueTypeList }.ToDataSourceResult(request, ModelState));
            }
            else if (issueTypeModel != null)
            {
                var v = IncidentNavigatorHelper.RemoveIssueType(issueTypeModel.Id);
            }

            return Json(new[] { issueTypeModel }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult GetBusSolu([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IncidentNavigatorHelper.GetBusFunctionDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddBusSolu(KEDBBusinessFunctionModel busFunctModel, int Id)
        {

            if (busFunctModel != null && busFunctModel.BusinessFunction.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                busFunctModel.ClientId = Client.ClientId;
                var res = IncidentNavigatorHelper.CreateBusFunction(busFunctModel, Id,usid);
                busFunctModel.Id = res;
            }
            return Json(new[] { busFunctModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditBusSolu([DataSourceRequest] DataSourceRequest request, KEDBBusinessFunctionModel busFunctModel)
        {
            if (busFunctModel != null && busFunctModel.BusinessFunction.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                busFunctModel.ClientId = Client.ClientId;
                var res = IncidentNavigatorHelper.UpdateBusFunction(busFunctModel,usid);
            }
            return Json(new[] { busFunctModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteBusSolu([DataSourceRequest] DataSourceRequest request, KEDBBusinessFunctionModel busFunctModel)
        {
            Db db = new Db();
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            var kedbInstanceIDList = (from a in db.KEDBInstance
                                      where a.ClientId == Client.ClientId
                                      select a.BusinessFunctionId).ToList();
            var kedbBusinessFuncList = (from a in db.KEDBBusinessFunction
                                     where a.ClientId == Client.ClientId
                                     select a).ToList();
            if (kedbInstanceIDList.Contains(busFunctModel.Id))
            {
                ModelState.AddModelError("BusinessFunction", "Selected Business Function  is used by a KEDB, can’t be deleted");
                return Json(new[] { kedbBusinessFuncList }.ToDataSourceResult(request, ModelState));
            }
            else if (busFunctModel != null)
            {
                var v = IncidentNavigatorHelper.RemoveBusFunction(busFunctModel.Id);
            }
            
            return Json(new[] { busFunctModel }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult GetSolCategrory([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IncidentNavigatorHelper.GetSolCateDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddSolCategrory(KEDBSolutionCategoryModel solcatModel, int Id)
        {
            if (solcatModel != null && solcatModel.SolCategory.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                solcatModel.ClientId = Client.ClientId;
                var res = IncidentNavigatorHelper.CreateSolCategory(solcatModel, Id,usid);
                solcatModel.Id = res;
            }
            return Json(new[] { solcatModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditSolCategrory([DataSourceRequest] DataSourceRequest request, KEDBSolutionCategoryModel solcatModel)
        {
            if (solcatModel != null && solcatModel.SolCategory.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                solcatModel.ClientId = Client.ClientId;
                var res = IncidentNavigatorHelper.UpdateSolCategory(solcatModel,usid);
            }
            return Json(new[] { solcatModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteSolCategrory([DataSourceRequest] DataSourceRequest request, KEDBSolutionCategoryModel solcatModel)
        {
            Db db = new Db();
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            var kedbInstanceIDList = (from a in db.KEDBInstance
                                      where a.ClientId == Client.ClientId
                                      select a.SolutionCategoryId).ToList();
            var kedbSolCatList = (from a in db.KEDBSolutionCategory
                                        where a.ClientId == Client.ClientId
                                        select a).ToList();
            if (kedbInstanceIDList.Contains(solcatModel.Id))
            {
                ModelState.AddModelError("SolutionCategory", "Selected Solution Category  is used by a KEDB, can’t be deleted");
                return Json(new[] { kedbSolCatList }.ToDataSourceResult(request, ModelState));
            }
            else if (solcatModel != null)
            {
                var v = IncidentNavigatorHelper.RemoveSolCategory(solcatModel.Id);
            }
            
            return Json(new[] { solcatModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        [ActionTrack]
        public ActionResult ReAssignKEDB(int sourceKedb,string Ids, int kedbID)
        {
            var res = IncidentNavigatorHelper.AssignKEDBNumber(sourceKedb,Ids, kedbID);
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ReassignKEDB(string incids)
        {
            return RedirectToAction("AssignKEDB", new { incids = incids });
        }
        public ActionResult AssignKEDB(string incids,int kid, string Clause)
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            var KedbAuth = IncidentNavigatorHelper.VerifyKEDB(Cl.ClientId, incids, kid);
            if (KedbAuth)
            {
                KEDBInstanceModel KIM = new KEDBInstanceModel();
                KIM.Incid = incids;
                KIM.KEDBID = kid;
                KIM.Clause = Clause;
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                return View("AssignKEDB", KIM);
            }
            else
            {
                return null;
            }
           
        }

        [HttpPost]
        public ActionResult GetKEDBIncidentDetailById([DataSourceRequest] DataSourceRequest request, string Incids,int sourcekedbid)
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            
            IEnumerable<KEDBAssIncModel> klist = IncidentNavigatorHelper.getKEDBwithIncident(Cl.ClientId,Incids, sourcekedbid);           
           return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetKEDBDetail([DataSourceRequest] DataSourceRequest request,int sourcekedbid)
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            IEnumerable<KEDBInstanceModel> klist = IncidentNavigatorHelper.getAllKEDBDetail(Cl.ClientId, sourcekedbid);
            return Json(klist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetSourceKEDBDetail([DataSourceRequest] DataSourceRequest request, int sourcekedbid)
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            IEnumerable<KEDBInstanceModel> klist = IncidentNavigatorHelper.getSourceKEDBDetail(Cl.ClientId, sourcekedbid);
            return Json(klist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionTrack]
        public ActionResult addKEDBAction( KEDBInstanceModel kedbmodel)
        {
            var Cl = (ClientDetail)Session["SessionClient"];

            //return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return Json(kedbmodel, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionTrack]
        public ActionResult Async_Save(IEnumerable<HttpPostedFileBase> KAFiles , int kedbID)
        {
            KEDBInstanceModel kem = new KEDBInstanceModel();
            // The Name of the Upload component is "files"
            if (KAFiles != null)
            {
                if (KAFiles != null)
                {
                    Stream filestream = KAFiles.First().InputStream;
                    BinaryReader br = new BinaryReader(filestream);
                    Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                    kem.KAFile = bytes;
                }
            }

            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var updatetitem = unitOfWork.KEDBInstance.GetByID(kedbID);
                updatetitem.KAFile = kem.KAFile;
                updatetitem.FileName = KAFiles.First().FileName;

                unitOfWork.Save();
                
            }

            // Return an empty string to signify success
            return Content("");
        }


        [HttpPost]
        [ActionTrack]
        public ActionResult Async_Edit(IEnumerable<HttpPostedFileBase> KAFiles_Edit, int kedbID)
        {
            KEDBInstanceModel kem = new KEDBInstanceModel();
            // The Name of the Upload component is "files"
            if (KAFiles_Edit != null)
            {
                if (KAFiles_Edit != null)
                {
                    Stream filestream = KAFiles_Edit.First().InputStream;
                    BinaryReader br = new BinaryReader(filestream);
                    Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                    kem.KAFile = bytes;
                }
            }

            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var updatetitem = unitOfWork.KEDBInstance.GetByID(kedbID);
                updatetitem.KAFile = kem.KAFile;
                updatetitem.FileName = KAFiles_Edit.First().FileName;

                unitOfWork.Save();

            }

            // Return an empty string to signify success
            return Content("");
        }


        public FileResult DownloadFile(string KEDBId)
        {
            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var kedbModel = unitOfWork.KEDBInstance.Get(a => a.KEDBNumber == KEDBId).FirstOrDefault();

                if (kedbModel.KAFile != null)
                {
                    return File(kedbModel.KAFile, System.Net.Mime.MediaTypeNames.Application.Octet, kedbModel.FileName );
                }
                else
                {
                    return null;
                }

            }
           
        }

        public ActionResult returnToSearch()
        {
            KEDBInstanceModel c = prepareCommonParameters();
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);


            var Cl = (ClientDetail)Session["SessionClient"];
            KEDBInstanceModel kedbView = new KEDBInstanceModel();
            Db db = new Db();
            //kedbView = IncidentNavigatorHelper.viewKedb(Cl.ClientId);
            kedbView = (from a in db.KEDBInstance
                        join b in db.KEDBIncident on a.Id equals b.KEDBID
                        join d in db.Incidents on b.IncidentID equals d.Id
                        select
                        new KEDBInstanceModel
                        {
                            TicketId = d.IncidentId,
                            Description = a.Description,
                            KEDBNumber = a.KEDBNumber,
                            IssueTypeId = a.IssueTypeId,
                            BusinessFunctionId = a.BusinessFunctionId,
                            SolutionCategoryId = a.SolutionCategoryId,
                            KnowledgeArticle = a.KnowledgeArticle,
                            SolutionStatusId = a.SolutionStatusId,
                            PointOfContact = a.PointOfContact
                        }).FirstOrDefault();
            return View("Index",c);

        }
        public ActionResult Kedb_Summary()
        {
            var Cl = (ClientDetail)Session["SessionClient"];  
            KEDBInstanceModel mod = prepareReportParameters();

            Log.Debug("Kedb Summary page , Client ID is " + Cl.ClientId);
            Log.Info("Kedb Summary page , Client ID is " + Cl.ClientId);
            Log.Warn("Kedb Summary page , Client ID is " + Cl.ClientId);

            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

            ViewBag.userEmailList = userEmailList;

            return View("Kedb_Summary", mod);

        }
        public ActionResult Kedb_ResolutionFormat()
        {
          
            KEDBInstanceModel mod = prepareReportParameters();
            var Cl = (ClientDetail)Session["SessionClient"];
            Log.Debug("Kedb ResolutionFormat , Client ID is " + Cl.ClientId);

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

            ViewBag.userEmailList = userEmailList;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View("Kedb_ResolutionFormat", mod);

        }
        public ActionResult Kedb_AuditLog()
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            KEDBInstanceModel mod = prepareReportParameters();
            Log.Debug("Kedb AuditLog , Client ID is " + Cl.ClientId);

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            ViewBag.userEmailList = userEmailList;
            return View("Kedb_AuditLog", mod);

        }
        [HttpPost]
        public ActionResult GetKEDBTickets([DataSourceRequest] DataSourceRequest request,  
                string Tower, int IssueTypeId, int SolutionCategoryId, int SolutionStatusId, string KEDBNumber, string ReportSelectedname)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IList<SLAMaintenenceModel> SLAMaintenencList = new List<SLAMaintenenceModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<SLAMaintenenceModel> IncidentDetails2 = IncidentNavigatorHelper.getKEDBCluster(client.ClientId, KEDBNumber, IssueTypeId, SolutionCategoryId, SolutionStatusId, Tower);
                
                SLAMaintenencList = IncidentDetails2.ToList();
            }

            return Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Export_Tickets_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);


            return File(fileContents, contentType, fileName);
        }


        [ActionTrack]
        [HttpPost]
        public ActionResult GetKEDB_Drilldown([DataSourceRequest] DataSourceRequest request, string Status_action, string IssueType_action, string Tower_action)
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();

            IEnumerable<KEDBInstanceModel> klist = IncidentNavigatorHelper.getKEDB_Drilldown(Cl.ClientId, Status_action, IssueType_action, Tower_action);


            foreach (var item in klist)
            {
                if (item.KnowledgeArticle != null)
                    item.LinkTextFile = "Link";
                if (item.ResolutionSteps != null)
                    item.LinkTextFile = "Description";
                if (item.FileName != null)
                    item.LinkTextFile = "File";
            }

            return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }


        [ActionTrack]
        [HttpPost]
        public ActionResult GetKEDBSummary_Drilldown([DataSourceRequest] DataSourceRequest request, string Tower , string KEDBNumber)
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();

            IEnumerable<KEDBInstanceModel> klist = IncidentNavigatorHelper.getKEDBSummary_Drilldown(Cl.ClientId, Tower, KEDBNumber);

           foreach (var item in klist)
            {
                if (item.KnowledgeArticle != null)
                    item.LinkTextFile = "Link";
                if (item.ResolutionSteps != null)
                    item.LinkTextFile = "Description";
                if (item.FileName != null)
                    item.LinkTextFile = "File";
            }
            
            return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [ActionTrack]
        [HttpPost]
        public ActionResult GetKEDB_RF_Drilldown([DataSourceRequest] DataSourceRequest request, string KADoc_action, string IssueType_action, string Tower_action, DateTime StartDate, DateTime EndDate)
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();

            IEnumerable<KEDBInstanceModel> klist = IncidentNavigatorHelper.getKEDB_RF_Drilldown(Cl.ClientId, KADoc_action, IssueType_action, Tower_action,   StartDate,  EndDate);


            foreach (var item in klist)
            {
                if (item.KnowledgeArticle != null)
                    item.LinkTextFile = "Link";
                if (item.ResolutionSteps != null)
                    item.LinkTextFile = "Description";
                if (item.FileName != null)
                    item.LinkTextFile = "File";
            }

            return Json(klist.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionTrack]
        public ActionResult saveCriteria(string criteria)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];

            Session["KEDBCriteria"] = criteria;
            var res = Session["KEDBCriteria"];
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ConfirmKEDBAutoAssignment(int KEDBID,int IncidentId)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                var uid = (int)Session["UserId"];
                var kd = IncidentNavigatorHelper.SaveKEDBAutoAssigment(KEDBID, IncidentId, uid, client.ClientId);

                return Json(kd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in Confirm KEDB ", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ConfirmMultKEDBAutoAssignment(int KEDBID,string selectedIDs)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                Log.Debug("Selected Incident IDs " + selectedIDs);
                var uid = (int)Session["UserId"];
                var kd = IncidentNavigatorHelper.SaveKEDBMultAutoAssigment(KEDBID, selectedIDs, uid, client.ClientId);

                return Json(kd, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Log.Error("Error in Confirm KEDB ", ex);
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
