//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// SortedDuplicateKey.cs
// Provides an IComparable implementation for use in sorted lists which need
// duplicate keys.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;

namespace NMatrix.Core.Collections
{
	/// <summary>
	/// Provides an IComparable implementation for use in sorted 
	/// lists which need duplicate keys.
	/// </summary>
	/// <remarks>See the <c>IComparable.CompareTo</c> method implementaion
	/// for details of the comparing mechanism.</remarks>
	public class SortedDuplicateKey : IComparable
	{
		/// <summary>
		/// Keep the order value.
		/// </summary>
		private int _order = 0;

		/// <summary>
		/// Create a key with the specified order.
		/// </summary>
		/// <param name="order">The element's position to use.</param>
		public SortedDuplicateKey(int order)
		{
			_order = order;
		}

		/// <summary>
		/// The key's order.
		/// </summary>
		public int Order
		{
			get { return _order; }
		}

		/// <summary>
		/// Implements IComparable interface, for use by a sorted list.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>The result of the comparison</returns>
		/// <remarks>If the passed order is int.MaxValue, is equal or is greater than
		/// this instance order, we return -1. If the passed order is zero or less than
		/// this instance order, we return +1.
		/// We don't return 0 (equal) for any value to allow duplicate keys in the list.
		/// The drawback is that if we try to access the sorted list by index or key, an exception
		/// will be thrown, as no match will be found. The list can only be enumerated.
		/// </remarks>
		int IComparable.CompareTo(object obj)
		{
			SortedDuplicateKey key = obj as SortedDuplicateKey;
      
			// If the passed call order is MaxValue or is equal to us, put it above us.
			if (key.Order == int.MaxValue || key.Order == _order || key.Order > _order)
				return -1;
			// If the order is less, it goes before us.
			else if (key.Order < _order)
				return +1;
			//Dumb return.
			return 0;
		}
	}
}
