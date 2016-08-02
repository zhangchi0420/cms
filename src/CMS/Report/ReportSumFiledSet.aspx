<%@ Page Title="报表汇总字段" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportSumFiledSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportSumFiledSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3>报表汇总字段</h3>
<div class="allcol1">
    <div class = "item_box_full">
    <span>查询字段</span>
    <span class = "left_star"></span>
        <asp:ComboControl ID = "ccQueryField" runat="server" IsRequired = "true" AutoSelectFirst = "true" AutoPostBack = "true" OnTextChanged = "ccQueryField_TextChanged" DropdownType = "DropdownList"></asp:ComboControl>
    </div>
    <div class = "item_box_full">
    <span>显示名称</span>
    <span class = "left_star"></span>
        <asp:TextControl ID = "tcDisplayText" runat = "server" IsRequired = "true" TextType = "String" MaxLength = "50" />
    </div>
</div>
<div class="cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnCount" runat="server" onclick="btnFunction_Click" CommandArgument = "1" Text = "数量Count" OnClientClick="return Validate();"/>
    <asp:LinkButton ID="btnSum" runat="server" onclick="btnFunction_Click" CommandArgument = "2"  Text = "求和Sum" OnClientClick="return Validate();"/>
    <asp:LinkButton ID="btnAvg" runat="server" onclick="btnFunction_Click" CommandArgument = "4"  Text = "平均Avg" OnClientClick="return Validate();"/>
    <asp:LinkButton ID="btnMax" runat="server" onclick="btnFunction_Click" CommandArgument = "8"  Text = "最大Max" OnClientClick="return Validate();"/>
    <asp:LinkButton ID="btnMin" runat="server" onclick="btnFunction_Click" CommandArgument = "16"  Text = "最小Min" OnClientClick="return Validate();"/>
</div>
<div class = "grid_title">
</div>
<div class="grid_filter"></div>
<asp:GridControl ID="gcList" runat="server"  AutoGenerateColumns="false" ShowFooter="false">
    <Columns>
        <asp:HeaderSortField DataTextField="DisplayText"  HeaderText="显示名称"  ></asp:HeaderSortField>
        <asp:HeaderSortField DataTextField="DefaultDisplayText"  HeaderText="默认名称"  ></asp:HeaderSortField>
        <asp:HeaderSortField DataTextField="SumType"  HeaderText="汇总方式"  ></asp:HeaderSortField>
        <asp:OperationField>
        <Buttons>
                <asp:LinkButton ID="btnDelete" runat="server" Text="删除" CommandArgument='SumFiledId' OnClientClick="if (!confirm('是否删除该查询条件？')) {return false;}" OnClick = "btnDelete_Click"></asp:LinkButton>
        </Buttons>
        </asp:OperationField>
    </Columns>
</asp:GridControl>

<div class="button_bar">
    <asp:LinkButton ID="btnPre"  runat="server" Text="上一步" OnClick = "btnPre_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnNext"  runat="server" Text="下一步" OnClick = "btnNext_Click"></asp:LinkButton>
</div>
</asp:Content>
