using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.AlignmentManager.Dto;

namespace WebUI.Areas.AlignmentManager.Controllers
{
    public class HeatMapSetupController : Controller
    {
        //
        // GET: /AlignmentManager/HeatMapSetup/

        public ActionResult Index(int cid=0)
        {
            heatMapSetupInput varheatMapSetupInput = new heatMapSetupInput(cid);
            varheatMapSetupInput.ClientId = cid;
            return View("HeatMapSetupHomePage", varheatMapSetupInput);
        }
        [HttpGet]
        public ActionResult DisplayClientHeatMaps(int ClientId = 0)
        {
            heatMapSetupInput varheatMapSetupInput = new heatMapSetupInput(ClientId);
            return View("HeatMapSetupHomePage", varheatMapSetupInput);

        }


        public ActionResult DisplayPAReports(int cid)
        {
            PAReportsInput paReportsInput = new PAReportsInput();
            paReportsInput.ClientId = cid;
            return View("PAreportsHomePage", paReportsInput);
        }

        public ActionResult DisplayCMReports(int cid, string rptname)
        {
            PAReportsInput paReportsInput = new PAReportsInput();
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            //ModelState.AddModelError("SelClient", "Please select a ticket type");
            paReportsInput.ReportName = rptname;
            paReportsInput.ClientId = cid;
            return View("CMreportsHomePage", paReportsInput);

        }

        [HttpGet]
        public ActionResult SetupHeatMapBuilder()
        {

            return View("SetupHeatMapBuilder");
        }


        [HttpGet]
        public ActionResult DisplayClientsPAReports(int ClientId = 0)
        {

            PAReportsInput paReportsInput = new PAReportsInput();
            paReportsInput.ClientId = ClientId;
            return View("PAreportsHomePage", paReportsInput);

        }







    }




}
