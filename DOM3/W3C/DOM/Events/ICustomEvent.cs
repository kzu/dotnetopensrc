using System;
using NMatrix.W3C.DOM.Core;

namespace NMatrix.W3C.DOM.Events
{
	// Introduced in DOM Level 3:
	public interface ICustomEvent : IEvent 
	{
		void SetCurrentTarget(INode target);
		void SetEventPhase(PhaseType phase);
	}
}
