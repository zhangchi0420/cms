<%@ Page Title="任务与提醒" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleManagement.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ScheduleManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box" style="display:none">
    <h3>工作项管理</h3>
    <div class="allcol2">
        <div class="item_box_col1">
            <span>工作项类型 </span>
            <asp:ComboControl ID="ccWorkItemType" runat="server" ShowEmptyItem="true" AutoSelectFirst="true"></asp:ComboControl>
        </div>
        <div class="cl">
        </div>
    </div>
    </div>
    <div class="button_bar" style="display:none">
    <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="btnAdd_Click">添加工作项</asp:LinkButton>
        <%--<asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="添加工作项" />--%>        
    </div>
    <br />
    <br />    
    <asp:ScheduleControl ID="sc" runat="server" ClientInstanceName="sc" ></asp:ScheduleControl>
    <script type="text/javascript">
        Sys.Application.add_load(function () {
            sc.set_onViewChanged(function (s, e) {
                var h = $(".ScheduleControl .fc-view-agendaWeek > div").height();
                if (h >= 780) {
                    $(".ScheduleControl .fc-view-agendaWeek > table .fc-sun > div").height(h);
                }

                h = $(".ScheduleControl .fc-view-agendaDay > div").height() + $(".ScheduleControl .fc-view-agendaDay > div > .fc-agenda-divider").next().height();
                if (h >= 780) {
                    $(".ScheduleControl .fc-view-agendaDay > table .fc-col0 > div").height(h);
                }
            });
        });
    </script>
</asp:Content>
