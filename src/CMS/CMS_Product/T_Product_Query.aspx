<%@ Page Title="产品查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Product_Query.aspx.cs" Inherits="Drision.Framework.Web.T_Product_Query" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Product_Query.css")))
   { %>
<link href="../ClientCSS/T_Product_Query.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Product_Query.js")))
   { %>
<script src="../ClientScripts/T_Product_Query.js" type="text/javascript"></script>
<% } %>
<asp:HiddenField ID="Hidden_SelectViewControlID" runat="server" />


<div class = "out_border">
<div class="main_box">
<h3 > 产品查询</h3>
<div class="allcol2">

    <div class = "item_box_full">
    
    
    <span>产品名称 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "tbProduct_Name" runat="server" TextType = "String" FieldName = "Product_Name"  Tag = "8"   ControlId = "2000000308103"    >  </sp:SText>

    </div>
        <div id = "moreCondition"> 
    <div class = "item_box_col1">
    
    
    <span>产品类型 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:Dropdown ID = "qc_ProductTypeIdH6" ClientInstanceName = "qc_ProductTypeIdH6" runat="server" FieldName = "ProductTypeId"  Tag = "3"    ControlId = "2000000308462">  </sp:Dropdown>

    </div>
    <div class = "item_box_col1">
    
    
    <span>状态 </span>
    <span class = "left_star">&nbsp;</span>
        <asp:ComboControl ID = "qc_StateKG" runat="server" FieldName = "State"  DropdownType = "DropdownList" ShowEmptyItem = "True" Tag = "3" AutoSelectFirst = "true"   ControlId = "2000000308459" />

    </div>
    <div class = "item_box_col1">
    
    
    <span>设计风格 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "qc_DesignStyleJB" runat="server" TextType = "String" FieldName = "DesignStyle"  Tag = "3"   ControlId = "2000000308460"    >  </sp:SText>

    </div>
    <div class = "item_box_col1">
    
    
    <span>主案设计师 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "qc_Designers8H" runat="server" TextType = "String" FieldName = "Designers"  Tag = "3"   ControlId = "2000000308461"    >  </sp:SText>

    </div>

</div> 
</div>
<div class="cl"></div>
<div class="button_bar" style='display: none;'>
<asp:LinkButton ID="btnQueryInner" runat="server" onclick="btnQuery_Click" onprerender="btnQuery_PreRender" Text = "查询" CssClass = "btnQuery"/>
<asp:LinkButton ID="btnClearConditionInner" runat="server" onclick="btnClearCondition_Click" Text = "重置"/>
</div>
</div>
</div>
<div class="button_bar" >
<asp:LinkButton ID="btnQuery" runat="server" onclick="btnQuery_Click" onprerender="btnQuery_PreRender" Text = "查询" CssClass = "btnQuery"/>
<asp:LinkButton ID="btnClearCondition" runat="server" onclick="btnClearCondition_Click" Text = "重置"/>
</div>



<div class = "grid_title">
    <asp:Label ID = "lblViewControlTitle" runat = "server"></asp:Label>
    <div class="grid_filter" style="display:none">
            <asp:LinkButton ID="DefaultViewListItem" runat="server" CommandArgument = "2000000308106" Text="产品列表" OnClick = "btnLoadView_Click"></asp:LinkButton>
    </div>
</div>
           <div id = "divDefaultViewControl" runat = "server"></div>
<div class="button_bar">
        <asp:LinkButton ID="btnAdd" runat="server"  UseSubmitBehavior = "false" Text="添加" onclick = "btnAdd_Click" OnClientClick=""/> 
</div>

</asp:Content>