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
	public class LogFormatter : BaseFormatter
	{
		/// <summary />
		public LogFormatter()
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
           
			// Finally remove any non-name schematron tag in the message.
            string res = TagExpressions.AllSchematron.Replace(sb.ToString(), String.Empty);
			sb = new StringBuilder();
			if (source is Assert)
			{
				sb.Append("\tAssert fails: ");
			}
			else
			{
				sb.Append("\tReport: ");
			}
			sb.Append(res);

			//Accumulate namespaces found during traversal of node for its position.
			Hashtable ns = new Hashtable();

            sb.Append("\r\n\tAt: ").Append(FormattingUtils.GetFullNodePosition(context.Clone(), String.Empty, source, ref ns));
			sb.Append(FormattingUtils.GetNodeSummary(context, ns, "\r\n\t    "));

			res = FormattingUtils.GetPositionInFile(context, "\r\n\t    ");
			if (res != String.Empty) sb.Append(res);

			res = FormattingUtils.GetNamespaceSummary(context, ns, "\r\n\t    ");
			if (res != string.Empty) sb.Append(res);

			return sb.ToString();
		}

		/// <summary />
		public override string Format(Pattern source, string messages, XPathNavigator context)
		{
			if (messages == String.Empty)
				return messages;
         
			StringBuilder sb = new StringBuilder();
			sb.Append("    From pattern \"").Append(source.Name).Append("\"\r\n");
			sb.Append(messages).Append("\r\n");

			return sb.ToString();
		}

		/// <summary />
		public override string Format(Schema source, string messages, XPathNavigator context)
		{
			if (messages == String.Empty)
				return messages;
			
			StringBuilder sb = new StringBuilder();
		
			if (source.Title != String.Empty)
				sb.Append(source.Title).Append("\r\n");
            else
                sb.Append("Results from Schematron validation\r\n");

			sb.Append(messages).Append("\r\n");

			return sb.ToString();
		}

		/// <summary />
		public override string Format(XmlValidatingReader reader, string messages)
		{
			if (messages == String.Empty)
				return messages;

			return "Results from XML " + 
				reader.ValidationType.ToString() +
				" validation:\r\n" + messages + "\r\n";
		}

		/// <summary />
		public override string Format(ValidationEventArgs source)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("  Error: ");
			sb.Append(FormattingUtils.XmlErrorPosition.Replace(source.Message, String.Empty));
			sb.Append("\r\n  At: (Line: ").Append(source.Exception.LineNumber);
			sb.Append(", Column: ").Append(source.Exception.LinePosition).Append(")\r\n");

			return sb.ToString();
		}
	}
}
