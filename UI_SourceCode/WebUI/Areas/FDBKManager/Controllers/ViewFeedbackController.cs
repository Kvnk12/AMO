using Data;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.FDBKManager.Models;
using WebUI.Dto;
using log4net;
//using WebUI.Controllers;
using NTTData.AMOPortal.WebUI.Filters;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Kendo.Mvc.Extensions;

namespace WebUI.Areas.FDBKManager.Controllers
{
    public class ViewFeedbackController : Controller
    {
        public static DataTable dtFeedback;
        public static StreamReader reader;
        public static string emailBody;
        public static string emailApproverBody;
        public static string eApprovedBody;
        public static string eRejectedBody;
        public static string IdeaLink;
        public static string ApproveLink;
        public static string Submitbase64String = null;
        public static string SubmitArrowbase64String = null;
        public static string NccLogobase64String = null;
        public static string FdbkLogobase64String = null;
        public static string Reviewbase64String = null;
        public static string Notifybase64String = null;
        public static string Rejectarrowbase64String = null;
        public static string Approverbase64String = null;
        public static string Approverarrowbase64String = null;
        // GET: FDBKManager/ViewFeedback
        private static readonly ILog Log = LogManager.GetLogger(typeof(ViewFeedbackController));
        [ActionTrack]
        public ActionResult Index(int viewtyp = 0)
        {
            loadImageforTemplate();
            FeedbackViewModel c = prepareCommonParameters();
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            //List<CommentList> cmtList = new List<CommentList>();
            //c.Comments = cmtList;
            //c.FdbkImages = new List<string>();
            FMHelper.LogFeedbackActivity("OpenFdbk manager", "OpenFdbk", client.ClientId, uid);
            ViewBag.ProductItems = FMHelper.getproductapps(client.ClientId, uid);
            ViewBag.viewtyp = viewtyp;
            return View("ViewFdbk", c);
        }
        private void loadImageforTemplate()
        {
            IdeaLink = Url.Action("Index", "viewfeedback", new { area = "FdbkManager" }, "https");
            ApproveLink = Url.Action("ApproveFeedback", "viewfeedback", new { area = "FdbkManager" }, "https");

            reader = new StreamReader(Server.MapPath("~/Areas/FDBKManager/Templates/SubmitForApprovalEmailNotification.html"));
            emailBody = reader.ReadToEnd();
            reader = new StreamReader(Server.MapPath("~/Areas/FDBKManager/Templates/NotifyApproverEmailNotification.html"));
            emailApproverBody = reader.ReadToEnd();
            byte[] imageArray = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/ncc.png"));
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            Submitbase64String = "data:image/png;base64," + base64ImageRepresentation;
            imageArray = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/SubmittedforReview.png"));
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            SubmitArrowbase64String = "data:image/png;base64," + base64ImageRepresentation;
            imageArray = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/NccEmail.png"));
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            NccLogobase64String = "data:image/png;base64," + base64ImageRepresentation;
            imageArray = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/NccFdbk.png"));
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            FdbkLogobase64String = "data:image/png;base64," + base64ImageRepresentation;
            imageArray = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/Review.png"));
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            Reviewbase64String = "data:image/png;base64," + base64ImageRepresentation;
            imageArray = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/NotifyApprover.png"));
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            Notifybase64String = "data:image/png;base64," + base64ImageRepresentation;
            imageArray = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/Rejected.png"));
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            Rejectarrowbase64String = "data:image/png;base64," + base64ImageRepresentation;
            imageArray = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/ApprovedHead.png"));
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            Approverbase64String = "data:image/png;base64," + base64ImageRepresentation;
            imageArray = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/Approved.png"));
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            Approverarrowbase64String = "data:image/png;base64," + base64ImageRepresentation;

        }
        private FeedbackViewModel prepareCommonParameters()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            FeedbackViewModel mod = new FeedbackViewModel();
            GetFilterInfo(mod, client.ClientId);
            mod.Typeid = 1;
            GetListData(mod, client.ClientId, uid);
            return mod;
        }
        public static void GetFilterInfo(FeedbackViewModel mod, int ClientId)
        {
            try
            {
                var paramName = new string[] { "ClientId" };
                var paramValue = new object[] { ClientId };
                DataTable dt = new DataTable();

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DataSet ds = new DataSet();
                    ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllFilterforPostFeedbackInfo", paramName, paramValue);
                    List<SelectListItem> ls;
                    for (int t = 0; t < ds.Tables.Count; t++)
                    {
                        int i = 0;
                        ls = new List<SelectListItem>();
                        foreach (DataRow row in ds.Tables[t].Rows)
                        {
                            ls.Add(new SelectListItem() { Value = row.ItemArray[1].ToString(), Text = row.ItemArray[0].ToString() });
                            i++;
                        }
                        switch (t)
                        {
                            case 0:
                                mod.IntClient = ls;
                                break;
                            case 1:
                                mod.ProductList = ls;
                                break;
                            case 2:
                                mod.offeringList = ls;
                                break;
                            case 3:
                                mod.TagsList = ls;
                                break;
                        }

                    }
                    List<SelectListItem> statuslist = new List<SelectListItem>();
                    List<ProductFtr> productlist = new List<ProductFtr>();
                    Db db = new Db();
                    var prdGrpList = (from a in db.IMFDBKProductGroups
                                      where a.ClientId == ClientId && a.IsDeleted == false
                                      orderby a.ProductGroup
                                      select a).ToList();

                    foreach (var listItm in prdGrpList)
                    {
                        ProductFtr pdftrlist = new ProductFtr();
                        pdftrlist.grpId = listItm.Id;
                        pdftrlist.ProductGroupName = listItm.ProductGroup;
                        var prdlist = (from a in db.IMFDBKProductApp
                                       where a.productGroupid == listItm.Id && a.ParentId == 0 && a.ClientId == ClientId && a.IsDeleted == false
                                       orderby a.Product
                                       select a).ToList();
                        List<prdList> prdlist1 = new List<prdList>();
                        foreach (var item in prdlist)
                        {
                            prdList plist = new prdList();
                            plist.prdid = item.Id;
                            plist.product = item.Product;
                            prdlist1.Add(plist);

                        }
                        pdftrlist.prdListitem = prdlist1;
                        productlist.Add(pdftrlist);
                    }
                    mod.ProductFtr = productlist;
                    foreach (var item in Enum.GetNames(typeof(IMFeedbackStatus)))
                    {
                        string text = item;
                        int value = (int)Enum.Parse(typeof(IMFeedbackStatus), item);
                        statuslist.Add(new SelectListItem
                        {
                            Text = text,
                            Value = value.ToString()
                        });
                    }
                    mod.StatusList = statuslist;
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error("ViewFeedback controller GetFilterInfo Action error 'GetFilterInfo()' ", ex);
                throw ex;
            }
        }
        // [ActionTrack]
        public static void GetListData(FeedbackViewModel mod, int ClientId, int uid)
        {
            try
            {
                var paramName = new string[] { "ClientId", "UserId" };
                var paramValue = new object[] { ClientId, uid };
                DataTable dt = new DataTable();
                Db db = new Db();

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var res = unitOfWork.GetDataFromStoredProcedure("IMKDBK_Pro_FetchAllFeedback", paramName, paramValue);
                    dtFeedback = res;
                    mod.Viewfeedbacklist = GetFeedbackfilteredData(res, ClientId, mod.Typeid, "recentCreated", uid);
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error("ViewFeedback controller GetFilterInfo Action error 'GetFilterInfo()' ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        public ActionResult GetFilteredFeedback(string statusSel, string productSel, string digitalSel, string tagsSel, int typeid, string sortByVal, string selMode, string inputstring)
        {
            FeedbackViewModel mod = new FeedbackViewModel();
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int uid = (int)Session["UserId"];
                GetFilterInfo(mod, client.ClientId);
                //List<CommentList> cmtList = new List<CommentList>();
                //mod.Comments = cmtList;
                //mod.FdbkImages = new List<string>();
                string filter = "Typeid = '" + typeid + "'";
                String[] keyword = inputstring.Split(' ');

                String[] tags = tagsSel.Split('#');
                if (tagsSel != "")
                {
                    filter += "and (tagname Like '%" + tags[0] + "%' ";
                    for (int i = 1; i < tags.Length; i++)
                    {
                        if (tags[i] != " ")
                            filter += "or tagname Like '%" + tags[i] + "%' ";
                    }
                    filter += ")";
                }
                if (statusSel != null && statusSel.ToString().Length >= 1)
                    filter += " and Statusid  IN ('" + statusSel.Replace("#", "', '") + "') ";
                else
                    filter += " and Statusid = null";
                if (typeid == 1)
                {
                    if (productSel.Length > 2)
                        filter += " and Prodid IN ('" + productSel.Replace("#", "', '") + "') ";
                    else
                        filter += " and Prodid = '" + productSel + "'";
                }
                else
                {
                    if (digitalSel.Length > 2)
                        filter += " and Digitalid IN ('" + digitalSel.Replace("#", "', '") + "') ";
                    else
                        filter += " and Digitalid = '" + digitalSel + "'";
                }
                if (selMode == "user")
                {
                    filter += " and CreatedBy = '" + uid + "'";
                }
                if (inputstring != "")
                {
                    filter += "and (ShortDescription Like '%" + keyword[0] + "%' ";
                    for (int i = 1; i < keyword.Length; i++)
                    {
                        if (keyword[i] != " ")
                            filter += "or ShortDescription Like '%" + keyword[i] + "%' ";
                    }
                    filter += ")";
                }
                DataTable filteredDt = dtFeedback != null ? dtFeedback.Select(filter).CopyToDataTable() : null;
                if (filteredDt != null)
                {
                    mod.Viewfeedbacklist = GetFeedbackfilteredData(filteredDt, client.ClientId, typeid, sortByVal, uid, selMode);
                }
                else
                {
                    mod.Viewfeedbacklist = new List<ViewFeedbackList>();
                }
                //return Json(mod, JsonRequestBehavior.AllowGet);
                return PartialView("_ViewFdbkList", mod);
            }
            catch (Exception ex)
            {
                mod.Viewfeedbacklist = new List<ViewFeedbackList>();
                return PartialView("_ViewFdbkList", mod);
            }
        }
        [ActionTrack]
        public static List<ViewFeedbackList> GetFeedbackfilteredData(DataTable dt, int clientId, int typeid, string sortByVal, int uid, string selMode = "All")
        {
            List<ViewFeedbackList> prod_digList = new List<ViewFeedbackList>();
            List<ViewFeedbackList> fdbkList = new List<ViewFeedbackList>();

            Db db = new Db();
            if (typeid == 1)
            {
                prod_digList = (from a in db.IMFDBKProductApp
                                where a.ClientId == clientId && a.ParentId == 0
                                select new ViewFeedbackList
                                {
                                    Prod_DigitalName = a.Product,
                                    Prod_DigiId = a.Id.ToString()
                                }).ToList();
            }
            else
            {
                prod_digList = (from a in db.IMFDBKDigitalOffering
                                where a.ClientId == clientId
                                select new ViewFeedbackList
                                {
                                    Prod_DigitalName = a.DigitalOffering,
                                    Prod_DigiId = a.Id.ToString()
                                }).ToList();

            }
            foreach (var item in prod_digList)//product1
            {
                ViewFeedbackList viewlist = new ViewFeedbackList();
                List<searchKeyFeedback> allList = new List<searchKeyFeedback>();
                foreach (DataRow dr in dt.Rows)
                {
                    searchKeyFeedback listfdbk = new searchKeyFeedback();
                    if (dr["Typeid"].ToString() == "1")
                    {
                        if (item.Prod_DigiId == dr["Prodid"].ToString())
                        {
                            listfdbk.FdbkId = Convert.ToInt32(dr["feedbackId"]);
                            string lastname = dr["lastname"].ToString();
                            lastname = lastname.Substring(0, 1).ToUpper() + lastname.Substring(1).ToLower();
                            string firstname = dr["firstname"].ToString();
                            firstname = firstname.Substring(0, 1).ToUpper() + firstname.Substring(1).ToLower();
                            listfdbk.creatorName = lastname + " " + firstname;
                            listfdbk.CreatedBy = Convert.ToInt32(dr["CreatedBy"]);
                            if (listfdbk.CreatedBy == uid)
                            {
                                listfdbk.isMyFeekback = true;
                            }
                            else
                            {
                                listfdbk.isMyFeekback = false;
                            }
                            if (item.Prod_DigiId != dr["Productid"].ToString())
                            {
                                listfdbk.prodDigiName = item.Prod_DigitalName + " (" + dr["Product"].ToString() + ")";
                            }
                            else
                            {
                                listfdbk.prodDigiName = item.Prod_DigitalName;
                            }
                            listfdbk.shortDesc = dr["ShortDescription"].ToString();
                            listfdbk.breif = dr["DetailDescription"].ToString();
                            if (!(dr["votestatus"] is DBNull))
                                listfdbk.Votestatus = Convert.ToBoolean(dr["votestatus"]);
                            if (!(dr["votecount"] is DBNull))
                                listfdbk.voteCount = Convert.ToInt32(dr["votecount"]);
                            //if (!(dr["Cmtcount"] is DBNull))
                            //  listfdbk.Cmtcount = Convert.ToInt32(dr["Cmtcount"]);
                            listfdbk.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            listfdbk.CreatedDateStr = listfdbk.CreatedDate.Value.ToString("MMM dd yyyy");
                            if (!(dr["LastUpdatedDate"] is DBNull))
                                listfdbk.LastUpdatedDate = Convert.ToDateTime(dr["LastUpdatedDate"]);
                            else
                                listfdbk.LastUpdatedDate = listfdbk.CreatedDate;
                            listfdbk.product_digitalid = Convert.ToInt32(dr["Productid"]);
                            listfdbk.Typeid = Convert.ToInt32(dr["Typeid"]);
                            listfdbk.FeedbackType = "New Nucleus Feature";
                            if (!(dr["Statusid"] is DBNull))
                            {
                                listfdbk.Statusid = Convert.ToInt32(dr["Statusid"]);
                                if (listfdbk.Statusid == 2)
                                    listfdbk.Status = "Submit For Approval";
                                else
                                    listfdbk.Status = Enum.GetName(typeof(IMFeedbackStatus), listfdbk.Statusid);
                            }
                            DateTime Today = DateTime.Now;
                            TimeSpan t = Today.Subtract((DateTime)listfdbk.LastUpdatedDate);
                            listfdbk.LastActivity = FMHelper.getLastActivity(t);
                            if (!(dr["Statusid"] is DBNull))
                                listfdbk.FdbkTagList = dr["tagname"].ToString();
                            allList.Add(listfdbk);
                        }
                    }
                    else
                    {
                        if (item.Prod_DigitalName == dr["DigitalOffering"].ToString())
                        {
                            listfdbk.FdbkId = Convert.ToInt32(dr["feedbackId"]);
                            string lastname = dr["lastname"].ToString();
                            lastname = lastname.Substring(0, 1).ToUpper() + lastname.Substring(1).ToLower();
                            string firstname = dr["firstname"].ToString();
                            firstname = firstname.Substring(0, 1).ToUpper() + firstname.Substring(1).ToLower();
                            listfdbk.creatorName = lastname + " " + firstname;
                            listfdbk.CreatedBy = Convert.ToInt32(dr["CreatedBy"]);
                            if (listfdbk.CreatedBy == uid)
                            {
                                listfdbk.isMyFeekback = true;
                            }
                            else
                            {
                                listfdbk.isMyFeekback = false;
                            }
                            listfdbk.prodDigiName = item.Prod_DigitalName;
                            listfdbk.shortDesc = dr["ShortDescription"].ToString();
                            listfdbk.breif = dr["DetailDescription"].ToString();
                            if (!(dr["votestatus"] is DBNull))
                                listfdbk.Votestatus = Convert.ToBoolean(dr["votestatus"]);
                            if (!(dr["votecount"] is DBNull))
                                listfdbk.voteCount = Convert.ToInt32(dr["votecount"]);
                            //if (!(dr["Cmtcount"] is DBNull))
                            //  listfdbk.Cmtcount = Convert.ToInt32(dr["Cmtcount"]);
                            listfdbk.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            listfdbk.CreatedDateStr = listfdbk.CreatedDate.Value.ToString("MMM dd yyyy");
                            if (!(dr["LastUpdatedDate"] is DBNull))
                                listfdbk.LastUpdatedDate = Convert.ToDateTime(dr["LastUpdatedDate"]);
                            else
                                listfdbk.LastUpdatedDate = listfdbk.CreatedDate;
                            listfdbk.product_digitalid = Convert.ToInt32(dr["Digitalid"]);
                            listfdbk.Typeid = Convert.ToInt32(dr["Typeid"]);
                            listfdbk.FeedbackType = "New Business Oppurtunity";
                            if (!(dr["Statusid"] is DBNull))
                            {
                                listfdbk.Statusid = Convert.ToInt32(dr["Statusid"]);
                                if (listfdbk.Statusid == 2)
                                    listfdbk.Status = "Submit For Approval";
                                else
                                    listfdbk.Status = Enum.GetName(typeof(IMFeedbackStatus), listfdbk.Statusid);
                            }
                            DateTime Today = DateTime.Now;
                            TimeSpan t = Today.Subtract((DateTime)listfdbk.LastUpdatedDate);
                            listfdbk.LastActivity = FMHelper.getLastActivity(t);
                            if (!(dr["Statusid"] is DBNull))
                                listfdbk.FdbkTagList = dr["tagname"].ToString();
                            allList.Add(listfdbk);
                        }
                    }

                }
                if (allList.Count > 0)
                {
                    viewlist.Prod_DigitalName = item.Prod_DigitalName;
                    viewlist.Prod_DigiId = item.Prod_DigiId;
                    viewlist.selMode = selMode;
                    //viewlist.Searchfeedback = allList;
                    List<searchKeyFeedback> sortedlist = allList;

                    if (sortByVal == "recentCreated")
                        sortedlist = allList.OrderByDescending(o => o.CreatedDate).ToList();
                    if (sortByVal == "recentUpdated")
                        sortedlist = allList.OrderByDescending(o => o.LastUpdatedDate).ToList();
                    if (sortByVal == "voted")
                        sortedlist = sortedlist.OrderByDescending(o => o.voteCount).ToList();
                    //if (sortByVal == "commented")
                    //  sortedlist = sortedlist.OrderByDescending(o => o.Cmtcount).ToList();
                    viewlist.Searchfeedback = sortedlist;

                    fdbkList.Add(viewlist);
                }
            }
            return fdbkList;

        }
        [ActionTrack]
        [HttpPost]
        public ActionResult SaveFeedbackDetails(int Typeid = 0, int? Productid = null, int? Digitalid = null, string ShortDesc = "", string DetailDesc = "", string ItrCl = "", string BtnVal = "", int? feedbackid = 0, string fdbkimg = "")
        {
            try
            {
                int uid = (int)Session["UserId"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                var EncodeDetailDesc = Server.HtmlDecode(DetailDesc);
                var Fdbkid = FMHelper.saveFeedback(Typeid, Productid, Digitalid, ShortDesc, DetailDesc, ItrCl, BtnVal, uid, client.ClientId, feedbackid, IdeaLink, ApproveLink);
                if (fdbkimg != "")
                {
                    FMHelper.saveFdbkImage(fdbkimg, Fdbkid);
                }
                FeedbackViewModel mod = new FeedbackViewModel();
                GetFilterInfo(mod, client.ClientId);
                //List<CommentList> cmtList = new List<CommentList>();
                //mod.Comments = cmtList;
                //mod.FdbkImages = new List<string>();
                mod.Typeid = 1;
                GetListData(mod, client.ClientId, uid);
                return PartialView("_ViewFdbkList", mod);
            }
            catch (Exception ex)
            {
                Log.Error("Error in ViewFeedbackController: SaveFeedbackDetails ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        public ActionResult SearchFeedbackwithKey(int Typeid = 0, int? Productid = null, int? Digitalid = null, string inputString = "")
        {
            try
            {
                var ClientID = (ClientDetail)Session["SessionClient"];
                int uid = (int)Session["UserId"];
                IEnumerable<searchKeyFeedback> temp;
                if (!string.IsNullOrEmpty(inputString.Trim()))
                {
                    temp = FMHelper.GetAllFeedbackForUser(Typeid, Productid, Digitalid, inputString, ClientID.ClientId, uid);
                    return PartialView("_SearchFeedback", temp);
                }
                else
                {
                    temp = null;
                    return PartialView("_SearchFeedback", temp);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in gettingSearchFeedbackwithKey ", ex);
                throw ex;
            }
        }
        public ActionResult AddFDBKComments(int fdbkid, string comment)
        {
            try
            {
                int uid = (int)Session["UserId"];
                var Client = (ClientDetail)Session["SessionClient"];
                int id = FMHelper.SaveFeedbackComment(fdbkid, uid, comment, Client.ClientId);
                FeedbackViewModel mod = new FeedbackViewModel();
                mod.Comments = getCmt(fdbkid);
                mod.FdbkImages = getFdbkImg(fdbkid);
                return PartialView("_ViewFdbkComments", mod);
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveVoteofUser ", ex);
                throw ex;
            }
        }
        public ActionResult GetFeedbackComments(int fdbkid)
        {
            try
            {
                int uid = (int)Session["UserId"];
                FeedbackViewModel mod = new FeedbackViewModel();
                mod.Comments = getCmt(fdbkid);
                return PartialView("_ViewFdbkComments", mod);
            }
            catch (Exception ex)
            {
                Log.Error("Error in Getting Comments ", ex);
                throw ex;
            }
        }
        public ActionResult GetFeedbackImages(int fdbkid)
        {
            try
            {
                int uid = (int)Session["UserId"];
                FeedbackViewModel mod = new FeedbackViewModel();
                mod.FdbkImages = getFdbkImg(fdbkid);
                return PartialView("_ViewFdbkImages", mod);
            }
            catch (Exception ex)
            {
                Log.Error("Error in Getting Images ", ex);
                throw ex;
            }
        }
        public List<CommentList> getCmt(int fdbkid)
        {
            Db db = new Db();
            var cmmt = (from a in db.IMFDBKComment
                        where a.ImfdbkId == fdbkid
                        select a).ToList();
            List<CommentList> cmtList = new List<CommentList>();
            foreach (var item in cmmt)
            {
                CommentList cmt = new CommentList();
                DateTime Today = DateTime.Now;
                TimeSpan t = Today.Subtract((DateTime)item.CommentedOn);
                cmt.Comments = item.Comments;
                cmt.CommentedBy = item.CommentedByName;
                cmt.CommentedOn = FMHelper.getLastActivity(t);
                cmt.fdbkId = item.ImfdbkId;
                cmtList.Add(cmt);
            }
            return cmtList;
        }
        public List<string> getFdbkImg(int fdbkid)
        {
            Db db = new Db();
            var cmmt = (from a in db.IMFDBKAttachment
                        where a.ImfdbkId == fdbkid
                        select a).ToList();
            List<string> ImgList = new List<string>();
            foreach (var item in cmmt)
            {

                string imgStr = item.Fdbkimg;
                ImgList.Add(imgStr);
            }
            return ImgList;
        }
        [HttpPost]
        public ActionResult saveUserNotification(int fdbkid, string notifytype)
        {
            int uid = (int)Session["UserId"];
            var res = FMHelper.UpdateUserNotification(fdbkid, Convert.ToInt32(notifytype), uid);
            return Json(new { res }, JsonRequestBehavior.AllowGet);
        }
        [ActionTrack]
        public ActionResult SaveVoteofUser(string voteid = "")
        {
            try
            {
                int uid = (int)Session["UserId"];
                int voterid = Convert.ToInt32(voteid.Replace("vote-", ""));
                int cnt = FMHelper.SaveFeedbackUserVote(voterid, uid);
                string sel = "feedbackId=" + voterid;
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var isvoted = unitOfWork.IMFDBKVote.Get(cid => cid.IMFDBKId == voterid && cid.CreatedBy == uid).FirstOrDefault();
                DataRow dr = dtFeedback.Select(sel).FirstOrDefault();
                if (dr != null)
                {
                    dr["votecount"] = cnt;
                    dr["votestatus"] = isvoted.Vote;
                }
                return Json(new { cnt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveVoteofUser ", ex);
                throw ex;
            }
        }
        public ActionResult SaveTags(int fdbkid, string tagname)
        {
            int uid = (int)Session["UserId"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            int res = FMHelper.SaveFdbkTag(fdbkid, uid, tagname, ClientID.ClientId);
            string sel = "feedbackId=" + fdbkid;
            DataRow dr = dtFeedback.Select(sel).FirstOrDefault();
            if (dr != null)
            {
                string tag = dr["tagname"].ToString();
                tag = tag + "," + tagname;
                dr["tagname"] = tag;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveTags(int fdbkid, string tagname)
        {
            var res = FMHelper.RemoveFdbkTag(fdbkid, tagname);
            string sel = "feedbackId=" + fdbkid;
            DataRow dr = dtFeedback.Select(sel).FirstOrDefault();
            if (dr != null)
            {
                string tag = dr["tagname"].ToString();
                tag = tag.Replace(tagname, "");
                dr["tagname"] = tag;
            }
            return Json(new { res }, JsonRequestBehavior.AllowGet);
        }
        [ActionTrack]
        public ActionResult ApproveFeedback(int viewtyp = 0)
        {
            loadImageforTemplate();
            ViewBag.viewtyp = viewtyp;
            IdeaLink = Url.Action("Index", "viewfeedback", new { area = "FdbkManager" }, "https");
            reader = new StreamReader(Server.MapPath("~/Areas/FDBKManager/Templates/ApprovedFdbkEmailNotification.html"));
            eApprovedBody = reader.ReadToEnd();
            reader = new StreamReader(Server.MapPath("~/Areas/FDBKManager/Templates/RejectedFdbkEmailNotification.html"));
            eRejectedBody = reader.ReadToEnd();

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            FMHelper.LogFeedbackActivity("OpenedApproveFeedback", "Opened-ApproveFeedbackScreen", client.ClientId, uid);
            FeedbackViewModel mod = new FeedbackViewModel();
            GetFilterInfo(mod, client.ClientId);
            //List<CommentList> cmtList = new List<CommentList>();
            //mod.FdbkImages = new List<string>();
            //mod.Comments = cmtList;
            mod.Typeid = 1;
            GetApproveFeedback(mod, client.ClientId, uid);


            return View(mod);
        }
        public ActionResult Email()
        {

            return View();
        }
        public static void GetApproveFeedback(FeedbackViewModel mod, int ClientId, int uid)
        {
            try
            {
                var paramName = new string[] { "ClientId", "UserId" };
                var paramValue = new object[] { ClientId, uid };
                DataTable dt = new DataTable();
                Db db = new Db();

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var res = unitOfWork.GetDataFromStoredProcedure("IMFDBK_FetchApproveFdbk", paramName, paramValue);
                    dtFeedback = res;

                    mod.Viewfeedbacklist = GetFeedbackfilteredData(res, ClientId, mod.Typeid, "recentCreated", uid);
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error("ViewFeedback controller GetFilterInfo Action error 'GetFilterInfo()' ", ex);
                throw ex;
            }
        }

        [ActionTrack]
        public ActionResult ApproveRejectFdbk(int fdbkId, int status, string cmt)
        {
            int uid = (int)Session["UserId"];
            bool temp = FMHelper.UpdateFdbkStatus(fdbkId, status, cmt, uid, IdeaLink);
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            FeedbackViewModel mod = new FeedbackViewModel();
            GetFilterInfo(mod, client.ClientId);
            //List<CommentList> cmtList = new List<CommentList>();
            //mod.FdbkImages = new List<string>();
            //mod.Comments = cmtList;
            mod.Typeid = 1;
            GetApproveFeedback(mod, client.ClientId, uid);

            return PartialView("_ViewFdbkList", mod);
        }
        public ActionResult GetFeedbackINCClient(int fdbkid = 0)
        {
            var temp = "";
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var list = unitOfWork.IMFDBKClient.Get(e => e.IMFDBKId == fdbkid);
                if (list != null)
                {
                    foreach (var cateory in list)
                    {
                        temp = temp + cateory.InternalClientId + "#";
                    }
                }
                temp = temp.Remove(temp.Length - 1);
                int[] array = temp.Split('#').Select(int.Parse).ToArray();
                return Json(new { array }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveVoteofUser ", ex);
                throw ex;
            }
        }
        public JsonResult Get_ProductApps()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            var apps = FMHelper.getproductapps(client.ClientId, usid);
            return Json(apps, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult Remote_Data_Binding_Get_Employees()
        //{
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    int usid = (int)Session["UserId"];
        //    int ClientId = client.ClientId;
        //    var employees = 
        //    return Json(employees, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult SaveActivityDetails(string ActionName = "", string ActionDetail = "")
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                FMHelper.LogFeedbackActivity(ActionName, ActionDetail, client.ClientId, usid);
                return Json(true);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostFeedbackController: SaveActivityDetails " + ex);
                throw ex;
            }

        }

        public ActionResult GetAndSaveActivityDetails(string ActionName = "", string DetailName = "", int DetailId = 0, string Action = "")
        {
            try
            {
                var ActionDetail = "";
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (DetailName == "Status")
                {
                    ActionDetail = Enum.GetName(typeof(IMFeedbackStatus), DetailId);
                }
                else if (DetailName == "Product")
                {
                    ActionDetail = unitOfWork.FeedbackProduct.GetByID(DetailId).Product;
                }
                else if (DetailName == "digital")
                {
                    ActionDetail = unitOfWork.FeedbackDigital.GetByID(DetailId).DigitalOffering;
                }
                else if (DetailName == "ProductGroup")
                {
                    ActionDetail = unitOfWork.IMFDBKProductGroups.GetByID(DetailId).ProductGroup;
                }
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];

                FMHelper.LogFeedbackActivity(ActionName, Action + "-" + ActionDetail, client.ClientId, usid);
                return Json(true);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostFeedbackController: SaveActivityDetails " + ex);
                throw ex;
            }

        }

        //------------------------------------Email Notification------------------------------------------------------
        public void SendPostFeedbackNotification(string email, string firstName, string fdbkLink)
        {
            try
            {
                var body = "";
                var mailId = email;
                string feedbacklink = fdbkLink;
                string Subject = "Thank you! Your feedback has been submitted successfully";
                body = SubmitForApprovalBody(firstName, feedbacklink);
                App_Start.Helper.SendEMail(mailId, Subject, body, true);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostFeedbackController: SendPostFeedbackNotification " + ex);
            }
        }
        public void SendNotifyFeedbackNotification(string email, string CreatorrName, string ApproverName, string ProductName, string fdbkLink)
        {
            try
            {
                var body = "";
                var mailId = email;
                string Subject = "";
                string feedbacklink = fdbkLink;
                Subject = "Update! New Feedback is submitted for Review ";
                body = NotifyFeedbackBody(CreatorrName, ApproverName, ProductName, feedbacklink);
                App_Start.Helper.SendEMail(mailId, Subject, body, true);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostFeedbackController: SendNotifyFeedbackNotification " + ex);
            }
        }
        public void SendApprovedFeedbackNotification(string email, string Creator, int votecount, string ProductName, string fdbkLink)
        {
            try
            {

                var body = "";
                var mailId = email;
                string Subject = "";
                string feedbacklink = fdbkLink;
                Subject = "Update! Your feedback has been Approved ";
                body = ApprovedFeedbackBody(Creator, votecount, ProductName, feedbacklink);
                App_Start.Helper.SendEMail(mailId, Subject, body, true);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostFeedbackController: SendApprovedFeedbackNotification " + ex);
            }
        }
        public void SendRejectedFeedbackNotification(string email, string firstName, string fdbkLink)
        {
            try
            {
                var body = "";
                var mailId = email;
                string feedbacklink = fdbkLink;
                string Subject = "Update! Your feedback has been Updated ";
                body = RejectedFeedbackBody(firstName, feedbacklink);
                App_Start.Helper.SendEMail(mailId, Subject, body, true);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostFeedbackController: New_KeywordReportList " + ex);
            }
        }
        private string SubmitForApprovalBody(string ReviewerName, string feedbacklink)
        {
            string body = string.Empty;
            body = emailBody;
            body = body.Replace("{creator}", ReviewerName);
            body = body.Replace("{feedbacklink}", feedbacklink);
            body = body.Replace("{Submitbase64String}", Submitbase64String);
            body = body.Replace("{SubmitArrowbase64String}", SubmitArrowbase64String);
            body = body.Replace("{NccLogobase64String}", NccLogobase64String);
            body = body.Replace("{FdbkLogobase64String}", FdbkLogobase64String);
            body = body.Replace("{Reviewbase64String}", Reviewbase64String);
            return body;
        }
        private string NotifyFeedbackBody(string CreatorrName, string ApproverName, string ProductName, string feedbacklink)
        {
            string body = string.Empty;
            body = emailApproverBody;
            body = body.Replace("{creator}", CreatorrName);
            body = body.Replace("{Approver}", ApproverName);
            body = body.Replace("{ProductName}", ProductName);
            body = body.Replace("{feedbacklink}", feedbacklink);
            body = body.Replace("{Notifybase64String}", Notifybase64String);
            body = body.Replace("{NccLogobase64String}", NccLogobase64String);
            body = body.Replace("{FdbkLogobase64String}", FdbkLogobase64String);
            body = body.Replace("{Reviewbase64String}", Reviewbase64String);
            return body;
        }
        private string ApprovedFeedbackBody(string Creator, int votecount, string ProductName, string feedbacklink)
        {
            string body = string.Empty;
            body = eApprovedBody;
            body = body.Replace("{creator}", Creator);
            body = body.Replace("{votecount}", votecount.ToString());
            body = body.Replace("{ProductName}", ProductName);
            body = body.Replace("{feedbacklink}", feedbacklink);
            body = body.Replace("{Approverbase64String}", Approverbase64String);
            body = body.Replace("{Approverarrowbase64String}", Approverarrowbase64String);
            body = body.Replace("{NccLogobase64String}", NccLogobase64String);
            body = body.Replace("{FdbkLogobase64String}", FdbkLogobase64String);
            return body;
        }
        private string RejectedFeedbackBody(string ReviewerName, string feedbacklink)
        {
            string body = string.Empty;
            body = eRejectedBody;
            body = body.Replace("{creator}", ReviewerName);
            body = body.Replace("{feedbacklink}", feedbacklink);
            body = body.Replace("{Notifybase64String}", Notifybase64String);
            body = body.Replace("{Rejectarrowbase64String}", Rejectarrowbase64String);
            body = body.Replace("{NccLogobase64String}", NccLogobase64String);
            body = body.Replace("{FdbkLogobase64String}", FdbkLogobase64String);
            body = body.Replace("{Reviewbase64String}", Reviewbase64String);
            return body;
        }

    }
}