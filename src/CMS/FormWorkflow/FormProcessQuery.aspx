<%@ Page Title="表单流程查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormProcessQuery.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormProcessQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            表单流程查询</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>流程名称 </span><span class="left_star">&nbsp;</span>
                <asp:TextControl ID="txtProcessName" runat="server" MaxLength="100" />
            </div>
            <div class="item_box_col1">
                <span>表单名称 </span><span class="left_star">&nbsp;</span>
                <asp:TextControl ID="txtFormName" runat="server" MaxLength="100" />
            </div>
            <div class="item_box_col1">
                <span>流程状态 </span><span class="left_star">&nbsp;</span>
                <asp:ComboControl ID="ddlProcessState" runat="server" DropdownType="DropdownList"
                    AutoSelectFirst="true">
                </asp:ComboControl>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查询" />
        <asp:LinkButton ID="btnClearCondition" runat="server" OnClick="btnClearCondition_Click"
            Text="重置" />
        <asp:LinkButton ID="btnAdd" runat="server" Text="新增" PostBackUrl="~/FormWorkflow/FormProcessDesigner_FirstStep.aspx"></asp:LinkButton>
    </div>
    <div class="grid_title" id="divViewControlTitle">
        表单流程信息</div>
    <div id="divviewcontrol">
        <asp:GridControl ID="gcProcess" runat="server" OnPageIndexChanging="gcProcess_PageIndexChanging"
            OnHeaderClick="gcProcess_HeaderClick">
            <Columns>
                <asp:HeaderSortField HeaderText="流程名称" DataTextField="ProcessName">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="表单名称" DataTextField="FormName">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="流程实体名称" DataTextField="ProcessEntity" Parameter="EntityId">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="流程状态" DataTextField="State" Parameter="StateValue">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="流程版本" DataTextField="ProcessVersion">
                </asp:HeaderSortField>
                <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDesign" runat="server" Text="设计" PostBackUrl='<%#"~/FormWorkflow/FormProcessDesigner_FirstStep.aspx?id=" + Eval("ProcessId")%>'></asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" Text="删除" Enabled='<%# Eval("CanDelete") %>' CommandArgument='<%# Eval("ProcessId") %>' OnClientClick="return confirm('确认操作？',function(){ShowWaiting(false);})" OnClick="btnDelete_Click"></asp:LinkButton>
                        <asp:LinkButton ID="btnStop" runat="server" Text="停用" Enabled='<%# Eval("CanStop") %>' CommandArgument='<%# Eval("ProcessId") %>' OnClientClick="return confirm('确认操作？',function(){ShowWaiting(false);})" OnClick="btnStop_Click"></asp:LinkButton>
                        <asp:LinkButton ID="btnStart" runat="server" Text="启用" Enabled='<%# Eval("CanStart") %>' CommandArgument='<%# Eval("ProcessId") %>' OnClientClick="return confirm('确认操作？',function(){ShowWaiting(false);})" OnClick="btnStart_Click"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
            </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
