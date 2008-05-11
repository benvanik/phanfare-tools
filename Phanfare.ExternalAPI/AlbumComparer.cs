using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phanfare.ExternalAPI
{
	public class AlbumComparer : IComparer<Album>
	{
		public bool ReverseWithinYear;
		public bool TimelessFirst;

		public AlbumComparer()
			: this( true, false )
		{
		}

		public AlbumComparer( bool reverseWithinYear, bool timelessFirst )
		{
			this.ReverseWithinYear = reverseWithinYear;
			this.TimelessFirst = timelessFirst;
		}

		public int Compare( Album x, Album y )
		{
			if( x == y )
				return 0;
			if( x == null )
				return -1;
			if( y == null )
				return 1;

			int? timelessCompare = this.TimelessCompare( x, y );
			if( timelessCompare != null )
				return timelessCompare.Value;

			// Newest year first
			DateTime endDateX = x.EndDate.Value;
			DateTime endDateY = y.EndDate.Value;
			if( endDateX.Year > endDateY.Year )
				return -1;
			else if( endDateX.Year < endDateY.Year )
				return 1;
			else
			{
				// 1 to sort descending, -1 to sort ascending (reverse order)
				int sorter = ( this.ReverseWithinYear == false ) ? 1 : -1;

				// Years are same
				int dateCompare = endDateX.CompareTo( endDateY );
				if( dateCompare != 0 )
					return dateCompare * sorter;
				else
				{
					return -x.AlbumID.CompareTo( y.AlbumID ) * sorter;
				}
			}
		}

		private int? TimelessCompare( Album x, Album y )
		{
			if( ( x.Type == AlbumType.Timeless ) && ( y.Type == AlbumType.Timeless ) )
			{
				int ret = string.Compare( x.Name, y.Name, true );
				if( ret == 0 )
				{
					// Same? Compare by album id
					return x.AlbumID.CompareTo( y.AlbumID );
				}
				else
					return ret;
			}
			else if( x.Type == AlbumType.Timeless )
				return this.TimelessFirst ? -1 : 1;
			else if( y.Type == AlbumType.Timeless )
				return this.TimelessFirst ? 1 : -1;
			else
				return null;
		}
	}
}
