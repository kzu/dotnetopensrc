using System;
using System.Xml;
using NMatrix.W3C.DOM.Core;

namespace NMatrix.NMatrix.Dom.Core
{
	public class Node : XmlNode, INode
	{
		/*
		string NodeName { get; }
		string NodeValue { get; set; } // raises(DOMException) on setting // raises(DOMException) on retrieval
		NodeType NodeType { get; }
		INode ParentNode { get; }
		INodeList ChildNodes { get; }
		INode FirstChild { get; }
		INode LastChild { get; }
		INode PreviousSibling { get; }
		INode NextSibling { get; }
		INamedNodeMap Attributes { get; }
		// Modified in DOM Level 2:
		IDocument OwnerDocument { get; }
		// Modified in DOM Level 3:
		INode InsertBefore(INode newChild, INode refChild); //raises(DOMException);
		// Modified in DOM Level 3:
		INode ReplaceChild(INode newChild, INode oldChild); //raises(DOMException);
		// Modified in DOM Level 3:
		INode RemoveChild(INode oldChild); //raises(DOMException);
		INode AppendChild(INode newChild); //raises(DOMException);
		bool HasChildNodes();
		INode CloneNode(bool deep);
		// Modified in DOM Level 2:
		void Normalize();
		// Introduced in DOM Level 2:
		bool IsSupported(string feature, string version);
		// Introduced in DOM Level 2:
		string namespaceURI { get; }
		// Introduced in DOM Level 2:
		string prefix { get; set; } // raises(DOMException) on setting
		// Introduced in DOM Level 2:
		string LocalName { get; }
		// Introduced in DOM Level 2:
		bool HasAttributes();
		// Introduced in DOM Level 3:
		string baseURI { get; }


		// Introduced in DOM Level 3:
		TreePosition CompareTreePosition(INode other); //raises(DOMException);
		// Introduced in DOM Level 3:
		string TextContent { get; set; }
		// raises(DOMException) on setting
		// raises(DOMException) on retrieval

		// Introduced in DOM Level 3:
		bool IsSameNode(INode other);
		// Introduced in DOM Level 3:
		string LookupNamespacePrefix(string namespaceURI);
		// Introduced in DOM Level 3:
		string LookupNamespaceURI(string prefix);
		// Introduced in DOM Level 3:
		void NormalizeNS();
		// Introduced in DOM Level 3:
		bool IsEqualNode(INode arg, bool deep);
		// Introduced in DOM Level 3:
		INode GetInterface(string feature);
		// Introduced in DOM Level 3:
		object SetUserData(string key, object data, IUserDataHandler handler);
		// Introduced in DOM Level 3:
		object GetUserData(string key);
		*/
	}
}
