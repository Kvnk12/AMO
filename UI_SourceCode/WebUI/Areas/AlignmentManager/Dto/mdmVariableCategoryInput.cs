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

    

    public class mdmVariableCategoryInput : Input
    {

        [Required(ErrorMessage = "Enter Variable Category")]
        [MaxLength(200,ErrorMessage="Max Length is 200")]
        public string VariableCategory { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedByUser { get; set; }

        //public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        public DateTime? CreatedDate { get; set; }

        private DateTime _appDate = DateTime.Now;

        public string LastUpdatedUser { get; set; }

        //public DateTime? LastUpdatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }
        public DateTime? LastUpdatedDate { get;  set;} 

    }

}