using System;
using System.Xml;
using NMatrix.W3C.DOM.Core;

namespace NMatrix.Dom.Core
{
	public class DOMImplementation : XmlImplementation//, IDOMImplementation
	{
		/// <summary>
		/// Holds the name table shared by all the document instances created
		/// from this implementation.
		/// </summary>
		NameTable _table = new NameTable();

		public new bool HasFeature(string feature, string version)
		{
			if (feature == "Events")
				return true;
			return base.HasFeature(feature, version);
		}

		// Introduced in DOM Level 2:
		public override XmlDocument CreateDocument()
		{
			return new DOMDocument(_table);
		}
		
		/*
		// Introduced in DOM Level 2:
		IDocumentType createDocumentType(string qualifiedName, string publicId, string systemId);
s		// Introduced in DOM Level 3:
		IDOMImplementation getInterface(string feature);
		*/
	}
}
