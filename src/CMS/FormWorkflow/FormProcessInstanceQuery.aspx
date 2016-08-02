<%@ Page Title="表单流程实例查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormProcessInstanceQuery.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormProcessInstanceQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            查询条件</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>表单流程 </span>
                <asp:ComboControl ID="cbProcess" ClientInstanceName="cbProcess" runat="server" AutoSelectFirst="true"
                    DropdownType="DropdownList" ShowEmptyItem="true">
                </asp:ComboControl>
            </div>
            <div class="item_box_col1">
                <span>表单编号 </span>
                <asp:TextControl ID="txtFormCode" runat="server" TextType="String"
                     MaxLength="100" />
            </div>
            <div class="item_box_col1">
                <span>启动时间从 </span>
                <asp:DateTimeControl ID="dtStartTime1" ClientInstanceName="dtStartTime1" runat="server"
                    FieldName="StartTime" />
            </div>
            <div class="item_box_col1">
                <span>到 </span>
                <asp:DateTimeControl ID="dtStartTime2" ClientInstanceName="dtStartTime2" runat="server"
                    FieldName="StartTime" />
            </div>
            <div class="item_box_col1">
                <span>完成时间从</span>
                <asp:DateTimeControl ID="dtEndTime1" ClientInstanceName="dtEndTime1" runat="server"
                    FieldName="EndTime" />
            </div>
            <div class="item_box_col1">
                <span>到 </span>
                <asp:DateTimeControl ID="dtEndTime2" ClientInstanceName="dtEndTime2" runat="server"
                    FieldName="EndTime" />
            </div>
            <div class="item_box_col1">
                <span>发起人 </span>
                <asp:ComboControl ID="cbStartUser" ClientInstanceName="cbStartUser" runat="server"
                    FieldName="User_Name" AutoSelectFirst="true" DropdownType="DropdownList" ShowEmptyItem="true">
                </asp:ComboControl>
            </div>
            <div class="item_box_col1">
                <span>状态 </span>
                <asp:ComboControl ID="cbStatus" ClientInstanceName="cbStatus" runat="server" FieldName="InstanceStatus"
                    AutoSelectFirst="true" DropdownType="DropdownList" ShowEmptyItem="true">
                    <Items>
                        <asp:ComboItem Value="0" Text="运行中" />
                        <asp:ComboItem Value="1" Text="挂起中" />
                        <asp:ComboItem Value="10" Text="已完成" />
                        <asp:ComboItem Value="20" Text="已取消" />
                    </Items>
                </asp:ComboControl>
            </div>            
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="lbtnQuery" runat="server" OnClick="btnQuery_Click">查询</asp:LinkButton>
        <asp:LinkButton ID="lbtnClearCondition" runat="server" OnClick="btnClearCondition_Click">重置</asp:LinkButton>        
    </div>
    <div class="grid_title" id="divViewControlTitle">
        流程信息</div>
    <div id="divviewcontrol" runat="server">
        <asp:GridControl OnHeaderClick="grid_HeaderClick" OnPageIndexChanging="grid_PageChanging" ID="gcProcessInstance" ClientInstanceName="gcProcessInstance" runat="server" AutoGenerateColumns="false" OnRowDataBound = "gcProcessInstance_RowDataBound" KeyFieldName="EntityId">
        <Columns>
            <asp:HeaderSortField DataTextField="ProcessName" HeaderText="流程名称" DisplayLength="20" > </asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="FormName" HeaderText="表单名称" DisplayLength="20"></asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="FormCode" HeaderText="表单编号"></asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="User_Name" HeaderText="发起人" > </asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="Department_Name" HeaderText="发起部门" > </asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="StartTime" HeaderText="启动时间" > </asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="EndTime" HeaderText="完成时间" > </asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="InstanceStatus" HeaderText="状态" > </asp:HeaderSortField>
            <asp:HeaderSortField DataTextField="ApproveResult" HeaderText="审核结果" > </asp:HeaderSortField>
            <asp:TemplateField HeaderText="操作" ShowHeader="False" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                 <asp:LinkButton ID="lbtnDetail" runat="server" Text="流程详情" PostBackUrl = '<%#"~/FormWorkflow/FormProcessInstanceDetail.aspx?id=" + Eval("ProcessInstanceId")%>'></asp:LinkButton>
                 <asp:LinkButton ID="lbtnFormInstanceDetail" runat="server" Text="表单详情"  PostBackUrl = '<%#"~/FormWorkflow/FormInstanceDetail.aspx?id=" + Eval("FormInstanceId")%>'></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings ShowPager="true" PageIndex="0" PageSize="20">
        </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
