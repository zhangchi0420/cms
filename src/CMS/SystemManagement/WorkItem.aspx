<%@ Page Title="待办事项" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="WorkItem.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.WorkItem" %>

<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--    <h3>我的工作项</h3>
    <div class="allcol2">
        <div class="item_box_col1">
            <span>流程名称 </span>
            <asp:TextControl ID="tbT_ProcessName" runat="server" CssClass="item_input" TextType="String" FieldName="ProcessName" />
        </div>
        <div class="cl">
        </div>
    </div>--%>
    <%--    <div class="button_bar">
        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查询" />
        <asp:Button ID="btnClearCondition" runat="server" OnClick="btnClearCondition_Click" Text="重置" />
    </div>--%>
    <br />
    <div class="grid_title" id="divViewControlTitle">
        工作项</div>
    <div id="divviewcontrol" runat="server">
        <asp:GridControl ID="gcWorkItem" ClientInstanceName="gcWorkItem" runat="server" AutoGenerateColumns="false"
            OnPageIndexChanging="gcWorkItem_PageIndexChanging" >
            <Columns>
                <%--<asp:HeaderSortField DataTextField="ProcessName" ShowHeader="false" HeaderText="流程名称"
                    DisplayLength="10">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="OwnerItemName" ShowHeader="false" HeaderText="对象"
                    DisplayLength="10">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="ActivityName" ShowHeader="false" HeaderText="活动名称"
                    DisplayLength="10">
                </asp:HeaderSortField>--%>
                <asp:HeaderSortField HorizontalAlign="Left" DataTextField="Title" ShowHeader="false" HeaderText="主题" DisplayLength="30"
                    >
                </asp:HeaderSortField>
                <asp:HeaderSortField HorizontalAlign="Left" DataTextField="User_Name" ShowHeader="false" HeaderText="提交人"
                    >
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="CreateTime" ShowHeader="false" HeaderText="创建时间"
                    DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                </asp:HeaderSortField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnDetail" runat="server" Text="查看" CommandArgument='<%#Eval("CompletePageUrl")%>'
                            OnClick="lbtnDetail_Click"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings ShowPager="true" PageSize="25">
            </PagerSettings>
        </asp:GridControl>
    </div>
    <div class="grid_title" id="div1">
        其它任务</div>
    <div id="div2" runat="server">
        <asp:GridControl ID="gcSchedule" OnPageIndexChanging="gcSchedule_PageIndexChanging" HorizontalAlign="Left" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:HeaderSortField DataTextField="Title" ShowHeader="false" HeaderText="标题" DisplayLength="30">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="CreateTime" ShowHeader="false" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"
                    HeaderText="开始时间" DisplayLength="10">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="DeadLine" ShowHeader="false" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"
                    HeaderText="截止时间" DisplayLength="10">
                </asp:HeaderSortField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='<%# Eval("ProcessPageUrl") %>'>详细</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings ShowPager="true" PageSize="25">
            </PagerSettings>
        </asp:GridControl>
    </div>
    <div class="grid_title" id="div3" style="display:none">
        提醒</div>
    <div id="div4" runat="server" style="display:none">
        <asp:GridControl ID="gcRemind" OnPageIndexChanging="gcRemind_PageIndexChanging" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:HeaderSortField HorizontalAlign="Left" ShowHeader="false" DataTextField="RemindName" DisplayLength="30"
                    HeaderText="标题">
                </asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="CreateUserName" HeaderText="提醒人">
                </asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"
                    HeaderText="创建时间">
                </asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="DeadLine" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"
                    HeaderText="截止时间">
                </asp:HeaderSortField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='<%# Eval("RemindURL") %>'>详细</a>
                        <asp:LinkButton ID="btnRead" runat="server" CommandArgument='<%# Eval("RemindId") %>' OnClientClick="return confirm('是否确认已经完成？')" OnClick="btnRead_Click">已读</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings ShowPager="true" PageSize="25">
            </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
