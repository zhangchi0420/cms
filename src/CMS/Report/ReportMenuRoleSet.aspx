<%@ Page Title="菜单权限" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportMenuRoleSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportMenuRoleSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3>菜单权限</h3>
<div class="allcol1">
    <div class = "item_box_col1">
    <span>父菜单</span>
    <span class = "left_star">*</span>
        <asp:ComboControl ID = "ccParent" runat="server" AutoSelectFirst = "true" DropdownType = "DropdownList"/>
    </div>
</div>

    <asp:TreeControl ID="tcRole" runat="server" CssClass = "treebox" ExpandAllNodes = "True"  ShowCheckBox="True" Width = "800" ></asp:TreeControl>
    <div class = "cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnPre"  runat="server" Text="上一步" OnClick = "btnPre_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnPreview"  runat="server" Text="预览" OnClick = "btnPreview_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnPublish"  runat="server" Text="发布" OnClick = "btnPublish_Click"></asp:LinkButton> 
</div>
</asp:Content>
