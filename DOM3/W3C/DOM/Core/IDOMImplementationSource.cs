using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IDOMImplementationSource
	{
		IDOMImplementation  getDOMImplementation(string features);
	}
}
