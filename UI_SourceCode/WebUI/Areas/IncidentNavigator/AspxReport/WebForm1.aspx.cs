using System;
using Telerik.Web.UI;
using System.Linq;
using System.Collections.Generic;
using Telerik.Web.UI.PivotGrid.Core.Fields;
using System.Web.UI;
using Telerik.Web.UI.PivotGrid.Xmla;
using WebUI.Areas.IncidentNavigator.Dto;
using WebUI.App_Start;

namespace WebUI.Areas.IncidentNavigator.AspxReport
{
    public partial class WebForm1 : AspReportpagesBaseClass
    {

        private int? ClientId
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["ClientId"]);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            RadPivotGrid2.OlapSettings.ProviderType = PivotGridOlapProviderType.Xmla;
            RadPivotGrid2.Rebind();

            if (!IsPostBack)
            {
            //    var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ////XmlaDataProvider provider = new XmlaDataProvider();
            ////RadPivotGrid1.OlapSettings.ProviderType = PivotGridOlapProviderType.Xmla;
            ////XmlaConnectionSettings cubeConnectionDetails = new XmlaConnectionSettings();
            ////cubeConnectionDetails.Cube = "IncidentAnalyticalCube";
            ////cubeConnectionDetails.Database = "NextGenToolIncidentCube";
            ////cubeConnectionDetails.ServerAddress = "http://192.43.232.177/OLAP/msmdpump.dll";
            ////XmlaQueryProperty localization = new XmlaQueryProperty();
            ////localization.Name = "[Clients].[Client Name]";
            ////localization.Value = ClientId;
            ////cubeConnectionDetails.QueryProperties.Add(localization);
            ////provider.ConnectionSettings = cubeConnectionDetails;
            //RadPivotGrid1.OlapSettings.ProviderType = PivotGridOlapProviderType.Xmla;
            //RadPivotGrid1.OlapSettings.XmlaConnectionSettings.Cube = "CubeClient"+ "1";
            //RadPivotGrid1.OlapSettings.XmlaConnectionSettings.DataBase = "NextGenToolIncidentCube";
            //RadPivotGrid1.OlapSettings.XmlaConnectionSettings.ServerAddress = "http://10.100.100.12/OLAP/msmdpump.dll";

          
                //foreach(var v in RadPivotGrid1.Fields)
                //{

                //    v.IsHidden = true;
                //}
              //  IList<TrendingReportInput> columnlist = IncidentNavigatorHelper.INCubeSourceColumnList(1).ToList();

                //PivotGridColumnField columnField = new PivotGridColumnField();
                //PivotGridRowField rowField = new PivotGridRowField();
               // foreach (var v in columnlist)
                //{

                //    switch (v.ToolColumnName)
                //    {
                //        case "Priority":
                //            RadPivotGrid1.Fields["Priority"].IsHidden = false;
                //            RadPivotGrid1.Fields["Priority"].Caption = v.ClientColumnName;
                //            break;
                //        case "Status":
                //            RadPivotGrid1.Fields["Status"].IsHidden = false;
                //            RadPivotGrid1.Fields["Status"].Caption = v.ClientColumnName;
                //            break;

                //        //case "Recyclecount":

                //        //    RadPivotGrid1.Fields["Recyclecount"].IsHidden = false;
                //        //    RadPivotGrid1.Fields["Recyclecount"].Caption = v.ClientColumnName;
                           
                //        //    break;

                //        //case "Actualworkeffort":
                //        //    RadPivotGrid1.Fields["Actualworkeffort"].IsHidden = false;
                //        //    RadPivotGrid1.Fields["Actualworkeffort"].Caption = v.ClientColumnName;
                //        //    break;

                //        //case "Estimatedworkeffort":
                //        //    RadPivotGrid1.Fields["Estimatedworkeffort"].IsHidden = false;
                //        //    RadPivotGrid1.Fields["Estimatedworkeffort"].Caption = v.ClientColumnName;
                //        //    break;

                //        //case "Scheduledchangecount":
                //        //    RadPivotGrid1.Fields["Scheduledchangecount"].IsHidden = false;
                //        //    RadPivotGrid1.Fields["Scheduledchangecount"].Caption = v.ClientColumnName;
                //        //    break;

                //        //case "Totalsuspenddurationacktoer":
                //        //    RadPivotGrid1.Fields["Totalsuspenddurationacktoer"].IsHidden = false;
                //        //    RadPivotGrid1.Fields["Totalsuspenddurationacktoer"].Caption = v.ClientColumnName;
                //        //    break;


                //        //case "Totalsuspenddurationassignedtoack":
                //        //    RadPivotGrid1.Fields["Totalsuspenddurationassignedtoack"].IsHidden = false;
                //        //    RadPivotGrid1.Fields["Totalsuspenddurationassignedtoack"].Caption = v.ClientColumnName;
                //        //    break;


                //        //case "Totalsuspenddurationertoresolution":
                //        //    RadPivotGrid1.Fields["Totalsuspenddurationertoresolution"].IsHidden = false;
                //        //    RadPivotGrid1.Fields["Totalsuspenddurationertoresolution"].Caption = v.ClientColumnName;
                //        //    break;

                //        //case "AffectedItemName":
                //        //    RadPivotGrid1.Fields["AffectedItemName"].IsHidden = false;
                //        //    RadPivotGrid1.Fields["AffectedItemName"].Caption = v.ClientColumnName;
                //        //    break;

                //        //case "Assignedtoperson":
                //        //    RadPivotGrid1.Fields["Assignedtoperson"].IsHidden = false;
                //        //    RadPivotGrid1.Fields["Assignedtoperson"].Caption = v.ClientColumnName;
                //        //    break;

                //            //case "Assignedtosupportgroup":
                //            //    RadPivotGrid1.Fields.Add(filterField);
                //            //    filterField.DataField = "[Incident 1].[Assignedtosupportgroup]";
                //            //    filterField.UniqueName = "Assignedtosupportgroup";
                //            //    filterField.Caption = v.ClientColumnName;
                //            //    break;


                //            //case "ClosureCategory":
                //            //    RadPivotGrid1.Fields.Add(filterField);
                //            //    filterField.DataField = "[Incident 1].[ClosureCategory]";
                //            //    filterField.UniqueName = "ClosureCategory";
                //            //    filterField.Caption = v.ClientColumnName;
                //            //    break;


                //            //case "Configitemname":
                //            //    RadPivotGrid1.Fields.Add(filterField);
                //            //    filterField.DataField = "[Incident 1].[Configitemname]";
                //            //    filterField.UniqueName = "Configitemname";
                //            //    filterField.Caption = v.ClientColumnName;
                //            //    break;

                //            //case "Resolutiontype":
                //            //    RadPivotGrid1.Fields.Add(filterField);
                //            //    filterField.DataField = "[Incident 1].[Resolutiontype]";
                //            //    filterField.UniqueName = "Resolutiontype";
                //            //    filterField.Caption = v.ClientColumnName;
                //            //    break;


                //            //case "Rootcausetype":
                //            //    RadPivotGrid1.Fields.Add(filterField);
                //            //    filterField.DataField = "[Incident 1].[Rootcausetype]";
                //            //    filterField.UniqueName = "Rootcausetype";
                //            //    filterField.Caption = v.ClientColumnName;
                //            //    break;

                //            ////case "Severity":
                //            ////    RadPivotGrid1.Fields.Add(filterField);
                //            ////    filterField.DataField = "[Incident 1].[Severity]";
                //            ////    filterField.UniqueName = "Severity";
                //            ////    filterField.Caption = v.ClientColumnName;
                //            //    //break;

                //            //case "SupportGroupType":
                //            //    RadPivotGrid1.Fields.Add(filterField);
                //            //    filterField.DataField = "[Incident 1].[SupportGroupType]";
                //            //    filterField.UniqueName = "SupportGroupType";
                //            //    filterField.Caption = v.ClientColumnName;
                //            //    break;

                //            //case "Type":
                //            //    RadPivotGrid1.Fields.Add(filterField);
                //            //    filterField.DataField = "[Incident 1].[Type]";
                //            //    filterField.UniqueName = "Type";
                //            //    filterField.Caption = v.ClientColumnName;
                //            //    break;

                //            ////case "Urgency":
                //            ////    RadPivotGrid1.Fields.Add(filterField);
                //            ////    filterField.DataField = "[Incident 1].[Urgency]";
                //            ////    filterField.UniqueName = "Urgency";
                //            ////    filterField.Caption = v.ClientColumnName;
                //            ////    break;

                //            //case "WorkGroupLevel":
                //            //    RadPivotGrid1.Fields.Add(filterField);
                //            //    filterField.DataField = "[Incident 1].[WorkGroupLevel]";
                //            //    filterField.UniqueName = "WorkGroupLevel";
                //            //    filterField.Caption = v.ClientColumnName;
                //            //    break;

                //            //case "Acknowledgeddatetime":
                //            //    RadPivotGrid1.Fields.Add(filterField);
                //            //    filterField.DataField = "[Incident 1].[Acknowledgedtime]";
                //            //    filterField.UniqueName = "Acknowledgedtime";
                //            //    filterField.Caption = "Acknowledged time";
                //            //    break;
                //    }
                //}


            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            CsrfHandler.Validate(this.Page, forgeryToken);

        }


        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);


        //}

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //string client = "[Clients].&[" + ClientId + "]";
            //this.RadPivotGrid1.SetFilterIncludes("ClientName", new List<object>() { client });
            //Control filterButton = FindControlRecursive(RadPivotGrid1, "ClientName_FilterPictButton");
            //Control sortButton = FindControlRecursive(RadPivotGrid1, "ClientName_SortLinkButton");
            //if (filterButton != null)
            //{
            //    filterButton.Visible = false;
            //}

            //if (sortButton != null)
            //{
            //    sortButton.Visible = false;
            //}
        }

        protected void RadPivotGrid1_GetDescriptionsDataCompleted(object sender, Telerik.Web.UI.PivotGrid.Core.Fields.GetDescriptionsDataCompletedEventArgs e)
        {
            //var root = e.DescriptionsData.RootFieldInfo;

            //if (root.HasChildren)
            //{
            //    List<ContainerNode> nodes = new List<ContainerNode>();

            //    foreach (var item in root.Children)
            //    {
            //        nodes.Add(item);
            //    }

            //    foreach (var item in nodes)
            //    {
            //        if (item.Name == "[Clients]")
            //        {
            //            root.Children.Remove(item);
            //        }
            //    }
            //}
        }

        public static Control FindControlRecursive(Control control, string id)
        {
            if (control == null) return null;
            Control ctrl = control.FindControl(id);

            if (ctrl == null)
            {
                foreach (Control child in control.Controls)
                {
                    ctrl = FindControlRecursive(child, id);

                    if (ctrl != null) break;
                }
            }
            return ctrl;
        }


    }






}