using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IAttr : INode 
	{
		string Name { get; }
		bool Specified { get; }
		string Value { get; set; } // raises(DOMException) on setting
		// Introduced in DOM Level 2:
		IElement OwnerElement { get; }
	}
}
