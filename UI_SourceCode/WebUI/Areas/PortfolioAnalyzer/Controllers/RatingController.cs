using Data;
using Domain.Entities.Survey;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.Dto;
using WebUI.Areas.RetirementAccelerator.Models;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class RatingController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/Rating/
        
        public ActionResult Index()
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var RatingUser = unitOfWork.Personnel.Get(g => g.PersonnelName.ToLower() == "aprratinguser").FirstOrDefault();
            //Or should add Client Name.... like 'Client'+'RatingUser'/ ClientId
            if (RatingUser == null)
            {
                Personnel entity = new Personnel()
                {
                    PersonnelName = "APRRatingUser",
                    FirstName = "APR",
                    LastName = "RatingUser"
                };
                unitOfWork.Personnel.Insert(entity);
                unitOfWork.Save();
            }
            return View();
        }

        public ActionResult RatingsWithExport()
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var RatingUser = unitOfWork.Personnel.Get(g => g.PersonnelName.ToLower() == "aprratinguser").FirstOrDefault();
            //Or should add Client Name.... like 'Client'+'RatingUser'/ ClientId
            if (RatingUser == null)
            {
                Personnel entity = new Personnel()
                {
                    PersonnelName = "APRRatingUser",
                    FirstName = "APR",
                    LastName = "RatingUser"
                };
                unitOfWork.Personnel.Insert(entity);
                unitOfWork.Save();
            }
            return View();
        }

        public ActionResult AssessRead([DataSourceRequest] DataSourceRequest request, int SurveyId = 0, int ApplicationId = 0)
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            IEnumerable<AssessmentModel> model = new List<AssessmentModel>();
            if (SurveyId != 0 )//&& ApplicationId != 0)
            {
                model = PortfolioAnalyzerHelper.FetchAssessDetails(SurveyId, ApplicationId, client.ClientId, uid);
            }
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult DynamicAssessRead([DataSourceRequest] DataSourceRequest request, int SurveyId = 0, int ApplicationId = 0)
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            IEnumerable<AssessmentModel> model = new List<AssessmentModel>();
            if (SurveyId != 0)//&& ApplicationId != 0)
            {
                model = PortfolioAnalyzerHelper.FetchDynamicAssessDetails(SurveyId, ApplicationId, client.ClientId, uid);
            }
            return Json(model.ToDataSourceResult(request));
        }
        
        public ActionResult GetDynamicSurveyList()
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.Survey.Get(x => x.ClientId == ClientId && (x.SurveyType.ToLower() == "dynamic" || x.SurveyType.ToLower() == "apr" || x.SurveyType.ToLower() == "apr2")).ToList();
            return Json(applicationList.ToList().OrderBy(x => x.Name).Select(A => new { Value = A.Id.ToString(), Text = A.Name }), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSurveyList()
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.Survey.Get(x => x.ClientId == ClientId && (x.SurveyType.ToLower() == "dynamic" || x.SurveyType.ToLower() == "apr" || x.SurveyType.ToLower() == "apr2")).ToList();
            return Json(applicationList.ToList().OrderBy(x => x.Name).Select(A => new { Value = A.Id.ToString(), Text = A.Name }), JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// For Start rating type of Survey
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AssessAjaxSave(RatingJSObject rating)
        {
            int uid = (int)Session["UserId"];
            var ratList = rating.RatingsList.ToList()[0].RowId;
            var SurUsrId = PortfolioAnalyzerHelper.GetSurUserIdFromSCId(ratList.Value);
            if (rating != null && rating.RatingsList != null && ModelState.IsValid)
            {
                foreach (var Q in rating.RatingsList)
                {
                    if (Q.RowId != null && Q.RowId != 0)
                    {
                        AssessmentModel model = new AssessmentModel();
                        model.Id = Q.RowId.Value;
                        model.OriginalRating = Q.OriginalRating;
                        model.FinalRating = Q.FinalRating;
                        model.Comments = Q.Comments;
                        PortfolioAnalyzerHelper.AssessUpdate(model);
                        //To kepp track -when was the ratings got updated- To match 
                        //with the date ratings given by SurevyParticipants
                        //AND show the latest userRAtings(OverRiding the generic users ratings)
                    }
                }
                PortfolioAnalyzerHelper.SurveyUserUpdateDate(SurUsrId);
            }
            return Json(rating);
        }

        ///// <summary>
        ///// For DropDown type survey 
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="surveyCollection"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult AssessSave([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AssessmentModel> surveyCollection)
        //{
        //    if (surveyCollection != null && ModelState.IsValid)
        //    {
        //        foreach (var product in surveyCollection)
        //        {
        //            PortfolioAnalyzerHelper.AssessUpdate(product);
        //        }
        //    }
        //    return Json(surveyCollection.ToDataSourceResult(request, ModelState));
        //}

        [HttpPost]
        public ActionResult CheckSurveyType(int SurveyId = 0)
        {
            var xtemp = PortfolioAnalyzerHelper.GetSurveyType(SurveyId);
            if (xtemp != null)
            {
                return Json(new { success = xtemp.SurveyType.Trim().ToLower() });
            }
            else
            {
                return Json(new { failure = "Please Select Survey!" });
            }

        }

        ////[HttpPost]
        ////public ActionResult Excel_Export_All(int surveyid)
        ////{
        ////    var client = (Client)Session["Client"];
        ////    // return PortfolioAnalyzerHelper.GetAllSurveyDetail(surveyid,client.Id);
        ////    return null;
        ////  //,  return WebUI.App_Start.Helper.GetXLFileStream();// (fileContents, contentType, fileName);
        ////}


        //Extract All
        // http://localhost:3612/PortfolioAnalyzer/Rating/ExtractSurvey?SurveyId=1118
        public ActionResult Excel_Export_All(int SurveyId = 0)

        {
            // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            string fileName = "Survey_All" + ".xlsx";

            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(GetCollection(SurveyId));
            stream.Position = 0;
            string mimeType = "application/Survey.ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;

        }
        public ActionResult Excel_Export(int SurveyId = 0, int Appid = 0)

        {
            // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            string fileName = "SurveyExtract" + ".xlsx";

            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(GetCollection(SurveyId, Appid));
            stream.Position = 0;
            string mimeType = "application/Survey.ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;

        }

        private System.Data.DataTable GetCollection(int surveyId)
        {
            List<SurveyCollectionModel> surveyCollection = new List<SurveyCollectionModel>();
            // Db db = new Db();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            // var RatingUser = unitOfWork.Personnel.Get(g => g.PersonnelName.ToLower() == "aprratinguser").FirstOrDefault();
            var paramName = new string[] { "surveyid" };
            var paramValue = new object[] { surveyId };
            var res = unitOfWork.GetDataFromStoredProcedure("ExtractSurveyResponses", paramName, paramValue);
            foreach (DataRow dr1 in res.Rows)
            {
                var sc = new SurveyCollectionModel();
                sc.Comments = dr1["Comments"].ToString();
                sc.SurveyForName = dr1["ApplicationName"].ToString();
                sc.Hint = dr1["Hint"].ToString();
                // sc.Question = dr1[].ToString();
                // sc.SurveyUserId = Convert.ToInt32(dr1["SurveyUserId"]);
                sc.Answer = dr1["RatingFactor"].ToString();
                sc.OriginalRatingAnswer = dr1["OriginalRatingFactor"].ToString();
                surveyCollection.Add(sc);
            }
            var surveyname = unitOfWork.Survey.GetByID(surveyId).Name;
            var QuestionList = surveyCollection.Select(x => x.Hint).Distinct().ToList();
            System.Data.DataTable dtConsolidatedTable = new System.Data.DataTable();
            dtConsolidatedTable.Columns.Add(surveyname);
            dtConsolidatedTable.Columns.Add("Application Name");

            foreach (var question in QuestionList)
            {
                if (!string.IsNullOrEmpty(question))
                {
                    dtConsolidatedTable.Columns.Add(question);
                }
            }
            //Get all applications for client
            var distinctapps = surveyCollection.Select(s => s.SurveyForName).Distinct().ToList();
            var SurveyType = unitOfWork.Survey.GetByID(surveyId).SurveyType;
            if (SurveyType.ToLower() == "apr2")
            {
                foreach (var app in distinctapps)
                {
                    System.Data.DataRow dr = dtConsolidatedTable.Rows.Add();
                    dr["Application Name"] = app;
                    foreach (var appSurvey in surveyCollection.Where(w => w.SurveyForName == app))
                    {
                        if (!string.IsNullOrEmpty(appSurvey.Hint))
                        {
                            dr[appSurvey.Hint] = appSurvey.Answer + "\n || Comments: " + appSurvey.Comments;
                        }

                    }
                }
            }
            else
            {
                foreach (var app in distinctapps)
                {
                    System.Data.DataRow dr = dtConsolidatedTable.Rows.Add();
                    dr["Application Name"] = app;
                    foreach (var appSurvey in surveyCollection.Where(w => w.SurveyForName == app))
                    {
                        if (!string.IsNullOrEmpty(appSurvey.Hint))
                        {
                            dr[appSurvey.Hint] = "Original Rating : " + appSurvey.OriginalRatingAnswer + "|| Final Rating : " + appSurvey.Answer + "\n || Comments: " + appSurvey.Comments;
                        }
                    }
                }
            }
            return dtConsolidatedTable;
        }

        private System.Data.DataTable GetCollection(int surveyId, int Appid)
        {
            List<SurveyCollectionModel> surveyCollection = new List<SurveyCollectionModel>();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var paramName = new string[] { "surveyid" };
            var paramValue = new object[] { surveyId };
            var res = unitOfWork.GetDataFromStoredProcedure("ExtractSurveyResponses", paramName, paramValue);
            foreach (DataRow dr1 in res.Rows)
            {
                var sc = new SurveyCollectionModel();
                if (Convert.ToInt32(dr1["Appid"]) == Appid)
                {
                    sc.SurveyForName = dr1["ApplicationName"].ToString();
                    sc.Comments = dr1["Comments"].ToString();
                    sc.SurveyForName = dr1["ApplicationName"].ToString();
                    sc.Hint = dr1["Hint"].ToString();
                    sc.Question = dr1["Question"].ToString();
                    sc.Answer = dr1["FinalRating"].ToString();// dr1["RatingFactor"].ToString(); 
                    sc.OriginalRatingAnswer = dr1["OriginalRatingFactor"].ToString();
                    surveyCollection.Add(sc);
                }
            }
            var surveyname = unitOfWork.Survey.GetByID(surveyId).Name;
            var AppName = surveyCollection.Select(x => x.SurveyForName).FirstOrDefault();
            System.Data.DataTable dtConsolidatedTable = new System.Data.DataTable();
            var SurveyType = unitOfWork.Survey.GetByID(surveyId).SurveyType;
            if (SurveyType.ToLower() == "apr2")
            {
                dtConsolidatedTable.Columns.Add(surveyname + " - " + AppName);
                dtConsolidatedTable.Columns.Add("Hint");
                dtConsolidatedTable.Columns.Add("Question");
                dtConsolidatedTable.Columns.Add("Responses");
                dtConsolidatedTable.Columns.Add("Comments");
                foreach (var app in surveyCollection)
                {
                    System.Data.DataRow dr = dtConsolidatedTable.Rows.Add();
                    dr["Hint"] = app.Hint;
                    dr["Question"] = app.Question;
                    dr["Responses"] = app.Answer;
                    dr["Comments"] = app.Comments;
                }
            }
            else
            {
                dtConsolidatedTable.Columns.Add(surveyname + " - " + AppName);
                dtConsolidatedTable.Columns.Add("Hint");
                dtConsolidatedTable.Columns.Add("Question");
                dtConsolidatedTable.Columns.Add("Original Rating");
                dtConsolidatedTable.Columns.Add("Final Rating");
                dtConsolidatedTable.Columns.Add("Comments");
                foreach (var app in surveyCollection)
                {
                    System.Data.DataRow dr = dtConsolidatedTable.Rows.Add();
                    dr["Hint"] = app.Hint;
                    dr["Question"] = app.Question;
                    dr["Original Rating"] = app.OriginalRatingAnswer;
                    dr["Final Rating"] = app.Answer;
                    dr["Comments"] = app.Comments;
                }
            }
            return dtConsolidatedTable;
        }




    }
}
