<%@ Page Title="产品详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Product_Detail.aspx.cs" Inherits="Drision.Framework.Web.T_Product_Detail" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Product_Detail.css")))
   { %>
<link href="../ClientCSS/T_Product_Detail.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Product_Detail.js")))
   { %>
<script src="../ClientScripts/T_Product_Detail.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基本信息</h3>

<div class="allcol2">

        <div class = "item_box_full">
        
        <span>产品名称</span>
        
            <div class = "item_display"><asp:Label ID = "txtProduct_NameXR" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>产品类型</span>
        
            <div class = "item_display"><asp:Label ID = "ddlProductTypeId7K" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>建筑面积(㎡)</span>
        
            <div class = "item_display"> <asp:Label ID = "txtAreaKE" runat = "server"></asp:Label> </div>

        </div>
        <div class = "item_box_col1">
        
        <span>设计风格</span>
        
            <div class = "item_display"><asp:Label ID = "txtDesignStyleAQ" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>主案设计师</span>
        
            <div class = "item_display"><asp:Label ID = "txtDesignersQ3" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>链接地址</span>
        
            <div class = "item_display"><asp:Label ID = "txtUrlXZ" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>图片</span>
        
            <div class = "item_display"> <sp:Attachment ID="txtImg4P" runat="server" ShowImage="false" ImageHeight="200" ImageWidth="200"  /> </div>

        </div>
        <div class = "item_box_col1">
        
        <span>状态</span>
        
            <div class = "item_display"><asp:Label ID = "txtState2S" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>摘要</span>
        
            <div class = "item_display"><asp:Label ID = "txtProductSummaryA7" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>内容</span>
        
            <div class = "item_display"><asp:Label ID = "txtContent8D" runat = "server"></asp:Label></div>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="btnReturnKP" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnReturnKP_Click" OnClientClick="return ShowWaiting(false);"/>
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