using System;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Crossword
{
	public partial class Play : System.Web.UI.Page
	{
		//public const int GridSize = 30;
		public const int MaxGridArea = 100000;
		public const int CheckSize = 18;
		public const int TextSize = 18;
		public const int MinRows = 5;
		public const int MinColumns = 5;

		public int ThisStep = 1;
		//public string ImagesPath = "";
		
		protected void Page_Load(object sender, EventArgs e)
		{
			//string root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly()?.Location ?? string.Empty);
			//ImagesPath = Path.Combine(Server.MapPath("/"), "Images/Grid");
			//if (!Directory.Exists(ImagesPath))
			//	Directory.CreateDirectory(ImagesPath);
			
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

			//string cssPath = CreateCss(rows * columns);
			//ClientScript.RegisterStartupScript(base.GetType(), "GridStyle", $"<link rel=\"stylesheet\" href=\"{cssPath}\" />");

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
						ID = $"Cell_{r}_{c}",
						CssClass = "cell"
					};

					string chkId = $"Chk_{r}_{c}";

					if (ThisStep == 2)
					{
						// Add checkboxes to set blank cells
						var chk = new CheckBox
						{
							ID = chkId,
							Height = Unit.Pixel(CheckSize),
							Width = Unit.Pixel(CheckSize),
							BorderStyle = BorderStyle.None,
							CssClass = "cell"
						};
						//chk.Attributes.Add("onclick", $"cellClick('{txt.ID}');");
						tc.Controls.Add(chk);
					}
					else if (ThisStep == 3)
					{
						// Add blank cell if checked, otherwise add textbox and number if beginning of new answer
						if (Request.Form[chkId]?.ToLower() == "on")
						{
							// This cell is blank
							tc.BackColor = Color.Black;
						}
						else
						{
							//tc.Attributes["class"] = $"grid_bg_{cellNum}";
							
							string leftChkId = $"Chk_{r}_{c - 1}";      // Cell to the left of this
							string rightChkId = $"Chk_{r}_{c + 1}";      // Cell to the right of this
							string prevRowChkId = $"Chk_{r - 1}_{c}";	// Cell above this row
							string nextRowChkId = $"Chk_{r + 1}_{c}";   // Cell below this row

							/* This cell is not blank. Add a number to...
								- all cells in first row
								- all cells in first column
								- any other cell with a blank to the left but not to the right, or a blank above but not below
							*/
							if (r == 0 || c == 0 ||
								(Request.Form[leftChkId] != null && Request.Form[rightChkId] == null) ||
								(Request.Form[prevRowChkId] != null && Request.Form[nextRowChkId] == null))
							{
								gridNumber++;
								// Add background SVG as inline style
								tc.Attributes["style"] = Server.HtmlEncode(SimpleSvg.GetGridCss(gridNumber));
							}
							
							var txt = new TextBox
							{
								ID = $"Text_{r}_{c}",
								Text = "",
								MaxLength = 1,
								Height = Unit.Pixel(TextSize),
								Width = Unit.Pixel(TextSize),
								BorderStyle = BorderStyle.None,
								CssClass = "cell"
							};
							//txt.Attributes.Add("onclick", $"cellClick('{txt.ID}');");
							tc.Controls.Add(txt);
						}
					}

					tr.Cells.Add(tc);
					tblGrid.Rows.Add(tr);
				}
			}
		}

		private string CreateCss(int count)
		{
			// See https://www.svgbackgrounds.com/how-to-add-svgs-with-css-background-image/
			string filename = "Grid.css"; //Guid.NewGuid().ToString();
			string path = Path.Combine(Server.MapPath("/"), filename);

			if (!File.Exists(path))
				using (var outputFile = new StreamWriter(path))
					for (int i = 1; i <= count; i++)
					{
						//string svg = SvgService.CreateGridSvg(i)
						string svg = SimpleSvg.CreateGridNumber(i)
							.Replace("<?", "<%3F")
							.Replace("?>", "%3F>")
							.Replace("\\", "")
							.Replace("\r\n", "")
							.Replace("  ", " ")
							.Replace("  ", " ");
						//.Replace(" <", "<")
						//.Replace("> ", ">");
						string css = $".grid_bg_{i} {{background-image: url('data:image/svg+xml,{svg}');}}";
						outputFile.WriteLine(css);
					}
			return filename;
		}

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