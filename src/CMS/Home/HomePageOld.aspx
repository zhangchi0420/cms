<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
 CodeBehind="HomePageOld.aspx.cs" Inherits="Drision.Framework.Web.Home.HomePageOld" Title="首页" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Redirect(url) {
            if (url != null && url != "") {
                window.location.href = url;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid_title" id="divViewControlTitle">提醒信息</div>
    <div id="divviewcontrol" runat="server">
        <asp:GridControl OnPageIndexChanging="grid_PageChanging" ID="grid" runat="server">
            <Columns>
                <asp:HeaderSortField ShowHeader="false" DataTextField="RemindName" DataValueField="RemindURL" OnClientClick="return Redirect($(this).attr('datavalue'));" ItemType="Link" DisplayLength="30" HeaderText="主题"></asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="OwnerUserName" HeaderText="负责人"></asp:HeaderSortField>                
                <asp:HeaderSortField ShowHeader="false" DataTextField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="创建时间"></asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="DeadLine" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="截止时间"></asp:HeaderSortField>                
            </Columns>
            <PagerSettings PageSize="20"></PagerSettings>
        </asp:GridControl>
    </div>
    <div class = "cl"></div>
<div class="button_bar">
    <asp:Button ID="btnWorkItem" runat="server" Text="查看工作项" PostBackUrl="~/SystemManagement/WorkItem.aspx" />
</div>    
</asp:Content>
