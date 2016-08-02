<%@ Page Title="产品类型新增" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_ProductType_Add.aspx.cs" Inherits="Drision.Framework.Web.T_ProductType_Add" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_ProductType_Add.css")))
   { %>
<link href="../ClientCSS/T_ProductType_Add.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_ProductType_Add.js")))
   { %>
<script src="../ClientScripts/T_ProductType_Add.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基础信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span>菜单Key</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtMenuKeyJY" ClientInstanceName = "txtMenuKeyJY"   runat="server" TextType = "String" FieldName = "MenuKey" IsRequired = "true"  MaxLength = "255"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>类型名称</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "tbProductType_Name" ClientInstanceName = "tbProductType_Name"   runat="server" TextType = "String" FieldName = "ProductType_Name" IsRequired = "true"  MaxLength = "100"  PlaceHolder = ""/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="btnSave" runat="server"  Text="保存" onclick = "btnSave_Click" OnClientClick="" onprerender="btnSave_PreRender"/>
        <asp:LinkButton ID="btnReturn" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnReturn_Click" OnClientClick=""/>
</div>

</asp:Content>