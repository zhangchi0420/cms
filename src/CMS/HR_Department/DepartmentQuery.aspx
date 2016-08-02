<%@ Page Title="部门查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartmentQuery.aspx.cs" Inherits="Drision.Framework.Web.DepartmentQuery" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/DepartmentQuery.css")))
   { %>
<link href="../ClientCSS/DepartmentQuery.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/DepartmentQuery.js")))
   { %>
<script src="../ClientScripts/DepartmentQuery.js" type="text/javascript"></script>
<% } %>
<asp:HiddenField ID="Hidden_SelectViewControlID" runat="server" />


<div class = "out_border">
<div class="main_box">
<h3 > 查询条件</h3>
<div class="allcol2">

    <div class = "item_box_col1">
    
    
    <span>组织名称 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "ctrl_Department_Code" runat="server" TextType = "String" FieldName = "Department_Name"  Tag = "8"   ControlId = "1000000064"    >  </sp:SText>

    </div>

</div>
<div class="cl"></div>
<div class="button_bar" style='display: none;'>
<asp:LinkButton ID="btnQueryInner" runat="server" onclick="btnQuery_Click" onprerender="btnQuery_PreRender" Text = "查询" CssClass = "btnQuery"/>
<asp:LinkButton ID="btnClearConditionInner" runat="server" onclick="btnClearCondition_Click" Text = "重置"/>
</div>
</div>
</div>
<div class="button_bar" >
<asp:LinkButton ID="btnQuery" runat="server" onclick="btnQuery_Click" onprerender="btnQuery_PreRender" Text = "查询" CssClass = "btnQuery"/>
<asp:LinkButton ID="btnClearCondition" runat="server" onclick="btnClearCondition_Click" Text = "重置"/>
</div>



<div class = "grid_title">
    <asp:Label ID = "lblViewControlTitle" runat = "server"></asp:Label>
    <div class="grid_filter">
            <asp:LinkButton ID="ViewItem66" runat="server" CommandArgument = "1000000381" Text="所有部门" OnClick = "btnLoadView_Click"></asp:LinkButton>
            <asp:LinkButton ID="ctrl_deptquery_viewlistitem" runat="server" CommandArgument = "1000000071" Text="停用部门" OnClick = "btnLoadView_Click"></asp:LinkButton>
            <asp:LinkButton ID="ViewItemWK" runat="server" CommandArgument = "1000000369" Text="启用部门" OnClick = "btnLoadView_Click"></asp:LinkButton>
    </div>
</div>
           <div id = "divviewcontrol46" runat = "server"></div>
           <div id = "divViewControlC2" runat = "server"></div>
           <div id = "divctrl_deptquery_view" runat = "server"></div>
</asp:Content>