<%@ Page Language="C#"  MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="AnalysisNew.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.AnalysisNew" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Assembly="Syncfusion.Core.XForms" Namespace="Syncfusion.Core.XForms" TagPrefix="ej" %>--%>

<asp:Content ID="headercontent" ContentPlaceHolderID="headertitle" runat="server">
<style type="text/css">
    /*.RadPivotGrid .rpgFieldItem {
        margin: 4px;
        display: inline-block !important;
    }
    .RadPivotGrid.RadPivotGrid_Default {
        border: none;
        margin-top: 24px;
    }*/

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
        padding-right:2px;
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
                <li class="active">Incident Analysis</li>
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
                            <telerik:RadButton RenderMode="Lightweight" ID="RadButtonDrillDown" CssClass="btnDrilldownExportExcel" runat="server"
                                            OnClick="ButtonExcelDrillDown_Click" AlternateText="Xlsx" />
                        
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
                                            Saved View :
                                             <telerik:RadDropDownList ID="doprdown1" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                                            DataValueField="Value" DataTextField="Text" DefaultMessage="Select Report View..." Width="250" Font-Names="verdana, sans-serif" DropDownWidth="250" OnSelectedIndexChanged="setToRadPivot">
                                        </telerik:RadDropDownList>
                                        </td>
                                        </tr>
                                        <tr>
                                        <td style="padding-top:10px"  colspan="3">
                                            Report View Name :
                                        <telerik:RadTextBox ID="ViewNameText" runat="server" Width="250px" CausesValidation="true" InputType="Text" />
                                        </td>
                                        </tr>
                                       <tr class="button-group"> 
                                 <td class="parameter-slider-td">
                                     <span class="td-checkbox">
                                        <asp:CheckBox ID="PublicChekbox" runat="server"/>
                                         <label for="PublicChekbox"> Make Public</label>
                                      </span>
                                     <telerik:RadButton ID="RadButton1" runat="server" Primary="true" OnClick="SaveViewDetails" Text="Save" />
                                </td>
                            </tr>
                                    <tr>
                                        <td style="width:100px;padding-top:10px">
                                            From Date :</td>
                                        <td colspan="2" style="padding-top:3px" >
                                         <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDatePicker1" Width="150px" runat="server" CssClass="k-widget k-datepicker" >
                                            <DateInput runat="server"
                                                DateFormat="MMM dd yyyy"
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
                                                DateFormat="MM dd yyyy"
                                                DisplayDateFormat=" MMMM dd yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        </td>
                                        </tr>
                                    <tr>
                                        <td style="width:100px;padding-top:10px">
                                            Date Type :</td>
                                        <td colspan="2" style="padding-top:3px"> <telerik:RadComboBox RenderMode="Lightweight" ID="DateTypedrp" runat="server"  Width="150px">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Created" Value="CreatedDateTime" />

                                                <telerik:RadComboBoxItem Text="Closed" Value="ClosedDateTime" />
                                                <telerik:RadComboBoxItem Text="Last Updated" Value="LastUpdatedDateTime" />
                                                <telerik:RadComboBoxItem Text="Resolved" Value="ResolutionDateTime" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        </td>
                                        </tr>
                                    <tr class="button-group" style="display:flex;justify-content: space-between;float: right;width: 9em;">
                                        <td class="parameter-slider-td single-item">
                                             <telerik:RadButton ID="RadButtonApply" runat="server" Primary="true" OnClick="RadButtonApply_Click1" Text="Apply" />
                                        </td>
                                     
                                        <td class="parameter-slider-td single-item">
                                             <telerik:RadButton ID="btnSubscribed" runat="server" Text="Subscribe" OnClientClicked="subscriptionClick"/>
                                        </td>
                                   </tr>
                                </table>
                                <ul>
                                </ul>
                            </div>
                        </div>
                    
                    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
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
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="RadButtonApply">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                                     <telerik:AjaxUpdatedControl ControlID="RadHtmlChart1"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="SaveView">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />

                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="doprdown1">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                                     <telerik:AjaxUpdatedControl ControlID="RadHtmlChart1"  />
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
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="form1">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                     <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default" EnableSkinTransparency="true">
                    </telerik:RadAjaxLoadingPanel>
                        <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
                            <PersistenceSettings>
                                <telerik:PersistenceSetting ControlID="RadPivotGrid1" />
                            </PersistenceSettings>
                        </telerik:RadPersistenceManager>
                     
                      <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" PostBackControls="doprdown1,ViewNameText,PublicChekbox,RadButtonApply,RadButton1,DateTypedrp,RadDatePicker2,RadDatePicker1,RadPivotGrid1,ChartDrptype,RadHtmlChart1,RadPersistenceManager1,btnExportpdf,btnExportimage,btnExportExcel,RadClientExportManager1,RadAjaxLoadingPanel1,RadAjaxManager1,RadScriptManager1" >

                        <telerik:RadPivotGrid RenderMode="Lightweight" runat="server" ID="RadPivotGrid1" Font-Size="Smaller" 
                            Font-Names="verdana,sans-serif" AllowPaging="true" OnNeedDataSource="RadPivotGrid1_NeedDataSource" OnCellDataBound="RadPivotGrid1_CellDataBound" 
                            AllowSorting="true" AllowFiltering="true" PageSize="50" TotalsSettings-RowsSubTotalsPosition="None" 
                            TotalsSettings-RowGrandTotalsPosition="Last" TotalsSettings-ColumnsSubTotalsPosition="None" 
                            RowGroupsDefaultExpanded="true" ColumnGroupsDefaultExpanded="true" Width="100%" ShowFilterHeaderZone="true" 
                            EnableZoneContextMenu="true" OnPivotGridCellExporting="RadPivotGrid1_PivotGridCellExporting">
                            <PagerStyle Mode="NumericPages" AlwaysVisible="true"  />

                            <Fields>

                                <telerik:PivotGridColumnField DataField="Status">
                                </telerik:PivotGridColumnField>
                                <telerik:PivotGridRowField DataField="Priority">
                                </telerik:PivotGridRowField>
                                 <telerik:PivotGridReportFilterField DataField="ClientPriority" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Urgency" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Type" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Outagetype" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Divisionimpacted" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Businessunitimpacted" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicecategory" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicesubcategory" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicearea" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Regionimpacted" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Locationsimpacted" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Customerdept" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Customerlastname" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Customerfirstname" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Customertitle" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Customersatisfaction" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Impacttype" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Configitemid" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Configtype" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Configitemname" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Environmentimpacted" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Module" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Rootcausetype" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolutiontype" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Solutionid" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Deploymentrelease" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Relatedchangeids" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Relatedids" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Relatedproblemids" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Helpdesk" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Assignedtoperson" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Assignedtosupportgroup" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Createdbygroup" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Createdbyperson" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Impactscope" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <%--<telerik:PivotGridReportFilterField DataField="Modificationnotes" IsHidden="true" >
                        </telerik:PivotGridReportFilterField>--%>
                                <telerik:PivotGridReportFilterField DataField="Esitmatedresolutiondate" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Assigneddatetime" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Createdtime" IsHidden="true" Caption="Created Time">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Createdweekday" IsHidden="true" Caption="Created Week Day" SortOrder="Ascending">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Createdweeknum" IsHidden="true" Caption="Created Week Num">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Createdmonth" IsHidden="true" Caption="Created Month" SortOrder="Ascending">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Createdyear" IsHidden="true" Caption="Created Year">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Acknowlededbygroup" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Acknowlededbyperson" IsHidden="true">
                                </telerik:PivotGridReportFilterField>

                                <telerik:PivotGridReportFilterField DataField="Acknowledgedtime" IsHidden="true" Caption="Acknowledged Time">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Acknowledgedweekday" IsHidden="true" Caption="Acknowledged Week Day" SortOrder="Ascending">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Acknowledgedweeknum" IsHidden="true" Caption="Acknowledged Week Num">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Acknowledgedmonth" IsHidden="true" Caption="Acknowledged Month" SortOrder="Ascending">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Acknowledgedyear" IsHidden="true" Caption="Acknowledged Year">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolvedbygroup" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolvedbyperson" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolvedbyvendor" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolutionefforthours" Caption="Resolution hours" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolutiontime" Caption="Total Resolution effort" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolutionweekday" IsHidden="true" Caption="Resolution Week Day" SortOrder="Ascending">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolutionweeknum" IsHidden="true" Caption="Resolution Week Num">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolutionyear" IsHidden="true" Caption="Resolution Year">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolutionmonth" IsHidden="true" Caption="Resolution Month" SortOrder="Ascending">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ResponseTimeMetSLA" IsHidden="true" Caption="Resolution Met(SLA)">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resptargettime" IsHidden="true" Caption="Response target time">
                                </telerik:PivotGridReportFilterField>



                                <telerik:PivotGridReportFilterField DataField="Modificationnotes" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ResolutionExceptionReason" IsHidden="true" Caption="Resolution Exception Reason" SortOrder="None">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ResponseExceptionReason" IsHidden="true" Caption="Response Exception Reason" SortOrder="None">
                                </telerik:PivotGridReportFilterField>


                                <telerik:PivotGridReportFilterField DataField="Servicerestoredbygroup" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicerestoredbyperson" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicerestoredtime" IsHidden="true" Caption="Service Restored Time">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicerestoredweekday" IsHidden="true" Caption="Service Restored Week Day" SortOrder="Ascending">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicerestoredweeknum" IsHidden="true" Caption="Service Restored Week Num">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicerestoredmonth" IsHidden="true" Caption="Service Restored Month" SortOrder="Ascending">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicerestoredyear" IsHidden="true" Caption="Service Restored Year">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Assignedbygroup" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Assignedbyperson" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Esitmatedresponsetimebygroup" IsHidden="true">
                                </telerik:PivotGridReportFilterField>

                                <telerik:PivotGridReportFilterField DataField="Waivedindicator" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Totalsuspenddurationassignedtoack" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Totalsuspenddurationacktoer" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Totalsuspenddurationertoresolution" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolvedindicator" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="IsNTTGroup" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="SupportGroupType" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Scheduledchangecount" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Company" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Site" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="TicketId" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Title" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <%--<telerik:PivotGridReportFilterField DataField="Description" IsHidden="true" >
                        </telerik:PivotGridReportFilterField>--%>
                                <telerik:PivotGridReportFilterField DataField="Datasourcenotes" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <%--                 <telerik:PivotGridReportFilterField DataField="Resolutiondescription" IsHidden="true" >
                        </telerik:PivotGridReportFilterField>--%>
                                <telerik:PivotGridReportFilterField DataField="Rootcausedescription" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Assigneecompany" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Assigneeorganization" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Estimatedworkeffort" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Esitmatedresponsedatetime" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Actualworkeffort" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Recyclecount" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ConfigItemCategory" IsHidden="true">
                                </telerik:PivotGridReportFilterField>

                                <telerik:PivotGridReportFilterField DataField="ClosureCategory" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Severity" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="WorkGroupLevel" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Datasource" IsHidden="true">
                                </telerik:PivotGridReportFilterField>

                                <telerik:PivotGridReportFilterField DataField="Statusreason" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="AffectedItemName" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="SLACostPerTicket" IsHidden="true" Caption="Total Ticket Cost">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="INResolutiontime" IsHidden="true" Caption="Total Resolution effort">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="INCostPerTicket" IsHidden="true" Caption="Total Ticket Cost">
                                </telerik:PivotGridReportFilterField>


                                <telerik:PivotGridReportFilterField DataField="Closeddatetime" DataFormatString="{0:d}" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ResolutionDate" DataFormatString="{0:d}" IsHidden="true">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Lastupdatedbyperson" IsHidden="true" Caption="Last updated by">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Lastupdateddatetime" IsHidden="true" Caption="Last updated">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Nodename" IsHidden="true" Caption="Node Name">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Createdtimezone" IsHidden="true" Caption="Created time zone">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Acknowledgedtimezone" IsHidden="true" Caption="Acknowledged time zone">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Resolvedtimezone" IsHidden="true" Caption="Resolved time zone">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Servicerestoredtimezone" IsHidden="true" Caption="Service restored time zone">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ScheduledstartdateTime" IsHidden="true" Caption="Scheduled Start Date">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ScheduledenddateTime" IsHidden="true" Caption="Scheduled Start Time">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Scheduledchangecount" IsHidden="true" Caption="Scheduled Change Count">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="InProgressdatetime" IsHidden="true" Caption="InProgress Date">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ActualstartdateTime" IsHidden="true" Caption="Actual start Date">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="AccountName" IsHidden="false" Caption="Account Name">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ContractName" IsHidden="false" Caption="Contract Name">
                                </telerik:PivotGridReportFilterField>

                                <%--added by DP--%>
                                <telerik:PivotGridReportFilterField DataField="Active" IsHidden="true" Caption="Active"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Activityduedatetime" IsHidden="true" Caption="Activityduedatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ActualenddateTime" IsHidden="true" Caption="ActualenddateTime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Additionalassigneelist" IsHidden="true" Caption="Additionalassigneelist"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Approval" IsHidden="true" Caption="Approval"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Approvalhistory" IsHidden="true" Caption="Approvalhistory"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Approvalsetdatetime" IsHidden="true" Caption="Approvalsetdatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Businessduration" IsHidden="true" Caption="Businessduration"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Closenotes" IsHidden="true" Caption="Closenotes"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Closedby" IsHidden="true" Caption="Closedby"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Commentsworknotes" IsHidden="true" Caption="Commentsworknotes"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Correlationdisplay" IsHidden="true" Caption="Correlationdisplay"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="CorrelationID" IsHidden="true" Caption="CorrelationID"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Deliveryplan" IsHidden="true" Caption="Deliveryplan"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Deliverytask" IsHidden="true" Caption="Deliverytask"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Domain" IsHidden="true" Caption="Domain"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Domainpath" IsHidden="true" Caption="Domainpath"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Escalation" IsHidden="true" Caption="Escalation"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Followupdatetime" IsHidden="true" Caption="Followupdatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Grouplist" IsHidden="true" Caption="Grouplist"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Knowledge" IsHidden="true" Caption="Knowledge"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Location" IsHidden="true" Caption="Location"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="MadeSLA" IsHidden="true" Caption="MadeSLA"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Openeddatetime" IsHidden="true" Caption="Openeddatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Openedby" IsHidden="true" Caption="Openedby"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Order" IsHidden="true" Caption="Order"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ParentticketID" IsHidden="true" Caption="ParentticketID"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Reassignmentcount" IsHidden="true" Caption="Reassignmentcount"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="SLAduedatetime" IsHidden="true" Caption="SLAduedatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Tags" IsHidden="true" Caption="Tags"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Timeworked" IsHidden="true" Caption="Timeworked"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Updatescount" IsHidden="true" Caption="Updatescount"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Uponapprovalstep" IsHidden="true" Caption="Uponapprovalstep"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Uponrejectstep" IsHidden="true" Caption="Uponrejectstep"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Userinput" IsHidden="true" Caption="Userinput"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Watchlist" IsHidden="true" Caption="Watchlist"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Worknotes" IsHidden="true" Caption="Worknotes"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Worknoteslist" IsHidden="true" Caption="Worknoteslist"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Businessresolvetime" IsHidden="true" Caption="Businessresolvetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Caller" IsHidden="true" Caption="Caller"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Causedbychange" IsHidden="true" Caption="Causedbychange"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ChildIncidentscount" IsHidden="true" Caption="ChildIncidentscount"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Incidentstate" IsHidden="true" Caption="Incidentstate"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Lastreopenedatdatetime" IsHidden="true" Caption="Lastreopenedatdatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Lastreopenedby" IsHidden="true" Caption="Lastreopenedby"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Notify" IsHidden="true" Caption="Notify"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ParentIncidentID" IsHidden="true" Caption="ParentIncidentID"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="TotalResolvetime" IsHidden="true" Caption="TotalResolvetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Knownerror" IsHidden="true" Caption="Knownerror"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Majorproblem" IsHidden="true" Caption="Majorproblem"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Problemstate" IsHidden="true" Caption="Problemstate"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="RelatedIncidentscount" IsHidden="true" Caption="RelatedIncidentscount"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Workaround" IsHidden="true" Caption="Workaround"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Backoutplan" IsHidden="true" Caption="Backoutplan"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="CABdatetime" IsHidden="true" Caption="CABdatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="CABdelegate" IsHidden="true" Caption="CABdelegate"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="CABrecommendation" IsHidden="true" Caption="CABrecommendation"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="CABrequired" IsHidden="true" Caption="CABrequired"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Changeplan" IsHidden="true" Caption="Changeplan"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Closecode" IsHidden="true" Caption="Closecode"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Conflictlastrundatetime" IsHidden="true" Caption="Conflictlastrundatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Conflictstatus" IsHidden="true" Caption="Conflictstatus"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Implementationplan" IsHidden="true" Caption="Implementationplan"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Onhold" IsHidden="true" Caption="Onhold"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="OnHoldChangeTasks" IsHidden="true" Caption="OnHoldChangeTasks"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Outsidemaintenanceschedule" IsHidden="true" Caption="Outsidemaintenanceschedule"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Phase" IsHidden="true" Caption="Phase"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Phasestate" IsHidden="true" Caption="Phasestate"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Productionsystem" IsHidden="true" Caption="Productionsystem"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Reason" IsHidden="true" Caption="Reason"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Requestedby" IsHidden="true" Caption="Requestedby"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Requestedbydatetime" IsHidden="true" Caption="Requestedbydatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Reviewcomments" IsHidden="true" Caption="Reviewcomments"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Reviewdatetime" IsHidden="true" Caption="Reviewdatetime"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Reviewstatus" IsHidden="true" Caption="Reviewstatus"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Risk" IsHidden="true" Caption="Risk"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Changerequestscope" IsHidden="true" Caption="Changerequestscope"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Changerequesttype" IsHidden="true" Caption="Changerequesttype"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Changerequesttype" IsHidden="true" Caption="Changerequesttype"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Tower" IsHidden="false" Caption="Tower"></telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="Age" IsHidden="false" Caption="Age"></telerik:PivotGridReportFilterField>
                                <%--end--%>

                                <telerik:PivotGridReportFilterField DataField="CreatedDate" IsHidden="false" Caption="Created Date">
                                </telerik:PivotGridReportFilterField>
                                <telerik:PivotGridReportFilterField DataField="ResolvedDate" IsHidden="false" Caption="Resolved Date">
                                </telerik:PivotGridReportFilterField>

                                <telerik:PivotGridAggregateField DataField="Incidentid" IgnoreNullValues="true" ZoneIndex="0" Caption="Total Tickets" Aggregate="Count">
                                </telerik:PivotGridAggregateField>



                            </Fields>

                            <ClientSettings EnableFieldsDragDrop="true">

                                <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                                <ClientMessages DragToReorder="Drag the field to change its order"></ClientMessages>
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
          
            <!-- ModalPopupExtender -->
<cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnSubscribed"
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
                                    <%--<asp:TextBox ID="EmailList" runat="server" Width="275px" /> --%>
                                    <asp:TextBox ID="EmailList" runat="server" TextMode="MultiLine" Height="96px" Width="329px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="10"  
            EnableCaching="false" CompletionSetCount="10" TargetControlID="EmailList" ID="AutoCompleteExtender1"  
            runat="server" FirstRowSelected="false">  
        </cc1:AutoCompleteExtender>
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
    </form>

        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script type="text/javascript">

    //var $ = $telerik.$;

    function exportPDF() {
        //document.getElementById('exportSectionID').contentWindow.print();
        // window.print();
        $find('<%=RadClientExportManager1.ClientID%>').exportPDF($(".RadAjaxPanel"));
       // $find('<%=RadClientExportManager1.ClientID%>').exportPDF($(".RadHtmlChart"));
    }

    function exportImage() {
        $find('<%=RadClientExportManager1.ClientID%>').exportImage($(".RadAjaxPanel"));
    }

    function ApplyDetails() {
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

    $("#export").click(function (e) {
        var pivotgrid = $("#pivotgrid").data("kendoPivotGrid");
        pivotgrid.saveAsExcel();
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

        var DateTypedrp = $find('<%=DateTypedrp.ClientID %>');
        var DateTypedrpValue = DateTypedrp.get_selectedItem().get_value();

        var reportParameter = "DateType: " + DateTypedrpValue + "; ReportView: " + reportview;
        console.log(reportParameter);
        document.getElementById('<%=edit_Description.ClientID %>').value = reportParameter;
    }
    function disableButton() {
        document.getElementById('<%= savebtn.ClientID %>').style.display = "none";

    }
</script>

 </telerik:RadCodeBlock>

        </asp:Content>
