# Copilot Instructions for Skyline.DataMiner.SDM.OData

## What this is

A .NET Framework 4.8 library that parses OData `$filter` / `$orderby` query strings
and translates them into SLDataGateway (`SLDataGateway.API.Querying`) filter/query
expressions for DataMiner SDM objects.

## Build & test

```powershell
dotnet build Skyline.DataMiner.SDM.OData.slnx
dotnet test                                                     # full suite
dotnet test --filter "FullyQualifiedName~ODataParserTests.Empty_Filter"   # single test
dotnet test --filter "FullyQualifiedName~ODataSdmTranslatorTests"         # single class
```

- `OData.csproj` targets `net48`, `AnyCpu`; `OData.Tests.csproj` targets `net48`, `x86` — keep this
  platform mismatch in mind if debugging odd build/reference issues.
- Tests use MSTest + FluentAssertions (implicit usings enabled, `Nullable` enabled).
- StyleCop.Analyzers + Skyline CICD/SecureCoding analyzers run as part of every build (see
  `Directory.Build.props`); expect analyzer warnings (SA*) to surface during `dotnet build`/`dotnet test`.
- Package versions are centrally managed in `Directory.Packages.props` (`ManagePackageVersionsCentrally`) —
  add new dependencies there, not with inline versions in a `.csproj`.
- CI (`.github/workflows/Internal.yml`, `Public.yml`) builds via the shared
  `SkylineCommunications/_ReusableWorkflows` "Master Workflow", triggered on push/tag; Public.yml
  additionally pushes to nuget.org on version tags.

## Architecture (pipeline)

The library is a classic tokenize → parse → visit pipeline:

1. **`ODataTokenizer`** — lexes a raw OData string into `ODataToken`s (see `ODataTokenType`).
2. **`ODataFilterParser`** / **`ODataOrderByParser`** — recursive-descent parsers that consume tokens
   and build an AST of `ODataNode` subclasses (`OData/Nodes/`): `FilterNode`, `AndNode`, `OrNode`,
   `NotNode`, `BinaryNode`, `FunctionCallNode`, `MethodCallNode`, `PropertyNode`, `LiteralNode`,
   `OrderNode`. Every node tracks its `Parent` for context (e.g. nested `any(...)` lambdas).
3. **Visitor pattern** — `IODataVisitor`/`ODataVisitor` define `VisitXxx` per node type.
   `ODataWalker` is a no-op traversal base (walks children, does nothing else) — extend it when you
   just need to visit every node. `ODataSdmTranslator<T>` is the real consumer: it visits the AST and
   builds `FilterElement<T>` / `IQuery<T>` objects using `FilterElementFactory` / `OrderByElementFactory`
   and `ExposerRegistry.findExposer(typeof(T), path)`.
4. **`ODataToFilterElementConverter`** (internal, generic `<T>` subclass) — reflects over `T`'s public
   properties to resolve OData property paths to `MemberInfo`, parses literals into the target
   property's CLR type, and maps OData operators (`ODataLogicalOperation`, `ODataArithmeticOperation`,
   `ODataStringOperation`, `ODataDateOperation`) to SLDataGateway `Comparer` values.

Key nuance: `any(...)` lambda expressions (`MethodCallNode`) recurse into a **new**
`ODataSdmTranslator<T>` instance constructed with a narrowed converter (collection element type) and an
accumulated `_basePath`, so nested/dotted property paths (e.g. `Children.Any(c => c.Name eq 'x')`)
resolve to `"Children.Name"` style paths via `GetPathForMember`.

Parse errors throw `Exceptions.ODataParseException`; unsupported OData constructs (functions, methods,
operators, orderings) throw `NotSupportedException` from `ODataSdmTranslator<T>`.

## Conventions

- Tabs for indentation, `namespace X { ... }` block-style (not file-scoped namespaces), `using` inside
  the namespace block — follow this in any new file (see `.editorconfig` for full style rules).
- `Nullable` is enabled in both projects — respect `?`/nullable-annotation on new APIs.
- New OData operators/functions/methods belong as `case` branches in `ODataSdmTranslator<T>`
  (`VisitBinary`/`VisitFunctionCall`/`VisitMethodCall`), with the operand/comparer mapping added in
  `ODataToFilterElementConverter`, and the token/enum added in `OData/Enums/` if new syntax is required.
- Tests mirror this split: `ODataParserTests` covers tokenizing/parsing into the AST;
  `ODataSdmTranslatorTests` covers AST → `IQuery<T>`/`FilterElement<T>` translation, using
  `Equality/QueryEqualityComparer` to compare resulting queries.
