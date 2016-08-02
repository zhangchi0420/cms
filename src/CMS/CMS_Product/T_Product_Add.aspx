<%@ Page Title="产品新增" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Product_Add.aspx.cs" Inherits="Drision.Framework.Web.T_Product_Add" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../ClientUserControl/CustomsEditor.ascx" tagname="CustomsEditor" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Product_Add.css")))
   { %>
<link href="../ClientCSS/T_Product_Add.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Product_Add.js")))
   { %>
<script src="../ClientScripts/T_Product_Add.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基础信息</h3>

<div class="allcol2">

        <div class = "item_box_full">
        
        <span>产品名称</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "tbProduct_Name" ClientInstanceName = "tbProduct_Name"   runat="server" TextType = "String" FieldName = "Product_Name" IsRequired = "true"  MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>产品类型</span>
        <span class = "left_star">*</span>
            <sp:Dropdown ID = "ddlProductTypeIdYW" ClientInstanceName = "ddlProductTypeIdYW"   runat="server" FieldName = "ProductTypeId" IsRequired = "true" ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_col1">
        
        <span>建筑面积(㎡)</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtArea3D" ClientInstanceName = "txtArea3D"   runat="server" TextType = "Int32" FieldName = "Area"     />

        </div>
        <div class = "item_box_col1">
        
        <span>设计风格</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtDesignStyleJM" ClientInstanceName = "txtDesignStyleJM"   runat="server" TextType = "String" FieldName = "DesignStyle"   MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>主案设计师</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtDesigners3W" ClientInstanceName = "txtDesigners3W"   runat="server" TextType = "String" FieldName = "Designers"   MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>链接地址</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtUrlAZ" ClientInstanceName = "txtUrlAZ"   runat="server" TextType = "String" FieldName = "Url"   MaxLength = "255"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>图片</span>
        <span class = "left_star">&nbsp;</span>
            <sp:Uploader ID="txtImgVB"  runat="server" ShowImage="false" ImageHeight="200" ImageWidth="200" MaxFileSize = "5mb" FileFilter = "文件类型|jpg,png" />

        </div>
        <div class = "item_box_full">
        
        <span>摘要</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtProductSummary5X" ClientInstanceName = "txtProductSummary5X"   runat="server" TextType = "String" FieldName = "ProductSummary"   MaxLength = "500"  PlaceHolder = ""/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
    <asp:CustomsEditor ID="CustomsEditor" runat="server" />
<div class="button_bar">
        <asp:LinkButton ID="btnSave" runat="server"  Text="保存" onclick = "btnSave_Click" OnClientClick="" onprerender="btnSave_PreRender"/>
        <asp:LinkButton ID="btnReturn" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnReturn_Click" OnClientClick=""/>
</div>

</asp:Content>