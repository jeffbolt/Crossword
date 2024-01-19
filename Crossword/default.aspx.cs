using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Crossword
{
	public partial class _default : System.Web.UI.Page
	{
		public const int SquareSize = 20;
		public const int MaxGridArea = 100000;
		public const int MinRows = 5;
		public const int MinColumns = 5;

		public int step = 0;
		public string ImagesPath = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			//string root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly()?.Location ?? string.Empty);
			ImagesPath = Path.Combine(Server.MapPath("/"), "Images/Grid");
			if (!Directory.Exists(ImagesPath))
				Directory.CreateDirectory(ImagesPath);

			if (Page.IsPostBack)
			{
				if (!int.TryParse(Step.Value, out step))
					step = 1;
			}

			switch (step)
			{
				case 1:
					Step1();
					break;
				case 2:
					Step2();
					break;
				//case 3:
				//	Step3();
				//	break;
			}
		}

		private void Step1()
		{
			pnlGrid.Visible = false;
			pnlStep1.Visible = true;
		}

		//protected void btnUpdate_Click(object sender, EventArgs e)
		private void Step2()
		{
			if (!int.TryParse(txtRows.Text, out int rows) || rows < MinRows)
			{
				showError("Invalid number of <b>Rows</b>.");
				return;
			}

			if (!int.TryParse(txtColumns.Text, out int columns) || columns < MinColumns)
			{
				showError("Invalid number of <b>Columns</b>.");
				return;
			}

			if (rows * columns > MaxGridArea)
			{
				showError($"Grid area cannot exceed {MaxGridArea:n0}.");
				return;
			}

			BuildGrid(rows, columns);
		}

		private void showError(string message)
		{
			pnlError.Visible = false;
			lblError.Text = message;
		}

		private void BuildGrid(int rows, int columns)
		{
			pnlGrid.Visible = true;
			Grid.Rows.Clear();

			string cssPath = createBackgroundImageCss(rows * columns);
			ClientScript.RegisterStartupScript(base.GetType(), "GridStyle", $"<link rel=\"stylesheet\" href=\"{cssPath}\" />");

			TableRow tr;
			int cellCount = 0;

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
						//Height = Unit.Pixel(SquareSize),
						//Width = Unit.Pixel(SquareSize),
						//BackColor = Color.White,
						//Text = $"{h},{w}" //"&nbsp;"
					};

					// TODO: Find a library to minimize?
					//tc.Attributes["style"] = $"background-image: url('/Images/Grid/{cellCount}.svg')";
					tc.Attributes["class"] = $"grid_bg_{cellCount}";

					//cellClick
					var txt = new TextBox {
						ID = $"Text_{r}_{c}",
						Text = "",
						MaxLength = 1,
						Height = Unit.Pixel(SquareSize),
						Width = Unit.Pixel(SquareSize),
						BorderStyle = BorderStyle.None,
						CssClass = "cell"
					};
					txt.Attributes.Add("onclick", $"cellClick('{txt.ID}');");
					//tc.Controls.Add(txt);
					tr.Cells.Add(tc);
					Grid.Rows.Add(tr);
				}
			}
		}

		private string createBackgroundImageCss(int count)
		{
			// See https://www.svgbackgrounds.com/how-to-add-svgs-with-css-background-image/
			string filename = "Grid.css"; //Guid.NewGuid().ToString();
			string path = Path.Combine(Server.MapPath("/"), filename);
			if (!File.Exists(path))
				using (var outputFile = new StreamWriter(path))
					for (int i = 1; i <= count; i++)
					{
						string svg = SvgService.CreateGridSvg(i)
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

		protected void Step_ValueChanged(object sender, EventArgs e)
		{

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