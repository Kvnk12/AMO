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
    public class SRBASummaryController : Cruder<ServiceReqeustDataSet, SLAMDashboardInput>
    {

        public SRBASummaryController(ICrudService<ServiceReqeustDataSet> service, IMapper<ServiceReqeustDataSet, SLAMDashboardInput> v)
            : base(service, v)
        {


        }

        public ActionResult SRBAView()
        {
            SLAMDashboardInput SLAMDInput = new SLAMDashboardInput();
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            SLAMDInput.ClientId = client.ClientId;
            //ModelState.AddModelError("SelClient", "Please select a Client");
            return View("Create", SLAMDInput);
        }

        public ActionResult GetSLAM(int cid)
        {
            SLAMDashboardInput input = new SLAMDashboardInput();
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;

            ModelState.AddModelError("SelFilter", "Please select all the filter attributes and submit");
            return View("Create", input);
        }

        [HttpPost]
        public ActionResult GetSLAM(SLAMDashboardInput input)
        {
            if (input.ECD_ACDMonth == 0 || input.ECD_ACDYear == 0)
            {
                ModelState.AddModelError("SelFilter", "Please select all the filter attributes and submit");
            }
         //   Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("Create", input);
        }

    }
}