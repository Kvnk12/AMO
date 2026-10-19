<%@ Page Language="C#" MasterPageFile="~/AnalysisMasterPage.Master" AutoEventWireup="true" CodeBehind="ReportViewerCPF.aspx.cs" Inherits="WebUI.Reports.ReportViewerCPF" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="headercontent" ContentPlaceHolderID="headertitle" runat="server">
     <span> <img alt="tool logo" src='<%=Page.ResolveUrl("~/Content/images/home/Client_Profile_manager_white.svg")%>'>Client Profile Manager</span>
    <style>
        #ctl00_ContentPlaceHolder1_ReportViewerControl_ctl09{
background:white;}
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<form runat ="server">
    <asp:HiddenField ID="forgeryToken" runat="server"/>
         <div class="content-main dark-bg">
             <div class="clear"></div>
            <ol class="breadcrumb">
                <li>Client Profile Manager</li>
                <li class="active">Reports</li>
            </ol>

             <div class="content-wrapper">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

   
    
           <rsweb:ReportViewer ID="ReportViewerControl" align="center" runat="server" Height="100%" Width="100%" SizeToReportContent="True" ZoomMode="PageWidth"></rsweb:ReportViewer>
     
    <script type="text/javascript">
        //to set ReportViewerControl Height/Width
        var id = "<%=ReportViewerControl.ClientID%>";
        document.getElementById(id).Width = "100%";
        document.getElementById(id).Height = "100%";
    </script>
         </div>
             </div>

</form>
</asp:Content>