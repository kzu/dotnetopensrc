//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// CurrentState.cs
// Provides information about changing state in the processing. 
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;
using System.Xml.Schema;
using System.CodeDom;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.Customization;

namespace NMatrix.XGoF.Host
{
	/// <summary>
	/// Provides information about changing state in the processing.
	/// </summary>
	public class CurrentState : ICurrentState
	{
		private RunnerSource _source;
		private CodeCompileUnit _unit;
        
		/// <summary>
		/// Default constructor.
		/// </summary>
		public CurrentState()
		{
		}

		/// <summary>
		/// Reinitialize values for the current processing state.
		/// </summary>
		/// <param name="source">The current source in use.</param>
		/// <param name="unit">The code graph to compile.</param>
		internal void Init(RunnerSource source, CodeCompileUnit unit)
		{
			_source = source;
			_unit = unit;
		}

		/// <summary>
		/// The current source being processed.
		/// </summary>
		public RunnerSource Source
		{
			get { return _source; }
		}

		/// <summary>
		/// The current code in the output.
		/// </summary>
        public CodeCompileUnit Unit
		{
			get { return _unit; }
		}
	}
}
