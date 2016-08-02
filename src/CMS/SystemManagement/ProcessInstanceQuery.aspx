<%@ Page Title="流程实例查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProcessInstanceQuery.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ProcessInstanceQuery" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3>查询条件</h3>
    <div class="allcol2">
        <div class="item_box_col1">
            <span>流程 </span>
                <asp:ComboControl ID = "cbProcess" ClientInstanceName = "cbProcess"   runat="server"  AutoSelectFirst="true" DropdownType = "DropdownList"  ShowEmptyItem = "true">
                </asp:ComboControl>
        </div>
        <div class="item_box_col1">
            <span>流程名称 </span>
            <asp:TextControl ID="tbT_ProcessName" runat="server" CssClass="item_input" TextType="String" FieldName="ProcessName" />
        </div>
        <div class="item_box_col1">
            <span>启动时间从 </span>
            <asp:DateTimeControl ID = "dtStartTime1" ClientInstanceName = "dtStartTime1"  runat="server" FieldName = "StartTime"  />
        </div>
        <div class="item_box_col1">
            <span>到 </span>
            <asp:DateTimeControl ID = "dtStartTime2" ClientInstanceName = "dtStartTime2"  runat="server" FieldName = "StartTime"  />
        </div>
        <div class="item_box_col1">
            <span>完成时间从</span>
            <asp:DateTimeControl ID = "dtEndTime1" ClientInstanceName = "dtEndTime1"  runat="server" FieldName = "EndTime"  />
        </div>
        <div class="item_box_col1">
            <span>到 </span>
            <asp:DateTimeControl ID = "dtEndTime2" ClientInstanceName = "dtEndTime2"  runat="server" FieldName = "EndTime"  />
        </div>
        <div class="item_box_col1">
            <span>发起人 </span>
                <asp:ComboControl ID = "cbStartUser" ClientInstanceName = "cbStartUser"   runat="server" FieldName = "User_Name" AutoSelectFirst="true" DropdownType = "DropdownList"  ShowEmptyItem = "true">
                </asp:ComboControl>
        </div>
        <div class="item_box_col1">
            <span>状态 </span>
                <asp:ComboControl ID = "cbStatus" ClientInstanceName = "cbStatus"   runat="server" FieldName = "InstanceStatus" AutoSelectFirst="true" DropdownType = "DropdownList"  ShowEmptyItem = "true">
                <Items>
                <asp:ComboItem Value="0" Text="运行中"  />
                <asp:ComboItem Value="1" Text="挂起中"  />
                <asp:ComboItem Value="10" Text="已完成"  />
                <asp:ComboItem Value="20" Text="已取消"  />
                </Items>
                </asp:ComboControl>
        </div>
        <div class="item_box_col1">
            <span>对象 </span>
            <asp:TextControl ID="tbT_OwnerItemName" runat="server" CssClass="item_input" TextType="String" FieldName="OwnerItemName" />
        </div>
        <div class="cl">
        </div>
    </div>
    </div>
    <div class="button_bar">
    <asp:LinkButton ID="lbtnQuery" runat="server" OnClick="btnQuery_Click">查询</asp:LinkButton>
        <asp:LinkButton ID="lbtnClearCondition" runat="server" OnClick="btnClearCondition_Click">重置</asp:LinkButton>
        <%--<asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查询" />
        <asp:Button ID="btnClearCondition" runat="server" OnClick="btnClearCondition_Click" Text="重置" />--%>
    </div>
    <div class="grid_title" id="divViewControlTitle">
        流程信息</div>
    <div id="divviewcontrol" runat="server">
        <asp:GridControl OnPageIndexChanging="grid_PageChanging" ID="gcProcessInstance" ClientInstanceName="gcProcessInstance" runat="server" AutoGenerateColumns="false" OnRowDataBound = "gcProcessInstance_RowDataBound" KeyFieldName="EntityId">
        <Columns>
            <asp:HeaderSortField ShowHeader="false" DataTextField="ProcessName" HeaderText="流程名称" > </asp:HeaderSortField>
            <%--<asp:HeaderSortField ShowHeader="false" DataTextField="DisplayText" HeaderText="流程实体" > </asp:HeaderSortField>
            --%><asp:HeaderSortField ShowHeader="false" DataTextField="OwnerItemName" HeaderText="对象" DisplayLength="20" HorizontalAlign="Left"></asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="User_Name" HeaderText="发起人" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="Department_Name" HeaderText="发起部门" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="StartTime" HeaderText="启动时间" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="EndTime" HeaderText="完成时间" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="InstanceStatus" HeaderText="状态" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="ApproveResult" HeaderText="审核结果" > </asp:HeaderSortField>
            <asp:TemplateField HeaderText="操作" ShowHeader="False">
            <ItemTemplate>
                 <asp:LinkButton ID="lbtnDetail" runat="server" Text="查看" CommandArgument='<%#Eval("ProcessInstanceId")%>' PostBackUrl = '<%#"~/SystemManagement/ProcessInstanceDetail.aspx?id=" + Eval("ProcessInstanceId")%>'></asp:LinkButton>
                 <asp:LinkButton ID="lbtnObjDetail" runat="server" Text="对象详情" OnClick="lbtnObjDetail_Click" CommandArgument='<%#Eval("EntityId")+","+Eval("ObjectId")%>'></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings ShowPager="true" PageIndex="0" PageSize="20">
        </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
