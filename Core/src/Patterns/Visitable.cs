//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// Visitable.cs
// Defines a base visitable class for simple classes implementing the Visitor pattern.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;

namespace NMatrix.Core.Patterns
{
	/// <summary>
	/// Abstract base class for visitable classes.
	/// </summary>
	/// <remarks>To implement the visitor pattern, classes can just
	/// inherit this base class.
	/// </remarks>
	public abstract class Visitable : IVisitable
	{
		/// <summary>
		/// See Visitor design pattern.
		/// </summary>
		/// <param name="visitor">The visitor to use for the operation.</param>
		public virtual void Accept(IVisitor visitor) 
		{
			visitor.Visit(this);
		}
	}
}
