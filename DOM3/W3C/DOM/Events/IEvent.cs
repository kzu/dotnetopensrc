using System;

namespace NMatrix.W3C.DOM.Events
{
	public enum PhaseType
	{
		Capturing = 1,
		Target = 2,
		Bubbling= 3
	}

	public interface IEvent 
	{
		string Type { get; }
		IEventTarget Target { get; }
		IEventTarget CurrentTarget { get; }
		PhaseType EventPhase { get; }
		bool Bubbles { get; }
		bool Cancelable { get; }
		DateTime TimeStamp { get; }
		void StopPropagation();
		void PreventDefault();
		void InitEvent(string eventType, bool canBubble, bool cancelable);
	}
}
