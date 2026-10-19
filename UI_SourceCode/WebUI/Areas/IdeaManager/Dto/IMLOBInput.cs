using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
//using WebUI.Areas.AlignmentManager.Objects;
using WebUI.Areas.IdeaManager.Objects;

namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class IMLOBInput : Input
        
    {
        public string LOB { set;  get; }

        public int? CreatedByUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string  LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public int ClientId { get; set; }

        //public IList<IMLOB> GetLOBSources
        //{
        //    get
        //    { 
        //        return IdeaManagerHelper.GetLOBSources();
        //    }
        //}
        public int? UpdatedBy { get; set; }
    }
}