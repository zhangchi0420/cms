<%@ Page Title="文章新增" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Article_Add.aspx.cs" Inherits="Drision.Framework.Web.T_Article_Add" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../ClientUserControl/CustomsEditor.ascx" tagname="CustomsEditor" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Article_Add.css")))
   { %>
<link href="../ClientCSS/T_Article_Add.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Article_Add.js")))
   { %>
<script src="../ClientScripts/T_Article_Add.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基础信息</h3>

<div class="allcol2">

        <div class = "item_box_full">
        
        <span>文章标题</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "tbArticle_Name" ClientInstanceName = "tbArticle_Name"   runat="server" TextType = "String" FieldName = "Article_Name" IsRequired = "true"  MaxLength = "255"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>栏目</span>
        <span class = "left_star">*</span>
            <sp:Dropdown ID = "ddlColumnsId4Q" ClientInstanceName = "ddlColumnsId4Q"   runat="server" FieldName = "ColumnsId" IsRequired = "true" ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_full">
        
        <span>标题图片</span>
        <span class = "left_star">&nbsp;</span>
            <sp:Uploader ID="txtImg7Y"  runat="server" ShowImage="false" ImageHeight="200" ImageWidth="200" MaxFileSize = "200kb" FileFilter = "文件类型|jpg,png" AutoShowImage = "true"/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
    <asp:CustomsEditor ID="CustomsEditor" runat="server" />
<div class="button_bar">
        <asp:LinkButton ID="btnSave" runat="server"  Text="保存" onclick = "btnSave_Click" OnClientClick="return ShowWaiting(true);" onprerender="btnSave_PreRender"/>
        <asp:LinkButton ID="btnSaveAndPub" runat="server"  Text="保存并发布" onclick = "btnSaveAndPub_Click" OnClientClick="return ShowWaiting(true);" onprerender="btnSaveAndPub_PreRender"/>
        <asp:LinkButton ID="btnReturn" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnReturn_Click" OnClientClick=""/>
</div>

</asp:Content>