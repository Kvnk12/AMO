using Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.Service;
using NTTData.AMOPortal.WebUI.Controllers;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using WebMatrix.WebData;
using WebUI.Areas.IdeaManager.Dto;
using System.IO;
using WebUI.Areas.IdeaManager.Objects;
using Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.IdeaManager.Controllers
{
    public class ChallengeManagerController : Cruder<IMPostChallenge, IMPostChallengeInput>
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




        public ChallengeManagerController(ICrudService<IMPostChallenge> service, IMapper<IMPostChallenge, IMPostChallengeInput> v)
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
            IMChallengeITBenefitsMapper = new Mapper<IMChallengeITBenefits, IMChallengeITBenefitsInput>();
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
           // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            //IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();


            ViewBag.LastModifiedDate = string.IsNullOrEmpty(sortOrder) ? "LastModifiedDate_desc" : "LastModifiedDate";
            ViewBag.Challenges = string.IsNullOrEmpty(sortOrder) ? "Challenges_desc" : "Challenges";
            ViewBag.Postedby = string.IsNullOrEmpty(sortOrder) ? "Postedby_desc" : "Postedby";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            ViewBag.StatusComments = string.IsNullOrEmpty(sortOrder) ? "StatusComments_desc" : "StatusComments";
            ViewBag.CreatedDate = string.IsNullOrEmpty(sortOrder) ? "CreatedDate_desc" : "CreatedDate";

            //IQueryable<IMPostChallenge> displaychallenges;


            //var IsAdmin= (from UA in db.UserApplications
            //                    join UR in db.UserRoles on UA.RoleId equals UR.Id
            //                    where ((UR.RoleName == "admin" || UR.RoleName == "sysadmin" )&& UA.UserId == uid)
            //                    select UR.RoleName
            //                                ).Distinct().Any();

            //if(IsAdmin)
            //{
            //var displaychallenges = from d in db.IMPostChallenge
            //                    where d.ClientId == client.Id
            //                    select d;
            //}
            //else
            //{
            //    displaychallenges = from d in db.IMPostChallenge
            //                            where d.CreatedBy==uid && d.ClientId==client.Id
            //                            select d;
            //}

            //var displayideas = IdeaManagerHelper.GetPostItems(client.Id);
            List<IMPostChallengeModel> displaychallengesModel = new List<IMPostChallengeModel>();        
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

            displaychallengesModel = displaychallenges.ToList();
            //IMPostChallengeInput.SortedPostedChallenges = displaychallenges.ToPagedList(page == null ? 1 : page.Value, 8);
            //if (ClientId > 0)
            //    {
            //    IMPostChallengeInput.ClientId = ClientId;
            //    Session["Client"] = ClientId;
            //    } 
            return View("SetupItems", displaychallengesModel.ToPagedList(page == null ? 1 : page.Value, 8));
            //return View("SetupItems", IMPostChallengeInput);
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
                if (ItemId != 0)
                {
                    //Update tbenefit Details

                    tbenefitinput.CreatedByUser = GetUserName(uid);
                    tbenefitinput.CreatedBy = uid;
                    tbenefitinput.CreatedDate = DateTime.Now;
                    tbenefitinput.LastUpdatedDate = DateTime.Now;
                    tbenefitinput.UpdatedBy = uid;
                    tbenefitinput.LastUpdatedUser = tbenefitinput.CreatedByUser;

                    int Id = IMChallengeITBenefitsService.Create(IMChallengeITBenefitsMapper.MapToEntity(tbenefitinput, entityIMChallengeITBenefits));

                    if (Id > 0)
                    {
                        tbenefitinput.Id = Id;
                        return Json(new { Result = "OK", Record = tbenefitinput });
                    }
                    else
                    {
                        throw new Exception("Error in saving the data");
                    }
                }
                else
                {
                    throw new Exception("Select Idea/Submit the one!");
                }

                
                    
            }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }

            }


        [HttpPost]
        public JsonResult UpdateITBenefitDetails(IMChallengeITBenefitsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
            if (!ModelState.IsValid)
            {
                return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
            }
            try
            {
                //IMChallengeITBenefits imidea = IMChallengeITBenefitsService.Get(tbenefitinput.Id);

                IMChallengeITBenefits imidea = new IMChallengeITBenefits();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    imidea = unitOfWork.IMChallengeITBenefits.GetByID(tbenefitinput.Id);
                    //imidea.Id = tbenefitinput.Id;
                    //imidea.ItemId = tbenefitinput.ItemId;
                    imidea.Name = tbenefitinput.Name;
                    imidea.FTEHours = tbenefitinput.FTEHours;
                    imidea.Detail = tbenefitinput.Detail;
                    //imidea.CreatedBy = tbenefitinput.CreatedBy;
                    imidea.Value = tbenefitinput.Value;
                    imidea.LastUpdatedDate = DateTime.Now;
                    imidea.LastUpdatedUser = GetUserName(uid);
                    imidea.UpdatedBy = uid;
                    unitOfWork.IMChallengeITBenefits.Update(imidea);
                    unitOfWork.Save();
                }

                //IMChallengeITBenefitsService.Save();
                return Json(new { Result = "OK", Record = imidea });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult DeleteITBenefitDetails(int Id)
        {
            try
            {
                IMChallengeITBenefitsService.Delete(Id);
                return Json(new { Result = "OK" });
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
                if (ItemId != 0)
                {

                    //Update tbenefit Details

                    tbenefitinput.CreatedByUser = GetUserName(uid);
                    tbenefitinput.CreatedBy = uid;
                    tbenefitinput.CreatedDate = DateTime.Now;
                    tbenefitinput.LastUpdatedDate = DateTime.Now;
                    tbenefitinput.LastUpdatedUser = tbenefitinput.CreatedByUser;
                    tbenefitinput.UpdatedBy = uid;
                    int Id = IMChallengeCommentsService.Create(IMChallengeCommentsMapper.MapToEntity(tbenefitinput, entityIMChallengeComments));

                    if (Id > 0)
                    {
                        tbenefitinput.Id = Id;
                        //return (GetCommentsDetails(ItemId));
                        return Json(new { Result = "OK", Record = tbenefitinput });
                    }
                    else
                    {
                        throw new Exception("Error in saving the data");
                    }
                }
                else
                {
                    throw new Exception("Select Idea/Submit the one!");
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
                postChallenge.UpdatedBy = uid;
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
                if(ItemId!=0)
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
                        tbenefitinput.Id = Id;
                        return Json(new { Result = "OK", Record = tbenefitinput });
                    }
                    else
                    {
                        throw new Exception("Error in saving the data");
                    }
                }
                else
                {
                    throw new Exception("Select Idea/Submit the one!");
                }


                }
            catch (Exception ex)
                {
                return Json(new { Result = "ERROR", Message = ex.Message });
                }

            }

        [HttpPost]
        public JsonResult UpdateBusinessBenefitDetails(IMChallengeBusinessBenefitsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
            if (!ModelState.IsValid)
            {
                return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
            }
            try
            {
                //IMChallengeBusinessBenefits imidea = IMChallengeBusinessBenefitsService.Get(tbenefitinput.Id);
                IMChallengeBusinessBenefits imidea = new IMChallengeBusinessBenefits();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    imidea = unitOfWork.IMChallengeBusinessBenefits.GetByID(tbenefitinput.Id);
                    //imidea.Id = tbenefitinput.Id;
                    //imidea.ItemId = tbenefitinput.ItemId;
                    imidea.Name = tbenefitinput.Name;
                    imidea.FTEHours = tbenefitinput.FTEHours;
                    imidea.Detail = tbenefitinput.Detail;
                    //imidea.CreatedBy = tbenefitinput.CreatedBy;
                    imidea.Value = tbenefitinput.Value;
                    imidea.LastUpdatedDate = DateTime.Now;
                    imidea.LastUpdatedUser = GetUserName(uid);
                    imidea.UpdatedBy = uid;
                    unitOfWork.IMChallengeBusinessBenefits.Update(imidea);
                    unitOfWork.Save();
                }
                //IMChallengeBusinessBenefitsService.Save();
                return Json(new { Result = "OK", Record = imidea });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult DeleteBusinessBenefitDetails(int Id)
        {
            try
            {
                IMChallengeBusinessBenefitsService.Delete(Id);
                return Json(new { Result = "OK" });
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
            IList<IMPostIdea> lst = IMPostChallengeInput.Get_Approved_IdeaItems;
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

        public ActionResult DisplayHomePage(int ItemId =0, string SaveData="", string TabName="home")
            {


            //var Client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();

            IMPostChallengeInput.ClientId = client.ClientId;
            //IMPostChallengeInput IMPostChallengeInput = new IMPostChallengeInput();
            //if (ClientId > 0)
            //    {
            //    IMPostChallengeInput.ClientId = ClientId;
            //    }
            if (ItemId > 0)
                {
                   IMPostChallengeInput.PostChallengeInput  =   IMPostChallangeMapper.MapToInput(service.Get(ItemId));
                   //IMPostChallengeInput.ClientId = IMPostChallengeInput.PostChallengeInput.ClientId;
                   //if (ClientId != IMPostChallengeInput.ClientId)
                   //{
                   //return RedirectToAction("DisplayHomePage", new { ClientId = IMPostChallengeInput.ClientId, ItemId = ItemId });
                   //}
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


        [ActionTrack]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SavePostChallenge(FormCollection form, IMPostChallengeInput inputs, string command, int ClientId = 0, int ItemId = 0, string PostCreate = "")

        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
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
                if (inputs.PostChallengeInput.StatusId == 0)
                {
                    inputs.PostChallengeInput.StatusId = (int)IMStatus.Created;
                }

                if (!ModelState.IsValid)
                    return View("ItemPostTypePage", inputs);


                //Get Login Id (ItemCreated By Id)


                if (ItemId > 0)
                {
                    inputs.PostChallengeInput.Id = ItemId;
                    inputs.PostChallengeInput.LastUpdatedDate = DateTime.Now;
                    inputs.PostChallengeInput.LastUpdatedUser = GetUserName(uid);
                    inputs.PostChallengeInput.UpdatedBy = uid;
                    IMPostChallangeMapper.MapToEntity(inputs.PostChallengeInput, service.Get(ItemId));

                    service.Save();

                }
                else
                {
                    if (Session["UserId"] == null)
                    {
                        postchallangeInput.CreatedBy = WebSecurity.GetUserId("amo");
                    }
                    else
                    {
                        postchallangeInput.CreatedByUser = GetUserName(uid);
                        postchallangeInput.CreatedBy = uid;
                    }
                    postchallangeInput.StatusId = (int)IMStatus.Created;
                    postchallangeInput.PriorityId = 5;
                    //postchallangeInput.CreatedUserId = uid;
                    postchallangeInput.CreatedDate = DateTime.Now;
                    ItemId = service.Create(AutoMapper.Mapper.Map<PostChallengeInput, IMPostChallenge>(postchallangeInput));

                    IMChallengePriorityHistory priorityHistory = new IMChallengePriorityHistory();
                    priorityHistory.OldPriorityId = (int)postchallangeInput.PriorityId;
                    priorityHistory.NewPriorityId = (int)postchallangeInput.PriorityId;
                    priorityHistory.ChallengeId = ItemId;
                    priorityHistory.CreatedBy = uid;
                    priorityHistory.UpdatedBy = uid;
                    priorityHistory.CreatedByUser = GetUserName(uid);
                    priorityHistory.LastUpdatedDate = DateTime.Now;
                    priorityHistory.CreatedDate = DateTime.Now;
                    priorityHistory.Comments = "Created(By default)";
                    priorityHistory.IsDeleted = false;
                    unitOfWork.IMChallengePriorityHistory.Insert(priorityHistory);

                    IMChallengeStatusHistory statusHistory = new IMChallengeStatusHistory();
                    statusHistory.ChallengeId = ItemId;
                    statusHistory.OldStatusId = (int)IMStatus.Created;//coz this is also not null
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.NewStatusId = (int)IMStatus.Created;// 1 for Created
                    statusHistory.CreatedBy = uid;
                    statusHistory.Comments = "Created(By default)";//Coz it is Not null;
                    unitOfWork.IMChallengeStatusHistory.Insert(statusHistory);
                        var m = unitOfWork.UserProfile.GetByID(uid);
                        var email = m.Email;
                        var fullname = m.FirstName + " " + m.LastName;
                        unitOfWork.Save();
                        SendIdeaNotification(email, fullname, postchallangeInput.ItemName, (IMStatus)postchallangeInput.StatusId, client.ClientName);
                    
                }

                //var e = service.Get(id);

                ModelState.AddModelError("Success", "Challenge posted successfully");

                SaveData = "true";
                TabName = "classification";
            }



            if (command == "savecategory")
            {
                TabName = "classification";
                SaveCategoryDetails(form, inputs, ClientId, ItemId);
                SaveData = "Save Category";
            }


            return RedirectToAction("DisplayHomePage", new { ItemId = ItemId, SaveData = SaveData, TabName = TabName });

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


            string[] selectedcategory = form.GetValues("challengeclassifcation");
            int userId = WebSecurity.GetUserId("amo");
            if (selectedcategory != null)
            {
                foreach (var item in selectedcategory)
                {
                    IMChallengeCategoryMap mapItem = new IMChallengeCategoryMap();
                    mapItem.CategoryId = Convert.ToInt32(item);
                    if (Session["UserId"] == null)
                    {
                        mapItem.CreatedBy = userId;
                    }
                    else
                    {
                        mapItem.CreatedBy = uid;
                    }
                    mapItem.ItemId = ItemId;
                    mapItem.CreatedByUser = GetUserName(uid);
                    mapItem.CreatedDate = DateTime.Now;
                    IMChallengeCategoryMapService.Create(mapItem);

                }

            }
            }
            


        

      


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
           // { }

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




        [ActionTrack]
        public ActionResult ChallengesApproval(int? page, string sortOrder)
        {
            int uid = (int)Session["UserId"];
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];

            ViewBag.Challenges = String.IsNullOrEmpty(sortOrder) ? "Challenge_desc" : "";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";    
            ViewBag.Description = string.IsNullOrEmpty(sortOrder) ? "Description_desc" : "Description";
            ViewBag.Createdby = string.IsNullOrEmpty(sortOrder) ? "Createdby_desc" : "Createdby";          
            ViewBag.LOB = string.IsNullOrEmpty(sortOrder) ? "LOB_desc" : "LOB";
            ViewBag.configitem = string.IsNullOrEmpty(sortOrder) ? "configitem_desc" : "configitem";

            var user = "";
            IList<ChallengeModel> x = new List<ChallengeModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                user = unitOfWork.UserProfile.Get(g => g.Id == uid).Select(g => g.RoleId).FirstOrDefault();
            }
            if (user.ToLower() == "superuser")
            {
                x = AllPostChallenges_SuperUser(client.ClientId);
            }
            else
            {
                x = AllPostChallenges(client.ClientId, uid);
            }

            // var x = AllPostChallenges(client.Id,uid)/*.ToPagedList(page ?? 1, 8)*/;
            IPagedList<ChallengeModel> challenges = null;
            switch (sortOrder)
            {
                case "Challenge_desc":
                    challenges = x.OrderByDescending(s => s.ChallengeName).ToPagedList(page ?? 1, 6);
                    break;
                case "Status_desc":
                    challenges = x.OrderByDescending(s => s.Status).ToPagedList(page ?? 1, 6);
                    break;
                case "Status":
                    challenges = x.OrderBy(s => s.Status).ToPagedList(page ?? 1, 6);
                    ViewBag.Status = "Status_desc";
                    break;
                case "Description_desc":
                    challenges = x.OrderByDescending(s => s.Description).ToPagedList(page ?? 1, 6);
                    break;
                case "Description":
                    challenges = x.OrderBy(s => s.Description).ToPagedList(page ?? 1, 6);
                    ViewBag.Description = "Description_desc";
                    break;
                case "Createdby_desc":
                    challenges = x.OrderByDescending(s => s.CreatedByUser).ToPagedList(page ?? 1, 6);
                    break;
                case "Createdby":
                    challenges = x.OrderBy(s => s.CreatedByUser).ToPagedList(page ?? 1, 6);
                    ViewBag.Createdby = "Createdby_desc";
                    break;
                case "LOB_desc":
                    challenges = x.OrderByDescending(s => s.LOB).ToPagedList(page ?? 1, 6);
                    break;
                case "LOB":
                    challenges = x.OrderBy(s => s.LOB).ToPagedList(page ?? 1, 6);
                    ViewBag.LOB = "LOB_desc";
                    break;
                case "configitem_desc":
                    challenges = x.OrderByDescending(s => s.ConfigItemName).ToPagedList(page ?? 1, 6);
                    break;
                case "configitem":
                    challenges = x.OrderBy(s => s.ConfigItemName).ToPagedList(page ?? 1, 6);
                    ViewBag.configitem = "configitem_desc";
                    break;

                default:
                    challenges = x.OrderBy(s => s.ChallengeName).ToPagedList(page ?? 1, 6);
                    break;
            }

            return View("ChallengesApproval", challenges);
            //return View("ChallengesApproval");
        }


        //[HttpPost]
        //public ActionResult Selection_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    int uid = (int)Session["UserId"];
        //    var client = (Client)Session["Client"];
        //    var x = AllPostChallenges(client.Id,uid);

        //    return Json(x.ToDataSourceResult(request));
        //}

        [HttpPost]
        public ActionResult Selection_Read([DataSourceRequest] DataSourceRequest request)
        {
            int uid = (int)Session["UserId"];
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];

            var user = "";
            IList<ChallengeModel> xcac = new List<ChallengeModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                user = unitOfWork.UserProfile.Get(g => g.Id == uid).Select(g => g.RoleId).FirstOrDefault();
            }
            if (user.ToLower() == "superuser")
            {
                xcac = AllPostChallenges_SuperUser(client.ClientId);
            }
            else
            {
                xcac = AllPostChallenges(client.ClientId, uid);
            }


            return Json(xcac.ToDataSourceResult(request));
        }

        public static IList<ChallengeModel> AllPostChallenges_SuperUser(int clientId)
        {

            Db db = new Db();
            var challengemod = (from posts in db.IMPostChallenge
                                join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                               // join CA in db.IMChallengeApprover on posts.Id equals CA.ChallengeId
                                //join idt in db.IMIdeaType on posts.IdeaTypeId equals idt.Id into tmpidt
                                //join ids in db.IMIdeaSource on posts.IdeaSourceId equals ids.Id into tmpids
                                join idst in db.IMItemStatus on posts.StatusId equals idst.Id into tmpidst
                                join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                                join lob in db.IMLOBSource on posts.LOBId equals lob.Id into tmplob
                                from status in tmpidst.DefaultIfEmpty()
                                    //from source in tmpids.DefaultIfEmpty()
                                    //from idtype in tmpidt.DefaultIfEmpty()
                                from apps in tmpapp.DefaultIfEmpty()
                                from lobs in tmplob.DefaultIfEmpty()
                                where posts.ClientId == clientId && posts.StatusId != (int)IMStatus.Implemented
                           select new ChallengeModel
                           {
                               //CategoryId = posts.CategoryId,
                               ConfigItem = posts.ConfigItem,
                               ConfigItemName = apps.ApplicationName,
                               CreatedByUser = usr.FirstName + " " + usr.LastName,
                               CreatedBy = posts.CreatedBy.Value,
                               UpdatedBy = posts.UpdatedBy.Value,
                               CreatedDate = posts.CreatedDate,
                               Description = posts.Description,
                               ChallengeId = posts.Id,
                               ChallengeName = posts.ItemName,

                               //IsDeleted = posts.IsDeleted,
                               //  ItemType= posts.id
                               LOB = lobs.LOB,
                               LOBId = posts.LOBId,
                               PriorityComments = posts.PriorityComments,
                               StatusComments = posts.StatusComments,
                               PriorityId = posts.PriorityId,
                               QtyFication = posts.QtyFication,
                               Status = status.Status,
                               StatusId = posts.StatusId,
                           }
                          ).ToList();
           return challengemod;

        }

        public static IList<ChallengeModel> AllPostChallenges(int clientId, int uid)
        {
            Db db = new Db();
            var challengemod = (from posts in db.IMPostChallenge
                           join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                           join CA in db.IMChallengeApprover on posts.Id equals CA.ChallengeId
                           //join idt in db.IMIdeaType on posts.IdeaTypeId equals idt.Id into tmpidt
                           //join ids in db.IMIdeaSource on posts.IdeaSourceId equals ids.Id into tmpids
                           join idst in db.IMItemStatus on posts.StatusId equals idst.Id into tmpidst
                           join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                           join lob in db.IMLOBSource on posts.LOBId equals lob.Id into tmplob
                           from status in tmpidst.DefaultIfEmpty()
                           //from source in tmpids.DefaultIfEmpty()
                           //from idtype in tmpidt.DefaultIfEmpty()
                           from apps in tmpapp.DefaultIfEmpty()
                           from lobs in tmplob.DefaultIfEmpty()
                           where posts.ClientId==clientId && CA.ApproverId == uid && posts.StatusId!=(int)IMStatus.Implemented
                           select new ChallengeModel
                           {
                               ClientId = posts.ClientId,
                               ConfigItem = posts.ConfigItem,
                               ConfigItemName = apps.ApplicationName,
                               CreatedByUser = usr.FirstName + " " + usr.LastName,
                               CreatedBy = posts.CreatedBy.Value,
                               UpdatedBy = posts.UpdatedBy.Value,
                               CreatedDate = posts.CreatedDate,
                               Description = posts.Description,
                               ChallengeId = posts.Id,
                               ChallengeName = posts.ItemName,
                               //IsDeleted = posts.IsDeleted,
                               //  ItemType= posts.id
                               LOB = lobs.LOB,
                               LOBId = posts.LOBId,
                               PriorityComments = posts.PriorityComments,
                               StatusComments = posts.StatusComments,
                               PriorityId = posts.PriorityId,
                               QtyFication = posts.QtyFication,
                               Status = status.Status,
                               StatusId = posts.StatusId,
                               
                           }
                          ).ToList();
            return challengemod;
            
        }
        


        public ActionResult testingUpdate(IList<ChallengeModel> inputs, string btnSubmit)
        {
            int uid = (int)Session["UserId"];
            //string[] ;
            ModelState.Clear();
            bool checkboxchecked =false;
            foreach (var c in inputs)
            {
                if (c.IsChecked == true)
                {
                    checkboxchecked = true;
                }
                if (c.IsChecked == true && (string.IsNullOrEmpty(c.StatusComments)  || c.PriorityId == 0))
                {

                    ModelState.AddModelError(c.ChallengeId + "mandate", c.ChallengeName + "- Priority and Comments are mandatory.");
                    
                }
            }
            if (!checkboxchecked)
            {
                ModelState.AddModelError("chkerr", "Please select any one of the challenges");
            }
            if (!ModelState.IsValid)
            {
                return View("ChallengesApproval", inputs.ToPagedList( 1, 8));
            }

            if (ModelState.IsValid)
            {
                foreach (var c in inputs)
                {
                    if (btnSubmit == "Approve")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)IMStatus.Created || c.StatusId == (int)IMStatus.Posted || c.StatusId == (int)IMStatus.Submitted || c.StatusId ==0 ))
                        {
                            updatestatus1(c.ChallengeId, IMStatus.Approved, c.StatusComments, c.PriorityId);
                            ApprovedDateUpdate(c.ChallengeId,uid);
                        }
                    }
                    if (btnSubmit == "Submit")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)IMStatus.Created || c.StatusId == (int)IMStatus.Posted || c.StatusId == (int)IMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.ChallengeId, IMStatus.Submitted, c.StatusComments, c.PriorityId);
                        }
                    }
                    if (btnSubmit == "Decline")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)IMStatus.Created || c.StatusId == (int)IMStatus.Posted || c.StatusId == (int)IMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.ChallengeId, IMStatus.Declined , c.StatusComments, c.PriorityId);
                        }
                    }
                    if (btnSubmit == "Close")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)IMStatus.Created || c.StatusId == (int)IMStatus.Posted || c.StatusId == (int)IMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.ChallengeId, IMStatus.Closed, c.StatusComments, c.PriorityId);
                        }
                    }
                }
                TempData.Add("ValidationSuccessMsg", "Status got updated successfully.");
            }

            //return View("ChallengesApproval", inputs);
           return RedirectToAction("ChallengesApproval");
        }
        public static void ApprovedDateUpdate(int ChallengeId, int uid)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var getRow = unitOfWork.IMChallengeApprover.Get(g => g.ChallengeId == ChallengeId && g.ApproverId == uid).FirstOrDefault();
                if (getRow != null)
                {
                    getRow.ApprovedDate = DateTime.Now;
                    unitOfWork.IMChallengeApprover.Update(getRow);
                    unitOfWork.Save();
                }
            }
            catch(Exception ex)
            {
                
            }
            
        }

        public void updatestatus1(int ChallengeId, IMStatus status, string Comments, int priority)
        {
            int uid = (int)Session["UserId"];
          //  var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                {
                    var challenge = unitOfWork.IMPostChallenege.GetByID(ChallengeId);
                    if (challenge != null)
                    {
                        var oldStatus = challenge.StatusId;
                        challenge.StatusId = (int)status; //2;// 2 for Submitted; 1 for created
                        challenge.StatusComments = Comments;
                        challenge.PriorityId = priority;
                        unitOfWork.IMPostChallenege.Update(challenge);

                        var oldPrioritObject = unitOfWork.IMChallengePriorityHistory.Get().Where(w => w.ChallengeId == ChallengeId).OrderByDescending(o => o.Id).FirstOrDefault();
                        int oldpId = 0;
                        if (oldPrioritObject != null)
                        {
                            oldpId = oldPrioritObject.NewPriorityId;
                        }
                        IMChallengePriorityHistory priorityHistory = new IMChallengePriorityHistory();
                        priorityHistory.OldPriorityId = oldpId;

                        priorityHistory.ChallengeId = ChallengeId;
                        priorityHistory.CreatedBy = uid;
                        priorityHistory.CreatedDate = DateTime.Now;
                        priorityHistory.CreatedByUser = GetUserName(uid);
                        priorityHistory.NewPriorityId = priority;
                        priorityHistory.UpdatedBy = uid;
                        priorityHistory.LastUpdatedDate = DateTime.Now;
                        priorityHistory.LastUpdatedUser = GetUserName(uid);
                        priorityHistory.Comments = Comments == null ? "" : Comments;
                        priorityHistory.IsDeleted = false;
                        unitOfWork.IMChallengePriorityHistory.Insert(priorityHistory);

                        IMChallengeStatusHistory statusHistory = new IMChallengeStatusHistory();
                        statusHistory.ChallengeId = challenge.Id;
                        statusHistory.Comments = challenge.StatusComments;
                        statusHistory.LastUpdatedDate = DateTime.Now;
                        statusHistory.UpdatedBy = uid;
                        statusHistory.OldStatusId = oldStatus;
                        statusHistory.NewStatusId = challenge.StatusId;
                        unitOfWork.IMChallengeStatusHistory.Insert(statusHistory);
                        //unitOfWork.Save();
                        //Db db = new Db();
                        //var listOfAdmins = (from UA in db.UserApplications
                        //                    join UP in db.UserProfiles on UA.UserId equals UP.Id
                        //                    join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                        //                    join UR in db.UserRoles on UA.RoleId equals UR.Id
                        //                    where ASA.ApplicationName.ToLower() == "idea manager" && UR.RoleName == "admin" && UA.ClientId == client.Id
                        //                    select UP.Id
                        //                   ).Distinct().ToList();
                        var listOfAdmins = unitOfWork.IMChallengeApprover.Get(g => g.ChallengeId == ChallengeId).Select(g => g.ApproverId).ToList();
                        foreach (var Adid in listOfAdmins)
                        {
                            var m1 = unitOfWork.UserProfile.GetByID(challenge.CreatedBy);
                            var email1 = m1.Email;
                            var fullname1 = m1.FirstName + " " + m1.LastName;
                            SendIdeaNotification(email1, fullname1, challenge.ItemName, (IMStatus)challenge.StatusId, client.ClientName);
                        }
                        var m = unitOfWork.UserProfile.GetByID(challenge.CreatedBy);
                        var email = m.Email;
                        var fullname = m.FirstName + " " + m.LastName;
                        SendIdeaNotification(email,fullname, challenge.ItemName, (IMStatus)challenge.StatusId, client.ClientName);
                    }
                }
                unitOfWork.Save();
            }

        }



        [ActionTrack]
        //public JsonResult ListOfAdmins()
        [HttpPost]
        public ActionResult ListOfAdmins([DataSourceRequest] DataSourceRequest request)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            Db db = new Db();
            var listOfAdmins = (from UA in db.UserApplications
                                join UP in db.UserProfiles on UA.UserId equals UP.Id
                                join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                join UR in db.UserRoles on UA.RoleId equals UR.Id
                                where ASA.ApplicationName.ToLower() == "idea manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin") && UA.ClientId == client.ClientId
                                select UP
                                            ).Distinct().ToList();
            var AdminsWithName = listOfAdmins.Select(g =>
                new SelectListItem
                {
                    Text = g.FirstName + " " + g.LastName,
                    Value = g.Id.ToString()
                }
                );
            //return Json(new { Result = "OK", Records = AdminsWithName });
            return Json(AdminsWithName.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult AddSelectedAdmins(int ItemId =0, string IdeaIds = "")
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            int uid = (int)Session["UserId"];
            string Result = "Success";
            try
            {
                if (IdeaIds == "")
                {
                    //throw new Exception("Select Approver!");
                    throw new Exception("1");
                }
                if(ItemId!=0 )
               // if (ItemId!=""|| ItemId!="0" )
                {
                    //string[] SelectedAdminIds = IdeaIds.Substring(1).Split(',');
                    string[] SelectedAdminIds = IdeaIds.Split(',');
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                       
                        var challenge= unitOfWork.IMPostChallenege.Get(g=>g.Id==ItemId).FirstOrDefault();
                        challenge.StatusId = (int)IMStatus.Posted;
                        challenge.LastUpdatedDate = DateTime.Now;
                        challenge.UpdatedBy = uid;
                        //challenge.LastUpdatedUser = GetUserName(uid);
                        unitOfWork.IMPostChallenege.Update(challenge);
                        foreach (var item in SelectedAdminIds)
                        {
                            if(item!=" "||item!=","||item!="")
                            {
                                IMChallengeApprover challengeApprover = new IMChallengeApprover();
                                challengeApprover.ChallengeId = Convert.ToInt32(ItemId);

                                challengeApprover.ApproverId = Convert.ToInt32(item);
                                unitOfWork.IMChallengeApprover.Insert(challengeApprover);

                                
                                var u = unitOfWork.UserProfile.Get(g => g.Id == challengeApprover.ApproverId).FirstOrDefault();
                                SendIdeaNotification(u.Email, u.FirstName + " " + u.LastName, challenge.ItemName, IMStatus.Posted, client.ClientName);

                            }
                        }
                        unitOfWork.Save();
                    }
                }
               
                else
                {
                    //throw new Exception("Select Idea/Submit the one!");
                    throw new Exception("2");
                }
                
            }
            catch (Exception exp)
            {
                //return Json(new { Result = "ERROR", Message = exp.Message });
                return Json(new { Result = "ERROR", Message = exp.Message });
            }
            
            return Json(Result, JsonRequestBehavior.AllowGet);
        }







        public ActionResult implementedStatus(int idid)
        {
            int uid = (int)Session["UserId"];
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                {
                    //var idea = unitOfWork.IMPostIdea.Get(e=>e.ItemName== ItemName).FirstOrDefault();
                    var challenge = unitOfWork.IMPostChallenege.GetByID(idid);
                    if (challenge != null)
                    {
                        var oldStatus = challenge.StatusId;
                        challenge.StatusId = (int)IMStatus.Implemented;

                        unitOfWork.IMPostChallenege.Update(challenge);


                        IMChallengeStatusHistory statusHistory = new IMChallengeStatusHistory();
                        statusHistory.ChallengeId = challenge.Id;
                        statusHistory.Comments = challenge.StatusComments;
                        statusHistory.LastUpdatedDate = DateTime.Now;
                        statusHistory.UpdatedBy = uid;
                        statusHistory.OldStatusId = oldStatus;
                        statusHistory.NewStatusId = challenge.StatusId;

                        unitOfWork.IMChallengeStatusHistory.Insert(statusHistory);
                        var m = unitOfWork.UserProfile.GetByID(challenge.CreatedBy);
                        var email = m.Email;
                       var fullname = m.FirstName + " " + m.LastName;
                       SendIdeaNotification(email,fullname, challenge.ItemName, (IMStatus)challenge.StatusId,client.ClientName);
                    }
                }
                unitOfWork.Save();
            }
            return RedirectToAction("DisplayChallenges");
        }


        [ActionTrack]
        public void SendIdeaNotification(string email, string fullname, string ChallengeName, IMStatus status,string clientname)
        {
            try
            {
                var body = "";

                var mailId = email;
                var Subject = "Nextgen IM AutoNotification";
                if (status == IMStatus.Created)
                {
                    body = ThankyouBody(fullname, clientname);
                }
                else if (status == IMStatus.Posted)
                {
                    //fulllname = Admin Name(Approver)
                    body = ChallengeApproverBody(fullname, ChallengeName, status.ToString(), clientname);
                }
                else
                {
                    body = StatusBody(fullname, ChallengeName, status.ToString(), clientname);
                }

                IdeaManagerHelper.SendEMail(mailId, Subject, body);
            }
            catch
            {

            }

            // return;
        }

        private string ThankyouBody(string username, string clientname)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/ThankYouChallengeNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{clientname}", clientname);

            return body;
        }

        [ActionTrack]
        private string ChallengeApproverBody(string adminName, string Challenge, string status, string clientname)
        {
            int uid = (int)Session["UserId"];
            var username = "";
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var som = unitOfWork.UserProfile.GetByID(uid);
                username = som.FirstName + som.LastName;
            }

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/ChallengeApproverEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{challengename}", Challenge);
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{Adminname}", adminName);

            return body;
        }

        private string StatusBody(string username, string Challenge, string status, string clientname)
        {
            int uid = (int)Session["UserId"];
            var adminName = "";
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var som = unitOfWork.UserProfile.GetByID(uid);
                adminName = som.FirstName + som.LastName;
            }
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/ChallengeStatusEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{ideaname}", Challenge);
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{Adminname}", adminName);

            return body;
        }






        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        public ActionResult GetMetricTarget([DataSourceRequest]DataSourceRequest request, int ChallengeId)
        {
            try
            {
                IList<IMMetricTarget> list = new List<IMMetricTarget>();
                //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];

                if (ChallengeId != 0 || ChallengeId != null)
                {
                    list = IdeaManagerHelper.GetMetricTargetDetails(ChallengeId);

                }
                return Json(list.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                return null;
                }
            }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult AddMetricTarget( MetricTargetModel metrictargetmodel, string ChallengeId)
        {
           
                if (metrictargetmodel != null && ModelState.IsValid)
                {
                var x = Convert.ToInt32(ChallengeId);
                      int usid = (int)Session["UserId"];
                      metrictargetmodel.CreatedBy = usid;
                      metrictargetmodel.UpdatedBy = usid;
                      
                      var res = IdeaManagerHelper.CreateMetricTarget(metrictargetmodel, x);
                      metrictargetmodel.Id = res;
                }
                return Json(new[] { metrictargetmodel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult EditMetricTarget([DataSourceRequest] DataSourceRequest request, MetricTargetModel metrictargetmodel)
        {
            if (metrictargetmodel != null && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                metrictargetmodel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateMetricTarget(metrictargetmodel);
            }

            return Json(new[] { metrictargetmodel }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult DeleteMetricTarget([DataSourceRequest] DataSourceRequest request, MetricTargetModel metrictargetmodel)
        {
            if (metrictargetmodel != null)
            {
                var v = IdeaManagerHelper.RemoveMetricTarget(metrictargetmodel.Id);
            }

            return Json(new[] { metrictargetmodel }.ToDataSourceResult(request, ModelState));
        }


        //public int updatestatus(string ff, IMStatus status, string Comments)
        //{
        //    int uid = (int)Session["UserId"];
        //    int x = 0;
        //    string[] ids = ff.Split(',');
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        foreach (string i in ids)
        //        {
        //            var challenge = unitOfWork.IMPostChallenege.GetByID(Convert.ToInt32(i));
        //            if (challenge != null)
        //            {
        //                var oldStatus = challenge.StatusId;
        //                challenge.StatusId = (int)status; //2;// 2 for Submitted; 1 for created
        //                challenge.StatusComments = Comments;
        //                unitOfWork.IMPostChallenege.Update(challenge);
        //                //unitOfWork.Save();
        //                x = 1;
        //                IMChallengeStatusHistory statusHistory = new IMChallengeStatusHistory();
        //                statusHistory.ChallengeId = challenge.Id;
        //                statusHistory.Comments = challenge.StatusComments;
        //                statusHistory.LastUpdatedDate = DateTime.Now;
        //                statusHistory.UpdatedBy = uid;
        //                statusHistory.OldStatusId = oldStatus;
        //                statusHistory.NewStatusId = challenge.StatusId;
        //                unitOfWork.IMChallengeStatusHistory.Insert(statusHistory);
        //                //unitOfWork.Save();
        //                //var m = unitOfWork.UserProfile.GetByID(idea.CreatedUserId);
        //                //var email = m.Email;
        //                //var fullname = m.FirstName + " " + m.LastName;
        //                //SendIdeaNotification(email, fullname, idea.ItemName, (IMStatus)idea.StatusId);
        //            }
        //        }
        //        unitOfWork.Save();
        //    }

        //    return x;
        //}

    }
}
    
