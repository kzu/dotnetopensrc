using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using NMatrix.Schematron;

namespace WinTest
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Home : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtXml;
		private System.Windows.Forms.Button btnXmlFile;
		private System.Windows.Forms.Button btnXsdFile;
		private System.Windows.Forms.TextBox txtSchema;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private System.Windows.Forms.Button btnExecute;
		private System.Windows.Forms.TextBox txtMsg;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbOutput;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Home()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Home));
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtXml = new System.Windows.Forms.TextBox();
			this.btnXmlFile = new System.Windows.Forms.Button();
			this.btnXsdFile = new System.Windows.Forms.Button();
			this.txtSchema = new System.Windows.Forms.TextBox();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.btnExecute = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.cbOutput = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// txtMsg
			// 
			this.txtMsg.AcceptsReturn = true;
			this.txtMsg.AcceptsTab = true;
			this.txtMsg.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtMsg.Location = new System.Drawing.Point(8, 88);
			this.txtMsg.Multiline = true;
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtMsg.Size = new System.Drawing.Size(488, 272);
			this.txtMsg.TabIndex = 1;
			this.txtMsg.Text = "";
			this.txtMsg.WordWrap = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 16);
			this.label2.TabIndex = 13;
			this.label2.Text = "XML File:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 14;
			this.label1.Text = "Schema:";
			// 
			// txtXml
			// 
			this.txtXml.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtXml.Location = new System.Drawing.Point(64, 32);
			this.txtXml.Name = "txtXml";
			this.txtXml.Size = new System.Drawing.Size(336, 21);
			this.txtXml.TabIndex = 9;
			this.txtXml.Text = "..\\po-instance.xml";
			// 
			// btnXmlFile
			// 
			this.btnXmlFile.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnXmlFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXmlFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnXmlFile.Location = new System.Drawing.Point(408, 32);
			this.btnXmlFile.Name = "btnXmlFile";
			this.btnXmlFile.Size = new System.Drawing.Size(24, 20);
			this.btnXmlFile.TabIndex = 12;
			this.btnXmlFile.Text = "...";
			this.btnXmlFile.Click += new System.EventHandler(this.btnXmlFile_Click);
			// 
			// btnXsdFile
			// 
			this.btnXsdFile.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnXsdFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXsdFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnXsdFile.Location = new System.Drawing.Point(408, 8);
			this.btnXsdFile.Name = "btnXsdFile";
			this.btnXsdFile.Size = new System.Drawing.Size(24, 20);
			this.btnXsdFile.TabIndex = 11;
			this.btnXsdFile.Text = "...";
			this.btnXsdFile.Click += new System.EventHandler(this.btnXsdFile_Click);
			// 
			// txtSchema
			// 
			this.txtSchema.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtSchema.Location = new System.Drawing.Point(64, 8);
			this.txtSchema.Name = "txtSchema";
			this.txtSchema.Size = new System.Drawing.Size(336, 21);
			this.txtSchema.TabIndex = 10;
			this.txtSchema.Text = "..\\po-schema.xml";
			// 
			// btnExecute
			// 
			this.btnExecute.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnExecute.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnExecute.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnExecute.Location = new System.Drawing.Point(440, 8);
			this.btnExecute.Name = "btnExecute";
			this.btnExecute.Size = new System.Drawing.Size(60, 20);
			this.btnExecute.TabIndex = 15;
			this.btnExecute.Text = "Execute";
			this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 16;
			this.label3.Text = "Output:";
			// 
			// cbOutput
			// 
			this.cbOutput.Location = new System.Drawing.Point(64, 56);
			this.cbOutput.Name = "cbOutput";
			this.cbOutput.Size = new System.Drawing.Size(240, 21);
			this.cbOutput.TabIndex = 17;
			// 
			// Home
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(504, 365);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cbOutput,
																		  this.label3,
																		  this.btnExecute,
																		  this.label2,
																		  this.label1,
																		  this.txtXml,
																		  this.btnXmlFile,
																		  this.btnXsdFile,
																		  this.txtSchema,
																		  this.txtMsg});
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Home";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " NMatrix Schematron.NET Test";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.Home_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Home());
		}

		private void btnXsdFile_Click(object sender, System.EventArgs e)
		{
			if (dlgOpen.ShowDialog() == DialogResult.OK)
				txtSchema.Text = dlgOpen.FileName;		
		}

		private void btnXmlFile_Click(object sender, System.EventArgs e)
		{
			if (dlgOpen.ShowDialog() == DialogResult.OK)
				txtXml.Text = dlgOpen.FileName;
		}

		private void btnExecute_Click(object sender, System.EventArgs e)
		{
			OutputFormatting format = (OutputFormatting) cbOutput.SelectedItem;

			Validator val = new Validator(format);
			val.AddSchema(txtSchema.Text);
			//val.ReturnType = NavigableType.XmlDocument;

			try
			{
				XPathDocument doc = (XPathDocument) 
					val.Validate(txtXml.Text);
				// Continue processing valid document.
				txtMsg.Text = "Valid file!";
			}
			catch (ValidationException ex)
			{
				txtMsg.Text = ex.Message;
			}		
		}

		private void Home_Load(object sender, System.EventArgs e)
		{
			foreach (OutputFormatting value in Enum.GetValues(typeof(OutputFormatting)))
			{
				cbOutput.Items.Add(value);
			}

			cbOutput.SelectedItem = OutputFormatting.Default;
		}
	}
}
