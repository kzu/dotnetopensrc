using System;
using System.Runtime.Serialization;

namespace NMatrix.W3C.DOM.Events
{
	// Introduced in DOM Level 2:
	[Serializable]
	public class EventException : ApplicationException, ISerializable 
	{
		int _code;

		public const int UNSPECIFIED_EVENT_TYPE_ERR = 0;

		public EventException()
			: this(EventException.UNSPECIFIED_EVENT_TYPE_ERR)
		{
		}

		public EventException(int code)
			: this(code, String.Empty)
		{
		}

		public EventException(int code, string message)
			: this(code, message, null)
		{
		}

		public EventException(int code, string message, Exception innerException)
			: base(message, innerException)
		{
			_code = code;
		}

		protected EventException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_code = info.GetInt32("code");
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("code", _code);
		}

		public int Code 
			{
			get { return _code; }
		}
	}
}
