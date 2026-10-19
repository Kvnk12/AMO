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

/////////////////////////////////////////////////////////////////////////////////////////////////////
// Modified By: Neeraj
// Modified Date: 06-03-2014
// Comment: Added ApplicationInvInput class for Application Inventory
/////////////////////////////////////////////////////////////////////////////////////////////////////

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class BCApplication 
    {
        public string ApplicationName { get; set; }
        public int Appid { get; set; }
        public string BCNames { get; set; }
        public string Description { get; set; }
        public int BCId { get; set; }
        // public bool SelectApp { get; set; }
        public string SelectApp { get; set; }
        public string BCIds { get; set; }
    }
        
}