using Data;
using Domain.Entities.ClientProfileManager;
using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.ViewModel;
using WebUI.Areas.ClientProfileManager.Dto;
using System.Net.Mime;
using System.Text;
using System.IO;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WebUI.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class TicketInfoController : Controller
    {
        //changed for pagination "int? page"

        public ActionResult Index(int? ClientId, int? page)
        {
            ticketInfoInput input = new ticketInfoInput();
          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            if (ClientId == null || ClientId <= 0)
            {
                ClientId = client.ClientId;
            }
            input.ClientId = ClientId;
            // int client = ClientId == null ? 0 : ClientId;
            input.ticketInfoInputList = ClientProfileManagerHelper.GetTicketInfoTypes(ClientId);
            input.ticketInfoList = ClientProfileManagerHelper.GetTicketInfoDetail(ClientId).ToPagedList(page ?? 1, 16);
            return View(input);
        }


        [HttpPost]
        public ActionResult AddTictetInfo(ticketInfoInput ticketInfoinput, int ? page)
        {
            ModelState.Clear();
            if (ticketInfoinput.fromDt >= ticketInfoinput.toDt || ticketInfoinput.fromDt == DateTime.MinValue || ticketInfoinput.toDt == DateTime.MinValue)
            {
                ModelState.AddModelError("DateRangeError", "Please check the date range.");
            }
            //bool valuesEntered = false;
            //foreach (var tic in ticketInfoinput.ticketInfoInputList)
            //{
            //    if (!string.IsNullOrEmpty(tic.Comments))
            //    {
            //        valuesEntered = true;
            //        break;
            //    }
            //}
            //if (!valuesEntered)
            //{
            //    ModelState.AddModelError("DataFillErr", "Please add the detail.(One of the comment field to be non-empty) ");
            //}

          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            ticketInfoInput input = new ticketInfoInput();
            if (ModelState.IsValid)
            {
            var res = ClientProfileManagerHelper.AddTicketInfo(ticketInfoinput);
            }
            input.ClientId = client.ClientId;
            input.ticketInfoInputList = ClientProfileManagerHelper.GetTicketInfoTypes(client.ClientId);
            input.ticketInfoList = ClientProfileManagerHelper.GetTicketInfoDetail(client.ClientId).ToPagedList(page ?? 1, 16);

            return View("index", input);
        }

        public PartialViewResult DeleteTicketInfo(int tickinfoid)
        {
            var res = ClientProfileManagerHelper.DeleteTicketInfo(tickinfoid);
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            //Set default emp records  
            return PartialView("_TicketInfoDetail", ClientProfileManagerHelper.GetTicketInfoDetail(client.ClientId).ToPagedList( 1, 16));
        }



        ///To get the Client details (Controller)

        public ActionResult ClientDetails()
        {
            //ClientDetailsModel output = new ClientDetailsModel();
         //   NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            //output.ClientTicketInfoList = ClientProfileManagerHelper.GetTicketInfoDetail(client.Id).ToList();
            return View ("ClientDetailInfo",ClientProfileManagerHelper.GetClientDetails(client.ClientId));
        }

        public ActionResult Excel_Export_Read([DataSourceRequest]DataSourceRequest request)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            return Json(ClientProfileManagerHelper.GetTicketInfoDetail(client.ClientId).AsEnumerable().ToDataSourceResult(request));
        }

    }
}
