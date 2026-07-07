namespace Skyline.DataMiner.SDM.OData
{
	/// <summary>
	/// Represents the root filter node in the OData syntax tree.
	/// </summary>
	public class FilterNode : ODataNode<FilterNode>
	{
		/// <summary>
		/// Represents an empty filter.
		/// </summary>
		public static readonly FilterNode Empty = new FilterNode(null);

		internal FilterNode(ODataNode? filter = null)
			: base(null)
		{
			Filter = filter?.WithParentInternal(this);
		}

		/// <summary>
		/// Gets the filter expression.
		/// </summary>
		public ODataNode? Filter { get; }

		/// <summary>
		/// Returns a copy of this node with the specified filter expression.
		/// </summary>
		/// <param name="filter">The filter expression.</param>
		/// <returns>The updated node.</returns>
		public FilterNode Update(
			ODataNode? filter)
		{
			if(filter != Filter)
			{
				var newNode = new FilterNode(filter);
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
			visitor.VisitFilter(this);
		}

		/// <summary>
		/// Returns a copy of this node with the specified filter expression.
		/// </summary>
		/// <param name="filter">The filter expression.</param>
		/// <returns>The updated node.</returns>
		public FilterNode WithFilter(ODataNode? filter)
		{
			return Update(filter);
		}

		/// <summary>
		/// Returns this instance because filter nodes do not have a parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>This node.</returns>
		protected internal override ODataNode WithParentInternal(ODataNode? parent)
		{
			// Filter nodes cannot have a parent they are top level nodes
			return this;
		}
	}
}
