namespace Skyline.DataMiner.SDM.OData
{
	/// <summary>
	/// Represents a literal value node in the OData syntax tree.
	/// </summary>
	public class LiteralNode : ODataNode<LiteralNode>
	{
		internal LiteralNode(ODataPrimitiveType type, object value, ODataNode? parent = null)
			: base(parent)
		{
			Type = type;
			Value = value;
		}

		/// <summary>
		/// Gets the literal type.
		/// </summary>
		public ODataPrimitiveType Type { get; }

		/// <summary>
		/// Gets the literal value.
		/// </summary>
		public object Value { get; }

		/// <summary>
		/// Returns a copy of this node with the specified type, value, and parent.
		/// </summary>
		/// <param name="type">The literal type.</param>
		/// <param name="value">The literal value.</param>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		public LiteralNode Update(
			ODataPrimitiveType type,
			object value,
			ODataNode? parent = null)
		{
			if (type != Type || value != Value || parent != Parent)
			{
				var newNode = new LiteralNode(type, value, parent);
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
			visitor.VisitLiteral(this);
		}

		/// <summary>
		/// Returns a copy of this node with the specified value.
		/// </summary>
		/// <param name="value">The literal value.</param>
		/// <returns>The updated node.</returns>
		public LiteralNode WithValue(object value)
		{
			return Update(Type, value, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified type.
		/// </summary>
		/// <param name="type">The literal type.</param>
		/// <returns>The updated node.</returns>
		public LiteralNode WithType(ODataPrimitiveType type)
		{
			return Update(type, Value, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		protected internal override ODataNode WithParentInternal(ODataNode? parent)
		{
			return Update(Type, Value, parent);
		}
	}
}
