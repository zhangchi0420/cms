
<%@ Page Title="设置角色" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetRole.aspx.cs" Inherits="Drision.Framework.Web.SetRole" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../AutoUserControls/HR_Common/T_Role_ViewControlF6.ascx" tagname="T_Role_ViewControlF6" tagprefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/SetRole.css")))
   { %>
<link href="../ClientCSS/SetRole.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/SetRole.js")))
   { %>
<script src="../ClientScripts/SetRole.js" type="text/javascript"></script>
<% } %>



    <div class="grid_title" id = "divViewControlTitle">角色列表</div>
<div id = "divViewControlF6" runat = "server">
    <asp:T_Role_ViewControlF6 ID = "ViewControlF6" runat="server"></asp:T_Role_ViewControlF6>
</div>

<div class="button_bar">
        <asp:LinkButton ID="OperationXK" runat="server"  UseSubmitBehavior = "false" Text="确定" onclick = "btnSure_Click" OnClientClick=""/>
        <asp:LinkButton ID="OperationC5" runat="server"  UseSubmitBehavior = "false" Text="取消" onclick = "btnCancel_Click" OnClientClick=""/>
</div>

</asp:Content>