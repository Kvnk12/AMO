<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.WebForm2" %>

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
       div.RadPivotGrid .rpgDataCell{
           text-align:right;
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
      .auto-style1 {
          width: 202px;
      }

      
                .progressIndicatorStyle {
                    /*background-color:gray;*/
                    width: 200px;
                    height: 200px;
                    /*border:solid 2px white;*/
                    top: 25%;
                    left: 40%;
                    z-index: 99999;
                    position: absolute;
                    display: block;
                }

      .auto-style2 {
          width: 351px;
      }
      
        @media print {
  body * {
    visibility: hidden;
  }
  #exportSectionID, #exportSectionID * {
    visibility: visible;
  }
  #exportSectionID {
    position: absolute;
    left: 0;
    top: 0;
  }
}
   
   </style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
        
</head>

<body  onload="closeProgressIndicator()">
    
     
    
    <form id="form1" runat="server" >
   <div>
     
             <table  class="tableCssStyle">
                <tbody>

                    <tr>
                        <td class="auto-style1">
                            <telerik:RadDropDownList ID="doprdown1" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                 DataValueField="Value" DataTextField="Text" DefaultMessage="Select Report View..." Width="250" Font-Names="verdana, sans-serif" DropDownWidth="250"  OnSelectedIndexChanged="setToRadPivot">
            </telerik:RadDropDownList>
                            

                        </td>
                       
                        <td style="padding-left:2em;" class="auto-style2">
                             Report View Name :
                        
                            <telerik:RadTextBox ID="ViewNameText" runat="server" Width="250px" CausesValidation="true" InputType="Text"/>
                        </td>  
                        <td style="padding-left:1em;">
                            Make Public :
                       
                             <asp:CheckBox ID="PublicChekbox" runat="server"  />
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

        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>
           <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest" >
        <AjaxSettings>
         <telerik:AjaxSetting AjaxControlID="RadPivotGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
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
               <telerik:AjaxSetting AjaxControlID="SaveView">
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
               <telerik:AjaxSetting AjaxControlID="form1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"  EnableSkinTransparency="true">
    </telerik:RadAjaxLoadingPanel>
   
          <div style="float:right">
        <%-- <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export Excel" 
        OnClick="ButtonExcel_Click" AlternateText="Xlsx" />--%>
              </div>
         <br />

        <div id="exportSectionID">
            <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  Font-Names="verdana,sans-serif" AllowPaging="true"  OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
               OnPageIndexChanged="PivotGridPageChange" OnPageSizeChanged="PivotGridPageSizeChange"  AllowSorting="true" AllowFiltering="true" PageSize="50" TotalsSettings-RowsSubTotalsPosition="None" TotalsSettings-RowGrandTotalsPosition="Last" TotalsSettings-ColumnsSubTotalsPosition="None"   RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"   ShowFilterHeaderZone="true"  EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"  >
            <PagerStyle Mode="NumericPages" AlwaysVisible="true"/>
                
            <Fields>
                
                <telerik:PivotGridColumnField DataField="Status">
                </telerik:PivotGridColumnField>
                <telerik:PivotGridRowField DataField="Priority">
                </telerik:PivotGridRowField>
                  <telerik:PivotGridReportFilterField DataField="Urgency" IsHidden="true">
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
                <%--<telerik:PivotGridReportFilterField DataField="Modificationnotes" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
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
                  <telerik:PivotGridReportFilterField DataField="ResponseTimeMetSLA" IsHidden="true" Caption="Resolution Met(SLA)" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Resptargettime" IsHidden="true" Caption="Response target time" >
                </telerik:PivotGridReportFilterField>



                <telerik:PivotGridReportFilterField DataField="Modificationnotes" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ResolutionExceptionReason" IsHidden="true" Caption="Resolution Exception Reason" SortOrder="None" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ResponseExceptionReason" IsHidden="true" Caption="Response Exception Reason" SortOrder="None" >
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
             
                 <telerik:PivotGridReportFilterField DataField="Waivedindicator" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Totalsuspenddurationassignedtoack" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Totalsuspenddurationacktoer" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Totalsuspenddurationertoresolution" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
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
                <%--<telerik:PivotGridReportFilterField DataField="Description" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
                     <telerik:PivotGridReportFilterField DataField="Datasourcenotes" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
<%--                 <telerik:PivotGridReportFilterField DataField="Resolutiondescription" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
                 <telerik:PivotGridReportFilterField DataField="Rootcausedescription" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Assigneecompany" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Assigneeorganization" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Estimatedworkeffort" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Esitmatedresponsedatetime" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Actualworkeffort" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Recyclecount" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ConfigItemCategory" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
          
                <telerik:PivotGridReportFilterField DataField="ClosureCategory" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Severity" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="WorkGroupLevel" IsHidden="true">
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
              
          
                <telerik:PivotGridReportFilterField DataField="Closeddatetime"  DataFormatString="{0:d}"  IsHidden="true" >
                </telerik:PivotGridReportFilterField>     
                <telerik:PivotGridReportFilterField DataField="ResolutionDate"  DataFormatString="{0:d}"  IsHidden="true"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Lastupdatedbyperson" IsHidden="true" Caption="Last updated by" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Lastupdateddatetime" IsHidden="true" Caption="Last updated" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Nodename" IsHidden="true" Caption="Node Name" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Createdtimezone" IsHidden="true" Caption="Created time zone" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Acknowledgedtimezone" IsHidden="true" Caption="Acknowledged time zone" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Resolvedtimezone" IsHidden="true" Caption="Resolved time zone" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Servicerestoredtimezone" IsHidden="true" Caption="Service restored time zone" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ScheduledstartdateTime" IsHidden="true" Caption="Scheduled Start Date" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ScheduledenddateTime" IsHidden="true" Caption="Scheduled Start Time" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Scheduledchangecount" IsHidden="true" Caption="Scheduled Change Count" >
                </telerik:PivotGridReportFilterField>
				<telerik:PivotGridReportFilterField DataField="InProgressdatetime" IsHidden="true" Caption="InProgress Date" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ActualstartdateTime" IsHidden="true" Caption="Actual start Date" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="AccountName" IsHidden="false" Caption="Account Name" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ContractName" IsHidden="false" Caption="Contract Name" >
                </telerik:PivotGridReportFilterField>
				
                <%--added by DP--%>
 <telerik:PivotGridReportFilterField DataField="Active" IsHidden="true" Caption="Active"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Activityduedatetime" IsHidden="true"  Caption="Activityduedatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="ActualenddateTime" IsHidden="true"  Caption="ActualenddateTime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Additionalassigneelist" IsHidden="true"  Caption="Additionalassigneelist" ></telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Approval" IsHidden="true"  Caption="Approval"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Approvalhistory" IsHidden="true"  Caption="Approvalhistory"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Approvalsetdatetime" IsHidden="true"  Caption="Approvalsetdatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Businessduration" IsHidden="true"  Caption="Businessduration"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Closenotes" IsHidden="true"  Caption="Closenotes"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Closedby" IsHidden="true"  Caption="Closedby"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Commentsworknotes" IsHidden="true"  Caption="Commentsworknotes"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Correlationdisplay" IsHidden="true"  Caption="Correlationdisplay"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="CorrelationID" IsHidden="true"  Caption="CorrelationID"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Deliveryplan" IsHidden="true"  Caption="Deliveryplan"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Deliverytask" IsHidden="true"  Caption="Deliverytask"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Domain" IsHidden="true"  Caption="Domain"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Domainpath" IsHidden="true"  Caption="Domainpath"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Escalation" IsHidden="true"  Caption="Escalation"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Followupdatetime" IsHidden="true"  Caption="Followupdatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Grouplist" IsHidden="true"  Caption="Grouplist"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Knowledge" IsHidden="true"  Caption="Knowledge"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Location" IsHidden="true"  Caption="Location"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="MadeSLA" IsHidden="true"  Caption="MadeSLA"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Openeddatetime" IsHidden="true"  Caption="Openeddatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Openedby" IsHidden="true"  Caption="Openedby"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Order" IsHidden="true"  Caption="Order"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="ParentticketID" IsHidden="true"  Caption="ParentticketID"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Reassignmentcount" IsHidden="true"  Caption="Reassignmentcount"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="SLAduedatetime" IsHidden="true"  Caption="SLAduedatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Tags" IsHidden="true"  Caption="Tags"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Timeworked" IsHidden="true"  Caption="Timeworked"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Updatescount" IsHidden="true"  Caption="Updatescount"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Uponapprovalstep" IsHidden="true"  Caption="Uponapprovalstep"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Uponrejectstep" IsHidden="true"  Caption="Uponrejectstep"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Userinput" IsHidden="true"  Caption="Userinput"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Watchlist" IsHidden="true"  Caption="Watchlist"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Worknotes" IsHidden="true"  Caption="Worknotes"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Worknoteslist" IsHidden="true"  Caption="Worknoteslist"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Businessresolvetime" IsHidden="true"  Caption="Businessresolvetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Caller" IsHidden="true"  Caption="Caller"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Causedbychange" IsHidden="true"  Caption="Causedbychange"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="ChildIncidentscount" IsHidden="true"  Caption="ChildIncidentscount"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Incidentstate" IsHidden="true"  Caption="Incidentstate"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Lastreopenedatdatetime" IsHidden="true"  Caption="Lastreopenedatdatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Lastreopenedby" IsHidden="true"  Caption="Lastreopenedby"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Notify" IsHidden="true"  Caption="Notify"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="ParentIncidentID" IsHidden="true"  Caption="ParentIncidentID"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="TotalResolvetime" IsHidden="true"  Caption="TotalResolvetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Knownerror" IsHidden="true"  Caption="Knownerror"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Majorproblem" IsHidden="true"  Caption="Majorproblem"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Problemstate" IsHidden="true"  Caption="Problemstate"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="RelatedIncidentscount" IsHidden="true"  Caption="RelatedIncidentscount"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Workaround" IsHidden="true" Caption="Workaround"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Backoutplan" IsHidden="true"  Caption="Backoutplan"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="CABdatetime" IsHidden="true"  Caption="CABdatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="CABdelegate" IsHidden="true"  Caption="CABdelegate"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="CABrecommendation" IsHidden="true"  Caption="CABrecommendation"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="CABrequired" IsHidden="true"  Caption="CABrequired"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Changeplan" IsHidden="true"  Caption="Changeplan"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Closecode" IsHidden="true"  Caption="Closecode"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Conflictlastrundatetime" IsHidden="true"  Caption="Conflictlastrundatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Conflictstatus" IsHidden="true"  Caption="Conflictstatus"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Implementationplan" IsHidden="true"  Caption="Implementationplan"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Onhold" IsHidden="true"  Caption="Onhold"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="OnHoldChangeTasks" IsHidden="true"  Caption="OnHoldChangeTasks"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Outsidemaintenanceschedule" IsHidden="true"  Caption="Outsidemaintenanceschedule"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Phase" IsHidden="true"  Caption="Phase"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Phasestate" IsHidden="true"  Caption="Phasestate" ></telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Productionsystem" IsHidden="true"  Caption="Productionsystem"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Reason" IsHidden="true"  Caption="Reason"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Requestedby" IsHidden="true"  Caption="Requestedby"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Requestedbydatetime" IsHidden="true"  Caption="Requestedbydatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Reviewcomments" IsHidden="true"  Caption="Reviewcomments" ></telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Reviewdatetime" IsHidden="true"  Caption="Reviewdatetime"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Reviewstatus" IsHidden="true"  Caption="Reviewstatus"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Risk" IsHidden="true"  Caption="Risk"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Changerequestscope" IsHidden="true"  Caption="Changerequestscope"> </telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Changerequesttype" IsHidden="true"  Caption="Changerequesttype" ></telerik:PivotGridReportFilterField>
 <telerik:PivotGridReportFilterField DataField="Changerequesttype" IsHidden="true"  Caption="Changerequesttype"> </telerik:PivotGridReportFilterField>
<telerik:PivotGridReportFilterField DataField="Tower" IsHidden="true"  Caption="Tower"> </telerik:PivotGridReportFilterField>
<%--end--%>

                <telerik:PivotGridReportFilterField DataField="CreatedDate" IsHidden="false" Caption="Created Date" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ResolvedDate" IsHidden="false" Caption="Resolved Date" >
                </telerik:PivotGridReportFilterField>
<%--                 <telerik:PivotGridReportFilterField DataField="Closeddate" IsHidden="false" Caption="Closed Day" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Lastupdateddate" IsHidden="false" Caption="Last Updated Day" >
                </telerik:PivotGridReportFilterField>
               <telerik:PivotGridReportFilterField DataField="AcknowledgedDate" IsHidden="false" Caption="Acknowledge Day" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Assigneddate" IsHidden="false" Caption="Assigned Day" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Servicerestoreddate" IsHidden="false" Caption="Service Stored Day" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Activityduedate" IsHidden="false" Caption="Activity Due Day" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Requestedbydate" IsHidden="false" Caption="Requested Day" >
                </telerik:PivotGridReportFilterField>--%>


                 <%--<telerik:PivotGridReportFilterField DataField="Impactdescription" IsHidden="true" Caption="Impact Description" >
                </telerik:PivotGridReportFilterField>--%>

                <telerik:PivotGridAggregateField DataField="Incidentid" ZoneIndex="0" Caption="Total Tickets"    Aggregate="Count" >
                  
                </telerik:PivotGridAggregateField>
               <%-- <telerik:PivotGridAggregateField DataField="Incidentid" ZoneIndex="1"  Caption="Total Percentage"    Aggregate="Sum"  >
                  <TotalFormat TotalFunction="PercentOfGrandTotal"  />
                </telerik:PivotGridAggregateField>--%>
         
                
            
            </Fields>
           
          <ClientSettings EnableFieldsDragDrop="true">
              
        
                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>
        </telerik:RadPivotGrid>
        </div>
       
        <br />
        
                                     <div class="tableCssStyle" style="float:right">
            
                                          <telerik:RadButton RenderMode="Lightweight" runat="server" OnClientClicked="exportPDF" Text="Export to PDF" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                           <telerik:RadButton RenderMode="Lightweight" runat="server"  OnClientClicked="exportImage" Text="Export to Image" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
      
                         <telerik:RadClientExportManager runat="server" ID="RadClientExportManager1">
        </telerik:RadClientExportManager>
            </div>

    </div>
    </form>
    <%--<div id="progressIndicator" class="progressIndicatorStyle">
                     <img src="../../../../Content/images/reload.gif" />
        </div>--%>
</body>
<%--     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server" OnDataBinding="RadTextBox1_TextChanged" OnLoad="TextBox2_TextChanged">--%>
     <script type="text/javascript">
        var $ = $telerik.$;
      //RadPivotGrid1 - ID - exportSectionID

         //function exportPDF() {
         //   var divElements = document.getElementById('exportSectionID').innerHTML;
         //   var oldPage = document.body.innerHTML;
         //   document.body.innerHTML = 
         //   "<html><head><title></title></head><body>" + 
         //        divElements + "</body>";

         //   window.print();
         //   document.body.innerHTML = oldPage;
         //}

         //window.onafterprint = function(){
         //   window.location.reload(true);
         //}



         function exportPDF() {
             //document.getElementById('exportSectionID').contentWindow.print();
             window.print();
         }

        <%--function exportPDF() {
            $find('<%=RadClientExportManager1.ClientID%>').exportPDF($("#form1"));
        }--%>
         function exportImage() {
             $find('<%=RadClientExportManager1.ClientID%>').exportImage($("#form1"));
         }
         
         //function closeProgressIndicator() {
         //    $("#progressIndicator").hide();
         //}
    </script>


           <%-- </telerik:RadCodeBlock>--%>
</html>

