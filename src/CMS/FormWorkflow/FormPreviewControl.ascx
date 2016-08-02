<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormPreviewControl.ascx.cs"
    Inherits="Drision.Framework.Web.FormWorkflow.FormPreviewControl" %>
<style type="text/css">
    .form
    {
        width: 700px;
        border: 1px solid Gray;
        margin: 0px auto;
    }
    .formHeader
    {
        border-bottom: 1px solid Gray;
    }
    .formTitle
    {
        font-size: 25px;
        text-align: center;
        padding: 10px 0 10px 0;
    }
    .formDate
    {
        text-align: left;
        margin-right: 10px;
        float: right;
    }
    .formCode
    {
        text-align: left;
        margin-left: 10px;
    }
    .formFieldSection
    {
        border-bottom: 1px solid Gray;
    }
    .formField
    {
        float: left;
        width: 100%;
        margin-top: 5px;
        margin-bottom: 5px;
    }
    .formFieldDisplayText
    {
        width: 200px;
        text-align: right;
        float: left;
        line-height: 22px;
    }
    .formFieldValue
    {
        float: left;
        line-height: 22px;
        white-space: normal;
        word-break: break-all;
        width:300px;
        border-bottom:solid 1px #CCCCCC;
        min-height:22px;  
    }
    .formFieldValue span
    {        
        line-height: 22px;
    }
    
    .formContainer .item_input
    {
        float: left;
        width: 300px;
    }
    .formContainer .left_star
    {
        float: left;
        line-height: 22px;
    }
    .formDescription
    {
        width: 650px;
        float: left;
        padding-left: 5px;
        min-height: 100px;
        border-left: 1px solid Gray;
        word-break: break-all;
    }
    .formManagement
    {
        border-bottom: 1px solid Gray;
    }
    .formLeft
    {
        width: 30px;
        float: left;
        padding-left: 5px;
    }
    .formAppoveHistory
    {
        min-height: 100px;
        border-left: 1px solid Gray;
        width: 663px;
        float: left;
    }
    .formProcess
    {
        margin-left: 5px;        
    }
    .formAI
    {
        margin-left: 15px;
        background-color:#FDFFE8;
        margin-bottom: 0px;
        border-top: solid 1px #666666;
    }    
    .formAIGroup
    {
        /*margin-left:15px;*/
    }
    .formApproveUser
    {
        margin-left: 15px;
        word-break: break-all;
        border: solid 1px #D0D0D0;
        margin: 1px 0px 1px 15px;
        background-color: #D0F4FF;
    }
    .formAppoveHistory label
    {
        font-weight:bold;
    }
</style>
<div class="formContainer">
    <div class="form">
        <div class="formHeader">
            <div class="formTitle">
                <asp:Label ID="lblFormTitle" runat="server"></asp:Label></div>
            <div class="formDate">
                日期：
                <asp:Label ID="lblFormDate" runat="server"></asp:Label></div>
            <div class="formCode">
                编号：
                <asp:Label ID="lblFormCode" runat="server"></asp:Label></div>
        </div>
        <asp:Panel ID="pFormContent" runat="server" CssClass="formContent">
        </asp:Panel>
        <div class="formManagement">
            <div class="formLeft">
                说明
            </div>
            <div class="formDescription">
                <asp:Label ID="lblFormDescription" runat="server"></asp:Label></div>
            <div class="cl">
            </div>
        </div>
        <div class="formWorkflow">
            <div class="formLeft">
                审核记录
            </div>
            <div class="formAppoveHistory">
                <asp:Repeater ID="rProcess" runat="server" OnItemDataBound="rProcess_ItemDataBound">
                    <ItemTemplate>
                        <div class="formProcess">
                            <div>
                                <label>
                                    流程：</label>
                                <asp:Label ID="lblProcess" runat="server"></asp:Label>
                                <label>
                                    提交时间：</label>
                                <asp:Label ID="lblStartTime" runat="server"></asp:Label>
                                <label>
                                    结果：</label>
                                <asp:Label ID="lblApproveResult" runat="server"></asp:Label>
                            </div>
                            <asp:Repeater ID="rActivity" runat="server" OnItemDataBound="rActivity_ItemDataBound">
                                <ItemTemplate>
                                    <div class="formAI">
                                        <div>
                                            <label>
                                                活动：</label>
                                            <asp:Label ID="ActivityName" runat="server" Text='<%# Eval("ActivityName") %>'></asp:Label>
                                            <label>
                                                结果：</label>
                                            <asp:Label ID="ApproveResult" runat="server" Text='<%# Eval("ApproveResult") %>'></asp:Label>
                                        </div>
                                        <asp:Repeater ID="rApproveGroup" runat="server" OnItemDataBound="rApproveGroup_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="formAIGroup">
                                                    <%--<label>
                                                    参与人：</label>
                                                <asp:Label ID="ParticipantName" runat="server" Text='<%# Eval("ParticipantName") %>'></asp:Label>
                                                <label>
                                                    组审核结果：</label>
                                                <asp:Label ID="ApproveResult" runat="server" Text='<%# Eval("ApproveResult") %>'></asp:Label>--%>
                                                    <asp:Repeater ID="rUser" runat="server">
                                                        <ItemTemplate>
                                                            <div class="formApproveUser">
                                                                <label>
                                                                    审核人：</label>
                                                                <asp:Label ID="User_Name" runat="server" Text='<%# Eval("User_Name") %>'></asp:Label>
                                                                <asp:Label ID="ProxyUser" runat="server" Text='<%# Eval("ProxyUser") %>'></asp:Label>
                                                                <asp:Label ID="AddingUser" runat="server" Text='<%# Eval("AddingUser") %>'></asp:Label>
                                                                <label>
                                                                    时间：</label>
                                                                <asp:Label ID="ApproveDate" runat="server" Text='<%# Eval("ApproveDate") %>'></asp:Label>
                                                                <label>
                                                                    结果：</label>
                                                                <asp:Label ID="ApproveResult" runat="server" Text='<%# Eval("ApproveResult") %>'></asp:Label>
                                                                <label>
                                                                    意见：</label>
                                                                <asp:Label ID="ApproveComment" runat="server" Text='<%# Eval("ApproveComment") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
</div>
