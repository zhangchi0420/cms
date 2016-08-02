<%@ Page Title="报表查询条件" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportQueryConditionSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportQueryConditionSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
    <asp:TreeControl ID="tcField" runat="server" AutoPostBack = "true" CssClass = "treebox" ExpandAllNodes = "True"  OnNodeClick = "tcField_NodeClick"></asp:TreeControl>
    <div class= "treeform">
        <div class = "main_box">
        <div class="allcol1">
            <div class = "item_box_full">
            <span>字段名</span>
            <span class = "left_star"></span>
                <asp:Label ID = "lbDisplayText" runat="server" ></asp:Label>
            </div>
            <div class = "item_box_full">
            <span>显示名称</span>
            <span class = "left_star">*</span>
                <asp:TextControl ID = "tcDisplayText" runat = "server" TextType = "String" IsRequired = "true" />
            </div>
            <div class = "item_box_full">
            <span>是否占整行</span>
            <span class = "left_star"></span>
                <asp:CheckControl ID = "ccIsFullRow" runat = "server" />
            </div>
            <div class = "item_box_full">
            <span>是否含子查询</span>
            <span class = "left_star"></span>
                <asp:CheckControl ID = "ccIsSubQuery" runat = "server" AutoPostBack = "true" OnCheckedChanged = "ccIsSubQuery_CheckedChanged"/>
            </div>
            <div class = "item_box_full">
            <span>比较方式</span>
            <span class = "left_star">*</span>
                <asp:ComboControl ID = "ccCompareType" runat = "server" IsRequired = "true" DropdownType = "DropdownList"></asp:ComboControl>
            </div>
        </div>
        <div class = "cl"></div>
        </div>
        <div class="button_bar">
            <asp:LinkButton ID="btnAdd" runat="server"  Text="添加" OnClientClick="return Validate();" OnClick = "btnAdd_Click"/> 
        </div>
        <div class = "grid_title">已有的查询条件</div>
        <div class="grid_filter"></div>
        <asp:GridControl ID="gcList" runat="server"  AutoGenerateColumns="false" ShowFooter="false">
            <Columns>
                <asp:BoundField  DataField ="OrderIndex"  HeaderText="序号"  ></asp:BoundField>
                <asp:BoundField  DataField ="FieldName"  HeaderText="字段名"  ></asp:BoundField> 
                <asp:BoundField  DataField ="DisplayText"  HeaderText="显示名称"  ></asp:BoundField>
                <asp:BoundField  DataField ="IsFullRow"  HeaderText="是否整行"  ></asp:BoundField>
                <asp:BoundField  DataField ="IsSubQuery"  HeaderText="是否包含子查询"  ></asp:BoundField>
                <asp:BoundField  DataField ="CompareType"  HeaderText="比较方式"  ></asp:BoundField>
                <asp:OperationField>
                <Buttons>
                    <asp:LinkButton ID="btnMoveUp" runat="server" Text="上移" CommandArgument='ConditionId' OnClick = "btnMoveUp_Click"></asp:LinkButton>
                    <asp:LinkButton ID="btnMoveDown" runat="server" Text="下移" CommandArgument='ConditionId' OnClick = "btnMoveDown_Click"></asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" Text="删除" CommandArgument='ConditionId' OnClientClick="if (!confirm('是否删除该查询条件？')) {return false;}" OnClick = "btnDelete_Click"></asp:LinkButton>
                </Buttons>
                </asp:OperationField>
            </Columns>
        </asp:GridControl>
        <div class = "main_box">
            <div class="allcol1">
            <div class = "item_box_full">
            <span>是否分组汇总</span>
            <span class = "left_star"></span>
                <asp:CheckControl ID = "ccIsGroupSum" runat = "server" />
            </div>
            </div>
            <div class = "cl"></div>
        </div>
    </div>
    <div class = "cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnPre"  runat="server" Text="上一步" OnClick = "btnPre_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnNext"  runat="server" Text="下一步" OnClick = "btnNext_Click"></asp:LinkButton> 
</div>
</asp:Content>
