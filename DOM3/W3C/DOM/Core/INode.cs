using System;

namespace NMatrix.W3C.DOM.Core
{
	public enum NodeType
	{
		ELEMENT_NODE                   = 1,
		ATTRIBUTE_NODE                 = 2,
		TEXT_NODE                      = 3,
		CDATA_SECTION_NODE             = 4,
		ENTITY_REFERENCE_NODE          = 5,
		ENTITY_NODE                    = 6,
		PROCESSING_INSTRUCTION_NODE    = 7,
		COMMENT_NODE                   = 8,
		DOCUMENT_NODE                  = 9,
		DOCUMENT_TYPE_NODE             = 10,
		DOCUMENT_FRAGMENT_NODE         = 11,
		NOTATION_NODE                  = 12
	}

	public enum TreePosition
	{
		TREE_POSITION_PRECEDING        = 0x01,
		TREE_POSITION_FOLLOWING        = 0x02,
		TREE_POSITION_ANCESTOR         = 0x04,
		TREE_POSITION_DESCENDANT       = 0x08,
		TREE_POSITION_SAME             = 0x10,
		TREE_POSITION_EXACT_SAME       = 0x20,
		TREE_POSITION_DISCONNECTED     = 0x00
	}

	public interface INode 
	{
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
	}
}
