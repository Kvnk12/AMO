using Data;
using System.Data;
using Domain.Abstract;
using Domain.Entities.ClientProfileManager;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.ClientProfileManager.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Domain.Service;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;
using PagedList;
using PagedList.Mvc;


using WebUI.Areas.ClientProfileManager.ViewModel;
using Domain.Entities.Survey;
using System.Web.Mvc;
using System.Net;

namespace WebUI.Areas.ClientProfileManager
{
    public class SurveyHelper//:IDisposable 
    {
       

        public static string GetName(int? uid)
        {
            //IList<SurveyViewModel> survey = new List<SurveyViewModel>();
            //var entityMapper = new Mapper<SurveyViewModel, UserProfile>();
            string name = "";
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var b = unitOfWork.UserProfile.GetByID(uid);
                name = b.FirstName + " " + b.LastName;
            }
            return name;
        }

        public static IList<Survey_Model> GetAllSurveys(int ClientId, int uid)
        {
            //to get user name;;ignore
            string name = "";
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var b = unitOfWork.UserProfile.GetByID(uid);
                name = b.FirstName + " " + b.LastName;
            }


            //actual method starts
            IList<Survey_Model> survey = new List<Survey_Model>();
            var entityMapper = new Mapper<Survey_Model, Survey>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<Survey> SurveyDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    SurveyDetails = unitOfWork.Survey.Get(cid => cid.ClientId == ClientId).ToList();

                if (SurveyDetails != null)
                {
                    foreach (var _surveydetail in SurveyDetails)
                    {
                        Survey_Model p = new Survey_Model();
                        p.Id = _surveydetail.Id;
                        p.ClientId = _surveydetail.ClientId;
                        //p.SurveyType = _surveydetail.SurveyType;
                        if (_surveydetail.SurveyType == "APR")
                            p.SurveyType = "Rating";
                        else // "APR2"
                            p.SurveyType = "Dynamic";
                        p.Name = _surveydetail.Name;
                        p.Description = _surveydetail.Description;
                        p.CreatedBy = _surveydetail.CreatedBy;
                        p.UpdatedBy = _surveydetail.UpdatedBy;
                        p.Createddt = _surveydetail.Createddt;
                        p.updateddt = _surveydetail.updateddt;
                        p.Launchdt = _surveydetail.Launchdt;
                        p.LaunchedBy = _surveydetail.LaunchedBy;
                        p.enddate = _surveydetail.enddate;
                        p.EndDay = _surveydetail.EndDay;
                        p.ToolId = _surveydetail.ToolId;
                        var c = unitOfWork.UserProfile.GetByID(p.CreatedBy);
                        if (c != null)
                        {
                            string ss = c.FirstName + " " + c.LastName;
                            p.CreatedByName = ss;
                        }
                        //To check SurveyLink sent or not.
                        p.IsLinkSent = unitOfWork.SurveyUser.Get().Where(g => g.SurveyId == _surveydetail.Id && g.UserSurveyToken != null).Any();

                        //var CreatedBy = unitOfWork.UserProfile.GetByID(p.CreatedBy);
                        //p.CreatedByName = CreatedBy.FirstName + " " + CreatedBy.LastName;
                       
                        survey.Add(p);
                    }
                }
                return survey;
            }

        }



        //public static Int32[] getexistingSurvUsers(int SuId)
        //{
        //    Int32[] XL = new Int32[] { };
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        var bbb = unitOfWork.SurveyUser.GetByID(SuId);
        //        bbb.PersonnelId
        //        foreach (var i in bbb)
        //        {
                    
        //            ;
        //        }
        //    }

        //    return XL;
        //}




        public static int AddSurvey(Survey_Model surveyinput)
        {
            int id = 0;

            try
            {
                var entityMapper = new Mapper<Survey, Survey_Model>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    Survey su = new Survey();

                    su.ClientId = surveyinput.ClientId;
                    su.Name = surveyinput.Name;
                    su.SurveyType = surveyinput.SurveyType;
                    su.Description = surveyinput.Description;
                    su.CreatedBy = surveyinput.CreatedBy;
                    su.UpdatedBy = surveyinput.UpdatedBy;
                    su.Createddt = surveyinput.Createddt;
                    su.updateddt = surveyinput.updateddt;
                    su.LaunchedBy = surveyinput.LaunchedBy;
                    su.Launchdt = surveyinput.Launchdt;
                    su.enddate = surveyinput.enddate;
                    su.ToolId = surveyinput.ToolId;
                    su.EndDay = surveyinput.EndDay;
                    unitOfWork.Survey.Insert(su);
                    unitOfWork.Save();
                    id = su.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }
        public static bool Update(Survey_Model surveyupdate)
        {
            try
            {
                //   var entityMapper = new Mapper<APRAppInterface, AprInterfaceModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var s = unitOfWork.Survey.GetByID(surveyupdate.Id);
                    //s.Id  = surveyupdate.Id ;
                    s.ClientId = surveyupdate.ClientId;
                    s.SurveyType = surveyupdate.SurveyType;
                    s.Name = surveyupdate.Name;
                    s.Description = surveyupdate.Description;
                    s.CreatedBy = surveyupdate.CreatedBy;
                    s.ToolId = Convert.ToInt32(surveyupdate.ToolId);
                    s.UpdatedBy = surveyupdate.UpdatedBy;
                    s.Createddt = surveyupdate.Createddt;
                    s.EndDay = surveyupdate.EndDay;
                    var currentTime = DateTime.Now;
                    s.updateddt = currentTime;
                    s.Launchdt = surveyupdate.Launchdt;
                    s.LaunchedBy = surveyupdate.LaunchedBy;
                    s.enddate = surveyupdate.enddate;

                    //    entityMapper.MapToEntity(interfaceupdate, appinterfaceinfo);
                    unitOfWork.Survey.Update(s);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void Delete(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var allQues = unitOfWork.SurveyModel.Get().Where(c => c.SurveyId == Id);
                foreach(var Ques in allQues)
                {
                    var allOptions = unitOfWork.SurveyModelRatingFactor.Get().Where(c => c.SurveyModelId == Ques.Id);
                    foreach(var option in allOptions)
                    {
                        unitOfWork.SurveyModelRatingFactor.Delete(option.Id);
                        unitOfWork.Save();//DO NOT DELETE
                    }
                    unitOfWork.SurveyModel.Delete(Ques.Id);
                    unitOfWork.Save();//DO NOT DELETE
                }
                unitOfWork.Survey.Delete(Id);
                unitOfWork.Save();
                
            }
            

        }


        public static string surveyDetails(int suID)
        {
            string surname = "";
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var b = unitOfWork.Survey.GetByID(suID);
                surname = b.Name;
            }
            return surname;
        }






        public static IList<SurveyQuestionModel> GetSurveyQuestionsDetails(int survId)
        {
            IList<SurveyQuestionModel> surveyQues = new List<SurveyQuestionModel>();
            var entityMapper = new Mapper<SurveyQuestionModel, SurveyModel>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<SurveyModel> SurveyQuesDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var surCheckLinkSent = unitOfWork.SurveyUser.Get().Where(g => g.SurveyId == survId && g.UserSurveyToken != null).Any();
                if (survId != 0)
                    SurveyQuesDetail = unitOfWork.SurveyModel.Get(cid => cid.SurveyId == survId).ToList();

                if (SurveyQuesDetail != null)
                {
                    foreach (var _surveyQuesdetail in SurveyQuesDetail)
                    {
                        SurveyQuestionModel SurveyQuesModel = new SurveyQuestionModel();
                        SurveyQuesModel.QId = _surveyQuesdetail.Id;
                        SurveyQuesModel.SurveyId = _surveyQuesdetail.SurveyId;
                        SurveyQuesModel.ModelType = _surveyQuesdetail.ModelType;
                        SurveyQuesModel.Question = _surveyQuesdetail.Question;
                        SurveyQuesModel.Hint = _surveyQuesdetail.Hint;
                        SurveyQuesModel.Weighting = _surveyQuesdetail.Weighting;
                        SurveyQuesModel.Include = _surveyQuesdetail.Include;
                        SurveyQuesModel.DefaultComment = _surveyQuesdetail.DefaultComment;
                        SurveyQuesModel.PerspectiveTypeId = _surveyQuesdetail.PerspectiveTypeId;
                        SurveyQuesModel.CreatedBy = _surveyQuesdetail.CreatedBy;
                        SurveyQuesModel.UpdatedBy = _surveyQuesdetail.UpdatedBy;
                        SurveyQuesModel.Createddt = _surveyQuesdetail.Createddt;
                        SurveyQuesModel.QuestionIdentifier = _surveyQuesdetail.QuestionIdentifier;
                        var currentTime = DateTime.Now;
                        SurveyQuesModel.updateddt = currentTime;
                        var c = unitOfWork.UserProfile.GetByID(_surveyQuesdetail.CreatedBy);
                        if (c != null)
                        {
                            string ss = c.FirstName+" "+c.LastName;
                            SurveyQuesModel.createdbyName = ss;
                        }
                        //SurveyQuesModel.createdbyName = getUserName(_surveyQuesdetail.CreatedBy);
                        SurveyQuesModel.IsSurveyLinkSent = surCheckLinkSent;
                        surveyQues.Add(SurveyQuesModel);
                    }
                }
                return surveyQues;
            }
        }


        public static int AddQuestion(SurveyQuestionModel SurveyQuestion)
        {
            int id = 0;
            var entityMapper = new Mapper<SurveyModel, SurveyQuestionModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var surQ = new SurveyModel();                   
                    surQ.SurveyId = SurveyQuestion.SurveyId;
                    surQ.ModelType = SurveyQuestion.ModelType;
                    surQ.Question = SurveyQuestion.Question;
                    surQ.Hint = SurveyQuestion.Hint;
                    surQ.Weighting = SurveyQuestion.Weighting;
                    surQ.Include = SurveyQuestion.Include;
                    surQ.DefaultComment = SurveyQuestion.DefaultComment;
                    surQ.PerspectiveTypeId = SurveyQuestion.PerspectiveTypeId;
                    surQ.Createddt = SurveyQuestion.Createddt;
                    surQ.updateddt = SurveyQuestion.updateddt;
                    surQ.CreatedBy = SurveyQuestion.CreatedBy;
                    surQ.UpdatedBy = SurveyQuestion.UpdatedBy;
                    surQ.QuestionIdentifier = SurveyQuestion.QuestionIdentifier;
                    unitOfWork.SurveyModel.Insert(surQ);

                    unitOfWork.Save();
                    id = surQ.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }



        public static bool updateQuestion(SurveyQuestionModel SurveyQuestion)
        {
            try
            {
                var entityMapper = new Mapper<SurveyModel, SurveyQuestionModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.SurveyModel.GetByID(SurveyQuestion.QId);
                    entityMapper.MapToEntity(SurveyQuestion, updatetitem);
                    unitOfWork.SurveyModel.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool deleteQuestion(int queId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var allOptions = unitOfWork.SurveyModelRatingFactor.Get().Where(c => c.SurveyModelId == queId);
                foreach (var option in allOptions)
                {
                    unitOfWork.SurveyModelRatingFactor.Delete(option.Id);
                    unitOfWork.Save();//DO NOT DELETE
                }
                unitOfWork.SurveyModel.Delete(queId);
                unitOfWork.Save();
                return true;
            }
        }


        public static IList<SurveyRatingFactorModel> GetOptions(int SurQuesId)
        {
            IList<SurveyRatingFactorModel> surveyOptions = new List<SurveyRatingFactorModel>();
            var entityMapper = new Mapper<SurveyRatingFactorModel,SurveyModelRatingFactor>();
            var surCheckLinkSent = false;
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<SurveyModelRatingFactor> SurveyOptions = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var GetSuvey = unitOfWork.SurveyModel.Get().Where(f => f.Id == SurQuesId).FirstOrDefault();
                if (GetSuvey != null)
                {
                    surCheckLinkSent = unitOfWork.SurveyUser.Get().Where(g => g.SurveyId == GetSuvey.SurveyId && g.UserSurveyToken != null).Any();
                }


                if (SurQuesId != 0)
                    SurveyOptions = unitOfWork.SurveyModelRatingFactor.Get(cid => cid.SurveyModelId == SurQuesId).ToList();

                if (SurveyOptions != null)
                {
                    foreach (var _surveyoptions in SurveyOptions)
                    {
                        SurveyRatingFactorModel SurveyOptionModel = new SurveyRatingFactorModel();
                        SurveyOptionModel.Id = _surveyoptions.Id;
                        SurveyOptionModel.SurveyModelId = _surveyoptions.SurveyModelId;
                        SurveyOptionModel.RatingFactor = _surveyoptions.RatingFactor;
                        SurveyOptionModel.RatingFactorValue = _surveyoptions.RatingFactorValue;
                        SurveyOptionModel.ROMEstimate = _surveyoptions.ROMEstimate;
                        SurveyOptionModel.EstimateAssumption = _surveyoptions.EstimateAssumption;
                        SurveyOptionModel.EstimateTypeId = _surveyoptions.EstimateTypeId;
                        SurveyOptionModel.EstimateCategoryId = _surveyoptions.EstimateCategoryId;
                        SurveyOptionModel.RatingContext = _surveyoptions.RatingContext;
                        SurveyOptionModel.CreatedBy = _surveyoptions.CreatedBy;
                        var c = unitOfWork.UserProfile.GetByID(_surveyoptions.CreatedBy);
                        if (c != null)
                        {
                            string ss = c.FirstName + " " + c.LastName;
                            SurveyOptionModel.createdbyName = ss;
                        }
                        SurveyOptionModel.IsSurveyLinkSent1 = surCheckLinkSent;
                        surveyOptions.Add(SurveyOptionModel);
                    }
                }
                return surveyOptions;
            }
        }


        public static int addOptions(SurveyRatingFactorModel survQoptions)
        {
            int id = 0;
            var entityMapper = new Mapper<SurveyModelRatingFactor, SurveyRatingFactorModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var surQop = new SurveyModelRatingFactor();
                    surQop.SurveyModelId = survQoptions.SurveyModelId;
                    surQop.RatingFactor = survQoptions.RatingFactor;
                    surQop.RatingFactorValue = survQoptions.RatingFactorValue;
                    surQop.ROMEstimate = survQoptions.ROMEstimate;
                    surQop.EstimateAssumption = survQoptions.EstimateAssumption;
                    surQop.EstimateTypeId = survQoptions.EstimateTypeId;
                    surQop.EstimateCategoryId = survQoptions.EstimateCategoryId;
                    surQop.RatingContext = survQoptions.RatingContext;
                    surQop.CreatedBy = survQoptions.CreatedBy;
                    unitOfWork.SurveyModelRatingFactor.Insert(surQop);

                    unitOfWork.Save();
                    id = surQop.Id.Value;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool updateOptions(SurveyRatingFactorModel survQuesOption)
        {
            try
            {
                var entityMapper = new Mapper<SurveyModelRatingFactor, SurveyRatingFactorModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var survOption = unitOfWork.SurveyModelRatingFactor.GetByID(survQuesOption.Id);
                     entityMapper.MapToEntity(survQuesOption, survOption);
                    //survOption.LOBName = survQuesOption.LOBName;
                    unitOfWork.SurveyModelRatingFactor.Update(survOption);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool deleteOptions(int OpId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.SurveyModelRatingFactor.Delete(OpId);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }




        //Manage Survey
        public static IEnumerable<SurveyUserViewModel> GetPersonnels(int clientId)
        {

            Db db = new Db();
            var personnellist = (from k in db.PersonnelRole
                                 join i in db.Personnels on k.PersonnelId equals i.Id
                                 where (k.ClientId == clientId)
                                 select new SurveyUserViewModel
                                 {
                                     clientID = clientId,
                                     PersonnelId = i.Id,
                                     PersonnelName = i.PersonnelName,
                                     Email = i.EmailAddress,
                                     FirstName = i.FirstName,
                                     LastName = i.LastName

                                 }
                                ).AsEnumerable();



            return personnellist;
        }


        public static IList<SurveyUserModel> getpersonSurvey(int SuyId)
        {
            IList<SurveyUserModel> surveyusers = new List<SurveyUserModel>();
            var entityMapper = new Mapper<SurveyUserModel, SurveyUser>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<SurveyUser> SurveyOptions = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (SuyId != 0)
                    SurveyOptions = unitOfWork.SurveyUser.Get(c=>c.SurveyId == SuyId).ToList();

                if (SurveyOptions != null)
                {
                    foreach (var _surveyoptions in SurveyOptions)
                    {
                        SurveyUserModel SurvuserMod = new SurveyUserModel();
                        SurvuserMod.PersonnelId = _surveyoptions.PersonnelId;
                        SurvuserMod.SurveyForRefId = _surveyoptions.SurveyForRefId;
                        surveyusers.Add(SurvuserMod);
                    }
                }
                return surveyusers;
            }
        }

        public static int AddParticipant(SurveyUserModel survUser)
        {
            var mnnm = new SurveyUserModel();
            int id = 0;
            var entityMapper = new Mapper<SurveyUser, SurveyUserModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var gg= unitOfWork.SurveyUser.Get(g => g.PersonnelId == survUser.PersonnelId && g.SurveyForRefId==survUser.SurveyForRefId && g.SurveyId == survUser.SurveyId).Any();
                    if (!gg)
                    {
                        var surQ = new SurveyUser();
                        surQ.SurveyId = survUser.SurveyId.Value;
                        surQ.PersonnelId = survUser.PersonnelId;
                        surQ.SurveyForRefId = survUser.SurveyForRefId;
                        surQ.CreatedBy = survUser.CreatedBy;
                        unitOfWork.SurveyUser.Insert(surQ);
                        unitOfWork.Save();
                        id = surQ.Id;
                    }

                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int CloneParticipants(IList<SurveyUserModel> sumlist,int OriginalSurveyId)
        {
            var mnnm = new SurveyUserModel();
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    foreach(var survUser in sumlist)
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var gg = unitOfWork.SurveyUser.Get(g => g.PersonnelId == survUser.PersonnelId && g.SurveyId == OriginalSurveyId && g.SurveyForRefId== survUser.SurveyForRefId).FirstOrDefault();
                        if (gg!=null)
                        {
                            var surQ = new SurveyUser();
                            surQ.SurveyId = survUser.SurveyId.Value;
                            surQ.PersonnelId = survUser.PersonnelId;
                            surQ.SurveyForRefId =gg.SurveyForRefId;
                            surQ.CreatedBy = survUser.CreatedBy;
                            surQ.Createddt = survUser.Createddt;
                            unitOfWork.SurveyUser.Insert(surQ);
                            unitOfWork.Save();
                            id = surQ.Id;
                        }
                    }
                    

                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool updateUserSurveyApp(SurveyUserModel surveyuserApp)
        {
            try
            {
                //   var entityMapper = new Mapper<APRAppInterface, AprInterfaceModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var s = unitOfWork.SurveyUser.GetByID(surveyuserApp.Id);
                    //s.Id  = surveyupdate.Id ;
                    s.SurveyForRefId= surveyuserApp.SurveyForRefId;
                    s.UpdatedBy = surveyuserApp.UpdatedBy;
                    s.updateddt = surveyuserApp.updateddt;

                    //    entityMapper.MapToEntity(interfaceupdate, appinterfaceinfo);
                    unitOfWork.SurveyUser.Update(s);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IEnumerable<SurveyUser> usersurveylinks(int surveyid, int PersonnelId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                return unitOfWork.SurveyUser.Get(g=>g.PersonnelId==PersonnelId && g.SurveyId==surveyid && g.UserSurveyToken!=null);
             }

        }

        public static bool updateToken(SurveyUserModel survUser)
        {
            var mnnm = new SurveyUserModel();
        
            var entityMapper = new Mapper<SurveyUser, SurveyUserModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var gg = unitOfWork.SurveyUser.Get(g => g.PersonnelId == survUser.PersonnelId && g.SurveyId == survUser.SurveyId && g.SurveyForRefId==survUser.SurveyForRefId).FirstOrDefault();
                    if (gg != null)
                    {
                        gg.UserSurveyToken = survUser.UserSurveyToken;
                        unitOfWork.SurveyUser.Update(gg);
                        unitOfWork.Save();
                        return true;
                    }
                    else return false;
                   
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool deleteSurveyUser(SurveyUserModel Personnel)
        {
            
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var sdf = unitOfWork.SurveyUser.Get(s => s.SurveyId == Personnel.SurveyId && s.PersonnelId==Personnel.PersonnelId&&s.SurveyForRefId==Personnel.SurveyForRefId).FirstOrDefault();
                    unitOfWork.SurveyUser.Delete(sdf);
                    unitOfWork.Save();

                    //TempData["SLACreatedDeleted"] = "Deleted successfully.";
               
                }
           

           

            return true;
        }



        ////public static void SendEMail(string emailid, string subject, string body)
        ////{

        ////    MailMessage mail = new MailMessage();
        ////    String EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
        ////    mail.From = new MailAddress(EmailFrom);
        ////    // mail.To
        ////    mail.To.Add(emailid); // Replace with a "To" address. If your account is still in the

        ////    String Bcc = WebConfigurationManager.AppSettings["EmailBcc"];
        ////    if (!string.IsNullOrEmpty(Bcc))
        ////    {
        ////        mail.Bcc.Add(Bcc);
        ////    }


        ////    mail.Subject = subject;
        ////    mail.Body = body;
        ////    mail.IsBodyHtml = true;

        ////    String HOST = WebConfigurationManager.AppSettings["smtpHost"];


        ////    int PORT = Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);


        ////    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
        ////    {

        ////        // Send the email. 
        ////        try
        ////        {

        ////            String pwd = WebConfigurationManager.AppSettings["smtpUserPwd"];
        ////            String userid = WebConfigurationManager.AppSettings["smtpUserId"];
        ////            if (!string.IsNullOrEmpty(pwd))
        ////            {
        ////                NetworkCredential Credentials = new NetworkCredential(userid, pwd);
        ////                client.Credentials = Credentials;
        ////            }
        ////            client.Send(mail);
        ////        }
        ////        catch (Exception ex)
        ////        {

        ////        }
        ////    }



        ////}




        public static IList<SurveyReportModel> surveystatus(int ClientId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                var CVB = (from a in db.SurveyUser
                           join p in db.Survey on a.SurveyId equals p.Id
                           join app in db.Applications on a.SurveyForRefId equals app.Id into ap
                           from app in ap.DefaultIfEmpty()
                           where (p.ClientId == ClientId && a.UserSurveyToken !=null)
                           select new SurveyReportModel
                           {
                               surveyId = a.SurveyId,
                               surveyName = p.Name,
                               SyrveyCompletedDate = a.Completedt,
                               surveyEndDate = p.enddate,
                               personnelId = a.PersonnelId,
                               SurveyFor = app.ApplicationName == null? "" : app.ApplicationName
                           }

                    ).ToList();
                foreach (var it in CVB)
                {
                    var c = unitOfWork.Personnel.GetByID(it.personnelId);
                    if (c != null)
                    {
                        string ss = c.FirstName+" "+c.LastName;
                        it.PersonnelName = ss;
                    }
                    


                    if (it.SyrveyCompletedDate != null)
                    {
                        it.SurveyStatus = "Completed";
                    }
                    else
                        it.SurveyStatus = "Yet to Submit the survey";

                }
                return CVB;
            }
            

        }

        //to get single survey details
        public static string singlesurveyDetails(int suID)
        {
            //    Survey_Model sdtails = new Survey_Model();
            var Name = "";
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var b = unitOfWork.Survey.GetByID(suID);
                //sdtails.Id = suID;
                 Name=b.Name;
                //sdtails.SurveyType = b.SurveyType;
                //sdtails.Description = b.Description;
                //sdtails.enddate = b.enddate;
                //sdtails.CreatedBy = b.CreatedBy;
            }
            return Name;
        }

        public static IEnumerable<SelectListItem> _getPerspectiveTypes(int Client, int surid)
        {
            Db db = new Db();
            var estimateTypes = (from su in db.Survey


                                 join sp in db.SurveyPerspective on su.ToolId equals sp.ToolId
                                 where su.ClientId == Client && su.Id == surid && sp.ClientId== Client

                                 select new { Id = sp.Id, App = sp.PerspectiveName }).ToList();

            var estiMateList = estimateTypes.Select(s => new SelectListItem
            { Text = s.App, Value = s.Id.ToString() });
            return estiMateList;
        }




        public static IEnumerable<SelectListItem> _getEstimateTypes(int Client)
        {
            Db db = new Db();
            var estimateTypes = (from e in db.EstimateType
                                 select new
                                 {
                                     Id = e.Id,
                                     estimatetype = e.EstimateType
                                 }).ToList();

            var estiMateList = estimateTypes.Select(s => new SelectListItem
            { Text = s.estimatetype, Value = s.Id.ToString() });
            return estiMateList;
        }
        public static IEnumerable<SelectListItem> _getEstimateCategories(int Client)
        {
            Db db = new Db();
            var estimateCategory = (from e in db.EstimateCategory
                                    select new
                                    {
                                        Id = e.Id,
                                        estimateCategory = e.EstimateCategory
                                    }).ToList();

            var estiMateCatList = estimateCategory.Select(s => new SelectListItem
            { Text = s.estimateCategory, Value = s.Id.ToString() });
            return estiMateCatList;
        }

        //public IEnumerable<SelectListItem> _getPerspectiveTypes(int ClientID, int SurveyId)
        //{

        //    // var client = (Client)Session["Client"];
        //    // int ClientId = client.Id;
        //    var contextFactory = new DbContextFactory();
        //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //    Db db = new Db();
        //    // var ToolNo = unitOfWork.Survey.Get().Where(x => x.ClientId == ClientID && x.Id == SurveyId).Select(x => x.ToolId).ToList();

        //    var perspectCat = (from su in db.Survey


        //                       join sp in db.SurveyPerspective on su.ToolId equals sp.ToolId
        //                       where su.ClientId == ClientID && su.Id == SurveyId

        //                       select new { Id = sp.Id, App = sp.PerspectiveName }).ToList();


        //    var item = perspectCat.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.App.ToString() });
        //    return item;


        public IEnumerable<SelectListItem> ApplicationList(int ClientID, int usid)
        {
            //  int usid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var db = new Db();
            var toolList = (from tm in db.AppStore
                            join tr in db.UserApplications on tm.Id equals tr.ApplicationId
                            where tr.ClientId == ClientID && tr.UserId == usid
                            select new { Id = tm.Id, App = tm.ApplicationName }).ToList();
            var item = toolList.OrderBy(x => x.App).Distinct().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.App.ToString(), Selected = true });
            return item;

        }
        public static IEnumerable<SelectListItem> _ApplicationList(int Client, int usid)
        {
            Db db = new Db();
            var rolename = (from up in db.UserProfiles
                            where up.Id == usid
                            select up.RoleId).FirstOrDefault();
            if (rolename.ToLower() == "superuser")
            {
                var toolList = (from tm in db.AppStore
                                where tm.IsDeleted == false
                                select new { Id = tm.Id, App = tm.ApplicationName }).ToList();
                var estiMateList = toolList.Distinct().Select(s => new SelectListItem
                { Text = s.App, Value = s.Id.ToString() });
                return estiMateList;
            }
            else
            {
                var toolList = (from tm in db.AppStore
                                join tr in db.UserApplications on tm.Id equals tr.ApplicationId
                                where tr.ClientId == Client && tr.UserId == usid && tm.IsDeleted == false
                                select new { Id = tm.Id, App = tm.ApplicationName }).ToList();

                var estiMateList = toolList.Distinct().Select(s => new SelectListItem
                { Text = s.App, Value = s.Id.ToString() });
                return estiMateList;
            }
        }


        public static bool updateINToken(SurveyINUser survUser)
        {
            
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    //var gg = unitOfWork.SurveyINUser.Get(g => g.SurveyUserName == survUser.SurveyUserName && g.SurveyId == survUser.SurveyId ).FirstOrDefault();
                    var gg = unitOfWork.SurveyINUser.Get(g=>g.Id==survUser.Id).FirstOrDefault();
                    if (gg != null)
                    {
                        gg.UserSurveyToken = survUser.UserSurveyToken;
                        gg.SurveySentDate = DateTime.Now;
                        gg.SurveyStatus = survUser.SurveyStatus;
                        gg.updateddt = survUser.updateddt;
                        gg.UpdatedBy = survUser.UpdatedBy;
                        unitOfWork.SurveyINUser.Update(gg);
                        unitOfWork.Save();
                        return true;
                    }
                    else return false;

                }
            }
            catch (Exception)
            {
                return false;
            }
        }




        public static int AddSurveyEmailConfigs(Survey_ScheduleEmailConfigModel suvemc)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var res2 = new SB_ScheduleEmailConfig();
                    res2.ClientId = suvemc.ClientId.Value;
                    res2.Survey_ID = suvemc.Survey_ID.Value;
                    res2.Ack_Message = suvemc.Ack_Message;
                    res2.FirstReminder_Message = suvemc.FirstReminder_Message;
                    res2.SecondReminder_Message = suvemc.SecondReminder_Message;
                    res2.CreatedDatetime = DateTime.Now.Date;
                    res2.UpdatedDatetime = DateTime.Now.Date;
                    res2.updatedBy = suvemc.updatedBy.Value;
                    unitOfWork.SB_ScheduleEmailConfig.Insert(res2);
                    unitOfWork.Save();
                    id = res2.Id;
                }
                return id;
                  
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int UpdateSurveyEmailConfigs(Survey_ScheduleEmailConfigModel suvemc)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var res2 = unitOfWork.SB_ScheduleEmailConfig.GetByID(suvemc.Id);
                  //  res2.ClientId = suvemc.ClientId.Value;
                   //res2.Survey_ID = suvemc.Survey_ID.Value;
                    res2.Ack_Message = suvemc.Ack_Message;
                    res2.FirstReminder_Message = suvemc.FirstReminder_Message;
                    res2.SecondReminder_Message = suvemc.SecondReminder_Message;
                  //  res2.CreatedDatetime = DateTime.Now.Date;
                    res2.UpdatedDatetime = DateTime.Now.Date;
                    res2.updatedBy = suvemc.updatedBy.Value;
                    unitOfWork.SB_ScheduleEmailConfig.Update(res2);
                    unitOfWork.Save();
   
                }

                return id;   

            }
            catch (Exception)
            {
                return 0;
            }
        }

      


    }


}
