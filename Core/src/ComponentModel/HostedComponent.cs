//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// HostedComponent.cs
// Provides a default implementation of the IHostedComponent interface.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;

namespace NMatrix.Core.ComponentModel
{
	/// <summary>
	/// A component hosted in the generator environment.
	/// </summary>
	public abstract class HostedComponent : IHostedComponent
	{
		/// <summary>
		/// Provides descendent classes access to the service provider.
		/// </summary>
		protected IServiceProvider Host;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public HostedComponent()
		{
		}

	 	/// <summary>
		/// Initializes the component passing the environment host variable.
		/// </summary>
		/// <param name="environment">The host reference</param>
		public void Initialize(IServiceProvider environment)
		{
			Host = environment;
		}
	}
}
