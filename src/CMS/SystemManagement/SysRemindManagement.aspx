<%@ Page Title="站内信管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SysRemindManagement.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.SysRemindManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:TabControl ID="tc" runat="server">
        <Tabs>
            <asp:TabItem ID="tiInBox" runat="server" TabText="收件箱">
                <div class="button_bar">
                    <a href="../SystemManagement/SysRemindAdd.aspx">新增</a>
                </div>
                <div>
                    <asp:GridControl ID="gcInBox" runat="server" ClientInstanceName="gcInBox" OnPageIndexChanging="gcInBox_PageIndexChanging">
                        <Columns>
                            <asp:FullCheckField DataValueField="RemindId" DataTextField="RemindName">
                            </asp:FullCheckField>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="RemindName" DisplayLength="20"
                                HeaderText="主题">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="CreateUser" HeaderText="发件人">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="CreateTime" HeaderText="发件时间">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="StateText" DataValueField="State"
                                HeaderText="状态">
                            </asp:HeaderSortField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <center>
                                        <a href='../SystemManagement/SysRemindDetail.aspx?id=<%# Eval("RemindId") %>'>详情</a>
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings ShowPager="true" PageSize="25">
                            <PagerButtons>
                                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="删除"></asp:LinkButton>
                                <asp:LinkButton ID="btnIsRead" runat="server" OnClick="btnIsRead_Click" Text="已读"></asp:LinkButton>
                                <asp:LinkButton ID="btnNotRead" runat="server" OnClick="btnNotRead_Click" Text="未读"></asp:LinkButton>
                            </PagerButtons>
                        </PagerSettings>
                    </asp:GridControl>
                </div>
            </asp:TabItem>
            <asp:TabItem ID="tiOutBox" runat="server" TabText="已发送">
                <div>
                    <asp:GridControl ID="gcOutBox" runat="server" ClientInstanceName="gcOutBox" OnPageIndexChanging="gcOutBox_PageIndexChanging">
                        <Columns>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="RemindName" DisplayLength="20"
                                HeaderText="主题">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="Owner" HeaderText="收件人">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="CreateTime" HeaderText="发件时间">
                            </asp:HeaderSortField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <center>
                                        <a href='../SystemManagement/SysRemindDetail.aspx?id=<%# Eval("RemindId") %>'>详情</a>
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings ShowPager="true" PageSize="25">
                        </PagerSettings>
                    </asp:GridControl>
                </div>
            </asp:TabItem>
            <asp:TabItem ID="tiDelete" runat="server" TabText="已删除">
                <div>
                    <asp:GridControl ID="gcIsDelete" runat="server" ClientInstanceName="gcIsDelete" OnPageIndexChanging="gcIsDelete_PageIndexChanging">
                        <Columns>
                            <asp:FullCheckField DataValueField="RemindId" DataTextField="RemindName">
                            </asp:FullCheckField>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="RemindName" DisplayLength="20"
                                HeaderText="主题">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="CreateUser" HeaderText="发件人">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" DataTextField="CreateTime" HeaderText="发件时间">
                            </asp:HeaderSortField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <center>
                                        <a href='../SystemManagement/SysRemindDetail.aspx?id=<%# Eval("RemindId") %>'>详情</a>
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings ShowPager="true" PageSize="25">
                            <PagerButtons>
                                <asp:LinkButton ID="btnTrueDelete" runat="server" OnClick="btnTrueDelete_Click" Text="彻底删除"></asp:LinkButton>
                            </PagerButtons>
                        </PagerSettings>
                    </asp:GridControl>
                </div>
            </asp:TabItem>
        </Tabs>
    </asp:TabControl>
    <script type="text/javascript">
        Sys.Application.add_load(function () {
            $(gcInBox._container).find("tr > td > span[datavalue=0]").each(function (i, p) {
                $(this).parent().parent().css("font-weight", "bold");
            });            
        });   
    </script>
</asp:Content>
