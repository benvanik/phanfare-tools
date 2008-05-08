using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public partial class PhanfareService
	{
		public GroupInfo[] GetMyGroups()
		{
			this.AssertSessionValid();

			Hashtable ht = this.MethodCall( "getmygroups" );

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseList<GroupInfo>( doc, "groupinfos", "num_groups", GroupInfo.FromXML );
		}

		public PublicProfile[] GetMyRelationships()
		{
			return this.GetMyRelationships( false );
		}

		public PublicProfile[] GetMyRelationships( bool externalLinks )
		{
			this.AssertSessionValid();

			return this.GetRelationships( false, 0, externalLinks );
		}

		public PublicProfile[] GetGroupRelationships( long groupId )
		{
			return this.GetGroupRelationships( groupId, false );
		}

		public PublicProfile[] GetGroupRelationships( long groupId, bool externalLinks )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "groupId", groupId );

			return this.GetRelationships( true, groupId, externalLinks );
		}

		private PublicProfile[] GetRelationships( bool forGroup, long groupId, bool externalLinks )
		{
			this.AssertSessionValid();

			Hashtable ht;
			if( forGroup == true )
			{
				ht = this.MethodCall( "getgrouprelations" );
				ht[ "group_id" ] = groupId;
			}
			else
			{
				ht = this.MethodCall( "getmyrelations" );
			}
			if( externalLinks == true )
				ht[ "external_links" ] = externalLinks;

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseList<PublicProfile>( doc, "profiles", "num_profiles", PublicProfile.FromXML );
		}
	}
}
