//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ICurrentState.cs
// Interface used to retrieve the CurrentState object with information about
// the transient state in the generation process. 
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;
using System.Xml.Schema;
using System.CodeDom;
using NMatrix.XGoF.Configuration;

namespace NMatrix.XGoF.Host
{
	/// <summary>
	/// Interface used to retrieve the CurrentState object with information about
	/// the transient state in the generation process. 
	/// </summary>
	public interface ICurrentState
	{
		/// <summary>
		/// The current source being processed.
		/// </summary>
		RunnerSource Source
		{ get; }

		/// <summary>
		/// The current code in the output.
		/// </summary>
        CodeCompileUnit Unit
		{ get; }
	}
}
