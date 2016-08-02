<%@ Page Title="表单设计第四步（预览发布）" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormDesigner_FourthStep.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormDesigner_FourthStep" %>
<%@ Register src="FormPreviewControl.ascx" tagname="FormPreviewControl" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:FormPreviewControl ID="formPreview" runat="server" />
<br />
<div class="button_bar">
    <asp:LinkButton ID="btnPrev" runat="server" Text="上一步" OnClick="btnPrev_Click" ></asp:LinkButton>
    <asp:LinkButton ID="btnPublish" runat="server" Text="发布" OnClientClick="return ShowWaiting(false);" OnClick="btnPublish_Click"></asp:LinkButton>
</div>
</asp:Content>
