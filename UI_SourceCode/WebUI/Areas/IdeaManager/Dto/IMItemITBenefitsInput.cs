using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
//using WebUI.Areas.AlignmentManager.Objects;


namespace NTTData.AMOPortal.WebUI.Dto
    {

    public class IMItemITBenefitsInput /*: Input*/
    {
    public int Id { get; set; }
    public int ItemId { get; set; }
    [StringLength(255)]
    [Required]
    public string Name { get; set; }
    //[StringLength(255)]
    public string Detail { get; set; }
    public decimal? Value { get; set; }
    public int? FTEHours { get; set; }
    public int? IncidentCount { get; set; }
        // public string CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }
    //public string LastUpdatedUser { get; set; }
    public DateTime LastUpdatedDate { get; set; }

    //public bool checkNameExists(int ItemId, string Name)
    //{

    //return false;
        
    //}
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string BenefitType { get; set; }
    }
}