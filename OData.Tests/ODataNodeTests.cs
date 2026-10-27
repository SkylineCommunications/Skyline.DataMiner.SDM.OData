namespace Skyline.DataMiner.SDM.OData.Tests
{
	using FluentAssertions;

	using Skyline.DataMiner.SDM.OData;

	[TestClass]
	public class ODataNodeTests
	{
		[TestMethod]
		public void FunctionCall_WithArgument_ReplacesArguments()
		{
			var original = new FunctionCallNode(
				"contains",
				new ODataNode[] { new PropertyNode("name") });

			var updated = original.WithArgument(
				new ODataNode[] { new PropertyNode("Name") });

			updated.Should().NotBeSameAs(original);
			updated.Arguments.Should().ContainSingle();
			updated.Arguments[0].As<PropertyNode>().Path.Should().Be("Name");
			original.Arguments[0].As<PropertyNode>().Path.Should().Be("name");
		}

		[TestMethod]
		public void FunctionCall_WithSameArguments_ReturnsSameInstance()
		{
			var original = new FunctionCallNode(
				"contains",
				new ODataNode[] { new PropertyNode("Name") });

			var updated = original.WithArgument(original.Arguments);

			updated.Should().BeSameAs(original);
		}
	}
}
