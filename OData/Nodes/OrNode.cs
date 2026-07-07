namespace Skyline.DataMiner.SDM.OData
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

	/// <summary>
	/// Represents a logical OR node in the OData syntax tree.
	/// </summary>
	public class OrNode : ODataNode<OrNode>
	{
		internal OrNode(IEnumerable<ODataNode> nodes, ODataNode? parent)
			: base(parent)
		{
			var list = new List<ODataNode>();
			foreach(var node in nodes)
			{
				list.Add(node.WithParentInternal(this));
			}

			Nodes = new ReadOnlyCollection<ODataNode>(list);
		}

		/// <summary>
		/// Gets the child nodes.
		/// </summary>
		public IReadOnlyCollection<ODataNode> Nodes { get; }

		/// <summary>
		/// Returns a copy of this node with the specified child nodes and parent.
		/// </summary>
		/// <param name="nodes">The child nodes.</param>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		public OrNode Update(IEnumerable<ODataNode> nodes, ODataNode? parent)
		{
			if (nodes != Nodes || parent != Parent)
			{
				var newNode = new OrNode(nodes, parent);
				return newNode;
			}

			return this;
		}

		/// <summary>
		/// Returns a copy of this node with the specified child nodes.
		/// </summary>
		/// <param name="nodes">The child nodes.</param>
		/// <returns>The updated node.</returns>
		public OrNode WithNodes(IEnumerable<ODataNode> nodes)
		{
			return Update(nodes, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified child node appended.
		/// </summary>
		/// <param name="node">The child node to add.</param>
		/// <returns>The updated node.</returns>
		public OrNode AddNode(ODataNode node)
		{
			var nodes = Nodes.ToList();
			nodes.Add(node);
			return Update(new ReadOnlyCollection<ODataNode>(nodes), Parent);
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor to accept.</param>
		public override void Accept(IODataVisitor visitor)
		{
			visitor.VisitOr(this);
		}

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		protected internal override ODataNode WithParentInternal(ODataNode? parent)
		{
			return Update(Nodes, parent);
		}
	}
}
