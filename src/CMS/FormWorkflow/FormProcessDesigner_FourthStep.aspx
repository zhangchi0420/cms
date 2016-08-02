<%@ Page Title="流程设计第四步（预览发布）" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormProcessDesigner_FourthStep.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormProcessDesigner_FourthStep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            流程详情</h3>
        <div class="allcol2">
            <div class="item_box_full">
                <span>流程名称 </span><span class="left_star">&nbsp;</span>
                <div class="item_display"><asp:Label ID="lblProcessName" runat="server"></asp:Label></div>
            </div>
            <div class="item_box_full">
                <span>关联表单 </span><span class="left_star">&nbsp;</span>
                <div class="item_display"><asp:Label ID="lblFormName" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>流程实体 </span><span class="left_star">&nbsp;</span>
                <div class="item_display"><asp:Label ID="lblProcessEntity" runat="server"></asp:Label>
                </div>
            </div>
            <div class="item_box_col1">
                <span>流程版本 </span><span class="left_star">&nbsp;</span>
                <div class="item_display"><asp:Label ID="lblProcessVersion" runat="server"></asp:Label>
                </div>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="grid_title" id="divViewControlTitle">
        活动信息</div>
    <div>
        <asp:GridControl ID="gcActivity" runat="server">
            <Columns>
                <asp:HeaderSortField DataTextField="ActivityName" ShowHeader="false" HeaderText="活动名称">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="ActivityType" ShowHeader="false" HeaderText="活动类型">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="ExecuteType" ShowHeader="false" HeaderText="执行方式">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="ActivityParticipant" ShowHeader="false" HeaderText="活动参与人">
                </asp:HeaderSortField>
            </Columns>
            <PagerSettings ShowPager="false"></PagerSettings>
        </asp:GridControl>
    </div>
    <asp:PopupControl ID="pcPreview" runat="server" ClientInstanceName="pcPreview" Title="流程示意图，可调整活动位置后保存" Width="720" Height="810">
        <PopupContent ID="pcPreviewContent" runat="server">
            <div id="silverlightControlHost" style="text-align: center;">
            <object data="data:application/x-silverlight-2," type="application/x-silverlight-2"
                width="700px" height="700px">
                <param name="source" value="../ClientBin/Drision.Framework.WorkflowDesigner.xap" />
                <param name="minRuntimeVersion" value="4.0.60310.0" />
                <param name="autoUpgrade" value="true" />
                <param name="InitParams" value="Background=E8E8E8,Type=ProcessShape,ProcessId=<%= ProcessId %>" />
                <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.60310.0" style="text-decoration: none">
                    <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="获取 Microsoft Silverlight"
                        style="border-style: none" />
                </a>
            </object>
            <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;
                border: 0px"></iframe>
        </div>
        </PopupContent>
        <Buttons>
            <asp:PopupButton Text="保存" />
        </Buttons>
    </asp:PopupControl>    
    <div class="grid_title" id="div1">
        流程验证错误信息</div>
    <div>
        <asp:GridControl ID="gcError" runat="server" EmptyDataText="流程验证正确，没有错误">
            <Columns>
                <asp:HeaderSortField ItemStyle-ForeColor="Red" HorizontalAlign="Left" DataTextField="Message" ShowHeader="false" HeaderText="错误描述">
                </asp:HeaderSortField>                
            </Columns>
            <PagerSettings ShowPager="false"></PagerSettings>
        </asp:GridControl>
    </div>
    <div class="button_bar">        
        <asp:LinkButton ID="btnPrev" runat="server" Text="上一步" OnClick="btnPrev_Click"></asp:LinkButton>
        <a onclick="pcPreview.open();" style="display:none">流程示意图</a>
        <asp:LinkButton ID="btnPublish" runat="server" Text="发布" OnClick="btnPublish_Click"></asp:LinkButton>
    </div>
</asp:Content>
