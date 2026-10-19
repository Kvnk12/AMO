using Data;
using Domain.Entities.PortfolioAnalyzer;
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
using WebUI.Areas.PortfolioAnalyzer.Models;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class ClientAccessController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/ClientAccess/

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ServerFiltering_GetClients(string text)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var clients = unitOfWork.Clients.Get(g=>g.IsDeleted==false && g.Id!=client.ClientId).OrderBy(x=>x.ClientName).Select(p => new SelectListItem
            {
                Text = p.ClientName,
                Value = p.Id.ToString()
            });

            if (!string.IsNullOrEmpty(text))
            {
                clients = clients.Where(p => p.Text.Contains(text));
            }

            return Json(clients, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetSubClients([DataSourceRequest] DataSourceRequest request)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var list = ( from c in db.Clients
                         join Sc1 in db.ClientAccessControl on c.Id equals Sc1.ClientId
                         join up in db.UserProfiles on Sc1.CreatedBy equals up.Id into tempUser
                         from UserP in tempUser.DefaultIfEmpty()
                             //IsDeleted - Check - Mandate
                         where Sc1.ParentClientId == client.ClientId && Sc1.IsDeleted==false
                         select new 
                         {
                             AccessId = Sc1.Id,
                             ClientName = c.ClientName,
                             CreatedByFirst = UserP.FirstName,
                             CreatedByLast = UserP.LastName,
                             CreatedDt= Sc1.CreatedDt
                         }).ToList();

            var LinkedClientList = list.Select(f => new ClientAccessModel
            {
                Id = f.AccessId,
                Client = f.ClientName,
                CreatedBy = f.CreatedByFirst + " " + f.CreatedByLast,
                CreatedDt = f.CreatedDt
            });
            return Json(LinkedClientList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult AddSubClient(int SCId=0)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (SCId != 0)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //IsDeleted - Check - Mandate
                var check = unitOfWork.ClientAccessControl.Get(g => g.ParentClientId == client.ClientId && g.ClientId == SCId && g.IsDeleted==false).Any();
                //True - return already Exists
                if (check)
                {
                    return Json(2);//Already Exists
                }
                else
                {
                    ClientAccessControl entity = new ClientAccessControl()
                    {
                        ClientId = SCId,
                        ParentClientId = client.ClientId,
                        CreatedBy = uid,
                        CreatedDt = DateTime.Now,
                        UpdatedBy = uid,
                        UpdatedDt = DateTime.Now,
                        IsDeleted = false
                    };
                    unitOfWork.ClientAccessControl.Insert(entity);
                    unitOfWork.Save();

                    return Json(1);//Success - Linked Client succcessfully
                }
            }
            else
            {
                return Json(0);//NO Client Id - please select
            }

        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult DeleteClientLink([DataSourceRequest] DataSourceRequest request, ClientAccessModel model)
        {
            var uid = (int)Session["UserId"];
            if (model != null)
            {
                //softdelete - Check IsDeleted.
                PortfolioAnalyzerHelper.DeleteClientLink(model.Id, uid);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


    }
}
