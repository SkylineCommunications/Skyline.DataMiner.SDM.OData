namespace Skyline.DataMiner.SDM.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	using SLDataGateway.API.Querying;
	using SLDataGateway.API.Types.Querying;

	public class ODataSdmTranslator<T> : ODataVisitor
		where T : class
	{
		private readonly ODataExposerResolver _resolver;

		private FilterElement<T> _filter = new TRUEFilterElement<T>();
		private IQuery<T> _query = new TRUEFilterElement<T>().ToQuery();

		public ODataSdmTranslator()
		{
			_resolver = new ODataExposerResolver(typeof(T));
		}

		// Used when entering a nested context (e.g. one level deeper via any()).
		internal ODataSdmTranslator(ODataExposerResolver resolver)
		{
			_resolver = resolver;
		}

		public FilterElement<T> TranslateFilter(string filter)
		{
			var tree = new ODataFilterParser(filter).Parse();
			Visit(tree);
			return _filter;
		}

		public IQuery<T> TranslateOrderBy(string orderBy)
		{
			var tree = new ODataOrderByParser(orderBy).Parse();
			Visit(tree);
			return _query;
		}

		public IQuery<T> Translate(string filter, string orderBy)
		{
			// Translate the filter
			var filterTree = new ODataFilterParser(filter).Parse();
			Visit(filterTree);
			_query = _filter.ToQuery();

			// Translate the sort order
			var orderTree = new ODataOrderByParser(orderBy).Parse();
			Visit(orderTree);
			return _query;
		}

		public override void VisitFilter(FilterNode node)
		{
			// Start of all filters
			_filter = new TRUEFilterElement<T>();

			Visit(node.Filter);
		}

		public override void VisitAnd(AndNode node)
		{
			var andFilters = new List<FilterElement<T>>();

			foreach (var child in node.Nodes)
			{
				Visit(child);
				andFilters.Add(_filter);
			}

			_filter = new ANDFilterElement<T>(andFilters.ToArray());
		}

		public override void VisitOr(OrNode node)
		{
			var orFilters = new List<FilterElement<T>>();

			foreach (var child in node.Nodes)
			{
				Visit(child);
				orFilters.Add(_filter);
			}

			_filter = new ORFilterElement<T>(orFilters.ToArray());
		}

		public override void VisitNot(NotNode node)
		{
			Visit(node.Child);
			_filter = _filter.NOT();
		}

		public override void VisitFunctionCall(FunctionCallNode node)
		{
			switch (node.Name)
			{
				case "contains":
				{
					if (!_resolver.TryResolve(node.Arguments[0], out var exposer) ||
						!_resolver.TryGetLiteral(node.Arguments[1], out var literal))
					{
						throw new NotSupportedException($"Unsupported comparison: {node}");
					}

					_filter = FilterElementFactory.Create<T>(exposer!, Comparer.Contains, ODataExposerResolver.ParseLiteral(exposer!.FieldType, literal!));
					break;
				}

				default:
				{
					throw new NotSupportedException($"Function '{node.Name}' is not supported.");
				}
			}
		}

		public override void VisitMethodCall(MethodCallNode node)
		{
			switch (node.Name)
			{
				case "any":
				{
					////if (!_resolver.TryResolve(node.Target, out _))
					////{
					////	throw new NotSupportedException($"Unsupported comparison: {node}");
					////}

					var translator = new ODataSdmTranslator<T>(_resolver.CreateNested(node.Target, node.Variable.Path));

					translator.Visit(node.Body);
					_filter = translator._filter;

					break;
				}

				default:
				{
					throw new NotSupportedException($"Method '{node.Name}' is not supported.");
				}
			}
		}

		public override void VisitBinary(BinaryNode node)
		{
			if (!_resolver.TryGetOperand(node, out var comparer))
			{
				throw new NotSupportedException($"Unsupported comparison: {node}");
			}

			if (!((_resolver.TryResolve(node.Left, out var exposer) && _resolver.TryGetLiteral(node.Right, out var literal)) ||
				(_resolver.TryResolve(node.Right, out exposer) && _resolver.TryGetLiteral(node.Left, out literal))))
			{
				throw new NotSupportedException($"Unsupported comparison: {node}");
			}

			_filter = FilterElementFactory.Create<T>(exposer!, comparer, ODataExposerResolver.ParseLiteral(exposer!.FieldType, literal!));
		}

		public override void VisitOrder(OrderNode node)
		{
			if (node.Order == ODataOrder.None)
			{
				// No order specified, skip.
				return;
			}

			if (!_resolver.TryResolve(node.Property, out var exposer))
			{
				throw new NotSupportedException($"Unsupported property to order by: '{node.Property.Path}'");
			}

			if (!_resolver.TryGetOrder(node, out var sortOrder))
			{
				throw new NotSupportedException($"Unsupported order type: '{node.Order}'");
			}

			if (!_query.Order.Elements.Any())
			{
				_query = _query.WithOrder(
					OrderBy.Default.SingleConcat(OrderByElementFactory.Create(exposer!, sortOrder, false)));
			}
			else
			{
				_query = _query.WithOrder(
					_query.Order.SingleConcat(OrderByElementFactory.Create(exposer!, sortOrder, false)));
			}
		}
	}
}
