using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface INamedNodeMap 
	{
		INode NamedItem { get; set; } //raises(DOMException);
		INode RemoveNamedItem(string name); //raises(DOMException);
		INode Item(int index);
		int Length { get; }
		// Introduced in DOM Level 2:
		INode GetNamedItemNS(string namespaceURI, string localName);
		// Introduced in DOM Level 2:
		INode SetNamedItemNS(INode arg); //raises(DOMException);
		// Introduced in DOM Level 2:
		INode RemoveNamedItemNS(string namespaceURI, string localName); //raises(DOMException);
	}
}
