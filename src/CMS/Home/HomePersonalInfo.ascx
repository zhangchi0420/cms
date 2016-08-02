<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomePersonalInfo.ascx.cs"
    Inherits="Drision.Framework.Web.Home.HomePersonalInfo" %>
<div class="left_top">
    <div class="info">
        <div class="avatar">
            <img alt="" src="../images/icon-user.png" width="80" height="80">
        </div>
        <a href="javascript:void(0)"><span>0</span> 条信息</a>
    </div>
    <ul class="links">
        <li class="name">
            <%= this.UserName %></li>
        <li id="IsShowInfo"><a href="../HR_Common/UserEdit.aspx?id=<%= this.LoginUserID %>" />个人信息修改</a></li>
        <li><a href="../Home/ChangePwd.aspx">登录密码修改</a></li>
    </ul>
    <div class="cl">
    </div>
</div>
<div class="role">
    <p>
        <%= this.DepartmentName %></p>
    <%= this.RoleName %>
</div>
<div id="IsShowQuery" class="sidebar_content">
    <div class="sidebar_nav">
        <ul class="menu collapsible">
            <li><a>查询</a></li>
            <li><a href="../SystemManagement/ScheduleManagement.aspx">计划任务</a> </li>
            <li><a href="../SystemManagement/StartProcessQuery.aspx">我的启动流程</a></li>
            <li><a href="../SystemManagement/ParticipateProcessQuery.aspx">我的参与流程</a></li>
            <li><a href="../SystemManagement/ProcessInstanceQuery.aspx">流程实例查询</a></li>
            <li><a href="../SystemManagement/WorkItem.aspx">我的工作项</a> </li>
        </ul>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
       var IsAdmin =<% =CanAdmin%>;
       if(IsAdmin!=1)
       {
       $("#IsShowInfo").hide();
       $("#IsShowQuery").hide();
       }
    })
</script>
