using System;
using System.Xml;
using NMatrix.W3C.DOM.Core;

namespace NMatrix.Dom.Core
{
	public class Attr : XmlAttribute, IAttr
	{
		// Introduced in DOM Level 2:
		//TODO : implement OwnerElement
		IElement OwnerElement 
		{ 
			get { return null; } 
		}
	}
}
