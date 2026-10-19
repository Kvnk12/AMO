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

    public class mdmVariableRelInput : Input
        {

        public bool IsDeleted { get; set; }
        public int RelationshipTypeId { get; set; }
        public int RelationshipVariableId1 { get; set; }
        public int RelationshipVariableId2 { get; set; }
        public int DisplayOrder { get; set; }


      

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }



        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }