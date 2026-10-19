using Data;
using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using WebUI.App_Start;
using WebUI.Areas.IncidentNavigator.Dto;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.AspxReport
{
    public partial class DashboardCostReport : AspReportpagesBaseClass
    {
        HashSet<string> columnNames = new HashSet<string>();
        Dictionary<string, ColumnSeries> ColumnSeriesByRowName = new Dictionary<string, ColumnSeries>();
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
               
            }
            


            columnNames = new HashSet<string>();
            ColumnSeriesByRowName = new Dictionary<string, ColumnSeries>();

            //var sortField = new PivotGridSortExpression();
            //sortField.FieldName = "Year";
            //sortField.SortOrder = PivotGridSortOrder.Ascending;
            //RadPivotGrid1.SortExpressions.Add(sortField);

            var sortField1 = new PivotGridSortExpression();
            sortField1.FieldName = "Category";
            sortField1.SortOrder = PivotGridSortOrder.Ascending;
            RadPivotGrid1.SortExpressions.Add(sortField1);
            foreach (PivotGridField fld in RadPivotGrid1.Fields)
            {
                string caption = fld.Caption.Trim();//.Replace(" ", "");
                fld.Caption = caption;
                //RadPivotGrid1.Fields[fltr.FieldName].Caption = "^" + caption;
            }

            foreach (PivotGridFilter fltr in RadPivotGrid1.Filters)
            {
                string caption = RadPivotGrid1.Fields[fltr.FieldName].Caption;
                RadPivotGrid1.Fields[fltr.FieldName].Caption = " " + caption;
            }
            RadPivotGrid1.Rebind();

           

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CsrfHandler.Validate(this.Page, forgeryToken);
            }
            catch (Exception)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/Home/Appstore");
                }
            }
        }



        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
          //  var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            (sender as RadPivotGrid).DataSource = PortfolioAnalyzerHelper.APRAppFinancialDashbord(objclient.ClientId);
            
        }

        protected void RadPivotGrid1_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {


            PivotGridDataCell dataCell = e.Cell as PivotGridDataCell;
            if (columnNames.Count == 0)
            {
                this.RadHtmlChart1.PlotArea.XAxis.Items.Clear();

                this.RadHtmlChart1.PlotArea.Series.Clear();

            }

            if (dataCell != null && dataCell.CellType == PivotGridDataCellType.DataCell)
            {
                string rowName = GetName(dataCell.ParentRowIndexes);

                string columnName = GetName(dataCell.ParentColumnIndexes);
                columnName = columnName.Replace('\'', ' ');
                if (columnNames.Add(columnName))
                {
                    AxisItem axisItem = new AxisItem(columnName);
                    this.RadHtmlChart1.PlotArea.XAxis.Items.Add(axisItem);
                }
                ColumnSeries columnSeries = null;

                if (ColumnSeriesByRowName.ContainsKey(rowName))
                {
                    columnSeries = ColumnSeriesByRowName[rowName];
                }
                else
                {
                    columnSeries = new ColumnSeries();

                    ColumnSeriesByRowName.Add(rowName, columnSeries);
                    columnSeries.Name = rowName;
                    columnSeries.Stacked = true;

                    // columnSeries.LabelsAppearance.DataFormatString = "C";
                    this.RadHtmlChart1.PlotArea.Series.Add(columnSeries);
                }
                columnSeries.LabelsAppearance.Visible = false;
                CategorySeriesItem item = new CategorySeriesItem();
                int value = 0;
                if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out value))
                {
                    item.Y = value;
                }
                else
                {
                    item.Y = null;
                }
                columnSeries.SeriesItems.Add(item);
            }
        }
        private string GetName(object[] indexes)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object index in indexes)
            {
                builder.Append(index.ToString());
                builder.Append("/");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        protected void RadPivotGrid1_PivotGridCellExporting(object sender, PivotGridCellExportingArgs e)
        {

        }
    }
}