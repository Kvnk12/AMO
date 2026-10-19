<%@ Page Title="" Language="C#" MasterPageFile="~/SubscriptionMasterPage.Master" AutoEventWireup="true" CodeBehind="RptInflowOutflowSubscription.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.RptInflowOutflowSubscription" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headertitle" runat="server">
    <style type="text/css">
   

    .PivotGridWindow {
        width: 700px !important;
        height: 440px !important;
    }

    div.RadPivotGrid .rpgDataCell {
        text-align: right;
    }

    .reportlbl {
        font-size: small;
        font-family: Helvetica Neue, Helvetica, Arial, sans-serif;
        font-weight: bold;
    }

    div.RadPivotGrid .rpgFieldItem.aggregateCustom1, div.RadPivotGrid .rpgColumnHeader.aggregateCustom1 {
        color: #006699;
    }

    div.RadPivotGrid .rpgFieldItem.aggregateCustom2, div.RadPivotGrid .rpgColumnHeader.aggregateCustom2 {
        color: #006666;
    }


    #form1 .RadDropDownList .rddlEmptyMessage, #form1 .RadDropDownList .rddlDefaultMessage {
        opacity: 1;
        font-style: normal;
        z-index:9999;
        font-family: HelveticaNeue;
    }


    html body .RadInput_Default .riTextBox, html body .RadInputMgr_Default {
        border-color: rgb(204, 204, 204);
        color: #333;
        background-color: #FFF;
        font: 12px "segoe ui",arial,sans-serif;
        border-style: solid;
        border-radius: 5px;
        height: 2.5em;
        z-index:9999 !important;
    }
    /*.auto-style1 {
        width: 202px;
    }*/
    .progressIndicatorStyle {
        width: 200px;
        height: 200px;
        top: 25%;
        left: 40%;
        z-index: 99999;
        position: absolute;
        display: block;
    }
    /*.auto-style2 {
        width: 262px;
    }*/

    @media print {
        body * {
            visibility: hidden;
        }

        #exportSectionID, #exportSectionID * {
            visibility: visible;
        }

        #exportSectionID {
            position: absolute;
            left: 0;
            top: 0;
        }
    }
    .header-logo{
        background-image:url(../../../Content//images/nttdata_logo.png);
        background-repeat: no-repeat;
        height: 2em;
        width: 12em;
        background-size: 11em;
        margin: 1.5em;
    }
    .tool-logo{
        background-image:url(../../../Content/images/home/incident_navigator_white.svg);
        background-repeat:no-repeat;
        height: 2em;
        width: 2em;
    }
    .clientLogoClass{
        width: auto;
        height: 1.5em;
        margin: 5px 10px;
        color: white;
        font-size: 16px;
    }
    .headerText{
        color:white;font-family:HelveticaNeue-Light;font-size:16px;margin: 6px;height: 1em;
    }
</style>

    <link rel="stylesheet" href="https://dynamoportal.net/AMOPortalSharedService/Content/css/custom-styles.css" />
    <script type="text/javascript" src="../../../Scripts/jquery-3.6.0.js"></script>
    <script type="text/javascript" src="../../../Scripts/bootstrap.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/jquery-ui-1.12.1.min.js"></script>
</asp:Content>

     <asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <form id="form1" runat="server">

         <div style="min-height: 70px;background-color: #0079A1;border: none;font-size: 15px;padding-top: unset;display: flex;padding-bottom: unset;">

             <div class="header-logo"> </div>
             <div style="padding-top:1.4em;display:flex">
                 <div class="tool-logo"> </div>
                 <asp:Image runat="server" ID="clientlogo" CssClass="clientLogoClass"/>
                       <asp:Label ID="clientNameLabel" runat="server" Text="" CssClass="headerText"></asp:Label>
                 <div style="color:white;font-family:HelveticaNeue-Light;font-size:16px;padding: 6px;height: 1em;"> Incident Navigator</div>
             </div>
         </div>

        <div class="content-main dark-bg">
            <div class="clear"></div>
            <ol class="breadcrumb">
                <li>Incident Navigator</li>
                <li class="active">Inflow Outflow Backlog Analysis</li>
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
                    </div>

                    <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
                    </telerik:RadScriptManager>
                       <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="RadPivotGrid1">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                                 <telerik:AjaxUpdatedControl ControlID="RadHtmlChart1"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                     </AjaxSettings>
<%--          <AjaxSettings>
               <telerik:AjaxSetting AjaxControlID="chkClassicViewPivot">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>--%>
                <AjaxSettings>
               <telerik:AjaxSetting AjaxControlID="doprdown1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                    <telerik:AjaxUpdatedControl ControlID="RadHtmlChart1"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
                <AjaxSettings>
               <telerik:AjaxSetting AjaxControlID="SaveView">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
                             <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="ChartDrptype">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                                      <telerik:AjaxUpdatedControl ControlID="RadHtmlChart1"  />
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
                      <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" PostBackControls="btnExportpdf,btnExportimage,btnExportExcel,RadClientExportManager1,doprdown1,ViewNameText,PublicChekbox,ApplyButton,SaveView,RadDatePicker2,RadDatePicker1,RadPivotGrid1,ChartDrptype,RadHtmlChart1,RadPersistenceManager1,RadAjaxLoadingPanel1,RadAjaxManager1" >

        <telerik:RadPivotGrid RenderMode="Lightweight"  runat="server" ID="RadPivotGrid1" Font-Size="Smaller"  
            Font-Names="verdana,sans-serif" AllowPaging="true"  OnCellDataBound="RadPivotGrid1_CellDataBound"
            AllowSorting="false" AllowFiltering="true" TotalsSettings-RowsSubTotalsPosition="None"  
            TotalsSettings-RowGrandTotalsPosition="None" TotalsSettings-ColumnsSubTotalsPosition="None"   
            RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%"   AggregatesPosition="Rows"
            EnableConfigurationPanel="false"  ShowFilterHeaderZone="true"  EnableZoneContextMenu="true" 
            OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting"  OnNeedDataSource="RadPivotGrid1_NeedDataSource" OnAddingFieldToZone="RadPivotGrid1_AddingFieldToZone" RowHeaderZoneText=" " >
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="false" />
            <Fields>
                <telerik:PivotGridReportFilterField DataField="Location"  Caption="FLTR-Location"  UniqueName="Location" IsHidden="false" >
                    
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Priority" Caption="FLTR-Priority" UniqueName="Priority" IsHidden="false">
                    
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Service Area" Caption="FLTR-Service Area" UniqueName="ServiceArea" IsHidden="false">
                    
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Tower"  Caption="FLTR-Tower" UniqueName="Tower" IsHidden="false" >
                    
                </telerik:PivotGridReportFilterField>
                
                <telerik:PivotGridReportFilterField DataField="Type"  Caption="FLTR-Type" UniqueName="Type" IsHidden="false"></telerik:PivotGridReportFilterField>
                    
                 <telerik:PivotGridReportFilterField DataField="Assignedtosupportgroup"  Caption="FLTR-AssignmentGroup" UniqueName="Assignedtosupportgroup" IsHidden="false" >
                </telerik:PivotGridReportFilterField>
                
                <telerik:PivotGridColumnField DataField="Week" Caption="DIM-Week"  UniqueName="Week" IsHidden="false">
                </telerik:PivotGridColumnField>

                <telerik:PivotGridReportFilterField DataField="Date" Caption="DIM-Date"  UniqueName="Date" IsHidden="false">
                </telerik:PivotGridReportFilterField>

                <telerik:PivotGridReportFilterField DataField="Month" Caption="DIM-Month"  UniqueName="Month" IsHidden="false">
                    
                </telerik:PivotGridReportFilterField>

                <telerik:PivotGridReportFilterField DataField="Quarter" Caption="DIM-Quarter" UniqueName="Quarter" IsHidden="false">
                     
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridReportFilterField DataField="Year" Caption="DIM-Year"  UniqueName="Year" IsHidden="false">
                     
                </telerik:PivotGridReportFilterField>

                <telerik:PivotGridAggregateField DataField="Created Tickets"  Caption="MSR-Created" UniqueName="CreatedCount" Aggregate="Sum"  IgnoreNullValues="true">
                    <HeaderCellTemplate>
                            <asp:Label ID="lbl010" Text="Created" runat="server" />
                        </HeaderCellTemplate>
                </telerik:PivotGridAggregateField>
                <telerik:PivotGridReportFilterField DataField="Closed Tickets" Caption="MSR-Closed" UniqueName="ClosedCount">
                    
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridAggregateField DataField="Resolved Tickets" Caption="MSR-Resolved" UniqueName="ResolvedCount" Aggregate="Sum"  IgnoreNullValues="true">
                        <HeaderCellTemplate>
                            <asp:Label ID="lbl0" Text="Resolved" runat="server" />
                        </HeaderCellTemplate>
                </telerik:PivotGridAggregateField>

                <telerik:PivotGridReportFilterField DataField="Closed Backlog" Caption="MSR-Closed Backlog" UniqueName="ClosedBackLogCount" >
                </telerik:PivotGridReportFilterField>
                <telerik:PivotGridAggregateField DataField="Resolved Backlog" Caption="MSR-Resolved Backlog" UniqueName="ResolvedBackLogCount"  IgnoreNullValues="true"   Aggregate="Sum">
                    <HeaderCellTemplate>
                            <asp:Label ID="lbl0212" Text="Resolved Backlog" runat="server" />
                        </HeaderCellTemplate>
                </telerik:PivotGridAggregateField>

                <telerik:PivotGridReportFilterField  DataField="Cumulative Closed Backlog" Caption="MSR-Cumulative Closed Backlog"  UniqueName="CumulativeClosedBacklog">
                    
                </telerik:PivotGridReportFilterField>
                 
                <telerik:PivotGridAggregateField  DataField="Cumulative Resolved Backlog"
                    Caption="MSR-Cumulative Resolved Backlog" UniqueName="CumulativeResolvedBacklog"   IgnoreNullValues="true"  Aggregate="Max">
                    
                          <HeaderCellTemplate>
                            <asp:Label ID="lbl1" Text="Cumulative Resolved Backlog" runat="server" />
                        </HeaderCellTemplate>
                </telerik:PivotGridAggregateField>
            </Fields>
           <%--<ConfigurationPanelSettings Position="Left" DefaultDeferedLayoutUpdate="true" />--%>
          <ClientSettings EnableFieldsDragDrop="true">
              <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                <ClientMessages DragToReorder="Drag the fields here"></ClientMessages>
             </ClientSettings>
        </telerik:RadPivotGrid>

                    <div class="analysis-graph-wrapper">
                         <div class="graph-type">
                            <label for="ChartDrptype">Graph Type :</label>
                            <telerik:RadDropDownList ID="ChartDrptype" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                                            DataValueField="Value" DataTextField="Text" Width="150px" OnSelectedIndexChanged="ChartDrp_SelectedIndexChanged">
                            <Items>
                                <telerik:DropDownListItem Text="Line Graph" Value="Line" />
                                <telerik:DropDownListItem Text="Bar Graph" Value="Bar" />
                                <telerik:DropDownListItem Text="Line and Bar Graph" Value="Both" />
                            </Items>              
                            </telerik:RadDropDownList>
                          </div>

                         <telerik:RadHtmlChart ID="RadHtmlChart1" Transitions="true" Height="350px" runat="server" Visible="true" Legend-Appearance-Position="Bottom"  PlotArea-XAxis-LabelsAppearance-RotationAngle="45" PlotArea-XAxis-MinorGridLines-Visible="true" PlotArea-YAxis-MinorGridLines-Visible="false"  PlotArea-YAxis-MinValue="0" PlotArea-YAxis-Type="Numeric" >
                            <PlotArea>
                                <XAxis>
                                    <MajorGridLines Color="#EFEFEF"></MajorGridLines>
                                </XAxis>
                                <YAxis>
                                    <MajorGridLines Color="#EFEFEF"></MajorGridLines>
                                </YAxis>
                            </PlotArea>
                             <Pan  Enabled="true" />
                                <Zoom Enabled="true">
                                    <MouseWheel Enabled="true" />
                                    <Selection Enabled="true" ModifierKey="Shift" />
                                </Zoom>
                        </telerik:RadHtmlChart>
                    </div>     
 </telerik:RadAjaxPanel>                       
                </div>
            </div>
       </div>
     <%--</div>--%>
     
    </form>

        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <script type="text/javascript">
         //var $ = $telerik.$;

         function exportPDF() {
             $find('<%=RadClientExportManager1.ClientID%>').exportPDF($(".RadAjaxPanel"));
       }
         function exportImage() {
             $find('<%=RadClientExportManager1.ClientID%>').exportImage($(".RadAjaxPanel"));
         }

         //function closeProgressIndicator() {
         //    $("#progressIndicator").hide();
         //}
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
