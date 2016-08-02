<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PluginView.aspx.cs" Inherits="Drision.Framework.Web.PluginView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="font-size:12pt">
            <asp:Repeater ID="Repeater1" runat="server" 
                onitemdatabound="Repeater1_ItemDataBound" >
                <ItemTemplate>
                    <tr>
                        <td style="border:1px solid blue">
                            <ul style="list-style:none">
                                <li style="display:inline-block; width:300px" >
                                    <asp:Label ID="labelPageName" runat="server" />
                                </li>
                                <li style="display:inline-block">
                                    <asp:Label ID="labelTypeName" runat="server" />
                                </li>
                            </ul>
                            <ul style="margin-left:30px">
                                <asp:Repeater ID="Repeater2" runat="server" onitemdatabound="Repeater2_ItemDataBound">
                                    <ItemTemplate>
                                        <li style="width: 300px; display:inline-block">
                                            <asp:Label ID="labelButtonName" runat="server" />
                                        </li>
                                        <li style="width: 100px; display:inline-block">
                                            <asp:Label ID="labelInvokeType" runat="server" />
                                        </li>
                                        <li style="width: 500px; display:inline-block">
                                            <asp:Label ID="labelMethod" runat="server" />
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
