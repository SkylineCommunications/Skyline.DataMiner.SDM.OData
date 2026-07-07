namespace Skyline.DataMiner.SDM.OData
{
	/// <summary>
	/// Represents a token produced by the OData tokenizer.
	/// </summary>
	internal class ODataToken
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ODataToken"/> class.
		/// </summary>
		/// <param name="type">The token type.</param>
		/// <param name="txt">The token text.</param>
		/// <param name="pos">The token position.</param>
		public ODataToken(ODataTokenType type, string txt, int pos)
		{
			Type = type;
			Text = txt;
			Position = pos;
		}

		/// <summary>
		/// Gets the token type.
		/// </summary>
		public ODataTokenType Type { get; }

		/// <summary>
		/// Gets the token text.
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// Gets the zero-based token position.
		/// </summary>
		public int Position { get; }

		/// <summary>
		/// Returns a string representation of the token.
		/// </summary>
		/// <returns>The token text representation.</returns>
		public override string ToString()
		{
			return $"{Type}('{Text}')";
		}
	}
}
