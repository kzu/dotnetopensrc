using System;
using System.Collections;
using System.Data;
using System.Management;

namespace NMatrix.WmiDataProvider
{
	/// <summary>
	/// Provides a reader to access the results of a WMI query.
	/// </summary>
	public sealed class WmiDataReader: IDataReader
	{
		#region Ctor & vars
		/// <summary>
		/// The connection associated with the reader.
		/// </summary>
		WmiConnection _connection;

		/// <summary>
		/// Maps column names to indexes.
		/// </summary>
		ArrayList _map = new ArrayList();

		/// <summary>
		/// The enumerator being used.
		/// </summary>
		IEnumerator _enumerator = null; 

		/// <summary>
		/// Indicates that an enumerator was asked for from outside the reader, for the first time.
		/// </summary>
		bool _isfirst = false;

		int _records = 0;

		/// <summary>
		/// Initializes a new instance of a WmiDataReader.
		/// </summary>
		internal WmiDataReader()
		{
		}

		/// <summary>
		/// Initializes a new instance of a WmiDataReader with the specified connection.
		/// </summary>
		internal WmiDataReader(WmiConnection connection)
		{
			_connection = connection;
		}

		#endregion

		#region Internal members

		/// <summary>
		/// The internal results from a query execution.
		/// </summary>
		internal ManagementObjectCollection Data
		{
			set
			{ 
				_data = value; 
				BuildMap();
			}
		} ManagementObjectCollection _data = null;

		public ManagementObject CurrentObject
		{
			get 
			{
				CheckState();
				return _enumerator.Current as ManagementObject;
			}
		}

		internal ManagementObject GetOne()
		{
			if (_enumerator == null)
			{
				_enumerator = _data.GetEnumerator();
				_enumerator.MoveNext();
				_isfirst = true;
			}

			try
			{
				if (_isfirst) _records++;
				return _enumerator.Current as ManagementObject;
			}
			catch (IndexOutOfRangeException)
			{
				if (_isfirst) _records--;
				return null;
			}
		}

		/// <summary>
		/// Builds a map of properies->indexes.
		/// </summary>
		private void BuildMap()
		{
			ManagementObject one = GetOne();
			if (one == null) return;

			foreach (PropertyData data in one.Properties)
				_map.Add(data.Name);
		}

		/// <summary>
		/// Checks that the enumerator is in a valid state. 
		/// </summary>
		/// <exception cref="InvalidOperationException">The method is 
		/// called and the enumerator is null or passed the end of the enumeration.</exception>
		private void CheckState()
		{
			if (_enumerator == null)
				throw new InvalidOperationException("Must first call read.");
			if (_enumerator.Current == null)
				throw new InvalidOperationException("There is no data in the current position.");
		}

		#endregion

		#region Implementation of IDataReader
		/// <summary>
		/// Not implemented. Always returns false.
		/// </summary>
		public bool NextResult()
		{
			return false;
		}

		/// <summary>
		/// Closes the reader. 
		/// </summary>
		public void Close()
		{
			_connection = null;
		}

		public bool Read()
		{
			if (_enumerator == null)
				_enumerator = _data.GetEnumerator();
			//Special case for enumerator already read by GetOne()
			if (_isfirst)
			{
				_isfirst = false;
				try
				{
					return _enumerator.Current != null;
				}
				catch (IndexOutOfRangeException)
				{
					return false;
				}
			}
			bool hasnext = _enumerator.MoveNext();
			if (hasnext) _records++;
			return hasnext;
		}

		/// <summary>
		/// Returns a DataTable that describes the column metadata of the IDataReader.
		/// </summary>
		/// <returns>The table with full details of the structure.</returns>
		public DataTable GetSchemaTable()
		{
			DataTable dt = new DataTable("Schema");
			dt.Columns.Add("ColumnName", typeof(string));
			dt.Columns.Add("DataType", typeof(Type));

			// Iterate through the list of properties
			ManagementObject mo = GetOne();
			if (mo == null)
				return dt;

			foreach (PropertyData prop in mo.Properties)
				dt.LoadDataRow(new object[] { prop.Name, prop.Value.GetType() }, true);

			return dt;
		}

		/// <summary>
		/// Returns the number of records affected by the last WMI query.
		/// </summary>
		public int RecordsAffected
		{
			get { return _records; }
		}

		/// <summary>
		/// Indicates whether the reader is closed. Always returns false.
		/// </summary>
		public bool IsClosed
		{
			get { return _enumerator == null || _records == 0; }
		}

		/// <summary>
		/// Not implemented. Always returns zero.
		/// </summary>
		public int Depth
		{
			get { return 0; }
		}

		#endregion

		#region Implementation of IDisposable
		/// <summary>
		/// Releases resources used by the reader.
		/// </summary>
		public void Dispose()
		{
			_connection = null;
		}
		#endregion

		#region Implementation of IDataRecord
		/// <summary>
		/// Provides access to the object value.
		/// </summary>
		/// <param name="i">The column index.</param>
		/// <returns>The value.</returns>
		public object GetValue(int i)
		{
			CheckState();

			if (i < 0 || i > ((ManagementObject)_enumerator.Current).Properties.Count)
				throw new IndexOutOfRangeException("Invalid column index.");

			return ((ManagementObject)_enumerator.Current).Properties[(string)_map[i]].Value;
		}

		public int GetInt32(int i)
		{
			return (int) GetValue(i);
		}

		public bool IsDBNull(int i)
		{
			return GetValue(i) == null;
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="NotSupportedException" />
		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotSupportedException();
		}

		public byte GetByte(int i)
		{
			return (byte) GetValue(i);
		}

		public System.Type GetFieldType(int i)
		{
			return GetValue(i).GetType();
		}

		public decimal GetDecimal(int i)
		{
			return (decimal) GetValue(i);
		}

		/// <summary>
		/// Retrieves all values in a single array of objects.
		/// </summary>
		/// <param name="values">An array of Object to copy the fields into.</param>
		/// <returns>The number of instances of Object in the array.</returns>
		public int GetValues(object[] values)
		{
			int idx = 0;

			foreach (PropertyData data in CurrentObject.Properties)
			{
				values[idx] = data.Value;
				idx++;
				if (idx > values.Length + 1) break;
			}
			
			return values.Length;
		}

		public string GetName(int i)
		{
			return (string) _map[i];
		}

		public long GetInt64(int i)
		{
			return (long) GetValue(i);
		}

		public double GetDouble(int i)
		{
			return (double) GetValue(i);
		}

		public bool GetBoolean(int i)
		{
			return (bool) GetValue(i);
		}

		public Guid GetGuid(int i)
		{
			return (Guid) GetValue(i);
		}

		public DateTime GetDateTime(int i)
		{
			return (DateTime) GetValue(i);
		}

		public int GetOrdinal(string name)
		{
			return _map.IndexOf(name);
		}

		public string GetDataTypeName(int i)
		{
			return GetFieldType(i).Name;
		}

		public float GetFloat(int i)
		{
			return (float) GetValue(i);
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="NotSupportedException" />
		public IDataReader GetData(int i)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="NotSupportedException" />
		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotSupportedException();
		}

		public string GetString(int i)
		{
			return GetValue(i).ToString();
		}

		public char GetChar(int i)
		{
			return (char) GetValue(i);
		}

		public short GetInt16(int i)
		{
			return (short) GetValue(i);
		}

		/// <summary>
		/// Gets the column value.
		/// </summary>
		public object this[string name]
		{
			get { return CurrentObject.Properties[name].Value; }
		}

		/// <summary>
		/// Gets the column value.
		/// </summary>
		public object this[int i]
		{
			get { return GetValue(i); }
		}

		/// <summary>
		/// The number of fields in the current object.
		/// </summary>
		public int FieldCount
		{
			get { return _map.Count; }
		}

		#endregion
	}
}
