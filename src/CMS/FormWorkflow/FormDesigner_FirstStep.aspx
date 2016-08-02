<%@ Page Title="表单设计第一步（基础信息）" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormDesigner_FirstStep.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormDesigner_FirstStep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header_form">
        <p class="form_box">
            表单设计<span></span>
        </p>
        <div class="form_info">
            <p>
                说明</p>
            <div class="form_information">
                <div>
                    1、如果【实体类型】选择“自定义新增”，【关联实体】请输入中文名称，【关联数据表名】会根据中文拼音翻译</div>
                <div>
                    2、【表单标题】、【表单说明】会在表单解析时作为默认的信息显示</div>
            </div>
        </div>
        <div class="out_border">
            <div class="main_box">
                <h3>
                    表单基础信息</h3>
                <div class="allcol2">
                    <div class="item_box_full">
                        <span>表单名称 </span><span class="left_star">*</span>
                        <asp:TextControl ID="txtFormName" runat="server" MaxLength="100" IsRequired="true" />
                    </div>
                    <div class="item_box_col1">
                        <span>实体类型 </span><span class="left_star">&nbsp;</span>
                        <asp:ComboControl ID="ddlEntityType" DropdownType="DropdownList" runat="server" AutoSelectFirst="true"
                            AutoPostBack="true" OnTextChanged="ddlEntityType_TextChanged">
                            <Items>
                                <asp:ComboItem Text="元数据预置" Value="0" />
                                <asp:ComboItem Text="自定义已存在" Value="1" />
                                <asp:ComboItem Text="自定义新增" Value="2" />
                            </Items>
                        </asp:ComboControl>
                    </div>
                    <div class="item_box_col1">
                        <span>关联实体 </span><span class="left_star">*</span>
                        <asp:ComboControl ID="ddlSysEntity" runat="server" IsRequired="true" DropdownType="DropdownList"
                            ShowEmptyItem="true" AutoSelectFirst="true" AutoPostBack="true" OnTextChanged="ddlSysEntity_TextChanged">
                        </asp:ComboControl>
                        <asp:ComboControl ID="ddlCustomEntity" IsRequired="true" runat="server" DropdownType="DropdownList"
                            ShowEmptyItem="true" AutoSelectFirst="true" AutoPostBack="true" OnTextChanged="ddlCustomEntity_TextChanged">
                        </asp:ComboControl>
                        <asp:TextControl ID="txtSysEntity" runat="server" MaxLength="8" IsRequired="true"
                            ValidateFunction="IsCN" ValidateMessage="输入长度必须小于8且不能带特殊字符" PlaceHolder="请输入实体名称"
                            ClientInstanceName="txtSysEntity" />
                    </div>
                    <div class="item_box_col1">
                        <span>创建人 </span><span class="left_star">&nbsp;</span>
                        <div class="item_display">
                            <asp:Label ID="lblCreateUser" runat="server"></asp:Label></div>
                    </div>
                    <div class="item_box_col1">
                        <span>关联数据表名 </span><span class="left_star">*</span>
                        <asp:TextControl ID="txtSysEntityName" runat="server" ClientInstanceName="txtSysEntityName"
                            ReadOnly="true" IsRequired="true"></asp:TextControl>
                    </div>
                    <div class="cl">
                    </div>
                </div>
            </div>
        </div>
        <div class="out_border">
            <div class="main_box">
                <h3>
                    表单实例信息</h3>
                <div class="allcol2">
                    <div class="item_box_full">
                        <span>表单标题 </span><span class="left_star">&nbsp;</span>
                        <asp:TextControl ID="txtFormTitle" runat="server" MaxLength="256" />
                    </div>
                    <div class="item_box_full">
                        <span>表单说明 </span><span class="left_star">&nbsp;</span>
                        <asp:TextControl ID="txtFormDescription" runat="server" MaxLength="4000" TextType="Text"
                            Height="100px" />
                    </div>
                    <div class="cl">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="btnNextSetp" runat="server" Text="下一步" OnClick="btnNextStep_Click"
            OnClientClick="return ShowWaiting(true);"></asp:LinkButton>
        <asp:LinkButton ID="btnReturn" runat="server" Text="返回" PostBackUrl="~/FormWorkflow/FormQuery.aspx"></asp:LinkButton>
    </div>
    <script type="text/javascript">
        function IsCN(str) {
            var reg = /^[a-z0-9A-Z\u4E00-\u9FA5]+$/;
            if (!reg.test(str)) {
                return false;
            }
            return true;
        }

        Sys.Application.add_load(function () {
            if (window.txtSysEntity != undefined) {
                $(txtSysEntity._container).keyup(function () {
                    if (bValidator.validate(true, $(this)) == true) {
                        var str = txtSysEntity.getValue();
                        if (str != null && str != "") {
                            $.post("ConvertPinYin.ashx", { Value: str }, function (data) {
                                txtSysEntityName.setValue("T_" + data);
                            });
                        }
                    }
                    else {
                        txtSysEntityName.setValue(null);
                    }
                });
            }
        });
    </script>
</asp:Content>
