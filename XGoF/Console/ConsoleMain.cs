using System;
using NMatrix.XGoF;
using NMatrix.Core;
using NMatrix.Core.Utility;
using System.IO;

namespace NMatrix.XGoF.Console
{
	/// <summary>
	/// Console interface to the DEVerest XGoF Generator
	/// </summary>
	class ConsoleMain
	{
		[STAThread]
		static void Main(string[] args)
		{
			//using(DumpObject util = new DumpObject("AppDomain.CurrentDomain DUMP", 2))
			//	util.Dump(AppDomain.CurrentDomain);

			foreach (string file in args)
			{
				try
				{
					Runner run = new Runner(file);
					run.Progress += new ProgressEventHandler(OnProgress);
					run.Start();
				}
				catch (Exception ex)
				{
					System.Console.Write(ex);
					if (ex.InnerException != null)
						System.Console.Write(ex.InnerException);
				}
				finally
				{
					System.Console.Read();
				}
			}
		}

		
		/// <summary>
		/// Output notifications received from the generator.
		/// </summary>
		private static void OnProgress(object sender, ProgressEventArgs e)
		{
			System.Console.WriteLine(e.Message);
			StringWriter report = new StringWriter();
			report.ToString();
		}
	}
}
