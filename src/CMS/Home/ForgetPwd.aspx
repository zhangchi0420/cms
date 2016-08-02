<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPwd.aspx.cs" Inherits="Drision.Framework.Web.Home.ForgetPwd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>忘记密码</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID = "ScriptManager1" runat = "server"></asp:ScriptManager>
        <asp:UpdatePanel ID = "UpdatePanel1" runat = "server">
        <ContentTemplate>
            <div>
                <span>用户名:</span>
                <asp:TextControl ClientInstanceName="txtUserCode" ID="txtUserCode" runat="server" Width="200px" TextType="String" MaxLength="50" IsRequired="true" ValidateMessage="请输入用户名" />
            </div>
            <div>
                <span>邮 箱:</span>
                <asp:TextControl ClientInstanceName="txtEMail" ID="txtEMail" runat="server" MaxLength="50" Width="200px" TextType="String" IsRequired="true" ValidateMessage="请输入邮箱" />
            </div>
            <div>
                <span class="button_blue">
                    <asp:Button ID="btnOk" Text="确定" OnClick="btnOk_Click" runat="server" OnClientClick="return bValidator.validate();" /></span>
                    <asp:Button ID ="btnReturn" UseSubmitBehavior = "false" Text = "返回" PostBackUrl = "~/Home/Login.aspx" runat = "server" />
            </div>
        </ContentTemplate></asp:UpdatePanel>
    </form>
</body>
</html>
