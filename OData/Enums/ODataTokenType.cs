namespace Skyline.DataMiner.SDM.OData
{
	/// <summary>
	/// Represents the token types recognized by the OData tokenizer.
	/// </summary>
	public enum ODataTokenType
	{
		// General

		/// <summary>
		/// Indicates the end of the input.
		/// </summary>
		EndOfToken = 0,

		/// <summary>
		/// Represents a system query token.
		/// </summary>
		SystemQuery = 9,

		/// <summary>
		/// Represents an identifier token.
		/// </summary>
		Identifier = 10,

		/// <summary>
		/// Represents a method name token.
		/// </summary>
		Method = 11,

		/// <summary>
		/// Represents a lambda parameter token.
		/// </summary>
		LamdaParameter = 12,

		/// <summary>
		/// Represents the <c>null</c> literal.
		/// </summary>
		Null = 15,

		/// <summary>
		/// Represents a numeric literal.
		/// </summary>
		Number = 16,

		/// <summary>
		/// Represents a string literal.
		/// </summary>
		String = 17,

		// Logicals

		/// <summary>
		/// Represents the OData <c>eq</c> token.
		/// </summary>
		Eq = 100,

		/// <summary>
		/// Represents the OData <c>ne</c> token.
		/// </summary>
		Ne,

		/// <summary>
		/// Represents the OData <c>gt</c> token.
		/// </summary>
		Gt,

		/// <summary>
		/// Represents the OData <c>ge</c> token.
		/// </summary>
		Ge,

		/// <summary>
		/// Represents the OData <c>lt</c> token.
		/// </summary>
		Lt,

		/// <summary>
		/// Represents the OData <c>le</c> token.
		/// </summary>
		Le,

		/// <summary>
		/// Represents the OData <c>and</c> token.
		/// </summary>
		And,

		/// <summary>
		/// Represents the OData <c>or</c> token.
		/// </summary>
		Or,

		/// <summary>
		/// Represents the OData <c>not</c> token.
		/// </summary>
		Not,

		// Arithmetic

		/// <summary>
		/// Represents the OData <c>add</c> token.
		/// </summary>
		Add = 200,

		/// <summary>
		/// Represents the OData <c>sub</c> token.
		/// </summary>
		Sub,

		/// <summary>
		/// Represents the OData <c>mul</c> token.
		/// </summary>
		Mul,

		/// <summary>
		/// Represents the OData <c>div</c> token.
		/// </summary>
		Div,

		/// <summary>
		/// Represents the OData <c>mod</c> token.
		/// </summary>
		Mod,

		// Grouping

		/// <summary>
		/// Represents the <c>(</c> token.
		/// </summary>
		LeftParenthesis = 300,	// '('

		/// <summary>
		/// Represents the <c>)</c> token.
		/// </summary>
		RightParenthesis,       // ')'

		/// <summary>
		/// Represents the <c>{</c> token.
		/// </summary>
		LeftBrace,              // '{'

		/// <summary>
		/// Represents the <c>}</c> token.
		/// </summary>
		RightBrace,             // '}'

		/// <summary>
		/// Represents the <c>,</c> token.
		/// </summary>
		Comma,                  // ','

		/// <summary>
		/// Represents the <c>:</c> token.
		/// </summary>
		Colon,                  // ':'

		/// <summary>
		/// Represents the <c>/</c> token.
		/// </summary>
		Slash,                  // '/'

		// Strings

		/// <summary>
		/// Represents the OData <c>substringof</c> token.
		/// </summary>
		SubStringOf = 400,

		/// <summary>
		/// Represents the OData <c>endswith</c> token.
		/// </summary>
		EndsWith,

		/// <summary>
		/// Represents the OData <c>startswith</c> token.
		/// </summary>
		StartsWith,

		/// <summary>
		/// Represents the OData <c>length</c> token.
		/// </summary>
		Length,

		/// <summary>
		/// Represents the OData <c>indexof</c> token.
		/// </summary>
		IndexOf,

		/// <summary>
		/// Represents the OData <c>replace</c> token.
		/// </summary>
		Replace,

		/// <summary>
		/// Represents the OData <c>substring</c> token.
		/// </summary>
		SubString,

		/// <summary>
		/// Represents the OData <c>tolower</c> token.
		/// </summary>
		ToLower,

		/// <summary>
		/// Represents the OData <c>toupper</c> token.
		/// </summary>
		ToUpper,

		/// <summary>
		/// Represents the OData <c>trim</c> token.
		/// </summary>
		Trim,

		/// <summary>
		/// Represents the OData <c>concat</c> token.
		/// </summary>
		Concat,

		// Date

		/// <summary>
		/// Represents the OData <c>day</c> token.
		/// </summary>
		Day = 500,

		/// <summary>
		/// Represents the OData <c>hour</c> token.
		/// </summary>
		Hour,

		/// <summary>
		/// Represents the OData <c>minute</c> token.
		/// </summary>
		Minute,

		/// <summary>
		/// Represents the OData <c>month</c> token.
		/// </summary>
		Month,

		/// <summary>
		/// Represents the OData <c>second</c> token.
		/// </summary>
		Second,

		/// <summary>
		/// Represents the OData <c>year</c> token.
		/// </summary>
		Year,

		/// <summary>
		/// Represents the OData <c>round</c> token.
		/// </summary>
		Round,

		/// <summary>
		/// Represents the OData <c>floor</c> token.
		/// </summary>
		Floor,

		/// <summary>
		/// Represents the OData <c>ceiling</c> token.
		/// </summary>
		Ceiling,

		// Type

		/// <summary>
		/// Represents the OData <c>isof</c> token.
		/// </summary>
		IsOf = 600,

		// Order

		/// <summary>
		/// Represents the OData <c>asc</c> token.
		/// </summary>
		Ascending = 700,

		/// <summary>
		/// Represents the OData <c>desc</c> token.
		/// </summary>
		Descending,

		// Collections

		/// <summary>
		/// Represents the OData <c>any</c> token.
		/// </summary>
		Any = 800,

		/// <summary>
		/// Represents the OData <c>all</c> token.
		/// </summary>
		All,
	}

	/// <summary>
	/// Provides helper methods for working with <see cref="ODataTokenType"/>.
	/// </summary>
	public static class ODataTokenTypeExtensions
	{
		/// <summary>
		/// Determines whether the token represents a logical operator.
		/// </summary>
		/// <param name="token">The token to inspect.</param>
		/// <returns><see langword="true"/> if the token is logical; otherwise, <see langword="false"/>.</returns>
		public static bool IsLogicalToken(this ODataToken token) => token.Type.IsLogicalToken();

		/// <summary>
		/// Determines whether the token type represents a logical operator.
		/// </summary>
		/// <param name="type">The token type to inspect.</param>
		/// <returns><see langword="true"/> if the token type is logical; otherwise, <see langword="false"/>.</returns>
		public static bool IsLogicalToken(this ODataTokenType type)
		{
			var index = (int)type;
			if (index >= 100 && index <= 199)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Determines whether the token represents a literal value.
		/// </summary>
		/// <param name="token">The token to inspect.</param>
		/// <returns><see langword="true"/> if the token is a literal; otherwise, <see langword="false"/>.</returns>
		public static bool IsLiteral(this ODataToken token) => IsLiteral(token.Type);

		/// <summary>
		/// Determines whether the token type represents a literal value.
		/// </summary>
		/// <param name="type">The token type to inspect.</param>
		/// <returns><see langword="true"/> if the token type is a literal; otherwise, <see langword="false"/>.</returns>
		public static bool IsLiteral(this ODataTokenType type)
		{
			var index = (int)type;
			if (index > 10 && index <= 99)
			{
				return true;
			}

			return false;
		}
	}
}
