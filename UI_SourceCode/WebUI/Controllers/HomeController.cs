using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using WebUI.App_Start;
using NTTData.AMOPortal.WebUI.Mapper;
using Data;
using WebUI.Dto;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult UtilityManager()
        {
            return View();
        }
        public ActionResult ConfigTools()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DashBoard()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            var userClaims = HttpContext.GetOwinContext().Authentication.User.Claims;
            return View(userClaims);
        }
        public ActionResult BAMLanding()
        {
            return View();
        }
        public ActionResult PALanding()
        {
            return View();
        }

        public ActionResult Appstore()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
           // var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            var UserId = (int)Session["UserId"];
            HomeInput input = (HomeInput)Session["MenuItemsInput"];
            if (input.UserRole != "superuser")
            {
                input.UserRole = GetDropDownListItems.UserRoleForClient(UserId, Client.ClientId).Text;
            }
            input.ClientStats = new Areas.ClientProfileManager.ViewModel.ServiceDeliveryStatsModel(); //---Helper.ClientDeliveryStats(Client.ClientId);
            //input.SupportEcosystem = SupportEcoSystem(Client.Id).Value;
            input.SupportEcosystem = Client.SupportEcoSystem == true ? true : false;// SupportEcoSystem(Client.Id).Value;


            return View("NewLanding", input);
        }

        public ActionResult Login()
        {
            // ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult PortfolioAnalyzer()
        {
            return View();
            // return Redirect(System.Configuration.ConfigurationManager.AppSettings["PortFolioAnalyzerURL"].ToString());
            //return Redirect("http://207.242.15.251:82/WebPages/UserLogin.aspx");
        }



        public ActionResult MigrationPlanner()
        {

            return Redirect(System.Configuration.ConfigurationManager.AppSettings["MigrationPlannerURL"].ToString());
        }

        public ActionResult RetirementAccelerator()
        {

            return Redirect(System.Configuration.ConfigurationManager.AppSettings["RetirementAcceleratorURL"].ToString());
        }



        public ActionResult SourcingeffectivenessEvaluator()
        {

            return Redirect(System.Configuration.ConfigurationManager.AppSettings["SourcingeffectivenessEvaluatorURL"].ToString());
        }

        public ActionResult IncidentNavigator()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["IncidentNavigatorURL"].ToString());
            //return Redirect("http://207.242.15.251:81/WebPages/WelcomePage.aspx");
        }

        public ActionResult KnowledgeMgr()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["KmURL"].ToString());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public bool? SupportEcoSystem(int ClientId)
        {
            bool supportEcoSystemOut = false;
            try
            {
                var entityMapper = new Mapper<Client, ClientInput>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var client = unitOfWork.Clients.GetByID(ClientId);
                    if (client.SupportEcoSystem == true)
                        supportEcoSystemOut = true;
                    else
                        supportEcoSystemOut = false;

                    return supportEcoSystemOut;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}


