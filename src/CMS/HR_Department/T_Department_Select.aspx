<%@ Page Title="组织选择页面" Language="C#" MasterPageFile="~/Select.Master" AutoEventWireup="true" CodeBehind="T_Department_Select.aspx.cs" Inherits="Drision.Framework.Web.T_Department_Select" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Department_Select.css")))
   { %>
<link href="../ClientCSS/T_Department_Select.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Department_Select.js")))
   { %>
<script src="../ClientScripts/T_Department_Select.js" type="text/javascript"></script>
<% } %>
<asp:TreeControl ID="tree" runat="server" ClientInstanceName="PopUpValueText" OnNodeClick="tree_NodeClick" CssClass = "alltreebox" 
    OnAjaxLoading="tree_AjaxLoading" AutoPostBack="true" LoadMode = "Default" ExpandAllNodes = "True" TwoState=""  ShowCheckBox="False">
</asp:TreeControl>
</asp:Content>