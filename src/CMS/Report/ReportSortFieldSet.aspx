<%@ Page Title="报表排序字段" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportSortFieldSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportSortFieldSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3>报表排序字段</h3>
<div class="allcol1">
    <div class = "item_box_full">
    <span>排序字段</span>
    <span class = "left_star"></span>
        <asp:ComboControl ID = "ccSortField" runat="server" IsRequired = "true" DropdownType = "DropdownList"></asp:ComboControl>
    </div>
    <div class = "item_box_full">
    <span>排序方式</span>
    <span class = "left_star"></span>
        <asp:ComboControl ID = "ccSortType" runat="server" IsRequired = "true" DropdownType = "DropdownList"></asp:ComboControl>
    </div>
</div>
<div class="cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnAdd" runat="server" onclick="btnAdd_Click" Text = "添加" OnClientClick="return Validate();"/>
</div>
<div class = "grid_title">
</div>
<div class="grid_filter"></div>
<asp:GridControl ID="gcList" runat="server"  AutoGenerateColumns="false" ShowFooter="false">
    <Columns>
        <asp:HeaderSortField DataTextField="OrderIndex"  HeaderText="序号"  ></asp:HeaderSortField>
        <asp:HeaderSortField DataTextField="DisplayText"  HeaderText="排序字段"  ></asp:HeaderSortField>
        <asp:HeaderSortField DataTextField="SortType"  HeaderText="排序方式"  ></asp:HeaderSortField>
        <asp:OperationField>
        <Buttons>
                <asp:LinkButton ID="btnMoveUp" runat="server" Text="上移" CommandArgument='SortFieldId' OnClick = "btnMoveUp_Click"></asp:LinkButton>
                <asp:LinkButton ID="btnMoveDown" runat="server" Text="下移" CommandArgument='SortFieldId' OnClick = "btnMoveDown_Click"></asp:LinkButton>
                <asp:LinkButton ID="btnDelete" runat="server" Text="删除" CommandArgument='SortFieldId' OnClientClick="if (!confirm('是否删除该查询条件？')) {return false;}" OnClick = "btnDelete_Click"></asp:LinkButton>
        </Buttons>
        </asp:OperationField>
    </Columns>
</asp:GridControl>

<div class="button_bar">
    <asp:LinkButton ID="btnPre"  runat="server" Text="上一步" OnClick = "btnPre_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnNext"  runat="server" Text="下一步" OnClick = "btnNext_Click"></asp:LinkButton>
</div>
</asp:Content>
