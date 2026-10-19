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

    public class RegisterDatasetMapInput : Input
        {






        public int RegisterDataSetId { get; set; }
        public string DataSetColumnName { get; set; }
        public string VariableType { get; set; }
        public bool IsSelected { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }



        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }