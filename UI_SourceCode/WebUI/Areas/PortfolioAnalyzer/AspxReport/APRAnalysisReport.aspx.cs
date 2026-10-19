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
using WebUI.Areas.PortfolioAnalyzer.Models;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.AspxReport
{
    public partial class APRAnalysisReport : AspReportpagesBaseClass
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
          //  var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];

            IList<AnalysisModel> columnlist = PortfolioAnalyzerHelper.APRSourceColumnList(objclient.ClientId).ToList();


            foreach (var v in columnlist)
            {

                switch (v.ToolColumnName)
                {
                    case "AppCriticalityId":
                        if (RadPivotGrid1.Fields["CriticalityName"]!=null)
                        {
                            RadPivotGrid1.Fields["CriticalityName"].IsHidden = false;
                            RadPivotGrid1.Fields["CriticalityName"].Caption = v.ClientColumnName;
                        }
                        break;
                    case "AppDomainId":
                        if (RadPivotGrid1.Fields["AppDomainName"] != null)
                        {
                            RadPivotGrid1.Fields["AppDomainName"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "ApplicationName":
                        if (RadPivotGrid1.Fields["ApplicationName"]!=null)
                        {
                            RadPivotGrid1.Fields["ApplicationName"].IsHidden = false;
                            RadPivotGrid1.Fields["ApplicationName"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AppLifeCycleId":
                        if (RadPivotGrid1.Fields["AppLifeCycleName"] != null)
                        {
                            RadPivotGrid1.Fields["AppLifeCycleName"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AppSourceCodeId":
                        if (RadPivotGrid1.Fields["SourceCodeType"] != null)
                        {
                            RadPivotGrid1.Fields["SourceCodeType"].IsHidden = false;
                            RadPivotGrid1.Fields["SourceCodeType"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AppProcessingId":
                        if (RadPivotGrid1.Fields["AppProcessingType"] != null)
                        {
                            RadPivotGrid1.Fields["AppProcessingType"].IsHidden = false;
                            RadPivotGrid1.Fields["AppProcessingType"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AppSecurityLevelId":
                        if (RadPivotGrid1.Fields["AppSecurityLevel"] != null)
                        {
                            RadPivotGrid1.Fields["AppSecurityLevel"].IsHidden = false;
                            RadPivotGrid1.Fields["AppSecurityLevel"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AppSize":
                        if (RadPivotGrid1.Fields["AppSize"] != null)
                        {
                            RadPivotGrid1.Fields["AppSize"].IsHidden = false;
                            RadPivotGrid1.Fields["AppSize"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AppTypeId":
                        if (RadPivotGrid1.Fields["AppType"] != null)
                        {
                            RadPivotGrid1.Fields["AppType"].IsHidden = false;
                            RadPivotGrid1.Fields["AppType"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AvgConcurrentUsers":
                        if (RadPivotGrid1.Fields["AvgConcurrentUsers"] != null)
                        {
                            RadPivotGrid1.Fields["AvgConcurrentUsers"].IsHidden = false;
                            RadPivotGrid1.Fields["AvgConcurrentUsers"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AvgIncidentTickets":
                        if (RadPivotGrid1.Fields["AvgIncidentTickets"] != null)
                        {
                            RadPivotGrid1.Fields["AvgIncidentTickets"].IsHidden = false;
                            RadPivotGrid1.Fields["AvgIncidentTickets"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AvgProblemTickets":
                        if (RadPivotGrid1.Fields["AvgProblemTickets"] != null)
                        {
                            RadPivotGrid1.Fields["AvgProblemTickets"].IsHidden = false;
                            RadPivotGrid1.Fields["AvgProblemTickets"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AvgServiceRequestTickets":
                        if (RadPivotGrid1.Fields["AvgServiceRequestTickets"] != null)
                        {
                            RadPivotGrid1.Fields["AvgServiceRequestTickets"].IsHidden = false;
                            RadPivotGrid1.Fields["AvgServiceRequestTickets"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "BusinessFunction":
                        if (RadPivotGrid1.Fields["BusinessFunction"] != null)
                        {
                            RadPivotGrid1.Fields["BusinessFunction"].IsHidden = false;
                            RadPivotGrid1.Fields["BusinessFunction"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "BusinessLogicInClientTier":
                        if (RadPivotGrid1.Fields["BusinessLogicInClientTier"] != null)
                        {
                            RadPivotGrid1.Fields["BusinessLogicInClientTier"].IsHidden = false;
                            RadPivotGrid1.Fields["BusinessLogicInClientTier"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "ClientApplicationNumber":
                        if (RadPivotGrid1.Fields["ClientApplicationNumber"] != null)
                        {
                            RadPivotGrid1.Fields["ClientApplicationNumber"].IsHidden = false;
                            RadPivotGrid1.Fields["ClientApplicationNumber"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "CodeComplexity":
                        if (RadPivotGrid1.Fields["CodeComplexity"] != null)
                        {
                            RadPivotGrid1.Fields["CodeComplexity"].IsHidden = false;
                            RadPivotGrid1.Fields["CodeComplexity"].Caption = v.ClientColumnName;
                        }
                        break;


                    case "Country":
                        if (RadPivotGrid1.Fields["Country"] != null)
                        {
                            RadPivotGrid1.Fields["Country"].IsHidden = false;
                            RadPivotGrid1.Fields["Country"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "CreatedBy":
                        if (RadPivotGrid1.Fields["CreatedBy"] != null)
                        {
                            RadPivotGrid1.Fields["CreatedBy"].IsHidden = false;
                            RadPivotGrid1.Fields["CreatedBy"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "InScope":
                        if (RadPivotGrid1.Fields["InScope"] != null)
                        {
                            RadPivotGrid1.Fields["InScope"].IsHidden = false;
                            RadPivotGrid1.Fields["InScope"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "IsApplicationDocAvailable":
                        if (RadPivotGrid1.Fields["IsApplicationDocAvailable"] != null)
                        {
                            RadPivotGrid1.Fields["IsApplicationDocAvailable"].IsHidden = false;
                            RadPivotGrid1.Fields["IsApplicationDocAvailable"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "IsDisasterRecoveryPlanAvailable":
                        if (RadPivotGrid1.Fields["IsDisasterRecoveryPlanAvailable"] != null)
                        {
                            RadPivotGrid1.Fields["IsDisasterRecoveryPlanAvailable"].IsHidden = false;
                            RadPivotGrid1.Fields["IsDisasterRecoveryPlanAvailable"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "IsHostedExternally":
                        if (RadPivotGrid1.Fields["IsHostedExternally"] != null)
                        {
                            RadPivotGrid1.Fields["IsHostedExternally"].IsHidden = false;
                            RadPivotGrid1.Fields["IsHostedExternally"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "IsServer":
                        if (RadPivotGrid1.Fields["IsServer"] != null)
                        {
                            RadPivotGrid1.Fields["IsServer"].IsHidden = false;
                            RadPivotGrid1.Fields["IsServer"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "IsTechnicalDocAvailable":
                        if (RadPivotGrid1.Fields["IsTechnicalDocAvailable"] != null)
                        {
                            RadPivotGrid1.Fields["IsTechnicalDocAvailable"].IsHidden = false;
                            RadPivotGrid1.Fields["IsTechnicalDocAvailable"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "LastModifiedBy":
                        if (RadPivotGrid1.Fields["LastModifiedBy"] != null)
                        {
                            RadPivotGrid1.Fields["LastModifiedBy"].IsHidden = false;
                            RadPivotGrid1.Fields["LastModifiedBy"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "LegacyReasonExists":
                        if (RadPivotGrid1.Fields["LegacyReasonExists"] != null)
                        {
                            RadPivotGrid1.Fields["LegacyReasonExists"].IsHidden = false;
                            RadPivotGrid1.Fields["LegacyReasonExists"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "MaxConcurrentUsers":
                        if (RadPivotGrid1.Fields["MaxConcurrentUsers"] != null)
                        {
                            RadPivotGrid1.Fields["MaxConcurrentUsers"].IsHidden = false;
                            RadPivotGrid1.Fields["MaxConcurrentUsers"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "MigrationDate":
                        if (RadPivotGrid1.Fields["MigrationDate"] != null)
                        {
                            RadPivotGrid1.Fields["MigrationDate"].IsHidden = false;
                            RadPivotGrid1.Fields["MigrationDate"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "MOTSLevel":
                        if (RadPivotGrid1.Fields["MOTSLevel"] != null)
                        {
                            RadPivotGrid1.Fields["MOTSLevel"].IsHidden = false;
                            RadPivotGrid1.Fields["MOTSLevel"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "NumberOfBugs":
                        if (RadPivotGrid1.Fields["NumberOfBugs"] != null)
                        {
                            RadPivotGrid1.Fields["NumberOfBugs"].IsHidden = false;
                            RadPivotGrid1.Fields["NumberOfBugs"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "NumberOfInstances":
                        if (RadPivotGrid1.Fields["NumberOfInstances"] != null)
                        {
                            RadPivotGrid1.Fields["NumberOfInstances"].IsHidden = false;
                            RadPivotGrid1.Fields["NumberOfInstances"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "NumberOfLinesOfCode":
                        if (RadPivotGrid1.Fields["NumberOfLinesOfCode"] != null)
                        {
                            RadPivotGrid1.Fields["NumberOfLinesOfCode"].IsHidden = false;
                            RadPivotGrid1.Fields["NumberOfLinesOfCode"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "OutageDuration":
                        if (RadPivotGrid1.Fields["OutageDuration"] != null)
                        {
                            RadPivotGrid1.Fields["OutageDuration"].IsHidden = false;
                            RadPivotGrid1.Fields["OutageDuration"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "OutageFrequency":
                        if (RadPivotGrid1.Fields["OutageFrequency"] != null)
                        {
                            RadPivotGrid1.Fields["OutageFrequency"].IsHidden = false;
                            RadPivotGrid1.Fields["OutageFrequency"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "APROwningCompanyId":
                        if (RadPivotGrid1.Fields["APROwningCompanyId"] != null)
                        {
                            RadPivotGrid1.Fields["APROwningCompanyId"].IsHidden = false;
                            RadPivotGrid1.Fields["APROwningCompanyId"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "PlannedRetirementDate":
                        if (RadPivotGrid1.Fields["PlannedRetirementDate"] != null)
                        {
                            RadPivotGrid1.Fields["PlannedRetirementDate"].IsHidden = false;
                            RadPivotGrid1.Fields["PlannedRetirementDate"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "RegionNames":
                        if (RadPivotGrid1.Fields["RegionNames"] != null)
                        {
                            RadPivotGrid1.Fields["RegionNames"].IsHidden = false;
                            RadPivotGrid1.Fields["RegionNames"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "SizeUnits":
                        if (RadPivotGrid1.Fields["SizeUnits"] != null)
                        {
                            RadPivotGrid1.Fields["SizeUnits"].IsHidden = false;
                            RadPivotGrid1.Fields["SizeUnits"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "TechnicalOwner":
                        if (RadPivotGrid1.Fields["TechnicalOwner"] != null)
                        {
                            RadPivotGrid1.Fields["TechnicalOwner"].IsHidden = false;
                            RadPivotGrid1.Fields["TechnicalOwner"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "TechnicalOwnerEmail":
                        if (RadPivotGrid1.Fields["TechnicalOwnerEmail"] != null)
                        {
                            RadPivotGrid1.Fields["TechnicalOwnerEmail"].IsHidden = false;
                            RadPivotGrid1.Fields["TechnicalOwnerEmail"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "TransactionVolumn":
                        if (RadPivotGrid1.Fields["TransactionVolumn"] != null)
                        {
                            RadPivotGrid1.Fields["TransactionVolumn"].IsHidden = false;
                            RadPivotGrid1.Fields["TransactionVolumn"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "TransactionVolumnUnit":
                        if (RadPivotGrid1.Fields["TransactionVolumnUnit"] != null)
                        {
                            RadPivotGrid1.Fields["TransactionVolumnUnit"].IsHidden = false;
                            RadPivotGrid1.Fields["TransactionVolumnUnit"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AliasName":
                        if (RadPivotGrid1.Fields["AliasName"] != null)
                        {
                            RadPivotGrid1.Fields["AliasName"].IsHidden = false;
                            RadPivotGrid1.Fields["AliasName"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AppArchitectureId":
                        if (RadPivotGrid1.Fields["AppArchitecture"] != null)
                        {
                            RadPivotGrid1.Fields["AppArchitecture"].IsHidden = false;
                            RadPivotGrid1.Fields["AppArchitecture"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AppEnvironmentId":
                        if (RadPivotGrid1.Fields["AppEnvironment"] != null)
                        {
                            RadPivotGrid1.Fields["AppEnvironment"].IsHidden = false;
                            RadPivotGrid1.Fields["AppEnvironment"].Caption = v.ClientColumnName;
                        }
                        break;

                    case "AppIsActive":
                        if (RadPivotGrid1.Fields["AppIsActive"] != null)
                        {
                            RadPivotGrid1.Fields["AppIsActive"].IsHidden = false;
                            RadPivotGrid1.Fields["AppIsActive"].Caption = v.ClientColumnName;
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
            this.RadHtmlChart1.PlotArea.XAxis.Items.Clear();
            this.RadHtmlChart1.PlotArea.Series.Clear();
            RadPivotGrid1.Rebind();

        }


        //-------------------------------------------//

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { //  var cid = (Client)Session["Client"];
                CsrfHandler.Validate(this.Page, forgeryToken);
                var cid = (ClientDetail)Session["SessionClient"];
                int uid = (int)Session["UserId"];
                RadPersistenceManager1.StorageProvider = new GridStateStorageProvider();
                if (!this.IsPostBack)
                {
                    var items = Helper.GetViewsList(uid, cid.ClientId, (int)ReportFor.PortfolioAnalyzer);
                    doprdown1.DataSource = items;
                    doprdown1.DataBind();
                    //if (Session["SysDefault"] == null)
                    //    Session["SysDefault"] = false;

                    //if (Session["ViewId"] != null)
                    //{
                    //    var SessionViewId = (int)Session["ViewId"];
                    //    if (SessionViewId > 0)
                    //    {
                    //        var dViewid = Helper.Find_IsDefault_View(cid.ClientId, uid, (int)ReportFor.PortfolioAnalyzer);
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
                    //    var dViewid = Helper.Find_IsDefault_View(cid.ClientId, uid, (int)ReportFor.PortfolioAnalyzer);
                    //    if (dViewid.ViewName != null)
                    //    {
                    //        doprdown1.SelectedValue = dViewid.Id.ToString();
                    //        defaultChekbox.Checked = true;
                    //        ViewNameText.Text = dViewid.ViewName;
                    //        //removing filters
                    //        //RadPivotGrid1.Rebind();
                    //        RadPivotGrid1.ClearAllFilters();
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
                //RadPivotGrid1.Rebind();
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
            Response.Redirect("APRAnalysisReport.aspx");
        }

        protected void SaveViewDetails(object sender, EventArgs e)
        {
            if (ViewNameText.Text.Trim() != "")
            {
                var cid = (ClientDetail)Session["SessionClient"];//    (Client)Session["Client"];
                int uid = (int)Session["UserId"];

                string key = cid.ClientId.ToString().Trim() + "|" + uid.ToString().Trim() + "|" + ViewNameText.Text.Trim() + "|" + Convert.ToString((int)ReportFor.PortfolioAnalyzer).Trim() + "|" + "false" + "|" + defaultChekbox.Checked;

                RadPersistenceManager1.StorageProviderKey = key;
                RadPersistenceManager1.SaveState();
                ViewNameText.Text = string.Empty;
                var items = Helper.GetViewsList(uid, cid.ClientId, (int)ReportFor.PortfolioAnalyzer);
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
                string key = cid.ClientId.ToString().Trim() + "|" + uid.ToString().Trim() + "|" + ViewNameText.Text.Trim() + "|" + Convert.ToString((int)ReportFor.PortfolioAnalyzer).Trim() + "|" + "false" + "|" + "false";

                string fileId = key;// Session["CustomPersistenceSettingsKey"].ToString();
                RadPersistenceManager1.StorageProviderKey = fileId;
                RadPersistenceManager1.LoadState();
                RadPivotGrid1.Rebind();
            }
            else
            {
                Response.Redirect("APRAnalysisReport.aspx");
            }
           
        }

        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
          //  var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            DataTable source = PortfolioAnalyzerHelper.APRAnalysisReport(objclient.ClientId);
            this.RadHtmlChart1.PlotArea.XAxis.Items.Clear();
            this.RadHtmlChart1.PlotArea.Series.Clear();
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
                   // lineSeries.Stacked = true;

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