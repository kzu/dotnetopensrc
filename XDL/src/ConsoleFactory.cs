namespace NMatrix.XDL
{
	using System;
	using System.Data;
	using NMatrix.XDL.Interfaces;

	/// <summary>
	/// Default factory for objects.
	/// </summary>
	public class ConsoleFactory : Factory
	{
		internal ConsoleFactory()
		{
		}

		public override BaseDataSet CreateDataSet(string schemaFile, Type typeToCreate)
		{
			Console.WriteLine("New " + typeToCreate.Name + " used.");
			return base.CreateDataSet(schemaFile, typeToCreate);
		}

		public override BaseDataSet CreateDataSet(string schemaFile, Type typeToCreate, DataSet state)
		{
			Console.WriteLine("New " + typeToCreate.Name + " used with preloaded data.");
			return base.CreateDataSet(schemaFile, typeToCreate, state);
		}

		public override IDataSet CreateIDataSet(string schemaFile, Type typeToCreate)
		{
			Console.WriteLine("New " + typeToCreate.Name + " used.");
			return base.CreateIDataSet(schemaFile, typeToCreate);
		}

		public override IDataSet CreateIDataSet(string schemaFile, Type typeToCreate, DataSet state)
		{
			Console.WriteLine("New " + typeToCreate.Name + " used with preloaded data.");
			return base.CreateIDataSet(schemaFile, typeToCreate, state);
		}

		public override IDataRow CreateIDataRow(Type typeToCreate)
		{
			Console.WriteLine("New " + typeToCreate.Name + " used.");
			return base.CreateIDataRow(typeToCreate);
		}

		public override IDataRow CreateIDataRow(Type typeToCreate, DataRow state)
		{
			Console.WriteLine("New " + typeToCreate.Name + " used.");
			return base.CreateIDataRow(typeToCreate, state);
		}

		public override IDataTable CreateIDataTable(Type typeToCreate)
		{
			Console.WriteLine("New " + typeToCreate.Name + " used.");
			return base.CreateIDataTable(typeToCreate);
		}

		public override IDataTable CreateIDataTable(Type typeToCreate, DataTable state)
		{
			Console.WriteLine("New " + typeToCreate.Name + " used with preloaded data.");
			return base.CreateIDataTable(typeToCreate, state);
		}
	}
}


