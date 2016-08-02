<%@ Page Title="栏目查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Columns_Query.aspx.cs" Inherits="Drision.Framework.Web.T_Columns_Query" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Columns_Query.css")))
   { %>
<link href="../ClientCSS/T_Columns_Query.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Columns_Query.js")))
   { %>
<script src="../ClientScripts/T_Columns_Query.js" type="text/javascript"></script>
<% } %>
<asp:HiddenField ID="Hidden_SelectViewControlID" runat="server" />


<div class = "out_border">
<div class="main_box">
<h3 > 栏目查询</h3>
<div class="allcol2">

    <div class = "item_box_col1">
    
    
    <span>菜单Key </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "qc_MenuKeyU2" runat="server" TextType = "String" FieldName = "MenuKey"  Tag = "8"   ControlId = "2000000308329"    >  </sp:SText>

    </div>
        <div id = "moreCondition"> 
    <div class = "item_box_col1">
    
    
    <span>栏目名称 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "tbColumns_Name" runat="server" TextType = "String" FieldName = "Columns_Name"  Tag = "8"   ControlId = "2000000308227"    >  </sp:SText>

    </div>
    <div class = "item_box_col1">
    
    
    <span>状态 </span>
    <span class = "left_star">&nbsp;</span>
        <asp:ComboControl ID = "qc_State3H" runat="server" FieldName = "State"  DropdownType = "DropdownList" ShowEmptyItem = "True" Tag = "3" AutoSelectFirst = "true"   ControlId = "2000000308328" />

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
            <asp:LinkButton ID="DefaultViewListItem" runat="server" CommandArgument = "2000000308230" Text="栏目列表" OnClick = "btnLoadView_Click"></asp:LinkButton>
    </div>
</div>
           <div id = "divDefaultViewControl" runat = "server"></div>
<div class="button_bar">
        <asp:LinkButton ID="btnAdd" runat="server"  UseSubmitBehavior = "false" Text="添加" onclick = "btnAdd_Click" OnClientClick=""/> 
</div>

</asp:Content>