<%@ Page Title="操作记录查看" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MobileOperationRecordDetail.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.MobileOperationRecordDetail" %>

<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            基本信息</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>执行人</span>
                <div class="item_display">
                    <asp:Label ID="lblCreateUserId" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>执行时间</span>
                <div class="item_display">
                    <asp:Label ID="lblCreateTime" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_full">
                <span>操作记录</span>
                <div class="item_display">
                    <asp:Label ID="lblOperationRecord" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>状态</span>
                <div class="item_display">
                    <asp:Label ID="lblState" runat="server"></asp:Label>
                </div>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="lbtnReturn" runat="server" UseSubmitBehavior="false" OnClick="btnReturn_Click">返回</asp:LinkButton>
    </div>
</asp:Content>
