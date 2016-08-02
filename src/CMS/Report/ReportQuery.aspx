<%@ Page Title="报表配置查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportQuery.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportQuery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3 id = "queryCondition" style = "cursor:pointer"><span id = "spIsCanShrink" >+</span>订单查询</h3>
<div class="allcol2">
    <div class = "item_box_col1">
    <span>报表名称</span>
    <span class = "left_star"></span>
        <sp:SText ID = "tcReportName" runat="server" TextType = "String">  </sp:SText>
    </div>
    <div id = "moreCondition">
    <div class = "item_box_col1">
    <span>模式</span>
    <span class = "left_star"></span>
        <asp:ComboControl ID = "ccReportModel" runat="server"  DropdownType = "DropdownList"></asp:ComboControl>
    </div>
    <div class = "item_box_col1">
    <span>状态</span>
    <span class = "left_star"></span>
        <asp:ComboControl ID = "ccState" runat="server"  DropdownType = "DropdownList"></asp:ComboControl>
    </div>
    </div>
</div>
<div class="cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnQuery" runat="server" onclick="btnQuery_Click" Text = "查询" CssClass = "btnQuery"/>
    <asp:LinkButton ID="btnClear" runat="server" onclick="btnClear_Click" Text = "重置" CssClass = "btnQuery"/>
</div>
<div class = "grid_title">
</div>
<div class="grid_filter"></div>
<asp:GridControl ID="gcList" OnPageIndexChanging="gcList_PageIndexChanging" runat="server"  AutoGenerateColumns="false" ShowFooter="false" onrowdatabound="gcList_RowDataBound">
    <Columns>
        <asp:HeaderSortField DataTextField="ReportName"  HeaderText="报表名称"  ></asp:HeaderSortField>
        <asp:HeaderSortField DataTextField="ReportModel"  HeaderText="报表模式"  ></asp:HeaderSortField>
        <asp:HeaderSortField DataTextField="EntityDisPlayText"  HeaderText="查询实体"  ></asp:HeaderSortField>
        <asp:HeaderSortField DataTextField="State"  HeaderText="状态"  ></asp:HeaderSortField>
    
        <asp:OperationField>
        <Buttons>
                <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='ReportId' OnClick="lbtnEdit_Click" >编辑</asp:LinkButton>
                <asp:LinkButton ID="lbtnDisable" runat="server" CommandArgument='ReportId' OnClientClick="if (!confirm('是否禁用该报表？')) {return false;}" OnClick="lbtnDisable_Click" Visible = "false">禁用</asp:LinkButton>
                <asp:LinkButton ID="lbtnDel" runat="server" CommandArgument='ReportId' OnClientClick="if (!confirm('是否删除该报表？')) {return false;}" OnClick="lbtnDel_Click">删除</asp:LinkButton>
        </Buttons>
        </asp:OperationField>

    </Columns>
    <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    </PagerSettings>
</asp:GridControl>

<div class="button_bar">
    <asp:LinkButton ID="btnAdd" runat="server" Text="添加" OnClick = "btnAdd_Click" /> 
</div>
</asp:Content>
