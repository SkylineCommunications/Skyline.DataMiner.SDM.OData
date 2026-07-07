namespace Skyline.DataMiner.SDM.OData
{
	/// <summary>
	/// Walks an OData syntax tree by visiting child nodes recursively.
	/// </summary>
	public class ODataWalker : ODataVisitor
	{
		/// <summary>
		/// Visits an AND node and its children.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public override void VisitAnd(AndNode node)
		{
			DefaultVisit(node);

			foreach (var child in node.Nodes)
			{
				child.Accept(this);
			}
		}

		/// <summary>
		/// Visits a binary node and its operands.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public override void VisitBinary(BinaryNode node)
		{
			DefaultVisit(node);

			node.Left.Accept(this);
			node.Right.Accept(this);
		}

		/// <summary>
		/// Visits a filter node and its expression.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public override void VisitFilter(FilterNode node)
		{
			DefaultVisit(node);

			node.Filter?.Accept(this);
		}

		/// <summary>
		/// Visits an OR node and its children.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public override void VisitOr(OrNode node)
		{
			DefaultVisit(node);

			foreach (var child in node.Nodes)
			{
				child.Accept(this);
			}
		}

		/// <summary>
		/// Visits an order node and its continuation.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public override void VisitOrder(OrderNode node)
		{
			DefaultVisit(node);

			node.Next?.Accept(this);
		}

		/// <summary>
		/// Visits a function call node and its arguments.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public override void VisitFunctionCall(FunctionCallNode node)
		{
			DefaultVisit(node);

			foreach(var argument in node.Arguments)
			{
				argument.Accept(this);
			}
		}

		/// <summary>
		/// Visits a method call node and its child nodes.
		/// </summary>
		/// <param name="node">The node to visit.</param>
		public override void VisitMethodCall(MethodCallNode node)
		{
			DefaultVisit(node);

			node.Target.Accept(this);
			node.Variable.Accept(this);
			node.Body.Accept(this);
		}
	}
}
