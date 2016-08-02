<%@ Page Title="表单查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormQuery.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            表单查询</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>表单名称 </span><span class="left_star">&nbsp;</span>
                <asp:TextControl ID="txtFormName" runat="server" MaxLength="100" />
            </div>
            <div class="item_box_col1">
                <span>表单状态 </span><span class="left_star">&nbsp;</span>
                <asp:ComboControl ID="ddlFormState" runat="server" DropdownType="DropdownList" ShowEmptyItem="true" AutoSelectFirst="true">
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
        <asp:LinkButton ID="btnAdd" runat="server" Text="新增" PostBackUrl="~/FormWorkflow/FormDesigner_FirstStep.aspx"></asp:LinkButton>
    </div>
    <div class="grid_title" id="divViewControlTitle">
        表单列表</div>
    <div id="divviewcontrol">
        <asp:GridControl ID="gcForm" runat="server" OnPageIndexChanging="gcForm_PageIndexChanging" OnHeaderClick="gcForm_HeaderClick">
            <Columns>
                <asp:HeaderSortField HeaderText="表单名称" DataTextField="FormName">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="关联实体名称" DataTextField="DisplayText" Parameter="EntityId">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="关联实体表名" DataTextField="EntityName" Parameter="EntityId">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="创建时间" DataTextField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="创建人" DataTextField="CreateUser">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="表单状态" DataTextField="FormState" Parameter="State">
                </asp:HeaderSortField>
                <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDesign" runat="server" Text="设计" PostBackUrl='<%#"~/FormWorkflow/FormDesigner_FirstStep.aspx?id=" + Eval("FormId")%>'></asp:LinkButton>
                        <asp:LinkButton ID="btnPreview" runat="server" Text="预览" PostBackUrl='<%#"~/FormWorkflow/FormDesigner_FourthStep.aspx?id=" + Eval("FormId")%>'></asp:LinkButton>
                        <asp:LinkButton ID="btnStop" runat="server" Text="停用" Enabled='<%# Eval("IsStartUsed") %>' OnClientClick="return confirm('确认操作？');" CommandArgument='<%# Eval("FormId") %>' OnClick="btnStop_Click"></asp:LinkButton>
                        <asp:LinkButton ID="btnAdd" runat="server" Text="新增实例" Enabled='<%# Eval("IsStartUsed") %>' PostBackUrl='<%#"~/FormWorkflow/FormInstanceAdd.aspx?id=" + Eval("FormId")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
            </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
