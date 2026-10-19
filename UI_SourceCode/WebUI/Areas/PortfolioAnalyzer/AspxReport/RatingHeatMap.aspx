<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="RatingHeatMap.aspx.cs" Inherits="WebUI.Areas.PortfolioAnalyzer.AspxReport.RatingHeatMap" %>


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
                <li>Portfolio Analyzer</li>
                <li class="active">Dashboard-> Rating</li>
            </ol>
            
            <div class="content-wrapper">  
                 <div class="parameter-section">
                     <div class="action-items">
                        <div class="export-wrapper">
                            <span class="export-label">Export - </span>
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
                                <tr style="padding-top:10px">
                                    <td>Survey :</td>
                                    <td>
                                    <telerik:RadDropDownList ID="doprdown1" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                         DataValueField="Value" DataTextField="Text" DefaultMessage="Select Report View..." Width="230" Font-Names="verdana, sans-serif" DropDownWidth="230" OnSelectedIndexChanged="setToRadPivot">
                    </telerik:RadDropDownList>
                                    </td>
                                </tr>
                                <tr></tr>
                                <tr style="padding-top:10px">
                                    <td class="parameter-slider-td">
                                     <span class="td-checkbox">
                                         <label for="PublicChekbox">Classic View :</label>
                                         <asp:CheckBox ID="chkClassicViewPivot" OnCheckedChanged="chkClassicViewPivot_CheckedChanged" 
                                             AutoPostBack="true" Checked="true" runat="server"  />
                                     </span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                      </div>

        <br />

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
            AllowSorting="true" AllowFiltering="true" TotalsSettings-RowsSubTotalsPosition="None" TotalsSettings-RowGrandTotalsPosition="Last" TotalsSettings-ColumnsSubTotalsPosition="None" RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"  
            ShowFilterHeaderZone="true"  EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"  >
         <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" /> 
            <Fields>
                
                <telerik:PivotGridColumnField DataField="PerspectiveName" Caption="Perspective Name" >
                </telerik:PivotGridColumnField>

                <telerik:PivotGridRowField DataField="LOBName" Caption="Portfolio">
                </telerik:PivotGridRowField>
                 <telerik:PivotGridRowField DataField="ApplicationName" Caption="Application Name">
                </telerik:PivotGridRowField>
            
               <telerik:PivotGridAggregateField DataField="Rating"  Caption="Rating" Aggregate="Average" DataFormatString="{0:0.0}">
                </telerik:PivotGridAggregateField>
                      
            </Fields>
              <ClientSettings EnableFieldsDragDrop="true">
              <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                <ClientMessages DragToReorder="Drag the fields here"></ClientMessages>
             </ClientSettings>
        </telerik:RadPivotGrid>
        <br />
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
<%--        function exportPDF() {
            $find('<%=RadClientExportManager1.ClientID%>').exportPDF($("#form1"));
        }
 function exportImage() {
     $find('<%=RadClientExportManager1.ClientID%>').exportImage($("#form1"));
        }--%>
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

