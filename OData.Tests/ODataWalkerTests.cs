namespace Skyline.DataMiner.SDM.OData.Tests
{
	using System.Collections.Generic;
	using System.Linq;

	using FluentAssertions;

	using Skyline.DataMiner.SDM.OData;

	[TestClass]
	public class ODataWalkerTests
	{
		[TestMethod]
		public void Walker_Visits_Not_Child()
		{
			// Arrange
			var tree = ODataFilterParser.Parse("not (Name eq 'Andi')");
			var walker = new RecordingWalker();

			// Act
			walker.Visit(tree);

			// Assert
			walker.Visited.OfType<PropertyNode>().Should().ContainSingle(node => node.Path == "Name");
		}

		[TestMethod]
		public void Walker_Visits_Order_Property()
		{
			// Arrange
			var tree = ODataOrderByParser.Parse("Name desc");
			var walker = new RecordingWalker();

			// Act
			walker.Visit(tree);

			// Assert
			walker.Visited.OfType<PropertyNode>().Should().ContainSingle(node => node.Path == "Name");
		}

		private sealed class RecordingWalker : ODataWalker
		{
			public List<ODataNode> Visited { get; } = new List<ODataNode>();

			public override void DefaultVisit(ODataNode node)
			{
				Visited.Add(node);
			}
		}
	}
}
