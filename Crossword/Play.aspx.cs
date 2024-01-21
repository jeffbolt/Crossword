using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Crossword
{
	public partial class Play : System.Web.UI.Page
	{
		public const int MaxGridArea = 100000;
		public const int CheckSize = 18;
		public const int TextSize = 18;
		public const int MinRows = 5;
		public const int MinColumns = 5;

		public int ThisStep = 1;
		
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Page.IsPostBack)
			{
				if (!int.TryParse(Step.Value, out ThisStep))
					ThisStep = 1;
			}
			
			switch (ThisStep)
			{
				case 1:
					Step1();
					break;
				case 2:
					Step2();
					break;
				case 3:
					Step3();
					break;
				default:
					DisplayError($"Unknown Step {ThisStep}.");
					break;
			}
		}

		private void Step1()
		{
			pnlStep1.Visible = true;
			pnlStep2.Visible = false;
			pnlStep3.Visible = false;
		}

		private void Step2()
		{
			pnlStep1.Visible = false;
			pnlStep2.Visible = true;
			pnlStep3.Visible = false;

			if (!int.TryParse(txtRows.Text, out int rows) || rows < MinRows)
			{
				DisplayError("Invalid number of <b>Rows</b>.");
				return;
			}

			if (!int.TryParse(txtColumns.Text, out int columns) || columns < MinColumns)
			{
				DisplayError("Invalid number of <b>Columns</b>.");
				return;
			}

			if (rows * columns > MaxGridArea)
			{
				DisplayError($"Grid area cannot exceed {MaxGridArea:n0}.");
				return;
			}

			BuildGrid(rows, columns);
		}

		private void Step3()
		{
			pnlStep1.Visible = false;
			pnlStep2.Visible = false;
			pnlStep3.Visible = true;

			int rows = int.Parse(txtRows.Text);
			int columns = int.Parse(txtColumns.Text);
			BuildGrid(rows, columns);
		}

		private void DisplayError(string message)
		{
			pnlError.Visible = false;
			lblError.Text = message;
		}

		private void BuildGrid(int rows, int columns)
		{
			tblGrid.Rows.Clear();

			TableRow tr;
			int cellCount = 0;
			int gridNumber = 0;

			for (int r = 0; r < rows; r++)
			{
				tr = new TableRow
				{
					ID = $"Row_{r}"
				};

				for (int c = 0; c < columns; c++)
				{
					cellCount++;
					var tc = new TableCell
					{
						ID = $"Cell_{r}_{c}"
					};

					string chkId = $"Chk_{r}_{c}";

					if (ThisStep == 2)
					{
						SetBackground(tc, r, c, ref gridNumber);

						// Add checkboxes to set blank cells
						var chk = new CheckBox
						{
							ID = chkId,
						};
						chk.Attributes.Add("onclick", $"cellCheck('{chk.ID}');");
						//chk.CheckedChanged += new EventHandler(CheckChange);
						//upCallback.ContentTemplateContainer.Controls.Add(chk);
						//upCallback.Triggers.Add(new AsyncPostBackTrigger
						//{
						//	ControlID = chk.UniqueID,
						//	EventName = "CheckedChanged"
						//});
						//ScriptManager1.RegisterAsyncPostBackControl(chk);
						tc.Controls.Add(chk);
					}
					else if (ThisStep == 3)
					{
						SetBackground(tc, r, c, ref gridNumber);

						// Add blank cell if checked, otherwise add textbox and number if beginning of new answer
						if (Request.Form[chkId] == null)
						{
							var txt = new TextBox
							{
								ID = $"Text_{r}_{c}",
								MaxLength = 1
							};
							//txt.Attributes.Add("onclick", $"textClick('{txt.ID}');");
							tc.Controls.Add(txt);
						}
					}

					tr.Cells.Add(tc);
					tblGrid.Rows.Add(tr);
				}
			}
		}

		protected void CheckChange(object sender, EventArgs e)
		{

		}

		//private void RegisterCheckboxes(List<string> CheckBoxIDs)
		//{
		//	foreach(string chkId in CheckBoxIDs)
		//	{
		//		CheckBox chk = (CheckBox)upCallback.FindControl(chkId);
		//		if (chk != null)
		//		{
		//			//ScriptManager1.RegisterPostBackControl(chk);
		//			//ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(chk);

		//			upCallback.Triggers.Add(new PostBackTrigger
		//			{
		//				ControlID = chk.UniqueID
		//				//EventName = "Click"
		//			});
		//		}
		//	}
		//}

		private void SetBackground(TableCell tc, int row, int cell, ref int number)
		{
			string chkId = $"Chk_{row}_{cell}";

			if (Request.Form[chkId]?.ToLower() == "on")
			{
				// This cell is blank
				tc.BackColor = Color.Black;
			}
			else
			{
				/* This cell is not blank. Add a number to...
					- all cells in first row
					- all cells in first column
					- any other cell with a blank to the left but not to the right, or a blank above but not below
				*/
				string leftChkId = $"Chk_{row}_{cell - 1}";      // Cell to the left of this
				string rightChkId = $"Chk_{row}_{cell + 1}";     // Cell to the right of this
				string prevRowChkId = $"Chk_{row - 1}_{cell}";   // Cell above this row
				string nextRowChkId = $"Chk_{row + 1}_{cell}";   // Cell below this row

				if (row == 0 || cell == 0 ||
					(Request.Form[leftChkId] != null && Request.Form[rightChkId] == null) ||
					(Request.Form[prevRowChkId] != null && Request.Form[nextRowChkId] == null))
				{
					number++;
					// Add background SVG as inline style
					tc.Attributes["style"] = Server.HtmlEncode(SimpleSvg.GetGridCss(number));
				}
			}
		}

		//private string CreateCss(int count)
		//{
		//	// See https://www.svgbackgrounds.com/how-to-add-svgs-with-css-background-image/
		//	string filename = "Grid.css"; //Guid.NewGuid().ToString();
		//	string path = Path.Combine(Server.MapPath("/"), filename);

		//	if (!File.Exists(path))
		//		using (var outputFile = new StreamWriter(path))
		//			for (int i = 1; i <= count; i++)
		//			{
		//				//string svg = SvgService.CreateGridSvg(i)
		//				string svg = SimpleSvg.CreateGridNumber(i)
		//					.Replace("<?", "<%3F")
		//					.Replace("?>", "%3F>")
		//					.Replace("\\", "")
		//					.Replace("\r\n", "")
		//					.Replace("  ", " ")
		//					.Replace("  ", " ");
		//				//.Replace(" <", "<")
		//				//.Replace("> ", ">");
		//				string css = $".grid_bg_{i} {{background-image: url('data:image/svg+xml,{svg}');}}";
		//				outputFile.WriteLine(css);
		//			}
		//	return filename;
		//}

		//private void createBackgroundImages(int count)
		//{
		//	for (int i = 1; i <= count; i++)
		//	{
		//		string path = Path.Combine(ImagesPath, $"{i}.svg");
		//		if (!File.Exists(path))
		//			SvgService.CreateGridNumber(path, i);
		//	}
		//}
	}
}