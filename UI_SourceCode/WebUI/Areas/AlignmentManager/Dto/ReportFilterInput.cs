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

    public class ReportFilterInput : Input
        {


        public int ReportConfigId { get; set; }
        //public int FilterId { get; set; }
        public string FilterValue { get; set; }
        public string FilterColumn { get; set; }
        public int SelectedColId { get; set; }


        

        public IList<ReportDataSet> SelectedColumns(int ReportConfigId=0)
            {
              ReportDataSet Rdconfig =   AlignmentHelper.GetReportDataSetsForReportConfigId(ReportConfigId).FirstOrDefault();

            return AlignmentHelper.GetReportDataSets(ReportConfigId, Rdconfig.RegisterDataSetId);
            }


        public IList<SelectListItem> ColumnData(int ColId=0)
            {
           
                return AlignmentHelper.GetColumnData(ColId);

            }


        public IList<String> ExistingFilters(int ReportConfigId = 0)
            {

            List<string> FilterColumns = new List<string>(); 
            IList<ReportDataSet> SelectedColumns = this.SelectedColumns(ReportConfigId);
            IList<ReportFilter> reportFilter = AlignmentHelper.GetReportFilterForReportConfigId(ReportConfigId);
            foreach (ReportDataSet item in SelectedColumns)
            {
            if (reportFilter.Where(c => c.FilterColumn.ToLower() == item.SelectedColumn.ToLower()).Count() > 0)
            {
            FilterColumns.Add(item.SelectedColumn);
            }
            }
            return FilterColumns;
            }



        public IList<String> ExistingFilterData(int ReportConfigId = 0, int ColumnId=0)
            {

            List<string> FilterData = new List<string>();
            ReportDataSet SelectedColumn = this.SelectedColumns(ReportConfigId).Where(c => c.Id == ColumnId).FirstOrDefault();
            IList<ReportFilter> reportFilter = AlignmentHelper.GetReportFilterForReportConfigId(ReportConfigId).Where(c=> c.FilterColumn.ToLower() == SelectedColumn.SelectedColumn.ToLower()).ToList();
            foreach (ReportFilter item in reportFilter)
                {
                FilterData.Add(item.FilterValue);
                
                }
            return FilterData;
            }

    


        //public IList<DataSetColumnsForHeatMapReportFilter> SelectedColumns(int ReportConfigId = 0)
        //    {

        //    return AlignmentHelper.GetColumnsForHeatMapFilter(ReportConfigId);
        //    }



        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }
