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

    public class ReportConfiguationInput : Input
        {






        


        public int ClientId { get; set; }
        
        //public string ReporName { get; set; }




        [Required(ErrorMessage = "Please Enter ReportName")]
        public string ReportName { get; set; }

        ////private int _variableTypeid; 
        //public int VariableTypeId
        //    {
        //    get { return _variableTypeid; } 
        
        //    set
        //    {
        //    if (this.Id > 0)
        //    {
        //        int reportVariableId = Convert.ToInt32(this.ReportVariableId);
        //        _variableTypeid = AlignmentHelper.GetVariableTypeId(reportVariableId);
        //    }
            
        //    }


            
        //     }


        public int VariableTypeId
            {
            get;
            set;
            }

        public string  variableTypeName { get; set; }
        public int RegisterDataSetId { get; set; }
        public int? ReportTypeID { get; set; }
        public int? ReportVariableId { get; set; }
        public int? ReportVariableRelId { get; set; }
        public int? ReportMetadataId { get; set; }
        public int? UseReportId { get; set; }
        public int? AlgorithmId { get; set; }
        public string SSRSReportName { get; set; }


        public IEnumerable<SelectListItem> GetAllTables
            {
            get
                {
                return AlignmentHelper.ListAllTableAndViews();
                }
            }


        public IEnumerable<SelectListItem> GetDataSetVariableTypes

        {
        get
        {
        return AlignmentHelper.GetDataSetVariableTypes(this.ClientId);   
        }
            
         }



        /// <summary>
        /// 6th May 2014
        /// Display RegDatasets For Heat Map, this has been changed as per Todd's mail.
        /// earlier registered datasets have been displayed based on the variable type selection. now this should be to display only the register datasets specific to the client. 
        /// </summary>

        public IList<vwRegDataSet> RegDataSets
            {
            get
                {
                if (this.ClientId > 0)
                    {
                    return AlignmentHelper.GetVwRegDataSet(this.ClientId).ToList();
                    }
                else
                    return AlignmentHelper.GetVwRegDataSet(0);
                }
            }



        //public IList<vwRegDataSet> RegDataSets
        //    {
        //    get
        //        {
        //        if (this.ClientId > 0 && this.variableTypeName != null)
        //            {
        //            return AlignmentHelper.GetVwRegDataSet(this.ClientId).Where(c => c.VariableType.ToLower() == this.variableTypeName.ToLower()).ToList();
        //            }
        //        else
        //            return AlignmentHelper.GetVwRegDataSet(0);
        //        }
        //    }


        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }


        public ReportDataSetInput reportDataSetInput { get; set; }

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