<%@ Page Title="产品类型查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_ProductType_Query.aspx.cs" Inherits="Drision.Framework.Web.T_ProductType_Query" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_ProductType_Query.css")))
   { %>
<link href="../ClientCSS/T_ProductType_Query.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_ProductType_Query.js")))
   { %>
<script src="../ClientScripts/T_ProductType_Query.js" type="text/javascript"></script>
<% } %>
<asp:HiddenField ID="Hidden_SelectViewControlID" runat="server" />


<div class = "out_border">
<div class="main_box">
<h3 > 产品类型查询</h3>
<div class="allcol2">

    <div class = "item_box_col1">
    
    
    <span>类型名称 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "tbProductType_Name" runat="server" TextType = "String" FieldName = "ProductType_Name"  Tag = "8"   ControlId = "2000000308041"    >  </sp:SText>

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
            <asp:LinkButton ID="DefaultViewListItem" runat="server" CommandArgument = "2000000308044" Text="基本视图" OnClick = "btnLoadView_Click"></asp:LinkButton>
    </div>
</div>
           <div id = "divDefaultViewControl" runat = "server"></div>
<div class="button_bar">
        <asp:LinkButton ID="btnAdd" runat="server"  UseSubmitBehavior = "false" Text="添加" onclick = "btnAdd_Click" OnClientClick=""/> 
</div>

</asp:Content>