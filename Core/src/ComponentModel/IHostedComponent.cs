//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// IHostedComponent.cs
// Provides the interface for components to be initialized inside the host 
// environment 
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;

namespace NMatrix.Core.ComponentModel
{
	/// <summary>
	/// A component which is initialized inside the hosting environment.
	/// </summary>
	public interface IHostedComponent
	{
	 	/// <summary>
		/// Initializes the component passing the environment host variable.
		/// </summary>
		/// <param name="environment">The host reference</param>
		void Initialize(IServiceProvider environment);
	}
}
