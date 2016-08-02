<%@ Page Title="表单实例审核" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormInstanceApprove.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormInstanceApprove" %>

<%@ Register Src="FormPreviewControl.ascx" TagName="FormPreviewControl" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            表单内容</h3>
        <asp:FormPreviewControl ID="formPreview" runat="server" />
    </div>
    <div class="main_box">
        <h3>
            审核意见</h3>
        <div class="allcol2">
            <div class="item_box_full">
                <span>审核意见</span><span class="left_star">&nbsp;</span>
                <asp:TextControl ID="txtApproveComment" runat="server" ValidationGroup="Reject" IsRequired="True"
                    TextType="Text" MaxLength="1000" />
            </div>
            <div runat="server" id="divIsAddUser" class="item_box_col1">
                <span>是否加签</span><span class="left_star">&nbsp;</span>
                <asp:CheckControl AutoPostBack="true" ID="ccIsAddUser" runat="server" OnCheckedChanged="ccIsAddUser_CheckedChanged" />
            </div>
            <div runat="server" id="divSelectAddUser" class="item_box_col1">
                <span>加签人</span><span class="left_star">&nbsp;</span>
                <asp:SelectControl ID="scAddUserId" runat="server" ReadOnly="true">
                    <PopupSettings ScrollBars="None" Height="600" Width="850" Title="请选择签核人" />
                    <IFrameSettings ContentUrl="../SystemManagement/UserSelect.aspx" ReturnControl="PopUpValueText" />
                </asp:SelectControl>
            </div>
        </div>
        <div class="cl">
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="btnPass" runat="server" Text="通过" OnClientClick="return ShowWaiting(false);"
            OnClick="btnPass_Click"></asp:LinkButton>
        <asp:LinkButton ID="btnReject" runat="server" Text="驳回" ValidationGroup="Reject"
            OnClientClick="if(ValidateGroup('Reject') == true){ShowWaiting();return true;}else{return false;}"
            OnClick="btnReject_Click"></asp:LinkButton>
        <asp:Repeater ID="rReject" runat="server">
            <ItemTemplate>
                <asp:LinkButton ID="btnTrueReject" runat="server" CommandArgument='<%# Eval("ActivityId") %>' Text='<%# Eval("ButtonName") %>' ValidationGroup="Reject"
                OnClientClick="if(ValidateGroup('Reject') == true){ShowWaiting();return true;}else{return false;}"
            OnClick="btnTrueReject_Click"></asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
        <asp:LinkButton ID="btnReturn" runat="server" Text="返回" PostBackUrl="~/Home/HomePage.aspx"></asp:LinkButton>
    </div>
</asp:Content>
