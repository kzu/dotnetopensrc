using System;
using System.Runtime.Serialization;

namespace NMatrix.W3C.DOM.Core
{
	public enum ExceptionCode
	{
		INDEX_SIZE_ERR                 = 1,
		DOMSTRING_SIZE_ERR             = 2,
		HIERARCHY_REQUEST_ERR          = 3,
		WRONG_DOCUMENT_ERR             = 4,
		INVALID_CHARACTER_ERR          = 5,
		NO_DATA_ALLOWED_ERR            = 6,
		NO_MODIFICATION_ALLOWED_ERR    = 7,
		NOT_FOUND_ERR                  = 8,
		NOT_SUPPORTED_ERR              = 9,
		INUSE_ATTRIBUTE_ERR            = 10,
		// Introduced in DOM Level 2:
		INVALID_STATE_ERR              = 11,
		// Introduced in DOM Level 2:
		SYNTAX_ERR                     = 12,
		// Introduced in DOM Level 2:
		INVALID_MODIFICATION_ERR       = 13,
		// Introduced in DOM Level 2:
		NAMESPACE_ERR                  = 14,
		// Introduced in DOM Level 2:
		INVALID_ACCESS_ERR             = 15
	}

	[Serializable]
	public class DOMException : ApplicationException, ISerializable 
	{
		ExceptionCode _code;

		public DOMException(ExceptionCode code)
			: this(code, String.Empty, null)
		{
		}

		public DOMException(ExceptionCode code, string message)
			: this(code, message, null)
		{
		}

		public DOMException(ExceptionCode code, string message, Exception innerException)
			: base(message, innerException)
		{
			if (!Enum.IsDefined(typeof(ExceptionCode), code))
                throw new ApplicationException("ExceptionCode is undefined");
			_code = code;
		}

		protected DOMException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_code = (ExceptionCode) info.GetValue("Code", typeof(ExceptionCode));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("code", _code);
		}

		public ExceptionCode Code 
		{
			get { return _code; }
		}
	}
}
