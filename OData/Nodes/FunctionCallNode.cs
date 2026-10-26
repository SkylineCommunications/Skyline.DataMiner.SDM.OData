namespace Skyline.DataMiner.SDM.OData
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

	/// <summary>
	/// Represents a function call node in the OData syntax tree.
	/// </summary>
	public class FunctionCallNode : ODataNode<FunctionCallNode>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FunctionCallNode"/> class.
		/// </summary>
		/// <param name="methodName">The function name.</param>
		/// <param name="arguments">The function arguments.</param>
		/// <param name="parent">The parent node.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="methodName"/> or <paramref name="arguments"/> is <see langword="null"/>.</exception>
		public FunctionCallNode(string methodName, IEnumerable<ODataNode> arguments, ODataNode? parent = null)
			: base(parent)
		{
			Name = methodName ?? throw new ArgumentNullException(nameof(methodName));

			if (arguments is null)
			{
				throw new ArgumentNullException(nameof(arguments));
			}

			var list = new List<ODataNode>();
			foreach (var argument in arguments)
			{
				list.Add(argument.WithParentInternal(this));
			}

			Arguments = new ReadOnlyCollection<ODataNode>(list);
		}

		/// <summary>
		/// Gets the function name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the function arguments.
		/// </summary>
		public IReadOnlyList<ODataNode> Arguments { get; }

		/// <summary>
		/// Returns a copy of this node with the specified name, arguments, and parent.
		/// </summary>
		/// <param name="methodName">The function name.</param>
		/// <param name="arguments">The function arguments.</param>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		public FunctionCallNode Update(
			string methodName,
			IEnumerable<ODataNode> arguments,
			ODataNode? parent)
		{
			if (Name != methodName ||
				!Arguments.SequenceEqual(arguments) ||
				Parent != parent)
			{
				var newNode = new FunctionCallNode(methodName, arguments, parent);
				return newNode;
			}

			return this;
		}

		/// <summary>
		/// Returns a copy of this node with the specified function name.
		/// </summary>
		/// <param name="methodName">The function name.</param>
		/// <returns>The updated node.</returns>
		public FunctionCallNode WithName(string methodName)
		{
			return Update(methodName, Arguments, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified arguments.
		/// </summary>
		/// <param name="arguments">The function arguments.</param>
		/// <returns>The updated node.</returns>
		public FunctionCallNode WithArgument(IEnumerable<ODataNode> arguments)
		{
			return Update(Name, arguments, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified argument appended.
		/// </summary>
		/// <param name="argument">The argument to add.</param>
		/// <returns>The updated node.</returns>
		public FunctionCallNode AddArgument(ODataNode argument)
		{
			var arguments = Arguments.ToList();
			arguments.Add(argument);
			return Update(Name, arguments, Parent);
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor to accept.</param>
		public override void Accept(IODataVisitor visitor)
		{
			visitor.VisitFunctionCall(this);
		}

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		protected internal override ODataNode WithParentInternal(ODataNode? parent)
		{
			return Update(Name, Arguments, parent);
		}
	}
}
