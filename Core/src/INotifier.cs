//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// INotifier.cs
// Provides the interface for elements which want to notify the hosting 
// environment of processing progress. Uses Progress.cs file classes.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;

namespace NMatrix.Core
{
	/// <summary>
	/// To implement in components which want to notify the environment.
	/// </summary>
	/// <remarks>A host can check the existence of this interface to 
	/// subscribe to progress notifications from a plug-in or component.</remarks>
	public interface INotifier
	{
		/// <summary>
		/// Provides an event for listeners to receive notifications of lengthy processes.
		/// </summary>
		event ProgressEventHandler Progress;
	}
}
