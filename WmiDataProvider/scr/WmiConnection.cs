using System;
using System.Data;
using System.Management;
using System.Text.RegularExpressions;

namespace NMatrix.WmiDataProvider
{
	/// <summary>
	/// Provides a connection to WMI.
	/// </summary>
	public sealed class WmiConnection : IDbConnection
	{
		#region Ctor & vars

		/// <summary>
		/// Matching string for the connection string message.
		/// </summary>
		static Regex _regconnection = new Regex(@"(Server=(?<server>[^;]*))?[;]?(Namespace=(?<namespace>[^;]*))?[;]?(User=(?<user>[^;]*))?[;]?(Password=(?<pwd>[^;]*))?[;]?", RegexOptions.Compiled);

		/// <summary>
		/// Initializes a new instance of the connection.
		/// </summary>
		public WmiConnection ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the connection with the received connection string.
		/// </summary>
		/// <param name="connectionString">The string to use to connect to the server.</param>
		public WmiConnection(string connectionString)
		{
			_connection = connectionString;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The scope to be used by other classes to connect and query the server. 
		/// </summary>
		internal ManagementScope Scope
		{
			get { return _scope; }
		} ManagementScope _scope = null;

		/// <summary>
		/// The connection string used to connect to the server. Format: Server=[server];Namespace=[namespace];User=[user];Password=[password]
		/// </summary>
		public string ConnectionString
		{
			get { return _connection; }
			set { _connection = value; }
		} string _connection;

		/// <summary>
		/// Not implemented. Always returns zero.
		/// </summary>
		public int ConnectionTimeout
		{
			get { return 0; }
			set { }
		}

		/// <summary>
		/// Returns the server+namespace currently active.
		/// </summary>
		public string Database
		{
			get 
			{
				if (_connection == null || _connection == String.Empty)
					return String.Empty;
				else
				{
					Match m = _regconnection.Match(_connection);
					return m.Groups["server"] + "\\" + m.Groups["namespace"].Value;
				}
			}
		}
		
		/// <summary>
		/// The state of the connection 
		/// </summary>
		public ConnectionState State
		{
			get { return _state; }
		} ConnectionState _state = ConnectionState.Closed;

		#endregion

		#region Public methods

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <returns></returns>
		public IDbTransaction BeginTransaction()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <returns></returns>
		public IDbTransaction BeginTransaction(IsolationLevel iLevel)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		/// <returns></returns>
		public void ChangeDatabase(string databaseName)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Creates and returns a WmiCommand object.
		/// </summary>
		/// <returns></returns>
		public IDbCommand CreateCommand()
		{
			return new WmiCommand();
		}

		/// <summary>
		/// Opens the connection to the Wmi Server.
		/// </summary>
		public void Open()
		{
			//Check status.
			if (_state == ConnectionState.Open)
				return;

			// make sure the connection string is valid
			if (_connection != "")
			{
				Match m = _regconnection.Match(ConnectionString);
				// create a new scope to connect to.
				ManagementPath path = new ManagementPath();
				path.Server = m.Groups["server"].Value;
				path.NamespacePath = m.Groups["namespace"].Value;
				_scope = new ManagementScope(path);
				if (String.Compare(path.Server, "localhost", true) != 0)
				{
					_scope.Options.Username = m.Groups["user"].Value;
					_scope.Options.Password = m.Groups["pwd"].Value;
					_scope.Options.Impersonation = ImpersonationLevel.Impersonate;
				}
				_scope.Connect();
				_state = ConnectionState.Open;
			}
			else
			{
				throw new Exception("Invalid Connection String.");
			}
		}
		/// <summary>
		/// Closes the connection to the server
		/// </summary>
		public void Close()
		{
			_scope = null;
			_state = ConnectionState.Closed;
		}

		#endregion

		#region Implementation of IDisposable
		/// <summary>
		/// Releases resources used by the connection.
		/// </summary>
		public void Dispose()
		{
		}
		#endregion
	}
}
