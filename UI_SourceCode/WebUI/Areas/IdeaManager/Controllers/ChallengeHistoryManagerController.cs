using Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.Service;
using NTTData.AMOPortal.WebUI.Controllers;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Filters;
using NTTData.AMOPortal.WebUI.Mapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IdeaManager.Objects;
using WebUI.Dto;

namespace WebUI.Areas.IdeaManager.Controllers
{
    public class ChallengeHistoryManagerController : Cruder<IMPostChallenge, IMPostChallengeInput>
        {

        //Entities
        private IMPostChallenge entityIMPostChallenge;
        private IMChallengeCategoryMap entityIMChallengeCategoryMap;
        private IMChallengeITBenefits entityIMChallengeITBenefits;
        private IMChallengeBusinessBenefits entityIMChallengeBusinessBenefits;
        private IMChallengeComments entityIMChallengeComments;
        private IMChallengeIdeasRelations entityIMChallengeIdeasRelations;
        private IMChallengePriorityHistory entityIMChallengePriorityHistory;
        private IMChallengeStatusHistory entityIMChallengeStatusHistory;

        



        //IMChallengeIdeasRelations

        //IMChallengeBusinessBenefits

        //Service

        private CrudService<IMPostChallenge> IMPostChallengeService;
        private CrudService<IMChallengeCategoryMap> IMChallengeCategoryMapService;
        private CrudService<IMChallengeITBenefits> IMChallengeITBenefitsService;
        private CrudService<IMChallengeBusinessBenefits> IMChallengeBusinessBenefitsService;
        private CrudService<IMChallengeComments> IMChallengeCommentsService;
        private CrudService<IMChallengeIdeasRelations> IMChallengeIdeasRelationsService;
        private CrudService<IMChallengePriorityHistory> IMChallengePriorityHistoryService;
        private CrudService<IMChallengeStatusHistory> IMChallengeStatusHistoryService;
        


        //Mappers

        private Mapper<IMPostChallenge, IMPostChallengeInput> IMPostChallengeMapper;
        private Mapper<IMPostChallenge, PostChallengeInput> IMPostChallangeMapper;
        private Mapper<IMChallengeCategoryMap, IMChallengeCategoryMapInput> IMChallengeCategoryMapMapper;
        private Mapper<IMChallengeITBenefits, IMChallengeITBenefitsInput> IMChallengeITBenefitsMapper;
        private Mapper<IMChallengeBusinessBenefits, IMChallengeBusinessBenefitsInput> IMChallengeBusinessBenefitsMapper;
        private Mapper<IMChallengeComments, IMChallengeCommentsInput> IMChallengeCommentsMapper;

        private Mapper<IMChallengeIdeasRelations, IMChallengeIdeasRelationsInput> IMChallengeIdeasRelationsMapper;


        //Repo

        private Repo<IMPostChallenge> repoIMPostChallenge;
        private Repo<IMChallengeCategoryMap> repoIMChallengeCategoryMap;
        private Repo<IMChallengeITBenefits> repoIMChallengeITBenefits;
        private Repo<IMChallengeBusinessBenefits> repoIMChallengeBusinessBenefits;
        private Repo<IMChallengeComments> repoIMChallengeComments;
        private Repo<IMChallengeIdeasRelations> repoIMChallengeIdeasRelations;
        private Repo<IMChallengePriorityHistory> repoIMChallengePriorityHistory;
        private Repo<IMChallengeStatusHistory> repoIMChallengeStatusHistory;

        


        public ChallengeHistoryManagerController(ICrudService<IMPostChallenge> service, IMapper<IMPostChallenge, IMPostChallengeInput> v)
            : base(service, v)
            {

            
            DbContextFactory Dbcon = new DbContextFactory();
            repoIMPostChallenge = new Repo<IMPostChallenge>(Dbcon);
            IMPostChallengeMapper = new Mapper<IMPostChallenge, IMPostChallengeInput>();
            IMPostChallangeMapper = new Mapper<IMPostChallenge, PostChallengeInput>();
            entityIMPostChallenge = new IMPostChallenge();


            repoIMChallengeCategoryMap = new Repo<IMChallengeCategoryMap>(Dbcon);
            IMChallengeCategoryMapMapper = new Mapper<IMChallengeCategoryMap, IMChallengeCategoryMapInput>();
            entityIMChallengeCategoryMap = new IMChallengeCategoryMap();


            repoIMChallengeITBenefits = new Repo<IMChallengeITBenefits>(Dbcon);
            IMChallengeITBenefitsMapper = new Mapper<IMChallengeITBenefits,IMChallengeITBenefitsInput>();
            entityIMChallengeITBenefits = new IMChallengeITBenefits();
            IMChallengeITBenefitsService = new CrudService<IMChallengeITBenefits>(repoIMChallengeITBenefits);


            repoIMChallengeBusinessBenefits = new Repo<IMChallengeBusinessBenefits>(Dbcon);
            IMChallengeBusinessBenefitsMapper = new Mapper<IMChallengeBusinessBenefits, IMChallengeBusinessBenefitsInput>();
            entityIMChallengeBusinessBenefits = new IMChallengeBusinessBenefits();
            IMChallengeBusinessBenefitsService = new CrudService<IMChallengeBusinessBenefits>(repoIMChallengeBusinessBenefits);

            repoIMChallengeComments = new Repo<IMChallengeComments>(Dbcon);
            IMChallengeCommentsMapper = new Mapper<IMChallengeComments, IMChallengeCommentsInput>();
            entityIMChallengeComments = new IMChallengeComments();
            IMChallengeCommentsService = new CrudService<IMChallengeComments>(repoIMChallengeComments);


            repoIMChallengeIdeasRelations = new Repo<IMChallengeIdeasRelations>(Dbcon);
            IMChallengeIdeasRelationsMapper = new Mapper<IMChallengeIdeasRelations, IMChallengeIdeasRelationsInput>();
            entityIMChallengeIdeasRelations = new IMChallengeIdeasRelations();
            IMChallengeIdeasRelationsService = new CrudService<IMChallengeIdeasRelations>(repoIMChallengeIdeasRelations);


            repoIMChallengePriorityHistory = new Repo<IMChallengePriorityHistory>(Dbcon);
            IMChallengePriorityHistoryService = new CrudService<IMChallengePriorityHistory>(repoIMChallengePriorityHistory);


            repoIMChallengeStatusHistory = new Repo<IMChallengeStatusHistory>(Dbcon);
            IMChallengeStatusHistoryService = new CrudService<IMChallengeStatusHistory>(repoIMChallengeStatusHistory);

            



            }

        public static string GetUserName(int? uid)
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

        public ActionResult DisplayChallenges(int? page, string sortOrder)
        {
        Db db = new Db();
        //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            //IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            IPagedList<IMPostChallenge> IMPostChallengeInput;
         

    ViewBag.LastModifiedDate = string.IsNullOrEmpty(sortOrder) ? "LastModifiedDate_desc" : "LastModifiedDate";
            ViewBag.Challenges = string.IsNullOrEmpty(sortOrder) ? "Challenges_desc" : "Challenges";
            ViewBag.Postedby = string.IsNullOrEmpty(sortOrder) ? "Postedby_desc" : "Postedby";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            ViewBag.StatusComments = string.IsNullOrEmpty(sortOrder) ? "StatusComments_desc" : "StatusComments";
            ViewBag.CreatedDate = string.IsNullOrEmpty(sortOrder) ? "CreatedDate_desc" : "CreatedDate";


            var displaychallenges = from c in db.IMPostChallenge
                                    join usrcreated in db.UserProfiles on c.CreatedBy equals usrcreated.Id
                                    join usrUpdated in db.UserProfiles on c.UpdatedBy equals usrUpdated.Id into lstusr
                                    from usrUpdated in lstusr.DefaultIfEmpty()
                                    where c.ClientId == client.ClientId
                                    select new IMPostChallengeModel
                                    {

                                        Id = c.Id,
                                        UpdatedBy = c.UpdatedBy,
                                        ClientId = c.ClientId,
                                        CreatedBy = c.CreatedBy,
                                        CreatedByUser = usrcreated.FirstName + " " + usrcreated.LastName,
                                        CreatedDate = c.CreatedDate,
                                        ItemName = c.ItemName,
                                        LastUpdatedDate = c.LastUpdatedDate,
                                        LastUpdatedUser = usrUpdated.FirstName + " " + usrUpdated.LastName,
                                        StatusComments = c.StatusComments,
                                        StatusId = c.StatusId

                                    };

            switch (sortOrder)
            {
                //case "LastModifiedDate_desc":
                //    displayideas = k.OrderByDescending(s => s.PostIdeaInput.LastUpdatedDate).ToPagedList(page ?? 1, 8);
                //    break;
                case "LastModifiedDate_desc":
                    displaychallenges = displaychallenges.OrderByDescending(d => d.LastUpdatedDate);
                    break;
                case "LastModifiedDate":
                    displaychallenges = displaychallenges.OrderBy(d => d.LastUpdatedDate);
                    ViewBag.LastModifiedDate = "LastModifiedDate_desc";
                    break;

                case "Challenges_desc":
                    displaychallenges = displaychallenges.OrderByDescending(d => d.ItemName);
                    break;
                case "Challenges":
                    displaychallenges = displaychallenges.OrderBy(d => d.ItemName);
                    ViewBag.Challenges = "Challenges_desc";
                    break;

                case "Postedby_desc":
                    displaychallenges = displaychallenges.OrderByDescending(d => d.CreatedBy);
                    break;
                case "Postedby":
                    displaychallenges = displaychallenges.OrderBy(d => d.CreatedBy);
                    ViewBag.Postedby = "Postedby_desc";
                    break;

                case "Status_desc":
                    displaychallenges = displaychallenges.OrderByDescending(d => d.StatusId);
                    break;
                case "Status":
                    displaychallenges = displaychallenges.OrderBy(d => d.StatusId);
                    ViewBag.Status = "Status_desc";
                    break;

                case "StatusComments_desc":
                    displaychallenges = displaychallenges.OrderByDescending(d => d.StatusComments);
                    break;
                case "StatusComments":
                    displaychallenges = displaychallenges.OrderBy(d => d.StatusComments);
                    ViewBag.StatusComments = "StatusComments_desc";
                    break;

                case "CreatedDate_desc":
                    displaychallenges = displaychallenges.OrderByDescending(d => d.CreatedDate);
                    break;
                case "CreatedDate":
                    displaychallenges = displaychallenges.OrderBy(d => d.CreatedDate);
                    ViewBag.CreatedDate = "CreatedDate_desc";
                    break;



                default:
                    displaychallenges = displaychallenges.OrderBy(d => d.ItemName);
                    break;
            }

           // var x = displaychallenges.ToList();
          // IMPostChallengeInput = displaychallenges.ToPagedList(page == null ? 1 : page.Value, 8);
                        
            return View("SetupItems", displaychallenges.ToPagedList(page == null ? 1 : page.Value, 8));
}


public ActionResult DisplayITBenefits(int ItemId = 0)
        {
        IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
        return View("DisplayITBenefits");
        }


        [HttpPost]
        public JsonResult AddITBenefitDetails(IMChallengeITBenefitsInput tbenefitinput, int ItemId=0)
            {
            int uid = (int)Session["UserId"];
            try
                {

                //Update tbenefit Details

                tbenefitinput.CreatedByUser = GetUserName(uid);
                tbenefitinput.CreatedBy = uid;
                tbenefitinput.CreatedDate = DateTime.Now;
                tbenefitinput.LastUpdatedDate = DateTime.Now;
                tbenefitinput.LastUpdatedUser = tbenefitinput.CreatedByUser;
                tbenefitinput.UpdatedBy = uid;

                int Id = IMChallengeITBenefitsService.Create(IMChallengeITBenefitsMapper.MapToEntity(tbenefitinput, entityIMChallengeITBenefits));
                if (Id > 0)
                    {

                    return Json(new { Result = "OK", Record = tbenefitinput });
                    }
                else
                {
                throw new Exception("Error in saving the data");
                }
                    
                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }

            }


        [ActionTrack]
        [HttpPost]
        public JsonResult AddSelectedIdeas(int ItemId = 0, int ClientId = 0, string IdeaIds = "")
            {
            int uid = (int)Session["UserId"];
            string Result = "Success";
            try
                {
                string[] SelectedIdeaIds = IdeaIds.Substring(1).Split(',');
                foreach (var item in SelectedIdeaIds)
                    {

                    IMChallengeIdeasRelations IMChallengeIdeasRelations = new IMChallengeIdeasRelations();
                    IMChallengeIdeasRelations.ChallengeId = ItemId;
                    IMChallengeIdeasRelations.IdeaId = int.Parse(item);
                    IMChallengeIdeasRelations.CreatedByUser = GetUserName(uid);
                    IMChallengeIdeasRelations.CreatedBy = uid;
                    IMChallengeIdeasRelations.CreatedDate = DateTime.Now;
                    IMChallengeIdeasRelations.LastUpdatedDate = DateTime.Now;
                    IMChallengeIdeasRelations.UpdatedBy = uid;
                    IMChallengeIdeasRelations.LastUpdatedUser = IMChallengeIdeasRelations.CreatedByUser;

                    int Id = IMChallengeIdeasRelationsService.Create(IMChallengeIdeasRelations);

                    }
                }
            catch (Exception exp)
            {
                Result = "Failure";
            }



                //Update tbenefit Details

                //    tbenefitinput.CreatedByUser = User.Identity.Name;
                //    tbenefitinput.CreatedDate = DateTime.Now;
                //    tbenefitinput.LastUpdatedDate = DateTime.Now;
                //    tbenefitinput.LastUpdatedUser = User.Identity.Name;

                //    int Id = IMChallengeCommentsService.Create(IMChallengeCommentsMapper.MapToEntity(tbenefitinput, entityIMChallengeComments));
                //    if (Id > 0)
                //        {
                //        //return (GetCommentsDetails(ItemId));
                //        return Json(new { Result = "OK", Record = tbenefitinput });
                //        }
                //    else
                //        {
                //        throw new Exception("Error in saving the data");
                //        }

                //    }
                //catch (Exception ex)
                //    {
                //    return Json(new { Result = "ERROR", Message = ex.Message });
                //    }
              return Json(Result, JsonRequestBehavior.AllowGet);
            //return Json(new { Result = "Success" });
            }



        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddCommentDetails(IMChallengeCommentsInput tbenefitinput, int ItemId = 0)
            {
            int uid = (int)Session["UserId"];
            try
                {

                //Update tbenefit Details

                tbenefitinput.CreatedByUser =GetUserName(uid);
                tbenefitinput.CreatedBy = uid;
                tbenefitinput.CreatedDate = DateTime.Now;
                tbenefitinput.LastUpdatedDate = DateTime.Now;
                tbenefitinput.LastUpdatedUser = tbenefitinput.CreatedByUser;
                tbenefitinput.UpdatedBy = uid;
                int Id = IMChallengeCommentsService.Create(IMChallengeCommentsMapper.MapToEntity(tbenefitinput, entityIMChallengeComments));
                if (Id > 0)
                    {
                    //return (GetCommentsDetails(ItemId));
                    return Json(new { Result = "OK", Record = tbenefitinput });
                    }
                else
                    {
                    throw new Exception("Error in saving the data");
                    }

                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }

            }

        [ActionTrack]
        [HttpPost]
        public JsonResult UpdateStatusHistory(int ChallengeId = 0, int SelectedStatusId = 0, string Comments = "")
            {
            int uid = (int)Session["UserId"];
            bool updateSuccess = false;
            try
                {
                IMPostChallenge postChallenge = service.Get(ChallengeId);
                int oldStatusId = postChallenge.PriorityId;
                postChallenge.StatusId = SelectedStatusId;
                postChallenge.StatusComments = Comments;
                postChallenge.LastUpdatedDate = DateTime.Now;                
                //postChallenge.LastUpdatedUser = GetUserName(uid);
                service.Save();
                //Priroty update history

                IMChallengeStatusHistory IMChallengeStatusHistory = new IMChallengeStatusHistory()
                {
                    ChallengeId = ChallengeId,
                    OldStatusId = oldStatusId,
                    NewStatusId = SelectedStatusId,
                    Comments = Comments,
                    IsDeleted = false,
                    CreatedByUser = GetUserName(uid),
                    CreatedDate = DateTime.Now

                };
                var createdId = IMChallengeStatusHistoryService.Create(IMChallengeStatusHistory);




                updateSuccess = true;
                }
            finally
                {
                updateSuccess = false;
                }


            return Json(updateSuccess, JsonRequestBehavior.AllowGet);

            }



        //[HttpPost]
        //public JsonResult UpdatePriorityHistory(int ChallengeId = 0, int SelectedPriorityId = 0, string Comments = "")
        //{
        //    int uid = (int)Session["UserId"];
        //    bool updateSuccess = false;
        //try
        //    {
        //    IMPostChallenge postChallenge = service.Get(ChallengeId);
        //    int oldPriorityId = postChallenge.PriorityId;
        //    postChallenge.PriorityId = SelectedPriorityId;
        //    postChallenge.PriorityComments = Comments;
        //    postChallenge.LastUpdatedDate = DateTime.Now;
        //    //postChallenge.LastUpdatedUser = GetUserName(uid);
        //    service.Save();
        //    //Priroty update history

        //    IMChallengePriorityHistory imchallengPriroty = new IMChallengePriorityHistory()
        //    {
        //        ChallengeId = ChallengeId,
        //        OldPriorityId = oldPriorityId,
        //        NewPriorityId = SelectedPriorityId,
        //        Comments = Comments,
        //        IsDeleted = false,
        //        CreatedByUser = GetUserName(uid),
        //        CreatedDate = DateTime.Now
        //    };
        //    var createdId = IMChallengePriorityHistoryService.Create(imchallengPriroty);
                



        //    updateSuccess = true;
        //    }
        //finally
        //{
        //updateSuccess = false;            
        //}


        //return Json(updateSuccess, JsonRequestBehavior.AllowGet);
            
        //}

        [HttpPost]
        public JsonResult CheckCurrentPriority(int ChallengeId=0, int selectedPrioiryId=0)
        {
        bool resultFound = false;
         IMPostChallenge postChallenge =  service.Get(ChallengeId);
         if (postChallenge.PriorityId == selectedPrioiryId)
             {
             resultFound = true;
             }
         else
             resultFound = false;

                return Json(resultFound, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult CheckCurrentStatus(int ChallengeId = 0, int SelectedStatusId = 0)
            {
            bool resultFound = false;
            IMPostChallenge postChallenge = service.Get(ChallengeId);
            if (postChallenge.PriorityId == SelectedStatusId)
                {
                resultFound = true;
                }
            else
                resultFound = false;

            return Json(resultFound, JsonRequestBehavior.AllowGet);

            }



        [HttpPost]
        public JsonResult GetCommentsDetails(int ItemId = 0)
            {

            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            IMPostChallengeInput.Id = ItemId;
            IList<IMChallengeComments> lst = IMPostChallengeInput.GetChallengeComments;
            //lst.Add(new IMChallengeITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
            try
                {
                //Get data from database
                //Return result to jTable
                return Json(new { Result = "OK", Records = lst });
                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }


            }

        [HttpPost]
        public JsonResult AddBusinessBenefitDetails(IMChallengeBusinessBenefitsInput tbenefitinput, int ItemId = 0)
            {
            int uid = (int)Session["UserId"];
            try
                {

                //Update tbenefit Details

                tbenefitinput.CreatedByUser = GetUserName(uid);
                tbenefitinput.CreatedBy = uid;
                tbenefitinput.CreatedDate = DateTime.Now;
                tbenefitinput.LastUpdatedDate = DateTime.Now;
                tbenefitinput.LastUpdatedUser = tbenefitinput.CreatedByUser;
                tbenefitinput.UpdatedBy = uid;
                int Id = IMChallengeBusinessBenefitsService.Create(IMChallengeBusinessBenefitsMapper.MapToEntity(tbenefitinput, entityIMChallengeBusinessBenefits));
                if (Id > 0)
                    {

                    return Json(new { Result = "OK", Record = tbenefitinput });
                    }
                else
                    {
                    throw new Exception("Error in saving the data");
                    }

                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }

            }

        [HttpPost]
        public JsonResult GetITBenefitDetails(int ItemId = 0)
        {

        IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
        IMPostChallengeInput.Id = ItemId;
        IList<IMChallengeITBenefits> lst = IMPostChallengeInput.GetChallengeITBenefits;  
        //lst.Add(new IMChallengeITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
        try
            {
            //Get data from database
            //Return result to jTable
            return Json(new { Result = "OK", Records =  lst});
            }
        catch (Exception ex)
            {
            return Json(new { Result = "ERROR", Message = ex.Message });
            }

 
        }



        [HttpPost]
        public JsonResult GetPriorityHistoryDetails(int ItemId = 0)
            {

            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            IMPostChallengeInput.Id = ItemId;
            IList<IMChallengePriorityHistory> lst = IMPostChallengeInput.GetPriorityHistoryDetails;
            //lst.Add(new IMChallengeITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
            try
                {
                //Get data from database
                //Return result to jTable
                return Json(new { Result = "OK", Records = lst });
                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }


            }

        [HttpPost]
        public JsonResult GetStatusHistoryDetails(int ItemId = 0)
            {

            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            IMPostChallengeInput.Id = ItemId;
            IList<vwChallengeStatusHistory> lst = IMPostChallengeInput.GetStatusHistoryDetails;
            //lst.Add(new IMChallengeITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
            try
                {
                //Get data from database
                //Return result to jTable
                return Json(new { Result = "OK", Records = lst });
                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }


            }


        [HttpPost]
        public JsonResult GetListedIdeas(int ClientId = 0, int ChallengeId = 0)
            {

            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            IMPostChallengeInput.ClientId = ClientId;
            IMPostChallengeInput.ChallengeId = ChallengeId;
            IList<vwIMSuggestIdeas> lstForChallengeId = IMPostChallengeInput.GetSuggestIdeasForChallengeId;
            

            //lst.Add(new IMChallengeITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
            try
                {
                //Get data from database
                //Return result to jTable
                return Json(new { Result = "OK", Records = lstForChallengeId });
                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }


            }


        [HttpPost]
        public JsonResult GetListedIdeaDetails(int ClientId = 0, int ChallengeId=0)
            {

            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            IMPostChallengeInput.ClientId = ClientId;
            IMPostChallengeInput.ChallengeId = ChallengeId;
            IList<IMPostIdea> lst = IMPostChallengeInput.GetSuggestIdeas;
            IList<vwIMSuggestIdeas> lstForChallengeId =IMPostChallengeInput.GetSuggestIdeasForChallengeId;
            foreach (vwIMSuggestIdeas item in lstForChallengeId)
            {
            List<IMPostIdea> itemToRemove = lst.Where(c => c.Id == item.IdeaId).ToList();
            foreach (IMPostIdea delitem in itemToRemove)
            {
            lst.Remove(delitem);
            }

            }
            

            //lst.Add(new IMChallengeITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
            try
                {
                //Get data from database
                //Return result to jTable
                return Json(new { Result = "OK", Records = lst });
                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }


            }




        [HttpPost]
        public JsonResult GetIdeaDetails(int ClientId = 0)
            {

            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            IMPostChallengeInput.ClientId = ClientId;
            IList<IMPostIdea> lst = IMPostChallengeInput.GetIdeaItems;
            //lst.Add(new IMChallengeITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
            try
                {
                //Get data from database
                //Return result to jTable
                return Json(new { Result = "OK", Records = lst });
                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }


            }


        [HttpPost]
        public JsonResult GetBusinessBenefitDetails(int ItemId = 0)
            {

            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            IMPostChallengeInput.Id = ItemId;
            IList<IMChallengeBusinessBenefits> lst = IMPostChallengeInput.GetChallengeBusinessBenefits;
            //lst.Add(new IMChallengeITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
            try
                {
                //Get data from database
                //Return result to jTable
                return Json(new { Result = "OK", Records = lst });
                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }


            }

        [ActionTrack]
        public ActionResult DislayChallengeHistory(int ClientId = 0, int ItemId = 0)
        {
            //Client client = (Client)Session["Client"];
            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
        IMPostChallengeInput.ClientId = ClientId;
        IMPostChallengeInput.Id = ItemId;
        IMPostChallengeInput.PostChallengeInput = IMPostChallangeMapper.MapToInput(service.Get(ItemId));
        return View("ItemPostTypePage", IMPostChallengeInput);

        }

        public ActionResult DisplayHomePage(int ClientId = 0, int ItemId =0, string SaveData="", string TabName="home")
            {

            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            if (ClientId > 0)
                {
                IMPostChallengeInput.ClientId = ClientId;
                }
            if (ItemId > 0)
                {
                   IMPostChallengeInput.PostChallengeInput  =   IMPostChallangeMapper.MapToInput(service.Get(ItemId));
                   IMPostChallengeInput.ClientId = IMPostChallengeInput.PostChallengeInput.ClientId;
                   if (ClientId != IMPostChallengeInput.ClientId)
                   {
                   return RedirectToAction("DisplayHomePage", new { ClientId = IMPostChallengeInput.ClientId, ItemId = ItemId });
                   }
                }
            else
                {
                IMPostChallengeInput.PostChallengeInput = new PostChallengeInput();
                }

            IMPostChallengeInput.TabName = TabName;
            IMPostChallengeInput.Id = ItemId;

            if (SaveData.Length > 0)
            {
            ModelState.AddModelError("Success", "Data saved successfully");
            }
            return View("ItemPostTypePage", IMPostChallengeInput);
            }





        //public ActionResult DeleteDatSetName(int RegDataSetId = 0)
        //    {

        //    RegisterDatasetInput registerDataSetInput = RegisterDataSetMapper.MapToInput(RegisterDataSetService.Get(RegDataSetId));
        //    registerDataSetInput.IsDeleted = true;
        //    registerDataSetInput.LastUpdatedDate = DateTime.Now;
        //    registerDataSetInput.LastUpdatedUser = User.Identity.Name;
        //    RegisterDataSetMapper.MapToEntity(registerDataSetInput, RegisterDataSetService.Get(RegDataSetId));
        //    RegisterDataSetService.Save();
        //    int ClientId = registerDataSetInput.ClientId;
        //    return RedirectToAction("DisplayHomePage", "DataSet", new { ClientId = ClientId });

        //    }

        [ActionTrack]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SavePostChallenge(FormCollection form, IMPostChallengeInput inputs, string command, int ClientId = 0, int ItemId = 0, string PostCreate = "")
            {
            int uid = (int)Session["UserId"];
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            string TabName = "";
            string SaveData = "";
            if (command == "saveidea")
                {
                TabName = "home";
                PostChallengeInput postchallangeInput = inputs.PostChallengeInput;
                postchallangeInput.ClientId = inputs.ClientId;

                if (!ModelState.IsValid)
                    return View("ItemPostTypePage", inputs);


                //Get Login Id (ItemCreated By Id)

                if (Session["UserId"] == null)
                {
                    postchallangeInput.CreatedBy = WebSecurity.GetUserId("amo");
                }
                else
                {
                    postchallangeInput.CreatedBy = uid;
                    postchallangeInput.CreatedByUser = GetUserName(uid);
                }
                postchallangeInput.UpdatedBy = uid;
                postchallangeInput.LastUpdatedDate = DateTime.Now;
                if (ItemId > 0)
                    {
                    inputs.PostChallengeInput.Id = ItemId;
                    IMPostChallangeMapper.MapToEntity(inputs.PostChallengeInput, service.Get(ItemId));

                    service.Save();

                    }
                else
                    {
                    ItemId = service.Create(AutoMapper.Mapper.Map<PostChallengeInput, IMPostChallenge>(postchallangeInput));

                    }

                //var e = service.Get(id);

                ModelState.AddModelError("Success", "Challenge posted successfully");

                SaveData = "true";
                TabName = "classification";
                }

            

            if (command == "savecategory")
            {
            TabName = "classification";
                SaveCategoryDetails(form,inputs,ClientId,  ItemId);
                SaveData = "Save Category";
            }


            return RedirectToAction("DisplayHomePage", new { ClientId = ClientId, ItemId = ItemId, SaveData = SaveData,TabName=TabName });

                }

        public void SaveCategoryDetails(FormCollection form, IMPostChallengeInput inputs, int ClientId, int ItemId)
            {
            int uid = (int)Session["UserId"];

            string SaveData = "Save Category";
            IMChallengeCategoryMapService = new CrudService<IMChallengeCategoryMap>(repoIMChallengeCategoryMap);
            if (ItemId > 0)
            {
               IList<IMChallengeCategoryMap> lst =  IMChallengeCategoryMapService.GetAll().Where(c => c.ItemId == ItemId).ToList();
               foreach (IMChallengeCategoryMap item in lst)
               {
               IMChallengeCategoryMapService.Delete(item.Id);
               }
            }
            foreach (var item in form.AllKeys)
                {
                if (item.EndsWith("category"))
                    {
                    if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                        bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                        if (chkSelectedVariable)
                            {
                            IMChallengeCategoryMap mapItem  = new IMChallengeCategoryMap();
                            mapItem.CategoryId = Convert.ToInt32(item.Substring(0, item.IndexOf("category")));
                            if (Session["UserId"] == null)
                                {
                                mapItem.CreatedBy = WebSecurity.GetUserId("amo");
                                }
                            else
                            {
                                mapItem.CreatedBy = uid;
                            }
                            mapItem.ItemId = ItemId;
                            mapItem.CreatedByUser =GetUserName(uid);
                            mapItem.CreatedDate = DateTime.Now;
                            mapItem.UpdatedBy = uid;
                            mapItem.LastUpdatedDate = DateTime.Now; 
                            
                            IMChallengeCategoryMapService.Create(mapItem);
                            }
                            
                        }
                    }
                }

            //return RedirectToAction("DisplayHomePage", new { ClientId = ClientId, ItemId = ItemId, SaveData = SaveData, TabName = "classification" });
            }
            






            
        //    //return View("DataSetAdd", inputs);
        //    TempData["Success"] = "Data has been saved successfully";

        //    return RedirectToAction("DisplayViewDataSet", new { ClientId = ClientId, RegDataSetId = RegDataSetId });
        //    }



      


        [HttpPost]
        public JsonResult CheckItemName(string ItemName = "", int ItemId = 0)
            {


            DbContextFactory Dbcon = new DbContextFactory();
            Db db = new Db();
            var existItemNames = from IMPostChallenge in db.IMPostChallenge
                                    where IMPostChallenge.ItemName.ToLower() == ItemName.ToLower()
                                    select new
                                    {
                                        Id = IMPostChallenge.Id,
                                        Name = IMPostChallenge.ItemName
                                    };
            { }

            bool resultFound = false;
            if (ItemId > 0)
                {
                if (existItemNames.Where(c => c.Id != ItemId).Count() > 0)
                    resultFound = true;
                }
            if (ItemId == 0 && existItemNames.Count() > 0)
                {
                resultFound = true;
                }
            return Json(resultFound, JsonRequestBehavior.AllowGet);
            }


        [HttpPost]
        public JsonResult GetMetricTarget(int ChallengeId)
        {
            try
            {
                IList<IMMetricTarget> list = new List<IMMetricTarget>();
               // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                if (ChallengeId != 0 || ChallengeId != null)
                {
                    list = IdeaManagerHelper.GetMetricTargetDetails(ChallengeId);

                }
                //return Json(list);
                return Json(new { Result = "OK", Records = list });
            }
            catch (Exception ex)
            {
                //return null;
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        [HttpPost]
        public JsonResult GetApproversList(int ChallengeId)
        {
            Db db = new Db();
            var listOfAdmins = (from UP in db.UserProfiles
                                let fN = UP.FirstName
                                let lN = UP.LastName
                                join CA in db.IMChallengeApprover on UP.Id equals CA.ApproverId
                                //let Dt = string.Format("{0:dd.MM.yy}", CA.ApprovedDate)
                                where CA.ChallengeId == ChallengeId
                                select new ApproverModel() { FirstName = fN, LastName = lN, ApprovedDate =CA.ApprovedDate }
                                            ).Distinct().ToList();
            var slectNames = listOfAdmins.Select(c => new
            {
                Name = string.Format("{0} {1}", c.FirstName, c.LastName),
                ApprovedDate = c.ApprovedDate==null?"":c.ApprovedDate.Value.ToShortDateString()
            }).ToList();
            try
            {
                return Json(new { Result = "OK", Records = slectNames });
            }
            catch (Exception ex)
            {
               return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



    }
}
    
