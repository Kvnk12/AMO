using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using NTTData.AMOPortal.WebUI.Controllers;
using Telerik.Web.UI;
using System.Collections;

namespace WebUI.Reports
{
    public partial class OverallSummaryReport : AspReportpagesBaseClass
    {

        private Db _dataContext;

        protected Db DbContext
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new Db();
                }
                return _dataContext;
            }
        }

        public override void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
            base.Dispose();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        public void FillGrid()
        {
            var list = DbContext.v_DashboardOverallSummary.ToList();
            gridOverallSummary.DataSource = list;

           // gridOverallSummary.DataBind();
        }

        protected void gridOverallSummary_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridGroupHeaderItem)
            {
                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
                item.DataCell.Text = groupDataRow["CustomMetricName"].ToString();
            }

            if (e.Item is GridDataItem)
            {
                var dataItem = (GridDataItem)e.Item;
                if (!string.IsNullOrEmpty(dataItem["ComplianceTarget"].Text) && !string.IsNullOrEmpty(dataItem["Percentage"].Text) &&
                    dataItem["ComplianceTarget"].Text != "&nbsp;" && dataItem["Percentage"].Text != "&nbsp;")
                {
                    int ComplianceTarget = Convert.ToInt32(dataItem["ComplianceTarget"].Text.Replace("%", ""));
                    int Percentage = Convert.ToInt32(dataItem["Percentage"].Text);
                    if (Percentage >= ComplianceTarget)
                    {
                        dataItem["Percentage"].ForeColor = System.Drawing.Color.Green;
                        dataItem["Percentage"].Text = dataItem["Percentage"].Text + "%";
                    }
                    else
                    {
                        dataItem["Percentage"].ForeColor = System.Drawing.Color.Red;
                        dataItem["Percentage"].Text = dataItem["Percentage"].Text + "%";
                    }
                }
              
            }
        }

        protected void gridOverallSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            FillGrid();
        }
     
    }
}