using System;
using System.Xml;
using NMatrix.W3C.DOM.Core;

namespace NMatrix.Dom.Core
{
	public class DOMDocument : XmlDocument//, IDocument
	{
		public DOMDocument() : base()
		{
		}

		public DOMDocument(XmlNameTable nt) : base(nt)
		{
		}
	}
}
