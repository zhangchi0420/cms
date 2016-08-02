<%@ Page Title="流程设计第二步（活动配置）" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormProcessDesigner_SecondStep.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormProcessDesigner_SecondStep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .containerTable
        {
            width: 100%;
            border-collapse: collapse;
            min-height: 500px;
            margin-bottom:20px;
        }
        .activityTd
        {
            width: 20%;
            vertical-align: top;
            border-right: 1px solid Gray;
        }
        .propertyTd
        {
            width: 80%;
            vertical-align: top;
            padding: 5px 20px 5px 20px;
        }
        .activity
        {
            width: 90%;
            margin: 10px 5% 10px 5%;
            text-align: center;
            vertical-align: middle;
            font-size: 20px;
            height: 80px;
            line-height: 80px;
            overflow: hidden;
        }
        .selected
        {
            border: 1px solid red;
        }
        .unselected
        {
            border: 1px dotted gray;
        }
    </style>
    <style type="text/css">
        .tableParticipant
        {
            width: 100%;
            border-collapse: collapse;
        }
        
        .tableParticipant > tbody > tr > td
        {
            padding: 10px 0 10px 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="containerTable">
        <tr>
            <td class="activityTd">
                <asp:Repeater ID="rActivity" runat="server">
                    <ItemTemplate>
                        <div onclick="SelectActivity(<%# Eval("ActivityId") %>);" class="activity <%# Eval("SelectClass") %>">
                            <%# Eval("ActivityName") %></div>
                    </ItemTemplate>
                </asp:Repeater>
            </td>
            <td class="propertyTd">
                <div class="main_box">
                    <h3>
                        活动基础信息</h3>
                    <div class="allcol2">
                        <div class="item_box_full">
                            <span>流程名称 </span><span class="left_star">&nbsp;</span>
                            <div class="item_display"><asp:Label ID="lblProcessName" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="item_box_full">
                            <span>流程实体 </span><span class="left_star">&nbsp;</span>
                            <div class="item_display"><asp:Label ID="lblProcessEntity" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="item_box_col1">
                            <span>活动名称 </span><span class="left_star">*</span>
                            <asp:TextControl ID="txtActivityName" runat="server" MaxLength="100" IsRequired="true" />
                        </div>
                        <div class="cl">
                        </div>
                    </div>
                </div>
                <div class="main_box">
                    <h3>
                        活动跳转配置</h3>
                    <div class="allcol2">
                        <div class="item_box_col1">
                            <span>驳回跳转方式 </span><span class="left_star">*</span>
                            <asp:ComboControl ID="ccRejectType" runat="server" ShowEmptyItem="true" AutoSelectFirst="true"
                                DropdownType="DropdownList" IsRequired="true" AutoPostBack="true" OnTextChanged="ccRejectType_TextChanged">
                            </asp:ComboControl>
                        </div>
                        <div id="divRejectTargetActivity" runat="server" class="item_box_col1">
                            <span>目标活动 </span><span class="left_star">*</span>
                            <asp:ComboControl ID="ccRejectTargetActivity" runat="server" DataTextField="ActivityName"
                                DataValueField="ActivityId" AutoSelectFirst="true" DropdownType="DropdownList"
                                IsRequired="true">
                            </asp:ComboControl>
                        </div>
                        <div class="cl">
                        </div>
                    </div>
                </div>
                <div class="grid_button">
                    <asp:LinkButton ID="btnAddParticipant" runat="server" Text="添加参与人" OnClientClick="pcAP.open();return false;"></asp:LinkButton>
                </div>
                <div class="grid_title" id="divViewControlTitle">
                    活动参与人列表</div>
                <div id="divviewcontrol">
                    <asp:GridControl ID="gcAP" runat="server">
                        <Columns>
                            <asp:HeaderSortField HeaderText="参与人名称" DataTextField="ParticipantName" ShowHeader="false">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField HeaderText="通过方式" DataTextField="PassType" ShowHeader="false">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField HeaderText="最小通过率" DataTextField="MinPassRatio" ShowHeader="false">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField HeaderText="最小通过数" DataTextField="MinPassNum" ShowHeader="false">
                            </asp:HeaderSortField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDeleteParticipant" runat="server" Text="删除" OnClientClick="return confirm('确认操作？',function(){ShowWaiting(false);});"
                                        CommandArgument='<%# Eval("ActivityParticipantId") %>' OnClick="btnDeleteParticipant_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings ShowPager="false">
                        </PagerSettings>
                    </asp:GridControl>
                </div>
                <div class="button_bar">
                    <asp:LinkButton ID="btnDelete" runat="server" Text="删除活动" OnClientClick="return confirm('确认操作？',function(){ ShowWaiting(false);});"
                        OnClick="btnDelete_Click"></asp:LinkButton>
                    <asp:LinkButton ID="btnSave" runat="server" Text="保存活动" OnClientClick="return ShowWaiting(true);"
                        OnClick="btnSave_Click"></asp:LinkButton>
                    <asp:LinkButton ID="btnAdd" runat="server" Text="新增活动" OnClientClick="return confirm('确认操作？',function(){ ShowWaiting(false);});"
                        OnClick="btnAdd_Click"></asp:LinkButton>
                    <asp:LinkButton ID="btnPrev" runat="server" Text="上一步" OnClick="btnPrev_Click"></asp:LinkButton>
                    <asp:LinkButton ID="btnNext" runat="server" Text="下一步" OnClick="btnNext_Click"></asp:LinkButton>
                </div>
            </td>
        </tr>
    </table>
    <asp:PopupControl ID="pcAP" runat="server" ClientInstanceName="pcAP" Title="添加活动参与人"
        Width="320" Height="300">
        <PopupContent ID="pcAPContent" runat="server">
            <table class="tableParticipant">
                <tr>
                    <td>
                        流程参与人
                    </td>
                    <td>
                        <asp:ComboControl ID="ccParticipant" runat="server" ClientInstanceName="ccParticipant"
                            DropdownType="DropdownList" IsRequired="true" ShowEmptyItem="true" AutoSelectFirst="true"
                            DataTextField="ParticipantName" DataValueField="ParticipantId">
                        </asp:ComboControl>
                    </td>
                </tr>
                <tr>
                    <td>
                        通过方式
                    </td>
                    <td>
                        <asp:ComboControl ID="ccPassType" runat="server" ClientInstanceName="ccPassType"
                            DropdownType="DropdownList" IsRequired="true" ShowEmptyItem="true" AutoSelectFirst="true">
                        </asp:ComboControl>
                    </td>
                </tr>
                <tr>
                    <td>
                        最小通过数
                    </td>
                    <td>
                        <asp:TextControl ID="txtMinPassNum" runat="server" ClientInstanceName="txtMinPassNum"
                            TextType="Int16" MinValue="1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        最小通过率
                    </td>
                    <td>
                        <asp:TextControl ID="txtMinPassRatio" runat="server" ClientInstanceName="txtMinPassRatio"
                            TextType="Decimal" MinValue="0" MaxValue="100" />
                    </td>
                </tr>
            </table>
        </PopupContent>
        <Buttons>
            <asp:PopupButton Text="确定" OnClick="ParticipantSave" />
            <asp:PopupButton Text="取消" OnClick="function(s,e){pcAP.close();}" />
        </Buttons>
    </asp:PopupControl>
    <asp:CallBackControl ID="cbc" runat="server" ClientInstanceName="cbc" CallBackType="JSON"
        OnCallBack="cbc_CallBack" />
    <asp:HiddenControl ID="hc" runat="server" ClientInstanceName="hc" />
    <asp:HiddenControl ID="hcSave" runat="server" ClientInstanceName="hcSave" />
    <script type="text/javascript">
        function ParticipantSave() {
            if (bValidator.validate(false, $(pcAP._container)) == true) {
                var parameter = {
                    ActivityId: hc.getValue(),
                    ParticipantId: ccParticipant.getValue(),
                    PassType: ccPassType.getValue(),
                    MinPassNum: txtMinPassNum.getValue(),
                    MinPassRatio: txtMinPassRatio.getValue()
                }
                cbc.callBack(parameter, function (data) {
                    var value = $.parseJSON(data);
                    if (value.IsError == true) {
                        alert(value.Error);
                    }
                    else {
                        pcAP.close();
                        eval(hcSave.getText());
                    }
                });
            }
        }

        function SelectActivity(id) {
            ShowWaiting(false);
            hc.setValue(id);
            eval(hc.getText());
        }
    </script>
</asp:Content>
