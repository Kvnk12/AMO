<%@ Page Language="C#" MasterPageFile="~/SubscriptionMasterPage.Master" AutoEventWireup="true" CodeBehind="RptAnalysisSubscription.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.RptAnalysisSubscription" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="headercontent" ContentPlaceHolderID="headertitle" runat="server">
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

    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                <li class="active">Incident Analysis</li>`
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
    $("#export").click(function (e) {
        var pivotgrid = $("#pivotgrid").data("kendoPivotGrid");
        pivotgrid.saveAsExcel();
    });
    </script>

 </telerik:RadCodeBlock>

</asp:Content>
