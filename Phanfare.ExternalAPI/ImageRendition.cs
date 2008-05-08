using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public class ImageRendition
	{
		public string Type { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public long FileSize { get; set; }
		public DateTime CreatedDate { get; set; }
		public string MediaType { get; set; }
		public int Quality { get; set; }
		public string URL { get; set; }

		public static ImageRendition FromXML( XmlElement el )
		{
			ImageRendition rendition = new ImageRendition();
			rendition.Type = el.GetAttribute( "rendition_type" );
			rendition.Width = el.GetAttributeInt32( "width" );
			rendition.Height = el.GetAttributeInt32( "height" );
			rendition.FileSize = el.GetAttributeInt64( "filesize" );
			rendition.CreatedDate = el.GetAttributeDate( "created_date" );
			rendition.MediaType = el.GetAttribute( "media_type" );
			rendition.Quality = el.GetAttributeInt32( "quality" );
			rendition.URL = el.GetAttribute( "url" );
			return rendition;
		}

	}
}
