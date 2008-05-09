using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public enum NewsItemType
	{
		NewAlbum,
		NewMedia,
	}

	public class NewsItem
	{
		public NewsItemType Type { get; set; }
		public DateTime Timestamp { get; set; }
		public long SourceID { get; set; }
		public long AlbumID { get; set; }
		public string AlbumName { get; set; }
		public string AlbumDate { get; set; }

		public ImageInfo[] LeadImages { get; set; }

		public static NewsItem FromXML( XmlElement el )
		{
			NewsItem item = new NewsItem();
			item.Type = el.GetAttributeEnum<NewsItemType>( "type" );
			item.Timestamp = el.GetAttributeDate( "timestamp" );
			item.SourceID = el.GetAttributeInt64( "source_id" );
			item.AlbumID = el.GetAttributeInt64( "album_id" );
			item.AlbumName = el.GetAttribute( "album_name" );
			item.AlbumDate = el.GetAttribute( "album_date" );

			XmlElement imagesElement = el[ "images" ];
			item.LeadImages = imagesElement.ReadList<ImageInfo>( "num_images", ImageInfo.FromXML );

			return item;
		}
	}
}
