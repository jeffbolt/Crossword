<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Play.aspx.cs" Inherits="Crossword.Play" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Crossword</title>

	<style type="text/css">
		html, input {
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

		.grid td {
			height: 30px;
			width: 30px;
			border: 1px solid black;
		}

		.grid td {
			background-repeat: no-repeat;
			text-align: center;
			vertical-align: middle;
		}

		.grid input[type=text] {
			font-size: 16px;
			font-weight: 600;
			text-transform: uppercase;
			text-align: center;
			vertical-align: middle;
		}

		legend {
			/*border: 1px solid #bababa;*/
			padding: 5px;
		}

		input[type=button], input[type=submit] {
			border: 1px solid;
			border-color: royalblue;
			background-color: dodgerblue;
			color: white;
			font-weight: 600;
			height: 25px;
			min-width: 80px;
			text-align: center;
		}
	</style>

	<script src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>
	<script type="text/javascript">
		//$(document).ready(function () {
		//	$('.cell').addEventListener("click", function () {
		//		console.log(this);
		//	});
		//});

		//$(document).on("click", ".cell", function (e) {
		//	var id = e.target.id;
		//	//console.log("id=" + id)
		//	var cell = $("#" + id);
		//	if (e.which == 1) {
		//		cell.css("background-color", "white");
		//		cell.prop("readonly", "false");
		//		break;
		//	}
		//}

		/*$(document).on("contextmenu", ".cell", function (e) {
			var id = e.target.id;
			console.log("id=" + id);
			var cell = $("#" + id);

			if (e.which == 3) {
				cell.val("");
				cell.css("background-color", "black");
				cell.prop("readonly", "true");
			}
			return false;
		});
		*/

		function nextStep(step) {
			if (step == 1) {
				window.location.href = window.location.href;
				return 0;
			}
			var frm = document.forms["Crossword"];
			frm.Step.value = step;
			frm.submit();
		}

		function cellClick(id) {
			try {

				var cell = document.getElementById(id);
				cell.select();
				cell.focus();
			} catch (e) {
				console.error(e);
			}
		}
	</script>
</head>
<body>
	<form id="Crossword" runat="server">
		<asp:HiddenField ID="Step" runat="server" />
		<div>
			<h2>Crossword</h2>
		</div>

		<%-- Step 1 --%>
		<asp:Panel ID="pnlStep1" runat="server" Visible="false" style="padding-bottom: 20px;">
			<fieldset id="fsDimensions" style="width: 200px">
				<legend>Step 1) Choose Dimensions</legend>
				<table>
					<colgroup>
						<col style="width: 100px" />
						<col />
					</colgroup>
					<tr>
						<td>Rows:</td>
						<td>
							<asp:TextBox ID="txtRows" runat="server" MaxLength="3" Width="20px">10</asp:TextBox>
						</td>
					</tr>
					<tr>
						<td>Columns:</td>
						<td>
							<asp:TextBox ID="txtColumns" runat="server" MaxLength="3" Width="20px">10</asp:TextBox>
						</td>
					</tr>
				</table>
				<div style="padding-top: 10px; text-align: center">
					<input type="button" id="btnReset1" onclick="nextStep(1);" value="Reset" />
					<input type="button" id="btnStep1" onclick="nextStep(2);" value="Next >>" />
				</div>
			</fieldset>
		</asp:Panel>

		<%-- Step 2 --%>
		<asp:Panel ID="pnlStep2" runat="server" Visible="false" style="padding-bottom: 20px;">
			<fieldset id="fsLayout" style="width: 200px">
				<legend>Step 2) Set Layout</legend>
				<p>
					Check cells to insert blanks.
				</p>
				<div style="padding-top: 10px; text-align: center">
					<input type="button" id="btnReset2" onclick="nextStep(1);" value="Reset" />
					<input type="button" id="btnStep2" onclick="nextStep(3);" value="Next >>" />
				</div>
			</fieldset>
		</asp:Panel>

		<%-- Grid --%>
		<asp:Table ID="tblGrid" runat="server" CssClass="grid"></asp:Table>

		<%-- Step 3 --%>
		<asp:Panel ID="pnlStep3" runat="server" Visible="false" style="padding-top: 20px;">
			<input type="button" id="btnReset3" onclick="nextStep(1);" value="Reset" />
		</asp:Panel>

		<%-- Error --%>
		<asp:Panel ID="pnlError" runat="server" Visible="false" Width="100%">
			<asp:Label ID="lblError" runat="server" ForeColor="Crimson"></asp:Label>
		</asp:Panel>
	</form>
</body>
</html>

