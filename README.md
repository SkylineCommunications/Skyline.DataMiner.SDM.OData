Skyline.DataMiner.SDM.OData
===

## About

Parse OData `$filter` and `$orderby` query strings and translate them into `FilterElement<T>` / `IQuery<T>` instances for `SLDataGateway`, so an SDM object model can be queried using OData directly from an HTTP API.

```csharp
using Skyline.DataMiner.SDM;
using Skyline.DataMiner.SDM.Exposers;
using Skyline.DataMiner.SDM.OData;

using SLDataGateway.API.Querying;

public class Person : SdmObject<Person>
{
	public override string Identifier { get; set; }

	public string Name { get; set; }

	public int Age { get; set; }
}

public static class PersonExposers
{
	public static readonly Exposer<Person, string> Name = new Exposer<Person, string>(p => p.Name, "Name");
	public static readonly Exposer<Person, int> Age = new Exposer<Person, int>(p => p.Age, "Age");
}

// Translate the incoming OData query string parameters into an SLDataGateway query.
var translator = new ODataSdmTranslator<Person>();
IQuery<Person> query = translator.Translate(
	filter: "Name eq 'John Doe' and Age gt 30",
	orderBy: "Age desc");

var results = repository.Read(query);
```

If you have questions, you can post them to
our [DataMiner community platform](https://community.dataminer.services/questions/).

### Repository structure

| Project | Purpose |
| --- | --- |
| `OData` | The main library (`Skyline.DataMiner.SDM.OData` on NuGet) — the OData tokenizer/parsers, AST, visitors, and the `ODataSdmTranslator<T>`. |
| `OData.Tests` | Unit tests for the project above (MSTest + FluentAssertions). |

### Installation

```bash
dotnet add package Skyline.DataMiner.SDM.OData
```

## Features

| Feature | Description |
| --- | --- |
| **`$filter` parsing** | `and`, `or`, `not`, comparison operators (`eq`, `ne`, `gt`, `ge`, `lt`, `le`), `contains(...)`, and grouping with parentheses |
| **`$orderby` parsing** | Single or multiple fields, `asc`/`desc` |
| **Collection filtering** | `any(variable: predicate)` on collection properties, including nested `contains(...)` calls |
| **SLDataGateway translation** | Resolves properties through `ExposerRegistry`, the same mechanism used by regular (non-OData) queries, so only properties exposed via an `Exposer<T, ...>` are queryable |
| **Extensible AST** | `ODataNode` hierarchy plus `IODataVisitor`/`ODataVisitor`/`ODataWalker` for writing custom translators or tooling |

## Building & testing

- Solution file: `Skyline.DataMiner.SDM.OData.slnx` (target framework `net48`).
- Build: `dotnet build .\Skyline.DataMiner.SDM.OData.slnx -c Release`
- Test: `dotnet test .\OData.Tests\OData.Tests.csproj`

## About DataMiner

DataMiner is a transformational platform that provides vendor-independent control and monitoring of devices and services. Out of the box and by design, it addresses key challenges such as security, complexity, multi-cloud, and much more. It has a pronounced open architecture and powerful capabilities enabling users to evolve easily and continuously.

The foundation of DataMiner is its powerful and versatile data acquisition and control layer. With DataMiner, there are no restrictions to what data users can access. Data sources may reside on premises, in the cloud, or in a hybrid setup.

A unique catalog of 7000+ connectors already exists. In addition, you can leverage DataMiner Development Packages to build your own connectors (also known as "protocols" or "drivers").

> **Note**
> See also: [About DataMiner](https://aka.dataminer.services/about-dataminer).

## About Skyline Communications

At Skyline Communications, we deal in world-class solutions that are deployed by leading companies around the globe. Check out [our proven track record](https://aka.dataminer.services/about-skyline) and see how we make our customers' lives easier by empowering them to take their operations to the next level.