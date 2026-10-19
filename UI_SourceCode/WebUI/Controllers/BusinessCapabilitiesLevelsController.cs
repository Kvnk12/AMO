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

namespace NTTData.AMOPortal.WebUI.Controllers
{
    ////public class BusinessCapabilitiesLevelsController : Cruder<BusinessCapabilitiesLevels, BusinessCapabilitiesLevelsInput>
    ////{

    ////    // GET: /BusinessCapabilitiesLevels/
    ////    //Entities
    ////    //private BusinessCapabilitiesLevels entityBusinessCapabilitiesLevel;

    ////    //// Service
    ////    //private CrudService<BusinessCapabilitiesLevels> businessCapabilitiesLevelService;

    ////    ////Mappers
    ////    //private Mapper<BusinessCapabilitiesLevels, BusinessCapabilitiesLevelsInput> businessCapabilitiesLevelsMapper;

    ////    ////Repo
    ////    //private Repo<BusinessCapabilitiesLevels> repoBusinessCapabilitiesLevels;

    ////    //// <summary>
    ////    //// BusinessCapabilitiesLevel1 mapper
    ////    //// </summary>
    ////    //// <param name="service"></param>
    ////    //// <param name="v"></param>


    ////    ////Entities
    ////    //private BusinessCapabilitiesLevel1 entityBusinessCapabilitiesLevel1;

    ////    ////Service

    ////    //private CrudService<BusinessCapabilitiesLevel1> businessCapabilitiesLevel1Service;

    ////    ////Mappers

    ////    //private Mapper<BusinessCapabilitiesLevel1, BusinessCapabilitiesLevel1Input> businessCapabilitiesLevel1Mapper;

    ////    ////Repo

    ////    //private Repo<BusinessCapabilitiesLevel1> repoBusinessCapabilitiesLevel1;

    ////    // <summary>
    ////    // BusinessCapabilitiesLevel2 mapper
    ////    // </summary>
    ////    // <param name="service"></param>
    ////    // <param name="v"></param>


    ////    //////Entities
    ////    ////private BusinessCapabilitiesLevel2 entityBusinessCapabilitiesLevel2;

    ////    //////Service

    ////    ////private CrudService<BusinessCapabilitiesLevel2> businessCapabilitiesLevel2Service;

    ////    //////Mappers

    ////    ////private Mapper<BusinessCapabilitiesLevel2, BusinessCapabilitiesLevel2Input> businessCapabilitiesLevel2Mapper;

    ////    //////Repo

    ////    ////private Repo<BusinessCapabilitiesLevel2> repoBusinessCapabilitiesLevel2;

    ////    // <summary>
    ////    // BusinessCapabilitiesLevel3 mapper
    ////    // </summary>
    ////    // <param name="service"></param>
    ////    // <param name="v"></param>


    ////    //Entities
    ////    ////private BusinessCapabilitiesLevel3 entityBusinessCapabilitiesLevel3;

    ////    //////Service

    ////    ////private CrudService<BusinessCapabilitiesLevel3> businessCapabilitiesLevel3Service;

    ////    //////Mappers

    ////    ////private Mapper<BusinessCapabilitiesLevel3, BusinessCapabilitiesLevel3Input> businessCapabilitiesLevel3Mapper;

    ////    //////Repo

    ////    ////private Repo<BusinessCapabilitiesLevel3> repoBusinessCapabilitiesLevel3;

    ////    ////public BusinessCapabilitiesLevelsController(ICrudService<BusinessCapabilitiesLevels> service, IMapper<BusinessCapabilitiesLevels, BusinessCapabilitiesLevelsInput> v)
    ////    ////    : base(service, v)
    ////    ////{


    ////    ////    DbContextFactory Dbcon = new DbContextFactory();
    ////    ////    repoBusinessCapabilitiesLevels = new Repo<BusinessCapabilitiesLevels>(Dbcon);
    ////    ////    businessCapabilitiesLevelsMapper = new Mapper<BusinessCapabilitiesLevels, BusinessCapabilitiesLevelsInput>();

    ////    ////    entityBusinessCapabilitiesLevel = new BusinessCapabilitiesLevels();

    ////    ////    businessCapabilitiesLevelService = new CrudService<BusinessCapabilitiesLevels>(repoBusinessCapabilitiesLevels);

    ////    ////    //    Setup BusinessCapabilitiesLevel1 Service

    ////    ////    //    DbContextFactory Dbcon = new DbContextFactory();


    ////    ////    repoBusinessCapabilitiesLevel1 = new Repo<BusinessCapabilitiesLevel1>(Dbcon);
    ////    ////    businessCapabilitiesLevel1Mapper = new Mapper<BusinessCapabilitiesLevel1, BusinessCapabilitiesLevel1Input>();
    ////    ////    entityBusinessCapabilitiesLevel1 = new BusinessCapabilitiesLevel1();
    ////    ////    businessCapabilitiesLevel1Service = new CrudService<BusinessCapabilitiesLevel1>(repoBusinessCapabilitiesLevel1);

    ////    ////    //    setup BusinessCapabilitiesLevel2 Service


    ////    ////    repoBusinessCapabilitiesLevel2 = new Repo<BusinessCapabilitiesLevel2>(Dbcon);
    ////    ////    businessCapabilitiesLevel2Mapper = new Mapper<BusinessCapabilitiesLevel2, BusinessCapabilitiesLevel2Input>();
    ////    ////    entityBusinessCapabilitiesLevel2 = new BusinessCapabilitiesLevel2();
    ////    ////    businessCapabilitiesLevel2Service = new CrudService<BusinessCapabilitiesLevel2>(repoBusinessCapabilitiesLevel2);

    ////    ////    //    setup BusinessCapabilitiesLevel3 Service


    ////    ////    repoBusinessCapabilitiesLevel3 = new Repo<BusinessCapabilitiesLevel3>(Dbcon);
    ////    ////    businessCapabilitiesLevel3Mapper = new Mapper<BusinessCapabilitiesLevel3, BusinessCapabilitiesLevel3Input>();
    ////    ////    entityBusinessCapabilitiesLevel3 = new BusinessCapabilitiesLevel3();
    ////    ////    businessCapabilitiesLevel3Service = new CrudService<BusinessCapabilitiesLevel3>(repoBusinessCapabilitiesLevel3);

    ////    ////}

    ////    ////[HttpGet]
    ////    ////public ActionResult DisplayBusinessCapabilitiesLevels(int Level1Id = 0, int Level2Id = 0, int Level3Id = 0, string Level1Name = "", string Level2Name = "", string Level1Create = "", string Level2Create = "", string Level3Create = "", string InExValue = "", int subVId = 0, string title = "")
    ////    ////{
    ////    ////    BusinessCapabilitiesLevelsInput businessCapabilitiesLevel = new BusinessCapabilitiesLevelsInput();
    ////    ////    businessCapabilitiesLevel.Level1Id = Level1Id;
    ////    ////    businessCapabilitiesLevel.Level2Id = Level2Id;
    ////    ////    businessCapabilitiesLevel.Level3Id = Level3Id;
    ////    ////    businessCapabilitiesLevel.Level1Name = Server.HtmlEncode(Level1Name);
    ////    ////    businessCapabilitiesLevel.Level2Name = Server.HtmlEncode(Level2Name);
    ////    ////    businessCapabilitiesLevel.Level1Create = Level1Create;
    ////    ////    businessCapabilitiesLevel.Level2Create = Level2Create;
    ////    ////    businessCapabilitiesLevel.Level3Create = Level3Create;
    ////    ////    businessCapabilitiesLevel.InExValue = InExValue;
    ////    ////    businessCapabilitiesLevel.subVId = subVId;
    ////    ////    businessCapabilitiesLevel.title = title;

    ////    ////    if (Level1Id > 0)
    ////    ////    {
    ////    ////        businessCapabilitiesLevel.businessCapabilitiesLevel1Input = businessCapabilitiesLevel1Mapper.MapToInput(businessCapabilitiesLevel1Service.Get(Level1Id));

    ////    ////    }


    ////    ////    if (Level1Id > 0 && Level2Id > 0 && Level3Id == 0)
    ////    ////    {
    ////    ////        businessCapabilitiesLevel.businessCapabilitiesLevel2Input = businessCapabilitiesLevel2Mapper.MapToInput(businessCapabilitiesLevel2Service.Get(Level2Id));

    ////    ////    }

    ////    ////    if (Level1Id > 0 && Level2Id > 0 && Level3Id > 0)
    ////    ////    {
    ////    ////        businessCapabilitiesLevel.businessCapabilitiesLevel3Input = businessCapabilitiesLevel3Mapper.MapToInput(businessCapabilitiesLevel3Service.Get(Level3Id));

    ////    ////    }

    ////    ////    if (Level1Id == 0 && businessCapabilitiesLevel.Level1Create == "Level1Create")
    ////    ////    {
    ////    ////        businessCapabilitiesLevel.businessCapabilitiesLevel1Input = businessCapabilitiesLevel1Mapper.MapToInput(entityBusinessCapabilitiesLevel1);
    ////    ////    }

    ////    ////    if (Level1Id == 0 && businessCapabilitiesLevel.Level1Create == "Level1Create")
    ////    ////    {
    ////    ////        businessCapabilitiesLevel.businessCapabilitiesLevel1Input = businessCapabilitiesLevel1Mapper.MapToInput(entityBusinessCapabilitiesLevel1);


    ////    ////    }

    ////    ////    if (Level1Id > 0 && Level2Create == "Level2Create")
    ////    ////    {

    ////    ////        businessCapabilitiesLevel.businessCapabilitiesLevel2Input = businessCapabilitiesLevel2Mapper.MapToInput(entityBusinessCapabilitiesLevel2);

    ////    ////    }


    ////    ////    if (Level1Id > 0 && Level2Id > 0 && Level3Create == "Level3Create")
    ////    ////    {

    ////    ////        businessCapabilitiesLevel.businessCapabilitiesLevel3Input = businessCapabilitiesLevel3Mapper.MapToInput(entityBusinessCapabilitiesLevel3);

    ////    ////    }
    ////    ////    if (TempData["Success"] != null)
    ////    ////    {
    ////    ////        ModelState.AddModelError("Success", "Data has been saved successfully");
    ////    ////    }
    ////    ////    return View("NewBusinessCapabilitiesLevels", businessCapabilitiesLevel);

    ////    ////}

    ////    [HttpPost]
    ////    public ActionResult DisplayBusinessCapabilitiesLevels(BusinessCapabilitiesLevelsInput inputs, int Level1Id = 0, int Level2Id = 0, int Level3Id = 0, string Level1Name = "", string Level2Name = "", string Level1Create = "", string Level2Create = "", string Level3Create = "", string InExValue = "", int subVId = 0, string title = "")
    ////    {
    ////        //ViewBag.StatusMessage = "";

    ////        inputs.Level1Id = Level1Id;
    ////        inputs.Level2Id = Level2Id;
    ////        inputs.Level3Id = Level3Id;
    ////        inputs.Level1Name = Server.HtmlEncode(Level1Name);
    ////        inputs.Level2Name = Server.HtmlEncode(Level2Name);
    ////        inputs.Level1Create = Level1Create;
    ////        inputs.Level2Create = Level2Create;
    ////        inputs.Level3Create = Level3Create;
    ////        inputs.InExValue = InExValue;
    ////        inputs.subVId = subVId;
    ////        inputs.title = title;


    ////        //Go To action account details saving
    ////        if ((inputs.Level2Id == 0 && Level2Create == "" && inputs.Level3Id == 0 && Level3Create == ""))
    ////        {
    ////            return (SaveLevel1Details(inputs));
    ////        }

    ////        if ((inputs.Level1Id > 0 && inputs.Level2Id > 0 && (inputs.Level3Id > 0 || Level3Create == "Level3Create")))
    ////        {
    ////            return (SaveLevel3Details(inputs));
    ////        }

    ////        var input = inputs.businessCapabilitiesLevel2Input;
    ////        input.Level1Id = inputs.Level1Id;


    ////        if (!ModelState.IsValid)
    ////        {
    ////            return View("NewBusinessCapabilitiesLevels", inputs);
    ////        }


    ////        //Edit Existing Contract
    ////        if (inputs.Level2Id != 0)
    ////        {
    ////            input.Id = inputs.Level2Id;
    ////            using (Db db = new Db())
    ////            {
    ////                BusinessCapabilitiesLevel2 existingLevel2Name = db.BusinessCapabilitiesLevel2.FirstOrDefault(c => c.Level2.ToLower().Trim() == input.Level2.ToLower().Trim() && c.Level1Id == input.Level1Id);
    ////                if (existingLevel2Name != null && (existingLevel2Name.Id != input.Id))
    ////                {
    ////                    if (existingLevel2Name != null)
    ////                    {
    ////                        ModelState.AddModelError("Level2Name", "Level2 Name already Exists");
    ////                        return View("NewBusinessCapabilitiesLevels", inputs);
    ////                    }
    ////                }
    ////            }
    ////        }



    ////        if (input.Id > 0)
    ////        {

    ////            businessCapabilitiesLevel2Mapper.MapToEntity(input, businessCapabilitiesLevel2Service.Get(input.Id));
    ////            businessCapabilitiesLevel2Service.Save();

    ////        }
    ////        else
    ////        {
    ////            //Add New Contract 

    ////            using (Db db = new Db())
    ////            {
    ////                BusinessCapabilitiesLevel2 existingLevel2Name = db.BusinessCapabilitiesLevel2.FirstOrDefault(c => c.Level2.ToLower().Trim() == input.Level2.ToLower().Trim() && c.Level1Id == input.Level1Id);
    ////                if (existingLevel2Name != null && (existingLevel2Name.Id != input.Id))
    ////                {
    ////                    if (existingLevel2Name != null)
    ////                    {
    ////                        ModelState.AddModelError("Level2Name", "Level2 Name already Exists");
    ////                        return View("NewBusinessCapabilitiesLevels", inputs);
    ////                    }
    ////                }
    ////            }

    ////            var SLevel2Id = businessCapabilitiesLevel2Service.Create(businessCapabilitiesLevel2Mapper.MapToEntity(input, entityBusinessCapabilitiesLevel2));
    ////            input.Id = Convert.ToInt32(SLevel2Id);

    ////        }

    ////        TempData["Success"] = "Data has been saved successfully";
    ////        return RedirectToAction("DisplayBusinessCapabilitiesLevels", new { Level1Id = inputs.Level1Id, Level1Name = inputs.Level1Name, Level2Id = input.Id, Level2Name = input.Level2, InExValue = inputs.InExValue, subVId = inputs.subVId });

    ////    }



    ////    [HttpPost]
    ////    public ActionResult SaveLevel1Details(BusinessCapabilitiesLevelsInput inputs)
    ////    {

    ////        BusinessCapabilitiesLevel1Input bc1 = inputs.businessCapabilitiesLevel1Input;
    ////        bc1.SubVerticalId = inputs.subVId;

    ////        if (!ModelState.IsValid)
    ////        {
    ////            return View("NewBusinessCapabilitiesLevels", inputs);
    ////        }
    ////        else
    ////        {


    ////            if (inputs.Level1Id > 0)
    ////            {

    ////                bc1.Id = inputs.Level1Id;
    ////            }
    ////            using (Db db = new Db())
    ////            {
    ////                BusinessCapabilitiesLevel1 existingLevel1Name = db.BusinessCapabilitiesLevel1.FirstOrDefault(c => c.Level1.ToLower().Trim() == bc1.Level1.ToLower().Trim() && c.SubVerticalId == bc1.SubVerticalId);
    ////                if (existingLevel1Name != null && (existingLevel1Name.Id != bc1.Id))
    ////                {
    ////                    if (existingLevel1Name != null)
    ////                    {
    ////                        ModelState.AddModelError("Level1Name", "Level1 Name already Exists");
    ////                        return View("NewBusinessCapabilitiesLevels", inputs);
    ////                    }
    ////                }
    ////            }



    ////            if (bc1.Id == 0)
    ////            {
    ////                var newlevel1 = businessCapabilitiesLevel1Service.Create(businessCapabilitiesLevel1Mapper.MapToEntity(bc1, entityBusinessCapabilitiesLevel1));
    ////                bc1.Id = Convert.ToInt32(newlevel1);
    ////            }
    ////            else
    ////            {

    ////                businessCapabilitiesLevel1Mapper.MapToEntity(bc1, businessCapabilitiesLevel1Service.Get(bc1.Id));
    ////                businessCapabilitiesLevel1Service.Save();
    ////            }


    ////        }

    ////        TempData["Success"] = "Data has been saved successfully";
    ////        return RedirectToAction("DisplayBusinessCapabilitiesLevels", new { Level1Id = bc1.Id, Level1Name = bc1.Level1, InExValue = bc1.InternalExternal, subVId = bc1.SubVerticalId });
    ////    }

    ////    [HttpPost]
    ////    public ActionResult SaveLevel3Details(BusinessCapabilitiesLevelsInput inputs)
    ////    {
    ////        BusinessCapabilitiesLevel3Input bc3 = inputs.businessCapabilitiesLevel3Input;
    ////        bc3.Level2Id = inputs.Level2Id;
    ////        if (!ModelState.IsValid)
    ////        {
    ////            return View("NewBusinessCapabilitiesLevels", inputs);
    ////        }
    ////        else
    ////        {


    ////            if (inputs.Level3Id > 0)
    ////            {

    ////                bc3.Id = inputs.Level3Id;
    ////            }

    ////            using (Db db = new Db())
    ////            {
    ////                BusinessCapabilitiesLevel3 existingLevel3Name = db.BusinessCapabilitiesLevel3.FirstOrDefault(c => c.Level3.ToLower().Trim() == bc3.Level3.ToLower().Trim() && c.Level2Id == bc3.Level2Id);
    ////                if (existingLevel3Name != null && (existingLevel3Name.Id != bc3.Id))
    ////                {
    ////                    if (existingLevel3Name != null)
    ////                    {
    ////                        ModelState.AddModelError("Level3Name", "Level3 Name already Exists");
    ////                        return View("NewBusinessCapabilitiesLevels", inputs);
    ////                    }
    ////                }
    ////            }

    ////            if (bc3.Id == 0)
    ////            {
    ////                var newlevel3 = businessCapabilitiesLevel3Service.Create(businessCapabilitiesLevel3Mapper.MapToEntity(bc3, entityBusinessCapabilitiesLevel3));
    ////                bc3.Id = Convert.ToInt32(newlevel3);
    ////            }
    ////            else
    ////            {

    ////                businessCapabilitiesLevel3Mapper.MapToEntity(bc3, businessCapabilitiesLevel3Service.Get(bc3.Id));
    ////                businessCapabilitiesLevel3Service.Save();
    ////            }


    ////        }

    ////        TempData["Success"] = "Data has been saved successfully";
    ////        return RedirectToAction("DisplayBusinessCapabilitiesLevels", new { Level3Id = bc3.Id, Level3Name = bc3.Level3, Level1Id = inputs.Level1Id, Level1Name = inputs.Level1Name, Level2Id = bc3.Level2Id, Level2Name = inputs.Level2Name, InExValue = inputs.InExValue, subVId = inputs.subVId });
    ////    }

    ////    public ActionResult DelLevel1(BusinessCapabilitiesLevelsInput input)
    ////    {
    ////        var l1 = businessCapabilitiesLevel1Service.Get(input.Level1Id);
    ////        if (l1 != null)
    ////        {
    ////            int id = input.Level1Id;
    ////            var listLevel2 = businessCapabilitiesLevel2Service.Where(x => x.Level1Id == id).ToList();
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


    ////            businessCapabilitiesLevel1Service.Delete(id);
    ////        }
    ////        input.Level1Id = 0;
    ////        input.Level1Name = "";
    ////        input.Level2Name = "";
    ////        return View("NewBusinessCapabilitiesLevels", input);
    ////    }
    ////    public ActionResult DelLevel2(BusinessCapabilitiesLevelsInput input)
    ////    {
    ////        var l2 = businessCapabilitiesLevel2Service.Get(input.Level2Id);
    ////        if (l2 != null)
    ////        {
    ////            int id = input.Level2Id;

    ////            var listIds = businessCapabilitiesLevel3Service.Where(x => x.Level2Id == id).ToList();
    ////            if (listIds.Count > 0)
    ////            {
    ////                foreach (var level3list in listIds)
    ////                {
    ////                    businessCapabilitiesLevel3Service.Delete(level3list.Id);
    ////                }
    ////            }
    ////            businessCapabilitiesLevel2Service.Delete(id);
    ////        }
    ////        input.Level2Id = 0;
    ////        input.Level2Name = "";

    ////        return View("NewBusinessCapabilitiesLevels", input);
    ////    }
    ////    public ActionResult DelLevel3(BusinessCapabilitiesLevelsInput input)
    ////    {
    ////        var l3 = businessCapabilitiesLevel3Service.Get(input.Level3Id);
    ////        if (l3 != null)
    ////        {
    ////            int id = input.Level3Id;

    ////            businessCapabilitiesLevel3Service.Delete(id);
    ////        }
    ////        input.Level3Id = 0;
    ////        input.Level3Name = "";

    ////        return View("NewBusinessCapabilitiesLevels", input);
    ////    }
    ////}
}
