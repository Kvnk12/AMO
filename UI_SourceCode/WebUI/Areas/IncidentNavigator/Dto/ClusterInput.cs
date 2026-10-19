using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using WebUI.Areas.IncidentNavigator.Models;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
//Added by Anandhi for Client Profile Begin
using System.ComponentModel.DataAnnotations.Schema;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Mapper;
using System.Globalization;
//Added by Raj for validation
using Foolproof;
using System.ComponentModel;
using PagedList;

namespace WebUI.Areas.IncidentNavigator.Dto
{
    public class ClusterInput
    {
        public int? ClientId { get; set; }
        public string ConfigItemName { get; set; }
        public string Type { get; set; }
        public int? ClusterId { get; set; }

        [MaxLength(500)]
        public string imagePerCluster { get; set; }
        public IList<ClusterlistModel> Clusterlist { get; set; }

        public IEnumerable<IncidentsClusterlistModel> IncidentClusterlist { get; set; }
        public IEnumerable<SelectListItem> ConfigItemList
        {

            get
            {


                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> Incidents = new NTTData.AMOPortal.Data.Repo<NTTData.AMOPortal.Domain.Entities.Incident>(Context);
                List<string> IncidentList = Incidents.Where(x => x.ClientId == ClientId).Select(x => x.ConfigItemName).Distinct().ToList();
                var Items = IncidentList.Select(CList => new SelectListItem
                {
                    Value = CList.ToString(),
                    Text = CList.ToString()
                });
                return Items;
            }
        }


        public IEnumerable<SelectListItem> TypeList
        {

            get
            {


                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> Incidents = new NTTData.AMOPortal.Data.Repo<NTTData.AMOPortal.Domain.Entities.Incident>(Context);
                List<string> Typelist = Incidents.Where(x => x.ClientId == ClientId && x.ConfigItemName == ConfigItemName).Select(x => x.Type).Distinct().ToList();
                var Items = Typelist.Select(CList => new SelectListItem
                {
                    Value = CList.ToString(),
                    Text = CList.ToString()
                });
                return Items;
            }
        }



    }

}