using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebUI.Areas.ClientProfileManager.Dto;
using WebUI.Areas.ClientProfileManager;
using WebUI.Dto;
using System.IO;
using System.Drawing;
using System.Data;
using Data;
using Domain.Entities;
using WebUI.Models;

namespace NTTData.AMOPortal.WebUI.Controllers
{

    public class ClientController : ClientCruder<Client, ClientInput, Domain.Entities.Address, Dto.Address, Domain.Entities.Engagements, Dto.Engagements>
    {

        public ClientController(ICrudService<Client> service, IMapper<Client, ClientInput> v,
            ICrudService<Domain.Entities.Address> Addressservice, IMapper<Domain.Entities.Address, Dto.Address> AddressMap,
            ICrudService<Domain.Entities.Engagements> EngagementService, IMapper<Domain.Entities.Engagements, Dto.Engagements> EngagementMap)
            : base(service, v, Addressservice, AddressMap, EngagementService, EngagementMap)
        {
            //
        }

        public override ActionResult Create()
        {
            var input = new ClientInput();
            int loginUser = (int)Session["UserId"];
            input.LoginUser = loginUser;
            input.PrimaryAddress = input.PrimaryAddress == null ? new Dto.Address() : input.PrimaryAddress;
            // input.Engagement = input.Engagement == null ? new Dto.Engagements() : input.Engagement;
            //input.cl
            // added by neeraj starts for cascading dropdown
            ViewData["country"] = GetDetails(1, 0, 0);
            ViewData["city"] = GetDetails(2, 0, 0);
            ViewData["subVertical"] = GetVerticalDetails(0, 0);
            // added by neeraj ends
            
            Db db = new Db();
            var userList = (from a in db.UserProfiles
                                               join b in db.UserApplications on a.Id equals b.UserId
                                               where b.ClientId == 0
                                               select a).Distinct().ToList();
            var userEmailList = userList.Select(UList => new SelectListItem
            {
                Value = UList.Id.ToString(),
                Text = UList.Email.ToString()
            });
            ViewData["EmailList"] = userEmailList;
            ViewBag.EmailList = userEmailList;
            return View(input);
        }

        [HttpPost]
        public override ActionResult Create(ClientInput input)
        {
            ViewData["country"] = GetDetails(1, 0, 0);
            ViewData["city"] = GetDetails(2, 0, 0);
            ViewData["subVertical"] = GetVerticalDetails(0, 0);
            var client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            Db db1 = new Db();
            var userRole = (from a in db1.UserProfiles
                            where a.Id == uid
                            select a.RoleId).FirstOrDefault();
            //var user = GetDropDownListItems.UserRoleForClient(uid, client.ClientId);
            ViewBag.EmailList = "";
            if (userRole.ToLower() == "superuser")
            { 
               
            if (!ModelState.IsValid)
            {
                
                return View(input);
            }
            using (Db db = new Db())
            {
                //ModelState.AddModelError("CreateMsg", "Client Created Successfully");
                ViewData["CreateMsg"] = "Client Created Successfully";


                var userList = (from a in db.UserProfiles
                                join b in db.UserApplications on a.Id equals b.UserId
                                where b.ClientId == 0
                                select a).Distinct().ToList();
                var userEmailList = userList.Select(UList => new SelectListItem
                {
                    Value = UList.Id.ToString(),
                    Text = UList.Email.ToString()
                });
                ViewData["EmailList"] = userEmailList;
                ViewBag.EmailList = userEmailList;

                Client ExisitingClients = db.Clients.FirstOrDefault(c => c.ClientName.ToLower() == input.ClientName.ToLower());
                if (ExisitingClients == null)
                {
                    //try end
                    var entity = new Client();
                    var e = createMapper.MapToEntity(input, entity);

                    var InputAddress = input.PrimaryAddress;
                    var EntityAddress = new Domain.Entities.Address();
                    var e1 = AddresscreateMapper.MapToEntity(InputAddress, EntityAddress);
                    int AddressId = Addressservice.Create(e1);
                    e.PrimaryAddressId = AddressId;

                    // var InputEngagement = input.Engagement;
                    //  var EntityEngagement = new Domain.Entities.Engagements();
                    //   var e2 = EngagementcreateMapper.MapToEntity(InputEngagement, EntityEngagement);
                    //   int EngagementId = Engagementservice.Create(e2);
                    //  e.EngagementId = EngagementId;

                    if (EntityAddress.RegionId != 0)
                    {
                        ViewData["country"] = GetDetails(1, 1, EntityAddress.RegionId);
                    }
                    else
                    {
                        ViewData["country"] = GetDetails(1, 0, 0);
                    }

                    if (EntityAddress.CountryId != 0)
                    {
                        ViewData["city"] = GetDetails(2, 1, EntityAddress.CountryId);
                    }
                    else
                    {
                        ViewData["city"] = GetDetails(1, 0, 0);
                    }
                    if (input.VerticalId == 0)
                    {
                        ModelState.AddModelError("VerticalId", "Please select the Vertical ID");
                        return View(input);
                    }
                    else if (input.SubVerticalId == 0)
                    {
                        ModelState.AddModelError("SubVerticalId", "Please select the SubVertical ID");
                        return View(input);
                    }

                    if (input.VerticalId != 0)
                    {
                        ViewData["subVertical"] = GetVerticalDetails(1, input.VerticalId);
                    }
                    else
                    {
                        ViewData["subVertical"] = GetVerticalDetails(0, 0);
                    }

                    int newClientId = service.Create(e);
                    if (input.httpPostedTemplateFile != null)
                    {
                        var s = input.httpPostedTemplateFile.FileName;
                        var fname = s.Split('\\');
                        var ProfileName = fname[fname.Length - 1];
                        Stream filestream = input.httpPostedTemplateFile.InputStream;
                        BinaryReader br = new BinaryReader(filestream);
                        Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                        var ClientRow = ClientProfileManagerHelper.UpdateProfile(bytes, ProfileName, newClientId);
                        string imageBase64Data = Convert.ToBase64String(bytes);
                        string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                        ViewBag.Photo = imageDataURL;
                    }
                    if (input.httpPostedEmailTemplateFile != null)
                    {
                        var s = input.httpPostedEmailTemplateFile.FileName;
                        var efname = s.Split('\\');
                        var emailProfileName = efname[efname.Length - 1];
                        Stream emailfilestream = input.httpPostedEmailTemplateFile.InputStream;
                        BinaryReader br = new BinaryReader(emailfilestream);
                        Byte[] bytes = br.ReadBytes((Int32)emailfilestream.Length);
                        var ClientRow = ClientProfileManagerHelper.UpdateClientProfile(bytes, emailProfileName, newClientId);
                        string imageBase64Data = Convert.ToBase64String(bytes);
                        string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                        ViewBag.EmailPhoto = imageDataURL;
                    }

                    HomeInput menuInput = (HomeInput)Session["MenuItemsInput"];
                    if (menuInput.UserRole != "superuser")
                    {
                        var userApps = GetDropDownListItems.UserApplications((int)Session["UserId"]);
                        DbContextFactory dbcontext = new DbContextFactory();
                        Repo<UserApplications> repoUserApplications = new Repo<UserApplications>(dbcontext);
                        Service.CrudService<UserApplications> UserApplicationsservice = new Service.CrudService<UserApplications>(repoUserApplications);
                        // Client client = (Client)Session["Client"];
                       // ClientDetail client = (ClientDetail)Session["SessionClient"];
                        string useRoleId = GetDropDownListItems.UserRoleForClient((int)Session["UserId"], client.ClientId).Value;
                        foreach (var ua in userApps)
                        {
                            UserApplications newUa = new UserApplications();
                            newUa.ApplicationId = Convert.ToInt32(ua.Value);
                            newUa.ClientId = newClientId;
                            newUa.UserId = (int)Session["UserId"];
                            newUa.RoleId = Convert.ToInt32(useRoleId);
                            //assign user to applications/roles
                            var ID = UserApplicationsservice.Create(newUa);
                        }
                    }
                }
                else
                {
                    Client ExisitingInactive = db.Clients.Where(m => m.IsDeleted == false).FirstOrDefault(c => c.ClientName.ToLower() == input.ClientName.ToLower());
                    if (ExisitingInactive == null)
                    {
                        ModelState.AddModelError("ClientName", "Client name already exist but inactivated. Please contact system administrator.");
                    }
                    else
                    {
                        ModelState.AddModelError("ClientName", "Client name already exists.");
                    }
                    input.LoginUser = (int)Session["UserId"];
                    return View(input);
                }

                input.LoginUser = (int)Session["UserId"];

               
                return View(input);
            }
            }
            else
            {
                ViewData["ErrorMsg"] = "You are not authorised to Create new Client Please contact AMSSupport@nttdata.com";
                return View(EditView, input);
            }
        }
        public override ActionResult Edit(int id)
        {
            var entity = service.Get(id, "PrimaryAddress");
            Mapper<Client, ClientInput> ClientMapper = new Mapper<Client, ClientInput>();
            var input = ClientMapper.MapToInput(entity);
            Db db = new Db();
            var userList = (from a in db.UserProfiles
                            join b in db.UserApplications on a.Id equals b.UserId
                            where b.ClientId == id
                            select a).Distinct().ToList();
            var userEmailList = userList.Select(UList => new SelectListItem
            {
                Value = UList.Id.ToString(),
                Text = UList.Email.ToString()
            });

            // ViewData["EmailList"] = userEmailList;
            ViewBag.EmailList = userEmailList;
            
            if (input == null)
            {
                input = input == null ? new ClientInput() : input;
                input.LoginUser = (int)Session["UserId"];
                input.PrimaryAddress = input.PrimaryAddress == null ? new Dto.Address() : input.PrimaryAddress;
                //  input.Engagement = input.Engagement == null ? new Dto.Engagements() : input.Engagement;
            }
            else
            {
                input.PrimaryAddress = entity.PrimaryAddress == null ? new Dto.Address() : AddresseditMapper.MapToInput(entity.PrimaryAddress);
                //  input.Engagement = entity.Engagement == null ? new Dto.Engagements() : EngagementeditMapper.MapToInput(entity.Engagement);
            }
            input.LoginUser = (int)Session["UserId"];

            if (entity.PrimaryAddress.RegionId != 0)
            {
                ViewData["country"] = GetDetails(1, 1, entity.PrimaryAddress.RegionId);
            }
            else
            {
                ViewData["country"] = GetDetails(1, 0, 0);
            }

            if (entity.PrimaryAddress.CountryId != 0)
            {
                ViewData["city"] = GetDetails(2, 1, entity.PrimaryAddress.CountryId);
            }
            else
            {
                ViewData["city"] = GetDetails(1, 0, 0);
            }

            if (input.VerticalId == 0)
            {
                ModelState.AddModelError("VerticalId", "Please select the Vertical ID");
                return View(EditView, input);
            }
            else if (input.SubVerticalId == 0)
            {
                ModelState.AddModelError("SubVerticalId", "Please select the SubVertical ID");
                return View(EditView, input);
            }

            if (input.VerticalId != 0)
            {
                ViewData["subVertical"] = GetVerticalDetails(1, input.VerticalId);
            }
            else
            {
                ViewData["subVertical"] = GetVerticalDetails(0, 0);
            }
            var Img = ClientProfileManagerHelper.getProfileClientDetail(input.Id);
            if (Img.CPMFile != null)
            {
                string imageBase64Data = Convert.ToBase64String(Img.CPMFile);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                ViewBag.Photo = imageDataURL;
            }
            if (Img.EmailTemplateImageCPMFile != null)
            {
                string imageBase64Data = Convert.ToBase64String(Img.EmailTemplateImageCPMFile);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                ViewBag.EmailPhoto = imageDataURL;
            }
           
               
                               
            return View(EditView, input);
        }

        public override ActionResult Edit(ClientInput input)
        {
            if (input.ClientStatus == 0)
            {
                ViewData["ErrorMsg"] = "Please Select Client Status";
                return View(EditView, input);
            }
            ////if (!ModelState.IsValid)
            ////    return View(EditView, input);
            var contextFactory = new DbContextFactory();
            DataTable returntable = new DataTable();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();

            var userList = (from a in db.UserProfiles
                            join b in db.UserApplications on a.Id equals b.UserId
                            where b.ClientId == input.Id
                            select a).Distinct().ToList();
            var userEmailList = userList.Select(UList => new SelectListItem
            {
                Value = UList.Id.ToString(),
                Text = UList.Email.ToString()
            });

            //  ViewData["EmailList"] = userEmailList;
            ViewBag.EmailList = userEmailList;

            var entity = service.Get(input.Id, "PrimaryAddress");
            if (entity == null)
            {
                entity = new Client();
                if (input.httpPostedTemplateFile != null)
                {
                    var s = input.httpPostedTemplateFile.FileName;
                    var fname = s.Split('\\');
                    var ProfileName = fname[fname.Length - 1];
                    Stream filestream = input.httpPostedTemplateFile.InputStream;
                    BinaryReader br = new BinaryReader(filestream);
                    Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                    var ClientRow = ClientProfileManagerHelper.UpdateProfile(bytes, ProfileName, input.Id);
                    string imageBase64Data = Convert.ToBase64String(bytes);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                    ViewBag.Photo = imageDataURL;
                }
                else
                {
                    var Img = ClientProfileManagerHelper.getProfileClientDetail(input.Id);
                    if (Img.CPMFile != null)
                    {
                        string imageBase64Data = Convert.ToBase64String(Img.CPMFile);
                        string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                        var ClientRow = ClientProfileManagerHelper.UpdateProfile(Img.CPMFile, Img.Filename, input.Id);
                        ViewBag.Photo = imageDataURL;
                    }
                }
                if (input.httpPostedEmailTemplateFile != null)
                {
                    var s = input.httpPostedEmailTemplateFile.FileName;
                    var efname = s.Split('\\');
                    var emailProfileName = efname[efname.Length - 1];
                    Stream emailfilestream = input.httpPostedEmailTemplateFile.InputStream;
                    BinaryReader br = new BinaryReader(emailfilestream);
                    Byte[] bytes = br.ReadBytes((Int32)emailfilestream.Length);
                    var ClientRow = ClientProfileManagerHelper.UpdateClientProfile(bytes, emailProfileName, input.Id);
                    string imageBase64Data = Convert.ToBase64String(bytes);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                    ViewBag.EmailPhoto = imageDataURL;
                }
                else
                {
                    var Img = ClientProfileManagerHelper.getProfileClientDetail(input.Id);
                    if (Img.EmailTemplateImageCPMFile != null)
                    {
                        string imageBase64Data = Convert.ToBase64String(Img.EmailTemplateImageCPMFile);
                        string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                        var ClientRow = ClientProfileManagerHelper.UpdateProfile(Img.EmailTemplateImageCPMFile, Img.EmailTemplateImageName, input.Id);

                        ViewBag.EmailPhoto = imageDataURL;
                    }
                }
                var e = createMapper.MapToEntity(input, entity);

                var InputAddress = input.PrimaryAddress;
                var EntityAddress = new Domain.Entities.Address();
                var e1 = AddresscreateMapper.MapToEntity(InputAddress, EntityAddress);
                int AddressId = Addressservice.Create(e1);
                e.PrimaryAddressId = AddressId;

                //var InputEngagement = input.Engagement;
                //var EntityEngagement = new Domain.Entities.Engagements();
                //var e2 = EngagementcreateMapper.MapToEntity(InputEngagement, EntityEngagement);
                //int EngagementId = Engagementservice.Create(e2);
                //e.EngagementId = EngagementId;
                service.Create(e);
            }
            else
            {
                // var oldid = unitOfWork.Clients.GetByID(input.Id).VerticalId;
                try
                {
                    var oldid = service.Get(input.Id).VerticalId;
                    if (input.VerticalId != oldid && oldid > 0)
                    {
                        var paramNames = new string[] { "ClientId" };
                        var paramValueset = new object[] { input.Id };
                        returntable = unitOfWork.GetDataFromStoredProcedure("DeleteBusinessCapabilities", paramNames, paramValueset);
                    }
                }
                catch
                {

                }
                //entity = new Client();
                if (input.httpPostedTemplateFile != null)
                {
                    var s = input.httpPostedTemplateFile.FileName;
                    var fname = s.Split('\\');
                    var ProfileName = fname[fname.Length - 1];
                    Stream filestream = input.httpPostedTemplateFile.InputStream;
                    BinaryReader br = new BinaryReader(filestream);
                    Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                    input.CPMFile = bytes;
                    input.Filename = ProfileName;
                    var ClientRow = ClientProfileManagerHelper.UpdateProfile(bytes, ProfileName, input.Id);
                    string imageBase64Data = Convert.ToBase64String(bytes);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.Photo = imageDataURL;
                }
                else
                {
                    var Img = ClientProfileManagerHelper.getProfileClientDetail(input.Id);
                    if (Img.CPMFile != null)
                    {
                        string imageBase64Data = Convert.ToBase64String(Img.CPMFile);
                        string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                        input.CPMFile = Img.CPMFile;
                        input.Filename = Img.Filename;
                        var ClientRow = ClientProfileManagerHelper.UpdateProfile(Img.CPMFile, Img.Filename, input.Id);
                        ViewBag.Photo = imageDataURL;
                    }

                }
                if (input.httpPostedEmailTemplateFile != null)
                {
                    var s = input.httpPostedEmailTemplateFile.FileName;
                    var efname = s.Split('\\');
                    var emailProfileName = efname[efname.Length - 1];
                    Stream emailfilestream = input.httpPostedEmailTemplateFile.InputStream;
                    BinaryReader br = new BinaryReader(emailfilestream);
                    Byte[] bytes = br.ReadBytes((Int32)emailfilestream.Length);
                    input.EmailTemplateImageCPMFile = bytes;
                    input.EmailTemplateImageName = emailProfileName;
                    var ClientRow = ClientProfileManagerHelper.UpdateClientProfile(bytes, emailProfileName, input.Id);
                    string imageBase64Data = Convert.ToBase64String(bytes);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.EmailPhoto = imageDataURL;
                }
                else
                {
                    var Img = ClientProfileManagerHelper.getProfileClientDetail(input.Id);
                    if (Img.EmailTemplateImageCPMFile != null)
                    {
                        string imageBase64Data = Convert.ToBase64String(Img.EmailTemplateImageCPMFile);
                        string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                        input.EmailTemplateImageCPMFile = Img.EmailTemplateImageCPMFile;
                        input.EmailTemplateImageName = Img.EmailTemplateImageName;
                        var ClientRow = ClientProfileManagerHelper.UpdateProfile(Img.EmailTemplateImageCPMFile, Img.EmailTemplateImageName, input.Id);

                        ViewBag.EmailPhoto = imageDataURL;
                    }

                }
                var e = editMapper.MapToEntity(input, entity);
                service.Save();

                var InputAddress = input.PrimaryAddress;
                var EntityAddress = Addressservice.Get(input.PrimaryAddressId);
                var e1 = AddresscreateMapper.MapToEntity(InputAddress, EntityAddress);
                Addressservice.Save();

                //var InputEngagement = input.Engagement;
                //var EntityEngagement = Engagementservice.Get(input.EngagementId);
                //var e2 = EngagementcreateMapper.MapToEntity(InputEngagement, EntityEngagement);
                //Engagementservice.Save();
            }

            if (input.PrimaryAddress.RegionId != 0)
            {
                ViewData["country"] = GetDetails(1, 1, input.PrimaryAddress.RegionId);
            }
            else
            {
                ViewData["country"] = GetDetails(1, 0, 0);
            }

            if (input.PrimaryAddress.CountryId != 0)
            {
                ViewData["city"] = GetDetails(2, 1, input.PrimaryAddress.CountryId);
            }
            else
            {
                ViewData["city"] = GetDetails(1, 0, 0);
            }
            if (input.VerticalId == 0)
            {
                ModelState.AddModelError("VerticalId", "Please select the Vertical ID");
                return View(EditView, input);
            }
            else if (input.SubVerticalId == 0)
            {
                ModelState.AddModelError("SubVerticalId", "Please select the SubVertical ID");
                return View(EditView, input);
            }

            if (input.VerticalId != 0)
            {
                ViewData["subVertical"] = GetVerticalDetails(1, input.VerticalId);
            }
            else
            {
                ViewData["subVertical"] = GetVerticalDetails(0, 0);
            }

            input.LoginUser = (int)Session["UserId"];
            //ModelState.AddModelError("EditMsg", "Client Data Modified Successfully");
            ViewData["EditMsg"] = "Client Data Modified Successfully";


            return View(EditView, input);
        }

        public ActionResult DeleteClient(int id)
        {
            var entity = service.Get(id, "PrimaryAddress");
            var input = editMapper.MapToInput(entity);
            input.IsDeleted = true;
            entity.IsDeleted = true;
            service.Save();
            TempData["IsDeleted"] = "1";
            ModelState.AddModelError("EditMsg", "Client Deleted Successfully");
            //TempData["Success"] = "Client Deleted Successfully";
            return RedirectToAction("Create");
        }


        public IEnumerable<SelectListItem> GetDetails(int entity, int mode, int? Id)
        {
            DbContextFactory Context = new DbContextFactory();
            // For Country
            if (entity == 1)
            {
                if (mode == 0)
                {
                    List<SelectListItem> listItems = new List<SelectListItem>();
                    listItems.Add(new SelectListItem() { Selected = false, Text = "", Value = "0" });
                    return listItems;
                }
                else
                {
                    Repo<NTTData.AMOPortal.Domain.Entities.Countries> Countries = new Data.Repo<Domain.Entities.Countries>(Context);
                    List<NTTData.AMOPortal.Domain.Entities.Countries> CountryList = Countries.GetAll().Where(m => m.RegionId == Id).OrderBy(m => m.CountryName).ToList();
                    var Items = CountryList.Select(CList => new SelectListItem
                    {
                        Value = CList.Id.ToString(),
                        Text = CList.CountryName.ToString()
                    }).ToList();
                    //Items.Insert(0, new SelectListItem { Text = "", Value = "" });
                    return Items;
                }
            }

            // For City
            if (entity == 2)
            {
                if (mode == 0)
                {
                    List<SelectListItem> listItems = new List<SelectListItem>();
                    listItems.Add(new SelectListItem() { Selected = false, Text = "", Value = "0" });
                    return listItems;
                }
                else
                {
                    Repo<NTTData.AMOPortal.Domain.Entities.Cities> Cities = new Data.Repo<Domain.Entities.Cities>(Context);
                    List<NTTData.AMOPortal.Domain.Entities.Cities> CityList = Cities.GetAll().Where(m => m.CountryId == Id).OrderBy(m => m.CityName).ToList();
                    var Items = CityList.Select(CList => new SelectListItem
                    {
                        Value = CList.Id.ToString(),
                        Text = CList.CityName.ToString()
                    }).ToList();
                    //Items.Insert(0, new SelectListItem { Text = "", Value = "" });
                    return Items;
                }
            }
            return null;
        }

        public IEnumerable<SelectListItem> GetVerticalDetails(int mode, int? Id)
        {
            if (mode == 0)
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem() { Selected = false, Text = "", Value = "0" });
                return listItems;
            }
            else
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.SubVerticals> IndustrySubVerticals = new Data.Repo<Domain.Entities.SubVerticals>(Context);
                List<NTTData.AMOPortal.Domain.Entities.SubVerticals> IndustrySubVerticalsList = IndustrySubVerticals.GetAll().Where(m => m.VerticalId == Id).OrderBy(m => m.SubVerticalName).ToList();
                var Items = IndustrySubVerticalsList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.SubVerticalName.ToString()
                }).ToList();
                //Items.Insert(0, new SelectListItem { Text = "", Value = "" });
                return Items;
            }
        }

        [HttpGet]
        public JsonResult GetCountry(int region)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            Db db = new Db();
            var CountryNames = from country in db.Countries
                               where country.RegionId == region
                               orderby country.CountryName
                               select new
                               {
                                   Id = country.Id,
                                   Name = country.CountryName
                               };
            return Json(CountryNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCity(int city)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            Db db = new Db();
            var CityNames = from citys in db.Cities
                            where citys.CountryId == city
                            orderby citys.CityName
                            select new
                            {
                                Id = citys.Id,
                                Name = citys.CityName
                            };
            return Json(CityNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubVertical(int vertical)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            Db db = new Db();
            var VerticlsNames = from verticls in db.IndustrySubVerticals
                                where verticls.VerticalId == vertical
                                orderby verticls.SubVerticalName
                                select new
                                {
                                    Id = verticls.Id,
                                    Name = verticls.SubVerticalName
                                };
            return Json(VerticlsNames, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult GetClientInfo()
        //{
        //    return View("ViewClientsInfo");
        //}


        ////public ActionResult GetEngagementInfo()
        ////{

        ////    return View("ViewEngagements");

        ////}

        //public ActionResult GetAccountsInfo()
        //{
        //    return View("ViewAccounts");
        //}


        //-------------------------Team----------------------------//     
        [HttpPost]
        public ActionResult GetTeams([DataSourceRequest] DataSourceRequest request, int ClientId = 0)
        {

            int usid = (int)Session["UserId"];

            return Json(ClientProfileManagerHelper.GetTeamsDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddTeams(IMTeamModel imTeamModel)
        {

            if (imTeamModel != null && imTeamModel.Team.Trim() != " " && ModelState.IsValid)
            {

                int usid = (int)Session["UserId"];
                //  imTeamModel.ClientId = ClientId;
                imTeamModel.CreatedBy = usid;
                var res = ClientProfileManagerHelper.CreateTeams(imTeamModel);
                imTeamModel.Id = res;
            }
            return Json(new[] { imTeamModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditTeams([DataSourceRequest] DataSourceRequest request, IMTeamModel imTeamModel)
        {
            if (imTeamModel != null && imTeamModel.Team.Trim() != " " && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imTeamModel.ClientId = client.ClientId;
                imTeamModel.UpdatedBy = usid;
                var res = ClientProfileManagerHelper.UpdateTeams(imTeamModel);
            }
            return Json(new[] { imTeamModel }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteTeams([DataSourceRequest] DataSourceRequest request, IMTeamModel imTeamModel)
        {
            if (imTeamModel != null)
            {
                var v = ClientProfileManagerHelper.RemoveTeams(imTeamModel.Id);
            }
            return Json(new[] { imTeamModel }.ToDataSourceResult(request, ModelState));
        }


        //-------------------------Leadership Team----------------------------//   
      
        public int LeadClient ;

        [HttpGet]
        public JsonResult AddLeadershipTeam(int Id)
        {
           
                Db db = new Db();
                LeadershipDetails additem = (from a in db.UserProfiles
                                             join b in db.UserApplications on a.Id equals b.UserId
                                             join c in db.Countries on a.CountryCode equals c.Id
                                             where  a.Id == Id
                                             select new LeadershipDetails
                                             {
                                                 Id = a.Id,
                                                 EmailAddress = a.Email,
                                                 FirstName = a.FirstName,
                                                 LastName = a.LastName,
                                                 Country = c.CountryName,
                                                 NttResource = a.NTTDataResource,
                                                 ReportingManager = a.ReportingManager,
                                                 ClientId = b.ClientId
                                             }).FirstOrDefault(x => x.Id == Id);

                return Json(additem);
           
        }

        [HttpPost]
        public JsonResult AddLeadershipTeamDetails([DataSourceRequest] DataSourceRequest request,LeadershipDetails model)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var userid = (int)Session["UserId"];
            Db db = new Db();
            var leaduser = (from a in db.UserLeadershipRole
                            where a.UserId == model.Id && a.ClientId == model.ClientId
                            select a).FirstOrDefault();
            if (leaduser == null)
            {
                var userdetails = (from a in db.UserProfiles
                                   where a.Id == model.Id
                                   select a).FirstOrDefault();
                var clientdetails = (from a in db.Clients
                                     where a.Id == model.ClientId
                                     select a).FirstOrDefault();
                var RoleId = (from a in db.LeadershipRoles where a.RoleName == model.LeadRole select a.Id).FirstOrDefault();
                var ShoreId = (from a in db.EngagementLocation where a.Location == model.Shore select a.Id).FirstOrDefault();

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var LeaderDetails = new UserLeadershipRole();
                    LeaderDetails.ClientId = model.ClientId.Value;
                    LeaderDetails.UserId = model.Id;
                    LeaderDetails.CreatedDt = DateTime.Now;
                    LeaderDetails.CreatedBy = userid;
                    LeaderDetails.UpdatedBy = userid;
                    LeaderDetails.UpdatedDt = DateTime.Now;
                    LeaderDetails.LeadershipRoleId = RoleId;
                    LeaderDetails.ShoreId = ShoreId;
                    LeaderDetails.ReportingManager = model.ReportingManager;
                    unitOfWork.UserLeadershipRole.Insert(LeaderDetails);
                    unitOfWork.Save();
                    int id = LeaderDetails.UserId;
                }
                var email = (from a in db.UserProfiles where a.Id == model.Id select a.Email).FirstOrDefault();
                model.EmailAddress = email;
                model.Name = userdetails.FirstName + " " + userdetails.LastName;
                if (userdetails.CountryCode != null)
                {
                    model.Country = getCountryName(userdetails.CountryCode.Value);
                }
                else
                {
                    model.Country = null;
                }
                if (model.ShoreID != null)
                {
                    model.Shore = getShoreName(ShoreId);
                }
                if (model.LeadRoleID != null)
                {
                    model.LeadRole = getLeadRoleName(RoleId);
                }
                model.EmailAddress = userdetails.Email;
                model.ReportingManager = model.ReportingManager;
                model.UpdatedByName = getUserName(userid);
                model.CreatedByName = getUserName(userid);
                model.UpdatedDate = DateTime.Now;
                model.ClientId = model.ClientId;
                model.Id = model.Id;
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = userid;
                model.Organization = userdetails.NTTDataResource == true ? "NTTDATA" : clientdetails.ClientName;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("Id", "Leadership User is already present");
            }

            return Json(new[] { model }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [HttpPost]
        public JsonResult getLeadershipDetails([DataSourceRequest] DataSourceRequest request,int ClientId = 0)
        {
            Db db = new Db();
            LeadClient = ClientId;
            var client = (from a in db.Clients
                          where a.Id == ClientId
                          select a).FirstOrDefault();
            if(client==null)
            {
                return null;
            }
            var result = getLeadership(client.Id, client.ClientName);
          
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private List<LeadershipDetails> getLeadership(int clientId,string cName)
        {
            IEnumerable<LeadershipDetails> LeaderShipTeams = null;
            List<LeadershipDetails> leadList = new List<LeadershipDetails>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();

                LeaderShipTeams = (from a in db.UserProfiles
                                   join b in db.UserLeadershipRole on a.Id equals b.UserId
                                   where b.ClientId == clientId
                                   select new LeadershipDetails { 
                                   Id=a.Id,
                                   EmailAddress=a.Email,
                                   FirstName=a.FirstName,
                                   LastName=a.LastName,
                                   ReportingManager=a.ReportingManager,
                                   LeadRoleID=b.LeadershipRoleId,
                                   CountryID=a.CountryCode,
                                   ShoreID=b.ShoreId,
                                   NttResource=a.NTTDataResource,
                                   CreatedBy=b.CreatedBy,
                                   UpdatedBy=b.UpdatedBy,
                                   CreatedDate=b.CreatedDt,
                                   UpdatedDate=b.UpdatedDt,
                                   ClientId=clientId
                                   }).Distinct().ToList();  
                
                foreach(var item in LeaderShipTeams)
                {
                    LeadershipDetails lList = new LeadershipDetails();
                    lList.Id = item.Id;
                    lList.EmailAddress = item.EmailAddress;
                    lList.FirstName = item.FirstName;
                    lList.LastName = item.LastName;
                    lList.Name = item.FirstName + " " + item.LastName;
                    lList.ReportingManager = item.ReportingManager;
                    if (item.LeadRoleID != null)
                    {
                        lList.LeadRoleID = item.LeadRoleID;
                        lList.LeadRole = getLeadRoleName(item.LeadRoleID.Value);
                    }
                    if (item.CountryID != null)
                    {
                        lList.CountryID = item.CountryID;
                        lList.Country = getCountryName(item.CountryID.Value);
                    }
                    if (item.ShoreID != null)
                    {
                        lList.ShoreID = item.ShoreID;
                        lList.Shore = getShoreName(item.ShoreID.Value);
                    }
                    lList.Organization = item.NttResource == true ? "NTTDATA" : cName;                   
                    lList.CreatedByName = getUserName(item.CreatedBy.Value);
                    lList.ClientId = item.ClientId;
                    if (item.UpdatedBy!= null)
                    {
                        lList.UpdatedByName = getUserName(item.UpdatedBy.Value);
                    }
                    if (item.CreatedDate != null)
                    {
                        lList.CreatedDate = item.CreatedDate;
                    }
                    if (item.UpdatedDate != null)
                    {
                        lList.UpdatedDate = item.UpdatedDate;
                    }
                    leadList.Add(lList);
                }
            }
            return leadList;
        }


        [HttpPost]
        public ActionResult LeadershipDetailsUpdate([DataSourceRequest] DataSourceRequest request, LeadershipDetails model, int ClientId = 0)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
            var RoleId = (from a in db.LeadershipRoles where a.RoleName == model.LeadRole select a.Id).FirstOrDefault();
            var ShoreId = (from a in db.EngagementLocation where a.Location == model.Shore select a.Id).FirstOrDefault();
            model.LeadRoleID = RoleId;
            model.ShoreID = ShoreId;
            int LeaderTeamId = updateLeadershipDetails(model,model.ClientId.Value, uid);
            var email = (from a in db.UserProfiles where a.Id == model.Id select a.Email).FirstOrDefault();
            model.EmailAddress = email;
            model.Name = model.FirstName + " " + model.LastName;
            if(model.CountryID!=null)
            {
                model.Country = getCountryName(model.CountryID.Value);
            }
            else
            {
                model.Country = null;
            }
            if (model.ShoreID != null)
            {
                model.Shore = getShoreName(model.ShoreID.Value);
            }
            if (model.LeadRoleID != null)
            {
                model.LeadRole = getLeadRoleName(model.LeadRoleID.Value);
            }
            model.ReportingManager = model.ReportingManager;
            model.UpdatedByName = getUserName(uid);
            model.UpdatedDate = DateTime.Now;
            ModelState.Clear();
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public static int updateLeadershipDetails(LeadershipDetails leadDetail, int clientId, int userId)
        {
            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
               
                UserLeadershipRole updateitem = (from a in db.UserLeadershipRole                                         
                                          where a.ClientId == clientId && a.UserId == leadDetail.Id 
                                          select a).FirstOrDefault(x => x.UserId == leadDetail.Id);
                updateitem.Id = updateitem.Id;
                updateitem.UserId = leadDetail.Id;
                updateitem.LeadershipRoleId = leadDetail.LeadRoleID.Value;
                updateitem.ShoreId = leadDetail.ShoreID.Value;
                updateitem.ReportingManager = leadDetail.ReportingManager;              
                updateitem.UpdatedDt = DateTime.Now;
                updateitem.UpdatedBy = userId;
                db.SaveChanges();
                leadDetail.CreatedDate = updateitem.CreatedDt;
                leadDetail.CreatedBy = updateitem.CreatedBy;
            }
            return leadDetail.Id;
        }

        [HttpPost]
        public JsonResult LeadershipDetailsDelete([DataSourceRequest] DataSourceRequest request, LeadershipDetails model, int ClientId = 0)
        {
            Db db = new Db();
            int Eid = deleteLeadershipDetails(model.Id,model.ClientId.Value);
            ModelState.Clear();

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public static int deleteLeadershipDetails(int Id,int cid)
        {
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var userApp = (from a in db.UserLeadershipRole
                               where a.UserId == Id && a.ClientId == cid
                               select a).FirstOrDefault();               
                    unitOfWork.UserLeadershipRole.Delete(userApp.Id);
                    unitOfWork.Save();                
            }
            return Id;
        }

        public static string getUserName(int uid)
        {
            Db db = new Db();
            UserProfile userDetails = (from a in db.UserProfiles where a.Id == uid select a).FirstOrDefault(x => x.Id == uid);
            return userDetails.FirstName + " " + userDetails.LastName;
        }
        public static string getCountryName(int cId)
        {
            Db db = new Db();
            string countryName = (from a in db.Countries where a.Id == cId select a.CountryName).FirstOrDefault();
            return countryName;
        }
        public static string getShoreName(int sId)
        {
            Db db = new Db();
            string shoreName = (from a in db.EngagementLocation where a.Id == sId select a.Location).FirstOrDefault();
            return shoreName;
        }
        public static string getLeadRoleName(int lId)
        {
            Db db = new Db();
            string RoleName = (from a in db.LeadershipRoles where a.Id == lId select a.RoleName).FirstOrDefault();
            return RoleName;
        }
        
    }

}
