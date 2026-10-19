<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master"  AutoEventWireup="true" CodeBehind="ClusterAnalysis.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.ClusterAnalysis" %>

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
    /*Paramters Styling | Start*/

.parameter-slider.open {
    transform: translateX(0);
    z-index: 9 !important;
}
.parameter-slider {
    width: 260px !important;
    overflow-x: hidden;
    overflow-y: auto;
    z-index: 99999 !important;
}


.breadcrumb {
  padding: 8px 15px;
  margin-bottom: 5px;
  list-style: none;
  background-color: #f5f5f5;
  border-radius: 4px;
  margin:0;
  font-size:13px;
}

.breadcrumb > li {
  display: inline-block;
}

.breadcrumb > li + li:before {
  padding: 0 5px;
  color: #cccccc;
  content: "/\00a0";
}

.breadcrumb > .active {
  color: #999999;
}

   </style>

<span> <img alt="tool logo" src='<%=Page.ResolveUrl("~/Content/images/incident_navigator_icon.png")%>'>Incident Navigator</span>
</asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <form id="form1" runat="server">
 
                <asp:HiddenField ID="forgeryToken" runat="server"/>
         <div class="content-main dark-bg">
             <div class="clear"></div>
            <ol class="breadcrumb">
                <li>Incident Navigator</li>
                <li class="active">Cluster Analysis</li>
            </ol>
             <div class="content-wrapper">
                <div class="parameter-section" style="width:100%">
  <div class="action-items">
                        <div class="export-wrapper">
                            <span class="export-label">Export - </span>
                            <telerik:RadButton RenderMode="Lightweight" runat="server" ID="btnExportpdf" OnClientClicked="exportPDF" CssClass="btnExportpdf" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                            <telerik:RadButton RenderMode="Lightweight" runat="server" ID="btnExportimage" OnClientClicked="exportImage" CssClass="btnExportimage" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                            <telerik:RadButton RenderMode="Lightweight" ID="btnExportExcel" CssClass="btnExportExcel" runat="server"
                                            OnClick="ButtonExcel_Click" AlternateText="Xlsx" />
                            <telerik:RadClientExportManager runat="server" ID="RadClientExportManager1">
                            </telerik:RadClientExportManager>
                        </div>
                        <span class="parameter-filter-icon"></span>
                        <div class="parameter-slider-overlay"></div>
                    </div>

                        <div class="parameter-slider">
                            <span class="close-parameters"></span>
                            <div id="invoiceIdSelector" class="parameter-list-main">
                                <table>
                                    <tr>
                                        <td colspan="3">
                                           <telerik:RadDropDownList ID="doprdown1" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                                               DataValueField="Value" DataTextField="Text" DefaultMessage="Select Report View..." Width="230" Font-Names="verdana, sans-serif" DropDownWidth="250" OnSelectedIndexChanged="setToRadPivot">
                                            </telerik:RadDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td style="padding-top:10px"  colspan="3">
                                             Report View Name :
                                           <telerik:RadTextBox ID="ViewNameText" runat="server" Width="230px" CausesValidation="true" InputType="Text"/>
                                       </td>
                                    </tr>
                                    <tr class="button-group">
                                        <td class="parameter-slider-td">
                                            <span class="td-checkbox">
                                             <asp:CheckBox ID="defaultChekbox" runat="server"/>
                                            <label for="PublicChekbox"> Is Default View :</label>
                                           </span>
                                        </td> 
                                    </tr>
                                    <tr class="button-group">
                                        <td class="parameter-slider-td">
                                            <span class="td-checkbox">
                                            <asp:CheckBox ID="chkClassicViewPivot" OnCheckedChanged="chkClassicViewPivot_CheckedChanged" AutoPostBack="true" Checked="true" runat="server"  />
                                            <label for="PublicChekbox">  Classic View : </label>
                                           </span>
                                            <telerik:RadButton ID="SaveView" runat="server" Primary="true" OnClick="SaveViewDetails" Text="Save"/>
                                        </td>
                                    </tr>
                                  
                                </table>
                                <ul>
                                </ul>
                            </div>
                        </div>

                    
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
         <br />
    <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="RadPivotGrid1" />
            </PersistenceSettings>
        </telerik:RadPersistenceManager>
                     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">

        <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller" 
            Font-Names="verdana,sans-serif" AllowPaging="true"  OnCellDataBound="RadPivotGrid1_CellDataBound" 
            OnNeedDataSource="RadPivotGrid1_NeedDataSource"
            AllowSorting="false" AllowFiltering="true"     TotalsSettings-RowsSubTotalsPosition="None" 
            TotalsSettings-RowGrandTotalsPosition="None" TotalsSettings-ColumnsSubTotalsPosition="None"  
            RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"    AggregatesPosition="Rows"
            EnableConfigurationPanel="false"  ShowFilterHeaderZone="true"  EnableZoneContextMenu="true" 
            OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"  RowHeaderZoneText=" " >
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="false" />
            <Fields>
                
                <telerik:PivotGridColumnField DataField="Status" >
                </telerik:PivotGridColumnField>
             
                 <telerik:PivotGridRowField DataField="Cluster" caption="Cluster" IsHidden="false" >
                </telerik:PivotGridRowField>
                <telerik:PivotGridReportFilterField DataField= "Priority" IsHidden="true"  >
                </telerik:PivotGridReportFilterField>

                <telerik:PivotGridReportFilterField DataField="Topword" IsHidden="false" >
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

                <telerik:PivotGridReportFilterField DataField="Title" IsHidden="true" >
                </telerik:PivotGridReportFilterField>

                <telerik:PivotGridReportFilterField DataField="Datasourcenotes" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Resolutiondescription" IsHidden="true" >
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
              <telerik:PivotGridReportFilterField DataField="Tower" IsHidden="false" Caption="Tower" >
                </telerik:PivotGridReportFilterField>

          
                <telerik:PivotGridReportFilterField DataField="Closeddatetime"  DataFormatString="{0:d}"  IsHidden="true"  >
                </telerik:PivotGridReportFilterField>     
                <telerik:PivotGridReportFilterField DataField="ResolutionDate"  DataFormatString="{0:d}"  IsHidden="true"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Lastupdatedbyperson" IsHidden="true" Caption="Last updated by" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Lastupdateddatetime" IsHidden="true" Caption="Last updated" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="TicketId"  IsHidden="true"     >
                </telerik:PivotGridReportFilterField>


                <telerik:PivotGridAggregateField DataField="Id"   Caption="Count of Tickets"    Aggregate="Count"  >
                </telerik:PivotGridAggregateField>
         
                
            
            </Fields>
           
          <ClientSettings EnableFieldsDragDrop="true">
              
                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>
        </telerik:RadPivotGrid>
       <br/>

      <telerik:RadHtmlChart ID="RadHtmlChart1" Height="350px"  runat="server" Visible="true" Legend-Appearance-Position="Bottom"
         PlotArea-XAxis-LabelsAppearance-RotationAngle="45" PlotArea-YAxis-MinValue="0" PlotArea-YAxis-Type="Numeric">
   
         <Pan Enabled="true" />
         <Zoom Enabled="true">
                <MouseWheel Enabled="true" />
                <Selection Enabled="true" ModifierKey="Shift" />
         </Zoom>
    </telerik:RadHtmlChart>
</telerik:RadAjaxPanel>
                 </div>
             </div>

    </div>
    </form>


         <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <script type="text/javascript">
      //  var $ = $telerik.$;
 
         function exportPDF() {
             $find('<%=RadClientExportManager1.ClientID%>').exportPDF($(".RadPivotGrid1"));
        }
 function exportImage() {
     $find('<%=RadClientExportManager1.ClientID%>').exportImage($(".RadHtmlChart1"));
        }
         $('.parameter-filter-icon').on('click', function () {
             $(this).addClass('open');
             $('.parameter-slider-overlay').addClass('open');
             $('.parameter-slider').addClass('open');
         });
         $('.close-parameters').on('click', function () {
             $('.parameter-filter-icon').removeClass('open');
             $('.parameter-slider-overlay').removeClass('open');
             $('.parameter-slider').removeClass('open');
         });
         $('.parameter-slider-overlay').click(function () {
             $('.parameter-filter-icon').removeClass('open');
             $('.parameter-slider-overlay').removeClass('open');
             $('.parameter-slider').removeClass('open');
         });

     </script>
        </telerik:RadCodeBlock>
  </asp:Content>
