//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// Visitor.cs
// Defines a base visitor that uses reflection to provide a high level of 
// extensibility and flexibility for inheriting classes, which can implement only 
// the Visit methods for the types they are interested in, leaving the rest 
// unimplemented.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Reflection;
using System.Diagnostics;

namespace NMatrix.Core.Patterns
{
	/// <summary>
	/// Abstract base class for visitors. Descendents implement
	/// only the Visit methods for the types it wants to catch and leave the 
	/// rest to this method.
	/// </summary>
	/// <remarks>The method uses reflection to call the most appropiate method it one
	/// is found. Else it does nothing. The pattern is implemented using method overloading
	/// which adds the flexibility exposed.
	/// </remarks>
	public abstract class Visitor : IVisitor
	{
		private MethodInfo _lastmethod = null;
		private object _lastvisitable = null;

		/// <summary>
		/// See Visitor design pattern.
		/// </summary>
		/// <param name="visitable"></param>
		public void Visit(object visitable) 
		{
			try
			{
				MethodInfo method = this.GetType().GetMethod(
					"Visit", 
					BindingFlags.ExactBinding | 
					BindingFlags.Public | 
					BindingFlags.Instance, 
					null, 
					new Type[] { visitable.GetType() },
					new ParameterModifier[0]);
				if (method != null) 
				{
					// Avoid StackOverflow exceptions by executing only if 
					// the method or visitable object are different 
					// from the last parameters used.
					if (method != _lastmethod || visitable != _lastvisitable)
					{
						_lastmethod = method;
						_lastvisitable = visitable;
						method.Invoke(this, new object[] { visitable });
					}
				}
			}
			catch (Exception ex)
			{
				//Try  to throw the most specific exception instead of the
				//generic reflection exception for the invocation.
				if (ex.InnerException != null)
					throw ex.InnerException;
				throw ex;
			}
		}
	}
}
