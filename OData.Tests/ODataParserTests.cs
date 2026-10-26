namespace Skyline.DataMiner.SDM.OData.Tests
{
	using System;

	using FluentAssertions;

	using Skyline.DataMiner.SDM.OData;
	using Skyline.DataMiner.SDM.OData.Exceptions;

	[TestClass]
	public class ODataParserTests
	{
		[TestMethod]
		public void Empty_Filter()
		{
			// Arrange
			var filter = "";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();
			tree.Filter.Should().BeNull();
			tree.Parent.Should().BeNull();
		}

		[TestMethod]
		public void String_Equal()
		{
			// Arrange
			var filter = "Name eq 'Andi Tamer'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<BinaryNode>();
			filterNode.Parent.Should().BeNull();

			var binaryNode = filterNode.Filter.As<BinaryNode>();
			binaryNode.Operation.Should().Be(ODataLogicalOperation.Equal);
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Name");
			binaryNode.Right.Should().BeOfType<LiteralNode>();
			binaryNode.Right.As<LiteralNode>().Value.Should().Be("Andi Tamer");
			binaryNode.Parent.Should().Be(filterNode);
		}

		[TestMethod]
		public void String_Equal_Inverted()
		{
			// Arrange
			var filter = "not Name eq 'Andi Tamer'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<NotNode>();
			filterNode.Parent.Should().BeNull();

			var notNode = filterNode.Filter.As<NotNode>();
			notNode.Child.Should().BeOfType<BinaryNode>();
			notNode.Parent.Should().Be(filterNode);

			var binaryNode = notNode.Child.As<BinaryNode>();
			binaryNode.Operation.Should().Be(ODataLogicalOperation.Equal);
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Name");
			binaryNode.Right.Should().BeOfType<LiteralNode>();
			binaryNode.Right.As<LiteralNode>().Value.Should().Be("Andi Tamer");
			binaryNode.Parent.Should().Be(notNode);
		}

		[TestMethod]
		public void String_StartsWith()
		{
			// Arrange
			var filter = "startswith(Name, 'Andi')";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.As<FunctionCallNode>();
			filterNode.Parent.Should().BeNull();

			var functionCallNode = filterNode.Filter.As<FunctionCallNode>();
			functionCallNode.Name.Should().Be("startswith");
			functionCallNode.Arguments[0].Should().BeOfType<PropertyNode>();
			functionCallNode.Arguments[0].As<PropertyNode>().Path.Should().Be("Name");
			functionCallNode.Arguments[1].Should().BeOfType<LiteralNode>();
			functionCallNode.Arguments[1].As<LiteralNode>().Value.Should().Be("Andi");
			functionCallNode.Parent.Should().Be(filterNode);
		}

		[TestMethod]
		public void String_Contains()
		{
			// Arrange
			var filter = "contains(Name, 'Andi')";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<FunctionCallNode>();
			filterNode.Parent.Should().BeNull();

			var functionCallNode = filterNode.Filter.As<FunctionCallNode>();
			functionCallNode.Name.Should().Be("contains");
			functionCallNode.Arguments[0].Should().BeOfType<PropertyNode>();
			functionCallNode.Arguments[0].As<PropertyNode>().Path.Should().Be("Name");
			functionCallNode.Arguments[1].Should().BeOfType<LiteralNode>();
			functionCallNode.Arguments[1].As<LiteralNode>().Value.Should().Be("Andi");
			functionCallNode.Parent.Should().Be(filterNode);
		}

		[TestMethod]
		public void Number_NotEqual()
		{
			// Arrange
			var filter = "Age ne 25";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<BinaryNode>();
			filterNode.Parent.Should().BeNull();

			var binaryNode = filterNode.Filter.As<BinaryNode>();
			binaryNode.Operation.Should().Be(ODataLogicalOperation.NotEqual);
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Age");
			binaryNode.Right.Should().BeOfType<LiteralNode>();
			binaryNode.Right.As<LiteralNode>().Value.Should().Be(25);
			binaryNode.Parent.Should().Be(filterNode);
		}

		[TestMethod]
		public void Number_GreaterThan()
		{
			// Arrange
			var filter = "Age gt 25";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<BinaryNode>();
			filterNode.Parent.Should().BeNull();

			var binaryNode = filterNode.Filter.As<BinaryNode>();
			binaryNode.Operation.Should().Be(ODataLogicalOperation.GreaterThan);
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Age");
			binaryNode.Right.Should().BeOfType<LiteralNode>();
			binaryNode.Right.As<LiteralNode>().Value.Should().Be(25);
			binaryNode.Parent.Should().Be(filterNode);
		}

		[TestMethod]
		public void Number_GreaterThanOrEqual()
		{
			// Arrange
			var filter = "Age ge 25";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<BinaryNode>();
			filterNode.Parent.Should().BeNull();

			var binaryNode = filterNode.Filter.As<BinaryNode>();
			binaryNode.Operation.Should().Be(ODataLogicalOperation.GreaterThanOrEqual);
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Age");
			binaryNode.Right.Should().BeOfType<LiteralNode>();
			binaryNode.Right.As<LiteralNode>().Value.Should().Be(25);
			binaryNode.Parent.Should().Be(filterNode);
		}

		[TestMethod]
		public void Number_LessThan()
		{
			// Arrange
			var filter = "Age lt 25";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<BinaryNode>();
			filterNode.Parent.Should().BeNull();

			var binaryNode = filterNode.Filter.As<BinaryNode>();
			binaryNode.Operation.Should().Be(ODataLogicalOperation.LessThan);
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Age");
			binaryNode.Right.Should().BeOfType<LiteralNode>();
			binaryNode.Right.As<LiteralNode>().Value.Should().Be(25);
			binaryNode.Parent.Should().Be(filterNode);
		}

		[TestMethod]
		public void Number_LessThanOrEqual()
		{
			// Arrange
			var filter = "Age le 25";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<BinaryNode>();
			filterNode.Parent.Should().BeNull();

			var binaryNode = filterNode.Filter.As<BinaryNode>();
			binaryNode.Operation.Should().Be(ODataLogicalOperation.LessThanOrEqual);
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Age");
			binaryNode.Right.Should().BeOfType<LiteralNode>();
			binaryNode.Right.As<LiteralNode>().Value.Should().Be(25);
			binaryNode.Parent.Should().Be(filterNode);
		}

		[TestMethod]
		public void And()
		{
			// Arrange
			var filter = "'Andi Tamer' eq Name and Age ge 25";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<AndNode>();

			var andNode = filterNode.Filter.As<AndNode>();
			andNode.Nodes.Should().HaveCount(2);

			foreach (var node in andNode.Nodes)
			{
				node.Parent.Should().Be(andNode);
			}
		}

		[TestMethod]
		public void Group_And()
		{
			// Arrange
			var filter = "Age ge 25 and (Heigth gt 160 and Heigth le 180)";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<AndNode>();
			filterNode.Parent.Should().BeNull();

			var andNode = filterNode.Filter.As<AndNode>();
			tree.Filter.As<AndNode>().Nodes.Should().HaveCount(2);

			var firstNode = andNode.Nodes.ElementAt(0);
			firstNode.Should().BeOfType<BinaryNode>();
			firstNode.Parent.Should().Be(andNode);

			var secondNode = andNode.Nodes.ElementAt(1);
			secondNode.Should().BeOfType<AndNode>();
			secondNode.Parent.Should().Be(andNode);
			secondNode.As<AndNode>().Nodes.Should().HaveCount(2);

			var secondNodeFirstChild = secondNode.As<AndNode>().Nodes.ElementAt(0);
			secondNodeFirstChild.Should().BeOfType<BinaryNode>();
			secondNodeFirstChild.Parent.Should().Be(secondNode);
			var secondNodeSecondChild = secondNode.As<AndNode>().Nodes.ElementAt(1);
			secondNodeSecondChild.Should().BeOfType<BinaryNode>();
			secondNodeSecondChild.Parent.Should().Be(secondNode);
		}

		[TestMethod]
		public void Or()
		{
			// Arrange
			var filter = "'Andi Tamer' eq Name or Age ge 25";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();

			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<OrNode>();
			filterNode.Parent.Should().BeNull();

			var orNode = filterNode.Filter.As<OrNode>();
			orNode.Nodes.Should().HaveCount(2);

			var firstNode = orNode.Nodes.ElementAt(0);
			firstNode.Should().BeOfType<BinaryNode>();
			firstNode.Parent.Should().Be(orNode);
			var secondNode = orNode.Nodes.ElementAt(1);
			secondNode.Should().BeOfType<BinaryNode>();
			secondNode.Parent.Should().Be(orNode);
		}

		[TestMethod]
		public void Collection_Any()
		{
			// Arrange
			var filter = "ExternalIdentifiers/any(e: e/ExternalName eq 'Andi Tamer')";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();
			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<MethodCallNode>();

			var methodCallNode = tree.Filter.As<MethodCallNode>();
			methodCallNode.Target.Should().BeOfType<PropertyNode>();
			methodCallNode.Variable.Should().BeOfType<PropertyNode>();
			methodCallNode.Body.Should().BeOfType<BinaryNode>();
			methodCallNode.Parent.Should().Be(filterNode);

			var binaryNode = methodCallNode.Body.As<BinaryNode>();
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("e/ExternalName");
			binaryNode.Operation.Should().Be(ODataLogicalOperation.Equal);
			binaryNode.Right.Should().BeOfType<LiteralNode>();
			binaryNode.Right.As<LiteralNode>().Type.Should().Be(ODataPrimitiveType.String);
			binaryNode.Right.As<LiteralNode>().Value.Should().Be("Andi Tamer");
			binaryNode.Parent.Should().Be(methodCallNode);
		}

		[TestMethod]
		public void Collection_Any_DOL()
		{
			//
			var id = Guid.NewGuid();
			var filter = $"ExternalIdentifiers/any(e: e/MyPersonTest eq '{id}')";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Should().BeOfType<FilterNode>();
			var filterNode = tree.As<FilterNode>();
			filterNode.Filter.Should().BeOfType<MethodCallNode>();

			var methodCallNode = tree.Filter.As<MethodCallNode>();
			methodCallNode.Target.Should().BeOfType<PropertyNode>();
			methodCallNode.Variable.Should().BeOfType<PropertyNode>();
			methodCallNode.Body.Should().BeOfType<BinaryNode>();
			methodCallNode.Parent.Should().Be(filterNode);

			var binaryNode = methodCallNode.Body.As<BinaryNode>();
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("e/MyPersonTest");
			binaryNode.Operation.Should().Be(ODataLogicalOperation.Equal);
			binaryNode.Right.Should().BeOfType<LiteralNode>();
			binaryNode.Right.As<LiteralNode>().Type.Should().Be(ODataPrimitiveType.String);
			binaryNode.Right.As<LiteralNode>().Value.Should().Be(id.ToString());
			binaryNode.Parent.Should().Be(methodCallNode);
		}

		[TestMethod]
		public void SubProperty_Path_Outside_Lambda_Is_Kept_As_Single_Path()
		{
			// Arrange
			var filter = "Mother/Identifier eq 'Person 001'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var binaryNode = tree.Filter.As<BinaryNode>();
			binaryNode.Left.Should().BeOfType<PropertyNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Mother/Identifier");
		}

		[TestMethod]
		public void SubProperty_Path_With_Multiple_Segments()
		{
			// Arrange
			var filter = "Mother/Mother/Identifier eq 'Person 001'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var binaryNode = tree.Filter.As<BinaryNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Mother/Mother/Identifier");
		}

		[TestMethod]
		public void Whitespace_Is_Optional_Around_Punctuation()
		{
			// Arrange
			var filter = "Name eq'Andi'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var binaryNode = tree.Filter.As<BinaryNode>();
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Name");
			binaryNode.Right.As<LiteralNode>().Value.Should().Be("Andi");
		}

		[TestMethod]
		public void Whitespace_Tabs_And_Newlines_Are_Skipped()
		{
			// Arrange
			var filter = "Name\teq\n'Andi'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var binaryNode = tree.Filter.As<BinaryNode>();
			binaryNode.Operation.Should().Be(ODataLogicalOperation.Equal);
			binaryNode.Right.As<LiteralNode>().Value.Should().Be("Andi");
		}

		[TestMethod]
		public void Operator_Keywords_Are_Case_Insensitive()
		{
			// Arrange
			var filter = "Name EQ 'Andi'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var binaryNode = tree.Filter.As<BinaryNode>();
			binaryNode.Operation.Should().Be(ODataLogicalOperation.Equal);
			binaryNode.Left.As<PropertyNode>().Path.Should().Be("Name");
		}

		[TestMethod]
		public void Identifier_Casing_Is_Preserved()
		{
			// Arrange
			var filter = "nAmE eq 'Andi'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Filter.As<BinaryNode>().Left.As<PropertyNode>().Path.Should().Be("nAmE");
		}

		[TestMethod]
		public void String_With_Escaped_Quote()
		{
			// Arrange
			var filter = @"Name eq 'O\'Brien'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Filter.As<BinaryNode>().Right.As<LiteralNode>().Value.Should().Be("O'Brien");
		}

		[TestMethod]
		public void String_With_Escaped_Newline_And_Tab()
		{
			// Arrange
			var filter = @"Name eq 'Line1\nLine2\tEnd'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Filter.As<BinaryNode>().Right.As<LiteralNode>().Value.Should().Be("Line1\nLine2\tEnd");
		}

		[TestMethod]
		public void Empty_String_Literal()
		{
			// Arrange
			var filter = "Name eq ''";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			tree.Filter.As<BinaryNode>().Right.As<LiteralNode>().Value.Should().Be(String.Empty);
		}

		[TestMethod]
		public void Null_Literal()
		{
			// Arrange
			var filter = "Name eq null";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var literal = tree.Filter.As<BinaryNode>().Right.As<LiteralNode>();
			literal.Type.Should().Be(ODataPrimitiveType.Null);
			literal.Value.Should().BeNull();
		}

		[TestMethod]
		public void Number_Decimal_With_Dot()
		{
			// Arrange
			var filter = "Age gt 1.5";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var literal = tree.Filter.As<BinaryNode>().Right.As<LiteralNode>();
			literal.Type.Should().Be(ODataPrimitiveType.Double);
			literal.Value.Should().Be(1.5);
		}

		[TestMethod]
		public void Number_Decimal_With_Comma()
		{
			// Arrange
			var filter = "Age gt 1,5";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var literal = tree.Filter.As<BinaryNode>().Right.As<LiteralNode>();
			literal.Type.Should().Be(ODataPrimitiveType.Double);
			literal.Value.Should().Be(1.5);
		}

		[TestMethod]
		public void Number_Integer_Type_Is_Int64()
		{
			// Arrange
			var filter = "Age gt 25";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var literal = tree.Filter.As<BinaryNode>().Right.As<LiteralNode>();
			literal.Type.Should().Be(ODataPrimitiveType.Int64);
			literal.Value.Should().Be(25L);
		}

		[TestMethod]
		public void Negative_Number()
		{
			// Arrange
			var filter = "Age gt -5";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var literal = tree.Filter.As<BinaryNode>().Right.As<LiteralNode>();
			literal.Type.Should().Be(ODataPrimitiveType.Int64);
			literal.Value.Should().Be(-5L);
		}

		[TestMethod]
		public void Negative_Decimal_Number()
		{
			// Arrange
			var filter = "Age gt -1.5";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var literal = tree.Filter.As<BinaryNode>().Right.As<LiteralNode>();
			literal.Type.Should().Be(ODataPrimitiveType.Double);
			literal.Value.Should().Be(-1.5);
		}

		[TestMethod]
		public void Unterminated_String_Throws()
		{
			// Arrange
			var filter = "Name eq 'Andi";
			// Act
			Action act = () => ODataFilterParser.Parse(filter);

			// Assert
			act.Should().Throw<ODataParseException>();
		}

		[TestMethod]
		public void Unexpected_Character_Throws()
		{
			// Arrange
			var filter = "Name eq #Andi";
			// Act
			Action act = () => ODataFilterParser.Parse(filter);

			// Assert
			act.Should().Throw<ODataParseException>();
		}

		[TestMethod]
		public void Missing_Closing_Parenthesis_In_Group_Throws()
		{
			// Arrange
			var filter = "(Name eq 'Andi'";
			// Act
			Action act = () => ODataFilterParser.Parse(filter);

			// Assert
			act.Should().Throw<ODataParseException>();
		}

		[TestMethod]
		public void Missing_Closing_Parenthesis_In_Function_Throws()
		{
			// Arrange
			var filter = "contains(Name, 'Andi'";
			// Act
			Action act = () => ODataFilterParser.Parse(filter);

			// Assert
			act.Should().Throw<ODataParseException>();
		}

		[TestMethod]
		public void Trailing_Garbage_After_Filter_Throws()
		{
			// Arrange
			var filter = "Name eq 'Andi' Age";
			// Act
			Action act = () => ODataFilterParser.Parse(filter);

			// Assert
			act.Should().Throw<ODataParseException>();
		}

		[TestMethod]
		public void Empty_Group_Throws()
		{
			// Arrange
			var filter = "()";
			// Act
			Action act = () => ODataFilterParser.Parse(filter);

			// Assert
			act.Should().Throw<NotSupportedException>();
		}

		[TestMethod]
		public void And_Binds_Tighter_Than_Or()
		{
			// Arrange
			var filter = "Name eq 'Andi' or Age ge 25 and Age le 30";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var orNode = tree.Filter.As<OrNode>();
			orNode.Nodes.Should().HaveCount(2);
			orNode.Nodes.ElementAt(0).Should().BeOfType<BinaryNode>();
			orNode.Nodes.ElementAt(1).Should().BeOfType<AndNode>();
			orNode.Nodes.ElementAt(1).As<AndNode>().Nodes.Should().HaveCount(2);
		}

		[TestMethod]
		public void Multiple_And_Conditions_Are_Flattened()
		{
			// Arrange
			var filter = "Name eq 'Andi' and Age ge 25 and Age le 130";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var andNode = tree.Filter.As<AndNode>();
			andNode.Nodes.Should().HaveCount(3);
			foreach (var node in andNode.Nodes)
			{
				node.Parent.Should().Be(andNode);
			}
		}

		[TestMethod]
		public void Multiple_Or_Conditions_Are_Flattened()
		{
			// Arrange
			var filter = "Age eq 1 or Age eq 2 or Age eq 3";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var orNode = tree.Filter.As<OrNode>();
			orNode.Nodes.Should().HaveCount(3);
		}

		[TestMethod]
		public void Double_Not()
		{
			// Arrange
			var filter = "not not Name eq 'Andi'";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var outerNot = tree.Filter.As<NotNode>();
			var innerNot = outerNot.Child.As<NotNode>();
			innerNot.Child.Should().BeOfType<BinaryNode>();
		}

		[TestMethod]
		public void Not_On_Grouped_Expression()
		{
			// Arrange
			var filter = "not (Name eq 'Andi' and Age gt 25)";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var notNode = tree.Filter.As<NotNode>();
			notNode.Child.Should().BeOfType<AndNode>();
			notNode.Child.As<AndNode>().Nodes.Should().HaveCount(2);
		}

		[TestMethod]
		public void Collection_Any_With_Or_Inside_Lambda()
		{
			// Arrange
			var filter = "ExternalIdentifiers/any(e: e/ExternalID eq 'p002' or e/ExternalName eq 'EXT Andi Tamer')";
			// Act
			var tree = ODataFilterParser.Parse(filter);

			// Assert
			var methodCallNode = tree.Filter.As<MethodCallNode>();
			methodCallNode.Body.Should().BeOfType<OrNode>();
			methodCallNode.Body.As<OrNode>().Nodes.Should().HaveCount(2);
		}

		[TestMethod]
		public void OrderBy_Empty_Returns_None()
		{
			// Arrange
			var orderBy = String.Empty;
			// Act
			var node = ODataOrderByParser.Parse(orderBy);

			// Assert
			node.Order.Should().Be(ODataOrder.None);
			node.Next.Should().BeNull();
		}

		[TestMethod]
		public void OrderBy_Defaults_To_Ascending()
		{
			// Arrange
			var orderBy = "Name";
			// Act
			var node = ODataOrderByParser.Parse(orderBy);

			// Assert
			node.Order.Should().Be(ODataOrder.Ascending);
			node.Property.Path.Should().Be("Name");
			node.Next.Should().BeNull();
		}

		[TestMethod]
		public void OrderBy_Explicit_Descending()
		{
			// Arrange
			var orderBy = "Name desc";
			// Act
			var node = ODataOrderByParser.Parse(orderBy);

			// Assert
			node.Order.Should().Be(ODataOrder.Descending);
		}

		[TestMethod]
		public void OrderBy_Multiple_Properties_Are_Chained()
		{
			// Arrange
			var orderBy = "Age desc, Name asc";
			// Act
			var node = ODataOrderByParser.Parse(orderBy);

			// Assert
			node.Property.Path.Should().Be("Age");
			node.Order.Should().Be(ODataOrder.Descending);
			node.Next.Should().NotBeNull();
			node.Next!.Property.Path.Should().Be("Name");
			node.Next!.Order.Should().Be(ODataOrder.Ascending);
			node.Next!.Next.Should().BeNull();
		}

		[TestMethod]
		public void OrderBy_Trailing_Comma_Throws()
		{
			// Arrange
			var orderBy = "Name,";
			// Act
			Action act = () => ODataOrderByParser.Parse(orderBy);

			// Assert
			act.Should().Throw<ODataParseException>();
		}

		[TestMethod]
		public void OrderBy_Missing_Property_Throws()
		{
			// Arrange
			var orderBy = "123 asc";
			// Act
			Action act = () => ODataOrderByParser.Parse(orderBy);

			// Assert
			act.Should().Throw<ODataParseException>();
		}
	}
}