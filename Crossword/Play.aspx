﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Play.aspx.cs" Inherits="Crossword.Play" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Crossword</title>

	<style type="text/css">
		body, input {
			font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
			font-size: 9pt;
		}

		body {
			padding: 10px;
		}

		table {
			border-collapse: collapse;
			empty-cells: show;
		}

		fieldset {
			 width: 200px;
		}

		.grid {
			/*display: inline-block;*/
		}

		.grid td {
			height: 32px;
			width: 32px;
			border: 1px solid black;
		}

		.grid td {
			background-repeat: no-repeat;
			text-align: center;
			vertical-align: middle;
		}

		.grid input[type=text] {
			height: 18px;
			width: 18px;
			border: 0;
			font-size: 18px;
			font-weight: 600;
			text-transform: uppercase;
			text-align: center;
			vertical-align: middle;
		}

		.grid input[type=checkbox] {
			height: 22px;
			width: 22px;
			border: 0;
			font-size: 8px;
			text-align: center;
			vertical-align: middle;
			cursor: pointer;
			-webkit-appearance: none;
			appearance: none;
		}

			.grid td:has(input[type=checkbox]):hover {
				background-color: gainsboro;
			}
			
		legend {
			padding: 5px;
		}

		.button {
			border: 1px solid;
			border-color: royalblue;
			background-color: dodgerblue;
			border-radius: 2px;
			box-shadow: 3px 3px 0 rgba(1, 1, 1, 0.11);
			color: #ffffffe0;
			font-weight: 600;
			height: 25px;
			min-width: 80px;
			text-align: center;
			margin: 4px;
			cursor: pointer;
		}

			.button:hover {
				color: white;
			}

			.button:focus {
				color: white;
				border-color: dodgerblue;
				background-color: royalblue;
				box-shadow: none;
				-webkit-transform: translate(2px, 2px);
				transform: translate(2px, 2px)
			}
	</style>

	<script src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			// Set cell bg black if checked
			$(".grid").find("input[type='checkbox']").click(function () {
				$(this).parent().css("background-color", this.checked ? "#000" : "#fff");
			});
		});

		function nextStep(step) {
			if (step == 1) {
				window.location.href = window.location.href;
				return 0;
			}
			var frm = document.forms["Crossword"];
			frm.Step.value = step;
			frm.submit();
		}

		function cellCheck() {
			var frm = document.forms["Crossword"];
			frm.submit();
		}
	</script>
</head>
<body>
	<form id="Crossword" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
		<asp:HiddenField ID="Step" runat="server" />
		<div>
			<h1>Crossword</h1>
		</div>
		
		<%-- Step 1 --%>
		<asp:Panel ID="pnlStep1" runat="server" Visible="false" style="padding-bottom: 20px;">
			<fieldset id="fsDimensions">
				<legend>Choose Dimensions</legend>
				<table>
					<colgroup>
						<col style="width: 100px" />
						<col />
					</colgroup>
					<tr>
						<td>Rows:</td>
						<td>
							<asp:TextBox ID="txtRows" runat="server" MaxLength="3" Width="20px">15</asp:TextBox>
						</td>
					</tr>
					<tr>
						<td>Columns:</td>
						<td>
							<asp:TextBox ID="txtColumns" runat="server" MaxLength="3" Width="20px">15</asp:TextBox>
						</td>
					</tr>
				</table>
				<div style="padding-top: 10px; text-align: center">
					<input type="button" id="btnReset1" onclick="nextStep(1);" class="button" value="Reset" />
					<input type="button" id="btnStep1" runat="server" class="button" onclick="nextStep(2);" value="Next >>" />
				</div>
			</fieldset>
		</asp:Panel>

		<%-- Step 2 --%>
		<asp:Panel ID="pnlStep2" runat="server" Visible="false" style="padding-bottom: 20px;">
			<fieldset id="fsLayout">
				<legend>Set Layout</legend>
				<p>
					Click cells to insert blanks.
				</p>
				<div style="padding-top: 10px; text-align: center">
					<input type="button" id="btnReset2" onclick="nextStep(1);" class="button" value="Reset" />
					<input type="button" id="btnStep2" onclick="nextStep(3);" class="button" value="Next >>" />
				</div>
			</fieldset>
		</asp:Panel>

		<%-- Grid --%>
		<asp:UpdatePanel ID="upCallback" runat="server">
			<ContentTemplate>
				<asp:Table ID="tblGrid" runat="server" CssClass="grid"></asp:Table>
			</ContentTemplate>
		</asp:UpdatePanel>

		<%-- Step 3 --%>
		<asp:Panel ID="pnlStep3" runat="server" Visible="false">
			<div style="padding-top: 20px;">
				<input type="button" id="btnReset3" onclick="nextStep(1);" class="button" value="Reset" />
			</div>
		</asp:Panel>

		<%-- Error --%>
		<asp:Panel ID="pnlError" runat="server" Visible="false" Width="100%">
			<asp:Label ID="lblError" runat="server" ForeColor="Crimson"></asp:Label>
		</asp:Panel>
	</form>
</body>
</html>

