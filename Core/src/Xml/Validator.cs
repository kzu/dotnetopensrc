//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// Validator.cs
// A class for accumulating validation errors for Xml reading.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace NMatrix.Core.Xml
{
	/// <summary>
	/// A class for accumulating validation errors for Xml reading.
	/// </summary>
	/// <remarks>Can be used when reading an <c>XmlSchema</c> or in junction with the <c>XmlValidatingReader</c> class.</remarks>
	/// <example>
	/// Validator validator = new Validator();<br />
	///	validator.Validate(@"C:\data.xml", @"C:\data.xsd");<br /><br />
	///	
	///	//Using only the OnValidation handler:<br />
	///	Validator validator = new Validator();<br />
	/// using (FileStream fs = new FileStream(@"C:\data.xsd", FileMode.Open))<br />
	///	schema = XmlSchema.Read(fs, new ValidationEventHandler(validator.OnValidation));<br />
	///	if (validator.HasErrors)<br />
	///		throw new InvalidOperationException("Schema is not valid!");
	/// </example>
	public class Validator
	{
		/// <summary>
		/// Accumulates errors as they happen.
		/// </summary>
		private StringWriter _errorswriter = new StringWriter();
		/// <summary>
		/// Flag signaling errors.
		/// </summary>
		private bool _errorfound = false;

		/// <summary>
		/// Public constructor doesn't take any parameters.
		/// </summary>
		public Validator()
		{
		}

		/// <summary>
		/// Initiates validation using the specified parameters. If there are errors
		/// in the document an <c>ArgumentException</c> is thrown with the message containing
		/// the accumulation of errors.
		/// </summary>
		/// <exception cref="ArgumentException"/>
		public void Validate(string documentFile, XmlSchema schema)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(documentFile);
			Validate(doc, schema);
		}

		/// <summary>
		/// Initiates validation using the specified parameters. If there are errors
		/// in the document an <c>ArgumentException</c> is thrown with the message containing
		/// the accumulation of errors.
		/// </summary>
		/// <exception cref="ArgumentException"/>
		public void Validate(string documentFile, string schemaFile)
		{
			XmlSchema schema;
			using (FileStream fs = new FileStream(schemaFile, FileMode.Open))
				schema = XmlSchema.Read(fs, new ValidationEventHandler(OnValidation));
			if (HasErrors)
				throw new ArgumentException("Schema is not valid." 
					+ Environment.NewLine + schema.SourceUri
					+ Environment.NewLine + Errors);
			
			XmlDocument doc = new XmlDocument();
			doc.Load(documentFile);
			Validate(doc, schema);			
		}

		/// <summary>
		/// Initiates validation using the specified parameters. If there are errors
		/// in the document an <c>ArgumentException</c> is thrown with the message containing
		/// the accumulation of errors.
		/// </summary>
		/// <exception cref="ArgumentException"/>
		public void Validate(XmlDocument document, string schemaFile)
		{
			XmlSchema schema;
			using (FileStream fs = new FileStream(schemaFile, FileMode.Open))
				schema = XmlSchema.Read(fs, new ValidationEventHandler(OnValidation));
			if (HasErrors)
				throw new ArgumentException("Schema is not valid." 
					+ Environment.NewLine + schema.SourceUri
					+ Environment.NewLine + Errors);
			
			_errorswriter = new StringWriter();
			Validate(document, schema);			
		}

		/// <summary>
		/// Initiates validation using the specified parameters. If there are errors
		/// in the document an <c>ArgumentException</c> is thrown with the message containing
		/// the accumulation of errors.
		/// </summary>
		/// <exception cref="ArgumentException"/>
		public void Validate(XmlDocument document, XmlSchema schema)
		{
			_errorfound = false;
			_errorswriter = new StringWriter();
			MemoryStream mem = new MemoryStream();
			XmlTextWriter w = new XmlTextWriter(mem, Encoding.Default);
			document.WriteContentTo(w);
			w.Flush();
            mem.Seek(0, SeekOrigin.Begin);
			XmlTextReader xr = new XmlTextReader(mem);

			XmlValidatingReader reader = new XmlValidatingReader(xr);
            reader.Schemas.Add(schema);
			reader.ValidationEventHandler += new ValidationEventHandler(OnValidation);
			while (reader.Read()) {}
			
			if (HasErrors) 
				throw new ArgumentException("Document is not valid."
					+ Environment.NewLine + document.BaseURI
					+ Environment.NewLine + Errors);
		}

		/// <summary>
		/// Flag signaling whether errors were found.
		/// </summary>
		public bool HasErrors
		{
			get { return _errorfound; }
		}

		/// <summary>
		/// String representation of the accumulation of errors.
		/// </summary>
		public string Errors
		{
			get { return _errorswriter.ToString(); }
		}

		/// <summary>
		/// <c>ValidationEventHandler</c> method implementation, made public to 
		/// allow use from outside the class.
		/// </summary>
		public void OnValidation(object sender, ValidationEventArgs e)
		{
			_errorfound = true;
			_errorswriter.WriteLine("Validation Error: {0}", e.Message);
		}
	}
}
