namespace Skyline.DataMiner.SDM.OData.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;

	using FluentAssertions;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM;
	using Skyline.DataMiner.SDM.Exposers;
	using Skyline.DataMiner.SDM.OData;
	using Skyline.DataMiner.SDM.OData.Exceptions;
	using Skyline.DataMiner.SDM.OData.Tests.Equality;

	using SLDataGateway.API.Querying;
	using SLDataGateway.API.Types.Querying;

	public class Person : SdmObject<Person>
	{
		public Person(string id, string name, int age)
		{
			Identifier = id;
			Name = name;
			Age = age;
		}

		public override string Identifier { get; set; }

		public string Name { get; set; }

		public int Age { get; set; }

		public double? Weight { get; set; }

		public ICollection<string> Aliases { get; } = new List<string>();

		public ICollection<ExternalIdentifier> ExternalIdentifiers { get; } = new List<ExternalIdentifier>();

		public SdmObjectReference<Person> Mother { get; set; }

		public SdmObjectReference<Person> Father { get; set; }

		public GrandParents GrandParents { get; set; }
	}

	public class ExternalIdentifier
	{
		public string ExternalID { get; set; }

		public string ExternalName { get; set; }
	}

	public class GrandParents
	{
		public SdmObjectReference<Person> MaternalGrandMother { get; set; }
		public SdmObjectReference<Person> MaternalGrandFather { get; set; }
		public SdmObjectReference<Person> PaternalGrandMother { get; set; }
		public SdmObjectReference<Person> PaternalGrandFather { get; set; }
	}

	public static class PersonExposers
	{
		static PersonExposers()
		{
			RuntimeHelpers.RunClassConstructor(typeof(ExternalIdentifiers).TypeHandle);
		}

		public static readonly Exposer<Person, string> Identifier = new Exposer<Person, string>((obj) => obj.Identifier, "Identifier");
		public static readonly Exposer<Person, string> Name = new Exposer<Person, string>((obj) => obj.Name, "Name");
		public static readonly Exposer<Person, int> Age = new Exposer<Person, int>((obj) => obj.Age, "Age");
		public static readonly Exposer<Person, double?> Weight = new Exposer<Person, double?>((obj) => obj.Weight, "Weight");
		public static readonly CollectionExposer<Person, string> Aliases = new CollectionExposer<Person, string>((obj) => obj.Aliases.Where(x => x is not null), "Aliases");
		public static readonly Exposer<Person, SdmObjectReference<Person>> Mother = new Exposer<Person, SdmObjectReference<Person>>((obj) => obj.Mother, "Mother");
		public static readonly Exposer<Person, SdmObjectReference<Person>> Father = new Exposer<Person, SdmObjectReference<Person>>((obj) => obj.Father, "Father");

		public static class ExternalIdentifiers
		{
			public static readonly CollectionExposer<Person, string> ExternalID = new CollectionExposer<Person, string>((obj) => obj.ExternalIdentifiers.Where(x => x.ExternalID is not null).Select(x => x.ExternalID), "ExternalIdentifiers.ExternalID");
			public static readonly CollectionExposer<Person, string> ExternalName = new CollectionExposer<Person, string>((obj) => obj.ExternalIdentifiers.Where(x => x.ExternalID is not null).Select(x => x.ExternalName), "ExternalIdentifiers.ExternalName");
		}

		public static class GrandParents
		{
			public static readonly Exposer<Person, SdmObjectReference<Person>> MaternalGrandMother = new Exposer<Person, SdmObjectReference<Person>>((obj) => obj.GrandParents.MaternalGrandMother, "GrandParents.MaternalGrandMother");
			public static readonly Exposer<Person, SdmObjectReference<Person>> MaternalGrandFather = new Exposer<Person, SdmObjectReference<Person>>((obj) => obj.GrandParents.MaternalGrandFather, "GrandParents.MaternalGrandFather");
			public static readonly Exposer<Person, SdmObjectReference<Person>> PaternalGrandMother = new Exposer<Person, SdmObjectReference<Person>>((obj) => obj.GrandParents.PaternalGrandMother, "GrandParents.PaternalGrandMother");
			public static readonly Exposer<Person, SdmObjectReference<Person>> PaternalGrandFather = new Exposer<Person, SdmObjectReference<Person>>((obj) => obj.GrandParents.PaternalGrandFather, "GrandParents.PaternalGrandFather");
		}
	}

	[TestClass]
	public class ODataSdmTranslatorTests
	{
		private static List<Person> _people = new List<Person>
		{
			new Person("Person 001", "Andi Tamer", 105)
			{
				Aliases =
				{
					"Agent Andi",
					"Tamer the Great",
				},
				ExternalIdentifiers =
				{
					new ExternalIdentifier
					{
						ExternalID = "e001",
						ExternalName = "EXT Andi Tamer",
					},
				},
			},
			new Person("Person 002", "John Doe", 35)
			{
				Aliases =
				{
					"JD",
					"Johnny",
				},
				ExternalIdentifiers =
				{
					new ExternalIdentifier
					{
						ExternalID = "p002",
						ExternalName = "EXT John Doe",
					},
				},
			},
			new Person("Person 003", "Fiber Squad", 3)
			{
				Aliases =
				{
					"Fiber",
					"The Squad",
				},
				ExternalIdentifiers =
				{
					new ExternalIdentifier
					{
						ExternalID = "p003",
						ExternalName = "SQUAD Fiber",
					},
				},
				Mother = new SdmObjectReference<Person>("Person 001"),
				Father = new SdmObjectReference<Person>("Person 002"),
			},
		};

		[TestMethod]
		public void Empty_Filter()
		{
			// Arrange
			var expectedFilter = new TRUEFilterElement<Person>();
			var odataFilter = "";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		[DataRow("Person 001")]
		[DataRow("2f46daec-421a-491a-a1ac-7ec4a4d01494")]
		public void SdmObjectReference_Equal(string identifier)
		{
			// Arrange
			var expectedFilter = PersonExposers.Mother.Equal(new SdmObjectReference<Person>(identifier));
			var odataFilter = $"Mother eq '{identifier}'";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		[DataRow("Person 001")]
		[DataRow("2f46daec-421a-491a-a1ac-7ec4a4d01494")]
		public void SubProperty_Resolves_To_RootProperty_With_Matching_Name(string identifier)
		{
			// Arrange
			var expectedFilter = PersonExposers.GrandParents.MaternalGrandFather.Equal(new SdmObjectReference<Person>(identifier));
			var odataFilter = $"GrandParents/MaternalGrandFather eq '{identifier}'";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void SubProperty_Without_RootProperty_Match_Throws()
		{
			// Arrange
			var odataFilter = "Mother/ExternalName eq 'Andi Tamer'";
			var translator = new ODataSdmTranslator<Person>();

			// Act
			Action act = () => translator.TranslateFilter(odataFilter);

			// Assert
			act.Should().Throw<NotSupportedException>();
		}

		[TestMethod]
		public void String_Equal()
		{
			// Arrange
			var expectedFilter = PersonExposers.Name.Equal("Andi Tamer");
			var odataFilter = "Name eq 'Andi Tamer'";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void String_NotEqual()
		{
			// Arrange
			var expectedFilter = PersonExposers.Name.NotEqual("Andi Tamer");
			var odataFilter = "Name ne 'Andi Tamer'";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void String_Contains()
		{
			// Arrange
			var expectedFilter = PersonExposers.Name.Contains("Andi");
			var odataFilter = "contains(Name, 'Andi')";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		[DataRow(20)]
		[DataRow(-5)]
		[DataRow(0)]
		public void Number_GreaterThan(int age)
		{
			// Arrange
			var expectedFilter = PersonExposers.Age.GreaterThan(age);
			var odataFilter = $"Age gt {age}";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		[DataRow(20)]
		[DataRow(-5)]
		[DataRow(0)]
		public void Number_GreaterThanOrEqual(int age)
		{
			// Arrange
			var expectedFilter = PersonExposers.Age.GreaterThanOrEqual(age);
			var odataFilter = $"Age ge {age}";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		[DataRow(20)]
		[DataRow(-5)]
		[DataRow(0)]
		public void Number_LessThan(int age)
		{
			// Arrange
			var expectedFilter = PersonExposers.Age.LessThan(age);
			var odataFilter = $"Age lt {age}";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		[DataRow(20)]
		[DataRow(-5)]
		[DataRow(0)]
		public void Number_LessThanOrEqual(int age)
		{
			// Arrange
			var expectedFilter = PersonExposers.Age.LessThanOrEqual(age);
			var odataFilter = $"Age le {age}";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		[DataRow(72d)]
		[DataRow(0d)]
		public void Nullable_Number_Equal(double weight)
		{
			// Arrange
			var expectedFilter = PersonExposers.Weight.Equal(weight);
			var odataFilter = $"Weight eq {weight}";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Nullable_Number_Equal_Null()
		{
			// Arrange
			var expectedFilter = PersonExposers.Weight.HasNoValue();
			var odataFilter = "Weight eq null";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Nullable_Number_NotEqual_Null()
		{
			// Arrange
			var expectedFilter = PersonExposers.Weight.HasValue();
			var odataFilter = "Weight ne null";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void OrderBy_Empty()
		{
			// Arrange
			var expectedQuery = new TRUEFilterElement<Person>().ToQuery();
			var orderBy = "";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var query = translator.TranslateOrderBy(orderBy);

			// Assert
			query.Should().Be(expectedQuery, new QueryEqualityComparer<Person>());
		}

		[TestMethod]
		public void OrderBy_ID()
		{
			// Arrange
			var expectedQuery = new TRUEFilterElement<Person>()
				.ToQuery()
				.OrderBy(PersonExposers.Identifier);
			var orderBy = "Identifier";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var query = translator.TranslateOrderBy(orderBy);

			// Assert
			query.Should().Be(expectedQuery, new QueryEqualityComparer<Person>());
		}

		[TestMethod]
		public void OrderBy_ID_Persons_Descending()
		{
			// Arrange
			var orderByOData = "Identifier desc";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var query = translator.TranslateOrderBy(orderByOData);
			var result = query.ExecuteInMemory(_people);

			// Assert
			Console.WriteLine(query);
			result.Should().BeInDescendingOrder(p => p.Identifier);
		}

		[TestMethod]
		public void StringCollection_Contains()
		{
			// Arrange
			var expectedFilter = PersonExposers.Aliases.Contains("Agent Andi");
			var odataFilter = "contains(Aliases, 'Agent Andi')";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Collection_Any()
		{
			// Arrange
			var expectedFilter = PersonExposers.ExternalIdentifiers.ExternalName.Equal("EXT Andi Tamer");
			var odataFilter = "ExternalIdentifiers/any(e: e/ExternalName eq 'EXT Andi Tamer')";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Collection_Any_With_And()
		{
			// Arrange
			var expectedFilter = new ANDFilterElement<Person>(
				PersonExposers.ExternalIdentifiers.ExternalID.Contains("p"),
				PersonExposers.ExternalIdentifiers.ExternalName.Contains("EXT"));
			var odataFilter = "ExternalIdentifiers/any(e: contains(e/ExternalID, 'p') and contains(e/ExternalName, 'EXT'))";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void And_With_Three_Conditions()
		{
			// Arrange
			var expectedFilter = new ANDFilterElement<Person>(
				PersonExposers.Name.Equal("Andi Tamer"),
				PersonExposers.Age.GreaterThan(25),
				PersonExposers.Age.LessThan(130));
			var odataFilter = "Name eq 'Andi Tamer' and Age gt 25 and Age lt 130";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Or_Translate()
		{
			// Arrange
			var expectedFilter = new ORFilterElement<Person>(
				PersonExposers.Name.Equal("Andi Tamer"),
				PersonExposers.Age.GreaterThanOrEqual(25));
			var odataFilter = "Name eq 'Andi Tamer' or Age ge 25";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Not_Translate()
		{
			// Arrange
			var expectedFilter = PersonExposers.Name.Equal("Andi Tamer").NOT();
			var odataFilter = "not Name eq 'Andi Tamer'";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Grouped_Or_And_And_Respect_Precedence()
		{
			// Arrange
			var expectedFilter = new ANDFilterElement<Person>(
				new ORFilterElement<Person>(
					PersonExposers.Name.Equal("Andi Tamer"),
					PersonExposers.Name.Equal("John Doe")),
				PersonExposers.Age.GreaterThan(18));
			var odataFilter = "(Name eq 'Andi Tamer' or Name eq 'John Doe') and Age gt 18";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Whitespace_Tolerant_Filter()
		{
			// Arrange
			var expectedFilter = PersonExposers.Name.Equal("Andi Tamer");
			var odataFilter = "Name   eq   'Andi Tamer'";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Operator_Keyword_Casing_Is_Ignored()
		{
			// Arrange
			var expectedFilter = PersonExposers.Name.Equal("Andi Tamer");
			var odataFilter = "Name EQ 'Andi Tamer'";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}

		[TestMethod]
		public void Unknown_Property_Throws()
		{
			// Arrange
			var odataFilter = "Unknown eq 'Andi Tamer'";
			var translator = new ODataSdmTranslator<Person>();

			// Act
			Action act = () => translator.TranslateFilter(odataFilter);

			// Assert
			act.Should().Throw<NotSupportedException>();
		}

		[TestMethod]
		public void Comparing_Two_Properties_Throws()
		{
			// Arrange
			var odataFilter = "Name eq Age";
			var translator = new ODataSdmTranslator<Person>();

			// Act
			Action act = () => translator.TranslateFilter(odataFilter);

			// Assert
			act.Should().Throw<ODataParseException>();
		}

		[TestMethod]
		public void Unsupported_Function_Throws()
		{
			// Arrange
			var odataFilter = "startswith(Name, 'Andi')";
			var translator = new ODataSdmTranslator<Person>();

			// Act
			Action act = () => translator.TranslateFilter(odataFilter);

			// Assert
			act.Should().Throw<NotSupportedException>();
		}

		[TestMethod]
		public void Unsupported_Lambda_Method_Throws()
		{
			// Arrange
			var odataFilter = "ExternalIdentifiers/all(e: e/ExternalName eq 'Andi Tamer')";
			var translator = new ODataSdmTranslator<Person>();

			// Act
			Action act = () => translator.TranslateFilter(odataFilter);

			// Assert
			act.Should().Throw<NotSupportedException>();
		}

		////Currently not possible due to limitation in DOM
		////[TestMethod]
		////public void OrderBy_Multiple_Properties()
		////{
		////	// Arrange
		////	var expectedQuery = new TRUEFilterElement<Person>()
		////		.ToQuery()
		////		.OrderByDescending(PersonExposers.Age)
		////		.ThenBy(PersonExposers.Name);
		////	var orderBy = "Age desc, Name asc";

		////	// Act
		////	var translator = new ODataSdmTranslator<Person>();
		////	var query = translator.TranslateOrderBy(orderBy);

		////	// Assert
		////	query.Should().Be(expectedQuery, new QueryEqualityComparer<Person>());
		////}

		[TestMethod]
		public void OrderBy_Multiple_Properties_ExecuteInMemory()
		{
			// Arrange
			var orderBy = "Age asc";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var query = translator.TranslateOrderBy(orderBy);
			var result = query.ExecuteInMemory(_people);

			// Assert
			result.Should().BeInAscendingOrder(p => p.Age);
		}

		[TestMethod]
		public void Translate_Filter_And_OrderBy_Together()
		{
			// Arrange
			var translator = new ODataSdmTranslator<Person>();

			// Act
			var query = translator.Translate("Age lt 130", "Age desc");
			var result = query.Filter.ToQuery().ExecuteInMemory(_people).OrderBy(p => -p.Age);

			// Assert
			result.Select(p => p.Identifier).Should().Equal(_people.OrderByDescending(p => p.Age).Select(p => p.Identifier));
		}

		[TestMethod]
		public void Filter_ExecuteInMemory_Returns_Matching_People()
		{
			// Arrange
			RuntimeHelpers.RunClassConstructor(typeof(PersonExposers).TypeHandle);
			var odataFilter = "Age gt 30";
			var translator = new ODataSdmTranslator<Person>();

			// Act
			var filter = translator.TranslateFilter(odataFilter);
			var result = filter.ToQuery().ExecuteInMemory(_people);

			// Assert
			result.Select(p => p.Identifier).Should().BeEquivalentTo(new[] { "Person 001", "Person 002" });
		}

		[TestMethod]
		public void Collection_Any_With_Or_Inside_Lambda()
		{
			// Arrange
			var expectedFilter = new ORFilterElement<Person>(
				PersonExposers.ExternalIdentifiers.ExternalID.Contains("p00"),
				PersonExposers.ExternalIdentifiers.ExternalName.Contains("SQUAD"));
			var odataFilter = "ExternalIdentifiers/any(e: contains(e/ExternalID, 'p00') or contains(e/ExternalName, 'SQUAD'))";

			// Act
			var translator = new ODataSdmTranslator<Person>();
			var filter = translator.TranslateFilter(odataFilter);

			// Assert
			filter.Should().Be(expectedFilter);
		}
	}
}
