using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Phanfare.MediaServer.Connectors.PhanfareConnector
{
	enum ImageRenditionType
	{
		ThumbnailSmall = 0,
		Thumbnail = 1,
		Web = 2,
		WebSmall = 3,
		WebLarge = 4,
		Full = 5,
		FullFlv = 6,
		Original = 7,
	}

	class ImageCache
	{
		public long TotalSize
		{
			get
			{
				return 0;
			}
		}

		public bool Contains( long imageId, ImageRenditionType renditionType )
		{
			return true;
		}

		public Stream BeginRead( long imageId, ImageRenditionType renditionType )
		{
			return null;
		}

		public void Write( long imageId, ImageRenditionType renditionType, Stream stream )
		{
		}

		public void Delete( long imageId )
		{
		}
	}
}
