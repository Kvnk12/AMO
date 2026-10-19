<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OLAPAnalysis.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.OLAPAnalysis" %>
<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.QuickStart" %>--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadPivotGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ConfiguratorPanel1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="ConfiguratorPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="PivotGridWrapper">
        <telerik:RadPivotGrid RenderMode="Lightweight" AllowFiltering="true" AllowSorting="true" ID="RadPivotGrid1"
            RowTableLayout="Tabular" ShowFilterHeaderZone="true" FieldsPopupSettings-AggregateFieldsMinCount="2" AllowPaging="true"
            PageSize="20" runat="server" EnableConfigurationPanel="false" EnableZoneContextMenu="true"
            AggregatesPosition="Rows" OnInit="RadPivotGrid1_Init">
            <PagerStyle AlwaysVisible="true" />
           <OlapSettings>
            </OlapSettings>
            <RowHeaderCellStyle Width="100px" />
            <Fields>
                <telerik:PivotGridReportFilterField DataField="[Customer].[City]" Caption="City" UniqueName="City" IsHidden="false" ></telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="[Customer].[State-Province]" Caption="State" UniqueName="State" IsHidden="false" ></telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="[Customer].[Country]" CellStyle-Width="0px" CellStyle-Height="0px"  Caption=" " UniqueName="Country" IsHidden="false" ></telerik:PivotGridReportFilterField>
                <telerik:PivotGridRowField DataField="[Product].[Category]" Caption="Category" UniqueName="Category">
                </telerik:PivotGridRowField>
                <telerik:PivotGridColumnField DataField="[Date].[Fiscal Year]" Caption="Year" UniqueName="FiscalYear">
                </telerik:PivotGridColumnField>
                <telerik:PivotGridAggregateField DataField="[Measures].[Internet Sales Amount]" Caption="Internet Sales Amount"
                    UniqueName="SalesAmount">
                </telerik:PivotGridAggregateField>
                <telerik:PivotGridAggregateField DataField="[Measures].[Internet Revenue Goal]" Caption="Revenue Goal"
                    UniqueName="RevenueGoal">
                </telerik:PivotGridAggregateField>
                <telerik:PivotGridAggregateField DataField="[Measures].[Internet Revenue Status]"
                    Caption="Revenue Status" UniqueName="SalesStatus">
                </telerik:PivotGridAggregateField>
                <telerik:PivotGridAggregateField DataField="[Measures].[Internet Revenue Trend]"
                    Caption="Revenue Trend" UniqueName="SalesTrend">
                </telerik:PivotGridAggregateField>
            </Fields>
            <%--<ConfigurationPanelSettings Position="Left" LayoutType="OneByFour" DefaultDeferedLayoutUpdate="true" />--%>
<%--            <ClientSettings>
                <Scrolling AllowVerticalScroll="false" ScrollHeight="600px" />
            </ClientSettings>--%>
            <ClientSettings EnableFieldsDragDrop="true">
                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>

        </telerik:RadPivotGrid>
    </div>
    </form>
</body>
  </html>

