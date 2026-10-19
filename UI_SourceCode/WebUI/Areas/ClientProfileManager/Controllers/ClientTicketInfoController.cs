using System;
using System.Collections.Generic;
using System.Linq;
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
using NTTData.AMOPortal.WebUI.Mapper;
using Domain.Entities.ClientProfileManager;
using NTTData.AMOPortal.Data;
using Data;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class ClientTicketInfoController : Controller
    {
        //
        // GET: /ClientProfileManager/ClientTicketInfo/

        public ActionResult Index()
        {
            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddTictetInfo([DataSourceRequest] DataSourceRequest request, ClientTicketInfoViewModel ticketInfoinput)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ModelState.Clear();
            if (ticketInfoinput.Quarter<=0)
            {
                ModelState.AddModelError("DateRangeError", "Please select the Quarter.");
            }
            if(ticketInfoinput.Year<=0)
            {
                ModelState.AddModelError("DateRangeError", "Please select the Year.");
            }
            ticketInfoinput.ClientId = client.ClientId;
            if (isQuarterDataExist(ticketInfoinput))
            {
                ModelState.AddModelError("DataExistErr", "Quarter and Year data already exist. Please delete existing data and add again.");
            }


            if (ModelState.IsValid && ticketInfoinput.Quarter >= 0 && ticketInfoinput.Year >= 0)

            {


                var res = HelperAddTicketInfooo(ticketInfoinput);

                if (res > 0)
                {
                    ticketInfoinput.Id = res;
                }

                else

                    ModelState.AddModelError("SaveErr", "Error occured while saving data. Please contact system admin.");


            }


            return Json(new[] { ticketInfoinput }.ToDataSourceResult(request, ModelState)); ;

        }
        private bool isQuarterDataExist(ClientTicketInfoViewModel ticketInfoinput)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                return unitOfWork.ClientTicketInfo.Get(g => g.ClientId == ticketInfoinput.ClientId && g.Quarter == ticketInfoinput.Quarter && g.Year == ticketInfoinput.Year).Any();
            }
        }





        //helper for add ticket
        public static int HelperAddTicketInfooo(ClientTicketInfoViewModel ticketInfoinput)
        {
            int id = 0;
            var entityMapper = new Mapper<ClientTicketInfo, TicketInfoViewModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var dnTicketinfo = new ClientTicketInfo();
                    //entityMapper.MapToEntity(ticketInfoinput, dnTicketinfo);
                    dnTicketinfo.ClientId = ticketInfoinput.ClientId;
                    //dnTicketinfo.FromYear = ticketInfoinput.FromYear;
                    //dnTicketinfo.FromQuarter = ticketInfoinput.FromQuarter;
                    dnTicketinfo.Year = ticketInfoinput.Year; //.Year;
                    dnTicketinfo.Quarter = ticketInfoinput.Quarter;
                    dnTicketinfo.IncidentNumber = ticketInfoinput.IncidentNumber;
                    dnTicketinfo.IncidentEffortHrs = ticketInfoinput.IncidentEffortHrs;
                    dnTicketinfo.IncidentEffortPerUnit = ticketInfoinput.IncidentEffortPerUnit;
                    dnTicketinfo.UssrNumber = ticketInfoinput.UssrNumber;
                    dnTicketinfo.UssrEffortHrs = ticketInfoinput.UssrEffortHrs;
                    dnTicketinfo.UssrEffortPerUnit = ticketInfoinput.UssrEffortPerUnit;
                    dnTicketinfo.EnhanceNumber = ticketInfoinput.EnhanceNumber;
                    dnTicketinfo.EnhaceEffortHrs = ticketInfoinput.EnhaceEffortHrs;
                    dnTicketinfo.EnhanceEffortPerUnit = ticketInfoinput.EnhanceEffortPerUnit;
                    dnTicketinfo.ProblemsNumber = ticketInfoinput.ProblemsNumber;
                    dnTicketinfo.ProblemsEffortHrs = ticketInfoinput.ProblemsEffortHrs;
                    dnTicketinfo.ProblemsEffortPerUnit = ticketInfoinput.ProblemsEffortPerUnit;
                    dnTicketinfo.InMTTRSev1 = ticketInfoinput.InMTTRSev1;
                    dnTicketinfo.InMTTRSev2 = ticketInfoinput.InMTTRSev2;
                    dnTicketinfo.InMTTRSev3 = ticketInfoinput.InMTTRSev3;
                    dnTicketinfo.InMTTRSev4 = ticketInfoinput.InMTTRSev4;
                    dnTicketinfo.SrMTTRSev1 = ticketInfoinput.SrMTTRSev1;
                    dnTicketinfo.SrMTTRSev2 = ticketInfoinput.SrMTTRSev2;
                    dnTicketinfo.SrMTTRSev3 = ticketInfoinput.SrMTTRSev3;
                    dnTicketinfo.SrMTTRSev4 = ticketInfoinput.SrMTTRSev4;
                    dnTicketinfo.SRBacklog = ticketInfoinput.SRBacklog;
                    dnTicketinfo.NttFTENearShore = ticketInfoinput.NttFTENearShore;
                    dnTicketinfo.NttFTEOffshore = ticketInfoinput.NttFTEOffshore;
                    dnTicketinfo.NttFTEOnsite = ticketInfoinput.NttFTEOnsite;
                    dnTicketinfo.NoOfApplications = ticketInfoinput.NoOfApplications;
                    dnTicketinfo.Comment = ticketInfoinput.Comment;


                    // dnTicketinfo.CreatedOn = DateTime.Now;
                    // dnTicketinfo.UpdatedOn = DateTime.Now;
                    unitOfWork.ClientTicketInfo.Insert(dnTicketinfo);

                    unitOfWork.Save();
                    id = dnTicketinfo.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0 ;
            }
        }
        //to get ticket info list
        [HttpPost]
        public ActionResult ClientTicInfoList([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            return Json(HelperGetClientTicketInfoDetail(ClientId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        // get ticket info list-Helper
        public static IList<ClientTicketInfoViewModel> HelperGetClientTicketInfoDetail(int? ClientId)
        {


            var db = new Db();
            var tinfo = (from tkt in db.ClientTicketInfo

                         where (tkt.ClientId == ClientId)
                         select tkt).ToList();

            IList<ClientTicketInfoViewModel> clientticketInfoDetailViewModels = tinfo.Select(tkt=> new ClientTicketInfoViewModel
            {
                Id = tkt.Id,
                ClientId = tkt.ClientId,
                //FromYear = tkt.FromYear,
                //FromQuarter = tkt.FromQuarter,
                Year =tkt.Year, //new DateTime(tkt.Year,1,1),  
                Quarter  = tkt.Quarter,
                IncidentNumber = tkt.IncidentNumber,
                IncidentEffortHrs = tkt.IncidentEffortHrs,
                IncidentEffortPerUnit = tkt.IncidentEffortPerUnit,
                UssrNumber = tkt.UssrNumber,
                UssrEffortHrs = tkt.UssrEffortHrs,
                UssrEffortPerUnit = tkt.UssrEffortPerUnit,
                EnhanceNumber = tkt.EnhanceNumber,
                EnhaceEffortHrs = tkt.EnhaceEffortHrs,
                EnhanceEffortPerUnit = tkt.EnhanceEffortPerUnit,
                ProblemsNumber = tkt.ProblemsNumber,
                ProblemsEffortHrs = tkt.ProblemsEffortHrs,
                ProblemsEffortPerUnit = tkt.ProblemsEffortPerUnit,
                SRBacklog = tkt.SRBacklog,
                InMTTRSev1 = tkt.InMTTRSev1,
                InMTTRSev2 = tkt.InMTTRSev2,
                InMTTRSev3 = tkt.InMTTRSev3,
                InMTTRSev4 = tkt.InMTTRSev4,
                SrMTTRSev1 = tkt.SrMTTRSev1,
                SrMTTRSev2 = tkt.SrMTTRSev2,
                SrMTTRSev3 = tkt.SrMTTRSev3,
                SrMTTRSev4 = tkt.SrMTTRSev4,
                NttFTENearShore = tkt.NttFTENearShore,
                NttFTEOffshore = tkt.NttFTEOffshore,
                NttFTEOnsite = tkt.NttFTEOnsite,
                NoOfApplications = tkt.NoOfApplications,
                Comment= tkt.Comment


            }
                    ).OrderBy(o => o.Year).ToList();
            return clientticketInfoDetailViewModels;

        }




        //public PartialViewResult DeleteTicketInfo(int tickinfoid)
        //{
        //    var res = ClientProfileManagerHelper.DeleteTicketInfo(tickinfoid);
        //    NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
        //    //Set default emp records  
        //    return PartialView("_TicketInfoDetail", ClientProfileManagerHelper.GetTicketInfoDetail(client.Id).ToPagedList(1, 16));
        //}

        public static bool HelperDeleteticket(int Id)
        {

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.ClientTicketInfo.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }


        }

        //[AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult DeleteTicketInfo(int did)
        {

            var v = HelperDeleteticket(did);


            return null;
        }


        //update logic
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult UpdateTicketInfo([DataSourceRequest] DataSourceRequest request, ClientTicketInfoViewModel ticketInfoinput)
        //{
        //    if (ticketInfoinput != null && ModelState.IsValid)
        //    {

        //        var res = HelperUpdateTicketInfo(ticketInfoinput);
        //    }

        //    return Json(new[] { ticketInfoinput }.ToDataSourceResult(request, ModelState));
        //}



        //public static bool HelperUpdateTicketInfo(ClientTicketInfoViewModel ticketInfoinput)
        //{
        //    try
        //    {
        //        var entityMapper = new Mapper<ClientTicketInfo, ClientTicketInfoViewModel>();
        //        using (var contextFactory = new DbContextFactory())
        //        {
        //            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //            var updatetitem = unitOfWork.ClientTicketInfo.GetByID(ticketInfoinput.Id);
        //            entityMapper.MapToEntity(ticketInfoinput, updatetitem);
        //            unitOfWork.ClientTicketInfo.Update(updatetitem);
        //            unitOfWork.Save();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        [HttpGet]
        public ActionResult getTicketInforForId(int _id)
        {
            var TicketInformationModel = new ClientTicketInfoViewModel();
            //ClientTicketInfoViewModel ewwe = new ClientTicketInfoViewModel();
            using (var contextFactory = new DbContextFactory())
            {
                var entityMapper = new Mapper<ClientTicketInfoViewModel, ClientTicketInfo>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var ticInfo = unitOfWork.ClientTicketInfo.GetByID(_id);
                entityMapper.MapToEntity(ticInfo, TicketInformationModel);
                // AppViewModels.Add(ApplicationInformationModel);

            }
            return View("_editingTicketDetails", TicketInformationModel);
        }

        [HttpPost]
        public ActionResult UpdateTicketInfo(ClientTicketInfoViewModel ticketInfoinput)
        {
            var res=false;
            if (ticketInfoinput != null && ModelState.IsValid)
            {
                var rrr = isQuarterDataExist11(ticketInfoinput);
                if (rrr==0)
                {
                    ModelState.AddModelError("DataExistErr", "Quarter and Year data already exist. Please delete existing data and add again.");
                }
                else
                {
                    ModelState.Clear();
                    res = HelperUpdateTicketInfo(ticketInfoinput);
                }

            }
            else {
                ModelState.AddModelError("ValidationError", "Invalid entries");
            }
            if (res)
            {
                TempData.Add("ValidationSuccessMsg", "Updated Successfully!");
            }
            else
            {
                ModelState.AddModelError("SaveErrMsg", "Error while updating Details.");
            }

            return View("_editingTicketDetails", ticketInfoinput);
        }

        private int isQuarterDataExist11(ClientTicketInfoViewModel ticketInfoinput)
        {
            var ret = 0;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cd = unitOfWork.ClientTicketInfo.Get(g => g.ClientId == ticketInfoinput.ClientId && g.Quarter == ticketInfoinput.Quarter && g.Year == ticketInfoinput.Year).FirstOrDefault();
                if (cd.Id == ticketInfoinput.Id)
                {
                    ret = 1; // to do update 
                }

                return ret;
            }
        }

        public static bool HelperUpdateTicketInfo(ClientTicketInfoViewModel ticketInfoinput)
        {
            try
            {
                var entityMapper = new Mapper<ClientTicketInfo, ClientTicketInfoViewModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.ClientTicketInfo.GetByID(ticketInfoinput.Id);
                    entityMapper.MapToEntity(ticketInfoinput, updatetitem);
                    unitOfWork.ClientTicketInfo.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}