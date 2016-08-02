<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeNotice.ascx.cs"
    Inherits="Drision.Framework.Web.Home.HomeNotice" %>
<div class="main_box notice_box" style="width: 260px; height: 280px;">
    <h3>
        公告通知<span></span></h3>
    <div class="metro_list">
        <ul>
            <asp:Repeater ID="rNotice" runat="server">
                <ItemTemplate>
                    <li><a href='<%# Eval("NoticeURL") %>'>
                        <%# Eval("NoticeName") %></a><%# Eval("Is_Viewed") %><span><%# Eval("PublishTime")%></span></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
    <div class="metro_more">
        <span><a href="../CallCenter_Common/T_CC_NoticeAndNote_Query.aspx">更多公告通知查看 ></a></span></div>
</div>
