<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InflowOutflowAnalysis.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.InflowOutflowAnalysis" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
  <style type="text/css">
      .RadPivotGrid .rpgFieldItem {
          margin: 4px;
          /*padding: 5px;
          display: block;*/
          display: inline-block !important;
      }
       
     
    .PivotGridWindow {
           width: 700px !important;
           height: 400px !important;

       }
       
       .reportlbl{
           font-size:small;
           font-family:Helvetica Neue, Helvetica, Arial, sans-serif;
           font-weight:bold;
               
       }  
       div.RadPivotGrid .rpgFieldItem.aggregateCustom1, div.RadPivotGrid .rpgColumnHeader.aggregateCustom1 {
    color: #006699;
}
 
div.RadPivotGrid .rpgFieldItem.aggregateCustom2, div.RadPivotGrid .rpgColumnHeader.aggregateCustom2 {
    color: #006666;
}

.tableCssStyle{
   /*width: 100%;*/
   /*padding: 20px;*/
   /*background-color: #ebebeb;
   border-radius: 4px;*/  
   font-family: verdana, sans-serif;
   font-size: smaller;
   font-weight:bold;
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
    border-color: rgb(204, 204, 204);
    color: #333;
    background-color: #FFF;
    font: 12px "segoe ui",arial,sans-serif;
    border-style: solid;
    border-radius: 5px;
    height: 2.5em;
    /*text-indent: .8em;*/
}
   </style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
        
</head>
<body onload="closeProgressIndicator()">
    <form id="form1" runat="server">
    <div>
                <asp:HiddenField ID="forgeryToken" runat="server"/>
        <%--<div>
            <telerik:RadDropDownList ID="doprdown1" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                 DataValueField="Value" DataTextField="Text" DefaultMessage="Select View..." OnSelectedIndexChanged="setToRadPivot">
            </telerik:RadDropDownList>
        
            <div style="padding:10px;text-align:center;">
                    
                    View Name :&nbsp;<telerik:RadTextBox ID="ViewNameText" runat="server" CausesValidation="true" InputType="Text"/>&nbsp;&nbsp;&nbsp;&nbsp;
                   
                    <telerik:RadButton ID="SaveView" runat="server" OnClick="SaveViewDetails" Text="Save"/>
                  
                </div>
        </div>      style="padding:5px;align-content:center"          --%>
             <table  class="tableCssStyle">
                <tbody>
                    <tr>
                        <td>
                            <telerik:RadDropDownList ID="doprdown1" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                 DataValueField="Value" DataTextField="Text" DefaultMessage="Select Report View..." Width="250" Font-Names="verdana, sans-serif" DropDownWidth="250" OnSelectedIndexChanged="setToRadPivot">
            </telerik:RadDropDownList>
                        </td>
                       
                        <td style="padding-left:2em;">
                             Report View Name :
                        
                            <telerik:RadTextBox ID="ViewNameText" runat="server" Width="250px" CausesValidation="true" InputType="Text"/>
                        </td>  
                         <td style="padding-left:1em;">
                            Is Default View :
                       
                             <asp:CheckBox ID="defaultChekbox" runat="server"  />
                        </td>                     
                        <td style="padding-left:2em;">
                             <telerik:RadButton ID="SaveView" runat="server" Primary="true" OnClick="SaveViewDetails" Text="Save"/>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    
                   
                 <%--   <tr>
                       
                       <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                       
                    </tr>--%>
                </tbody>
            </table>
        <br />
        <div style="float:right" class="tableCssStyle">
            <table>
                <tr>
                     <td style="padding-left:2em;">
                            Classic View : <asp:CheckBox ID="chkClassicViewPivot" OnCheckedChanged="chkClassicViewPivot_CheckedChanged" AutoPostBack="true" Checked="true" runat="server"  />
                        </td>
                        <td style="padding-left:2em;float:right">
                             <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export Excel" 
        OnClick="ButtonExcel_Click" AlternateText="Xlsx" />
                        </td>
                </tr>
            </table>
            </div>

     <%-- <h1>hi</h1>
    <label runat="server" id="lblClient"></label>
        <label runat="server" id="lblConfigItem"></label>--%>
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>
           <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
        <AjaxSettings>
         <telerik:AjaxSetting AjaxControlID="RadPivotGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                     <telerik:AjaxUpdatedControl ControlID="RadHtmlChart1"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
            </AjaxSettings>
               <AjaxSettings>
               <telerik:AjaxSetting AjaxControlID="chkClassicViewPivot">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                  
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
               <AjaxSettings>
               <telerik:AjaxSetting AjaxControlID="doprdown1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                  
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
               <AjaxSettings>
               <telerik:AjaxSetting AjaxControlID="defaultChekbox">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                  
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
               <AjaxSettings>
               <telerik:AjaxSetting AjaxControlID="SaveView">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                  
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
             
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"  EnableSkinTransparency="true" >
    </telerik:RadAjaxLoadingPanel>

        
                 

          <div style="float:right">
        <%-- <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export Excel" 
        OnClick="ButtonExcel_Click" AlternateText="Xlsx" />--%>
              </div>
         <br />
          <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="RadPivotGrid1" />
            </PersistenceSettings>
        </telerik:RadPersistenceManager>
        <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  Font-Names="verdana,sans-serif" AllowPaging="true"  OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
            AllowSorting="true" AllowFiltering="true"     TotalsSettings-RowsSubTotalsPosition="None" TotalsSettings-RowGrandTotalsPosition="Last" TotalsSettings-ColumnsSubTotalsPosition="None"   RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"  EnableConfigurationPanel="true"  ShowFilterHeaderZone="false"  EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"  >
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
            <Fields>
                
                <telerik:PivotGridColumnField DataField="year" >
                </telerik:PivotGridColumnField>
             
              
                
                 <telerik:PivotGridRowField DataField="TicketFlow"   >
                </telerik:PivotGridRowField>
               
                <telerik:PivotGridReportFilterField DataField="weeklabel" Caption="By Week"  CellStyle-Width="1"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="monthname" Caption="By Month" SortOrder="None"  >
                </telerik:PivotGridReportFilterField>
               
                 <telerik:PivotGridReportFilterField DataField="Priority" IsHidden="false">
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Status" IsHidden="false" >
                </telerik:PivotGridReportFilterField>
                  <telerik:PivotGridReportFilterField DataField="Urgency" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                  <telerik:PivotGridReportFilterField DataField="Type" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                   <telerik:PivotGridReportFilterField DataField="Outagetype" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Divisionimpacted" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Businessunitimpacted" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicecategory" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicesubcategory" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicearea" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Regionimpacted" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Locationsimpacted" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Customerdept" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Customerlastname" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Customerfirstname" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Customertitle" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Customersatisfaction" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Impacttype" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Configitemid" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Configtype" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Configitemname" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Environmentimpacted" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Module" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Rootcausetype" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Resolutiontype" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Solutionid" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Deploymentrelease" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Relatedchangeids" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Relatedids" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Relatedproblemids" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Helpdesk" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Assignedtoperson" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Assignedtosupportgroup" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Createdbygroup" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Createdbyperson" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Impactscope" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Modificationnotes" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Esitmatedresolutiondate" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Assigneddatetime" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Createdtime" IsHidden="true" Caption="Created Time" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Createdweekday" IsHidden="true" Caption="Created Week Day" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Createdweeknum" IsHidden="true" Caption="Created Week Num" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Createdmonth" IsHidden="true" Caption="Created Month" SortOrder="None" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Createdyear" IsHidden="true" Caption="Created Year" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Acknowlededbygroup" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Acknowlededbyperson" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
           
                <telerik:PivotGridReportFilterField DataField="Acknowledgedtime" IsHidden="true" Caption="Acknowledged Time" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Acknowledgedweekday" IsHidden="true" Caption="Acknowledged Week Day" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Acknowledgedweeknum" IsHidden="true" Caption="Acknowledged Week Num" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Acknowledgedmonth" IsHidden="true" Caption="Acknowledged Month" SortOrder="None" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Acknowledgedyear" IsHidden="true" Caption="Acknowledged Year" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Resolvedbygroup" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Resolvedbyperson" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Resolvedbyvendor" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Resolutionefforthours" Caption="Resolution hours"  IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Resolutiontime" Caption="Total Resolution effort"  IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Resolutionweekday" IsHidden="true" Caption="Resolution Week Day" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Resolutionweeknum" IsHidden="true" Caption="Resolution Week Num">
                </telerik:PivotGridReportFilterField>
                   <telerik:PivotGridReportFilterField DataField="Resolutionyear" IsHidden="true" Caption="Resolution Year" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Resolutionmonth" IsHidden="true" Caption="Resolution Month" SortOrder="None" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicerestoredbygroup" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicerestoredbyperson" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicerestoredtime"  IsHidden="true" Caption="Service Restored Time" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicerestoredweekday" IsHidden="true" Caption="Service Restored Week Day" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicerestoredweeknum" IsHidden="true" Caption="Service Restored Week Num">
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicerestoredmonth" IsHidden="true" Caption="Service Restored Month" SortOrder="None" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicerestoredyear" IsHidden="true" Caption="Service Restored Year" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Assignedbygroup" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Assignedbyperson" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Esitmatedresponsetimebygroup" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
             
         <%--        <telerik:PivotGridReportFilterField DataField="Waivedindicator" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Totalsuspenddurationassignedtoack" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Totalsuspenddurationacktoer" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Totalsuspenddurationertoresolution" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
                 <telerik:PivotGridReportFilterField DataField="Resolvedindicator" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="IsNTTGroup" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="SupportGroupType" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Scheduledchangecount" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Company" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Site" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="TicketId" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Title" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Description" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Datasourcenotes" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Resolutiondescription" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Rootcausedescription" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Assigneecompany" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Assigneeorganization" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
  <%--              <telerik:PivotGridReportFilterField DataField="Estimatedworkeffort" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
                 <telerik:PivotGridReportFilterField DataField="Esitmatedresponsedatetime" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
         <%--       <telerik:PivotGridReportFilterField DataField="Actualworkeffort" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
                <telerik:PivotGridReportFilterField DataField="Recyclecount" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ConfigItemCategory" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
          
                <telerik:PivotGridReportFilterField DataField="ClosureCategory" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Severity" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="WorkGroupLevel" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Datasource" IsHidden="true" >
                </telerik:PivotGridReportFilterField>

                 <telerik:PivotGridReportFilterField DataField="Statusreason" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="AffectedItemName" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="SLACostPerTicket" IsHidden="true" Caption="Total Ticket Cost" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="INResolutiontime" IsHidden="true" Caption="Total Resolution effort" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="INCostPerTicket" IsHidden="true" Caption="Total Ticket Cost" >
                </telerik:PivotGridReportFilterField>
              
          
                <telerik:PivotGridReportFilterField DataField="Closeddatetime"  DataFormatString="{0:d}"  IsHidden="true"  >
                </telerik:PivotGridReportFilterField>     
                <telerik:PivotGridReportFilterField DataField="ResolutionDate"  DataFormatString="{0:d}"  IsHidden="true"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Lastupdatedbyperson" IsHidden="true" Caption="Last updated by" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Lastupdateddatetime" IsHidden="true" Caption="Last updated" >
                </telerik:PivotGridReportFilterField>
                
                <telerik:PivotGridReportFilterField DataField="AccountName" IsHidden="false" Caption="Account Name" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ContractName" IsHidden="false" Caption="Contract Name" >
                </telerik:PivotGridReportFilterField>

            
                <telerik:PivotGridAggregateField DataField="Incidentid" ZoneIndex="0"   Caption="Total Tickets"    Aggregate="Count"  >
                  
                </telerik:PivotGridAggregateField>
               <%-- <telerik:PivotGridAggregateField DataField="Incidentid" ZoneIndex="1"  Caption="Total Percentage"    Aggregate="Sum"  >
                  <TotalFormat TotalFunction="PercentOfGrandTotal"  />
                </telerik:PivotGridAggregateField>--%>
         
                
            
            </Fields>
           <ConfigurationPanelSettings Position="Left" DefaultDeferedLayoutUpdate="true" />
          <%--<ClientSettings EnableFieldsDragDrop="true">
              
        
                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>--%>
        </telerik:RadPivotGrid>
       
        <br />
                
         <telerik:RadHtmlChart ID="RadHtmlChart1" Height="350px"  runat="server" Visible="true" Legend-Appearance-Position="Bottom"
         PlotArea-XAxis-LabelsAppearance-RotationAngle="45" PlotArea-YAxis-MinValue="0" PlotArea-YAxis-Type="Numeric">
   
                       <Pan Enabled="true" />
         <Zoom Enabled="true">
                <MouseWheel Enabled="true" />
                <Selection Enabled="true" ModifierKey="Shift" />
            </Zoom>
    </telerik:RadHtmlChart>

         <div  class="tableCssStyle" style="float:right">
             
                                          <telerik:RadButton RenderMode="Lightweight" runat="server" OnClientClicked="exportPDF" Text="Export to PDF" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                           <telerik:RadButton RenderMode="Lightweight" runat="server"  OnClientClicked="exportImage" Text="Export to Image" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
      
                         <telerik:RadClientExportManager runat="server" ID="RadClientExportManager1">
        </telerik:RadClientExportManager>
            </div>
    </div>
    </form>
</body>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <script type="text/javascript">
        //var $ = $telerik.$;

         function exportPDF() {
           $find('<%=RadClientExportManager1.ClientID%>').exportPDF($("#form1"));          
        }
         function exportImage() {
       $find('<%=RadClientExportManager1.ClientID%>').exportImage($("#form1"));    
        }
      
    </script>
            </telerik:RadCodeBlock>
</html>

