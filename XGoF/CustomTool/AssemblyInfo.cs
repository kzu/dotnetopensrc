/*
 * Registration:
 * regasm /codebase NMatrix.XGoF.CustomTool.dll
 *
 * Unregistration:
 * regasm /unregister NMatrix.XGoF.CustomTool.dll
 * 
 * Usage:
 * Add a configuration file to the solution in the format
 * specified by XGoF. Then set:
 *  Build Action: Content
 *  Custom Tool: XGoF
 * 
*/

using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("NMatrix XGoF CustomTool")]
[assembly: AssemblyDescription("A VS .NET custom tool to automatically generate code from the IDE.")]
[assembly: AssemblyCompany("NMatrix Project")]
[assembly: AssemblyProduct("XGoF CustomTool")]

[assembly: AssemblyVersion("0.82.*")]

[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile(@"..\..\..\NMatrix.snk")]
[assembly: AssemblyKeyName("NMatrix")]

//TODO: create an assembly-level attribute for the license. Put the license text there.

/// <summary>
/// Contains assembly level documentation.
/// </summary>
/// <license>MPL1.1</license>
/// <author id="dcazzulino" name="Daniel Cazzulino" email="nmatrixdotnet@msn.com">Project starter.</author>
public class ThisAssembly 
{
}
