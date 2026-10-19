using Data;
using Domain.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.WebUI.Dto;
using WebUI.Areas.IdeaManager.Objects;
using System.Data;
using System.IO;
using WebUI.Dto;

namespace WebUI.Controllers
{
    public class BusinessCapabilitiesController : Controller
    {
        //
        // GET: /BusinessCapabilities/

        [CustomAuthorize(new string[] { "superuser" }, new string[] { "Business Alignment" }, null)]
        public ActionResult Index()
        {
            BusinessCapabilitiesModel model = new BusinessCapabilitiesModel();

           int SubVerticalId = Convert.ToInt32( Request.QueryString["subVId"].ToString());
            model.SubverticalName = Request.QueryString["title"].ToString();
            model.SubVerticalId = SubVerticalId;
            return View("Index",model);
        }
        
        [CustomAuthorize(new string[] { "superuser" }, new string[] { "Business Alignment" }, null)]
        [HttpPost]
        public JsonResult GetAllTemplates([DataSourceRequest] DataSourceRequest request ,int SubVerticalId)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            var result = GetAll(SubVerticalId).ToTreeDataSourceResult(request,
                e => e.BCId,
                e => e.BCParentId,
                
                e => e
            );

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Helper Method to Get all templates for a Client_Id       
        private static IEnumerable<BusinessCapabilitiesModel> GetAll(int subverticalId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templatesList = unitOfWork.BAMBusinessCapabilities.Get(g => g.SubVerticalId == subverticalId);
            var templatesList_1 = templatesList.Select(c => new BusinessCapabilitiesModel
            {
                BCId = c.Id,
                BusinessCapName = c.BusinessCapName,

                BCParentId = c.ParentId,
                CreatedBy = c.CreatedBy,
                CreatedDate = c.CreatedDate,
                SubVerticalId = subverticalId,
                InternalExternal = c.InternalExternal
            });
            return templatesList_1;
        }

        private static IEnumerable<BCApplication> getAllApplicationswithBC(int clientid,string BCids="0",string PortfolioSelId="0")
        {
            //var client = (Client) Session["Client"];
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramNames = new string[] { "BCValues", "ClilentId" , "PortfolioSelId" };
                if (string.IsNullOrEmpty(BCids))
                    BCids = "0";
                if (string.IsNullOrEmpty(PortfolioSelId))
                    PortfolioSelId = "0";
                var paramValueset = new object[] { BCids, clientid, PortfolioSelId };
                dt = unitOfWork.GetDataFromStoredProcedure("getAllApplicationswithPortfolio", paramNames, paramValueset);
            }
             IList<BCApplication> bcApplications = new List<BCApplication>();
            foreach (DataRow dr in dt.Rows)
            {
                var bcApp = new BCApplication();
                bcApp.Appid = Convert.ToInt32(dr["Appid"]);
                bcApp.BCNames = dr["BCNames"].ToString();
                bcApp.BCIds = dr["BCIds"].ToString();
                bcApp.Description = dr["Description"].ToString();
                bcApp.ApplicationName= dr["ApplicationName"].ToString();
                bcApp.SelectApp = dr["BCPresent"] == DBNull.Value ? "false" : "true";
                bcApplications.Add(bcApp);
            }
            return bcApplications;

        }




       // [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Business Alignment Manager" }, null)]
        [HttpPost]
        public JsonResult Create([DataSourceRequest] DataSourceRequest request, BusinessCapabilitiesModel template, int SubVertId = 0)
        {
            var uid = (int)Session["UserId"];
            //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                //template.ClientId = client.Id;
                template.CreatedBy = uid;
                template.SubVerticalId = SubVertId;
                //template.UpdatedBy = uid;
                var something = Capability_Insert(template);
                template.BCId = something;
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Create New Template/Task
        public static int Capability_Insert(BusinessCapabilitiesModel template)
        {
            var NewId = 0;
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = new BAMBusinessCapabilities();
                cv.BusinessCapName = template.BusinessCapName;
                //cv.ClientId = template.ClientId;
                cv.ParentId = template.BCParentId;
                cv.CreatedBy = template.CreatedBy;
                //cv.UpdatedBy = template.UpdatedBy;
                cv.CreatedDate = DateTime.Now;
                cv.SubVerticalId = template.SubVerticalId;
                cv.InternalExternal = "Internal";
              //  cv.UpdatedDt = DateTime.Now;
                unitOfWork.BAMBusinessCapabilities.Insert(cv);
                unitOfWork.Save();
                NewId = cv.Id;
            }
            return NewId;
        }

        public void ClientCapabilityDragUpdate(int Bcids, int? ParentId)
        {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());            


            var bclist = unitOfWork.BAMClientBusinessCapabilities.Get(g => g.Id == Bcids).FirstOrDefault();
            var bcapps = unitOfWork.BCApplicationJunction.Get(g=>g.BCId==Bcids);
            foreach (var bcap in bcapps)
                unitOfWork.BCApplicationJunction.Delete(bcap.Id);
            bclist.ParentId = ParentId;
            //order bc
            var bccurrentlist1 = unitOfWork.BAMClientBusinessCapabilities.Get(g =>g.ParentId == bclist.ParentId).ToList();//.OrderBy(e => e.OrderId).First();
            if (bccurrentlist1.Count != 0)
            {
                var bccurrentlist = bccurrentlist1.OrderBy(g => g.OrderId).First();
                var temporder = bccurrentlist.OrderId;
                if (temporder<bclist.OrderId)
                {
                    bccurrentlist.OrderId = bclist.OrderId;
                    bclist.OrderId = temporder;
                }    
                unitOfWork.BAMClientBusinessCapabilities.Update(bclist);
                unitOfWork.BAMClientBusinessCapabilities.Update(bccurrentlist);
                unitOfWork.Save();
               // return true;
            }
            else
            {
                return ;
            }

            //unitOfWork.BAMClientBusinessCapabilities.Update(bclist);
            //unitOfWork.Save();
            return;
        }
        [HttpPost]
        public bool UpdateBCApplicationsOrderId(int affectedid)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var bcaffectedlist = unitOfWork.BAMClientBusinessCapabilities.GetByID(affectedid);
            var bccurrentlist1 = unitOfWork.BAMClientBusinessCapabilities.Get(g => g.ClientId==client.ClientId&& g.OrderId < bcaffectedlist.OrderId && g.ParentId == bcaffectedlist.ParentId).ToList();//.OrderByDescending(e => e.OrderId).First();
            if (bccurrentlist1.Count!=0) {
                var bccurrentlist = bccurrentlist1.OrderByDescending(e => e.OrderId).First();
                var temporder = bccurrentlist.OrderId;
                bccurrentlist.OrderId = bcaffectedlist.OrderId;
                bcaffectedlist.OrderId = temporder;
                unitOfWork.BAMClientBusinessCapabilities.Update(bcaffectedlist);
                unitOfWork.BAMClientBusinessCapabilities.Update(bccurrentlist);
                unitOfWork.Save();
                return true;
            }
            else { return false; }
      
        }

        public bool UpdateBCApplicationsOrderIddesc(int affectedid)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var bcaffectedlist = unitOfWork.BAMClientBusinessCapabilities.GetByID(affectedid);
            var bccurrentlist1 = unitOfWork.BAMClientBusinessCapabilities.Get(g => g.ClientId == client.ClientId&&g.OrderId > bcaffectedlist.OrderId && g.ParentId == bcaffectedlist.ParentId).ToList();//.OrderBy(e => e.OrderId).First();
            if (bccurrentlist1.Count!=0)
            {
                var bccurrentlist = bccurrentlist1.OrderBy(g => g.OrderId).First();
                var temporder = bccurrentlist.OrderId;
                bccurrentlist.OrderId = bcaffectedlist.OrderId;
                bcaffectedlist.OrderId = temporder;
                unitOfWork.BAMClientBusinessCapabilities.Update(bcaffectedlist);
                unitOfWork.BAMClientBusinessCapabilities.Update(bccurrentlist);
                unitOfWork.Save();
                return true;
            }
            else
            {
                return false;
            }
            //try
            //{
                
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }
        public bool UpdateordertoActual()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var getParentIdNull = unitOfWork.BAMClientBusinessCapabilities.Get(g=>g.ClientId==client.ClientId && g.ParentId==null).OrderBy(o=>o.Id);
            int orderid = 0;
            foreach (var iMod in getParentIdNull)
            {
                updateOneCapability(iMod.Id, client.ClientId, null, orderid);
                orderid++;
            }

            return true;
        }
        private void updateOneCapability(int BamId, int ClientId, int? parentId, int orderid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //int orderid = 0;
            if (parentId == null)
            {
                var childitems = unitOfWork.BAMClientBusinessCapabilities.GetByID(BamId);
                if (childitems != null)
                {
                    //  BAMClientBusinessCapabilities cv = new BAMClientBusinessCapabilities();

                    childitems.OrderId = orderid;
                    unitOfWork.BAMClientBusinessCapabilities.Update(childitems);
                    unitOfWork.Save();
                    var NewBamId = childitems.Id;
                    //Recursive;
                    updateOneCapability(childitems.Id, ClientId, NewBamId, 0);
                }
            }
            else
            {
                var childitems = unitOfWork.BAMClientBusinessCapabilities.Get(w => w.ParentId == BamId);
                //orderid = 0;
                foreach (var chitem in childitems)
                {
                    //  BAMClientBusinessCapabilities cv = new BAMClientBusinessCapabilities();

                    chitem.OrderId = orderid;
                    unitOfWork.BAMClientBusinessCapabilities.Update(chitem);
                    unitOfWork.Save();
                    var NewBamId = chitem.Id;
                    orderid++;
                    //Recursive
                    updateOneCapability(chitem.Id, ClientId, NewBamId, 0);
                }
            }
        }


        [HttpPost]
        // public JsonResult UpdateBCApplications([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<BCApplication> applicationIds, string Bcids, string Apps, string bcToam)
        public bool UpdateBCApplications( string Bcids, string Apps, string bcToam)
        {
          // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            //template.UpdatedBy = uid;
            if (ModelState.IsValid && (!string.IsNullOrEmpty(Bcids) || !string.IsNullOrEmpty(Apps)))
            {
                var contextFactory = new DbContextFactory();
                using (var unitOfWork = new UnitOfWorks(contextFactory.GetContext()))
                {
                  //  bool appsDeleted = false;
                    if (!string.IsNullOrEmpty(Apps) && Apps.Split (',').Length == 1 && bcToam.ToLower()=="false")
                    {
                      //  appsDeleted = true;
                        var appid = Convert.ToInt32(Apps);
                        var bclisttoDelete = unitOfWork.BCApplicationJunction.Get(g => g.AppId == appid).ToList();
                        //var allapps = getAllApplicationswithBC(client.ClientId, Bcids).Where(w => w.SelectApp).ToList();
                        foreach (var bcappj in bclisttoDelete)
                        {
                            unitOfWork.BCApplicationJunction.Delete(bcappj.Id);
                        }
                        if (!string.IsNullOrEmpty(Bcids))
                        {
                            foreach (var bc in Bcids.Split(',').ToList())
                            {
                                var bcapj = new BCApplicationJunction();
                                bcapj.AppId = appid;
                                bcapj.BCId = Convert.ToInt32(bc);
                                unitOfWork.BCApplicationJunction.Insert(bcapj);
                            }
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(Bcids) && Bcids.Split(',').Length == 1 && bcToam.ToLower() == "true")
                    {
                        var bcid = Convert.ToInt32(Bcids);
                        var applisttoDelete = unitOfWork.BCApplicationJunction.Get(g => g.BCId == bcid).ToList();
                        foreach (var bcappj in applisttoDelete)
                        {
                            unitOfWork.BCApplicationJunction.Delete(bcappj.Id);
                        }
                        if (!string.IsNullOrEmpty(Apps))
                        {
                            foreach (var app in Apps.Split(',').ToList())
                            {
                                var bcapj = new BCApplicationJunction();
                                bcapj.AppId = Convert.ToInt32(app);
                                bcapj.BCId = bcid;
                                unitOfWork.BCApplicationJunction.Insert(bcapj);
                            }
                        }
                    }

                    unitOfWork.Save();
                }
            }

            //return Json(applicationIds.ToDataSourceResult(request, ModelState));
            return true;
        }
       // [HttpPost]
        //public bool UpdateBCApplication(string Bcids, string Apps)
        //{
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    if (ModelState.IsValid && (!string.IsNullOrEmpty(Bcids) || !string.IsNullOrEmpty(Apps)))
        //    {
        //        var contextFactory = new DbContextFactory();
        //        using (var unitOfWork = new UnitOfWorks(contextFactory.GetContext()))
        //        {
        //            if (!string.IsNullOrEmpty(Apps) && Apps.Split(',').Length == 1)
        //            {
        //                var appid = Convert.ToInt32(Apps);
        //                var bclisttoDelete = unitOfWork.BCApplicationJunction.Get(g => g.AppId == appid).ToList();
        //                //var allapps = getAllApplicationswithBC(client.ClientId, Bcids).Where(w => w.SelectApp).ToList();
        //                foreach (var bcappj in bclisttoDelete)
        //                {
        //                    unitOfWork.BCApplicationJunction.Delete(bcappj.Id);
        //                }
        //                if (!string.IsNullOrEmpty(Bcids))
        //                {
        //                    foreach (var bc in Bcids.Split(',').ToList())
        //                    {
        //                        var bcapj = new BCApplicationJunction();
        //                        bcapj.AppId = appid;
        //                        bcapj.BCId = Convert.ToInt32(bc);
        //                        unitOfWork.BCApplicationJunction.Insert(bcapj);
        //                    }
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(Bcids) && Bcids.Split(',').Length == 1)
        //            {
        //                var bcid = Convert.ToInt32(Bcids);
        //                var applisttoDelete = unitOfWork.BCApplicationJunction.Get(g => g.BCId == bcid).ToList();
        //                foreach (var bcappj in applisttoDelete)
        //                {
        //                    unitOfWork.BCApplicationJunction.Delete(bcappj.Id);
        //                }
        //                if (!string.IsNullOrEmpty(Apps))
        //                {
        //                    foreach (var app in Apps.Split(',').ToList())
        //                    {
        //                        var bcapj = new BCApplicationJunction();
        //                        bcapj.AppId = Convert.ToInt32(app);
        //                        bcapj.BCId = bcid;
        //                        unitOfWork.BCApplicationJunction.Insert(bcapj);
        //                    }
        //                }
        //            }

        //            unitOfWork.Save();
        //        }
        //    }

        //    return true;
        //}

        //   [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Business Alignment Manager" }, null)]
        [HttpPost]
        public JsonResult Update([DataSourceRequest] DataSourceRequest request, BusinessCapabilitiesModel template)
        {
            //var uid = (int)Session["UserId"];
            //template.UpdatedBy = uid;
            if (ModelState.IsValid)
            {
                Capability_Update(template);
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Create New Template/Task
        public static void Capability_Update(BusinessCapabilitiesModel template)
        {
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = unitOfWork.BAMBusinessCapabilities.GetByID(template.BCId);
                cv.BusinessCapName = template.BusinessCapName;
                //cv.UpdatedBy = template.UpdatedBy;
                //cv.UpdatedDt = DateTime.Now;
                unitOfWork.BAMBusinessCapabilities.Update(cv);
                unitOfWork.Save();
            }
        }

        [HttpPost]
        public ActionResult ImportDefaultCapabilities(int SubVerticalId = 0)
        {
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var userId = (int)Session["UserId"];
            try
            {
                if (SubVerticalId == 0)
                {
                    return Json(new { result = false, msg = "Vertical/Sub Vertical not assigned!!" });
                }
                if(ImportDefaultBusinessCapabilities(client.ClientId, SubVerticalId, userId))
                {
                    return Json(new { result = true, msg = "Imported Successfully" });
                }
                else
                {
                    return Json(new { result = false, msg = "No default capabilities are available for this Vertical/Sub-Vertical!!"});
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { result = false, msg = "Internal Error! Please try again later!"+ ex.Message });
            }
        }
        private bool ImportDefaultBusinessCapabilities(int Clientid, int SubVerticalId, int UserId)
        {

            var contextFactory = new DbContextFactory();
            DataTable returntable = new DataTable();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramNames = new string[] { "ClientId" };
                var paramValueset = new object[] { Clientid };
                returntable=unitOfWork.GetDataFromStoredProcedure("DeleteBusinessCapabilities", paramNames, paramValueset);
            //unitOfWork.Save();
            var getParentIdNull = unitOfWork.BAMBusinessCapabilities.Get(g => g.ParentId == null && g.SubVerticalId == SubVerticalId);
            int orderid = 0;
            foreach (var iMod in getParentIdNull)
            {
                importOneCapability(iMod.Id, Clientid, UserId, null,orderid);
                orderid++;
            }
            return true;
        }
        private void importOneCapability(int BamId, int ClientId, int UserId, int? parentId,int orderid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //int orderid = 0;
            if (parentId==null)
            {
                var childitems = unitOfWork.BAMBusinessCapabilities.GetByID(BamId);
                if (childitems!=null)
                {
                    BAMClientBusinessCapabilities cv = new BAMClientBusinessCapabilities();

                    cv.BusinessCapName = childitems.BusinessCapName;
                    cv.ClientId = ClientId;
                    cv.CreatedBy = UserId;
                    cv.CreatedDate = DateTime.Now;
                    cv.SubVerticalId = childitems.SubVerticalId;
                    cv.ParentId = parentId;
                    cv.OrderId = orderid;
                    unitOfWork.BAMClientBusinessCapabilities.Insert(cv);
                    unitOfWork.Save();
                    var NewBamId = cv.Id;
                    //Recursive;
                    importOneCapability(childitems.Id, ClientId, UserId, NewBamId,0);
                }
            }
            else
            {
                var childitems = unitOfWork.BAMBusinessCapabilities.Get(w => w.ParentId == BamId);
                //orderid = 0;
                foreach (var chitem in childitems)
                {
                    BAMClientBusinessCapabilities cv = new BAMClientBusinessCapabilities();

                    cv.BusinessCapName = chitem.BusinessCapName;
                    cv.ClientId = ClientId;
                    cv.CreatedBy = UserId;
                    cv.CreatedDate = DateTime.Now;
                    cv.SubVerticalId = chitem.SubVerticalId;
                    cv.ParentId = parentId;
                    cv.OrderId = orderid;
                    unitOfWork.BAMClientBusinessCapabilities.Insert(cv);
                    unitOfWork.Save();
                    var NewBamId = cv.Id;
                    orderid++;
                    //Recursive
                    importOneCapability(chitem.Id, ClientId, UserId, NewBamId, 0);
                }
            }
        }
        

        /// <summary>
        /// Client Specific Modification of Business Capabilities
        /// </summary>
        /// <returns></returns>
        public ActionResult ClientCapabilities()
        {
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            BusinessCapabilitiesModel model = new BusinessCapabilitiesModel();
            model.SubVerticalId = client.SubVerticalId;
            model.SubverticalName = getSubVerticalName(client.SubVerticalId);
            model.PortfolioList = GetPortfolioInfo(client.ClientId);
            ViewData["ProcessHealth"] = ProcessHealthList(client.ClientId);
            ViewData["ProcessImportance"] = ProcessImportanceList(client.ClientId);
            ViewData["DosList"] = GetListOfDos(client.ClientId);
            return View(model);
        }
            public static IEnumerable<SelectListItem> GetPortfolioInfo(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var Templist = unitOfWork.LineOfBusinesses.Get(g => g.ClientId == ClientId).ToList();
            var list = Templist.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.LOBName });
            return list;
        }

        public JsonResult GetBCApplications([DataSourceRequest] DataSourceRequest request, string Bcids,string PortfolioSelId)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var bcapps = getAllApplicationswithBC(client.ClientId, Bcids, PortfolioSelId);
            var bcapp = bcapps.OrderBy(c => c.ApplicationName);
            return Json(bcapps.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }
        public IEnumerable<SelectListItem> GetListOfDos(int clientid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var Temp = unitOfWork.BAMDegreeofSupport.Get(g => g.ClientId == clientid).ToList();
            var Templist = Temp.OrderBy(x => x.DOS).Distinct().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DOS.ToString() });
            return Templist;
        }
        
        
        [HttpPost]
        public ActionResult UpdateBCApplicationsForDos([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<BCApplicationJunctionModel> template)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (template!=null) {
                foreach (var item in template)
                {
                    var cv = unitOfWork.BCApplicationJunction.GetByID(item.Id);
                    if (cv!=null)
                    {
                        if (item.DoSId!=null)
                        {
                            cv.DoSId = Convert.ToInt32(item.DoSId);
                        }
                        if (item.PostDoSId!=null)
                        {
                            cv.PostDoSId = Convert.ToInt32(item.PostDoSId);
                        }
                       

                        unitOfWork.BCApplicationJunction.Update(cv);
                    }
                }
            }
          
            unitOfWork.Save();
            return Json(template.ToDataSourceResult(request, ModelState));
         
        }

        public JsonResult GetBCApplicationsForDos([DataSourceRequest] DataSourceRequest request, int Appid)
        {
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramNames = new string[] {"Appid"};
                var paramValueset = new object[] { Appid };
                dt = unitOfWork.GetDataFromStoredProcedure("getAllApplicationswithBcforDos", paramNames, paramValueset);
            }
            IList<BCApplicationJunctionModel> bcApplications = new List<BCApplicationJunctionModel>();
            foreach (DataRow dr in dt.Rows)
            {
                var bcApp = new BCApplicationJunctionModel();
                bcApp.Id = Convert.ToInt32(dr["Id"]);
                bcApp.AppId = Convert.ToInt32(dr["Appid"]);
                bcApp.BCId= Convert.ToInt32(dr["BCId"]);
                bcApp.DoSId= dr["DoSId"].ToString();
                bcApp.PostDoSId= dr["PostDoSId"].ToString();
              //  bcApp.PostDosName = dr["PostDOSName"].ToString();
                bcApp.BCName = dr["BCName"].ToString();
                bcApp.DOSName = dr["DOS"].ToString();
                bcApp.Hierarchy= dr["hierarchy"].ToString();
                bcApplications.Add(bcApp);
            }
            //return bcApplications;
            //ClientDetail client = (ClientDetail)Session["SessionClient"];
            //Db db = new Db();
            //var bcapps = (from bcapp in db.BCApplicationJunction
            //              join bcc in db.BAMClientBusinessCapabilities on bcapp.BCId equals bcc.Id into tempbp
            //              join dos in db.BAMDegreeofSupport on bcapp.DoSId equals dos.Id into tempdos
            //              where bcapp.AppId == Appid
            //              from bpi in tempbp.DefaultIfEmpty()
            //              from bph in tempdos.DefaultIfEmpty()
            //              select new
            //              {
            //                  Id = bcapp.Id,
            //                  BCId = bpi.Id,
            //                  BCName = bpi.BusinessCapName,
            //                  DoSId = bcapp.DoSId,
            //                  DOSName = bph.DOS,
            //                  AppId = bcapp.AppId

            //              }).ToList();
            //var Tempbcapps = bcapps.Select(e => new BCApplicationJunctionModel
            //{
            //    Id = e.Id,
            //    AppId = e.AppId,
            //    BCId = e.BCId,
            //    DoSId = e.DoSId.ToString(),
            //    BCName = e.BCName,
            //    DOSName = e.DOSName

            //});
            return Json(bcApplications.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        private static string getSubVerticalName(int Subvertical)
        {
            Db db = new Db();
            var res = db.IndustrySubVerticals.FirstOrDefault(g => g.Id == Subvertical);
            if (res != null)
                return res.SubVerticalName.Trim();
            else
                return "";
        }

        public JsonResult GetClientCapabilities([DataSourceRequest] DataSourceRequest request)
        {
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var result = _getAllClientCapabilities(client.ClientId).ToTreeDataSourceResult(request,
                e => e.BCId,
                e => e.BCParentId,
                e => e
            );

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Helper Method to Get all templates for a Client_Id       
        private static IEnumerable<BusinessCapabilitiesModel> _getAllClientCapabilities(int ClientId)
        {
          //  IList<BusinessCapabilitiesModel> Bcap = new List<BusinessCapabilitiesModel>();
            var contextFactory = new DbContextFactory();
         //   var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
         //   var templatesList = unitOfWork.BAMClientBusinessCapabilities.Get(g => g.ClientId== ClientId);

            Db db = new Db();
            var Bcap = (from bc in db.BAMClientBusinessCapabilities
                                 join bpi in db.BusinessProcessImportance on bc.ProcessImportanceId equals bpi.Id into tempbpi
                                 join bph in db.Businessprocesshealth on bc.ProcessHealthId equals bph.Id into tempbph
                                 where bc.ClientId == ClientId
                                 from bpi in tempbpi.DefaultIfEmpty()
                                 from bph in tempbph.DefaultIfEmpty()
                                 select new BusinessCapabilitiesModel
                                 {
                                     BCId = bc.Id,
            BusinessCapName = bc.BusinessCapName,
            ProcessImportanceId = bc.ProcessImportanceId,
            ProcessHealthId = bc.ProcessHealthId,
            ProcessImportance = bpi.Processimportance,
           ProcessHealth = bph.ProcessHealth,
            Description = bc.Description,
            Notes = bc.Notes,
            BCParentId = bc.ParentId,
            CreatedBy = bc.CreatedBy,
            CreatedDate = bc.CreatedDate,
            ClientId = bc.ClientId.Value,
            InternalExternal = bc.InternalExternal,
            OrderId= bc.OrderId

                                 }).OrderBy(x =>x.OrderId);

            
      
                return Bcap;
  
        }




        // [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Business Alignment Manager" }, null)]
        [HttpPost]
        public JsonResult ClientCapabilityCreate([DataSourceRequest] DataSourceRequest request, BusinessCapabilitiesModel template)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var uid = (int)Session["UserId"];
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (template.BusinessCapName.Trim()!=""||ModelState.IsValid)
            {
                template.ClientId = client.ClientId;
                template.CreatedBy = uid;
                template.SubVerticalId = client.SubVerticalId;
                var something = _clientCapabilityInsert(template);
                template.BCId = something;
                //template
                try
                {
                    if (template.ProcessImportanceId != null)
                        template.ProcessImportance = unitOfWork.BusinessProcessImportance.GetByID(template.ProcessImportanceId).Processimportance;
                    else
                        template.ProcessImportance = "";
                }
                catch { }
                try
                {
                    if (template.ProcessHealthId != null)
                        template.ProcessHealth = unitOfWork.Businessprocesshealth.GetByID(template.ProcessHealthId).ProcessHealth;
                    else
                        template.ProcessHealth = "";
                }
                catch { }
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Create New Template/Task
        private static int _clientCapabilityInsert(BusinessCapabilitiesModel template)
        {
            var NewId = 0;
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = new BAMClientBusinessCapabilities();
                cv.BusinessCapName = template.BusinessCapName;
                cv.ParentId = template.BCParentId;
                cv.CreatedBy = template.CreatedBy;
                cv.ClientId = template.ClientId;
                cv.CreatedDate = DateTime.Now;
                cv.SubVerticalId = template.SubVerticalId;
                cv.InternalExternal = "Internal";
                cv.Description = template.Description;
                cv.Notes = template.Notes;
                cv.ProcessHealthId = template.ProcessHealthId;
                cv.ProcessImportanceId = template.ProcessImportanceId;
                //int count = (unitOfWork.BAMClientBusinessCapabilities.Get(g => g.ClientId == template.ClientId && g.ParentId == template.BCParentId).Count())+1;
                cv.OrderId =unitOfWork.BAMClientBusinessCapabilities.Get(g => g.ClientId==template.ClientId && g.ParentId== template.BCParentId).Count();
                unitOfWork.BAMClientBusinessCapabilities.Insert(cv);
                unitOfWork.Save();
                NewId = cv.Id;
             //   template.ProcessHealth = template.ProcessHealthId.Value;
            }
            return NewId;
        }


        //   [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Business Alignment Manager" }, null)]
        [HttpPost]
        public JsonResult ClientCapabilityUpdate([DataSourceRequest] DataSourceRequest request, BusinessCapabilitiesModel template)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (template.BusinessCapName.Trim()!="")
            {
                _clientCapabilityUpdate(template);
            }
            try
            {
                if (template.ProcessImportanceId != null)
                    template.ProcessImportance = unitOfWork.BusinessProcessImportance.GetByID(template.ProcessImportanceId).Processimportance;
                else
                    template.ProcessImportance = "";
            }
            catch { }

            try
            {
                if (template.ProcessHealthId != null)
                    template.ProcessHealth = unitOfWork.Businessprocesshealth.GetByID(template.ProcessHealthId).ProcessHealth;
                else
                    template.ProcessHealth = "";
            }
            catch { }
            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Create New Template/Task
        public static void _clientCapabilityUpdate(BusinessCapabilitiesModel template)
        {
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = unitOfWork.BAMClientBusinessCapabilities.GetByID(template.BCId);
                cv.BusinessCapName = template.BusinessCapName;
                cv.Notes = template.Notes;
                cv.Description = template.Description;
                cv.ProcessHealthId = template.ProcessHealthId;
                cv.ProcessImportanceId = template.ProcessImportanceId;
               // cv.OrderId = template.OrderId;
                //cv.UpdatedBy = template.UpdatedBy;
                //cv.UpdatedDt = DateTime.Now;
                unitOfWork.BAMClientBusinessCapabilities.Update(cv);
                unitOfWork.Save();
            }
        }

        public ActionResult ClientCapabilityDelete([DataSourceRequest] DataSourceRequest request, BusinessCapabilitiesModel template)
        {
            deleteChildren(template.BCId);
            return Json(new[] { template }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult DestroyBCs([DataSourceRequest] DataSourceRequest request, BusinessCapabilitiesModel template)
        {
            deleteChildBCs(template.BCId);
            return Json(new[] { template }.ToDataSourceResult(request, ModelState));
        }

        private static void deleteChildBCs(int SubId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            var allCapabilities = unitOfWork.BAMBusinessCapabilities.GetByID(SubId);
            var childCapabilities = unitOfWork.BAMBusinessCapabilities.Get(t => t.ParentId == SubId).ToList();

            if (childCapabilities.Count() != 0)
            {
                foreach (var childTask in childCapabilities)
                {
                    deleteChildren(childTask.Id);
                }
            }
            unitOfWork.BAMBusinessCapabilities.Delete(allCapabilities.Id);
            unitOfWork.Save();
        }

        private static void deleteChildren(int SubId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var bcapps = unitOfWork.BCApplicationJunction.Get(g => g.BCId == SubId);
            foreach (var bcapp in bcapps)
            {
                try
                {
                    unitOfWork.BCApplicationJunction.Delete(bcapp.Id);
                    unitOfWork.Save();
                }
                catch
                {

                }
            }
            var allCapabilities = unitOfWork.BAMClientBusinessCapabilities.GetByID(SubId);
            var bccurrentlist1 = unitOfWork.BAMClientBusinessCapabilities.Get(g => g.OrderId > allCapabilities.OrderId && g.ClientId==allCapabilities.ClientId && g.ParentId == allCapabilities.ParentId).ToList();
            if (bccurrentlist1.Count()!=0)
            {
                foreach (var item in bccurrentlist1)
                {
                    item.OrderId = item.OrderId - 1;
                    unitOfWork.BAMClientBusinessCapabilities.Update(item);
                    unitOfWork.Save();
                }
            }
            var childCapabilities = unitOfWork.BAMClientBusinessCapabilities.Get(t => t.ParentId == SubId).ToList();
            
            if (childCapabilities.Count() != 0)
            {
                foreach (var childTask in childCapabilities)
                {
                    var bcchildapps = unitOfWork.BCApplicationJunction.Get(g => g.BCId == childTask.Id);
                    foreach (var bccapp in bcchildapps)
                    {
                        try
                        {
                            unitOfWork.BCApplicationJunction.Delete(bccapp.Id);
                        }
                        catch
                        {

                        }
                    }
                    unitOfWork.Save();
                    deleteChildren(childTask.Id);
                }
            }
            unitOfWork.BAMClientBusinessCapabilities.Delete(allCapabilities.Id);
            unitOfWork.Save();
        }

       // [HttpPost]
        public ActionResult ExportDefaultCapabilities()
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            string fileName = client.ClientName.Replace(" ", "_")+"_BusinessCapabilities" + ".xlsx";
            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(GetCapabilities(ClientId));
            stream.Position = 0;
            string mimeType = "application/ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;
        }

        private System.Data.DataTable GetCapabilities(int ClientId)
        {
            //List<BCApplication> bcCollection = new List<BCApplication>();
            // Db db = new Db();
            var contextFactory = new DbContextFactory();
            DataTable returntable = new DataTable();
            using (var unitOfWork = new UnitOfWorks(contextFactory.GetContext()))
            {
                var paramName = new string[] { "clientid" };
                var paramValue = new object[] { ClientId };
                returntable = unitOfWork.GetDataFromStoredProcedure("GetClientBCApplications", paramName, paramValue);
            }
            //DataTable returntable2 = new DataTable();
            ////foreach (DataColumn col in returntable.Columns)
            ////{
            ////    returntable2.Columns.Add(col.ColumnName);
            ////}
            int count = returntable.Rows.Count;
            List<int> rownums=new List<int>();
            for (int i= 0; i< count; i++)
            {
                if (i == returntable.Rows.Count - 1)
                {
                   // returntable.Rows.Remove(returntable.Rows[i]);
                    // returntable2.Rows.Add(returntable.Rows[i]);
                }
                else
                {
                    string strConcat = returntable.Rows[i][1].ToString() + returntable.Rows[i][2].ToString() + returntable.Rows[i][3] + returntable.Rows[i][4].ToString();
                    string strConcatNext = returntable.Rows[i + 1][1].ToString() + returntable.Rows[i + 1][2].ToString() + returntable.Rows[i + 1][3] + returntable.Rows[i + 1][4].ToString();
                    if (strConcatNext.StartsWith(strConcat))
                    {
                        rownums.Add(i);
                        //returntable.Rows.Remove(returntable.Rows[i]);
                        // returntable2.Rows.Add(returntable.Rows[i]);
                    }
                }
            }
            foreach(int i in rownums)
            {
                try
                {
                    returntable.Rows.RemoveAt(i);
                }
                catch { }
            }
            returntable.TableName = "BAMCapabilities";
            return returntable;
        }
        public IEnumerable<SelectListItem> ProcessHealthList(int ClientID)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templatesList = unitOfWork.Businessprocesshealth.Get(g => g.Clientid == ClientID);
            var item = templatesList.OrderBy(x => x.ProcessHealth).Distinct().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ProcessHealth.ToString() });
            return item;
        }    public IEnumerable<SelectListItem> ProcessImportanceList(int ClientID)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templatesList = unitOfWork.BusinessProcessImportance.Get(g => g.Clientid == ClientID);
            var item = templatesList.OrderBy(x => x.Processimportance).Distinct().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Processimportance.ToString()});
            return item;
        }
             
        public ActionResult ExportBCList()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            string fileName1 = client.ClientName.Replace(" ", "_") + "_BusinessCapabilities" + ".xlsx";
            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(GetBusinessCapabilities(ClientId));
            stream.Position = 0;
            string mimeType = "application/ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName1;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;
        }

        private System.Data.DataTable GetBusinessCapabilities(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            DataTable returntable = new DataTable();
            using (var unitOfWork = new UnitOfWorks(contextFactory.GetContext()))
            {
                var paramName = new string[] { "clientid" };
                var paramValue = new object[] { ClientId };
                returntable = unitOfWork.GetDataFromStoredProcedure("GetClientBCs", paramName, paramValue);
            }
            returntable.TableName = "BAMCapabilities";
            return returntable;
        }



    }
}
