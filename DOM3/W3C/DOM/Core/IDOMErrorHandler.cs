using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IDOMErrorHandler 
	{
		bool HandleError(IDOMError error);
	}
}
