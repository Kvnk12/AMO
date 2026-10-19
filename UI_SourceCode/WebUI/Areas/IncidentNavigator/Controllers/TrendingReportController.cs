using Domain.Entities.ClientProfileManager;
using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Globalization;
using WebUI.Areas.IncidentNavigator.Dto;
//using Microsoft.AnalysisServices.AdomdClient;
using WebUI.Areas.IncidentNavigator.Models;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class TrendingReportController : Controller
    {
        //
        // GET: /IncidentNavigator/TrendingReport/

        public ActionResult Index(int? ClientId)
        {

            //ClusterManagementInput c = new ClusterManagementInput();
            //c.ClientId = ClientId;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View("AnalysisNew");

        }


        [ActionTrack]
        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

      //  private static readonly string connectionString = "DataSource=http://10.100.100.12/OLAP/msmdpump.dll;Catalog=NextGenToolIncidentCube";

        ////public ActionResult Read([PivotDataSourceRequest] PivotDataSourceRequest request)
        ////{
        ////    using (var connection = new AdomdConnection(connectionString))
        ////    {
        ////        connection.Open();
        ////        var command = connection.CreateCommand();
        ////        command.CommandText = request.Statement;

        ////        using (var reader = command.ExecuteXmlReader())
        ////        {
        ////            return Content(reader.ToXmlaResult());
        ////        }
        ////    }
        ////}

        ////public ActionResult Discover([PivotDataSourceRequest] PivotDataSourceRequest request)
        ////{
        ////    using (var connection = new AdomdConnection(connectionString))
        ////    {
               
        ////        connection.Open();

        ////        var restrictions = new AdomdRestrictionCollection();
        ////        foreach (var restriction in request.Restrictions)
        ////        {
        ////            restrictions.Add(restriction.Key, restriction.Value);
        ////        }

        ////        var result = connection.GetSchemaDataSet(request.Command, restrictions);

        ////        return Content(result.ToXmlaDiscoverResult());
        ////    }
        ////}

        public ActionResult ClusterAnalysis(int? ClientId)
        {

            ClusterManagementInput d = new ClusterManagementInput();
            d.ClientId = ClientId;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View(d);

        }

        public ActionResult InflowOutflowAnalysis(int? ClientId)
        {

            ClusterManagementInput d = new ClusterManagementInput();
            d.ClientId = ClientId;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View(d);

        }

        //public ActionResult IncidentAnalysis()
        //{           
        //    return View();

        //}

        //public ActionResult OLAPAnalysis()
        //{
        //    return View();

        //}
        public ActionResult BackLogAnalysis()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View();

        }

        public ActionResult AnalysisNew()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View();

        }
        public string getFullName(int userId)
        {
            Db db = new Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }

    }
}
