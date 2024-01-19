using Svg;

using System.Drawing;

namespace Crossword
{
	public static class SvgService
	{
		public static string CreateGridSvg(int number)
		{
			var svgDoc = new SvgDocument
			{
				Width = 20,
				Height = 20
				//ViewBox = new SvgViewBox(-250, -250, 500, 500)
			};

			var group = new SvgGroup();
			svgDoc.Children.Add(group);

			group.Children.Add(new SvgText
			{
				X = new SvgUnitCollection { new SvgUnit(SvgUnitType.Pixel, 1) },
				Y = new SvgUnitCollection { new SvgUnit(SvgUnitType.Pixel, 7) },
				Fill = new SvgColourServer(Color.Gray),
				//Stroke = new SvgColourServer(Color.Gray),
				//StrokeWidth = new SvgUnit(SvgUnitType.Pixel, 1),
				Font = "Arial",
				//FontFamily = "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;",
				FontSize = new SvgUnit(SvgUnitType.Pixel, 8),
				//Color = new SvgColourServer(Color.Red)
				Text = number.ToString()
			});

			//group.Children.Add(new SvgCircle
			//{
			//	Radius = 100,
			//	Fill = new SvgColourServer(Color.Red),
			//	Stroke = new SvgColourServer(Color.Black),
			//	StrokeWidth = 2
			//});

			//svgDoc.Write(path);
			return svgDoc.GetXML();
		}
	}
}