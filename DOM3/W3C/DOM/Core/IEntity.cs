using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IEntity : INode 
	{
		string PublicId { get; }
		string SystemId { get; }
		string NotationName { get; }
		// Introduced in DOM Level 3:
		string ActualEncoding { get; set; }
		// Introduced in DOM Level 3:
		string Encoding { get; set; }
		// Introduced in DOM Level 3:
		string Version { get; set; }
	}
}
