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

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class ServiceRequestController : Cruder<ServiceReqeustDataSet, SLAMDashboardInput>
    {
        public ServiceRequestController(ICrudService<ServiceReqeustDataSet> service, IMapper<ServiceReqeustDataSet, SLAMDashboardInput> v)
            : base(service, v)
        {

        }

        public ActionResult SRView()
        {
            SLAMDashboardInput input = new SLAMDashboardInput();
            ModelState.AddModelError("CreateMsg", "Please select a Client");
            return View("ViewSRbyClient", input);
        }

        public ActionResult ViewSRbyClient(int ClientId, int PageNumber = 1)
        {
            SLAMDashboardInput SRInput = new SLAMDashboardInput();
            SRInput.ClientId = ClientId;

            ViewBag.pagedItems = SRInput.GetPagedList(PageNumber);
            return View("ViewSRbyClient", SRInput);
        }

        public ActionResult DelSR(int id)
        {
            List<ServiceReqeustDataSet> AllList = service.GetAll().ToList();
            var SRData = AllList.Where(a => a.Id == id).FirstOrDefault();
            int clientId = SRData.ClientId;
            service.Delete(id);
            SLAMDashboardInput SRInput = new SLAMDashboardInput();
            SRInput.ClientId = clientId;
            ModelState.AddModelError("CreateMsg", "Service Request record deleted Successfully");
            ViewBag.pagedItems = SRInput.GetPagedList(1);
            return View("ViewSRbyClient", SRInput);
            //return RedirectToAction("ViewSRbyClient", new { ClientId = clientId });
        }

        public override ActionResult Edit(int id)
        {
            ServiceReqeustDataSet SRData = new ServiceReqeustDataSet();
            SRData = service.Get(id);

            SLAMDashboardInput SRInput = new SLAMDashboardInput();
            SRInput = AutoMapper.Mapper.Map<ServiceReqeustDataSet, SLAMDashboardInput>(SRData);
            return View("Edit", SRInput);
        }

        [HttpPost]
        public override ActionResult Edit(SLAMDashboardInput input)
        {
            if (!ModelState.IsValid)
                return View("Edit", input);
            //var e = editMapper.MapToEntity(input, service.Get(input.Id));
            var e = AutoMapper.Mapper.Map<SLAMDashboardInput, ServiceReqeustDataSet>(input, service.Get(input.Id));
            service.Save();
            ModelState.AddModelError("CreateMsg", "Service Request Edited successfully");
            return View(input);

        }

        public ActionResult CreateSR(int id)
        {
            SLAMDashboardInput srInput = new SLAMDashboardInput();
            srInput.ClientId = id;
            return View(srInput);
        }

        [HttpPost]
        public ActionResult CreateSR(SLAMDashboardInput input)
        {
            if (!ModelState.IsValid)
                return View("CreateSR", input);
            ServiceReqeustDataSet SRSet = new ServiceReqeustDataSet();
            var id = service.Create(AutoMapper.Mapper.Map<SLAMDashboardInput, ServiceReqeustDataSet>(input));
            var e = service.Get(id);
            ModelState.AddModelError("CreateMsg", "Service Request record created successfully");

            return View(input);

        }


    }
}