<%@ Page Title="报表图形配置" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportGraphSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportGraphSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3>报表图形配置</h3>
<div class="allcol1">
    <div class = "item_box_col1">
    <span>图形类型</span>
    <span class = "left_star">*</span>
        <asp:ComboControl ID = "ccChartType" runat="server" AutoSelectFirst = "true" IsRequired = "true" DropdownType = "DropdownList"/>
    </div>
    <div class = "item_box_col1">
    <span>显示为横轴</span>
    <span class = "left_star">*</span>
        <asp:ComboControl ID = "ccXMember" runat="server" AutoSelectFirst = "true" IsRequired = "true" DropdownType = "DropdownList"/>
    </div>
    <div class = "item_box_col1">
    <span>显示为纵轴</span>
    <span class = "left_star">*</span>
        <asp:ComboControl ID = "ccYMember" runat="server" AutoSelectFirst = "true" IsRequired = "true" DropdownType = "DropdownList"/>
    </div>

    <div class = "item_box_col1">
    <span>显示多组图</span>
    <span class = "left_star"></span>
        <asp:ComboControl ID = "ccSeriesMember" runat="server" ShowEmptyItem = "true" DropdownType = "DropdownList"/>
    </div>
</div>
<div class = "cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnPre"  runat="server" Text="上一步" OnClick = "btnPre_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnNext"  runat="server" Text="下一步" OnClick = "btnNext_Click" OnClientClick="return Validate();"></asp:LinkButton>
</div>
</asp:Content>
