using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface ICharacterData : INode 
	{
		string Data { get; set; }
		// raises(DOMException) on setting
		// raises(DOMException) on retrieval

		int Length { get; set; }
		string SubstringData(int offset, int count); //raises(DOMException);
		void AppendData(string arg); //raises(DOMException);
		void InsertData(int offset, string arg); //raises(DOMException);
		void DeleteData(int offset, int count); //raises(DOMException);
		void ReplaceData(int offset, int count, string arg);  //raises(DOMException);
	}
}
