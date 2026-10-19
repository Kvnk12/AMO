using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Reflection;
using System.Globalization;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WebUI.Dto;
using WebUI.Models;
using Data;
using NTTData.AMOPortal.WebUI.Filters;
using log4net;

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class VerticalSubVerticalsGridController : Cruder<VerticalAndSubVertical, VerticalAndSubVerticalInput>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(VerticalSubVerticalsGridController));

        //Entities
        private VerticalAndSubVertical entityVerticalSubVertical;

        //Service
        private CrudService<VerticalAndSubVertical> verticalAndSubVerticalService;

        //Mappers
        private Mapper<VerticalAndSubVertical, VerticalAndSubVerticalInput> verticalAndSubVerticalMapper;

        //Repo
        private Repo<VerticalAndSubVertical> repoVerticalAndSubVertical;

        /// <summary>
        /// Get Selected Vertical to SubVertical
        /// </summary>
        /// <param name="service"></param>
        /// <param name="v"></param>
        /// 

        //Entities
        private SubVerticals entitySubVerticals;

        //Service

        private CrudService<SubVerticals> subVerticalsService;

        //Mappers

        private Mapper<SubVerticals, SubVerticalsInput> subVerticalsMapper;

        //Repo

        private Repo<SubVerticals> repoSubVerticals;


        //For Verticals mapper

        //Entities
        private Verticals entityVerticals;

        //Service

        private CrudService<Verticals> verticalsService;

        //Mappers

        private Mapper<Verticals, VerticalsInput> verticalsMapper;

        //Repo

        private Repo<Verticals> repoVerticals;


        // <summary>
        // BusinessCapabilitiesLevel1 mapper
        // </summary>
        // <param name="service"></param>
        // <param name="v"></param>


        //////Entities
        ////private BusinessCapabilitiesLevel1 entityBusinessCapabilitiesLevel1;

        //////Service

        ////private CrudService<BusinessCapabilitiesLevel1> businessCapabilitiesLevel1Service;

        //////Mappers

        ////private Mapper<BusinessCapabilitiesLevel1, BusinessCapabilitiesLevel1Input> businessCapabilitiesLevel1Mapper;

        //////Repo

        ////private Repo<BusinessCapabilitiesLevel1> repoBusinessCapabilitiesLevel1;

        ////// <summary>
        ////// BusinessCapabilitiesLevel2 mapper
        ////// </summary>
        ////// <param name="service"></param>
        ////// <param name="v"></param>


        //////Entities
        ////private BusinessCapabilitiesLevel2 entityBusinessCapabilitiesLevel2;

        //////Service

        ////private CrudService<BusinessCapabilitiesLevel2> businessCapabilitiesLevel2Service;

        //////Mappers

        ////private Mapper<BusinessCapabilitiesLevel2, BusinessCapabilitiesLevel2Input> businessCapabilitiesLevel2Mapper;

        //////Repo

        ////private Repo<BusinessCapabilitiesLevel2> repoBusinessCapabilitiesLevel2;

        // <summary>
        // BusinessCapabilitiesLevel3 mapper
        // </summary>
        // <param name="service"></param>
        // <param name="v"></param>


        //Entities
        ////private BusinessCapabilitiesLevel3 entityBusinessCapabilitiesLevel3;

        //////Service

        ////private CrudService<BusinessCapabilitiesLevel3> businessCapabilitiesLevel3Service;

        //////Mappers

        ////private Mapper<BusinessCapabilitiesLevel3, BusinessCapabilitiesLevel3Input> businessCapabilitiesLevel3Mapper;

        //////Repo

        ////private Repo<BusinessCapabilitiesLevel3> repoBusinessCapabilitiesLevel3;

        public VerticalSubVerticalsGridController(ICrudService<VerticalAndSubVertical> service, IMapper<VerticalAndSubVertical, VerticalAndSubVerticalInput> v)
            : base(service, v)
        {


            DbContextFactory Dbcon = new DbContextFactory();
            repoVerticalAndSubVertical = new Repo<VerticalAndSubVertical>(Dbcon);
            verticalAndSubVerticalMapper = new Mapper<VerticalAndSubVertical, VerticalAndSubVerticalInput>();

            entityVerticalSubVertical = new VerticalAndSubVertical();

            verticalAndSubVerticalService = new CrudService<VerticalAndSubVertical>(repoVerticalAndSubVertical);

            ///Setup Client Account Contract Service
            ///
            //DbContextFactory Dbcon = new DbContextFactory();

            repoSubVerticals = new Repo<SubVerticals>(Dbcon);
            subVerticalsMapper = new Mapper<SubVerticals, SubVerticalsInput>();
            entitySubVerticals = new SubVerticals();
            subVerticalsService = new CrudService<SubVerticals>(repoSubVerticals);

            ///setup Client Account Service
            ///

            repoVerticals = new Repo<Verticals>(Dbcon);
            verticalsMapper = new Mapper<Verticals, VerticalsInput>();
            entityVerticals = new Verticals();

            verticalsService = new CrudService<Verticals>(repoVerticals);


            //    Setup BusinessCapabilitiesLevel1 Service

            //    DbContextFactory Dbcon = new DbContextFactory();


            ////repoBusinessCapabilitiesLevel1 = new Repo<BusinessCapabilitiesLevel1>(Dbcon);
            ////businessCapabilitiesLevel1Mapper = new Mapper<BusinessCapabilitiesLevel1, BusinessCapabilitiesLevel1Input>();
            ////entityBusinessCapabilitiesLevel1 = new BusinessCapabilitiesLevel1();
            ////businessCapabilitiesLevel1Service = new CrudService<BusinessCapabilitiesLevel1>(repoBusinessCapabilitiesLevel1);

            //////    setup BusinessCapabilitiesLevel2 Service


            ////repoBusinessCapabilitiesLevel2 = new Repo<BusinessCapabilitiesLevel2>(Dbcon);
            ////businessCapabilitiesLevel2Mapper = new Mapper<BusinessCapabilitiesLevel2, BusinessCapabilitiesLevel2Input>();
            ////entityBusinessCapabilitiesLevel2 = new BusinessCapabilitiesLevel2();
            ////businessCapabilitiesLevel2Service = new CrudService<BusinessCapabilitiesLevel2>(repoBusinessCapabilitiesLevel2);

            //    setup BusinessCapabilitiesLevel3 Service


            ////repoBusinessCapabilitiesLevel3 = new Repo<BusinessCapabilitiesLevel3>(Dbcon);
            ////businessCapabilitiesLevel3Mapper = new Mapper<BusinessCapabilitiesLevel3, BusinessCapabilitiesLevel3Input>();
            ////entityBusinessCapabilitiesLevel3 = new BusinessCapabilitiesLevel3();
            ////businessCapabilitiesLevel3Service = new CrudService<BusinessCapabilitiesLevel3>(repoBusinessCapabilitiesLevel3);

        }

        [HttpGet]
        public ActionResult DisplayVerticalSubVerticals(int VerticalId = 0, int SubVerticalId = 0, string VerticalName = "", string VerticalCreate = "", string SubVerticalCreate = "", int subVId = 0, string title = "")
        {
            //Client client =(Client) Session["Client"];

            VerticalAndSubVerticalInput verticalSubVerticals = new VerticalAndSubVerticalInput();
            verticalSubVerticals.VerticalId = VerticalId;
           // verticalSubVerticals.ClientId = client.Id;
            verticalSubVerticals.SubVerticalId = SubVerticalId;
            verticalSubVerticals.VerticalName = Server.HtmlEncode(VerticalName);
            verticalSubVerticals.VerticalCreate = VerticalCreate;
            verticalSubVerticals.SubVerticalCreate = SubVerticalCreate;
            verticalSubVerticals.subVId = subVId;
            verticalSubVerticals.title = title;
            //clientAccounts = clientAndAccountsService.Get(ContractId);

            if (VerticalId > 0)
            {
                verticalSubVerticals.verticalInput = verticalsMapper.MapToInput(verticalsService.Get(VerticalId));

            }


            if (VerticalId > 0 && SubVerticalId > 0)
            {
                verticalSubVerticals.subVerticalInput = subVerticalsMapper.MapToInput(subVerticalsService.Get(SubVerticalId));

            }


            if (VerticalId == 0 && verticalSubVerticals.VerticalCreate == "VerticalCreate")
            {
               // entityVerticals.ClientId = client.Id;
                verticalSubVerticals.verticalInput = verticalsMapper.MapToInput(entityVerticals);


            }

            if (VerticalId > 0 && SubVerticalCreate == "SubVerticalCreate")
            {

                verticalSubVerticals.subVerticalInput = subVerticalsMapper.MapToInput(entitySubVerticals);




            }

            if (TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", "Data has been saved successfully");
            }
            return View("NewVerticalSubVerticalsGrid", verticalSubVerticals);

        }

        [HttpPost]
        public ActionResult DisplayVerticalSubVerticals(VerticalAndSubVerticalInput inputs, int VerticalId = 0, int SubVerticalId = 0, string VerticalName = "", string VerticalCreate = "", string SubVerticalCreate = "", int subVId = 0, string title = "")
        {
            //ViewBag.StatusMessage = "";
            //Client client = (Client)Session["Client"];
            //inputs.ClientId = client.Id;
            inputs.VerticalId = VerticalId;
            inputs.SubVerticalId = SubVerticalId;
            inputs.VerticalName = Server.HtmlEncode(VerticalName);
            inputs.VerticalCreate = VerticalCreate;
            inputs.SubVerticalCreate = SubVerticalCreate;


            //Go To action account details saving
            if ((inputs.SubVerticalId == 0 && SubVerticalCreate == ""))
            {
                return (SaveVerticalDetails(inputs));
            }

            var input = inputs.subVerticalInput;
            input.VerticalId = inputs.VerticalId;


            if (!ModelState.IsValid)
            {
                return View("NewVerticalSubVerticalsGrid", inputs);
            }


            //Edit Existing Contract
            if (inputs.SubVerticalId != 0)
            {
                input.Id = inputs.SubVerticalId;
                using (Db db = new Db())
                {
                    SubVerticals existingSubVerticalName = db.IndustrySubVerticals.FirstOrDefault(c => c.SubVerticalName.ToLower().Trim() == input.SubVerticalName.ToLower().Trim());
                    if (existingSubVerticalName != null && (existingSubVerticalName.Id != input.Id))
                    {
                        if (existingSubVerticalName != null)
                        {
                            ModelState.AddModelError("SubVerticalName", "SubVertical Name already Exists");
                            return View("NewVerticalSubVerticalsGrid", inputs);
                        }
                    }
                }
            }



            if (input.Id > 0)
            {
                subVerticalsMapper.MapToEntity(input, subVerticalsService.Get(input.Id));
                subVerticalsService.Save();
            }
            else
            {
                //Add New Contract 

                using (Db db = new Db())
                {
                    SubVerticals existingSubVerticalName = db.IndustrySubVerticals.FirstOrDefault(c => c.SubVerticalName.ToLower().Trim() == input.SubVerticalName.ToLower().Trim());
                    if (existingSubVerticalName != null && (existingSubVerticalName.Id != input.Id))
                    {
                        if (existingSubVerticalName != null)
                        {
                            ModelState.AddModelError("SubVerticalName", "SubVertical Name already Exists");
                            return View("NewVerticalSubVerticalsGrid", inputs);
                        }
                    }
                }

                var SsubVerticalId = subVerticalsService.Create(subVerticalsMapper.MapToEntity(input, entitySubVerticals));
                input.Id = Convert.ToInt32(SsubVerticalId);

            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayVerticalSubVerticals", new { VerticalId = inputs.VerticalId, VerticalName = inputs.VerticalName, SubVerticalId = input.Id, tabs = "Tab2" });

        }



        [HttpPost]
        public ActionResult SaveVerticalDetails(VerticalAndSubVerticalInput inputs)
        {
            //Client client = (Client)Session["Client"];

            VerticalsInput vertical = inputs.verticalInput;
           // vertical.ClientId = client.Id;
            if (!ModelState.IsValid)
            {
                return View("NewVerticalSubVerticalsGrid", inputs);
            }
            else
            {


                if (inputs.VerticalId > 0)
                {

                    vertical.Id = inputs.VerticalId;

                }


                if (vertical.Id == 0)
                {
                  //  entityVerticals.ClientId=client.Id;
                    var newvertical = verticalsService.Create(verticalsMapper.MapToEntity(vertical, entityVerticals));
                    vertical.Id = Convert.ToInt32(newvertical);
                }
                else
                {

                    verticalsMapper.MapToEntity(vertical, verticalsService.Get(vertical.Id));
                    verticalsService.Save();
                }


            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayVerticalSubVerticals", new { VerticalId = vertical.Id, VerticalName = vertical.VerticalName, tabs = "Tab2" });
        }

        ////public ActionResult DelVertical(int id)
        ////{
        ////    var e = verticalsService.Get(id);
        ////    var listIds = subVerticalsService.Where(x => x.VerticalId == e.Id).ToList();
        ////    if (listIds.Count > 0)
        ////    {
        ////        foreach (var subVerticals in listIds)
        ////        {
        ////            var listLevel1 = businessCapabilitiesLevel1Service.Where(x => x.SubVerticalId == subVerticals.Id).ToList();
        ////            if (listLevel1.Count > 0)
        ////            {
        ////                foreach (var level1Ids in listLevel1)
        ////                {
        ////                    var listLevel2 = businessCapabilitiesLevel2Service.Where(x => x.Level1Id == level1Ids.Id).ToList();
        ////                    if (listLevel2.Count > 0)
        ////                    {
        ////                        foreach (var level2Ids in listLevel2)
        ////                        {
        ////                            var listLevel3 = businessCapabilitiesLevel3Service.Where(x => x.Level2Id == level2Ids.Id).ToList();
        ////                            if (listLevel3.Count > 0)
        ////                            {
        ////                                foreach (var level3Ids in listLevel3)
        ////                                {
        ////                                    businessCapabilitiesLevel3Service.Delete(level3Ids.Id);
        ////                                }
        ////                            }
        ////                            businessCapabilitiesLevel2Service.Delete(level2Ids.Id);
        ////                        }
        ////                    }
        ////                    businessCapabilitiesLevel1Service.Delete(level1Ids.Id);

        ////                }
        ////            }
        ////            subVerticalsService.Delete(subVerticals.Id);
        ////        }
        ////    }
        ////    verticalsService.Delete(id);
        ////    VerticalAndSubVerticalInput verticalSubVerticals = new VerticalAndSubVerticalInput();
        ////    verticalSubVerticals.VerticalId = 0;
        ////    verticalSubVerticals.SubVerticalId = 0;
        ////    verticalSubVerticals.VerticalName = "";
        ////    verticalSubVerticals.VerticalCreate = "";
        ////    verticalSubVerticals.SubVerticalCreate = "";
        ////    verticalSubVerticals.tabs = "Tab2";
        ////    verticalSubVerticals.subVId = 0;
        ////    verticalSubVerticals.title = "";
        ////    //TempData["Success"] = "Data has been deleted successfully";
        ////    return View("NewVerticalSubVerticalsGrid", verticalSubVerticals);
        ////}

        ////public ActionResult DelSubVertical(int id)
        ////{
        ////    VerticalAndSubVerticalInput verticalSubVerticals = new VerticalAndSubVerticalInput();
        ////    var e = subVerticalsService.Get(id);
        ////    verticalSubVerticals.VerticalId = e.VerticalId;
        ////    var listLevel1 = businessCapabilitiesLevel1Service.Where(x => x.SubVerticalId == id).ToList();
        ////    if (listLevel1.Count > 0)
        ////    {
        ////        foreach (var level1Ids in listLevel1)
        ////        {
        ////            var listLevel2 = businessCapabilitiesLevel2Service.Where(x => x.Level1Id == level1Ids.Id).ToList();
        ////            if (listLevel2.Count > 0)
        ////            {
        ////                foreach (var level2Ids in listLevel2)
        ////                {
        ////                    var listLevel3 = businessCapabilitiesLevel3Service.Where(x => x.Level2Id == level2Ids.Id).ToList();
        ////                    if (listLevel3.Count > 0)
        ////                    {
        ////                        foreach (var level3Ids in listLevel3)
        ////                        {
        ////                            businessCapabilitiesLevel3Service.Delete(level3Ids.Id);
        ////                        }
        ////                    }
        ////                    businessCapabilitiesLevel2Service.Delete(level2Ids.Id);
        ////                }
        ////            }
        ////            businessCapabilitiesLevel1Service.Delete(level1Ids.Id);

        ////        }
        ////    }

        ////    subVerticalsService.Delete(id);

        ////    var v = verticalsService.Where(x => x.Id == e.VerticalId).FirstOrDefault();
        ////    verticalSubVerticals.SubVerticalId = 0;
        ////    verticalSubVerticals.VerticalName = v.VerticalName;
        ////    verticalSubVerticals.VerticalCreate = "";
        ////    verticalSubVerticals.SubVerticalCreate = "";
        ////    verticalSubVerticals.tabs = "Tab2";
        ////    verticalSubVerticals.subVId = 0;
        ////    verticalSubVerticals.title = "";
        ////    //TempData["Success"] = "Data has been deleted successfully";
        ////    return View("NewVerticalSubVerticalsGrid", verticalSubVerticals);
        ////}

        public ActionResult VerticalAndSubVertical_New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetIndVerticalList([DataSourceRequest] DataSourceRequest request)
        {
            Db db = new Db();
            ////IEnumerable<IndVerticalModel> vlist = (from a in db.IndustryVerticals
            ////                                       where a.IsDeleted==false
            ////                                       select new IndVerticalModel{ 
            ////                                            VerticalId=a.Id,
            ////                                            Description=a.Description,
            ////                                            VerticalName=a.VerticalName
            ////                                        }).ToList();
            IEnumerable<IndVerticalModel> vlist = (from a in db.IndustryVerticals
                                                      select new IndVerticalModel
                                                   {
                                                       VerticalId = a.Id,
                                                       Description = a.Description,
                                                       VerticalName = a.VerticalName
                                                   }).ToList();
            return Json(vlist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetSubVerticalList([DataSourceRequest] DataSourceRequest request, int VerticalId = 0)
        {
            IEnumerable<IndSubVerticalModel> klist = new List<IndSubVerticalModel>();
            Db db = new Db();
            if (VerticalId != 0)
            {
               // klist = (from a in db.IndustrySubVerticals
                     //    where a.IsDeleted == false && a.VerticalId==VerticalId
                klist = (from a in db.IndustrySubVerticals
                                   where  a.VerticalId == VerticalId
                                   select new IndSubVerticalModel
                         {
                             VerticalId = a.VerticalId,
                             SubVerticalId = a.Id,
                             SubVerticalName = a.SubVerticalName,
                             Description = a.Description
                         }).ToList();
            }
            return Json(klist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateIndVertical(IndVerticalModel model)
        {
            Db db = new Db();
            if (ModelState.IsValid)
            {
                var newAccSetup = CreateNewIndVertical( model);
            }
            return Json(new[] { model }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }
        [HttpPost]
        public ActionResult EditIndVertical([DataSourceRequest] DataSourceRequest request, IndVerticalModel model)
        {
            Db db = new Db();

            if (ModelState.IsValid)
            {
                var res = UpdateIndVertical( model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult DeleteIndVertical(int VerticalId)
        {
            Db db = new Db();
            var res = DeleteIndVerticals(VerticalId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateSubVertical([DataSourceRequest] DataSourceRequest request, IndSubVerticalModel model, int VerticalId = 0)
        {
            IndSubVerticalModel contdet = null;
            Db db = new Db();
            if (ModelState.IsValid)
            {
                if (VerticalId != 0)
                {
                    model.VerticalId = VerticalId;
                    var newContSetup = CreateSubVertical(model);
                    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
                }
            }
            return Json(new[] { contdet }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult EditSubVertical([DataSourceRequest] DataSourceRequest request, IndSubVerticalModel model)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();

            if (ModelState.IsValid)
            {
                var updateAcc = UpdateSubVertical(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult DeleteSubVertical(int SubVerticalId)
        {
            Db db = new Db();
            var res = DeleteSubVerticals(SubVerticalId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public static int CreateNewIndVertical( IndVerticalModel model)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    Verticals cv = new Verticals();
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    cv.VerticalName = model.VerticalName;
                    cv.Description = model.Description;
                   
                    unitOfWork.Verticals.Insert(cv);
                    unitOfWork.Save();
                    id = cv.Id;
                }
                return id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateNewIndVertical ", ex);
                throw ex;
            }
        }
        public static bool UpdateIndVertical(IndVerticalModel model)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cv = unitOfWork.Verticals.GetByID(model.VerticalId);
                    cv.VerticalName = model.VerticalName;
                    cv.Description = model.Description;
                    
                    unitOfWork.Verticals.Update(cv);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in UpdateIndVertical ", ex);
                throw ex;
            }
        }
        public static bool DeleteIndVerticals(int VerticalId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var subVertId = unitOfWork.SubVerticals.Get(g => g.VerticalId == VerticalId);
                foreach (var i in subVertId)
                {
                    unitOfWork.SubVerticals.Delete(i.Id);
                }

                unitOfWork.Verticals.Delete(VerticalId);
                unitOfWork.Save();
                
                return true;
            }
        }
        public static int CreateSubVertical(IndSubVerticalModel model)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    SubVerticals cv = new SubVerticals();
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    cv.VerticalId = model.VerticalId;
                    cv.SubVerticalName = model.SubVerticalName;
                    cv.Description = model.Description;
                    
                    unitOfWork.SubVerticals.Insert(cv);
                    unitOfWork.Save();
                    id = cv.Id;
                }
                return id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateSubVertical ", ex);
                throw ex;
            }
        }
        public static bool UpdateSubVertical(IndSubVerticalModel model)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cv = unitOfWork.SubVerticals.GetByID(model.SubVerticalId);
                    cv.SubVerticalName = model.SubVerticalName;
                    cv.Description = model.Description;
                    
                    unitOfWork.SubVerticals.Update(cv);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in UpdateSubVertical ", ex);
                throw ex;
            }
        }
        public static bool DeleteSubVerticals(int SubVerticalId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.SubVerticals.Delete(SubVerticalId);
                unitOfWork.Save();
                return true;
            }
        }
        [CustomAuthorize(new string[] { "superuser" }, new string[] { "Business Alignment" }, null)]
        [HttpPost]
        public JsonResult GetAllTemplates([DataSourceRequest] DataSourceRequest request, int SubVerticalId)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            var result = GetAll(SubVerticalId).ToTreeDataSourceResult(request,
                e => e.BCId,
                e => e.BCParentId,

                e => e
            );

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Helper Method to Get all templates for a Client_Id       
        private static IEnumerable<BusinessCapabilitiesModel> GetAll(int subverticalId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templatesList = unitOfWork.BAMBusinessCapabilities.Get(g => g.SubVerticalId == subverticalId);
            var templatesList_1 = templatesList.Select(c => new BusinessCapabilitiesModel
            {
                BCId = c.Id,
                BusinessCapName = c.BusinessCapName,

                BCParentId = c.ParentId,
                CreatedBy = c.CreatedBy,
                CreatedDate = c.CreatedDate,
                SubVerticalId = subverticalId,
                InternalExternal = c.InternalExternal
            });
            return templatesList_1;
        }
        [HttpPost]
        public JsonResult CreateBcc([DataSourceRequest] DataSourceRequest request, BusinessCapabilitiesModel template, int SubVertId = 0)
        {
            var uid = (int)Session["UserId"];
            //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                //template.ClientId = client.Id;
                template.CreatedBy = uid;
                template.SubVerticalId = SubVertId;
                //template.UpdatedBy = uid;
                var something = Capability_Insert(template);
                template.BCId = something;
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Create New Template/Task
        public static int Capability_Insert(BusinessCapabilitiesModel template)
        {
            var NewId = 0;
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = new BAMBusinessCapabilities();
                cv.BusinessCapName = template.BusinessCapName;
                //cv.ClientId = template.ClientId;
                cv.ParentId = template.BCParentId;
                cv.CreatedBy = template.CreatedBy;
                //cv.UpdatedBy = template.UpdatedBy;
                cv.CreatedDate = DateTime.Now;
                cv.SubVerticalId = template.SubVerticalId;
                cv.InternalExternal = "Internal";
                //  cv.UpdatedDt = DateTime.Now;
                unitOfWork.BAMBusinessCapabilities.Insert(cv);
                unitOfWork.Save();
                NewId = cv.Id;
            }
            return NewId;
        }
        [HttpPost]
        public JsonResult UpdateBcc([DataSourceRequest] DataSourceRequest request, BusinessCapabilitiesModel template)
        {
            //var uid = (int)Session["UserId"];
            //template.UpdatedBy = uid;
            if (ModelState.IsValid)
            {
                Capability_Update(template);
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Create New Template/Task
        public static void Capability_Update(BusinessCapabilitiesModel template)
        {
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = unitOfWork.BAMBusinessCapabilities.GetByID(template.BCId);
                cv.BusinessCapName = template.BusinessCapName;
                //cv.UpdatedBy = template.UpdatedBy;
                //cv.UpdatedDt = DateTime.Now;
                unitOfWork.BAMBusinessCapabilities.Update(cv);
                unitOfWork.Save();
            }
        }
        public ActionResult DestroyBCC([DataSourceRequest] DataSourceRequest request, BusinessCapabilitiesModel template)
        {
            deleteChildBCs(template.BCId);
            return Json(new[] { template }.ToDataSourceResult(request, ModelState));
        }

        private static void deleteChildBCs(int SubId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            var allCapabilities = unitOfWork.BAMBusinessCapabilities.GetByID(SubId);
            var childCapabilities = unitOfWork.BAMBusinessCapabilities.Get(t => t.ParentId == SubId).ToList();

            if (childCapabilities.Count() != 0)
            {
                foreach (var childTask in childCapabilities)
                {
                    deleteChildren(childTask.Id);
                }
            }
            unitOfWork.BAMBusinessCapabilities.Delete(allCapabilities.Id);
            unitOfWork.Save();
        }
        private static void deleteChildren(int SubId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var bcapps = unitOfWork.BCApplicationJunction.Get(g => g.BCId == SubId);
            foreach (var bcapp in bcapps)
            {
                try
                {
                    unitOfWork.BCApplicationJunction.Delete(bcapp.Id);
                    unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    Log.Error("Error in deleteChildren ", ex);
                    throw ex;
                }
            }
            var allCapabilities = unitOfWork.BAMClientBusinessCapabilities.GetByID(SubId);
            var bccurrentlist1 = unitOfWork.BAMClientBusinessCapabilities.Get(g => g.OrderId > allCapabilities.OrderId && g.ClientId == allCapabilities.ClientId && g.ParentId == allCapabilities.ParentId).ToList();
            if (bccurrentlist1.Count() != 0)
            {
                foreach (var item in bccurrentlist1)
                {
                    item.OrderId = item.OrderId - 1;
                    unitOfWork.BAMClientBusinessCapabilities.Update(item);
                    unitOfWork.Save();
                }
            }
            var childCapabilities = unitOfWork.BAMClientBusinessCapabilities.Get(t => t.ParentId == SubId).ToList();

            if (childCapabilities.Count() != 0)
            {
                foreach (var childTask in childCapabilities)
                {
                    var bcchildapps = unitOfWork.BCApplicationJunction.Get(g => g.BCId == childTask.Id);
                    foreach (var bccapp in bcchildapps)
                    {
                        try
                        {
                            unitOfWork.BCApplicationJunction.Delete(bccapp.Id);
                        }
                        catch(Exception ex)
                        {
                            Log.Error("Error in deleteChildren ", ex);
                            throw ex;
                        }
                    }
                    unitOfWork.Save();
                    deleteChildren(childTask.Id);
                }
            }
            unitOfWork.BAMClientBusinessCapabilities.Delete(allCapabilities.Id);
            unitOfWork.Save();
        }

    }
}
