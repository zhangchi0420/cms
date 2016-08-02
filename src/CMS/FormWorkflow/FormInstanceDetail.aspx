<%@ Page Title="表单实例详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormInstanceDetail.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormInstanceDetail" %>
<%@ Register Src="FormPreviewControl.ascx" TagName="FormPreviewControl" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
        <h3>
            表单内容</h3>
    <asp:FormPreviewControl ID="formPreview" runat="server" IsDetailPage="True" />
    </div>
    <div class="button_bar">
        <asp:Repeater ID="rApply" runat="server">
            <ItemTemplate>
                <asp:LinkButton ID="btnApply" runat="server" Text='<%# Eval("ButtonName") %>' CommandArgument='<%# Eval("ProcessName") %>'
                    OnClick="btnApply_Click" OnClientClick="return ShowWaiting(false);"></asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
        <asp:LinkButton ID="btnReturn" runat="server" Text="返回" OnClick="btnReturn_Click"></asp:LinkButton>
    </div>
</asp:Content>
