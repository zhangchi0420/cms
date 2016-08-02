<%@ Page Title="站内信详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SysRemindDetail.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.SysRemindDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            摘要</h3>
        <div class="allcol2">
            <div class="item_box_full">
                <span>主题</span>
                <div class="item_display">
                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>发件人</span>
                <div class="item_display">
                    <asp:Label ID="lblCreateUser" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>收件人</span>
                <div class="item_display">
                    <asp:Label ID="lblOwner" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>发送时间</span>
                <div class="item_display">
                    <asp:Label ID="lblCreateTime" runat="server"></asp:Label>
                </div>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="main_box">
        <h3>
            内容</h3>
        <asp:HtmlEditorControl ID="txtContent" runat="server" ReadOnly="true" />
    </div>
    <div class="button_bar">
        <a href="../SystemManagement/SysRemindManagement.aspx">返回</a>
    </div>
</asp:Content>
