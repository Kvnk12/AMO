using Data;
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
    public partial class IncidentAnalysis : AspReportpagesBaseClass
    {

        //protected void showDefaultView(object sender, EventArgs e)
        //{
        //    Session["SysDefault"] = true;
        //    defaultChekbox.Checked = false;
        //    PublicChekbox.Checked = false;
        //    Response.Redirect("IncidentAnalysis.aspx");
        //}

        protected void SaveViewDetails(object sender, EventArgs e)
        {
            if (ViewNameText.Text.Trim() != "")
            {
               // var cid = (Client)Session["Client"];
                var cid = (ClientDetail)Session["SessionClient"];
                int uid = Convert.ToInt32(Session["UserId"]);
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var VieId = Helper.SaveReportViewDetails(cid.ClientId, uid, defaultChekbox.Checked, ViewNameText.Text, RadPivotGrid1.Fields, Convert.ToInt32(ReportFor.IncidentNavigator), PublicChekbox.Checked);
                ViewNameText.Text = string.Empty;
                var items = Helper.GetViewsList(uid, cid.ClientId, Convert.ToInt32(ReportFor.IncidentNavigator));
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

               // Session["ViewId"] = ViewIdd;
               
            }
            else
            {
                //Session["SysDefault"] = true;
                ////defaultChekbox.Checked = false;

            }
           // Response.Redirect("IncidentAnalysis.aspx");
        }

        
        protected void RadPivotGrid1_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
        {
            List<INTrendingReportEntity> replst = new List<INTrendingReportEntity>();
         //   var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            var objclient = (ClientDetail)Session["SessionClient"];
            replst = IncidentNavigatorHelper.INTrendingReportList(objclient.ClientId);


            if (replst != null)
            {
                (sender as RadPivotGrid).UseQueryableDataProvider = true;

                (sender as RadPivotGrid).DataSource = replst;//.AsEnumerable();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
           // RadPivotGrid1.ContextMenu.PreRender += ContextMenu_PreRender;
        }

        //private void ContextMenu_PreRender(object sender, EventArgs e)
        //{
        //    RadPivotGrid1.ContextMenu.FindItemByValue("Hide").Visible = false;
        //}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                //this.RadPivotGrid1.FilterTop(
                //    this.RadPivotGrid1.Fields["Location"],
                //    this.RadPivotGrid1.Fields["TotalPrice"] as PivotGridAggregateField,
                //    PivotGridAggregateType.Items,
                //    8, true);
            }
        }

        HashSet<string> columnNames = new HashSet<string>();
        Dictionary<string, ColumnSeries> columnSeriesByRowName = new Dictionary<string, ColumnSeries>();
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
                ColumnSeries columnSeries = null;

                if (columnSeriesByRowName.ContainsKey(rowName))
                {
                    columnSeries = columnSeriesByRowName[rowName];
                }
                else
                {
                    columnSeries = new ColumnSeries();
                    columnSeriesByRowName.Add(rowName, columnSeries);
                    columnSeries.Name = rowName;
                    columnSeries.LabelsAppearance.DataFormatString = "C";
                    this.RadHtmlChart1.PlotArea.Series.Add(columnSeries);
                }
                CategorySeriesItem item = new CategorySeriesItem();
                decimal value = 0;
                if (e.Cell.DataItem != null && decimal.TryParse(e.Cell.DataItem.ToString(), out value))
                {
                    item.Y = value;
                }
                else
                {
                    item.Y = null;
                }
                columnSeries.SeriesItems.Add(item);
            }
        }

        private string GetName(object[] indexes)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object index in indexes)
            {
                builder.Append(index.ToString());
                builder.Append(" / ");
            }
            builder.Remove(builder.Length - 3, 3);
            return builder.ToString();
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


        protected void SaveButton_Click(object sender, EventArgs e)

        {

            Session["CustomPersistenceSettingsKey"] = this.Session.SessionID;

            string fileId = Session["CustomPersistenceSettingsKey"].ToString();

            LoadButton.Enabled = true;

            RadPersistenceManager1.StorageProviderKey = fileId;

            RadPersistenceManager1.SaveState();

        }



        protected void ResetButton_Click(object sender, EventArgs e)

        {

            Response.Redirect(Request.Url.ToString());

        }



        protected void LoadButton_Click(object sender, EventArgs e)

        {

            string fileId = Session["CustomPersistenceSettingsKey"].ToString();

            RadPersistenceManager1.StorageProviderKey = fileId;

            RadPersistenceManager1.LoadState();

            RadPivotGrid1.Rebind();

        }

    }
}