<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeFastAccess.ascx.cs"
    Inherits="Drision.Framework.Web.Home.HomeFastAccess" %>
<div class="main_box quick_box" style="width: 505px; min-height: 250px;">
    <h3>
        快捷操作<span></span></h3>
    <ul class="shortcuts tab-content" style="display: block;">
        <li>
            <div class="shortcut-icon">
                <img alt="" src="../images/quickicon/pencil.png" />
                <div class="divider">
                </div>
            </div>
            <a class="shortcut-description" href="../SystemManagement/ProjectWorkItem.aspx">项目审核<span id="projectNum" runat="server"></span></a>
        </li>    
       <%--  <li>
            <div class="shortcut-icon">
                <img alt="" src="../images/quickicon/quickicon10.png" />
                <div class="divider">
                </div>
            </div>
            <a class="shortcut-description" href="../Trade_Project/T_Company_Query.aspx">企业</a>
        </li>   
         <li>
            <div class="shortcut-icon">
                <img alt="" src="../images/quickicon/quickicon7.png" />
                <div class="divider">
                </div>
            </div>
            <a class="shortcut-description" href="../Trade_Project/T_TradeResult_Query.aspx">竞价结果</a>
        </li>   --%>
    </ul>
    <div class="metro_more" style="display:none">
        <span><a href="javascript:;">配置快捷操作 ></a></span></div>
</div>
