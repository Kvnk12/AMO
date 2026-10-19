using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using PagedList;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class UserApplicationsDto
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public int Appid { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int UserId { get; set; }
       // public string RoleName { get; set; }
    }

    public class UserApplicationsInput
    {
        public IList<SelectListItem> Apps { get; set; }
        public int[] selectedAppIds { get; set; }
        public int RoleId { get; set; }
        public int ClientId { get; set; }
        //public string ClientName { get; set; }
        public int UserId { get; set; }
        public IList<SelectListItem> UserRoles { get; set; }
    }

}
