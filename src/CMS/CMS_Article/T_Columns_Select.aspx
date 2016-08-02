
<%@ Page Title="栏目选择" Language="C#" MasterPageFile="~/Select.Master" AutoEventWireup="true" CodeBehind="T_Columns_Select.aspx.cs" Inherits="Drision.Framework.Web.T_Columns_Select" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../AutoUserControls/CMS_Article/T_Columns_SelectViewControl.ascx" tagname="T_Columns_SelectViewControl" tagprefix="asp" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Columns_Select.css")))
   { %>
<link href="../ClientCSS/T_Columns_Select.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Columns_Select.js")))
   { %>
<script src="../ClientScripts/T_Columns_Select.js" type="text/javascript"></script>
<% } %>

<div class = "out_border">
<div class="main_box">
<h3 > 栏目查询</h3>
<div class="allcol2">

    <div class = "item_box_col1">
    
    
    <span>菜单Key </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "qc_MenuKey7Q" runat="server" TextType = "String" FieldName = "MenuKey"  Tag = "8"   ControlId = "2000000308337"    >  </sp:SText>

    </div>
        <div id = "moreCondition"> 
    <div class = "item_box_col1">
    
    
    <span>栏目名称 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "tbColumns_Name" runat="server" TextType = "String" FieldName = "Columns_Name"  Tag = "8"   ControlId = "2000000308238"    >  </sp:SText>

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


    <div class="grid_title" id = "divViewControlTitle">选择视图</div>
<div id = "divSelectViewControl" runat = "server">
    <asp:T_Columns_SelectViewControl ID = "SelectViewControl" runat="server"></asp:T_Columns_SelectViewControl>
</div>

</asp:Content>