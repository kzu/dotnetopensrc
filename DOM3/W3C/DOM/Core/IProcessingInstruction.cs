using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IProcessingInstruction : INode 
	{
		string Target { get; }
		string Data { get; set; } // raises(DOMException) on setting
	}
}
