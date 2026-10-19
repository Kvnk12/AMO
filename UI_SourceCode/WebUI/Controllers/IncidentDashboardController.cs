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
    
    public class IncidentDashboardController : Cruder<Incident, IncidentInput>
    {

        public IncidentDashboardController(ICrudService<Incident> service, IMapper<Incident, IncidentInput> v)
            : base(service, v)
        {


        }

        public ActionResult IncidentView(string rptName)
        {
            IncidentInput IncInput = new IncidentInput();
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IncInput.ClientId = client.ClientId;
            IncInput.ReportName = rptName;
           // ModelState.AddModelError("SelClient", "Please select a Client");
            return View("CreateInc", IncInput);
        }

        public ActionResult PrdSuptView()
        {
            IncidentInput IncInput = new IncidentInput();
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IncInput.ClientId = client.ClientId;
           // ModelState.AddModelError("SelClient", "Please select a Client");
            return View("CreateIncPss", IncInput);
        }

        public ActionResult GetPrdSuptView(int cid)
        {
            IncidentInput input = new IncidentInput();
            ModelState.AddModelError("SelClient", "Please select a ticket type");
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreateIncPss", input);
        }

        public ActionResult GetView(int cid, string rptname, int dataset)
        {
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            cid = client.ClientId;
            if (dataset == 1)
            {
                IncidentInput input = new IncidentInput();
                input.ReportName = rptname;
                input.ClientId = cid;
                input.Datasetid = dataset;
                return View("CreateInc", input);
            }
            else if (dataset == 2)
            {
                ProblemInput input = new ProblemInput();
                input.ReportName = rptname;
                input.ClientId = cid;
                input.Datasetid = dataset;
                return View("CreatePblm", input);
            }
            else
            {
                MaintenanceRequestInput input = new MaintenanceRequestInput();
                input.ReportName = rptname;
                input.ClientId = cid;
                input.Datasetid = dataset;
                return View("CreateMR", input);
            }
        }


        //public ActionResult GetViewPssHM(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //   // Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    input.ClientId = client.ClientId;
        //    return View("CreatePssHM", input);
        //}

        //public ActionResult GetProjView(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    //Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    input.ClientId = client.ClientId;
        //    return View("CreateProj", input);
        //}

        //public ActionResult GetIntView(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //   // Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    input.ClientId = client.ClientId;
        //    return View("CreateInt", input);
        //}

        //public ActionResult GetViewAvg(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //   // Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    input.ClientId = client.ClientId;
        //    return View("CreateAvg", input);
        //}

        //public ActionResult GetViewRspAvg(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //   // Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    input.ClientId = client.ClientId;
        //    return View("CreateRspAvg", input);
        //}

        //public ActionResult GetViewTicketT(int cid)
        //{
        //    IncidentInput input = new IncidentInput();

        //   // Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    input.ClientId = client.ClientId;
        //    return View("CreateTicketT", input);
        //}

        //public ActionResult GetViewOverallSummary(int cid)
        //{
        //    MetricSpecificationInput input = new MetricSpecificationInput();
        //   // Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    input.ClientId = client.ClientId;
        //    return View("OverallSLASummary", input);
        //}

        public ActionResult GetProdSuptView(int dataset)
        {

          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (dataset == 1)
            {
                IncidentInput input = new IncidentInput();
              
                input.ClientId = client.ClientId;
                input.Datasetid = dataset;
                return View("CreateIncPss", input);
            }
            else if (dataset == 2)
            {
                ProblemInput input = new ProblemInput();
                input.ClientId = client.ClientId;
                input.Datasetid = dataset;
                return View("CreatePblmPss", input);
            }
            else
            {
                MaintenanceRequestInput input = new MaintenanceRequestInput();
                input.ClientId = client.ClientId;
                input.Datasetid = dataset;
                return View("CreateMRPss", input);
            }
        }

        //public ActionResult GetRptView(int cid, string rptname)
        //{
        //  //  Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    IncidentInput input = new IncidentInput();
        //    ModelState.AddModelError("SelClient", "Please select a ticket type");
        //    input.ReportName = rptname;
        //    input.ClientId = client.ClientId;
        //    return View("CreateInc", input);
        //}

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetViewPrdSuptHM(IncidentInput input)
        {
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreatePssHM", input);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetViewInc(IncidentInput input)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreateInc", input);
        }
        [HttpPost]
        public ActionResult GetViewMR(MaintenanceRequestInput input)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreateMR", input);
        }
        [HttpPost]
        public ActionResult GetViewPblm(ProblemInput input)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreatePblm", input);
        }
        [HttpPost]
        public ActionResult GetViewPrdSuptInc(IncidentInput input)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreateIncPss", input);
        }
        [HttpPost]
        public ActionResult GetViewPrdSuptMR(MaintenanceRequestInput input)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreateMRPss", input);
        }
        [HttpPost]
        public ActionResult GetViewPrdSuptPblm(ProblemInput input)
        {
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreatePblmPss", input);
        }
        [HttpPost]
        public ActionResult GetViewOverallSummary(MetricSpecificationInput input)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("OverallSLASummary", input);
        }


        public ActionResult GetViewOverallSummaryConfig(MetricSpecificationInput input)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("OverallSLASummaryConfig", input);
        }

        [HttpPost]
        public ActionResult GetProjView(IncidentInput input)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreateProj", input);
        }
        [HttpPost]
        public ActionResult GetIncView(IncidentInput input)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            return View("CreateInt", input);
        }


        //public ActionResult GetFirstTimeRight(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateFirstTimeRight", input);
        //}


        //public ActionResult GetBacklog(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateBacklog", input);
        //}

        //public ActionResult GetDeliveryEndDate(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateDeliveryEndDate", input);
        //}

        public ActionResult GetViewTicketAgeingAnalysis(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateTicketAgeingAnalysis", input);
        }



        //public ActionResult GetViewTicketVolume(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateTicketVolume", input);
        //}

        //public ActionResult GetViewTicketVolumeByStatus(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateTicketVolumeByStatus", input);
        //}

        //public ActionResult GetViewTicketVolumeByAssignedGroups(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateTicketVolumeByAssignedGroups", input);
        //}

        //public ActionResult GetViewTicketAging(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateTicketAging", input);
        //}

        //public ActionResult GetViewIncidenMetricByDatasource(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateIncidenMetricByDatasource", input);
        //}

        //public ActionResult GetViewIncidenUnAssignedToPerson(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateIncidentUnAssignedToPerson", input);
        //}

        //public ActionResult GetViewIncidenAssignedToPerson(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateIncidentAssignedToPerson", input);
        //}

        //public ActionResult GetViewScheduledVariance(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateScheduledVariance", input);
        //}

        //public ActionResult GetViewEffortVariance(int cid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = cid;
        //    return View("CreateEffortVariance", input);
        //}

        public ActionResult GetViewCustomerSatisfactionM(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateCustomerSatisfactionM", input);
        }

        public ActionResult GetViewCustomerSatisfactionQ(int cid)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = cid;
            return View("CreateCustomerSatisfactionQ", input);
        }
    }
}