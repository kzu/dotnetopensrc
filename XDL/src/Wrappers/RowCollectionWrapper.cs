namespace NMatrix.XDL.Wrappers
{
	using System;
	using System.Data;
	using System.Collections;
	using NMatrix.XDL.Interfaces;

	public class RowCollectionWrapper : IDataRowCollection
	{
		protected Factory _factory = new Engine().FactoryInstance;
		private DataRowCollection _sourcecollection;
		private Type _rowtype;

		internal RowCollectionWrapper(Type rowType)
		{
			_rowtype = rowType;
		}

		internal RowCollectionWrapper(Type rowType, DataRowCollection collectionData) 
		{
			_rowtype = rowType;
			SetState(collectionData);
		}

		private IDataRow CreateDataRow()
		{
			return _factory.CreateIDataRow(_rowtype);;
		}

		public virtual DataRowCollection GetState()
		{
			return _sourcecollection;
		}

		public virtual void SetState(DataRowCollection state)
		{
			_sourcecollection = state;
		}

	#region Changed members
		public virtual IDataRow this[int index] 
		{
			get 
			{ 
				IDataRow obj = CreateDataRow();
				obj.SetState(_sourcecollection[index]);
				return obj; 
			}
		}

		public virtual IDataRow Add(object[] values) 
		{
			IDataRow obj = CreateDataRow();
			obj.SetState(_sourcecollection.Add(values));
			return obj; 
		}

		public virtual IEnumerator GetEnumerator() 
		{
			return new RowCollectionWrapperEnumerator(this);
		}

		public virtual IDataRow Find(object key) 
		{
			DataRow r = _sourcecollection.Find(key);
			if (r != null) 
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(r);
				return obj;
			}
			return null;
		}

		public virtual IDataRow Find(object[] keys) 
		{
			DataRow r = _sourcecollection.Find(keys);
			if (r != null) 
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(r);
				return obj;
			}
			return null;
		}

		public virtual void Add(IDataRow row)
		{
			_sourcecollection.Add(row.GetState());
		}

		public virtual void InsertAt(IDataRow row, int pos) 
		{
			_sourcecollection.InsertAt(row.GetState(), pos);
		}

		public virtual void Remove(IDataRow row) 
		{
			_sourcecollection.Remove(row.GetState());
		}

		public virtual void Add(DataRow row)
		{
			_sourcecollection.Add(row);
		}

		public virtual void InsertAt(DataRow row, int pos) 
		{
			_sourcecollection.InsertAt(row, pos);
		}

		public virtual void Remove(DataRow row) 
		{
			_sourcecollection.Remove(row);
		}
	#endregion

	#region Properties
		public virtual int Count 
		{
			get { return _sourcecollection.Count; }
		}

		public virtual bool IsReadOnly 
		{
			get 
			{
				return _sourcecollection.IsReadOnly;
			}
		}

		public virtual bool IsSynchronized 
		{
			get 
			{
				return _sourcecollection.IsSynchronized;
			}
		}

		public virtual object SyncRoot 
		{
			get 
			{
				return _sourcecollection.SyncRoot;
			}
		}
	#endregion

	#region Common methods
		public virtual void CopyTo(System.Array ar, int index) 
		{
			_sourcecollection.CopyTo(ar, index);
		}

		public override int GetHashCode() 
		{
			return _sourcecollection.GetHashCode();
		}

		public override bool Equals(object obj) 
		{
			return _sourcecollection.Equals(obj);
		}

		public override string ToString() 
		{
			return _sourcecollection.ToString();
		}

		public virtual void Clear() 
		{
			_sourcecollection.Clear();
		}

		public virtual bool Contains(object key) 
		{
			return _sourcecollection.Contains(key);
		}

		public virtual bool Contains(object[] keys) 
		{
			return _sourcecollection.Contains(keys);
		}

		public virtual void RemoveAt(int index) 
		{
			_sourcecollection.RemoveAt(index);
		}
	#endregion    

	#region Collection enumerator
		/// <summary>
		///		A custom enumerator for the collection, made private to avoid direct creation
		/// </summary>
		private class RowCollectionWrapperEnumerator: System.Collections.IEnumerator
		{
			private RowCollectionWrapper _col;
			private int _current;

			public RowCollectionWrapperEnumerator(RowCollectionWrapper source)
			{
				_col = source;
				_current = -1;
			}
			public object Current
			{
				get { return _col[_current]; }
			}
			public bool MoveNext()
			{
				if (_current == _col.Count -1) { return false; }
				_current += 1;
				return true;
			}
			public void Reset()
			{
				_current = -1;
			}
		}
	#endregion
	}
}