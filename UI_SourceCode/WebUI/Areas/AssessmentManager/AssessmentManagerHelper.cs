using Data;
using System.Data;
using Domain.Abstract;
using WebUI.Areas.ClientProfileManager.Dto;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.IncidentNavigator.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using WebUI.Dto;
using WebUI.Models;
using NTTData.AMOPortal.Domain.Service;
using System.IO;
using System.Text;
using WebUI.Areas.IncidentNavigator.Models;
using WebUI.Areas.AssessmentManager.Models;
using Domain.Entities.IncidentNavigator;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using WebUI.Areas.IncidentNavigator.AspxReport;
using System.Reflection;
using WebUI.Areas.ClientProfileManager.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Text.RegularExpressions;
using log4net;
using Domain.Entities.A3MM;
using WebUI.Areas.AssessmentManager.Controllers;
using System.Globalization;
using Domain.Entities;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IdeaManager.Controllers;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace WebUI.Areas.AssessmentManager
{
    public class AssessmentManagerHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssessmentController));

        public static IEnumerable<A3MMModel> getDisciplineDetails(int clientId, int SurveyTypeID, string QSetNo, string isView = "No")
        {
            IList<A3MMModel> disciplineList = new List<A3MMModel>();
            var paramName = new string[] { "ClientId", "SurveyTypeID", "QSetNo" };
            var paramValue = new object[] { clientId, SurveyTypeID, QSetNo };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("A3MM_ExtractDisciplineData", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new A3MMModel();
                    //coldet.ClientId = Convert.ToInt32(dr["ClientID"]);
                    if (!(dr["Id"] is DBNull))
                        coldet.Id = Convert.ToInt32(dr["Id"]);
                    else
                        coldet.Id = 0;

                    coldet.DisciplineId = Convert.ToInt32(dr["DisciplineId"]);
                    if (!(dr["DisciplineId"] is DBNull))
                        coldet.DisciplineId = Convert.ToInt32(dr["DisciplineId"]);
                    else
                        coldet.DisciplineId = 0;
                    //coldet.DisciplineName = dr["DisciplineName"].ToString();
                    //coldet.ComponentId = Convert.ToInt32(dr["ComponentId"]);
                    if (!(dr["ComponentId"] is DBNull))
                        coldet.ComponentId = Convert.ToInt32(dr["ComponentId"]);
                    else
                        coldet.ComponentId = 0;
                    //coldet.ComponentName = dr["ComponentName"].ToString();
                    //coldet.ComponentDefinition = dr["ComponentDefinition"].ToString();
                    if (!(dr["AspectID"] is DBNull))
                        coldet.AspectID = Convert.ToInt32(dr["AspectID"]);
                    else
                        coldet.AspectID = 0;
                    //coldet.AspectName = dr["AspectName"].ToString();
                    if (!(dr["DatasourceId"] is DBNull))
                        coldet.DatasourceId = Convert.ToInt32(dr["DatasourceId"]);
                    else
                        coldet.DatasourceId = 0;
                    if (!(dr["ParentId"] is DBNull))
                        coldet.ParentID = Convert.ToInt32(dr["ParentId"]);
                    else
                        coldet.ParentID = null;
                    if (!(dr["QuestionId"] is DBNull))
                        coldet.QuestionId = Convert.ToInt32(dr["QuestionId"]);
                    else
                        coldet.QuestionId = 0;

                    if (!(dr["QNo"] is DBNull))
                        coldet.QNo = Convert.ToInt32(dr["QNo"]);
                    else
                        coldet.QNo = 0;

                    coldet.Question = dr["Question"].ToString();
                    if (coldet.Question == "")
                    {
                        coldet.Question = null;
                    }


                    coldet.QuestionIdentifier = dr["QuestionIdentifier"].ToString();
                    if (coldet.QuestionIdentifier == "")
                    {
                        coldet.QuestionIdentifier = null;
                    }

                    coldet.ExampleAnswer = dr["ExampleAns"].ToString();
                    coldet.DisciplineName = dr["DisciplineName"].ToString();
                    coldet.ComponentName = dr["ComponentName"].ToString();
                    coldet.ComponentDefinition = dr["ComponentDefinition"].ToString();
                    coldet.AspectName = dr["AspectName"].ToString();
                    coldet.Datasource = dr["DataSource"].ToString();
                    coldet.Status = dr["QuestionStatus"].ToString();
                    if (coldet.Status == "")
                    {
                        coldet.Status = "Draft";
                    }
                    coldet.isView = isView;
                    coldet.SQNo = dr["SQNo"].ToString();

                    disciplineList.Add(coldet);
                }

                return disciplineList;
            }


        }


        public static int addQuestion(A3MMModel a3mm)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    EngagementQuestionnaire eq = new EngagementQuestionnaire();
                    eq.VersionID = a3mm.VersionID;
                    eq.AspectID = a3mm.AspectID;
                    eq.Question = a3mm.Question;
                    eq.ExampleAns = a3mm.ExampleAnswer;
                    eq.DataSourceID = a3mm.DatasourceId;
                    eq.QNo = a3mm.QNo;
                    eq.SurveyTypeID = Convert.ToInt32(a3mm.SurveyTypeID);
                    eq.Status = "Draft";
                    eq.Discipline_ID = a3mm.DisciplineId;
                    eq.Component_ID = a3mm.ComponentId;
                    eq.DisciplineName = a3mm.DisciplineName;
                    eq.ComponentName = a3mm.ComponentName;
                    eq.AspectName = a3mm.AspectName;
                    eq.QuestionOrder = a3mm.QuestionOrder;
                    eq.SQNo = a3mm.SQNo;
                    if (a3mm.ParentID != null) //sub question
                    {
                        eq.ParentID = a3mm.ParentID;
                        eq.SubQOrder = a3mm.SubQOrder;

                        var ParentQuestion = unitOfWork.EngagementQuestionnaire.GetByID(eq.ParentID);
                        var ParentQuestinIdentifier = ParentQuestion.QuestionIdentifier;
                        var subQUestionIdentifierNumber = ParentQuestinIdentifier.Substring(6);
                       
                        var subQuestionCharacter = ((char)(eq.SubQOrder + 64));

                        //to change
                        eq.QuestionIdentifier = a3mm.DiscplineAbbrevation + "-" + a3mm.ComponentAbbrevation + "-" + subQUestionIdentifierNumber + subQuestionCharacter;

                    }

                    if (a3mm.ParentID == null)
                    {
                        a3mm.countOfQOfSameDisandComp += 1;

                        if (a3mm.countOfQOfSameDisandComp < 10)
                        {
                            eq.QuestionIdentifier = a3mm.DiscplineAbbrevation + "-" + a3mm.ComponentAbbrevation + "-" + "0" + a3mm.countOfQOfSameDisandComp;

                        }
                        else
                        {
                            eq.QuestionIdentifier = a3mm.DiscplineAbbrevation + "-" + a3mm.ComponentAbbrevation + "-" + a3mm.countOfQOfSameDisandComp;

                        }
                    }





                    unitOfWork.EngagementQuestionnaire.Insert(eq);
                    unitOfWork.Save();
                    id = eq.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public static int addQuestionEdit(A3MMModel a3mm)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var updatetitem = unitOfWork.EngagementQuestionnaire.GetByID(a3mm.QuestionId);
                    updatetitem.VersionID = a3mm.VersionID;
                    updatetitem.AspectID = a3mm.AspectID;
                    updatetitem.Question = a3mm.Question;
                    updatetitem.ExampleAns = a3mm.ExampleAnswer;
                    updatetitem.DataSourceID = a3mm.DatasourceId;
                    updatetitem.Discipline_ID = a3mm.DisciplineId;
                    updatetitem.Component_ID = a3mm.ComponentId;
                    updatetitem.AspectID = a3mm.AspectID;
                    updatetitem.DisciplineName = a3mm.DisciplineName;
                    updatetitem.ComponentName = a3mm.ComponentName;
                    updatetitem.AspectName = a3mm.AspectName;


                    //if (a3mm.ParentID != null) //sub question
                    //{
                    //    updatetitem.ParentID = a3mm.ParentID;
                    //    updatetitem.SubQOrder = a3mm.SubQOrder;

                    //    //a3mm.countOfQOfSameDisandComp += 1;


                    //    var subQuestionCharacter = ((char)(updatetitem.SubQOrder + 64));

                    //    updatetitem.QuestionIdentifier = a3mm.DiscplineAbbrevation + "-" + a3mm.ComponentAbbrevation + "-" + a3mm.countOfQOfSameDisandComp + subQuestionCharacter;

                    //}

                    //Main Question
                    if (a3mm.ParentID == null && a3mm.isEditDisciplineComponentChange)
                    {
                        a3mm.countOfQOfSameDisandComp += 1;

                        if (a3mm.countOfQOfSameDisandComp < 10)
                        {
                            updatetitem.QuestionIdentifier = a3mm.DiscplineAbbrevation + "-" + a3mm.ComponentAbbrevation + "-" + "0" + a3mm.countOfQOfSameDisandComp;

                        }
                        else
                        {
                            updatetitem.QuestionIdentifier = a3mm.DiscplineAbbrevation + "-" + a3mm.ComponentAbbrevation + "-" + a3mm.countOfQOfSameDisandComp;
                         
                        }
                    }

                    unitOfWork.EngagementQuestionnaire.Update(updatetitem);
                    unitOfWork.Save();
                    id = updatetitem.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public static int addLaunchAssessmentDetails(A3MMModel a3mm)
        {
        
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    CultureInfo cultures = new CultureInfo("en-US");
                    Assessment assess = new Assessment();
                    assess.SurveyTypeID = a3mm.AssessmentID;
                    assess.AccountID = a3mm.AccountID;
                    assess.ContractID = a3mm.ContractID;
                    assess.EngagementTeamPointOfContact = a3mm.EngagementPocID;
                    assess.EngagementTeamtoCommunicate = a3mm.EngagementComID;
                    assess.AssessmentTeam = a3mm.AssessmentTeam;
                    assess.AssessmentName = a3mm.AssessmentName;
                    assess.QuestionaireVersion = a3mm.VersionID;
                    assess.StartDate = DateTime.Parse(a3mm.startDateIgnore, cultures);
                    assess.AnalysisDate = DateTime.Parse(a3mm.analysisDateIgnore, cultures);
                    assess.ReviewDate = DateTime.Parse(a3mm.reviewDateIgnore, cultures);
                    if (a3mm.PCDateIgnore != null)
                        assess.Planned_CompletionDate = DateTime.Parse(a3mm.PCDateIgnore, cultures);
                    assess.Status = a3mm.Status;
                    assess.ClientId = a3mm.ClientId;
                    unitOfWork.Assessment.Insert(assess);
                    unitOfWork.Save();
                    id = assess.Id;

                    return id;

                }
            }
            catch (Exception ex)
            {
                Log.Error("error in addLaunchAssessmentDetails ", ex);

            }
            return 0;
        }

        public static void SendLaunchEmail(UserProfile poc, UserProfile commUser, UserProfile username, string Status, string AssessmentName, string responseLink)
        {
            try
            {
                string[] ccuser = { commUser.Email, "A3MMAssessmentTeam@nttdata.com" };
                new AssessmentController().SendLaunchNotification(poc.Email, ccuser, poc.FirstName + " " + poc.LastName, username.FirstName + " " + username.LastName, Status, AssessmentName, responseLink);
            }
            catch (Exception ex)
            {
                Log.Error("SendLaunchEmail " + ex);
            }
        }
        public static void SendReviewEmail(UserProfile poc, string[] ccuser, UserProfile username, string AssessmentType, string AssessmentName, string responseLink, int questCount,string ActionStatus)
        {
            try
            {
                new AssessmentController().SendReviewNotification(poc.Email, ccuser, poc.FirstName + " " + poc.LastName, username.FirstName + " " + username.LastName, AssessmentType, AssessmentName, responseLink, questCount, ActionStatus);
            }
            catch (Exception ex)
            {
                Log.Error("SendLaunchEmail " + ex);
            }
        }
        public static void SendReviewEmailtoAssessmentTeam(string assteam, string[] ccuser, UserProfile username, string AssessmentType, string AssessmentName, string responseLink, int questCount, string ActionStatus)
        {
            try
            {
                new AssessmentController().SendReviewNotification(assteam, ccuser, "A3MM Assessment Team", username.FirstName + " " + username.LastName, AssessmentType, AssessmentName, responseLink, questCount, ActionStatus);
            }
            catch (Exception ex)
            {
                Log.Error("SendLaunchEmail " + ex);
            }
        }

        public static void addQuestionResponseRecords(IList<EngagementQuestionnaire> qlist, int AssessmentID, int? SurveyTypeID, int clientID)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    foreach (var a in qlist)
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        QuestionnaireResponse qr = new QuestionnaireResponse();
                        qr.AssessmentID = AssessmentID;
                        qr.QNo = a.QNo;//deva-need to change
                        qr.SurveyTypeId = SurveyTypeID;
                        qr.VersionID = a.VersionID;
                        qr.ClientID = clientID;
                        qr.responseStatus = "Unanswered";
                        qr.SQNo = a.SQNo;
                        unitOfWork.QuestionnaireResponse.Insert(qr);
                        unitOfWork.Save();
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error("error in addQuestionResponseRecords ", ex);
            }
        }






        public static int editLaunchAssessmentDetails(A3MMModel a3mm)

        {

            int id = 0;
            CultureInfo cultures = new CultureInfo("en-US");
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var updatetitem = unitOfWork.Assessment.GetByID(a3mm.AssessmentID);
                    updatetitem.AssessmentTeam = a3mm.AssessmentTeam;
                    updatetitem.Status = "Initiated";
                    updatetitem.Planned_CompletionDate = DateTime.Parse(a3mm.PCDateIgnore, cultures);
                    updatetitem.EngagementTeamPointOfContact = a3mm.EngagementPocID;
                    updatetitem.EngagementTeamtoCommunicate = a3mm.EngagementComID;


                    unitOfWork.Assessment.Update(updatetitem);
                    unitOfWork.Save();
                    id = updatetitem.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("error in editLaunchAssessmentDetails ", ex);

            }
            return 0;
        }
        public static IEnumerable<A3MMAssessmentModel> Get_AssessmentDetails(int clientId)
        {
            IList<A3MMAssessmentModel> AssessmentList = new List<A3MMAssessmentModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { clientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("A3MM_GetAssessmentDetails", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new A3MMAssessmentModel();
                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    coldet.ClientId = Convert.ToInt32(dr["ClientId"]);
                    if ((DateTime)dr["StartDate"] > new DateTime(1900, 01, 02))
                    {
                        coldet.StartDate = (DateTime)dr["StartDate"];
                        coldet.StartDateString = (coldet.StartDate).ToString("MM/dd/yyyy");
                    }
                    if ((DateTime)dr["EndDate"] > new DateTime(1900, 01, 02))
                    {
                        coldet.EndDate = (DateTime)dr["EndDate"];
                        coldet.EndDateString = (coldet.EndDate).ToString("MM/dd/yyyy");
                    }
                    coldet.AccountID = Convert.ToInt32(dr["AccountID"]);
                    coldet.EngagementName = dr["AccountName"].ToString();
                    coldet.ContractID = Convert.ToInt32(dr["ContractID"]);
                    coldet.ContractName = dr["ContractName"].ToString();
                    coldet.AssessmentName = dr["AssessmentName"].ToString();
                    coldet.EngagementTeamPointOfContact = Convert.ToInt32(dr["EngagementTeamPointOfContact"]);
                    coldet.EngagementTeamPointOfContactName = dr["EngagementTeamPointOfContactName"].ToString();
                    coldet.EngagementTeamtoCommunicate = Convert.ToInt32(dr["EngagementTeamtoCommunicate"]);
                    coldet.EngagementTeamtoCommunicateName = dr["EngagementTeamtoCommunicateName"].ToString();
                    coldet.AssessmentTeam = dr["AssessmentTeam"].ToString();
                    coldet.QuestionaireVersion = dr["QuestionaireVersion"].ToString();
                    if (!(dr["SurveyTypeID"] is DBNull))
                        coldet.SurveyTypeID = Convert.ToInt32(dr["SurveyTypeID"]);
                    else
                        coldet.SurveyTypeID = 0;
                    coldet.SurveyType = dr["AssessmentType"].ToString();
                    coldet.Status = dr["AssessmentStatus"].ToString();
                    if (coldet.Status == "")
                    {
                        coldet.Status = null;
                    }

                    AssessmentList.Add(coldet);
                }

                return AssessmentList;
            }


        }

        public static A3MMAssessmentModel getAssessmentDetail(int AssessmentID)
        {
            A3MMAssessmentModel Assessment = new A3MMAssessmentModel();
            var paramName = new string[] { "AssessmentID" };
            var paramValue = new object[] { AssessmentID };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("A3MM_GetOneAssessmentDetail", paramName, paramValue);
                var coldet = new A3MMAssessmentModel();
                foreach (DataRow dr in res.Rows)
                {

                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    coldet.ClientId = Convert.ToInt32(dr["ClientId"]);
                    if (!(dr["StartDate"] is DBNull))
                    {
                        if ((DateTime)dr["StartDate"] > new DateTime(1900, 01, 02))
                            coldet.StartDate = (DateTime)dr["StartDate"];
                    }
                    if (!(dr["Planned_CompletionDate"] is DBNull))
                    {
                        if ((DateTime)dr["Planned_CompletionDate"] > new DateTime(1900, 01, 02))
                        {
                            coldet.Planned_CompletionDate = (DateTime)dr["Planned_CompletionDate"];
                        }
                    }
                    if (!(dr["AnalysisDate"] is DBNull))
                    {
                        if ((DateTime)dr["AnalysisDate"] > new DateTime(1900, 01, 02))
                        {
                            coldet.AnalysisDateEnd = (DateTime)dr["AnalysisDate"];
                        }
                    }
                    if (!(dr["ReviewDate"] is DBNull))
                    {
                        if ((DateTime)dr["ReviewDate"] > new DateTime(1900, 01, 02))
                        {
                            coldet.ReviewDateEnd = (DateTime)dr["ReviewDate"];
                        }
                    }
                    coldet.AccountID = Convert.ToInt32(dr["AccountID"]);
                    coldet.EngagementName = dr["AccountName"].ToString();
                    coldet.ContractID = Convert.ToInt32(dr["ContractID"]);
                    coldet.ContractName = dr["ContractName"].ToString();
                    coldet.AssessmentName = dr["AssessmentName"].ToString();
                    coldet.EngagementTeamPointOfContact = Convert.ToInt32(dr["EngagementTeamPointOfContact"]);
                    coldet.EngagementTeamPointOfContactName = dr["EngagementTeamPointOfContactName"].ToString();
                    coldet.EngagementTeamtoCommunicate = Convert.ToInt32(dr["EngagementTeamtoCommunicate"]);
                    coldet.EngagementTeamtoCommunicateName = dr["EngagementTeamtoCommunicateName"].ToString();
                    coldet.AssessmentTeam = dr["AssessmentTeam"].ToString();
                    coldet.QuestionaireVersion = dr["QuestionaireVersion"].ToString();
                    if (!(dr["SurveyTypeID"] is DBNull))
                        coldet.SurveyTypeID = Convert.ToInt32(dr["SurveyTypeID"]);
                    else
                        coldet.SurveyTypeID = 0;
                    coldet.SurveyType = dr["AssessmentType"].ToString();
                    coldet.Status = dr["AssessmentStatus"].ToString();
                    if (coldet.Status == "")
                    {
                        coldet.Status = null;
                    }
                }
                return coldet;
            }
        }

        public static bool UpdateAssessment(A3MMAssessmentModel assess)
        {

            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    CultureInfo cultures = new CultureInfo("en-US");
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.Assessment.GetByID(assess.Id);
                    updatetitem.StartDate = DateTime.Parse(assess.startDateIgnore, cultures);
                    if (updatetitem.Status != "Preparation")
                    {
                        updatetitem.Planned_CompletionDate = DateTime.Parse(assess.plannedCompletionDateIgnore, cultures);
                    }
                 
                    updatetitem.AnalysisDate = DateTime.Parse(assess.analysisDateIgnore, cultures);
                    updatetitem.ReviewDate = DateTime.Parse(assess.reviewDateIgnore, cultures);
                    updatetitem.AccountID = assess.AccountID;
                    updatetitem.ContractID = assess.ContractID;
                    updatetitem.AssessmentName = assess.AssessmentName;
                    updatetitem.SurveyTypeID = assess.SurveyTypeID;
                    updatetitem.EngagementTeamPointOfContact = assess.EngagementTeamPointOfContact;
                    updatetitem.EngagementTeamtoCommunicate = assess.EngagementTeamtoCommunicate;
                    updatetitem.QuestionaireVersion = assess.QuestionaireVersion;
                    updatetitem.AssessmentTeam = assess.AssessmentTeam;
                    updatetitem.SurveyTypeID = assess.SurveyTypeID;
                    updatetitem.ClientId = assess.ClientId;

                    unitOfWork.Assessment.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public static IEnumerable<A3MMModel> GetQuestionSet(int AssessmentId, int surveyTypeID)
        {
            IList<A3MMModel> A3MMList = new List<A3MMModel>();
            var paramName = new string[] { "@AssessmentId", "@AssessmentTypeId" };
            var paramValue = new object[] { AssessmentId, surveyTypeID };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("A3MM_DataGathering", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new A3MMModel();
                    coldet.ClientId = Convert.ToInt32(dr["ClientID"]);
                    coldet.QNo = Convert.ToInt32(dr["QNo"]);
                    coldet.AssessmentID = Convert.ToInt32(dr["Id"]);
                    coldet.AccountID = Convert.ToInt32(dr["AccountID"]);
                    coldet.ContractID = Convert.ToInt32(dr["ContractID"]);
                    coldet.EngagementPocID = Convert.ToInt32(dr["EngagementTeamPointOfContact"]);
                    coldet.EngagementComID = Convert.ToInt32(dr["EngagementTeamtoCommunicate"]);
                    coldet.StartDate = Convert.ToDateTime(dr["StartDate"]);
                    coldet.Status = dr["Status"].ToString();
                    coldet.AssessmentTeam = dr["AssessmentTeam"].ToString();
                    coldet.AssessmentName = dr["AssessmentName"].ToString();
                    coldet.VersionID = dr["VersionID"].ToString();
                    coldet.Feedback = dr["FeedBack"].ToString();
                    coldet.FollowupQuestion = dr["FollowUpQuestion"].ToString();
                    coldet.AreasofImprovement = dr["AreaOfImprovement"].ToString();
                    coldet.Response = dr["Response"].ToString();
                    if (!(dr["MaturityLevel"] is DBNull))
                    {
                        coldet.MaturityLevel = dr["MaturityLevel"].ToString();
                    }

                    if (!(dr["SurveyTypeID"] is DBNull))
                        coldet.SurveyTypeID = Convert.ToInt32(dr["SurveyTypeID"]);
                    else
                        coldet.SurveyTypeID = 0;

                    if (!(dr["DatasourceId"] is DBNull))
                        coldet.DatasourceId = Convert.ToInt32(dr["DatasourceId"]);
                    else
                        coldet.DatasourceId = 0;
                    if (!(dr["AspectID"] is DBNull))
                        coldet.AspectID = Convert.ToInt32(dr["AspectID"]);
                    else
                        coldet.AspectID = 0;

                    coldet.Question = dr["Question"].ToString();
                    if (coldet.Question == "")
                    {
                        coldet.Question = null;
                    }
                    coldet.ExampleAnswer = dr["ExampleAns"].ToString();
                    if (!(dr["LastFeedBackOn"] is DBNull))
                    {
                        coldet.LastFeedbackOn = Convert.ToDateTime(dr["LastFeedBackOn"]);
                    }
                    else
                    {
                        coldet.LastFeedbackOn = null;
                    }

                    if (!(dr["LastUpdatedOn"] is DBNull))
                    {
                        coldet.LastUpdatedOn = Convert.ToDateTime(dr["LastUpdatedOn"]);
                    }
                    else
                    {
                        coldet.LastUpdatedOn = null;
                    }


                    if (!(dr["LastFeedBackBy"] is DBNull))
                        coldet.LastFeedbackBy = Convert.ToInt32(dr["LastFeedBackBy"]);
                    else
                        coldet.LastFeedbackBy = 0;

                    if (!(dr["LastUpdatedBy"] is DBNull))
                        coldet.LastUpdatedBy = Convert.ToInt32(dr["LastUpdatedBy"]);
                    else
                        coldet.LastUpdatedBy = 0;
                    A3MMList.Add(coldet);
                }

                return A3MMList;
            }


        }

        public static int saveDataGathering(A3MMModel a3mm)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var updatetitem = unitOfWork.QuestionnaireResponse.GetByID(a3mm.QuestionResponseID);
                    updatetitem.MaturityLevel = a3mm.MaturityLevel;
                    updatetitem.Response = a3mm.Response;
                    updatetitem.AssessmentResponse = a3mm.AssessmentResponse;
                    updatetitem.FollowUpQuestion = a3mm.FollowupQuestion;
                    var ActionItem = "";

                    //if (a3mm.AreasofImprovement != null && updatetitem.AreaOfImprovement == null)
                    //{
                    //    ActionItem = a3mm.AreasofImprovement;
                    //}
                    ActionItem = a3mm.AreasofImprovement;
                    updatetitem.AreaOfImprovement = a3mm.AreasofImprovement;
                    updatetitem.Feedback = a3mm.Feedback;
                    updatetitem.LastFeedBackBy = a3mm.LastFeedbackBy;
                    updatetitem.LastFeedBackOn = a3mm.LastFeedbackOn;
                    updatetitem.LastUpdatedBy = a3mm.LastUpdatedBy;
                    updatetitem.LastUpdatedOn = a3mm.LastUpdatedOn;



                    //if (a3mm.AreasofImprovement!=null && updatetitem.AreaOfImprovement==null) //for first response
                    //{
                    //     ActionItem = a3mm.AreasofImprovement;
                    //}

                    //if (a3mm.AreasofImprovement != null && updatetitem.AreaOfImprovement != null) //for second response
                    //{
                    //    ActionItem = a3mm.AreasofImprovement;
                    //}



                    unitOfWork.QuestionnaireResponse.Update(updatetitem);
                    unitOfWork.Save();
                    id = updatetitem.Id;


                    var updateitem2 = unitOfWork.Assessment.GetByID(a3mm.AssessmentID);
                    if (updateitem2.Status == "Initiated")
                    {
                        updateitem2.Status = "Analysis";
                    }
                    unitOfWork.Assessment.Update(updateitem2);
                    unitOfWork.Save();


                    if (ActionItem != "" && ActionItem != null)
                    {
                        if (a3mm.Status == "Review" || a3mm.Status == "Engagement Published")
                        {
                            ActionItem = Regex.Replace(ActionItem, @"<[^>]+>|&nbsp;", "").Trim();
                            //ActionItem = Regex.Replace(ActionItem, @"\s{2,}", " ");
                            if (a3mm.ActionItemID != null && a3mm.ActionItemID != 0)
                            {
                                ActionItemTracker ait1 = unitOfWork.ActionItemTracker.GetByID(a3mm.ActionItemID);
                                ait1.ActionItem = ActionItem;
                                unitOfWork.ActionItemTracker.Update(ait1);
                                unitOfWork.Save();

                            }
                            else
                            {
                                ActionItemTracker ait = new ActionItemTracker();
                                ait.AssessmentID = a3mm.AssessmentID;
                                ait.Discipline_ID = a3mm.DisciplineId;
                                ait.Component_ID = a3mm.ComponentId;
                                ait.Aspect_ID = Convert.ToInt32(a3mm.AspectID);
                                ait.ClientID = a3mm.ClientId;
                                ait.ActionItem = ActionItem;
                                ait.Status = "Defined";
                                ait.QResID = a3mm.QuestionResponseID;
                                unitOfWork.ActionItemTracker.Insert(ait);
                                unitOfWork.Save();
                            }
                        }
                    }

                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

      

        public static int createNewQuestionnaire(int surveyTypeIDInt, string VersionID, string Status, int usid)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    EngagementQuestionSet eq = new EngagementQuestionSet();
                    eq.Assessment_ID = surveyTypeIDInt;
                    eq.VersionID = VersionID;
                    eq.Status = Status;
                    eq.LastUpdatedBy = usid;
                    eq.LastUpdatedDate = DateTime.Now;
                    eq.isdeleted = 0;
                    unitOfWork.EngagementQuestionSet.Insert(eq);
                    unitOfWork.Save();
                    id = eq.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public static int clonePreviousQuestionSet(List<EngagementQuestionnaire> questionList, int surveyTypeIDInt, string VersionID)
        {
            var parentQuestionList = (from a in questionList
                                      where a.ParentID == null
                                      select a).ToList();

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {

                    foreach (var a in parentQuestionList)
                    {

                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        EngagementQuestionnaire eq = new EngagementQuestionnaire();

                        // var hasSubQuestion = unitOfWork.EngagementQuestionnaire.Get(e => e.ParentID == a.Id).Any();

                        var hasSubQuestion= (from ql in questionList
                                             where ql.ParentID == a.Id
                                             select a).Any();
                        eq.SurveyTypeID = surveyTypeIDInt;
                        eq.VersionID = VersionID;
                        eq.Status = "Draft";
                        eq.Question = a.Question;
                        eq.QNo = a.QNo;
                        eq.ExampleAns = a.ExampleAns;
                        eq.Discipline_ID = a.Discipline_ID;
                        eq.Component_ID = a.Component_ID;
                        eq.AspectID = a.AspectID;
                        eq.DisciplineName = a.DisciplineName;
                        eq.ComponentName = a.ComponentName;
                        eq.AspectName = a.AspectName;
                        eq.DataSourceID = a.DataSourceID;
                        eq.SQNo = a.SQNo;
                        eq.ParentID = a.ParentID;
                        eq.QuestionOrder = a.QuestionOrder;
                        eq.SubQOrder = a.SubQOrder;
                        eq.QuestionIdentifier = a.QuestionIdentifier;

                        unitOfWork.EngagementQuestionnaire.Insert(eq);
                        unitOfWork.Save();
                        id = eq.Id;

                        if (hasSubQuestion)
                        {
                            // var subQuestionList = unitOfWork.EngagementQuestionnaire.Get(e => e.ParentID == a.Id).ToList();
                            var subQuestionList=(from ql in questionList
                                                 where ql.ParentID == a.Id
                                                 select ql).ToList();
                            foreach (var item in subQuestionList)
                            {
                                EngagementQuestionnaire sq = new EngagementQuestionnaire();
                                sq.SurveyTypeID = surveyTypeIDInt;
                                sq.VersionID = VersionID;
                                sq.Status = "Draft";
                                sq.Question = item.Question;
                                sq.QNo = item.QNo;
                                sq.ExampleAns = item.ExampleAns;
                                sq.Discipline_ID = item.Discipline_ID;
                                sq.Component_ID = item.Component_ID;
                                sq.AspectID = a.AspectID;
                                sq.DisciplineName = item.DisciplineName;
                                sq.ComponentName = item.ComponentName;
                                sq.AspectName = item.AspectName;
                                sq.DataSourceID = item.DataSourceID;
                                sq.SQNo = item.SQNo;
                                sq.ParentID = id;
                                sq.QuestionOrder = item.QuestionOrder;
                                sq.SubQOrder = item.SubQOrder;
                                sq.QuestionIdentifier = item.QuestionIdentifier;
                                unitOfWork.EngagementQuestionnaire.Insert(sq);
                                unitOfWork.Save();
                            }
                        }
                                
                    }
                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }


        public static void FinalizeQuestions(List<EngagementQuestionnaire> questionList)
        {


            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    foreach (var a in questionList)
                    {
                        var updatetitem = unitOfWork.EngagementQuestionnaire.GetByID(a.Id);
                        updatetitem.Status = "Finalized";
                        unitOfWork.EngagementQuestionnaire.Update(updatetitem);
                        unitOfWork.Save();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public static void FinalizeSingleQuestion(int Id)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    
                        var updatetitem = unitOfWork.EngagementQuestionnaire.GetByID(Id);
                        updatetitem.Status = "Finalized";
                        unitOfWork.EngagementQuestionnaire.Update(updatetitem);
                        unitOfWork.Save();                    
                }
            }
            catch (Exception)
            {

            }

        }

        public static void ArchiveQuestions(List<EngagementQuestionnaire> questionstoArchiveList)
        {


            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    foreach (var a in questionstoArchiveList)
                    {
                        var updatetitem = unitOfWork.EngagementQuestionnaire.GetByID(a.Id);
                        updatetitem.Status = "Archived";
                        unitOfWork.EngagementQuestionnaire.Update(updatetitem);
                        unitOfWork.Save();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }


        public static void ArchivePreviousQuestionSet(List<EngagementQuestionSet> questionSetList)
        {


            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    foreach (var a in questionSetList)
                    {
                        var updatetitem = unitOfWork.EngagementQuestionSet.GetByID(a.Id);
                        updatetitem.Status = "Archived";
                        unitOfWork.EngagementQuestionSet.Update(updatetitem);
                        unitOfWork.Save();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }


        public static void FinalizeQuestionSet(EngagementQuestionSet getDraftVersionToFinalize, int usid, string viewLink, string surveyType, UserProfile username, string QSetNo)
        {


            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var updatetitem = unitOfWork.EngagementQuestionSet.GetByID(getDraftVersionToFinalize.Id);
                    updatetitem.Status = "Finalized";
                    updatetitem.LastUpdatedBy = usid; //change
                    updatetitem.LastUpdatedDate = DateTime.Now; //change
                    unitOfWork.EngagementQuestionSet.Update(updatetitem);
                    unitOfWork.Save();

                }

                new AssessmentController().SendFinaliseNotification(username.FirstName + " " + username.LastName, QSetNo, surveyType, viewLink);
            }
            catch (Exception ex)
            {

            }

        }




        public static IEnumerable<ActionItemTrackerModel> GetActionItems(int AssessmentId, int AssessmentTypeID, int AccountID/*, int ContractID*/, int ClientId)
        {
            IList<ActionItemTrackerModel> ActionItemList = new List<ActionItemTrackerModel>();
            var paramName = new string[] { "@AssessmentId", "@AssessmentTypeId", "@AccountID"/*, "@ContractID "*/, "@ClientId" };
            var paramValue = new object[] { AssessmentId, AssessmentTypeID, AccountID/*, ContractID*/, ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("A3MM_GetActionItemTrackerDetails", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ActionItemTrackerModel();
                    //coldet.ClientID = Convert.ToInt32(dr["ClientID"]);
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    coldet.AssessmentID = Convert.ToInt32(dr["AssessmentID"]);
                    coldet.Discipline_ID = Convert.ToInt32(dr["DiscplineID"]);
                    coldet.Component_ID = Convert.ToInt32(dr["ComponentID"]);
                    coldet.Aspect_ID = Convert.ToInt32(dr["AspectID"]);
                    coldet.DisciplineName = dr["DisciplineName"].ToString();
                    coldet.ComponentName = dr["ComponentName"].ToString();
                    coldet.AspectName = dr["AspectName"].ToString();
                    coldet.OwnerName = dr["OwnerName"].ToString();
                    coldet.ActionItem = new AssessmentController().GetPlainText(dr["ActionItem"].ToString());
                    coldet.AssessmentName = dr["AssessmentName"].ToString();
                    coldet.Status = dr["Status"].ToString();
                    coldet.Comments = dr["Comments"].ToString();
                    coldet.Benefits = dr["Benefits"].ToString();
                    coldet.ImplementationPlan = dr["ImplementationPlan"].ToString();
                    coldet.BusinessDriver = dr["BusinessDriver"].ToString();
                    coldet.AMSCatalog = dr["Catalog"].ToString();

                    if (!(dr["Priority"] is DBNull))
                    {
                        coldet.Priority = Convert.ToInt32(dr["Priority"]);
                    }
                    //else
                    //{
                    //    coldet.Priority = 0;
                    //}

                   

                    if (!(dr["PlannedCompletionDate"] is DBNull))
                    {
                        coldet.PlannedCompletionDate = Convert.ToDateTime(dr["PlannedCompletionDate"]);
                    }
                    else
                    {
                        coldet.PlannedCompletionDate = null;
                    }

                        
                    ActionItemList.Add(coldet);
                }

                return ActionItemList;
            }


        }




        public static int addorEditActionItem(ActionItemTrackerModel ait,int userid, ActionItemTracker updatetitem,string IdeaLink,string ClientName)
        {
            Db db = new Db();
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                   // var updatetitem = unitOfWork.ActionItemTracker.GetByID(ait.Id);
                    var updateString = "";


                    if (updatetitem.OwnerID != ait.OwnerID)
                    {
                        var owner1 = (from a in db.UserProfiles
                                      where a.Id == updatetitem.OwnerID
                                      select a.FirstName + " " + a.LastName).FirstOrDefault();

                        var owner2 = (from a in db.UserProfiles
                                      where a.Id == ait.OwnerID
                                      select a.FirstName + " " + a.LastName).FirstOrDefault();
                        if (owner1 == null || owner1 == "")
                        {
                            updateString = updateString + "Owner added - " + owner2 + " ";

                        }
                        else
                        {
                            updateString = updateString + "Owner changed from " + owner1 + " to " + owner2 + " ";
                        }
                    }
                    updatetitem.OwnerID = ait.OwnerID;

                    if (updatetitem.PlannedCompletionDate != ait.PlannedCompletionDate)
                    {
                        if (updatetitem.PlannedCompletionDate == null)
                        {
                            updateString = updateString + ", Planned Completion Date - " + ait.PlannedCompletionDate.Value.AddDays(0).ToString("MM/dd/yyyy") + "  ";

                        }
                        else
                        {
                            Log.Error("plannedCompDate : " + ait.PlannedCompletionDate);
                            var plannedCompletionDateString = ait.PlannedCompletionDate.Value.AddDays(0).ToString("MM/dd/yyyy");
                            //updateString = updateString + ", Planned Completion Date changed from " + updatetitem.PlannedCompletionDate + " to " + ait.PlannedCompletionDate + "  ";
                            updateString = updateString + ", Planned Completion Date changed from " + updatetitem.PlannedCompletionDate.Value.AddDays(0).ToString("MM/dd/yyyy")+ " to " + plannedCompletionDateString + "  ";
                        }
                    }
                    updatetitem.PlannedCompletionDate = ait.PlannedCompletionDate;

                    if (updatetitem.Status != ait.Status)
                    {
                        if (updatetitem.Status != null)
                        {
                            updateString = updateString + ", Status updated from " + updatetitem.Status + " to " + ait.Status + "  ";
                        }
                        else
                        {
                            updateString = updateString + ", Status - " + ait.Status + "  ";

                        }
                    }
                    updatetitem.Status = ait.Status;

                    if (updatetitem.Comments != ait.Comments)
                    {
                        if (updatetitem.Comments != null)
                        {
                            updateString = updateString + ", Comments updated from " + updatetitem.Comments + " to " + ait.Comments + "   ";
                        }
                        else
                        {
                            updateString = updateString + ", Comments - " + ait.Comments + "   ";

                        }
                    }
                    updatetitem.Comments = ait.Comments;

                    if (updatetitem.Benefits != ait.Benefits)
                    {
                        if (updatetitem.Benefits != null)
                        {
                            updateString = updateString + ", Benefits updated from " + updatetitem.Benefits + " to " + ait.Benefits + "   ";
                        }
                        else
                        {
                            updateString = updateString + ", Benefits - " + ait.Benefits + "   ";

                        }
                    }
                    updatetitem.Benefits = ait.Benefits;


                    unitOfWork.ActionItemTracker.Update(updatetitem);

                    id = updatetitem.Id;

                    if (updateString != "")
                    {
                        ActionItemTrackerLog alog = new ActionItemTrackerLog();
                        alog.ActionItemID = ait.Id;
                        alog.Updates = updateString;
                        alog.ClientID = ait.ClientID;
                        alog.updateDate = DateTime.Now;
                        unitOfWork.ActionItemTrackerLog.Insert(alog);
                    }
                    unitOfWork.Save();

                    //Update Idea manager
                    var Ideadt = unitOfWork.IMPostIdea.GetByID(updatetitem.IdeaId);
                    if (Ideadt != null)
                    {
                        if(ait.Benefits!= null)
                        {
                            var benefitid = unitOfWork.IMItemBusinessBenefits.Get(x => x.ItemId == Ideadt.Id).FirstOrDefault();
                            if (benefitid == null)
                            {
                                IMItemBusinessBenefits BB = new IMItemBusinessBenefits();
                                BB.Detail = ait.Benefits;
                                BB.Name = ait.ActionItem;
                                BB.ItemId = Ideadt.Id;
                                BB.CreatedDate = DateTime.Now;
                               // BB.CreatedBy =;
                                BB.LastUpdatedDate= DateTime.Now;
                                unitOfWork.IMItemBusinessBenefits.Insert(BB);
                                unitOfWork.Save();
                                updatetitem.BenefitId = BB.Id;
                                unitOfWork.ActionItemTracker.Update(updatetitem);
                                unitOfWork.Save();
                            }
                            else
                            {
                                benefitid.Detail= ait.Benefits;
                                benefitid.LastUpdatedDate= DateTime.Now;
                                unitOfWork.IMItemBusinessBenefits.Update(benefitid);
                                unitOfWork.Save();
                            }
                        }
                        Ideadt.StatusComment = ait.Comments;
                        Ideadt.LastUpdatedDate = DateTime.Now;
                        Ideadt.UpdatedBy = userid;
                        var up = unitOfWork.UserProfile.GetByID(Ideadt.CreatedBy);
                        Ideadt.StatusComment = ait.Comments;
                        if (ait.Status == "In Progress")
                        {
                            var isApproved=ApproveIdeaStatus(Ideadt,ait.OwnerID, userid);
                            Ideadt.ImpStatusId = 17;
                            Ideadt.PlannedEndDate=ait.PlannedCompletionDate;
                            Ideadt.ActualStartDate = DateTime.Now;
                        }
                        else if (ait.Status == "Completed")
                        {
                            var isApproved = ApproveIdeaStatus(Ideadt, ait.OwnerID, userid);
                            Ideadt.ImpStatusId = 4;
                            Ideadt.ImplementationProgress = 100;
                            Ideadt.ActualEndDate = DateTime.Now;
                            if (up != null)
                            {
                                new PostIdeaController().SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, Ideadt.ItemName, IMStatus.Implemented, ClientName, IdeaLink);
                            }
                        }
                        else if (ait.Status == "Hold")
                        {
                            var isapproved = (Ideadt.ImpStatusId == null) ? false : true;
                            if (isapproved)
                            {
                                Ideadt.ImpStatusId = 18;
                            }
                            else
                            {
                                Ideadt.StatusId = 18;
                            }
                            if (up != null)
                            {
                                new PostIdeaController().SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, Ideadt.ItemName, IMStatus.Hold, ClientName, IdeaLink);
                            }
                            
                        }
                        else if (ait.Status == "Cancelled")
                        {
                            var isapproved = (Ideadt.ImpStatusId == null) ? false : true;
                            if (isapproved)
                            {
                                Ideadt.ImpStatusId = 5;
                            }
                            else
                            {
                                Ideadt.StatusId = 6;
                            }
                            if (up != null)
                            {
                                new PostIdeaController().SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, Ideadt.ItemName, IMStatus.Cancelled, ClientName, IdeaLink);
                            }                            
                        }
                        unitOfWork.IMPostIdea.Update(Ideadt);
                        unitOfWork.Save();
                    }
                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        public static bool ApproveIdeaStatus(IMPostIdea Idea,int? OwnerId,int userid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var Ideastatusresult = checkIdeaStatus(Idea.Id);
            IMIdeaApprover ideaApprover = new IMIdeaApprover();
            if (Ideastatusresult== "SubmitForPlanning"|| Ideastatusresult == "Approved")
            {
                Idea.ImpStatusId = 16; //Planned
                unitOfWork.IMPostIdea.Update(Idea);
                unitOfWork.Save();
            }
            else
            {
                if ((Ideastatusresult == "Created"||Ideastatusresult == "SubmitForReview" || Ideastatusresult == "Revised")|| (Idea.ImpStatusId == null && Ideastatusresult=="Hold"))
                {
                    var getApprovers = unitOfWork.IMIdeaApprover.Get(x => x.IdeaId == Idea.Id).ToList();
                    var getOwner = getApprovers.Find(x => x.ApproverId == OwnerId.Value);
                    if (getOwner != null)
                    {
                        getOwner.ApprovedDate = DateTime.Now;
                        getOwner.UpdatedDt = DateTime.Now;
                        getOwner.UpdatedBy = userid;
                        unitOfWork.IMIdeaApprover.Update(getOwner);
                    }
                    else
                    {
                        ideaApprover.IdeaId = Convert.ToInt32(Idea.Id);
                        ideaApprover.ApproverId = OwnerId.Value;
                        ideaApprover.Createdby = userid;
                        ideaApprover.CreatedDt = DateTime.Now;
                        ideaApprover.ApprovedDate = DateTime.Now;
                        ideaApprover.UpdatedDt = DateTime.Now;
                        ideaApprover.UpdatedBy = userid;

                        Idea.ApprovedBy = userid;
                        Idea.ApprovedDt = DateTime.Now;
                        Idea.ImpStatusId = 16; //Planned
                        unitOfWork.IMIdeaApprover.Insert(ideaApprover);
                        unitOfWork.IMPostIdea.Update(Idea);
                        unitOfWork.Save();
                    }
                }
              
            }
            return true;
        }
            
        public static string checkActionIdeaStatus(ActionItemTrackerModel ait)
        {
            var Ideastatusresult = "";
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var updatetitem = unitOfWork.ActionItemTracker.GetByID(ait.Id);
                Ideastatusresult = checkIdeaStatus(updatetitem.IdeaId);                          
                return Ideastatusresult;
            }
        }

        public static string checkIdeaStatus(int? IdeaId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                {
                    var Ideastatusresult = "";
                    var ideadetail = unitOfWork.IMPostIdea.GetByID(IdeaId);
                    if (ideadetail.ImpStatusId != null)
                    {
                        var temp1 = (IMStatus)ideadetail.ImpStatusId.Value;
                        Ideastatusresult = temp1.ToString();
                    }
                    else
                    {
                        var temp = (IMStatus)ideadetail.StatusId;
                        Ideastatusresult = temp.ToString();
                    }
                    return Ideastatusresult;
                }
            }
        }

        public static bool DeletedraftQset(int QsetID, int usid)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //unitOfWork.EngagementQuestionSet.Delete(QsetID);
                var updatetitem = unitOfWork.EngagementQuestionSet.GetByID(QsetID);
                updatetitem.isdeleted = 1; //soft delete
                updatetitem.LastUpdatedDate = DateTime.Now;
                updatetitem.LastUpdatedBy = usid;
                unitOfWork.Save();
                return true;
            }
        }


        public static bool DeletedraftQuestions(string versionID, int surveyTypeID)
        {
            IList<ActionItemTrackerModel> ActionItemList = new List<ActionItemTrackerModel>();
            var paramName = new string[] { "@versionID", "@surveyTypeID" };
            var paramValue = new object[] { versionID, surveyTypeID };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.GetDataFromStoredProcedure("A3MM_deleteDraftQuestions", paramName, paramValue);
                return true;
            }

        }
        public static bool changeSubQuestionIdentifier(int Id, int DisciplineId, int ComponentId, int AspectID, string DisciplineName, string ComponentName, string AspectName,string updateSubQIdentifier, int subQId)
        {
            var paramName = new string[] { "@Id", "@DisciplineId", "@ComponentId", "@AspectID", "@DisciplineName", "@ComponentName", "@AspectName", "@updateSubQIdentifier", "@subQId" };
            var paramValue = new object[] { Id, DisciplineId, ComponentId, AspectID, DisciplineName, ComponentName, AspectName, updateSubQIdentifier,subQId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.GetDataFromStoredProcedure("A3MM_ChangeSubQuestionIdentifier", paramName, paramValue);
                return true;
            }

        }

        public static bool changeSubQuestion(int Id, int DisciplineId, int ComponentId, int AspectID, string DisciplineName, string ComponentName, string AspectName)
        {
            var paramName = new string[] { "@Id", "@DisciplineId", "@ComponentId", "@AspectID", "@DisciplineName", "@ComponentName", "@AspectName"};
            var paramValue = new object[] { Id, DisciplineId, ComponentId, AspectID, DisciplineName, ComponentName, AspectName };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.GetDataFromStoredProcedure("A3MM_ChangeSubQuestion", paramName, paramValue);
                return true;
            }

        }

        public static bool addIMIntegration(int clientid, int uid, int assessID, Assessment ass)
        {

            Db db = new Db();
            int LobId = AddLob(clientid, uid, ass.AccountID);
            int TeamId = AddTeam(clientid, uid, ass.AccountID,ass.SurveyTypeID);
            int complexityId = AddComplexity(clientid, uid);
            int createdby = ass.EngagementTeamPointOfContact;
            //int createdby = (from u in db.UserProfiles
            //                 where (u.Email.Trim().ToUpper() == ass.EngagementTeamPointOfContact.Trim().ToUpper()
            //                 || u.UserName.Trim().ToLower() == ass.AssessmentTeam.Trim().ToLower())
            //                 select u.Id).FirstOrDefault();
            //var paramName = new string[] { "@clientid", "@LobId", "@TeamId","@createdby", "@assessID", "@AssessmentName" };
            //var paramValue = new object[] { clientid, LobId, TeamId, createdby, assessID, AssessmentName };  
            var ActionItemSet = from ac in db.ActionItemTracker
                                where ac.AssessmentID == assessID
                                select ac;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                int A3mm_IdeaId = 0;
                foreach (var item in ActionItemSet)
                {
                    IMPostIdea idea = new IMPostIdea();
                    idea.ItemName = ass.AssessmentName+"-"+ item.ActionItem;
                    if (ass.SurveyTypeID == 1)
                    {
                        idea.IdeaSource = "NTT DATA_CESIVV";
                    }
                    else if (ass.SurveyTypeID == 2)
                    {
                        idea.IdeaSource = "NTT DATA_CESA3MM";
                    }
                    else
                    {
                        idea.IdeaSource = "NTT DATA_CESDA";
                    }
                    idea.InternalInitiative = true;
                    idea.TeamId = TeamId;
                    idea.Description = item.ActionItem;
                    idea.ComplexityId = complexityId;
                    idea.LOBId = LobId;
                    idea.CreatedBy = createdby;
                    idea.CreatedDate = DateTime.Now;//Need to do Offset.
                    idea.LastUpdatedDate = DateTime.Now;//Need to do Offset.
                    idea.StatusId = (int)IMStatus.Created;
                    idea.ClientId = clientid;
                    idea.StatusDt = DateTime.Now;
                    idea.MultipleApprovals = true;
                    idea.ActionitemId = item.Id;
                    unitOfWork.IMPostIdea.Insert(idea);
                    unitOfWork.Save();

                    if (item.OwnerID != null)
                    {
                        IMIdeaApprover ideaApprover = new IMIdeaApprover();
                        ideaApprover.IdeaId = Convert.ToInt32(idea.Id);
                        ideaApprover.ApproverId = item.OwnerID.Value;
                        ideaApprover.Createdby = uid;
                        ideaApprover.CreatedDt = DateTime.Now;
                        unitOfWork.IMIdeaApprover.Insert(ideaApprover);

                        unitOfWork.Save();
                    }


                    IMIdeaStatusHistory IMIdeaStatusHistory = new IMIdeaStatusHistory()
                    {
                        IdeaId = idea.Id,
                        NewStatusId = idea.StatusId,
                        IsDeleted = false,
                        CreatedBy = uid,
                        UpdatedBy = uid,
                        LastUpdatedDate = DateTime.Now,
                        CreatedDate = DateTime.Now
                    };
                    unitOfWork.IMIdeaStatusHistory.Insert(IMIdeaStatusHistory);

                    unitOfWork.Save();
                    A3mm_IdeaId = idea.Id;
                    ActionItemTracker ait1 = unitOfWork.ActionItemTracker.GetByID(item.Id);
                    ait1.IdeaId = A3mm_IdeaId;
                    unitOfWork.ActionItemTracker.Update(ait1);
                    unitOfWork.Save();

                }

            }




            return true;
        }
        public static IList<A3MMModel> getMaturityLevelCompare_drilldown(int clientid, string ComponentName, int Component_ID, string Assessmentids, string AssessmentName, int avg_MaturityLevel)
        {
            IList<A3MMModel> disciplineList = new List<A3MMModel>();


            var paramName = new string[] { "ClientId", "Component_ID ", "Assessmentids", "AssessmentName" };
            var paramValue = new object[] { clientid, Component_ID, Assessmentids, AssessmentName };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("A3MM_MaturityLevelCompare_drilldown", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new A3MMModel();
                    if (!(dr["Component_ID"] is DBNull))
                        coldet.ComponentId = Convert.ToInt32(dr["Component_ID"]);
                    else
                        coldet.ComponentId = 0;
                    if (!(dr["Discipline_ID"] is DBNull))
                        coldet.DisciplineId = Convert.ToInt32(dr["Discipline_ID"]);
                    else
                        coldet.DisciplineId = 0;
                    if (!(dr["SurveyTypeID"] is DBNull))
                        coldet.SurveyTypeID = Convert.ToInt32(dr["SurveyTypeID"]);
                    else
                        coldet.SurveyTypeID = 0;
                    if (!(dr["assessmentid"] is DBNull))
                        coldet.AssessmentID = Convert.ToInt32(dr["assessmentid"]);
                    else
                        coldet.AssessmentID = 0;
                    coldet.ComponentName = dr["componentname"].ToString();
                    coldet.AssessmentName = dr["AssessmentName"].ToString();
                    coldet.Response = dr["ActionItem"].ToString();
                    coldet.AspectName = dr["AspectName"].ToString();
                    coldet.MaturityLevel = dr["avg_MaturityLevel"].ToString();
                    disciplineList.Add(coldet);
                }

                return disciplineList;
            }

        }
        public static int AddLob(int clientid,int uid, int AccountID)
        {
            using (var contextFactory = new DbContextFactory())
            {
                int Lobid=0;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var accName = unitOfWork.ClientAccounSetup.GetByID(AccountID).AccountName;               
                var LOBList = unitOfWork.IMLOB.Get(x => x.ClientId == clientid).ToList();
                var isAccExist = LOBList.FindAll(x => x.LOB.ToLower() == accName.ToLower()).Any();
                if (!isAccExist)
                {
                    var Lob = new IMLOB();
                    Lob.ClientId = clientid;
                    Lob.LOB = accName;
                    Lob.CreatedDate = DateTime.Now;
                    Lob.CreatedBy = uid;
                    unitOfWork.IMLOB.Insert(Lob);
                    unitOfWork.Save();
                    Lobid = Lob.Id;                   
                }
                else
                {
                    Lobid= unitOfWork.IMLOB.Get(x => x.ClientId == clientid && x.LOB.ToLower() == accName.ToLower()).FirstOrDefault().Id;
                }
                return Lobid;               
            }
        }
        public static int AddTeam(int clientid, int uid, int AccountID,int SurveyTypeId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                int Teamid=0;
                var Source = "";
                if (SurveyTypeId == 1)
                {
                    Source = "NTT DATA_CESIVV";
                }
                else if (SurveyTypeId == 2)
                {
                    Source = "NTT DATA_CESA3MM";
                }
                else
                {
                    Source = "NTT DATA_CESDA";
                }
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var accName = unitOfWork.ClientAccounSetup.GetByID(AccountID).AccountName;
                var TeamList = unitOfWork.IMTeam.Get(x => x.ClientId == clientid).ToList();
                var isTeamExist = TeamList.FindAll(x => x.Team.ToLower() == accName.ToLower() && x.IdeaSourceVal== Source).Any();
                if (!isTeamExist)
                {
                    var tea = new IMTeam();
                    tea.ClientId = clientid;
                    tea.Team = accName;
                    tea.IdeaSourceVal = Source;
                    tea.CreatedDt = DateTime.Now;
                    tea.CreatedBy = uid;
                    tea.IdeaSourceVal = Source;
                    unitOfWork.IMTeam.Insert(tea);
                    unitOfWork.Save();
                    Teamid = tea.Id;                   
                }
                else
                {
                    Teamid = unitOfWork.IMTeam.Get(x => x.ClientId == clientid && x.Team.ToLower() == accName.ToLower()&&x.IdeaSourceVal== Source).FirstOrDefault().Id;
                }
                return Teamid;
            }
        }
        public static int AddComplexity(int clientid, int uid)
        {
            using (var contextFactory = new DbContextFactory())
            {
                int complexityId = 0;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var complexity = unitOfWork.IMComplexity.Get(w => w.ClientId == clientid && w.Complexity.ToLower() == "medium").FirstOrDefault();
             
                if (complexity == null)
                {
                    var tea = new IMComplexity();
                    tea.ClientId = clientid;
                    tea.Complexity = "Medium";
                    tea.CreatedDt = DateTime.Now;
                    tea.CreatedBy = uid;
                    unitOfWork.IMComplexity.Insert(tea);
                    unitOfWork.Save();
                    complexityId = tea.Id;

                }
                else
                {
                    complexityId = complexity.Id;
                }
                return complexityId;
            }
        }


        public static int saveFollowupQuestion(int ResponseID, int subresponseid, int AssessmentId, string followup)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    A3MM_MultipleFollowUpQuestions res = new A3MM_MultipleFollowUpQuestions();
                    res.ResponseID = ResponseID;
                    res.SubResponseId = subresponseid;
                    res.SubFollowUpQuestion = followup;
                    res.AssessmentId = AssessmentId;
                    res.SubFollowUpQOrder=unitOfWork.A3MM_MultipleFollowUpQuestions.Get(x => x.ResponseID == ResponseID && x.AssessmentId == AssessmentId).Count()+1;
                    unitOfWork.A3MM_MultipleFollowUpQuestions.Insert(res);
                    unitOfWork.Save();
                    id = res.Id;

                    var updateitem2 = unitOfWork.QuestionnaireResponse.GetByID(ResponseID);
                    if (updateitem2.responseStatus == "Responded"|| updateitem2.responseStatus == "RatingInProgress" || updateitem2.responseStatus == "Completed")
                    {
                        updateitem2.responseStatus = "ResponseInProgress";
                        unitOfWork.QuestionnaireResponse.Update(updateitem2);
                    }

                    unitOfWork.Save();
                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
       
        public static int saveEngagementResponse(int QuestionResponseID, string SQNo,string Response, int usid, int AssessmentID)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    A3MM_MultipleResponses res = new A3MM_MultipleResponses();
                    res.ResponseID = QuestionResponseID;
                    res.SubResponse = Response;
                    res.SQNo = SQNo;
                    res.AssessmentId = AssessmentID;
                    res.SubResponseOrder=unitOfWork.A3MM_MultipleResponses.Get(x => x.ResponseID == QuestionResponseID && x.AssessmentId == AssessmentID).Count()+1;
                    unitOfWork.A3MM_MultipleResponses.Insert(res);
                    unitOfWork.Save();
                    id = res.Id;


                    var updateitem2 = unitOfWork.Assessment.GetByID(AssessmentID);
                     if (updateitem2.Status == "Initiated")
                    {
                        updateitem2.Status = "Analysis";
                        unitOfWork.Assessment.Update(updateitem2);
                    }
                   
                    var updateitem3 = unitOfWork.QuestionnaireResponse.GetByID(QuestionResponseID);
                    if (updateitem3.responseStatus == "Unanswered")
                    {
                        updateitem3.responseStatus = "ResponseInProgress";
                        unitOfWork.QuestionnaireResponse.Update(updateitem3);
                    }
                   
                    unitOfWork.Save();




                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        public static int UpdateEngagementResponse(int Id, string Response, int usid)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.A3MM_MultipleResponses.GetByID(Id);
                    updatetitem.SubResponse = Response;
                    unitOfWork.A3MM_MultipleResponses.Update(updatetitem);
                    unitOfWork.Save();
                    id = updatetitem.Id;   
                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        
        public static int UpdateFollowupResponse(int Id, string FollowVal, int usid)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.A3MM_MultipleFollowUpQuestions.GetByID(Id);
                    updatetitem.SubFollowUpQuestion= FollowVal;
                    unitOfWork.A3MM_MultipleFollowUpQuestions.Update(updatetitem);
                    unitOfWork.Save();
                    id = updatetitem.Id;   
                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }


        public static int saveEngagementSelfAssessment(int responseID, string rating, string comments, bool markAsCompletecheckBox)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    QuestionnaireResponse res = unitOfWork.QuestionnaireResponse.GetByID(responseID);
                    res.EngagementRating = rating;
                    res.EngagementComments = comments;

                    if (markAsCompletecheckBox == true)
                    {
                        res.responseStatus = "Responded";
                    }


                    unitOfWork.QuestionnaireResponse.Update(res);
                    unitOfWork.Save();
                    id = res.Id;

                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }



        public static int saveAssessRating(int responseID, string rating, string AssessmentResponse, string AreasOfImprovement, int impPlanID,bool MarkAsCompletecheckBox, int busDriverID, int priority, int amsCatalogID, string Status,int clientID,int? disciplineID,int? componentID,int? aspectID,int ActionItemID)
        {

            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    QuestionnaireResponse res = unitOfWork.QuestionnaireResponse.GetByID(responseID);
                    res.MaturityLevel = rating.ToString();
                    res.AssessmentResponse = AssessmentResponse;
                    res.AreaOfImprovement = AreasOfImprovement;
                    if (MarkAsCompletecheckBox == true)
                    {
                        res.responseStatus = "Completed";
                    }
                    else
                    {
                        res.responseStatus = "RatingInProgress";
                    }

                    var assessmentID = res.AssessmentID;

                    if(Status == "Analysis" || Status == "Review")
                    {
                        if (AreasOfImprovement != "" && AreasOfImprovement != null)
                        {

                            AreasOfImprovement = Regex.Replace(AreasOfImprovement, @"<[^>]+>|&nbsp;", "").Trim();
                            //AreasOfImprovement = Regex.Replace(AreasOfImprovement, @"\s{2,}", " ");
                            if (AreasOfImprovement != null && ActionItemID != 0)
                                {
                                    ActionItemTracker ait1 = unitOfWork.ActionItemTracker.GetByID(ActionItemID);
                                    ait1.ActionItem = AreasOfImprovement;
                                    ait1.ImplementationPlan_ID = impPlanID;
                                    ait1.Priority = priority;
                                    ait1.BusinessDriver_ID = busDriverID;
                                    ait1.Catalog_ID = amsCatalogID;
                                    unitOfWork.ActionItemTracker.Update(ait1);
                                    unitOfWork.Save();

                                }
                                else
                                {
                                    ActionItemTracker ait = new ActionItemTracker();
                                    ait.AssessmentID = assessmentID;
                                    ait.Discipline_ID = Convert.ToInt32(disciplineID);
                                    ait.Component_ID = Convert.ToInt32(componentID);
                                    ait.Aspect_ID = Convert.ToInt32(aspectID);
                                    ait.ClientID =clientID;
                                    ait.ActionItem = AreasOfImprovement;
                                    ait.Status = "Defined";
                                    ait.QResID = responseID;
                                if (impPlanID != 0)
                                {
                                    ait.ImplementationPlan_ID = impPlanID;
                                }
                                if (priority != 0)
                                {
                                    ait.Priority = priority;
                                }
                                if (busDriverID != 0)
                                {
                                    ait.BusinessDriver_ID = busDriverID;
                                }
                                if (amsCatalogID != 0)
                                {
                                    ait.Catalog_ID = amsCatalogID;
                                }

                                unitOfWork.ActionItemTracker.Insert(ait);
                                    unitOfWork.Save();
                                }
                            }                      
                    }

                    unitOfWork.QuestionnaireResponse.Update(res);
                    unitOfWork.Save();
                    id = res.Id;

                    return id;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

    }
}