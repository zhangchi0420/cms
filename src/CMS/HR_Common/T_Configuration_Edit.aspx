<%@ Page Title="配置编辑" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Configuration_Edit.aspx.cs" Inherits="Drision.Framework.Web.T_Configuration_Edit" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Configuration_Edit.css")))
   { %>
<link href="../ClientCSS/T_Configuration_Edit.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Configuration_Edit.js")))
   { %>
<script src="../ClientScripts/T_Configuration_Edit.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
配置信息</h3>

<div class="allcol2">

        <div class = "item_box_full">
        
        <span>配置标题</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtConfiguration_TitleNB" ClientInstanceName = "txtConfiguration_TitleNB"   runat="server" TextType = "String" FieldName = "Configuration_Title" IsRequired = "true"  MaxLength = "255"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>配置键</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtConfiguration_KeyAY" ClientInstanceName = "txtConfiguration_KeyAY"   runat="server" TextType = "String" FieldName = "Configuration_Key" IsRequired = "true"  MaxLength = "255"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>配置分组</span>
        <span class = "left_star">*</span>
            <sp:Dropdown ID = "ddlConfiguration_Group_IdA7" ClientInstanceName = "ddlConfiguration_Group_IdA7"   runat="server" FieldName = "Configuration_Group_Id" IsRequired = "true" ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_full">
        
        <span>配置值</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtConfiguration_Value3N" ClientInstanceName = "txtConfiguration_Value3N"   runat="server" TextType = "Text" FieldName = "Configuration_Value" IsRequired = "true"  MaxLength = "4000"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>描述</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtConfiguration_DescriptionKD" ClientInstanceName = "txtConfiguration_DescriptionKD"   runat="server" TextType = "Text" FieldName = "Configuration_Description" IsRequired = "true"  MaxLength = "4000"  PlaceHolder = ""/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="btnSaveFQ" runat="server"  Text="保存" onclick = "btnSaveFQ_Click" OnClientClick="" onprerender="btnSaveFQ_PreRender"/>
        <asp:LinkButton ID="btnReturn7W" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnReturn7W_Click" OnClientClick=""/>
</div>

    <asp:HiddenControl ID="hcPostBack"  runat="server" ClientInstanceName="hcPostBack" />
    <asp:CallBackControl ID="cbcPatchAdd" ClientInstanceName="cbcPatchAdd" runat="server" CallBackType="JSON"
    OnCallBack="cbcPatchAdd_CallBack" OnComplete="function(data){
        if(data == '1'){
            eval(hcPostBack.getText());
        }
        else{
            alert(data);
        }
    }" OnError="function(data){alert(data);}"
    />
</asp:Content>