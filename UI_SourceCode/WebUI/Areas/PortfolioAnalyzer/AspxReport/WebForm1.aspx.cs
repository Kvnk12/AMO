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
using WebUI.Areas.PortfolioAnalyzer.Models;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.AspxReport
{
    public partial class WebForm1 : AspReportpagesBaseClass
    {
        HashSet<string> columnNames = new HashSet<string>();
        Dictionary<string, PieSeries> PieSeriesByRowName = new Dictionary<string, PieSeries>();
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                //if (AffectedItemId == 0)
                //{
                //    using (var db = new Entities())
                //    {
                //        var v = db.IN_Trending_dS(string.Join(",", TicketTypelist));
                //        string year = v.Max(x => x.Year).GetValueOrDefault().ToString();
                //        RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.Equals, RadPivotGrid1.Fields["Year"], year);
                //        //RadPivotGrid1.FilterByValue(PivotGridFilterFunction.Top, RadPivotGrid1.Fields["AffectedItemName"], RadPivotGrid1.Fields["IncidentId"] as PivotGridAggregateField, "5");
                //    }

                //}
                //else
                //{

                //    using (var db = new Entities())
                //    {
                //        var v = db.IN_TrendingByAffectedItem_dS(AffectedItemId.Value, string.Join(",", TicketTypelist));
                //        var year = v.Max(x => x.Year).GetValueOrDefault().ToString();
                //        RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.Contains, RadPivotGrid1.Fields["Year"], year);
                //    }


                //}
            }


            //RadPivotGrid1.Fields["CauseCategoryName"].Caption = ClosureCategoryName;


            columnNames = new HashSet<string>();
            PieSeriesByRowName = new Dictionary<string, PieSeries>();
          //  var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            IList<AnalysisModel> columnlist = PortfolioAnalyzerHelper.APRSourceColumnList(1388).ToList();


            foreach (var v in columnlist)
            {

                //switch (v.ToolColumnName)
              //  {
                    //case "Priority":

                    //    RadPivotGrid1.Fields["Priority"].Caption = v.ClientColumnName;
                    //    break;
                    //case "Status":

                    //    RadPivotGrid1.Fields["Status"].Caption = v.ClientColumnName;
                    //    break;

                    //case "Urgency":

                    //    RadPivotGrid1.Fields["Urgency"].IsHidden = false;
                    //    RadPivotGrid1.Fields["Urgency"].Caption = v.ClientColumnName;

                    //    break;






               // }
            }
            RadPivotGrid1.Rebind();

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (AffectedItemId == 0)
            //{
            //    lblReport.Text = "Monthly Incidents by " + ClosureCategoryName + " for All Applications";
            //}
            //else
            //{
            //    var v = listAffectedItemlist.Where(x => x.Id == AffectedItemId.Value).FirstOrDefault();
            //    lblReport.Text = "Monthly Incidents by " + ClosureCategoryName + "  for " + v.AffectedItemName;
            //}



            //RadPivotGrid1.Rebind();
        }

        //public static DataTable ToDataTable<T>(List<T> items)
        //{
        //    DataTable dataTable = new DataTable(typeof(T).Name);

        //    //Get all the properties
        //    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (PropertyInfo prop in Props)
        //    {
        //        //Defining type of data column gives proper data table 
        //        var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
        //        //Setting column names as Property names
        //        dataTable.Columns.Add(prop.Name, type);
        //    }
        //    foreach (T item in items)
        //    {
        //        var values = new object[Props.Length];
        //        for (int i = 0; i < Props.Length; i++)
        //        {
        //            //inserting property values to datatable rows
        //            values[i] = Props[i].GetValue(item, null);
        //        }
        //        dataTable.Rows.Add(values);
        //    }
        //    //put a breakpoint here and check datatable
        //    return dataTable;
        //}


        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
          //  var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            DataTable source = PortfolioAnalyzerHelper.APRAnalysisReport(1388);
            if (source != null)
            {

                (sender as RadPivotGrid).DataSource = source;
            }
            //if (AffectedItemId == 0)
            //{
            //    using (var db = new Entities())
            //    {
            //        var v = db.IN_Trending_dS(string.Join(",", TicketTypelist));
            //        (sender as RadPivotGrid).DataSource = ToDataTable(v.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList());

            //    }


            //}
            //else
            //{
            //    using (var db = new Entities())
            //    {
            //        var v = db.IN_TrendingByAffectedItem_dS(AffectedItemId.Value, string.Join(",", TicketTypelist));
            //        if (v != null)
            //        {
            //            var selectedAppsource = v.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList();

            //            (sender as RadPivotGrid).DataSource = ToDataTable(selectedAppsource);
            //        }
            //    }

            //}
        }

        protected void RadPivotGrid1_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {
            //this.RadHtmlChart1.PlotArea.Series.Clear();

            PivotGridDataCell dataCell = e.Cell as PivotGridDataCell;
            //if (columnNames.Count == 0)
            //{
            //    //this.RadHtmlChart1.PlotArea.XAxis.Items.Clear();

            //    this.RadHtmlChart1.PlotArea.Series.Clear();

            //}

            if (dataCell != null && dataCell.CellType == PivotGridDataCellType.DataCell)
            {
                string rowName = GetName(dataCell.ParentRowIndexes);

                //string columnName = GetName(dataCell.ParentColumnIndexes);
                //columnName = columnName.Replace('\'', ' ');
                //if (columnNames.Add(columnName))
                //{
                //    AxisItem axisItem = new AxisItem(columnName);
                //    this.RadHtmlChart1.PlotArea.XAxis.Items.Add(axisItem);
                //}
                //PieSeries pieSeries = null;

                //if (PieSeriesByRowName.ContainsKey(rowName))
                //{
                //    pieSeries = PieSeriesByRowName[rowName];
                //}
                //else
                //{
                //    pieSeries = new PieSeries();

                //    PieSeriesByRowName.Add(rowName, pieSeries);
                //    pieSeries.Name = rowName;


                //    // columnSeries.LabelsAppearance.DataFormatString = "C";
                //    this.RadHtmlChart1.PlotArea.Series.Add(pieSeries);
                //}
                //pieSeries.LabelsAppearance.Visible = true;
                //PieSeriesItem item = new PieSeriesItem();
                //int value = 0;
                ////if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out value))
                ////{
                ////    item.Y = value;
                ////}
                ////else
                ////{
                ////    item.Y = null;
                ////}
                //item.Name = rowName;
                //RadHtmlChart1.PlotArea.Series[0].Items.Add(item);
                //pieSeries.SeriesItems.Add(item);
            }
            else if(dataCell.CellType == PivotGridDataCellType.RowGrandTotalDataCell)
            {

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

            PivotGridBaseModelCell modelDataCell = e.PivotGridModelCell as PivotGridBaseModelCell;
            if (modelDataCell != null)
            {
                AddStylesToDataCells(modelDataCell, e);
            }

            if (modelDataCell.TableCellType == PivotGridTableCellType.RowHeaderCell)
            {
                AddStylesToRowHeaderCells(modelDataCell, e);
            }

            if (modelDataCell.TableCellType == PivotGridTableCellType.ColumnHeaderCell)
            {
                AddStylesToColumnHeaderCells(modelDataCell, e);
            }

            if (modelDataCell.IsGrandTotalCell)
            {
                e.ExportedCell.Style.BackColor = Color.FromArgb(128, 128, 128);
                e.ExportedCell.Style.Font.Bold = true;
            }

            if (IsTotalDataCell(modelDataCell))
            {
                e.ExportedCell.Style.BackColor = Color.FromArgb(150, 150, 150);
                e.ExportedCell.Style.Font.Bold = true;
                AddBorders(e);
            }

            if (IsGrandTotalDataCell(modelDataCell))
            {
                e.ExportedCell.Style.BackColor = Color.FromArgb(128, 128, 128);
                e.ExportedCell.Style.Font.Bold = true;
                AddBorders(e);
            }
        }

        private void AddStylesToDataCells(PivotGridBaseModelCell modelDataCell, PivotGridCellExportingArgs e)
        {
            if (modelDataCell.Data != null && modelDataCell.Data.GetType() == typeof(decimal))
            {
                decimal value = Convert.ToDecimal(modelDataCell.Data);
                if (value > 100000)
                {
                    e.ExportedCell.Style.BackColor = Color.FromArgb(51, 204, 204);
                    AddBorders(e);
                }

                e.ExportedCell.Format = "$0.0";
            }
        }

        private void AddStylesToColumnHeaderCells(PivotGridBaseModelCell modelDataCell, PivotGridCellExportingArgs e)
        {
            if (e.ExportedCell.Table.Columns[e.ExportedCell.ColIndex].Width == 0)
            {
                e.ExportedCell.Table.Columns[e.ExportedCell.ColIndex].Width = 200D;
            }

            if (modelDataCell.IsTotalCell)
            {
                e.ExportedCell.Style.BackColor = Color.FromArgb(150, 150, 150);
                e.ExportedCell.Style.Font.Bold = true;
            }
            else
            {
                e.ExportedCell.Style.BackColor = Color.FromArgb(192, 192, 192);
            }
            AddBorders(e);
        }

        private void AddStylesToRowHeaderCells(PivotGridBaseModelCell modelDataCell, PivotGridCellExportingArgs e)
        {
            if (e.ExportedCell.Table.Columns[e.ExportedCell.ColIndex].Width == 0)
            {
                e.ExportedCell.Table.Columns[e.ExportedCell.ColIndex].Width = 80D;
            }
            if (modelDataCell.IsTotalCell)
            {
                e.ExportedCell.Style.BackColor = Color.FromArgb(150, 150, 150);
                e.ExportedCell.Style.Font.Bold = true;
            }
            else
            {
                e.ExportedCell.Style.BackColor = Color.FromArgb(192, 192, 192);
            }

            AddBorders(e);
        }

        private static void AddBorders(PivotGridCellExportingArgs e)
        {
            e.ExportedCell.Style.BorderBottomColor = Color.FromArgb(128, 128, 128);
            e.ExportedCell.Style.BorderBottomWidth = new Unit(1);
            e.ExportedCell.Style.BorderBottomStyle = BorderStyle.Solid;

            e.ExportedCell.Style.BorderRightColor = Color.FromArgb(128, 128, 128);
            e.ExportedCell.Style.BorderRightWidth = new Unit(1);
            e.ExportedCell.Style.BorderRightStyle = BorderStyle.Solid;

            e.ExportedCell.Style.BorderLeftColor = Color.FromArgb(128, 128, 128);
            e.ExportedCell.Style.BorderLeftWidth = new Unit(1);
            e.ExportedCell.Style.BorderLeftStyle = BorderStyle.Solid;

            e.ExportedCell.Style.BorderTopColor = Color.FromArgb(128, 128, 128);
            e.ExportedCell.Style.BorderTopWidth = new Unit(1);
            e.ExportedCell.Style.BorderTopStyle = BorderStyle.Solid;
        }

        private bool IsTotalDataCell(PivotGridBaseModelCell modelDataCell)
        {
            return modelDataCell.TableCellType == PivotGridTableCellType.DataCell &&
               (modelDataCell.CellType == PivotGridDataCellType.ColumnTotalDataCell ||
                 modelDataCell.CellType == PivotGridDataCellType.RowTotalDataCell ||
                 modelDataCell.CellType == PivotGridDataCellType.RowAndColumnTotal);
        }

        private bool IsGrandTotalDataCell(PivotGridBaseModelCell modelDataCell)
        {
            return modelDataCell.TableCellType == PivotGridTableCellType.DataCell &&
                (modelDataCell.CellType == PivotGridDataCellType.ColumnGrandTotalDataCell ||
                    modelDataCell.CellType == PivotGridDataCellType.ColumnGrandTotalRowTotal ||
                    modelDataCell.CellType == PivotGridDataCellType.RowGrandTotalColumnTotal ||
                    modelDataCell.CellType == PivotGridDataCellType.RowGrandTotalDataCell ||
                    modelDataCell.CellType == PivotGridDataCellType.RowAndColumnGrandTotal);
        }

        protected void ButtonExcel_Click(object sender, EventArgs e)
        {
            string alternateText = "Xlsx";
            RadPivotGrid1.ExportSettings.Excel.Format = (PivotGridExcelFormat)Enum.Parse(typeof(PivotGridExcelFormat), alternateText);
            RadPivotGrid1.ExportSettings.IgnorePaging = true;
            RadPivotGrid1.ExportToExcel();
        }
    }
}