using Data;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.AssessmentManager.Models;
using System.IO;
using WebUI.Dto;
using log4net;
using System.Data;
using Kendo.Mvc.UI;
using Domain.Entities.A3MM;
using Kendo.Mvc.Extensions;
using WebUI.Areas.IncidentNavigator;
using System.Reflection;
using NTTData.AMOPortal.WebUI.Filters;
using System.IO;
using Domain.Abstract;
using WebUI.Models;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace WebUI.Areas.AssessmentManager.Controllers
{
    public class AssessmentController : Controller
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(AssessmentController));
        // GET: AssessmentManager/Assessment
        public ActionResult Index()
        {
            return View();
        }

        private A3MMModel prepareCommonParameters()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            A3MMModel mod = new A3MMModel();

            HomeInput input = (HomeInput)Session["MenuItemsInput"];
            var uid = (int)Session["UserId"];
            GetDrpdwnInfo(mod, client.ClientId, input, uid);
            mod.UserName = IdeaManager.IMHelper.GetUserName(uid);
            mod.ClientId = client.ClientId;
            mod.CreatedDt = DateTime.Now;

            return mod;
        }



        private EngagementQuestionnaireModel prepareCommonParameters1()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            EngagementQuestionnaireModel mod = new EngagementQuestionnaireModel();
            HomeInput input = (HomeInput)Session["MenuItemsInput"];
            var uid = (int)Session["UserId"];
            GetDrpdwnInfo1(mod, client.ClientId, input, uid);

            return mod;
        }

        private ActionItemTrackerModel prepareCommonParameters2()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ActionItemTrackerModel mod = new ActionItemTrackerModel();
            HomeInput input = (HomeInput)Session["MenuItemsInput"];
            var uid = (int)Session["UserId"];
            GetDrpdwnInfo2(mod, client.ClientId, input, uid);

            return mod;
        }

        public static void GetDrpdwnInfo(A3MMModel mod, int ClientId, HomeInput input, int uid)
        {
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                DataSet ds = new DataSet();

                // DataSet ds1 = new DataSet();
                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllDrpdwnforA3MM", paramName, paramValue);

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
                            mod.AssessmentList = ls;
                            break;
                        case 1:
                            mod.DisciplineList = ls;
                            break;
                        case 2:
                            mod.ComponentList = ls;
                            break;
                        case 3:
                            mod.AspectList = ls;
                            break;
                        case 4:
                            mod.DataSourceList = ls;
                            break;
                        case 5:
                            mod.AccountNamesList = ls;
                            break;
                        case 6:
                            mod.ContractList = ls;
                            break;
                        case 7:
                            mod.AssessmentNameList = ls;
                            break;
                        case 8:
                            mod.QuestionSetNoList = ls;
                            break;

                    }
                }
                ds = unitOfWork.GetDataFromStoredProcedureDS("[getUserListForA3MM]", paramName, paramValue);

                List<SelectListItem> userList = new List<SelectListItem>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    userList.Add(new SelectListItem() { Text = row.ItemArray[1].ToString(), Value = row.ItemArray[2].ToString() });

                }
                mod.EngagementPOCList = userList;
                mod.EngagementTeamToCommunicateList = userList;

                ds = unitOfWork.GetDataFromStoredProcedureDS("[getAdminListForA3MM]", paramName, paramValue);
                List<SelectListItem> userList1 = new List<SelectListItem>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    userList1.Add(new SelectListItem() { Text = row.ItemArray[1].ToString(), Value = row.ItemArray[1].ToString() });

                }
                mod.AssessmentTeamNameList = userList1;

                return;
            }
        }

        [HttpPost]
        public ActionResult GetAssessmentDetails([DataSourceRequest] DataSourceRequest request, string ComponentName, int Component_ID, string Assessment_id, string AssessmentName, int avg_MaturityLevel)
        {
            try
            {


                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<A3MMModel> disciplineList;
                using (var contextFactory = new DbContextFactory())
                {

                    disciplineList = AssessmentManagerHelper.getMaturityLevelCompare_drilldown(client.ClientId, ComponentName, Component_ID, Assessment_id, AssessmentName, avg_MaturityLevel);
                }



                Console.WriteLine("WriteLine");
                Console.Write("Write");

                return Json(disciplineList.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("GetTickets Index Action error 'GetTickets()' ", ex);
                throw ex;
            }

        }



        public static void GetDrpdwnInfo1(EngagementQuestionnaireModel mod, int ClientId, HomeInput input, int uid)
        {
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                DataSet ds = new DataSet();

                // DataSet ds1 = new DataSet();
                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllDrpdwnforA3MM", paramName, paramValue);

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
                            mod.AssessmentList = ls;
                            break;
                        case 1:
                            mod.DisciplineList = ls;
                            break;
                        case 2:
                            mod.ComponentList = ls;
                            break;
                        case 3:
                            mod.AspectList = ls;
                            break;
                        case 4:
                            mod.DataSourceList = ls;
                            break;
                        case 5:
                            mod.AccountNamesList = ls;
                            break;
                        case 6:
                            mod.ContractList = ls;
                            break;
                        case 7:
                            mod.AssessmentNameList = ls;
                            break;
                        case 8:
                            mod.QuestionSetNoList = ls;
                            break;

                    }
                }
                ds = unitOfWork.GetDataFromStoredProcedureDS("[getUserListForA3MM]", paramName, paramValue);

                List<SelectListItem> userList = new List<SelectListItem>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    userList.Add(new SelectListItem() { Text = row.ItemArray[1].ToString(), Value = row.ItemArray[2].ToString() });

                }
                mod.EngagementPOCList = userList;
                mod.EngagementTeamToCommunicateList = userList;





                return;
            }
        }




        public static void GetDrpdwnInfo2(ActionItemTrackerModel mod, int ClientId, HomeInput input, int uid)
        {
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                DataSet ds = new DataSet();

                // DataSet ds1 = new DataSet();
                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllDrpdwnforA3MM", paramName, paramValue);

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
                            mod.AssessmentList = ls;
                            break;
                        case 1:
                            mod.DisciplineList = ls;
                            break;
                        case 2:
                            mod.ComponentList = ls;
                            break;
                        case 3:
                            mod.AspectList = ls;
                            break;
                        case 4:
                            mod.DataSourceList = ls;
                            break;
                        case 5:
                            mod.AccountNamesList = ls;
                            break;
                        case 6:
                            mod.ContractList = ls;
                            break;
                        case 7:
                            mod.AssessmentNameList = ls;
                            break;
                        case 8:
                            mod.QuestionSetNoList = ls;
                            break;

                    }
                }
                ds = unitOfWork.GetDataFromStoredProcedureDS("[getUserListForA3MMActionItemOwners]", paramName, paramValue);

                List<SelectListItem> userList = new List<SelectListItem>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    userList.Add(new SelectListItem() { Text = row.ItemArray[1].ToString(), Value = row.ItemArray[2].ToString() });

                }
                mod.EngagementPOCList = userList;
                mod.EngagementTeamToCommunicateList = userList;

                return;
            }
        }


        //Question Set Administration screen
        [ActionTrack]
        public ActionResult BuildDiscipline(int QSetTableID = 0, string isView = "")
        {
            A3MMModel c = prepareCommonParameters();
            A3MMModel getOne = new A3MMModel();

            Db db = new Db();
            getOne = (from d in db.Discipline
                      join comp in db.Component on d.Id equals comp.DisciplineId
                      join a in db.Aspect on comp.Id equals a.ComponentId
                      select
                      new A3MMModel
                      {
                          DisciplineId = d.Id,
                          AspectID = a.Id,
                          ComponentId = a.ComponentId,
                          DisciplineName = d.DisciplineName,
                          ComponentName = comp.ComponentName,
                          AspectName = a.AspectName,
                          //ClientId = a.ClientId
                      }).FirstOrDefault();

            c.AspectID = getOne.AspectID;
            c.ComponentId = getOne.ComponentId;
            c.DisciplineId = getOne.DisciplineId;
            c.AspectName = getOne.AspectName;
            c.ComponentName = getOne.ComponentName;
            c.DisciplineName = getOne.DisciplineName;

            AssessmentModel1 am = new AssessmentModel1();
            am = (from a in db.Assessment
                  select new AssessmentModel1
                  {
                      Id = a.Id,
                      EngagementName = a.EngagementName,
                      Status = a.Status
                  }
                 ).ToList().FirstOrDefault();

            if (am != null)
            {
                c.AssessmentID = am.Id;
                c.Status = am.Status;
            }

            var EngagementQuestionSetdetail = (from a in db.EngagementQuestionSet
                                               where a.isdeleted == 0
                                               select a).FirstOrDefault();
            c.QSetStatus = EngagementQuestionSetdetail.Status;
            c.VersionID = EngagementQuestionSetdetail.VersionID;
            c.SurveyTypeID = EngagementQuestionSetdetail.Assessment_ID;

            if (QSetTableID != 0)
            {
                EngagementQuestionSet eq = (from a in db.EngagementQuestionSet
                                            where a.Id == QSetTableID && a.isdeleted == 0
                                            select a).FirstOrDefault();

                c.VersionID = eq.VersionID;
                c.QSetStatus = eq.Status;
                c.SurveyTypeID = eq.Assessment_ID;


                c.isView = "No";

                if (isView == "Yes")
                {
                    c.isView = "Yes";
                }

            }

            List<SelectListItem> versionlist = (from a in db.EngagementQuestionSet
                                                where a.Assessment_ID == c.SurveyTypeID && a.isdeleted == 0
                                                select new SelectListItem
                                                {
                                                    Text = a.VersionID,
                                                    Value = a.VersionID.ToString()
                                                }).ToList();

            c.QuestionSetNoList = versionlist;

            return View("BuildDiscipline", c);
        }


        public ActionResult GetDisciplineold([DataSourceRequest] DataSourceRequest request, int SurveyTypeID, string QSetNo, string isView = "No")
        {
            int UserId = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            if (isView == "")
            {
                isView = "No";
            }
            IEnumerable<A3MMModel> disciplineList = AssessmentManagerHelper.getDisciplineDetails(Cl.ClientId, SurveyTypeID, QSetNo, isView);

            var currentUserRoleList = (from u in db.UserApplications
                                       join r in db.UserRoles on u.RoleId equals r.Id
                                       where u.ClientId == Cl.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                       select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole = "";


            var Item = currentUserRoleList.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item != null)
                UserRole = Item.Text;
            else
                UserRole = "";


            if (UserRole == "Sysadmin")
            {
                UserRole = "Admin";
            }

            foreach (var a in disciplineList)
            {
                a.UserRole = UserRole;
            }

            return Json(disciplineList.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDiscipline([DataSourceRequest] DataSourceRequest request, int SurveyTypeID, string QSetNo, string isView = "No")
        {
            int UserId = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            if (isView == "")
            {
                isView = "No";
            }
            IEnumerable<A3MMModel> disciplineList = AssessmentManagerHelper.getDisciplineDetails(Cl.ClientId, SurveyTypeID, QSetNo, isView);

            var currentUserRoleList = (from u in db.UserApplications
                                       join r in db.UserRoles on u.RoleId equals r.Id
                                       where u.ClientId == Cl.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                       select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole = "";


            var Item = currentUserRoleList.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item != null)
                UserRole = Item.Text;
            else
                UserRole = "";


            if (UserRole == "Sysadmin")
            {
                UserRole = "Admin";
            }

            foreach (var a in disciplineList)
            {
                a.UserRole = UserRole;
            }
            var result = disciplineList.ToTreeDataSourceResult(request,
               e => e.Id,
               e => e.ParentID,
               e => e
           );

            return Json(result, JsonRequestBehavior.AllowGet);
            // return Json(disciplineList.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuestionsAndSubQuestions([DataSourceRequest] DataSourceRequest request, int SurveyTypeID, string QSetNo, string isView = "No")
        {
            int UserId = (int)Session["UserId"];
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            if (isView == "")
            {
                isView = "No";
            }

            IEnumerable<A3MMModel> disciplineList = AssessmentManagerHelper.getDisciplineDetails(Cl.ClientId, SurveyTypeID, QSetNo, isView);


            var currentUserRoleList = (from u in db.UserApplications
                                       join r in db.UserRoles on u.RoleId equals r.Id
                                       where u.ClientId == Cl.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                       select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole = "";


            var Item = currentUserRoleList.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item != null)
                UserRole = Item.Text;
            else
                UserRole = "";


            if (UserRole == "Sysadmin")
            {
                UserRole = "Admin";
            }

            foreach (var a in disciplineList)
            {
                a.UserRole = UserRole;
            }

            var result = disciplineList.ToTreeDataSourceResult(request,
                e => e.Id,
                e => e.ParentID,
                e => e
            );

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ActionTrack]
        public JsonResult AddQuesAndSubQues([DataSourceRequest] DataSourceRequest request, A3MMModel a3MMModel)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            a3MMModel.ClientId = client.ClientId;
            return Json(new[] { a3MMModel }.ToTreeDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult GetMetaDetails(string AspectID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int aID = Convert.ToInt32(AspectID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            A3MMModel getOne = new A3MMModel();

            getOne = (from d in db.Discipline
                      join comp in db.Component on d.Id equals comp.DisciplineId
                      join a in db.Aspect on comp.Id equals a.ComponentId
                      where a.Id == aID
                      select
                      new A3MMModel
                      {
                          DisciplineId = d.Id,
                          AspectID = a.Id,
                          ComponentId = a.ComponentId,
                          DisciplineName = d.DisciplineName,
                          ComponentName = comp.ComponentName,
                          AspectName = a.AspectName,
                          //ClientId = a.ClientId
                      }).FirstOrDefault();

            return Json(getOne, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult latestDraftVersionForType(string SurveyTypeID)
        {
            Db db = new Db();
            int SurveyTypeIDint = Convert.ToInt32(SurveyTypeID);
            var lastestVersion = (from a in db.EngagementQuestionSet
                                  where a.Assessment_ID == SurveyTypeIDint && a.Status == "Draft" && a.isdeleted == 0
                                  select a.VersionID).FirstOrDefault();

            return Json(lastestVersion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionTrack]
        public ActionResult SaveQuestion(A3MMModel AM)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            //Can Add Discipline name , component name, component definition  , aspect name into question table for performance improvement 
            AM.ClientId = client.ClientId;
            Db db = new Db();

            //for sub question
            if (AM.ParentID != null)
            {
                var subresult = (from a in db.EngagementQuestionnaire
                                 where a.VersionID == AM.VersionID && a.SurveyTypeID == AM.SurveyTypeID && a.ParentID == AM.ParentID
                                 orderby a.SubQOrder descending
                                 select a.SubQOrder.Value).FirstOrDefault();
                AM.SubQOrder = (subresult == 0 ? 0 : subresult) + 1;
                AM.SQNo = AM.QNo + ((char)(AM.SubQOrder + 64)).ToString();
                AM.QuestionOrder = AM.QNo;
                AM.QNo = null;


            }
            else
            {
                var result = (from a in db.EngagementQuestionnaire
                              where a.VersionID == AM.VersionID && a.SurveyTypeID == AM.SurveyTypeID
                              orderby a.QNo descending
                              select a.QNo).FirstOrDefault();
                var prevQNo = result == null ? 0 : result;
                AM.QNo = prevQNo + 1;
                AM.QuestionOrder = prevQNo + 1;
                AM.SQNo = (prevQNo + 1).ToString();
            }
            var DiscipleDetail= (from dis in db.Discipline
                                 where dis.Id == AM.DisciplineId
                                 select dis).FirstOrDefault();
            AM.DisciplineName = DiscipleDetail.DisciplineName;
            AM.DiscplineAbbrevation = DiscipleDetail.DiscplineAbbrevation;


            var ComponentDetail = (from comp in db.Component
                                where comp.Id == AM.ComponentId
                                select comp).FirstOrDefault();
            AM.ComponentName = ComponentDetail.ComponentName;
            AM.ComponentAbbrevation = ComponentDetail.ComponentAbbrevation;

            AM.AspectName = (from asp in db.Aspect
                             where asp.Id == AM.AspectID
                             select asp.AspectName).FirstOrDefault().ToString();

            AM.countOfQOfSameDisandComp = (from a in db.EngagementQuestionnaire
                              where a.VersionID == AM.VersionID && a.SurveyTypeID == AM.SurveyTypeID && a.Discipline_ID == AM.DisciplineId && a.Component_ID == AM.ComponentId && a.ParentID==null
                              select a.Id).ToList().Count();          


            var res = AssessmentManagerHelper.addQuestion(AM);

           return Json(AM, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionTrack]
        public ActionResult SaveQuestionEdit(A3MMModel AM)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            AM.ClientId = client.ClientId;

            AM.DisciplineName = (from a in db.Discipline
                                 where a.Id == AM.DisciplineId
                                 select a.DisciplineName).FirstOrDefault();

            AM.ComponentName = (from a in db.Component
                                where a.Id == AM.ComponentId
                                select a.ComponentName).FirstOrDefault();

            AM.AspectName = (from a in db.Aspect
                             where a.Id == AM.AspectID
                             select a.AspectName).FirstOrDefault();


            if (AM.isEditdrpdwnchange)
            {

                if (AM.isEditDisciplineComponentChange)
                {
                    //Question Identifier update for parent question
                    //Collecting Dis/Comp abbr for new update and also current count of new dis/comp
                    AM.DiscplineAbbrevation = (from dis in db.Discipline
                                               where dis.Id == AM.DisciplineId
                                               select dis.DiscplineAbbrevation).FirstOrDefault().ToString();

                    AM.ComponentAbbrevation = (from comp in db.Component
                                               where comp.Id == AM.ComponentId
                                               select comp.ComponentAbbrevation).FirstOrDefault().ToString();

                    AM.countOfQOfSameDisandComp = (from a in db.EngagementQuestionnaire
                                                   where a.VersionID == AM.VersionID && a.SurveyTypeID == AM.AssessmentID && a.Discipline_ID == AM.DisciplineId && a.Component_ID == AM.ComponentId && a.ParentID == null
                                                   select a.Id).ToList().Count();

                    var res = AssessmentManagerHelper.addQuestionEdit(AM);


                    //On change of Dis/Comp for Parent question, sub question Question Identifier should change

                    List<int> subQuestionsIDs = (from a in db.EngagementQuestionnaire
                                                 where a.ParentID == AM.QuestionId.Value
                                                 select a.Id).ToList();

                    EngagementQuestionnaire parentEQ =  (from a in db.EngagementQuestionnaire
                                                             where a.Id == AM.QuestionId.Value
                                                             select a).ToList().FirstOrDefault();


                    var ParentQuestionIdentifierAfterUpdate = parentEQ.QuestionIdentifier;
                    var last2Characters = ParentQuestionIdentifierAfterUpdate.Substring(ParentQuestionIdentifierAfterUpdate.Length - 2);
                    char[] twoArray = last2Characters.ToCharArray();
                    //01 or 11
                    var subNumber = "";
                    if (Char.ToString(twoArray[0]) == "0") // from 01-09
                    {
                         //subNumber = twoArray[1].ToString(); //1,2,3
                         subNumber = Char.ToString(twoArray[1]); //1,2,3
                    }
                    else
                    {
                        //subNumber =twoArray.ToString(); //10,11,12...
                        subNumber = new string(twoArray);//10,11,12...
                    }
                    foreach (var a in subQuestionsIDs)
                    {
                        //pass Sub Question Identifier and Sub Question ID

                        var subQId = a;
                        var subQIdentiferNumber = subNumber;
                        var existingSubQuestionIdentifier = (from b in db.EngagementQuestionnaire
                                                          where b.Id == subQId
                                                          select b).FirstOrDefault();

                        var charFromSubQOrder =  ((char)(existingSubQuestionIdentifier.SubQOrder + 64)).ToString();


                        var FinalSubQNo = subQIdentiferNumber + charFromSubQOrder; //1A or 11A

                        var updateSubQIdentifier = AM.DiscplineAbbrevation + "-" + AM.ComponentAbbrevation + "-" + FinalSubQNo;


                        AssessmentManagerHelper.changeSubQuestionIdentifier(AM.QuestionId.Value, AM.DisciplineId, AM.ComponentId, AM.AspectID.Value, AM.DisciplineName, AM.ComponentName, AM.AspectName, updateSubQIdentifier, subQId);

                    }



                }
                else
                {
                    var res = AssessmentManagerHelper.addQuestionEdit(AM);

                    AssessmentManagerHelper.changeSubQuestion(AM.QuestionId.Value, AM.DisciplineId, AM.ComponentId, AM.AspectID.Value, AM.DisciplineName, AM.ComponentName, AM.AspectName);

                }

            }
            else
            {
                var res = AssessmentManagerHelper.addQuestionEdit(AM);

            }
            return Json(AM, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetQuestionDetails(string QuestionID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int qID = Convert.ToInt32(QuestionID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            A3MMModel getOne = new A3MMModel();

            getOne = (from q in db.EngagementQuestionnaire
                      where q.Id == qID
                      select
                      new A3MMModel
                      {
                          QuestionId = q.Id,
                          AspectID = q.AspectID,
                          VersionID = q.VersionID,
                          Question = q.Question,
                          ExampleAnswer = q.ExampleAns,
                          DatasourceId = q.DataSourceID,
                          DisciplineId = (int)q.Discipline_ID,
                          ComponentId = (int)q.Component_ID,
                          DisciplineName = q.DisciplineName,
                          ComponentName = q.ComponentName,
                          QNo = q.QNo,
                          SQNo = q.SQNo,
                          ParentID = q.ParentID,
                          SurveyTypeID = q.SurveyTypeID
                      }).FirstOrDefault();

            return Json(getOne, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetLatestVersionstatus(int SurveyTypeID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            List<SelectListItem> versionlist = (from a in db.EngagementQuestionSet
                                                where a.Assessment_ID == SurveyTypeID && a.isdeleted == 0
                                                select new SelectListItem
                                                {
                                                    Text = a.Status,
                                                    Value = a.VersionID.ToString()
                                                }).ToList().OrderByDescending(e => e.Value).ToList();


            return Json(versionlist, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public ActionResult Export_QSetAdministration_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }



        [HttpPost]
        public ActionResult GetQSetStatus(string SurveyTypeID, string QSetNo)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                int assessID = Convert.ToInt32(SurveyTypeID);


                Db db = new Db();

                var QuestionSet = (from a in db.EngagementQuestionSet
                                   where a.Assessment_ID == assessID && a.VersionID == QSetNo && a.isdeleted == 0
                                   select a).FirstOrDefault();




                return Json(QuestionSet, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionTrack]
        public ActionResult FinalizeQuestionSet(string AssessmentTypeID, string QSetNo)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
            int surveyTypeID = Convert.ToInt32(AssessmentTypeID);

            //Finalize questions for the versionID and surveyTypeID combination

            List<EngagementQuestionnaire> questionList = (from a in db.EngagementQuestionnaire
                                                          where a.VersionID == QSetNo && a.SurveyTypeID == surveyTypeID && a.Status == "Draft"
                                                          select a).ToList();


            AssessmentManagerHelper.FinalizeQuestions(questionList);


            //Archive all the previously finalized Questions


            List<EngagementQuestionnaire> questionstoArchiveList = (from a in db.EngagementQuestionnaire
                                                                    where a.VersionID != QSetNo && a.SurveyTypeID == surveyTypeID && a.Status == "Finalized"
                                                                    select a).ToList();


            AssessmentManagerHelper.ArchiveQuestions(questionstoArchiveList);






            //Finalize current version in Qset table
            EngagementQuestionSet getDraftVersionToFinalize = (from a in db.EngagementQuestionSet
                                                               where a.VersionID == QSetNo && a.Assessment_ID == surveyTypeID && a.Status == "Draft" && a.isdeleted==0
                                                               select a).FirstOrDefault();
            var viewLink = Url.Action("BuildDiscipline", "Assessment", new { area = "AssessmentManager" }, "https");
            viewLink = viewLink + "?QSetTableID=" + getDraftVersionToFinalize.Id + "&isView=Yes";
            var surveyType = "";

            if (surveyTypeID == 1)
            {
                surveyType = "CES-IVV";
            }
            else if (surveyTypeID == 2)
            {
                surveyType = "CES-A3MM";
            }
            else
            {
                surveyType = "CES-DA";
            }
            var username = (from a in db.UserProfiles
                            where a.Id == uid
                            select a).FirstOrDefault();
            AssessmentManagerHelper.FinalizeQuestionSet(getDraftVersionToFinalize, uid,viewLink,surveyType,username, QSetNo);


            //Archive previously finalized version in Qset table
            List<EngagementQuestionSet> finalizedQsetList = (from a in db.EngagementQuestionSet
                                                             where a.VersionID != QSetNo && a.Assessment_ID == surveyTypeID && a.Status == "Finalized" && a.isdeleted==0
                                                             select a).ToList();

            AssessmentManagerHelper.ArchivePreviousQuestionSet(finalizedQsetList);


            return Json(getDraftVersionToFinalize, JsonRequestBehavior.AllowGet);


        }

        public ActionResult getLatestVersionID(int surveyTypeID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;

            Db db = new Db();

            var surveyTypeIDInt = Convert.ToInt32(surveyTypeID);

            var lastestVersion = (from a in db.EngagementQuestionSet
                                  orderby a.VersionID descending
                                  where a.Assessment_ID == surveyTypeIDInt && a.isdeleted==0
                                  select a.VersionID).FirstOrDefault();

            //Ver - 2.0

            //Currently limit is Ver -9.1 - Ver- 9.9 after 10.0 it doesnt work , find alternative solution after phase2

            string ver = lastestVersion;

            int len = ver.Length;
            var lastDigit = Convert.ToInt32(ver.Substring(8, 1)); //4

            if (Convert.ToInt32(lastDigit) >= 0 && Convert.ToInt32(lastDigit) < 9)
            {
                lastDigit = lastDigit + 1; //4+1
                ver = ver.Substring(0, 8) + lastDigit.ToString(); //ver - 0.5
            }
            else if (Convert.ToInt32(lastDigit) == 9)
            {
                lastDigit = 0;
                int lastButOneDigit = Convert.ToInt32(ver.Substring(6, 1)); //1
                lastButOneDigit = lastButOneDigit + 1; //2 , 0
                ver = ver.Substring(0, 6) + lastButOneDigit + "." + lastDigit;
            }



            return Json(ver, JsonRequestBehavior.AllowGet);
        }









        // Launch Assessment Screen
        [ActionTrack]
        public ActionResult LaunchAssessmentUI()
        {
            int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();

            A3MMModel a3 = prepareCommonParameters();


            List<SelectListItem> newSurveyType = (from a in db.A3SurveyType
                                            where a.ID == 3
                                            select new SelectListItem
                                            {
                                                Value = a.ID.ToString(),
                                                Text = a.SurveyName
                                            }).ToList();


           
            a3.AssessmentList = newSurveyType;
            List < SelectListItem > assessmentNameInitial = new List<SelectListItem>();
            assessmentNameInitial = (from a in db.UserProfiles
                                     where a.Id == usid
                                     select new SelectListItem
                                     {
                                         Value = a.Email,
                                         Text = a.Email

                                     }).ToList();

            List<SelectListItem> accountNameInitial = new List<SelectListItem>();
            accountNameInitial = (from a in db.ClientAccountSetup
                                  where a.ClientId == Cl.ClientId
                                  select new SelectListItem
                                  {
                                      Value = a.Id.ToString(),
                                      Text = a.AccountName

                                  }).ToList();


            List<SelectListItem> contractNameInitial = new List<SelectListItem>();
            int accountIDInitial = (from a in db.ClientAccountSetup
                                    where a.ClientId == Cl.ClientId
                                    select a.Id).FirstOrDefault();

            contractNameInitial = (from a in db.ClientAccountContract
                                   where a.AccountId == accountIDInitial
                                   select new SelectListItem
                                   {
                                       Value = a.Id.ToString(),
                                       Text = a.ContractName

                                   }).ToList();

            a3.AssessmentTeamNameList = assessmentNameInitial;
            a3.AccountNamesList = accountNameInitial;
            a3.ContractList = contractNameInitial;
            return View(a3);

        }


        [HttpPost]
        [ActionTrack]
        public ActionResult LaunchAssessment(A3MMModel AM)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                //AM.StartDate = Convert.ToDateTime(AM.startDateIgnore); //dd/mm/yy
                AM.ClientId = client.ClientId;
                int assessID = Convert.ToInt32(AM.AssessmentID);
                int accID = Convert.ToInt32(AM.AccountID);
                int conID = Convert.ToInt32(AM.ContractID);

                Db db = new Db();
                var assessmentId = 0;
                var assessment = (from a in db.Assessment
                                  where a.SurveyTypeID == assessID && a.AccountID == accID && a.ContractID == conID && a.AssessmentName== AM.AssessmentName
                                  select a).FirstOrDefault();
                   var responseLink = Url.Action("RedirectToGathering", "ResponseGathering", new { area = "AssessmentManager" }, "https");
                if (assessment != null)
                {
                    AM.SurveyTypeID = assessment.SurveyTypeID;
                    AM.AssessmentID = assessment.Id;
                    var res = AssessmentManagerHelper.editLaunchAssessmentDetails(AM);
                    assessmentId = assessment.Id;
                }
                else
                {
                    var AssessmentID = AssessmentManagerHelper.addLaunchAssessmentDetails(AM);
                    assessmentId = AssessmentID;
                    var qList = (from a in db.EngagementQuestionnaire
                                 where a.VersionID == AM.VersionID && a.SurveyTypeID == AM.AssessmentID && a.Status == "Finalized"
                                 select a).ToList();
                    if (qList.Count != 0)
                    {
                        AssessmentManagerHelper.addQuestionResponseRecords(qList, AssessmentID, AM.AssessmentID, client.ClientId);
                    }
                }
                if (AM.Status == "Initiated")
                {
                    var assResponse = unitOfWork.QuestionnaireResponse.Get(a => a.AssessmentID == assessmentId).OrderBy(a => a.Id).FirstOrDefault();
                    responseLink = responseLink + "?AssessmentIDparam=" + assessmentId ;

                    var poc = unitOfWork.UserProfile.GetByID(AM.EngagementPocID);
                    var username = unitOfWork.UserProfile.GetByID(usid);
                    var commUser = unitOfWork.UserProfile.GetByID(AM.EngagementComID);
                    AssessmentManagerHelper.SendLaunchEmail(poc, commUser, username, AM.Status, AM.AssessmentName, responseLink);

                }
                return Json(AM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("error in LaunchAssessment controller", ex);
                throw new Exception("error LaunchAssessment controller", ex);
            }
        }

        //[HttpPost]
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
                           }).ToList();
            return Json(dispSln.Select(A => new { Value = A.Id.ToString(), Text = A.ContractName, Selected = true }), JsonRequestBehavior.AllowGet);
            //return Json(dispSln, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ReadAllAssessments([DataSourceRequest] DataSourceRequest request)
        {
            //var Cl = (Client)Session["Client"];
            // ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            //int ClientId = client.Id;
            int UserId = (int)Session["UserId"];

            Db db = new Db();
            var client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            var currentUserRoleList1 = (from u in db.UserApplications
                                        join r in db.UserRoles on u.RoleId equals r.Id
                                        where u.ClientId == client.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                        select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole1 = "";


            var Item1 = currentUserRoleList1.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item1 != null)
                UserRole1 = Item1.Text;
            else
                UserRole1 = "";

            //ret1.UserRole = UserRole1;
            IEnumerable<A3MMAssessmentModel> a3 = AssessmentManagerHelper.Get_AssessmentDetails(ClientId);

            foreach (var item in a3)
            {
                item.userRole = UserRole1;
            }

            //return Json(AssessmentManagerHelper.Get_AssessmentDetails(ClientId).ToDataSourceResult(request));
            return Json(a3.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Export_Assessment_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public ActionResult ValidateAssessmentNameForLA(string AssessmentID, string AccountID, string ContractID, string AssessmentName, int quarter, int year)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                int assessID = Convert.ToInt32(AssessmentID);
                int accID = Convert.ToInt32(AccountID);
                int conID = Convert.ToInt32(ContractID);
                string quarter2 = "Q"+quarter.ToString();
                string year2 = year.ToString();
                var client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;

                Db db = new Db();

                var assessment = new Assessment();

                if (assessID == 1) //IVV
                {
                    assessment = (from a in db.Assessment
                                  where a.SurveyTypeID == assessID && a.AccountID == accID && a.ContractID == conID && a.ClientId == ClientId && a.AssessmentName == AssessmentName
                                  select a).FirstOrDefault();

                    if (assessment != null) { 
                        var quarterassess = assessment.AssessmentName.Substring(assessment.AssessmentName.Length - 2); //Q3
                        var yearassess = assessment.AssessmentName.Substring(assessment.AssessmentName.Length - 7, 4);

                        if (quarter2 != quarterassess)
                        {
                            assessment = null;
                        }
                    }

                }

                if (assessID == 2) //A3MM
                {
                    assessment = (from a in db.Assessment
                                  where a.SurveyTypeID == assessID && a.AccountID == accID && a.ContractID == conID && a.ClientId == ClientId && a.AssessmentName == AssessmentName
                                  select a).FirstOrDefault();

                    if (assessment != null)
                    {
                        var yearassess2 = assessment.AssessmentName.Substring(assessment.AssessmentName.Length - 7, 4);

                        if (year2 != yearassess2)
                        {
                            assessment = null;
                        }
                    }

                }


                if (assessID == 3) //DA
                {
                    assessment = (from a in db.Assessment
                                  where a.SurveyTypeID == assessID && a.AccountID == accID && a.ContractID == conID && a.ClientId == ClientId && a.AssessmentName == AssessmentName
                                  select a).FirstOrDefault();

                    if (assessment != null)
                    {
                        var yearassess2 = assessment.AssessmentName.Substring(assessment.AssessmentName.Length - 7, 4);

                        if (year2 != yearassess2)
                        {
                            assessment = null;
                        }
                    }

                }

                //var assessment = (from a in db.Assessment
                //                  where a.SurveyTypeID == assessID && a.AccountID == accID && a.ContractID == conID 
                //                  select a).FirstOrDefault();
                if (assessment == null)
                {
                    var QuestionaireVersion = (from a in db.EngagementQuestionSet
                                               where a.Assessment_ID == assessID && a.Status == "Finalized" && a.isdeleted == 0
                                               select a.VersionID).FirstOrDefault();

                    if (QuestionaireVersion == null)
                    {
                        QuestionaireVersion = "Ver - 0.1";
                    }
                    Assessment ass = new Assessment() { QuestionaireVersion = QuestionaireVersion };
                    assessment = ass;
                }



                return Json(assessment, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getLatestFinalizedVersionForAssessmentType(string TypeID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;

            Db db = new Db();

            var Type = Convert.ToInt32(TypeID);

            var version = (from a in db.EngagementQuestionSet
                           where a.Assessment_ID == Type && a.Status == "Finalized" && a.isdeleted == 0
                           select a.VersionID).FirstOrDefault();

            if (version == null)
            {
                version = "Ver - 0.1";
            }

            return Json(version, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getLatestFinalizedVersionIDForAssessmentType(string TypeID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;

            Db db = new Db();

            var version = (from a in db.EngagementQuestionSet
                           where a.Assessment_ID == Convert.ToInt32(TypeID) && a.Status == "Finalized" && a.isdeleted == 0
                           select a.VersionID).LastOrDefault();

            return Json(version, JsonRequestBehavior.AllowGet);
        }











        //Assessment Inventory screen
        [ActionTrack]
        public ActionResult AssessmentInventory()
        {
            var UserId = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;
            //ViewData["GetStringResult"] = getFullName(UserId);
            A3MMModel c = prepareCommonParameters();
            A3MMModel getOne = new A3MMModel();

            Db db = new Db();
            getOne = (from d in db.Discipline
                      join comp in db.Component on d.Id equals comp.DisciplineId
                      join a in db.Aspect on comp.Id equals a.ComponentId
                      select
                      new A3MMModel
                      {
                          DisciplineId = d.Id,
                          AspectID = a.Id,
                          ComponentId = a.ComponentId,
                          DisciplineName = d.DisciplineName,
                          ComponentName = comp.ComponentName,
                          AspectName = a.AspectName,
                          //ClientId = a.ClientId
                      }).FirstOrDefault();

            c.AspectID = getOne.AspectID;
            c.ComponentId = getOne.ComponentId;
            c.DisciplineId = getOne.DisciplineId;
            c.AspectName = getOne.AspectName;
            c.ComponentName = getOne.ComponentName;
            c.DisciplineName = getOne.DisciplineName;

            AssessmentModel1 am = new AssessmentModel1();
            am = (from a in db.Assessment
                  where a.ClientId == cid
                  select new AssessmentModel1
                  {
                      Id = a.Id,
                      EngagementName = a.EngagementName,
                  }
                 ).ToList().FirstOrDefault();

            if (am != null)
            {
                c.AssessmentID = am.Id;
            }


            return View(c);

        }



        [HttpPost]
        public ActionResult GetOneAssessment(int AssessmentID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            //A3MMAssessmentModel a3mm = new A3MMAssessmentModel();
            A3MMAssessmentModel a3mm = AssessmentManagerHelper.getAssessmentDetail(AssessmentID);
            return Json(a3mm, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ActionTrack]
        public ActionResult EditAssessment(A3MMAssessmentModel AM)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int clientid = client.ClientId;
                AM.ClientId = clientid;
                int usid = (int)Session["UserId"];
                //AM.StartDate = Convert.ToDateTime(AM.startDateIgnore);
                var res = AssessmentManagerHelper.UpdateAssessment(AM);
                return Json(AM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in EditAssessment ", ex);
                throw ex;
            }

        }


        public ActionResult RedirectToDGFromAI(string AssessmentID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int assessID = Convert.ToInt32(AssessmentID);
            Db db = new Db();



            var surveyTypeID = (from a in db.Assessment
                                where a.Id == assessID
                                select a.SurveyTypeID).FirstOrDefault();
            int isAI = 1;

            var ret = "window.location='DataGathering?AssessmentIDparam=" + assessID + "&isAI=" + isAI + "'";
            //return JavaScript("window.location = 'DataGathering?AssessmentIDparam=15&QNoparam=3'");
            return JavaScript(ret);


        }






        //Data Gathering screen
        [ActionTrack]
        public ActionResult DataGathering(int AssessmentIDparam = 0, int QNoparam = 0, int isView = 0, int isAI = 0, int SurveyTypeID = 0)
        {
            var UserId = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            //ViewData["GetStringResult"] = getFullName(UserId);
            A3MMModel c = prepareCommonParameters();
            A3MMModel getOne = new A3MMModel();
            Db db = new Db();
            if (AssessmentIDparam != 0)
            {

                A3MMModel bfrret1 = prepareCommonParameters();
                A3MMModel ret1 = new A3MMModel();
                var surveyTypeIDget = (from a in db.Assessment
                                       where a.Id == AssessmentIDparam && a.ClientId == Cl.ClientId
                                       select a.SurveyTypeID).FirstOrDefault();
                //fetch version from Assessment table

                var versionIDinitial1 = (from a in db.Assessment
                                         where a.Id == AssessmentIDparam && a.SurveyTypeID == surveyTypeIDget && a.ClientId == Cl.ClientId
                                         select a.QuestionaireVersion).FirstOrDefault(); //New

                //var versionIDinitial1 = (from a in db.EngagementQuestionSet
                //                         where a.Assessment_ID == surveyTypeIDget
                //                         select a.VersionID).FirstOrDefault(); // may not be required


                //get list of questions from Question and Response table for AssessmentID and store it in Questionset field


                List<A3MMModel> qSET1 = new List<A3MMModel>();

                var questionList1 = (from a in db.EngagementQuestionnaire
                                     where a.SurveyTypeID == surveyTypeIDget && a.VersionID == versionIDinitial1
                                     select a).ToList();

                var responseList1 = (from a in db.QuestionnaireResponse
                                     where a.SurveyTypeId == surveyTypeIDget && a.AssessmentID == AssessmentIDparam && a.VersionID == versionIDinitial1 && a.ClientID == Cl.ClientId
                                     select a).ToList();


                foreach (var a in responseList1)
                {
                    EngagementQuestionnaire e = (from b in questionList1
                                                 where b.SQNo == a.SQNo && b.SurveyTypeID == a.SurveyTypeId
                                                 select b).FirstOrDefault();
                    List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                               where x.QuestionRespId == a.Id
                                                               select new ResponseAttachment
                                                               {
                                                                   Id = x.Id,
                                                                   filename = x.FileName,
                                                                   fileCont = x.AttachFile
                                                               }).ToList();


                    if (e != null)
                    {
                        A3MMModel a3mm = new A3MMModel();
                        a3mm.QNo = a.QNo;
                        a3mm.SurveyTypeID = e.SurveyTypeID;
                        a3mm.AssessmentID = a.AssessmentID;
                        a3mm.DisciplineId = Convert.ToInt32(e.Discipline_ID);
                        a3mm.DisciplineName = e.DisciplineName;
                        a3mm.ComponentId = Convert.ToInt32(e.Component_ID);
                        a3mm.ComponentName = e.ComponentName;
                        a3mm.AspectID = e.AspectID;
                        a3mm.AspectName = e.AspectName;
                        a3mm.Question = e.Question;
                        a3mm.ExampleAnswer = e.ExampleAns;
                        a3mm.Response = a.Response;
                        a3mm.AssessmentResponse = a.AssessmentResponse;
                        a3mm.Feedback = a.Feedback;
                        a3mm.FollowupQuestion = a.FollowUpQuestion;
                        a3mm.MaturityLevel = a.MaturityLevel;
                        //a3mm.MaturityLevelInt = Convert.ToInt32(a.MaturityLevel);
                        a3mm.AreasofImprovement = a.AreaOfImprovement;
                        a3mm.QuestionId = e.Id;
                        a3mm.ClientId = 1;
                        a3mm.QuestionResponseID = a.Id;
                        a3mm.LastFeedbackBy = Convert.ToInt32(a.LastFeedBackBy);
                        a3mm.LastFeedbackOn = a.LastFeedBackOn;
                        a3mm.LastUpdatedBy = Convert.ToInt32(a.LastUpdatedBy);
                        a3mm.LastUpdatedOn = a.LastUpdatedOn;
                        a3mm.VersionID = a.VersionID;
                        a3mm.DatasourceId = e.DataSourceID;
                        a3mm.SQNo = e.SQNo;
                        a3mm.SubQOrder = e.SubQOrder;
                        a3mm.QuestionOrder = e.QuestionOrder;
                        a3mm.RespAttachment = RespAttachment;

                        qSET1.Add(a3mm);
                    }


                }



                if (isAI != 0) //for Assessment Inventory screen , just redirects screen to Data Gathering
                {
                    if (qSET1.Count != 0)
                    {
                        ret1 = qSET1.ToList().FirstOrDefault();
                        if (ret1 != null)
                        {
                            var LastUpdatedByName1 = (from a in db.UserProfiles
                                                      where a.Id == ret1.LastUpdatedBy
                                                      select a.UserName).FirstOrDefault();

                            ret1.LastUpdatedByName = LastUpdatedByName1;

                            var LastFeedbackByName1 = (from a in db.UserProfiles
                                                       where a.Id == ret1.LastFeedbackBy
                                                       select a.UserName).FirstOrDefault();

                            ret1.LastFeedbackByName = LastFeedbackByName1;
                        }
                    }
                }
                else //for Assessment Overview
                {
                    ret1 = (from a in qSET1
                            where a.QNo == QNoparam
                            select a).FirstOrDefault();

                    var LastUpdatedByName1 = (from a in db.UserProfiles
                                              where a.Id == ret1.LastUpdatedBy
                                              select a.UserName).FirstOrDefault();

                    ret1.LastUpdatedByName = LastUpdatedByName1;

                    var LastFeedbackByName1 = (from a in db.UserProfiles
                                               where a.Id == ret1.LastFeedbackBy
                                               select a.UserName).FirstOrDefault();

                    ret1.LastFeedbackByName = LastFeedbackByName1;

                }


                if (ret1.LastUpdatedByName == null)
                {
                    ret1.LastUpdatedByName = "";
                }
                if (ret1.LastFeedbackByName == null)
                {
                    ret1.LastFeedbackByName = "";
                }
                if (isView == 1)
                {
                    ret1.isView = "View";
                    ret1.fromAssessmentOverview = 1;
                }
                else
                {
                    ret1.isView = "Edit";
                    ret1.fromAssessmentOverview = 1;

                }
                if (isAI == 1)
                {
                    ret1.isView = "DataGatheringFromAI";
                    ret1.fromAssessmentOverview = 0;

                }

                ret1.DisciplineList = bfrret1.DisciplineList;
                ret1.ComponentList = bfrret1.ComponentList;
                ret1.AspectList = bfrret1.AspectList;
                ret1.AssessmentList = bfrret1.AssessmentList;
                ret1.AccountNamesList = bfrret1.AccountNamesList;
                ret1.ContractList = bfrret1.ContractList;
                ret1.DataSourceList = bfrret1.DataSourceList;
                ret1.QuestionSet = qSET1.OrderBy(e => e.QuestionOrder).ToList();
                ret1.Status = (from a in db.Assessment
                               where a.Id == AssessmentIDparam
                               select a.Status).FirstOrDefault();
                List<SelectListItem> AssessmentNameList1 = (from a in db.Assessment
                                                            where a.SurveyTypeID == surveyTypeIDget && a.ClientId == Cl.ClientId
                                                            select new SelectListItem
                                                            {
                                                                Text = a.AssessmentName,
                                                                Value = a.Id.ToString()
                                                            }).ToList();

                ret1.AssessmentNameList = AssessmentNameList1;

                var currentUserRoleList1 = (from u in db.UserApplications
                                            join r in db.UserRoles on u.RoleId equals r.Id
                                            where u.ClientId == Cl.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                            select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

                var UserRole1 = "";


                var Item1 = currentUserRoleList1.Select(lst => new
                {
                    lst.Value,
                    lst.Text,
                    lst.Weight
                }).OrderByDescending(o => o.Weight).FirstOrDefault();
                if (Item1 != null)
                    UserRole1 = Item1.Text;
                else
                    UserRole1 = "";

                ret1.UserRole = UserRole1;
                if (ret1.MaturityLevel == null)
                {
                    ret1.MaturityLevel = "0";
                }
                return View(ret1);
            }


            //New Approach
            //Initial Data gathering Load
            A3MMModel ret = prepareCommonParameters(); // fetching dropdown values
            A3MMModel initial = new A3MMModel();

            var AssessmentIDinitial = (from a in db.Assessment
                                       where a.ClientId == Cl.ClientId //A3MM
                                       select a.Id).FirstOrDefault(); //get first assessment ID


            var surveyTypeIDinitial = (from a in db.Assessment    //2
                                       where a.Id == AssessmentIDinitial && a.ClientId == Cl.ClientId
                                       select a.SurveyTypeID).FirstOrDefault(); //fetching survey TypeID for above assessment

            //Only finalized Question set should be available for Data Gathering
            //get latest finalized version for the assessment type

            //var versionIDinitial = (from a in db.EngagementQuestionSet //Ver - 0.3
            //                        where a.Assessment_ID == surveyTypeIDinitial && a.Status == "Finalized"
            //                        select a.VersionID).FirstOrDefault(); // may need change

            var versionIDinitial = (from a in db.Assessment
                                    where a.Id == AssessmentIDinitial && a.ClientId == Cl.ClientId
                                    select a.QuestionaireVersion).FirstOrDefault();


            //get list of questions from Question and Response table for AssessmentID and store it in Questionset field


            List<A3MMModel> qSET = new List<A3MMModel>();

            var questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionIDinitial
                                select a).ToList(); //fetching questions created for Ver - 0.3 and type 2

            List<QuestionnaireResponse> responseList = new List<QuestionnaireResponse>();




            if (questionList.Count != 0)
            {

                responseList = (from a in db.QuestionnaireResponse
                                where a.SurveyTypeId == surveyTypeIDinitial && a.AssessmentID == AssessmentIDinitial && a.VersionID == versionIDinitial && a.ClientID == Cl.ClientId
                                select a).ToList(); //One Assessment Name can be linked to only One Assessment Type


                foreach (var a in responseList)
                {
                    EngagementQuestionnaire e = (from b in questionList
                                                 where b.QNo == a.QNo && b.SurveyTypeID == a.SurveyTypeId
                                                 select b).FirstOrDefault();
                    List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                               where x.QuestionRespId == a.Id
                                                               select new ResponseAttachment
                                                               {
                                                                   Id = x.Id,
                                                                   filename = x.FileName,
                                                                   fileCont = x.AttachFile
                                                               }).ToList();


                    if (e != null)
                    {
                        A3MMModel a3mm = new A3MMModel();
                        a3mm.QNo = a.QNo;
                        a3mm.SurveyTypeID = e.SurveyTypeID;
                        a3mm.AssessmentID = a.AssessmentID;
                        a3mm.DisciplineId = Convert.ToInt32(e.Discipline_ID);
                        a3mm.DisciplineName = e.DisciplineName;
                        a3mm.ComponentId = Convert.ToInt32(e.Component_ID);
                        a3mm.ComponentName = e.ComponentName;
                        a3mm.AspectID = e.AspectID;
                        a3mm.AspectName = e.AspectName;
                        a3mm.Question = e.Question;
                        a3mm.ExampleAnswer = e.ExampleAns;
                        a3mm.Response = a.Response;
                        a3mm.AssessmentResponse = a.AssessmentResponse;
                        a3mm.Feedback = a.Feedback;
                        a3mm.FollowupQuestion = a.FollowUpQuestion;
                        a3mm.MaturityLevel = a.MaturityLevel;
                        //a3mm.MaturityLevelInt = Convert.ToInt32(a.MaturityLevel);
                        a3mm.AreasofImprovement = a.AreaOfImprovement;
                        a3mm.QuestionId = e.Id;
                        a3mm.ClientId = 1;
                        a3mm.QuestionResponseID = a.Id;
                        a3mm.LastFeedbackBy = Convert.ToInt32(a.LastFeedBackBy);
                        a3mm.LastFeedbackOn = a.LastFeedBackOn;
                        a3mm.LastUpdatedBy = Convert.ToInt32(a.LastUpdatedBy);
                        a3mm.LastUpdatedOn = a.LastUpdatedOn;
                        a3mm.VersionID = a.VersionID;
                        a3mm.DatasourceId = e.DataSourceID;
                        a3mm.SQNo = e.SQNo;
                        a3mm.SubQOrder = e.SubQOrder;
                        a3mm.QuestionOrder = e.QuestionOrder;
                        a3mm.RespAttachment = RespAttachment;
                        qSET.Add(a3mm);
                    }



                }
            }
            if (qSET.Count != 0)
            {
                ret.QuestionSet = qSET.OrderBy(e => e.QuestionOrder).ToList();
                ret.SurveyTypeID = qSET[0].SurveyTypeID;
                ret.AssessmentID = qSET[0].AssessmentID;
                ret.Status = (from a in db.Assessment
                              where a.Id == AssessmentIDinitial && a.SurveyTypeID == surveyTypeIDinitial
                              select a.Status).FirstOrDefault();
                ret.Question = qSET[0].Question;
                ret.Response = qSET[0].Response;
                ret.AspectID = qSET[0].AspectID;
                ret.DisciplineId = qSET[0].DisciplineId;
                ret.ComponentId = qSET[0].ComponentId;
                ret.QNo = qSET[0].QNo;
                ret.SQNo = qSET[0].SQNo;
                ret.ClientId = qSET[0].ClientId;
                ret.VersionID = qSET[0].VersionID;
                ret.MaturityLevel = qSET[0].MaturityLevel;
                //ret.MaturityLevelInt = qSET[0].MaturityLevelInt;
                ret.FollowupQuestion = qSET[0].FollowupQuestion;
                ret.Response = qSET[0].Response;
                ret.AssessmentResponse = qSET[0].AssessmentResponse;
                ret.Feedback = qSET[0].Feedback;
                ret.AreasofImprovement = qSET[0].AreasofImprovement;
                ret.DatasourceId = qSET[0].DatasourceId;
                ret.LastFeedbackBy = qSET[0].LastFeedbackBy;
                ret.LastFeedbackOn = qSET[0].LastFeedbackOn;
                ret.LastUpdatedBy = qSET[0].LastUpdatedBy;
                ret.LastUpdatedOn = qSET[0].LastUpdatedOn;
                ret.RespAttachment = qSET[0].RespAttachment;
                ret.LastUpdatedByName = (from a in db.UserProfiles
                                         where a.Id == ret.LastUpdatedBy
                                         select a.UserName).FirstOrDefault();

                ret.LastFeedbackByName = (from a in db.UserProfiles
                                          where a.Id == ret.LastFeedbackBy
                                          select a.UserName).FirstOrDefault();
            }

            List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                       where a.SurveyTypeID == surveyTypeIDinitial && a.ClientId == Cl.ClientId
                                                       select new SelectListItem
                                                       {
                                                           Text = a.AssessmentName,
                                                           Value = a.Id.ToString()
                                                       }).ToList();

            ret.AssessmentNameList = AssessmentNameList;
            ret.SurveyTypeID = surveyTypeIDinitial;


            if (questionList.Count == 0)
            {
                ret.SQNo = "-";
            }

            var currentUserRoleList = (from u in db.UserApplications
                                       join r in db.UserRoles on u.RoleId equals r.Id
                                       where u.ClientId == Cl.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                       select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole = "";


            var Item = currentUserRoleList.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item != null)
                UserRole = Item.Text;
            else
                UserRole = "";

            ret.UserRole = UserRole;

            return View(ret);

        }

        [HttpPost]
        public ActionResult GetAssessmentType(string AssessmentID)
        {
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            var UserId = (int)Session["UserId"];
            int assessID = Convert.ToInt32(AssessmentID); //id of Assessment Table
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();


            //New Approach

            A3MMModel ret = prepareCommonParameters(); // fetching dropdown values
            A3MMModel initial = new A3MMModel();
            var AssessmentIDinitial = assessID;  //recieved as parameter

            var surveyTypeIDinitial = (from a in db.Assessment
                                       where a.Id == AssessmentIDinitial && a.ClientId == Cl.ClientId
                                       select a.SurveyTypeID).FirstOrDefault();


            var versionID = (from a in db.Assessment
                             where a.Id == AssessmentIDinitial && a.ClientId == Cl.ClientId
                             select a.QuestionaireVersion).FirstOrDefault();

            //Only finalized Question set should be available for Data Gathering
            //get latest finalized version for the assessment type

            //var versionIDinitial = (from a in db.EngagementQuestionSet
            //                        where a.Assessment_ID == surveyTypeIDinitial  && a.Status == "Finalized"
            //                        select a.VersionID).FirstOrDefault(); // may need change




            //get list of questions from Question and Response table for AssessmentID and store it in Questionset field


            List<A3MMModel> qSET = new List<A3MMModel>();

            var questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID
                                select a).ToList();

            var responseList = (from a in db.QuestionnaireResponse
                                where a.SurveyTypeId == surveyTypeIDinitial && a.AssessmentID == AssessmentIDinitial && a.VersionID == versionID && a.ClientID == Cl.ClientId
                                select a).ToList();


            foreach (var a in responseList)
            {
                EngagementQuestionnaire e = (from b in questionList
                                             where b.SQNo == a.SQNo && b.SurveyTypeID == a.SurveyTypeId
                                             select b).FirstOrDefault();
                List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                           where x.QuestionRespId == a.Id
                                                           select new ResponseAttachment
                                                           {
                                                               Id = x.Id,
                                                               filename = x.FileName,
                                                               fileCont = x.AttachFile
                                                           }).ToList();


                if (e != null)
                {
                    A3MMModel a3mm = new A3MMModel();
                    a3mm.QNo = e.QNo;
                    a3mm.SurveyTypeID = a.SurveyTypeId;
                    a3mm.AssessmentID = a.AssessmentID;
                    a3mm.DisciplineId = Convert.ToInt32(e.Discipline_ID);
                    a3mm.DisciplineName = e.DisciplineName;
                    a3mm.ComponentId = Convert.ToInt32(e.Component_ID);
                    a3mm.ComponentName = e.ComponentName;
                    a3mm.AspectID = e.AspectID;
                    a3mm.AspectName = e.AspectName;
                    a3mm.Question = e.Question;
                    a3mm.ExampleAnswer = e.ExampleAns;
                    a3mm.Response = a.Response;
                    a3mm.AssessmentResponse = a.AssessmentResponse;
                    a3mm.Feedback = a.Feedback;
                    a3mm.FollowupQuestion = a.FollowUpQuestion;
                    a3mm.MaturityLevel = a.MaturityLevel;
                    //a3mm.MaturityLevelInt = Convert.ToInt32(a.MaturityLevel);
                    a3mm.AreasofImprovement = a.AreaOfImprovement;
                    a3mm.QuestionId = e.Id;
                    a3mm.ClientId = a.ClientID;
                    a3mm.QuestionResponseID = a.Id;
                    a3mm.LastFeedbackBy = Convert.ToInt32(a.LastFeedBackBy);
                    a3mm.LastFeedbackOn = a.LastFeedBackOn;
                    a3mm.LastUpdatedBy = Convert.ToInt32(a.LastUpdatedBy);
                    a3mm.LastUpdatedOn = a.LastUpdatedOn;
                    a3mm.VersionID = a.VersionID;
                    a3mm.DatasourceId = e.DataSourceID;
                    a3mm.SQNo = e.SQNo;
                    a3mm.SubQOrder = e.SubQOrder;
                    a3mm.QuestionOrder = e.QuestionOrder;
                    a3mm.RespAttachment = RespAttachment;

                    qSET.Add(a3mm);
                }


            }

            if (qSET.Count != 0)
            {
                ret.QuestionSet = qSET.OrderBy(e => e.QuestionOrder).ToList();
                ret.SurveyTypeID = qSET[0].SurveyTypeID;
                ret.AssessmentID = qSET[0].AssessmentID;
                ret.Status = (from a in db.Assessment
                              where a.Id == AssessmentIDinitial && a.SurveyTypeID == surveyTypeIDinitial
                              select a.Status).FirstOrDefault();
                qSET[0].Status = ret.Status;
                ret.AspectID = qSET[0].AspectID;
                ret.DisciplineId = qSET[0].DisciplineId;
                ret.ComponentId = qSET[0].ComponentId;
                ret.QNo = qSET[0].QNo;
                ret.ClientId = qSET[0].ClientId;
                ret.VersionID = qSET[0].VersionID;
                ret.MaturityLevel = qSET[0].MaturityLevel;
                //ret.MaturityLevelInt = qSET[0].MaturityLevelInt;
                ret.FollowupQuestion = qSET[0].FollowupQuestion;
                ret.Response = qSET[0].Response;
                ret.Feedback = qSET[0].Response;
                ret.AreasofImprovement = qSET[0].AreasofImprovement;
                ret.DatasourceId = qSET[0].DatasourceId;
                ret.LastFeedbackBy = qSET[0].LastFeedbackBy;
                ret.LastFeedbackOn = qSET[0].LastFeedbackOn;
                ret.LastUpdatedBy = qSET[0].LastUpdatedBy;
                ret.LastUpdatedOn = qSET[0].LastUpdatedOn;
                ret.RespAttachment = qSET[0].RespAttachment;
                ret.LastUpdatedByName = (from a in db.UserProfiles
                                         where a.Id == ret.LastUpdatedBy
                                         select a.UserName).FirstOrDefault();
                qSET[0].LastUpdatedByName = ret.LastUpdatedByName;
                ret.LastFeedbackByName = (from a in db.UserProfiles
                                          where a.Id == ret.LastFeedbackBy
                                          select a.UserName).FirstOrDefault();
                qSET[0].LastFeedbackByName = ret.LastFeedbackByName;

            }


            var currentUserRoleList = (from u in db.UserApplications
                                       join r in db.UserRoles on u.RoleId equals r.Id
                                       where u.ClientId == Cl.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                       select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole = "";


            var Item1 = currentUserRoleList.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item1 != null)
                UserRole = Item1.Text;
            else
                UserRole = "";

            ret.QuestionSet[0].UserRole = UserRole;



            return Json(ret.QuestionSet, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetAssessmentNameList(string AssessmentTypeID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int assessTypeID = Convert.ToInt32(AssessmentTypeID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();

            List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                       where a.SurveyTypeID == assessTypeID && a.ClientId == client.ClientId
                                                       select new SelectListItem
                                                       {
                                                           Text = a.AssessmentName,
                                                           Value = a.Id.ToString()
                                                       }).ToList();
            A3MMModel a3 = new A3MMModel();
            a3.AssessmentList = AssessmentNameList;
            if(AssessmentNameList.Count != 0) {
                int assessmentid = Convert.ToInt32(AssessmentNameList[0].Value);
                var Assessmentdetail = (from a in db.Assessment
                                        where a.Id == assessmentid
                                        select a).FirstOrDefault();
                a3.VersionID = Assessmentdetail.QuestionaireVersion;
                a3.Status = Assessmentdetail.Status;
            }


            //var version = (from a in db.EngagementQuestionSet
            //               where a.Assessment_ID == assessTypeID && a.Status == "Finalized" && a.isdeleted == 0
            //               select a.VersionID).FirstOrDefault();

            //a3.VersionID = version;

            var currentUserRoleList1 = (from u in db.UserApplications
                                        join r in db.UserRoles on u.RoleId equals r.Id
                                        where u.ClientId == client.ClientId && u.UserId == uid && u.IsDeleted == false && u.ApplicationId == 20
                                        select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole1 = "";


            var Item1 = currentUserRoleList1.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item1 != null)
                UserRole1 = Item1.Text;
            else
                UserRole1 = "";

            a3.UserRole = UserRole1;
            return Json(a3, JsonRequestBehavior.AllowGet);

        }





        public ActionResult GetVersionList(string AssessmentTypeID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int assessTypeID = Convert.ToInt32(AssessmentTypeID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();

            List<SelectListItem> versionlist = (from a in db.EngagementQuestionSet
                                                where a.Assessment_ID == assessTypeID && a.isdeleted == 0
                                                select new SelectListItem
                                                {
                                                    Text = a.VersionID,
                                                    Value = a.VersionID.ToString()
                                                }).ToList();



            //A3MMModel a3 = new A3MMModel();
            //a3.AssessmentList = AssessmentNameList;

            //var version = (from a in db.EngagementQuestionSet
            //               where a.Assessment_ID == assessTypeID && a.Status == "Finalized"
            //               select a.VersionID).FirstOrDefault();

            //a3.VersionID = version;

            return Json(versionlist, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetPageClickAssessmentDetail(string AssessmentID, string SQNo) //need change
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int assessID = Convert.ToInt32(AssessmentID); //Id of Assessment Table
            //int questionNumber = Convert.ToInt32(QNo);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            int surveyTypeID = new int();
            A3MMModel a3 = new A3MMModel();
            surveyTypeID = (from a in db.Assessment
                            where a.Id == assessID
                            select a.SurveyTypeID).FirstOrDefault();


            //New Approach refer history for old


            //var versionIDinitial = (from a in db.EngagementQuestionSet
            //                        where a.Assessment_ID == surveyTypeID && a.Status == "Finalized"
            //                        select a.VersionID).FirstOrDefault(); // may need change

            var versionIDinitial = (from a in db.Assessment
                                    where a.SurveyTypeID == surveyTypeID && a.Id == assessID
                                    select a.QuestionaireVersion).FirstOrDefault();

            var questionRecord = (from a in db.EngagementQuestionnaire
                                  where a.SQNo == SQNo && a.VersionID == versionIDinitial && a.SurveyTypeID == surveyTypeID
                                  select a).FirstOrDefault();


            var responseRecord = (from a in db.QuestionnaireResponse
                                  where a.SQNo == SQNo && a.AssessmentID == assessID && a.VersionID == versionIDinitial && a.ClientID == client.ClientId && a.SurveyTypeId == surveyTypeID
                                  select a).FirstOrDefault();
            List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                       where x.QuestionRespId == responseRecord.Id
                                                       select new ResponseAttachment
                                                       {
                                                           Id = x.Id,
                                                           filename = x.FileName,
                                                           fileCont = x.AttachFile
                                                       }).ToList();

            A3MMModel ret = prepareCommonParameters();
            ret.QNo = questionRecord.QNo;
            ret.SQNo = questionRecord.SQNo;
            ret.ClientId = responseRecord.ClientID;
            ret.DatasourceId = questionRecord.DataSourceID;
            ret.Question = questionRecord.Question;
            ret.ExampleAnswer = questionRecord.ExampleAns;
            ret.VersionID = questionRecord.VersionID;
            ret.AspectID = questionRecord.AspectID;
            ret.ComponentId = Convert.ToInt32(questionRecord.Component_ID);
            ret.DisciplineId = Convert.ToInt32(questionRecord.Discipline_ID);
            ret.AspectName = questionRecord.AspectName;
            ret.ComponentName = questionRecord.ComponentName;
            ret.DisciplineName = questionRecord.DisciplineName;
            //ret.QuestionSet = (List<A3MMModel>)res;
            ret.Feedback = responseRecord.Feedback;
            ret.Response = responseRecord.Response;
            ret.AssessmentResponse = responseRecord.AssessmentResponse;
            ret.AreasofImprovement = responseRecord.AreaOfImprovement;
            ret.FollowupQuestion = responseRecord.FollowUpQuestion;
            ret.LastFeedbackBy = Convert.ToInt32(responseRecord.LastFeedBackBy);
            ret.LastUpdatedBy = Convert.ToInt32(responseRecord.LastUpdatedBy);
            ret.LastFeedbackOn = responseRecord.LastUpdatedOn;
            ret.LastUpdatedOn = responseRecord.LastUpdatedOn;
            ret.MaturityLevel = responseRecord.MaturityLevel;
            //ret.MaturityLevelInt = Convert.ToInt32(responseRecord.MaturityLevel);
            ret.Status = (from a in db.Assessment
                          where a.Id == assessID
                          select a.Status).FirstOrDefault();
            var LastUpdatedByName1 = (from a in db.UserProfiles
                                      where a.Id == ret.LastUpdatedBy
                                      select a.UserName).FirstOrDefault();

            var LastFeedbackByName1 = (from a in db.UserProfiles
                                       where a.Id == ret.LastFeedbackBy
                                       select a.UserName).FirstOrDefault();

            ret.LastUpdatedByName = LastUpdatedByName1;
            ret.LastFeedbackByName = LastFeedbackByName1;
            ret.RespAttachment = RespAttachment;
            return Json(ret, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetQuestionSet([DataSourceRequest] DataSourceRequest request)
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            AssessmentModel1 am = new AssessmentModel1();
            A3MMModel c = new A3MMModel();
            am = (from a in db.Assessment
                  select new AssessmentModel1
                  {
                      Id = a.Id,
                      EngagementName = a.EngagementName,
                  }
                 ).ToList().FirstOrDefault();

            if (am != null)
            {
                c.AssessmentID = am.Id;
            }

            var surveyTypeID = (from a in db.Assessment
                                where a.Id == am.Id
                                select a.SurveyTypeID).FirstOrDefault();

            var res = AssessmentManagerHelper.GetQuestionSet(am.Id, surveyTypeID);
            c.QuestionSet = (List<A3MMModel>)res;

            return Json(c.QuestionSet.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionTrack]
        public ActionResult SaveDataGathering(A3MMModel AM)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            AM.ClientId = client.ClientId;
            AM.QuestionId = (from a in db.EngagementQuestionnaire
                             where a.SQNo == AM.SQNo && a.SurveyTypeID == AM.SurveyTypeID && a.VersionID == AM.VersionID
                             select a.Id).FirstOrDefault();

            AM.QuestionResponseID = (from a in db.QuestionnaireResponse
                                     where a.SQNo == AM.SQNo && a.AssessmentID == AM.AssessmentID && a.SurveyTypeId == AM.SurveyTypeID && a.VersionID == AM.VersionID && a.ClientID == client.ClientId
                                     select a.Id).FirstOrDefault();

            AM.ActionItemID = (from a in db.ActionItemTracker
                               where a.AssessmentID == AM.AssessmentID && a.QResID == AM.QuestionResponseID && a.ClientID == AM.ClientId
                               select a.Id).FirstOrDefault();

            AM.LastFeedbackBy = usid;
            AM.LastUpdatedBy = usid;
            AM.LastFeedbackOn = DateTime.Now;
            AM.LastUpdatedOn = DateTime.Now;

            var res = AssessmentManagerHelper.saveDataGathering(AM);


            //New Approach

            A3MMModel ret = prepareCommonParameters(); // fetching dropdown values
            A3MMModel initial = new A3MMModel();

            //var AssessmentIDinitial = (from a in db.Assessment
            //                           select a.Id).FirstOrDefault(); //get first assessment ID


            var AssessmentIDinitial = AM.AssessmentID; //get first assessment ID

            var surveyTypeIDinitial = (from a in db.Assessment
                                       where a.Id == AssessmentIDinitial
                                       select a.SurveyTypeID).FirstOrDefault();

            //Only finalized Question set should be available for Data Gathering
            //get latest finalized version for the assessment type

            //var versionIDinitial = (from a in db.EngagementQuestionSet
            //                        where a.Assessment_ID == surveyTypeIDinitial
            //                        select a.VersionID).FirstOrDefault(); // may need change

            var versionIDinitial = (from a in db.Assessment
                                    where a.Id == AssessmentIDinitial
                                    select a.QuestionaireVersion).FirstOrDefault(); // may need change


            //get list of questions from Question and Response table for AssessmentID and store it in Questionset field


            List<A3MMModel> qSET = new List<A3MMModel>();
            List<EngagementQuestionnaire> questionList = new List<EngagementQuestionnaire>();
            if (AM.isAspChanged == 1 && AM.AspectID > 0)
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionIDinitial && a.AspectID == AM.AspectID
                                select a).ToList();
            else if (AM.iscompChanged == 1 && AM.ComponentId > 0)
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionIDinitial && a.Component_ID == AM.ComponentId
                                select a).ToList();
            else if (AM.isdiscChanged == 1 && AM.DisciplineId > 0)
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionIDinitial && a.Discipline_ID == AM.DisciplineId
                                select a).ToList();
            else
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionIDinitial
                                select a).ToList();

            var responseList = (from a in db.QuestionnaireResponse
                                where a.SurveyTypeId == surveyTypeIDinitial && a.AssessmentID == AM.AssessmentID && a.VersionID == versionIDinitial && a.ClientID == client.ClientId
                                select a).ToList();


            foreach (var a in responseList)
            {
                EngagementQuestionnaire e = (from b in questionList
                                             where b.SQNo == a.SQNo && b.SurveyTypeID == a.SurveyTypeId
                                             select b).FirstOrDefault();

                if (e != null)
                {
                    A3MMModel a3mm = new A3MMModel();
                    a3mm.QNo = e.QNo;
                    //
                    a3mm.SQNo = e.SQNo;
                    a3mm.QuestionOrder = e.QuestionOrder;
                    a3mm.SubQOrder = e.SubQOrder;
                    //
                    a3mm.SurveyTypeID = a.SurveyTypeId;
                    a3mm.AssessmentID = a.AssessmentID;
                    a3mm.DisciplineId = Convert.ToInt32(e.Discipline_ID);
                    a3mm.DisciplineName = e.DisciplineName;
                    a3mm.ComponentId = Convert.ToInt32(e.Component_ID);
                    a3mm.ComponentName = e.ComponentName;
                    a3mm.AspectID = e.AspectID;
                    a3mm.AspectName = e.AspectName;
                    a3mm.Question = e.Question;
                    a3mm.ExampleAnswer = e.ExampleAns;
                    a3mm.Response = a.Response;
                    a3mm.Feedback = a.Feedback;
                    a3mm.FollowupQuestion = a.FollowUpQuestion;
                    a3mm.MaturityLevel = a.MaturityLevel;
                    //a3mm.MaturityLevelInt = Convert.ToInt32(a.MaturityLevel);
                    a3mm.AreasofImprovement = a.AreaOfImprovement;
                    a3mm.QuestionId = e.Id;
                    a3mm.ClientId = a.ClientID;
                    a3mm.QuestionResponseID = a.Id;
                    a3mm.LastFeedbackBy = Convert.ToInt32(a.LastFeedBackBy);
                    a3mm.LastFeedbackOn = a.LastFeedBackOn;
                    a3mm.LastUpdatedBy = Convert.ToInt32(a.LastUpdatedBy);
                    a3mm.LastUpdatedOn = a.LastUpdatedOn;
                    a3mm.VersionID = a.VersionID;
                    a3mm.DatasourceId = e.DataSourceID;

                    qSET.Add(a3mm);
                }


            }
            var orderqset = qSET.OrderBy(e => e.QuestionOrder).ThenBy(e => e.SQNo).ToList();
            //if (AM.isEditdrpdwnchange)
            //{
            //    A3MMModel qcurrent = (from a in orderqset
            //                          where a.SQNo == AM.SQNo &&
            //                          select a).FirstOrDefault();
            //}
            A3MMModel qcurrent = (from a in orderqset
                                  where a.SQNo == AM.SQNo //&& a.QuestionOrder > AM.QNo
                                  select a).FirstOrDefault();
            var qcount = 0;
            if (orderqset.Count() == orderqset.IndexOf(qcurrent) + 1)
            {
                qcount = orderqset.IndexOf(qcurrent);
            }
            else
            {
                qcount = orderqset.IndexOf(qcurrent) + 1;
            }
            A3MMModel qNXT = orderqset[qcount];
            return Json(qNXT, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionTrack]
        public ActionResult Save_Reference(IEnumerable<HttpPostedFileBase> RefFiles, string SQNo, int AssessmentID, int SurveyTypeID, string VersionID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            byte[] filecnt = null;
            var filename = "";
            if (RefFiles != null)
            {
                Stream filestream = RefFiles.First().InputStream;
                filename = RefFiles.First().FileName;
                BinaryReader br = new BinaryReader(filestream);
                Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                filecnt = bytes;
            }

            int fileId = 0;
            Db db = new Db();
            int QuestionResponseID = (from a in db.QuestionnaireResponse
                                      where a.SQNo == SQNo && a.AssessmentID == AssessmentID && a.SurveyTypeId == SurveyTypeID && a.VersionID == VersionID && a.ClientID == client.ClientId
                                      select a.Id).FirstOrDefault();
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                QuestRespAttachments rspAttach = new QuestRespAttachments();
                rspAttach.FileCnt = filecnt;
                rspAttach.ClientId = client.ClientId;
                rspAttach.FileName = filename;
                rspAttach.QuestionRespId = QuestionResponseID;
                unitOfWork.QuestRespAttachments.Insert(rspAttach);

                unitOfWork.Save();
                fileId = rspAttach.Id;
            }

            return Content(fileId.ToString());
        }
        [HttpPost]
        [ActionTrack]
        public ActionResult RemoveFile(int fileId)
        {
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.QuestRespAttachments.Delete(fileId);
                unitOfWork.Save();
            }
            return Json(fileId, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RedirectToAssessOverview(string AssessmentID, string QNo)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int assessID = Convert.ToInt32(AssessmentID);
            int questionNumber = Convert.ToInt32(QNo);


            var ret = "window.location='AssessmentOverview?AssessmentIDparam=" + assessID + "&QNoparam=" + questionNumber + "'";
            //return JavaScript("window.location = 'DataGathering?AssessmentIDparam=15&QNoparam=3'");
            return JavaScript(ret);

        }


        //Assessment Overview Screen
        [ActionTrack]
        public ActionResult AssessmentOverview(int AssessmentIDparam = 0, int QNoparam = 0)
        {
            var UserId = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int clientID = client.ClientId;
            //ViewData["GetStringResult"] = getFullName(UserId);
            A3MMModel c = prepareCommonParameters();
            A3MMModel getOne = new A3MMModel();
            Db db = new Db();

            var currentUserRoleList1 = (from u in db.UserApplications
                                        join r in db.UserRoles on u.RoleId equals r.Id
                                        where u.ClientId == client.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                        select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole1 = "";


            var Item1 = currentUserRoleList1.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item1 != null)
                UserRole1 = Item1.Text;
            else
                UserRole1 = "";




            if (AssessmentIDparam != 0)
            {
                A3MMModel getOne2 = new A3MMModel();
                var surveyTypeID1 = (from a in db.Assessment
                                     where a.Id == AssessmentIDparam
                                     select a.SurveyTypeID).FirstOrDefault();

                var versionID1 = (from a in db.Assessment
                                  where a.Id == AssessmentIDparam
                                  select a.QuestionaireVersion).FirstOrDefault();

                var status1 = (from a in db.Assessment
                               where a.Id == AssessmentIDparam
                               select a.Status).FirstOrDefault();

                var res1 = AssessmentManagerHelper.GetQuestionSet(AssessmentIDparam, surveyTypeID1);

                A3MMModel a3 = new A3MMModel();
                A3MMModel a31 = new A3MMModel();

                a3 = (from a in res1
                      where a.QNo == QNoparam
                      select a).FirstOrDefault();

                a31 = prepareCommonParameters();

                a3.DisciplineList = a31.DisciplineList;
                a3.ComponentList = a31.ComponentList;
                a3.AspectList = a31.AspectList;
                a3.AssessmentList = a31.AssessmentList;
                a3.AccountNamesList = a31.AccountNamesList;
                a3.AssessmentNameList = a31.AssessmentNameList;
                a3.ContractList = a31.ContractList;
                a3.DataSourceList = a31.DataSourceList;

                var LastUpdatedByName1 = (from a in db.UserProfiles
                                          where a.Id == a3.LastUpdatedBy
                                          select a.UserName).FirstOrDefault();

                var LastFeedbackByName1 = (from a in db.UserProfiles
                                           where a.Id == a3.LastFeedbackBy
                                           select a.UserName).FirstOrDefault();



                getOne2 = (from d in db.Discipline
                           join comp in db.Component on d.Id equals comp.DisciplineId
                           join a in db.Aspect on comp.Id equals a.ComponentId
                           where a.Id == a3.AspectID
                           select
                           new A3MMModel
                           {
                               DisciplineId = d.Id,
                               AspectID = a.Id,
                               ComponentId = a.ComponentId,
                               DisciplineName = d.DisciplineName,
                               ComponentName = comp.ComponentName,
                               AspectName = a.AspectName,
                               //ClientId = a.ClientId
                           }).FirstOrDefault();

                a3.AspectID = getOne2.AspectID;
                a3.ComponentId = getOne2.ComponentId;
                a3.DisciplineId = getOne2.DisciplineId;
                a3.AspectName = getOne2.AspectName;
                a3.ComponentName = getOne2.ComponentName;
                a3.DisciplineName = getOne2.DisciplineName;

                a3.QuestionSet = (List<A3MMModel>)AssessmentManagerHelper.GetQuestionSet(AssessmentIDparam, surveyTypeID1);

                a3.LastUpdatedByName = LastUpdatedByName1;
                a3.LastFeedbackByName = LastFeedbackByName1;
                if (a3.LastUpdatedByName == null)
                {
                    a3.LastUpdatedByName = "";
                }
                if (a3.LastFeedbackByName == null)
                {
                    a3.LastFeedbackByName = "";
                }

                a3.VersionID = versionID1;
                a3.Status = status1;
                a3.SurveyTypeID = surveyTypeID1;
                a3.UserRole = UserRole1;
                return View(a3);
            }




            //Required code below, above code might not be required


            var assessmentID = (from a in db.Assessment
                                where a.ClientId == clientID
                                select a.Id).FirstOrDefault();

            var surveyTypeIDinitial = (from a in db.Assessment
                                       where a.ClientId == clientID
                                       select a.SurveyTypeID).FirstOrDefault();

            var versionID = (from a in db.Assessment
                             where a.ClientId == clientID
                             select a.QuestionaireVersion).FirstOrDefault();

            var status = (from a in db.Assessment
                          where a.ClientId == clientID
                          select a.Status).FirstOrDefault();


            c.AssessmentID = assessmentID;
            c.SurveyTypeID = surveyTypeIDinitial;
            c.VersionID = versionID;
            c.Status = status;

            List<SelectListItem> AssessmentNameList1 = new List<SelectListItem>();

            if (surveyTypeIDinitial != 0)
            {
                AssessmentNameList1 = (from a in db.Assessment
                                       where a.SurveyTypeID == surveyTypeIDinitial && a.ClientId == clientID
                                       select new SelectListItem
                                       {
                                           Text = a.AssessmentName,
                                           Value = a.Id.ToString()
                                       }).ToList();
            }
            c.AssessmentNameList = AssessmentNameList1;
            c.UserRole = UserRole1;


            return View(c);

        }



        [HttpPost]
        public ActionResult Export_Assessment_Overview_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public ActionResult GetAssessmentOverview([DataSourceRequest] DataSourceRequest request, string SurveyTypeID, string AssessmentID)
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            int clientid = Cl.ClientId;
            Db db = new Db();
            int assessmentID = Convert.ToInt32(AssessmentID);
            int surveyTypeID = Convert.ToInt32(SurveyTypeID);

            //var versionIDinitial = (from a in db.EngagementQuestionSet
            //                        where a.Assessment_ID == surveyTypeID && a.Status == "Finalized"
            //                        select a.VersionID).FirstOrDefault(); // may need change

            var versionIDinitial = (from a in db.Assessment
                                    where a.Id == assessmentID
                                    select a.QuestionaireVersion).FirstOrDefault();


            var qlist = (from a in db.QuestionnaireResponse
                         where a.AssessmentID == assessmentID && a.SurveyTypeId == surveyTypeID && a.VersionID == versionIDinitial && a.ClientID == clientid
                         select a).ToList();


            var eQlist = (from a in db.EngagementQuestionnaire
                          where a.SurveyTypeID == surveyTypeID && a.VersionID == versionIDinitial
                          select a).ToList();

            IList<A3MMModel> a3list = new List<A3MMModel>();

            foreach (var a in qlist) //72
            {
                EngagementQuestionnaire e = (from b in eQlist
                                             where b.SQNo == a.SQNo && b.SurveyTypeID == a.SurveyTypeId
                                             select b).FirstOrDefault();
                List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                           where x.QuestionRespId == a.Id
                                                           select new ResponseAttachment
                                                           {
                                                               Id = x.Id,
                                                               filename = x.FileName,
                                                               fileCont = x.AttachFile
                                                           }).ToList();


                if (e != null)
                {
                    A3MMModel a3mm = new A3MMModel();
                    a3mm.QNo = a.QNo;
                    //
                    a3mm.Id = e.Id;
                    a3mm.SQNo = e.SQNo;
                    a3mm.ParentID = e.ParentID;
                    //
                    a3mm.SurveyTypeID = a.SurveyTypeId;
                    a3mm.AssessmentID = a.AssessmentID;
                    a3mm.DisciplineName = e.DisciplineName;
                    a3mm.ComponentName = e.ComponentName;
                    a3mm.AspectName = e.AspectName;
                    a3mm.AspectID = e.AspectID;
                    a3mm.Question = e.Question;
                    a3mm.ExampleAnswer = e.ExampleAns;
                    a3mm.Response = GetPlainText((from mr in db.A3MM_MultipleResponses
                                                  where mr.AssessmentId == assessmentID && mr.ResponseID == a.Id
                                                  select mr.SubResponse).ToList().FirstOrDefault()); ;
                    // a3mm.Response = GetPlainText(a.Response);
                    // a3mm.Feedback = GetPlainText(a.Feedback);
                    //a3mm.Feedback = GetPlainText((from mf in db.A3MM_MultipleFollowUpQuestions
                    //                 where mf.AssessmentId == assessmentID && mf.ResponseID == a.Id
                    //                 select mf.SubFollowUpQuestion).FirstOrDefault());
                    a3mm.MaturityLevel = a.MaturityLevel;
                    a3mm.AreasofImprovement = GetPlainText(a.AreaOfImprovement);
                    a3mm.QuestionId = e.Id;
                    a3mm.ClientId = 1;
                    a3mm.QuestionResponseID = a.Id;
                    a3mm.RespAttachment = RespAttachment;
                    a3list.Add(a3mm);
                }
            }

            var result = a3list.ToTreeDataSourceResult(request,
                e => e.Id,
                e => e.ParentID,
                e => e
            );

            return Json(result, JsonRequestBehavior.AllowGet);



            //return Json(a3list.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult GetVersionAndStatus(string AssessmentID)
        {
            var UserId = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int assessID = Convert.ToInt32(AssessmentID);
            Db db = new Db();
            A3MMModel a3 = new A3MMModel();
            var Assessmentdetail = (from a in db.Assessment
                                    where a.Id == assessID
                                    select a).FirstOrDefault();

            var res = AssessmentManagerHelper.GetQuestionSet(assessID, Assessmentdetail.SurveyTypeID);
            a3.VersionID = Assessmentdetail.QuestionaireVersion;
            a3.Status = Assessmentdetail.Status;


            var currentUserRoleList1 = (from u in db.UserApplications
                                        join r in db.UserRoles on u.RoleId equals r.Id
                                        where u.ClientId == client.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                        select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole1 = "";


            var Item1 = currentUserRoleList1.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item1 != null)
                UserRole1 = Item1.Text;
            else
                UserRole1 = "";
            a3.UserRole = UserRole1;

            return Json(a3, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult RedirectToDG(string AssessmentID, string QNo, string SurveyTypeID) //AssessmentOverview Screen
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int assessID = Convert.ToInt32(AssessmentID);
            int questionNumber = Convert.ToInt32(QNo);
            int SurveyTypeID1 = Convert.ToInt32(SurveyTypeID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();


            var ret = "window.location='DataGathering?AssessmentIDparam=" + assessID + "&QNoparam=" + questionNumber + "&SurveyTypeID=" + SurveyTypeID1 + "'";
            //return JavaScript("window.location = 'DataGathering?AssessmentIDparam=15&QNoparam=3'");
            return JavaScript(ret);


        }


        public ActionResult RedirectToDGView(string AssessmentID, string QNo)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int assessID = Convert.ToInt32(AssessmentID);
            int questionNumber = Convert.ToInt32(QNo);
            int isView = 1;

            var ret = "window.location='DataGathering?AssessmentIDparam=" + assessID + "&QNoparam=" + questionNumber + "&isView=" + isView + "'";
            //return JavaScript("window.location = 'DataGathering?AssessmentIDparam=15&QNoparam=3'");
            return JavaScript(ret);


        }

        [HttpPost]
        public ActionResult CheckQReviewvalidation(int AssessmentID)
        {
            Db db = new Db();
            var AssessmentValue = (from a in db.Assessment
                                   where a.Id == AssessmentID
                                   select a).FirstOrDefault();


            var questionList = (from a in db.EngagementQuestionnaire
                                join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                where a.SurveyTypeID == AssessmentValue.SurveyTypeID && a.VersionID == AssessmentValue.QuestionaireVersion && b.AssessmentID == AssessmentValue.Id
                                select new Qset
                                {
                                    SQNo = a.SQNo,
                                    Questionorder = a.QuestionOrder.Value,
                                    QuestionStatus = b.responseStatus,
                                    AssessmentId = b.AssessmentID
                                }).ToList();

            var UnAnsQset = questionList.Where(x => x.QuestionStatus == "Unanswered").OrderBy(o => o.Questionorder).ToList();

            if (UnAnsQset.Count != 0)
            {
                var result = "";
                if (UnAnsQset.Count > 20)
                {
                    result = "More than 20 Questions are not responded, please refer the data gathering screen";
                }
                else
                {
                    // var e = string.Join(",", UnAnsQset);
                    var e = "";
                    foreach (var item in UnAnsQset)
                    {
                        e = e + item.SQNo + ",";
                    }
                    result = "Please enter response to the following Question(s) to release the assessment : " + e;
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult Checkvalidation(int AssessmentID)
        {
            Db db = new Db();

            var Qset = (from ait in db.QuestionnaireResponse
                        where ait.AssessmentID == AssessmentID
                        select ait).ToList();
            var hasMaturitylvl = Qset.Where(x => x.MaturityLevel == null).ToList();
            var QStatus = Qset.Where(x => x.responseStatus != "Completed").ToList();
            if (hasMaturitylvl.Count != 0 || QStatus.Count != 0)
            {
                var result = "";
                if (hasMaturitylvl.Count > 20)
                {
                    result = "Marturity level for more than 20 Questions are not assigned, please refer the data gathering screen";
                }
                else if (hasMaturitylvl.Count < 20 && hasMaturitylvl.Count != 0)
                {
                    var e = "";// string.Join(",", hasMaturitylvl);
                    foreach (var item in hasMaturitylvl)
                    {
                        e = e + item.SQNo + ",";
                    }
                    result = "Please enter Maturity level to the following Question(s) to publish the assessment : " + e;
                }
                else if (QStatus.Count != 0)
                {
                    result = "Please Complete all the question before publish, please refer the data gathering screen";

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult CheckQResvalidation(int AssessmentID)
        {

            //var isallResponsed = (from ait in db.QuestionnaireResponse
            //                      where ait.AssessmentID == AssessmentID && ait.Response == null
            //                      select ait).ToList();

            Db db = new Db();
            var AssessmentValue = (from a in db.Assessment
                                   where a.Id == AssessmentID
                                   select a).FirstOrDefault();


            var questionList = (from a in db.EngagementQuestionnaire
                                join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                where a.SurveyTypeID == AssessmentValue.SurveyTypeID && a.VersionID == AssessmentValue.QuestionaireVersion && b.AssessmentID == AssessmentValue.Id
                                select new Qset
                                {
                                    SQNo = a.SQNo,
                                    Questionorder = a.QuestionOrder.Value,
                                    QuestionStatus = b.responseStatus,
                                    AssessmentId = b.AssessmentID
                                }).ToList();

            var ResponseInProgressQset = questionList.Where(x => x.QuestionStatus == "ResponseInProgress").OrderBy(o => o.Questionorder).ToList();


            if (ResponseInProgressQset.Count != 0)
            {
                var result = "";
                if (ResponseInProgressQset.Count > 20)
                {
                    result = "More than 20 Questions are not responded, please refer the data gathering screen";
                    //result = string.Join(",", isallResponsed.Take(5).Select(t => t.SQNo))+",......";
                }
                else
                {
                    var e = ""; //= string.Join(",", ResponseInProgressQset);
                    foreach (var item in ResponseInProgressQset)
                    {
                        e = e + item.SQNo + ",";
                    }
                    result = "Please enter the Followup Response to the following Question(s) to publish the assessment : " + e;

                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ActionTrack]
        public ActionResult ReviewAssessment(string AssessmentID, string Action)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var Client = (ClientDetail)Session["SessionClient"];
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var uid = (int)Session["UserId"];
                int assessID = Convert.ToInt32(AssessmentID);
                var assessment = unitOfWork.Assessment.GetByID(assessID);
                assessment.Status = Action;
                if (Action == "Published")
                {
                    assessment.EndDate = DateTime.Now;
                    var success = AssessmentManagerHelper.addIMIntegration(Client.ClientId, uid, assessID, assessment);
                }
                unitOfWork.Assessment.Update(assessment);
                unitOfWork.Save();

                var assResponse = unitOfWork.QuestionnaireResponse.Get(a => a.AssessmentID == assessID).OrderBy(a => a.Id).FirstOrDefault();
                //  var questCount = unitOfWork.QuestionnaireResponse.Get(a => a.AssessmentID == assessID && (a.Response == null || a.Response == "")).Count();
                var sqnolist = unitOfWork.A3MM_MultipleResponses.Get(x => x.AssessmentId == assessID).Select(x => x.SQNo).Distinct().ToList();
                var Sourcesqnolist = unitOfWork.QuestionnaireResponse.Get(x => x.AssessmentID == assessID).Select(x => x.SQNo).Distinct().ToList();

                var questCount = Sourcesqnolist.Where(i => !sqnolist.Contains(i)).Count();
                // var responseLink = Url.Action("DataGathering", "Assessment", new { area = "AssessmentManager" }, "https");
                var responseLink = Url.Action("RedirectToGathering", "ResponseGathering", new { area = "AssessmentManager" }, "https");

                responseLink = responseLink + "?AssessmentIDparam=" + assessID;

                var poc = unitOfWork.UserProfile.GetByID(assessment.EngagementTeamPointOfContact);
                var username = unitOfWork.UserProfile.GetByID(uid);
                var commUser = unitOfWork.UserProfile.GetByID(assessment.EngagementTeamtoCommunicate);
                string[] ccuser = { commUser.Email, "A3MMAssessmentTeam@nttdata.com" };
                string[] eu_ccuser = { commUser.Email, poc.Email };
                var AssessmentType = "";
                if (assResponse.SurveyTypeId == 1)
                    AssessmentType = "CES-IVV";
                else if (assResponse.SurveyTypeId == 2)
                    AssessmentType = "CES-A3MM";
                else
                    AssessmentType = "CES-DA";
                if (Action == "Engagement Published")
                {
                    AssessmentManagerHelper.SendReviewEmailtoAssessmentTeam("A3MMAssessmentTeam@nttdata.com", eu_ccuser, username, AssessmentType, assessment.AssessmentName, responseLink, questCount, Action);
                }
                else
                {
                    AssessmentManagerHelper.SendReviewEmail(poc, ccuser, username, AssessmentType, assessment.AssessmentName, responseLink, questCount, Action);

                }
                return Json(assessment, JsonRequestBehavior.AllowGet);
            }
        }


        //HeatMap Dashboard
        [ActionTrack]
        public ActionResult HeatMap()
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();

            List<int> disciplinelist = (from a in db.Discipline
                                        orderby a.Id descending
                                        select a.Id
                                  ).Distinct().ToList();

            ViewBag.disciplinelist = disciplinelist;


            List<int> componentList = (from a in db.Component
                                       orderby a.Id descending
                                       select a.Id).Distinct().ToList();

            ViewBag.componentList = componentList;
            EngagementQuestionnaireModel pr = prepareCommonParameters1();
            //IList<QuestionnaireResponse> qrList = new List<QuestionnaireResponse>();

            var AssessmentInitial = (from a in db.Assessment
                                     where a.ClientId == Cl.ClientId
                                     select a).FirstOrDefault();

            List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                       where a.ClientId == Cl.ClientId
                                                       select new SelectListItem
                                                       {
                                                           Text = a.AssessmentName,
                                                           Value = a.Id.ToString()
                                                       }).ToList();


            //qrList = (from a in db.QuestionnaireResponse
            //          where a.AssessmentID == AssessmentInitial.Id && a.SurveyTypeId == AssessmentInitial.SurveyTypeID && a.ClientID == Cl.ClientId && a.MaturityLevel != "NA"
            //          select a).ToList();

            //IList<EngagementQuestionnaire> qList = (from a in db.EngagementQuestionnaire
            //                                        where a.SurveyTypeID == AssessmentInitial.SurveyTypeID && a.VersionID == AssessmentInitial.QuestionaireVersion
            //                                        select a).ToList(); // add version ID paramter in where





            //foreach (var a in qrList)
            //{
            //    //EngagementQuestionnaire e = (from b in db.EngagementQuestionnaire
            //    //                             where a.QNo == b.QNo && a.SurveyTypeId == b.SurveyTypeID && a.VersionID == AssessmentInitial.QuestionaireVersion
            //    //                             select b).FirstOrDefault();

            //    EngagementQuestionnaire e = (from b in db.EngagementQuestionnaire
            //                                 where a.SQNo == b.QNo.ToString() && a.SurveyTypeId == b.SurveyTypeID && a.VersionID == AssessmentInitial.QuestionaireVersion
            //                                 select b).FirstOrDefault();


            //    if (e != null)
            //    {
            //        EngagementQuestionnaireModel em = new EngagementQuestionnaireModel();
            //        em.Discipline_ID = e.Discipline_ID;
            //        em.DisciplineName = e.DisciplineName;
            //        em.Component_ID = e.Component_ID;
            //        em.ComponentName = e.ComponentName;
            //        em.AspectID = e.AspectID;
            //        em.MaturityLevel = a.MaturityLevel;
            //        em.AssessmentID = 1;
            //        em.QNo = e.QNo; //deva-need to change
            //        retList.Add(em);
            //    }

            //}
            List<EngagementQuestionnaireModel> retList = FetchHeatMapData(Cl.ClientId, AssessmentInitial.Id);

            retList[0].AssessmentNameList = AssessmentNameList;
            retList[0].AccountID = AssessmentInitial.AccountID;
            retList[0].ContractID = AssessmentInitial.AccountID;
            retList[0].AccountNamesList = pr.AccountNamesList;
            List<SelectListItem> ContractNameList = (from c in db.ClientAccountContract
                                                     where c.AccountId == AssessmentInitial.AccountID && c.IsDeleted == false
                                                     select new SelectListItem
                                                     {
                                                         Text = c.ContractName,
                                                         Value = c.Id.ToString()
                                                     }).ToList();

            retList[0].ContractList = ContractNameList;
            retList[0].DisciplineList = pr.DisciplineList;
            retList[0].ComponentList = pr.ComponentList;





            return View(retList);

        }


        [HttpPost]
        public ActionResult getHeatMapFilterData(string AssessmentID, string AccountID/*, string ContractID*/)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var Cl = (ClientDetail)Session["SessionClient"];
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                int assessID = Convert.ToInt32(AssessmentID);
                int accID = Convert.ToInt32(AccountID);
                // int conID = Convert.ToInt32(ContractID);


                Db db = new Db();


                //New

                List<string> disciplinelist1 = (from a in db.Discipline
                                                orderby a.Id descending
                                                select a.DisciplineName
                                  ).Distinct().ToList();

                ViewBag.disciplinelist = disciplinelist1;


                var componentList1 = (from a in db.Component
                                      orderby a.Id descending
                                      select a.ComponentName).Distinct().ToList();

                ViewBag.componentList = componentList1;
                EngagementQuestionnaireModel pr = prepareCommonParameters1();
                IList<QuestionnaireResponse> qrList = new List<QuestionnaireResponse>();



                List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                           where a.Id == assessID && a.ClientId == Cl.ClientId
                                                           select new SelectListItem
                                                           {
                                                               Text = a.AssessmentName,
                                                               Value = a.Id.ToString()
                                                           }).ToList();




                var assessment = (from a in db.Assessment
                                  where a.Id == assessID
                                  select a).FirstOrDefault();



                List<EngagementQuestionnaireModel> retList = FetchHeatMapData(Cl.ClientId, assessment.Id);

                retList[0].AssessmentNameList = AssessmentNameList;
                retList[0].AccountID = assessment.AccountID;
                retList[0].ContractID = assessment.ContractID;
                retList[0].AccountNamesList = pr.AccountNamesList;
                List<SelectListItem> ContractNameList = (from c in db.ClientAccountContract
                                                         where c.AccountId == assessment.AccountID && c.IsDeleted == false
                                                         select new SelectListItem
                                                         {
                                                             Text = c.ContractName,
                                                             Value = c.Id.ToString()
                                                         }).ToList();

                retList[0].ContractList = ContractNameList;
                retList[0].DisciplineList = pr.DisciplineList;
                retList[0].ComponentList = pr.ComponentList;


                //return Json(modelList, JsonRequestBehavior.AllowGet);
                return PartialView("_HeatMapTablePartial", retList);
            }
        }




        [HttpPost]
        public ActionResult getHeatMapComparativeFilterData(string FirstAssessmentID, string SecondAssessmentID, string ThirdAssessmentID, string FourthAssessmentID)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var Cl = (ClientDetail)Session["SessionClient"];
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                int firstAssessID = Convert.ToInt32(FirstAssessmentID);
                int SecondAssessID = Convert.ToInt32(SecondAssessmentID);
                int ThirdAssessID = Convert.ToInt32(ThirdAssessmentID);
                int FourthAssessID = Convert.ToInt32(FourthAssessmentID);

                Db db = new Db();

                //New

                List<string> disciplinelist1 = (from a in db.Discipline
                                                orderby a.Id descending
                                                select a.DisciplineName
                                  ).Distinct().ToList();

                ViewBag.disciplinelist = disciplinelist1;


                var componentList1 = (from a in db.Component
                                      orderby a.Id descending
                                      select a.ComponentName).Distinct().ToList();

                ViewBag.componentList = componentList1;
                EngagementQuestionnaireModel pr = prepareCommonParameters1();
                IList<QuestionnaireResponse> qrList = new List<QuestionnaireResponse>();



                List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                           where a.Id == firstAssessID && a.ClientId == Cl.ClientId
                                                           select new SelectListItem
                                                           {
                                                               Text = a.AssessmentName,
                                                               Value = a.Id.ToString()
                                                           }).ToList();


                List<SelectListItem> SecondAssessmentNameList = (from a in db.Assessment
                                                                 where a.Id != firstAssessID && a.ClientId == Cl.ClientId
                                                                 select new SelectListItem
                                                                 {
                                                                     Text = a.AssessmentName,
                                                                     Value = a.Id.ToString()
                                                                 }).ToList();


                List<SelectListItem> ThirdAssessmentNameList = (from a in db.Assessment
                                                                 where a.Id != firstAssessID && a.Id != SecondAssessID && a.ClientId == Cl.ClientId 
                                                                select new SelectListItem
                                                                {
                                                                    Text = a.AssessmentName,
                                                                    Value = a.Id.ToString()
                                                                }).ToList();

                List<SelectListItem> FourthAssessmentNameList = (from a in db.Assessment
                                                                 where a.Id != firstAssessID && a.Id != SecondAssessID && a.Id != ThirdAssessID && a.ClientId == Cl.ClientId
                                                                 select new SelectListItem
                                                                 {
                                                                     Text = a.AssessmentName,
                                                                     Value = a.Id.ToString()
                                                                 }).ToList();

                var newItem = new SelectListItem
                {
                    Text = "No Selection", // Display text
                    Value = "0" // Value for the option
                };

                // Insert the new item at the top of the list
                ThirdAssessmentNameList.Insert(0, newItem);
                FourthAssessmentNameList.Insert(0, newItem);

                var firstAssessment = (from a in db.Assessment
                                       where a.Id == firstAssessID
                                       select a).FirstOrDefault();

                var secondAssessment = (from a in db.Assessment
                                        where a.Id == SecondAssessID
                                        select a).FirstOrDefault();

                var thirdAssessment = new Assessment();

                if (ThirdAssessID != 0)
                {
                    thirdAssessment = (from a in db.Assessment
                                       where a.Id == ThirdAssessID
                                       select a).FirstOrDefault();
                }

                var fourthAssessment = new Assessment();

                if (FourthAssessID != 0)
                {
                    fourthAssessment = (from a in db.Assessment
                                        where a.Id == FourthAssessID
                                        select a).FirstOrDefault();
                }

                List<EngagementQuestionnaireModel> retList = FetchHeatMapData(Cl.ClientId, firstAssessment.Id);
                List<EngagementQuestionnaireModel> retList2 = FetchHeatMapData(Cl.ClientId, secondAssessment.Id);

                List<EngagementQuestionnaireModel> retList3 = new List<EngagementQuestionnaireModel>();
                if (ThirdAssessID != 0)
                {
                    retList3 = FetchHeatMapData(Cl.ClientId, thirdAssessment.Id);
                }

                List<EngagementQuestionnaireModel> retList4 = new List<EngagementQuestionnaireModel>();
                if (FourthAssessID != 0)
                {
                    retList4 = FetchHeatMapData(Cl.ClientId, fourthAssessment.Id);
                }


                if (retList.Count == 0)
                {
                    EngagementQuestionnaireModel et = new EngagementQuestionnaireModel();
                    retList.Add(et);
                }


                retList[0].secondAssessmentDataList = retList2;
                if (ThirdAssessID != 0)
                {
                    retList[0].Count = 3;
                    retList[0].thirdAssessmentDataList = retList3;
                }

                if (FourthAssessID != 0)
                {
                    retList[0].Count = 4;
                    retList[0].fourthAssessmentDataList = retList4;
                }

                retList[0].AssessmentNameList = AssessmentNameList;
                //retList[0].ThirdAssessmentList = ThirdAssessmentNameList;
                retList[0].AccountID = firstAssessment.AccountID;
                retList[0].ContractID = firstAssessment.ContractID;
                retList[0].AccountNamesList = pr.AccountNamesList;
                List<SelectListItem> ContractNameList = (from c in db.ClientAccountContract
                                                         where c.AccountId == firstAssessment.AccountID && c.IsDeleted == false
                                                         select new SelectListItem
                                                         {
                                                             Text = c.ContractName,
                                                             Value = c.Id.ToString()
                                                         }).ToList();

                retList[0].ContractList = ContractNameList;
                retList[0].ContractList = ContractNameList;
                retList[0].DisciplineList = pr.DisciplineList;
                retList[0].ComponentList = pr.ComponentList;


                //return Json(modelList, JsonRequestBehavior.AllowGet);
                return PartialView("_HeatMapComparativePartial", retList);
            }
        }

        // HeatMapComparative Dashboard
        [ActionTrack]
        public ActionResult HeatMapComparative()
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();

            List<int> disciplinelist = (from a in db.Discipline
                                        orderby a.Id descending
                                        select a.Id
                                  ).Distinct().ToList();

            ViewBag.disciplinelist = disciplinelist;


            List<int> componentList = (from a in db.Component
                                       orderby a.Id descending
                                       select a.Id).Distinct().ToList();

            ViewBag.componentList = componentList;


            EngagementQuestionnaireModel pr = prepareCommonParameters1();
            //IList<QuestionnaireResponse> qrList = new List<QuestionnaireResponse>();







            var AssessmentInitial = (from a in db.Assessment
                                     where a.ClientId == Cl.ClientId
                                     select a).FirstOrDefault();

            var AssessmentSecond = new Assessment();

            if (AssessmentInitial != null)
            {


                AssessmentSecond = (from a in db.Assessment
                                    where a.ClientId == Cl.ClientId && a.Id != AssessmentInitial.Id
                                    select a).FirstOrDefault();
            }


            var AssessmentThird = new Assessment();

            if (AssessmentSecond != null)
            {

                AssessmentThird = (from a in db.Assessment
                                   where a.ClientId == Cl.ClientId && a.Id != AssessmentInitial.Id && a.Id != AssessmentSecond.Id
                                   select a).FirstOrDefault();
            }





            List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                       where a.ClientId == Cl.ClientId
                                                       select new SelectListItem
                                                       {
                                                           Text = a.AssessmentName,
                                                           Value = a.Id.ToString()
                                                       }).ToList();

            List<SelectListItem> SecondAssessmentList = new List<SelectListItem>();

            if (AssessmentInitial != null)
            {
                SecondAssessmentList = (from a in db.Assessment
                                        where a.ClientId == Cl.ClientId && a.Id != AssessmentInitial.Id
                                        select new SelectListItem
                                        {
                                            Text = a.AssessmentName,
                                            Value = a.Id.ToString()
                                        }).ToList();
            }


            //var newItem = new SelectListItem
            //{
            //    Text = "No Selection", // Display text
            //    Value = "0" // Value for the option
            //};

            //SecondAssessmentList.Insert(0, newItem);

            List<SelectListItem> thirdAssessmentList = new List<SelectListItem>();
            List<SelectListItem> fourthAssessmentList = new List<SelectListItem>();


            if (AssessmentSecond != null)
            {
                thirdAssessmentList = (from a in db.Assessment
                                       where a.ClientId == Cl.ClientId && a.Id != AssessmentInitial.Id && a.Id != AssessmentSecond.Id
                                       select new SelectListItem
                                       {
                                           Text = a.AssessmentName,
                                           Value = a.Id.ToString()
                                       }).ToList();
            }
            var newItem2 = new SelectListItem
            {
                Text = "No Selection", // Display text
                Value = "0" // Value for the option
            };

            thirdAssessmentList.Insert(0, newItem2);


            if (AssessmentThird.Id != 0)
            {
                fourthAssessmentList = (from a in db.Assessment
                                        where a.ClientId == Cl.ClientId && a.Id != AssessmentInitial.Id && a.Id != AssessmentSecond.Id && a.Id != AssessmentThird.Id
                                        select new SelectListItem
                                        {
                                            Text = a.AssessmentName,
                                            Value = a.Id.ToString()
                                        }).ToList();
            }



            var newItem1 = new SelectListItem
            {
                Text = "No Selection", // Display text
                Value = "0" // Value for the option
            };

            fourthAssessmentList.Insert(0, newItem1);

            List<EngagementQuestionnaireModel> retList = FetchHeatMapData(Cl.ClientId, AssessmentInitial.Id);
            List<EngagementQuestionnaireModel> retList2 = new List<EngagementQuestionnaireModel>();
            if (AssessmentSecond != null)
            {
                retList2 = FetchHeatMapData(Cl.ClientId, AssessmentSecond.Id);
            }

            retList[0].secondAssessmentDataList = retList2;
            retList[0].AssessmentNameList = AssessmentNameList;
            retList[0].SecondAssessmentList = SecondAssessmentList;
            retList[0].ThirdAssessmentList = thirdAssessmentList;
            retList[0].FourthAssessmentList = fourthAssessmentList;
            retList[0].AccountID = AssessmentInitial.AccountID;
            retList[0].ContractID = AssessmentInitial.AccountID;
            retList[0].AccountNamesList = pr.AccountNamesList;


            List<SelectListItem> ContractNameList = (from c in db.ClientAccountContract
                                                     where c.AccountId == AssessmentInitial.AccountID && c.IsDeleted == false
                                                     select new SelectListItem
                                                     {
                                                         Text = c.ContractName,
                                                         Value = c.Id.ToString()
                                                     }).ToList();

            retList[0].ContractList = ContractNameList;
            retList[0].DisciplineList = pr.DisciplineList;
            retList[0].ComponentList = pr.ComponentList;





            return View(retList);

        }

        public ActionResult getSecondAssessmentList(string AssessmentName/*, string ContractID*/)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            int AssessmentID = (from a in db.Assessment
                                where a.AssessmentName == AssessmentName
                                select a.Id).FirstOrDefault();
            //int ContractIDint = Convert.ToInt32(ContractID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());


            List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                       where a.Id != AssessmentID && a.ClientId == Cl.ClientId
                                                       select new SelectListItem
                                                       {
                                                           Text = a.AssessmentName,
                                                           Value = a.Id.ToString()
                                                       }).ToList();

            var newItem = new SelectListItem
            {
                Text = "No Selection", // Display text
                Value = "0" // Value for the option
            };

            AssessmentNameList.Insert(0, newItem);


            return Json(AssessmentNameList, JsonRequestBehavior.AllowGet);

        }




        public ActionResult getThirdAssessmentList(string AssessmentName1, string AssessmentName2)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            int AssessmentID1 = (from a in db.Assessment
                                 where a.AssessmentName == AssessmentName1
                                 select a.Id).FirstOrDefault();

            int AssessmentID2 = (from a in db.Assessment
                                 where a.AssessmentName == AssessmentName2
                                 select a.Id).FirstOrDefault();

            //int ContractIDint = Convert.ToInt32(ContractID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());


            List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                       where a.Id != AssessmentID1 && a.Id != AssessmentID2 && a.ClientId == Cl.ClientId
                                                       select new SelectListItem
                                                       {
                                                           Text = a.AssessmentName,
                                                           Value = a.Id.ToString()
                                                       }).ToList();


            var newItem = new SelectListItem
            {
                Text = "No Selection", // Display text
                Value = "0" // Value for the option
            };

            AssessmentNameList.Insert(0, newItem);

            return Json(AssessmentNameList, JsonRequestBehavior.AllowGet);

        }


        public ActionResult getFourthAssessmentList(string AssessmentName1, string AssessmentName2, string AssessmentName3)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            int AssessmentID1 = (from a in db.Assessment
                                 where a.AssessmentName == AssessmentName1
                                 select a.Id).FirstOrDefault();

            int AssessmentID2 = (from a in db.Assessment
                                 where a.AssessmentName == AssessmentName2
                                 select a.Id).FirstOrDefault();

            int AssessmentID3 = (from a in db.Assessment
                                 where a.AssessmentName == AssessmentName3
                                 select a.Id).FirstOrDefault();

            //int ContractIDint = Convert.ToInt32(ContractID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());


            var count = (from a in db.Assessment
                         where a.ClientId == Cl.ClientId
                         select a).Count();


            List<SelectListItem> AssessmentNameList = new List<SelectListItem>();

            if (count > 3)
            {
                AssessmentNameList = (from a in db.Assessment
                                      where a.Id != AssessmentID1 && a.Id != AssessmentID2 && a.Id != AssessmentID3 && a.ClientId == Cl.ClientId
                                      select new SelectListItem
                                      {
                                          Text = a.AssessmentName,
                                          Value = a.Id.ToString()
                                      }).ToList();
            }



            var newItem = new SelectListItem
            {
                Text = "No Selection", // Display text
                Value = "0" // Value for the option
            };

            AssessmentNameList.Insert(0, newItem);


            return Json(AssessmentNameList, JsonRequestBehavior.AllowGet);

        }

        [ActionTrack]
        public ActionResult A3MMDashboard()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;
            A3MMDigiJrny mod = new A3MMDigiJrny();
            Db db = new Db(); List<SelectListItem> ls = new List<SelectListItem>();
            var AssessmentList = (from a in db.Assessment
                                  where a.ClientId == client.ClientId
                                  select a).Distinct().ToList();
            foreach (var item in AssessmentList)
            {
                ls.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
            }
            var assessmentName = "";
            if (ls.Count != 0)
            {
                assessmentName = ls[0].Text;
            }
            var assid = (from c in db.Assessment
                         where c.AssessmentName == assessmentName && c.ClientId == client.ClientId
                         select c.Id).FirstOrDefault();
            mod = FetchEngageLevel(cid, assessmentName, assid);
            mod.AssessmentList = ls;
            return View(mod);
        }

        public static A3MMDigiJrny FetchEngageLevel(int clientid, string AssessmentName, int AssessmentId)
        {
            A3MMDigiJrny mod1 = new A3MMDigiJrny();
            mod1.Account = "No Data";
            mod1.Contract = "No Data";
            mod1.ContractDuration = "No Data";
            Db db = new Db();
            var acctCont = (from x in db.Assessment where x.AssessmentName == AssessmentName && x.ClientId == clientid select new { acctid = x.AccountID, contId = x.ContractID }).FirstOrDefault();
            if (acctCont != null)
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    List<A3MMEngagementLevels> listlevel = new List<A3MMEngagementLevels>();
                    var paramName = new string[] { "ClientId", "AssessmentID" };
                    var paramValue = new object[] { clientid, AssessmentId };
                    var ds = unitOfWork.GetDataFromStoredProcedure("FetchEngagementLevel", paramName, paramValue);
                    var ds2 = unitOfWork.GetDataFromStoredProcedure("FetchNAEngagementLevel", paramName, paramValue);
                    foreach (DataRow dr in ds.Rows)
                    {
                        var enggLvl = new A3MMEngagementLevels();
                        enggLvl.EnggComponent = dr["ComponentName"].ToString();
                        if (!(dr["MaturityLevel"] is DBNull))
                            enggLvl.level = dr["MaturityLevel"].ToString();
                        else enggLvl.level = "0";
                        enggLvl.ComponentAbbrevation = dr["ComponentAbbrevation"].ToString();
                        listlevel.Add(enggLvl);
                    }
                    mod1.EngagementList = listlevel;

                    List<A3MMEngagementLevels> NAEngLevel = new List<A3MMEngagementLevels>();
                    foreach (DataRow dr in ds2.Rows)
                    {
                        var enggNALvl = new A3MMEngagementLevels();
                        enggNALvl.EnggComponent = dr["ComponentName"].ToString();
                        if (!(dr["MaturityLevel"] is DBNull))
                            enggNALvl.level = dr["MaturityLevel"].ToString();
                        else enggNALvl.level = "0";
                        enggNALvl.ComponentAbbrevation = dr["ComponentAbbrevation"].ToString();
                        NAEngLevel.Add(enggNALvl);
                    }
                    var englevel = NAEngLevel.GroupBy(x => x.ComponentAbbrevation);
                    List<string> NALevel = new List<string>();
                    foreach (var item in englevel)
                    {
                        if (item.All(a => a.level == "NA"))
                        {
                            NALevel.Add(item.Key);
                        }
                    }
                    mod1.NAEngagementList = NALevel;

                    mod1.Account = unitOfWork.ClientAccounSetup.GetByID(acctCont.acctid).AccountName;
                    mod1.Contract = unitOfWork.ClientAccountContract.GetByID(acctCont.contId).ContractName;
                    var ContractStDt = unitOfWork.ClientAccountContract.GetByID(acctCont.contId).ContractOpenDate;
                    var ContractEndDt = unitOfWork.ClientAccountContract.GetByID(acctCont.contId).ContractCloseDate;
                    mod1.ContractDuration = ContractStDt.ToString("MMMM") + " " + ContractStDt.ToString("yyyy") + " - " + ContractEndDt.ToString("MMMM") + " " + ContractEndDt.ToString("yyyy");
                    mod1.AssessmentName = AssessmentName;
                }
            return mod1;
        }


        public ActionResult DigitalJourneyComparison()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;
            A3MMDigiJrny mod = new A3MMDigiJrny();
            Db db = new Db();
            List<SelectListItem> ls = new List<SelectListItem>();
            List<SelectListItem> ls2 = new List<SelectListItem>();
            List<SelectListItem> ls3 = new List<SelectListItem>();
            List<SelectListItem> ls4 = new List<SelectListItem>();

            var newItem2 = new SelectListItem
            {
                Text = "No Selection", // Display text
                Value = "0" // Value for the option
            };
            ls3.Insert(0, newItem2);
            ls4.Insert(0, newItem2);
            var AssessmentInitial = (from a in db.Assessment
                                     where a.ClientId == cid
                                     select a).FirstOrDefault();

            var AssessmentSecond = (from a in db.Assessment
                                    where a.ClientId == cid && a.Id != AssessmentInitial.Id
                                    select a).FirstOrDefault();
            var AssessmentThird = (from a in db.Assessment
                                   where a.ClientId == cid && a.Id != AssessmentInitial.Id
                                   && a.Id != AssessmentSecond.Id
                                   select a).FirstOrDefault();
            var AssessmentFourth = (from a in db.Assessment
                                    where a.ClientId == cid && a.Id != AssessmentInitial.Id
                                     && a.Id != AssessmentSecond.Id && a.Id != AssessmentThird.Id
                                    select a).FirstOrDefault();

            var AssessmentList = (from a in db.Assessment
                                  where a.ClientId == client.ClientId
                                  select a).Distinct().ToList();

            var AssessmentList2 = (from a in db.Assessment
                                   where a.ClientId == cid && a.Id != AssessmentInitial.Id
                                   select a).Distinct().ToList();

            foreach (var item in AssessmentList)
            {
                ls.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
            }

            foreach (var item in AssessmentList2)
            {
                ls2.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
            }
            mod = FetchEngageLevel(cid, AssessmentInitial.AssessmentName, AssessmentInitial.Id);
            mod.secondAssessmentDataList = FetchEngageLevel(cid, AssessmentSecond.AssessmentName, AssessmentSecond.Id);
            mod.AssessmentList = ls;
            mod.secondAssessmentDataList.AssessmentList = ls2;


            mod.AssessmentName = AssessmentInitial.AssessmentName;
            mod.secondAssessmentDataList.AssessmentName = AssessmentSecond.AssessmentName;

            var AssessmentList3 = (from a in db.Assessment
                                   where a.ClientId == client.ClientId && a.Id != AssessmentInitial.Id
                                     && a.Id != AssessmentSecond.Id
                                   select a).Distinct().ToList();

            if (AssessmentList3.Count != 0)
            {
                foreach (var item in AssessmentList3)
                {
                    ls3.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
                }
                mod.ThirdAssessmentDataList = FetchEngageLevel(cid, AssessmentThird.AssessmentName, AssessmentThird.Id);
                mod.ThirdAssessmentDataList.AssessmentList = ls3;
                mod.ThirdAssessmentDataList.AssessmentName = AssessmentThird.AssessmentName;

                var AssessmentList4 = (from a in db.Assessment
                                       where a.ClientId == cid && a.Id != AssessmentInitial.Id
                                        && a.Id != AssessmentSecond.Id && a.Id != AssessmentThird.Id
                                       select a).Distinct().ToList();

                if (AssessmentList4.Count != 0)
                {
                    foreach (var item in AssessmentList4)
                    {
                        ls4.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
                    }
                    mod.FourthAssessmentDataList = FetchEngageLevel(cid, AssessmentFourth.AssessmentName, AssessmentFourth.Id);

                    mod.FourthAssessmentDataList.AssessmentList = ls4;
                    mod.FourthAssessmentDataList.AssessmentName = AssessmentFourth.AssessmentName;
                }
            }
            mod.ComponentList = (from a in db.Component
                                 select new SelectListItem
                                 {
                                     Text = a.ComponentName,
                                     Value = a.ComponentAbbrevation
                                 }).ToList();
            return View(mod);
        }

        public static List<EngagementQuestionnaireModel> FetchHeatMapData(int clientid, int AssessmentId)
        {
            List<EngagementQuestionnaireModel> listlevel = new List<EngagementQuestionnaireModel>();

            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramName = new string[] { "ClientId", "AssessmentID" };
                var paramValue = new object[] { clientid, AssessmentId };
                var ds = unitOfWork.GetDataFromStoredProcedure("A3MM_FetchHeatMapData", paramName, paramValue);
                foreach (DataRow dr in ds.Rows)
                {
                    var heatmapDt = new EngagementQuestionnaireModel();
                    heatmapDt.ComponentName = dr["ComponentName"].ToString();
                    heatmapDt.DisciplineName = dr["DisciplineName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                        heatmapDt.MaturityLevelAVG = Math.Round(Convert.ToDecimal(dr["MaturityLevel"]), 2);

                    listlevel.Add(heatmapDt);
                }
            }
            return listlevel;
        }

        public ActionResult GetEngageLevelByAssName(string AssessmentName)
        {
            try
            {
                var client = (ClientDetail)Session["SessionClient"];
                int cid = client.ClientId;
                A3MMDigiJrny mod = new A3MMDigiJrny();
                Db db = new Db();
                List<SelectListItem> ls = new List<SelectListItem>();

                var AssessmentList = (from a in db.Assessment
                                      select a).Distinct().ToList();
                foreach (var item in AssessmentList)
                {
                    ls.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
                }

                var assessmentName = AssessmentName;

                var assid = (from c in db.Assessment
                             where c.AssessmentName == assessmentName
                             select c.Id).FirstOrDefault();
                mod = FetchEngageLevel(cid, assessmentName, assid);
                mod.AssessmentList = ls;

                return PartialView("_DigitalJourney", mod);
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEngageLevelByAssName ", ex);
                throw ex;
            }
        }



        public ActionResult GetAssessmentNameListforDashboard(string AccountID/*, string ContractID*/)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int AccountIDint = Convert.ToInt32(AccountID);
            //int ContractIDint = Convert.ToInt32(ContractID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();

            List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                       where a.AccountID == AccountIDint /*&& a.ContractID == ContractIDint*/
                                                       select new SelectListItem
                                                       {
                                                           Text = a.AssessmentName,
                                                           Value = a.Id.ToString()
                                                       }).ToList();




            return Json(AssessmentNameList, JsonRequestBehavior.AllowGet);

        }



        public ActionResult GetEngageLevelByAssNameForComparison(string AssessmentName, string AssessmentName2,
                                                                 string AssessmentName3, string AssessmentName4)
        {
            try
            {
                var client = (ClientDetail)Session["SessionClient"];
                int cid = client.ClientId;
                A3MMDigiJrny mod = new A3MMDigiJrny();
                Db db = new Db();
                List<SelectListItem> ls = new List<SelectListItem>();
                List<SelectListItem> ls2 = new List<SelectListItem>();
                List<SelectListItem> ls3 = new List<SelectListItem>();
                List<SelectListItem> ls4 = new List<SelectListItem>();

                var firstAssessment = (from a in db.Assessment
                                       where a.ClientId == cid && a.AssessmentName == AssessmentName
                                       select a).FirstOrDefault();

                var firstAssessmentList = (from a in db.Assessment
                                           where a.ClientId == cid
                                           select a).Distinct().ToList();

                var secondAssessment = (from a in db.Assessment
                                        where a.ClientId == cid && a.AssessmentName == AssessmentName2
                                        select a).FirstOrDefault();

                var secondAssessmentList = (from a in db.Assessment
                                            where a.ClientId == cid && a.Id != firstAssessment.Id
                                            select a).Distinct().ToList();
                foreach (var item in firstAssessmentList)
                {
                    ls.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
                }
                foreach (var item in secondAssessmentList)
                {
                    ls2.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
                }
                mod = FetchEngageLevel(cid, firstAssessment.AssessmentName, firstAssessment.Id);
                mod.secondAssessmentDataList = FetchEngageLevel(cid, secondAssessment.AssessmentName, secondAssessment.Id);

                mod.AssessmentList = ls;
                mod.secondAssessmentDataList.AssessmentList = ls2;
                if (AssessmentName3.Trim() != "No Selection")
                {
                    var ThirdAssessment = (from a in db.Assessment
                                           where a.ClientId == cid && a.AssessmentName == AssessmentName3
                                           select a).FirstOrDefault();
                    var ThirdAssessmentList = (from a in db.Assessment
                                               where a.ClientId == cid && a.Id != firstAssessment.Id
                                               && a.Id != secondAssessment.Id
                                               select a).Distinct().ToList();
                    foreach (var item in ThirdAssessmentList)
                    {
                        ls3.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
                    }
                    mod.ThirdAssessmentDataList = FetchEngageLevel(cid, ThirdAssessment.AssessmentName, ThirdAssessment.Id);
                    mod.ThirdAssessmentDataList.AssessmentList = ls3;

                    if (AssessmentName4.Trim() != "No Selection")
                    {
                        var FourthAssessment = (from a in db.Assessment
                                                where a.ClientId == cid && a.AssessmentName == AssessmentName4
                                                select a).FirstOrDefault();

                        var FourthAssessmentList = (from a in db.Assessment
                                                    where a.ClientId == cid && a.Id != firstAssessment.Id
                                                     && a.Id != secondAssessment.Id && a.Id != ThirdAssessment.Id
                                                    select a).Distinct().ToList();
                        foreach (var item in FourthAssessmentList)
                        {
                            ls4.Add(new SelectListItem() { Text = item.AssessmentName, Value = item.AssessmentName });
                        }
                        mod.FourthAssessmentDataList = FetchEngageLevel(cid, FourthAssessment.AssessmentName, FourthAssessment.Id);

                        mod.FourthAssessmentDataList.AssessmentList = ls4;
                    }

                }
                mod.ComponentList = (from a in db.Component
                                     select new SelectListItem
                                     {
                                         Text = a.ComponentName,
                                         Value = a.ComponentAbbrevation
                                     }).ToList();
                return PartialView("_DigitalJourneyComp", mod);
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEngageLevelByAssNameForComparison ", ex);
                throw ex;
            }
        }

        public ActionResult GetAssessmentNameListforDashboard2(string AccountID, string AssessmentTypeID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int AccountIDint = Convert.ToInt32(AccountID);
            int TypeID = Convert.ToInt32(AssessmentTypeID);
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();

            List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                       where a.AccountID == AccountIDint && a.SurveyTypeID == TypeID
                                                       select new SelectListItem
                                                       {
                                                           Text = a.AssessmentName,
                                                           Value = a.Id.ToString()
                                                       }).ToList();




            return Json(AssessmentNameList, JsonRequestBehavior.AllowGet);

        }
        public void SendLaunchNotification(string email, string[] ccemail, string POC, string username, string status, string AssessmentName, string respURL)
        {
            try
            {
                var body = "";
                var mailId = email;
                var Subject = "Optimization Tool A3MM AutoNotification";
                body = AssessmentLaunchBody(POC, username, status, AssessmentName, respURL);
                App_Start.Helper.SendEMailwithMultiCC(mailId, ccemail, Subject, body);
            }
            catch (Exception ex)
            {
                Log.Error("SendLaunch :" + ex);
            }
            //App_Start.Helper.SendEMailwithCC(mailId, ccemail, Subject, body);
        }
        private string AssessmentLaunchBody(string POC, string username, string status, string AssessmentName, string respURL)
        {
            try
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Areas/AssessmentManager/Templates/AssessmentUpdateNotification.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{Assessment_Name}", AssessmentName);
                body = body.Replace("{UserName}", username);
                body = body.Replace("{POC}", POC);
                body = body.Replace("{Status}", status);
                body = body.Replace("{responseURL}", respURL);

                return body;
            }
            catch (Exception ex)
            {
                Log.Error("AssessmentLaunchBody Error " + ex);
                return "";
            }
        }

        public void SendReviewNotification(string email, string[] ccemail, string POC, string username, string AssessmentType, string AssessmentName, string respURL, int questCount, string ActionStatus)
        {
            try
            {
                var body = "";
                var Subject = "";
                var mailId = email;

                if (ActionStatus == "Review")
                {
                    Subject = "Assessment Released For Review";
                }
                else if (ActionStatus == "Published")
                {
                    Subject = "Assessment Published by Assessment Team";
                }
                else
                {
                    Subject = "Assessment Published by Engagement Team";

                }
                body = AssessmentReviewBody(POC, username, AssessmentType, AssessmentName, respURL, questCount, ActionStatus);
                App_Start.Helper.SendEMailwithMultiCC(mailId, ccemail, Subject, body);
            }
            catch (Exception ex)
            {
                Log.Error("SendLaunch :" + ex);
            }
        }


        private string AssessmentReviewBody(string POC, string username, string AssessmentType, string AssessmentName, string respURL, int questCount, string ActionStatus)
        {
            string body = string.Empty;
            string statusText = "", statusdetailtext = "", regardtxt = "";

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Areas/AssessmentManager/Templates/AssessmentReviewNotification.html")))
            {
                body = reader.ReadToEnd();
            }
            if (ActionStatus == "Review")
            {
                regardtxt = "A3MM Assessment Team";
                statusText = "released for review";
                statusdetailtext = "review the assessment team clarifications/follow up questions and provide any additional information as applicable";
            }
            else if (ActionStatus == "Engagement Published")
            {
                regardtxt = username;
                statusText = "Published";
                statusdetailtext = "review and publish";
            }
            else
            {
                regardtxt = "A3MM Assessment Team";
                statusText = "Published";
                statusdetailtext = "access the assessment details";
            }
            body = body.Replace("{Assessment_Name}", AssessmentName);
            body = body.Replace("{UserName}", username);
            body = body.Replace("{POC}", POC);
            body = body.Replace("{AssessmentType}", AssessmentType);
            body = body.Replace("{Status_text}", statusText);
            body = body.Replace("{Status_detailtxt}", statusdetailtext);
            body = body.Replace("{regard_txt}", regardtxt);

            if (questCount > 1)
            {
                body = body.Replace("{questionCountStatement}", "There are " + questCount + " questions which are not responded.");
            }
            else if (questCount == 1)
            {
                body = body.Replace("{questionCountStatement}", "There is " + questCount + " question which is not responded.");
            }
            else
            {
                body = body.Replace("{questionCountStatement}", "");

            }
            body = body.Replace("{responseURL}", respURL);

            return body;
        }


        public void SendFinaliseNotification(string username, string VersionID, string SurveyType, string viewURL)
        {
            try
            {
                var body = "";
                var mailId = "A3MMAssessmentTeam@nttdata.com";
                var Subject = "Optimization Tool A3MM AutoNotification";
                body = AssessmentFinaliseBody(username, VersionID, SurveyType, viewURL);
                App_Start.Helper.SendEMail(mailId, Subject, body);
            }
            catch (Exception ex)
            {
                Log.Error("SendLaunch :" + ex);
            }
        }
        private string AssessmentFinaliseBody(string username, string VersionID, string SurveyType, string viewURL)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Areas/AssessmentManager/Templates/AssessmentFinaliseNotification.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{VersionID}", VersionID);
            body = body.Replace("{SurveyType}", SurveyType);
            body = body.Replace("{UserName}", username);
            body = body.Replace("{viewURL}", viewURL);

            return body;
        }

        //Question Set Inventory Screen
        [ActionTrack]
        public ActionResult QuestionSetInventory()
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            A3MMModel a3 = prepareCommonParameters();

            List<SelectListItem> newSurveyType = (from a in db.A3SurveyType
                                                  where a.ID == 3
                                                  select new SelectListItem
                                                  {
                                                      Value = a.ID.ToString(),
                                                      Text = a.SurveyName
                                                  }).ToList();


            a3.AssessmentList = newSurveyType;

            return View(a3);

        }


        public ActionResult ReadAllQuestionSet([DataSourceRequest] DataSourceRequest request)
        {
            var client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            Db db = new Db();
            A3MMModel a3 = prepareCommonParameters();
            IList<A3MMQuestionSet> qSetList = (from a in db.EngagementQuestionSet
                                               where a.isdeleted == 0
                                               select new A3MMQuestionSet
                                               {
                                                   ID = a.Id,
                                                   versionID = a.VersionID,
                                                   surveyTypeID = a.Assessment_ID,
                                                   status = a.Status,
                                                   clientID = 1,
                                                   LastUpdatedBy = a.LastUpdatedBy,
                                                   LastUpdatedDate = a.LastUpdatedDate,
                                                   isdeleted = a.isdeleted
                                               }).ToList();


            qSetList[0].AssessmentList = a3.AssessmentList;


            foreach (var a in qSetList)
            {
                a.surveyType = (from b in a3.AssessmentList
                                where Convert.ToInt32(b.Value) == a.surveyTypeID
                                select b.Text).FirstOrDefault();

                a.LastUpdatedByName = (from c in db.UserProfiles
                                       where c.Id == a.LastUpdatedBy
                                       select c.FirstName + " " + c.LastName).FirstOrDefault();
            }

            return Json(qSetList.ToDataSourceResult(request));
        }


        public ActionResult CheckForFinalizedVersion(string surveyTypeID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;

            Db db = new Db();

            var surveyTypeIDInt = Convert.ToInt32(surveyTypeID);

            var version = (from a in db.EngagementQuestionSet
                           where a.Assessment_ID == surveyTypeIDInt && a.Status == "Finalized" && a.isdeleted == 0
                           select a.VersionID).FirstOrDefault();

            var res = "";

            if (version != null)
            {
                res = "Finalized";
            }
            else
            {
                res = "not finalized";
            }

            var draftVersion = (from a in db.EngagementQuestionSet
                                where a.Assessment_ID == surveyTypeIDInt && a.Status == "draft" && a.isdeleted == 0
                                select a.VersionID).FirstOrDefault();

            if (draftVersion != null)
            {
                res = "Draft";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [ActionTrack]
        public ActionResult SaveNewQuestionnaire(string surveyTypeID, string VersionID, string Status)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;
            int usid = (int)Session["UserId"];

            Db db = new Db();

            var surveyTypeIDInt = Convert.ToInt32(surveyTypeID);




            var res = AssessmentManagerHelper.createNewQuestionnaire(surveyTypeIDInt, VersionID, Status, usid);




            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [ActionTrack]
        public ActionResult SaveandCloneNewQuestionnaire(string surveyTypeID, string VersionID, string Status)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int cid = client.ClientId;
            int usid = (int)Session["UserId"];
            Db db = new Db();

            var surveyTypeIDInt = Convert.ToInt32(surveyTypeID);

            var previousVersionID = (from a in db.EngagementQuestionSet
                                     where a.Assessment_ID == surveyTypeIDInt && a.Status == "Finalized" && a.isdeleted == 0
                                     select a.VersionID).FirstOrDefault();

            var res = AssessmentManagerHelper.createNewQuestionnaire(surveyTypeIDInt, VersionID, Status, usid);


            var questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDInt && a.VersionID == previousVersionID
                                && a.Status == "Finalized"
                                select a).ToList();



            var res2 = AssessmentManagerHelper.clonePreviousQuestionSet(questionList, surveyTypeIDInt, VersionID);


            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RedirectToBuildDisciplineEdit(string QSetID, string isView)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int QSetIDint = Convert.ToInt32(QSetID);



            var ret = "window.location='BuildDiscipline?QSetTableID=" + QSetIDint + "&isView=" + isView + "'";

            //return JavaScript("window.location = 'DataGathering?AssessmentIDparam=15&QNoparam=3'");
            return JavaScript(ret);


        }


        [HttpPost]
        [ActionTrack]
        public ActionResult Deletedraftversion(int ID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int QSetID = Convert.ToInt32(ID);
            Db db = new Db();

            var versionID = (from a in db.EngagementQuestionSet
                             where a.Id == QSetID && a.isdeleted == 0
                             select a.VersionID).FirstOrDefault();

            var surveyTypeID = (from a in db.EngagementQuestionSet
                                where a.Id == QSetID && a.isdeleted == 0
                                select a.Assessment_ID).FirstOrDefault();

            var res = AssessmentManagerHelper.DeletedraftQset(QSetID, uid);



            var surveyTypeID1 = Convert.ToInt32(surveyTypeID);

            var res1 = AssessmentManagerHelper.DeletedraftQuestions(versionID, surveyTypeID1);

            return Json(res, JsonRequestBehavior.AllowGet);


        }



        //Maturity Level Compare Graph
        [ActionTrack]
        public ActionResult A3MM_MaturityLevelCompare()
        {
            try
            {
                A3MMModel ret = prepareCommonParameters(); // fetching dropdown values
                A3MMModel initial = new A3MMModel();


                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

                ViewBag.userEmailList = userEmailList;

                ret.AssessmentNameListDropDown = ret.AssessmentNameList;

                //               ret.AssessmentNameList = AssessmentNameList;
                //               ret.SurveyTypeID = surveyTypeIDinitial;
                return View(ret);

            }
            catch (Exception ex)
            {
                Log.Error("A3MM_MaturityLevelCompare Index Action error 'A3MM_MaturityLevelCompare()' ", ex);
                throw ex;
            }
        }


        public ActionResult OnAccountChanged(int[] AccountDropDown)
        {
            int uid = (int)Session["UserId"];

            A3MMModel ret1 = new A3MMModel();
            Db db = new Db();
            ret1.AssessmentNameListDropDown = ret1.AssessmentNameList;

            if (AccountDropDown != null && AccountDropDown.Length > 0)
            {
                List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                           where AccountDropDown.Any(n => a.AccountID.Equals(n))
                                                           select new SelectListItem
                                                           {
                                                               Text = a.AssessmentName,
                                                               Value = a.Id.ToString()
                                                           }).ToList();

                ret1.AssessmentNameListDropDown = AssessmentNameList;
            }
            return Json(ret1.AssessmentNameListDropDown, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getassessmentnames1(string AccrDesc)
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


            string[] stringSplit = AccrDesc.Split(new string[] { "#" }, StringSplitOptions.None);
            int[] intSplit = new int[stringSplit.Length];
            int i = 0;
            foreach (var a in stringSplit)
            {
                if (a.Length > 0)
                    intSplit[i] = Convert.ToInt32(a);
                else
                    intSplit[i] = 0;
                i++;
            }

            var AssessmentNameList = (from a in db.Assessment
                                      where intSplit.Any(n => a.AccountID.Equals(n))
                                      select new SelectListItem
                                      {
                                          Text = a.AssessmentName,
                                          Value = a.Id.ToString()
                                      }).ToList();

            return Json(AssessmentNameList.ToList().Select(A => new { Value = A.Value, Text = A.Text, Selected = true }), JsonRequestBehavior.AllowGet);

        }

        public JsonResult getAssessmentNameforMatGraph(int[] Accarr2)
        {
            A3MMModel ret1 = new A3MMModel();
            Db db = new Db();

            ret1.AssessmentNameListDropDown = ret1.AssessmentNameList;

            if (Accarr2 != null && Accarr2.Length > 0)
            {
                List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                           where Accarr2.Any(n => a.AccountID.Equals(n))
                                                           select new SelectListItem
                                                           {
                                                               Text = a.AssessmentName,
                                                               Value = a.Id.ToString()
                                                           }).ToList();

                ret1.AssessmentNameListDropDown = AssessmentNameList;
            }

            return Json(ret1.AssessmentNameListDropDown, JsonRequestBehavior.AllowGet);
        }



        //Action Item Tracker screen
        [ActionTrack]
        public ActionResult ActionItemTracker()
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            ActionItemTrackerModel a3 = prepareCommonParameters2();
            Db db = new Db();

            var accountID = Convert.ToInt32(a3.AccountNamesList.FirstOrDefault().Value);



            List<SelectListItem> AssessmentNameList = (from a in db.Assessment
                                                       where a.ClientId == Cl.ClientId && a.SurveyTypeID == 1 && a.AccountID == accountID
                                                       select new SelectListItem
                                                       {
                                                           Text = a.AssessmentName,
                                                           Value = a.Id.ToString()
                                                       }).ToList();

            a3.AssessmentNameList = AssessmentNameList;

            return View(a3);

        }


        public ActionResult ReadAllActionItem([DataSourceRequest] DataSourceRequest request, ActionItemTrackerModel ait)
        {
            var client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            Db db = new Db();
            A3MMModel a3 = prepareCommonParameters();
            var AssessmentID = ait.AssessmentID;

            var AssessmentTypeID = ait.AssessmentTypeID;
            var AccountID = ait.AccountID;
            // var ContractID = ait.ContractID;

            IList<ActionItemTrackerModel> actionItemList = (List<ActionItemTrackerModel>)AssessmentManagerHelper.GetActionItems(AssessmentID, AssessmentTypeID, AccountID/*, ContractID*/, ClientId);

            return Json(actionItemList.ToDataSourceResult(request));
        }


        public ActionResult ReadAllActionItemLog([DataSourceRequest] DataSourceRequest request, ActionItemTrackerModel ait)
        {
            var client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            Db db = new Db();
            var AssessmentID = ait.AssessmentID;
            var AssessmentTypeID = ait.AssessmentTypeID;
            var AccountID = ait.AccountID;
            //var ContractID = ait.ContractID;
            IList<ActionItemTrackerModel> actionItemList = (List<ActionItemTrackerModel>)AssessmentManagerHelper.GetActionItems(AssessmentID, AssessmentTypeID, AccountID/*, ContractID*/, ClientId);


            List<int> ActionItemIDList = (from a in actionItemList
                                          select a.Id).Distinct().ToList();

            IList<ActionItemTrackerModel> actionItemLogList = (from a in db.ActionItemTrackerLog
                                                               where ActionItemIDList.Contains(a.ActionItemID)
                                                               select new ActionItemTrackerModel
                                                               {
                                                                   Id = a.Id,
                                                                   ActionItemID = a.ActionItemID,
                                                                   LogupdateDate = a.updateDate,
                                                                   Updates = a.Updates
                                                               }).OrderByDescending(x => x.Id).ToList();



            return Json(actionItemLogList.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult getActionItemRecord(int ActionItemID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            //A3MMAssessmentModel a3mm = new A3MMAssessmentModel();

            Db db = new Db();
            ActionItemTrackerModel ait = (from a in db.ActionItemTracker
                                          join b in db.Assessment on a.AssessmentID equals b.Id
                                          where a.Id == ActionItemID
                                          select new ActionItemTrackerModel
                                          {
                                              Id = a.Id,
                                              OwnerID = a.OwnerID,
                                              PlannedCompletionDate = a.PlannedCompletionDate,
                                              Status = a.Status,
                                              Benefits = a.Benefits,
                                              Comments = a.Comments,
                                              AssessmentStatus = b.Status,
                                              ActionItem=a.ActionItem,
                                              AssessmentName=b.AssessmentName
                                          }).FirstOrDefault();

            if (ait != null)
            {
                var ownerName = (from a in db.UserProfiles
                                 where a.Id == ait.OwnerID
                                 select a.FirstName + a.LastName).FirstOrDefault();

                ait.OwnerName = ownerName;
            }
            return Json(ait, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ActionTrack]
        public ActionResult SaveActionItem(ActionItemTrackerModel ait)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ait.ClientID = client.ClientId;
            int usid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var updatetitem = unitOfWork.ActionItemTracker.GetByID(ait.Id);
            var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false, IdeaId = updatetitem.IdeaId }, "http");
            var res = AssessmentManagerHelper.addorEditActionItem(ait, usid, updatetitem, IdeaLink, client.ClientName);


            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckActionIdeaStatus(ActionItemTrackerModel ait)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ait.ClientID = client.ClientId;
            var res = AssessmentManagerHelper.checkActionIdeaStatus(ait);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAssessmentNames(int typeid = 0)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            Db db = new Db();

            var assessList = (from a in db.Assessment
                              where a.SurveyTypeID == typeid && a.ClientId == ClientId
                              select new
                              {
                                  Id = a.Id,
                                  AssessmentName = a.AssessmentName
                              }).ToList().Distinct();
            return Json(assessList.ToList().Select(A => new { Value = A.Id.ToString(), Text = A.AssessmentName, Selected = true }), JsonRequestBehavior.AllowGet);

        }

        [ActionTrack]
        public FileResult DownloaduploadedfileAction(int fileId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var fileUpload = unitOfWork.QuestRespAttachments.GetByID(fileId);


                byte[] fileContent = null;
                if (fileUpload.AttachFile != null)
                {
                    var str = fileUpload.AttachFile.Substring(fileUpload.AttachFile.LastIndexOf(',') + 1);
                    fileContent = Convert.FromBase64String(str);
                }
                else
                    fileContent = fileUpload.FileCnt;
                return File(fileContent, System.Net.Mime.MediaTypeNames.Application.Octet, fileUpload.FileName);
            }
        }


        [HttpPost]
        [ActionTrack]
        public ActionResult FinalizeSingleQuestion(int? QuestionId = 0)
        {
            if (QuestionId != 0)
            {
                AssessmentManagerHelper.FinalizeSingleQuestion((int)QuestionId);
            }
            return Json(true, JsonRequestBehavior.AllowGet);


        }


        public ActionResult ExportAssessmentoverview(int SurveyTypeID, int AssessmentID, string version)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            string fileName = version + "_QuestionSet" + ".xlsx";
            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(GetAssessmentoverview(ClientId, SurveyTypeID, AssessmentID, version));
            stream.Position = 0;
            string mimeType = "application/ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;
        }
        private System.Data.DataTable GetAssessmentoverview(int ClientId, int SurveyTypeID, int AssessmentID, string version)
        {
            //int usid = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            int clientid = Cl.ClientId;
            Db db = new Db();
            int assessmentID = Convert.ToInt32(AssessmentID);
            int surveyTypeID = Convert.ToInt32(SurveyTypeID);


            var versionIDinitial = (from a in db.Assessment
                                    where a.Id == assessmentID
                                    select a.QuestionaireVersion).FirstOrDefault();


            var qlist = (from a in db.QuestionnaireResponse
                         where a.AssessmentID == assessmentID && a.SurveyTypeId == surveyTypeID && a.VersionID == versionIDinitial && a.ClientID == clientid
                         select a).ToList();


            var eQlist = (from a in db.EngagementQuestionnaire
                          where a.SurveyTypeID == surveyTypeID && a.VersionID == versionIDinitial
                          select a).ToList();

            IList<A3MMModel> a3list = new List<A3MMModel>();
            IList<AssessmentOverviewExport> a3exportlist = new List<AssessmentOverviewExport>();
            foreach (var a in qlist) //72
            {
                EngagementQuestionnaire e = (from b in eQlist
                                             where b.SQNo == a.SQNo && b.SurveyTypeID == a.SurveyTypeId
                                             select b).FirstOrDefault();
                List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                           where x.QuestionRespId == a.Id
                                                           select new ResponseAttachment
                                                           {
                                                               Id = x.Id,
                                                               filename = x.FileName,
                                                               fileCont = x.AttachFile
                                                           }).ToList();


                if (e != null)
                {
                    AssessmentOverviewExport a3mm = new AssessmentOverviewExport();

                    a3mm.SQNo = e.SQNo;
                    a3mm.DisciplineName = e.DisciplineName;
                    a3mm.ComponentName = e.ComponentName;
                    a3mm.AspectName = e.AspectName;
                    a3mm.Question = e.Question;
                    a3mm.ExampleAnswer = e.ExampleAns;
                    // a3mm.Response = GetPlainText(a.Response);
                    // a3mm.Feedback = GetPlainText(a.Feedback);
                    a3mm.Response = GetPlainText((from mr in db.A3MM_MultipleResponses
                                                  where mr.AssessmentId == assessmentID && mr.ResponseID == a.Id
                                                  && mr.SubResponseOrder == 1
                                                  select mr.SubResponse).ToList().FirstOrDefault());

                    a3mm.FollowupQuestion = GetPlainText((from mf in db.A3MM_MultipleFollowUpQuestions
                                                          where mf.AssessmentId == assessmentID && mf.ResponseID == a.Id
                                                          select mf.SubFollowUpQuestion).FirstOrDefault());
                    a3mm.Followup_Response = GetPlainText((from mr in db.A3MM_MultipleResponses
                                                           where mr.AssessmentId == assessmentID && mr.ResponseID == a.Id
                                                           && mr.SubResponseOrder == 2
                                                           select mr.SubResponse).ToList().FirstOrDefault());
                    a3mm.MaturityLevel = a.MaturityLevel;
                    a3mm.AreasofImprovement = GetPlainText(a.AreaOfImprovement);
                    // a3mm.RespAttachment = RespAttachment;
                    a3exportlist.Add(a3mm);
                }
            }
            ListtoDataTable lsttodt = new ListtoDataTable();
            DataTable dt = lsttodt.ToDataTable(a3exportlist.ToList());
            dt.TableName = "QuestionSet";

            return dt;
        }


        public ActionResult ExportBuildDiscipline(int SurveyTypeID, string QSetNo, string isView = "No")
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            string fileName = QSetNo + "_QuestionSet" + ".xlsx";
            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(GetBuildDiscipline(ClientId, SurveyTypeID, QSetNo, isView));
            stream.Position = 0;
            string mimeType = "application/ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;
        }
        private System.Data.DataTable GetBuildDiscipline(int ClientId, int SurveyTypeID, string QSetNo, string isView)
        {
            var contextFactory = new DbContextFactory();
            DataTable returntable = new DataTable();
            using (var unitOfWork = new UnitOfWorks(contextFactory.GetContext()))
            {
                var paramName = new string[] { "ClientId", "SurveyTypeID", "QSetNo" };
                var paramValue = new object[] { ClientId, SurveyTypeID, QSetNo };
                returntable = unitOfWork.GetDataFromStoredProcedure("A3MM_ExtractDisciplineData_Export", paramName, paramValue);
            }
            returntable.TableName = "QuestionSet";
            return returntable;
        }

        [HttpPost]
        public ActionResult GetAssessmentTypeWithACD(A3MMDCAFilter mod)
        {
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            var UserId = (int)Session["UserId"];
            int assessID = Convert.ToInt32(mod.AssessmentID); //id of Assessment Table
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();


            //New Approach

            A3MMModel ret = prepareCommonParameters(); // fetching dropdown values
            A3MMModel initial = new A3MMModel();
            var AssessmentIDinitial = assessID;  //recieved as parameter

            var surveyTypeIDinitial = (from a in db.Assessment
                                       where a.Id == AssessmentIDinitial && a.ClientId == Cl.ClientId
                                       select a.SurveyTypeID).FirstOrDefault();


            var versionID = (from a in db.Assessment
                             where a.Id == AssessmentIDinitial && a.ClientId == Cl.ClientId
                             select a.QuestionaireVersion).FirstOrDefault();


            List<A3MMModel> qSET = new List<A3MMModel>();

            List<EngagementQuestionnaire> questionList = new List<EngagementQuestionnaire>();

            if (mod.isAspChanged == 1 && mod.AspectID > 0)
            {
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID && a.AspectID == mod.AspectID
                                select a).ToList();
            }
            else if (mod.iscompChanged == 1 && mod.ComponentId > 0)
            {
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID
                                && a.Component_ID == mod.ComponentId
                                select a).ToList();
            }
            else if (mod.isdiscChanged == 1 && mod.DisciplineId > 0)
            {
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID && a.Discipline_ID == mod.DisciplineId
                                select a).ToList();
            }
            else
            {
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID
                                select a).ToList();
            }

            var responseList = (from a in db.QuestionnaireResponse
                                where a.SurveyTypeId == surveyTypeIDinitial && a.AssessmentID == AssessmentIDinitial && a.VersionID == versionID && a.ClientID == Cl.ClientId
                                select a).ToList();


            foreach (var a in responseList)
            {
                EngagementQuestionnaire e = (from b in questionList
                                             where b.SQNo == a.SQNo && b.SurveyTypeID == a.SurveyTypeId
                                             select b).FirstOrDefault();
                List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                           where x.QuestionRespId == a.Id
                                                           select new ResponseAttachment
                                                           {
                                                               Id = x.Id,
                                                               filename = x.FileName,
                                                               fileCont = x.AttachFile
                                                           }).ToList();


                if (e != null)
                {
                    A3MMModel a3mm = new A3MMModel();
                    a3mm.QNo = e.QNo;
                    a3mm.SurveyTypeID = a.SurveyTypeId;
                    a3mm.AssessmentID = a.AssessmentID;
                    a3mm.DisciplineId = Convert.ToInt32(e.Discipline_ID);
                    a3mm.DisciplineName = e.DisciplineName;
                    a3mm.ComponentId = Convert.ToInt32(e.Component_ID);
                    a3mm.ComponentName = e.ComponentName;
                    a3mm.AspectID = e.AspectID;
                    a3mm.AspectName = e.AspectName;
                    a3mm.Question = e.Question;
                    a3mm.ExampleAnswer = e.ExampleAns;
                    a3mm.Response = a.Response;
                    a3mm.AssessmentResponse = a.AssessmentResponse;
                    a3mm.Feedback = a.Feedback;
                    a3mm.FollowupQuestion = a.FollowUpQuestion;
                    a3mm.MaturityLevel = a.MaturityLevel;
                    // a3mm.MaturityLevelInt = Convert.ToInt32(a.MaturityLevel);
                    a3mm.AreasofImprovement = a.AreaOfImprovement;
                    a3mm.QuestionId = e.Id;
                    a3mm.ClientId = a.ClientID;
                    a3mm.QuestionResponseID = a.Id;
                    a3mm.LastFeedbackBy = Convert.ToInt32(a.LastFeedBackBy);
                    a3mm.LastFeedbackOn = a.LastFeedBackOn;
                    a3mm.LastUpdatedBy = Convert.ToInt32(a.LastUpdatedBy);
                    a3mm.LastUpdatedOn = a.LastUpdatedOn;
                    a3mm.VersionID = a.VersionID;
                    a3mm.DatasourceId = e.DataSourceID;
                    a3mm.SQNo = e.SQNo;
                    a3mm.SubQOrder = e.SubQOrder;
                    a3mm.QuestionOrder = e.QuestionOrder;
                    a3mm.RespAttachment = RespAttachment;

                    qSET.Add(a3mm);
                }


            }

            if (qSET.Count != 0)
            {
                ret.QuestionSet = qSET.OrderBy(e => e.QuestionOrder).ToList();
                ret.SurveyTypeID = qSET[0].SurveyTypeID;
                ret.AssessmentID = qSET[0].AssessmentID;
                ret.Status = (from a in db.Assessment
                              where a.Id == AssessmentIDinitial && a.SurveyTypeID == surveyTypeIDinitial
                              select a.Status).FirstOrDefault();
                qSET[0].Status = ret.Status;
                ret.AspectID = qSET[0].AspectID;
                ret.DisciplineId = qSET[0].DisciplineId;
                ret.ComponentId = qSET[0].ComponentId;
                ret.QNo = qSET[0].QNo;
                ret.ClientId = qSET[0].ClientId;
                ret.VersionID = qSET[0].VersionID;
                ret.MaturityLevel = qSET[0].MaturityLevel;
                // ret.MaturityLevelInt = qSET[0].MaturityLevelInt;
                ret.FollowupQuestion = qSET[0].FollowupQuestion;
                ret.Response = qSET[0].Response;
                ret.Feedback = qSET[0].Response;
                ret.AreasofImprovement = qSET[0].AreasofImprovement;
                ret.DatasourceId = qSET[0].DatasourceId;
                ret.LastFeedbackBy = qSET[0].LastFeedbackBy;
                ret.LastFeedbackOn = qSET[0].LastFeedbackOn;
                ret.LastUpdatedBy = qSET[0].LastUpdatedBy;
                ret.LastUpdatedOn = qSET[0].LastUpdatedOn;
                ret.RespAttachment = qSET[0].RespAttachment;
                ret.LastUpdatedByName = (from a in db.UserProfiles
                                         where a.Id == ret.LastUpdatedBy
                                         select a.UserName).FirstOrDefault();
                qSET[0].LastUpdatedByName = ret.LastUpdatedByName;
                ret.LastFeedbackByName = (from a in db.UserProfiles
                                          where a.Id == ret.LastFeedbackBy
                                          select a.UserName).FirstOrDefault();
                qSET[0].LastFeedbackByName = ret.LastFeedbackByName;

            }


            var currentUserRoleList = (from u in db.UserApplications
                                       join r in db.UserRoles on u.RoleId equals r.Id
                                       where u.ClientId == Cl.ClientId && u.UserId == UserId && u.IsDeleted == false && u.ApplicationId == 20
                                       select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

            var UserRole = "";


            var Item1 = currentUserRoleList.Select(lst => new
            {
                lst.Value,
                lst.Text,
                lst.Weight
            }).OrderByDescending(o => o.Weight).FirstOrDefault();
            if (Item1 != null)
                UserRole = Item1.Text;
            else
                UserRole = "";
            if (qSET.Count != 0)
            {
                ret.QuestionSet[0].UserRole = UserRole;
            }
            return Json(ret.QuestionSet, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ReadAspect(int? DisciplineId = 0, int? ComponentId = 0)
        {
            Db db = new Db();
            var apList = (from a in db.Aspect
                          where a.A3_DisciplineId == DisciplineId && a.ComponentId == ComponentId
                          select a).ToList();
            var item = apList.OrderBy(x => x.AspectName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.AspectName.ToString() });
            return Json(item, JsonRequestBehavior.AllowGet); ;

        }
        public ActionResult Readcomponent(int? DisciplineId = 0)
        {
            Db db = new Db();
            var apList = (from a in db.Component
                          where a.DisciplineId == DisciplineId
                          select a).ToList();
            var item = apList.OrderBy(x => x.ComponentName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ComponentName.ToString() });
            return Json(item, JsonRequestBehavior.AllowGet); ;

        }
        public string GetPlainText(string editorText)
        {
            if (editorText != null)
            {
                var text = editorText;
                text = text.Replace("<", "");
                text = text.Replace(">", "");
                text = text.Replace("&nbsp;", " ");
                text = text.Replace("&gt;", ">");
                text = text.Replace("&lt;", "<");
                text = text.Replace("&amp;", "&");
                text = text.Replace("amp;", "");
                text = text.Replace("&quot;", "'");
                text = text.Replace("&apos;", "'");
                text = text.Replace("&bull;", "-");
                text = text.Replace("/&ndash;", "-");
                text = text.Replace("br", " ");
                return text;
            }
            else
                return " ";
        }
        [HttpPost]
        public ActionResult ComponentDeatils([DataSourceRequest] DataSourceRequest request, string assessmentName, string ComponentName)
        {
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            List<DashModel> list2 = new List<DashModel>();

            Db db = new Db();
            var assid = (from c in db.Assessment
                         where c.AssessmentName == assessmentName
                         select c.Id).FirstOrDefault();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                List<EngagementQuestionnaireModel> listlevel = new List<EngagementQuestionnaireModel>();
                var paramName = new string[] { "ClientId", "AssessmentID", "ComponentName" };
                var paramValue = new object[] { Cl.ClientId, assid, ComponentName };
                var ds = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue);
                foreach (DataRow dr in ds.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel.Add(aspect);
                }
                var aspt = listlevel.GroupBy(x => x.AspectName);
                foreach (var grp in aspt)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;
                    m1.MaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.Comment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null)
                            m1.Comment = m1.Comment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.Comment = m1.Comment != "" ? m1.Comment.Substring(5) : "";
                    list2.Add(m1);
                }
            }

            return Json(list2.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ComparativeCompDeatils([DataSourceRequest] DataSourceRequest request, string firstassessment, string secondassessment, string ComponentName)
        {
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            List<DashModel> finalList = new List<DashModel>();

            Db db = new Db();
            var assessdetail = (from c in db.Assessment
                                select c).ToList();
            var FirstAssid = assessdetail.Where(w => w.AssessmentName == firstassessment).Select(w => w.Id).FirstOrDefault();
            var SecondAssid = assessdetail.Where(w => w.AssessmentName == secondassessment).Select(w => w.Id).FirstOrDefault();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                List<EngagementQuestionnaireModel> listlevel1 = new List<EngagementQuestionnaireModel>();
                List<EngagementQuestionnaireModel> listlevel2 = new List<EngagementQuestionnaireModel>();
                var paramName = new string[] { "ClientId", "AssessmentID", "ComponentName" };
                var paramValue = new object[] { Cl.ClientId, FirstAssid, ComponentName };
                var ds = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue);
                foreach (DataRow dr in ds.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel1.Add(aspect);
                }
                var paramValue1 = new object[] { Cl.ClientId, SecondAssid, ComponentName };
                var ds1 = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue1);
                foreach (DataRow dr in ds1.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel2.Add(aspect);
                }
                var aspt1 = listlevel1.GroupBy(x => x.AspectName);
                foreach (var grp in aspt1)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;
                    m1.FirstMaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.FirstComment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null && resp.Response != "")
                            m1.FirstComment = m1.FirstComment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.FirstComment = m1.FirstComment != "" ? m1.FirstComment.Substring(5) : "";
                    m1.FirstAssessmentName = firstassessment;
                    finalList.Add(m1);
                }

                var aspt2 = listlevel2.GroupBy(x => x.AspectName);
                foreach (var grp in aspt2)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;

                    m1.SecondMaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.SecondComment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null && resp.Response != "")
                            m1.SecondComment = m1.SecondComment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.SecondComment = m1.SecondComment != "" ? m1.SecondComment.Substring(5) : "";
                    m1.SecondAssessmentName = secondassessment;

                    foreach (var mc in finalList.Where(x => x.AspectName == grp.Key))
                    {
                        mc.SecondMaturityLevel = m1.SecondMaturityLevel;
                        mc.SecondComment = m1.SecondComment;

                    }
                }
            }

            return Json(finalList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ComparativeCompDeatils3([DataSourceRequest] DataSourceRequest request, string firstassessment,
            string secondassessment, string thirdassessment, string ComponentName)
        {
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            List<DashModel> finalList = new List<DashModel>();

            Db db = new Db();
            var assessdetail = (from c in db.Assessment
                                select c).ToList();
            var FirstAssid = assessdetail.Where(w => w.AssessmentName == firstassessment).Select(w => w.Id).FirstOrDefault();
            var SecondAssid = assessdetail.Where(w => w.AssessmentName == secondassessment).Select(w => w.Id).FirstOrDefault();
            var ThirdAssid = assessdetail.Where(w => w.AssessmentName == thirdassessment).Select(w => w.Id).FirstOrDefault();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                List<EngagementQuestionnaireModel> listlevel1 = new List<EngagementQuestionnaireModel>();
                List<EngagementQuestionnaireModel> listlevel2 = new List<EngagementQuestionnaireModel>();
                List<EngagementQuestionnaireModel> listlevel3 = new List<EngagementQuestionnaireModel>();
                var paramName = new string[] { "ClientId", "AssessmentID", "ComponentName" };
                var paramValue = new object[] { Cl.ClientId, FirstAssid, ComponentName };
                var ds = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue);
                foreach (DataRow dr in ds.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel1.Add(aspect);
                }
                var paramValue1 = new object[] { Cl.ClientId, SecondAssid, ComponentName };
                var ds1 = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue1);
                foreach (DataRow dr in ds1.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel2.Add(aspect);
                }
                var paramValue2 = new object[] { Cl.ClientId, ThirdAssid, ComponentName };
                var ds2 = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue2);
                foreach (DataRow dr in ds2.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel3.Add(aspect);
                }

                var aspt1 = listlevel1.GroupBy(x => x.AspectName);
                foreach (var grp in aspt1)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;
                    m1.FirstMaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.FirstComment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null && resp.Response != "")
                            m1.FirstComment = m1.FirstComment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.FirstComment = m1.FirstComment != "" ? m1.FirstComment.Substring(5) : "";
                    m1.FirstAssessmentName = firstassessment;
                    finalList.Add(m1);
                }

                var aspt2 = listlevel2.GroupBy(x => x.AspectName);
                foreach (var grp in aspt2)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;

                    m1.SecondMaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.SecondComment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null && resp.Response != "")
                            m1.SecondComment = m1.SecondComment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.SecondComment = m1.SecondComment != "" ? m1.SecondComment.Substring(5) : "";
                    m1.SecondAssessmentName = secondassessment;

                    foreach (var mc in finalList.Where(x => x.AspectName == grp.Key))
                    {
                        mc.SecondMaturityLevel = m1.SecondMaturityLevel;
                        mc.SecondComment = m1.SecondComment;

                    }
                }

                var aspt3 = listlevel3.GroupBy(x => x.AspectName);
                foreach (var grp in aspt3)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;

                    m1.ThirdMaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.ThirdComment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null && resp.Response != "")
                            m1.ThirdComment = m1.ThirdComment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.ThirdComment = m1.ThirdComment != "" ? m1.ThirdComment.Substring(5) : "";
                    m1.ThirdAssessmentName = thirdassessment;

                    foreach (var mc in finalList.Where(x => x.AspectName == grp.Key))
                    {
                        mc.ThirdMaturityLevel = m1.ThirdMaturityLevel;
                        mc.ThirdComment = m1.ThirdComment;

                    }
                }
            }

            return Json(finalList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ComparativeCompDeatils4([DataSourceRequest] DataSourceRequest request, string firstassessment, string secondassessment, string thirdassessment, string fourassessment, string ComponentName)
        {
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            List<DashModel> finalList = new List<DashModel>();

            Db db = new Db();
            var assessdetail = (from c in db.Assessment
                                select c).ToList();
            var FirstAssid = assessdetail.Where(w => w.AssessmentName == firstassessment).Select(w => w.Id).FirstOrDefault();
            var SecondAssid = assessdetail.Where(w => w.AssessmentName == secondassessment).Select(w => w.Id).FirstOrDefault();
            var ThirdAssid = assessdetail.Where(w => w.AssessmentName == thirdassessment).Select(w => w.Id).FirstOrDefault();
            var FourAssid = assessdetail.Where(w => w.AssessmentName == fourassessment).Select(w => w.Id).FirstOrDefault();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                List<EngagementQuestionnaireModel> listlevel1 = new List<EngagementQuestionnaireModel>();
                List<EngagementQuestionnaireModel> listlevel2 = new List<EngagementQuestionnaireModel>();
                List<EngagementQuestionnaireModel> listlevel3 = new List<EngagementQuestionnaireModel>();
                List<EngagementQuestionnaireModel> listlevel4 = new List<EngagementQuestionnaireModel>();
                var paramName = new string[] { "ClientId", "AssessmentID", "ComponentName" };
                var paramValue = new object[] { Cl.ClientId, FirstAssid, ComponentName };
                var ds = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue);
                foreach (DataRow dr in ds.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel1.Add(aspect);
                }
                var paramValue1 = new object[] { Cl.ClientId, SecondAssid, ComponentName };
                var ds1 = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue1);
                foreach (DataRow dr in ds1.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel2.Add(aspect);
                }
                var paramValue2 = new object[] { Cl.ClientId, ThirdAssid, ComponentName };
                var ds2 = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue2);
                foreach (DataRow dr in ds2.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel3.Add(aspect);
                }
                var paramValue3 = new object[] { Cl.ClientId, FourAssid, ComponentName };
                var ds3 = unitOfWork.GetDataFromStoredProcedure("A3MM_ComponentDetails", paramName, paramValue3);
                foreach (DataRow dr in ds3.Rows)
                {
                    var aspect = new EngagementQuestionnaireModel();
                    aspect.ComponentName = dr["ComponentName"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        if (dr["MaturityLevel"].ToString() == "NA")
                            aspect.MaturityLevel = "0";
                        else
                            aspect.MaturityLevel = dr["MaturityLevel"].ToString();
                    }
                    else
                        aspect.MaturityLevel = "0";
                    aspect.AspectName = dr["AspectName"].ToString();
                    aspect.Response = dr["AssessmentResponse"].ToString();
                    listlevel4.Add(aspect);
                }

                var aspt1 = listlevel1.GroupBy(x => x.AspectName);
                foreach (var grp in aspt1)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;
                    m1.FirstMaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.FirstComment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null && resp.Response != "")
                            m1.FirstComment = m1.FirstComment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.FirstComment = m1.FirstComment != "" ? m1.FirstComment.Substring(5) : "";
                    m1.FirstAssessmentName = firstassessment;
                    finalList.Add(m1);
                }

                var aspt2 = listlevel2.GroupBy(x => x.AspectName);
                foreach (var grp in aspt2)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;

                    m1.SecondMaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.SecondComment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null && resp.Response != "")
                            m1.SecondComment = m1.SecondComment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.SecondComment = m1.SecondComment != "" ? m1.SecondComment.Substring(5) : "";
                    m1.SecondAssessmentName = secondassessment;

                    foreach (var mc in finalList.Where(x => x.AspectName == grp.Key))
                    {
                        mc.SecondMaturityLevel = m1.SecondMaturityLevel;
                        mc.SecondComment = m1.SecondComment;

                    }
                }

                var aspt3 = listlevel3.GroupBy(x => x.AspectName);
                foreach (var grp in aspt3)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;

                    m1.ThirdMaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.ThirdComment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null && resp.Response != "")
                            m1.ThirdComment = m1.ThirdComment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.ThirdComment = m1.ThirdComment != "" ? m1.ThirdComment.Substring(5) : "";
                    m1.ThirdAssessmentName = thirdassessment;

                    foreach (var mc in finalList.Where(x => x.AspectName == grp.Key))
                    {
                        mc.ThirdMaturityLevel = m1.ThirdMaturityLevel;
                        mc.ThirdComment = m1.ThirdComment;

                    }
                }

                var aspt4 = listlevel4.GroupBy(x => x.AspectName);
                foreach (var grp in aspt4)
                {
                    DashModel m1 = new DashModel();
                    m1.AspectName = grp.Key;

                    m1.FourMaturityLevel = Math.Round(grp.Average(a => Convert.ToInt32(a.MaturityLevel)), 2); ;
                    m1.FourComment = "";
                    foreach (var resp in grp)
                    {
                        if (resp.Response != null && resp.Response != "")
                            m1.FourComment = m1.FourComment + " <br>" + resp.Response.Replace("&nbsp;", "");
                    }
                    m1.FourComment = m1.FourComment != "" ? m1.FourComment.Substring(5) : "";
                    m1.ThirdAssessmentName = thirdassessment;

                    foreach (var mc in finalList.Where(x => x.AspectName == grp.Key))
                    {
                        mc.FourMaturityLevel = m1.FourMaturityLevel;
                        mc.FourComment = m1.FourComment;

                    }
                }
            }

            return Json(finalList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssessmentReport()
        {
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();

            A3MMModel mod = new A3MMModel();
            List<SelectListItem> accountNameInitial = new List<SelectListItem>();
            var AccountDetail = (from a in db.ClientAccountSetup
                                 where a.ClientId == Cl.ClientId
                                 select a).FirstOrDefault();


            mod.AccountName = AccountDetail.AccountName;

            var AssessmentInitial = (from a in db.Assessment
                                     where a.ClientId == Cl.ClientId
                                     select a).FirstOrDefault();
            var AssessmentSecond = (from a in db.Assessment
                                    where a.ClientId == Cl.ClientId && a.Id != AssessmentInitial.Id
                                    select a).FirstOrDefault();
            var AssessmentThird = (from a in db.Assessment
                                   where a.ClientId == Cl.ClientId && a.Id != AssessmentInitial.Id
                                   && a.Id != AssessmentSecond.Id
                                   select a).FirstOrDefault();
            var AssessmentFourth = (from a in db.Assessment
                                    where a.ClientId == Cl.ClientId && a.Id != AssessmentInitial.Id
                                     && a.Id != AssessmentSecond.Id && a.Id != AssessmentThird.Id
                                    select a).FirstOrDefault();

            mod.AssessmentNameList = (from b in db.Assessment
                                      where b.AccountID == AccountDetail.Id
                                      select new SelectListItem
                                      {
                                          Value = b.Id.ToString(),
                                          Text = b.AssessmentName
                                      }).ToList();
            mod.SecondAssessmentNameList = (from b in db.Assessment
                                            where b.AccountID == AccountDetail.Id && b.Id != AssessmentInitial.Id
                                            select new SelectListItem
                                            {
                                                Value = b.Id.ToString(),
                                                Text = b.AssessmentName
                                            }).ToList();

            mod.ThirdAssessmentNameList = (from b in db.Assessment
                                           where b.AccountID == AccountDetail.Id && b.Id != AssessmentInitial.Id
                                           && b.Id != AssessmentSecond.Id
                                           select new SelectListItem
                                           {
                                               Value = b.Id.ToString(),
                                               Text = b.AssessmentName
                                           }).ToList();
            SelectListItem newItem = new SelectListItem { Text = "No Selection", Value = "0" };
            if (mod.ThirdAssessmentNameList != null)
            {
                List<SelectListItem> modifiableList = mod.ThirdAssessmentNameList.ToList();
                modifiableList.Insert(0, newItem);
                mod.ThirdAssessmentNameList = modifiableList;
            }



            mod.FourthAssessmentNameList = (from b in db.Assessment
                                            where b.AccountID == AccountDetail.Id && b.Id != AssessmentInitial.Id
                                            && b.Id != AssessmentSecond.Id && b.Id != AssessmentThird.Id
                                            select new SelectListItem
                                            {
                                                Value = b.Id.ToString(),
                                                Text = b.AssessmentName
                                            }).ToList();
            if (mod.FourthAssessmentNameList != null)
            {
                List<SelectListItem> modifiableList1 = mod.FourthAssessmentNameList.ToList();
                modifiableList1.Insert(0, newItem);
                mod.FourthAssessmentNameList = modifiableList1;
            }
            mod.AssessmentID = Convert.ToInt32(mod.AssessmentNameList.First().Value);
            mod.AssessmentName = mod.AssessmentNameList.First().Text;
            mod.ClientId = Cl.ClientId;
            mod.ClientName = Cl.ClientName;

            return View(mod);
        }
    }
    public class ListtoDataTable
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
