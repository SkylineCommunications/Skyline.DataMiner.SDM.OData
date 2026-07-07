namespace Skyline.DataMiner.SDM.OData.Tests
{
	using System.Threading.Tasks;

	using PublicApiGenerator;

	[TestClass]
	[UsesVerify]
	public partial class ApiChanges
	{
		[TestMethod]
		public Task PublicChanges()
		{
			var assembly = typeof(ODataSdmTranslator<>).Assembly;
			var publicApi = assembly.GeneratePublicApi();

			return Verify(publicApi)
				.UseFileName("Skyline.DataMiner.SDM.OData");
		}
	}
}
