<%@ Page Title="设置按钮权限" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OperationPrivilegeOfRole.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.OperationPrivilegeOfRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="allcol2" style="padding-bottom: 15px;">
        <div class="item_box_col1">
            <span>角色名称 </span><span class="left_star">&nbsp;</span>
            <asp:ComboControl ID="cbRole" ClientInstanceName="cbRole" DataTextField="Role_Name"
                DataValueField="Role_ID" runat="server" AutoSelectFirst="true" DropdownType="DropdownList"
                ShowEmptyItem="false" OnTextChanged="ddlRoleList_SelectedIndexChanged" AutoPostBack="true">
            </asp:ComboControl>
        </div>
    </div>
    <asp:ComboControl ID="cbPage" ClientInstanceName="cbPage" DataTextField="PageName"
        DataValueField="ControlId" runat="server" AutoSelectFirst="true" DropdownType="DropdownList"
        ShowEmptyItem="true" Visible="false">
    </asp:ComboControl>
    <br />
    <asp:Repeater runat="server" ID="dataListOperation" OnItemDataBound="dataListOperation_ItemDataBound">
        <ItemTemplate>
            <div class="main_box">
                <div class="grid_title">
                    <%--<asp:CheckBox ID="CheckBoxAll" runat="server" OnCheckedChanged="CheckBoxOperation_CheckedChanged"
                    AutoPostBack="True" />--%>
                    <h3>
                        <%# Eval("DisplayText")%>(<%# Eval("PageName")%>)</h3>
                    页面按钮
                    <asp:HiddenField ID="hiddenPageName" runat="server" Value='<%# Eval("PageName") %>' />
                </div>
                <div class="grid_tbody">
                    <div class="grid_content  mb10">
                        <asp:DataList ID="dataList" runat="server" DataKeyField="OperationName" OnPreRender="dataList_PreRender"
                            RepeatColumns="6" OnItemDataBound="dataList_ItemDataBound">
                            <ItemStyle HorizontalAlign="Left" Width="16%" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hiddenOp" runat="server" Value='<%# Eval("OperationName") %>' />
                                <asp:HiddenField ID="hiddenPage" runat="server" Value='<%# Eval("PageName") %>' />
                                &nbsp;&nbsp;<asp:CheckBox ID="CheckBoxItem" runat="server" />
                                <asp:Label ID="Permissions_NameLabel" runat="server" Text='<%# Eval("OperationDisplayText") %>' />&nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
                <div>
                    <asp:Repeater runat="server" ID="dataListView" OnItemDataBound="dataListView_ItemDataBound">
                        <ItemTemplate>
                            <div class="grid_title">
                                <%--<asp:CheckBox ID="CheckBoxAll" runat="server" OnCheckedChanged="CheckBoxView_CheckedChanged"
                                AutoPostBack="True" />--%>
                                <%# Eval("DisplayText")%>
                                <asp:HiddenField ID="hiddenViewName" runat="server" />
                            </div>
                            <div class="grid_tbody">
                                <div class="grid_content  mb10">
                                    <asp:DataList ID="dataListViewOp" runat="server" DataKeyField="OperationName" OnItemDataBound="dataListViewOp_ItemDataBound"
                                        RepeatColumns="6">
                                        <ItemStyle HorizontalAlign="Left" Width="16%" />
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hiddenViewOp" runat="server" Value='<%# Eval("OperationName") %>' />
                                            <asp:HiddenField ID="hiddenViewPage" runat="server" Value='<%# Eval("PageName") %>' />
                                            &nbsp;&nbsp;<asp:CheckBox ID="CheckBoxItem" runat="server" />
                                            <asp:Label ID="PermissionsView_NameLabel" runat="server" Text='<%# Eval("OperationDisplayText") %>' />&nbsp;&nbsp;
                                        </ItemTemplate>
                                    </asp:DataList>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="grid_foot">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div class="button_bar">
                        <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="btnAdd_Click">保存</asp:LinkButton>
                        <%--<asp:LinkButton ID="lbtnCancel" runat="server" OnClick="btnCancel_Click">取消</asp:LinkButton>--%>
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
</asp:Content>
