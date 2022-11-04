using MagicTooltips.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTooltipsTests
{
	[TestClass]
	public class TriggerServiceTests
	{
		[DataTestMethod]
		[DataRow("Get-MgApplication", "mgapplication" )]
		[DataRow("Get-MgApplication -ApplicationId c1ca6040-d0ae-493d-9b48-d35018390ea2 ", "mgapplication", "d0ae-493d-9b48-d35018390ea2")]
		[DataRow("Foreach-Object -Process { Get-MgApplication -ApplicationId c1ca6040-d0ae-493d-9b48-d35018390ea2 }", "object", "mgapplication", "d0ae-493d-9b48-d35018390ea2")]
    [DataRow("Get-AzAdApplication", "azadapplication")]
		public void ParseLine(string input, params string[] expected)
		{
			var actual = TriggerService.ParseLine(input).ToArray();
			CollectionAssert.AreEqual(expected, actual);
		}

	}
}
