using System;
using System.Xml;

namespace NMatrix.Dom.Core
{
	public class DOMAttribute : XmlAttribute
	{
		public DOMAttribute(string name) : base (null, null)
		{
		}

		public override XmlElement OwnerElement
		{
			get { return base.OwnerElement; }
		}
	}
}
