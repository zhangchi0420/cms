<%@ Page Title="流程查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProcessQuery.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ProcessQuery" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
    <h3>流程查询</h3>
    <div class="allcol2">
        <div class="item_box_col1">
            <span>流程名称 </span><span class="left_star">&nbsp;</span>
            <asp:TextControl ID="tbT_ProcessName" runat="server" TextType="String" FieldName="ProcessName"
                Tag="8" />
        </div>
        <div class="cl">
        </div>
    </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查询" />
        <asp:LinkButton ID="btnClearCondition" runat="server" OnClick="btnClearCondition_Click"
            Text="重置" />
    </div>
    <div class="grid_title" id="divViewControlTitle">
        流程信息</div>
    <div id="divviewcontrol" runat="server">
        <asp:GridControl ID="gcProcess" ClientInstanceName="PopUpValueText" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:HeaderSortField HeaderText="流程ID" DataTextField="ProcessId"></asp:HeaderSortField>
            <asp:HeaderSortField HeaderText="流程名称" DataTextField="ProcessName"></asp:HeaderSortField>
            <asp:HeaderSortField HeaderText="流程类型" DataTextField="ProcessCategory"></asp:HeaderSortField>
            <asp:HeaderSortField HeaderText="流程实体" DataTextField="EntityName"></asp:HeaderSortField>
            <asp:HeaderSortField HeaderText="活动实体" DataTextField="ActivityEntityName"></asp:HeaderSortField>
            <asp:HeaderSortField HeaderText="状态" DataTextField="ProcessStatus"></asp:HeaderSortField>
            <asp:HeaderSortField HeaderText="版本" DataTextField="ProcessVersion"></asp:HeaderSortField>
            <asp:TemplateField HeaderText="操作" ShowHeader="False">
            <ItemTemplate>
                 <asp:LinkButton ID="lbtnDetail" runat="server" Text="代理设置" CommandArgument='<%#Eval("ProcessId")%>' PostBackUrl = '<%#"~/SystemManagement/ProcessDetail.aspx?id=" + Eval("ProcessId")%>'></asp:LinkButton>
                 <asp:LinkButton ID="btnDesign" runat="server" Text="流程设计" CommandArgument='<%#Eval("ProcessId") %>' PostBackUrl = '<%#"~/SystemManagement/ProcessDesigner.aspx?id=" + Eval("ProcessId")%>'></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
        </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
