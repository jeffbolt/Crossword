using System.Web.Services;

namespace Crossword
{
	/// <summary>
	/// Callback method to return SVG image data for CSS background
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	[System.Web.Script.Services.ScriptService]
	public class Callback : System.Web.Services.WebService
	{
		[WebMethod]
		public string GetGridCss(int number)
		{
			return SimpleSvg.GetGridCss(number);
		}
	}
}
