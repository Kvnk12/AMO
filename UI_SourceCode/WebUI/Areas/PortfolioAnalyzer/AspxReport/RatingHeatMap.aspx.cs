using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using WebUI.App_Start;
using WebUI.Areas.PortfolioAnalyzer.Controllers;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.AspxReport
{
    public partial class RatingHeatMap : AspReportpagesBaseClass
    {
        HashSet<string> columnNames = new HashSet<string>();
        Dictionary<string, LineSeries> LineSeriesByRowName = new Dictionary<string, LineSeries>();

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {

            }
            
            
            if (chkClassicViewPivot.Checked)
            {
                RadPivotGrid1.RowTableLayout = PivotGridLayout.Tabular;

            }
            else
            {
                RadPivotGrid1.RowTableLayout = PivotGridLayout.Compact;
            }
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
                //  var cid = (Client)Session["Client"];
                CsrfHandler.Validate(this.Page, forgeryToken);
                var cid = (ClientDetail)Session["SessionClient"];
                int uid = (int)Session["UserId"];
                if (!this.IsPostBack)
                {
                    var v = new DashboardController();
                    var surveyList = v.surveyList(cid.ClientId).ToList();
                    var something = new SelectListItem();
                    something.Text = "Select Survey..";
                    something.Value = "0";
                    surveyList.Add(something);
                    doprdown1.DataSource = surveyList.AsEnumerable();
                    doprdown1.DataBind();
                    if (doprdown1.SelectedValue == "" || doprdown1.SelectedValue == "0")
                    {
                        RadPivotGrid1.DataSource = PortfolioAnalyzerHelper.APRRatingHeatMap(cid.ClientId, 0);
                        RadPivotGrid1.DataBind();
                        RadPivotGrid1.ShowFilterHeaderZone = false;
                        RadPivotGrid1.ColumnGroupsDefaultExpanded = true;
                        RadPivotGrid1.RowGroupsDefaultExpanded = true;
                        RadPivotGrid1.ClientSettings.EnableFieldsDragDrop = false;
                        RadPivotGrid1.AllowFiltering = false;
                        RadPivotGrid1.TotalsSettings.GrandTotalsVisibility = PivotGridGrandTotalsVisibility.None;
                    }
                    // 0 - OverAll Survey(ALL) Ratings
                }
                else
                {
                    if (doprdown1.SelectedValue == "" || doprdown1.SelectedValue == "0")
                    {
                        RadPivotGrid1.DataSource = PortfolioAnalyzerHelper.APRRatingHeatMap(cid.ClientId, 0);

                    }
                    else
                    {
                        var SurveyIdd = Convert.ToInt32(doprdown1.SelectedValue);
                        RadPivotGrid1.DataSource = PortfolioAnalyzerHelper.APRRatingHeatMap(cid.ClientId, SurveyIdd);
                    }
                    RadPivotGrid1.DataBind();
                    RadPivotGrid1.ShowFilterHeaderZone = false;
                    RadPivotGrid1.ColumnGroupsDefaultExpanded = true;
                    RadPivotGrid1.RowGroupsDefaultExpanded = true;
                    RadPivotGrid1.ClientSettings.EnableFieldsDragDrop = false;
                    RadPivotGrid1.AllowFiltering = false;
                    RadPivotGrid1.TotalsSettings.GrandTotalsVisibility = PivotGridGrandTotalsVisibility.None;
                }
            }
            catch (Exception)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/Home/Appstore");
                }
            }

        }
        
        //setting to RagPivot
        protected void setToRadPivot(object sender, DropDownListEventArgs e)
        {
           // var cclient = (Client)Session["Client"];
            var cclient = (ClientDetail)Session["SessionClient"];
            var SurveyIdd = 0;
            if (e.Value != null && e.Value != "" && e.Value != "-1" && e.Value != "0")
            {
                SurveyIdd = Convert.ToInt32(e.Value);

                if (SurveyIdd > 0)
                {
                    RadPivotGrid1.DataSource = PortfolioAnalyzerHelper.APRRatingHeatMap(cclient.ClientId, SurveyIdd);
                    RadPivotGrid1.DataBind();
                    RadPivotGrid1.ShowFilterHeaderZone = false;
                    RadPivotGrid1.ColumnGroupsDefaultExpanded = true;
                    RadPivotGrid1.RowGroupsDefaultExpanded = true;
                    RadPivotGrid1.ClientSettings.EnableFieldsDragDrop = false;
                    RadPivotGrid1.AllowFiltering = false;
                    RadPivotGrid1.TotalsSettings.GrandTotalsVisibility = PivotGridGrandTotalsVisibility.None;
                }

            }
            
        }







        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
            DataTable source= new DataTable();
           // var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            if (doprdown1.SelectedValue != null && doprdown1.SelectedValue != "")
            {
                var Sid_Buffer = doprdown1.SelectedValue;
                var Sid = Convert.ToInt32(Sid_Buffer);
                if (Sid > 0)
                {
                    source = PortfolioAnalyzerHelper.APRRatingHeatMap(objclient.ClientId, Sid);
                }
            }
            if (source != null)
            {
                (sender as RadPivotGrid).DataSource = source;
            }

        }











        protected void RadPivotGrid1_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {
            if (e.Cell is PivotGridDataCell)
            {
                PivotGridDataCell cell = e.Cell as PivotGridDataCell;
                if (cell.DataItem != null)
                {
                    if (cell.DataItem.ToString().Length > 0)
                    {
                        decimal x = Convert.ToDecimal(cell.DataItem);
                        if (x < 3)
                        {
                            cell.BackColor = Color.Red;
                        }
                        else if (x >= 3 && x < 4)
                        {
                            cell.BackColor = Color.Yellow;
                        }
                        else if (x >= 4)
                        {
                            cell.BackColor = Color.Green;
                        }
                        else
                        {
                            cell.BackColor = Color.White;
                        }
                    }
                }
                

            }
            

            //if (columnNames.Count == 0)
            //{
            //    this.RadHtmlChart1.PlotArea.XAxis.Items.Clear();

            //    this.RadHtmlChart1.PlotArea.Series.Clear();

            //}

            //if (dataCell != null && dataCell.CellType == PivotGridDataCellType.DataCell)
            //{
            //    string rowName = GetName(dataCell.ParentRowIndexes);

            //    string columnName = GetName(dataCell.ParentColumnIndexes);
            //    columnName = columnName.Replace('\'', ' ');
            //    if (columnNames.Add(columnName))
            //    {
            //        AxisItem axisItem = new AxisItem(columnName);
            //        this.RadHtmlChart1.PlotArea.XAxis.Items.Add(axisItem);
            //    }
            //    LineSeries lineSeries = null;

            //    if (LineSeriesByRowName.ContainsKey(rowName))
            //    {
            //        lineSeries = LineSeriesByRowName[rowName];
            //    }
            //    else
            //    {
            //        lineSeries = new LineSeries();

            //        LineSeriesByRowName.Add(rowName, lineSeries);
            //        lineSeries.Name = rowName;
            //        lineSeries.Stacked = true;

            //        // columnSeries.LabelsAppearance.DataFormatString = "C";
            //        this.RadHtmlChart1.PlotArea.Series.Add(lineSeries);
            //    }
            //    lineSeries.LabelsAppearance.Visible = false;
            //    CategorySeriesItem item = new CategorySeriesItem();
            //    int value = 0;
            //    if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out value))
            //    {
            //        item.Y = value;
            //    }
            //    else
            //    {
            //        item.Y = null;
            //    }
            //    lineSeries.SeriesItems.Add(item);
            //}
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
        protected void chkClassicViewPivot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClassicViewPivot.Checked)
            {
                RadPivotGrid1.RowTableLayout = PivotGridLayout.Tabular;

            }
            else
            {
                RadPivotGrid1.RowTableLayout = PivotGridLayout.Compact;
            }
            RadPivotGrid1.Rebind();
        }
    }
}