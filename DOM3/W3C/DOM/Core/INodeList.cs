using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface INodeList 
	{
		INode Item(int index);
		int Length { get; }
	}
}
