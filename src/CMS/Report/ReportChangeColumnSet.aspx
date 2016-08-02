<%@ Page Title="报表行转列" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportChangeColumnSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportChangeColumnSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3>报表行转列</h3>
<div class="allcol1">
    <div class = "item_box_col1">
    <span>显示为行</span>
    <span class = "left_star">*</span>
        <asp:ComboControl ID = "ccRowFieldAliases" runat="server" AutoSelectFirst = "true" IsRequired = "true" DropdownType = "DropdownList"/>
    </div>
    <div class = "item_box_col1">
    <span>显示为列</span>
    <span class = "left_star">*</span>
        <asp:ComboControl ID = "ccColumnFieldAliases" runat="server" AutoSelectFirst = "true" IsRequired = "true" DropdownType = "DropdownList"/>
    </div>
    <div class = "item_box_col1">
    <span>显示的值</span>
    <span class = "left_star">*</span>
        <asp:ComboControl ID = "ccValueFieldAliases" runat="server" AutoSelectFirst = "true" IsRequired = "true" DropdownType = "DropdownList"/>
    </div>
</div>
<div class = "cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnPre"  runat="server" Text="上一步" OnClick = "btnPre_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnNext"  runat="server" Text="下一步" OnClick = "btnNext_Click" OnClientClick="return Validate();"></asp:LinkButton>
</div>
</asp:Content>
