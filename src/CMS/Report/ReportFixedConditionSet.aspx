<%@ Page Title="报表固定条件" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportFixedConditionSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportFixedConditionSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
    <asp:TreeControl ID="tcField" runat="server" AutoPostBack = "true" CssClass = "treebox"  ExpandAllNodes = "True"  OnNodeClick = "tcField_NodeClick"></asp:TreeControl>
    <div class= "treeform">
        <div class="allcol1">
            <div class = "item_box_full">
            <span>字段名称</span>
            <span class = "left_star"></span>
                <asp:Label ID = "lbConditionField" runat="server"/>
            </div>
            <div class = "item_box_full">
            <span>比较方式</span>
            <span class = "left_star"></span>
                <asp:ComboControl runat = "server" ID = "ccCompareType" DropdownType = "DropdownList" AutoPostBack = "true" OnTextChanged = "ccCompareType_TextChanged"></asp:ComboControl>
            </div>
            <div class = "item_box_full">
            <span>比较值</span>
            <span class = "left_star"></span>
                <asp:ComboControl runat = "server" ID = "tcConditionValue" IsRequired = "true" DropdownType = "AutoComplete"></asp:ComboControl>
            </div>
        </div>
        <div class="button_bar">
            <asp:LinkButton ID="btnAdd" runat="server"  Text="添加" OnClientClick="return Validate();" OnClick = "btnAdd_Click"/> 
        </div>
        <div class = "grid_title">已配置条件</div>
        <div class="grid_filter"></div>
        <asp:GridControl ID="gcList" runat="server"  AutoGenerateColumns="false" ShowFooter="false" KeyFieldName = "ConditionId" SingleSelect = "false" >
            <Columns>
                <asp:FullCheckField DataTextField = "DisPlayText"></asp:FullCheckField>
                <asp:BoundField  DataField ="DisPlayText"  HeaderText="条件名称"  ></asp:BoundField>
                <asp:OperationField>
                <Buttons>
                        <asp:LinkButton ID="lbtnDel" runat="server" Text="删除" CommandArgument='ConditionId' OnClientClick="if (!confirm('是否删除该固定条件？')) {return false;}" OnClick = "btnDelete_Click"></asp:LinkButton>
                </Buttons>
                </asp:OperationField>
            </Columns>
        </asp:GridControl>
        <div class="button_bar" style = "text-align:left">
            <asp:LinkButton ID="btnAnd"  runat="server" Text="And" OnClick = "btnAnd_Click"></asp:LinkButton> 
            <asp:LinkButton ID="btnOr"  runat="server" Text="Or" OnClick = "btnOr_Click"></asp:LinkButton> 
        </div>
    </div>
    <div class = "cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnPre"  runat="server" Text="上一步" OnClick = "btnPre_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnNext"  runat="server" Text="下一步" OnClick = "btnNext_Click"></asp:LinkButton> 
</div>
</asp:Content>
