<%@ Page Title="配置详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Configuration_Detail.aspx.cs" Inherits="Drision.Framework.Web.T_Configuration_Detail" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Configuration_Detail.css")))
   { %>
<link href="../ClientCSS/T_Configuration_Detail.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Configuration_Detail.js")))
   { %>
<script src="../ClientScripts/T_Configuration_Detail.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
配置信息</h3>

<div class="allcol2">

        <div class = "item_box_full">
        
        <span>标题</span>
        
            <div class = "item_display"><asp:Label ID = "txtConfiguration_TitleWG" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>配置键</span>
        
            <div class = "item_display"><asp:Label ID = "txtConfiguration_KeyWW" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>配置分组</span>
        
            <div class = "item_display"><asp:Label ID = "ddlConfiguration_Group_Id7T" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>配置值</span>
        
            <div class = "item_display"><asp:Label ID = "txtConfiguration_ValuePH" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>描述</span>
        
            <div class = "item_display"><asp:Label ID = "txtConfiguration_DescriptionWS" runat = "server"></asp:Label></div>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="btnReturnVX" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnReturnVX_Click" OnClientClick="return ShowWaiting(false);"/>
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