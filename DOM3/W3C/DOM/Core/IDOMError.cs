using System;

namespace NMatrix.W3C.DOM.Core
{
	public enum SeverityType
	{
		SEVERITY_WARNING               = 0,
		SEVERITY_ERROR                 = 1,
		SEVERITY_FATAL_ERROR           = 2
	}

	public interface IDOMError
	{
		SeverityType Severity { get; }
		string Message { get; }
		object Exception { get; }
		IDOMLocator Location { get; }
	}
}
