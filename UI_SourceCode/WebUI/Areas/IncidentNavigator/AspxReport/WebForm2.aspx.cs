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
 

            columnNames = new HashSet<string>();
                LineSeriesByRowName = new Dictionary<string, LineSeries>();
               // var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                var objclient = (ClientDetail)Session["SessionClient"];
                Db db = new Db();
                var target = (from insvchrs in db.INIncidentServicehours
                              where insvchrs.ClientId == objclient.ClientId
                              select insvchrs).ToList();
                if (target.Count > 0)
                {
                    if (Convert.ToInt32(target.FirstOrDefault().HasSLAManager) == 1)
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
                            if (RadPivotGrid1.Fields["Createdmonth"] != null)
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
                            if (RadPivotGrid1.Fields[v.ToolColumnName] != null && v.ToolColumnName != "Priority" && v.ToolColumnName != "Status" && v.ToolColumnName != "Resolutiontime" && v.ToolColumnName != "SLACostPerTicket")
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
                //To clear repeating chart series
                //////RadHtmlChart1.PlotArea.XAxis.Items.Clear();
                //////RadHtmlChart1.PlotArea.Series.Clear();
                RadPivotGrid1.Rebind();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           // var cid = (Client)Session["Client"];
            var cid = (ClientDetail)Session["SessionClient"];
            int uid = Convert.ToInt32(Session["UserId"]);
            //RadDatePicker1.SelectedDate = DateTime.Now.AddDays(-6);
            //    RadDatePicker2.SelectedDate = DateTime.Now;
            if (!this.IsPostBack)
            {
                var items = Helper.GetViewsList(uid, cid.ClientId, Convert.ToInt32(ReportFor.IncidentNavigator));
                doprdown1.DataSource = items;
                doprdown1.DataBind();
                if (Session["SysDefault"] == null)
                    Session["SysDefault"] = false;
               
                if (Session["ViewId"] != null)
                {
                    var SessionViewId = Convert.ToInt32(Session["ViewId"]);
                    if (SessionViewId > 0)
                    {
                        var chek = Helper.IsPublicForReport(SessionViewId);
                        if (chek)
                        {                       
                            PublicChekbox.Checked = true;
                        }
                        else
                        {
                            PublicChekbox.Checked = false;
                        }
                        var dViewid = Helper.Find_IsDefault_View(cid.ClientId, uid, Convert.ToInt32(ReportFor.IncidentNavigator));
                        if (dViewid.ViewName != null)
                        {
                            doprdown1.SelectedValue = dViewid.Id.ToString();
                            defaultChekbox.Checked = true;
                            PublicChekbox.Checked = dViewid.IsPublic;
                            ViewNameText.Text = dViewid.ViewName;
                        }
                        doprdown1.SelectedValue = SessionViewId.ToString();
                        ViewNameText.Text = doprdown1.SelectedText;
                        Helper.settingFiltersToPivot(SessionViewId, ref RadPivotGrid1);
                        Helper.setRadPivot(SessionViewId, ref RadPivotGrid1);
                        Session["ViewId"] = null;
                    }
                }

                else if (!(bool)Session["SysDefault"])
                {
                    var dViewid = Helper.Find_IsDefault_View(cid.ClientId, uid, Convert.ToInt32(ReportFor.IncidentNavigator));
                    if (dViewid.ViewName != null)
                    {
                        doprdown1.SelectedValue = dViewid.Id.ToString();
                        defaultChekbox.Checked = true;
                        PublicChekbox.Checked = dViewid.IsPublic;
                        ViewNameText.Text = dViewid.ViewName;
                        //removing filters
                        //RadPivotGrid1.Rebind();
                        RadPivotGrid1.ClearAllFilters();
                        Helper.settingFiltersToPivot(dViewid.Id, ref RadPivotGrid1);
                        Helper.setRadPivot(dViewid.Id, ref RadPivotGrid1);
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
                        //removing filters
                        //RadPivotGrid1.Rebind();
                        RadPivotGrid1.ClearAllFilters();
                        Helper.settingFiltersToPivot(Vid, ref RadPivotGrid1);
                        Helper.setRadPivot(Vid, ref RadPivotGrid1);
                    }
                }
            }
            //RadPivotGrid1.Rebind();
        }

        protected void showDefaultView(object sender, EventArgs e)
        {
            Session["SysDefault"] = true;
            defaultChekbox.Checked = false;
            PublicChekbox.Checked = false;
            Response.Redirect("WebForm2.aspx");
        }

        protected void SaveViewDetails(object sender, EventArgs e)
        {
            if (ViewNameText.Text.Trim() != "")
            {
               // var cid = (Client)Session["Client"];
                var cid = (ClientDetail)Session["SessionClient"];
                int uid = Convert.ToInt32(Session["UserId"]);
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var VieId = Helper.SaveReportViewDetails(cid.ClientId, uid, defaultChekbox.Checked, ViewNameText.Text, RadPivotGrid1.Fields, Convert.ToInt32(ReportFor.IncidentNavigator),PublicChekbox.Checked);
                ViewNameText.Text = string.Empty;
                var items = Helper.GetViewsList(uid,cid.ClientId, Convert.ToInt32(ReportFor.IncidentNavigator));
                //defaultChekbox.Checked = false;
                doprdown1.DataSource = items;
                doprdown1.DataBind();
                //base.OnPreRender(e);
                Helper.saveFilterDetails(VieId, ref RadPivotGrid1);
            }
        }

        //setting to RagPivot
        protected void setToRadPivot(object sender, DropDownListEventArgs e)
        {
            var ViewIdd = 0;
            if (e.Value != null && e.Value != "" && e.Value != "-1" && e.Value != "0")
            {
              //  ViewNameText.Text = e.Text;
                ViewIdd = Convert.ToInt32(e.Value);
                ////var chek = Helper.IsCheckedForReport(ViewIdd);
                ////if (chek)
                ////{
                ////    defaultChekbox.Checked = true;
                ////}
                ////else
                ////    defaultChekbox.Checked = false;

                ////var publicChek = Helper.IsPublicForReport(ViewIdd);
                ////if (publicChek)
                ////{
                ////    PublicChekbox.Checked = true;
                ////}
                ////else
                ////{
                ////    PublicChekbox.Checked = false;
                ////}

                Session["ViewId"] = ViewIdd;
                ////Response.Redirect("WebForm2.aspx");
               

                ////removing filters
                //RadPivotGrid1.Rebind();
                //RadPivotGrid1.ClearAllFilters();
                //Helper.settingFiltersToPivot(ViewIdd, ref RadPivotGrid1);
                //Helper.setRadPivot(ViewIdd, ref RadPivotGrid1);
            }
            else
            {
                Session["SysDefault"] = true;
                ////defaultChekbox.Checked = false;

            }
            Response.Redirect("WebForm2.aspx");
        }


        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
            List<INTrendingReportEntity> replst = new List<INTrendingReportEntity>();
           // var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            replst = IncidentNavigatorHelper.INTrendingReportList(objclient.ClientId);
            

            if (replst != null)
            {
                (sender as RadPivotGrid).UseQueryableDataProvider = true;

                (sender as RadPivotGrid).DataSource = replst;//.AsEnumerable();
            }
            //To clear repeating chart series
            //////RadHtmlChart1.PlotArea.XAxis.Items.Clear();
            //////RadHtmlChart1.PlotArea.Series.Clear();

        }

        protected void RadPivotGrid1_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {


            //////PivotGridDataCell dataCell = e.Cell as PivotGridDataCell;
            //////if (columnNames.Count == 0)
            //////{
            //////    this.RadHtmlChart1.PlotArea.XAxis.Items.Clear();

            //////    this.RadHtmlChart1.PlotArea.Series.Clear();

            //////}

            //////if (dataCell != null && dataCell.CellType == PivotGridDataCellType.DataCell)
            //////{
            //////    string rowName = GetName(dataCell.ParentRowIndexes);

            //////    string columnName = GetName(dataCell.ParentColumnIndexes);
            //////    columnName = columnName.Replace('\'', ' ');
            //////    if (columnNames.Add(columnName))
            //////    {
            //////        AxisItem axisItem = new AxisItem(columnName);
            //////        this.RadHtmlChart1.PlotArea.XAxis.Items.Add(axisItem);
            //////    }
            //////    LineSeries lineSeries = null;

            //////    if (LineSeriesByRowName.ContainsKey(rowName))
            //////    {
            //////        lineSeries = LineSeriesByRowName[rowName];
            //////    }
            //////    else
            //////    {
            //////        lineSeries = new LineSeries();

            //////        LineSeriesByRowName.Add(rowName, lineSeries);
            //////        lineSeries.Name = rowName;
            //////     //   lineSeries.Stacked = true;

            //////        // columnSeries.LabelsAppearance.DataFormatString = "C";
            //////        this.RadHtmlChart1.PlotArea.Series.Add(lineSeries);
            //////    }
            //////    lineSeries.LabelsAppearance.Visible = false;
            //////    CategorySeriesItem item = new CategorySeriesItem();
            //////    int value = 0;
            //////    if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out value))
            //////    {
            //////        item.Y = value;
            //////    }
            //////    else
            //////    {
            //////        item.Y = null;
            //////    }
            //////    lineSeries.SeriesItems.Add(item);
            //////}
        }
        protected void PivotGridPageChange(object sender, PivotGridPageChangedEventArgs e)
        {
            //var SessionViewId = Convert.ToInt32(Session["ViewId"]);
            //Helper.settingFiltersToPivot(SessionViewId, ref RadPivotGrid1);
            //Helper.setRadPivot(SessionViewId, ref RadPivotGrid1);
        }
        protected void PivotGridPageSizeChange(object sender, PivotGridPageSizeChangedEventArgs e)
        {
            //var SessionViewId = Convert.ToInt32(Session["ViewId"]);
            //Helper.settingFiltersToPivot(SessionViewId, ref RadPivotGrid1);
            //Helper.setRadPivot(SessionViewId, ref RadPivotGrid1);
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

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {

        }

        //protected void Filter(object sender, EventArgs e)
        //{
        //    List<INTrendingReportEntity> replst = new List<INTrendingReportEntity>();
        //    DateTime start, end;

        //    start = Convert.ToDateTime(RadDatePicker1.SelectedDate);
        //    end = Convert.ToDateTime(RadDatePicker2.SelectedDate);
        //    var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
        //    DataTable source = IncidentNavigatorHelper.INTrendingReportList(objclient.Id,start,end);

        //    foreach (DataRow dr in source.Rows)
        //    {
        //        INTrendingReportEntity repoelement = new INTrendingReportEntity();
        //        repoelement.ID = Convert.ToInt32(dr["ID"]);
        //        repoelement.Clientid = Convert.ToInt32(dr["Clientid"]);
        //        repoelement.TicketId = Convert.ToString(dr["TicketId"]);
        //        repoelement.Incidentid = Convert.ToString(dr["Incidentid"]);
        //        repoelement.Createddatetime = dr["Createddatetime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["Createddatetime"]);
        //        repoelement.Status = Convert.ToString(dr["Status"]);
        //        repoelement.Title = Convert.ToString(dr["Title"]);
        //        repoelement.Priority = Convert.ToString(dr["Priority"]);
        //        repoelement.Urgency = Convert.ToString(dr["Urgency"]);
        //        repoelement.Type = Convert.ToString(dr["Type"]);
        //        repoelement.Outagetype = Convert.ToString(dr["Outagetype"]);
        //        repoelement.Divisionimpacted = Convert.ToString(dr["Divisionimpacted"]);
        //        repoelement.Businessunitimpacted = Convert.ToString(dr["Businessunitimpacted"]);
        //        repoelement.Servicecategory = Convert.ToString(dr["Servicecategory"]);
        //        repoelement.Servicesubcategory = Convert.ToString(dr["Servicesubcategory"]);
        //        repoelement.Servicearea = Convert.ToString(dr["Servicearea"]);
        //        repoelement.Regionimpacted = Convert.ToString(dr["Regionimpacted"]);
        //        repoelement.Locationsimpacted = Convert.ToString(dr["Locationsimpacted"]);
        //        repoelement.Customerdept = Convert.ToString(dr["Customerdept"]);
        //        repoelement.Customerlastname = Convert.ToString(dr["Customerlastname"]);
        //        repoelement.Customerfirstname = Convert.ToString(dr["Customerfirstname"]);
        //        repoelement.Customertitle = Convert.ToString(dr["Customertitle"]);
        //        repoelement.Customersatisfaction = Convert.ToString(dr["Customersatisfaction"]);
        //        repoelement.Impacttype = Convert.ToString(dr["Impacttype"]);
        //        repoelement.Configitemid = Convert.ToString(dr["Configitemid"]);
        //        repoelement.Configtype = Convert.ToString(dr["Configtype"]);
        //        repoelement.Configitemname = Convert.ToString(dr["Configitemname"]);
        //        repoelement.Environmentimpacted = Convert.ToString(dr["Environmentimpacted"]);

        //        repoelement.Nodename = Convert.ToString(dr["Nodename"]);
        //        repoelement.Rootcausetype = Convert.ToString(dr["Rootcausetype"]);
        //        repoelement.Rootcausedescription = Convert.ToString(dr["Rootcausedescription"]);
        //        repoelement.Resolutiontype = Convert.ToString(dr["Resolutiontype"]);
        //        repoelement.Solutionid = Convert.ToString(dr["Solutionid"]);
        //        repoelement.Deploymentrelease = Convert.ToString(dr["Deploymentrelease"]);
        //        repoelement.Relatedchangeids = Convert.ToString(dr["Relatedchangeids"]);
        //        repoelement.Relatedids = Convert.ToString(dr["Relatedids"]);
        //        repoelement.Relatedproblemids = Convert.ToString(dr["Relatedproblemids"]);
        //        repoelement.Helpdesk = Convert.ToString(dr["Helpdesk"]);
        //        repoelement.Assignedtoperson = Convert.ToString(dr["Assignedtoperson"]);
        //        repoelement.Assignedtosupportgroup = Convert.ToString(dr["Assignedtosupportgroup"]);
        //        repoelement.Createdbygroup = Convert.ToString(dr["Createdbygroup"]);
        //        repoelement.Createdbyperson = Convert.ToString(dr["Createdbyperson"]);
        //        repoelement.Createdtimezone = Convert.ToString(dr["Createdtimezone"]);
        //        repoelement.Createdtime = Convert.ToString(dr["Createdtime"]);
        //        repoelement.Createdweekday = Convert.ToString(dr["Createdweekday"]);
        //        repoelement.Createdweeknum = Convert.ToString(dr["Createdweeknum"]);
        //        repoelement.Createdmonth = Convert.ToString(dr["Createdmonth"]);
        //        repoelement.Createdyear = Convert.ToString(dr["Createdyear"]);
        //        repoelement.Acknowlededbygroup = Convert.ToString(dr["Acknowlededbygroup"]);
        //        repoelement.Acknowlededbyperson = Convert.ToString(dr["Acknowlededbyperson"]);
        //        repoelement.Acknowledgeddatetime = dr["Acknowledgeddatetime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["Acknowledgeddatetime"]);
        //        repoelement.Acknowledgedtimezone = Convert.ToString(dr["Acknowledgedtimezone"]);
        //        repoelement.Acknowledgedtime = Convert.ToString(dr["Acknowledgedtime"]);
        //        repoelement.Acknowledgedweekday = Convert.ToString(dr["Acknowledgedweekday"]);
        //        repoelement.Acknowledgedweeknum = Convert.ToString(dr["Acknowledgedweeknum"]);
        //        repoelement.Acknowledgedmonth = Convert.ToString(dr["Acknowledgedmonth"]);
        //        repoelement.Acknowledgedyear = Convert.ToString(dr["Acknowledgedyear"]);
        //        repoelement.Resolvedbygroup = Convert.ToString(dr["Resolvedbygroup"]);
        //        repoelement.Resolvedbyperson = Convert.ToString(dr["Resolvedbyperson"]);
        //        repoelement.Resolvedbyvendor = Convert.ToString(dr["Resolvedbyvendor"]);
        //        repoelement.Resolutionefforthours = Convert.ToString(dr["Resolutionefforthours"]);
        //        repoelement.Resolutiondatetime = dr["Resolutiondatetime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["Resolutiondatetime"]);
        //        repoelement.Resolvedtimezone = Convert.ToString(dr["Resolvedtimezone"]);
        //        repoelement.Resolutiontime = Convert.ToInt32(dr["Resolutiontime"]);
        //        repoelement.Resolutionweekday = Convert.ToString(dr["Resolutionweekday"]);
        //        repoelement.Resolutionweeknum = Convert.ToString(dr["Resolutionweeknum"]);
        //        repoelement.Servicerestoredbygroup = Convert.ToString(dr["Servicerestoredbygroup"]);
        //        repoelement.Servicerestoredbyperson = Convert.ToString(dr["Servicerestoredbyperson"]);
        //        repoelement.Servicerestoreddatetime = dr["Servicerestoreddatetime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["Servicerestoreddatetime"]);
        //        repoelement.Servicerestoredtimezone = Convert.ToString(dr["Servicerestoredtimezone"]);
        //        repoelement.Servicerestoredtime = Convert.ToString(dr["Servicerestoredtime"]);
        //        repoelement.Servicerestoredweekday = Convert.ToString(dr["Servicerestoredweekday"]);
        //        repoelement.Servicerestoredweeknum = Convert.ToString(dr["Servicerestoredweeknum"]);
        //        repoelement.Servicerestoredmonth = Convert.ToString(dr["Servicerestoredmonth"]);
        //        repoelement.Servicerestoredyear = Convert.ToString(dr["Servicerestoredyear"]);
        //        repoelement.Lastupdatedbyperson = Convert.ToString(dr["Lastupdatedbyperson"]);
        //        repoelement.Lastupdateddatetime = dr["Lastupdateddatetime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["Lastupdateddatetime"]);
        //        repoelement.Datasource = Convert.ToString(dr["Datasource"]);
        //        repoelement.Datasourcenotes = Convert.ToString(dr["Datasourcenotes"]);
        //        repoelement.Assignedbygroup = Convert.ToString(dr["Assignedbygroup"]);
        //        repoelement.Assignedbyperson = Convert.ToString(dr["Assignedbyperson"]);
        //        repoelement.Assigneddatetime = dr["Assigneddatetime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["Assigneddatetime"]);
        //        repoelement.Esitmatedresponsetimebygroup = Convert.ToString(dr["Esitmatedresponsetimebygroup"]);
        //        repoelement.Esitmatedresponsedatetime = Convert.ToDateTime(dr["Esitmatedresponsedatetime"]);
        //        repoelement.Resolutionyear = Convert.ToString(dr["Resolutionyear"]);
        //        repoelement.Resolutionmonth = Convert.ToString(dr["Resolutionmonth"]);
        //        repoelement.Waivedindicator = Convert.ToString(dr["Waivedindicator"]);
        //        repoelement.Esitmatedresolutiondate = Convert.ToDateTime(dr["Esitmatedresolutiondate"]);
        //        repoelement.Totalsuspenddurationassignedtoack = Convert.ToInt32(dr["Totalsuspenddurationassignedtoack"]);
        //        repoelement.Totalsuspenddurationacktoer = Convert.ToInt32(dr["Totalsuspenddurationacktoer"]);
        //        repoelement.Totalsuspenddurationertoresolution = Convert.ToInt32(dr["Totalsuspenddurationertoresolution"]);
        //        repoelement.Resolvedindicator = Convert.ToString(dr["Resolvedindicator"]);
        //        repoelement.IsNTTGroup = Convert.ToString(dr["IsNTTGroup"]);
        //        repoelement.SupportGroupType = Convert.ToString(dr["SupportGroupType"]);
        //        repoelement.InProgressdatetime = dr["InProgressdatetime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["InProgressdatetime"]);
        //        repoelement.Closeddatetime = dr["Closeddatetime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["Closeddatetime"]);
        //        repoelement.ScheduledstartdateTime = dr["ScheduledstartdateTime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["ScheduledstartdateTime"]);
        //        repoelement.ScheduledenddateTime = dr["ScheduledenddateTime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["ScheduledenddateTime"]);
        //        repoelement.Scheduledchangecount = Convert.ToInt32(dr["Scheduledchangecount"]);
        //        repoelement.Company = Convert.ToString(dr["Company"]);
        //        repoelement.Site = Convert.ToString(dr["Site"]);
        //        repoelement.Assigneecompany = Convert.ToString(dr["Assigneecompany"]);
        //        repoelement.Assigneeorganization = Convert.ToString(dr["Assigneeorganization"]);
        //        repoelement.Estimatedworkeffort = Convert.ToInt32(dr["Estimatedworkeffort"]);
        //        repoelement.Actualworkeffort = Convert.ToDecimal(dr["Actualworkeffort"]);
        //        repoelement.ActualstartdateTime = dr["ActualstartdateTime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["ActualstartdateTime"]);
        //        repoelement.Recyclecount = Convert.ToInt32(dr["Recyclecount"]);
        //        repoelement.Statusreason = Convert.ToString(dr["Statusreason"]);
        //        repoelement.ConfigItemCategory = Convert.ToString(dr["ConfigItemCategory"]);
        //        repoelement.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
        //        repoelement.ResponseTimeMetSLA = Convert.ToString(dr["ResponseTimeMetSLA"]);
        //        repoelement.ResolutionTimeMetSLA = Convert.ToString(dr["ResolutionTimeMetSLA"]);
        //        repoelement.EstimateResponseTime = Convert.ToInt32(dr["EstimateResponseTime"]);
        //        repoelement.ClosureCategory = Convert.ToString(dr["ClosureCategory"]);
        //        repoelement.EstimatedResponseTimeMetSLA = Convert.ToString(dr["EstimatedResponseTimeMetSLA"]);
        //        repoelement.EstResptarget = Convert.ToString(dr["EstResptarget"]);
        //        repoelement.Reslntargettime = Convert.ToString(dr["Reslntargettime"]);
        //        repoelement.ResolutionExceptionIndicator = Convert.ToString(dr["ResolutionExceptionIndicator"]);
        //        repoelement.ResolutionExceptionReason = Convert.ToString(dr["ResolutionExceptionReason"]);
        //        repoelement.ResponseExceptionIndicator = Convert.ToString(dr["ResponseExceptionIndicator"]);
        //        repoelement.ResponseExceptionReason = Convert.ToString(dr["ResponseExceptionReason"]);
        //        repoelement.Resptargettime = Convert.ToString(dr["Resptargettime"]);
        //        repoelement.Severity = Convert.ToString(dr["Severity"]);
        //        repoelement.WorkGroupLevel = Convert.ToString(dr["WorkGroupLevel"]);
        //        repoelement.AffectedItemName = Convert.ToString(dr["AffectedItemName"]);
        //        repoelement.SLACostPerTicket = Convert.ToInt32(dr["SLACostPerTicket"]);
        //        repoelement.INResolutiontime = Convert.ToInt32(dr["INResolutiontime"]);
        //        repoelement.INCostPerTicket = Convert.ToInt32(dr["INCostPerTicket"]);
        //        repoelement.AccountName = Convert.ToString(dr["AccountName"]);
        //        repoelement.ContractName = Convert.ToString(dr["ContractName"]);
        //        replst.Add(repoelement);
        //    }

        //    if (source != null)
        //    {
        //        (sender as RadPivotGrid).UseQueryableDataProvider = true;

        //        (sender as RadPivotGrid).DataSource = replst;//.AsEnumerable();
        //    }
        //    //To clear repeating chart series
        //    RadHtmlChart1.PlotArea.XAxis.Items.Clear();
        //    RadHtmlChart1.PlotArea.Series.Clear();






        //}
    }
}