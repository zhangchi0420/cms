<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RightOfMenu.aspx.cs"
    Inherits="Drision.Framework.Web.SystemManagement.RightOfMenu" Title="设置功能" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            角色</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>角色名称 </span>
                <asp:Label ID="lblRoleName" runat="server"></asp:Label>
                <asp:ComboControl ID="ddlRoleList" runat="server" AutoSelectFirst="true" AutoPostBack="true"
                    Visible="false" OnTextChanged="ddlRoleList_SelectedIndexChanged">
                </asp:ComboControl>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <asp:Repeater runat="server" ID="dataListModule" OnItemDataBound="dataListModule_ItemDataBound">
        <ItemTemplate>
            <div class="grid_title">
                <asp:CheckBox ID="CheckBoxAll" runat="server" OnCheckedChanged="CheckBoxModule_CheckedChanged"
                    AutoPostBack="True" />
                <%# Eval("Permission_Name") %>——<%# Eval("Category") %>
            </div>
            <div class="grid_tbody">
                <div class="grid_content  mb10">
                    <asp:DataList ID="dataListFunction" runat="server" DataKeyField="Function_ID" OnPreRender="dataListFunction_PreRender"
                        RepeatColumns="1" OnItemDataBound="dataListFunction_ItemDataBound">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:HiddenField ID="hiddenFun" runat="server" Value='<%# Eval("Function_ID") %>' />
                            &nbsp;&nbsp;<asp:CheckBox ID="CheckBoxItem" runat="server" OnCheckedChanged="CheckBoxFunction_CheckedChanged"
                                AutoPostBack="True" Checked='<%# Eval("CheckedFlage") %>' />
                            <asp:Label ID="Permissions_NameLabel" runat="server" Text='<%# Eval("Permission_Name") %>' />——
                            <%# Eval("Category")%>
                            <asp:Repeater ID="repeaterThird" runat="server">
                                <ItemTemplate>
                                    <div>
                                        <asp:HiddenField ID="hiddenFunThird" runat="server" Value='<%# Eval("Function_ID") %>' />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="CheckBoxItemThird" runat="server"
                                            Checked='<%# Eval("CheckedFlage") %>' />
                                        <asp:Label ID="Permissions_NameLabe2" runat="server" Text='<%# Eval("Permission_Name") %>' />——
                                        <%# Eval("Category")%>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:DataList>
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
                        <asp:LinkButton ID="lbtnCancel" runat="server" OnClick="btnCancel_Click">取消</asp:LinkButton>
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
