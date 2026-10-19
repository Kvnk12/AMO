<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncidentAnalysis.aspx.cs" Inherits="WebUI.Areas.IncidentNavigator.AspxReport.IncidentAnalysis" %>

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
       div.RadPivotGrid .rpgFieldItem.aggregateCustom1, div.RadPivotGrid .rpgColumnHeader.aggregateCustom1 {
    color: #006699;
}
 
              div.RadPivotGrid .rpgDataCell{
           text-align:right;
       }

div.RadPivotGrid .rpgFieldItem.aggregateCustom2, div.RadPivotGrid .rpgColumnHeader.aggregateCustom2 {
    color: #006666;
}

.tableCssStyle{
   /*width: 100%;*/
   /*padding: 20px;*/
   /*background-color: #ebebeb;
   border-radius: 4px;*/  
   font-family: verdana, sans-serif;
   font-size: smaller;
   font-weight:bold;
}
#form1 .RadDropDownList .rddlEmptyMessage,#form1 .RadDropDownList .rddlDefaultMessage {
    opacity: 1;
    font-style: normal;
}


.demo-settings {
    margin-bottom: 20px;
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
    <form id="form1" runat="server">
             <table  class="tableCssStyle">
                <tbody>
                    <tr>
                        <td class="auto-style1">
                            <telerik:RadDropDownList ID="doprdown1" RenderMode="Lightweight" runat="server" AutoPostBack="true"
                 DataValueField="Value" DataTextField="Text" DefaultMessage="Select Report View..." Width="250" Font-Names="verdana, sans-serif" DropDownWidth="250"  OnSelectedIndexChanged="setToRadPivot">
            </telerik:RadDropDownList>
                        </td>
                       
                        <td style="padding-left:2em;" class="auto-style2">
                             Report View Name :
                        
                            <telerik:RadTextBox ID="ViewNameText" runat="server" Width="250px" CausesValidation="true" InputType="Text"/>
                        </td>  
                        <td style="padding-left:1em;">
                            Make Public :
                       
                             <asp:CheckBox ID="PublicChekbox" runat="server"  />
                        </td> 
                         <td style="padding-left:1em;">
                            Is Default View :
                       
                             <asp:CheckBox ID="defaultChekbox" runat="server"  />
                        </td>                     
                        <td style="padding-left:2em;">
                             &nbsp;</td>
                        <td></td>
                        <td></td>
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
                             <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export Excel" 
        OnClick="ButtonExcel_Click" AlternateText="Xlsx" />
                        </td>
                </tr>
            </table>
            </div>

    <div>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadPivotGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="RadHtmlChart1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadPersistenceManager runat="server" ID="RadPersistenceManager1">
        <PersistenceSettings>
            <telerik:PersistenceSetting ControlID="RadPivotGrid1" />
        </PersistenceSettings>
    </telerik:RadPersistenceManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="demo-container">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <div class="demo-settings">
                <telerik:RadButton RenderMode="Lightweight" ID="SaveButton" runat="server" Text="Save state" OnClick="SaveViewDetails" />
                <telerik:RadButton RenderMode="Lightweight" ID="ResetButton" runat="server" Text="Reset stored state" OnClick="ResetButton_Click" />
                <telerik:RadButton RenderMode="Lightweight" ID="LoadButton" runat="server" Text="Load state" OnClick="LoadButton_Click" />
            </div>
            <telerik:RadPivotGrid RenderMode="Lightweight" ID="RadPivotGrid1" runat="server" AllowFiltering="true"
                AllowPaging="false" ShowFilterHeaderZone="false" AllowSorting="true" EnableConfigurationPanel="true">
                <Fields>
                <telerik:PivotGridColumnField DataField="Location">
                </telerik:PivotGridColumnField>
                <telerik:PivotGridRowField DataField="Track">
                </telerik:PivotGridRowField>
                   
                     <telerik:PivotGridAggregateField DataField="Incidentid" ZoneIndex="0" Caption="Total Tickets"    Aggregate="Count" ></telerik:PivotGridAggregateField>
                </Fields>
                <ConfigurationPanelSettings Position="Left" DefaultDeferedLayoutUpdate="true" />
            </telerik:RadPivotGrid>
        </telerik:RadAjaxPanel>
    </div>
    </div>
       
 <br />
    <br />
    <telerik:RadHtmlChart ID="RadHtmlChart1" runat="server" Visible="true" Legend-Appearance-Position="Top"
        PlotArea-YAxis-LabelsAppearance-DataFormatString="C" PlotArea-XAxis-LabelsAppearance-RotationAngle="45">
    </telerik:RadHtmlChart>
    </form>
</body>


</html>
