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
using System.Reflection;

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
			Type type =	LoadType(typeName);
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

		/// <summary>
		/// Loads the specified type from either the GAC or a local folder.
		/// </summary>
		/// <param name="className">The partial or fully qualified name of the type to load.</param>
		/// <returns>The loaded type.</returns>
		/// <exception cref="TypeLoadException">The received <paramref name="className" /> couldn't be loaded.</exception>
		public static Type LoadType(string className) 
		{
			Type reqtype = Type.GetType(className);

			if (reqtype == null) 
			{
				// If the type is in the GAC, and the class name is incomplete
				// (i.e.: doesn't include version or keytoken), the previous 
				// line won't be able to load it. Try with a parcial search 
				// for the class in the GAC.
				//
				// TODO: there may be versioning issues. Check.
				string[] names = className.Split(',');
				if (names.Length < 2)
					throw new TypeLoadException(String.Format(
						"Couldn't load type '{0}'. Ensure it's available from the application folder or the GAC.", className));

				Assembly asm = LoadAssembly(names[1].Trim());
				reqtype = asm.GetType(names[0].Trim(), true);
			}

			return reqtype;
		}

		/// <summary>
		/// Loads the specified assembly from either the GAC or a local folder.
		/// </summary>
		/// <param name="assemblyName">The partial or full name of the assembly to load.</param>
		/// <returns>The loaded asssembly.</returns>
		/// <exception cref="TypeLoadException">The received <paramref name="assemblyName" /> couldn't be loaded.</exception>
		public static Assembly LoadAssembly(string assemblyName) 
		{
			Assembly asm = null;
			try
			{
				asm = Assembly.Load(assemblyName);
			}
			catch
			{
			}

			if (asm == null)
			{
				asm = Assembly.LoadWithPartialName(assemblyName);
				if (asm == null)
					throw new TypeLoadException(String.Format(
						"Couldn't load assembly '{0}'. Ensure it's available from the application folder or the GAC.", assemblyName));
			}

			return asm;
		}

		/// <summary>
		/// Sets the desired property value on the received object.
		/// </summary>
		/// <param name="target">The object to which the property value applies.</param>
		/// <param name="property">The name of the property to set.</param>
		/// <param name="value">The property value to set.</param>
		/// <exception cref="AmbiguousMatchException">More than one property is found with the specified name.</exception>
		/// <exception cref="ArgumentNullException">Parameter <paramref name="property" /> is a null reference.</exception>
		/// <exception cref="ArgumentException">The received <paramref name="property" /> wasn't found for the current <paramref name="target" /> object.</exception>
		/// <exception cref="InvalidCastException">The received <paramref name="value" /> does not implement <see cref="IConvertible"/> or a conversion to 
		/// the target property type couldn't be performed.</exception>
		public static void SetProperty(object target, string property, object value)
		{
			// Locate the type and retrieve the property.
			Type t = target.GetType();
			PropertyInfo prop = t.GetProperty(property);
			if (prop == null) 
				throw new ArgumentException(String.Format(
					"Couldn't locate property '{0}' on object '{1}'.", property, target));

			// Convert the value to the appropriate type and set the property.
			// This is an expensive operation (it's even culture-aware) so there may be room for perf. improvements.
			object propvalue = Convert.ChangeType(value, prop.PropertyType);
			prop.SetValue(target, propvalue, new object[0]);
		}
	}
}
