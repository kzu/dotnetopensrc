using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NMatrix.XGoF;
using NMatrix.Core;

namespace AddIn
{
	/// <summary>
	/// Summary description for ProgressForm.
	/// </summary>
	public class ProgressForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.LinkLabel lnkClose;
		private System.Windows.Forms.TextBox lblMsg;

		string _path;

		public ProgressForm(string path)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			_path = path;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProgressForm));
			this.lnkClose = new System.Windows.Forms.LinkLabel();
			this.lblMsg = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lnkClose
			// 
			this.lnkClose.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.lnkClose.Location = new System.Drawing.Point(488, 0);
			this.lnkClose.Name = "lnkClose";
			this.lnkClose.Size = new System.Drawing.Size(40, 16);
			this.lnkClose.TabIndex = 2;
			this.lnkClose.TabStop = true;
			this.lnkClose.Text = "Close";
			this.lnkClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClose_LinkClicked);
			// 
			// lblMsg
			// 
			this.lblMsg.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lblMsg.BackColor = System.Drawing.SystemColors.Control;
			this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblMsg.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblMsg.Location = new System.Drawing.Point(8, 24);
			this.lblMsg.Multiline = true;
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.lblMsg.Size = new System.Drawing.Size(512, 296);
			this.lblMsg.TabIndex = 3;
			this.lblMsg.Text = "Results...";
			this.lblMsg.WordWrap = false;
			// 
			// ProgressForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 328);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lblMsg,
																		  this.lnkClose});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "ProgressForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " XGoF Progress";
			this.Load += new System.EventHandler(this.ProgressForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ProgressForm_Load(object sender, System.EventArgs e)
		{
			try
			{
				Runner run = new Runner(_path);
				run.Progress += new ProgressEventHandler(OnProgress);
				run.Start();
			}
			catch (Exception ex)
			{
				lblMsg.Text += "Couldn't finish generation!" + Environment.NewLine + ex.ToString();
			}

			lnkClose.Enabled = true;
		}

		/// <summary>
		/// Output notifications received from the generator.
		/// </summary>
		private void OnProgress(object sender, ProgressEventArgs e)
		{
			lblMsg.Text += e.Message + Environment.NewLine;
		}

		private void lnkClose_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Close();
		}
	}
}
