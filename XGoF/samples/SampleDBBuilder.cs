using System;
using System.Data.SqlClient;
using NMatrix.Core.Patterns;
using NMatrix.XGoF.XSD;
using NMatrix.XGoF.Host;

namespace NMatrix.XGoF.Samples
{
	/// <summary>
	/// Sample builder building database elements based on the source schema.
	/// </summary>
	public class SampleDBBuilder : HostedVisitor
	{
		public SampleDBBuilder()
		{
		}

		/// <summary>
		/// Create a database with the name of schema target namespace.
		/// </summary>
		public void Visit(VisitableSchemaRoot element)
		{
			//Can create a database for the schema
		}


		public void Visit(VisitableElementComplexType element)
		{
			//Can create a table for the type
		}
	}
}
