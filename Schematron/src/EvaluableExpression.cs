using System;
using System.Xml;
using System.Xml.XPath;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Base class for elements that can be evaluated by an XPath expression.
	/// </summary>
	/// <remarks>
	/// This class performs the expression compilation, and provides
	/// access to the context through two methods. 
	/// </remarks>
	/// <author ref="dcazzulino" />
	/// <progress amount="100" />
	public abstract class EvaluableExpression
	{
		string _xpath;
        XPathExpression _expr;
		XmlNamespaceManager _ns;

		/// <summary>
		/// Cache the return type to avoid cloning the expression.
		/// </summary>
		XPathResultType _ret;

		/// <summary>Initializes a new instance of the element with the expression specified.</summary>
		/// <param name="xpathExpression">The expression to evaluate.</param>
		internal protected EvaluableExpression(string xpathExpression)
		{
			InitializeExpression(xpathExpression);
		}

		/// <summary>Initializes a new instance of the element.</summary>
		internal protected EvaluableExpression()
		{
		}

		/// <summary>Reinitializes the element with a new expression, 
		/// after the class has already been constructed</summary>
		/// <param name="xpathExpression">The expression to evaluate.</param>
		protected void InitializeExpression(string xpathExpression)
		{
			_xpath = xpathExpression;
			_expr = Config.DefaultNavigator.Compile(xpathExpression);
			_ret = _expr.ReturnType;
			if (_ns != null) _expr.SetContext(_ns);
		}

		/// <summary>Contains the compiled version of the expression.</summary>
		/// <remarks>
		/// A clone of the expression is always returned, because the compiled
		/// expression is not thread-safe for evaluation.
		/// </remarks>
		public XPathExpression CompiledExpression
		{
			get 
			{ 
				if (_expr != null) return _expr.Clone(); 
				else return null;
			}
		}

		/// <summary>Contains the string version of the expression.</summary>
		public string Expression
		{
			get { return _xpath; }
		}

		/// <summary>Contains the string version of the expression.</summary>
		public XPathResultType ReturnType
		{
			get { return _ret; }
		}

		/// <summary>Returns the manager in use to resolve expression namespaces.</summary>
		public XmlNamespaceManager GetContext()
		{
			return _ns;
		}

		/// <summary>Sets the manager to use to resolve expression namespaces.</summary>
		public void SetContext(XmlNamespaceManager nsManager)
		{
			if (_expr != null) _expr.SetContext(nsManager);
			_ns = nsManager;
		}
	}
}
