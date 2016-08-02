<%@ Page Title="表单实例新增" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormInstanceAdd.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormInstanceAdd" %>

<%@ Register Src="FormPreviewControl.ascx" TagName="FormPreviewControl" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            表单内容</h3>
        <asp:FormPreviewControl ID="formPreview" runat="server" />
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click"></asp:LinkButton>
        <asp:Repeater ID="rApply" runat="server">
            <ItemTemplate>
                <asp:LinkButton ID="btnApply" runat="server" Text='<%# Eval("ButtonName") %>' CommandArgument='<%# Eval("ProcessName") %>'
                    OnClick="btnApply_Click" OnClientClick="return ShowWaiting(false);"></asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
        <asp:LinkButton ID="btnReturn" runat="server" Text="返回" PostBackUrl="~/FormWorkflow/FormQuery.aspx"></asp:LinkButton>
    </div>
</asp:Content>
