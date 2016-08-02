<%@ Page Title="新增页面" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="T_Department_Add.aspx.cs" Inherits="Drision.Framework.Web.T_Department_Add" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/T_Department_Add.css")))
   { %>
<link href="../ClientCSS/T_Department_Add.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/T_Department_Add.js")))
   { %>
<script src="../ClientScripts/T_Department_Add.js" type="text/javascript"></script>
<% } %>
            <div class = "out_border">
            <div class="main_box"><h3>&nbsp;</h3>
<asp:TreeControl ID="tree" runat="server" ClientInstanceName="tree" OnNodeClick="tree_NodeClick" CssClass = "treebox" GetAllChecked = "true"
    OnAjaxLoading="tree_AjaxLoading" AutoPostBack="true" LoadMode = "Default" ExpandAllNodes = "True" TwoState="False"  ShowCheckBox="True">
</asp:TreeControl><div class= "treeform">
<div class = "out_border">
<div class="main_box">
<h3>
段落</h3>

<div class="allcol1">

        <div class = "item_box_col1">
        
        <span>组织名称</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txt_Department_NameMZ" ClientInstanceName = "txt_Department_NameMZ"   runat="server" TextType = "String" FieldName = "Department_Name"   MaxLength = "50"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>组织层级编码</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txt_Organization_Code2S" ClientInstanceName = "txt_Organization_Code2S" ReadOnly = "true"  runat="server" TextType = "String" FieldName = "Organization_Code"   MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>组织编码</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txt_Organization_EnCodeP3" ClientInstanceName = "txt_Organization_EnCodeP3"   runat="server" TextType = "String" FieldName = "Organization_EnCode"   MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>组织完整代码</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txt_Department_Full_Code8J" ClientInstanceName = "txt_Department_Full_Code8J" ReadOnly = "true"  runat="server" TextType = "String" FieldName = "Department_Full_Code"   MaxLength = "100"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>部门经理</span>
        <span class = "left_star">&nbsp;</span>
            <sp:Dropdown ID = "ddlManager_IDNM" ClientInstanceName = "ddlManager_IDNM"   runat="server" FieldName = "Manager_ID"  ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_col1">
        
        <span>是否停用</span>
        <span class = "left_star">&nbsp;</span>
            <asp:ComboControl ID = "txt_Department_StatusBE" ClientInstanceName = "txt_Department_StatusBE"   runat="server" FieldName = "Department_Status" DropdownType = "DropdownList"  ShowEmptyItem = "true" AutoSelectFirst = "true"/>

        </div>
        <div class = "item_box_col1">
        
        <span>组织描述</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "txt_RemarkZQ" ClientInstanceName = "txt_RemarkZQ"   runat="server" TextType = "Text" FieldName = "Remark"   MaxLength = "100"  PlaceHolder = ""/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="btnAdd" runat="server"  UseSubmitBehavior = "false" Text="新增" onclick = "btnAdd_Click" OnClientClick=""/> 
        <asp:LinkButton ID="btnSave" runat="server"  Text="保存" onclick = "btnDeptTreeSave_Click" OnClientClick="" onprerender="btnSave_PreRender"/>
        <asp:LinkButton ID="btnCustom6M" runat="server"  UseSubmitBehavior = "false" Text="配置岗位" onclick = "btnCustom6M_Click" OnClientClick=""/>
        <asp:LinkButton ID="btnDeleteH6" runat="server"  UseSubmitBehavior = "false" Text="删除" onclick = "btnDeleteH6_Click" OnClientClick="if (!confirm(&#39;是否删除该记录？&#39;)) {return false;}"/>
</div>

</div>            <div class="cl"></div>
            </div>
            </div>

</asp:Content>