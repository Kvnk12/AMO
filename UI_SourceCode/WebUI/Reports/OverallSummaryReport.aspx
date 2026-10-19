<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="OverallSummaryReport.aspx.cs" Inherits="WebUI.Reports.OverallSummaryReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="headercontent" ContentPlaceHolderID="headertitle" runat="server">
  <style type="text/css">
   .RadPivotGrid .rpgFieldItem {
          margin: 3px;
          padding: 5px;
          display: block;
          display: inline-block !important;
          font-size: smaller;
      }
    .PivotGridWindow {
           width: 700px !important;
           height: 400px !important;
       }
       
       .reportlbl{
           font-size:small;
           font-family:Helvetica Neue, Helvetica, Arial, sans-serif;
           font-weight:bold;
           font-size: smaller;   
       }  
      div.RadPivotGrid .rpgFieldItem.aggregateCustom1, div.RadPivotGrid .rpgColumnHeader.aggregateCustom1 {
    color: #006699;
}
 
div.RadPivotGrid .rpgFieldItem.aggregateCustom2, div.RadPivotGrid .rpgColumnHeader.aggregateCustom2 {
    color: #006666;
}

#form1 .RadDropDownList .rddlEmptyMessage,#form1 .RadDropDownList .rddlDefaultMessage {
    opacity: 1;
    font-style: normal;
}

       div.RadPivotGrid .rpgDataCell{
           text-align:right;
       }
html body .RadInput_Default .riTextBox, html body .RadInputMgr_Default
{
    /*border-color: rgb(204, 204, 204);
    color: #333;*/
    background-color: #FFF;
    font: 12px "segoe ui",arial,sans-serif;
    border-style: solid;
    border-radius: 5px;
    height: 2.5em;
    /*text-indent: .8em;*/
}
   
   </style>


     <span> <img alt="tool logo" src='<%=Page.ResolveUrl("~/Content/images/incident_navigator_icon.png")%>'>Incident Navigator</span>
</asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <form id="form1" runat="server">
         <asp:HiddenField ID="forgeryToken" runat="server"/>
  <div class="content-wrapper">                  
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gridOverallSummary">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridOverallSummary" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
        <telerik:RadGrid ID="gridOverallSummary" OnNeedDataSource="gridOverallSummary_NeedDataSource" OnItemDataBound="gridOverallSummary_ItemDataBound" AutoGenerateColumns="false" ClientSettings-AllowExpandCollapse="false" ClientSettings-AllowGroupExpandCollapse="false" MasterTableView-DataKeyNames="ID" runat="server">
             <ExportSettings ExportOnlyData="true" HideStructureColumns="true" OpenInNewWindow="true" IgnorePaging="true"></ExportSettings>
            <MasterTableView>
                 <GroupByExpressions>
                    <telerik:GridGroupByExpression>
                      <SelectFields>
                            <telerik:GridGroupByField HeaderText=""  FieldName="CustomMetricName" 
                               
                               ></telerik:GridGroupByField>
                        </SelectFields>
                        <GroupByFields>
                            <telerik:GridGroupByField FieldName="CustomMetricName"  ></telerik:GridGroupByField>
                        </GroupByFields>
                    </telerik:GridGroupByExpression>
                    
                </GroupByExpressions>
                                 <CommandItemSettings ShowExportToCsvButton="true" ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToWordButton="true" />

                <Columns>
                      <%-- <telerik:GridBoundColumn  HeaderText="Performance Category" 
                        DataField="CustomMetricName" Groupable="true"/>--%>
                    <telerik:GridBoundColumn  HeaderText="Performance Category" 
                        DataField="CustomSLAName"/>
                    <telerik:GridBoundColumn  HeaderText="Target" UniqueName="ComplianceTarget" 
                        DataField="ComplianceTarget"/>
                    <telerik:GridBoundColumn  HeaderText="Current"   UniqueName="Percentage" 
                        DataField="Percentage"/>
                </Columns>
            </MasterTableView>
             <GroupingSettings ShowUnGroupButton="false" ></GroupingSettings>
        </telerik:RadGrid>
       

    </div>
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>

    </form>
  </asp:Content>
