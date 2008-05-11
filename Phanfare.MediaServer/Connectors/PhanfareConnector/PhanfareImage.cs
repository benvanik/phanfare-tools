using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Phanfare.ExternalAPI;

namespace Phanfare.MediaServer.Connectors.PhanfareConnector
{
	class PhanfareImage : ContentBase
	{
		public PhanfareSection ParentSection { get { return ( PhanfareSection )this.Parent; } }
		public readonly ImageInfo Object;
		public readonly ImageRendition LargeRendition;
		public readonly ImageRendition MediumRendition;
		public readonly ImageRendition SmallRendition;

		public PhanfareImage( PhanfareSection section, ImageInfo source )
			: base( section )
		{
			this.Object = source;
			this.LargeRendition = this.FindRendition( 1400, 1400 );
			this.MediumRendition = this.FindRendition( 700, 700 );
			this.SmallRendition = this.FindRendition( 119, 119 );
		}

		public PhanfareImage( PhanfareRandomSet set, ImageInfo source )
			: base( set )
		{
			this.Object = source;
			this.LargeRendition = this.FindRendition( 1400, 1400 );
			this.MediumRendition = this.FindRendition( 700, 700 );
			this.SmallRendition = this.FindRendition( 119, 119 );
		}

		public bool IsValid
		{
			get
			{
				return ( this.Object.IsVideo == false ) && ( this.LargeRendition != null );
			}
		}

		public ImageRendition FindRendition( int desiredWidth, int desiredHeight )
		{
			ImageRendition current = null;
			foreach( ImageRendition rendition in this.Object.Renditions )
			{
				// Can handle more than just jpegs?
				if( rendition.MediaType != MediaType.Jpeg )
					continue;
				if( current == null )
					current = rendition;
				else if( ( rendition.Width > rendition.Height ) && ( Math.Abs( desiredWidth - current.Width ) > Math.Abs( desiredWidth - rendition.Width ) ) )
					current = rendition;
				else if( Math.Abs( desiredHeight - current.Height ) > Math.Abs( desiredHeight - rendition.Height ) )
					current = rendition;
			}
			return current;
		}

		private static string MediaTypeToMimeType( MediaType mediaType )
		{
			switch( mediaType )
			{
				case MediaType.Bmp:
					return "image/bmp";
				case MediaType.Gif:
					return "image/gif";
				case MediaType.Jpeg:
					return "image/jpeg";
				case MediaType.Psd:
					return "image/psd";
				case MediaType.Tga:
					return "image/tga";
				case MediaType.Tif:
					return "image/tiff";
				case MediaType.Threegpp:
					return "video/3gpp";
				case MediaType.Threegpp2:
					return "video/3gpp2";
				case MediaType.Asf:
					return "video/x-ms-asf";
				case MediaType.Avi:
					return "video/x-msvideo";
				case MediaType.Divx:
					return "video/x-divx";
				case MediaType.Flv:
					return "video/x-flv";
				case MediaType.Matroska:
					return "video/x-matroska";
				case MediaType.Mov:
					return "video/quicktime";
				case MediaType.Mp4:
					return "video/mp4";
				case MediaType.Ogg:
					return "application/ogg";
				case MediaType.Wmv:
					return "video/x-ms-wmv";
				case MediaType.Mpg:
					return "video/mpeg";
				case MediaType.None:
				default:
					return "application/octet-stream";
			}
		}

		public override string GetDescription()
		{
			string ext = Path.GetExtension( this.Object.FileName ).ToLowerInvariant();

			StringBuilder sb = new StringBuilder();
			sb.AppendFormat( "<item id=\"{0}\" parentID=\"{1}\" restricted=\"true\">", this.ID, this.Parent.ID );
			sb.Append( "<dc:title>" + HttpUtility.HtmlEncode( this.Object.Caption ) + "</dc:title>" );
			if( this.Object.ImageDate != null )
				sb.Append( "<dc:date>" + this.Object.ImageDate.Value.ToString( "s" ) + "</dc:date>" );
			sb.Append( this.GetRenditionResource( this.LargeRendition, ext ) );
			sb.Append( this.GetRenditionResource( this.MediumRendition, ext ) );
			sb.Append( this.GetRenditionResource( this.SmallRendition, ext ) );
			sb.Append( "<upnp:class>object.item.imageItem.photo</upnp:class></item>" );
			return sb.ToString();
		}

		private string GetRenditionResource( ImageRendition rendition, string ext )
		{
			string jpegType;
			if( rendition.Width > 1300 )
				jpegType = "JPEG_LRG";
			else if( rendition.Width >= 700 )
				jpegType = "JPEG_MED";
			else
				jpegType = "JPEG_TN";
			//DLNA_ORG_FLAG_STREAMING_TRANSFER_MODE    = (1 << 24),
			//DLNA_ORG_FLAG_BACKGROUND_TRANSFERT_MODE  = (1 << 22),
			//DLNA_ORG_FLAG_CONNECTION_STALL           = (1 << 21),
			//DLNA_ORG_FLAG_DLNA_V15                   = (1 << 20),
			string protocolInfo = "http-get:*:" + MediaTypeToMimeType( rendition.MediaType ) + ":DLNA.ORG_PN=" + jpegType + ";DLNA.ORG_OP=01;DLNA.ORG_PS=1;DLNA.ORG_CI=0;DLNA.ORG_FLAGS=00b80000000000000000000000000000";
			string resolution = rendition.Width + "x" + rendition.Height;
			string url = string.Format( this.Parent.BaseURL + "/{0}/{1}.{2}{3}", PhanfareSystem.PhotoHandler, this.ID, rendition.Type, ext );
			return string.Format( "<res protocolInfo=\"{0}\" resolution=\"{1}\" size=\"{2}\">{3}</res>", protocolInfo, resolution, rendition.FileSize, HttpUtility.HtmlEncode( url ) );
		}
	}
}
