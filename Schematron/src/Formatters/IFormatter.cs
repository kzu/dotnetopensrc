using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Runtime.Remoting.Messaging;

namespace NMatrix.Schematron.Formatters
{
	/// <summary />
	public interface IFormatter
	{
		/// <summary />
		string Format(Test source, XPathNavigator context);

		/// <summary />
		string Format(Rule source, string messages, XPathNavigator context);

		/// <summary />
		string Format(Pattern source, string messages, XPathNavigator context);

		/// <summary />
		string Format(Phase source, string messages, XPathNavigator context);

		/// <summary />
		string Format(Schema source, string messages, XPathNavigator context);
			
		/// <summary />
		string Format(XmlValidatingReader reader, string messages);
			
		/// <summary />
		string Format(ValidationEventArgs source);
	}
}
