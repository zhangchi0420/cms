<%@ Page Title="表单设计第三步（表单权限配置)" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormDesigner_ThirdStep.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormDesigner_ThirdStep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .section
        {
            width:100%;
            margin: 10px 0px 20px 0px;            
            border-collapse:collapse;
        }
        
        .sectionHeader
        {
            background-color:#F0F0F0;
            font-weight:bold;
        }
        
        .section td
        {
            border:1px solid gray;
        }
        
        .field
        {
            width: 55%;            
            height: 30px;
            text-align: left;
            vertical-align: middle;
            font-size: 15px;            
        }
        .checkbox
        {
            width: 15%;            
            height: 30px;
            text-align:center;
            vertical-align:middle;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            角色</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>角色名称 </span><span class="left_star">*</span>
                <asp:ComboControl ID="ddlRoleList" runat="server" AutoSelectFirst="true" AutoPostBack="true"
                    OnTextChanged="ddlRoleList_SelectedIndexChanged" DataTextField="Role_Name" DataValueField="Role_ID">
                </asp:ComboControl>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <table class="section sectionHeader">
        <tr>
            <td class="field">
                表单字段
            </td>
            <td class="checkbox">
                不可见
            </td>
            <td class="checkbox">
                只读
            </td>
            <td class="checkbox">
                可编辑
            </td>
        </tr>
    </table>
    <asp:Repeater ID="rSection" runat="server">
        <ItemTemplate>
            <table class="section">
                <asp:Repeater ID="rField" runat="server" DataSource='<%# Eval("Fields") %>'>
                    <ItemTemplate>
                        <tr>
                            <td class="field">
                                <asp:HiddenField ID="hf" runat="server" Value='<%# Eval("FormFieldId") %>' />
                                <%# Eval("DisplayText") %>
                            </td>
                            <td class="checkbox">
                                <asp:RadioButton ID="ccInVisible" runat="server" Checked='<%# Eval("Invisible") %>'
                                    GroupName='<%# Eval("FormFieldId") %>' />
                            </td>
                            <td class="checkbox">
                                <asp:RadioButton ID="ccReadOnly" runat="server" Checked='<%# Eval("ReadOnly") %>'
                                    GroupName='<%# Eval("FormFieldId") %>' />
                            </td>
                            <td class="checkbox">
                                <asp:RadioButton ID="ccReadWrite" runat="server" Checked='<%# Eval("ReadWrite") %>'
                                    GroupName='<%# Eval("FormFieldId") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="cl">
                </div>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <div class="button_bar">
        <asp:LinkButton ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click"></asp:LinkButton>
        <asp:LinkButton ID="btnPrev" runat="server" Text="上一步" OnClick="btnPrev_Click"></asp:LinkButton>
        <asp:LinkButton ID="btnNext" runat="server" Text="下一步" OnClick="btnNext_Click"></asp:LinkButton>
    </div>
</asp:Content>
