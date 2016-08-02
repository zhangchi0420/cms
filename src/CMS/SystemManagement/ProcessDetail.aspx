<%@ Page Title="流程代理详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProcessDetail.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ProcessDetail" %>

<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>流程信息</h3>
        <div class="allcol2">
        <div class="item_box_col1">
            <span>流程名称</span>
            <div class="item_display">
                <asp:Label ID="lblProcessName" runat="server"></asp:Label>
            </div>
        </div>
        <div class="item_box_col1">
            <span>状态</span>
            <div class="item_display">
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </div>
        </div>
        <div class="item_box_full">
            <span>流程描述</span>
            <div class="item_display">
                <asp:Label ID="lblProcessDescription" runat="server"></asp:Label>
            </div>
        </div>
        <div class="cl">
        </div>
    </div>
    </div>
    
    
    <div class="grid_title" id = "divViewControlTitle">代理设置列表</div>
        <div id = "divProcessProxy" runat = "server">
        <asp:GridControl ID="gcProcessProxy" ClientInstanceName="PopUpValueText" runat="server" AutoGenerateColumns="false" OnRowDataBound = "gcProcessProxy_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <div>负责人</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblOwner" runat="server" Text='<%# GetUserName(Eval("OwnerId")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField>
                <HeaderTemplate>
                    <div>代理人</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblProxy" runat="server" Text='<%# GetUserName(Eval("ProxyId")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <HeaderTemplate>
                    <div>起始时间</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblStartTime" runat="server" Text='<%# string.Format("{0:yyyy-MM-dd}",Eval("StartTime")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <HeaderTemplate>
                    <div>结束时间</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblEndTime" runat="server" Text='<%# string.Format("{0:yyyy-MM-dd}",Eval("EndTime")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <HeaderTemplate>
                    <div>状态</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" Text='<%# GetStatusName(Eval("Status")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作" ShowHeader="False">
            <ItemTemplate>
                 <asp:LinkButton ID="lbtnEdit" runat="server" Visible = "false" Text="编辑" CommandArgument='<%#Eval("ProcessProxyId")%>' PostBackUrl = '<%#"~/SystemManagement/ProcessProxyAdd.aspx?id=" + Eval("ProcessProxyId")%>'></asp:LinkButton>
                 <asp:LinkButton ID="lbtnDel" runat="server"  Visible = "false" Text="删除" CommandArgument='<%#Eval("ProcessProxyId")%>' OnClientClick="if (!confirm('是否删除该记录？')) {return false;}" OnClick="lbtnDel_Click"></asp:LinkButton>
                 <asp:LinkButton ID="lbtnEnable" runat="server"  Visible = "false" Text="启用" CommandArgument='<%#Eval("ProcessProxyId")%>' OnClick = "lbtnEnable_Click"></asp:LinkButton>
                 <asp:LinkButton ID="lbtnDisable" runat="server" Text="禁用" CommandArgument='<%#Eval("ProcessProxyId")%>' OnClick = "lbtnDisable_Click"></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridControl>
        </div>
<div class="button_bar">
<asp:LinkButton ID="btnAdd" runat="server" UseSubmitBehavior = "false" Text="添加"/>
<asp:LinkButton ID="btnReturn" runat="server" UseSubmitBehavior = "false" Text="返回" PostBackUrl = "~/SystemManagement/ProcessQuery.aspx"/>
</div>
</asp:Content>
