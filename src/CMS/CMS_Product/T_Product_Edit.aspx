<%@ Page Title="产品编辑" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Product_Edit.aspx.cs" Inherits="Drision.Framework.Web.T_Product_Edit" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>



        <%@ Register src="../ClientUserControl/CustomsEditor.ascx" tagname="CustomsEditor" tagprefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Product_Edit.css")))
   { %>
<link href="../ClientCSS/T_Product_Edit.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Product_Edit.js")))
   { %>
<script src="../ClientScripts/T_Product_Edit.js" type="text/javascript"></script>
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
            <sp:Dropdown ID = "ddlProductTypeIdEN" ClientInstanceName = "ddlProductTypeIdEN"   runat="server" FieldName = "ProductTypeId" IsRequired = "true" ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_col1">
        
        <span>建筑面积(㎡)</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtAreaHS" ClientInstanceName = "txtAreaHS"   runat="server" TextType = "Int32" FieldName = "Area"     />

        </div>
        <div class = "item_box_col1">
        
        <span>设计风格</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtDesignStyleA6" ClientInstanceName = "txtDesignStyleA6"   runat="server" TextType = "String" FieldName = "DesignStyle"   MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>主案设计师</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtDesignersT8" ClientInstanceName = "txtDesignersT8"   runat="server" TextType = "String" FieldName = "Designers"   MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>链接地址</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtUrlX6" ClientInstanceName = "txtUrlX6"   runat="server" TextType = "String" FieldName = "Url"   MaxLength = "255"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>图片</span>
        <span class = "left_star">&nbsp;</span>
            <sp:Uploader ID="txtImgEV"  runat="server" ShowImage="false" ImageHeight="200" ImageWidth="200"   />

        </div>
        <div class = "item_box_col1">
        
        <span>状态</span>
        <span class = "left_star">&nbsp;</span>
            <asp:ComboControl ID = "txtStateFY" ClientInstanceName = "txtStateFY"   runat="server" FieldName = "State" DropdownType = "DropdownList"  ShowEmptyItem = "true" AutoSelectFirst = "true"/>

        </div>
        <div class = "item_box_full">
        
        <span>摘要</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txtProductSummaryCR" ClientInstanceName = "txtProductSummaryCR"   runat="server" TextType = "String" FieldName = "ProductSummary"   MaxLength = "500"  PlaceHolder = ""/>

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