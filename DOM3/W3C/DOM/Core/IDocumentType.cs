using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IDocumentType : INode 
	{
		string Name { get; }
		INamedNodeMap Entities { get; }
		INamedNodeMap Notations { get; }
		// Introduced in DOM Level 2:
		string PublicId { get; }
		// Introduced in DOM Level 2:
		string SystemId { get; }
		// Introduced in DOM Level 2:
		string InternalSubset { get; }
	}
}
