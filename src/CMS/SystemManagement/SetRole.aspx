<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetRole.aspx.cs"
    Inherits="Drision.Framework.Web.SystemManagement.SetRole" Title="设置角色" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid_content">
        <p class="grid_title">
            角色列表</p>
        <div class="grid_filter">
            <asp:Label ID="lblUserName" runat="server" Text="用户姓名："></asp:Label>
            <asp:DropDownList ID="ddlRoleList" runat="server" Width="130px" AutoPostBack="true"
                Visible="false" OnSelectedIndexChanged="ddlRoleList_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <asp:GridView ID="gvShow" runat="server" AutoGenerateColumns="False" Width="100%"
            GridLines="Vertical" DataKeyNames="Role_ID" EmptyDataText="没有符合条件的信息！">
            <AlternatingRowStyle BackColor="#E5EAEF" />
            <HeaderStyle Wrap="false" />
            <Columns>
                <asp:TemplateField HeaderText="选择" HeaderStyle-BackColor="#94B0CE">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("CheckedFlag") %>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="角色ID" DataField="Role_ID" Visible="False">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                </asp:BoundField>
                <asp:BoundField HeaderText="角色名称" HeaderStyle-BackColor="#94B0CE" DataField="Role_Name">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                </asp:BoundField>
                <asp:BoundField HeaderText="备注" HeaderStyle-BackColor="#94B0CE" DataField="Role_Comment">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" Width="99%" PrevPageText="上一页"
            FirstPageText="首页" NextPageText="下一页" LastPageText="末页" ShowPageIndexBox="Always"
            PageIndexBoxType="TextBox" HorizontalAlign="right" PageSize="12" OnPageChanged="AspNetPager1_PageChanged"
            EnableTheming="true" ShowCustomInfoSection="Left" CustomInfoSectionWidth="32%">
        </webdiyer:AspNetPager>
        <div class="grid_foot">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="button_bar">
                            <asp:Button ID="btnAdd" runat="server" Text="保存" OnClick="btnAdd_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
                        </div>
                    </td>
                    <td>
                        <div class="grid_page_bar">
                            <ul class="page_bar">
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
