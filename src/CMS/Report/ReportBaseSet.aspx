<%@ Page Title="报表基本信息" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportBaseSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportBaseSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3>基础信息</h3>

<div class="allcol1">
    <div class = "item_box_col1">
    <span>报表名称</span>
    <span class = "left_star">*</span>
        <asp:TextControl ID = "tcReportName" runat="server" TextType = "String" IsRequired = "true"  MaxLength = "50"/>
    </div>
    <div class = "item_box_col1">
    <span>报表模式</span>
    <span class = "left_star">*</span>
        <asp:ComboControl ID = "ccReportModel" runat="server" AutoSelectFirst = "true" DropdownType = "DropdownList" ReadOnly = "true"/>
    </div>
    <div class = "item_box_col1">
    <span>查询主实体</span>
    <span class = "left_star">*</span>
        <asp:ComboControl ID = "ccEntityName" runat="server" AutoSelectFirst = "true" DropdownType = "DropdownList"/>
    </div>
</div>
<div class = "cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnReturn" runat="server"  UseSubmitBehavior = "false" Text="返回" PostBackUrl = "~/Report/ReportQuery.aspx"/>
    <asp:LinkButton ID="btnNext" runat="server"  Text="下一步" onclick = "btnNext_Click" OnClientClick="return Validate();"/>
</div>
</asp:Content>
