namespace NMatrix.XDL
{
	using System;
	using System.Data;
	using NMatrix.XDL.Interfaces;
	using System.Reflection;
	using System.Globalization;

	/// <summary>
	/// Default factory for objects.
	/// </summary>
	public class Factory
	{
		internal Factory()
		{
		}

		public virtual BaseDataSet CreateDataSet(string schemaFile, Type typeToCreate)
		{
			EnsureBaseType(typeToCreate, typeof(BaseDataSet));
			return (BaseDataSet) Activator.CreateInstance(typeToCreate, 
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
				Type.DefaultBinder, new object[] { schemaFile }, CultureInfo.CurrentCulture);
		}

		public virtual BaseDataSet CreateDataSet(string schemaFile, Type typeToCreate, DataSet state)
		{
			EnsureBaseType(typeToCreate, typeof(BaseDataSet));
			return (BaseDataSet) Activator.CreateInstance(typeToCreate, 
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
				Type.DefaultBinder, new object[] { schemaFile, state }, CultureInfo.CurrentCulture);
		}

		public virtual IDataSet CreateIDataSet(string schemaFile, Type typeToCreate)
		{
			EnsureInterface(typeToCreate, typeof(IDataSet));
			return (IDataSet) CreateDataSet(schemaFile, typeToCreate);
		}

		public virtual IDataSet CreateIDataSet(string schemaFile, Type typeToCreate, DataSet state)
		{
			EnsureInterface(typeToCreate, typeof(IDataSet));
			return (IDataSet) CreateDataSet(schemaFile, typeToCreate, state);
		}

		public virtual IDataRow CreateIDataRow(Type typeToCreate)
		{
			EnsureInterface(typeToCreate, typeof(IDataRow));
			return (IDataRow) Activator.CreateInstance(typeToCreate, 
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
				Type.DefaultBinder, new object[0], CultureInfo.CurrentCulture);
		}

		public virtual IDataRow CreateIDataRow(Type typeToCreate, DataRow state)
		{
			EnsureInterface(typeToCreate, typeof(IDataRow));
			return (IDataRow) Activator.CreateInstance(typeToCreate, 
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
				Type.DefaultBinder, new object[] { state }, CultureInfo.CurrentCulture);
		}

		public virtual IDataTable CreateIDataTable(Type typeToCreate)
		{
			EnsureInterface(typeToCreate, typeof(IDataTable));
			return (IDataTable) Activator.CreateInstance(typeToCreate, 
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
				Type.DefaultBinder, new object[0], CultureInfo.CurrentCulture);
		}

		public virtual IDataTable CreateIDataTable(Type typeToCreate, DataTable state)
		{
			EnsureInterface(typeToCreate, typeof(IDataTable));
			return (IDataTable) Activator.CreateInstance(typeToCreate, 
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
				Type.DefaultBinder, new object[] { state }, CultureInfo.CurrentCulture);
		}

		private void EnsureInterface(Type typeToCheck, Type typeToEnsure)
		{
			if (typeToCheck.FindInterfaces(System.Reflection.Module.FilterTypeName, typeToEnsure.Name).Length == 0)
				throw new ArgumentException(String.Format("Type to create must implement {0}.", typeToEnsure.Name));
		}

		private void EnsureBaseType(Type typeToCheck, Type typeToEnsure)
		{
			if (!typeToCheck.IsSubclassOf(typeToEnsure) && typeToCheck != typeToEnsure)
				throw new ArgumentException(String.Format("Type to create must be a {0} descendent.", typeToEnsure.Name));
		}
	}
}
