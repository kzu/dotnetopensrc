using System;
using System.Management;

namespace NMatrix.WmiDataProvider
{
	/// <summary />
	public class WmiConvert
	{
		private WmiConvert()
		{
		}

		/// <summary>
		/// Provides conversion. 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Type WmiToClr(CimType type)
		{
			return typeof(string);
			//TODO: complete conversion.
			switch (type)
			{
				case CimType.Boolean:
					return typeof(bool);
				case CimType.Char16:
					return typeof(string);
				case CimType.DateTime:
					return typeof(DateTime);
				case CimType.Real32:
					return typeof(float);
				case CimType.Real64:
				case CimType.UInt64:
					return typeof(decimal);
				case CimType.Reference:
				case CimType.Object:
				case CimType.String:
					return typeof(string);
				case CimType.SInt8:
					return typeof(short);
				case CimType.SInt16:
				case CimType.SInt32:
				case CimType.UInt8:
				case CimType.UInt16:
					return typeof(int);
				case CimType.SInt64:
					return typeof(long);
				default:
					return typeof(string);
			}
		}
	}
}
