namespace NMatrix.XDL.Interfaces
{
	using System;
	using System.Data;
	using System.Collections;

	public interface IDataRowCollection : IEnumerable
	{

		DataRowCollection GetState();
		void SetState(DataRowCollection state);

	#region Changed members
		IDataRow this[int index]
		{
			get ;
		}
		IDataRow Add(object[] values);
		IDataRow Find(object key);
		IDataRow Find(object[] keys);
		void Add(IDataRow row);
		void InsertAt(IDataRow row, int pos);
		void Remove(IDataRow row);
		//Original syntax
		void Add(DataRow row);
		void InsertAt(DataRow row, int pos);
		void Remove(DataRow row);
	#endregion

	#region Properties
		int Count 
		{
			get ;
		}

		bool IsReadOnly 
		{
			get ;
		}

		bool IsSynchronized 
		{
			get ;
		}

		object SyncRoot 
		{
			get ;
		}
	#endregion

	#region Common methods
		void CopyTo(System.Array ar, int index);
		void Clear();
		bool Contains(object key);
		bool Contains(object[] keys);
		void RemoveAt(int index);
	#endregion
	}
}