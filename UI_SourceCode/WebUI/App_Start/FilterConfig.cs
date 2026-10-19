using System.Web;
using System.Web.Mvc;
using NttData.AMOPortal.Core;
 
namespace WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandledErrorLoggerFilter());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new NTTData.AMOPortal.WebUI.Filters.CustomAuthorizeAttribute());
            //filters.Add(new NTTData.AMOPortal.WebUI.Filters.APIAuthenticationAttribute());
        }
    }
}