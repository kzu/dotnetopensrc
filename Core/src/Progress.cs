//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// Progress.cs
// Provides a delegate and custom EventArgs class to handle progress 
// notifications from lengthy processes.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;

namespace NMatrix.Core
{
	/// <summary>
	/// A delegate to track what's happening inside our classes or processes.
	/// </summary>
	public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

	/// <summary>
	/// Progress callback event arguments.
	/// </summary>
	public class ProgressEventArgs : EventArgs
	{
		private string _message;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="progressMessage">The message to pass to listeners.</param>
		public ProgressEventArgs(string progressMessage)
		{
			_message = progressMessage;
		}

		/// <summary>
		/// The message to pass to the listeners.
		/// </summary>
		public string Message
		{
			get { return _message; }
		}
	}
}
