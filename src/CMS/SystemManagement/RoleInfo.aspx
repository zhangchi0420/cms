<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RoleInfo.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.RoleInfo"
    Title="角色管理" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="com_box">
                <div class="title">
                    <h3>
                        角色信息列表</h3>
                </div>
                <div class="Content" style="text-align: center">
                    <div class="tab">
                        <div style="width: 350px; margin-left: 230px; padding: 10px 0; float: left;">
                            <span style="height: 29px; line-height: 29px; display: block; float: left; overflow: hidden;">
                                <asp:Label ID="Label5" runat="server" Text="角色名称："></asp:Label>
                                <asp:TextBox ID="txtSearchName" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                            </span><span style="height: 29px; line-height: 29px; margin-top: -8px; margin-left: 10px;
                                display: block; float: left;">
                                <asp:Button ID="btnSearch" runat="server" CausesValidation="False" CssClass="chaxun t5"
                                    OnClick="btnSearch_Click" />
                                <asp:Button ID="btnAddPanel" runat="server" CausesValidation="False" CssClass="tianjia t5"
                                    OnClick="btnAddPanel_Click" />
                            </span>
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center">
                            <tr>
                                <td>
                                    <asp:GridView ID="gvShow" runat="server" AutoGenerateColumns="False" Width="100%"
                                        GridLines="Vertical" OnRowDataBound="gvShow_RowDataBound" OnRowCommand="gvShow_RowCommand"
                                        EmptyDataText="没有符合条件的信息！">
                                        <AlternatingRowStyle BackColor="#E5EAEF" />
                                        <HeaderStyle Wrap="false" />
                                        <Columns>
                                            <asp:BoundField HeaderText="角色ID" HeaderStyle-BackColor="#94B0CE" DataField="Role_ID"
                                                Visible="False">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="角色名称" HeaderStyle-BackColor="#94B0CE" DataField="Role_Name">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="是否停用">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblState" runat="server" Text='<%# Bind("Role_Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="操作" HeaderStyle-BackColor="#94B0CE">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnState" runat="server" CssClass="links" CausesValidation="False"
                                                        CommandName="state" OnClientClick="if (!confirm('是否启用角色？')) {return false;}">启用</asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="links" CausesValidation="False"
                                                        CommandName="up">编辑</asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnDel" runat="server" CssClass="links" CausesValidation="False"
                                                        CommandName="del" OnClientClick="if (!confirm('是否删除角色？')) {return false;}">删除</asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnSet" runat="server" CssClass="links" CausesValidation="False"
                                                        CommandName="set">设置权限</asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" Width="99%" PrevPageText="上一页"
                                        FirstPageText="首页" NextPageText="下一页" LastPageText="末页" ShowPageIndexBox="Always"
                                        PageIndexBoxType="TextBox" HorizontalAlign="right" PageSize="12" OnPageChanged="AspNetPager1_PageChanged"
                                        EnableTheming="true" ShowCustomInfoSection="Left" CustomInfoSectionWidth="32%">
                                    </webdiyer:AspNetPager>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <asp:Panel ID="Panel1" runat="server" Visible="false">
                <div class="com_box">
                    <div class="title">
                        <h3>
                            角色详细信息</h3>
                    </div>
                    <div class="Content" style="text-align: center">
                        <div class="tab">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center">
                                <tr>
                                    <td style="text-align: right;">
                                        角色名称：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtName" runat="server" MaxLength="20"></asp:TextBox>
                                        <span style="color: Red">*</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName"
                                            ErrorMessage="角色名称不能为空！" Display="None">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right;" rowspan="2">
                                        备注：
                                    </td>
                                    <td style="text-align: left;" rowspan="2">
                                        <asp:TextBox ID="txtRemark" runat="server" Rows="3" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REV" runat="server" ControlToValidate="txtRemark"
                                            ValidationExpression="^(\s|\S){0,100}$" ErrorMessage="备注不能超过100个字！" Display="None">*</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        是否停用：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:RadioButtonList ID="rbtnState" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0">是</asp:ListItem>
                                            <asp:ListItem Value="1" Selected="True">否</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEdit" runat="server" Visible="False"></asp:Label>
                                    </td>
                                    <td style="text-align: left;">
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td style="width: 165px;">
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" Text="Button" CssClass="baocun" OnClick="btnAdd_Click" />
                                                    <asp:Button ID="btnEdit" runat="server" Text="Button" CssClass="baocun" Visible="false"
                                                        OnClick="btnAdd_Click" />
                                                    <asp:Button ID="btnCancle" runat="server" Text="Button" CssClass="quxiao" OnClick="btnCancle_Click"
                                                        CausesValidation="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                            ShowSummary="False" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
