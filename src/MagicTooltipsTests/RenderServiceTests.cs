using MagicTooltips.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MagicTooltipsTests
{
	[TestClass]
	public class RenderServiceTests
	{
		[DataTestMethod]
		[DataRow("", "#ff0000", "#ffffff", "")]
		[DataRow(null, "#ff0000", "#ffffff", "")]
		[DataRow("foo", "#ff0000", "#ffffff", "\x1b[38;2;255;0;0;48;2;255;255;255mfoo\x1b[0m")]
		[DataRow("foo", "#ff0000", "", "\x1b[38;2;255;0;0mfoo\x1b[0m")]
		[DataRow("foo", "", "#ffffff", "\x1b[48;2;255;255;255mfoo\x1b[0m")]
		public void GetColoredString(string text, string fgColor, string bgColor, string expected)
		{
			var output = RenderService.GetColoredString(text, fgColor, bgColor);
			Assert.AreEqual(expected, output);
		}

		[DataTestMethod]
		[DataRow("")]
		[DataRow(null)]
		[DataRow("foo")]
		[DataRow("000000")]
		[DataRow("#000")]
		[DataRow("#GGGGGG")]
		public void ConvertHexToRgb_Invalid(string input)
		{
			var output = RenderService.ConvertHexToRgb(input);
			Assert.IsNull(output);
		}

		[DataTestMethod]
		[DataRow("#FF0000", 255, 0, 0)]
		[DataRow("#ff0000", 255, 0, 0)]
		[DataRow("#00FF00", 0, 255, 0)]
		[DataRow("#0000FF", 0, 0, 255)]
		public void ConvertHexToRgb(string input, int expectedR, int expectedG, int expectedB)
		{
			var output = RenderService.ConvertHexToRgb(input);
			Assert.AreEqual((expectedR, expectedG, expectedB), output);
		}
	}
}
