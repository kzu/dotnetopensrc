//===============================================================================
// NMatrix XGoF Generator XDL PlugIn.
// http://www.deverest.com.ar/XGoF
//
// DataSetOrganizer.cs
// The visitor that organizes types inside the CompileUnit. It puts each DataRow
// and DataTable inside the dataset type declaration.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.CodeDom;
using NMatrix.XGoF.XSD;

namespace NMatrix.XGoF.PlugIn.XDL.DataSets
{
	/// <summary>
	/// Organizes classes in the generated dataset.
	/// </summary>
	public class DataSetOrganizer : BaseVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DataSetOrganizer()
		{
		}

		/// <summary>
		/// Visitor implementation.
		/// </summary>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			if (IsDataSet)
			{
				// Add subtypes.
				foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
					if (type.Name != CurrentDataSetType.Name &&
						type.Name != CurrentDataSetType.Name + Configuration.TypeNaming)
						CurrentDataSetType.Members.Add(type);

				// Remove types from the namespace.
				CodeTypeDeclaration[] types = new CodeTypeDeclaration[CurrentNamespace.Types.Count];
				CurrentNamespace.Types.CopyTo(types, 0);

				foreach (CodeTypeDeclaration type in types)
					if (type.Name != CurrentDataSetType.Name)
						CurrentNamespace.Types.Remove(type);
			}
		}
	
		/// <summary>
		/// Visitor implementation.
		/// </summary>
		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Organizing classes in the DataSet ...");
			base.Visit(schema);
		}
	}
}
