using System;

namespace NMatrix.W3C.DOM.Events
{
	// Introduced in DOM Level 2:
	public interface IDocumentEvent 
	{
		IEvent CreateEvent(string eventType); //raises(DOMException);
		IEventGroup CreateEventGroup();
	}
}
