using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public class Section
	{
		public long AlbumID { get; set; }
		public long SectionID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int ImageCount { get; set; }
		public ImageInfo[] Images { get; set; }

		public static Section FromXML( XmlElement el )
		{
			Section section = new Section();
			section.AlbumID = el.GetAttributeInt64( "album_id" );
			section.SectionID = el.GetAttributeInt64( "section_id" );
			section.Name = el.GetAttribute( "section_name" );
			section.Description = el.GetAttribute( "section_descr" );

			XmlElement imagesElement = el[ "images" ];
			int actualCount;
			section.Images = imagesElement.ReadList<ImageInfo>( "num_images", ImageInfo.FromXML, out actualCount );
			section.ImageCount = actualCount;

			return section;
		}
	}
}
