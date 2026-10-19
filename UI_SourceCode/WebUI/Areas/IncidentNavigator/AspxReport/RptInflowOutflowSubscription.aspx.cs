using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using NTTData.AMOPortal.Data;
using WebUI.Areas.IncidentNavigator.Dto;
using WebUI.Areas.IncidentNavigator.Models;
using WebUI.Areas.ClientProfileManager;
using System.Data;
using System.Text;
using System.Drawing;
using System.IO;
using Data;

namespace WebUI.Areas.IncidentNavigator.AspxReport
{
    public partial class RptInflowOutflowSubscription : System.Web.UI.Page
    {
        HashSet<string> columnNames = new HashSet<string>();
        string ClientName = null;
        string ClientId = null;
        string StartDate = null;
        string EndDate = null;
        string DateType = null;
        string Key = null;
        DateTime fromDate = new DateTime();
        DateTime toDate = new DateTime();
        Dictionary<string, LineSeries> LineSeriesByRowName = new Dictionary<string, LineSeries>();
        Dictionary<string, ColumnSeries> BarSeriesByRowName = new Dictionary<string, ColumnSeries>();
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            columnNames = new HashSet<string>();
            LineSeriesByRowName = new Dictionary<string, LineSeries>();
            BarSeriesByRowName = new Dictionary<string, ColumnSeries>();
            RadPivotGrid1.RowTableLayout = PivotGridLayout.Tabular;

            foreach (PivotGridField fld in RadPivotGrid1.Fields)
            {
                string caption = fld.Caption.Replace(" ", "");
                fld.Caption = caption;
                //RadPivotGrid1.Fields[fltr.FieldName].Caption = "^" + caption;
            }

            foreach (PivotGridFilter fltr in RadPivotGrid1.Filters)
            {
                string caption = RadPivotGrid1.Fields[fltr.FieldName].Caption;//.Replace("^", "");
                RadPivotGrid1.Fields[fltr.FieldName].Caption = " " + caption;
            }
            RadPivotGrid1.Rebind();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RadPersistenceManager1.StorageProvider = new GridStateStorageProvider();
                var rptUID =  Request.QueryString["id"];
                if (!IsPostBack)
                {
                    Db db = new Db();
                    var rptJob = (from a in db.RPTJobDetail
                                  where a.ReportUID == rptUID
                                  select new
                                  {
                                      subid = a.RPTSubscriptionId,
                                      startDate = a.RPTStartDate,
                                      endDate = a.RPTEndDate
                                  }).FirstOrDefault();
                    var rptParam = (from a in db.RPTSubscription
                                    where a.Id == rptJob.subid
                                    select new
                                    {
                                        param = a.ReportParameters,
                                        clientid = a.ClientID,
                                        rptname = a.ReportName,
                                        rptFor = a.ReportFor
                                    }).FirstOrDefault();
                    var parameter = rptParam.param.Trim(new Char[] { '{', '}', ' ' });
                    var paramList = parameter.Split(',');

                    ClientName = paramList[0].Substring(paramList[0].IndexOf(":") + 1);
                    ClientId = paramList[1].Substring(paramList[1].IndexOf(":") + 1);
                    StartDate = paramList[2].Substring(paramList[2].IndexOf(":") + 1);
                    EndDate = paramList[3].Substring(paramList[3].IndexOf(":") + 1);
                    Key = paramList[4].Substring(paramList[4].IndexOf(":") + 1);

                    fromDate = rptJob.startDate.Value;
                    toDate = rptJob.endDate.Value;

                    var Img = ClientProfileManagerHelper.getProfileClientDetail(int.Parse(ClientId));
                    if (Img.CPMFile != null)
                    {
                        string imageBase64Data = Convert.ToBase64String(Img.CPMFile);
                        clientlogo.ImageUrl = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    }
                    else
                        clientNameLabel.Text = ClientName;

                    RadPivotGrid1.Rebind();
                }
                
            }
            catch (Exception ex)
            {
            }

        }

        protected void showDefaultView(object sender, EventArgs e)
        {
            Response.Redirect("BackLogAnalysis.aspx");
        }

        protected void ApplyButtonClick(object sender, EventArgs e)
        {
            Refresh();
        }
        protected void Refresh()
        {
            RadPivotGrid1.Rebind();

        }


        //setting to RagPivot


        protected void RadPivotGrid1_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {

            if (columnNames.Count == 0)
            {
                this.RadHtmlChart1.PlotArea.XAxis.Items.Clear();
                this.RadHtmlChart1.PlotArea.Series.Clear();
            }
            PivotGridDataCell dataCell = e.Cell as PivotGridDataCell;

            if (dataCell != null && dataCell.IsGrandTotalCell && dataCell.Field.Caption.IndexOf("Cumulative") >= 0)
            {
                dataCell.Text = "";
            }
            PivotGridRowHeaderCell rowheader = e.Cell as PivotGridRowHeaderCell;
            if (rowheader != null)// && rowheader.CellType == PivotGridRowHeaderCell.RowHeader PivotGridDataCellType.ColumnGrandTotalDataCell)
            {
                string st = rowheader.Text.ToString();

                if (st.IndexOf("Sum of ") >= 0 || st.IndexOf("Max of ") >= 0 || st.IndexOf("Min of ") >= 0)
                {
                    rowheader.Text = st.Substring(7, st.Length - 7);
                }
                else if (st.IndexOf("Count of ") >= 0)
                {
                    rowheader.Text = st.Substring(9, st.Length - 9);
                }

            }
            if (dataCell != null && dataCell.CellType == PivotGridDataCellType.DataCell)
            {
                string rowName = GetName(dataCell.ParentRowIndexes);
                rowName = rowName.Replace("Sum of", "");
                // rowName = rowName;
                string columnName = GetName(dataCell.ParentColumnIndexes);
                columnName = columnName.Replace('\'', ' ');
                if (columnNames.Add(columnName))
                {
                    AxisItem axisItem = new AxisItem(columnName);
                    this.RadHtmlChart1.PlotArea.XAxis.Items.Add(axisItem);
                }
                var chartType = ChartDrptype.SelectedItem.Value;
                if (chartType == "Bar")
                {
                    ColumnSeries barSeries = null;

                    if (BarSeriesByRowName.ContainsKey(rowName))
                    {
                        barSeries = BarSeriesByRowName[rowName];
                    }
                    else
                    {
                        barSeries = new ColumnSeries();
                        BarSeriesByRowName.Add(rowName, barSeries);
                        barSeries.Name = rowName + "||"; ;
                        this.RadHtmlChart1.PlotArea.Series.Add(barSeries);
                    }
                    barSeries.LabelsAppearance.Visible = true;
                    barSeries.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.InsideEnd;
                    switch (rowName)
                    {
                        case "Created Tickets":
                            barSeries.Appearance.FillStyle.BackgroundColor = Color.Black;
                            break;
                        case "Resolved Tickets":
                            barSeries.Appearance.FillStyle.BackgroundColor = Color.Red;
                            break;
                        case "Resolved Backlog":
                            barSeries.Appearance.FillStyle.BackgroundColor = Color.Magenta;
                            break;
                        case "Max of Cumulative Resolved Backlog":
                            barSeries.Appearance.FillStyle.BackgroundColor = Color.Yellow;
                            break;

                    }

                    CategorySeriesItem item = new CategorySeriesItem();
                    int value = 0;
                    if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out value))
                    {
                        if (rowName.IndexOf("Cumulative") >= 0)
                        {
                            int c = barSeries.SeriesItems.Count;
                            //if (c == 0)
                            //{
                            //    e.Cell.DataItem = value;
                            //    e.Cell.Text = Convert.ToString(value);
                            //}
                            //else
                            //{
                            //    e.Cell.DataItem = value + barSeries.SeriesItems[c - 1].Y;
                            //    e.Cell.Text = Convert.ToString(value + barSeries.SeriesItems[c - 1].Y);
                            //}
                            // e.Cell.RenderControl(new HtmlTextWriter(System.IO.TextWriter.Null));
                            int val2 = 0;
                            if (int.TryParse(e.Cell.DataItem.ToString(), out val2))
                            {
                                item.Y = val2;
                            }
                            else
                                item.Y = 0;
                        }
                        else
                        {
                            item.Y = value;
                        }
                    }
                    else
                    {
                        item.Y = 0;
                    }
                    barSeries.SeriesItems.Add(item);
                }
                else if (chartType == "Line")
                {
                    LineSeries lineSeries = null;
                    if (LineSeriesByRowName.ContainsKey(rowName))
                    {
                        lineSeries = LineSeriesByRowName[rowName];
                    }
                    else
                    {
                        lineSeries = new LineSeries();
                        LineSeriesByRowName.Add(rowName, lineSeries);
                        lineSeries.Name = rowName + "=";
                        this.RadHtmlChart1.PlotArea.Series.Add(lineSeries);

                    }
                    lineSeries.LabelsAppearance.Visible = true;
                    lineSeries.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                    CategorySeriesItem item = new CategorySeriesItem();
                    int value = 0;
                    if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out value))
                    {
                        if (rowName.IndexOf("Cumulative") >= 0)
                        {
                            int c = lineSeries.SeriesItems.Count;
                            //if (c == 0)
                            //{
                            //    e.Cell.DataItem = value;
                            //    e.Cell.Text = Convert.ToString(value);
                            //}
                            //else
                            //{
                            //    e.Cell.DataItem = value + lineSeries.SeriesItems[c - 1].Y;
                            //    e.Cell.Text = Convert.ToString(value + lineSeries.SeriesItems[c - 1].Y);
                            //}
                            // e.Cell.RenderControl(new HtmlTextWriter(System.IO.TextWriter.Null));
                            int val2 = 0;
                            if (int.TryParse(e.Cell.DataItem.ToString(), out val2))
                            {
                                item.Y = val2;
                            }
                            else
                                item.Y = 0;
                        }
                        else
                        {
                            item.Y = value;
                        }

                    }
                    else
                    {
                        item.Y = 0;
                    }
                    lineSeries.SeriesItems.Add(item);
                }
                else
                {

                    LineSeries lineSeries = null;
                    ColumnSeries barSeries = null;

                    if (LineSeriesByRowName.ContainsKey(rowName) || BarSeriesByRowName.ContainsKey(rowName))
                    {
                        lineSeries = LineSeriesByRowName[rowName];
                        barSeries = BarSeriesByRowName[rowName];
                    }
                    else
                    {
                        lineSeries = new LineSeries();
                        barSeries = new ColumnSeries();
                        LineSeriesByRowName.Add(rowName, lineSeries);
                        BarSeriesByRowName.Add(rowName, barSeries);
                        lineSeries.Name = rowName + "=";
                        barSeries.Name = rowName + "||"; ;
                        //   lineSeries.Stacked = true;
                        // columnSeries.LabelsAppearance.DataFormatString = "C";
                        this.RadHtmlChart1.PlotArea.Series.Add(lineSeries);
                        this.RadHtmlChart1.PlotArea.Series.Add(barSeries);
                    }
                    barSeries.LabelsAppearance.Visible = true;
                    barSeries.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.InsideEnd;
                    lineSeries.LabelsAppearance.Visible = true;
                    lineSeries.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                    CategorySeriesItem item = new CategorySeriesItem();
                    int value = 0;
                    if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out value))
                    {
                        if (rowName.IndexOf("Cumulative") >= 0)
                        {
                            int c = lineSeries.SeriesItems.Count;
                            //if (c == 0)
                            //{
                            //    e.Cell.DataItem = value;
                            //    e.Cell.Text = Convert.ToString(value);
                            //}
                            //else
                            //{
                            //    e.Cell.DataItem = value + lineSeries.SeriesItems[c - 1].Y;
                            //    e.Cell.Text = Convert.ToString(value + lineSeries.SeriesItems[c - 1].Y);

                            //}
                            // e.Cell.RenderControl(new HtmlTextWriter(System.IO.TextWriter.Null));
                            int val2 = 0;
                            if (int.TryParse(e.Cell.DataItem.ToString(), out val2))
                            {
                                item.Y = val2;
                            }
                            else
                                item.Y = 0;
                        }
                        else
                        {

                            item.Y = value;
                        }
                    }
                    else
                    {
                        item.Y = 0;
                    }
                    lineSeries.SeriesItems.Add(item);
                    barSeries.SeriesItems.Add(item);
                }
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

        ////protected void chkClassicViewPivot_CheckedChanged(object sender, EventArgs e)
        ////{
        ////    if (chkClassicViewPivot.Checked)
        ////    {
        ////        RadPivotGrid1.RowTableLayout = PivotGridLayout.Tabular;
        ////    }
        ////    else
        ////    {
        ////        RadPivotGrid1.RowTableLayout = PivotGridLayout.Compact;
        ////    }
        ////    RadPivotGrid1.Rebind();
        ////}

        protected void RadPivotGrid1_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
        {
            try
            {
                // List<INTrendingReportEntity> replst = new List<INTrendingReportEntity>();
                string strGroupName = "";
                string strColumnFltrs = "";
                List<String> Grplist = new List<string>()
                    { "Year", "Quarter", "Month","Week", "Date" };
                List<String> Rowlist = new List<string>()
                    { "CumulativeResolvedBacklog", "CumulativeClosedBacklog"};
                bool bCumulativeflag = false;
                string TowerLst = "";
                string LocationLst = "";
                string TypeLst = "";
                string ServiceareaLst = "";
                string PriorityLst = "";
                string strAssignmentGps = "";
                foreach (PivotGridFilter fltr in RadPivotGrid1.Filters)
                {
                    string FieldName = "";
                    string fltlst = "";

                    if (fltr is Telerik.Web.UI.PivotGridLabelGroupFilter)
                    {
                        var fltrfld = (PivotGridLabelGroupFilter)fltr;
                        foreach (var item in ((Telerik.Web.UI.PivotGridSetCondition)fltrfld.Condition).Items)
                        {
                            fltlst += item.ToString() + ",";

                        }
                        FieldName = fltrfld.FieldName;
                    }
                    else
                    {
                        var fltrfld = (PivotGridReportFilter)fltr;
                        foreach (var item in ((Telerik.Web.UI.PivotGridSetCondition)fltrfld.Condition).Items)
                        {
                            fltlst += item.ToString() + ",";

                        }
                        FieldName = fltrfld.FieldName;
                    }
                    if (fltlst.Length > 1)
                    {
                        if (FieldName == "Tower")
                        {

                            TowerLst = fltlst.Substring(0, fltlst.Length - 1);

                        }
                        else if (FieldName == "Type")
                        {
                            TypeLst = fltlst.Substring(0, fltlst.Length - 1);
                        }
                        else if (FieldName == "Priority")
                        {
                            PriorityLst = fltlst.Substring(0, fltlst.Length - 1);
                        }
                        else if (FieldName == "Service area")
                        {
                            ServiceareaLst = fltlst.Substring(0, fltlst.Length - 1);
                        }
                        else if (FieldName == "Location")
                        {
                            LocationLst = fltlst.Substring(0, fltlst.Length - 1);
                        }
                    }
                }

                foreach (PivotGridField fld in RadPivotGrid1.Fields)
                {
                    if (fld.FieldType == "PivotGridColumnField" && strGroupName.Length < 1)
                    {
                        strGroupName = fld.Caption.Replace(" ", "");
                        strGroupName = strGroupName.Substring(strGroupName.IndexOf('-') + 1);
                        if (!Grplist.Contains(strGroupName))
                        {
                            if (strGroupName.IndexOf("Priority") >= 0)
                                strGroupName = "ClientPriority";
                            else if (strGroupName.IndexOf("Service Area") >= 0)
                                strGroupName = "ServiceArea";
                            strColumnFltrs += strGroupName.ToString() + ",";
                            strGroupName = "";
                        }



                    }
                    else if (fld.FieldType == "PivotGridAggregateField" && !bCumulativeflag)
                    {
                        var strName = fld.Caption.Replace(" ", "");
                        strName = strName.Substring(strName.IndexOf('-') + 1);
                        if (Rowlist.Contains(strName))
                            bCumulativeflag = true;

                    }

                }

                if (strColumnFltrs.Length > 1)
                {
                    strColumnFltrs = strColumnFltrs.Substring(0, strColumnFltrs.Length - 1);
                }
                    
                DataTable replst = IncidentNavigatorHelper.INBackLogAnalysis(int.Parse(ClientId), fromDate, toDate, strGroupName, bCumulativeflag, TowerLst, LocationLst, TypeLst, ServiceareaLst, PriorityLst, strAssignmentGps, strColumnFltrs);

                if (replst != null)
                {
                    //  (sender as RadPivotGrid).UseQueryableDataProvider = true;

                    (sender as RadPivotGrid).DataSource = replst;//.AsEnumerable();
                }


            }

            catch (Exception ex)
            {
                //Log.Error("BackLogAnalysis RadPivotGrid1_NeedDataSource ," + ex);
            }
        }



        protected void RadPivotGrid1_AddingFieldToZone(object sender, PivotGridAddingFieldToZoneEventArgs e)
        {
            if (e.Field.FieldType == "PivotGridAggregateField" && e.Field.Caption.IndexOf("Cumulative") > 0)
            {
                PivotGridAggregateField fld = (PivotGridAggregateField)e.Field;
                fld.Aggregate = PivotGridAggregate.Max;
            }
        }

        protected void ChartDrp_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            RadPivotGrid1.Rebind();
        }

    }

}