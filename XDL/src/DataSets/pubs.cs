//------------------------------------------------------------------------------
// This code file was generated by XSDT Code Generator.
// You may inherit these classes to add functionality. 
// Changes to this file will be lost upon regeneration.
//
// Author:    Daniel Cazzulino
// Source:    Pubs.xsd
// Transform: dataset.xslt
// Generated: Monday, December 17, 2001 1:13 AM
//------------------------------------------------------------------------------

namespace NMatrix.XDL.DataSets
{		
	using System;
	using System.Data;
	using System.Xml;
	using System.Runtime.Serialization;
	using NMatrix.XDL.DataSets;
	using NMatrix.XDL.Wrappers;
		
	[Serializable()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public class dsPubs : BaseDataSet 
	{
		internal dsPubs(string schemaFile) : base(schemaFile)
		{
		}
		
		internal dsPubs(string schemaFile, InternalDataSet state) : base(schemaFile, state)
		{
		}
		
		#region Public DataTable accessors
			[System.ComponentModel.Browsable(false)]
			[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
			public publishersDataTable publishers
			{
				
				get { return (publishersDataTable)RetrieveDataTable(typeof(publishersDataTable), "publishers"); }
			}
			[System.ComponentModel.Browsable(false)]
			[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
			public titlesDataTable titles
			{
				
				get { return (titlesDataTable)RetrieveDataTable(typeof(titlesDataTable), "titles"); }
			}
			[System.ComponentModel.Browsable(false)]
			[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
			public titleauthorsDataTable titleauthors
			{
				
				get { return (titleauthorsDataTable)RetrieveDataTable(typeof(titleauthorsDataTable), "titleauthors"); }
			}
			[System.ComponentModel.Browsable(false)]
			[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
			public authorsDataTable authors
			{
				
				get { return (authorsDataTable)RetrieveDataTable(typeof(authorsDataTable), "authors"); }
			}
		#endregion
		
		#region Delegates declarations
			public delegate void publishersRowChangeEventHandler(object sender, publishersRowChangeEvent e);
		
			public delegate void titlesRowChangeEventHandler(object sender, titlesRowChangeEvent e);
		
			public delegate void titleauthorsRowChangeEventHandler(object sender, titleauthorsRowChangeEvent e);
		
			public delegate void authorsRowChangeEventHandler(object sender, authorsRowChangeEvent e);
		#endregion
				
	public class publishersDataTable : BaseDataTable 
	{
		protected publishersDataTable(DataTable table) : base(table)
		{
		}
		
		public publishersRow this[int index] 
		{
			get { return ((publishersRow)(this.Rows[index])); }
		}
	
		
		public event publishersRowChangeEventHandler publishersRowChanged;

		public event publishersRowChangeEventHandler publishersRowChanging;

		public event publishersRowChangeEventHandler publishersRowDeleted;

		public event publishersRowChangeEventHandler publishersRowDeleting;
		
		public void Add(publishersRow row) 
		{
			this.Rows.Add(row);
		}
        
		public publishersRow Add(string pub_id, string pub_name, string city, string state, string country) 
		{
			publishersRow row = this.NewRow();
			row.ItemArray = new object[] 
				{ pub_id, pub_name, city, state, country };
			this.Rows.Add(row);
			return row;
		}
					
		public new publishersRow NewRow() 
		{
			return ((publishersRow)(base.NewRow()));
		}

		protected override Type GetRowType() 
		{
			return typeof(publishersRow);
		}
        
		#region Event raising methods
		
			protected override void OnRowChanged(DataRowChangeEventArgs e) 
			{
				base.OnRowChanged(e);
				if ((this.publishersRowChanged != null)) 
				{
					this.publishersRowChanged(this, new publishersRowChangeEvent(new publishersRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowChanging(DataRowChangeEventArgs e) 
			{
				base.OnRowChanging(e);
				if ((this.publishersRowChanging != null)) 
				{
					this.publishersRowChanging(this, new publishersRowChangeEvent(new publishersRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowDeleted(DataRowChangeEventArgs e) 
			{
				base.OnRowDeleted(e);
				if ((this.publishersRowDeleted != null)) 
				{
					this.publishersRowDeleted(this, new publishersRowChangeEvent(new publishersRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowDeleting(DataRowChangeEventArgs e) 
			{
				base.OnRowDeleting(e);
				if ((this.publishersRowDeleting != null)) 
				{
					this.publishersRowDeleting(this, new publishersRowChangeEvent(new publishersRow(e.Row), e.Action));
				}
			}
		#endregion
        
		
		public void Remove(publishersRow row) 
		{
			this.Rows.Remove(row);
		}

		public void Remove(DataRow row) 
		{
			this.Rows.Remove(row);
		}
	}
		
	public class publishersRow : BaseRow 
	{
		internal publishersRow(DataRow row) : base(row) 
		{
		}
		
		internal publishersRow() 
		{
		}
		
		protected override Type GetRowType()
		{
			return typeof(publishersRow);
		}

		protected override Type GetTableType()
		{
			return typeof(publishersDataTable);
		}

			
		#region Schema properties
		
		public string pub_id
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("pub_id"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("pub_id", value);
			}
		}		
	
		public string pub_name
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("pub_name"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("pub_name", value);
			}
		}		
	
		public string city
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("city"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("city", value);
			}
		}		
	
		public string state
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("state"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("state", value);
			}
		}		
	
		public string country
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("country"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("country", value);
			}
		}		
	
		public titlesRow[] titles
		{
			get 
			{ 
				return ((titlesRow[])(this.GetChildRows(this.Table.ChildRelations["publisherstitles"], typeof(titlesRow))));
			}

			set 
			{ 
				//TODO: MS TypedDataSetGenerator does nothings here. What should be done? Insert rows?
				throw new InvalidOperationException("Functionality not available.");
			}
		}
	
		#endregion
		
		#region Null-related Methods
		
		public bool Ispub_idNull
		{
			get { return this.IsNullColumn("pub_id"); }
			set { if (value == true) SetNullColumn("pub_id"); }
		}		
					
		public bool Ispub_nameNull
		{
			get { return this.IsNullColumn("pub_name"); }
			set { if (value == true) SetNullColumn("pub_name"); }
		}		
					
		public bool IscityNull
		{
			get { return this.IsNullColumn("city"); }
			set { if (value == true) SetNullColumn("city"); }
		}		
					
		public bool IsstateNull
		{
			get { return this.IsNullColumn("state"); }
			set { if (value == true) SetNullColumn("state"); }
		}		
					
		public bool IscountryNull
		{
			get { return this.IsNullColumn("country"); }
			set { if (value == true) SetNullColumn("country"); }
		}		
					
		#endregion
		
	}

	public class publishersRowChangeEvent : EventArgs 
	{    
		private publishersRow eventRow;
		private System.Data.DataRowAction eventAction;
		
		public publishersRowChangeEvent(publishersRow row, DataRowAction action) 
		{
			this.eventRow = row;
			this.eventAction = action;
		}
		
		public publishersRow Row 
		{
			get { return this.eventRow; }
		}
		
		public DataRowAction Action 
		{
			get { return this.eventAction; }
		}
		}
			
	public class titlesDataTable : BaseDataTable 
	{
		internal titlesDataTable(DataTable table) : base(table)
		{
		}
		
		public titlesRow this[int index] 
		{
			get { return ((titlesRow)(this.Rows[index])); }
		}
	
		
		public event titlesRowChangeEventHandler titlesRowChanged;

		public event titlesRowChangeEventHandler titlesRowChanging;

		public event titlesRowChangeEventHandler titlesRowDeleted;

		public event titlesRowChangeEventHandler titlesRowDeleting;
		
		public void Add(titlesRow row) 
		{
			this.Rows.Add(row);
		}
        
		public titlesRow Add(string title_id, string description, string type, string pub_id, decimal price, decimal advance, Int32 royalty, Int32 ytd_sales, string notes, DateTime pubdate) 
		{
			titlesRow row = ((titlesRow)(this.NewRow()));
			row.ItemArray = new object[] 
				{ title_id, description, type, pub_id, price, advance, royalty, ytd_sales, notes, pubdate };
			this.Rows.Add(row);
			return row;
		}
					
		public new titlesRow NewRow() 
		{
			return ((titlesRow)(base.NewRow()));
		}

		protected override Type GetRowType() 
		{
			return typeof(titlesRow);
		}
        
		#region Event raising methods
		
			protected override void OnRowChanged(DataRowChangeEventArgs e) 
			{
				base.OnRowChanged(e);
				if ((this.titlesRowChanged != null)) 
				{
					this.titlesRowChanged(this, new titlesRowChangeEvent(new titlesRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowChanging(DataRowChangeEventArgs e) 
			{
				base.OnRowChanging(e);
				if ((this.titlesRowChanging != null)) 
				{
					this.titlesRowChanging(this, new titlesRowChangeEvent(new titlesRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowDeleted(DataRowChangeEventArgs e) 
			{
				base.OnRowDeleted(e);
				if ((this.titlesRowDeleted != null)) 
				{
					this.titlesRowDeleted(this, new titlesRowChangeEvent(new titlesRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowDeleting(DataRowChangeEventArgs e) 
			{
				base.OnRowDeleting(e);
				if ((this.titlesRowDeleting != null)) 
				{
					this.titlesRowDeleting(this, new titlesRowChangeEvent(new titlesRow(e.Row), e.Action));
				}
			}
		#endregion
        
		
		public void Remove(titlesRow row) 
		{
			this.Rows.Remove(row);
		}
	}
		
	public class titlesRow : BaseRow 
	{
		internal titlesRow(DataRow row) : base(row) 
		{
		}
		
		internal titlesRow() 
		{
		}
		
		protected override Type GetRowType()
		{
			return typeof(titlesRow);
		}

		protected override Type GetTableType()
		{
			return typeof(titlesDataTable);
		}

			
		#region Schema properties
		
		public string title_id
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("title_id"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("title_id", value);
			}
		}		
	
		public string description
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("description"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("description", value);
			}
		}		
	
		public string type
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("type"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("type", value);
			}
		}		
	
		public string pub_id
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("pub_id"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("pub_id", value);
			}
		}		
	
		public decimal price
		{
			get 
			{ 
				try 
				{			
					return (decimal) GetValue("price"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("price", value);
			}
		}		
	
		public decimal advance
		{
			get 
			{ 
				try 
				{			
					return (decimal) GetValue("advance"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("advance", value);
			}
		}		
	
		public Int32 royalty
		{
			get 
			{ 
				try 
				{			
					return (Int32) GetValue("royalty"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("royalty", value);
			}
		}		
	
		public Int32 ytd_sales
		{
			get 
			{ 
				try 
				{			
					return (Int32) GetValue("ytd_sales"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("ytd_sales", value);
			}
		}		
	
		public string notes
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("notes"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("notes", value);
			}
		}		
	
		public DateTime pubdate
		{
			get 
			{ 
				try 
				{			
					return (DateTime) GetValue("pubdate"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("pubdate", value);
			}
		}		
	
		public titleauthorsRow[] titleauthors
		{
			get 
			{ 
				return ((titleauthorsRow[])(this.GetChildRows(this.Table.ChildRelations["titlestitleauthor"], typeof(titleauthorsRow))));
			}

			set 
			{ 
				//TODO: MS TypedDataSetGenerator does nothings here. What should be done? Insert rows?
				throw new InvalidOperationException("Functionality not available.");
			}
		}		
	
		#endregion
		
		#region Null-related Methods
		
		public bool Istitle_idNull
		{
			get { return this.IsNullColumn("title_id"); }
			set { if (value == true) SetNullColumn("title_id"); }
		}		
					
		public bool IsdescriptionNull
		{
			get { return this.IsNullColumn("description"); }
			set { if (value == true) SetNullColumn("description"); }
		}		
					
		public bool IstypeNull
		{
			get { return this.IsNullColumn("type"); }
			set { if (value == true) SetNullColumn("type"); }
		}		
					
		public bool Ispub_idNull
		{
			get { return this.IsNullColumn("pub_id"); }
			set { if (value == true) SetNullColumn("pub_id"); }
		}		
					
		public bool IspriceNull
		{
			get { return this.IsNullColumn("price"); }
			set { if (value == true) SetNullColumn("price"); }
		}		
					
		public bool IsadvanceNull
		{
			get { return this.IsNullColumn("advance"); }
			set { if (value == true) SetNullColumn("advance"); }
		}		
					
		public bool IsroyaltyNull
		{
			get { return this.IsNullColumn("royalty"); }
			set { if (value == true) SetNullColumn("royalty"); }
		}		
					
		public bool Isytd_salesNull
		{
			get { return this.IsNullColumn("ytd_sales"); }
			set { if (value == true) SetNullColumn("ytd_sales"); }
		}		
					
		public bool IsnotesNull
		{
			get { return this.IsNullColumn("notes"); }
			set { if (value == true) SetNullColumn("notes"); }
		}		
					
		public bool IspubdateNull
		{
			get { return this.IsNullColumn("pubdate"); }
			set { if (value == true) SetNullColumn("pubdate"); }
		}		
					
		#endregion
		
		public publishersRow publishersRow
		{
			get { return ((publishersRow)(this.GetParentRow(this.Table.ParentRelations["publisherstitles"], typeof(publishersRow)))); }
			set { SetParentRow(value, this.Table.ParentRelations["publisherstitles"]); }
		}
	}

	public class titlesRowChangeEvent : EventArgs 
	{    
		private titlesRow eventRow;
		private System.Data.DataRowAction eventAction;
		
		public titlesRowChangeEvent(titlesRow row, DataRowAction action) 
		{
			this.eventRow = row;
			this.eventAction = action;
		}
		
		public titlesRow Row 
		{
			get { return this.eventRow; }
		}
		
		public DataRowAction Action 
		{
			get { return this.eventAction; }
		}
		}
			
	public class titleauthorsDataTable : BaseDataTable 
	{
		internal titleauthorsDataTable(DataTable table) : base(table)
		{
		}
		
		public titleauthorsRow this[int index] 
		{
			get { return ((titleauthorsRow)(this.Rows[index])); }
		}
	
		
		public event titleauthorsRowChangeEventHandler titleauthorsRowChanged;

		public event titleauthorsRowChangeEventHandler titleauthorsRowChanging;

		public event titleauthorsRowChangeEventHandler titleauthorsRowDeleted;

		public event titleauthorsRowChangeEventHandler titleauthorsRowDeleting;
		
		public void Add(titleauthorsRow row) 
		{
			this.Rows.Add(row);
		}
        
		public titleauthorsRow Add(string au_id, string title_id, Byte au_ord, Int32 royaltyper) 
		{
			titleauthorsRow row = ((titleauthorsRow)(this.NewRow()));
			row.ItemArray = new object[] 
				{ au_id, title_id, au_ord, royaltyper };
			this.Rows.Add(row);
			return row;
		}
					
		public new titleauthorsRow NewRow() 
		{
			return ((titleauthorsRow)(base.NewRow()));
		}

		protected override Type GetRowType() 
		{
			return typeof(titleauthorsRow);
		}
        
		#region Event raising methods
		
			protected override void OnRowChanged(DataRowChangeEventArgs e) 
			{
				base.OnRowChanged(e);
				if ((this.titleauthorsRowChanged != null)) 
				{
					this.titleauthorsRowChanged(this, new titleauthorsRowChangeEvent(new titleauthorsRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowChanging(DataRowChangeEventArgs e) 
			{
				base.OnRowChanging(e);
				if ((this.titleauthorsRowChanging != null)) 
				{
					this.titleauthorsRowChanging(this, new titleauthorsRowChangeEvent(new titleauthorsRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowDeleted(DataRowChangeEventArgs e) 
			{
				base.OnRowDeleted(e);
				if ((this.titleauthorsRowDeleted != null)) 
				{
					this.titleauthorsRowDeleted(this, new titleauthorsRowChangeEvent(new titleauthorsRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowDeleting(DataRowChangeEventArgs e) 
			{
				base.OnRowDeleting(e);
				if ((this.titleauthorsRowDeleting != null)) 
				{
					this.titleauthorsRowDeleting(this, new titleauthorsRowChangeEvent(new titleauthorsRow(e.Row), e.Action));
				}
			}
		#endregion
        
		
		public void Remove(titleauthorsRow row) 
		{
			this.Rows.Remove(row);
		}
	}
		
	public class titleauthorsRow : BaseRow 
	{
		internal titleauthorsRow(DataRow row) : base(row) 
		{
		}
		
		internal titleauthorsRow() 
		{
		}
		
		protected override Type GetRowType()
		{
			return typeof(titleauthorsRow);
		}

		protected override Type GetTableType()
		{
			return typeof(titleauthorsDataTable);
		}

			
		#region Schema properties
		
		public string au_id
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("au_id"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("au_id", value);
			}
		}		
	
		public string title_id
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("title_id"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("title_id", value);
			}
		}		
	
		public Byte au_ord
		{
			get 
			{ 
				try 
				{			
					return (Byte) GetValue("au_ord"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("au_ord", value);
			}
		}		
	
		public Int32 royaltyper
		{
			get 
			{ 
				try 
				{			
					return (Int32) GetValue("royaltyper"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("royaltyper", value);
			}
		}		
	
		public authorsRow[] authors
		{
			get 
			{ 
				return ((authorsRow[])(this.GetChildRows(this.Table.ChildRelations["titleauthorauthors"], typeof(authorsRow))));
			}

			set 
			{ 
				//TODO: MS TypedDataSetGenerator does nothings here. What should be done? Insert rows?
				throw new InvalidOperationException("Functionality not available.");
			}
		}		
	
		#endregion
		
		#region Null-related Methods
		
		public bool Isau_idNull
		{
			get { return this.IsNullColumn("au_id"); }
			set { if (value == true) SetNullColumn("au_id"); }
		}		
					
		public bool Istitle_idNull
		{
			get { return this.IsNullColumn("title_id"); }
			set { if (value == true) SetNullColumn("title_id"); }
		}		
					
		public bool Isau_ordNull
		{
			get { return this.IsNullColumn("au_ord"); }
			set { if (value == true) SetNullColumn("au_ord"); }
		}		
					
		public bool IsroyaltyperNull
		{
			get { return this.IsNullColumn("royaltyper"); }
			set { if (value == true) SetNullColumn("royaltyper"); }
		}		
					
		#endregion
		
		public titlesRow titlesRow
		{
			get { return ((titlesRow)(this.GetParentRow(this.Table.ParentRelations["titlestitleauthor"], typeof(titlesRow)))); }
			set { this.SetParentRow(value, this.Table.ParentRelations["titlestitleauthor"]); }
		}
	}

	public class titleauthorsRowChangeEvent : EventArgs 
	{    
		private titleauthorsRow eventRow;
		private System.Data.DataRowAction eventAction;
		
		public titleauthorsRowChangeEvent(titleauthorsRow row, DataRowAction action) 
		{
			this.eventRow = row;
			this.eventAction = action;
		}
		
		public titleauthorsRow Row 
		{
			get { return this.eventRow; }
		}
		
		public DataRowAction Action 
		{
			get { return this.eventAction; }
		}
		}
			
	public class authorsDataTable : BaseDataTable 
	{
		internal authorsDataTable(DataTable table) : base(table)
		{
		}
		
		public authorsRow this[int index] 
		{
			get { return ((authorsRow)(this.Rows[index])); }
		}
	
		
		public event authorsRowChangeEventHandler authorsRowChanged;

		public event authorsRowChangeEventHandler authorsRowChanging;

		public event authorsRowChangeEventHandler authorsRowDeleted;

		public event authorsRowChangeEventHandler authorsRowDeleting;
		
		public void Add(authorsRow row) 
		{
			this.Rows.Add(row);
		}
        
		public authorsRow Add(string au_id, string au_lname, string au_fname, string phone, string address, string city, string state, string zip, bool contract) 
		{
			authorsRow row = ((authorsRow)(this.NewRow()));
			row.ItemArray = new object[] 
				{ au_id, au_lname, au_fname, phone, address, city, state, zip, contract };
			this.Rows.Add(row);
			return row;
		}
					
		public new authorsRow NewRow() 
		{
			return ((authorsRow)(base.NewRow()));
		}

		protected override Type GetRowType() 
		{
			return typeof(authorsRow);
		}
        
		#region Event raising methods
		
			protected override void OnRowChanged(DataRowChangeEventArgs e) 
			{
				base.OnRowChanged(e);
				if ((this.authorsRowChanged != null)) 
				{
					this.authorsRowChanged(this, new authorsRowChangeEvent(new authorsRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowChanging(DataRowChangeEventArgs e) 
			{
				base.OnRowChanging(e);
				if ((this.authorsRowChanging != null)) 
				{
					this.authorsRowChanging(this, new authorsRowChangeEvent(new authorsRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowDeleted(DataRowChangeEventArgs e) 
			{
				base.OnRowDeleted(e);
				if ((this.authorsRowDeleted != null)) 
				{
					this.authorsRowDeleted(this, new authorsRowChangeEvent(new authorsRow(e.Row), e.Action));
				}
			}
	        
			protected override void OnRowDeleting(DataRowChangeEventArgs e) 
			{
				base.OnRowDeleting(e);
				if ((this.authorsRowDeleting != null)) 
				{
					this.authorsRowDeleting(this, new authorsRowChangeEvent(new authorsRow(e.Row), e.Action));
				}
			}
		#endregion
        
		
		public void Remove(authorsRow row) 
		{
			this.Rows.Remove(row);
		}
	}
		
	public class authorsRow : BaseRow 
	{
		internal authorsRow(DataRow row) : base(row) 
		{
		}
		
		internal authorsRow() 
		{
		}
		
		protected override Type GetRowType()
		{
			return typeof(authorsRow);
		}

		protected override Type GetTableType()
		{
			return typeof(authorsDataTable);
		}

			
		#region Schema properties
		
		public string au_id
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("au_id"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("au_id", value);
			}
		}		
	
		public string au_lname
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("au_lname"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("au_lname", value);
			}
		}		
	
		public string au_fname
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("au_fname"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("au_fname", value);
			}
		}		
	
		public string phone
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("phone"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("phone", value);
			}
		}		
	
		public string address
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("address"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("address", value);
			}
		}		
	
		public string city
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("city"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("city", value);
			}
		}		
	
		public string state
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("state"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("state", value);
			}
		}		
	
		public string zip
		{
			get 
			{ 
				try 
				{			
					return (string) GetValue("zip"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("zip", value);
			}
		}		
	
		public bool contract
		{
			get 
			{ 
				try 
				{			
					return (bool) GetValue("contract"); 
				}
				catch (InvalidCastException e) 
				{
					throw new StrongTypingException("Cannot get value because it is DBNull.", e);
				}
			}

			set 
			{ 
				SetValue("contract", value);
			}
		}		
	
		#endregion
		
		#region Null-related Methods
		
		public bool Isau_idNull
		{
			get { return this.IsNullColumn("au_id"); }
			set { if (value == true) SetNullColumn("au_id"); }
		}		
					
		public bool Isau_lnameNull
		{
			get { return this.IsNullColumn("au_lname"); }
			set { if (value == true) SetNullColumn("au_lname"); }
		}		
					
		public bool Isau_fnameNull
		{
			get { return this.IsNullColumn("au_fname"); }
			set { if (value == true) SetNullColumn("au_fname"); }
		}		
					
		public bool IsphoneNull
		{
			get { return this.IsNullColumn("phone"); }
			set { if (value == true) SetNullColumn("phone"); }
		}		
					
		public bool IsaddressNull
		{
			get { return this.IsNullColumn("address"); }
			set { if (value == true) SetNullColumn("address"); }
		}		
					
		public bool IscityNull
		{
			get { return this.IsNullColumn("city"); }
			set { if (value == true) SetNullColumn("city"); }
		}		
					
		public bool IsstateNull
		{
			get { return this.IsNullColumn("state"); }
			set { if (value == true) SetNullColumn("state"); }
		}		
					
		public bool IszipNull
		{
			get { return this.IsNullColumn("zip"); }
			set { if (value == true) SetNullColumn("zip"); }
		}		
					
		public bool IscontractNull
		{
			get { return this.IsNullColumn("contract"); }
			set { if (value == true) SetNullColumn("contract"); }
		}		
					
		#endregion
		
		public titleauthorsRow titleauthorsRow
		{
			get { return ((titleauthorsRow)(this.GetParentRow(this.Table.ParentRelations["titleauthorauthors"], typeof(titleauthorsRow)))); }
			set { this.SetParentRow(value, this.Table.ParentRelations["titleauthorauthors"]); }
		}
	}

	public class authorsRowChangeEvent : EventArgs 
	{    
		private authorsRow eventRow;
		private System.Data.DataRowAction eventAction;
		
		public authorsRowChangeEvent(authorsRow row, DataRowAction action) 
		{
			this.eventRow = row;
			this.eventAction = action;
		}
		
		public authorsRow Row 
		{
			get { return this.eventRow; }
		}
		
		public DataRowAction Action 
		{
			get { return this.eventAction; }
		}
		}
	
	}
		
}
		