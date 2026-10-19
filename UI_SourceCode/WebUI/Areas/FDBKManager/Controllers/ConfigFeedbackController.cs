using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.FDBKManager.Models;
using WebUI.Dto;

namespace WebUI.Areas.FDBKManager.Controllers
{
    public class ConfigFeedbackController : Controller
    {
        // GET: FDBKManager/ConfigFeedback
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult GetFeedbackType([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(FMHelper.GetfeedbackTypeDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddFeedbackType(FeedbackTypeModel feedbackTypeModel, int Id)
        {

            if (feedbackTypeModel != null && feedbackTypeModel.FeedbackType.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                feedbackTypeModel.ClientId = Client.ClientId;
                feedbackTypeModel.CreatedBy = usid;
                var res = FMHelper.CreateFeedbackType(feedbackTypeModel, Id);
                feedbackTypeModel.Id = res;
            }
            return Json(new[] { feedbackTypeModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditFeedbackType([DataSourceRequest] DataSourceRequest request, FeedbackTypeModel feedbackTypeModel)
        {
            if (feedbackTypeModel != null && feedbackTypeModel.FeedbackType.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                feedbackTypeModel.ClientId = Client.ClientId;
                feedbackTypeModel.UpdatedBy = usid;
                var res = FMHelper.UpdateFeedbackType(feedbackTypeModel);
            }
            return Json(new[] { feedbackTypeModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteFeedbackType([DataSourceRequest] DataSourceRequest request, FeedbackTypeModel feedbackTypeModel)
        {
            if (feedbackTypeModel != null)
            {
                var v = FMHelper.RemoveFeedbackType(feedbackTypeModel.Id);
            }
            return Json(new[] { feedbackTypeModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult GetProuctApp([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(FMHelper.GetProductAppDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddProductApp(FeedbackProductModel feedbackProductModel, int Id)
        {

            if (feedbackProductModel != null && feedbackProductModel.Product.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                feedbackProductModel.ClientId = Client.ClientId;
                feedbackProductModel.CreatedBy = usid;
                var res = FMHelper.CreateProductApp(feedbackProductModel, Id);
                feedbackProductModel.Id = res;
            }
            return Json(new[] { feedbackProductModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProductApp([DataSourceRequest] DataSourceRequest request, FeedbackProductModel feedbackProductModel)
        {
            if (feedbackProductModel != null && feedbackProductModel.Product.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                feedbackProductModel.ClientId = Client.ClientId;
                feedbackProductModel.UpdatedBy = usid;
                var res = FMHelper.UpdateProductApp(feedbackProductModel);
            }
            return Json(new[] { feedbackProductModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteProductApp([DataSourceRequest] DataSourceRequest request, FeedbackProductModel feedbackProductModel)
        {
            if (feedbackProductModel != null)
            {
                var v = FMHelper.RemoveProductApp(feedbackProductModel.Id);
            }
            return Json(new[] { feedbackProductModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult GetDigitalOff([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(FMHelper.GetDigitalOfferDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddDigitalOff(FeedbackDigitalOfferModel feedbackDigitalOfferModel, int Id)
        {

            if (feedbackDigitalOfferModel != null && feedbackDigitalOfferModel.DigitalOffering.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                feedbackDigitalOfferModel.ClientId = Client.ClientId;
                feedbackDigitalOfferModel.CreatedBy = usid;
                var res = FMHelper.CreateDigitalOffer(feedbackDigitalOfferModel, Id);
                feedbackDigitalOfferModel.Id = res;
            }
            return Json(new[] { feedbackDigitalOfferModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDigitalOff([DataSourceRequest] DataSourceRequest request, FeedbackDigitalOfferModel feedbackDigitalOfferModel)
        {
            if (feedbackDigitalOfferModel != null && feedbackDigitalOfferModel.DigitalOffering.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                feedbackDigitalOfferModel.ClientId = Client.ClientId;
                feedbackDigitalOfferModel.UpdatedBy = usid;
                var res = FMHelper.UpdateDigitalOffer(feedbackDigitalOfferModel);
            }
            return Json(new[] { feedbackDigitalOfferModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDigitalOff([DataSourceRequest] DataSourceRequest request, FeedbackDigitalOfferModel feedbackDigitalOfferModel)
        {
            if (feedbackDigitalOfferModel != null)
            {
                var v = FMHelper.RemoveDigitalOffer(feedbackDigitalOfferModel.Id);
            }
            return Json(new[] { feedbackDigitalOfferModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult GetInternClient([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(FMHelper.GetInternClientDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddInternClient(FeedbackInternClientModel feedbackInternClientModel, int Id)
        {

            if (feedbackInternClientModel != null && feedbackInternClientModel.Intern_Client.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                feedbackInternClientModel.ClientId = Client.ClientId;
                feedbackInternClientModel.CreatedBy = usid;
                var res = FMHelper.CreateInternClient(feedbackInternClientModel, Id);
                feedbackInternClientModel.Id = res;
            }
            return Json(new[] { feedbackInternClientModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditInternClient([DataSourceRequest] DataSourceRequest request, FeedbackInternClientModel feedbackInternClientModel)
        {
            if (feedbackInternClientModel != null && feedbackInternClientModel.Intern_Client.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                feedbackInternClientModel.ClientId = Client.ClientId;
                feedbackInternClientModel.UpdatedBy = usid;
                var res = FMHelper.UpdateInternClient(feedbackInternClientModel);
            }
            return Json(new[] { feedbackInternClientModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteInternClient([DataSourceRequest] DataSourceRequest request, FeedbackInternClientModel feedbackInternClientModel)
        {
            if (feedbackInternClientModel != null)
            {
                var v = FMHelper.RemoveInternClient(feedbackInternClientModel.Id);
            }
            return Json(new[] { feedbackInternClientModel }.ToDataSourceResult(request, ModelState));
        }

    }
}