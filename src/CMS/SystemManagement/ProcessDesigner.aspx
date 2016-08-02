<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcessDesigner.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ProcessDesigner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>流程设计器</title>    
</head>
<body>
    <form id="form1" runat="server" style="height:100%">
    <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
    <div id="silverlightControlHost" style="padding-left:50px;padding-top:50px;">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="1060px" height="700px">
		  <param name="source" value="../ClientBin/Drision.Framework.WorkflowDesigner.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="4.0.60310.0" />          
		  <param name="autoUpgrade" value="true" />
          <param name="InitParams" value="Type=Designer,ProcessId=<%= this.ProcessId %>" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.60310.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="获取 Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe></div>
    </form>
</body>
</html>
