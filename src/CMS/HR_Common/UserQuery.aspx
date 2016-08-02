<%@ Page Title="用户查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserQuery.aspx.cs" Inherits="Drision.Framework.Web.UserQuery" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/UserQuery.css")))
   { %>
<link href="../ClientCSS/UserQuery.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/UserQuery.js")))
   { %>
<script src="../ClientScripts/UserQuery.js" type="text/javascript"></script>
<% } %>
<asp:HiddenField ID="Hidden_SelectViewControlID" runat="server" />


<div class = "out_border">
<div class="main_box">
<h3  id = 'queryCondition' style = 'cursor:pointer' ><span id = 'spIsCanShrink'>+</span> 查询条件</h3>
<div class="allcol2">

    <div class = "item_box_col1">
    
    
    <span>帐号 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "ctrl_User_Code" runat="server" TextType = "String" FieldName = "User_Code"  Tag = "8"   ControlId = "1000000183"    >  </sp:SText>

    </div>
        <div id = "moreCondition"> 
    <div class = "item_box_col1">
    
    
    <span>姓名 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "ctrl_User_Name" runat="server" TextType = "String" FieldName = "User_Name"  Tag = "8"   ControlId = "1000000184"    >  </sp:SText>

    </div>
    <div class = "item_box_col1">
    
    
    <span>部门 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:Dropdown ID = "ctrl_Department_ID" ClientInstanceName = "ctrl_Department_ID" runat="server" FieldName = "Department_ID"  Tag = "3"    ControlId = "1000000182">  </sp:Dropdown>

    </div>
    <div class = "item_box_col1">
    
    
    <span>用户类别 </span>
    <span class = "left_star">&nbsp;</span>
        <asp:ComboControl ID = "txtUser_TypeV4" runat="server" FieldName = "User_Type"  DropdownType = "DropdownList" ShowEmptyItem = "True" Tag = "3" AutoSelectFirst = "true"   ControlId = "1000076864" />

    </div>
    <div class = "item_box_col1">
    
    
    <span>是否有效 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SRadioList ID="qc_User_StatusJ4" runat="server"  FieldName = "User_Status" Tag = "3" ControlId = "1000305589"/>

    </div>
    <div class = "item_box_col1">
    
    
    <span>是否禁web登录 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SCheckBoxList ID="qc_Is_Prohibit_WebVJ" runat="server"  FieldName = "Is_Prohibit_Web" Tag = "3" ControlId = "1000305590"/>

    </div>

    <div class = "item_box_col1">
    <span>每页显示行数</span>
    <span class = "left_star">&nbsp;</span>
    <asp:TextControl ID = "tcCustomerPageSize" runat="server" TextType = "Int32" />
    </div>
</div> 
</div>
<div class="cl"></div>
<div class="button_bar" >
<asp:LinkButton ID="btnQueryInner" runat="server" onclick="btnQuery_Click" onprerender="btnQuery_PreRender" Text = "查询" CssClass = "btnQuery"/>
<asp:LinkButton ID="btnClearConditionInner" runat="server" onclick="btnClearCondition_Click" Text = "重置"/>
</div>
</div>
</div>
<div class="button_bar" style='display: none;'>
<asp:LinkButton ID="btnQuery" runat="server" onclick="btnQuery_Click" onprerender="btnQuery_PreRender" Text = "查询" CssClass = "btnQuery"/>
<asp:LinkButton ID="btnClearCondition" runat="server" onclick="btnClearCondition_Click" Text = "重置"/>
</div>



<div class = "grid_title">
    <asp:Label ID = "lblViewControlTitle" runat = "server"></asp:Label>
    <div class="grid_filter" style="display:none">
            <asp:LinkButton ID="ctrl_userquery_viewlistitem1" runat="server" CommandArgument = "1000000188" Text="所有用户" OnClick = "btnLoadView_Click"></asp:LinkButton>
    </div>
</div>
           <div id = "divctrl_userquery_view" runat = "server"></div>
<div class="button_bar">
        <asp:LinkButton ID="btnRE" runat="server"  UseSubmitBehavior = "false" Text="批量删除" onclick = "btn5K_Click" OnClientClick=""/>
        <asp:LinkButton ID="ctrl_userquery_op_add" runat="server"  UseSubmitBehavior = "false" Text="新增" onclick = "btnAdd_Click" OnClientClick=""/> 
</div>

</asp:Content>