using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;

namespace NMatrix.Schematron.Formatters
{
	/// <summary />
	public class SimpleFormatter : LogFormatter
	{
		/// <summary />
		public SimpleFormatter()
		{
		}

		/// <summary />
		public override string Format(Test source, XPathNavigator context)
		{
			string msg = source.Message;
			//TODO: is this Capacity initialization usefull? or it is for number of chars?
			StringBuilder sb = new StringBuilder(source.NameExpressions.Count);
			XPathExpression expr;

			// As we move on, we have to append starting from the last point,
			// skipping the <name> expression itself: Substring(offset, name.Index - offset).
			int offset = 0;
			
			for (int i = 0; i < source.NameExpressions.Count; i++)
			{
				Match name = source.NameExpressions[i];
				expr = source.NamePaths[i];

				// Append the text without the expression.
				sb.Append(msg.Substring(offset, name.Index - offset));

				// Does the name element have a path attribute?
				if (expr != null)
				{
					expr.SetContext(source.GetContext());

					string result = null;
					if (expr.ReturnType == XPathResultType.NodeSet)
					{
						// It the result of the expression is a nodeset, we only get the element 
						// name of the first node, which is compatible with XSLT implementation.
						XPathNodeIterator nodes = (XPathNodeIterator)context.Evaluate(expr);
						if (nodes.MoveNext())
							result = nodes.Current.Name;
					}
					else
						result = context.Evaluate(expr) as string;

					if (result != null)
						sb.Append(result);
				}
				else
					sb.Append(context.Name);

				offset = name.Index + name.Length;
			}

			sb.Append(msg.Substring(offset));
           
			if (source is Assert)
				sb.Insert(0, "\tAssert fails: ");
			else
				sb.Insert(0, "\tReport: ");

			Hashtable ns = new Hashtable();
            sb.Append("\r\n\tAt: " + FormattingUtils.GetFullNodePosition(context.Clone(), String.Empty, source, ref ns));

			return sb.ToString();
		}
	}
}
