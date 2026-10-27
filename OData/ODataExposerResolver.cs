namespace Skyline.DataMiner.SDM.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	// Resolves OData AST nodes to FieldExposers via ExposerRegistry, the same path resolution used by regular querying.
	internal class ODataExposerResolver
	{
		private readonly Type _type;
		private readonly string _basePath;
		private readonly string? _lambdaVariable;

		public ODataExposerResolver(Type type, string basePath = "", string? lambdaVariable = null)
		{
			_type = type;
			_basePath = basePath ?? String.Empty;
			_lambdaVariable = lambdaVariable;
		}

		public bool TryResolve(ODataNode? node, out FieldExposer? exposer)
		{
			exposer = null;

			if (!(node is PropertyNode pn))
			{
				return false;
			}

			exposer = ExposerRegistry.findExposer(_type, GetFullPath(pn));
			return exposer is not null;
		}

		public bool TryGetLiteral(ODataNode? node, out LiteralNode? value)
		{
			value = null;

			if (node is LiteralNode ln)
			{
				value = ln;
				return true;
			}

			return false;
		}

		public bool TryGetOperand(BinaryNode? node, out Comparer comparer)
		{
			comparer = Comparer.Equals;

			switch (node?.Operation)
			{
				case ODataLogicalOperation.Equal:
					comparer = Comparer.Equals;
					break;

				case ODataLogicalOperation.NotEqual:
					comparer = Comparer.NotEquals;
					break;

				case ODataLogicalOperation.GreaterThan when node?.Left is PropertyNode:
				case ODataLogicalOperation.LessThan when node?.Right is PropertyNode:
					comparer = Comparer.GT;
					break;

				case ODataLogicalOperation.GreaterThanOrEqual when node?.Left is PropertyNode:
				case ODataLogicalOperation.LessThanOrEqual when node?.Right is PropertyNode:
					comparer = Comparer.GTE;
					break;

				case ODataLogicalOperation.LessThan when node?.Left is PropertyNode:
				case ODataLogicalOperation.GreaterThan when node?.Right is PropertyNode:
					comparer = Comparer.LT;
					break;

				case ODataLogicalOperation.LessThanOrEqual when node?.Left is PropertyNode:
				case ODataLogicalOperation.GreaterThanOrEqual when node?.Right is PropertyNode:
					comparer = Comparer.LTE;
					break;

				default:
					return false;
			}

			return true;
		}

		public bool TryGetOrder(OrderNode? node, out SLDataGateway.API.Types.Querying.SortOrder sortOrder)
		{
			switch (node?.Order)
			{
				case ODataOrder.Ascending:
					sortOrder = SLDataGateway.API.Types.Querying.SortOrder.Ascending;
					return true;

				case ODataOrder.Descending:
					sortOrder = SLDataGateway.API.Types.Querying.SortOrder.Descending;
					return true;

				default:
					sortOrder = SLDataGateway.API.Types.Querying.SortOrder.None;
					return false;
			}
		}

		// Scopes a resolver one level deeper for an any(variable: body) call.
		public ODataExposerResolver CreateNested(PropertyNode target, string lambdaVariable)
		{
			return new ODataExposerResolver(_type, GetFullPath(target), lambdaVariable);
		}

		// Strips the lambda variable segment (e.g. "e" in "e/Property" or "e"); any other '/' is kept as a literal path segment.
		private string GetFullPath(PropertyNode node)
		{
			var path = node.Path;

			if (String.Equals(path, _lambdaVariable, StringComparison.Ordinal))
			{
				path = String.Empty;
			}
			else if (_lambdaVariable is not null &&
				path.StartsWith(_lambdaVariable + "/", StringComparison.Ordinal))
			{
				path = path.Substring(_lambdaVariable.Length + 1);
			}

			path = path.Replace('/', '.');

			if (String.IsNullOrEmpty(path))
			{
				return _basePath;
			}

			return String.IsNullOrEmpty(_basePath) ? path : _basePath + "." + path;
		}

		private static bool TryGetCollectionElementType(Type type, out Type elementType)
		{
			// Handle arrays
			if (type.IsArray)
			{
				elementType = type.GetElementType()!;
				return true;
			}

			// Handle generic collections (List<T>, IEnumerable<T>, ICollection<T>, etc.)
			if (type.IsGenericType)
			{
				var genDef = type.GetGenericTypeDefinition();
				if (typeof(IEnumerable<>).IsAssignableFrom(genDef) ||
					typeof(ICollection<>).IsAssignableFrom(genDef) ||
					typeof(IList<>).IsAssignableFrom(genDef) ||
					typeof(List<>).IsAssignableFrom(genDef))
				{
					elementType = type.GetGenericArguments()[0];
					return true;
				}
			}

			// Handle types that *implement* IEnumerable<T> somewhere
			var enumerableInterface = type.GetInterfaces()
				.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

			if (enumerableInterface != null)
			{
				elementType = enumerableInterface.GetGenericArguments()[0];
				return true;
			}

			elementType = null!;
			return false;
		}

		public static object ParseLiteral(Type type, LiteralNode literal)
		{
			if (Nullable.GetUnderlyingType(type) is Type underlying)
			{
				if (literal.Value is null || string.Equals(Convert.ToString(literal.Value), "null", StringComparison.OrdinalIgnoreCase))
				{
					return null!;
				}

				return ParseLiteral(underlying, literal);
			}

			if (literal.Value is null && type.IsValueType)
			{
				throw new NotSupportedException($"Null is not supported for non-nullable type '{type.FullName}'.");
			}

			TryGetCollectionElementType(type, out var elementType);

			if (type == typeof(string) ||
				elementType == typeof(string))
			{
				return Convert.ToString(literal.Value);
			}
			else if (type == typeof(bool) ||
				elementType == typeof(bool))
			{
				return Convert.ToBoolean(literal.Value);
			}
			else if (type == typeof(byte) ||
				elementType == typeof(byte))
			{
				return Convert.ToByte(literal.Value);
			}
			else if (type == typeof(sbyte) ||
				elementType == typeof(sbyte))
			{
				return Convert.ToSByte(literal.Value);
			}
			else if (type == typeof(short) ||
				elementType == typeof(short))
			{
				return Convert.ToInt16(literal.Value);
			}
			else if (type == typeof(ushort) ||
				elementType == typeof(ushort))
			{
				return Convert.ToUInt16(literal.Value);
			}
			else if (type == typeof(int) ||
				elementType == typeof(int))
			{
				return Convert.ToInt32(literal.Value);
			}
			else if (type == typeof(uint) ||
				elementType == typeof(uint))
			{
				return Convert.ToUInt32(literal.Value);
			}
			else if (type == typeof(long) ||
				elementType == typeof(long))
			{
				return Convert.ToInt64(literal.Value);
			}
			else if (type == typeof(ulong) ||
				elementType == typeof(ulong))
			{
				return Convert.ToUInt64(literal.Value);
			}
			else if (type == typeof(DateTime) ||
				elementType == typeof(DateTime))
			{
				return DateTime.Parse(Convert.ToString(literal.Value));
			}
			else if (type == typeof(TimeSpan) ||
				elementType == typeof(TimeSpan))
			{
				return TimeSpan.Parse(Convert.ToString(literal.Value));
			}
			else if (type == typeof(double) ||
				elementType == typeof(double))
			{
				return Convert.ToDouble(literal.Value);
			}
			else if (type == typeof(float) ||
				elementType == typeof(float))
			{
				return Convert.ToSingle(literal.Value);
			}
			else if (type == typeof(decimal) ||
				elementType == typeof(decimal))
			{
				return Convert.ToDecimal(literal.Value);
			}
			else if (type == typeof(Guid) ||
				elementType == typeof(Guid))
			{
				return Guid.Parse(Convert.ToString(literal.Value));
			}
			else if (type.IsEnum ||
				(elementType?.IsEnum ?? false))
			{
				var enumType = type.IsEnum ? type : elementType;
				return Enum.Parse(enumType, Convert.ToString(literal.Value));
			}
			else if ((type.IsGenericType && type.GetGenericTypeDefinition().FullName == "Skyline.DataMiner.SDM.SdmObjectReference`1") ||
				((elementType?.IsGenericType ?? false) && elementType?.GetGenericTypeDefinition().FullName == "Skyline.DataMiner.SDM.SdmObjectReference`1"))
			{
				// Handle SdmObjectReference<T>
				var sdmObjectReferenceType = type.IsGenericType && type.GetGenericTypeDefinition().FullName == "Skyline.DataMiner.SDM.SdmObjectReference`1"
					? type
					: elementType;

				var identifier = Convert.ToString(literal.Value);
				var constructor = sdmObjectReferenceType.GetConstructor(new Type[] { typeof(string) });
				if (constructor == null)
				{
					throw new NotSupportedException($"No suitable constructor found for type: {sdmObjectReferenceType.FullName}");
				}

				return constructor.Invoke(new object[] { identifier });
			}
			else
			{
				throw new NotSupportedException($"Unsupported property type: {type.FullName}");
			}
		}
	}
}
