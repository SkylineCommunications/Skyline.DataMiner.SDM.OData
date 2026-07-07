namespace Skyline.DataMiner.SDM.OData
{
	/// <summary>
	/// Defines a visitor for OData syntax tree nodes.
	/// </summary>
	public interface IODataVisitor
	{
		/// <summary>
		/// Visits a literal node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitLiteral(LiteralNode node);

		/// <summary>
		/// Visits a property node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitProperty(PropertyNode node);

		/// <summary>
		/// Visits a function call node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitFunctionCall(FunctionCallNode node);

		/// <summary>
		/// Visits a method call node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitMethodCall(MethodCallNode node);

		/// <summary>
		/// Visits a binary node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitBinary(BinaryNode node);

		/// <summary>
		/// Visits a filter node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitFilter(FilterNode node);

		/// <summary>
		/// Visits an AND node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitAnd(AndNode node);

		/// <summary>
		/// Visits an OR node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitOr(OrNode node);

		/// <summary>
		/// Visits a NOT node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitNot(NotNode node);

		/// <summary>
		/// Visits an order node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		void VisitOrder(OrderNode node);
	}
}
