namespace Skyline.DataMiner.SDM.OData
{
	using System;

	/// <summary>
	/// Represents a property path node in the OData syntax tree.
	/// </summary>
	public class PropertyNode : ODataNode<PropertyNode>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyNode"/> class.
		/// </summary>
		/// <param name="path">The property path.</param>
		/// <param name="parent">The parent node.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is <see langword="null"/>.</exception>
		public PropertyNode(string path, ODataNode? parent = null)
			: base(parent)
		{
			Path = path ?? throw new ArgumentNullException(nameof(path));
		}

		/// <summary>
		/// Gets the property path.
		/// </summary>
		public string Path { get; }

		/// <summary>
		/// Returns a copy of this node with the specified path and parent.
		/// </summary>
		/// <param name="path">The property path.</param>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		public PropertyNode Update(
			string path,
			ODataNode? parent)
		{
			if (path != Path || parent != Parent)
			{
				var newNode = new PropertyNode(path, parent);
				return newNode;
			}

			return this;
		}

		/// <summary>
		/// Returns a copy of this node with the specified path.
		/// </summary>
		/// <param name="path">The property path.</param>
		/// <returns>The updated node.</returns>
		public PropertyNode WithPath(string path)
		{
			return Update(path, Parent);
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor to accept.</param>
		public override void Accept(IODataVisitor visitor)
		{
			visitor.VisitProperty(this);
		}

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		protected internal override ODataNode WithParentInternal(ODataNode? parent)
		{
			return Update(Path, parent);
		}
	}
}
