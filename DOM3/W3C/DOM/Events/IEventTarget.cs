using System;

namespace NMatrix.W3C.DOM.Events
{
	public interface IEventTarget 
	{
		void AddEventListener(string type, IEventListener listener, bool useCapture);
		void RemoveEventListener(string type, IEventListener listener, bool useCapture);
		bool DispatchEvent(IEvent evt); //raises(EventException);
		// Introduced in DOM Level 3:
		void AddGroupedEventListener(string type, IEventListener listener, 
			bool useCapture, IEventGroup evtGroup);
		// Introduced in DOM Level 3:
		void RemoveGroupedEventListener(string type, IEventListener listener, 
			bool useCapture, IEventGroup evtGroup);
		// Introduced in DOM Level 3:
		bool CanTrigger(string type);
		// Introduced in DOM Level 3:
		bool IsRegisteredHere(string type);
	}
}
