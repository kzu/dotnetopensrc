using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IDocument : INode 
	{
		// Modified in DOM Level 3:
		IDocumentType DocType { get; }
		IDOMImplementation Implementation { get; }
		IElement DocumentElement { get; }
		IElement CreateElement(string tagName); //raises(DOMException)
		IDocumentFragment CreateDocumentFragment();
		IText CreateTextNode(string data);
		IComment CreateComment(string data);
		ICDATASection CreateCDATASection(string data); //raises(DOMException)
		IProcessingInstruction CreateProcessingInstruction(string target, string data); //raises(DOMException)
		IAttr CreateAttribute(string name); //raises(DOMException)
		IEntityReference CreateEntityReference(string name); //raises(DOMException)
		INodeList GetElementsByTagName(string tagName);
		// Introduced in DOM Level 2:
		INode ImportNode(INode importedNode, bool deep); //raises(DOMException)
		// Introduced in DOM Level 2:
		IElement CreateElementNS(string namespaceURI, string qualifiedName); //raises(DOMException)
		// Introduced in DOM Level 2:
		IAttr CreateAttributeNS(string namespaceURI, string qualifiedName); //raises(DOMException)
		// Introduced in DOM Level 2:
		INodeList GetElementsByTagNameNS(string namespaceURI, string localName);
		// Introduced in DOM Level 2:
		IElement GetElementById(string elementId);
		// Introduced in DOM Level 3:
		string ActualEncoding { get; set; }
		// Introduced in DOM Level 3:
		string Encoding { get; set; }
		// Introduced in DOM Level 3:
		bool Standalone { get; set; }
		// Introduced in DOM Level 3:
		bool StrictErrorChecking { get; set; }
		// Introduced in DOM Level 3:
		string Version { get; set; }
		// Introduced in DOM Level 3:
		INode AdoptNode(INode source); //raises(DOMException)
		// Introduced in DOM Level 3:
		void SetBaseURI(string baseURI); //raises(DOMException)
	}
}
