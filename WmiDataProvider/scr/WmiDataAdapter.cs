using System;
using System.Data;
using System.Management;
using System.Text.RegularExpressions;

namespace NMatrix.WmiDataProvider
{
	/// <summary>
	/// Represents a set of data commands and a WMI connection that are used to fill 
	/// the DataSet and update a WMI Server.
	/// </summary>
	public sealed class WmiDataAdapter : IDbDataAdapter
	{
		#region Internal members

		static Regex _commandtable = new Regex(@"FROM\s(?<table>[^\s]*)\s?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private void LoadSchema(DataSet ds, WmiCommand command, WmiDataReader reader)
		{
			//Match the query "table".
			string name = _commandtable.Match(command.CommandText).Groups["table"].Value;
			if (ds.Tables.Contains(name)) ds.Tables.Remove(name);

			DataTable table = new DataTable(name);
			//Use the first object to build the table structure.
			ManagementObject one = reader.GetOne();
			if (one != null)
			{
				foreach (PropertyData prop in one.Properties)
					table.Columns.Add(prop.Name, WmiConvert.WmiToClr(prop.Type));
			}
			//Add dummy column if no data is found. For consistency with other ADO.NET providers which always load the schema. 
			if (table.Columns.Count == 0)
				table.Columns.Add("Object", typeof(string));
			ds.Tables.Add(table);
		}

		#endregion

		#region Ctors
		/// <summary>
		/// Initializes a new instance of the adapter.
		/// </summary>
		public WmiDataAdapter()
		{
		}

		/// <summary>
		/// Creates an instance of the adapter with the values specified.
		/// </summary>
		public WmiDataAdapter(WmiCommand selectCommand)
		{
			_select = selectCommand;
		}

		/// <summary>
		/// Creates an instance of the adapter with the values specified.
		/// </summary>
		public WmiDataAdapter(WmiCommand selectCommand, WmiConnection connection)
		{
			_select = selectCommand;
			_select.Connection = connection;
		}

		/// <summary>
		/// Creates an instance of the adapter with the values specified.
		/// </summary>
		public WmiDataAdapter(string selectCommand, string connectionString)
		{
			_select = new WmiCommand(selectCommand, new WmiConnection(connectionString));
		}
		#endregion

		#region Implementation of IDbDataAdapter
		
		/// <summary>
		/// Not implemented. Always returns null.
		/// </summary>
		public IDbCommand UpdateCommand
		{
			get { return null; }
			set { }
		}

		/// <summary>
		/// Not implemented. Always returns null.
		/// </summary>
		public IDbCommand DeleteCommand
		{
			get { return null; }
			set { }
		}

		/// <summary>
		/// Not implemented. Always returns null.
		/// </summary>
		public IDbCommand InsertCommand
		{
			get { return null; }
			set { }
		}

		/// <summary>
		/// Gets or sets a command used to select records in the data source.
		/// </summary>
		public IDbCommand SelectCommand
		{
			get
			{
				return _select;
			}
			set
			{
				if (!(value is WmiCommand))
					throw new ArgumentException("A WMI command must be used.");
				_select = (WmiCommand) value;
			}
		} WmiCommand _select;

		#endregion

		#region Implementation of IDataAdapter
		/// <summary>
		/// Adds rows in the DataSet to match those in the data source.
		/// </summary>
		/// <param name="dataSet">A DataSet to fill with records and, if necessary, schema.</param>
		/// <returns>The number of rows successfully added to the DataSet.</returns>
		public int Fill(DataSet dataSet)
		{
			if (_select == null || _select.CommandText == String.Empty)
				throw new InvalidOperationException("Select command doesn't contain a valid query to execute.");

			//Open connection if needed.
			bool doclose = false;
			if(_select.Connection.State != ConnectionState.Open)
			{
				_select.Connection.Open();
				doclose = true;
			}

			WmiDataReader dr = (WmiDataReader) _select.ExecuteReader();
			LoadSchema(dataSet, _select, dr);
			DataTable tb = dataSet.Tables[_commandtable.Match(_select.CommandText).Groups["table"].Value];
			if (tb != null)
			{
				tb.BeginLoadData();
				while(dr.Read())
				{
					object[] values = new object[dr.FieldCount];
					dr.GetValues(values);
					tb.LoadDataRow(values, true);
				}
			}

			//Close connection if appropriate.
			if (doclose) 
				_select.Connection.Close();

			return dr.RecordsAffected;
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <returns></returns>
		public IDataParameter[] GetFillParameters()
		{
			throw new NotSupportedException();
		}

		public DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
		{
			LoadSchema(dataSet, _select, _select.ExecuteReader() as WmiDataReader);
			DataTable[] tables = new DataTable[dataSet.Tables.Count];
			dataSet.Tables.CopyTo(tables, 0);
			return tables;
		}

		/// <summary>
		/// Not implemented. Always returns zero.
		/// </summary>
		public int Update(System.Data.DataSet dataSet)
		{
			return 0;
		}

		/// <summary>
		/// Not implemented. Always returns null.
		/// </summary>
		public ITableMappingCollection TableMappings
		{
			get { return null; }
		}

		/// <summary>
		/// Not implemented. Always returns MissingSchemaAction.Ignore.
		/// </summary>
		public MissingSchemaAction MissingSchemaAction
		{
			get { return MissingSchemaAction.Ignore; }
			set { }
		}

		/// <summary>
		/// Not implemented. Always returns MissingMappingAction.Passthrough.
		/// </summary>
		public System.Data.MissingMappingAction MissingMappingAction
		{
			get { return MissingMappingAction.Passthrough; }
			set { }
		}
		#endregion
	}
}
