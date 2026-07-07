namespace Skyline.DataMiner.SDM.OData
{
	using System;

	using Skyline.DataMiner.SDM.OData.Exceptions;

	/// <summary>
	/// Parses OData order-by expressions into syntax tree nodes.
	/// </summary>
	public class ODataOrderByParser
	{
		private readonly ODataTokenizer _tokenizer;

		private ODataToken _token = new ODataToken(ODataTokenType.EndOfToken, String.Empty, 0);

		private ODataOrderByParser(string order)
		{
			_tokenizer = new ODataTokenizer(order);
			Next();
		}

		/// <summary>
		/// Parses the specified order-by expression into a syntax tree.
		/// </summary>
		/// <param name="order">The order-by expression to parse.</param>
		/// <returns>The parsed order node.</returns>
		/// <exception cref="ODataParseException">Thrown when the order-by expression contains invalid syntax.</exception>
		public static OrderNode Parse(string order)
		{
			return new ODataOrderByParser(order).Parse();
		}

		private OrderNode Parse()
		{
			var node = default(OrderNode);
			if (_token.Type == ODataTokenType.EndOfToken)
			{
				node = new OrderNode(new PropertyNode(String.Empty), ODataOrder.None, null, node);
				return node;
			}

			var order = ParseOrder(null);
			if (_token.Type != ODataTokenType.EndOfToken)
			{
				throw new ODataParseException($"Unexpected token {_token} at the end.");
			}

			return order;
		}

		/// <summary>
		/// Parses an order clause and its optional continuation.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The parsed order node.</returns>
		/// <exception cref="ODataParseException">Thrown when the order clause contains invalid syntax.</exception>
		private OrderNode ParseOrder(OrderNode? parent)
		{
			if (_token.Type != ODataTokenType.Identifier)
			{
				throw new ODataParseException($"Unexpected token {_token} at {_token.Position}");
			}

			var property = new PropertyNode(_token.Text);
			Next();

			ODataOrder order = ODataOrder.Ascending;
			if (_token.Type == ODataTokenType.Ascending)
			{
				order = ODataOrder.Ascending;
				Next();
			}
			else if (_token.Type == ODataTokenType.Descending)
			{
				order = ODataOrder.Descending;
				Next();
			}

			OrderNode? next = null;
			if (_token.Type == ODataTokenType.Comma)
			{
				Next();
				next = ParseOrder(parent);
			}

			return new OrderNode(property, order, next, parent);
		}

		private void Next()
		{
			_token = _tokenizer.NextToken();
		}
	}
}
