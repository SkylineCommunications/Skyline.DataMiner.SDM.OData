namespace Skyline.DataMiner.SDM.OData
{
	using System;

	/// <summary>
	/// Represents a binary comparison node in the OData syntax tree.
	/// </summary>
	public class BinaryNode : ODataNode<BinaryNode>
	{
		internal BinaryNode(ODataNode left, ODataLogicalOperation operation, ODataNode right, ODataNode? parent = null)
			: base(parent)
		{
			Left = left?.WithParentInternal(this) ?? throw new ArgumentNullException(nameof(left));
			Operation = operation;
			Right = right?.WithParentInternal(this) ?? throw new ArgumentNullException(nameof(right));
		}

		/// <summary>
		/// Gets the comparison operator.
		/// </summary>
		public ODataLogicalOperation Operation { get; }

		/// <summary>
		/// Gets the left operand.
		/// </summary>
		public ODataNode Left { get; }

		/// <summary>
		/// Gets the right operand.
		/// </summary>
		public ODataNode Right { get; }

		/// <summary>
		/// Returns a copy of this node with the specified operands, operator, and parent.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <param name="operation">The comparison operator.</param>
		/// <param name="right">The right operand.</param>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		public BinaryNode Update(
			ODataNode left,
			ODataLogicalOperation operation,
			ODataNode right,
			ODataNode? parent)
		{
			if (left != Left || operation != Operation || right != Right || parent != Parent)
			{
				var newNode = new BinaryNode(left, operation, right, parent);
				return newNode;
			}

			return this;
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor to accept.</param>
		public override void Accept(IODataVisitor visitor)
		{
			visitor.VisitBinary(this);
		}

		/// <summary>
		/// Returns a copy of this node with the specified left operand.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <returns>The updated node.</returns>
		public BinaryNode WithLeft(ODataNode left)
		{
			return Update(left, Operation, Right, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified operator.
		/// </summary>
		/// <param name="operation">The comparison operator.</param>
		/// <returns>The updated node.</returns>
		public BinaryNode WithOperation(ODataLogicalOperation operation)
		{
			return Update(Left, operation, Right, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified right operand.
		/// </summary>
		/// <param name="right">The right operand.</param>
		/// <returns>The updated node.</returns>
		public BinaryNode WithRight(ODataNode right)
		{
			return Update(Left, Operation, right, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		protected internal override ODataNode WithParentInternal(ODataNode? parent)
		{
			return Update(Left, Operation, Right, parent);
		}
	}
}
