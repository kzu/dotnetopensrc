using System;
using System.Data;
using NMatrix.XDL;
using NMatrix.XDL.DataSets;
using NMatrix.XDL.Wrappers;

namespace NMatrix.XDL
{
	/// <summary>
	/// Summary description for PublisherSchema.
	/// </summary>
	public class PublisherSchema : dsPubs
	{
		internal PublisherSchema(string schemaFile) : base(schemaFile)
		{
		}
		
		internal PublisherSchema(string schemaFile, InternalDataSet state) : base(schemaFile, state)
		{
		}

		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public PublisherCollection Publishers
		{
				
			get { return (PublisherCollection)RetrieveDataTable(typeof(PublisherCollection), "publishers"); }
		}

		public class PublisherCollection : dsPubs.publishersDataTable
		{
			internal PublisherCollection(DataTable table) : base(table)
			{
			}

			public new Publisher this[int index] 
			{
				get { return ((Publisher)(this.Rows[index])); }
			}

			public new Publisher NewRow() 
			{
				return ((Publisher)(base.NewRow()));
			}

			protected override Type GetRowType() 
			{
				return typeof(Publisher);
			}
		}

		public class Publisher : dsPubs.publishersRow 
		{
			internal Publisher(DataRow row) : base(row) 
			{
			}
		
			internal Publisher() 
			{
			}
		
			protected override Type GetRowType()
			{
				return typeof(Publisher);
			}

			protected override Type GetTableType()
			{
				return typeof(PublisherCollection);
			}

			public void CheckAccount()
			{
				//This is a custom added functionality.
			}
		}
	}
}
