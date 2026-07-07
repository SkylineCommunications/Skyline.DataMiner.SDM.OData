namespace Skyline.DataMiner.SDM.OData.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// The exception that is thrown when an OData expression cannot be parsed.
	/// </summary>
	[Serializable]
	public class ODataParseException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ODataParseException"/> class.
		/// </summary>
		public ODataParseException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ODataParseException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The error message.</param>
		public ODataParseException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ODataParseException"/> class with a specified error message and inner exception.
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <param name="innerException">The exception that caused the current exception.</param>
		public ODataParseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ODataParseException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The serialization data.</param>
		/// <param name="context">The streaming context.</param>
		protected ODataParseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
