using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public class Session
	{
		public string SessionCookie { get; set; }
		public long UserID { get; set; }
		public bool IsPremium { get; set; }
		public long FriendGroupID { get; set; }
		public long FamilyGroupID { get; set; }
		public long PublicGroupID { get; set; }
		public string WebsiteTitle { get; set; }
		public string TimelessHeader { get; set; }
		public bool IsTimelessFirst { get; set; }
		public PublicProfile Profile { get; set; }

		public static Session FromXML( XmlElement el )
		{
			Session session = new Session();
			session.SessionCookie = el.GetAttribute( "cookie" );
			session.UserID = el.GetAttributeInt64( "uid" );
			session.IsPremium = el.GetAttributeBoolean( "premium" );
			session.FriendGroupID = el.GetAttributeInt64( "friend_group_id" );
			session.FamilyGroupID = el.GetAttributeInt64( "family_group_id" );
			session.PublicGroupID = el.GetAttributeInt64( "public_group_id" );
			session.WebsiteTitle = el.GetAttribute( "website_title" );
			session.TimelessHeader = el.GetAttribute( "timeless_header" );
			session.IsTimelessFirst = el.GetAttributeBoolean( "timeless_first" );
			session.Profile = PublicProfile.FromXML( el[ "profile" ] );
			return session;
		}
	}
}
