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
using WebUI.Areas.DemandManager.Dto;
using WebUI.Areas.DemandManager.Object;
using Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;
using System.Data.Entity;
using WebUI.Dto;

namespace WebUI.Areas.DemandManager.Controllers
{
    public class demandManagerController : Cruder<DMPostDemand, DMPostDemandInput>
    {

        //Entities
        private DMPostDemand entityDMPostDemand;
        private DMItemCategoryMap entityDMItemCategoryMap;
        private DMItemITBenefits entityDMItemITBenefits;
        private DMItemBusinessBenefits entityDMItemBusinessBenefits;
        private DMItemComments entityDMItemComments;
        //private DMPriorityHistory entityDMPriorityHistory;
        //private DMStatusHistory entityDMStatusHistory;
       // //private DMDemandSource entityDMDemandSource;
       // private DMDemandType entityDMDemandType;



        //DMItemBusinessBenefits

        //Service

        //private CrudService<DMPostDemand> DMPostDemandService;
        private CrudService<DMItemCategoryMap> DMItemCategoryMapService;
        private CrudService<DMItemITBenefits> DMItemITBenefitsService;
        private CrudService<DMItemBusinessBenefits> DMItemBusinessBenefitsService;
        private CrudService<DMItemComments> DMItemCommentsService;
        //private CrudService<DMPriorityHistory> DMPriorityHistoryService;
        private CrudService<DMStatusHistory> DMDemandStatusHistoryService;
        //private CrudService<DMDemandSource> DMDemandSourceService;
        private CrudService<DMDemandType> DMDemandTypeService;
        private CrudService<DMLOB> DMLOBService;


        //Mappers

        private Mapper<DMPostDemand, DMPostDemandInput> DMPostDemandMapper;
        private Mapper<DMPostDemand, PostDemandInput> DMPostChallangeMapper;
        private Mapper<DMItemCategoryMap, DMItemCategoryMapInput> DMItemCategoryMapMapper;
        private Mapper<DMItemITBenefits, DMItemITBenefitsInput> DMItemITBenefitsMapper;
        private Mapper<DMItemBusinessBenefits, DMItemBusinessBenefitsInput> DMItemBusinessBenefitsMapper;
        private Mapper<DMItemComments, DMItemCommentsInput> DMItemCommentsMapper;


        //Repo

        private Repo<DMPostDemand> repoDMPostDemand;
        private Repo<DMItemCategoryMap> repoDMItemCategoryMap;
        private Repo<DMItemITBenefits> repoDMItemITBenefits;
        private Repo<DMItemBusinessBenefits> repoDMItemBusinessBenefits;
        private Repo<DMItemComments> repoDMItemComments;
        // private Repo<DMPriorityHistory> repoDMDemandPriorityHistory;
        private Repo<DMStatusHistory> repoDMDemandStatusHistory;
        private Repo<DMDemandType> repoDMDemandType;
        //private Repo<DMDemandSource> repoDMDemandSource;
        private Repo<DMLOB> repoDMLOB;


        public demandManagerController(ICrudService<DMPostDemand> service, IMapper<DMPostDemand, DMPostDemandInput> v)
            : base(service, v)
        {


            DbContextFactory Dbcon = new DbContextFactory();
            repoDMPostDemand = new Repo<DMPostDemand>(Dbcon);
            DMPostDemandMapper = new Mapper<DMPostDemand, DMPostDemandInput>();
            DMPostChallangeMapper = new Mapper<DMPostDemand, PostDemandInput>();
            entityDMPostDemand = new DMPostDemand();

            repoDMItemCategoryMap = new Repo<DMItemCategoryMap>(Dbcon);
            DMItemCategoryMapMapper = new Mapper<DMItemCategoryMap, DMItemCategoryMapInput>();
            entityDMItemCategoryMap = new DMItemCategoryMap();


            repoDMItemITBenefits = new Repo<DMItemITBenefits>(Dbcon);
            DMItemITBenefitsMapper = new Mapper<DMItemITBenefits, DMItemITBenefitsInput>();
            entityDMItemITBenefits = new DMItemITBenefits();
            DMItemITBenefitsService = new CrudService<DMItemITBenefits>(repoDMItemITBenefits);


            repoDMItemBusinessBenefits = new Repo<DMItemBusinessBenefits>(Dbcon);
            DMItemBusinessBenefitsMapper = new Mapper<DMItemBusinessBenefits, DMItemBusinessBenefitsInput>();
            entityDMItemBusinessBenefits = new DMItemBusinessBenefits();
            DMItemBusinessBenefitsService = new CrudService<DMItemBusinessBenefits>(repoDMItemBusinessBenefits);

            repoDMItemComments = new Repo<DMItemComments>(Dbcon);
            DMItemCommentsMapper = new Mapper<DMItemComments, DMItemCommentsInput>();
            entityDMItemComments = new DMItemComments();
            DMItemCommentsService = new CrudService<DMItemComments>(repoDMItemComments);

            repoDMDemandStatusHistory = new Repo<DMStatusHistory>(Dbcon);
            DMDemandStatusHistoryService = new CrudService<DMStatusHistory>(repoDMDemandStatusHistory);

            //repoDMDemandSource = new Repo<DMDemandSource>(Dbcon);
            //DMDemandSourceService = new CrudService<DMDemandSource>(repoDMDemandSource);

            repoDMDemandType = new Repo<DMDemandType>(Dbcon);
            DMDemandTypeService = new CrudService<DMDemandType>(repoDMDemandType);

            repoDMLOB = new Repo<DMLOB>(Dbcon);
            DMLOBService = new CrudService<DMLOB>(repoDMLOB);
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


        public ActionResult DisplayDemandTypes()
        {

            DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
            return View("DisplayDemandTypes", DMPostDemandInput);

        }


        public ActionResult DisplayLOB()
        {

            DMLOBInput DMLobInput = new DMLOBInput();
            return View("DisplayLOB", DMLobInput);

        }
        [ActionTrack]
        public ActionResult TrendingReport()
        {
            return View("TrendingReport");//, DMLobInput);

        }

        [ActionTrack]
        public ActionResult DisplayDemandDashboard(int ClientId = 0)
        {
            DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
            if (Session["SessionClient"] != null)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                DMPostDemandInput.ClientId = Client.ClientId;
            }
            return View("DisplayDashboard", DMPostDemandInput);
        }


        public ActionResult DisplayDemandSources()
        {

            DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
            return View("DisplayDemandSources", DMPostDemandInput);

        }


       
        public ActionResult DisplayDemands(int? page, string sortOrder)
        {
            Db db = new Db();
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            ViewBag.LastModifiedDate = string.IsNullOrEmpty(sortOrder) ? "LastModifiedDate_desc" : "LastModifiedDate";
            ViewBag.Demands = string.IsNullOrEmpty(sortOrder) ? "Demands_desc" : "Demands";
            ViewBag.Postedby = string.IsNullOrEmpty(sortOrder) ? "Postedby_desc" : "Postedby";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            ViewBag.StatusComments = string.IsNullOrEmpty(sortOrder) ? "StatusComments_desc" : "StatusComments";
            ViewBag.CreatedDate = string.IsNullOrEmpty(sortOrder) ? "CreatedDate_desc" : "CreatedDate";

            List<DMPostDemandModel> displayDemandModel = new List<DMPostDemandModel>();

            var displayDemands = from d in db.DMPostDemand
                               join usrcreated in db.UserProfiles on d.CreatedBy equals usrcreated.Id
                               join usrUpdated in db.UserProfiles on d.UpdatedBy equals usrUpdated.Id into lstusr
                               from usrUpdated in lstusr.DefaultIfEmpty()
                               where d.ClientId == client.ClientId
                               select new DMPostDemandModel
                               {
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
                                   //InternalInitiative = d.InternalInitiative,
                                   ItemName = d.ItemName,
                                   //ItemTypeId = d.ItemTypeId,
                                   LastUpdatedDate = d.LastUpdatedDate,
                                   LastUpdatedUser = usrUpdated.FirstName + " " + usrUpdated.LastName,
                                
                                   StatusId=d.StatusId
                                  
                               };

            switch (sortOrder)
            {
                case "LastModifiedDate_desc":
                    displayDemands = displayDemands.OrderByDescending(d => d.LastUpdatedDate);
                    break;
                case "LastModifiedDate":
                    displayDemands = displayDemands.OrderBy(d => d.LastUpdatedDate);
                    ViewBag.LastModifiedDate = "LastModifiedDate_desc";
                    break;

                case "Demands_desc":
                    displayDemands = displayDemands.OrderByDescending(d => d.ItemName);
                    break;
                case "Demands":
                    displayDemands = displayDemands.OrderBy(d => d.ItemName);
                    ViewBag.Demands = "Demands_desc";
                    break;

                case "Postedby_desc":
                    displayDemands = displayDemands.OrderByDescending(d => d.CreatedBy);
                    break;
                case "Postedby":
                    displayDemands = displayDemands.OrderBy(d => d.CreatedBy);
                    ViewBag.Postedby = "Postedby_desc";
                    break;

                case "Status_desc":
                    displayDemands = displayDemands.OrderByDescending(d => d.StatusId);
                    break;
                case "Status":
                    displayDemands = displayDemands.OrderBy(d => d.StatusId);
                    ViewBag.Status = "Status_desc";
                    break;

                case "StatusComments_desc":
                    displayDemands = displayDemands.OrderByDescending(d => d.StatusComments);
                    break;
                case "StatusComments":
                    displayDemands = displayDemands.OrderBy(d => d.StatusComments);
                    ViewBag.StatusComments = "StatusComments_desc";
                    break;

                case "CreatedDate_desc":
                    displayDemands = displayDemands.OrderByDescending(d => d.CreatedDate);
                    break;
                case "CreatedDate":
                    displayDemands = displayDemands.OrderBy(d => d.CreatedDate);
                    ViewBag.CreatedDate = "CreatedDate_desc";
                    break;

                default:
                    displayDemands = displayDemands.OrderBy(d => d.ItemName);
                    break;
            }


            displayDemandModel = displayDemands.ToList();
         
            return View("SetupItems", displayDemandModel.ToPagedList(page == null ? 1 : page.Value, 8));
        }




        public ActionResult DisplayITBenefits(int ItemId = 0)
        {
            DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
            return View("DisplayITBenefits");
        }



        [ActionTrack]
        [HttpPost]
        public JsonResult UpdateStatusHistory(int DemandId = 0, int SelectedStatusId = 0, string Comments = "")
        {

            bool updateSuccess = false;
            try
            {
                int uid = (int)Session["UserId"];
                DMPostDemand postDemand = service.Get(DemandId);
                postDemand.StatusId = SelectedStatusId;
                postDemand.LastUpdatedDate = DateTime.Now;
                postDemand.UpdatedBy = uid;
                service.Save();
                //Priroty update history

                DMStatusHistory DMStatusHistory = new DMStatusHistory()
                {
                    DemandId = DemandId,
               //     OldStatusId = oldStatusId,
                    NewStatusId = SelectedStatusId,
                    Comments = Comments,
                    IsDeleted = false,
              //      CreatedByUser = GetUserName(uid),
                    CreatedDate = DateTime.Now
                };
                var createdId = DMDemandStatusHistoryService.Create(DMStatusHistory);

                updateSuccess = true;
            }
            finally
            {
                updateSuccess = false;
            }

            return Json(updateSuccess, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult CheckCurrentPriority(int DemandId = 0, int selectedPrioiryId = 0)
        {
            bool resultFound = false;
            DMPostDemand postDemand = service.Get(DemandId);
            if (postDemand.PriorityId == selectedPrioiryId)
            {
                resultFound = true;
            }
            else
                resultFound = false;

            return Json(resultFound, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckCurrentStatus(int DemandId = 0, int SelectedStatusId = 0)
        {
            bool resultFound = false;
            DMPostDemand postDemand = service.Get(DemandId);
            if (postDemand.PriorityId == SelectedStatusId)
            {
                resultFound = true;
            }
            else
                resultFound = false;

            return Json(resultFound, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddDemandTypeDetails(DMDemandType tDemandType)
        {
            int uid = (int)Session["UserId"];
           // var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
                List<DMDemandType> lst = DemandManagerHelper.GetDemandTypes1(Client.ClientId).Where(c => c.DemandType.ToLower() == tDemandType.DemandType.ToLower() && c.ClientId == Client.ClientId).ToList();
                if (lst.Count > 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Demand Type Already Exists  <br>" +
                        "Please correct it and try again."
                    });
                }
            }

            tDemandType.CreatedDate = DateTime.Now;
            tDemandType.LastUpdatedDate = DateTime.Now;
            tDemandType.UpdatedBy = uid;
            tDemandType.ClientId = Client.ClientId;
            try
            {

                int Id = DMDemandTypeService.Create(tDemandType);
                if (Id > 0)
                {
                    tDemandType.Id = Id;
                    return Json(new { Result = "OK", Record = tDemandType });
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
        //public JsonResult AddDemandSourceDetails(DMDemandSource tDemandsource)
        //{
        //    int uid = (int)Session["UserId"];
        //   // var Client = (Client)Session["Client"];
        //    ClientDetail Client = (ClientDetail)Session["SessionClient"];

        //    if (ModelState.IsValid)
        //    {
        //        DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
        //        List<DMDemandSource> lst = DemandManagerHelper.GetDemandSources1(Client.ClientId).Where(c => c.DemandSource.ToLower() == tDemandsource.DemandSource.ToLower() && c.ClientId == Client.ClientId).ToList();

        //        if (lst.Count > 0)
        //        {
        //            return Json(new
        //            {
        //                Result = "ERROR",
        //                Message = "Demand Source Type Already Exists  <br>" +
        //                "Please correct it and try again."
        //            });
        //        }
        //    }

        //    tDemandsource.CreatedBy = uid;
        //    tDemandsource.CreatedDate = DateTime.Now;
        //    tDemandsource.LastUpdatedDate = DateTime.Now;
        //    tDemandsource.UpdatedBy = uid;
        //    tDemandsource.ClientId = Client.ClientId;
        //    try
        //    {

        //        int Id = DMDemandSourceService.Create(tDemandsource);
        //        if (Id > 0)
        //        {
        //            tDemandsource.Id = Id;
        //            return Json(new { Result = "OK", Record = tDemandsource });
        //        }
        //        else
        //        {
        //            throw new Exception("Error in saving the data");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        [HttpPost]
        public JsonResult AddLOBDetails(DMLOB tDemandsource)
        {
            int uid = (int)Session["UserId"];
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                DMLOBInput DMPostDemandInput = new DMLOBInput();
                List<DMLOB> lst = DemandManagerHelper.GetLOBSources1(client.ClientId).Where(c => c.DemandLOB.ToLower() == tDemandsource.DemandLOB.ToLower()).ToList();
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

            tDemandsource.CreatedDate = DateTime.Now;
            tDemandsource.LastUpdatedDate = DateTime.Now;
            tDemandsource.UpdatedBy = uid;
            tDemandsource.ClientId = Client.ClientId;
            tDemandsource.IsDeleted = false;
            try
            {

                int Id = DMLOBService.Create(tDemandsource);
                if (Id > 0)
                {
                    tDemandsource.Id = Id;
                    return Json(new { Result = "OK", Record = tDemandsource });
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
        //public JsonResult UpdateDemandSourceDetails(DMDemandSource tDemandsource)
        //{
        //    int uid = (int)Session["UserId"];
        //  //  var Client = (Client)Session["Client"];
        //    ClientDetail Client = (ClientDetail)Session["SessionClient"];
        //    if (ModelState.IsValid)
        //    {
        //        DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
        //        List<DMDemandSource> lst = DemandManagerHelper.GetDemandSources1(Client.ClientId).Where(c => c.DemandSource.ToLower() == tDemandsource.DemandSource.ToLower() && c.Id != tDemandsource.Id && c.ClientId == Client.ClientId).ToList();
        //        if (lst.Count > 0)
        //        {
        //            return Json(new
        //            {
        //                Result = "ERROR",
        //                Message = "Demand Source Already Exists  <br>" +
        //                "Please correct it and try again."
        //            });
        //        }
        //    }

        //    try
        //    {
        //        DMDemandSource DmDemandsource = DMDemandSourceService.Get(tDemandsource.Id);
        //        DmDemandsource.DemandSource = tDemandsource.DemandSource;
        //        DmDemandsource.LastUpdatedDate = DateTime.Now;
        //        DmDemandsource.UpdatedBy = uid;
        //        DMDemandTypeService.Save();
        //        return Json(new { Result = "OK", Record = DmDemandsource });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        [HttpPost]
        public JsonResult UpdateLOBDetails(DMLOB tlobsource)
        {
            int uid = (int)Session["UserId"];
            //var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                DMLOBInput DMLobInput = new DMLOBInput();
                List<DMLOB> lst = DemandManagerHelper.GetLOBSources1(Client.ClientId).Where(c => c.DemandLOB.ToLower() == tlobsource.DemandLOB.ToLower() && c.Id != tlobsource.Id && c.ClientId == Client.ClientId).ToList();
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
                DMLOB Dmlobsource = DMLOBService.Get(tlobsource.Id);
                Dmlobsource.DemandLOB = tlobsource.DemandLOB;
                Dmlobsource.LastUpdatedDate = DateTime.Now;
                Dmlobsource.UpdatedBy = uid;
                DMLOBService.Save();
                return Json(new { Result = "OK", Record = Dmlobsource });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateDemandTypeDetails(DMDemandType tDemandType)
        {
            int uid = (int)Session["UserId"];
         //   var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
               DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
                List<DMDemandType> lst = DemandManagerHelper.GetDemandTypes1(Client.ClientId).Where(c => c.DemandType.ToLower() == tDemandType.DemandType.ToLower() && c.Id != tDemandType.Id && c.ClientId == Client.ClientId).ToList();
                if (lst.Count > 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Demand Type Already Exists  <br>" +
                        "Please correct it and try again."
                    });
                }
            }

            try
            {
                DMDemandType DmDemand = DMDemandTypeService.Get(tDemandType.Id);
                DmDemand.DemandType = tDemandType.DemandType;
                DmDemand.LastUpdatedDate = DateTime.Now;
                DmDemand.UpdatedBy = uid;

                DMDemandTypeService.Save();
                return Json(new { Result = "OK", Record = DmDemand });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult AddITBenefitDetails(DMItemITBenefitsInput tbenefitinput, int ItemId = 0)
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

                    int Id = DMItemITBenefitsService.Create(DMItemITBenefitsMapper.MapToEntity(tbenefitinput, entityDMItemITBenefits));

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
                    throw new Exception("Select Demand/Submit the one!");
                }


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult UpdateITBenefitDetails(DMItemITBenefitsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
            if (!ModelState.IsValid)
            {
                return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
            }
            try
            {
                DMItemITBenefits DmDemand = new DMItemITBenefits();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DmDemand = unitOfWork.DMItemITBenefits.GetByID(tbenefitinput.Id);
                    DmDemand.Name = tbenefitinput.Name;
                    DmDemand.FTEHours = tbenefitinput.FTEHours;
                    DmDemand.Detail = tbenefitinput.Detail;
                    DmDemand.Value = tbenefitinput.Value;
                    DmDemand.LastUpdatedDate = DateTime.Now;
                    DmDemand.UpdatedBy = uid;
                    unitOfWork.DMItemITBenefits.Update(DmDemand);
                    unitOfWork.Save();
                }
               
                return Json(new { Result = "OK", Record = DmDemand });
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
               // var rr = false;
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    unitOfWork.DMItemITBenefits.Delete(Id);
                    unitOfWork.Save();
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }






        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddCommentDetails(DMItemCommentsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
            try
            {
                if(ItemId!=0)
                {
                    tbenefitinput.CreatedBy = uid;
                    tbenefitinput.CreatedDate = DateTime.Now;
                    tbenefitinput.LastUpdatedDate = DateTime.Now;
                    tbenefitinput.UpdatedBy = uid;
                    int Id = DMItemCommentsService.Create(DMItemCommentsMapper.MapToEntity(tbenefitinput, entityDMItemComments));

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
                    throw new Exception("Select Demand/Submit the one!");
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }


        public JsonResult GetDemandTypeDetails()
        {
           // var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            var lst = DemandManagerHelper.GetDemandTypes1(Client.ClientId);
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

        //public JsonResult GetDemandSourceDetails()
        //{
        //  //  var Client = (Client)Session["Client"];
        //    ClientDetail Client = (ClientDetail)Session["SessionClient"];
        //    var lst = DemandManagerHelper.GetDemandSources1(Client.ClientId);
        //    try
        //    {
        //        //Get data from database
        //        //Return result to jTable
        //        return Json(new { Result = "OK", Records = lst });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        public JsonResult GetLOBDetails()
        {
           // var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            var lst = DemandManagerHelper.GetLOBSources1(Client.ClientId);
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

            DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
            DMPostDemandInput.Id = ItemId;
            IList<DMItemComments> lst = DMPostDemandInput.GetItemComments;
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
        public JsonResult AddBusinessBenefitDetails(DMItemBusinessBenefitsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
            try
            {
                if(ItemId!=0)
                {
                    //Update tbenefit Details

                    tbenefitinput.CreatedBy = uid;
                    tbenefitinput.CreatedDate = DateTime.Now;
                    tbenefitinput.LastUpdatedDate = DateTime.Now;
                    tbenefitinput.UpdatedBy = uid;
                    int Id = DMItemBusinessBenefitsService.Create(DMItemBusinessBenefitsMapper.MapToEntity(tbenefitinput, entityDMItemBusinessBenefits));

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
                    throw new Exception("Select Demand/Submit the one!");
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }


        [HttpPost]
        public JsonResult UpdateBusinessBenefitDetails(DMItemBusinessBenefitsInput tbenefitinput, int ItemId = 0)
        {
            int uid = (int)Session["UserId"];
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
            try
            {
                DMItemBusinessBenefits DmDemand = new DMItemBusinessBenefits();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DmDemand = unitOfWork.DMItemBusinessBenefits.GetByID(tbenefitinput.Id);
                    DmDemand.Name = tbenefitinput.Name;
                    DmDemand.FTEHours = tbenefitinput.FTEHours;
                    DmDemand.Detail = tbenefitinput.Detail;
                    DmDemand.Value = tbenefitinput.Value;
                    DmDemand.LastUpdatedDate = DateTime.Now;
                    DmDemand.UpdatedBy = uid;
                    unitOfWork.DMItemBusinessBenefits.Update(DmDemand);
                    unitOfWork.Save();
                }
                return Json(new { Result = "OK", Record = DmDemand });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
        }

        [HttpPost]
        public JsonResult DeleteBusinessBenefitDetails(DMItemBusinessBenefitsInput tbenefitinput)
        {
            try
            {
                DMItemBusinessBenefitsService.Delete(tbenefitinput.Id);
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

            DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
            DMPostDemandInput.Id = ItemId;
            IList<DMItemBusinessBenefits> lst = DMPostDemandInput.GetBusinessBenefits;
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
            DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();
            DMPostDemandInput.Id = ItemId;
            IList<DMItemITBenefits> lst = DMPostDemandInput.GetITBenefits;
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
            DMPostDemandInput DMPostDemandInput = new DMPostDemandInput();

            DMPostDemandInput.ClientId = Client.ClientId;
            
            if (ItemId > 0)
            {
                DMPostDemandInput.PostDemandInput = DMPostChallangeMapper.MapToInput(service.Get(ItemId));
               // return RedirectToAction("DisplayHomePage", new { ItemId = ItemId });
            }
            else
            {
                DMPostDemandInput.PostDemandInput = new PostDemandInput();
            }

            DMPostDemandInput.TabName = TabName;
            DMPostDemandInput.Id = ItemId;

            if (SaveData.Length > 0)
            {
                ModelState.AddModelError("Success", "Data saved successfully");
            }
            return View("ItemPostTypePage", DMPostDemandInput);
        }

        public ActionResult getClientId()
        {
            //var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            return Json(Client.ClientId);
        }

        [ActionTrack]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SavePostDemand(FormCollection form, DMPostDemandInput inputs, string command, int ClientId = 0, int ItemId = 0, string PostCreate = "")
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            int uid = (int)Session["UserId"];
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            string TabName = "";
            string SaveData = "";
            if (command == "saveDemand")
            {
                TabName = "home";
                PostDemandInput postchallangeInput = inputs.PostDemandInput;
                postchallangeInput.ClientId = inputs.ClientId;
                if (inputs.PostDemandInput.StatusId==0)
                {
                    inputs.PostDemandInput.StatusId = (int)DMStatus.Created;
                }

                if (!ModelState.IsValid)
                {
                      return View("ItemPostTypePage", inputs);
                }

                //Get Login Id (ItemCreated By Id)
                
                if (ItemId > 0)
                {
                    inputs.PostDemandInput.Id = ItemId;
                    inputs.PostDemandInput.LastUpdatedDate = DateTime.Now;
                    inputs.PostDemandInput.LastUpdatedUser = GetUserName(uid);
                    inputs.PostDemandInput.UpdatedBy = uid;
                   
                    //postchallangeInput.Description = Server.HtmlDecode(postchallangeInput.Description);
                    DMPostChallangeMapper.MapToEntity(inputs.PostDemandInput, service.Get(ItemId));                
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
                   
                    postchallangeInput.StatusId =(int)DMStatus.Created;
                    postchallangeInput.PriorityId = 5;
                    
                    //postchallangeInput.CreatedUserId = uid;
                    postchallangeInput.CreatedDate = DateTime.Now;
                    //postchallangeInput.Description = Server.HtmlDecode(postchallangeInput.Description);
                    ItemId = service.Create(AutoMapper.Mapper.Map<PostDemandInput, DMPostDemand>(postchallangeInput));


                    DMPriorityHistory priorityHistory = new DMPriorityHistory();
                    priorityHistory.OldPriorityId = (int)postchallangeInput.PriorityId;
                    priorityHistory.NewPriorityId = (int)postchallangeInput.PriorityId;
                    priorityHistory.DemandId = ItemId;
                    priorityHistory.CreatedBy = uid;
                    priorityHistory.UpdatedBy = uid;
                    priorityHistory.LastUpdatedDate = DateTime.Now;
                    priorityHistory.CreatedDate = DateTime.Now;
                    priorityHistory.Comments = "Created(By default)";
                    priorityHistory.IsDeleted = false;
                    priorityHistory.ClientId = client.ClientId;
                    unitOfWork.DMPriorityHistory.Insert(priorityHistory);

                    DMStatusHistory statusHistory = new DMStatusHistory();
                    statusHistory.DemandId = ItemId;
                    statusHistory.OldStatusId = (int)DMStatus.Created;//coz this is also not null
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.NewStatusId = (int)DMStatus.Created;// 1 for Created
                    statusHistory.CreatedBy = uid;
                    statusHistory.Comments = "Created(By default)";//Coz it is Not null;
                    statusHistory.ClientId = client.ClientId;
                        //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        unitOfWork.DMStatusHistory.Insert(statusHistory);
                        var m = unitOfWork.UserProfile.GetByID(uid);
                        var    email = m.Email;
                        var    fullname = m.FirstName +" "+ m.LastName;
                        unitOfWork.Save();
                    SendDemandNotification(email,fullname, postchallangeInput.ItemName, (DMStatus)postchallangeInput.StatusId,client.ClientName);
                    
                }

                ModelState.AddModelError("Success", "Demand posted successfully");

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

        public void SaveCategoryDetails(FormCollection form, DMPostDemandInput inputs, int ClientId, int ItemId)
        {

            int uid = (int)Session["UserId"];
         //  string SaveData = "Save Category";
            DMItemCategoryMapService = new CrudService<DMItemCategoryMap>(repoDMItemCategoryMap);
            if (ItemId > 0)
            {
                IList<DMItemCategoryMap> lst = DMItemCategoryMapService.GetAll().Where(c => c.ItemId == ItemId).ToList();
                foreach (DMItemCategoryMap item in lst)
                {
                    DMItemCategoryMapService.Delete(item.Id);
                }
            }


            string[] selectedcategory = form.GetValues("Demandclassifcation");
            int userId = WebSecurity.GetUserId("amo");
            if (selectedcategory != null)
            {
                foreach (var item in selectedcategory)
                {
                    DMItemCategoryMap mapItem = new DMItemCategoryMap();
                    mapItem.CategoryId = Convert.ToInt32(item);
               
                    mapItem.ItemId = ItemId;
                  //  mapItem.CreatedByUser = GetUserName(uid);
                    mapItem.CreatedDate = DateTime.Now;
                    mapItem.UpdatedBy = uid;
                    DMItemCategoryMapService.Create(mapItem);

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
            var existItemNames = from DMPostDemand in db.DMPostDemand
                                 where DMPostDemand.ItemName.ToLower() == ItemName.ToLower() && DMPostDemand.CreatedBy == uid && DMPostDemand.ClientId == client.ClientId
                                 select new
                                 {
                                     Id = DMPostDemand.Id,
                                     Name = DMPostDemand.ItemName
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
        public ActionResult DemandsApproved(int? page, string sortOrder)
        {
            int uid = (int)Session["UserId"];
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ViewBag.Demands = String.IsNullOrEmpty(sortOrder) ? "Demand_desc" : "";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            ViewBag.Description = string.IsNullOrEmpty(sortOrder) ? "Description_desc" : "Description";
            ViewBag.Createdby = string.IsNullOrEmpty(sortOrder) ? "Createdby_desc" : "Createdby";
            ViewBag.Demandtype = string.IsNullOrEmpty(sortOrder) ? "Demandtype_desc" : "Demandtype";
            ViewBag.Demandsource = string.IsNullOrEmpty(sortOrder) ? "Demandsource_desc" : "Demandsource";
            ViewBag.LOB = string.IsNullOrEmpty(sortOrder) ? "LOB_desc" : "LOB";
            ViewBag.configitem = string.IsNullOrEmpty(sortOrder) ? "configitem_desc" : "configitem";
            var user = "";
            IList<DemandModel> x = new List<DemandModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                user = unitOfWork.UserProfile.Get(g => g.Id == uid).Select(g => g.RoleId).FirstOrDefault();
            }
            if (user.ToLower() == "superuser")
            {
                x = AllPostDemand_SuperUser(client.ClientId);
            }
            else
            {
                x = AllPostDemands(client.ClientId, uid);
            }

            IPagedList<DemandModel> Demands = null;
            switch (sortOrder)
            {
                case "Demand_desc":
                    Demands = x.OrderByDescending(s => s.DemandName).ToPagedList(page ?? 1, 6);
                    break;
                case "Status_desc":
                    Demands = x.OrderByDescending(s => s.Status).ToPagedList(page ?? 1, 6);
                    break;
                case "Status":
                    Demands = x.OrderBy(s => s.Status).ToPagedList(page ?? 1, 6);
                    ViewBag.Status = "Status_desc";
                    break;
                case "Description_desc":
                    Demands = x.OrderByDescending(s => s.Description).ToPagedList(page ?? 1, 6);
                    break;
                case "Description":
                    Demands = x.OrderBy(s => s.Description).ToPagedList(page ?? 1, 6);
                    ViewBag.Description = "Description_desc";
                    break;
                case "Createdby_desc":
                    Demands = x.OrderByDescending(s => s.CreatedByUser).ToPagedList(page ?? 1, 6);
                    break;
                case "Createdby":
                    Demands = x.OrderBy(s => s.CreatedByUser).ToPagedList(page ?? 1, 6);
                    ViewBag.Createdby = "Createdby_desc";
                    break;
                case "Demandtype_desc":
                    Demands = x.OrderByDescending(s => s.DemandType).ToPagedList(page ?? 1, 6);
                    break;
                case "Demandtype":
                    Demands = x.OrderBy(s => s.DemandType).ToPagedList(page ?? 1, 6);
                    ViewBag.Demandtype = "Demandtype_desc";
                    break;
                case "Demandsource_desc":
                    Demands = x.OrderByDescending(s => s.DemandSource).ToPagedList(page ?? 1, 6);
                    break;
                case "Demandsource":
                    Demands = x.OrderBy(s => s.DemandSource).ToPagedList(page ?? 1, 6);
                    ViewBag.Demandsource = "Demandsource_desc";
                    break;
                case "LOB_desc":
                    Demands = x.OrderByDescending(s => s.LOB).ToPagedList(page ?? 1, 6);
                    break;
                case "LOB":
                    Demands = x.OrderBy(s => s.LOB).ToPagedList(page ?? 1, 6);
                    ViewBag.LOB = "LOB_desc";
                    break;
                case "configitem_desc":
                    Demands = x.OrderByDescending(s => s.ConfigItemName).ToPagedList(page ?? 1, 6);
                    break;
                case "configitem":
                    Demands = x.OrderBy(s => s.ConfigItemName).ToPagedList(page ?? 1, 6);
                    ViewBag.configitem = "configitem_desc";
                    break;

                default:
                    Demands = x.OrderBy(s => s.DemandName).ToPagedList(page ?? 1, 6);
                    break;
            }
            return View("DemandsApproved", Demands);
        }

        [HttpPost]
        public ActionResult Selection_Read([DataSourceRequest] DataSourceRequest request)
        {
            int uid = (int)Session["UserId"];
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var user = "";
            IList<DemandModel> xcac = new List<DemandModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                user = unitOfWork.UserProfile.Get(g => g.Id == uid).Select(g => g.RoleId).FirstOrDefault();
            }
            if (user.ToLower() == "superuser")
            {
                xcac = AllPostDemand_SuperUser(client.ClientId);
            }
            else
            {
                xcac = AllPostDemands(client.ClientId, uid);
            }


            return Json(xcac.ToDataSourceResult(request));
        }


        public static IList<DemandModel> AllPostDemand_SuperUser(int clientId)
        {

            Db db = new Db();
            var Demandmod = (from posts in db.DMPostDemand
                           join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                           join idt in db.DMDemandType on posts.DemandTypeId equals idt.Id into tmpidt
                           join idst in db.DMItemStatus on posts.StatusId equals idst.Id into tmpidst
                          // join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                           join lob in db.DMLOB on posts.LOBId equals lob.Id into tmplob
                           from status in tmpidst.DefaultIfEmpty()
                           //from source in tmpids.DefaultIfEmpty()
                           from idtype in tmpidt.DefaultIfEmpty()
                         //  from apps in tmpapp.DefaultIfEmpty()
                           from lobs in tmplob.DefaultIfEmpty()
                           where posts.ClientId == clientId && posts.StatusId != (int)DMStatus.Implemented
                           select new DemandModel
                           {
                              // CategoryId = posts.CategoryId,
                               ClientId = posts.ClientId,
                               //ConfigItem = posts.ConfigItem,
                               //ConfigItemName = apps.ApplicationName,
                               CreatedByUser = usr.FirstName + " " + usr.LastName,
                               CreatedBy = posts.CreatedBy,
                               CreatedDate = posts.CreatedDate,
                               Description = posts.Description,
                               DemandId = posts.Id,
                               DemandName = posts.ItemName,
                               DemandType = idtype.DemandType,
                               IsDeleted = posts.IsDeleted,
                               //  ItemType= posts.id
                               LOB = lobs.DemandLOB,
                               LOBId = posts.LOBId,
                             //  PriorityComments = posts.PriorityComments,
                             //  StatusComments = posts.StatusComments,
                             //  PriorityId = posts.PriorityId,
                               QtyFication = posts.QtyFication,
                               Status = status.Status,
                               StatusId = posts.StatusId
                           }
                          ).ToList();
            return Demandmod;

        }


        public static IList<DemandModel> AllPostDemands(int clientId, int uid)
        {
            
            Db db = new Db();
            var Demandmod = (from posts in db.DMPostDemand
                           join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                          join IA in db.DMDemandApprover on posts.Id equals IA.DemandId
                           join idt in db.DMDemandType on posts.DemandTypeId equals idt.Id into tmpidt
                          join idst in db.DMItemStatus on posts.StatusId equals idst.Id into tmpidst
                       //   join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                          join lob in db.DMLOB on posts.LOBId equals lob.Id into tmplob
                          from status in tmpidst.DefaultIfEmpty()
                          //from source in tmpids.DefaultIfEmpty()
                          from idtype in tmpidt.DefaultIfEmpty()
                        //  from apps in tmpapp.DefaultIfEmpty()
                          from lobs in tmplob.DefaultIfEmpty()
                          where posts.ClientId == clientId && IA.ApproverId==uid && posts.StatusId != (int)DMStatus.Implemented
                          select new DemandModel
                          {
                             // CategoryId = posts.CategoryId,
                              ClientId = posts.ClientId,
                             // ConfigItem = posts.ConfigItem,
                           //   ConfigItemName = apps.ApplicationName,
                              CreatedByUser = usr.FirstName + " " + usr.LastName,
                              CreatedBy = posts.CreatedBy,
                              CreatedDate = posts.CreatedDate,
                              Description = posts.Description,
                              DemandId = posts.Id,
                              DemandName = posts.ItemName,
                              DemandType = idtype.DemandType,
                              IsDeleted = posts.IsDeleted,
                              //  ItemType= posts.id
                              LOB = lobs.DemandLOB,
                              LOBId = posts.LOBId,
                              QtyFication = posts.QtyFication,
                              Status = status.Status,
                              StatusId = posts.StatusId
                          }
                          ).ToList();
            return Demandmod;

        }


        public ActionResult testingUpdate(IList<DemandModel> inputs, string btnSubmit)
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

                    ModelState.AddModelError(c.DemandId + "mandate", c.DemandName + "- Priority and Comments are mandatory.");
                    checkboxchecked = true;
                }
            }
            if(!checkboxchecked)
            {
                ModelState.AddModelError("chkerr", "Please select any one of the Demands");
            }
            if (!ModelState.IsValid)
            {
                return View("DemandsApproved", inputs.ToPagedList(1, 8));
            }
            if (ModelState.IsValid)
            {
                foreach (var c in inputs)
                {
                    if (btnSubmit == "Approve")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)DMStatus.Created || c.StatusId==(int)DMStatus.Posted || c.StatusId == (int)DMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.DemandId, DMStatus.Approved, c.StatusComments, c.PriorityId);
                            ApprovedDateUpdate(c.DemandId, uid);
                        }
                    }
                    if (btnSubmit == "Submit")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)DMStatus.Created || c.StatusId == (int)DMStatus.Posted || c.StatusId == (int)DMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.DemandId, DMStatus.Submitted, c.StatusComments, c.PriorityId);
                        }
                    }
                    if (btnSubmit == "Decline")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)DMStatus.Created || c.StatusId == (int)DMStatus.Posted || c.StatusId == (int)DMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.DemandId, DMStatus.Declined , c.StatusComments, c.PriorityId);
                        }
                    }
                    if (btnSubmit == "Close")
                    {
                        if (c.IsChecked == true && (c.StatusId == (int)DMStatus.Created || c.StatusId == (int)DMStatus.Posted || c.StatusId == (int)DMStatus.Submitted || c.StatusId == 0))
                        {
                            updatestatus1(c.DemandId, DMStatus.Cancelled, c.StatusComments, c.PriorityId);
                        }
                    }
                }
                TempData.Add("ValidationSuccessMsg", "Status got updated successfully.");
            }

            return RedirectToAction("DemandsApproved");
        }
        public static void ApprovedDateUpdate(int DemandId, int uid)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var getRow = unitOfWork.DMDemandApprover.Get(g => g.DemandId == DemandId && g.ApproverId == uid).FirstOrDefault();
                if (getRow != null)
                {
                    getRow.ApprovedDate = DateTime.Now;
                    unitOfWork.DMDemandApprover.Update(getRow);
                    unitOfWork.Save();
                }
            }
            catch (Exception)
            {

            }
        }
        
        public void updatestatus1(int Demandid, DMStatus status, string Comments, int priority)
        {
            int uid = (int)Session["UserId"];
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                {
                    var Demand = unitOfWork.DMPostDemand.GetByID(Demandid);
                    if (Demand != null)
                    {
                        var oldStatus = Demand.StatusId;
                        Demand.StatusId = (int)status; //2;// 2 for Submitted; 1 for created
                        Demand.PriorityId = priority;
                        unitOfWork.DMPostDemand.Update(Demand);


                        var oldPrioritObject = unitOfWork.DMPriorityHistory.Get().Where(w => w.DemandId == Demandid).OrderByDescending(o => o.Id).FirstOrDefault();
                        int oldpId = 0;
                        if (oldPrioritObject != null)
                        {
                         //   oldpId = oldPrioritObject.NewPriorityId;
                        }
                        DMPriorityHistory priorityHistory = new DMPriorityHistory();
                        priorityHistory.OldPriorityId = oldpId;

                        priorityHistory.DemandId = Demandid;
                        priorityHistory.CreatedBy = uid;
                        priorityHistory.CreatedDate = DateTime.Now;
                        priorityHistory.NewPriorityId = priority;
                        priorityHistory.UpdatedBy = uid;
                        priorityHistory.LastUpdatedDate = DateTime.Now;
                        priorityHistory.Comments = Comments==null?"":Comments;
                        priorityHistory.IsDeleted = false;
                        priorityHistory.ClientId = client.ClientId;
                        unitOfWork.DMPriorityHistory.Insert(priorityHistory);

                        DMStatusHistory statusHistory = new DMStatusHistory();
                        statusHistory.DemandId = Demand.Id;
                        //statusHistory.Comments = Demand.StatusComments==null?"":Demand.StatusComments;
                        statusHistory.LastUpdatedDate = DateTime.Now;
                        statusHistory.UpdatedBy = uid;
                        statusHistory.OldStatusId = oldStatus;
                        statusHistory.ClientId = client.ClientId;
                        statusHistory.NewStatusId = Demand.StatusId;
                        unitOfWork.DMStatusHistory.Insert(statusHistory);

                        var listOfAdmins = unitOfWork.DMDemandApprover.Get(g => g.DemandId == Demandid).Select(g=>g.ApproverId).ToList();
                        foreach (var Adid in listOfAdmins)
                        {
                            var m1 = unitOfWork.UserProfile.GetByID(Demand.CreatedBy);
                            var email1 = m1.Email;
                            var fullname1 = m1.FirstName + " " + m1.LastName;
                            SendDemandNotification(email1, fullname1, Demand.ItemName, (DMStatus)Demand.StatusId, client.ClientName);
                        }
                        var m = unitOfWork.UserProfile.GetByID(Demand.CreatedBy);
                        var email = m.Email;
                        var fullname = m.FirstName + " " + m.LastName;
                        SendDemandNotification(email, fullname, Demand.ItemName, (DMStatus)Demand.StatusId, client.ClientName);

                    }
                }
                unitOfWork.Save();
            }


        }
        //[HttpPost]
        //public JsonResult ListOfDMAdmins([DataSourceRequest] DataSourceRequest request, int DemandId)
        //{ 
        //    //Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    Db db = new Db();
        //    var TEMPlistOfAdmins = (from UA in db.UserApplications
        //                        join UP in db.UserProfiles on UA.UserId equals UP.Id
        //                        join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
        //                        join UR in db.UserRoles on UA.RoleId equals UR.Id
        //                        join APPRS in db.DMDemandApprover.Where(w=>w.DemandId == DemandId) on UP.Id equals APPRS.ApproverId into tmpApprs
        //                        from aprs in tmpApprs.DefaultIfEmpty()
        //                        where ASA.ApplicationName.ToLower() == "demand manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin") && UA.ClientId == client.ClientId
        //                        select new DemandApproverModel
        //                        {
        //                            Id =UP.Id,
        //                            FirstName = UP.FirstName,
        //                            LastName=UP.LastName,
        //                            DemandId = DemandId,
        //                            DemandApproverId = aprs.Id,
        //                            ApprovedDate = aprs.ApprovedDate
        //                        }
        //                       ).Distinct().OrderBy(x=>x.FirstName).ToList();

        //    //Adding Status infront it ...
        //    //Logic is => IF (__ApprovedDt__ is NULL) then Admin has not Approved(might have cancelled/Hold/Decline)
        //    //Else  (i.e, __ApprovedDt__ is NOT NULl) Admin has Approved the IDEA
        //    //Also checking DemandApproverId property in above Object -> to check whether they are selected as aprrover or not
        //    var listOfAdmins = TEMPlistOfAdmins.Select(f =>
        //            new DemandApproverModel
        //            {
        //                Id = f.Id,
        //                FirstName = f.FirstName,
        //                LastName = f.LastName,
        //                DemandId = f.DemandId,
        //                DemandApproverId = f.DemandApproverId,
        //                ApprovedDate = f.ApprovedDate,
        //                ApprovedOrNot = (f.ApprovedDate != null || f.ApprovedDate > DateTime.MinValue) ? "Approved" : (f.DemandApproverId == null ? "":"Not Approved")

        //            }
        //    );

        //    return Json(listOfAdmins.ToDataSourceResult(request));
        //}

        [ActionTrack]
        [HttpPost]
        public ActionResult AddSelectedAdmins(int ItemId =0, string DemandIds = "")
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            string Result = "Success";
            try
            {
                if (DemandIds == "")
                {
                    //throw new Exception("Select Approver!");
                    throw new Exception("1");
                }
                if(ItemId!=0 )
               // if (ItemId!=""|| ItemId!="0" )
                {
                    //string[] SelectedAdminIds = DemandIds.Substring(1).Split(',');
                    string[] SelectedAdminIds = DemandIds.Split(',');
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                        var Demandname = unitOfWork.DMPostDemand.Get(g => g.Id == ItemId).FirstOrDefault();
                        Demandname.StatusId = (int)DMStatus.Posted;
                        Demandname.LastUpdatedDate = DateTime.Now;
                        Demandname.UpdatedBy = uid;
                        unitOfWork.DMPostDemand.Update(Demandname);
                        
                        foreach (var item in SelectedAdminIds)
                        {
                            if(item!=" "||item!=","||item!="")
                            {
                                DMDemandApprover DemandApprover = new DMDemandApprover();
                                DemandApprover.DemandId = Convert.ToInt32(ItemId);
                                DemandApprover.ClientId = client.ClientId;
                                DemandApprover.ApproverId = Convert.ToInt32(item);
                                unitOfWork.DMDemandApprover.Insert(DemandApprover);

                                
                                var u = unitOfWork.UserProfile.Get(g => g.Id == DemandApprover.ApproverId).FirstOrDefault();
                                SendDemandNotification(u.Email, u.FirstName + " " + u.LastName, Demandname.ItemName, DMStatus.Posted, client.ClientName);

                            }
                        }
                        unitOfWork.Save();
                    }
                }
               
                else
                {
                    //throw new Exception("Select Demand/Submit the one!");
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
                    var Demand = unitOfWork.DMPostDemand.GetByID(idid);
                    if (Demand != null)
                    {
                        var oldStatus = Demand.StatusId;
                        Demand.StatusId = (int)DMStatus.Implemented;

                        unitOfWork.DMPostDemand.Update(Demand);


                        DMStatusHistory statusHistory = new DMStatusHistory();
                        statusHistory.DemandId = Demand.Id;
                        statusHistory.LastUpdatedDate = DateTime.Now;
                        statusHistory.UpdatedBy = uid;
                        statusHistory.OldStatusId = oldStatus;
                        statusHistory.NewStatusId = Demand.StatusId;
                        statusHistory.ClientId = client.ClientId;
                        unitOfWork.DMStatusHistory.Insert(statusHistory);
                        var m = unitOfWork.UserProfile.GetByID(Demand.CreatedBy);
                        var email = m.Email;
                        var fullname = m.FirstName + " " + m.LastName;
                        SendDemandNotification(email,fullname, Demand.ItemName, (DMStatus)Demand.StatusId, client.ClientName);
                    }
                }
                unitOfWork.Save();
            }
            return RedirectToAction("DisplayDemands");
        }

        public void SendDemandNotification(string email,string fullname,string DemandName, DMStatus status, string clientname)
        {
            try
            {
                var body = "";

                var mailId = email;
                var Subject = "Smart IT Demand Manager Auto notification";
                if (status == DMStatus.Created)
                {
                    body = ThankyouBody(fullname, clientname);
                }
                else if(status == DMStatus.Posted)
                {
                    //fulllname = Admin Name(Approver)
                    body = DemandApproverBody(fullname, DemandName, status.ToString(), clientname);
                }
                else
                {
                    body = StatusBody(fullname, DemandName, status.ToString(), clientname);
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

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/ThankYouEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{clientname}", clientname);

            return body;
        }
        [ActionTrack]
        private string DemandApproverBody(string adminName, string Demand, string status, string clientname)
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
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/DemandApproverEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{Demandname}", Demand);
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{Adminname}", adminName);

            return body;
        }


        private string StatusBody(string username, string Demand, string status, string clientname)
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
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/DemandStatusEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{Demandname}", Demand);
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{Adminname}", adminName);

            return body;
        }

    }
}
    
