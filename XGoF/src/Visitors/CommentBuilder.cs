//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// CommentBuilder.cs
// Adds empty Xml documentation comments to all public types and members that 
// don't already have one, to avoid compiler warnings.
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
	/// Adds empty Xml documentation comments to all public types and members that 
	/// don't already have one, to avoid compiler warnings.
	/// </summary>
	internal class CommentBuilder : BaseCodeVisitor
	{
		public CommentBuilder()
		{
		}

		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Creating Xml documentation comments ...");
			base.Visit(schema);

			
		}
	}
}