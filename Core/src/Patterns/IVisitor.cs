//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// IVisitor.cs
// Defines the interfaces for the a visitor, following the Visitor design pattern.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;

namespace NMatrix.Core.Patterns
{
	/// <summary>
	/// The visitor interface implemented by descendents using visitable
	/// objects implementing the visitor pattern. Implements a single 
	/// method, Visit.
	/// </summary>
	public interface IVisitor
	{
		/// <summary>
		/// See Visitor design pattern.
		/// </summary>
		/// <param name="visitable"></param>
		void Visit(object visitable);
	}
}
