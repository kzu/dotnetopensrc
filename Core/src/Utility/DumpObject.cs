//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// DumpObject.cs
// A utility class to dump and object to disk.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections;

namespace NMatrix.Core.Utility
{
	/// <summary>
	/// Dumps a complete object to disk.
	/// </summary>
	/// <example>
	/// using (Utility util = new Utility(2))
	///	{	util.DumpObject(context); }
	/// </example>
	public class DumpObject : IDisposable
	{
		//Current indentation
		private int _indent = 0;
		//Default max level of recursion
		private int _depth = 3;
		//Current level of recursion
		private int _current = 0;
		//Default file to send debugging information to
		private string _file = @"C:\log.txt";
	
		/// <summary>
		/// Initializes the class.
		/// </summary>
		public DumpObject()
		{
			AttachListener();
			Debug.WriteLine(new string('-', 120));
		}

		/// <summary>
		/// Initializes the class.
		/// </summary>
		/// <param name="depth">Specifies the level at which to stop recursion.</param>
		public DumpObject(int depth)
		{
			Init("Custom depth", _file, depth);
		}

		/// <summary>
		/// Initializes the class.
		/// </summary>
		/// <param name="methodName">A name to separate the section in the log.</param>
		public DumpObject(string methodName)
		{
			Init(methodName, _file, _depth);
		}

		/// <summary>
		/// Initializes the class.
		/// </summary>
		/// <param name="methodName">A name to separate the section in the log.</param>
		/// <param name="depth">Specifies the level at which to stop recursion.</param>
		public DumpObject(string methodName, int depth)
		{
			Init(methodName, _file, depth);
		}

		/// <summary>
		/// Initializes the class.
		/// </summary>
		/// <param name="methodName">A name to separate the section in the log.</param>
		/// <param name="fileName">The file to write the dump to.</param>
		/// <param name="depth">Specifies the level at which to stop recursion.</param>
		public DumpObject(string methodName, string fileName, int depth)
		{
			Init(methodName, fileName, depth);
		}

		private void Init(string methodName, string fileName, int depth)
		{
			_file = fileName;
			_depth = depth;
			AttachListener();
			Debug.WriteLine(new string('-', 120));
			Debug.WriteLine(new string('-', 10) + " " + methodName + " " + new string('-', 10));
			Debug.WriteLine(new string('-', 120));
		}

		private void AttachListener()
		{
			if (Debug.Listeners["DumpUtility"] == null)
			{
				Debug.Listeners.Add(new TextWriterTraceListener(_file, "DumpUtility"));
				Debug.AutoFlush = true;
			}
		}

		/// <summary>
		/// Dumps the specified object.
		/// </summary>
		/// <param name="objectToDump"></param>
		public void Dump(object objectToDump)
		{
			if (_current == _depth) return;
			if (objectToDump == null) 
			{
				Debug.WriteLine("Null");
				return;
			}
			Dump(objectToDump, objectToDump.GetType(), _depth);
		}

		/// <summary>
		/// Dumps the specified object.
		/// </summary>
		/// <param name="objectToDump"></param>
		/// <param name="reflectType">The type of the object. This limits the members
		/// returned as only the specified type will be queried for members.</param>
		public void Dump(object objectToDump, Type reflectType)
		{
			if (_current == _depth) return;
			if (objectToDump == null) 
			{
				Debug.WriteLine("Null");
				return;
			}
			Dump(objectToDump, reflectType, _depth);
		}

		/// <summary>
		/// Dumps the specified object.
		/// </summary>
		/// <param name="objectToDump"></param>
		/// <param name="reflectType">The type of the object. This limits the members
		/// returned as only the specified type will be queried for members.</param>
		/// <param name="recurseDepth">The current depth in the recursive process.</param>
		public void Dump(object objectToDump, Type reflectType, int recurseDepth)
		{
			_depth = recurseDepth;
			if (_current == _depth) return;

			if (objectToDump == null) 
			{
				Debug.WriteLine("Null");
				return;
			}
			_indent += 4;
			Type t = reflectType;
			Debug.Write(string.Format("{0}--> Type: {1} ", Indentation, t.FullName));
			if (t.Name.IndexOf("Hash")==-1)
				Debug.Write(string.Format("- Object.ToString(): {0}", objectToDump.ToString()));
			Debug.WriteLine(string.Empty);

			if (!IsValueType(t) && t.Name.IndexOf("Hash")==-1)
			{
				_current++;
				_indent += 4;
				
				foreach (PropertyInfo p in t.GetProperties())
				{
					if (p.Name != "SyncRoot" && p.PropertyType.Name.IndexOf("Hash")==-1)
					{
						Debug.Write(Indentation + "Property: " + p.Name);
						if (IsValueType(p.PropertyType))
						{
							if (p.GetIndexParameters().Length == 0)
							{
								try
								{
									if (p.GetValue(objectToDump, null) != null)
										Debug.Write(new string(' ', 30 - p.Name.Length) + " - Value: " + p.GetValue(objectToDump, null));
								}
								catch (Exception ex)
								{
									Debug.Write(new string(' ', 30 - p.Name.Length) + " - FAILED CALL: ");
									Debug.Write(ex.InnerException.Message);
								}
							}
							else
							{
								if (p.GetIndexParameters().Length == 1 &&
									IsNumeric(p.GetIndexParameters()[0].ParameterType))
								{
									try
									{
										for (int i = 0; true; i++)
										{
											object obj = p.GetValue(objectToDump, new object[] { i });
											if (obj != null) 
												Debug.Write(new string(' ', 30 - p.Name.Length) + " - Value: " + p.GetValue(objectToDump, null));
											else
												break;
										}
									}
									catch (Exception ex)
									{
										Debug.Write(new string(' ', 30 - p.Name.Length) + " - FAILED CALL: ");
										Debug.Write(ex.InnerException.Message);
									}
								}
								else
								{
									Debug.Write(new string(' ', 30 - p.Name.Length) + " - Property indexed with " + p.GetIndexParameters()[0].ParameterType);
								}
							}
							Debug.WriteLine(String.Empty);
						}
						else
						{
							Debug.WriteLine(String.Empty);
							if (IsIEnumerable(p.PropertyType))
							{
								try
								{
									if (p.GetValue(objectToDump, null) != null)
									{
										IEnumerator enu = ((System.Collections.IEnumerable)p.GetValue(objectToDump, null)).GetEnumerator();
										while (enu.MoveNext())
											Dump(enu.Current);
									}
								}
								catch (Exception ex)
								{
									Debug.Write(Indentation + " - FAILED CALL: ");
									Debug.Write(ex.InnerException.Message);
									Debug.WriteLine(string.Empty);
								}
							}
							else
							{
								if (p.GetIndexParameters().Length == 0)
								{
									try
									{
										Dump(p.GetValue(objectToDump, null), p.PropertyType);
									}
									catch (Exception ex)
									{
										Debug.Write(Indentation + " - FAILED CALL: ");
										Debug.Write(ex.InnerException.Message);
										Debug.WriteLine(string.Empty);
									}
								}
								else
								{
									if (p.GetIndexParameters().Length == 1 &&
										p.GetIndexParameters()[0].ParameterType == typeof(int))
									{
										try
										{
											for (int i = 0; true; i++)
											{
												object obj = p.GetValue(objectToDump, new object[] { i });
												if (obj != null) 
													Dump(obj);
												else
													break;
											}
										}
										catch (Exception ex)
										{
											Debug.Write(Indentation + " - FAILED CALL: ");
											Debug.Write(ex.InnerException.Message);
											Debug.WriteLine(string.Empty);
										}
									}
									else
									{
										Debug.WriteLine(Indentation + "Property indexed with " + p.GetIndexParameters()[0].ParameterType);
									}
								}
							}
						}
					}
				}

				_indent -= 4;
				_current--;
			}

			Debug.WriteLine(Indentation + "<--");
			_indent -= 4;
		}

		private string Indentation
		{
			get { return new string(' ', _indent); }
		}

		private bool IsIEnumerable(Type typeToCheck)
		{
			return (typeToCheck.FindInterfaces(System.Reflection.Module.FilterTypeName, "IEnumerable").Length != 0);
		}

		private bool IsValueType(Type typeToCheck)
		{
			return (typeToCheck.IsValueType || typeToCheck == typeof(string));
		}

		private bool IsNumeric(Type typeToCheck)
		{
			return (typeToCheck == typeof(int) || typeToCheck == typeof(long) ||
				typeToCheck == typeof(short) || typeToCheck == typeof(double));
		}

		/// <summary>
		/// Diposes the class, closes the file and removes the debug listeners.
		/// </summary>
		public void Dispose()
		{
			Debug.Close();
			Debug.Listeners.Remove("DumpUtility");
		}

		/// <summary>
		/// Removes the debug listeners.
		/// </summary>
		~DumpObject()
		{
			Debug.Listeners.Remove("DumpUtility");
		}
	}
}
