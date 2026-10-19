using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using WebUI.Areas.ClientProfileManager.ViewModel;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Data;
using Data;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class ExportTicketInfoController : Controller
    {
        //
        // GET: /ClientProfileManager/ExportTicketInfo/

        public ActionResult Index()
        {
           // ExportTicketInfoViewModel
            return View();
        }

        public IEnumerable<SelectListItem> ClientStatusList()
        {

            
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.ClientStatus.Get().ToList();
            var item = applicationList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Status.ToString() });
            return item;

        }
        
        [HttpPost]
        public ActionResult ExportTicket([DataSourceRequest] DataSourceRequest request, ClientTicketinfoSearchModel searchCriteria)
        {
            int usid = (int)Session["UserId"];
            List<ExportTicketInfoViewModel> lstTicketInfo = new List<ExportTicketInfoViewModel>();
           // lstTicketInfo.Add(new ExportTicketInfoViewModel { ClientId = 1, ClientStatusName = "xyz", SrMTTRSev4 =(decimal) 3.3 });
            lstTicketInfo = ClientProfileManagerHelper.GetAllClientTicketInfoDetail(searchCriteria, usid);
            ModelState.Clear();
            return Json(lstTicketInfo.ToDataSourceResult(request, ModelState));
            //return null;
        }
    }
}
