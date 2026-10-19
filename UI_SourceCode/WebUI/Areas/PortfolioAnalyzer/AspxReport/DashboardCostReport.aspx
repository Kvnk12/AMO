<%@ Page Language="C#"  MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="DashboardCostReport.aspx.cs" Inherits="WebUI.Areas.PortfolioAnalyzer.AspxReport.DashboardCostReport" %>

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
                <li class="active">Dashboard-> Cost</li>
            </ol>
            <div class="parameter-section">
            <div class="action-items">
                 <div class="export-wrapper">
                            <span class="export-label">Export - </span>
                              <telerik:RadButton RenderMode="Lightweight" runat="server" ID="btnExportpdf" OnClientClicked="exportPDF" CssClass="btnExportpdf" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                            <telerik:RadButton RenderMode="Lightweight" runat="server" ID="btnExportimage" OnClientClicked="exportImage" CssClass="btnExportimage" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                            
                            <telerik:RadClientExportManager runat="server" ID="RadClientExportManager1">
                            </telerik:RadClientExportManager>
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
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"  EnableSkinTransparency="true" >
    </telerik:RadAjaxLoadingPanel>

        
           <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                          
         
         <telerik:RadHtmlChart ID="RadHtmlChart1" Height="350px"  runat="server" Visible="false" Legend-Appearance-Position="Bottom"
         PlotArea-XAxis-LabelsAppearance-RotationAngle="45" PlotArea-YAxis-MinValue="0" PlotArea-YAxis-Type="Numeric">

                       <Pan Enabled="true" />
         <Zoom Enabled="true">
                <MouseWheel Enabled="true" />
                <Selection Enabled="true" ModifierKey="Shift" />
            </Zoom>
    </telerik:RadHtmlChart>
                 

         <br />
  
        <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  Font-Names="verdana,sans-serif" AllowPaging="false"  OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
            AllowSorting="true" AllowFiltering="true"   ShowFilterHeaderZone="true" TotalsSettings-ColumnsSubTotalsPosition="None"  EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"  >
            <Fields>
                
                <telerik:PivotGridReportFilterField DataField="Year" Caption="Year">
                </telerik:PivotGridReportFilterField>
             
                 <telerik:PivotGridColumnField DataField="Category" Caption="Category" >
                </telerik:PivotGridColumnField>
                 <telerik:PivotGridReportFilterField DataField="Type" Caption="Type">
                </telerik:PivotGridReportFilterField>
                  <telerik:PivotGridReportFilterField DataField="InternalOrExternal" Caption="InternalOrExternal" >
                </telerik:PivotGridReportFilterField>
                <%-- <telerik:PivotGridReportFilterField DataField="Quarter"    >
                </telerik:PivotGridReportFilterField>--%>
               <%-- <telerik:PivotGridColumnField DataField="MonthName"    >
                </telerik:PivotGridColumnField>--%>
                <telerik:PivotGridReportFilterField DataField="ClientApplicationNumber" Caption="Client Application Number" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="ApplicationClassificationName" Caption="Application Classification" >
                </telerik:PivotGridReportFilterField>
                
                 <telerik:PivotGridReportFilterField DataField="BusinessFunction"  Caption="Business Function"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridRowField DataField="ApplicationName" Caption="Application"   >
                </telerik:PivotGridRowField>
                
                 
              <%--  <telerik:PivotGridRowField DataField="City">
                </telerik:PivotGridRowField>--%>
                <telerik:PivotGridAggregateField DataField="Total"  Caption="Total Cost"    Aggregate="Sum" DataFormatString="{0:C}" >
                </telerik:PivotGridAggregateField>
                
              <%--    <telerik:PivotGridAggregateField DataField="TotalCost">
                </telerik:PivotGridAggregateField>--%>
                
            
            </Fields>
           
          <ClientSettings EnableFieldsDragDrop="true">
                <%--<Scrolling AllowVerticalScroll="true"></Scrolling>--%>
                
        
                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>
        </telerik:RadPivotGrid>
       </telerik:RadAjaxPanel>
  
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
      
     </script>
            </telerik:RadCodeBlock>

</asp:Content>