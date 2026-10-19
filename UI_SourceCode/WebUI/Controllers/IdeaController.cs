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
    public class IdeaController : Cruder<IdeaDataSet, IdeaInput>
    {
        public IdeaController(ICrudService<IdeaDataSet> service, IMapper<IdeaDataSet, IdeaInput> v)
            : base(service, v)
        {

        }

        public ActionResult IdeaView()
        {
            IdeaInput input = new IdeaInput();
            ModelState.AddModelError("CreateMsg", "Please select a Client");
            return View("ViewIdeabyClient", input);
        }

        public ActionResult ViewIdeabyClient(int ClientId, int PageNumber = 1)
        {
            IdeaInput SRInput = new IdeaInput();
            SRInput.ClientId = ClientId;
            ViewBag.pagedItems = SRInput.GetPagedList(PageNumber);
            return View("ViewIdeabyClient", SRInput);
        }

        public ActionResult DelIdea(int id)
        {
            List<IdeaDataSet> AllList = service.GetAll().ToList();
            var SRData = AllList.Where(a => a.Id == id).FirstOrDefault();
            int clientId = SRData.ClientId;
            service.Delete(id);
            IdeaInput SRInput = new IdeaInput();
            SRInput.ClientId = clientId;
            ModelState.AddModelError("CreateMsg", "Idea deleted Successfully");
            ViewBag.pagedItems = SRInput.GetPagedList(1);

            return View("ViewIdeabyClient", SRInput);
        }

        public ActionResult EditIdea(int id)
        {
            IdeaDataSet SRData = new IdeaDataSet();
            SRData = service.Get(id);

            IdeaInput SRInput = new IdeaInput();
            SRInput = AutoMapper.Mapper.Map<IdeaDataSet, IdeaInput>(SRData);



            return View("EditIdea", SRInput);
        }

        [HttpPost]
        public ActionResult EditIdea(IdeaInput input)
        {
            if (!ModelState.IsValid)
                return View("Edit", input);
            var e = AutoMapper.Mapper.Map<IdeaInput, IdeaDataSet>(input, service.Get(input.Id));
            service.Save();
            ModelState.AddModelError("CreateMsg", "Idea Edited successfully");
            return View(input);

        }

        public ActionResult CreateIdea(int id)
        {
            IdeaInput srInput = new IdeaInput();
            srInput.ClientId = id;
            return View(srInput);
        }

        [HttpPost]
        public ActionResult CreateIdea(IdeaInput input)
        {
            if (!ModelState.IsValid)
                return View("CreateSR", input);
            IdeaDataSet SRSet = new IdeaDataSet();
            var id = service.Create(AutoMapper.Mapper.Map<IdeaInput, IdeaDataSet>(input));
            var e = service.Get(id);
            ModelState.AddModelError("CreateMsg", "Idea added successfully");

            return View(input);

        }


    }
}