using System;

namespace NMatrix.W3C.DOM.Core
{
	public enum OperationType
	{
		CLONED                         = 1,
		IMPORTED                       = 2,
		DELETED                        = 3
	}

	public interface IUserDataHandler 
	{
		void Handle(OperationType operation, string key, object data, INode src, INode dst);
	}
}
