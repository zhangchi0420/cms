<%@ Page Title="角色新增" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleAdd.aspx.cs" Inherits="Drision.Framework.Web.RoleAdd" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/RoleAdd.css")))
   { %>
<link href="../ClientCSS/RoleAdd.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/RoleAdd.js")))
   { %>
<script src="../ClientScripts/RoleAdd.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基本信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span>角色名称</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "ctrl_Role_Name" ClientInstanceName = "ctrl_Role_Name"   runat="server" TextType = "String" FieldName = "Role_Name" IsRequired = "true"  MaxLength = "20"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>备注</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "ctrl_Role_Comment" ClientInstanceName = "ctrl_Role_Comment"   runat="server" TextType = "Text" FieldName = "Role_Comment"   MaxLength = "256"  PlaceHolder = ""/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="ctrl_roleadd_op_save" runat="server"  Text="保存" onclick = "btnSave_Click" OnClientClick="" onprerender="ctrl_roleadd_op_save_PreRender"/>
        <asp:LinkButton ID="ctrl_roleadd_op_back" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnBack_Click" OnClientClick=""/>
</div>

</asp:Content>