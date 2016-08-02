<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UserInfo.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.UserInfo"
    Title="用户管理" %>

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
                        用户信息列表</h3>
                </div>
                <div class="Content" style="text-align: center">
                    <div class="tab">
                        <div style="width: 550px; margin-left: 130px; padding: 10px 0; float: left;">
                            <span style="height: 29px; line-height: 29px; display: block; float: left; overflow: hidden;">
                                <asp:Label ID="Label5" runat="server" Text="用户姓名："></asp:Label>
                                <asp:TextBox ID="txtSearchName" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                            </span><span style="height: 29px; line-height: 29px; margin-top: -8px; margin-left: 10px;
                                display: block; float: left;">
                                <asp:Button ID="btnSearch" runat="server" CssClass="chaxun t5" OnClick="btnSearch_Click"
                                    CausesValidation="False" />
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
                                            <asp:BoundField HeaderText="用户ID" HeaderStyle-BackColor="#94B0CE" DataField="User_ID"
                                                Visible="False">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="用户工号" HeaderStyle-BackColor="#94B0CE" DataField="User_Code">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="用户姓名" HeaderStyle-BackColor="#94B0CE" DataField="User_Name">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="所属部门">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDept" runat="server" Text='<%# Bind("Department_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="入职日期" HeaderStyle-BackColor="#94B0CE" DataField="EntryDate"
                                                DataFormatString="{0:yyyy-MM-dd}">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="身份证号" HeaderStyle-BackColor="#94B0CE" DataField="Card_No"
                                                Visible="False">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="手机号码" HeaderStyle-BackColor="#94B0CE" DataField="User_Mobile">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="电子邮件" HeaderStyle-BackColor="#94B0CE" DataField="UserEMail"
                                                Visible="False">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="用户角色">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserRole" runat="server" Text='<%# Bind("User_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="是否停用">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblState" runat="server" Text='<%# Bind("User_Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="备注" HeaderStyle-BackColor="#94B0CE" DataField="User_Comment"
                                                Visible="False">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="密码" HeaderStyle-BackColor="#94B0CE" DataField="User_Password"
                                                Visible="False">
                                                <HeaderStyle CssClass="title1" Wrap="False" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="操作" HeaderStyle-BackColor="#94B0CE">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnReset" runat="server" Font-Underline="true" CssClass="links"
                                                        CausesValidation="False" CommandName="re" OnClientClick="if (!confirm('是否重置密码？')) {return false;}">重置密码</asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnState" runat="server" Font-Underline="true" CssClass="links"
                                                        CausesValidation="False" CommandName="state" OnClientClick="if (!confirm('是否启用用户？')) {return false;}">启用</asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" Font-Underline="true" CssClass="links"
                                                        CausesValidation="False" CommandName="up">编辑</asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnDel" runat="server" Font-Underline="true" CssClass="links"
                                                        CausesValidation="False" CommandName="del" OnClientClick="if (!confirm('是否删除用户？')) {return false;}">删除</asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnSet" runat="server" Font-Underline="true" CssClass="links"
                                                        CausesValidation="False" CommandName="set">设置角色</asp:LinkButton>
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
                <table class="tab">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="友情提醒：重置密码后，统一恢复为默认密码“1”。"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel ID="Panel1" runat="server" Visible="false">
                <div class="com_box">
                    <div class="title">
                        <h3>
                            用户详细信息</h3>
                    </div>
                    <div class="Content" style="text-align: center">
                        <div class="tab">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center">
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        用户工号：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtCode" runat="server" MaxLength="10"></asp:TextBox>
                                        <span style="color: Red">*</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode"
                                            ErrorMessage="用户工号不能为空！" Display="None">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right;">
                                        用户姓名：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtName" runat="server" MaxLength="20"></asp:TextBox>
                                        <span style="color: Red">*</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName"
                                            ErrorMessage="用户姓名不能为空！" Display="None">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        手机号码：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="15"></asp:TextBox>
                                        <span style="color: Red">*</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPhone"
                                            ErrorMessage="手机号码不能为空！" Display="None">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right;">
                                        身份证号：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtCardNum" runat="server" MaxLength="18"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtCardNum"
                                            Display="None" ErrorMessage="身份证号不正确！" ValidationExpression="\d{17}[\d|X]|\d{15}">*</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        入职日期：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtEntryTime" runat="server" MaxLength="10" onclick="SelectDate(this,'yyyy-MM-dd')"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtEntryTime"
                                            ErrorMessage="请确认上岗日期格式为yyyy-mm-dd！" Display="None" ValidationExpression="^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)$">*</asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: right;">
                                        电子邮箱：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtEmail"
                                            Display="None" ErrorMessage="电子邮件不正确！" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        所属部门：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlDept" runat="server" AppendDataBoundItems="True" Width="153px">
                                        </asp:DropDownList>
                                    </td>
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
                                        是否为ME：
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:RadioButtonList ID="rbtnIsME" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1">是</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="text-align: right;">
                                    </td>
                                    <td style="text-align: left;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        备注：
                                    </td>
                                    <td colspan="3" style="text-align: left;">
                                        <asp:TextBox ID="txtRemark" runat="server" Rows="5" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REV" runat="server" ControlToValidate="txtRemark"
                                            ValidationExpression="^(\s|\S){0,100}$" ErrorMessage="备注不能超过100个字！" Display="None">*</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td colspan="3" style="text-align: left;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEdit" runat="server" Visible="False"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td style="text-align: left;">
                                        <table>
                                            <tr>
                                                <td style="width: 15px;">
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
                                    <td colspan="4">
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                            ShowSummary="False" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <table class="tab">
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="友情提醒：WEB端登录名为用户工号，手机端登录名为手机号码。"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
