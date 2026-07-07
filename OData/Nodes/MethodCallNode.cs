namespace Skyline.DataMiner.SDM.OData
{
	using System;

	/// <summary>
	/// Represents a collection method call node in the OData syntax tree.
	/// </summary>
	public class MethodCallNode : ODataNode<MethodCallNode>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodCallNode"/> class.
		/// </summary>
		/// <param name="methodName">The method name.</param>
		/// <param name="target">The collection target.</param>
		/// <param name="variable">The lambda variable.</param>
		/// <param name="body">The method body.</param>
		/// <param name="parent">The parent node.</param>
		/// <exception cref="ArgumentNullException">Thrown when a required argument is <see langword="null"/>.</exception>
		public MethodCallNode(string methodName, PropertyNode target, PropertyNode variable, ODataNode body, ODataNode? parent = null)
			: base(parent)
		{
			Name = methodName ?? throw new ArgumentNullException(nameof(methodName));
			Target = target ?? throw new ArgumentNullException(nameof(target));
			Variable = variable ?? throw new ArgumentNullException(nameof(variable));
			Body = body?.WithParentInternal(this) ?? throw new ArgumentNullException(nameof(body));
		}

		/// <summary>
		/// Gets the method name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the target collection.
		/// </summary>
		public PropertyNode Target { get; }

		/// <summary>
		/// Gets the lambda variable.
		/// </summary>
		public PropertyNode Variable { get; }

		/// <summary>
		/// Gets the method body.
		/// </summary>
		public ODataNode Body { get; }

		/// <summary>
		/// Returns a copy of this node with the specified values.
		/// </summary>
		/// <param name="methodName">The method name.</param>
		/// <param name="target">The collection target.</param>
		/// <param name="variable">The lambda variable.</param>
		/// <param name="body">The method body.</param>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		public MethodCallNode Update(
			string methodName,
			PropertyNode target,
			PropertyNode variable,
			ODataNode body,
			ODataNode? parent)
		{
			if (Name != methodName ||
				Target != target ||
				Variable != variable ||
				Body != body ||
				Parent != parent)
			{
				var newNode = new MethodCallNode(methodName, target, variable, body, parent);
				return newNode;
			}

			return this;
		}

		/// <summary>
		/// Returns a copy of this node with the specified method name.
		/// </summary>
		/// <param name="methodName">The method name.</param>
		/// <returns>The updated node.</returns>
		public MethodCallNode WithName(string methodName)
		{
			return Update(methodName, Target, Variable, Body, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified target.
		/// </summary>
		/// <param name="target">The collection target.</param>
		/// <returns>The updated node.</returns>
		public MethodCallNode WithTarget(PropertyNode target)
		{
			return Update(Name, target, Variable, Body, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified variable.
		/// </summary>
		/// <param name="variable">The lambda variable.</param>
		/// <returns>The updated node.</returns>
		public MethodCallNode WithVariable(PropertyNode variable)
		{
			return Update(Name, Target, variable, Body, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified body.
		/// </summary>
		/// <param name="body">The method body.</param>
		/// <returns>The updated node.</returns>
		public MethodCallNode WithBody(ODataNode body)
		{
			return Update(Name, Target, Variable, body, Parent);
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor to accept.</param>
		public override void Accept(IODataVisitor visitor)
		{
			visitor.VisitMethodCall(this);
		}

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		protected internal override ODataNode WithParentInternal(ODataNode? parent)
		{
			return Update(Name, Target, Variable, Body, parent);
		}
	}
}
