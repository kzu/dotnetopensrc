using System;
using System.Data;
using System.Management;

namespace NMatrix.WmiDataProvider
{
	/// <summary>
	/// Represents a WMI query that can executed against a WMI server.
	/// </summary>
	public sealed class WmiCommand : IDbCommand
	{
		#region Ctor & vars 

		/// <summary>
		/// Initializes a new instance of the command.
		/// </summary>
		public WmiCommand()
		{
		}

		/// <summary>
		/// Initializes a new instance of the command.
		/// </summary>
		public WmiCommand(string commandText)
		{
			_query = commandText;
		}

		/// <summary>
		/// Initializes a new instance of the command.
		/// </summary>
		public WmiCommand(string commandText, WmiConnection connection)
		{
			_query = commandText;
			_connection = connection;
		}

		#endregion

		#region Public properties
		/// <summary>
		/// The WMI query.
		/// </summary>
		public string CommandText
		{
			get { return _query; }
			set { _query = value; }
		} string _query = String.Empty;

		/// <summary>
		/// Not supported. Always return zero.
		/// </summary>
		public int CommandTimeout
		{
			get { return 0; }
			set	{} 
		}

		/// <summary>
		/// The type of command. CommandType.Text is the only one supported.
		/// </summary>
		public CommandType CommandType
		{
			get { return CommandType.Text; }
			set
			{
				if (value != CommandType.Text)
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// The connection object associated with
		/// this command.
		/// </summary>
		public IDbConnection Connection
		{
			get { return _connection; }
			set
			{
				if (!(value is WmiConnection))
					throw new ArgumentException("Connection must be of type WmiConnection.");
				_connection = (WmiConnection) value;
			}
		} WmiConnection _connection = null;

		/// <summary>
		/// Not supported. Always returns null.
		/// </summary>
		public IDataParameterCollection  Parameters
		{
			get { return null; }
		}

		/// <summary>
		/// Not supported. Always returns null.
		/// </summary>
		public IDbTransaction Transaction
		{
			get { return null; }
			set { throw new NotSupportedException(); }
		}

		/// <summary>
		/// UpdateRowSource.None is the only one supported.
		/// </summary>
		public UpdateRowSource UpdatedRowSource
		{
			get { return UpdateRowSource.None; }
			set
			{
				if (value != UpdateRowSource.None)
					throw new NotSupportedException();
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Not supported.
		/// </summary>
		public void Cancel()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <returns></returns>
		public IDbDataParameter CreateParameter()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		public int ExecuteNonQuery()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Executes the WMI query and returns an IDataReader.
		/// </summary>
		/// <returns>The reader with the results.</returns>
		public IDataReader ExecuteReader()
		{
			if (_connection == null || _connection.State != ConnectionState.Open)
				throw new InvalidOperationException("The connection must be set and opened in order to execute the command.");

			if (_query == null || _query == String.Empty)
				throw new InvalidOperationException("The CommandText must be set in order to execute the command.");

			// create a reader
			WmiDataReader reader = new WmiDataReader(_connection);
			ManagementObjectSearcher os = new ManagementObjectSearcher(_connection.Scope, new ObjectQuery(_query));
			reader.Data = os.Get();
			return reader;
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		public IDataReader ExecuteReader(CommandBehavior behavior)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns the first column of the first row from the results set.  
		/// </summary>
		public object ExecuteScalar()
		{
			if (_connection == null || _connection.State != ConnectionState.Open)
				throw new InvalidOperationException("The connection must be set and opened in order to execute the command.");

			if (_query == null || _query == String.Empty)
				throw new InvalidOperationException("The CommandText must be set in order to execute the command.");

			IDataReader dr = ExecuteReader();
			dr.Read();
			return dr[0];
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		public void Prepare()
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Implementation of IDisposable
		/// <summary>
		/// Releases resources used by the command.
		/// </summary>
		public void Dispose()
		{
			_connection = null;
		}
		#endregion
	}
}
