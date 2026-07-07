namespace Skyline.DataMiner.SDM.OData
{
	using System;

	/// <summary>
	/// Represents a logical NOT node in the OData syntax tree.
	/// </summary>
	public class NotNode : ODataNode<NotNode>
	{
		internal NotNode(ODataNode child, ODataNode? parent = null)
			: base(parent)
		{
			Child = child?.WithParentInternal(this) ?? throw new ArgumentNullException(nameof(child));
		}

		/// <summary>
		/// Gets the logical operation.
		/// </summary>
		public ODataLogicalOperation Operation { get; } = ODataLogicalOperation.Not;

		/// <summary>
		/// Gets the child node.
		/// </summary>
		public ODataNode Child { get; }

		/// <summary>
		/// Returns a copy of this node with the specified child and parent.
		/// </summary>
		/// <param name="child">The child node.</param>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		public NotNode Update(
			ODataNode child,
			ODataNode? parent)
		{
			if (child != Child || parent != Parent)
			{
				var newNode = new NotNode(child, parent);
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
			visitor.VisitNot(this);
		}

		/// <summary>
		/// Returns a copy of this node with the specified child.
		/// </summary>
		/// <param name="child">The child node.</param>
		/// <returns>The updated node.</returns>
		public NotNode WithChild(ODataNode child)
		{
			return Update(child, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		protected internal override ODataNode WithParentInternal(ODataNode? parent)
		{
			return Update(Child, parent);
		}
	}
}
