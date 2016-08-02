<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Drision.Framework.Web.WebForm2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="Link1" runat="server" href="~/class/apple/jquery-ui-1.8.16.custom.css"
        rel="stylesheet" type="text/css" />
    <link id="Link2" runat="server" href="~/class/base.css" rel="stylesheet" type="text/css" />
    <link id="Link3" runat="server" href="~/class/common.css" rel="stylesheet" type="text/css" />
    <link href="~/template_apple/template.css" rel="stylesheet" type="text/css" id="linkTemplate"
        runat="server" />
    <link href="~/class/page.css" rel="stylesheet" type="text/css" id="linkFrame" runat="server" />
    <script src="js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.12.custom.min.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/Common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" style="text-align: center;" runat="server">
    <asp:ScriptManager ID="sm" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div style="text-align: left">
                <div>
                    Client alert
                    <input type="button" value="Alert" onclick="alert('这是测试的消息');" />
                </div>
                <div>
                    Server: MessageBox.Alert
                    <asp:Button ID="btn" runat="server" Text="Alert" OnClick="btn_Click" />
                </div>
                <div>
                    Client: confirm
                    <input type="button" value="Confirm" onclick="confirm('确认删除吗？',function(){alert('已删除！');})" />
                </div>
                <div>
                    asp:Button return confirm(""); + MessageBox.Alert
                    <asp:Button ID="btnConfirm" runat="server" Text="ConfirmAndPostback" OnClientClick="return confirm('确认删除吗？');"
                        OnClick="btnConfirm_Click" />
                </div>
                <div>
                    asp:LinkButton return confirm(""); + MessageBox.Alert
                    <asp:LinkButton ID="btnConfirmLink" runat="server" Text="ConfirmAndPostback" OnClientClick="return confirm('确认删除吗？');"
                        OnClick="btnConfirm_Click"></asp:LinkButton>
                </div>
                <div>
                    Server: MessageBox.Confirm
                    <asp:Button ID="btnConfirmServer" runat="server" Text="ConfirmServer" OnClick="btnConfirmServer_Click" />
                </div>
                <div>多文件上传</div>
    <sp:MultiUploader ID="mu" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>    
</body>
</html>
