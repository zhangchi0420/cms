<%@ Page Title="设置考勤" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Attend_User_Set.aspx.cs" Inherits="Drision.Framework.Web.T_Attend_User_Set" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Attend_User_Set.css")))
   { %>
<link href="../ClientCSS/T_Attend_User_Set.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Attend_User_Set.js")))
   { %>
<script src="../ClientScripts/T_Attend_User_Set.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基本信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span>所属部门</span>
        <span class = "left_star">&nbsp;</span>
            <sp:Dropdown ID = "ddlDepartment_ID8T" ClientInstanceName = "ddlDepartment_ID8T" ReadOnly = "true"  runat="server" FieldName = "Department_ID"  ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_col1">
        
        <span>工号</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtUser_Code7Z" ClientInstanceName = "txtUser_Code7Z" ReadOnly = "true"  runat="server" TextType = "String" FieldName = "User_Code" IsRequired = "true"  MaxLength = "50"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>用户姓名</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "txtUser_Name8F" ClientInstanceName = "txtUser_Name8F" ReadOnly = "true"  runat="server" TextType = "String" FieldName = "User_Name" IsRequired = "true"  MaxLength = "20"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>是否考勤</span>
        <span class = "left_star">&nbsp;</span>
            <asp:CheckControl ID = "cbIs_AttendKD" ClientInstanceName = "cbIs_AttendKD"  runat = "server" FieldName = "Is_Attend" Text = "" />

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