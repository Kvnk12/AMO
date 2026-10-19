using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReportId { get; set; }
        public int ClientId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsPublic { get; set; }
        public string ViewName { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DateType { get; set; }
    }

}