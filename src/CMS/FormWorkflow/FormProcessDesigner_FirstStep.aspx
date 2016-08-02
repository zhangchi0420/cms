<%@ Page Title="流程设计第一步（基础信息）" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormProcessDesigner_FirstStep.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormProcessDesigner_FirstStep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        
        .header_form .grid_button
        {
            margin-bottom: 0px;
            margin-top: -30px;
            line-height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header_form">
        <p class="form_box">
            流程设计<span></span>
        </p>
        <div class="form_info">
            <p>
                说明</p>
            <div class="form_information">
                <div>
                    1、流程参与人：参与审核的角色或人员，供下一步活动设计中选择</div>
            </div>
        </div>
        <div class="out_border">
            <div class="main_box">
                <h3>
                    表单流程信息</h3>
                <div class="allcol2">
                    <div class="item_box_full">
                        <span>流程名称 </span><span class="left_star">*</span>
                        <asp:TextControl ID="txtProcessName" runat="server" MaxLength="100" IsRequired="true" />
                    </div>
                    <div class="item_box_full">
                        <span>关联表单 </span><span class="left_star">*</span>
                        <asp:ComboControl ID="ddlForm" runat="server" DropdownType="DropdownList" DataTextField="FormName"
                            DataValueField="FormId" ShowEmptyItem="true" AutoSelectFirst="true" IsRequired="true">
                        </asp:ComboControl>
                    </div>
                    <div class="cl">
                    </div>
                </div>
            </div>
        </div>
        <div class="out_border">
            <div class="main_box">
                <div class="grid_title">流程参与人列表</div>                    
                <div class="grid_button">
                    <asp:LinkButton ID="btnAdd" runat="server" Text="添加参与人" CssClass="add" OnClientClick="pcParticipant.open();return false;"></asp:LinkButton>
                </div>
                <div id="divviewcontrol">
                    <asp:GridControl ID="gcProcessParticipant" runat="server">
                        <Columns>
                            <asp:HeaderSortField ShowHeader="false" HeaderText="参与人类型" DataTextField="ParticipantFunctionType">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" HeaderText="参与人名称" DataTextField="ParticipantName">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" HeaderText="关联用户" DataTextField="UserName">
                            </asp:HeaderSortField>
                            <asp:HeaderSortField ShowHeader="false" HeaderText="关联角色" DataTextField="RoleName">
                            </asp:HeaderSortField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete" runat="server" Text="删除" OnClientClick="return confirm('确认操作？',function(){ShowWaiting(false);});"
                                        CommandArgument='<%# Eval("ParticipantId") %>' OnClick="btnDelete_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings ShowPager="false">
                        </PagerSettings>
                    </asp:GridControl>
                </div>
            </div>
        </div>
    </div>
    <asp:PopupControl ID="pcParticipant" runat="server" Title="添加流程参与人" ClientInstanceName="pcParticipant"
        Width="320" Height="200">
        <PopupContent ID="pcParticipantContent" runat="server">
            <table class="tableParticipant">
                <tr>
                    <td>
                        参与人类型
                    </td>
                    <td>
                        <asp:ComboControl ID="ddlFunctionType" runat="server" DropdownType="DropdownList"
                            AutoSelectFirst="true" ClientInstanceName="ddlFunctionType" ShowEmptyItem="true"
                            IsRequired="true">
                            <Items>
                                <asp:ComboItem Text="直属主管" Value="9" />
                                <asp:ComboItem Text="具体角色" Value="6" />
                                <asp:ComboItem Text="具体人员" Value="1" />
                            </Items>
                            <ClientSideEvents OnSelectChanged="FunctionTypeChanged" />
                        </asp:ComboControl>
                    </td>
                </tr>
                <tr id="trUser">
                    <td>
                        关联用户
                    </td>
                    <td>
                        <asp:ComboControl ID="ddlUser" runat="server" DropdownType="DropdownList" ShowEmptyItem="true"
                            AutoSelectFirst="true" IsRequired="true" ClientInstanceName="ddlUser" DataTextField="User_Name"
                            DataValueField="User_ID">
                        </asp:ComboControl>
                    </td>
                </tr>
                <tr id="trRole">
                    <td>
                        关联角色
                    </td>
                    <td>
                        <asp:ComboControl ID="ddlRole" runat="server" DropdownType="DropdownList" ShowEmptyItem="true"
                            AutoSelectFirst="true" IsRequired="true" ClientInstanceName="ddlRole" DataTextField="Role_Name"
                            DataValueField="Role_ID">
                        </asp:ComboControl>
                    </td>
                </tr>
            </table>
        </PopupContent>
        <Buttons>
            <asp:PopupButton Text="确定" OnClick="ParticipantSave" />
            <asp:PopupButton Text="取消" OnClick="function(s,e){pcParticipant.close();}" />
        </Buttons>
    </asp:PopupControl>
    <asp:HiddenControl ID="hc" runat="server" ClientInstanceName="hc" />
    <asp:CallBackControl ID="cbc" runat="server" CallBackType="JSON" OnCallBack="cbc_CallBack"
        ClientInstanceName="cbc" />
    <div class="button_bar">
        <asp:LinkButton ID="btnNextSetp" runat="server" Text="下一步" OnClick="btnNextStep_Click"
            OnClientClick="return ShowWaiting(true);"></asp:LinkButton>
        <asp:LinkButton ID="btnReturn" runat="server" Text="返回" PostBackUrl="~/FormWorkflow/FormProcessQuery.aspx"></asp:LinkButton>
    </div>
    <script type="text/javascript">
        function ParticipantSave() {
            if (bValidator.validate(false, $(pcParticipant._container)) == true) {
                var parameter = {
                    FunctionType: ddlFunctionType.getValue(),
                    UserId: ddlUser.getValue(),
                    RoleId: ddlRole.getValue(),
                    ProcessId: hc.getValue()
                };
                cbc.callBack(parameter, function (data) {
                    var value = $.parseJSON(data);
                    if (value.IsError == true) {
                        alert(value.Error);
                    }
                    else {
                        pcParticipant.close();
                        eval(hc.getText());
                    }
                });
            }
        }

        function FunctionTypeChanged(s, e) {
            var trRole = $('#trRole');
            var trUser = $('#trUser');
            if (e.Value == '9') {
                trUser.hide();
                trRole.hide();
            }
            if (e.Value == '6') {
                trUser.hide();
                trRole.show();
            }
            if (e.Value == '1') {
                trUser.show();
                trRole.hide();
            }
            ddlUser.setValue(null);
            ddlRole.setValue(null);
        }

        Sys.Application.add_load(function () {
            $('#trRole').hide();
            $('#trUser').hide();
        });
    </script>
</asp:Content>
