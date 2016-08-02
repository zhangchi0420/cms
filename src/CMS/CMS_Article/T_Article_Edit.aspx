<%@ Page Title="文章编辑" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Article_Edit.aspx.cs" Inherits="Drision.Framework.Web.T_Article_Edit" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>



        <%@ Register src="../ClientUserControl/CustomsEditor.ascx" tagname="CustomsEditor" tagprefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Article_Edit.css")))
   { %>
<link href="../ClientCSS/T_Article_Edit.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Article_Edit.js")))
   { %>
<script src="../ClientScripts/T_Article_Edit.js" type="text/javascript"></script>
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
            <sp:Dropdown ID = "ddlColumnsIdRX" ClientInstanceName = "ddlColumnsIdRX"   runat="server" FieldName = "ColumnsId" IsRequired = "true" ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_col1">
        
        <span>状态</span>
        <span class = "left_star">*</span>
            <asp:ComboControl ID = "txtStateK6" ClientInstanceName = "txtStateK6"   runat="server" FieldName = "State" DropdownType = "DropdownList" IsRequired = "true" ShowEmptyItem = "true" AutoSelectFirst = "true"/>

        </div>
        <div class = "item_box_full">
        
        <span>标题图片</span>
        <span class = "left_star">&nbsp;</span>
            <sp:Uploader ID="txtImgEJ"  runat="server" ShowImage="false" ImageHeight="200" ImageWidth="200"   />

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