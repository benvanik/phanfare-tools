using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;
using System.Collections;

namespace Phanfare.ExternalAPI
{
	public partial class PhanfareService
	{
		public ImageInfo NewImage( long userId, long albumId, long sectionId, ImageInfo image )
		{
			return this.NewImage( userId, albumId, sectionId, image, false, false );
		}

		public ImageInfo NewImage( long userId, long albumId, long sectionId, ImageInfo image, bool externalLinks )
		{
			return this.NewImage( userId, albumId, sectionId, image, false, externalLinks );
		}

		public ImageInfo NewImage( long userId, long albumId, long sectionId, ImageInfo image, bool setAsProfilePicture, bool externalLinks )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterValidID( "sectionId", sectionId );
			this.AssertParameterNotNull( "image", image );

			Hashtable ht = this.MethodCall( "newimage" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "section_id" ] = sectionId;

			if( ( image.FileName == null ) || ( File.Exists( image.FileName ) == false ) )
				throw new PhanfareException( string.Format( "{0} is not a valid filename", image.FileName ?? "(null)" ) );

			ht[ "filename" ] = image.FileName;
			if( image.Caption != null )
				ht[ "caption" ] = image.Caption;
			if( image.ImageDate != null )
				ht[ "image_date" ] = image.ImageDate.Value.ToUniversalTime();
			ht[ "is_video" ] = image.IsVideo;
			ht[ "hidden" ] = image.IsHidden;
			ht[ "set_as_profile" ] = setAsProfilePicture;
			ht[ "external_links" ] = externalLinks;

			long length = new FileInfo( image.FileName ).Length;
			if( length == 0 )
				throw new PhanfareException( string.Format( "{0} has length 0", image.FileName ) );

			using( FileStream stream = new FileStream( image.FileName, FileMode.Open ) )
			{
				XmlDocument doc = this.MakeRequest( ht, stream, length );
				return this.ReadResponseItem<ImageInfo>( doc, ImageInfo.FromXML );
			}
		}

		public byte[] GetImageData( ImageRendition rendition )
		{
			byte[] buffer = null;
			this.GetImageData( rendition, ref buffer );
			return buffer;
		}

		public int GetImageData( ImageRendition rendition, ref byte[] buffer )
		{
			this.AssertSessionValid();

			HttpWebRequest request = ( WebRequest.Create( rendition.URL ) as HttpWebRequest );
			request.KeepAlive = true;
			request.UserAgent = "Phanfare API v2";
			request.Timeout = DataTimeout;
			request.Method = "GET";
			request.AutomaticDecompression = DecompressionMethods.GZip;
			request.CookieContainer = new CookieContainer( 1 );
			request.CookieContainer.Add( this.GetCookie() );

			int length = -1;
			using( WebResponse response = request.GetResponse() )
			using( Stream stream = response.GetResponseStream() )
			{
				length = ( int )response.ContentLength;
				if( buffer == null )
					buffer = new byte[ length ];
				else if( buffer.Length < length )
					buffer = new byte[ length ];

				int offset = 0;
				int timeout = 0;
				while( offset < buffer.Length )
				{
					int read = stream.Read( buffer, offset, Math.Min( 32 * 1024, buffer.Length - offset ) );
					offset += read;

					if( read == 0 )
					{
						timeout++;
						if( timeout == 5 )
							break;
					}
				}
			}
			return length;
		}
	}
}
