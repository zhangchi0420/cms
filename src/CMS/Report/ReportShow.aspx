<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportShow.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportShow" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
    <h3>查询条件</h3>
    <div class="allcol2" runat = "server" id= "divQueryCondition">
    </div>
</div>
<div class="button_bar">
<asp:LinkButton ID="btnQuery" runat="server" onclick="btnQuery_Click" Text = "统计" CssClass = "btnQuery"/>
<asp:LinkButton ID="btnReturn" runat="server" onclick="btnReturn_Click" Text = "返回" Visible = "false" CssClass = "btnQuery"/>
</div>
<div class="main_box">
    <h3>统计</h3>
    <div runat = "server" id = "divEmptyData">
        暂无数据显示
    </div>
    <div runat = "server" id = "divData" visible = "false">
    <div class="button_bar">
        <asp:LinkButton ID="btnExportList" runat="server" onclick="btnExportList_Click" Text = "列表导出" CssClass = "btnQuery"/>
        <asp:LinkButton ID="btnExportChart" runat="server" onclick="btnExportChart_Click" Text = "图形导出" CssClass = "btnQuery"/>
    </div>
    <div class = "grid_tbody">
        <div class = "ui-widget grid_content">
            <asp:GridView ID="gcList" runat="server"  AutoGenerateColumns="false" 
                ShowFooter="false" CssClass = "grid_tbody">
                <AlternatingRowStyle CssClass = "grid_row grid_content_row" />
            </asp:GridView>
        </div>
    </div>

    <div style = "text-align:center; overflow:auto" runat = "server" id = "divChart">
        <asp:Chart ID="chReport" runat="server" Palette="BrightPastel" ImageType="Png" BackSecondaryColor="White" BackGradientStyle="TopBottom"  
                BorderWidth="2" BackColor="WhiteSmoke" BorderColor="26, 59, 105" AntiAliasing="All" BorderlineDashStyle="Solid" Width = "1038" Height = "500" OnClick = "chReport_Click" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
            <Series>
            </Series>
            <Legends>
                <asp:Legend>
                </asp:Legend>
            </Legends>
            <borderskin SkinStyle="Emboss"></borderskin>
            <ChartAreas>
                <asp:ChartArea Name="caReport" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent" BackGradientStyle="TopBottom">
            	    <area3dstyle Rotation="0" />
				    <axisy LineColor="64, 64, 64, 64">
					    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
					    <MajorGrid LineColor="64, 64, 64, 64" />
				    </axisy>
				    <axisx LineColor="64, 64, 64, 64">
					    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
					    <MajorGrid LineColor="64, 64, 64, 64" />
				    </axisx>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
    </div>
</div>

</asp:Content>
