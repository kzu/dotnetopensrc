using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;

namespace NMatrix.Schematron.Formatters
{
	/// <summary />
	public class BooleanFormatter : BaseFormatter
	{
		/// <summary />
		public BooleanFormatter()
		{
		}

		/// <summary />
		public override string Format(Test source, XPathNavigator context)
		{
			return "-";
		}

		/// <summary />
		public override string Format(Schema source, string messages, XPathNavigator context)
		{
			if (messages != String.Empty)
				return "Validation failed!";

			return messages;
		}
	}
}
