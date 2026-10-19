using Domain.Entities.ClientProfileManager;
using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Globalization;
using WebUI.Areas.IncidentNavigator.Dto;
using Kendo.Mvc.UI;
using WebUI.Dto;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class ClusterController : Controller
    {
        //
        // GET: /IncidentNavigator/Cluster/

        [ActionTrack]
        public ActionResult Index(int? ClientId, string ConfigItemName ,string Type,int? clusterId )
        
            {
            ClusterInput c = new ClusterInput();
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientID = client.ClientId;
            c.ClientId = ClientID;
            c.ConfigItemName = ConfigItemName;
            c.Type = Type;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            ////if (ClientId.HasValue && !string.IsNullOrEmpty(ConfigItemName) && !string.IsNullOrEmpty(Type))
            ////{
            ////    c.Clusterlist = IncidentNavigatorHelper.INGetDistinctClusters(ClientId.Value,Type , ConfigItemName);

            ////}
            ////c.ClusterId = clusterId;

            ////if (ClientId.HasValue && !string.IsNullOrEmpty(ConfigItemName) && !string.IsNullOrEmpty(Type) && clusterId.HasValue)
            ////{
            ////     c.IncidentClusterlist = IncidentNavigatorHelper.INGetIncidentClusterlist(ClientId.Value, Type, ConfigItemName,clusterId.Value);

            ////}
            return View(c);
        }
        public string getFullName(int userId)
        {
            Db db = new Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }


    }
}
