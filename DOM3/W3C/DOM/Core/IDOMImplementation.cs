using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IDOMImplementation
	{
		bool HasFeature(string feature, string version);
		// Introduced in DOM Level 2:
		IDocumentType createDocumentType(string qualifiedName, string publicId, string systemId);
		// Introduced in DOM Level 2:
		IDocument createDocument(string namespaceURI, string qualifiedName, IDocumentType doctype);
		// Introduced in DOM Level 3:
		IDOMImplementation getInterface(string feature);
	}
}
