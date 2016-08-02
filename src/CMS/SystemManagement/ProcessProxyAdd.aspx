<%@ Page Title="流程代理设置" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProcessProxyAdd.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ProcessProxyAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var alian = '<%=System.Configuration.ConfigurationManager.AppSettings["PageFieldRedStarAlign"].ToString() %>';
            $(".left_star").addClass(alian + "_star");
            $(".right_star").removeClass("left_star");
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
    <h3>
        基础信息</h3>
    <div class="allcol2">
        <div class="item_box_col1">
            <span>负责人</span><span class="left_star">*</span>
            <asp:SelectControl ID="scOwner" ClientInstanceName="scOwner" runat="server" FieldName="OwnerId" IsRequired = "true">
                <PopupSettings ScrollBars="None" Height="600" Width="850" Title="请选择" />
                <IFrameSettings ContentUrl="../HR_Common/UserSelect.aspx" ReturnControl="PopUpValueText" />
            </asp:SelectControl>
        </div>
        <div class="item_box_col1">
            <span>负责人</span><span class="left_star">*</span>
            <asp:SelectControl ID="scProxy" ClientInstanceName="scProxy" runat="server" FieldName="ProxyId" IsRequired = "true">
                <PopupSettings ScrollBars="None" Height="600" Width="850" Title="请选择" />
                <IFrameSettings ContentUrl="../HR_Common/UserSelect.aspx" ReturnControl="PopUpValueText" />
            </asp:SelectControl>
        </div>
        <div class="item_box_col1">
            <span>起始时间</span><span class="left_star">*</span>
            <asp:DateTimeControl ID="dtcStartTime" ClientInstanceName="dtcStartTime" runat="server" IsRequired = "true"
                FieldName="StartTime" />
        </div>
        <div class="item_box_col1">
            <span>结束时间</span><span class="left_star">*</span>
            <asp:DateTimeControl ID="dtcEndTime" ClientInstanceName="dtcEndTime" runat="server" IsRequired = "true"
                FieldName="EndTime" />
        </div>
        <div class="cl">
        </div>
    </div>
    <div class = "cl"></div>
    </div>

    <div class="button_bar">
    <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnPreRender="btnSave_PreRender">保存</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server">取消</asp:LinkButton>
        <%--<asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click"
            OnPreRender="btnSave_PreRender" />
            <asp:Button ID="btnCancel" runat="server" Text="取消" />--%>
    </div>
</asp:Content>
