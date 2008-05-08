using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public enum AlbumType
	{
		Dated,
		Timeless,
	}

	public class Album
	{
		public long AlbumID { get; set; }
		public AlbumType Type { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime LastModified { get; set; }
		public bool? AutoDate { get; set; }
		public string URL { get; set; }
		public long OwnerID { get; set; }

		public Section[] Sections { get; set; }
		public Group[] Groups { get; set; }

		public static Album FromXML( XmlElement el )
		{
			Album album = new Album();
			album.AlbumID = el.GetAttributeInt64( "album_id" );
			album.Type = el.GetAttributeEnum<AlbumType>( "album_type" );
			album.Name = el.GetAttribute( "album_name" );
			album.Description = el.GetAttribute( "album_descr" );
			album.StartDate = el.GetAttributeDate( "album_start_date" );
			album.EndDate = el.GetAttributeDate( "album_end_date" );
			album.CreationDate = el.GetAttributeDate( "album_creation_date" );
			album.LastModified = el.GetAttributeDate( "album_last_modified" );
			album.AutoDate = el.GetAttributeBoolean( "auto_date" );
			album.URL = el.GetAttribute( "album_url" );
			album.OwnerID = el.GetAttributeInt64( "owner_uid" );

			XmlElement sectionsElement = el[ "sections" ];
			album.Sections = sectionsElement.ReadList<Section>( "num_sections", Section.FromXML );

			XmlElement groupsElement = el[ "groups" ];
			album.Groups = groupsElement.ReadList<Group>( "num_groups", Group.FromXML );

			return album;
		}

		public bool IsPrivate { get { return this.Groups.Length == 0; } }

		public bool IsSharedWith( long groupId )
		{
			return this.Groups.Any<Group>( ( group ) => ( group.GroupID == groupId ) );
		}
	}
}
