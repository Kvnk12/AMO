using Domain.Entities.ClientProfileManager;
using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.Dto;
using WebUI.Areas.ClientProfileManager.ViewModel;
using NTTData.AMOPortal.WebUI.Filters;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.Data;
using Data;
using Domain.Entities.Survey;
using WebUI.Areas.RetirementAccelerator.Models;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class INUserSurveyController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(int nextgensurveyId, string userSurveyToken)
        {

            var su = ValidateSurveyUser(nextgensurveyId, userSurveyToken);
            if (su == null)
            {
                ViewBag.Message = "Thanks for showing interest in taking this survey. Either survey is expired or invalid survey access or You are no longer participant of this survey.";
                
                return View("SubmitSurvey");
            }
            else if (su.completedDt != null)
            {
                ViewBag.Message = "You have already completed the Survey!";
                return View("SubmitSurvey");
                
            }

           
                return View(su);
           
        }

        private IEnumerable<SurveyCollectionModel> GetCollection(int surveyId, int SurveyPersonnel)
        {

            Db db = new Db();
            var SurUserrole = (from spr in db.PersonnelRole where spr.PersonnelId == SurveyPersonnel select spr).FirstOrDefault();
            var surveyCollection = (from su in db.SurveyUser
                                    join sm in db.SurveyModel on su.SurveyId equals sm.SurveyId
                                    join sp in db.SurveyPerspective on sm.PerspectiveTypeId equals sp.Id
                                    join pc in db.PerspectiveCategory on sp.PerspectiveCategoryId equals pc.Id

                                    join sc in db.SurveyCollection on sm.Id equals sc.SurveyModelId into tmpSC
                                    join smf in db.SurveyModelRatingFactor on sm.Id equals smf.SurveyModelId into tmpSMF
                                    where su.PersonnelId == SurveyPersonnel && (SurUserrole.IsSurveyParticipant.IndexOf(pc.PerspectiveCategoryName) >= 0 || SurUserrole.IsSurveyParticipant == "Both" || SurUserrole.IsSurveyParticipant == "")
                                    from LSC in tmpSC.DefaultIfEmpty()
                                    from LSMF in tmpSMF.DefaultIfEmpty()

                                    select new SurveyCollectionModel
                                    {
                                        Comments = LSC.Comments,
                                        FinalRating = LSC.FinalRating.Value,
                                        Question = sm.Question,
                                        SurveyModelId = sm.Id,
                                        Hint = sm.Hint,
                                        SurveyUserId = su.Id,
                                        qa = new QA { qid = sm.Id, answerid = LSC.FinalRating.Value }
                                    }
                                    ).ToList();

            return surveyCollection;
        }

     
        public IEnumerable<AssessmentModel> GetCollection2(int SurveyId, string emailid, int AppId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<AssessmentModel> returnList = new List<AssessmentModel>();
            Db db = new Db();
            var SurUserID = 0;
            if (SurveyId != 0 && AppId != 0 && !string.IsNullOrEmpty( emailid) )
            {
                var SurUser = unitOfWork.SurveyINUser.Get(g => g.IncidentId == AppId && g.SurveyUserEmail == emailid && g.SurveyId == SurveyId).FirstOrDefault();
                //var SurUserrole = unitOfWork.PersonnelRole.Get(g => g.em == PersonnelId && g.AppId == AppId).FirstOrDefault();
                if (SurUser != null)
                {
                    SurUserID = SurUser.Id;
                }
                if (SurUserID != 0)
                {
                    var IsRatingThere = unitOfWork.SurveyCollection.Get(g => g.SurveyUserId == SurUserID).Any();
                    if (IsRatingThere)
                    {
                        //With Linq query
                        var tempList = (from SM in db.SurveyModel
                                        join sp in db.SurveyPerspective on SM.PerspectiveTypeId equals sp.Id
                                        join pc in db.PerspectiveCategory on sp.PerspectiveCategoryId equals pc.Id
                                     //   where SM.SurveyId == SurveyId && (SurUserrole.IsSurveyParticipant.IndexOf(pc.PerspectiveCategoryName) >= 0 || SurUserrole.IsSurveyParticipant == "Both" || SurUserrole.IsSurveyParticipant == "")
                                        join ASS in db.SurveyCollection on SM.Id equals ASS.SurveyModelId into tempAssess
                                        from Assess in tempAssess.DefaultIfEmpty()
                                        where Assess.SurveyUserId == SurUserID
                                        select new
                                        {
                                            ApplicationId = AppId,
                                            subPerspective = SM.QuestionIdentifier,
                                            Question = SM.Question,
                                            OriginalRating = Assess.OriginalRating,
                                            FinalRating = Assess.FinalRating,
                                            Comments = Assess.Comments,
                                            Hint = SM.Hint,
                                            Options = (from op in db.SurveyModelRatingFactor
                                                       where op.SurveyModelId == SM.Id
                                                       orderby op.RatingFactorValue descending
                                                       select new
                                                       {
                                                           RatingText = op.RatingFactor,
                                                           RatingId = op.Id
                                                       }),
                                            Id = Assess.Id
                                        }

                            ).ToList();

                        var returnTempList = tempList.AsEnumerable().Select(e => new AssessmentModel
                        {
                            Id = e.Id,
                            subPerspective = e.subPerspective,
                            Question = e.Question,
                            OriginalRating = e.OriginalRating,
                            FinalRating = e.FinalRating,
                            Comments = e.Comments,
                            Hint = e.Hint,
                            QuestionOptions = e.Options.ToList().Select(f => new ratingMod
                            {
                                Id = f.RatingId,
                                Rating = f.RatingText
                            }),
                            RAApplicationid = e.ApplicationId
                        });
                        returnList = returnTempList;
                    }
                    else
                    {
                        var tempList = (from SM in db.SurveyModel
                                        where SM.SurveyId == SurveyId
                                        select new
                                        {
                                            SurveyModelId = SM.Id
                                        }

                            ).ToList();

                        //After fetching the SurveyQuestions, write it to database.
                        foreach (var ra in tempList)
                        {
                            var entity = new SurveyCollection();
                            entity.SurveyUserId = SurUserID;
                            entity.SurveyModelId = ra.SurveyModelId;
                            //entity.OriginalRating = ra.OriginalRating;
                            //entity.FinalRating = ra.FinalRating;
                            //entity.Comments = ra.Comments;
                            entity.Createddt = DateTime.Now;
                            entity.updateddt = DateTime.Now;
                            unitOfWork.SurveyCollection.Insert(entity);
                        }
                        unitOfWork.Save();

                        //Read agan with joins to Questions and Ratings
                        var AssessmentList = (from SM in db.SurveyModel
                                              join sp in db.SurveyPerspective on SM.PerspectiveTypeId equals sp.Id
                                              join pc in db.PerspectiveCategory on sp.PerspectiveCategoryId equals pc.Id
                                            //  where SM.SurveyId == SurveyId && (SurUserrole.IsSurveyParticipant.IndexOf(pc.PerspectiveCategoryName) >= 0 || SurUserrole.IsSurveyParticipant == "Both" || SurUserrole.IsSurveyParticipant == "")
                                              join ASS in db.SurveyCollection on SM.Id equals ASS.SurveyModelId into tempAssess
                                              from Assess in tempAssess.DefaultIfEmpty()
                                              where Assess.SurveyUserId == SurUserID
                                              select new
                                              {
                                                  ApplicationId = AppId,
                                                  subPerspective = SM.QuestionIdentifier,
                                                  SurveyModelId = SM.Id,
                                                  Question = SM.Question,
                                                  OriginalRating = Assess.OriginalRating,
                                                  FinalRating = Assess.FinalRating,
                                                  Hint = SM.Hint,
                                                  Comments = Assess.Comments,
                                                  Options = (from op in db.SurveyModelRatingFactor
                                                             where op.SurveyModelId == SM.Id
                                                             orderby op.RatingFactorValue descending
                                                             select new
                                                             {
                                                                 RatingText = op.RatingFactor,
                                                                 RatingId = op.Id
                                                             }),
                                                  Id = Assess.Id
                                              }

                            ).ToList();

                        var returnTempList = AssessmentList.Select(e => new AssessmentModel
                        {
                            Id = e.Id,
                            subPerspective = e.subPerspective,
                            Question = e.Question,
                            OriginalRating = e.OriginalRating,
                            FinalRating = e.FinalRating,
                            Comments = e.Comments,
                            Hint = e.Hint,
                            QuestionOptions = e.Options.ToList().Select(f => new ratingMod
                            {
                                Id = f.RatingId,
                                Rating = f.RatingText
                            }),
                            RAApplicationid = e.ApplicationId
                        });
                        returnList = returnTempList;
                    }
                }
            }
            return returnList;
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdateSurvey([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<SurveyCollectionModel> surveyCollection)
        {

            //foreach (var sc in SubmitUserSurvey.surveyCollection)
            //{
            //    //
            //}
            return Json(request, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SubmitUserSurvey(UserINSurvey SubmitUserSurvey, string btnSubmit)
        {

            string strMessage = "";
            ModelState.Clear();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                foreach (var sc in SubmitUserSurvey.surveyCollection)
                {
                    var sce = unitOfWork.SurveyINCollection.Get(s => s.SurveyModelId == sc.SurveyModelId && s.SurveyINUserId == sc.SurveyUserId).FirstOrDefault();

                    if (sce != null)
                    {
                        sce.FinalRating = sc.FinalRating;
                        sce.Comments = SubmitUserSurvey.SurveyComments;//sc.Comments;
                        sce.updateddt = DateTime.Now;
                        unitOfWork.SurveyINCollection.Update(sce);
                        var surevyUser = unitOfWork.SurveyINUser.GetByID(SubmitUserSurvey.surveyCollection.FirstOrDefault().SurveyUserId);
                        surevyUser.SurveyComments = SubmitUserSurvey.SurveyComments;
                        unitOfWork.SurveyINUser.Update(surevyUser);

                    }
                    else
                    {
                        SurveyINCollection scen = new SurveyINCollection();
                        scen.FinalRating = sc.FinalRating;
                        scen.Comments = sc.Comments;
                        scen.Createddt = DateTime.Now;
                        scen.SurveyModelId = sc.SurveyModelId;
                        scen.SurveyINUserId = sc.SurveyUserId;
                        unitOfWork.SurveyINCollection.Insert(scen);
                        var surevyUser = unitOfWork.SurveyINUser.GetByID(SubmitUserSurvey.surveyCollection.FirstOrDefault().SurveyUserId);
                        surevyUser.SurveyComments = SubmitUserSurvey.SurveyComments;
                        unitOfWork.SurveyINUser.Update(surevyUser);
                       
                    }
                }
                if (btnSubmit.ToLower() == "submit" && SubmitUserSurvey != null)
                {
                    var surevyUser = unitOfWork.SurveyINUser.GetByID(SubmitUserSurvey.surveyCollection.FirstOrDefault().SurveyUserId);
                    surevyUser.Completedt = DateTime.Now;
                    //surevyUser.sur
                    //To kepp track -when was the ratings got updated- To match 
                    //with the date ratings given by SurevyParticipants
                    //AND show the latest userRAtings(OverRiding the generic users ratings)
                    surevyUser.updateddt = DateTime.Now;
                    surevyUser.SurveyComments = SubmitUserSurvey.SurveyComments;
                    unitOfWork.SurveyINUser.Update(surevyUser);
                }
                unitOfWork.Save();
            }

            if (btnSubmit.ToLower() == "save")
            {
                strMessage = "Your responses are updated successfully. Please submit to complete the Survey.";
                TempData.Add("ValidationSuccessMsg", strMessage);
                //var su = ValidateSurveyUser2(SubmitUserSurvey.SurveyId, SubmitUserSurvey.UserSurveyToken);
                return RedirectToAction("Index", new { nextgensurveyId = SubmitUserSurvey.SurveyId, userSurveyToken = SubmitUserSurvey.UserSurveyToken });
            }
            else
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var surevyUser = unitOfWork.SurveyINUser.GetByID(SubmitUserSurvey.surveyCollection.FirstOrDefault().SurveyUserId);
                surevyUser.SurveyStatus = "Survey Received";
                unitOfWork.SurveyINUser.Update(surevyUser);
                unitOfWork.Save();
                ViewBag.Message = "Thank you for participating in this survey.";
                return View("SubmitSurvey");
            }

        }

        [AllowAnonymous]
        public ActionResult GetAnswers(int questionId)
        {
            return Json(getAnswers(questionId), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetSurveyCollection([DataSourceRequest] DataSourceRequest request, int surveyId = 0, int SurveyPersonnel = 0, int ApplicationId = 0)
        {
            return Json(GetCollection(surveyId, SurveyPersonnel).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return Json(GetCollection(surveyId, SurveyPersonnel,ApplicationId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<SelectListItem> getAnswers(int questionId)
        {
            Db db = new Db();
            var ans = db.SurveyModelRatingFactor.Where(w => w.SurveyModelId == questionId).ToList();
            var selAnswers = ans.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.RatingFactor });
            return selAnswers;
        }


        private UserINSurvey ValidateSurveyUser(int surveyId, string token)
        {

            if (string.IsNullOrEmpty(token) || surveyId <= 0)
            {
                return null;
            }

            Db db = new Db();

            var sur = (from s in db.Survey 
                       join cl in db.Clients on s.ClientId equals cl.Id
                       join su in db.SurveyINUser on s.Id equals su.SurveyId
                       join inc in db.Incidents on su.IncidentId equals inc.Id
                       where (s.Id == surveyId
                       //&& (s.enddate >= DateTime.Now)
                       && su.UserSurveyToken == token)
                       //&& su.Completedt==null)
                       //join 
                       orderby s.Createddt descending
                       select new UserINSurvey
                       {
                           TicketId = su.TicketId,
                           ClientName = cl.ClientName,
                           ClientId=cl.Id,
                           SurveyType = s.SurveyType,
                           SurveyUserId = su.Id,
                           Description = inc.ShortDescription, //s.Description,
                           SurveyName = s.Name,
                           SurveyFor=su.INTitle,
                           emailid = su.SurveyUserEmail,
                           UserSurveyToken = su.UserSurveyToken,
                           SurveyId = s.Id,
                           SurveyComments=su.SurveyComments,
                          // IncidentId=su.IncidentId,
                           SurveyUserEmail=su.SurveyUserEmail,
                           SurveyUserName=su.SurveyUserName,
                           SurveyStatus =su.SurveyStatus,
                           surveyenddate = s.enddate,
                           completedDt = su.Completedt,
                           SurveyValidDays= s.EndDay,
                           SurveyCreatedDate=su.Createddt.Value,
                           SurveySentDate=su.SurveySentDate

                       }).FirstOrDefault();
            if (sur == null)
                return null;
            else
            {
                if (sur.SurveySentDate.Value.AddDays(sur.SurveyValidDays.Value)<=DateTime.Now || (sur.completedDt!=null))
                    return null;
                sur.surveyCollection = GetSurveyCollections(sur.SurveyId, sur.emailid, sur.SurveyUserId);

            }

            return sur;

        }

        private IList<SurveyCollectiondto> GetSurveyCollections(int surveyId, string emailid, int SurveyINUserId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var SurUser = unitOfWork.SurveyINUser.Get(g => g.SurveyId == surveyId && g.Id == SurveyINUserId).FirstOrDefault();
           // var SurUserrole = unitOfWork.PersonnelRole.Get(g => g.PersonnelId == SurveyPersonnel && g.AppId == SurUser.SurveyForRefId).FirstOrDefault();

            Db db = new Db();
            var surveyINCollection = (from su in db.SurveyINUser
                                    join sm in db.SurveyModel on su.SurveyId equals sm.SurveyId
                                    join sp in db.SurveyPerspective on sm.PerspectiveTypeId equals sp.Id
                                    join pc in db.PerspectiveCategory on sp.PerspectiveCategoryId equals pc.Id
                                    join sc in db.SurveyINCollection.Where(w => w.SurveyINUserId == SurveyINUserId) on sm.Id equals sc.SurveyModelId into tmpSC
                                    // join smf in db.SurveyModelRatingFactor on sm.Id equals smf.SurveyModelId into tmpSMF
                                    where (su.SurveyUserEmail == emailid
                                    && su.SurveyId == surveyId
                                    && su.Id == SurveyINUserId)
                                    from LSC in tmpSC.DefaultIfEmpty()
                                        //from LSMF in tmpSMF.DefaultIfEmpty()
                                    select new SurveyCollectiondto
                                    {
                                        Comments = LSC.Comments,
                                        FinalRating = LSC.FinalRating.Value,
                                        Question = sm.Question,
                                        SurveyModelId = sm.Id,
                                        Hint = sm.Hint,
                                        SurveyUserId = su.Id
                                        //   = su.Id,
                                        //  qa = new QA { qid = sm.Id, answerid = LSC.FinalRating.Value }
                                    }
                                    ).ToList();

            foreach (var item in surveyINCollection)
            {
                item.SurveyModelAnswers = getAnswers(item.SurveyModelId).ToList();
            }

            return surveyINCollection;
        }




        [AllowAnonymous]
        [HttpPost]
        public JsonResult AssessRead([DataSourceRequest] DataSourceRequest request, int SurveyId = 0, int SurveyPersonnel = 0, int ApplicationId = 0)
        {
            return Json(GetCollection2(SurveyId, "", ApplicationId).ToDataSourceResult(request));
        }

        public static void AssessUpdate(AssessmentModel model)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var exists = unitOfWork.SurveyINCollection.GetByID(model.Id);
            if (exists != null && (exists.OriginalRating != model.OriginalRating || exists.FinalRating != model.FinalRating || exists.Comments != model.Comments))
            {
                exists.OriginalRating = model.OriginalRating;
                exists.FinalRating = model.FinalRating;
                exists.updateddt = DateTime.Now;
                exists.Comments = model.Comments;
                unitOfWork.SurveyINCollection.Update(exists);
                unitOfWork.Save();
            }
        }
        public static void AssessCompletedUpdate(int SurveyUserId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var exists = unitOfWork.SurveyINUser.GetByID(SurveyUserId);
            if (exists != null)
            {
                exists.Completedt = DateTime.Now;
                exists.updateddt = DateTime.Now;
                unitOfWork.SurveyINUser.Update(exists);
                unitOfWork.Save();

            }
        }
        //New method for saving ratings out of grid saving Functionality
        [HttpPost]
        [AllowAnonymous]
        public ActionResult AssessAjaxSave(RatingJSObject rating)
        {
            var SurveyUserId = 0;
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
                        AssessUpdate(model);
                        SurveyUserId = Q.SurveyUserId;
                    }
                }
                if (rating.ButtonType == "save")
                {
                    return Json(rating.ButtonType);
                }
                else
                {
                    AssessCompletedUpdate(rating.SurUserId);
                    return Json(rating.ButtonType);
                }
            }
            return Json(0);
        }

        [AllowAnonymous]
        public ActionResult ThankYou()
        {
            return Content("Thank you for participating in this survey.");
        }


        public string getCltName(int SurId)
        {
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var c = unitOfWork.Survey.Get(g => g.Id == SurId).FirstOrDefault().ClientId;
                var ClientName= unitOfWork.Clients.GetByID(c).ClientName;
                string cname = ClientName;
                return cname;
            }
        }


    }
}
