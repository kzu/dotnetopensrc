//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ITraverser.cs
// Interface for use by extenders which want to provide their own traversing 
// mechanism.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml.Schema;
using NMatrix.Core.ComponentModel;
using NMatrix.Core.Patterns;

namespace NMatrix.XGoF
{
	/// <summary>
	/// Interface for use by extenders to provide custom traversing of the XmlSchema.
	/// </summary>
	public interface ITraverser : IHostedComponent
	{
		/// <summary>
		/// Starts traversal of the file passed and returns the loaded component.
		/// </summary>
		/// <param name="sourceFile">The source file to traverse.</param>
		IVisitableComponent Traverse(string sourceFile);
	}
}
