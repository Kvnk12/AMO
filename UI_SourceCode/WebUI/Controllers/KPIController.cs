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
    public class KPIController : Cruder<Incident, IncidentInput>
    {

        public KPIController(ICrudService<Incident> service, IMapper<Incident, IncidentInput> v)
            : base(service, v)
        {


        }
        public ActionResult LayoutView(IncidentInput input)
        {
            return View(input);
        }

        public ActionResult GetViewTicketT(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateTicketT", input);
        }

        public ActionResult GetViewTicketVolume(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateTicketVolume", input);
        }

        public ActionResult GetViewTicketVolumeByStatus(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateTicketVolumeByStatus", input);
        }

        public ActionResult GetViewTicketVolumeByAssignedGroups(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateTicketVolumeByAssignedGroups", input);
        }

        //public ActionResult GetViewTicketByAssignedGroups(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateTicketByAssignedGroups", input);
        //}

        public ActionResult GetViewIncidenMetricByDatasource(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateIncidenMetricByDatasource", input);
        }

        public ActionResult GetViewIncidenUnAssignedToPerson(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateIncidentUnAssignedToPerson", input);
        }

        public ActionResult GetViewIncidenAssignedToPerson(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateIncidentAssignedToPerson", input);
        }

        public ActionResult GetViewTicketAgeingAnalysis (int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateTicketAgeingAnalysis", input);
        }
    }
}
