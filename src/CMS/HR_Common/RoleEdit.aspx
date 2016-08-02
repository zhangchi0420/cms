<%@ Page Title="角色修改" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleEdit.aspx.cs" Inherits="Drision.Framework.Web.RoleEdit" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../AutoUserControls/HR_Common/T_User_viewcontrolWG.ascx" tagname="T_User_viewcontrolWG" tagprefix="asp" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/RoleEdit.css")))
   { %>
<link href="../ClientCSS/RoleEdit.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/RoleEdit.js")))
   { %>
<script src="../ClientScripts/RoleEdit.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基本信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span>角色名称</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "ctrl_Role_Name" ClientInstanceName = "ctrl_Role_Name"   runat="server" TextType = "String" FieldName = "Role_Name" IsRequired = "true"  MaxLength = "20"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_full">
        
        <span>备注</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "ctrl_Role_Comment" ClientInstanceName = "ctrl_Role_Comment"   runat="server" TextType = "Text" FieldName = "Role_Comment"   MaxLength = "256"  PlaceHolder = ""/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>

<div class="button_bar">
        <asp:LinkButton ID="btnMMRelationAddJZ" runat="server"  UseSubmitBehavior = "false" Text="添加用户" onclick = "btnMMRelationAddJZ_Click" OnClientClick="return ShowWaiting(false);"/>
</div>



<div id = "divviewcontrolWG" runat = "server">
    <asp:T_User_viewcontrolWG ID = "viewcontrolWG" runat="server"></asp:T_User_viewcontrolWG>
</div>

<div class="button_bar">
        <asp:LinkButton ID="ctrl_roleedit_op_save" runat="server"  Text="保存" onclick = "btnSave_Click" OnClientClick="" onprerender="ctrl_roleedit_op_save_PreRender"/>
        <asp:LinkButton ID="ctrl_roleedit_op_back" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnBack_Click" OnClientClick=""/>
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