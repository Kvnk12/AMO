<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebUI.Areas.PortfolioAnalyzer.AspxReport.WebForm1" %>

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
       
   </style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
        
</head>
<body>
    <form id="form1" runat="server">
    <div>
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

                                          <telerik:RadButton RenderMode="Lightweight" runat="server" OnClientClicked="exportPDF" Text="Export to PDF" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
                           <telerik:RadButton RenderMode="Lightweight" runat="server"  OnClientClicked="exportImage" Text="Export to Image" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
      
                         <telerik:RadClientExportManager runat="server" ID="RadClientExportManager1">
        </telerik:RadClientExportManager>
            </div>
         
         <telerik:RadHtmlChart ID="RadHtmlChart1" Height="350px"  runat="server" Visible="true" Legend-Appearance-Position="Bottom"
       >
    
             <PlotArea>
                 <Series>
                     <telerik:PieSeries Name="pieSeries" StartAngle="90">

                     </telerik:PieSeries>
                 </Series>
             </PlotArea>
                       <Pan Enabled="true" />
         <Zoom Enabled="true">
                <MouseWheel Enabled="true" />
                <Selection Enabled="true" ModifierKey="Shift" />
            </Zoom>
    </telerik:RadHtmlChart>
                 
     <br />
          <div style="float:right">
         <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export Excel" 
        OnClick="ButtonExcel_Click" AlternateText="Xlsx" />
              </div>
         <br />
  
        <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  Font-Names="verdana,sans-serif" AllowPaging="false"  OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource" 
            AllowSorting="true" AllowFiltering="true" Width="100%"   ShowFilterHeaderZone="true"  EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"  >
        
            <Fields>
                
                <telerik:PivotGridColumnField DataField="AppDomainName" >
                </telerik:PivotGridColumnField>
             
              
                
                 <telerik:PivotGridRowField DataField="AppLifeCycleName"   >
                </telerik:PivotGridRowField>
               
            <%--      <telerik:PivotGridReportFilterField DataField="Urgency" IsHidden="true" >
                </telerik:PivotGridReportFilterField>--%>
                 
            
                <telerik:PivotGridAggregateField DataField="Id"  Caption="Total Application"    Aggregate="Count"  >
                </telerik:PivotGridAggregateField>
                
         
                
            
            </Fields>
           
          <ClientSettings EnableFieldsDragDrop="true">
              
        
                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>
        </telerik:RadPivotGrid>
       
    </div>
    </form>
</body>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <script type="text/javascript">
        var $ = $telerik.$;
 
        function exportPDF() {
            $find('<%=RadClientExportManager1.ClientID%>').exportPDF($("#form1"));
        }
 function exportImage() {
     $find('<%=RadClientExportManager1.ClientID%>').exportImage($("#form1"));
        }
      
    </script>
            </telerik:RadCodeBlock>
</html>