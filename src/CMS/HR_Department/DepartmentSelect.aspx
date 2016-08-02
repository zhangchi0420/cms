
<%@ Page Title="部门选择" Language="C#" MasterPageFile="~/Select.Master" AutoEventWireup="true" CodeBehind="DepartmentSelect.aspx.cs" Inherits="Drision.Framework.Web.DepartmentSelect" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../AutoUserControls/HR_Department/T_Department_ctrl_deptselect_view.ascx" tagname="T_Department_ctrl_deptselect_view" tagprefix="asp" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/DepartmentSelect.css")))
   { %>
<link href="../ClientCSS/DepartmentSelect.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/DepartmentSelect.js")))
   { %>
<script src="../ClientScripts/DepartmentSelect.js" type="text/javascript"></script>
<% } %>

<div class = "out_border">
<div class="main_box">
<h3 > 查询条件</h3>
<div class="allcol2">

    <div class = "item_box_col1">
    
    
    <span>部门名称 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "ctrl_Department_Name" runat="server" TextType = "String" FieldName = "Department_Name"  Tag = "8"   ControlId = "1000000274"    >  </sp:SText>

    </div>
        <div id = "moreCondition"> 
    <div class = "item_box_col1">
    
    
    <span>部门编码 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "ctrl_Deportment_Encode" runat="server" TextType = "String" FieldName = "Deportment_Encode"  Tag = "8"   ControlId = "1000000272"    >  </sp:SText>

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


    <div class="grid_title" id = "divViewControlTitle">选择部门</div>
<div id = "divctrl_deptselect_view" runat = "server">
    <asp:T_Department_ctrl_deptselect_view ID = "ctrl_deptselect_view" runat="server"></asp:T_Department_ctrl_deptselect_view>
</div>

</asp:Content>