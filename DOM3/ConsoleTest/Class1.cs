using System;
using System.IO;
using System.Xml;
using NMatrix.Dom.Core;

namespace ConsoleTest
{
	class Class1
	{
		[STAThread]
		public static void Main()
		{
			Sample();
			Console.Read();
		}
 
		public static void Sample()
		{
			try
			{
				XmlDocument doc = new DOMImplementation().CreateDocument();
				doc.LoadXml("<?xml version=\"1.0\"?>" +
					"<!-- Sample XML document -->" +
					"<bookstore xmlns:bk=\"urn:samples\">" +
					"  <book genre=\"novel\" publicationdate=\"1997\" " +
					"        bk:ISBN=\"1-861001-57-5\">" +
					"    <title>Pride And Prejudice</title>" +
					"    <author>" +
					"      <first-name>Jane</first-name>" +
					"      <last-name>Austen</last-name>" +
					"    </author>" +
					"    <price>24.95</price>" +
					"  </book>" +
					"  <book genre=\"novel\" publicationdate=\"1992\" " +
					"        bk:ISBN=\"1-861002-30-1\">" +
					"    <title>The Handmaid's Tale</title>" +
					"    <author>" +
					"      <first-name>Margaret</first-name>" +
					"      <last-name>Atwood</last-name>" +
					"    </author>" +
					"    <price>29.95</price>" +
					"  </book>" +
					"</bookstore>"); 
 
				XmlNode currNode = doc.DocumentElement;
 
				//create and add a new element
				string prefix=currNode.GetPrefixOfNamespace("urn:samples");
				XmlElement newElem=doc.CreateElement(prefix, "style", "urn:samples");
				newElem.InnerText="hardcover";
 
				currNode.FirstChild.AppendChild(newElem);
 
				/*
				Console.WriteLine("Display the modified XML...");
				XmlTextWriter writer = new XmlTextWriter(Console.Out);
				writer.Formatting = Formatting.Indented;
				doc.WriteTo( writer );
				writer.Flush();
				writer.Close();
				*/

				Console.WriteLine(doc.Implementation); 
			}
			catch (Exception e)
			{
				Console.WriteLine ("Exception: {0}", e.ToString());
			}
 
		}
	}
}
