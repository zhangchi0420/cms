<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPagePlugin.aspx.cs" Inherits="Drision.Framework.Web.TestPagePlugin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <p>
    <asp:Label ID="Label1" runat="server" />
        
    </p>
    <p>
    <asp:TextBox ID="TextBox1" runat="server" />
    </p>
    <p>
    <asp:Button ID="Button1" runat="server" Text="Click" />
    </p>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
