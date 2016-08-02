<%@ Page Title="详情页面" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Role_Detail.aspx.cs" Inherits="Drision.Framework.Web.T_Role_Detail" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../AutoUserControls/HR_Common/T_User_viewcontrol5D.ascx" tagname="T_User_viewcontrol5D" tagprefix="asp" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Role_Detail.css")))
   { %>
<link href="../ClientCSS/T_Role_Detail.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Role_Detail.js")))
   { %>
<script src="../ClientScripts/T_Role_Detail.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
角色信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span>角色名称</span>
        
            <div class = "item_display"><asp:Label ID = "txtRole_NameEB" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>是否停用</span>
        
            <div class = "item_display"><asp:Label ID = "txtRole_StatusGT" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>备注</span>
        
            <div class = "item_display"><asp:Label ID = "txtRole_CommentDV" runat = "server"></asp:Label></div>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>

<div class="grid_button">
        <asp:LinkButton ID="btnMMRelationAddBC" runat="server"  UseSubmitBehavior = "false" Text="添加用户" onclick = "btnMMRelationAddBC_Click" OnClientClick="return ShowWaiting(false);"/>
</div>



    <div class="grid_title" id = "divViewControlTitle">用户列表</div>
<div id = "divviewcontrol5D" runat = "server">
    <asp:T_User_viewcontrol5D ID = "viewcontrol5D" runat="server"></asp:T_User_viewcontrol5D>
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