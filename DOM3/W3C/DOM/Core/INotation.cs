using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface INotation : INode 
	{
		string PublicId { get; }
		string SystemId { get; }
	}
}
