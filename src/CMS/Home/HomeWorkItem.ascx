<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeWorkItem.ascx.cs"
    Inherits="Drision.Framework.Web.Home.HomeWorkItem" %>
<div class="main_box handle_box" style="width: 505px; min-height: 280px;">
    <h3>
        事项办理<span></span></h3>
    <div class="metro_accordion" id="js_accordion">
        <ul>
            <li class="metro_accordion_title">我的工作项<span><%= this.WorkItemCount %></span></li>
            <asp:Repeater ID="rWorkItem" runat="server">
                <ItemTemplate>
                    <li><a href='<%# Eval("WorkItemURL") %>'>
                        <%# Eval("DisplayText")%></a><span><%# Eval("CreateTime")%></span></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
        <ul class="collapsed">
            <li class="metro_accordion_title">站内信<span><%= this.RemindCount %></span></li>
            <asp:Repeater ID="rRemind" runat="server">
                <ItemTemplate>
                    <li><a href='<%# Eval("Url") %>'>
                        <%# Eval("Subject")%></a><span><%# Eval("ReceiveTime")%></span></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
    <div class="metro_more">
        <span><a href="../SystemManagement/WorkItem.aspx">更多 ></a></span></div>
</div>
