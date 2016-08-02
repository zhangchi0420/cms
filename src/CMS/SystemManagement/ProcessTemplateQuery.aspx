<%@ Page Title="流程提醒模板查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessTemplateQuery.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ProcessTemplateQuery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 100%;">
        <asp:GridControl ID="gcProcessTemplateQuery" ClientInstanceName="gcProcessTemplateQuery" runat="server" AutoGenerateColumns="false" OnRowDataBound = "gcProcessTemplateQuery_RowDataBound">
        <Columns>
            <asp:HeaderSortField DataTextField="TemplateName" HeaderText="模板名称"></asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="TemplateType" HeaderText="模板类型"></asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="UseTimeType" HeaderText="执行时机"></asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="ProcessEntityName" HeaderText="流程实体"></asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="ActivityEntityName" HeaderText="活动实体"></asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="State" HeaderText="模板状态"></asp:HeaderSortField>
            <asp:TemplateField HeaderText="操作" ShowHeader="False">
            <ItemTemplate>
                    <asp:LinkButton ID="lbtnEdit" runat="server" Text="编辑" CommandArgument='<%#Eval("TemplateId")%>' PostBackUrl = '<%# "~/SystemManagement/ProcessTemplate.aspx?id=" + Eval("TemplateId")%>'></asp:LinkButton>
                    <asp:LinkButton ID="lbtnEnable" runat="server" Text="启用" CommandArgument='<%#Eval("TemplateId")%>' OnClick="lbtnEnable_Click"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnDisable" runat="server" Text="禁用" CommandArgument='<%#Eval("TemplateId")%>' OnClick="lbtnDisable_Click"></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridControl>
    </div>
    <div class="cl"></div>
    <div class="button_bar">
        <asp:Button ID="lbtnAdd" runat="server" Text="新增" PostBackUrl = "~/SystemManagement/ProcessTemplate.aspx" />
    </div>
</asp:Content>
