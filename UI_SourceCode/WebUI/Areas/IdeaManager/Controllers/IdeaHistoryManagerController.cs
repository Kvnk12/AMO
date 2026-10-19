using Data;
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
using WebUI.Dto;
namespace WebUI.Areas.IdeaManager.Controllers
{
    public class IdeaHistoryManagerController : Cruder<IMPostIdea, IMPostIdeaInput>
        {

        //Entities
        private IMPostIdea entityIMPostIdea;
        private IMItemCategoryMap entityIMIdeaCategoryMap;
        private IMItemITBenefits entityIMIdeaITBenefits;
        private IMItemBusinessBenefits entityIMIdeaBusinessBenefits;
        private IMItemComments entityIMIdeaComments;
        //private IMIdeaIdeasRelations entityIMIdeaIdeasRelations;
        private IMIdeaPriorityHistory entityIMIdeaPriorityHistory;
        private IMIdeaStatusHistory entityIMIdeaStatusHistory;

        



        //IMIdeaIdeasRelations

        //IMIdeaBusinessBenefits

        //Service

        private CrudService<IMPostIdea> IMPostIdeaService;
        private CrudService<IMItemCategoryMap> IMIdeaCategoryMapService;
        private CrudService<IMItemITBenefits> IMIdeaITBenefitsService;
        private CrudService<IMItemBusinessBenefits> IMIdeaBusinessBenefitsService;
        private CrudService<IMItemComments> IMIdeaCommentsService;
        //private CrudService<IMIdeaIdeasRelations> IMIdeaIdeasRelationsService;
        //private CrudService<IMIdeaPriorityHistory> IMIdeaPriorityHistoryService;
        private CrudService<IMIdeaStatusHistory> IMIdeaStatusHistoryService;
        


        //Mappers

        private Mapper<IMPostIdea, IMPostIdeaInput> IMPostIdeaMapper;
        private Mapper<IMPostIdea, PostIdeaInput> IMPostChallangeMapper;
        private Mapper<IMItemCategoryMap, IMItemCategoryMapInput> IMIdeaCategoryMapMapper;
        private Mapper<IMItemITBenefits, IMItemITBenefitsInput> IMIdeaITBenefitsMapper;
        private Mapper<IMItemBusinessBenefits, IMItemBusinessBenefitsInput> IMIdeaBusinessBenefitsMapper;
        private Mapper<IMItemComments, IMItemCommentsInput> IMIdeaCommentsMapper;

        


        //Repo

        private Repo<IMPostIdea> repoIMPostIdea;
        private Repo<IMItemCategoryMap> repoIMIdeaCategoryMap;
        private Repo<IMItemITBenefits> repoIMIdeaITBenefits;
        private Repo<IMItemBusinessBenefits> repoIMIdeaBusinessBenefits;
        private Repo<IMItemComments> repoIMIdeaComments;
       // private Repo<IMIdeaPriorityHistory> repoIMIdeaPriorityHistory;
        private Repo<IMIdeaStatusHistory> repoIMIdeaStatusHistory;

        


        public IdeaHistoryManagerController(ICrudService<IMPostIdea> service, IMapper<IMPostIdea, IMPostIdeaInput> v)
            : base(service, v)
            {

            
            DbContextFactory Dbcon = new DbContextFactory();
            repoIMPostIdea = new Repo<IMPostIdea>(Dbcon);
            IMPostIdeaMapper = new Mapper<IMPostIdea, IMPostIdeaInput>();
            IMPostChallangeMapper = new Mapper<IMPostIdea, PostIdeaInput>();
            entityIMPostIdea = new IMPostIdea();


            repoIMIdeaCategoryMap = new Repo<IMItemCategoryMap>(Dbcon);
            IMIdeaCategoryMapMapper = new Mapper<IMItemCategoryMap, IMItemCategoryMapInput>();
            entityIMIdeaCategoryMap = new IMItemCategoryMap();


            repoIMIdeaITBenefits = new Repo<IMItemITBenefits>(Dbcon);
            IMIdeaITBenefitsMapper = new Mapper<IMItemITBenefits,IMItemITBenefitsInput>();
            entityIMIdeaITBenefits = new IMItemITBenefits();
            IMIdeaITBenefitsService = new CrudService<IMItemITBenefits>(repoIMIdeaITBenefits);


            repoIMIdeaBusinessBenefits = new Repo<IMItemBusinessBenefits>(Dbcon);
            IMIdeaBusinessBenefitsMapper = new Mapper<IMItemBusinessBenefits, IMItemBusinessBenefitsInput>();
            entityIMIdeaBusinessBenefits = new IMItemBusinessBenefits();
            IMIdeaBusinessBenefitsService = new CrudService<IMItemBusinessBenefits>(repoIMIdeaBusinessBenefits);

            repoIMIdeaComments = new Repo<IMItemComments>(Dbcon);
            IMIdeaCommentsMapper = new Mapper<IMItemComments, IMItemCommentsInput>();
            entityIMIdeaComments = new IMItemComments();
            IMIdeaCommentsService = new CrudService<IMItemComments>(repoIMIdeaComments);



            //repoIMIdeaPriorityHistory = new Repo<IMIdeaPriorityHistory>(Dbcon);
            //IMIdeaPriorityHistoryService = new CrudService<IMIdeaPriorityHistory>(repoIMIdeaPriorityHistory);


            repoIMIdeaStatusHistory = new Repo<IMIdeaStatusHistory>(Dbcon);
            IMIdeaStatusHistoryService = new CrudService<IMIdeaStatusHistory>(repoIMIdeaStatusHistory);

            



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
        

        public ActionResult DisplayIdeas(int? page, string sortOrder)
        {
            Db db = new Db();
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IPagedList<IMPostIdea> IMPostIdeaInput;
            
            ViewBag.LastModifiedDate = string.IsNullOrEmpty(sortOrder) ? "LastModifiedDate_desc" : "LastModifiedDate";
            ViewBag.ideas = string.IsNullOrEmpty(sortOrder) ? "ideas_desc" : "ideas";
            ViewBag.Postedby = string.IsNullOrEmpty(sortOrder) ? "Postedby_desc" : "Postedby";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            ViewBag.StatusComments = string.IsNullOrEmpty(sortOrder) ? "StatusComments_desc" : "StatusComments";
            ViewBag.CreatedDate = string.IsNullOrEmpty(sortOrder) ? "CreatedDate_desc" : "CreatedDate";

            var displayideas = from d in db.IMPostIdea
                               join usrcreated in db.UserProfiles on d.CreatedBy equals usrcreated.Id
                               join usrUpdated in db.UserProfiles on d.UpdatedBy equals usrUpdated.Id into lstusr
                               from usrUpdated in lstusr.DefaultIfEmpty()
                               where d.ClientId == client.ClientId
                               select new IMPostIdeaModel
                               {
                                   //ActualEndDate = d.ActualEndDate,
                                   Id = d.Id,
                                   UpdatedBy = d.UpdatedBy,
                                   ClientId = d.ClientId,

                                   CreatedBy = d.CreatedBy,
                                   CreatedByUser = usrcreated.FirstName + " " + usrcreated.LastName,
                                   CreatedDate = d.CreatedDate,

                                   ItemName = d.ItemName,
                                   //ItemTypeId = d.ItemTypeId,
                                   LastUpdatedDate = d.LastUpdatedDate,
                                   LastUpdatedUser = usrUpdated.FirstName + " " + usrUpdated.LastName,

                                   StatusComments = d.StatusComments,
                                   StatusId = d.StatusId

                               };

            //var displayideas = IdeaManagerHelper.GetPostItems(client.Id);


            switch (sortOrder)
            {              
                case "LastModifiedDate_desc":
                    displayideas = displayideas.OrderByDescending(d => d.LastUpdatedDate);
                    break;
                case "LastModifiedDate":
                    displayideas = displayideas.OrderBy(d => d.LastUpdatedDate);
                    ViewBag.LastModifiedDate = "LastModifiedDate_desc";
                    break;

                case "ideas_desc":
                    displayideas = displayideas.OrderByDescending(d => d.ItemName);
                    break;
                case "ideas":
                    displayideas = displayideas.OrderBy(d => d.ItemName);
                    ViewBag.ideas = "ideas_desc";
                    break;

                case "Postedby_desc":
                    displayideas = displayideas.OrderByDescending(d => d.CreatedBy);
                    break;
                case "Postedby":
                    displayideas = displayideas.OrderBy(d => d.CreatedBy);
                    ViewBag.Postedby = "Postedby_desc";
                    break;

                case "Status_desc":
                    displayideas = displayideas.OrderByDescending(d => d.StatusId);
                    break;
                case "Status":
                    displayideas = displayideas.OrderBy(d => d.StatusId);
                    ViewBag.Status = "Status_desc";
                    break;

                case "StatusComments_desc":
                    displayideas = displayideas.OrderByDescending(d => d.StatusComments);
                    break;
                case "StatusComments":
                    displayideas = displayideas.OrderBy(d => d.StatusComments);
                    ViewBag.StatusComments = "StatusComments_desc";
                    break;

                case "CreatedDate_desc":
                    displayideas = displayideas.OrderByDescending(d => d.CreatedDate);
                    break;
                case "CreatedDate":
                    displayideas = displayideas.OrderBy(d => d.CreatedDate);
                    ViewBag.CreatedDate = "CreatedDate_desc";
                    break;



                default:
                    displayideas = displayideas.OrderBy(d => d.ItemName);
                    break;
            }


           // var x = displayideas.ToList();

           // IMPostIdeaInput = x.ToPagedList(page == null ? 1 : page.Value, 8);       
            return View("SetupItems", displayideas.ToPagedList(page == null ? 1 : page.Value, 8));
        }


        public ActionResult DisplayITBenefits(int ItemId = 0)
        {
        IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
        return View("DisplayITBenefits");
        }


        [HttpPost]
        public JsonResult AddITBenefitDetails(IMItemITBenefitsInput tbenefitinput, int ItemId=0)
            {
            int uid = (int)Session["UserId"];
            try
                {

                //Update tbenefit Details

                //tbenefitinput.CreatedByUser = User.Identity.Name;
                tbenefitinput.CreatedByUser = GetUserName(uid);
                tbenefitinput.CreatedBy = uid;
                tbenefitinput.CreatedDate = DateTime.Now;
                tbenefitinput.LastUpdatedDate = DateTime.Now;
                tbenefitinput.LastUpdatedUser = tbenefitinput.CreatedByUser;
                tbenefitinput.UpdatedBy = uid;
                int Id = IMIdeaITBenefitsService.Create(IMIdeaITBenefitsMapper.MapToEntity(tbenefitinput, entityIMIdeaITBenefits));
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



        //[HttpPost]
        //public JsonResult AddSelectedIdeas(int ItemId = 0, int ClientId = 0, string IdeaIds = "")
        //    {

        //    string Result = "Success";
        //    try
        //        {
        //        string[] SelectedIdeaIds = IdeaIds.Substring(1).Split(',');
        //        foreach (var item in SelectedIdeaIds)
        //            {

        //            IMIdeaIdeasRelations IMIdeaIdeasRelations = new IMIdeaIdeasRelations();
        //            IMIdeaIdeasRelations.IdeaId = ItemId;
        //            IMIdeaIdeasRelations.IdeaId = int.Parse(item);
        //            IMIdeaIdeasRelations.CreatedByUser = User.Identity.Name;
        //            IMIdeaIdeasRelations.CreatedDate = DateTime.Now;
        //            IMIdeaIdeasRelations.LastUpdatedDate = DateTime.Now;
        //            IMIdeaIdeasRelations.LastUpdatedUser = User.Identity.Name;

        //            int Id = IMIdeaIdeasRelationsService.Create(IMIdeaIdeasRelations);

        //            }
        //        }
        //    catch (Exception exp)
        //    {
        //        Result = "Failure";
        //    }



        //        //Update tbenefit Details

        //        //    tbenefitinput.CreatedByUser = User.Identity.Name;
        //        //    tbenefitinput.CreatedDate = DateTime.Now;
        //        //    tbenefitinput.LastUpdatedDate = DateTime.Now;
        //        //    tbenefitinput.LastUpdatedUser = User.Identity.Name;

        //        //    int Id = IMIdeaCommentsService.Create(IMIdeaCommentsMapper.MapToEntity(tbenefitinput, entityIMIdeaComments));
        //        //    if (Id > 0)
        //        //        {
        //        //        //return (GetCommentsDetails(ItemId));
        //        //        return Json(new { Result = "OK", Record = tbenefitinput });
        //        //        }
        //        //    else
        //        //        {
        //        //        throw new Exception("Error in saving the data");
        //        //        }

        //        //    }
        //        //catch (Exception ex)
        //        //    {
        //        //    return Json(new { Result = "ERROR", Message = ex.Message });
        //        //    }
        //      return Json(Result, JsonRequestBehavior.AllowGet);
        //    //return Json(new { Result = "Success" });
        //    }



        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddCommentDetails(IMItemCommentsInput tbenefitinput, int ItemId = 0)
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
                int Id = IMIdeaCommentsService.Create(IMIdeaCommentsMapper.MapToEntity(tbenefitinput, entityIMIdeaComments));
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
        public JsonResult UpdateStatusHistory(int IdeaId = 0, int SelectedStatusId = 0, string Comments = "")
        {
            int uid = (int)Session["UserId"];
            bool updateSuccess = false;
            try
                {
                IMPostIdea PostIdea = service.Get(IdeaId);
                int oldStatusId = PostIdea.PriorityId;
                PostIdea.StatusId = SelectedStatusId;
                PostIdea.StatusComments = Comments;
                PostIdea.LastUpdatedDate = DateTime.Now;
                //PostIdea.LastUpdatedUser = GetUserName(uid);
                service.Save();
                //Priroty update history

                IMIdeaStatusHistory IMIdeaStatusHistory = new IMIdeaStatusHistory()
                {
                    IdeaId = IdeaId,
                    OldStatusId = oldStatusId,
                    NewStatusId = SelectedStatusId,
                    Comments = Comments,
                    IsDeleted = false,
                    CreatedByUser =GetUserName(uid),
                    CreatedDate = DateTime.Now
                };
                var createdId = IMIdeaStatusHistoryService.Create(IMIdeaStatusHistory);




                updateSuccess = true;
                }
            finally
                {
                updateSuccess = false;
                }


            return Json(updateSuccess, JsonRequestBehavior.AllowGet);

            }



        //[HttpPost]
        //public JsonResult UpdatePriorityHistory(int IdeaId = 0, int SelectedPriorityId = 0, string Comments = "")
        //{
        //    int uid = (int)Session["UserId"];
        //    bool updateSuccess = false;
        //try
        //    {
        //    IMPostIdea PostIdea = service.Get(IdeaId);
        //    int oldPriorityId = PostIdea.PriorityId;
        //    PostIdea.PriorityId = SelectedPriorityId;
        //    PostIdea.PriorityComments = Comments;
        //    PostIdea.LastUpdatedDate = DateTime.Now;
        //    PostIdea.LastUpdatedUser = GetUserName(uid);
        //    service.Save();
        //    //Priroty update history

        //    IMIdeaPriorityHistory imchallengPriroty = new IMIdeaPriorityHistory()
        //    {
        //        IdeaId = IdeaId,
        //        OldPriorityId = oldPriorityId,
        //        NewPriorityId = SelectedPriorityId,
        //        Comments = Comments,
        //        IsDeleted = false,
        //        CreatedByUser = GetUserName(uid),
        //        CreatedDate = DateTime.Now
        //    };
        //    var createdId = IMIdeaPriorityHistoryService.Create(imchallengPriroty);
                



        //    updateSuccess = true;
        //    }
        //finally
        //{
        //updateSuccess = false;            
        //}


        //return Json(updateSuccess, JsonRequestBehavior.AllowGet);
            
        //}

        [HttpPost]
        public JsonResult CheckCurrentPriority(int IdeaId=0, int selectedPrioiryId=0)
        {
        bool resultFound = false;
         IMPostIdea PostIdea =  service.Get(IdeaId);
         if (PostIdea.PriorityId == selectedPrioiryId)
             {
             resultFound = true;
             }
         else
             resultFound = false;

                return Json(resultFound, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult CheckCurrentStatus(int IdeaId = 0, int SelectedStatusId = 0)
            {
            bool resultFound = false;
            IMPostIdea PostIdea = service.Get(IdeaId);
            if (PostIdea.PriorityId == SelectedStatusId)
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

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            IMPostIdeaInput.Id = ItemId;
            IList<IMItemComments> lst = IMPostIdeaInput.GetIdeaComments;
            //lst.Add(new IMIdeaITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
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
        public JsonResult AddBusinessBenefitDetails(IMItemBusinessBenefitsInput tbenefitinput, int ItemId = 0)
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
                int Id = IMIdeaBusinessBenefitsService.Create(IMIdeaBusinessBenefitsMapper.MapToEntity(tbenefitinput, entityIMIdeaBusinessBenefits));
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

        IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
        IMPostIdeaInput.Id = ItemId;
        IList<IMItemITBenefits> lst = IMPostIdeaInput.GetITBenefits;  
        //lst.Add(new IMIdeaITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
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

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            IMPostIdeaInput.Id = ItemId;
            IList<vwIdeaPriorityHistory> lst = IMPostIdeaInput.GetIdeaPriorityHistoryDetails;
            //lst.Add(new IMIdeaITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
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

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            IMPostIdeaInput.Id = ItemId;
            IList<vwIdeaStatusHistory> lst = IMPostIdeaInput.GetIdeaStatusHistoryDetails;
            //lst.Add(new IMIdeaITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
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


        //[HttpPost]
        //public JsonResult GetListedIdeas(int ClientId = 0, int IdeaId = 0)
        //    {

        //    IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
        //    IMPostIdeaInput.ClientId = ClientId;
        //    IMPostIdeaInput.IdeaId = IdeaId;
        //    IList<vwIMSuggestIdeas> lstForIdeaId = IMPostIdeaInput.GetSuggestIdeasForIdeaId;


        //    //lst.Add(new IMIdeaITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
        //    try
        //        {
        //        //Get data from database
        //        //Return result to jTable
        //        return Json(new { Result = "OK", Records = lstForIdeaId });
        //        }
        //    catch (Exception ex)
        //        {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //        }


        //    }


        //[HttpPost]
        //public JsonResult GetListedIdeaDetails(int ClientId = 0, int IdeaId=0)
        //    {

        //    IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
        //    IMPostIdeaInput.ClientId = ClientId;
        //    IMPostIdeaInput.IdeaId = IdeaId;
        //    IList<IMPostIdea> lst = IMPostIdeaInput.GetSuggestIdeas;
        //    IList<vwIMSuggestIdeas> lstForIdeaId =IMPostIdeaInput.GetSuggestIdeasForIdeaId;
        //    foreach (vwIMSuggestIdeas item in lstForIdeaId)
        //    {
        //    List<IMPostIdea> itemToRemove = lst.Where(c => c.Id == item.IdeaId).ToList();
        //    foreach (IMPostIdea delitem in itemToRemove)
        //    {
        //    lst.Remove(delitem);
        //    }

        //    }


        //    //lst.Add(new IMIdeaITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
        //    try
        //        {
        //        //Get data from database
        //        //Return result to jTable
        //        return Json(new { Result = "OK", Records = lst });
        //        }
        //    catch (Exception ex)
        //        {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //        }


        //    }



        [ActionTrack]
        [HttpPost]
        public JsonResult GetIdeaDetails(int ClientId = 0)
            {

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            IMPostIdeaInput.ClientId = ClientId;
            IList<IMPostIdea> lst = IMPostIdeaInput.GetIdeaItems;
            //lst.Add(new IMIdeaITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
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

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            IMPostIdeaInput.Id = ItemId;
            IList<IMItemBusinessBenefits> lst = IMPostIdeaInput.GetBusinessBenefits;
            //lst.Add(new IMIdeaITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
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
        public ActionResult DislayIdeaHistory(int ClientId = 0, int ItemId = 0)
        {

        IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
        IMPostIdeaInput.ClientId = ClientId;
        IMPostIdeaInput.Id = ItemId;
        IMPostIdeaInput.PostIdeaInput = IMPostChallangeMapper.MapToInput(service.Get(ItemId));
        return View("ItemPostTypePage", IMPostIdeaInput);

        }

        public ActionResult DisplayHomePage(int ClientId = 0, int ItemId =0, string SaveData="", string TabName="home")
            {

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            if (ClientId > 0)
                {
                IMPostIdeaInput.ClientId = ClientId;
                }
            if (ItemId > 0)
                {
                   IMPostIdeaInput.PostIdeaInput  =   IMPostChallangeMapper.MapToInput(service.Get(ItemId));
                   IMPostIdeaInput.ClientId = IMPostIdeaInput.PostIdeaInput.ClientId;
                   if (ClientId != IMPostIdeaInput.ClientId)
                   {
                   return RedirectToAction("DisplayHomePage", new { ClientId = IMPostIdeaInput.ClientId, ItemId = ItemId });
                   }
                }
            else
                {
                IMPostIdeaInput.PostIdeaInput = new PostIdeaInput();
                }

            IMPostIdeaInput.TabName = TabName;
            IMPostIdeaInput.Id = ItemId;

            if (SaveData.Length > 0)
            {
            ModelState.AddModelError("Success", "Data saved successfully");
            }
            return View("ItemPostTypePage", IMPostIdeaInput);
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
        public ActionResult SavePostIdea(FormCollection form, IMPostIdeaInput inputs, string command, int ClientId = 0, int ItemId = 0, string PostCreate = "")
            {
            string TabName = "";
            string SaveData = "";
            int uid = (int)Session["UserId"];
            if (command == "saveidea")
                {
                TabName = "home";
                PostIdeaInput postchallangeInput = inputs.PostIdeaInput;
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
                    postchallangeInput.CreatedBy = (int)Session["UserId"];
                }

                postchallangeInput.UpdatedBy = uid; 
                if (ItemId > 0)
                    {
                    inputs.PostIdeaInput.Id = ItemId;
                    IMPostChallangeMapper.MapToEntity(inputs.PostIdeaInput, service.Get(ItemId));

                    service.Save();

                    }
                else
                    {
                    ItemId = service.Create(AutoMapper.Mapper.Map<PostIdeaInput, IMPostIdea>(postchallangeInput));

                    }

                //var e = service.Get(id);

                ModelState.AddModelError("Success", "Idea posted successfully");

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

        public void SaveCategoryDetails(FormCollection form, IMPostIdeaInput inputs, int ClientId, int ItemId)
            {
            int uid = (int)Session["UserId"];
            string SaveData = "Save Category";
            IMIdeaCategoryMapService = new CrudService<IMItemCategoryMap>(repoIMIdeaCategoryMap);
            if (ItemId > 0)
            {
               IList<IMItemCategoryMap> lst =  IMIdeaCategoryMapService.GetAll().Where(c => c.ItemId == ItemId).ToList();
               foreach (IMItemCategoryMap item in lst)
               {
               IMIdeaCategoryMapService.Delete(item.Id);
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
                            IMItemCategoryMap mapItem = new IMItemCategoryMap();
                            mapItem.CategoryId = Convert.ToInt32(item.Substring(0, item.IndexOf("category")));
                            if (Session["UserId"] == null)
                            {
                                mapItem.CreatedUserId = WebSecurity.GetUserId("amo");
                            }
                            else {
                                mapItem.CreatedUserId = uid;
                            }
                            mapItem.ItemId = ItemId;
                            mapItem.CreatedByUser = GetUserName(uid);
                            mapItem.CreatedUserId = uid;
                            mapItem.CreatedDate = DateTime.Now;
                            mapItem.UpdatedBy = uid;
                            mapItem.LastUpdatedDate = DateTime.Now;
                            
                            IMIdeaCategoryMapService.Create(mapItem);
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
            var existItemNames = from IMPostIdea in db.IMPostIdea
                                    where IMPostIdea.ItemName.ToLower() == ItemName.ToLower()
                                    select new
                                    {
                                        Id = IMPostIdea.Id,
                                        Name = IMPostIdea.ItemName
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
        public JsonResult GetApproversList(int IdeaId)
        {
            Db db = new Db();
            var listOfAdmins = (from UP in db.UserProfiles
                                let fN = UP.FirstName
                                let lN = UP.LastName
                                join CA in db.IMIdeaApprover on UP.Id equals CA.ApproverId
                                //let Dt =  CA.ApprovedDate
                                where CA.IdeaId == IdeaId
                                select new ApproverModel() { FirstName = fN, LastName = lN, ApprovedDate = CA.ApprovedDate }
                                            ).Distinct().ToList();
            var slectNames = listOfAdmins.Select(c => new
            {
                Name = string.Format("{0} {1}", c.FirstName, c.LastName),
                ApprovedDate = c.ApprovedDate == null ? "" : c.ApprovedDate.Value.ToShortDateString()
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
    
