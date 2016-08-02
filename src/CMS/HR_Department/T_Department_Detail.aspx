<%@ Page Title="组织详情页面" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Department_Detail.aspx.cs" Inherits="Drision.Framework.Web.T_Department_Detail" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Department_Detail.css")))
   { %>
<link href="../ClientCSS/T_Department_Detail.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Department_Detail.js")))
   { %>
<script src="../ClientScripts/T_Department_Detail.js" type="text/javascript"></script>
<% } %>
            <div class = "out_border">
            <div class="main_box"><h3>&nbsp;</h3>
<asp:TreeControl ID="tree" runat="server" ClientInstanceName="tree" OnNodeClick="tree_NodeClick" CssClass = "treebox" 
    OnAjaxLoading="tree_AjaxLoading" AutoPostBack="true" LoadMode = "Default" ExpandAllNodes = "True" TwoState=""  ShowCheckBox="False">
</asp:TreeControl><div class= "treeform">
</div>            <div class="cl"></div>
            </div>
            </div>

</asp:Content>