using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using System.ComponentModel.DataAnnotations.Schema;


namespace NTTData.AMOPortal.WebUI.Dto
    {
    
    public class mdmVariableLabelInput : Input
        {

        public bool IsDeleted { get; set; }
        
        [Display(Name = "Variable Category")]
        [Required(ErrorMessage = "Select Variable Category")]
        public int VariableCatId { get; set; }

        
        [MaxLength(50)]
        [Display(Name = "Variable Name")]
        [Required(ErrorMessage="Select Variable Name")]
        public String VariableLabel { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description")]
        
        public string FullDescription { get; set; }

        public int? DisplayOrder { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }
        
        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }