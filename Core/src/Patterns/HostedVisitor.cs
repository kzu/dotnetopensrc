//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// HostedVisitor.cs
// Provides initialization of the IHostedComponent interface.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using NMatrix.Core.ComponentModel;

namespace NMatrix.Core.Patterns
{
	/// <summary>
	/// A visitor hosted in the generator environment.
	/// </summary>
	public abstract class HostedVisitor : Visitor, IHostedComponent
	{
		/// <summary>
		/// Provides descendent classes access to the service provider.
		/// </summary>
		protected IServiceProvider Host;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public HostedVisitor()
		{
		}

	 	/// <summary>
		/// Initializes the component passing the environment host variable.
		/// </summary>
		/// <param name="environment">The host reference</param>
		public virtual void Initialize(IServiceProvider environment)
		{
			Host = environment;
		}
	}
}
