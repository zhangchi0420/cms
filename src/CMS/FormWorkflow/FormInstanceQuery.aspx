<%@ Page Title="表单实例查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormInstanceQuery.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormInstanceQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            表单实例查询</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>表单 </span><span class="left_star">&nbsp;</span>
                <asp:ComboControl ID="ccForm" runat="server" DropdownType="DropdownList" ShowEmptyItem="true"
                    AutoSelectFirst="true" DataTextField="FormName" DataValueField="FormId">
                </asp:ComboControl>
            </div>
            <div class="item_box_col1">
                <span>表单编号 </span><span class="left_star">&nbsp;</span>
                <asp:TextControl ID="txtCode" runat="server" MaxLength="100" />
            </div>
            <div class="item_box_col1">
                <span>状态 </span><span class="left_star">&nbsp;</span>
                <asp:ComboControl ID="ccState" runat="server" DropdownType="DropdownList" ShowEmptyItem="true"
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
    </div>
    <div class="grid_title" id="divViewControlTitle">
        表单实例信息</div>
    <div id="divviewcontrol">
        <asp:GridControl ID="gcFormInstance" runat="server" OnPageIndexChanging="gcFormInstance_PageIndexChanging"
            OnHeaderClick="gcFormInstance_HeaderClick">
            <Columns>
                <asp:HeaderSortField HeaderText="表单名称" DataTextField="FormName">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="标题" DataTextField="FormTitle">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="编号" DataTextField="FormCode">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="创建时间" DataTextField="CreateTime">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="状态" DataTextField="FormInstanceState" Parameter="State">
                </asp:HeaderSortField>
                <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" Text="编辑" Enabled='<%# Eval("CanEdit") %>' PostBackUrl='<%#"~/FormWorkflow/FormInstanceEdit.aspx?id=" + Eval("FormInstanceId") %>'></asp:LinkButton>
                        <asp:LinkButton ID="btnDetail" runat="server" Text="详情" PostBackUrl='<%#"~/FormWorkflow/FormInstanceDetail.aspx?id=" + Eval("FormInstanceId") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
            </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
