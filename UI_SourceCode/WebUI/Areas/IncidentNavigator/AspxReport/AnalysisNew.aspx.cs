using Data;
using Domain.Entities.Report;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using WebUI.App_Start;
using WebUI.Areas.ClientProfileManager;
using WebUI.Areas.ClientProfileManager.ViewModel;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IncidentNavigator.Dto;
using WebUI.Areas.IncidentNavigator.Models;
using WebUI.Dto;

namespace WebUI.Areas.IncidentNavigator.AspxReport
{
    public partial class AnalysisNew : AspReportpagesBaseClass
    {
        HashSet<string> columnNames = new HashSet<string>();
        Dictionary<string, LineSeries> LineSeriesByRowName = new Dictionary<string, LineSeries>();
        Dictionary<string, ColumnSeries> BarSeriesByRowName = new Dictionary<string, ColumnSeries>();
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
 

            columnNames = new HashSet<string>();
                LineSeriesByRowName = new Dictionary<string, LineSeries>();
                //var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
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

            }
            
            //RadPivotGrid1.RowTableLayout = PivotGridLayout.Tabular;
            foreach (PivotGridField fld in RadPivotGrid1.Fields)
            {
                fld.Caption = fld.Caption.Trim(); ;
                //RadPivotGrid1.Fields[fltr.FieldName].Caption = "^" + caption;
            }

            foreach (PivotGridFilter fltr in RadPivotGrid1.Filters)
            {
                RadPivotGrid1.Fields[fltr.FieldName].Caption = " " + RadPivotGrid1.Fields[fltr.FieldName].Caption;
            }
            
            RadPivotGrid1.Rebind();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CsrfHandler.Validate(this.Page, forgeryToken);
                RadPersistenceManager1.StorageProvider = new GridStateStorageProvider();
                var cid = (ClientDetail)Session["SessionClient"];
                int uid = Convert.ToInt32(Session["UserId"]);

                Email_Subject.Text = "";
                EmailList.Text = "";
                StartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                EndDate.Text = DateTime.Now.AddMonths(3).ToString("dd/MM/yyyy");
               
                if (!IsPostBack)
                {
                    RadDatePicker1.SelectedDate = DateTime.Now.AddMonths(-1);
                    RadDatePicker2.SelectedDate = DateTime.Today;
                    var items = Helper.GetViewsList(uid, cid.ClientId, Convert.ToInt32(ReportFor.IncidentNavigatorNew));
                    doprdown1.DataSource = items;
                    doprdown1.DataBind();
                    ////if (Session["SysDefault"] == null)
                    ////    Session["SysDefault"] = false;
                    ////if (Session["ViewId"] != null)
                    ////{
                    ////    var SessionViewId = Convert.ToInt32(Session["ViewId"]);
                    ////    if (SessionViewId > 0)
                    ////    {
                    ////        var chek = Helper.IsPublicForReport(SessionViewId);
                    ////        if (chek)
                    ////        {
                    ////            PublicChekbox.Checked = true;
                    ////        }
                    ////        else
                    ////        {
                    ////            PublicChekbox.Checked = false;
                    ////        }
                    ////        doprdown1.SelectedValue = SessionViewId.ToString();
                    ////        ViewNameText.Text = doprdown1.SelectedText;
                    ////        Helper.settingFiltersToPivot(SessionViewId, ref RadPivotGrid1);
                    ////        Helper.setRadPivot(SessionViewId, ref RadPivotGrid1);
                    ////        Session["ViewId"] = null;
                    ////    }
                    ////}

                    for (int i = 0; i < 24; i++)
                    {
                        selectHours.Items.Add(i.ToString());
                        selectHours_Weekly.Items.Add(i.ToString());
                        selectHours_Monthly.Items.Add(i.ToString());
                    }
                    for(int j = 0; j < 60; j++)
                    {
                        selectMinutes.Items.Add(j.ToString());
                        selectMinutes_Weekly.Items.Add(j.ToString());
                        selectMinutes_Monthly.Items.Add(j.ToString());
                    }
                    for (int i = 1; i <= 31; i++)
                    {
                        if (i >= 1 && i < 10)
                        {
                            selectMonth.Items.Add("0" + i.ToString());
                        }
                        else
                        {
                            selectMonth.Items.Add(i.ToString());
                        }
                    }
                }

                var month = DateTime.Now.ToString("dd");
                selectMonth.Items.FindByValue(month).Selected = true;
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
            ////defaultChekbox.Checked = false;
            //PublicChekbox.Checked = false;
            Response.Redirect("AnalysisNew.aspx");
        }

        protected void SaveViewDetails(object sender, EventArgs e)
        {
            if (ViewNameText.Text.Trim() != "")
            {
                var cid = (ClientDetail)Session["SessionClient"];
                int uid = Convert.ToInt32(Session["UserId"]);

                string key = cid.ClientId.ToString().Trim() + "|" + uid.ToString().Trim() + "|" + ViewNameText.Text.Trim() + "|" + Convert.ToString((int)ReportFor.IncidentNavigatorNew).Trim() + "|false|"+PublicChekbox.Checked+"|" + DateTypedrp.SelectedItem.Value;
                RadPersistenceManager1.StorageProviderKey = key;
                RadPersistenceManager1.SaveState();
                //Convert.ToDateTime()
                ViewNameText.Text = string.Empty;
                var items = Helper.GetViewsList(uid, cid.ClientId, (int)ReportFor.IncidentNavigatorNew);
                doprdown1.DataSource = items;
                doprdown1.DataBind();
                //base.OnPreRender(e);
                //Helper.saveFilterDetails(VieId, ref RadPivotGrid1);
            }
        }

        //setting to RagPivot
        protected void setToRadPivot(object sender, DropDownListEventArgs e)
        {
            
            ClientDetail cid = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            //var ViewIdd = 0;
            if (e.Value != null && e.Value != "" && e.Value != "-1" && e.Value != "0")
            {
                this.RadHtmlChart1.PlotArea.Series.Clear();
                ViewNameText.Text = e.Text;
                reportview.Value = ViewNameText.Text;

                string key = cid.ClientId.ToString().Trim() + "|" + uid.ToString().Trim() + "|" + ViewNameText.Text.Trim() + "|" + Convert.ToString((int)ReportFor.IncidentNavigatorNew).Trim() + "|false|" + "false";
                reportKey.Value = key;

                string fileId = key;// Session["CustomPersistenceSettingsKey"].ToString();
                RadPersistenceManager1.StorageProviderKey = fileId;
                RadPersistenceManager1.LoadState();
                RadPivotGrid1.Rebind();

            }
            else
            {
                Response.Redirect("AnalysisNew.aspx");
            }
            
        }

        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
           // this.RadHtmlChart1.PlotArea.Series.Clear();
            List<INTrendingReportEntity> replst = new List<INTrendingReportEntity>();
            var objclient = (ClientDetail)Session["SessionClient"];
            //take from and to date and pass it over here
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            fromDate = RadDatePicker1.SelectedDate.Value;
            toDate = RadDatePicker2.SelectedDate.Value;
            var dateType = DateTypedrp.SelectedItem.Value;
            replst = IncidentNavigatorHelper.INTrendingReportListRange(objclient.ClientId, fromDate, toDate,dateType);
            

            if (replst != null)
            {
                (sender as RadPivotGrid).UseQueryableDataProvider = true;

                (sender as RadPivotGrid).DataSource = replst;//.AsEnumerable();
            }


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

        protected void ButtonExcelDrillDown_Click(object sender, EventArgs e)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            string fileName =  "Analysis_" + client.ClientName.ToString() + ".xlsx";
            Response.Clear();

            Stream iStream = null;
            const int bufferSize = 64 * 1024;
            byte[] buffer = new Byte[bufferSize];
            int length;
            long dataToRead;

            try
            {
                iStream = ButtonExcelDrillDown(ClientId);
                dataToRead = iStream.Length;
                Response.ContentType =  "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                while (dataToRead > 0)
                {
                    if (Response.IsClientConnected)
                    {
                        length = iStream.Read(buffer, 0, bufferSize);
                        Response.OutputStream.Write(buffer, 0, length);
                        Response.Flush();
                        buffer = new byte[bufferSize];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (iStream != null)
                {
                    iStream.Close();
                }
                Response.Close();
            }
        }
            
        protected System.IO.Stream ButtonExcelDrillDown(int ClientId)
        
        {
            var dateType = DateTypedrp.SelectedItem.Value;
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            fromDate = RadDatePicker1.SelectedDate.Value;
            toDate = RadDatePicker2.SelectedDate.Value;
            var dt = GetTicketData(ClientId, dateType,fromDate, toDate);//, "AnalysisNew");


            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(dt);
            stream.Position = 0;
            return stream;
        }
        
        private System.Data.DataTable GetTicketData(int ClientId, string Datetype, DateTime? StartDate, DateTime? EndDate)
        {
            DataTable returntable = new DataTable();
            try
            {
                var paramName = new string[] { "ClientId", "Datecolumn",  "StartDate", "EndDate" };
                var paramValue = new object[] { ClientId, Datetype,   StartDate, EndDate };
                DataTable dt = new DataTable();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    dt = unitOfWork.GetDataFromStoredProcedure("AnalysisNew_DrilldownExport", paramName, paramValue);
                }
                //To change table header
                DataTable dtmap = new DataTable();
                var paramName1 = new string[] { "ClientId" };
                var paramValue1 = new object[] { ClientId };
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    dtmap = unitOfWork.GetDataFromStoredProcedure("INSourceMappingColumnForChart", paramName1, paramValue1);
                }


                PivotGridFiltersCollection filters = RadPivotGrid1.Filters;
                foreach (PivotGridLabelGroupFilter filter in filters.OfType<PivotGridLabelGroupFilter>())
                {
                    string colName = filter.FieldName;
                    PivotGridSetCondition condition = filter.Condition as PivotGridSetCondition;
                    IEnumerable<string> item = condition.Items.Select(x => x.ToString());
                    string Comparison = condition.Comparison.ToString();
                    dt.AcceptChanges();
                    foreach (DataRow row in dt.Rows)
                    {
                        string colValue = row[colName].ToString();
                        if((Comparison=="Includes")&& (!(item.Any(s => s == colValue))))
                        {
                             row.Delete();
                        }
                        else if ((Comparison == "DoesNotInclude")&& (item.Any(s => s == colValue)))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }

                for (int c=0;  c< dt.Columns.Count; c++)
                {
                    string st = dt.Columns[c].ColumnName;
                   
                    foreach (DataRow row in dtmap.Rows)
                    {
                        var coldet = new TeamRuleModel();
                        coldet.INFieldName = row["ToolColumnName"].ToString();
                        
                        if (!String.IsNullOrWhiteSpace(coldet.INFieldName))
                        {
                            if (coldet.INFieldName == st)
                            {
                                dt.Columns[c].ColumnName = row["ClientColumnName"].ToString();
                                break;
                            }
                        }
                    }
                    
                }


                return dt;
            }
            catch (Exception ex)
            {
                //  Log.Error("Integration_Report Controller  ExportIntegrationReport , Client ID is " + ClientId, ex);
            }

            return returntable;
        }

        private System.Data.DataTable GetTicketDatas(int ClientId, string TicketType, string Status, string Track, string Lever, string Category, string Token, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract, string Tower)
        {
            DataTable returntable = new DataTable();
            try
            {
                IList<IntegrationReportModel> Modellist = new List<IntegrationReportModel>();
                var paramName = new string[] { "ClientId", "TicketType", "Status", "Track", "Lever", "Category", "Token", "Priority", "Datetype", "StartDate", "EndDate", "contract" };
                var paramValue = new object[] { ClientId, TicketType, Status, Track, Lever, Category, Token, Priority, "Createddatetime", StartDate, EndDate, contract };
                //var paramValue = new object[] { ClientId, TicketType, Status, Track, Lever, Category, Token, Priority, Datetype, StartDate, EndDate, contract };
                DataTable dt = new DataTable();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    dt = unitOfWork.GetDataFromStoredProcedure("FetchIntegrationReportDataByTowerandLever_drilldown", paramName, paramValue);
                }
                return dt;
                //List<IntegrationReportModel> model = IncidentNavigatorHelper.INIntegration_ReportTowerandLever_drilldown(2518, "Incident", "", "SAP", "", "MAIL", "MAILBOX ISSUE", "", "Createddatetime", StartDate, EndDate, "138", "SAP").ToList();
                //--exec FetchIntegrationReportDataByTowerandLever_drilldown @ClientId = 2518, @TicketType = N'Incident',@Status = N'',@Track = N'SAP',@Lever = N'',@Category = N'MAIL',@Token = N'',@Priority = N'',@Datetype = N'Createddatetime',@StartDate = '',@EndDate = '',@contract = N'138'
            }
            catch (Exception ex)
            {
              //  Log.Error("Integration_Report Controller  ExportIntegrationReport , Client ID is " + ClientId, ex);
            }

            return returntable;
        }

        private System.Data.DataTable GetTicketDataaaa(int ClientId,  DateTime fromDate, DateTime toDate, string dateType)
        {
            var contextFactory = new DbContextFactory();
            DataTable returntable = new DataTable();
            using (var unitOfWork = new UnitOfWorks(contextFactory.GetContext()))
            {
                var paramName = new string[] { "ClientId", "Datecolumn", "StartDate", "EndDate" };
                var paramValue = new object[] { ClientId, dateType, fromDate, toDate };
                returntable = unitOfWork.GetDataFromStoredProcedure("AnalysisNew_DrilldownExport", paramName, paramValue);

            }
            return returntable;
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
                if (!columnNames.Contains(columnName))
                {
                    if (columnNames.Add(columnName))
                    {
                        AxisItem axisItem = new AxisItem(columnName);
                        this.RadHtmlChart1.PlotArea.XAxis.Items.Add(axisItem);
                        // this.RadHtmlChart1.PlotArea.YAxis.Items.Add(axisItem);
                    }
                }
                var chartType = ChartDrptype.SelectedItem.Value;
                if (chartType == "Both")
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
                        lineSeries.Name = rowName;
                        barSeries.Name = rowName;
                        //   lineSeries.Stacked = true;
                        // columnSeries.LabelsAppearance.DataFormatString = "C";
                        this.RadHtmlChart1.PlotArea.Series.Add(lineSeries);
                        this.RadHtmlChart1.PlotArea.Series.Add(barSeries);
                    }
                    barSeries.LabelsAppearance.Visible = true;
                    barSeries.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.InsideEnd;
                  //  barSeries.. = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.InsideEnd;
                    lineSeries.LabelsAppearance.Visible = true;
                    lineSeries.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                    CategorySeriesItem item = new CategorySeriesItem();
                    int value = 0;
                    if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out value))
                    {

                        item.Y = value;

                    }
                    else
                    {
                        item.Y = 0; // null;
                    }
                    if (columnNames.Count > lineSeries.SeriesItems.Count)
                    {
                        lineSeries.SeriesItems.Add(item);
                        barSeries.SeriesItems.Add(item);
                    }

                }
                else if (chartType == "Bar")
                {
                    ColumnSeries barSeries = null;

                    if (BarSeriesByRowName.ContainsKey(rowName)) // || BarSeriesByRowName.ContainsKey(rowName))
                    {
                        barSeries = BarSeriesByRowName[rowName];
                    }
                    else
                    {

                        barSeries = new ColumnSeries();
                        BarSeriesByRowName.Add(rowName, barSeries);
                        barSeries.Name = rowName;
                        this.RadHtmlChart1.PlotArea.Series.Add(barSeries);
                    }
                    barSeries.LabelsAppearance.Visible = true;
                    barSeries.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.InsideEnd;
                    CategorySeriesItem baritem = new CategorySeriesItem();
                    int barvalue = 0;
                    if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out barvalue))
                    {

                        baritem.Y = barvalue;

                    }
                    else
                    {
                        baritem.Y = 0; // null;
                    }
                    if (columnNames.Count > barSeries.SeriesItems.Count)
                    {
                        barSeries.SeriesItems.Add(baritem);
                    }
                }
                else 
                {
                    LineSeries lineSeries = null;
                    //BarSeries barSeries = null;

                    if (LineSeriesByRowName.ContainsKey(rowName)) // || BarSeriesByRowName.ContainsKey(rowName))
                    {
                        lineSeries = LineSeriesByRowName[rowName];
                        // barSeries = BarSeriesByRowName[rowName];
                    }
                    else
                    {
                        lineSeries = new LineSeries();
                        //barSeries = new BarSeries();
                        LineSeriesByRowName.Add(rowName, lineSeries);
                        // BarSeriesByRowName.Add(rowName, barSeries);
                        lineSeries.Name = rowName;
                        //barSeries.Name = rowName;
                        //   lineSeries.Stacked = true;
                        // columnSeries.LabelsAppearance.DataFormatString = "C";
                        this.RadHtmlChart1.PlotArea.Series.Add(lineSeries);
                        // this.RadHtmlChart1.PlotArea.Series.Add(barSeries);
                    }
                    ////barSeries.LabelsAppearance.Visible = true;
                    ////barSeries.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.InsideEnd;
                    lineSeries.LabelsAppearance.Visible = true;
                    lineSeries.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                    CategorySeriesItem item = new CategorySeriesItem();
                    int value = 0;
                    if (e.Cell.DataItem != null && int.TryParse(e.Cell.DataItem.ToString(), out value))
                    {

                        item.Y = value;

                    }
                    else
                    {
                        item.Y = 0; // null;
                    }
                    if (columnNames.Count > lineSeries.SeriesItems.Count)
                    {
                        lineSeries.SeriesItems.Add(item);
                        // barSeries.SeriesItems.Add(item);
                    }
                }

            }
        }

        protected void RadButtonApply_Click1(object sender, EventArgs e)
        {
            // this.RadHtmlChart1.PlotArea.Series.Clear();
            List<INTrendingReportEntity> replst = new List<INTrendingReportEntity>();
            int uid = (int)Session["UserId"];
            var objclient = (ClientDetail)Session["SessionClient"];
            //take from and to date and pass it over here
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            fromDate = RadDatePicker1.SelectedDate.Value;
            toDate = RadDatePicker2.SelectedDate.Value;
            var dateType = DateTypedrp.SelectedItem.Value;
            replst = IncidentNavigatorHelper.INTrendingReportListRange(objclient.ClientId, fromDate, toDate, dateType);
            //var items = Helper.GetViewsList(uid, objclient.ClientId, Convert.ToInt32(ReportFor.IncidentNavigatorNew), fromDate.Date.ToString(), toDate.Date.ToString(), dateType);
            var items = Helper.GetViewsList(uid, objclient.ClientId, (int)ReportFor.IncidentNavigatorNew);
            doprdown1.DataSource = items;
            doprdown1.DataBind();

            if (replst != null)
            {
                // (sender as RadPivotGrid).UseQueryableDataProvider = true;
                RadPivotGrid1.UseQueryableDataProvider = true;
                RadPivotGrid1.DataSource = replst;//.AsEnumerable();
                RadPivotGrid1.Rebind();
            }
           
        }

        protected void ChartDrp_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            // RadHtmlChart1.
           // this.RadHtmlChart1.PlotArea.Series.Clear();
            RadPivotGrid1.Rebind();
        }
        protected void saveSubscrClick(object sender, EventArgs e)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            RTPSubcriptionModel RPT = new RTPSubcriptionModel();

            RPT.EmailSubject = Email_Subject.Text;
            RPT.Recipients = EmailList.Text;
            RPT.ReportFor= Convert.ToString(ReportFortxt.SelectedItem.Value);
            RPT.StartDate = StartDate.Text;
            RPT.EndDate = EndDate.Text;
            RPT.ReportParameters ="{ClientName:"+client.ClientName+", Clientid:"+client.ClientId+", StartDate:"+RPT.StartDate+", EndDate:"+RPT.EndDate+", DateType:"+DateTypedrp.SelectedValue+", Key:"+reportKey.Value+"}";

            var ScheduleType = selperiod.Value;
            RPT.ScheduleType = ScheduleType;

            switch (ScheduleType)
            {
                case "Daily":
                    RPT.selectHours= Convert.ToString(selectHours.SelectedItem.Value);
                    RPT.selectMinutes = Convert.ToString(selectMinutes.SelectedItem.Value);
                    RPT.Timezone = Convert.ToString(selectTimeZone.SelectedItem.Value);
                    break;
                case "Weekly":
                    RPT.selectHours = Convert.ToString(selectHours_Weekly.SelectedItem.Value);
                    RPT.selectMinutes = Convert.ToString(selectMinutes_Weekly.SelectedItem.Value);
                    RPT.Timezone = Convert.ToString(selectTimeZone1.SelectedItem.Value);
                    RPT.WeekDay =Convert.ToInt32(selWeek.Value);
                    break;
                case "Monthly":
                    RPT.selectHours = Convert.ToString(selectHours_Monthly.SelectedItem.Value);
                    RPT.selectMinutes = Convert.ToString(selectMinutes_Monthly.SelectedItem.Value);
                    RPT.Timezone = Convert.ToString(selectTimeZone2.SelectedItem.Value);
                    RPT.MonthDay = Convert.ToInt32(selectMonth.SelectedItem.Value);
                    break;
            }
           
            RPT.ReportName = "Analysis";
            RPT.TelerikReportName = null;
            
            int usid = (int)Session["UserId"];
            RPT.CreatedBy = usid;
            RPT.CreatedDate = DateTime.Now;
            RPT.Status = "Draft";
            RPT.ClientID = client.ClientId;
            RPT.AppID = (int)App_Start.Helper.AMOApplicationName.IncidentNavigator;
            
            if (RPT.Timezone == "PST")
            {
                RPT.selectHours = (Convert.ToInt32(RPT.selectHours) + 3).ToString();
                if (Convert.ToInt32(RPT.selectHours) >= 24)
                {
                    RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 24).ToString();
                }
            }

            if (RPT.Timezone == "CST")
            {
                if (RPT.selectHours == "23")
                {
                    RPT.selectHours = "0";
                }
                else
                {
                    RPT.selectHours = (Convert.ToInt32(RPT.selectHours) + 1).ToString();
                }
            }

            RPT.ScheduleTime = (RPT.selectHours + ":" + RPT.selectMinutes + ":" + "00").ToString(); //if pm selecthours + 12

            var res = ClientProfileManagerHelper.CreateReportSubscription(RPT);
        }
    }
}