<%@ Page Title="表单设计第二步（内容设计）" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormDesigner_SecondStep.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormDesigner_SecondStep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .containerTable
        {
            width: 100%;
            border-collapse: collapse;
            min-height: 600px;
        }
        .operationTd
        {
            width: 20%;
            vertical-align: top;
        }
        .operationTd > div
        {
            margin-left: 20px;
        }
        .form
        {
            width: 80%;
            vertical-align: top;
        }
        .section
        {
            width: 96%;
            min-height: 100px;
            margin: 10px 2% 10px 2%;
            float: left;
        }
        .field
        {
            width: 90%;
            height: 30px;
            float: left;
            text-align: left;
            vertical-align: middle;
            margin: 5px 10px 5px 10px;
            background-color: #F3F3F3;
            font-size: 15px;
        }
        .button_bar div
        {
            text-align: center;
            margin: 5px;
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
        .divRelation
        {
            font-size: 18px;
        }
        .divField
        {
            margin-left: 40px;
        }
        .divField span
        {
            vertical-align: middle;
            margin: 0px 2px 0px 2px;
        }
        .tableNewField
        {
            width: 100%;
            border-collapse: collapse;
        }
        .tableNewField > tbody > tr > td
        {
            padding: 10px 0 10px 0;
        }
        .tableFieldProperty
        {
            width: 100%;
            border-collapse: collapse;
        }
        .tableFieldProperty > tbody > tr > td
        {
            padding: 10px 0 10px 0;
        }
    </style>
    <script type="text/javascript">
        function HideOrShow(isNewSection, isSelectField, isNewField, isProperty, isMoveUp, isMoveDown, isDelete) {
            if (isNewSection == 1) $("#btnNewSection").show(); else $("#btnNewSection").hide();
            if (isSelectField == 1) $("#btnSelectField").show(); else $("#btnSelectField").hide();
            if (isNewField == 1 && hc.getValue() == "N") $("#btnNewField").show(); else $("#btnNewField").hide();
            if (isProperty == 1) $("#btnProperty").show(); else $("#btnProperty").hide();
            if (isMoveUp == 1) $("#btnMoveUp").show(); else $("#btnMoveUp").hide();
            if (isMoveDown == 1) $("#btnMoveDown").show(); else $("#btnMoveDown").hide();
            if (isDelete == 1) $("#btnDelete").show(); else $("#btnDelete").hide();

            if (isNewSection + isSelectField + isNewField == 0) $("#divAdd").hide(); else $("#divAdd").show();
            if (isProperty + isMoveUp + isMoveDown + isDelete == 0) $("#divOperation").hide(); else $("#divOperation").show();
        }

        function Select(obj) {
            $(".containerTable .selected").removeClass("selected").addClass("unselected");
            $(obj).removeClass("unselected").addClass("selected");
            hc.setText($(obj).attr("id"));

            if ($(obj).hasClass("form")) {
                HideOrShow(1, 0, 0, 0, 0, 0, 0);
            }
            if ($(obj).hasClass("section")) {
                HideOrShow(0, 1, 1, 0, 1, 1, 1);
            }
            if ($(obj).hasClass("field")) {
                HideOrShow(0, 0, 0, 1, 1, 1, 1);
            }

            //阻止冒泡
            if (event.stopPropagation) {
                event.stopPropagation();
            }
            else if (window.event) {
                window.event.cancelBubble = true;
            }
        }

        function NewFormSection() {
            confirm("确定添加？", function () {
                var type = "NewSection";
                var id = hc.getText();
                cbc.callBack({ Type: type, SelectedId: id, Parameter: null }, function (data) {
                    var value = $.parseJSON(data);
                    if (value.IsError == true) {
                        alert(value.Error);
                    }
                    else {
                        var section = $(value.DesignerContent);
                        Select(section);
                        $(".form").append(section);
                    }
                });
            });
        }

        function NewField() {
            pcNewField.open();
        }

        function NewFieldSave() {
            if (bValidator.validate(false, $(pcNewField._container)) == true) {
                var parameter = {
                    DisplayText: txtNewField.getValue(),
                    FieldName: txtNewFieldName.getValue(),
                    DataType: ccFieldType.getValue()
                };

                var type = "NewField";
                var id = hc.getText();
                var section = $("#" + id);
                cbc.callBack({ Type: type, SelectedId: id, Parameter: JSON.stringify(parameter) }, function (data) {
                    var value = $.parseJSON(data);
                    if (value.IsError == true) {
                        alert(value.Error);
                    }
                    else {
                        var fields = $(value.DesignerContent);
                        section.append(fields);
                        $(".divRelation").append($(value.OtherContent));
                        pcNewField.close();
                    }
                });
            }
        }

        function SelectField() {
            pcSelectField.open();
        }

        function SelectFieldSave() {
            var parameter = [];
            $("#divSelectField input:checked").each(function (i, p) {
                var field = {
                    FieldId: $(p).attr("fid"),
                    RelationId: $(p).parent().parent().parent().attr("rid")
                };
                parameter.push(field);
            });

            var type = "SelectField";
            var id = hc.getText();
            var section = $("#" + id);
            cbc.callBack({ Type: type, SelectedId: id, Parameter: JSON.stringify(parameter) }, function (data) {
                var value = $.parseJSON(data);
                if (value.IsError == true) {
                    alert(value.Error);
                }
                else {
                    var fields = $(value.DesignerContent);
                    section.append(fields);
                    pcSelectField.close();
                }
            });
        }

        function Delete() {
            confirm("确定删除？", function () {
                var type = "Delete";
                var id = hc.getText();
                var obj = $("#" + id);
                cbc.callBack({ Type: type, SelectedId: id, Parameter: null }, function (data) {
                    var value = $.parseJSON(data);
                    if (value.IsError == true) {
                        alert(value.Error);
                    }
                    else {
                        obj.remove();
                        Select(".form");
                    }
                });
            });
        }

        function MoveUp() {
            var type = "MoveUp";
            var id = hc.getText();
            var obj = $("#" + id);
            cbc.callBack({ Type: type, SelectedId: id, Parameter: null }, function (data) {
                var value = $.parseJSON(data);
                if (value.IsError == true) {
                    alert(value.Error);
                }
                else {
                    obj.prev().before(obj);
                }
            });
        }

        function MoveDown() {
            var type = "MoveDown";
            var id = hc.getText();
            var obj = $("#" + id);
            cbc.callBack({ Type: type, SelectedId: id, Parameter: null }, function (data) {
                var value = $.parseJSON(data);
                if (value.IsError == true) {
                    alert(value.Error);
                }
                else {
                    obj.next().after(obj);
                }
            });
        }

        function Property() {
            var type = "PropertyRead";
            var id = hc.getText();
            cbc.callBack({ Type: type, SelectedId: id, Parameter: null }, function (data) {
                var value = $.parseJSON(data);
                if (value.IsError == true) {
                    alert(value.Error);
                }
                else {
                    if (value.OtherContent.DisplayType != null) {
                        ccDisplayType.setValue(value.OtherContent.DisplayType);
                    }
                    txtCusomLabel.setValue(value.OtherContent.CustomLabel);
                    txtMaxLength.setValue(value.OtherContent.MaxLength);
                    txtMinLength.setValue(value.OtherContent.MinLength);
                    txtMaxValue.setValue(value.OtherContent.MaxValue);
                    txtMinValue.setValue(value.OtherContent.MinValue);
                    if (value.OtherContent.IsNullable != null) {
                        ccIsNullable.setValue(value.OtherContent.IsNullable);
                    }
                    else {
                        ccIsNullable.setValue(true);
                    }
                    txtDefaultValue.setValue(value.OtherContent.DefaultValue);
                    pcFieldProperty.open();
                }
            });            
        }

        function FieldPropertySave() {
            var type = "PropertySave";
            var id = hc.getText();
            var parameter = {
                DisplayType : ccDisplayType.getValue(),
                CustomLabel : txtCusomLabel.getValue(),
                MaxLength : txtMaxLength.getValue(),
                MinLength : txtMinLength.getValue(),
                MaxValue : txtMaxValue.getValue(),
                MinValue : txtMinValue.getValue(),
                IsNullable: ccIsNullable.getValue(),
                DefaultValue: txtDefaultValue.getValue()
            };
            var obj = $("#" + id);
            cbc.callBack({ Type: type, SelectedId: id, Parameter: JSON.stringify(parameter) }, function (data) {
                var value = $.parseJSON(data);
                if (value.IsError == true) {
                    alert(value.Error);
                }
                else {
                    if (value.DesignerContent != null && value.DesignerContent != "") {
                        obj.text(value.DesignerContent);
                    }
                    pcFieldProperty.close();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="containerTable">
        <tr>
            <td class="form selected" id='<%= this.FormId %>' onclick="Select(this);">
                <asp:Repeater ID="rSection" runat="server">
                    <ItemTemplate>
                        <div class="section unselected" id='<%# Eval("FormSectionId") %>' onclick="Select(this);">
                            <asp:Repeater ID="rFormField" runat="server" DataSource='<%# Eval("Fields") %>'>
                                <ItemTemplate>
                                    <div class="field unselected" id='<%# Eval("FormFieldId") %>' onclick="Select(this);">
                                        <%# Eval("DisplayText") %>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </td>
            <td class="operationTd">
                <div id="divAdd" class="button_bar">
                    <h2>
                        添加</h2>
                    <div id="btnNewSection">
                        <a onclick="NewFormSection();">表单段落</a></div>
                    <div id="btnSelectField">
                        <a onclick="SelectField();">现有字段</a></div>
                    <div id="btnNewField">
                        <a onclick="NewField();">新增字段</a></div>
                </div>
                <div id="divOperation" class="button_bar">
                    <h2>
                        操作</h2>
                    <div id="btnProperty">
                        <a onclick="Property();">属性</a></div>
                    <div id="btnMoveUp">
                        <a onclick="MoveUp();">前移</a></div>
                    <div id="btnMoveDown">
                        <a onclick="MoveDown();">后移</a></div>
                    <div id="btnDelete">
                        <a onclick="Delete();">删除</a></div>
                </div>
            </td>
        </tr>
    </table>
    <div class="button_bar">
        <asp:LinkButton ID="btnPrev" runat="server" Text="上一步" OnClick="btnPrev_Click"></asp:LinkButton>
        <asp:LinkButton ID="btnNext" runat="server" Text="下一步" OnClick="btnNext_Click"></asp:LinkButton>
    </div>
    <asp:HiddenControl ID="hc" runat="server" ClientInstanceName="hc" />
    <asp:CallBackControl ID="cbc" runat="server" CallBackType="JSON" ClientInstanceName="cbc"
        OnCallBack="cbc_CallBack" />
    <asp:PopupControl ID="pcNewField" runat="server" ClientInstanceName="pcNewField"
        Width="330" Height="250" Title="新增字段">
        <PopupContent ID="pcNewFieldContent" runat="server">
            <table class="tableNewField">
                <tr>
                    <td>
                        字段显示名称：
                    </td>
                    <td>
                        <asp:TextControl ID="txtNewField" runat="server" ClientInstanceName="txtNewField"
                            MaxLength="8" IsRequired="true" ValidateFunction="IsCN" ValidateMessage="输入长度必须小于8且不能带特殊字符"
                            PlaceHolder="请输入字段显示名称" />
                    </td>
                </tr>
                <tr>
                    <td>
                        数据库字段名：
                    </td>
                    <td>
                        <asp:TextControl ID="txtNewFieldName" runat="server" ClientInstanceName="txtNewFieldName"
                            ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        字段类型：
                    </td>
                    <td>
                        <asp:ComboControl ID="ccFieldType" runat="server" ClientInstanceName="ccFieldType"
                            DropdownType="DropdownList" AutoSelectFirst="true">                            
                        </asp:ComboControl>
                    </td>
                </tr>
            </table>
        </PopupContent>
        <Buttons>
            <asp:PopupButton Text="确定" OnClick="NewFieldSave" />
            <asp:PopupButton Text="取消" OnClick="function(s,e){pcNewField.close();}" />
        </Buttons>
    </asp:PopupControl>
    <asp:PopupControl ID="pcSelectField" runat="server" ClientInstanceName="pcSelectField"
        Width="550" Height="550" Title="选择现有字段">
        <PopupContent ID="pcSelectFieldContent" runat="server">
            <div id="divSelectField">
                <asp:Repeater ID="rRelation" runat="server">
                    <ItemTemplate>
                        <div class="divRelation" rid='<%# Eval("RelationId") %>'>
                            <%# Eval("DisplayName") %>
                            <asp:Repeater ID="rSelectField" runat="server" DataSource='<%# Eval("Fields") %>'>
                                <ItemTemplate>
                                    <div class="divField">
                                        <span>
                                            <input type="checkbox" fid='<%# Eval("FieldId") %>' /></span><span><%# Eval("DisplayText") %></span><span><%# Eval("FieldName") %></span></div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </PopupContent>
        <Buttons>
            <asp:PopupButton Text="确定" OnClick="SelectFieldSave" />
            <asp:PopupButton Text="取消" OnClick="function(s,e){pcSelectField.close();}" />
        </Buttons>
    </asp:PopupControl>
    <asp:PopupControl ID="pcFieldProperty" runat="server" ClientInstanceName="pcFieldProperty" Width="330" Height="450"
        Title="表单字段属性">
        <PopupContent ID="pcFieldPropertyContent" runat="server">
            <table class="tableFieldProperty">
                <tr>
                    <td>
                        显示形式：
                    </td>
                    <td>
                        <asp:ComboControl ID="ccDisplayType" runat="server" ClientInstanceName="ccDisplayType" DropdownType="DropdownList"  AutoSelectFirst="true">                            
                        </asp:ComboControl>
                    </td>
                </tr>
                <tr>
                    <td>
                        自定义标签：
                    </td>
                    <td>
                        <asp:TextControl ID="txtCusomLabel" runat="server" ClientInstanceName="txtCusomLabel"
                            MaxLength="25" />
                    </td>
                </tr>
                <tr>
                    <td>
                        字符最大长度：
                    </td>
                    <td>
                        <asp:TextControl ID="txtMaxLength" runat="server" ClientInstanceName="txtMaxLength"
                            TextType="Int16" MinValue="1" MaxValue="4000" />
                    </td>
                </tr>
                <tr>
                    <td>
                        字符最小长度：
                    </td>
                    <td>
                        <asp:TextControl ID="txtMinLength" runat="server" ClientInstanceName="txtMinLength"
                            TextType="Int16" MinValue="0" MaxValue="4000" />
                    </td>
                </tr>
                <tr>
                    <td>
                        数字最大值：
                    </td>
                    <td>
                        <asp:TextControl ID="txtMaxValue" runat="server" ClientInstanceName="txtMaxValue"
                            TextType="Decimal" />
                    </td>
                </tr>
                <tr>
                    <td>
                        数字最小值：
                    </td>
                    <td>
                        <asp:TextControl ID="txtMinValue" runat="server" ClientInstanceName="txtMinValue"
                            TextType="Decimal"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        可以为空：
                    </td>
                    <td>
                        <asp:CheckControl ID="ccIsNullable" runat="server" ClientInstanceName="ccIsNullable" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        默认值：
                    </td>
                    <td>
                        <asp:TextControl ID="txtDefaultValue" runat="server" ClientInstanceName="txtDefaultValue" MaxLength="250" />
                    </td>
                </tr>
            </table>
        </PopupContent>
        <Buttons>
            <asp:PopupButton Text="确定" OnClick="FieldPropertySave" />
            <asp:PopupButton Text="取消" OnClick="function(s,e){pcFieldProperty.close();}" />
        </Buttons>
    </asp:PopupControl>
    <script type="text/javascript">
        function IsCN(str) {
            var reg = /^[a-z0-9A-Z\u4E00-\u9FA5]+$/;
            if (!reg.test(str)) {
                return false;
            }
            return true;
        }

        Sys.Application.add_load(function () {
            if (window.txtNewField != undefined) {
                $(txtNewField._container).keyup(function () {
                    if (bValidator.validate(true, $(this)) == true) {
                        var str = txtNewField.getValue();
                        if (str != null && str != "") {
                            $.post("ConvertPinYin.ashx", { Value: str }, function (data) {
                                txtNewFieldName.setValue(data);
                            });
                        }
                    }
                    else {
                        txtNewFieldName.setValue(null);
                    }
                });
            }
        });

        Sys.Application.add_load(function () {
            Select($(".form"));
        });
    </script>
</asp:Content>
