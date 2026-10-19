using Data;
using DocumentFormat.OpenXml.Drawing.Charts;
using Domain.Entities.Survey;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.PortfolioAnalyzer;
using WebUI.Areas.PortfolioAnalyzer.Models;
using WebUI.Areas.RetirementAccelerator.Models;
using WebUI.Dto;

namespace WebUI.Areas.RetirementAccelerator.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /RetirementAccelerator/Dashboard/

        public ActionResult RatingBySubPerspectiveReport()
        {
            RatingBySubperspective c = new RatingBySubperspective();
            return View("PerspectiveReport", c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public JsonResult GetPerspectiveTypelist()
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var model = APRGetPerspectiveTypesBySurvey(client.ClientId);
            var item = model.Select(p => new SelectListItem { Value = p, Text = p }).ToList();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private static List<string> APRGetPerspectiveTypesBySurvey(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var list = unitOfWork.SurveyPerspective.Get(g => g.ClientId == ClientId).Select(r => r.PerspectiveType).ToList();
            return list;
        }

        [HttpPost]
        public ActionResult GetPerspectivesReport(string PerspectiveX, string PerspectiveY)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<RatingPerspReport> model = null;
           // string Apps = null;

            model = APRSubperspectiveReport(PerspectiveX, PerspectiveY, client.ClientId).ToList();

            return Json(model);
        }
        // spRAPerspectiveReport

        private static IList<RatingPerspReport> APRSubperspectiveReport(string PerspectiveX, string PerspectiveY, int ClientId)
        {
            IList<RatingPerspReport> list = new List<RatingPerspReport>();
            var paramName = new string[] { "PerspectiveX", "PerspectiveY", "ClientId" };
            var paramValue = new object[] { PerspectiveX, PerspectiveY, ClientId };


            System.Data.DataTable dt = new System.Data.DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("spRAPerspectiveReport", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new RatingPerspReport();


                    coldet.ApplicationName = dr["ApplicationName"].ToString();
                    coldet.X = dr["X"].ToString();
                    coldet.Y = dr["Y"].ToString();

                    try
                    {
                        coldet.Xrating = Convert.ToDecimal(dr["Xrating"]);
                        coldet.Yrating = Convert.ToDecimal(dr["Yrating"]);
                    }

                    catch { }
                    list.Add(coldet);
                }

            }
            return list;
        }

        public ActionResult GetApplications()
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var ApplicationList = RAHelper.getApplications(client.ClientId);
            return Json(ApplicationList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();

        }
        public ActionResult OnAppClick(int AppId)
        {
            var UserId = (int)Session["UserId"];
           // var Cl = (Client)Session["Client"];
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            RADashboardModel DashboardList = new RADashboardModel();
            DashboardList = RAHelper.getDashboardDetails(AppId);
            DashboardList.FinalRates = RAHelper.AppSurveyPerspectiveRating(AppId, Cl.ClientId, UserId);
            return Json(DashboardList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListOfProjectHealth()
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var listOfProHealth = (from UA in db.RAProjectHealth
                                   where UA.ClientId == client.ClientId
                                   select UA
                                            ).Distinct().ToList();
            var ProHealth = listOfProHealth.Select(g =>
                new SelectListItem
                {
                    Text = g.ProjectHealth,
                    Value = g.Id.ToString()
                }
                );
            return Json(ProHealth, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTaskDetail([DataSourceRequest] DataSourceRequest request, int ApplicationId = 0)
        {
            //var clid = (Client)Session["Client"];
            return Json(RAHelper.getTaskDetails(ApplicationId).AsEnumerable().ToDataSourceResult(request));
        }



    }
}
