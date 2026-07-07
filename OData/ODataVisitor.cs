namespace Skyline.DataMiner.SDM.OData
{
	/// <summary>
	/// Provides a base visitor for OData syntax tree nodes.
	/// </summary>
	public class ODataVisitor : IODataVisitor
	{
		/// <summary>
		/// Visits the specified node when it is not <see langword="null"/>.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void Visit(ODataNode? node)
		{
			if (node is not null)
			{
				node.Accept(this);
			}
		}

		/// <summary>
		/// Performs the default visit logic for a node.
		/// </summary>
		/// <param name="node">The visited node.</param>
		public virtual void DefaultVisit(ODataNode node)
		{
		}

		/// <summary>
		/// Visits a filter node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitFilter(FilterNode node)
		{
			DefaultVisit(node);
		}

		/// <summary>
		/// Visits an AND node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitAnd(AndNode node)
		{
			DefaultVisit(node);
		}

		/// <summary>
		/// Visits an OR node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitOr(OrNode node)
		{
			DefaultVisit(node);
		}

		/// <summary>
		/// Visits a NOT node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitNot(NotNode node)
		{
			DefaultVisit(node);
		}

		/// <summary>
		/// Visits a binary node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitBinary(BinaryNode node)
		{
			DefaultVisit(node);
		}

		/// <summary>
		/// Visits a property node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitProperty(PropertyNode node)
		{
			DefaultVisit(node);
		}

		/// <summary>
		/// Visits a literal node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitLiteral(LiteralNode node)
		{
			DefaultVisit(node);
		}

		/// <summary>
		/// Visits an order node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitOrder(OrderNode node)
		{
			DefaultVisit(node);
		}

		/// <summary>
		/// Visits a function call node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitFunctionCall(FunctionCallNode node)
		{
			DefaultVisit(node);
		}

		/// <summary>
		/// Visits a method call node.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public virtual void VisitMethodCall(MethodCallNode node)
		{
			DefaultVisit(node);
		}
	}
}
