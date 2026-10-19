<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="INTrendingReport.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.INTrendingReport" %>
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
              div.RadPivotGrid .rpgDataCell{
           text-align:right;
       }
   </style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
        
</head>
<body>
    <form id="form1" runat="server">
    <div>
                <asp:HiddenField ID="forgeryToken" runat="server"/>
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
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"  EnableSkinTransparency="true" >
    </telerik:RadAjaxLoadingPanel>

        
                                     <div style="float:right">
              <%--  <asp:Label ID="Label2" runat="server" Text="Month/ Year" cssclass="col-lg-3 control-label" Style="margin-top:-10px!important"></asp:Label>
            
                                         <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="RadMonthYearPicker1" OnSelectedDateChanged="RadMonthYearPicker1_SelectedDateChanged" AutoPostBack="true" runat="server" Width="200px">
            </telerik:RadMonthYearPicker>--%>
                                         <telerik:RadButton RenderMode="Lightweight" runat="server" OnClientClicked="exportPDF" Text="Export to PDF" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                           <telerik:RadButton RenderMode="Lightweight" runat="server"  OnClientClicked="exportImage" Text="Export to Image" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
      
                         <telerik:RadClientExportManager runat="server" ID="RadClientExportManager1">
        </telerik:RadClientExportManager>
            </div>
         <telerik:RadHtmlChart ID="RadHtmlChart1" Height="350px"  runat="server" Visible="true" Legend-Appearance-Position="Bottom"
         PlotArea-XAxis-LabelsAppearance-RotationAngle="45" PlotArea-YAxis-MinValue="0" PlotArea-YAxis-Type="Numeric">
       
    </telerik:RadHtmlChart>
     <br />
          <div style="float:right">
         <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export Excel" 
        OnClick="ButtonExcel_Click" AlternateText="Xlsx" />
              </div>
         <br />
     <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Names="verdana,sans-serif" Font-Size="Small"  AllowPaging="true"  OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"
            AllowSorting="true" AllowFiltering="true" Width="100%"  ShowFilterHeaderZone="true" EnableZoneContextMenu="true" EnableCaching="true" >
      <%-- <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true"></PagerStyle>--%>
           <%-- <DataCellStyle Width="50px" />--%>
            <Fields>
                 <telerik:PivotGridRowField DataField="Priority"    >
                </telerik:PivotGridRowField>
                  <telerik:PivotGridColumnField DataField="Status" >
                </telerik:PivotGridColumnField>
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
               <%-- <telerik:PivotGridReportFilterField DataField="Createdtimezone" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
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
               <%-- <telerik:PivotGridReportFilterField DataField="Acknowledgeddatetime" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
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
                <telerik:PivotGridReportFilterField DataField="Servicerestoredtime" IsHidden="true" Caption="Service Restored Time" >
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
                 <telerik:PivotGridReportFilterField DataField="Assigneecompany" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Assigneeorganization" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Estimatedworkeffort" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Actualworkeffort" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Recyclecount" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ConfigItemCategory" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
               <%-- <telerik:PivotGridReportFilterField DataField="ResponseTime" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
                <telerik:PivotGridReportFilterField DataField="ClosureCategory" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Severity" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="WorkGroupLevel" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="AffectedItemName" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="SLACostPerTicket" IsHidden="true" Caption="Total Ticket Cost" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="INResolutiontime" IsHidden="true" Caption="Total Resolution effort" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="INCostPerTicket" IsHidden="true" Caption="Total Ticket Cost" >
                </telerik:PivotGridReportFilterField>
              
              <%--   <telerik:PivotGridReportFilterField DataField="ResolutionQuarter"    >
                </telerik:PivotGridReportFilterField>--%>
                 
                 <%--<telerik:PivotGridReportFilterField DataField="CongifItem"    >
                </telerik:PivotGridReportFilterField>--%>
                
            <%--      <telerik:PivotGridReportFilterField DataField="Level"  Caption="Service Level"   >
                </telerik:PivotGridReportFilterField>--%>
                 
                     
                      <telerik:PivotGridReportFilterField DataField="ResolutionDate"  DataFormatString="{0:d}"  IsHidden="true"  >
                </telerik:PivotGridReportFilterField>
                <%--   
                  <telerik:PivotGridReportFilterField DataField="CostPerTicket"  Caption="Total Cost"  >
                </telerik:PivotGridReportFilterField>--%>
            <%--    <telerik:PivotGridReportFilterField DataField="Resolutionmonth"  IsHidden="true"  SortOrder="None" >
                </telerik:PivotGridReportFilterField>--%>
              <%--  <telerik:PivotGridReportFilterField DataField="ClosureCategory" IsHidden="true">
                </telerik:PivotGridReportFilterField>--%>

             
                <telerik:PivotGridAggregateField DataField="Incidentid" Caption="Ticket Count" Aggregate="Count" SortOrder="Ascending" >
                </telerik:PivotGridAggregateField>
                
            
            </Fields>
           
          <ClientSettings EnableFieldsDragDrop="true">
                <%--<Scrolling AllowVerticalScroll="true"></Scrolling>--%>
                
        
                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>
        </telerik:RadPivotGrid>
    </div>
    </form>
</body>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <script type="text/javascript">
       // var $ = $telerik.$;
 
        function exportPDF() {
            $find('<%=RadClientExportManager1.ClientID%>').exportPDF($("#form1"));
        }
 function exportImage() {
     $find('<%=RadClientExportManager1.ClientID%>').exportImage($("#form1"));
        }
      
    </script>
            </telerik:RadCodeBlock>
</html>
