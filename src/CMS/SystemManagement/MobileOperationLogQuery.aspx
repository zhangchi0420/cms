<%@ Page Title="手机端操作错误日志" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MobileOperationLogQuery.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.MobileOperationLogQuery" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
    <div class="grid_title" id="divViewControlTitle">
        日志信息</div>
    <div id="divviewcontrol" runat="server">
        <asp:GridControl OnPageIndexChanging="grid_PageChanging" ID="gcProcessInstance" ClientInstanceName="gcProcessInstance" runat="server" AutoGenerateColumns="false" OnRowDataBound = "gcProcessInstance_RowDataBound" KeyFieldName="Id">
        <Columns>
        <asp:TemplateField HeaderText="操作记录" ShowHeader="False" HeaderStyle-Width="15%">
        <ItemTemplate>
                 <asp:LinkButton ID="btnView"  runat="server"  Text='<%#Eval("OperationRecord") %>'  PostBackUrl = '<%#"~/SystemManagement/MobileOperationRecordDetail.aspx?id=" + Eval("Id")%>'></asp:LinkButton>
        </ItemTemplate>
        </asp:TemplateField>
            <%--<asp:HeaderSortField ShowHeader="false" DataTextField="OperationRecord" HeaderText="操作记录" HeaderStyle-Width="40%" HorizontalAlign="Left" > </asp:HeaderSortField>--%>
            <asp:HeaderSortField ShowHeader="false" DataTextField="CreateUserId" HeaderText="执行人" DisplayLength="10"  HeaderStyle-Width="30px"></asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="CreateTime" HeaderText="执行时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"  HeaderStyle-Width="20%" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="State" HeaderText="状态"  HeaderStyle-Width="30px"> </asp:HeaderSortField>
            <asp:TemplateField HeaderText="操作" ShowHeader="False" HeaderStyle-Width="15%">
            <ItemTemplate>
                 <asp:LinkButton ID="btnCanExec"  runat="server" Text="可再执行" CommandArgument='<%#Eval("Id") %>'  OnClientClick="if (!confirm('确定要改为可再执行？')) {return false;}"  OnClick="btnCanExec_Click"></asp:LinkButton>
                 <asp:LinkButton ID="btnComplete"  runat="server" Text="完成" CommandArgument='<%#Eval("Id") %>'  OnClientClick="if (!confirm('确定要改为完成？')) {return false;}"  OnClick="btnComplete_Click"></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
        </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
