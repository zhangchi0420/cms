<%@ Page Title="站内信发送" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SysRemindAdd.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.SysRemindAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            概要</h3>
        <div class="allcol2">
            <div class="item_box_full">
                <span>主题</span><span class="left_star"></span>
                <asp:TextControl ID="txtTitle" runat="server" CssClass="item_input" MaxLength="100" />
            </div>
            <div class="item_box_full">
                <span>收件人</span><span class="left_star">*</span>
                <asp:SelectControl ID="scReceiveUser" ClientInstanceName="sc2" runat="server" SelectType="List"
                    IsRequired="true">
                    <ListSettings IsGrouped="true">
                    </ListSettings>
                </asp:SelectControl>
            </div>
            <div class="cl">
            </div>
        </div>
        <div class="cl">
        </div>
    </div>
    <div class="main_box">
        <h3>
            内容</h3>
        <asp:HtmlEditorControl ID="txtContent" runat="server" />
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="return Validate();" OnClick="btnSave_Click">发送</asp:LinkButton>
        <a href="../SystemManagement/SysRemindManagement.aspx">返回</a>
    </div>
</asp:Content>
