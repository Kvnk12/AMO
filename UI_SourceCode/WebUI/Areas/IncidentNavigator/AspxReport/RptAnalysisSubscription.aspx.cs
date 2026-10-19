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
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using WebUI.App_Start;
using WebUI.Areas.ClientProfileManager;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IncidentNavigator.Dto;
using WebUI.Areas.IncidentNavigator.Models;
using WebUI.Dto;

namespace WebUI.Areas.IncidentNavigator.AspxReport
{
    public partial class RptAnalysisSubscription : System.Web.UI.Page
    {
        HashSet<string> columnNames = new HashSet<string>();
        string ClientName = null;
        string ClientId = null;
        string StartDate = null;
        string EndDate =null;
        string DateType = null;
        string Key = null;
        DateTime fromDate = new DateTime();
        DateTime toDate = new DateTime();

        Dictionary<string, LineSeries> LineSeriesByRowName = new Dictionary<string, LineSeries>();
        Dictionary<string, ColumnSeries> BarSeriesByRowName = new Dictionary<string, ColumnSeries>();
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {


                columnNames = new HashSet<string>();
                LineSeriesByRowName = new Dictionary<string, LineSeries>();
                int Clientid = 1;

                Db db = new Db();
                var target = (from insvchrs in db.INIncidentServicehours
                              where insvchrs.ClientId == Clientid
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
                IList<TrendingReportInput> columnlist = IncidentNavigatorHelper.INCubeSourceColumnList(Clientid).ToList();
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
                RadPersistenceManager1.StorageProvider = new GridStateStorageProvider();
                List<INTrendingReportEntity> replst = new List<INTrendingReportEntity>();
                var rptUID = Request.QueryString["id"];
                if (!IsPostBack)
                {
                    Db db = new Db();
                    var rptJob = (from a in db.RPTJobDetail
                                    where a.ReportUID == rptUID
                                    select new {
                                        subid=a.RPTSubscriptionId,
                                        startDate=a.RPTStartDate,
                                        endDate=a.RPTEndDate}).FirstOrDefault();
                    var rptParam = (from a in db.RPTSubscription
                                    where a.Id == rptJob.subid
                                    select new { 
                                        param=a.ReportParameters,
                                        clientid=a.ClientID,
                                        rptname=a.ReportName,
                                        rptFor=a.ReportFor
                                    }).FirstOrDefault();
                    var parameter = rptParam.param.Trim(new Char[] { '{', '}',' '});
                    var paramList = parameter.Split(',');

                     ClientName=paramList[0].Substring(paramList[0].IndexOf(":")+1);
                     ClientId=paramList[1].Substring(paramList[1].IndexOf(":")+1);
                     StartDate=paramList[2].Substring(paramList[2].IndexOf(":")+1);
                     EndDate=paramList[3].Substring(paramList[3].IndexOf(":")+1);
                     DateType=paramList[4].Substring(paramList[4].IndexOf(":")+1);
                     Key = paramList[5].Substring(paramList[5].IndexOf(":")+1);

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

                    replst = IncidentNavigatorHelper.INTrendingReportListRange(int.Parse(ClientId), fromDate, toDate, DateType);
                    
                    if (replst != null)
                    {
                        RadPivotGrid1.UseQueryableDataProvider = true;
                        RadPivotGrid1.DataSource = replst;//.AsEnumerable();
                        RadPivotGrid1.Rebind();

                        string fileId = Key;
                        RadPersistenceManager1.StorageProviderKey = fileId;
                        RadPersistenceManager1.LoadState();
                        //setPivotGrid();
                        RadPivotGrid1.Rebind();
                        
                    }
                }

            }
            catch (Exception ex)
            {
               
            }

        }
       
        protected void showDefaultView(object sender, EventArgs e)
        {
            Response.Redirect("AnalysisNew.aspx");
        }

        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
            // this.RadHtmlChart1.PlotArea.Series.Clear();
            List<INTrendingReportEntity> replst = new List<INTrendingReportEntity>();

            var rptUID = Request.QueryString["id"];
            Db db = new Db();
            var rptJob = (from a in db.RPTJobDetail
                          where a.ReportUID == rptUID
                          select new
                          {
                              subid = a.RPTSubscriptionId,
                              startDate = a.RPTStartDate,
                              endDate = a.RPTEndDate
                          }).FirstOrDefault();
            fromDate = rptJob.startDate.Value;
            toDate = rptJob.endDate.Value;
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
            DateType = paramList[4].Substring(paramList[4].IndexOf(":") + 1);

            replst = IncidentNavigatorHelper.INTrendingReportListRange(int.Parse(ClientId), fromDate, toDate, DateType);

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
            var rptUID = Request.QueryString["id"];
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

            string fileName = "Analysis_" + ClientName.ToString() + ".xlsx";
            Response.Clear();

            Stream iStream = null;
            const int bufferSize = 64 * 1024;
            byte[] buffer = new Byte[bufferSize];
            int length;
            long dataToRead;

            try
            {
                iStream = ButtonExcelDrillDown();
                dataToRead = iStream.Length;
                Response.ContentType = "application/vnd.ms-excel";
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


        protected System.IO.Stream ButtonExcelDrillDown()
        {
            var rptUID = Request.QueryString["id"];
            Db db = new Db();
            var rptJob = (from a in db.RPTJobDetail
                          where a.ReportUID == rptUID
                          select new
                          {
                              subid = a.RPTSubscriptionId,
                              startDate = a.RPTStartDate,
                              endDate = a.RPTEndDate
                          }).FirstOrDefault();
            fromDate = rptJob.startDate.Value;
            toDate = rptJob.endDate.Value;
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
            DateType = paramList[4].Substring(paramList[4].IndexOf(":") + 1);
            var dt = GetTicketData(int.Parse(ClientId), DateType, fromDate, toDate);//, "AnalysisNew");

            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(dt);
            stream.Position = 0;
            return stream;
        }

        private System.Data.DataTable GetTicketData(int ClientId, string Datetype, DateTime? StartDate, DateTime? EndDate)
        {
            DataTable returntable = new DataTable();
            try
            {
                var paramName = new string[] { "ClientId", "Datecolumn", "StartDate", "EndDate" };
                var paramValue = new object[] { ClientId, Datetype, StartDate, EndDate };
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
                        if ((Comparison == "Includes") && (!(item.Any(s => s == colValue))))
                        {
                            row.Delete();
                        }
                        else if ((Comparison == "DoesNotInclude") && (item.Any(s => s == colValue)))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }

                for (int c = 0; c < dt.Columns.Count; c++)
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

      
        protected void ChartDrp_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            // RadHtmlChart1.
            // this.RadHtmlChart1.PlotArea.Series.Clear();
            RadPivotGrid1.Rebind();
        }
    }
}