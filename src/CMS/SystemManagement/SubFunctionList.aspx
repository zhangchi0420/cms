<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SubFunctionList.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.SubFunctionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #subFunctions
        {
            width: 100%;
        }
        
        #subFunctions > li
        {
            float: left;
            width: 45%;
            font-size: 12px;
            height:100px;
            margin-left :20px;            
        } 
        
        #subFunctions > li a
        {            
            font-size: 18px;                     
        }       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ul id="subFunctions">
        <asp:Repeater ID="repeater" runat="server">
            <ItemTemplate>
                <li>
                    <p>
                        <a href='<%# Eval("PageURL") %>'>
                            <%# Eval("Permission_Name")%></a>
                    </p>                    
                    <p>
                        <%# Eval("Function_Comment")%>
                    </p>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</asp:Content>
