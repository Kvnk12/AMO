using Data;
using Domain.Entities.Report;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
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
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IncidentNavigator.Dto;
using WebUI.Dto;
using log4net;

namespace WebUI.Areas.IncidentNavigator.AspxReport
{
    public partial class InflowOutflowAnalysis : AspReportpagesBaseClass
    {
        HashSet<string> columnNames = new HashSet<string>();
        Dictionary<string, LineSeries> LineSeriesByRowName = new Dictionary<string, LineSeries>();
        int ClientId = 0;

        private static readonly ILog Log = LogManager.GetLogger(typeof(InflowOutflowAnalysis));
        protected override void OnPreRender(EventArgs e)
        {
            try {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                ClientId = client.ClientId;
                base.OnPreRender(e);
            if (!Page.IsPostBack)
            {

            }


           
            columnNames = new HashSet<string>();
            LineSeriesByRowName = new Dictionary<string, LineSeries>();
           // var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            IList<TrendingReportInput> columnlist = IncidentNavigatorHelper.INCubeSourceColumnList(objclient.ClientId).ToList();
            foreach (var v in columnlist)
            {
                switch (v.ToolColumnName)
                {
                    case "Priority":
                        if (RadPivotGrid1.Fields["Priority"] != null)
                        {
                            RadPivotGrid1.Fields["Priority"].Caption = v.ClientColumnName;
                        }
                        break;
                    case "Status":
                        if (RadPivotGrid1.Fields["Status"] != null)
                        {
                            RadPivotGrid1.Fields["Status"].Caption = v.ClientColumnName;
                        }
                        break;
                 
                    default:
                        if (RadPivotGrid1.Fields[v.ToolColumnName] != null && v.ToolColumnName != "TicketFlow" && v.ToolColumnName != "year" && v.ToolColumnName != "Resolutiontime" && v.ToolColumnName != "SLACostPerTicket")
                        {
                            RadPivotGrid1.Fields[v.ToolColumnName].IsHidden = false;
                            RadPivotGrid1.Fields[v.ToolColumnName].Caption = v.ClientColumnName;
                        }
                        break;
                }
                

            }
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
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   OnPreRender , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }

}
protected void Page_Load(object sender, EventArgs e)
        {
            try { 
            CsrfHandler.Validate(this.Page, forgeryToken);
            RadPersistenceManager1.StorageProvider = new GridStateStorageProvider();
          //  var cid = (Client)Session["Client"];
            var cid = (ClientDetail)Session["SessionClient"]; 
            int uid = (int)Session["UserId"];
            if (!this.IsPostBack)
            {
                var items = Helper.GetViewsList(uid, cid.ClientId, (int)ReportFor.InflowOutflowAnalysis);
                doprdown1.DataSource = items;
                doprdown1.DataBind();
                if (Session["SysDefault"] == null)
                    Session["SysDefault"] = false;
                if (!(bool)Session["SysDefault"])
                {
                    var dViewid = Helper.Find_IsDefault_View(cid.ClientId, uid, (int)ReportFor.InflowOutflowAnalysis);
                    if (dViewid.ViewName != null)
                    {
                        doprdown1.SelectedValue = dViewid.Id.ToString();
                        defaultChekbox.Checked = true;
                        ViewNameText.Text = dViewid.ViewName;
                        // Helper.setRadPivot(dViewid.Id, ref RadPivotGrid1);
                        //  Helper.settingFiltersToPivot(dViewid.Id, ref RadPivotGrid1);

                        string key = cid.ClientId.ToString().Trim() + "|" + uid.ToString().Trim() + "|" + ViewNameText.Text.Trim() + "|" + Convert.ToString((int)ReportFor.InflowOutflowAnalysis).Trim() + "|" + defaultChekbox.Checked.ToString() + "|" + "false";
                        // Helper.setRadPivot(ViewIdd, ref RadPivotGrid1);
                        // Helper.settingFiltersToPivot(ViewIdd, ref RadPivotGrid1);
                        string fileId = key;// Session["CustomPersistenceSettingsKey"].ToString();
                        RadPersistenceManager1.StorageProviderKey = fileId;
                        RadPersistenceManager1.LoadState();
                        RadPivotGrid1.Rebind();
                        Session["SysDefault"] = false;
                    }
                }
            }
            else
            {
                ////if (doprdown1.SelectedValue != null && doprdown1.SelectedValue != "")
                ////{
                ////    var Vid_Buffer = doprdown1.SelectedValue;
                ////    var Vid = Convert.ToInt32(Vid_Buffer);
                ////    if (Vid > 0)
                ////    {
                ////        Helper.setRadPivot(Vid, ref RadPivotGrid1);
                ////        Helper.settingFiltersToPivot(Vid, ref RadPivotGrid1);
                ////    }
                ////}
            }
            //RadPivotGrid1.Rebind();
        }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   Page_Load , Client ID is " + ((ClientDetail) Session["SessionClient"]).ClientId, ex);
            }
}

protected void showDefaultView(object sender, EventArgs e)
        {
            try { 
            Session["SysDefault"] = true;
            defaultChekbox.Checked = false;
            Response.Redirect("InflowOutflowAnalysis.aspx");
            }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   showDefaultView , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }

        }

        protected void SaveViewDetails(object sender, EventArgs e)
        {
            try { 
            if (ViewNameText.Text.Trim() != "")
            {
               // var cid = (Client)Session["Client"];
                var cid = (ClientDetail)Session["SessionClient"];
                int uid = (int)Session["UserId"];

                string key = cid.ClientId.ToString().Trim() + "|" + uid.ToString().Trim() + "|" + ViewNameText.Text.Trim() + "|" + Convert.ToString((int)ReportFor.InflowOutflowAnalysis).Trim() + "|" + defaultChekbox.Checked.ToString()+"|" + "false";

                 RadPersistenceManager1.StorageProviderKey = key;
                 RadPersistenceManager1.SaveState();

                //var VieId = Helper.SaveReportViewDetails(cid.Id, uid, defaultChekbox.Checked, ViewNameText.Text, RadPivotGrid1.Fields, (int)ReportFor.InflowOutflowAnalysis);
                ViewNameText.Text = string.Empty;
                var items = Helper.GetViewsList(uid, cid.ClientId, (int)ReportFor.InflowOutflowAnalysis);
                doprdown1.DataSource = items;
                doprdown1.DataBind();
                base.OnPreRender(e);
               // Helper.saveFilterDetails(VieId, ref RadPivotGrid1);
            }
            }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   SaveViewDetails , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }

        }

        //setting to RagPivot
        protected void setToRadPivot(object sender, DropDownListEventArgs e)
        {
            try{
                ClientDetail cid = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var ViewIdd = 0;
            if (e.Value != null && e.Value != "" && e.Value != "-1" && e.Value != "0")
            {
                ViewNameText.Text = e.Text;
                ViewIdd = Convert.ToInt32(e.Value);
                var chek = Helper.IsCheckedForReport(ViewIdd);
                if (chek)
                {
                    defaultChekbox.Checked = true;
                }
                else
                    defaultChekbox.Checked = false;
                string key = cid.ClientId.ToString().Trim() + "|" + uid.ToString().Trim() + "|" + ViewNameText.Text.Trim() + "|" + Convert.ToString((int)ReportFor.InflowOutflowAnalysis).Trim() + "|" + defaultChekbox.Checked.ToString() + "|" + "false";

                // Helper.setRadPivot(ViewIdd, ref RadPivotGrid1);
                // Helper.settingFiltersToPivot(ViewIdd, ref RadPivotGrid1);

                string fileId = key;// Session["CustomPersistenceSettingsKey"].ToString();
                RadPersistenceManager1.StorageProviderKey = fileId;
                RadPersistenceManager1.LoadState();
                RadPivotGrid1.Rebind();

            }
            else
            {
                Session["SysDefault"] = true;
                defaultChekbox.Checked = false;
                Response.Redirect("InflowOutflowAnalysis.aspx");
            }
            }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   setToRadPivot , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }

        }


        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
            try { 
           // var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            DataTable source = IncidentNavigatorHelper.INInflowOutflowAnalysis(objclient.ClientId);
            if (source != null)
            {

                (sender as RadPivotGrid).DataSource = source;
            }
        }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   RadPivotGrid1_NeedDataSource , Client ID is " + ((ClientDetail) Session["SessionClient"]).ClientId, ex);
            }

}

protected void RadPivotGrid1_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {
            try { 

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
                    //   lineSeries.Stacked = true;

                    // columnSeries.LabelsAppearance.DataFormatString = "C";
                    this.RadHtmlChart1.PlotArea.Series.Add(lineSeries);
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
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   RadPivotGrid1_CellDataBound , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }

        }
        private string GetName(object[] indexes)
        {
            try { 
            StringBuilder builder = new StringBuilder();
            foreach (object index in indexes)
            {
                builder.Append(index.ToString());
                builder.Append("/");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   GetName , Client ID is " + ((ClientDetail) Session["SessionClient"]).ClientId, ex);
            }
return "";
}

protected void RadPivotGrid1_PivotGridCellExporting(object sender, PivotGridCellExportingArgs e)
        {
            try { 
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
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   RadPivotGrid1_PivotGridCellExporting , Client ID is " + ((ClientDetail) Session["SessionClient"]).ClientId, ex);
            }


}

private void AddStylesToDataCells(PivotGridBaseModelCell modelDataCell, PivotGridCellExportingArgs e)
        {
            try { 
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
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   AddStylesToDataCells , Client ID is " + ((ClientDetail) Session["SessionClient"]).ClientId, ex);
            }

}

private void AddStylesToColumnHeaderCells(PivotGridBaseModelCell modelDataCell, PivotGridCellExportingArgs e)
        {
            try { 
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
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   AddStylesToColumnHeaderCells , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }

        }

        private void AddStylesToRowHeaderCells(PivotGridBaseModelCell modelDataCell, PivotGridCellExportingArgs e)
        {
            try { 
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
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   AddStylesToRowHeaderCells , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }

        }

        private static void AddBorders(PivotGridCellExportingArgs e)
        {
            try{
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
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   AddBorders , Client ID is " , ex);
            }

        }

        private bool IsTotalDataCell(PivotGridBaseModelCell modelDataCell)
        {
            try
            {
                return modelDataCell.TableCellType == PivotGridTableCellType.DataCell &&
                   (modelDataCell.CellType == PivotGridDataCellType.ColumnTotalDataCell ||
                     modelDataCell.CellType == PivotGridDataCellType.RowTotalDataCell ||
                     modelDataCell.CellType == PivotGridDataCellType.RowAndColumnTotal);
            }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   IsTotalDataCell , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }
            return false;
        }

        private bool IsGrandTotalDataCell(PivotGridBaseModelCell modelDataCell)
        {
            try { 
            return modelDataCell.TableCellType == PivotGridTableCellType.DataCell &&
                (modelDataCell.CellType == PivotGridDataCellType.ColumnGrandTotalDataCell ||
                    modelDataCell.CellType == PivotGridDataCellType.ColumnGrandTotalRowTotal ||
                    modelDataCell.CellType == PivotGridDataCellType.RowGrandTotalColumnTotal ||
                    modelDataCell.CellType == PivotGridDataCellType.RowGrandTotalDataCell ||
                    modelDataCell.CellType == PivotGridDataCellType.RowAndColumnGrandTotal);
            }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   IsGrandTotalDataCell , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }
            return false;

        }

        protected void ButtonExcel_Click(object sender, EventArgs e)
        {
            try { 
            string alternateText = "Xlsx";
            RadPivotGrid1.ExportSettings.Excel.Format = (PivotGridExcelFormat)Enum.Parse(typeof(PivotGridExcelFormat), alternateText);
            RadPivotGrid1.ExportSettings.IgnorePaging = true;
            RadPivotGrid1.ExportToExcel();
            }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   ButtonExcel_Click , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }

        }

        protected void chkClassicViewPivot_CheckedChanged(object sender, EventArgs e)
        {
            try { 
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
            catch (Exception ex)
            {
                Log.Error("InflowOutflowAnalysis   chkClassicViewPivot_CheckedChanged , Client ID is " + ((ClientDetail)Session["SessionClient"]).ClientId, ex);
            }

        }



        ////protected void RadPivotGrid1_GetDescriptionsDataCompleted(object sender, GetDescriptionsDataCompletedEventArgs e)
        ////{
        ////    var root = e.DescriptionsData.RootFieldInfo;

        ////    if (root.HasChildren)
        ////    {
        ////        List<Telerik.Web.UI.PivotGrid.Core.Fields.ContainerNode> nodes = new List<Telerik.Web.UI.PivotGrid.Core.Fields.ContainerNode>();

        ////        foreach (var item in root.Children)
        ////        {
        ////            nodes.Add(item);
        ////        }

        ////        foreach (var item in nodes)
        ////        {
        ////            if (item.Name != "[Account]" &&
        ////                item.Name != "[Department]")
        ////            {
        ////               // item.
        ////                root.Children.Remove(item);
        ////            }
        ////        }
        ////    }
        ////}

    }
}