<%@ Page Language="c#" Inherits="GlobalLink.Commerce.ui.CommGadget" AutoEventWireup="true" CodeBehind="CommGadget.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>GlobalLink Commerce Gadget</title>
        <link rel="stylesheet" type="text/css" href="<%= EPiServer.Shell.Paths.ToShellClientResource("ClientResources/epi/themes/sleek/sleek.css")  %>"/>
		<style>
		.epi-cmsButton
		{
			display: inline-block;
			white-space: nowrap;
			-webkit-border-radius: 3px;
			-moz-border-radius: 3px;
			-ms-border-radius: 3px;
			-o-border-radius: 3px;
			border-radius: 3px;
			padding: 2px 6px;
			border: 1px solid #768388;
			color: #333;
			margin: 0;
			background:#E7E7E7;
			cursor:pointer;
		}
        .Sleek .epi-icon--medium.iconCustom, .Sleek .iconCustom2 {
                width: 26px;
height: 26px;
border-radius: 3px;
padding: 2px 4px;
border: 1px solid #768388;
box-shadow: 0 1px 0 0 rgba(255,255,255,0),inset 0 1px 0 rgba(255,255,255,0),inset 0 -1px 0 rgba(255,255,255,0);
color: #333;
margin:5px;
cursor:pointer;
		    }
		</style>
    </head>
    <body class="Sleek">
        <form id="Form1" runat="server" action="CommGadget.aspx">
            <asp:Label ID="notLocalizable" Visible="false" runat="server" Text="Content not localizable" />
            <asp:Label ID="noAccess" Visible="false" runat="server" Text="You don't have access to the GlobalLink Gadget" />
               <asp:Button ID="btnNewProjectBtn" runat="server" CssClass="epi-iconPlus epi-icon--medium iconCustom"  onclick="btnNewSubmission_Click" ToolTip="New submission" />
               <asp:Button ID="viewDashboard" runat="server" CssClass="epi-iconMenu epi-icon--medium iconCustom" onclick="btnViewDashboard_Click" ToolTip="GlobalLink Dashboard" />
        </form>
    </body>
</html>