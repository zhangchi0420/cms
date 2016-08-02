<%@ Page Title="栏目编辑" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Columns_Edit.aspx.cs" Inherits="Drision.Framework.Web.T_Columns_Edit" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Columns_Edit.css")))
   { %>
<link href="../ClientCSS/T_Columns_Edit.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Columns_Edit.js")))
   { %>
<script src="../ClientScripts/T_Columns_Edit.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基础信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span>菜单Key</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtMenuKey2E" ClientInstanceName = "txtMenuKey2E"   runat="server" TextType = "String" FieldName = "MenuKey" IsRequired = "true"  MaxLength = "255"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>栏目名称</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "tbColumns_Name" ClientInstanceName = "tbColumns_Name"   runat="server" TextType = "String" FieldName = "Columns_Name" IsRequired = "true"  MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>状态</span>
        <span class = "left_star">&nbsp;</span>
            <asp:ComboControl ID = "txtStateGR" ClientInstanceName = "txtStateGR"   runat="server" FieldName = "State" DropdownType = "DropdownList"  ShowEmptyItem = "true" AutoSelectFirst = "true"/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
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