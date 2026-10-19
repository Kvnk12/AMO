using System.Web;
using System.Web.Mvc;

namespace okta_aspnet_mvc_example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [Authorize]
        public ActionResult OktaProfile()
        {
            var userClaimsauttype = HttpContext.User.Identity.AuthenticationType;
                 var userClaims = HttpContext.GetOwinContext().Authentication.User.Claims;

            return View("Profile", userClaims);
        }
    }
}