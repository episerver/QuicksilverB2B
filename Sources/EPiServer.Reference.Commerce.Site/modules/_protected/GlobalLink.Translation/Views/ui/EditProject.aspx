<%@ Page Language="C#" EnableViewState="true" EnableViewStateMac="true" AutoEventWireup="true"
	MaintainScrollPositionOnPostback="true" CodeBehind="EditProject.aspx.cs" Inherits="GlobalLink.Translation.EditProject" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Translation wizard</title>
    <link rel="stylesheet" type="text/css" href="<%= EPiServer.Shell.Paths.ToShellClientResource("ClientResources/epi/themes/sleek/sleek.css")  %>"/>
	<script type="text/javascript">
		function selectbox(selected) {
			var selectbox = document.forms[0].elements['files'];

			for (var i = 0; i < selectbox.length; i++)
				selectbox[i].checked = selected;
		}

		function popupCalendar() {
			var dateField = document.getElementById('dateField');
			// toggle the div

			if (dateField.style.display == 'none')
				dateField.style.display = 'block';
			else
				dateField.style.display = 'none';
		}
		function isMaxLength(obj) {
			if (obj.value.length > 249) {
				obj.value = obj.value.substring(0, 249);
				return false;
			} else
				return obj.value.length <= 249;
		}
		function refresh() {
			//window.opener.__doPostBack('GridView1', '');
			window.close();
		}
	</script>
	<style type="text/css">
		body, th, table
		{
			font-family: Verdana,Arial,Helvetica,sans-serif;
			font-size: 1em;
		}
		.aspNetDisabled
		{
			color:#999;
		}
		.EP-tableHeading
		{
			white-space: nowrap;
		}
		.calend
		{
			position: absolute;
		}
		.calend2
		{
			position: static;
		}
		div.epi-buttonContainer, div.epi-buttonContainer-simple
		{
			margin: 0 0 1.1em;
			padding: 0;
			text-align: right;
		}
		div.epi-buttonContainer
		{
			padding: 1em 0 0;
		}
		.epi-cmsButton, .epi-cmsButton img, .epi-cmsButton a img, .epi-cmsButton input[type="image"], .epi-cmsButton img + input, .epi-cmsButton img + input[value], .epi-cmsButton input
		{
			cursor: pointer;
		}
		.epi-cmsButton, .epi-cmsButtondisabled
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
		}
		.finish
		{
			text-align:center;
		}
		.red{color:Red}
	    .dijitTextBox{
	        padding: 4px 5px;
	    }
        select.dijitTextBox{
	        padding: 4px 0px; width:212px;
	    }
        .gllogo{
        position:absolute;
        right:20px;
        top:20px;
        width:120px;
        height:80px;
        background-image: url(data:image/png;base64,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);
        }
        .Sleek.gl .dijitButton .dijitButtonNode{line-height: 20px;
padding: 0px 6px;
        }
        .Sleek.gl .dijitButton .dijitButtonNode.aspNetDisabled{color:#999
        }
	    .dijitButton.dijitButton2 {
	        margin: 0;
	    }
        .dijitButton.dijitButton3 {
	        margin: 0;
            display: inline-block;
	    }
	    .checks input[type="checkbox"] {
            margin-left:0;
	    }
	</style>
</head>
<body class="Sleek gl">
    
	<form id="form1" runat="server">
	<div>
		<table width="100%" height="386" border="0" align="left" cellpadding="0" cellspacing="0">
			<tr>
				<td width="100%" height="386" align="center" valign="top">
					<br />
					<asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" Font-Names="Verdana"
						Font-Size="0.8em" DisplaySideBar="False" OnActiveStepChanged="StepChanged" OnFinishButtonClick="WizardFinished">
						<WizardSteps>
							<asp:WizardStep ID="ProjectSetting" runat="server" Title="Select source language">
                                <div class="gllogo"></div>
								<table width="100%" cellspacing="0" cellpadding="1">
									<tr>
										<th height="20" align="left" valign="middle" class="style8" scope="col">
											<strong>Submission Name</strong>
										</th>
										<th class="style8" scope="col">
											
										</th>
										<th align="left" valign="middle" class="style8" scope="col">
											<asp:TextBox ID="submissionName" CssClass="dijitTextBox" runat="server" MaxLength="45"></asp:TextBox><asp:RequiredFieldValidator
												ForeColor="Red" ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
												ControlToValidate="submissionName"></asp:RequiredFieldValidator>
										</th>
									</tr>
									<tr>
										<th height="20" align="left" valign="middle" class="style8" scope="col">
											<strong>User Name</strong>
										</th>
										<th class="style8" scope="col">
											
										</th>
										<th align="left" valign="middle" class="style8" scope="col">
											<asp:TextBox ID="txtUserName" CssClass="dijitTextBox" runat="server" MaxLength="45"></asp:TextBox><asp:RequiredFieldValidator
												ForeColor="Red" ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
												ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
										</th>
									</tr>
									<tr>
										<th align="left" class="style8" height="20" scope="col" valign="middle">
											<strong>Source language</strong>
										</th>
										<th class="style8" scope="col">
											
										</th>
										<th align="left" class="style8" scope="col" valign="middle">
											<asp:DropDownList CssClass="dijitTextBox" ID="lstSourceLanguage" runat="server" OnSelectedIndexChanged="sourceChanged"
												AutoPostBack="true">
											</asp:DropDownList>
										</th>
									</tr>
									<tr>
										<th align="left" class="style8" height="20" scope="col" valign="middle">
											<strong>Project</strong>
										</th>
										<th class="style8" scope="col">
											
										</th>
										<th align="left" class="style8" scope="col" valign="middle">
											<asp:DropDownList ID="projectsList" runat="server" CssClass="dijitTextBox" OnSelectedIndexChanged="projectChanged" AutoPostBack="true">
											</asp:DropDownList>
										</th>
									</tr>
									<tr>
										<th align="left" class="style8" height="20" scope="col" valign="middle">
											<strong>Due Date</strong>
										</th>
										<th class="style8" scope="col">
											
										</th>
										<th align="left" class="style8" scope="col" valign="middle">
											<asp:TextBox ID="txtDate" CssClass="dijitTextBox" runat="server" ReadOnly="true">
											</asp:TextBox><div class="dijitButton dijitButton3"><input type="button" class="dijitButtonNode" id="calButton" runat="server" value="..." onclick="popupCalendar()" /></div>
											<div id="dateField" style="display: none; position: absolute">
												<asp:Calendar ID="calDate" runat="server" CellPadding="4" BorderColor="#999999" Font-Names="Verdana"
													Font-Size="8pt" BackColor="#CCCCCC" Height="180px" ForeColor="Black" DayNameFormat="FirstLetter"
													OnDayRender="DayRender" OnVisibleMonthChanged="KeepCalendarVisible" OnSelectionChanged="calDate_SelectionChanged">
													<TodayDayStyle ForeColor="Black" BackColor="#CCCCCC"></TodayDayStyle>
													<SelectorStyle BackColor="#CCCCCC"></SelectorStyle>
													<NextPrevStyle VerticalAlign="Bottom"></NextPrevStyle>
													<DayHeaderStyle Font-Size="7pt" Font-Bold="True" BackColor="#CCCCCC"></DayHeaderStyle>
													<SelectedDayStyle Font-Bold="True" ForeColor="White" BackColor="#666666"></SelectedDayStyle>
													<TitleStyle Font-Bold="True" BorderColor="Black" BackColor="#999999"></TitleStyle>
													<WeekendDayStyle BackColor="LightSteelBlue"></WeekendDayStyle>
													<OtherMonthDayStyle ForeColor="#808080"></OtherMonthDayStyle>
												</asp:Calendar>
											</div>
											<asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator2" runat="server"
												ErrorMessage="*" ControlToValidate="txtDate"></asp:RequiredFieldValidator>
										</th>
									</tr>
								</table>
								<table width="100%" cellspacing="0" cellpadding="0" border="0">
									<tr>
										<th width="200" height="24" align="left" valign="middle">
											Available languages:
										</th>
										<th>
											&nbsp;
										</th>
										<th width="200" align="left" valign="middle">
											Target languages:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" runat="server"
												ErrorMessage="*" ControlToValidate="lstTarget"></asp:RequiredFieldValidator>&nbsp;
										</th>
									</tr>
									<tr>
										<th align="left" class="style8" height="99" scope="col" valign="middle">
											<asp:ListBox ID="lstSource" runat="server" CssClass="dijitTextBox" Height="100px" SelectionMode="Multiple"></asp:ListBox>
										</th>
										<th align="center" class="style8" scope="col" valign="middle">
											<table align="center">
												<tr>
													<td align="left">
														<div class="dijitButton dijitButton2"><asp:Button runat="server" Width="30" CssClass="dijitButtonNode" ID="btnSelectsingletarget"  OnClick="btnSelectsingletarget_Click" CausesValidation="false" Text=">" /></div>
													</td>
												</tr>
												<tr>
													<td align="left">
														<div class="dijitButton dijitButton2"><asp:Button runat="server" Width="30" CssClass="dijitButtonNode" ID="btnRemovesingletarget"  OnClick="btnRemovesingletarget_Click" CausesValidation="false" Text="<" /></div>
													</td>
												</tr>
												<tr>
													<td align="left">
														<div class="dijitButton dijitButton2"><asp:Button runat="server" Width="30" CssClass="dijitButtonNode" ID="btnSelecttarget"  OnClick="btnSelecttarget_Click" CausesValidation="false" Text=">>" /></div>
													</td>
												</tr>
												<tr>
													<td align="left">
														<div class="dijitButton dijitButton2"><asp:Button runat="server" Width="30" CssClass="dijitButtonNode" ID="btnRemovetarget"  OnClick="btnRemovetarget_Click" CausesValidation="false" Text="<<" /></div>
													</td>
												</tr>
											</table>
										</th>
										<th align="left" class="style8" scope="col" valign="middle">
											<asp:ListBox ID="lstTarget" runat="server" CausesValidation="True" CssClass="dijitTextBox"
												Height="100px" SelectionMode="Multiple"></asp:ListBox>
										</th>
									</tr>
									<tr>
										<th align="left" height="24" valign="middle" colspan="3">
											Translation Instructions
										</th>
									</tr>
									<tr>
										<th align="left" valign="middle" colspan="3">
											<asp:TextBox CssClass="dijitTextBox dijitTextArea" runat="server" MaxLength="254" Width="461" ID='transInstruction' Rows="3"
												TextMode="MultiLine"></asp:TextBox><br />
										</th>
									</tr>
								</table>
								<table width="100%" height="85" cellpadding="0" cellspacing="0">
									<tr>
										<th align="left" valign="middle" scope="col">
											<span class="checks">
												<asp:CheckBox runat="server" ID='includeChildren' Text="Include child nodes" OnCheckedChanged="includeChildren_changed" /><br />
												<asp:CheckBox runat="server" ID='includeBlocks' Text="Include referenced content" OnCheckedChanged="includeBlocks_changed" /><br />
                                                <asp:CheckBox runat="server" ID='isSkipContentArea' Text="Disable copying contentArea references" /><br />
												<asp:CheckBox runat="server" ID='CheckDetection' Text="Exclude pages Unchanged since last translation" /><br />
												<asp:CheckBox runat="server" ID='excludeExpired' Text="Exclude expired pages" /><br />
												<asp:CheckBox runat="server" ID='AutoPublish' Text="Auto-publish translated content on completion " /><br />
                                                <asp:CheckBox runat="server" ID='isCancelExisting' Text="Cancel already sent targets if resending" /><br />
												
												<asp:Panel ID="Panel2" Visible="false" runat="server">
													<table>
														<tr>
															<th align="left" valign="middle" class="style8" scope="col">
																&nbsp;
															</th>
															<th height="20" align="left" valign="middle" class="style8" scope="col">
																<strong>From</strong>
															</th>
															<th class="style8" scope="col">
																:
															</th>
															<th align="left" valign="middle" class="style8" scope="col">
																<asp:TextBox ID="txtStart" ReadOnly="true" runat="server"></asp:TextBox>
															</th>
															<th height="20" align="left" valign="middle" class="style8" scope="col">
																<strong>To</strong>
															</th>
															<th class="style8" scope="col">
																:
															</th>
															<th align="left" valign="middle" class="style8" scope="col">
																<asp:TextBox ID="txtEnd" ReadOnly="true" runat="server"></asp:TextBox>
															</th>
														</tr>
													</table>
												</asp:Panel>
											</span>
										</th>
									</tr>
								</table>
								<div style="float:right"><asp:CheckBox ID="skipSecondStep" runat="server" Text="Skip summary step" /></div>
							</asp:WizardStep>
							<asp:WizardStep ID="ProjectPages" runat="server" Title="Modify pages">
								<table width="600" cellspacing="0" border="0" cellpadding="0">
									<tr>
										<td valign="top" align="left">
											<asp:Repeater runat="server" ID="pagesToTranslate">
												<HeaderTemplate>
													<table width="100%" cellspacing="0" border="0" cellpadding="0">
														<tr height="22px" class="EP-tableHeading">
															<td style="white-space: nowrap;">
																<strong>&nbsp;&nbsp;Content</strong>
															</td>
															<td>
																&nbsp;
															</td>
														</tr>
												</HeaderTemplate>
												<ItemTemplate>
													<tr height="22px" style="font-weight: bold; font-size: 11px; background-color: <%# GetRowColor() %>;">
														<td style="white-space: nowrap;" align="left">
															<img id="Img1" runat="server" src="Images/blank.gif" width='<%#int.Parse(Eval("indent")+"0")%>'
																height="0" />
															<asp:CheckBox ID='chkPage' CssClass='<%#Eval("pageLink") %>' runat="server" AutoPostBack="true" OnCheckedChanged="changePageStatus" Checked='<%# Eval("status").ToString().Equals("2") %>' />
															<%#Eval("displayName") %>
														</td>
														<td style="white-space: nowrap;" align="right" width="70">
														</td>
														<td>
															&nbsp;
														</td>
													</tr>
												</ItemTemplate>
												<FooterTemplate>
													</table></FooterTemplate>
											</asp:Repeater>
										</td>
										<td valign="TOP">
											<asp:Panel ID="Panel1" runat="server" Visible="true">
											</asp:Panel>
										</td>
									</tr>
								</table>
                                <p style="width:600px">
                                    <asp:Label runat="server" ID="disableChkLbl" CssClass="red">* some checkboxes are disabled because this target language is not enabled for this content</asp:Label>
                                </p>
								<p style="width:600px">
									<asp:Label ID="existingLbl" runat="server" CssClass="red" /></p>
							</asp:WizardStep>
							<asp:WizardStep ID='ProjectFiles' runat="server" StepType="Complete" Title='Choose files'>
							<div class="finish">
								<strong>
									<asp:Label ID="Label3" runat="server" /></strong>
								<br /><br />
									<input type="button" value='Close window' onclick='refresh()' class="epi-cmsButton" />
								</div>
							</asp:WizardStep>
						</WizardSteps>
						<StartNavigationTemplate>
							<div class="epi-buttonContainer dijitButton">
								<asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Next"
									CssClass="dijitButtonNode" />
							</div>
						</StartNavigationTemplate>
						<FinishNavigationTemplate>
							<div class="epi-buttonContainer dijitButton">
									<asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
										Text="Previous" CssClass="dijitButtonNode" />
									<asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" Text="Finish"
										CssClass="dijitButtonNode" />
							</div>
						</FinishNavigationTemplate>
						<HeaderStyle VerticalAlign="Top" HorizontalAlign="Left"></HeaderStyle>
					</asp:Wizard>
				</td>
			</tr>
		</table>
	</div>
	</form>
	<navigationstyle verticalalign="Top" horizontalalign="Left" />
	<sidebarstyle verticalalign="Top" horizontalalign="Left" />
</body>
</html>
