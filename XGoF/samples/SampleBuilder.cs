using System;
using System.CodeDom;
using NMatrix.Core.Patterns;
using NMatrix.XGoF.XSD;
using NMatrix.XGoF.Host;

namespace NMatrix.XGoF.Samples
{
	/// <summary>
	/// Sample builder.
	/// </summary>
	public class SampleBuilder : HostedVisitor
	{
		CodeNamespace _currentns;

		public SampleBuilder()
		{
		}

		public void Visit(VisitableSchemaRoot element)
		{
			_currentns = 
				new CodeNamespace(element.SchemaObject.TargetNamespace);

			//Get the current state of the generation process.
			ICurrentState state = (ICurrentState) 
				Host.GetService(typeof(ICurrentState));
				
			state.Unit.Namespaces.Add(_currentns);
		}

		public void Visit(VisitableElementComplexType element)
		{
			//Create a class declaration and add it to the namespace.
			CodeTypeDeclaration type = 
				new CodeTypeDeclaration(element.TypeName);
			_currentns.Types.Add(type);
		}
	}
}
