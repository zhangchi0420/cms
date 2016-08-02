<%@ Page Title="功能管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FunctionManagement.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.FunctionManagement" %>

<%@ Import Namespace="Drision.Framework.Web.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //由于这个确定按钮不是submit，所以要手动验证
        function FunctionValidate() {
            var c = bValidator.validate();
            return c;
        }

        //点击弹出框的确定
        function OnSave() {
            if (FunctionValidate() == true) {
                //收集表单数据
                var item = {
                    PermissionName: txtName.getText(),
                    PermissionType: cbType.getValue(),
                    SecondPermissionType: cbSecondType.getValue(),
                    FunctionComment: txtComment.getText(),
                    FunctionId: hcId.getValue(),
                    URL: txtUrl.getText(),
                    IsMenu: chkIsMenu.getValue(),
                    EntityId: cbEntity.getValue(),
                    PageId: cbPage.getValue(),
                    OrderIndex: txtIndex.getText(),
                    Category: txtCategory.getText(),
                    ClassName: txtClassName.getText()
                };
                //回调
                cbcSave.callBack(item);
            }
        }

        //保存完成
        function OnSaveComplete(data) {
            if (data == "1") {
                pc.close();
                eval(hcId.getText()); //刷新
            }
            else {
                OnError("服务端异常，保存失败！");
            }
        }

        //点击添加或编辑
        function OnOpenEditor(s) {
            var id = $(s).attr("dataValue");
            cbcOpen.callBack(id);
            return false;
        }

        //从服务端获得要添加/编辑的对象信息完毕
        function OnOpenComplete(value) {
            var data = $.parseJSON(value); //parseJSON

            pc.open(); //弹出

            //赋值
            txtName.setText(data.PermissionName);
            cbType.setValue(data.PermissionType == "" ? null : data.PermissionType);
            txtComment.setText(data.FunctionComment);
            hcId.setValue(data.FunctionId);
            txtUrl.setText(data.URL);
            chkIsMenu.setValue(data.IsMenu);
            cbEntity.setValue(data.EntityId == "" ? null : data.EntityId);
            txtIndex.setText(data.OrderIndex);
            txtCategory.setText(data.Category);
            txtClassName.setText(data.ClassName);

            if (data.EntityId == null || data.EntityId == "") {
                cbPage.callBack(null);
            }
            else {
                cbPage.callBack(data.EntityId, function () {
                    cbPage.setValue(data.PageId);
                });
            }

            if (data.PermissionType == null || data.PermissionType == "") {
                cbSecondType.callBack(null);
            }
            else {
                cbSecondType.callBack(data.PermissionType, function () {
                    cbSecondType.setValue(data.SecondPermissionType);
                });
            }


            bValidator.reset(); //重置验证
        }

        //回调出错
        function OnError(msg) {
            alert(msg);
        }

        //点击弹出框的取消
        function OnCancel() {
            bValidator.reset();
            pc.close();
        }

        //删除
        function OnDelete() {
            var result = false;
            var values = grid.getValue();
            if (values.length == 0) {
                alert("请选择要删除的功能");                
            }
            confirm('确认删除所选择的功能？',function(){
                result = true;
            });
            return result;
        }
        $(document).ready(function () {
            var alian = '<%=AppSettings.PageFieldRedStarAlign %>';
            $(".left_star").addClass(alian + "_star");
            $(".right_star").removeClass("left_star");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div class="main_box">
                <h3>
                    查询条件</h3>
                <div class="allcol2">
                    <div class="item_box_col1">
                        <span>菜单名称 </span><span class="left_star">&nbsp;</span>
                        <asp:ComboControl DropdownType="AutoComplete" ID="cbNameQuery" runat="server">
                        </asp:ComboControl>
                    </div>
                    <div class="item_box_col1">
                        <span>父菜单 </span><span class="left_star">&nbsp;</span>
                        <asp:ComboControl ID="ddlParent" runat="server">
                        </asp:ComboControl>
                    </div>                    
                    <div class="cl">
                    </div>
                </div>
            </div>
            <div class="button_bar">
                <asp:LinkButton ID="lbtnQuery" runat="server" OnClick="btnQuery_Click">查询</asp:LinkButton>
                <asp:LinkButton ID="lbtnAdd" runat="server" OnClientClick="return OnOpenEditor(this);">添加</asp:LinkButton>                
            </div>
            <div class="grid_title" id="divViewControlTitle">
                菜单列表</div>
            <div>
                <asp:GridControl ID="grid" runat="server" ClientInstanceName="grid" OnPageIndexChanging="grid_PageChanging"
                    OnHeaderClick="grid_HeaderClick">
                    <Columns>
                        <asp:FullCheckField DataValueField="FunctionId">
                        </asp:FullCheckField>
                        <asp:OperationField>
                            <Buttons>
                                <asp:LinkButton ID="btnEdit" OnClientClick="return OnOpenEditor(this)" CommandArgument="FunctionId"
                                    runat="server" Text="编辑"></asp:LinkButton>
                            </Buttons>
                        </asp:OperationField>                        
                        <asp:HeaderSortField DataTextField="PermissionName" HeaderText="菜单名称" DisplayLength="6">
                        </asp:HeaderSortField>
                        <asp:HeaderSortField DataTextField="PermissionType" HeaderText="父菜单" DisplayLength="6">
                        </asp:HeaderSortField>
                        <asp:HeaderSortField DisplayLength="20" DataTextField="Category" HeaderText="菜单类别">
                        </asp:HeaderSortField>                        
                        <asp:HeaderSortField DisplayLength="35" DataTextField="PageUrl" HeaderText="最终路径">
                        </asp:HeaderSortField>                        
                    </Columns>
                    <PagerSettings PageSize="100000">
                        <PagerButtons>
                            <asp:LinkButton ID="btnDelete" OnClientClick="return OnDelete();" OnClick="btnDelete_Click"
                                runat="server" Text="删除"></asp:LinkButton>
                        </PagerButtons>
                    </PagerSettings>
                </asp:GridControl>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenControl ClientInstanceName="hcId" ID="hcId" runat="server" />
    <asp:CallBackControl CallBackType="JSON" ID="cbcSave" runat="server" ClientInstanceName="cbcSave"
        OnCallBack="cbcSave_CallBack" OnComplete="OnSaveComplete" OnError="OnError" />
    <asp:CallBackControl ID="cbcOpen" runat="server" ClientInstanceName="cbcOpen" OnCallBack="cbcOpen_CallBack"
        OnComplete="OnOpenComplete" OnError="OnError" />
    <asp:PopupControl Title="新增/编辑功能" ClientInstanceName="pc" Width="450px" Height="550px"
        ID="pc" runat="server">
        <PopupContent ID="pContent" runat="server">
            <div class="h20">
            </div>
            <div class="item_box_col1">
                <span>父功能(一级)</span>
                <asp:ComboControl Width="300" ShowEmptyItem="true" AutoSelectFirst="true" ClientInstanceName="cbType"
                    ID="cbPermissionType" runat="server">
                    <ClientSideEvents OnSelectChanged="function(s,e){
                        cbSecondType.callBack(e.Value);
                    }" />
                </asp:ComboControl>
            </div>
            <div class="item_box_col1">
                <span>父功能(二级)</span>
                <asp:ComboControl Width="300" ShowEmptyItem="true" AutoSelectFirst="true" ClientInstanceName="cbSecondType"
                    ID="cbSecondPermissionType" runat="server" OnCustomCallBack="cbSecondPermissionType_CallBack">                    
                </asp:ComboControl>
            </div>
            <div class="item_box_col1">
                <span>功能名称</span>
                <asp:TextControl Width="300" ClientInstanceName="txtName" ID="txtName" runat="server" IsRequired="true"
                    MaxLength="25" />
            </div>
            <div class="item_box_col1">
                <span>类别</span>
                <asp:TextControl Width="300" ClientInstanceName="txtCategory" ID="txtCategory" runat="server" MaxLength="100" />
            </div>
            <div class="item_box_col1">
                <span>样式名称</span>
                <asp:TextControl Width="300" ClientInstanceName="txtClassName" ID="txtClassName" runat="server" MaxLength="100" />
            </div>
            <div class="item_box_col1">
                <span>是否菜单</span>
                <asp:CheckControl ClientInstanceName="chkIsMenu" ID="chkIsMenu" runat="server" />
            </div>
            <div class="item_box_col1">
                <span>顺序</span>
                <asp:TextControl Width="300" ClientInstanceName="txtIndex" ID="txtIndex" runat="server" 
                    TextType="Int32" MaxValue="10000" MinValue="0" />
            </div>           
            <div class="item_box_col1">
                <span>所属实体</span>
                <asp:ComboControl Width="300" ShowEmptyItem="true" ClientInstanceName="cbEntity" ID="cbEntity"
                    runat="server" AutoSelectFirst="true">
                    <ClientSideEvents OnSelectChanged="function(s,e){
                                cbPage.callBack(e.Value);
                            }" />
                </asp:ComboControl>
            </div>
            <div class="item_box_col1">
                <span>所属页面</span>
                <asp:ComboControl Width="300" ClientInstanceName="cbPage" ShowEmptyItem="true" ID="cbPage" OnCustomCallBack="cbPage_CallBack"
                    runat="server" AutoSelectFirst="true">
                </asp:ComboControl>
            </div>
            <div class="item_box_col1">
                <span>自定义URL</span>
                <asp:TextControl Width="300" ClientInstanceName="txtUrl" ID="txtUrl" runat="server" />
            </div>
            <div class="item_box_col1">
                <span>功能描述</span>
                <asp:TextControl Width="300" ClientInstanceName="txtComment" ID="txtComment" runat="server" TextType="Text"
                    Height="100px" MaxLength="500" />
            </div>
            <div class="cl">
            </div>
        </PopupContent>
        <Buttons>
            <asp:PopupButton Text="确定" OnClick="OnSave" />
            <asp:PopupButton Text="取消" OnClick="OnCancel" />
        </Buttons>
    </asp:PopupControl>
</asp:Content>
