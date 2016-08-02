<%@ Page Title="表单流程实例详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormProcessInstanceDetail.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormProcessInstanceDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
        <h3>
            基本信息</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>流程名称</span>
                <div class="item_display">
                    <asp:Label ID="lblProcessName" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>表单名称</span>
                <div class="item_display">
                    <asp:Label ID="lblFormName" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>发起人</span>
                <div class="item_display">
                    <asp:Label ID="lblStartUser" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>启动时间</span>
                <div class="item_display">
                    <asp:Label ID="lblStartTime" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>完成时间</span>
                <div class="item_display">
                    <asp:Label ID="lblEndTime" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>发起部门</span>
                <div class="item_display">
                    <asp:Label ID="lblStartDept" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>状态</span>
                <div class="item_display">
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                </div>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="main_box" id="divHistory" runat="server">
        <h3>
            历史审核记录</h3>
        <div class="check_info check_activity">
            <asp:Repeater ID="rpActivityInstance" runat="server" OnItemDataBound="rpActivityInstance_ItemDataBound">
                <ItemTemplate>
                    <ul onclick="$(this).next().toggle(500)">
                        <li>
                            <div>
                                <label>
                                    活动名称：</label>
                                <asp:Label ID="ActivityName" runat="server" Text='<%# Eval("ActivityName") %>'></asp:Label>
                                <label>
                                    审核结果：</label>
                                <asp:Label ID="ApproveResult" runat="server" Text='<%# Eval("ApproveResult") %>'></asp:Label>
                            </div>
                        </li>
                    </ul>
                    <div class="check_info_son check_approve_group">
                        <asp:Repeater ID="rpApproveGroup" runat="server" OnItemDataBound="rpApproveGroup_ItemDataBound">
                            <ItemTemplate>
                                <ul>
                                    <li>
                                        <div>
                                            <label>
                                                参与人：</label>
                                            <asp:Label ID="ParticipantName" runat="server" Text='<%# Eval("ParticipantName") %>'></asp:Label>
                                        </div>
                                        <div>
                                            <label>
                                                组审核结果：</label>
                                            <asp:Label ID="ApproveResult" runat="server" Text='<%# Eval("ApproveResult") %>'></asp:Label>
                                        </div>
                                    </li>
                                </ul>
                                <div class="check_info_son check_approve_detail">
                                    <asp:Repeater ID="rpApproveActivity" runat="server">
                                        <ItemTemplate>
                                            <ul>
                                                <li>
                                                    <div>
                                                        <label>
                                                            审核人：</label>
                                                        <asp:Label ID="User_Name" runat="server" Text='<%# Eval("User_Name") %>'></asp:Label>
                                                        <asp:Label ID="ProxyUser" runat="server" Text='<%# Eval("ProxyUser") %>'></asp:Label>
                                                        <asp:Label ID="AddingUser" runat="server" Text='<%# Eval("AddingUser") %>'></asp:Label>
                                                    </div>
                                                    <div>
                                                        <label>
                                                            时间：</label>
                                                        <asp:Label ID="ApproveDate" runat="server" Text='<%# Eval("ApproveDate") %>'></asp:Label>
                                                    </div>
                                                    <div>
                                                        <label>
                                                            审核结果：</label>
                                                        <asp:Label ID="ApproveResult" runat="server" Text='<%# Eval("ApproveResult") %>'></asp:Label>
                                                    </div>
                                                    <div>
                                                        <label>
                                                            审核意见：</label>
                                                        <asp:Label ID="ApproveComment" runat="server" Text='<%# Eval("ApproveComment") %>'></asp:Label>
                                                    </div>
                                                </li>
                                            </ul>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="main_box" style="display:none">
        <h3>
            流程图追踪</h3>
        <div id="silverlightControlHost" style="text-align: center;">
            <object data="data:application/x-silverlight-2," type="application/x-silverlight-2"
                width="700px" height="700px">
                <param name="source" value="../ClientBin/Drision.Framework.WorkflowDesigner.xap" />
                <param name="minRuntimeVersion" value="4.0.60310.0" />
                <param name="autoUpgrade" value="true" />
                <param name="windowless" value="true"/>
                <param name="InitParams" value="Background=<%= TracerBackground %>,Type=Tracer,ProcessInstanceId=<%= ProcessInstanceId %>" />
                <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.60310.0" style="text-decoration: none">
                    <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="获取 Microsoft Silverlight"
                        style="border-style: none" />
                </a>
            </object>
            <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;
                border: 0px"></iframe>
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="lbtnReturn" runat="server" UseSubmitBehavior="false" OnClick="btnReturn_Click">返回</asp:LinkButton>
    </div>
    <script type="text/javascript">
        Sys.Application.add_load(function () {
            $(".check_approve_detail").each(function (i, p) {
                if ($(p).children("ul").length == 0) {
                    $(p).prev().andSelf().remove();
                }
            });
        });
    </script>
</asp:Content>
