
<%@ Page Title="用户选择" Language="C#" MasterPageFile="~/Select.Master" AutoEventWireup="true" CodeBehind="UserSelect.aspx.cs" Inherits="Drision.Framework.Web.UserSelect" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../AutoUserControls/HR_Common/T_User_ctrl_userselect_view.ascx" tagname="T_User_ctrl_userselect_view" tagprefix="asp" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/UserSelect.css")))
   { %>
<link href="../ClientCSS/UserSelect.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/UserSelect.js")))
   { %>
<script src="../ClientScripts/UserSelect.js" type="text/javascript"></script>
<% } %>

<div class = "out_border">
<div class="main_box">
<h3 > 查询条件</h3>
<div class="allcol2">

    <div class = "item_box_col1">
    
    
    <span>帐号 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "ctrl_User_Code" runat="server" TextType = "String" FieldName = "User_Code"  Tag = "8"   ControlId = "1000000253"    >  </sp:SText>

    </div>
        <div id = "moreCondition"> 
    <div class = "item_box_col1">
    
    
    <span>姓名 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "ctrl_User_Name" runat="server" TextType = "String" FieldName = "User_Name"  Tag = "8"   ControlId = "1000000254"    >  </sp:SText>

    </div>
    <div class = "item_box_col1">
    
    
    <span>部门 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:Dropdown ID = "ctrl_Department_ID" ClientInstanceName = "ctrl_Department_ID" runat="server" FieldName = "Department_ID"  Tag = "3"    ControlId = "1000000252">  </sp:Dropdown>

    </div>

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


    <div class="grid_title" id = "divViewControlTitle">选择用户</div>
<div id = "divctrl_userselect_view" runat = "server">
    <asp:T_User_ctrl_userselect_view ID = "ctrl_userselect_view" runat="server"></asp:T_User_ctrl_userselect_view>
</div>

</asp:Content>