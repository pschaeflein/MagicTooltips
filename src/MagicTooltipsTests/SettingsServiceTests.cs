using MagicTooltips.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace MagicTooltipsTests
{
	[TestClass]
	public class SettingsServiceTests
	{
		[DataTestMethod]
		[DataRow("left", "left")]
		[DataRow("right", "right")]
		[DataRow(null, "right")]
		public void Populate_HorizontalAlignment(string input, string expected)
		{
			var hash = new Hashtable
						{
								{"HorizontalAlignment", input }
						};

			SettingsService.Populate(hash);

			Assert.AreEqual(expected, SettingsService.Settings.HorizontalAlignment.ToString(), true);
		}

		[DataTestMethod]
		[DataRow(true, true)]
		[DataRow(false, false)]
		[DataRow(null, false)]
		public void Populate_Debug(bool? input, bool expected)
		{
			var hash = new Hashtable
						{
								{"Debug", input }
						};

			SettingsService.Populate(hash);

			Assert.AreEqual(expected, SettingsService.Settings.Debug);
		}

		[DataTestMethod]
		[DataRow(99, 99)]
		[DataRow(-99, -99)]
		[DataRow(null, 0)]
		public void Populate_Horizontaloffset(int? input, int expected)
		{
			var hash = new Hashtable
						{
								{"HorizontalOffset", input }
						};

			SettingsService.Populate(hash);

			Assert.AreEqual(expected, SettingsService.Settings.HorizontalOffset);
		}

		[DataTestMethod]
		[DataRow("foo", "foo")]
		[DataRow(null, "kubectl,helm,kubens,kubectx,oc,istioctl,kogito,k9s,helmlist")]
		public void Populate_KubernetesCommands(string input, string expected)
		{
			var hash = new Hashtable
						{
								{ "Providers", new Hashtable
										{
												{ "kubernetes", new Hashtable
														{
																{ "Commands", input }
														}
												}
										}
								}
						};

			SettingsService.Populate(hash);

			Assert.AreEqual(expected, SettingsService.Settings.Providers["kubernetes"].Commands);
		}
	}
}
