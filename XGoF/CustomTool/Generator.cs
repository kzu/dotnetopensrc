using System;
//Located in: Common7\IDE\Microsoft.VSDesigner.dll
using Microsoft.VSDesigner.CodeGenerator; 
using Microsoft.Win32;
using System.Runtime.InteropServices; 
using System.IO;
using System.Reflection;
using System.Security.Policy;
using NMatrix.XGoF;
using NMatrix.Core;

namespace NMatrix.XGoF.CustomTool
{
	[Guid("6C65A337-FA2F-400f-8C53-589E52CE4F59")]
    public class Generator : BaseCodeGeneratorWithSite
    {
		// The same GUID as the one in the GuidAttribute of this class.
		static string _gen = "{6C65A337-FA2F-400f-8C53-589E52CE4F59}";
		static Guid _cs = new Guid("{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}");
		static Guid _vb = new Guid("{164B10B9-B200-11D0-8C61-00A0C91E29D5}");
		
		static string _cskey = 
			@"SOFTWARE\Microsoft\VisualStudio\7.0\Generators\{" + _cs.ToString() + @"}\XGoF";
		static string _vbkey = 
			@"SOFTWARE\Microsoft\VisualStudio\7.0\Generators\{" + _vb.ToString() + @"}\XGoF";

		string _output = String.Empty;
		
		public override byte[] GenerateCode(string fileName, string fileContents)
        {
            _output = String.Empty;

			/*
			// Create the AppDomain
			Evidence evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			setup.PrivateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
			setup.PrivateBinPathProbe = AppDomain.CurrentDomain.SetupInformation.PrivateBinPathProbe;

			AppDomain gen = AppDomain.CreateDomain("Generator", null, AppDomain.CurrentDomain.SetupInformation);
			gen.SetShadowCopyFiles();
			//gen.SetupInformation.ApplicationBase = setup.ApplicationBase;
			*/
	
			try
			{
				//Runner r = (Runner)gen.CreateInstanceFromAndUnwrap("NMatrix.XGoF", "NMatrix.XGoF.Runner");
				//r.File = fileName;
				Runner r = new Runner(fileName);
				r.FileFinished += new ProgressEventHandler(OnFileFinished);
				r.Start();
			}
			catch (Exception e)
			{
				base.GeneratorErrorCallback(false, 0, e.ToString(), 0, 0);
				_output = "/* Couldn't generate output!\n" + e.ToString() + "\n*/";
			}
			finally
			{
				//AppDomain.Unload(gen);
			}

            return System.Text.Encoding.ASCII.GetBytes(_output);
        }

		private void OnFileFinished(object sender, ProgressEventArgs e)
		{
			try
			{
				//Read output file and return as a byte array.
				using (FileStream fs = new System.IO.FileStream(e.Message, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					_output += new StreamReader(fs).ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				_output += ex.ToString() + "\n";
			}
		}

		[ComRegisterFunction]
		static void Register(Type type)
        {
			// Register the appropriate keys.

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(_cskey))
            {
                key.SetValue("", "NMatrix XGoF");
                key.SetValue("CLSID", _gen);
                key.SetValue("GeneratesDesignTimeSource", 1);
            }

			using (RegistryKey key = Registry.LocalMachine.CreateSubKey(_vbkey))
			{
				key.SetValue("", "NMatrix XGoF");
				key.SetValue("CLSID", _gen);
				key.SetValue("GeneratesDesignTimeSource", 1);
			}
		}

		[ComUnregisterFunction]
		static void UnRegister(Type type)
        {
            Registry.LocalMachine.DeleteSubKey(_cskey, false);
			Registry.LocalMachine.DeleteSubKey(_vbkey, false);
		}
    }
}
