using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using System.Data;
using System.Data.SqlClient;
using NTTData.AMOPortal.WebUI.Dto;



namespace WebUI.Areas.AlignmentManager.Dto
    {

    //public class   heatMapItem
    //    {
    //       public string heatMapCategory;
    //       public string heatMapItemName;
    //       public string  heatMapColor;
    //       public string id;
           
    //    }
    public class PAReportsInput : Input
        {

        public string detailId { get; set; }

        public int ClientId { get; set; }
        public int? HeatMapReportId { get; set; }
        public string HeatMapReportName { get; set; }
        public string PAReportName { get; set; }
        public string ReportName { get; set; }
        public int SSRSTemplateFileId { get; set; }
        public string SSRSReportURLString { get; set; }


        public int ReportId { get; set; }

        //public string heatMapVariableName { get; set; }
        //public int heatMapVariableId { get; set; }

        public heatMapData heatMapData;
        public DataTable _heatMapDataTable;

        public PPTreeView PPTreeViewRoot
            {
            get
                {
                IList<PPTreeView> AllTreeViewItems = AlignmentHelper.GetTreeViewItems(this.ClientId);
                PPTreeView root = AllTreeViewItems.Where(c => c.ParentId == 0).FirstOrDefault();
                SetChildren(root, AllTreeViewItems);
                return root;
                }

            }


        public int  GetSSRSReportTemplateId(int ReportId) 
        {
        return (AlignmentHelper.SSRSReportTemplateId(ReportId));
        }

        private void SetChildren(PPTreeView ppItem, IList<PPTreeView> list)
            {
            var children = list.Where(c => c.ParentId == ppItem.Id).ToList();
            if (children.Count > 0)
                {
                foreach (var child in children)
                    {
                    SetChildren(child, list);
                    ppItem.TreeViewItems.Add(child);
                    }
                }

            }

        
        public DataTable GetHeatMapDataTable
            {
            get
                {
                return _heatMapDataTable;
                }
            }


        public List<SelectListItem> PAReportNamesWithSSRSURL
            {

            get
            {
            List<SelectListItem> paReportWithURLString = new List<SelectListItem>();
            IEnumerable<SelectListItem> paReportNames = this.PAReportNames;

            foreach (SelectListItem item in paReportNames)
                {
                int ReportId = Convert.ToInt32(item.Value);
                if (ReportId > 0)
                    {
                    SSRSTemplateFileId = this.GetSSRSReportTemplateId((ReportId));
                    //ReportName="+@Model.ReportName+ "&ClientID="+Model.ClientId+"&PAReportName="+@Model.PAReportName + ""
                    ReportChartTemplates chartTemplateInfo = AlignmentHelper.GetChartInfo(SSRSTemplateFileId);
                    ReportConfiguration reportConfigInfo = AlignmentHelper.GetById<ReportConfiguration>(ReportId);
                    string SSRSUrl = ""; //"ReportName=" + PAReportName + "&ClientID=" + ClientId + "&PAReportName=" + "";

                    if (chartTemplateInfo != null)
                        {
                        SSRSUrl += "ReportName=" + chartTemplateInfo.SSRSReportTemplateFile;
                        SSRSUrl += "&ClientID=" + reportConfigInfo.ClientId;
                        SSRSUrl += "&PAReportName=" + reportConfigInfo.ReportName;
                        SSRSUrl += "&" + chartTemplateInfo.ParameterToPass + "=" + chartTemplateInfo.ParameterValueToPass;
                        SSRSReportURLString = SSRSUrl;
                        var selectitem = new SelectListItem();
                        selectitem.Text = item.Text;
                        selectitem.Value = SSRSUrl;
                        paReportWithURLString.Insert(paReportWithURLString.Count, selectitem);
                        }

                    }
                }

            return paReportWithURLString;
            }
        }

        public IEnumerable<SelectListItem> PAReportNames
            {
            get
                {
                return GetDropDownListItems.PAReportNames(this.ClientId);
                }
            }



        public IEnumerable<SelectListItem> PAReportList
            {
            get
                {
                return GetDropDownListItems.PAReportList(this.ClientId);
                }
            }

        public IEnumerable<SelectListItem> HeatMapReportNames
            {
            get
                {
                return GetDropDownListItems.HeatMapReportNames(this.ClientId);
                }
            }



        public IEnumerable<SelectListItem> ClientList
            {
            get
                {
                return GetDropDownListItems.ClientList;
                }
            }



        }
    }