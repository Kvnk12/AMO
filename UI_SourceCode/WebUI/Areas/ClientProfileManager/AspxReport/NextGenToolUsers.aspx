<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="NextGenToolUsers.aspx.cs" Inherits="WebUI.Areas.ClientProfileManager.AspxReport.NextGenToolUsers" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="headercontent" ContentPlaceHolderID="headertitle" runat="server">

  <style type="text/css">
      .RadPivotGrid .rpgFieldItem {
          margin: 4px;
          /*padding: 5px;
          display: block;*/
          display: inline-block !important;
      }
       
     
    /*.PivotGridWindow {
           width: 700px !important;
           height: 400px !important;
           
       }*/
       
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
   </style>
      <span><img alt="tool logo" src='<%=Page.ResolveUrl("~/Content/images/home/Client_Profile_manager_white.svg")%>'>Client profile manager</span>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
                    <asp:HiddenField ID="forgeryToken" runat="server"/>

          <div class="content-main dark-bg">
             <div class="clear"></div>
            <ol class="breadcrumb">
                <li>Configuration Manager</li>
                <li class="active">AM-Optimization User List</li>
            </ol>
              
             <div >
                 <div class="parameter-section">

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
         <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export Excel" 
        OnClick="ButtonExcel_Click" AlternateText="Xlsx" />
              </div>
         <br />
  <div style="width:100%; overflow-x:scroll;">
        <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  Font-Names="verdana,sans-serif" AllowPaging="true" PageSize="20" OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
            AllowSorting="true" AllowFiltering="true" RowTableLayout="Tabular" TotalsSettings-RowsSubTotalsPosition="None" TotalsSettings-RowGrandTotalsPosition="None" TotalsSettings-ColumnsSubTotalsPosition="None" TotalsSettings-ColumnGrandTotalsPosition="Last" RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"   ShowFilterHeaderZone="true"  EnableZoneContextMenu="true"  OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting">
            
<%--            <TotalsSettings RowsSubTotalsPosition="None" RowGrandTotalsPosition="None"
ColumnsSubTotalsPosition="None" ColumnGrandTotalsPosition="None" />--%>


            <Fields>

                <telerik:PivotGridRowField DataField="Client" Caption="Client Name">
                </telerik:PivotGridRowField>

                <telerik:PivotGridReportFilterField DataField="Manager">
                </telerik:PivotGridReportFilterField>

                <telerik:PivotGridRowField DataField="Name" Caption="UserName/Email">
                </telerik:PivotGridRowField>

                <telerik:PivotGridRowField DataField="Contact" Caption="Contact Number">
                </telerik:PivotGridRowField>

                <telerik:PivotGridRowField DataField="Work" Caption="User Role">
                </telerik:PivotGridRowField>

                <telerik:PivotGridColumnField DataField="AppModule" Caption="Next Gen Tool">
                </telerik:PivotGridColumnField>

                 <telerik:PivotGridAggregateField DataField="UserId" CellStyle-CssClass="onTheRight"  Caption="Count"  DataFormatString = "{0:N0}"   Aggregate="Count"  >
                </telerik:PivotGridAggregateField>

       
                
            
            </Fields>
           
            <ClientSettings EnableFieldsDragDrop="true">
       
                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
            </ClientSettings>
        </telerik:RadPivotGrid>
       </div>
                     </div>
                 </div>
              </div>
    </form>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <script type="text/javascript">
        var $ = $telerik.$;
 

      
    </script>
            </telerik:RadCodeBlock>
</asp:Content>