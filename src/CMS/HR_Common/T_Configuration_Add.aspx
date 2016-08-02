<%@ Page Title="配置新增" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Configuration_Add.aspx.cs" Inherits="Drision.Framework.Web.T_Configuration_Add" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Configuration_Add.css")))
   { %>
<link href="../ClientCSS/T_Configuration_Add.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Configuration_Add.js")))
   { %>
<script src="../ClientScripts/T_Configuration_Add.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
配置信息</h3>

<div class="allcol2">

        <div class = "item_box_full">
        
        <span>配置标题</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtConfiguration_TitleDD" ClientInstanceName = "txtConfiguration_TitleDD"   runat="server" TextType = "String" FieldName = "Configuration_Title" IsRequired = "true"  MaxLength = "255"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>配置键</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtConfiguration_KeyNX" ClientInstanceName = "txtConfiguration_KeyNX"   runat="server" TextType = "String" FieldName = "Configuration_Key" IsRequired = "true"  MaxLength = "255"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>配置分组</span>
        <span class = "left_star">*</span>
            <sp:Dropdown ID = "ddlConfiguration_Group_IdB8" ClientInstanceName = "ddlConfiguration_Group_IdB8"   runat="server" FieldName = "Configuration_Group_Id" IsRequired = "true" ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_full">
        
        <span>配置值</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtConfiguration_Value3C" ClientInstanceName = "txtConfiguration_Value3C"   runat="server" TextType = "Text" FieldName = "Configuration_Value" IsRequired = "true"  MaxLength = "4000"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>描述</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtConfiguration_DescriptionDF" ClientInstanceName = "txtConfiguration_DescriptionDF"   runat="server" TextType = "Text" FieldName = "Configuration_Description" IsRequired = "true"  MaxLength = "4000"  PlaceHolder = ""/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="btnSave28" runat="server"  Text="保存" onclick = "btnSave28_Click" OnClientClick="return ShowWaiting(true);" onprerender="btnSave28_PreRender"/>
        <asp:LinkButton ID="btnReturnMW" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnReturnMW_Click" OnClientClick="return ShowWaiting(false);"/>
</div>

</asp:Content>