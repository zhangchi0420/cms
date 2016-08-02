<%@ Page Title="考勤员工设置" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Attend_User_Query_Set.aspx.cs" Inherits="Drision.Framework.Web.T_Attend_User_Query_Set" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Attend_User_Query_Set.css")))
   { %>
<link href="../ClientCSS/T_Attend_User_Query_Set.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Attend_User_Query_Set.js")))
   { %>
<script src="../ClientScripts/T_Attend_User_Query_Set.js" type="text/javascript"></script>
<% } %>
<asp:HiddenField ID="Hidden_SelectViewControlID" runat="server" />


<div class = "out_border">
<div class="main_box">
<h3 > 查询条件</h3>
<div class="allcol2">

    <div class = "item_box_col1">
    
    
    <span>所属部门 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:Dropdown ID = "ddlDepartment_IDQ6" ClientInstanceName = "ddlDepartment_IDQ6" runat="server" FieldName = "Department_ID"  Tag = "3"    ControlId = "1000300699">  </sp:Dropdown>

    </div>
        <div id = "moreCondition"> 
    <div class = "item_box_col1">
    
    
    <span>工号 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "txtUser_Code5S" runat="server" TextType = "String" FieldName = "User_Code"  Tag = "3"   ControlId = "1000300701"    >  </sp:SText>

    </div>
    <div class = "item_box_col1">
    
    
    <span>用户姓名 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:SText ID = "txtUser_NameMD" runat="server" TextType = "String" FieldName = "User_Name"  Tag = "3"   ControlId = "1000300703"    >  </sp:SText>

    </div>
    <div class = "item_box_col1">
    
    
    <span>是否有效 </span>
    <span class = "left_star">&nbsp;</span>
        <asp:ComboControl ID = "txtUser_StatusDV" runat="server" FieldName = "User_Status"  DropdownType = "DropdownList" ShowEmptyItem = "True" Tag = "3" AutoSelectFirst = "true"   ControlId = "1000300705" />

    </div>
    <div class = "item_box_col1">
    
    
    <span>是否考勤 </span>
    <span class = "left_star">&nbsp;</span>
        <sp:RadioList ID = "cbIs_Attend8D" runat = "server" FieldName = "Is_Attend"  Tag = "3" ControlId = "1000300707" TrueText = "是" FalseText = "否"/>

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
            <asp:LinkButton ID="ViewItemVV" runat="server" CommandArgument = "1000300722" Text="考勤用户列表" OnClick = "btnLoadView_Click"></asp:LinkButton>
    </div>
</div>
           <div id = "divviewcontrolYX" runat = "server"></div>
</asp:Content>