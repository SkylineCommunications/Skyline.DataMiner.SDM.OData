namespace Skyline.DataMiner.SDM.OData
{
	using System;

	/// <summary>
	/// Represents an order-by clause node in the OData syntax tree.
	/// </summary>
	public class OrderNode : ODataNode<OrderNode>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OrderNode"/> class.
		/// </summary>
		/// <param name="property">The property to order by.</param>
		/// <param name="order">The sort direction.</param>
		/// <param name="next">The next order clause.</param>
		/// <param name="parent">The parent node.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <see langword="null"/>.</exception>
		public OrderNode(PropertyNode property, ODataOrder order, OrderNode? next, ODataNode? parent = null) : base(parent)
		{
			Property = property?.WithParent(this) ?? throw new ArgumentNullException(nameof(property));
			Next = next?.WithParent(this);
			Order = order;
		}

		/// <summary>
		/// Gets the property to order by.
		/// </summary>
		public PropertyNode Property { get; }

		/// <summary>
		/// Gets the sort direction.
		/// </summary>
		public ODataOrder Order { get; }

		/// <summary>
		/// Gets the next order clause.
		/// </summary>
		public OrderNode? Next { get; }

		/// <summary>
		/// Returns a copy of this node with the specified values.
		/// </summary>
		/// <param name="property">The property to order by.</param>
		/// <param name="order">The sort direction.</param>
		/// <param name="next">The next order clause.</param>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		public OrderNode Update(PropertyNode property, ODataOrder order, OrderNode? next, ODataNode? parent)
		{
			if (property != Property || order != Order || next != Next || parent != Parent)
			{
				var newNode = new OrderNode(property, order, next, parent);
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
			visitor.VisitOrder(this);
		}

		/// <summary>
		/// Returns a copy of this node with the specified property.
		/// </summary>
		/// <param name="property">The property to order by.</param>
		/// <returns>The updated node.</returns>
		public OrderNode WithProperty(PropertyNode property)
		{
			return Update(property, Order, Next, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified sort direction.
		/// </summary>
		/// <param name="order">The sort direction.</param>
		/// <returns>The updated node.</returns>
		public OrderNode WithOrder(ODataOrder order)
		{
			return Update(Property, order, Next, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified next order clause.
		/// </summary>
		/// <param name="next">The next order clause.</param>
		/// <returns>The updated node.</returns>
		public OrderNode WithNext(OrderNode? next)
		{
			return Update(Property, Order, next, Parent);
		}

		/// <summary>
		/// Returns a copy of this node with the specified parent.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <returns>The updated node.</returns>
		protected internal override ODataNode WithParentInternal(ODataNode? parent)
		{
			return Update(Property, Order, Next, parent);
		}
	}
}
