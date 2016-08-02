<%@ Page Title="文章详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Article_Detail.aspx.cs" Inherits="Drision.Framework.Web.T_Article_Detail" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Article_Detail.css")))
   { %>
<link href="../ClientCSS/T_Article_Detail.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Article_Detail.js")))
   { %>
<script src="../ClientScripts/T_Article_Detail.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基础信息</h3>

<div class="allcol2">

        <div class = "item_box_full">
        
        <span>文章标题</span>
        
            <div class = "item_display"><asp:Label ID = "tbArticle_Name" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>栏目</span>
        
            <div class = "item_display"><asp:Label ID = "ddlColumnsIdCQ" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>状态</span>
        
            <div class = "item_display"><asp:Label ID = "txtState3B" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>标题图片</span>
        
            <div class = "item_display"> <sp:Attachment ID="txtImgYE" runat="server" ShowImage="false" ImageHeight="200" ImageWidth="200"  /> </div>

        </div>
        <div class = "item_box_full">
        
        <span>内容</span>
        
            <div class = "item_display"><asp:Label ID = "txtContentF3" runat = "server"></asp:Label></div>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
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