using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IDOMLocator 
	{
		int LineNumber { get; }
		int ColumnNumber { get; }
		int Offset { get; }
		INode ErrorNode { get; }
		string Uri { get; }
	}
}
