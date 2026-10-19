using Data;
using Domain.Entities.A3MM;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.AssessmentManager.Models;
using WebUI.Dto;
using System.IO;
using System.Web.UI;
using System.ComponentModel;
using WebUI.Areas.IncidentNavigator;
using DocumentFormat.OpenXml.Office2010.Excel;
using log4net;
using WebUI.Areas.IdeaManager.Controllers;

namespace WebUI.Areas.AssessmentManager.Controllers
{
    public class ResponseGatheringController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ViewIdeaController));
        // GET: AssessmentManager/ResponseGathering
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
        [ActionTrack]
        public ActionResult EngagementDataGathering(int AssessmentIDparam = 0, int QNoparam = 0, int isView = 0, int isAI = 0, int SurveyTypeID = 0)
        {
            A3MMModel ret1 = new A3MMModel();
            ret1.AssessmentID = AssessmentIDparam;
            ret1= GetDataGatheringDetail(ret1,"");
            if (ret1.AssessmentID==0)
            {
                return View("ErrorView");
            }
            return View(ret1);
            }

        [ActionTrack]
        public ActionResult AssessmentDataGathering(int AssessmentIDparam = 0, string QNoparam = "", int isView = 0, int isAI = 0, int SurveyTypeID = 0)
        {

            // A3MMModel bfrret1 = prepareCommonParameters();
            A3MMModel ret1 = new A3MMModel();
            ret1.AssessmentID = AssessmentIDparam;
            ret1.SQNo = QNoparam;
            ret1 = GetDataGatheringDetail(ret1, "AssTeamView");

            return View(ret1);
        }

        [ActionTrack]
        public ActionResult AssessmentResponseView(int AssessmentIDparam = 0, int QNoparam = 0, int isView = 0, int isAI = 0, int SurveyTypeID = 0)
        {

            // A3MMModel bfrret1 = prepareCommonParameters();
            A3MMModel ret1 = new A3MMModel();
            ret1.AssessmentID = AssessmentIDparam;
            ret1 = GetDataGatheringDetail(ret1,"AssessmentResponseview");
            if (ret1.AssessmentID == 0)
            {
                return View("ErrorView");
            }
            return View(ret1);
        }

        private A3MMModel GetDataGatheringDetail(A3MMModel ret1,string ViewName="")
        {
            var UserId = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var AssessmentIDParam = ret1.AssessmentID;
            ret1 = prepareCommonParameters();
            
            var AssessmentValue = new Assessment();
            if (AssessmentIDParam != 0)
            {

                AssessmentValue = (from a in db.Assessment
                                   where a.Id == AssessmentIDParam && a.ClientId == Cl.ClientId
                                   select a).FirstOrDefault();
            }else if (ViewName=="AssessmentResponseview")
            {
                AssessmentValue = (from a in db.Assessment
                                   where a.ClientId == Cl.ClientId  //A3MM
                                   select a).OrderByDescending(x=>x.Id).FirstOrDefault();
            }
            else
            {
                AssessmentValue = (from a in db.Assessment
                                   where a.ClientId == Cl.ClientId && a.Status != "Published" & a.Status != "Preparation" //A3MM
                                   select a).FirstOrDefault();
            }
           
            if (AssessmentValue == null)
            {
                ret1.AssessmentID = 0;
                return ret1;
            }

            int AssessmentIDinitial = AssessmentValue.Id;
            var surveyTypeIDinitial = AssessmentValue.SurveyTypeID;
            var versionIDinitial = AssessmentValue.QuestionaireVersion;
            ret1.AssessmentName = AssessmentValue.AssessmentName;
         
            List<A3MMModel> qSET1 = new List<A3MMModel>();

            var questionList1 = (from a in db.EngagementQuestionnaire
                                 where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionIDinitial
                                 select a).ToList();
            var questionList = (from a in db.EngagementQuestionnaire
                                join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionIDinitial && b.AssessmentID == AssessmentIDinitial
                                select new Qset
                                {
                                    SQNo = a.SQNo,
                                    Questionorder = a.QuestionOrder.Value,
                                    QuestionStatus = b.responseStatus,
                                    AssessmentId = b.AssessmentID
                                }).ToList();

            ret1.UnAnsQset = questionList.Where(x => x.QuestionStatus == "Unanswered").OrderBy(o => o.Questionorder).ToList();
            ret1.ResponseInProgressQset = questionList.Where(x => x.QuestionStatus == "ResponseInProgress").OrderBy(o => o.Questionorder).ToList();
            ret1.RespondedQset = questionList.Where(x => x.QuestionStatus == "Responded").OrderBy(o => o.Questionorder).ToList();
            ret1.RatingInProgressQset = questionList.Where(x => x.QuestionStatus == "RatingInProgress").OrderBy(o => o.Questionorder).ToList();
            //   ret1.IncompleteQset = questionList.Where(x => x.QuestionStatus == "Incompleted").OrderBy(o => o.Questionorder).ToList();
            ret1.CompleteQset = questionList.Where(x => x.QuestionStatus == "Completed").OrderBy(o => o.Questionorder).ToList();

            var tempsqno = "";
            if (ViewName == "AssTeamView")
            {
                if (ret1.RespondedQset.Count() != 0)
                {
                    tempsqno = ret1.RespondedQset[0].SQNo;
                }
                else if(ret1.RatingInProgressQset.Count() != 0)
                {
                    tempsqno = ret1.RatingInProgressQset[0].SQNo;
                }
                else if(ret1.UnAnsQset.Count() !=0)
                {
                    tempsqno = ret1.UnAnsQset[0].SQNo;

                }
                else if (ret1.ResponseInProgressQset.Count() != 0)
                {
                    tempsqno = ret1.ResponseInProgressQset[0].SQNo;
                }
                else
                {
                    tempsqno = ret1.CompleteQset[0].SQNo;
                }
            }
            else
            {
                if (ret1.UnAnsQset.Count() != 0)
                {
                    tempsqno = ret1.UnAnsQset[0].SQNo;
                }
                else if (ret1.ResponseInProgressQset.Count() != 0)
                {
                    tempsqno = ret1.ResponseInProgressQset[0].SQNo;
                }
                else if (ret1.RespondedQset.Count() != 0)
                {
                    tempsqno = ret1.RespondedQset[0].SQNo;
                }
                else if (ret1.RatingInProgressQset.Count() != 0)
                {
                    tempsqno = ret1.RatingInProgressQset[0].SQNo;
                }
                else
                {
                    tempsqno = ret1.CompleteQset[0].SQNo;
                }
            }
            var responseList1 = (from a in db.QuestionnaireResponse
                                 where a.SurveyTypeId == surveyTypeIDinitial && a.AssessmentID == AssessmentIDinitial && a.VersionID == versionIDinitial && a.ClientID == Cl.ClientId
                                 && a.SQNo == tempsqno
                                 select a).ToList().FirstOrDefault();




            EngagementQuestionnaire e = (from b in questionList1
                                         where b.SQNo == tempsqno && b.SurveyTypeID == responseList1.SurveyTypeId && b.VersionID == versionIDinitial
                                         select b).FirstOrDefault();
            List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                       where x.QuestionRespId == responseList1.Id
                                                       select new ResponseAttachment
                                                       {
                                                           Id = x.Id,
                                                           filename = x.FileName,
                                                           fileCont = x.AttachFile
                                                       }).ToList();
            List<MultipleResponses> Response = (from x in db.A3MM_MultipleResponses
                                                where x.AssessmentId == AssessmentIDinitial && responseList1.Id == x.ResponseID
                                                select new MultipleResponses
                                                {
                                                    Id = x.Id,
                                                    ResponseID = x.ResponseID,
                                                    ResponseNumber = x.ResponseNumber,
                                                    SubResponse = x.SubResponse,
                                                    ClientID = x.ClientID,
                                                    AssessmentId = x.AssessmentId,
                                                    SubResponseOrder = x.SubResponseOrder,
                                                    SQNo = x.SQNo
                                                }).OrderBy(x => x.Id).ToList();
            List<MultipleFollowUpQuestions> followup = (from x in db.A3MM_MultipleFollowUpQuestions
                                                        where x.AssessmentId == AssessmentIDinitial && x.ResponseID == responseList1.Id
                                                        select new MultipleFollowUpQuestions
                                                        {
                                                            Id = x.Id,
                                                            ResponseID = x.ResponseID,
                                                            FollowUpQNumber = x.FollowUpQNumber,
                                                            SubFollowUpQuestion = x.SubFollowUpQuestion,
                                                            ClientID = x.ClientID,
                                                            SubFollowUpQOrder = x.SubFollowUpQOrder,
                                                            AssessmentId = x.AssessmentId,
                                                            SubResponseId = x.SubResponseId,
                                                        }).ToList();

            if (e != null)
            {

                ret1.QNo = responseList1.QNo;
                ret1.SurveyTypeID = e.SurveyTypeID;
                ret1.AssessmentID = responseList1.AssessmentID;
                ret1.DisciplineId = System.Convert.ToInt32(e.Discipline_ID);
                ret1.DisciplineName = e.DisciplineName;
                ret1.ComponentId = System.Convert.ToInt32(e.Component_ID);
                ret1.ComponentName = e.ComponentName;
                ret1.AspectID = e.AspectID;
                ret1.AspectName = e.AspectName;
                ret1.Question = e.Question;
                ret1.ExampleAnswer = e.ExampleAns;
                ret1.Response = responseList1.Response;
                ret1.AssessmentResponse = responseList1.AssessmentResponse;
                ret1.Feedback = responseList1.Feedback;
                ret1.FollowupQuestion = responseList1.FollowUpQuestion;
                ret1.MaturityLevel = responseList1.MaturityLevel;
                ret1.AreasofImprovement = responseList1.AreaOfImprovement;
                ret1.QuestionId = e.Id;
                ret1.ClientId = responseList1.ClientID;
                ret1.QuestionResponseID = responseList1.Id;
                ret1.LastFeedbackBy = System.Convert.ToInt32(responseList1.LastFeedBackBy);
                ret1.LastFeedbackOn = responseList1.LastFeedBackOn;
                ret1.LastUpdatedBy = System.Convert.ToInt32(responseList1.LastUpdatedBy);
                ret1.LastUpdatedOn = responseList1.LastUpdatedOn;
                ret1.VersionID = responseList1.VersionID;
                ret1.DatasourceId = e.DataSourceID;
                ret1.SQNo = e.SQNo;
                ret1.SubQOrder = e.SubQOrder;
                ret1.QuestionOrder = e.QuestionOrder;
                ret1.RespAttachment = RespAttachment;
                ret1.MultipleResponses = Response;
                ret1.MultipleFollowUpQuestions = followup;
                ret1.EngagementRating = responseList1.EngagementRating;
                ret1.engagementComments = responseList1.EngagementComments;
                ret1.responseStatus = responseList1.responseStatus;
                ret1.Status = AssessmentValue.Status;

            }

            return ret1;
        }
        [HttpPost]
        [ActionTrack]

        private A3MMModel GetDataGatheringDetailForAssessmentOverview(A3MMModel ret1, string SQNo = "")
        {
            var UserId = (int)Session["UserId"];
            var Cl = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var AssessmentIDParam = ret1.AssessmentID;
            var QNoParam = ret1.SQNo;
            ret1 = prepareCommonParameters();

            var AssessmentValue = new Assessment();


            AssessmentValue = (from a in db.Assessment
                               where a.Id == AssessmentIDParam && a.ClientId == Cl.ClientId
                               select a).FirstOrDefault();




            int AssessmentIDinitial = AssessmentValue.Id;
            var surveyTypeIDinitial = AssessmentValue.SurveyTypeID;
            var versionIDinitial = AssessmentValue.QuestionaireVersion;
            ret1.AssessmentName = AssessmentValue.AssessmentName;



            List<A3MMModel> qSET1 = new List<A3MMModel>();

            var questionList1 = (from a in db.EngagementQuestionnaire
                                 where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionIDinitial
                                 select a).ToList();

            var questionList = (from a in db.EngagementQuestionnaire
                                join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionIDinitial && b.AssessmentID == AssessmentIDinitial
                                select new Qset
                                {
                                    SQNo = a.SQNo,
                                    Questionorder = a.QuestionOrder.Value,
                                    QuestionStatus = b.responseStatus,
                                    AssessmentId = b.AssessmentID
                                }).ToList();

            ret1.UnAnsQset = questionList.Where(x => x.QuestionStatus == "Unanswered").OrderBy(o => o.Questionorder).ToList();
            ret1.ResponseInProgressQset = questionList.Where(x => x.QuestionStatus == "ResponseInProgress").OrderBy(o => o.Questionorder).ToList();
            ret1.RespondedQset = questionList.Where(x => x.QuestionStatus == "Responded").OrderBy(o => o.Questionorder).ToList();
            ret1.RatingInProgressQset = questionList.Where(x => x.QuestionStatus == "RatingInProgress").OrderBy(o => o.Questionorder).ToList();
            //   ret1.IncompleteQset = questionList.Where(x => x.QuestionStatus == "Incompleted").OrderBy(o => o.Questionorder).ToList();
            ret1.CompleteQset = questionList.Where(x => x.QuestionStatus == "Completed").OrderBy(o => o.Questionorder).ToList();

            var tempsqno = SQNo;




            var responseList1 = (from a in db.QuestionnaireResponse
                                 where a.SurveyTypeId == surveyTypeIDinitial && a.AssessmentID == AssessmentIDinitial && a.VersionID == versionIDinitial && a.ClientID == Cl.ClientId
                                 && a.SQNo == tempsqno
                                 select a).ToList().FirstOrDefault();




            EngagementQuestionnaire e = (from b in questionList1
                                         where b.SQNo == tempsqno && b.SurveyTypeID == responseList1.SurveyTypeId && b.VersionID == versionIDinitial
                                         select b).FirstOrDefault();
            List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                       where x.QuestionRespId == responseList1.Id
                                                       select new ResponseAttachment
                                                       {
                                                           Id = x.Id,
                                                           filename = x.FileName,
                                                           fileCont = x.AttachFile
                                                       }).ToList();
            List<MultipleResponses> Response = (from x in db.A3MM_MultipleResponses
                                                where x.AssessmentId == AssessmentIDinitial && responseList1.Id == x.ResponseID
                                                select new MultipleResponses
                                                {
                                                    Id = x.Id,
                                                    ResponseID = x.ResponseID,
                                                    ResponseNumber = x.ResponseNumber,
                                                    SubResponse = x.SubResponse,
                                                    ClientID = x.ClientID,
                                                    AssessmentId = x.AssessmentId,
                                                    SubResponseOrder = x.SubResponseOrder,
                                                    SQNo = x.SQNo
                                                }).OrderBy(x => x.Id).ToList();
            List<MultipleFollowUpQuestions> followup = (from x in db.A3MM_MultipleFollowUpQuestions
                                                        where x.AssessmentId == AssessmentIDinitial && x.ResponseID == responseList1.Id
                                                        select new MultipleFollowUpQuestions
                                                        {
                                                            Id = x.Id,
                                                            ResponseID = x.ResponseID,
                                                            FollowUpQNumber = x.FollowUpQNumber,
                                                            SubFollowUpQuestion = x.SubFollowUpQuestion,
                                                            ClientID = x.ClientID,
                                                            SubFollowUpQOrder = x.SubFollowUpQOrder,
                                                            AssessmentId = x.AssessmentId,
                                                            SubResponseId = x.SubResponseId,
                                                        }).ToList();

            if (e != null)
            {

                ret1.QNo = responseList1.QNo;
                ret1.SurveyTypeID = e.SurveyTypeID;
                ret1.AssessmentID = responseList1.AssessmentID;
                ret1.DisciplineId = System.Convert.ToInt32(e.Discipline_ID);
                ret1.DisciplineName = e.DisciplineName;
                ret1.ComponentId = System.Convert.ToInt32(e.Component_ID);
                ret1.ComponentName = e.ComponentName;
                ret1.AspectID = e.AspectID;
                ret1.AspectName = e.AspectName;
                ret1.Question = e.Question;
                ret1.ExampleAnswer = e.ExampleAns;
                ret1.Response = responseList1.Response;
                ret1.AssessmentResponse = responseList1.AssessmentResponse;
                ret1.Feedback = responseList1.Feedback;
                ret1.FollowupQuestion = responseList1.FollowUpQuestion;
                ret1.MaturityLevel = responseList1.MaturityLevel;
                ret1.AreasofImprovement = responseList1.AreaOfImprovement;
                ret1.QuestionId = e.Id;
                ret1.ClientId = responseList1.ClientID;
                ret1.QuestionResponseID = responseList1.Id;
                ret1.LastFeedbackBy = System.Convert.ToInt32(responseList1.LastFeedBackBy);
                ret1.LastFeedbackOn = responseList1.LastFeedBackOn;
                ret1.LastUpdatedBy = System.Convert.ToInt32(responseList1.LastUpdatedBy);
                ret1.LastUpdatedOn = responseList1.LastUpdatedOn;
                ret1.VersionID = responseList1.VersionID;
                ret1.DatasourceId = e.DataSourceID;
                ret1.SQNo = e.SQNo;
                ret1.SubQOrder = e.SubQOrder;
                ret1.QuestionOrder = e.QuestionOrder;
                ret1.RespAttachment = RespAttachment;
                ret1.MultipleResponses = Response;
                ret1.MultipleFollowUpQuestions = followup;
                ret1.EngagementRating = responseList1.EngagementRating;
                ret1.engagementComments = responseList1.EngagementComments;
                ret1.responseStatus = responseList1.responseStatus;
                ret1.Status = AssessmentValue.Status;

            }






            return ret1;
        }


        public ActionResult SaveEngagementResponse(int AssessmentID, string SQNo, string Response,int DisciplineId,int ComponentId,int AspectID, int isdiscChanged,int iscompChanged,int isAspChanged)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            int ClientId = client.ClientId;
            var QuestionResponseID = (from a in db.QuestionnaireResponse
                                      where a.AssessmentID == AssessmentID && a.SQNo == SQNo && a.ClientID == ClientId
                                      select a).ToList().FirstOrDefault();
            var res = AssessmentManagerHelper.saveEngagementResponse(QuestionResponseID.Id, SQNo, Response, usid, AssessmentID);
            A3MMModel ret = new A3MMModel(); 
            if (QuestionResponseID.responseStatus== "Unanswered")
            {
                List<Qset> questionList1 = new List<Qset>();
                if (isAspChanged == 1 && AspectID > 0)
                {
                    questionList1 = (from a in db.EngagementQuestionnaire
                                    join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                     where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == AssessmentID
                                     && a.AspectID == AspectID
                                     select new Qset
                                     {
                                         SQNo = a.SQNo,
                                         Questionorder = a.QuestionOrder.Value,
                                         QuestionStatus = b.responseStatus,
                                         AssessmentId = b.AssessmentID
                                     }).ToList();
                }
                else if (iscompChanged == 1 && ComponentId > 0)
                {
                    questionList1 = (from a in db.EngagementQuestionnaire
                                     join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                     where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == AssessmentID
                                     && a.Component_ID == ComponentId
                                     select new Qset
                                     {
                                         SQNo = a.SQNo,
                                         Questionorder = a.QuestionOrder.Value,
                                         QuestionStatus = b.responseStatus,
                                         AssessmentId = b.AssessmentID
                                     }).ToList();
                }
                else if (isdiscChanged == 1 && DisciplineId > 0)
                {
                    questionList1 = (from a in db.EngagementQuestionnaire
                                     join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                     where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == AssessmentID
                                     && a.Discipline_ID == DisciplineId
                                     select new Qset
                                     {
                                         SQNo = a.SQNo,
                                         Questionorder = a.QuestionOrder.Value,
                                         QuestionStatus = b.responseStatus,
                                         AssessmentId = b.AssessmentID
                                     }).ToList();
                }
                else
                {
                    questionList1 = (from a in db.EngagementQuestionnaire
                                     join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                     where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == AssessmentID
                                     select new Qset
                                     {
                                         SQNo = a.SQNo,
                                         Questionorder = a.QuestionOrder.Value,
                                         QuestionStatus = b.responseStatus,
                                         AssessmentId = b.AssessmentID
                                     }).ToList();
                }

                ret.UnAnsQset = questionList1.Where(x => x.QuestionStatus == "Unanswered").OrderBy(o => o.Questionorder).ToList();
                ret.ResponseInProgressQset = questionList1.Where(x => x.QuestionStatus == "ResponseInProgress").OrderBy(o => o.Questionorder).ToList();
                ret.RespondedQset = questionList1.Where(x => x.QuestionStatus == "Responded").OrderBy(o => o.Questionorder).ToList();
                ret.RatingInProgressQset = questionList1.Where(x => x.QuestionStatus == "RatingInProgress").OrderBy(o => o.Questionorder).ToList();
                //   ret1.IncompleteQset = questionList.Where(x => x.QuestionStatus == "Incompleted").OrderBy(o => o.Questionorder).ToList();
                ret.CompleteQset = questionList1.Where(x => x.QuestionStatus == "Completed").OrderBy(o => o.Questionorder).ToList();

                ret.SurveyTypeID = QuestionResponseID.SurveyTypeId;
                ret.currentresId= res;
                return PartialView("_QuestionNavigation", ret);
            }
            else {
                return Json(res, JsonRequestBehavior.AllowGet); }

        } 
         [HttpPost]
        [ActionTrack]
        public ActionResult SaveFollowupQuestion(int ResponseID,int subresponseid,int AssessmentId,string followup, int DisciplineId, int ComponentId, int AspectID, int isdiscChanged, int iscompChanged, int isAspChanged)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            int ClientId = client.ClientId;

            var res = AssessmentManagerHelper.saveFollowupQuestion(ResponseID, subresponseid, AssessmentId, followup);
            A3MMModel ret = new A3MMModel();
            var QuestionResponseID = (from a in db.QuestionnaireResponse
                                      where a.Id == ResponseID && a.ClientID == ClientId
                                      select a).ToList().FirstOrDefault();
            List<Qset> questionList1 = new List<Qset>();
                if (isAspChanged == 1 && AspectID > 0)
                {
                    questionList1 = (from a in db.EngagementQuestionnaire
                                     join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                     where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == AssessmentId
                                     && a.AspectID == AspectID
                                     select new Qset
                                     {
                                         SQNo = a.SQNo,
                                         Questionorder = a.QuestionOrder.Value,
                                         QuestionStatus = b.responseStatus,
                                         AssessmentId = b.AssessmentID
                                     }).ToList();
                }
                else if (iscompChanged == 1 && ComponentId > 0)
                {
                    questionList1 = (from a in db.EngagementQuestionnaire
                                     join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                     where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == AssessmentId
                                     && a.Component_ID == ComponentId
                                     select new Qset
                                     {
                                         SQNo = a.SQNo,
                                         Questionorder = a.QuestionOrder.Value,
                                         QuestionStatus = b.responseStatus,
                                         AssessmentId = b.AssessmentID
                                     }).ToList();
                }
                else if (isdiscChanged == 1 && DisciplineId > 0)
                {
                    questionList1 = (from a in db.EngagementQuestionnaire
                                     join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                     where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == AssessmentId
                                     && a.Discipline_ID == DisciplineId
                                     select new Qset
                                     {
                                         SQNo = a.SQNo,
                                         Questionorder = a.QuestionOrder.Value,
                                         QuestionStatus = b.responseStatus,
                                         AssessmentId = b.AssessmentID
                                     }).ToList();
                }
                else
                {
                    questionList1 = (from a in db.EngagementQuestionnaire
                                     join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                     where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == AssessmentId
                                     select new Qset
                                     {
                                         SQNo = a.SQNo,
                                         Questionorder = a.QuestionOrder.Value,
                                         QuestionStatus = b.responseStatus,
                                         AssessmentId = b.AssessmentID
                                     }).ToList();
                }

            ret.UnAnsQset = questionList1.Where(x => x.QuestionStatus == "Unanswered").OrderBy(o => o.Questionorder).ToList();
            ret.ResponseInProgressQset = questionList1.Where(x => x.QuestionStatus == "ResponseInProgress").OrderBy(o => o.Questionorder).ToList();
            ret.RespondedQset = questionList1.Where(x => x.QuestionStatus == "Responded").OrderBy(o => o.Questionorder).ToList();
            ret.RatingInProgressQset = questionList1.Where(x => x.QuestionStatus == "RatingInProgress").OrderBy(o => o.Questionorder).ToList();
            //   ret1.IncompleteQset = questionList.Where(x => x.QuestionStatus == "Incompleted").OrderBy(o => o.Questionorder).ToList();
            ret.CompleteQset = questionList1.Where(x => x.QuestionStatus == "Completed").OrderBy(o => o.Questionorder).ToList();
            ret.SurveyTypeID = QuestionResponseID.SurveyTypeId;
                ret.currentfollowupId = res;
                return PartialView("_QuestionNavigation", ret);
          
         
        } 
        
        [HttpPost]
        [ActionTrack]
        public ActionResult UpdateEngagementResponse(int Id, string Response)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            int ClientId = client.ClientId;
            var res = AssessmentManagerHelper.UpdateEngagementResponse(Id, Response, usid);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionTrack]
        public ActionResult UpdateFollowup(int Id, string FollowVal)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            int ClientId = client.ClientId;
            var res = AssessmentManagerHelper.UpdateFollowupResponse(Id, FollowVal, usid);
            return Json(res, JsonRequestBehavior.AllowGet);
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
            List<Qset> questionList1 = new List<Qset>();

            if (mod.isAspChanged == 1 && mod.AspectID > 0)
            {
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID && a.AspectID == mod.AspectID
                                select a).ToList();
               questionList1 = (from a in db.EngagementQuestionnaire
                                     join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                     where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID && a.AspectID == mod.AspectID && b.AssessmentID == assessID
                                select new Qset
                                     {
                                         SQNo = a.SQNo,
                                         Questionorder = a.QuestionOrder.Value,
                                         QuestionStatus = b.responseStatus,
                                         AssessmentId = b.AssessmentID
                                     }).ToList();
            }
            else if (mod.iscompChanged == 1 && mod.ComponentId > 0)
            {
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID 
                                && a.Component_ID == mod.ComponentId
                                select a).ToList();
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID && a.Component_ID == mod.ComponentId && b.AssessmentID == assessID
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            else if (mod.isdiscChanged == 1 && mod.DisciplineId > 0)
            {
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID && a.Discipline_ID == mod.DisciplineId
                                select a).ToList();
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID && a.Discipline_ID == mod.DisciplineId && b.AssessmentID == assessID
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            else
            {
                questionList = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID
                                select a).ToList();
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == surveyTypeIDinitial && a.VersionID == versionID && b.AssessmentID == assessID
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            // var firstQrecord = questionList[0].SQNo;
            ret.UnAnsQset = questionList1.Where(x => x.QuestionStatus == "Unanswered").OrderBy(o => o.Questionorder).ToList();
            ret.ResponseInProgressQset = questionList1.Where(x => x.QuestionStatus == "ResponseInProgress").OrderBy(o => o.Questionorder).ToList();
            ret.RespondedQset = questionList1.Where(x => x.QuestionStatus == "Responded").OrderBy(o => o.Questionorder).ToList();
            ret.RatingInProgressQset = questionList1.Where(x => x.QuestionStatus == "RatingInProgress").OrderBy(o => o.Questionorder).ToList();
            //   ret1.IncompleteQset = questionList.Where(x => x.QuestionStatus == "Incompleted").OrderBy(o => o.Questionorder).ToList();
            ret.CompleteQset = questionList1.Where(x => x.QuestionStatus == "Completed").OrderBy(o => o.Questionorder).ToList();
            var tempsqno = "";
            if (ret.UnAnsQset.Count() != 0)
            {
                tempsqno = ret.UnAnsQset[0].SQNo;
            }
            else if (ret.ResponseInProgressQset.Count() != 0)
            {
                tempsqno = ret.ResponseInProgressQset[0].SQNo;
            }
            else if (ret.RespondedQset.Count() != 0)
            {
                tempsqno = ret.RespondedQset[0].SQNo;
            }
            else if (ret.RatingInProgressQset.Count() != 0)
            {
                tempsqno = ret.RatingInProgressQset[0].SQNo;
            }
            else
            {
                tempsqno = ret.CompleteQset[0].SQNo;
            }

            var responseList = (from a in db.QuestionnaireResponse
                                where a.SurveyTypeId == surveyTypeIDinitial 
                                && a.AssessmentID == AssessmentIDinitial && a.VersionID == versionID && a.ClientID == Cl.ClientId
                                && a.SQNo == tempsqno
                                select a).ToList().FirstOrDefault();
                EngagementQuestionnaire e = (from b in questionList
                                             where b.SQNo == responseList.SQNo && b.SurveyTypeID == responseList.SurveyTypeId
                                             select b).FirstOrDefault();
                List<ResponseAttachment> RespAttachment = (from x in db.QuestRespAttachments
                                                           where x.QuestionRespId == responseList.Id
                                                           select new ResponseAttachment
                                                           {
                                                               Id = x.Id,
                                                               filename = x.FileName,
                                                               fileCont = x.AttachFile
                                                           }).ToList();
            List<MultipleResponses> Response = (from x in db.A3MM_MultipleResponses
                                                where x.AssessmentId == AssessmentIDinitial && responseList.Id == x.ResponseID
                                                select new MultipleResponses
                                                {
                                                    Id = x.Id,
                                                    ResponseID = x.ResponseID,
                                                    ResponseNumber = x.ResponseNumber,
                                                    SubResponse = x.SubResponse,
                                                    ClientID = x.ClientID,
                                                    AssessmentId = x.AssessmentId,
                                                    SubResponseOrder = x.SubResponseOrder,
                                                    SQNo = x.SQNo
                                                }).OrderBy(x=>x.Id).ToList();
            List<MultipleFollowUpQuestions> followup = (from x in db.A3MM_MultipleFollowUpQuestions
                                                        where x.AssessmentId == AssessmentIDinitial && x.ResponseID == responseList.Id
                                                        select new MultipleFollowUpQuestions
                                                        {
                                                            Id = x.Id,
                                                            ResponseID = x.ResponseID,
                                                            FollowUpQNumber = x.FollowUpQNumber,
                                                            SubFollowUpQuestion = x.SubFollowUpQuestion,
                                                            ClientID = x.ClientID,
                                                             SubFollowUpQOrder = x.SubFollowUpQOrder,
                                                            AssessmentId = x.AssessmentId,
                                                            SubResponseId = x.SubResponseId,
                                                        }).ToList();


            if (e != null)
                {
                ret.QNo = e.QNo;
                ret.SurveyTypeID = responseList.SurveyTypeId;
                ret.AssessmentID = responseList.AssessmentID;
                ret.DisciplineId = Convert.ToInt32(e.Discipline_ID);
                ret.DisciplineName = e.DisciplineName;
                ret.ComponentId = Convert.ToInt32(e.Component_ID);
                ret.ComponentName = e.ComponentName;
                ret.AspectID = e.AspectID;
                ret.AspectName = e.AspectName;
                ret.Question = e.Question;
                ret.ExampleAnswer = e.ExampleAns;
                ret.Response = responseList.Response;
                ret.AssessmentResponse = responseList.AssessmentResponse;
                ret.FollowupQuestion = responseList.FollowUpQuestion;
                ret.MaturityLevel = responseList.MaturityLevel;
                ret.AreasofImprovement = responseList.AreaOfImprovement;
                ret.QuestionId = e.Id;
                ret.ClientId = responseList.ClientID;
                ret.QuestionResponseID = responseList.Id;
                ret.LastFeedbackBy = Convert.ToInt32(responseList.LastFeedBackBy);
                ret.LastFeedbackOn = responseList.LastFeedBackOn;
                ret.LastUpdatedBy = Convert.ToInt32(responseList.LastUpdatedBy);
                ret.LastUpdatedOn = responseList.LastUpdatedOn;
                ret.VersionID = responseList.VersionID;
                ret.DatasourceId = e.DataSourceID;
                ret.SQNo = e.SQNo;
                ret.SubQOrder = e.SubQOrder;
                ret.QuestionOrder = e.QuestionOrder;
                ret.RespAttachment = RespAttachment;
                ret.MultipleResponses = Response;
                ret.MultipleFollowUpQuestions = followup;
                ret.EngagementRating = responseList.EngagementRating;
                ret.engagementComments = responseList.EngagementComments;
            }
            return PartialView("_QuestionNavigation", ret);


          //  return Json(ret, JsonRequestBehavior.AllowGet);

        }       
     
        [HttpPost]
        public ActionResult GetNextQuestionforFollowup(int ResponseId,string grpName="") 
        {
            A3MMModel ret = new A3MMModel();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            var responsedetail = unitOfWork.QuestionnaireResponse.GetByID(ResponseId);// current Sqno
            var nextQuestion= (from a in db.QuestionnaireResponse
                              where a.Id> responsedetail.Id && a.SurveyTypeId== responsedetail.SurveyTypeId 
                              && a.VersionID== responsedetail.VersionID && a.responseStatus== grpName
                               && a.AssessmentID == responsedetail.AssessmentID
                               select a).OrderBy(a => a.Id).ToList().FirstOrDefault();
            if (nextQuestion !=null)
            {
                ret = GetAssessmentQDetail(nextQuestion.AssessmentID.ToString(), nextQuestion.SQNo);
            }else
            {
                ret = GetAssessmentQDetail(responsedetail.AssessmentID.ToString(), responsedetail.SQNo);
            }
           
            return PartialView("_AssessmentQuestionDetail", ret);

        }
        [HttpPost]
        public ActionResult GetNextQuestionforResponse(int ResponseId,string grpName="") 
        {
            A3MMModel ret = new A3MMModel();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            var responsedetail = unitOfWork.QuestionnaireResponse.GetByID(ResponseId);// current Sqno
            var nextQuestion= (from a in db.QuestionnaireResponse
                              where a.Id> responsedetail.Id && a.SurveyTypeId== responsedetail.SurveyTypeId 
                              && a.VersionID== responsedetail.VersionID && a.responseStatus== grpName
                               && a.AssessmentID == responsedetail.AssessmentID
                               select a).OrderBy(a => a.Id).ToList().FirstOrDefault();
            if (nextQuestion !=null)
            {
                ret = GetAssessmentQDetail(nextQuestion.AssessmentID.ToString(), nextQuestion.SQNo);
            }else
            {
                ret = GetAssessmentQDetail(responsedetail.AssessmentID.ToString(), responsedetail.SQNo);
            }
           
            return PartialView("_QuestionDetail", ret);

        }
        [HttpPost]
        public ActionResult GetPageClickAssessmentDetail(string AssessmentID, string SQNo) 
        {
            A3MMModel ret = new A3MMModel();
            ret = GetAssessmentQDetail(AssessmentID, SQNo);
            return PartialView("_QuestionDetail", ret);

        }
        
        [HttpPost]
        public ActionResult GetPageClickAssessmentDetailforFollowup(string AssessmentID, string SQNo) //need change
        {
            A3MMModel ret = new A3MMModel();
            ret = GetAssessmentQDetail(AssessmentID, SQNo);
            return PartialView("_AssessmentQuestionDetail", ret);

        }
        [HttpPost]
        public ActionResult GetPageClickAssessmentDetailforResponseView(string AssessmentID, string SQNo) //need change
        {
            A3MMModel ret = new A3MMModel();
            ret = GetAssessmentQDetail(AssessmentID, SQNo);
            return PartialView("_AssessmentViewQuestionDetail", ret);

        }
        private A3MMModel GetAssessmentQDetail(string AssessmentID,string SQNo)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int assessID = Convert.ToInt32(AssessmentID); //Id of Assessment Table
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            int surveyTypeID = new int();
            // A3MMModel a3 = new A3MMModel();
            surveyTypeID = (from a in db.Assessment
                            where a.Id == assessID
                            select a.SurveyTypeID).FirstOrDefault();

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
            List<MultipleResponses> Response = (from x in db.A3MM_MultipleResponses
                                                where x.AssessmentId == assessID && responseRecord.Id == x.ResponseID
                                                select new MultipleResponses
                                                {
                                                    Id = x.Id,
                                                    ResponseID = x.ResponseID,
                                                    ResponseNumber = x.ResponseNumber,
                                                    SubResponse = x.SubResponse,
                                                    ClientID = x.ClientID,
                                                    AssessmentId = x.AssessmentId,
                                                    SubResponseOrder = x.SubResponseOrder,
                                                    SQNo = x.SQNo
                                                }).OrderBy(x => x.Id).ToList();
            List<MultipleFollowUpQuestions> followup = (from x in db.A3MM_MultipleFollowUpQuestions
                                                        where x.AssessmentId == assessID && x.ResponseID == responseRecord.Id
                                                        select new MultipleFollowUpQuestions
                                                        {
                                                            Id = x.Id,
                                                            ResponseID = x.ResponseID,
                                                            FollowUpQNumber = x.FollowUpQNumber,
                                                            SubFollowUpQuestion = x.SubFollowUpQuestion,
                                                            ClientID = x.ClientID,
                                                            SubFollowUpQOrder = x.SubFollowUpQOrder,
                                                            AssessmentId = x.AssessmentId,
                                                            SubResponseId = x.SubResponseId,
                                                        }).ToList();

           

            A3MMModel ret = prepareCommonParameters();
            if (followup.Count + 1 > Response.Count)
            {
                ret.boolFollowResponseCount = true;
            }
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
            ret.MultipleResponses = Response;
            ret.MultipleFollowUpQuestions = followup;
            ret.EngagementRating = responseRecord.EngagementRating;
            ret.engagementComments = responseRecord.EngagementComments;
            ret.QuestionResponseID = responseRecord.Id;
            ret.responseStatus = responseRecord.responseStatus;
            ret.AssessmentID = responseRecord.AssessmentID;
            return ret;
        }

        public ActionResult SaveEngagementSelfAssessment(int responseID, string rating, string comments, bool markAsCompletecheckBox,int DisciplineId, int ComponentId, int AspectID, int isdiscChanged, int iscompChanged, int isAspChanged)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            int ClientId = client.ClientId;

            var res = AssessmentManagerHelper.saveEngagementSelfAssessment(responseID, rating, comments, markAsCompletecheckBox);
            A3MMModel ret1 = new A3MMModel();
            var QuestionResponseID = (from a in db.QuestionnaireResponse
                            where a.Id == responseID select a).FirstOrDefault();

            List<Qset> questionList1 = new List<Qset>();
            if (isAspChanged == 1 && AspectID > 0)
            {
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == QuestionResponseID.AssessmentID
                                 && a.AspectID == AspectID
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            else if (iscompChanged == 1 && ComponentId > 0)
            {
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == QuestionResponseID.AssessmentID
                                 && a.Component_ID == ComponentId
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            else if (isdiscChanged == 1 && DisciplineId > 0)
            {
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == QuestionResponseID.AssessmentID
                                 && a.Discipline_ID == DisciplineId
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            else
            {
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == QuestionResponseID.AssessmentID
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            ret1.UnAnsQset = questionList1.Where(x => x.QuestionStatus == "Unanswered").OrderBy(o => o.Questionorder).ToList();
            ret1.ResponseInProgressQset = questionList1.Where(x => x.QuestionStatus == "ResponseInProgress").OrderBy(o => o.Questionorder).ToList();
            ret1.RespondedQset = questionList1.Where(x => x.QuestionStatus == "Responded").OrderBy(o => o.Questionorder).ToList();
            ret1.RatingInProgressQset = questionList1.Where(x => x.QuestionStatus == "RatingInProgress").OrderBy(o => o.Questionorder).ToList();
            //   ret1.IncompleteQset = questionList.Where(x => x.QuestionStatus == "Incompleted").OrderBy(o => o.Questionorder).ToList();
            ret1.CompleteQset = questionList1.Where(x => x.QuestionStatus == "Completed").OrderBy(o => o.Questionorder).ToList();

            ret1.SurveyTypeID = QuestionResponseID.SurveyTypeId;
            return PartialView("_QuestionNavigation", ret1);

        }

        //Assessment UI
        public ActionResult SaveAssessmentRating(int responseID, string rating, string AssessmentResponse, string AreasOfImprovement,bool MarkAsCompletecheckBox, int impPlanID, int busDriverID, int priority, int amsCatalogID, string Status,
            int DisciplineId, int ComponentId, int AspectID, int isdiscChanged, int iscompChanged, int isAspChanged)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            int ClientId = client.ClientId;

            var responseRecord = (from a in db.QuestionnaireResponse
                                where a.Id == responseID
                                select a).ToList().FirstOrDefault();


            var disciplineID = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == responseRecord.SurveyTypeId && a.VersionID == responseRecord.VersionID && a.SQNo == responseRecord.SQNo
                                select a.Discipline_ID).ToList().FirstOrDefault();

            var componentID = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == responseRecord.SurveyTypeId && a.VersionID == responseRecord.VersionID && a.SQNo == responseRecord.SQNo
                                select a.Component_ID).ToList().FirstOrDefault();

            var aspectID = (from a in db.EngagementQuestionnaire
                                where a.SurveyTypeID == responseRecord.SurveyTypeId && a.VersionID == responseRecord.VersionID && a.SQNo == responseRecord.SQNo
                                select a.AspectID).ToList().FirstOrDefault();


            var ActionItemID = (from a in db.ActionItemTracker
                                where a.QResID == responseID
                                select a.Id).ToList().FirstOrDefault();

            var res = AssessmentManagerHelper.saveAssessRating(responseID, rating, AssessmentResponse, AreasOfImprovement, impPlanID, MarkAsCompletecheckBox,  busDriverID,  priority,  amsCatalogID,  Status,ClientId, disciplineID,componentID,aspectID, ActionItemID);


           

            A3MMModel ret1 = new A3MMModel();
            var QuestionResponseID = (from a in db.QuestionnaireResponse
                            where a.Id == responseID
                            select a).FirstOrDefault();
           
            List<Qset> questionList1 = new List<Qset>();
            if (isAspChanged == 1 && AspectID > 0)
            {
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == QuestionResponseID.AssessmentID
                                 && a.AspectID == AspectID
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            else if (iscompChanged == 1 && ComponentId > 0)
            {
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == QuestionResponseID.AssessmentID
                                 && a.Component_ID == ComponentId
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            else if (isdiscChanged == 1 && DisciplineId > 0)
            {
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == QuestionResponseID.AssessmentID
                                 && a.Discipline_ID == DisciplineId
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            else
            {
                questionList1 = (from a in db.EngagementQuestionnaire
                                 join b in db.QuestionnaireResponse on a.SQNo equals b.SQNo
                                 where a.SurveyTypeID == QuestionResponseID.SurveyTypeId && a.VersionID == QuestionResponseID.VersionID && b.AssessmentID == QuestionResponseID.AssessmentID
                                 select new Qset
                                 {
                                     SQNo = a.SQNo,
                                     Questionorder = a.QuestionOrder.Value,
                                     QuestionStatus = b.responseStatus,
                                     AssessmentId = b.AssessmentID
                                 }).ToList();
            }
            ret1.UnAnsQset = questionList1.Where(x => x.QuestionStatus == "Unanswered").OrderBy(o => o.Questionorder).ToList();
            ret1.ResponseInProgressQset = questionList1.Where(x => x.QuestionStatus == "ResponseInProgress").OrderBy(o => o.Questionorder).ToList();
            ret1.RespondedQset = questionList1.Where(x => x.QuestionStatus == "Responded").OrderBy(o => o.Questionorder).ToList();
            ret1.RatingInProgressQset = questionList1.Where(x => x.QuestionStatus == "RatingInProgress").OrderBy(o => o.Questionorder).ToList();
            //   ret1.IncompleteQset = questionList.Where(x => x.QuestionStatus == "Incompleted").OrderBy(o => o.Questionorder).ToList();
            ret1.CompleteQset = questionList1.Where(x => x.QuestionStatus == "Completed").OrderBy(o => o.Questionorder).ToList();
            ret1.SurveyTypeID = QuestionResponseID.SurveyTypeId;
            return PartialView("_QuestionNavigation", ret1);
            // return Json(res, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GeteditorView(int FId, string SqNo)
        {
            A3MMModel a3mm = new A3MMModel();
            a3mm.SelectedSqNo = SqNo;
            a3mm.Followuporder = FId;
            return PartialView("_PartialEditorView", a3mm);
        }
        public ActionResult GeteditorViewforfollowup(int SubResponseOrder,int ResponseID,int Id,int AssessmentId)
        {
            A3MMModel a3mm = new A3MMModel();
            a3mm.AssessmentID = AssessmentId;
            a3mm.SubResponseId = Id;
            a3mm.QuestionResponseID = ResponseID;
            a3mm.SubResponseOrder= SubResponseOrder;
            return PartialView("_followupEditorView", a3mm);
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


        public ActionResult GetSelfAssessmentDetailsForResponse(int responseID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            int ClientId = client.ClientId;

            A3MMModel am = new A3MMModel();

            A3MMModel detail = (from a in db.QuestionnaireResponse
                                where a.Id == responseID
                                select new A3MMModel
                                {
                                    QuestionResponseID = a.Id,
                                    EngagementRating = a.EngagementRating,
                                    engagementComments = a.EngagementComments,
                                    responseStatus = a.responseStatus
                                }).ToList().FirstOrDefault();


            return Json(detail, JsonRequestBehavior.AllowGet);

        }

        //Assessment UI
        public ActionResult GetAssessmentRatingDetails(int responseID)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            int ClientId = client.ClientId;

            A3MMModel am = new A3MMModel();

            A3MMModel detail = (from a in db.QuestionnaireResponse
                                where a.Id == responseID
                                select new A3MMModel
                                {
                                    QuestionResponseID = a.Id,
                                    MaturityLevel = a.MaturityLevel,
                                    AssessmentResponse = a.AssessmentResponse,
                                    AreasofImprovement = a.AreaOfImprovement
                                    //meta data might be included
                                }).ToList().FirstOrDefault();


            var actionItemRecord = (from a in db.ActionItemTracker
                                    where a.QResID == responseID
                                    select a).ToList().FirstOrDefault();

            if (actionItemRecord != null)
            {
                detail.ImplementationPlanId =Convert.ToInt32(actionItemRecord.ImplementationPlan_ID);
                detail.BusinessDriverId =Convert.ToInt32(actionItemRecord.BusinessDriver_ID);
                detail.Priority =Convert.ToInt32(actionItemRecord.Priority);
                detail.AMSCatalogId =Convert.ToInt32(actionItemRecord.Catalog_ID);
                
            }

            return Json(detail, JsonRequestBehavior.AllowGet);

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
        public ActionResult getDrpDownvalue(string respID123="")
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            Db db = new Db();
            int resid = Convert.ToInt32(respID123); ;
            int ClientId = client.ClientId;

            A3MMModel am = new A3MMModel();

            A3MMModel detail = (from a in db.QuestionnaireResponse
                                join b in db.EngagementQuestionnaire on a.VersionID equals b.VersionID
                                where a.Id == resid
                                select new A3MMModel
                                {
                                    DisciplineId = b.Discipline_ID.Value,
                                    ComponentId = b.Component_ID.Value,
                                    AspectID = b.AspectID
                                }).ToList().FirstOrDefault();
            return Json(detail, JsonRequestBehavior.AllowGet);

        }


        public ActionResult ReadImplementationPlan()
        {
            Db db = new Db();
            var apList = (from a in db.A3MM_ImplementationPlan
                          select a).ToList();
            var item = apList.OrderBy(x => x.ID).Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.ImplementationPlan.ToString() });
            return Json(item, JsonRequestBehavior.AllowGet); ;

        }

        public ActionResult ReadBusinessDriver()
        {
            Db db = new Db();
            var apList = (from a in db.A3MM_BusinessDriver
                          select a).ToList();
            var item = apList.OrderBy(x => x.ID).Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.BusinessDriver.ToString() });
            return Json(item, JsonRequestBehavior.AllowGet); ;

        }

        public ActionResult ReadAMSCatolog()
        {
            Db db = new Db();
            var apList = (from a in db.A3MM_Catalog
                          select a).ToList();
            var item = apList.OrderBy(x => x.ID).Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Catalog.ToString() });
            return Json(item, JsonRequestBehavior.AllowGet); ;

        }

        public ActionResult ReadPriority()
        {
            Db db = new Db();
            List<int> priorityList = new List<int> { 1, 2, 3, 4 };
            var item = priorityList.OrderBy(x => x).Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() });
            return Json(item, JsonRequestBehavior.AllowGet); ;

        }

        public ActionResult RedirectToDGFromAI(string AssessmentID,string UI)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int assessID = Convert.ToInt32(AssessmentID);
            Db db = new Db();



            var surveyTypeID = (from a in db.Assessment
                                where a.Id == assessID
                                select a.SurveyTypeID).FirstOrDefault();
           
            A3MMModel ret1 = new A3MMModel();
            ret1.AssessmentID = assessID;
          

            //var ret = "window.location='EngagementDataGathering?AssessmentIDparam=" + assessID + "&isAI=" + isAI + "'";
            //return JavaScript("window.location = 'DataGathering?AssessmentIDparam=15&QNoparam=3'");


            if (UI == "AssessmentUI") {
                ret1 = GetDataGatheringDetail(ret1, "AssTeamView");
                return View("AssessmentDataGathering", ret1);
            }
            if (UI == "ResponseUI") {
                ret1 = GetDataGatheringDetail(ret1,"");
                return View("EngagementDataGathering", ret1);
            }
            ret1 = GetDataGatheringDetail(ret1,"");
            return View("AssessmentResponseView", ret1);
            

            

        }
          public ActionResult RedirectToGathering(string AssessmentIDparam)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                var uid = (int)Session["UserId"];
                int assessID = Convert.ToInt32(AssessmentIDparam);
                Db db = new Db();

                var surveyTypeID = (from a in db.Assessment
                                    where a.Id == assessID
                                    select a.SurveyTypeID).FirstOrDefault();

                A3MMModel ret1 = new A3MMModel();
                ret1.AssessmentID = assessID;
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                // var userrole= unitOfWork.UserApplications.(x=>x.ApplicationId==&x.ClientId== client.ClientId&&x.IsDeleted==0&&x.UserId==uid)
                var Userrole = (from x in db.UserApplications
                                where x.ApplicationId == 20 & x.ClientId == client.ClientId &&
                                x.IsDeleted == false && x.UserId == uid
                                select x.RoleId).FirstOrDefault();
                if (Userrole == 2)
                {//admin
                    ret1 = GetDataGatheringDetail(ret1, "AssTeamView");
                    return View("AssessmentDataGathering", ret1);

                }
                if (Userrole == 1)
                {
                    ret1 = GetDataGatheringDetail(ret1, "");
                    return View("EngagementDataGathering", ret1);

                }
                ret1 = GetDataGatheringDetail(ret1, "");
                return View("AssessmentResponseView", ret1);
            }
            catch (Exception ex)
            {
                Log.Error("Error in redirecting to datagathering screen: Approval ", ex);
                throw ex;
            }
         
        }

        public ActionResult A3MM_BusinessDriverReport()
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
                //Log.Error("A3MM_MaturityLevelCompare Index Action error 'A3MM_MaturityLevelCompare()' ", ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult RedirectToDG(string AssessmentID, string QNo) //AssessmentOverview Screen
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            int assessID = Convert.ToInt32(AssessmentID);
            int questionNumber = Convert.ToInt32(QNo);
          //  int SurveyTypeID1 = Convert.ToInt32(SurveyTypeID);

            var ret = "window.location='ResponseGathering/EngagementDataGathering?AssessmentIDparam=" + assessID + "&QNoparam=" + questionNumber + "'";
           
            return JavaScript(ret);


        }


        public ActionResult RedirectToDG(string AssessmentID, string QuestionId, string UserRole) //AssessmentOverview Screen
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
            int assessID = Convert.ToInt32(AssessmentID);
            int qID = Convert.ToInt32(QuestionId);
            string QNo = (from a in db.EngagementQuestionnaire
                          where a.Id == qID
                          select a.SQNo).ToList().FirstOrDefault();


            //var ret = "window.location='ResponseGathering/EngagementDataGathering?AssessmentIDparam=" + assessID + "&QNoparam=" + questionNumber + "'";

            A3MMModel ret1 = new A3MMModel();
            ret1.AssessmentID = assessID;
            ret1.SQNo = QNo;

            ret1 = GetDataGatheringDetailForAssessmentOverview(ret1, QNo);

            if (UserRole == "Admin")
            {
                return View("AssessmentDataGathering", ret1);

            }
            else
            {
                return View("EngagementDataGathering", ret1);

            }

        }


        public ActionResult RedirectToDGView(string AssessmentID, string QuestionId) //AssessmentOverview Screen
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
            int assessID = Convert.ToInt32(AssessmentID);
            int qID = Convert.ToInt32(QuestionId);
            string QNo = (from a in db.EngagementQuestionnaire
                          where a.Id == qID
                          select a.SQNo).ToList().FirstOrDefault();


            //var ret = "window.location='ResponseGathering/EngagementDataGathering?AssessmentIDparam=" + assessID + "&QNoparam=" + questionNumber + "'";

            A3MMModel ret1 = new A3MMModel();
            ret1.AssessmentID = assessID;
            ret1.SQNo = QNo;

            ret1 = GetDataGatheringDetailForAssessmentOverview(ret1, QNo);

            return View("AssessmentResponseView", ret1);




        }

    }
}