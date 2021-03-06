﻿<%@ Page Title="部门新增" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartmentAdd.aspx.cs" Inherits="Drision.Framework.Web.DepartmentAdd" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/DepartmentAdd.css")))
   { %>
<link href="../ClientCSS/DepartmentAdd.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/DepartmentAdd.js")))
   { %>
<script src="../ClientScripts/DepartmentAdd.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基本信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span>部门编码</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "ctrl_Deportment_Encode" ClientInstanceName = "ctrl_Deportment_Encode"   runat="server" TextType = "String" FieldName = "Deportment_Encode" IsRequired = "true"  MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>部门名称</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "ctrl_Department_Name" ClientInstanceName = "ctrl_Department_Name"   runat="server" TextType = "String" FieldName = "Department_Name" IsRequired = "true"  MaxLength = "50"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>部门经理</span>
        <span class = "left_star">&nbsp;</span>
            <sp:Dropdown ID = "ctrl_Manager_ID" ClientInstanceName = "ctrl_Manager_ID"   runat="server" FieldName = "Manager_ID"  ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_col1">
        
        <span>父级部门</span>
        <span class = "left_star">&nbsp;</span>
            <sp:Dropdown ID = "ctrl_Parent_ID" ClientInstanceName = "ctrl_Parent_ID"   runat="server" FieldName = "Parent_ID"  ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_col1">
        
        <span>是否停用</span>
        <span class = "left_star">*</span>
            <asp:ComboControl ID = "ctrl_Department_StatusP3" ClientInstanceName = "ctrl_Department_StatusP3"   runat="server" FieldName = "Department_Status" DropdownType = "DropdownList" IsRequired = "true" ShowEmptyItem = "true" AutoSelectFirst = "true"/>

        </div>
        <div class = "item_box_full">
        
        <span>备注</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "ctrl_Department_Comment" ClientInstanceName = "ctrl_Department_Comment"   runat="server" TextType = "Text" FieldName = "Department_Comment"   MaxLength = "256"  PlaceHolder = ""/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="ctrl_deptadd_op_save" runat="server"  Text="保存" onclick = "btnSave_Click" OnClientClick="" onprerender="ctrl_deptadd_op_save_PreRender"/>
        <asp:LinkButton ID="ctrl_deptadd_op_back" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnBack_Click" OnClientClick=""/>
</div>

</asp:Content>