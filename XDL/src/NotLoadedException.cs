using System;

namespace NMatrix.XDL
{
	public class NotLoadedException : InvalidOperationException
	{
		public NotLoadedException() : base()
		{
		}

		public NotLoadedException(string message) : base(message)
		{
		}

		public NotLoadedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
