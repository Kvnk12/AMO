<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.WebForm1" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns='http://www.w3.org/1999/xhtml'>
<head runat="server">
    <title>Telerik ASP.NET Example</title>
   <%-- <link href="styles.css" rel="stylesheet" />--%>
     <link rel="stylesheet" href="https://dynamoportal.net/AMOPortalSharedService/Content/css/custom-styles.css" />
</head>
<body>
    <form id="form1" runat="server">
                <asp:HiddenField ID="forgeryToken" runat="server"/>
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
   
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"  EnableSkinTransparency="true" >
   
    </telerik:RadAjaxLoadingPanel>
     
  <%--  <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"   >
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadPivotGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
      
          
        </AjaxSettings>
    </telerik:RadAjaxManager>--%>
  <%--  <div class="PivotGridWrapper">--%>
        <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <telerik:RadPivotGrid ID="RadPivotGrid2" RenderMode="Lightweight" AllowFiltering="true" AllowSorting="true" 
            AllowPaging="true"
            PageSize="20" runat="server" EnableZoneContextMenu="true"
           >
            <PagerStyle AlwaysVisible="true" />
             <OlapSettings ProviderType="Xmla">
                <AdomdConnectionSettings Cube="IncidentAnalyticalCube" DataBase="NextGenToolIncidentCube" 
                    ConnectionString="Data Source=http://192.43.232.177/OLAP/msmdpump.dll;Catalog=NextGenToolIncidentCube"></AdomdConnectionSettings>
                <XmlaConnectionSettings ServerAddress="http://10.100.100.12/OLAP/msmdpump.dll"
                    Cube="CubeClient1" DataBase="NextGenToolIncidentCube" ></XmlaConnectionSettings>
            </OlapSettings>
            <Fields>
                
                 <%--<telerik:PivotGridReportFilterField DataField="[Measures].[Assignedtoperson]"  IsHidden="true"   UniqueName="Assignedtoperson">
                </telerik:PivotGridReportFilterField>
                  <telerik:PivotGridReportFilterField DataField="[Measures].[AffectedItemName]"  IsHidden="true"   UniqueName="AffectedItemName">
                </telerik:PivotGridReportFilterField>
                   <telerik:PivotGridReportFilterField DataField="[Measures].[Totalsuspenddurationertoresolution]"  IsHidden="true"   UniqueName="Totalsuspenddurationertoresolution">
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="[Measures].[Totalsuspenddurationassignedtoack]"  IsHidden="true"   UniqueName="Totalsuspenddurationassignedtoack">
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="[Measures].[Totalsuspenddurationacktoer]"  IsHidden="true"   UniqueName="Totalsuspenddurationacktoer">
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="[Measures].[Scheduledchangecount]"  IsHidden="true"   UniqueName="Scheduledchangecount">
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="[Measures].[Estimatedworkeffort]"  IsHidden="true"   UniqueName="Estimatedworkeffort">
                </telerik:PivotGridReportFilterField>--%>
                
                <telerik:PivotGridRowField DataField="[Incident 1].[Priority]"     UniqueName="Priority">
                </telerik:PivotGridRowField>
                <telerik:PivotGridColumnField DataField="[Incident 1].[Status]"     UniqueName="Status">
                </telerik:PivotGridColumnField>
                <telerik:PivotGridAggregateField DataField="[Measures].[Incident Count]" Caption="Ticket Count"
                    UniqueName="SalesAmount">
                </telerik:PivotGridAggregateField>
                 <telerik:PivotGridAggregateField DataField="[Measures].[Actualworkeffort]" ZoneIndex="1"      UniqueName="Actualworkeffort">
                </telerik:PivotGridAggregateField>
                 <telerik:PivotGridAggregateField DataField="[Measures].[Recyclecount]" ZoneIndex="0"    UniqueName="Recyclecount">
                </telerik:PivotGridAggregateField>
            
         
            </Fields>
                <ClientSettings EnableFieldsDragDrop="true">
                <Scrolling AllowVerticalScroll="false" ScrollHeight="600px" />
            </ClientSettings>
            </telerik:RadPivotGrid>
            
        </telerik:RadAjaxPanel>
        
   <%-- </div>--%>
   <%-- <telerik:ConfiguratorPanel runat="server" ID="ConfiguratorPanel1">
        <Views>
            <telerik:View>
                <telerik:RadioButtonList ID="rdlProviderType" runat="server" AutoPostBack="true" 
                    Label="Set the olap data provider"
                    OnSelectedIndexChanged="rdlProviderType_SelectedIndexChanged">
                    <asp:ListItem Text="Xmla" Value="Xmla" Selected="True">
                    </asp:ListItem>
                    <asp:ListItem Text="Adomd" Value="Adomd"></asp:ListItem>
                </telerik:RadioButtonList>
            </telerik:View>
        </Views>
    </telerik:ConfiguratorPanel>--%>
    </form>
</body>
</html>