using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Collections;
using NMatrix.Schematron.Formatters;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Evaluates all the schema elements synchronously.
	/// </summary>
	/// <authorref id="dcazzulino" />
	/// <progress amount="100" />
	/// <remarks>
	/// See <see cref="EvaluationContextBase"/> for a description of the purpose and 
	/// of evaluation contexts, and where are they used.
	/// </remarks>
	internal class SyncEvaluationContext : EvaluationContextBase
	{
		/// <summary>Creates the evaluation context</summary>
		public SyncEvaluationContext()
		{
		}

		/// <summary>
		/// Starts the evaluation process.
		/// </summary>
		/// <remarks>
		/// When the process is finished, the results are placed 
		/// in the <see cref="EvaluationContextBase.Messages"/> property.
		/// </remarks>
		public override void Start()
		{
			Messages = String.Empty;

			// Is there something to evaluate at all?
			if (Schema.Patterns.Count == 0) return;
            
			// If no phase was received, try the default phase defined for the schema.
			// If no default phase is defined, all patterns will be tested.
			if (Phase == String.Empty)
			{
				if (Schema.DefaultPhase == String.Empty)
				{
					Phase = Schematron.Phase.All;
				}
				else
				{
					Phase = Schema.DefaultPhase;
				}
			}

			string res = Evaluate(Schema.Phases[Phase]);
			Messages = Formatter.Format(Schema, res, Source);
		}

		/// <summary>
		/// Evaluates the selected <see cref="Phase"/>.
		/// </summary>
		/// <remarks>
		///	As most of the other evaluation methods, it repositions the 
		///	<see cref="EvaluationContextBase.Source"/> navigator on the root node.
		/// </remarks>
		/// <param name="phase">The <see cref="Phase"/> to evaluate.</param>
		/// <returns>The messages accumulated by the evaluation of all the child
		/// <see cref="Pattern"/>, or <see cref="String.Empty"/> if there are no messages.</returns>
		private string Evaluate(Phase phase)
		{
			Source.MoveToRoot();
			StringBuilder sb = new StringBuilder();

			foreach (Pattern pt in phase.Patterns)
			{
				sb.Append(Evaluate(pt));
			}

			string res = sb.ToString();
			return Formatter.Format(phase, res, Source);
		}

		/// <summary>
		/// Evaluates the selected <see cref="Pattern"/>.
		/// </summary>
		/// <remarks>
		///	As most of the other evaluation methods, it repositions the 
		///	<see cref="EvaluationContextBase.Source"/> navigator on the root node.
		/// <para>
		/// Clears the <see cref="EvaluationContextBase.Matched"/> object before
		/// proceeding, as the restriction about node mathing (see <linkref id="schematron" />)
		/// applies only inside a single pattern.
		/// </para>
		/// </remarks>
		/// <param name="pattern">The <see cref="Pattern"/> to evaluate.</param>
		/// <returns>The messages accumulated by the evaluation of all the child
		/// <see cref="Rule"/>, or <see cref="String.Empty"/> if there are no messages.</returns>
		private string Evaluate(Pattern pattern)
		{
			Source.MoveToRoot();
			StringBuilder sb = new StringBuilder();

			// Reset matched nodes, as across patters, nodes can be 
			// evaluated more than once.
			Matched.Clear();

			foreach (Rule rule in pattern.Rules)
			{
				sb.Append(Evaluate(rule));
			}

			string res = sb.ToString();
			return Formatter.Format(pattern, res, Source);
		}

		/// <summary>
		/// Evaluates the selected <see cref="Rule"/>.
		/// </summary>
		/// <remarks>
		/// <see cref="Rule.Asserts"/> and <see cref="Rule.Reports"/> are iterated
		/// and each <see cref="Assert"/> and <see cref="Report"/> is executed against
		/// the context selected by the <see cref="Rule.Context"/>.
		/// <para>
		/// Nodes matched are added to the <see cref="EvaluationContextBase.Matched"/> list of 
		/// nodes to skip in the next rule, using the <see cref="IMatchedNodes.AddMatched"/> method.
		/// This object is a strategy object which implements different algorithms for matching and 
		/// saving node references, as the actual <see cref="XPathNavigator"/> implementation provides
		/// different methods for accessing the underlying source. 
		/// <para>
		/// This makes the implementation both performant and compliant with
		/// the restriction about node mathing (see <linkref id="schematron" />) in the spec.
		/// </para>
		/// <para>
		///		<seealso cref="DomMatchedNodes"/>
		///		<seealso cref="XPathMatchedNodes"/>
		///		<seealso cref="GenericMatchedNodes"/>
		/// </para>
		///	As most of the other evaluation methods, it repositions the 
		///	<see cref="EvaluationContextBase.Source"/> navigator on the root node.
		/// </para>
		/// </remarks>
		/// <param name="rule">The <see cref="Rule"/> to evaluate.</param>
		/// <returns>The messages accumulated by the evaluation of all the child
		/// <see cref="Assert"/> and <see cref="Report"/>, or <see cref="String.Empty"/> 
		/// if there are no messages to show.</returns>
		/// <exception cref="InvalidOperationException">
		/// The rule to evaluate is abstract (see <see cref="Rule.IsAbstract"/>).
		/// </exception>
		private string Evaluate(Rule rule)
		{
			if (rule.IsAbstract)
				throw new InvalidOperationException("The Rule is abstract, so it can't be evaluated.");

			StringBuilder sb = new StringBuilder();
			Source.MoveToRoot();
			XPathNodeIterator nodes = Source.Select(rule.CompiledExpression);
			ArrayList evaluables = new ArrayList(nodes.Count);

			// The navigator doesn't contain line info
			while (nodes.MoveNext())
			{
				if (!Matched.IsMatched(nodes.Current))
				{
					// Add the navigator to the list to be evaluated and to 
					// the list of pattern-level nodes matched so far.
					XPathNavigator curr = nodes.Current.Clone();
					evaluables.Add(curr);
					Matched.AddMatched(curr);
				}
			}

			foreach (Assert asr in rule.Asserts)
			{
				foreach (XPathNavigator node in evaluables)
				{
					string str = EvaluateAssert(asr, node.Clone());
					if (str != String.Empty)
					{
						sb.Append(str).Append(System.Environment.NewLine);
					}
				}
			}

			foreach (Report rpt in rule.Reports)
			{
				foreach (XPathNavigator node in evaluables)
				{
					string str = EvaluateReport(rpt, node.Clone());
					if (str != String.Empty) 
					{
						sb.Append(str).Append(System.Environment.NewLine);
					}
				}
			}

			string res = sb.ToString();
			return Formatter.Format(rule, res, Source);
		}

		/// <summary>
		/// Performs the evaluation of the <see cref="Assert"/>.
		/// </summary>
		/// <remarks>
		/// This is where the actual assert expression is evaluated. If
		/// the <see cref="EvaluableExpression.Expression"/> returns false, 
		/// a formated message is generated from the <see cref="Test.Message"/> property.
		/// </remarks>
		/// <param name="assert">The <see cref="Assert"/> to evaluate.</param>
		/// <param name="context">The context node for the execution.</param>
		/// <returns>The formatted message for a failing <see cref="Assert"/>, or 
		/// <see cref="String.Empty"/>.
		/// </returns>
		private string EvaluateAssert(Assert assert, XPathNavigator context)
		{
			object eval = context.Evaluate(assert.CompiledExpression);
			bool result = true;

			if (assert.ReturnType == XPathResultType.Boolean)
			{
				result = (bool)eval;
			}
			else if (assert.ReturnType == XPathResultType.NodeSet &&
				((XPathNodeIterator)eval).Count == 0)
			{
				result = false;
			}
			
			if (!result) return Formatter.Format(assert, context);

			return String.Empty;
		}

		/// <summary>
		/// Performs the evaluation of the <see cref="Report"/>.
		/// </summary>
		/// <remarks>
		/// This is where the actual report expression is evaluated. If
		/// the <see cref="EvaluableExpression.Expression"/> returns true, 
		/// a formated message is generated from the <see cref="Test.Message"/> property.
		/// </remarks>
		/// <param name="report">The <see cref="Report"/> to evaluate.</param>
		/// <param name="context">The context node for the execution.</param>
		/// <returns>The formatted message for a succesful <see cref="Report"/>, or 
		/// <see cref="String.Empty"/>.
		/// </returns>
		private string EvaluateReport(Report report, XPathNavigator context)
		{
			object eval = context.Evaluate(report.CompiledExpression);
			bool result = false;

			if (report.ReturnType == XPathResultType.Boolean)
			{
				result = (bool)eval;
			}
			else if (report.ReturnType == XPathResultType.NodeSet &&
				((XPathNodeIterator)eval).Count != 0)
			{
				result = true;
			}

			if (result) return Formatter.Format(report, context);

			return String.Empty;
		}
	}
}
