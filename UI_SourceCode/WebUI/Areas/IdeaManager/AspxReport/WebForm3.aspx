<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="WebUI.Areas.IdeaManager.AspxReport.WebForm3" %>
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
#form2 .RadDropDownList .rddlEmptyMessage,#form2 .RadDropDownList .rddlDefaultMessage {
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



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
    <div>

        <table class="tableCssStyle">
                <tbody>
                    <tr>
                        <td>
                            <telerik:RadDropDownList ID="doprdown1" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                 DataValueField="Value" DataTextField="Text" DefaultMessage="Select Report View..." Width="250" Font-Names="verdana, sans-serif" DropDownWidth="250" OnSelectedIndexChanged="setToRadPivot">
            </telerik:RadDropDownList>
                        </td>
                        <td></td>
                        <td style="padding-left:2em;">
                             Report View Name :
                        </td>
                        <td>
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
                    <tr>
                        <td><br /></td>
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
                             <telerik:RadButton ID="RadButton1" runat="server" Text="Export Excel" 
                                  OnClick="ButtonExcel_Click" AlternateText="Xlsx" />
                    </td>
                </tr>           
            </table>
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

  <%--        <div style="float:right">         
             <telerik:RadButton RenderMode="Lightweight" runat="server" OnClientClicked="exportPDF" Text="Export to PDF" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
             <telerik:RadButton RenderMode="Lightweight" runat="server"  OnClientClicked="exportImage" Text="Export to Image" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
             <telerik:RadClientExportManager runat="server" ID="RadClientExportManager1"></telerik:RadClientExportManager>
         </div>
         <telerik:RadHtmlChart ID="RadHtmlChart1" Height="350px"  runat="server" Visible="true" Legend-Appearance-Position="Bottom"
         PlotArea-XAxis-LabelsAppearance-RotationAngle="45" PlotArea-YAxis-MinValue="0" PlotArea-YAxis-Type="Numeric">
         <Pan Enabled="true" />
         <Zoom Enabled="true">
                <MouseWheel Enabled="true" />
                <Selection Enabled="true" ModifierKey="Shift" />
         </Zoom>
         </telerik:RadHtmlChart>
                 
     <br />--%>
         <div style="float:right">
            <%-- <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export Excel" 
               OnClick="ButtonExcel_Click" AlternateText="Xlsx" />--%>
         </div>
    <br />
         <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  Font-Names="verdana,sans-serif" AllowPaging="true"  OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
            AllowSorting="true" AllowFiltering="true" TotalsSettings-RowsSubTotalsPosition="None" TotalsSettings-RowGrandTotalsPosition="Last" TotalsSettings-ColumnsSubTotalsPosition="None" RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"   ShowFilterHeaderZone="true"  EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting">
              <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" /> 
                <Fields>

                <telerik:PivotGridColumnField DataField="Status" >
                </telerik:PivotGridColumnField>
   
                <telerik:PivotGridRowField DataField="LOB Department">
                </telerik:PivotGridRowField>

                <telerik:PivotGridReportFilterField DataField="Challenge Name" IsHidden="false" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Quantification" IsHidden="false" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Description" IsHidden="false" >
                </telerik:PivotGridReportFilterField>               
                <telerik:PivotGridReportFilterField DataField="Priority" IsHidden="false"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Config Item" IsHidden="false"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Idea Count" IsHidden="false"  >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Created By" IsHidden="false">
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Created Month-Year"  DataFormatString="{0:d}" IsHidden="false"  >
                </telerik:PivotGridReportFilterField>
                <%--<telerik:PivotGridReportFilterField DataField="Created Date"  DataFormatString="{0:d}"  IsHidden="false"  >
                </telerik:PivotGridReportFilterField>     
                <telerik:PivotGridReportFilterField DataField="Updated Date"  DataFormatString="{0:d}"  IsHidden="false"  >
                </telerik:PivotGridReportFilterField>--%>

                <telerik:PivotGridAggregateField DataField="Id" ZoneIndex="0"   Caption="Total Count"    Aggregate="Count"  >                  
                </telerik:PivotGridAggregateField>

               </Fields>

            <ClientSettings EnableFieldsDragDrop="true">
            <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>

        </telerik:RadPivotGrid>

         <br />
                <div class="tableCssStyle" style="float:right">         
             <telerik:RadButton RenderMode="Lightweight" runat="server" OnClientClicked="exportPDF" Text="Export to PDF" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
             <telerik:RadButton RenderMode="Lightweight" runat="server"  OnClientClicked="exportImage" Text="Export to Image" AutoPostBack="false" UseSubmitBehavior="false"></telerik:RadButton>
             <telerik:RadClientExportManager runat="server" ID="RadClientExportManager1"></telerik:RadClientExportManager>
         </div>
         <telerik:RadHtmlChart ID="RadHtmlChart1" Height="350px"  runat="server" Visible="true" Legend-Appearance-Position="Bottom"
         PlotArea-XAxis-LabelsAppearance-RotationAngle="45" PlotArea-YAxis-MinValue="0" PlotArea-YAxis-Type="Numeric">
         <Pan Enabled="true" />
         <Zoom Enabled="true">
                <MouseWheel Enabled="true" />
                <Selection Enabled="true" ModifierKey="Shift" />
         </Zoom>
         </telerik:RadHtmlChart>
                 
    
    </div>
    </form>
</body>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

           //var $ = $telerik.$;
            function exportPDF() {
                $find('<%=RadClientExportManager1.ClientID%>').exportPDF($("#form2"));
            }
            function exportImage() {
                $find('<%=RadClientExportManager1.ClientID%>').exportImage($("#form2"));
            }
            
           
       </script>
    </telerik:RadCodeBlock>
</html>
