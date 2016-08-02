<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeAddressBook.ascx.cs"
    Inherits="Drision.Framework.Web.Home.HomeAddressBook" %>
<script type="text/javascript">
    var table = $("#tAddress");

    function PrevPage() {
        var index = parseInt($("#sCurrentPage").html());
        var total = parseInt($("#sTotalPage").html());

        index = index - 1;

        if (index > 0) {
            cbcPage.callBack(index - 1, function (data) {
                $("#tAddress").find("tbody > tr").slice(1, 4).remove();
                $("#tAddress").find("tbody").append(data);

                $("#sCurrentPage").html(index);
            });
        }
    }

    function NextPage() {
        var index = parseInt($("#sCurrentPage").html());
        var total = parseInt($("#sTotalPage").html());

        index = index + 1;
        if (index <= total) {
            cbcPage.callBack(index - 1, function (data) {
                $("#tAddress").find("tbody > tr").slice(1, 4).remove();
                $("#tAddress").find("tbody").append(data);

                $("#sCurrentPage").html(index);
            });
        }
    }

    function QueryData() {
        var parameter = $("#txtQuery").val();
        cbcQuery.callBack(parameter, function (data) {
            var result = $.parseJSON(data);
            if (result.Content != "") {
                $("#sCurrentPage").html(result.Index);
                $("#tAddress").find("tbody > tr").slice(1, 4).remove();
                $("#tAddress").find("tbody").append(result.Content);
            }
        });
    }
</script>
<div class="main_box content_box" style="width: 260px; height: 250px;">
    <h3>
        通讯簿<span></span></h3>
    <div class="metro_tool" style="height: 40px">
        <input style="width: 150px; *width: auto; height: 20px; line-height: 20px; margin: 0 5px;"
            name="" type="text" id="txtQuery" />
        <input style="height: 24px; padding: 0 5px; line-height: 24px" name="" type="button"
            value="查询" onclick="QueryData();" />
    </div>
    <div class=" metro_grid">
        <table id="tAddress">
            <tr>
                <th>
                    姓名
                </th>
                <th>
                    工号
                </th>
                <th>
                    联系电话
                </th>
            </tr>
            <asp:Repeater ID="rUserAddress" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("User_Name") %>
                        </td>
                        <td>
                            <%# Eval("User_Code") %>
                        </td>
                        <td>
                            <%# Eval("User_Mobile") %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tfoot>
                <tr>
                    <td colspan="3">
                        <a style="cursor: pointer" onclick="PrevPage();">上一页</a><span id="sCurrentPage">1</span>/<span
                            id="sTotalPage"><%= this.TotalPage %></span><a style="cursor: pointer" onclick="NextPage();">下一页</a>
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
    <asp:CallBackControl ID="cbcPage" ClientInstanceName="cbcPage" runat="server" OnCallBack="cbcPage_CallBack" />
    <asp:CallBackControl ID="cbcQuery" ClientInstanceName="cbcQuery" runat="server" OnCallBack="cbcQuery_CallBack" />
    <div class="metro_more">
        <span><a href="../SystemManagement/AddressBook.aspx">查看通讯簿 ></a></span></div>
</div>
