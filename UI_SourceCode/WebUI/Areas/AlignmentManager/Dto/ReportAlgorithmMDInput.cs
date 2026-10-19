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


    public class ReportAlgorithmMDKey
        {
        public string Key { get; set; }
        public string Value { get; set; }
        public string KeyNames { get; set; }
        public List<string> SplitKeyNames { get; set; }
        public List<string> SplitValues { get; set; }
        }

    public class ReportAlgorithmMDInput : Input
        {


        //public int ReportConfigId { get; set; }
        public int AlgorithmId { get; set; }
        public string key { get; set; }
        public string Value { get; set; }
        public string KeyNames {get;set;}

        //public string UsedColumnOne { get; set; }
        //public string UsedColumnTwo { get; set; }


        

        public IList<ReportDataSet> SelectedColumns(int ReportConfigId=0)
            {
              ReportDataSet Rdconfig =   AlignmentHelper.GetReportDataSetsForReportConfigId(ReportConfigId).FirstOrDefault();

            return AlignmentHelper.GetReportDataSets(ReportConfigId, Rdconfig.RegisterDataSetId);
            }


        public IList<SelectListItem> GetColumnsForAlgorithm(int ReportConfigId =0)
            {
           
                return AlignmentHelper.GetColumns(ReportConfigId);

            }


        public IList<SelectListItem> GetAlgorithms()
            {

            return AlignmentHelper.GetAlgorithms();

            }



        public IList<SelectListItem> GetColumnsForchar(int ReportConfigId = 0)
            {

            return AlignmentHelper.GetColumns(ReportConfigId);

            }




        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }
