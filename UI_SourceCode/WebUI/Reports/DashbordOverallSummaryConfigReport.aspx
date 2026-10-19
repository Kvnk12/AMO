<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="DashbordOverallSummaryConfigReport.aspx.cs" Inherits="WebUI.Reports.DashbordOverallSummaryConfigReport" %>

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
   </style>


     <span> <img alt="tool logo" src='<%=Page.ResolveUrl("~/Content/images/incident_navigator_icon.png")%>'>Incident Navigator</span>
</asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <form id="form1" runat="server">
          <asp:HiddenField ID="forgeryToken" runat="server"/>
    <div class="content-wrapper">
  
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
         <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel2"  CssClass="div-container no-bg" LoadingPanelID="RadAjaxLoadingPanel1">

        <telerik:RadGrid ID="RadGrid1" OnBatchEditCommand="RadGrid1_BatchEditCommand" OnPreRender="RadGrid1_PreRender"  Width="90%"  OnItemDataBound="RadGrid1_ItemDataBound" EnableEmbeddedSkins="true"  OnItemUpdated="RadGrid1_ItemUpdated" runat="server">
             <MasterTableView CommandItemDisplay="Top" DataKeyNames="summaryID" ClientDataKeyNames="ViewSpecID"   Font-Size="Small" CommandItemSettings-ShowAddNewRecordButton="false" CommandItemSettings-ShowRefreshButton="false"
                HorizontalAlign="NotSet" EditMode="Batch" AutoGenerateColumns="False">
      
                  <BatchEditingSettings EditType="Cell" OpenEditingEvent="Click" />
                 <Columns>
                     <telerik:GridCheckBoxColumn HeaderText="Include in Dashboard" UniqueName="IsIncludeInDashboard" HeaderStyle-Width="80px" DataField="IsIncludeInDashboard"></telerik:GridCheckBoxColumn>
                  <telerik:GridBoundColumn DataField="MetricName" ReadOnly="true" ForceExtractValue="Always"   HeaderText="Performance Category Original Name"
                        UniqueName="MetricName">
                    </telerik:GridBoundColumn>
                       <telerik:GridBoundColumn DataField="CustomSLAName"   HeaderText="Performance Category Custom name"
                        UniqueName="CustomSLAName">
                    </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn DataField="ParentID" Display="false"   HeaderText="ParentID" SortExpression="ParentID"
                        UniqueName="ParentID">
                    </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn DataField="ViewSpecID" Display="false" ForceExtractValue="Always"   HeaderText="ViewSpecID" SortExpression="ViewSpecID"
                        UniqueName="ViewSpecID">
                    </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn DataField="summaryID" Display="false"   HeaderText="summaryID" SortExpression="summaryID"
                        UniqueName="summaryID">
                    </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn DataField="ComplianceTarget" ForceExtractValue="Always"  ReadOnly="true" HeaderStyle-Width="100px"  HeaderText="Target"
                        UniqueName="ComplianceTarget">
                    </telerik:GridBoundColumn>
                    
                 </Columns>
                 
                  </MasterTableView>
            <ClientSettings AllowKeyboardNavigation="true">
                  <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="350px" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling>
                       <ClientEvents OnBatchEditCellValueChanging="RadGridSummary_OnBatchEditCellValueChanging" />
            </ClientSettings>
                  </telerik:RadGrid>

             </telerik:RadAjaxPanel>
    </div>
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>
<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            function RadGridSummary_OnBatchEditCellValueChanging(sender, args) {

                var grid = $find('<%= RadGrid1.ClientID %>');
                var masterTable = grid.get_masterTableView();
                var rows = masterTable.get_dataItems();
                var rowArgs = args.get_row();
                var rowIndex = rowArgs.sectionRowIndex;

                var row = args.get_row();
                //  var rowIndex = row.rowIndex;
                var columnName = args.get_columnUniqueName();
                var cell = args.get_cell();
                var oldValue = args.get_cellValue();
                // var newValue = args.get_editorValue();
                var IsmodifiedAlready = false;
                var row1 = rows[rowIndex];
                var txtTotal = row1.get_cell("IsIncludeInDashboard");
                if (columnName === "IsIncludeInDashboard") {

                    var newValue = args.get_editorValue();
                    var firstDataItem = $find("<%=RadGrid1.ClientID %>").get_masterTableView().get_dataItems()[rowIndex];

                  var ParentID = row1.get_cell("ParentID");
                  var getCurrentParentID = grid.get_batchEditingManager().getCellValue(ParentID);
           
                  if (getCurrentParentID === "" && IsmodifiedAlready === false) {
                      //alert("parentNull");
                      IsmodifiedAlready = true;
                      var viewspecId = firstDataItem.getDataKeyValue("ViewSpecID");
                      for (var i = 0; i < rows.length; i++) {
                          var ParentIDCell = rows[i].get_cell("ParentID");
                          var ParentID = grid.get_batchEditingManager().getCellValue(ParentIDCell);
                          if (viewspecId === ParentID) {
                              //  alert(ParentID + "=" + viewspecId);
                              var ParentChecked = rows[i].get_cell("IsIncludeInDashboard");
                              var IsParentChecked = grid.get_batchEditingManager().getCellValue(ParentChecked);
                              grid.get_batchEditingManager().changeCellValue(ParentChecked, newValue);
                              // break;
                          }

                      }
                  }
                 
              }

          }
           </script>
        </telerik:RadScriptBlock>
         
    </form>
</asp:content>