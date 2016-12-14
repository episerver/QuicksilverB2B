<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" EnableViewStateMac="true"
	CodeBehind="Configuration.aspx.cs" Inherits="GlobalLink.Translation.ui.Configuration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>GlobalLink adaptor configuration</title>
	<style type="text/css">
		html, body, div, span, applet, object, iframe, p, blockquote, pre, a, abbr, acronym, address, big, cite, code, del, dfn, em, img, ins, kbd, q, s, samp, small, strike, strong, tt, var, dl, dt, dd, ol, ul, li, fieldset, form, label, legend, caption, tbody, tfoot, thead
		{
			border: 0 none;
			font-family: inherit;
			font-size: 100%;
			font-style: inherit;
			font-weight: inherit;
			margin: 0;
			padding: 0;
			vertical-align: baseline;
		}
		textarea, input, select, option, optgroup
		{
			font: 1em "Lucida Grande" , "Lucida Sans Unicode" ,Arial,Verdana,Sans-Serif;
		}
		textarea, input, select
		{
			margin: 0;
			padding: 0;
		}
		thead, tfoot, tbody
		{
			margin: 0;
			padding: 0;
		}
		a:link, a:visited
		{
			color: #FFFFFF;
			text-decoration: none;
		}
		body
		{
			font: 75%/1.231 "Lucida Grande" , "Lucida Sans Unicode" ,Arial,Verdana,Sans-Serif;
			text-align: left;
			color: #000000;
		}
		.epi-contentContainer
		{
			max-width: 65em;
			min-width: 45em;
			overflow: visible;
			position: relative;
		}
		.epi-padding
		{
			padding: 1.5em;
		}
		h1
		{
			font-family: Helvetica,Arial,Sans-Serif;
			font-size: 1.7em;
			font-weight: normal;
			line-height: 1.3;
		}
		.EP-prefix
		{
			position: relative;
		}
		.epi-contentContainer h1
		{
			border-bottom: 1px solid #BEBEBE;
			color: #333333;
			margin-bottom: 0.6em;
			padding-bottom: 0.3em;
		}
		.epi-tabView
		{
			overflow: hidden;
			padding-top: 0.4em;
			width: 100%;
		}
		ol, ul
		{
			list-style: none outside none;
		}
		ul.epi-tabView-navigation
		{
			background: url("Images/tab_bottomBorder.gif") repeat-x scroll left bottom transparent;
			list-style-type: none;
			overflow: hidden;
			padding: 0 0 0 0.7em;
		}
		li.epi-tabView-navigation-item a.epi-tabView-tab, li.epi-tabView-navigation-item-selected, li.epi-tabView-navigation-item-disabled
		{
			-moz-border-radius-topleft: 3px;
			-moz-border-radius-topright: 3px;
			background: url("Images/Gradients.png") repeat-x scroll left -4200px transparent;
			border: 1px solid #1C1C1C;
			float: left;
			height: 1.5em;
			margin-right: 0.2em;
			padding: 0.5em 0.6em 0;
			position: relative;
			text-shadow: 0 1px 0 #222222;
			white-space: nowrap !important;
		}
		li.epi-tabView-navigation-item a.epi-tabView-tab, li.epi-tabView-navigation-item-selected
		{
			background: url("Images/Gradients.png") repeat-x scroll left -2300px #DCDCDC;
			border: 1px solid #4D4D4D;
			color: #1D1D1D;
			text-shadow: 0 1px 0 #F8F8F8;
		}
		li.epi-tabView-navigation-item-selected
		{
			background: url("Images/Gradients.png") repeat-x scroll left -5200px #F8F8F8;
			border-bottom-color: #C7C7C7;
			color: #000000;
			text-shadow: 0 1px 0 #FFFFFF;
			display: inline-block;
		}
		li.epi-tabView-navigation-item-selected a, li.epi-tabView-navigation-item-selected a:visited, li.epi-tabView-navigation-item-selected a:active
		{
			color: #1D1D1D;
		}
		li.epi-tabView-navigation-item-selected a.epi-tabView-tab
		{
			font-weight: bold;
		}
		li.epi-tabView-navigation-item, li.epi-tabView-navigation-item-disabled
		{
			display: inline;
		}
		.epi-formArea div.epi-size25, .epi-formArea div.epi-size25 div
		{
			margin: 0.5em 0;
			min-height:18px;
		}
		.epi-formArea div.epi-size25 label, .epi-formArea label.epi-size25
		{
			display: inline-block;
			margin-right: 0.5em;
			vertical-align: top;
			width: 10em;
		}
		.epi-formArea div.epi-size25 label, .epi-formArea label.epi-size25
		{
			width: 25em;
			float:left;
		}
		.epi-formArea div.epi-size25.epi-size25-1 input
		{
			float:left;
		}
		select, textarea, input[type="text"], input[type="password"]
		{
			background-color: #FFFFFF;
			border: 1px solid #B4B4B4;
			color: #000000;
		}
		div.epi-buttonContainer, div.epi-buttonContainer-simple
		{
			margin: 0 0 1.1em;
			padding: 0;
			text-align: right;
		}
		div.epi-buttonContainer
		{
			border-top: 1px solid #BEBEBE;
			padding: 1em 0 0;
		}
		span.epi-cmsButton, span.epitoolbutton, span.epi-cmsButton img, span.epitoolbutton img, span.epitoolbuttonmousedown img, span.epi-cmsButton a img, span.epitoolbutton a img, span.epitoolbutton a img, span.epitoolbuttonmousedown a img, span.epitoolbutton input, span.epi-cmsButton input[type="image"], span.epitoolbutton input[type="image"], span.epi-cmsButton input[type="image"], span.epitoolbutton input[type="image"], span.epi-cmsButton img + input, span.epitoolbutton img + input, span.epi-cmsButton img + input[value], span.epitoolbutton img + input[value], span.epitoolbuttonmousedown, .epi-cmsButton input, .epi-cmsButtonmousedown input
		{
			cursor: pointer;
		}
		input.epi-cmsButton-tools, input.epi-cmsButton-imageEditor
		{
			background-color: transparent;
			background-repeat: no-repeat;
			border: 0 none;
			height: 18px;
			margin: 0;
			overflow: visible;
			padding: 0;
			vertical-align: middle;
			width: 18px;
		}
		span.epi-cmsButton, span.epi-cmsButtondisabled
		{
			background-image: url("Images/ToolButtonBg.gif");
			background-position: left top;
			background-repeat: repeat-x;
			border: 1px solid #8D8D8D;
			display: inline-block;
			margin-right: 5px;
			overflow: visible;
			padding: 2px;
			white-space: nowrap;
		}
		input.epi-cmsButton-tools
		{
			background-image: url("Images/SpriteTools.png");
		}
		input.epi-cmsButton-text
		{
			min-width: 20px;
			padding-left: 20px;
			width: auto;
		}
		.epi-cmsButton-Delete
		{
			background-position: 0 -2478px;
		}
		.epi-cmsButton-Save
		{
			background-position: 0 -6842px;
		}
		.epi-cmsButton-Add
		{
			background-position: 0 0;
		}
		.epi-cmsButton-Download
		{
			background-position: 0 -2949px;
		}
		.epi-cmsButton-Clean
		{
			background-position: 0 -2477px;
		}
		.epi-contentArea table, table.epi-default, table.epi-simple, table.epi-simpleWrapped
		{
			border: medium none;
			border-collapse: collapse;
			margin-bottom: 1em;
			width: 100%;
		}
		.epi-contentArea table, table.epi-default, table.epi-simple, table.epi-simpleWrapped
		{
			background-color: #FFFFFF;
		}
		table.epi-default
		{
			border: 1px solid #BEBEBE !important;
		}
		caption, th, td
		{
			font-weight: normal;
			text-align: left;
		}
		td
		{
			vertical-align: top;
		}
		.epi-contentArea th, table.epi-default th, table.epi-simple th, table.epi-simpleWrapped th
		{
			background: url("Images/Gradients.png") repeat-x scroll left -200px #BEBEBE;
			border-color: #333333;
			border-style: solid;
			border-width: 1px 1px 1px 0;
			color: #000000;
			font-weight: normal;
			padding: 0.1em 0.4em;
			text-shadow: 0 1px 0 #ABABAB;
		}
		.epi-contentArea th, table.epi-default th, table.epi-simple th, table.epi-simpleWrapped th
		{
			background: url("Images/Gradients.png") repeat-x scroll left -2200px #F1F1F1;
			border-color: #AEAEAE;
			color: #000000;
			text-shadow: 0 1px 0 #FFFFFF;
		}
		.epi-contentArea td, table.epi-default td, table.epi-simple td, table.epi-simpleWrapped td
		{
			border-bottom: 1px solid #666666;
			padding: 0.3em 0.4em;
		}
		.epi-contentArea td, table.epi-default td, table.epi-simple td, table.epi-simpleWrapped td
		{
			border-color: #BEBEBE;
		}
		.tableTdCentered td, .centered
		{
			text-align: center;
		}
		select
		{
			border: 1px solid #7F9DB9;
			font-size: 12px;
			padding: 1px 0;
		}
		.episize240
		{
			border: 1px solid #7F9DB9;
			width: 240px;
		}
		select.episize240
		{
			min-width: 242px;
		}
		span.episize240
		{
			border: 0 none;
			display: inline-block;
		}
		textarea, input, select
		{
			margin: 0;
			padding: 0;
		}
		.epi-formArea input[type="radio"], .epi-formArea input[type="checkbox"]
		{
			margin-right: 0.3em;
		}
		.epi-formArea input[type="checkbox"]
		{
			display: inline;
			width: auto;
		}
		.lblStatus
        {
            color: Green;
        }
        .lblError
        {
            color: red;
        }
        .allLeft div
        {
            float: left;
        }
        .allLeft
        {
            height: 25px;
        }
        .required
        {
            padding-right: 10px;
            width: 5px;
            color: Red;
        }
        .alertMsg
        {
            padding-left: 10px;
            color:Red;
            display:block;
        }
        .invis
        {
            display:none;
        }
        .left
        {
        	float:left;
        }
        .status
        {
			font-size: 13px;
			padding: 4px;
		}
		.statusOk
		{
			color:Green;
		}
		.statusNotOk
		{
			color:Red;
		}
		#dateField td
		{
			font-size:8pt;
			padding:1px;
		}
		#dateField td a
		{
			font-weight:bold
		}
		#dateField .calHead
		{
			padding:4px;
		}
		.roleButtons
		{
			text-align:center;
			margin:0 auto;
			vertical-align:middle;
		}
		.table{display:table}
		.table > div{display:table-row; height:25px}
.table > div > * {display:table-cell}
.table input{margin-top:2px}
.table #cleanDateBtn{height:19px; margin:1px 2px; line-height:12px}
hr {
    display: block;
    margin: 15px 0;
}
	</style>
	<script type="text/javascript">
		function confirmDelete(obj) {
			var anychecked = false;
			for (var i = 0; i < obj.form.elements.length; i++) {
				if (obj.form.elements[i].name.indexOf('chkDelete') != -1 &&
					obj.form.elements[i].type == 'checkbox') {
					if (obj.form.elements[i].checked) {
						anychecked = true;
					}
				}
			}
			if (anychecked) {
				if (confirm("If you delete language from language map, you will not be able to receive completed targets from GL with deleted target language. Are you sure you want to delete selected languages?")) {
					return true;
				} else {
					return false;
				}
			} else {
				alert("Please check items you would like to delete");
				return false;
			}
		}
		function confirmCleanDB(obj) {
		    var anychecked = false;

		    for (var i = 0; i < obj.form.elements.length; i++) {
		        if (obj.form.elements[i].name.indexOf('cleanType') != -1 &&
					obj.form.elements[i].type == 'radio') {
		            if (obj.form.elements[i].checked) {
		                var id = obj.form.elements[i].id;
		                var message = "";
		                if (id == 'cleanType1') {
		                    message = "Deleting ALL submissions (active and history). Are you sure?";
		                } else if (id == 'cleanType2') {
		                    message = "Deleting all active submissions (keeping history). Are you sure?";
		                } else if (id == 'cleanTypeHistory') {
		                    message = "Deleting all history submissions (keeping active). Are you sure?";
		                } else if (id == 'cleanType3') {
		                    message = "Deleting all submissions created after " + obj.form.elements.cleanDate.value + " (newest). Are you sure?";
		                } else if (id == 'cleanTypeOld') {
		                    message = "Deleting all submissions created before " + obj.form.elements.cleanOldDate.value + " (oldest). Are you sure?";
		                } else if (id == 'cleanType4') {
		                    message = "Deleting project with id " + obj.form.elements.cleanProjectId.value + ". Are you sure?";
		                } else if (id == 'cleanType5') {
		                    message = "Deleting item with id " + obj.form.elements.cleanItemId.value + ". Are you sure?";
		                }
		                if (confirm(message)) {
		                    return true;
		                } else {
		                    return false;
		                }
		            }
		        }
		    }
		    return false;
		}

		function CheckAllDelete(obj) {
			for (var i = 0; i < obj.form.elements.length; i++) {
				if (obj.form.elements[i].name.indexOf('chkDelete') != -1 &&
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
		function switchOnLoad() {
			var id = document.getElementById('curpage').value;
			switchTab(id);
		}
		function switchTab(id) {
			document.getElementById('curpage').value = id;
			document.getElementById('actionTab1').setAttribute("class", "epi-tabView-navigation-item");
			document.getElementById('actionTab2').setAttribute("class", "epi-tabView-navigation-item");
			document.getElementById('actionTab3').setAttribute("class", "epi-tabView-navigation-item");
			document.getElementById('actionTab4').setAttribute("class", "epi-tabView-navigation-item");
			document.getElementById('actionTab6').setAttribute("class", "epi-tabView-navigation-item");
			document.getElementById('actionTab7').setAttribute("class", "epi-tabView-navigation-item");
			document.getElementById('actionTab8').setAttribute("class", "epi-tabView-navigation-item");
			document.getElementById('actionTab9').setAttribute("class", "epi-tabView-navigation-item");
			if(document.getElementById('actionTab5'))
				document.getElementById('actionTab5').setAttribute("class", "epi-tabView-navigation-item");
			document.getElementById('actionTab' + id).setAttribute("class", "epi-tabView-navigation-item-selected");
			document.getElementById('view1').style.display = "none";
			document.getElementById('view2').style.display = "none";
			document.getElementById('view3').style.display = "none";
			document.getElementById('view4').style.display = "none";
			document.getElementById('view5').style.display = "none";
			document.getElementById('view6').style.display = "none";
			document.getElementById('view7').style.display = "none";
			document.getElementById('view8').style.display = "none";
			document.getElementById('view9').style.display = "none";
			document.getElementById('view' + id).style.display = "block";
		}
		function popupCalendar() {
			var dateField = document.getElementById('dateField');

			if (dateField.style.display == 'none')
				dateField.style.display = 'block';
			else
				dateField.style.display = 'none';
		}
		function popupCalendar2() {
			var dateField = document.getElementById('dateField2');

			if (dateField.style.display == 'none')
				dateField.style.display = 'block';
			else
				dateField.style.display = 'none';
		}
		function popupCalendar3() {
		    var dateField = document.getElementById('cleanOldDateCal');

		    if (dateField.style.display == 'none')
		        dateField.style.display = 'block';
		    else
		        dateField.style.display = 'none';
		}
	</script>
</head>
<body onload="switchOnLoad()">
	<form id="form1" runat="server">
	<div class="epi-contentContainer epi-padding">
		<h1 class="EP-prefix">
			GlobalLink adaptor configuration</h1>
		<div class="epi-tabView">
			<ul class="epi-tabView-navigation">
				<li id="actionTab1" class="epi-tabView-navigation-item-selected" onclick="switchTab(1, this)">
					<a class="epi-tabView-tab" target="_self" href="javascript:void(0)">GlobalLink Settings</a>
				</li>
				<li id="actionTab2" class="epi-tabView-navigation-item" onclick="switchTab(2, this)">
					<a class="epi-tabView-tab" target="_self" href="javascript:void(0)">Locale mapping</a>
				</li>
				<li id="actionTab3" class="epi-tabView-navigation-item" onclick="switchTab(3, this)"
					runat="server"><a class="epi-tabView-tab" target="_self" href="javascript:void(0)">Other
						settings</a> </li>
				<li id="actionTab4" class="epi-tabView-navigation-item" onclick="switchTab(4, this)"
					runat="server"><a class="epi-tabView-tab" target="_self" href="javascript:void(0)">Field configuration</a> </li>
				<li id="actionTab5" class="epi-tabView-navigation-item" onclick="switchTab(5, this)"
					runat="server"><a class="epi-tabView-tab" target="_self" href="javascript:void(0)">Commerce</a> </li>
				<li id="actionTab6" class="epi-tabView-navigation-item" onclick="switchTab(6, this)"
					runat="server"><a class="epi-tabView-tab" target="_self" href="javascript:void(0)">Database Cleanup</a> </li>
				<li id="actionTab7" class="epi-tabView-navigation-item" onclick="switchTab(7, this)"
					runat="server"><a class="epi-tabView-tab" target="_self" href="javascript:void(0)">Notification</a> </li>
				<li id="actionTab8" class="epi-tabView-navigation-item" onclick="switchTab(8, this)"
					runat="server"><a class="epi-tabView-tab" target="_self" href="javascript:void(0)">Security</a> </li>
                <li id="actionTab9" class="epi-tabView-navigation-item" onclick="switchTab(9, this)"
					runat="server"><a class="epi-tabView-tab" target="_self" href="javascript:void(0)">Wizard settings</a> </li>
			</ul>
		</div>
		<div style="display: none">
			<asp:TextBox ID="curpage" Text="1" runat="server"></asp:TextBox>
		</div>
		<div id="view1" style="display: block">
			<div class="epi-contentContainer">
				<div class="epi-padding">
					<asp:Label ID="lblStatus1" runat="server" CssClass="lblStatus" Text=""></asp:Label>
					<asp:Label ID="lblErrorStatus1" runat="server" CssClass="lblError" Text=""></asp:Label>
					<div class="epi-formArea" style="display: block;">
						<div class="epi-size25">
							 <div class="allLeft">
                                <div>
                                    <label for="txtURL">
                                        GlobalLink URL</label></div>
                                <div class="required">
                                    *
                                </div>
                                <div>
                                    <asp:TextBox ID="txtURL" runat="server" CssClass="episize240"></asp:TextBox>
                                </div>
                            </div>
							<div class="allLeft">
                                <div>
                                    <label for="txtUsername">
                                        User Id</label></div>
                                <div class="required">
                                    *</div>
                                <div>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="episize240"></asp:TextBox></div>
                            </div>
							<div class="allLeft">
                                <div>
                                    <label for="txtPassword">
                                        Password</label></div>
                                <div class="required">
                                    *</div>
                                <div>
                                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="episize240"></asp:TextBox></div>
                            </div>
							<div class="allLeft">
                                <div>
                                    <label for="txtPrefix">
                                        Submission Name Prefix</label></div>
                                <div class="required">
                                    &nbsp;</div>
                                <div>
                                    <asp:TextBox ID="txtPrefix" runat="server" CssClass="episize240"></asp:TextBox></div>
                            </div>
                            <div class="allLeft">
                                <div>
                                    <label for="txtFileFormat">
                                        Classifier (File format)</label></div>
                                <div class="required">
                                    *</div>
                                <div>
                                    <asp:TextBox ID="txtFileFormat" runat="server" CssClass="episize240 left"></asp:TextBox>
								</div>
								<div>
									<asp:Label runat="server" id="msgFileFormatIncorrect" CssClass="alertMsg" Visible="false">Classifier is incorrect</asp:Label>
								</div>
                            </div>
							<div class="allLeft">
                                <div>
                                    <label for="txtMaxTargets">
                                        Max Targets Count</label></div>
                                <div class="required">
                                    *</div>
                                <div>
                                    <asp:TextBox ID="txtMaxTargets" runat="server" CssClass="episize240"></asp:TextBox></div>
                                <div>
                                    <asp:CompareValidator ID="maxTargetsCV" runat="server" ControlToValidate="txtMaxTargets"
                                        Type="Integer" Display="Dynamic" Operator="DataTypeCheck" ErrorMessage="Value must be an integer"
                                        CssClass="alertMsg" />
                                </div>
                            </div>
						</div>
					</div>
				</div>
				<div class="epi-buttonContainer">
					<span class="epi-cmsButton">
						<asp:Button ID="btnSave1" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save"
							OnClick="btnSave1_Click" Text="Save&Test GL Settings" runat="server" />
					</span>
				</div>
			</div>
		</div>
		<div id="view2" style="display: none">
			<div class="epi-contentContainer">
				<div class="epi-padding">
					<asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CellPadding="2"
						DataKeyNames="Id" BorderStyle="None" GridLines="Horizontal" EmptyDataText="No locale mapping set"
						CssClass="epi-default">
						<Columns>
							<asp:TemplateField HeaderText="Delete">
								<HeaderTemplate>
									<asp:CheckBox ID="chkAllDelete" runat="server" onclick="CheckAllDelete(this)" Text="Delete" />
								</HeaderTemplate>
								<ItemTemplate>
									<asp:CheckBox ID="chkDelete" runat="server" CssClass="centered" />
								</ItemTemplate>
								<ItemStyle Wrap="False" />
							</asp:TemplateField>
							<asp:BoundField DataField="EPiLanguage" HeaderText="EPiServer locale" ReadOnly="True">
								<ItemStyle Wrap="False" />
							</asp:BoundField>
							<asp:BoundField DataField="PDLanguage" HeaderText="GL locale" ReadOnly="True">
								<ItemStyle Wrap="False" />
							</asp:BoundField>
							<asp:TemplateField HeaderText="Available as source locale">
								<ItemTemplate>
									<asp:CheckBox ID="chkTblSource" runat="server" Enabled="false" Checked='<%# bool.Parse(Eval("isSource").ToString())%>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Available as target locale">
								<ItemTemplate>
									<asp:CheckBox ID="chkTblTarget" runat="server" Enabled="false" Checked='<%# bool.Parse(Eval("isTarget").ToString())%>' />
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
					<div class="epi-buttonContainer">
						<span class="epi-cmsButton">
							<asp:Button ID="btnDelete2" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Delete"
								type="submit" OnClick="btnDelete2_Click" Text="Delete" runat="server" />
						</span>
					</div>
					<hr />
					<div class="epi-formArea" style="display: block;">
						<div class="epi-size25">
							<div>
								<label for="lstSource">
									EPiServer locale</label>
								<asp:DropDownList ID="lstSource" runat="server" CssClass="episize240">
								</asp:DropDownList>
							</div>
							<div>
								<label for="lstTargets">
									GL locale</label>
								<asp:DropDownList ID="lstTargets" runat="server" CssClass="episize240">
								</asp:DropDownList>
							</div>
							<div>
								<label for="chkAvSource">
									Available as source locale</label>
								<asp:CheckBox ID="chkAvSource" Checked="true" runat="server" CssClass="episize240">
								</asp:CheckBox>
							</div>
							<div>
								<label for="chkAvTarget">
									Available as target locale</label>
								<asp:CheckBox ID="chkAvTarget" Checked="true" runat="server" CssClass="episize240">
								</asp:CheckBox>
							</div>
						</div>
					</div>
					<div class="epi-buttonContainer">
						<span class="epi-cmsButton">
							<asp:Button ID="btnAdd2" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Add"
								type="submit" OnClick="btnAdd2_Click" Text="Add" runat="server" />
						</span>
					</div>
				</div>
			</div>
		</div>
		<div id="view3" style="display: none">
			<div class="epi-contentContainer">
				<div class="epi-padding">
					<asp:Label ID="lblStatus3" runat="server" CssClass="lblStatus" Text=""></asp:Label>
					<div class="epi-formArea" style="display: block;">
						<div class="epi-size25">
							<div>
								<label for="txtSchedulerUser">
									Scheduler User</label>
								<asp:TextBox ID="txtSchedulerUser" runat="server" CssClass="episize240"></asp:TextBox>
							</div>
							<div>
								<label for="txtExclude">
									Property Import Excludes</label>
								<asp:TextBox ID="txtExclude" runat="server" CssClass="episize240"></asp:TextBox>
							</div>
                            <div>
								<label for="lstCopyUrl">
									Copy Name in URL</label>
								<asp:DropDownList ID="lstCopyUrl" runat="server" CssClass="episize240">
								</asp:DropDownList>
							</div>
                            <div>
								<label for="lstNTCreate">
									If page doesn't have translatable fields</label>
								<asp:DropDownList ID="lstNTCreate" runat="server" CssClass="episize240">
								</asp:DropDownList>
							</div>
							<div>
								<label for="chkPushToPD">
									Automatically Push Submissions to GlobalLink</label>
								<asp:CheckBox ID="chkPushToPD" runat="server" CssClass="episize240" />
							</div>
							<div>
								<label for="chkIncUnpub">
									Include Unpublished Child Nodes</label>
								<asp:CheckBox ID="chkIncUnpub" runat="server" CssClass="episize240" />
							</div>
							<div>
								<label for="chkKeepOnUninstall">
									Keep settings and submissions when uninstalling</label>
								<asp:CheckBox ID="chkKeepOnUninstall" runat="server" CssClass="episize240" />
							</div>
                            <div>
								<label for="chkAllowCancel">
									Allow cancellation</label>
								<asp:CheckBox ID="chkAllowCancel" runat="server" CssClass="episize240" />
							</div>
                            <div>
								<label for="txtDashboardLevel">
									Number of child levels to display on dashboard</label>
                                <asp:TextBox ID="txtDashboardLevel" runat="server" CssClass="episize240"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtDashboardLevel"
                                        Type="Integer" Display="Dynamic" Operator="DataTypeCheck" ErrorMessage="Value must be an integer"
                                        CssClass="alertMsg" />
							</div>
							<div>
								<label for="lstSaveStatus">
									Default status on importing</label>
								<asp:DropDownList ID="lstSaveStatus" runat="server" CssClass="episize240">
								</asp:DropDownList>
							</div>
							<div>
								<label for="lstLogLevel">
									Log level</label>
								<asp:DropDownList ID="lstLogLevel" runat="server" CssClass="episize240">
								</asp:DropDownList>
							</div>
							<div>
								<label for="txtArchiveDate">Archive submissions</label>
								<table><tr><td>
								<asp:TextBox ID="txtArchiveDate" runat="server" ReadOnly="true" CssClass="episize240"></asp:TextBox>
								<input type="button" id="calButton" runat="server" value="..." onclick="popupCalendar()" />
								<asp:ImageButton ID="btnArchive" runat="server" Width="16px" ImageUrl="Images/archive.png"
									OnClick="btnArchive_Click" ToolTip="Archive submissions which were published or cancelled before selected date" />
								<div id="dateField" style="display: none;position:absolute">
									<asp:Calendar ID="calDate" runat="server" CellPadding="4" BorderColor="#999999" Font-Names="Verdana"
										Font-Size="8pt" BackColor="#CCCCCC" Height="180px" ForeColor="Black" DayNameFormat="FirstLetter"
										OnVisibleMonthChanged="KeepCalendarVisible" OnSelectionChanged="calDate_SelectionChanged">
										<TodayDayStyle ForeColor="Black" BackColor="#CCCCCC"></TodayDayStyle>
										<SelectorStyle BackColor="#CCCCCC"></SelectorStyle>
										<NextPrevStyle VerticalAlign="Bottom"></NextPrevStyle>
										<DayHeaderStyle Font-Size="7pt" Font-Bold="True" BackColor="#CCCCCC"></DayHeaderStyle>
										<SelectedDayStyle Font-Bold="True" ForeColor="White" BackColor="#666666"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" BorderColor="Black" BackColor="#999999" CssClass="calHead"></TitleStyle>
										<WeekendDayStyle BackColor="LightSteelBlue"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="#808080"></OtherMonthDayStyle>
									</asp:Calendar>
								</div>
								</td></tr></table>
							</div>
							<div>
								<span class="epi-cmsButton">
									<asp:Button ID="btnDwnlLog" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Download"
										OnClick="btnDwnlLog_Click" runat="server" Text="Download adaptor logs" />
								</span>
								<span class="epi-cmsButton">
									<asp:Button ID="btnDumpDb" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Download"
										OnClick="btnDump_Click" runat="server" Text="Dump DB to log" />
							</span>
							</div>
							<asp:TextBox ID="txtLog" runat="server" Visible="false" Width="200" Text="C:\\log\\GlobalLinkLog.txt"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="epi-buttonContainer">
					<span class="epi-cmsButton">
						<asp:Button ID="Button3" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save"
							OnClick="btnSave3_Click" Text="Save Other Settings" runat="server" />
					</span>
				</div>
			</div>
		</div>
		<div id="view4" style="display: none">
			<div class="epi-contentContainer">
				<div class="epi-padding">
					<div class="epi-formArea" style="display: block;">
						<div class="epi-size25">
							<div>
								<label for="lstSource">
									Content Type</label>
								<asp:DropDownList ID="pageTypeList" runat="server" CssClass="episize240" AutoPostBack="true" OnSelectedIndexChanged="pageTypeChanged">
								</asp:DropDownList>
							</div>
						</div>
					</div>
					<asp:GridView ID="pagePropsGrid" runat="server" AutoGenerateColumns="False" CellPadding="2"
						DataKeyNames="propertyId" BorderStyle="None" GridLines="Horizontal" EmptyDataText=""
						CssClass="epi-default">
						<Columns>
							<asp:BoundField DataField="contentTypeId" Visible="false" />
							<asp:BoundField DataField="propertyId" HeaderText="Id" ReadOnly="True">
								<ItemStyle Wrap="False" />
							</asp:BoundField>
							<asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True">
								<ItemStyle Wrap="False" />
							</asp:BoundField>
							<asp:BoundField DataField="displayName" HeaderText="Field name" ReadOnly="True">
								<ItemStyle Wrap="False" />
							</asp:BoundField>
							<asp:BoundField DataField="type" HeaderText="Type" ReadOnly="True">
								<ItemStyle Wrap="False" />
							</asp:BoundField>
							<asp:BoundField DataField="stringLength" HeaderText="Length" ReadOnly="True">
								<ItemStyle Wrap="False" />
							</asp:BoundField>
							<asp:TemplateField HeaderText="Language Specific">
								<ItemTemplate>
									<asp:CheckBox ID="chkLocalized" runat="server" Enabled="false" CssClass="centered" Checked='<%# bool.Parse(Eval("localized").ToString())%>' />
								</ItemTemplate>
								<ItemStyle Wrap="False" />
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Translatable">
								<ItemTemplate>
									<asp:CheckBox ID="chkTranslate" runat="server" CssClass="centered" Enabled='<%# bool.Parse(Eval("localized").ToString())%>' Checked='<%# bool.Parse(Eval("translatable").ToString())%>'
										AutoPostBack="true" OnCheckedChanged="chkTranslate_changed"  />
								</ItemTemplate>
								<ItemStyle Wrap="False" />
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
					<div class="epi-buttonContainer">
						<span class="epi-cmsButton">
							<asp:Button ID="syncButton" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Add"
								OnClick="btnSync_Click" Text="Synchronize" runat="server" />
						</span>
					</div>
				</div>
			</div>
		</div>
		<div id="view5" style="display: none">
			<div class="epi-contentContainer">
				<div class="epi-padding">
					<asp:Label ID="lblStatus5" runat="server" CssClass="lblStatus" Text=""></asp:Label>
					<div class="epi-formArea" style="display: block;">
						<div class="epi-size25">
							<div>
								<label for="lstCommProject">
									Default project</label>
								<asp:DropDownList ID="lstCommProject" runat="server" CssClass="episize240" />
							</div>
							<div>
								<label for="txtCommFileFormat">
									Classifier</label>
								<asp:TextBox ID="txtCommFileFormat" runat="server" CssClass="episize240"></asp:TextBox>
							</div>
						</div>
					</div>
				</div>
				<div class="epi-buttonContainer">
					<span class="epi-cmsButton">
						<asp:Button ID="Button6" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save"
							OnClick="btnSave5_Click" Text="Save Commerce Settings" runat="server" />
					</span>
				</div>
			</div>
		</div>
		<div id="view6" style="display: none">
			<div class="epi-contentContainer">
				<div class="epi-padding">
					<asp:Label ID="lblStatus6" runat="server" CssClass="lblStatus" Text=""></asp:Label>
					<asp:Label ID="lblErorrStatus6" runat="server" CssClass="lblError" Text=""></asp:Label>
					<div class="epi-formArea" style="display: block;">
						<div class="epi-size25 epi-size25-1">
							<div class="table">
									<div><asp:RadioButton runat="server" GroupName="cleanType" Checked="true" ID="cleanType1" Text="All submissions (active and history)" /></div>
									<div><asp:RadioButton runat="server" GroupName="cleanType" ID="cleanType2" Text="All active submissions only (keep history)" /></div>
                                    <div><asp:RadioButton runat="server" GroupName="cleanType" ID="cleanTypeHistory" Text="All history submissions only (keep active)" /></div>
									<div>
										<asp:RadioButton runat="server" GroupName="cleanType" ID="cleanType3" Text="All submissions created after" />
										<table style="float:left" cellpadding="0" cellspacing="0"><tr><td>
											<asp:TextBox ID="cleanDate" runat="server" ReadOnly="true" CssClass="episize240"></asp:TextBox>
											<input type="button" id="cleanDateBtn" runat="server" value="..." onclick="popupCalendar2()" />
								<div id="dateField2" style="display: none;position:absolute">
									<asp:Calendar ID="calCleanDate" runat="server" CellPadding="4" BorderColor="#999999" Font-Names="Verdana"
										Font-Size="8pt" BackColor="#CCCCCC" Height="180px" ForeColor="Black" DayNameFormat="FirstLetter"
										OnVisibleMonthChanged="KeepCalendarVisible2" OnSelectionChanged="calDate2_SelectionChanged">
										<TodayDayStyle ForeColor="Black" BackColor="#CCCCCC"></TodayDayStyle>
										<SelectorStyle BackColor="#CCCCCC"></SelectorStyle>
										<NextPrevStyle VerticalAlign="Bottom"></NextPrevStyle>
										<DayHeaderStyle Font-Size="7pt" Font-Bold="True" BackColor="#CCCCCC"></DayHeaderStyle>
										<SelectedDayStyle Font-Bold="True" ForeColor="White" BackColor="#666666"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" BorderColor="Black" BackColor="#999999" CssClass="calHead"></TitleStyle>
										<WeekendDayStyle BackColor="LightSteelBlue"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="#808080"></OtherMonthDayStyle>
									</asp:Calendar>
								</div>
								</td></tr></table>
									</div>
									<div><asp:RadioButton runat="server" GroupName="cleanType" ID="cleanType4" Text="Project with ID" /><asp:TextBox ID="cleanProjectId" runat="server" CssClass="episize240"></asp:TextBox></div>
									<div><asp:RadioButton runat="server" GroupName="cleanType" ID="cleanType5" Text="Item with ID" /><asp:TextBox ID="cleanItemId" runat="server" CssClass="episize240"></asp:TextBox></div>
                                    <div>
										<asp:RadioButton runat="server" GroupName="cleanType" ID="cleanTypeOld" Text="All submissions created before" />
										<table style="float:left" cellpadding="0" cellspacing="0"><tr><td>
											<asp:TextBox ID="cleanOldDate" runat="server" ReadOnly="true" CssClass="episize240"></asp:TextBox>
											<input type="button" id="cleanOldDateBtn" runat="server" value="..." onclick="popupCalendar3()" />
								    <div id="cleanOldDateCal" style="display: none;position:absolute">
									<asp:Calendar ID="calCleanOldDate" runat="server" CellPadding="4" BorderColor="#999999" Font-Names="Verdana"
										Font-Size="8pt" BackColor="#CCCCCC" Height="180px" ForeColor="Black" DayNameFormat="FirstLetter"
										OnVisibleMonthChanged="KeepCalendarVisible3" OnSelectionChanged="calOldDate3_SelectionChanged">
										<TodayDayStyle ForeColor="Black" BackColor="#CCCCCC"></TodayDayStyle>
										<SelectorStyle BackColor="#CCCCCC"></SelectorStyle>
										<NextPrevStyle VerticalAlign="Bottom"></NextPrevStyle>
										<DayHeaderStyle Font-Size="7pt" Font-Bold="True" BackColor="#CCCCCC"></DayHeaderStyle>
										<SelectedDayStyle Font-Bold="True" ForeColor="White" BackColor="#666666"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" BorderColor="Black" BackColor="#999999" CssClass="calHead"></TitleStyle>
										<WeekendDayStyle BackColor="LightSteelBlue"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="#808080"></OtherMonthDayStyle>
									</asp:Calendar>
								</div>
								</td></tr></table>
									</div>

							</div>
							<div class="allLeft table">
                                <div>
                                    <label for="cleanPassword">
                                        Password provided by translations.com</label></div>
                                <div class="required">
                                    *</div>
                                <div>
                                    <asp:TextBox ID="cleanPassword" TextMode="Password" runat="server" CssClass="episize240"></asp:TextBox>
								</div>
                            </div>
							<div class="table">
								<span class="alertMsg">NOTE: This action cannot be undone</span>
							</div>
							<div class="epi-buttonContainer">
								<span class="epi-cmsButton">
									<asp:Button ID="btnClearDB" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Clean"
										OnClick="btnCleanDB_Click" runat="server" Text="CLEAN" />
								</span>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div id="view7" style="display: block">
			<div class="epi-contentContainer">
				<div class="epi-padding">
					<asp:Label ID="lblStatus7" runat="server" CssClass="lblStatus" Text=""></asp:Label>
					<asp:Label ID="lblErorrStatus7" runat="server" CssClass="lblError" Text=""></asp:Label>
					<div class="epi-formArea" style="display: block;">
						<div class="epi-size25">
							 <div class="allLeft">
                                <div>
                                    <label for="txtSmtpHost">
                                        SMTP Host</label></div>
								<div class="required">
                                    *</div>
                                <div>
                                    <asp:TextBox ID="txtSmtpHost" runat="server" CssClass="episize240"></asp:TextBox>
                                </div>
                            </div>
							<div class="allLeft">
                                <div>
                                    <label for="txtSmtpPort">
                                        SMTP Port</label></div>
								<div class="required">
                                    *</div>
                                <div>
                                    <asp:TextBox ID="txtSmtpPort" runat="server" CssClass="episize240"></asp:TextBox>
								</div>
								<div>
									<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtSmtpPort"
										Type="Integer" Display="Dynamic" Operator="DataTypeCheck" ErrorMessage="Value must be an integer"
										CssClass="alertMsg" />
								</div>
                            </div>
							<div class="allLeft">
                                <div>
                                    <label for="txtSmtpLogin">
                                        Login</label></div>
                                <div class="required">
                                    &nbsp;</div>
                                <div>
                                    <asp:TextBox ID="txtSmtpLogin" runat="server" CssClass="episize240"></asp:TextBox></div>
                            </div>
							<div class="allLeft">
                                <div>
                                    <label for="txtSmtpPass">
                                        Password</label></div>
                                <div class="required">
                                    &nbsp;</div>
                                <div>
                                    <asp:TextBox ID="txtSmtpPass" TextMode="Password" runat="server" CssClass="episize240"></asp:TextBox></div>
                            </div>
							<div class="allLeft">
								<div>
									<label for="chkEnableSSL">Use SSL</label>
								</div>
								<div class="required">&nbsp;</div>
                                <div>
									<asp:CheckBox ID="chkEnableSSL" runat="server" CssClass="episize240" />
								</div>
							</div>
                            <div class="allLeft">
                                <div>
                                    <label for="txtMailRecepients">
                                        Recipients</label></div>
                                <div class="required">
                                    *</div>
                                <div>
                                    <asp:TextBox ID="txtMailRecepients" runat="server" CssClass="episize240 left"></asp:TextBox>
								</div>
                            </div>
							<div class="allLeft">
								<div>
									<label for="chkSendError">Send mail for errors</label>
								</div>
								<div class="required">&nbsp;</div>
								<div>
									<asp:CheckBox ID="chkSendError" runat="server" CssClass="episize240" />
								</div>
							</div>
						</div>
					</div>
				</div>
				<div class="epi-buttonContainer">
					<span class="epi-cmsButton left">
						<asp:Button ID="btnTestEmail" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save"
							OnClick="btnTestEmail_Click" Text="Send test email"
							runat="server" />
					</span>
					<span class="epi-cmsButton">
						<asp:Button ID="btnSave7" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save"
							OnClick="btnSave7_Click" Text="Save Notification Settings" runat="server" />
					</span>
				</div>
			</div>
		</div>
		<div id="view8" style="display: block">
			<div class="epi-contentContainer">
				<div class="epi-padding">
					<asp:Label ID="lblStatus8" runat="server" CssClass="lblStatus" Text=""></asp:Label>
					<asp:Label ID="lblErrorStatus8" runat="server" CssClass="lblError" Text=""></asp:Label>
<br>
					<asp:Label Text="Define which roles can access the GlobalLink gadget" ID="lbll8" runat="server"></asp:Label>
										<div class="epi-formArea" style="display: block;">
						<div class="epi-size25">
							 <table cellspacing="0" cellpadding="0" border="0">
									<tr>
										<th align="left" valign="middle" class="style8" scope="col" style="width: 12px">
											&nbsp;
										</th>
										<th height="24" align="left" valign="middle" class="style8" scope="col" style="width:260px;">
											Existing roles
										</th>
										<th class="style8" scope="col" style="width:40px;">
											&nbsp;
										</th>
										<th align="left" valign="middle" class="style8" scope="col" style="width:260px;">
											Granted access to GlobalLink gadget
										</th>
									</tr>
									<tr>
										<td align="left" class="style8" scope="col" style="width: 12px" valign="middle">
											&nbsp;
										</td>
										<td align="left" class="style8" height="99" scope="col" valign="middle">
											<asp:ListBox ID="lstRoles" runat="server" CssClass="style8" Height="150px" SelectionMode="Multiple"
												Width="250"></asp:ListBox>
										</td>
										<td align="center" class="roleButtons" scope="col" valign="middle">
											<table align="center" width="100%" cellspacing="2">
												<tr>
													<td align="left">
														<asp:Button runat="server" Width="30" CssClass="epi-cmsButton" ID="btnSelectsingletarget"  OnClick="btnSelectsingletarget_Click" CausesValidation="false" Text=">" />
													</td>
												</tr>
												<tr>
													<td align="left">
														<asp:Button runat="server" Width="30" CssClass="epi-cmsButton" ID="btnRemovesingletarget"  OnClick="btnRemovesingletarget_Click" CausesValidation="false" Text="<" />
													</td>
												</tr>
												<tr>
													<td align="left">
														<asp:Button runat="server" Width="30" CssClass="epi-cmsButton" ID="btnSelecttarget"  OnClick="btnSelecttarget_Click" CausesValidation="false" Text=">>" />
													</td>
												</tr>
												<tr>
													<td align="left">
														<asp:Button runat="server" Width="30" CssClass="epi-cmsButton" ID="btnRemovetarget"  OnClick="btnRemovetarget_Click" CausesValidation="false" Text="<<" />
													</td>
												</tr>
											</table>
										</td>
										<td align="left" class="style8" scope="col" valign="middle">
											<asp:ListBox ID="lstGranted" runat="server" CausesValidation="True" CssClass="style8"
												Height="150px" SelectionMode="Multiple" Width="250"></asp:ListBox>
										</td>
									</tr>
								</table>
						</div>
					</div>
				</div>
				<div class="epi-buttonContainer">
					<span class="epi-cmsButton">
						<asp:Button ID="btnSave8" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save"
							OnClick="btnSave8_Click" Text="Save Security Settings" runat="server" />
					</span>
				</div>
			</div>
		</div>
        <div id="view9" style="display: none">
			<div class="epi-contentContainer">
				<div class="epi-padding">
					<asp:Label ID="lblStatus9" runat="server" CssClass="lblStatus" Text=""></asp:Label>
                    <asp:Label ID="lblErorrStatus9" runat="server" CssClass="lblError" Text=""></asp:Label>
					<div class="epi-formArea" style="display: block;">
						<div class="epi-size25">
                            <div>
								<label for="lstSkipSummary">
									Skip summary</label>
								<asp:DropDownList ID="lstSkipSummary" runat="server" CssClass="episize240">
								</asp:DropDownList>
							</div>
                            <hr />
                            <div>
								<label for="lstCmsProject">
									Default project</label>
								<asp:DropDownList ID="lstCmsProject" runat="server" CssClass="episize240" />
							</div>
							<div>
								<label for="lstDefSource">
									Default Source Locale</label>
								<asp:DropDownList ID="lstDefSource" runat="server" CssClass="episize240">
								</asp:DropDownList>
							</div>
                            <div>
								<label for="txtDefDueDate">
									Due date (M/d/yyyy or +5d)</label>
								<asp:TextBox ID="txtDefDueDate" runat="server" CssClass="episize240"></asp:TextBox>
							</div>
                            <hr />
							<div>
								<label for="chkIncludeChild">
									Include child nodes</label>
								<asp:CheckBox ID="chkIncludeChild" runat="server" CssClass="episize240" />
							</div>
                            <div>
								<label for="chkIncludeReferences">
									Include referenced content</label>
								<asp:CheckBox ID="chkIncludeReferences" runat="server" CssClass="episize240" />
							</div>
                            <div>
								<label for="chkDisCopyCA">
									Disable copying contentArea references</label>
								<asp:CheckBox ID="chkDisCopyCA" runat="server" CssClass="episize240" />
							</div>
                            <div>
								<label for="chkExclUnchanged">
									Exclude pages Unchanged since last translation</label>
								<asp:CheckBox ID="chkExclUnchanged" runat="server" CssClass="episize240" />
							</div>
                            <div>
								<label for="chkExclExpired">
									Exclude expired pages</label>
								<asp:CheckBox ID="chkExclExpired" runat="server" CssClass="episize240" />
							</div>
                            <div>
								<label for="chkAutoPublish">
									Auto-publish translated content on completions</label>
								<asp:CheckBox ID="chkAutoPublish" runat="server" CssClass="episize240" />
							</div>
							<div>
								<label for="chkCancelIfResend">
									Cancel already sent targets if resending</label>
								<asp:CheckBox ID="chkCancelIfResend" runat="server" CssClass="episize240" />
							</div>
                            <hr />
                            <asp:Label Text="Select which contentTypes should be unchecked by default" ID="Label1" runat="server"></asp:Label>
										<div class="epi-formArea" style="display: block;">
						<div class="epi-size25">
							 <table cellspacing="0" cellpadding="0" border="0">
									<tr>
										<th align="left" valign="middle" class="style8" scope="col" style="width: 12px">
											&nbsp;
										</th>
										<th height="24" align="left" valign="middle" class="style8" scope="col" style="width:260px;">
											Existing contentTypes
										</th>
										<th class="style8" scope="col" style="width:40px;">
											&nbsp;
										</th>
										<th align="left" valign="middle" class="style8" scope="col" style="width:260px;">
											Non-translatable content types
										</th>
									</tr>
									<tr>
										<td align="left" class="style8" scope="col" style="width: 12px" valign="middle">
											&nbsp;
										</td>
										<td align="left" class="style8" height="99" scope="col" valign="middle">
											<asp:ListBox ID="lstNTSource" runat="server" CssClass="style8" Height="150px" SelectionMode="Multiple"
												Width="250"></asp:ListBox>
										</td>
										<td align="center" class="roleButtons" scope="col" valign="middle">
											<table align="center" width="100%" cellspacing="2">
												<tr>
													<td align="left">
														<asp:Button runat="server" Width="30" CssClass="epi-cmsButton" ID="Button1"  OnClick="btnNTSS_Click" CausesValidation="false" Text=">" />
													</td>
												</tr>
												<tr>
													<td align="left">
														<asp:Button runat="server" Width="30" CssClass="epi-cmsButton" ID="Button2"  OnClick="btnNTRS_Click" CausesValidation="false" Text="<" />
													</td>
												</tr>
												<tr>
													<td align="left">
														<asp:Button runat="server" Width="30" CssClass="epi-cmsButton" ID="Button4"  OnClick="btnNTSA_Click" CausesValidation="false" Text=">>" />
													</td>
												</tr>
												<tr>
													<td align="left">
														<asp:Button runat="server" Width="30" CssClass="epi-cmsButton" ID="Button5"  OnClick="btnNTRA_Click" CausesValidation="false" Text="<<" />
													</td>
												</tr>
											</table>
										</td>
										<td align="left" class="style8" scope="col" valign="middle">
											<asp:ListBox ID="lstNTTarget" runat="server" CausesValidation="True" CssClass="style8"
												Height="150px" SelectionMode="Multiple" Width="250"></asp:ListBox>
										</td>
									</tr>
								</table>
						</div>
						</div>
					</div>
				</div>
				<div class="epi-buttonContainer">
					<span class="epi-cmsButton">
						<asp:Button ID="btnSave9" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save"
							OnClick="btnSave9_Click" Text="Save Wizard Settings" runat="server" />
					</span>
				</div>
			</div>
		</div>
	</div>
	</form>
</body>
</html>
