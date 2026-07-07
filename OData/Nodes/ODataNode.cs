namespace Skyline.DataMiner.SDM.OData
{
	/// <summary>
	/// Represents a node in the OData syntax tree.
	/// </summary>
	public interface IODataNode
	{
		/// <summary>
		/// Gets the parent node.
		/// </summary>
		ODataNode? Parent { get; }

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor to accept.</param>
		void Accept(IODataVisitor visitor);
	}

	/// <summary>
	/// Provides the base implementation for OData syntax tree nodes.
	/// </summary>
	public abstract class ODataNode : IODataNode
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ODataNode"/> class.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		protected ODataNode(ODataNode? parent = null)
		{
			Parent = parent;
		}

		/// <summary>
		/// Gets the parent node.
		/// </summary>
		public ODataNode? Parent { get; }

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor to accept.</param>
		public abstract void Accept(IODataVisitor visitor);

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		protected internal abstract ODataNode WithParentInternal(ODataNode? parent);
	}

	/// <summary>
	/// Provides a typed base implementation for OData syntax tree nodes.
	/// </summary>
	/// <typeparam name="TNode">The node type.</typeparam>
	public abstract class ODataNode<TNode> : ODataNode
		where TNode : ODataNode
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ODataNode{TNode}"/> class.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		protected ODataNode(ODataNode? parent = null)
			: base(parent)
		{
		}

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		public TNode WithParent(ODataNode? parent)
		{
			return (TNode)WithParentInternal(parent);
		}
	}
}
