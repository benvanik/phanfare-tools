using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phanfare.ExternalAPI
{
	public class PublicProfileComparer : IComparer<PublicProfile>
	{
		public int Compare( PublicProfile x, PublicProfile y )
		{
			if( x == y )
				return 0;
			int lastResult = string.Compare( x.LastName, y.LastName, true );
			if( lastResult == 0 )
			{
				int firstResult = string.Compare( x.FirstName, y.FirstName, true );
				if( firstResult == 0 )
					return x.UserID.CompareTo( y.UserID );
				else
					return firstResult;
			}
			else
				return lastResult;
		}
	}
}
