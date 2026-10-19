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
using WebUI.App_Start;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using WebUI.Areas.IdeaManager.Objects;
using WebUI.Areas.IdeaManager.Dto;
using Domain.Entities.Report;
using System.Web.Mvc;
using Telerik.Web.UI.PivotGrid.Core;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Dto;

namespace WebUI.Areas.IdeaManager.AspxReport
{
    public partial class WebForm2 : AspReportpagesBaseClass
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
            //To clear repeating chart series
            ////RadHtmlChart1.PlotArea.XAxis.Items.Clear();
            ////RadHtmlChart1.PlotArea.Series.Clear();
            RadPivotGrid1.Rebind();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //var cid = (Client)Session["Client"];
                CsrfHandler.Validate(this.Page, forgeryToken);
                var cid = (ClientDetail)Session["SessionClient"];
                RadPersistenceManager1.StorageProvider = new GridStateStorageProvider();

                int uid = (int)Session["UserId"];
                if (!this.IsPostBack)
                {
                    var items = Helper.GetViewsList(uid, cid.ClientId, (int)ReportFor.IdeaManager);
                    doprdown1.DataSource = items;
                    doprdown1.DataBind();
                    //if (Session["SysDefault"] == null)
                    //    Session["SysDefault"] = false;

                    //if (Session["ViewId"] != null)
                    //{
                    //    var SessionViewId = (int)Session["ViewId"];
                    //    if (SessionViewId > 0)
                    //    {
                    //        var dViewid = Helper.Find_IsDefault_View(cid.ClientId, uid, (int)ReportFor.IdeaManager);
                    //        if (dViewid.ViewName != null)
                    //        {
                    //            doprdown1.SelectedValue = dViewid.Id.ToString();
                    //            defaultChekbox.Checked = true;
                    //            ViewNameText.Text = dViewid.ViewName;
                    //        }
                    //        doprdown1.SelectedValue = SessionViewId.ToString();
                    //        ViewNameText.Text = doprdown1.SelectedText;
                    //        Helper.settingFiltersToPivot(SessionViewId, ref RadPivotGrid1);
                    //        Helper.setRadPivot(SessionViewId, ref RadPivotGrid1);
                    //        Session["ViewId"] = null;
                    //    }
                    //}

                    //else if (!(bool)Session["SysDefault"])
                    //{
                    //    var dViewid = Helper.Find_IsDefault_View(cid.ClientId, uid, (int)ReportFor.IdeaManager);
                    //    if (dViewid.ViewName != null)
                    //    {
                    //        doprdown1.SelectedValue = dViewid.Id.ToString();
                    //        defaultChekbox.Checked = true;
                    //        ViewNameText.Text = dViewid.ViewName;
                    //        Helper.setRadPivot(dViewid.Id, ref RadPivotGrid1);
                    //        Helper.settingFiltersToPivot(dViewid.Id, ref RadPivotGrid1);
                    //        Session["SysDefault"] = false;
                    //    }
                    //}
                }
                //else
                //{
                //    if (doprdown1.SelectedValue != null && doprdown1.SelectedValue != "")
                //    {
                //        var Vid_Buffer = doprdown1.SelectedValue;
                //        var Vid = Convert.ToInt32(Vid_Buffer);
                //        if (Vid > 0)
                //        {
                //            //removing filters
                //            //RadPivotGrid1.Rebind();
                //            RadPivotGrid1.ClearAllFilters();
                //            Helper.setRadPivot(Vid, ref RadPivotGrid1);
                //            Helper.settingFiltersToPivot(Vid, ref RadPivotGrid1);
                //        }
                //    }
                //}
            }
             catch (Exception)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/Home/Appstore");
                }
            }
        }

        protected void showDefaultView(object sender, EventArgs e)
        {
            //Session["SysDefault"] = true;
            //defaultChekbox.Checked = false;
            Response.Redirect("WebForm2.aspx");
        }

        protected void SaveViewDetails(object sender, EventArgs e)
        {
            if (ViewNameText.Text.Trim() != "")
            {
                var cid = (ClientDetail)Session["SessionClient"];//    (Client)Session["Client"];
                int uid = (int)Session["UserId"];

                string key = cid.ClientId.ToString().Trim() + "|" + uid.ToString().Trim() + "|" + ViewNameText.Text.Trim() + "|" + Convert.ToString((int)ReportFor.IdeaManager).Trim() + "|" + "false" + "|" + defaultChekbox.Checked;

                RadPersistenceManager1.StorageProviderKey = key;
                RadPersistenceManager1.SaveState();
                ViewNameText.Text = string.Empty;
                var items = Helper.GetViewsList(uid, cid.ClientId, (int)ReportFor.IdeaManager);
                doprdown1.DataSource = items;
                doprdown1.DataBind();
                // base.OnPreRender(e);
                // Helper.saveFilterDetails(VieId, ref RadPivotGrid1);
            }
          
        }
        
        //setting to RagPivot
        protected void setToRadPivot(object sender, DropDownListEventArgs e)
        {
            ClientDetail cid = (ClientDetail)Session["SessionClient"];//    (Client)Session["Client"];
            int uid = (int)Session["UserId"];
            var ViewIdd = 0;
            if (e.Value != null && e.Value != "" && e.Value != "-1" && e.Value != "0")
            {
                ViewNameText.Text = e.Text;
                ViewIdd = Convert.ToInt32(e.Value);
                string key = cid.ClientId.ToString().Trim() + "|" + uid.ToString().Trim() + "|" + ViewNameText.Text.Trim() + "|" + Convert.ToString((int)ReportFor.IdeaManager).Trim() + "|" + "false" + "|" + "false";

                string fileId = key;// Session["CustomPersistenceSettingsKey"].ToString();
                RadPersistenceManager1.StorageProviderKey = fileId;
                RadPersistenceManager1.LoadState();
                RadPivotGrid1.Rebind();
            }
            else
            {
                Response.Redirect("WebForm2.aspx");
            }
        }


        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
            //var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            //var source = (from IMposts in db.IMPostIdea
            //              where IMposts.CategoryId== objclient.Id
            //              Se);
            DataTable source = IdeaManagerHelper.IMTrendingReportList(objclient.ClientId);
            if (source != null)
            {

                (sender as RadPivotGrid).DataSource = source;
            }
            ////RadHtmlChart1.PlotArea.XAxis.Items.Clear();
            ////RadHtmlChart1.PlotArea.Series.Clear();

        }

        protected void RadPivotGrid1_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {


            PivotGridDataCell dataCell = e.Cell as PivotGridDataCell;
            if (columnNames.Count == 0)
            {
                ////this.RadHtmlChart1.PlotArea.XAxis.Items.Clear();

                ////this.RadHtmlChart1.PlotArea.Series.Clear();

            }

            if (dataCell != null && dataCell.CellType == PivotGridDataCellType.DataCell)
            {
                string rowName = GetName(dataCell.ParentRowIndexes);

                string columnName = GetName(dataCell.ParentColumnIndexes);
                columnName = columnName.Replace('\'', ' ');
                if (columnNames.Add(columnName))
                {
                    AxisItem axisItem = new AxisItem(columnName);
                    ////this.RadHtmlChart1.PlotArea.XAxis.Items.Add(axisItem);
                }
                LineSeries lineSeries = null;

                if (LineSeriesByRowName.ContainsKey(rowName))
                {
                    lineSeries = LineSeriesByRowName[rowName];
                }
                else
                {
                    lineSeries = new LineSeries();

                    LineSeriesByRowName.Add(rowName, lineSeries);
                    lineSeries.Name = rowName;
                   // lineSeries.Stacked = true;

                    // columnSeries.LabelsAppearance.DataFormatString = "C";
                    ////this.RadHtmlChart1.PlotArea.Series.Add(lineSeries);
                }
                lineSeries.LabelsAppearance.Visible = false;
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
                lineSeries.SeriesItems.Add(item);
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