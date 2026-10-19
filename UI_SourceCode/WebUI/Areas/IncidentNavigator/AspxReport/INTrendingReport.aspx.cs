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
using WebUI.App_Start;
using WebUI.Areas.IncidentNavigator.Dto;
using WebUI.Dto;

namespace WebUI.Areas.IncidentNavigator.AspxReport
{
    public partial class INTrendingReport : AspReportpagesBaseClass
    {
        private int? ClientId
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["ClientId"]);
            }
        }
        private string ConfigItem
        {
            get
            {
                return Convert.ToString(Request.QueryString["ConfigItem"]);
            }
        }
        private string TicketType
        {
            get
            {
                return Convert.ToString(Request.QueryString["TicketType"]);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            columnNames = new HashSet<string>();
            LineSeriesByRowName = new Dictionary<string, LineSeries>();
           // var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var target = unitOfWork.INIncidentServicehours.Get(x => x.ClientId == ClientId).ToList();
                if (target.Count > 0)
                {
                   if( Convert.ToInt32(target.FirstOrDefault().HasSLAManager)==1)
                    {
                        RadPivotGrid1.Fields["Resolutiontime"].IsHidden = false;
                        RadPivotGrid1.Fields["SLACostPerTicket"].IsHidden = false;
                    }
                   else
                    {
                        RadPivotGrid1.Fields["INResolutiontime"].IsHidden = false;
                        RadPivotGrid1.Fields["INCostPerTicket"].IsHidden = false;
                    }


                }

                else
                {
                    RadPivotGrid1.Fields["Resolutiontime"].IsHidden = false;
                    RadPivotGrid1.Fields["SLACostPerTicket"].IsHidden = false;
                }
            }
                IList<TrendingReportInput> columnlist = IncidentNavigatorHelper.INCubeSourceColumnList(objclient.ClientId).ToList();

            //PivotGridColumnField columnField = new PivotGridColumnField();
            //PivotGridRowField rowField = new PivotGridRowField();
            foreach (var v in columnlist)
            {

                switch (v.ToolColumnName)
                {
                    case "Priority":
                        
                        RadPivotGrid1.Fields["Priority"].Caption = v.ClientColumnName;
                        break;
                    case "Status":
                  
                        RadPivotGrid1.Fields["Status"].Caption = v.ClientColumnName;
                        break;

                    case "Urgency":

                        RadPivotGrid1.Fields["Urgency"].IsHidden = false;
                        RadPivotGrid1.Fields["Urgency"].Caption = v.ClientColumnName;

                        break;


                    case "Type":

                        RadPivotGrid1.Fields["Type"].IsHidden = false;
                        RadPivotGrid1.Fields["Type"].Caption = v.ClientColumnName;

                        break;

                    case "Outagetype":

                        RadPivotGrid1.Fields["Outagetype"].IsHidden = false;
                        RadPivotGrid1.Fields["Outagetype"].Caption = v.ClientColumnName;

                        break;

                    case "Divisionimpacted":

                        RadPivotGrid1.Fields["Divisionimpacted"].IsHidden = false;
                        RadPivotGrid1.Fields["Divisionimpacted"].Caption = v.ClientColumnName;

                        break;

                    case "Businessunitimpacted":

                        RadPivotGrid1.Fields["Businessunitimpacted"].IsHidden = false;
                        RadPivotGrid1.Fields["Businessunitimpacted"].Caption = v.ClientColumnName;

                        break;

                    case "Servicecategory":

                        RadPivotGrid1.Fields["Servicecategory"].IsHidden = false;
                        RadPivotGrid1.Fields["Servicecategory"].Caption = v.ClientColumnName;

                        break;

                    case "Servicesubcategory":

                        RadPivotGrid1.Fields["Servicesubcategory"].IsHidden = false;
                        RadPivotGrid1.Fields["Servicesubcategory"].Caption = v.ClientColumnName;

                        break;

                    case "Servicearea":

                        RadPivotGrid1.Fields["Servicearea"].IsHidden = false;
                        RadPivotGrid1.Fields["Servicearea"].Caption = v.ClientColumnName;

                        break;

                    case "Regionimpacted":

                        RadPivotGrid1.Fields["Regionimpacted"].IsHidden = false;
                        RadPivotGrid1.Fields["Regionimpacted"].Caption = v.ClientColumnName;

                        break;

                    case "Locationsimpacted":

                        RadPivotGrid1.Fields["Locationsimpacted"].IsHidden = false;
                        RadPivotGrid1.Fields["Locationsimpacted"].Caption = v.ClientColumnName;

                        break;

                    case "Customerdept":

                        RadPivotGrid1.Fields["Customerdept"].IsHidden = false;
                        RadPivotGrid1.Fields["Customerdept"].Caption = v.ClientColumnName;

                        break;

                    case "Customerlastname":

                        RadPivotGrid1.Fields["Customerlastname"].IsHidden = false;
                        RadPivotGrid1.Fields["Customerlastname"].Caption = v.ClientColumnName;

                        break;

                    case "Customerfirstname":

                        RadPivotGrid1.Fields["Customerfirstname"].IsHidden = false;
                        RadPivotGrid1.Fields["Customerfirstname"].Caption = v.ClientColumnName;

                        break;

                    case "Customertitle":

                        RadPivotGrid1.Fields["Customertitle"].IsHidden = false;
                        RadPivotGrid1.Fields["Customertitle"].Caption = v.ClientColumnName;

                        break;

                    case "Customersatisfaction":

                        RadPivotGrid1.Fields["Customersatisfaction"].IsHidden = false;
                        RadPivotGrid1.Fields["Customersatisfaction"].Caption = v.ClientColumnName;

                        break;

                    case "Impacttype":

                        RadPivotGrid1.Fields["Impacttype"].IsHidden = false;
                        RadPivotGrid1.Fields["Impacttype"].Caption = v.ClientColumnName;

                        break;

                    case "Configitemid":

                        RadPivotGrid1.Fields["Configitemid"].IsHidden = false;
                        RadPivotGrid1.Fields["Configitemid"].Caption = v.ClientColumnName;

                        break;

                    case "Configtype":

                        RadPivotGrid1.Fields["Configtype"].IsHidden = false;
                        RadPivotGrid1.Fields["Configtype"].Caption = v.ClientColumnName;

                        break;

                    case "Configitemname":

                        RadPivotGrid1.Fields["Configitemname"].IsHidden = false;
                        RadPivotGrid1.Fields["Configitemname"].Caption = v.ClientColumnName;

                        break;

                    case "Environmentimpacted":

                        RadPivotGrid1.Fields["Environmentimpacted"].IsHidden = false;
                        RadPivotGrid1.Fields["Environmentimpacted"].Caption = v.ClientColumnName;

                        break;

                    case "Module":

                        RadPivotGrid1.Fields["Module"].IsHidden = false;
                        RadPivotGrid1.Fields["Module"].Caption = v.ClientColumnName;

                        break;

                    case "Rootcausetype":

                        RadPivotGrid1.Fields["Rootcausetype"].IsHidden = false;
                        RadPivotGrid1.Fields["Rootcausetype"].Caption = v.ClientColumnName;

                        break;

                    case "Resolutiontype":

                        RadPivotGrid1.Fields["Resolutiontype"].IsHidden = false;
                        RadPivotGrid1.Fields["Resolutiontype"].Caption = v.ClientColumnName;

                        break;

                    case "Solutionid":

                        RadPivotGrid1.Fields["Solutionid"].IsHidden = false;
                        RadPivotGrid1.Fields["Solutionid"].Caption = v.ClientColumnName;

                        break;

                    case "Deploymentrelease":

                        RadPivotGrid1.Fields["Deploymentrelease"].IsHidden = false;
                        RadPivotGrid1.Fields["Deploymentrelease"].Caption = v.ClientColumnName;

                        break;

                    case "Relatedchangeids":

                        RadPivotGrid1.Fields["Relatedchangeids"].IsHidden = false;
                        RadPivotGrid1.Fields["Relatedchangeids"].Caption = v.ClientColumnName;

                        break;

                    case "Relatedids":

                        RadPivotGrid1.Fields["Relatedids"].IsHidden = false;
                        RadPivotGrid1.Fields["Relatedids"].Caption = v.ClientColumnName;

                        break;

                    case "Relatedproblemids":

                        RadPivotGrid1.Fields["Relatedproblemids"].IsHidden = false;
                        RadPivotGrid1.Fields["Relatedproblemids"].Caption = v.ClientColumnName;

                        break;

                    case "Helpdesk":

                        RadPivotGrid1.Fields["Helpdesk"].IsHidden = false;
                        RadPivotGrid1.Fields["Helpdesk"].Caption = v.ClientColumnName;

                        break;

                    case "Assignedtoperson":

                        RadPivotGrid1.Fields["Assignedtoperson"].IsHidden = false;
                        RadPivotGrid1.Fields["Assignedtoperson"].Caption = v.ClientColumnName;

                        break;

                    case "Assignedtosupportgroup":

                        RadPivotGrid1.Fields["Assignedtosupportgroup"].IsHidden = false;
                        RadPivotGrid1.Fields["Assignedtosupportgroup"].Caption = v.ClientColumnName;

                        break;

                    case "Createdbygroup":

                        RadPivotGrid1.Fields["Createdbygroup"].IsHidden = false;
                        RadPivotGrid1.Fields["Createdbygroup"].Caption = v.ClientColumnName;

                        break;

                    case "Createdbyperson":

                        RadPivotGrid1.Fields["Createdbyperson"].IsHidden = false;
                        RadPivotGrid1.Fields["Createdbyperson"].Caption = v.ClientColumnName;

                        break;

                    case "Createddatetime":

                        RadPivotGrid1.Fields["Createdtime"].IsHidden = false;
                        RadPivotGrid1.Fields["Createdweekday"].IsHidden = false;
                        RadPivotGrid1.Fields["Createdweeknum"].IsHidden = false;
                        RadPivotGrid1.Fields["Createdmonth"].IsHidden = false;
                        RadPivotGrid1.Fields["Createdyear"].IsHidden = false;

                        break;

                    case "Acknowlededbygroup":

                        RadPivotGrid1.Fields["Acknowlededbygroup"].IsHidden = false;
                        RadPivotGrid1.Fields["Acknowlededbygroup"].Caption = v.ClientColumnName;

                        break;

                    case "Acknowlededbyperson":

                        RadPivotGrid1.Fields["Acknowlededbyperson"].IsHidden = false;
                        RadPivotGrid1.Fields["Acknowlededbyperson"].Caption = v.ClientColumnName;

                        break;


                    case "Acknowledgeddatetime":

                        RadPivotGrid1.Fields["Acknowledgedtime"].IsHidden = false;
                        RadPivotGrid1.Fields["Acknowledgedweekday"].IsHidden = false;
                        RadPivotGrid1.Fields["Acknowledgedweeknum"].IsHidden = false;
                        RadPivotGrid1.Fields["Acknowledgedmonth"].IsHidden = false;
                        RadPivotGrid1.Fields["Acknowledgedyear"].IsHidden = false;

                        break;

                    case "Resolvedbygroup":

                        RadPivotGrid1.Fields["Resolvedbygroup"].IsHidden = false;
                        RadPivotGrid1.Fields["Resolvedbygroup"].Caption = v.ClientColumnName;

                        break;

                    case "Resolvedbyperson":

                        RadPivotGrid1.Fields["Resolvedbyperson"].IsHidden = false;
                        RadPivotGrid1.Fields["Resolvedbyperson"].Caption = v.ClientColumnName;

                        break;

                    case "Resolvedbyvendor":

                        RadPivotGrid1.Fields["Resolvedbyvendor"].IsHidden = false;
                        RadPivotGrid1.Fields["Resolvedbyvendor"].Caption = v.ClientColumnName;

                        break;

                    case "Resolutiondatetime":

                      
                        RadPivotGrid1.Fields["Resolutionweekday"].IsHidden = false;
                        RadPivotGrid1.Fields["Resolutionweeknum"].IsHidden = false;
                        RadPivotGrid1.Fields["Resolutionyear"].IsHidden = false;
                        RadPivotGrid1.Fields["Resolutionmonth"].IsHidden = false;

                        break;

                    case "Servicerestoredbygroup":

                        RadPivotGrid1.Fields["Servicerestoredbygroup"].IsHidden = false;
                        RadPivotGrid1.Fields["Servicerestoredbygroup"].Caption = v.ClientColumnName;

                        break;

                    case "Servicerestoredbyperson":

                        RadPivotGrid1.Fields["Servicerestoredbyperson"].IsHidden = false;
                        RadPivotGrid1.Fields["Servicerestoredbyperson"].Caption = v.ClientColumnName;

                        break;

                    case "Servicerestoreddatetime":

                        RadPivotGrid1.Fields["Servicerestoredtime"].IsHidden = false;
                        RadPivotGrid1.Fields["Servicerestoredweekday"].IsHidden = false;
                        RadPivotGrid1.Fields["Servicerestoredweeknum"].IsHidden = false;
                        RadPivotGrid1.Fields["Servicerestoredmonth"].IsHidden = false;
                        RadPivotGrid1.Fields["Servicerestoredyear"].IsHidden = false;

                        break;

                    case "Assignedbygroup":

                        RadPivotGrid1.Fields["Assignedbygroup"].IsHidden = false;
                        RadPivotGrid1.Fields["Assignedbygroup"].Caption = v.ClientColumnName;

                        break;

                    case "Assignedbyperson":

                        RadPivotGrid1.Fields["Assignedbyperson"].IsHidden = false;
                        RadPivotGrid1.Fields["Assignedbyperson"].Caption = v.ClientColumnName;

                        break;

                    case "Esitmatedresponsetimebygroup":

                        RadPivotGrid1.Fields["Esitmatedresponsetimebygroup"].IsHidden = false;
                        RadPivotGrid1.Fields["Esitmatedresponsetimebygroup"].Caption = v.ClientColumnName;

                        break;

                    case "Waivedindicator":

                        RadPivotGrid1.Fields["Waivedindicator"].IsHidden = false;
                        RadPivotGrid1.Fields["Waivedindicator"].Caption = v.ClientColumnName;

                        break;

                    case "Totalsuspenddurationassignedtoack":

                        RadPivotGrid1.Fields["Totalsuspenddurationassignedtoack"].IsHidden = false;
                        RadPivotGrid1.Fields["Totalsuspenddurationassignedtoack"].Caption = v.ClientColumnName;

                        break;

                    case "Totalsuspenddurationacktoer":

                        RadPivotGrid1.Fields["Totalsuspenddurationacktoer"].IsHidden = false;
                        RadPivotGrid1.Fields["Totalsuspenddurationacktoer"].Caption = v.ClientColumnName;

                        break;

                    case "Totalsuspenddurationertoresolution":

                        RadPivotGrid1.Fields["Totalsuspenddurationertoresolution"].IsHidden = false;
                        RadPivotGrid1.Fields["Totalsuspenddurationertoresolution"].Caption = v.ClientColumnName;

                        break;

                    case "Resolvedindicator":

                        RadPivotGrid1.Fields["Resolvedindicator"].IsHidden = false;
                        RadPivotGrid1.Fields["Resolvedindicator"].Caption = v.ClientColumnName;

                        break;

                    case "IsNTTGroup":

                        RadPivotGrid1.Fields["IsNTTGroup"].IsHidden = false;
                        RadPivotGrid1.Fields["IsNTTGroup"].Caption = v.ClientColumnName;

                        break;

                    case "SupportGroupType":

                        RadPivotGrid1.Fields["SupportGroupType"].IsHidden = false;
                        RadPivotGrid1.Fields["SupportGroupType"].Caption = v.ClientColumnName;

                        break;

                    case "Scheduledchangecount":

                        RadPivotGrid1.Fields["Scheduledchangecount"].IsHidden = false;
                        RadPivotGrid1.Fields["Scheduledchangecount"].Caption = v.ClientColumnName;

                        break;

                    case "Company":

                        RadPivotGrid1.Fields["Company"].IsHidden = false;
                        RadPivotGrid1.Fields["Company"].Caption = v.ClientColumnName;

                        break;

                    case "Site":

                        RadPivotGrid1.Fields["Site"].IsHidden = false;
                        RadPivotGrid1.Fields["Site"].Caption = v.ClientColumnName;

                        break;

                    case "Assigneecompany":

                        RadPivotGrid1.Fields["Assigneecompany"].IsHidden = false;
                        RadPivotGrid1.Fields["Assigneecompany"].Caption = v.ClientColumnName;

                        break;
                    case "Assigneeorganization":

                        RadPivotGrid1.Fields["Assigneeorganization"].IsHidden = false;
                        RadPivotGrid1.Fields["Assigneeorganization"].Caption = v.ClientColumnName;

                        break;
                    case "Estimatedworkeffort":

                        RadPivotGrid1.Fields["Estimatedworkeffort"].IsHidden = false;
                        RadPivotGrid1.Fields["Estimatedworkeffort"].Caption = v.ClientColumnName;

                        break;
                    case "Actualworkeffort":

                        RadPivotGrid1.Fields["Actualworkeffort"].IsHidden = false;
                        RadPivotGrid1.Fields["Actualworkeffort"].Caption = v.ClientColumnName;

                        break;
                    case "Recyclecount":

                        RadPivotGrid1.Fields["Recyclecount"].IsHidden = false;
                        RadPivotGrid1.Fields["Recyclecount"].Caption = v.ClientColumnName;

                        break;
                    case "ConfigItemCategory":

                        RadPivotGrid1.Fields["ConfigItemCategory"].IsHidden = false;
                        RadPivotGrid1.Fields["ConfigItemCategory"].Caption = v.ClientColumnName;

                        break;
                    case "ClosureCategory":

                        RadPivotGrid1.Fields["ClosureCategory"].IsHidden = false;
                        RadPivotGrid1.Fields["ClosureCategory"].Caption = v.ClientColumnName;

                        break;

                    case "Severity":

                        RadPivotGrid1.Fields["Severity"].IsHidden = false;
                        RadPivotGrid1.Fields["Severity"].Caption = v.ClientColumnName;

                        break;

                    case "WorkGroupLevel":

                        RadPivotGrid1.Fields["WorkGroupLevel"].IsHidden = false;
                        RadPivotGrid1.Fields["WorkGroupLevel"].Caption = v.ClientColumnName;

                        break;

                    case "AffectedItemName":

                        RadPivotGrid1.Fields["AffectedItemName"].IsHidden = false;
                        RadPivotGrid1.Fields["AffectedItemName"].Caption = v.ClientColumnName;

                        break;

                  

                    case "Resolutionyear":

                        RadPivotGrid1.Fields["Resolutionyear"].IsHidden = false;
                        RadPivotGrid1.Fields["Resolutionyear"].Caption = v.ClientColumnName;

                        break;

                  

                  
                }
            }
            RadPivotGrid1.Rebind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            CsrfHandler.Validate(this.Page, forgeryToken);

        }

        protected void RadPivotGrid1_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
            //if (ClientId != null && !string.IsNullOrEmpty(ConfigItem) && !string.IsNullOrEmpty(TicketType))
            //{
          //  var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            var source = IncidentNavigatorHelper.INTrendingReportList(objclient.ClientId);
                if (source != null)
                {
                    
                    (sender as RadPivotGrid).DataSource = source;
                }
            //}
        }


        HashSet<string> columnNames = new HashSet<string>();
        Dictionary<string, LineSeries> LineSeriesByRowName = new Dictionary<string, LineSeries>();
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
    }
    }