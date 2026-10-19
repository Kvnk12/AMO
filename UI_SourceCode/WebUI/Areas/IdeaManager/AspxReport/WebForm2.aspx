<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="WebUI.Areas.IdeaManager.AspxReport.WebForm2" %>

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

<span> <img alt="tool logo" src='<%=Page.ResolveUrl("~/Content/images/idea_manager.png")%>'>Idea Manager</span>
</asp:Content>
    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="form1" runat="server">
        <asp:HiddenField ID="forgeryToken" runat="server"/>
            <div class="content-main dark-bg">
             <div class="clear"></div>
            <ol class="breadcrumb">
                <li>Idea Manager</li>
                <li>Dashboard </li>
                <li class="active">Analysis Idea Report</li>
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

                            <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="RadPivotGrid1" />
            </PersistenceSettings>
        </telerik:RadPersistenceManager>
     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">

                                <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  Font-Names="verdana,sans-serif" AllowPaging="true"  OnCellDataBound="RadPivotGrid1_CellDataBound" OnNeedDataSource="RadPivotGrid1_NeedDataSource"  
                                    AllowSorting="true" AllowFiltering="true" TotalsSettings-RowsSubTotalsPosition="None" TotalsSettings-RowGrandTotalsPosition="Last" TotalsSettings-ColumnsSubTotalsPosition="None" RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"   ShowFilterHeaderZone="true"  EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"  >
                                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />       
                                   <Fields>
                
                                    <telerik:PivotGridColumnField DataField="Status" >
                                    </telerik:PivotGridColumnField>
   
                                    <telerik:PivotGridRowField DataField="LOB Department"   >
                                    </telerik:PivotGridRowField>
               
                                   <telerik:PivotGridReportFilterField DataField="Idea Name" IsHidden="false" >
                                   </telerik:PivotGridReportFilterField>
                                   <telerik:PivotGridReportFilterField DataField="Quantification" IsHidden="false" Caption="Value/Quantity in $" >
                                    </telerik:PivotGridReportFilterField>
                                   <telerik:PivotGridReportFilterField DataField="Idea Type" IsHidden="false" >
                                    </telerik:PivotGridReportFilterField>
                                  <telerik:PivotGridReportFilterField DataField="Idea Source" IsHidden="false"  >
                                     </telerik:PivotGridReportFilterField>
                                  <telerik:PivotGridReportFilterField DataField="Priority" IsHidden="false"  >
                                     </telerik:PivotGridReportFilterField>
                                  <telerik:PivotGridReportFilterField DataField="Implementation Priority" IsHidden="false"  >
                                     </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Created By" IsHidden="false" >
                                     </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Created Year" IsHidden="false" >
                                     </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Created Month" IsHidden="false" >
                                     </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Created Month-Year" Caption="Creation Date"  DataFormatString="{0:d}" IsHidden="false"  >
                                     </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Team" IsHidden="false" >
                                     </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Complexity" IsHidden="false" >
                                     </telerik:PivotGridReportFilterField>
                                  <telerik:PivotGridReportFilterField DataField="Internal Initiative" IsHidden="false" >
                                    </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Implementation Status" IsHidden="false" >
                                    </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Approved By" IsHidden="false" >
                                   </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Approved Date" IsHidden="false" >
                                    </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="SubmittedForReviewDt" Caption="Idea Submission Date" IsHidden="false">
                                    </telerik:PivotGridReportFilterField>
                                 <telerik:PivotGridReportFilterField DataField="Implementation Manager" IsHidden="false" >
                                   </telerik:PivotGridReportFilterField>

                                <telerik:PivotGridReportFilterField DataField="IdeaNumber" Caption ="Idea Number"  IsHidden="false" >
                                  </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="PlannedStartDate" Caption="Planned Start Date" IsHidden="false" >
                                  </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="PlannedEndDate" Caption="Planned End Date" IsHidden="false" >
                                  </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ActualStartDate" Caption="Actual Start Date" IsHidden="false" >
                                  </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ActualEndDate" Caption="Actual End Date" IsHidden="false" >
                                  </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ImplementationProgress" Caption="Implementation Progress" IsHidden="false" >
                                  </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="CompletedDt" Caption="Completion Date" IsHidden="false" >
                                  </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="LastReviewDt" Caption="Last Review Date" IsHidden="false" >
                                  </telerik:PivotGridReportFilterField>
                
                            <telerik:PivotGridAggregateField DataField="Id" ZoneIndex="0" Caption="Total Count" Aggregate="Count">
                              </telerik:PivotGridAggregateField>
        
                        </Fields>
           
                          <ClientSettings EnableFieldsDragDrop="true">
                            <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
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
      //  var $ = $telerik.$;
 
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
