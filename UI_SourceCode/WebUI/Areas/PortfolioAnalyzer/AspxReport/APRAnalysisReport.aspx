<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="APRAnalysisReport.aspx.cs" Inherits="WebUI.Areas.PortfolioAnalyzer.AspxReport.APRAnalysisReport" %>

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
<span> <img alt="tool logo" src='<%=Page.ResolveUrl("~/Content/images/home/portfolio_analyzer_white.svg")%>'>Portfolio Analyzer</span>
</asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <form id="form1" runat="server">
    
        <asp:HiddenField ID="forgeryToken" runat="server"/>

         <div class="content-main dark-bg">
             <div class="clear"></div>
            <ol class="breadcrumb">
                <li>Portfolio Analyser</li>
                <li class="active">Analysis</li>
            </ol>
             <div class="content-wrapper">
                 <div class="parameter-section">
                    <div class="action-items">
                        <div class="export-wrapper">
                            <span class="export-label">Export - </span>
                            <telerik:RadButton RenderMode="Lightweight" runat="server" ID="btnExportpdf" CssClass="btnExportpdf" OnClientClicked="exportPDF" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                            <telerik:RadButton RenderMode="Lightweight" runat="server" ID="btnExportimage" CssClass="btnExportimage" OnClientClicked="exportImage" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton1" CssClass="btnExportExcel" runat="server"
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

        <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  Font-Names="verdana,sans-serif" AllowPaging="false"  OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
            AllowSorting="true" AllowFiltering="true" TotalsSettings-RowsSubTotalsPosition="None" TotalsSettings-RowGrandTotalsPosition="Last" TotalsSettings-ColumnsSubTotalsPosition="None" RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"   ShowFilterHeaderZone="true"  EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"  >
         <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" /> 
            <Fields>
                
                <telerik:PivotGridColumnField DataField="AppDomainName" >
                </telerik:PivotGridColumnField>
             
              
                
                 <telerik:PivotGridRowField DataField="AppLifeCycleName"   >
                </telerik:PivotGridRowField>
               
                  <telerik:PivotGridReportFilterField DataField="ApplicationName" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="CriticalityName" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="SourceCodeType" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                  <telerik:PivotGridReportFilterField DataField="AppProcessingType" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="AppSecurityLevel" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="AppSize" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="AppType" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="AvgConcurrentUsers" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="AvgIncidentTickets" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="AvgProblemTickets" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="AvgServiceRequestTickets" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="BusinessFunction" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="BusinessLogicInClientTier" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="ClientApplicationNumber" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="CodeComplexity" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                 <telerik:PivotGridReportFilterField DataField="Country" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="CreatedBy" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="InScope" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="IsApplicationDocAvailable" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="IsDisasterRecoveryPlanAvailable" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="IsHostedExternally" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="IsServer" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="IsTechnicalDocAvailable" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="LastModifiedBy" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="LegacyReasonExists" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="MaxConcurrentUsers" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="MigrationDate" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="MOTSLevel" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="NumberOfBugs" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="NumberOfInstances" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="NumberOfLinesOfCode" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="OutageDuration" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="OutageFrequency" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="OwningCompanies" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="PlannedRetirementDate" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="RegionNames" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="RevenueImpactIfDown" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="SizeUnits" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="TechnicalOwner" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="TechnicalOwnerEmail" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="TransactionVolumn" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="TransactionVolumnUnit" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="AliasName" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="AppArchitecture" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="AppEnvironment" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="AppIsActive" IsHidden="true" >
                </telerik:PivotGridReportFilterField>
               
            
                <telerik:PivotGridAggregateField DataField="Id"  Caption="Total Application"    Aggregate="Count">
                </telerik:PivotGridAggregateField>
                
         
                
            
            </Fields>
           
          <ClientSettings EnableFieldsDragDrop="true">
              
                      <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />

                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>

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
     </telerik:RadAjaxPanel>


                 </div>
             </div>
        </div>

    </form>

        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <script type="text/javascript">
        var $ = $telerik.$;
 
         function exportPDF() {
             $find('<%=RadClientExportManager1.ClientID%>').exportPDF($(".RadAjaxPanel"));
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

