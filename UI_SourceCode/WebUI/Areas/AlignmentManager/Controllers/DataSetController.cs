using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.AlignmentManager.Dto;

namespace WebUI.Areas.AlignmentManager.Controllers
    {
    public class DataSetController : Controller
        {
        //
        // GET: /AlignmentManager/HeatMapSetup/

        public ActionResult DisplayHomePage(int ClientId = 0)
            {

            DataSetupInput dataSetupInput= new DataSetupInput(); 
            if (ClientId > 0)
                {
                dataSetupInput.ClientId = ClientId;
                
                }
            

            return View("DataSetHomePage", dataSetupInput);
            }




        public ActionResult DisplaySSRSReport(int ClientId = 0)
            {

            ReportInput reportInput = new ReportInput();
            if (ClientId > 0)
                {
                reportInput.ClientId = ClientId;
                }

            reportInput.ReportName = "ReportConfigurationData";
            reportInput.PageHeading = "Report configuration Data";
            return View("ReportViewPage", reportInput);

            }




        //[HttpGet]
        //public ActionResult DisplayClientHeatMaps(int ClientId = 0)
        //    {
        //            heatMapSetupInput varheatMapSetupInput = new heatMapSetupInput(ClientId);
        //            return View("HeatMapSetupHomePage", varheatMapSetupInput);

        //    }
        //}

        }
    }
