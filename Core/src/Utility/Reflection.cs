//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// Reflection.cs
// Provides general utilities related to reflection and types.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;

namespace NMatrix.Core.Utility
{
	/// <summary>
	/// Provides global utility methods.
	/// </summary>
	public class Reflection
	{
		private Reflection()
		{
		}

		/// <summary>
		/// Returns an object instance from the string passed.
		/// </summary>
		/// <param name="typeName">The type name to instantiate, fully qualified.</param>
		/// <returns>The instantiated object, or null.</returns>
		public static object GetObject(string typeName)
		{
			Type type = Type.GetType(typeName);
			if (type == null) return null;
			return Activator.CreateInstance(type);
		}

		/// <summary>
		/// Checks that the specified type implements an specific interface.
		/// </summary>
		/// <param name="typeToCheck">The type to check.</param>
		/// <param name="typeToEnsure">The interface to look for.</param>
		public static void EnsureInterface(Type typeToCheck, Type typeToEnsure)
		{
			if (typeToCheck.FindInterfaces(System.Reflection.Module.FilterTypeName, typeToEnsure.Name).Length == 0)
				throw new ArgumentException(String.Format("Type to create must implement {0}.", typeToEnsure.Name));
		}

		/// <summary>
		/// Checks that the specified type is a subtype of another.
		/// </summary>
		/// <param name="typeToCheck">The type to check.</param>
		/// <param name="typeToEnsure">The base type to check.</param>
		public static void EnsureBaseType(Type typeToCheck, Type typeToEnsure)
		{
			if (!typeToCheck.IsSubclassOf(typeToEnsure) && typeToCheck != typeToEnsure)
				throw new ArgumentException(String.Format("Type to create must be a {0} descendent.", typeToEnsure.Name));
		}
	}
}
