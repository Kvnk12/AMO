<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="BackLogAnalysis.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.BackLogAnalysis" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
    .modalPopup{
        position: fixed !important;
    z-index: 10002 !important;
    top: 18.5px !important;
    left: 133px !important;
    width: 950px !important;
    height: 572px !important;
    background-color: white !important;
    text-align: left;
    overflow:auto;
    }
    hr {
        color: rgb(91,121,171);
        border-top: dashed 1.5px;
        width: 56.2em;
        margin-left: 13px;
    }
    .closeBtn{
        background-image:url(../../../Content/images/close.svg);
        background-repeat: no-repeat;
        background-size: 0.75em;
        height: 1em;
        width: 1em;
        margin-right: 0.5em;
    }
    .ui-datepicker.ui-widget.ui-widget-content{
    background: whitesmoke;
    padding: 1em;
    width: 14em;
    border: 1px solid saddlebrown;
}
table.ui-datepicker-calendar {
    width: 12.5em;
    margin-top: 1em;
}
.ui-datepicker-header.ui-widget-header.ui-helper-clearfix.ui-corner-all {
    display: flex;
    justify-content: space-between;
}
textarea {
    resize: none;
}
.RptpopUp .RPTDetails .radio-buttons input[type="radio"] {
    margin-top: 6px;
}
.selectDay .checkboxicon {
    background-size: 1em;
}
    .custom-blue-label {
        padding-right:1px;
    }
    .calenderIcon{
        background-image: url(../../../Content/images/calendar_blue.svg);
    background-repeat: no-repeat;
    background-size: 1.5em;
    height: 2em;
    width: 3em;
    margin: 0.5em;
    }
    span#ContentPlaceHolder1_RequiredFieldValidator1 {
        font-size: 10px;
        padding-left: 9em;
    }
    input[type=text] {
    border: none;
    border-bottom: 1px solid;
}
    div#ContentPlaceHolder1_UpdatePanel4 {
    padding-top: 6px;
}
   </style>


     <span> <img alt="tool logo" src='<%=Page.ResolveUrl("~/Content/images/incident_navigator_icon.png")%>'>Incident Navigator</span>
</asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <form id="form1" runat="server">
                <asp:HiddenField ID="forgeryToken" runat="server"/>
                <asp:HiddenField ID="selperiod" Value="Daily" runat="server"/>
                <asp:HiddenField ID="selWeek" Value="" runat="server"/>
                <asp:HiddenField ID="reportKey" Value="" runat="server"/>
                <asp:HiddenField ID="reportview" Value="" runat="server"/>
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
                        <span class="parameter-filter-icon"></span>
                        <div class="parameter-slider-overlay"></div>
                    </div>
                    <div class="parameter-slider">
                        <span class="close-parameters"></span>
                        <div id="invoiceIdSelector" class="parameter-list-main">
                            <table>
                                <tr style="padding-top:10px">
                                    <td>Saved Views :</td>
                                    <td>
                                    <telerik:RadDropDownList ID="doprdown1" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                         DataValueField="Value" DataTextField="Text" DefaultMessage="Select Report View..." Width="230" Font-Names="verdana, sans-serif" DropDownWidth="230" OnSelectedIndexChanged="setToRadPivot">
                    </telerik:RadDropDownList>
                                    </td>
                               </tr>
                                <tr style="padding-top:10px">
                                <td>Report View Name :</td>
                                <td><telerik:RadTextBox ID="ViewNameText" runat="server" Width="230px" CausesValidation="true" InputType="Text"/></td> 
                                </tr>
                                <tr style="padding-top:10px">
                                 <%--<td>
                                      Is Default View :
                                     <asp:CheckBox ID="defaultChekbox" runat="server"  />
                                </td>--%>
                                 <td class="parameter-slider-td">
                                     <span class="td-checkbox">
                                        <asp:CheckBox ID="PublicChekbox" runat="server"  /> 
                                         <label for="PublicChekbox"> Make Public</label>
                                     </span>
                                     <%--<td style="padding-top:10px">
                                    Classic View : <asp:CheckBox ID="chkClassicViewPivot"       OnCheckedChanged="chkClassicViewPivot_CheckedChanged" AutoPostBack="true" Checked="true" runat="server"  />--%>
                                    <telerik:RadButton ID="SaveView" runat="server" Primary="true"  OnClick="SaveViewDetails" Text="Save"/>
                                </td>               
                      
                            </tr>

                                 <tr>
                                        <td style="width:100px;padding-top:10px">
                                            From Date :</td>
                                        <td colspan="2" style="padding-top:3px" >
                                         <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDatePicker1" Width="150px" runat="server" CssClass="k-widget k-datepicker" >
                                            <DateInput runat="server"
                                                DateFormat="MMMM dd yyyy"
                                                DisplayDateFormat=" MMMM dd yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        </td>
                                        </tr>
                                    <tr><td style="width:100px;padding-top:10px">
                                        To Date :</td>
                                        <td colspan="2"  style="padding-top:3px">
                                             <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDatePicker2" Width="150px" runat="server" >
                                            <DateInput runat="server"
                                                DateFormat="MMMM dd yyyy"
                                                DisplayDateFormat=" MMMM dd yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        </td>
                                        </tr>
                        

                                <tr style="display:flex;justify-content: space-between;float: right;width: 9em;">                         
                                    <td class="parameter-slider-td single-item">                          
                                    <telerik:RadButton ID="ApplyButton" runat="server" Primary="true" OnClick="ApplyButtonClick" Text="Apply"/>
                                    </td>
                                     <td class="parameter-slider-td single-item">
                                             <telerik:RadButton ID="btnSubscribe" runat="server" Text="Subscribe" OnClientClicked="subscriptionClick"/>
                                        </td>
                                </tr>
                    </table>
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
           
            <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnSubscribe"
     BackgroundCssClass="modalstyle" Enabled="true">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" style = "display:none" >

    <div style="width:100%;height:3em;background-color:rgb(91,121,171);color:white;font-size:16px;font-family:'HelveticaNeue-Light';padding: 12px;display:flex;justify-content:space-between">
        <div>New Subscription for Analysis Report</div>
        <div id="btnClose" class="closeBtn" onclick="cancelPopup()" ></div>
    </div>
       <div class="RptpopUp" style="padding: 1em; padding-top: 0em;">
         <div class="RPTDetails">
            <div class="Emailsub_Recipients" style="height:11em">
                <div style="display:flex">
                  <p class="custom-blue-label" style="font-weight:bold">Email : </p>
                  <hr style="width:61em"/>
                </div>
                <div style="display:flex">
                <div class="firstrowFields" style=" padding-left: 8px;width: 32em;display:flex;">
                   <span class="custom-blue-label">Email Subject:</span>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:TextBox ID="Email_Subject" runat="server" Height="96px" Width="288px" TextMode="MultiLine"></asp:TextBox> 
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                 </div>   
                    <div class="email-content-bottom" style="">
                       <div class="email-heading" style="display: flex; margin-left: -50px; width: 37em; ">
                            <span class="custom-blue-label" style="margin-left: 1em;">Add Recipients : </span>
                           <asp:UpdatePanel runat="server" ID="UpdatePanel7" UpdateMode="Always">
                                <ContentTemplate>
                                    
                                    <asp:TextBox ID="EmailList" runat="server" TextMode="MultiLine" Height="96px" Width="329px"></asp:TextBox>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                               
                           <br />
                       
                       </div>
                    </div>
               
                </div>
                
                <div style="color:darkorange;font-size:10px;font-family:HelveticaNeue;width: 80.6em;text-align: right;">Note: For multiple Recipients add email ids separated by delimiter(;)</div>
                    
            </div>
             <div style="display:flex;" >
                <p class="custom-blue-label" style="font-weight:bold">Frequency :</p>
                <hr style="width:59em"/>
               </div>
             <div class="rpt_secondrowFields" id="rpt_secrowfields">
            <div class="ReportFor_ScheduledDate" >
                <div class="ReportFor" style="display: flex; margin-left: 10px;">
                    <span class="custom-blue-label" >Report For :</span>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:DropDownList ID="ReportFortxt" runat="server">
                                <asp:ListItem Enabled="true" Text="Current Week" Selected="True" Value="Current Week"></asp:ListItem>
                                <asp:ListItem Text= "Monthly" Value="Monthly"></asp:ListItem>
                                <asp:ListItem Text= "Quarter" Value="Quarter"></asp:ListItem>
                                <asp:ListItem Text= "Half Yearly" Value="Half Yearly"></asp:ListItem>
                                <asp:ListItem Text= "Yearly" Value="Yearly"></asp:ListItem>

                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <div style="display:flex" >
                    <p class="custom-blue-label" style="margin-left:5.2em;font-weight:bold">Schedule Duration :</p>
                    <span class="custom-blue-label" style="margin-left:1.7em">Start Date :</span>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:TextBox ID="StartDate" runat="server" Width="135px" AutoPostBack="False" TabIndex="1" placeholder="mm/dd/yyyy"
                                      autocomplete="off" MaxLength="10"></asp:TextBox>
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="calenderIcon"></div>
                    <span class="custom-blue-label" style="margin-left: 2.5em;">End Date :</span>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:TextBox ID="EndDate" runat="server" Width="135px" AutoPostBack="False" TabIndex="1" placeholder="mm/dd/yyyy"
                                      autocomplete="off" MaxLength="10"></asp:TextBox>
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                          <div class="calenderIcon"></div>
                   </div>
               </div>
           </div>
           
                 <div class="radio-buttons" style="display: flex;margin-bottom: 1em; margin-left: 10px;">
                 <span class="custom-blue-label" > Frequency :</span>
                 <div style="margin-top: 6px; margin-left: 6px;display:flex">
                  <input type="radio" style="margin-left: 2.2em; " id="daily" checked name="periodradio" value="Daily" onclick="onDailyClick()"/>
                     <div style="font-weight: unset; padding-left: 4px; padding-right: 10px; padding-top: 3px;">
                        Daily
                     </div>
                     <input type="radio"  id="weekly" name="periodradio" value="Weekly" onclick="onWeeklyClick()" />
                     <div style="font-weight: unset; padding-left: 4px; padding-right: 10px; padding-top: 3px;">
                        Weekly
                     </div>
                     <input type="radio"  id="monthly" name="periodradio" value="Monthly" onclick="onMonthlyClick()" />
                     <div style="font-weight: unset; padding-left: 4px; padding-right: 10px; padding-top: 3px;">
                        Monthly
                     </div>
               </div>
             </div>

             <div class="SelectTime" id="selectTimeHid" style="display: flex; margin-top: 9px; margin-left: 10px;">
                  <span class="custom-blue-label">Schedule Time : </span>

                 <div class="custom-dropdown" style="margin-left: 1em; margin-right: 1em; margin-top: 7px;">
                   <asp:DropDownList runat="server" ID="selectHours"></asp:DropDownList>
                  </div>

                     <div class="custom-dropdown" style="margin-left: 1em; margin-right: 1em; margin-top: 7px;">
                                   <asp:DropDownList runat="server" ID="selectMinutes"></asp:DropDownList>
                     </div>
                 <div>
                    
               </div>
                   <div>
                     <span class="custom-blue-label" style="margin-left: 2em; position: absolute; padding-top: 11px;">Select Time Zone : </span>
                  </div>
               <div class="custom-dropdown" style="margin-left: 9.5em; margin-right: 1em; margin-top: 7px;">
                 <asp:UpdatePanel runat="server" ID="UpdatePanel8" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:DropDownList ID="selectTimeZone" runat="server">
                                <asp:ListItem Enabled="true" Text="EST" Selected="True" Value="EST"></asp:ListItem>
                                <asp:ListItem Text= "PST" Value="PST"></asp:ListItem>
                                <asp:ListItem Text= "CST" Value="CST"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                     </div>
                <p style=" margin-top: 8px;">(24 hour format)</p>


              </div>

                <div class="SelectTime_Day" id="SelectTime_Day" style="display:none;">
                   <div class="selectDay" id="select_day" style="margin-left: 10px;">
                   <span class="custom-blue-label">Schedule Day :</span>

                   <input type="checkbox" value="1" class="day-of-week" style="width:0" id="0" name="dayOfWeek">
                   <label class="checkboxicon" style="margin-left: 11px;" for="1">Sunday</label>
                   <input type="checkbox" value="2" class="day-of-week" style="width:0" id="1"  name="dayOfWeek">
                   <label class="checkboxicon" for="2">Monday</label>
                   <input type="checkbox" value="3" class="day-of-week" style="width:0" id="2" name="dayOfWeek">
                   <label class="checkboxicon" for="3">Tuesday</label>
                   <input type="checkbox" value="4" class="day-of-week" style="width:0" id="3"  name="dayOfWeek">
                   <label class="checkboxicon" for="4">Wednesday</label>
                   <input type="checkbox" value="5" class="day-of-week" style="width:0" id="4"  name="dayOfWeek">
                   <label class="checkboxicon" for="5">Thursday</label>
                   <input type="checkbox" value="6" class="day-of-week" style="width:0"  id="5" name="dayOfWeek">
                   <label class="checkboxicon" for="6">Friday</label>
                   <input type="checkbox" value="7" class="day-of-week" style="width:0" id="6"  name="dayOfWeek">
                   <label class="checkboxicon" for="7">Saturday</label>
                   </div>

                    <div class="SelectTime" id="select_time" style="display: flex; margin-left: 10px;">
                         <span class="custom-blue-label">Schedule Time :</span>

                 <div class="custom-dropdown" style="margin-left: 1em; margin-right: 1em; margin-top: 7px;">
                    <asp:DropDownList runat="server" ID="selectHours_Weekly"></asp:DropDownList>
                        </div>

                     <div class="custom-dropdown" style="margin-left: 1em; margin-right: 1em; margin-top: 7px;">
                    <asp:DropDownList runat="server" ID="selectMinutes_Weekly"></asp:DropDownList>
                        </div>

                         <div>
                     <span class="custom-blue-label" style="margin-left: 2em; position: absolute; padding-top: 11px;">Select Time Zone : </span>
                  </div>
               <div class="custom-dropdown" style="margin-left: 9.5em; margin-right: 1em; margin-top: 7px;">
                 <asp:UpdatePanel runat="server" ID="UpdatePanel9" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:DropDownList ID="selectTimeZone1" runat="server">
                                <asp:ListItem Enabled="true" Text="EST" Selected="True" Value="EST"></asp:ListItem>
                                <asp:ListItem Text= "PST" Value="PST"></asp:ListItem>
                                <asp:ListItem Text= "CST" Value="CST"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                     </div>
                 <div>
                     <p style=" margin-top: 8px; font-weight: 600;">(24 hour format)</p>
                 </div>
                </div>
               </div>

                <div class="select-time-monthly" id="selectTimeMonthly" style="display: none; margin-left: 10px; }">
                  <span class="custom-blue-label">Month Date :</span>
				
                 <div class="custom-dropdown" style="margin-left: 2em; margin-right: 1em; margin-top: 7px;">
                        <asp:DropDownList runat="server" ID="selectMonth"></asp:DropDownList>
                  </div>
				
                  <span class="custom-blue-label" style="margin-left: 1em;">Schedule Time  : </span>

                 <div class="custom-dropdown" style="margin-left: 1em; margin-right: 1em; margin-top: 7px;">
                   <asp:DropDownList runat="server" ID="selectHours_Monthly"></asp:DropDownList>
                        </div>

                     <div class="custom-dropdown" style="margin-left: 1em; margin-right: 1em; margin-top: 7px;">
                    <asp:DropDownList runat="server" ID="selectMinutes_Monthly"></asp:DropDownList>
                        </div>
                  <div>
                     <span class="custom-blue-label" style="margin-left: 2em; position: absolute; padding-top: 11px;">Select Time Zone : </span>
                  </div>
                  <div class="custom-dropdown" style="margin-left: 9.5em; margin-right: 1em; margin-top: 7px;">
                  <asp:DropDownList runat="server" ID="selectTimeZone2">
                      <asp:ListItem Enabled="true" Text="EST" Selected="True" Value="EST"></asp:ListItem>
                                <asp:ListItem Text= "PST" Value="PST"></asp:ListItem>
                                <asp:ListItem Text= "CST" Value="CST"></asp:ListItem>
                  </asp:DropDownList>
                     </div>
                 <div>
                     <p style=" margin-top: 8px;">(24 hour format)</p>
                 </div>
              

                 </div>
            </div>
             <div style="display: flex; margin-top: 2px" >
                <p class="custom-blue-label" style="font-weight:bold">Reporting Criteria :</p>
                <hr class="delim_criteria" />
               </div>

             <div class="FilterCriteria_Rptformat" id="FiltertxtArea" style="display:flex">
                 <p class="custom-blue-label" style=" margin-left: 10px;">Selected Filter Criteria :  </p>
                  <asp:TextBox ID="edit_Description" runat="server" Height="68px" Width="500px" TextMode="MultiLine" Enabled="false" BackColor="LightGray"></asp:TextBox> 

             </div>


    <div class="kedb-buttons" style="padding:unset">
    <div style="margin: 0 1em 0 0;">

        <button class="custom-button" type="button" style="background-color:lightgrey;color:white"onclick="cancelPopup()"  >CANCEL</button>
    </div>
    <div id="saveForsub" style="margin: 0 1.5em 0 0;">
        <asp:Button ID="savebtn" runat="server" Text="SAVE" OnClick="saveSubscrClick" onclientclick="disableButton()" CssClass="custom-button"/>
    </div>
   
    </div>

</div>
           </div>
</asp:Panel>
                
      
                
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
 $('.parameter-filter-icon').on('click', function(){
        $(this).addClass('open');
        $('.parameter-slider-overlay').addClass('open');
        $('.parameter-slider').addClass('open');
    });
    $('.close-parameters').on('click', function(){
        $('.parameter-filter-icon').removeClass('open');
        $('.parameter-slider-overlay').removeClass('open');
        $('.parameter-slider').removeClass('open');
    });
    $('.parameter-slider-overlay').click(function () {
        $('.parameter-filter-icon').removeClass('open');
        $('.parameter-slider-overlay').removeClass('open');
        $('.parameter-slider').removeClass('open');
    });


         function onDailyClick() {
             document.getElementById('selectTimeMonthly').style.display = "none";
             document.getElementById('SelectTime_Day').style.display = "none";
             document.getElementById('selectTimeHid').style.display = "block";
             document.getElementById('selectTimeHid').style.display = "flex";
             document.getElementById('rpt_secrowfields').style.height = "10em";
             document.getElementById('select_time').style.marginTop = "0em";
             document.getElementById('select_day').style.marginTop = "0em";
             document.getElementById('<%=selperiod.ClientID%>').value = "Daily";
         }

         function onWeeklyClick() {
             document.getElementById('selectTimeMonthly').style.display = "none";
             document.getElementById('selectTimeHid').style.display = "none";
             document.getElementById('SelectTime_Day').style.display = "block";
             document.getElementById('rpt_secrowfields').style.height = "12em";
             document.getElementById('select_time').style.marginTop = "1em";
             document.getElementById('select_day').style.marginTop = "1.2em";
             document.getElementById('<%=selperiod.ClientID%>').value = "Weekly";
         }

         function onMonthlyClick() {
             document.getElementById('SelectTime_Day').style.display = "none";
             document.getElementById('selectTimeHid').style.display = "none";
             document.getElementById('selectTimeMonthly').style.display = "block";
             document.getElementById('selectTimeMonthly').style.display = "flex";
             document.getElementById('rpt_secrowfields').style.height = "10em";
             document.getElementById('select_time').style.marginTop = "0em";
             document.getElementById('select_day').style.marginTop = "0em";
             document.getElementById('<%=selperiod.ClientID%>').value = "Monthly";
         }

         $(function () {
             document.getElementById('<%= savebtn.ClientID %>').style.display = "unset";

             $("#<%= StartDate.ClientID %>").datepicker();
             $("#<%= EndDate.ClientID %>").datepicker();
             const d = new Date();
             let day = d.getDay()
             document.getElementById(day).checked = true;
    });

    function cancelPopup() {
        $find('<%= mp1.ClientID%>').hide();
        location.reload();
    }
    $('input[type="checkbox"]').on('change', function () {
        $('input[name="' + this.name + '"]').not(this).prop('checked', false);
        document.getElementById('<%=selWeek.ClientID%>').value = this.value;
    });
    function subscriptionClick() {
        console.log("subscribe click");
        var reportview = $("#<%= reportview.ClientID %>").val();

        console.log(reportview);

        var reportParameter = " ReportView: " + reportview;
        console.log(reportParameter);
             document.getElementById('<%=edit_Description.ClientID %>').value = reportParameter;
         }

         function disableButton() {
            // console.log("save click");
             //document.getElementById('<%= savebtn.ClientID %>').disabled = true;
            // $find('<%= savebtn.ClientID %>').hide();
             document.getElementById('<%= savebtn.ClientID %>').style.display = "none";

         }

     </script>
            </telerik:RadCodeBlock>

        </asp:Content>