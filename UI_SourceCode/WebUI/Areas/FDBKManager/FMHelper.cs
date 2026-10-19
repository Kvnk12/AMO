using Data;
using Domain.Entities;
using Domain.Entities.FDBKManager;
using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebUI.Areas.FDBKManager.Models;
using log4net;
using System.IO;
using WebUI.Areas.FDBKManager.Controllers;
using WebUI.Areas.IdeaManager.Controllers;
using WebUI.Areas.IdeaManager.Dto;
using Kendo.Mvc.UI;

namespace WebUI.Areas.FDBKManager
{
    public class FMHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FMHelper));
        public static IList<FeedbackTypeModel> GetfeedbackTypeDetails(int ClientId, int uid)
        {

            IList<FeedbackTypeModel> FeedbackType = new List<FeedbackTypeModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMFDBKFeedbackType> FeedbackTypeDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    FeedbackTypeDetails = unitOfWork.FeedbackType.Get(cid => cid.ClientId == ClientId).ToList();

                if (FeedbackTypeDetails != null)
                {
                    foreach (var _FeedbackTypedetail in FeedbackTypeDetails)
                    {
                        FeedbackTypeModel cs = new FeedbackTypeModel();
                        cs.Id = _FeedbackTypedetail.Id;
                        cs.ClientId = _FeedbackTypedetail.ClientId;
                        cs.FeedbackType = _FeedbackTypedetail.FeedbackType;
                        cs.CreatedBy = _FeedbackTypedetail.CreatedBy;
                        cs.UpdatedBy = _FeedbackTypedetail.UpdatedBy;
                        cs.CreatedDate = _FeedbackTypedetail.CreatedDate;
                        cs.UpdatedBy = _FeedbackTypedetail.UpdatedBy;
                        FeedbackType.Add(cs);
                    }
                }
                return FeedbackType;
            }

        }
        public static int CreateFeedbackType(FeedbackTypeModel FeedbackTypeModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var FeedbackType = new IMFDBKFeedbackType();
                    FeedbackType.ClientId = FeedbackTypeModel.ClientId;
                    FeedbackType.FeedbackType = FeedbackTypeModel.FeedbackType;
                    FeedbackType.CreatedDate = DateTime.Now;
                    FeedbackType.CreatedBy = FeedbackTypeModel.CreatedBy;
                    unitOfWork.FeedbackType.Insert(FeedbackType);
                    unitOfWork.Save();
                    id = FeedbackType.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateFeedbackType(FeedbackTypeModel FeedbackTypeModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.FeedbackType.GetByID(FeedbackTypeModel.Id);
                    updatetitem.ClientId = FeedbackTypeModel.ClientId;
                    updatetitem.FeedbackType = FeedbackTypeModel.FeedbackType;
                    updatetitem.UpdatedBy = FeedbackTypeModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.FeedbackType.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveFeedbackType(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.FeedbackType.Delete(Id);
                unitOfWork.Save();

                return true;
            }
        }
        public static IEnumerable<DropDownTreeItemModel> getproductapps(int ClientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            List<DropDownTreeItemModel> cc = new List<DropDownTreeItemModel>();
            var CType = unitOfWork.IMFDBKProductGroups.Get().Where(r => r.ClientId == ClientId && r.IsDeleted == false).OrderBy(r => r.ProductGroup);
            if (CType != null)
            {
                foreach (var C in CType)
                {
                    DropDownTreeItemModel ctype = new DropDownTreeItemModel();
                    ctype.Text = C.ProductGroup;
                    ctype.Id = C.Id.ToString();
                    ctype.Enabled = false;
                    ctype.Expanded = true;
                    var configItems = unitOfWork.FeedbackProduct.Get().Where(e => e.ClientId == ClientId && e.productGroupid == C.Id && e.ParentId == 0).OrderBy(e => e.Product);
                    if (configItems.Count() != 0)
                    {
                        ctype.HasChildren = true;
                        List<DropDownTreeItemModel> ConfigIt = new List<DropDownTreeItemModel>();
                        foreach (var Ci in configItems)
                        {
                            DropDownTreeItemModel ConfItem = new DropDownTreeItemModel();
                            ConfItem.Id = Ci.Id.ToString();
                            ConfItem.Text = Ci.Product;
                            ConfItem.Expanded = true;
                            var childItems = unitOfWork.FeedbackProduct.Get().Where(e => e.ClientId == ClientId && e.productGroupid == C.Id && e.ParentId == Ci.Id).OrderBy(e => e.Product);
                            if (childItems.Count() != 0)
                            {
                                ConfItem.HasChildren = true;
                                List<DropDownTreeItemModel> childList = new List<DropDownTreeItemModel>();
                                foreach (var ii in childItems)
                                {
                                    DropDownTreeItemModel dr = new DropDownTreeItemModel();
                                    dr.Text = ii.Product;
                                    dr.Id = ii.Id.ToString();
                                    ConfItem.Expanded = true;
                                    childList.Add(dr);
                                }
                                ConfItem.Items = childList;
                            }
                            ConfigIt.Add(ConfItem);
                        }
                        ctype.Items = ConfigIt;
                    }
                    cc.Add(ctype);
                }

            }
            return cc;

        }
        public static IList<FeedbackProductModel> GetProductAppDetails(int ClientId, int uid)
        {

            IList<FeedbackProductModel> ProductApp = new List<FeedbackProductModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMFDBKProductApp> ProductAppDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    ProductAppDetails = unitOfWork.FeedbackProduct.Get(cid => cid.ClientId == ClientId).ToList();

                if (ProductAppDetails != null)
                {
                    foreach (var _ProductAppdetail in ProductAppDetails)
                    {
                        FeedbackProductModel cs = new FeedbackProductModel();
                        cs.Id = _ProductAppdetail.Id;
                        cs.ClientId = _ProductAppdetail.ClientId;
                        cs.Product = _ProductAppdetail.Product;
                        cs.CreatedBy = _ProductAppdetail.CreatedBy;
                        cs.UpdatedBy = _ProductAppdetail.UpdatedBy;
                        cs.CreatedDate = _ProductAppdetail.CreatedDate;
                        cs.UpdatedBy = _ProductAppdetail.UpdatedBy;
                        ProductApp.Add(cs);
                    }
                }
                return ProductApp;
            }

        }
        public static int CreateProductApp(FeedbackProductModel FeedbackProductModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var ProductApp = new IMFDBKProductApp();
                    ProductApp.ClientId = FeedbackProductModel.ClientId;
                    ProductApp.Product = FeedbackProductModel.Product;
                    ProductApp.CreatedDate = DateTime.Now;
                    ProductApp.CreatedBy = FeedbackProductModel.CreatedBy;
                    unitOfWork.FeedbackProduct.Insert(ProductApp);
                    unitOfWork.Save();
                    id = ProductApp.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateProductApp(FeedbackProductModel FeedbackProductModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.FeedbackProduct.GetByID(FeedbackProductModel.Id);
                    updatetitem.ClientId = FeedbackProductModel.ClientId;
                    updatetitem.Product = FeedbackProductModel.Product;
                    updatetitem.UpdatedBy = FeedbackProductModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.FeedbackProduct.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveProductApp(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.FeedbackProduct.Delete(Id);
                unitOfWork.Save();

                return true;
            }
        }
        public static IList<FeedbackDigitalOfferModel> GetDigitalOfferDetails(int ClientId, int uid)
        {

            IList<FeedbackDigitalOfferModel> DigitalOffering = new List<FeedbackDigitalOfferModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMFDBKDigitalOffering> DigitalOfferingDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    DigitalOfferingDetails = unitOfWork.FeedbackDigital.Get(cid => cid.ClientId == ClientId).ToList();

                if (DigitalOfferingDetails != null)
                {
                    foreach (var _DigitalOfferingdetail in DigitalOfferingDetails)
                    {
                        FeedbackDigitalOfferModel cs = new FeedbackDigitalOfferModel();
                        cs.Id = _DigitalOfferingdetail.Id;
                        cs.ClientId = _DigitalOfferingdetail.ClientId;
                        cs.DigitalOffering = _DigitalOfferingdetail.DigitalOffering;
                        cs.CreatedBy = _DigitalOfferingdetail.CreatedBy;
                        cs.UpdatedBy = _DigitalOfferingdetail.UpdatedBy;
                        cs.CreatedDate = _DigitalOfferingdetail.CreatedDate;
                        cs.UpdatedBy = _DigitalOfferingdetail.UpdatedBy;
                        DigitalOffering.Add(cs);
                    }
                }
                return DigitalOffering;
            }

        }
        public static int CreateDigitalOffer(FeedbackDigitalOfferModel FeedbackDigitalOfferModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var DigitalOffering = new IMFDBKDigitalOffering();
                    DigitalOffering.ClientId = FeedbackDigitalOfferModel.ClientId;
                    DigitalOffering.DigitalOffering = FeedbackDigitalOfferModel.DigitalOffering;
                    DigitalOffering.CreatedDate = DateTime.Now;
                    DigitalOffering.CreatedBy = FeedbackDigitalOfferModel.CreatedBy;
                    unitOfWork.FeedbackDigital.Insert(DigitalOffering);
                    unitOfWork.Save();
                    id = DigitalOffering.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateDigitalOffer(FeedbackDigitalOfferModel FeedbackDigitalOfferModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.FeedbackDigital.GetByID(FeedbackDigitalOfferModel.Id);
                    updatetitem.ClientId = FeedbackDigitalOfferModel.ClientId;
                    updatetitem.DigitalOffering = FeedbackDigitalOfferModel.DigitalOffering;
                    updatetitem.UpdatedBy = FeedbackDigitalOfferModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.FeedbackDigital.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveDigitalOffer(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.FeedbackDigital.Delete(Id);
                unitOfWork.Save();

                return true;
            }
        }
        public static IList<FeedbackInternClientModel> GetInternClientDetails(int ClientId, int uid)
        {

            IList<FeedbackInternClientModel> InternClient = new List<FeedbackInternClientModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<InternalClient> InternClientDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    InternClientDetails = unitOfWork.InternalClient.Get(cid => cid.ClientId == ClientId).ToList();

                if (InternClientDetails != null)
                {
                    foreach (var _InternClientdetail in InternClientDetails)
                    {
                        FeedbackInternClientModel cs = new FeedbackInternClientModel();
                        cs.Id = _InternClientdetail.Id;
                        cs.ClientId = _InternClientdetail.ClientId;
                        cs.Intern_Client = _InternClientdetail.InternalClientName;
                        cs.CreatedBy = _InternClientdetail.CreatedBy;
                        cs.UpdatedBy = _InternClientdetail.UpdatedBy;
                        cs.CreatedDate = _InternClientdetail.CreatedDate;
                        cs.UpdatedBy = _InternClientdetail.UpdatedBy;
                        InternClient.Add(cs);
                    }
                }
                return InternClient;
            }

        }
        public static int CreateInternClient(FeedbackInternClientModel FeedbackInternClientModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var InternClient = new InternalClient();
                    InternClient.ClientId = FeedbackInternClientModel.ClientId;
                    InternClient.InternalClientName = FeedbackInternClientModel.Intern_Client;
                    InternClient.CreatedDate = DateTime.Now;
                    InternClient.CreatedBy = FeedbackInternClientModel.CreatedBy;
                    unitOfWork.InternalClient.Insert(InternClient);
                    unitOfWork.Save();
                    id = InternClient.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateInternClient(FeedbackInternClientModel FeedbackInternClientModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.InternalClient.GetByID(FeedbackInternClientModel.Id);
                    updatetitem.ClientId = FeedbackInternClientModel.ClientId;
                    updatetitem.InternalClientName = FeedbackInternClientModel.Intern_Client;
                    updatetitem.UpdatedBy = FeedbackInternClientModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.InternalClient.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveInternClient(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.InternalClient.Delete(Id);
                unitOfWork.Save();

                return true;
            }
        }
        public static int saveFeedback(int Typeid, int? Productid, int? Digitalid, string ShortDesc, string DetailDesc, string ItrCl, string BtnVal, int uid, int clientid, int? feedbackid, string IdeaLink, string ApproveLink, string OriginatedFrom = null, string sentimentName = "")
        {
            int Feedbackid;
            int statusId;
            int statuslist = 0;
            string ProDig = "";
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (feedbackid == 0)
            {
                IMFDBKPost fdpost = new IMFDBKPost();
                {
                    fdpost.Typeid = Typeid;
                    if (Typeid == 1)
                    {
                        fdpost.Productid = Productid;
                        ProDig = unitOfWork.FeedbackProduct.GetByID(Productid).Product;
                    }
                    else
                    {
                        fdpost.Digitalid = Digitalid;
                        ProDig = unitOfWork.FeedbackDigital.GetByID(Digitalid).DigitalOffering;
                    }
                    fdpost.ShortDescription = ShortDesc;
                    fdpost.DetailDescription = DetailDesc;
                    fdpost.CreatedBy = uid;
                    //fdpost.isDeleted = false;
                    fdpost.CreatedDate = DateTime.Now;
                    fdpost.LastUpdatedDate = DateTime.Now;
                    fdpost.UpdatedBy = uid;
                    fdpost.ClientId = clientid;
                    fdpost.OriginatedFrom = OriginatedFrom;
                    if (sentimentName != "")
                    {
                        Db db = new Db();
                        var sentval = (from a in db.IMFDBKSentiment
                                       where a.SentimentName == sentimentName
                                       select a.Id).FirstOrDefault();
                        fdpost.FdbkSentiment = sentval;
                    }
                };
                unitOfWork.IMFDBKPost.Insert(fdpost);
                unitOfWork.Save();
                Feedbackid = fdpost.Id;
                IdeaLink = IdeaLink + "/?viewtyp=" + Feedbackid;
                ApproveLink = ApproveLink + "/?viewtyp=" + Feedbackid;
                var isnew = true;
                savefeedbackInternalclient(unitOfWork, Feedbackid, ItrCl, uid, isnew);
                if (BtnVal == "Draft")
                {
                    statusId = (int)IMFeedbackStatus.Draft;
                    savefeedbackStatus(unitOfWork, Feedbackid, statusId, statuslist, uid);
                }
                else if (BtnVal == "Post")
                {
                    statusId = (int)IMFeedbackStatus.Pending;
                    savefeedbackStatus(unitOfWork, Feedbackid, statusId, statuslist, uid);
                    SaveFeedbackApprover(unitOfWork, Feedbackid, ApproveLink, Typeid, Productid, Digitalid, uid, ProDig);
                    var u = unitOfWork.UserProfile.GetByID(uid);
                    if (u != null)
                    {
                        new ViewFeedbackController().SendPostFeedbackNotification(u.Email, u.FirstName, IdeaLink);
                    }

                }
                return Feedbackid;
            }
            else
            {
                var Existfdbk = unitOfWork.IMFDBKPost.GetByID(feedbackid);
                Existfdbk.Typeid = Typeid;
                if (Typeid == 1)
                {
                    Existfdbk.Productid = Productid;
                    ProDig = unitOfWork.FeedbackProduct.GetByID(Productid).Product;
                }
                else
                {
                    Existfdbk.Digitalid = Digitalid;
                    ProDig = unitOfWork.FeedbackDigital.GetByID(Digitalid).DigitalOffering;
                }
                Existfdbk.ShortDescription = ShortDesc;
                Existfdbk.DetailDescription = DetailDesc;
                Existfdbk.UpdatedBy = uid;
                Existfdbk.LastUpdatedDate = DateTime.Now;
                Existfdbk.ClientId = clientid;
                unitOfWork.IMFDBKPost.Update(Existfdbk);
                unitOfWork.Save();
                var isnew = false;
                savefeedbackInternalclient(unitOfWork, Existfdbk.Id, ItrCl, uid, isnew);
                statuslist = unitOfWork.IMFDBKStatus.Get(e => e.IMFDBKId == Existfdbk.Id).First().StatusId;
                if (BtnVal == "Draft")
                {
                    statusId = (int)IMFeedbackStatus.Draft;
                    savefeedbackStatus(unitOfWork, Existfdbk.Id, statusId, statuslist, uid);
                }
                else if (BtnVal == "Post")
                {
                    statusId = (int)IMFeedbackStatus.Pending;
                    savefeedbackStatus(unitOfWork, Existfdbk.Id, statusId, statuslist, uid);
                    SaveFeedbackApprover(unitOfWork, Existfdbk.Id, ApproveLink, Typeid, Productid, Digitalid, uid, ProDig);
                    var u = unitOfWork.UserProfile.GetByID(uid);
                    if (u != null)
                    {
                        new ViewFeedbackController().SendPostFeedbackNotification(u.Email, u.FirstName, IdeaLink);
                    }
                }
                return Existfdbk.Id;
            }
        }
        public static bool SaveFeedbackApprover(UnitOfWorks unitOfWork, int fdbkId, string IdeaLink, int Typeid, int? Productid = 0, int? Digitalid = 0, int? uid = 0, string ProdDigName = "")
        {
            if (Typeid == 1)
            {
                var PdtapproversList = unitOfWork.IMFDBKProductMgr.Get().Where(f => f.ProductId == Productid && f.IsDeleted != true);
                foreach (var item in PdtapproversList)
                {
                    IMFDBKUser fdApprove = new IMFDBKUser();
                    fdApprove.IMFDBKId = fdbkId;
                    fdApprove.UserId = (int)item.UserId;
                    fdApprove.UserRoleId = item.RoleId;
                    //fdApprove.UserAction = (int)IMFeedbackStatus.Pending;
                    fdApprove.CreatedBy = uid;
                    fdApprove.CreatedDate = DateTime.Now;
                    unitOfWork.IMFDBKUser.Insert(fdApprove);
                    unitOfWork.Save();
                    var ApproverId = unitOfWork.UserProfile.GetByID((int)item.UserId);
                    var CreatorId = unitOfWork.UserProfile.GetByID(uid);
                    if (ApproverId != null)
                    {
                        new ViewFeedbackController().SendNotifyFeedbackNotification(ApproverId.Email, CreatorId.FirstName, ApproverId.FirstName, ProdDigName, IdeaLink);
                    }
                }
            }
            else if (Typeid == 2)
            {
                var DgtlapproversList = unitOfWork.IMFDBKDgtlOfferingMgr.Get().Where(f => f.DgtlOfferingId == Digitalid && f.IsDeleted != true);
                foreach (var item in DgtlapproversList)
                {
                    IMFDBKUser fdApprove = new IMFDBKUser();
                    fdApprove.IMFDBKId = fdbkId;
                    fdApprove.UserId = (int)item.UserId;
                    fdApprove.UserRoleId = item.RoleId;
                    //// fdApprove.UserAction = (int)IMFeedbackStatus.Pending;
                    fdApprove.CreatedBy = uid;
                    fdApprove.CreatedDate = DateTime.Now;
                    unitOfWork.IMFDBKUser.Insert(fdApprove);
                    unitOfWork.Save();
                    var ApproverId = unitOfWork.UserProfile.GetByID((int)item.UserId);
                    var CreatorId = unitOfWork.UserProfile.GetByID(uid);
                    if (ApproverId != null)
                    {
                        new ViewFeedbackController().SendNotifyFeedbackNotification(ApproverId.Email, CreatorId.FirstName, ApproverId.FirstName, ProdDigName, IdeaLink);
                    }
                }
            }
            return true;
        }
        public static bool savefeedbackInternalclient(UnitOfWorks unitOfWork, int Feedbackid, string ItrCl = "", int? uid = 0, bool? isnew = false)
        {
            if (isnew == false)
            {
                var list = unitOfWork.IMFDBKClient.Get(e => e.IMFDBKId == Feedbackid);
                if (list != null)
                {
                    foreach (var cateory in list)
                    {
                        unitOfWork.IMFDBKClient.Delete(cateory.Id);
                    }
                    unitOfWork.Save();
                }

            }
            IMFDBKClient intCl = new IMFDBKClient();
            String[] temp = ItrCl.Split('#');
            foreach (var item in temp)
            {
                if (item != "")
                {
                    intCl.IMFDBKId = Feedbackid;
                    intCl.InternalClientId = Convert.ToInt32(item);
                    intCl.isDeleted = false;
                    intCl.CreatedBy = uid;
                    intCl.CreatedDate = DateTime.Now;
                    unitOfWork.IMFDBKClient.Insert(intCl);
                    unitOfWork.Save();
                }
            }

            return true;
        }
        public static int SaveFeedbackComment(int fdbkid, int uid, string comment, int ClientID)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IMFDBKComment fdCmt = new IMFDBKComment();
            fdCmt.ImfdbkId = fdbkid;
            fdCmt.ClientId = ClientID;
            fdCmt.CommentedBy = uid;
            fdCmt.Comments = comment;
            var b = unitOfWork.UserProfile.GetByID(uid);
            fdCmt.CommentedByName = b.FirstName + " " + b.LastName;
            fdCmt.CommentedOn = DateTime.Now;
            unitOfWork.IMFDBKComment.Insert(fdCmt);
            unitOfWork.Save();
            return fdCmt.Id;
        }
        public static bool savefeedbackStatus(UnitOfWorks unitOfWork, int Feedbackid, int statusId, int PrevStatus, int? uid = 0)
        {
            IMFDBKStatus fdstatus = new IMFDBKStatus();
            fdstatus.IMFDBKId = Feedbackid;
            fdstatus.StatusId = statusId;
            fdstatus.PrevStatusId = PrevStatus;
            fdstatus.CreatedBy = uid;
            fdstatus.CreatedDate = DateTime.Now;
            unitOfWork.IMFDBKStatus.Insert(fdstatus);
            unitOfWork.Save();
            return true;
        }
        public static IEnumerable<searchKeyFeedback> GetAllFeedbackForUser(int Typeid, int? Productid, int? Digitalid, string inputString, int ClientId, int Userid)
        {
            IList<searchKeyFeedback> Clusterlist = new List<searchKeyFeedback>();
            _ = (Typeid == 1) ? Digitalid = 0 : Productid = 0;
            string[] whitespace = new string[] { " the "," in ", " on ", " a ", " an ", " with ", " is ", " was ", " were ",
                " and ", " because "," for "," or ","so"," but "," yet "," if ", " though "," - "," _ " };
            foreach (var c in whitespace)
            {
                inputString = inputString.Replace(c, "");
            }
            inputString = inputString.Replace(" ", "#");
            inputString += "#";
            var paramName = new string[] { "TypeId", "ProductId", "DigitalId", "inputString", "ClientId", "Userid" };
            var paramValue = new object[] { Typeid, Productid, Digitalid, inputString, ClientId, Userid };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("IMKDBK_Pro_SearchKeyword", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    try
                    {
                        var coldet = new searchKeyFeedback();
                        coldet.UniqueId = Convert.ToInt32(dr["Id"]);
                        coldet.creatorName = dr["firstname"].ToString() + " " + dr["lastname"].ToString();
                        coldet.CreatedBy = Convert.ToInt32(dr["CreatedBy"]);
                        coldet.shortDesc = dr["ShortDescription"].ToString();
                        coldet.breif = dr["DetailDescription"].ToString();
                        if (!(dr["votestatus"] is DBNull))
                        {
                            coldet.Votestatus = Convert.ToBoolean(dr["votestatus"]);
                        }
                        if (!(dr["LastUpdatedDate"] is DBNull))
                        {
                            coldet.LastUpdatedDate = Convert.ToDateTime(dr["LastUpdatedDate"]);
                        }
                        if (!(dr["CreatedDate"] is DBNull))
                        {
                            coldet.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }
                        if (!(dr["votecount"] is DBNull))
                        {
                            coldet.voteCount = Convert.ToInt32(dr["votecount"]);
                        }

                        Clusterlist.Add(coldet);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("FMHelper error 'GetAllFeedbackForUser' ", ex);
                    }

                    foreach (var item in Clusterlist)
                    {
                        if (item.LastUpdatedDate == null)
                        {
                            item.LastUpdatedDate = item.CreatedDate;
                        }
                        DateTime Today = DateTime.Now;
                        TimeSpan t = Today.Subtract((DateTime)item.LastUpdatedDate);

                        item.LastActivity = getLastActivity(t);

                    }

                }
                return Clusterlist;
            }
        }
        public static string getLastActivity(TimeSpan t)
        {
            int count;
            string LastActivity = "";
            if (t.Days > 365)
            {
                count = t.Days / 365;
                LastActivity = (count == 1) ? (count + " year ago") : (count + " years ago");
            }
            else if (t.Days >= 31)
            {
                count = t.Days / 31;
                LastActivity = (count == 1) ? (count + " month ago") : (count + " months ago");
            }
            else if (t.Days >= 7)
            {
                count = t.Days / 7;
                LastActivity = (count == 1) ? (count + " week ago") : (count + " weeks ago");
            }
            else if (t.Days < 7 && t.Days != 0)
            {
                count = t.Days;
                LastActivity = (count == 1) ? (count + " day ago") : (count + " days ago");
            }
            else if (t.Days == 0 && t.Hours > 0)
            {
                count = t.Hours;
                LastActivity = (count == 1) ? (count + " hour ago") : (count + " hours ago");
            }
            else if (t.Hours == 0 && t.Days == 0 && t.Minutes > 0)
            {
                count = t.Minutes;
                LastActivity = (count <= 1) ? ("1 minute ago") : (count + " Minutes ago");
            }
            else if (t.Hours == 0 && t.Days == 0 && t.Minutes == 0)
            {
                count = t.Seconds;
                LastActivity = (count <= 1) ? ("1 second ago") : (count + " seconds ago");
            }
            else
            {
                count = t.Days;
                LastActivity = (count == 1) ? (count + " day ago") : (count + " days ago");
            }
            return LastActivity;
        }
        public static int SaveFeedbackUserVote(int Voterid, int userid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var isvoted = unitOfWork.IMFDBKVote.Get(cid => cid.IMFDBKId == Voterid && cid.CreatedBy == userid).FirstOrDefault();
            if (isvoted == null)
            {
                IMFDBKVote fdvote = new IMFDBKVote();
                {
                    fdvote.IMFDBKId = Voterid;
                    fdvote.Vote = true;
                    fdvote.CreatedBy = userid;
                    fdvote.CreatedDate = DateTime.Now;
                };
                unitOfWork.IMFDBKVote.Insert(fdvote);
                unitOfWork.Save();
            }
            else
            {
                if (isvoted.Vote == true)
                {
                    isvoted.Vote = false;
                }
                else
                {
                    isvoted.Vote = true;
                }

                isvoted.CreatedBy = userid;
                isvoted.CreatedDate = DateTime.Now;
                unitOfWork.IMFDBKVote.Update(isvoted);
                unitOfWork.Save();

                var fdbkDtl3 = unitOfWork.IMFDBKPost.Get(a => a.Id == Voterid).FirstOrDefault();
                fdbkDtl3.LastUpdatedDate = DateTime.Now;
                fdbkDtl3.UpdatedBy = userid;
                unitOfWork.IMFDBKPost.Update(fdbkDtl3);
                unitOfWork.Save();
            }
            var voteCnt = unitOfWork.IMFDBKVote.Get(cid => cid.IMFDBKId == Voterid && cid.Vote == true).Count();
            return voteCnt;
        }

        public static bool UpdateFdbkStatus(int fdbkId, int status, string cmt, int uid, string IdeaLink)
        {
            var statusstring = "";
            var ProdDigital = "";
            IdeaLink = IdeaLink + "/?viewtyp=" + fdbkId;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var fddetail = unitOfWork.IMFDBKPost.GetByID(fdbkId);
                var Creater = unitOfWork.UserProfile.GetByID(fddetail.CreatedBy);
                if (fddetail != null && fddetail.Productid != null)
                {
                    ProdDigital = unitOfWork.FeedbackProduct.GetByID(fddetail.Productid).Product;
                }
                else if (fddetail != null && fddetail.Digitalid != null)
                {
                    ProdDigital = unitOfWork.FeedbackDigital.GetByID(fddetail.Digitalid).DigitalOffering;
                }

                var fdbkDtl = unitOfWork.IMFDBKUser.Get(a => a.IMFDBKId == fdbkId && a.UserId == uid).FirstOrDefault();
                fdbkDtl.UserAction = status;
                if (status == (int)IMFeedbackStatus.Approved)
                {
                    statusstring = "Approved";
                }
                else if (status == (int)IMFeedbackStatus.Rejected)
                {
                    statusstring = "Rejected";
                }
                fdbkDtl.ActionComment = cmt;
                fdbkDtl.UpdatedDate = DateTime.Now;
                unitOfWork.IMFDBKUser.Update(fdbkDtl);
                unitOfWork.Save();
                var fdbkDtl2 = unitOfWork.IMFDBKUser.Get(a => a.IMFDBKId == fdbkId);
                var fdbkstatus = unitOfWork.IMFDBKStatus.Get(a => a.IMFDBKId == fdbkId).OrderByDescending(a => a.Id).FirstOrDefault();

                IMFDBKStatus fdstatus = new IMFDBKStatus();
                fdstatus.IMFDBKId = fdbkId;
                fdstatus.PrevStatusId = fdbkstatus.StatusId;
                if (fdbkDtl2.All(i => i.UserAction == 3))
                {
                    fdstatus.StatusId = (int)IMFeedbackStatus.Approved;
                }
                else if (fdbkDtl2.All(i => i.UserAction == 4))
                {
                    fdstatus.StatusId = (int)IMFeedbackStatus.Rejected;
                }
                else if (fdbkDtl2.Any(a => a.UserAction == 3) && fdbkDtl2.Any(a => a.UserAction == 4))
                {
                    fdstatus.StatusId = (int)IMFeedbackStatus.PartiallyApproved;
                }
                else if (fdbkDtl2.Any(a => a.UserAction == null))
                {
                    fdstatus.StatusId = (int)IMFeedbackStatus.Pending;
                }
                fdstatus.CreatedBy = uid;
                fdstatus.CreatedDate = DateTime.Now;
                unitOfWork.IMFDBKStatus.Insert(fdstatus);
                unitOfWork.Save();
                var fdbkDtl3 = unitOfWork.IMFDBKPost.Get(a => a.Id == fdbkId).FirstOrDefault();
                fdbkDtl3.LastUpdatedDate = DateTime.Now;
                fdbkDtl3.UpdatedBy = uid;
                unitOfWork.IMFDBKPost.Update(fdbkDtl3);
                unitOfWork.Save();
                var voteCount = unitOfWork.IMFDBKVote.Get(e => e.IMFDBKId == fdbkId).Count();
                if (Creater != null)
                {
                    if (statusstring == "Approved")
                    {
                        new ViewFeedbackController().SendApprovedFeedbackNotification(Creater.Email, Creater.FirstName, voteCount, ProdDigital, IdeaLink);
                    }
                    else if (statusstring == "Rejected")
                    {
                        new ViewFeedbackController().SendRejectedFeedbackNotification(Creater.Email, Creater.FirstName, IdeaLink);
                    }


                }

                return true;
            }
        }
        public static int CheckForUser(string userMail, int cid)
        {
            Db db = new Db();
            var userid = (from a in db.UserProfiles
                          join b in db.UserApplications on a.Id equals b.UserId
                          where a.Email == userMail && b.ApplicationId == 19
                          select a.Id).FirstOrDefault();
            return userid;
        }

        public static int CheckForUser(string userMail)
        {
            Db db = new Db();
            var userid = (from a in db.UserProfiles
                          where a.Email == userMail
                          select a.Id).FirstOrDefault();
            return userid;
        }

        public static int SaveFdbkTag(int fdbkid, int uid, string tagname, int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tagdtl = unitOfWork.IMFDBKTag.Get(a => a.TagName == tagname && a.ClientId == ClientId).FirstOrDefault();
            int tagid = 0;
            int restyp = 0;
            if (tagdtl == null)
            {
                IMFDBKTag fdtag = new IMFDBKTag();
                fdtag.TagName = tagname;
                fdtag.ClientId = ClientId;
                fdtag.CreatedBy = uid;
                fdtag.CreatedDate = DateTime.Now;

                unitOfWork.IMFDBKTag.Insert(fdtag);
                unitOfWork.Save();
                tagid = fdtag.Id;
                restyp = tagid;
            }
            else
            {
                tagid = tagdtl.Id;
            }

            IMFDBKMapTag fdmaptag = new IMFDBKMapTag();
            fdmaptag.FdbkId = fdbkid;
            fdmaptag.Tagid = tagid;
            fdmaptag.ClientId = ClientId;
            fdmaptag.CreatedBy = uid;
            fdmaptag.CreatedDate = DateTime.Now;

            unitOfWork.IMFDBKMapTag.Insert(fdmaptag);
            unitOfWork.Save();

            return restyp;
        }
        public static int RemoveFdbkTag(int fdbkid, string tagname)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tagdtl = unitOfWork.IMFDBKTag.Get(a => a.TagName == tagname).FirstOrDefault();
            var mappedTag = unitOfWork.IMFDBKMapTag.Get(a => a.Tagid == tagdtl.Id && a.FdbkId == fdbkid).FirstOrDefault();
            unitOfWork.IMFDBKMapTag.Delete(mappedTag.Id);
            unitOfWork.Save();
            return mappedTag.Id;
        }
        public static void LogFeedbackActivity(string ActionName, string ActionDetail, int clientid, int userId)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                IMFDBKActivityLogs lg = new IMFDBKActivityLogs();
                lg.ActionName = ActionName;
                lg.ActionDetail = ActionDetail;
                lg.clientId = clientid;
                lg.CreatedBy = userId;
                lg.CreatedDate = DateTime.Now;
                unitOfWork.IMFDBKActivityLogs.Insert(lg);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                Log.Error("Error in Logging Activity: LogFeedbackActivity ", ex);
                throw ex;
            }

        }
        public static int UpdateUserNotification(int fdbkid, int notifyType, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var notiDtl = unitOfWork.IMFDBKNotification.Get(a => a.IMFDBKId == fdbkid).FirstOrDefault();
            if (notiDtl == null)
            {
                IMFDBKNotification fdnotify = new IMFDBKNotification();
                fdnotify.IMFDBKId = fdbkid;
                fdnotify.NotificationUser = uid;
                fdnotify.NotificationTypeId = notifyType;
                fdnotify.CreatedBy = uid;
                fdnotify.CreatedDate = DateTime.Now;
                unitOfWork.IMFDBKNotification.Insert(fdnotify);
                unitOfWork.Save();
                return fdnotify.Id;
            }
            else
            {
                notiDtl.NotificationTypeId = notifyType;
                unitOfWork.IMFDBKNotification.Update(notiDtl);
                unitOfWork.Save();
                return notiDtl.Id;
            }
        }

        public static void saveFdbkImage(string fdbkimg, int Fdbkid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            String[] temp = fdbkimg.Split('#');
            var image = "";
            foreach (var item in temp)
            {
                image = item.Replace("#", "");
                if (item != "")
                {
                    IMFDBKAttachment intCl = new IMFDBKAttachment();
                    intCl.ImfdbkId = Fdbkid;
                    intCl.Fdbkimg = image;
                    unitOfWork.IMFDBKAttachment.Insert(intCl);
                    unitOfWork.Save();
                }
            }

        }
        public static string GetUserNameOnEmail(string email)
        {
            Db db = new Db();
            var username = "";
            if (email == "amo")
            {
                username = "AMO";
            }
            else
            {
                var user = (from a in db.UserProfiles
                            where a.Email == email
                            select a).FirstOrDefault();
                username = user.FirstName + " " + user.LastName;
            }

            return username;
        }

    }
}