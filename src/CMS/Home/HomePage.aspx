<%@ Page Title="首页" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="HomePage.aspx.cs" Inherits="Drision.Framework.Web.Home.HomePage" %>

<%@ Register Src="HomePersonalInfo.ascx" TagName="HomePersonalInfo" TagPrefix="uc1" %>
<%@ Register Src="HomeWorkItem.ascx" TagName="HomeWorkItem" TagPrefix="uc2" %>
<%@ Register Src="HomeNotice.ascx" TagName="HomeNotice" TagPrefix="uc3" %>
<%@ Register Src="HomeFastAccess.ascx" TagName="HomeFastAccess" TagPrefix="uc4" %>
<%@ Register Src="HomeAddressBook.ascx" TagName="HomeAddressBook" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../class/superhome.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jsaccordion.js"></script>
    <script type="text/javascript" src="../js/shift.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            initAccordion();
            $(".main > h2").hide();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <div class="left_info">
        <uc1:HomePersonalInfo ID="HomePersonalInfo1" runat="server" />
        <div class="cl">
        </div>
    </div>
    <div class="metroui">
        <h2>
            用户中心</h2>
        <uc2:HomeWorkItem ID="HomeWorkItem1" runat="server" />
        <uc3:HomeNotice ID="HomeNotice1" runat="server" />
        <uc4:HomeFastAccess ID="HomeFastAccess1" runat="server" />
        <uc5:HomeAddressBook ID="HomeAddressBook1" runat="server" />        
    </div>
</asp:Content>
