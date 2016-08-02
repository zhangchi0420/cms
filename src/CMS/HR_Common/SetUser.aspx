
<%@ Page Title="设置用户" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetUser.aspx.cs" Inherits="Drision.Framework.Web.SetUser" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../AutoUserControls/HR_Common/T_User_viewcontrolXS.ascx" tagname="T_User_viewcontrolXS" tagprefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/SetUser.css")))
   { %>
<link href="../ClientCSS/SetUser.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/SetUser.js")))
   { %>
<script src="../ClientScripts/SetUser.js" type="text/javascript"></script>
<% } %>



    <div class="grid_title" id = "divViewControlTitle">用户列表</div>
<div id = "divviewcontrolXS" runat = "server">
    <asp:T_User_viewcontrolXS ID = "viewcontrolXS" runat="server"></asp:T_User_viewcontrolXS>
</div>

<div class="button_bar">
        <asp:LinkButton ID="btnMMRelationSaveJ6" runat="server"  UseSubmitBehavior = "false" Text="确定" onclick = "btnMMRelationSaveJ6_Click" OnClientClick="return ShowWaiting(true);"/>
        <asp:LinkButton ID="btnReturnWA" runat="server"  UseSubmitBehavior = "false" Text="取消" onclick = "btnReturnWA_Click" OnClientClick="return ShowWaiting(false);"/>
</div>

</asp:Content>