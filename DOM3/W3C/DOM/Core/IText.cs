using System;

namespace NMatrix.W3C.DOM.Core
{
	public interface IText : ICharacterData 
	{
		IText SplitText(int offset); //raises(DOMException);
		// Introduced in DOM Level 3:
		bool IsWhitespaceInElementContent { get; }
		// Introduced in DOM Level 3:
		string WholeText { get; }
		// Introduced in DOM Level 3:
		IText ReplaceWholeText(string content); //raises(DOMException);
	}
}
