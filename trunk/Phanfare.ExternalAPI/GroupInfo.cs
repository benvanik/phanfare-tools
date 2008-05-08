using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public class GroupInfo
	{
		public long GroupID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public long OwnerID { get; set; }

		public static GroupInfo FromXML( XmlElement el )
		{
			GroupInfo group = new GroupInfo();
			group.GroupID = el.GetAttributeInt64( "group_id" );
			group.Name = el.GetAttribute( "name" );
			group.Description = el.GetAttribute( "descr" );
			group.OwnerID = el.GetAttributeInt64( "owner_uid" );
			return group;
		}
	}
}
