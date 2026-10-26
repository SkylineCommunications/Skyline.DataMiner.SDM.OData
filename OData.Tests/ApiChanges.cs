namespace Skyline.DataMiner.SDM.OData.Tests
{

	[TestClass]
	public partial class ApiChanges
	{
		[TestMethod]
		public void PublicApiShouldNotContainUnshippedEntries()
		{
			using Stream? stream = typeof(ApiChanges).Assembly.GetManifestResourceStream("PublicAPI.Unshipped.txt");

			if (stream is null)
			{
				Assert.Fail("The public API baseline resource was not found.");
				return;
			}

			using var reader = new StreamReader(stream);
			var unshippedEntries = reader.ReadToEnd()
				.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
				.Where(line => !String.IsNullOrWhiteSpace(line))
				.Where(line => !line.TrimStart().StartsWith("#", StringComparison.Ordinal))
				.ToList();

			Assert.AreEqual(
				0,
				unshippedEntries.Count,
				$"The public API contains unshipped entries:{Environment.NewLine}{String.Join(Environment.NewLine, unshippedEntries)}");
		}
	}
}
