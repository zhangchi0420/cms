<%@ Page Title="配置查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Configuration_Query.aspx.cs" Inherits="Drision.Framework.Web.T_Configuration_Query" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Configuration_Query.css")))
   { %>
<link href="../ClientCSS/T_Configuration_Query.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Configuration_Query.js")))
   { %>
<script src="../ClientScripts/T_Configuration_Query.js" type="text/javascript"></script>
<% } %>
<asp:HiddenField ID="Hidden_SelectViewControlID" runat="server" />





<div class = "grid_title">
    <asp:Label ID = "lblViewControlTitle" runat = "server"></asp:Label>
    <div class="grid_filter" style="display:none">
            <asp:LinkButton ID="ViewItemGP" runat="server" CommandArgument = "2000000307749" Text="配置列表" OnClick = "btnLoadView_Click"></asp:LinkButton>
    </div>
</div>
            <div id = "divviewcontrolSB" runat = "server" class="multigrid_box"></div>
</asp:Content>