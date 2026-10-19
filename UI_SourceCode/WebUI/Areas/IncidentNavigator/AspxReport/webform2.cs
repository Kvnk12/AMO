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

namespace WebUI.Areas.IncidentNavigator.AspxReport
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        HashSet<string> columnNames = new HashSet<string>();
        Dictionary<string, LineSeries> LineSeriesByRowName = new Dictionary<string, LineSeries>();
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
               
            }


            //RadPivotGrid1.Fields["CauseCategoryName"].Caption = ClosureCategoryName;


            columnNames = new HashSet<string>();
            LineSeriesByRowName = new Dictionary<string, LineSeries>();
            var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            //using (var contextFactory = new DbContextFactory())
            //{
            //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //    var target = unitOfWork.INIncidentServicehours.Get(x => x.ClientId == objclient.Id).ToList();
                Db db = new Db();
                var target = (from insvchrs in db.INIncidentServicehours
                                where insvchrs.ClientId == objclient.Id
                                select insvchrs).ToList();
                if (target.Count > 0)
                {
                    if (Convert.ToInt32(target.FirstOrDefault().HasSLAManager) == 1)
                    {
                        if (RadPivotGrid1.Fields["Resolutiontime"] != null)
                        {
                            RadPivotGrid1.Fields["Resolutiontime"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["SLACostPerTicket"]!=null)
                        {
                            RadPivotGrid1.Fields["SLACostPerTicket"].IsHidden = false;
                        }
                    }
                    else
                    {
                        if (RadPivotGrid1.Fields["INResolutiontime"] != null)
                        {
                            RadPivotGrid1.Fields["INResolutiontime"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["INCostPerTicket"] != null)
                        {
                            RadPivotGrid1.Fields["INCostPerTicket"].IsHidden = false;
                        }
                    }


                }

                else
                {
                    if (RadPivotGrid1.Fields["Resolutiontime"] != null)
                    {
                        RadPivotGrid1.Fields["Resolutiontime"].IsHidden = false;
                    }
                    if (RadPivotGrid1.Fields["SLACostPerTicket"] != null)
                    {
                        RadPivotGrid1.Fields["SLACostPerTicket"].IsHidden = false;
                    }
                    
                }
           // }
            IList<TrendingReportInput> columnlist = IncidentNavigatorHelper.INCubeSourceColumnList(objclient.Id).ToList();
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
                        if (RadPivotGrid1.Fields["Status"]!=null)
                        {
                            RadPivotGrid1.Fields["Status"].Caption = v.ClientColumnName;
                        }
                        break;

                 
                    case "Createddatetime":
                        if (RadPivotGrid1.Fields["Createdtime"] != null)
                        {
                            RadPivotGrid1.Fields["Createdtime"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Createdweekday"] != null)
                        {
                            RadPivotGrid1.Fields["Createdweekday"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Createdweeknum"] != null)
                        {
                            RadPivotGrid1.Fields["Createdweeknum"].IsHidden = false;
                        }
                        if(RadPivotGrid1.Fields["Createdmonth"] != null)
                        {
                            RadPivotGrid1.Fields["Createdmonth"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Createdyear"] != null)
                        {
                            RadPivotGrid1.Fields["Createdyear"].IsHidden = false;
                        }
                        break;

                    case "Acknowledgeddatetime":
                        if (RadPivotGrid1.Fields["Acknowledgedtime"] != null)
                        {
                            RadPivotGrid1.Fields["Acknowledgedtime"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Acknowledgedweekday"] != null)
                        {
                            RadPivotGrid1.Fields["Acknowledgedweekday"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Acknowledgedweeknum"] != null)
                        {
                            RadPivotGrid1.Fields["Acknowledgedweeknum"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Acknowledgedmonth"] != null)
                        {
                            RadPivotGrid1.Fields["Acknowledgedmonth"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Acknowledgedyear"] != null)
                        {
                            RadPivotGrid1.Fields["Acknowledgedyear"].IsHidden = false;
                        }

                        break;

                    case "Resolutiondatetime":
                        if (RadPivotGrid1.Fields["Resolutionefforthours"] != null)
                        {
                            RadPivotGrid1.Fields["Resolutionefforthours"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Resolutionweekday"] != null)
                        {
                            RadPivotGrid1.Fields["Resolutionweekday"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Resolutionweeknum"] != null)
                        {
                            RadPivotGrid1.Fields["Resolutionweeknum"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Resolutionyear"] != null)
                        {
                            RadPivotGrid1.Fields["Resolutionyear"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Resolutionmonth"] != null)
                        {
                            RadPivotGrid1.Fields["Resolutionmonth"].IsHidden = false;
                        }

                        break;
                    case "Servicerestoreddatetime":
                        if (RadPivotGrid1.Fields["Servicerestoredtime"] != null)
                        {
                            RadPivotGrid1.Fields["Servicerestoredtime"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Servicerestoredweekday"] != null)
                        {
                            RadPivotGrid1.Fields["Servicerestoredweekday"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Servicerestoredweeknum"] != null)
                        {
                            RadPivotGrid1.Fields["Servicerestoredweeknum"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Servicerestoredmonth"] != null)
                        {
                            RadPivotGrid1.Fields["Servicerestoredmonth"].IsHidden = false;
                        }
                        if (RadPivotGrid1.Fields["Servicerestoredyear"] != null)
                        {
                            RadPivotGrid1.Fields["Servicerestoredyear"].IsHidden = false;
                        }

                        break;
                    default:
                        if (RadPivotGrid1.Fields[v.ToolColumnName] != null && v.ToolColumnName!="Priority" && v.ToolColumnName!="Status" && v.ToolColumnName != "Resolutiontime" && v.ToolColumnName != "SLACostPerTicket")
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
        protected void Page_Load(object sender, EventArgs e)
        {
            var cid = (Client)Session["Client"];
            int uid = (int)Session["UserId"];
            if (!this.IsPostBack)
            {
                var items = Helper.GetViewsList(uid, cid.Id, (int)ReportFor.IncidentNavigator);
                doprdown1.DataSource = items;
                doprdown1.DataBind();
                if (Session["SysDefault"] == null)
                    Session["SysDefault"] = false;
                if (!(bool)Session["SysDefault"])
                {
                    var dViewid = Helper.Find_IsDefault_View(cid.Id, uid, (int)ReportFor.IncidentNavigator);
                    if (dViewid.ViewName != null)
                    {
                        doprdown1.SelectedValue = dViewid.Id.ToString();
                        defaultChekbox.Checked = true;
                        ViewNameText.Text = dViewid.ViewName;
                        Helper.setRadPivot(dViewid.Id, ref RadPivotGrid1);
                        Helper.settingFiltersToPivot(dViewid.Id, ref RadPivotGrid1);
                        Session["SysDefault"] = false;
                    }
                }
            }
            else
            {
                if (doprdown1.SelectedValue != null && doprdown1.SelectedValue != "")
                {
                    var Vid_Buffer = doprdown1.SelectedValue;
                    var Vid = Convert.ToInt32(Vid_Buffer);
                    if (Vid > 0)
                    {
                        Helper.setRadPivot(Vid, ref RadPivotGrid1);
                        Helper.settingFiltersToPivot(Vid, ref RadPivotGrid1);
                    }
                }
            }
            //RadPivotGrid1.Rebind();
        }

        protected void showDefaultView(object sender, EventArgs e)
        {
            Session["SysDefault"] = true;
            defaultChekbox.Checked = false;
            Response.Redirect("WebForm2.aspx");
        }

        protected void SaveViewDetails(object sender, EventArgs e)
        {
            if (ViewNameText.Text.Trim() != "")
            {
                var cid = (Client)Session["Client"];
                int uid = (int)Session["UserId"];
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var VieId = Helper.SaveReportViewDetails(cid.Id, uid, defaultChekbox.Checked, ViewNameText.Text, RadPivotGrid1.Fields, (int)ReportFor.IncidentNavigator);
                ViewNameText.Text = string.Empty;
                var items = Helper.GetViewsList(uid,cid.Id, (int)ReportFor.IncidentNavigator);
                //defaultChekbox.Checked = false;
                doprdown1.DataSource = items;
                doprdown1.DataBind();
                base.OnPreRender(e);
                Helper.saveFilterDetails(VieId, ref RadPivotGrid1);
            }
        }

        //setting to RagPivot
        protected void setToRadPivot(object sender, DropDownListEventArgs e)
        {
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

                Helper.setRadPivot(ViewIdd, ref RadPivotGrid1);
                Helper.settingFiltersToPivot(ViewIdd, ref RadPivotGrid1);
            }
            else
            {
                Session["SysDefault"] = true;
                defaultChekbox.Checked = false;
                Response.Redirect("WebForm2.aspx");
            }
        }


        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
            var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            DataTable source = IncidentNavigatorHelper.INTrendingReportList(objclient.Id);
            if (source != null)
            {

                (sender as RadPivotGrid).DataSource = source;
            }
            
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
                    lineSeries.Stacked = true;

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
            if(chkClassicViewPivot.Checked)
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