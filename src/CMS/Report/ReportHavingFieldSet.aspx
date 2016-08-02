<%@ Page Title="二次过滤" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportHavingFieldSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportHavingFieldSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3>二次过滤字段</h3>
<div class="allcol1">
    <div class = "item_box_full">
    <span>汇总字段</span>
    <span class = "left_star"></span>
        <asp:ComboControl ID = "ccSumField" runat="server" IsRequired = "true" AutoSelectFirst = "true" DropdownType = "DropdownList"></asp:ComboControl>
    </div>
    <div class = "item_box_full">
    <span>比较方式</span>
    <span class = "left_star"></span>
        <asp:ComboControl ID = "ccCompareType" runat="server" IsRequired = "true" AutoSelectFirst = "true" DropdownType = "DropdownList"></asp:ComboControl>
    </div>
    <div class = "item_box_full">
    <span>比较值</span>
    <span class = "left_star"></span>
        <asp:TextControl ID = "tcCompareValue" runat = "server" IsRequired = "true" TextType = "Decimal" />
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
        <asp:HeaderSortField DataTextField="SumFieldDisplayText"  HeaderText="汇总字段"  ></asp:HeaderSortField>
        <asp:HeaderSortField DataTextField="CompareType"  HeaderText="比较方式"  ></asp:HeaderSortField>
        <asp:HeaderSortField DataTextField="CompareValue"  HeaderText="比较值"  ></asp:HeaderSortField>
        <asp:OperationField>
        <Buttons>
                <asp:LinkButton ID="btnDelete" runat="server" Text="删除" CommandArgument='HavingFieldId' OnClientClick="if (!confirm('是否删除该查询条件？')) {return false;}" OnClick = "btnDelete_Click"></asp:LinkButton>
        </Buttons>
        </asp:OperationField>
    </Columns>
</asp:GridControl>
<div class="main_box">
<div class="allcol2">
    <div class = "item_box_col1">
    <span>多条件链接方式</span>
    <span class = "left_star"></span>
        <asp:ComboControl ID = "ccHavingRelation" runat="server" IsRequired = "true" AutoSelectFirst = "true" DropdownType = "DropdownList"></asp:ComboControl>
    </div>
    <div class = "item_box_col1">
    <span>是否行转列</span>
    <span class = "left_star"></span>
        <asp:CheckControl ID = "ccIsChangeColumn" runat="server" />
    </div>
</div>
<div class="cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnPre"  runat="server" Text="上一步" OnClick = "btnPre_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnNext"  runat="server" Text="下一步" OnClick = "btnNext_Click"></asp:LinkButton>
</div>
</asp:Content>
