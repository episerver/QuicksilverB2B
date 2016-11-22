<%@ Page Language="c#" CodeBehind="PageTranslation.aspx.cs" AutoEventWireup="true"
	EnableViewState="true" EnableViewStateMac="true" Inherits="GlobalLink.Translation.ui.PageTranslation" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="<%# EPiServer.Shell.Paths.ToShellClientResource("ClientResources/epi/themes/sleek/sleek.css")  %>"/>
	<style type="text/css">
        html,head,body{margin:0; padding:0; font: 13px Verdana, Arial, Helvetica, sans-serif;}
		table.epi-default
		{
			width: 100%;
			border-collapse: collapse;
			border: none;
			empty-cells: show;
			border: 1px solid #aeaeae;
			color: #131313;
		}
		.epi-header
		{
			border: 1px solid #aeaeae;
			background: #e4e4e4 url(images/Gradients.png) 0px -2300px repeat-x;
			-webkit-background-size: auto 100% !important;
			background-size: 100% 100%;
			font-weight: normal;
			height: 25px;
		}
		.epi-header th
		{
			font-weight: normal;
		}
		tr, td, th
		{
			font-family: Verdana,Arial,Helvetica,sans-serif;
			font-size: 13px;
		}
		tr, td, th a
		{
			color:#131313;
		}
		th
		{
			text-align:left;
		}
		table.epi-default td
		{
			padding: 3px 11px;
			height:auto;
			height: 35px;
			border-top:1px solid #000 ;
			border-bottom:1px solid #000 ;
		}
		table.epi-default tbody > tr:nth-child(odd) > td
		{
			background-color: #f3f3f3;
		}
		.menutabs
		{
			margin-left: 13px !important;
		}
		.notSelected
		{
			font-weight: bold;
			background-color: #DDD;
			border: Solid 1px black;
			padding: 5px 10px !important;
			color: #111;
			font-family: Verdana,Arial,Helvetica,sans-serif;
			font-size: 13px;
		}
		.pagespacing
		{
			margin: 10px;
		}
		.selected
		{
			background: #58595b url("<%# EPiServer.Shell.Paths.ToShellClientResource("ClientResources/epi/themes/sleek/layout/images/tabstrip_connector.png")  %>") center bottom no-repeat;
box-shadow: 0 -3px 5px rgba(0,0,0,0.1) inset;
text-shadow: 0 -1px rgba(0,0,0,0.1);
        color:#FFF;
		}
        a{color:#333; text-decoration:none}
        .dijitTabListContainer-top{padding:0;margin:0}
		.empty
		{
			min-width: 0;
		}
        .spp.dijitTextBox{width:auto}
		tr.pager td table td
		{
			padding: 0 5px 0 0;
		}
		tr.pager td table td a
		{
			text-decoration: underline;
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
        .Sleek .iconCustom2 {background-position: 50% 50%;}
        .Sleek .iconCustom3 {
                width: 18px;
height: 18px;
border-radius: 3px;
padding: 3px 4px;
border: 1px solid #768388;
box-shadow: 0 1px 0 0 rgba(255,255,255,0),inset 0 1px 0 rgba(255,255,255,0),inset 0 -1px 0 rgba(255,255,255,0);
color: #333;
cursor:pointer;
		    }
        .iconReload{background:url("Images/refresh-24.png") no-repeat;}
        .iconExcel{background:url("Images/excel-24.png") no-repeat;}
        .iconCancel{background:url("Images/cancel-24.png") no-repeat;}
        .gllogo{
        position:absolute;
        right:20px;
        top:20px;
        width:120px;
        height:80px;
        background-image: url(data:image/png;base64,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);
        }
        .dijitTextBox{
	        padding: 4px 5px;
	    }
        .Sleek.gl .dijitButton .dijitButtonNode{line-height: 20px;
padding: 2px 8px;
        }
        .epi-default a {
    text-decoration: underline;
}
	</style>
	<title>GlobalLink Dashboard</title>
	<script type="text/javascript">
		function CheckAll(obj, objId) {
			for (var i = 0; i < obj.form.elements.length; i++) {
			    if (obj.form.elements[i].name.indexOf(objId) != -1 &&
        obj.form.elements[i].type == 'checkbox' &&
        obj.form.elements[i].name != obj.name) {
					if (obj.checked) {
						obj.form.elements[i].checked = true;
					}
					else {
						obj.form.elements[i].checked = false;
					}
				}
			}
		}
		function CheckOther(obj, id) {
			var prefix = new String(obj.id);
			prefix = prefix.substring(0, prefix.indexOf('GridView1')) + 'GridView1_ctl01_' + id;
			if (document.getElementById(prefix))
				document.getElementById(prefix).checked = false;
		}
	</script>
</head>
<body class="Sleek gl">
    <div class="gllogo"></div>
	<form id="form1" runat="server" action="PageTranslation.aspx">
	<div class="pagespacing">
		<p>
		</p>
		<table width="155" cellspacing="0" cellpadding="0" style="margin-left: 7px">
			<tr>
				<th>
                    <asp:Button ID="btnNewProjectBtn" runat="server" CssClass="epi-iconPlus epi-icon--medium iconCustom"  onclick="btnNewProject_Click" ToolTip="New Project" />
				</th>
                <th>
                    <asp:Button ID="btnNewCommProjectBtn" runat="server" CssClass="epi-iconPlus epi-icon--medium iconCustom"  onclick="btnNewCommProject_Click" ToolTip="New Project" />
				</th>
				<th>
                    <asp:Button ID="btnRefreshBtn" runat="server" CssClass="iconReload iconCustom2" OnClick="btnRefresh_Click" ToolTip="Refresh" />
				</th>
				<th>
                    <asp:Button ID="btnExportDashboardBtn" CssClass="iconExcel iconCustom2" runat="server" onclick="btnExportDashboard_Click" ToolTip="Export All Active Submissions to csv" />
				</th>
				<th>
                    <asp:Button ID="btnCSVBtn" CssClass="iconExcel iconCustom2" runat="server" onclick="btnCSV_Click" ToolTip="Export History to CSV" />
				</th>
				<th>
                    <asp:Button ID="btnGetAllPagesBtn" CssClass="epi-iconTree epi-icon--medium iconCustom" runat="server" onclick="btnGetAllPages_Click" ToolTip="List all pages from this node" />
				</th>
			</tr>
		</table>
		<br />
		<asp:Panel ID="pnlGetcount" runat="server" Visible="true">
			<div>
				<asp:Menu ID="mymenuTabs" StaticMenuStyle-CssClass="dijitTabListContainer-top dijitTabContainerTop-tabs" StaticMenuItemStyle-CssClass="dijitTab dijitTabContent"
					StaticSelectedStyle-CssClass="dijitTabChecked" Orientation="Horizontal" OnMenuItemClick="menuTabs_MenuItemClick" runat="server" EnableTheming="false" IncludeStyleBlock="false" >
					<StaticSelectedStyle CssClass="dijitTab dijitTabChecked" />
					<StaticMenuItemStyle CssClass="dijitTab dijitTabContent" />
					<Items>
						<asp:MenuItem Value="0" Selected="True" ToolTip="Active Submissions" Text="Active Submissions" />
						<asp:MenuItem Text="Translation History" Value="1" ToolTip="Translation History" />
					</Items>
				</asp:Menu>
				<br />
				<asp:Panel ID="Panel1" runat="server" BorderStyle="None" Direction="LeftToRight"
					EnableTheming="False" HorizontalAlign="Left" Width="690px" Wrap="False" DefaultButton="filterGrid">
					<table cellspacing="2">
						<tr>
							<td align="left">
								<asp:Label ID="lblSearch" runat="server" Font-Bold="False" Font-Overline="False"
									Text="Search "></asp:Label>
							</td>
							<td>
								<asp:TextBox ID="txtPageName" runat="server" CssClass="dijitTextBox"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td align="left">
								<asp:Label ID="Label1" runat="server" Font-Bold="False" Text="Target Language "></asp:Label>
							</td>
							<td>
								<asp:DropDownList ID="lstSource" runat="server" CssClass="dijitTextBox">
									<asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr id="tdStatus" runat="server">
							<td align="left">
								<asp:Label ID="lblStatus" runat="server" Font-Bold="False" Text="Status "></asp:Label>
							</td>
							<td>
								<asp:DropDownList ID="ddlstatus"  CssClass="dijitTextBox" runat="server">
									<asp:ListItem Selected="True" Text="Select" Value="-1"></asp:ListItem>
									<asp:ListItem Selected="false" Text="WaitingToSend" Value="1"></asp:ListItem>
									<asp:ListItem Selected="false" Text="Sent" Value="4"></asp:ListItem>
									<asp:ListItem Selected="false" Text="ReadyForImport" Value="7"></asp:ListItem>
									<asp:ListItem Selected="false" Text="ToCancel" Value="13"></asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr id="tdHistoryStatus" runat="server">
							<td align="left">
								<asp:Label ID="Label2" runat="server" Font-Bold="False" Text="Status "></asp:Label>
							</td>
							<td>
								<asp:DropDownList ID="ddlHistoryStatus" CssClass="dijitTextBox" runat="server">
									<asp:ListItem Selected="True" Text="Select" Value="-1"></asp:ListItem>
									<asp:ListItem Selected="false" Text="Imported" Value="9"></asp:ListItem>
									<asp:ListItem Selected="false" Text="Published" Value="10"></asp:ListItem>
									<asp:ListItem Selected="false" Text="Cancelled" Value="14"></asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr id="tdPageType" runat="server">
							<td align="left">
								<asp:Label ID="lblPageType" runat="server" Font-Bold="False" Text="Content type "></asp:Label>
							</td>
							<td>
								<asp:DropDownList ID="ddlPageType" CssClass="dijitTextBox" runat="server">
									<asp:ListItem Selected="True" Text="Blocks/Pages" Value="1"></asp:ListItem>
									<asp:ListItem Selected="false" Text="Pages" Value="2"></asp:ListItem>
									<asp:ListItem Selected="false" Text="Blocks" Value="3"></asp:ListItem>
									<asp:ListItem Selected="false" Text="Nodes/Products" Value="4"></asp:ListItem>
									<asp:ListItem Selected="false" Text="Nodes" Value="5"></asp:ListItem>
									<asp:ListItem Selected="false" Text="Products" Value="6"></asp:ListItem>
									<asp:ListItem Selected="false" Text="All" Value="7"></asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>
                                <div class="dijitButton" style="float:left"><asp:Button ID="filterGrid" CssClass="dijitButtonNode" runat="server" Text="Filter" OnClick="btnFilter_Click" /></div>
								<div class="dijitButton" style="float:left"><asp:Button ID="clearValues1" CssClass="dijitButtonNode" runat="server" Text="Reset filters" OnClick="btnClear1_Click" /></div>
                                
							</td>
						</tr>
					</table>
				</asp:Panel>
				<table border="0">
					<tr>
						<th align="left" valign="middle" scope="col">
							<asp:MultiView ID="multiTabs" runat="server" ActiveViewIndex="0">
								<asp:View ID="view1" runat="server">
									<table border="0">
										<tr>
											<td align="right" style="text-align: right">
												Show per page
												<asp:DropDownList ID="gridSize1" runat="server" AutoPostBack="true" EnableTheming="false" OnSelectedIndexChanged="GridView1_sizeChanged"
													CssClass="empty dijitTextBox spp">
													<asp:ListItem Value="20" Selected="True">20</asp:ListItem>
													<asp:ListItem Value="50" Selected="False">50</asp:ListItem>
													<asp:ListItem Value="75" Selected="False">75</asp:ListItem>
													<asp:ListItem Value="100" Selected="False">100</asp:ListItem>
													<asp:ListItem Value="200" Selected="False">200</asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td>
												<asp:GridView ID="GridView1" runat="server" AllowSorting="True" OnSorting="GridView1_Sorting" AllowPaging="true"
													AutoGenerateColumns="False" CellPadding="2" DataKeyNames="pkid" EmptyDataRowStyle-CssClass="error"
													EmptyDataText="No pages found!" OnPageIndexChanging="GridView1_PageIndexChanging" OnDataBound="GridView1_DataBound1"
													CssClass="epi-default" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" GridLines="Horizontal" BorderWidth="1"
													PagerSettings-Mode="NumericFirstLast">
                                                    <HeaderStyle CssClass="epi-header" />
													<RowStyle CssClass="FM-ItemRow" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last"
														NextPageText="Next" PreviousPageText="Previous" />
													<PagerStyle ForeColor="Black" HorizontalAlign="Right" BackColor="#C6C3C6" CssClass="pager">
													</PagerStyle>
													<EmptyDataRowStyle CssClass="error" />
													<Columns>
                                                        <asp:TemplateField Visible="false">
															<ItemTemplate>
																<asp:Label ID="subgiud" runat="server" Text='<%# Eval("subgiud").ToString() %>'></asp:Label>
															</ItemTemplate>
														</asp:TemplateField>
														<asp:TemplateField HeaderText="Submission" SortExpression="subname">
															<ItemTemplate>
																<asp:Label ID="subname" runat="server" Text='<%# Eval("subname").ToString() %>'></asp:Label>
																<br />
																<asp:LinkButton Text="Cancel submission" AutoPostBack="true" Visible='<%# isAllowCancel() %>' ID="btnCancelSubmission" runat="server" OnClick="btn_cancelSubmissionClick" OnClientClick="return confirm('You are going to cancel all targets for this submission. Are you sure?');" />
															</ItemTemplate>
															<ItemStyle Wrap="False" />
														</asp:TemplateField>
														<asp:BoundField DataField="sourceLanguage" HeaderText="Source" ReadOnly="True" SortExpression="sourceLanguage">
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="createdDate" DataFormatString="{0:dd-MM-yyyy}"
															HeaderText="Date Created" HtmlEncode="false" ReadOnly="True" SortExpression="createdDate">
															<ItemStyle Wrap="false" />
														</asp:BoundField>
														<asp:BoundField DataField="dueDate" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Due Date"
															HtmlEncode="false" ReadOnly="True" SortExpression="dueDate">
															<ItemStyle Wrap="false" />
														</asp:BoundField>
														<asp:BoundField DataField="createdBy" HeaderText="Created by" ReadOnly="True" SortExpression="createdBy">
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="pdProjectName" HeaderText="PD Project" ReadOnly="True" SortExpression="pdProjectName" />
														<asp:BoundField DataField="pageLink" HeaderText="ContentID" SortExpression="pageLink" />
														<asp:BoundField DataField="pageType" HeaderText="Type" SortExpression="pageType" />
                                                        <asp:HyperLinkField Target="_blank" DataNavigateUrlFields="pageUrl" DataNavigateUrlFormatString="{0}" ItemStyle-Wrap="false" DataTextField="pageName" HeaderText="Content Name" SortExpression="pageName" />
														
														<asp:BoundField DataField="projectLanguage" HeaderText="Target" ReadOnly="True" SortExpression="targetLanguage">
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<asp:TemplateField HeaderText="Status" SortExpression="status">
															<ItemTemplate>
																<%# GetLocalStatus(Eval("status").ToString())%>
															</ItemTemplate>
															<ItemStyle Wrap="False" />
														</asp:TemplateField>
														<asp:TemplateField HeaderText="Cancel">
															<HeaderTemplate>
																<asp:CheckBox ID="chkAllCancel" runat="server" onclick="CheckAll(this, 'chkCancel')" Text="Cancel" />
															</HeaderTemplate>
															<ItemTemplate>
																<asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# GetLocalStatus2(Eval("status").ToString())%>'>
																	<asp:CheckBox ID="chkCancel" runat="server" onclick="CheckOther(this, 'chkAllCancel');" />
																</asp:PlaceHolder>
															</ItemTemplate>
															<ItemStyle Wrap="False" />
														</asp:TemplateField>
														<asp:TemplateField HeaderText="AutoPublish">
                                                            <HeaderTemplate>
																<asp:CheckBox ID="chkAllPublish" runat="server" onclick="CheckAll(this, 'chkAutoPublish')" Text="AutoPublish" />
															</HeaderTemplate>
															<ItemTemplate>
																<asp:CheckBox ID="chkAutoPublish" runat="server" Checked='<%# Convert.ToBoolean(Eval("autoPublishItem")) %>' onclick="CheckOther(this, 'chkAllPublish');" />
															</ItemTemplate>
															<ItemStyle Wrap="False" />
														</asp:TemplateField>
														
													</Columns>
												</asp:GridView>
											</td>
										</tr>
									</table>
									<table>
										<tr>
											<td>
												<%--    <!-- DataFormatString="{0:dd-MM-yyyy HH:MM:ss}"  HtmlEncode="false" -->      <asp:Button id="btnImport" CssClass="buttons"  runat="server" OnClick="btnImport_Click" Text="Import" /> <asp:Button id="btnPublished" runat="server" CssClass="buttons" Text="Publish" OnClick="btnPublished_Click" /> --%>
                                                <asp:Button ID="btnCancelBtn" runat="server" Text="Cancel selected" OnClick="btnCancel_Click" ToolTip="Cancel selected content" />
                                                <asp:Button ID="btnAutoPublishButton" runat="server" Text="Update AutoPublish" OnClick="btnAutoPublish_Click" ToolTip="Update autopublish" />
											</td>
										</tr>
									</table>
									<br />
									<br />
									&nbsp;<br />
									<br />
								</asp:View>
								<asp:View ID="view2" runat="server">
									<table>
										<tr>
											<td align="right" style="text-align: right">
												Show per page
												<asp:DropDownList ID="gridSize2" runat="server" AutoPostBack="true" EnableTheming="false" OnSelectedIndexChanged="GridView2_sizeChanged"
													CssClass="empty dijitTextBox spp">
													<asp:ListItem Value="20" Selected="True">20</asp:ListItem>
													<asp:ListItem Value="50" Selected="False">50</asp:ListItem>
													<asp:ListItem Value="75" Selected="False">75</asp:ListItem>
													<asp:ListItem Value="100" Selected="False">100</asp:ListItem>
													<asp:ListItem Value="200" Selected="False">200</asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td>
												<asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" AllowPaging="true"
													AllowSorting="true" CellPadding="3" EmptyDataRowStyle-CssClass="error" EmptyDataText="No pages found!"
													CssClass="epi-default" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting"
													PagerSettings-Mode="NumericFirstLast">
													<HeaderStyle CssClass="epi-header" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last"
														NextPageText="Next" PreviousPageText="Previous" />
													<PagerStyle ForeColor="Black" HorizontalAlign="Right" BackColor="#C6C3C6" CssClass="pager">
													</PagerStyle>
													<RowStyle CssClass="EP-tableCell" />
													<EmptyDataRowStyle CssClass="error" />
													<Columns>
														<asp:BoundField DataField="pageLink" HeaderText="ContentID" ReadOnly="True" SortExpression="pageLink" />
														<asp:BoundField DataField="pageType" HeaderText="Type" SortExpression="pageType" />
                                                        <asp:HyperLinkField Target="_blank" DataNavigateUrlFields="pageUrl" DataNavigateUrlFormatString="{0}" ItemStyle-Wrap="false" DataTextField="pageName" HeaderText="Content Name" SortExpression="pageName" />
														<asp:BoundField DataField="sourceLanguage" HeaderText="Source" ReadOnly="True" SortExpression="sourceLanguage" >
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<asp:TemplateField HeaderText="Target" SortExpression="targetLanguage">
															<ItemTemplate>
																<asp:Label ID="Label1" runat="server" Text='<%# Bind("projectLanguage") %>'></asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:Label ID="Label1" runat="server" Text='<%# Eval("projectLanguage") %>'></asp:Label>
															</EditItemTemplate>
															<ItemStyle Wrap="False" />
														</asp:TemplateField>
														<asp:BoundField DataField="sentDate" HeaderText="Sent" HtmlEncode="false" ReadOnly="True" SortExpression="sentDate">
															<ItemStyle Wrap="false" />
														</asp:BoundField>
														<asp:BoundField DataField="receivedDate" HeaderText="Imported" HtmlEncode="false"
															ReadOnly="True" SortExpression="receivedDate">
															<ItemStyle Wrap="false" />
														</asp:BoundField>
														<asp:BoundField DataField="importDate" HeaderText="Published" HtmlEncode="false"
															ReadOnly="True" SortExpression="importDate">
															<ItemStyle Wrap="false" />
														</asp:BoundField>
														<asp:BoundField DataField="cancelDate" HeaderText="Cancelled" HtmlEncode="False"
															ReadOnly="True" SortExpression="cancelDate">
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="createdBy" HeaderText="User" ReadOnly="True" SortExpression="createdBy">
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<asp:BoundField DataField="subname" HeaderText="Submission" ReadOnly="True" SortExpression="subname">
															<ItemStyle Wrap="False" />
														</asp:BoundField>
													</Columns>
												</asp:GridView>
											</td>
										</tr>
									</table>
								</asp:View>
							</asp:MultiView>
						</th>
						<th align="left" valign="middle">
							<span class="style8"></span>
						</th>
						<th align="left" valign="middle">
							&nbsp;
						</th>
					</tr>
				</table>
			</div>
		</asp:Panel>
		<asp:HiddenField ID="hiddenpagelink" runat="server" />
	</div>
	</form>
</body>
</html>
