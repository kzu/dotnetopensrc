using System;
using System.Runtime.Serialization;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Represents the an error in the Schematron schema.
	/// </summary>
	/// <author ref="dcazzulino" />
	/// <progress amount="100" />
	[Serializable()]
	public class ValidationException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationException"/> class.
		/// </summary>
		public ValidationException()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public ValidationException(string message) : base(message)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationException"/> class.
		/// </summary>
		/// <param name="info">Info</param>
		/// <param name="context">Context</param>
		protected ValidationException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public ValidationException(string message, Exception innerException) :
			base(message, innerException)
		{
		}
	}
}
