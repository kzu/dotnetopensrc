using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Text;

namespace NMatrix.Schematron.Formatters
{
	/// <summary />
	public abstract class BaseFormatter : IFormatter
	{
		/// <summary />
		public BaseFormatter()
		{
		}

		/// <summary />
		public virtual string Format(Test source, XPathNavigator context)
		{
			return source.Message;
		}

		/// <summary />
		public virtual string Format(Rule source, string messages, XPathNavigator context)
		{
			return messages;
		}

		/// <summary />
		public virtual string Format(Pattern source, string messages, XPathNavigator context)
		{
			return messages;
		}

		/// <summary />
		public virtual string Format(Phase source, string messages, XPathNavigator context)
		{
			return messages;
		}

		/// <summary />
		public virtual string Format(Schema source, string messages, XPathNavigator context)
		{
			return messages;
		}

		/// <summary />
		public virtual string Format(XmlValidatingReader reader, string messages)
		{
			return messages;
		}

		/// <summary />
		public virtual string Format(ValidationEventArgs source)
		{
			return source.Message;
		}
	}
}