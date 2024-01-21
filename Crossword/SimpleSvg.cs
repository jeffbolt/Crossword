using System.IO;
using System.Web;
using System.Xml;

namespace Crossword
{
	public static class SimpleSvg
	{
		public const string SvgGridNumber = @"url('data:image/svg+xml,<%3Fxml version=""1.0"" encoding=""utf-8""%3F><!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd""><svg version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:xml=""http://www.w3.org/XML/1998/namespace"" width=""30"" height=""30""><g><text x=""1px"" y=""7px"" font-family=""Arial"" font-size=""8px"" font=""Arial"" style=""fill:gray;"">{0}</text></g></svg>');";

		public static string GetGridCss(int number)
		{
			string svg = HttpContext.Current.Server.HtmlEncode(string.Format(SvgGridNumber, number.ToString()));
			return svg;
		}

		public static string CreateGridNumber(int number)
		{
			//string root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly()?.Location ?? string.Empty);
			string root = System.Web.HttpContext.Current.Server.MapPath("App_Data");
			var doc = new XmlDocument();
			doc.Load(Path.Combine(root, "SvgGridNumber.xml"));
			string svg = string.Format(doc.InnerXml, number.ToString());
			return svg;
		}
	}
}