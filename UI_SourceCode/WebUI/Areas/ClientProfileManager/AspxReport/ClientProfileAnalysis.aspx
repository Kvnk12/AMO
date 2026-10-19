<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="ClientProfileAnalysis.aspx.cs" Inherits="WebUI.Areas.ClientProfileManager.AspxReport.ClientProfileAnalysis" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="headercontent" ContentPlaceHolderID="headertitle" runat="server">

<style type="text/css">
      .RadPivotGrid .rpgFieldItem {
          margin: 4px;
          /*padding: 5px;
          display: block;*/
          display: inline-block !important;
      }
       
     
    .PivotGridWindow {
           width: 700px !important;
           height: 800px !important;

       }
       
       .reportlbl{
           font-size:small;
           font-family:Helvetica Neue, Helvetica, Arial, sans-serif;
           font-weight:bold;
               
       }  
       .RadPivotGrid_Default th, .RadPivotGrid_Default td, .RadPivotGrid_Default .rpgOuterTableWrapper, .RadPivotGrid_Default .rpgContentZoneDiv td
       {
           text-align:right;
       }
       div.RadPivotGrid .rpgFieldItem.aggregateCustom1, div.RadPivotGrid .rpgColumnHeader.aggregateCustom1 {
    color: #006699;
}
 
div.RadPivotGrid .rpgFieldItem.aggregateCustom2, div.RadPivotGrid .rpgColumnHeader.aggregateCustom2 {
    color: #006666;
}

.tableCssStyle{
   /*width: 70%;
   padding: 20px;
   background-color: #ebebeb;
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
   </style>


     <span><img alt="tool logo" src='<%=Page.ResolveUrl("~/Content/images/home/Client_Profile_manager_white.svg")%>'>Client profile manager</span>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
      <asp:HiddenField ID="forgeryToken" runat="server"/>
         <div class="content-main dark-bg">
             <div class="clear"></div>
            <ol class="breadcrumb">
                <li>Client Profile Manager</li>
                <li class="active">Analysis</li>
            </ol>

             <div class="content-wrapper">
                 <div class="parameter-section">

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
                                               DataValueField="Value" DataTextField="Text" DefaultMessage="Select Report View..." Width="250" Font-Names="verdana, sans-serif" DropDownWidth="250" OnSelectedIndexChanged="setToRadPivot">
                                            </telerik:RadDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td style="padding-top:10px"  colspan="3">
                                             Report View Name :
                                           <telerik:RadTextBox ID="ViewNameText" runat="server" Width="250px" CausesValidation="true" InputType="Text"/>
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
                                            <telerik:RadButton ID="RadButton1" runat="server" Primary="true" OnClick="SaveViewDetails" Text="Save"/>
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
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"  EnableSkinTransparency="true" >
    </telerik:RadAjaxLoadingPanel>

        <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="RadPivotGrid1" />
            </PersistenceSettings>
        </telerik:RadPersistenceManager>
       <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">

        <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  Font-Names="verdana,sans-serif" AllowPaging="false"  OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
            AllowSorting="true" AllowFiltering="true" TotalsSettings-RowsSubTotalsPosition="None" TotalsSettings-RowGrandTotalsPosition="Last" TotalsSettings-ColumnsSubTotalsPosition="None" RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"   ShowFilterHeaderZone="true"  EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting">
              <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" /> 
        
            <Fields>
                <telerik:PivotGridRowField DataField="ClientName"   >
                </telerik:PivotGridRowField>
                <telerik:PivotGridColumnField DataField="Year" >
                </telerik:PivotGridColumnField>
                <telerik:PivotGridAggregateField DataField="IncidentNumber" CellStyle-CssClass="onTheRight"  Caption="Incident Number"  DataFormatString = "{0:N0}"   Aggregate="Sum"  >
                </telerik:PivotGridAggregateField>
                 <telerik:PivotGridReportFilterField DataField="Quarter" Caption="Quarter" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="IncidentEffortHrs" CellStyle-CssClass="onTheRight" Caption="Incident Effort Hrs."   >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="IncidentEffortPerUnit" Caption="Incident Effort/Unit"  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="UssrNumber" Caption="User Support SR"  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="UssrEffortHrs" Caption="User Effort Hrs." >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="UssrEffortPerUnit"  Caption="User Effort/Unit"  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="EnhanceNumber"  Caption="Enhancement Number" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="EnhaceEffortHrs" Caption="Enhancement Effort Hrs." >  
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="EnhanceEffortPerUnit"  Caption="Enhancement Effort/Unit" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="ProblemsNumber" Caption="Problem Number"  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="ProblemsEffortHrs" Caption="Problem Effort Hrs."  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="ProblemsEffortPerUnit"  Caption="Problem Effort/Unit"  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="SRBacklog" Caption="SR Backlog"  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="InMTTRSev1" Caption="MTTR-Sev1"  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="InMTTRSev2"  Caption="MTTR-Sev2"  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="InMTTRSev3"  Caption="MTTR-Sev3"  >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="InMTTRSev4"  Caption="MTTR-Sev4"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="SrMTTRSev1" Caption="MTT Fulfil SR -Sev1"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="SrMTTRSev2" Caption="MTT Fulfil SR -Sev2"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="SrMTTRSev3" Caption="MTT Fulfil SR -Sev3"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="SrMTTRSev4" Caption="MTT Fulfil SR -Sev4" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="NttFTENearShore" Caption="MTT Fulfil SR -Sev1"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="NttFTEOffshore" Caption="NTTD - Offshore"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="NttFTEOnsite" Caption="NTTD - On Site" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="NoOfApplications" Caption="No. of Apps."  >
                </telerik:PivotGridReportFilterField>
 
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
        var $ = $telerik.$;
 
         function exportPDF() {
             //document.getElementById('exportSectionID').contentWindow.print();
             // window.print();
             $find('<%=RadClientExportManager1.ClientID%>').exportPDF($(".RadAjaxPanel"));
           // $find('<%=RadClientExportManager1.ClientID%>').exportPDF($(".RadHtmlChart"));
       }
 function exportImage() {
     $find('<%=RadClientExportManager1.ClientID%>').exportImage($(".RadAjaxPanel"));
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