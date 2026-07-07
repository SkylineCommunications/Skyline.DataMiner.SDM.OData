namespace Skyline.DataMiner.SDM.OData
{
	using Skyline.DataMiner.SDM.OData.Exceptions;

	/// <summary>
	/// Represents an OData logical operator.
	/// </summary>
	public enum ODataLogicalOperation
	{
		/// <summary>
		/// Represents the OData <c>eq</c> operator.
		/// </summary>
		Equal,

		/// <summary>
		/// Represents the OData <c>ne</c> operator.
		/// </summary>
		NotEqual,

		/// <summary>
		/// Represents the OData <c>gt</c> operator.
		/// </summary>
		GreaterThan,

		/// <summary>
		/// Represents the OData <c>ge</c> operator.
		/// </summary>
		GreaterThanOrEqual,

		/// <summary>
		/// Represents the OData <c>lt</c> operator.
		/// </summary>
		LessThan,

		/// <summary>
		/// Represents the OData <c>le</c> operator.
		/// </summary>
		LessThanOrEqual,

		/// <summary>
		/// Represents the OData <c>and</c> operator.
		/// </summary>
		And,

		/// <summary>
		/// Represents the OData <c>or</c> operator.
		/// </summary>
		Or,

		/// <summary>
		/// Represents the OData <c>not</c> operator.
		/// </summary>
		Not,
	}

	internal static class ODataLogicalOperationExtensions
	{
		public static ODataLogicalOperation ToLogicalOperator(this ODataToken token)
		{
			if (!token.Type.IsLogicalToken())
			{
				throw new ODataParseException("Could not parse token to ODataLogationOperation");
			}

			return token.Text switch
			{
				"eq" => ODataLogicalOperation.Equal,
				"ne" => ODataLogicalOperation.NotEqual,
				"gt" => ODataLogicalOperation.GreaterThan,
				"ge" => ODataLogicalOperation.GreaterThanOrEqual,
				"lt" => ODataLogicalOperation.LessThan,
				"le" => ODataLogicalOperation.LessThanOrEqual,
				"not" => ODataLogicalOperation.Not,

				_ => throw new ODataParseException("Could not parse token to ODataLogationOperation"),
			};
		}
	}
}
