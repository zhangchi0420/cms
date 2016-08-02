<%@ Page Title="角色查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleQuery.aspx.cs" Inherits="Drision.Framework.Web.RoleQuery" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/RoleQuery.css")))
   { %>
<link href="../ClientCSS/RoleQuery.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/RoleQuery.js")))
   { %>
<script src="../ClientScripts/RoleQuery.js" type="text/javascript"></script>
<% } %>
<asp:HiddenField ID="Hidden_SelectViewControlID" runat="server" />


<div class = "out_border">
<div class="main_box">
<h3 > 查询条件</h3>
<div class="allcol2">

    <div class = "item_box_col1">
    
    
    <span>角色名称 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "ctrl_Role_Name" runat="server" TextType = "String" FieldName = "Role_Name"  Tag = "8"   ControlId = "1000000362"    >  </sp:SText>

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
            <asp:LinkButton ID="ctrl_rolequery_viewlistitem" runat="server" CommandArgument = "1000000363" Text="所有角色" OnClick = "btnLoadView_Click"></asp:LinkButton>
            <asp:LinkButton ID="ViewItem5C" runat="server" CommandArgument = "1000000383" Text="启用角色" OnClick = "btnLoadView_Click"></asp:LinkButton>
            <asp:LinkButton ID="ViewItemR4" runat="server" CommandArgument = "1000000384" Text="停用角色" OnClick = "btnLoadView_Click"></asp:LinkButton>
    </div>
</div>
           <div id = "divctrl_rolequery_view" runat = "server"></div>
           <div id = "divviewcontrolB6" runat = "server"></div>
           <div id = "divviewcontrolVX" runat = "server"></div>
<div class="button_bar">
        <asp:LinkButton ID="ctrl_rolequery_op_add" runat="server"  UseSubmitBehavior = "false" Text="新增" onclick = "btnAdd_Click" OnClientClick=""/> 
</div>

</asp:Content>