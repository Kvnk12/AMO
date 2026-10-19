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

    public class ReportVariableInput : Input
        {



        public int VariableTypeId { get; set; }
        public int? VariableId { get; set; }
        public int? VariableLabelId { get; set; }



        public IEnumerable<SelectListItem> GetAllTables
            {
            get
                {
                return AlignmentHelper.ListAllTableAndViews();
                }
            }


        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }




        //public IList<SelectListItem> RegDataList 
        //    {
        //    get
        //        {
        //        return AlignmentHelper.HeatMapReportNames(ClientId);
        //        }
                
        //    }

        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }