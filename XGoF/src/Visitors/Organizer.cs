//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// Organizer.cs
// The visitor that organizes types inside the CompileUnit. It puts each class
// inside its enclosing type if it is applicable.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.CodeDom;
using System.Xml;
using System.Xml.Schema;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.XSD;
using System.Collections;

namespace NMatrix.XGoF.Visitors
{
	/// <summary>
	/// Organizes types inside the CompileUnit, putting each class
	/// inside its enclosing type if applicable.
	/// </summary>
	internal class Organizer : BaseCodeVisitor
	{
		public Organizer()
		{
		}

		/// <summary>
		/// Recursivelly traverses types and its members to find a matching type declaration.
		/// </summary>
		/// <param name="type">The type to traverse.</param>
		/// <param name="match">The type name to find.</param>
		/// <returns></returns>
		private CodeTypeDeclaration RecurseMembers(CodeTypeDeclaration type, string match)
		{
			CodeTypeDeclaration result = null;
			foreach (CodeTypeMember member in type.Members)
			{
				if (member is CodeTypeDeclaration)
				{
					CodeTypeDeclaration current = member as CodeTypeDeclaration;
					if (current.Name == match) 
					{
						return current;
					}
					else 
					{
						CodeTypeDeclaration inner = RecurseMembers(current, match);
						if (inner != null) return inner;
					}
				}
			}
			return result;
		}

		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			// If the element is contained in another element, add the type as a subtype.
			if (element.Parent != null && element.Parent is VisitableElementComplexType)
			{
				string name = element.Parent.TypeName;
				XmlAttribute[] attributes = 
					((VisitableElementComplexType)element.Parent).SchemaObject.UnhandledAttributes;

				// If the IsDataSet attribute is true, don't add the TypeNaming to the type.
				if ( attributes != null )
					foreach (XmlAttribute attr in attributes)
						if (attr.LocalName == "IsDataSet" && attr.Value == "true")
							name = element.Parent.Name;

				CodeTypeDeclaration enclosing = null;
				foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
				{
					if (type.Name == name)
					{
						enclosing = type;
						break;
					}
					else 
					{
						enclosing = RecurseMembers(type, name);
						if (enclosing != null) break;
					}
				}

				// If we find the parent type...
				if (enclosing != null && CurrentType != null)
				{
					CurrentNamespace.Types.Remove(CurrentType);
					enclosing.Members.Add(CurrentType);
				}

				// If there is an enclosing type, move the corresponding collection type too.
				if (enclosing != null)
				{
					CodeTypeDeclaration collection = null;
					foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
					{
						if (type.Name == element.Name + Configuration.CollectionNaming)
						{
							collection = type;
							break;
						}
						else 
						{
							collection = RecurseMembers(type, element.Name + Configuration.CollectionNaming);
							if (collection != null) break;
						}
					}

					if (collection != null)
					{
						CurrentNamespace.Types.Remove(collection);
						enclosing.Members.Add(collection);
					}
				}
			}
			
			base.Visit(element);
		}	

		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Organizing classes ...");
			base.Visit(schema);
		}
	}
}