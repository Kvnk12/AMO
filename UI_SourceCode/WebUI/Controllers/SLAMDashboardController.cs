using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using WebUI.Dto;

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class SLAMDashboardController : Cruder<ServiceReqeustDataSet, SLAMDashboardInput>
    {

        public SLAMDashboardController(ICrudService<ServiceReqeustDataSet> service, IMapper<ServiceReqeustDataSet, SLAMDashboardInput> v)
            : base(service, v)
        {


        }

        public ActionResult SLAMDView()
        {
            SLAMDashboardInput SLAMDInput = new SLAMDashboardInput();
            //   Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            SLAMDInput.ClientId = client.ClientId;
           // ModelState.AddModelError("SelClient", "Please select a Client");
            return View("Create", SLAMDInput);
        }

        public ActionResult LayoutView(SLAMDashboardInput input)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View(input);
        }

        public ActionResult GetSLAM(int id)
        {
            SLAMDashboardInput input = new SLAMDashboardInput();
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("Create", input);
        }

        [HttpPost]
        public ActionResult GetSLAM(SLAMDashboardInput input)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("Create", input);
        }

        public ActionResult GetFilters(int id)
        {
            SLAMDashboardInput SLAMInput = new SLAMDashboardInput();
            DbContextFactory Dbcon = new DbContextFactory();
            Repo<SRFilters> repoFilters = new Repo<SRFilters>(Dbcon);
            //Mapper<SRFilters, SRFiltersInput> mapperSRF = new Mapper<SRFilters, SRFiltersInput>();
            CrudService<SRFilters> serSRF = new CrudService<SRFilters>(repoFilters);
            var e = serSRF.Get(id);
            SLAMInput.srFilters.Name = e.Name;
            SLAMInput.srFilters.Description = e.Description;
            SLAMInput.ECD_ACDMonth = e.Month;
            SLAMInput.ECD_ACDYear = e.Year;
            SLAMInput.IncludeRecord = e.IncludeRecord;
            SLAMInput.ServiceRequestCategory = e.SRCategory;
            SLAMInput.Priority = e.Priority;
            SLAMInput.DivisionImpacted = e.DivisionImpacted;
            SLAMInput.BusinessUnitImpacted = e.BUImpacted;
            SLAMInput.PrimarySystemAffected = e.PYSysAffected;
            SLAMInput.RegionsImpacted = e.RegImpacted;
            SLAMInput.LocationsImpacted = e.LocImpacted;
            SLAMInput.ServiceArea = e.ServiceArea;
            SLAMInput.ServiceCategory = e.ServiceCategory;
            SLAMInput.ServiceSubcategory = e.ServiceSubCategory;

            return View("Create", SLAMInput);
        }

        [HttpPost]
        public ActionResult GetFilters(SLAMDashboardInput input)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            Repo<SRFilters> repoFilters = new Repo<SRFilters>(Dbcon);
            Mapper<SRFilters, SRFiltersInput> mapperSRF = new Mapper<SRFilters, SRFiltersInput>();
            CrudService<SRFilters> serSRF = new CrudService<SRFilters>(repoFilters);
            SRFilters SLAMInput = new SRFilters();

            SLAMInput.Name = input.srFilters.Name;
            SLAMInput.Description = input.srFilters.Description;
            SLAMInput.Month = input.ECD_ACDMonth;
            SLAMInput.Year = input.ECD_ACDYear;
            SLAMInput.IncludeRecord = input.IncludeRecord;
            SLAMInput.SRCategory = input.ServiceRequestCategory;
            SLAMInput.Priority = input.Priority;
            SLAMInput.DivisionImpacted = input.DivisionImpacted;
            SLAMInput.BUImpacted = input.BusinessUnitImpacted;
            SLAMInput.PYSysAffected = input.PrimarySystemAffected;
            SLAMInput.RegImpacted = input.RegionsImpacted;
            SLAMInput.LocImpacted = input.LocationsImpacted;
            SLAMInput.ServiceArea = input.ServiceArea;
            SLAMInput.ServiceCategory = input.ServiceCategory;
            SLAMInput.ServiceSubCategory = input.ServiceSubcategory;
            var e = serSRF.Create(SLAMInput);
            return View("Create", serSRF.Get(e));
        }


    }
}