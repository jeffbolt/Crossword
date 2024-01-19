<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Crossword._default" %>

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
			height: 20px;
			width: 20px;
			border: 1px solid black;
		}

		.grid td {
			background-repeat: no-repeat;
		}

		.grid input[type=text] {
			font-size: 12px;
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
			min-width: 40px;
		}
	</style>

	<script src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			//$('.cell').addEventListener("click", function () {
			//	console.log(this);
			//});
			//console.log($("#Cell_1_6").prop("id"))
		});

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
			//console.log("id=" + id)
			var cell = $("#" + id);

			if (e.which == 3) {
				cell.val("");
				cell.css("background-color", "black");
				cell.prop("readonly", "true");
			}
			//switch (e.which) {
			//	case 1:
			//		console.log('Left Mouse button pressed.');
			//		break;
			//	case 2:
			//		console.log('Middle Mouse button pressed.');
			//		break;
			//	case 3:
			//		console.log('Right Mouse button pressed.');
			//		break;
			//	default:
			//		console.log('You have a strange Mouse!');
			//}
			return false;
		});
		*/

		function nextStep(step) {
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

		function handleEvent(event) {
			if (event.type === "fullscreenchange") {
				/* handle a full screen toggle */
			} else {
				/* handle a full screen toggle error */
			}
		}
	</script>
</head>
<body>
	<form id="Crossword" runat="server" onsubmit="nextStep();">
		<asp:HiddenField ID="Step" runat="server" OnValueChanged="Step_ValueChanged" />
		<div>
			<h2>Crossword</h2>
		</div>

		<%-- Step 1 --%>
		<asp:Panel ID="pnlStep1" runat="server" Visible="false">
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
							<asp:TextBox ID="txtRows" runat="server" MaxLength="3" Width="20px">25</asp:TextBox></td>
					</tr>
					<tr>
						<td>Columns:</td>
						<td>
							<asp:TextBox ID="txtColumns" runat="server" MaxLength="3" Width="20px">25</asp:TextBox></td>
					</tr>
				</table>
				<div style="padding-top: 10px; text-align: center">
					<input type="button" id="btnStep1" onclick="nextStep(2);" value="Next >>" />
					<%--<asp:Button ID="btnUpdate" runat="server" Text="Update" Width="100px" OnClick="btnUpdate_Click" />--%>
				</div>
			</fieldset>
		</asp:Panel>

		<%-- Step 2 --%>
		<asp:Panel ID="pnlStep2" runat="server" Visible="false">
			<fieldset id="fsLayout" style="width: 200px">
				<legend>Step 2) Set Layout</legend>
				<p>
					Right-click squares to insert blanks.
				</p>
				<div style="padding-top: 10px; text-align: center">
					<input type="button" id="btnStep2" onclick="nextStep(3);" value="Next >>" />
					<%--<asp:Button ID="btnUpdate" runat="server" Text="Update" Width="100px" OnClick="btnUpdate_Click" />--%>
				</div>
			</fieldset>
		</asp:Panel>

		<%-- Grid --%>
		<asp:Panel ID="pnlGrid" runat="server" Visible="false">
			<asp:Table ID="Grid" runat="server" CssClass="grid" ClientIDMode="Static"></asp:Table>
		</asp:Panel>

		<%-- Error --%>
		<asp:Panel ID="pnlError" runat="server" Visible="false" Width="100%">
			<asp:Label ID="lblError" runat="server" ForeColor="Crimson"></asp:Label>
		</asp:Panel>
	</form>
</body>
</html>
