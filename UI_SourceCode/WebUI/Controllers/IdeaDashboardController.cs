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
    public class IdeaDashboardController : Cruder<IdeaDataSet, IdeaInput>
    {

        public IdeaDashboardController(ICrudService<IdeaDataSet> service, IMapper<IdeaDataSet, IdeaInput> v)
            : base(service, v)
        {


        }

        public ActionResult BenefitbyLOB()
        {
            IdeaInput IdInput = new IdeaInput();
            ModelState.AddModelError("SelClient", "Please select a Client");
            return View(IdInput);
        }

        public ActionResult BenefitbyTrend()
        {
            IdeaInput IdInput = new IdeaInput();
            ModelState.AddModelError("SelClient", "Please select a Client");
            return View(IdInput);
        }

        public ActionResult InnoTrend()
        {
            IdeaInput IdInput = new IdeaInput();
            ModelState.AddModelError("SelClient", "Please select a Client");
            return View(IdInput);
        }

        public ActionResult GetIdeaL(int id)
        {
            IdeaInput input = new IdeaInput();
            input.ClientId = id;
            return View("BenefitbyLOB", input);
        }

        [HttpPost]
        public ActionResult GetIdeaL(IdeaInput input)
        {
            return View("BenefitbyLOB", input);
        }

        public ActionResult GetIdeaT(int id)
        {
            IdeaInput input = new IdeaInput();
            input.ClientId = id;
            return View("BenefitbyTrend", input);
        }

        [HttpPost]
        public ActionResult GetIdeaT(IdeaInput input)
        {
            return View("BenefitbyTrend", input);
        }

        public ActionResult GetIdeaI(int id)
        {
            IdeaInput input = new IdeaInput();
            input.ClientId = id;
            return View("InnoTrend", input);
        }

        [HttpPost]
        public ActionResult GetIdeaI(IdeaInput input)
        {
            return View("InnoTrend", input);
        }
    }
}