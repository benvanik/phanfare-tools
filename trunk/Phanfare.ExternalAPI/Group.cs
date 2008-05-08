using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public class Group
	{
		public long GroupID { get; set; }

		public static Group FromXML( XmlElement el )
		{
			Group group = new Group();
			group.GroupID = el.GetAttributeInt64( "group_id" );
			return group;
		}
	}
}
