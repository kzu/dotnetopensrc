//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// IVisitable.cs
// Defines an object that can be visited following the Visitor design pattern.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;

namespace NMatrix.Core.Patterns
{
	/// <summary>
	/// The interface for visitable objects in the visitor pattern.
	/// Implements only one method, Accept.
	/// </summary>
	public interface IVisitable
	{
		/// <summary>
		/// See Visitor design pattern.
		/// </summary>
		/// <param name="visitor"></param>
		void Accept(IVisitor visitor);
	}
}
