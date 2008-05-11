using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phanfare.ExternalAPI
{
	public class GroupInfoComparer : IComparer<GroupInfo>
	{
		public int Compare( GroupInfo x, GroupInfo y )
		{
			if( x == y )
				return 0;
			int lastResult = string.Compare( x.Name, y.Name, true );
			if( lastResult == 0 )
				return x.GroupID.CompareTo( y.GroupID );
			else
				return lastResult;
		}
	}
}
