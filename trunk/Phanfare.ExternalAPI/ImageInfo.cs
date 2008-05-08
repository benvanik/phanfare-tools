using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public class ImageInfo
	{
		public long AlbumID { get; set; }
		public long SectionID { get; set; }
		public long ImageID { get; set; }
		public string FileName { get; set; }
		public string Caption { get; set; }
		public DateTime? ImageDate { get; set; }
		public bool IsVideo { get; set; }
		public bool IsHidden { get; set; }
		public ImageRendition[] Renditions { get; set; }

		public static ImageInfo FromXML( XmlElement el )
		{
			ImageInfo image = new ImageInfo();
			image.AlbumID = el.GetAttributeInt64( "album_id" );
			image.SectionID = el.GetAttributeInt64( "section_id" );
			image.ImageID = el.GetAttributeInt64( "image_id" );
			image.FileName = el.GetAttribute( "filename" );
			image.Caption = el.GetAttribute( "caption" );
			string imageDate = el.GetAttribute( "image_date" );
			image.ImageDate = string.IsNullOrEmpty( imageDate ) ? null : ( DateTime? )DateTime.Parse( imageDate );
			image.IsVideo = el.GetAttributeBoolean( "is_video" );
			image.IsHidden = el.GetAttributeBoolean( "hidden" );

			XmlElement renditionsElement = el[ "renditions" ];
			image.Renditions = renditionsElement.ReadList<ImageRendition>( "num_renditions", ImageRendition.FromXML );

			return image;
		}
	}
}
