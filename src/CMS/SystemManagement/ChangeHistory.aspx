<%@ Page Title="数据变更历史" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangeHistory.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ChangeHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="check_info">
        <asp:Repeater ID="rpLog" runat="server" onitemdatabound="rpLog_ItemDataBound">
            <ItemTemplate>
            <ul onclick="$(this).next().toggle(500)">
                <li>
                <div>
                    <label>变更时间：</label>
                    <asp:Label ID = "lblChangeTime" runat = "server" Text='<%# Eval("ChangeTime") %>'></asp:Label>
                </div>
                <div>
                    <label>变更人：</label>
                    <asp:Label ID = "lblChangeUser" runat = "server" Text='<%# Eval("ChangeUser") %>'></asp:Label>
                </div>
                </li>
             </ul>
             <div class="check_info_son" style="display:none">
                <asp:Repeater ID="rpLogDetail" runat="server">
                    <ItemTemplate>
                    <ul>
                        <li>
                        <div>
                            <label>字段名称：</label>
                            <asp:Label ID = "lblFieldDesc" runat = "server" Text='<%# Eval("DisplayText") %>'></asp:Label>
                        </div>
                        <div>
                            <label>初始值：</label>
                            <asp:Label ID = "lblOriginalValue" runat = "server" Text='<%# Eval("OriginalValue") %>'></asp:Label>
                        </div>
                        <div>
                            <label>变更值：</label>
                            <asp:Label ID = "lblCurrentValue" runat = "server" Text='<%# Eval("CurrentValue") %>'></asp:Label>
                        </div>
                        </li>
                    </ul>
                    </ItemTemplate>
                </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="lbtnReturn" runat="server" UseSubmitBehavior = "false" Text="返回" onclick = "lbtnReturn_Click"/>
    </div>
</asp:Content>
