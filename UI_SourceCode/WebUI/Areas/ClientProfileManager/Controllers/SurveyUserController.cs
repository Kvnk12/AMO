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
using WebUI.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    [AllowAnonymous]
    public class SurveyUserController : Controller
    {
       
        public ActionResult Index(int nextgensurveyId, string userSurveyToken)


        {

            var su = ValidateSurveyUser2(nextgensurveyId, userSurveyToken);

            if (su == null)
            {
                //ModelState.AddModelError("InvalidAccess","Either survey is expired or invalid survey accesss.");
                // return Content("Thanks for showing interest in taking this survey. Either survey is expired or invalid survey access or You are no longer participant of this survey.");
                ViewBag.Message = "Thanks for showing interest in taking this survey. Either survey is expired or invalid survey access or You are no longer participant of this survey.";

                return View("SubmitSurvey");
            }
            // else if (su.completedDt != null)
            else if (su.userSurveyApplications.Count() == 0)
                {
                    ViewBag.Message = "You have already completed the Survey!";
                    return View("SubmitSurvey");


                }


            //return View(su); 
            if (!su.SurveyType.StartsWith("APR2") && !su.SurveyType.StartsWith("Dynamic"))
            {
                return View("NewSurveyPage", su);
            }
            else
            {
                return View("UserSurvey", su);
            }
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
                                        Comments = LSC.Comments,//(LSC.Comments != null && LSC.Comments.Length > 0) ? LSC.Comments : sm.DefaultComment),//,
                                        DefaultComment = sm.DefaultComment,
                                        //Comments = ((LSC.Comments!=null&& LSC.Comments .Length>0)? LSC.Comments: sm.DefaultComment),
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


        public IEnumerable<AssessmentModel> GetCollection2(int SurveyId, int PersonnelId, int AppId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<AssessmentModel> returnList = new List<AssessmentModel>();
            Db db = new Db();
            var SurUserID = 0;
            if (SurveyId != 0 && AppId != 0 && PersonnelId != 0)
            {
                var SurUser = unitOfWork.SurveyUser.Get(g => g.SurveyForRefId == AppId && g.PersonnelId == PersonnelId && g.SurveyId == SurveyId).FirstOrDefault();
                var SurUserrole = unitOfWork.PersonnelRole.Get(g => g.PersonnelId == PersonnelId && g.AppId == AppId).FirstOrDefault();
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
                                        where SM.SurveyId == SurveyId && (SurUserrole.IsSurveyParticipant.IndexOf(pc.PerspectiveCategoryName) >= 0 || SurUserrole.IsSurveyParticipant == "Both" || SurUserrole.IsSurveyParticipant == ""|| (SurUserrole.IsSurveyParticipant).ToLower() == "all")
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
                                              where SM.SurveyId == SurveyId && (SurUserrole.IsSurveyParticipant.IndexOf(pc.PerspectiveCategoryName) >= 0 || SurUserrole.IsSurveyParticipant == "Both" || SurUserrole.IsSurveyParticipant == "")
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
        public ActionResult SubmitUserSurvey(UserSurvey SubmitUserSurvey, string btnSubmit)
        {

            string strMessage = "";
            ModelState.Clear();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                foreach (var sc in SubmitUserSurvey.surveyCollection)
                {

                    var sce = unitOfWork.SurveyCollection.Get(s => s.SurveyModelId == sc.SurveyModelId && s.SurveyUserId == sc.SurveyUserId).FirstOrDefault();

                    if (sce != null)
                    {
                        sce.FinalRating = sc.FinalRating;
                        sce.Comments = sc.Comments;
                        sce.updateddt = DateTime.Now;
                        unitOfWork.SurveyCollection.Update(sce);

                    }
                    else
                    {
                        SurveyCollection scen = new SurveyCollection();
                        scen.FinalRating = sc.FinalRating;
                        scen.Comments = sc.Comments;
                        scen.Createddt = DateTime.Now;
                        scen.SurveyModelId = sc.SurveyModelId;
                        scen.SurveyUserId = sc.SurveyUserId;
                        unitOfWork.SurveyCollection.Insert(scen);
                    }
                    unitOfWork.Save();
                }
                if (btnSubmit.ToLower() != "resubmit" && btnSubmit == "Submit")
                {
                    foreach (var sc in SubmitUserSurvey.surveyCollection)
                    {
                        if ((sc.CanShow && sc.FinalRating == null)||(!sc.CanShow && sc.Comments == null))
                        {
                                ModelState.Clear();
                                TempData.Clear();
                                strMessage = "All questions need to be answered.<br> Highlighted questions in red are not answered yet.<br> Are you sure to submit the survey ? ";
                                TempData.Add("ValidationFailMsg", strMessage);
                                return RedirectToAction("Index", new { nextgensurveyId = SubmitUserSurvey.SurveyId, userSurveyToken = SubmitUserSurvey.UserSurveyToken });
                            
                        }
                    }
                }
                if ((btnSubmit.ToLower() == "submit" || btnSubmit.ToLower() == "resubmit") && SubmitUserSurvey != null)
                {
                    var surevyUser = unitOfWork.SurveyUser.GetByID(SubmitUserSurvey.surveyCollection.FirstOrDefault().SurveyUserId);
                    surevyUser.Completedt = DateTime.Now;
                    surevyUser.SurveyStatus = "Completed";

                    //To kepp track -when was the ratings got updated- To match 
                    //with the date ratings given by SurevyParticipants
                    //AND show the latest userRAtings(OverRiding the generic users ratings)
                    surevyUser.updateddt = DateTime.Now;
                    unitOfWork.SurveyUser.Update(surevyUser);
                }
                unitOfWork.Save();
            }

            if (btnSubmit.ToLower() == "save")
            {
                ModelState.Clear();
                TempData.Clear();
                strMessage = "<br> Your responses are updated successfully. Please submit to complete the Survey.";
                TempData.Add("ValidationSuccessMsg", strMessage);
                //var su = ValidateSurveyUser2(SubmitUserSurvey.SurveyId, SubmitUserSurvey.UserSurveyToken);
                return RedirectToAction("Index", new { nextgensurveyId = SubmitUserSurvey.SurveyId, userSurveyToken = SubmitUserSurvey.UserSurveyToken });
            }
            else
            {
                ModelState.Clear();
                TempData.Clear();
               
                ViewBag.Message = "Thank you for participating in this survey.";
                Db db = new Db();
                var userSurveyApplications = (from s in db.Survey
                                              //join su in db.SurveyUser on s.Id equals su.SurveyId
                                              join sp in db.SurveyUser on s.Id equals sp.SurveyId
                                              join app in db.Applications on sp.SurveyForRefId equals app.Id
                                              where sp.PersonnelId == SubmitUserSurvey.PersonnelId
                                              && sp.SurveyId == SubmitUserSurvey.SurveyId
                                              && s.enddate >= DateTime.Now
                                               && !string.IsNullOrEmpty(sp.UserSurveyToken)
                                               && sp.Completedt == null
                                              select new SelectListItem
                                              {
                                                  Text = app.ApplicationName,
                                                  Value = sp.UserSurveyToken
                                              }).FirstOrDefault();

                if (userSurveyApplications!=null)
                {
                    strMessage = "Thank you for participating in this survey.";
                    TempData.Add("ValidationSubmitsMsg", strMessage);
                    return RedirectToAction("Index", new { nextgensurveyId = SubmitUserSurvey.SurveyId, userSurveyToken = userSurveyApplications.Value });
                }
                else
                {
                    return View("SubmitSurvey");
                }

                   
                // return View("SubmitSurvey");
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

        private IEnumerable<ratingModel> getAnswers(int questionId)
        {
            Db db = new Db();
            var ans = db.SurveyModelRatingFactor.Where(w => w.SurveyModelId == questionId).ToList();
            var selAnswers = ans.Select(s => new ratingModel { Id = s.Id, Rating = s.RatingFactor,RatingContext=s.RatingContext, QuestionId=s.SurveyModelId });
            return selAnswers;
        }

        //public string getRatingContext(int id)
        //{
        //    Db db = new Db();
        //    var ans = db.SurveyModelRatingFactor.Where(w => w.Id == id).FirstOrDefault().RatingContext;
        //    //var selAnswers = ans.Select(s => new SelectListItem { Value = s.RatingContext, Text = s.RatingContext });
        //    return ans;
        //}


        private SurveyUserModel ValidateSurveyUser(int surveyId, string token)
        {

            if (string.IsNullOrEmpty(token) || surveyId <= 0)
            {
                return null;
            }

            Db db = new Db();

            var sur = (from s in db.Survey
                       join su in db.SurveyUser on s.Id equals su.SurveyId
                       where (s.Id == surveyId
                       && s.enddate >= DateTime.Now && su.UserSurveyToken == token)
                       select new SurveyUserModel
                       {
                           Description = s.Description,
                           SurveyName = s.Name,
                           PersonnelId = su.PersonnelId,
                           UserSurveyToken = su.UserSurveyToken,
                           SurveyId = s.Id,
                           surveyenddate = s.enddate
                       }).FirstOrDefault();

            return sur;

        }


        private UserSurvey ValidateSurveyUser2(int surveyId, string token)
        {

            if (string.IsNullOrEmpty(token) || surveyId <= 0)
            {
                return null;
            }

            Db db = new Db();

            var sur = (from s in db.Survey
                       join cl in db.Clients on s.ClientId equals cl.Id
                       join su in db.SurveyUser on s.Id equals su.SurveyId
                       where (s.Id == surveyId
                       && s.enddate >= DateTime.Now
                       && su.UserSurveyToken == token)
                       //&& su.Completedt==null)
                       //join 
                       select new UserSurvey
                       {
                           SurveyForRefId = su.SurveyForRefId.Value,
                           ClientName = cl.ClientName,
                           SurveyType = s.SurveyType,
                           SurveyUserId = su.Id,
                           Description = s.Description,
                           SurveyName = s.Name,
                           PersonnelId = su.PersonnelId,
                           UserSurveyToken = su.UserSurveyToken,
                           SurveyId = s.Id,
                           surveyenddate = s.enddate,
                           completedDt = su.Completedt
                       }).FirstOrDefault();

            if (sur == null)
                return null;
            else
            {
                UserSurvey oldsur = new UserSurvey();
                oldsur.completedDt = sur.completedDt;
                oldsur.userSurveyApplications = new List<SelectListItem>();
                if (sur.completedDt != null)
                {
                     sur = (from s in db.Survey
                                   join cl in db.Clients on s.ClientId equals cl.Id
                                   join su in db.SurveyUser on s.Id equals su.SurveyId
                                   where (s.Id == surveyId
                                   && s.enddate >= DateTime.Now
                                   && su.PersonnelId == sur.PersonnelId
                                   // && su.UserSurveyToken == token)
                                   && su.Completedt == null)
                                   //join 
                                   select new UserSurvey
                                   {
                                       SurveyForRefId = su.SurveyForRefId.Value,
                                       ClientName = cl.ClientName,
                                       SurveyType = s.SurveyType,
                                       SurveyUserId = su.Id,
                                       Description = s.Description,
                                       SurveyName = s.Name,
                                       PersonnelId = su.PersonnelId,
                                       UserSurveyToken = su.UserSurveyToken,
                                       SurveyId = s.Id,
                                       surveyenddate = s.enddate,
                                       completedDt = su.Completedt
                                   }).FirstOrDefault();
                    if (sur == null) {
                        return oldsur; }
                }

                    if (sur.SurveyForRefId > 0 && (sur.SurveyType.ToUpper().StartsWith("RA") || sur.SurveyType.ToUpper().StartsWith("APR") || sur.SurveyType.ToUpper().StartsWith("CLOUD")))
                    // if (sur.SurveyForRefId > 0 && (sur.SurveyType.ToUpper().StartsWith("APR") || sur.SurveyType.ToUpper().StartsWith("CLOUD")))
                    {
                        sur.SurveyFor = db.Applications.Where(w => w.Id == sur.SurveyForRefId).Select(s => s.ApplicationName).FirstOrDefault();
                    }
                    sur.userSurveyApplications = (from s in db.Survey
                                                  join sp in db.SurveyUser on s.Id equals sp.SurveyId
                                                  join app in db.Applications on sp.SurveyForRefId equals app.Id
                                                  where sp.PersonnelId == sur.PersonnelId
                                                  && sp.SurveyId == sur.SurveyId
                                                  && s.enddate >= DateTime.Now
                                                   && !string.IsNullOrEmpty(sp.UserSurveyToken)
                                                   && sp.Completedt == null
                                                  select new SelectListItem
                                                  {
                                                      Text = app.ApplicationName,
                                                      Value = sp.UserSurveyToken
                                                  }).Distinct().ToList();


                    sur.surveyCollection = GetSurveyCollections(sur.SurveyId, sur.PersonnelId, sur.SurveyUserId);

                }

                return sur;
            
        }

        private IList<SurveyCollectiondto> GetSurveyCollections(int surveyId, int SurveyPersonnel, int SurveyUserId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var SurUser = unitOfWork.SurveyUser.Get(g => g.SurveyId == surveyId && g.Id == SurveyUserId).FirstOrDefault();
            var SurUserrole = unitOfWork.PersonnelRole.Get(g => g.PersonnelId == SurveyPersonnel && g.AppId == SurUser.SurveyForRefId).FirstOrDefault();

            Db db = new Db();
            var surveyCollection = (from su in db.SurveyUser
                                    join sm in db.SurveyModel on su.SurveyId equals sm.SurveyId
                                    join sp in db.SurveyPerspective on sm.PerspectiveTypeId equals sp.Id
                                    join pc in db.PerspectiveCategory on sp.PerspectiveCategoryId equals pc.Id
                                    where (SurUserrole.IsSurveyParticipant.IndexOf(pc.PerspectiveCategoryName) >= 0 || SurUserrole.IsSurveyParticipant == "All" || SurUserrole.IsSurveyParticipant == "")
                                    join sc in db.SurveyCollection.Where(w => w.SurveyUserId == SurveyUserId) on sm.Id equals sc.SurveyModelId into tmpSC
                                    // join smf in db.SurveyModelRatingFactor on sm.Id equals smf.SurveyModelId into tmpSMF
                                    where (su.PersonnelId == SurveyPersonnel
                                    && su.SurveyId == surveyId
                                    && su.Id == SurveyUserId)
                                    from LSC in tmpSC.DefaultIfEmpty()
                                        //from LSMF in tmpSMF.DefaultIfEmpty()

                                    select new SurveyCollectiondto
                                    {

                                        Comments = LSC.Comments,
                                        DefaultComment=sm.DefaultComment,
                                        //Comments = (LSC.Comments!=null? LSC.Comments:sm.DefaultComment),
                                        FinalRating = LSC.FinalRating.Value,
                                        Question = sm.Question,
                                        SurveyModelId = sm.Id,
                                        Hint = sm.Hint,
                                        ModelType = sm.ModelType,
                                        CanShow= (sm.ModelType!= "freetext"),
                                        SurveyUserId = su.Id
                                        //   = su.Id,
                                        //  qa = new QA { qid = sm.Id, answerid = LSC.FinalRating.Value }
                                    }
                                    ).ToList();

            foreach (var item in surveyCollection)
            {
               // item.SurveyModelAnswers = getAnswers(item.SurveyModelId).ToList();
               item.QuestionOptions = getAnswers(item.SurveyModelId).ToList();

                //                       where op.SurveyModelId == item.SurveyModelId
                //                       orderby op.RatingFactorValue descending
                //                       select new ratingMod
                //                       {
                //                           RatingText = op.RatingFactor,
                //                           RatingId = op.Id
                //                       })



            }

            return surveyCollection;
        }


        public string getCltName(int clno)
        {
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var c = unitOfWork.Clients.Get(g => g.Id == clno).FirstOrDefault();
                string cname = c.ClientName;
                return cname;
            }
        }

        //-------------------------------------------------New Survey Page--------------------------------------------------//

        [AllowAnonymous]
        [HttpPost]
        public JsonResult AssessRead([DataSourceRequest] DataSourceRequest request, int SurveyId = 0, int SurveyPersonnel = 0, int ApplicationId = 0)
        {
            return Json(GetCollection2(SurveyId, SurveyPersonnel, ApplicationId).ToDataSourceResult(request));
        }

        public static void AssessUpdate(AssessmentModel model)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var exists = unitOfWork.SurveyCollection.GetByID(model.Id);
            if (exists != null && (exists.OriginalRating != model.OriginalRating || exists.FinalRating != model.FinalRating || exists.Comments != model.Comments))
            {
                exists.OriginalRating = model.OriginalRating;
                exists.FinalRating = model.FinalRating;
                exists.updateddt = DateTime.Now;
                exists.Comments = model.Comments;
                unitOfWork.SurveyCollection.Update(exists);
                unitOfWork.Save();
            }
        }
        public static void AssessCompletedUpdate(int SurveyUserId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var exists = unitOfWork.SurveyUser.GetByID(SurveyUserId);
            if (exists != null)
            {
                exists.Completedt = DateTime.Now;
                exists.updateddt = DateTime.Now;
                unitOfWork.SurveyUser.Update(exists);
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

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult NewAssessAjaxSave(RatingJSObject rating, string btnsubmit)
        //{
        //    int uid = (int)Session["UserId"];
        //    if (rating != null && rating.RatingsList != null && ModelState.IsValid)
        //    {
        //        foreach (var Q in rating.RatingsList)
        //        {
        //            if (Q.RowId != null && Q.RowId != 0)
        //            {
        //                AssessmentModel model = new AssessmentModel();
        //                model.Id = Q.RowId.Value;
        //                model.OriginalRating = Q.OriginalRating;
        //                model.FinalRating = Q.FinalRating;
        //                model.Comments = Q.Comments;
        //                AssessUpdate(model);

        //            }
        //        }
        //    }
        //    return Json(rating);
        //}

        [HttpPost]
        public ActionResult GetSurveyQuestionCollection([DataSourceRequest]DataSourceRequest request, int SurId,string Token )
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //var client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
               
                var validateSurvey = ValidateSurveyUser2(SurId,Token);
                IList<SurveyCollectiondto> sc = new List<SurveyCollectiondto>();
                foreach (var item in validateSurvey.surveyCollection)
                {
                    SurveyCollectiondto sct = new SurveyCollectiondto();
                    sct.Id = item.Id;
                    sct.Hint = item.Hint;
                    sct.Question = item.Question;
                    sct.FinalRating = item.FinalRating;
                    sct.Comments = item.Comments;
                    sct.SurveyModelId = item.SurveyModelId;
                    sct.SurveyUserId = item.SurveyUserId;
                    sct.QuestionOptions = item.QuestionOptions;
                   // sct.SurveyModelAnswerContext = item.SurveyModelAnswerContext;
                    sc.Add(sct);

                }


                return Json(sc.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
        }


        [HttpPost]
        public ActionResult SurveyUpdate(IList<SurveyCollectiondto> collectionsurvey,string btnType)
        {
           
            var SaveSurvey= SubmitSaveSurveyCollection(collectionsurvey,btnType);
           

            return Json(collectionsurvey);
        }

            public ActionResult SurveyRead([DataSourceRequest] DataSourceRequest request, int SurveyId = 0, string Token =null)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            //  int uid = (int)Session["UserId"];
            var su = ValidateSurveyUser2(SurveyId, Token);
            IEnumerable<SurveyCollectiondto> model = su.surveyCollection;

            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private string SubmitSaveSurveyCollection(IList<SurveyCollectiondto> SubmitUserSurvey, string btnSubmit)
        {

            string strMessage = "";
            ModelState.Clear();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                foreach (var sc in SubmitUserSurvey)
                {
                    // var sce = unitOfWork.SurveyCollection.Get(s => s.SurveyModelId == sc.SurveyModelId && s.SurveyUserId == sc.SurveyUserId).FirstOrDefault();
                    var sce = unitOfWork.SurveyCollection.Get(s => s.Id == sc.Id).FirstOrDefault();
                    if (sce != null)
                    {
                        sce.FinalRating = sc.FinalRating;
                        sce.Comments = sc.Comments;
                        sce.updateddt = DateTime.Now;
                        unitOfWork.SurveyCollection.Update(sce);

                    }
                    else
                    {
                        SurveyCollection scen = new SurveyCollection();
                        scen.FinalRating = sc.FinalRating;
                        scen.Comments = sc.Comments;
                        scen.Createddt = DateTime.Now;
                        scen.SurveyModelId = sc.SurveyModelId;
                        scen.SurveyUserId = sc.SurveyUserId;
                        unitOfWork.SurveyCollection.Insert(scen);
                    }
                }
                if (btnSubmit.ToLower() == "submit" && SubmitUserSurvey != null)
                {
                    var surevyUser = unitOfWork.SurveyUser.GetByID(SubmitUserSurvey.FirstOrDefault().SurveyUserId);
                    surevyUser.Completedt = DateTime.Now;
                    //To kepp track -when was the ratings got updated- To match 
                    //with the date ratings given by SurevyParticipants
                    //AND show the latest userRAtings(OverRiding the generic users ratings)
                    surevyUser.updateddt = DateTime.Now;
                    unitOfWork.SurveyUser.Update(surevyUser);
                }
                unitOfWork.Save();
            }

            if (btnSubmit.ToLower() == "save")
            {
                strMessage = "Your responses are updated successfully. Please submit to complete the Survey.";
              //  TempData.Add("ValidationSuccessMsg", strMessage);
                //var su = ValidateSurveyUser2(SubmitUserSurvey.SurveyId, SubmitUserSurvey.UserSurveyToken);
                // return RedirectToAction("Index", new { nextgensurveyId = SubmitUserSurvey.SurveyId, userSurveyToken = SubmitUserSurvey.UserSurveyToken });
                return strMessage;
            }
            else
            {
                // ViewBag.Message = "Thank you for participating in this survey.";
                // return View("SubmitSurvey");
                strMessage= "Thank you for participating in this survey.";
                return strMessage;
            }

        }



    }
}