using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;

using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Controllers;
using NTTData.AMOPortal.WebUI.Dto;


namespace WebUI.Areas.AlignmentManager.Controllers
{

    public class Client1Controller : ClientCruder<Client, ClientInput, NTTData.AMOPortal.Domain.Entities.Address, NTTData.AMOPortal.WebUI.Dto.Address, NTTData.AMOPortal.Domain.Entities.Engagements, NTTData.AMOPortal.WebUI.Dto.Engagements>
    {

        public Client1Controller(ICrudService<Client> service, IMapper<Client, ClientInput> v,
            ICrudService<NTTData.AMOPortal.Domain.Entities.Address> Addressservice, IMapper<NTTData.AMOPortal.Domain.Entities.Address, NTTData.AMOPortal.WebUI.Dto.Address> AddressMap,
            ICrudService<NTTData.AMOPortal.Domain.Entities.Engagements> EngagementService, IMapper<NTTData.AMOPortal.Domain.Entities.Engagements, NTTData.AMOPortal.WebUI.Dto.Engagements> EngagementMap)
            : base(service, v, Addressservice, AddressMap, EngagementService, EngagementMap)
        {
            //
        }

        public override ActionResult Create()
        {
            var input = new ClientInput();
            input.PrimaryAddress = input.PrimaryAddress == null ? new NTTData.AMOPortal.WebUI.Dto.Address() : input.PrimaryAddress;
            input.Engagement = input.Engagement == null ? new NTTData.AMOPortal.WebUI.Dto.Engagements() : input.Engagement;

            // added by neeraj starts for cascading dropdown
            ViewData["country"] = GetDetails(1, 0, 0);
            ViewData["city"] = GetDetails(2, 0, 0);
            ViewData["subVertical"] = GetVerticalDetails(0, 0);
            // added by neeraj ends

            return View(input);
        }

        [HttpPost]
        public override ActionResult Create(ClientInput input)
        {
            ViewData["country"] = GetDetails(1, 0, 0);
            ViewData["city"] = GetDetails(2, 0, 0);
            ViewData["subVertical"] = GetVerticalDetails(0, 0);

            if (!ModelState.IsValid)
                return View(input);
            using (Db db = new Db())
            {
                Client ExisitingClients = db.Clients.FirstOrDefault(c => c.ClientName.ToLower() == input.ClientName.ToLower());
                if (ExisitingClients == null)
                {
                    //try end
                    var entity = new Client();
                    input.ClientStatus = 3;
                    var e = createMapper.MapToEntity(input, entity);

                    //var InputAddress = input.PrimaryAddress;
                    //var EntityAddress = new NTTData.AMOPortal.Domain.Entities.Address();
                    //var e1 = AddresscreateMapper.MapToEntity(InputAddress, EntityAddress);
                    //int AddressId = Addressservice.Create(e1);
                    //e.PrimaryAddressId = AddressId;

                    //var InputEngagement = input.Engagement;
                    //var EntityEngagement = new NTTData.AMOPortal.Domain.Entities.Engagements();
                    //var e2 = EngagementcreateMapper.MapToEntity(InputEngagement, EntityEngagement);
                    //int EngagementId = Engagementservice.Create(e2);
                    //e.EngagementId = EngagementId;

                    //if (EntityAddress.RegionId != 0)
                    //{
                    //    ViewData["country"] = GetDetails(1, 1, EntityAddress.RegionId);
                    //}
                    //else
                    //{
                    //    ViewData["country"] = GetDetails(1, 0, 0);
                    //}

                    //if (EntityAddress.CountryId != 0)
                    //{
                    //    ViewData["city"] = GetDetails(2, 1, EntityAddress.CountryId);
                    //}
                    //else
                    //{
                    //    ViewData["city"] = GetDetails(1, 0, 0);
                    //}

                    //if (input.VerticalId != null)
                    //{
                    //    ViewData["subVertical"] = GetVerticalDetails(1, input.VerticalId);
                    //}
                    //else
                    //{
                    //    ViewData["subVertical"] = GetVerticalDetails(0, 0);
                    //}

                    service.Create(e);
                }
                else
                {
                    ModelState.AddModelError("ClientName", "Client name already exists. Please enter a different client name.");
                    return View(input);
                }

                ModelState.AddModelError("CreateMsg", "Client Created Successfully");
                return View(input);
            }

        }

        public override ActionResult Edit(int id)
        {
            var entity = service.Get(id); //, "PrimaryAddress", "Engagement");

            var input = editMapper.MapToInput(entity);
            //if (input == null)
            //{
            //    input = input == null ? new ClientInput() : input;
            //    input.PrimaryAddress = input.PrimaryAddress == null ? new NTTData.AMOPortal.WebUI.Dto.Address() : input.PrimaryAddress;
            //    input.Engagement = input.Engagement == null ? new NTTData.AMOPortal.WebUI.Dto.Engagements() : input.Engagement;
            //}
            //else
            //{
            //    input.PrimaryAddress = entity.PrimaryAddress == null ? new NTTData.AMOPortal.WebUI.Dto.Address() : AddresseditMapper.MapToInput(entity.PrimaryAddress);
            //    input.Engagement = entity.Engagement == null ? new NTTData.AMOPortal.WebUI.Dto.Engagements() : EngagementeditMapper.MapToInput(entity.Engagement);

            //}

            //if (entity.PrimaryAddress.RegionId != 0)
            //{
            //    ViewData["country"] = GetDetails(1, 1, entity.PrimaryAddress.RegionId);
            //}
            //else
            //{
            //    ViewData["country"] = GetDetails(1, 0, 0);
            //}

            //if (entity.PrimaryAddress.CountryId != 0)
            //{
            //    ViewData["city"] = GetDetails(2, 1, entity.PrimaryAddress.CountryId);
            //}
            //else
            //{
            //    ViewData["city"] = GetDetails(1, 0, 0);
            //}

            if (input.VerticalId != null)
            {
                ViewData["subVertical"] = GetVerticalDetails(1, input.VerticalId);
            }
            else
            {
                ViewData["subVertical"] = GetVerticalDetails(0, 0);
            }


            return View(EditView, input);
        }

        [HttpPost]
        public override ActionResult Edit(ClientInput input)
        {
            if (!ModelState.IsValid)
                return View(EditView, input);


            input.ClientStatus = 3;
            var entity = new Client();
            var e = editMapper.MapToEntity(input, entity);
            service.Save();


            //var entity = service.Get(input.Id, "PrimaryAddress");
            //if (entity == null)
            //{
            //    entity = new Client();
            //    var e = createMapper.MapToEntity(input, entity);

            //    //var InputAddress = input.PrimaryAddress;
            //    //var EntityAddress = new NTTData.AMOPortal.Domain.Entities.Address();
            //    //var e1 = AddresscreateMapper.MapToEntity(InputAddress, EntityAddress);
            //    //int AddressId = Addressservice.Create(e1);
            //    //e.PrimaryAddressId = AddressId;

            //    //var InputEngagement = input.Engagement;
            //    //var EntityEngagement = new NTTData.AMOPortal.Domain.Entities.Engagements();
            //    //var e2 = EngagementcreateMapper.MapToEntity(InputEngagement, EntityEngagement);
            //    //int EngagementId = Engagementservice.Create(e2);
            //    //e.EngagementId = EngagementId;
            //    service.Create(e);
            //}
            //else
            //{

            //    input.ClientStatus = 3;
            //    var e = editMapper.MapToEntity(input, entity);
            //    service.Save();

            //    //var InputAddress = input.PrimaryAddress;
            //    //var EntityAddress = Addressservice.Get(input.PrimaryAddressId);
            //    //var e1 = AddresscreateMapper.MapToEntity(InputAddress, EntityAddress);
            //    //Addressservice.Save();

            //    //var InputEngagement = input.Engagement;
            //    //var EntityEngagement = Engagementservice.Get(input.EngagementId);
            //    //var e2 = EngagementcreateMapper.MapToEntity(InputEngagement, EntityEngagement);
            //    //Engagementservice.Save();
            //}

            ////if (input.PrimaryAddress.RegionId != 0)
            ////{
            ////    ViewData["country"] = GetDetails(1, 1, input.PrimaryAddress.RegionId);
            ////}
            ////else
            ////{
            ////    ViewData["country"] = GetDetails(1, 0, 0);
            ////}

            ////if (input.PrimaryAddress.CountryId != 0)
            ////{
            ////    ViewData["city"] = GetDetails(2, 1, input.PrimaryAddress.CountryId);
            ////}
            ////else
            ////{
            ////    ViewData["city"] = GetDetails(1, 0, 0);
            ////}

            if (input.VerticalId != null)
            {
                ViewData["subVertical"] = GetVerticalDetails(1, input.VerticalId);
            }
            else
            {
                ViewData["subVertical"] = GetVerticalDetails(0, 0);
            }

            ModelState.AddModelError("EditMsg", "Client Edited Successfully");
            return View(EditView, input);
        }


        public ActionResult DeleteClient(int id)
        {
        var entity = service.Get(id); //, "PrimaryAddress", "Engagement");
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
                    Repo<NTTData.AMOPortal.Domain.Entities.Countries> Countries = new Repo<NTTData.AMOPortal.Domain.Entities.Countries>(Context);
                    List<NTTData.AMOPortal.Domain.Entities.Countries> CountryList = Countries.GetAll().Where(m => m.RegionId == Id).OrderBy(m => m.CountryName).ToList();
                    var Items = CountryList.Select(CList => new SelectListItem
                    {
                        Value = CList.Id.ToString(),
                        Text = CList.CountryName.ToString()
                    });
                    return Items.ToList();
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
                    Repo<NTTData.AMOPortal.Domain.Entities.Cities> Cities = new Repo<NTTData.AMOPortal.Domain.Entities.Cities>(Context);
                    List<NTTData.AMOPortal.Domain.Entities.Cities> CityList = Cities.GetAll().Where(m => m.CountryId == Id).OrderBy(m => m.CityName).ToList();
                    var Items = CityList.Select(CList => new SelectListItem
                    {
                        Value = CList.CountryId.ToString(),
                        Text = CList.CityName.ToString()
                    });
                    return Items.ToList();
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
                Repo<NTTData.AMOPortal.Domain.Entities.SubVerticals> IndustrySubVerticals = new Repo<NTTData.AMOPortal.Domain.Entities.SubVerticals>(Context);
                List<NTTData.AMOPortal.Domain.Entities.SubVerticals> IndustrySubVerticalsList = IndustrySubVerticals.GetAll().Where(m => m.VerticalId == Id).OrderBy(m => m.SubVerticalName).ToList();
                var Items = IndustrySubVerticalsList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.SubVerticalName.ToString()
                });
                return Items.ToList();
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

    }
}
