<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleOfEntityControl.ascx.cs"
    Inherits="Drision.Framework.Web.SystemManagement.RoleOfEntityControl" %>
<table width="100%" style="text-align: center; border-collapse: collapse; table-layout: fixed;"
    border="0" cellspacing="0" cellpadding="0">
    <tr>
        <th style="width: 30px;">
            <span>&nbsp;</span>
        </th>
        <th style="width:250px;">
            <span>实体表</span>
        </th>
        <th style="width: 16%;">
            <span>添加权限</span>
        </th>
        <th style="width: 16%;">
            <span>删除权限</span>
        </th>
        <th style="width: 16%;">
            <span>更新权限</span>
        </th>
        <th style="width: 16%">
            <span>查询权限</span>
        </th>
        <th style="width: 16%">
        </th>
    </tr>
    <tr title='<%# Eval("Description")%>'>
        <td colspan="2" style="text-align: center">
            整列批选
        </td>
        <td>
            <select cell="2" onchange="cellChange(this)" class="cellOPsel" style="width: 90%;">
                <option selected="selected" value="-1">请选择</option>
                <option value="5" >全部权限</option>
                        <option value="4" >部门及子部门</option>
                        <option value="3" >部门</option>
                        <option value="2" >个人</option>
                        <option value="1" >无权限</option>
            </select>
        </td>
        <td>
            <select cell="3" onchange="cellChange(this)" class="cellOPsel" style="width: 90%;">
                <option selected="selected" value="-1">请选择</option>
                <option value="5" >全部权限</option>
                        <option value="4" >部门及子部门</option>
                        <option value="3" >部门</option>
                        <option value="2" >个人</option>
                        <option value="1" >无权限</option>
            </select>
        </td>
        <td>
            <select cell="4" onchange="cellChange(this)" class="cellOPsel" style="width: 90%;">
                <option selected="selected" value="-1">请选择</option>
                <option value="5" >全部权限</option>
                        <option value="4" >部门及子部门</option>
                        <option value="3" >部门</option>
                        <option value="2" >个人</option>
                        <option value="1" >无权限</option>
            </select>
        </td>
        <td>
            <select cell="5" onchange="cellChange(this)" class="cellOPsel" style="width: 90%;">
                <option selected="selected" value="-1">请选择</option>
                <option value="5" >全部权限</option>
                        <option value="4" >部门及子部门</option>
                        <option value="3" >部门</option>
                        <option value="2" >个人</option>
                        <option value="1" >无权限</option>
            </select>
        </td>
        <td>
            整行批选
        </td>
    </tr>
    <asp:Repeater ID="TrRept" runat="server">
        <ItemTemplate>
            <tr title='<%# Eval("Description")%>'>
                <td style="text-align: center">
                    <%#Container.ItemIndex + 1%>
                </td>
                <td style="text-align: left; overflow: hidden; white-space: nowrap;">
                    <%# Eval("EntityName")%>(<%# Eval("DisplayText")%>)
                </td>
                <td>
                    <asp:DropDownList CssClass="cell2" ID="ddlAdd" runat="server" Width="90%">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList CssClass="cell3" ID="ddlDel" runat="server" Width="90%">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList CssClass="cell4" ID="ddlUpdt" runat="server" Width="90%">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList CssClass="cell5" ID="ddlQuery" runat="server" Width="90%">
                    </asp:DropDownList>
                </td>
                <td>
                    <select onchange="rowChange(this)" onmouseover="removeOption(this);" style="width: 90%;">
                        <option selected="selected" value="-1">请选择</option>
                        <option value="5" >全部权限</option>
                        <option value="4" >部门及子部门</option>
                        <option value="3" >部门</option>
                        <option value="2" >个人</option>
                        <option value="1" >无权限</option>
                    </select>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr title='<%# Eval("Description")%>' style="background-color: #E5EAEF">
                <td style="text-align: center">
                    <%#Container.ItemIndex + 1%>
                </td>
                <td style="text-align: left; overflow: hidden; white-space: nowrap;">
                    <%# Eval("EntityName")%>(<%# Eval("DisplayText")%>)
                </td>
                <td>
                    <asp:DropDownList CssClass="cell2" ID="ddlAdd" runat="server" Width="90%">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList CssClass="cell3" ID="ddlDel" runat="server" Width="90%">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList CssClass="cell4" ID="ddlUpdt" runat="server" Width="90%">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList CssClass="cell5" ID="ddlQuery" runat="server" Width="90%">
                    </asp:DropDownList>
                </td>
                <td>
                    <select onchange="rowChange(this)" onmouseover="removeOption(this);" style="width: 90%;">
                        <option selected="selected" value="-1">请选择</option>
                        <option value="5" >全部权限</option>
                        <option value="4" >部门及子部门</option>
                        <option value="3" >部门</option>
                        <option value="2" >个人</option>
                        <option value="1" >无权限</option>
                    </select>
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:Repeater>
</table>
<script language="javascript" type="text/javascript">


    /*
    function ChangeColor(node) {
    $(node).parent().css("background-color") = $(node).css("background-color");
    }
    */
    /* 改变选项颜色 */
    function Reset() {
        $("td select").each(function (i, node) {
            if (node.value != 1) {
                jQuery.each(node.options, function (o, item) {
                    if (item.value == 1) {
                        node.selectedIndex = o;
                    }
                });
            }
        });
        alert('重置页面成功,点击保存!');
    }

    $("td select option").each(function (i, node) {
//        if (node.value == 1) {
//            $(node).css("background-color", "#c9ccc4");
//        }
//        else if (node.value == 2) {
//            $(node).css("background-color", "#d2f5b0");
//        }
//        else if (node.value == 3) {
//            $(node).css("background-color", "#c2dfff");
//        }
//        else if (node.value == 4) {
//            $(node).css("background-color", "#fff494");
//        }
//        else if (node.value == 5) {
//            $(node).css("background-color", "#ffcd85");
//        }
    });

    function removeDelaultOp(sel) {
        $("option", sel).each(function (index, op) {
            var currop = $(op);
            if (currop.val() == -1) {
                currop.remove();
            }
        });
    }

    function cellChange(sel) {
        var thi = $(sel);
        //选中的值
        var sval = thi.val();
        var cell = thi.attr("cell");
        $(".cell" + cell).each(function (index, select) {
            $(select).val(sval);
        });
        removeDelaultOp(thi);
    }

    function rowChange(sel) {
        var thi = $(sel);
        //选中的值
        var sval = thi.val();
        thi.parent().parent().find("select").each(function (index, select) {
            $(select).val(sval);
        });

        removeDelaultOp(thi);
    }

    function removeOption(sel) {
        var thi = $(sel);
        //选中的值
        var sval = thi.val();
        var ops = thi.parent().parent().find("select:first").find("option");
        if (ops.length == 2) {
            $("option", thi).each(function (index, op) {
                var currop = $(op);
                if (currop.val() != 5 && currop.val() != 1 && currop.val() != -1) {
                    currop.remove();
                }
            });
        }
    }
    



</script>
<script src="../js/RoleOfEntityManage.js" type="text/javascript"></script>
