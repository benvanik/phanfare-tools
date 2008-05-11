using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public enum Relation
	{
		Self,
		Friend,
		Family,
	}

	public class PublicProfile
	{
		public long UserID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Relation Relation { get; set; }
		public bool IsPremium { get; set; }
		public string ImageURL { get; set; }

		public static PublicProfile FromXML( XmlElement el )
		{
			PublicProfile profile = new PublicProfile();
			profile.UserID = el.GetAttributeInt64( "uid" );
			profile.FirstName = el.GetAttribute( "first_name" );
			profile.LastName = el.GetAttribute( "last_name" );
			profile.Relation = el.GetAttributeEnum<Relation>( "relation" );
			profile.IsPremium = el.GetAttributeBoolean( "premium" );
			profile.ImageURL = el.GetAttribute( "image_url" );
			return profile;
		}
	}
}
