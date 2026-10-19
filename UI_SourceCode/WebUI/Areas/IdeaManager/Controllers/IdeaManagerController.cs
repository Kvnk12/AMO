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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebMatrix.WebData;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IdeaManager.Objects;
using Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;
using System.Data.Entity;
using WebUI.Dto;

namespace WebUI.Areas.IdeaManager.Controllers
{
    public class IdeaManagerController : Cruder<IMPostIdea, IMPostIdeaInput>
    {

        //Entities
        private IMPostIdea entityIMPostIdea;
        private IMItemCategoryMap entityIMItemCategoryMap;
        private IMItemITBenefits entityIMItemITBenefits;
        private IMItemBusinessBenefits entityIMItemBusinessBenefits;
        private IMItemComments entityIMItemComments;
        //private IMIdeaPriorityHistory entityIMIdeaPriorityHistory;
        //private IMIdeaStatusHistory entityIMIdeaStatusHistory;
        //private IMIdeaSource entityIMIdeaSource;
        //private IMIdeaType entityIMIdeaType;



        //IMItemBusinessBenefits

        //Service

        //private CrudService<IMPostIdea> IMPostIdeaService;
        private CrudService<IMItemCategoryMap> IMItemCategoryMapService;
        private CrudService<IMItemITBenefits> IMItemITBenefitsService;
        private CrudService<IMItemBusinessBenefits> IMItemBusinessBenefitsService;
        private CrudService<IMItemComments> IMItemCommentsService;
        //private CrudService<IMIdeaPriorityHistory> IMIdeaPriorityHistoryService;
        private CrudService<IMIdeaStatusHistory> IMIdeaStatusHistoryService;
        private CrudService<IMIdeaSource> IMIdeaSourceService;
        private CrudService<IMIdeaType> IMIdeaTypeService;
        private CrudService<IMLOB> IMLOBService;


        //Mappers

        private Mapper<IMPostIdea, IMPostIdeaInput> IMPostIdeaMapper;
        private Mapper<IMPostIdea, PostIdeaInput> IMPostChallangeMapper;
        private Mapper<IMItemCategoryMap, IMItemCategoryMapInput> IMItemCategoryMapMapper;
        private Mapper<IMItemITBenefits, IMItemITBenefitsInput> IMItemITBenefitsMapper;
        private Mapper<IMItemBusinessBenefits, IMItemBusinessBenefitsInput> IMItemBusinessBenefitsMapper;
        private Mapper<IMItemComments, IMItemCommentsInput> IMItemCommentsMapper;


        //Repo

        private Repo<IMPostIdea> repoIMPostIdea;
        private Repo<IMItemCategoryMap> repoIMItemCategoryMap;
        private Repo<IMItemITBenefits> repoIMItemITBenefits;
        private Repo<IMItemBusinessBenefits> repoIMItemBusinessBenefits;
        private Repo<IMItemComments> repoIMItemComments;
       // private Repo<IMIdeaPriorityHistory> repoIMIdeaPriorityHistory;
        private Repo<IMIdeaStatusHistory> repoIMIdeaStatusHistory;
        private Repo<IMIdeaType> repoIMIdeaType;
        private Repo<IMIdeaSource> repoIMIdeaSource;
        private Repo<IMLOB> repoIMLOB;







        public IdeaManagerController(ICrudService<IMPostIdea> service, IMapper<IMPostIdea, IMPostIdeaInput> v)
            : base(service, v)
        {


            DbContextFactory Dbcon = new DbContextFactory();
            repoIMPostIdea = new Repo<IMPostIdea>(Dbcon);
            IMPostIdeaMapper = new Mapper<IMPostIdea, IMPostIdeaInput>();
            IMPostChallangeMapper = new Mapper<IMPostIdea, PostIdeaInput>();
            entityIMPostIdea = new IMPostIdea();

            repoIMItemCategoryMap = new Repo<IMItemCategoryMap>(Dbcon);
            IMItemCategoryMapMapper = new Mapper<IMItemCategoryMap, IMItemCategoryMapInput>();
            entityIMItemCategoryMap = new IMItemCategoryMap();


            repoIMItemITBenefits = new Repo<IMItemITBenefits>(Dbcon);
            IMItemITBenefitsMapper = new Mapper<IMItemITBenefits, IMItemITBenefitsInput>();
            entityIMItemITBenefits = new IMItemITBenefits();
            IMItemITBenefitsService = new CrudService<IMItemITBenefits>(repoIMItemITBenefits);


            repoIMItemBusinessBenefits = new Repo<IMItemBusinessBenefits>(Dbcon);
            IMItemBusinessBenefitsMapper = new Mapper<IMItemBusinessBenefits, IMItemBusinessBenefitsInput>();
            entityIMItemBusinessBenefits = new IMItemBusinessBenefits();
            IMItemBusinessBenefitsService = new CrudService<IMItemBusinessBenefits>(repoIMItemBusinessBenefits);

            repoIMItemComments = new Repo<IMItemComments>(Dbcon);
            IMItemCommentsMapper = new Mapper<IMItemComments, IMItemCommentsInput>();
            entityIMItemComments = new IMItemComments();
            IMItemCommentsService = new CrudService<IMItemComments>(repoIMItemComments);


            //repoIMIdeaPriorityHistory = new Repo<IMIdeaPriorityHistory>(Dbcon);
            //IMIdeaPriorityHistoryService = new CrudService<IMIdeaPriorityHistory>(repoIMIdeaPriorityHistory);


            repoIMIdeaStatusHistory = new Repo<IMIdeaStatusHistory>(Dbcon);
            IMIdeaStatusHistoryService = new CrudService<IMIdeaStatusHistory>(repoIMIdeaStatusHistory);

            repoIMIdeaSource = new Repo<IMIdeaSource>(Dbcon);
            IMIdeaSourceService = new CrudService<IMIdeaSource>(repoIMIdeaSource);

            repoIMIdeaType = new Repo<IMIdeaType>(Dbcon);
            IMIdeaTypeService = new CrudService<IMIdeaType>(repoIMIdeaType);

            repoIMLOB = new Repo<IMLOB>(Dbcon);
            IMLOBService = new CrudService<IMLOB>(repoIMLOB);
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


        public ActionResult DisplayIdeaTypes()
        {

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            return View("DisplayIdeaTypes", IMPostIdeaInput);

        }


        public ActionResult DisplayLOB()
        {

            IMLOBInput IMLobInput = new IMLOBInput();
            return View("DisplayLOB", IMLobInput);

        }
        [ActionTrack]
        public ActionResult TrendingReport()
        {

            // IMLOBInput IMLobInput = new IMLOBInput();
            return View("TrendingReport");//, IMLobInput);

        }

        [ActionTrack]
        public ActionResult DisplayIdeaDashboard(int ClientId = 0)
        {
            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            if (Session["SessionClient"] != null)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                IMPostIdeaInput.ClientId = Client.ClientId;
            }
            //if (ClientId > 0)
            //{
            //    IMPostIdeaInput.ClientId = ClientId;
            //    Session["Client"] = ClientId;
            //}
            return View("DisplayDashboard", IMPostIdeaInput);
        }


        public ActionResult DisplayIdeaSources()
        {

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            return View("DisplayIdeaSources", IMPostIdeaInput);

        }


       
        public ActionResult DisplayIdeas(int? page, string sortOrder)
        {
            Db db = new Db();
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            // IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            //if (Session["Client"] != null)
            //{

            //    IMPostIdeaInput.ClientId = client.Id;
            //}
            ViewBag.LastModifiedDate = string.IsNullOrEmpty(sortOrder) ? "LastModifiedDate_desc" : "LastModifiedDate";
            ViewBag.ideas = string.IsNullOrEmpty(sortOrder) ? "ideas_desc" : "ideas";
            ViewBag.Postedby = string.IsNullOrEmpty(sortOrder) ? "Postedby_desc" : "Postedby";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            ViewBag.StatusComments = string.IsNullOrEmpty(sortOrder) ? "StatusComments_desc" : "StatusComments";
            ViewBag.CreatedDate = string.IsNullOrEmpty(sortOrder) ? "CreatedDate_desc" : "CreatedDate";

            List<IMPostIdeaModel> displayideasModel = new List<IMPostIdeaModel>();

            var displayideas = from d in db.IMPostIdea
                               join usrcreated in db.UserProfiles on d.CreatedBy equals usrcreated.Id
                               join usrUpdated in db.UserProfiles on d.UpdatedBy equals usrUpdated.Id into lstusr
                               from usrUpdated in lstusr.DefaultIfEmpty()
                               where d.ClientId == client.ClientId
                               select new IMPostIdeaModel {
                                   //ActualEndDate = d.ActualEndDate,
                                   Id = d.Id,
                                   UpdatedBy = d.UpdatedBy,
                                   //CategoryId = d.CategoryId,
                                   //ActualStartDate = d.ActualStartDate,
                                   ClientId = d.ClientId,
                                   //Complexity = d.Complexity,
                                   //ConfigItem = d.ConfigItem,
                                   CreatedBy = d.CreatedBy,
                                   CreatedByUser =  usrcreated.FirstName + " " + usrcreated.LastName,
                                   CreatedDate = d.CreatedDate,
                                   //Description = d.Description,
                                   //IdeaSourceId = d.IdeaSourceId,
                                   //IdeaTypeId = d.IdeaTypeId,
                                   //InternalInitiative = d.InternalInitiative,
                                   ItemName = d.ItemName,
                                   //ItemTypeId = d.ItemTypeId,
                                   LastUpdatedDate = d.LastUpdatedDate,
                                   LastUpdatedUser = usrUpdated.FirstName + " " + usrUpdated.LastName,
                                   //LOBId=d.LOBId,
                                   //PlannedEndDate=d.PlannedEndDate,
                                   //PlannedStartDate=d.PlannedStartDate,
                                   //PriorityComments=d.PriorityComments,
                                   //PriorityId=d.PriorityId,
                                   //ProgressStatus=d.ProgressStatus,
                                   //QtyFication=d.QtyFication,
                                   //Risk=d.Risk,
                                   //RiskMitigation=d.RiskMitigation,
                                //   StatusComments=d.StatusComments,
                                   StatusId=d.StatusId
                                  
                               };

           


            switch (sortOrder)
            {
                //case "LastModifiedDate_desc":
                //    displayideas = k.OrderByDescending(s => s.PostIdeaInput.LastUpdatedDate).ToPagedList(page ?? 1, 8);
                //    break;
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


            displayideasModel = displayideas.ToList();

          //  IMPostIdeaInput.SortedPostedIdeas = displayideasModel.ToPagedList(page== null?1:page.Value,8);

         
            return View("SetupItems", displayideasModel.ToPagedList(page == null ? 1 : page.Value, 8));
        }




        public ActionResult DisplayITBenefits(int ItemId = 0)
        {
            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            return View("DisplayITBenefits");
        }



        [ActionTrack]
        [HttpPost]
        public JsonResult UpdateStatusHistory(int IdeaId = 0, int SelectedStatusId = 0, string Comments = "")
        {

            bool updateSuccess = false;
            try
            {
                int uid = (int)Session["UserId"];
                IMPostIdea postIdea = service.Get(IdeaId);
             //   int oldStatusId = postIdea.PriorityId;
                postIdea.StatusId = SelectedStatusId;
            //    postIdea.StatusComments = Comments;
                postIdea.LastUpdatedDate = DateTime.Now;
                //postIdea.LastUpdatedUser = User.Identity.Name;
                postIdea.UpdatedBy = uid;
                service.Save();
                //Priroty update history

                IMIdeaStatusHistory IMIdeaStatusHistory = new IMIdeaStatusHistory()
                {
                    IdeaId = IdeaId,
               //     OldStatusId = oldStatusId,
                    NewStatusId = SelectedStatusId,
                    Comments = Comments,
                    IsDeleted = false,
              //      CreatedByUser = GetUserName(uid),
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
        

        [HttpPost]
        public JsonResult CheckCurrentPriority(int IdeaId = 0, int selectedPrioiryId = 0)
        {
            bool resultFound = false;
            IMPostIdea postIdea = service.Get(IdeaId);
            if (postIdea.PriorityId == selectedPrioiryId)
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
            IMPostIdea postIdea = service.Get(IdeaId);
            if (postIdea.PriorityId == SelectedStatusId)
            {
                resultFound = true;
            }
            else
                resultFound = false;

            return Json(resultFound, JsonRequestBehavior.AllowGet);

        }

        [ActionTrack]
        [HttpPost]
        public JsonResult AddIdeaTypeDetails(IMIdeaType tideaType)
        {
            int uid = (int)Session["UserId"];
           // var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
                List<IMIdeaType> lst = IdeaManagerHelper.GetIdeaTypes1(Client.ClientId).Where(c => c.IdeaType.ToLower() == tideaType.IdeaType.ToLower() && c.ClientId == Client.ClientId).ToList();
                if (lst.Count > 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Idea Type Already Exists  <br>" +
                        "Please correct it and try again."
                    });
                }
            }

            //tideaType.CreatedByUser = User.Identity.Name;
           // tideaType.CreatedByUser = GetUserName(uid);
            tideaType.CreatedDate = DateTime.Now;
            tideaType.LastUpdatedDate = DateTime.Now;
            //tideaType.LastUpdatedUser = User.Identity.Name;
           // tideaType.LastUpdatedUser = tideaType.CreatedByUser;
            tideaType.UpdatedBy = uid;
            tideaType.ClientId = Client.ClientId;
            try
            {

                int Id = IMIdeaTypeService.Create(tideaType);
                if (Id > 0)
                {
                    tideaType.Id = Id;
                    return Json(new { Result = "OK", Record = tideaType });
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
        public JsonResult AddIdeaSourceDetails(IMIdeaSource tideasource)
        {
            int uid = (int)Session["UserId"];
           // var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];

            if (ModelState.IsValid)
            {
                IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
                List<IMIdeaSource> lst = IdeaManagerHelper.GetIdeaSources1(Client.ClientId).Where(c => c.IdeaSource.ToLower() == tideasource.IdeaSource.ToLower() && c.ClientId == Client.ClientId).ToList();

                if (lst.Count > 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Idea Source Type Already Exists  <br>" +
                        "Please correct it and try again."
                    });
                }
            }

            //tideasource.CreatedByUser = GetUserName(uid);
            tideasource.CreatedBy = uid;
            tideasource.CreatedDate = DateTime.Now;
            tideasource.LastUpdatedDate = DateTime.Now;
            //tideasource.LastUpdatedUser = tideasource.CreatedByUser;
            tideasource.UpdatedBy = uid;
            tideasource.ClientId = Client.ClientId;
            try
            {

                int Id = IMIdeaSourceService.Create(tideasource);
                if (Id > 0)
                {
                    tideasource.Id = Id;
                    return Json(new { Result = "OK", Record = tideasource });
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
        public JsonResult AddLOBDetails(IMLOB tideasource)
        {
            int uid = (int)Session["UserId"];
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                IMLOBInput IMPostIdeaInput = new IMLOBInput();
                List<IMLOB> lst = IdeaManagerHelper.GetLOBSources1(client.ClientId).Where(c => c.LOB.ToLower() == tideasource.LOB.ToLower()).ToList();
                if (lst.Count > 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "LOB Type Already Exists  <br>" +
                        "Please correct it and try again."
                    });
                }
            }
          //  var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];

            //tideasource.CreatedByUser =uid;
            tideasource.CreatedDate = DateTime.Now;
            tideasource.LastUpdatedDate = DateTime.Now;
            // tideasource.LastUpdatedUser = GetUserName(uid); 
            tideasource.UpdatedBy = uid;
            tideasource.ClientId = Client.ClientId;
            tideasource.IsDeleted = false;
            try
            {

                int Id = IMLOBService.Create(tideasource);
                if (Id > 0)
                {
                    tideasource.Id = Id;
                    return Json(new { Result = "OK", Record = tideasource });
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
        public JsonResult UpdateIdeaSourceDetails(IMIdeaSource tideasource)
        {
            int uid = (int)Session["UserId"];
          //  var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
                List<IMIdeaSource> lst = IdeaManagerHelper.GetIdeaSources1(Client.ClientId).Where(c => c.IdeaSource.ToLower() == tideasource.IdeaSource.ToLower() && c.Id != tideasource.Id && c.ClientId == Client.ClientId).ToList();
                if (lst.Count > 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Idea Source Already Exists  <br>" +
                        "Please correct it and try again."
                    });
                }
            }

            try
            {
                IMIdeaSource imideasource = IMIdeaSourceService.Get(tideasource.Id);
                imideasource.IdeaSource = tideasource.IdeaSource;
                imideasource.LastUpdatedDate = DateTime.Now;
                //imideasource.LastUpdatedUser = User.Identity.Name;
                //imideasource.LastUpdatedUser = GetUserName(uid);
                imideasource.UpdatedBy = uid;
                IMIdeaTypeService.Save();
                return Json(new { Result = "OK", Record = imideasource });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [ActionTrack]
        [HttpPost]
        public JsonResult UpdateLOBDetails(IMLOB tlobsource)
        {
            int uid = (int)Session["UserId"];
            //var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                IMLOBInput IMLobInput = new IMLOBInput();
                List<IMLOB> lst = IdeaManagerHelper.GetLOBSources1(Client.ClientId).Where(c => c.LOB.ToLower() == tlobsource.LOB.ToLower() && c.Id != tlobsource.Id && c.ClientId == Client.ClientId).ToList();
                if (lst.Count > 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "LOB Already Exists  <br>" +
                        "Please correct it and try again."
                    });
                }
            }

            try
            {
                IMLOB imlobsource = IMLOBService.Get(tlobsource.Id);
                imlobsource.LOB = tlobsource.LOB;
                imlobsource.LastUpdatedDate = DateTime.Now;
                //imlobsource.LastUpdatedUser = User.Identity.Name;
                //imlobsource.LastUpdatedUser = GetUserName(uid);
                imlobsource.UpdatedBy = uid;
                IMLOBService.Save();
                return Json(new { Result = "OK", Record = imlobsource });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [ActionTrack]
        [HttpPost]
        public JsonResult UpdateIdeaTypeDetails(IMIdeaType tideaType)
        {
            int uid = (int)Session["UserId"];
         //   var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
                List<IMIdeaType> lst = IdeaManagerHelper.GetIdeaTypes1(Client.ClientId).Where(c => c.IdeaType.ToLower() == tideaType.IdeaType.ToLower() && c.Id != tideaType.Id && c.ClientId == Client.ClientId).ToList();
                if (lst.Count > 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Idea Type Already Exists  <br>" +
                        "Please correct it and try again."
                    });
                }
            }

            try
            {
                IMIdeaType imidea = IMIdeaTypeService.Get(tideaType.Id);
                imidea.IdeaType = tideaType.IdeaType;
                imidea.LastUpdatedDate = DateTime.Now;
                //  imidea.LastUpdatedUser = User.Identity.Name;
                //imidea.LastUpdatedUser = GetUserName(uid);
                imidea.UpdatedBy = uid;

                IMIdeaTypeService.Save();
                return Json(new { Result = "OK", Record = imidea });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [ActionTrack]
        [HttpPost]
        public JsonResult AddITBenefitDetails(IMItemITBenefitsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];

            try
            {
                if(ItemId!=0)
                {

                    //Update tbenefit Details
                   // var Client = (Client)Session["Client"];
                    ClientDetail Client = (ClientDetail)Session["SessionClient"];
                    // tbenefitinput.CreatedByUser = GetUserName(uid);
                    tbenefitinput.CreatedBy = uid;
                    tbenefitinput.CreatedDate = DateTime.Now;
                    tbenefitinput.LastUpdatedDate = DateTime.Now;
                    //tbenefitinput.LastUpdatedUser = User.Identity.Name;
                   // tbenefitinput.LastUpdatedUser = tbenefitinput.CreatedByUser;
                    tbenefitinput.UpdatedBy = uid;

                    int Id = IMItemITBenefitsService.Create(IMItemITBenefitsMapper.MapToEntity(tbenefitinput, entityIMItemITBenefits));

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
        [ActionTrack]
        [HttpPost]
        public JsonResult UpdateITBenefitDetails(IMItemITBenefitsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
            if (!ModelState.IsValid)
            {
                return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
            }
            try
            {
                //IMItemITBenefits imidea = IMItemITBenefitsService.Get(tbenefitinput.Id);
                IMItemITBenefits imidea = new IMItemITBenefits();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    imidea = unitOfWork.IMItemITBenefits.GetByID(tbenefitinput.Id);
                    //imidea.Id = tbenefitinput.Id;
                    //imidea.ItemId = tbenefitinput.ItemId;
                    imidea.Name = tbenefitinput.Name;
                    imidea.FTEHours = tbenefitinput.FTEHours;
                    imidea.Detail = tbenefitinput.Detail;
                    //imidea.CreatedBy = tbenefitinput.CreatedBy;
                    imidea.Value = tbenefitinput.Value;
                    imidea.LastUpdatedDate = DateTime.Now;
                 //   imidea.LastUpdatedUser = GetUserName(uid);
                    imidea.UpdatedBy = uid;
                    unitOfWork.IMItemITBenefits.Update(imidea);
                    unitOfWork.Save();
                }
               
                return Json(new { Result = "OK", Record = imidea });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [ActionTrack]
        [HttpPost]
        public JsonResult DeleteITBenefitDetails(int Id)
        {
            try
            {
              //  var rr = false;
                //IMItemITBenefitsService.Delete(tbenefitinput.Id);
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    unitOfWork.IMItemITBenefits.Delete(Id);
                    unitOfWork.Save();
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }





        [ActionTrack]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddCommentDetails(IMItemCommentsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
            try
            {
                if(ItemId!=0)
                {
                    //Update tbenefit Details

                    //tbenefitinput.CreatedByUser = User.Identity.Name;
                   // tbenefitinput.CreatedByUser = GetUserName(uid);
                    tbenefitinput.CreatedBy = uid;
                    tbenefitinput.CreatedDate = DateTime.Now;
                    tbenefitinput.LastUpdatedDate = DateTime.Now;
                    tbenefitinput.UpdatedBy = uid;
                    //tbenefitinput.LastUpdatedUser = User.Identity.Name;
                   // tbenefitinput.LastUpdatedUser = tbenefitinput.CreatedByUser;
                    int Id = IMItemCommentsService.Create(IMItemCommentsMapper.MapToEntity(tbenefitinput, entityIMItemComments));

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


        public JsonResult GetIdeaTypeDetails()
        {
           // var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            //IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();

            //IList<IMIdeaType> lst = IMPostIdeaInput.GetIdeaTypes;
            // var lst = IdeaManagerHelper.GetIdeaTypes(Client.Id);
            var lst = IdeaManagerHelper.GetIdeaTypes1(Client.ClientId);
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


        public JsonResult GetIdeaSourceDetails()
        {
          //  var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            //IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();

            //IList<IMIdeaSource> lst = IMPostIdeaInput.GetIdeaSources;
            //var lst = IdeaManagerHelper.GetIdeaSources(Client.Id);
            var lst = IdeaManagerHelper.GetIdeaSources1(Client.ClientId);
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

        public JsonResult GetLOBDetails()
        {
           // var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            //IMLOBInput IMLobInput = new IMLOBInput();

            // IList<IMLOB> lst = IMLobInput.GetLOBSources;
            //var lst = IdeaManagerHelper.GetLOBSources();
            var lst = IdeaManagerHelper.GetLOBSources1(Client.ClientId);
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
        public JsonResult GetCommentsDetails(int ItemId = 0)
        {

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            IMPostIdeaInput.Id = ItemId;
            IList<IMItemComments> lst = IMPostIdeaInput.GetItemComments;
            //lst.Add(new IMItemITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
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
        [HttpPost]
        public JsonResult AddBusinessBenefitDetails(IMItemBusinessBenefitsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
            try
            {
                if(ItemId!=0)
                {
                    //Update tbenefit Details

                    //tbenefitinput.CreatedByUser = User.Identity.Name;
                 //   tbenefitinput.CreatedByUser = GetUserName(uid);
                    tbenefitinput.CreatedBy = uid;
                    tbenefitinput.CreatedDate = DateTime.Now;
                    tbenefitinput.LastUpdatedDate = DateTime.Now;
                    tbenefitinput.UpdatedBy = uid;
                    //tbenefitinput.LastUpdatedUser = User.Identity.Name;
                  //  tbenefitinput.LastUpdatedUser = tbenefitinput.CreatedByUser;
                    int Id = IMItemBusinessBenefitsService.Create(IMItemBusinessBenefitsMapper.MapToEntity(tbenefitinput, entityIMItemBusinessBenefits));

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

        [ActionTrack]
        [HttpPost]
        public JsonResult UpdateBusinessBenefitDetails(IMItemBusinessBenefitsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
            try
            {
                //IMItemBusinessBenefits imidea = IMItemBusinessBenefitsService.Get(tbenefitinput.Id);
                IMItemBusinessBenefits imidea = new IMItemBusinessBenefits();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    imidea = unitOfWork.IMItemBusinessBenefits.GetByID(tbenefitinput.Id);
                    //imidea.Id = tbenefitinput.Id;
                    //imidea.ItemId = tbenefitinput.ItemId;
                    imidea.Name = tbenefitinput.Name;
                    imidea.FTEHours = tbenefitinput.FTEHours;
                    imidea.Detail = tbenefitinput.Detail;
                    //imidea.CreatedBy = tbenefitinput.CreatedBy;
                    imidea.Value = tbenefitinput.Value;
                    imidea.LastUpdatedDate = DateTime.Now;
                //    imidea.LastUpdatedUser = GetUserName(uid);
                    imidea.UpdatedBy = uid;
                    unitOfWork.IMItemBusinessBenefits.Update(imidea);
                    unitOfWork.Save();
                }
                //IMItemBusinessBenefitsService.Save();
                return Json(new { Result = "OK", Record = imidea });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
        }
        [ActionTrack]
        [HttpPost]
        public JsonResult DeleteBusinessBenefitDetails(IMItemBusinessBenefitsInput tbenefitinput)
        {
            try
            {
                IMItemBusinessBenefitsService.Delete(tbenefitinput.Id);
                return Json(new { Result = "OK" });
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
            //lst.Add(new IMItemITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
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
        public JsonResult GetITBenefitDetails(int ItemId = 0)
        {

            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            IMPostIdeaInput.Id = ItemId;
            IList<IMItemITBenefits> lst = IMPostIdeaInput.GetITBenefits;
            //lst.Add(new IMItemITBenefits { Id = 1, FTEHours = 25, ItemId = 2, Name = "first", Value = 100 });
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
        

        public ActionResult DisplayHomePage(int ItemId = 0, string SaveData = "", string TabName = "home")
        {
            //var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            IMPostIdeaInput IMPostIdeaInput = new IMPostIdeaInput();
            
                IMPostIdeaInput.ClientId = Client.ClientId;
            
            if (ItemId > 0)
            {
                IMPostIdeaInput.PostIdeaInput = IMPostChallangeMapper.MapToInput(service.Get(ItemId));
               // return RedirectToAction("DisplayHomePage", new { ItemId = ItemId });
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


        ////public ActionResult GetIdeaF()
        //////public ActionResult GetIdeaF(int ClientId = 0)
        ////{
        ////    var Client = (Client)Session["Client"];
        ////    IdeaInput IMPostIdeaInput = new IdeaInput();
        ////    IMPostIdeaInput.ClientId = Client.Id;
        ////    return View("GetIdeaF", IMPostIdeaInput);
        ////}
        //////public ActionResult GetIdeaL(int ClientId = 0)
        ////public ActionResult GetIdeaL()
        ////{
        ////    var Client = (Client)Session["Client"];
        ////    IdeaInput IMPostIdeaInput = new IdeaInput();
        ////    IMPostIdeaInput.ClientId = Client.Id;
        ////    return View("GetIdeaL", IMPostIdeaInput);
        ////}

        //////public ActionResult GetIdeaI(int ClientId = 0)
        ////public ActionResult GetIdeaI()
        ////{
        ////    var Client = (Client)Session["Client"];
        ////    IdeaInput IMPostIdeaInput = new IdeaInput();
        ////    IMPostIdeaInput.ClientId = Client.Id;
        ////    return View("GetIdeaI", IMPostIdeaInput);
        ////}



       


        public ActionResult getClientId()
        {
            //var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            return Json(Client.ClientId);
        }

        [ActionTrack]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SavePostIdea(FormCollection form, IMPostIdeaInput inputs, string command, int ClientId = 0, int ItemId = 0, string PostCreate = "")
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            int uid = (int)Session["UserId"];
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            string TabName = "";
            string SaveData = "";
            if (command == "saveidea")
            {
                TabName = "home";
                PostIdeaInput postchallangeInput = inputs.PostIdeaInput;
                postchallangeInput.ClientId = inputs.ClientId;
                if (inputs.PostIdeaInput.StatusId==0)
                {
                    inputs.PostIdeaInput.StatusId = (int)IMStatus.Created;
                }

                if (!ModelState.IsValid)
                {
                    //if (inputs.PostIdeaInput.PriorityId>0)
                      return View("ItemPostTypePage", inputs);
                }

                //Get Login Id (ItemCreated By Id)
                


                if (ItemId > 0)
                {
                    inputs.PostIdeaInput.Id = ItemId;
                   // inputs.PostIdeaInput.CreatedUserId = postchallangeInput.CreatedUserId;
                    inputs.PostIdeaInput.LastUpdatedDate = DateTime.Now;
                    inputs.PostIdeaInput.LastUpdatedUser = GetUserName(uid);
                    inputs.PostIdeaInput.UpdatedBy = uid;
                   
                    //postchallangeInput.Description = Server.HtmlDecode(postchallangeInput.Description);
                    IMPostChallangeMapper.MapToEntity(inputs.PostIdeaInput, service.Get(ItemId));
                   
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
                   
                    postchallangeInput.StatusId =(int)IMStatus.Created;
                    postchallangeInput.PriorityId = 5;
                    
                    //postchallangeInput.CreatedUserId = uid;
                    postchallangeInput.CreatedDate = DateTime.Now;
                    //postchallangeInput.Description = Server.HtmlDecode(postchallangeInput.Description);
                    ItemId = service.Create(AutoMapper.Mapper.Map<PostIdeaInput, IMPostIdea>(postchallangeInput));


                    IMIdeaPriorityHistory priorityHistory = new IMIdeaPriorityHistory();
                    priorityHistory.OldPriorityId = (int)postchallangeInput.PriorityId;
                    priorityHistory.NewPriorityId = (int)postchallangeInput.PriorityId;
                    priorityHistory.IdeaId = ItemId;
                    priorityHistory.CreatedBy = uid;
                    priorityHistory.UpdatedBy = uid;
                    priorityHistory.LastUpdatedDate = DateTime.Now;
                    priorityHistory.CreatedDate = DateTime.Now;
                    priorityHistory.Comments = "Created(By default)";
                    priorityHistory.IsDeleted = false;
                    unitOfWork.IMIdeaPriorityHistory.Insert(priorityHistory);

                    IMIdeaStatusHistory statusHistory = new IMIdeaStatusHistory();
                    statusHistory.IdeaId = ItemId;
                    statusHistory.OldStatusId = (int)IMStatus.Created;//coz this is also not null
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.NewStatusId = (int)IMStatus.Created;// 1 for Created
                    statusHistory.CreatedBy = uid;
                    statusHistory.Comments = "Created(By default)";//Coz it is Not null;
                    
                        //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        unitOfWork.IMIdeaStatusHistory.Insert(statusHistory);
                        var m = unitOfWork.UserProfile.GetByID(uid);
                        var    email = m.Email;
                        var    fullname = m.FirstName +" "+ m.LastName;
                        unitOfWork.Save();
                        SendIdeaNotification(email,fullname, postchallangeInput.ItemName, (IMStatus)postchallangeInput.StatusId,client.ClientName);
                    
                                        
                }

                //var e = service.Get(id);

                ModelState.AddModelError("Success", "Idea posted successfully");

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
        [ActionTrack]
        public void SaveCategoryDetails(FormCollection form, IMPostIdeaInput inputs, int ClientId, int ItemId)
        {

            int uid = (int)Session["UserId"];
          //  string SaveData = "Save Category";
            IMItemCategoryMapService = new CrudService<IMItemCategoryMap>(repoIMItemCategoryMap);
            if (ItemId > 0)
            {
                IList<IMItemCategoryMap> lst = IMItemCategoryMapService.GetAll().Where(c => c.ItemId == ItemId).ToList();
                foreach (IMItemCategoryMap item in lst)
                {
                    IMItemCategoryMapService.Delete(item.Id);
                }
            }


            string[] selectedcategory = form.GetValues("ideaclassifcation");
            int userId = WebSecurity.GetUserId("amo");
            if (selectedcategory != null)
            {
                foreach (var item in selectedcategory)
                {
                    IMItemCategoryMap mapItem = new IMItemCategoryMap();
                    mapItem.CategoryId = Convert.ToInt32(item);
                    //if (Session["UserId"] == null)
                    //{
                    //    mapItem.CreatedUserId = userId;
                    //}
                    //else
                    //{
                    //    mapItem.CreatedUserId = uid;
                    //}
                    mapItem.ItemId = ItemId;
                  //  mapItem.CreatedByUser = GetUserName(uid);
                    mapItem.CreatedDate = DateTime.Now;
                    mapItem.UpdatedBy = uid;
                    IMItemCategoryMapService.Create(mapItem);

                }
            }
             }


        


        [HttpPost]
        public JsonResult CheckItemName(string ItemName = "", int ItemId = 0)
        {

            int uid = (int)Session["UserId"];
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            DbContextFactory Dbcon = new DbContextFactory();
            Db db = new Db();
            var existItemNames = from IMPostIdea in db.IMPostIdea
                                 where IMPostIdea.ItemName.ToLower() == ItemName.ToLower() && IMPostIdea.CreatedBy == uid && IMPostIdea.ClientId == client.ClientId
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







        [ActionTrack]
        public ActionResult IdeasApproved(int? page, string sortOrder)
        {
            int uid = (int)Session["UserId"];
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ViewBag.ideas = String.IsNullOrEmpty(sortOrder) ? "idea_desc" : "";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            ViewBag.Description = string.IsNullOrEmpty(sortOrder) ? "Description_desc" : "Description";
            ViewBag.Createdby = string.IsNullOrEmpty(sortOrder) ? "Createdby_desc" : "Createdby";
            ViewBag.ideatype = string.IsNullOrEmpty(sortOrder) ? "ideatype_desc" : "ideatype";
            ViewBag.ideasource = string.IsNullOrEmpty(sortOrder) ? "ideasource_desc" : "ideasource";
            ViewBag.LOB = string.IsNullOrEmpty(sortOrder) ? "LOB_desc" : "LOB";
            ViewBag.configitem = string.IsNullOrEmpty(sortOrder) ? "configitem_desc" : "configitem";
            var user = "";
            IList<IdeaModel> x = new List<IdeaModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                user = unitOfWork.UserProfile.Get(g => g.Id == uid).Select(g => g.RoleId).FirstOrDefault();
            }
            if (user.ToLower() == "superuser")
            {
                x = AllPostIdea_SuperUser(client.ClientId);
            }
            else
            {
                x = AllPostIdeas(client.ClientId, uid);
            }

          /*  var x = AllPostIdeas(client.Id, uid);*////.ToPagedList(page ?? 1, 8)/*.OrderByDescending(s => s.IdeaName)*/ ;
            IPagedList<IdeaModel> ideas = null;
            switch (sortOrder)
            {
                case "idea_desc":
                    ideas = x.OrderByDescending(s => s.IdeaName).ToPagedList(page ?? 1, 6);
                    break;
                case "Status_desc":
                    ideas = x.OrderByDescending(s => s.Status).ToPagedList(page ?? 1, 6);
                    break;
                case "Status":
                    ideas = x.OrderBy(s => s.Status).ToPagedList(page ?? 1, 6);
                    ViewBag.Status = "Status_desc";
                    break;
                case "Description_desc":
                    ideas = x.OrderByDescending(s => s.Description).ToPagedList(page ?? 1, 6);
                    break;
                case "Description":
                    ideas = x.OrderBy(s => s.Description).ToPagedList(page ?? 1, 6);
                    ViewBag.Description = "Description_desc";
                    break;
                case "Createdby_desc":
                    ideas = x.OrderByDescending(s => s.CreatedByUser).ToPagedList(page ?? 1, 6);
                    break;
                case "Createdby":
                    ideas = x.OrderBy(s => s.CreatedByUser).ToPagedList(page ?? 1, 6);
                    ViewBag.Createdby = "Createdby_desc";
                    break;
                case "ideatype_desc":
                    ideas = x.OrderByDescending(s => s.IdeaType).ToPagedList(page ?? 1, 6);
                    break;
                case "ideatype":
                    ideas = x.OrderBy(s => s.IdeaType).ToPagedList(page ?? 1, 6);
                    ViewBag.ideatype = "ideatype_desc";
                    break;
                case "ideasource_desc":
                    ideas = x.OrderByDescending(s => s.IdeaSource ).ToPagedList(page ?? 1, 6);
                    break;
                case "ideasource":
                    ideas = x.OrderBy(s => s.IdeaSource).ToPagedList(page ?? 1, 6);
                    ViewBag.ideasource = "ideasource_desc";
                    break;
                case "LOB_desc":
                    ideas = x.OrderByDescending(s => s.LOB).ToPagedList(page ?? 1, 6);
                    break;
                case "LOB":
                    ideas = x.OrderBy(s => s.LOB).ToPagedList(page ?? 1, 6);
                    ViewBag.LOB = "LOB_desc";
                    break;
                case "configitem_desc":
                    ideas = x.OrderByDescending(s => s.ConfigItemName).ToPagedList(page ?? 1, 6);
                    break;
                case "configitem":
                    ideas = x.OrderBy(s => s.ConfigItemName).ToPagedList(page ?? 1, 6);
                    ViewBag.configitem = "configitem_desc";
                    break;

                default:
                   ideas = x.OrderBy(s => s.IdeaName).ToPagedList(page ?? 1, 6);
                    break;
            }
            return View("IdeasApproved", ideas);
        }


        //[HttpPost]
        //public ActionResult Selection_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    int uid = (int)Session["UserId"];
        //    var client = (Client)Session["Client"];
        //    var x = AllPostIdeas(client.Id, uid);        

        //    return Json(x.ToDataSourceResult(request));
        //}


        [HttpPost]
        public ActionResult Selection_Read([DataSourceRequest] DataSourceRequest request)
        {
            int uid = (int)Session["UserId"];
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var user = "";
            IList<IdeaModel> xcac = new List<IdeaModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                user = unitOfWork.UserProfile.Get(g => g.Id == uid).Select(g => g.RoleId).FirstOrDefault();
            }
            if (user.ToLower() == "superuser")
            {
                xcac = AllPostIdea_SuperUser(client.ClientId);
            }
            else
            {
                xcac = AllPostIdeas(client.ClientId, uid);
            }


            return Json(xcac.ToDataSourceResult(request));
        }


        public static IList<IdeaModel> AllPostIdea_SuperUser(int clientId)
        {

            Db db = new Db();
            var ideamod = (from posts in db.IMPostIdea
                           join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                          // join IA in db.IMIdeaApprover on posts.Id equals IA.IdeaId
                           join idt in db.IMIdeaType on posts.IdeaTypeId equals idt.Id into tmpidt
                           //join ids in db.IMIdeaSource on posts.IdeaSourceId equals ids.Id into tmpids
                           join idst in db.IMItemStatus on posts.StatusId equals idst.Id into tmpidst
                          // join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                           join lob in db.IMLOBSource on posts.LOBId equals lob.Id into tmplob
                           from status in tmpidst.DefaultIfEmpty()
                           //from source in tmpids.DefaultIfEmpty()
                           from idtype in tmpidt.DefaultIfEmpty()
                         //  from apps in tmpapp.DefaultIfEmpty()
                           from lobs in tmplob.DefaultIfEmpty()
                           where posts.ClientId == clientId && posts.StatusId != (int)IMStatus.Implemented
                           select new IdeaModel
                           {
                              // CategoryId = posts.CategoryId,
                               ClientId = posts.ClientId,
                               //ConfigItem = posts.ConfigItem,
                               //ConfigItemName = apps.ApplicationName,
                               CreatedByUser = usr.FirstName + " " + usr.LastName,
                               CreatedBy = posts.CreatedBy,
                               CreatedDate = posts.CreatedDate,
                               Description = posts.Description,
                               IdeaId = posts.Id,
                               IdeaName = posts.ItemName,
                              // IdeaSource = source.IdeaSource,
                               //IdeaSourceId = posts.IdeaSourceId,
                               IdeaType = idtype.IdeaType,
                             //  IdeaTypeId = posts.IdeaTypeId,
                               IsDeleted = posts.IsDeleted,
                               //  ItemType= posts.id
                               LOB = lobs.LOB,
                               LOBId = posts.LOBId,
                             //  PriorityComments = posts.PriorityComments,
                             //  StatusComments = posts.StatusComments,
                             //  PriorityId = posts.PriorityId,
                               QtyFication = posts.QtyFication,
                               Status = status.Status,
                               StatusId = posts.StatusId
                           }
                          ).ToList();
            return ideamod;

        }

        [ActionTrack]
        public static IList<IdeaModel> AllPostIdeas(int clientId, int uid)
        {
            
            Db db = new Db();
            var ideamod = (from posts in db.IMPostIdea
                          join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                          join IA in db.IMIdeaApprover on posts.Id equals IA.IdeaId
                          join idt in db.IMIdeaType on posts.IdeaTypeId equals idt.Id into tmpidt
                          //join ids in db.IMIdeaSource on posts.IdeaSourceId equals ids.Id into tmpids
                          join idst in db.IMItemStatus on posts.StatusId equals idst.Id into tmpidst
                       //   join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                          join lob in db.IMLOBSource on posts.LOBId equals lob.Id into tmplob
                          from status in tmpidst.DefaultIfEmpty()
                          //from source in tmpids.DefaultIfEmpty()
                          from idtype in tmpidt.DefaultIfEmpty()
                        //  from apps in tmpapp.DefaultIfEmpty()
                          from lobs in tmplob.DefaultIfEmpty()
                          where posts.ClientId == clientId && IA.ApproverId==uid && posts.StatusId != (int)IMStatus.Implemented
                          select new IdeaModel
                          {
                             // CategoryId = posts.CategoryId,
                              ClientId = posts.ClientId,
                             // ConfigItem = posts.ConfigItem,
                           //   ConfigItemName = apps.ApplicationName,
                              CreatedByUser = usr.FirstName + " " + usr.LastName,
                              CreatedBy = posts.CreatedBy,
                              CreatedDate = posts.CreatedDate,
                              Description = posts.Description,
                              IdeaId = posts.Id,
                              IdeaName = posts.ItemName,
                              //IdeaSource = source.IdeaSource,
                              //IdeaSourceId = posts.IdeaSourceId,
                              IdeaType = idtype.IdeaType,
                           //   IdeaTypeId = posts.IdeaTypeId,
                              IsDeleted = posts.IsDeleted,
                              //  ItemType= posts.id
                              LOB = lobs.LOB,
                              LOBId = posts.LOBId,
                           //   PriorityComments = posts.PriorityComments,
                           //   StatusComments=posts.StatusComments,
                           //   PriorityId =posts.PriorityId,
                              QtyFication = posts.QtyFication,
                              Status = status.Status,
                              StatusId = posts.StatusId
                          }
                          ).ToList();
            return ideamod;

        }


        public ActionResult testingUpdate(IList<IdeaModel> inputs, string btnSubmit)
        {
            int uid = (int)Session["UserId"];
            //string[] ;
            ModelState.Clear();
            bool checkboxchecked = false;
            foreach (var c in inputs)
            {
                if (c.IsChecked)
                {
                    checkboxchecked = true;
                }
                if (c.IsChecked == true && (string.IsNullOrEmpty(c.StatusComments)  || c.PriorityId == 0))
                {

                    ModelState.AddModelError(c.IdeaId + "mandate", c.IdeaName + "- Priority and Comments are mandatory.");
                    checkboxchecked = true;
                }
            }
            if(!checkboxchecked)
            {
                ModelState.AddModelError("chkerr", "Please select any one of the ideas");
            }
            if (!ModelState.IsValid)
            {
                return View("IdeasApproved", inputs.ToPagedList(1, 8));
            }
            if (ModelState.IsValid)
            {
                foreach (var c in inputs)
                {
                    if (btnSubmit == "Approve")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)IMStatus.Created || c.StatusId==(int)IMStatus.Posted || c.StatusId == (int)IMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.IdeaId, IMStatus.Approved, c.StatusComments, c.PriorityId);
                            ApprovedDateUpdate(c.IdeaId, uid);
                        }
                    }
                    if (btnSubmit == "Submit")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)IMStatus.Created || c.StatusId == (int)IMStatus.Posted || c.StatusId == (int)IMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.IdeaId, IMStatus.Submitted, c.StatusComments, c.PriorityId);
                        }
                    }
                    if (btnSubmit == "Decline")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)IMStatus.Created || c.StatusId == (int)IMStatus.Posted || c.StatusId == (int)IMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.IdeaId, IMStatus.Declined , c.StatusComments, c.PriorityId);
                        }
                    }
                    if (btnSubmit == "Close")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)IMStatus.Created || c.StatusId == (int)IMStatus.Posted || c.StatusId == (int)IMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.IdeaId, IMStatus.Cancelled, c.StatusComments, c.PriorityId);
                        }
                    }
                }
                TempData.Add("ValidationSuccessMsg", "Status got updated successfully.");
            }

            //return View("IdeasApproved", inputs);

            return RedirectToAction("IdeasApproved");
        }
        public static void ApprovedDateUpdate(int IdeaId, int uid)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var getRow = unitOfWork.IMIdeaApprover.Get(g => g.IdeaId == IdeaId && g.ApproverId == uid).FirstOrDefault();
                if (getRow != null)
                {
                    getRow.ApprovedDate = DateTime.Now;
                    unitOfWork.IMIdeaApprover.Update(getRow);
                    unitOfWork.Save();
                }
            }
            catch (Exception)
            {

            }
        }
        [ActionTrack]
        public void updatestatus1(int ideaid, IMStatus status, string Comments, int priority)
        {
            int uid = (int)Session["UserId"];
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                {
                    var idea = unitOfWork.IMPostIdea.GetByID(ideaid);
                    if (idea != null)
                    {
                        var oldStatus = idea.StatusId;
                        idea.StatusId = (int)status; //2;// 2 for Submitted; 1 for created
                      //  idea.StatusComments = Comments;
                        idea.PriorityId = priority;
                        unitOfWork.IMPostIdea.Update(idea);


                        var oldPrioritObject = unitOfWork.IMIdeaPriorityHistory.Get().Where(w => w.IdeaId == ideaid).OrderByDescending(o => o.Id).FirstOrDefault();
                        int oldpId = 0;
                        if (oldPrioritObject != null)
                        {
                         //   oldpId = oldPrioritObject.NewPriorityId;
                        }
                        IMIdeaPriorityHistory priorityHistory = new IMIdeaPriorityHistory();
                        priorityHistory.OldPriorityId = oldpId;

                        priorityHistory.IdeaId = ideaid;
                        priorityHistory.CreatedBy = uid;
                        priorityHistory.CreatedDate = DateTime.Now;
                        priorityHistory.NewPriorityId = priority;
                        priorityHistory.UpdatedBy = uid;
                        priorityHistory.LastUpdatedDate = DateTime.Now;
                        priorityHistory.Comments = Comments==null?"":Comments;
                        priorityHistory.IsDeleted = false;
                        unitOfWork.IMIdeaPriorityHistory.Insert(priorityHistory);
                        //priorityHistory.NewPriorityId = priority;
                        //priorityHistory.IdeaId = ideaid;
                        //priorityHistory.CreatedBy = uid;
                        //priorityHistory.UpdatedBy = uid;
                        //priorityHistory.LastUpdatedDate = DateTime.Now;
                        //priorityHistory.CreatedDate = DateTime.Now;
                        //unitOfWork.IMIdeaPriorityHistory.Insert(priorityHistory);

                        IMIdeaStatusHistory statusHistory = new IMIdeaStatusHistory();
                        statusHistory.IdeaId = idea.Id;
                        //statusHistory.Comments = idea.StatusComments==null?"":idea.StatusComments;
                        statusHistory.LastUpdatedDate = DateTime.Now;
                        statusHistory.UpdatedBy = uid;
                        statusHistory.OldStatusId = oldStatus;
                        statusHistory.NewStatusId = idea.StatusId;
                        unitOfWork.IMIdeaStatusHistory.Insert(statusHistory);
                        //unitOfWork.Save();
                        //Db db = new Db();
                        //var listOfAdmins = (from UA in db.UserApplications
                        //                    join UP in db.UserProfiles on UA.UserId equals UP.Id
                        //                    join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                        //                    join UR in db.UserRoles on UA.RoleId equals UR.Id
                        //                    where ASA.ApplicationName.ToLower() == "idea manager" && UR.RoleName == "admin" && UA.ClientId == client.Id
                        //                    select UP.Id
                        //                    ).Distinct().ToList();
                        var listOfAdmins = unitOfWork.IMIdeaApprover.Get(g => g.IdeaId == ideaid).Select(g=>g.ApproverId).ToList();
                        foreach (var Adid in listOfAdmins)
                        {
                            var m1 = unitOfWork.UserProfile.GetByID(idea.CreatedBy);
                            var email1 = m1.Email;
                            var fullname1 = m1.FirstName + " " + m1.LastName;
                            SendIdeaNotification(email1, fullname1, idea.ItemName, (IMStatus)idea.StatusId, client.ClientName);
                        }
                        var m = unitOfWork.UserProfile.GetByID(idea.CreatedBy);
                        var email = m.Email;
                        var fullname = m.FirstName + " " + m.LastName;
                        SendIdeaNotification(email, fullname, idea.ItemName, (IMStatus)idea.StatusId, client.ClientName);

                    }
                }
                unitOfWork.Save();
            }


        }
        ////[ActionTrack]
        //////public JsonResult ListOfAdmins()
        ////[HttpPost]
        ////public ActionResult ListOfAdmins([DataSourceRequest] DataSourceRequest request)
        ////{
        ////    Client client = (Client)Session["Client"];
        ////    Db db = new Db();
        ////    var listOfAdmins = (from UA in db.UserApplications
        ////                        join UP in db.UserProfiles on UA.UserId equals UP.Id
        ////                        join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
        ////                        join UR in db.UserRoles on UA.RoleId equals UR.Id
        ////                        where ASA.ApplicationName.ToLower() == "idea manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin") && UA.ClientId == client.Id
        ////                        select UP
        ////                                    ).Distinct().ToList();
        ////    var AdminsWithName = listOfAdmins.Select(g =>
        ////        new SelectListItem
        ////        {
        ////            Text = g.FirstName + " " + g.LastName,
        ////            Value = g.Id.ToString()
        ////        }
        ////        );
        ////    //return Json(new { Result = "OK", Records = AdminsWithName });
        ////    return Json(AdminsWithName.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        ////}

        public ActionResult ListOfAdmins([DataSourceRequest] DataSourceRequest request, int ideaId)
        { 
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var TEMPlistOfAdmins = (from UA in db.UserApplications
                                join UP in db.UserProfiles on UA.UserId equals UP.Id
                                join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                join UR in db.UserRoles on UA.RoleId equals UR.Id
                                join APPRS in db.IMIdeaApprover.Where(w=>w.IdeaId==ideaId) on UP.Id equals APPRS.ApproverId into tmpApprs
                                from aprs in tmpApprs.DefaultIfEmpty()
                                where ASA.ApplicationName.ToLower() == "idea manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin") && UA.ClientId == client.ClientId
                                select new ApproverModel {
                                    Id =UP.Id,
                                    FirstName = UP.FirstName,
                                    LastName=UP.LastName,
                                    IdeaId=ideaId,
                                    IdeaApproverId = aprs.Id,
                                    ApprovedDate = aprs.ApprovedDate
                                }
                               ).Distinct().OrderBy(x=>x.FirstName).ToList();

            //Adding Status infront it ...
            //Logic is => IF (__ApprovedDt__ is NULL) then Admin has not Approved(might have cancelled/Hold/Decline)
            //Else  (i.e, __ApprovedDt__ is NOT NULl) Admin has Approved the IDEA
            //Also checking IdeaApproverId property in above Object -> to check whether they are selected as aprrover or not
            var listOfAdmins = TEMPlistOfAdmins.Select(f =>
                    new ApproverModel
                    {
                        Id = f.Id,
                        FirstName = f.FirstName,
                        LastName = f.LastName,
                        IdeaId = f.IdeaId,
                        IdeaApproverId = f.IdeaApproverId,
                        ApprovedDate = f.ApprovedDate,
                        ApprovedOrNot = (f.ApprovedDate != null || f.ApprovedDate > DateTime.MinValue) ? "Approved" : (f.IdeaApproverId == null ? "":"Not Approved")

                    }
            );

            return Json(listOfAdmins.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult AddSelectedAdmins(int ItemId =0, string IdeaIds = "")
        {
           // Client client = (Client)Session["Client"];
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

                        var ideaname = unitOfWork.IMPostIdea.Get(g => g.Id == ItemId).FirstOrDefault();
                        ideaname.StatusId = (int)IMStatus.Posted;
                        ideaname.LastUpdatedDate = DateTime.Now;
                        ideaname.UpdatedBy = uid;
                        //ideaname.LastUpdatedUser = GetUserName(uid);
                        unitOfWork.IMPostIdea.Update(ideaname);
                        
                        foreach (var item in SelectedAdminIds)
                        {
                            if(item!=" "||item!=","||item!="")
                            {
                                IMIdeaApprover ideaApprover = new IMIdeaApprover();
                                ideaApprover.IdeaId = Convert.ToInt32(ItemId);

                                ideaApprover.ApproverId = Convert.ToInt32(item);
                                unitOfWork.IMIdeaApprover.Insert(ideaApprover);

                                
                                var u = unitOfWork.UserProfile.Get(g => g.Id == ideaApprover.ApproverId).FirstOrDefault();
                                SendIdeaNotification(u.Email, u.FirstName + " " + u.LastName, ideaname.ItemName, IMStatus.Posted, client.ClientName);

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
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int Clientid = client.ClientId;

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                {
                    //var idea = unitOfWork.IMPostIdea.Get(e=>e.ItemName== ItemName).FirstOrDefault();
                    var idea = unitOfWork.IMPostIdea.GetByID(idid);
                    if (idea != null)
                    {
                        var oldStatus = idea.StatusId;
                        idea.StatusId = (int)IMStatus.Implemented;

                        unitOfWork.IMPostIdea.Update(idea);


                        IMIdeaStatusHistory statusHistory = new IMIdeaStatusHistory();
                        statusHistory.IdeaId = idea.Id;
                     //   statusHistory.Comments = idea.StatusComments;
                        statusHistory.LastUpdatedDate = DateTime.Now;
                        statusHistory.UpdatedBy = uid;
                        statusHistory.OldStatusId = oldStatus;
                        statusHistory.NewStatusId = idea.StatusId;

                        unitOfWork.IMIdeaStatusHistory.Insert(statusHistory);
                        var m = unitOfWork.UserProfile.GetByID(idea.CreatedBy);
                        var email = m.Email;
                        var fullname = m.FirstName + " " + m.LastName;
                        SendIdeaNotification(email,fullname, idea.ItemName, (IMStatus)idea.StatusId, client.ClientName);
                    }
                }
                unitOfWork.Save();
            }
            return RedirectToAction("DisplayIdeas");
        }








        public void SendIdeaNotification(string email,string fullname,string IdeaName, IMStatus status, string clientname)
        {
            try
            {
                var body = "";

                var mailId = email;
                var Subject = "Optimization Tool IM AutoNotification";
                if (status == IMStatus.Created)
                {
                    body = ThankyouBody(fullname, clientname);
                }
                else if(status == IMStatus.Posted)
                {
                    //fulllname = Admin Name(Approver)
                    body = IdeaApproverBody(fullname, IdeaName, status.ToString(), clientname);
                }
                else
                {
                    body = StatusBody(fullname, IdeaName, status.ToString(), clientname);
                }

                App_Start.Helper.SendEMail(mailId, Subject, body);
            }
            catch
            {

            }
            
           // return;
        }
        private string ThankyouBody(string username, string clientname)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/ThankYouEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{clientname}", clientname);

            return body;
        }
        [ActionTrack]
        private string IdeaApproverBody(string adminName, string Idea, string status, string clientname)
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
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/IdeaApproverEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{ideaname}", Idea);
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{Adminname}", adminName);

            return body;
        }


        private string StatusBody(string username, string Idea, string status, string clientname)
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
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/IdeaStatusEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{ideaname}", Idea);
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{Adminname}", adminName);

            return body;
        }

    }
}
    
