<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Drision.Framework.Web.WebForm1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat = "server" ID = "ScriptManager1"></asp:ScriptManager>
    <div>
    <asp:HiddenControl ID="hc" runat="server" UsingDefaultJquery="true" />
        <asp:Button ID="btnSave" OnClick="btnSave_Click" OnClientClick="alert('sadfjfiedee苦别协膛2大规模大规模大规模2土38sd9f3!');return false;" runat="server" Text="保存" />
        <asp:TextBox ID="txt" runat="server" Text="AA"></asp:TextBox>
        <asp:Label ID="lbl" runat="server" Text="<%# txt.Text %>"></asp:Label>
    </div>
    <div id="divUC" runat="server">
    </div>
    <div>多文件上传</div>
    <sp:MultiUploader ID="mu" runat="server" />
    </form>    
</body>
<script src="js/Common.js" type="text/javascript"></script>
</html>
